<%@ Page Language="vb" AutoEventWireup="false" Codebehind="verFacturasPendientes.aspx.vb" Inherits="BPColSysOP.verFacturasPendientes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>verFacturasPendientes</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Recibir Factura - 
				Listado de Facturas Pendientes </b></font>
		<hr>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="recibirFacturaInicio.aspx">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"></asp:label></TD>
				</TR>
			</TABLE>
			<table class="tabla" cellSpacing="1">
				<TR>
					<TD class="tdCampoRec"><asp:hyperlink id="hlCrearFactura" runat="server" ForeColor="Blue" Font-Bold="True" NavigateUrl="crearFacturaExterna.aspx?factura=@f&amp;idTp=@tp&amp;idTr=@idTr"><img src='images/new.gif' border='0' alt='Crear Nueva Factura' />Ir a Crear Nueva Factura</asp:hyperlink>
						<asp:hyperlink id="hlRecibirTraslado" runat="server" NavigateUrl="recibirFacturaExternaExistentePorTraslado.aspx?f=@f&amp;idTp=@tp&amp;idF=@idF&amp;idTr=@idTr"
							Font-Bold="True" ForeColor="DarkGreen" Visible="False"><img src='images/update.gif' border='0' alt='Recibir Factura por Traslado' />Recibir Factura Existente por Traslado</asp:hyperlink>&nbsp;&nbsp;<br>
					</TD>
				</TR>
				<tr>
					<td class="tdTituloRec"><asp:label id="Label1" runat="server" Font-Size="X-Small">FACTURAS PENDIENTES POR RECIBIR</asp:label></td>
				</tr>
				<TR>
					<TD class="tdCampoRec"><asp:datagrid id="dgFacturas" runat="server" CssClass="tabla" AutoGenerateColumns="False" BorderColor="#999999"
							BorderStyle="None" BorderWidth="1px" BackColor="White" CellPadding="3" Width="100%" ShowFooter="True" GridLines="Vertical">
							<FooterStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="proveedor" HeaderText="PROVEERDOR"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="producto" HeaderText="PRODUCTO (REFERENCIA)"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="factura" HeaderText="FACTURA"></asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="cantidadEsperada" HeaderText="CANTIDAD ESPERADA">
									<HeaderStyle Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="fecha" HeaderText="FECHA ESPERADA" DataTextFormatString="{0:dd-MMM-yyyy}">
									<HeaderStyle Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:HyperLinkColumn>
							</Columns>
							<PagerStyle HorizontalAlign="Center" ForeColor="Black" BackColor="#999999" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
			</table>
		</form>
	</body>
</HTML>
