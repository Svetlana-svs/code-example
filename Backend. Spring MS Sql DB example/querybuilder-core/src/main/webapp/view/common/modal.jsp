<%--
    @author Svetlana Suvorova
    @lastEdit Svetlana Suvorova
--%>

<%@ include file="global.jsp"%>

<div class="modal fade"
        id="modalWindow"
        tabindex="-1"
        role="dialog"
        aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title"></h5>
                <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">

            </div>
            <div class="modal-footer">
                <button class="btn btn-outline-primary" type="button" data-dismiss="modal">No</button>
                <button class="btn btn-primary" type="button">Yes</button>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        QB.ModalController.init();
    });
</script>
