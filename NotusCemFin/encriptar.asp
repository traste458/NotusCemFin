<!--#include file="include/md5.asp"-->
<%
dim clave
clave = request.queryString("cadena")
response.write(md5(clave))
%>
