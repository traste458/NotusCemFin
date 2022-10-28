<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearFactura.aspx.vb" Inherits="BPColSysOP.CrearFactura" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crear Factura</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <div>
    
        <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />
    
    </div>
        <table class="tablaGris">
            <tr>
                <th colspan="2" align="center">Informacion Factura</th>
            </tr>
            <tr>
                <td style="width:40%;">Factura:</td>
                <td>
                    <asp:TextBox ID="txtFactura" runat="server"></asp:TextBox>                    
                </td>
            </tr>
            <tr>
                <td style="width:40%;">Cantidad:</td>
                <td>
                    <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>                    
                </td>
            </tr>
            <tr>
                <td>Ciudad de Compra:</td>
                <td>
                    <asp:DropDownList ID="ddlCiudadCompra" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Estado:</td>
                <td>
                    <asp:DropDownList ID="ddlEstado" runat="server">
                    </asp:DropDownList>                        
                </td>
            </tr>                        
        </table>
        <div>
            <asp:Button ID="btnCrear" runat="server" Text="Crear Factura" CssClass="boton" />&nbsp;&nbsp;
            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="boton" />
        </div>
    </form>
</body>
</html>
