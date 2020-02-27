<%--
    @author Svetlana Suvorova
    @lastEdit Svetlana Suvorova
--%>

<%@ include file="common/global.jsp"%>

<html>
    <head>
        <!-- TODO: set folder common as parameter or variable -->
        <jsp:include page="common/head.jsp"/>
        <title>Login</title>
    </head>

    <body>
        <jsp:include page="common/header.jsp"/>

        <main>
            <div class="container">
                <form class="login w-50" action="login" method="POST" name="f">
                    <h2>Login</h2>
                    <div class="form-group">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="login-addon">Name</span>
                            </div>
                            <input type="text" class="form-control" aria-label="Username" aria-describedby="login-addon" name="username" value="">
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="password-addon">Password</span>
                            </div>
                            <input type="password" class="form-control" aria-label="Username" aria-describedby="basic-addon1" name="password">
                        </div>
                    </div>
                    <div class="form-group">
                        <input name="submit" type="submit" class="btn btn-primary btn-block w-50" id=login-btn value="Login">
                    </div>
                    <div class="alert alert-danger hide-error-alert" role="alert" id="error-alert">
                        Wrong credantial
                    </div>
                </form>
            </div>
        </main>

        <jsp:include page="common/footer.jsp"/>
    </body>
</html>

<script type="text/javascript">
    document.addEventListener("DOMContentLoaded", function() {
        QB.LoginController.init();
    });
</script>
