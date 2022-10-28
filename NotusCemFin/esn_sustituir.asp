<%@ language= "VBscript"%>
<% session("titulo") = "Sustituir ESNs" %>
<html>
<head>
<title>Sustituir</title>
<Script Language="JavaScript">
  function validacion ()
	{
         		if (document.forma.esn.value  ==  "")
			{
			alert ("Digite el Actual ESN");
			document.forma.esn.focus();
			return (false);
			}
			if (document.forma.esn_nuevo.value  ==  "")
			{
			alert ("Digite el Nuevo ESN");
			document.forma.esn_nuevo.focus();
			return (false);
			}
	}
</Script>

</head>
<body bgcolor="#FFFFFF" text="#000000">
<!--#include file="include/titulo1.inc.asp"-->
  &nbsp; &nbsp; <br>
<a href="adm_operativo/otras_opciones.asp">Regresar</a>
<form name="forma" method="post" action="esn_sustituir2.asp" onSubmit="return validacion ()">  
<font color=blue size=5>Buscando ESNs</font> 
   <table>
    <tr><td><font color=blue><i>ESN Actual</i></font></td><td><input type="text" name="esn"></td>
<BR>
    <tr><td><font color=blue><i>ESN Nuevo</i></font></td><td><input type="text" name="esn_nuevo"></td>
     <td><input type="submit" name=submit value="Continuar .."></td></tr>
  </table>
</form>
<Script Language="JavaScript">
   document.forma.esn.focus();
</Script>
</body>
</html>
