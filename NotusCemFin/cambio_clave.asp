<!--#include file="include/seguridad.inc.asp"-->
<!--#include file="include/conexion.inc.asp"-->
<html>
<head>
<SCRIPT LANGUAGE="JavaScript" TYPE="Text/JavaScript">
function validacion () {
  if (document.forma.c1.value == "") {
    alert ("Escriba su clave actual.");
    document.forma.c1.focus();
    return (false);
  }
  if (document.forma.c2.value == "") {
    alert ("Escriba su nueva Clave.");
    document.forma.c2.focus();
    return (false);
  }
  if (document.forma.c3.value == "") {
    alert ("Confirm su nueva Clave.");
    document.forma.c3.focus();
    return (false);
  }
  if (document.forma.c2.value == document.forma.usuario.value)
   {
     alert ("La clave no cumple con los requisitos de seguridad. No puede tener el mismo nombre de usuario");
     document.forma.c2.value = "";
     document.forma.c3.value = "";
     document.forma.c2.focus();
     return (false);
   }
  if (document.forma.c2.value.length < 5)
   {
     alert ("La clave no cumple con los requisitos de seguridad. Debe tener como mínimo 5 digitos");
     document.forma.c2.value = "";
     document.forma.c3.value = "";
     document.forma.c2.focus();
     return (false);
   }


//validacion de nombres en la clave. no puede tener el partes del nombre en la clave
    var verificar_clave
    var mayusculas
    var contador = 0;
    var clavetemp = document.forma.c2.value.toUpperCase();
    mayusculas = document.forma.tercero.value.toUpperCase();
    verificar_clave = mayusculas.split(" ");
  
    
   

}
</SCRIPT>
</head>
<body>
<% titulo="Cambio de Clave" %>
<!--#include file="include/titulo1.inc.asp"-->
<hr>
<form name=forma method=post action="cambio_clave2.asp" onsubmit="return validacion();">
<%
estado= request.querystring("estado")
if estado = 1 then %>
  <center><font color=red size=4 face=arial>Su clave es incorrecta. No se Actualiza.</font></center>
  <%
end if
if estado = 2 then %>
  <center><font color=red size=4 face=arial>La confirmación no concuerda con su nueva clave. No se Actualiza.</font></center>
  <%
end if
if estado = 3 then %>
  <center><font color=blue size=4 face=arial>Su clave ha sido actualizada.</font></center>
  <%
end if
if estado = 4 then %>
  <center><font color=red size=4 face=arial>Por favor cambie la clave. La clave no se Actualizó.</font></center>
  <%
end if

%>
<ul>
  <table border=0>
    <tr> 
      <td><i><font size="2">Clave Actual</font></i></td>
      <td><input type=password name=c1 size=10 maxlenght=10></td>
    </tr>
    <tr> 
      <td><i><font size="2">Clave Nueva</font></i></td>
      <td><input type=password name=c2 size=10 maxlenght=10></td>
    </tr>
    <tr> 
      <td><i><font size="2">Confirme Clave</font></i></td>
      <td><input type=password name=c3 size=10 maxlenght=10></td>
    </tr>
  </table>
  <br><input type=submit name=boton value=Continuar>
</ul>
<%
sql = "select usuario from terceros where idtercero = " & session("usxp001")
set rs = conn.execute(sql)

%>
<input type="hidden" name="usuario" value="<%=rs("usuario")%>">
		

<input type="hidden" name="tercero" value="<%=session("usxp002")%>">
</form>
</body>
<script language="JavaScript">
  document.forma.c1.focus();
</script>
</html>

<%
conn.close
set conn = nothing

%>