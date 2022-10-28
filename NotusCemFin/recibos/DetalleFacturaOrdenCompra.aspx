<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DetalleFacturaOrdenCompra.aspx.vb" Inherits="BPColSysOP.DetalleFacturaOrdenCompra" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<%@ Register src="../ControlesDeUsuario/ModalProgress.ascx" tagname="ModalProgress" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Detalle de factura</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function esRangoValido(source, arguments) {
            try {
                if (document.getElementById("txtFechaSalida").value.trim() != "" || document.getElementById("txtFechaEsperadaArribo").value.trim() != "") {
                    if (document.getElementById("txtFechaSalida").value.trim() != "" && document.getElementById("txtFechaEsperadaArribo").value.trim() == "") {
                        arguments.IsValid = false;
                    } else {
                        if (document.getElementById("txtFechaSalida").value.trim() == "" && document.getElementById("txtFechaEsperadaArribo").value.trim() != "") {
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
    </script>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnableScriptGlobalization="true" runat="server">
    </asp:ScriptManager>
    <div>
    <asp:UpdatePanel ID="upCabecera" runat="server">
        <ContentTemplate>
            <uc1:EncabezadoPagina ID="EncabezadoPagina" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
    <div id="pnlGuiasAgregadas" style="width:600px;">
        <p class="subtitulo" style="width:100%;">Guias Agregadas</p>
        <asp:UpdatePanel ID="upGvGuiasAgregadas" runat="server">
        <ContentTemplate>                
        <asp:GridView ID="gvGuiasAgregadas" runat="server" CssClass="tablaGris" 
            Width="100%" AutoGenerateColumns="False" EmptyDataText="No hay datos">
            <Columns>
                <asp:BoundField DataField="guia" HeaderText="No. Guia" >
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="transportadora" HeaderText="Transportadora" />
                <asp:BoundField DataField="CantidadFacGui" HeaderText="Cantidad">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad de Origen" />
                <asp:BoundField DataField="fechaSalida" HeaderText="Fecha de Salida" 
                    DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="fechaEsperadaArribo" 
                    HeaderText="Fecha de Espera Arribo" DataFormatString="{0:dd/MM/yyyy}" />
                <asp:BoundField DataField="pesoNeto" HeaderText="Peso Neto(Kg)" >
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="pesoBruto" HeaderText="Peso Bruto(Kg)" >
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="Opciones">
                    <ItemTemplate>
                        <asp:ImageButton ID="imgBtnEditarGuia" runat="server" CommandName="editarGuia" CommandArgument='<%# Bind("idGuia") %>'
                            ImageUrl="~/images/Edit-32.png" ToolTip="Editar Guia" />
                        <asp:ImageButton ID="imgBtnEliminarGuia" runat="server" CommandName="eliminarGuia" CommandArgument='<%# Bind("idGuia") %>'
                            ImageUrl="~/images/Delete-32.png" ToolTip="Eliminar Guia" />
                        <cc1:ConfirmButtonExtender ID="cbeEliminarGuia" 
                                    runat="server" TargetControlID="imgBtnEliminarGuia" ConfirmText="Esta seguro de eliminar esta guia?">
                        </cc1:ConfirmButtonExtender>
                        <asp:ImageButton ID="imgBtnEliminarRelacionFacGuia" runat="server" CommandName="eliminarRelacionFacGuia" CommandArgument='<%# Bind("idGuia") %>'
                            ImageUrl="~/images/remove.png" OnClientClick="return confirm('Realmente desea desvincular esta guia de la factura?');" ToolTip="Desvincular la guia de la factura." />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hfIdOrdenCompra" runat="server" />
        <asp:HiddenField ID="hfIdFactura" runat="server" />        
        <cc1:ModalPopupExtender ID="mpeEditarGuia" runat="server" 
            PopupControlID="pnlEditarGuia" BackgroundCssClass="modalBackground"
             TargetControlID="hfValidarCierre">
        </cc1:ModalPopupExtender>       
    <asp:HiddenField ID="hfValidarCierre" runat="server" />
    <asp:Panel ID="pnlEditarGuia" runat="server" CssClass="modalPopUp" style="display:none;"  >
    
    <asp:UpdatePanel ID="upPnlEditarGuia" runat="server">
            <ContentTemplate>
        <div style="width: 600px;">
            <uc1:EncabezadoPagina ID="EncabezadoFacGuia" runat="server" />
        </div>
        <asp:HiddenField ID="hfIdGuiaEdicionActual" runat="server" />
        <div style="text-align: right">
            <asp:ImageButton ID="imgBtnCerrarPopUp" runat="server" ImageUrl="~/images/cerrar.gif" /></div>
        <table class="tablaGris" width="600">
            <tr>
                <th colspan="2" align="center">
                    Editar Guia No:
                    <asp:Label ID="lblGuia" runat="server" Text=""></asp:Label>
                </th>
            </tr>
            <tr>
                <td>
                    Transportadora:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTransportadora" runat="server" ValidationGroup="EditarInfoGuia"
                        Enabled="False">
                    </asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvTransportadora" runat="server" ControlToValidate="ddlTransportadora"
                            Display="Dynamic" InitialValue="0" ErrorMessage="Escoja la transportadora" ValidationGroup="EditarInfoGuia"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    Cantidad:
                </td>
                <td>
                    <asp:TextBox ID="txtCantidad" runat="server" MaxLength="8"></asp:TextBox>
                    <asp:Label ID="lblCantidadPermitidaPorFactura" runat="server" Text="" CssClass="comentario"></asp:Label>    
                    <div>
                        <asp:RequiredFieldValidator ID="rfvCantidad" runat="server" ControlToValidate="txtCantidad"
                            Display="Dynamic" ErrorMessage="Escoja la transportadora" ValidationGroup="EditarInfoGuia"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revCantidad" runat="server" ErrorMessage="Ingrese la cantidad"
                            ControlToValidate="txtCantidad" ValidationGroup="EditarInfoGuia" Display="Dynamic"
                            ValidationExpression="[0-9]+"></asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="cvCantidad" runat="server" ControlToValidate="txtCantidad" Display="Dynamic" ValidationGroup="EditarInfoGuia" 
                            ValueToCompare="0" Operator="GreaterThan" ErrorMessage="La cantidad debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    Pais:
                </td>
                <td>
                    <asp:DropDownList ID="ddlPaisFacGuia" runat="server" ValidationGroup="EditarInfoGuia"
                        AutoPostBack="True">
                    </asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvPaisFacGuia" runat="server" ControlToValidate="ddlPaisFacGuia"
                            InitialValue="0" ValidationGroup="EditarInfoGuia" ErrorMessage="Seleccione el pais"
                            Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    Ciudad de Origen:
                </td>
                <td>
                    <asp:DropDownList ID="ddlCiudadOrigen" runat="server" ValidationGroup="EditarInfoGuia">
                    </asp:DropDownList>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvCiudadOrigen" runat="server" ControlToValidate="ddlCiudadOrigen"
                            Display="Dynamic" InitialValue="0" ErrorMessage="Seleccione la ciudad de origen"
                            ValidationGroup="EditarInfoGuia"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    Fecha de Salida:
                </td>
                <td>
                    <asp:TextBox ID="txtFechaSalida" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFechaSalida_CalendarExtender" runat="server" PopupButtonID="imgFechaSalida"
                        Format="dd/MM/yyyy" CssClass="calendarTheme" TargetControlID="txtFechaSalida">
                    </cc1:CalendarExtender>
                    <img src="../images/date-32.png" id="imgFechaSalida" alt="Fecha de salida" title="Fecha de salida" />
                    <div>
                        <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaSalida" runat="server" ErrorMessage="Fecha no válida." ValidationGroup="EditarInfoGuia"
                                ControlToValidate="txtFechaSalida" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>   
                        <asp:RequiredFieldValidator ID="rfvFechaSalida" runat="server" ControlToValidate="txtFechaSalida"
                            Display="Dynamic" ErrorMessage="Indique la fecha de salida" ValidationGroup="EditarInfoGuia"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    Fecha Esperada Arribo:
                </td>
                <td>
                    <asp:TextBox ID="txtFechaEsperadaArribo" runat="server"></asp:TextBox>
                    <cc1:CalendarExtender ID="txtFechaEsperadaArribo_CalendarExtender" runat="server"
                        PopupButtonID="imgFechaEsperadaArribo" Format="dd/MM/yyyy" CssClass="calendarTheme"
                        TargetControlID="txtFechaEsperadaArribo">
                    </cc1:CalendarExtender>
                    <img src="../images/date-32.png" id="imgFechaEsperadaArribo" alt="Fecha esperada de arribo"
                        title="Fecha esperada de arribo" />
                    <div>
                        <asp:RegularExpressionValidator Display="Dynamic" ID="revFechaEsperadaArribo" runat="server" ErrorMessage="Fecha no válida." ValidationGroup="EditarInfoGuia"
                                ControlToValidate="txtFechaEsperadaArribo" ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((1[6-9]|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((1[6-9]|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"></asp:RegularExpressionValidator>                        
                        <asp:CompareValidator ID="cvRangoFecha" runat="server" ControlToCompare="txtFechaSalida"
                                    ControlToValidate="txtFechaEsperadaArribo" ErrorMessage="La Fecha de salida no debe ser mayor a la fecha de arribo"
                                    Operator="GreaterThanEqual" Type="Date" Display="Dynamic" ValidationGroup="EditarInfoGuia"></asp:CompareValidator>                      
                        <asp:CustomValidator ID="cusRango" runat="server" ErrorMessage="Es necesario especificar los dos valores de las fechas"
                            Display="Dynamic" ClientValidationFunction="esRangoValido" ValidationGroup="EditarInfoGuia"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="rfvFechaEsperadaArribo" runat="server" ControlToValidate="txtFechaEsperadaArribo"
                            Display="Dynamic" ErrorMessage="Indique la Fecha Esperada de Arribo" ValidationGroup="EditarInfoGuia"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    Peso Neto(Kg):
                </td>
                <td>
                    <asp:TextBox ID="txtPesoNeto" runat="server" ValidationGroup="EditarInfoGuia" MaxLength="18"></asp:TextBox><label
                        class="comentario">Formato ###,##</label>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvPesoNeto" runat="server" ControlToValidate="txtPesoNeto"
                            Display="Dynamic" ErrorMessage="Ingrese el peso neto" ValidationGroup="EditarInfoGuia"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rglPesoNeto" runat="server" ErrorMessage="Ingrese el formato de peso indicado"
                            ControlToValidate="txtPesoNeto" ValidationGroup="EditarInfoGuia" Display="Dynamic"
                            ValidationExpression="^(\d{1,6})(,\d{2})*$"></asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="cvPesoNeto" runat="server" ControlToValidate="txtPesoNeto" Display="Dynamic" ValidationGroup="EditarInfoGuia" 
	                        ValueToCompare="0" Operator="GreaterThan" ErrorMessage="El peso neto debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    Peso Bruto(Kg):
                </td>
                <td>
                    <asp:TextBox ID="txtPesoBruto" runat="server" ValidationGroup="EditarInfoGuia" MaxLength="18"></asp:TextBox>
                    <label class="comentario">
                        Formato ###,##</label>
                    <div>
                        <asp:RequiredFieldValidator ID="rfvPesoBruto" runat="server" ControlToValidate="txtPesoBruto"
                            Display="Dynamic" ErrorMessage="Ingrese el peso bruto" ValidationGroup="EditarInfoGuia"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rglPesoBruto" runat="server" ErrorMessage="Ingrese el formato de peso indicado"
                            ControlToValidate="txtPesoBruto" ValidationGroup="EditarInfoGuia" Display="Dynamic"
                            ValidationExpression="^(\d{1,6})(,\d{2})*$"></asp:RegularExpressionValidator>
                        <asp:CompareValidator ID="cvPesoBruto" runat="server" ControlToValidate="txtPesoBruto" Display="Dynamic" ValidationGroup="EditarInfoGuia" 
	                                ValueToCompare="0" Operator="GreaterThan" ErrorMessage="El peso bruto debe ser mayor de 0, por favor verifique."></asp:CompareValidator>
                    </div>
                </td>
            </tr>
        </table>
        <div>
            <asp:Button ID="btnEditarGuia" runat="server" Text="Editar" CssClass="boton" ValidationGroup="EditarInfoGuia" />&nbsp;&nbsp;
        </div>
        </ContentTemplate>        
        </asp:UpdatePanel>
     
    </asp:Panel>
    </div>
    <uc2:ModalProgress ID="ModalProgress1" runat="server" />
    </form>
</body>
</html>
