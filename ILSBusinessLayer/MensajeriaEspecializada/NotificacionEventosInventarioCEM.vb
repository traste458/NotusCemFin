Imports System.IO
Imports System.Text
Imports System.Net.Mail
Imports System.Net.Mime
Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes
Imports ILSBusinessLayer.Productos
Imports ILSBusinessLayer.Estructuras

Namespace MensajeriaEspecializada

    Public Class NotificacionEventosInventarioCEM

#Region "Atributos"

        Private _inicioMensaje As String
        Private _finMensaje As String
        Private _titulo As String
        Private _asunto As String
        Private _firmaMensaje As String
        Private _mailRespuesta As String
        Private _usuarioRespuesta As String
        Private _tipoNotificacion As Comunes.AsuntoNotificacion.Tipo

#End Region

#Region "Propiedades"

        Public Property InicioMensaje As String
            Get
                Return _inicioMensaje
            End Get
            Set(value As String)
                _inicioMensaje = value
            End Set
        End Property

        Public Property FinMensaje As String
            Get
                Return _finMensaje
            End Get
            Set(value As String)
                _finMensaje = value
            End Set
        End Property

        Public Property Titulo As String
            Get
                Return _titulo
            End Get
            Set(value As String)
                _titulo = value
            End Set
        End Property

        Public Property Asunto As String
            Get
                Return _asunto
            End Get
            Set(value As String)
                _asunto = value
            End Set
        End Property

        Public Property MailRespuesta As String
            Get
                Return _mailRespuesta
            End Get
            Set(value As String)
                _mailRespuesta = value
            End Set
        End Property

        Public Property UsuarioRespuesta As String
            Get
                Return _usuarioRespuesta
            End Get
            Set(value As String)
                _usuarioRespuesta = value
            End Set
        End Property

        Public Property FirmaMensaje As String
            Get
                Return _firmaMensaje
            End Get
            Set(value As String)
                _firmaMensaje = value
            End Set
        End Property

        Public Property TipoNotificacion As Comunes.AsuntoNotificacion.Tipo
            Get
                Return _tipoNotificacion
            End Get
            Set(value As Comunes.AsuntoNotificacion.Tipo)
                _tipoNotificacion = value
            End Set
        End Property

#End Region

#Region "Métodos Públicos"

        Public Function NotificacionEvento(Optional ByVal mensaje As String = "", Optional mensajeDetalle As String = "", Optional idBodega As Integer = 0) As ResultadoProceso
            Dim Notificacion As New AdministradorCorreo
            Dim DestinosPP As New MailAddressCollection
            Dim DestinosCC As New MailAddressCollection
            Dim respuestaEnvio As New ResultadoProceso
            Dim sbContenido As New StringBuilder

            Try
                With sbContenido
                    .Append(_inicioMensaje)
                    If mensaje <> "" Then
                        .Append("<br/> " & mensaje)
                    End If
                    .Append("<br/> " & _finMensaje)
                    If mensajeDetalle <> "" Then
                        .Append("<br/> " & mensajeDetalle)
                    End If
                End With

                With Notificacion

                    respuestaEnvio = CargarDestinatarios(DestinosPP, DestinosCC, idBodega)
                    If respuestaEnvio.Valor = 20 Then
                        Return respuestaEnvio
                    Else
                        .Titulo = _titulo
                        .Asunto = _asunto
                        .TextoMensaje = sbContenido.ToString
                        .FirmaMensaje = _firmaMensaje
                        .Receptor = DestinosPP
                        .Copia = DestinosCC
                        If Not String.IsNullOrEmpty(_mailRespuesta) Or Not String.IsNullOrEmpty(_usuarioRespuesta) Then _
                            .EstablecerCuentaRespuesta(_mailRespuesta, _usuarioRespuesta)
                        If Not .EnviarMail() Then
                            respuestaEnvio.EstablecerMensajeYValor(1, "Ocurrió un error inesperado y no fué posible enviar la notificación")
                        End If
                    End If


                    
                End With
            Finally
            End Try
            Return respuestaEnvio
        End Function

#End Region

#Region "Métodos Privados"

        Public Function CargarDestinatarios(ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection, Optional ByVal idBodega As Integer = 0) As ResultadoProceso
            Dim ConfiguracionUsuario As New UsuarioNotificacion
            Dim filtro As New FiltroUsuarioNotificacion
            Dim dtDestinos As New DataTable
            Dim strDestinoPP As String = String.Empty
            Dim strDestinoCC As String = String.Empty
            Dim respuestaEnvio As New ResultadoProceso

            filtro.IdAsuntoNotificacion = _tipoNotificacion
            If idBodega <> 0 Then
                filtro.IdBodega = idBodega
            End If
            filtro.Separador = ","
            Try
                dtDestinos = UsuarioNotificacion.ObtenerDestinatarioNotificacion(filtro)
                For Each fila As DataRow In dtDestinos.Rows
                    strDestinoPP += fila.Item("destinoPara")
                    strDestinoCC += fila.Item("destinoCopia")
                Next
                If (strDestinoPP = "" And strDestinoCC = "") Then
                    respuestaEnvio.EstablecerMensajeYValor(20, "No se encontró información de destinatarios de Correo Para la bodega")
                    'Throw New System.Exception("No se encontró información de destinatarios de Correo Para la bodega")
                Else
                    If (strDestinoPP <> "") Then
                        destinoPP.Add(strDestinoPP)
                    End If
                    If (strDestinoCC <> "") Then
                        destinoCC.Add(strDestinoCC)
                    End If
                End If
                Return respuestaEnvio
            Catch ex As Exception
            Finally
                If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
            End Try

        End Function

#End Region

    End Class

End Namespace