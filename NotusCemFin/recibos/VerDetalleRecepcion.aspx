<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="VerDetalleRecepcion.aspx.vb" Inherits="BPColSysOP.VerDetalleRecepcion" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/ModalProgress.ascx" tagname="ModalProgress" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
   <title>Crear Detalle de Recepcion</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <style type="text/css">
    .exito
    {
    	color:Green;
    	font-size:15px;
    }
     .tablaGris
    {
    	width:100%;
    	padding:0 10px;
    	margin:0;
    }
    .float
    {
    	float:left;
    	width:50%;
    }
    #pnlDetalleOrdenRecepcion .both a:hover
    {
    	color:Blue;
    	text-decoration:underline;
    	font-size:9pt;
    	cursor:pointer;
    }
    </style>    
    <script type="text/javascript" language="javascript">
        $(document).ready(init);
        function init() {
        }
        function validarProducto(source,arguments) {
            var idProducto = $("#ddlProducto").val();
            if (idProducto == 0)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }        
    </script> 
</head>
<body class="cuerpo2">
        <form id="form1" runat="server">
    <div id="contenedorPrin" style="width:1000px">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />
        </div>
        
        <div id="pnlDetalleOrdenRecepcion" style="border: solid #000 1px;">
            <p class="subtitulo" >Datos de Orden de Recepción</p>
            
            <table class="float tablaGris">
                <tr>
                    <td width="140px">Tipo de Producto:</td>
                    <td><asp:Label ID="lblTipoProducto" runat="server" /></td>
                </tr>
                <tr>
                    <td>Tipo de Recepción:</td>
                    <td><asp:Label ID="lblTipoRecepcion" runat="server" /></td>
                </tr>
                <tr>
                    <td>Orden de Compra:</td>
                    <td><asp:Label ID="lblNumeroOrdenCompra" runat="server" /></td>
                </tr>
                <tr>
                    <td width="140px">Remisión:</td>
                    <td><asp:Label ID="lblRemision" runat="server" /></td>
                </tr>
                 <tr>
                    <td width="140px">Cantidad:</td>
                    <td><asp:Label ID="lblCantidad" runat="server" /></td>
                </tr>
                <tr>
                    <td>Consignado a:</td>
                    <td><asp:Label ID="lblConsignado" runat="server" /></td>
                </tr>
            </table>
            
            <table class="float tablaGris">
                <tr>
                    <td>Factura:</td>
                    <td><asp:Label ID="lblFactura" runat="server" /></td>
                </tr> 
                <tr>
                    <td>Guia:</td>
                    <td><asp:Label ID="lblGuia" runat="server" /></td>
                </tr>  
                <tr>
                    <td>No Recepción:</td>
                    <td><asp:Label ID="lblNumeroRecepcion" runat="server" /></td>
                </tr> 
                <tr>
                    <td>Fecha de Recepción:</td>
                    <td><asp:Label ID="lblFechaRecepcion" runat="server" /></td>
                </tr>  
                <tr>
                    <td>Destinatario:</td>
                    <td><asp:Label ID="lblDestinatario" runat="server" /></td>
                </tr>                
            </table>
            
            <div style="clear:both;" class="both">
            </div>
        </div>
        
        
                  
                <asp:HiddenField ID="hfFacturaGuia" runat="server" />
                <asp:HiddenField ID="hfOrdenRecepcion" runat="server" />
           

        
        <div id="pnlPalletCreados" class="float">
            <table class="tablaGris">
                <tr>
                    <th>Pallets Adicionados</th>
                </tr>
            </table>
            <asp:GridView ID="gvDetallePallet" runat="server" CssClass="tablaGris" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="idPallet" HeaderText="ID. Pallet" />
                <asp:BoundField DataField="nombreProducto" HeaderText="Producto" />
                <asp:BoundField DataField="cantidad" HeaderText="Cantidad" />
                <asp:BoundField DataField="cantidadRecibida" HeaderText="Cantidad Recibida" />
                <asp:BoundField DataField="unidadEmpaque" HeaderText="Tipo Unidad" />
                <asp:BoundField DataField="idOrdenBodega" HeaderText="Orden de Bodega" />
                <asp:BoundField DataField="peso" HeaderText="Peso" />
                <asp:TemplateField HeaderText="Opciones">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgEditarDetallePallet" runat="server" CommandName="Editar"
                            CommandArgument='<%# Bind("idDetallePallet") %>' ImageUrl="~/images/Edit-32.png"
                            ToolTip="Editar" Visible="false" />
                        <asp:ImageButton ID="imgEliminarDetallePallet" runat="server" CommandName="Eliminar"
                            CommandArgument='<%# Bind("idDetallePallet") %>' ImageUrl="~/images/Delete-32.png"
                            ToolTip="Eliminar" Visible="false"  />
                        <cc1:ConfirmButtonExtender ID="imgEliminarDetallePallet_ConfirmButtonExtender" runat="server"
                            TargetControlID="imgEliminarDetallePallet" ConfirmText="Esta seguro de eliminar este detalle del pallet?">
                        </cc1:ConfirmButtonExtender>
                        <asp:CheckBox ID="ckReImpresion" runat="server" Visible="false" />
                        <asp:ImageButton ID="imgBtnVerNovedades" runat="server" CommandName="verNovedades" CommandArgument='<%# Bind("idPallet") %>' 
                            ImageUrl="~/images/view.png" ToolTip="Ver Novedades" />  
                        <asp:ImageButton ID="imgBtnGenerar" CommandName="imprimirViajera" CommandArgument='<%# Bind("idPallet") %>' runat="server" 
                            ImageUrl="~/images/Pdf.gif" />
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Label CssClass="comentario" runat="server" ID="lblTotalPallet" ></asp:Label>
        </div>
        
        <div style="clear:both"></div>
        
        
        <asp:HiddenField ID="hfValidarCierreNovedades" runat="server" />
        <cc1:ModalPopupExtender ID="mpeMostrarNovedades" runat="server" PopupControlID="pnlMostrarNovedades" BackgroundCssClass="modalBackground"
             TargetControlID="hfValidarCierreNovedades"></cc1:ModalPopupExtender>
    <asp:Panel ID="pnlMostrarNovedades" runat="server" CssClass="modalPopUp" style="width:250px;display:none;">      
        
        <div style="text-align:right"><asp:ImageButton ID="imgBtnCerrarPopUp" runat="server" ImageUrl="~/images/cerrar.gif" /></div>    
        <div class="subtitulo" style="text-align:center;">
            Novedades            
        </div>        
        <div>
            <div style="text-align:justify;padding:5px;">
                <asp:BulletedList ID="bltNovedades" runat="server" DataTextField="novedad">
                </asp:BulletedList>
            </div>
        </div>       
    </asp:Panel>
        
    </div>
            <uc2:ModalProgress ID="ModalProgress1" runat="server" />
        
        </form>
    </body>
</html>
