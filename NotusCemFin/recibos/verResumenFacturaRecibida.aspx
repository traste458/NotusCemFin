<%@ Page Language="vb" AutoEventWireup="false" Codebehind="verResumenFacturaRecibida.aspx.vb" Inherits="BPColSysOP.verResumenFacturaRecibida" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>verResumenFacturaRecibida</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Resumen Factura 
				Recibida</b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="recibirFacturaInicio.aspx">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"></asp:label><asp:label id="lblRes" runat="server" ForeColor="Blue" Font-Bold="True" Font-Size="X-Small"></asp:label></TD>
				</TR>
			</TABLE>
			<table class="tabla">
				<TR>
					<TD>
						<TABLE class="tabla" id="Table2" borderColor="#006699" cellSpacing="1" cellPadding="1"
							width="100%" border="1">
							<TR>
								<TD class="tdTituloRec" colSpan="2"><asp:label id="Label1" runat="server">INFORMACIÓN REGISTRADA</asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label14" runat="server" Font-Bold="True">FACTURA:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblFactura" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label2" runat="server" Font-Bold="True">ORDEN DE COMPRA:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblOrdenCompra" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label9" runat="server" Font-Bold="True">GUÍA AEREA:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblGuia" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label18" runat="server" Font-Bold="True">PROVEEDOR:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblProveedor" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label19" runat="server" Font-Bold="True">PRODUCTO:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblProducto" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label21" runat="server" Font-Bold="True">TIPO DE RECEPCIÓN:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblTipoRecepcion" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label3" runat="server" Font-Bold="True">CANTIDAD ESPERADA:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblCantidadEsperada" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec" width="120"><asp:label id="Label10" runat="server" Font-Bold="True">NÚMERO DE PALETS:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblPalets" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label11" runat="server" Font-Bold="True">CANTIDAD APROX.:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblCantidadAprox" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label13" runat="server" Font-Bold="True">PESO:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblPeso" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label4" runat="server" Font-Bold="True">ESTADO RECEPCION:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblEstadoRecepcion" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label12" runat="server" Font-Bold="True">BODEGA:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblBodega" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label15" runat="server" Font-Bold="True">OBSERVACIÓN:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">&nbsp;<asp:label id="lblObservacion" runat="server" Font-Bold="True" ForeColor="MediumBlue" Width="300px"></asp:label></TD>
							</TR>
						</TABLE>
						<INPUT id="hIdTipoProducto" style="WIDTH: 16px; HEIGHT: 22px" type="hidden" size="1" name="hIdTipoProducto"
							runat="server"><INPUT id="hTipoProducto" style="WIDTH: 16px; HEIGHT: 22px" type="hidden" size="1" name="hTipoProducto"
							runat="server">
					</TD>
				</TR>
			</table>
			<br>
		</form>
	</body>
</HTML>
