<html>
<% session("titulo")="Busqueda de ESNs" %>
<body bgcolor="#FFFFFF" text="#000000">
<!--#include file="include/conexion.inc.asp"--> 
<!--#include file="include/titulo1.inc.asp"-->
<a href="buscar_esns.asp">Realizar Otra Busqueda</a> <br>
<form name="forma" method="post" >
<%
esn = Request.Form("esn")
if esn = "" then
  esn = request.querystring("esn")%>
	<a href="javascript:history.back();">Regresar</a> <br>
	<%
end if
    sql="select idorden,fecha,producto,idfactura2,guia_aerea,caja,estiva,factura_secuencia,min,sim,tipo,region,etiquetado,orden_secuencia,linea,empacado,"
    sql=sql & " empaque_caja,empaque_secuencia,revisada,isnull(idorden,0) as aux_idorden from vi_productos_serial where serial='" & esn & "'"
'response.write sql & "<br>"  ' la comilla es comentario
Set rs=conn.execute(sql)
if rs.eof then ' si la consulta dio vacia
  response.write "<font size=4 color=black><i>No existe el ESN No. " & esn & "</i></font></h2>"
  response.end
end if
  if cdbl(rs("aux_idorden")) > 0 then
    sql="select idorden2,fecha,subproducto,tipoorden as tipo, leer_sim, preactivada,cargada from vi_ordenesdetalle"
     sql=sql & " where idorden=" & rs("idorden")
    ' response.write sql & "<br>"
     set rs2=conn.execute(sql)
     if not rs2.eof then
       vidorden2 = rs2("idorden2")
       vorden_fecha = rs2("fecha")
       vsubproducto = rs2("subproducto")
       tipo = rs2("tipo")
       leer_sim = rs2("leer_sim")
       preactivada = rs2("preactivada")
       cargada = rs2("cargada")
     end if
     rs2.close
  end if
  %>
  <table>
  <tr bgcolor="#dddddd"><td><i>ESN No.</i></td><td><%=esn%></td></tr>
  <tr><td><i>Ingreso</i></td><td><%=rs("fecha")%></td></tr> 
  <tr><td><i>Producto-Fábrica</i></td><td><%=rs("producto")%></td></tr>
  <tr><td><i>Fac.Fábrica</i></td><td bgcolor="#dddddd"><b><%=rs("idfactura2")%> /Guía Aerea:<%=rs("guia_aerea")%></b></td></tr>
  <tr><td><i>Caja/Estiba</i></td><td><%=rs("caja")%>/<%=rs("estiva")%>, Posición <%=rs("factura_secuencia")%></td></tr>
  <tr><td><i>MIN</i></td><td><%=rs("min")%></td></tr> 
  <tr><td><i>SIM</i></td><td><%=rs("sim")%></td></tr> 
  <tr><td>Tecnología</td><td><%=rs("tipo")%></td></tr> 
  <tr bgcolor="#dddddd"><td><i>ORDEN</i></td></tr> 
  <tr><td><i>Region</i></td><td><%=rs("region")%></td></tr>
  <tr><td><i>Orden No.</i></td><td><a href=../BpcolsysOP/adm_operativo/ordenes_mostrar.asp?idorden=<%=rs("idorden")%>&origen=serial&esn=<%=esn%>>
                        <font size="2" face="Arial" <%=colorLink%>><b><%=vidorden2%>(<%=vorden_fecha%>)</b></a></td>
	<% if rs("revisada") = 0 then  %>
	<br><font size=2 color="#0000ff"> <i>Orden sin revisión de producto</i></font>
	<% end if  %>
	</td></tr>
  <tr><td><i>Producto Final</i></td><td><%=vsubproducto%></td></tr>
  <tr><td><i>Etiquetado/Posición</i></td><td><%=rs("etiquetado")%>, 
   Posición:<%=rs("orden_secuencia")%>, Línea:<%=rs("linea")%></td></tr>
  <tr bgcolor="#dddddd"><td>EMPAQUE</td><td></td></tr>
  <tr><td><i>Empacado</i></td><td><%=rs("empacado")%></td></tr>
  <tr><td><i>Caja/Sec.Empaque.</i></td><td><%=rs("empaque_caja")%>, Posición de Empaque <%=rs("empaque_secuencia")%></td></tr>
  <tr bgcolor=#CCCCCC><td><i>Preactivada</i></td><td><%=preactivada%></td></tr>
  <tr bgcolor=#CCCCCC><td><i>Cargada</i></td><td><%=cargada%></td></tr>
  </table>
<hr>

<%
'sql = "select rep.serial,rep.min,rep.sim,(select rtrim(idorden2) from ordenes " 
'sql = sql & " where idorden=rep.idorden) as idorden2,rep.reprocesado,isnull(idordenant,0) "
'sql = sql & " as idOrdenAnt,idorden from reprocesos rep where rep.serial='" & esn & "' " 
'sql = sql & " order by reprocesado "

sql = "select rep.serial,rep.min,rep.sim,o.idorden2,rep.reprocesado,isnull(idordenant,0)" 
sql = sql & "as idOrdenAnt,rep.idorden,o.fecha_inicio as fechaOrden,revisada from reprocesos rep with(nolock)" 
sql = sql & " inner join ordenes o with(nolock) on rep.idorden = o.idorden where rep.serial='" & esn & "'"
sql = sql & " union "
sql = sql & "select rep.serial,rep.min,rep.sim,o.idorden2,null as reprocesado,idordenant as idOrdenAnt,"
sql = sql & " idordenant as idorden, o.fecha_inicio as fechaOrden,revisada from reprocesos rep with(nolock) inner" 
sql = sql & " join ordenes o with(nolock) on rep.idordenant = o.idorden where rep.serial='" & esn & "' and" 
sql = sql & " idordenant not in (select idorden from reprocesos with(nolock) where serial ='" & esn & "')"
sql = sql & " order by fechaOrden,reprocesado"
'response.write sql
set rs=conn.execute(sql)
esOtroOperador = false
if not rs.eof then
  laOrden = trim(rs("idorden2"))
	idOrden = rs("idorden")
	idOrdenAnt = rs("idOrdenAnt")
	if left(Ucase(laOrden),4)="OTR-" and (cdbl(idOrdenAnt)= 0 or cdbl(idOrden) = cdbl(idOrdenAnt)) then
	  esOtroOperador = true
	end if
else
  if left(Ucase(trim(vidorden2)),4) = "OTR-" then
	  esOtroOperador = true
	end if
end if
if esOtroOperador = true then
%>
  <ul>
	  <font size="3" face="Arial" color="red"><b>Serial Producido Por Otro Operador</font>
	</ul>
  <hr>
<%
end if
%>
<b>Traza de Reprocesos</b><br>
<%
'sql = "select * from reprocesos where serial ='"&esn&"'"
'set rs=conn.execute(sql)
if rs.eof then
   response.write "<br><font color=gray>Este serial no ha sido reprocesado</font>"
   rs.close
  response.end
end if %>
</table>
  <hr>
  <table border="1" cellpadding="1" cellspacing="2">
  <tr align="center" bgcolor="#DDDDDD"><th>ESN</th><th>Min</th><th>Sim</th><th>Destino</th><th>Reprocesado</th><th>Fecha Orden</th><th>Revisado</th></tr>
  <%
   i=0
   while not rs.eof %>
     <tr>
		 		 <td><%=rs("serial")%>&nbsp;</td>
				 <td><%=rs("min")%>&nbsp;</td>
				 <td><%=rs("sim")%>&nbsp;</td>
                 <td>
                    <a href=../BpcolsysOP/adm_operativo/ordenes_mostrar.asp?idorden=<%=rs("idorden")%>&origen=serial&esn=<%=esn%>>
                        <font size="2" face="Arial" <%=colorLink%>><b><%=rs("idorden2")%></b>
                    </a>                  
                 </td>
				 <td><%=rs("reprocesado")%>&nbsp;</td>
				 <td><%=rs("fechaOrden")%>&nbsp;</td>
				 <td align="center"><% if rs("revisada") = 0 then response.write("NO") else response.write("SI") end if%></td>
		 </tr>
     <%
     rs.movenext
   wend   %>
   </table>
<%
rs.close %>
</form>
</body>
</html>