<%@ Page Language="vb" AutoEventWireup="false" Codebehind="crearOperadorLogistico.aspx.vb" Inherits="BPColSysOP.crearOperadorLogistico" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>crearOperadorLogistico</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
		<script language="javascript" type="text/javascript">
		   function validacion(){
		     if(document.Form1.txtNombre.value==""){
		       alert("Digite el nombre del Operador Logístico, Por favor");
		       document.Form1.txtNombre.focus();
		       return(false);
		     }
		     if(document.Form1.ddlCiudad.value==""||document.Form1.ddlCiudad.value=="0"){
		       alert("Escoja la Ciudad en la que se encuentra el Operador Logístico, Por favor");
		       document.Form1.ddlCiudad.focus();
		       return(false);
		     }
		     if(document.Form1.txtEmail.value!=""){
		       var emailRegEx = new RegExp("^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$");
		       str = document.Form1.txtEmail.value;
		       if(!str.match(emailRegEx)){
		         alert("El E-Mail digitado no tiene el formato adecuado. Por favor digite un E-Mail válido");
		         document.Form1.txtEmail.focus();
		         return(false);
		       }
		     }
		   }
		</script>
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Crear Nuevo 
				Operador Logístico (Orígenes de Traslado)</b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
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
					<TD class="tdTituloRec" bgColor="#dddddd" colSpan="2">INFORMACION A REGISTRAR</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label3" runat="server" Font-Bold="True">Nombre:</asp:label></TD>
					<TD class="tdCampoRec"><font color="blue" size="2"><asp:textbox id="txtNombre" runat="server" CssClass="textbox" MaxLength="100" Width="250px"></asp:textbox><FONT color="#0000ff" size="2">*</FONT></font></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label2" runat="server" Font-Bold="True">Identificación:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtIdentificacion" runat="server" CssClass="textbox" MaxLength="30"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label5" runat="server" Font-Bold="True">Dirección:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtDireccion" runat="server" CssClass="textbox" MaxLength="100" Width="250px"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label6" runat="server" Font-Bold="True">Telefonos:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtTelefonos" runat="server" CssClass="textbox" MaxLength="30"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label7" runat="server" Font-Bold="True">E-Mail:</asp:label></TD>
					<TD class="tdCampoRec"><asp:textbox id="txtEmail" runat="server" CssClass="textbox" MaxLength="50" Width="250px"></asp:textbox></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" style="HEIGHT: 21px" bgColor="#dddddd"><asp:label id="Label4" runat="server" Font-Bold="True">Ciudad:</asp:label></TD>
					<TD class="tdCampoRec" style="HEIGHT: 21px"><asp:dropdownlist id="ddlCiudad" runat="server"></asp:dropdownlist><FONT color="#0000ff" size="2">*</FONT></TD>
				</TR>
			</table>
			<table class="tabla" width="80%">
				<tr>
					<td><font color="#0000ff" size="2">*</font> Capo Obligatorio</td>
				</tr>
				<tr>
					<td><br>
						<asp:button id="btnGuardar" runat="server" ForeColor="White" CssClass="botonRec" Text="Guardar"></asp:button></td>
				</tr>
			</table>
			<br>
			<table class="tabla" cellSpacing="0" cellPadding="0">
				<TR>
					<TD align="center"><asp:label id="Label1" runat="server" CssClass="tdTituloRec" Width="100%">LISTADO DE OPERADORES LOGÍSTICOS REGISTRADOS</asp:label></TD>
				</TR>
				<tr>
					<td><asp:datagrid id="dgDatos" runat="server" CssClass="tabla" AllowPaging="True" PageSize="20" AutoGenerateColumns="False"
							GridLines="Vertical" CellPadding="3" BackColor="White" BorderWidth="1px" BorderStyle="None"
							BorderColor="#999999">
							<PagerStyle Font-Size="X-Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="Indigo"
								BackColor="#999999" Mode="NumericPages"></PagerStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<FooterStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#000084" BackColor="#CCCCCC"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="nombre" HeaderText="NOMBRE"></asp:BoundColumn>
								<asp:BoundColumn DataField="identificacion" HeaderText="IDENTIFICACION"></asp:BoundColumn>
								<asp:BoundColumn DataField="direccion" HeaderText="DIRECCION"></asp:BoundColumn>
								<asp:BoundColumn DataField="telefonos" HeaderText="TELEFONO"></asp:BoundColumn>
								<asp:BoundColumn DataField="email" HeaderText="E-MAIL"></asp:BoundColumn>
								<asp:BoundColumn DataField="ciudad" HeaderText="CIUDAD"></asp:BoundColumn>
								<asp:BoundColumn DataField="fechaCreacion" HeaderText="FECHA DE CREACION">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
							</Columns>
						</asp:datagrid></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
