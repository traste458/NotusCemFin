Imports LMDataAccessLayer

Namespace SAC

    Public Class UsuarioModuloSAC

#Region "Atributos"

        Private _idUsuario As Integer
        Private _idUnidadNegocio As Byte
        Private _nombre As String
        Private _email As String
        Private _activo As Boolean
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdUsuario() As Short
            Get
                Return _idUsuario
            End Get
        End Property

        Public Property IdUnidadNegocio() As Byte
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Byte)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property EMail() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
            End Set
        End Property

        Public Property Activo() As Boolean
            Get
                Return _activo
            End Get
            Set(ByVal value As Boolean)
                _activo = value
            End Set
        End Property

        Public ReadOnly Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
            _email = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            Me.New()
            CargarDatos(identificador)
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal identificador As Integer)
            Dim dbManager As New LMDataAccess
            Dim idPerfil As Integer
            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                With dbManager
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = identificador
                    .ejecutarReader("ConsultarUsuarioModuloSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idUsuario").ToString, _idUsuario)
                            Byte.TryParse(.Reader("idUnidadNegocio").ToString(), _idUnidadNegocio)
                            _nombre = .Reader("nombre").ToString
                            _email = .Reader("email").ToString
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

        Protected Friend Sub EstablecerIdentificador(ByVal identificador As Short)
            _idUsuario = identificador
        End Sub

        Protected Friend Sub MarcarComoRegistrado()
            _registrado = True
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Me._nombre.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess
                Dim idPerfil As Integer
                Try
                    If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                        Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                    Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                    With dbManager
                        Me._idUnidadNegocio = usuarioUnidad.IdUnidadNegocio
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar, 100).Value = Me._nombre.Trim
                        If Me._idUnidadNegocio > 0 Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = Me._idUnidadNegocio
                        If Me._email.Trim.Length > 0 Then .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = Me._email.Trim
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .EjecutarNonQuery("RegistrarUsuarioModuloSAC", CommandType.StoredProcedure)
                    End With
                    With resultado
                        .Valor = CShort(dbManager.SqlParametros("@returnValue").Value)
                        If .Valor <> 0 Then
                            Select Case .Valor
                                Case 1
                                    .Mensaje = "Ya existe un usuario registrado con el nombre especificado. Por favor verifique."
                                Case 2
                                    .Mensaje = "Ocurrió un error inesperado al tratar de registar datos. Por favor intente nuevamente"
                            End Select
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.Valor = 3
                resultado.Mensaje = "No se han proporcionado todos los datos requeridos para procesar el registro de los datos. Por favor verifique"
            End If

            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

#End Region

    End Class

End Namespace
