Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class TipoReproceso

#Region "Atributos (Campos)"

    Private _idTipo As Byte
    Private _descripcion As String
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _descripcion = ""
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdTipo() As Byte
        Get
            Return _idTipo
        End Get
        Set(ByVal value As Byte)
            _idTipo = value
        End Set
    End Property

    Public Property Descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso


        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso


        Return resultado
    End Function

#End Region

End Class
