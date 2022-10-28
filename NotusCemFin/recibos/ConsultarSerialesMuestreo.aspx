<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ConsultarSerialesMuestreo.aspx.vb" Inherits="BPColSysOP.ConsultarSerialesMuestreo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ModalProgress.ascx" TagName="ModalProgress" TagPrefix="uc2" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Consultar Seriales Muestreo</title>
    <link rel="stylesheet" type="text/css" href="../include/styleBACK.css" />
    <script type="text/javascript" src="../include/jquery-1.js" ></script>
</head>
<body class="cuerpo2">
    <form id="frmSerialesMuestreo" runat="server">
        <asp:ScriptManager ID="smSerialesMuestreo" runat="server" EnableScriptGlobalization="True">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="uplEncabezado" runat="server" RenderMode="Inline">
            <ContentTemplate>
                <uc1:EncabezadoPagina ID="epMuestreo" runat="server" />
                <uc2:ModalProgress ID="mpMuestreo" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
            <table class="tabla">
                <tr>
                    <th >
                        Lista de seriales de Muestreo
                    </th>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkDescargarExcel" runat="server" AutoUpdateAfterCallBack="true"
                            EnableCallBack="false"><img src="../images/Excel.gif" alt="" />&nbsp;Descargar Reporte en Excel</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlSeriales" runat="server" AutoUpdateAfterCallBack="true">
                            <asp:GridView ID="grvMuestreo" runat="server" AutoGenerateColumns="False" CssClass="grid"
                            AllowPaging="True" ShowFooter="true"
                            EmptyDataText="No se encontraron registros que coincidan con el filtro." 
                                PageSize="20">
                                <PagerStyle HorizontalAlign="Center" />
                                <HeaderStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="factura" HeaderText="Factura" InsertVisible="False" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="guia" HeaderText="Guía" InsertVisible="False" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="serial" HeaderText="Serial" InsertVisible="False" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Center"/>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="orden" HeaderText="Orden" InsertVisible="False" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fecha" HeaderText="Fecha de Muestreo" InsertVisible="False" ReadOnly="True">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                </Columns> 
                                <FooterStyle CssClass="thGris" />
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
