<!--#include file="include/seguridad.inc.asp" -->
<!--#include file="include/conexion.inc.asp" -->
<HTML>
<HEAD>
<script language="JavaScript">
<!--
function validacion()
{
  if (document.forma.idmenu.value == "")
  {
    return false;
  }
	var expReg = /^[0-9]+$/
  if (!expReg.test(document.forma.idmenu.value) )
  {
    document.forma.idmenu.value = "";
    return false;
  }
}
//-->
</script>
<LINK REL="StyleSheet" HREF="css/vinculos.css" TYPE="text/css">
<LINK REL="StyleSheet" HREF="include/styleBACK.css" TYPE="text/css">
<script type="text/javascript" src="include/jquery.tools.min.js"></script>
<style type="text/css">
<!--
  A:link, A:visited {text-decoration:none;color:#FFFFFF;font-size:7pt;font-family: verdana}
  A:active, A:hover {text-decoration:none;color:#FFC726;font-size:7pt;font-family: verdana}
//-->
</style>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        $('#divAyuda').load('Handlers/ObjetoAyuda.ashx?idPerfil=<%=session("usxp009") %>');
    });
</script>
</HEAD>
<BODY BGCOLOR="FFFFFF" LEFTMARGIN="0" TOPMARGIN="0" RIGHTMARGIN="0" BOTTOMMARGIN="0" CLASS="cuerpo">
<TABLE BORDER="0" WIDTH="100%"  CELLPADDING="0" CELLSPACING="0" ALIGN=CENTER class="tabla">
  <TR>
    <TD HEIGHT="60" ALIGN="center" valign="center">&nbsp;&nbsp;&nbsp;<IMG BORDER="0" SRC="images/logo_trans.png" ></TD>
    <TD HEIGHT="60" VALIGN="TOP" ALIGN="LEFT">
      <TABLE WIDTH=100% class="tabla">
        <TR>
          <TD COLSPAN="3"><font size="2" class="fuente"><b>Usuario: </b><%=session("usxp002") & " (" &session("usxp004") &")"%></font></TD>
            <td rowspan="3" style="vertical-align: middle;">
                <div id="divAyuda" name="divAyuda"></div> 
            </td>
        </TR>
        <TR>
          <TD COLSPAN="3"><font size="2" class="fuente"><b>Fecha: </b><%=FormatDatetime(date(), 1)%></font>&nbsp;<font size="3" Color="red" class="fuente"><!--<b>DESARROLLO</b>!-->&nbsp;</font></TD>
        </TR>
        <TR>
          <TD WIDTH="30%" BGCOLOR="#333333" align="center">
            <A HREF="login.asp" target="_parent"><b>Cambiar Usuario</b></A>
          </TD>
          <TD WIDTH="30%" BGCOLOR="#333333" align="center">
            <A HREF="JavaScript: top.close()"><b>Salir</b></A>
          </TD>
          <TD WIDTH="30%" BGCOLOR="#333333" align="center">
            <A HREF="cambio_clave.asp" target="Back"><b>Cambiar Clave</b></A>
          </TD>

        </TR> 
      </TABLE>   
    </TD>
    <TD HEIGHT="60" ALIGN="center" valign="center">&nbsp;&nbsp;&nbsp;<img BORDER="0" SRC="images/LogoNotus.png" width="145" height="55" ></TD>
  </TR>
</table>
<center>
<table BORDER="0" WIDTH="100%"  CELLPADDING="0" CELLSPACING="0" VALIGN="TOP" ALIGN="center" bgcolor="#883485"  >
  <TR ALIGN="center">
  <%
  sql = "select a.idmenu, a.menu, b.posicion from menus a, menus_perfiles b where a.idmenu = b.idmenu and b.idperfil = " & session("usxp009")
  sql = sql & " and a.idmenupadre is null order by b.posicion"
  'response.write sql
  'Response.end
  set rs = conn.execute(sql)
  while not rs.eof
  %>
    <TD align="center" valign="top"  >
      <font size="2" class="fuente"><a href="frames_back.asp?idmenu=<%=rs("idmenu")%>&posicion=<%=rs("posicion")%>" target="Back"><b><%=trim(rs("menu"))%></b></a></font>

    </TD>
  <%
  rs.movenext
  wend
	sesionesASP = "&sASP=" & session("usxp001") & ";" & trim(session("usxp002")) & _
       ";" & trim(session("usxp003")) & ";" & trim(session("usxp004")) & ";" & session("usxp005") & _
			 ";" & session("usxp006") & ";" & session("usxp007") & ";" & session("usxp008") & _
			 ";" & session("usxp009") & ";" & session("usxp010") & ";" & session("usxp012")
  %>
    <td valign="top">
      <FORM NAME="forma" METHOD="POST" ACTION="ControladorSesiones.aspx?toFormGo=true<%=sesionesASP%>" onSubmit="return validacion()" target="Back">
        <input type="text" name="idmenu" class="textbox" size="4" maxlength="4">
        <input type="submit" value="Ir" class="boton">
      </form>
    </td>
  </TR>
</TABLE>
</center>
</BODY>
</HTML>

<%
conn.close
%>