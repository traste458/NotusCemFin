Imports LMDataAccessLayer
Imports System.Reflection

Namespace MensajeriaEspecializada

    ''' <summary>
    ''' Author: Beltrán, Diego
    ''' Create date: 31/07/2014
    ''' Description: Colección diseñada para cargar los servicios de mesnajeria especializada que se asignan a tránsito y se reciben en las diferentes bodegas CEM
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TransitosMensajeriaEspecializadaColeccion
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Private _listaIdServicio As List(Of Long)
        Private _listaRadicado As List(Of Long)
        Private _listaIdCiudad As List(Of Integer)
        Private _listaIdBodega As List(Of Integer)
        Private _listaIdEstado As List(Of Integer)
        Private _listaIdTipoServicio As List(Of Integer)
        Private _fechaInicioCreacion As Date
        Private _fechaFinCreacion As Date
        Private _fechaInicioPreactivacion As Date
        Private _fechaFinPreactivacion As Date

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
        Default Public Property Item(ByVal index As Integer) As TransitosMensajeriaEspecializada
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As TransitosMensajeriaEspecializada)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la lista de IdServicios que se desean cargar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaIdServicio As List(Of Long)
            Get
                If _listaIdServicio Is Nothing Then _listaIdServicio = New List(Of Long)
                Return _listaIdServicio
            End Get
            Set(value As List(Of Long))
                _listaIdServicio = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la lista de números de radicado que se desean consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaRadicado As List(Of Long)
            Get
                If _listaRadicado Is Nothing Then _listaRadicado = New List(Of Long)
                Return _listaRadicado
            End Get
            Set(value As List(Of Long))
                _listaRadicado = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el listado de IdCiudades de los servicios que se desean consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaIdCiudad As List(Of Integer)
            Get
                If _listaIdCiudad Is Nothing Then _listaIdCiudad = New List(Of Integer)
                Return _listaIdCiudad
            End Get
            Set(value As List(Of Integer))
                _listaIdCiudad = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la lista de idBodegas destino de los servicios que se desean consultar
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
        ''' Define o establece el listado de idEstados de los servicios que se desean consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaIdEstado As List(Of Integer)
            Get
                If _listaIdEstado Is Nothing Then _listaIdEstado = New List(Of Integer)
                Return _listaIdEstado
            End Get
            Set(value As List(Of Integer))
                _listaIdEstado = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el listado de idTipoServicio de los servicios que se desean consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaIdTipoServicio As List(Of Integer)
            Get
                If _listaIdTipoServicio Is Nothing Then _listaIdTipoServicio = New List(Of Integer)
                Return _listaIdTipoServicio
            End Get
            Set(value As List(Of Integer))
                _listaIdTipoServicio = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la fecha inicial de creación
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaInicioCreacion As Date
            Get
                Return _fechaInicioCreacion
            End Get
            Set(value As Date)
                _fechaInicioCreacion = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la fecha final de creación de los servicios a consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaFinCreacion As Date
            Get
                Return _fechaFinCreacion
            End Get
            Set(value As Date)
                _fechaFinCreacion = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la fecha Inicial de preactivación de los servicios a consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaInicioPreactivacion As Date
            Get
                Return _fechaInicioPreactivacion
            End Get
            Set(value As Date)
                _fechaInicioPreactivacion = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la fecha Final de preactivación de los servicios a consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaFinPreactivacion As Date
            Get
                Return _fechaFinPreactivacion
            End Get
            Set(value As Date)
                _fechaFinPreactivacion = value
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
            Dim objTransitosMensajeriaEspecializada As Type = GetType(TransitosMensajeriaEspecializada)
            Dim pInfo As PropertyInfo

            For Each pInfo In objTransitosMensajeriaEspecializada.GetProperties
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
        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TransitosMensajeriaEspecializada)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        ''' <summary>
        ''' Método que permite Adicionar elementos a la colección
        ''' </summary>
        ''' <param name="valor"></param>
        ''' <remarks></remarks>
        Public Sub Adicionar(ByVal valor As TransitosMensajeriaEspecializada)
            Me.InnerList.Add(valor)
        End Sub

        ''' <summary>
        ''' Método que permite adicionar un rango de elementos a la colección
        ''' </summary>
        ''' <param name="rango"></param>
        ''' <remarks></remarks>
        Public Sub AdicionarRango(ByVal rango As TransitosMensajeriaEspecializada)
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
            Dim miRegistro As TransitosMensajeriaEspecializada

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), TransitosMensajeriaEspecializada)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(TransitosMensajeriaEspecializada).GetProperties
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
                    If _listaIdServicio IsNot Nothing AndAlso _listaIdServicio.Count > 0 Then _
                        .Add("@listaIdServicio", SqlDbType.VarChar).Value = String.Join(",", _listaIdServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listaRadicado IsNot Nothing AndAlso _listaRadicado.Count > 0 Then _
                        .Add("@listaRadicado", SqlDbType.VarChar).Value = String.Join(",", _listaRadicado.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listaIdCiudad IsNot Nothing AndAlso _listaIdCiudad.Count > 0 Then _
                        .Add("@listaIdCiudad", SqlDbType.VarChar).Value = String.Join(",", _listaIdCiudad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listaIdBodega IsNot Nothing AndAlso _listaIdBodega.Count > 0 Then _
                        .Add("@listaIdBodega", SqlDbType.VarChar).Value = String.Join(",", _listaIdBodega.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listaIdEstado IsNot Nothing AndAlso _listaIdEstado.Count > 0 Then _
                        .Add("@listaIdEstado", SqlDbType.VarChar).Value = String.Join(",", _listaIdEstado.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listaIdTipoServicio IsNot Nothing AndAlso _listaIdTipoServicio.Count > 0 Then _
                        .Add("@listaIdTipoServicio", SqlDbType.VarChar).Value = String.Join(",", _listaIdTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _fechaInicioCreacion > Date.MinValue Then .Add("@fechaInicioCreacion", SqlDbType.DateTime).Value = _fechaInicioCreacion
                    If _fechaFinCreacion > Date.MinValue Then .Add("@fechaFinCreacion", SqlDbType.DateTime).Value = _fechaFinCreacion
                    If _fechaInicioPreactivacion > Date.MinValue Then .Add("@fechaInicioPreactivacion", SqlDbType.DateTime).Value = _fechaInicioPreactivacion
                    If _fechaFinPreactivacion > Date.MinValue Then .Add("@fechaFinPreactivacion", SqlDbType.DateTime).Value = _fechaFinPreactivacion
                End With
                
                .ejecutarReader("ObtenerInfoTransitosMensajeriaEspecializada", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    Dim objTransitosMensajeriaEspecializada As TransitosMensajeriaEspecializada
                    While .Reader.Read
                        objTransitosMensajeriaEspecializada = New TransitosMensajeriaEspecializada()
                        objTransitosMensajeriaEspecializada.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(objTransitosMensajeriaEspecializada)
                    End While
                    _cargado = True
                End If
            End With
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Sub

#End Region

    End Class

End Namespace