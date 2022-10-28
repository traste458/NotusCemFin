Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idServicioMensajeria As Integer
    Private _idTipoServicio As Integer
    Private _material As String
    Private _descripcionMaterial As String
    Private _idProducto As Integer
    Private _idTipoProducto As Integer
    Private _idMaterialServicio As Integer
    Private _cantidadLeida As Integer
    Private _cantidadCambio As Integer
    Private _cantidadDisponible As Integer
    Private _idUsuarioRegistra As Integer
    Private _cantidad As Integer
    Private _fechaRegistro As Date
    Private _tieneDisponibilidad As Boolean
    Private _fechaDevolucion As Date
    Private _esSerializado As Boolean
    Private _registrado As Boolean
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idServicio As Integer)
        Me.New()
        _idServicioMensajeria = idServicio
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdServicioMensajeria() As Integer
        Get
            Return _idServicioMensajeria
        End Get
        Set(ByVal value As Integer)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property Material() As String
        Get
            Return _material
        End Get
        Set(ByVal value As String)
            _material = value
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

    Public Property IdTipoServicio() As Integer
        Get
            Return _idTipoServicio
        End Get
        Set(ByVal value As Integer)
            _idTipoServicio = value
        End Set
    End Property

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property IdMaterialServicio() As Integer
        Get
            Return _idMaterialServicio
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idMaterialServicio = value
        End Set
    End Property

    Public Property DescripcionMaterial() As String
        Get
            Return _descripcionMaterial
        End Get
        Set(ByVal value As String)
            _descripcionMaterial = value
        End Set
    End Property

    Public Property Cantidad() As Integer
        Get
            Return _cantidad
        End Get
        Set(ByVal value As Integer)
            _cantidad = value
        End Set
    End Property

    Public Property CantidadDisponible() As Integer
        Get
            Return _cantidadDisponible
        End Get
        Protected Friend Set(ByVal value As Integer)
            _cantidadDisponible = value
        End Set
    End Property

    Public Property CantidadLeida() As Integer
        Get
            Return _cantidadLeida
        End Get
        Protected Friend Set(ByVal value As Integer)
            _cantidadLeida = value
        End Set
    End Property

    Public Property FechaDevolucion As Date
        Get
            Return _fechaDevolucion
        End Get
        Protected Friend Set(value As Date)
            _fechaDevolucion = value
        End Set
    End Property

    Public Property EsSerializado As Boolean
        Get
            Return _esSerializado
        End Get
        Protected Friend Set(value As Boolean)
            _esSerializado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miDetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion As Type = GetType(DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion)
        Dim pInfo As PropertyInfo

        For Each pInfo In miDetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion.GetProperties
            If pInfo.PropertyType.Namespace = "System" Then
                With dtAux
                    .Columns.Add(pInfo.Name, pInfo.PropertyType)
                End With
            End If
        Next

        Return dtAux
    End Function

#End Region

#Region "Métodos Públicos"



    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = Me._idServicioMensajeria
                If Me._idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = Me._idTipoServicio
                If Me._material <> String.Empty Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = Me._material
                If Me._idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = Me._idProducto
                If Me._idTipoProducto > 0 Then .SqlParametros.Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto

                .ejecutarReader("ObtenerDetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim elDetalle As DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion

                    While .Reader.Read
                        elDetalle = New DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion
                        elDetalle.CargarResultadoConsulta(.Reader)
                        _cargado = True
                        Me.InnerList.Add(elDetalle)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idMaterialServicio").ToString, _idMaterialServicio)
                Integer.TryParse(reader("idTipoServicio").ToString, _idTipoServicio)
                _material = reader("material").ToString
                _descripcionMaterial = reader("descripcionMaterial").ToString
                Integer.TryParse(reader("cantidad"), _cantidad)
                Integer.TryParse(reader("cantidadLeida"), _cantidadLeida)
                Integer.TryParse(reader("cantidadCambio"), _cantidadCambio)
                Integer.TryParse(reader("cantidadDisponible"), _cantidadDisponible)
                Integer.TryParse(reader("idUsuarioRegistra"), _idUsuarioRegistra)
                _tieneDisponibilidad = CBool(_cantidadDisponible)
                _fechaRegistro = CDate(reader("fechaRegistro"))
                Integer.TryParse(reader("idProducto"), _idProducto)
                Integer.TryParse(reader("idTipoProducto"), _idTipoProducto)
                If Not IsDBNull(reader("fechaDevolucion")) Then _fechaDevolucion = CDate(reader("fechaDevolucion"))
                If Not IsDBNull(reader("esSerializado")) Then _esSerializado = CBool(reader("esSerializado"))
                _registrado = True
            End If
        End If

    End Sub

#End Region

End Class
