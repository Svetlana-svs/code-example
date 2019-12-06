package com.softwerke.com.components.user.password.servlet;

import com.softwerke.com.components.user.password.dto.UserPasswordChangeData;
import com.softwerke.com.components.user.password.dto.UserPasswordChangeAuthResult;
import com.softwerke.com.components.user.password.dto.UserPasswordChangeResult;
import com.softwerke.com.core.config.SiteConfigurationService;
import com.softwerke.com.core.cookie.CookieController;
import com.softwerke.com.core.cookie.UserSessionCookie;
import com.softwerke.com.core.exception.BadCookieException;
import com.softwerke.com.core.exception.NoSuchBrandException;
import com.softwerke.com.core.exception.ServiceNotConfigured;
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
import java.util.*;


@Component(
        service = Servlet.class,
        property = {
                "sling.servlet.methods=" + HttpConstants.METHOD_POST,
                "sling.servlet.paths=" + "/bin/userprofile/passwordChange"
        })
public class UserPasswordChangeServlet extends AbstractServlet {

    private static final Logger logger = LoggerFactory.getLogger(UserPasswordChangeServlet.class);

    private static final String REQUEST_PARAMETER_EMAIL = "email";
    private static final String REQUEST_PARAMETER_TOKEN = "token";

    private static final String REQUEST_PARAMETER_PASSWORD_OLD = "passwordOld";
    private static final String REQUEST_PARAMETER_PASSWORD_NEW = "passwordNew";

    @Reference
    private UserRepository userRepository;

    @Reference
    private SiteConfigurationService siteConfigurationService;

    @Reference
    private CookieController cookieController;

    private String brandId;

    @Override
    protected JsonElement execute(final SlingHttpServletRequest slingRequest, final SlingHttpServletResponse slingResponse)
            throws ServiceNotConfigured, NoSuchBrandException, BadCookieException {
        if (userRepository == null) {
            throw new ServiceNotConfigured(UserRepository.class.getName() + " is not available");
        }

        brandId = this.getBrandId(slingRequest);
        UserPasswordChangeData userData = null;

        try {
            if (cookieController.isUserLoggedIn(slingRequest, brandId)) {
                // Get authenticated user data by change password from user profile
                userData = getAuthUserData(slingRequest);
            } else {
                // Get not authenticated user data by reset password with reset password link
                userData = getNotAuthUserData(slingRequest);
            }
        } catch (ApplicationPersistenceException e) {
            logger.error("Database connection error. Please, check database connection and data integrity.", e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        } catch (NoSuchEntityException e) {
            logger.warn("Bad request. There is no entity with such session data {}.", userData.getId());
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        } catch (TooManyEntitiesException e) {
            logger.warn("Bad request. There are too many entities with such session data {}.", userData.getId());
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        } catch (IllegalArgumentException e) {
            logger.warn("Bad request. Request parameters are not valid. ", e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_BAD_REQUEST);
        }

        try {
            // Store new password in the database
            userRepository.update(getArguments(userData), getAuthConditions(userData));
        } catch (ApplicationPersistenceException e) {
            logger.error("Database connection error. Please, check database connection and data integrity.", e);
            return this.buildBasicResponse(false, HttpServletResponse.SC_INTERNAL_SERVER_ERROR);
        }

        return this.buildBasicResponse(true, HttpServletResponse.SC_OK);
    }

    private UserPasswordChangeData getAuthUserData(final SlingHttpServletRequest slingRequest)
            throws ApplicationPersistenceException, NoSuchEntityException, TooManyEntitiesException, BadCookieException, IllegalArgumentException {
        // Get user information from session cookie
        final UserSessionCookie userSessionCookie = cookieController.getUserSessionCookie(slingRequest, brandId);
        if (userSessionCookie == null) {
            throw new BadCookieException("User session cookies is not available.");
        }
        // Get request data and validate
        UserPasswordChangeData userData = getUserData(slingRequest, userSessionCookie);
        if (!validateUserData(userData)) {
            throw new IllegalArgumentException("UserPasswordChange validation failed. Some request parameter is not valid.");
        }

        // Get user data from the database
        UserPasswordChangeAuthResult userAuthResult = userRepository.fetch(UserPasswordChangeAuthResult.class, getAuthConditions(userData));

        // Check compare old password from request with password stored in the database
        if (!validateUserPassword(userData, userAuthResult)) {
            throw new IllegalArgumentException("UserPasswordChange validation failed. Database result parameter is not valid.");
        }

        return userData;
    }

    private UserPasswordChangeData getNotAuthUserData(final SlingHttpServletRequest slingRequest)
            throws ApplicationPersistenceException, NoSuchEntityException, TooManyEntitiesException {
        // Get request data and validate
        UserPasswordChangeData userData = getUserData(slingRequest);
        if (!validateUserData(userData)) {
            throw new IllegalArgumentException("UserPasswordChange validation failed. Some request parameter is not valid.");
        }

        // Get user data from the database
        UserPasswordChangeResult userResult = userRepository.fetch(UserPasswordChangeResult.class, getConditions(userData));
        // Check token
        if (!validateUserResult(userData, userResult)) {
            throw new IllegalArgumentException("UserPasswordChange validation failed. Database result parameter is not valid.");
        }
        userData.setId(userResult.getId());

        return userData;
    }

    /*
     * Set user data with data from request in the case of password change by authenticated user
     *
     */
    private UserPasswordChangeData getUserData(SlingHttpServletRequest slingRequest, UserSessionCookie userSessionCookie) {

        String passwordOld = RequestUtil.getParameter(slingRequest, REQUEST_PARAMETER_PASSWORD_OLD);
        String passwordNew = RequestUtil.getParameter(slingRequest, REQUEST_PARAMETER_PASSWORD_NEW);

        UserPasswordChangeData userData = new UserPasswordChangeData();
        userData.setPasswordOld(passwordOld);
        userData.setPasswordNew(passwordNew);
        userData.setId(Long.parseLong(userSessionCookie.getUserId()));

        return userData;
    }

    /*
     * Set user data with data from request in the case of password reset
     *
     */
    private UserPasswordChangeData getUserData(SlingHttpServletRequest slingRequest) {

        final String email = RequestUtil.getParameter(slingRequest, REQUEST_PARAMETER_EMAIL);
        final String token = RequestUtil.getParameter(slingRequest, REQUEST_PARAMETER_TOKEN);
        String passwordNew = RequestUtil.getParameter(slingRequest, REQUEST_PARAMETER_PASSWORD_NEW);

        UserPasswordChangeData userData = new UserPasswordChangeData();
        userData.setEmail(email);
        userData.setToken(token);
        userData.setPasswordNew(passwordNew);

        return userData;
    }

    private boolean validateUserData(UserPasswordChangeData userData) {
        if (!StringUtils.isEmpty(userData.getToken())) {
            // Check email field in the case of the password reset
            if (!UserCredentialsData.validateEmail(userData.getEmail())) {
                return false;
            }
        }
        // Passwords fields

        return true;
    }

    private boolean validateUserPassword(UserPasswordChangeData userData, UserPasswordChangeAuthResult userResult) {
        return UserUtil.checkSecret(userData.getPasswordOld(), userResult.getPassword());
    }

    private boolean validateUserResult(UserPasswordChangeData userData, UserPasswordChangeResult userResult) {
        DateTimeFormatter formatter = DateTimeFormatter.ofPattern(UserUtil.SECRET_DATE_FORMAT);
        LocalDateTime passwordResetDate = LocalDateTime.parse(userResult.getPasswordResetDate().format(formatter), formatter);

        // Generate token with data stored in the database
        final String secret = String.format(UserUtil.SECRET_FORMAT,
                String.valueOf(Timestamp.valueOf(passwordResetDate).getTime()),
                brandId, userResult.getEmail());

        return isPasswordResetDateValid(userResult.getPasswordResetDate()) &&
                UserUtil.checkSecret(secret, userResult.getPasswordResetKey() + "$" + userData.getToken());
    }

    private boolean isPasswordResetDateValid(LocalDateTime passwordResetDate) {
        long userPasswordResetExpTime = siteConfigurationService.getSiteConfiguration(brandId).getUserPasswordResetExpirationTime();
        return (passwordResetDate == null)  ||
                Duration.between(passwordResetDate, LocalDateTime.now()).getSeconds() < Duration.ofSeconds(userPasswordResetExpTime).getSeconds();

    }

    private Map<String, Object> getArguments(UserPasswordChangeData userData) {
        Map<String, Object> userPasswordChangeArguments = new HashMap<String, Object>();
        String passwordHash = UserUtil.getSecretHash(userData.getPasswordNew());
        userPasswordChangeArguments.put("password", passwordHash);

        return userPasswordChangeArguments;
    }

    private List<Condition> getConditions(UserPasswordChangeData userData) {
        List<Condition> conditions = new ArrayList<>();
        conditions.add(new Condition("email", userData.getEmail()));
        conditions.add(new Condition("brandId", brandId));

        return conditions;
    }

    private List<Condition> getAuthConditions(UserPasswordChangeData userData) {
        List<Condition> conditions = new ArrayList<>();
        conditions.add(new Condition("id", userData.getId()));

        return conditions;
    }
}
