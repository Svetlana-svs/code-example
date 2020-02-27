<%--
@author Sergei Kharitonov
@lastEdit Sergei Kharitonov
--%>

<%@ page contentType="text/html; charset=UTF-8" %> <!-- TODO: solves utf-8 germanese characters, maybe apply to whole project later -->
<%@ include file="common/global.jsp"%>

<html>
<head>
    <jsp:include page="common/head.jsp"/>
</head>

<body class="formbuilder-body">
<div id="header">
    <jsp:include page="common/header.jsp"/>
</div>

<!-- TODO after role implementation, place containers to assigned security checks -->
<security:authorize access="hasRole('ROLE_USER')">

</security:authorize>

<security:authorize access="hasRole('ROLE_ADMIN')">

</security:authorize>

<div class="container query-header">
    <div class="row">
        <div class="col-12">
            <h1 id="query-title">List page</h1>
        </div>
    </div>
</div>

<!-- Notifications container -->
<div class="notification-container row align-items-center" id="notification-container">
    <div class="alert alert-success col-6 offset-3" role="alert">

    </div>
    <div class="alert alert-danger col-6 offset-3" role="alert">

    </div>
</div>

<div class="container result-head-controls">
    <div class="row">
        <div class="col-6">
            <button class="btn" id="back-link">
                Back
            </button>
        </div>
        <div class="col-6 search-result-info" id="result-count">

        </div>
    </div>
</div>

<div class="container search-criteria" id="criteria-container">

    <script data-container="criteria-template" type="text/x-handlebars-template">
        <div class="row">
            <div class="col-6 field-title criteria-header">
                Search criteria:
            </div>
        </div>

        {{#each criteria}}
            <div class="row criteria-row">


                <div class="col-6 field-title">
                    {{getDisplayFieldTitle this}}
                </div>
                <div class="col-6 field-content">
                    {{getValueForFIlterField this.metadata.fieldAlias}}
                </div>


            </div>
        {{/each}}

    </script>

</div>

<div class="container pagination" id="pagination-top">

    <script data-container="pagination-template" type="text/x-handlebars-template">

    <div class="paginationControls">
        <a href="" class="prev">Prev</a>
        <select class="page-number" id="page-number">
            {{#each this}}
            <option value="{{this}}" {{#unless @index }}selected{{/unless}}> {{this}} </option>
            {{/each}}
        </select>
        <a href="" class="next">Next</a>
    </div>

    </script>

</div>

<div class="container query-list">
    <div class="row">
        <div class="col-12" id="query-list-fields-container">

            <div class="spinner-border text-muted"></div>

        </div>
    </div>
</div>

<script data-container="list-template" type="text/x-handlebars-template">

    {{#each searchResult}}
    <div class="search-result-container">

        {{#if ../this.displayCounter}}
        <div class="counter">
            {{increment @index}}
        </div>
        {{/if}}

        {{#each ../this.list}}
        <div class="row">
            <div class="col-6 field-title">
                {{getDisplayFieldTitle this}}
            </div>
            <div class="col-6 field-content">
                {{getValueByAlias metadata.fieldAlias @../index}}
            </div>
        </div>
        {{/each}}


        <div class="row detail-button-container">
            <button class="btn detail-link" id="{{increment @index}}">
                Detail
            </button>
        </div>
    </div>
    {{/each}}

</script>


<div id="footer">
    <jsp:include page="common/footer.jsp"/>
</div>
</body>
</html>

<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        QB.QueryListController.init();
    });
</script>