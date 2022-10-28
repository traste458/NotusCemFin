<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="crearNuevaFactura.aspx.vb" Inherits="BPColSysOP.crearNuevaFactura" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>crearNuevaFactura</title>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
		<script language="javascript" type="text/javascript">
		    function validacion(){
		      if(document.Form1.txtFactura.value==""){
		        alert("Digite la Factura, Por Favor. Si no tiene la Factura,\nse recomienda utilizar para este campo el No. de la Guia");
		        document.Form1.txtFactura.focus();
		        return(false);
		      }
		      if(document.Form1.txtGuia.value==""){
		        alert("Digite la Guía Aerea, Por Favor");
		        document.Form1.txtGuia.focus();
		        return(false);
		      }
		      if(document.Form1.ddlProveedor.value=="0"||document.Form1.ddlProveedor.value==""){
		        alert("Escoja el Proveedor, Por Favor");
		        document.Form1.ddlProveedor.focus();
		        return(false);
		      }
		      if(document.Form1.ddlProducto.value=="0"||document.Form1.ddlProducto.value==""){
		        alert("Escoja el Producto, Por Favor");
		        document.Form1.ddlProducto.focus();
		        return(false);
		      }
		      if(document.Form1.ddlOrigenTraslado){
		        if(document.Form1.ddlOrigenTraslado.value=="0"||document.Form1.ddlOrigenTraslado.value==""){
		          alert("Escoja el Origen del Traslado, Por Favor");
		          document.Form1.ddlOrigenTraslado.focus();
		          return(false);
		        }
		      }
		      if(document.Form1.txtPalets.value==""){
		        alert("Digite el Número de Palets(Guacales) recibidos, Por Favor");
		        document.Form1.txtPalets.focus();
		        return(false);
		      }
		      if(isNaN(document.Form1.txtPalets.value)){
		        alert("El campo Número de Palets(Guacales) es numérico.\nDigite un número válido, Por Favor");
		        document.Form1.txtPalets.focus();
		        return(false);
		      }
		      if(document.Form1.txtCantidad.value==""){
		        alert("Digite la Cantidad recibida aproximada, Por Favor");
		        document.Form1.txtCantidad.focus();
		        return(false);
		      }
		      if(isNaN(document.Form1.txtCantidad.value)){
		        alert("El campo Cantidad recibida aproximada es numérico.\nDigite un número válido, Por Favor");
		        document.Form1.txtCantidad.focus();
		        return(false);
		      }
		      
		      if(document.Form1.txtPeso.value==""){
		        alert("Digite el Peso de la carga, Por Favor");
		        document.Form1.txtPeso.focus();
		        return(false);
		      }
		      if(isNaN(document.Form1.txtPeso.value)){
		        alert("El campo Peso es numérico.\nDigite un número válido, Por Favor");
		        document.Form1.txtPeso.focus();
		        return(false);
		      }
		      if(document.Form1.ddlEstadoRecepcion.value==""||document.Form1.ddlEstadoRecepcion.value=="0"){
		        alert("Escoja el Estado de la Recepción, Por Favor");
		        document.Form1.ddlEstadoRecepcion.focus();
		        return(false);
		      }
		    }
		</script>
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Recibir Factura</b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="recibirFacturaInicio.aspx">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><anthem:label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"
							AutoUpdateAfterCallBack="True"></anthem:label>
						<anthem:label id="lblRes" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Blue"
							AutoUpdateAfterCallBack="True"></anthem:label></TD>
				</TR>
			</TABLE>
			<table class="tabla" width="80%">
				<TR>
					<TD>
						<TABLE class="tabla" id="Table2" width="100%" borderColor="#006699" cellSpacing="1" cellPadding="1"
							border="1">
							<TR>
								<TD class="tdTituloRec" colSpan="2"><asp:label id="Label4" runat="server">INFORMACIÓN DE LA FACTURA</asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label14" runat="server" Font-Bold="True">Factura:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:textbox id="txtFactura" runat="server" CssClass="textbox" MaxLength="25"></asp:textbox><FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec">
									<asp:label id="Label5" runat="server" Font-Bold="True">Orden de Compra:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">
									<asp:textbox id="txtOrdenCompra" runat="server" CssClass="textbox" MaxLength="20"></asp:textbox></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label9" runat="server" Font-Bold="True">Guía Aerea:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:textbox id="txtGuia" runat="server" CssClass="textbox" MaxLength="25"></asp:textbox><FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec">
									<asp:label id="Label1" runat="server" Font-Bold="True">Proveedor:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">
									<anthem:dropdownlist id="ddlProveedor" runat="server" AutoCallBack="True"></anthem:dropdownlist><FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec">
									<asp:label id="Label2" runat="server" Font-Bold="True">Producto:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">
									<anthem:dropdownlist id="ddlProducto" runat="server" AutoUpdateAfterCallBack="True"></anthem:dropdownlist><FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<anthem:panel id="pnlTraslado" runat="server" visible="false">
								<TR>
									<TD class="tdPrinRec">
										<asp:label id="Label3" runat="server" Font-Bold="True"> Origen Traslado:</asp:label></TD>
									<TD class="tdCampoRec" style="HEIGHT: 18px">
										<asp:dropdownlist id="ddlOrigenTraslado" runat="server"></asp:dropdownlist><FONT color="blue" size="2">*</FONT></TD>
								</TR>
							</anthem:panel>
							<TR>
								<TD class="tdPrinRec" width="120"><asp:label id="Label10" runat="server" Font-Bold="True">Número de Palets:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:textbox id="txtPalets" runat="server" Width="48px" CssClass="textbox"></asp:textbox><FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label11" runat="server" Font-Bold="True">Cantidad Aprox.:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:textbox id="txtCantidad" runat="server" Width="48px" CssClass="textbox"></asp:textbox><FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label13" runat="server" Font-Bold="True">Peso:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:textbox id="txtPeso" runat="server" Width="48px"></asp:textbox>(Kg)
									<FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec">
									<asp:label id="Label18" runat="server" Font-Bold="True">Estado Recepción:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px">
									<asp:dropdownlist id="ddlEstadoRecepcion" runat="server"></asp:dropdownlist><FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label12" runat="server" Font-Bold="True">Bodega:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:dropdownlist id="ddlBodega" runat="server"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec">
									<asp:label id="Label6" runat="server" Font-Bold="True">Archivo Recepción:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><INPUT class="textbox" id="flArchivo" style="WIDTH: 400px; HEIGHT: 22px" type="file" size="44"
										name="flArchivo" runat="server"></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label15" runat="server" Font-Bold="True">Observación:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:textbox id="txtObservacion" runat="server" MaxLength="200" Width="300px" TextMode="MultiLine"
										Height="75px" CssClass="textbox"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr>
					<td>
						<font color="blue" size="2">*</font> Campo Obligatorio&nbsp;
					</td>
				</tr>
				<tr>
					<td><br>
						<asp:button id="btnGuardar" runat="server" Text="Registrar Datos" CssClass="botonRec" ForeColor="White"></asp:button></td>
				</tr>
			</table>
			<br>
		</form>
	</body>
</HTML>
