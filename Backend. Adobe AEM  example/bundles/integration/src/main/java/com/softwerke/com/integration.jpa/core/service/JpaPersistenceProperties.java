package com.softwerke.com.integration.jpa.core.service;

import com.google.common.collect.ImmutableMap;

import java.util.Map;


public class JpaPersistenceProperties {

    private static final String DIALECT             =   "hibernate.dialect";
    private static final String SHOW_SQL            =   "hibernate.show_sql";
    private static final String FORMAT_SQL          =   "hibernate.format_sql";

    private static final String JPA_JDBC_DRIVER     =   "hibernate.connection.driver_class";
    private static final String JPA_JDBC_URL        =   "hibernate.connection.url";
    private static final String JPA_JDBC_USERNAME   =   "hibernate.connection.username";
    private static final String JPA_JDBC_PASSWORD   =   "hibernate.connection.password";

    public static Map<String, Object> getProperties(JpaEntityManagerConfiguration configuration) {
        if (configuration == null) {
            return null;
        }
        return ImmutableMap.<String, Object>builder()
                .put(JPA_JDBC_DRIVER, configuration.hibernate_connection_driver__class())
                .put(JPA_JDBC_URL,configuration.hibernate_connection_url() )
                .put(DIALECT, configuration.hibernate_dialect())
                .put(SHOW_SQL, configuration.hibernate_show__sql())
                .put(FORMAT_SQL, configuration.hibernate_format__sql())
                .put(JPA_JDBC_USERNAME, configuration.hibernate_connection_username())
                .put(JPA_JDBC_PASSWORD, configuration.hibernate_connection_password())
                .build();
    }
}
