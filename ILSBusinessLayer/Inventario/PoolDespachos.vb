Imports LMDataAccessLayer

Public Class PoolDespachos

#Region "Atributos (Campos)"

    Private _dbManager As New LMDataAccess

#End Region


#Region "Métodos Públicos"

    Public Function ConsultarEstado() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                .iniciarTransaccion()
                dtDatos = .ejecutarDataTable("ConsultarEstado", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            If _dbManager IsNot Nothing AndAlso _dbManager.estadoTransaccional Then _dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class
