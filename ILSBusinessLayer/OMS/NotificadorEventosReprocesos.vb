Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Comunes
Imports LMDataAccessLayer
Imports System.Net.Mail
Imports System.Text
Imports System.IO

Public Class NotificadorEventosReprocesos

#Region "Métodos Públicos"

    Public Function NotificarCreacion(ByVal idInstruccion As Integer) As ResultadoProceso
        Dim Notificacion As New AdministradorCorreo
        Dim DestinosPP As New MailAddressCollection
        Dim DestinosCC As New MailAddressCollection
        Dim respuestaEnvio As ResultadoProceso
        Dim sbContenido As New StringBuilder
        'Dim _adjuntos As New ArrayList
        'Dim dtAdjunto As New DataTable
        Try

            With sbContenido
                .Append("Se notifica la creación de la instrucción de reproceso número: " & idInstruccion)
                .Append("<br/>Por favor ingrese al sistema para visualizar las intrucciones pendientes. ")
            End With

            With Notificacion

                CargarDestinatarios(AsuntoNotificacion.Tipo.NotificaciónInstrucciónReproceso, DestinosPP, DestinosCC)

                'dtAdjunto = ObtenerRuta(idInstruccion)
                'Dim dr As DataRow() = dtAdjunto.Select("idAutorizacion=" & idInstruccion.ToString())
                'Dim ruta As String = dr(0).Item("rutaArchivo").ToString
                '_adjuntos.Add(ruta)

                .Titulo = "Creación Instrucción Reproceso"
                .Asunto = "Notificación de creación instrucción reproceso"
                .TextoMensaje = sbContenido.ToString
                .FirmaMensaje = "Logytech Mobile S.A.S <br />"
                .Receptor = DestinosPP
                .Copia = DestinosCC
                '.RutaAttachment = _adjuntos
                If Not .EnviarMail() Then
                    respuestaEnvio.Valor = 1
                    respuestaEnvio.Mensaje = "Ocurrió un error inesperado y no fué posible enviar la notificación"
                End If
            End With

        Finally

        End Try
    End Function

    Public Function NotificarEnvioLectura(ByVal IdInstruccion As Integer, ByVal mensaje As String, ByVal ruta As String, _
                                          ByVal consecutivo As Integer, ByVal creador As String) As ResultadoProceso

        Dim Notificacion As New AdministradorCorreo
        Dim DestinosPP As New MailAddressCollection
        Dim DestinosCC As New MailAddressCollection
        Dim resultado As New ResultadoProceso
        Dim sbContenido As New StringBuilder
        Dim _adjuntos As New ArrayList
        Dim dtAdjunto As New DataTable

        Try
            With sbContenido
                .Append("Se notifica la lectura de seriales para la instrucción de reproceso número: " & IdInstruccion & ", consecutivo de envío: CONS" & consecutivo)
                .Append("<br/>A continuación se relaciona el detalle de la lectura. ")
                .Append("<br/>" & mensaje)
                .Append("<br/> Instrucción Creada por: " & creador)
            End With

            With Notificacion
                CargarDestinatarios(AsuntoNotificacion.Tipo.NotificaciónEnvioLecturaReproceso, DestinosPP, DestinosCC)
                _adjuntos.Add(ruta)

                If DestinosPP.Count > 0 Then
                    .Titulo = "Envío de lectura de reprocesos"
                    .Asunto = "Notificación de lectura de seriales para la instrucción de reproceso número: " & IdInstruccion & ", consecutivo de envío: CONS" & consecutivo
                    .TextoMensaje = sbContenido.ToString
                    .FirmaMensaje = "Logytech Mobile S.A.S <br />"
                    .Receptor = DestinosPP
                    .Copia = DestinosCC
                    .RutaAttachment = _adjuntos
                    If Not .EnviarMail() Then
                        resultado.Valor = 1
                        resultado.Mensaje = "Ocurrió un error inesperado y no fué posible enviar la notificación"
                    Else
                        resultado.Valor = 0
                        resultado.Mensaje = "El envío de lectura se realizo satisfactoriamente. "
                    End If
                Else
                    resultado.Valor = 2
                    resultado.Mensaje = "No se encontraron destinatarios para el envío de notificación. "
                End If
            End With

        Catch ex As Exception
            Throw New Exception
        End Try
        Return resultado
    End Function

    Private Function CargarDestinatarios(ByVal tipo As Comunes.AsuntoNotificacion.Tipo, ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection) As MailAddressCollection
        Dim ConfiguracionUsuario As New UsuarioNotificacion
        Dim filtro As New FiltroUsuarioNotificacion
        Dim dtDestinos As New DataTable
        Dim strDestinoPP, strDestinoCC As String

        filtro.IdAsuntoNotificacion = tipo
        filtro.Separador = ","
        Try
            dtDestinos = ConfiguracionUsuario.ObtenerDestinatarioNotificacion(filtro)
            For Each fila As DataRow In dtDestinos.Rows
                strDestinoPP += fila.Item("destinoPara")
                strDestinoCC += fila.Item("destinoCopia")
            Next

            destinoPP.Add(strDestinoPP)
            destinoCC.Add(strDestinoCC)

        Catch ex As Exception
        Finally
            If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
        End Try

    End Function

    Private Function ObtenerRuta() As DataTable
        'Dim _dbManager As New LMDataAccessLayer.LMDataAccess
        'Try
        '    With _dbManager
        '        'If idAutorizacion > 0 Then _
        '        ' .SqlParametros.Add("@idAutorizacion", SqlDbType.Int).Value = idAutorizacion
        '    End With
        '    Return _dbManager.ejecutarDataTable("ConsultarAutorizacionesCambioSoftware", CommandType.StoredProcedure)
        'Catch ex As Exception
        '    Throw New Exception(ex.Message)
        'Finally
        '    If _dbManager IsNot Nothing Then _dbManager.Dispose()
        'End Try

    End Function

#End Region

End Class
