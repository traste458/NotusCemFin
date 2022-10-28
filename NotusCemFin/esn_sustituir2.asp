<html>
<% session("titulo")="Sustituir Esn de ESNs" %>
<body bgcolor="#FFFFFF" text="#000000">
<!--#include file="include/conexion.inc.asp"--> 
<!--#include file="include/titulo1.inc.asp"-->
<a href="esn_sustituir.asp">Regresar </a> <br><br>
<form name="forma" method="post" action="esn_sustituir3.asp" onSubmit="return validacion ()">  
<%
esn = Request.Form("esn")
esn_nuevo = Request.Form("esn_nuevo")

sql=" select *, isnull(idorden,0) as aux_idorden from vi_productos_serial where serial='" & esn & "'"
'response.write sql & "<br>"  ' la comilla es comentario
Set rs=conn.execute(sql)
if rs.eof then ' si la consulta dio vacia
  response.write "<font size=4 color=black><i>No existe el ESN No. " & esn & "</i></font></h2>"
else  ' de lo contrario (es decir no es vacia) empezamos a mostrar los datos en una tablita
  if cdbl(rs("aux_idorden")) > 0 then
    sql="select idorden2,fecha,subproducto,tipoorden from vi_ordenesdetalle"
     sql=sql & " where idorden=" & rs("idorden")
     response.write sql & "<br>"
     set rs2=conn.execute(sql)
     if not rs2.eof then
       vidorden2 = rs2("idorden2")
       vorden_fecha = rs2("fecha")
       vsubproducto = rs2("subproducto")
     end if
     rs2.close
  end if
  %>
  <table border=1 width="50%">
  <tr><td><i><font color=red>ESN ACTUAL</font></i></td><td><%=esn%></td></tr>
  </table>
  <table border=1>  
  <tr><td bgcolor=#FF9900><i>Producto-Fábrica</i></td><td><%=rs("producto")%></td></tr>
  <tr><td  bgcolor=#FF9900><i>Factura-Fábrica</i></td><td><%=rs("idfactura2")%></td></tr>
  <tr><td  bgcolor=#FF9900><i>Caja/Estiba</i></td><td><%=rs("caja")%>/<%=rs("estiva")%>
  <tr><td  bgcolor=#FF9900><i>MIN</i></td><td><%=rs("min")%></td></tr> 
  <tr><td  bgcolor=#FF9900><i>Orden No.</i></td><td><%=vidorden2%> (<%=vorden_fecha%>) </td></tr>
  <tr><td  bgcolor=#FF9900><i>Empacado</i></td><td><%=rs("empacado")%></td></tr>
  </table>
<%
sql=" select *, isnull(idorden,0) as aux_idorden from vi_productos_serial where serial='" & esn_nuevo & "'"
'response.write sql & "<br>"  ' la comilla es comentario
  Set rs=conn.execute(sql)
     if not rs.eof then
%>
<table border=1 width="50%">
  <tr><td><i><font color=red>ESN NUEVO</font></i></td><td><%=esn_nuevo%></td></tr>
</table>
<br><input type="submit" name=submit value="Sustituir Ahora">
</form>
<% else  
%>
<table border=1 width="50%">
  <tr><td><i><font color=red>ESN NUEVO</font></i></td><td>NO EXISTE</td></tr>
</table>
<% end if  %>
<% end if 
rs.close %>
</body>
</html>
