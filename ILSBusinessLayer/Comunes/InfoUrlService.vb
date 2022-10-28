Imports LMDataAccessLayer

Namespace Comunes

    Public Class InfoUrlService

#Region "Atributos (Campos)"

        Private _idUrl As Integer
        Private _nombreServicio As String
        Private _url As String
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdUrl As Integer
            Get
                Return _idUrl
            End Get
            Set(value As Integer)
                _idUrl = value
            End Set
        End Property

        Public Property NombreServicio As String
            Get
                Return _nombreServicio
            End Get
            Set(value As String)
                _nombreServicio = value
            End Set
        End Property

        Public Property Url As String
            Get
                Return _url
            End Get
            Set(value As String)
                _url = value
            End Set
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Set(value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombreServicio = ""
            _url = ""
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

#Region "Métodos Privados"

        Public Sub CargarDatos(ByVal nombre As String)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@nombreServicio", SqlDbType.VarChar, 400).Value = nombre
                    .ejecutarReader("ObtenerInfoUrlService", CommandType.StoredProcedure)
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

        Public Sub AsignarUrl(ByRef objWS As System.Web.Services.Protocols.SoapHttpClientProtocol)
            If _registrado AndAlso _url IsNot Nothing AndAlso _url.Trim.Length > 0 Then
                objWS.Url = _url
            Else
                Throw New Exception("No se encontró registro de la dirección URL asociada al Web Service de nombre: " & _nombreServicio)
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idUrl"), _idUrl)
                    If Not IsDBNull(reader("nombreServicio")) Then _nombreServicio = (reader("nombreServicio").ToString)
                    If Not IsDBNull(reader("url")) Then _url = (reader("url").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace