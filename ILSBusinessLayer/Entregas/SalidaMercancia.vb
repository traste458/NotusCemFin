Namespace ProcesoEntregas
    Public Class SalidaMercancia

#Region "Atributos"
        Private _identificador As Integer
        Private _despachos As ArrayList
        Private _nombreTransportador As String
        Private _identificacionTransportador As String
        Private _placaVehiculo As String
        Private _fechaSalida As Date
#End Region

#Region "Constructor"
        Public Sub New()
            _despachos = New ArrayList
        End Sub

        Public Sub New(ByVal idSalida As Integer)
            Me.New()
            Me.SeleccionarPorID(idSalida)
        End Sub
#End Region

#Region "Propiedades"
        Public ReadOnly Property Identificador() As Integer
            Get
                Return _identificador
            End Get
        End Property
        Public Property Despachos() As ArrayList
            Get
                Return _despachos
            End Get
            Set(ByVal value As ArrayList)
                _despachos = value
            End Set
        End Property

        Public Property NombreTransportador() As String
            Get
                Return _nombreTransportador
            End Get
            Set(ByVal value As String)
                _nombreTransportador = value
            End Set
        End Property

        Public Property IdentificacionTransportador() As String
            Get
                Return _identificacionTransportador
            End Get
            Set(ByVal value As String)
                _identificacionTransportador = value
            End Set
        End Property

        Public Property PlacaVehiculo() As String
            Get
                Return _placaVehiculo
            End Get
            Set(ByVal value As String)
                _placaVehiculo = value
            End Set
        End Property

        Public Property FechaSalida() As Date
            Get
                Return _fechaSalida
            End Get
            Set(ByVal value As Date)
                _fechaSalida = value
            End Set
        End Property
#End Region

#Region "Métodos Privados"
        Private Sub SeleccionarPorID(ByVal idSalida As Integer)

        End Sub
#End Region

#Region "Métodos Públicos"
        Public Sub RegistrarSalida()
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.iniciarTransaccion()
                adminBD.agregarParametroSQL("@nombreTransportador", Me._nombreTransportador, SqlDbType.VarChar)
                adminBD.agregarParametroSQL("@identificacionTransportador", Me._identificacionTransportador, SqlDbType.VarChar)
                adminBD.agregarParametroSQL("@placaVehiculo", Me._placaVehiculo, SqlDbType.VarChar)
                _identificador = adminBD.ejecutarScalar("CrearSalidaMercancia", CommandType.StoredProcedure)

                'Registro de despachos de la salida
                If Identificador <> 0 Then
                    For Each i As Despachos.Despacho In Me.Despachos
                        i.ActualizarEstado(New Estado(33)) 'TODO: Hacer esto transaccional!
                        adminBD.SqlParametros.Clear()
                        adminBD.agregarParametroSQL("@idDespacho", i.IdDespacho)
                        adminBD.agregarParametroSQL("@idSalida", _identificador)
                        adminBD.ejecutarNonQuery("RegistrarSalidaDespacho", CommandType.StoredProcedure)
                    Next

                    For Each item As Despachos.Despacho In Me.Despachos
                        adminBD.SqlParametros.Clear()
                        adminBD.agregarParametroSQL("@idDespacho", item.IdDespacho)
                        adminBD.agregarParametroSQL("@idSalida", _identificador)
                        adminBD.ejecutarNonQuery("ActualizarInformacionFormularios", CommandType.StoredProcedure)
                    Next
                Else
                    Throw New Exception("No se obtuvo identificador de salida")
                End If
                adminBD.confirmarTransaccion()
            Catch ex As Exception
                adminBD.abortarTransaccion()
                Throw New Exception("Error registrando la salida de mercancía: " & ex.Message)
            Finally
                adminBD.Dispose()
            End Try
        End Sub


        Public Function ValidarFormularios(ByRef idFabricante As Integer, ByVal idDespacho As Integer) As Boolean
            Dim respuesta As Boolean = False
            Dim retorno As Integer = 0
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.agregarParametroSQL("@idDespacho", idDespacho)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.ejecutarNonQuery("ValidarFormularios", CommandType.StoredProcedure)

                retorno = adminBD.SqlParametros("@return_value").Value

                If retorno = 0 Then
                    respuesta = True
                Else
                    idFabricante = retorno
                End If
            Catch ex As Exception
                Throw New Exception("Ocurrió un error validando información de los formularios: " & ex.Message)
            End Try

            Return respuesta
        End Function

        Public Function ExisteSalida(ByVal idDespacho As Integer) As Boolean
            Dim respuesta As Boolean = False
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.agregarParametroSQL("@idDespacho", idDespacho)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.ejecutarNonQuery("VerificarExistenciaPase", CommandType.StoredProcedure)

                respuesta = CBool(adminBD.SqlParametros("@return_value").Value)
            Catch ex As Exception
                Throw New Exception("Ocurrió un error consultando información de la salida de mercancía: " & ex.Message)
            End Try

            Return respuesta
        End Function

        Public Shared Function ValidarEstadoDespacho(ByVal estadoValido As Integer, ByVal idDespacho As Integer) As Integer
            Dim respuesta As Boolean = False
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.agregarParametroSQL("@idDespacho", idDespacho)
                adminBD.agregarParametroSQL("@idEstado", estadoValido)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.ejecutarNonQuery("VerificarEstadoDespacho", CommandType.StoredProcedure)

                respuesta = adminBD.SqlParametros("@return_value").Value
            Catch ex As Exception
                Throw New Exception("Ocurrió un error consultando información de la salida de mercancía: " & ex.Message)
            End Try

            Return respuesta
        End Function
#End Region

    End Class
End Namespace
