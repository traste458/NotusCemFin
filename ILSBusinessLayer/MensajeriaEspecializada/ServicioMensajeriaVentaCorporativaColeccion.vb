Imports LMDataAccessLayer
Imports System.Reflection

Namespace MensajeriaEspecializada

    ''' <summary>
    ''' Author: Beltrán, Diego
    ''' Create date: 22/10/2014
    ''' Description: Colección diseñada para administrar la información de la tabla ServicioMensajeria para el tipo de servicio "Venta Corporativa"
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ServicioMensajeriaVentaCorporativaColeccion
        Inherits CollectionBase

#Region "Filtros de Búsqueda"

        Private _listaIdServicio As List(Of Long)
        Private _listaRadicado As List(Of Long)
        Private _listaIdCiudad As List(Of Integer)
        Private _listaIdEstado As List(Of Integer)
        Private _listaIdBodega As List(Of Integer)
        Private _listaIdTipoServicio As List(Of Integer)
        Private _listaIdproceso As List(Of Integer)
        Private _fechaInicial As DateTime
        Private _fechaFinal As DateTime
        Private _idUsuarioConsulta As Integer

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
        Default Public Property Item(ByVal index As Integer) As ServicioMensajeriaVentaCorporativa
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As ServicioMensajeriaVentaCorporativa)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el listado de IdServicios que se desean consultar
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
        ''' Define o establece el listado de Radicados que se desean consultar
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
        ''' Define o establece el listado de idCiuaddes por las que se desea consultar
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
        ''' Define o establece el identificador de estados por los que se desea consultar
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
        ''' Defiene o establece el listado de bodegas por las que se desea consultar
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
        ''' Define o establece el listado de tipos de servicio por los que se desea consultar
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
        ''' Define o establece el listado de idProcesos por los que se desea consultar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListaIdProceso As List(Of Integer)
            Get
                If _listaIdproceso Is Nothing Then _listaIdproceso = New List(Of Integer)
                Return _listaIdproceso
            End Get
            Set(value As List(Of Integer))
                _listaIdTipoServicio = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la fecha inicial de consulta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaInicial As DateTime
            Get
                Return _fechaInicial
            End Get
            Set(value As DateTime)
                _fechaInicial = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece la fecha final de consulta
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FechaFinal As DateTime
            Get
                Return _fechaFinal
            End Get
            Set(value As DateTime)
                _fechaFinal = value
            End Set
        End Property

        ''' <summary>
        ''' Define o establece el idUsuario que realiza la consulta, con el fin de filtrar por la bodega correspondiente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IdUsuarioConsulta As Integer
            Get
                Return _idUsuarioConsulta
            End Get
            Set(value As Integer)
                _idUsuarioConsulta = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idServicio As Integer)
            MyBase.New()
            ListaIdServicio.Add(idServicio)
            CargarDatos()
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
            Dim objServicioMensajeriaVentaCorporativa As Type = GetType(ServicioMensajeriaVentaCorporativa)
            Dim pInfo As PropertyInfo

            For Each pInfo In objServicioMensajeriaVentaCorporativa.GetProperties
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
        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ServicioMensajeriaVentaCorporativa)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        ''' <summary>
        ''' Método que permite Adicionar elementos a la colección
        ''' </summary>
        ''' <param name="valor"></param>
        ''' <remarks></remarks>
        Public Sub Adicionar(ByVal valor As ServicioMensajeriaVentaCorporativa)
            Me.InnerList.Add(valor)
        End Sub

        ''' <summary>
        ''' Método que permite adicionar un rango de elementos a la colección
        ''' </summary>
        ''' <param name="rango"></param>
        ''' <remarks></remarks>
        Public Sub AdicionarRango(ByVal rango As ServicioMensajeriaVentaCorporativa)
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
            Dim miRegistro As ServicioMensajeriaVentaCorporativa

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miRegistro = CType(Me.InnerList(index), ServicioMensajeriaVentaCorporativa)
                If miRegistro IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(ServicioMensajeriaVentaCorporativa).GetProperties
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
                        .Add("@listaCiudad", SqlDbType.VarChar).Value = String.Join(",", _listaIdCiudad.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listaIdEstado IsNot Nothing AndAlso _listaIdEstado.Count > 0 Then _
                        .Add("@listaEstados", SqlDbType.VarChar).Value = String.Join(",", _listaIdEstado.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listaIdBodega IsNot Nothing AndAlso _listaIdBodega.Count > 0 Then _
                        .Add("@listaBodega", SqlDbType.VarChar).Value = String.Join(",", _listaIdBodega.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listaIdTipoServicio IsNot Nothing AndAlso _listaIdTipoServicio.Count > 0 Then _
                        .Add("@listaTipoServicio", SqlDbType.VarChar).Value = String.Join(",", _listaIdTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listaIdproceso IsNot Nothing AndAlso _listaIdproceso.Count > 0 Then _
                        .Add("@listaProceso", SqlDbType.VarChar).Value = String.Join(",", _listaIdproceso.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _fechaInicial > Date.MinValue Then .Add("@fechaInicial", SqlDbType.DateTime).Value = _fechaInicial
                    If _fechaFinal > Date.MinValue Then .Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
                    If _idUsuarioConsulta > 0 Then .Add("@idUsuarioConsulta", SqlDbType.Int).Value = _idUsuarioConsulta
                End With

                .ejecutarReader("ObtenerInfoGeneralServicioVentaCorporativa", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    Dim objServicioMensajeriaVentaCorporativa As ServicioMensajeriaVentaCorporativa
                    While .Reader.Read
                        objServicioMensajeriaVentaCorporativa = New ServicioMensajeriaVentaCorporativa()
                        objServicioMensajeriaVentaCorporativa.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(objServicioMensajeriaVentaCorporativa)
                    End While
                    _cargado = True
                End If
            End With
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Sub

#End Region

    End Class

End Namespace
