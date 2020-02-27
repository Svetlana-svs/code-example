package ru.softwerke.querybuilder.core.service;

import ru.softwerke.querybuilder.core.data.dto.Table;
import ru.softwerke.querybuilder.core.data.queryForm.Metadata;
import ru.softwerke.querybuilder.core.data.queryForm.QueryForm;

import java.util.List;

public interface MetadataService {

    String getConnectionMdb(String connectionMdb);

    String getConnectionSdb(String databaseName, String tableName);

    List<Metadata> getFieldList(String connectionMdb, int id);

    List<Table> getTableList(String connectionMdb);

    void setMetadata(QueryForm queryForm);
}
