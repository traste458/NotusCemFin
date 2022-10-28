Imports ILSBusinessLayer.Enumerados

Namespace OMS
    Public Class OrdenTrabajo
        Public Const ID_ENTIDAD = 11

#Region "Variables"
        Private _idOrden As Long
        Private _codigo As String
        Private _idInstruccion As Long
        Private _linea As Integer
        Private _cantidadPedida As Long
        Private _cantidadLeida As Long
        Private _idCreador As Long
        Private _fechaCreacion As Date
        Private _fechaFinalizacion As Date
        Private _observacion As String
        Private _idEstado As Integer
        Private _leerSimSuelta As Boolean
        Private _revisada As Boolean
        Private _idModificador As Integer
        Private _unidadesCaja As Long
        Private _cajasEstiba As Long
        Private _idOperador As Integer
        Private _justificaCambioEstado As Boolean = False
        Private _crearEnvio As Boolean = False
        Private _idUsuarioCambio As Long
        Private _justificacion As String
        Private _contieneSeriales As Boolean
        Private _idOrdenEnvioLectura As Long
        Private _idClasificacionInstruccion As Integer
        Private _requierePin As Boolean
        Private _leerDupla As Enumerados.SiNo
        Private _materialSim As String
        Private _tieneCertificadoHomologacion As Boolean
        Private _cobrar As Enumerados.SiNo
#End Region

#Region "Constructores"

        Sub New()
            MyBase.New()
            _revisada = 1
            _leerDupla = SiNo.NoEstablecido
            _cobrar = SiNo.NoEstablecido
        End Sub

        Sub New(ByVal idOrden As Long)
            Me.New()
            Me.CargarDatos(idOrden)
        End Sub

#End Region

#Region "Propiedades"

        Public Property RequierePin() As Boolean
            Get
                Return _requierePin
            End Get
            Set(ByVal value As Boolean)
                _requierePin = value
            End Set
        End Property

        Public ReadOnly Property IdOrden() As Long
            Get
                Return _idOrden
            End Get

        End Property

        Public Property Codigo() As String
            Get
                Return _codigo
            End Get
            Set(ByVal value As String)
                _codigo = value
            End Set
        End Property

        Public Property IdModificador() As Integer
            Get
                Return _idModificador
            End Get
            Set(ByVal value As Integer)
                _idModificador = value
            End Set
        End Property

        Public Property IdInstruccion() As Long
            Get
                Return _idInstruccion
            End Get
            Set(ByVal value As Long)
                _idInstruccion = value
            End Set
        End Property

        Public Property Linea() As Integer
            Get
                Return _linea
            End Get
            Set(ByVal value As Integer)
                _linea = value
            End Set
        End Property

        Public Property CantidadPedida() As Long
            Get
                Return _cantidadPedida
            End Get
            Set(ByVal value As Long)
                _cantidadPedida = value
            End Set
        End Property

        Public Property CantidadLeida() As Long
            Get
                Return _cantidadLeida
            End Get
            Set(ByVal value As Long)
                _cantidadLeida = value
            End Set
        End Property

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
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

        Public Property FechaFinalizacion() As Date
            Get
                Return _fechaFinalizacion
            End Get
            Set(ByVal value As Date)
                _fechaFinalizacion = value
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

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property LeerSimSuelta() As Boolean
            Get
                Return _leerSimSuelta
            End Get
            Set(ByVal value As Boolean)
                _leerSimSuelta = value
            End Set
        End Property

        Public Property CajasEstiba() As Integer
            Get
                Return _cajasEstiba
            End Get
            Set(ByVal value As Integer)
                _cajasEstiba = value
            End Set
        End Property

        Public Property IdOperador() As Integer
            Get
                Return _idOperador
            End Get
            Set(ByVal value As Integer)
                _idOperador = value
            End Set
        End Property
        Public Property UnidadesCaja() As Long
            Get
                Return _unidadesCaja
            End Get
            Set(ByVal value As Long)
                _unidadesCaja = value
            End Set
        End Property

        Public Property Revisada() As Boolean
            Get
                Return _revisada
            End Get
            Set(ByVal value As Boolean)
                _revisada = value
            End Set
        End Property

        Public Property JustificaCambioEstado() As Boolean
            Get
                Return _justificaCambioEstado
            End Get
            Set(ByVal value As Boolean)
                _justificaCambioEstado = value
            End Set
        End Property

        Public Property CrearEnvio() As Boolean
            Get
                Return _crearEnvio
            End Get
            Set(ByVal value As Boolean)
                _crearEnvio = value
            End Set
        End Property

        Public Property IdUsuarioCambio() As Long
            Get
                Return _idUsuarioCambio
            End Get
            Set(ByVal value As Long)
                _idUsuarioCambio = value
            End Set
        End Property

        Public Property Justificacion() As String
            Get
                Return _justificacion
            End Get
            Set(ByVal value As String)
                _justificacion = value
            End Set
        End Property

        Public ReadOnly Property ContieneSeriales() As Boolean
            Get
                Return _contieneSeriales
            End Get
        End Property

        Public Property IdOrdenEnvioLectura() As Long
            Get
                Return _idOrdenEnvioLectura
            End Get
            Set(ByVal value As Long)
                _idOrdenEnvioLectura = value
            End Set
        End Property

        Public Property IdClasificacionInstruccion() As Long
            Get
                Return _idClasificacionInstruccion
            End Get
            Set(ByVal value As Long)
                _idClasificacionInstruccion = value
            End Set
        End Property

        Public Property LeerDupla() As SiNo
            Get
                Return _leerDupla
            End Get
            Set(ByVal value As SiNo)
                _leerDupla = value
            End Set
        End Property

        Public Property Cobrar() As SiNo
            Get
                Return _cobrar
            End Get
            Set(ByVal value As SiNo)
                _cobrar = value
            End Set
        End Property

        Public Property MaterialSim() As String
            Get
                Return _materialSim
            End Get
            Set(ByVal value As String)
                _materialSim = value
            End Set
        End Property

        Public Property TieneCertificadoHomologacion() As Boolean
            Get
                Return _tieneCertificadoHomologacion
            End Get
            Set(ByVal value As Boolean)
                _tieneCertificadoHomologacion = value
            End Set
        End Property

#End Region

#Region "Metodos"

        Public Sub Crear()
            Dim db As New LMDataAccessLayer.LMDataAccess

            Try
                With db
                    .SqlParametros.Add("@idOrden", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .agregarParametroSQL("@idInstruccion", _idInstruccion, SqlDbType.BigInt)
                    .agregarParametroSQL("@linea", _linea, SqlDbType.Int)
                    .agregarParametroSQL("@idOperador", _idOperador, SqlDbType.Int)
                    .agregarParametroSQL("@cantidadPedida", _cantidadPedida, SqlDbType.Int)
                    .agregarParametroSQL("@unidadesCaja", _unidadesCaja, SqlDbType.Int)
                    .agregarParametroSQL("@cajasEstiba", _cajasEstiba, SqlDbType.Int)
                    .agregarParametroSQL("@revisada", _revisada, SqlDbType.Bit)
                    .agregarParametroSQL("@requierePin", _requierePin, SqlDbType.Bit)
                    .agregarParametroSQL("@leerSimSuelta", _leerSimSuelta, SqlDbType.Bit)
                    If _idModificador > 0 Then .agregarParametroSQL("@idModificador", _idModificador, SqlDbType.Int)
                    .agregarParametroSQL("@idCreador", _idCreador, SqlDbType.BigInt)
                    .agregarParametroSQL("@observacion", _observacion)
                    If _leerDupla <> SiNo.NoEstablecido Then _
                        .SqlParametros.Add("@leerDupla", SqlDbType.Bit).Value = CBool(_leerDupla)
                    If _cobrar <> SiNo.NoEstablecido Then .SqlParametros.Add("@cobrar", SqlDbType.Bit).Value = CBool(_cobrar)
                    If Not String.IsNullOrEmpty(_materialSim) Then _
                        .SqlParametros.Add("@materialSim", SqlDbType.VarChar, 10).Value = _materialSim
                    .SqlParametros.Add("@tieneCertificadoHomologacion", SqlDbType.Bit).Value = _tieneCertificadoHomologacion
                    .SqlParametros.Add("@codigoError", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue

                    'el sp debe validar las cantidades
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearOrdenTrabajo", CommandType.StoredProcedure)

                    Integer.TryParse(db.SqlParametros("@idOrden").Value.ToString(), _idOrden)

                    If _idOrden > 0 Then
                        'If _idClasificacionInstruccion = 1 Then
                        '    If _crearEnvio Then
                        '        CreacionEnvioLectura(db)
                        '    Else
                        '        CreacionDetalleEnvioLectura(db)
                        '    End If
                        'Else
                        .confirmarTransaccion()
                        'End If
                    Else
                        Throw New Exception(db.SqlParametros("@codigoError").Value)
                    End If
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
                If db IsNot Nothing AndAlso db.estadoTransaccional Then db.abortarTransaccion()
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try

        End Sub

        Private Sub CreacionEnvioLectura(ByRef db As LMDataAccessLayer.LMDataAccess)
            With db.SqlParametros
                .Clear()
                .Add("@idInstruccion", SqlDbType.BigInt).Value = _idInstruccion
                .Add("@idOrdenTrabajo", SqlDbType.BigInt).Value = _idOrden
                .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador
                .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Direction = ParameterDirection.Output
                .Add("@codigoError", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
            End With
            ' Crea las ordenes de envio de lectura al envío unicamente cuando sea la primera orden de una instruccion de producción
            db.ejecutarNonQuery("CrearOrdenEnvioLectura", CommandType.StoredProcedure)
            Short.TryParse(db.SqlParametros("@idOrdenEnvioLectura").Value.ToString, _idOrdenEnvioLectura)
            If _idOrdenEnvioLectura > 0 Then
                db.confirmarTransaccion()
            Else
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(db.SqlParametros("@codigoError").Value)
            End If
        End Sub

        Private Sub CreacionDetalleEnvioLectura(ByRef db As LMDataAccessLayer.LMDataAccess)
            Dim resultado As Short
            With db.SqlParametros
                .Clear()
                .Add("@idOrdenTrabajo", SqlDbType.BigInt).Value = _idOrden
                .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = _idOrdenEnvioLectura
                .Add("@idDetalleEnvioLectura", SqlDbType.BigInt).Direction = ParameterDirection.Output
                .Add("@codigoError", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
            End With

            'relaciona la orden de trabajo con el envio de lectura activo
            db.ejecutarNonQuery("CrearDetalleEnvioLectura", CommandType.StoredProcedure)

            Short.TryParse(db.SqlParametros("@codigoError").Value.ToString, resultado)
            If resultado = 0 Then
                db.confirmarTransaccion()
            Else
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(resultado)
            End If
        End Sub

        Public Sub Eliminar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", _idOrden)
            db.ejecutarNonQuery("EliminarOrdenTrabajo", CommandType.StoredProcedure)
        End Sub

        Public Function Actualizar() As Short
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short

            Try
                With db
                    .agregarParametroSQL("@idOrden", _idOrden, SqlDbType.BigInt)
                    .agregarParametroSQL("@linea", _linea, SqlDbType.Int)
                    .agregarParametroSQL("@idOperador", _idOperador, SqlDbType.Int)
                    .agregarParametroSQL("@cantidadPedida", _cantidadPedida, SqlDbType.Int)
                    .agregarParametroSQL("@cantidadLeida", _cantidadLeida, SqlDbType.Int)
                    .agregarParametroSQL("@fechaFinalizacion", _fechaFinalizacion, SqlDbType.Date)
                    .agregarParametroSQL("@observacion", _observacion)
                    .agregarParametroSQL("@idEstado", _idEstado, SqlDbType.Int)
                    .agregarParametroSQL("@leerSimSuelta", _leerSimSuelta, SqlDbType.Bit)
                    .agregarParametroSQL("@revisada", _revisada, SqlDbType.Bit)
                    .agregarParametroSQL("@unidadesCaja", _unidadesCaja, SqlDbType.Int)
                    .agregarParametroSQL("@cajasEstiba", _cajasEstiba, SqlDbType.Int)
                    .agregarParametroSQL("@idUsuarioModificador", _idUsuarioCambio, SqlDbType.Int)
                    .agregarParametroSQL("@requierePin", _requierePin, SqlDbType.Bit)
                    If _leerDupla <> SiNo.NoEstablecido Then _
                        .SqlParametros.Add("@leerDupla", SqlDbType.Bit).Value = CBool(_leerDupla)
                    If _cobrar <> SiNo.NoEstablecido Then .SqlParametros.Add("@cobrar", SqlDbType.Bit).Value = CBool(_cobrar)
                    If Not String.IsNullOrEmpty(_materialSim) Then _
                        .SqlParametros.Add("@materialSim", SqlDbType.VarChar, 10).Value = _materialSim
                    .SqlParametros.Add("@tieneCertificadoHomologacion", SqlDbType.Bit).Value = _tieneCertificadoHomologacion
                    .SqlParametros.Add("@codigoError", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    'el sp debe validar las cantidades
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarOrdenTrabajo", CommandType.StoredProcedure)

                    Short.TryParse(.SqlParametros("@codigoError").Value.ToString, resultado)
                    If resultado = 0 Then
                        resultado = CrearJustificacionCambioEstado(db)
                        If resultado = 0 Then db.confirmarTransaccion()
                    Else
                        If db IsNot Nothing AndAlso db.estadoTransaccional Then db.abortarTransaccion()
                        Throw New Exception(resultado)
                    End If
                End With
            Catch ex As Exception
                If db IsNot Nothing AndAlso db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try

            Return resultado
        End Function

        Private Function CrearJustificacionCambioEstado(ByRef db As LMDataAccessLayer.LMDataAccess) As Short
            Dim resultado As Short
            If _justificaCambioEstado Then
                With db.SqlParametros
                    .Clear()
                    .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                    .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@idUsuarioCambio", SqlDbType.Int).Value = _idUsuarioCambio
                    .Add("@justificacion", SqlDbType.VarChar, 200).Value = _justificacion
                    .Add("@codigoError", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    db.ejecutarNonQuery("CrearJustificacionOrdenTrabajo", CommandType.StoredProcedure)

                    Short.TryParse(db.SqlParametros("@codigoError").Value.ToString, resultado)
                    If resultado <> 0 Then
                        If db IsNot Nothing AndAlso db.estadoTransaccional Then db.abortarTransaccion()
                    End If
                End With
            End If
            Return resultado
        End Function

        Public Shared Function ObtenerOrdenes(ByVal filtros As Estructuras.FiltroOrdenTrabajo) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            With filtros
                If .IdOrden > 0 Then db.agregarParametroSQL("@IdOrden", .IdOrden, SqlDbType.Int)
                If .Codigo <> "" Then db.agregarParametroSQL("@codigo", .Codigo)
                If .IdInstruccion > 0 Then db.agregarParametroSQL("@idInstruccion", .IdInstruccion, SqlDbType.BigInt)
                If .Linea > 0 Then db.agregarParametroSQL("@linea", .Linea, SqlDbType.Int)
                If .idCreador > 0 Then db.agregarParametroSQL("@idCreador", .idCreador, SqlDbType.BigInt)
                If .IdEstado > 0 Then db.agregarParametroSQL("@idEstado", .IdEstado, SqlDbType.Int)
                If .LeerSimSuelta > 0 Then db.agregarParametroSQL("@leerSimSuelta", .LeerSimSuelta, SqlDbType.Int)
                If .Revisada > 0 Then db.agregarParametroSQL("@revisada", .Revisada, SqlDbType.Bit)
                If .factura <> "" Then db.agregarParametroSQL("@factura", .factura)
                If .guia <> "" Then db.agregarParametroSQL("@guia", .guia)
                If .IdOperador > 0 Then db.agregarParametroSQL("@idOperador", .IdOperador, SqlDbType.Int)
                If .idFactura > 0 Then db.agregarParametroSQL("@idFactura", .IdOperador, SqlDbType.Int)
                If .idRegion > 0 Then db.agregarParametroSQL("@idRegion", .IdOperador, SqlDbType.Int)
                If .fechaCreacionInicial > Date.MinValue Then
                    db.agregarParametroSQL("@fechaCreacionInicial", .fechaCreacionInicial, SqlDbType.Date)
                    db.agregarParametroSQL("@fechaCreacionFinal", .fechaCreacionFinal, SqlDbType.Date)
                End If

                If .fechaCierreInicial > Date.MinValue Then
                    db.agregarParametroSQL("@fechaCierreInicial", .fechaCierreInicial, SqlDbType.Date)
                    db.agregarParametroSQL("@fechaCierreFinal", .fechaCierreFinal, SqlDbType.Date)
                End If

                If .fechaFinalizacionInicial > Date.MinValue Then
                    db.agregarParametroSQL("@fechaFinalizacionInicial", .fechaFinalizacionInicial, SqlDbType.Date)
                    db.agregarParametroSQL("@fechaFinalizacionFinal", .fechaFinalizacionFinal, SqlDbType.Date)
                End If

                db.agregarParametroSQL("@cargarActivas", .cargarActivas)
                Dim dt As DataTable = db.ejecutarDataTable("ObtenerOrdenesTrabajo", CommandType.StoredProcedure)
                Return dt
            End With
        End Function

        Private Sub CargarDatos(ByVal idOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    .agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
                    .ejecutarReader("ObtenerOrdenesTrabajo", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("idOrden").ToString, _idOrden)
                        _codigo = .Reader("codigo").ToString
                        Long.TryParse(.Reader("idInstruccion").ToString, _idInstruccion)
                        Integer.TryParse(.Reader("linea").ToString, _linea)
                        Long.TryParse(.Reader("cantidadPedida").ToString, _cantidadPedida)
                        Long.TryParse(.Reader("cantidadLeida").ToString, _cantidadLeida)
                        _idCreador = .Reader("idCreador")
                        _fechaCreacion = .Reader("fechaCreacion")
                        Date.TryParse(.Reader("fechaFinalizacion").ToString(), _fechaFinalizacion)
                        _observacion = .Reader("observacion").ToString()
                        Integer.TryParse(.Reader("idEstado").ToString, _idEstado)
                        _leerSimSuelta = CBool(.Reader("leerSimSuelta").ToString)
                        _revisada = CBool(.Reader("revisada").ToString)
                        Integer.TryParse(.Reader("unidadesCaja").ToString(), _unidadesCaja)
                        Integer.TryParse(.Reader("cajasEstiba"), _cajasEstiba)
                        Integer.TryParse(.Reader("idModificador").ToString(), _idModificador)
                        Integer.TryParse(.Reader("idOperador").ToString(), _idOperador)
                        _contieneSeriales = CBool(.Reader("contieneSeriales").ToString())
                        _requierePin = CBool(.Reader("requierePin").ToString())
                        _leerDupla = IIf(CBool(db.Reader("leerDupla")), 1, 0)
                        _cobrar = IIf(CBool(.Reader("cobrar").ToString()), 1, 0)
                        _materialSim = .Reader("materialSim").ToString
                        _tieneCertificadoHomologacion = CBool(.Reader("tieneCertificadoHomologacion"))
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Shared Function ObtenerModificadores(ByVal idClasificacion As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idClasificacion", idClasificacion, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerModificadoresOrdentrabajo", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ObtenerSerialesOSimsDeOrden(ByVal idOrden As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As New DataTable()

            With db
                .agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
                '.agregarParametroSQL("@esSerial", esSerial, SqlDbType.Int)
                dt = .ejecutarDataTable("ObtenerSerialesOSimsDeOrden", CommandType.StoredProcedure)
            End With
            Return dt
        End Function

        Public Sub Anular()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", _idOrden, SqlDbType.Int)
            db.ejecutarNonQuery("AnularOrdenTrabajo", CommandType.StoredProcedure)
        End Sub

        Public Shared Function ObtenerEstadoEdicion(ByVal idEstadoActual As Integer) As DataTable
            Dim dtAux As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            With dbManager
                .SqlParametros.Add("@idEstadoActual", SqlDbType.Int).Value = idEstadoActual
                dtAux = .ejecutarDataTable("ObtenerEstadosEdicionOrdenTrabajo", CommandType.StoredProcedure)
            End With
            Return dtAux
        End Function
#End Region

        Public Enum Estado
            Creada = 28
            Pausada = 29
            Cerrada = 30
            Anulada = 31
            EnProceso = 32
            Reabrir = 38
        End Enum

    End Class
End Namespace

