<%--
@author Sergei Kharitonov
@lastEdit Sergei Kharitonov
--%>

<%@ page contentType="text/html; charset=UTF-8" %>
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
            <h1 id="query-title">Filter page</h1>
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

<div class="container query-filter">
    <div class="row">
        <div class="col-12" id="query-filter-fields-container">

            <div class="spinner-border text-muted"></div>

            <script data-container="filter-template" type="text/x-handlebars-template">
                {{#each filter }}

                {{#if (isSelect settings.type)}}
                <div class="filter-field single-select" id="{{metadata.fieldAlias}}">
                    <span> {{getFieldTitle metadata settings}} </span>
                    <select>
                        {{#each data}}
                        <option value="{{this}}">
                            {{getOptionValue ../this.metadata.type ../this.metadata.data this}}
                        </option>
                        {{/each}}
                    </select>
                </div>
                {{/if}}

                {{#if (isMultichoice settings.type)}}
                <div class="filter-field multi-select" id="{{metadata.fieldAlias}}">
                    <span> {{getFieldTitle metadata settings}} </span>

                    <div class="toggle-controls">
                        <span class="toggle-select-all">select all</span>
                        <span class="toggle-deselect-all">deselect all</span>
                    </div>

                    <select multiple size="{{getMultichoiceSize data}}">
                        {{#each data}}
                        <option value="{{this}}">
                            {{getOptionValue ../this.metadata.type ../this.metadata.data this}}
                        </option>
                        {{/each}}
                    </select>
                </div>
                {{/if}}

                {{#if (isCheckbox settings.type)}}
                <div class="filter-field checkbox-select" id="{{metadata.fieldAlias}}">
                    <span> {{getFieldTitle metadata settings}} </span>

                    <div class="toggle-controls">
                        <span class="toggle-select-all">select all</span>
                        <span class="toggle-deselect-all">deselect all</span>
                    </div>

                    <div>
                        {{#each data}}
                        <div>
                            <input type="checkbox" value="{{this}}">
                            <span> {{getOptionValue ../this.metadata.type ../this.metadata.data this}} </span>
                        </div>
                        {{/each}}
                    </div>
                </div>
                {{/if}}

                {{#if (isRadio settings.type)}}
                <div class="filter-field radio-select" id="{{metadata.fieldAlias}}">
                    <span> {{getFieldTitle metadata settings}} </span>
                    <div>
                        {{#each data}}
                        <div>
                            <input type="radio" value="{{this}}"
                                   name="{{../this.metadata.fieldAlias}}">
                            <span> {{getOptionValue ../this.metadata.type ../this.metadata.data this}} </span>
                        </div>
                        {{/each}}
                    </div>
                </div>
                {{/if}}

                {{#if (isYesNo settings.type)}}
                <div class="filter-field yesno-select" id="{{metadata.fieldAlias}}">
                    <span> {{getFieldTitle metadata settings}} </span>
                    <div>
                        <input type="checkbox" value="true">
                        <span> Yes </span>
                    </div>
                    <div>
                        <input type="checkbox" value="false">
                        <span> No </span>
                    </div>
                </div>
                {{/if}}

                {{#if (isInput settings.type)}}
                    {{#if (isExpression this)}}
                        <div class="filter-field yesno-select" id="{{metadata.fieldAlias}}">
                            <span> {{getFieldTitle metadata settings}} </span>
                            <div>
                                <input type="checkbox" value="true">
                                <span> Yes </span>
                            </div>
                            <div>
                                <input type="checkbox" value="false">
                                <span> No </span>
                            </div>
                        </div>
                    {{/if}}
                    {{#unless (isExpression this)}}
                    <div class="filter-field textinput-select" id="{{metadata.fieldAlias}}">
                        <span> {{getFieldTitle metadata settings}} </span>
                        <input type="text">
                    </div>
                    {{/unless}}
                {{/if}}

                {{#if (isSoundex settings.type)}}
                    {{#if (isExpression this)}}
                    <div class="filter-field yesno-select" id="{{metadata.fieldAlias}}">
                        <span> {{getFieldTitle metadata settings}} </span>
                        <div>
                            <input type="checkbox" value="true">
                            <span> Yes </span>
                        </div>
                        <div>
                            <input type="checkbox" value="false">
                            <span> No </span>
                        </div>
                    </div>
                    {{/if}}
                    {{#unless (isExpression this)}}
                    <div class="filter-field soundex-select" id="{{metadata.fieldAlias}}">
                        <span> {{getFieldTitle metadata settings}} </span>
                        <input type="text">
                    </div>
                    {{/unless}}
                {{/if}}

                {{#if (isRTF settings.type)}}
                <div class="filter-field textinput-select" id="{{metadata.fieldAlias}}">
                    <span> {{getFieldTitle metadata settings}} </span>
                    <input type="text">
                </div>
                {{/if}}

                {{#if (isButton settings.type)}}
                <div class="filter-field button-row">
                    <button class="btn {{settings.id}}">
                        {{getButtonTitle settings}}
                    </button>
                </div>
                {{/if}}

                {{/each}}
            </script>
        </div>
    </div>
</div>

<div class="container query-filter">
    <div class="row">
        <div class="col-12">
            <div class="filter-field button-row">
                <button class="btn field-submit-button" id="submit-link">
                    Submit
                </button>

                <button class="btn field-reset-button" id="reset-link">
                    Reset
                </button>
            </div>
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
        QB.QueryFilterController.init();
    });
</script>