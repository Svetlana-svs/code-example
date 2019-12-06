package com.softwerke.com.components.user.profile.servlet;

import com.softwerke.com.components.user.profile.dto.UserProfileResult;
import com.softwerke.com.core.cookie.CookieController;
import com.softwerke.com.core.cookie.UserSessionCookie;
import com.softwerke.com.core.exception.BadCookieException;
import com.softwerke.com.core.exception.NoSuchBrandException;
import com.softwerke.com.core.exception.ServiceNotConfigured;
import com.softwerke.com.core.servlet.AbstractServlet;
import com.softwerke.com.core.userdata.UserSessionModel;
import com.softwerke.com.integration.jpa.core.exception.ApplicationPersistenceException;
import com.softwerke.com.integration.jpa.core.exception.NoSuchEntityException;
import com.softwerke.com.integration.jpa.core.exception.TooManyEntitiesException;
import com.softwerke.com.integration.jpa.core.repository.Condition;
import com.softwerke.com.integration.jpa.user.repository.UserRepository;
import com.google.gson.*;

import org.apache.commons.lang3.StringUtils;
import org.apache.sling.api.SlingHttpServletRequest;
import org.apache.sling.api.SlingHttpServletResponse;
import org.apache.sling.api.servlets.HttpConstants;
import org.osgi.service.component.annotations.Component;
import org.osgi.service.component.annotations.Reference;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import javax.servlet.Servlet;
import javax.servlet.http.HttpServletResponse;
import java.util.ArrayList;
import java.util.List;


@Component(
        service = Servlet.class,
        property = {
                "sling.servlet.methods=" + HttpConstants.METHOD_GET,
                "sling.servlet.paths=" + "/bin/userprofile/getUserProfile"
        })
public class UserProfileServlet extends AbstractServlet {

    private static final Logger logger = LoggerFactory.getLogger(UserProfileServlet.class);

    @Reference
    private CookieController cookieController;

    @Reference
    private UserRepository userRepository;

    private String brandId;
    @Override
    protected JsonElement execute(SlingHttpServletRequest slingRequest, SlingHttpServletResponse slingResponse)
            throws ServiceNotConfigured, NoSuchBrandException, BadCookieException {

        if (userRepository == null) {
            throw new ServiceNotConfigured(UserRepository.class.getName() + " is not available");
        }

        brandId = this.getBrandId(slingRequest);

        if (!cookieController.isUserLoggedIn(slingRequest, brandId)) {
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        }

        // Get user information from session cookie
        final UserSessionCookie userSessionCookie = cookieController.getUserSessionCookie(slingRequest, brandId);
        if (userSessionCookie == null) {
            throw new BadCookieException("User session cookies is not available.");
        }
        UserSessionModel userSessionData = new UserSessionModel(userSessionCookie);

        // Get user data from the database
        UserProfileResult userProfileResult = null;
        try {
            userProfileResult = userRepository.fetch(UserProfileResult.class, getConditions(userSessionData));
        } catch (ApplicationPersistenceException e) {
            logger.error("Database connection error. Please, check database connection and data integrity.", e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        } catch (NoSuchEntityException e) {
            logger.warn("Bad request. There is no entity with such session data {}.", userSessionData.getUserId());
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        } catch (TooManyEntitiesException e) {
            logger.warn("Bad request. There are too many entities with such session data {}.", userSessionData.getUserId());
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        }

        JsonObject jsonResponse = this.buildBasicResponse(true, HttpServletResponse.SC_OK);
        jsonResponse.addProperty("userData", userProfileResult.toJson());

        return jsonResponse;
    }

    private List<Condition> getConditions(UserSessionModel userSessionData) {
        List<Condition> conditions = new ArrayList<>();
        conditions.add(new Condition("id", userSessionData.getUserId()));

        return conditions;
    }
}
