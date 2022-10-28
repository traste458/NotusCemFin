Imports ILSBusinessLayer
Imports System.Web
'Imports System.Reflection

Public Class wsDevolucion

#Region "Atributos"
    Private _idDevolucionServicioTecnico As Integer
    Private _usuario As String
    Private _objDatos As New wsSerialDevolucionColeccion
#End Region

#Region "Propiedades"

    Public Property IdDevolucionServicioTecnico As Integer
        Get
            Return _idDevolucionServicioTecnico
        End Get
        Set(value As Integer)
            _idDevolucionServicioTecnico = value
        End Set
    End Property

    Public Property Usuario As String
        Get
            Return _usuario
        End Get
        Set(value As String)
            _usuario = value
        End Set
    End Property

    Public Property ObjDatos As wsSerialDevolucionColeccion
        Get
            Return _objDatos
        End Get
        Set(value As wsSerialDevolucionColeccion)
            _objDatos = value
        End Set
    End Property

#End Region
End Class
