<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="inicioConsultaFacturasRecibidas.aspx.vb" Inherits="BPColSysOP.inicioConsultaFacturasRecibidas" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>informacion_Consulta_Factura_Recibidas</title>
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
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Reporte de Facturas 
				Recibidas - Inicio</b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Red"></asp:label></TD>
				</TR>
			</TABLE>
			<table class="tabla" width="70%" borderColor="#006699" cellSpacing="1" cellPadding="1"
				border="1">
				<TR>
					<TD class="tdTituloRec" bgColor="#dddddd" colSpan="2">PARAMETROS DE BUSQUEDA</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="lblFactura" runat="server" Font-Bold="True">Factura:</asp:label></TD>
					<TD class="tdCampoRec"><font color="blue" size="2"><asp:textbox id="txtFactura" runat="server" CssClass="textbox" MaxLength="25"></asp:textbox></font></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label1" runat="server" Font-Bold="True">Orden de Compra:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtOrdenCompra" runat="server" CssClass="textbox" MaxLength="25"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label2" runat="server" Font-Bold="True">Guía Aerea:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtGuia" runat="server" CssClass="textbox" MaxLength="25"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label6" runat="server" Font-Bold="True">Tipo de Producto:</asp:label></TD>
					<TD class="tdCampoRec">
						<anthem:DropDownList id="ddlTipoProducto" runat="server" AutoCallBack="True"></anthem:DropDownList></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label4" runat="server" Font-Bold="True">Proveedor:</asp:label></TD>
					<TD class="tdCampoRec">
						<anthem:DropDownList id="ddlProveedor" runat="server" AutoCallBack="True"></anthem:DropDownList></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd" style="HEIGHT: 17px"><asp:label id="Label3" runat="server" Font-Bold="True">Producto:</asp:label></TD>
					<TD class="tdCampoRec" style="HEIGHT: 17px">
						<anthem:DropDownList id="ddlProducto" runat="server" AutoUpdateAfterCallBack="True"></anthem:DropDownList></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" style="HEIGHT: 17px" bgColor="#dddddd"><asp:label id="Label5" runat="server" Font-Bold="True">Tipo de Recepción:</asp:label></TD>
					<TD class="tdCampoRec" style="HEIGHT: 17px"><asp:dropdownlist id="ddlTipoRecepcion" runat="server"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd">
						<asp:label id="Label9" runat="server" Font-Bold="True">Estado Recepción:</asp:label></TD>
					<TD class="tdCampoRec">
						<asp:dropdownlist id="ddlEstadoRecepcion" runat="server"></asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd">
						<asp:label id="Label8" runat="server" Font-Bold="True">Estado de Factura:</asp:label></TD>
					<TD class="tdCampoRec">
						<asp:dropdownlist id="ddlEstado" runat="server">
							<asp:ListItem Value="-2">Escoja un Estado</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd" width="120"><asp:label id="Label7" runat="server" Font-Bold="True">Fecha Recepción:</asp:label></TD>
					<TD class="tdCampoRec">De <INPUT class="textbox" id="fechaInicial" readOnly size="11" name="fechaInicial" runat="server"><A hideFocus onclick="if(self.gfPop)gfPop.fStartPop(document.Form1.fechaInicial,document.Form1.fechaFinal);return false;"
							href="javascript:void(0)"><IMG class="PopcalTrigger" height="22" alt="Seleccione una Fecha Inicial" src="../include/HelloWorld/calbtn.gif"
								width="34" align="absMiddle" border="0"></A>&nbsp;&nbsp;a&nbsp;&nbsp;<INPUT class="textbox" id="fechaFinal" readOnly size="11" name="fechaFinal" runat="server"><A hideFocus onclick="if(self.gfPop)gfPop.fEndPop(document.Form1.fechaInicial,document.Form1.fechaFinal);return false;"
							href="javascript:void(0)"><IMG class="PopcalTrigger" height="22" alt="Seleccione una Fecha Final" src="../include/HelloWorld/calbtn.gif"
								width="34" align="absMiddle" border="0"></A><font color="red" size="2">**&nbsp;
						</font>
					</TD>
				</TR>
			</table>
			<table class="tabla" width="80%">
				<tr>
					<td>
						<font color="red" size="2">**</font> Se debe escoger los dos valores
					</td>
				</tr>
				<tr>
					<td><br>
						<asp:button id="btnContinuar" runat="server" ForeColor="White" CssClass="botonRec" Text="Continuar"></asp:button></td>
				</tr>
			</table>
			<br>
			<!-- iframe para uso de selector de fechas --><iframe id="gToday:contrast:agenda.js" style="Z-INDEX: 999; LEFT: -500px; VISIBILITY: visible; POSITION: absolute; TOP: -500px"
				name="gToday:contrast:agenda.js" src="../include/DateRange/ipopeng.htm" frameBorder="0" width="132" scrolling="no" height="142">
			</iframe>
		</form>
	</body>
</HTML>
