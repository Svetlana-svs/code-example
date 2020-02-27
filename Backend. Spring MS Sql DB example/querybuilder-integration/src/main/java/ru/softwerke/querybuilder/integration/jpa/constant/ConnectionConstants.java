package ru.softwerke.querybuilder.integration.jpa.constant;

/*
 *  @author Svetlana Suvorova
 *
 *  Enum which declares the constant used across the database configuration setting.
 *
 */
public  class ConnectionConstants {

    // MS SQL Server common constants

    /*
     * Application database management system SQL Server.
     */
    public static final String MSSQL = "MS SQL-Server";

    /*
     * MS SQL Driver name.
     */
    public static final String MSSQL_DRIVER_NAME = "com.microsoft.sqlserver.jdbc.SQLServerDriver";


    // Gcf file constants

    /*
     * Starting point connections configuration file name.
     */
    public static final String CONFIGURATION_FILE_NAME_GCF = "DUVA4.gcf";

    /*
     * Starting point connections configuration file name.
     */
    public static final String CONFIGURATION_FILE_NAME_INI = "DUVA4";

    /*
     * Gcf file required section.
     */
    public static final String CONFIGURATION_GSF_SECTION_NETWORK = "NETWORK";

    /*
     * Constant of the NETWORK section set ini file path.
     */
    public static final String CONFIGURATION_GSF_NETWORK_FILE_PATH_INI = "DUVANetDir";

    /*
     * Gcf file optional section.
     */
    public static final String CONFIGURATION_GSF_SECTION_REGISTRY = "Registry";

    /*
     * Constant of the Registry section set ini file name (without "ini" extension).
     */
    public static final String CONFIGURATION_GCF_REGISTRY_FILE_NAME_INI = "Profile";

    /*
     * Constant of the Registry section set default meta database MDB alias.
     */
    public static final String CONFIGURATION_GCF_REGISTRY_MDB_ALIAS = "Alias";


    // Ini file constants.

    /*
     * Ini file required section.
     */
    public static final String CONFIGURATION_INI_SECTION_CONNECTIONS = "Connections";

    /*
     * Constant of the Connections section set of the ConnectionX sections number.
     */
    public static final String CONFIGURATION_INI_CONNECTIONS_NUMBER = "Anzahl";

    /*
     * Ini file required section for each connection specification.
     */
    public static final String CONFIGURATION_INI_SECTION_CONNECTION = "Connection";

    /*
     * Constant of the Connection section set data source SQL System.
     */
    public static final String CONFIGURATION_INI_CONNECTION_DBSYSTEM = "DBSystem";

    /*
     * Constant of the Connection section set database alias.
     */
    public static final String CONFIGURATION_INI_CONNECTION_ALIAS = "Alias";

    /*
     * Constant of the Connection section set database type (MDB - meta database or SDB - database with data).
     */
    public static final String CONFIGURATION_INI_CONNECTION_DBUSAGE = "DBUsage";

    /*
     * Constant of the Connection section set connection configuration properties.
     */
    public static final String CONFIGURATION_INI_CONNECTION_CONNECTION_STRING = "ConnectionString";

    /*
     * Constant of the Connection section set connection configuration property "User ID".
     */
    public static final String CONFIGURATION_INI_CONNECTION_CONNECTION_STRING_USERNAME = "User ID";

    /*
     * Constant of the Connection section set connection configuration property "Password".
     */
    public static final String CONFIGURATION_INI_CONNECTION_CONNECTION_STRING_PASSWORD = "Password";

    /*
     * Constant of the Connection section set connection configuration property "Database name".
     */
    public static final String CONFIGURATION_INI_CONNECTION_CONNECTION_STRING_INITIAL_CATALOG = "Initial Catalog=";
}