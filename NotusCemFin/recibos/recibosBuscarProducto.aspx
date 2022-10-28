<%@ Page Language="vb" AutoEventWireup="false" Codebehind="recibosBuscarProducto.aspx.vb" Inherits="BPColSysOP.recibosBuscarProducto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>recibosBuscarProducto</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Buscar Productos 
				(Referencias) - Inicio</b></font>
		<hr>
		<font color="gray" size="2"><i>
				<ul>
					Solo se listan Productos (Referencias) que no tienen distribución Regional.
				</ul>
			</i></font>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"></asp:label></TD>
				</TR>
			</TABLE>
			<table class="tabla" width="70%" borderColor="#006699" cellSpacing="1" cellPadding="1"
				border="1">
				<TR>
					<TD class="tdTituloRec" bgColor="#dddddd" colSpan="2">PARAMETROS DE BUSQUEDA</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label3" runat="server" Font-Bold="True">Producto:</asp:label></TD>
					<TD class="tdCampoRec"><font color="blue" size="2"><asp:textbox id="txtProducto" runat="server" Width="250px" MaxLength="25" CssClass="textbox"></asp:textbox><FONT color="gray" size="2">*</FONT></font></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" style="HEIGHT: 21px" bgColor="#dddddd"><asp:label id="Label4" runat="server" Font-Bold="True">Proveedor:</asp:label></TD>
					<TD class="tdCampoRec" style="HEIGHT: 21px"><asp:dropdownlist id="ddlProveedor" runat="server"></asp:dropdownlist><FONT color="#0000ff" size="2"></FONT></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label6" runat="server" Font-Bold="True">Tipo de Producto:</asp:label></TD>
					<TD class="tdCampoRec"><asp:dropdownlist id="ddlTipoProducto" runat="server"></asp:dropdownlist><FONT color="#0000ff" size="2"></FONT></TD>
				</TR>
			</table>
			<table class="tabla" width="80%">
				<tr>
					<td style="HEIGHT: 18px"><font color="gray" size="2">*</font> Se puede digitar el 
						nombre del Producto completo o parte de él</td>
				</tr>
				<tr>
					<td><br>
						<asp:button id="btnbuscar" runat="server" ForeColor="White" CssClass="botonRec" Text="Buscar"></asp:button></td>
				</tr>
			</table>
			<br>
		</form>
	</body>
</HTML>
