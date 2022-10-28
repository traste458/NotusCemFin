Imports LMDataAccessLayer
Imports System.Reflection

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 14/08/2014
''' Description: Colección diseñada para el manejo y administración de los datos almacenados en la tabla TransitoriaTransitoriaTipoServicioUnidadNegocio
''' </summary>
''' <remarks></remarks>
Public Class TransitoriaTipoServicioUnidadNegocioColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _idRegistro As List(Of Integer)
    Private _listIdTipoServicio As List(Of Integer)
    Private _idUsuario As List(Of Integer)

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
    Default Public Property Item(ByVal index As Integer) As TransitoriaTipoServicioUnidadNegocio
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As TransitoriaTipoServicioUnidadNegocio)
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
    Public Property ListIdRegistro As List(Of Integer)
        Get
            If _idRegistro Is Nothing Then _idRegistro = New List(Of Integer)
            Return _idRegistro
        End Get
        Set(value As List(Of Integer))
            _idRegistro = value
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
    ''' Define o establecce el listado de identificadores usuarios por los que se desea consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ListIdUsuario As List(Of Integer)
        Get
            If _idUsuario Is Nothing Then _idUsuario = New List(Of Integer)
            Return _idUsuario
        End Get
        Set(value As List(Of Integer))
            _idUsuario = value
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
        Dim objTransitoriaTipoServicioUnidadNegocio As Type = GetType(TransitoriaTipoServicioUnidadNegocio)
        Dim pInfo As PropertyInfo

        For Each pInfo In objTransitoriaTipoServicioUnidadNegocio.GetProperties
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
    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TransitoriaTipoServicioUnidadNegocio)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    ''' <summary>
    ''' Método que permite Adicionar elementos a la colección
    ''' </summary>
    ''' <param name="valor"></param>
    ''' <remarks></remarks>
    Public Sub Adicionar(ByVal valor As TransitoriaTipoServicioUnidadNegocio)
        Me.InnerList.Add(valor)
    End Sub

    ''' <summary>
    ''' Método que permite adicionar un rango de elementos a la colección
    ''' </summary>
    ''' <param name="rango"></param>
    ''' <remarks></remarks>
    Public Sub AdicionarRango(ByVal rango As TransitoriaTipoServicioUnidadNegocio)
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
        Dim miRegistro As TransitoriaTipoServicioUnidadNegocio

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), TransitoriaTipoServicioUnidadNegocio)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(TransitoriaTipoServicioUnidadNegocio).GetProperties
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
                If _idRegistro IsNot Nothing AndAlso _idRegistro.Count > 0 Then _
                    .Add("@idRegistro", SqlDbType.VarChar).Value = String.Join(",", _idRegistro.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _listIdTipoServicio IsNot Nothing AndAlso _listIdTipoServicio.Count > 0 Then _
                    .Add("@listIdTipoServicio", SqlDbType.VarChar).Value = String.Join(",", _listIdTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                If _idUsuario IsNot Nothing AndAlso _idUsuario.Count > 0 Then _
                    .Add("@idUsuario", SqlDbType.VarChar).Value = String.Join(",", _idUsuario.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            End With

            .ejecutarReader("ObtenerInfoTransitoriaTipoServicioUnidadNegocio", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objTransitoriaTipoServicioUnidadNegocio As TransitoriaTipoServicioUnidadNegocio
                While .Reader.Read
                    objTransitoriaTipoServicioUnidadNegocio = New TransitoriaTipoServicioUnidadNegocio()
                    objTransitoriaTipoServicioUnidadNegocio.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objTransitoriaTipoServicioUnidadNegocio)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class