Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO

Public Class ProductoDevolucionReproceso

#Region "Atributos (campos)"

    Dim _idDevolucion As Integer
    Dim _idProducto As Integer
    Dim _producto As String
    Dim _idTipoProductos As String
    Dim _tipoProducto As String
    Dim _cantidad As Integer
    Dim _idEstados As String
    Private _fechaInicio As Date
    Private _fechaFinal As Date

#End Region

#Region "Propiedades"

    Public Property IdDevolucion As Integer
        Get
            Return _idDevolucion
        End Get
        Set(value As Integer)
            _idDevolucion = value
        End Set
    End Property

    Public Property IdProducto As Integer
        Get
            Return _idProducto
        End Get
        Set(value As Integer)
            _idProducto = value
        End Set
    End Property

    Public Property Producto As String
        Get
            Return _producto
        End Get
        Set(value As String)
            _producto = value
        End Set
    End Property

    Public Property IdTipoProducto As String
        Get
            Return _idTipoProductos
        End Get
        Set(value As String)
            _idTipoProductos = value
        End Set
    End Property

    Public Property TipoProducto As String
        Get
            Return _tipoProducto
        End Get
        Set(value As String)
            _tipoProducto = value
        End Set
    End Property

    Public Property Cantidad As Integer
        Get
            Return _cantidad
        End Get
        Set(value As Integer)
            _cantidad = value
        End Set
    End Property

    Public Property Idestados As String
        Get
            Return _idEstados
        End Get
        Set(value As String)
            _idEstados = value
        End Set
    End Property

    Public Property FechaInicio As Date
        Get
            Return _fechaInicio
        End Get
        Set(value As Date)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFinal As Date
        Get
            Return _fechaFinal
        End Get
        Set(value As Date)
            _fechaFinal = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
        _producto = ""
        _tipoProducto = ""
    End Sub

#End Region

#Region "Métodos Privados"

#End Region

#Region "Métodos Públicos"

    Public Function ObtenerPoolDevoluciones() As DataTable
        Dim dtDatos As New DataTable
        Dim dbManager As New LMDataAccess

        With dbManager
            With .SqlParametros
                .Clear()
                .Add("@idTipoProductos", SqlDbType.VarChar, 200).Value = _idTipoProductos
                .Add("@idEstados", SqlDbType.VarChar, 200).Value = _idEstados
                If _idDevolucion > 0 Then .Add("@idDevolucion", SqlDbType.Int).Value = _idDevolucion
                If _idProducto > 0 Then .Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If Not _fechaInicio.Equals(Date.MinValue) Then _
                    .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                If Not _fechaFinal.Equals(Date.MinValue) Then _
                        .Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
            End With
            dtDatos = .ejecutarDataTable("ObtenerPoolDevoluciones", CommandType.StoredProcedure)
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
        Return dtDatos
    End Function

#End Region

#Region "Enumerados"

    Public Enum Estados
        Creada = 0
        Recibida = 1
        Leida = 2
        Consultada = 3
        Enviada = 4
        Reprocesada = 5
        CArgada = 6
        Entregada = 7
    End Enum

    Public Enum TipoProductos
        Telefonos = 1
        Simcard = 2
        TarjetasPrepago = 3
    End Enum

    Public Enum TipoInstruccion
        cambioRegion = 1
        cambioMaterial = 2
        cambioMaterialRegion = 3
    End Enum

    Public Enum TipoClasificacionInstruccion
        ClienteExterno = 1
        InHouse = 2
        InternaFallaProceso = 3
        InternaFallaTraslado = 4
        ExternaDevolucion = 5
        AutomaticaDevolucion = 6
    End Enum

#End Region

End Class
