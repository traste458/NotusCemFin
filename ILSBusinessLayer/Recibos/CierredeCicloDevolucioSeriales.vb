﻿Imports LMDataAccessLayer

Public Class CierredeCicloDevolucioSeriales
    Public Function CargarSerialesCierreCicloDevolucion(ByVal SerialesCierreCicloDevolucion As DataTable, ByVal idUsuario As Integer, ByRef resultado As Int32) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .ejecutarNonQuery("EliminaRegistroTempCierreCicloDevolucion", CommandType.StoredProcedure)
                .iniciarTransaccion()
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TempCierreCicloDevolucion"
                    .ColumnMappings.Add("Fila", "Fila")
                    .ColumnMappings.Add("serial", "serial")
                    .ColumnMappings.Add("fechaCierre", "fechaCierre")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(SerialesCierreCicloDevolucion)
                End With

                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .ejecutarDataTable("CierreCicloDevolucion_Seriales", CommandType.StoredProcedure)
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
