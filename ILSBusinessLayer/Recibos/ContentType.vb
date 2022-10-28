Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class ContentType

#Region "Atributos"
    Private _extencion As String
#End Region

#Region "Propiedades"

    Public Property Extencion() As String
        Get
            Return _extencion
        End Get
        Set(value As String)
            _extencion = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos Publicos"

    Public Function ObtenerContentType() As DataTable
        Dim dtResultado As DataTable
        Dim dbmanager As New LMDataAccess
        Try
            With dbmanager
                .SqlParametros.Clear()
                If _extencion <> "" Then .SqlParametros.Add("@extencion", SqlDbType.VarChar).Value = _extencion
                dtResultado = .ejecutarDataTable("ObtenerContentType", CommandType.StoredProcedure)
            End With
        Finally
            If dbmanager IsNot Nothing Then dbmanager.Dispose()
        End Try
        Return dtResultado
    End Function

#End Region

#Region "Estructuras"

#End Region

End Class