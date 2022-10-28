Imports LMDataAccessLayer

Public Class InfoReporteFacturacionTransporteCEM
#Region "Propiedades"
    Public Property FechaInicial As Date
    Public Property FechaFinal As Date
    Public Property IdTransportadora As Integer
    Public Property IdTipoServicio As Integer
    Public Property IdBodega As Integer
#End Region
#Region "Métodos Públicos"
    Public Function ReporteFacturacionTransporteCEMExcel(ByVal nombreArchivo As String, ByVal rutaPlantilla As String) As InfoResultado
        Dim dbManager As New LMDataAccess
        Dim resultado As New InfoResultado
        Try
            With dbManager
                .SqlParametros.Clear()
                If Me.FechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = FechaInicial
                If Me.FechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = FechaFinal
                If IdTipoServicio > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = IdTipoServicio
                If IdBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = IdBodega
                If IdTransportadora > 0 Then .SqlParametros.Add("@idAgrupacion", SqlDbType.Int).Value = IdTransportadora
                .TiempoEsperaComando = 0
                resultado = .GenerarArchivoExcel("ReporteFacturacionTransporteCEM", nombreArchivo, CommandType.StoredProcedure, rutaPlantilla, "ReporteFacturacionTransporteCEM", 3)
            End With
            Return resultado
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Function

    Public Function ReporteFacturacionTransporteCEM() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As DataTable
        Try
            With dbManager
                .SqlParametros.Clear()
                If Me.FechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = FechaInicial
                If Me.FechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = FechaFinal
                If IdTipoServicio > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = IdTipoServicio
                If IdBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = IdBodega
                If IdTransportadora > 0 Then .SqlParametros.Add("@idAgrupacion", SqlDbType.Int).Value = IdTransportadora
                .TiempoEsperaComando = 0
                dt = .EjecutarDataTable("ReporteFacturacionTransporteCEM", CommandType.StoredProcedure)
            End With
            Return dt
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Function

#End Region


End Class
