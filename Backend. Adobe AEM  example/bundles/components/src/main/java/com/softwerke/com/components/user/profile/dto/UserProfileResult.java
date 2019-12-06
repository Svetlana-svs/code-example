package com.softwerke.com.components.user.profile.dto;

import com.softwerke.com.core.userdata.UserPersonalData;
import com.softwerke.com.integration.jpa.core.repository.AbstractResult;

import java.util.Map;


public class UserProfileResult extends UserPersonalData implements AbstractResult {

    public UserProfileResult() {}

    public UserProfileResult(Map<String, Object> fields) {
        super(fields);
    }
}
