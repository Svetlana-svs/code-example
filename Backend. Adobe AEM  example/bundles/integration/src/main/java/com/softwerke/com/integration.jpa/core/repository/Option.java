package com.softwerke.com.integration.jpa.core.repository;


public enum Option {

    EQUAL("equal"),
    ISNULL("isNull"),
    LIKE("like"),
    LESS("lessThan"),
    GREATER("greaterThan"),
    GREATER_OR_EQUAL("greaterThanOrEqualTo");

    private String name;

    Option(String name) {
        this.name = name;
    }

    public String getName() {
        return name;
    }
}

