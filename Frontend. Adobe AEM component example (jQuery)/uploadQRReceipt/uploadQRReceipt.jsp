<%@page session="false" pageEncoding="utf-8" contentType="text/html; charset=utf-8" trimDirectiveWhitespaces="true" %>
<%@include file="/apps/e/global.jsp" %>

<sling:adaptTo var="uploadQRReceiptComponent" adaptable="${slingRequest}"
               adaptTo="com.e.dmp.components.ugc.uploadqrreceipt.UploadQRReceiptComponent"/>

<section id="uploadQRReceipt_${uploadQRReceiptComponent.instanceId}"
         class="component-ui uploadQRReceipt ${uploadQRReceiptComponent.cssClass}">

    <c:if test="${uploadQRReceiptComponent.formCollapsed || uploadQRReceiptComponent.formRenderMode == 'Modal'}">
        <div class="upload-qr-receipt__item">
            <button class="btn btn-link uploadQRReceiptStart"
                    type="button"
                    <c:if test="${uploadQRReceiptComponent.formRenderMode != 'Modal'}">
                        data-toggle="collapse"
                        data-target="#uploadQRReceiptBody_${uploadQRReceiptComponent.instanceId}"
                        aria-expanded="${not uploadQRReceiptComponent.formCollapsed}"
                        aria-controls="uploadQRReceiptBody_${uploadQRReceiptComponent.instanceId}"
                    </c:if>>
                <fmt:message key="uploadqrreceipt.button.form.collapse"/>
            </button>
        </div>
    </c:if>

    <div id="uploadQRReceiptBody_${uploadQRReceiptComponent.instanceId}"
            class="upload-qr-receipt__item ${uploadQRReceiptComponent.formCollapsed &&
                    uploadQRReceiptComponent.formRenderMode != 'Modal' ? 'collapse' : ''}">

        <c:set var="uploadQRReceiptComponent" value="${uploadQRReceiptComponent}" scope="request"/>
        <c:choose>
            <c:when test="${uploadQRReceiptComponent.formRenderMode == 'Inline'}">
                <cq:include script="uploadQRReceiptForm.jsp"/>
            </c:when>
            <c:otherwise>
                <div class="modal fade uploadQRReceiptModal" role="dialog">
                    <div class="modal-dialog" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title">
                                    <fmt:message key="uploadqrreceipt.modal.title"/>
                                </h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                            <div class="modal-body">
                                <cq:include script="uploadQRReceiptForm.jsp"/>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-dismiss="modal">
                                    <fmt:message key="uploadqrreceipt.modal.close"/>
                                </button>
                                <button type="button" class="btn btn-primary">
                                    <fmt:message key="uploadqrreceipt.modal.save"/>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </c:otherwise>
        </c:choose>

        <div class="modal fade uploadQRReceiptPreviewZoom" tabindex="-1"
             role="dialog" aria-labelledby="upload-receip-preview-zoom" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <img class="preview-image" src="" alt='<fmt:message key="uploadqrreceipt.image.preview"/>'/>
                    </div>
                </div>
            </div>
        </div>

    </div>
</section>

<cq:includeClientLib js="S.qr-worker"/>
<cq:includeClientLib js="S.qr-scanner"/>


<script  type="text/javascript">
    $(function () {
        new S.MainNamespace.UploadQRReceipt.uploadQRReceiptForm(${uploadQRReceiptComponent.controllerSettingsJson});
    });
</script>
