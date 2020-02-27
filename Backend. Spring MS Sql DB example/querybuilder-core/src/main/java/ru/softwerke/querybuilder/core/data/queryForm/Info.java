package ru.softwerke.querybuilder.core.data.queryForm;

import com.fasterxml.jackson.annotation.JsonAutoDetect;
import com.fasterxml.jackson.annotation.JsonView;
import ru.softwerke.querybuilder.integration.jpa.dto.ConnectionType;

import java.io.Serializable;
import java.time.LocalDateTime;
import java.util.List;
import java.util.Map;

/*
 *  @author Svetlana Suvorova
 */
@JsonAutoDetect(fieldVisibility = JsonAutoDetect.Visibility.ANY)
public class Info implements Serializable {

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class, QueryFormViews.User.class})
    private String name;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class})
    private String comment;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class})
    private String author;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class})
    private LocalDateTime date;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class})
    private Map<ConnectionType, String> connection;

    private String database;

    private String table;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class})
    private int tableId;

    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class})
    private  boolean isAuthenticated;

    // List of users id
    @JsonView({QueryFormViews.Internal.class, QueryFormViews.Admin.class})
    private List<Integer> users;

    public Info() {
    }

    String getName() {
        return name;
    }

    void setName(String name) {
        this.name = name;
    }

    public String getComment() {
        return comment;
    }

    public void setComment(String comment) {
        this.comment = comment;
    }

    public String getAuthor() {
        return author;
    }

    public void setAuthor(String author) {
        this.date = LocalDateTime.now();
        this.author = author;
    }

    public LocalDateTime getDate() {
        return date;
    }

    void setDate(LocalDateTime date) {
        this.date = date;
    }

    public Map<ConnectionType, String> getConnection() {
        return connection;
    }

    public void setConnection(Map<ConnectionType, String> connection) {
        this.connection = connection;
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

    public int getTableId() {
        return tableId;
    }

    public void setTableId(int tableId) {
        this.tableId = tableId;
    }

    public boolean isAuthenticated() {
        return isAuthenticated;
    }

    public void setIsAuthenticated(boolean isAuthenticated) {
        this.isAuthenticated = isAuthenticated;
    }

    public List<Integer> getUsers() {
        return users;
    }

    public void setUsers(List<Integer> users) {
        this.users = users;
    }
}
