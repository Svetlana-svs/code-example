/**
 * @author Sergei Kharitonov
 * @lastEdit Sergei Kharitonov
 */

QB.QueryListController = (function() {

    let formData = {};
    let searchResult = [];
    let formId = "";
    let searchParameters = "";

    function init() {
        const urlProps = window.location.pathname.split("/");
        formId = urlProps[4];
        searchParameters = decodeURI(getUrlVars()["data"]);
        loadFormData();
        initEventListeners();
    }

    function loadFormData() {
        axios({
            method: "post",
            url: QB.getContextPath() + "/api/userForm/filter/" + formId,
        })
            .then(function(response) {
                if (response && response.data) {
                    formData = response.data;
                    document.getElementById("query-title").innerHTML = formData.info.name;
                    drawSearchCriteria();
                    loadSearchResults();
                }
            })
            .catch(function(error) {
                console.log(error);
                showNotification("error", "Error loading query form");
            })
            .finally(function() {});
    }

    function loadSearchResults() {
        let param = new URLSearchParams();
        param.append("data", searchParameters);
        axios({
            method: "post",
            url: QB.getContextPath() + "/api/userForm/list/" + formId,
            data: param
        })
            .then(function(response) {
                if (response && response.data) {
                    searchResult = response.data;
                    includeCustomCss(formData.cssFile);
                    document.getElementById("query-title").innerHTML = formData.info.name;
                    if (searchResult.length == 0) {
                        let errorMessage = "Search query had 0 results";
                        if (formData.errorMessage != "" && formData.errorMessage != undefined &&
                            formData.errorMessage != null) {
                            errorMessage = formData.errorMessage;
                        }
                        showNotification("error", errorMessage);
                    } else {
                        if (formData.resultOptions.count) {
                            document.getElementById("result-count").innerHTML = searchResult.length + " results found";
                        }
                        drawViewElements(1);
                        drawPagination();
                    }
                }
            })
            .catch(function(error) {
                console.log(error);
                showNotification("error", "Server error during search");
            })
            .finally(function() {
                let spinners = document.querySelectorAll(".spinner-border");
                for (let i=0; i<spinners.length; i++) {
                    spinners[i].parentElement.removeChild(spinners[i]);
                }
            });
    }

    function drawViewElements(pageNumber) {

        Handlebars.registerHelper('getValueByAlias', function (alias, index) {
            let elIndex = (pageNumber-1)*10 + index;
            for (let i=0; i < searchResult[elIndex].length; i++) {
                if (searchResult[elIndex][i].alias.trim() == alias.trim()) {
                    return searchResult[elIndex][i].value;
                }
            }
        });

        Handlebars.registerHelper('getDisplayFieldTitle', function (field) {
            if (field.settings.title != "" && field.settings.title != undefined &&
                field.settings.title != null) {
                return field.settings.title;
            }
            return field.metadata.title;
        });

        Handlebars.registerHelper('increment', function (value) {
            return (pageNumber-1)*10 + parseInt(value) + 1;
        });

        const templateData = {};
        templateData.list = formData.list;
        templateData.searchResult = searchResult.slice((pageNumber-1)*10, (pageNumber-1)*10+10);
        templateData.displayCounter = formData.resultOptions.numbers;

        let listContainer = document.getElementById("query-list-fields-container");
        let listTemplateContent = document.querySelectorAll("[data-container=list-template]")[0].innerHTML;
        let listTemplate = Handlebars.compile(listTemplateContent);
        const listFieldsHTML = listTemplate(templateData);
        listContainer.innerHTML = listFieldsHTML;
    }

    function drawPagination() {
        if (searchResult.length > 10) {
            const numberOfPages = 1 + parseInt(searchResult.length/10);
            let pageNumbers = [];
            for (let i=0; i < numberOfPages; i++) {
                pageNumbers.push(i+1);
            }

            const paginationContainer = document.getElementById("pagination-top");
            let paginationTemplateContent = document.querySelectorAll("[data-container=pagination-template]")[0].innerHTML;
            let paginationTemplate = Handlebars.compile(paginationTemplateContent);
            const paginationHTML = paginationTemplate(pageNumbers);
            paginationContainer.innerHTML = paginationHTML;
        }
    }

    function drawSearchCriteria() {
        if (formData.resultOptions.criteria) {
            const criteriaData = {};
            criteriaData.criteria = formData.filter;
            const searchParametersJson = JSON.parse(searchParameters);

            Handlebars.registerHelper('getValueForFIlterField', function (alias) {
                if (searchParametersJson[alias] != undefined) {
                    if (searchParametersJson[alias].length > 0) {
                        return searchParametersJson[alias].join('\n');
                    }
                }
                return "*";
            });

            Handlebars.registerHelper('getDisplayFieldTitle', function (field) {
                if (field.settings.title != "" && field.settings.title != undefined &&
                    field.settings.title != null) {
                    return field.settings.title;
                }
                return field.metadata.title;
            });

            const criteriaContainer = document.getElementById("criteria-container");
            let criteriaTemplateContent = document.querySelectorAll("[data-container=criteria-template]")[0].innerHTML;
            let criteriaTemplate = Handlebars.compile(criteriaTemplateContent);
            const criteriaFieldsHTML = criteriaTemplate(criteriaData);
            criteriaContainer.innerHTML = criteriaFieldsHTML;
        }
    }

    function initEventListeners() {
        $('#query-list-fields-container').on('click', '.detail-link', function (event) {
            window.location.href = QB.getContextPath() + "/userForm/detail/" + formId +
                "?data=" + encodeURI(searchParameters) +
                "&index=" + (event.target.id -1);
        });

        $('#pagination-top').on('change', '#page-number', function (event) {
            let selectedPage = $( "#" + event.target.id + " option:selected" ).val();
            drawViewElements(selectedPage);
        });

        $('#pagination-top').on('click', '.prev', function (event) {
            event.preventDefault();
            $("#page-number > option:selected")
                .prop("selected", false)
                .prev()
                .prop("selected", true);
            let selectedPage = $( "#page-number option:selected" ).val();
            drawViewElements(selectedPage);
        });

        $('#pagination-top').on('click', '.next', function (event) {
            event.preventDefault();
            $("#page-number > option:selected")
                .prop("selected", false)
                .next()
                .prop("selected", true);
            let selectedPage = $( "#page-number option:selected" ).val();
            drawViewElements(selectedPage);
        });

        //backlink button handler
        const backButton = document.getElementById("back-link");
        backButton.addEventListener("click", function(e) {
            window.location.href = QB.getContextPath() + "/userForm/filter/" + formId;
        });
    }

    function getUrlVars() {
        var vars = {};
        var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
            vars[key] = value;
        });
        return vars;
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
