<%@ Page Language="vb" AutoEventWireup="false" Codebehind="recibosResultadoBuscarProducto.aspx.vb" Inherits="BPColSysOP.recibosResultadoBuscarProducto" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>recibosResultadoBuscarProducto</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Buscar Producto 
				(Referencia) - Resultado</b></font>
		<hr>
		<font color="gray" size="2"><i>
				<ul>
					Solo se lista Productos (Referencias) que no tienen distribución Regional.
				</ul>
			</i></font>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="recibosBuscarProducto.aspx">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"></asp:label></TD>
				</TR>
			</TABLE>
			<br>
			<table class="tabla" cellSpacing="0" cellPadding="0">
				<TR>
					<TD align="center"><asp:label id="lblTitulo" runat="server" Width="100%" CssClass="tdTituloRec">LISTADO DE PRODUCTOS REGISTRADOS</asp:label></TD>
				</TR>
				<tr>
					<td><anthem:datagrid id="dgProductos" runat="server" CssClass="tabla" BorderColor="#999999" BorderStyle="None"
							BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Vertical" ShowFooter="True" AutoGenerateColumns="False"
							PageSize="20" AllowPaging="True">
							<PagerStyle Font-Size="X-Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="Indigo"
								BackColor="#999999" Mode="NumericPages"></PagerStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<FooterStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#000084" BackColor="#CCCCCC"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="idproducto" HeaderText="ID"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="proveedor" HeaderText="PROVEEDOR"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="producto" HeaderText="PRODUCTO (REFERENCIA)"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="material" HeaderText="MATERIAL">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="tipoProducto" HeaderText="TIPO DE PRODUCTO"></asp:HyperLinkColumn>
							</Columns>
						</anthem:datagrid></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
