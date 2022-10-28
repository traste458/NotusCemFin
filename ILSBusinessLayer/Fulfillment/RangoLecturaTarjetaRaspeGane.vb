Imports LMDataAccessLayer

Namespace Fulfillment

    Public Class RangoLecturaTarjetaRaspeGane

#Region "Atributos"

        Private _idRango As Integer
        Private _inicioRango As Long
        Private _finRango As Long
        Private _consecutivoActual As Long
        Private _linea As Integer
        Private _idEstado As Short
        Private _fechaRegistro As Date
        Private _idUsuario As Integer
        Private _registrado As Boolean

#End Region

#Region "Contructores"

        Public Sub New()
            _idEstado = -1
        End Sub

        Public Sub New(ByVal identificador As Integer)
            Me.New()
            _idRango = identificador
            CargarInformacion()
        End Sub

        Public Sub New(ByVal linea As Integer, ByVal idEstado As Short)
            Me.New()
            _linea = linea
            _idEstado = idEstado
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdRango As Integer
            Get
                Return _idRango
            End Get
            Protected Friend Set(ByVal value As Integer)
                _idRango = value
            End Set
        End Property

        Public Property InicioRango As Long
            Get
                Return _inicioRango
            End Get
            Set(ByVal value As Long)
                _inicioRango = value
            End Set
        End Property

        Public Property FinRango As Long
            Get
                Return _finRango
            End Get
            Set(ByVal value As Long)
                _finRango = value
            End Set
        End Property

        Public Property ConsecutivoActual As Long
            Get
                Return _consecutivoActual
            End Get
            Protected Friend Set(value As Long)
                _consecutivoActual = value
            End Set
        End Property

        Public Property Linea As Integer
            Get
                Return _linea
            End Get
            Set(value As Integer)
                _linea = value
            End Set
        End Property

        Public Property IdEstado As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        Public Property FechaRegistro As Date
            Get
                Return _fechaRegistro
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdUsuario As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If Me._idRango > 0 OrElse (Me._linea > 0 AndAlso Me._idEstado >= 0) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me._idRango > 0 Then .SqlParametros.Add("@idRango", SqlDbType.Int).Value = Me._idRango
                        If Me._linea > 0 AndAlso Me._idEstado >= 0 Then
                            .SqlParametros.Add("@linea", SqlDbType.Int).Value = Me._linea
                            .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = Me._idEstado
                        End If

                        .ejecutarReader("ObtenerInfoRangoLecturaTarjetaRaspeGane", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                AsignarValorAPropiedades(.Reader)
                            End If
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub AsignarValorAPropiedades(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idRango").ToString, Me._idRango)
                    Long.TryParse(reader("inicioRango").ToString, Me._inicioRango)
                    Long.TryParse(reader("finRango").ToString, Me._finRango)
                    Short.TryParse(reader("idEstado").ToString, Me._idEstado)
                    Date.TryParse(reader("fechaRegistro").ToString, _fechaRegistro)
                    Integer.TryParse(reader("idUsuario").ToString, Me._idUsuario)
                    Me._registrado = True
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso(200, "Proceso no exitoso. Por favor intente nuevamente")
            If Me._idUsuario > 0 AndAlso Me._linea > 0 AndAlso Me._inicioRango > 0 AndAlso Me._finRango > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@inicioRango", SqlDbType.BigInt).Value = Me._inicioRango
                        .SqlParametros.Add("@finRango", SqlDbType.BigInt).Value = Me._finRango
                        .SqlParametros.Add("@linea", SqlDbType.Int).Value = Me._linea
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                        .SqlParametros.Add("@idRango", SqlDbType.Int).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("RegistrarRangoLecturaTarjetaRaspeGane", CommandType.StoredProcedure)
                        If Short.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            Me._idRango = CInt(.SqlParametros("@idRango").Value)
                            resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                        Else
                            resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta emitida por el servidor. Por favor intente nuevamente")
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(300, "No se han establecido todos los datos requeridos para realizar el registro. Por favor verifique")
            End If

            Return resultado
        End Function

#End Region

    End Class

End Namespace