<!--#include file="include/seguridad.inc.asp" -->
<html>
<head>
</head>
<body>
<% session("titulo")="Busqueda de ESNs" %>
<!-- #include file="include/titulo1.inc.asp" -->
<%
  response.buffer=true
	datos = session("datosESN")
	response.contentType = "application/vnd.ms-excel"
%>
<table class="tabla" border="1">
  <tr bgcolor="#dddddd" align="center">
	   <td colspan="8" align="center"><b>ESN</b></td>
		 <td colspan="4" align="center"><b>ORDEN</b></td>
		 <td colspan="2" align="center"><b>EMPAQUE</b></td>
		 <td colspan="3"></td>
	</tr>
	<tr bgcolor="#dddddd" align="center">
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
		<td><b>Revisión de Producto</b></td>
	</tr>
	<%
		response.flush
		total = 0
		on error resume next
		for index = 0 to Ubound(datos,2)
		  if datos(24,index) = "BP" then 
		    produce="Logytech Mobile"
 		  else
		    produce="OTRO OPERADOR"
		  end if
		%>
  	<tr>
  	  <td>&nbsp;<%=datos(0,index)%></td>
      <td><%=datos(1,index)%></td> 
      <td><%=datos(2,index)%></td>
      <td><b><%=datos(3,index)%> /Guía Aerea:<%=datos(4,index)%></b></td>
      <td><%=datos(5,index)%>/<%=datos(6,index)%>, Posición <%=datos(7,index)%></a></td>
      <td><%=datos(8,index)%></td> 
      <td><%=datos(9,index)%></td> 
      <td><%=datos(10,index)%></td> 
      <td><%=datos(11,index)%></td>
      <td><%=datos(15,index)%> (<%=datos(16,index)%>)</td>
      <td><%=datos(17,index)%></td>
      <td><%=datos(12,index)%>, Posición:<%=datos(13,index)%>, Línea:<%=datos(14,index)%></td>
      <td><%=datos(21,index)%></td>
      <td><%=datos(22,index)%>, Posición de Empaque <%=datos(23,index)%></td>
      <td><%=datos(19,index)%></td>
      <td><%=produce%></td>
			<td align="center"><%if datos(25,index) = 0 then
							 response.write "<i>No</i>"
							 else
							 response.write "<i>Si</i>" 
			end if  
			
			
			%></td>
	<%
			total = total + 1
			response.flush
		next
	%>
	<tr bgcolor="#dddddd"><td colspan="17"></td></tr>
</table>
</body>
</html>