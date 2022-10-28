<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AgregarDetalleOrdenCompraGeneral.aspx.vb" Inherits="BPColSysOP.AgregarDetalleOrdenCompraGeneral" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>


<%@ Register src="../ControlesDeUsuario/ModalProgress.ascx" tagname="ModalProgress" tagprefix="uc2" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Agregar Detalle Orden de Compra General</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <style type="text/css" >
    #pnlInfoOrdenCompra .tablaGris
    {
    	float:left;
    	width:50%;
    }
    </style>
    <script type="text/javascript" language="javascript">
        function CalcularTotalDistribucion() {
            try {
                var arrTextBox = document.getElementsByTagName("input");
                if (document.getElementById("hfCantidadDistribucion").value == "") { document.getElementById("hfCantidadDistribucion").value = "0" }
                var totalDistribucion = 0;
                for (var i = 0; i < arrTextBox.length; i++) {
                    if (arrTextBox(i).id.indexOf("txtCantidadRegion") != -1) {
                        if (arrTextBox(i).value.trim() != "") { totalDistribucion += parseFloat(arrTextBox(i).value); }
                    }
                }
                document.getElementById("hfCantidadDistribucion").value = totalDistribucion;                
            } catch (e) { }
        }
        function TotalesValidos(source, args) {
            try {
                var totalOrden = parseFloat(document.getElementById("hfTotalOrdenCompra").value);
                var totalDistribucion = parseFloat(document.getElementById("hfCantidadDistribucion").value);
                if (totalDistribucion != 0) {
                    if (totalOrden == totalDistribucion) {
                        args.IsValid = true;
                    } else {
                        args.IsValid = false;
                    }
                } else {
                    args.IsValid = true;
                }
            } catch (e) {
                args.IsValid = false;
            }
        }
        function ExisteDistribucion(source, args) {
            try {
                var totalDistribucion = parseFloat(document.getElementById("hfCantidadDistribucion").value);
                if (!isNaN(totalDistribucion)) {
                    if (totalDistribucion > 0) {
                        args.IsValid = true;
                    } else {
                        args.IsValid = false;
                    }
                } else {
                    args.IsValid = false;
                }
            } catch (e) {
                args.IsValid = false;
            }
        }
    </script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>    
        <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />    
    </div>
    <div>
        <div id="pnlInfoOrdenCompra" style="width: 800px;">
            <p class="subtitulo">
                Orden de Compra</p>
            <table class="tablaGris">
                <tr>
                    <td style="width: 40%">
                        Numero de Orden:
                    </td>
                    <td>
                        <asp:Label ID="lblNumeroOrden" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Proveedor:
                    </td>
                    <td>
                        <asp:Label ID="lblProveedor" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Moneda:
                    </td>
                    <td>
                        <asp:Label ID="lblMoneda" runat="server" Text="" />
                    </td>
                </tr>
            </table>
            <table class="tablaGris">
                <tr>
                    <td style="width: 40%">
                        Incoterm:
                    </td>
                    <td>
                        <asp:Label ID="lblIncoterm" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Observación:
                    </td>
                    <td>
                        <asp:Label ID="lblObservacion" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Estado:
                    </td>
                    <td>
                        <asp:Label ID="lblEstado" runat="server" Text="" />
                    </td>
                </tr>
            </table>
            <div style="clear: both;">
            </div>
        </div>
        <table class="tablaGris" width="800">
            <tr>
                <th style="width: 30px;">
                    <asp:ImageButton ID="ibMostrarOcultarEdicionOrden" runat="server" ImageUrl="~/images/arrow_up2.gif"
                        ToolTip="Mostrar/Ocultar formulario de edición de la orden" />
                </th>
                <th style="width:770px;">
                    <div>
                        Editar Orden No.
                        <asp:Label ID="lblEditarOrdenNo" runat="server" Text=""></asp:Label>
                        <asp:Label ID="lblTituloEdicionOrdenCompra" runat="server" Text=""></asp:Label>
                    </div>
                </th>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Panel ID="pnlEdicionOrdenCompra" runat="server" style="overflow:hidden;">
                        <table width="100%">
                            <tr>
                                <td>
                                    Proveedor:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEditarProveedorOrden" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvProveedor" runat="server" ControlToValidate="ddlEditarProveedorOrden"
                                        Display="Dynamic" InitialValue="0" CssClass="bloque" ValidationGroup="OrdenCompra"
                                        ErrorMessage="Seleccione un proveedor, por favor"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Moneda:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEditarMonedaOrden" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvMoneda" runat="server" ControlToValidate="ddlEditarMonedaOrden"
                                        Display="Dynamic" InitialValue="0" CssClass="bloque" ValidationGroup="OrdenCompra"
                                        ErrorMessage="Seleccione una moneda, por favor"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Incoterm:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEditarIncotermOrden" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvIncoterm" runat="server" ControlToValidate="ddlEditarIncotermOrden"
                                        InitialValue="0" CssClass="bloque" ValidationGroup="OrdenCompra" ErrorMessage="Seleccione un termino  de negociación (Incoterm), por favor"
                                        Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Observación:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEditarObservacionOrden" runat="server" TextMode="MultiLine" Height="54px"
                                        Width="100%" MaxLength="399" ValidationGroup="OrdenCompra"></asp:TextBox>
                                    <div>
                                        <asp:RegularExpressionValidator ID="revEditarObservacionOrden" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                            Display="Dynamic" ControlToValidate="txtEditarObservacionOrden" ValidationGroup="OrdenCompra" ErrorMessage="La observación contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr id="trDistribucionRegional" runat="server">
                                <td>
                                    Distribución Por Región:
                                </td>
                                <td>
                                    <asp:GridView ID="gvRegion" runat="server" AutoGenerateColumns="False">
                                        <FooterStyle CssClass="thGris" />
                                        <Columns>
                                            <asp:BoundField DataField="region" HeaderText="Región" ItemStyle-CssClass="field">
                                                <ItemStyle CssClass="field" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="Cantidad">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCantidadRegion" MaxLength="8" runat="server" onkeyup="CalcularTotalDistribucion();"
                                                        Text='<%# Bind("cantidad") %>'></asp:TextBox>
                                                    <div style="display: block">
                                                        <asp:RegularExpressionValidator ID="revCantidadRegion" runat="server" ErrorMessage="El campo cantidad es numérico. Digite un número válido"
                                                            ControlToValidate="txtCantidadRegion" Display="Dynamic" ValidationExpression="(\s+)?(\d+)(\s+)?"
                                                            ValidationGroup="OrdenCompra">
                                                        </asp:RegularExpressionValidator>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="idRegion" HeaderText="ID Region" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:HiddenField ID="hfCantidadDistribucion" runat="server" Value="0" />
                                    
                                </td>
                            </tr>
                            
                        </table>
                    </asp:Panel>
                    <cc1:CollapsiblePanelExtender ID="cpeDetailCollapser" runat="server" CollapseControlID="ibMostrarOcultarEdicionOrden"
                        Enabled="True" ExpandControlID="ibMostrarOcultarEdicionOrden" TargetControlID="pnlEdicionOrdenCompra"
                        CollapsedImage="~/images/arrow_down2.gif" ExpandedImage="~/images/arrow_up2.gif"
                        ImageControlID="ibMostrarOcultarEdicionOrden" ScrollContents="False" SuppressPostBack="True"
                        Collapsed="False" CollapsedText="[Formulario Oculto ...]" ExpandedText="[Formulario Visible ...]"
                        TextLabelID="lblTituloEdicionOrdenCompra">
                    </cc1:CollapsiblePanelExtender>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                <div style="display: block">
                                        <asp:CustomValidator ID="cvExisteCantidadDistribucion" runat="server" ErrorMessage="Debe proporcionar la distribución de cantidades por región"
                                            ValidationGroup="OrdenCompra" Display="Dynamic" ClientValidationFunction="ExisteDistribucion"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCantidadDistribucion" runat="server" ErrorMessage="El total por regiones no corresponde con el total del detalle de la Orden"
                                            ValidationGroup="OrdenCompra" Display="Dynamic" ClientValidationFunction="TotalesValidos"></asp:CustomValidator>
                                    </div>
                    <asp:Button ID="btnEditarOrdenCompra" ValidationGroup="OrdenCompra" CssClass="boton"
                        runat="server" Text="Confirmar" />
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton" />
                    <cc1:ConfirmButtonExtender ID="cbeCancelarAccion" TargetControlID="btnCancelar" ConfirmText="Esta seguro de cancelar los cambios?" runat="server">
                                    </cc1:ConfirmButtonExtender>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <label class="comentario" style="color:#D7DF01;">
                        Nota: para confirmar sus cambios por favor de click en el boton "Confirmar" de lo
                        contrario estos no sera confirmados.</label>
                </td>
            </tr>
        </table>
        <!-- **************Bloque para ingresar el detalle de la orden de compra***************-->
        <asp:Panel ID="pnlAdicionarDetalleOrdenCompra" runat="server">
            <table class="tablaGris" width="800px">
                <tr>
                    <th colspan="2" align="center">
                        Detalles de la Orden de Compra
                    </th>
                </tr>                               
                <tr>
                    <td>
                        Detalles de la orden de compra:
                    </td>
                    <td>
                        <asp:Panel ID="pnlAgregarDetalleOrdenCompra" runat="server">
                            <asp:ImageButton runat="server" ImageUrl="~/images/add.png" ID="imgBtnAgregarDetalle"
                                ToolTip="Adicionar Detalle" />
                            <label class="negrita">
                                Agregar detalle a la orden de compra</label></asp:Panel>
                        <asp:Panel ID="pnlListarDetalleOrdenCompra" runat="server">
                            <asp:GridView ID="gvDetalleOrdenCompra" runat="server" CssClass="tablaGris" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField DataField="fabricante" HeaderText="Fabricante" />
                                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                                    <asp:BoundField DataField="TipoUnidad" HeaderText="Tipo de Unidad" />
                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="valorUnitario" HeaderText="Valor Unitario" DataFormatString="{0:C}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="observacion" HeaderText="Observacion" />
                                    <asp:TemplateField HeaderText="Opciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnEditarDetalleOrdenCompra" runat="server" CommandArgument='<%# Bind("idDetalle") %>'
                                                CommandName="Editar" ImageUrl="~/images/Edit-32.png" />
                                            <asp:ImageButton ID="imgBtnEliminarDetalleOrdenCompra" runat="server" CommandArgument='<%# Bind("idDetalle") %>'
                                                CommandName="Eliminar" ImageUrl="~/images/Delete-32.png" Visible="true" />
                                                <cc1:ConfirmButtonExtender
                                                    TargetControlID="imgBtnEliminarDetalleOrdenCompra" ConfirmText="Esta seguro de eliminar este detalle?" ID="cbeEliminarDetalle" runat="server">
                                                </cc1:ConfirmButtonExtender>
                                            <asp:HiddenField ID="hfPosicionDetalleOrdenCompra" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="hfTotalOrdenCompra" runat="server" Value="0" />
                        </asp:Panel>
                    </td>
                </tr>
                <!-- ********************** Detalle Adicional de la orden ********************************** -->
                <tr id="trProductoAdicional" runat="server">
                    <td>
                        Producto Adicional:
                    </td>
                    <td>
                        <asp:Panel ID="pnlProductoAdicional" runat="server">
                            <asp:UpdatePanel ID="upMostrarFormProductoAdicional" runat="server">
                            <ContentTemplate>
                            <asp:ImageButton runat="server" ImageUrl="~/images/add.png" ID="imgBtnAgregarProductoAdicional"
                                ToolTip="Agregar Producto Adicional" />
                            <label class="negrita">
                                Agregar producto adicional a la orden de compra</label>
                            </ContentTemplate>
                            </asp:UpdatePanel>
                            </asp:Panel>
                        <asp:Panel ID="pnlListarProductoAdicional" runat="server">
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
                                            <asp:ImageButton ID="imgBtnEditarProductoAdicional" runat="server" CommandArgument='<%# Bind("idDetalle") %>'
                                                CommandName="Editar" ImageUrl="~/images/Edit-32.png" />
                                            <asp:ImageButton ID="imgBtnEliminarProductoAdicional" runat="server" CommandArgument='<%# Bind("idDetalle") %>'
                                                CommandName="Eliminar" ImageUrl="~/images/Delete-32.png" OnClientClick="return confirm('Realmente desea eliminar el producto adicional indicado?');" />
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
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div>
            <asp:HiddenField ID="hfModalPopUp" runat="server" />
            <cc1:ModalPopupExtender ID="mpeAgregarDetalle" TargetControlID="hfModalPopUp" BackgroundCssClass="modalBackground"
                PopupControlID="pnlDetalleOrdenCompra" runat="server">
            </cc1:ModalPopupExtender>
        </div>
        <!-- **************Fin Bloque para ingresar el detalle de la orden de compra***************-->
        <!-- **************Listado de Detalle Agregados a la orden de compra actual****************-->
        <!-- **************Fin Listado de Detalle Agregados a la orden de compra actual****************-->
        <asp:Panel ID="pnlDetalleOrdenCompra" runat="server" CssClass="modalPopUp" Style="width: 500px;display:none;">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <div>
                        <div style="text-align: right">
                            <asp:ImageButton ID="imgBtnCerrarPopUp" runat="server" ImageUrl="~/images/cerrar.gif" /></div>
                        <uc1:EncabezadoPagina ID="EncabezadoPaginaAgregarDetalle" runat="server" />
                    </div>
                    <table width="500px" class="tablaGris">
                        <tr>
                            <th colspan="2">
                                <div>
                                    <asp:Label ID="lblTituloAccion" runat="server" Text=""></asp:Label></div>
                            </th>
                        </tr>
                        <tr>
                            <td style="width: 35%;">
                                Fabricante:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlFabricante" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvFabricante" runat="server" ErrorMessage="Seleccione el fabricante"
                                        InitialValue="0" ControlToValidate="ddlFabricante" Display="Dynamic" ValidationGroup="AgregarDetalle">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Producto:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlProducto" runat="server" AutoPostBack="True">
                                </asp:DropDownList>
                                <div>
                                    <asp:Label ID="lblCantidadProducto" CssClass="comentario" runat="server" Text=""></asp:Label>
                                </div>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvProducto" runat="server" ErrorMessage="Seleccione el producto"
                                        InitialValue="0" ControlToValidate="ddlProducto" Display="Dynamic" ValidationGroup="AgregarDetalle">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr runat="server" id="filaTipoUnidad">
                            <td>
                                Tipo Unidad:
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTipoUnidad" runat="server">
                                </asp:DropDownList>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvTipoUnidad" runat="server" ErrorMessage="Seleccione el tipo de unidad"
                                        InitialValue="0" ControlToValidate="ddlTipoUnidad" Display="Dynamic" ValidationGroup="AgregarDetalle">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Cantidad:
                            </td>
                            <td>
                                <asp:TextBox ID="txtCantidad" runat="server" MaxLength="8"></asp:TextBox>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" ErrorMessage="Ingrese la cantidad"
                                        ControlToValidate="txtCantidad" Display="Dynamic" ValidationGroup="AgregarDetalle">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revCantidad" runat="server" ErrorMessage="Ingrese solo números"
                                        ControlToValidate="txtCantidad" ValidationGroup="AgregarDetalle" Display="Dynamic"
                                        ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Valor Unitario:
                            </td>
                            <td>
                                <asp:TextBox ID="txtValorUnitario" runat="server" MaxLength="18"></asp:TextBox><label
                                    class="comentario">Formato 12.845.250,23</label>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvValorUnitario" runat="server" ErrorMessage="Ingrese el valor unitario"
                                        ControlToValidate="txtValorUnitario" Display="Dynamic" ValidationGroup="AgregarDetalle">
                                    </asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvValor" runat="server" ErrorMessage="El formato de moneda especificado no es válido"
                                        ControlToValidate="txtValorUnitario" Display="Dynamic" Operator="DataTypeCheck"
                                        Type="Currency" ValidationGroup="AgregarDetalle"></asp:CompareValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Observación:
                            </td>
                            <td>
                                <asp:TextBox ID="txtObservacionDetalleOrdenCompra" runat="server" TextMode="MultiLine"
                                    ValidationGroup="AgregarDetalle" Height="44px" Width="100%" 
                                    MaxLength="399"></asp:TextBox>
                                <div>
                                    <asp:RegularExpressionValidator ID="revObservacionDetalleOrdenCompra" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                        Display="Dynamic" ControlToValidate="txtObservacionDetalleOrdenCompra" ValidationGroup="AgregarDetalle" ErrorMessage="La observación contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <asp:Button ID="btnCrearDetalleOrden" runat="server" Text="Agregar Detalle" CssClass="boton"
                    ValidationGroup="AgregarDetalle" />
                <asp:Button ID="btnEditarDetalleOrden" runat="server" Text="Editar Detalle" CssClass="boton"
                    ValidationGroup="AgregarDetalle" Visible="False" />
                <asp:HiddenField ID="hfIdDetalle" runat="server" />
                <asp:HiddenField ID="hfIdOrdenCompra" runat="server" />
            </div>
        </asp:Panel>
        
        <div>
            <asp:HiddenField ID="hfModaPopUpProductoAdicional" runat="server" />
            <cc1:ModalPopupExtender ID="mpeAgregarProductoAdcional" TargetControlID="hfModaPopUpProductoAdicional" BackgroundCssClass="modalBackground"
                PopupControlID="pnlEditarAgregarProductoAdicional" runat="server" CancelControlID="imbBtnCerrarProductoAdicional">
            </cc1:ModalPopupExtender>
        </div>
         
        
        <asp:Panel ID="pnlEditarAgregarProductoAdicional" runat="server" CssClass="modalPopUp" Style="width: 500px;display:none;">
                                           
                        <div style="text-align: right">                            
                            <asp:ImageButton ID="imbBtnCerrarProductoAdicional" runat="server" ImageUrl="~/images/cerrar.gif" />
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">            
                        <ContentTemplate>    
                        <uc1:EncabezadoPagina ID="EncabezadoProductoAdicional" runat="server" />                                       
                        <table width="500px" class="tablaGris">
                        <tr>
                            <th colspan="2">
                                <div>
                                    <asp:Label ID="lblTituloAccionProductoAdicional" runat="server" Text=""></asp:Label></div>
                            </th>
                        </tr>
                        <tr>
                                <td style="width:80px;">Tipo de Producto:</td>
                                <td>
                                    <asp:DropDownList ID="ddlTipoProductoAdicional" runat="server" 
                                        AutoPostBack="True" >                                    
                                    </asp:DropDownList>     
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvTipoProductoAdicional" ControlToValidate="ddlTipoProductoAdicional" runat="server" InitialValue="0" 
                                        Display="Dynamic" ErrorMessage="Por favor, seleccione el tipo de producto." ValidationGroup="productoAdicional"></asp:RequiredFieldValidator>
                                    </div>                               
                                </td>
                            </tr>
                        <tr>
                                <td>Producto:</td>
                                <td>
                                    <asp:DropDownList ID="ddlProductoAdicional" runat="server">
                                    </asp:DropDownList><br />
                                    <asp:Label ID="lblCantidadProductoAdicional" runat="server" Text="" CssClass="comentario"></asp:Label>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvProductoAdicional" ControlToValidate="ddlProductoAdicional" runat="server" InitialValue="0" 
                                        Display="Dynamic" ErrorMessage="Por favor, seleccione el producto." ValidationGroup="productoAdicional"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>Cantidad:</td>
                                <td>
                                    
                                    <asp:TextBox ID="txtCantidadAcional" runat="server" MaxLength="8"></asp:TextBox>
                                    <div>
                                        <asp:RegularExpressionValidator ID="revCantidadAdicional" runat="server" ErrorMessage="El campo cantidad es numérico. Digite un número válido, por favor"
                                            ControlToValidate="txtCantidadAcional" ValidationGroup="productoAdicional" Display="Dynamic"
                                            ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvCantidadAdicional" runat="server" ValidationGroup="productoAdicional"
                                            ControlToValidate="txtCantidadAcional" ErrorMessage="Digite el valor del campo cantidad, por favor"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                    </table>
                    
                
                <div>
                 <asp:Button ID="btnAgregarAdicionales" ValidationGroup="productoAdicional" runat="server" Text="Agregar Producto Adicional" CssClass="boton" />
                <asp:Button ID="btnEditarAdicionles" ValidationGroup="productoAdicional" runat="server" Text="Editar Producto Adicional" CssClass="boton" />                
                <asp:HiddenField ID="hfIdDetalleAdicional" runat="server" />                                            
            </div>
             </ContentTemplate>                
            </asp:UpdatePanel>
        </asp:Panel>
        
    </div>
    <uc2:ModalProgress ID="ModalProgress1" runat="server" />
    </form>
</body>
</html>
