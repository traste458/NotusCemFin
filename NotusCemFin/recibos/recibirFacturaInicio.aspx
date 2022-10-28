<%@ Page Language="vb" AutoEventWireup="false" Codebehind="recibirFacturaInicio.aspx.vb" Inherits="BPColSysOP.recibirFacturaInicio" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<HTML>
	<HEAD>
		<title>recibirFacturaInicio</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="Visual Basic .NET 7.1" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<LINK href="../include/styleBACK.css" type="text/css" rel="stylesheet">
		<script language="javascript" type="text/javascript">
		    function validacion(){
		      if(document.Form1.ddlTipoProducto.value=="0"||document.Form1.ddlTipoProducto.value==""){
		        alert("Escoja el Tipo de Producto a recibir, Por Favor");
		        document.Form1.ddlTipoProducto.focus();
		        return(false);
		      }
		      if(document.Form1.ddlTipoRecepcion.value=="0"||document.Form1.ddlTipoRecepcion.value==""){
		        alert("Escoja el Tipo de Recepción, Por Favor");
		        document.Form1.ddlTipoRecepcion.focus();
		        return(false);
		      }
		      /*var index;
		      index = document.Form1.ddlTipoRecepcion.selectedIndex;
		      if(document.Form1.ddlTipoRecepcion[index].text.indexOf("TRASLADO")!=-1){
		        if(document.Form1.txtFactura.value==""){
		          alert("Digite número de la Factura, Por favor");
		          document.Form1.txtFactura.focus();
		          return(false);
		        }
		      }*/
		    }
		    
		  /*  function showHideLabel(){
		      var index = document.Form1.ddlTipoRecepcion.selectedIndex;
		      if(document.Form1.ddlTipoRecepcion[index].text.indexOf("TRASLADO")!=-1){
		        document.getElementById("lblAsterisco").innerHTML="*";
		      }else{
		        document.getElementById("lblAsterisco").innerHTML="";
		      }
		    }*/
		</script>
	</HEAD>
	<body class="cuerpo2" >
		<font face="Arial, Helvetica, sans-serif" color="black" size="4"><b>Recibir Factura - 
				Inicio</b></font>
		<hr>
		<form id="Form1" onsubmit="return validacion();" method="post" runat="server">
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
					<TD class="tdTituloRec" bgColor="#dddddd" colSpan="2">INFORMACION DE BUSQUEDA</TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="100" bgColor="#dddddd"><asp:label id="Label6" runat="server" Font-Bold="True">Tipo de Producto:</asp:label></TD>
					<TD class="tdCampoRec"><asp:dropdownlist id="ddlTipoProducto" runat="server"></asp:dropdownlist><FONT color="#0000ff" size="2">*</FONT></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" width="110" bgColor="#dddddd"><asp:label id="Label3" runat="server" Font-Bold="True">Tipo de Recepción:</asp:label></TD>
					<TD class="tdCampoRec"><asp:dropdownlist id="ddlTipoRecepcion" runat="server"></asp:dropdownlist><FONT color="#0000ff" size="2">*</FONT></TD>
				</TR>
				<TR>
					<TD class="tdPrinRec" bgColor="#dddddd"><asp:label id="lblFactura" runat="server" Font-Bold="True">Factura:</asp:label></TD>
					<TD class="tdCampoRec"><font color="blue" size="2"><asp:textbox id="txtFactura" runat="server" MaxLength="25" CssClass="textbox"></asp:textbox><asp:label id="lblAsterisco" runat="server" Font-Size="X-Small"></asp:label></font></TD>
				</TR>
			</table>
			<table class="tabla" width="80%">
				<tr>
					<td><font color="blue" size="2">*</font> Campo Obligatorio&nbsp;
					</td>
				</tr>
				<tr>
					<td><br>
						<asp:button id="btnContinuar" runat="server" ForeColor="White" CssClass="botonRec" Text="Continuar"></asp:button></td>
				</tr>
			</table>
			<br>
		</form>
	</body>
</HTML>
