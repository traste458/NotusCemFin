Imports LMDataAccessLayer
Imports System.Reflection

Namespace PermisoOpcion

    Public Class OpcionEspecificaDenegada

        Inherits CollectionBase

        Private _idOpcionEspecifica As Integer
        Private _idOpcionFuncional As Integer
        Private _nombreOpcionFuncional As String
        Private _nombreOpcionEspecifica As String
        Private _activo As Enumerados.EstadoBinario
        Private _idPerfil As Integer
        Private _idDenegacion As Integer
        Private _identificadorDenegacion As Integer
        Private _Registrado As Boolean

        Public Property IdOpcionEspecifica() As Integer
            Get
                Return _idOpcionEspecifica
            End Get
            Set(ByVal value As Integer)
                _idOpcionEspecifica = value
            End Set
        End Property

        Public Property IdOpcionFuncional() As Integer
            Get
                Return _idOpcionFuncional
            End Get
            Set(ByVal value As Integer)
                _idOpcionFuncional = value
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

        Public Property IdDenegacion() As Integer
            Get
                Return _idDenegacion
            End Get
            Set(ByVal value As Integer)
                _idDenegacion = value
            End Set
        End Property

        Public Property IdentificadorDenegacion() As Integer
            Get
                Return _identificadorDenegacion
            End Get
            Set(ByVal value As Integer)
                _identificadorDenegacion = value
            End Set
        End Property

        Public Property NombreOpcionFuncional() As String
            Get
                Return _nombreOpcionFuncional
            End Get
            Set(ByVal value As String)
                _nombreOpcionFuncional = value
            End Set
        End Property

        Public Property NombreOpcionEspecifica() As String
            Get
                Return _nombreOpcionEspecifica
            End Get
            Set(ByVal value As String)
                _nombreOpcionEspecifica = value
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
                Return _Registrado
            End Get
            Set(ByVal value As Boolean)
                _Registrado = value
            End Set
        End Property

        Public Sub New()
            MyBase.New()
            _nombreOpcionEspecifica = ""
            _Registrado = False
        End Sub

        Public Sub New(ByVal idDenegacion As Integer, ByVal identificador As Integer)
            MyBase.New()
            _nombreOpcionEspecifica = ""
            _Registrado = False
            _idDenegacion = idDenegacion
            _identificadorDenegacion = identificador
            CargarInformacion()
        End Sub

        Private Sub CargarInformacion()
            Dim db As New LMDataAccess
            Try
                With db
                    .SqlParametros.Add("@idDenegacion", SqlDbType.Int).Value = _idDenegacion
                    .SqlParametros.Add("@identificador", SqlDbType.Int).Value = _identificadorDenegacion
                    .ejecutarReader("ObtenerOpcionEspecificaDenegada", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                        _idOpcionEspecifica = .Reader("idListadoOpcion")
                        _idOpcionFuncional = .Reader("idOpcionFuncional")
                        _nombreOpcionFuncional = .Reader("nombreOpcionFuncional")
                        _nombreOpcionEspecifica = .Reader("nombreOpcionEspecifica")
                        _activo = .Reader("activo")
                        _idPerfil = .Reader("idPerfil")
                        _Registrado = True
                    End If
                End With
            Catch ex As Exception
                Throw New Exception(" ocurrio un error consultando la información de la opcion funcional " & ex.Message)
            End Try
        End Sub

        Public Shared Function ObtenerListado() As DataTable
            Dim dtAux As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                dtAux = dbManager.ejecutarDataTable("ObtenerOpcionEspecificaDenegada", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtAux
        End Function
    End Class

End Namespace
