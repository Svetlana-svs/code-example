package ru.softwerke.querybuilder.integration.jpa.context;

import org.springframework.util.Assert;

/*
 *  @author Svetlana Suvorova
 */
public class ConnectionContextHolder {

    private static ThreadLocal<String> contextHolder = new ThreadLocal<>();

    public static void set(String connection) {
        Assert.notNull(connection, "ConnectionContextHolder error: connection name cannot be null.");
        contextHolder.set(connection);
    }

    public static String getConnection() {
        return contextHolder.get();
    }

    public static void clear() {
        contextHolder.remove();
    }
}
