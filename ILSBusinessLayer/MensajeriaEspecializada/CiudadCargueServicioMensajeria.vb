Imports LMDataAccesLayer
Imports LMDataAccessLayer

Public Class CiudadCargueServicioMensajeria

#Region "Variables"

    Private _idCiudadCargue As Integer
    Private _idCiudadEquivalente As Integer
    Private _nombre As String
    Private _activo As Boolean

#End Region

#Region "Propiedades"

    Public ReadOnly Property IdCiudadCargue() As Integer
        Get
            Return _idCiudadCargue
        End Get
    End Property

    Public Property IdCiudadEquivalente() As Integer
        Get
            Return _idCiudadEquivalente
        End Get
        Set(ByVal value As Integer)
            _idCiudadEquivalente = value
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

    Public Property Activo() As Boolean
        Get
            Return _activo
        End Get
        Set(ByVal value As Boolean)
            _activo = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idCiudadEquivalente As Integer)
        MyBase.New()
        _idCiudadEquivalente = idCiudadEquivalente
        CargarDatos()
    End Sub

    Public Sub New(ByVal nombre As String)
        MyBase.New()
        _nombre = nombre
        CargarDatos()
    End Sub

#End Region

#Region "Metodos Publicos"

#End Region

#Region "Metodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idCiudadEquivalente > 0 Or (Not String.IsNullOrEmpty(_nombre)) Then
                    If _idCiudadEquivalente > 0 Then .SqlParametros.Add("@idCiudadEquivalente", SqlDbType.Int).Value = _idCiudadEquivalente
                    If (Not String.IsNullOrEmpty(_nombre)) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
                    .ejecutarReader("ObtenerCiudadCargueServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then CargarResultadoConsulta(.Reader)
                        .Reader.Close()
                    End If
                Else
                    Throw New Exception("No se ha establecido ningun dato para el objeto de traslado. ")
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Metodos Compartidos"

    Public Overloads Shared Function ObtenerListado() As DataTable
        Dim db As New LMDataAccess
        Dim dtDatos As DataTable
        dtDatos = db.ejecutarDataTable("ObtenerCiudadCargueServicioMensajeria", CommandType.StoredProcedure)
        Return dtDatos
    End Function


#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idCiudadCargue").ToString, _idCiudadCargue)
                Integer.TryParse(reader("idCiudadEquivalente").ToString, _idCiudadEquivalente)
                _nombre = reader("nombre").ToString()
                _activo = 1
            End If
        End If

    End Sub

#End Region

End Class
