Imports LMDataAccessLayer

Namespace SAC

    Public Class TipoDeClienteSAC

#Region "Atributos"

        Private _idTipo As Short
        Private _descripcion As String
        Private _idTipoGestion As Short
        Private _activo As Boolean
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdTipoCliente() As Short
            Get
                Return _idTipo
            End Get
            Protected Friend Set(ByVal value As Short)
                _idTipo = value
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

        Public Property IdTipoGestion() As Short
            Get
                Return _idTipoGestion
            End Get
            Set(ByVal value As Short)
                _idTipoGestion = value
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

        Public Sub New(ByVal identificador As Short)
            MyBase.New()
            CargarDatos(identificador)
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal identificador As Short)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idTipoCliente", SqlDbType.SmallInt).Value = identificador
                    .ejecutarReader("ConsultarTipoDeClienteSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Short.TryParse(.Reader("idTipoCliente").ToString, _idTipo)
                            _descripcion = .Reader("descripcion").ToString
                            Short.TryParse(.Reader("idTipoGestion").ToString, _idTipoGestion)
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