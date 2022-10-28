Namespace LogisticaInversa
    Public Class TrasladoRecoleccionSerial
#Region "Variables"

        Private _novedadSerial As NovedadSerial
        Private _idRegistroSerial As Integer
        Private _idTraslado As Integer
        Private _idClasificacion As Integer
        Private _serial As String
        Private _fechaRegistro As Date
#End Region

#Region "Propiedades"

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

        Public Property IdTraslado() As Integer
            Get
                Return _idTraslado
            End Get
            Set(ByVal value As Integer)
                _idTraslado = value
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
        Public Shared Function Consultar(ByVal idTraslado As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idTraslado", idTraslado, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarTrasladoSerial", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Sub Registrar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idTraslado", _idTraslado, SqlDbType.Int)
            db.agregarParametroSQL("@serial", _serial)
            db.agregarParametroSQL("@idClasificacion", _idClasificacion)
            db.SqlParametros.Add("@idRegistroSerial", SqlDbType.Int).Direction = ParameterDirection.Output
            db.SqlParametros.Add("@codigoError", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            Try
                db.iniciarTransaccion()
                db.ejecutarNonQuery("InsertarTrasladoSerial", CommandType.StoredProcedure)
                Dim codigoError As Integer = db.SqlParametros("@codigoError").Value
                If codigoError = 0 Then
                    _novedadSerial.IdRegistroSerial = db.SqlParametros("@idRegistroSerial").Value
                    _novedadSerial.Registrar(NovedadSerial.ProcesoNovedad.Entrega, db)
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

        Public Shared Function ValidarMateral(ByVal serial As String, ByVal idTraslado As Integer) As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idTraslado", idTraslado, SqlDbType.Int)
            db.agregarParametroSQL("@serial", serial)
            Dim esValido As Boolean = db.ejecutarScalar("ValidarMaterialRecoleccion", CommandType.StoredProcedure)
            Return esValido
        End Function

        Public Sub Eliminar(ByVal idRecoleccionSerial As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idRegistroSerial", idRecoleccionSerial, SqlDbType.Int)
            db.ejecutarNonQuery("EliminarTrasladoSerial", CommandType.StoredProcedure)
        End Sub

#End Region

        Public Sub New()
            _novedadSerial = New NovedadSerial
        End Sub
    End Class
End Namespace

