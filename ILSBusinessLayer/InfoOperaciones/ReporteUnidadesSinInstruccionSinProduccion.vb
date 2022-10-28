Imports LMDataAccessLayer

Public Class ReporteUnidadesSinInstruccionSinProduccion

#Region "Atributos"
    Private _idOrdenRecepcion As Integer
    Private _idOrdenCompra As Double
    Private _numeroOrdenCompra As String
    Private _idTipoProducto As Integer
    Private _estado As Integer
    Private _idProducto As Integer
    Private _fechaInicial As Date
    Private _fechaFinal As Date
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

    Public Property IdOrdenCompra As Double
        Get
            Return _idOrdenCompra
        End Get
        Set(value As Double)
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

    Public Property IdProducto As Integer
        Get
            Return _idProducto
        End Get
        Set(value As Integer)
            _idProducto = value
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

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos Publicos"

    Public Function ObtenerInformacionUnidades() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                .TiempoEsperaComando = 300
                If _idOrdenRecepcion > 0 Then .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                If _idOrdenCompra > 0 Then .SqlParametros.Add("@idOrdenCompra", SqlDbType.Decimal).Value = _idOrdenCompra
                If _numeroOrdenCompra IsNot Nothing Then .SqlParametros.Add("@ordenCompra", SqlDbType.VarChar).Value = _numeroOrdenCompra
                If _idTipoProducto > 0 Then .SqlParametros.Add("@tipoProducto", SqlDbType.Int).Value = _idTipoProducto
                If _estado > 0 Then .SqlParametros.Add("@estado", SqlDbType.Int).Value = _estado
                If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = _fechaInicial
                If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = _fechaFinal
                dtResultado = .ejecutarDataTable("ObtenerInformacionDeReporteSinInstruccionSinProduccion", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

#End Region

End Class
