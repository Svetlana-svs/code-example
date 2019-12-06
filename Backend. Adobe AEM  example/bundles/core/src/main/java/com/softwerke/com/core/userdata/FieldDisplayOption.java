package com.softwerke.com.core.userdata;

import java.util.Arrays;
import java.util.Objects;


public enum FieldDisplayOption {
    DISABLED("disabled"),
    OPTIONAL("optional"),
    REQUIRED("required");

    private String value;

    FieldDisplayOption(String value) {
        this.value = value;
    }

    public String getValue() {
        return this.value;
    }

    public static FieldDisplayOption fromObject(Object text, FieldDisplayOption defaultValue) {
        return Arrays.stream(FieldDisplayOption.values())
                .filter(v -> v.getValue().equalsIgnoreCase(Objects.toString(text, "")))
                .findAny()
                .orElse(defaultValue);
    }

    public boolean isRequired() {
        return this.equals(REQUIRED);
    }

    public boolean isOptional() {
        return this.equals(OPTIONAL);
    }

    public boolean isDisabled() {
        return this.equals(DISABLED);
    }
}