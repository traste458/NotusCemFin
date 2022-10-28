Imports LMDataAccessLayer

Namespace SAC

    Public Class ClaseDeServicioSAC

#Region "Atributos"

        Private _idClase As Short
        Private _idUnidadNegocio As Byte
        Private _codigo As String
        Private _descripcion As String
        Private _tiempoMaxParaRespuesta As Integer
        Private _activo As Boolean
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdClase() As Short
            Get
                Return _idClase
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

        Public Property Codigo() As String
            Get
                Return _codigo
            End Get
            Set(ByVal value As String)
                _codigo = value
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

        Public Property TiempoMaximoParaRespuesta() As Integer
            Get
                Return _tiempoMaxParaRespuesta
            End Get
            Set(ByVal value As Integer)
                _tiempoMaxParaRespuesta = value
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
            _codigo = ""
            _descripcion = ""
            _registrado = False
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
                    .SqlParametros.Add("@listaIdClase", SqlDbType.VarChar).Value = identificador.ToString
                    .ejecutarReader("ConsultarClaseDeServicioSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Short.TryParse(.Reader("idClaseServicio").ToString, _idClase)
                            _codigo = .Reader("codigo").ToString
                            _descripcion = .Reader("descripcion").ToString
                            Integer.TryParse(.Reader("tiempoRespuesta").ToString, _tiempoMaxParaRespuesta)
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
            _idClase = identificador
        End Sub

        Protected Friend Sub MarcarComoRegistrado()
            _registrado = True
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