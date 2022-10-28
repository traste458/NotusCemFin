<%@ Page Language="vb" AutoEventWireup="false" Codebehind="resultadoBuscarFacturasRecibidas.aspx.vb" Inherits="BPColSysOP.resultadoBuscarFacturasRecibidas" enableViewState="True" culture="es-CO" uiCulture="es-CO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>resultadoBuscarFacturasRecibidas</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Buscar Facturas 
				Recibidas - Resultado </b></font>
		<hr>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="buscarFacturasRecibidas.aspx">Nueva Busqueda</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Red"></asp:label></TD>
				</TR>
			</TABLE>
			<table class="tabla" cellSpacing="1">
				<TR>
					<TD class="tdCampoRec">
						<asp:LinkButton id="lbExportar" runat="server" ForeColor="Blue" Font-Bold="True" Visible="False"><img src='../images/excel.gif' border='0' alt='Exportar Reporte a Excel'>&nbsp;Exportar Datos a Excel</asp:LinkButton></TD>
				</TR>
				<tr id="elTitulo" runat="server">
					<td class="tdTituloRec"><asp:label id="Label1" runat="server" Font-Size="X-Small"> FACTURAS RECIBIDAS</asp:label></td>
				</tr>
				<TR>
					<TD class="tdCampoRec"><asp:datagrid id="dgFacturas" runat="server" ShowFooter="True" Width="100%" CellPadding="3" BackColor="White"
							BorderWidth="1px" BorderStyle="None" BorderColor="#999999" AutoGenerateColumns="False" CssClass="tabla" GridLines="Vertical"
							AllowPaging="True" PageSize="50">
							<FooterStyle Font-Size="X-Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="Black"
								BackColor="#CCCCCC"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="proveedor" HeaderText="PROVEERDOR"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="producto" HeaderText="PRODUCTO (REFERENCIA)"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="tipoProducto" HeaderText="TIPO DE PRODUCTO"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="guia" HeaderText="GUIA AEREA"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="factura" HeaderText="FACTURA"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="ordenCompra" HeaderText="ORDEN DE COMPRA"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="numeroPalets" HeaderText="NUMERO DE PALETS">
									<HeaderStyle Width="70px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="cantidadEsperada" HeaderText="CANTIDAD ESPERADA">
									<HeaderStyle Width="70px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="cantidadAprox" HeaderText="CANTIDAD RECIBIDA APROXIMADA">
									<HeaderStyle Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="peso" HeaderText="PESO (Kg)">
									<HeaderStyle Width="50px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Right"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="tipoRecepcion" HeaderText="TIPO DE RECEPCI&#211;N"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="fecha" HeaderText="FECHA RECEPCI&#211;N"
									DataTextFormatString="{0:dd/MM/yyyy}">
									<HeaderStyle Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="estadoRecepcion" HeaderText="ESTADO RECEPCI&#211;N">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="estadoFactura" HeaderText="ESTADO"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="bodega" HeaderText="BODEGA"></asp:HyperLinkColumn>
								<asp:BoundColumn DataField="observacion" HeaderText="OBSERVACIONES"></asp:BoundColumn>
							</Columns>
							<PagerStyle Font-Size="Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="Black" BackColor="#999999"
								Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
