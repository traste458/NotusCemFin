Public Class TrazabilidadGuias
    Public Shared Property numeroRadicado As Decimal
    Public Shared Property pedido As String
    Public Shared Property guia As String

    Public Shared Function ObtenerInformacionTrazabilidadGuias() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If Not String.IsNullOrEmpty(numeroRadicado) Then db.SqlParametros.Add("@numeroRadicado", SqlDbType.Decimal).Value = numeroRadicado
        If Not String.IsNullOrEmpty(pedido) Then db.SqlParametros.Add("@pedido", SqlDbType.VarChar, 200).Value = pedido
        If Not String.IsNullOrEmpty(guia) Then db.SqlParametros.Add("@guia", SqlDbType.VarChar, 200).Value = guia
        Return db.EjecutarDataTable("ObtenerInformacionTrazabilidadGuias", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerMovimientosGuias() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@guia", SqlDbType.VarChar, 200).Value = guia
        Return db.EjecutarDataTable("ObtenerInformacionTrazabilidadGuiasDetalle", CommandType.StoredProcedure)
    End Function

End Class
