package ru.softwerke.querybuilder.integration.jpa.context;

import org.springframework.jdbc.datasource.lookup.AbstractRoutingDataSource;

/*
 *  @author Svetlana Suvorova
 */
public class DataSourceRouter extends AbstractRoutingDataSource {

    @Override
    protected Object determineCurrentLookupKey() {
        return ConnectionContextHolder.getConnection();
    }
}
