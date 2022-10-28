Imports LMDataAccessLayer

Namespace SAC

    Public Class UsuarioTramitadorCasoSAC

#Region "Atributos"

        Private _idUsuario As Integer
        Private _nombre As String
        Private _email As String
        Private _idPerfil As Short
        Private _perfil As String
        Private _activo As Boolean
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
        End Property

        Public ReadOnly Property Nombre() As String
            Get
                Return _nombre
            End Get
        End Property

        Public ReadOnly Property EMail() As String
            Get
                Return _email
            End Get
        End Property

        Public ReadOnly Property Activo() As Boolean
            Get
                Return _activo
            End Get
        End Property

        Public ReadOnly Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
        End Property

        Public ReadOnly Property IdPerfil() As Short
            Get
                Return _idPerfil
            End Get
        End Property

        Public ReadOnly Property Perfil() As String
            Get
                Return _perfil
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
            _email = ""
            _perfil = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            CargarDatos(identificador)
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal identificador As Integer)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = identificador
                    .ejecutarReader("ConsultarUsuarioTramitadorCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idUsuario").ToString, _idUsuario)
                            _nombre = .Reader("nombre").ToString
                            _email = .Reader("email").ToString
                            Short.TryParse(.Reader("idPerfil").ToString, _idPerfil)
                            _perfil = .Reader("perfil").ToString
                            Boolean.TryParse(.Reader("activo").ToString, _activo)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub EstablecerIdentificador(ByVal identificador As Integer)
            _idUsuario = identificador
        End Sub

        Protected Friend Sub EstablecerNombre(ByVal valor As String)
            _nombre = valor
        End Sub

        Protected Friend Sub EstablecerEmail(ByVal valor As String)
            _email = valor
        End Sub

        Protected Friend Sub EstablecerIdPerfil(ByVal valor As Short)
            _idPerfil = valor
        End Sub

        Protected Friend Sub EstablecerPerfil(ByVal valor As String)
            _perfil = valor
        End Sub

        Protected Friend Sub MarcarComoRegistrado()
            _registrado = True
        End Sub

#End Region

#Region "Métodos Públicos"

#End Region

    End Class

End Namespace

