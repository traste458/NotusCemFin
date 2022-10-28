<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="recibosCrearProducto.aspx.vb" Inherits="BPColSysOP.recibosCrearProducto" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>recibosCrearProducto</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
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
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Crear Nuevo 
				Producto (Referencia)</b></font>
		<hr>
		<font color="gray" size="2"><i>
				<ul>
					Solo se pueden crear Productos (Referencias) que no tengan distribución 
					Regional.
				</ul>
			</i></font>
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
			<table class="tabla" borderColor="#006699" cellSpacing="1" cellPadding="1" width="70%"
				border="1">
				<TR>
					<TD class="tdTituloRec" bgColor="#dddddd" colSpan="2">PARAMETROS DE BUSQUEDA</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd">&nbsp;<asp:label id="Label3" runat="server" Font-Bold="True">Producto:</asp:label></TD>
					<TD class="tdCampoRec"><font color="blue" size="2"><asp:textbox id="txtProducto" runat="server" CssClass="textbox" MaxLength="40" Width="250px"></asp:textbox><FONT color="#0000ff" size="2">*</FONT></font></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" style="HEIGHT: 21px" bgColor="#dddddd">&nbsp;<asp:label id="Label4" runat="server" Font-Bold="True">Proveedor:</asp:label></TD>
					<TD class="tdCampoRec" style="HEIGHT: 21px"><asp:dropdownlist id="ddlProveedor" runat="server"></asp:dropdownlist><FONT color="#0000ff" size="2">*</FONT></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd">&nbsp;<asp:label id="Label6" runat="server" Font-Bold="True">Tipo de Producto:</asp:label></TD>
					<TD class="tdCampoRec"><FONT color="#0000ff" size="2">
							<asp:dropdownlist id="ddlTipoProducto" runat="server"></asp:dropdownlist>*</FONT></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd">&nbsp;<asp:label id="Label2" runat="server" Font-Bold="True"> Material:</asp:label></TD>
					<TD class="tdCampoRec"><asp:TextBox id="txtMaterial" runat="server" MaxLength="10" Width="100px" CssClass="textbox"></asp:TextBox></TD>
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
					<TD align="center"><asp:label id="Label1" runat="server" CssClass="tdTituloRec" Width="100%">LISTADO DE PRODUCTOS REGISTRADOS</asp:label></TD>
				</TR>
				<tr>
					<td><anthem:datagrid id="dgProductos" runat="server" CssClass="tabla" AllowPaging="True" PageSize="20"
							AutoGenerateColumns="False" ShowFooter="True" GridLines="Vertical" CellPadding="3" BackColor="White"
							BorderWidth="1px" BorderStyle="None" BorderColor="#999999">
							<PagerStyle Font-Size="X-Small" Font-Bold="True" HorizontalAlign="Center" ForeColor="Indigo"
								BackColor="#999999" Mode="NumericPages"></PagerStyle>
							<AlternatingItemStyle BackColor="Gainsboro"></AlternatingItemStyle>
							<FooterStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="#000084" BackColor="#CCCCCC"></FooterStyle>
							<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#008A8C"></SelectedItemStyle>
							<ItemStyle ForeColor="Black" BackColor="#EEEEEE"></ItemStyle>
							<HeaderStyle Font-Bold="True" HorizontalAlign="Center" ForeColor="White" BackColor="#000084"></HeaderStyle>
							<Columns>
								<asp:BoundColumn DataField="idproducto" HeaderText="ID"></asp:BoundColumn>
								<asp:BoundColumn DataField="proveedor" HeaderText="PROVEEDOR"></asp:BoundColumn>
								<asp:BoundColumn DataField="producto" HeaderText="PRODUCTO (REFERENCIA)"></asp:BoundColumn>
								<asp:BoundColumn DataField="material" HeaderText="MATERIAL">
									<ItemStyle HorizontalAlign="Center"></ItemStyle>
								</asp:BoundColumn>
								<asp:BoundColumn DataField="tipoProducto" HeaderText="TIPO DE PRODUCTO"></asp:BoundColumn>
							</Columns>
						</anthem:datagrid></td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
