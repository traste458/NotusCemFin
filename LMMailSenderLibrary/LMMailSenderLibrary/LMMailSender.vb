Imports System.Net
Imports System.Net.Mail

Public Class LMMailSender

#Region "Atributos"

    Private _manejadorMensaje As MailMessage
    Private _clienteSmtp As SmtpClient
    Private _nombreUsuario As String
    Private _password As String
    Private _dominio As String
    Private _adjuntos As Attachment
    Private _adjuntosUrl As ArrayList

#End Region

#Region "Constructores y Destructores"

    Public Sub New()
        MyBase.New()
        _clienteSmtp = New SmtpClient
        _manejadorMensaje = New MailMessage
        _nombreUsuario = ""
        _password = ""
        _dominio = ""
    End Sub

    Public Sub Dispose()
        If _manejadorMensaje IsNot Nothing Then _manejadorMensaje.Dispose()
    End Sub

#End Region

#Region "Propiedades"

    'Public ReadOnly Property ManejadorMensaje() As MailMessage
    '    Get
    '        Return _manejadorMensaje
    '    End Get
    'End Property

    'Public ReadOnly Property ClienteSmtp() As SmtpClient
    '    Get
    '        Return _clienteSmtp
    '    End Get
    'End Property

    Public ReadOnly Property Destanatarios() As MailAddressCollection
        Get
            Return _manejadorMensaje.To
        End Get
    End Property

    Public ReadOnly Property DestanatariosCopia() As MailAddressCollection
        Get
            Return _manejadorMensaje.CC
        End Get
    End Property

    Public ReadOnly Property DestanatariosCopiaOculta() As MailAddressCollection
        Get
            Return _manejadorMensaje.Bcc
        End Get
    End Property

    Public Property Cuerpo() As String
        Get
            Return _manejadorMensaje.Body
        End Get
        Set(ByVal value As String)
            _manejadorMensaje.Body = value
        End Set
    End Property

    Public Property Asunto() As String
        Get
            Return _manejadorMensaje.Subject
        End Get
        Set(ByVal value As String)
            _manejadorMensaje.Subject = value
        End Set
    End Property

    Public Property CuerpoEsHtml() As Boolean
        Get
            Return _manejadorMensaje.IsBodyHtml
        End Get
        Set(ByVal value As Boolean)
            _manejadorMensaje.IsBodyHtml = value
        End Set
    End Property

    Public Property Prioridad() As MailPriority
        Get
            Return _manejadorMensaje.Priority
        End Get
        Set(ByVal value As MailPriority)
            _manejadorMensaje.Priority = value
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

    Public Property ServidorCorreo() As String
        Get
            Return _clienteSmtp.Host
        End Get
        Set(ByVal value As String)
            _clienteSmtp.Host = value
        End Set
    End Property

    Public ReadOnly Property HayDestinatario() As Boolean
        Get
            Dim numDestinatario As Integer = _manejadorMensaje.To.Count
            Dim numDestinatarioCopia As Integer = _manejadorMensaje.CC.Count
            Dim numDestinatarioCopiaOculta As Integer = _manejadorMensaje.Bcc.Count
            Return CBool(numDestinatario + numDestinatarioCopia + numDestinatarioCopiaOculta)
        End Get
    End Property

    Public Property AdjuntoUrl() As ArrayList
        Get
            Return _adjuntosUrl
        End Get
        Set(ByVal value As ArrayList)
            _adjuntosUrl = value
        End Set
    End Property

    Public ReadOnly Property VistaAlternativa() As AlternateViewCollection
        Get
            Return _manejadorMensaje.AlternateViews
        End Get
    End Property

#End Region

#Region "Métodos Públicos"

    Public Overloads Sub EstablecerCuentaOrigen(ByVal direccion As String)
        _manejadorMensaje.From = New MailAddress(direccion)
    End Sub

    Public Overloads Sub EstablecerCuentaOrigen(ByVal direccion As String, ByVal nombreAMostrar As String)
        _manejadorMensaje.From = New MailAddress(direccion, nombreAMostrar)
    End Sub

    Public Overloads Sub AdicionarDestinatario(ByVal direccion As String)
        _manejadorMensaje.To.Add(direccion)
    End Sub

    Public Overloads Sub AdicionarDestinatario(ByVal direccion As String, ByVal nombreAMostrar As String)
        _manejadorMensaje.To.Add(New MailAddress(direccion, nombreAMostrar))
    End Sub

    Public Overloads Sub AdicionarDestinatarioCopia(ByVal direccion As String)
        _manejadorMensaje.CC.Add(direccion)
    End Sub

    Public Overloads Sub AdicionarDestinatarioCopia(ByVal direccion As String, ByVal nombreAMostrar As String)
        _manejadorMensaje.CC.Add(New MailAddress(direccion, nombreAMostrar))
    End Sub

    Public Overloads Sub AdicionarDestinatarioCopiaOculta(ByVal direccion As String)
        _manejadorMensaje.Bcc.Add(direccion)
    End Sub

    Public Overloads Sub AdicionarDestinatarioCopiaOculta(ByVal direccion As String, ByVal nombreAMostrar As String)
        _manejadorMensaje.Bcc.Add(New MailAddress(direccion, nombreAMostrar))
    End Sub

    Public Overloads Sub EstablecerCuentaRespuesta(ByVal direccion As String, ByVal nombreAMostrar As String)
        _manejadorMensaje.ReplyTo = New MailAddress(direccion, nombreAMostrar)
    End Sub

    Public Sub EstablecerCredenciales(ByVal nombreUsuario As String, ByVal password As String, ByVal dominio As String)
        _clienteSmtp.Credentials = New NetworkCredential(nombreUsuario, password, dominio)
        _nombreUsuario = nombreUsuario
        _password = password
        _dominio = dominio
    End Sub

    Public Sub AdjuntarArchivos()

        For Each ruta As String In AdjuntoUrl
            If (ruta <> String.Empty) Then
                _adjuntos = New Attachment(ruta)
                _manejadorMensaje.Attachments.Add(_adjuntos)
            End If
        Next

    End Sub

    Public Sub Enviar()
        If _manejadorMensaje.From Is Nothing OrElse String.IsNullOrEmpty(_manejadorMensaje.From.Address) Then
            _manejadorMensaje.From = New MailAddress("system.notifier@logytechmobile.com", "Notificador de Eventos")
        End If
        With _clienteSmtp
            .DeliveryMethod = SmtpDeliveryMethod.Network
            .Send(_manejadorMensaje)
        End With

    End Sub

    Public Sub LimpiarDestinatarios()
        _manejadorMensaje.To.Clear()
    End Sub

    Public Sub LimpiarDestinatariosCopia()
        _manejadorMensaje.CC.Clear()
    End Sub

    Public Sub LimpiarDestinatariosCopiaOculta()
        _manejadorMensaje.Bcc.Clear()
    End Sub

    Public Sub LimpiarTodosLosDestinatarios()
        _manejadorMensaje.To.Clear()
        _manejadorMensaje.CC.Clear()
        _manejadorMensaje.Bcc.Clear()
    End Sub

    'Public Sub IncluirEncabezadoMensaje()
    '    Dim cuerpo As New System.Text.StringBuilder
    '    With cuerpo
    '        If Now.Hour < 12 Then
    '            .Append("<font class='fuente'>Buenos Días, ")
    '        ElseIf Now.Hour > 18 Then
    '            .Append("<font class='fuente'>Buenas Noches, ")
    '        Else
    '            .Append("<font class='fuente'>Buenas Tardes, ")
    '        End If
    '        .Append(_manejadorMensaje.Body)
    '        .Append("Este mensaje fue generado automáticamente, si tiene alguna duda o inquietud respecto al mismo, por favor envía un correo electrónico al grupo <a href='mailto:itdevelopment@logytechmobile.com?subject=Inquietud Notificador'> IT Development </a>")
    '    End With
    '    _manejadorMensaje.Body = cuerpo.ToString()
    '    _manejadorMensaje.BodyEncoding = System.Text.Encoding.UTF7
    'End Sub

    Public Sub IncluirEncabezadoMensaje()
        Dim firmaMensaje = "<br> Logytech Mobile S.A.S."
        Dim notaMensaje = "Este mensaje fue generado automáticamente, si tiene alguna duda o inquietud respecto al mismo, por favor envía un correo electrónico al grupo <a href='mailto:itdevelopment@logytechmobile.com?subject=Inquietud Notificador'> IT Development </a>"
        Dim cuerpo As New System.Text.StringBuilder


        With cuerpo
            .Append("<HTML>")
            .Append("      <HEAD>")
            .Append("             <LINK href='include/styleBACK.css' type='text/css' rel='stylesheet'>")
            .Append("             <LINK href='Estilos/estiloContenido.css' type='text/css' rel='stylesheet'>")
            .Append("      </HEAD>")
            .Append("      <body class='cuerpo2'>")
            .Append("      <table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'")
            .Append("             ID='Table1'>")
            .Append("             <tr>")
            .Append("                   <td width='20%' ><img src='http://www.logytechmobile.com/notusils/images/logo_trans.png'>")
            .Append("                   </td>")
            .Append("                   <td align='center' bgcolor='#38610B' width='80%'><font size='3' face='arial' color='white'><b>" & _manejadorMensaje.Subject & "</b></font></td>")
            .Append("             </tr>")
            .Append("      </table>")
            .Append("      <br />")
            .Append("      <br />")

            If Now.Hour < 12 Then
                .Append("  <font class='fuente'>Buenos Días, ")
            ElseIf Now.Hour > 18 Then
                .Append("  <font class='fuente'>Buenas Noches, ")
            Else
                .Append("  <font class='fuente'>Buenas Tardes, ")
            End If

            .Append("<br /><br />" & _manejadorMensaje.Body)
            .Append("      <br />")
            .Append("</font>")
            .Append("<br />       <font class='fuente'>Cordial Saludo,<br />")
            .Append("             <br><b>" & firmaMensaje & "</b><br /><br />")
            .Append("</font><br /><br /><br /><br /><br />")
            If notaMensaje <> "" Then
                .Append("  <font class='fuente' size='1'><i>Nota: " & notaMensaje & ".</i></font")
            End If
            .Append("      </body>")
            .Append("</HTML>")
        End With

        _manejadorMensaje.Body = cuerpo.ToString()
        _manejadorMensaje.BodyEncoding = System.Text.Encoding.UTF7

    End Sub




#End Region

End Class
