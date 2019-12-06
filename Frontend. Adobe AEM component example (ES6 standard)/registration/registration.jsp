<%@page session="false" pageEncoding="utf-8" contentType="text/html; charset=utf-8" trimDirectiveWhitespaces="true" %>
<%@include file="/apps/uapps/global.jsp" %>

<sling:adaptTo var="registrationComponent" adaptable="${slingRequest}"
        adaptTo="com.softwerke.test.components.user.registration.model.RegistrationModel"/>

<section id="registrationComponent" class="component-ui registration ${registrationComponent.cssClass}">

    <h2>
        <c:set var="defaultHeading"><fmt:message key="user.registration.form.heading"/></c:set>
        ${not empty registrationComponent.heading ? registrationComponent.heading : defaultHeading}
    </h2>

    <c:if test="${not empty registrationComponent.description}">
        <div class="component-desc">
           <p>${registrationComponent.description}</p>
        </div>
    </c:if>

    <div class="component-body">
        <div class="messages registration-messages">
            <div class="message message--success [ alert alert-success ]" style="display:none;"></div>
            <div class="message message--error [ alert alert-danger ]" style="display:none;"></div>
        </div>

        <div class="form__registration">

            <form class="form-horizontal">

                <div class="registration__item form-group">
                    <label for="regEmailInput" class="control-label">
                        <fmt:message key="user.registration.form.email.label"/>
                    </label>
                    <input id="regEmailInput"
                            name="email"
                            class="form-control"
                            value=""
                            type="email"
                            required
                            message-required='<fmt:message key="user.registration.form.validation.email.required"/>'
                            message-pattern='<fmt:message key="user.registration.form.validation.email.format"/>'
                            placeholder='<fmt:message key="user.registration.form.email.placeholder"/>'/>
                    <label for="regEmailInput" class="error" aria-live="polite" style="display:none;"></label>
                </div>

                <c:if test="${!registrationComponent.name.disabled}">
                    <div class="registration__item form-group">
                        <label for="regFirstNameInput" class="control-label">
                            <fmt:message key="user.registration.form.name.label"/>
                        </label>
                        <input id="regFirstNameInput"
                                name="firstName"
                                class="form-control"
                                value=""
                                type="text"
                                <c:if test="${registrationComponent.name.required}">
                                    required
                                    message-required='<fmt:message key="user.registration.form.validation.name.required"/>'
                                </c:if>
                                pattern="${registrationComponent.patternNameCharacters}"
                                message-pattern='<fmt:message key="user.registration.form.validation.name.format.characters"/>'
                                placeholder='<fmt:message key="user.registration.form.name.placeholder"/>'/>
                        <label for="regFirstNameInput" class="error" aria-live="polite" style="display:none;"/>
                    </div>
                </c:if>

                <c:if test="${!registrationComponent.phone.disabled}">
                    <div class="form-group">
                        <label for="regPhoneInput">
                            <fmt:message key="user.registration.form.phone.label"/>
                        </label>
                        <input id="regPhoneInput"
                                name="phone"
                                class="form-control"
                                type="text"
                                <c:if test="${registrationComponent.phone.required}">
                                    required
                                    message-required='<fmt:message key="user.registration.form.validation.phone.required"/>'
                                </c:if>
                                message-pattern='<fmt:message key="user.registration.form.validation.phone.format"/>'
                                placeholder='<fmt:message key="user.registration.form.phone.placeholder"/>'/>
                        <label for="regPhoneInput" class="error" aria-live="polite" style="display:none;"/>
                    </div>
                </c:if>

                <c:if test="${!registrationComponent.about.disabled}">
                    <div class="registration__item form-group">
                        <label for="regAboutInput" class="control-label">
                            <fmt:message key="user.registration.form.about.label"/>
                        </label>
                        <input id="regAboutInput"
                                name="about"
                                class="form-control"
                                value=""
                                type="text"
                                <c:if test="${registrationComponent.about.required}">
                                    required
                                    message-required='<fmt:message key="user.registration.form.validation.about.required"/>'
                                </c:if>
                                placeholder='<fmt:message key="user.registration.form.about.placeholder"/>'/>
                        <label for="regAboutInput" class="error" aria-live="polite" style="display:none;"/>
                    </div>
                </c:if>

                <c:if test="${!registrationComponent.rulesAgreement.disabled}">
                    <div class="registration__item form-group custom-control custom-checkbox">
                        <input id="regRulesAgreementInput"
                                name="rulesAgreement"
                                class="form-control custom-control-input"
                                value="true"
                                <c:if test="${registrationComponent.rulesAgreement.required}">
                                    required
                                </c:if>
                                type="checkbox"/>
                        <label for="regRulesAgreementInput" class="custom-control-label valid">
                            <fmt:message key="user.registration.form.rules.text"/>
                            <c:if test="${not empty registrationComponent.rulesPageLink}">
                                <a href="${registrationComponent.rulesPageLink}" target="_blank">
                                    <fmt:message key="user.registration.form.rules.link"/>
                                </a>
                            </c:if>
                        </label>
                    </div>
                </c:if>

                <c:if test="${!registrationComponent.pdaAndCommunicationAgreement.disabled}">
                    <div class="registration__item form-group custom-control custom-checkbox">
                        <input id="communicationAgreementInput"
                                name="communicationAgreement"
                                class="form-control custom-control-input"
                                value="true"
                                <c:if test="${registrationComponent.pdaAndCommunicationAgreement.required}">
                                    required
                                </c:if>
                                 type="checkbox"/>
                        <label for="communicationAgreementInput" class="custom-control-label">
                            <fmt:message key="user.registration.form.communication.text"/>
                            <c:if test="${not empty registrationComponent.pdaAndCommunicationAgreementPageLink}">
                                <a href="${registrationComponent.pdaAndCommunicationAgreementPageLink}" target="_blank">
                                    <fmt:message key="user.registration.form.communication.link" />
                                </a>
                            </c:if>
                        </label>
                    </div>
                </c:if>

                <c:if test="${not empty registrationComponent.agreementText}">
                    <div class="registration__item form-group" id="agreementText">
                        ${registrationComponent.agreementText}
                    </div>
                </c:if>

                <div class="registration__item form-group button-panel">
                    <button type="submit" class="[ btn btn-primary btn-lg ] btn_save">
                        <fmt:message key="user.registration.form.button.submit"/>
                    </button>
                </div>

            </form>
        </div>
    </div>
</section>

<cq:includeClientLib categories="uapps.mask"/>

<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        UUU.MainNamespace.RegistrationController.init();
    });
</script>