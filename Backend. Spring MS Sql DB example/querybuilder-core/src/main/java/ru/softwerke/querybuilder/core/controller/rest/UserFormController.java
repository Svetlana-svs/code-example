package ru.softwerke.querybuilder.core.controller.rest;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.softwerke.querybuilder.core.exception.QueryFormIOException;
import ru.softwerke.querybuilder.core.service.UserFormService;

/*
 *  @author Svetlana Suvorova
 *
 *  Controller for mapping and handling all web requests related userForm (Filter, List, Detail) and generation the responses.
 */
@RestController
@RequestMapping(path = "/api/userForm")
public class UserFormController {

    @Autowired
    private final UserFormService userFormService;

    private final Logger logger = LoggerFactory.getLogger(this.getClass());

    @Autowired
    public UserFormController(UserFormService userFormService) {
        this.userFormService = userFormService;
    }

    @PostMapping(path = {"/filter/{id}"}, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<Object> getFilterForm(@PathVariable("id") String id) {

        String formJson ="";
        try {
            formJson = userFormService.getFilterForm(id);
        } catch (final QueryFormIOException e) {
            return new ResponseEntity<>(e.getMessage(), HttpStatus.INTERNAL_SERVER_ERROR);
        }
        catch (final IllegalArgumentException e) {
            return new ResponseEntity<>(e.getMessage(), HttpStatus.BAD_REQUEST);
        }

        return new ResponseEntity<>(formJson, HttpStatus.OK);
    }


    @PostMapping(path = {"/list/{id}"}, produces = MediaType.APPLICATION_JSON_UTF8_VALUE)
    public ResponseEntity<Object> getList(@PathVariable("id") String id,
                                          @RequestParam String data) {
        String listResult = "";
        try {
            listResult = userFormService.getListResults(id, data);
        } catch (final QueryFormIOException e) {
            return new ResponseEntity<>(e.getMessage(), HttpStatus.INTERNAL_SERVER_ERROR);
        }
        catch (final IllegalArgumentException e) {
            return new ResponseEntity<>(e.getMessage(), HttpStatus.BAD_REQUEST);
        }

        return new ResponseEntity<>(listResult, HttpStatus.OK);
    }
}
