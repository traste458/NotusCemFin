Namespace OMS
    Public Class InstruccionTrabajo
        Public Const ID_ENTIDAD = 3

#Region "Variables"

        Private _observacion As String
        Private _idInstruccion As Long
        Private _idFacturaGuia As Long
        Private _idRegion As Integer
        Private _idTipoInstruccion As Integer
        Private _material As String
        Private _prioridad As Integer
        Private _cantidad As Integer
        Private _parcialEnvio As Integer
        Private _idEstado As Integer
        Private _fechaCreacion As Date
        Private _idCreador As Long
        Private _idDistribucion As Integer
        Private _idClasificacion As Short
        Private _tipoInstruccion As TipoInstruccion
        Private _region As String
        Private _estado As String
        Private _referencia As String
        Private _idUsuarioModificador As Integer
        Private _idTipoProducto As Integer
#End Region

#Region "Constructores"
        Public Sub New()
            MyBase.New()
        End Sub

        ''' <param name="IdInstruccion">Identificador unico de instruccion</param>
        Public Sub New(ByVal idInstruccion As Long)
            Me.New()
            Me.CargarDatos(idInstruccion)
        End Sub
#End Region

#Region "Propiedades"

        Public Property IdUsuarioModificador() As Long
            Get
                Return _idUsuarioModificador
            End Get
            Set(ByVal value As Long)
                _idUsuarioModificador = value
            End Set
        End Property

        Public ReadOnly Property IdInstruccion() As Long
            Get
                Return _idInstruccion
            End Get

        End Property

        Public Property Estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _region = value
            End Set
        End Property

        Public Property Region() As String
            Get
                Return _region
            End Get
            Set(ByVal value As String)
                _region = value
            End Set
        End Property

        Public Property Tipo() As TipoInstruccion
            Get
                If _tipoInstruccion Is Nothing Then _tipoInstruccion = New TipoInstruccion(_idTipoInstruccion)
                Return _tipoInstruccion
            End Get
            Set(ByVal value As TipoInstruccion)
                _tipoInstruccion = value
            End Set
        End Property

        Public Property IdFacturaGuia() As Long
            Get
                Return _idFacturaGuia
            End Get
            Set(ByVal value As Long)
                _idFacturaGuia = value
            End Set
        End Property

        Public Property IdDistribucion() As Integer
            Get
                Return _idDistribucion
            End Get
            Set(ByVal value As Integer)
                _idDistribucion = value
            End Set
        End Property

        Public Property IdRegion() As Integer
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Integer)
                _idRegion = value
            End Set
        End Property

        Public Property IdTipoInstruccion() As Long
            Get
                Return _idTipoInstruccion
            End Get
            Set(ByVal value As Long)
                _idTipoInstruccion = value
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

        Public Property Prioridad() As Integer
            Get
                Return _prioridad
            End Get
            Set(ByVal value As Integer)
                _prioridad = value
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

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
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

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
            End Set
        End Property

        Public Property Referencia() As String
            Get
                Return _referencia
            End Get
            Set(ByVal value As String)
                _referencia = value
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

        Public Property ParcialEnvio() As Integer
            Get
                Return _parcialEnvio
            End Get
            Set(ByVal value As Integer)
                _parcialEnvio = value
            End Set
        End Property

        Public Property IdClasificacion() As Integer
            Get
                Return _idClasificacion
            End Get
            Set(ByVal value As Integer)
                _idClasificacion = value
            End Set
        End Property

        Public Property IdTipoProducto() As Integer
            Get
                Return _idTipoProducto
            End Get
            Protected Friend Set(ByVal value As Integer)
                _idTipoProducto = value
            End Set
        End Property

#End Region

#Region "Metodos"

        Public Sub Crear(ByVal db As LMDataAccessLayer.LMDataAccess)
            db.SqlParametros.Clear()
            db.SqlParametros.Add("@idInstruccion", SqlDbType.BigInt).Direction = ParameterDirection.Output
            db.agregarParametroSQL("@idDistribucion", _idDistribucion, SqlDbType.Int)
            db.agregarParametroSQL("@idCreador", _idCreador, SqlDbType.Int)
            db.agregarParametroSQL("@idFacturaGuia", _idFacturaGuia, SqlDbType.BigInt)

            Me.EstablecerParametros(db)
            'el sp debe validar las cantidades
            db.ejecutarNonQuery("CrearInstruccion", CommandType.StoredProcedure)
            Long.TryParse(db.SqlParametros("@idInstruccion").Value.ToString, _idInstruccion)
            If _idInstruccion <= 0 Then
                Throw New Exception(db.SqlParametros("@codigoError").Value)
            End If
        End Sub

        Public Function CrearSubInstruccion() As Integer
            Dim idSubInstruccion As Integer
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idInstruccionPadre", _idInstruccion, SqlDbType.BigInt)
            db.SqlParametros.Add("@idInstruccion", SqlDbType.BigInt).Direction = ParameterDirection.Output
            db.agregarParametroSQL("@idCreador", _idCreador, SqlDbType.Int)
            Me.EstablecerParametros(db)
            Try
                db.iniciarTransaccion()
                db.ejecutarNonQuery("CrearSubInstruccion", CommandType.StoredProcedure)
                Dim resultado As Integer = db.SqlParametros("@codigoError").Value
                If resultado > 0 Then
                    db.abortarTransaccion()
                    Throw New Exception(db.SqlParametros("@codigoError").Value)
                End If
                db.confirmarTransaccion()
                idSubInstruccion = db.SqlParametros("@idInstruccion").Value
                Return idSubInstruccion
            Catch ex As Exception
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
            'el sp debe validar las cantidades

        End Function

        Private Sub EstablecerParametros(ByRef db As LMDataAccessLayer.LMDataAccess)
            With db
                .agregarParametroSQL("@observacion", _observacion)
                .agregarParametroSQL("@idRegion", _idRegion, SqlDbType.Int)
                .agregarParametroSQL("@idTipoInstruccion", _idTipoInstruccion, SqlDbType.Int)
                .agregarParametroSQL("@material", _material)
                .agregarParametroSQL("@prioridad", _prioridad, SqlDbType.Int)
                .agregarParametroSQL("@cantidad", _cantidad, SqlDbType.Int)
                .agregarParametroSQL("@idEstado", _idEstado, SqlDbType.Int)
                .agregarParametroSQL("@parcialEnvio", _parcialEnvio, SqlDbType.Int)
                .SqlParametros.Add("@codigoError", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
            End With
            
        End Sub

        Public Sub Actualizar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idInstruccion", _idInstruccion, SqlDbType.BigInt)
            db.agregarParametroSQL("@idFacturaGuia", _idFacturaGuia, SqlDbType.BigInt)
            db.agregarParametroSQL("@idUsuarioModificador", _idFacturaGuia, SqlDbType.BigInt)
            Me.EstablecerParametros(db)

            'el sp debe validar las cantidades
            db.ejecutarNonQuery("ActualizarInstruccion", CommandType.StoredProcedure)
            Dim codigoError As Integer = db.SqlParametros("@codigoError").Value
            If codigoError <> 0 Then Throw New Exception(db.SqlParametros("@codigoError").Value)
           
        End Sub

        Private Sub CargarDatos(ByVal idInstruccion As Long)
            Dim db As New LMDataAccessLayer.LMDataAccess

            Try
                With db
                    .agregarParametroSQL("@idInstruccion", idInstruccion, SqlDbType.BigInt)
                    .ejecutarReader("ObtenerInstrucciones", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _observacion = .Reader("observacion").ToString()
                            Long.TryParse(.Reader("idInstruccion").ToString, _idInstruccion)
                            Long.TryParse(.Reader("idFacturaGuia").ToString, _idFacturaGuia)
                            Integer.TryParse(.Reader("idRegion").ToString, _idRegion)
                            Integer.TryParse(.Reader("idTipoInstruccion").ToString, _idTipoInstruccion)
                            _material = .Reader("material").ToString
                            Integer.TryParse(.Reader("prioridad").ToString, _prioridad)
                            Integer.TryParse(.Reader("cantidad").ToString, _cantidad)
                            Integer.TryParse(.Reader("idEstado").ToString, _idEstado)
                            Dim fechaAux As Date
                            If Date.TryParse(.Reader("fechaCreacion").ToString, fechaAux) Then _fechaCreacion = fechaAux
                            Integer.TryParse(.Reader("idCreador").ToString, _idCreador)
                            _referencia = .Reader("subproducto").ToString
                            _region = db.Reader("nombreRegion").ToString
                            _estado = db.Reader("estado").ToString
                            Integer.TryParse(.Reader("idClasificacion").ToString, _idClasificacion)
                            Integer.TryParse(.Reader("idTipoProducto").ToString, _idTipoProducto)
                            Integer.TryParse(.Reader("parcialEnvio").ToString(), _parcialEnvio)
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Overloads Shared Function ObtenerInstrucciones() As DataTable
            Dim filtro As New Estructuras.FiltroInstruccion
            Dim dtDatos As DataTable = ObtenerInstrucciones(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerInstrucciones(ByVal filtros As Estructuras.FiltroInstruccion) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            With filtros
                If .idInstruccion > 0 Then db.agregarParametroSQL("@idInstruccion", .idInstruccion, SqlDbType.BigInt)
                If .idFacturaGuia > 0 Then db.agregarParametroSQL("@idFacturaGuia", .idFacturaGuia, SqlDbType.BigInt)
                If .idRegion > 0 Then db.agregarParametroSQL("@idRegion", .idRegion, SqlDbType.Int)
                If .idTipoInstruccion > 0 Then db.agregarParametroSQL("@idTipoInstruccion", .idTipoInstruccion, SqlDbType.Int)
                If .material <> Nothing Then db.agregarParametroSQL("@material", .material)
                If .prioridad > 0 Then db.agregarParametroSQL("@prioridad", .prioridad, SqlDbType.Int)
                If .cantidad > 0 Then db.agregarParametroSQL("@cantidad", .cantidad, SqlDbType.Int)
                If .idEstado > 0 Then db.agregarParametroSQL("@idEstado", .idEstado, SqlDbType.Int)
                If .idCreador > 0 Then db.agregarParametroSQL("@idCreador", .idCreador, SqlDbType.BigInt)
                If .mostrarOcultos > 0 Then
                    If .mostrarOcultos = Enumerados.EstadoBinario.Activo Then
                        db.agregarParametroSQL("@visiblePorCliente", False, SqlDbType.Bit)
                    Else
                        db.agregarParametroSQL("@visiblePorCliente", True, SqlDbType.Bit)
                    End If
                End If
                If .obtenerActivas Then db.agregarParametroSQL("@obtenerActivas", .obtenerActivas, SqlDbType.Bit)
                If .idClasificacion > 0 Then db.agregarParametroSQL("@idClasificacion", .idClasificacion, SqlDbType.Int)
                Dim dt As DataTable = db.ejecutarDataTable("ObtenerInstrucciones", CommandType.StoredProcedure)
                Return dt
            End With
        End Function

        Public Shared Function ObtenerTotalInstruccionesActivas(Optional ByVal idFacturaGuia As Integer = 0) As Integer
            Dim db As New LMDataAccessLayer.LMDataAccess
            If idFacturaGuia > 0 Then db.agregarParametroSQL("@idFacturaGuia", idFacturaGuia, SqlDbType.Int)
            Dim total As Integer = db.ejecutarScalar("ObtenerTotalinstruccionesActivas", CommandType.StoredProcedure)
            Return total
        End Function

        Public Shared Sub ModificarPrioridad(ByVal idInstruccion As Integer, ByVal incrementador As Integer, Optional ByVal PoolGeneral As Boolean = False)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idInstruccion", idInstruccion, SqlDbType.Int)
            db.agregarParametroSQL("@incrementador", incrementador, SqlDbType.Int)
            If PoolGeneral Then
                db.ejecutarNonQuery("MoverPrioridadDetallado", CommandType.StoredProcedure)
            Else
                db.ejecutarNonQuery("AumentarPrioridadInstruccion", CommandType.StoredProcedure)
            End If

        End Sub

        Public Shared Function ObtenerFacturasAsociadas() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerFacturaGuiaInstruccion", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ObtenerCantidadDisponible(ByVal idClasificacion As Integer, ByVal idFacturaGuia As Integer) As Integer
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idClasificacion", idClasificacion, SqlDbType.Int)
            db.agregarParametroSQL("@idFacturaGuia", idFacturaGuia, SqlDbType.Int)
            Dim cantidad As Integer = db.ejecutarScalar("SELECT dbo.ObtenerCantidadDisponible(@idFacturaGuia,@idClasificacion)", CommandType.Text)
            Return cantidad
        End Function

        'Public Shared Function ObtenerNotificacionInstrucciones(ByVal idFacturaGuia As Integer) As DataTable
        '    Dim db As New LMDataAccessLayer.LMDataAccess
        '    db.agregarParametroSQL("@idFacturaGuia", idFacturaGuia, SqlDbType.Int)
        '    Dim dt As DataTable = db.ejecutarDataTable("ObtenerNotificacionInstrucciones", CommandType.StoredProcedure)
        '    Return dt
        'End Function

#End Region
    End Class
End Namespace
