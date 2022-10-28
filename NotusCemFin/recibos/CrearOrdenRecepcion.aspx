<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenRecepcion.aspx.vb" Inherits="BPColSysOP.CrearOrdenRecepcion" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crear Orden de Recepcion</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <style type="text/css">
    .tablaGris
    {
    	width:600px;
    	padding:0 10px;
    	margin:0;
    }
    #contenido
    {
    	padding:12px 0 0 8px;
    }
    </style>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
    
        <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />
    
    </div>
    <div id="contenido">
     <table class="tablaGris">
            <tr>
                <th colspan="2" align="center">Datos de la Recepción</th>
            </tr>
            <tr>
                <td style="width:140px;">Tipo de Producto:</td>
                <td>
                    <asp:DropDownList ID="ddlTipoProducto" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <div>
            <%--<iframe runat="server" id="frModulo" src="" width="80%" frameborder="0" scrolling="no"></iframe>    --%>        
        </div>
        </div>
    </form>
</body>
</html>
