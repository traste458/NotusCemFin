Namespace OMS
    Public Class Distribucion
        Private _idDistribucion As Integer
        Private _fechaCreacion As Date
        Private _listaInstruccciones As List(Of InstruccionTrabajo)
        Private _idUsuario As Integer
        Public Property Idusuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Sub Crear(ByVal ListaCantidadRegiones As List(Of CantidadRegion), ByVal instruccion As InstruccionTrabajo)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                db.iniciarTransaccion()
                db.agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                db.SqlParametros.Add("@idDistribucion", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                db.ejecutarNonQuery("InsertarDistribucion", CommandType.StoredProcedure)
                instruccion.IdDistribucion = db.SqlParametros("@idDistribucion").Value
                If instruccion.IdDistribucion > 0 Then
                    For Each objDistribucion As CantidadRegion In ListaCantidadRegiones
                        instruccion.IdRegion = objDistribucion.idRegion
                        instruccion.Cantidad = objDistribucion.cantidad
                        instruccion.ParcialEnvio = objDistribucion.parcialEnvio
                        instruccion.Crear(db)
                    Next
                    db.confirmarTransaccion()
                Else
                    Throw New Exception("1")
                End If
            Catch ex As Exception
                db.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                db.Dispose()
            End Try
        End Sub

        Public Structure CantidadRegion
            Public idRegion As Integer
            Public cantidad As Integer
            Public parcialEnvio As Integer
        End Structure

        Public Shared Function ConsultarTotales(ByVal idFacturaGuia As Integer, ByVal mostrarOcultos As Boolean) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            If mostrarOcultos Then db.agregarParametroSQL("@visiblePorCliente", True, SqlDbType.Bit)
            db.agregarParametroSQL("@idFacturaGuia", idFacturaGuia, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarDistribucion", CommandType.StoredProcedure)
            Return dt
        End Function
    End Class
End Namespace

