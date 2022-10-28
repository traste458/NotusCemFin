Imports LMDataAccessLayer
Imports ILSBusinessLayer.MensajeriaEspecializada.OfficeTrack

Namespace MensajeriaEspecializada

    Public Class RutaServicioMensajeriaRadicado
        Inherits RutaServicioMensajeria

#Region "Atributos"

        Private _idZona As Short
        Private _listServicios As List(Of Long)

#End Region

#Region "Propiedades"

        Public Property IdZona As Short
            Get
                Return _idZona
            End Get
            Set(value As Short)
                _idZona = value
            End Set
        End Property

        Public Property ListaServicios As List(Of Long)
            Get
                Return _listServicios
            End Get
            Set(value As List(Of Long))
                _listServicios = value
            End Set
        End Property

#End Region

#Region "Funciones"

        Public Overrides Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Try
                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega
                        .SqlParametros.Add("@idZona", SqlDbType.SmallInt).Value = _idZona
                        .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .SqlParametros.Add("@idUsuarioLog", SqlDbType.Int).Value = _idUsuarioLog
                        .SqlParametros.Add("@idTipoRuta", SqlDbType.Int).Value = _tipoRuta
                        If _listServicios IsNot Nothing AndAlso _listServicios.Count > 0 Then _
                            .SqlParametros.Add("@listServicios", SqlDbType.VarChar).Value = String.Join(",", _listServicios.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 4000).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@idRutaServicio", SqlDbType.Int).Direction = ParameterDirection.Output

                        .iniciarTransaccion()

                        .ejecutarNonQuery("RegistrarRutaServicioMensajeriaRadicado", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), resultado.Valor)
                        Integer.TryParse(.SqlParametros("@idRutaServicio").Value.ToString(), _idRuta)

                        If resultado.Valor = 0 Then
                            resultado.Mensaje = "Se creó satisfactoriamente la Ruta de Entrega Número: [" & _idRuta.ToString() & "]"
                            .confirmarTransaccion()
                        Else
                            Select Case resultado.Valor
                                Case 1
                                    resultado.Mensaje = "Los siguientes radicados [" & .SqlParametros("@mensaje").Value & "] ya no se encuentran en estado válido para asignar a ruta, por favor verificar."
                                Case Else
                                    resultado.Mensaje = "No fue posible crear la ruta de entrega, por favor intenete nuevamente."
                            End Select
                            .abortarTransaccion()
                        End If
                        If resultado.Valor = 0 Then
                            '-------------OfficeTrack-------------
                            Dim EnviadoOfficeTrack As Boolean = False
                            Dim MensajeErrorOfficeTrack As String = ""
                            Dim idDetalle As Integer
                            Dim ConfigurationID As Integer = ConfigurationManager.AppSettings.Item("idDatabase")
                            Dim userName As String = ConfigurationManager.AppSettings.Item("userName")
                            Dim password As String = ConfigurationManager.AppSettings.Item("password")
                            Try

                                .SqlParametros.Clear()
                                Dim objOfficeTrack As New ConfiguracionOfficeTrack

                                For Each Detalle As DataRow In objOfficeTrack.CargarIdDetalleRuta(_idRuta, dbManager).Rows

                                    idDetalle = Integer.Parse(Detalle("idDetalle"))
                                    Dim pidDetalle As String = idDetalle
                                    .SqlParametros.Clear()
                                    With objOfficeTrack
                                        objOfficeTrack.CargarConfigOfficeTrack(pidDetalle, userName, password, ConfigurationID, dbManager)
                                    End With
                                Next

                                EnviadoOfficeTrack = True
                            Catch ex As Exception

                                .SqlParametros.Clear()
                                Dim TaskNumber As String = (idDetalle.ToString() & ConfigurationID.ToString())
                                .SqlParametros.Add("@TaskNumber", SqlDbType.BigInt).Value = Int64.Parse(TaskNumber)
                                .EjecutarNonQuery("ActualizarOfficeTrackTaskNoEnviado", CommandType.StoredProcedure)

                                logServicio("   IdDetalle:" + idDetalle.ToString())
                                logServicio("   ConfigurationID:" + ConfigurationID.ToString())
                                logServicio("   Message: " + ex.Message)
                                logServicio("   StackTrace: " + ex.StackTrace)

                                EnviadoOfficeTrack = False
                                MensajeErrorOfficeTrack = ex.Message
                            End Try
                            '-------------Fin OfficeTrack-------------
                        End If
                    End With
                End Using
                Return resultado
            Catch ex As Exception
                Throw ex
            End Try
        End Function

#End Region

    End Class

End Namespace
