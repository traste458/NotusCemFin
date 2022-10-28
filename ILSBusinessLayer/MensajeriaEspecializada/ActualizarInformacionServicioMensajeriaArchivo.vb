Imports LMDataAccessLayer
Public Class ActualizarInformacionServicioMensajeriaArchivo

#Region "Propiedades"
   
    Public Property IdUsuario As Integer
    Public Property resultado As Integer

#End Region
#Region "Métodos Públicos"

    Public Function ActualizarInformacionServicioMensajeria(ByVal dtDatos As DataTable) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .EjecutarNonQuery("EliminarTansitoriaActualizarInformacionServicioMensajeria", CommandType.StoredProcedure)
                .TiempoEsperaComando = 0
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TansitoriaActualizarInformacionServicioMensajeria"
                    .ColumnMappings.Add("Fila", "Fila")
                    .ColumnMappings.Add("idActualizacion", "idActualizacion")
                    .ColumnMappings.Add("NumeroRadicado", "NumeroRadicado")
                    .ColumnMappings.Add("Valor1", "Valor1")
                    .ColumnMappings.Add("Valor2", "Valor2")
                    .ColumnMappings.Add("Valor3", "Valor3")
                    .ColumnMappings.Add("Valor4", "Valor4")
                    .ColumnMappings.Add("Valor5", "Valor5")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtDatos)
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .EjecutarDataTable("ValidarActualizarInformacionServicioMensajeria", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado = 1
                    .ConfirmarTransaccion()
                    Return dt
                Else
                    .AbortarTransaccion()
                    resultado = 0
                    Return dt
                    Exit Function
                End If

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Function

#End Region
End Class
