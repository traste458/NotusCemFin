<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenCompraMerchanPopInsumoP1.aspx.vb" Inherits="BPColSysOP.CrearOrdenCompraMerchanPopInsumoP1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="../ControlesDeUsuario/EncabezadoPagina.ascx" tagname="EncabezadoPagina" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Crear Orden Compra</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />
    <script src="../include/jquery-1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    $(document).ready(function() {        
        $(":checkbox").click(countChecked);
        $(".error").width($("#gvRemisiones").width());
    });
    function modificarAltoFramePadre() {
        $("body.cuerpo2").ready(function() {
            $("#frModulo", parent.document).height($("body").height() + 80);
        });
    }    
    function validarChk() {
        var n = $("#gvRemisiones input:checked").length;
        var retorno = false;
        countChecked()
        if (n > 0)
            if (confirm("Esta seguro de continuar con las ordenes de recepcion seleccionadas ?"))
                retorno = true;
        return retorno;
    }
    function countChecked() {
        var n = $("#gvRemisiones input:checked").length;
        var mensaje = $(".error");
        if (n > 0)
            mensaje.hide();
        else
            mensaje.show();
    }
    
    </script>
    <style type="text/css">
    body.cuerpo2
    {
    	background-image:none;
    }
    </style>
</head>
<body class="cuerpo2">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div>
        <uc1:EncabezadoPagina ID="epEncabezado" runat="server" />
        <asp:HiddenField ID="hfIdTipoProducto" runat="server" />
    </div>
    <div id="contenedor" runat="server">
        <div style="padding:20px 15px;font-size:15px;">
            <asp:Label ID="lblTipoProducto" runat="server" Text="" CssClass="negrita"></asp:Label>
        </div>
        
        <div>
            <div>
                <label>Seleccione el proveedor:</label>
                <asp:DropDownList ID="ddlProveedor" runat="server" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Label ID="lblNumProveedores" runat="server" Text="" CssClass="comentario"></asp:Label>
            </div>
            <div>
            <span class="comentario" id="lblMensaje" visible="false" runat="server">Nota: Seleccione las remisiones que se van a agregar a la orden
                de compra. </span>
            </div>
            <asp:GridView ID="gvRemisiones" CssClass="tablaGris" runat="server" style="min-width:700px;border:2px solid silver;"
                AutoGenerateColumns="False" EmptyDataText="No existen remisiones pendientes.">
                <Columns>
                    <asp:TemplateField HeaderText="Seleccione">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkRemision" runat="server" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="remision" HeaderText="No. Remisión" >
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="idOrdenRecepcion" HeaderText="No. Orden Recepción" >
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Producto(s)">
                        <ItemTemplate>
                            <asp:BulletedList ID="bltProductosAgregados" DataTextField="nombreProducto" style="margin:4px 0;" runat="server">
                            </asp:BulletedList>                            
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Pallet(s)">
                        <ItemTemplate>
                            <asp:BulletedList ID="bltPalletsAgregados" DataTextField="palletPeso" style="margin:4px 0;" runat="server">
                            </asp:BulletedList> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                </Columns>
                <FooterStyle Font-Bold="True" ForeColor="#FF9900" />
            </asp:GridView>
        </div>
        <div>
            <div class="error" style="display:none;">Por favor, seleccione al menos una remisión.</div>
            <asp:Button ID="btnEnviar" runat="server" Text="Agregar" 
                OnClientClick="return validarChk()" CssClass="boton" Visible="False" />            
        </div>
    </div>
    </form>
</body>
</html>
