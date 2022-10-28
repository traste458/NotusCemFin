<%@ language= "VBscript"%>
<!--#include file="include/seguridad.inc.asp" -->
<% session("titulo") = "Buscar ESNs/MINs/SIMs" %>
<html>
<SCRIPT LANGUAGE="JavaScript">
function comprobar_esn() {
 if(document.forma_esn.esn.value == "" ){
   Error() ;
 } else { 
   document.forma_esn.submit();
 }
}

function validarArchivo(){
  if(document.formaArchivo.archivo.value=="")
	{
	  alert("Escoja el archivo que contiene los ESNs a buscar, Por Favor");
		document.formaArchivo.archivo.focus();
		return(false); 
	}
	else
	{
	  document.formaArchivo.submit();
	}
}

function comprobar_min()        {
if(document.forma_min.min.value == ""){
   Error2() ;
 }else{ 
   document.forma_min.submit() ;
   }
}

function comprobar_sim()        {
if(document.forma_sim.sim.value == ""){
   Error3() ;
 }else{ 
   document.forma_sim.submit() ;
   }
}



function Error() {
 alert("Favor Diligite el ESN a buscar");
 document.forma_esn.esn.focus();
}

function Error2(){
   alert("Favor Diligite el MIN a buscar");
   document.forma_min.min.focus();
}

function Error3(){
   alert("Favor Diligite el SIM a buscar");
   document.forma_sim.sim.focus();
}


</SCRIPT>

</head>
<head>
<title>Buscar ESNs/ MINs/ SIMs</title>
</head>
<body bgcolor="#FFFFFF" text="#000000">
<!--#include file="include/titulo1.inc.asp"-->
  &nbsp; &nbsp; <br>
  <table summary="" >
    <tr>
      <td>
			  <form name="forma_esn" method="post" action="buscar_esns2.asp">
  			  <font color=blue size=5 face="Arial">Buscar ESN:</font>
          <input type="text" name="esn"><br>
          <input type="button" onclick="comprobar_esn () ;" value="Buscar ahora">
			  </form>		
			</td>
		</tr>	
		<tr>	
			<td>
			  <form name="formaArchivo" method="post" action="buscarESNArchivo.asp" enctype="multipart/form-data">
  			  <font color=blue size=5 face="Arial">Buscar Grupo de ESNs:</font>
          <input type="file" id="archivo" name="archivo" size="40"><br>
          <input type="button" onclick="validarArchivo();" value="Buscar ahora">
				</form>
			</td>
    </tr>
  </table>  
<hr>

<form name="forma_min" method="post" action="buscar_esns3.asp">
  <font color=blue size=5 face="Arial">Buscar MIN:</font> 
   </td><td><input type="text" name="min"><br>
     <input type="button" onclick="comprobar_min () ;" value="Buscar ahora">
</form>
<HR>
<form name="forma_sim" method="post" action="buscar_esns4.asp">
  <font color=blue size=5 face="Arial">Buscar SIM:</font> 
   </td><td><input type="text" name="sim"><br>
     <input type="button" onclick="comprobar_sim () ;" value="Buscar ahora">
</form>



<Script Language="JavaScript">
   document.forma_esn.esn.focus();
</Script>

</body>
</html>
