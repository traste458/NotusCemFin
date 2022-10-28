<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetalleOrdenRecepcionGeneral.aspx.vb"
    Inherits="BPColSysOP.DetalleOrdenRecepcionGeneral" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ModalProgress.ascx" TagName="ModalProgress"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detalle de Orden de Recepción</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function RefrescaUpdatePaneOrden(txt, hfield) {
            var filtro = $get(txt).value;
            var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\s áéíóúÁÉÍÓÚ.$*]+\s*$");
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
            enfocar("#" + txt);           
        }
        function enfocar(id) {
            $(id).focus();
        }
        
        function CompararCantidad() {
            var banderaNovedad = false;
            var retorno = false;
            var mensajePrevio = "Se encontraron las siguientes novedades:\n\n";
            var mensaje = "";

            var cantidadEsperada = $("#hfCantidadPermitida").val();
            var cantidadRegistrada = $("#hfCantidadPalletRegistrada").val();
            if (cantidadEsperada != cantidadRegistrada) {
                banderaNovedad = true;
                mensaje = "Las cantidades de los pallets no coinciden.\n";
            }

            var cantidadEsperadaProductoAdicional = $("#lblCantidadProductoAdicional").text();

            if ($("#hfBoolProductoAdicional").val() == 1) {
                var cantidadRegistradaProductoAdicional = $("#hfCantidadPalletAdicionalRegistrada").val();

                if (cantidadEsperadaProductoAdicional != cantidadRegistradaProductoAdicional) {
                    banderaNovedad = true;
                    mensaje += "Las cantidades de pallets producto adicional no coinciden.\n";
                }
            }
            if (banderaNovedad) {
                mensaje += "\nRealmente desea cerrar la orden?";
                mensaje = mensajePrevio + mensaje;
            } else {
                mensaje += "\nEsta seguro de cerrar la orden de recepción?";
            }
            if (confirm(mensaje)) {
                __doPostBack('lbCerrarOrden', '');
            } else {
                retorno = false;
            }
            return retorno;
        }
    </script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="smAjaxManager" runat="server" EnableScriptGlobalization = "true" >
    </asp:ScriptManager>
    <script type="text/javascript" language="javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function BeginRequestHandler(sender, args) {            
        }
        function EndRequestHandler(sender, args) {
            //Finaliza
            if ($("#txtFiltroMaterial").val() != "")
                $("#txtFiltroMaterial").focus();
        }
        </script>
    <asp:UpdatePanel ID="upGeneral" runat="server">
        <ContentTemplate>
            <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
            <asp:Panel ID="pnlGeneral" runat="server">
                <table class="tablaGris" width="800px">
                    <tr>
                        <th colspan="4" style="text-align: center">
                            INFORMACIÓN GENERAL DE LA ORDEN</th>
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
                            Orden de Compra:
                        </td>
                        <td>
                            <asp:Label ID="lblOrdenCompra" runat="server"></asp:Label>
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
                    <tr>
                        <td class="field">Consignado a:</td>
                        <td><asp:Label ID="lblConsignado" runat="server" /></td>
                        <td class="field">Destinatario:</td>
                    <td><asp:Label ID="lblDestinatario" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="field">
                            Estado:
                        </td>
                        <td>
                            <asp:Label ID="lblEstadoOrden" runat="server" />
                        </td>
                        <td class="field">
                            Cantidad Total:
                        </td>
                        <td>
                            <asp:Label ID="lblCantidadTotal" runat="server" Text=""></asp:Label>                            
                        </td>
                    </tr>
                    <tr id="trDistribucionPorRegiones" runat="server">
                        <td class="field">
                            Distribución por Regiones:
                        </td>
                        <td>
                            <asp:GridView ID="gvDistribucion" runat="server">
                                <RowStyle HorizontalAlign="Center" />
                            </asp:GridView>
                        </td>
                        <td class="field" valign="top">
                            Cantidad Recibida:
                        </td>                
                        <td valign="top">
                            <asp:Label ID="lblCantidadRecibida" runat="server" Text=""></asp:Label>
                        </td>    
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="BtnCerrarRecepcion" OnClientClick="return CompararCantidad();" runat="server" Text="Cerrar Recepción" />                            
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hfIdTipoProducto" runat="server" />
                <asp:HiddenField ID="hfIdOrdenRecepcion" runat="server" />
                <asp:HiddenField ID="hfIdOrdenCompra" runat="server" />
                <asp:HiddenField ID="hfIdEstadoOrdenRecepcion" runat="server" />
                <asp:HiddenField ID="hfCantidadPermitida" runat="server" />
                <asp:HiddenField ID="hfCantidadPalletRegistrada" runat="server" />    
                <asp:HiddenField ID="hfCantidadCajaEmpaqueTemporal" runat="server" />            
                
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
                                    <tr id="trPeso" runat="server">
                                        <td class="field">
                                            Peso (Kg):
                                        </td>
                                        <td>
                                            <div style="display: inline">
                                                <asp:TextBox ID="txtPeso" runat="server" MaxLength="10"></asp:TextBox>&nbsp;&nbsp;<asp:Label ID="lblFormato"
                                                    runat="server" Text="Formato ###,##" Font-Italic="True" Font-Size="8pt" ForeColor="Gray"></asp:Label></div>
                                            <div style="display: block">
                                                <asp:RequiredFieldValidator ID="rfvPeso" runat="server" ErrorMessage="Digite el peso del pallet, por favor"
                                                    Display="Dynamic" ControlToValidate="txtPeso" ValidationGroup="crearPallet"></asp:RequiredFieldValidator>
                                            </div>
                                            <div style="display: block">
                                                <asp:RegularExpressionValidator ID="revPeso" runat="server" ErrorMessage="Peso no válido. Ingrese el formato indicado."
                                                    ControlToValidate="txtPeso" Display="Dynamic" ValidationExpression="^(\d{1,6})(,\d{2})*$"
                                                    ValidationGroup="crearPallet"></asp:RegularExpressionValidator>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="field">
                                            Observación:
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtObservacion" runat="server" Columns="40" Rows="3" 
                                                TextMode="MultiLine" MaxLength="399" ValidationGroup="crearPallet"></asp:TextBox>
                                            <div>
                                                <asp:RegularExpressionValidator ID="revObservacion" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                                    Display="Dynamic" ValidationGroup="crearPallet" 
                                                    ErrorMessage="La observación contiene caracteres no validos, por favor verifique." 
                                                    ControlToValidate="txtObservacion"></asp:RegularExpressionValidator>
                                            </div>
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
                                                        Material:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:HiddenField ID="hfFlagFiltradoMaterial" runat="server" /> 
                                                        <asp:TextBox ID="txtFiltroMaterial" runat="server" Width="120px" MaxLength="40"  
                                                            onkeyup="RefrescaUpdatePaneOrden('txtFiltroMaterial','hfFlagFiltradoMaterial');" ></asp:TextBox>
                                                        -                                                        
                                                                <asp:DropDownList ID="ddlMaterial" runat="server">
                                                                </asp:DropDownList>
                                                                <div>
                                                                    <asp:Label ID="lblCantidadMateriales" runat="server" Text="Label" CssClass="comentario"></asp:Label></div>
                                                                <div>
                                                                        <asp:RequiredFieldValidator ID="rfvProducto" runat="server" ErrorMessage="Escoja un Material, por favor"
                                                                            ControlToValidate="ddlMaterial" Display="Dynamic" InitialValue="0" 
                                                                            ValidationGroup="crearCaja"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                <asp:HiddenField ID="hfCantidadDisponible" runat="server" />                                                                                                                        
                                                        
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field">
                                                        Región:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlRegion" runat="server">
                                                        </asp:DropDownList>
                                                        <div style="display: block">
                                                            <asp:RequiredFieldValidator ID="rfvRegion" runat="server" ErrorMessage="Escoja una Región, por favor"
                                                                ControlToValidate="ddlRegion" Display="Dynamic" InitialValue="0" ValidationGroup="crearCaja"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                    <td class="field">
                                                        Cantidad:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCantidad" runat="server" MaxLength="8"></asp:TextBox>
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
                                                <asp:LinkButton ID="lbAdicionarCaja" runat="server" ValidationGroup="crearCaja" CssClass="search"><img src="../images/Folder-add-32.png" alt="" />&nbsp;Adicionar Caja</asp:LinkButton>
                                                <br />
                                                <br />
                                                <asp:GridView ID="gvCajas" runat="server" AutoGenerateColumns="False" 
                                                    ShowFooter="True" style="width:100%"
                                                    
                                                    EmptyDataText="&lt;blockquote&gt;&lt;i&gt;No existen cajas temporalmente registradas&lt;/i&gt;&lt;/blockquote&gt;">
                                                    <Columns>
                                                        <asp:BoundField DataField="numCaja" HeaderText="No." 
                                                            ItemStyle-HorizontalAlign="Center" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="idCaja" HeaderText="ID Caja" 
                                                            ItemStyle-HorizontalAlign="Center" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="producto" HeaderText="Producto" />
                                                        <asp:BoundField DataField="material" HeaderText="Material" />
                                                        <asp:BoundField DataField="region" HeaderText="Región" />
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
                                        </td>
                                    </tr>
                                </table>
                                <table class="tablaGris" id="tblProductoAdicional" runat="server">
                                <tr id="trProductoAdicional" runat="server">
                                        <td class="field">
                                            Adicional:
                                        </td>
                                        <td>
                                            <table width="100%">
                                                <tr>
                                                    <th colspan="4">
                                                        PRODUCTO ADICIONAL
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <td class="field">
                                                        Producto:
                                                    </td>
                                                    <td colspan="3">                                                                                                                
                                                                <asp:DropDownList ID="ddlProductoAdicional" runat="server">
                                                                </asp:DropDownList>
                                                                &nbsp;<asp:Label ID="lblCantidadAdicional" runat="server" Font-Italic="True" Font-Size="8pt"
                                                                    ForeColor="Gray"></asp:Label><div>
                                                                        <asp:RequiredFieldValidator ID="rfvProductoAdicional" runat="server" ErrorMessage="Escoja un Producto, por favor"
                                                                            ControlToValidate="ddlProductoAdicional" Display="Dynamic" InitialValue="0" ValidationGroup="crearAdicional"></asp:RequiredFieldValidator>
                                                                    </div>                                                           
                                                    </td>
                                                </tr>
                                                <tr>                                                    
                                                    <td class="field">
                                                        Cantidad:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCantidadAdicional" runat="server" MaxLength="8"></asp:TextBox>
                                                        <div style="display: block">
                                                            <asp:RequiredFieldValidator ID="rfvCantidadAdicional" runat="server" ErrorMessage="Digite la cantidad recibida, por favor"
                                                                ControlToValidate="txtCantidadAdicional" Display="Dynamic" ValidationGroup="crearAdicional"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div style="display: block">
                                                            <asp:RegularExpressionValidator ID="revCantidadAdicional" runat="server" ErrorMessage="El campo es numérico.<br/>Digite un número válido, por favor"
                                                                ControlToValidate="txtCantidadAdicional" Display="Dynamic" ValidationExpression="(\s+)?(\d+)(\s+)?"
                                                                ValidationGroup="crearAdicional"></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div>
                                                <br />
                                                <asp:LinkButton ID="lnkAgregarProductoAdicional" runat="server" ValidationGroup="crearCaja" CssClass="search"><img src="../images/Folder-add-32.png" alt="" />&#160;Adicionar</asp:LinkButton>
                                                <br />
                                                <br />
                                                <asp:UpdatePanel ID="upActulizacionGrillaProductoAdicional" runat="server">
                                                <ContentTemplate>
                                                <asp:GridView ID="gvProductoAdicional" runat="server" CssClass="tablaGris" AutoGenerateColumns="False" style="width:500px;">
                                                    <Columns>                                    
                                                        <asp:BoundField DataField="producto" HeaderText="Producto" />                                    
                                                        <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>                                    
                                                        <asp:TemplateField HeaderText="Opciones" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:ImageButton ID="ibEliminarCaja" runat="server" ImageUrl="~/images/remove.png"
                                                                    CommandName="Anular" ToolTip="Remover Caja" CommandArgument='<%#Bind("idCaja") %>'
                                                                    OnClientClick="return confirm('¿Realmente desea remover el item indicado?');" />
                                                                <asp:HiddenField ID="hfPosicionProductoAdicional" runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                <asp:HiddenField ID="hfTotalProductoAdicional" runat="server" Value="0" />
                                                </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                            <br />                                                                                        
                                            
                                            <table class="tablaGris" id="tblCrearPalletAdicional" runat="server">
                <tr>
                    <td>
                        Peso(Kg):
                    </td>
                    <td>
                        <asp:TextBox ID="txtPesoPalletAdicionl" runat="server" ValidationGroup="crearPalletAdicional" 
                            MaxLength="18"></asp:TextBox>Formato ###,##
                        <div>
                        <asp:RequiredFieldValidator ID="rfvPesoPalletAdicional" runat="server" ControlToValidate="txtPesoPalletAdicionl" Display="Dynamic" ValidationGroup="crearPalletAdicional"
                            ErrorMessage="Ingrese el formato de peso indicado, para el pallet de producto adicional"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revPesoPalletAdicional" runat="server" ErrorMessage="Peso no válido. Se espera un valor decimal, por favor verifique"
                                                    ControlToValidate="txtPesoPalletAdicionl" Display="Dynamic" ValidationExpression="(\d+)(,\d{1,2})?"
                                                    ValidationGroup="crearPalletAdicional"></asp:RegularExpressionValidator>     
                        <asp:CompareValidator ID="cvPesoPalletAdicional" runat="server" ControlToValidate="txtPesoPalletAdicionl" Display="Dynamic" ValidationGroup="crearPalletAdicional" 
	                        ValueToCompare="0" Operator="GreaterThan" ErrorMessage="El peso debe ser mayor de 0, por favor verifique."></asp:CompareValidator>                       
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">                                            
                        <asp:LinkButton ID="lnkCrearPalletProAdicional" runat="server" CssClass="search" ValidationGroup="crearPallet"><img 
                                            src="../images/package.png" alt=""/>&nbsp;Crear Pallet Producto Adicional</asp:LinkButton>
                    </td>
                </tr>
             </table>
                                            
                                            
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
                                                        <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:BoundField DataField="nombreProducto" HeaderText="Producto" />
                                                                <asp:BoundField DataField="region" HeaderText="Region" />
                                                                <asp:BoundField DataField="cantidadRecibida" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Opc." ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibImprimir" runat="server" CommandArgument='<%#Bind("idPallet") %>'
                                                            CommandName="Imprimir" ImageUrl="~/images/pdf.gif" />
                                                        <asp:ImageButton ID="imgBtnEliminarPallet" runat="server" ImageUrl="~/images/remove.png"
                                                                    CommandName="Eliminar" ToolTip="Eliminar Pallet" CommandArgument='<%#Bind("idPallet") %>'
                                                                    OnClientClick="return confirm('¿Realmente desea remover el pallet indicado?');" />    
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle CssClass="thGris" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                            <div style="height:25px;"></div>
                            <table class="tablaGris" style="width:100%" runat="server" id="tblPalletsProductoAdicional">
                                <tr>
                                    <th>
                                        PALLETs ADICIONADOS PRODUCTO ADICIONAL
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvPalletProductoAdicional" runat="server" AutoGenerateColumns="False" CssClass="tablaGris" style="width:100%"
                                            FooterStyle-CssClass="thGris" ShowFooter="True" 
                                            EmptyDataText="&lt;blockquote&gt;No se han adicionado Pallets para producto adicional&lt;/blockquote&gt;">
                                            <Columns>
                                                <asp:BoundField DataField="idPallet" HeaderText="No. Pallet" 
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Peso" HeaderText="Peso(Kg)">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" 
                                                    HeaderText="Detalle del Pallet">
                                                    <ItemTemplate>
                                                        <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <asp:BoundField DataField="nombreProducto" HeaderText="Producto" />                                                                
                                                                <asp:BoundField DataField="cantidadRecibida" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Opc." ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibImprimir" runat="server" CommandArgument='<%#Bind("idPallet") %>'
                                                            CommandName="Imprimir" ImageUrl="~/images/pdf.gif" />
                                                        <asp:ImageButton ID="imgBtnEliminarPallet" runat="server" ImageUrl="~/images/remove.png"
                                                                    CommandName="Eliminar" ToolTip="Eliminar Pallet" CommandArgument='<%#Bind("idPallet") %>'
                                                                    OnClientClick="return confirm('¿Realmente desea remover el pallet indicado?');" />    
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
