package ru.softwerke.querybuilder.core.data.queryForm;

import com.fasterxml.jackson.annotation.JsonView;

import java.io.Serializable;

public class LookUp implements Serializable {

    @JsonView({QueryFormViews.Admin.class, QueryFormViews.User.class})
    private String id;

    @JsonView({QueryFormViews.Admin.class, QueryFormViews.User.class})
    private String value;

    public LookUp() {
    }

    public LookUp(String id, String value) {
        this.id = id;
        this.value = value;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }
}
