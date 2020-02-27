<%--
    @author Svetlana Suvorova
    @lastEdit Sergei Kharitonov
--%>

<%@ include file="global.jsp"%>

<footer class="fixed-bottom">
    <div class="container-fluid">
        Generiert am <span id="current-time"></span><br>
        DUVA Anfrage Generator V<span id="app-version"></span>
    </div>
</footer>

<script src="${urlJs}/lib/jquery-3.4.1.slim.min.js"></script>
<script src="${urlJs}/lib/popper.min.js"></script>
<script src="${urlJs}/lib/bootstrap.min.js"></script>
<script src="${urlJs}/lib/axios.min.js"></script>
<script src="${urlJs}/lib/handlebars.js"></script>
<script src="${urlJs}/lib/datatables.min.js"></script>

<script src="${urlJs}/common.js"></script>
<script src="${urlJs}/footer.js"></script>
<script src="${urlJs}/login.js"></script>
<script src="${urlJs}/modal.js"></script>
<script src="${urlJs}/eventHandler.js"></script>
<script src="${urlJs}/queryFormList.js"></script>

<script src="${urlJs}/queryFilter.js"></script>
<script src="${urlJs}/queryList.js"></script>
<script src="${urlJs}/queryDetail.js"></script>

<jsp:include page="modal.jsp"/>

<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        QB.FooterController.init();
    });
</script>