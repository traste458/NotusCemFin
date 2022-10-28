Imports LMDataAccessLayer

Public Class ActualizarFacturasServiciosdeMensajería
    Public Function ActualizarFacturaServicioMensajería(ByVal FacturaServicioMensajería As DataTable, ByVal idUsuario As Integer, ByRef resultado As Int32) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .ejecutarNonQuery("EliminarTransitoriaActualizarFacturaServicioMensajeria", CommandType.StoredProcedure)
                .iniciarTransaccion()
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaActualizarFacturaServicioMensajeria"
                    .ColumnMappings.Add("Fila", "Fila")
                    .ColumnMappings.Add("NumeroRadicado", "NumeroRadicado")
                    .ColumnMappings.Add("Factura", "Factura")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(FacturaServicioMensajería)
                End With

                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .ejecutarDataTable("ActualizarFacturaServicioMensajeria", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado = 1
                    .confirmarTransaccion()
                    Return dt
                Else
                    .abortarTransaccion()
                    resultado = 0
                    Return dt
                    Exit Function
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
