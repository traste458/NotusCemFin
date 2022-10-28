Imports LMDataAccessLayer
Public Class ActualizarUsuarioEjecutorServicioCem

    Public Function AcualizarUsuarioEjecutor(ByVal dtUsuarioEjecutor As DataTable, ByVal idUsuario As Integer, ByRef resultado As Int32) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .ejecutarNonQuery("EliminaRegistroTrancitoriaActualizarUsuarioEjecutorServicioCem", CommandType.StoredProcedure)
                .InicilizarBulkCopy()
                .TiempoEsperaComando = 0
                With .BulkCopy
                    .DestinationTableName = "TrancitoriaActualizarUsuarioEjecutorServicioCem"
                    .ColumnMappings.Add("Fila", "Fila")
                    .ColumnMappings.Add("Radicado", "Radicado")
                    .ColumnMappings.Add("Ejecutor", "Ejecutor")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtUsuarioEjecutor)
                End With

                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .TiempoEsperaComando = 0
                dt = .EjecutarDataTable("ActualizarUsuarioEjecutorServicioCem", CommandType.StoredProcedure)
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
