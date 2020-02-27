package ru.softwerke.querybuilder.core.data.queryForm;

public class QueryFormViews {
    interface InternalViews {}
    interface UserViews {}
    interface AdminViews {}

    public static class Internal implements InternalViews {}
    public static class User implements UserViews {}
    public static class Admin implements  AdminViews {}
}