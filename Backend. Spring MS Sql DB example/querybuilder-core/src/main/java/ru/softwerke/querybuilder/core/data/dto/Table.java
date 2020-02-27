package ru.softwerke.querybuilder.core.data.dto;

import com.fasterxml.jackson.annotation.JsonAutoDetect;

import java.io.Serializable;

/*
 *  @author Svetlana Suvorova
 */
@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
public class Table implements Serializable {

    private int id;
    private String name;

    public Table() {
    }

    public Table(int id, String name) {
        this.id = id;
        this.name = name;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    void setName(String name) {
        this.name = name;
    }
}
