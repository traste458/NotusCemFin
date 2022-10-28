<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CrearOrdenCompra.aspx.vb"  Inherits="BPColSysOP.CrearOrdenCompra" %>

<%@ Register Src="../ControlesDeUsuario/EncabezadoPagina.ascx" TagName="EncabezadoPagina"
    TagPrefix="uc1" %>
<%@ Register Src="../ControlesDeUsuario/ModalProgress.ascx" TagName="ModalProgress"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crear Orden de Compra</title>
    <link href="../include/styleBACK.css" rel="stylesheet" type="text/css" />

    <script src="../include/jquery-1.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(document).ready(init);
        var opcionActual;
        function init() {
//            $("#frModulo").load(function() {
//                $("#precarga").hide(1, function() {
//                    $("#frModulo").show();
//                });                
//            });
        }
        

    </script>

    <style type="text/css">
        .tablaGris
        {
            width: 600px;
            padding: 0 10px;
            margin: 0;
        }
        #contenido
        {
            padding: 12px 0 0 8px;
        }
    </style>
</head>
<body class="cuerpo2">
    <form id="frmOrdenCompra" runat="server">
    <div>
        <uc1:EncabezadoPagina ID="epNotificador" runat="server" />
    </div>
    <div id="contenido">
        <table class="tablaGris">
            <tr>
                <th colspan="2">
                    Informacion de la Orden de Compra
                </th>
            </tr>
            <tr>
                <td style="width: 140px;">
                    Tipo de Producto:
                </td>
                <td>
                    <asp:DropDownList ID="ddlTipoProducto" runat="server" AutoPostBack="True">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <div>                        
             
            <%--<iframe runat="server" id="frModulo" src="" width="80%" frameborder="0" scrolling="no"></iframe>--%>

        </div>
    </div>
    </form>
</body>
</html>
