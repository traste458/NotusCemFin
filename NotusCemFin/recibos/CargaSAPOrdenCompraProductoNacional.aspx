<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CargaSAPOrdenCompraProductoNacional.aspx.vb"
    Inherits="BPColSysOP.CargaSAPOrdenCompraProductoNacional" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register src="../ControlesDeUsuario/ModalProgress.ascx" tagname="ModalProgress" tagprefix="uc2" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Carga SAP Orden Compra Producto Nacional</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />

    <script src="../include/jquery-1.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(document).ready(function() {
            $("#gvDatos #gvDatos_ctl01_chkEncabezadoGenerar").click(function() {
                if (this.checked) {
                    $("#gvDatos .chkOrdenSeleccionarDes").children().each(function() { this.checked = true });
                } else {
                    $("#gvDatos .chkOrdenSeleccionarDes").children().each(function() { this.checked = false });
                }
            });
            ActualizarSession();
        });
        function ActualizarSession() {
            $.ajax({
                type: "POST",
                url: "CargaSAPOrdenCompraProductoNacional.aspx/HoraSesion",
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(msg) {                
                    if(msg.d != "")
                    $("#lblHoraActualizacion").text(msg.d);
                }
            });
            setTimeout("ActualizarSession()", 600000);
        }
        
        function DatosConfiguracion(config) {            
            var configArray = config.split("_");
            this.centro = configArray[0];
            this.almacen = configArray[1];
            this.cantidad = configArray[2];
            this.material = configArray[3];
        }
        function validarChk() {
            var validacion = true;
            var chks = $("#gvDatos .chkOrdenSeleccionarDes input:checked");
            var valCantidad, valAlmacen, valUnidadMedida, cajaCantidad, dllAlmacen, ddlUnidadMedida,configDatosIngresados,configDatosRecepcion;
            $.each(chks, function(key, value) {
                cajaCantidad = $(this).parents("tr").find('input:visible[id*="txtCantidad"]');
                cajaCantidadACargar = $(this).parents("tr").find('input:visible[id*="txtCantidadACargar"]');
                dllAlmacen = $(this).parents("tr").find('select[id*="ddlAlmacen"]');
                ddlUnidadMedida = $(this).parents("tr").find('select[id*="ddlUnidadMedida"]');
                valCantidad = $(this).parents("tr").find('.txtCantidadError');
                valCantidadACargar = $(this).parents("tr").find('.txtCantidadACargarError');
                valAlmacen = $(this).parents("tr").find('.ddlAlmacenError');
                valUnidadMedida = $(this).parents("tr").find('.ddlUnidadMedidaError');
                
                configDatosIngresados = new DatosConfiguracion($(this).parents("tr").find('input[id$="hfMaterialCentroCantidad"]').val());
                configDatosIngresados.cantidad = $(this).parents("tr").find('input[id$="txtCantidad"]').val();
                
                configDatosRecepcion = new DatosConfiguracion($("#gvInfo").find('input[value*="' + configDatosIngresados.material + '"]').val());
                if (configDatosIngresados.cantidad > 0) {
                    if (!validarCantidad(cajaCantidad, valCantidadACargar, configDatosRecepcion.cantidad))
                        validacion = false;
                }
                if (cajaCantidad.length > 0) {
                    if (!validarCantidad(cajaCantidad, valCantidad))
                        validacion = false;
                }
                if (cajaCantidadACargar.length > 0) {
                    if (!validarCantidad(cajaCantidadACargar, valCantidadACargar))
                        validacion = false;
                }
                if (dllAlmacen.length > 0) {
                    if (!validarAlmacen(dllAlmacen, valAlmacen))
                        validacion = false;
                }
                if (ddlUnidadMedida.length > 0) {
                    if (!validarUnidadMedida(ddlUnidadMedida, valUnidadMedida))
                        validacion = false;
                }
            });
            var mensaje = $(".errorMsn");
            if (chks.length > 0) {
                mensaje.hide();
            } else {
                mensaje.show();
                return false;
            }
            if (validacion)
                return confirm("Esta seguro de cargar en SAP los materiales seleccionados");
            else
                return false;
        }
        function validarCantidad(caja, validador, cantidadRecepcion) {
            var retorno = true;
            var exp_reg = /[0-9]+/;
            if (caja.val() == "") {
                validador.text("Por favor ingrese la cantidad.");
                retorno = false;
            } else if (!exp_reg.test(caja.val())) {
                validador.text("Por favor ingrese un número valido.");
                retorno = false;
            } else if (!(parseInt(caja.val()) > 0)) {
                validador.text("Por favor ingrese un número mayor de 0.");
                retorno = false;
            }
            if (cantidadRecepcion) {                
                if (parseInt(caja.val()) > parseInt(cantidadRecepcion)) {                    
                    validador.text("La cantidad no puede ser mayor que la de recepción.");
                    retorno = false;
                }
            }
            
            if (retorno)
                validador.hide();
            else
                validador.show();
            return retorno;
        }
        function validarAlmacen(lista, validador) {
            var retorno = true;
            if (!(lista.val() != 0)) {
                validador.text("Seleccione un almacen por favor.");
                retorno = false;
                validador.show();
            } else
                validador.hide();
            return retorno;
        }
        function validarUnidadMedida(lista, validador) {
            var retorno = true;
            if (!(lista.val() != 0)) {
                validador.text("Seleccione una unidad de medida por favor.");
                retorno = false;
                validador.show();
            } else
                validador.hide();
            return retorno;
        }        
    </script>

    <style type="text/css">
        .divTitulo
        {
        	font-size:12px;
        }
        .contenedorOpciones
        {
            float: left;
            width: 49%;
        }
        .contenedorOpciones .tablaGris
        {
            width: 100%;
        }
        .tablaLeft
        {
            float: left;
            /*width: 40%;*/
        }
        .error
        {
        	color:Red;
        }
        .ok
        {
        	color:Green;
        }
        .paginador span
        {
        	font-weight: bold;
            text-decoration: underline;
        }
    </style>
</head>
<body class="cuerpo2">
    <form id="frmPrincipal" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="20000">
    </asp:ScriptManager>
    
    <asp:UpdatePanel runat="server" ID="upGeneral">
    <ContentTemplate>
    <uc1:EncabezadoPagina ID="epPrincipal" runat="server" />
    <asp:Label ID="lblHoraActualizacion" runat="server" ></asp:Label>    
    <asp:Panel ID="pnlReimprimirDocumento" runat="server">
        <asp:Button ID="btnReimprimirDocumento" runat="server" Text="Reimprimir Documento" CssClass="search" />
    </asp:Panel>
    
    <div id="pnlContenedorOpciones" runat="server">
        <asp:HiddenField ID="hfIdOrdenRecepcion" runat="server" />
        <div class="contenedorOpciones">
            <h1>
                Datos de la Orden de Compra</h1>
            <table class="tablaGris">
                <tr>
                    <td class="field">
                        Numero de Orden:
                    </td>
                    <td>
                        <asp:Label ID="lblNumeroOrden" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="field">
                        Texto cabecera:
                    </td>
                    <td>
                        <asp:Label ID="lblTextoCabecera" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Remisión:
                    </td>
                    <td>
                        <asp:Label ID="lblRemision" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="field">
                        Nota Entrega:
                    </td>
                    <td>
                        <asp:Label ID="lblNotaEntrega" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="field">
                        Observación:
                    </td>
                    <td>
                        <asp:Label ID="lblObservacion" runat="server" Text=""></asp:Label>
                    </td>
                    <td class="field">Identificador de Recepción:</td>
                    <td>
                        <asp:Label ID="lblIdOrdenRecepcion" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </div><br />
        <div style="clear: both;height:20px;">
        </div>
        <div style="width: 100%;">
            <asp:UpdatePanel ID="upPrincipal" runat="server">
            <ContentTemplate>
            <asp:GridView ID="gvDatos" CssClass="tablaGris tablaLeft" runat="server" 
                AutoGenerateColumns="False"                                 
                    Caption="&lt;div class=&quot;divTitulo&quot;&gt;Datos de la Orden de Compra en SAP&lt;/div&gt;" 
                    DataKeyNames="material,centro">
                <Columns>
                    <asp:TemplateField HeaderText="Opc.">
                        <HeaderTemplate>
                            <input id="chkEncabezadoGenerar" type="checkbox" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox CssClass="chkOrdenSeleccionarDes" ID="chkAgregar" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="posicionContable" HeaderText="Posición">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="material" HeaderText="Material" />
                    <asp:BoundField DataField="referencia" HeaderText="Descripción Material" />
                    <asp:TemplateField HeaderText="Cantidad">
                        <ItemTemplate>
                            <asp:TextBox ID="txtCantidad" runat="server" MaxLength="8" Width="80" OnTextChanged="ActualizarCantidad" ></asp:TextBox>                           
                            <div class="txtCantidadError error" style="display: none;">
                            </div>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unidad de Medida">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlUnidadMedida" AutoPostBack="true" OnTextChanged="ActualizarCantidad" runat="server">
                            </asp:DropDownList>
                            <div class="ddlUnidadMedidaError error" style="display: none;">
                            </div>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cantidad a Cargar" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <asp:Label ID="lblCantidadACargar" runat="server" Text='<%# Bind("cantidadPendiente") %>'></asp:Label>
                            <asp:TextBox ID="txtCantidadACargar" runat="server" Width="80px" Visible="false" ></asp:TextBox>
                            <div class="txtCantidadACargarError error" style="display: none;">
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                    </asp:TemplateField>                    
                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad Pendiente" ItemStyle-Width="70px">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="unidadMedida" HeaderText="Unidad" />
                    <asp:BoundField DataField="centro" HeaderText="Centro" />
                    <asp:TemplateField HeaderText="Almacen" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlAlmacen" runat="server">
                            </asp:DropDownList>
                            <div class="ddlAlmacenError error" style="display: none;">
                            </div>
                            <asp:HiddenField ID="hfMaterialCentroCantidad" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="70px"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle Height="40px" />
            </asp:GridView>
            </ContentTemplate>
            </asp:UpdatePanel>
            
            <asp:GridView ID="gvInfo" CssClass="tablaGris tablaLeft" AutoGenerateColumns="False"
                runat="server" 
                Caption="&lt;div class=&quot;divTitulo&quot;&gt;Detalle de la Orden de Recepci&oacute;n&lt;/div&gt;" 
                DataKeyNames="material,centro">
                <Columns>
                    <asp:BoundField DataField="productoPadre" HeaderText="Producto" />
                    <asp:BoundField DataField="material" HeaderText="Material" />
                    <asp:BoundField DataField="cantidad" HeaderText="Cantidad">
                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Cantidad Pendiente por Cargue" >
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkCantidadPendiente" CommandArgument='<%# Bind("idOrdenRecepcion") %>'
                                CommandName="verSerialesPendientes" 
                                ToolTip="Ver Seriales Pendientes por Cargar" runat="server" 
                                Text='<%# Bind("cantidadPendienteCargar") %>' ForeColor="Blue"></asp:LinkButton>
                            <asp:HiddenField ID="hfMaterialCentroCantidad" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="70px" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="codigoEmpaque" HeaderText="Unidad Medida" />
                    <asp:BoundField DataField="centro" HeaderText="Centro">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="almacen" HeaderText="Almacen">
                        <ItemStyle Width="50px" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Opciones">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgVerSeriales" runat="server" 
                                CommandArgument='<%# Bind("idOrdenRecepcion") %>'
                                CommandName="verSerialesCargados" ImageUrl="~/images/view.png" ToolTip="Ver Seriales Cargados" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle Height="40px" />
            </asp:GridView>
            <div style="clear: both;">
            </div>
        </div>
        <div>
            <div class="error errorMsn" style="display: none;">
                Por favor, seleccione al menos una posición.</div>
            <br />
            <asp:Button ID="btnCargar" CssClass="search" runat="server" Text="Cargar" ValidationGroup="cargaSAP"
                OnClientClick="return validarChk()" />
            
        </div>
        <div style="padding-top: 15px;">
            <br />
            <asp:GridView ID="gvDocumentosCargados" runat="server" CssClass="tablaGris" AutoGenerateColumns="False" Style="border: 2px solid Green;">
                <Columns>
                    <asp:BoundField DataField="indice" HeaderText="Indice" />
                    <asp:BoundField DataField="lote" HeaderText="Lote" />
                    <asp:BoundField DataField="noDocumento" HeaderText="No Documento" />
                </Columns>
            </asp:GridView>
            <asp:GridView ID="gvErrores" CssClass="tablaGris" AutoGenerateColumns="False" runat="server"
                Style="border: 2px solid red;">
                <Columns>
                    <asp:BoundField DataField="indice" HeaderText="Indice">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="descripcion" HeaderText="Mensaje" />
                </Columns>
            </asp:GridView>
            <asp:GridView ID="gvInfoWS" CssClass="tablaGris" AutoGenerateColumns="False" runat="server"
                Style="border: 2px solid orange;">
                <Columns>
                    <asp:BoundField DataField="indice" HeaderText="Indice">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="descripcion" HeaderText="Error" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    
    <asp:HiddenField ID="hfControlPopUpReimprimirDocumento" runat="server" />    
    
    <cc1:ModalPopupExtender ID="mpeReimpresionDocumento" PopupControlID="pnlReimpresionDocumento"
        runat="server" TargetControlID="hfControlPopUpReimprimirDocumento" CancelControlID="imgBtnCerrarPopUpReimpresionDocumento" BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <asp:Panel ID="pnlReimpresionDocumento" runat="server" CssClass="modalPopUp" style="width:700px;display:none;">
        <div style="text-align: right;">
            <asp:ImageButton ID="imgBtnCerrarPopUpReimpresionDocumento" runat="server" ImageUrl="~/images/cerrar.gif" />
        </div>
        <div>
            <div style="text-align:center;margin-bottom:5px;">
                <label style="font-size:18px;font-weight:bold;">Reimpresion de Documento</label>    
            </div>
            <div>
                <label>Número de Documento:</label>
                <asp:TextBox ID="txtNumeroDocumento" runat="server" MaxLength="15" ValidationGroup="vgReimprimir"></asp:TextBox>
                &nbsp;<label>Año ejercicio:</label>
                <asp:TextBox ID="txtYearEjercicion" runat="server" MaxLength="4" ValidationGroup="vgReimprimir"></asp:TextBox>
                <div>
                    <div>
                        <asp:RegularExpressionValidator ID="rglYearEjercicion" runat="server" ErrorMessage="El campo año ejercicio es numérico. Digite un año válido, por favor"
                            ControlToValidate="txtYearEjercicion" ValidationGroup="vgReimprimir" Display="Dynamic" ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvNumeroDocumento" runat="server" ErrorMessage="Ingrese el numero de documento, por favor." 
                            ControlToValidate="txtNumeroDocumento" Display="Dynamic" ValidationGroup="vgReimprimir"></asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvYearEjercicion" runat="server" ErrorMessage="Ingrese el año ejercicio, por favor." 
                            ControlToValidate="txtYearEjercicion" Display="Dynamic" ValidationGroup="vgReimprimir"></asp:RequiredFieldValidator>
                    </div>
                    <div>
                        <asp:Label ID="lblRespuesta" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
            <div>
                <asp:Button ID="btnReImprimir" runat="server" Text="Reimprimir" CssClass="search" ValidationGroup="vgReimprimir" />
            </div>
        </div>
    </asp:Panel>
    
    
    </ContentTemplate>        
        <Triggers>
            <asp:PostBackTrigger ControlID="gvInfo" />            
        </Triggers>
    </asp:UpdatePanel>    
    <uc2:ModalProgress ID="ModalProgress1" runat="server" />
    </form>
</body>
</html>
