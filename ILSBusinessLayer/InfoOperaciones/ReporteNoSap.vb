Imports LMDataAccessLayer

Public Class ReporteNoSap

#Region "Atributos"
    Private _idOrdenRecepcion As Integer
    Private _idOrdenCompra As Integer
    Private _numeroOrdenCompra As String
    Private _idTipoProducto As Integer
    Private _estado As Integer
    Private _estadoInstruccion As String
    Private _fabricante As Integer
    Private _factura As String
    Private _guia As String
    Private _idProducto As Integer
    Private _material As String
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _fechaInstruccionInicial As Date
    Private _fechaInstruccionFinal As Date
    Private _fechaEnvioInicial As Date
    Private _fechaEnvioFinal As Date
    Private _estructuraTablaConsulta As DataTable
#End Region

#Region "Propiedades"

    Public Property IdOrdenRecepcion As Integer
        Get
            Return _idOrdenRecepcion
        End Get
        Set(value As Integer)
            _idOrdenRecepcion = value
        End Set
    End Property

    Public Property IdOrdenCompra As Integer
        Get
            Return _idOrdenCompra
        End Get
        Set(value As Integer)
            _idOrdenCompra = value
        End Set
    End Property

    Public Property NumeroOrdenCompra As String
        Get
            Return _numeroOrdenCompra
        End Get
        Set(value As String)
            _numeroOrdenCompra = value
        End Set
    End Property

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property Estado As Integer
        Get
            Return _estado
        End Get
        Set(value As Integer)
            _estado = value
        End Set
    End Property

    Public Property EstadoInstruccion As String
        Get
            Return _estadoInstruccion
        End Get
        Set(value As String)
            _estadoInstruccion = value
        End Set
    End Property

    Public Property Fabricante As Integer
        Get
            Return _fabricante
        End Get
        Set(value As Integer)
            _fabricante = value
        End Set
    End Property

    Public Property Factura As String
        Get
            Return _factura
        End Get
        Set(value As String)
            _factura = value
        End Set
    End Property

    Public Property Guia As String
        Get
            Return _guia
        End Get
        Set(value As String)
            _guia = value
        End Set
    End Property

    Public Property IdProducto As Integer
        Get
            Return _idProducto
        End Get
        Set(value As Integer)
            _idProducto = value
        End Set
    End Property

    Public Property Material As String
        Get
            Return _material
        End Get
        Set(value As String)
            _material = value
        End Set
    End Property

    Public Property FechaInicial As Date
        Get
            Return _fechaInicial
        End Get
        Set(value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal As Date
        Get
            Return _fechaFinal
        End Get
        Set(value As Date)
            _fechaFinal = value
        End Set
    End Property

    Public Property FechaInstruccionInicial As Date
        Get
            Return _fechaInstruccionInicial
        End Get
        Set(value As Date)
            _fechaInstruccionInicial = value
        End Set
    End Property

    Public Property FechaInstruccionFinal As Date
        Get
            Return _fechaInstruccionFinal
        End Get
        Set(value As Date)
            _fechaInstruccionFinal = value
        End Set
    End Property

    Public Property FechaEnvioInicial As Date
        Get
            Return _fechaEnvioInicial
        End Get
        Set(value As Date)
            _fechaEnvioInicial = value
        End Set
    End Property

    Public Property FechaEnvioFinal As Date
        Get
            Return _fechaEnvioFinal
        End Get
        Set(value As Date)
            _fechaEnvioFinal = value
        End Set
    End Property

    Public Property EstructuraTablaConsulta As DataTable
        Get
            Return _estructuraTablaConsulta
        End Get
        Set(value As DataTable)
            _estructuraTablaConsulta = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos Publicos"

    Public Function ObtenerInformacionNoSap() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                .TiempoEsperaComando = 300
                If _idOrdenRecepcion > 0 Then .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                If _idOrdenCompra > 0 Then .SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = _idOrdenCompra
                If _numeroOrdenCompra IsNot Nothing Then .SqlParametros.Add("@ordenCompra", SqlDbType.VarChar).Value = _numeroOrdenCompra
                If _idTipoProducto > 0 Then .SqlParametros.Add("@tipoProducto", SqlDbType.Int).Value = _idTipoProducto
                If _estado > 0 Then .SqlParametros.Add("@estado", SqlDbType.Int).Value = _estado
                If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = _fechaInicial
                If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = _fechaFinal
                dtResultado = .ejecutarDataTable("ObtenerInformacionDeReporteNoSap", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Function ObtenerInformacionNoSapAduanera() As DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                .TiempoEsperaComando = 0
                If _numeroOrdenCompra IsNot Nothing AndAlso _numeroOrdenCompra <> "" Then .SqlParametros.Add("@ordenCompra", SqlDbType.VarChar).Value = _numeroOrdenCompra
                If _idTipoProducto > 0 Then .SqlParametros.Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
                If _estadoInstruccion IsNot Nothing AndAlso _estadoInstruccion <> "" Then .SqlParametros.Add("@estado", SqlDbType.VarChar).Value = _estadoInstruccion
                If _fabricante > 0 Then .SqlParametros.Add("@idFabricante", SqlDbType.Int).Value = _fabricante
                If _factura IsNot Nothing AndAlso _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                If _guia IsNot Nothing AndAlso _guia <> "" Then .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If _material IsNot Nothing AndAlso _material <> "" Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
                If _fechaInstruccionInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInstruccionInicial", SqlDbType.Date).Value = _fechaInstruccionInicial
                If _fechaInstruccionFinal <> Date.MinValue Then .SqlParametros.Add("@fechaInstruccionFinal", SqlDbType.Date).Value = _fechaInstruccionFinal
                If _fechaEnvioInicial <> Date.MinValue Then .SqlParametros.Add("@fechaEnvioInicial", SqlDbType.Date).Value = _fechaEnvioInicial
                If _fechaEnvioFinal <> Date.MinValue Then .SqlParametros.Add("@fechaEnvioFinal", SqlDbType.Date).Value = _fechaEnvioFinal

                _estructuraTablaConsulta = .ejecutarDataTable("ConsultarDatosReporteNoSap", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Function

#End Region

End Class
