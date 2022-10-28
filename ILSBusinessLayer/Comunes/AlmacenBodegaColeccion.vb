Imports LMDataAccessLayer
Imports System.Reflection

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 01/08/2014
''' Description: Colección diseñada para el manejo y administración de los datos almacenados en la tabla almacenBodega
''' </summary>
''' <remarks></remarks>
Public Class AlmacenBodegaColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _listaIdAlmacenBodega As List(Of Integer)
    Private _listaIdBodega As List(Of Integer)
    Private _listaCentro As List(Of String)
    Private _listaAlmacen As List(Of String)
    Private _listaIdClienteCEM As List(Of Integer)
    Private _esClienteCEM As Nullable(Of Boolean)
    Private _estado As Nullable(Of Boolean)
    Private _descripcion As String
    Private _idTipoBodega As Integer

    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Propiedad por defecto que instancia la clase a la cual se asigna la colección
    ''' </summary>
    ''' <param name="index"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal index As Integer) As AlmacenBodega
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As AlmacenBodega)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el listado de IdAlmacenBodega que se desean consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListaIdAlmacenBodega As List(Of Integer)
        Get
            If _listaIdAlmacenBodega Is Nothing Then _listaIdAlmacenBodega = New List(Of Integer)
            Return _listaIdAlmacenBodega
        End Get
        Set(value As List(Of Integer))
            _listaIdAlmacenBodega = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el listado de IdBodegas de los almacenes que se desean consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListaIdBodega As List(Of Integer)
        Get
            If _listaIdBodega Is Nothing Then _listaIdBodega = New List(Of Integer)
            Return _listaIdBodega
        End Get
        Set(value As List(Of Integer))
            _listaIdBodega = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el listado de centros de los almacenes que se desean consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListaCentro As List(Of String)
        Get
            If _listaCentro Is Nothing Then _listaCentro = New List(Of String)
            Return _listaCentro
        End Get
        Set(value As List(Of String))
            _listaCentro = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el listado de almacenes de los almacenes que se desean consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListaAlmacen As List(Of String)
        Get
            If _listaAlmacen Is Nothing Then _listaAlmacen = New List(Of String)
            Return _listaAlmacen
        End Get
        Set(value As List(Of String))
            _listaAlmacen = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el listado de idClienteCEM de los almacenes que se desean consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListaIdClienteCEM As List(Of Integer)
        Get
            If _listaIdClienteCEM Is Nothing Then _listaIdClienteCEM = New List(Of Integer)
            Return _listaIdClienteCEM
        End Get
        Set(value As List(Of Integer))
            _listaIdClienteCEM = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece si el almacén pertenece a cliente CEM
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EsClienteCEM As Nullable(Of Boolean)
        Get
            Return _esClienteCEM
        End Get
        Set(value As Nullable(Of Boolean))
            _esClienteCEM = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el estado (Activo - Inactivo)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Estado As Nullable(Of Boolean)
        Get
            Return _estado
        End Get
        Set(value As Nullable(Of Boolean))
            _estado = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el filtro rapido de la descripción del almacén
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Descripcion As String
        Get
            Return _descripcion
        End Get
        Set(value As String)
            _descripcion = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el tipo de bodega a la cual pertenece el almancén
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdTipoBodega As Integer
        Get
            Return _idTipoBodega
        End Get
        Set(value As Integer)
            _idTipoBodega = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

#End Region

#Region "Métodos Privados"

    ''' <summary>
    ''' Función que permite crear la estructura de la tabla, basada en las propiedades de la clase instanciada
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objAlmacenBodega As Type = GetType(AlmacenBodega)
        Dim pInfo As PropertyInfo

        For Each pInfo In objAlmacenBodega.GetProperties
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

    ''' <summary>
    ''' Método que permite Insertar elementos a la colección
    ''' </summary>
    ''' <param name="posicion"></param>
    ''' <param name="valor"></param>
    ''' <remarks></remarks>
    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As AlmacenBodega)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    ''' <summary>
    ''' Método que permite Adicionar elementos a la colección
    ''' </summary>
    ''' <param name="valor"></param>
    ''' <remarks></remarks>
    Public Sub Adicionar(ByVal valor As AlmacenBodega)
        Me.InnerList.Add(valor)
    End Sub

    ''' <summary>
    ''' Método que permite adicionar un rango de elementos a la colección
    ''' </summary>
    ''' <param name="rango"></param>
    ''' <remarks></remarks>
    Public Sub AdicionarRango(ByVal rango As AlmacenBodega)
        Me.InnerList.AddRange(rango)
    End Sub

    ''' <summary>
    ''' Método que permite generar un elemento de tipo datatable
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As AlmacenBodega

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), AlmacenBodega)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(AlmacenBodega).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    ''' <summary>
    ''' Método que permite cargar la colección con los datos obtenidos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        If _cargado Then Me.InnerList.Clear()
        With dbManager
            With .SqlParametros
                If _listaIdAlmacenBodega IsNot Nothing AndAlso _listaIdAlmacenBodega.Count > 0 Then _
                    .Add("@listaIdAlmacenBodega", SqlDbType.VarChar).Value = String.Join(",", _listaIdAlmacenBodega.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listaIdBodega IsNot Nothing AndAlso _listaIdBodega.Count > 0 Then _
                    .Add("@listaIdBodega", SqlDbType.VarChar).Value = String.Join(",", _listaIdBodega.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listaCentro IsNot Nothing AndAlso _listaCentro.Count > 0 Then _
                    .Add("@listaCentro", SqlDbType.VarChar).Value = String.Join(",", _listaCentro.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listaAlmacen IsNot Nothing AndAlso _listaAlmacen.Count > 0 Then _
                    .Add("@listaAlmacen", SqlDbType.VarChar).Value = String.Join(",", _listaAlmacen.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listaIdClienteCEM IsNot Nothing AndAlso _listaIdClienteCEM.Count > 0 Then _
                    .Add("@listaIdClienteCEM", SqlDbType.VarChar).Value = String.Join(",", _listaIdClienteCEM.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _esClienteCEM IsNot Nothing Then .Add("@esClienteCEM", SqlDbType.Bit).Value = _esClienteCEM
                If _estado IsNot Nothing Then .Add("@estado", SqlDbType.Bit).Value = _estado
                If _idTipoBodega > 0 Then .Add("@idTipoBodega", SqlDbType.Int).Value = _idTipoBodega
                If Not String.IsNullOrEmpty(_descripcion) Then .Add("@descripcion", SqlDbType.VarChar, 450).Value = _descripcion
            End With

            .ejecutarReader("ObtenerInfoAlmacenBodega", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objAlmacenBodega As AlmacenBodega
                While .Reader.Read
                    objAlmacenBodega = New AlmacenBodega()
                    objAlmacenBodega.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objAlmacenBodega)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
