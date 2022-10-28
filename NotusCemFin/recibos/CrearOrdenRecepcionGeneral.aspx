<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenRecepcionGeneral.aspx.vb"
    Inherits="BPColSysOP.CrearOrdenRecepcionGeneral" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register src="../ControlesDeUsuario/ModalProgress.ascx" tagname="ModalProgress" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Creación de Orden de Recepción - General</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function modificarAltoFramePadre() {
            $("body.cuerpo2").ready(function() {
                $("#frModulo", parent.document).height($("body").height() + 130);
            });
        }
        //String.prototype.trim = function() { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, ""); }
        function FiltrarListadoOrdenCompra() {
            var filtro = $get("txtFiltroOrdenCompra").value;
            if (filtro.trim() != "" || $get("hfFlagFiltroOrdenCompra").value == "1") {
                var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\.\s áéíóúñÁÉÍÓÚÑ]+\s*$");
                if (patron.test(filtro) || filtro.trim() == "") {
                    if (filtro.length >= 2) {
                        $get("hfFlagFiltroOrdenCompra").value = 1;
                        $get("ddlOrdenCompra").disabled = true;
                        __doPostBack('txtFiltroOrdenCompra', '');
                        $find(ModalProgress).hide();
                    }
                    else if (filtro.length <= 2 && $get("hfFlagFiltroOrdenCompra").value == 1) {
                        $get("hfFlagFiltroOrdenCompra").value = 0
                        $get("ddlOrdenCompra").disabled = true;
                        __doPostBack('txtFiltroOrdenCompra', '');
                        $find(ModalProgress).hide();
                    }
                }
                else if ($get("txtFiltroOrdenCompra").value != "") { alert("El filtro de búqueda tiene caracteres no permitidos.\nPor favor verifique") }
            }
        }
    </script>

</head>
<body class="cuerpo2" style="background-image:none;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="smAjaxManager" runat="server">
    </asp:ScriptManager>
    <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
    <asp:UpdatePanel ID="upCrearOrdenRecepcion" runat="server">
    <ContentTemplate>
    <table class="tablaGris" border="1">
        <tr>
            <th colspan="2" style="text-align: center">
                INFORMACIÓN GENERAL DE LA ORDEN
            </th>
        </tr>
        <tr>
            <td class="field">
                Tipo de Recepción:
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoRecepcion" runat="server" AutoPostBack="True">
                </asp:DropDownList>
                <div style="display: block;">
                    <asp:RequiredFieldValidator ID="rfvTipoRecepcion" runat="server" ErrorMessage="Seleccione un tipo de recepción, por favor"
                        ControlToValidate="ddlTipoRecepcion" Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                </div>
            </td>
        </tr>
        <tr id="trConsignatario" runat="server">
                 <td class="field">
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
            <td class="field">
                Orden de Compra:
            </td>
            <td>
            <span class="comentario" style="padding-left:0;">Ingrese Número o Identificador</span><br />
                        <asp:TextBox ID="txtFiltroOrdenCompra" runat="server" onkeyup="FiltrarListadoOrdenCompra();"
                            Width="80px" MaxLength="15" OnTextChanged="txtFiltroOrdenCompra_TextChanged" ></asp:TextBox> -                             
                        <asp:DropDownList ID="ddlOrdenCompra" runat="server">
                        </asp:DropDownList>
                        &nbsp;<asp:Label ID="lblNumOrdenesCompra" runat="server" Font-Italic="True" Font-Size="8pt"
                            ForeColor="Gray"></asp:Label><div style="display: block;">
                                <asp:RequiredFieldValidator ID="rfvOrdenCompra" runat="server" ErrorMessage="Seleccione una orden de compra, por favor"
                                    Display="Dynamic" ControlToValidate="ddlOrdenCompra" InitialValue="0"></asp:RequiredFieldValidator>
                            </div>                                                        
            </td>
        </tr>
        <tr>
            <td class="field">
                Remisión:
            </td>
            <td>
                <asp:TextBox ID="txtRemision" runat="server" MaxLength="15"></asp:TextBox>
                <div style="display: block;">
                    <asp:RequiredFieldValidator ID="rfvRemision" runat="server" ErrorMessage="Digite el número de la remisión, por favor"
                        ControlToValidate="txtRemision" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div style="display: block;">
                    <asp:RegularExpressionValidator ID="revRemision" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                            Display="Dynamic" ControlToValidate="txtRemision" ErrorMessage="El número de remisión contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td class="field">Destinatario:</td>                                
            <td>                                
                <asp:DropDownList ID="ddlClienteExterno" runat="server">
                </asp:DropDownList>
                <div>
                <asp:RequiredFieldValidator ID="rfvClienteExterno" runat="server" ControlToValidate="ddlClienteExterno" InitialValue="0"
                ErrorMessage="Seleccione el destinatario" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <asp:HiddenField ID="hfFlagFiltroOrdenCompra" runat="server" Value="0" />    
             </td>
        </tr>        
    </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="txtFiltroOrdenCompra" />
    </Triggers>
    </asp:UpdatePanel>
    <div>
        <asp:Button ID="btnCrear" runat="server" Text="Crear Orden" CssClass="boton" />
    </div>
    <uc2:ModalProgress ID="ModalProgress1" runat="server" />
    </form>
</body>
</html>
