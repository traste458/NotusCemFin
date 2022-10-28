Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class ProductoComercialColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _listIdProductoComercial As List(Of Integer)
    Private _listIdClienteExterno As List(Of Integer)
    Private _listIdProductoExterno As List(Of Integer)
    Private _idCampania As Integer
    Private _tipoProducto As Boolean

    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As ProductoComercial
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As ProductoComercial)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property ListIdProductoComercial As List(Of Integer)
        Get
            If _listIdProductoComercial Is Nothing Then _listIdProductoComercial = New List(Of Integer)
            Return _listIdProductoComercial
        End Get
        Set(value As List(Of Integer))
            _listIdProductoComercial = value
        End Set
    End Property

    Public Property ListIdClienteExterno As List(Of Integer)
        Get
            If _listIdClienteExterno Is Nothing Then _listIdClienteExterno = New List(Of Integer)
            Return _listIdClienteExterno
        End Get
        Set(value As List(Of Integer))
            _listIdClienteExterno = value
        End Set
    End Property

    Public Property ListIdProductoExterno As List(Of Integer)
        Get
            If _listIdProductoExterno Is Nothing Then _listIdProductoExterno = New List(Of Integer)
            Return _listIdProductoExterno
        End Get
        Set(value As List(Of Integer))
            _listIdProductoExterno = value
        End Set
    End Property

    Public Property IdCampania As Integer
        Get
            Return _idCampania
        End Get
        Set(value As Integer)
            _idCampania = value
        End Set
    End Property

    Public Property TipoProducto As Boolean
        Get
            Return _tipoProducto
        End Get
        Set(value As Boolean)
            _tipoProducto = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal idCampania As Integer)
        MyBase.new()
        _idCampania = idCampania
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objProductoComercial As Type = GetType(ProductoComercial)
        Dim pInfo As PropertyInfo

        For Each pInfo In objProductoComercial.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ProductoComercial)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As ProductoComercial)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As ProductoComercial)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As ProductoComercial

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), ProductoComercial)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(ProductoComercial).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        If _cargado Then Me.InnerList.Clear()
        With dbManager
            If _listIdProductoComercial IsNot Nothing AndAlso _listIdProductoComercial.Count > 0 Then _
                .SqlParametros.Add("@listIdProductoComercial", SqlDbType.VarChar).Value = String.Join(",", _listIdProductoComercial.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdClienteExterno IsNot Nothing AndAlso _listIdClienteExterno.Count > 0 Then _
                .SqlParametros.Add("@listIdClienteExterno", SqlDbType.VarChar).Value = String.Join(",", _listIdClienteExterno.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listIdProductoExterno IsNot Nothing AndAlso _listIdProductoExterno.Count > 0 Then _
                .SqlParametros.Add("@listIdProductoExterno", SqlDbType.VarChar).Value = String.Join(",", _listIdProductoExterno.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
            If _tipoProducto <> Nothing AndAlso _tipoProducto = True Then
                .SqlParametros.Add("@tipoProducto", SqlDbType.VarChar).Value = _tipoProducto
            End If

            .ejecutarReader("ObtenerProductoComercial", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objProductoComercial As ProductoComercial
                While .Reader.Read
                    objProductoComercial = New ProductoComercial
                    objProductoComercial.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objProductoComercial)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
