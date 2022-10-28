Imports LMDataAccesLayer
Imports LMDataAccessLayer

Public Class TipoArchivoCargueServicioMensajeria

#Region "Variables"

    Private _idTipoArchivo As Integer
    Private _idTipoServico As Integer
    Private _nombre As String
    Private _activo As Boolean

#End Region

#Region "Propiedades"

    Public ReadOnly Property IdTipoArchivo() As Integer
        Get
            Return _idTipoArchivo
        End Get
    End Property

    Public Property IdTipoServicio() As Integer
        Get
            Return _idTipoServico
        End Get
        Set(ByVal value As Integer)
            _idTipoServico = value
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

    Public Sub New(ByVal idTipoArchivo As Integer)
        MyBase.New()
        _idTipoArchivo = idTipoArchivo
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
                If _idTipoArchivo > 0 Or _idTipoServico > 0 Then
                    If _idTipoArchivo > 0 Then .SqlParametros.Add("@idTipoArchivo", SqlDbType.Int).Value = _idTipoArchivo
                    If _idTipoServico > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServico
                    .ejecutarReader("ObtenerTipoArchivoCargueServicioMensajeria", CommandType.StoredProcedure)
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
        dtDatos = db.ejecutarDataTable("ObtenerTipoArchivoCargueServicioMensajeria", CommandType.StoredProcedure)
        Return dtDatos
    End Function


#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idTipoArchivo").ToString, _idTipoArchivo)
                Integer.TryParse(reader("idTipoServicio").ToString, _idTipoServico)
                _nombre = reader("tipoArchivo").ToString()
                _activo = 1
            End If
        End If

    End Sub

#End Region

End Class
