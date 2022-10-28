Imports System.Data.SqlClient
Imports System.Text
Imports System.Web.Mail

Partial Class actualizarSalidaDeFactura
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Dim idFactura As Integer, isFrom As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me)
            lblError.Text = ""
            lblRes.Text = ""
            idFactura = Request.QueryString("idF")
            isFrom = Request.QueryString("isFrom")
            If Not Me.IsPostBack Then
                If isFrom = "f" Then
                    trUnidades.Style.Add("display", "none")
                    trCajas.Style.Add("display", "none")
                End If
                getDestinosTraslado()
                getInformacionFactura()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message
        End Try
    End Sub

    Private Sub getDestinosTraslado()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtOperadores As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idOperadorLogistico,nombre from OperadorLogistico where estado=1"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtOperadores)
            With ddlDestinoTraslado
                .DataSource = dtOperadores
                .DataTextField = "nombre"
                .DataValueField = "idOperadorLogistico"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Destino", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al trartar de obtener el listado de Orígenes de Traslado. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub getInformacionFactura()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String

        If isFrom = "fe" Then
            sqlSelect = "select isnull(idfactura2,'') as factura,isnull(ordenCompra,'') as ordenCompra,isnull(guia_aerea,'')"
            sqlSelect += " as guia,(select tipoProducto from TipoProducto with(nolock) where idTipoProducto=fe.idTipoProducto)"
            sqlSelect += " as tipoProducto,(select proveedor from proveedores with(nolock) where idproveedor=fe.idproveedor) as"
            sqlSelect += " proveedor,(select producto from productos with(nolock) where idproducto=fe.idproducto) as producto,"
            sqlSelect += " (select tipoRecepcion from TipoRecepcion with(nolock) where idTipoRecepcion=fe.idTipoRecepcion) as"
            sqlSelect += " tipoRecepcion,numeroPalets,cantidadAprox,peso,isnull((select bodega from bodegas with(nolock) where"
            sqlSelect += " idbodega=fe.idBodega),'') as bodega,fecha,(select estadoRecepcion from EstadoRecepcionFactura with(nolock)"
            sqlSelect += " where idEstadoRecepcion=fe.idEstadoRecepcion) as estadoRecepcion,isnull(observacion,'') as observacion,"
            sqlSelect += " isnull(convert(varchar,fechaSalida,103),'')as fechaSalida,isnull((select top 1 idOperadorLogistico from"
            sqlSelect += " SalidaDeFactura with(nolock) where idFactura=fe.idFactura and fechaSalida=fe.fechaSalida),0) as"
            sqlSelect += " idDestinoTraslado,fe.idTipoProducto,unidades_caja,cajas_estiva from facturas_externas fe with(nolock)"
            sqlSelect += " where fe.idfactura = @idFactura"
        Else
            sqlSelect = "select isnull(factura,'') as factura,isnull(ordenCompra,'') as ordenCompra,isnull(guia,'') as guia,"
            sqlSelect += " (select tipoProducto from TipoProducto with(nolock) where idTipoProducto=f.idTipoProducto) as"
            sqlSelect += " tipoProducto,(select proveedor from proveedores with(nolock) where idproveedor=f.idProveedor) as"
            sqlSelect += " as proveedor,(select producto from productos with(nolock) where idproducto=f.idProducto) as producto,"
            sqlSelect += " (select tipoRecepcion from TipoRecepcion with(nolock) where idTipoRecepcion=f.idTipoRecepcion) as"
            sqlSelect += "  tipoRecepcion,numeroPalets,cantidadAprox,peso,isnull((select bodega from bodegas with(nolock) "
            sqlSelect += " where idbodega=f.idBodega),'') as bodega,fechaLlegada,(select estadoRecepcion from EstadoRecepcionFactura "
            sqlSelect += " with(nolock) where idEstadoRecepcion=f.idEstadoRecepcion) as estadoRecepcion,isnull(observacion,'') "
            sqlSelect += " as observacion,isnull(convert(varchar,fechaSalida,103),'')as fechaSalida,isnull((select top 1 "
            sqlSelect += " idOperadorLogistico from SalidaDeFactura with(nolock) where idFactura=f.idFactura and "
            sqlSelect += " fechaSalida=f.fechaSalida),0) as idDestinoTraslado,idTipoProducto from Factura f where idFactura=@idFactura"
        End If

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idFactura", SqlDbType.Int).Value = idFactura
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            With sqlRead
                If .Read Then
                    lblFactura.Text = .Item("factura").ToString
                    lblOrdenCompra.Text = .Item("ordenCompra").ToString
                    lblGuia.Text = .Item("guia").ToString
                    lblTipoProducto.Text = .Item("tipoProducto").ToString
                    lblProveedor.Text = .Item("proveedor").ToString
                    lblProducto.Text = .Item("producto").ToString
                    lblTipoRecepcion.Text = .Item("tipoRecepcion").ToString
                    lblPalet.Text = .Item("numeroPalets").ToString
                    lblCantidad.Text = .Item("cantidadAprox").ToString
                    lblPeso.Text = .Item("peso").ToString
                    lblBodega.Text = .Item("bodega").ToString
                    lblFechaRecepcion.Text = String.Format("{0:dd-MMM-yyyy}", CDate(.Item("fechaLlegada")))
                    lblEstadoRecepcion.Text = .Item("estadoRecepcion").ToString
                    lblObservacion.Text = .Item("observacion").ToString
                    fechaSalida.Value = .Item("fechaSalida").ToString
                    hFechaSalidaActual.Value = .Item("fechaSalida").ToString
                    With ddlDestinoTraslado
                        .SelectedIndex = .Items.IndexOf(.Items.FindByValue(sqlRead.Item("idDestinoTraslado").ToString))
                    End With
                    hIdTipoProducto.Value = .Item("idTipoProducto").ToString
                    If isFrom = "fe" Then
                        lblUnidadesCaja.Text = .Item("unidades_caja").ToString
                        lblCajasPalet.Text = .Item("cajas_estiva").ToString
                    End If
                End If
                .Close()
            End With
            hDestinoTrasladoActual.Value = ddlDestinoTraslado.SelectedItem.Text
        Catch ex As Exception
            Throw New Exception("Error tratando de obtener la Información de la Factura. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub actualizarDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlTransaccion As SqlTransaction, sqlQuery, sqlUpdate As String

        If isFrom = "fe" Then
            sqlUpdate = "update facturas_externas set estado=6, fechaSalida=@fechaSalida where idfactura=@idFactura"
        Else
            sqlUpdate = "update Factura set fechaSalida=@fechaSalida,estado=6 where idFactura=@idFactura"
        End If

        If hFechaSalidaActual.Value = "" Then
            sqlQuery = "insert into SalidaDeFactura values(@idFactura,@idOperador,@fechaSalida,@tabla,getdate(),@idUsuario)"
        Else
            sqlQuery = "update SalidaDeFactura set idOperadorLogistico=@idOperador,fechaSalida=@fechaSalida,fechaRegistro=getdate(),"
            sqlQuery += "idUsuario=@idUsuario where idFactura=@idFactura and convert(varchar,fechaSalida,112)=@fechaSalidaActual "
        End If

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlUpdate)
            With sqlComando.Parameters
                .Add("@idFactura", SqlDbType.Int).Value = idFactura
                .Add("@idOperador", SqlDbType.Int).Value = ddlDestinoTraslado.SelectedValue
                .Add("@fechaSalida", SqlDbType.SmallDateTime).Value = CDate(fechaSalida.Value)
                .Add("@tabla", SqlDbType.VarChar).Value = isFrom
                .Add("@idUsuario", SqlDbType.Int).Value = Session("usxp001")
                If hFechaSalidaActual.Value <> "" Then
                    .Add("@fechaSalidaActual", SqlDbType.VarChar).Value = String.Format("{0:yyyyMMdd}", CDate(hFechaSalidaActual.Value))
                End If
            End With
            sqlConexion.Open()
            sqlTransaccion = sqlConexion.BeginTransaction
            sqlComando.Transaction = sqlTransaccion
            sqlComando.ExecuteNonQuery()
            sqlComando.CommandText = sqlQuery
            sqlComando.ExecuteNonQuery()
            sqlTransaccion.Commit()
            lblRes.Text = "Los datos se Actualizaron satisfactoriamente.<br><br> "
            hFechaSalidaActual.Value = fechaSalida.Value
            hDestinoTrasladoActual.Value = ddlDestinoTraslado.SelectedItem.Text
        Catch ex As Exception
            If Not sqlTransaccion Is Nothing Then sqlTransaccion.Rollback()
            Throw New Exception("Error al tratar de Actualizar información. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            actualizarDatos()
            enviarMail()
        Catch ex As Exception
            lblError.Text = ex.Message & "<br><br>"
        End Try
    End Sub

    Private Function armarCuerpoMail() As String
        Dim cuerpoMail As New StringBuilder

        Try
            Dim nombreSitio, logo As String
            With ConfigurationManager.AppSettings
                nombreSitio = .Item("nombreSitio")
                logo = .Item("logo")
            End With
            With cuerpoMail
                .Append("<HTML>")
                .Append(" <HEAD>")
                .Append("  <LINK href='" & nombreSitio & "/include/styleBACK.css' type='text/css' rel='stylesheet'>")
                .Append(" </HEAD>")
                .Append("<body class='cuerpo2'>")
                .Append(" <table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'>")
                .Append("  <tr class='encabezadoMail'>")
                .Append("   <td width='220'bgcolor='#e0e0e0'><img src='" & logo & "'></td>")
                .Append("   <td><b>NOTIFICACION SALIDA DE FACTURA</b></td>")
                .Append("  </tr>")
                .Append("</table>")
                .Append("<br>")
                If Now.Hour < 12 Then
                    .Append("<font class='fuente'>Buenos Días")
                Else
                    .Append("<font class='fuente'>Buenas Tardes")
                End If
                .Append("<br><br>Se acaba de registrar en el sistema, la Salida de una Factura con la siguiente información:")
                .Append("</font><br><br>")
                .Append("<TABLE class='tabla' id='Table2' borderColor='#006699' cellSpacing='1' cellPadding='1' border='1' >")
                .Append(" <TR>")
                .Append("  <TD class='tdTituloRec' colSpan='2'>INFORMACIÓN REGISTRADA</TD> ")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>FACTURA:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblFactura.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>ORDEN DE COMPRA:</b>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblOrdenCompra.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>GUÍA AEREA:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblGuia.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>TIPO DE PRODUCTO:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblTipoProducto.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>PROVEEDOR:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblProveedor.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>PRODUCTO:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblProducto.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>TIPO DE RECEPCIÓN:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblTipoRecepcion.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec' width='120'><b>NÚMERO DE PALETS:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblPalet.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>CANTIDAD APROX.:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblCantidad.Text & "</b></font></TD>")
                .Append(" </TR>")
                If isFrom = "fe" Then
                    .Append(" <TR>")
                    .Append("  <TD class='tdPrinRec'><b>UNIDADES POR CAJA:</b></TD>")
                    .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblUnidadesCaja.Text & "</b></font></TD>")
                    .Append(" </TR>")
                    .Append(" <TR>")
                    .Append("  <TD class='tdPrinRec'><b>CAJAS POR PALET:</b></TD>")
                    .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblCajasPalet.Text & "</b></font></TD>")
                    .Append(" </TR>")
                End If
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>PESO:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblPeso.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>FECHA RECEPCIÓN:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblFechaRecepcion.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>ESTADO RECEPCION:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblEstadoRecepcion.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>BODEGA:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblBodega.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>DESTINO TRASLADO:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & ddlDestinoTraslado.SelectedItem.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>OBSERVACIÓN:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblObservacion.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" </TABLE>")
                .Append("<br><font class='fuente'>Cordial Saludo,<br><br><b>RECEPCION DE PRODUCTO</b><br><br>")
                .Append("</font><font class='fuente' size='1'><i>Nota: Este correo es generado automaticamente, ")
                .Append("si tiene alguna duda, inquietud o comentario, por favor comuníquese con el responsable del proceso.</i></font></font>")
                .Append("</body>")
                .Append("</HTML>")
                Return .ToString
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de armar cuerpo del Mail de notificación de Salida de Factura. " & ex.Message)
        End Try
    End Function

    Private Sub enviarMail()
        Dim eMail As New MailMessage, cuerpoMail, destinatarios As String

        Try
            destinatarios = getDestinatarios()
            If destinatarios <> "" Then
                cuerpoMail = armarCuerpoMail()
                If cuerpoMail <> "" Then
                    SmtpMail.SmtpServer = ConfigurationManager.AppSettings("mailServer")
                    With eMail
                        .From = "Sistema de Recepción de Facturas <" & ConfigurationManager.AppSettings("mailSender") & ">"
                        .Subject = "Notificación de Salida de Factura - " & lblTipoProducto.Text
                        .To = destinatarios
                        .BodyFormat = MailFormat.Html
                        .Body = cuerpoMail
                    End With
                    SmtpMail.Send(eMail)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de enviar Mail de notificación de Salida de Factura. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Function getDestinatarios() As String
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String
        Dim destinatarios As New StringBuilder

        sqlSelect += "select email from UsuarioAvisarRecepcionFactura ua with(nolock) where "
        sqlSelect += " enviarSiempre=1 or (enviarSiempre=0 and idUsuario in (select idUsuario from "
        sqlSelect += " UsuarioAvisarTipoProducto with(nolock) where idTipoProducto=@idTipoProducto "
        sqlSelect += " and idUsuario=ua.idUsuario))"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idTipoProducto", SqlDbType.Int).Value = hIdTipoProducto.Value
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            While sqlRead.Read
                destinatarios.Append(sqlRead.GetValue(0) & ";")
            End While
            sqlRead.Close()
            If destinatarios.ToString <> "" Then destinatarios.Length = destinatarios.Length - 1
            Return destinatarios.ToString
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el listado de destinatarios. ")
        Finally
            If Not sqlComando Is Nothing Then sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Function
End Class
