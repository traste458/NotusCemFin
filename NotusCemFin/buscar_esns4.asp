<% session("titulo")="Busqueda de SIMs"%>
<html>
<body bgcolor="#FFFFFF" text="#000000">
<!--#include file="include/conexion.inc.asp"--> 
<!--#include file="include/titulo1.inc.asp"-->
<a href="buscar_esns.asp">Realizar Otra Busqueda</a> 
<form name="forma" method="post" >
<%
sim= request.form("sim")
if sim = "" then
  sim = request.querystring("sim")
end if
sql= "select * from vi_sims where sim='" & sim & "'"
set rs=conn.execute(sql)
if rs.eof then 
response.write "<font size=4 color=black><i>No existe el SIM No. " & sim & "</i></font>"
  else 
  anulado = "No"
  if cdbl(rs("idestado"))=0 then
    anulado = "Si"
  end if
  %>
 
  <table>
   <tr"#dddddd"><td><i>SIM</i></td><td><% =sim%></td></tr>    
    <tr><td><i>Ingresado</i></td><td><% =rs("fecha") %></td></tr>
    <tr><td><b><i>Anulado</i></b></td><td align=center><b><%=anulado%></b></td></tr>
    <tr><td><i>Factura</i></td><td><% =rs("idfactura2")%></td></tr>
    <tr><td><i>Producto</i></td><td><% =rs("producto")%></td></tr>
    <tr><td><i>Región</i></td><td><% =rs("region")%></td></tr>
    <tr><td><i>Guia Aerea</i></td><td><% =rs("guia_aerea")%></td></tr>
    <tr><td><i>Formulario</i></td><td><% =rs("formulariozf")%></td></tr> 
  </table>
  <hr>
  <%
  sql= "select * from vi_productos_serial where sim='" & sim & "'"
  set rs2=conn.execute(sql)
     if rs2.eof then
     response.write "<h2>Este SIM no ha sido asignado a un ESN </h2>"
  else %>
   <i><b>SIM Asignado al ESN <%=rs2("serial")%></b> (Min:<%=rs2("min")%>)</i><br>
  <table>
  <tr><td><i>Producto-Fábrica</i></td><td><%=rs2("producto")%></td></tr>
  <tr><td><i>Factura-Fábrica</i></td><td><%=rs2("idfactura2")%></td></tr>
  <tr><td><i>Caja/Estiva</i></td><td><%=rs2("caja")%>/<%=rs2("estiva")%></td></tr>
  <tr><td><i>Tecnología</i></td><td><%=rs2("tipo")%></td></tr>
  <tr bgcolor="#dddddd"><td><i>Estado/Lugar</i></td><td><%=rs2("lugar")%></td></tr>
  <tr><td><i>Region</i></td><td><%=rs2("region")%></td></tr>
  <tr>
  <td><i>Orden No.</i></td>
  <td>
    <a href=../BpcolsysOP/adm_operativo/ordenes_mostrar.asp?idorden=<%=rs2("idorden")%>&origen=sim&sim=<%=sim%>>
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
