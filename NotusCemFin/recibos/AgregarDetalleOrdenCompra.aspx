<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AgregarDetalleOrdenCompra.aspx.vb" Inherits="BPColSysOP.AgregarDetalleOrdenCompra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>


<%@ Register src="../ControlesDeUsuario/ModalProgress.ascx" tagname="ModalProgress" tagprefix="uc2" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Agregar Detalle Orden de Compra</title>
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
        function mostrar() {
            $(".calendarTheme").css({ "z-index": "2000000" });
        }
        function CalcularTotalDistribucion() {
            try {
                var totalDistribucion = 0;
                $.each($("#gvRegion input"), function() {
                    totalDistribucion += parseInt($.trim($(this).val()));
                });
                $("#hfCantidadDistribucion").val(totalDistribucion);
                //console.log(totalDistribucion);
            } catch (e) {
                //alert(e);
            }
//            try {
//                var arrTextBox = document.getElementsByTagName("input");
//                if (document.getElementById("hfCantidadDistribucion").value == "") { document.getElementById("hfCantidadDistribucion").value = "0" }
//                var totalDistribucion = 0;
//                for (var i = 0; i < arrTextBox.length; i++) {
//                    if (arrTextBox(i).id.indexOf("txtCantidadRegion") != -1) {
//                        if (arrTextBox(i).value.trim() != "") { totalDistribucion += parseFloat(arrTextBox(i).value); }
//                    }
//                }
//                document.getElementById("hfCantidadDistribucion").value = totalDistribucion;                
//            } catch (e) { }
        }
        function TotalesValidos(source, args) {
            try {
                var totalOrden = parseFloat(document.getElementById("hfTotalOrdenCompra").value);
                var totalDistribucion = parseFloat(document.getElementById("hfCantidadDistribucion").value);
                if (totalDistribucion != 0) {
                    if (totalOrden == totalDistribucion) {
                        args.IsValid = true;
                    } else {
                    args.IsValid = true;
                    }
                } else {
                args.IsValid = true;
                }
            } catch (e) {
            args.IsValid = true;
            }
        }
        function ExisteDistribucion(source, args) {
            try {
                var totalDistribucion = parseFloat(document.getElementById("hfCantidadDistribucion").value);
                if (!isNaN(totalDistribucion)) {
                    if (totalDistribucion > 0) {
                        args.IsValid = true;
                    } else {
                    args.IsValid = true;
                    }
                } else {
                args.IsValid = true;
                }
            } catch (e) {
            args.IsValid = true;
            }
        }
        function InfoEstadoOrdenCompra() {
            var validaciones = "";
            validaciones = $("#hfInformacionEstadoOrdenCompra").val();           
            var mensajes = validaciones.split("|");
            var mensaje;
            mensaje = "La prealerta se encuentra con:\n\n";
            $.each(mensajes, function(key, value) {
                mensaje += value + "\n";
            });
            alert(mensaje);
        }
        function EliminarDetalleOrden(obj) {
            var validaciones = "";
            validaciones = obj.parent("td").find('input[id*="hfInfoEstadoDetalleOrden"]').val();
            if (validaciones == "") {
                return confirm('Realmente desea eliminar\nel detalle de orden de compra?');
            } else {
                var mensajes = validaciones.split("|");
                var mensaje;
                mensaje = "El detalle se encuentra con:\n\n";
                $.each(mensajes, function(key, value) {
                    mensaje += value + "\n";
                });
                alert(mensaje);
                return false;
            }
        }
        function validarCantidadEdicion(source, arg) {
            var cantidad = $("#txtCantidad").val();
            var cantidadMin = parseInt($("#hfCantidadMinEdicionPermitida").val());
            var cantidadMax = parseInt($("#hfCantidadMaxEdicionPermitida").val());
            if (!isNaN(cantidad)) {
                cantidad = parseInt(cantidad);
                if (cantidad > cantidadMax) {
                    $("#cvCantidadPermitida").text("La cantidad no puede ser mayor de " + cantidadMax + " , por favor verifique.");
                    arg.IsValid = false;
                } else if (cantidad < cantidadMin) {
                    $("#cvCantidadPermitida").text("La cantidad no puede ser menor de " + cantidadMin + " , por favor verifique.");
                    arg.IsValid = false;
                } else
                    arg.IsValid = true;
            }  
        }
    </script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upGeneral" runat="server">
    <ContentTemplate>
    <asp:HiddenField ID="hfIdTipoProducto" runat="server" />
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
                        Identificador de la Orden:
                    </td>
                    <td>
                        <asp:Label ID="lblIdOrden" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Número de la Orden:
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
                <tr>
                    <td>
                        Tipo Producto:
                    </td>
                    <td>
                        <asp:Label ID="lblTipoProducto" runat="server" Text="" />
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
                                <td width="170">Número de la Orden:</td>
                                <td>
                                    <asp:TextBox ID="txtNumeroOrden" runat="server" MaxLength="15"></asp:TextBox>
                                    <div>
                                        <asp:RegularExpressionValidator ID="revNumeroOrden" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                            Display="Dynamic" ControlToValidate="txtNumeroOrden" ValidationGroup="OrdenCompra" ErrorMessage="El número de orden contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvNumeroOrden" runat="server" ControlToValidate="txtNumeroOrden"
                                            Display="Dynamic" CssClass="bloque" ErrorMessage="Digite el número de la orden, por favor"
                                            ValidationGroup="OrdenCompra"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
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
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Fecha prevista de llegada:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaPrevista" runat="server" MaxLength="10"></asp:TextBox>                                    
                                    <cc1:CalendarExtender ID="txtFechaPrevista_CalendarExtender" runat="server" 
                                        TargetControlID="txtFechaPrevista" Format="dd/MM/yyyy" CssClass="calendarTheme" OnClientShown="mostrar" PopupButtonID="imgFechaPrevista">
                                    </cc1:CalendarExtender>
                                    <img runat="server" src="../images/date-32.png" id="imgFechaPrevista" style="cursor: pointer;" alt="Fecha prevista de llegada"
                                        title="Fecha prevista de llegada" />
                                    <div>
                                        <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaPrevista" runat="server"
                                            ErrorMessage="El formato de fecha no es valido, por favor verifique." ControlToValidate="txtFechaPrevista"
                                            ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvFechaPrevista" runat="server" ControlToValidate="txtFechaPrevista"
                                            Display="Dynamic" ErrorMessage="Indique la fecha prevista de llegada, por favor."
                                            ValidationGroup="OrdenCompra"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Observación:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEditarObservacionOrden" runat="server" TextMode="MultiLine" Height="54px"
                                        Width="100%" MaxLength="399"></asp:TextBox>
                                    <div>
                                        <asp:RegularExpressionValidator ID="revObservacion" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
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
                                                    <asp:TextBox ID="txtCantidadRegion" runat="server" MaxLength="8" onkeyup="CalcularTotalDistribucion();"
                                                        Text='<%# Bind("cantidad") %>'></asp:TextBox>
                                                    <div style="display: block">
                                                        <asp:RegularExpressionValidator ID="revCantidadRegion" runat="server" ErrorMessage="El campo cantidad es numérico. Digite un número válido"
                                                            ControlToValidate="txtCantidadRegion" Display="Dynamic" ValidationExpression="(\s+)?(\d+)(\s+)?"
                                                            ValidationGroup="OrdenCompra">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:CompareValidator ID="cvCantidadRegion" runat="server" ControlToValidate="txtCantidadRegion" Display="Dynamic" ValidationGroup="OrdenCompra" 
	                                                        ValueToCompare="0" Operator="GreaterThan" ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="idRegion" HeaderText="ID Region" />
                                        </Columns>
                                    </asp:GridView>
                                    <asp:HiddenField ID="hfCantidadDistribucion" runat="server" Value="0" />
                                    <div style="display: block">
                                        <asp:CustomValidator ID="cvExisteCantidadDistribucion" runat="server" ErrorMessage="Debe proporcionar la distribución de cantidades por región"
                                            ValidationGroup="OrdenCompra" Display="Dynamic" ClientValidationFunction="ExisteDistribucion"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cvCantidadDistribucion" runat="server" ErrorMessage="El total por regiones no corresponde con el total del detalle de la Orden"
                                            ValidationGroup="OrdenCompra" Display="Dynamic" ClientValidationFunction="TotalesValidos"></asp:CustomValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="btnEditarOrdenCompra" ValidationGroup="OrdenCompra" CssClass="boton"
                                        runat="server" Text="Actualizar" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <cc1:CollapsiblePanelExtender ID="cpeDetailCollapser" runat="server" CollapseControlID="ibMostrarOcultarEdicionOrden"
                        Enabled="True" ExpandControlID="ibMostrarOcultarEdicionOrden" TargetControlID="pnlEdicionOrdenCompra"
                        CollapsedImage="~/images/arrow_down2.gif" ExpandedImage="~/images/arrow_up2.gif"
                        ImageControlID="ibMostrarOcultarEdicionOrden" ScrollContents="False" SuppressPostBack="True"
                        Collapsed="false" CollapsedText="[Formulario Oculto ...]" ExpandedText="[Formulario Visible ...]"
                        TextLabelID="lblTituloEdicionOrdenCompra">
                    </cc1:CollapsiblePanelExtender>
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
                        <asp:Panel ID="pnlInfoEstadoOrdenCompra" runat="server" onclick="InfoEstadoOrdenCompra();" style="cursor:pointer;">
                            <strong>Información de la orden de compra:</strong>
                            <img id="imgInfoEstadoOrdenCompra" src="../images/Info-32.png" alt="Información de la orden de compra" title="Información de la orden de compra" />
                            <asp:HiddenField ID="hfInformacionEstadoOrdenCompra" runat="server" />
                        </asp:Panel>            
                        <asp:Panel ID="pnlAgregarDetalleOrdenCompra" runat="server">
                           <asp:LinkButton runat="server" ID="lnkAgregarDetalle" CssClass="negrita">
                                <img  src="../images/add.png" alt="Adicionar Detalle" title="Adicionar Detalle" />
                                Agregar detalle a la orden de compra
                           </asp:LinkButton>
                        </asp:Panel>                        
                        <asp:Panel ID="pnlListarDetalleOrdenCompra" runat="server">

                            <asp:GridView ID="gvDetalleOrdenCompra" runat="server" CssClass="tablaGris" AutoGenerateColumns="False" style="width:100%;">
                                <Columns>
                                    <asp:BoundField DataField="fabricante" HeaderText="Fabricante" >
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="producto" HeaderText="Producto" >
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TipoUnidad" HeaderText="Tipo de Unidad" />
                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="valorUnitario" HeaderText="Valor Unitario" DataFormatString="{0:C}">
                                        <ItemStyle HorizontalAlign="Right" Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="observacion" HeaderText="Observacion" >
                                        <ItemStyle Width="250px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Opciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnEditarDetalleOrdenCompra" runat="server" CommandArgument='<%# Bind("idDetalleOrden") %>'
                                                CommandName="Editar" ImageUrl="~/images/Edit-32.png" />
                                            <asp:ImageButton ID="imgBtnEliminarDetalleOrdenCompra" runat="server" CommandArgument='<%# Bind("idDetalleOrden") %>'
                                                CommandName="Eliminar" ImageUrl="~/images/Delete-32.png" OnClientClick="return EliminarDetalleOrden($(this));" />                                                                                           
                                            <asp:HiddenField ID="hfPosicionDetalleOrdenCompra" runat="server" />
                                            <asp:HiddenField ID="hfInfoEstadoDetalleOrden" runat="server" />
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
                <tr id="trProductoAdicional" runat="server" visible="false">
                    <td>
                        Producto Adicional:
                    </td>
                    <td>
                        <asp:Panel ID="pnlProductoAdicional" runat="server">
                            <asp:LinkButton runat="server" ID="lnkAgregarProductoAdicional" CssClass="negrita" >
                                <img src="../images/add.png" alt="Agregar producto adicional a la orden de compra" title="Agregar producto adicional a la orden de compra" />
                                Agregar producto adicional a la orden de compra
                            </asp:LinkButton>
                        </asp:Panel>
                        <asp:Panel ID="pnlListarProductoAdicional" runat="server">

                            <asp:GridView ID="gvProductoAdicional" runat="server" CssClass="tablaGris" AutoGenerateColumns="False" style="width:500px;">
                                <Columns>                                    
                                    <asp:BoundField DataField="producto" HeaderText="Producto" >                                    
                                        <ItemStyle Width="200px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>                                    
                                    <asp:TemplateField HeaderText="Opciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnEditarProductoAdicional" runat="server" CommandArgument='<%# Bind("idDetalleOrden") %>'
                                                CommandName="Editar" ImageUrl="~/images/Edit-32.png" />
                                            <asp:ImageButton ID="imgBtnEliminarProductoAdicional" runat="server" CommandArgument='<%# Bind("idDetalleOrden") %>'
                                                CommandName="Eliminar" ImageUrl="~/images/Delete-32.png" OnClientClick="return confirm('Realmente desea eliminar el producto adicional indicado?');" />
                                            <asp:HiddenField ID="hfPosicionProductoAdicional" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:HiddenField ID="hfTotalProductoAdicional" runat="server" Value="0" />
 
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div>
            <asp:HiddenField ID="hfModalPopUp" runat="server" />
            <cc1:ModalPopupExtender ID="mpeAgregarDetalle" TargetControlID="hfModalPopUp" BackgroundCssClass="modalBackground"
                PopupControlID="pnlDetalleOrdenCompra" CancelControlID="imgBtnCerrarPopUp" runat="server">
            </cc1:ModalPopupExtender>
        </div>        
        <!-- **************Fin Bloque para ingresar el detalle de la orden de compra***************-->
        <!-- **************Listado de Detalle Agregados a la orden de compra actual****************-->
        <!-- **************Fin Listado de Detalle Agregados a la orden de compra actual****************-->
        <asp:Panel ID="pnlDetalleOrdenCompra" runat="server" CssClass="modalPopUp" Style="width: 500px;display:none;">                                
                        <div style="text-align: right">
                            <asp:ImageButton ID="imgBtnCerrarPopUp" runat="server" ImageUrl="~/images/cerrar.gif" /></div>

                        <uc1:EncabezadoPagina ID="EncabezadoPaginaAgregarDetalle" runat="server" />
                  
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
                                <asp:HiddenField ID="hfCantidadMinEdicionPermitida" runat="server" />
                                <asp:HiddenField ID="hfCantidadMaxEdicionPermitida" runat="server" />
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" ErrorMessage="Ingrese la cantidad"
                                        ControlToValidate="txtCantidad" Display="Dynamic" ValidationGroup="AgregarDetalle">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="revCantidad" runat="server" ErrorMessage="Ingrese solo números"
                                        ControlToValidate="txtCantidad" ValidationGroup="AgregarDetalle" Display="Dynamic"
                                        ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                                    <asp:CompareValidator ID="cvCantidad" runat="server" ControlToValidate="txtCantidad" Display="Dynamic" ValidationGroup="AgregarDetalle" 
	                                    ValueToCompare="0" Operator="GreaterThan" ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
	                                <asp:CustomValidator ID="cvCantidadPermitida" runat="server" Display="Dynamic" ValidationGroup="AgregarDetalle" 
	                                    ErrorMessage="Cantidad no permitida, por favor verifique." ClientValidationFunction="validarCantidadEdicion" ></asp:CustomValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Valor Unitario:
                            </td>
                            <td>
                                <asp:TextBox ID="txtValorUnitario" runat="server" MaxLength="18"></asp:TextBox><label
                                    class="comentario">Formato ##.###,##</label>
                                <div>
                                    <asp:RequiredFieldValidator ID="rfvValorUnitario" runat="server" ErrorMessage="Ingrese el valor unitario"
                                        ControlToValidate="txtValorUnitario" Display="Dynamic" ValidationGroup="AgregarDetalle">
                                    </asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="rglValorUnitario" runat="server" 
                                        ErrorMessage="Ingrese el formato de moneda indicado" ControlToValidate="txtValorUnitario" 
                                        ValidationGroup="AgregarDetalle" Display="Dynamic"                                 
                                         ValidationExpression="^(\d{1,3})(\.?\d{3})*(,\d{1,3})*$"></asp:RegularExpressionValidator>    
                                    <asp:CompareValidator ID="cvValorUnitario" runat="server" ControlToValidate="txtValorUnitario" Display="Dynamic" ValidationGroup="AgregarDetalle" 
                                        ValueToCompare="0" Operator="GreaterThan" ErrorMessage="El valor unitario debe ser mayor de 0, por favor verifique."></asp:CompareValidator>                                 
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Observacion:
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

            <div>
                <asp:Button ID="btnCrearDetalleOrden" runat="server" Text="Agregar Detalle" CssClass="boton"
                    ValidationGroup="AgregarDetalle" />
                <asp:Button ID="btnEditarDetalleOrden" runat="server" Text="Editar Detalle" CssClass="boton"
                    ValidationGroup="AgregarDetalle" />
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

        </asp:Panel>
        
       
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    <uc2:ModalProgress ID="ModalProgress1" runat="server" />
    </form>
</body>
</html>
