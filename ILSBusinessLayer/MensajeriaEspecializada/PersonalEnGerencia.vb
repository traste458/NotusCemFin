Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class PersonalEnGerencia

#Region "Atributos"

        Private _idPersonaGerencia As Integer
        Private _idGerencia As Integer
        Private _gerencia As String
        Private _idPersona As Integer
        Private _persona As String
        Private _identificacionPersona As String
        Private _idPersonaPadre As Integer
        Private _personaPadre As String
        Private _fechaRegistro As DateTime
        Private _idUsuarioRegitra As Integer

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdPersonaGerencia As Integer
            Get
                Return _idPersonaGerencia
            End Get
            Set(value As Integer)
                _idPersonaGerencia = value
            End Set
        End Property

        Public Property IdGerencia As Integer
            Get
                Return _idGerencia
            End Get
            Set(value As Integer)
                _idGerencia = value
            End Set
        End Property

        Public Property Gerencia As String
            Get
                Return _gerencia
            End Get
            Protected Friend Set(value As String)
                _gerencia = value
            End Set
        End Property

        Public Property IdPersona As Integer
            Get
                Return _idPersona
            End Get
            Set(value As Integer)
                _idPersona = value
            End Set
        End Property

        Public Property Persona As String
            Get
                Return _persona
            End Get
            Protected Friend Set(value As String)
                _persona = value
            End Set
        End Property

        Public Property IdentificacionPersona As String
            Get
                Return _identificacionPersona
            End Get
            Set(value As String)
                _identificacionPersona = value
            End Set
        End Property

        Public Property IdPersonaPadre As Integer
            Get
                Return _idPersonaPadre
            End Get
            Set(value As Integer)
                _idPersonaPadre = value
            End Set
        End Property

        Public Property PersonaPadre As String
            Get
                Return _personaPadre
            End Get
            Protected Friend Set(value As String)
                _personaPadre = value
            End Set
        End Property

        Public Property FechaRegistro As DateTime
            Get
                Return _fechaRegistro
            End Get
            Set(value As DateTime)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdUsuarioRegitra As Integer
            Get
                Return _idUsuarioRegitra
            End Get
            Set(value As Integer)
                _idUsuarioRegitra = value
            End Set
        End Property

#End Region

#Region "Construtores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idPersonaGerencia As Integer)
            MyBase.New()
            _idPersonaGerencia = idPersonaGerencia
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Protected Overloads Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idPersonaGerencia > 0 Then .SqlParametros.Add("@idPersonaGerencia", SqlDbType.Int).Value = _idPersonaGerencia
                        .ejecutarReader("ObtienePersonalEnGerencia", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                CargarResultadoConsulta(.Reader)
                            End If
                            .Reader.Close()
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idGerencia", SqlDbType.Int).Value = _idGerencia
                        If _idPersona > 0 Then .SqlParametros.Add("@idPersona", SqlDbType.Int).Value = _idPersona
                        If _idPersonaPadre > 0 Then .SqlParametros.Add("@idPersonaPadre", SqlDbType.Int).Value = _idPersonaPadre
                        .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegitra

                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@idPersonaGerencia", SqlDbType.Int).Direction = ParameterDirection.Output

                        .iniciarTransaccion()

                        .ejecutarScalar("RegistraPersonaEnGerencia", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), resultado.Valor)

                        If resultado.Valor = 0 Then
                            Integer.TryParse(.SqlParametros("@idPersonaGerencia").Value.ToString(), _idPersonaGerencia)
                            resultado.EstablecerMensajeYValor(0, "Registro de Persona en Gerencia exitoso.")
                            .confirmarTransaccion()
                        Else
                            resultado.EstablecerMensajeYValor(resultado.Valor, "Se generó un error inesperado al intentar registrar la Persona.")
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Function Eliminar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idPersonaGerencia", SqlDbType.Int).Value = _idPersonaGerencia
                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()

                        .ejecutarScalar("EliminaPersonaEnGerencia", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), resultado.Valor)

                        If resultado.Valor = 0 Then
                            resultado.EstablecerMensajeYValor(0, "Se realizó la desvinculación correctamente.")
                            .confirmarTransaccion()
                        Else
                            resultado.EstablecerMensajeYValor(resultado.Valor, "Se generó un error inesperado al intentar desvincular la Persona.")
                            .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idPersonaGerencia").ToString(), _idPersonaGerencia)
                    Integer.TryParse(reader("idGerencia").ToString(), _idGerencia)
                    _gerencia = reader("gerencia").ToString()
                    Integer.TryParse(reader("idPersona").ToString(), _idPersona)
                    _persona = reader("persona").ToString()
                    _identificacionPersona = reader("identificacionPersona").ToString()
                    Integer.TryParse(reader("idPersonaPadre"), _idPersonaPadre)
                    _personaPadre = reader("personaPadre").ToString()

                    _registrado = True
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
