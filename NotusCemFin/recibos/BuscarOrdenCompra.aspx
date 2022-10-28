<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BuscarOrdenCompra.aspx.vb" Inherits="BPColSysOP.BuscarOrdenCompra" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/MensajeModal.ascx" tagname="MensajeModal" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Busqueda Orden de Compra</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <style type="text/css" >
    #grdOrden ul
    {
    	margin:0;
    	padding:5px 5px 5px 15px
    }
    .DetalleOrden
    {    	
    	padding:5px;
    	background-color:Silver;    	
    }
    .DetalleOrden div table
    {
    	width:100%;
    }
    .info
    {
    	font-weight:bold;
    	font-size:14px;
    	padding:15px 5px;
    	display:block;
    }
    .fecha
    {
    	width:80px;
    }
    </style>
    <script type="text/javascript" language="javascript">
        $(document).ready(init);
        function init() {
            $("#txtFechaInicial,#txtFechaFinal").css({ 'width': '80px' });
            $(".DetalleOrden").hide();
            $(".mostrarDetalle").css({ 'cursor': 'pointer' });
            $(".mostrarDetalle").toggle(function() {
                var tabla = $(this).parents("tr").next("tr").find(".DetalleOrden");
                tabla.show();
                $(this).attr('src', '../images/remove.png');
            }, function() {
                var tabla = $(this).parents("tr").next("tr").find(".DetalleOrden");
                tabla.hide();
                $(this).attr('src', '../images/add.png');
            }
            );
        }
        function entra() {
            console.log("entra");
        }
        function sale() {
            console.log("sale");
        }
        function validarVacios(source, arguments) {
            try {
                var idOrden = $("#txtIdOrden").val();
                var numeroOrden = $("#txtNumeroOrden").val();
                var idTipoProducto = $("#ddlTipoProducto").val();
                var idProveedor = $("#ddlProveedor").val();
                var idMoneda = $("#ddlMoneda").val();
                var idIncoterm = $("#ddlIncoterm").val();
                var idEstado = $("#ddlEstado").val();
                var fechaInicial = $("#txtFechaInicial").val();
                var fechaFinal = $("#txtFechaFinal").val();
                if (idOrden =="" && numeroOrden == "" && idTipoProducto == "0" && idProveedor == "0" && idMoneda == "0" && idIncoterm == "0" && idEstado == "0" && fechaInicial == "" && fechaFinal == "") {
                    arguments.IsValid = false;
                } else {
                    arguments.IsValid = true;
                }
            } catch (e) {
                arguments.IsValid = false;
            }
        }
        function showConfirmation() {
            return (confirm("Está seguro de que desea cancelar la orden indicada?"));
        }
        function esRangoValido(source, arguments) {
            try {
                if (document.getElementById("txtFechaInicial").value.trim() != "" || document.getElementById("txtFechaFinal").value.trim() != "") {
                    if (document.getElementById("txtFechaInicial").value.trim() != "" && document.getElementById("txtFechaFinal").value.trim() == "") {
                        arguments.IsValid = false;
                    } else {
                        if (document.getElementById("txtFechaInicial").value.trim() == "" && document.getElementById("txtFechaFinal").value.trim() != "") {
                            arguments.IsValid = false;
                        } else {
                            arguments.IsValid = true;
                        }
                    }
                } else {
                    arguments.IsValid = true;
                }
            } catch (e) {
                arguments.IsValid = false;
            }
        }
        function AnularOrden(obj) {            
            var validaciones = "";
            validaciones = obj.parent("td").find('input[id*="hfAnularOrden"]').val();
            if (validaciones == ""){
                return confirm('Realmente desea anular la orden seleccionada?');
            } else {
                var mensajes = validaciones.split("|");
                var mensaje;                   
                mensaje = "La prealerta se encuentra con:\n\n";
                $.each(mensajes, function(key,value) {
                    mensaje += value + "\n";
                });
                alert(mensaje);
                return false;
            }
        } 
    </script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true">
    </asp:ScriptManager>
    <div>
    
        <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />
    
    </div>

    <table class="tablaGris" width="600px">
            <tr>
                <th colspan="2" align="center">Buscar Orden de Compra</th>
            </tr>
            <tr>
                <td style="width:40%;">Identificador de la Orden de Compra:</td>
                <td>
                    <asp:TextBox ID="txtIdOrden" runat="server" MaxLength="15"></asp:TextBox>    
                    <div>
                        <asp:RegularExpressionValidator ID="rglIdOrden" runat="server" ErrorMessage="El campo identificador es numérico. Digite un número válido, por favor"
                            ControlToValidate="txtIdOrden" Display="Dynamic"
                            ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width:40%;">Número de la Orden de Compra:</td>
                <td>
                    <asp:TextBox ID="txtNumeroOrden" runat="server" MaxLength="15"></asp:TextBox>   
                    <div>
                        <asp:RegularExpressionValidator ID="revNumeroOrden" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                            Display="Dynamic" ControlToValidate="txtNumeroOrden" ErrorMessage="El número de orden contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                    </div>                 
                </td>
            </tr>
            <tr>
                <td>Tipo de Producto:</td>
                <td>
                    <asp:DropDownList ID="ddlTipoProducto" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Proveedor:</td>
                <td>
                    <asp:DropDownList ID="ddlProveedor" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Moneda:</td>
                <td>
                    <asp:DropDownList ID="ddlMoneda" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Incoterm:</td>
                <td>
                    <asp:DropDownList ID="ddlIncoterm" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>Estado:</td>
                <td>
                    <asp:DropDownList ID="ddlEstado" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>   
             <tr>
                <td>Fecha:</td>
                <td>
                    <asp:TextBox ID="txtFechaInicial" runat="server" CssClass="fecha" 
                        MaxLength="10"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFechaInicial_CalendarExtender" runat="server" PopupButtonID="imgFechaIni" Format="dd/MM/yyyy" CssClass="calendarTheme"
                        TargetControlID="txtFechaInicial">
                    </cc1:CalendarExtender>
                    <img src="../images/date-32.png" id="imgFechaIni" alt="Fecha Inicial" title="Fecha Inicial" />&nbsp;&nbsp;&nbsp;
                    <asp:TextBox ID="txtFechaFinal" runat="server" CssClass="fecha" MaxLength="10"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFechaFinal_CalendarExtender" runat="server" PopupButtonID="imgFechaFinal" Format="dd/MM/yyyy" CssClass="calendarTheme"
                        TargetControlID="txtFechaFinal">
                    </cc1:CalendarExtender>
                    <img src="../images/date-32.png" id="imgFechaFinal" alt="Fecha Final" title="Fecha Final" />
                    <div>
                        <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaInicial" runat="server" ErrorMessage="Fecha inicial no válida."
                                ControlToValidate="txtFechaInicial" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaFinal" runat="server" ErrorMessage="Fecha final no válida."
                                ControlToValidate="txtFechaFinal" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>                                                
                        <asp:CompareValidator ID="cvRangoFecha" runat="server" ControlToCompare="txtFechaInicial"
                                ControlToValidate="txtFechaFinal" ErrorMessage="La Fecha Final debe ser mayor o igual a la Fecha Inicial"
                                Operator="GreaterThanEqual" Type="Date" Display="Dynamic" ValidationGroup="buscarOrdenCompra"></asp:CompareValidator>
                        <asp:CustomValidator ID="cusRango" runat="server" ErrorMessage="Es necesario especificar los dos valores del Rango"
                                Display="Dynamic" ClientValidationFunction="esRangoValido" ValidationGroup="buscarOrdenCompra"></asp:CustomValidator>
                    </div>
                </td>
            </tr>          
        </table>

        <div>
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar Orden" CssClass="boton" ValidationGroup="buscarOrdenCompra" />
            <asp:Button ID="btnBorrarFiltros" runat="server" Text="Limpiar Filtros" CssClass="boton" />
            <asp:CustomValidator ID="cvValidarVacios" runat="server" 
                ErrorMessage="Seleccione un filtro de búsqueda" ClientValidationFunction="validarVacios" ValidationGroup="buscarOrdenCompra"></asp:CustomValidator>
        </div>
        <div>
        </div>
        
        <div style="padding-top:20px;">
            <asp:GridView ID="grdOrden" runat="server" CssClass="tablaGris" 
                AutoGenerateColumns="False" EmptyDataText="Filtro de búsqueda sin datos." 
                ShowFooter="True" style="width:1000px;">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Image ID="imgShow" runat="server" ImageUrl="~/images/add.png" 
                                CssClass="mostrarDetalle" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="idOrden" HeaderText="Identificador" />
                    <asp:BoundField DataField="numeroOrden" HeaderText="Numero Orden" />
                    <asp:BoundField DataField="tipoProducto" HeaderText="Tipo Producto" />
                    <asp:BoundField DataField="proveedor" HeaderText="Proveedor" />
                    <asp:BoundField DataField="moneda" HeaderText="Moneda" />
                    <asp:BoundField DataField="incoterm" HeaderText="Incoterm" />
                    <asp:TemplateField HeaderText="Dis. por Región">
                        <ItemTemplate>
                            <asp:BulletedList ID="bltDisPorRegion" runat="server">
                            </asp:BulletedList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="fechaCreacion" HeaderText="Fecha Creación" >
                        <ItemStyle Width="150px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="estado" HeaderText="Estado" />
                    <asp:BoundField DataField="observacion" HeaderText="Observación" >
                        <ItemStyle Width="200px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Opciones">
                        <ItemTemplate>                            
                            <asp:ImageButton ID="imgEditarOrdenCompra" runat="server" 
                                CommandName="imgEditarOrdenCompra"
                                ImageUrl="~/images/view.png" ToolTip="Administrar Detalle" Visible="false" />                            
                            <asp:ImageButton ID="imgAnularOrden" runat="server" 
                                CommandName="AnularOrdenCompra" CommandArgument='<%# Bind("idOrden") %>'
                                ImageUrl="~/images/remove.png" ToolTip="Anular Orden" Visible="false" OnClientClick="return AnularOrden($(this));" />                            
                            <asp:HiddenField ID="hfAnularOrdenMsn" runat="server" />
                            <asp:ImageButton ID="imgActivarOrden" runat="server" 
                                CommandName="ActivarOrdenCompra" CommandArgument='<%# Bind("idOrden") %>'
                                ImageUrl="~/images/cancelar.png" ToolTip="Activar Orden" Visible="false" OnClientClick="return confirm('Realmente desea activar la orden seleccionada?');" />                            
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    <cc1:ModalPopupExtender ID="imgEditarOrdenCompra_ModalPopupExtender" BackgroundCssClass="modalBackground"
                                PopupControlID="pnlEditOrdenCompra" runat="server" TargetControlID="hfValidarCierre" >
                            </cc1:ModalPopupExtender> 
    <asp:HiddenField ID="hfValidarCierre" runat="server" />    
    
    <asp:Panel ID="pnlEditOrdenCompra" runat="server" CssClass="modalPopUp" style="width:500px;display:none;">
    <div style="text-align:right"><asp:ImageButton ID="imgBtnCerrarPopUp" runat="server" ImageUrl="~/images/cerrar.gif" /></div>
    <table class="tablaGris" width="500">
        <tr>
            <th colspan="2">
                <div>
                    Editar Orden No. 
                <asp:Label ID="lblEditarOrdenNo" runat="server" Text=""></asp:Label></div>                
            </th>
            
        </tr>
        <tr>
            <td>Proveedor:</td>
            <td>
                <asp:DropDownList ID="ddlEditarProveedorOrden" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Moneda:</td>
            <td>
                <asp:DropDownList ID="ddlEditarMonedaOrden" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>Incoterm:</td>
            <td>
                <asp:DropDownList ID="ddlEditarIncotermOrden" runat="server">
                </asp:DropDownList>
            </td>
        </tr>        
        <tr>
            <td>Observación:</td>
            <td>
                <asp:TextBox ID="txtEditarObservacionOrden" runat="server" TextMode="MultiLine" 
                    Height="54px" Width="100%" MaxLength="400"></asp:TextBox>
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
                                <asp:BoundField DataField="nombreRegion" HeaderText="Región" 
                                    ItemStyle-CssClass="field" >
                                    <ItemStyle CssClass="field" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Cantidad">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCantidadRegion" runat="server" onkeyup="CalcularTotalDistribucion();"></asp:TextBox>
                                        <div style="display: block">
                                            <asp:RegularExpressionValidator ID="revCantidadRegion" runat="server" ErrorMessage="El campo cantidad es numérico. Digite un número válido"
                                                ControlToValidate="txtCantidadRegion" Display="Dynamic" ValidationExpression="(\s+)?(\d+)(\s+)?" ValidationGroup="OrdenCompra">
                                            </asp:RegularExpressionValidator>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="idRegion" HeaderText="ID Region" Visible="false" />
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
                <asp:Button ID="btnEditarOrdenCompra" CssClass="boton" runat="server" Text="Editar" />
            </td>
        </tr>
    </table>
        <asp:HiddenField ID="hfIdOrdenEditar" runat="server" />
    </asp:Panel>
    
    <uc2:MensajeModal ID="mmInfo" runat="server" />
    
    </form>
</body>
</html>
