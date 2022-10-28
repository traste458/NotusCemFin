Imports LMDataAccessLayer

Namespace SAC

    Public Class OrigenRespuestaGestionCasoSAC

#Region "Atributos"

        Private _idOrigenRespuesta As Byte
        Private _idUnidadNegocio As Byte
        Private _descripcion As String
        Private _requiereArchivo As Boolean
        Private _activo As Boolean
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdOrigenRespuesta() As Byte
            Get
                Return _idOrigenRespuesta
            End Get
            Protected Friend Set(ByVal value As Byte)
                _idOrigenRespuesta = value
            End Set
        End Property

        Public Property IdUnidadNegocio() As Byte
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Byte)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property RequiereArchivo() As Boolean
            Get
                Return _requiereArchivo
            End Get
            Set(ByVal value As Boolean)
                _requiereArchivo = value
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

        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _descripcion = ""
        End Sub

        Public Sub New(ByVal identificador As Byte)
            MyBase.New()
            CargarDatos(identificador)
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal identificador As Byte)
            Dim dbManager As New LMDataAccess            
            Try
                Dim idPerfil As Integer
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                With dbManager
                    .SqlParametros.Add("@idOrigen", SqlDbType.TinyInt).Value = identificador
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                    .ejecutarReader("ConsultarOrigenRespuestaGestionCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Byte.TryParse(.Reader("idOrigen").ToString, _idOrigenRespuesta)
                            Byte.TryParse(.Reader("idUnidadNegocio").ToString(), _idUnidadNegocio)
                            _descripcion = .Reader("descripcion").ToString
                            Boolean.TryParse(.Reader("requiereArchivo").ToString, _requiereArchivo)
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

#Region "Métodos Públicos"

        Private Sub Registrar()

        End Sub

        Private Sub Actualizar()

        End Sub

#End Region

    End Class

End Namespace

