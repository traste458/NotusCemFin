Namespace LogisticaInversa
    Public Class DevolucionDetalleSerial

#Region "Variables"
        Private _id As Integer
        Private _idDevolucion As Integer
        Private _serial As String
        Private _fecha As Date
        Private _idEstado As Integer
        Private _idDetalle As Integer
        Private _idPosicion As Integer
        Private _material As String
        Private _idProducto As Integer
        Private _idClasificacion As Integer
        Private _novedades As NovedadSerial
#End Region

#Region "Propiedades"
        Public Property Novedad() As NovedadSerial
            Get
                Return _novedades
            End Get
            Set(ByVal value As NovedadSerial)
                _novedades = value
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

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
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

        Public Property IdPosicion() As Integer
            Get
                Return _idPosicion
            End Get
            Set(ByVal value As Integer)
                _idPosicion = value
            End Set
        End Property

        Public Property IdDetalle() As Integer
            Get
                Return _idDetalle
            End Get
            Set(ByVal value As Integer)
                _idDetalle = value
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

        Public Property Fecha() As Date
            Get
                Return _fecha
            End Get
            Set(ByVal value As Date)
                _fecha = value
            End Set
        End Property

        Public Property Serial() As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property

        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property IdDevolucion() As Integer
            Get
                Return _idDevolucion
            End Get
            Set(ByVal value As Integer)
                _idDevolucion = value
            End Set
        End Property
#End Region

#Region "Metodos"
        Public Shared Function ObtenerSeriales(ByVal idDevolucion, ByVal idDetalle) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            If idDetalle > 0 Then db.agregarParametroSQL("@idDetalle", idDetalle, SqlDbType.Int)
            If idDevolucion > 0 Then db.agregarParametroSQL("@idDevolucion", idDevolucion, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerDevolucionDetalleSerial", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Sub Registrar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            With db
                .agregarParametroSQL("@serial", Serial)
                .agregarParametroSQL("@idDevolucion", _idDevolucion, SqlDbType.Int)
                .agregarParametroSQL("@idClasificacion", _idClasificacion, SqlDbType.Int)
                .SqlParametros.Add("@codigoError", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                db.SqlParametros.Add("@idRegistroSerial", SqlDbType.Int).Direction = ParameterDirection.Output
                Try
                    db.iniciarTransaccion()
                    .ejecutarNonQuery("InsertarDevolucionDetalleSerial", CommandType.StoredProcedure)
                    Dim codigoError As Integer = db.SqlParametros("@codigoError").Value
                    If codigoError = 0 Then
                        _id = db.SqlParametros("@idRegistroSerial").Value
                        _novedades.IdRegistroSerial = _id
                        _novedades.Registrar(NovedadSerial.ProcesoNovedad.Devolucion, db)
                        db.confirmarTransaccion()
                    Else
                        Throw New Exception(codigoError)
                    End If
                Catch ex As Exception
                    db.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    db.Dispose()
                End Try
            End With
        End Sub

        Public Sub Eliminar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            With db
                .agregarParametroSQL("@serial", _serial)
                .agregarParametroSQL("@idDevolucion", _idDevolucion, SqlDbType.Int)
                .SqlParametros.Add("@codigoError", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .ejecutarNonQuery("EliminarDevolucionDetalleSerial", CommandType.StoredProcedure)
                Dim codigoError As Integer = .SqlParametros("@codigoError").Value
                If codigoError <> 0 Then Throw New Exception(codigoError.ToString())
            End With
        End Sub

#End Region

        Public Sub New()
            _novedades = New NovedadSerial
        End Sub
    End Class
End Namespace

