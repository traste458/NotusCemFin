<%@ Page Language="vb" AutoEventWireup="false" Codebehind="recibosActualizarProducto.aspx.vb" Inherits="BPColSysOP.recibosActualizarProducto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>recibosActualizarProducto</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
		<script language="javascript" type="text/javascript">
		   function validacion(){
		     if(document.Form1.txtProducto.value==""){
		       alert("Digite el nombre del Producto, Por favor");
		       document.Form1.txtProducto.focus();
		       return(false);
		     }
		     if(document.Form1.ddlProveedor.value==""||document.Form1.ddlProveedor.value=="0"){
		       alert("Escoja el Proveedor, Por favor");
		       document.Form1.ddlProveedor.focus();
		       return(false);
		     }
		     if(document.Form1.ddlTipoProducto.value==""||document.Form1.ddlTipoProducto.value=="0"){
		       alert("Escoja el Tipo de Producto, Por favor");
		       document.Form1.ddlTipoProducto.focus();
		       return(false);
		     }
		   }
		</script>
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Actualizar Producto 
				(Referencia) </b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"></asp:label><asp:label id="lblRes" runat="server" ForeColor="Blue" Font-Bold="True" Font-Size="X-Small"></asp:label></TD>
				</TR>
			</TABLE>
			<table class="tabla" width="70%" borderColor="#006699" cellSpacing="1" cellPadding="1"
				border="1">
				<TR>
					<TD class="tdTituloRec" bgColor="#dddddd" colSpan="2">DATOS DEL PRODUCTO 
						(REFERENCIA)</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label3" runat="server" Font-Bold="True">Producto:</asp:label></TD>
					<TD class="tdCampoRec"><font color="blue" size="2"><asp:textbox id="txtProducto" runat="server" Width="250px" MaxLength="40" CssClass="textbox"></asp:textbox><FONT color="#0000ff" size="2">*</FONT></font></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" style="HEIGHT: 21px" bgColor="#dddddd"><asp:label id="Label4" runat="server" Font-Bold="True">Proveedor:</asp:label></TD>
					<TD class="tdCampoRec" style="HEIGHT: 21px"><asp:dropdownlist id="ddlProveedor" runat="server"></asp:dropdownlist><FONT color="#0000ff" size="2">*</FONT></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="Label6" runat="server" Font-Bold="True">Tipo de Producto:</asp:label></TD>
					<TD class="tdCampoRec"><asp:dropdownlist id="ddlTipoProducto" runat="server"></asp:dropdownlist><FONT color="#0000ff" size="2">*<INPUT id="hIdTipoProducto" style="WIDTH: 24px; HEIGHT: 22px" type="hidden" size="1" name="hIdTipoProducto"
								runat="server"></FONT></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd">
						<asp:label id="Label2" runat="server" Font-Bold="True">Material:</asp:label></TD>
					<TD class="tdCampoRec">
						<asp:textbox id="txtMaterial" runat="server" CssClass="textbox" MaxLength="25" Width="100px"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd">
						<asp:label id="Label1" runat="server" Font-Bold="True">Estado:</asp:label></TD>
					<TD class="tdCampoRec">
						<asp:dropdownlist id="ddlEstado" runat="server">
							<asp:ListItem Value="-1">Escoja un Estado</asp:ListItem>
							<asp:ListItem Value="1">ACTIVO</asp:ListItem>
							<asp:ListItem Value="2">INACTIVO</asp:ListItem>
						</asp:dropdownlist></TD>
				</TR>
			</table>
			<table class="tabla" width="80%">
				<tr>
					<td><font color="#0000ff" size="2">*</font> Capo Obligatorio</td>
				</tr>
				<tr>
					<td><br>
						<asp:button id="btnActualizar" runat="server" ForeColor="White" CssClass="botonRec" Text="Actualizar"></asp:button></td>
				</tr>
			</table>
			<br>
		</form>
	</body>
</HTML>
