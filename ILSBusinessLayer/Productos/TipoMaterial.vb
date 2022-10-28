Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace Productos

    Public Class TipoMaterial

#Region "Atributos (Campos)"

        Private _idTipoMaterial As Short
        Private _prefijo As String
        Private _activo As Boolean
        Private _registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _prefijo = ""
            _registrado = False
        End Sub

        Public Sub New(ByVal idTipoMaterial As Short)
            Me.New()
            CargarDatos(idTipoMaterial)
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdTipoMaterial() As Short
            Get
                Return _idTipoMaterial
            End Get
            Set(ByVal value As Short)
                _idTipoMaterial = value
            End Set
        End Property

        Public Property Prefijo() As String
            Get
                Return _prefijo
            End Get
            Set(ByVal value As String)
                _prefijo = value
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

        Private Sub CargarDatos(ByVal idTipoMaterial As Short)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idTipoMaterial", SqlDbType.SmallInt).Value = idTipoMaterial
                    .ejecutarReader("ObtenerListadoTiposMaterial", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Short.TryParse(.Reader("idTipoMaterial").ToString, _idTipoMaterial)
                            _prefijo = .Reader("prefijo").ToString
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