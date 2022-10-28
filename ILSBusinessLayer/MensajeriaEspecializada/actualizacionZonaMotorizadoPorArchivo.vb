
Imports LMDataAccessLayer
Public Class actualizacionZonaMotorizadoPorArchivo

    Public Function AcualizarZonaMotorizadoPorArchivo(ByVal dtUsuarioEjecutor As DataTable, ByVal idUsuario As Integer, ByRef resultado As Int32) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .ejecutarNonQuery("EliminaRegistroTrancitoriaActualizarZonaMotorizados", CommandType.StoredProcedure)
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TrancitoriaActualizarZonaMotorizados"
                    .ColumnMappings.Add("Fila", "Fila")
                    .ColumnMappings.Add("Identificacion", "Identificacion")
                    .ColumnMappings.Add("Ciudad", "Ciudad")
                    .ColumnMappings.Add("Departamento", "Departamento")
                    .ColumnMappings.Add("Telefono", "Telefono")
                    .ColumnMappings.Add("Placa", "Placa")
                    .ColumnMappings.Add("Zonas", "Zonas")
                    .ColumnMappings.Add("TipoServicios", "TipoServicios")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtUsuarioEjecutor)
                End With

                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .ejecutarDataTable("ActualizarZonaMotorizadoPorArchivo", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado = 0
                    Return dt
                    Exit Function
                Else
                    resultado = 1
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
