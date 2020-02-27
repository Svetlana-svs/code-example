<%--
@author Sergei Kharitonov
@lastEdit Sergei Kharitonov
--%>

<%@ page contentType="text/html; charset=UTF-8" %>
<%@ include file="common/global.jsp"%>

<html>
    <head>
        <jsp:include page="common/head.jsp"/>
        <jsp:include page="common/header-admin.jsp"/>
    </head>

    <body class="formbuilder-body">
        <div id="header">
            <jsp:include page="common/header.jsp"/>
        </div>

        <!-- Notifications container -->
        <div class="notification-container row align-items-center" id="notification-container">
            <div class="alert alert-success col-6 offset-3" role="alert">

            </div>
            <div class="alert alert-danger col-6 offset-3" role="alert">

            </div>
        </div>

        <!-- Top controls container -->
        <div class="container builder-controls">

            <input type="hidden" value="${connectionMdb}" id="connection-mdb">
            <input type="hidden" value="${connectionSdb}" id="connection-sdb">
            <input type="hidden" value="${database}" id="database">
            <input type="hidden" value="0" id="form-id">
            <input type="hidden" value="0" id="table-id">

            <h3 class="database-header" id="database-header">
                Datei: ${connectionMdb}
            </h3>

            <div class="row">

                <!-- TOP ui inputs & elements -->
                <div class="col-10">

                    <div class="row">

                        <div class="col-6">
                            <div class="textinput-container">
                                <span class="input-label">Abfragetitel:</span>
                                <input type="text" id="title-input">
                            </div>

                            <div class="textinput-container">
                                <span class="input-label">Meldung bei ergebnisloser:</span>
                                <input type="text" id="errorMessage-input">
                            </div>

                            <div class="textinput-container">
                                <span class="input-label">Comment:</span>
                                <input type="text" id="comment-input">
                            </div>
                        </div>

                        <div class="col-6">
                            <div class="row">
                                <div class="col-6">
                                    <p>Optionen fur die Ergebnisliste:</p>

                                    <div class="check-container">
                                        <input type="checkbox" id="show-count">
                                        <label for="show-count">
                                            Anzahl Ergebnisse anzeigen
                                        </label>
                                    </div>

                                    <div class="check-container">
                                        <input type="checkbox" id="show-numbers">
                                        <label for="show-numbers">
                                            Ergebnisse nummerien
                                        </label>
                                    </div>

                                    <div class="check-container">
                                        <input type="checkbox" id="show-criteria">
                                        <label for="show-criteria">
                                            Suchkriterien anzeigen
                                        </label>
                                    </div>

                                </div>

                                <div class="col-6">
                                    <p>Anzeige von Zusatzinformationen:</p>

                                    <div class="check-container">
                                        <input type="radio" id="top-radio-1" name="additional-info" value="footnote">
                                        <label for="top-radio-1">
                                            als Fußnoten
                                        </label>
                                    </div>

                                    <div class="check-container">
                                        <input type="radio" id="top-radio-2" name="additional-info" value="reference">
                                        <label for="top-radio-2">
                                            als Verweise
                                        </label>
                                    </div>

                                    <div class="check-container">
                                        <input type="radio" id="top-radio-3" name="additional-info" checked value="none">
                                        <label for="top-radio-3">
                                            gar nicht
                                        </label>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Top low part selects -->
                    <div class="row">

                        <div class="col-3 select-container">

                            <span class="input-label">
                                Startpoint of query:
                            </span>
                            <select id="startpoint-select">
                                <option value="filter" selected> Filter </option>
                                <option value="list"> List </option>
                                <option value="detail"> Detail </option>
                            </select>

                        </div>
                        <div class="col-3 select-container">

                            <span class="input-label">
                                CSS FIle:
                            </span>
                            <select id="cssfile-select">

                            </select>
                        </div>

                        <div class="col-3 select-container user-select-container">
                            <div class="collapse-pannel">
                                <a data-toggle="collapse" href="#user-list">Show/hide users</a>
                            </div>
                            <div id="user-list" class="panel-collapse collapse">

                                <script data-container="user-list-template" type="text/x-handlebars-template">
                                    {{#each user}}
                                    <div class="user-option-container">
                                        <input type="checkbox" value="{{id}}" {{#if (isUserSelected id)}}checked{{/if}}>
                                        <label> {{name}} </label>
                                    </div>
                                    {{/each}}
                                </script>

                            </div>
                        </div>

                        <div class="col-3 select-container user-select-container">
                            <div class="collapse-pannel">
                                <a data-toggle="collapse" href="#alias-list">Show/hide aliases</a>
                            </div>
                            <div id="alias-list" class="panel-collapse collapse">

                                <script data-container="alias-list-template" type="text/x-handlebars-template">
                                    {{#each satzeintragList}}
                                    <div class="user-option-container">
                                        <span>{{title}} : {{fieldAlias}}</span>
                                    </div>
                                    {{/each}}
                                </script>

                            </div>
                        </div>
                    </div>
                </div>

                <!-- Right side buttopns -->
                <div class="col-2 button-container">

                    <button class="btn" id="back-link">
                        Zurück ohne speichern
                    </button>

                    <button class="btn" id="reset-link">
                        Alles löschen
                    </button>

                    <button class="btn" id="submit">
                        Speichem
                    </button>

                    <button class="btn" id="save-and-back-link">
                        Zurück mit speichern
                    </button>

                    <button class="btn" id="preview-link">
                        Zeige Formular
                    </button>

                </div>
            </div>
        </div>

        <!-- Working area container -->
        <div class="container working-area">

            <div class="row">

                <!-- field select area -->
                <div class="col-3 field-select-container">
                    <!-- Header -->
                    <div class="row head">
                        <div class="col-6">
                            Field Name
                        </div>
                        <div class="col-2">
                            O/U
                        </div>
                        <div class="col-2">
                            I/S/W
                        </div>
                        <div class="col-2">
                            Size
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-3 field-select-container" id="field-select-container">

                    <div class="spinner-border text-muted"></div>

                    <!-- START: script template for HandleBars -->
                    <script data-container="fields-bar-template" type="text/x-handlebars-template">


                    <!-- rows -->
                    <!-- defaults -->
                    <!--
                    <div class="ui-draggable ui-draggable-handle">
                        <div class="row draggable-headline">
                            <div class="col-6">
                                Container
                            </div>
                            <div class="col-2">
                                NA
                            </div>
                            <div class="col-2">
                                NA
                            </div>
                            <div class="col-2">
                                NA
                            </div>
                        </div>
                        <div class="row draggable-controls">
                            <div class="col-7">
                                <div class="row-container-droppable ui-sortable">

                                </div>
                            </div>

                            <div class="col-2">
                                <span class="delete-link"> delete </span>
                            </div>

                        </div>
                    </div>
                    -->
                    <div class="ui-draggable ui-draggable-handle">
                        <div class="row draggable-headline">
                            <div class="col-6">
                                Submit
                            </div>
                            <div class="col-2">
                                NA
                            </div>
                            <div class="col-2">
                                NA
                            </div>
                            <div class="col-2">
                                NA
                            </div>
                        </div>
                        <div class="row draggable-controls">
                            <div class="col-6">
                                <div class="textinput-container">
                                    <span class="input-label">Submit Button Title:</span>
                                    <input type="text" class="field-submit-button">
                                </div>
                            </div>
                            <div class="col-3">
                                <span class="selected-fieldtype">Button</span>
                            </div>
                            <div class="col-2">
                                <span class="delete-link"> delete </span>
                            </div>
                        </div>
                    </div>
                    <div class="ui-draggable ui-draggable-handle">
                        <div class="row draggable-headline">
                            <div class="col-6">
                                Reset
                            </div>
                            <div class="col-2">
                                NA
                            </div>
                            <div class="col-2">
                                NA
                            </div>
                            <div class="col-2">
                                NA
                            </div>
                        </div>
                        <div class="row draggable-controls">
                            <div class="col-6">
                                <div class="textinput-container">
                                    <span class="input-label">Reset Button Title:</span>
                                    <input type="text"  class="field-reset-button">
                                </div>
                            </div>
                            <div class="col-3">
                                <span class="selected-fieldtype">Button</span>
                            </div>
                            <div class="col-2">
                                <span class="delete-link"> delete </span>
                            </div>
                        </div>
                    </div>

                    {{#each satzeintragList }}
                    <div class="ui-draggable ui-draggable-handle">
                        <div class="row draggable-headline">
                            <div class="col-6">
                                {{title}}
                            </div>
                            <div class="col-2">
                                {{fieldType}}
                            </div>
                            <div class="col-2">
                                {{type}}
                            </div>
                            <div class="col-2">
                                {{length}}
                            </div>
                        </div>
                        <div class="row draggable-controls">
                            <div class="col-6">
                                <div class="textinput-container">
                                    <span class="input-label">{{title}}:</span>
                                    <input type="text" class="{{satzPos}}">
                                </div>
                            </div>
                            <div class="col-3">

                                <span class="selected-fieldtype">{{getDefaultISWType type}}</span>

                                {{#if (isIType type)}}
                                <button class="btn cog"></button>
                                <ul class="field-type-select">
                                    <li class="cog-selected">Auswahlliste</li>
                                    <li>Mehrfachuswahl</li>
                                    <li>Radio Buttons</li>
                                    <li>Checkboxen</li>
                                    <li>Ya/Nein-Field</li>
                                    <li>Eingabefeld</li>
                                    <li>Soundex-Field</li>
                                    <li>RTF-Field</li>
                                </ul>
                                {{/if}}

                                {{#if (isWType type)}}
                                <button class="btn cog"></button>
                                <ul class="field-type-select">
                                    <li class="cog-selected">Eingabefeld</li>
                                    <li>Soundex-Field</li>
                                </ul>
                                {{/if}}

                                {{#if (isSType type)}}
                                <button class="btn cog"></button>
                                <ul class="field-type-select">
                                    <li class="cog-selected">Auswahlliste</li>
                                    <li>Mehrfachuswahl</li>
                                    <li>Radio Buttons</li>
                                    <li>Checkboxen</li>
                                </ul>
                                {{/if}}

                            </div>

                            <div class="col-2">
                                <span class="delete-link"> delete </span>
                            </div>
                        </div>

                        {{#if (isSType type)}}
                        <div class="row s-field-options">
                            <div class="col-6">
                                <span class="toggle-s-showhide"></span>
                                <div class="collapse-pannel">
                                    <a data-toggle="collapse" href="{{getAliasReference fieldAlias}}">Show/hide options</a>
                                </div>
                                <div id="{{fieldAlias}}" class="panel-collapse collapse show">
                                    <span class="toggle-select-all">select all</span>
                                    <span class="toggle-deselect-all">deselect all</span>
                                    {{#each data}}
                                    <div class="s-field-option-container">
                                        <input type="checkbox" value="{{id}}" checked>
                                        <label> {{value}} </label>
                                    </div>
                                    {{/each}}
                                </div>
                            </div>
                        </div>
                        {{/if}}

                        {{#if (isWType type)}}
                        <div class="row w-field-expression">
                            <div class="col-6">
                                <input type="text" class="eq-left">
                                <select class="eq-compare">
                                    <option value="="> = </option>
                                    <option value="!="> != </option>
                                    <option value=">"> > </option>
                                    <option value="<"> < </option>
                                    <option value=">="> >= </option>
                                    <option value="<="> <= </option>
                                </select>
                                <input type="text" class="eq-right">
                            </div>
                        </div>
                        {{/if}}

                    </div>
                    {{/each}}

                    </script>
                    <!-- END: script template for HandleBars -->

                </div>

                <!-- working area -->
                <div class="col-9">
                    <ul class="nav nav-tabs" id="working-area-tabs" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active" id="filter-tab" data-toggle="tab" href="#filter" role="tab" aria-controls="filter" aria-selected="true">
                                Filter
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="list-tab" data-toggle="tab" href="#list" role="tab" aria-controls="list" aria-selected="false">
                                List
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" id="detail-tab" data-toggle="tab" href="#detail" role="tab" aria-controls="detail" aria-selected="false">
                                Detail
                            </a>
                        </li>
                    </ul>
                    <div class="tab-content" id="working-area-tabs-content">
                        <div class="tab-pane fade show active fields-tab" id="filter" role="tabpanel" aria-labelledby="filter-tab">
                            <div id="selectedFields-filter" class="selected-fields ui-sortable">
                                <div class="spinner-border text-muted"></div>
                            </div>
                        </div>
                        <div class="tab-pane fade list-tab" id="list" role="tabpanel" aria-labelledby="list-tab">
                            <div id="selectedFields-list" class="selected-fields ui-sortable">
                                <div class="spinner-border text-muted"></div>
                            </div>
                        </div>
                        <div class="tab-pane fade detail-tab" id="detail" role="tabpanel" aria-labelledby="detail-tab">
                            <div id="selectedFields-detail" class="selected-fields ui-sortable">
                                <div class="spinner-border text-muted"></div>
                            </div>
                        </div>
                    </div>

                    <!-- START: script template for HandleBars -->
                    <script data-container="workingarea-field-template" type="text/x-handlebars-template">
                        {{#each fields }}
                        <div class="ui-draggable ui-draggable-handle ui-sortable-handle">
                            <div class="row draggable-headline">
                                <div class="col-6">
                                    {{metadata.title}}
                                </div>
                                <div class="col-2">
                                    {{metadata.fieldType}}
                                </div>
                                <div class="col-2">
                                    {{metadata.type}}
                                </div>
                                <div class="col-2">
                                    {{metadata.length}}
                                </div>
                            </div>
                            <div class="row draggable-controls">
                                <div class="col-6">
                                    <div class="textinput-container">
                                        <span class="input-label">{{getInputLabelText metadata settings}}</span>
                                        <input type="text" class="{{settings.id}}" value="{{settings.title}}">
                                    </div>
                                </div>
                                <div class="col-3">

                                    <span class="selected-fieldtype">{{fieldTextByTypeId settings.type}}</span>

                                    {{#if (isIType metadata.type)}}
                                    <button class="btn cog"></button>
                                    <ul class="field-type-select">
                                        <li {{#if (isSelect settings.type)}}class="cog-selected"{{/if}}>Auswahlliste</li>
                                        <li {{#if (isMultichoice settings.type)}}class="cog-selected"{{/if}}>Mehrfachuswahl</li>
                                        <li {{#if (isRadio settings.type)}}class="cog-selected"{{/if}}>Radio Buttons</li>
                                        <li {{#if (isCheckbox settings.type)}}class="cog-selected"{{/if}}>Checkboxen</li>
                                        <li {{#if (isYesNo settings.type)}}class="cog-selected"{{/if}}>Ya/Nein-Field</li>
                                        <li {{#if (isInput settings.type)}}class="cog-selected"{{/if}}>Eingabefeld</li>
                                        <li {{#if (isSoundex settings.type)}}class="cog-selected"{{/if}}>Soundex-Field</li>
                                        <li {{#if (isRTF settings.type)}}class="cog-selected"{{/if}}>RTF-Field</li>
                                    </ul>
                                    {{/if}}

                                    {{#if (isWType metadata.type)}}
                                    <button class="btn cog"></button>
                                    <ul class="field-type-select">
                                        <li {{#if (isInput settings.type)}}class="cog-selected"{{/if}}>Eingabefeld</li>
                                        <li {{#if (isSoundex settings.type)}}class="cog-selected"{{/if}}>Soundex-Field</li>
                                    </ul>
                                    {{/if}}

                                    {{#if (isSType metadata.type)}}
                                    <button class="btn cog"></button>
                                    <ul class="field-type-select">
                                        <li {{#if (isSelect settings.type)}}class="cog-selected"{{/if}}>Auswahlliste</li>
                                        <li {{#if (isMultichoice settings.type)}}class="cog-selected"{{/if}}>Mehrfachuswahl</li>
                                        <li {{#if (isRadio settings.type)}}class="cog-selected"{{/if}}>Radio Buttons</li>
                                        <li {{#if (isCheckbox settings.type)}}class="cog-selected"{{/if}}>Checkboxen</li>
                                    </ul>
                                    {{/if}}

                                </div>

                                <div class="col-2">
                                    <span class="delete-link"> delete </span>
                                </div>
                            </div>

                            {{#if (isSType metadata.type)}}
                            <div class="row s-field-options">
                                <div class="col-6">
                                    <span class="toggle-s-showhide"></span>
                                    <div class="collapse-pannel">
                                        <a data-toggle="collapse" href="{{getAliasReference metadata.fieldAlias}}">Show/hide options</a>
                                    </div>
                                    <div id="{{metadata.fieldAlias}}" class="panel-collapse collapse show">
                                        <span class="toggle-select-all">select all</span>
                                        <span class="toggle-deselect-all">deselect all</span>
                                        {{#each metadata.data}}
                                        <div class="s-field-option-container">
                                            <input type="checkbox" value="{{id}}"
                                                   {{#if (isSelectedField id ../this.data)}}checked{{/if}}>
                                            <label>{{value}}</label>
                                        </div>
                                        {{/each}}
                                    </div>
                                </div>
                            </div>
                            {{/if}}

                            {{#if (isWType metadata.type)}}
                            <div class="row w-field-expression">
                                <div class="col-6">
                                    <input type="text" class="eq-left" value="{{getLeftEq data}}">
                                    <select class="eq-compare">
                                        <option value="=" {{#if (isE data)}}selected{{/if}}> = </option>
                                        <option value="!=" {{#if (isNE data)}}selected{{/if}}> != </option>
                                        <option value=">" {{#if (isM data)}}selected{{/if}}> > </option>
                                        <option value="<" {{#if (isL data)}}selected{{/if}}> < </option>
                                        <option value=">=" {{#if (isME data)}}selected{{/if}}> >= </option>
                                        <option value="<=" {{#if (isLE data)}}selected{{/if}}> <= </option>
                                    </select>
                                    <input type="text" class="eq-right" value="{{getRightEq data}}">
                                </div>
                            </div>
                            {{/if}}

                        </div>
                        {{/each}}
                    </script>
                    <!-- END: script template for HandleBars -->

                </div>
            </div>
        </div>
        <div id="footer">
            <jsp:include page="common/footer.jsp"/>
            <jsp:include page="common/footer-admin.jsp"/>
        </div>
    </body>
</html>

<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        QB.QueryFormBuilderController.init();
    });
</script>