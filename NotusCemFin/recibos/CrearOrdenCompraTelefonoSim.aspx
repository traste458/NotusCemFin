<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenCompraTelefonoSim.aspx.vb" 
    Inherits="BPColSysOP.CrearOrdenCompraTelefonoSim" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ModalProgress.ascx" TagName="ModalProgress"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Creación de Orden de Compra</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />

    <script src="../include/jquery-1.js" type="text/javascript"></script>

    <style type="text/css">
        .tablaGris
        {
            padding: 0 10px;
            margin: 0;
            width: 433px;
        }
        .bloque
        {
            display: block;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function mostrar() {
            $(".calendarTheme").css({ "z-index": "2000000" });
        }
        function focoArriba() {
            setTimeout('subir()', 5000);
        }
        function subir() {
            //$(window).load(function() {                
                $("html:not(:animated),body:not(:animated)",parent.document).animate({ scrollTop: 0 }, 1100);
            //});
        }
        function subir2() {                            
            $("html:not(:animated),body:not(:animated)", parent.document).animate({ scrollTop: 0 }, 1100);            
        }                            
        function ocultar() {
            $("#lblMensajeDetalleOrden").fadeOut(20000, function() { $("#lblMensajeDetalleOrden").html(""); });
        }
        function ocultarAdicional() {            
            $("#lblDetalleOrdenAdicionado").fadeOut(20000, function() { $("#lblDetalleOrdenAdicionado").html(""); });
        }
        function modificarAltoFramePadre() {
            $("#frModulo", parent.document).height($("body").height()+80);
        }
        String.prototype.trim = function() { return this.replace(/^[\s\t\r\n]+|[\s\t\r\n]+$/g, "") }
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

        function resizeIframe() {
            if (window.frameElement) {
                var iFrameID = window.frameElement.id;
                if (self == parent) return false; /* Checks that page is in iframe. */
                else {
                    if (document.getElementById && document.all) {
                        var framePageHeight
                        if (document.body.offsetHeight) { framePageHeight = document.body.offsetHeight + 40; }
                        else { framePageHeight = document.body.scrollHeight + 40; }
                        parent.document.getElementById(iFrameID).style.height = framePageHeight;
                    }
                }
            }
        }        
    </script>

</head>
<body class="cuerpo2" id="contenedor" style="background: none; margin: 0;"   >    
    <form id="form1" runat="server">
    <asp:ScriptManager ID="smAjaxManager" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
            <asp:UpdatePanel ID="upEncabezado" runat="server"><ContentTemplate>
            <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
            </ContentTemplate></asp:UpdatePanel>
            <table class="tablaGris" style="width: 100%">
                <tr>
                    <th colspan="2" align="center">
                        INFORMACIÓN GENERAL DE LA ORDEN
                    </th>
                </tr>
                <tr>
                    <td class="field" width="170">
                        Número de Orden:
                    </td>
                    <td>
                        <asp:TextBox ID="txtNumeroOrden" runat="server" MaxLength="15"></asp:TextBox>
                        <div style="display: block;">
                            <asp:RegularExpressionValidator ID="revNumeroOrden" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                Display="Dynamic" ControlToValidate="txtNumeroOrden" ValidationGroup="OrdenCompra" ErrorMessage="El número de orden contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvNumeroOrden" runat="server" ControlToValidate="txtNumeroOrden"
                                Display="Dynamic" CssClass="bloque" ErrorMessage="Digite el número de la orden, por favor"
                                ValidationGroup="OrdenCompra"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Proveedor:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProveedor" runat="server">
                        </asp:DropDownList>
                        <div style="display: block;">
                            <asp:RequiredFieldValidator ID="rfvProveedor" runat="server" ControlToValidate="ddlProveedor"
                                Display="Dynamic" InitialValue="0" CssClass="bloque" ValidationGroup="OrdenCompra"
                                ErrorMessage="Seleccione un proveedor, por favor"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Moneda:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMoneda" runat="server">
                        </asp:DropDownList>
                        <div style="display: block;">
                            <asp:RequiredFieldValidator ID="rfvMoneda" runat="server" ControlToValidate="ddlMoneda"
                                Display="Dynamic" InitialValue="0" CssClass="bloque" ValidationGroup="OrdenCompra"
                                ErrorMessage="Seleccione una moneda, por favor"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Incoterm:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlIncoterm" runat="server">
                        </asp:DropDownList>
                        <div style="display: block;">
                            <asp:RequiredFieldValidator ID="rfvIncoterm" runat="server" ControlToValidate="ddlIncoterm"
                                InitialValue="0" CssClass="bloque" ValidationGroup="OrdenCompra" ErrorMessage="Seleccione un termino  de negociación (Incoterm), por favor"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                            
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Fecha prevista de llegada:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFechaPrevista" runat="server" MaxLength="10"></asp:TextBox>
                        <cc1:CalendarExtender Format="dd/MM/yyyy" CssClass="calendarTheme" ID="txtFechaPrevista_CalendarExtender"
                            runat="server" TargetControlID="txtFechaPrevista" OnClientShown="mostrar" PopupButtonID="imgFechaPrevista">
                        </cc1:CalendarExtender>
                        <img src="../images/date-32.png" id="imgFechaPrevista" style="cursor: pointer;" alt="Fecha prevista de llegada"
                            title="Fecha prevista de llegada" />
                        <div>
                            <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaPrevista" runat="server" ErrorMessage="El formato de fecha no es valido, por favor verifique."
                                ControlToValidate="txtFechaPrevista" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvFechaPrevista" runat="server" ControlToValidate="txtFechaPrevista"
                                Display="Dynamic" ErrorMessage="Indique la fecha prevista de llegada, por favor."
                                ValidationGroup="OrdenCompra"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Observación:
                    </td>
                    <td>
                        <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" ValidationGroup="OrdenCompra"
                            Height="70px" Width="800px" MaxLength="399"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="revObservacion" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                Display="Dynamic" ControlToValidate="txtObservacion" ValidationGroup="OrdenCompra" ErrorMessage="La observación contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Detalle de la Orden:
                    </td>
                    <td>
                    <asp:UpdatePanel ID="upGeneral" runat="server">
                        <ContentTemplate>
                        <!-- **************Bloque para ingresar el detalle de la orden de compra***************-->
                        <asp:Panel ID="pnlAdicionarDetalle" runat="server">
                            <table class="tablaGris" width="800px">
                                <tr>
                                    <th style="width: 30px">
                                        <asp:ImageButton ID="ibMostrarOcultarDetalle" runat="server" 
                                            ImageUrl="~/images/arrow_up2.gif" 
                                            ToolTip="Mostrar/Ocultar formulario de creación o edición de detalle" />
                                    </th>
                                    <th align="center" style="width: 770px">
                                        INFORMACIÓN DEL DETALLE:&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblTitulo" runat="server"
                                            Text=""></asp:Label></th>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlFormularioDetalle" runat="server">
                                            <table>
                                                <tr>
                                                    <td style="width: 120px;" class="field">
                                                        Fabricante:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlFabricante" runat="server" AutoPostBack="True" ValidationGroup="valDetalleOrden">
                                                        </asp:DropDownList>
                                                        <div style="display: block;">
                                                            <asp:RequiredFieldValidator ID="rfvFabricante" runat="server" ValidationGroup="valDetalleOrden"
                                                                InitialValue="0" ControlToValidate="ddlFabricante" ErrorMessage="Seleccione un fabricante, por favor"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field">
                                                        Producto:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlProducto" runat="server" AutoPostBack="True">
                                                        </asp:DropDownList>
                                                        &nbsp;<asp:Label ID="lblCantidadProducto" CssClass="comentario" runat="server" Text=""></asp:Label><div
                                                            style="display: block;">
                                                            <asp:RequiredFieldValidator ID="rfvCantidadProducto" runat="server" ValidationGroup="valDetalleOrden"
                                                                InitialValue="0" ControlToValidate="ddlProducto" ErrorMessage="Seleccione un producto, por favor"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr runat="server" id="filaTipoUnidad">
                                                    <td class="field">
                                                        Unidad de Empaque:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlTipoUnidad" runat="server">
                                                        </asp:DropDownList>
                                                        <div style="display: block;">
                                                            <asp:RequiredFieldValidator ID="rfvTipoUnidad" runat="server" ValidationGroup="valDetalleOrden"
                                                                InitialValue="0" ControlToValidate="ddlTipoUnidad" ErrorMessage="Seleccione una unidad de empaque, por favor"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field">
                                                        Cantidad:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtCantidad" runat="server" MaxLength="8"></asp:TextBox>
                                                        <div style="display: block">                                                        
                                                            <asp:RegularExpressionValidator ID="rglCantidad" runat="server" ErrorMessage="El campo cantidad es numérico. Digite un número válido, por favor"
                                                                ControlToValidate="txtCantidad" ValidationGroup="valDetalleOrden" Display="Dynamic"
                                                                ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                                                            <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" ValidationGroup="valDetalleOrden"
                                                                ControlToValidate="txtCantidad" ErrorMessage="Digite el valor del campo cantidad, por favor"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cvCantidad" runat="server" ControlToValidate="txtCantidad" Display="Dynamic" ValidationGroup="valDetalleOrden" 
                                                                ValueToCompare="0" Operator="GreaterThan" ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field">
                                                        Valor Unitario:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtValorUnitario" runat="server" MaxLength="18"></asp:TextBox>&nbsp;<label
                                                            class="comentario">Formato ##.###,##</label>
                                                        <div style="display: block">
                                                             <asp:RegularExpressionValidator ID="rglValorUnitario" runat="server" 
                                                                ErrorMessage="Ingrese el formato de moneda indicado" ControlToValidate="txtValorUnitario" 
                                                                ValidationGroup="valDetalleOrden" Display="Dynamic"                                 
                                                                 ValidationExpression="^(\d{1,3})(\.?\d{3})*(,\d{1,3})*$"></asp:RegularExpressionValidator>                                                             
                                                            <asp:RequiredFieldValidator ID="rfvValorUnitario" runat="server" ValidationGroup="valDetalleOrden"
                                                                ControlToValidate="txtValorUnitario" ErrorMessage="Digite el valor unitario del producto, por favor"
                                                                Display="Dynamic"></asp:RequiredFieldValidator>
                                                            <asp:CompareValidator ID="cvValorUnitario" runat="server" ControlToValidate="txtValorUnitario" Display="Dynamic" ValidationGroup="valDetalleOrden" 
                                                                ValueToCompare="0" Operator="GreaterThan" ErrorMessage="El valor unitario debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="field">
                                                        Observacion:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtObservacionDetalleOrdenCompra" runat="server" TextMode="MultiLine"
                                                            ValidationGroup="DetalleOrdenCompra" Height="60px" Width="500px" 
                                                            MaxLength="399"></asp:TextBox>
                                                        <div>
                                                            <asp:RegularExpressionValidator ID="revObservacionDatalleOrdenCompra" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                                                Display="Dynamic" ControlToValidate="txtObservacionDetalleOrdenCompra" ValidationGroup="DetalleOrdenCompra" ErrorMessage="La observación contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div>
                                                <br />
                                                <asp:Button ID="btnCrearDetalleOrden" runat="server" Text="Registrar Detalle" CssClass="boton"
                                                    ValidationGroup="valDetalleOrden" />
                                                <asp:Button ID="btnEditarDetalleOrden" runat="server" Text="Editar Detalle" CssClass="boton"
                                                    ValidationGroup="valDetalleOrden" />
                                                <asp:Label ID="lblMensajeDetalleOrden" runat="server" Text=""></asp:Label>                                               
                                                <asp:HiddenField ID="hfIdDetalle" runat="server" />
                                            </div>
                                        </asp:Panel>
                                        <cc1:CollapsiblePanelExtender ID="cpeDetailCollapser" runat="server" CollapseControlID="ibMostrarOcultarDetalle"
                                            Enabled="True" ExpandControlID="ibMostrarOcultarDetalle" TargetControlID="pnlFormularioDetalle"
                                            CollapsedImage="~/images/arrow_down2.gif" ExpandedImage="~/images/arrow_up2.gif"
                                            ImageControlID="ibMostrarOcultarDetalle" ScrollContents="false" 
                                            SuppressPostBack="True" Collapsed="False"
                                            CollapsedText="[Formulario Oculto ...]" ExpandedText="[Formulario Visible ...]" 
                                            TextLabelID="lblTitulo">
                                        </cc1:CollapsiblePanelExtender>
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <!-- **************Fin Bloque para ingresar el detalle de la orden de compra***************-->
                        <!-- **************Bloque para listar el detalle de la orden de compra***************-->
                        <asp:Panel ID="pnlListarDetalleOrdenCompra" runat="server">
                            <asp:GridView ID="gvDetalleOrdenCompra" runat="server" CssClass="tablaGris" style="width:100%;"
                                AutoGenerateColumns="False" 
                                EmptyDataText="&lt;blockquote&gt;&lt;i&gt;No existen registros de Detalle de la Orden de Compra&lt;/i&gt;&lt;/blockquote&gt;">
                                <Columns>
                                    <asp:BoundField DataField="fabricante" HeaderText="Fabricante" >
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="producto" HeaderText="Producto" >
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="tipoUnidad" HeaderText="Unidad de Empaque" />
                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="valorUnitario" HeaderText="Valor Unitario" DataFormatString="{0:C}">
                                        <ItemStyle HorizontalAlign="Right" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="observacion" HeaderText="Observacion" />
                                    <asp:TemplateField HeaderText="Opciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibEditarDetalle" CommandName="Editar" CommandArgument='<%# Bind("idDetalleOrden") %>'
                                                runat="server" ImageUrl="~/images/Edit-32.png" />
                                            <asp:ImageButton ID="ibEliminarDetalle" runat="server" CommandName="Eliminar" CommandArgument='<%# Bind("idDetalleOrden") %>'
                                                ImageUrl="~/images/Delete-32.png" OnClientClick="return confirm('Realmente desea eliminar el detalle seleccionado?');" />
                                            <asp:HiddenField ID="hfPosicionDetalleOrdenCompra" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:HiddenField ID="hfPerfilUsuario" runat="server" />
                        <asp:HiddenField ID="hfTotalOrdenCompra" runat="server" Value="0" />
                        <!-- **************Fin Bloque para listar el detalle de la orden de compra***************-->
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>                
                
                <!--********************************* Opciones productos adicionales ***************************-->
                <tr id="trAccesorios" runat="server">
                    <td class="field">Productos Adicionales:</td>
                   
                    <td>
                        <asp:UpdatePanel ID="upProductoAdicional" runat="server" >
                        <ContentTemplate>                        
                        <table class="tablaGris" style="width:700px;">
                            <tr>
                                <th colspan="2">Productos Adicionales</th>
                            </tr>
                            <tr>
                                <td class="field" style="width:150px;">Tipo de Producto:</td>
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
                                <td class="field">Producto:</td>
                                <td>
                                    <asp:DropDownList ID="ddlProductoAdicional" runat="server">
                                    </asp:DropDownList>
                                    <asp:Label ID="lblCantidadProductoAdicional" runat="server" Text="" CssClass="comentario"></asp:Label>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvProductoAdicional" ControlToValidate="ddlProductoAdicional" runat="server" InitialValue="0" 
                                        Display="Dynamic" ErrorMessage="Por favor, seleccione el producto." ValidationGroup="productoAdicional"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="field">Cantidad:</td>
                                <td>
                                    
                                    <asp:TextBox ID="txtCantidadAcional" runat="server" MaxLength="8"></asp:TextBox>
                                    <div>
                                        <asp:RegularExpressionValidator ID="revCantidadAdicional" runat="server" ErrorMessage="El campo cantidad es numérico. Digite un número válido, por favor"
                                            ControlToValidate="txtCantidadAcional" ValidationGroup="productoAdicional" Display="Dynamic"
                                            ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvCantidadAdicional" runat="server" ValidationGroup="productoAdicional"
                                            ControlToValidate="txtCantidadAcional" ErrorMessage="Digite el valor del campo cantidad, por favor"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cvCantidadAdicional" runat="server" ControlToValidate="txtCantidadAcional" Display="Dynamic" ValidationGroup="productoAdicional" 
                                            ValueToCompare="0" Operator="GreaterThan" ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>                                
                                <td colspan="2">                                    
                                    <asp:Button ID="btnAgregarAdicionales" ValidationGroup="productoAdicional" runat="server" Text="Agregar Producto Adicional" CssClass="boton" />
                                    <asp:Button ID="btnEditarAdicionles" ValidationGroup="productoAdicional" runat="server" Text="Editar Producto Adicional" CssClass="boton" />
                                    <asp:Label ID="lblDetalleOrdenAdicionado" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField ID="hfIdDetalleAdicional" runat="server" />
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvProductoAdicionales" runat="server" CssClass="tablaGris"
                            AutoGenerateColumns="False" style="width:600px;"
                            EmptyDataText="&lt;blockquote&gt;&lt;i&gt;No existen registros de Producto Adicionales&lt;/i&gt;&lt;/blockquote&gt;">
                            <Columns>
                                <asp:BoundField DataField="producto" HeaderText="Producto" >
                                    <ItemStyle Width="200px" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" >
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Opciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ibEditarDetalleAdicional" CommandName="Editar" CommandArgument='<%# Bind("idDetalleOrden") %>'
                                                runat="server" ImageUrl="~/images/Edit-32.png" />
                                            <asp:ImageButton ID="ibEliminarDetalleAdicional" runat="server" CommandName="Eliminar" CommandArgument='<%# Bind("idDetalleOrden") %>'
                                                ImageUrl="~/images/Delete-32.png" OnClientClick="return confirm('Realmente desea eliminar el detalle seleccionado?');" />
                                            <asp:HiddenField ID="hfPosicionDetalleOrdenCompraAdicional" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
             <div>
        <br />
        <asp:Button ID="btnCrearOrden" runat="server" Text="Crear Orden" CssClass="boton"
            ValidationGroup="OrdenCompra" />
            &nbsp;</div>        
   
    <uc2:ModalProgress ID="mpWait" runat="server" />    
    </form>
</body>
</html>
