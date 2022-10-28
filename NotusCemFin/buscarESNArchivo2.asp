<html>
<head>
  <link rel="stylesheet" type="text/css" href="include/styleBACK.css"/>
	<script type="text/javascript" language="javascript">
	  function abrirVentana()
		 {
		   var parametros;
			 parametros = "resizable=yes,directories=no,scrollbars=yes,toolbar=no,menubar=yes"
			 window.open("buscarESNArchivoExcel.asp","buscarESNExcel",parametros);
		 }
	</script>
</head>
<% session("titulo")="Busqueda de ESNs" %>
<body bgcolor="#FFFFFF" text="#000000" class="cuerpo2">
<!--#include file="include/conexion.inc.asp"--> 
<!--#include file="include/titulo1.inc.asp"-->
<a href="buscar_esns.asp">Realizar Otra Busqueda</a> <br>
<form name="forma" method="post" >
<%
archivo = "ESNsConsultar" & session("usxp001") & ".txt"
Server.ScriptTimeout = 1000
'Leer archivo con seriales
set objFSO = Server.createObject("Scripting.FileSystemObject")
wPlano = server.mapPath("archivos_planos/" & archivo)
'response.write wPlano
'response.end
set objTStream = objFSO.OpenTextFile(wPlano)
cadena=""
linea=""
do While Not objTStream.AtEndOfStream
  linea = objTStream.ReadLine
	if trim(linea)<>"" then
	  cadena = cadena & "," & "'" & trim(linea) & "'" '& vbcrlf
	end if
loop
if not objTStream is nothing then objTStream.close
if left(cadena,1) = "," then
  cadena = right(cadena,len(cadena)-1)
end if
if right(cadena,1) = "," then
  cadena = left(cadena,len(cadena)-1)
end if
cadena = "(" & cadena & ")" 
'response.write cadena
'response.end 
sqlSelect = "select serial,fecha,rtrim(producto) as producto,rtrim(idfactura2) as idfactura2,"
sqlSelect = sqlSelect & "guia_aerea,caja,estiva,factura_secuencia,[min],sim,rtrim(tipo) as tipo,region,"
sqlSelect = sqlSelect & "etiquetado,orden_secuencia,linea,"
sqlSelect = sqlSelect & "(select rtrim(idorden2) from vi_ordenesdetalle with(nolock) where idorden=vi.idorden) as idorden2,"
sqlSelect = sqlSelect & "(select fecha from vi_ordenesdetalle with(nolock) where idorden=vi.idorden) as fecha2,"
sqlSelect = sqlSelect & "(select rtrim(subproducto) from vi_ordenesdetalle with(nolock) where idorden=vi.idorden) as subproducto,"
sqlSelect = sqlSelect & "(select leer_sim from vi_ordenesdetalle with(nolock) where idorden=vi.idorden) as leer_sim,"
sqlSelect = sqlSelect & "(select preactivada from vi_ordenesdetalle with(nolock) where idorden=vi.idorden) as preactivada,"
sqlSelect = sqlSelect & "(select cargada from vi_ordenesdetalle with(nolock) where idorden=vi.idorden) as cargada, "
sqlSelect = sqlSelect & "empacado,empaque_caja,empaque_secuencia,"
sqlSelect = sqlSelect & "case when (idorden2 like 'OTR-%' and (select count(0) from reprocesos with(nolock) "
sqlSelect = sqlSelect & " where serial=vi.serial)=0) or ((select rtrim(idorden2) from ordenes with(nolock) "
sqlSelect = sqlSelect & "  where idorden=(select max(idorden) from reprocesos with(nolock) where serial=vi.serial " 
sqlSelect = sqlSelect & "  and reprocesado=(select min(reprocesado) from reprocesos with(nolock) "
sqlSelect = sqlSelect & "  where serial=vi.serial))) like 'OTR-%') then 'OO' else 'BP' end as origen,revisada "
sqlSelect = sqlSelect & " from vi_productos_serial vi with(nolock) where serial in " & cadena

set rsDetalle = conn.execute(sqlSelect)
'response.write sqlselect
'response.end
if not rsDetalle.eof then%>
  <a href="javascript:abrirVentana();" ><font color="blue"><b>Ver Reporte en Excel</b></font></a>
 <table class="tablapequena">
  <tr bgcolor="#dddddd">
	   <td colspan="8" align="center"><b>ESN</b></td>
		 <td colspan="4" align="center"><b>ORDEN</b></td>
		 <td colspan="2" align="center"><b>EMPAQUE</b></td>
		 <td colspan="2"></td>
	</tr>
	<tr bgcolor="#f0f0f0" align="center">
	  <td><b>ESN No.</b></td>
	  <td><b>Ingreso</b></td>
		<td><b>Producto-Fábrica</b></td>
		<td><b>Fac.Fábrica</b></td>
		<td><b>Caja/Estiba</b></td>
		<td><b>MIN</b></td>
		<td><b>SIM</b></td>
		<td><b>Tecnología</b></td>
		<td><b>Región</b></td>
		<td><b>Orden No.</b></td>
		<td><b>Producto Final</b></td>
		<td><b>Etiquetado/Posición</b></td>
		<td><b>Empacado</b></td>
		<td><b>Casa/Sec.Empaque</b></td>
		<td><b>Preactivada</b></td>
		<td><b>Producido Por</b></td>
	</tr>
<%  datos = rsDetalle.GetRows
	session("datosESN") = datos
	set rsDetalle = nothing
	for index = 0 to Ubound(datos,2)
	  if datos(24,index) = "BP" then 
		  produce="Logytech Mobile"
		else
		  produce="OTRO OPERADOR"
		end if
	%>
	<tr>
	  <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(0,index)%></a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(1,index)%></a></td> 
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(2,index)%></a></td>
    <td><b><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(3,index)%> /Guía Aerea:<%=datos(4,index)%></b></a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(5,index)%>/<%=datos(6,index)%>, Posición <%=datos(7,index)%></a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(8,index)%></td> 
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(9,index)%></a></td> 
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(10,index)%></a></td> 
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(11,index)%></a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(15,index)%> (<%=datos(16,index)%>)  
		<% if datos(25,index) = 0 then  %>
		<br><font size=1 color="#0000FF"> <i>Sin revisión de producto</i></font>
		<% end if  %>
		</a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(17,index)%></a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(12,index)%>, Posición:<%=datos(13,index)%>, Línea:<%=datos(14,index)%></a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(21,index)%></a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(22,index)%>, Posición de Empaque <%=datos(23,index)%></a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=datos(19,index)%></a></td>
    <td><a href="buscar_esns2.asp?esn=<%=datos(0,index)%>"><%=produce%></a></td>
<%next%>
  <tr bgcolor="#dddddd">
	   <td colspan="16"><b><%=index%> Registros</b></td>
	</tr>
</table>
<%else%>
 <center>
	  <font color="red" face="arial" size="3"><b>No se encontraron datos</b></font>
	</center>
<%
  conn.close
  set conn = nothing
  response.end
end if
conn.close
set conn = nothing
%>
</form>
</body>
</html>