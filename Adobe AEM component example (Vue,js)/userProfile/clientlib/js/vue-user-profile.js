Vue.component('user-profile', {
    props: ['componentSettings'],
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
            message: {
                error: "",
                success: ""
            },
            titleFormModal: "",
            titleButtonSubmit: "",
            titleButtonCancel: "",
            titleButtonFormEdit: "",
            formState: "view"
         }
    },

    mounted: function() {
        this.formRenderMode = this.componentSettings.formRenderMode;
        this.titleFormModal = ABBOTT.MainNamespace.I18n.get("user.profile.form.edit.heading");
        this.titleButtonSubmit = ABBOTT.MainNamespace.I18n.get("user.profile.form.button.submit");
        this.titleButtonFormEdit = ABBOTT.MainNamespace.I18n.get("user.profile.button.openformedit");
    },

    created: function() {
        this.titleButtonCancel = ABBOTT.MainNamespace.I18n.get("user.profile.form.button.cancel");
        this.clearForm = function() {
            this.message.success = "";
            this.message.error = "";
        }
    },

    computed: {
        isFormStateView: function() {
            return this.formState === this.FormStateType.VIEW;
        },

        isFormRenderModeModal: function() {
            return (this.componentSettings.formRenderMode === this.FormRenderModeType.MODAL);
        },

        generalMessageEnabled: function() {
            return this.isFormStateView || !this.isFormRenderModeModal;
        },

        isUserDataEmpty: function() {
        //           return (Object.keys(this.userData).length === 0);
            const t = this.$refs['user-profile-form'];
            return false;
        },

        userProfileHeading: function() {
            const defaultHeading = ABBOTT.MainNamespace.I18n.get("user.profile.form.view.heading");
            return (this.componentSettings.heading ? this.componentSettings.heading : defaultHeading);
        }
    },

    methods: {
        openFormEdit: function() {
            this.clearForm();
            this.$set( this, "formState", this.FormStateType.EDIT);
            if (this.isFormRenderModeModal) {
                this.$refs['user-profile-modal'].show();
            }
        },

        handleSubmit: function(event) {
            this.$refs['user-profile-form-modal'].handleSubmit(event);
        },

        closeModal: function() {
            this.$refs['user-profile-modal'].hide();
        }
    },

    template:
        `<div>
             <h2>{{ userProfileHeading }}</h2>

             <div class="component-desc"
                  v-if="componentSettings.description">
                <p>{{ componentSettings.description }}</p>
             </div>

             <div class="component-body">
                 <div class="messages user-profile-messages">
                     <div class="message message--success [ alert alert-success ]"
                          v-show="message.success && generalMessageEnabled">
                         {{ message.success }}
                     </div>
                     <div class="message message--error [ alert alert-danger ]"
                          v-show="message.error && generalMessageEnabled"
                          v-cloak>
                         {{ message.error }}
                     </div>
                     <user-profile-form ref="user-profile-form"
                             :component-settings='componentSettings'
                             :render-mode='FormRenderModeType.INLINE'
                             :form-state.sync="formState"
                             :form-message.sync="message"
                     ></user-profile-form>
                 </div>
                 <div class="user-profile__item form-group button-panel"
                      v-show="!isUserDataEmpty"
                      v-cloak>
                     <button class="[ btn ] btn_edit"
                             type="button"
                             v-show="isFormStateView"
                             @click="openFormEdit">
                         <span class="glyphicon glyphicon-pencil"></span>
                         <span>{{ titleButtonFormEdit }}</span>
                     </button>
                 </div>

                 <b-modal ref="user-profile-modal"
                          v-if='isFormRenderModeModal'
                          :title='titleFormModal'
                          :hide-footer='true'>

                     <user-profile-form ref="user-profile-form-modal"
                             :component-settings='componentSettings'
                             :render-mode='FormRenderModeType.MODAL'
                             :form-state.sync="formState"
                             :form-message.sync="message"
                             @exit="closeModal">
                     </user-profile-form>
                 </b-modal>
             </div>
         </div>`
});
