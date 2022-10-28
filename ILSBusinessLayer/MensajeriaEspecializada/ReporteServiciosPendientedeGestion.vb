Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mime
Imports LMMailSenderLibrary
Imports LMDataAccessLayer
Imports System.Configuration

Namespace MensajeriaEspecializada
    Public Class ReporteServiciosPendientedeGestion

#Region "Atributos (Filtros de Búsqueda)"
        Private _dtDatosReporte As DataTable
        Private _direccionDestino As ArrayList
        Private _nombreUsuario As String
        Private _password As String
        Private _dominio As String
        Private _nombreServidor As String
        Private _cuentaOrigen As String
        Private _resultado As New InfoResultado
#End Region

#Region "Propiedades"

        Public Property Resultado As InfoResultado
            Get
                Return _resultado
            End Get
            Set(value As InfoResultado)
                _resultado = value
            End Set
        End Property

        Public Property DtDatosReporte As DataTable
            Get
                Return _dtDatosReporte
            End Get
            Set(value As DataTable)
                _dtDatosReporte = value
            End Set
        End Property

        Public ReadOnly Property DireccionDestino() As ArrayList
            Get
                If _direccionDestino Is Nothing Then _direccionDestino = New ArrayList
                Return _direccionDestino
            End Get
        End Property

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

#End Region

#Region "Métodos Públicos"

        Public Function ObtenerMailNotificaciones(ByVal idNotificacion As Integer) As DataTable
            Dim _dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With _dbManager
                    .SqlParametros.Add("@idAsuntoNotificacion", SqlDbType.Int).Value = idNotificacion
                End With
                Return _dbManager.ejecutarDataTable("ObtenerUsuarioNotificacion", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
        End Function

        Public Function ObtenerReporte(ByVal nombreArchivo As String, ByVal rutaPlantilla As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .abrirConexion()
                    '_dtDatosReporte = .ejecutarDataTable("ObtenerInformacionServiciosPendientesdeGestion", CommandType.StoredProcedure)
                    _resultado = .GenerarArchivoExcel("ObtenerInformacionServiciosPendientesdeGestion", nombreArchivo, CommandType.StoredProcedure, rutaPlantilla, "NOTIFICACIÓN DE SERVICIOS PENDIENTES DE GESTION", 4)

                    .cerrarConexion()
                End With
            Catch ex As Exception
                Throw ex
            End Try
            Return resultado
        End Function

        Public Sub NotificarServiciosPendientesGestion(ByVal Adjunto As String)
            If _direccionDestino IsNot Nothing AndAlso _direccionDestino.Count > 0 Then
                Dim mailSender As New MailMessage
                Dim mailHandler As New LMMailSender
                Dim fecha As String = DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy")
                InicializarCadenasDeTexto()
                Try
                    Dim mailBody As New StringBuilder
                    With mailBody
                        .Append("<html>")
                        .Append("<head><title>Notificador de Servicios Pendientes de Gestion</title>")
                        .Append("<style type='text/css'>")
                        .Append("body {font: 70%/1.5em Verdana, Tahoma, arial, sans-serif; margin: 5px 10px; padding: 5px 5px; background-color: #ffffff;}")
                        .Append(".tabla {font-family: Helvetica, Arial, sans-serif; font-size:9pt; border-color:Gray}")
                        .Append("</style></head>")
                        .Append("<body>")
                        .Append("<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'>")
                        .Append("<td align='center' bgcolor='#883485' width='80%'><font size='3' face='arial' color='white'>" & _
                                "<b>NOTIFICACIÓN DE SERVICIOS PENDIENTES DE GESTION</b></font></td></tr>")
                        .Append("</table><br><br>")
                        .Append("<table><tr>")
                        .Append("<td><font size='2'><b>&nbsp; A continuación se relacionan los Servicios que quedaron pendientes de gestión,  agendados para el día  " & fecha & " </b></td>")
                        .Append("<tr><td><font size='3'><b>&nbsp;  </b></td></tr>")
                        .Append("</tr></table>")
                        .Append("<font name='Verdana' size='2'>")
                        .Append("<br><br><font name='Verdana' size='2'><b>Atentamente,<br>IT Development - Logytech Mobile S.A.S</b></font><br><br>")
                        .Append("<font class='Verdana' size='1'><i>Nota: Este correo es generado automaticamente, ")
                        .Append("si tiene alguna duda, inquietud o comentario por favor envíe un e-mail a 'IT Development' <ITDevelopment@logytechmobile.com>")
                        .Append("</body>")
                        .Append("</html>")
                    End With
                    Dim htmlBody As AlternateView = AlternateView.CreateAlternateViewFromString(mailBody.ToString, Nothing, MediaTypeNames.Text.Html)
                    With mailHandler
                        .ServidorCorreo = _nombreServidor
                        .EstablecerCredenciales(_nombreUsuario, _password, _dominio)
                        .EstablecerCuentaOrigen(_cuentaOrigen)
                        For index As Integer = 0 To _direccionDestino.Count - 1
                            .Destanatarios.Add(_direccionDestino(index))
                        Next
                        If Not String.IsNullOrEmpty(Adjunto) Then
                            Dim _adjuntos As New ArrayList
                            _adjuntos.Add(Adjunto)
                            .AdjuntoUrl = _adjuntos
                            .AdjuntarArchivos()
                        End If
                        .Prioridad = Net.Mail.MailPriority.High
                        .CuerpoEsHtml = True
                        .Asunto = "Notificación de Servicios pendientes de gestion " & Date.Now
                        .VistaAlternativa.Add(htmlBody)
                        .Enviar()
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try
            End If
        End Sub

#End Region

#Region "Metodos Privados"
        Private Sub InicializarCadenasDeTexto()
            _nombreUsuario = "system.notifier"
            _password = "12345.LM"
            _dominio = "LM"
            _nombreServidor = ConfigurationManager.AppSettings("mailServer")
            _cuentaOrigen = ConfigurationManager.AppSettings("mailSender")
        End Sub
#End Region

    End Class
End Namespace
