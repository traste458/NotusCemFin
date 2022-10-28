<%@ Page Language="vb" AutoEventWireup="false" Codebehind="recibirFacturaExternaExistentePorTraslado.aspx.vb" Inherits="BPColSysOP.recibirFacturaExternaExistentePorTraslado" culture="es-CO" uiCulture="es-CO" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>recbirFacturaExistentePorTraslado</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
		<script language="javascript" type="text/javascript">
		    function validacion(){
		      if(document.Form1.ddlOrigenTraslado.value==""||document.Form1.ddlOrigenTraslado.value=="0"){
		        alert("Escoja el Origen del Traslado, Por favor");
		        document.Form1.ddlOrigenTraslado.focus();
		        return(false);
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
		        alert("Digite el Peso, Por Favor");
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
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Recibir Factura 
				Existente Por Traslado </b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="verFacturasPendientes.aspx?factura=@f&amp;idTp=@tp&amp;idTr=@idTr">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="X-Small"></asp:label></TD>
				</TR>
			</TABLE>
			<table class="tabla">
				<TR>
					<TD>
						<TABLE class="tabla" borderColor="#006699" cellSpacing="1" cellPadding="1" width="100%"
							border="1">
							<TR>
								<TD class="tdTituloRec" align="center" bgColor="#dddddd" colSpan="4"><asp:label id="Label1" runat="server">INFORMACIÓN REGISTRADA</asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec" style="HEIGHT: 18px" align="center" bgColor="#dddddd"><asp:label id="Label6" runat="server" Font-Bold="True">FACTURA</asp:label></TD>
								<TD class="tdPrinRec" style="HEIGHT: 18px" align="center"><asp:label id="Label2" runat="server" Font-Bold="True">PROVEEDOR</asp:label></TD>
								<TD class="tdPrinRec" style="HEIGHT: 18px" align="center" colSpan="2"><asp:label id="Label3" runat="server" Font-Bold="True">PRODUCTO</asp:label></TD>
							</TR>
							<TR>
								<TD align="center"><asp:label id="lblFactura" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label>&nbsp;</TD>
								<TD align="center"><asp:label id="lblProveedor" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label>&nbsp;</TD>
								<TD align="center" colSpan="2"><asp:label id="lblProducto" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label>&nbsp;</TD>
							</TR>
							<TR>
								<TD class="tdPrinRec" style="HEIGHT: 15px" align="center" bgColor="#dddddd"><asp:label id="Label16" runat="server" Font-Bold="True">GUÍA AÉREA</asp:label></TD>
								<TD class="tdPrinRec" style="HEIGHT: 15px" align="center"><asp:label id="Label7" runat="server" Font-Bold="True">ORDEN DE COMPRA</asp:label></TD>
								<TD class="tdPrinRec" style="HEIGHT: 15px" align="center"><asp:label id="Label8" runat="server" Font-Bold="True">FECHA RECIBO ACTUAL</asp:label></TD>
								<TD class="tdPrinRec" style="HEIGHT: 15px" align="center"><asp:label id="Label5" runat="server" Font-Bold="True">FECHA DE SALIDA</asp:label></TD>
							</TR>
							<TR>
								<TD style="HEIGHT: 13px" align="center"><asp:label id="lblGuia" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label>&nbsp;</TD>
								<TD style="HEIGHT: 13px" align="center"><asp:label id="lblOrdenCompra" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label>&nbsp;</TD>
								<TD style="HEIGHT: 13px" align="center">&nbsp;
									<asp:label id="lblFecha" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
								<TD style="HEIGHT: 13px" align="center"><asp:label id="lblFechaSalida" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label>&nbsp;</TD>
							</TR>
							<TR>
								<TD class="tdPrinRec" style="HEIGHT: 13px" align="center"><asp:label id="Label14" runat="server" Font-Bold="True">CANTIDAD RECIBIDA ACTUAL</asp:label></TD>
								<TD class="tdPrinRec" style="HEIGHT: 13px" align="center"><asp:label id="Label17" runat="server" Font-Bold="True">CANTIDAD PEDIDA ACTUAL</asp:label></TD>
								<TD class="tdPrinRec" style="HEIGHT: 13px" align="center"><asp:label id="Label18" runat="server" Font-Bold="True">CANTIDAD PROCESADA</asp:label></TD>
								<TD class="tdPrinRec" style="HEIGHT: 13px" align="center" colSpan="1"><asp:label id="Label21" runat="server" Font-Bold="True">ESTADO RECEPCION ACTUAL</asp:label></TD>
							</TR>
							<TR>
								<TD align="center">&nbsp;
									<asp:label id="lblCantidad" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
								<TD align="center">&nbsp;
									<asp:label id="lblCantidadPedida" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
								<TD align="center">&nbsp;
									<asp:label id="lblCantidadProcesada" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
								<TD align="center">&nbsp;
									<asp:label id="lblEstadoRecepcion" runat="server" ForeColor="MediumBlue" Font-Bold="True"></asp:label></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<TR>
					<TD>
						<TABLE class="tabla" id="Table2" borderColor="#006699" cellSpacing="1" cellPadding="1"
							width="100%" border="1">
							<TR>
								<TD class="tdTituloRec" colSpan="2"><asp:label id="Label4" runat="server">INFORMACIÓN ADICIONAL</asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec" width="120"><asp:label id="Label9" runat="server" Font-Bold="True">Origen Traslado:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:dropdownlist id="ddlOrigenTraslado" runat="server"></asp:dropdownlist><FONT color="blue" size="2">*</FONT></TD>
							</TR>
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
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:textbox id="txtPeso" runat="server" Width="48px" CssClass="textbox"></asp:textbox>(Kg)
									<FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label19" runat="server" Font-Bold="True">Estado Recepción:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:dropdownlist id="ddlEstadoRecepcion" runat="server"></asp:dropdownlist><FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label12" runat="server" Font-Bold="True">Bodega:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:dropdownlist id="ddlBodega" runat="server"></asp:dropdownlist></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label15" runat="server" Font-Bold="True">Observación:</asp:label></TD>
								<TD class="tdCampoRec" style="HEIGHT: 18px"><asp:textbox id="txtObservacion" runat="server" Width="300px" CssClass="textbox" MaxLength="200"
										TextMode="MultiLine" Height="75px"></asp:textbox></TD>
							</TR>
						</TABLE>
					</TD>
				</TR>
				<tr>
					<td><font color="blue" size="2">*</font> Campo Obligatorio&nbsp;
					</td>
				</tr>
				<tr>
					<td><br>
						<asp:button id="btnGuardar" runat="server" ForeColor="White" CssClass="botonRec" Text="Registrar Datos"></asp:button><INPUT id="hTipoRecepcion" style="WIDTH: 8px; HEIGHT: 22px" type="hidden" size="1" name="hTipoRecepcion"
							runat="server"></td>
				</tr>
			</table>
			<br>
		</form>
	</body>
</HTML>
