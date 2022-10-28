<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VerDetalleOrdenCompra.aspx.vb" Inherits="BPColSysOP.VerDetalleOrdenCompra" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

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
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>    
        <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />    
    </div>
    <div>
        <div id="pnlInfoOrdenCompra" style="width:800px;">
            <p class="subtitulo">Orden de Compra</p>
            <table class="tablaGris">
                <tr>
                    <td style="width:40%">Numero de Orden:</td>
                    <td><asp:Label ID="lblNumeroOrden" runat="server" Text="" /></td>
                </tr>
                <tr>
                    <td>Proveedor:</td>
                    <td><asp:Label ID="lblProveedor" runat="server" Text="" /></td>
                </tr>
                <tr>
                    <td>Moneda:</td>
                    <td><asp:Label ID="lblMoneda" runat="server" Text="" /></td>
                </tr>
            </table>
            <table class="tablaGris">
                <tr>
                    <td style="width:40%">Incoterm:</td>
                    <td><asp:Label ID="lblIncoterm" runat="server" Text="" /></td>
                </tr>
                <tr>
                    <td>Observación:</td>
                    <td><asp:Label ID="lblObservacion" runat="server" Text="" /></td>
                </tr>                
            </table>
            <div style="clear:both;"></div>
        </div>
              <!-- **************Bloque para ingresar el detalle de la orden de compra***************-->        
        <asp:Panel ID="pnlAdicionarDetalleOrdenCompra" runat="server">
            <table class="tablaGris" width="800px">
            <tr>
                <th colspan="2" align="center">Detalles Agregados a la Orden de Compra</th>
            </tr>            
            <tr>
                <td>Detalles de la orden de compra:</td>
                <td>
                    <asp:Panel ID="pnlListarDetalleOrdenCompra" runat="server">
            <asp:GridView ID="gvDetalleOrdenCompra" runat="server" CssClass="tablaGris" Width="100%"
                AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="fabricante" HeaderText="Fabricante" />
                    <asp:BoundField DataField="producto" HeaderText="Producto" />
                    <asp:BoundField DataField="TipoUnidad" HeaderText="Tipo de Unidad" />
                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                    <asp:BoundField DataField="valorUnitario" HeaderText="Valor Unitario" />
                    <asp:BoundField DataField="observacion" HeaderText="Observacion" />                    
                </Columns>
            </asp:GridView>
        </asp:Panel>
                </td>
            </tr>
            </table>
            <div>
                <asp:HiddenField ID="hfIdDetalle" runat="server" />
                <asp:HiddenField ID="hfIdOrdenCompra" runat="server" />
            </div>
        </asp:Panel>
        <!-- **************Fin Bloque para ingresar el detalle de la orden de compra***************-->
        
        <!-- **************Listado de Detalle Agregados a la orden de compra actual****************-->
        
        <!-- **************Fin Listado de Detalle Agregados a la orden de compra actual****************-->

    </div>
    </form>
</body>
</html>
