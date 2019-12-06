package com.package.name.components.user.password.dto;

import com.package.name.integration.jpa.core.repository.AbstractResult;

import java.time.LocalDateTime;
import java.util.Map;
import java.util.Optional;


public class UserPasswordChangeResult implements AbstractResult {

    protected long id;
    protected String email;
    protected String passwordResetKey;
    protected LocalDateTime passwordResetDate;

    public UserPasswordChangeResult() {}

    public UserPasswordChangeResult(Map<String, Object> fields) {
        this.id = Long.parseLong(fields.get("id").toString());
        this.email = Optional.ofNullable(fields.get("email")).map(Object::toString).orElse(null);
        this.passwordResetKey = Optional.ofNullable(fields.get("passwordResetKey")).map(Object::toString).orElse(null);
        this.passwordResetDate = Optional.ofNullable(fields.get("passwordResetDate")).map(field -> LocalDateTime.parse(field.toString())).orElse(null);
    }

    public Long getId() {
        return id;
    }

    public String getEmail() {
        return email;
    }

    public String getPasswordResetKey() {
        return passwordResetKey;
    }

    public LocalDateTime getPasswordResetDate() {
        return passwordResetDate;
    }
}
