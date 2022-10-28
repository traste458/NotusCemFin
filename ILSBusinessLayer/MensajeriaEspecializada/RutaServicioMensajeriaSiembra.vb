Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes

Namespace MensajeriaEspecializada

    Public Class RutaServicioMensajeriaSiembra
        Inherits RutaServicioMensajeria

#Region "Funciones"

        Public Overrides Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim noResultado As Integer = -1
            Dim idRutaServicio As Integer

            Using dbManager As New LMDataAccess
                With dbManager
                    Try
                        With .SqlParametros
                            If _idResponsableEntrega > 0 Then .Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega
                            .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            If _fechaCreacion <> Date.MinValue Then .Add("@fechaCreacion", SqlDbType.DateTime).Value = _fechaCreacion
                            If _fechaSalida <> Date.MinValue Then .Add("@fechaSalida", SqlDbType.DateTime).Value = _fechaSalida
                            If _fechaCierre <> Date.MinValue Then .Add("@fechaCierre", SqlDbType.DateTime).Value = _fechaCierre
                            .Add("@idUsuarioLog", SqlDbType.Int).Value = _idUsuarioLog
                            .Add("@idTipoRuta", SqlDbType.Int).Value = _tipoRuta

                            .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@idRutaServicio", SqlDbType.Int).Direction = ParameterDirection.Output
                        End With

                        .iniciarTransaccion()

                        'Registro en [RutaServicio]
                        .ejecutarNonQuery("RegistrarRutaServicioMensajeria", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)
                        Integer.TryParse(.SqlParametros("@idRutaServicio").Value.ToString(), idRutaServicio)

                        If idRutaServicio > 0 Then
                            If _serviciosDatatable IsNot Nothing AndAlso _serviciosDatatable.Rows.Count > 0 Then
                                Dim columnaIdRuta As New DataColumn("idRuta", GetType(Integer), idRutaServicio)
                                _serviciosDatatable.Columns.Add(columnaIdRuta)

                                Dim columnaidUsuario As New DataColumn("idUsuarioLog", GetType(Integer), _idUsuarioLog)
                                _serviciosDatatable.Columns.Add(columnaidUsuario)

                                .inicilizarBulkCopy()

                                If _tipoRuta = Enumerados.TipoRutaServicioMensajeria.RecoleccionClienteSiembra Then
                                    With .BulkCopy
                                        .DestinationTableName = "DetalleDespachoServicioMensajeria"
                                        .ColumnMappings.Add("idRuta", "idRuta")
                                        .ColumnMappings.Add("idDetalleSerial", "idDetalleSerial")
                                        .ColumnMappings.Add("idUsuarioLog", "idUsuarioLog")
                                        .WriteToServer(_serviciosDatatable)
                                    End With
                                End If

                                'Se realiza el cambio de estado de los Servicios
                                If _serviciosDatatable.Columns.Contains("idServicio") Then
                                    Dim idServicioTemp As Integer = CInt(_serviciosDatatable(0).Item("idServicio"))
                                    infoEstados = New InfoEstadoRestriccionCEM(Enumerados.TipoServicio.Siembra, _
                                                               Enumerados.ProcesoMensajeria.Legalización, _
                                                               Enumerados.ProcesoMensajeria.Entrega_Recolección_Siembra, 0)
                                End If

                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = idRutaServicio
                                If (_idUsuarioLog > 0) Then
                                    .SqlParametros.Add("@idUsuarioLog", SqlDbType.Int).Value = _idUsuarioLog
                                End If
                                If infoEstados IsNot Nothing Then .SqlParametros.Add("@idEstadoServicio", SqlDbType.Int).Value = infoEstados.IdEstadoSiguiente
                                .SqlParametros.Add("@idEstadoRuta", SqlDbType.Int).Value = Enumerados.RutaMensajeria.Reparto

                                .ejecutarNonQuery("ActualizaEstadoServiciosRuta", CommandType.StoredProcedure)

                                .confirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, idRutaServicio.ToString() & "-Transacción exitosa.")
                            Else
                                resultado.EstablecerMensajeYValor(2, "Imposible registrar ruta sin servicios asociados.")
                                .abortarTransaccion()
                            End If
                        Else
                            resultado.EstablecerMensajeYValor(1, "Imposible crear el registro en la tabla Ruta Servicio.")
                            .abortarTransaccion()
                        End If
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw ex
                    End Try
                End With
            End Using

            Return resultado
        End Function

#End Region

    End Class

End Namespace

