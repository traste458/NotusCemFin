﻿Imports ILSBusinessLayer
Imports System.Web

Public Class wsSerialDevolucion

#Region "Atributos"
    Private _serial As String
    Private _estadoReparacion As String
    Private _serialCambio As String
    Private _ods As String
    Private _material As String
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

    Public Property EstadoReparacion As String
        Get
            Return _estadoReparacion
        End Get
        Set(value As String)
            _estadoReparacion = value
        End Set
    End Property

    Public Property SerialCambio As String
        Get
            Return _estadoReparacion
        End Get
        Set(value As String)
            _estadoReparacion = value
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

    Public Property Material As String
        Get
            Return _material
        End Get
        Set(value As String)
            _material = value
        End Set
    End Property

#End Region

End Class
