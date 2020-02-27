package ru.softwerke.querybuilder.core.config;

/*
 *  @author Svetlana Suvorova
 */
public enum Role {

    ROLE_ADMIN,
    ROLE_USER;

    public String getName(){return this.name().toLowerCase();};
}
