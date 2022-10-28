Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports GemBox.Spreadsheet
Imports System.Web
Imports System.IO


Public Class ReporteDeTransmisionesTcc

#Region "Atributos (Campos)"

    Private _codigoCuenta As Integer
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _arrListaEntrega As ArrayList
    Private _datosReporte As DataTable
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _arrListaEntrega = New ArrayList
        _cargado = False
    End Sub

    Public Sub New(ByVal codigoCuenta As String)
        Me.New()
        _codigoCuenta = codigoCuenta
    End Sub

#End Region

#Region "Propiedades"

    Public Property CodigoCuenta() As String
        Get
            Return _codigoCuenta
        End Get
        Set(ByVal value As String)
            _codigoCuenta = value
        End Set
    End Property

    Public Property FechaInicial() As Date
        Get
            Return _fechaInicial
        End Get
        Set(ByVal value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal() As Date
        Get
            Return _fechaFinal
        End Get
        Set(ByVal value As Date)
            _fechaFinal = value
        End Set
    End Property

    Public Property ListaEntregas() As ArrayList
        Get
            If _arrListaEntrega Is Nothing Then _arrListaEntrega = New ArrayList
            Return _arrListaEntrega
        End Get
        Set(ByVal value As ArrayList)
            _arrListaEntrega = value
        End Set
    End Property

    Public ReadOnly Property DatosReporte() As DataTable
        Get
            If _datosReporte Is Nothing OrElse _cargado = False Then CargarDatos()
            Return _datosReporte
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Public Sub CargarDatos()
        If Not String.IsNullOrEmpty(_codigoCuenta) Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@codigoCuenta", SqlDbType.Int).Value = _codigoCuenta
                    If _fechaInicial > Date.MinValue And _fechaFinal > Date.MinValue Then
                        .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                        .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                    End If
                    If _arrListaEntrega IsNot Nothing AndAlso _arrListaEntrega.Count > 0 Then _
                    .SqlParametros.Add("@listadoEntregas", SqlDbType.VarChar).Value = Join(_arrListaEntrega.ToArray, ",")
                    .TiempoEsperaComando = 600
                    _datosReporte = .ejecutarDataTable("ReporteTransmisionesTCC", CommandType.StoredProcedure)
                    _cargado = True
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            Throw New Exception("No se han propocionado todos los filtros requeridos para generar el reporte.")
        End If
    End Sub

    Public Function GenerarReporteEnArchivoDeTexto() As ResultadoProceso
        If _datosReporte Is Nothing OrElse _cargado = False Then CargarDatos()
        Dim resultado As New ResultadoProceso
        Dim rutaArchivo As String
        Dim swFileManager As StreamWriter

        With HttpContext.Current
            rutaArchivo = .Server.MapPath("~/archivos_planos/R" & _codigoCuenta & "_" & .Session("usxp001") & ".txt")
        End With

        resultado.EstablecerMensajeYValor(1, "Imposible Generar el archivo. Por favor intente nuevamente.")
        Try
            swFileManager = File.CreateText(rutaArchivo)
            For Each drAux As DataRow In _datosReporte.Rows
                swFileManager.WriteLine(drAux("transmision").ToString)
            Next

            If System.IO.File.Exists(rutaArchivo) Then resultado.EstablecerMensajeYValor(0, rutaArchivo)
        Finally
            If swFileManager IsNot Nothing Then
                swFileManager.Close()
                swFileManager.Dispose()
            End If
        End Try
        
        Return resultado
    End Function

#End Region
End Class
