<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenRecepcionTelefonoSim.aspx.vb" Inherits="BPColSysOP.CrearOrdenRecepcionTelefonoSim" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/ModalProgress.ascx" tagname="ModalProgress" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crear Orden de Recepcion</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function modificarAltoFramePadre() {
            $("body.cuerpo2").ready(function() {
                $("#frModulo", parent.document).height($("body").height() + 130);
            });
        }
        function RefrescaUpdatePaneFactura() {
            var filtro = $get("txtFiltroFactura").value;
            var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\s áéíóúÁÉÍÓÚ]+\s*$");
            if (patron.test(filtro)) {
                if (filtro.length >= 2) {
                    $get("hfFlagFiltradoFactura").value = 1;
                    __doPostBack('txtFiltroFactura', '');
                    $find(ModalProgress).hide();
                }
                else if (filtro.length <= 2 && $get("hfFlagFiltradoFactura").value == 1) {
                $get("hfFlagFiltradoFactura").value = 0
                    __doPostBack('txtFiltroFactura', '');
                    $find(ModalProgress).hide();
                }
            }
            else if ($get("txtFiltroFactura").value != "") { alert("los caracteres especiales no son permitidos") }
        }
        function RefrescaUpdatePaneOrden(txt,hfield) {
            var filtro = $get(txt).value;
            var patron = new RegExp("^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$");
            if (patron.test(filtro)) {
                if (filtro.length >= 2) {
                    $get(hfield).value = 1;
                    __doPostBack(txt, '');
                    $find(ModalProgress).hide();
                }
                else if (filtro.length <= 2 && $get(hfield).value == 1) {
                $get(hfield).value = 0
                    __doPostBack(txt, '');
                    $find(ModalProgress).hide();
                }
            }
            else if ($get(txt).value != "") { alert("los caracteres especiales no son permitidos") }
        }
        function RefrescaUpdatePaneGuia() {
            var filtro = $get("txtFiltroGuia").value;
            var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\s áéíóúÁÉÍÓÚ]+\s*$");
            if (patron.test(filtro)) {
                if (filtro.length >= 2) {
                    $get("hfFlagFiltradoGuia").value = 1;
                    __doPostBack('txtFiltroGuia', '');
                    $find(ModalProgress).hide();
                }
                else if (filtro.length <= 2 && $get("hfFlagFiltradoGuia").value == 1) {
                $get("hfFlagFiltradoGuia").value = 0
                __doPostBack('txtFiltroGuia', '');
                    $find(ModalProgress).hide();
                }
            }
            else if ($get("txtFiltroGuia").value != "") { alert("los caracteres especiales no son permitidos") }
        }
    </script>
    <style type="text/css">
    .exito
    {
    	color:Green;
    	font-size:15px;
    }
    </style>
</head>
<body class="cuerpo2" style="background-image:none;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
    
        <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />
    
    </div>           
    <table class="tablaGris" width="600px">
        <tr>
            <th colspan="2" align="center">
                Datos de la Recepción
            </th>
        </tr>
        <tr>
            <td>
                Tipo de Recepción:
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoRecepcion" runat="server" AutoPostBack="true">
                </asp:DropDownList>
                <div>
                    <asp:RequiredFieldValidator ID="rfvTipoRecepcion" runat="server" ControlToValidate="ddlTipoRecepcion"
                        InitialValue="0" ErrorMessage="Seleccione un tipo de recepción" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr id="trConsignatario" runat="server">
            <td>
                Consignado a:
            </td>
            <td>
                <asp:DropDownList ID="ddlConsignado" runat="server">
                </asp:DropDownList>
                <div>
                    <asp:RequiredFieldValidator ID="rfvConsignado" runat="server" ControlToValidate="ddlConsignado"
                        InitialValue="0" ErrorMessage="Seleccione el consignado a" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                Orden de Compra:
            </td>
            <td>
            <asp:HiddenField ID="hfFlagFiltradoOrden" runat="server" /> 
                <span class="comentario" style="padding-left:0;">Ingrese Número o Identificador</span><br />                     
                        <asp:TextBox ID="txtFiltroOrden" MaxLength="15" runat="server" onkeyup="RefrescaUpdatePaneOrden('txtFiltroOrden','hfFlagFiltradoOrden');"
                            OnTextChanged="FiltrarOrden"></asp:TextBox>-
                <asp:UpdatePanel ID="upFiltroOrdenNumero" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        
                        <asp:DropDownList ID="ddlOrdenCompra" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtFiltroOrden" />
                    </Triggers>
                </asp:UpdatePanel>
                <div>
                    <asp:RequiredFieldValidator ID="rfvOrdenCompra" runat="server" ControlToValidate="ddlOrdenCompra"
                        InitialValue="0" ErrorMessage="Seleccione la orden de compra" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                Factura:
            </td>
            <td>
            <asp:HiddenField ID="hfFlagFiltradoFactura" runat="server" />
                        <asp:TextBox ID="txtFiltroFactura" runat="server" MaxLength="15" onkeyup="RefrescaUpdatePaneFactura();"
                            OnTextChanged="FiltrarFactura"></asp:TextBox>-
                <asp:UpdatePanel ID="upFiltroFactura" runat="server" RenderMode="Inline">
                    <ContentTemplate>                        
                        <asp:DropDownList ID="ddlFactura" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtFiltroFactura" />
                    </Triggers>
                </asp:UpdatePanel>
                <div>
                    <asp:RequiredFieldValidator ID="rfvFactura" runat="server" ControlToValidate="ddlFactura"
                        InitialValue="0" ErrorMessage="Seleccione la factura" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                Guía:
            </td>
            <td>
                        <asp:HiddenField ID="hfFlagFiltradoGuia" runat="server" />
                        <asp:TextBox ID="txtFiltroGuia" runat="server" MaxLength="15" onkeyup="RefrescaUpdatePaneGuia();"></asp:TextBox>-
                <asp:UpdatePanel ID="upFiltroGuia" runat="server" RenderMode="Inline">
                    <ContentTemplate>                        
                        <asp:DropDownList ID="ddlGuia" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtFiltroGuia" />
                    </Triggers>
                </asp:UpdatePanel>
                <div>
                    <asp:RequiredFieldValidator ID="rfvGuia" runat="server" ControlToValidate="ddlGuia"
                        InitialValue="0" ErrorMessage="Seleccione la guia" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                Destinatario:
            </td>
            <td>
                <asp:DropDownList ID="ddlClienteExterno" runat="server">
                </asp:DropDownList>
                <div>
                    <asp:RequiredFieldValidator ID="rfvClienteExterno" runat="server" ControlToValidate="ddlClienteExterno"
                        InitialValue="0" ErrorMessage="Seleccione el destinatario" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                Remisión:
            </td>
            <td>
                <asp:TextBox ID="txtRemision" runat="server" MaxLength="15"></asp:TextBox>
                <asp:RegularExpressionValidator ID="revRemision" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                Display="Dynamic" ControlToValidate="txtRemision" ErrorMessage="El número de remisión contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
            </td>
        </tr>
    </table>
    <div>
            <asp:Button ID="btnBuscar" runat="server" Text="Crear Orden" CssClass="boton" />
        </div>          
            <uc2:ModalProgress ID="mpCrearOrdenRecepcion" runat="server" />             
    </form>
</body>
</html>
