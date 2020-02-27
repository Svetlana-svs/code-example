package ru.softwerke.querybuilder.core.service.impl;

import org.apache.commons.lang3.StringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import ru.softwerke.querybuilder.core.data.converter.MetadataListConverter;
import ru.softwerke.querybuilder.core.data.converter.TableListConverter;
import ru.softwerke.querybuilder.core.data.dto.Table;
import ru.softwerke.querybuilder.core.data.queryForm.Metadata;
import ru.softwerke.querybuilder.core.data.queryForm.QueryForm;
import ru.softwerke.querybuilder.core.service.MetadataService;
import ru.softwerke.querybuilder.integration.jpa.config.ConnectionConfiguration;
import ru.softwerke.querybuilder.integration.jpa.context.ConnectionContextHolder;
import ru.softwerke.querybuilder.integration.jpa.dto.Connection;
import ru.softwerke.querybuilder.integration.jpa.dto.ConnectionType;
import ru.softwerke.querybuilder.integration.jpa.entity.Datei;
import ru.softwerke.querybuilder.integration.jpa.entity.Satzeintrag;
import ru.softwerke.querybuilder.integration.jpa.repository.AdminRepository;
import ru.softwerke.querybuilder.integration.jpa.repository.DateiRepository;
import ru.softwerke.querybuilder.integration.jpa.repository.service.ConnectionService;

import java.util.Comparator;
import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;
import java.util.stream.Stream;

@Service
public class MetadataServiceImpl implements MetadataService {

    @Autowired
    private final AdminRepository adminRepository;

    @Autowired
    private final DateiRepository dateiRepository;

    @Autowired
    private final ConnectionService connectionService;

    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    public MetadataServiceImpl(AdminRepository adminRepository, DateiRepository dateiRepository, ConnectionService connectionService) {
        this.adminRepository = adminRepository;
        this.dateiRepository = dateiRepository;
        this.connectionService = connectionService;
    }

    @Override
    public String getConnectionMdb(String connectionMdb) {
        return  Optional.ofNullable(
                ConnectionConfiguration.getConnections().stream()
                        .filter(Connection::isConnectionMdb)
                        .filter(c -> c.getAlias().equals(connectionMdb))
                        .findFirst())
                .filter(Optional::isPresent)
                .map(c -> c.get().getAlias()).orElse("");
    }

    @Override
    public String getConnectionSdb(String databaseName, String tableName) {

        String connectionSdb = "";
        List<String> connectionSdbList  = ConnectionConfiguration.getConnections().stream()
                .filter(c -> !c.isConnectionMdb())
                .map(Connection::getAlias)
                .collect(Collectors.toList());
        for (String connection : connectionSdbList) {
            String queryFormat = "IF OBJECT_ID('[%s].[dbo].[%s]') IS NOT NULL SELECT 1 AS 'result' ELSE SELECT 0 AS 'result';";
            final String query = String.format(queryFormat, databaseName, tableName);
            try {
                ConnectionContextHolder.set(connection);
                List<Object> results = connectionService.getResultList(query);
                ConnectionContextHolder.clear();
                if (!results.isEmpty() && (Integer.parseInt(String.valueOf(results.get(0))) == 1)) {
                    connectionSdb = connection;
                    break;
                }
            } catch (RuntimeException e) {
                logger.info(e.getMessage());
            }
        }

        return connectionSdb;
    }

    @Override
    public List<Metadata> getFieldList(String connectionMdb, int id) {

        String connectionMdbAlias = getConnectionMdb(connectionMdb);

        ConnectionContextHolder.set(connectionMdbAlias);
        List<Satzeintrag> satzeintragList = adminRepository.findAllTableFields(id);
        ConnectionContextHolder.clear();

        return MetadataListConverter.convert(satzeintragList);
    }

    @Override
    public List<Table> getTableList(String connectionMdb) {

        String connectionMdbAlias = getConnectionMdb(connectionMdb);

        ConnectionContextHolder.set(connectionMdbAlias);
        List<Datei> dateiList = dateiRepository.findDatabaseAllTables();
        ConnectionContextHolder.clear();

        List<Table> tableList = TableListConverter.convert(dateiList);
        tableList.sort(Comparator.comparing(Table::getName));

        return tableList;
    }

    @Override
    public void setMetadata(QueryForm queryForm) {

        String connectionMdbAlias = getConnectionMdb(queryForm.getInfo().getConnection().get(ConnectionType.MDB));

        ConnectionContextHolder.set(connectionMdbAlias);
        List<Satzeintrag> fieldsList = adminRepository.findAllTableFieldsByType(queryForm.getInfo().getTableId());
        ConnectionContextHolder.clear();

        List<Metadata> metadataList = MetadataListConverter.convert(fieldsList);
        Stream.concat(queryForm.getFilter().stream(), Stream.concat(queryForm.getList().stream(), queryForm.getDetail().stream()))
                .collect(Collectors.toList())
                .stream()
                .filter(f -> StringUtils.isNotBlank(f.getMetadata().getFieldAlias()))
                .filter(f -> f.getMetadata().getType().equals("S"))
                .forEach(f -> f.getMetadata().setData(
                        Optional.ofNullable(
                                metadataList.stream()
                                        .filter(d -> d.getFieldAlias().equals(f.getMetadata().getFieldAlias()))
                                        .findAny())
                                .filter(Optional::isPresent)
                                .map(s -> s.get().getData())
                                .orElse(null)
                ));

        String databaseName = Optional.ofNullable(metadataList.stream()
                .filter(f -> StringUtils.isNotBlank(f.getDatabase()))
                .findFirst())
                .filter(Optional::isPresent)
                .map(f -> f.get().getDatabase())
                .orElse("");
        String tableName = Optional.ofNullable(metadataList.stream()
                .filter(f -> StringUtils.isNotBlank(f.getTable()))
                .findFirst())
                .filter(Optional::isPresent)
                .map(f -> f.get().getTable())
                .orElse("");

        queryForm.getInfo().setDatabase(databaseName);
        queryForm.getInfo().setTable(tableName);
    }
}
