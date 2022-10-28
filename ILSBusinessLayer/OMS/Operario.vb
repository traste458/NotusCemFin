Imports LMDataAccessLayer
Public Class Operario
    Public Shared Function ObteneLista(Optional ByVal idLinea As Integer = 0) As DataTable
        Dim db As New LMDataAccess
        If idLinea > 0 Then db.agregarParametroSQL("linea", idLinea, SqlDbType.Int)
        Dim dt As DataTable = db.ejecutarDataTable("ObtenerOperarioLinea", CommandType.StoredProcedure)
        Return dt
    End Function
End Class
