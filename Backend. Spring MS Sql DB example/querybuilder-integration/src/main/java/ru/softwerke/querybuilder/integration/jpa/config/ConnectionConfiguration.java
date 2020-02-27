package ru.softwerke.querybuilder.integration.jpa.config;

import org.apache.commons.lang3.StringUtils;
import org.ini4j.Config;
import org.ini4j.Ini;
import org.ini4j.Profile;
import org.springframework.boot.context.properties.ConfigurationProperties;
import org.springframework.context.annotation.Configuration;
import org.springframework.util.ResourceUtils;
import org.springframework.validation.annotation.Validated;
import ru.softwerke.querybuilder.integration.jpa.constant.ConnectionConstants;
import ru.softwerke.querybuilder.integration.jpa.dto.Connection;
import ru.softwerke.querybuilder.integration.jpa.dto.ConnectionType;
import ru.softwerke.querybuilder.integration.jpa.dto.Credential;
import ru.softwerke.querybuilder.integration.jpa.repository.service.ConnectionService;

import javax.annotation.PostConstruct;
import javax.validation.constraints.NotNull;
import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

/*
 *  @author Svetlana Suvorova
 */
@Configuration
@Validated
@ConfigurationProperties(prefix = "app.db")
public class ConnectionConfiguration {

    @NotNull
    private List<ConnectionConfig> connection = new ArrayList<>();

    private static List<Connection> connections = new ArrayList<>();

    @PostConstruct
    public void setUp() {

        try {
            Config.getGlobal().setEscape(false);
            // Parsing gcf file
            File file = ResourceUtils.getFile(this.getClass().getResource(
                    String.format("/%s", ConnectionConstants.CONFIGURATION_FILE_NAME_GCF)));
            final Ini fileGcf = new Ini(file);
            Profile.Section sectionNetwork = fileGcf.get(ConnectionConstants.CONFIGURATION_GSF_SECTION_NETWORK);
            String fileIniPath = sectionNetwork.get(ConnectionConstants.CONFIGURATION_GSF_NETWORK_FILE_PATH_INI);
            Profile.Section sectionRegistry = fileGcf.get(ConnectionConstants.CONFIGURATION_GSF_SECTION_REGISTRY);
            String fileIniName = "";
            String connectionMdbDefault = "";

            if (sectionRegistry != null) {
               fileIniName  = sectionRegistry.get(ConnectionConstants.CONFIGURATION_GCF_REGISTRY_FILE_NAME_INI);
               connectionMdbDefault  = sectionRegistry.get(ConnectionConstants.CONFIGURATION_GCF_REGISTRY_MDB_ALIAS);
            }
            if (StringUtils.isBlank(fileIniName)) {
                fileIniName = ConnectionConstants.CONFIGURATION_FILE_NAME_INI;
            }

            final Ini fileIni = new Ini(new File(String.format("%s/%s.ini", fileIniPath, fileIniName)));

            // Parsing ini file
            Profile.Section sectionConnections = fileIni.get(ConnectionConstants.CONFIGURATION_INI_SECTION_CONNECTIONS);
            int connectionSize = Integer.min(Integer.valueOf(sectionConnections.get(ConnectionConstants.CONFIGURATION_INI_CONNECTIONS_NUMBER)),
                    connection.size());

            for (int i = 0; i < connectionSize; i++) {
                Profile.Section sectionConnection = fileIni.get(ConnectionConstants.CONFIGURATION_INI_SECTION_CONNECTION + String.valueOf(i + 1));
                String dbSystem = sectionConnection.get(ConnectionConstants.CONFIGURATION_INI_CONNECTION_DBSYSTEM);
                if (!dbSystem.equals(ConnectionConstants.MSSQL)) {
                    //  Ignore connections other then MS SQL-Server
                    continue;
                }
                String alias = sectionConnection.get(ConnectionConstants.CONFIGURATION_INI_CONNECTION_ALIAS);
                String dbUsage = sectionConnection.get(ConnectionConstants.CONFIGURATION_INI_CONNECTION_DBUSAGE);

                List<String> connectionStrings = Arrays.stream(
                        sectionConnection.get(ConnectionConstants.CONFIGURATION_INI_CONNECTION_CONNECTION_STRING).split(";"))
                        .collect(Collectors.toList());

                String userName = connectionStrings.stream()
                        .filter(s -> s.startsWith(ConnectionConstants.CONFIGURATION_INI_CONNECTION_CONNECTION_STRING_USERNAME))
                        .findFirst()
                        .get().substring(8);
                String password = connectionStrings.stream()
                        .filter(s -> s.startsWith(ConnectionConstants.CONFIGURATION_INI_CONNECTION_CONNECTION_STRING_PASSWORD))
                        .findFirst()
                        .get().substring(9);
                String decodePassword = ConnectionService.decryptString(password, fileIni.getConfig().getFileEncoding());

                String databaseName = connectionStrings.stream()
                        .filter(s -> s.startsWith(ConnectionConstants.CONFIGURATION_INI_CONNECTION_CONNECTION_STRING_INITIAL_CATALOG))
                        .findFirst()
                        .get().substring(16);

                connections.add(new Connection(alias,
                        databaseName,
                        connection.get(i).getJdbcUrl(),
                        connection.get(i).getDriverClassName(),
                        new Credential(userName, decodePassword),
                        dbUsage.equals("MDB") ? ConnectionType.MDB : ConnectionType.SDB,
                        alias.equals(connectionMdbDefault)
                        ));
            }

            if (StringUtils.isBlank(connectionMdbDefault) ||
                    connections.stream().noneMatch(Connection::isDefaultConnectionMdb)) {
                setMetaDataDefault();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public List<ConnectionConfig> getConnection() {
        return connection;
    }

    public void setConnection(List<ConnectionConfig> connection) {
        this.connection = connection;
    }

    public static List<Connection> getConnections() {
        return connections;
    }

    private void setMetaDataDefault() {

        ConnectionConfiguration.getConnections().stream()
                .filter(Connection::isConnectionMdb)
                .findFirst().get().setDefaultConnectionMdb(true);
    }

    public static class ConnectionConfig {

        private String jdbcUrl;
        private String driverClassName = ConnectionConstants.MSSQL_DRIVER_NAME;

        public ConnectionConfig() {
        }

        public ConnectionConfig(String jdbcUrl) {
            this.jdbcUrl = jdbcUrl;
        }

        String getJdbcUrl() {
            return jdbcUrl;
        }

        public void setJdbcUrl(String jdbcUrl) {
            this.jdbcUrl = jdbcUrl;
        }

        String getDriverClassName() {
            return driverClassName;
        }

        public void setDriverClassName(String driverClassName) {
            this.driverClassName = driverClassName;
        }
    }
}
