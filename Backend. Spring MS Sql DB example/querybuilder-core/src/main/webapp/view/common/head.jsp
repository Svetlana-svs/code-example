<%--
    @author Svetlana Suvorova
    @lastEdit Svetlana Suvorova
--%>

<%@ include file="global.jsp"%>

<link href="${urlCss}/lib/datatables.min.css" rel="stylesheet"/>
<link href="${urlCss}/lib/bootstrap.min.css" rel="stylesheet"/>

<link href="${urlCss}/style.css" rel="stylesheet"/>

<script>
    document.addEventListener("DOMContentLoaded", function() {
        QB.init(${settings});
    });
</script>