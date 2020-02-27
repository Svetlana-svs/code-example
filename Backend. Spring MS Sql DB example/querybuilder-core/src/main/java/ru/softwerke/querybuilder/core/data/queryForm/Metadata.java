package ru.softwerke.querybuilder.core.data.queryForm;

import com.fasterxml.jackson.annotation.JsonView;

import java.io.Serializable;
import java.util.List;

/*
 *  @author Svetlana Suvorova
 */
public class Metadata implements Serializable {

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private int satzPos;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private String title;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private int length;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private String type;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private String fieldType;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private String fieldAlias;

    private String database;

    private String table;

    // Value of the lookup fields with "S" type
    @JsonView({QueryFormViews.Admin.class, QueryFormViews.User.class})
    private List<LookUp> data;

    public Metadata() {
    }

    public Metadata(int satzPos, String title, int length, String type, String fieldType, String fieldAlias, String database, String table,
                    List<LookUp> data) {
        this.satzPos = satzPos;
        this.title = title;
        this.length = length;
        this.type = type;
        this.fieldType = fieldType;
        this.fieldAlias = fieldAlias;
        this.database = database;
        this.table = table;
        this.data = data;
    }

    public int getSatzPos() {
        return satzPos;
    }

    public void setSatzPos(int satzPos) {
        this.satzPos = satzPos;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public int getLength() {
        return length;
    }

    public void setLength(int length) {
        this.length = length;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public String getFieldType() {
        return fieldType;
    }

    public void setFieldType(String fieldType) {
        this.fieldType = fieldType;
    }

    public String getFieldAlias() {
        return fieldAlias;
    }

    public void setFieldAlias(String fieldAlias) {
        this.fieldAlias = fieldAlias;
    }

    public String getDatabase() {
        return database;
    }

    public void setDatabase(String database) {
        this.database = database;
    }

    public String getTable() {
        return table;
    }

    public void setTable(String table) {
        this.table = table;
    }

    public List<LookUp> getData() {
        return data;
    }

    public void setData(List<LookUp> data) {
        this.data = data;
    }
}

