Namespace LogisticaInversa

    Public Class OrdenRecoleccion

#Region "Variables"
        Private _idOrden As Integer
        Private _idOrigen As Integer
        Private _idDestino As Integer
        Private _idTransportadora As Integer
        Private _transportadora As String
        Private _guia As String
        Private _ordenServicio As String
        Private _estado As Boolean
        Private _idCreador As Integer
        Private _fechaCreacion As Date
        Private _fechaRecoleccionTrans As Date
        Private _fechaRecoleccionPunto As Date
        Private _idConfirmadorRecoleccion As Integer
        Private _referencia As OrdenRecoleccionDetalle
        Private _accesorio As OrdenRecoleccionAccesorio
        Private _origen As Cliente
        Private _destino As Cliente
        Private _observacion As String
        Private _valorDeclarado As Long
        Private _idUsuarioPool As Integer
        Private _nombreOrigen As String
        Private _cantidad As Integer
        Private _cantidadCajas As Integer
#End Region

#Region "Propiedades"

        Public Property ValorDeclarado() As Long
            Get
                Return _valorDeclarado
            End Get
            Set(ByVal value As Long)
                _valorDeclarado = value
            End Set
        End Property

        Public Property CantidadCajas() As Integer
            Get
                Return _cantidadCajas
            End Get
            Set(value As Integer)
                _cantidadCajas = value
            End Set
        End Property

        Public Property IdOrden() As Integer
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Integer)
                _idOrden = value
            End Set
        End Property

        Public Property IdOrigen() As Integer
            Get
                Return _idOrigen
            End Get
            Set(ByVal value As Integer)
                _idOrigen = value
            End Set
        End Property

        Public Property IdDestino() As Integer
            Get
                Return _idDestino
            End Get
            Set(ByVal value As Integer)
                _idDestino = value
            End Set
        End Property

        Public Property IdTransportadora() As Integer
            Get
                Return _idTransportadora
            End Get
            Set(ByVal value As Integer)
                _idTransportadora = value
            End Set
        End Property

        Public Property Guia() As String
            Get
                Return _guia
            End Get
            Set(ByVal value As String)
                _guia = value
            End Set
        End Property

        Public Property nombreOrigen() As String
            Get
                Return _nombreOrigen
            End Get
            Set(value As String)
                _nombreOrigen = value
            End Set
        End Property

        Public Property idUsuarioPool() As String
            Get
                Return _idUsuarioPool
            End Get
            Set(value As String)
                _idUsuarioPool = value
            End Set
        End Property

        Public Property OrdenServicio() As String
            Get
                Return _ordenServicio
            End Get
            Set(ByVal value As String)
                _ordenServicio = value
            End Set
        End Property

        Public Property estado() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property

        Public Property IdCreador() As Integer
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Integer)
                _idCreador = value
            End Set
        End Property

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As Date)
                _fechaCreacion = value
            End Set
        End Property

        Public Property FechaRecoleccionTrans() As Date
            Get
                Return _fechaRecoleccionTrans
            End Get
            Set(ByVal value As Date)
                _fechaRecoleccionTrans = value
            End Set
        End Property

        Public Property FechaRecoleccionPunto() As Date
            Get
                Return _fechaRecoleccionPunto
            End Get
            Set(ByVal value As Date)
                _fechaRecoleccionPunto = value
            End Set
        End Property

        Public Property IdConfirmadorRecoleccion() As String
            Get
                Return _idConfirmadorRecoleccion
            End Get
            Set(ByVal value As String)
                _idConfirmadorRecoleccion = value
            End Set
        End Property

        Public Property Referencias() As OrdenRecoleccionDetalle
            Get
                Return _referencia
            End Get
            Set(ByVal value As OrdenRecoleccionDetalle)
                _referencia = value
            End Set
        End Property


        Public Property Accesorios() As OrdenRecoleccionAccesorio
            Get
                Return _accesorio
            End Get
            Set(ByVal value As OrdenRecoleccionAccesorio)
                _accesorio = value
            End Set
        End Property

        Public ReadOnly Property Origen() As Cliente
            Get
                If _origen Is Nothing Then
                    _origen = New Cliente(_idOrigen)
                End If
                Return _origen
            End Get

        End Property

        Public ReadOnly Property Destino() As Cliente
            Get
                If _destino Is Nothing Then
                    _destino = New Cliente(_idDestino)
                End If
                Return _destino
            End Get

        End Property

        Public Property Transportadora() As String
            Get
                Return _transportadora
            End Get
            Set(ByVal value As String)
                _transportadora = value
            End Set
        End Property

        Public Property Observacion() As String
            Get
                Return _observacion
            End Get
            Set(ByVal value As String)
                _observacion = value
            End Set
        End Property

        Public Property cantidad() As Integer
            Get
                Return _cantidad
            End Get
            Set(value As Integer)
                _cantidad = value
            End Set
        End Property

#End Region

#Region "Metodos"

        Public Sub Crear()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.SqlParametros.Add("@idOrden", SqlDbType.Int).Direction = ParameterDirection.Output
            db.SqlParametros.Add("@error", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            db.agregarParametroSQL("@idDestino", _idDestino, SqlDbType.Int)
            db.agregarParametroSQL("@idOrigen", _idOrigen, SqlDbType.Int)
            db.agregarParametroSQL("@idTransportadora", _idTransportadora, SqlDbType.Int)
            db.agregarParametroSQL("@guia", _guia)
            If _ordenServicio <> "" Then db.agregarParametroSQL("@ordenServicio", _ordenServicio)
            db.agregarParametroSQL("@observacion", _observacion)
            db.agregarParametroSQL("@idCreador", _idCreador, SqlDbType.Int)
            db.agregarParametroSQL("@valorDeclarado", _valorDeclarado, SqlDbType.BigInt)
            Try
                db.iniciarTransaccion()
                db.ejecutarNonQuery("CrearOrdenRecoleccion", CommandType.StoredProcedure)
                If db.SqlParametros("@error").Value = 0 Then
                    _idOrden = db.SqlParametros("@idOrden").Value
                    _referencia.IdOrden = _idOrden
                    _accesorio.IdOrden = _idOrden
                    _referencia.Registrar(db, _idCreador)
                    _accesorio.Registrar(db)
                    db.confirmarTransaccion()
                    calcularValorDeclarado(_idOrden)
                    'calcularValorFlete(_idOrden)
                Else
                    db.abortarTransaccion()
                    Throw New Exception(db.SqlParametros("@error").Value)
                End If

            Catch ex As Exception
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
        End Sub

        Public Sub Actualizar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", _idOrden, SqlDbType.Int)
            db.agregarParametroSQL("@idDestino", _idDestino, SqlDbType.Int)
            db.agregarParametroSQL("@idOrigen", _idOrigen, SqlDbType.Int)
            db.agregarParametroSQL("@idTransportadora", _idTransportadora, SqlDbType.Int)
            db.agregarParametroSQL("@guia", _guia)
            db.agregarParametroSQL("@ordenServicio", _ordenServicio)
            db.agregarParametroSQL("@observacion", _observacion)
            db.agregarParametroSQL("@valorDeclarado", _valorDeclarado, SqlDbType.BigInt)
            db.SqlParametros.Add("@error", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

            Try
                db.iniciarTransaccion()
                db.ejecutarNonQuery("ActualizarOrdenRecoleccion", CommandType.StoredProcedure)
                _idOrden = db.SqlParametros("@idOrden").Value
                _referencia.IdOrden = _idOrden
                _accesorio.IdOrden = _idOrden
                If db.SqlParametros("@error").Value = 0 Then
                    db.confirmarTransaccion()
                    _referencia.Actualizar(db, _idCreador)
                    _accesorio.Actualizar(db)
                Else
                    db.abortarTransaccion()
                    Throw New Exception(db.SqlParametros("@error").Value)
                End If
            Catch ex As Exception
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
        End Sub

        Private Sub CargarDatos(ByVal idOrden As Integer, ByVal idUsuarioPool As Integer, ByVal idTipoMovimiento As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
            If idUsuarioPool > 0 Then db.agregarParametroSQL("@idUsuarioPool", idUsuarioPool, SqlDbType.Int)
            If idTipoMovimiento > 0 Then db.agregarParametroSQL("@tipoMovimiento", idTipoMovimiento, SqlDbType.Int)
            db.TiempoEsperaComando = 1200
            Try
                db.ejecutarReader("ConsultarOrdenesRecoleccion", CommandType.StoredProcedure)
                With db
                    If db.Reader.Read Then
                        _idOrden = .Reader("idOrden")
                        _idOrigen = .Reader("idOrigen")
                        '_origen = .Reader("idOrigen")
                        _nombreOrigen = .Reader("origen")
                        _idDestino = .Reader("idDestino")
                        _idTransportadora = .Reader("idTransportadora")
                        _transportadora = .Reader("transportadora")
                        _guia = .Reader("guia")
                        _ordenServicio = .Reader("ordenServicio").ToString()
                        _estado = .Reader("estado")
                        _idCreador = .Reader("idCreador")
                        _fechaCreacion = .Reader("fechaCreacion")
                        _observacion = .Reader("observacion").ToString()
                        Long.TryParse(.Reader("valorDeclarado").ToString(), _valorDeclarado)
                        Date.TryParse(.Reader("fechaRecoleccionTrans").ToString(), _fechaRecoleccionTrans)
                        Date.TryParse(.Reader("fechaRecoleccionPunto").ToString(), _fechaRecoleccionPunto)
                        Integer.TryParse(.Reader("idConfirmadorRecoleccion").ToString(), _idConfirmadorRecoleccion)
                        Integer.TryParse(.Reader("cantidad").ToString(), _cantidad)
                    End If
                End With
            Catch ex As Exception
            Finally
                db.Dispose()
            End Try
        End Sub

        Private Sub CargarDatosOrden(ByVal idOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
            
            Try
                db.ejecutarReader("ConsultarOrdenRecoleccion", CommandType.StoredProcedure)
                With db
                    If db.Reader.Read Then
                        _idOrden = .Reader("idOrden")
                        _idOrigen = .Reader("idOrigen")
                        '_origen = .Reader("idOrigen")
                        _nombreOrigen = .Reader("origen")
                        _idDestino = .Reader("idDestino")
                        _idTransportadora = .Reader("idTransportadora")
                        _transportadora = .Reader("transportadora")
                        _guia = .Reader("guia")
                        _ordenServicio = .Reader("ordenServicio").ToString()
                        _estado = .Reader("estado")
                        _idCreador = .Reader("idCreador")
                        _fechaCreacion = .Reader("fechaCreacion")
                        _observacion = .Reader("observacion").ToString()
                        Long.TryParse(.Reader("valorDeclarado").ToString(), _valorDeclarado)
                        Date.TryParse(.Reader("fechaRecoleccionTrans").ToString(), _fechaRecoleccionTrans)
                        Date.TryParse(.Reader("fechaRecoleccionPunto").ToString(), _fechaRecoleccionPunto)
                        Integer.TryParse(.Reader("idConfirmadorRecoleccion").ToString(), _idConfirmadorRecoleccion)
                        Integer.TryParse(.Reader("cantidad").ToString(), _cantidad)
                    End If
                End With
            Catch ex As Exception
            Finally
                db.Dispose()
            End Try
        End Sub

        Public Shared Function ConsultarOrdenes(ByVal filtros As Estructuras.FiltroOrdenRecoleccion) As DataTable

            Dim db As New LMDataAccessLayer.LMDataAccess
            With filtros
                If .IdOrden > 0 Then
                    db.agregarParametroSQL("@idOrden", .IdOrden, SqlDbType.Int)
                Else
                    If .idUsuarioPool > 0 Then db.agregarParametroSQL("@idUsuarioPool", .idUsuarioPool, SqlDbType.Int)
                    If .IdOrigen > 0 Then db.agregarParametroSQL("@idOrigen", .IdOrigen, SqlDbType.Int)
                    If .IdDestino > 0 Then db.agregarParametroSQL("@idDestino", .IdDestino, SqlDbType.Int)
                    If .OrdenServicio <> "" Then db.agregarParametroSQL("@ordenServicio", .OrdenServicio)
                    If .Guia <> "" Then db.agregarParametroSQL("@guia", .Guia)
                    If .IdTransportadora > 0 Then db.agregarParametroSQL("@idTransportadora", .IdTransportadora)

                    If .TipoFecha > 0 Then
                        db.agregarParametroSQL("@tipoFecha", .TipoFecha, SqlDbType.Int)
                        If .FechaIncio > Date.MinValue Then
                            db.agregarParametroSQL("@fechaIncio", .FechaIncio, SqlDbType.Date)
                            db.agregarParametroSQL("@fechaFin", .FechaFin, SqlDbType.Date)
                        End If
                    End If

                End If
            End With
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarOrdenesRecoleccion", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ConsultarOrdenes(ByVal pIdOrden As String, ByVal pfechaInicial As String) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", pIdOrden, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarOrdenRecoleccion", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ConsultarOrdenRecoleccion(ByVal pIdOrden As String) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", pIdOrden, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarOrdenRecoleccion", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function validarConfiguracion(_pIdTipoProducto, _pIdProducto, _pIdOrigen, _pIdDestino, _pIdTransportadora) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            With db
                .agregarParametroSQL("@idtipoProducto", _pIdTipoProducto, SqlDbType.Int)
                .agregarParametroSQL("@ciudadOrigen", _pIdOrigen, SqlDbType.Int)
                .agregarParametroSQL("@ciudadDestino", _pIdDestino, SqlDbType.Int)
                .agregarParametroSQL("@miTransportadora", _pIdTransportadora)
                .agregarParametroSQL("@idProducto", _pIdProducto)
            End With
            Dim dt As DataTable = db.ejecutarDataTable("ValidaInformacionRutasTransportadoras", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Sub ConfirmarRecoleccionPunto()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idConfirmadorRecoleccion", _idConfirmadorRecoleccion, SqlDbType.Int)
            db.agregarParametroSQL("@idOrden", _idOrden, SqlDbType.Int)
            db.agregarParametroSQL("@observacion", _observacion)
            db.agregarParametroSQL("@cantidadCajas", _cantidadCajas)
            db.ejecutarNonQuery("ConfirmarRecoleccionPunto", CommandType.StoredProcedure)
        End Sub

        Public Shared Function ObtenerLog(ByVal idRecoleccion As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idRecoleccion", idRecoleccion, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerLogOrdenRecoleccion", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Sub calcularValorDeclarado(ByVal idOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
            Try
                db.ejecutarNonQuery("ActualizaValorDeclaradoOrdenRecoleccionTraslado", CommandType.StoredProcedure)
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub calcularValorFlete(ByVal idorden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", idorden, SqlDbType.Int)
            Try
                db.ejecutarNonQuery("CalcularValorFleteRecoleccionTraslado", CommandType.StoredProcedure)
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

#End Region

        Public Sub New(ByVal idOrden As Integer, ByVal idUsuarioPool As Integer, ByVal idTipoMovimiento As Integer)
            Me.New()
            Me.CargarDatos(idOrden, idUsuarioPool, idTipoMovimiento)
        End Sub

        Public Sub New(ByVal idOrden As Integer)
            Me.New()
            Me.CargarDatosOrden(idOrden)
        End Sub

        Public Sub New()
            _referencia = New OrdenRecoleccionDetalle
            _accesorio = New OrdenRecoleccionAccesorio
        End Sub

        Public Enum fechaFiltro
            NoEstablecido = 0
            fechaCreacion = 1
            fechaRecoleccionTrans = 2
            fechaRecoleccionPunto = 3
        End Enum

    End Class

End Namespace
