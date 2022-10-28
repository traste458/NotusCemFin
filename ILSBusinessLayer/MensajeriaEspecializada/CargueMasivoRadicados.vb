Imports LMDataAccessLayer

Public Class CargueMasivoRadicados
    Public Function CargarRadicados(ByVal dtDetalleMines As DataTable, ByVal dtDetalleReferencias As DataTable, ByVal dtInformacionGeneral As DataTable, ByVal idUsuario As Integer, ByRef resultado As Int32) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        dtDetalleMines.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        dtDetalleReferencias.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        dtInformacionGeneral.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .ejecutarNonQuery("EliminaRegistroMasivoServicioMensajeria", CommandType.StoredProcedure)
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "DetalleReferencias_Equipos_Carga_Radicados"
                    .ColumnMappings.Add("NUMERO DE RADICADO", "numeroRadicado")
                    .ColumnMappings.Add("MATERIAL", "material")
                    .ColumnMappings.Add("CANTIDAD", "cantidad")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtDetalleReferencias)
                End With
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "DetalleMines_Carga_Radicados"
                    .ColumnMappings.Add("NUMERO DE RADICADO", "numeroRadicado")
                    .ColumnMappings.Add("MIN", "msisdn")
                    .ColumnMappings.Add("ACTIVA EQUIPO ANTERIOR (S/N) - OPCIONAL", "activaEquipoAnterior")
                    .ColumnMappings.Add("COMSEGURO (S/N) - OPCIONAL", "comSeguro")
                    .ColumnMappings.Add("NUMERO DE RESERVA - OPCIONAL", "numeroReserva")
                    .ColumnMappings.Add("PRECIO SIN IVA", "precioSinIVA")
                    .ColumnMappings.Add("PRECIO CON IVA", "precioConIVA")
                    .ColumnMappings.Add("CLAUSULA", "Clausula")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtDetalleMines)
                End With
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "InformacionGeneral_Carga_Radicados"
                    .ColumnMappings.Add("TIPO DE SERVICIO", "TipoServicio")
                    .ColumnMappings.Add("NUMERO DE RADICADO", "numeroRadicado")
                    .ColumnMappings.Add("PRIORIDAD", "idPrioridad")
                    .ColumnMappings.Add("FECHA VENCIMIENTO RESERVA", "fechaVencimientoReserva")
                    .ColumnMappings.Add("USUARIO EJECUTOR (OPCIONAL)", "usuarioEjecutor")
                    .ColumnMappings.Add("NOMBRE CLIENTE", "nombre")
                    .ColumnMappings.Add("PERSONA AUTORIZADA (OPCIONAL)", "nombreAutorizado")
                    .ColumnMappings.Add("IDENTIFICACION CLIENTE (OPCIONAL)", "identicacion")
                    .ColumnMappings.Add("CIUDAD", "Ciudad")
                    .ColumnMappings.Add("DEPARTAMENTO", "Departamento")
                    .ColumnMappings.Add("BARRIO (OPCIONAL)", "barrio")
                    .ColumnMappings.Add("DIRECCION", "direccion")
                    .ColumnMappings.Add("TELEFONO", "telefono")
                    .ColumnMappings.Add("TIPO DE TELEFONO", "tipoTelefono")
                    .ColumnMappings.Add("FECHA DE ASIGNACION", "fechaAsignacion")
                    .ColumnMappings.Add("CLIENTE VIP (S/N)", "clienteVIP")
                    .ColumnMappings.Add("PLAN ACTUAL (OPCIONAL)", "planActual")
                    .ColumnMappings.Add("OBSERVACIONES (OPCIONAL)", "observacion")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtInformacionGeneral)
                End With
                .iniciarTransaccion()
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                '.ejecutarScalar("RegistroMasivoServicioMensajeria", CommandType.StoredProcedure)
                .TiempoEsperaComando = 0
                dt = .ejecutarDataTable("RegistroMasivoServicioMensajeria", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)

                If resul = 0 Then
                    .abortarTransaccion()
                    resultado = 0
                    Return dt
                    Exit Function
                Else
                    resultado = 1
                    .confirmarTransaccion()
                    Return dt
                End If

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Function
End Class
