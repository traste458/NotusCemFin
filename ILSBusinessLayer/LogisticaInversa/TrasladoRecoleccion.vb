Namespace LogisticaInversa
    Public Class TrasladoRecoleccion
        Private _idTraslado As Integer
        Private _idOrden As Integer
        Private _fechaTraslado As Date
        Private _idusuario As Integer
        Private _cantidadCajas As Integer

        Public Property Idusuario() As Integer
            Get
                Return _idusuario
            End Get
            Set(ByVal value As Integer)
                _idusuario = value
            End Set
        End Property

        Public Property FechaTraslado() As Date
            Get
                Return _fechaTraslado
            End Get
            Set(ByVal value As Date)
                _fechaTraslado = value
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

        Public Property IdTraslado() As Integer
            Get
                Return _idTraslado
            End Get
            Set(ByVal value As Integer)
                _idTraslado = value
            End Set
        End Property

        Private Sub CargarDatos(ByVal idOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            With db
                .agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
                Try
                    .ejecutarReader("ConsultarTrasladoRecoleccion", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _idTraslado = .Reader("idTraslado")
                            _idOrden = .Reader("idOrden")
                            Date.TryParse(.Reader("fechaTraslado").ToString(), _fechaTraslado)
                            Integer.TryParse(.Reader("idusuario").ToString, _idusuario)
                        End If
                    End If
                Finally
                    If Not db.Reader.IsClosed Then db.Reader.Close()
                    db.Dispose()
                End Try
            End With
        End Sub

        Public Sub New()

        End Sub

        Public Sub New(ByVal idOrden As Integer)
            Me.CargarDatos(idOrden)
        End Sub

        Public Sub Crear()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", _idOrden, SqlDbType.Int)
            db.SqlParametros.Add("@idTraslado", SqlDbType.Int).Direction = ParameterDirection.Output
            db.ejecutarNonQuery("RegistrarTraslado", CommandType.StoredProcedure)
            _idTraslado = db.SqlParametros("@idTraslado").Value
        End Sub

        Public Sub Cerrar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idUsuario", _idusuario, SqlDbType.Int)
            db.agregarParametroSQL("@idTraslado", _idTraslado, SqlDbType.Int)
            db.agregarParametroSQL("@cantidadCajas", _cantidadCajas, SqlDbType.Int)
            db.ejecutarNonQuery("CerrarTraslado", CommandType.StoredProcedure)
        End Sub

        Public Shared Function ObtenerPoolTraslados(ByVal idUsuario As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idUsuarioPool", idUsuario, SqlDbType.Int)
            'se envia 2 al sp para que filtre por las recolecciones que van hacia el usuario
            db.agregarParametroSQL("@tipoMovimiento", 2, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarOrdenesRecoleccion", CommandType.StoredProcedure)
            Return dt
        End Function

    End Class
End Namespace

