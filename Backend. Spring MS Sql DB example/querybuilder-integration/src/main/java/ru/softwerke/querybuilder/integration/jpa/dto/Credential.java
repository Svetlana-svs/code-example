package ru.softwerke.querybuilder.integration.jpa.dto;


public class Credential {
    private String password;
    private String userName;

    public Credential(String userName, String password) {
        this.password = password;
        this.userName = userName;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }
}
