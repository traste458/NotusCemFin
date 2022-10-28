<%@ Page Language="vb" AutoEventWireup="false" Codebehind="inicioBuscarOperadorLogistico.aspx.vb" Inherits="BPColSysOP.inicioBuscarOperadorLogistico" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>inicioBuscarOperadorLogistico</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Buscar Operador 
				Logístico (Orígenes de Traslado)</b></font>
		<hr>
		<form id="Form1" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Red"></asp:label><asp:label id="lblRes" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Blue"></asp:label></TD>
				</TR>
			</TABLE>
			<table class="tabla" width="70%" borderColor="#006699" cellSpacing="1" cellPadding="1"
				border="1">
				<TR>
					<TD class="tdTituloRec" bgColor="#dddddd" colSpan="2">PARAMETROS DE BUSQUEDA</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label3" runat="server" Font-Bold="True">Nombre:</asp:label></TD>
					<TD class="tdCampoRec"><font color="blue" size="2"><asp:textbox id="txtNombre" runat="server" CssClass="textbox" MaxLength="100" Width="250px"></asp:textbox><FONT color="gray" size="2">*</FONT></font></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label2" runat="server" Font-Bold="True">Identificación:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtIdentificacion" runat="server" CssClass="textbox" MaxLength="30"></asp:textbox><FONT color="gray" size="2">*</FONT></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" style="HEIGHT: 21px" bgColor="#dddddd"><asp:label id="Label4" runat="server" Font-Bold="True">Ciudad:</asp:label></TD>
					<TD class="tdCampoRec" style="HEIGHT: 21px"><asp:dropdownlist id="ddlCiudad" runat="server"></asp:dropdownlist><FONT color="#0000ff" size="2"></FONT></TD>
				</TR>
			</table>
			<table class="tabla" width="80%">
				<tr>
					<td><font color="gray" size="2">*</font> Puede digitar la totalida o solo una parte 
						del valor&nbsp; que desea buscar</td>
				</tr>
				<tr>
					<td><br>
						<asp:button id="btnBuscar" runat="server" ForeColor="White" CssClass="botonRec" Text="Buscar"></asp:button></td>
				</tr>
			</table>
			<br>
		</form>
	</body>
</HTML>
