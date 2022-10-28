<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AdmTipoProductoPrincipalSecundario.aspx.vb" Inherits="BPColSysOP.AdmTipoProductoPrincipalSecundario" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/ModalProgress.ascx" tagname="ModalProgress" tagprefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Adm. Tipos de Producto(s) Principal y Secundario</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <style type="text/css">
        .columna                
        {
        	float:left;
        	width:600px;
        }
        .vacio
        {
        	padding:20px 0;
        }
    </style>
</head>
<body class="cuerpo2">
    <form id="frmPrincipal" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upGeneral" runat="server">    
    <ContentTemplate>
    <div>            
        <uc1:EncabezadoPagina ID="epPrincipal" runat="server" />    
    </div>
    <div>
        <div class="columna contenedorOpciones">
            <h1>Tipo de Producto Principal</h1>
            <asp:GridView ID="gvTipoPrincipal" CssClass="tablaGris" runat="server" AutoGenerateColumns="False" style="width:100%;">
                <Columns>
                    <asp:BoundField DataField="descripcion" HeaderText="Tipo Producto" />
                    <asp:BoundField DataField="unidadEmpaque" HeaderText="Unidad de Empaque" />
                    <asp:TemplateField HeaderText="Opc.">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnVerDetalle" runat="server" 
                                CommandArgument="<%# Bind('idTipoProducto') %>" CommandName="verSecundario" 
                                ImageUrl="~/images/view.png" ToolTip="Ver productos secundarios" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="columna contenedorOpciones" id="pnlTipoSecundario" runat="server">
            <h1>Tipo de Producto Secundario &nbsp; 
                <asp:Label ID="lblInfoTipoPrincipal" runat="server" Text=""></asp:Label></h1>
            <div>
                <asp:LinkButton ID="lnkAgregarSecundario" runat="server" style="background-color:Silver;padding:5px;">
                    <img src="../images/add.png" alt="Adicionar Tipos de Producto(s) Secundarios" title="Adicionar Tipos de Producto(s) Secundarios" />
                    Adicionar Tipos de Producto(s) Secundarios
                </asp:LinkButton>
            </div>
            <asp:GridView ID="gvTipoSecundario" runat="server" CssClass="tablaGris" style="width:100%;"
                AutoGenerateColumns="False" 
                EmptyDataText="&lt;p class='vacio'&gt;No existen registros.&lt;/p&gt;">
                <Columns>
                    <asp:BoundField DataField="TipoProductoAdicional" 
                        HeaderText="Tipo de Producto" />
                    <asp:BoundField DataField="unidadEmpaqueAdicional" 
                        HeaderText="Unidad de Empaque" />
                    <asp:TemplateField HeaderText="Opc.">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgBtnEliminar" runat="server" OnClientClick="return confirm('¿Es seguro de eliminar este tipo de producto ?');"
                                CommandArgument="<%# Bind('idTipoProductoSecundario') %>" 
                                CommandName="eliminar" ImageUrl="~/images/cross.png" 
                                ToolTip="Eliminar tipo producto" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <asp:HiddenField ID="hfTipoSeleccionado" runat="server" />        
        <asp:HiddenField ID="hfControlPopTipoProducto" runat="server" />        
        <cc1:ModalPopupExtender ID="mpeTipoProductoSecundario" BackgroundCssClass="modalBackground" 
            CancelControlID="imgBtnCerrarPopUp" PopupControlID="pnlAdicionarProductos" runat="server" TargetControlID="hfControlPopTipoProducto">
        </cc1:ModalPopupExtender>
        
        <div id="pnlAdicionarProductos" runat="server" class="contenedorOpciones modalPopUp" style="display:none;">
            <div style="text-align:right;">
                <asp:ImageButton ID="imgBtnCerrarPopUp" runat="server" ImageUrl="~/images/cerrar.gif" />
            </div>   
            <h1>Tipos de Productos</h1>
            <div>                
                <asp:CheckBoxList ID="cblTipoProductos" runat="server">
                </asp:CheckBoxList>
            </div>         
            <div>
                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" CssClass="submit" />
            </div>
        </div>
        <div style="clear:both;"></div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    <uc2:ModalProgress ID="modelProgressPrin" runat="server" />
    </form>
</body>
</html>
