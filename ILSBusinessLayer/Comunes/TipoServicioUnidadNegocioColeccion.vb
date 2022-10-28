Imports LMDataAccessLayer
Imports System.Reflection

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 12/08/2014
''' Description: Colección diseñada para el manejo y administración de los datos almacenados en la tabla TipoServicioUnidadNegocio
''' </summary>
''' <remarks></remarks>
Public Class TipoServicioUnidadNegocioColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _listIdTipoServicioNegocio As List(Of Integer)
    Private _listIdTipoServicio As List(Of Integer)
    Private _listIdUnidadNegocio As List(Of Integer)

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
    Default Public Property Item(ByVal index As Integer) As TipoServicioUnidadNegocio
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As TipoServicioUnidadNegocio)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el listado de identificadores de la tabla, que se desean consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListIdTipoServicioNegocio As List(Of Integer)
        Get
            If _listIdTipoServicioNegocio Is Nothing Then _listIdTipoServicioNegocio = New List(Of Integer)
            Return _listIdTipoServicioNegocio
        End Get
        Set(value As List(Of Integer))
            _listIdTipoServicioNegocio = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el listado de identificadores de tipos de servicios por los que se desea consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListIdTipoServicio As List(Of Integer)
        Get
            If _listIdTipoServicio Is Nothing Then _listIdTipoServicio = New List(Of Integer)
            Return _listIdTipoServicio
        End Get
        Set(value As List(Of Integer))
            _listIdTipoServicio = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establecce el listado de identificadores de unidades de negocio por los que se desea consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListIdUnidadNegocio As List(Of Integer)
        Get
            If _listIdUnidadNegocio Is Nothing Then _listIdUnidadNegocio = New List(Of Integer)
            Return _listIdUnidadNegocio
        End Get
        Set(value As List(Of Integer))
            _listIdUnidadNegocio = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
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
        Dim objTipoServicioUnidadNegocio As Type = GetType(TipoServicioUnidadNegocio)
        Dim pInfo As PropertyInfo

        For Each pInfo In objTipoServicioUnidadNegocio.GetProperties
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
    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoServicioUnidadNegocio)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    ''' <summary>
    ''' Método que permite Adicionar elementos a la colección
    ''' </summary>
    ''' <param name="valor"></param>
    ''' <remarks></remarks>
    Public Sub Adicionar(ByVal valor As TipoServicioUnidadNegocio)
        Me.InnerList.Add(valor)
    End Sub

    ''' <summary>
    ''' Método que permite adicionar un rango de elementos a la colección
    ''' </summary>
    ''' <param name="rango"></param>
    ''' <remarks></remarks>
    Public Sub AdicionarRango(ByVal rango As TipoServicioUnidadNegocio)
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
        Dim miRegistro As TipoServicioUnidadNegocio

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), TipoServicioUnidadNegocio)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(TipoServicioUnidadNegocio).GetProperties
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
                If _listIdTipoServicioNegocio IsNot Nothing AndAlso _listIdTipoServicioNegocio.Count > 0 Then _
                    .Add("@listIdTipoServicioNegocio", SqlDbType.VarChar).Value = String.Join(",", _listIdTipoServicioNegocio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdTipoServicio IsNot Nothing AndAlso _listIdTipoServicio.Count > 0 Then _
                    .Add("@listIdTipoServicio", SqlDbType.VarChar).Value = String.Join(",", _listIdTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdUnidadNegocio IsNot Nothing AndAlso _listIdUnidadNegocio.Count > 0 Then _
                    .Add("@listIdUnidadNegocio", SqlDbType.VarChar).Value = String.Join(",", _listIdUnidadNegocio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            End With

            .ejecutarReader("ObtenerInfoTipoServicioUnidadNegocio", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objTipoServicioUnidadNegocio As TipoServicioUnidadNegocio
                While .Reader.Read
                    objTipoServicioUnidadNegocio = New TipoServicioUnidadNegocio()
                    objTipoServicioUnidadNegocio.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objTipoServicioUnidadNegocio)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
