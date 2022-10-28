<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EditarOrdenCompraMerchanPopInsumo.aspx.vb" Inherits="BPColSysOP.EditarOrdenCompraMerchanPopInsumo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Editar Orden Compra</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    $(document).ready(function() {        
        $(":checkbox").click(countChecked);
        $(".error").width($("#gvAdicionarRecepcion").width());
    });
    function modificarAltoFramePadre() {
        $("body.cuerpo2").ready(function() {
            $("#frModulo", parent.document).height($("body").height() + 80);
        });
    }    
    function validarChk() {
        var n = $("#gvAdicionarRecepcion input:checked").length;
        var retorno = false;
        countChecked()
        if (n > 0)
            if (confirm("Esta seguro de asociar las ordenes de recepcion seleccionadas"))
                retorno = true;
        return retorno;
    }
    function countChecked() {
        var n = $("#gvAdicionarRecepcion input:checked").length;
        var mensaje = $(".error");
        if (n > 0)
            mensaje.hide();
        else
            mensaje.show();
    }
    
    </script>
    <style type="text/css">
    body.cuerpo2
    {
    	background-image:none;
    }
    </style>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <uc1:EncabezadoPagina ID="epEncabezado" runat="server" />
        <asp:HiddenField ID="hfIdOrdenCompra" runat="server" />
        <asp:HiddenField ID="hfIdTipoProducto" runat="server" />
    </div>
    <div class="contenedorOpciones" style="width:950px;">
        <h1>
            <asp:Label ID="lblTipoProducto" runat="server" Text=""></asp:Label>
        </h1>
        <div class="subcontenedor" style="padding-top:10px;">
            <table class="tablaGris" style="float:left;" width="412">
                <tr><th colspan="2">DATOS DE LA ORDEN</th></tr>
                <tr>
                    <td class="field" style="width:100px;"><label>Numero Orden:</label></td>
                    <td style="width:600px;">
                        <asp:TextBox ID="txtNumeroOrden" runat="server" MaxLength="15"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvNumeroOrden" runat="server" ControlToValidate="txtNumeroOrden"
                                Display="Dynamic" CssClass="bloque" ErrorMessage="Digite el número de la orden, por favor"
                                ValidationGroup="OrdenCompra"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field"><label>Moneda:</label></td>
                    <td>
                        <asp:DropDownList ID="ddlMoneda" runat="server">
                        </asp:DropDownList>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvMoneda" runat="server" ControlToValidate="ddlMoneda"
                                Display="Dynamic" InitialValue="0" CssClass="bloque" ValidationGroup="OrdenCompra"
                                ErrorMessage="Seleccione una moneda, por favor"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field"><label>Incoterm:</label></td>
                    <td>
                        <asp:DropDownList ID="ddlIncoterm" runat="server">
                        </asp:DropDownList>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvIncoterm" runat="server" ControlToValidate="ddlIncoterm"
                                InitialValue="0" CssClass="bloque" ValidationGroup="OrdenCompra" ErrorMessage="Seleccione un termino  de negociación (Incoterm), por favor"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="field"><label>Obervación:</label></td>
                    <td>
                        <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" ValidationGroup="OrdenCompra" MaxLength="399"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="revObservacion" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                Display="Dynamic" ControlToValidate="txtObservacion" ValidationGroup="OrdenCompra" ErrorMessage="La observación contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                        </div>
                    </td>
                </tr>
                
            </table>            
            <asp:GridView ID="gvRegion" runat="server" AutoGenerateColumns="False" style="float:left;margin-left:10px;width:255px;" CssClass="tablaGris">
                            <FooterStyle CssClass="thGris" />
                            <Columns>
                                <asp:BoundField DataField="nombreRegion" HeaderText="Región" 
                                    ItemStyle-CssClass="field" >
                                    <ItemStyle CssClass="field" Width="80px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Cantidad">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCantidadRegion" MaxLength="8" runat="server" onkeyup="CalcularTotalDistribucion();"></asp:TextBox>
                                        <div>
                                            <asp:RegularExpressionValidator ID="revCantidadRegion" runat="server" ErrorMessage="El campo cantidad es numérico. Digite un número válido"
                                                ControlToValidate="txtCantidadRegion" Display="Dynamic" ValidationExpression="(\s+)?(\d+)(\s+)?" ValidationGroup="OrdenCompra">
                                            </asp:RegularExpressionValidator>
                                            <asp:CompareValidator ID="cvCantidadRegion" runat="server" ControlToValidate="txtCantidadRegion" Display="Dynamic" ValidationGroup="OrdenCompra" 
	                                            ValueToCompare="0" Operator="GreaterThan" ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle Width="145px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="idRegion" HeaderText="ID Region" Visible="false" />
                            </Columns>
                        </asp:GridView>
             
            <div style="clear:both;"></div>
            <asp:GridView ID="gvCargaRecepcion" CssClass="tablaGris" runat="server" style="width:900px;" 
                AutoGenerateColumns="False">
                <Columns>
                    <asp:TemplateField HeaderText="Fabricante">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlFabricante" runat="server">
                            </asp:DropDownList>
                            <div>
                                <asp:RequiredFieldValidator ID="rfvFabricante" runat="server" ControlToValidate="ddlFabricante"
                                    Display="Dynamic" InitialValue="0" CssClass="bloque" ValidationGroup="OrdenCompra"
                                    ErrorMessage="Seleccione un fabricante, por favor"></asp:RequiredFieldValidator>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                    <asp:BoundField DataField="tipoUnidad" HeaderText="Unidad de Empaque" />
                    <asp:TemplateField HeaderText="Cantidad">
                        <ItemTemplate>
                            <asp:TextBox ID="txtCantidad" MaxLength="8" runat="server"></asp:TextBox>
                            <div>
                                <asp:Label ID="lblCantidadRegistrada" runat="server" Text=""></asp:Label>
                            </div>
                            <div>
                            <asp:RegularExpressionValidator ID="rglCantidad" runat="server" ErrorMessage="El campo cantidad es numérico. Digite un número válido, por favor"
                                ControlToValidate="txtCantidad" ValidationGroup="OrdenCompra" Display="Dynamic"
                                ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" ValidationGroup="OrdenCompra"
                                ControlToValidate="txtCantidad" ErrorMessage="Digite el valor del campo cantidad, por favor"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvCantidad" runat="server" ControlToValidate="txtCantidad" Display="Dynamic" ValidationGroup="OrdenCompra" 
	                            ValueToCompare="0" Operator="GreaterThan" ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Valor Unitario">
                        <ItemTemplate>
                            <asp:TextBox ID="txtValorUnitario" MaxLength="18" runat="server" ></asp:TextBox>  
                            <div>
                                <label class="comentario">Formato ##.###,##</label>
                            </div>                          
                            <div>
                            <asp:RegularExpressionValidator ID="rglValorUnitario" runat="server" 
                                ErrorMessage="Ingrese el formato de moneda indicado" ControlToValidate="txtValorUnitario" 
                                ValidationGroup="OrdenCompra" Display="Dynamic"                                 
                                 ValidationExpression="^(\d{1,3})(\.?\d{3})*(,\d{1,3})*$"></asp:RegularExpressionValidator>                            
                            <asp:RequiredFieldValidator ID="rfvValorUnitario" runat="server" ControlToValidate="txtValorUnitario"
                                Display="Dynamic" CssClass="bloque" ErrorMessage="Digite el valor unitario, por favor"
                                ValidationGroup="OrdenCompra"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvValorUnitario" runat="server" ControlToValidate="txtValorUnitario" Display="Dynamic" ValidationGroup="OrdenCompra" 
                                ValueToCompare="0" Operator="GreaterThan" ErrorMessage="El valor unitario debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Observación">
                        <ItemTemplate>
                            <asp:TextBox ID="txtObservacion" MaxLength="399" runat="server" TextMode="MultiLine" ValidationGroup="OrdenCompra"></asp:TextBox>
                            <div>
                                <asp:RegularExpressionValidator ID="revObservacion" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                    Display="Dynamic" ControlToValidate="txtObservacion" ValidationGroup="OrdenCompra" ErrorMessage="La observación contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                            </div>
                            <asp:HiddenField ID="hfIdProducto" runat="server" />
                            <asp:HiddenField ID="hfIdTipoUnidad" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            
            <div>                
     
    <div class="contenedorOpciones" runat="server" id="pnlProductoAdicionalContenedor" style="width:300px;">
        <h1>
            <asp:Label ID="lblTituloProductoAdicional" runat="server" Text="Producto Adicional"></asp:Label>
        </h1>    
    <asp:Panel ID="pnlProductoAdicional" runat="server">
        <asp:GridView ID="gvCargaRecepcionAdicional" CssClass="tablaGris" 
    runat="server" style="width:100%;" 
                AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="producto" HeaderText="Producto" />
                <asp:TemplateField HeaderText="Cantidad">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCantidadAdicional" MaxLength="8" runat="server"></asp:TextBox>
                        <asp:HiddenField ID="hfIdProductoAdicional" runat="server" Value='<%#Bind("idProducto") %>' />
                        <div>
                            <asp:Label ID="lblCantidadRegistradaAdicional" runat="server" Text=""></asp:Label>
                        </div>
                        <div>
                            <div><asp:RegularExpressionValidator ID="rglCantidadAdicional" runat="server" ErrorMessage="El campo cantidad es numérico. Digite un número válido, por favor"
                                ControlToValidate="txtCantidadAdicional" ValidationGroup="OrdenCompra" Display="Dynamic"
                                ValidationExpression="[0-9]+"></asp:RegularExpressionValidator></div>
                            <div><asp:RequiredFieldValidator ID="rfvCantidadAdicional" runat="server" ValidationGroup="OrdenCompra"
                                ControlToValidate="txtCantidadAdicional" ErrorMessage="Digite el valor del campo cantidad, por favor"
                                Display="Dynamic"></asp:RequiredFieldValidator></div>
                            <div>
                                <asp:CompareValidator ID="cvCantidadAdicional" runat="server" ControlToValidate="txtCantidadAdicional" Display="Dynamic" ValidationGroup="OrdenCompra" 
	                            ValueToCompare="0" Operator="GreaterThan" ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        
        </asp:Panel>    
    </div>
    
                <asp:Button ID="btnEnviar" runat="server" Text="Editar Orden Compra" 
                    ValidationGroup="OrdenCompra" CssClass="boton2" />            
            </div>
        </div>
        
        
    </div>
    <div id="contenedor" runat="server">
        <div>
            <span class="comentario">Remisiones agregadas a esta orden de compra.</span>
            <asp:ImageButton ID="IbtnAsociarRecepcion" runat="server" 
                ImageUrl="~/images/add.png" ToolTip="Asociar nueva orden de recepción" />
        </div>
        <div>
            <asp:GridView ID="gvRemisiones" CssClass="tablaGris" runat="server" style="min-width:700px;border:2px solid silver;"
                AutoGenerateColumns="False" EmptyDataText="No existen remisiones pendientes.">
                <Columns>                    
                    <asp:BoundField DataField="remision" HeaderText="No. Remisión" >
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="idOrdenRecepcion" HeaderText="No. Orden Recepción" >
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Producto(s)">
                        <ItemTemplate>
                            <asp:BulletedList ID="bltProductosAgregados" DataTextField="nombreProducto" style="margin:4px 0;" runat="server">
                            </asp:BulletedList>                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pallet(s)">
                        <ItemTemplate>
                            <asp:BulletedList ID="bltPallet" DataTextField="palletPeso" runat="server">
                            </asp:BulletedList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                    <asp:TemplateField HeaderText="Opciones">
                        <ItemTemplate>
                            <asp:ImageButton ID="IbtnRemoverRecepcion" runat="server" CommandArgument='<%# Bind("idOrdenRecepcion") %>' CommandName="DesvincularRemision" 
                                ImageUrl="~/images/remove.png" ToolTip="Remover de la Orden de Compra" />
                            <cc1:ConfirmButtonExtender runat="server" ID="cbeAnularOrden" ConfirmText="Esta seguro de desvincular esta remisión de la orden de compra.?\n Esta operación desvincula los productos de la remisión en la orden de compra." TargetControlID="IbtnRemoverRecepcion" ></cc1:ConfirmButtonExtender>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle Font-Bold="True" ForeColor="#FF9900" />
            </asp:GridView>
            <asp:HiddenField ID="hfIdProveedor" runat="server" />
        </div>
        
    </div>
    <cc1:ModalPopupExtender ID="mpeAgregarRecepcion" PopupControlID="pnlAgregarRecepcion" BackgroundCssClass="modalBackground" TargetControlID="hfValidarCierre" runat="server">
    </cc1:ModalPopupExtender>
    <asp:HiddenField ID="hfValidarCierre" runat="server" />
     
    <asp:Panel ID="pnlAgregarRecepcion" runat="server" CssClass="modalPopUp" style="width:700px;" >
        <div style="text-align:right"><asp:ImageButton ID="imgBtnCerrarPopUp" runat="server" ImageUrl="~/images/cerrar.gif" /></div>
        <div id="pnlMensajeRecibos" runat="server"><span class="comentario">Nota: Seleccione las remisiones que se van a agregar a la orden de compra. </span></div>
        <asp:GridView ID="gvAdicionarRecepcion" CssClass="tablaGris" runat="server" style="border:2px solid silver; width:100%;"
                AutoGenerateColumns="False" EmptyDataText="No existen remisiones pendientes.">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccione">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRemision" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="remision" HeaderText="No. Remisión" >
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="idOrdenRecepcion" HeaderText="No. Orden Recepción" >
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Producto(s)">
                        <ItemTemplate>
                            <asp:BulletedList ID="bltProductosAgregados" DataTextField="nombreProducto" style="margin:4px 0;" runat="server">
                            </asp:BulletedList>                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                </Columns>
                <FooterStyle Font-Bold="True" ForeColor="#FF9900" />
            </asp:GridView>
            <div>
            <div class="error" style="display:none;">Por favor, seleccione al menos una remisión.</div>
            <asp:Button ID="btnAgregarRecepcion" runat="server" Text="Asociar" 
                OnClientClick="return validarChk()" CssClass="boton" />            
            </div>
    </asp:Panel>
    
    
     <div>
            <asp:HiddenField ID="hfModaPopUpProductoAdicional" runat="server" />
            <cc1:ModalPopupExtender ID="mpeAgregarProductoAdcional" TargetControlID="hfModaPopUpProductoAdicional" BackgroundCssClass="modalBackground"
                PopupControlID="pnlEditarAgregarProductoAdicional" runat="server" CancelControlID="imbBtnCerrarProductoAdicional">
            </cc1:ModalPopupExtender>
        </div>         
    
    </form>
</body>
</html>
