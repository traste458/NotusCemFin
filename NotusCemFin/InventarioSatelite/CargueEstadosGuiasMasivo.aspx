<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CargueEstadosGuiasMasivo.aspx.vb" Inherits="BPColSysOP.CargueEstadosGuiasMasivo" %>

<!DOCTYPE html>
<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina" TagPrefix="uc1" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>::Cargue estados guias - Masivo::</title>
    <link rel="shortcut icon" href="../images/baloons_small.png" />
    <link href="../include/styleBACK.css" type="text/css" rel="stylesheet" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script src="../include/JavaScriptFunctions.js" type="text/javascript"></script>
    <script type="text/javascript">
        var ultimaBodega = null;
        function onSelectedBodegasChanged(cmbTipoBodega) {
           
            //Llena bodegas
            if (cmbBodegasDisp.InCallback()) {
                ultimaBodega = cmbTipoBodega.GetValue().toString();
            }
            else {
                cmbBodegasDisp.PerformCallback(cmbTipoBodega.GetValue().toString());
            }
        }

        function VerEjemplo() {
            window.location.href = 'Plantillas/CargueMasivoEstadoGuia.xlsx';
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

                        <br />
                        <dx:ASPxRoundPanel ID="roundPanelLectura" runat="server" ClientInstanceName="roundPanelLectura" HeaderText="Lectura de Productos" Width="1100px" Theme="SoftOrange">
                            <PanelCollection>
                                <dx:PanelContent runat="server">

                                    <dx:ASPxPageControl ShowTabs="true" ID="pcProductos" runat="server" ClientInstanceName="pcProductos" ActiveTabIndex="1" Width="100%" Theme="softorange" Visible="true">
                                        <TabPages>
                                            <dx:TabPage Text="Lectura Guia">
                                                <ContentCollection>
                                                    <dx:ContentControl>
                                                        
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <%--<asp:FileUpload ID="fuArchivoGuia" runat="server" Width="370px" ValidationGroup="GuardarMasivo" />--%>

                                                                    
                                                                    <dx:ASPxUploadControl ID="upArchivoGuia" runat="server" ClientInstanceName="upArchivoGuia" Width="100%"
                                                                             ShowProgressPanel="True"  NullText="Seleccione un archivo...">

                                                                        <ValidationSettings AllowedFileExtensions=".xls,.xlsx">
                                                                        </ValidationSettings>

                                                                    </dx:ASPxUploadControl>
                                                                    <dx:ASPxLabel ID="aspLabelGuia" runat="server" ClientInstanceName="aspLabelGuia"></dx:ASPxLabel>
                                                                                                                                       
                                                                </td>
                                                                <td>
                                                                    <dx:ASPxButton ID="btnCargarGuias" runat="server" AutoPostBack="False" ClientInstanceName="btnCargarGuias" Text="Cargar" ValidationGroup="GuardarMasivo" Width="180px" Font-Bold="true" Theme="SoftOrange">
                                                                        <ClientSideEvents Click="function(s,e){
                                                                                                       if (ASPxClientEdit.ValidateGroup('GuardarMasivo')) {
                                                                                                            cpPrincipal.PerformCallback(); 
                                                                                                        }  
                                                                                                    }" />
                                                                        <Image Url="~/images/add.png">
                                                                        </Image>
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <br />
                                                                     <a href="javascript:void(0);" id="VerEjemplo" onclick="javascript:VerEjemplo();"
                                                                        class="style2"><span class="style3">(Ver Archivo Excel de Ejemplo) </span></a>
                                                                  
                                                                    <br />
                                                                </td>
                                                            </tr>
                                                        </table>

                                                        <table width="100%">
                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxGridView ID="gvError" runat="server" ClientInstanceName="gvError" AutoGenerateColumns="false" Width="60%"
                                                                        KeyFieldName="idSubproducto" Theme="SoftOrange">
                                                                        <Columns>
                                                                            <dx:GridViewDataTextColumn FieldName="Fila" Caption="Fila" VisibleIndex="0" Width="10" Visible="true">
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn FieldName="Mensaje" Caption="Mensaje" VisibleIndex="0" Width="90" Visible="true">
                                                                            </dx:GridViewDataTextColumn>
                                                                        </Columns>
                                                                        <SettingsBehavior AllowDragDrop="False" AllowGroup="False" />
                                                                    </dx:ASPxGridView>
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </dx:ContentControl>
                                                </ContentCollection>
                                            </dx:TabPage>
                                            
                                        </TabPages>
                                    </dx:ASPxPageControl>                                    
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
