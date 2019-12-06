package com.package.name.components.user.profile.dto;

import com.package.name.core.userdata.UserPersonalData;

import java.util.Map;


public class UserProfileArgument extends UserPersonalData {

    public UserProfileArgument() {}

    public UserProfileArgument(Map<String, Object> fields) {
        super(fields);
    }
}
