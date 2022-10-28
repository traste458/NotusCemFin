<%
Server.ScriptTimeout = 2000
Dim upl, NewFileName, objFSO, objTStream, intLineNum, cadena, linea
Set upl = Server.CreateObject("ASPSimpleUpload.Upload")
archivo = upl.form("archivo")
if right(archivo,4)<>".txt" then
  %>
  <a href="buscar_esns.asp">Regresar</a>
  <center><font color="red" size=4><b>ERROR: Formato de archivo incorrecto. Se espera un archivo de Texto con extensión .txt<br>
	Está tratando de subir un archivo con extensión <%=right(archivo,4)%></b></font></center> 
  <%
	response.end
end if
div ="<center> <div id=""espera"" style=""display:block"">"
div = div & "<font face=arial size=2><b> Subiendo archivo. Por favor espere...</b></font>"
div = div & "<br><img src=""images/kit_black.gif"" height=20 width = 100>"
div = div & "</div></center>"
Response.write div
wPlano = "ESNsConsultar" & session("usxp001") & ".txt"
nuevoArchivo = "archivos_planos/" & wPlano
if not upl.SaveToWeb("archivo",nuevoArchivo) then
  response.write "<script language=""JavaScript"">espera.style.display='none';</script>"
  %>
  <a href="buscar_esns.asp">Regresar</a>
  <center><font color="red" size=4><b>ERROR: No se pudo subir el archivo</b></font></center> 
  <%
	set upl = nothing
	response.end
end if
set upl = nothing
response.redirect("buscarESNArchivo2.asp")
%>
