<!--#include file="include/seguridad.inc.asp" -->
<!--#include file="include/conexion.inc.asp" -->
<HTML>
<HEAD>
<LINK REL="StyleSheet" HREF="include/styleBACK.css" TYPE="text/css">
</HEAD>
<body class="cuerpo2">
<%
idmenu = request.form("idmenu")
if idmenu = "" then idmenu = request.querystring("idmenu")

sqlMenu = "select idmenu, menu, url from menus where idmenu = " & idmenu
sqlMenu = sqlMenu & " and idmenu in (select idmenu from menus where idmenupadre in (select idmenu from menus_perfiles where idperfil = " & session("usxp009") & " )) "
'response.write sqlMenu
'response.end
set rsMenu = conn.execute(sqlMenu)
if not rsMenu.eof then
   go = trim(rsMenu("url"))
   response.redirect go
else
  %>
  <ul>
    <font color="990000" >
      No esta autorizado para ver este Menu
    </font><br><br>
    <%
    sqlMenus = "select idmenu, menu, url from menus where "
    sqlMenus = sqlMenus & " idmenu in (select idmenu from menus where idmenupadre in (select idmenu from menus_perfiles where idperfil = " & session("usxp009") & " )) "
    sqlMenus = sqlMenus & " and url is not null"
    set rsMenus = conn.execute(sqlMenus)
    %>
    <table class="tabla">
      <tr bgcolor="d0d0d0">
        <td ><b>Menus Permitidos para su perfil</b></td>
      </tr>
      <%
      bgcolor=""
      while not rsMenus.eof
        %>
        <tr bgcolor="<%=bgcolor%>">
          <td>
            <font size="1" color="990000"><%=trim(rsMenus("idmenu"))%></font>
            <a href="<%=trim(rsMenus("url"))%>"><b><%=trim(rsMenus("menu"))%></b></a></td>
          </td>
        </tr>
        <%
        if bgcolor="" then
          bgcolor="f0f0f0"
        else
          bgcolor=""
        end if
        rsMenus.movenext
      wend
      %>
    </table>

  </ul>
  <%
end if

%>
</body>
</html>