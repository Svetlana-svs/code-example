/**
 * @author Sergei Kharitonov
 * @lastEdit Sergei Kharitonov
 */

QB.QueryFilterController = (function() {

    let formData = {};
    let formId = "";

    function init() {
        const urlProps = window.location.pathname.split("/");
        formId = urlProps[4];
        loadFormData();
        initEventListeners();
    }

    function loadFormData() {
        axios({
            method: "post",
            url: QB.getContextPath() + "/api/userForm/filter/" + formId
        })
            .then(function(response) {
                if (response && response.data) {
                    formData = response.data;
                    includeCustomCss(formData.cssFile);
                    document.getElementById("query-title").innerHTML = formData.info.name;
                    drawViewElements();
                }
            })
            .catch(function(error) {
                console.log(error);
                showNotification("error", "Error loading query form");
            })
            .finally(function() {
                let spinners = document.querySelectorAll("spinner-border");
                for (let i=0; i<spinners.length; i++) {
                    spinners[i].parentElement.removeChild(spinners[i]);
                }
            });
    }

    function drawViewElements() {
        //handlebars helpers
        Handlebars.registerHelper('isSelect', function (fieldId) {
            return fieldId == "select";
        });
        Handlebars.registerHelper('isMultichoice', function (fieldId) {
            return fieldId == "multiplechoice";
        });
        Handlebars.registerHelper('isRadio', function (fieldId) {
            return fieldId == "radio";
        });
        Handlebars.registerHelper('isCheckbox', function (fieldId) {
            return fieldId == "checkbox";
        });
        Handlebars.registerHelper('isYesNo', function (fieldId) {
            return fieldId == "yesno";
        });
        Handlebars.registerHelper('isInput', function (fieldId) {
            return fieldId == "input";
        });
        Handlebars.registerHelper('isSoundex', function (fieldId) {
            return fieldId == "soundex";
        });
        Handlebars.registerHelper('isRTF', function (fieldId) {
            return fieldId == "rtf";
        });
        Handlebars.registerHelper('isButton', function (fieldId) {
            return fieldId == "button";
        });

        Handlebars.registerHelper('isExpression', function (field) {
            if(field.metadata.type == "W" &&
                field.data.length == 1 &&
                field.data[1] != "") {
                return true;
            }
            return false;
        });

        Handlebars.registerHelper('getMultichoiceSize', function (data) {
            if (data.length >= 15) {
                return 15
            } else {
                return data.length;
            }
            return "";
        });

        Handlebars.registerHelper('getFieldTitle', function (metadata, settings) {
            if (settings.title != "") {
                return settings.title;
            } else {
                return metadata.title;
            }
            return "";
        });

        Handlebars.registerHelper('getButtonTitle', function (settings) {
            if (settings.title != "") {
                return settings.title;
            } else if (settings.id == "field-submit-button") {
                return "Submit";
            } else if (settings.id == "field-reset-button") {
                return "Reset";
            }
            return "Button";
        });

        Handlebars.registerHelper('getOptionValue', function (fieldType, data, id) {
            if (fieldType == "S") {
                for (let i=0; i < data.length; i++) {
                    if (data[i].id == id) {
                        return data[i].value;
                    }
                }
                return 0;
            } else {
                return id;
            }
        });

        const filterContainer = document.getElementById("query-filter-fields-container");
        let filterTemplateContent = document.querySelectorAll("[data-container=filter-template]")[0].innerHTML;
        let filterTemplate = Handlebars.compile(filterTemplateContent);
        const filterFieldsHTML = filterTemplate(formData);
        filterContainer.innerHTML = filterFieldsHTML;
    }

    function initEventListeners() {

        $('body').on('click', '.field-submit-button', function (event) {
            const searchParams = getUserInputAsString();

            window.location.href = QB.getContextPath() + "/userForm/list/" + formId +
                "?data=" + encodeURI(searchParams);
        });

        $('body').on('click', '.field-reset-button', function (event) {
            let selects = document.getElementsByTagName("select");
            for (let i=0; i < selects.length; i++) {
                let options = selects[i].children;
                for (let j=0; j < options.length; j++) {
                    options[j].selected = false;
                }
            }

            let inputs = document.getElementsByTagName("input");
            for (let i=0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox" || inputs[i].type == "radio") {
                    inputs[i].checked = false;
                } else {
                    inputs[i].value = "";
                }
            }
        });

        $('#query-filter-fields-container').on('click', '.toggle-select-all', function (event) {
            //select-all handler
            let container = event.target.parentElement.parentElement;
            let checkboxes = container.getElementsByTagName("input");
            for (let i=0; i < checkboxes.length; i++) {
                if (checkboxes[i].type = "checkbox") {
                    checkboxes[i].checked = true;
                }
            }

            let options = container.getElementsByTagName("option");
            for (let i=0; i < options.length; i++) {
                options[i].selected = true;
            }
        });

        $('#query-filter-fields-container').on('click', '.toggle-deselect-all', function (event) {
            let container = event.target.parentElement.parentElement;
            let checkboxes = container.getElementsByTagName("input");
            for (let i=0; i < checkboxes.length; i++) {
                if (checkboxes[i].type = "checkbox") {
                    checkboxes[i].checked = false;
                }
            }

            let options = container.getElementsByTagName("option");
            for (let i=0; i < options.length; i++) {
                options[i].selected = false;
            }
        });
    }

    function getUserInputAsString() {
        const filterContainer = document.getElementById("query-filter-fields-container");
        let filterFields = filterContainer.children;
        let paramObject = {};

        for (let i=0; i < filterFields.length; i++) {
            let field = filterFields[i];
            let name = field.id;
            let value = [];

            if (field.getElementsByTagName("select").length > 0) {
                //handle selects
                let element = field.getElementsByTagName("select")[0];
                let selectedElements = [];
                for (let j=0; j< element.children.length; j++) {
                    if(element.children[j].selected) {
                        selectedElements.push(element.children[j].value)
                    }
                }
                value = selectedElements;

            } else if (field.getElementsByTagName("input").length > 0 &&
                field.getElementsByTagName("input")[0].type == "checkbox") {
                //handle checkboxes
                let elements = field.getElementsByTagName("input");
                let selectedElements = [];

                for (let j = 0; j < elements.length; j++) {
                    if (elements[j].checked) {
                        selectedElements.push(elements[j].value);
                    }
                }
                if (JSON.stringify(selectedElements)==JSON.stringify(["true","false"])) {
                    selectedElements = [];
                }

                value = selectedElements;

            } else if (field.getElementsByTagName("input").length > 0 &&
                field.getElementsByTagName("input")[0].type == "radio") {
                //handle radiobuttons
                let elements = field.getElementsByTagName("input");
                let selectedElements = [];
                for (let j=0; j< elements.length; j++) {
                    if(elements[j].checked) {
                        selectedElements.push(elements[j].value);
                        break;
                    }
                }
                value = selectedElements;

            } else if(field.getElementsByTagName("input")[0] != undefined) {
                //handle textinputs
                value = [];
                let element = field.getElementsByTagName("input")[0];
                value.push(element.value);
            }

            if (name != undefined && name != "") {
                paramObject[name] = value;
            }
        }
        return JSON.stringify(paramObject);
    }

    function showNotification(type, text) {
        const notificationContainer = document.getElementById("notification-container");
        if (notificationContainer != undefined) {
            notificationContainer.getElementsByClassName("alert-success")[0].style.display = "none";
            notificationContainer.getElementsByClassName("alert-danger")[0].style.display = "none";

            if (type == "success") {
                notificationContainer.getElementsByClassName("alert-success")[0].innerHTML = text;
                notificationContainer.getElementsByClassName("alert-success")[0].style.display = "block";
            } else if (type == "error") {
                notificationContainer.getElementsByClassName("alert-danger")[0].innerHTML = text;
                notificationContainer.getElementsByClassName("alert-danger")[0].style.display = "block";
            }
        }
    }

    function includeCustomCss(fileName) {
        let link = document.createElement('link');
        link.rel = 'stylesheet';
        link.type = 'text/css';
        link.href = QB.getContextPath() + "/ext/" + fileName;
        document.getElementsByTagName('head')[0].appendChild(link);
    }

    return {
        init: init
    };
})();
