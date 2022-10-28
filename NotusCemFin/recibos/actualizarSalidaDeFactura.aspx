<%@ Page Language="vb" AutoEventWireup="false" Codebehind="actualizarSalidaDeFactura.aspx.vb" Inherits="BPColSysOP.actualizarSalidaDeFactura" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>actualizarSalidaDeFactura</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
		<script language="javascript" type="text/javascript">
		    function validacion(){
		      if(document.Form1.fechaSalida.value==""){
		        alert("Escoja la Fecha de Salida, Por favor");
		        document.Form1.fechaSalida.focus();
		        return(false);
		      }
		      if(document.Form1.ddlDestinoTraslado.value==""||document.Form1.ddlDestinoTraslado.value=="0"){
		        alert("Escoja el destino del Traslado, Por favor");
		        document.Form1.ddlDestinoTraslado.focus();
		        return(false);
		      }
		      var index = document.Form1.ddlDestinoTraslado.selectedIndex;
		      var destinoTraslado=document.Form1.ddlDestinoTraslado[index].text;
		      if(document.Form1.fechaSalida.value==document.Form1.hFechaSalidaActual.value&&
		         destinoTraslado==document.Form1.hDestinoTrasladoActual.value){
		       alert("No se ha efectuado ninguna modificación sobre los datos Actuales.\nNo se actualizarán los datos.");
		       document.Form1.ddlDestinoTraslado.focus();
		       return(false);
		      }
		    }
		</script>
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Actualizar Salida 
				de Factura </b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
			<TABLE id="Table1" cellSpacing="1" cellPadding="1" width="80%" border="0">
				<TR>
					<TD colSpan="2"><asp:hyperlink id="hlRegresar" runat="server" NavigateUrl="resultadoBuscarFacturasRecibidas.aspx">Regresar</asp:hyperlink><br>
						<br>
					</TD>
				</TR>
				<TR>
					<TD align="center" colSpan="2"><asp:label id="lblError" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Red"></asp:label><asp:label id="lblRes" runat="server" Font-Size="X-Small" Font-Bold="True" ForeColor="Blue"></asp:label></TD>
				</TR>
			</TABLE>
			<table class="tabla" width="95%">
				<TR>
					<TD>
						<TABLE class="tabla" id="Table2" borderColor="#006699" cellSpacing="1" cellPadding="1"
							width="70%" border="1">
							<TR>
								<TD class="tdTituloRec" colSpan="2"><asp:label id="Label4" runat="server">INFORMACIÓN DE LA FACTURA</asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label14" runat="server" Font-Bold="True">Factura:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblFactura" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label7" runat="server" Font-Bold="True">Orden de Compra:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblOrdenCompra" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label9" runat="server" Font-Bold="True">Guía Aerea:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblGuia" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label8" runat="server" Font-Bold="True">Tipo de Producto:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblTipoProducto" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label1" runat="server" Font-Bold="True">Proveedor:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblProveedor" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label2" runat="server" Font-Bold="True">Producto:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblProducto" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label3" runat="server" Font-Bold="True">Tipo de Recepción:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblTipoRecepcion" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec" width="120"><asp:label id="Label10" runat="server" Font-Bold="True">Número de Palets:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblPalet" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label11" runat="server" Font-Bold="True">Cantidad Aprox.:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblCantidad" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR id="trUnidades" runat="server">
								<TD class="tdPrinRec">
									<asp:label id="Label5" runat="server" Font-Bold="True">Unidades por Caja:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblUnidadesCaja" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR id="trCajas" runat="server">
								<TD class="tdPrinRec">
									<asp:label id="Label6" runat="server" Font-Bold="True">Cajas por Palet:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblCajasPalet" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label13" runat="server" Font-Bold="True">Peso:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblPeso" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label12" runat="server" Font-Bold="True">Bodega:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblBodega" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label16" runat="server" Font-Bold="True">Fecha de Recepción:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblFechaRecepcion" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec">
									<asp:label id="Label19" runat="server" Font-Bold="True">Estado Recepción:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblEstadoRecepcion" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label17" runat="server" Font-Bold="True">Fecha de Salida</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<INPUT class="textbox" id="fechaSalida" readOnly size="11" name="fechaSalida" runat="server"><A hideFocus onclick="if(self.gfPop)gfPop.fStartPop(document.Form1.fechaSalida,document.Form1.fechaFinal);return false;"
										href="javascript:void(0)"><IMG class="PopcalTrigger" height="22" alt="Seleccione una Fecha Inicial" src="../include/HelloWorld/calbtn.gif"
											width="34" align="absMiddle" border="0"></A>&nbsp;<FONT color="blue" size="2">*</FONT><INPUT id="hFechaSalidaActual" style="WIDTH: 8px; HEIGHT: 22px" type="hidden" size="1"
										name="hFechaSalidaActual" runat="server"><INPUT id="fechaFinal" style="WIDTH: 8px; HEIGHT: 22px" type="hidden" size="1" name="fechaFinal"><INPUT id="hDestinoTrasladoActual" style="WIDTH: 8px; HEIGHT: 22px" type="hidden" size="1"
										name="hDestinoTrasladoActual" runat="server"><INPUT id="hIdTipoProducto" style="WIDTH: 8px; HEIGHT: 22px" type="hidden" size="1" name="hIdTipoProducto"
										runat="server"></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label18" runat="server" Font-Bold="True">Destino Traslado</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:dropdownlist id="ddlDestinoTraslado" runat="server"></asp:dropdownlist>&nbsp;<FONT color="blue" size="2">*</FONT></TD>
							</TR>
							<TR>
								<TD class="tdPrinRec"><asp:label id="Label15" runat="server" Font-Bold="True">Observación:</asp:label></TD>
								<TD class="tdCampoRec">&nbsp;<asp:label id="lblObservacion" runat="server" Font-Bold="True" ForeColor="MediumBlue"></asp:label></TD>
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
						<asp:button id="btnGuardar" runat="server" ForeColor="White" CssClass="botonRec" Text="Actualizar Datos"></asp:button></td>
				</tr>
			</table>
			<br>
			<!-- iframe para uso de selector de fechas --><iframe id="gToday:contrast:agenda.js" style="Z-INDEX: 999; LEFT: -500px; VISIBILITY: visible; POSITION: absolute; TOP: -500px"
				name="gToday:contrast:agenda.js" src="../include/DateRange/ipopeng.htm" frameBorder="0" width="132" scrolling="no" height="142">
			</iframe>
		</form>
	</body>
</HTML>
