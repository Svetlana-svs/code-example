Vue.component('user-profile-field', {
    props: ['field', 'formRenderMode', 'formState', 'renderMode', 'value'],
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
            }
        };
    },

    mounted: function() {
        if (this.field.name === "phone") {
            const phone = this.$el.querySelector("input");
            if (phone !== null) {
                this.$nextTick(function() {
                    vanillaTextMask.maskInput({
                        inputElement: phone,
                        mask: ["+", "7", "(", /[1-9]/, /\d/, /\d/, ")", /\d/, /\d/, /\d/, "-", /\d/, /\d/, "-", /\d/, /\d/]
                    });
                });
            }
        }

        if (this.field.name === "birthDate") {
            const date = this.$el.querySelector("input");
            // TODO: set user date format in dependency for language
            const USER_DATE_FORMAT = 'd-m-Y';
            const language = document.documentElement.getAttribute("lang");

            if (date !== null) {
                this.$nextTick(function() {
                    flatpickr(date, {
                        dateFormat: USER_DATE_FORMAT,
                        allowInput: true,
                        defaultDate: "02.02.2000"
                    });
                });
            }
        }
    },

    methods: {
        isInputRadio: function(value) {
            return (this.field.type === 'radio') &&
                ((this.renderMode === this.FormRenderModeType.MODAL) ||
                ((this.renderMode === this.FormRenderModeType.INLINE) &&
                ((this.field.value === value) ||
                ((this.formState === this.FormStateType.EDIT) && (this.formRenderMode === this.FormRenderModeType.INLINE)))));
        },

        isChecked: function(value) {
            return (this.field.type === "radio" && this.field.value === value) ? true : null;
        },

        isImageEmpty: function(value) {
            return (this.field.type === 'radio');
        },

        getImageAlt: function(value) {
            return value ? ABBOTT.MainNamespace.I18n.get("user.profile.form." + this.field.name.toLowerCase() + "." +
                    value.toLowerCase() + ".alt") : "";
        },

        getImageTitle: function(value) {
            return value ? ABBOTT.MainNamespace.I18n.get("user.profile.form." + this.field.name.toLowerCase() + "." +
                    value.toLowerCase() + ".title") : "";
        },

        getImageSrc: function(value) {
            return (this.field.image !== null && value &&  this.field.image[value]) ? this.field.image[value] : "";
        },

        getLabelRatio:  function(value) {
            if (this.isInputShow) {
                return ABBOTT.MainNamespace.I18n.get("user.profile.form.edit." + this.field.name.toLowerCase() + "." + value + ".label");
            } else {
                return ABBOTT.MainNamespace.I18n.get("user.profile.form.view." +  this.field.name.toLowerCase() + "." + value);
            }
        },

        getValue: function(value) {
            return (this.field.type === "radio") ? value : this.value;
        }
    },

    computed: {
        state: function() {
            return this.formState;
        },

        isElementRender: function() {
            return (this.renderMode === this.FormRenderModeType.INLINE);
        },

        isInputRender: function() {
            return (this.renderMode === this.FormRenderModeType.MODAL) ||
                    (this.formRenderMode === this.FormRenderModeType.INLINE);
        },

        isElementShow: function() {
             return (this.renderMode === this.FormRenderModeType.INLINE) &&
              ((this.formRenderMode === this.FormRenderModeType.MODAL) || (this.formState === this.FormStateType.VIEW));
        },

        isInputShow: function() {
             return (this.renderMode === this.FormRenderModeType.MODAL) || (this.formState === this.FormStateType.EDIT);
        },

        isElementRadio:  function() {
            return  (this.field.type === "radio");
        },

        rules: function() {
            return {rules:this.field.validationRules};
        },

        label:  function() {
            if (this.isInputShow) {
                return ABBOTT.MainNamespace.I18n.get("user.profile.form.edit." + this.field.name.toLowerCase() + ".label");
            } else {
                return ABBOTT.MainNamespace.I18n.get("user.profile.form.view." + this.field.name.toLowerCase());
            }
        },

        placeholder:  function() {
            return ABBOTT.MainNamespace.I18n.get("user.profile.form.edit." + this.field.name.toLowerCase() + ".placeholder");
        }
    },

    template:
        `<div class="user-table__item form-group">
            <label class="control-label">{{ label || ""}}</label>
            <span v-if="isElementRender"
                  v-show="isElementShow && !isElementRadio">
                {{ field.value }}
            </span>
            <div v-for="val in field.values">
                <input class="form-control"
                       v-if="isInputRender"
                       v-show="isInputShow"
                       :name="field.name"
                       :type="field.type"
                       :value="getValue(val)"
                       v-validate="rules"
                       :checked="isChecked(val)"
                       :placeholder="placeholder"
                       @input="$emit('input', $event.target.value)"/>
                <label class="control-label" for=""
                        v-if="isInputRadio(val)">
                    <img v-if="field.image"
                            :title="getImageTitle(val)"
                            :alt="getImageAlt(val)"
                            :src="getImageSrc(val)"/>
                    <span v-if="!field.image">{{ getLabelRatio(val) || "" }}</span>
                </label>
            </div>
            <span class="error"
                  v-show="isInputShow && errors.has(field.name)" >
                {{ errors.first(field.name) }}
            </span>
        </div>`
})