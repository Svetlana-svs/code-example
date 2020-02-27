

<%@include file="global.jsp" %>

<html>
<head>
<jsp:include page="head.jsp"/>
</head>

<body>
<div id="header">
    <jsp:include page="header.jsp"/>
</div>

<div id="main">
    Lorem ipsum dolor sit amet, consectetur adipisicing elit,
    sed do eiusmod tempor incididunt ut labore et dolore magna
    aliqua.
</div>

<div id="footer">
    <jsp:include page="footer.jsp"/>
</div>	<h1>Login</h1>

	<form name='f' action="perform_login" method='POST'>

		<table>
			<tr>
				<td>User admin:</td>
				<td><input type='text' name='username' value=''></td>
			</tr>
			<tr>
				<td>Password:</td>
				<td><input type='password' name='password' /></td>
			</tr>
			<tr>
				<td><input name="submit" type="submit" value="submit" /></td>
			</tr>
		</table>

	</form>

</body>
</html>