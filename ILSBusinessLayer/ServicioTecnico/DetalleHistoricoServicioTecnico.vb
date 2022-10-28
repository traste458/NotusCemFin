Imports LMDataAccessLayer
Imports System.Reflection

Public Class DetalleHistoricoServicioTecnico

#Region "Atributos"

    Private _serial As String
    Private _existe As Boolean
    Private _material As String
    Private _referencia As String
    Private _registrado As Boolean

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

    Public Property Existe As Boolean
        Get
            Return _existe
        End Get
        Set(value As Boolean)
            _existe = value
        End Set
    End Property

    Public Property Material As String
        Get
            Return _material
        End Get
        Set(value As String)
            _material = value
        End Set
    End Property

    Public Property Referencia As String
        Get
            Return _referencia
        End Get
        Set(value As String)
            _referencia = value
        End Set
    End Property
 
#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                _serial = reader("serial").ToString
                _existe = reader("existe").ToString
                _material = reader("material").ToString
                _referencia = reader("referencia").ToString
                Me._registrado = True
            End If
        End If
    End Sub

#End Region

End Class
