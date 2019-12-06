package com.softwerke.com.integration.jpa.user.entity;

import com.softwerke.com.core.userdata.Gender;

import javax.persistence.AttributeConverter;
import javax.persistence.Converter;


@Converter(autoApply = true)
public class GenderAttributeConverter implements AttributeConverter<Gender, String> {
    @Override
    public String convertToDatabaseColumn(Gender attribute) {
        if (attribute == null)
            return null;

        switch (attribute) {
            case MALE:
                return "m";

            case FEMALE:
                return "f";

            default:
                throw new IllegalArgumentException(attribute + " not supported.");
        }
    }

    @Override
    public Gender convertToEntityAttribute(String dbData) {
        if (dbData == null)
            return null;

        switch (dbData) {
            case "m":
                return Gender.MALE;

            case "f":
                return Gender.FEMALE;

            default:
                throw new IllegalArgumentException(dbData + " not supported.");
        }
    }
}
