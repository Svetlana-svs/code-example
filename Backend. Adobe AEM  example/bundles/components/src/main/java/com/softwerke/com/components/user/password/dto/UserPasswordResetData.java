package com.package.name.components.user.password.dto;

import com.package.name.core.userdata.UserWorkflowStatus;

import java.time.LocalDateTime;


/**
 * Model to send password reset confirmation after validation and before save
 */
public class UserPasswordResetData {
    private String email;
    private LocalDateTime passwordResetDate;
    private UserWorkflowStatus workflowStatus;
    private String passwordResetKey;
    private String passwordHash;

    public UserPasswordResetData() {
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public LocalDateTime getPasswordResetDate() {
        return passwordResetDate;
    }

    public void setPasswordResetDate(LocalDateTime passwordResetDate) {
        this.passwordResetDate = passwordResetDate;
    }

    public UserWorkflowStatus getWorkflowStatus() {
        return workflowStatus;
    }

    public void setWorkflowStatus(UserWorkflowStatus workflowStatus) {
        this.workflowStatus = workflowStatus;
    }

    public String getPasswordResetKey() {
        return passwordResetKey;
    }

    public void setPasswordResetKey(String passwordResetKey) {
        this.passwordResetKey = passwordResetKey;
    }

    public String getPasswordHash() {
        return passwordHash;
    }

    public void setPasswordHash(String passwordHash) {
        this.passwordHash = passwordHash;
    }
}
