package com.package.name.components.user.password.dto;


/**
 * Model to send password reset confirmation after validation and before save
 */
public class UserPasswordChangeData {
    // Authenticated user password change fields
    private String passwordOld;
    private String passwordNew;

    // Password change fields by password reset
    private Long id;
    private String token;
    private String email;

    public UserPasswordChangeData() {}

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPasswordOld() {
        return passwordOld;
    }

    public void setPasswordOld(String passwordOld) {
        this.passwordOld = passwordOld;
    }

    public String getPasswordNew() {
        return passwordNew;
    }

    public void setPasswordNew(String passwordNew) {
        this.passwordNew = passwordNew;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
    }
}
