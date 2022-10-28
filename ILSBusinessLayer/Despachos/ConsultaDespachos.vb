Imports LMDataAccessLayer
Public Class ConsultaDespachos

#Region "Atributos (Campos)"
    Private _resultado As New InfoResultado
#End Region

#Region "Propiedades"
    Public Property Resultado() As InfoResultado
        Get
            Return _resultado
        End Get
        Set(ByVal value As InfoResultado)
            _resultado = value
        End Set
    End Property

#End Region

#Region "Métodos"
    Public Function ConsultarDespachos(ByVal dtDespachos As DataTable, ByVal idUsuario As Integer, ByVal nombreArchivo As String, ByVal rutaPlantilla As String, ByRef resultado As Int32) As InfoResultado
        Dim dbManager As New LMDataAccess
        dtDespachos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .ejecutarNonQuery("EliminaTransitoriaConsultaDespachos", CommandType.StoredProcedure)
                .inicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaConsultaDespachos"
                    .ColumnMappings.Add("Pedidos", "Pedidos")
                    .ColumnMappings.Add("idusuario", "idusuario")
                    .WriteToServer(dtDespachos)
                End With
                .TiempoEsperaComando = 0
                'With .SqlParametros
                '    .Clear()
                '    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario

                'End With
                _resultado = .GenerarArchivoExcel("ConsultaResultadoDespachos", nombreArchivo, CommandType.StoredProcedure, rutaPlantilla, "ConsultaSerialesDespachos", 4)

                'dt = .ejecutarDataTable("ConsultaResultadoDespachos", CommandType.StoredProcedure)
                Return _resultado

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Function
#End Region

End Class
