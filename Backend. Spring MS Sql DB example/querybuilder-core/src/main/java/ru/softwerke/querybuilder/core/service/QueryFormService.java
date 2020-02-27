package ru.softwerke.querybuilder.core.service;

import ru.softwerke.querybuilder.core.data.queryForm.QueryForm;
import ru.softwerke.querybuilder.core.exception.QueryFormIOException;

import javax.naming.AuthenticationException;
import java.util.List;
import java.util.Map;

/*
 *  @author Svetlana Suvorova
 */
public interface QueryFormService {

    String createQueryForm(String data) throws QueryFormIOException, AuthenticationException;

    QueryForm getQueryForm(String id) throws QueryFormIOException, IllegalArgumentException;

    String getQueryFormFull(String id) throws QueryFormIOException;

    void saveQueryForm(String id, String data) throws QueryFormIOException, AuthenticationException;

    void deleteQueryForm(String id) throws QueryFormIOException;

    Map<String, String> getQueryFormList();

    List<String> getCssFileList();
}
