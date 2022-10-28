<!--#include file="include/seguridad.inc.asp"-->
<!--#include file="include/conexion.inc.asp"-->
<!--#include file="include/md5.asp"-->

<%
c1=UCase(md5(trim(request.form("c1"))))
c2=UCase(md5(trim(request.form("c2"))))
c3=UCase(md5(trim(request.form("c3"))))

sql = "select clave from terceros where idtercero = "&session("usxp001")
 set rs=conn.execute(sql)


clave = UCase(rs("clave"))
 if trim(clave) <> trim(c1) then
  rs.close
  conn.close
  set rs =nothing
  set conn = nothing
  response.redirect("cambio_clave.asp?estado=1")
end if
 
if trim(c2) <> trim(c3) then
  rs.close
  conn.close
  set rs =nothing
  set conn = nothing
  response.redirect("cambio_clave.asp?estado=2")
end if

if trim(c1) = trim(c2) then
  rs.close
  conn.close
  set rs =nothing
  set conn = nothing
  response.redirect("cambio_clave.asp?estado=4")
end if



sql = "update terceros set clave = '"&trim(c2)&"' where idtercero = "& session("usxp001")
 set rs=conn.execute(sql)

conn.close
set conn = nothing
response.redirect("cambio_clave.asp?estado=3")
%>