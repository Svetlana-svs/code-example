package com.softwerke.com.components.user.profile.dto;

import com.softwerke.com.core.userdata.UserPersonalData;

import java.util.Map;


public class UserProfileArgument extends UserPersonalData {

    public UserProfileArgument() {}

    public UserProfileArgument(Map<String, Object> fields) {
        super(fields);
    }
}
