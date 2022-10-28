<html>
<body>
<!--#include file="../include/conexion.inc.asp" -->
<!-- #include file="../include/titulo1.inc.asp" -->

<% sql="select * from vi_ordenes "
Set rs=conn.execute(sql) 
%>
  
  <table border=1>
    <tr bgcolor=skyblue align=center> 
      <td><font face="Arial, Helvetica, sans-serif" size="2">Fecha</font></td>
      <td><font face="Arial, Helvetica, sans-serif" size="2">Orden No.</font></td>
      <td><font face="Arial, Helvetica, sans-serif" size="2">Producto</font></td>
      <td><font face="Arial, Helvetica, sans-serif" size="2">Operario</font></td>
      <td><font face="Arial, Helvetica, sans-serif" size="2">Cantidad<br>Pedida</font></td>
      <td><font face="Arial, Helvetica, sans-serif" size="2">Cantidad<br>Empaque</font></td>
      <td><font face="Arial, Helvetica, sans-serif" size="2">Estado<br>Susp./Anular</font></td>
    </tr>
    
      <td><font face="Arial, Helvetica, sans-serif" size="2"><%=mid(rs("fecha"),1,9)%></font></td>
      <td> <font face="Arial, Helvetica, sans-serif" size="2">
    <a href=ordenestrabajo_mostrar.asp?idorden=<%=rs("idorden")%>><%=rs("idorden2")%> </a> </font></td>
      <td><font face="Arial, Helvetica, sans-serif" size="2"><%=rs("subproducto")%>	</font></td>
      <td><font face="Arial, Helvetica, sans-serif" size="2"><%=rs("tercero")%></font></td>
      <td align=right><font face="Arial, Helvetica, sans-serif" size="2"><%=rs("cantidad_pedida")%></font></td>
      <td align=right><font face="Arial, Helvetica, sans-serif" size="2"><%=rs("cantidad_empaque")%></font></td>       
      <td><font face="Arial, Helvetica, sans-serif" size="2">
         <% if cdbl(rs("estado")) = 0 then %>
          <%=estado%>
         <% else %>
           <a href=ordenesestado.asp?idorden=<%=rs("idorden")%>><%=estado%></a></font>
         <%end if %>
        </td>
    </tr>
    <%rs.movenext
wend %>
  </table>
</body>
</html>
