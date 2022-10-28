Imports LMDataAccessLayer

Public Class IntermediariasAduaneras

#Region "Atributos"
    Private _idAduaneras As Integer
#End Region

#Region "Propiedades"
    Public Property IdAduaneras() As Integer
        Get
            Return _idAduaneras
        End Get
        Set(value As Integer)
            _idAduaneras = value
        End Set
    End Property
#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos"
    Public Function ConsultarInfoAduaneras() As DataTable
        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    If _idAduaneras > 0 Then .Add("@idAdunaeras", SqlDbType.Int).Value = _idAduaneras
                End With
                dt = .ejecutarDataTable("ObtenerInfointermediariasAduaneras", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dt
    End Function
#End Region

End Class
