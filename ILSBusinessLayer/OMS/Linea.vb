Imports LMDataAccessLayer
Public Class Linea
    Public Shared Function ObtenerLista() As DataTable
        Dim db As New LMDataAccess
        Dim dt As DataTable = db.ejecutarDataTable("ObtenerLineas", CommandType.StoredProcedure)
        Return dt
    End Function
End Class
