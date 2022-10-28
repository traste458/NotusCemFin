<%@ Page Language="vb" AutoEventWireup="false" Codebehind="buscarOperadorLogistico.aspx.vb" Inherits="BPColSysOP.buscarOperadorLogistico" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>buscarOperadorLogistico</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Resultado Búsqueda 
				Operador Logístico (Orígenes de Traslado)</b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="inicioBuscarOperadorLogistico.aspx">Nueva Búsqueda</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Red"></asp:label></TD>
				</TR>
			</TABLE>
			<br>
			<asp:Panel ID="pnlDatos" runat="server">
				<TABLE class="tabla" cellSpacing="0" cellPadding="0">
					<TR>
						<TD align="center">
							<asp:label id="Label1" runat="server" Width="100%" CssClass="tdTituloRec">LISTADO DE OPERADORES LOGÍSTICOS REGISTRADOS</asp:label></TD>
					</TR>
					<TR>
						<TD>
							<asp:datagrid id="dgDatos" runat="server" CssClass="tabla" BorderColor="#999999" BorderStyle="None"
								BorderWidth="1px" BackColor="White" CellPadding="3" GridLines="Vertical" AutoGenerateColumns="False"
								PageSize="20" ShowFooter="True">
								<FooterStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#000084" BackColor="#CCCCCC"></FooterStyle>
								<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
								<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
								<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
								<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#000084"></HeaderStyle>
								<Columns>
									<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="nombre" HeaderText="NOMBRE"></asp:HyperLinkColumn>
									<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="identificacion" HeaderText="IDENTIFICACION"></asp:HyperLinkColumn>
									<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="direccion" HeaderText="DIRECCION"></asp:HyperLinkColumn>
									<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="telefonos" HeaderText="TELEFONO"></asp:HyperLinkColumn>
									<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="email" HeaderText="E-MAIL"></asp:HyperLinkColumn>
									<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="ciudad" HeaderText="CIUDAD"></asp:HyperLinkColumn>
									<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="fechaCreacion" HeaderText="FECHA DE CREACION"
										DataTextFormatString="{0:dd-MMM-yyyy}">
										<ItemStyle HorizontalAlign="Center"></ItemStyle>
									</asp:HyperLinkColumn>
									<asp:HyperLinkColumn DataNavigateUrlField="url" DataTextField="estado" HeaderText="ESTADO"></asp:HyperLinkColumn>
								</Columns>
								<PagerStyle Font-Size="X-Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="Indigo"
									BackColor="#999999" Mode="NumericPages"></PagerStyle>
							</asp:datagrid></TD>
					</TR>
				</TABLE>
			</asp:Panel>
		</form>
	</body>
</HTML>
