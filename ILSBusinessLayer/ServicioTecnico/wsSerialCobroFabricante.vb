Imports ILSBusinessLayer
Imports System.Web

Public Class wsSerialCobroFabricante

#Region "Atributos"
    Private _serial As String
    Private _ods As String
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

    Public Property Ods As String
        Get
            Return _ods
        End Get
        Set(value As String)
            _ods = value
        End Set
    End Property

#End Region

End Class
