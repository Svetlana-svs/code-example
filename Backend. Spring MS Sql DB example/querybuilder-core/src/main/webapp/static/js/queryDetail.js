/**
 * @author Sergei Kharitonov
 * @lastEdit Sergei Kharitonov
 */

QB.QueryDetailController = (function() {

    let formData = {};
    let formId = "";
    let searchParameters = "";
    let dislpayPos = 0;

    function init() {
        const urlProps = window.location.pathname.split("/");
        formId = urlProps[4];
        searchParameters = decodeURI(getUrlVars()["data"]);
        dislpayPos = parseInt(getUrlVars()["index"]);
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
                    loadSearchResults();
                }
            })
            .catch(function(error) {
                console.log(error);
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
                    document.getElementById("query-title").innerHTML = formData.info.name;
                    drawViewElements();
                }
            })
            .catch(function(error) {
                console.log(error);
            })
            .finally(function() {
                let spinners = document.querySelectorAll(".spinner-border");
                for (let i=0; i<spinners.length; i++) {
                    spinners[i].parentElement.removeChild(spinners[i]);
                }
            });
    }


    function drawViewElements() {
        Handlebars.registerHelper('getValueByAlias', function (alias, index) {
            for (let i=0; i < templateData.searchResult[index].length; i++) {
                if (templateData.searchResult[index][i].alias.trim() == alias.trim()) {
                    return templateData.searchResult[index][i].value;
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

        const templateData = {};
        templateData.list = formData.detail;
        templateData.searchResult = [];
        templateData.searchResult.push(searchResult[dislpayPos]);

        const detailContainer = document.getElementById("query-detail-fields-container");
        let detailTemplateContent = document.querySelectorAll("[data-container=detail-template]")[0].innerHTML;
        let detailTemplate = Handlebars.compile(detailTemplateContent);
        const detailFieldsHTML = detailTemplate(templateData);
        detailContainer.innerHTML = detailFieldsHTML;
    }

    function initEventListeners() {
        //backlink button handler
        const backButton = document.getElementById("back-link");
        backButton.addEventListener("click", function(e) {
            window.location.href = QB.getContextPath() + "/userForm/list/" + formId +
                "?data=" + encodeURI(searchParameters);
        });
    }

    function getUrlVars() {
        var vars = {};
        var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m,key,value) {
            vars[key] = value;
        });
        return vars;
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
