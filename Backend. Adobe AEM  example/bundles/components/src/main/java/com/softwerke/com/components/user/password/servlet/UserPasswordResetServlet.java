package com.softwerke.com.components.user.password.servlet;

import com.softwerke.com.components.user.password.dto.UserPasswordResetData;
import com.softwerke.com.components.user.password.dto.UserPasswordChangeResult;
import com.softwerke.com.components.user.registration.exception.VerificationEmailException;
import com.softwerke.com.components.user.registration.service.EmailVerificationService;
import com.softwerke.com.core.analytics.AnalyticsEventController;
import com.softwerke.com.core.config.SiteConfigurationService;
import com.softwerke.com.core.exception.NoSuchBrandException;
import com.softwerke.com.core.exception.ServiceNotConfigured;
import com.softwerke.com.core.service.PromoNodesFinder;
import com.softwerke.com.core.servlet.AbstractServlet;
import com.softwerke.com.core.userdata.UserCredentialsData;
import com.softwerke.com.core.util.RequestUtil;
import com.softwerke.com.core.util.UserUtil;
import com.softwerke.com.integration.jpa.core.exception.ApplicationPersistenceException;
import com.softwerke.com.integration.jpa.core.exception.NoSuchEntityException;
import com.softwerke.com.integration.jpa.core.exception.TooManyEntitiesException;
import com.softwerke.com.integration.jpa.core.repository.Condition;
import com.softwerke.com.integration.jpa.user.repository.UserRepository;
import com.google.gson.JsonElement;
import org.apache.commons.lang3.StringUtils;
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
import java.sql.Timestamp;
import java.time.Duration;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;


@Component(
        service = Servlet.class,
        property = {
                "sling.servlet.methods=" + HttpConstants.METHOD_POST,
                "sling.servlet.paths=" + "/bin/userprofile/passwordReset"
        })
public class UserPasswordResetServlet extends AbstractServlet {

    private static final Logger logger = LoggerFactory.getLogger(UserPasswordResetServlet.class);

    private static final String REQUEST_PARAMETER_EMAIL = "email";
    private static final String REQUEST_PARAMETER_RESOURCE_PATH = "resourcePath";

    @Reference
    private UserRepository userRepository;

    @Reference
    private SiteConfigurationService siteConfigurationService;

    @Reference
    private EmailVerificationService emailVerificationService;

    @Reference
    private PromoNodesFinder promoNodesFinder;

    @Reference
    private AnalyticsEventController analyticsEventController;

    private String brandId;

    @Override
    protected JsonElement execute(final SlingHttpServletRequest slingRequest, final SlingHttpServletResponse slingResponse)
            throws ServiceNotConfigured, NoSuchBrandException {
        if (userRepository == null) {
            throw new ServiceNotConfigured(UserRepository.class.getName() + " is not available");
        }
        if (siteConfigurationService == null) {
            throw new ServiceNotConfigured(SiteConfigurationService.class.getName() + " is not available");
        }
        if (emailVerificationService == null) {
            throw new ServiceNotConfigured(EmailVerificationService.class.getName() + " is not available");
        }
        if (promoNodesFinder == null) {
            throw new ServiceNotConfigured(PromoNodesFinder.class.getName() + " is not available");
        }

        brandId = this.getBrandId(slingRequest);

        // Get form data and validate it
        UserPasswordResetData userData = getUserData(slingRequest);
        if (!validateUserData(userData)) {
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        }

        // Get user data from the database
        UserPasswordChangeResult userPasswordChangeResult = null;
        try {
            userPasswordChangeResult = userRepository.fetch(UserPasswordChangeResult.class, getConditions(userData));
        } catch (ApplicationPersistenceException e) {
            logger.error("Database connection error. Please, check database connection and data integrity. ", e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        } catch (NoSuchEntityException e) {
            logger.warn("Bad request. There is no entity with such session data {}.", userData.getEmail());
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        } catch (TooManyEntitiesException e) {
            logger.warn("Bad request. There are too many entities with such session data {}.", userData.getEmail());
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        }

        // Get component settings
        String resourcePath = slingRequest.getParameter(REQUEST_PARAMETER_RESOURCE_PATH);
        ValueMap componentSettings;
        try {
            componentSettings = promoNodesFinder.getResourceValueMap(resourcePath);
        } catch (ResourceNotFoundException e) {
            logger.error("Unable to get UserPasswordReset component settings. Component resource path: {}", resourcePath, e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        }

        Map<String, Object> updateArguments = new HashMap<>();
        try {
            updateUserData(userData, userPasswordChangeResult, updateArguments);
        } catch (IllegalAccessException e) {
            logger.error("User password reset is not accessible. Too many attempts in a time period. ", e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        }

        try {
            emailVerificationService.sendUserPasswordResetEmail(componentSettings, userData, brandId);
        } catch (VerificationEmailException e) {
            logger.error("Unable to send verification email. ", e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        }

        try {
            userRepository.update(updateArguments, getUpdateConditions(userPasswordChangeResult));
        } catch (ApplicationPersistenceException e) {
            logger.error("Database connection error. Please, check database connection and data integrity. ", e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        }

        return this.buildBasicResponse(true, HttpServletResponse.SC_OK);
    }

    private UserPasswordResetData getUserData(SlingHttpServletRequest slingRequest) {

        String email = RequestUtil.getParameter(slingRequest, REQUEST_PARAMETER_EMAIL);

        UserPasswordResetData userData = new UserPasswordResetData();
        userData.setEmail(email);

        return userData;
    }

    private void updateUserData(UserPasswordResetData userData, UserPasswordChangeResult userResult, Map<String, Object> arguments)
            throws IllegalAccessException{

        DateTimeFormatter formatter = DateTimeFormatter.ofPattern(UserUtil.SECRET_DATE_FORMAT);
        LocalDateTime passwordResetDate = LocalDateTime.now();
        LocalDateTime passwordResetDateFormat = LocalDateTime.parse(passwordResetDate.format(formatter), formatter);

        if (!isPasswordResetDateValid(LocalDateTime.now(), userResult)) {
            throw new IllegalAccessException("User password reset is not accessible. Too many attempts in a time period.");
        }

        // Generate token for password reset and safe secret key of this token generation
        final String secret = String.format(UserUtil.SECRET_FORMAT,
                String.valueOf(Timestamp.valueOf(passwordResetDateFormat).getTime()), brandId, userData.getEmail());
        String[] secretData = UserUtil.getSecret(secret);

        arguments.put("passwordResetDate", passwordResetDateFormat);
        arguments.put("passwordResetKey", secretData[0]);

        userData.setPasswordResetKey(secretData[1]);
    }

    private boolean validateUserData(UserPasswordResetData userData) {
        return UserCredentialsData.validateEmail(userData.getEmail());
    }

    private boolean isPasswordResetDateValid(LocalDateTime passwordResetDate, UserPasswordChangeResult userPasswordResetResult) {
        long userPasswordResetExpTime = siteConfigurationService.getSiteConfiguration(brandId).getUserPasswordResetExpirationTime();

        return (userPasswordResetResult.getPasswordResetDate() == null)  ||
                (Duration.between(userPasswordResetResult.getPasswordResetDate(), passwordResetDate).getSeconds() > Duration.ofSeconds(userPasswordResetExpTime).getSeconds());
    }

    private List<Condition> getConditions(UserPasswordResetData userData) {
        List<Condition> conditions = new ArrayList<>();
        conditions.add(new Condition("email", userData.getEmail()));
        conditions.add(new Condition("brandId", brandId));

        return conditions;
    }

    private List<Condition> getUpdateConditions(UserPasswordChangeResult userResult) {
        List<Condition> conditions = new ArrayList<>();
        conditions.add(new Condition("id", userResult.getId()));

        return conditions;
    }
}
