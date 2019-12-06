package com.softwerke.com.core.userdata;

import java.util.Arrays;
import java.util.Objects;


public enum Gender {

    MALE("m"),
    FEMALE("f");

    private String value;

    Gender(String value) {
        this.value = value;
    }

    public String getValue() {
        return this.value;
    }


    public static Gender fromString(String text) {
        return Arrays.stream(Gender.values())
                .filter(v -> v.getValue().equalsIgnoreCase(Objects.toString(text, "")))
                .findAny()
                .orElse(null);
    }
}
