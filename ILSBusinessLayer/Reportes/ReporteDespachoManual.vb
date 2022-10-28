Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class ReporteDespachoManual

#Region "Atributos (Campos)"

    Private _listaDestinatario As ArrayList
    Private _fechaDespachoIni As Date
    Private _fechaDespachoFin As Date
    Private _dtReporteDetallado As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _listaDestinatario = New ArrayList
    End Sub

#End Region

#Region "Propiedades"

    Public Property ListaDestinatario() As ArrayList
        Get
            If _listaDestinatario Is Nothing Then _listaDestinatario = New ArrayList
            Return _listaDestinatario
        End Get
        Set(ByVal value As ArrayList)
            _listaDestinatario = value
        End Set
    End Property

    Public Property FechaDespachoInicial() As Date
        Get
            Return _fechaDespachoIni
        End Get
        Set(ByVal value As Date)
            _fechaDespachoIni = value
        End Set
    End Property

    Public Property FechaDespachoFinal() As Date
        Get
            Return _fechaDespachoFin
        End Get
        Set(ByVal value As Date)
            _fechaDespachoFin = value
        End Set
    End Property

    Public ReadOnly Property DatosReporteDetallado() As DataTable
        Get
            If _dtReporteDetallado Is Nothing Then CargarDatosReporte()
            Return _dtReporteDetallado
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatosReporte()
        Dim dbManager As New LMDataAccess

        Try
            With dbManager

                If _listaDestinatario IsNot Nothing AndAlso _listaDestinatario.Count > 0 Then _
                    .SqlParametros.Add("@listaIdDestinatario", SqlDbType.VarChar, 8000).Value = Join(_listaDestinatario.ToArray, ",")
                If _fechaDespachoIni > Date.MinValue AndAlso _fechaDespachoFin > Date.MinValue Then
                    .SqlParametros.Add("@fechaDespachoIni", SqlDbType.SmallDateTime).Value = _fechaDespachoIni
                    .SqlParametros.Add("@fechaDespachoFin", SqlDbType.SmallDateTime).Value = _fechaDespachoFin
                End If
                _dtReporteDetallado = .ejecutarDataTable("ReporteDespachosManuales", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region



End Class
