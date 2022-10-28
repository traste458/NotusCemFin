<%@ Page Language="vb" AutoEventWireup="false" Codebehind="reporteFacturasPendientesRecibir.aspx.vb" Inherits="BPColSysOP.reporteFacturasPendientesRecibir" culture="es-CO" uiCulture="es-CO" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>reporteFacturasPorRecibir</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2">
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Reporte de Facturas 
				Pendientes Por Recibir</b></font>
		<hr>
		<form id="Form1" onsubmit="return validaciones();" method="post" runat="server">
			<TABLE class="tabla" id="Table1" cellSpacing="1" cellPadding="1" width="90%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="inicioConsultaFacturaPenRecibir.aspx">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><anthem:label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"
							Font-Names="Arial" AutoUpdateAfterCallBack="True"></anthem:label></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 17px" colSpan="2"><asp:linkbutton id="lbExportar" runat="server" ForeColor="Blue" Font-Bold="True" Visible="False"><img src='../images/excel.gif' border='0' alt='Exportar Resultado a Excel'>&nbsp;Exportar Resultado a Excel</asp:linkbutton></TD>
				</TR>
				<TR>
					<TD style="HEIGHT: 17px" colSpan="2"><anthem:datagrid id="dgDatos" runat="server" AllowPaging="True" CssClass="tabla" CellPadding="3"
							AutoGenerateColumns="False" BackColor="White" BorderWidth="1px" BorderStyle="None" BorderColor="#999999" ShowFooter="True" GridLines="Vertical"
							PageSize="30" AutoUpdateAfterCallBack="True" TextDuringCallBack="Procesando ..." UpdateAfterCallBack="True">
							<PagerStyle Font-Size="X-Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="DarkBlue"
								BackColor="#999999" PageButtonCount="25" Mode="NumericPages"></PagerStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<FooterStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="Black" BackColor="#CCCCCC"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="factura" HeaderText="FACTURA"></asp:BoundColumn>
								<asp:BoundColumn DataField="proveedor" HeaderText="PROVEEDOR"></asp:BoundColumn>
								<asp:BoundColumn DataField="producto" HeaderText="PRODUCTO (REFERENCIA)"></asp:BoundColumn>
								<asp:BoundColumn DataField="tipoProducto" HeaderText="TIPO DE PRODUCTO"></asp:BoundColumn>
								<asp:BoundColumn DataField="tipoRecepcion" HeaderText="TIPO DE RECEPCI&amp;Oacute;N" FooterText="TOTAL"></asp:BoundColumn>
								<asp:BoundColumn DataField="cantidadEsperada" HeaderText="CANTIDAD ESPERADA">
									<HeaderStyle Width="90px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="fecha" HeaderText="FECHA PROBABLE RECEPCI&amp;Oacute;N" DataFormatString="{0:dd-MMM-yyyy}">
									<HeaderStyle Width="100px"></HeaderStyle>
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</anthem:datagrid></TD>
				</TR>
			</TABLE>
			<br>
		</form>
	</body>
</HTML>
