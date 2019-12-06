package com.package.name.components.user.profile.dto;

import com.package.name.core.userdata.UserPersonalData;
import com.package.name.integration.jpa.core.repository.AbstractResult;

import java.util.Map;


public class UserProfileResult extends UserPersonalData implements AbstractResult {

    public UserProfileResult() {}

    public UserProfileResult(Map<String, Object> fields) {
        super(fields);
    }
}
