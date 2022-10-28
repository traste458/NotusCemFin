Imports LMDataAccessLayer
Public Class CambioMaterialSims

#Region "Atributos"

        Private _resultado As Int32
    Private _IdUsuario As Int32
#End Region

#Region "Propiedades"

   
    Public Property Resultado() As Int32
        Get
            Return _resultado
        End Get
        Set(ByVal value As Int32)
            _resultado = value
        End Set
    End Property
    Public Property IdUsuario() As Int32
        Get
            Return _IdUsuario
        End Get
        Set(ByVal value As Int32)
            _IdUsuario = value
        End Set
    End Property

#End Region
#Region "Métodos"

    Public Function GeneraReporte(ByVal dtSeriales As DataTable) As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        dtSeriales.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), IdUsuario))
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .ejecutarNonQuery("EliminaRegistroTransitoriaCambioMaterialSims", CommandType.StoredProcedure)
                .inicilizarBulkCopy()
                .TiempoEsperaComando = 0
                With .BulkCopy
                    .DestinationTableName = "TransitoriaCambioMaterialSims"
                    .ColumnMappings.Add("Fila", "Fila")
                    .ColumnMappings.Add("Serial", "Serial")
                    .ColumnMappings.Add("Material", "Material")
                    .ColumnMappings.Add("Region", "Region")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtSeriales)
                End With
                .iniciarTransaccion()
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .ejecutarDataTable("ProcesarCambioMaterialSims", CommandType.StoredProcedure)

                Dim resu As String = .SqlParametros("@resultado").Value.ToString
                _resultado = CType(.SqlParametros("@resultado").Value.ToString, Int32)

                If _resultado <> 0 Then
                    .abortarTransaccion()
                    Return dt
                Else
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

#End Region

End Class
