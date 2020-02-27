/**
 * @author Sergei Kharitonov
 * @lastEdit Sergei Kharitonov
 */

QB.QueryFormBuilderController = (function() {

    //semi-globals generally needed everywhere
    let fieldList = {};
    let cssFileList = [];
    let userList = {};
    let formData = {};

    function init() {
        loadFormData();
        initEventListeners();
    }

    function loadFormData() {
        //get form data if edit
        const urlProps = window.location.pathname.split("/");
        if(urlProps[3] == "edit") {
            if (getUrlVars()["newQueryEdit"] == "true") {
                showNotification("success", "Your query has been saved successfully")
            }
            document.getElementById("form-id").value = urlProps[4];

            axios({
                method: "post",
                url: QB.getContextPath() + "/api/queryForm/get/" + urlProps[4],
            })
                .then(function(response) {
                    if (response && response.data) {
                        formData = response.data;
                        //general, non-so-dymnamic fields
                        if (Object.entries(formData).length > 0) {
                            document.getElementById("connection-mdb").value = formData.info.connection.mdb;
                            document.getElementById("connection-sdb").value = formData.info.connection.sdb;
                            document.getElementById("database").value = formData.info.database;
                            document.getElementById("table-id").value = formData.info.tableId;
                            document.getElementById("title-input").value = formData.info.name;
                            document.getElementById("comment-input").value = formData.info.comment;
                            document.getElementById("database-header").innerHTML = "Datei: " + formData.info.connection.mdb;
                            document.getElementById("errorMessage-input").value = formData.errorMessage;
                            document.getElementById("show-count").checked = formData.resultOptions.count;
                            document.getElementById("show-numbers").checked = formData.resultOptions.numbers;
                            document.getElementById("show-criteria").checked = formData.resultOptions.criteria;
                            let radios = document.getElementsByName('additional-info');
                            for (let i = 0, length = radios.length; i < length; i++) {
                                if (radios[i].value == formData.additionalInfo) {
                                    radios[i].checked = "checked";
                                    break;
                                }
                            }
                            document.getElementById("startpoint-select").value = formData.startPoint;
                            document.getElementById("cssfile-select").value = formData.cssFile;

                            document.getElementById(formData.startPoint + "-tab").click();
                        } else {
                            showNotification("error", "Form data is empty");
                        }
                        loadUserList(formData.info.users);
                        loadFields();
                        loadCssFileList();

                    }
                    else {
                        console.log(response);
                        showNotification("error", "Form data is empty");
                    }
                })
                .catch(function(error) {
                    console.log(error);
                    showNotification("error", "Error loading form data from server");
                })
                .finally(function() {});

        } else {
            formData.cssFile = "default.css";
            document.getElementById("table-id").value = getUrlVars()["table"];
            loadUserList([]);
            loadFields();
            loadCssFileList();
        }
    }

    function initFields() {

        //init handlebars helpers(aka methods)
        //register HandleBars helpers
        Handlebars.registerHelper('fieldTextByTypeId', function (fieldId) {
            if (fieldId == "select") {
                return "Auswahlliste";
            } else if (fieldId == "multiplechoice") {
                return "Mehrfachuswahl";
            } else if (fieldId == "radio") {
                return "Radio Buttons";
            } else if (fieldId == "checkbox") {
                return "Checkboxen";
            } else if (fieldId == "yesno") {
                return "Ya/Nein-Field";
            } else if (fieldId == "input") {
                return "Eingabefeld";
            } else if (fieldId == "soundex") {
                return "Soundex-Field";
            } else if (fieldId == "button") {
                return "Button";
            } else if (fieldId == "info") {
                return "Info";
            } else if (fieldId == "rtf") {
                return "RTF-Field";
            } else {
                return "";
            }
        });

        Handlebars.registerHelper('getDefaultISWType', function (type) {
            if (type == "I") {
                return "Auswahlliste";
            } else if (type == "S") {
                return "Auswahlliste";
            } else if (type == "W") {
                return "Eingabefeld";
            };
        });

        Handlebars.registerHelper('getInputLabelText', function (metadata, settings) {
            if (metadata.title == null ||  metadata.title == "null") {
                if (settings.id == "field-submit-button") {
                    return "Submit Button Title:";
                } else if (settings.id == "field-reset-button") {
                    return "Reset Button Title:";
                }
            } else {
                return metadata.title;
            }
            return "";
        });

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

        Handlebars.registerHelper('isIType', function (type) {
            return type == "I";
        });
        Handlebars.registerHelper('isSType', function (type) {
            return type == "S";
        });
        Handlebars.registerHelper('isWType', function (type) {
            return type == "W";
        });

        Handlebars.registerHelper('isDefaultCss', function (fileName) {
            return fileName == "default.css";
        });

        Handlebars.registerHelper('isSelectedField', function (value, data) {
            if (data != null && data != undefined) {
                return data.includes(value.toString());
            } else {
                return false;
            }
        });

        Handlebars.registerHelper('getAliasReference', function (alias) {
            return "#" + alias;
        });

        Handlebars.registerHelper('getLeftEq', function (data) {
            if (data != null) {
                if (data.length == 1) {
                    return splitExpression(data[0])[0];
                }
            }
            return "";
        });

        Handlebars.registerHelper('getRightEq', function (data) {
            if (data != null) {
                if (data.length == 1) {
                    return splitExpression(data[0])[2];
                }
            }
            return "";
        });

        Handlebars.registerHelper('isME', function (data) {
            return (data != null && data.length == 1 && splitExpression(data[0])[1] == ">=");
        });

        Handlebars.registerHelper('isLE', function (data) {
            return (data != null && data.length == 1 && splitExpression(data[0])[1] == "<=");
        });

        Handlebars.registerHelper('isNE', function (data) {
            return (data != null && data.length == 1 && splitExpression(data[0])[1] == "!=");
        });

        Handlebars.registerHelper('isE', function (data) {
            return (data != null && data.length == 1 && splitExpression(data[0])[1] == "=");
        });

        Handlebars.registerHelper('isM', function (data) {
            return (data != null && data.length == 1 && splitExpression(data[0])[1] == ">");
        });

        Handlebars.registerHelper('isL', function (data) {
            return (data != null && data.length == 1 && splitExpression(data[0])[1] == "<");
        });

        const fieldSelectContainer = document.getElementById("field-select-container");
        let fieldListContent = document.querySelectorAll("[data-container=fields-bar-template]")[0].innerHTML;
        let fieldListTemplate = Handlebars.compile(fieldListContent);
        const fieldListHTML = fieldListTemplate(fieldList);
        fieldSelectContainer.innerHTML = fieldListHTML;

        //worling area
        const fieldsFilterContainer = document.getElementById("selectedFields-filter");
        const fieldsListContainer = document.getElementById("selectedFields-list");
        const fieldsDetailContainer = document.getElementById("selectedFields-detail");
        let fieldTemplateContent = document.querySelectorAll("[data-container=workingarea-field-template]")[0].innerHTML;
        let fieldTemplate = Handlebars.compile(fieldTemplateContent);

        let templateOptions = {};
        templateOptions.fields = formData.filter;
        const filterFieldsHTML = fieldTemplate(templateOptions);
        templateOptions.fields = formData.list;
        const listFieldsHTML = fieldTemplate(templateOptions);
        templateOptions.fields = formData.detail;
        const detailFieldsHTML = fieldTemplate(templateOptions);

        fieldsFilterContainer.innerHTML = filterFieldsHTML;
        fieldsListContainer.innerHTML = listFieldsHTML;
        fieldsDetailContainer.innerHTML = detailFieldsHTML;


        const aliasFieldsContainer = document.getElementById("alias-list");
        let aliasFieldsContent = document.querySelectorAll("[data-container=alias-list-template]")[0].innerHTML;
        let aliasFieldsTemplate = Handlebars.compile(aliasFieldsContent);
        const aliasFieldsHTML = aliasFieldsTemplate(fieldList);
        aliasFieldsContainer.innerHTML = aliasFieldsHTML;

        initDragAndDrops();
    }

    function initDragAndDrops() {

        //filter tab droppable
        $( "#selectedFields-filter" ).sortable({
            revert: true,
            over : function(event, ui) {}
        });

        //list tab droppable
        $( "#selectedFields-list" ).sortable({
            revert: true,

            //Place all handlers for dynamic dropped fields here (init them)
            over : function(event, ui) {
                let deleteLink = ui.item.find("span.delete-link")[0];
                if (deleteLink != undefined) {
                    deleteLink.addEventListener("click", function(e) {
                        if (confirm("Delete this field")) {
                            const dropContainer = document.getElementById("selectedFields-list");
                            let droppedElement = deleteLink.parentElement.parentElement.parentElement;
                            dropContainer.removeChild(droppedElement);
                        }
                    });
                }
            }
        });

        //detail tab droppable
        $( "#selectedFields-detail" ).sortable({
            revert: true,

            //Place all handlers for dynamic dropped fields here (init them)
            over : function(event, ui) {
                let deleteLink = ui.item.find("span.delete-link")[0];
                if (deleteLink != undefined) {
                    deleteLink.addEventListener("click", function(e) {
                        if (confirm("Delete this field")) {
                            const dropContainer = document.getElementById("selectedFields-detail");
                            let droppedElement = deleteLink.parentElement.parentElement.parentElement;
                            dropContainer.removeChild(droppedElement);
                        }
                    });
                }
            }
        });

        //all draggables init
        document.getElementsByClassName("ui-draggable");
        $("#field-select-container .ui-draggable").draggable({
            connectToSortable: ".selected-fields",
            helper: "clone",
            revert: "invalid"
        });
    }


    function initEventListeners() {

        //submit button handler
        const submitButton = document.getElementById("submit");
        submitButton.addEventListener("click", function(e) {
            const data = getQueryDataAsJson();
            const formId = document.getElementById("form-id").value;

            //differ between new and edit
            let url = "";
            if (formId == "0" || formId  == "") {
                url = QB.getContextPath() + "/api/queryForm/new";
            } else {
                url = QB.getContextPath() + "/api/queryForm/edit/" + formId;
            }

            //validate just in case
            if (data == null) {
                return;
            }
            if (url == "" || data == undefined || data == {}) {
                showNotification("error", "Unexpected data send error");
                return;
            } else if (data.filter.length == 0 || data.list.length == 0 || data.detail.length == 0) {
                showNotification("error", "Please, add fields to all tabs");
                return;
            } else if (data.info.name == undefined || data.info.name == "") {
                showNotification("error", "Please, name your query");
                return;
            }

            let param = new URLSearchParams();
            param.append("data", JSON.stringify(data));

            //send ajax via AXIOS
            axios({
                method: "post",
                url: url,
                data: param
            })
                .then(function(response) {
                    if (response && response.data) {
                        if (formId == "0" || formId  == "") {
                            const redirectUrl = QB.getContextPath() + "/queryFormBuilder/edit/" +
                                response.data.id + "?newQueryEdit=true";
                            window.location.href = redirectUrl;
                        } else {
                            showNotification("error", "Query data has not been saved");
                        }
                    } else if(response && response.status === 200) {
                        showNotification("success", "Your query changes have been saved");
                    }else {
                        console.log(response);
                        showNotification("error", "Query data has not been saved");
                    }
                })
                .catch(function(error) {
                    console.log(error);
                    showNotification("error", "Data send error occured");
                })
                .finally(function() {/**/});
        });

        //submit and back button handler
        const submitAndBackButton = document.getElementById("save-and-back-link");
        submitAndBackButton.addEventListener("click", function(e) {
            const data = getQueryDataAsJson();
            const formId = document.getElementById("form-id").value;

            //differ between new and edit
            let url = "";
            if (formId == "0" || formId  == "") {
                url = QB.getContextPath() + "/api/queryForm/new";
            } else {
                url = QB.getContextPath() + "/api/queryForm/edit/" + formId;
            }

            //validate just in case
            if (url == "" || data == undefined || data == {}) {
                showNotification("error", "Unexpected data send error");
                return;
            } else if (data.filter.length == 0 || data.list.length == 0 || data.detail.length == 0) {
                showNotification("error", "Please, add fields to all tabs");
                return;
            } else if (data.info.name == undefined || data.info.name == "") {
                showNotification("error", "Please, name your query");
                return;
            }

            let param = new URLSearchParams();
            param.append("data", JSON.stringify(data));

            //send ajax via AXIOS
            axios({
                method: "post",
                url: url,
                data: param
            })
                .then(function(response) {
                    if (response && response.status === 200) {
                        const redirectUrl = QB.getContextPath() + "/queryFormList";
                        window.location.href = redirectUrl;
                    } else {
                        console.log(response);
                        showNotification("error", "Query data has not been saved");
                    }
                })
                .catch(function(error) {
                    console.log(error);
                    showNotification("error", "Data send error occured");
                })
                .finally(function() {/**/});
        });

        //preview button
        const previewButton = document.getElementById("preview-link");
        previewButton.addEventListener("click", function(e) {
            if (document.getElementById("form-id").value != "" && document.getElementById("form-id").value!= undefined &&
                document.getElementById("form-id").value != null) {
                const redirectUrl = QB.getContextPath() + "/userForm/filter/" + document.getElementById("form-id").value;
                window.open(redirectUrl, '_blank');
            } else {
                showNotification("error", "Please, save your query before preview");
            }
        });

        //backlink button handler
        const backButton = document.getElementById("back-link");
        backButton.addEventListener("click", function(e) {
            if (confirm("Wollen Sie wirklich ohne speichern beenden?")) {
                window.location.href = QB.getContextPath();
            }
        });

        //clear button handler
        const resetButton = document.getElementById("reset-link");
        resetButton.addEventListener("click", function(e) {
            if (confirm("clear fields?")) {
                const filterContainer = document.getElementById("selectedFields-filter");
                const listContainer = document.getElementById("selectedFields-list");
                const detailContainer = document.getElementById("selectedFields-detail");

                //much faster than clearing inner html and also clears JS handlers
                while (filterContainer.firstChild) {
                    filterContainer.removeChild(filterContainer.firstChild);
                }
                while (listContainer.firstChild) {
                    listContainer.removeChild(listContainer.firstChild);
                }
                while (detailContainer.firstChild) {
                    detailContainer.removeChild(detailContainer.firstChild);
                }
            }
        });

        $('#working-area-tabs-content').on('click', '.cog', function (event) {
            //cog button and menu handlers
            let cogButton = event.target;
            closeAllCogMenus();
            if (cogButton != undefined) {
                if (cogButton.parentElement.children[2].style.display != "block") {
                    cogButton.parentElement.children[2].style.display = "block";
                }
            }
        });

        $('#working-area-tabs-content').on('click', '.field-type-select', function (e) {
            let cogMenu = e.target.parentElement.parentElement.children[2];
            if (e.target && e.target.matches("li")) {
                Array.prototype.forEach.call(cogMenu.children, child => {
                    child.classList.remove("cog-selected");
                });
                e.target.className = "cog-selected";
                cogMenu.parentElement.children[0].innerHTML = e.target.innerText;
                cogMenu.style.display = "none";
            }
        });

        $('#working-area-tabs-content').on('click', '.delete-link', function (event) {
            //delete links handlers
            let deleteLink = event.target;
            if (deleteLink != undefined) {
                if (confirm("Delete this field")) {
                    const dropContainer = deleteLink.parentElement.parentElement.parentElement.parentElement;
                    let droppedElement = deleteLink.parentElement.parentElement.parentElement;
                    dropContainer.removeChild(droppedElement);
                }
            }
        });

        $('#working-area-tabs-content').on('click', '.toggle-select-all', function (event) {
            //select-all handler
            let container = event.target.parentElement;
            let checkboxes = container.getElementsByTagName("input");
            for (let i=0; i < checkboxes.length; i++) {
                checkboxes[i].checked = true;
            }
        });

        $('#working-area-tabs-content').on('click', '.toggle-deselect-all', function (event) {
            //select-all handler
            let container = event.target.parentElement;
            let checkboxes = container.getElementsByTagName("input");
            for (let i=0; i < checkboxes.length; i++) {
                checkboxes[i].checked = false;
            }
        });

        window.addEventListener('click', function(e){
            if (!e.target.classList.contains("cog")) {
                closeAllCogMenus();
            }

            if (!e.target.classList.contains("btn")) {
                const notificationContainer = document.getElementById("notification-container");
                if (notificationContainer != undefined) {
                    notificationContainer.getElementsByClassName("alert-success")[0].style.display = "none";
                    notificationContainer.getElementsByClassName("alert-danger")[0].style.display = "none";
                }
            }
        });
    }

    function loadFields() {
        const connectionMdbName = document.getElementById("connection-mdb").value;
        const tableId = document.getElementById("table-id").value;
        let param = new URLSearchParams();
        param.append("connectionMdb", connectionMdbName);
        param.append("id", tableId);

        //get fields
        axios({
            method: "post",
            url: QB.getContextPath() + "/api/queryForm/fieldList",
            data: param
        })
            .then(function(response) {
                if (response && response.data) {
                    fieldList.satzeintragList = response.data.fields;
                    initFields();
                }
                else {
                    console.log(response);
                    showNotification("error", "Field list is empty");
                }
            })
            .catch(function(error) {
                console.log(error);
                showNotification("error", "Error loading fields");
            })
            .finally(function() {
                let spinners = document.querySelectorAll(".spinner-border");
                for (let i=0; i<spinners.length; i++) {
                    spinners[i].parentElement.removeChild(spinners[i]);
                }
            });
    }

    function loadCssFileList() {
        //get css files
        axios({
            method: "post",
            url: QB.getContextPath() + "/api/queryForm/cssFileList"
        })
            .then(function(response) {
                if (response && response.data) {
                    cssFileList = response.data;

                    //fill css select
                    const cssFileSelect = document.getElementById("cssfile-select");
                    let cssFileOptions = "";
                    for (let i=0; i < cssFileList.length; i++) {
                        if (cssFileList[i] == formData.cssFile) {
                            cssFileOptions = cssFileOptions + "<option value=\"" + cssFileList[i] + "\" selected>" + cssFileList[i] + "</option>";
                        } else {
                            cssFileOptions = cssFileOptions + "<option value=\"" + cssFileList[i] + "\">" + cssFileList[i] + "</option>";
                        }
                    }
                    cssFileSelect.innerHTML = cssFileOptions;
                }
                else {
                    console.log(response);
                    showNotification("error", "Css file list is empty");
                }
            })
            .catch(function(error) {
                console.log(error);
                showNotification("error", "Error loading css file list");
            })
            .finally(function() {});
    }

    function loadUserList(selectedUsers) {
        const connectionMdbName = document.getElementById("connection-mdb").value;
        let param = new URLSearchParams();
        param.append("connectionMdb", connectionMdbName);

        axios({
            method: "post",
            url: QB.getContextPath() + "/api/queryForm/userList",
            data: param
        })
            .then(function(response) {
                if (response && response.data) {
                    userList = response.data.users;

                    Handlebars.registerHelper('isUserSelected', function (id) {
                        return selectedUsers.includes(id);
                    });

                    let templateData = {};
                    templateData.user = userList;
                    const userFieldsContainer = document.getElementById("user-list");
                    let userFieldsContent = document.querySelectorAll("[data-container=user-list-template]")[0].innerHTML;
                    let userFieldsTemplate = Handlebars.compile(userFieldsContent);
                    const userFieldsHTML = userFieldsTemplate(templateData);
                    userFieldsContainer.innerHTML = userFieldsHTML;
                }
                else {
                    console.log(response);
                    showNotification("error", "User list is empty");
                }
            })
            .catch(function(error) {
                console.log(error);
                showNotification("error", "Error loading user list");
            })
            .finally(function() {});
    }



    //Utility functions
    function getTypeIdByTitle(typeTitle) {
        if (typeTitle == "Auswahlliste") {
            return "select";
        } else if (typeTitle == "Mehrfachuswahl") {
            return "multiplechoice";
        } else if (typeTitle == "Radio Buttons") {
            return "radio";
        } else if (typeTitle == "Checkboxen") {
            return "checkbox";
        } else if (typeTitle == "Ya/Nein-Field") {
            return "yesno";
        } else if (typeTitle == "Eingabefeld") {
            return "input";
        } else if (typeTitle == "Soundex-Field") {
            return "soundex";
        } else if (typeTitle == "RTF-Field") {
            return "rtf";
        } else if (typeTitle == "Button") {
            return "button";
        } else if (typeTitle == "Info") {
            return "info";
        } else {
            return "";
        }
    }

    function getQueryDataAsJson() {
        let data = {
            filter: [],
            list: [],
            detail: [],
        };

        //head info
        let info = {};
        let connectionMdb = document.getElementById("connection-mdb").value;
        let connectionSdb = document.getElementById("connection-sdb").value;
        let database = document.getElementById("database").value;
        let name = document.getElementById("title-input").value;
        let tableId = document.getElementById("table-id").value;
        let comment = document.getElementById("comment-input").value;

        let isAuthenticated = false;
        let users = [];
        let userCheckboxes = document.getElementById("user-list").getElementsByTagName("input");
        if (userCheckboxes != null && userCheckboxes != undefined) {
            for (let i=0; i < userCheckboxes.length; i++) {
                if (userCheckboxes[i].checked) {
                    users.push(userCheckboxes[i].value)
                }
            }
            isAuthenticated = (users.length > 0);
        }

		info.connection = {
			mdb: connectionMdb,
			sdb: connectionSdb
		}
        info.database = database;
        info.name = name;
        info.tableId = tableId;
        info.comment = comment;
        info.isAuthenticated = isAuthenticated;
        info.users = users;
        let errorMessage = document.getElementById("errorMessage-input").value;
        let resultOptions = {
            count: document.getElementById("show-count").checked,
            numbers: document.getElementById("show-numbers").checked,
            criteria: document.getElementById("show-criteria").checked
        };
        let additionalInfo = "";
        let radios = document.getElementsByName('additional-info');
        for (let i = 0, length = radios.length; i < length; i++) {
            if (radios[i].checked) {
                additionalInfo = radios[i].value;
                break;
            }
        }
        let startpoint = document.getElementById("startpoint-select").value;
        let cssFile = document.getElementById("cssfile-select").value;
        data.info = info;
        data.errorMessage = errorMessage;
        data.resultOptions = resultOptions;
        data.additionalInfo = additionalInfo;
        data.startPoint = startpoint;
        data.cssFile = cssFile;

        const filterFieldsContainer = document.getElementById("selectedFields-filter");
        let filterFields = filterFieldsContainer.children;
        for (let i=0; i < filterFields.length; i++) {
            let dataContainer = filterFields[i];
            let id = dataContainer.getElementsByTagName("input")[0].classList[0];
            let type = getTypeIdByTitle(dataContainer.getElementsByClassName("selected-fieldtype")[0].innerHTML);
            let title = dataContainer.getElementsByTagName("input")[0].value;

            let fieldMetaData = findFieldObjectById(id);
            let metadata = {};
            if (Object.entries(fieldMetaData).length > 0) {
                metadata = fieldMetaData;
            }

            let selectedValues = [];
            if (filterFields[i].getElementsByClassName("s-field-options")[0] != undefined) {
                let sSelects = filterFields[i].getElementsByClassName("s-field-options")[0].getElementsByTagName("input");
                for (let j = 0; j < sSelects.length; j++) {
                    if (sSelects[j].checked) {
                        selectedValues.push(sSelects[j].value);
                    }
                }
            }

            if (metadata.type == "W") {
                const expression = validateAndGetExpression(dataContainer);
                if (expression == null) {
                    showNotification("error", "Invalid expression in Field view, field: " + metadata.title);
                    return null;
                } else if (expression != "") {
                    selectedValues.push(expression);
                }
            }

            let fieldObject = {
                metadata: {},
                settings: {},
                data : []
            };

            fieldObject.metadata = metadata;
            fieldObject.data = selectedValues;
            fieldObject.settings.id = id;
            fieldObject.settings.type = type;
            fieldObject.settings.title = title;

            data.filter.push(fieldObject);
        }

        const listFieldsContainer = document.getElementById("selectedFields-list");
        let listFields = listFieldsContainer.children;
        for (let i=0; i < listFields.length; i++) {
            let dataContainer = listFields[i];
            let id = dataContainer.getElementsByTagName("input")[0].classList[0];
            let type = getTypeIdByTitle(dataContainer.getElementsByClassName("selected-fieldtype")[0].innerHTML);
            let title = dataContainer.getElementsByTagName("input")[0].value;

            let fieldMetaData = findFieldObjectById(id);
            let metadata = {};
            if (Object.entries(fieldMetaData).length > 0) {
                metadata = fieldMetaData;
            }

            let selectedValues = [];
            if (metadata.type == "W") {
                const expression = validateAndGetExpression(dataContainer);
                if (expression == null) {
                    showNotification("error", "Invalid expression in Field view, field: " + metadata.title);
                    return null;
                } else if (expression != "") {
                    selectedValues.push(expression);
                }
            }

            let fieldObject = {
                metadata: {},
                settings: {},
                data : []
            };

            fieldObject.metadata = metadata;
            fieldObject.data = selectedValues;
            fieldObject.settings.id = id;
            fieldObject.settings.type = type;
            fieldObject.settings.title = title;

            data.list.push(fieldObject);
        }

        const detailFieldsContainer = document.getElementById("selectedFields-detail");
        let detailFields = detailFieldsContainer.children;
        for (let i=0; i < detailFields.length; i++) {
            let dataContainer = detailFields[i];
            let id = dataContainer.getElementsByTagName("input")[0].classList[0];
            let type = getTypeIdByTitle(dataContainer.getElementsByClassName("selected-fieldtype")[0].innerHTML);
            let title = dataContainer.getElementsByTagName("input")[0].value;

            let fieldMetaData = findFieldObjectById(id);
            let metadata = {};
            if (Object.entries(fieldMetaData).length > 0) {
                metadata = fieldMetaData;
            }

            let selectedValues = [];
            if (metadata.type == "W") {
                const expression = validateAndGetExpression(dataContainer);
                if (expression == null) {
                    showNotification("error", "Invalid expression in Field view, field: " + metadata.title);
                    return null;
                } else if (expression != "") {
                    selectedValues.push(expression);
                }
            }

            let fieldObject = {
                metadata: {},
                settings: {},
                data : []
            };

            fieldObject.metadata = metadata;
            fieldObject.data = selectedValues;
            fieldObject.settings.id = id;
            fieldObject.settings.type = type;
            fieldObject.settings.title = title;

            data.detail.push(fieldObject);
        }

        return data;
    }

    function validateAndGetExpression(fieldContainer) {
        const eqLeftContainer = fieldContainer.getElementsByClassName("eq-left")[0];
        const eqRightContainer = fieldContainer.getElementsByClassName("eq-right")[0];

        if (eqLeftContainer != null && eqLeftContainer != undefined &&
            eqRightContainer != null && eqRightContainer != undefined) {

            const eqLeft = eqLeftContainer.value.toUpperCase();
            const eqRight = eqRightContainer.value.toUpperCase();
            const compareSymbol = fieldContainer.getElementsByClassName("eq-compare")[0].value;

            if (eqLeft != "" && eqRight!="") {

                let eqLeftEval = eqLeft;
                let eqRightEval = eqLeft;
                for (let i = 0; i < fieldList.satzeintragList.length; i++) {
                    eqLeftEval = eqLeftEval.split(fieldList.satzeintragList[i].fieldAlias).join("1");
                    eqRightEval = eqRightEval.split(fieldList.satzeintragList[i].fieldAlias).join("1");
                }

                try {
                    if (math.evaluate(eqLeftEval) && math.evaluate(eqRightEval)) {
                        return eqLeft + compareSymbol + eqRight;
                    }
                } catch (error) {
                    return null;
                }
            }
        }

        return "";
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

    function splitExpression(expression) {
        let result = ["","",""];
        if (expression.split(">=").length == 2) {
            result[0] = expression.split(">=")[0];
            result[1] = ">=";
            result[2] = expression.split(">=")[1];
        } else if (expression.split("<=").length == 2) {
            result[0] = expression.split("<=")[0];
            result[1] = "<=";
            result[2] = expression.split("<=")[1];
        } else if (expression.split("!=").length == 2) {
            result[0] = expression.split("!=")[0];
            result[1] = "!=";
            result[2] = expression.split("!=")[1];
        } else if (expression.split("=").length == 2) {
            result[0] = expression.split("=")[0];
            result[1] = "=";
            result[2] = expression.split("=")[1];
        } else if (expression.split("<").length == 2) {
            result[0] = expression.split("<")[0];
            result[1] = "<";
            result[2] = expression.split("<")[1];
        } else if (expression.split(">").length == 2) {
            result[0] = expression.split(">")[0];
            result[1] = ">";
            result[2] = expression.split(">")[1];
        }

        return result;
    }

    function findFieldObjectById(fieldId) {
        for (let i=0; i < fieldList.satzeintragList.length; i++) {
            if (fieldList.satzeintragList[i].satzPos == fieldId ) {
                return fieldList.satzeintragList[i];
            }
        }
        return {};
    }

    function getUrlVars() {
        var vars = {};
        var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
            vars[key] = value;
        });
        return vars;
    }

    function closeAllCogMenus() {
        var cogMenus = document.getElementsByClassName("field-type-select");
        Array.prototype.forEach.call(cogMenus, function(cogMenu) {
            cogMenu.style.display = "none";
        });
    }

    return {
        init: init
    };
})();
