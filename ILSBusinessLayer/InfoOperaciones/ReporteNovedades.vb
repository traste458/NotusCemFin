Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.Drawing

Public Class ReporteNovedades

#Region "Atributos"
    Private _factura As String
    Private _idOrdenCompra As Integer
    Private _numeroOrdenCompra As String
    Private _fechaInicialRecepcion As Date
    Private _fechaFinalRecepcion As Date
    Private _fechaInicialProduccion As Date
    Private _fechaFinalProduccion As Date
    Private _sinSerial As Boolean
#End Region

#Region "Propiedades"

    Public Property Factura As String
        Get
            Return _factura
        End Get
        Set(value As String)
            _factura = value
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

    Public Property FechaInicialRecepcion As Date
        Get
            Return _fechaInicialRecepcion
        End Get
        Set(value As Date)
            _fechaInicialRecepcion = value
        End Set
    End Property

    Public Property FechaFinalRecepcion As Date
        Get
            Return _fechaFinalRecepcion
        End Get
        Set(value As Date)
            _fechaFinalRecepcion = value
        End Set
    End Property

    Public Property FechaInicialProduccion As Date
        Get
            Return _fechaInicialProduccion
        End Get
        Set(value As Date)
            _fechaInicialProduccion = value
        End Set
    End Property

    Public Property FechaFinalProduccion As Date
        Get
            Return _fechaFinalProduccion
        End Get
        Set(value As Date)
            _fechaFinalProduccion = value
        End Set
    End Property

    Public Property SinSerial As Boolean
        Get
            Return _sinSerial
        End Get
        Set(value As Boolean)
            _sinSerial = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos"

    Public Function ObtenerInformacionNovedad() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                .TiempoEsperaComando = 300
                If _factura <> Nothing Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                If _idOrdenCompra > 0 Then .SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = _idOrdenCompra
                If _numeroOrdenCompra IsNot Nothing Then .SqlParametros.Add("@ordenCompra", SqlDbType.VarChar).Value = _numeroOrdenCompra
                If _fechaInicialRecepcion <> Date.MinValue Then .SqlParametros.Add("@fechaInicialRecepcion", SqlDbType.Date).Value = _fechaInicialRecepcion
                If _fechaFinalRecepcion <> Date.MinValue Then .SqlParametros.Add("@fechaFinalRecepcion", SqlDbType.Date).Value = _fechaFinalRecepcion
                If _fechaInicialProduccion <> Date.MinValue Then .SqlParametros.Add("@fechaInicialProduccion", SqlDbType.Date).Value = _fechaInicialProduccion
                If _fechaFinalProduccion <> Date.MinValue Then .SqlParametros.Add("@fechaFinalProduccion", SqlDbType.Date).Value = _fechaFinalProduccion
                If SinSerial Then
                    dtResultado = .ejecutarDataTable("ObtenerInformacionDeReporteDeNovedadesSinSeriales", CommandType.StoredProcedure)
                Else
                    dtResultado = .ejecutarDataTable("ObtenerInformacionDeReporteDeNovedadesConSeriales", CommandType.StoredProcedure)
                End If

            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

#End Region

End Class