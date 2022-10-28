<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearDetalleRecepcion.aspx.vb"
    Inherits="BPColSysOP.CrearDetalleRecepcion" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="../ControlesDeUsuario/ModalProgress.ascx" TagName="ModalProgress"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crear Detalle de Recepcion</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <style type="text/css">
        .exito
        {
            color: Green;
            font-size: 15px;
        }
        .tablaGris
        {
            width: 100%;
            padding: 0 10px;
            margin: 0;
        }
        .float
        {
            float: left;
            width: 50%;
        }
        #pnlDetalleOrdenRecepcion .both a:hover
        {
            color: Blue;
            text-decoration: underline;
            font-size: 9pt;
            cursor: pointer;
        }
        #pnlInfoOrdenRecepcion, pnlInfoDetalleOrdenRecepcion, pnlInfoFactura, pnlInfoGuia
        {
            cursor: pointer;
        }
        a.search, .search
        {
            padding: 5px;
            display: inline-block;
        }
        .tabular
        {
            padding-left: 20px;
        }
    </style>
    <script type="text/javascript" language="javascript">
        function CompararCantidad() {
            var banderaNovedad = false;
            var retorno = false;
            var mensajePrevio = "Se encontraron las siguientes novedades:\n\n";
            var mensaje = "";

            var cantidadEsperada = $("#hfCantidadPermitida").val();
            var cantidadRegistrada = $("#hfCantidadPalletRegistrada").val();
            if (cantidadEsperada != cantidadRegistrada) {
                banderaNovedad = true;
                mensaje = "Las cantidades de los pallets no coinciden.\n";
            }

            var cantidadEsperadaProductoAdicional = $("#lblCantidadProductoAdicional").text();

            if ($("#hfBoolProductoAdicional").val() == 1) {
                var cantidadRegistradaProductoAdicional = $("#hfCantidadPalletAdicionalRegistrada").val();

                if (cantidadEsperadaProductoAdicional != cantidadRegistradaProductoAdicional) {
                    banderaNovedad = true;
                    mensaje += "Las cantidades de pallets producto adicional no coinciden.\n";
                }
            }
            if (banderaNovedad) {
                mensaje += "\nRealmente desea cerrar la orden?";
                mensaje = mensajePrevio + mensaje;
            } else {
                mensaje += "\nEsta seguro de cerrar la orden de recepción?";
            }
            if (confirm(mensaje)) {
                __doPostBack('BtnCerrarRecepcion', '');
            } else {
                retorno = false;
            }
            return retorno;
        }
        $(document).ready(init);
        function init() {
            $(".subContenido").css({ 'display': 'none' })
            $("#pnlInfoOrdenRecepcion").click(function () { $("#pnlInfoOrdenRecepcion .subContenido").toggle(); });
            $("#pnlInfoDetalleOrdenRecepcion").click(function () { $("#pnlInfoDetalleOrdenRecepcion .subContenido").toggle(); });
            $("#pnlInfoFactura").click(function () { $("#pnlInfoFactura .subContenido").toggle(); });
            $("#pnlInfoGuia").click(function () { $("#pnlInfoGuia .subContenido").toggle(); });
            $("#imgAgregarNovedades").css({ 'cursor': 'pointer' }).toggle(
                function () {
                    $(this).attr('src', '../images/remove.png');
                    $("#gvNovedades").slideDown();
                },
                function () {
                    $("#gvNovedades").slideUp();
                    $(this).attr('src', '../images/add.png');
                }
            );
        }
        function validarProducto(source, arguments) {
            var idProducto = $("#ddlProducto").val();
            if (idProducto == 0)
                arguments.IsValid = false;
            else
                arguments.IsValid = true;
        }
        function validarLongitud(source, arguments) {
            try {
                var observacion = $("#txtObservacion").val();
                if (observacion.length > 400) {
                    arguments.IsValid = false;
                } else {
                    arguments.IsValid = true;
                }
            } catch (e) {
                arguments.IsValid = false;
            }
        }
        function RefrescaUpdatePaneProducto() {
            var filtro = $get("txtFiltroProducto").value;
            var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\s áéíóúÁÉÍÓÚ]+\s*$");
            if (patron.test(filtro)) {
                if (filtro.length > 2) {
                    $get("hfFlagFiltradoProducto").value = 1;
                    __doPostBack('txtFiltroProducto', '');
                    //$find(ModalProgress).hide();
                }
                else if (filtro.length <= 3 && $get("hfFlagFiltradoProducto").value == 1) {
                    $get("hfFlagFiltradoProducto").value = 0
                    __doPostBack('txtFiltroProducto', '');
                    //$find(ModalProgress).hide();
                }
            }
            else if ($get("txtFiltroProducto").value != "") { alert("los caracteres especiales no son permitidos") }
        }

        function TamanioVentana() {
            if (typeof (window.innerWidth) == 'number') {
                //Non-IE
                myWidth = window.innerWidth;
                myHeight = window.innerHeight;
            } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                //IE 6+ in 'standards compliant mode'
                myWidth = document.documentElement.clientWidth;
                myHeight = document.documentElement.clientHeight;
            } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                //IE 4 compatible
                myWidth = document.body.clientWidth;
                myHeight = document.body.clientHeight;
            }
        }

        function VerNovedad() {
            dialogoRegistro.PerformCallback('{0}' + ':cargar');
            dialogoRegistro.ShowWindow();
        }

        function VerImagenes() {
            dialogoImagen.PerformCallback('{0}' + ':cargar');
            TamanioVentana();
            dialogoImagen.SetSize(myWidth * 0.4, myHeight * 0.5);
            dialogoImagen.ShowWindow();
        }

        function VerGaleria() {
            dialogoVisor.PerformCallback('{0}' + ':visualizar');
            LoadingPanel.Show();
        }

        function Visor() {
            TamanioVentana();
            dialogoVisor.SetSize(myWidth * 0.4, myHeight * 0.50);
            var pcWidth = myWidth;
            var pxHieght = myWidth;
            var width = _aspxGetDocumentClientWidth()
            var height = _aspxGetDocumentClientHeight();
            dialogoVisor.ShowAtPos(width / 4, (pxHieght - height) / 10);
            dialogoVisor.ShowWindow();
        }

        function _aspxGetDocumentClientWidth() {
            if (__aspxSafari || __aspxIE55 || document.documentElement.clientWidth == 0)
                return document.body.clientWidth;
            return document.documentElement.clientWidth;
        }

        function _aspxGetDocumentClientHeight() {
            if (__aspxSafari)
                return window.innerHeight;
            if (__aspxIE55 || __aspxOpera || document.documentElement.clientHeight == 0)
                return document.body.clientHeight;
            return document.documentElement.clientHeight;
        }

        //---------------------------------------------------------------------------
        //Cargue Archivo
        function ProcesarCargaArchivo(s, e) {
        }

        function CargueArchivos(s, e) {
            if (s.cpResultado != null) {
                var mensaje = s.cpResultado;
                $('#pcRegistro_lblMensaje').text(mensaje);
                e.processOnServer = false;
            }

            if (s.cpPeso >= 5242880) {
                alert('La imagenes seleccionadas no se cargaron en el sistema porque exceden el tamaño permitido de 5 Megas. Por favor seleccionar las imagenes nuevamente.');
                e.processOnServer = false;
            }
            if (s.cpMensaje != null) {
                var mensaje = s.cpMensaje;
                $('#pcRegistro_lblImagen').text(mensaje);
                $('#pcRegistro_lblRespuesta').text(s.cpCantidad);
            }
        }

        function ucArchivo_OnUploadStart(s, e) {
            ucArchivo.Upload();
        }
        //---------------------------------------------------------------------------
        //Cargue Imagenes
        function ProcesarCargaImagen(s, e) {
            if (s.cpMensaje != null) {
                var cantidad = s.cpMensaje;
                ValidarCantidad(cantidad)
            }
        }

        function ValidarCantidad(cantidad) {
            if (cantidad >= 15) {
                document.getElementById("trAdjuntar").style.display = "none";
                document.getElementById("trBlanco").style.display = "none";
            } if (cantidad < 15) {
                document.getElementById("trAdjuntar").style.display = "block";
                document.getElementById("trBlanco").style.display = "block";
            }
            $('#lblCantidadArchivos').text(cantidad);
        }

        function ucImagen_OnUploadStart(s, e) {
            var files = s.GetText().replace(/\s|C:\\fakepath\\/g, "").split(",");
            if (files.length > 15) {
                e.cancel = true;
                alert("Número de archivos seleccionados excede la cantidad permitida (15 Archivos).");
            } else {
                ucImagen.Upload();
            }
        }

        //---------------------------------------------------------------------------
        function ConfirmacionNovedad() {
            if (!confirm("Novedad almacenada exitosamente, desea registrar una nueva novedad?")) {
                dialogoRegistro.Hide();
            }
        }

    </script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <div id="contenedorPrin" style="width: 1100px">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div>
            <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />
        </div>
        <div id="pnlDetalleOrdenRecepcion" style="border: solid #000 1px;">
            <p class="subtitulo">
                Datos de Orden de Recepción</p>
            <table class="float tablaGris">
                <tr>
                    <td>
                        Tipo de Producto:
                    </td>
                    <td>
                        <asp:Label ID="lblTipoProducto" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Tipo de Recepción:
                    </td>
                    <td>
                        <asp:Label ID="lblTipoRecepcion" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Orden de Compra:
                    </td>
                    <td>
                        <asp:Label ID="lblNumeroOrdenCompra" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Remisión:
                    </td>
                    <td>
                        <asp:Label ID="lblRemision" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Cantidad:
                    </td>
                    <td>
                        <asp:Label ID="lblCantidad" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Cantidad recibida:
                    </td>
                    <td>
                        <asp:Label ID="lblCantidadRecibida" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Consignado a:
                    </td>
                    <td>
                        <asp:Label ID="lblConsignado" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Novedades:
                    </td>
                    <td>
                        <asp:LinkButton ID="lbNovedad" runat="server" OnClientClick="VerNovedad(); return false;">
                        <img alt="Imagen" src="../images/comment_add.png" />&nbsp;&nbsp;Adicionar Novedad
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr id="trBlanco">
                    <td colspan="2" style="height: 65px">
                        <br />
                    </td>
                </tr>
                <tr id="trCantidadProductoAdicional" runat="server">
                    <td>
                        Cant. producto adicional:
                    </td>
                    <td>
                        <asp:Label ID="lblCantidadProductoAdicional" runat="server" />
                    </td>
                </tr>
            </table>
            <table class="float tablaGris">
                <tr>
                    <td>
                        Factura:
                    </td>
                    <td>
                        <asp:Label ID="lblFactura" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Guia:
                    </td>
                    <td>
                        <asp:Label ID="lblGuia" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Orden Recepción No:
                    </td>
                    <td>
                        <asp:Label ID="lblNumeroRecepcion" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Fecha de Creación:
                    </td>
                    <td>
                        <asp:Label ID="lblFechaRecepcion" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Destinatario:
                    </td>
                    <td>
                        <asp:Label ID="lblDestinatario" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Peso total pallet's:
                    </td>
                    <td>
                        <asp:Label ID="lblPesoPallet" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Estado:
                    </td>
                    <td>
                        <asp:Label ID="lblEstadoOrden" runat="server" />
                    </td>
                </tr>
                <tr id="trAdjuntar">
                    <td>
                        Adjuntar Imagenes:
                    </td>
                    <td align="left">
                        <dx:ASPxUploadControl ID="ucImagen" runat="server" ClientInstanceName="ucImagen"
                            NullText="Seleccione las imagenes de la Recepción..." ShowProgressPanel="True"
                            UploadMode="Advanced" Width="350px" ShowAddRemoveButtons="False" ShowUploadButton="False">
                            <AdvancedModeSettings EnableMultiSelect="true" />
                            <ClientSideEvents TextChanged="function(s, e) { ucImagen_OnUploadStart(s, e); }" />
                            <ClientSideEvents FileUploadComplete="function(s, e) { ProcesarCargaImagen(s, e); }" />
                            <ValidationSettings AllowedFileExtensions=".png, .gif, .jpg" MaxFileSize="10485760">
                            </ValidationSettings>
                        </dx:ASPxUploadControl>
                    </td>
                </tr>
                <tr id="trLinkImagen">
                    <td>
                        <asp:LinkButton ID="lbImagen" runat="server" OnClientClick="VerImagenes(); return false;">
                        <img alt="Imagenes" src="../images/DxSearch16.png" />&nbsp;&nbsp;Ver imagenes cargadas
                        </asp:LinkButton>
                    </td>
                    <td>
                        <dx:ASPxLabel ID="lblMensajeCantidad" runat="server" Text="Cantidad de Imagenes: "
                            ClientInstanceName="lblMensajeCantidad">
                        </dx:ASPxLabel>
                        <dx:ASPxLabel ID="lblCantidadArchivos" runat="server" Text="0" ClientInstanceName="lblCantidadArchivos">
                        </dx:ASPxLabel>
                    </td>
                </tr>
            </table>
            <div style="clear: both; text-align: center;" class="both">
                <asp:HiddenField ID="hfBoolProductoAdicional" runat="server" />
                <asp:HiddenField ID="hfCantidadPermitida" runat="server" />
                <asp:HiddenField ID="hfCantidadPalletRegistrada" runat="server" />
                <asp:HiddenField ID="hfCantidadPalletAdicionalRegistrada" runat="server" />
                <asp:Button ID="BtnCerrarRecepcion" OnClientClick="return CompararCantidad();" runat="server"
                    Text="Cerrar Recepción" />
            </div>
        </div>
        <div id="pnlCrearPallet" class="float" runat="server">
            <table class="tablaGris">
                <tr>
                    <th colspan="2" align="center">
                        Crear Pallet
                    </th>
                </tr>
                <tr>
                    <td style="width: 60px;">
                        Producto:
                    </td>
                    <td>
                        <asp:HiddenField ID="hfFlagFiltradoProducto" runat="server" />
                        <asp:TextBox ID="txtFiltroProducto" runat="server" onkeyup="RefrescaUpdatePaneProducto();"
                            OnTextChanged="FiltrarProducto" Width="70px" MaxLength="15"></asp:TextBox>
                        <asp:DropDownList ID="ddlProducto" runat="server" ValidationGroup="grpIngresoPallet"
                            AutoPostBack="true" Width="150px">
                        </asp:DropDownList>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvProducto" runat="server" ErrorMessage="Seleccione el producto"
                                InitialValue="0" ValidationGroup="grpIngresoPallet" Display="Dynamic" ControlToValidate="ddlProducto"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Color:
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="cmbColor" runat="server" Width="150px">
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Cantidad:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCantidad" runat="server" ValidationGroup="grpIngresoPallet" MaxLength="8"></asp:TextBox>
                        <div>
                            <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" ControlToValidate="txtCantidad"
                                Display="Dynamic" ValidationGroup="grpIngresoPallet" ErrorMessage="Ingrese la cantidad"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rglConsultarPromotorDoc" runat="server" ErrorMessage="Ingrese solo números"
                                ControlToValidate="txtCantidad" ValidationGroup="grpIngresoPallet" Display="Dynamic"
                                ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="cvCantidad" runat="server" ControlToValidate="txtCantidad"
                                Display="Dynamic" ValidationGroup="grpIngresoPallet" ValueToCompare="0" Operator="GreaterThan"
                                ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Peso(Kg):
                    </td>
                    <td>
                        <asp:TextBox ID="txtPeso" runat="server" ValidationGroup="grpIngresoPallet" MaxLength="10"></asp:TextBox>Formato
                        ###,##
                        <div>
                            <asp:RequiredFieldValidator ID="rfvPeso" runat="server" ControlToValidate="txtPeso"
                                Display="Dynamic" ErrorMessage="Ingrese el peso." ValidationGroup="grpIngresoPallet"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="rglPeso" runat="server" ErrorMessage="Ingrese el formato de peso indicado"
                                ControlToValidate="txtPeso" ValidationGroup="grpIngresoPallet" Display="Dynamic"
                                ValidationExpression="^(\d{1,6})(,\d{1,2})*$"></asp:RegularExpressionValidator>
                            <asp:CompareValidator ID="cvPeso" runat="server" ControlToValidate="txtPeso" Display="Dynamic"
                                ValidationGroup="grpIngresoPallet" ValueToCompare="0" Operator="GreaterThan"
                                ErrorMessage="El peso neto debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        Observación:
                    </td>
                    <td>
                        <asp:TextBox ID="txtObservacion" runat="server" MaxLength="399" TextMode="MultiLine"
                            ValidationGroup="grpIngresoPallet"></asp:TextBox>
                        <div>
                            <asp:RegularExpressionValidator ID="revObservacion" runat="server" ValidationExpression="^([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])([a-zA-Z_0-9,\-\s\.áéíóúñÁÉÍÓÚÑ])*([a-zA-Z_0-9áéíóúñÁÉÍÓÚÑ])*$"
                                Display="Dynamic" ControlToValidate="txtObservacion" ValidationGroup="grpIngresoPallet"
                                ErrorMessage="La observación contiene caracteres no validos, por favor verifique."></asp:RegularExpressionValidator>
                            <asp:CustomValidator ID="cvValidarLongitud" runat="server" ErrorMessage="El maximo de caracteres es 400. Por favor verifique."
                                ClientValidationFunction="validarLongitud" ValidationGroup="grpIngresoPallet"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
            </table>
            <div>
                <img src="../images/add.png" alt="Agregar Novedades" title="Agregar Novedades" id="imgAgregarNovedades" />
                <label class="negrita">
                    Agregar Novedades</label></div>
            <asp:GridView ID="gvNovedades" runat="server" AutoGenerateColumns="False" CssClass="tablaGris"
                Style="width: auto; display: none; padding: 5px; border: none;">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HiddenField ID="hfIdNovedad" runat="server" Value='<%# Bind("idNovedad") %>' />
                            <asp:CheckBox ID="chkNovedad" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="descripcion" HeaderText="Novedad" />
                </Columns>
            </asp:GridView>
            <div style="margin-top: 20px;">
                <hr />
                <asp:LinkButton ID="btnCrear" runat="server" CssClass="search" ValidationGroup="grpIngresoPallet"><img 
                                            src="../images/package.png" alt=""/>&nbsp;Crear Pallet</asp:LinkButton>
                <asp:HiddenField ID="hfFacturaGuia" runat="server" />
                <asp:HiddenField ID="hfOrdenRecepcion" runat="server" />
            </div>
            <div id="pnlProductoAdicional" runat="server">
                <table class="tablaGris">
                    <tr id="trProductoAdicional" runat="server">
                        <td colspan="2">
                            <table width="100%">
                                <tr>
                                    <th colspan="4">
                                        PRODUCTO ADICIONAL
                                    </th>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Producto:
                                    </td>
                                    <td colspan="3">
                                        <asp:DropDownList ID="ddlProductoAdicional" runat="server">
                                        </asp:DropDownList>
                                        &nbsp;<asp:Label ID="lblCantidadAdicional" runat="server" Font-Italic="True" Font-Size="8pt"
                                            ForeColor="Gray"></asp:Label><div>
                                                <asp:RequiredFieldValidator ID="rfvProductoAdicional" runat="server" ErrorMessage="Escoja un Producto, por favor"
                                                    ControlToValidate="ddlProductoAdicional" Display="Dynamic" InitialValue="0" ValidationGroup="crearAdicional"></asp:RequiredFieldValidator>
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="field">
                                        Cantidad:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCantidadAdicional" runat="server" MaxLength="8"></asp:TextBox>
                                        <div style="display: block">
                                            <asp:RequiredFieldValidator ID="rfvCantidadAdicional" runat="server" ErrorMessage="Digite la cantidad recibida, por favor"
                                                ControlToValidate="txtCantidadAdicional" Display="Dynamic" ValidationGroup="crearAdicional"></asp:RequiredFieldValidator>
                                        </div>
                                        <div style="display: block">
                                            <asp:RegularExpressionValidator ID="revCantidadAdicional" runat="server" ErrorMessage="El campo es numérico.<br/>Digite un número válido, por favor"
                                                ControlToValidate="txtCantidadAdicional" Display="Dynamic" ValidationExpression="(\s+)?(\d+)(\s+)?"
                                                ValidationGroup="crearAdicional"></asp:RegularExpressionValidator>
                                            <asp:CompareValidator ID="cvCantidadAdicional" runat="server" ControlToValidate="txtCantidadAdicional"
                                                Display="Dynamic" ValidationGroup="crearAdicional" ValueToCompare="0" Operator="GreaterThan"
                                                ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:LinkButton ID="lnkAgregarProductoAdicional" runat="server" ValidationGroup="crearAdicional"
                                    CssClass="search"><img src="../images/Folder-add-32.png" alt="" />&#160;Adicionar</asp:LinkButton>
                                <asp:GridView ID="gvProductoAdicional" runat="server" CssClass="tablaGris" AutoGenerateColumns="False"
                                    Style="width: 500px;">
                                    <Columns>
                                        <asp:BoundField DataField="producto" HeaderText="Producto" />
                                        <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Opciones" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibEliminarCaja" runat="server" ImageUrl="~/images/remove.png"
                                                    CommandName="Anular" ToolTip="Remover Caja" CommandArgument='<%#Bind("idCaja") %>'
                                                    OnClientClick="return confirm('¿Realmente desea remover el item indicado?');" />
                                                <asp:HiddenField ID="hfPosicionProductoAdicional" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:HiddenField ID="hfTotalProductoAdicional" runat="server" Value="0" />
                            </div>
                        </td>
                    </tr>
                </table>
                <div style="height: 20px;">
                </div>
                <hr />
                <table class="tablaGris" id="tblCrearPalletAdicional" runat="server">
                    <tr>
                        <td>
                            Peso(Kg):
                        </td>
                        <td>
                            <asp:TextBox ID="txtPesoPalletAdicionl" runat="server" ValidationGroup="crearPalletAdicional"
                                MaxLength="18"></asp:TextBox>Formato ###,##
                            <div>
                                <asp:RequiredFieldValidator ID="rfvPesoPalletAdicional" runat="server" ControlToValidate="txtPesoPalletAdicionl"
                                    Display="Dynamic" ValidationGroup="crearPalletAdicional" ErrorMessage="Ingrese el formato de peso indicado, para el pallet de producto adicional"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="revPesoPalletAdicional" runat="server" ErrorMessage="Peso no válido. Se espera un valor decimal, por favor verifique"
                                    ControlToValidate="txtPesoPalletAdicionl" Display="Dynamic" ValidationExpression="(\d+)(,\d{1,2})?"
                                    ValidationGroup="crearPalletAdicional"></asp:RegularExpressionValidator>
                                <asp:CompareValidator ID="cvPesoPalletAdicional" runat="server" ControlToValidate="txtPesoPalletAdicionl"
                                    Display="Dynamic" ValidationGroup="crearPalletAdicional" ValueToCompare="0" Operator="GreaterThan"
                                    ErrorMessage="El peso debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:LinkButton ID="lnkCrearPalletProAdicional" runat="server" CssClass="search"
                                ValidationGroup="crearPalletAdicional"><img 
                        src="../images/package.png" alt=""/>&nbsp;Crear Pallet Producto Adicional</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="pnlPalletCreados" class="float">
            <table class="tablaGris">
                <tr>
                    <th>
                        PALLETs ADICIONADOS
                    </th>
                </tr>
            </table>
            <asp:GridView ID="gvDetallePallet" runat="server" FooterStyle-CssClass="thGris" CssClass="tablaGris"
                AutoGenerateColumns="False" ShowFooter="True" EmptyDataText="&lt;blockquote&gt;No se han adicionado Pallets&lt;/blockquote&gt;">
                <Columns>
                    <asp:BoundField DataField="idPallet" HeaderText="No. Pallet">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="nombreProducto" HeaderText="Producto" />
                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad" Visible="False">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="nombreColor" HeaderText="Color" Visible="true">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="cantidadRecibida" HeaderText="Cantidad Recibida">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="unidadEmpaque" HeaderText="Tipo Unidad" />
                    <asp:BoundField DataField="idOrdenBodega" HeaderText="Orden de Bodega" />
                    <asp:BoundField DataField="peso" HeaderText="Peso(Kg)">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Opciones">
                        <ItemTemplate>
                            <asp:CheckBox ID="ckReImpresion" runat="server" Visible="false" />
                            <asp:ImageButton ID="imgBtnVerNovedades" runat="server" CommandName="verNovedades"
                                CommandArgument='<%# Bind("idPallet") %>' ImageUrl="~/images/view.png" ToolTip="Ver Novedades" />
                            <asp:ImageButton ID="imgBtnGenerar" CommandName="imprimirViajera" CommandArgument='<%# Bind("idPallet") %>'
                                runat="server" ImageUrl="~/images/Pdf.gif" />
                            <asp:ImageButton ID="imgBtnEliminarPallet" runat="server" ImageUrl="~/images/remove.png"
                                CommandName="Eliminar" ToolTip="Eliminar Pallet" CommandArgument='<%#Bind("idPallet") %>'
                                OnClientClick="return confirm('¿Realmente desea remover el pallet indicado?');" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle CssClass="thGris" />
            </asp:GridView>
            <div style="height: 25px;">
            </div>
            <table class="tablaGris" style="width: 100%" runat="server" id="tblPalletsProductoAdicional">
                <tr>
                    <th>
                        PALLETs ADICIONADOS PRODUCTO ADICIONAL
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvPalletProductoAdicional" runat="server" AutoGenerateColumns="False"
                            CssClass="tablaGris" Style="width: 100%" FooterStyle-CssClass="thGris" ShowFooter="True"
                            EmptyDataText="&lt;blockquote&gt;No se han adicionado Pallets para producto adicional&lt;/blockquote&gt;">
                            <Columns>
                                <asp:BoundField DataField="idPallet" HeaderText="No. Pallet" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Peso" HeaderText="Peso(Kg)">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Detalle del Pallet">
                                    <ItemTemplate>
                                        <asp:GridView ID="gvDetalle" runat="server" AutoGenerateColumns="False">
                                            <Columns>
                                                <asp:BoundField DataField="nombreProducto" HeaderText="Producto" />
                                                <asp:BoundField DataField="cantidadRecibida" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                        </asp:GridView>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Opc." ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibImprimir" runat="server" CommandArgument='<%#Bind("idPallet") %>'
                                            CommandName="Imprimir" ImageUrl="~/images/pdf.gif" />
                                        <asp:ImageButton ID="imgBtnEliminarPallet" runat="server" ImageUrl="~/images/remove.png"
                                            CommandName="Eliminar" ToolTip="Eliminar Pallet" CommandArgument='<%#Bind("idPallet") %>'
                                            OnClientClick="return confirm('¿Realmente desea remover el pallet indicado?');" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle CssClass="thGris" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <div style="clear: both">
        </div>
        <asp:HiddenField ID="hfValidarCierre" runat="server" />
        <cc1:ModalPopupExtender ID="mpeConfirmarRecepcion" runat="server" PopupControlID="pnlAceptar"
            BackgroundCssClass="modalBackground" TargetControlID="hfValidarCierre">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="pnlAceptar" runat="server" CssClass="modalPopUp" Style="width: 250px;
            display: none;">
            <div>
                <p style="text-align: justify; padding: 15px;">
                    La cantidad ingresada es mayor a la cantidad indicada para esta recepción. En caso
                    de seguir con la recepción se modificara la cantidad de la factura y guia para esta
                    recepción. &nbsp;
                    <div style="text-align: center;">
                        ¿Desea recepcionar así?</div>
                </p>
            </div>
            <div style="text-align: center;">
                <asp:Button ID="btnAceptar" runat="server" Text="Aceptar" CssClass="boton" ValidationGroup="OpcionPallet" />&nbsp;&nbsp;<asp:Button
                    ID="btnCancelar" runat="server" Text="Cancelar" CssClass="boton" ValidationGroup="OpcionPallet" />
            </div>
        </asp:Panel>
        <asp:HiddenField ID="hfValidarCierreNovedades" runat="server" />
        <cc1:ModalPopupExtender ID="mpeMostrarNovedades" runat="server" PopupControlID="pnlMostrarNovedades"
            BackgroundCssClass="modalBackground" TargetControlID="hfValidarCierreNovedades">
        </cc1:ModalPopupExtender>
        <asp:Panel ID="pnlMostrarNovedades" runat="server" CssClass="modalPopUp" Style="width: 250px;
            display: none;">
            <div style="text-align: right;">
                <asp:ImageButton ID="imgBtnCerrarPopUp" runat="server" ImageUrl="~/images/cerrar.gif" /></div>
            <div class="subtitulo" style="text-align: center;">
                Novedades
            </div>
            <div>
                <div style="text-align: justify; padding: 5px;">
                    <asp:BulletedList ID="bltNovedades" runat="server" DataTextField="novedad">
                    </asp:BulletedList>
                </div>
            </div>
        </asp:Panel>
    </div>
    <br />
    <uc2:ModalProgress ID="ModalProgress1" runat="server" />
    <dx:ASPxPopupControl ID="pcRegistro" runat="server" ClientInstanceName="dialogoRegistro"
        ShowHeader="true" ShowFooter="false" HeaderText="Registro de Novedades" AllowDragging="True"
        Width="400px" Height="200px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Modal="True" CloseAction="CloseButton">
        <ClientSideEvents EndCallback="function(s,e){
            if (s.cpResultado==999){
                ConfirmacionNovedad();
            } else {
                $('#pcRegistro_lblMensaje').text(s.cpMensaje);
            }
        }" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <table width="100%">
                    <tr>
                        <td align="center" colspan="2">
                            <dx:ASPxLabel ID="lblMensaje" runat="server" Text="" ClientInstanceName="lblMensaje">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                            Observaciones:
                        </td>
                        <td align="left">
                            <dx:ASPxMemo ID="mmObservacion" runat="server" Height="100px" Width="500px" NullText="Digite Observación..."
                                ClientInstanceName="mmObservacion">
                            </dx:ASPxMemo>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Adjuntar Archivo:
                        </td>
                        <td align="left">
                            <dx:ASPxUploadControl ID="ucArchivo" runat="server" ClientInstanceName="ucArchivo"
                                NullText="Seleccione imagenes o Archivos..." ShowProgressPanel="True" UploadMode="Advanced"
                                Width="100%" ShowAddRemoveButtons="False" ShowUploadButton="False">
                                <AdvancedModeSettings EnableMultiSelect="true" />
                                <ClientSideEvents TextChanged="function(s, e) { ucArchivo_OnUploadStart(s, e); }" />
                                <ClientSideEvents FileUploadComplete="function(s, e) { ProcesarCargaArchivo(s, e); }" />
                                <ClientSideEvents FilesUploadComplete="function(s, e){ CargueArchivos(s,e)}" />
                                <ValidationSettings AllowedFileExtensions=".doc, .docx, .png, .gif, .jpg, .pdf" MaxFileSize="5242880">
                                </ValidationSettings>
                            </dx:ASPxUploadControl>
                            <div>
                                <dx:ASPxLabel ID="lblImagen" runat="server" ClientInstanceName="lblImagen" ForeColor="#0000CC">
                                </dx:ASPxLabel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <dx:ASPxButton ID="btnRegistrar" runat="server" Text="Registrar Novedad" HorizontalAlign="center"
                                ClientInstanceName="btnRegistrar" ValidationGroup="Cargar" AutoPostBack="false"
                                Image-Url="../images/DxConfirm32.png" Width="200px" Height="27px">
                                <Image Url="../images/DxConfirm32.png">
                                </Image>
                                <ClientSideEvents Click="function(s, e) {
                                    if (lblRespuesta.GetText() == '-1' || mmObservacion.GetText()==''){
                                        alert('Debe digitar Novedad y seleccionar el archivo antes de continuar con el proceso.');
                                        e.processOnServer = false; 
                                    } else {
                                        dialogoRegistro.PerformCallback('{0}' + ':grabar');
                                    }
                                }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <dx:ASPxLabel ID="lblRespuesta" runat="server" ClientVisible="false" Text="-1" ClientInstanceName="lblRespuesta">
                            </dx:ASPxLabel>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <br />
    <dx:ASPxPopupControl ID="pcImagen" runat="server" ClientInstanceName="dialogoImagen"
        ShowHeader="true" ShowFooter="false" HeaderText="Imagenes de la Recepción" AllowDragging="True"
        Width="200px" Height="200px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        Modal="True" CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table width="100%">
                    <tr>
                        <td align="right">
                            <asp:LinkButton ID="lbGaleria" runat="server" OnClientClick="VerGaleria(); return false;"
                                Font-Bold="True" Font-Italic="True" Font-Underline="True">
                                <img alt="Imagenes" src="../images/view.png" />&nbsp;&nbsp;Ver Galeria
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <dx:ASPxRoundPanel ID="rpImagenes" runat="server" HeaderText="Imagenes Cargadas"
                    Width="100%" Height="50%">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent5" runat="server" SupportsDisabledAttribute="True">
                            <dx:ASPxGridView ID="gvImagenes" runat="server" Width="100%" ClientInstanceName="gvImagenes"
                                AutoGenerateColumns="False" KeyFieldName="idImagen">
                                <ClientSideEvents EndCallback="function(s,e){
                                    $(&#39;#divEncabezado&#39;).html(s.cpMensaje);
                                     if (s.cpCantidad != null) {
                                        ValidarCantidad(s.cpCantidad)
                                     }
                                }"></ClientSideEvents>
                                <Columns>
                                    <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="0" FieldName="nombre"
                                        Caption="Nombre Imagen">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn ShowInCustomizationForm="True" VisibleIndex="1" FieldName="peso"
                                        Caption="Peso Imagen">
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataBinaryImageColumn Caption="Imagen" ShowInCustomizationForm="true"
                                        VisibleIndex="2" FieldName="imagen" PropertiesBinaryImage-ImageWidth="100px"
                                        PropertiesBinaryImage-ImageHeight="100px">
                                        <PropertiesBinaryImage ImageHeight="50px" ImageWidth="50px">
                                        </PropertiesBinaryImage>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <CellStyle HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataBinaryImageColumn>
                                    <dx:GridViewDataColumn Caption="Acciones" VisibleIndex="5" CellStyle-HorizontalAlign="Center"
                                        Width="20px">
                                        <DataItemTemplate>
                                            <dx:ASPxHyperLink runat="server" ID="lnkEliminar" ImageUrl="../images/confirmation.png"
                                                Cursor="pointer" ToolTip="Eliminar imagen" OnInit="Link_Init">
                                                <ClientSideEvents Click="function(s, e) {
                                                    gvImagenes.PerformCallback('{0}'+':eliminar');
                                                    VerImagenes()
                                                }" />
                                            </dx:ASPxHyperLink>
                                        </DataItemTemplate>
                                        <CellStyle HorizontalAlign="Center">
                                        </CellStyle>
                                    </dx:GridViewDataColumn>
                                </Columns>
                                <SettingsPager PageSize="5">
                                </SettingsPager>
                            </dx:ASPxGridView>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <br />
    <dx:ASPxPopupControl ID="pcVisor" runat="server" ClientInstanceName="dialogoVisor"
        ShowCloseButton="true" ShowHeader="true" ShowFooter="false"
        HeaderText="Visor de Imagenes" AllowDragging="True" Width="200px" Height="50px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Modal="True"
        CloseAction="CloseButton">
        <ClientSideEvents EndCallback="function(s,e){
            LoadingPanel.Hide(); 
            Visor()
        }" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <dx:ASPxImageSlider ID="isImagenes" runat="server">
                </dx:ASPxImageSlider>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <br />
    <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
        Modal="True">
    </dx:ASPxLoadingPanel>
    <br />
    </form>
</body>
</html>
