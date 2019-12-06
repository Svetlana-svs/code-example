<%@page session="false" pageEncoding="utf-8" contentType="text/html; charset=utf-8" trimDirectiveWhitespaces="true" %>
<%@include file="/apps/e/global.jsp" %>

<section class="upload-qr-receipt__header">
    <h2>
        <c:set var="defaultHeading"><fmt:message key="uploadqrreceipt.heading"/></c:set>
        <span class="heading">${not empty uploadQRReceiptComponent.heading ? uploadQRReceiptComponent.heading : defaultHeading}</span>
    </h2>
    <c:if test="${not empty uploadQRReceiptComponent.description}">
        <div class="component-desc"><p>${uploadQRReceiptComponent.description}</p></div>
    </c:if>
</section>

<section class="upload-qr-receipt__body">
    <div class="upload-qr-receipt_messages">
        <div class="message message--success [ alert alert-success ]" style="display:none;"></div>
        <div class="message message--error [ alert alert-danger ]" style="display:none;"></div>
        <div class="message message--info [ alert alert-info ]" style="display:none;"></div>
    </div>

    <form id="uploadReceiptForm_${uploadQRReceiptComponent.instanceId}" class="form-horizontal">

        <c:if test="${uploadQRReceiptComponent.qrScannerEnabled}">
            <ul class="uploadReceiptForm__tabs-nav nav nav-tabs nav-fill">
                <li class="nav-item active">
                    <a class="nav-link active"
                            href="#tabVideoScanner_${uploadQRReceiptComponent.instanceId}"
                            data-toggle="tab">
                        <fmt:message key="uploadqrreceipt.form.tab.name.scan"/>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link"
                            href="#tabFileUpload_${uploadQRReceiptComponent.instanceId}"
                            data-toggle="tab">
                        <fmt:message key="uploadqrreceipt.form.tab.name.fileupload"/>
                    </a>
                </li>
            </ul>

            <div class="components-list__tabs-content tab-content">
                <div class="tab-pane active" id="tabVideoScanner_${uploadQRReceiptComponent.instanceId}">

                    <div class="video-scanning">
                        <video muted autoplay playsinline class="scanningVideo"></video>
                        <a href="#" class="btn btnScanRemove" style="display: none"><fmt:message key="uploadqrreceipt.form.scan.button.remove"/></a>
                        <a href="#" class="btn btnScanningStart"><fmt:message key="uploadqrreceipt.form.scan.button.start"/></a>
                    </div>
                    <label for="uploadReceiptTextInput_${uploadQRReceiptComponent.instanceId}" class="control-label">
                        <fmt:message key="uploadqrreceipt.form.scan.result.label"/>
                    </label>
                    <input id="scanningResultInput"
                            class="scanningResult"
                            name="scanningResult"
                            type="text"
                            readonly="readonly">
                    </input>
                    <hr>

                </div>
                <div class="tab-pane" id="tabFileUpload_${uploadQRReceiptComponent.instanceId}">
        </c:if>

        <div class="upload-receipt__item receipt-file form-group">
            <label for="receiptFileInput_${uploadQRReceiptComponent.instanceId}" class="col-sm-4 control-label">
                <fmt:message key="uploadqrreceipt.form.file.label"/>
            </label>
            <div class="col-sm-8">
                <input id="receiptFileInput_${uploadQRReceiptComponent.instanceId}"
                       class="form-control receiptFileInput"
                       name="file"
                       type="file"
                <%--accept="${uploadQRReceiptComponent.imageMimeTypes}"--%>
                       image="image/png"
                       filesize="${uploadQRReceiptComponent.fileMaxSize}"
                />
            </div>
        </div>

        <div class="receipt-container" style="display:none;">
            <p class="file-name"></p>
            <a href="#" class="btnFileRemove">
                <span class="close">&times;</span>
                <fmt:message key="uploadqrreceipt.form.file.button.remove"/>
            </a>
            <div class="file-preview">
                <img class="preview-image" src=""/>
                <div class="preview-loupe"></div>
            </div>
        </div>

        <c:if test="${uploadQRReceiptComponent.qrScannerEnabled}">
                </div>
            </div>
        </c:if>

        <c:if test="${uploadQRReceiptComponent.textEnabled}">
            <label for="uploadReceiptTextInput_${uploadQRReceiptComponent.instanceId}" class="control-label">
                <fmt:message key="uploadqrreceipt.form.text.label"/>
            </label>
            <input id="uploadReceiptTextInput_${uploadQRReceiptComponent.instanceId}"
                    class="form-control uploadReceiptText
                    ${uploadQRReceiptComponent.textRequired ? 'required' : ''}"
                    name="text"
                    type="text"
                    maxlength="100"
                    placeholder='<fmt:message key="uploadqrreceipt.form.text.placeholder"/>'>
            </input>
        </c:if>

        <button class="btn" type="submit">
            <fmt:message key="uploadqrreceipt.form.button.submit"/>
        </button>

        <input type="hidden" id="resourcePath_${uploadQRReceiptComponent.instanceId}" name="resourcePath" value="${resource.path}"/>
    </form>
</section>

