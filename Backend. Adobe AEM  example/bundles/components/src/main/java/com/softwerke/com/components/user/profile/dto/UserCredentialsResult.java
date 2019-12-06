package com.softwerke.com.components.user.profile.dto;

import com.softwerke.com.integration.jpa.core.repository.AbstractResult;

import java.util.Map;
import java.util.Optional;


public class UserCredentialsResult implements AbstractResult {

    protected String email;

    public UserCredentialsResult() {}

    public UserCredentialsResult(Map<String, Object> fields) {
        this.email = Optional.ofNullable(fields.get("email")).map(Object::toString).orElse(null);
    }

    public String getEmail() {
        return email;
    }
}
