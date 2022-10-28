Namespace LogisticaInversa
    Public Class NovedadCarga
        Private _idNovedadCarga As Integer
        Private _idTraslado As Integer
        Private _fechaSistema As Date
        Private _idEstado As Integer
        Private _observaciones As String
        Private _descripcion As String
        Private _idNovedadILS As Integer

        Public Property Observaciones() As String
            Get
                Return _observaciones
            End Get
            Set(ByVal value As String)
                _observaciones = value
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

        Public Property IdNovedadILS() As Integer
            Get
                Return _idNovedadILS
            End Get
            Set(ByVal value As Integer)
                _idNovedadILS = value
            End Set
        End Property

        Public Property IdNovedadCarga() As Integer
            Get
                Return _idNovedadCarga
            End Get
            Set(ByVal value As Integer)
                _idNovedadCarga = value
            End Set
        End Property

        Public Shared Function ObtenerListado() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerTipoNovedadesCargaILS", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function Consultar(ByVal idTraslado As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idTraslado", idTraslado, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerNovedadCargaILS", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ConsultarPorRecoleccion(ByVal idOrdenRecoleccion As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idRecoleccion", idOrdenRecoleccion, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerNovedadCargaILS", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Sub Registrar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.SqlParametros.Add("@idNovedadCarga", SqlDbType.Int).Direction = ParameterDirection.Output
            db.agregarParametroSQL("@idTraslado", _idTraslado, SqlDbType.Int)
            db.agregarParametroSQL("@idNovedadILS", _idNovedadILS, SqlDbType.Int)
            db.agregarParametroSQL("@observaciones", _observaciones)
            db.ejecutarNonQuery("RegistrarNovedadCargaILS", CommandType.StoredProcedure)
            _idNovedadCarga = db.SqlParametros("@idNovedadCarga").Value
        End Sub

        Public Sub Eliminar(ByVal idNovedad As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idNovedadCarga", idNovedad, SqlDbType.Int)
            db.ejecutarNonQuery("EliminarNovedadCargaILS", CommandType.StoredProcedure)
        End Sub
    End Class
End Namespace

