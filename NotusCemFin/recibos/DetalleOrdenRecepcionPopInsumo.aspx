<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetalleOrdenRecepcionPopInsumo.aspx.vb"
    Inherits="BPColSysOP.DetalleOrdenRecepcionPopInsumo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ModalProgress.ascx" TagName="ModalProgress"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detalle de Orden de Recepción</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function stiker(nombre, material, fecha, cantidad) {
            var controlActive = document.getElementById('myControl1');
            controlActive.generar(nombre, material, fecha, cantidad);
        }
        function doScript() {
            form1.myControl1.generar("Steven", "Material", "08 09 1984", "10");
        }
        function prueba() {
            alert("saludo");
        }
    
        String.prototype.trim = function() { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, ""); }
        function FiltrarProducto() {
            var filtro = $get("txtFiltroProducto").value;
            if (filtro.trim() != "" || $get("hfFlagFiltroProducto").value == "1") {                
                var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\.\s áéíóúñÁÉÍÓÚÑ]+\s*$");
                if (patron.test(filtro) || filtro.trim() == "") {
                    
                    if (filtro.length > 2) {
                        $get("hfFlagFiltroProducto").value = 1;
                        //$get("ddlProducto").disabled = true;
                        __doPostBack('txtFiltroProducto', '');
                        //$find(ModalProgress).hide();                        
                    }
                    else if (filtro.length < 3 && $get("hfFlagFiltroProducto").value == 1) {
                        $get("hfFlagFiltroProducto").value = 0
                        //$get("ddlProducto").disabled = true;
                        __doPostBack('txtFiltroProducto', '');
                        //$find(ModalProgress).hide();
                    }
                    $get("txtFiltroProducto").focus();
                }
                else if ($get("txtFiltroProducto").value != "") { alert("El filtro de búqueda tiene caracteres no permitidos.\nPor favor verifique") }
            }
        }
    </script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">    
    <asp:ScriptManager ID="smAjaxManager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upGeneral" runat="server">
        <ContentTemplate>
            <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
            <asp:Panel ID="pnlGeneral" runat="server">
                <table class="tablaGris" width="800px">
                    <tr>
                        <th colspan="4" style="text-align: center">
                            INFORMACIÓN GENERAL DE LA ORDEN
                        </th>
                    </tr>
                    <tr>
                        <td class="field" style="width: 150px">
                            Orden de Recepción:
                        </td>
                        <td style="width: 250px">
                            <asp:Label ID="lblOrdenRecepcion" runat="server"></asp:Label>
                        </td>
                        <td class="field" style="width: 150px">
                            Fecha de Recepción:
                        </td>
                        <td>
                            <asp:Label ID="lblFechaRecepcion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Proveedor:
                        </td>
                        <td>
                            <asp:Label ID="lblProveedor" runat="server"></asp:Label>
                        </td>
                        <td class="field">
                            Remisión:
                        </td>
                        <td>
                            <asp:Label ID="lblRemision" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="field">
                            Tipo de Producto:
                        </td>
                        <td>
                            <asp:Label ID="lblTipoProducto" runat="server"></asp:Label>
                        </td>
                        <td class="field">
                            Tipo de Recepción:
                        </td>
                        <td>
                            <asp:Label ID="lblTipoRecepcion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr  id="fOrdenCompra" runat="server">
                        <td class="field">
                            Orden Compra:
                        </td>
                        <td>
                            <asp:Label ID="lblOrdenCompra" runat="server"></asp:Label>
                        </td>                        
                        <td class="field"></td>
                        <td>
                            <asp:HiddenField ID="hfIdEstadoOrdenRecepcion" runat="server" />
                            <asp:HiddenField ID="hfIdTipoProducto" runat="server" />
                            <asp:HiddenField ID="hfIdOrdenCompra" runat="server" />                            
                        </td>
                    </tr>                    
                    <tr>
                        <td colspan="4">
                            <br />
                            <br />
                            <asp:LinkButton ID="lbCerrarOrden" runat="server" CssClass="search" OnClientClick="return confirm('¿Realmente desea cerrar la Orden de Recepción?')"><img src="../images/save_all.png" alt=""/>&nbsp;Cerrar Orden</asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <uc1:EncabezadoPagina ID="epAuxNotificacion" runat="server" />
                <br />
                <table>
                    <tr>
                        <td valign="top">
                            <asp:Panel ID="pnlCreacionPallet" runat="server">
                                <table class="tablaGris">
                                    <tr>
                                        <th colspan="2">
                                            FORMULARIO DE CREACIÓN DE PALLETs
                                        </th>
                                    </tr>
                                    <tr>
                                        <td class="field">
                                            Peso (Kg):
                                        </td>
                                        <td>
                                            <div style="display: inline">
                                                <asp:TextBox ID="txtPeso" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblFormato"
                                                    runat="server" Text="Formato: ###.##" Font-Italic="True" Font-Size="8pt" ForeColor="Gray"></asp:Label></div>
                                            <div style="display: block">
                                                <asp:RequiredFieldValidator ID="rfvPeso" runat="server" ErrorMessage="Digite el peso del pallet, por favor"
                                                    Display="Dynamic" ControlToValidate="txtPeso" ValidationGroup="crearPallet"></asp:RequiredFieldValidator>
                                            </div>
                                            <div style="display: block">
                                                <asp:RegularExpressionValidator ID="revPeso" runat="server" ErrorMessage="Peso no válido. Se espera un valor decimal, por favor verifique"
                                                    ControlToValidate="txtPeso" Display="Dynamic" ValidationExpression="(\d+)(\.\d{1,2})?"
                                                    ValidationGroup="crearPallet"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">
                                            Observación:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtObservacion" runat="server" Columns="40" Rows="3" TextMode="MultiLine"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">
                                            Novedades:
                                        </td>
                                        <td>
                                            <asp:CheckBoxList ID="cblNovedad" runat="server" CssClass="tablaGris" RepeatColumns="5"
                                                RepeatDirection="Horizontal">
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">
                                            Detalle:
                                        </td>
                                        <td>
                                            <table>
                                                <tr>
                                                    <th colspan="4">
                                                        INFORMACIÓN DE CAJA
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td class="field">
                                                        Producto:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:HiddenField ID="hfFlagFiltroProducto" runat="server" />
                                                        <asp:TextBox ID="txtFiltroProducto" runat="server" Width="80px" onkeyup="FiltrarProducto();"
                                                        MaxLength="15" ></asp:TextBox>
                                                        -
                                                        <asp:UpdatePanel ID="upProducto" runat="server" RenderMode="Inline">
                                                            <ContentTemplate>
                                                                <asp:DropDownList ID="ddlProducto" runat="server" >
                                                                </asp:DropDownList>                                                                
                                                                    <div style="display: block">
                                                                        <asp:RequiredFieldValidator ID="rfvProducto" runat="server" ErrorMessage="Escoja un Producto, por favor"
                                                                            ControlToValidate="ddlProducto" Display="Dynamic" InitialValue="0" ValidationGroup="crearCaja"></asp:RequiredFieldValidator>
                                                                    </div>                                                                
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="txtFiltroProducto" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>                                                    
                                                    <td class="field">
                                                        Cantidad:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
                                                        <div style="display: block">
                                                            <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" ErrorMessage="Digite la cantidad recibida, por favor"
                                                                ControlToValidate="txtCantidad" Display="Dynamic" ValidationGroup="crearCaja"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div style="display: block">
                                                            <asp:RegularExpressionValidator ID="revCantidad" runat="server" ErrorMessage="El campo es numérico.<br/>Digite un número válido, por favor"
                                                                ControlToValidate="txtCantidad" Display="Dynamic" ValidationExpression="(\s+)?(\d+)(\s+)?"
                                                                ValidationGroup="crearCaja"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div>
                                                <br />
                                                
                                                <asp:LinkButton ID="lbAdicionarCaja" runat="server" ValidationGroup="crearCaja" CssClass="search"><img style="border:none;" src="../images/Folder-add-32.png" alt="" />&nbsp;Adicionar Empaque</asp:LinkButton>
                                                <br />
                                                <br />
                                                <asp:GridView ID="gvCajas" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                    
                                                    EmptyDataText="&lt;blockquote&gt;&lt;i&gt;No existen cajas temporalmente registradas&lt;/i&gt;&lt;/blockquote&gt;">
                                                    <Columns>
                                                        <asp:BoundField DataField="numCaja" HeaderText="No." 
                                                            ItemStyle-HorizontalAlign="Center" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="idCaja" HeaderText="ID Empaque" 
                                                            ItemStyle-HorizontalAlign="Center" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="producto" HeaderText="Producto" />
                                                        <asp:BoundField DataField="cantidad" HeaderText="Cantidad" 
                                                            ItemStyle-HorizontalAlign="Center" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Opc.">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibEliminarCaja" runat="server" ImageUrl="~/images/remove.png"
                                                                    CommandName="Anular" ToolTip="Remover Caja" CommandArgument='<%#Bind("idCaja") %>'
                                                                    OnClientClick="return confirm('¿Realmente desea remover la caja seleccionada?');" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle CssClass="thGris" />
                                                </asp:GridView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                            <br />
                                            <asp:LinkButton ID="lbCrearPallet" runat="server" CssClass="search" ValidationGroup="crearPallet"><img 
                                            src="../images/package.png" alt=""/>&nbsp;Crear Pallet</asp:LinkButton>
                                            <cc1:ConfirmButtonExtender runat="server" ID="cbeCrearPallet" ConfirmText="Desea crear este pallet con los producto asignados ?" TargetControlID="lbCrearPallet" ></cc1:ConfirmButtonExtender>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td style="width: 20px"></td>
                        <td valign="top">
                            <table class="tablaGris">
                                <tr>
                                    <th>
                                        PALLETs ADICIONADOS
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvPallets" runat="server" AutoGenerateColumns="false" CssClass="tablaGris"
                                            FooterStyle-CssClass="thGris" ShowFooter="True" EmptyDataText="&lt;blockquote&gt;No se han adicionado Pallets a la orden&lt;/blockquote&gt;">
                                            <Columns>
                                                <asp:BoundField DataField="numPallet" HeaderText="No." ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="idPallet" HeaderText="ID Pallet" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="peso" HeaderText="Peso (Kg)" ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="observacion" HeaderText="Observación" />
                                                <asp:BoundField DataField="novedad" HeaderText="Novedades" HtmlEncode="false" />
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:GridView ID="gvDetalle" style="width:100%" runat="server" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:BoundField DataField="nombreProducto" HeaderText="Producto" />
                                                                <asp:BoundField DataField="cantidadRecibida" HeaderText="Cantidad" 
                                                                    ItemStyle-HorizontalAlign="Center" >
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Opc." ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibImprimir" runat="server" CommandArgument='<%#Bind("idPallet") %>'
                                                            CommandName="Imprimir" ImageUrl="~/images/pdf.gif" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle CssClass="thGris" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <uc2:ModalProgress ID="ModalProgress1" runat="server" />
    </form>
</body>
</html>
