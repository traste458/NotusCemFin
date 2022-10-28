Imports LMDataAccessLayer

Public Class ModuloUnidadNegocio

#Region "Atributos"

    Private _idModulo As Integer
    Private _idUnidadNegocio As Byte
    Private _modulo As String
    Private _unidadNegocio As String
    Private _url As String

#End Region

#Region "Propiedades"

    Public Property IdModulo() As Integer
        Get
            Return _idModulo
        End Get
        Set(ByVal value As Integer)
            _idModulo = value
        End Set
    End Property

    Public Property IdUnidadNegocio() As Byte
        Get
            Return _idUnidadNegocio
        End Get
        Set(ByVal value As Byte)
            _idUnidadNegocio = value
        End Set
    End Property

    Public Property Modulo() As String
        Get
            Return _modulo
        End Get
        Set(ByVal value As String)
            _modulo = value
        End Set
    End Property

    Public Property UnidadNegocio() As String
        Get
            Return _unidadNegocio
        End Get
        Set(ByVal value As String)
            _unidadNegocio = value
        End Set
    End Property

    Public Property Url() As String
        Get
            Return _url
        End Get
        Set(ByVal value As String)
            _url = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New(ByVal idModulo As Integer, ByVal idUnidadNegocio As Byte)
        MyBase.New()
        CargarDatos(idModulo, idUnidadNegocio)
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargarDatos(ByVal idModulo As Integer, ByVal idUnidadNegocio As Byte)
        Dim db As New LMDataAccess
        Try
            With db
                .SqlParametros.Add("@idModulo", SqlDbType.Int).Value = idModulo
                .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = _idUnidadNegocio
                .ejecutarReader("ObtenerModuloUnidadNegocio", CommandType.StoredProcedure)
                If Not .Reader Is Nothing Then
                    If .Reader.Read Then                        
                        _modulo = .Reader("modulo").ToString()
                        _unidadNegocio = .Reader("unidadNegocio").ToString()
                        _url = .Reader("url").ToString()
                    End If
                    .Reader.Close()
                End If
            End With
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
    End Sub

#End Region

#Region "Metodos Protegidos"

#End Region

#Region "Metodos Publicos"

#End Region


End Class
