package com.softwerke.com.components.user.profile.servlet;

import com.softwerke.com.components.user.profile.dto.UserProfileArgument;
import com.softwerke.com.core.cookie.CookieController;
import com.softwerke.com.core.cookie.UserSessionCookie;
import com.softwerke.com.core.exception.BadCookieException;
import com.softwerke.com.core.exception.NoSuchBrandException;
import com.softwerke.com.core.exception.ServiceNotConfigured;
import com.softwerke.com.core.service.PromoNodesFinder;
import com.softwerke.com.core.servlet.AbstractServlet;
import com.softwerke.com.core.util.RequestUtil;
import com.softwerke.com.integration.jpa.core.exception.ApplicationPersistenceException;
import com.softwerke.com.integration.jpa.core.repository.Condition;
import com.softwerke.com.integration.jpa.user.repository.UserRepository;
import com.google.gson.JsonElement;
import org.apache.sling.api.SlingHttpServletRequest;
import org.apache.sling.api.SlingHttpServletResponse;
import org.apache.sling.api.resource.ResourceNotFoundException;
import org.apache.sling.api.resource.ValueMap;
import org.apache.sling.api.servlets.HttpConstants;
import org.osgi.service.component.annotations.Component;
import org.osgi.service.component.annotations.Reference;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import javax.servlet.Servlet;
import javax.servlet.http.HttpServletResponse;
import java.util.*;


@Component(
        service = Servlet.class,
        property = {
                "sling.servlet.methods=" + HttpConstants.METHOD_POST,
                "sling.servlet.paths=" + "/bin/userprofile/updateUserProfile"
        })
public class UserProfileUpdateServlet extends AbstractServlet {

    private static final Logger logger = LoggerFactory.getLogger(UserProfileServlet.class);

    private static final String REQUEST_PARAMETER_RESOURCE_PATH = "resourcePath";
    private static final String[] requestParameters = {
            "firstName", "middleName", "lastName", "phone", "birthDate", "gender", "city", "address"
    };

    @Reference
    private UserRepository userRepository;

    @Reference
    private CookieController cookieController;

    @Reference
    private PromoNodesFinder promoNodesFinder;

    private String brandId;

    @Override
    protected JsonElement execute(SlingHttpServletRequest slingRequest, SlingHttpServletResponse slingResponse)
            throws ServiceNotConfigured, NoSuchBrandException, BadCookieException {

        if (userRepository == null) {
            throw new ServiceNotConfigured(UserRepository.class.getName() + " is not available.");
        }
        if (promoNodesFinder == null) {
            throw new ServiceNotConfigured(PromoNodesFinder.class.getName() + " is not available");
        }

        brandId = this.getBrandId(slingRequest);

        if (!cookieController.isUserLoggedIn(slingRequest, brandId)) {
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        }

        // Get user information from session cookie.
        final UserSessionCookie userSessionCookie = cookieController.getUserSessionCookie(slingRequest, brandId);
        if (userSessionCookie == null) {
            throw new BadCookieException("User session cookies is not available.");
        }

        // Update user data
        // Get form data and validate it based on component settings
         UserProfileArgument userData = getUserData(slingRequest);
        if (!validateUserData(slingRequest, userData)) {
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        }

        // Update user data inthe database
        try {
            userRepository.update(getArguments(userData), getConditions(userSessionCookie));
        } catch (ApplicationPersistenceException e) {
            logger.error("Database connection error. Please, check database connection and data integrity. ", e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        }

        return this.buildBasicResponse(true, HttpServletResponse.SC_OK);
    }

    private UserProfileArgument getUserData(SlingHttpServletRequest slingRequest) {
        // User personal fields
        Map<String, Object> fields = Arrays.stream(requestParameters)
                .collect(HashMap::new, (m, p) -> m.put(p, RequestUtil.getParameter(slingRequest, p)), HashMap::putAll);

        return new UserProfileArgument(fields);
    }

    private boolean validateUserData(SlingHttpServletRequest slingRequest, UserProfileArgument userData) {
        String resourcePath = slingRequest.getParameter(REQUEST_PARAMETER_RESOURCE_PATH);
        ValueMap componentSettings;
        try {
            // Get component settings
            componentSettings = promoNodesFinder.getResourceValueMap(resourcePath);
        } catch (ResourceNotFoundException e) {
            logger.error("Unable to get UserProfile component settings. Component resource path: {}", resourcePath, e);
            return false;
        }

        return userData.validateUserPersonalData(componentSettings);
    }

    private Map<String, Object> getArguments(UserProfileArgument userData) {
        return userData.toMap();
    }

    private List<Condition> getConditions( UserSessionCookie userSessionCookie) {
        List<Condition> conditions = new ArrayList<>();
        conditions.add(new Condition("id", userSessionCookie.getUserId()));

        return conditions;
    }
}
