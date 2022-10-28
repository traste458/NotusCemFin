Imports LMDataAccessLayer
Namespace PermisoOpcion

    Public Class OpcionFuncionalDenegada

        Private _idOpcion As Integer

        Private _idDenegacion As Integer
        Private _nombreOpcion As String
        Private _idPerfil As Integer
        Private _perfil As String
        Private _activo As Enumerados.EstadoBinario
        Private _Registrado As Boolean

        Public Property IdOpcion() As Integer
            Get
                Return _idOpcion
            End Get
            Set(ByVal value As Integer)
                _idOpcion = value
            End Set
        End Property

        Public Property NombreOpcion() As String
            Get
                Return _nombreOpcion
            End Get
            Set(ByVal value As String)
                _nombreOpcion = value
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

        Public Property IdDenegacion() As Integer
            Get
                Return _idDenegacion
            End Get
            Set(ByVal value As Integer)
                _idDenegacion = value
            End Set
        End Property

        Public Property IdPerfil() As Integer
            Get
                Return _idPerfil
            End Get
            Set(ByVal value As Integer)
                _idPerfil = value
            End Set
        End Property

        Public Property Perfil() As String
            Get
                Return _perfil
            End Get
            Set(ByVal value As String)
                _perfil = value
            End Set
        End Property

        Public Property Registrado() As Boolean
            Get
                Return _Registrado
            End Get
            Set(ByVal value As Boolean)
                _Registrado = value
            End Set
        End Property

        Public Sub New()
            MyBase.New()
            _nombreOpcion = ""
            _Registrado = False
        End Sub

        Public Sub New(ByVal idDenegacion As Integer)
            MyBase.New()
            _nombreOpcion = ""
            _Registrado = False
            _idDenegacion = idDenegacion
            CargarInformacion()
        End Sub

        Private Sub CargarInformacion()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idDenegacion", SqlDbType.Int).Value = _idOpcion
                    .ejecutarReader("ObtenerOpcionFuncionalDenegada", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                        _idOpcion = .Reader.Item("idOpcionFuncional")
                        _nombreOpcion = .Reader.Item("nombre")
                        _activo = .Reader.Item("activo")
                        _idDenegacion = .Reader("idDenegarOpcion")
                        _idPerfil = .Reader("idPerfil")
                        _perfil = .Reader("perfil")
                        _Registrado = True
                    End If
                End With
            Catch ex As Exception
                Throw New Exception(" ocurrió un error al consultar la configuración de permisos sobre la opción " & ex.Message)
            End Try
        End Sub

        Public Function ObtenerListado() As DataTable
            Dim dtAux As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                dtAux = dbManager.ejecutarDataTable("ObtenerOpcionFuncionalDenegada", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtAux
        End Function

    End Class


End Namespace
