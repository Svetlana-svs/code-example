Vue.component('user-profile-form', {
    props: ['componentSettings', 'renderMode', 'formState', 'formMessage'],
    inject: ['$validator'],
    data: function () {
        return {
            // Enum
            FormRenderModeType: {
                INLINE: "inline",
                MODAL: "modal"
            },
            FormStateType:   {
                 EDIT: "edit",
                 VIEW: "view"
            },
            FieldDisplayOption: {
                REQUIRED: "required",
                OPTIONAL: "optional",
                DISABLED: "disabled"
            },
            groups: {
                name: ["firstName", "middleName", "lastName"],
                common: ["phone", "birthDate", "gender"],
                address: ["city", "address", "addressLine"]
            },
            radio: {
                gender: ["male", "female"]
            },
             // Class
            UserDataField: function(type, name, value, displayOption, validationRules, image) {
                this.type = type;
                this.name = name;
                this.value = value;
                // Field display option of the FieldDisplayOption type
                this.displayOption = displayOption;
                // Properties for radio buttons data
                this.validationRules = validationRules;
                this.image = image
            },
            formRenderMode: "modal",
            userDataUpdated: false,
            formData: {
                brandId: ABBOTT.MainNamespace.Utils.getBrandId(),
                firstName: "",
                middleName: "",
                lastName: "",
                phone: "",
                gender: "",
                birthDate: "",
                country: "",
                city: "",
                address: ""
            },
            userData: {},
            images: null,
            // Validation error messages dictionary
            validationDictionary: {
                 custom: {}
            },
            validationDictionaryLoaded: false,
            titleButtonSubmit: "",
            titleButtonCancel: "",
        }
    },

    mounted: function() {
        that = this;
        this.formRenderMode = this.componentSettings.formRenderMode;
        this.titleButtonSubmit = ABBOTT.MainNamespace.I18n.get("user.profile.form.button.submit");
        this.titleButtonCancel = ABBOTT.MainNamespace.I18n.get("user.profile.form.button.cancel");

        // Set fields specific options
        if ((this.componentSettings.imageMaleURL || this.componentSettings.imageMaleURL)) {
            this.images = {
                gender: {
                    male: this.componentSettings.imageMaleURL,
                    female: this.componentSettings.imageFemaleURL
                }
            };
        }

        // Add event listeners
        if (this.formRenderMode == this.FormRenderModeType.MODAL) {
            const that = this;
            this.$root.$on('bv::modal::hidden', function(bvEvent, modalId) {
                that.cancelEdit();
            });
        }

    },

    created: function() {
        const that = this;

        const SERVER_BIRTH_DATE_FORMAT = "YYYY-MM-DD";
        const USER_BIRTH_DATE_FORMAT = 'DD-MM-YYYY';
        const language = document.documentElement.getAttribute("lang");

        // Array of the cookie designed for update
        this.cookieNames = ['firstName'];

        // Private functions
        this.getType = function(name) {
            let type = "text";
            switch (name) {
                case "gender":
                    type = "radio"
                break;
                case "email":
                    type = "email";
                break;
                default:
                    type = "text";
                break;
            }
            return type;
        };

        this.getValidationRules = function(name) {
            const validationRules = {};
            validationRules.required = (this.componentSettings.userDataSettings[name].displayOption === this.FieldDisplayOption.REQUIRED);

            if (this.componentSettings.userDataSettings[name].hasOwnProperty("validationRule")) {
                for (let [key, value] of Object.entries(this.componentSettings.userDataSettings[name].validationRule)) {
                    Object.assign(validationRules, { [key]: (value === "regex") ?  new RegExp(value) : value});
                };
            }

            return validationRules;
        };

        this.setValidationDictionary = function(name, validationRules) {
            const validationMessages = {};
            Object.keys(validationRules).forEach(function(key) {
                    Object.assign(validationMessages,
                        {[key]: ABBOTT.MainNamespace.I18n.get("user.profile.form.validation." + name.toLowerCase() + "." + key) });
                    });
            Object.assign(this.validationDictionary.custom, {
                [name]: validationMessages
            });
        };

        this.getValues = function(field) {
            if (field.name === "birthDate") {
                let birthDate =  moment(field.value, SERVER_BIRTH_DATE_FORMAT);
                if (birthDate.isValid()) {
                    field.value = birthDate.format(USER_BIRTH_DATE_FORMAT);
                }
            }

            return ((field.type === "radio") ? this.radio[field.name] : [field.value]);
        };

        this.addFieldAddressLine = function() {
            const addressLine = this.getAddressLine();
            if (addressLine) {
                Object.assign(this.userData, { ["addressLine"]: new this.UserDataField(
                    "",
                    "addressLine",
                    addressLine,
                    this.FieldDisplayOption.OPTIONAL,
                    []
                )});
                if (this.userData.city) {
                    this.userData.city.value = "";
                }
                if (this.userData.address) {
                    this.userData.address.value = "";
                }
            }
        };

        this.getAddressLine = function() {
            const arrayAddress = [
                this.userData.country ? this.userData.country.value : null,
                this.userData.city ? this.userData.city.value : null,
                this.userData.address ? this.userData.address.value : null];
            return arrayAddress.filter(function(element){
                return element;
            }).join(', ');
        };

        (this.updateUserData = function(userDataUpdateNeeded = false) {
            ABBOTT.MainNamespace.UserProfileController.getUserProfile(function success(data) {
                if (data.success) {
                    for (let [key, value] of Object.entries(data.userData)) {
                        if (that.componentSettings.userDataSettings[key] &&
                                that.componentSettings.userDataSettings[key].hasOwnProperty("displayOption") &&
                                that.componentSettings.userDataSettings[key].displayOption !== that.FieldDisplayOption.DISABLED) {

                            const field = new that.UserDataField(
                                that.getType(key),
                                key,
                                value,
                                that.componentSettings.userDataSettings[key].displayOption,
                                that.getValidationRules(key),
                                (that.images ? that.images[key] : null)
                            );
                            field.values = that.getValues(field);

                            that.$set(that.userData, key, field);
                            Object.assign(that.formData, { [key]: that.userData[key].value });
                       }
                    }

                    // Trigger event after update data
                    if (userDataUpdateNeeded) {
                        that.userDataUpdated = !that.userDataUpdated;
                    }

                    that.addFieldAddressLine();

                    // Set validation error messages dictionary
                    if (!that.validationDictionaryLoaded) {
                        Object.keys(that.userData).forEach(function(key) {
                            that.setValidationDictionary(key, that.userData[key].validationRules);
                        });
                        that.$validator.localize('en', that.validationDictionary);
                        that.validationDictionaryLoaded = true;
                    }
                } else {
                    that.message.error = ABBOTT.MainNamespace.I18n.get("error." + ((data && data.errorCode) ? data.errorCode : "unknown"));
                }
            },
            function fail(data) {
                that.message.error = ABBOTT.MainNamespace.I18n.get("error.server.internal");
            })
        })();

        this.submitForm = function() {
            axios({
                method: "post",
                url: "/bin/abbott/updateUserProfile",
                params: this.formData
            })
            .then(function (response) {
                if (response && response.data) {
                    const data = response.data;
                    if (data.success) {
                        ABBOTT.MainNamespace.UserProfileController.resetCache();
                        that.message.success = ABBOTT.MainNamespace.I18n.get("user.profile.form.submit.success");
                        that.updateUserData(true);
                        if (!that.isFormRenderModeModal) {
                            that.closeFormEdit();
                        }
                    } else {
                        that.message.error = ABBOTT.MainNamespace.I18n.get("error." + ((data && data.errorCode) ? data.errorCode : "unknown"));
                    }
                } else {
                    that.message.error = ABBOTT.MainNamespace.I18n.get("error.server.internal");
                }
            })
            .catch(function (error) {
                if (error && error.response) {
                    const data = error.response.data;
                    that.message.error = ABBOTT.MainNamespace.I18n.get("error." + ((data && data.errorCode) ? data.errorCode : "unknown"));
                } else {
                    that.message.error = ABBOTT.MainNamespace.I18n.get("error.server.internal");
                }
            });
        };

        this.closeFormEdit = function() {
            this.state = this.FormStateType.VIEW;
        };

        this.clearForm = function() {
            this.message.success = "";
            this.message.error = "";
            this.$validator.reset();
        }
    },

    computed: {
        state: {
            get() {
                return this.formState;
            },
            set(value) {
                this.$emit('update:formState', value);
            }
        },

        message: {
            get() {
                return this.formMessage;
            },
            set(value) {
                this.$emit('update:formMessage', value.error);
                this.$emit('update:formMessage', value.success);
            }
        },

        isFormStateView: function() {
            return this.state === this.FormStateType.VIEW;
        },

        isFormRenderModeModal: function() {
            return (this.componentSettings.formRenderMode === this.FormRenderModeType.MODAL);
        },

        generalMessageEnabled: function() {
            return (this.renderMode === this.FormRenderModeType.MODAL)
        },

        isUserDataEmpty: function() {
            return (Object.keys(this.userData).length === 0);
        }
    },

    methods: {
        fieldEnabled: function(fieldName) {
            return (this.userData && this.userData[fieldName]);
        },

        getGroupLabel: function(groupName) {
            return ABBOTT.MainNamespace.I18n.get("user.profile.form.group." + groupName.toLowerCase());
        },

        groupEnabled: function(groupName) {
            if (!this.userData) {
                return false;
            }

            const that = this;
            let result = false;
            this.groups[groupName].some(function(fieldName) {
                if (that.userData[fieldName]) {
                    result = true;
                    return;
                }
            });
            return result;
        },

        cancelEdit: function() {
            this.clearForm();
            if (this.isFormRenderModeModal) {
                this.$emit('exit', true);
            }
            this.state = this.FormStateType.VIEW;
       },

        handleSubmit: function(event) {
            event.preventDefault();

            const that = this;
            this.$validator.validateAll().then(function(valid)  {
                if (valid) {
                    that.submitForm();
                    return;
                }
            });
        }
    },

    watch: {
        userDataUpdated: function() {
            const that = this;
            this.cookieNames.forEach(function(name) {
                const newValue = (that.userData[name] ? that.userData[name].value : "");
                ABBOTT.MainNamespace.UserProfileController.updateUserDataCache(name, newValue);
            });
            // Update user name in header greetings
            ABBOTT.MainNamespace.Mediator.publish(ABBOTT.MainNamespace.Mediator.channels.USER_DATA_UPDATED);
        },

        formState: function() {
            this.updateUserData();
        }
    },

    template:
        `<div>
             <div class="messages user-profile-messages">
                 <div class="message message--success [ alert alert-success ]"
                      v-show="message.success && generalMessageEnabled"
                       v-cloak>
                     {{ message.success }}
                 </div>
                 <div class="message message--error [ alert alert-danger ]"
                      v-show="message.error && generalMessageEnabled"
                      v-cloak>
                     {{ message.error }}
                 </div>
             </div>

             <form class="form-horizontal"
                   @submit.prevent="handleSubmit"
                   v-show="!isUserDataEmpty"
                   v-cloak>

                 <div v-for="(group, groupName) in groups">
                     <label v-show="groupEnabled(groupName)">
                         {{ getGroupLabel(groupName) }}
                     </label>
                     <div v-for="fieldName in group">
                         <user-profile-field ref="user-profile-form"
                                 v-if='fieldEnabled(fieldName)'
                                 v-model="formData[fieldName]"
                                 :field='userData[fieldName]'
                                 :form-render-mode='formRenderMode'
                                 :render-mode='renderMode'
                                 :form-state='state'>
                         </user-profile-field>
                     </div>
                 </div>

                 <div class="user-profile__item form-group button-panel"
                      v-show="!isFormStateView">
                     <button class="[ btn ] btn_cancel"
                             type="button"
                             @click="cancelEdit">
                         <span class="glyphicon glyphicon-pencil"></span>
                         <span>{{ titleButtonCancel }}</span>
                     </button>

                     <button class="[ btn btn-primary btn-lg ] btn_save"
                             :disabled="errors.any()"
                             type="submit">
                         <span class="glyphicon glyphicon-ok"></span>
                         <span>{{ titleButtonSubmit }}</span>
                     </button>

                 </div>
             </form>
         </div>`
});
