Imports ILSBusinessLayer
Imports System.Web
Imports System.Reflection

Public Class wsCobroFabricante

#Region "Atributos"
    Private _idcobro As Integer
    Private _fabricante As String
    Private _archivo As Byte()
    Private _nombreArchivo As String
    Private _usuario As String
    Private _observacion As String
    Private _objDatos As New wsSerialCobroFabricanteColeccion
#End Region

#Region "Propiedades"

    Public Property IdCobro As Integer
        Get
            Return _idcobro
        End Get
        Set(value As Integer)
            _idcobro = value
        End Set
    End Property

    Public Property fabricante As String
        Get
            Return _fabricante
        End Get
        Set(value As String)
            _fabricante = value
        End Set
    End Property

    Public Property Archivo As Byte()
        Get
            Return _archivo
        End Get
        Set(value As Byte())
            _archivo = value
        End Set
    End Property

    Public Property NombreArchivo As String
        Get
            Return _nombreArchivo
        End Get
        Set(value As String)
            _nombreArchivo = value
        End Set
    End Property

    Public Property usuario As String
        Get
            Return _usuario
        End Get
        Set(value As String)
            _usuario = value
        End Set
    End Property

    Public Property Observacion As String
        Get
            Return _observacion
        End Get
        Set(value As String)
            _observacion = value
        End Set
    End Property

    Public Property ObjDatos As wsSerialCobroFabricanteColeccion
        Get
            Return _objDatos
        End Get
        Set(value As wsSerialCobroFabricanteColeccion)
            _objDatos = value
        End Set
    End Property

#End Region

End Class
