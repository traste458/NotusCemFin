<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenCompraDetallePopInsumo.aspx.vb" Inherits="BPColSysOP.CrearOrdenCompraDetallePopInsumo" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crear Orden Compra</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    function modificarAltoFramePadre() {
        $("body.cuerpo2").ready(function() {
            $("#frModulo", parent.document).height($("body").height() + 80);
        });

    }
    </script>
    <style type="text/css">
    body.cuerpo2
    {
    	background-image:none;
    }
    </style>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <div>    
        <uc1:EncabezadoPagina ID="epEncabezado" runat="server" />    
    </div>
    <div>
    <table style="width:400px;margin-left:15px;">
        <tr>
            <td align="right">Tipo de Orden:</td>
            <td>
                <asp:DropDownList ID="ddlTipoOrden" runat="server">
                    <asp:ListItem Value="0">Seleccione</asp:ListItem>
                    <asp:ListItem Value="1">Orden Nueva</asp:ListItem>
                    <asp:ListItem Value="2">Producto Recibido</asp:ListItem>
                </asp:DropDownList>
                <div>
                    <asp:RequiredFieldValidator ID="rfvTipoOrden" Display="Dynamic" runat="server" ControlToValidate="ddlTipoOrden" InitialValue="0" ErrorMessage="Seleccione el tipo de orden por favor."></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Button ID="btnEnviar" runat="server" Text="Enviar" CssClass="boton" />
                <asp:HiddenField ID="hfIdTipoProducto" runat="server" />
            </td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
