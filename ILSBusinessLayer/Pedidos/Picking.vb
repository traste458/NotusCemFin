Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Data.SqlClient

Namespace Pedidos

    Public Class Picking

        Private _idPickingList As Integer
        Private _idPedido As Integer
        Private _listaPedido As ArrayList
        Private _idTipoPedido As Integer
        Private _idEstado As Short
        Private _estado As String
        Private _fechaCreacion As String
        Private _fechaAtencionBodega As String
        Private _idUsuarioCreacion As Integer
        Private _usuarioCreacion As String
        Private _idUsuarioAtencion As Integer
        Private _usuarioAtencion As String
        Private _detallePicking As New DetallePicking
        Private _dtLogErrorRegistro As DataTable
        Private _idSession As String
        Private _listaPicking As ArrayList
        Private _ResultadoTransaccion As New ResultadoProceso


        Public Property FechaAtencionBodega() As String
            Get
                Return _fechaAtencionBodega

            End Get
            Set(ByVal value As String)
                _fechaAtencionBodega = value
            End Set
        End Property

        Public Property FechaCreacion() As String
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As String)
                _fechaCreacion = value
            End Set
        End Property

        Public Property IdEstado() As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        Public ReadOnly Property Estado() As String
            Get
                Return _estado
            End Get
        End Property


        Public Property IdPedido() As Integer
            Get
                Return _idPedido
            End Get
            Set(ByVal value As Integer)
                _idPedido = value
            End Set
        End Property

        Public Property IdTipoPedido() As Integer
            Get
                Return _idTipoPedido
            End Get
            Set(ByVal value As Integer)
                _idTipoPedido = value
            End Set
        End Property

        Public Property IdPickingList() As Integer
            Get
                Return _idPickingList
            End Get
            Set(ByVal value As Integer)
                _idPickingList = value
            End Set
        End Property

        Public Property IdUsuarioAtencion() As Integer
            Get
                Return _idUsuarioAtencion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioAtencion = value
            End Set
        End Property

        Public ReadOnly Property UsuarioAtencion() As String
            Get
                Return _usuarioAtencion
            End Get

        End Property

        Public Property IdUsuarioCreacion() As Integer
            Get
                Return _idUsuarioCreacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCreacion = value
            End Set
        End Property

        Public ReadOnly Property UsuarioCreacion() As String
            Get
                Return _usuarioCreacion
            End Get
        End Property

        Public Property IdSession() As String
            Get
                Return _idSession

            End Get
            Set(ByVal value As String)
                _idSession = value
            End Set
        End Property

        Public ReadOnly Property Detalle() As DetallePicking
            Get
                If _detallePicking Is Nothing Then ObtenerDetalle()
                Return _detallePicking
            End Get
        End Property

        Public ReadOnly Property LogErrorRegistro() As DataTable
            Get
                Return _dtLogErrorRegistro
            End Get
        End Property

        Public ReadOnly Property ListaPedido() As ArrayList
            Get
                Return _listaPedido
            End Get
        End Property

        Public ReadOnly Property ListaPicking() As ArrayList
            Get
                Return _listaPicking
            End Get
        End Property


        Public ReadOnly Property ResultadoTransaccion() As ResultadoProceso
            Get
                Return _ResultadoTransaccion
            End Get
        End Property


        Public Sub New()
            MyBase.New()
            _listaPedido = New ArrayList
            _listaPicking = New ArrayList
        End Sub

        Public Sub New(ByVal idPicking As Integer)
            ObtenerPorId(idPicking)
            _listaPedido = New ArrayList
            _listaPicking = New ArrayList
        End Sub

        Public Sub ObtenerPorId(ByVal idPicking As Integer)
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With dbManager
                    .agregarParametroSQL("@idPicking", idPicking, SqlDbType.Int)
                    .ejecutarReader("ObtenerPicking", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                        If .Reader.Read Then
                            _idPickingList = .Reader("idPickingList")
                            _idEstado = .Reader("idEstado")
                            _estado = .Reader("estado")
                            _fechaCreacion = .Reader("fechaCreacion")
                            _fechaAtencionBodega = .Reader("fechaAtencionBodega").ToString
                            _idPedido = .Reader("idPedido")
                            Integer.TryParse(.Reader("idUsuarioBodega").ToString, _idUsuarioAtencion)
                            Integer.TryParse(.Reader("idUsuarioCreacion").ToString, _idUsuarioCreacion)
                            Integer.TryParse(.Reader("idTipoPedido").ToString, _idTipoPedido)
                        End If
                    End If
                    If .Reader Is Nothing Then .Reader.Close()
                End With

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub

        Private Sub ObtenerDetalle()
            _detallePicking = New DetallePicking(_idPickingList)
        End Sub

        Public Function CrearParaDespacho() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim registrado As Boolean = False
            Dim dSResultado As New DataSet
            Dim dtPicking As New DataTable

            Try
                With dbManager
                    .TiempoEsperaComando = 600
                    .iniciarTransaccion()
                    If _listaPedido.Count > 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = Join(_listaPedido.ToArray, ",")
                    ElseIf _idPedido <> 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = _idPedido.ToString
                    Else
                        Throw New Exception("No se recibió informacion de pedido.")
                    End If
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioCreacion
                    dSResultado = .EjecutarDataSet("CrearPickingListPoolPedido", CommandType.StoredProcedure)

                    If dSResultado.Tables.Count = 0 Then
                        Throw New Exception("No se obtubo respuesta valida del proceso de creación de picking.")
                    Else
                        dtPicking = dSResultado.Tables(0)
                        _dtLogErrorRegistro = dSResultado.Tables(1)

                        If dtPicking.Rows.Count = 0 Then
                            .abortarTransaccion()
                            _ResultadoTransaccion.Valor = 1
                            _ResultadoTransaccion.Mensaje = "Se presentaron inconsistencias en la información al crear PickingList del/los pedido(s)."
                        Else
                            .confirmarTransaccion()
                            For Each dr As DataRow In dtPicking.Rows
                                _listaPicking.Add(dr("idPickingList"))
                            Next
                            registrado = True
                            If _dtLogErrorRegistro.Rows.Count = 0 Then
                                _ResultadoTransaccion.Valor = 0
                                _ResultadoTransaccion.Mensaje = "Se creó correctamente PickingList para el/los pedido(s) seleccionado(s)."
                            Else
                                _ResultadoTransaccion.Valor = 2
                                _ResultadoTransaccion.Mensaje = "Se presentaron inconsistencias en la información al crear PickingList de uno o más pedidos."
                            End If
                        End If
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                _ResultadoTransaccion.Valor = 1
                _ResultadoTransaccion.Mensaje = ex.Message
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return _ResultadoTransaccion
        End Function

        Public Function CrearParaLecturaDeCuarentena() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim registrado As Boolean = False
            Dim dSResultado As New DataSet
            Dim dtPicking As New DataTable

            Try
                With dbManager
                    .TiempoEsperaComando = 900
                    .iniciarTransaccion()
                    If _listaPedido.Count > 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = Join(_listaPedido.ToArray, ",")
                    ElseIf _idPedido <> 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = _idPedido.ToString
                    Else
                        _ResultadoTransaccion.Valor = -1
                        _ResultadoTransaccion.Mensaje = "No se recibió informacion de pedido."
                    End If
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioCreacion
                    dSResultado = .EjecutarDataSet("CrearPickingListCuarentena", CommandType.StoredProcedure)

                    dtPicking = dSResultado.Tables(0)
                    _dtLogErrorRegistro = dSResultado.Tables(1)

                    If _dtLogErrorRegistro.Rows.Count > 0 Then
                        .abortarTransaccion()
                        _ResultadoTransaccion.Valor = 1
                        _ResultadoTransaccion.Mensaje = "Se presentaron inconsistencias en la información al crear PickingList."
                    Else
                        .confirmarTransaccion()
                        _ResultadoTransaccion.Valor = 0
                        _ResultadoTransaccion.Mensaje = "Se creó correctamente PickingList para el/los pedido(s) seleccionado(s)."
                        For Each dr As DataRow In dtPicking.Rows
                            _listaPicking.Add(dr("idPickingList"))
                        Next
                        registrado = True
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.abortarTransaccion()
                _ResultadoTransaccion.Valor = 1
                _ResultadoTransaccion.Mensaje = ex.Message
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return _ResultadoTransaccion
        End Function

        Public Function CrearPickingPedidoEspecial() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim registrado As Boolean = False
            Dim dSResultado As New DataSet
            Dim dtPicking As New DataTable

            Try
                With dbManager
                    .TiempoEsperaComando = 900
                    .iniciarTransaccion()
                    If _listaPedido.Count > 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = Join(_listaPedido.ToArray, ",")
                    ElseIf _idPedido <> 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = _idPedido.ToString
                    Else
                        _ResultadoTransaccion.Valor = -1
                        _ResultadoTransaccion.Mensaje = "No se recibió informacion de pedido."
                    End If
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioCreacion
                    dSResultado = .EjecutarDataSet("CrearPickingListPedidoEspecialParaDespacho", CommandType.StoredProcedure)

                    dtPicking = dSResultado.Tables(0)
                    _dtLogErrorRegistro = dSResultado.Tables(1)

                    If _dtLogErrorRegistro.Rows.Count > 0 Then
                        .abortarTransaccion()
                        _ResultadoTransaccion.Valor = 1
                        _ResultadoTransaccion.Mensaje = "Se presentaron inconsistencias en la información al crear PickingList."
                    Else
                        .confirmarTransaccion()
                        _ResultadoTransaccion.Valor = 0
                        _ResultadoTransaccion.Mensaje = "Se creó correctamente PickingList para el/los pedido(s) seleccionado(s)."
                        For Each dr As DataRow In dtPicking.Rows
                            _listaPicking.Add(dr("idPickingList"))
                        Next
                        registrado = True
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.abortarTransaccion()
                _ResultadoTransaccion.Valor = 1
                _ResultadoTransaccion.Mensaje = ex.Message
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return _ResultadoTransaccion
        End Function

        Public Function CrearPickingServicioTecnico() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim registrado As Boolean = False
            Dim dSResultado As New DataSet
            Dim dtPicking As New DataTable

            Try
                With dbManager
                    .TiempoEsperaComando = 900
                    .iniciarTransaccion()
                    If _listaPedido.Count > 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = Join(_listaPedido.ToArray, ",")
                    ElseIf _idPedido <> 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = _idPedido.ToString
                    Else
                        _ResultadoTransaccion.Valor = -1
                        _ResultadoTransaccion.Mensaje = "No se recibió informacion de pedido."
                    End If
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioCreacion
                    dSResultado = .EjecutarDataSet("CrearPickingListPedidoServicioTecnicoParaDespacho", CommandType.StoredProcedure)

                    dtPicking = dSResultado.Tables(0)
                    _dtLogErrorRegistro = dSResultado.Tables(1)

                    If _dtLogErrorRegistro.Rows.Count > 0 Then
                        .abortarTransaccion()
                        _ResultadoTransaccion.Valor = 1
                        _ResultadoTransaccion.Mensaje = "Se presentaron inconsistencias en la información al crear PickingList."
                    Else
                        .confirmarTransaccion()
                        _ResultadoTransaccion.Valor = 0
                        _ResultadoTransaccion.Mensaje = "Se creó correctamente PickingList para el/los pedido(s) seleccionado(s)."
                        For Each dr As DataRow In dtPicking.Rows
                            _listaPicking.Add(dr("idPickingList"))
                        Next
                        registrado = True
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.abortarTransaccion()
                _ResultadoTransaccion.Valor = 1
                _ResultadoTransaccion.Mensaje = ex.Message
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return _ResultadoTransaccion
        End Function

        Public Function CrearLiberacionParaDespacho() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim registrado As Boolean = False
            Dim dSResultado As New DataSet
            Dim dtPicking As New DataTable

            Try
                With dbManager
                    .TiempoEsperaComando = 900
                    .iniciarTransaccion()
                    If _listaPedido.Count > 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = Join(_listaPedido.ToArray, ",")
                    ElseIf _idPedido <> 0 Then
                        .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = _idPedido.ToString
                    Else
                        _ResultadoTransaccion.Valor = -1
                        _ResultadoTransaccion.Mensaje = "No se recibió informacion de pedido."
                    End If
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioCreacion
                    dSResultado = .EjecutarDataSet("CrearPickingListLiberacionParaDespacho", CommandType.StoredProcedure)

                    dtPicking = dSResultado.Tables(0)
                    _dtLogErrorRegistro = dSResultado.Tables(1)

                    If _dtLogErrorRegistro.Rows.Count > 0 Then
                        .abortarTransaccion()
                        _ResultadoTransaccion.Valor = 1
                        _ResultadoTransaccion.Mensaje = "Se presentaron inconsistencias en la información al crear PickingList."
                    Else
                        .confirmarTransaccion()
                        _ResultadoTransaccion.Valor = 0
                        _ResultadoTransaccion.Mensaje = "Se creó correctamente PickingList para el/los pedido(s) seleccionado(s)."
                        For Each dr As DataRow In dtPicking.Rows
                            _listaPicking.Add(dr("idPickingList"))
                        Next
                        registrado = True
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.abortarTransaccion()
                _ResultadoTransaccion.Valor = 1
                _ResultadoTransaccion.Mensaje = ex.Message
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return _ResultadoTransaccion
        End Function

        Public Sub AgregarIdPedidoALista(ByVal idPedido As Integer)
            _listaPedido.Add(idPedido)
        End Sub
        Public Sub AgregarIdPickingALista(ByVal idPicking As Integer)
            _listaPicking.Add(idPicking)
        End Sub

#Region "ENUMS"

        Public Enum EstadoPicking
            Creado = 12
            Atendido = 14
        End Enum
#End Region

    End Class

End Namespace
