<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PruebaInventario.aspx.vb" Inherits="BPColSysOP.PruebaInventario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Prueba Inventario</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="itemsGridView" runat="server" BackColor="White" 
            BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
            ForeColor="Black" GridLines="Vertical">
            <RowStyle BackColor="#F7F7DE" />
            <FooterStyle BackColor="#CCCC99" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White"/>
        </asp:GridView>
        <br />
        <asp:BulletedList ID="itemsBulletedList" runat="server">
        </asp:BulletedList>
    
    </div>
    </form>
</body>
</html>
