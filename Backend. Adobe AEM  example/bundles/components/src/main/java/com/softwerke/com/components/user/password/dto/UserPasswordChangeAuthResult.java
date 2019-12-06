package com.package.name.components.user.password.dto;

import com.package.name.integration.jpa.core.repository.AbstractResult;

import java.util.Map;
import java.util.Optional;


public class UserPasswordChangeAuthResult implements AbstractResult {

    protected String password;

    public UserPasswordChangeAuthResult() {}

    public UserPasswordChangeAuthResult(Map<String, Object> fields) {
        this.password =  Optional.ofNullable(fields.get("password")).map(Object::toString).orElse(null);
    }

    public String getPassword() {
        return password;
    }
}
