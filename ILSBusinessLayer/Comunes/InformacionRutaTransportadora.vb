Public Class InformacionRutaTransportadora

#Region "Atributos"
    Private _idCiudadOrigen As Integer
    Private _idCiudadDestino As Integer
    Private _idTransportadora As Integer
    Private _idTipoProducto As Integer
    Private _idTipoDestinatario As Integer
    Private _idTipoTransporte As Integer
    Private _codigo As String
#End Region

#Region "Propiedades"
    Public Property IdCiudadOrigen() As Integer
        Get
            Return _idCiudadOrigen
        End Get
        Set(ByVal value As Integer)
            _idCiudadOrigen = value
        End Set
    End Property

    Public Property IdCiudadDestino() As Integer
        Get
            Return _idCiudadDestino
        End Get
        Set(ByVal value As Integer)
            _idCiudadDestino = value
        End Set
    End Property

    Public Property IdTransportadora() As Integer
        Get
            Return _idTransportadora
        End Get
        Set(ByVal value As Integer)
            _idTransportadora = value
        End Set
    End Property

    Public Property IdTipoProducto() As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(ByVal value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property IdTipoDestinatario() As Integer
        Get
            Return _idTipoDestinatario
        End Get
        Set(ByVal value As Integer)
            _idTipoDestinatario = value
        End Set
    End Property

    Public Property IdTipoTransporte() As Integer
        Get
            Return _idTipoTransporte
        End Get
        Set(ByVal value As Integer)
            _idTipoTransporte = value
        End Set
    End Property

    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property
#End Region

#Region "Constructores"
    Public Sub New()
        _idCiudadOrigen = 0
        _idCiudadDestino = 0
        _idTransportadora = 0
        _idTipoProducto = 0
        _idTipoDestinatario = 0
        _idTipoTransporte = 0
        _codigo = ""
    End Sub

    Public Sub New(ByVal idCiudadOrigen As Integer, ByVal idCiudadDestino As Integer)
        Me.New()
        ConsultarInformacionRuta(idCiudadOrigen, idCiudadDestino)
    End Sub
#End Region

#Region "Métodos privados"
    Private Sub ConsultarInformacionRuta(ByVal idCiudadOrigen As Integer, ByVal idCiudadDestino As Integer)
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        Try
            adminBD.agregarParametroSQL("@idCiudadOrigen", idCiudadOrigen)
            adminBD.agregarParametroSQL("@idCiudadDestino", idCiudadDestino)
            adminBD.ejecutarReader("SeleccionarInfoRutaTransportadora", CommandType.StoredProcedure)
            While adminBD.Reader.Read()
                Me._idCiudadOrigen = adminBD.Reader("idCiudadOrigen")
                Me._idCiudadDestino = adminBD.Reader("idCiudadDestino")
                Me._idTransportadora = adminBD.Reader("idTransportadora")
                Me._idTipoProducto = adminBD.Reader("idTipoProducto")
                Me._idTipoDestinatario = adminBD.Reader("idTipoDestinatario")
                Me._idTipoTransporte = adminBD.Reader("idTipoTransporte")
                Me._codigo = adminBD.Reader("codigo").ToString
            End While
        Catch ex As Exception
            Throw New Exception("Imposible obtener información con los datos especificados")
        Finally
            If Not adminBD.Reader.IsClosed Then adminBD.Reader.Close()
            adminBD.Dispose()
        End Try
    End Sub
#End Region

#Region "Métodos compartidos"
    Public Shared Function ObtenerCiudadesOrigen() As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim respuesta As New DataTable

        respuesta = adminBD.ejecutarDataTable("ObtenerCiudadesOrigen", CommandType.StoredProcedure)

        Return respuesta
    End Function

    Public Shared Function ObtenerCiudadesDestino(Optional ByVal idCiudadOrigen As Integer = 0) As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim respuesta As New DataTable

        adminBD.agregarParametroSQL("@idCiudadOrigen", idCiudadOrigen)
        respuesta = adminBD.ejecutarDataTable("ObtenerCiudadesDestino", CommandType.StoredProcedure)

        Return respuesta
    End Function

    Public Shared Function ObtenerTransportadoras() As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim respuesta As New DataTable
        Dim filtroTransportadora As Estructuras.FiltroTransportadora

        filtroTransportadora.Activo = Enumerados.EstadoBinario.Activo
        filtroTransportadora.CargaPorImportacion = 2

        respuesta = Transportadora.ListadoTransportadoras(filtroTransportadora)

        Return respuesta
    End Function

    Public Shared Function ObtenerTipoProducto() As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim respuesta As New DataTable

        respuesta = adminBD.ejecutarDataTable("ObtenerTiposProducto", CommandType.StoredProcedure)

        Return respuesta
    End Function

    Public Shared Function ObtenerTipoDestinatario() As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim respuesta As New DataTable

        respuesta = adminBD.ejecutarDataTable("ObtenerTiposDestinatario", CommandType.StoredProcedure)

        Return respuesta
    End Function

    Public Shared Function ObtenerTipoTransporte() As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim respuesta As New DataTable

        respuesta = Despachos.TipoTransporte.ListadoTipos

        Return respuesta
    End Function

#End Region

#Region "Métodos Públicos"
    Public Sub EditarDatoRutaTransportadora()
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        adminBD.agregarParametroSQL("@idCiudadOrigen", IdCiudadOrigen)
        adminBD.agregarParametroSQL("@idCiudadDestino", IdCiudadDestino)
        adminBD.agregarParametroSQL("@idTransportadora", Me._idTransportadora)
        adminBD.agregarParametroSQL("@idTipoProducto", Me._idTipoProducto)
        adminBD.agregarParametroSQL("@idTipoDestinatario", Me._idTipoDestinatario)
        adminBD.agregarParametroSQL("@idTipoTransporte", Me._idTipoTransporte)
        adminBD.agregarParametroSQL("@codigo", Me._codigo)

        adminBD.ejecutarNonQuery("EditarInformacionRutaTransportadora", CommandType.StoredProcedure)
    End Sub
#End Region


End Class
