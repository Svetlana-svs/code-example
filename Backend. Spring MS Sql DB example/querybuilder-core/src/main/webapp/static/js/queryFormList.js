/**
 * @author Polina Lappo
 * @lastEdit Sergei Kharitonov
 */

QB.QueryFormListController = (function() {
    "use strict"

    const ID_CONTAINER = "queryFormList";
    const QUERY_FORM_DELETE_MODAL_TITLE = "Delete form";
    const QUERY_FORM_NEW_MODAL_TITLE = "Auswahl von Dateien";

    const templateTableContext = {
        forms: []
    }
    const templateDatabaseContext = {
        databases: [],
        tables: {}
    }

    let container = {};
    let tableContainer = {};
    let templateTable = {};
	let table = {};

    function initElements() {
	    container = document.getElementById(ID_CONTAINER);
        if (container === null) {
            console.log('Dom element is not defined.');
            return;
        }

        // Container where handlebars template set
	    tableContainer = container.querySelectorAll("[data-container=table-container]");
        if (tableContainer === null) {
           console.log('Dom element is not defined.');
            return;
        }

        // Handlebars template
        const templateTableContent = container.querySelectorAll("[data-container=table-content-template]")[0].innerHTML;
        templateTable = Handlebars.compile(templateTableContent);

        setDatabaseList();
    }

    function initEventListeners() {
        const newButton = document.getElementById("queryFormNew");
        newButton.addEventListener("click", function(event) {
            const modalBodyHtml = getNewFormModalBodyHtml(event);
            QB.ModalController.openModal(QUERY_FORM_NEW_MODAL_TITLE, modalBodyHtml, QB.EventHandler.channels.MODAL_QUERY_FORM_NEW);
            if (document.getElementById("table-select") != null &&
                document.getElementById("table-select") != undefined) {
                setTableList((templateDatabaseContext.databases && templateDatabaseContext.databases.length
                    && templateDatabaseContext.databases[0].name) || 0);
            }
        });

        $('#form-list-table tbody').on('click', 'a.del', function (event) {
            const modalBodyHtml = getDeleteFormModalBodyHtml(event);
            QB.ModalController.openModal(QUERY_FORM_DELETE_MODAL_TITLE, modalBodyHtml, QB.EventHandler.channels.MODAL_QUERY_FORM_DELETE);
        });

		QB.EventHandler.subscribe(QB.EventHandler.channels.MODAL_QUERY_FORM_DELETE, function(data) {
            if (data && data.confirm) {
                deleteForm(data);
            }
        });

        QB.EventHandler.subscribe(QB.EventHandler.channels.MODAL_QUERY_FORM_NEW, function(data) {
            if (data && data.confirm) {
                const selectedConnectionMdb = $("#queryFormDatabase option:selected").html();
                const selectedTable = document.getElementById("table-select").value;
                window.location.href = QB.getContextPath() + "/queryFormBuilder/new?metaDb=" + selectedConnectionMdb +
                    "&table=" + selectedTable;
            }
        });

        $('body').on('change', '#queryFormDatabase', function (event) {
            let selectedConnectionMdb = $( "#" + event.target.id + " option:selected" ).text();
            setTableList(selectedConnectionMdb);
        });

    }

    function setDatabaseList() {
        axios({
            method: "post",
            url: QB.getContextPath() + "/api/queryForm/databaseList"
        })
        .then(function(response) {
            if (response && response.data) {
                templateDatabaseContext.databases = response.data.databases;
                const selectedConnectionMdb = (templateDatabaseContext.databases && templateDatabaseContext.databases.length
                && templateDatabaseContext.databases[0].name) || "";
                setTableList(selectedConnectionMdb);
            }
            else {
                console.log(response);
            }
        })
        .catch(function(error) {
            console.log(error);
        })
        .finally(function() {/**/});
    }

    function setTableList(selectedConnectionMdb) {
        let param = new URLSearchParams();
        param.append("connectionMdb",  selectedConnectionMdb);
        axios({
            method: "post",
            url: QB.getContextPath() + "/api/queryForm/tableList",
            data: param
        })
            .then(function(response) {
                if (response && response.data) {
                    templateDatabaseContext.tables = response.data.tables;
                    if (templateDatabaseContext.tables.length > 0) {
                        document.querySelectorAll(".modal-footer .btn-primary")[0].disabled = false;
                        if (document.getElementById("table-select") != null &&
                            document.getElementById("table-select") != undefined) {

                            document.getElementById("table-select").disabled = false;
                            const template = "{{#each tables}}\n" +
                                "                 <option value=\"{{id}}\" {{#unless @index }}selected{{/unless}}>{{name}}</option>\n" +
                                "             {{/each}}";
                            let templateHandler = Handlebars.compile(template);
                            document.getElementById("table-select").innerHTML = templateHandler(templateDatabaseContext);
                        }
                    } else {
                        document.querySelectorAll(".modal-footer .btn-primary")[0].disabled = true;
                        if (document.getElementById("table-select") != null &&
                            document.getElementById("table-select") != undefined) {
                            document.getElementById("table-select").disabled = true;
                        }
                    }
                }
                else {
                    console.log(response);
                }
            })
            .catch(function(error) {
                console.log(error);
            })
            .finally(function() {/**/});
    }

    function setDataTable() {
        // Set data to template
        const tableHtml = templateTable(templateTableContext);
        // Set template with data in container div
        tableContainer[0].outerHTML = tableHtml;

        // Container with table
        const tableElement = container.querySelectorAll("[data-container=table-template]");
        if (tableElement === null) {
            console.log('Dom element is not defined.');
            return;
        }

        $(tableElement).DataTable({
            "scrollY":        "50vh",
            "scrollCollapse": true,
            initComplete: function(settings) {
                  table = this.DataTable();
              }
        });

        initEventListeners();
    }

    function deleteForm(data) {
        const modalBody = data.body;
        // Extract deleting form id
        const id = modalBody.querySelector("[class*=query-form-delete-modal-body]").getAttribute("query-form-id");

        axios({
            method: "post",
            url: QB.getContextPath() + "/api/queryForm/delete/" + id
        })
        .then(function(response) {
            if (response && response.status === 200) {
                // Remove form row from table
                // TODO: update table with new data from server instead row remove
                let target = document.querySelector(".del-" + id);
                while((target = target.parentNode) && (target.nodeName !== "TR")){}

                table.row(target).remove().draw();
                table.reload();
            }
            else {
                console.log(response);
            }
        })
        .catch(function(error) {
            console.log(error);
        })
        .finally(function() {/**/});
    }

    function getDeleteFormModalBodyHtml(event) {
        // Handlebars template
        const templateModalBodyContent = container.querySelectorAll("[data-container=query-form-delete-modal-template]")[0].innerHTML;
        const templateModalBody = Handlebars.compile(templateModalBodyContent);

        const classes = event.currentTarget.className.split(" ");
        // Get deleting form id from table row and set as attribute in modal body
        const id = classes.find(function(className) {
            return className.startsWith("del-");
        }).substring(4);
        // Get deleting form name from table and set in text of the modal body
        const formName = container.querySelector(".name-" + id).innerHTML;

		const templateModalBodyContext = {
			id: id,
			name: formName
		}
        const modalBodyHtml = templateModalBody(templateModalBodyContext);

        return modalBodyHtml;
    }

    function getNewFormModalBodyHtml(event) {
        // Handlebars template
        const templateModalBodyContent = container.querySelectorAll("[data-container=query-form-new-modal-template]")[0].innerHTML;
        const templateModalBody = Handlebars.compile(templateModalBodyContent);
        const modalBodyHtml = templateModalBody(templateDatabaseContext);

        return modalBodyHtml;
    }

    function init() {

        initElements();

        axios({
            method: "post",
            url: QB.getContextPath() + "/api/queryForm/all"
        })
        .then(function(response) {
            if (response && response.data) {
                const data = response.data;
				Object.keys(data).forEach(function (key) {
				    try {
                        const form = JSON.parse(data[key]).info;
                        form.id = key;
                        templateTableContext.forms.push(form);
                    } catch (error) {
                        console.log("Error parsing form object: \n" + data[key]);
                        console.log(error);
                    }
                });
                setDataTable();
            }
            else {
                console.log(response);
            }
        })
        .catch(function(error) {
            console.log(error);
        })
        .finally(function() {/**/});
    }

    return {
        init: init
    };
})();
