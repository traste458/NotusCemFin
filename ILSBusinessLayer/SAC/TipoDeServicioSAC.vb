Imports LMDataAccessLayer

Namespace SAC

    Public Class TipoDeServicioSAC

#Region "Atributos"

        Private _idTipoServicio As Integer
        Private _descripcion As String
        Private _idClaseServicio As Short
        Private _claseDeServicio As String
        Private _activo As Boolean
        Private _idUnidadNegocio As Short
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdTipo() As Short
            Get
                Return _idTipoServicio
            End Get
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property IdClaseServicio() As String
            Get
                Return _idClaseServicio
            End Get
            Set(ByVal value As String)
                _idClaseServicio = value
            End Set
        End Property

        Public ReadOnly Property ClaseDeServicio() As String
            Get
                Return _claseDeServicio
            End Get
        End Property

        Public Property Activo() As Boolean
            Get
                Return _activo
            End Get
            Set(ByVal value As Boolean)
                _activo = value
            End Set
        End Property

        Public Property IdUnidadNegocio() As Short
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Short)
                _idUnidadNegocio = value
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
            _descripcion = ""
            _claseDeServicio = ""
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
                    .SqlParametros.Add("@listaIdTipoServicio", SqlDbType.VarChar).Value = identificador.ToString
                    .ejecutarReader("ConsultarTipoDeServicioSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
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
            _idTipoServicio = identificador
        End Sub

        Protected Friend Sub EstablecerClaseDeServicio(ByVal valor As String)
            _claseDeServicio = valor
        End Sub

        Protected Friend Sub MarcarComoRegistrado()
            _registrado = True
        End Sub

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idTipoServicio").ToString, _idTipoServicio)
                    _descripcion = reader("descripcion").ToString
                    Short.TryParse(reader("idClaseServicio").ToString, _idClaseServicio)
                    _claseDeServicio = reader("claseDeServicio").ToString
                    Boolean.TryParse(reader("activo").ToString, _activo)
                    Short.TryParse(reader("idUnidadNegocio").ToString, _idUnidadNegocio)
                    _registrado = True
                End If
            End If

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
