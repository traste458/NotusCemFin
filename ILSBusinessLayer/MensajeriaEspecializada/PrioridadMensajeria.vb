Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class PrioridadMensajeria

#Region "Atributos (Campos)"

    Private _idPrioridad As Integer
    Private _prioridad As String
    Private _activo As Boolean
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idPrioridad As Integer)
        MyBase.New()
        _idPrioridad = idPrioridad
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdPrioridad() As Integer
        Get
            Return _idPrioridad
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idPrioridad = value
        End Set
    End Property

    Public Property Prioridad() As String
        Get
            Return _prioridad
        End Get
        Protected Friend Set(ByVal value As String)
            _prioridad = value
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

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        Try
            With dbManager
                .SqlParametros.Add("@idPrioridad", SqlDbType.Int).Value = _idPrioridad
                .ejecutarReader("ObtenerPrioridadesDeMensajeria", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        Integer.TryParse(.Reader("idPrioridad").ToString, _idPrioridad)
                        _prioridad = .Reader("prioridad").ToString
                        _activo = CBool(.reader("activo"))
                        _registrado = True
                    End If
                    .Reader.Close()
                End If

            End With
        Finally

        End Try
    End Sub

#End Region

End Class
