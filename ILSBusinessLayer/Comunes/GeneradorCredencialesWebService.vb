Imports System.Net
Imports LMDataAccessLayer

Public Class GeneradorCredencialesWebService

#Region "Atributos (Campos)"
    Private _credenciales As NetworkCredential
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal usuario As String, ByVal password As String)
        MyBase.New()
        _credenciales = New NetworkCredential(usuario, password)
    End Sub

#End Region

#Region "Propiedades"

    Public ReadOnly Property Credenciales() As NetworkCredential
        Get
            If _credenciales Is Nothing Then InicializarCredenciales()
            Return _credenciales
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub InicializarCredenciales()
        Dim dbManager As New LMDataAccess
        Dim credenciales As String = ""

        Try
            With dbManager
                .ejecutarReader("ObtenerCredencialesSapWebServices", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        credenciales = .Reader("configKeyValue").ToString
                    End If
                    .Reader.Close()
                End If
            End With
            _credenciales = New NetworkCredential
            If credenciales.Trim.Length > 0 Then
                Dim arrCredencial() As String = credenciales.Split("|")
                If arrCredencial.GetUpperBound(0) >= 1 Then
                    _credenciales.UserName = arrCredencial(0)
                    _credenciales.Password = arrCredencial(1)
                End If
            End If
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
