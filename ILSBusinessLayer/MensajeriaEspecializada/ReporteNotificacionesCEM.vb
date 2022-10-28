Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO

Namespace MensajeriaEspecializada

    Public Class ReporteNotificacionesCEM

#Region "Atributos"

        Private _numeroRadicado As Long
        Private _bodega As String
        Private _usuarioRegistro As String
        Private _estado As String
        Private _novedad As String

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property NumeroRadicado
            Get
                Return _numeroRadicado
            End Get
            Set(value)
                _numeroRadicado = value
            End Set
        End Property

        Public Property Bodega As String
            Get
                Return _bodega
            End Get
            Set(value As String)
                _bodega = value
            End Set
        End Property

        Public Property UsuarioRegistro As String
            Get
                Return _bodega
            End Get
            Set(value As String)
                _bodega = value
            End Set
        End Property

        Public Property Estado As String
            Get
                Return _estado
            End Get
            Set(value As String)
                _estado = value
            End Set
        End Property

        Public Property Novedad As String
            Get
                Return _novedad
            End Get
            Set(value As String)
                _novedad = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal numeroRadicado As Long)
            MyBase.New()
            _numeroRadicado = numeroRadicado
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@numeroRadicado", SqlDbType.Int).Value = CStr(_numeroRadicado)
                    .ejecutarReader("ReporteNotificacionesCEM", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
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

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("numeroRadicado"), _numeroRadicado)
                    If Not IsDBNull(reader("bodega")) Then _bodega = (reader("bodega").ToString)
                    If Not IsDBNull(reader("usuarioRegistro")) Then _usuarioRegistro = (reader("usuarioRegistro").ToString)
                    If Not IsDBNull(reader("estado")) Then _estado = (reader("estado").ToString)
                    If Not IsDBNull(reader("novedad")) Then _novedad = (reader("novedad").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace


