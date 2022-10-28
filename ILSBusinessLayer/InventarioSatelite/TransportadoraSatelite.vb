Imports LMDataAccessLayer

Public Class TransportadoraSatelite
    Public Property IdUsuario As Integer
    Public Property resultado As ResultadoProceso

    Public Shared Function ObtenerTransportadoras() As DataTable
        Dim db As New LMDataAccess

        Return db.EjecutarDataTable("ObtenerTransportadoraSatelite", CommandType.StoredProcedure)
    End Function

    Public Function CargarAsignacionEstadoGuias(dtGuias As DataTable) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As New DataTable
        resultado = New ResultadoProceso

        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .EjecutarNonQuery("EliminarTransitoriaAsignacionEstadoGuias", CommandType.StoredProcedure)
                .TiempoEsperaComando = 0
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaAsignacionEstadoGuias"
                    .ColumnMappings.Add("fila", "fila")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .ColumnMappings.Add("GUIA", "guia")
                    .ColumnMappings.Add("ESTADO", "estado")
                    .ColumnMappings.Add("FECHA", "fecha")
                    .ColumnMappings.Add("NOVEDAD", "novedad")
                    .ColumnMappings.Add("ACLARACION", "aclaracion")
                    .WriteToServer(dtGuias)
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .EjecutarDataTable("ValidarAsignacionEstadoGuias", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado.EstablecerMensajeYValor(0, "El archivo se proceso de forma correcta")
                    .ConfirmarTransaccion()
                    Return dt
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "Se presentaron errores en el cargue del archivo")
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
        Return dt
    End Function
End Class
