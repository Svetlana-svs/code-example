package ru.softwerke.querybuilder.integration.jpa.dto;

import com.fasterxml.jackson.annotation.JsonProperty;

public enum ConnectionType {
    @JsonProperty("mdb")
    MDB,
    @JsonProperty("sdb")
    SDB;
}
