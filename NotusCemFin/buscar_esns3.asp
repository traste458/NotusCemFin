<html>
<% session("titulo")="Busqueda de MINS"%>
<body bgcolor="#FFFFFF" text="#000000">
<!--#include file="include/conexion.inc.asp"--> 
<!--#include file="include/titulo1.inc.asp"-->
<a href="buscar_esns.asp">Realizar Otra Busqueda</a> 
<form name="forma" method="post" >
<%
min = request.form("min")
if min = "" then
  min = request.querystring("min")
 end if
'response.Write(min)
'response.End
sql= "select * from vi_mines where min='" & min & "'"
set rs=conn.execute(sql)
if rs.eof then 
response.write "<font size=4 color=black><i>No existe el MIN No. " & min & "</i></font>"
  else 
  anulado = "No"
  if cdbl(rs("estado"))=0 then
    anulado = "Si"
  end if
  %>
 
  <table>
   <tr"#dddddd"><td><i>MIN</i></td><td><% =min %></td></tr>    
    <tr><td><i>Ingresado</i></td><td><% =rs("fecha") %></td></tr>
    <tr><td><b><i>Anulado</i></b></td><td align=center><b><%=anulado%></b></td></tr>
  </table>
  <hr>
  <%
  sql= "select * from vi_productos_serial where min='" & min & "'"
  'response.Write(sql)
  'response.End
  set rs2=conn.execute(sql)
     if rs2.eof then
     response.write "<h2>Este MIN no ha sido asignado a un ESN </h2>"
  else %>
   <i><b>MIN Asignado al ESN <%=rs2("serial")%></b></i><br>
  <table>
  <tr><td><i>Producto-Fábrica</i></td><td><%=rs2("producto")%></td></tr>
  <tr><td><i>Factura-Fábrica</i></td><td><%=rs2("idfactura2")%></td></tr>
  <tr><td><i>Caja/Estiva</i></td><td><%=rs2("caja")%>/<%=rs2("estiva")%></td></tr>
  <tr><td><i>Tecnología</i></td><td><%=rs("tipo")%></td></tr>
  <tr bgcolor="#dddddd"><td><i>Estado/Lugar</i></td><td><%=rs2("lugar")%></td></tr>
  <tr><td><i>Region</i></td><td><%=rs2("region")%></td></tr>
  <tr>
  <td><i>Orden No.</i></td>
  <td>
    <a href=../BpcolsysOP/adm_operativo/ordenes_mostrar.asp?idorden=<%=rs2("idorden")%>&origen=min&min=<%=min%>>
        <font size="2" face="Arial" <%=colorLink%>><b><%=rs2("idorden2")%></b>
    </a>                  
  </td></tr>
  <tr><td><i>Ingreso</i></td><td><%=rs2("fecha")%></td></tr>
  <tr><td><i>Etiquetado</i></td><td><%=rs2("etiquetado")%></td></tr>
  <tr><td><i>Empacado</i></td><td><%=rs2("empacado")%></td></tr>
  </table>
  <%
  end if
  rs2.close

end if
rs.close %>

</form>
</body>
</html>
