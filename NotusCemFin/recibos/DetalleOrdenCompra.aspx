<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetalleOrdenCompra.aspx.vb"
    Inherits="BPColSysOP.DetalleOrdenCompra" %>

<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/Loader.ascx" TagName="Loader" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Detalle Orden de Compra</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />

    <script src="../include/jquery-1.js" type="text/javascript"></script>

    <style type="text/css">
        .DetalleFactura
        {
            padding: 5px;
            background-color: Silver;
        }
        .DetalleFactura div table
        {
            width: 100%;
        }
        #pnlInfoDetalleOrdenCompra .tablaGris
        {
            float: left;
            width: 50%;
        }
        #pnlInfoOrdenCompra .tablaGris
        {
            float: left;
            width: 50%;
        }
        #txtFechaSalida_CalendarExtender_popupDiv, #txtFechaEsperadaArribo_CalendarExtender_popupDiv
        {
            left: 326px !important;
            top: 108px !important;
        }
    </style>

    <script type="text/javascript" language="javascript">
        function mostrar() {
            $(".calendarTheme").css({ "z-index": "2000000" });
        }
        $(document).ready(init);
        function init() {
            $("#pnlAdicionarDetalleOrdenCompra").css({ "padding-left": "30px" });
            $("#hlkAdicionarFactura").click(function() {
                $("#pnlDetalleOrdenCompra").slideToggle("slow");
            });

            $(".DetalleFactura").hide();
            $(".mostrarDetalle").css({ 'cursor': 'pointer' });
            $(".mostrarDetalle").click(function() {
                $(this).parents("tr").next("tr").find(".DetalleFactura").toggle();
            });
        }
        //  filtra las Ciudades
        function RefrescaUpdatePanel(idFiltro, idFlag) {
            var filtro = document.getElementById(idFiltro).value;
            var comboFiltrado = document.getElementById(idFlag).value;
            var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\s áéíóúÁÉÍÓÚ]+\s*$");
            try {
                if (patron.test(filtro) || filtro.length==0) {
                    if (filtro.length >= 3 || (filtro.length < 3 && comboFiltrado == "1")) {
                        MostrarOcultarDivFloater(true);
                        eo_Callback("cpFiltroFactura", "filtrarCiudad");
                        if (filtro.length >= 3) {
                            document.getElementById(idFlag).value = "1";
                        } else {
                            document.getElementById(idFlag).value = "0";
                        }
                    }
                } else if (filtro.value != "") { alert("Los caracteres especiales no son permitidos") }
                document.getElementById(idFiltro).focus();
            } catch (e) {
                MostrarOcultarDivFloater(false);
                alert("Error al tratar de filtrar Ciudades.\n" + e.description);
            }
        }

        function esRangoValido(source, arguments) {
            try {
                var vFechaSalida = $("#_eo_dlgInfoGuia_ctl00_dpFechaSalida_picker").val();
                var vFechaArribo = $("#_eo_dlgInfoGuia_ctl00_dpFechaEsperaArribo_picker").val();
                if (vFechaSalida != "" || vFechaArribo != "") {
                    if (vFechaSalida != " / / " && vFechaArribo == " / / ") {
                        arguments.IsValid = false;
                    } else {
                        if (vFechaSalida == " / / " && vFechaArribo != " / / ") {
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

        function fechasLogicas(source, arguments) {
            try {
                var vFechaSalida = $("#_eo_dlgInfoGuia_ctl00_dpFechaSalida_picker").val();
                var vFechaArribo = $("#_eo_dlgInfoGuia_ctl00_dpFechaEsperaArribo_picker").val();
                if (compare_dates(vFechaSalida, vFechaArribo))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;                                                                   
            } catch (e) {
                arguments.IsValid = false;
            }
        }

        function valFechaSalida(source, arguments) {
            try {                
                var valor = $("#_eo_dlgInfoGuia_ctl00_dpFechaSalida_picker").val();
                var patron = new RegExp("^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$");
                if (patron.test(valor))            
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            } catch (e) {
                arguments.IsValid = false;
            }            
        }
        function valFechaArribo(source, arguments) {
            try {                
                var valor = $("#_eo_dlgInfoGuia_ctl00_dpFechaEsperaArribo_picker").val();
                var patron = new RegExp("^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$");
                if (patron.test(valor))
                    arguments.IsValid = false;
                else
                    arguments.IsValid = true;
            } catch (e) {
                arguments.IsValid = false;
            }
        }
        
        function enfocar(id) {
            $(id).focus();
        }


        function CallbackAfterUpdateHandler(callback, extraData) {
            try {
                MostrarOcultarDivFloater(false);
            } catch (e) {
                alert("Error al tratar de evaluar respuesta del servidor.\n" + e.description);
            }

        }

        function MostrarOcultarDivFloater(mostrar) {
            var valorDisplay = mostrar ? "block" : "none";
            var elDiv = document.getElementById("divFloater");
            elDiv.style.display = valorDisplay;            
        }

        function compare_dates(fecha, fecha2) {
            var xMonth = fecha.substring(3, 5);
            var xDay = fecha.substring(0, 2);
            var xYear = fecha.substring(6, 10);
            var yMonth = fecha2.substring(3, 5);
            var yDay = fecha2.substring(0, 2);
            var yYear = fecha2.substring(6, 10);
            if (xYear > yYear) {
                return (true)
            }
            else {
                if (xYear == yYear) {
                    if (xMonth > yMonth) {
                        return (true)
                    }
                    else {
                        if (xMonth == yMonth) {
                            if (xDay > yDay)
                                return (true);
                            else
                                return (false);
                        }
                        else
                            return (false);
                    }
                }
                else
                    return (false);
            }
        }

        function EliminarFactura(obj) {
            var validaciones = "";
            validaciones = obj.parent("td").find('input[id*="hfInfoEstadoFactura"]').val();
            if (validaciones == "") {
                return confirm('Realmente desea eliminar\nla factura?');
            } else {
                var mensajes = validaciones.split("|");
                var mensaje;
                mensaje = "La factura se encuentra con:\n\n";
                $.each(mensajes, function(key, value) {
                    mensaje += value + "\n";
                });
                alert(mensaje);
                return false;
            }
        }

        function InfoEstadoDetalleOrden() {
            var validaciones = "";
            validaciones = $("#hfInformacionEstadoDetalleOrden").val();
            var mensajes = validaciones.split("|");
            var mensaje;
            mensaje = "El detalle de orden de compra se encuentra con:\n\n";
            $.each(mensajes, function(key, value) {
                mensaje += value + "\n";
            });
            alert(mensaje);
        }
    </script>

</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" runat="server">
    </asp:ScriptManager>
    <asp:HiddenField ID="hfIdOrdenCompra" runat="server" />
    <div>
        <eo:CallbackPanel ID="cpEncabezado" runat="server" UpdateMode="Always" Width="98%">
            <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />
        </eo:CallbackPanel>
    </div>
    <div>
        <div id="pnlInfoOrdenCompra" style="width: 800px;">
            <p class="subtitulo">
                Orden de Compra</p>
            <table class="tablaGris">
                <tr>
                    <td style="width: 40%;">
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
                        <asp:Label ID="lblObservacionOrden" runat="server" Text="" />
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
        <div id="pnlInfoDetalleOrdenCompra" style="width: 800px;">
            <p class="subtitulo">
                Detalle de la Orden de Compra</p>
            <table class="tablaGris">
                <tr>
                    <td style="width: 40%">
                        Fabricante:
                    </td>
                    <td>
                        <asp:Label ID="lblFabricante" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Producto:
                    </td>
                    <td>
                        <asp:Label ID="lblProducto" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Cantidad:
                    </td>
                    <td>
                        <asp:Label ID="lblCantidad" runat="server" Text="" />
                    </td>
                </tr>
            </table>
            <table class="tablaGris">
                <tr>
                    <td style="width: 40%">
                        Fecha de Registro:
                    </td>
                    <td>
                        <asp:Label ID="lblFechaRegistro" runat="server" Text="" />
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
                        Valor Unitario:
                    </td>
                    <td>
                        <asp:Label ID="lblValorUnitario" runat="server" Text="" />
                    </td>
                </tr>
            </table>
            <div style="clear: both; text-align: center;">      
                <eo:CallbackPanel ID="cpPnlBotonAgregarFactura" runat="server" Width="100%" UpdateMode="Always">
                <div id="pnlBotonAgregarFactura" runat="server">                                    
                    <asp:LinkButton ID="lnkAgregarFactura" runat="server" CssClass="negrita" >
                        <img src="../images/add.png" alt="Agregar factura" title="Agregar factura" />Agregar factura
                    </asp:LinkButton>
                </div>
                <asp:Panel ID="pnlInfoEstadoDetalleOrden" runat="server" onclick="InfoEstadoDetalleOrden();" style="cursor:pointer;">
                            <strong>Información del detalle de orden de compra:</strong>
                            <img id="imgInfoEstadoDetalleOrden" src="../images/Info-32.png" alt="Información del detalle de orden de compra" title="Información del detalle de orden de compra" />
                            <asp:HiddenField ID="hfInformacionEstadoDetalleOrden" runat="server" />
                        </asp:Panel>                 
                </eo:CallbackPanel>          
            </div>
        </div>
    </div>
    
    <eo:CallbackPanel ID="cpFacturasAgregadas" runat="server" UpdateMode="Conditional" Width="98%"
        LoadingDialogID="ldrWait_dlgWait">
        <div id="pnlFacturasAgregadas" runat="server" style="padding-top: 30px; width: 700px;">
            <p class="subtitulo">
                Facturas Agregadas</p>
            <asp:GridView ID="gvFacturasAgregadas" CssClass="tablaGris" runat="server" Width="100%"
                AutoGenerateColumns="False" EmptyDataText="No hay datos">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Image ID="imgShow" runat="server" ImageUrl="~/images/add.png" Visible="false"
                                CssClass="mostrarDetalle" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="factura" HeaderText="Factura" />
                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="CiudadCompra" HeaderText="Ciudad de Compra" />
                    <asp:TemplateField HeaderText="Guias">
                        <ItemTemplate>
                            <asp:BulletedList ID="bltGuias" runat="server" DataTextField="guiTransp" DataValueField="guiTransp">
                            </asp:BulletedList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Opciones">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEditarFatura" runat="server" CommandName="editar" CommandArgument='<%# Bind("idFactura") %>'
                                ImageUrl="~/images/Edit-32.png" ToolTip="Editar" />
                            <asp:ImageButton ID="imbAgregarFactura" runat="server" CommandName="agregarFactura"
                                CommandArgument='<%# Bind("idFactura") %>' ImageUrl="~/images/Folder-add-32.png"
                                ToolTip="Adicionar Guia" />
                            <asp:ImageButton ID="imgBtnDetalleFactura" runat="server" CommandName="detalleFactura"
                                CommandArgument='<%# Bind("idFactura") %>' ImageUrl="~/images/view.png" ToolTip="Detalle de factura" />
                            <asp:ImageButton ID="imgBtnEliminarFactura" runat="server" CommandName="eliminar"
                                CommandArgument='<%# Bind("idFactura") %>' ImageUrl="~/images/Delete-32.png" OnClientClick="return EliminarFactura($(this));"
                                ToolTip="Eliminar" />
                            <asp:HiddenField ID="hfInfoEstadoFactura" runat="server" />                            
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:HiddenField ID="hfIdFacturaEdicionActual" runat="server" />
        </div>
    </eo:CallbackPanel>
    
    <eo:CallbackPanel ID="cpGuias" runat="server" Width="90%" LoadingDialogID="ldrWait_dlgWait" ChildrenAsTriggers="true" UpdateMode="Group">
    <eo:Dialog runat="server" ID="dlgInfoGuia" CloseButtonUrl="00070101" 
        ControlSkinID="None" BorderColor="#335C88"
        BorderStyle="Solid" BorderWidth="1px" HeaderHtml="<b>Informaci&oacute;n de la Factura</b>" ResizeImageUrl="00020014" RestoreButtonUrl="00070103"
        ShadowColor="LightGray" ShadowDepth="3" MaxHeight="350" MaxWidth="550"
        VerticalAlign="Middle" BackShadeColor="Gray" BackShadeOpacity="50" Width="550px" AllowMove="False">
        <ContentTemplate> 
            <div>
                <uc1:EncabezadoPagina ID="EncabezadoFacGuia" runat="server" />
            </div>                       
            <div style="padding:10px 10px 0 10px;">
            <table class="tablaGris" width="100%">                
                <tr>
                    <td style="width: 140px;">
                        Transportadora:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTransportadora" runat="server" ValidationGroup="AgregarGuia">
                        </asp:DropDownList>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvTransportadora" runat="server" ControlToValidate="ddlTransportadora"
                                Display="Dynamic" InitialValue="0" ErrorMessage="Escoja la transportadora" ValidationGroup="consultarGuia"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        No. Guía:
                    </td>
                    <td>
                        <asp:TextBox ID="txtNoGuia" runat="server" ValidationGroup="consultarGuia" MaxLength="25"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="revNoGuia" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                Display="Dynamic" ControlToValidate="txtNoGuia" ValidationGroup="consultarGuia"
                                ErrorMessage="El no. guía contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="rfvNoGuia" runat="server" ControlToValidate="txtNoGuia"
                                Display="Dynamic" ErrorMessage="Ingrese el No. de guia" ValidationGroup="consultarGuia"></asp:RequiredFieldValidator>
                        </div>                                                                                                
                    </td>
                </tr>
                
                <tr>
                    <td colspan="2">
                        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" CssClass="boton" ValidationGroup="consultarGuia" />                       
                    </td>
                </tr>
            </table>
            </div>
            <div style="padding:5px 10px 10px 10px;">
            <div id="pnlAdicionarGuia" runat="server" style="width: 600px; text-align: center;
                background-color: #F9ED9B;" visible="false">
                <div style="padding: 10px;">
                    <table width="100%">
                        <tr>
                            <td>
                                Cantidad:
                            </td>
                            <td>
                                <asp:TextBox ID="txtCantidadGuiaExistente" runat="server" ValidationGroup="guiaExistente"
                                    MaxLength="8"></asp:TextBox>
                                <asp:Label ID="lblCantidadPermitidaGuiaExistente" runat="server" Text="" CssClass="comentario"></asp:Label>
                                <div>
                                    <asp:RegularExpressionValidator ID="revCantidadGuiaExistente" runat="server" ErrorMessage="Ingrese solo numeros"
                                        ControlToValidate="txtCantidadGuiaExistente" ValidationGroup="guiaExistente" Display="Dynamic"
                                        ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                                    <asp:RequiredFieldValidator ID="rfvCantidadGuiaExistente" runat="server" ControlToValidate="txtCantidadGuiaExistente"
                                        Display="Dynamic" ErrorMessage="Ingrese la cantidad" ValidationGroup="guiaExistente"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="cvCantidadGuiaExistente" runat="server" ControlToValidate="txtCantidadGuiaExistente"
                                        Display="Dynamic" ValidationGroup="guiaExistente" ValueToCompare="0" Operator="GreaterThan"
                                        ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                                    <asp:CompareValidator ID="cvCantidadPermitidaGuiaExistente" runat="server" ControlToValidate="txtCantidadGuiaExistente"
                                        Display="Dynamic" ValidationGroup="guiaExistente" Operator="LessThanEqual"
                                        ErrorMessage="La cantidad debe ser menor, por favor verifique."></asp:CompareValidator>
                                </div>
                            </td>
                        </tr>
                    </table>
                    <p>
                        Esta guia ya se encuentra registrada desea asociársela a la factura?</p>
                </div>
                <div>
                    <asp:Button ID="BtnAdicionarGuia" CssClass="boton" runat="server" Text="Adicionar" ValidationGroup="guiaExistente" />
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="BtnCancelarAdicion" CssClass="boton" runat="server" 
                        Text="Cancelar" />
                </div>
            </div>
            <table class="tablaGris" width="600" id="tblContenidoGuia" runat="server" visible="false">
                <tr>
                    <td>
                        Cantidad:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCantidadFacGuia" runat="server" ValidationGroup="consultarGuia"
                            MaxLength="8"></asp:TextBox>
                        <asp:Label ID="lblCantidadPermitidaPorFactura" runat="server" Text="" CssClass="comentario"></asp:Label>
                        <div>
                            <asp:RegularExpressionValidator ID="revCantidadFacGuia" runat="server" ErrorMessage="Ingrese solo numeros"
                                ControlToValidate="txtCantidadFacGuia" ValidationGroup="AgregarGuia" Display="Dynamic"
                                ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvCantidadFactGuia" runat="server" ControlToValidate="txtCantidadFacGuia"
                                Display="Dynamic" ErrorMessage="Ingrese la cantidad" ValidationGroup="AgregarGuia"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvCantidadFacGuia" runat="server" ControlToValidate="txtCantidadFacGuia"
                                Display="Dynamic" ValidationGroup="AgregarGuia" ValueToCompare="0" Operator="GreaterThan"
                                ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>                              
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="width: 140px;">
                        Pais:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPaisFacGuia" runat="server" ValidationGroup="AgregarGuia"
                            AutoPostBack="True">
                        </asp:DropDownList>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvPaisFacGuia" runat="server" ControlToValidate="ddlPaisFacGuia"
                                InitialValue="0" ValidationGroup="AgregarGuia" ErrorMessage="Seleccione el pais"
                                Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Ciudad de Origen:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCiudadOrigen" runat="server" ValidationGroup="AgregarGuia">
                        </asp:DropDownList>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvCiudadOrigen" runat="server" ControlToValidate="ddlCiudadOrigen"
                                Display="Dynamic" InitialValue="0" ErrorMessage="Seleccione la ciudad de origen"
                                ValidationGroup="AgregarGuia"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fecha de Salida:
                    </td>
                    <td>                         
                        <eo:DatePicker ID="dpFechaSalida" runat="server" PickerFormat="dd/MM/yyyy" ControlSkinID="None" CssBlock="&lt;style type=&quot;text/css&quot;&gt;
                        .DatePickerStyle1 {background-color:white;border-bottom-color:Silver;border-bottom-style:solid;border-bottom-width:1px;border-left-color:Silver;border-left-style:solid;border-left-width:1px;border-right-color:Silver;border-right-style:solid;border-right-width:1px;border-top-color:Silver;border-top-style:solid;border-top-width:1px;color:#2C0B1E;padding-bottom:5px;padding-left:5px;padding-right:5px;padding-top:5px}
                        .DatePickerStyle2 {border-bottom-color:#f5f5f5;border-bottom-style:solid;border-bottom-width:1px;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle3 {font-family:Verdana;font-size:8pt}
                        .DatePickerStyle4 {background-image:url('00040402');color:#1c7cdc;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle5 {background-image:url('00040401');color:#1176db;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle6 {color:gray;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle7 {cursor:pointer;cursor:hand;margin-bottom:0px;margin-left:4px;margin-right:4px;margin-top:0px}
                        .DatePickerStyle8 {background-image:url('00040403');color:Brown;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle9 {cursor:pointer;cursor:hand}
                        .DatePickerStyle10 {font-family:Verdana;font-size:8.75pt;padding-bottom:5px;padding-left:5px;padding-right:5px;padding-top:5px}
&lt;/style&gt;" DayCellHeight="15" DayCellWidth="31" DayHeaderFormat="Short" DisabledDates="" 
                        OtherMonthDayVisible="True" SelectedDates="" TitleFormat="MMMM, yyyy" 
                        TitleLeftArrowImageUrl="DefaultSubMenuIconRTL" 
                        TitleRightArrowImageUrl="DefaultSubMenuIcon" VisibleDate="2011-05-01">
                        <TodayStyle CssClass="DatePickerStyle5" />
                        <SelectedDayStyle CssClass="DatePickerStyle8" />
                        <DisabledDayStyle CssClass="DatePickerStyle6" />
                        <FooterTemplate>
                            <table border="0" cellPadding="0" cellspacing="5" 
                                style="font-size: 11px; font-family: Verdana">
                                <tr>
                                    <td width="30">
                                    </td>
                                    <td valign="center">
                                        <img src="{img:00040401}"></img></td>
                                    <td valign="center">
                                        Today: {var:today:dd/MM/yyyy}</td>
                                </tr>
                            </table>
                        </FooterTemplate>
                        <CalendarStyle CssClass="DatePickerStyle1" />
                        <TitleArrowStyle CssClass="DatePickerStyle9" />
                        <DayHoverStyle CssClass="DatePickerStyle4" />
                        <MonthStyle CssClass="DatePickerStyle7" />
                        <TitleStyle CssClass="DatePickerStyle10" />
                        <DayHeaderStyle CssClass="DatePickerStyle2" />
                        <DayStyle CssClass="DatePickerStyle3" />
                    </eo:DatePicker>
                        
                                              
                        <div>
                            <asp:CustomValidator ID="cvFechaSalida" runat="server" ErrorMessage="Por favor especificar una fecha de salida valida."
                                Display="Dynamic" ClientValidationFunction="valFechaSalida" ValidationGroup="AgregarGuia"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="rfvFechaSalida" runat="server" ControlToValidate="dpFechaSalida"
                                Display="Dynamic" ErrorMessage="Indique la fecha de salida" ValidationGroup="AgregarGuia"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Fecha Esperada Arribo:
                    </td>
                    <td>                                                
                        <eo:DatePicker ID="dpFechaEsperaArribo" runat="server" PickerFormat="dd/MM/yyyy" ControlSkinID="None" CssBlock="&lt;style type=&quot;text/css&quot;&gt;
                        .DatePickerStyle1 {background-color:white;border-bottom-color:Silver;border-bottom-style:solid;border-bottom-width:1px;border-left-color:Silver;border-left-style:solid;border-left-width:1px;border-right-color:Silver;border-right-style:solid;border-right-width:1px;border-top-color:Silver;border-top-style:solid;border-top-width:1px;color:#2C0B1E;padding-bottom:5px;padding-left:5px;padding-right:5px;padding-top:5px}
                        .DatePickerStyle2 {border-bottom-color:#f5f5f5;border-bottom-style:solid;border-bottom-width:1px;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle3 {font-family:Verdana;font-size:8pt}
                        .DatePickerStyle4 {background-image:url('00040402');color:#1c7cdc;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle5 {background-image:url('00040401');color:#1176db;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle6 {color:gray;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle7 {cursor:pointer;cursor:hand;margin-bottom:0px;margin-left:4px;margin-right:4px;margin-top:0px}
                        .DatePickerStyle8 {background-image:url('00040403');color:Brown;font-family:Verdana;font-size:8pt}
                        .DatePickerStyle9 {cursor:pointer;cursor:hand}
                        .DatePickerStyle10 {font-family:Verdana;font-size:8.75pt;padding-bottom:5px;padding-left:5px;padding-right:5px;padding-top:5px}
&lt;/style&gt;" DayCellHeight="15" DayCellWidth="31" DayHeaderFormat="Short" DisabledDates="" 
                        OtherMonthDayVisible="True" SelectedDates="" TitleFormat="MMMM, yyyy" 
                        TitleLeftArrowImageUrl="DefaultSubMenuIconRTL" 
                        TitleRightArrowImageUrl="DefaultSubMenuIcon" VisibleDate="2011-05-01">
                        <TodayStyle CssClass="DatePickerStyle5" />
                        <SelectedDayStyle CssClass="DatePickerStyle8" />
                        <DisabledDayStyle CssClass="DatePickerStyle6" />
                        <FooterTemplate>
                            <table border="0" cellPadding="0" cellspacing="5" 
                                style="font-size: 11px; font-family: Verdana">
                                <tr>
                                    <td width="30">
                                    </td>
                                    <td valign="center">
                                        <img src="{img:00040401}"></img></td>
                                    <td valign="center">
                                        Today: {var:today:dd/MM/yyyy}</td>
                                </tr>
                            </table>
                        </FooterTemplate>
                        <CalendarStyle CssClass="DatePickerStyle1" />
                        <TitleArrowStyle CssClass="DatePickerStyle9" />
                        <DayHoverStyle CssClass="DatePickerStyle4" />
                        <MonthStyle CssClass="DatePickerStyle7" />
                        <TitleStyle CssClass="DatePickerStyle10" />
                        <DayHeaderStyle CssClass="DatePickerStyle2" />
                        <DayStyle CssClass="DatePickerStyle3" />
                    </eo:DatePicker>                       
                        <div>                           
                            <asp:CustomValidator ID="cvFechaArribo" runat="server" ErrorMessage="Por favor especificar una fecha de arribo valida."
                                Display="Dynamic" ClientValidationFunction="valFechaArribo" ValidationGroup="AgregarGuia"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="rfvFechaEsperadaArribo" runat="server" ControlToValidate="dpFechaEsperaArribo"
                                Display="Dynamic" ErrorMessage="Indique la Fecha Esperada de Arribo" ValidationGroup="AgregarGuia"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="cusRango" runat="server" ErrorMessage="Es necesario especificar los dos valores de las fechas"
                                Display="Dynamic" ClientValidationFunction="esRangoValido" ValidationGroup="AgregarGuia"></asp:CustomValidator>
                            <asp:CustomValidator ID="cvFechasLogicas" runat="server" ErrorMessage="La Fecha de salida no debe ser mayor a la fecha de arribo"
                                Display="Dynamic" ClientValidationFunction="fechasLogicas" ValidationGroup="AgregarGuia"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Peso(Kg):
                    </td>
                    <td>
                        <asp:TextBox ID="txtPesoNeto" runat="server" ValidationGroup="AgregarGuia" MaxLength="8"></asp:TextBox><label
                            class="comentario">Formato ###,##</label>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvPesoNeto" runat="server" ControlToValidate="txtPesoNeto"
                                Display="Dynamic" ErrorMessage="Ingrese el peso neto" ValidationGroup="AgregarGuia"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rglPesoNeto" runat="server" ErrorMessage="Ingrese el formato de peso indicado"
                                ControlToValidate="txtPesoNeto" ValidationGroup="AgregarGuia" Display="Dynamic"
                                ValidationExpression="^(\d{1,6})(,\d{1,2})*$"></asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="cvPesoNeto" runat="server" ControlToValidate="txtPesoNeto"
                                Display="Dynamic" ValidationGroup="AgregarGuia" ValueToCompare="0" Operator="GreaterThan"
                                ErrorMessage="El peso debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                        </div>
                    </td>
                </tr>                
            </table>
            
            <div>
                <asp:Button ID="btnCrearGuia" runat="server" Text="Crear Guia" CssClass="boton" ValidationGroup="AgregarGuia" />&nbsp;&nbsp;
                <asp:Button ID="btnCancelarAdicionGuia" runat="server" Text="Cancelar" CssClass="boton" />
            </div>            
            </div>
        </ContentTemplate>
          <HeaderStyleActive CssText="padding-right: 4px; padding-left: 4px; font-size: 11px; background-image: url(00070104); padding-bottom: 3px; padding-top: 3px; font-family: tahoma" />
        <FooterStyleActive CssText="background-color: #e5f1fd; padding-bottom: 8px;" />
        <ContentStyleActive CssText="backcolor:white;background-color:white;border-top-color:#335c88;border-top-style:solid;border-top-width:1px;"/>        
    </eo:Dialog>
    </eo:CallbackPanel>
    
    <!--*********************************************** Panel para agregar factura *************** -->
    <eo:CallbackPanel ID="cpFactura" runat="server" Width="90%" LoadingDialogID="ldrWait_dlgWait"
        ChildrenAsTriggers="true"
        UpdateMode="Self">
        
        <eo:Dialog ID="dlgInfoFactura" runat="server" CloseButtonUrl="00070101" 
        ControlSkinID="None" BorderColor="#335C88"
        BorderStyle="Solid" BorderWidth="1px" HeaderHtml="<b>Informaci&oacute;n de la Factura</b>" ResizeImageUrl="00020014" RestoreButtonUrl="00070103"
        ShadowColor="LightGray" ShadowDepth="3" MaxHeight="350" MaxWidth="550"
        VerticalAlign="Middle" BackShadeColor="Gray" BackShadeOpacity="50" Height="280px" Width="550px" AllowMove="False"> 
            <ContentTemplate>                
                    <div id="divFloater" style="display: none;">                        
                                    <asp:Image ID="imgLoading" runat="server" ImageUrl="~/images/loader_dots.gif" />                                
                                    <b>Procesando...</b>                                
                    </div>
                    <div id="pnlDetalleOrdenCompra" runat="server" style="padding:10px;">
                        <div style="width: 500px">
                            <uc1:EncabezadoPagina ID="EncabezadoAgregarFactura" runat="server" />
                        </div>
                        <table class="tablaGris" width="550px">
                            <tr>
                                <td style="width: 140px;">
                                    Factura:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFactura" runat="server" ValidationGroup="AgregarFactura" TabIndex="0" MaxLength="25"></asp:TextBox>
                                    <div>
                                        <asp:RegularExpressionValidator ID="revFactura" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                            Display="Dynamic" ControlToValidate="txtFactura" ValidationGroup="AgregarFactura"
                                            ErrorMessage="La factura contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvFactura" runat="server" ControlToValidate="txtFactura"
                                            ValidationGroup="AgregarFactura" ErrorMessage="Ingrese la factura" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                    <asp:HiddenField ID="hfIdDetalleOrdenCompra" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Cantidad:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCantidad" runat="server" ValidationGroup="AgregarFactura" MaxLength="8"></asp:TextBox>
                                    <asp:Label ID="lblInfoCantMaxPermitida" runat="server" Text="" CssClass="comentario"></asp:Label>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" ControlToValidate="txtCantidad"
                                            ValidationGroup="AgregarFactura" ErrorMessage="Ingrese la cantidad" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="rglCantidad" runat="server" ErrorMessage="Ingrese solo números"
                                            ControlToValidate="txtCantidad" ValidationGroup="AgregarFactura" Display="Dynamic"
                                            ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                                        <asp:CompareValidator ID="cvCantidad" runat="server" ControlToValidate="txtCantidad"
                                            Display="Dynamic" ValidationGroup="AgregarFactura" ValueToCompare="0" Operator="GreaterThan"
                                            ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Pais:
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPais" runat="server" ValidationGroup="AgregarFactura" AutoPostBack="True">
                                        <asp:ListItem Value="0" Text="Seleccione..."></asp:ListItem>
                                    </asp:DropDownList>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvPais" runat="server" ControlToValidate="ddlPais"
                                            InitialValue="0" ValidationGroup="AgregarFactura" ErrorMessage="Seleccione el pais"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Ciudad de Compra:
                                </td>
                                <td>
                                    <asp:Label ID="lblIngreseCiudad" Style="display: block;" runat="server" Text="Ingrese la ciudad"
                                        CssClass="comentario"></asp:Label>
                                    <asp:TextBox ID="txtFiltroCiudad" runat="server" Width="150px" MaxLength="15"></asp:TextBox>
                                    &nbsp;-&nbsp;
                                    <eo:CallbackPanel ID="cpFiltroFactura" runat="server" UpdateMode="Self" ClientSideAfterUpdate="CallbackAfterUpdateHandler"
                                        Style="display: inline; padding: 0px 0px 0px 0px; vertical-align: middle">
                                        <asp:HiddenField ID="hfFlagFiltrado" runat="server" />
                                        <asp:DropDownList ID="ddlCiudadCompra" runat="server" ValidationGroup="AgregarFactura">
                                        </asp:DropDownList>
                                    </eo:CallbackPanel>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvCiudad" runat="server" ControlToValidate="ddlCiudadCompra"
                                            InitialValue="0" ValidationGroup="AgregarFactura" ErrorMessage="Seleccione la ciudad de compra"
                                            Display="Dynamic"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div style="width: 550px"><br />
                            <center>
                            <asp:Button ID="btnCrear" runat="server" Text="Crear Factura" CssClass="boton" ValidationGroup="AgregarFactura"
                                Visible="false" />&nbsp;&nbsp;
                            <asp:Button ID="btnEditarFactura" runat="server" Text="Editar" CssClass="boton" ValidationGroup="AgregarFactura"
                                Visible="false" />
                                </center>
                        </div>
                    </div>                
            </ContentTemplate>
            <HeaderStyleActive CssText="padding-right: 4px; padding-left: 4px; font-size: 11px; background-image: url(00070104); padding-bottom: 3px; padding-top: 3px; font-family: tahoma" />
        <FooterStyleActive CssText="background-color: #e5f1fd; padding-bottom: 8px;" />
        <ContentStyleActive CssText="backcolor:white;background-color:white;border-top-color:#335c88;border-top-style:solid;border-top-width:1px;"/>
        
        </eo:Dialog>
    </eo:CallbackPanel>
    <uc2:Loader ID="ldrWait" runat="server" />
    </form>
</body>
</html>
