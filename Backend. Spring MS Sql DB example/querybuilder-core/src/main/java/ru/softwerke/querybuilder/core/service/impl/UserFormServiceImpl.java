package ru.softwerke.querybuilder.core.service.impl;

import com.fasterxml.jackson.core.JsonProcessingException;
import org.apache.commons.lang3.BooleanUtils;
import org.apache.commons.lang3.StringUtils;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import ru.softwerke.querybuilder.core.data.queryForm.*;
import ru.softwerke.querybuilder.core.data.result.FilterResult;
import ru.softwerke.querybuilder.core.data.result.ListResult;
import ru.softwerke.querybuilder.core.exception.QueryFormIOException;
import ru.softwerke.querybuilder.core.service.MetadataService;
import ru.softwerke.querybuilder.core.service.UserFormService;
import ru.softwerke.querybuilder.integration.jpa.config.ConnectionConfiguration;
import ru.softwerke.querybuilder.integration.jpa.context.ConnectionContextHolder;
import ru.softwerke.querybuilder.integration.jpa.repository.service.ConnectionService;

import java.util.*;
import java.util.stream.Collectors;
import java.util.stream.IntStream;
import java.util.stream.Stream;

/*
 *  @author Svetlana Suvorova
 */
@Service
public class UserFormServiceImpl implements UserFormService {

    @Autowired
    private final ConnectionService connectionService;

    @Autowired
    private MetadataService metadataService;

    @Autowired
    private QueryFormServiceImpl queryFormService;

    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    public UserFormServiceImpl(ConnectionService connectionService, QueryFormServiceImpl queryFormService, MetadataService metadataService) {
        this.connectionService = connectionService;
        this.queryFormService = queryFormService;
        this.metadataService = metadataService;
    }

    @Override
    public String getFilterForm(String id) throws QueryFormIOException, IllegalArgumentException {

        String filterJson ="";
        // Get form json data by form id
        QueryForm queryForm = queryFormService.getQueryForm(id);
        metadataService.setMetadata(queryForm);

        final List<Field> filter = queryForm.getFilter();
        final String databaseName = queryForm.getInfo().getDatabase();
        final String tableName = queryForm.getInfo().getTable();

        String queryFilter = getFilterQuery(databaseName, tableName, filter);
        if (StringUtils.isNotBlank(queryFilter)) {
            String databaseAlias = Optional.ofNullable(
                    ConnectionConfiguration.getConnections().stream()
                            .filter(c -> !c.isConnectionMdb())
                            .findFirst())
                    .filter(Optional::isPresent)
                    .map(c -> c.get().getAlias()).orElse("");

            ConnectionContextHolder.set(databaseAlias);
            List<Object[]> results = connectionService.getResultList(queryFilter);
            ConnectionContextHolder.clear();

            List<FilterResult> filterResults = results.stream()
                    .map(obj -> new FilterResult(String.valueOf(obj[1]), String.valueOf(obj[0])))
                    .collect(Collectors.toList());

            Map<String, List<String>> selectionFieldList = filterResults.stream()
                    .collect(Collectors.groupingBy(FilterResult::getAlias,
                            Collectors.mapping(FilterResult::getValue, Collectors.toCollection(ArrayList::new))));

            // Set data for filter fields
            filter.forEach(field -> selectionFieldList.entrySet().stream()
                    .filter(f -> f.getKey().equals(field.getMetadata().getFieldAlias()))
                    .forEach(f -> field.setData(f.getValue())));

            queryForm.setFilter(filter);
        }

        try {
            filterJson = queryFormService.mapper.writerWithView(QueryFormViews.User.class).writeValueAsString(queryForm);
        } catch (final JsonProcessingException e) {
            logger.error("UserFormService json processing exception by filter form get: %s", (Object)e.getStackTrace());
            throw new IllegalArgumentException("UserFormService json processing exception by filter form get.");
        }

        return filterJson;
    }

    private String getFilterQuery(final String databaseName, final String tableName, List<Field> filter) {

        // List fields grouping by table name
        List<String> selectionFields = filter.stream()
                .filter(f -> StringUtils.isNotBlank(f.getMetadata().getFieldAlias()))
                .filter(f -> f.getMetadata().getType().equals("I"))
                .filter(f  -> (f.getData() != null) && (f.getData().size() == 0))
                .map(f -> f.getMetadata().getFieldAlias())
                .collect(Collectors.toList());

        String querySelectFormat = "SELECT DISTINCT CAST (t.%s AS VARCHAR) as 'value', '%s' AS 'alias' FROM [%s].[dbo].[%s] t";

        // General sql query from list of separate sql query
        return selectionFields.stream()
                .map(f -> String.format(querySelectFormat, f, f, databaseName, tableName))
                .collect(Collectors.joining(" UNION "));
    }

    /*
     *  Return form json with data displaying on the User List Page
     *  @param id form identifier
     *  @param dataJson json string with filter values as {field alias: [filter value identifier array]}
     *  @return list results as json string
     */
    @Override
    public String getListResults(String id, String dataJson) throws QueryFormIOException, IllegalArgumentException {

        String listJson ="";
        try {
            // Get form json data by form id
            final QueryForm queryForm = queryFormService.getQueryForm(id);
            Map<String, List<String>> data = queryFormService.mapper.readValue(dataJson, Map.class);
            metadataService.setMetadata(queryForm);

            final String databaseName = queryForm.getInfo().getDatabase();
            final String tableName = queryForm.getInfo().getTable();

            String databaseAlias = Optional.ofNullable(
                    ConnectionConfiguration.getConnections().stream()
                            .filter(c -> !c.isConnectionMdb())
//                            .filter(c -> c.getAlias().equals(queryForm.getInfo().getDatabase()))
                            .findFirst())
                    .filter(Optional::isPresent)
                    .map(c -> c.get().getAlias()).orElse(null);

            String[] queryList = getListQuery(queryForm, databaseName, tableName, data);
            List<String> selectionFields = Arrays.stream(queryList[1].split(","))
                    .map(String::trim)
                    .collect(Collectors.toList());

            List<Object[]> results = new ArrayList<>();

            ConnectionContextHolder.set(databaseAlias);
            if (selectionFields.size() == 1) {
                // Only one column in the result
                List<Object> tempResults = connectionService.getResultList(queryList[0]);
                results = tempResults.stream()
                        .map(result -> new Object[]{result} )
                        .collect(Collectors.toList());

            } else {
                // Several columns in the result
                results = connectionService.getResultList(queryList[0]);
            }
            ConnectionContextHolder.clear();

            List<List<ListResult>> listResults = results.stream()
                    .map(result ->
                        IntStream.range(0, result.length)
                                .mapToObj(i -> getListResult(queryForm, selectionFields, result, i))
                                .collect(Collectors.toList()))
                    .collect(Collectors.toList());

            listJson = queryFormService.mapper.writeValueAsString(listResults);
        } catch (JsonProcessingException e) {
            logger.error("UserFormService exception by list results writing to json: %s", (Object)e.getStackTrace());
            throw new IllegalArgumentException("UserFormService json processing exception by list results get." + e.getMessage());
        }

        return listJson;
    }

    private ListResult getListResult(QueryForm queryForm, List<String> selectionFields, Object[] result, int i) {
        // Mapping result to field by alias field
        return  Optional.ofNullable(
                // Check all fields of the list and detail on the "S" type
                Stream.concat(queryForm.getList().stream(), queryForm.getDetail().stream())
                        .collect(Collectors.toList())
                        .stream()
                        .filter(f -> StringUtils.isNotBlank(f.getMetadata().getFieldAlias()))
                .filter(f -> f.getMetadata().getFieldAlias().equals(selectionFields.get(i)) &&
                        f.getMetadata().getType().equals("S"))
                .findAny())
                .filter(Optional::isPresent)
                // For field of the "S" type get value from select option
                .map(f -> new ListResult(selectionFields.get(i), Optional.ofNullable(
                        f.get().getMetadata().getData().stream()
                        .filter(lookUp -> lookUp.getId().equals(String.valueOf(result[i])))
                        .findAny())
                        .filter(Optional::isPresent)
                        .map(s -> s.get().getValue())
                        .orElse(String.valueOf(result[i]))))
                 // For all not the "S" type fields set value as result
                .orElse(new ListResult(selectionFields.get(i), String.valueOf(result[i])));
    }

    private String[] getListQuery(final QueryForm queryForm, final String databaseName, final String tableName,
                                  final Map<String, List<String>> data) {

        List<Field> filter = queryForm.getFilter();
        List<Field> list = queryForm.getList();
        List<Field> details = queryForm.getDetail();

        String[] selectionFieldList = Stream.concat(list.stream(), details.stream())
                .collect(Collectors.toList())
                .stream()
                .filter(f -> StringUtils.isNotBlank(f.getMetadata().getFieldAlias()))
                .map(f -> f.getMetadata().getFieldAlias())
                .distinct()
                .toArray(String[]::new);

        String selectionFields = String.join(", ", selectionFieldList);
        String statement = (selectionFieldList.length == 1) ? "DISTINCT" : "";
        // List fields grouping by table name
        List<String> conditionFields = filter.stream()
                .filter(f -> StringUtils.isNotBlank(f.getMetadata().getFieldAlias()))
                .map(f -> {
                        String condition = "";
                        if (data.containsKey(f.getMetadata().getFieldAlias()) && ((data.get(f.getMetadata().getFieldAlias()).size() != 0) ||
                            (f.getMetadata().getType().equals("W") && (f.getData().size() != 0)))) {
                            final List<String> fieldDataList = data.get(f.getMetadata().getFieldAlias());
                            switch (f.getMetadata().getType()) {
                                case "S":
                                case "I":
                                    if (f.getSettings().getType() == Field.FieldType.YESNO) {
                                        condition = (fieldDataList.size() != 1) ? "" :
                                                "t." + f.getMetadata().getFieldAlias() + "=" +
                                                        BooleanUtils.toInteger(Boolean.getBoolean(fieldDataList.get(0)));
                                    } else if (f.getSettings().getType() == Field.FieldType.RTF) {
                                        String[] conditions = fieldDataList.get(0).split(" ");
                                        condition =  "(" +
                                             Arrays.stream(conditions)
                                                    .map(c -> "t." + f.getMetadata().getFieldAlias() + " LIKE '%" + c + "%'")
                                                    .collect(Collectors.joining(" OR ")) +
                                        ")";
                                } else {
                                        condition = "t." + f.getMetadata().getFieldAlias() + " IN (" +
                                                fieldDataList.stream()
                                                        .map(v -> "'" + v
//                                                                        f.getMetadata().getData().stream()
//                                                                                .filter(l -> Integer.valueOf(v) == l.getId()))
                                                                + "'")
                                                        .collect(Collectors.joining(",")) +
                                                ")";
                                    }
                                    break;
                                case "W":
                                    if (!fieldDataList.isEmpty()) {
                                       // Input field
                                       condition = "t." + f.getMetadata().getFieldAlias() + " LIKE '%" + fieldDataList.get(0) + "%'";
                                    } else {
                                        // Expression
                                        condition = StringUtils.join(f.getData(), " AND ");
                                    }
                                    break;
                            }
                        }
                        return condition;
                    })
                .collect(Collectors.toList());
 //                       , Collectors.toCollection(ArrayList::new))));

        String querySelectFormat = "SELECT %s %s FROM [%s].[dbo].[%s] t WHERE %s";

        String whereClause = conditionFields.stream()
                .filter(StringUtils::isNotBlank)
                .collect(Collectors.joining(" AND "));

        // General sql query
        String queryList = String.format(querySelectFormat, statement, selectionFields, databaseName, tableName,
                (StringUtils.isBlank(whereClause) ? "1=1" : whereClause));

        return new String[] {queryList, selectionFields};
    }
}
