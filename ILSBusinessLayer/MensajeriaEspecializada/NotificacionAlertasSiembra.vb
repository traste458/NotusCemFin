Imports LMDataAccessLayer
Imports System.Net.Mail
Imports System.Text
Imports LMMailSenderLibrary
Imports System.Net.Mime
Imports System.Configuration
Imports ILSBusinessLayer.Comunes
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.MensajeriaEspecializada

Public Class NotificacionAlertasSiembra

#Region "Atributos"

    Private _nombreUsuario As String
    Private _password As String
    Private _dominio As String
    Private _nombreServidor As String
    Private _cuentaOrigen As String

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function EnviarNotificaciones(Optional _idServicio As Integer = 0) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dtNotificaciones As DataTable
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Clear()
                    If _idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                    dtNotificaciones = .EjecutarDataTable("ObtieneServiciosSiembraPendientesNotificacion", CommandType.StoredProcedure)
                    If dtNotificaciones.Rows.Count > 0 Then
                        Dim dtServicios As DataTable = dtNotificaciones.DefaultView.ToTable(True, "idServicioMensajeria", "nombreCliente", "idPersonaConsultor", "tipoNotificacion", "descripcion")
                        For Each servicio As DataRow In dtServicios.Rows
                            Dim infoCorreo As New DatosCorreo
                            With infoCorreo
                                If Not IsDBNull(servicio("idPersonaConsultor")) Then
                                    .idServicio = CLng(servicio("idServicioMensajeria"))
                                    .nombreCliente = servicio("nombreCliente")
                                    .Consultor = New Usuario(idUsuario:=CInt(servicio("idPersonaConsultor")))

                                    If CInt(servicio("tipoNotificacion")) = TipoNotificacion.Primera Then
                                        .tipoNotificacion = TipoNotificacion.Primera
                                    ElseIf CInt(servicio("tipoNotificacion")) = TipoNotificacion.Segunda Then
                                        .tipoNotificacion = TipoNotificacion.Segunda
                                    ElseIf CInt(servicio("tipoNotificacion")) = TipoNotificacion.Manual Then
                                        .tipoNotificacion = TipoNotificacion.Manual
                                    End If

                                    .dtMSISDN = dtNotificaciones.Select("idServicioMensajeria = " & servicio("idServicioMensajeria").ToString())

                                    resultado = EnviarCorreoNotificacion(infoCorreo)
                                End If
                            End With
                        Next
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

#Region "Métodos Privados"

    Private Function EnviarCorreoNotificacion(ByVal datos As DatosCorreo) As ResultadoProceso
        Dim resultado As New ResultadoProceso

        Try
            Dim objEmail As New EMailManager(AsuntoNotificacion.Tipo.NotificaciónVencimientoSiembra)

            With objEmail
                'Asunto
                Select Case datos.tipoNotificacion
                    Case TipoNotificacion.Primera
                        .Prioridad = Net.Mail.MailPriority.Normal
                        .Asunto = "Alerta Vencimiento (Semana) " & datos.nombreCliente
                    Case TipoNotificacion.Segunda
                        .Prioridad = Net.Mail.MailPriority.High
                        .Asunto = "Alerta Vencimiento (3 días) " & datos.nombreCliente
                End Select

                'Cuerpo
                Dim sbMensaje As New StringBuilder
                With sbMensaje
                    Select Case datos.tipoNotificacion
                        Case TipoNotificacion.Primera
                            .Append("Dentro de una semana se vence el plazo de préstamo de(los) equipo(s) préstados a: ")
                            .Append("<b>" & datos.nombreCliente & "</b> ")
                            .Append("pertenecientes al Número de Servicio <b>" & datos.idServicio & "</b>.<br/><br/>")
                            .Append("A continuación se relacionan el(los) equipo(s):<br/><br/>")


                        Case TipoNotificacion.Segunda
                            .Append("Dentro de tres (3) días se vence el plazo de préstamo de(los) equipo(s) préstados a: ")
                            .Append("<b>" & datos.nombreCliente & "</b> ")
                            .Append("pertenecientes al Número de Servicio <b>" & datos.idServicio & "</b>.<br/><br/>")
                            .Append("A continuación se relacionan el(los) equipo(s):<br/><br/>")
                    End Select

                    .Append("<table style='border: 1px solid #000; border-collapse: collapse;'>")
                    .Append("   <tr>")
                    .Append("       <th style='border: 1px solid #000;'><p>MSISDN</p></th>")
                    .Append("       <th style='border: 1px solid #000;'><p>Fecha Devolución</p></th>")
                    .Append("   </tr>")
                    For Each equipo As DataRow In datos.dtMSISDN
                        .Append("   <tr>")
                        .Append("       <td style='border: 1px solid #000;'><p>" & equipo("msisdn") & "</p></td>")
                        .Append("       <td style='border: 1px solid #000;'><p>" & equipo("fechaDevolucion") & "</p></td>")
                        .Append("   </tr>")
                    Next
                    .Append("</table>")
                End With
                .TextoMensaje = sbMensaje.ToString()

                'Se notifica al consultor
                Dim objServicio As New ServicioMensajeriaSiembra(datos.idServicio)
                If Not String.IsNullOrEmpty(objServicio.EmailConsultor) Then .Destanatarios.Add(objServicio.EmailConsultor)

            End With
            objEmail.EnviarMail()

            resultado = RegistrarNotificacion(datos)
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function

    Private Function RegistrarNotificacion(ByVal datos As DatosCorreo) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    Dim dtNotificacion As DataTable = datos.dtMSISDN.CopyToDataTable()

                    .InicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "NotificacionServicioSiembra"
                        .ColumnMappings.Add("idMin", "idMin")
                        .ColumnMappings.Add("tipoNotificacion", "tipoNotificacion")
                        .WriteToServer(dtNotificacion)
                    End With
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

#Region "Propiedades"

    Public ReadOnly Property NombreUsuario() As String
        Get
            If _nombreUsuario = String.Empty Then _nombreUsuario = System.Configuration.ConfigurationManager.AppSettings("NombreUsuario")
            Return _nombreUsuario
        End Get
    End Property

    Public ReadOnly Property Password() As String
        Get
            If _password = String.Empty Then _password = System.Configuration.ConfigurationManager.AppSettings("Password")
            Return _password
        End Get
    End Property

    Public ReadOnly Property Dominio() As String
        Get
            If _dominio = String.Empty Then _dominio = System.Configuration.ConfigurationManager.AppSettings("Dominio")
            Return _dominio
        End Get
    End Property

    Public ReadOnly Property NombreServidor() As String
        Get
            If _nombreServidor = String.Empty Then _nombreServidor = System.Configuration.ConfigurationManager.AppSettings("NombreServidor")
            Return _nombreServidor
        End Get
    End Property

    Public ReadOnly Property CuentaOrigen() As String
        Get
            If _cuentaOrigen = String.Empty Then _cuentaOrigen = System.Configuration.ConfigurationManager.AppSettings("CuentaOrigen")
            Return _cuentaOrigen
        End Get
    End Property

#End Region

#Region "Estructuras"

    Private Enum TipoNotificacion
        Primera = 1
        Segunda = 2
        Manual = 0
    End Enum

    Private Structure DatosCorreo

#Region "Atributos"

        Private arrDestinatariosPrincipal As ArrayList
        Private arrDestinatariosCopia As ArrayList
        Private dicDestinatariosCuerpo As Dictionary(Of String, String)
        Public idServicio As Long
        Public Consultor As Usuario
        Public nombreCliente As String
        Public tipoNotificacion As TipoNotificacion
        Public dtMSISDN() As DataRow

#End Region

#Region "Propiedades"

        Public ReadOnly Property DestinatariosPrincipal() As ArrayList
            Get
                If arrDestinatariosPrincipal Is Nothing Then arrDestinatariosPrincipal = New ArrayList
                Return arrDestinatariosPrincipal
            End Get
        End Property

        Public ReadOnly Property DestinatariosCopia() As ArrayList
            Get
                If arrDestinatariosCopia Is Nothing Then arrDestinatariosCopia = New ArrayList
                Return arrDestinatariosCopia
            End Get
        End Property

        Public ReadOnly Property DestinatariosCuerpo() As Dictionary(Of String, String)
            Get
                If dicDestinatariosCuerpo Is Nothing Then dicDestinatariosCuerpo = New Dictionary(Of String, String)
                Return dicDestinatariosCuerpo
            End Get
        End Property

#End Region

    End Structure

#End Region

End Class
