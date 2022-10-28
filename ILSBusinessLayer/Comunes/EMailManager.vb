Imports System.Reflection
Imports System.IO
Imports System.Text

Namespace Comunes

    Public Class EMailManager
        Inherits AdministradorCorreo

#Region "Propiedades"

        Private Property TipoAsuntoNotificacion As AsuntoNotificacion.Tipo
        Private Property InfoAdicional As Object()

#End Region

#Region "Constructores"

        Public Sub New(ByVal AsuntoNotificacion As AsuntoNotificacion.Tipo)
            MyBase.New()
            Me.TipoAsuntoNotificacion = AsuntoNotificacion

            CargarDatos()
        End Sub

        Public Sub New(ByVal AsuntoNotificacion As AsuntoNotificacion.Tipo, ByVal ParamArray Informacion As Object())
            MyBase.New()
            Me.TipoAsuntoNotificacion = AsuntoNotificacion
            Me.InfoAdicional = Informacion

            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Try
                Dim objAsunto As New AsuntoNotificacion(Me.TipoAsuntoNotificacion)

                'Establece Destinatarios
                For Each destino As UsuarioNotificacion In objAsunto.ListaUsuarios
                    Select Case destino.TipoDestino
                        Case TipoDestino.Principal
                            Me.Receptor.Add(destino.Email)

                        Case TipoDestino.Copia
                            Me.Copia.Add(destino.Email)

                    End Select
                Next

                'Establece campos
                For Each elementoCorreo As DetalleAsuntoNotificacion In objAsunto.ListaDetalleAsunto
                    GenerarInformacion(elementoCorreo.Mensaje)

                    Select Case elementoCorreo.Seccion.ToUpper()
                        Case "ASUNTO"
                            Me.Asunto = elementoCorreo.Mensaje

                        Case "TITULO"
                            Me.Titulo = elementoCorreo.Mensaje

                        Case "MENSAJE"
                            Me.TextoMensaje = elementoCorreo.Mensaje

                        Case "FIRMA"
                            Me.FirmaMensaje = elementoCorreo.Mensaje

                        Case "NOTA_PIE"
                            Me.NotaMensaje = elementoCorreo.Mensaje

                        Case "NOTA_LEGAL"
                            Me.MensajeLegal = elementoCorreo.Mensaje

                    End Select
                Next

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Function GetCurrentAssemblyPath() As String
            Dim dllFilePath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
            Dim directoryPath As String = System.IO.Path.GetDirectoryName(dllFilePath)
            Return directoryPath
        End Function

        Private Function ObtenerInfoCuerpo() As String
            Dim info As String = String.Empty
            'Using sr As New StreamReader(GetCurrentAssemblyPath() & "\ArchivosAuxiliares\EstiloCorreo.htm")
            Using sr As New StreamReader(Path.GetDirectoryName(New Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) & "\ArchivosAuxiliares\EstiloCorreo.htm")
                info = sr.ReadToEnd()
            End Using
            Return info
        End Function

#End Region

#Region "Métodos Públicos"

        Protected Overrides Sub CrearCuerpoMensajeHTML()
            Dim strCuerpo As String = ObtenerInfoCuerpo()
            Dim strTextoMensaje As String
            Dim dt As DateTime = DateTime.Now

            If Not String.IsNullOrEmpty(Me.Titulo) Then strCuerpo = strCuerpo.Replace("[[TITULO]]", Me.Titulo)
            strCuerpo = strCuerpo.Replace("[[FECHA]]", dt.ToString("MMMM dd, yyyy", New System.Globalization.CultureInfo("es-CO")))

            If Now.Hour < 12 Then
                strTextoMensaje = "Buenos Días,"
            ElseIf Now.Hour > 18 Then
                strTextoMensaje = "Buenas Noches,"
            Else
                strTextoMensaje = "Buenas Tardes,"
            End If
            strTextoMensaje = strTextoMensaje & "</br></br>" & Me.TextoMensaje
            strCuerpo = strCuerpo.Replace("[[CUERPO]]", strTextoMensaje)

            If Not String.IsNullOrEmpty(Me.FirmaMensaje) Then strCuerpo = strCuerpo.Replace("[[FIRMA]]", Me.FirmaMensaje)
            If Not String.IsNullOrEmpty(Me.NotaMensaje) Then strCuerpo = strCuerpo.Replace("[[NOTA]]", Me.NotaMensaje)
            If Not String.IsNullOrEmpty(Me.MensajeLegal) Then strCuerpo = strCuerpo.Replace("[[LEGAL]]", Me.MensajeLegal)

            Me.Mensaje.Body = strCuerpo
        End Sub

        Private Function GenerarInformacion(ByRef mensaje As String) As String
            If Me.InfoAdicional IsNot Nothing Then
                For index As Integer = 0 To Me.InfoAdicional.Length - 1
                    Dim objType As Type = Me.InfoAdicional(index).GetType()
                    Dim pInfo As PropertyInfo

                    For Each pInfo In objType.GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            mensaje = Replace(mensaje, "[[" & pInfo.Name & "]]", pInfo.GetValue(Me.InfoAdicional(index), Nothing))
                        End If
                    Next
                Next
            End If
            Return mensaje
        End Function

#End Region

#Region "Enumerados"

        Public Enum TipoDestino As Short
            Principal = 1
            Copia = 2
        End Enum

#End Region

    End Class

End Namespace
