Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace Productos

    Public Class ClasificacionProducto

#Region "Atributos (Campos)"

        Private _idClasificacion As Short
        Private _nombre As String
        Private _visibilidadInterna As Boolean
        Private _visibilidadExterna As Boolean
        Private _activa As Boolean
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
        End Sub

        Public Sub New(ByVal identificador As Short)
            Me.New()
            _idClasificacion = identificador
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdClasificacion() As Short
            Get
                Return _idClasificacion
            End Get
            Set(ByVal value As Short)
                _idClasificacion = value
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

        Public Property VisibilidadInterna() As Boolean
            Get
                Return _visibilidadInterna
            End Get
            Set(ByVal value As Boolean)
                _visibilidadInterna = value
            End Set
        End Property

        Public Property VisibilidadExterna() As Boolean
            Get
                Return _visibilidadExterna
            End Get
            Set(ByVal value As Boolean)
                _visibilidadExterna = value
            End Set
        End Property

        Public Property Activa() As Boolean
            Get
                Return _activa
            End Get
            Set(ByVal value As Boolean)
                _activa = value
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

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@material", SqlDbType.VarChar, 7).Value = _idClasificacion
                    .ejecutarReader("ConsultarListadoClasificacionProducto", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _nombre = .Reader("nombre").ToString
                            _visibilidadInterna = CBool(.Reader("visibilidadInterna").ToString)
                            _visibilidadExterna = CBool(.Reader("visibilidadExterna").ToString)
                            _activa = CBool(.Reader("activa").ToString)
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
