package ru.softwerke.querybuilder.core.data.dto;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.apache.commons.lang3.StringUtils;

/*
 *  @author Svetlana Suvorova
 */
public class ControllerSettings {

    private String contextPath;

    public ControllerSettings(String contextPath) {
        this.contextPath = contextPath;
    }

    public String getContextPath() {
        return contextPath;
    }

    public void setContextPath(String contextPath) {
        this.contextPath = contextPath;
    }

    public String toJson() {
        ObjectMapper objectMapper = new ObjectMapper();
        try {
            return objectMapper.writeValueAsString(this);
        } catch (JsonProcessingException e) {
            // TODO: error handler
            e.printStackTrace();
        }

        return StringUtils.EMPTY;
    }
}
