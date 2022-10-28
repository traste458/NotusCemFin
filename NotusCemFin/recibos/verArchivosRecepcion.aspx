<%@ Page Language="vb" AutoEventWireup="false" Codebehind="verArchivosRecepcion.aspx.vb" Inherits="BPColSysOP.verArchivosRecepcion" culture="es-CO" uiCulture="es-CO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>verArchivosRecepcion</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Reporte Archivos de 
				Recepción Por Factura</b></font>
		<hr>
		<form id="Form1" onsubmit="return validaciones();" method="post" runat="server">
			<TABLE class="tabla" id="Table1" cellSpacing="1" cellPadding="1" width="90%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="inicioVerArchivosRecepcion.aspx">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" Font-Names="Arial" Font-Size="X-Small" Font-Bold="True"
							ForeColor="Red"></asp:label></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 17px" colSpan="2"><asp:datagrid id="dgDatos" runat="server" PageSize="25" GridLines="Vertical" BorderColor="#999999"
							BorderStyle="None" BorderWidth="1px" BackColor="White" AutoGenerateColumns="False" CellPadding="3" CssClass="tabla" AllowPaging="True">
							<FooterStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="factura" HeaderText="FACTURA"></asp:BoundColumn>
								<asp:BoundColumn DataField="guia" HeaderText="GUIA AEREA"></asp:BoundColumn>
								<asp:BoundColumn DataField="fechaRegistro" HeaderText="FECHA REGISTRO ARCHIVO" DataFormatString="{0:dd-MMM-yy hh:mm tt}">
									<HeaderStyle Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="nombreArchivo" HeaderText="ARCHIVO RECEPCION">
									<ItemStyle Font-Bold="True"></ItemStyle>
								</asp:HyperLinkColumn>
								<asp:TemplateColumn HeaderText="DESCARGAR">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
									<ItemTemplate>
										<asp:HyperLink id=hlDescargar runat="server" NavigateUrl='<%# DataBinder.Eval(Container, "DataItem.url") %>' ToolTip="Descargar Archivo" ImageUrl="../images/flecha_descargar.gif">
										</asp:HyperLink>
									</ItemTemplate>
								</asp:TemplateColumn>
							</Columns>
							<PagerStyle Font-Size="X-Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="DarkBlue"
								BackColor="#999999" PageButtonCount="25" Mode="NumericPages"></PagerStyle>
						</asp:datagrid></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"></TD>
				</TR>
			</TABLE>
			<br>
		</form>
	</body>
</HTML>
