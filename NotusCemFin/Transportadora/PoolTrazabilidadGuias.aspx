<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PoolTrazabilidadGuias.aspx.vb" Inherits="BPColSysOP.PoolTrazabilidadGuias" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>::Pool Trazabilidad Guias::</title>
    <link rel="shortcut icon" href="../images/baloons_small.png" />
    <link href="../include/styleBACK.css" type="text/css" rel="stylesheet" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script src="../include/JavaScriptFunctions.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Consultar(s, e) {
            LoadingPanel.Show();

            cpPrincipal.PerformCallback('200');
            LoadingPanel.Hide();
        }
        function solonumeros(e) {
            var key = window.event ? e.which : e.keyCode;
            if (key < 48 || key > 57) {
                e.preventDefault();
            }

        }
        function LimpiaFormulario() {
            if (confirm("¿Realmente desea limpiar los campos del formulario?")) {
                txtRadicado.SetText('');
                txtPedido.SetText('');
                txtGuia.SetText('');
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallbackPanel ID="cpPrincipal" ClientInstanceName="cpPrincipal" runat="server">
                <%--<ClientSideEvents EndCallback="function(s,e){
                    
                    LoadingPanel.Hide();
                    
                }" />--%>
                <PanelCollection>
                    <dx:PanelContent>
                        <div id="divEncabezadoPrincipal">
                            <uc1:EncabezadoPagina ID="epPrincipal" runat="server" />
                        </div>
                        <dx:ASPxRoundPanel ID="roundPanelFiltros" runat="server" ClientInstanceName="roundPanelFiltros" HeaderText="Filtros de Búsqueda" Width="1100px" Theme="SoftOrange">
                            <PanelCollection>
                                <dx:PanelContent>

                                    <table style="width: 100%">

                                        <tr>
                                            <td class="field">Numero Radicado:</td>
                                            <td>
                                                <dx:ASPxTextBox ID="txtRadicado" runat="server" ClientInstanceName="txtRadicado" onkeypress="solonumeros(event);" Theme="SoftOrange" Width="200px" MaxLength="18" ValidationGroup="vgBuscar" NullText="Numero Radicado">
                                                    <ValidationSettings>
                                                        <RegularExpression ErrorText="Solo Valores numericos" ValidationExpression="\d+" />
                                                    </ValidationSettings>
                                                </dx:ASPxTextBox>
                                            </td>

                                            <td class="field">Pedido o Factura:</td>
                                            <td>
                                                <dx:ASPxTextBox ID="txtPedido" runat="server" ClientInstanceName="txtPedido" Theme="SoftOrange" Width="200px">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="field">Guia:</td>
                                            <td>
                                                <dx:ASPxTextBox ID="txtGuia" runat="server" ClientInstanceName="txtGuia" Theme="SoftOrange" Width="200px">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td rowspan="2">
                                                <div style="float: right">
                                                    <dx:ASPxImage ID="imgBuscar" runat="server" ValidationGroup="vgBuscar" ImageUrl="../images/DxConfirm32.png" TabIndex="6"
                                                        ToolTip="Búsqueda" ClientInstanceName="imgBuscar"  Cursor="pointer">
                                                        <ClientSideEvents Click="function(s,e){
                                                             if(txtRadicado.GetValue() || txtPedido.GetValue() || txtGuia.GetValue())
                                                             { if (ASPxClientEdit.ValidateGroup('vgBuscar')){
                                                                Consultar(s,e);
                                                             }}
                                                             else
                                                             {
                                                                alert('Debe seleccionar por lo menos un filtro de búsqueda.')
                                                             }
                                                             }" />
                                                    </dx:ASPxImage>
                                                    <div>
                                                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Filtrar" CssClass="comentario"></dx:ASPxLabel>
                                                    </div>
                                                </div>
                                            </td>

                                            <td rowspan="2">
                                                <div style="float: left">
                                                    &nbsp;&nbsp;<dx:ASPxImage ID="imgBorrar" runat="server" ImageUrl="../images/DxCancel32.png" ToolTip="Borrar Filtros" ClientInstanceName="imgBuscar" TabIndex="7" Cursor="pointer">
                                                        <ClientSideEvents Click="function(s, e){
                                                            LimpiaFormulario();
                                                        }" />
                                                    </dx:ASPxImage>
                                                    <div>
                                                        <dx:ASPxLabel ID="lblComentarioBorrar" runat="server" Text="Borrar" CssClass="comentario">
                                                        </dx:ASPxLabel>
                                                    </div>
                                                </div>

                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>
                        <br />
                        <dx:ASPxRoundPanel ID="rpConsulta" runat="server" ClientInstanceName="rpConsulta" HeaderText="Información" Width="93%" Theme="SoftOrange">
                            <PanelCollection>
                                <dx:PanelContent>
                                    <dx:ASPxPanel ID="pnlExportar" runat="server" ClientInstanceName="pnlExportar" ClientVisible="True">
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <dx:ASPxButton ID="lbExportar" runat="server" ClientInstanceName="lbExportar" Text="Exportar " AutoPostBack="false">
                                                                <Image Url="~/images/Excel.gif">
                                                                </Image>
                                                            </dx:ASPxButton>
                                                            <dx:ASPxButton ID="btnExcel" runat="server" ClientInstanceName="btnExcel" Text="Exportar " Visible="false">
                                                                <Image Url="~/images/Excel.gif">
                                                                </Image>
                                                            </dx:ASPxButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dx:ASPxPanel>
                                    <dx:ASPxGridView ID="gvTrazabilidadGuia" runat="server" Width="100%" AutoGenerateColumns="False" ClientInstanceName="gvTrazabilidadGuia"
                                        KeyFieldName="guia" Font-Size="Small" Theme="SoftOrange">
                                        <SettingsBehavior AllowGroup="false" AllowDragDrop="false" />
                                        <Columns>
                                            <dx:GridViewDataTextColumn FieldName="tipo" Caption="Tipo" ShowInCustomizationForm="True" VisibleIndex="1">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="numeroRadicado" Caption="Número Radicado" ShowInCustomizationForm="True" VisibleIndex="2">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="guia" Caption="Número Guia" ShowInCustomizationForm="True" VisibleIndex="3">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="transportadora" Caption="Transportadora" ShowInCustomizationForm="True" VisibleIndex="4">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn FieldName="estadoTransportadora" Caption="Estado Transportadora" ShowInCustomizationForm="True" VisibleIndex="4">
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <CellStyle HorizontalAlign="Center"></CellStyle>
                                            </dx:GridViewDataTextColumn>
                                        </Columns>
                                        <SettingsDetail ShowDetailRow="True" />
                                        <Templates>
                                            <DetailRow>
                                                <dx:ASPxGridView ID="gridDetail" runat="server" AutoGenerateColumns="False" OnBeforePerformDataSelect="gridDetail_BeforePerformDataSelect" ClientInstanceName="gridDetail"
                                                    Font-Size="Small" KeyFieldName="guia" Width="100%">
                                                    <Columns>
                                                        <dx:GridViewDataTextColumn Caption="Guia" FieldName="guia" ShowInCustomizationForm="True" Visible="false" VisibleIndex="0">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Nombre Concepto" FieldName="nombre_concepto" ShowInCustomizationForm="True" VisibleIndex="1">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Nombre Movimiento" FieldName="nombre_movimiento" ShowInCustomizationForm="True" VisibleIndex="2">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                        <dx:GridViewDataTextColumn Caption="Fecha Movimiento" FieldName="fecha_movimiento" ShowInCustomizationForm="True" VisibleIndex="3">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <CellStyle HorizontalAlign="Center">
                                                            </CellStyle>
                                                        </dx:GridViewDataTextColumn>
                                                    </Columns>
                                                </dx:ASPxGridView>
                                            </DetailRow>
                                        </Templates>

                                    </dx:ASPxGridView>

                                    <br />
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxRoundPanel>

                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxCallbackPanel>

            <dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" ClientInstanceName="LoadingPanel"
                Modal="True">
            </dx:ASPxLoadingPanel>
        </div>
    </form>
</body>
</html>
