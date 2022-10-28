<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReporteRecepcionProducto.aspx.vb"
    Inherits="BPColSysOP.ReporteRecepcionProducto" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc2" %>
<%@ Register Src="~/ControlesDeUsuario/UcShowmessages.ascx" TagName="Showmessages"
    TagPrefix="UcShowmessages" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporte de Recepción de Producto</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <style type="text/css">
        .vacio
        {
            padding: 30px 0;
            font-size: 15px;
        }
        #pnlExportarExcel
        {
            padding: 30px 10px;
        }
        .fecha
        {
            width: 70px;
        }
        .tablaGris
        {
            width: 100%;
        }
    </style>
    <script type="text/javascript">
        function validarVacios(source, arguments) {
            try {
                var txtIdOrdenRecepcion = $("#txtIdOrdenRecepcion").val();
                var txtIdOrdenCompra = $("#txtIdOrdenCompra").val();
                var txtNoOrdenCompra = $("#txtNoOrdenCompra").val();
                var ddlTipoProducto = $("#ddlTipoProducto").val();
                var ddlProducto = $("#ddlProducto").val();
                var ddlEstado = $("#ddlEstado").val();
                var fechaInicial = $("#txtFechaInicial").val();
                var fechaFinal = $("#txtFechaFinal").val();
                if (txtIdOrdenRecepcion == "" && txtIdOrdenCompra == "" && txtNoOrdenCompra == "" && ddlTipoProducto == "0" && ddlProducto == "0" && ddlEstado == "0" && fechaInicial == "" && fechaFinal == "") {
                    arguments.IsValid = false;
                } else {
                    arguments.IsValid = true;
                }
            } catch (e) {
                arguments.IsValid = false;
            }
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

        function FiltrarDevExpProducto(s, e) {
            try {
                if (s.GetText().length >= 4 || cmbProducto.GetItemCount() != 0) {
                    cpFiltroProducto.PerformCallback(s.GetText());
                } else {
                    cmbProducto.ClearItems();
                }
            }
            catch (e) { }
        }

    </script>
</head>
<body class="cuerpo2">
    <form id="frmPrin" runat="server">
    <asp:HiddenField ID="hfFiltroProdAplicado" runat="server" Value="0" />
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <div>
            <uc1:EncabezadoPagina ID="epPrincipal" runat="server" />
        </div>
        <div id="pnlFiltros" style="width: 600px;">
            <p class="subtitulo" style="width: 100%;">
                Filtros de búsqueda.</p>
            <table class="tablaGris">
                <tr>
                    <td class="field" style="width: 150px;">
                        Id. Orden Recepción:
                    </td>
                    <td>
                        <asp:TextBox ID="txtIdOrdenRecepcion" runat="server" MaxLength="15" ValidationGroup="buscarOrden"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="revOrdenRecepcion" runat="server" ErrorMessage="El campo identificador de orden de recepción es numérico. Digite un número válido, por favor"
                                ControlToValidate="txtIdOrdenRecepcion" Display="Dynamic" ValidationExpression="[0-9]+"
                                ValidationGroup="buscarOrden"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Id. Orden Compra:
                    </td>
                    <td>
                        <asp:TextBox ID="txtIdOrdenCompra" runat="server" MaxLength="8" ValidationGroup="buscarOrden"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="rglIdOrden" runat="server" ErrorMessage="El campo identificador de orden de compra es numérico. Digite un número válido, por favor"
                                ControlToValidate="txtIdOrdenCompra" Display="Dynamic" ValidationExpression="[0-9]+"
                                ValidationGroup="buscarOrden"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        No. Orden Compra:
                    </td>
                    <td>
                        <asp:TextBox ID="txtNoOrdenCompra" runat="server" MaxLength="20" ValidationGroup="buscarOrden"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="revNumeroOrden" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                Display="Dynamic" ControlToValidate="txtNoOrdenCompra" ErrorMessage="El número de orden de compra contiene caracteres no validos, por favor verifique."
                                ValidationGroup="buscarOrden"></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Tipo de Producto:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTipoProducto" runat="server" ValidationGroup="buscarOrden">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Producto:
                    </td>
                    <td>
                        <div style="display: inline; float: left;">
                            <dx:ASPxTextBox ID="txtProductoFiltro" runat="server" Width="100px" Height="25px"
                                MaxLength="15" TabIndex="2">
                                <ClientSideEvents KeyUp="function(s, e) { 
                                    FiltrarDevExpProducto(s, e) 
                                }"></ClientSideEvents>
                            </dx:ASPxTextBox>
                        </div>
                        <dx:ASPxCallbackPanel ID="cpFiltroProducto" runat="server" RenderMode="Div" ClientInstanceName="cpFiltroProducto">
                            <ClientSideEvents EndCallback="function(s, e) {}"></ClientSideEvents>
                            <PanelCollection>
                                <dx:PanelContent>
                                    <div style="display: inline; float: left">
                                        <dx:ASPxComboBox ID="cmbProducto" runat="server" Width="200px" IncrementalFilteringMode="Contains"
                                            ClientInstanceName="cmbProducto" DropDownStyle="DropDownList" TabIndex="3">
                                        </dx:ASPxComboBox>
                                    </div>
                                    <div id="divResultadoproducto" style="width: 250px">
                                        <dx:ASPxLabel ID="lblResultadoProducto" runat="server" CssClass="comentario" Width="100%"
                                            Font-Size="XX-Small" Font-Italic="True">
                                        </dx:ASPxLabel>
                                    </div>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxCallbackPanel>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Estado:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEstado" runat="server" ValidationGroup="buscarOrden">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Fecha:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFechaInicial" CssClass="fecha" runat="server" MaxLength="10"
                            ValidationGroup="buscarOrden"></asp:TextBox>
                        <cc1:CalendarExtender ID="txtFechaInicial_CalendarExtender" runat="server" PopupButtonID="imgFechaIni"
                            Format="dd/MM/yyyy" CssClass="calendarTheme" TargetControlID="txtFechaInicial">
                        </cc1:CalendarExtender>
                        <img src="../images/date-32.png" id="imgFechaIni" alt="Fecha Inicial" title="Fecha Inicial" />&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="txtFechaFinal" CssClass="fecha" runat="server" MaxLength="10" ValidationGroup="buscarOrden"></asp:TextBox>
                        <cc1:CalendarExtender ID="txtFechaFinal_CalendarExtender" runat="server" PopupButtonID="imgFechaFinal"
                            Format="dd/MM/yyyy" CssClass="calendarTheme" TargetControlID="txtFechaFinal">
                        </cc1:CalendarExtender>
                        <img src="../images/date-32.png" id="imgFechaFinal" alt="Fecha Final" title="Fecha Final" />
                        <div>
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaInicial" runat="server"
                                ValidationGroup="buscarOrden" ErrorMessage="Fecha inicial no válida." ControlToValidate="txtFechaInicial"
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaFinal" runat="server"
                                ValidationGroup="buscarOrden" ErrorMessage="Fecha final no válida." ControlToValidate="txtFechaFinal"
                                ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="cvRangoFecha" runat="server" ControlToCompare="txtFechaInicial"
                                ValidationGroup="buscarOrden" ControlToValidate="txtFechaFinal" ErrorMessage="La Fecha Final debe ser mayor o igual a la Fecha Inicial"
                                Operator="GreaterThanEqual" Type="Date" Display="Dynamic"></asp:CompareValidator>
                            <asp:CustomValidator ID="cusRango" runat="server" ErrorMessage="Es necesario especificar los dos valores del Rango"
                                ValidationGroup="buscarOrden" Display="Dynamic" ClientValidationFunction="esRangoValido"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnBuscar" CssClass="search" runat="server" Text="Buscar" ValidationGroup="buscarOrden" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnBorrarFiltros" CssClass="search" runat="server" Text="Borrar Filtros" />
                        <asp:CustomValidator ID="cvValidarVacios" runat="server" ErrorMessage="Seleccione un filtro de búsqueda"
                            ClientValidationFunction="validarVacios" ValidationGroup="buscarOrden"></asp:CustomValidator>
                    </td>
                </tr>
            </table>
        </div>
        <div id="pnlExportarExcel">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkGenerarExcel" runat="server" Style="cursor: pointer;"><img src="../images/Excel.gif" alt="Descargar Excel" title="Descargar Excel" border="0" /> Descargar Excel</asp:LinkButton>
                    </td>
                    <td>
                        &nbsp&nbsp&nbsp
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkEnviarInformacion" runat="server" Style="cursor: pointer;"><img src="../images/delivery_ok.png" alt="Enviar Información" title="Enviar información al cliente" border="0" /> Enviar información al cliente</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
        <div id="pnlResultados">
            <asp:GridView ID="gvDatos" runat="server" CssClass="grid" AutoGenerateColumns="False"
                Style="width: 90%; min-width: 800px;" EmptyDataText="&lt;div class=&quot;vacio&quot;&gt;No se encontraron resultados con el filtro indicado.&lt;/div&gt;"
                EnableModelValidation="True">
                <Columns>
                    <asp:BoundField DataField="ordenCompra" HeaderText="Orden de Compra" />
                    <asp:BoundField DataField="remision" HeaderText="Remisión" />
                    <asp:BoundField DataField="material" HeaderText="Material" />
                    <asp:BoundField DataField="referencia" HeaderText="Referencia" />
                    <asp:BoundField DataField="numeroPiezas" HeaderText="Número de Piezas">
                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="cantidadEstimada" HeaderText="Cantidad Estimada">
                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="guia" HeaderText="Guía" />
                    <asp:BoundField DataField="factura" HeaderText="Factura" />
                    <asp:BoundField DataField="fechaLlegada" HeaderText="Fecha de Llegada">
                        <ItemStyle Width="90px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="observacion" HeaderText="Observación" />
                    <asp:TemplateField HeaderText="Notificada al cliente" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgNotificacion" runat="server" ImageUrl="~/images/BallGreen.png"
                                Enabled="false" ImageAlign="Middle" />
                        </ItemTemplate>
                        <HeaderStyle Width="50px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ver Imagenes" HeaderStyle-HorizontalAlign="Center"
                        HeaderStyle-VerticalAlign="Middle">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgVerImagenes" runat="server" ToolTip="Ver Imagenes" CommandArgument='<%#Bind("idOrdenRecepcion")%>'
                                CommandName="VerImagen" ImageUrl="~/images/DxSearch16.png" />
                        </ItemTemplate>
                        <HeaderStyle Width="50px" />
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
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
