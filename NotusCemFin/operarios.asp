<%@ language= "VBscript"%>
<%
Response.Expires=0
Response.Buffer=True
%>
<html>
<head>
<title>Operarios</title>
</head>
<body bgcolor="#FFFFFF" text="#000000">
<!--#include file="include/conexion.inc.asp"--> 
<!--#include file="include/titulo1.inc.asp"-->
<% sql="select * from terceros where idcargo>15 and idcargo<20"
set rs=conn.execute(sql)  
%>
  <font color=blue size=5>Operarios</font> 
  <table>
  <tr><td><i>ESN No.</i></td><td><%=rs("idtercero")%></td></tr>
  <tr><td><i>Producto-Fábrica</i></td><td><%=rs("tercors")%></td></tr>
    </table>
<%  
rs.close %>

</body>
</html>
