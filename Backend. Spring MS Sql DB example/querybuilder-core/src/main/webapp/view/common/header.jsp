<%--
    @author Svetlana Suvorova
    @lastEdit Svetlana Suvorova
--%>

<%@ include file="global.jsp"%>

<header>
    <div class="container-fluid">
        <div class="row">
            <div class="col-4">
                <img src="${urlImg}/Freiburg_WbMarke_72dpiRGB.jpg" alt="">
            </div>
            <div class="col-4">
                <h1 class="">Anfragegenerator</h1>
            </div>
<%--        <security:authorize access="hasRole('ROLE_USER')">--%>
<%--            This text is only visible to a user--%>
<%--            <br/> <br/>--%>
<%--            <br/> <br/>--%>
<%--        </security:authorize>--%>

<%--        <security:authorize access="hasRole('ROLE_ADMIN')">--%>
<%--            This text is only visible to an admin--%>
<%--            <br/>--%>
<%--            <br/>--%>
<%--        </security:authorize>--%>
            <div class="col-4 auth-btns">

                <security:authorize access="hasRole('ROLE_ADMIN')">

                    <div class="float-right w-25">
                        <div>
                            <a href="${pageContext.request.contextPath}/logout" role="button"><img src="${urlImg}/svg/logout.svg" alt=""><span>Beenden</span></a>
                        </div>
                    </div>

                </security:authorize>

                <security:authorize access="hasRole('ROLE_USER')">

                    <div class="float-right w-25">
                        <div>
                            <a href="${pageContext.request.contextPath}/logout" role="button"><img src="${urlImg}/svg/logout.svg" alt=""><span>Beenden</span></a>
                        </div>
                    </div>

                </security:authorize>

            </div>
        </div>
    </div>
</header>