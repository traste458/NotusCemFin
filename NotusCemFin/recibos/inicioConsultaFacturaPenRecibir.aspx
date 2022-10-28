<%@ Page Language="vb" AutoEventWireup="false" Codebehind="inicioConsultaFacturaPenRecibir.aspx.vb" Inherits="BPColSysOP.consultaFactura" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>informacion_Consulta_Factura_Recibir</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
		<script language="jscript" type="text/Jscript">
		</script>
	</HEAD>
	<body class="cuerpo2" onload="window.document.Form1.txtFactura.focus();" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Reportes Facturas 
				Pendientes por Recibir</b></font>
		<hr>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server">Regresar</asp:hyperlink><br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2">&nbsp;
						<anthem:label id="lblError" runat="server" Font-Size="X-Small" ForeColor="Red" Font-Names="Arial"
							Font-Bold="True" AutoUpdateAfterCallBack="True"></anthem:label></TD>
				</TR>
			</TABLE>
			<TABLE class="tabla" cellSpacing="1" cellPadding="1" width="70%" border="1" borderColor="#006699">
				<TR>
					<TD class="tdTituloRec" bgColor="#dddddd" colSpan="2">PARAMETROS DE BUSQUEDA</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label1" runat="server">Factura:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtFactura" tabIndex="1" runat="server" MaxLength="25"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label3" runat="server">Tipo de Producto:</asp:label></TD>
					<TD class="tdCampoRec"><anthem:dropdownlist id="ddlTipoProducto" tabIndex="2" runat="server" AutoCallBack="True"></anthem:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label2" runat="server">Proveedor:</asp:label></TD>
					<TD class="tdCampoRec"><anthem:dropdownlist id="ddlProveedor" tabIndex="3" runat="server" AutoCallBack="True"></anthem:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label4" runat="server">Producto:</asp:label></TD>
					<TD class="tdCampoRec"><anthem:dropdownlist id="ddlProducto" tabIndex="4" runat="server" AutoUpdateAfterCallBack="True"></anthem:dropdownlist>&nbsp;
					</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="160" bgColor="#dddddd"><asp:label id="Label6" runat="server" Font-Names="Arial"> Fecha Esperada Recepción:</asp:label></TD>
					<TD class="tdCampoRec"><INPUT class="textbox" id="fechaInicial" tabIndex="5" readOnly size="11" name="fechaInicial"
							runat="server"><A hideFocus onclick="if(self.gfPop)gfPop.fStartPop(document.Form1.fechaInicial,document.Form1.fechaFinal);return false;"
							href="javascript:void(0)"><IMG class="PopcalTrigger" height="22" alt="Seleccione una Fecha Inicial" src="../include/HelloWorld/calbtn.gif"
								width="34" align="absMiddle" border="0"></A><INPUT style="WIDTH: 16px; HEIGHT: 22px" type="hidden" size="1" name="fechaFinal"></TD>
				</TR>
			</TABLE>
			<TABLE cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><BR>
						</FONT><BR>
						<asp:button id="btnContinuar" tabIndex="6" runat="server" CssClass="botonRec" Text="Continuar"></asp:button>&nbsp;</TD>
				</TR>
			</TABLE> <!-- iframe para uso de selector de fechas --><IFRAME id="gToday:contrast:agenda.js" style="Z-INDEX: 101; LEFT: -500px; VISIBILITY: visible; POSITION: absolute; TOP: -500px"
				name="gToday:contrast:agenda.js" src="../include/DateRange/ipopeng.htm" frameBorder="0" width="132" scrolling="no" height="142">
			</IFRAME>
		</form>
	</body>
</HTML>
