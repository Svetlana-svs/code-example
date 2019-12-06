package com.package.name.components.user.profile.model;


import java.util.HashMap;
import java.util.Map;

/**
 * Model of the user data field with field properties is used to send user data for display
 */
public class UserDataFieldSettings {

    private Map<String, Object> validationRule;
    private String displayOption;

    UserDataFieldSettings() {
    }

    public UserDataFieldSettings(Map<String, Object> validationRule, String fieldDisplayOption) {
        this.validationRule = (validationRule == null)? null : new HashMap<String, Object>(validationRule);
        this.displayOption = fieldDisplayOption;
    }

    public Map<String, Object> getValidationRule() {
        return validationRule;
    }

    public void setValidationRule(Map<String, Object> validationRule) {
        this.validationRule = validationRule;
    }

    public String getDisplayOption() {
        return displayOption;
    }

    public void setDisplayOption(String displayOption) {
        this.displayOption = displayOption;
    }
}
