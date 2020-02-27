<%--
@author Svetlana Suvorova
@lastEdit Sergei Kharitonov
--%>

<%@ include file="common/global.jsp"%>

<html>
    <head>
        <!-- TODO: set folder common as parameter -->
        <jsp:include page="common/head.jsp"/>
        <title>Query List</title>
    </head>

    <body>
        <jsp:include page="common/header.jsp"/>
        <main>
            <div class="container query-form-list"
                    id="queryFormList">
                <div class="row">
                    <h2>List of queries</h2>
                </div>
                <div class="row manipulate-btns">
                    <a class="btn btn-primary"
                            id="queryFormNew"
                            href="#"
                            role="button" >
                       New
                    </a>
<%--                    <button class="btn btn-outline-primary" id="filter" type="button">Filter</button>--%>
<%--                    <button class="btn btn-outline-primary" id="remove-filter" type="button">Remove filter</button>--%>
                </div>
                <div class="row">
                    <div class="container-fluid" data-container="table-container"></div>
                </div>
 <img src="ext/logo_sw.png">
                <script data-container="table-content-template" type="text/x-handlebars-template">
                    <table class="table table-striped table-hover" id="form-list-table"
                            data-container="table-template">
                        <thead>
                            <tr>
                                <th scope="col"></th>
                                <th scope="col">Name</th>
                                <th scope="col">Comment</th>
                                <th scope="col">Author</th>
                                <th scope="col">Date</th>
                            </tr>
                        </thead>
                        <tbody>
                            {{#each forms }}
                                <tr>
                                    <td>
                                        <div class="edit-btns">
                                            <a class="edit"
                                                    href="${pageContext.request.contextPath}/queryFormBuilder/edit/{{ id }}"
                                                    role="button"">
                                                <img src="${urlImg}/svg/edit.svg">
                                            </a>
                                            <a class="del del-{{ id }}" href="#" role="button">
                                                <img src="${urlImg}/svg/del.svg">
                                            </a>
                                        </div>
                                    </td>
                                    <td class="name-{{ id }}">{{ name }}</td>
                                    <td>{{ comment }}</td>
                                    <td>{{ author }}</td>
                                    <td>{{ date }}</td>
                                </tr>
                            {{/each}}
                        </tbody>
                    </table>
                </script>

                <script data-container="query-form-delete-modal-template" type="text/x-handlebars-template">
                    <div class="query-form-delete-modal-body" query-form-id="{{ id }}">Do you realy want to delete "{{ name }}" form?</div>
                </script>

                <script data-container="query-form-new-modal-template" type="text/x-handlebars-template">
                    <div class="form-group">
                        <label for="database">Dateiliste:</label>
                        <select class="form-control" id="queryFormDatabase">
                            {{#each databases}}
                                <option value="{{ id }}" {{#unless @index }}selected{{/unless}}>{{ name }}</option>
                            {{/each}}
                        </select>

                        <label for="table">Tableliste:</label>
                        <select class="form-control" id="table-select">
                            {{#each tables}}
                            <option value="{{ id }}" {{#unless @index }}selected{{/unless}}>{{ name }}</option>
                            {{/each}}
                        </select>

                    </div>
                </script>
            </div>
        </main>
        <jsp:include page="common/footer.jsp"/>
    </body>
</html>

<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        QB.QueryFormListController.init();
    });
</script>
