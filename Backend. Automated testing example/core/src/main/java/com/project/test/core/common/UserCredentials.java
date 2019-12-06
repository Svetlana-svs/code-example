package project.test.core.common;


public class UserCredentials {
    private final static String PROPERTY_NAME_USER_LOGIN = "user.login";
    private final static String PROPERTY_NAME_USER_PASSWORD = "user.password";

    public UserCredentials() {

    }

    public static String getLogin() {
        return Configuration.getProperty(PROPERTY_NAME_USER_LOGIN);
    }

    public static String getLogin(String propertyLogin) {
        return (propertyLogin == null || propertyLogin.isEmpty()) ? getPassword() : Configuration.getProperty(propertyLogin);
    }

    public static String getPassword() {
        return Configuration.getProperty(PROPERTY_NAME_USER_PASSWORD);
    }

    public static String getPassword(String propertyPassword) {
        return (propertyPassword == null || propertyPassword.isEmpty()) ? getPassword() : Configuration.getProperty(propertyPassword);
    }
}
