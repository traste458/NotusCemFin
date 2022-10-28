Imports LMDataAccessLayer
Namespace OMS

    Public Class ReprocesoSIM

#Region "Atributos"
        Private _idOrdenReproceso As Integer
        Private _sim As String
        Private _idOrdenAnterior As Integer
        Private _idsubproducto As Integer
        Private _linea As Integer
        Private _estiba As Integer
        Private _caja As Integer
        Private _fechaReprocesado As Date
        Private _simInicial As String
        Private _simFinal As String
        Private _idOtb As Long

#End Region

#Region "Propiedes"
        Public Property SIM() As String
            Get
                Return _sim
            End Get
            Set(ByVal value As String)
                _sim = value
            End Set
        End Property

        Public Property IdOrdenReproceso() As Integer
            Get
                Return _idOrdenReproceso
            End Get
            Set(ByVal value As Integer)
                _idOrdenReproceso = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function RegistrarReprocesoSim() As ResultadoProceso
            Dim db As New LMDataAccess
            Dim resultado As New ResultadoProceso
            Try
                resultado.Mensaje = "Resultado Exitoso"
                With db
                    .agregarParametroSQL("@idOrdenReproceso", _idOrdenReproceso, SqlDbType.Int)
                    If Not String.IsNullOrEmpty(_simInicial) Then _
                        .agregarParametroSQL("@simInicial", _simInicial, SqlDbType.VarChar, 50)
                    If Not String.IsNullOrEmpty(_simFinal) Then _
                        .agregarParametroSQL("@simFinal", _simFinal, SqlDbType.VarChar, 50)
                    If _idOtb > 0 Then .agregarParametroSQL("@idOTB", _idOtb, SqlDbType.BigInt)
                    .SqlParametros.Add("@codRetorno", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ReprocesarSIMS", CommandType.StoredProcedure)
                    If Integer.TryParse(.SqlParametros("@codRetorno").Value.ToString(), resultado.Valor) Then
                        If resultado.Valor = 0 Then
                            If .estadoTransaccional Then .confirmarTransaccion()
                        Else
                            If db IsNot Nothing AndAlso db.estadoTransaccional Then db.abortarTransaccion()
                            resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString()
                        End If
                    Else
                        Throw New Exception("Imposible validar la respuesta del servidor. Por favor intente nuevamente.")
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

#End Region

#Region "Metodos"

        Public Function LeerSimSuelta(ByVal sim As String) As ResultadoProceso
            _simInicial = sim
            _simFinal = ""
            _idOtb = 0

            Return RegistrarReprocesoSim()
        End Function

        Public Function LeerRango(ByVal simInicial As String, ByVal simFinal As String) As ResultadoProceso
            _simInicial = simInicial
            _simFinal = simFinal
            _idOtb = 0

            Return RegistrarReprocesoSim()
        End Function

        Public Function LeerOtb(ByVal idOtb As Long) As ResultadoProceso
            _simInicial = ""
            _simFinal = ""
            _idOtb = idOtb

            Return RegistrarReprocesoSim()
        End Function

        Public Shared Function ExisteSim(ByVal serial As String) As Boolean
            Dim siExiste As Boolean
            Dim db As New LMDataAccess
            db.agregarParametroSQL("@sim", serial, SqlDbType.VarChar)
            siExiste = (db.ejecutarScalar("ValidarExisteSIM", CommandType.StoredProcedure))
            Return siExiste
        End Function

        Public Shared Function ObtenerSimsReprocesadas(ByVal idOrden As Integer) As DataTable
            Dim dt As DataTable
            Dim db As New LMDataAccess
            db.agregarParametroSQL("@idOrdenReproceso", idOrden, SqlDbType.Int)
            dt = db.ejecutarDataTable("ObtenerSerialesReprocesoSIM", CommandType.StoredProcedure)
            Return dt
        End Function
#End Region
    End Class

End Namespace

