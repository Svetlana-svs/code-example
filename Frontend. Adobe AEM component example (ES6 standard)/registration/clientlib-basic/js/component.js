UUU.MainNamespace.AbstractComponent = (function() {
    "use strict"

    const DEFAULT_ERROR_KEY = "error.server.internal";
    const DEFAULT_DATE_FORMAT = "YYYY-MM-DD HH:mm:ss";

    const CLASSNAME_ERROR = "error";

    const brandId = UUU.MainNamespace.Utils.getBrandId();

    // Private functions
    function handleCheckboxFieldError(field, customValidity) {
        if (!field.validity.valid) {
            field.parentNode.classList.toggle(CLASSNAME_ERROR);
            field.setCustomValidity(customValidity ? " " : "");
        }
    }

    function handleRadioFieldError(field, customValidity) {
        if (!field.validity.valid) {
            const fields = field.parentNode.parentNode.querySelectorAll("input[type=radio]");
            [].forEach.call(fields, function(radio) {
                radio.parentNode.toggle(CLASSNAME_ERROR);
            });
            field.setCustomValidity(customValidity ? " " : "");
        }
    }

    function handleTextFieldError(field, customValidity) {
        if (!field.validity.valid) {

            const labelError = field.parentNode.querySelector("label.error");
            if (customValidity) {
                const messageId = "message-" +
                        ((field.validity.patternMismatch || field.validity.typeMismatch) ? "pattern" : "required");
                if (labelError !== null) {
                    if (field.hasAttribute(messageId)) {
                        labelError.innerHTML = field.getAttribute(messageId);
                    }
                    labelError.style.display = "";
                }
            } else {
                if (labelError !== null) {
                    labelError.innerHTML = "";
                    labelError.style.display = "none";
                }
            }

            field.classList.toggle(CLASSNAME_ERROR);
            field.setCustomValidity(customValidity ? " " : "");
        }
    }

    // Public functions
    function handleInputEventListener(field) {
        if (field === null) {
            return;
        }
        switch (field.type) {
            case "checkbox":
                handleCheckboxFieldError(field, false);
            break;
            case "radio":
                handleRadioFieldError(field, false);
            break;
            default:
                if (!field.validity.patternMismatch && !field.validity.typeMismatch && !field.validity.valueMissing) {
                    handleTextFieldError(field, false);
                } else if (field.validity.patternMismatch || field.validity.typeMismatch) {
                    handleTextFieldError(field, true);
                }
            break;
        }
    }

    function handleInvalidEventListener(field) {
        if (field === null) {
            return;
        }
        switch (field.type) {
            case "checkbox":
                handleCheckboxFieldError(field, true);
            break;
            case "radio":
                handleRadioFieldError(field, true);
            break;
            default:
                handleTextFieldError(field, true);
            break;
        }
    }

    function getFormData(form) {
        const formData = (form == null)? new FormData() : new FormData(form);
        const data = {};

        for (let [key, val] of formData.entries()) {
            Object.assign(data, { [key]: val });
        }
        data.brandId = brandId;

        return data;
    }

    function clearMessage(messageSuccess, messageError) {
        if (messageSuccess !== null) {
            messageSuccess.style.display = "none";
            messageSuccess.textContent = "";
        }
        if (messageError !== null) {
            messageError.style.display = "none";
            messageError.textContent = "";
        }
    }

    return {
        handleInputEventListener : handleInputEventListener,

        handleInvalidEventListener : handleInvalidEventListener,

        getFormData: getFormData,

        clearMessage: clearMessage
    };
})();