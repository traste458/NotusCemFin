Imports LMDataAccessLayer
Imports System.Web

Public Class RutaSimpliRoute
#Region "Propiedades"

    Public Property FechaInicial As DateTime
    Public Property FechaFinal As DateTime
    Public Property Opcion As Integer
    Public Property IdRuta As Integer
    Public Property IdUsuario As Integer

#End Region

    Public Function ObtenerReporteSimpliRoute() As DataTable
        Dim dtReporte As DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If FechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = FechaInicial
                If FechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = FechaFinal
                If IdRuta > 0 Then .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = IdRuta
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                .SqlParametros.Add("@opcion", SqlDbType.Int).Value = Opcion

                dtReporte = .EjecutarDataTable("ObtenerReporteRutasSimpliRoute", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtReporte
    End Function

    Public Function ConsultarRutasCargueMasivoVisitaSimpliRoute(dtRutas As DataTable) As DataTable
        Dim dtMasivo As DataTable
        Dim dbManager As New LMDataAccess

        Try
            With dbManager

                .SqlParametros.Add("@tbIdRutas", SqlDbType.Structured).Value = dtRutas
                .SqlParametros.Add("@idUsuarioGenerador", SqlDbType.Int).Value = IdUsuario

                dtMasivo = .EjecutarDataTable("ConsultarRutasCargueMasivoVisitaSimpliRoute", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtMasivo
    End Function

End Class
