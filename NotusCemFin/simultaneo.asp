<%@ LANGUAGE="VBScript" %>
<%
Server.ScriptTimeout = 2000
%><% Response.Buffer=True %>
<!--#include file="include/conexion.inc.asp"-->
<%  

serial = request.querystring("serial")
caja = 1
estiba = 1
idposicion = 2931
linea = request.querystring("linea")
idorden = request.querystring("idorden")

while i < 10

  sql = sql & " exec sp_produccion '"&serial&"',70,1680,'OR',"&caja&","&estiba&",384,"&idorden&","&idposicion&","&linea&",10,64,0,640"
'  response.write sql&"<br>"
    set rs = conn.execute(sql)
  serial = serial+1
  caja = rs("caja")
  estiba = rs("estiva")
  obs = rs("obs")
'  response.write obs&"<br>"
  i = i+1
wend
response.write "LISTO -> "& i 
conn.close
%>