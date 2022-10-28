Namespace LogisticaInversa
    Public Class RecoleccionSerial
#Region "Variables"
        Private _idOrden As Integer
        Private _serial As String
        Private _fechaRegistro As Date
        Private _cajaVacia As Boolean
        Private _idClasificacion As Integer
        Private _novedadSerial As NovedadSerial
#End Region

#Region "Propiedades"
        Public Property CajaVacia() As Boolean
            Get
                Return _cajaVacia
            End Get
            Set(ByVal value As Boolean)
                _cajaVacia = value
            End Set
        End Property

        Public ReadOnly Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get

        End Property

        Public Property Serial() As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property

        Public Property idOrden() As Integer
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Integer)
                _idOrden = value
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

        Public Property Novedades() As NovedadSerial
            Get
                Return _novedadSerial
            End Get
            Set(ByVal value As NovedadSerial)
                _novedadSerial = value
            End Set
        End Property

#End Region

#Region "Metodos"
        Public Shared Function Consultar(ByVal idOrden As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarRecoleccionSerial", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Sub Registrar(Optional ByVal sistema As Enumerados.Sistema = Enumerados.Sistema.BPColsys)

            Dim db As New LMDataAccessLayer.LMDataAccess

            db.agregarParametroSQL("@idOrden", _idOrden, SqlDbType.Int)
            db.agregarParametroSQL("@serial", _serial)
            db.agregarParametroSQL("@idClasificacion", _idClasificacion)
            'db.SqlParametros.Add("@idRecoleccionSerial", SqlDbType.Int).Direction = ParameterDirection.Output
            db.SqlParametros.Add("@codigoError", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            Try
                db.iniciarTransaccion()
                db.ejecutarNonQuery("InsertarRecoleccionSerial", CommandType.StoredProcedure)
                Dim codigoError As Integer = db.SqlParametros("@codigoError").Value
                If codigoError > 0 Then
                    _novedadSerial.IdRegistroSerial = db.SqlParametros("@codigoError").Value
                    _novedadSerial.Registrar(NovedadSerial.ProcesoNovedad.Recoleccion, db)
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

        End Sub

        Public Sub Eliminar(ByVal idRecoleccionSerial As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idRecoleccionSerial", idRecoleccionSerial, SqlDbType.Int)
            db.agregarParametroSQL("@idProceso", NovedadSerial.ProcesoNovedad.Recoleccion, SqlDbType.Int)
            db.ejecutarNonQuery("EliminarRecoleccionSerial", CommandType.StoredProcedure)
        End Sub

        Public Shared Function ValidarMateral(ByVal serial As String, ByVal idOrden As Integer) As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idOrden", idOrden, SqlDbType.Int)
            db.agregarParametroSQL("@serial", serial)
            Dim esValido As Boolean = db.ejecutarScalar("ValidarMaterialRecoleccion", CommandType.StoredProcedure)
            Return esValido
        End Function
#End Region

        Public Sub New()
            _novedadSerial = New NovedadSerial
        End Sub
    End Class

End Namespace
