package com.softwerke.com.core.userdata;

import com.softwerke.com.core.util.ValidationUtil;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;
import com.google.gson.TypeAdapter;
import com.google.gson.reflect.TypeToken;
import com.google.gson.stream.JsonReader;
import com.google.gson.stream.JsonWriter;
import org.apache.commons.lang3.StringUtils;
import org.apache.sling.api.resource.ValueMap;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.IOException;
import java.time.LocalDate;
import java.time.format.DateTimeFormatter;
import java.util.Map;
import java.util.Optional;


public class UserPersonalData {

    public static final String USER_INFO_DATE_FORMAT = "dd-MM-yyyy";
    public static final String USER_SERVER_DATE_FORMAT = "yyyy-MM-dd";
    private static final DateTimeFormatter formatter = DateTimeFormatter.ofPattern(USER_SERVER_DATE_FORMAT);

    private static final Logger logger = LoggerFactory.getLogger(UserPersonalData.class);

    private static final class LocalDateAdapter extends TypeAdapter<LocalDate> {
        @Override
        public LocalDate read(final JsonReader jsonReader) throws IOException {
            return LocalDate.parse(jsonReader.nextString());
        }

        @Override
        public void write(final JsonWriter jsonWriter, final LocalDate localDate) throws IOException {
            jsonWriter.value(localDate.toString());
        }
    }

    private static final Gson gson = new GsonBuilder()
            .serializeNulls()
            .registerTypeAdapter(LocalDate.class, new UserPersonalData.LocalDateAdapter().nullSafe())
            .create();

    protected String firstName;

    protected String middleName;

    protected String lastName;

    protected String phone;

    protected LocalDate birthDate;

    protected Gender gender;

    protected String city;

    protected String address;

    public UserPersonalData() {
    }

    public UserPersonalData(Map<String, Object> fields) {
        this.phone = Optional.ofNullable(fields.get("phone")).map(Object::toString).orElse(null);

        this.firstName = Optional.ofNullable(fields.get("firstName")).map(Object::toString).orElse(null);
        this.middleName = Optional.ofNullable(fields.get("middleName")).map(Object::toString).orElse(null);
        this.lastName = Optional.ofNullable(fields.get("lastName")).map(Object::toString).orElse(null);

        this.birthDate = Optional.ofNullable(fields.get("birthDate")).map(field -> LocalDate.parse(field.toString(), formatter)).orElse(null);
        this.gender = Optional.ofNullable(fields.get("gender")).map(field -> Gender.valueOf(field.toString().toUpperCase())).orElse(null);

        this.city = Optional.ofNullable(fields.get("city")).map(Object::toString).orElse(null);
        this.address = Optional.ofNullable(fields.get("address")).map(Object::toString).orElse(null);
    }

    public String toJson() {
        return gson.toJson(this, this.getClass());
    }

    public Map<String, Object> toMap() {
        String userDataJson = gson.toJson(this, this.getClass());
        Map<String, Object> userDataMap = gson.fromJson(userDataJson, new TypeToken<Map<String, Object>>(){}.getType());

        // Put fields to map with type save
        if (userDataMap.containsKey("birthDate")) {
            userDataMap.replace("birthDate", this.birthDate);
        }
        if (userDataMap.containsKey("gender")) {
            userDataMap.replace("gender", this.gender);
        }

        return userDataMap;
    }

    public String getFirstName() {
        return firstName;
    }

    public String getMiddleName() {
        return middleName;
    }

    public String getLastName() {
        return lastName;
    }

    public String getPhone() {
        return phone;
    }

    public LocalDate getBirthDate() {
        return birthDate;
    }

    public Gender getGender() {
        return gender;
    }

    public String getCity() {
        return city;
    }

    public String getAddress() {
        return address;
    }

    public String getValidationRule(String fieldName) {
        switch (fieldName) {
            case "firstName":
            case "lastName":
                return ValidationUtil.REGEX_NAME_CHARACTERS;
            case "middleName":
            case "country":
                return ValidationUtil.REGEX_NAME_LETTERS;
            case "phone":
                return ValidationUtil.REGEX_DIGIT;
            case "city":
                return ValidationUtil.REGEX_CITY;
            case "address":
                return ValidationUtil.REGEX_ADDRESS_LINE;
            default:
                return null;
        }
    }

    public boolean validateFirstName(FieldDisplayOption fieldDiasplayOption) {
        return (!((fieldDiasplayOption == FieldDisplayOption.REQUIRED) && StringUtils.isBlank(firstName)) &&
                ValidationUtil.validateParameter(firstName, getValidationRule("firstName")));
    }

    public boolean validateMiddleName(FieldDisplayOption fieldDiasplayOption) {
        return (!((fieldDiasplayOption == FieldDisplayOption.REQUIRED) && StringUtils.isBlank(middleName)) &&
                (StringUtils.isBlank(middleName) || ValidationUtil.validateParameter(middleName, getValidationRule("middleName"))));
    }

    public boolean validateLastName(FieldDisplayOption fieldDiasplayOption) {
        return (!((fieldDiasplayOption == FieldDisplayOption.REQUIRED) && StringUtils.isEmpty(lastName)) &&
                (StringUtils.isEmpty(lastName) || ValidationUtil.validateParameter(lastName, getValidationRule("lastName"))));
    }

    public boolean validatePhone(FieldDisplayOption fieldDiasplayOption) {
        return (!((fieldDiasplayOption == FieldDisplayOption.REQUIRED) && StringUtils.isBlank(phone)) &&
                (StringUtils.isBlank(phone) || ValidationUtil.validateParameter(phone, getValidationRule("phone"))));
    }

    public boolean validateBirthDate(FieldDisplayOption fieldDiasplayOption) {
        return !((fieldDiasplayOption == FieldDisplayOption.REQUIRED) && (birthDate == null));
    }

    public boolean validateGender(FieldDisplayOption fieldDiasplayOption) {
        return !((fieldDiasplayOption == FieldDisplayOption.REQUIRED) && (gender == null));
    }

    public boolean validateCity(FieldDisplayOption fieldDiasplayOption) {
        return (!((fieldDiasplayOption == FieldDisplayOption.REQUIRED) && StringUtils.isBlank(city)) &&
                (StringUtils.isBlank(city) || ValidationUtil.validateParameter(city, getValidationRule("city"))));
    }

    public boolean validateAddress(FieldDisplayOption fieldDiasplayOption) {
        return (!((fieldDiasplayOption == FieldDisplayOption.REQUIRED) && StringUtils.isEmpty(address)) &&
                (StringUtils.isEmpty(address) || ValidationUtil.validateParameter(address, getValidationRule("address"))));
    }

    public boolean validateUserPersonalData(ValueMap componentSettings) {

        FieldDisplayOption fieldDisplayOption = FieldDisplayOption.fromObject(componentSettings.get("firstName"), FieldDisplayOption.DISABLED);
        if (!validateFirstName(fieldDisplayOption)) {
            logger.debug("UserPersonalData validation failed. Field 'firstName' is not valid.");
            return false;
        }
        fieldDisplayOption = FieldDisplayOption.fromObject(componentSettings.get("middleName"), FieldDisplayOption.DISABLED);
        if (!validateMiddleName(fieldDisplayOption)) {
            logger.debug("UserPersonalData validation failed. Field 'middleName' is not valid.");
            return false;
        }
        fieldDisplayOption = FieldDisplayOption.fromObject(componentSettings.get("lastName"), FieldDisplayOption.DISABLED);
        if (!validateLastName(fieldDisplayOption)) {
            logger.debug("UserPersonalData validation failed. Field 'lastName' is not valid.");
            return false;
        }
        fieldDisplayOption = FieldDisplayOption.fromObject(componentSettings.get("phone"), FieldDisplayOption.DISABLED);
        if (!validatePhone(fieldDisplayOption)) {
            logger.debug("UserPersonalData validation failed. Field 'phone' is not valid.");
            return false;
        }
        fieldDisplayOption = FieldDisplayOption.fromObject(componentSettings.get("birthDate"), FieldDisplayOption.DISABLED);
        if (!validateBirthDate(fieldDisplayOption)) {
            logger.debug("UserPersonalData validation failed. Field 'birthDate' is not valid.");
            return false;
        }
        fieldDisplayOption = FieldDisplayOption.fromObject(componentSettings.get("gender"), FieldDisplayOption.DISABLED);
        if (!validateGender(fieldDisplayOption)) {
            logger.debug("UserRegistration validation failed. Field 'gender' is not valid.");
            return false;
        }
        fieldDisplayOption = FieldDisplayOption.fromObject(componentSettings.get("city"), FieldDisplayOption.DISABLED);
        if (!validateCity(fieldDisplayOption)) {
            logger.debug("UserPersonalData validation failed. Field 'city' is not valid.");
            return false;
        }
        fieldDisplayOption = FieldDisplayOption.fromObject(componentSettings.get("address"), FieldDisplayOption.DISABLED);
        if (!validateAddress(fieldDisplayOption)) {
            logger.debug("UserPersonalData validation failed. Field 'address' is not valid.");
            return false;
        }

        return true;
    }
}
