<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BuscarOrdenRecepcion.aspx.vb"
    Inherits="BPColSysOP.BuscarOrdenRecepcion" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Busqueda Orden de Recepcion</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        $(document).ready(init);
        function init() {
            $("#txtFechaInicial,#txtFechaFinal").css({ 'width': '80px' });
        }
        function validarVacios(source, arguments) {
            try {
                var idOrdenRecepcion = $("#txtIdOrdenRecepcion").val();
                var idOrden = $("#txtIdOrden").val();
                var numeroOrden = $("#txtNoOrden").val();
                var idTipoProducto = $("#ddlTipoProducto").val();
                var idTipoRecepcion = $("#ddlTipoRecepcion").val();
                var idEstado = $("#ddlEstado").val();
                var fechaInicial = $("#txtFechaInicial").val();
                var fechaFinal = $("#txtFechaFinal").val();
                if (idOrdenRecepcion == "" && idOrden == "" && numeroOrden == "" && idTipoProducto == "0" && idTipoRecepcion == "0" && idEstado == "0" && fechaInicial == "" && fechaFinal == "") {
                    arguments.IsValid = false;
                } else {
                    arguments.IsValid = true;
                }
            } catch (e) {
                arguments.IsValid = false;
            }
        }
        function showConfirmation() {
            return (confirm("Está seguro de que desea cancelar la orden indicada?"));
        }
        function esRangoValido(source, arguments) {
            try {
                if (document.getElementById("txtFechaInicial").value.trim() != "" || document.getElementById("txtFechaFinal").value.trim() != "") {
                    if (document.getElementById("txtFechaInicial").value.trim() != "" && document.getElementById("txtFechaFinal").value.trim() == "") {
                        arguments.IsValid = false;
                    } else {
                        if (document.getElementById("txtFechaInicial").value.trim() == "" && document.getElementById("txtFechaFinal").value.trim() != "") {
                            arguments.IsValid = false;
                        } else {
                            arguments.IsValid = true;
                        }
                    }
                } else {
                    arguments.IsValid = true;
                }
            } catch (e) {
                arguments.IsValid = false;
            }
        }   
    </script>
    <style type="text/css">
        .exito
        {
            color: Green;
            font-size: 15px;
        }
    </style>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <div>
        <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />
    </div>
    <table class="tablaGris" style="width: 600px;">
        <tr>
            <th colspan="2" align="center">
                Datos de Busqueda
            </th>
        </tr>
        <tr>
            <td style="width: 40%;">
                Identificador de la Orden de Recepción:
            </td>
            <td>
                <asp:TextBox ID="txtIdOrdenRecepcion" runat="server" MaxLength="15"></asp:TextBox>
                <div>
                    <asp:RegularExpressionValidator ID="revIdOrdenRecepcion" runat="server" ErrorMessage="El campo identificador es numérico. Digite un número válido, por favor"
                        ControlToValidate="txtIdOrdenRecepcion" Display="Dynamic" ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 40%;">
                Identificador de la Orden de Compra:
            </td>
            <td>
                <asp:TextBox ID="txtIdOrden" runat="server" MaxLength="15"></asp:TextBox>
                <div>
                    <asp:RegularExpressionValidator ID="rglIdOrden" runat="server" ErrorMessage="El campo identificador es numérico. Digite un número válido, por favor"
                        ControlToValidate="txtIdOrden" Display="Dynamic" ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                No. Orden de Compra:
            </td>
            <td>
                <asp:TextBox ID="txtNoOrden" runat="server" MaxLength="15"></asp:TextBox>
                <div>
                    <asp:RegularExpressionValidator ID="revNoOrden" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                        Display="Dynamic" ControlToValidate="txtNoOrden" ErrorMessage="El no. orden contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td style="width: 140px;">
                Tipo de Producto:
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoProducto" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Tipo de Recepción:
            </td>
            <td>
                <asp:DropDownList ID="ddlTipoRecepcion" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Estado:
            </td>
            <td>
                <asp:DropDownList ID="ddlEstado" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                Fecha:
            </td>
            <td>
                <asp:TextBox ID="txtFechaInicial" runat="server" MaxLength="10"></asp:TextBox>
                <cc1:CalendarExtender ID="txtFechaInicial_CalendarExtender" runat="server" PopupButtonID="imgFechaIni"
                    Format="dd/MM/yyyy" CssClass="calendarTheme" TargetControlID="txtFechaInicial">
                </cc1:CalendarExtender>
                <img src="../images/date-32.png" id="imgFechaIni" alt="Fecha Inicial" title="Fecha Inicial" />&nbsp;&nbsp;&nbsp;
                <asp:TextBox ID="txtFechaFinal" runat="server" MaxLength="10"></asp:TextBox>
                <cc1:CalendarExtender ID="txtFechaFinal_CalendarExtender" runat="server" PopupButtonID="imgFechaFinal"
                    Format="dd/MM/yyyy" CssClass="calendarTheme" TargetControlID="txtFechaFinal">
                </cc1:CalendarExtender>
                <img src="../images/date-32.png" id="imgFechaFinal" alt="Fecha Final" title="Fecha Final" />
                <div>
                    <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaInicial" runat="server"
                        ErrorMessage="Fecha inicial no válida." ControlToValidate="txtFechaInicial" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaFinal" runat="server"
                        ErrorMessage="Fecha final no válida." ControlToValidate="txtFechaFinal" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                    <asp:CompareValidator ID="cvRangoFecha" runat="server" ControlToCompare="txtFechaInicial"
                        ControlToValidate="txtFechaFinal" ErrorMessage="La Fecha Final debe ser mayor o igual a la Fecha Inicial"
                        Operator="GreaterThanEqual" Type="Date" Display="Dynamic"></asp:CompareValidator>
                    <asp:CustomValidator ID="cusRango" runat="server" ErrorMessage="Es necesario especificar los dos valores del Rango"
                        Display="Dynamic" ClientValidationFunction="esRangoValido"></asp:CustomValidator>
                </div>
            </td>
        </tr>
    </table>
    <div>
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="boton" />
        <asp:Button ID="btnBorrarFiltros" runat="server" Text="Limpiar Filtros" CssClass="boton" />
        <asp:CustomValidator ID="cvValidarVacios" runat="server" ErrorMessage="Seleccione un filtro de búsqueda"
            ClientValidationFunction="validarVacios"></asp:CustomValidator>
    </div>
    <div>
        <asp:GridView ID="gvDatosRecepcion" runat="server" CssClass="tablaGris" AutoGenerateColumns="False"
            EmptyDataText="No se encontraron órdenes que cumplan con los criterios indicados.">
            <Columns>
                <asp:BoundField DataField="idOrdenRecepcion" HeaderText="Id Orden Recepción" />
                <asp:BoundField DataField="numeroOrden" HeaderText="No. de Orden de Compra" />
                <asp:BoundField DataField="tipoProducto" HeaderText="Tipo de Producto" />
                <asp:BoundField DataField="remision" HeaderText="Remision" />
                <asp:BoundField DataField="fechaRecepcion" HeaderText="Fecha de Recepción" />
                <asp:BoundField DataField="tipoRecepcion" HeaderText="Tipo de Recepcion" />
                <asp:BoundField DataField="idTipoProducto" HeaderText="Tipo de Producto" Visible="False" />
                <asp:BoundField DataField="estado" HeaderText="Estado" />
                <asp:TemplateField HeaderText="Opciones">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEditarOrdenRecepcion" runat="server" ImageUrl="~/images/Edit-32.png"
                            ToolTip="Editar" Visible="false" />
                        <asp:ImageButton ID="imgEliminarOrdenRecepcion" runat="server" ImageUrl="~/images/Delete-32.png"
                            ToolTip="Eliminar" Visible="false" />
                        <asp:ImageButton ID="imgAgregarDetalleOrdenRecepcion" runat="server" ImageUrl="~/images/Folder-add-32.png"
                            ToolTip="Agregar Detalle" />
                        <asp:ImageButton ID="ImbBtnVerDetalleOrden" runat="server" ImageUrl="~/images/view.png"
                            ToolTip="Ver" />
                        <asp:ImageButton ID="imgBtnCargarSAP" runat="server" ImageUrl="~/images/package.png"
                            ToolTip="Cargar SAP" />
                        <asp:ImageButton ID="imgBtnExportarLotes" runat="server" ImageUrl="~/images/Excel.gif"
                            ToolTip="Exportar Lotes" CommandName="ExportarSeriales" CommandArgument='<%#Bind("idOrdenRecepcion")%>' />
                        <asp:Image ID="imgInfo" runat="server" ImageUrl="~/images/Info-32.png" ToolTip="Las opciones de recepción para este tipo de producto estan disponibles en el aplicativo de escritorio." />
                        <asp:ImageButton ID="ImgConsecutivos" runat="server" ToolTip="Ver Consecutivos" CommandArgument='<%#Bind("idOrdenRecepcion")%>'
                            CommandName="Detalle" ImageUrl="~/images/DxSearch16.png" />
                        <asp:ImageButton ID="imgVerImagenes" runat="server" ToolTip="Ver Imagenes" CommandArgument='<%#Bind("idOrdenRecepcion")%>'
                            CommandName="VerImagen" ImageUrl="~/images/VerImagen.jpg" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div>
        <asp:HiddenField ID="hfpnlDetalle" runat="server" />
        <cc1:ModalPopupExtender ID="mdDetalle" runat="server" Enabled="true" BackgroundCssClass="modalBackground"
            PopupControlID="pnlVerDetalle" TargetControlID="hfpnlDetalle" CancelControlID="lnkSalir">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="pnlVerDetalle" runat="server" CssClass="modalPopUp" Style="display: none;">
            <asp:HiddenField ID="hfIdOrden" runat="server" />
            <table class="tablaGris">
                <tr>
                    <th>
                        Detalle de Consecutivos
                    </th>
                </tr>
                <tr>
                    <th>
                        <asp:GridView ID="gvDetalle" runat="server" CssClass="tablaGris" AutoGenerateColumns="False"
                            EmptyDataText="No se encontraron datos en la orden consultada.">
                            <Columns>
                                <asp:BoundField DataField="consInicial" HeaderText="Consecutivo Inicial" />
                                <asp:BoundField DataField="consFinal" HeaderText="Consecutivo Final" />
                                <asp:BoundField DataField="total" HeaderText="Total" />
                            </Columns>
                        </asp:GridView>
                    </th>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:LinkButton ID="lnkSalir" runat="server" CssClass="search"><img src="../images/error.png" alt="" /> Cerrar</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <br />
    <asp:HiddenField ID="hfUpdateManager" runat="server" />
    <cc1:ModalPopupExtender ID="mpeVisualizacionImagen" runat="server" 
        Enabled="True" TargetControlID="hfUpdateManager" BackgroundCssClass="modalBackground"
        PopupControlID="pnlVerImagen">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlVerImagen" runat="server">
        <dx:ASPxImageSlider ID="isImagenes" runat="server">
        </dx:ASPxImageSlider>
        <asp:Button ID="btnCerrarVisualizar" runat="server" Text="Cerrar" CssClass="submit" />
    </asp:Panel>
    </form>
</body>
</html>
