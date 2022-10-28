Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace Productos

    Public Class TipoEtiqueta
#Region "Atributos (Campos)"

        Private _idTipoEtiqueta As Short
        Private _descripcion As String
        Private _porDefecto As Boolean
        Private _activo As Boolean
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _descripcion = ""
            _registrado = False
        End Sub

        Public Sub New(ByVal idTipo As Short)
            Me.New()
            CargarDatos(idTipo)
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdTipoEtiqueta() As Short
            Get
                Return _idTipoEtiqueta
            End Get
            Set(ByVal value As Short)
                _idTipoEtiqueta = value
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

        Public Property PorDefecto() As Boolean
            Get
                Return _porDefecto
            End Get
            Set(ByVal value As Boolean)
                _porDefecto = True
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

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal idTipoEtiqueta As Short)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idTipoEtiqueta", SqlDbType.SmallInt).Value = idTipoEtiqueta
                    .ejecutarReader("ObtenerListadoTiposEtiqueta", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Short.TryParse(.Reader("idTipoEtiqueta").ToString, _idTipoEtiqueta)
                            _descripcion = .Reader("descripcion").ToString
                            _porDefecto = CBool(.Reader("porDefecto"))
                            _activo = CBool(.Reader("activo"))
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

#Region "Métodos Publicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Return resultado
        End Function

#End Region
    End Class

End Namespace
