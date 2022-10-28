Imports LMDataAccessLayer
Public Class ReporteDeInformacionDeReprocesos




#Region "Atributos"

    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _nombreArchivo As String
    Private _nombrePlantilla As String
    Private _resultado As New InfoResultado
    Private _IdUsuario As Int32
#End Region

#Region "Propiedades"

    Public Property NombreArchivo() As String
        Get
            Return _nombreArchivo
        End Get
        Set(ByVal value As String)
            _nombreArchivo = value
        End Set
    End Property
    Public Property NombrePlantilla() As String
        Get
            Return _nombrePlantilla
        End Get
        Set(ByVal value As String)
            _nombrePlantilla = value
        End Set
    End Property
    Public Property FechaInicial() As Date
        Get
            Return _fechaInicial
        End Get
        Set(ByVal value As Date)
            _fechaInicial = value
        End Set
    End Property
    Public Property FechaFinal() As Date
        Get
            Return _fechaFinal
        End Get
        Set(ByVal value As Date)
            _fechaFinal = value
        End Set
    End Property
    Public Property Resultado() As InfoResultado
        Get
            Return _resultado
        End Get
        Set(ByVal value As InfoResultado)
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

    Public Sub GeneraReporte(ByVal dtSeriales As DataTable)
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        dtSeriales.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .ejecutarNonQuery("EliminaRegistroTransitoriaReporteDeInformacionDeReprocesos", CommandType.StoredProcedure)
                .iniciarTransaccion()
                .inicilizarBulkCopy()
                .TiempoEsperaComando = 0
                With .BulkCopy
                    .DestinationTableName = "TransitoriaReporteDeInformacionDeReprocesos"
                    .ColumnMappings.Add("Serial", "Serial")
                    If (dtSeriales.Columns("GrupoDevolucion") IsNot Nothing) Then
                        .ColumnMappings.Add("GrupoDevolucion", "GrupoDevolucion")
                    End If
                    If (dtSeriales.Columns.Contains("Revisado")) Then
                        .ColumnMappings.Add("Revisado", "Revisado")
                    End If
                    .ColumnMappings.Add("Material", "Material")
                    .ColumnMappings.Add("Region", "Region")
                    .ColumnMappings.Add("Descripcion", "Descripcion")
                    .ColumnMappings.Add("TipoOrden", "TipoOrden")
                    .ColumnMappings.Add("Fecha", "Fecha")
                    .ColumnMappings.Add("observacion", "observacion")
                    .ColumnMappings.Add("idUsuario", "idUsuario")
                    .WriteToServer(dtSeriales)
                End With
                .confirmarTransaccion()
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario

                End With
                _resultado = .GenerarArchivoExcel("ReporteDeInformacionDeReprocesos", _nombreArchivo, CommandType.StoredProcedure, _nombrePlantilla, "Reporte De Informacion De Reprocesos", 4)

              
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
    End Sub

#End Region

End Class
