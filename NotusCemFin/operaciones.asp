<%@ LANGUAGE="VBScript" %>
<!--#include file="include/seguridad.inc.asp" -->
<% Response.Buffer=True %>
<% session("titulo")=("BPCOLSYS Administrador Logistico") %>

<html>
<head>
<title>admistracion</title>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" >
</head>
<body bgcolor="#FFFFFF" text="#000000" background="">
<!-- #include file="include/titulo1.inc.asp" -->

<p><center><font size="3" color="black" face= "Arial"><b>Sistema de Informaci�n para registro, programaci�n e inventarios de <br>
Celulares en Tecnologias TDMA y GSM.</b></center><br><br>

En este perfil coordinamos:<br><br>

1. Subimos planos provenientes de los fabricantes con ESNs, SIMs y MINes<br><br>

2. Registramos las facturas de importaci�n y capturamos con lector optico tel�fono por tel�fono, comprandolo
  con el archivo plano cargado previamente.<br><br>

3. Definimos las personas que trabajaran en cada l�nea de producci�n.<br><br>

4. Definimos los productos terminados, sus caracteristicas por regi�n (Oriente,Occidente,etc)
  y el tipo de tecnologia al que pertenecen (TDMA o GSM). <br><br>

5. Creamos las ordenes de trabajo y asignamos la linea en la que se empacar�. <br><br>

6. Con nuestro modulo visual capturaremos tel�fono por tel�fono e imprimiremos las respectivas etiquetas (steackers)
   en codigo de barras.<br><br>

7. Registramos el empaque.<br><br>

  </font></p>
<br>
<br>
<br>
<font color=black size=2 face="Arial"><b>Brightpoint de Colombia Inc.</b></font>

</body>
</html>
