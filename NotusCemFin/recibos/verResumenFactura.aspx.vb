Imports System.Data.SqlClient
Imports System.Text
Imports System.Web.Mail

Partial Class verResumenFactura
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

    Dim idFactura As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me)
            lblError.Text = ""
            idFactura = Request.QueryString("idFactura")
            If Not Me.IsPostBack Then
                getDatos()
                enviarMail()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar la página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String

        sqlSelect = "select factura,guia,(select proveedor from proveedores with(nolock) where idproveedor=fac.idProveedor)"
        sqlSelect += " as proveedor,(select producto from productos with(nolock) where idproducto=fac.idproducto) as producto,"
        sqlSelect += " (select tipoRecepcion from Tiporecepcion with(nolock) where idTipoRecepcion=fac.idTipoRecepcion) as"
        sqlSelect += " tipoRecepcion,cantidadEsperada,numeroPalets,cantidadAprox,peso,(select bodega from bodegas with(nolock)"
        sqlSelect += " where idbodega=fac.idBodega) as bodega,observacion,ordenCompra,idTipoProducto,(select tipoProducto "
        sqlSelect += " from TipoProducto with(nolock) where idTipoProducto=fac.idTipoProducto) as tipoProducto,(select "
        sqlSelect += " estadoRecepcion from EstadoRecepcionFactura with(nolock) where idEstadoRecepcion=fac.idEstadoRecepcion)"
        sqlSelect += " as estadoRecepcion from Factura fac with(nolock) where idFactura=@idFactura "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idFactura", idFactura)
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            With sqlRead
                If .Read Then
                    lblFactura.Text = .Item("factura").ToString
                    lblGuia.Text = .Item("guia").ToString
                    lblProveedor.Text = .Item("proveedor").ToString
                    lblProducto.Text = .Item("producto").ToString
                    lblTipoRecepcion.Text = .Item("tipoRecepcion").ToString
                    lblCantidadEsperada.Text = .Item("cantidadEsperada").ToString
                    lblPalets.Text = .Item("numeroPalets").ToString
                    lblCantidadAprox.Text = .Item("cantidadAprox").ToString
                    lblPeso.Text = .Item("peso").ToString & " Kg"
                    lblBodega.Text = .Item("bodega").ToString
                    lblObservacion.Text = .Item("observacion").ToString
                    lblOrdenCompra.Text = .Item("ordenCompra").ToString
                    hIdTipoProducto.Value = .Item("idTipoProducto").ToString
                    hTipoProducto.Value = .Item("tipoProducto").ToString
                    lblEstadoRecepcion.Text = .Item("estadoRecepcion").ToString
                    lblRes.Text = "Los Datos han sido registrados satisfactoriamente.<br><br>"
                End If
                .Close()
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los Datos. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Function armarCuerpoMail() As String
        Dim cuerpoMail As New StringBuilder

        Try
            Dim swTableInfo As New System.IO.StringWriter, htwTableInfo As New System.Web.UI.HtmlTextWriter(swTableInfo)
            tblInformacion.RenderControl(htwTableInfo)

            Dim nombreSitio, logo As String
            With System.Configuration.ConfigurationManager.AppSettings
                nombreSitio = Request.ServerVariables("SERVER_NAME")
                logo = .Item("logo")
            End With
            With cuerpoMail
                .Append("<HTML><HEAD>")
                .Append(" <LINK href='" & nombreSitio & "/include/styleBACK.css' type='text/css' rel='stylesheet'>")
                .Append("</HEAD>")
                .Append("<body class='cuerpo2'>")
                .Append("<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'>")
                .Append(" <tr class='encabezadoMail'>")
                .Append("  <td bgcolor='#e0e0e0'><img src='" & nombreSitio & logo & " '></td>")
                .Append("  <td width='95%'><b>NOTIFICACION RECEPCION DE FACTURA</b></td>")
                .Append("</tr></table><br>")
                If Now.Hour < 12 Then
                    .Append("	<font class='fuente'>Buenos Días")
                Else
                    .Append("	<font class='fuente'>Buenas Tardes")
                End If
                .Append("<br><br>Se acaba de registrar en el sistema, la recepción de una Factura con la siguiente información:")
                .Append("</font><br><br>")
                .Append("<table class='tabla'>")
                .Append(" <TR>")
                .Append("  <TD>")
                .Append(swTableInfo.ToString)
                .Append("  </TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdTituloRec'></TD>")
                .Append(" </TR>")
                .Append("</table>")
                .Append("<br><font class='fuente'>Cordial Saludo,<br>")
                .Append("<br><b>RECEPCION DE PRODUCTO</b><br><br>")
                .Append("</font><font class='fuente' size='1'><i>Nota: Este correo es generado automaticamente, ")
                .Append(" si tiene alguna duda, inquietud o comentario,por favor comuníquese con el responsable del proceso.</i></font></font> ")
                .Append("</body>")
                .Append("</HTML>")
                Return .ToString
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de armar cuerpo del Mail de notificación de Recepción de Factura. " & ex.Message)
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
                        .Subject = "Notificación de Recepción de Factura - " & hTipoProducto.Value
                        .To = destinatarios
                        .BodyFormat = MailFormat.Html
                        .Body = cuerpoMail
                    End With
                    SmtpMail.Send(eMail)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de enviar Mail de notificación de Recepción de Factura. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Function getDestinatarios() As String
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String
        Dim destinatarios As New StringBuilder

        sqlSelect += "select email from UsuarioAvisarRecepcionFactura ua with(nolock) where "
        sqlSelect += " enviarSiempre=1 or (enviarSiempre=0 and idUsuario in (select idUsuario "
        sqlSelect += " from UsuarioAvisarTipoProducto with(nolock) where idTipoProducto=@idTipoProducto "
        sqlSelect += " and idUsuario=ua.idUsuario)) "

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
