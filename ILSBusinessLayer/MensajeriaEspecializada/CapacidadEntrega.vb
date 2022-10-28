Imports LMDataAccessLayer

Public Class CapacidadEntrega


#Region "Propiedades"
    Public Property IdUsuario As Integer
    Public Property NombreEquipo As String
#End Region

#Region "Métodos Públicos"
    Public Function RegistrarCapacidadEntregaMasivo(ByVal dtDatos As DataTable, ByRef resultado As Int32) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager

            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .EjecutarNonQuery("EliminarTransitoriaRegistroCapacidadEntregaMasivo", CommandType.StoredProcedure)
                .InicilizarBulkCopy()
                .TiempoEsperaComando = 0

                With .BulkCopy
                    .DestinationTableName = "TransitoriaRegistroCapacidadEntregaMasivo"
                    .ColumnMappings.Add("IdUsuario", "idUsuario")
                    .ColumnMappings.Add("Fila", "fila")
                    .ColumnMappings.Add("Fecha", "fecha")
                    .ColumnMappings.Add("IdJornada", "jornada")
                    .ColumnMappings.Add("NumeroTurnos", "numeroTurnos")
                    .ColumnMappings.Add("Bodega", "idBodega")
                    .ColumnMappings.Add("Empresa", "Empresa")
                    .ColumnMappings.Add("Agrupacion", "idAgrupacion")
                    .WriteToServer(dtDatos)
                End With

                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = _IdUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .TiempoEsperaComando = 0
                .IniciarTransaccion()
                dt = .EjecutarDataTable("RegistrarCapacidadEntregaMasivo", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                .ConfirmarTransaccion()

                If resul = 0 Then
                    resultado = 0
                    Return dt
                    Exit Function
                Else
                    resultado = 1
                    Return dt
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
