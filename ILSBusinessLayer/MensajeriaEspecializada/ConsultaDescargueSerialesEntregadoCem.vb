Imports LMDataAccessLayer

Public Class ConsultaDescargueSerialesEntregadoCem
    Public Function CargarVL06G(ByVal dtVL06G As DataTable, ByVal idUsuario As Integer, ByRef resultado As Int32) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        dtVL06G.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .ejecutarNonQuery("EliminaRegistroTempVL06G", CommandType.StoredProcedure)
                .iniciarTransaccion()
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TempVL06G"
                    .ColumnMappings.Add("Entrega", "Entrega")
                    .ColumnMappings.Add("Material", "Material")
                    .ColumnMappings.Add("Denominacion", "Denominacion")
                    .ColumnMappings.Add("Doccompra", "Doccompra")
                    .ColumnMappings.Add("Salmcias", "Salmcias")
                    .ColumnMappings.Add("Cantidadentrega", "Cantidadentrega")
                    .ColumnMappings.Add("idusuario", "idusuario")
                    .WriteToServer(dtVL06G)
                End With

                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario

                End With
                dt = .ejecutarDataTable("ConsultaResultadoTempVL06G", CommandType.StoredProcedure)
                .confirmarTransaccion()
                Return dt

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Function

End Class
