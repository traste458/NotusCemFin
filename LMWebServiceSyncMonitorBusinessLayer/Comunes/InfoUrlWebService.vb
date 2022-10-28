Imports LMDataAccessLayer
Imports LMWebServiceSyncMonitorBusinessLayer.ClasesComunes

Public Class InfoUrlWebService

#Region "Atributos (Campos)"
    Private _idUrl As Integer
    Private _nombreServicio As String
    Private _url As String
    Private _proveedor As String
    Private _usuarioAcceso As String
    Private _passwordAcceso As String
    Private _dominioAcceso As String
    Private _registrado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _nombreServicio = ""
        _url = ""
        _proveedor = ""
        _usuarioAcceso = ""
        _passwordAcceso = ""
        _dominioAcceso = ""
    End Sub

    Public Sub New(ByVal objWS As System.Web.Services.Protocols.SoapHttpClientProtocol)
        Me.New()
        If objWS IsNot Nothing Then _nombreServicio = objWS.GetType().Name
        CargarDatos(_nombreServicio)
    End Sub

    Public Sub New(ByVal objWS As System.Web.Services.Protocols.SoapHttpClientProtocol, ByVal asignarUrlObtenida As Boolean)
        Me.New()
        If objWS IsNot Nothing Then _nombreServicio = objWS.GetType().Name
        CargarDatos(_nombreServicio)
        If asignarUrlObtenida Then AsignarUrl(objWS)
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdUrl() As Integer
        Get
            Return _idUrl
        End Get
        Set(ByVal value As Integer)
            _idUrl = value
        End Set
    End Property

    Public Property NombreServicio() As String
        Get
            Return _nombreServicio
        End Get
        Set(ByVal value As String)
            _nombreServicio = value
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

    Public Property ProveedorServicio() As String
        Get
            Return _proveedor
        End Get
        Set(ByVal value As String)
            _proveedor = value
        End Set
    End Property

    Public Property UsuarioAcceso() As String
        Get
            Return _usuarioAcceso
        End Get
        Set(ByVal value As String)
            _usuarioAcceso = value
        End Set
    End Property

    Public Property PassWordAcceso() As String
        Get
            Return _passwordAcceso
        End Get
        Set(ByVal value As String)
            _passwordAcceso = value
        End Set
    End Property

    Public Property DominioAcceso() As String
        Get
            Return _dominioAcceso
        End Get
        Set(ByVal value As String)
            _dominioAcceso = value
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

#Region "Métodos Provados"

    Private Sub CargarDatos(ByVal nombre As String)
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@nombreServicio", SqlDbType.VarChar, 400).Value = nombre
                .ejecutarReader("ObtenerInfoUrlWebService", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        Integer.TryParse(.Reader("idUrl").ToString, _idUrl)
                        _nombreServicio = .Reader("nombreServicio").ToString
                        _url = .Reader("url").ToString
                        _proveedor = .Reader("proveedor").ToString
                        _usuarioAcceso = .Reader("usuarioAcceso").ToString
                        _passwordAcceso = .Reader("passwordAcceso").ToString
                        _dominioAcceso = .Reader("dominioAcceso").ToString
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

    Public Sub AsignarUrl(ByRef objWS As System.Web.Services.Protocols.SoapHttpClientProtocol)
        If _registrado AndAlso _url IsNot Nothing AndAlso _url.Trim.Length > 0 Then
            objWS.Url = _url
        Else
            Throw New Exception("No se encontró registro de la dirección URL asociada al Web Service de nombre: " & _nombreServicio)
        End If
    End Sub

#End Region

End Class

