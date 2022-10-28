<%@ Page Language="vb" AutoEventWireup="false" Codebehind="reporteFacturasRecibidas.aspx.vb" Inherits="BPColSysOP.reporteFacturasRecibidas" culture="es-CO" uiCulture="es-CO" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>reporteFacturasRecibidas</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2">
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Reporte de Facturas 
				Recibidas</b></font>
		<hr>
		<form id="Form1" onsubmit="return validaciones();" method="post" runat="server">
			<TABLE class="tabla" id="Table1" cellSpacing="1" cellPadding="1" width="95%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="inicioConsultaFacturasRecibidas.aspx">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">
						<anthem:Label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"
							AutoUpdateAfterCallBack="True"></anthem:Label></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 19px" colSpan="2">
						<asp:LinkButton id="lbExportar" runat="server" Font-Bold="True" ForeColor="Blue" Visible="False"><img src='../images/excel.gif' border='0' alt='Exportar Resultado a Excel'>&nbsp;Exportar Resultado a Excel</asp:LinkButton></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 19px" colSpan="2"><anthem:datagrid id="dgDatos" runat="server" ShowFooter="True" BorderColor="#999999" BorderStyle="None"
							BorderWidth="1px" BackColor="White" AutoGenerateColumns="False" CellPadding="3" CssClass="tabla" AllowPaging="True" GridLines="Vertical"
							PageSize="50" TextDuringCallBack="Procesando..." AutoUpdateAfterCallBack="True" UpdateAfterCallBack="True">
							<PagerStyle Font-Size="X-Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="DarkBlue"
								BackColor="#999999" PageButtonCount="25" Mode="NumericPages"></PagerStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<FooterStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="factura" HeaderText="FACTURA"></asp:BoundColumn>
								<asp:BoundColumn DataField="ordenCompra" HeaderText="ORDEN DE COMPRA"></asp:BoundColumn>
								<asp:BoundColumn DataField="guia" HeaderText="GU&amp;Iacute;A AEREA"></asp:BoundColumn>
								<asp:BoundColumn DataField="proveedor" HeaderText="PROVEEDOR"></asp:BoundColumn>
								<asp:BoundColumn DataField="producto" HeaderText="PRODUCTO (REFERENCIA)"></asp:BoundColumn>
								<asp:BoundColumn DataField="tipoProducto" HeaderText="TIPO DE PRODUCTO"></asp:BoundColumn>
								<asp:BoundColumn DataField="tipoRecepcion" HeaderText="TIPO DE RECEPCI&amp;Oacute;N" FooterText="TOTAL"></asp:BoundColumn>
								<asp:BoundColumn DataField="numeroPalets" HeaderText="N&#218;MERO PALETS">
									<HeaderStyle Width="70px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="cantidadAprox" HeaderText="CANTIDAD RECIBIDA APROX.">
									<HeaderStyle Width="80px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="cantidadEsperada" HeaderText="CANTIDAD ESPERADA">
									<HeaderStyle Width="85px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="bodega" HeaderText="BODEGA"></asp:BoundColumn>
								<asp:BoundColumn DataField="fecha" HeaderText="FECHA RECEPCI&amp;Oacute;N" DataFormatString="{0:dd-MMM-yyyy}">
									<HeaderStyle Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="estadoRecepcion" HeaderText="ESTADO RECEPCI&#211;N">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="estado" HeaderText="ESTADO">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="observacion" HeaderText="OBSERVACI&amp;Oacute;N"></asp:BoundColumn>
							</Columns>
						</anthem:datagrid></TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"></TD>
				</TR>
			</TABLE>
			<br>
		</form>
	</body>
</HTML>
