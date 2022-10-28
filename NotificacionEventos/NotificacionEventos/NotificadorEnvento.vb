Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mime
Imports LMMailSenderLibrary
Imports LMDataAccessLayer

Public Class NotificadorEvento

#Region "Atributos"

    Private _rutaArchivo As String
    Private fullApplicationPath As String
    Private _nombreUsuario As String
    Private _password As String
    Private _dominio As String
    Private _nombreServidor As String
    Private _cuentaOrigen As String
    Private _direccionDestino As ArrayList
    Private _direccionDestinoCopia As ArrayList

#End Region

#Region "Contructores"

    Public Sub New()
        InicializarRutaDeArchivo()
        InicializarCadenasDeTexto()
    End Sub

    Public Sub New(ByVal rutaArchivo As String)
        Me._rutaArchivo = rutaArchivo
        'ComprobarCambioDeArchivo()
        InicializarCadenasDeTexto()
    End Sub

#End Region

#Region "Propiedades"

    Public Property Usuario() As String
        Get
            Return _nombreUsuario
        End Get
        Set(ByVal value As String)
            _nombreUsuario = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public Property Dominio() As String
        Get
            Return _dominio
        End Get
        Set(ByVal value As String)
            _dominio = value
        End Set
    End Property

    Public Property NombreServidor() As String
        Get
            Return _nombreServidor
        End Get
        Set(ByVal value As String)
            _nombreServidor = value
        End Set
    End Property

    Public Property CuentaOrigen() As String
        Get
            Return _cuentaOrigen
        End Get
        Set(ByVal value As String)
            _cuentaOrigen = value
        End Set
    End Property

    Public ReadOnly Property DireccionDestino() As ArrayList
        Get
            If _direccionDestino Is Nothing Then _direccionDestino = New ArrayList
            Return _direccionDestino
        End Get
    End Property

    Public ReadOnly Property DireccionDestinoCopia() As ArrayList
        Get
            If _direccionDestinoCopia Is Nothing Then _direccionDestinoCopia = New ArrayList
            Return _direccionDestinoCopia
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub InicializarRutaDeArchivo()
        Dim miCI As New System.Globalization.CultureInfo("es-CO")
        Dim miCalendario As System.Globalization.Calendar = miCI.Calendar
        Dim semana As String = miCalendario.GetWeekOfYear(Now, Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString

        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current

        If context IsNot Nothing Then
            fullApplicationPath = context.Server.MapPath("~/Logs")
        ElseIf String.IsNullOrEmpty(fullApplicationPath) Then
            fullApplicationPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath & "\Logs"
            If String.IsNullOrEmpty(fullApplicationPath) Then
                fullApplicationPath = Environment.CurrentDirectory & "\Logs"
            End If
        End If


        If (Not System.IO.Directory.Exists(fullApplicationPath)) Then System.IO.Directory.CreateDirectory(fullApplicationPath)

        _rutaArchivo = fullApplicationPath & "\LogEventos_" + Now.Year.ToString + "_" + semana + ".log"
        '_rutaArchivo = fullApplicationPath & "\LogEventos.log"
        'ComprobarCambioDeArchivo()
    End Sub

    Private Sub ComprobarCambioDeArchivo()
        If File.Exists(_rutaArchivo) Then
            Dim fechaCreacion As Date = File.GetCreationTime(_rutaArchivo)
            If DateDiff(DateInterval.Day, fechaCreacion, Now.Date) >= 7 Then
                Dim nombreDirectorio As String = Path.GetDirectoryName(_rutaArchivo)
                Dim nombreArchivo As String = Path.GetFileNameWithoutExtension(_rutaArchivo) & Now.ToString("ddMMyy")
                Dim extension As String = Path.GetExtension(_rutaArchivo)
                Dim nombreFinal As String = nombreArchivo & extension
                If Not File.Exists(nombreFinal) Then
                    My.Computer.FileSystem.RenameFile(_rutaArchivo, nombreFinal)
                End If
            End If
        End If
    End Sub

    Private Function ObtenerNombreDeIcono(ByVal tEvento As TipoEvento) As System.Drawing.Bitmap
        Select Case tEvento
            Case TipoEvento.Exito
                Return New System.Drawing.Bitmap(My.Resources.Recursos.success_icon)
            Case TipoEvento.Error
                Return New System.Drawing.Bitmap(My.Resources.Recursos.error_icon)
            Case TipoEvento.Alerta
                Return New System.Drawing.Bitmap(My.Resources.Recursos.warning_icon)
            Case Else
                Return New System.Drawing.Bitmap(My.Resources.Recursos.Info)
        End Select
    End Function

    Private Function ObtenerStreamDeImagen(ByVal imagen As System.Drawing.Bitmap) As MemoryStream
        Dim ic As New System.Drawing.ImageConverter
        Dim b As Byte()
        b = ic.ConvertTo(imagen, GetType(Byte()))
        Dim m As New MemoryStream(b)
        Return m
    End Function

#End Region

#Region "Métodos Públicos"


    Public Sub InicializarCadenasDeTexto()
        If ConfigurationManager.AppSettings("notificadorEvento") IsNot Nothing Then _
            _cuentaOrigen = ConfigurationManager.AppSettings("notificadorEvento").ToString

        If ConfigurationManager.AppSettings("servidorCorreo") IsNot Nothing Then _
            _nombreServidor = ConfigurationManager.AppSettings("servidorCorreo").ToString

        Dim dtDestinos As New DataTable
        Dim strDestinoPP As String()
        Dim strDestinoCC As String()
        Try
            If _direccionDestino Is Nothing And _direccionDestinoCopia Is Nothing Then
                dtDestinos = ObtenerDestinatarioNotificacion(103)
                For Each fila As DataRow In dtDestinos.Rows
                    strDestinoPP = fila.Item("destinoPara").ToString.Split(",")
                    strDestinoCC = fila.Item("destinoCopia").ToString.Split(",")
                Next
                If strDestinoPP.Length > 0 Then
                    If _direccionDestino Is Nothing Then _direccionDestino = New ArrayList
                    For index As Integer = 0 To strDestinoPP.Length - 1
                        If Not String.IsNullOrEmpty(strDestinoPP(index)) Then _direccionDestino.Add(strDestinoPP(index))
                    Next
                End If
                If strDestinoCC.Length > 0 Then
                    If _direccionDestinoCopia Is Nothing Then _direccionDestinoCopia = New ArrayList
                    For index As Integer = 0 To strDestinoCC.Length - 1
                        If Not String.IsNullOrEmpty(strDestinoCC(index)) Then _direccionDestinoCopia.Add(strDestinoCC(index))
                    Next
                End If
            End If
          

        Catch ex As Exception
        Finally
            If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
        End Try


        If _direccionDestino Is Nothing And _direccionDestinoCopia Is Nothing Then
            If ConfigurationManager.AppSettings("destinoNotificacion") IsNot Nothing Then
                Dim direccion() As String = ConfigurationManager.AppSettings("destinoNotificacion").ToString.Split(";")
                If direccion.Length > 0 Then
                    If _direccionDestino Is Nothing Then _direccionDestino = New ArrayList
                    For index As Integer = 0 To direccion.Length - 1
                        If Not String.IsNullOrEmpty(direccion(index)) Then _direccionDestino.Add(direccion(index))
                    Next
                End If
            Else
                _direccionDestino = New ArrayList
            End If
        End If


        If ConfigurationManager.AppSettings("credencialesNotificador") IsNot Nothing Then
            Dim credenciales() As String = ConfigurationManager.AppSettings("credencialesNotificador").Split(";")
            If credenciales.Length >= 3 Then
                _nombreUsuario = credenciales(0).Trim
                _password = credenciales(1).Trim
                _dominio = credenciales(2).Trim
            End If
        End If

        If String.IsNullOrEmpty(_nombreUsuario) Then _nombreUsuario = "system.notifier"
        If String.IsNullOrEmpty(_password) Then _password = "12345.LM"
        If String.IsNullOrEmpty(_dominio) Then _dominio = "LM"
        If String.IsNullOrEmpty(_nombreServidor) Then _nombreServidor = "colbogsa062"
        If String.IsNullOrEmpty(_cuentaOrigen) Then _cuentaOrigen = "system.notifier@logytechmobile.com"
    End Sub
    Public Overloads Shared Function ObtenerDestinatarioNotificacion(ByVal IdAsuntoNotificacion As Integer) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtDatos As DataTable
        Try
            With dbManager
                With .SqlParametros
                    If IdAsuntoNotificacion <> 0 Then .Add("@idAsuntoNotificacion", SqlDbType.Int).Value = IdAsuntoNotificacion

                End With
                dtDatos = .EjecutarDataTable("ObtenerDestinatarioNotificacion", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function
    Public Sub AdicionarRegistro(ByVal mensaje As String, ByVal elEvento As TipoEvento)
        Try
            If _rutaArchivo.Trim.Length > 0 Then
              
                Dim registro As String = Now.ToString("dd/MM/yyyy hh:mm:ss tt") & vbTab & mensaje & vbTab & " Tipo Evento: " & elEvento.ToString
                File.AppendAllText(_rutaArchivo, registro)

            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub NotificarViaMail(ByVal proceso As String, ByVal mensaje As String, ByVal tipo As TipoEvento,
        Optional ByVal guardarRegistro As Boolean = True)
        If (_direccionDestino IsNot Nothing AndAlso _direccionDestino.Count > 0) Then

            Dim mailSender As New MailMessage
            Dim mailHandler As New LMMailSender
            Try
                Dim mailBody As New StringBuilder
                With mailBody
                    .Append("<html>")
                    .Append("<head><title>Notificador de Eventos Desatendidos</title>")
                    .Append("<style type='text/css'>")
                    .Append("body {font: 70%/1.5em Verdana, Tahoma, arial, sans-serif; margin: 5px 10px; padding: 5px 5px; background-color: #ffffff;}")
                    .Append(".tabla {font-family: Helvetica, Arial, sans-serif; font-size:9pt; border-color:Gray}")
                    .Append("</style></head>")
                    .Append("<body>")
                    .Append("<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'>")
                    .Append("<tr><td width='20%'><img src='cid:logoEmpresa' alt=''/></td>")
                    .Append("<td align='center' bgcolor='#38610B' width='80%'><font size='3' face='arial' color='white'>" &
                            "<b>NOTIFICACIÓN DE EVENTOS: " & proceso & "</b></font></td></tr>")
                    .Append("</table><br><br>")
                    .Append("<table><tr>")
                    .Append("<td valign='top'><img src='cid:icono' alt=''/></td><td><font size='3'><b>&nbsp;" & mensaje & "</b></td>")
                    .Append("</tr></table>")
                    .Append("<font name='Verdana' size='2'>")
                    .Append("<br><br><font name='Verdana' size='2'><b>Atentamente,<br>IT Development - Logytech Mobile S.A.S</b></font><br><br>")
                    .Append("<font class='Verdana' size='1'><i>Nota: Este correo es generado automaticamente, ")
                    .Append("si tiene alguna duda, inquietud o comentario por favor envíe un e-mail a su área de contacto")
                    .Append("</body>")
                    .Append("</html>")
                End With
                'AlmacenarImagenes()
                Dim htmlBody As AlternateView = AlternateView.CreateAlternateViewFromString(mailBody.ToString, Nothing, MediaTypeNames.Text.Html)
                Dim msLogo As System.IO.MemoryStream = ObtenerStreamDeImagen(My.Resources.Recursos.logoEmpresa)
                'Dim b As New System.Drawing.Bitmap(My.Resources.Recursos.logoEmpresa)
                'b.Save(msLogo, System.Drawing.Imaging.ImageFormat.Gif)
                Dim lsLogo As New LinkedResource(msLogo, "image/gif")

                Dim icono As System.Drawing.Bitmap = ObtenerNombreDeIcono(tipo)
                Dim msIcono As MemoryStream = ObtenerStreamDeImagen(icono)
                'icono.Save(msIcono, System.Drawing.Imaging.ImageFormat.Gif)
                Dim lsIcono As New LinkedResource(msIcono, "image/gif")

                lsLogo.ContentId = "logoEmpresa"
                lsLogo.TransferEncoding = TransferEncoding.Base64

                lsIcono.ContentId = "icono"
                lsIcono.TransferEncoding = TransferEncoding.Base64

                htmlBody.LinkedResources.Add(lsLogo)
                htmlBody.LinkedResources.Add(lsIcono)

                With mailHandler
                    .ServidorCorreo = _nombreServidor
                    .EstablecerCredenciales(_nombreUsuario, _password, _dominio)
                    .EstablecerCuentaOrigen(_cuentaOrigen)

                    For index As Integer = 0 To _direccionDestino.Count - 1
                        .Destanatarios.Add(_direccionDestino(index))
                    Next

                    If _direccionDestinoCopia IsNot Nothing AndAlso _direccionDestinoCopia.Count > 0 Then
                        For index As Integer = 0 To _direccionDestinoCopia.Count - 1
                            .DestanatariosCopia.Add(_direccionDestinoCopia(index))
                        Next
                    End If

                    .Prioridad = Net.Mail.MailPriority.High
                    .CuerpoEsHtml = True
                    .Asunto = "Notificación de Evento en Proceso: " & proceso
                    .VistaAlternativa.Add(htmlBody)
                    .Enviar()
                End With
            Catch ex As Exception
                AdicionarRegistro("Ocurrió un error al tratar de notificar error vía Mail", TipoEvento.Error)
            End Try
        End If
        If guardarRegistro Then AdicionarRegistro(mensaje, tipo)
    End Sub
    Public Sub NotificarViaMailDatatable(ByVal proceso As String, ByVal mensaje As String, ByVal tipo As TipoEvento, ByVal datos As DataTable,
        Optional ByVal guardarRegistro As Boolean = True)
        If (_direccionDestino IsNot Nothing AndAlso _direccionDestino.Count > 0) Then

            Dim mailSender As New MailMessage
            Dim mailHandler As New LMMailSender

            Try
                Dim mailBody As New StringBuilder
                With mailBody
                    .Append("<html>")
                    .Append("<head><title>Notificador de Eventos Desatendidos</title>")
                    .Append("<style type='text/css'>")
                    .Append("body {font: 70%/1.5em Verdana, Tahoma, arial, sans-serif; margin: 5px 10px; padding: 5px 5px; background-color: #ffffff;}")
                    .Append(".tabla {font-family: Helvetica, Arial, sans-serif; font-size:9pt; border-color:Gray}")
                    .Append("</style></head>")
                    .Append("<body>")
                    .Append("<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'>")
                    .Append("<tr><td width='20%'><img src='cid:logoEmpresa' alt=''/></td>")
                    .Append("<td align='center' bgcolor='#38610B' width='80%'><font size='3' face='arial' color='white'>" &
                            "<b>NOTIFICACIÓN DE EVENTOS: " & proceso & "</b></font></td></tr>")
                    .Append("</table><br><br>")
                    .Append("<table><tr>")
                    .Append("<td valign='top'><img src='cid:icono' alt=''/></td><td><font size='3'><b>&nbsp;" & mensaje & "</b></td>")
                    .Append("</tr></table>")
                    .Append("<table border='2'><tr>")
                    For c As Integer = 0 To datos.Columns.Count - 1
                        .Append("<th>" + datos.Columns(c).Caption + "</th>")
                    Next
                    .Append("</tr>")
                    For Each i As DataRow In datos.Rows
                        .Append("<tr>")
                        For j As Integer = 0 To datos.Columns.Count - 1
                            .Append("<td>")
                            .Append(i.Item(j).ToString)
                            .Append("</td>")
                        Next
                        .Append("</tr>")
                    Next
                    .Append("</table>")
                    .Append("<font name='Verdana' size='2'>")
                    .Append("<br><br><font name='Verdana' size='2'><b>Atentamente,<br>IT Development - Logytech Mobile S.A.S</b></font><br><br>")
                    .Append("<font class='Verdana' size='1'><i>Nota: Este correo es generado automaticamente, ")
                    .Append("si tiene alguna duda, inquietud o comentario por favor envíe un e-mail a su área de contacto")
                    .Append("</body>")
                    .Append("</html>")
                End With
                'AlmacenarImagenes()
                Dim htmlBody As AlternateView = AlternateView.CreateAlternateViewFromString(mailBody.ToString, Nothing, MediaTypeNames.Text.Html)
                Dim msLogo As System.IO.MemoryStream = ObtenerStreamDeImagen(My.Resources.Recursos.logoEmpresa)
                'Dim b As New System.Drawing.Bitmap(My.Resources.Recursos.logoEmpresa)
                'b.Save(msLogo, System.Drawing.Imaging.ImageFormat.Gif)
                Dim lsLogo As New LinkedResource(msLogo, "image/gif")

                Dim icono As System.Drawing.Bitmap = ObtenerNombreDeIcono(tipo)
                Dim msIcono As MemoryStream = ObtenerStreamDeImagen(icono)
                'icono.Save(msIcono, System.Drawing.Imaging.ImageFormat.Gif)
                Dim lsIcono As New LinkedResource(msIcono, "image/gif")

                lsLogo.ContentId = "logoEmpresa"
                lsLogo.TransferEncoding = TransferEncoding.Base64

                lsIcono.ContentId = "icono"
                lsIcono.TransferEncoding = TransferEncoding.Base64

                htmlBody.LinkedResources.Add(lsLogo)
                htmlBody.LinkedResources.Add(lsIcono)

                With mailHandler
                    .ServidorCorreo = _nombreServidor
                    .EstablecerCredenciales(_nombreUsuario, _password, _dominio)
                    .EstablecerCuentaOrigen(_cuentaOrigen)

                    For index As Integer = 0 To _direccionDestino.Count - 1
                        .Destanatarios.Add(_direccionDestino(index))
                    Next

                    If _direccionDestinoCopia IsNot Nothing AndAlso _direccionDestinoCopia.Count > 0 Then
                        For index As Integer = 0 To _direccionDestinoCopia.Count - 1
                            .DestanatariosCopia.Add(_direccionDestinoCopia(index))
                        Next
                    End If

                    .Prioridad = Net.Mail.MailPriority.High
                    .CuerpoEsHtml = True
                    .Asunto = "Notificación de Evento en Proceso: " & proceso
                    .VistaAlternativa.Add(htmlBody)
                    .Enviar()
                End With
            Catch ex As Exception
                AdicionarRegistro("Ocurrió un error al tratar de notificar error vía Mail", TipoEvento.Error)
            End Try
        End If
        If guardarRegistro Then AdicionarRegistro(mensaje, tipo)
    End Sub

    Public Sub AlmacenarImagenes()

        Try
            If Not File.Exists(fullApplicationPath & "\imgError.gif") Then _
                My.Resources.Recursos.error_icon.Save(fullApplicationPath & "\imgError.gif")

            If Not File.Exists(fullApplicationPath & "\imgSuccess.gif") Then _
                My.Resources.Recursos.success_icon.Save(fullApplicationPath & "\imgSuccess.gif")

            If Not File.Exists(fullApplicationPath & "\imgWarning.gif") Then _
                My.Resources.Recursos.warning_icon.Save(fullApplicationPath & "\imgWarning.gif")

            If Not File.Exists(fullApplicationPath & "\imgInformation.gif") Then _
                My.Resources.Recursos.Info.Save(fullApplicationPath & "\imgInformation.gif")

            If Not File.Exists(fullApplicationPath & "\logoEmpresa.gif") Then _
                My.Resources.Recursos.logoEmpresa.Save(fullApplicationPath & "\logoEmpresa.gif")

        Catch ex As Exception
            AdicionarRegistro("Error al tratar de almacenar imágenes", TipoEvento.Error)
        End Try
    End Sub
    'Private Sub CargarDestinatarios(ByVal tipo As Comunes.AsuntoNotificacion.Tipo, ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection)
    '    Dim ConfiguracionUsuario As New UsuarioNotificacion
    '    Dim filtro As New FiltroUsuarioNotificacion
    '    Dim dtDestinos As New DataTable
    '    Dim strDestinoPP As String = String.Empty
    '    Dim strDestinoCC As String = String.Empty

    '    filtro.IdAsuntoNotificacion = tipo
    '    filtro.Separador = ","
    '    Try
    '        dtDestinos = UsuarioNotificacion.ObtenerDestinatarioNotificacion(filtro)
    '        For Each fila As DataRow In dtDestinos.Rows
    '            strDestinoPP += fila.Item("destinoPara")
    '            strDestinoCC += fila.Item("destinoCopia")
    '        Next
    '        ' strDestinoCC = "faiber.losada@logytechmobile.com,carlos.mazorra@logytechmobile.com"
    '        ' strDestinoPP = "faiber.losada@logytechmobile.com,carlos.mazorra@logytechmobile.com"
    '        destinoPP.Add(strDestinoPP)
    '        destinoCC.Add(strDestinoCC)

    '    Catch ex As Exception
    '    Finally
    '        If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
    '    End Try
    'End Sub

#End Region

#Region "Enumeraciones"

    Public Enum TipoEvento
        [Información] = 0
        Alerta = 1
        [Error] = 2
        Exito = 3
    End Enum

#End Region

End Class


