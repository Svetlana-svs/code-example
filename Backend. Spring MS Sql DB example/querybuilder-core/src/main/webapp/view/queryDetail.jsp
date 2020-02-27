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
            <h1 id="query-title">Detail page</h1>
        </div>
    </div>
</div>

<div class="container result-head-controls">
    <div class="row">
        <div class="col-6">
            <button class="btn" id="back-link">
                Back
            </button>
        </div>
        <div class="col-6 search-result-info">

        </div>
    </div>
</div>

<div class="container query-detail">
    <div class="row">
        <div class="col-12" id="query-detail-fields-container">

            <div class="spinner-border text-muted"></div>

            <script data-container="detail-template" type="text/x-handlebars-template">

                {{#each searchResult}}
                <div class="search-result-container">

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

                </div>
                {{/each}}

            </script>

        </div>
    </div>
</div>


<div id="footer">
    <jsp:include page="common/footer.jsp"/>
</div>
</body>
</html>

<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        QB.QueryDetailController.init();
    });
</script>