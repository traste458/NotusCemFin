<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="buscarFacturasRecibidas.aspx.vb" Inherits="BPColSysOP.buscarFacturasRecibidas" culture="es-CO" uiCulture="es-CO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>buscarFacturasRecibidas</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
		<script language="javascript" type="text/javascript">
		    function validacion(){
		      if(document.Form1.fechaInicial.value!=""&&document.Form1.fechaFinal.value==""){
		        alert("Escoja la Fecha Final, Por Favor");
		        document.Form1.fechaFinal.focus();
		        return(false);
		      }
		      if(document.Form1.fechaInicial.value==""&&document.Form1.fechaFinal.value!=""){
		        alert("Escoja la Fecha Inicial, Por Favor");
		        document.Form1.fechaInicial.focus();
		        return(false);
		      }
		    }
		</script>
	</HEAD>
	<body class="cuerpo2">
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Buscar Facturas 
				Recibidas</b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><anthem:label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"
							AutoUpdateAfterCallBack="True"></anthem:label></TD>
				</TR>
			</TABLE>
			<font color="gray" size="2"><i>
					<ul>
						Solo se mostrarán por defecto las Facturas cuyo Estado correponda a un estado 
						Activo.
					</ul>
				</i></font>
			<table class="tabla" width="70%" borderColor="#006699" cellSpacing="1" cellPadding="1"
				border="1">
				<TR>
					<TD class="tdTituloRec" bgColor="#dddddd" colSpan="2">PATRONES DE BUSQUEDA</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label6" runat="server" Font-Bold="True">Tipo de Producto:</asp:label></TD>
					<TD class="tdCampoRec"><anthem:dropdownlist id="ddlTipoProducto" runat="server" AutoCallBack="True"></anthem:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="lblFactura" runat="server" Font-Bold="True">Factura:</asp:label></TD>
					<TD class="tdCampoRec"><font color="blue" size="2"><asp:textbox id="txtFactura" runat="server" MaxLength="25" CssClass="textbox"></asp:textbox></font></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label1" runat="server" Font-Bold="True">Orden de Compra:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtOrdenCompra" runat="server" MaxLength="25" CssClass="textbox"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label2" runat="server" Font-Bold="True">Guía Aerea:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtGuia" runat="server" MaxLength="25" CssClass="textbox"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label4" runat="server" Font-Bold="True">Proveedor:</asp:label></TD>
					<TD class="tdCampoRec"><anthem:dropdownlist id="ddlProveedor" runat="server" AutoCallBack="True"></anthem:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" style="HEIGHT: 16px" bgColor="#dddddd"><asp:label id="Label3" runat="server" Font-Bold="True">Producto:</asp:label></TD>
					<TD class="tdCampoRec" style="HEIGHT: 16px"><anthem:dropdownlist id="ddlProducto" runat="server" AutoUpdateAfterCallBack="True"></anthem:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label5" runat="server" Font-Bold="True">Tipo de Recepción:</asp:label></TD>
					<TD class="tdCampoRec"><asp:dropdownlist id="ddlTipoRecepcion" runat="server"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd">
						<asp:label id="Label8" runat="server" Font-Bold="True">Estado Recepción:</asp:label></TD>
					<TD class="tdCampoRec">
						<asp:dropdownlist id="ddlEstadoRecepcion" runat="server"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd">
						<asp:label id="Label9" runat="server" Font-Bold="True">Estado Factura:</asp:label></TD>
					<TD class="tdCampoRec">
						<asp:dropdownlist id="ddlEstado" runat="server"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label7" runat="server" Font-Bold="True">Fecha Recepción:</asp:label></TD>
					<TD class="tdCampoRec">De <INPUT class="textbox" id="fechaInicial" readOnly size="11" name="fechaInicial" runat="server"><A hideFocus onclick="if(self.gfPop)gfPop.fStartPop(document.Form1.fechaInicial,document.Form1.fechaFinal);return false;"
							href="javascript:void(0)"><IMG class="PopcalTrigger" height="22" alt="Seleccione una Fecha Inicial" src="../include/HelloWorld/calbtn.gif"
								width="34" align="absMiddle" border="0"></A>&nbsp;&nbsp;a&nbsp;&nbsp;<INPUT class="textbox" id="fechaFinal" readOnly size="11" name="fechaFinal" runat="server"><A hideFocus onclick="if(self.gfPop)gfPop.fEndPop(document.Form1.fechaInicial,document.Form1.fechaFinal);return false;"
							href="javascript:void(0)"><IMG class="PopcalTrigger" height="22" alt="Seleccione una Fecha Final" src="../include/HelloWorld/calbtn.gif"
								width="34" align="absMiddle" border="0"></A><font color="red" size="2">**&nbsp;
							<anthem:panel id="pnlEsRegionalizado" runat="server" AutoUpdateAfterCallBack="True" Width="16px">
								<INPUT id="hEsRegionalizado" style="WIDTH: 8px; HEIGHT: 22px" type="hidden" size="1" name="hEsRegionalizado"
									runat="server"></anthem:panel></font></TD>
				</TR>
			</table>
			<table class="tabla" width="80%">
				<tr>
					<td><font color="red" size="2">**</font> Se debe escoger los dos valores
					</td>
				</tr>
				<tr>
					<td><br>
						<asp:button id="btnContinuar" runat="server" ForeColor="White" CssClass="botonRec" Text="Buscar"></asp:button></td>
				</tr>
			</table>
			<br>
			<!-- iframe para uso de selector de fechas --><iframe id="gToday:contrast:agenda.js" style="Z-INDEX: 999; LEFT: -500px; VISIBILITY: visible; POSITION: absolute; TOP: -500px"
				name="gToday:contrast:agenda.js" src="../include/DateRange/ipopeng.htm" frameBorder="0" width="132" scrolling="no" height="142">
			</iframe>
		</form>
	</body>
</HTML>
