package com.package.name.components.user.profile.model;

import com.package.name.core.annotation.Inherited;
import com.package.name.core.model.AbstractComponentModel;
import com.package.name.core.util.ValidationUtil;
import com.google.gson.Gson;
import com.google.gson.JsonObject;
import org.apache.sling.api.SlingHttpServletRequest;
import org.apache.sling.models.annotations.Default;
import org.apache.sling.models.annotations.DefaultInjectionStrategy;
import org.apache.sling.models.annotations.Model;
import org.apache.sling.models.annotations.Via;

import javax.inject.Inject;
import javax.inject.Named;
import java.util.*;

@Model(adaptables = {SlingHttpServletRequest.class}, defaultInjectionStrategy = DefaultInjectionStrategy.OPTIONAL)
public class UserProfileModel extends AbstractComponentModel {

    public static final Integer DEFAULT_STARTING_YEAR = 1917;

    private final FormRenderModeType DEFAULT_RENDER_MODE = FormRenderModeType.MODAL;

    @Inject
    @Via("resource")
    @Inherited
    private String heading;

    @Inject
    @Via("resource")
    @Inherited
    private String description;

    @Inject
    @Via("resource")
    @Default(values="modal")
    private String formRenderMode;

    @Inject
    @Via("resource")
    private Integer startingYear;

    @Inject
    @Via("resource")
    @Named("imageMale/fileReference")
    private String imageMaleURL;

    @Inject
    @Via("resource")
    @Named("imageFemale/fileReference")
    private String imageFemaleURL;

    @Inject
    @Inherited
    @Via("resource")
    @Named("firstName")
    @Default(values="required")
    private String firstNameFieldDisplayOption;

    @Inject
    @Inherited
    @Via("resource")
    @Named("middleName")
    @Default(values="disabled")
    private String middleNameFieldDisplayOption;

    @Inject
    @Inherited
    @Via("resource")
    @Named("lastName")
    @Default(values="disabled")
    private String lastNameFieldDisplayOption;

    @Inject
    @Inherited
    @Via("resource")
    @Named("phone")
    @Default(values="disabled")
    private String phoneFieldDisplayOption;

    @Inject
    @Inherited
    @Via("resource")
    @Named("birthDate")
    @Default(values="disabled")
    private String birthDateFieldDisplayOption;

    @Inject
    @Inherited
    @Via("resource")
    @Named("gender")
    @Default(values="disabled")
    private String genderFieldDisplayOption;

    @Inject
    @Inherited
    @Via("resource")
    @Named("country")
    @Default(values="disabled")
    private String countryFieldDisplayOption;

    @Inject
    @Inherited
    @Via("resource")
    @Named("city")
    @Default(values="disabled")
    private String cityFieldDisplayOption;

    @Inject
    @Inherited
    @Via("resource")
    @Named("address")
    @Default(values="disabled")
    private String addressFieldDisplayOption;

    private Integer currentYear = Calendar.getInstance().get(Calendar.YEAR);

    public UserProfileModel(SlingHttpServletRequest request) {
        super(request);
    }

    public String getHeading() {
        return heading;
    }

    public void setHeading(String heading) {
        this.heading = heading;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getFormRenderMode() {
        return formRenderMode != null ? formRenderMode : DEFAULT_RENDER_MODE.toString();
    }

    public Integer getStartingYear() {
        return startingYear != null ? startingYear : DEFAULT_STARTING_YEAR;
    }

    public Integer getCurrentYear() {
        return currentYear;
    }

    public String getImageMaleURL() {
        return imageMaleURL;
    }

    public String getImageFemaleURL() {
        return imageFemaleURL;
    }

    private UserDataFieldSettings getFirstNameFieldSettings() {
        Map<String, Object> validationRule= new HashMap<String, Object>();
        validationRule.put("regex", ValidationUtil.REGEX_NAME_CHARACTERS);
        return new UserDataFieldSettings(validationRule,  firstNameFieldDisplayOption);
    }

    private UserDataFieldSettings getMiddleNameFieldSettings() {
        Map<String, Object> validationRule= new HashMap<String, Object>();
//       TODO: use included in VeeValidate rule (with cyrillic support) instead custom regex
//        validationRule.put("alpha", true);
        validationRule.put("regex", ValidationUtil.REGEX_NAME_LETTERS);
        return new UserDataFieldSettings(validationRule, middleNameFieldDisplayOption);
    }

    private UserDataFieldSettings getLastNameFieldSettings() {
        Map<String, Object> validationRule= new HashMap<String, Object>();
        validationRule.put("regex", ValidationUtil.REGEX_NAME_CHARACTERS);
        return new UserDataFieldSettings(validationRule, lastNameFieldDisplayOption);
    }

    private UserDataFieldSettings getPhoneFieldSettings() {
        Map<String, Object> validationRule= new HashMap<String, Object>();
        validationRule.put("regex", ValidationUtil.REGEX_DIGIT);
        return new UserDataFieldSettings(validationRule, phoneFieldDisplayOption);
    }

    private UserDataFieldSettings getBirthDateFieldSettings() {
        return new UserDataFieldSettings(null, birthDateFieldDisplayOption);
    }

    private UserDataFieldSettings getGenderFieldSettings() {
        return new UserDataFieldSettings(null, genderFieldDisplayOption);
    }

    private UserDataFieldSettings getCountryFieldSettings() {
        Map<String, Object> validationRule= new HashMap<String, Object>();
        validationRule.put("regex", ValidationUtil.REGEX_NAME_LETTERS);
        return new UserDataFieldSettings(validationRule, countryFieldDisplayOption);
    }

    private UserDataFieldSettings getCityFieldSettings() {
        Map<String, Object> validationRule= new HashMap<String, Object>();
        validationRule.put("regex", ValidationUtil.REGEX_CITY);
        return new UserDataFieldSettings(validationRule, cityFieldDisplayOption);
    }

    private UserDataFieldSettings getAddressFieldSettings() {
        Map<String, Object> validationRule= new HashMap<String, Object>();
        validationRule.put("regex", ValidationUtil.REGEX_ADDRESS_LINE);
        return new UserDataFieldSettings(validationRule, addressFieldDisplayOption);
    }

    private Map<String, UserDataFieldSettings> getUserDataSettings() {
        Map<String, UserDataFieldSettings> userDataSettingsList = new HashMap<String, UserDataFieldSettings>();

        userDataSettingsList.put("firstName", getFirstNameFieldSettings());
        userDataSettingsList.put("middleName",getMiddleNameFieldSettings());
        userDataSettingsList.put("lastName", getLastNameFieldSettings());
        userDataSettingsList.put("phone", getPhoneFieldSettings());
        userDataSettingsList.put("birthDate", getBirthDateFieldSettings());
        userDataSettingsList.put("gender", getGenderFieldSettings());
        userDataSettingsList.put("country", getCountryFieldSettings());
        userDataSettingsList.put("city", getCityFieldSettings());
        userDataSettingsList.put("address", getAddressFieldSettings());

        return userDataSettingsList;
    }

    @Override
    public String getComponentSettings() {
        JsonObject settings = new JsonObject();
        settings.addProperty("formRenderMode", getFormRenderMode().toString());
        settings.addProperty("imageMaleURL", imageMaleURL);
        settings.addProperty("imageFemaleURL", imageFemaleURL);
        settings.add("userDataSettings", new Gson().toJsonTree(getUserDataSettings()));

        return settings.toString();
    }

    /**
     * Possible values of form's render mode
     */
    private enum FormRenderModeType {
        MODAL("modal"),
        INLINE("inline");
        private String value;

        FormRenderModeType(String value) {
            this.value = value;
        }

        @Override
        public String toString() {
            return value;
        }
    }

}
