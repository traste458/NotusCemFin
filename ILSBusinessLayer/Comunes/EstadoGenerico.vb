Imports LMDataAccessLayer

Namespace Comunes

    Public Class EstadoGenerico

#Region "Variables"

        Private _idEstado As Short
        Private _descripcion As String
        Private _idEntidad As Short
        Private _entidad As String
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _descripcion = ""
            _entidad = ""
        End Sub

        Public Sub New(ByVal idEntidad As Short, ByVal idEstado As Short)
            Me.New()
            Me.CargarDatos(idEntidad, idEstado)
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdEstado() As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        Public Property IdEntidad() As Short
            Get
                Return _idEntidad
            End Get
            Set(ByVal value As Short)
                _idEntidad = value
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

        Public Property Entidad() As String
            Get
                Return _entidad
            End Get
            Protected Friend Set(ByVal value As String)
                _entidad = value
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

#Region "Metodos"

        Private Sub CargarDatos(ByVal idEntidad As Short, ByVal idEstado As Short)

            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idEntidad", SqlDbType.SmallInt).Value = idEntidad
                    .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = idEstado
                    .ejecutarReader("ObtenerEstadosGenericos", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Short.TryParse(.Reader("idEstado").ToString, _idEstado)
                            _descripcion = .Reader("descripcion").ToString
                            Short.TryParse(.Reader("idEntidad").ToString, _idEntidad)
                            _entidad = .Reader("entidad").ToString
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

        

    End Class

End Namespace
