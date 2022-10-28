#Region "Librerías"

Imports System.Net.Mail
Imports System.Text
Imports System.Configuration
Imports System.Net
Imports ILSBusinessLayer.Comunes

#End Region

Public Class AdministradorCorreo

#Region "Variables"

    Private _emisor As MailAddress
    Private _receptor As New MailAddressCollection
    Private _copia As New MailAddressCollection
    Private _asunto As String
    Private _tituloMensaje As String
    Private _textoMensaje As String
    Private _firmaMensaje As String
    Private _notaMensaje As String
    Private _mensajeLegal As String
    Private _cuerpo As New StringBuilder
    Private _adjuntosURL As New ArrayList
    Private _mensaje As New MailMessage
    Private _adjuntos As Attachment
    Private _clienteSmtp As New SmtpClient
    Private _direccionOrigen As String
    Private _direccionDestino As String
    Private _rutaArchivos As New ArrayList
    Private _attach As Attachment
    Private _displayName As String
    Private _nombreUsuario As String
    Private _password As String
    Private _dominio As String

#End Region

#Region "Properties"

    Public ReadOnly Property Destanatarios() As MailAddressCollection
        Get
            Return _mensaje.To
        End Get
    End Property

    Public ReadOnly Property DestanatariosCopia() As MailAddressCollection
        Get
            Return _mensaje.CC
        End Get
    End Property

    Public ReadOnly Property DestanatariosCopiaOculta() As MailAddressCollection
        Get
            Return _mensaje.Bcc
        End Get
    End Property

    Public Property Emisor() As MailAddress
        Get
            Return _emisor
        End Get
        Set(ByVal value As MailAddress)
            _emisor = value
        End Set
    End Property

    Public Property Receptor() As MailAddressCollection
        Get
            Return _receptor
        End Get
        Set(ByVal value As MailAddressCollection)
            _receptor = value
        End Set
    End Property

    Public Property Copia() As MailAddressCollection
        Get
            Return _copia
        End Get
        Set(ByVal value As MailAddressCollection)
            _copia = value
        End Set
    End Property

    Public Property Asunto() As String
        Get
            Return _asunto
        End Get
        Set(ByVal value As String)
            _asunto = value
        End Set
    End Property

    Public Property DisplayName() As String
        Get
            Return _displayName
        End Get
        Set(ByVal value As String)
            _displayName = value
        End Set
    End Property

    Public Property Titulo() As String
        Get
            Return _tituloMensaje
        End Get
        Set(ByVal value As String)
            _tituloMensaje = value
        End Set
    End Property

    Public Property TextoMensaje() As String
        Get
            Return _textoMensaje
        End Get
        Set(ByVal value As String)
            _textoMensaje = value
        End Set
    End Property

    Public Property FirmaMensaje() As String
        Get
            Return _firmaMensaje
        End Get
        Set(ByVal value As String)
            _firmaMensaje = value
        End Set
    End Property

    Public Property NotaMensaje() As String
        Get
            Return _notaMensaje
        End Get
        Set(ByVal value As String)
            _notaMensaje = value
        End Set
    End Property

    Public Property MensajeLegal As String
        Get
            Return _mensajeLegal
        End Get
        Set(value As String)
            _mensajeLegal = value
        End Set
    End Property

    Public Property Cuerpo() As StringBuilder
        Get
            Return _cuerpo
        End Get
        Set(value As StringBuilder)
            _cuerpo = value
        End Set
    End Property

    Public Property AdjuntosURL() As ArrayList
        Get
            Return _adjuntosURL
        End Get
        Set(ByVal value As ArrayList)
            _adjuntosURL = value
        End Set
    End Property

    Public Property Mensaje() As MailMessage
        Get
            Return _mensaje
        End Get
        Set(ByVal value As MailMessage)
            _mensaje = value
        End Set
    End Property


    Public Property ClienteSmtp() As SmtpClient
        Get
            Return _clienteSmtp
        End Get
        Set(ByVal value As SmtpClient)
            _clienteSmtp = value
        End Set
    End Property

    Public Property RutaAttachment() As ArrayList
        Get
            Return _rutaArchivos
        End Get
        Set(ByVal Value As ArrayList)
            _rutaArchivos = Value
        End Set
    End Property

    Public ReadOnly Property NombreUsuario() As String
        Get
            Return _nombreUsuario
        End Get
    End Property

    Public ReadOnly Property Password() As String
        Get
            Return _password
        End Get
    End Property

    Public ReadOnly Property Dominio() As String
        Get
            Return _dominio
        End Get
    End Property

    Public ReadOnly Property VistaAlternativa() As AlternateViewCollection
        Get
            Return _mensaje.AlternateViews
        End Get
    End Property

    Public Property Prioridad() As MailPriority
        Get
            Return _mensaje.Priority
        End Get
        Set(ByVal value As MailPriority)
            _mensaje.Priority = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()
        _nombreUsuario = ""
        _password = ""
        _dominio = ""
        _mensaje = New MailMessage
        Me.EstablecerValoresPorDefecto()
    End Sub

    Public Sub Dispose()
        If _mensaje IsNot Nothing Then _mensaje.Dispose()
    End Sub
#End Region

#Region "Class Private Methods"

    Protected Overridable Sub CrearCuerpoMensajeHTML()
        Dim value As New ConfigValues("URL_IMAGEN_NOTIFICACION")
        Dim url As String = value.ConfigKeyValue
        With _cuerpo
            .Append("<HTML>")
            .Append("	<HEAD>")
            .Append("		<LINK href='" & url & "include/styleBACK.css' type='text/css' rel='stylesheet'>")
            .Append("	</HEAD>")
            .Append("	<body class='cuerpo2'>")
            .Append("	<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'")
            .Append("		ID='Table1'>")
            .Append("		<tr>")
            .Append("			<td width='20%' ><img src='" & url & "images/logo_trans.png'>")
            .Append("			</td>")
            .Append("			<td align='center' bgcolor='#883485' width='80%'><font size='3' face='arial' color='white'><b>" & _tituloMensaje & "</b></font></td>")
            .Append("		</tr>")
            .Append("	</table>")
            .Append("	<br />")
            .Append("	<br />")

            If Now.Hour < 12 Then
                .Append("	<font class='fuente'>Buenos Días, ")
            ElseIf Now.Hour > 18 Then
                .Append("	<font class='fuente'>Buenas Noches, ")
            Else
                .Append("	<font class='fuente'>Buenas Tardes, ")
            End If

            .Append("<br /><br />" & _textoMensaje)
            .Append("	<br />")
            .Append("</font>")
            .Append("<br />	<font class='fuente'>Cordial Saludo,<br />")
            .Append("		<br><b>" & _firmaMensaje & "</b><br /><br />")
            .Append("</font><br /><br /><br /><br /><br />")
            If _notaMensaje <> "" Then
                .Append("	<font class='fuente' size='1'><i>Nota: " & _notaMensaje & ".</i></font")
            End If
            .Append("	</body>")
            .Append("</HTML>")
        End With

        _mensaje.Body = _cuerpo.ToString

    End Sub

    Private Sub AdjuntarArchivos()
        For Each ruta As String In RutaAttachment
            _attach = New Attachment(ruta)
            Mensaje.Attachments.Add(_attach)
        Next
    End Sub

    Private Function ObtenerValordeConfiguracion(ByVal nombreConfiguracion As String) As String
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim sql As String = "SELECT valor FROM  ConfigValues WHERE nombreConfiguracion = @nombreConfiguracion"
        db.agregarParametroSQL("@nombreConfiguracion", nombreConfiguracion)
        Return db.ejecutarScalar(sql)
    End Function

#End Region

#Region "Class Public Methods"

    Public Overloads Sub EstablecerCuentaOrigen(ByVal direccion As String)
        _mensaje.From = New MailAddress(direccion)
    End Sub

    Public Overloads Sub EstablecerCuentaOrigen(ByVal direccion As String, ByVal nombreAMostrar As String)
        _mensaje.From = New MailAddress(direccion, nombreAMostrar)
    End Sub

    Public Overloads Sub AdicionarDestinatario(ByVal direccion As String)
        _mensaje.To.Add(direccion)
    End Sub

    Public Overloads Sub AdicionarDestinatario(ByVal direccion As String, ByVal nombreAMostrar As String)
        _mensaje.To.Add(New MailAddress(direccion, nombreAMostrar))
    End Sub

    Public Overloads Sub AdicionarDestinatarioCopia(ByVal direccion As String)
        _mensaje.CC.Add(direccion)
    End Sub

    Public Overloads Sub AdicionarDestinatarioCopia(ByVal direccion As String, ByVal nombreAMostrar As String)
        _mensaje.CC.Add(New MailAddress(direccion, nombreAMostrar))
    End Sub

    Public Overloads Sub AdicionarDestinatarioCopiaOculta(ByVal direccion As String)
        _mensaje.Bcc.Add(direccion)
    End Sub

    Public Overloads Sub AdicionarDestinatarioCopiaOculta(ByVal direccion As String, ByVal nombreAMostrar As String)
        _mensaje.Bcc.Add(New MailAddress(direccion, nombreAMostrar))
    End Sub

    Public Overloads Sub EstablecerCuentaRespuesta(ByVal direccion As String, ByVal nombreAMostrar As String)
        _mensaje.ReplyTo = New MailAddress(direccion, nombreAMostrar)
    End Sub

    Public Sub EstablecerCredenciales(ByVal nombreUsuario As String, ByVal password As String, ByVal dominio As String)
        _clienteSmtp.Credentials = New NetworkCredential(nombreUsuario, password, dominio)
        _nombreUsuario = nombreUsuario
        _password = password
        _dominio = dominio
    End Sub

    Public Sub EstablecerValoresPorDefecto()
        'Dim displayName As String
        If String.IsNullOrEmpty(_displayName) Then _displayName = "Notificador NOTUS"

        ' Set encoding values
        _mensaje.SubjectEncoding = Encoding.UTF8
        _mensaje.BodyEncoding = Encoding.UTF8

        ' Set other values
        _mensaje.Priority = MailPriority.Normal
        _mensaje.IsBodyHtml = True
        _clienteSmtp.Host = ConfigurationManager.AppSettings("mailServer")
        _mensaje.From = New MailAddress(ConfigurationManager.AppSettings("mailSender"), _displayName)


        If ConfigurationManager.AppSettings("credenciales") IsNot Nothing Then
            Dim credenciales() As String
            credenciales = ConfigurationManager.AppSettings("credenciales").Split(";")
            _nombreUsuario = credenciales(0)
            _password = credenciales(1)
            _dominio = credenciales(2)
        End If

        _emisor = _mensaje.From
        _receptor = _mensaje.To
        _firmaMensaje = "-- <br> ||| Dinatech Mobile S.A.S."
        _notaMensaje = "Este mensaje fue generado automáticamente, si tiene alguna duda o inquietud respecto al mismo, por favor envía un correo electrónico al grupo <a href='mailto:itdevelopment@logytechmobile.com?subject=Inquietud Notificador'> IT Development </a>"
    End Sub

    Public Function EnviarMail() As Boolean
        Dim answer As Boolean
        Try
            For Each direccion As MailAddress In _receptor
                If Not _mensaje.To.Contains(direccion) Then _mensaje.To.Add(direccion)
            Next

            For Each direccion As MailAddress In _copia
                If Not _mensaje.CC.Contains(direccion) Then _mensaje.CC.Add(direccion)
            Next

            _mensaje.Subject = _asunto
            If _emisor Is Nothing Then
                Me.EstablecerValoresPorDefecto()
            Else
                _mensaje.From = _emisor
            End If

            If _mensaje.IsBodyHtml = True Then
                CrearCuerpoMensajeHTML()
            Else
                _mensaje.Body = _textoMensaje
            End If
            ' Adjunta Archivos
            AdjuntarArchivos()

            If _nombreUsuario IsNot Nothing AndAlso _nombreUsuario.Length > 0 AndAlso _password IsNot Nothing AndAlso _
                _password.Length > 0 AndAlso _dominio IsNot Nothing AndAlso _dominio.Length > 0 Then
                EstablecerCredenciales(_nombreUsuario, _password, _dominio)
            End If

            With ClienteSmtp
                .DeliveryMethod = SmtpDeliveryMethod.Network
                .Timeout = 99999
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
                .EnableSsl = True
                .Send(_mensaje)
            End With

            answer = True
        Catch smtpEx As SmtpException
            Dim statusCode As SmtpStatusCode = smtpEx.StatusCode
            Select Case statusCode
                Case SmtpStatusCode.ExceededStorageAllocation
                    Throw New Exception("El tamaño del mensaje es mayor al tamaño permitido por el buzón de correo del receptor")

                Case SmtpStatusCode.GeneralFailure
                    Throw New Exception("No se pudo establecer Conexion con el servidor SMTP de Logytech Mobile")

                Case SmtpStatusCode.InsufficientStorage
                    Throw New Exception("El servidor SMTP de Logytech Mobile no tiene espacio suficiente para guardar el mensaje")

                Case SmtpStatusCode.MailboxBusy, SmtpStatusCode.MailboxUnavailable
                    Throw New Exception("El buzón de correo del receptor está ocupado o no disponible, reintentando en 5 segundos")
                    For i As Integer = 0 To 2
                        System.Threading.Thread.Sleep(5000)
                        ClienteSmtp.Send(_mensaje)
                    Next

                Case SmtpStatusCode.TransactionFailed
                    Throw New Exception("Transacción fallida, reintentando en 5 segundos")
                    For i As Integer = 0 To 2
                        System.Threading.Thread.Sleep(5000)
                        ClienteSmtp.Send(_mensaje)
                    Next

                Case SmtpStatusCode.ServiceNotAvailable
                    Throw New Exception("Servicio no disponible temporalmente, reintentando en 5 segundos")
                    For i As Integer = 0 To 2
                        System.Threading.Thread.Sleep(5000)
                        ClienteSmtp.Send(_mensaje)
                    Next
            End Select

        Catch ex As Exception
            Throw New Exception("Correo electrónico no enviado: " & ex.Message)
        End Try

        Return answer
    End Function

    Public Function ValidarDominio(ByVal email As String) As Boolean
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dominios As String = ObtenerValordeConfiguracion("DOMINIOS_VALIDOS_NOTIFICACION")
        Dim arrDominiosValidos As String() = dominios.Split(",")
        Dim dominioActual As String() = email.Split("@")
        Dim flag As Boolean
        If dominioActual.Length > 1 Then
            flag = arrDominiosValidos.Contains(dominioActual(1))
        Else
            flag = False
        End If
        Return flag
    End Function

#End Region


End Class
