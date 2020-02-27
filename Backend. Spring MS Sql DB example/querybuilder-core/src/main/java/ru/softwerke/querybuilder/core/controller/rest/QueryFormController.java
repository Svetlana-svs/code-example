package ru.softwerke.querybuilder.core.controller.rest;

import com.fasterxml.jackson.databind.node.ObjectNode;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.softwerke.querybuilder.core.data.dto.Database;
import ru.softwerke.querybuilder.core.data.dto.Table;
import ru.softwerke.querybuilder.core.data.queryForm.Metadata;
import ru.softwerke.querybuilder.core.exception.QueryFormIOException;
import ru.softwerke.querybuilder.core.service.MetadataService;
import ru.softwerke.querybuilder.core.service.UserRoleService;
import ru.softwerke.querybuilder.core.service.impl.QueryFormServiceImpl;
import ru.softwerke.querybuilder.integration.jpa.config.ConnectionConfiguration;
import ru.softwerke.querybuilder.integration.jpa.dto.Connection;
import ru.softwerke.querybuilder.integration.jpa.dto.User;

import javax.naming.AuthenticationException;
import javax.servlet.http.HttpServletRequest;
import java.util.*;
import java.util.concurrent.atomic.AtomicInteger;
import java.util.stream.Collectors;

/*
 *  @author Svetlana Suvorova
 *
 *  Controller for mapping and handling all web requests related queryForm and generation the responses.
 *
 */
@RestController
@RequestMapping(path = "/api/queryForm")
public class QueryFormController {

    @Autowired
    private final UserRoleService userRoleService;

    @Autowired
    private final QueryFormServiceImpl queryFormService;

    @Autowired
    private final MetadataService metadataService;

    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    @Autowired
    public QueryFormController(UserRoleService userRoleService, QueryFormServiceImpl queryFormService, MetadataService metadataService) {
        this.userRoleService = userRoleService;
        this.queryFormService = queryFormService;
        this.metadataService = metadataService;
    }

    @PostMapping(path = {"/all"}, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<Object> getQueryFormList() {

        final Map<String, String> queryFormList = queryFormService.getQueryFormList();

        return new ResponseEntity<>(queryFormList, HttpStatus.OK);
    }

    @PostMapping(path = {"/databaseList"}, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<Object> getDatabaseList() {
        // Create databases map with sort order databases saving
        AtomicInteger index = new AtomicInteger(0);
        List<Database> databaseList = ConnectionConfiguration.getConnections().stream()
                .filter(Connection::isConnectionMdb)
                .map(c -> new Database(index.incrementAndGet(), c.getAlias()))
                .collect(Collectors.toList());

        ObjectNode jsonObject = queryFormService.mapper.createObjectNode();
        jsonObject.putPOJO("databases", databaseList);

        return new ResponseEntity<>(jsonObject, HttpStatus.OK);
    }

    @PostMapping(path = {"/cssFileList"}, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<Object> getCssFileList() {

        final List<String> cssFileList = queryFormService.getCssFileList();

        return new ResponseEntity<>(cssFileList, HttpStatus.OK);
    }

    @PostMapping(path = {"/tableList"}, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<Object> getTableList(@RequestParam String connectionMdb) {

        final List<Table> tableList = metadataService.getTableList(connectionMdb);

        ObjectNode jsonObject = queryFormService.mapper.createObjectNode();
        jsonObject.putPOJO("tables", tableList);

        return new ResponseEntity<>(jsonObject, HttpStatus.OK);
    }

    @PostMapping(path = {"/fieldList"}, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<Object> getFieldList(@RequestParam String connectionMdb,
                                               @RequestParam int id) {

        final List<Metadata> metadataList = metadataService.getFieldList(connectionMdb, id);

        ObjectNode jsonObject = queryFormService.mapper.createObjectNode();
        jsonObject.putPOJO("fields", metadataList);

        return new ResponseEntity<>(jsonObject, HttpStatus.OK);
    }

    @PostMapping(path = {"/userList"}, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<Object> getUserList(@RequestParam String connectionMdb) {

        final List<User> userList = userRoleService.getAllUserList(connectionMdb);

        ObjectNode jsonObject = queryFormService.mapper.createObjectNode();
        jsonObject.putPOJO("users", userList);

        return new ResponseEntity<>(jsonObject, HttpStatus.OK);
    }

    @PostMapping(path = {"/{action}/{id}", "/{action}"}, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<Object> handleQueryForm(HttpServletRequest request,
                                                  @PathVariable("action") String action,
                                                  @PathVariable("id") Optional<String> id,
                                                  @RequestParam Optional<String> data) {
        try {
            // Root to action handler call
            switch (action) {
                case "new":
                    // Create new query form
                    String newId = queryFormService.createQueryForm(
                            Optional.ofNullable(data.get()).map(Objects::toString).orElseThrow(IllegalArgumentException::new));

                    ObjectNode jsonObject = queryFormService.mapper.createObjectNode();
                    jsonObject.putPOJO("id", newId);

                    return new ResponseEntity<>(jsonObject, HttpStatus.OK);
                case "edit":
                    // Edit query form by id
                    queryFormService.saveQueryForm(Optional.ofNullable(id.get()).map(Objects::toString).orElseThrow(IllegalArgumentException::new),
                            Optional.ofNullable(data.get()).map(Objects::toString).orElseThrow(IllegalArgumentException::new) );
                    break;
                case "get":
                    // Get query form data by id
                    // TODO: set metadata - data for fields of the type "S" Admin class all select options
                    String queryFormContent = queryFormService.getQueryFormFull(Optional.ofNullable(id.get()).map(Objects::toString).orElseThrow(IllegalArgumentException::new));
                    return new ResponseEntity<>(queryFormContent, HttpStatus.OK);
                case "delete":
                    // Remove query form by id
                    queryFormService.deleteQueryForm(Optional.ofNullable(id.get()).map(Objects::toString).orElseThrow(IllegalArgumentException::new));
                    break;
            }
        } catch (QueryFormIOException e) {
            return new ResponseEntity<>(e.getMessage(), HttpStatus.INTERNAL_SERVER_ERROR);
        }
        catch (IllegalArgumentException | AuthenticationException e) {
            return new ResponseEntity<>(e.getMessage(), HttpStatus.BAD_REQUEST);
        }

        return new ResponseEntity<>(HttpStatus.OK);
    }
}
