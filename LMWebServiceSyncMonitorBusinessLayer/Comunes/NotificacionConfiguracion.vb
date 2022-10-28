Imports System.Text
Imports System.Configuration

Public Class NotificacionConfiguracion


    Private _cuerpoMensaje As String
    Private _destinartariosPP As New ArrayList
    Private _destinartariosCC As New ArrayList
    Private _titulo As String
    Private _tipoNotificacion As Integer
    Private _textoMensaje As String

    Public ReadOnly Property CuerpoMensaje() As String
        Get
            Return _cuerpoMensaje
        End Get

    End Property

    Public ReadOnly Property DestinarariosPP() As ArrayList
        Get
            Return _destinartariosPP
        End Get
    End Property

    Public ReadOnly Property DestinarariosCC() As ArrayList
        Get
            Return _destinartariosCC
        End Get
    End Property

    Public ReadOnly Property Titulo() As String
        Get
            Return _titulo
        End Get
    End Property

    Public Property TipoNotificacion() As Integer
        Get
            Return _tipoNotificacion
        End Get
        Set(ByVal value As Integer)
            _tipoNotificacion = value
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

    Public Sub New()
        MyBase.New()
        _destinartariosPP.Clear()
        _destinartariosCC.Clear()
        _titulo = String.Empty
        _cuerpoMensaje = String.Empty
    End Sub

    Private Sub CargarInformacion()
        Dim dm As New LMDataAccessLayer.LMDataAccess
        Dim index As Integer = 0
        Dim dtConfiguracion As New DataTable
        Try
            With dm
                .agregarParametroSQL("@idAsuntoNotificacion", _tipoNotificacion, SqlDbType.Int)
                dtConfiguracion = .ejecutarDataTable("ObtenerInfoUsuarioNotificacion", CommandType.StoredProcedure)

                For Each drConfig As DataRow In dtConfiguracion.Rows
                    _titulo = drConfig.Item("nombreAsunto")
                    If Integer.Parse(drConfig.Item("tipoDestino").ToString) = 1 Then
                        _destinartariosPP.Add(drConfig.Item("email") & ";" & drConfig.Item("nombreCompleto"))
                    Else
                        _destinartariosCC.Add(drConfig.Item("email") & ";" & drConfig.Item("nombreCompleto"))
                    End If
                Next
              
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If dm IsNot Nothing Then dm.Dispose()
        End Try

        _cuerpoMensaje = CrearCuerpoMensajeHTML()

    End Sub

    Private Function CrearCuerpoMensajeHTML() As String
        Dim cm As New StringBuilder
        Dim nombreServidor As String = ""
        Dim nombreSitioWeb As String = ""
        If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("httpServerName")) Then nombreServidor = ConfigurationManager.AppSettings("httpServerName").ToString
        If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("webSiteName")) Then nombreSitioWeb = ConfigurationManager.AppSettings("webSiteName").ToString
        With cm
            .Append("<html>")
            .Append("	<head>")
            .Append("		<LINK href='" & nombreServidor & nombreSitioWeb & "/include/styleBACK.css' type='text/css' rel='stylesheet'>")
            .Append("	</head>")
            .Append("	<body class='cuerpo2'>")
            .Append("	<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'")
            .Append("		ID='Table1'>")
            .Append("		<tr>")
            .Append("			<td width='20%' ><img src='" & nombreServidor & nombreSitioWeb & "/images/logo_trans.png'>")
            .Append("			</td>")
            .Append("			<td align='center' bgcolor='#38610B' width='80%'><font size='3' face='arial' color='white'><b>" & _titulo & "</b></font></td>")
            .Append("		</tr>")
            .Append("	</table>")
            .Append("	<br />")
            .Append("	<br />")

            If Now.Hour < 12 Then
                .AppendLine("Buenos Dias")
            ElseIf Now.Hour >= 12 And Now.Hour < 19 Then
                .AppendLine("Buenas Tardes")
            Else
                .AppendLine("Buenas Noches")
            End If
            .AppendLine("<br/><br/>" & _textoMensaje & "<br/>")
            .AppendLine("<font class=""fuente"">")
            .AppendLine("Cordial Saludo<br><br>")
            .AppendLine("Proceso IT<br><br></font>")
            .AppendLine("<font class=""fuente"" size=""1""><i>")
            .AppendLine("Nota: Este correo es generado automaticamente, si tiene alguna duda, inquietud o comentario ")
            .AppendLine("envienos sus observaciones via e-mail al grupo IT Development")
            .AppendLine("</i></font></font></html>")
        End With
        Return cm.ToString

    End Function

    Public Sub ObtenerPorId(ByVal idAsuntoNotificacion As Integer)
        _tipoNotificacion = idAsuntoNotificacion
        CargarInformacion()
    End Sub

End Class