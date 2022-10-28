<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenRecepcionMerchanPopInsumo.aspx.vb" Inherits="BPColSysOP.CrearOrdenRecepcionMerchanPopInsumo" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/ModalProgress.ascx" tagname="ModalProgress" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crear Orden de Recepcion</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function modificarAltoFramePadre() {
            $("body.cuerpo2").ready(function() {
                $("#frModulo", parent.document).height($("body").height() + 130);              
            });
            
        }
        function RefrescaUpdatePaneOrden() {
            var filtro = $get("txtFiltroOrden").value;
            var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\s áéíóúÁÉÍÓÚ]+\s*$");
            if (patron.test(filtro)) {
                if (filtro.length > 3) {
                    $get("hfFlagFiltradoOrden").value = 1;
                    __doPostBack('txtFiltroOrden', '');
                    $find(ModalProgress).hide();
                }
                else if (filtro.length <= 3 && $get("hfFlagFiltradoOrden").value == 1) {
                $get("hfFlagFiltradoOrden").value = 0
                    __doPostBack('txtFiltroOrden', '');
                    $find(ModalProgress).hide();
                }
            }
            else if ($get("txtFiltroOrden").value != "") { alert("los caracteres especiales no son permitidos") }
        }
        function RefrescaUpdatePaneProveedor() {
            var filtro = $get("txtFiltroProveedor").value;
            var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\s áéíóúÁÉÍÓÚ]+\s*$");
            if (patron.test(filtro)) {
                if (filtro.length > 3) {
                    $get("hfFlagFiltradoProveedor").value = 1;
                    __doPostBack('txtFiltroProveedor', '');
                    $find(ModalProgress).hide();
                }
                else if (filtro.length <= 3 && $get("hfFlagFiltradoProveedor").value == 1) {
                $get("hfFlagFiltradoProveedor").value = 0
                    __doPostBack('txtFiltroProveedor', '');
                    $find(ModalProgress).hide();
                }
            }
            else if ($get("txtFiltroProveedor").value != "") { alert("los caracteres especiales no son permitidos") }
        }
        
    </script> 
    <style type="text/css">
    body.cuerpo2
    {
    	background-image:none;
    }
    .contador
    {
    	padding-left:150px;
    }
    </style>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width:700px;">    
        <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />    
    </div>
    <asp:UpdatePanel ID="upCrearOrdenRecepcion" runat="server">
    <ContentTemplate>
     <table class="tablaGris" width="600px">
            <tr>
                <th colspan="2" align="center">Datos de la Recepción</th>
            </tr>            
            <tr>
                <td>Tipo de Recepción:</td>
                <td>                            
                    <asp:DropDownList ID="ddlTipoRecepcion" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                    <div>
                    <asp:RequiredFieldValidator ID="rfvTipoRecepcion" runat="server" ControlToValidate="ddlTipoRecepcion" InitialValue="0"
                    ErrorMessage="Seleccione un tipo de recepción" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr id="trConsignatario" runat="server">
                 <td>
                     Consignado a:
                 </td>
                 <td>
                     <asp:DropDownList ID="ddlConsignado" runat="server">
                     </asp:DropDownList>
                     <div>
                         <asp:RequiredFieldValidator ID="rfvConsignado" runat="server" ControlToValidate="ddlConsignado"
                             InitialValue="0" ErrorMessage="Seleccione el consignado a" Display="Dynamic"></asp:RequiredFieldValidator>
                     </div>
                 </td>
             </tr>
            <tr>
                <td>Orden de Compra:</td>
                <td>
                <asp:HiddenField ID="hfFlagFiltradoOrden" runat="server" />  
                <span class="comentario" style="padding-left:0;">Ingrese Número o Identificador</span><br />      
                <asp:TextBox ID="txtFiltroOrden" MaxLength="15" runat="server" onkeyup="RefrescaUpdatePaneOrden();"
                            OnTextChanged="FiltrarOrden"></asp:TextBox> -   
                 <asp:UpdatePanel ID="upFiltroOrdenNumero" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        
                        <asp:DropDownList ID="ddlOrdenCompra" runat="server" AutoPostBack="true">
                        </asp:DropDownList>
                        <div class="contador"><asp:Label ID="lblNumOrdenesCompra" runat="server" CssClass="comentario"></asp:Label></div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="txtFiltroOrden" />
                    </Triggers>
                </asp:UpdatePanel>                  
                                                        
                                                                                                              
                    
                </td>
            </tr>            
            <tr>
                <td>Proveedor:</td>
                <td>
                <asp:HiddenField ID="hfFlagFiltradoProveedor" runat="server" />        
                <asp:TextBox ID="txtFiltroProveedor" MaxLength="15" runat="server" 
                        onkeyup="RefrescaUpdatePaneProveedor();"></asp:TextBox> -                                             
                                <asp:DropDownList ID="ddlProveedor" runat="server">
                                </asp:DropDownList>
                                <div class="contador"><asp:Label ID="lblNumProveedores" runat="server" CssClass="comentario"></asp:Label></div>                            
                    <div><asp:RequiredFieldValidator ID="rfvProveedor" Display="Dynamic" runat="server" ControlToValidate="ddlProveedor" InitialValue="0" ErrorMessage="Por favor seleccione el proveedor."></asp:RequiredFieldValidator></div>
                </td>
            </tr>   
            <tr>
                <td>Factura:</td>
                <td>
                    <asp:TextBox ID="txtFactura" MaxLength="15" runat="server"></asp:TextBox>
                    <div>
                        <asp:RegularExpressionValidator ID="revFactura" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                            Display="Dynamic" ControlToValidate="txtFactura" ErrorMessage="La factura contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr> 
            <tr>
                <td>Guía:</td>
                <td>
                    <asp:TextBox ID="txtGuia" MaxLength="15" runat="server"></asp:TextBox>
                    <div>
                        <asp:RegularExpressionValidator ID="revGuia" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                            Display="Dynamic" ControlToValidate="txtGuia" ErrorMessage="La guía contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>                     
            <tr>
                <td>Remisión:</td>
                <td>
                    <asp:TextBox ID="txtRemision" runat="server" MaxLength="15"></asp:TextBox>
                    <div>
                        <asp:RegularExpressionValidator ID="revRemision" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                            Display="Dynamic" ControlToValidate="txtRemision" ErrorMessage="El número de remisión contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator
                            ID="rqfRemision" runat="server" ErrorMessage="Por favor ingrese el número de remisión." ControlToValidate="txtRemision" Display="Dynamic"></asp:RequiredFieldValidator>                        
                    </div>                             
                </td>
            </tr>
            <tr>
            <td>Destinatario:</td>                                
            <td>                                
                <asp:DropDownList ID="ddlClienteExterno" runat="server">
                </asp:DropDownList>
                <div>
                <asp:RequiredFieldValidator ID="rfvClienteExterno" runat="server" ControlToValidate="ddlClienteExterno" InitialValue="0"
                ErrorMessage="Seleccione el destinatario" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>                
             </td>
        </tr>                         
        </table>
      </ContentTemplate>
       <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtFiltroOrden" />
            <asp:AsyncPostBackTrigger ControlID="txtFiltroProveedor" />
       </Triggers>
      </asp:UpdatePanel>
        <div>
            <asp:HiddenField ID="hfIdTipoProducto" runat="server" />
            <asp:Button ID="btnBuscar" runat="server" Text="Crear Orden de Recepción" CssClass="boton" />
        </div>
    <uc2:ModalProgress ID="ModalProgress1" runat="server" />
    </form>
</body>
</html>
