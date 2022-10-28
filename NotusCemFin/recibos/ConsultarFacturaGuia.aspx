<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConsultarFacturaGuia.aspx.vb" Inherits="BPColSysOP.ConsultarFacturaGuia" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ModalProgress.ascx" TagName="ModalProgress" TagPrefix="uc2" %>


<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Consultar Porcentaje Muestreo</title>
    <link rel="stylesheet" type="text/css" href="../include/styleBACK.css" />
    <script type="text/javascript" src="../include/jquery-1.js" ></script>
    <script language="javascript" type="text/javascript">
        function ValidarSeleccionFiltros(source, arguments) {
            try {
                var idFactura = document.getElementById("ddlFactura").value;
                var idGuia = document.getElementById("ddlGuia").value;

                if (idFactura == 0 || idGuia == 0) {
                    arguments.IsValid = false;
                } else {
                    arguments.IsValid = true;
                }
            } catch (e) {
                alert(e.description);
                arguments.IsValid = false;
            }
        }

        function RefrescaUpdatePanel(nombreCajaTexto, flagFiltrado) {
            var filtro = $get(nombreCajaTexto).value;
            var patron = new RegExp("^\s*[a-zA-Z_0-9 ,\s áéíóúÁÉÍÓÚ, \s -]+\s*$");
            if (patron.test(filtro)) {
                if (filtro.length > 3) {
                    $get(flagFiltrado).value = 1;
                    __doPostBack(nombreCajaTexto, '');
                    $find(ModalProgress).hide();
                }
                else if (filtro.length <= 3 && $get(flagFiltrado).value == 1) {
                $get(flagFiltrado).value = 0
                    __doPostBack(nombreCajaTexto, '');
                    $find(ModalProgress).hide();
                }
            }
            else if ($get(nombreCajaTexto).value != "") { alert("los caracteres especiales no son permitidos") }
        }

        function validarSoloNumero(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        function makeSubmit(btn) {
            if (event.keyCode == 13) {
                event.returnValue = false;
                event.cancel = true;
                btn.click();
            }
        }
    </script>
    
</head>
<body class="cuerpo2">
    <form id="fmrMuestreo" runat="server">
        <asp:ScriptManager ID="smMuestreo" runat="server" EnableScriptGlobalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="uplEncabezado" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <uc1:EncabezadoPagina ID="epMuestreo" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <div id="lyrBusquedaFactura">
            <table class="tablaGris">
                <tr>
                    <th colspan="2">
                        CONSULTAR POR FACTURA - GUIA
                    </th>
                </tr>
                <tr>
                    <td>
                        <table style="padding: 2px 0;">
                            <tr>
                                <td class="field">Factura:</td>
                                <td>
                                    <asp:HiddenField ID="hfFlagFiltrado" runat="server" />        
                                    <asp:TextBox ID="txtFiltroFactura" runat="server" Width="127px" size="10" 
                                        MaxLength="20" onkeyup="RefrescaUpdatePanel('txtFiltroFactura','hfFlagFiltrado');"
                                    OnTextChanged="FiltrarFactura"></asp:TextBox>-
                                    <asp:UpdatePanel ID="uplFactura" runat="server" RenderMode="Inline">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlFactura" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtFiltroFactura"/>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td class="field">Guía:</td>                                
                                <td>
                                    <asp:HiddenField ID="hfFlagFiltradoGuia" runat="server" />        
                                    <asp:TextBox ID="txtFiltroGuia" runat="server" Width="100px" size="10" MaxLength="15" onkeyup="RefrescaUpdatePanel('txtFiltroGuia','hfFlagFiltradoGuia');"
                                    OnTextChanged="FiltrarGuia"></asp:TextBox>-
                                    <asp:UpdatePanel ID="uplGuia" runat="server" RenderMode="Inline">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlGuia" runat="server" AutoPostBack="True">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="txtFiltroGuia"/>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="display: block;">
                            <asp:CustomValidator ID="cvFiltros" runat="server" ClientValidationFunction="ValidarSeleccionFiltros" ErrorMessage="Debe seleccionar factura y guía." 
                            Display="Dynamic" ValidationGroup="filtros"></asp:CustomValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align = "center">
                        <asp:UpdatePanel ID="uplConsultar" runat="server" RenderMode="Inline">
                            <ContentTemplate>
                                <asp:Button ID="btnConsultar" CssClass="boton" Text="Consultar" runat="server" ValidationGroup="filtros" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>

        <br />
        <asp:UpdatePanel ID="uplInfo" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <asp:HiddenField ID="hfIdFacturaGuia" runat="server" />
                <asp:Panel ID="pnlFactura" runat="server" AddCallBacks="false" AutoUpdateAfterCallBack="true" >
                    <div id="lyrInfoFactura" style="float:left;">
                    <table class="tablaGris">
                        <tr>
                            <th colspan="2">
                                Información General de la Factura
                            </th>
                        </tr>                
                        <tr >
                            <td class="field">Factura:</td>
                            <td>
                                <asp:Label ID="lblFactura" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr >
                            <td class="field">Guia:</td>
                            <td>
                                <asp:Label ID="lblGuia" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr >
                            <td class="field">Cantidad:</td>
                            <td>
                                <asp:Label ID="lblCantidad" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr >
                            <td class="field">Orden de Compra:</td>
                            <td>
                                <asp:Label ID="lblCompra" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr >
                            <td class="field">Producto:</td>
                            <td>
                                <asp:Label ID="lblProducto" runat="server"></asp:Label>
                            </td>
                        </tr>                        
                        <tr >
                            <td class="field">Porcentaje Muestreo:</td>
                            <td>
                                <asp:TextBox ID="txtMuestreo" size="5" MaxLength="2" onkeydown="makeSubmit(btnActualizar);" onkeypress="return validarSoloNumero(event);" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="revMuestreo" runat="server" ControlToValidate="txtMuestreo" 
                                Display="Dynamic" ErrorMessage="El valor debe ser numerico." ValidationGroup="Muestreo"
                                ValidationExpression="\d+"></asp:RegularExpressionValidator>                                
                                <asp:Label ID="lblMuestreo" runat="server" Visible="false" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: block;">
                                    <asp:RequiredFieldValidator ID="rfvMuestreo" runat="server" ControlToValidate="txtMuestreo" ErrorMessage="El porcentaje de muestreo es obligatorio." 
                                    Display="Dynamic" ValidationGroup="Muestreo"></asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2" >
                                <asp:Button ID="btnActualizar" CssClass="boton" Text="Actualizar" runat="server" ValidationGroup="Muestreo" />
                            </td>
                        </tr>
                    </table>
                    </div>
                    <div style="float:left; padding:5px 0px 0px 5px" >
                        <asp:LinkButton ID="lkbCerrar" CssClass="search" ToolTip="Ver seriales de Muestreo" ForeColor="DarkBlue" Visible="false"
                        Font-Bold="true" Font-Underline="true" PostBackUrl="~/recibos/ConsultarSerialesMuestreo.aspx"  runat="server">Ver Seriales</asp:LinkButton>                                            
                    </div>
                    <div style="clear:both"></div>
                </asp:Panel>   
            </ContentTemplate>
        </asp:UpdatePanel>
        <uc2:ModalProgress ID="mpMuestreo" runat="server" />
    </form>
</body>
</html>
