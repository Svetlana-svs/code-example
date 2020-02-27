package ru.softwerke.querybuilder.integration.jpa.dto;

import org.springframework.boot.jdbc.DataSourceBuilder;

import javax.sql.DataSource;

public class Connection {

    private String alias;
    private Credential credential;
    private DataSource dataSource;
    private ConnectionType connectionType;
    private boolean isDefaultConnectionMdb;

    public Connection() {
    }

    public Connection(String alias, String databaseName, String jdbcUrl, String driverClassName, Credential credential,
                      ConnectionType connectionType, boolean isDefaultConnectionMdb) {
        this.alias = alias;
        this.credential = credential;
        this.connectionType = connectionType;
        this.isDefaultConnectionMdb = isDefaultConnectionMdb;

        DataSourceBuilder dataSourceBuilder = DataSourceBuilder.create();
        dataSourceBuilder.url(jdbcUrl + "databaseName=" + databaseName);
        dataSourceBuilder.driverClassName(driverClassName);
        dataSourceBuilder.username(credential.getUserName());
        dataSourceBuilder.password(credential.getPassword());
        dataSource = dataSourceBuilder.build();
    }

    public String getAlias() {
        return alias;
    }

    public void setAlias(String alias) {
        this.alias = alias;
    }

    public Credential getCredential() {
        return credential;
    }

    public void setCredential(Credential credential) {
        this.credential = credential;
    }

    public DataSource getDataSource() {
        return dataSource;
    }

    public void setDataSource(DataSource dataSource) {
        this.dataSource = dataSource;
    }

    public boolean isConnectionMdb() {
        return connectionType == ConnectionType.MDB;
    }

    public boolean isDefaultConnectionMdb() {
        return isDefaultConnectionMdb;
    }

    public void setDefaultConnectionMdb(boolean defaultConnectionMdb) {
        isDefaultConnectionMdb = defaultConnectionMdb;
    }
}
