UUU.MainNamespace.RegistrationController = (function() {
    "use strict"

    const PARAMETER_LOGIN_MODAL_OPEN = "autoLogin";

    let component = null;
    let messageSuccess = null;
    let messageError = null;

    let form = null;
    let buttonSubmit = null;

    function submitForm(event) {
        event.preventDefault();

        UUU.MainNamespace.AbstractComponent.clearMessage(messageSuccess, messageError);
        UUU.MainNamespace.AnimationController.toggleButton(buttonSubmit);
        const formData = UUU.MainNamespace.AbstractComponent.getFormData(form);

        axios({
            method: "post",
            url: "/bin/uapps/registration",
            params: formData
        })
        .then(function (response) {
            if (response && response.data) {
                const data = response.data;
                if (data.success) {
                    messageSuccess.textContent = UUU.MainNamespace.I18n.get("user.registration.form.submit.success");
                    messageSuccess.style.display = "";
                } else {
                    messageError.textContent = UUU.MainNamespace.I18n.get("error." + ((data && data.errorCode) ? data.errorCode : "unknown"));
                    messageError.style.display = "";
                    UUU.MainNamespace.AnimationController.toggleButton(buttonSubmit);
                }
            } else {
                messageError.textContent = UUU.MainNamespace.I18n.get("error.server.internal");
                messageError.style.display = "";
                UUU.MainNamespace.AnimationController.toggleButton(buttonSubmit);
            }
        })
        .catch(function (error) {
            if (error && error.response) {
                const data = error.response.data;
                messageError.textContent = UUU.MainNamespace.I18n.get("error." + ((data && data.errorCode) ? data.errorCode : "unknown"));
            } else {
                messageError.textContent = UUU.MainNamespace.I18n.get("error.server.internal");
            }
            messageError.style.display = "";
            UUU.MainNamespace.AnimationController.toggleButton(buttonSubmit);
        })
    }

    function hasHashParam(param) {
        const match = location.hash.match(new RegExp("[#&]{1}" + param + "[=&]?"));
        return !!match;
    }

    function initElements() {
        // Set DOM elements
        component = document.getElementById("registrationComponent");
        if (component === null) {
            console.error("One of the necessary DOM elements not found");
            return;
        }
        messageSuccess = component.querySelector(".message--success");
        messageError = component.querySelector(".message--error");

        form = component.querySelector("form");
        buttonSubmit = component.querySelector(".btn[type=submit]");

        let phone = component.querySelector("input[name=phone]");
        if (phone !== null) {
            vanillaTextMask.maskInput({
                inputElement: phone,
                mask: ["+", "7", "(", /[1-9]/, /\d/, /\d/, ")", /\d/, /\d/, /\d/, "-", /\d/, /\d/, "-", /\d/, /\d/]
            });
        }
    }

    function initEventListeners() {

        form.addEventListener("submit", submitForm);

        // Set event listeners for all data input fields
        const inputFields = component.querySelectorAll("input,textarea");
        [].forEach.call(inputFields, function(field) {
            field.addEventListener("input", function (event) {
                UUU.MainNamespace.AbstractComponent.handleInputEventListener(field);
            });

            field.addEventListener("invalid", function (event) {
                UUU.MainNamespace.AbstractComponent.handleInvalidEventListener(field);
            });
        });
    }

    function init() {

        initElements();
        initEventListeners();

        if (hasHashParam(PARAMETER_LOGIN_MODAL_OPEN)) {
            UUU.MainNamespace.CookieController.isUserLoggedIn(function(data) {
                if (!data.success)  {
                    UUU.MainNamespace.Mediator.publish(UUU.MainNamespace.Mediator.channels.LOGIN_MODAL_OPEN);
                }
            });
        }
    }

    return {
        init: init
    }
})();