<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearGuia.aspx.vb" Inherits="BPColSysOP.CrearGuia" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crear Guia</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>    
        <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />    
    </div>
        <table class="tablaGris" width="500">
            <tr>
                <th colspan="2" align="center">Informacion de la Guia</th>
            </tr>
            <tr>
                <td style="width:140px;">Orden de Compra:</td>
                <td>
                    <asp:DropDownList ID="ddlOrdenCompra" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>No Guia:</td>
                <td>
                    <asp:TextBox ID="txtNoGuia" runat="server"></asp:TextBox>                    
                </td>
            </tr>
            <tr>
                <td>Transportadora:</td>
                <td>
                    <asp:DropDownList ID="ddlTransportadora" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvTransportadora" runat="server"  ControlToValidate="ddlTransportadora"
                        ErrorMessage="Escoja la transportadora"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Ciudad de Origen:</td>
                <td>
                    <asp:DropDownList ID="ddlCiudadOrigen" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Fecha de Salida:</td>
                <td>
                    <asp:TextBox ID="txtFechaSalida" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFechaSalida_CalendarExtender" Format="yyyy-MM-dd" runat="server" 
                        TargetControlID="txtFechaSalida">
                    </cc1:CalendarExtender>
                    <asp:RequiredFieldValidator ID="rfvFechaSalida" runat="server"  ControlToValidate="txtFechaSalida"
                        ErrorMessage="Indique la Fecha de Salida"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Fecha Esperada de Arribo:</td>
                <td>
                    <asp:TextBox ID="txtFechaEsperadaArribo" runat="server"></asp:TextBox>   
                    <cc1:CalendarExtender ID="txtFechaEsperadaArribo_CalendarExtender"  Format="yyyy-MM-dd"
                        runat="server" TargetControlID="txtFechaEsperadaArribo">
                    </cc1:CalendarExtender>
                    <asp:RequiredFieldValidator ID="rfvFechaEsperadaArribo" runat="server"  ControlToValidate="txtFechaEsperadaArribo"
                        ErrorMessage="Indique la Fecha Esperada de Arribo"></asp:RequiredFieldValidator>                 
                </td>
            </tr>
            <tr>
                <td>Estado:</td>
                <td>
                    <asp:DropDownList ID="ddlEstado" runat="server">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvEstado" runat="server"  ControlToValidate="ddlEstado"
                        ErrorMessage="Escoja el estado"></asp:RequiredFieldValidator>                 
                </td>
            </tr>
            <tr>
                <td>Peso Neto:</td>
                <td>
                    <asp:TextBox ID="txtPesoNeto" runat="server"></asp:TextBox>                    
                </td>
            </tr>
            <tr>
                <td>Peso Bruto:</td>
                <td>
                    <asp:TextBox ID="txtPesoBruto" runat="server"></asp:TextBox>                    
                </td>
            </tr>
        </table>
        <div>
            <asp:Button ID="btnCrear" runat="server" Text="Crear Guia" CssClass="boton" />&nbsp;&nbsp;
            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="boton" />
        </div>
    </form>
</body>
</html>
