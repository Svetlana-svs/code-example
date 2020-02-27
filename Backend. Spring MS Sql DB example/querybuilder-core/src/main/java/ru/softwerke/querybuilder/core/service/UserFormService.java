package ru.softwerke.querybuilder.core.service;

import ru.softwerke.querybuilder.core.exception.QueryFormIOException;

/*
 *  @author Svetlana Suvorova
 */
public interface UserFormService {

    String getFilterForm(String id) throws QueryFormIOException, IllegalArgumentException;

    String getListResults(String id, String dataJson) throws QueryFormIOException, IllegalArgumentException;
}
