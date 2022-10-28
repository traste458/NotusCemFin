Imports ILSBusinessLayer
Imports System.Web

Public Class wsHistoricoServicioTecnico

#Region "Atributos"
    Private _serial As String
    Private _listaSeriales As ArrayList
#End Region

#Region "Constructores"

    Public Sub New()

    End Sub

#End Region

#Region "Propiedades"

    Public Property Serial As String
        Get
            Return _serial
        End Get
        Set(value As String)
            _serial = value
        End Set
    End Property

    Public Property ListaSeriales As ArrayList
        Get
            If _listaSeriales Is Nothing Then _listaSeriales = New ArrayList
            Return _listaSeriales
        End Get
        Set(value As ArrayList)
            _listaSeriales = value
        End Set
    End Property

#End Region

End Class
