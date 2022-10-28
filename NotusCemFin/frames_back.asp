<!--#include file="include/seguridad.inc.asp" -->
<!--#include file="include/conexion.inc.asp" -->
<%
idmenu = request.querystring("idmenu")
posicion = request.querystring("posicion")
control = request.querystring("control")
%>
<HTML>
<HEAD>
<script language="JavaScript">
<!--
function desplegar(nombreMenu){
 if(document.getElementById(nombreMenu.replace("tbl","mnu"))){
  var menu = document.getElementById(nombreMenu.replace("tbl","mnu"));
  var numeroMenus = parseInt(document.forma.numMenus.value);
  if(menu.style.display == "none"){
    menu.style.display = "block";
	document.getElementById(nombreMenu.replace("tbl","img")).src="images/arrow_up2.gif";
	for(i=0;i<numeroMenus;i++){
	  if("mnu_"+i.toString()!=menu.id){
	    //alert("mnu_"+i.toString())
	    document.getElementById("mnu_"+i.toString()).style.display = "none"
	    document.getElementById("img_"+i.toString()).src="images/arrow_down2.gif"
	  }
	}
  }
  else{
    menu.style.display = "none";
	  document.getElementById(nombreMenu.replace("tbl","img")).src="images/arrow_down2.gif"
  }
}
}


function cambiarEstilo(obj,accion,itemIndex){
  var colorFondo,colorFuente,clase;
	
  if(accion==1){
	  colorFondo = "#5B235A";
		colorFuente = "White";
		clase = "fuenteUrlActiva";
	}else{
	  colorFondo = "#E5D9E5";
		colorFuente = "#883485";
		clase = "fuenteUrl";
	}
	obj.style.backgroundColor=colorFondo;
	document.getElementById("laFuente"+itemIndex.toString()).style.color=colorFuente;
	document.getElementById("fuenteUrl"+itemIndex.toString()).className=clase;
}
//-->
</script>
<LINK REL="StyleSheet" HREF="css/vinculos.css" TYPE="text/css">
<LINK REL="StyleSheet" HREF="include/styleBACK.css" TYPE="text/css">
</HEAD>
<BODY class="cuerpo2">
<form name="forma">
<%
If (IsNull(idmenu)) or (idmenu = "") then
%>
  <br>
  <br>
  <br>
  <br>
  <br>
  <center><IMG BORDER="0" SRC="images/logytechValores_medium.png"></center>
  <br>
  <br>
<%

else
'subimos el menu a session
session("idmenu") = idmenu
session("posicion") = posicion
if control = "" then
   sesionesASP = "&sASP=" & session("usxp001") & ";" & session("usxp002") & _
       ";" & session("usxp003") & ";" & session("usxp004") & ";" & session("usxp005") & _
			 ";" & session("usxp006") & ";" & session("usxp007") & ";" & session("usxp008") & _
			 ";" & session("usxp009") & ";" & session("usxp010") & ";" & session("usxp012")
   response.redirect "ControladorSesiones.aspx?idmenu=" & idmenu & "&posicion=" & posicion & sesionesASP 
end if

'mostrar menus

sql = "select a.idmenu, a.menu, a.url from menus a, menus_perfiles b where a.idmenupadre = b.idmenu and b.idperfil = " & session("usxp009")
sql = sql & " and a.idmenupadre = " & idmenu & " order by a.posicion "
'response.write sql
set rs = conn.execute (sql)
%>
<br>
<ul>
<table class="tabla" >
  <tr><td>
<%
  contador = 0
	cont = 0 
  while not rs.eof
    if (isnull(rs("url"))) or (rs("url") = "") then
		  if contador <>0 then%>
			  </ul>
				</td></tr>
				</table>
		<%end if%>
		    <br>
				<table width="100%">
          <tr>
					  <td>
						  <table class="tablamenu" width="100%" id="tbl_<%=contador%>" style="cursor:hand" onclick="desplegar(this.id)">
							  <tr>
								  <td width="100%"><%=trim(rs("menu"))%></td>
									<%
                  if contador = 0 then
									  laImagen = "images/arrow_up2.gif"
                  else
                    laImagen = "images/arrow_down2.gif"
                  end if
					       %>
								 <td align="right"><img id="img_<%=contador%>" src="<%=laImagen%>" alt="Dar click para ver u ocultar opciones"></td>
					      </tr>
							</table>
						</td>
					</tr>
					<tr bgcolor="#E5D9E5"><td>
					<%
					  'if contador = 0 then
						'  display = "block"
						'else
						'  display = "none"
						'end if
					%>
				<ul id="mnu_<%=contador%>" style="DISPLAY:<%=display%>; MARGIN: 5px">
		<%contador= contador + 1
		  else%>
          <li id="opc<%=cont%>" style="LIST-STYLE-TYPE: none;" onmouseover="cambiarEstilo(this,1,<%=cont%>);this.style.cursor='hand'" onmouseout="cambiarEstilo(this,2,<%=cont%>);" onclick=" javascript: window.location.href='<%=trim(rs("url"))%>'"> 
						   <font id="laFuente<%=cont%>" size="1" color="#883485"><b><%=trim(rs("idmenu"))%></b></font>
              <a href="<%=trim(rs("url"))%>"><font id="fuenteUrl<%=cont%>" class="fuenteUrl"><b><%=trim(rs("menu"))%></b></font></a>
          </li>
    <%
		    cont = cont + 1
		  end if
    rs.movenext
  wend%>
	 </ul>
	 </td></tr>
	 </table>
<%	
end if
response.flush
%>
</td></tr>
</table>
</ul>
 <input type="hidden" name="numMenus" id="numMenus" value="<%=contador%>">
</form>
<script language="javascript" type="text/javascript">
 // desplegar("tbl_0");
</script>
</BODY>
</HTML>

<%

conn.close
%>
