<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenCompraMerchanPopInsumoP2.aspx.vb" Inherits="BPColSysOP.CrearOrdenCompraMerchanPopInsumoP2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crear Orden Compra</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    $(document).ready(function() {        
        $(":checkbox").click(countChecked);
        $(".error").width($("#gvRemisiones").width());
    });
    function ocultarAdicional() {
        $("#lblDetalleOrdenAdicionado").fadeOut(20000, function() { $("#lblDetalleOrdenAdicionado").html(""); });
    }
    function modificarAltoFramePadre() {
        $("body.cuerpo2").ready(function() {
            $("#frModulo", parent.document).height($("body").height() + 80);
        });
    }    
    function validarChk() {
        var n = $("#gvRemisiones input:checked").length;
        var retorno = false;
        countChecked()
        if (n > 0)
            if (confirm("Esta seguro de continuar con las ordenes de compra seleccionadas"))
                retorno = true;
        return retorno;
    }
    function countChecked() {
        var n = $("#gvRemisiones input:checked").length;
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
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization = "true">
    </asp:ScriptManager>
    <div>
        <uc1:EncabezadoPagina ID="epEncabezado" runat="server" />
        <asp:HiddenField ID="hfIdTipoProducto" runat="server" />
    </div>
    <div class="contenedorOpciones" style="width:900px;">
        <h1>
            <asp:Label ID="lblTipoProducto" runat="server" Text=""></asp:Label>
        </h1>
        <div style="padding-top:10px;">
            <table class="tablaGris" style="float:left;" width="412">
                <tr><th colspan="2">DATOS DE LA ORDEN</th></tr>
                <tr>
                    <td class="field" style="width:100px;"><label>Número de Orden:</label></td>
                    <td style="width:600px;">
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
                    </td>
                </tr>
                <tr>
                    <td class="field"><label>Obervación:</label></td>
                    <td>
                        <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" MaxLength="399"></asp:TextBox>
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
            
            <asp:GridView ID="gvCargaRecepcion" CssClass="tablaGris" runat="server" style="width:100%;" 
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
                    <asp:BoundField DataField="producto" HeaderText="Producto" >
                        <ItemStyle Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="tipoUnidad" HeaderText="Unidad de Empaque" />
                    <asp:TemplateField HeaderText="Cantidad">
                        <ItemTemplate>
                            <asp:TextBox ID="txtCantidad" MaxLength="8" runat="server" Width="120px"></asp:TextBox>
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
                        <ItemStyle Width="120px" />
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Valor Unitario">
                        <ItemTemplate>
                            <asp:TextBox ID="txtValorUnitario" runat="server" Width="200px" MaxLength="18" ></asp:TextBox>  
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
                        <ItemStyle Width="200px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Observación">
                        <ItemTemplate>
                            <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" ValidationGroup="OrdenCompra" MaxLength="399" ></asp:TextBox>
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
            
            <div id="pnlProductoAdicional" runat="server" class="contenedorOpciones" style="width:300px;">
                <h1><label>Producto Adicional</label></h1>
                <asp:GridView ID="gvProductoAdicional" CssClass="tablaGris" 
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
            </div>
            
            <div>                
                <asp:Button ID="btnEnviar" runat="server" Text="Crear Orden Compra" ValidationGroup="OrdenCompra" CssClass="boton2" />            
                <cc1:ConfirmButtonExtender runat="server" ID="cbeCrearOrden" ConfirmText="Esta seguro de continuar con la creación de esta orden de compra ?" TargetControlID="btnEnviar" ></cc1:ConfirmButtonExtender>
            </div>
        </div>
                
    </div>
    <div id="contenedor" runat="server">
        <div>
            <span class="comentario">Remisiones agregadas a esta orden de compra.</span>
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
                </Columns>
                <FooterStyle Font-Bold="True" ForeColor="#FF9900" />
            </asp:GridView>
            <asp:HiddenField ID="hfIdProveedor" runat="server" />
        </div>
        
    </div>
    </form>
</body>
</html>
