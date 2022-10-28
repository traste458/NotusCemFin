Imports LMDataAccessLayer

Public Class ReporteZonaServicio

#Region "Atributos (Campos)"

    Private _numeroRadicado As String
    Private _dbManager As New LMDataAccess

#End Region

#Region "Propiedades"

    Public Property NumeroRadicado() As String
        Get
            Return _numeroRadicado
        End Get
        Set(ByVal value As String)
            _numeroRadicado = value
        End Set
    End Property

    
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _numeroRadicado = ""
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ConsultarZona() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If Not String.IsNullOrEmpty(NumeroRadicado) Then _
                    .Add("@numeroRadicado", SqlDbType.VarChar, 50).Value = NumeroRadicado
                End With
                dtDatos = .ejecutarDataTable("ConsultarZonaNumeroRadicadoCEM", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class
