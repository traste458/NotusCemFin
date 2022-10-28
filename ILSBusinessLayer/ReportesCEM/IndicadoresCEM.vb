Imports LMDataAccessLayer
Imports System.Web

Public Class IndicadoresCEM

#Region "Atributos (Filtros de Busqueda)"

    Dim _fechaInicialAgenda As DateTime
    Dim _fechaFinalAgenda As DateTime
    Dim _fechaCreacionInicial As DateTime
    Dim _fechaCreacionFinal As DateTime
    Dim _msisdn As String
    Dim _radicado As String
    Dim _idTipoServicio As Integer
    Dim _idEstado As Integer
    Dim _fechaAsignadoInicial As DateTime
    Dim _fechaAsignadoFinal As DateTime
    Dim _fechaTransitoInicial As DateTime
    Dim _fechaTransitoFinal As DateTime
    Dim _idCiudad As Integer
    Dim _idBodega As Integer
    Dim _documentoRes As String

#End Region

#Region "Propiedades"

    Public Property FechaInicialAgenda() As DateTime
        Get
            Return _fechaInicialAgenda
        End Get
        Set(ByVal value As DateTime)
            _fechaInicialAgenda = value
        End Set
    End Property

    Public Property FechaFinalAgenda() As DateTime
        Get
            Return _fechaFinalAgenda
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalAgenda = value
        End Set
    End Property
    Public Property FechaCreacionInicial() As DateTime
        Get
            Return _fechaCreacionInicial
        End Get
        Set(ByVal value As DateTime)
            _fechaCreacionInicial = value
        End Set
    End Property

    Public Property FechaCreacionFinal() As DateTime
        Get
            Return _fechaCreacionFinal
        End Get
        Set(ByVal value As DateTime)
            _fechaCreacionFinal = value
        End Set
    End Property

    Public Property Msisdn() As String
        Get
            Return _msisdn
        End Get
        Set(ByVal value As String)
            _msisdn = value
        End Set
    End Property

    Public Property Radicado() As String
        Get
            Return _radicado
        End Get
        Set(ByVal value As String)
            _radicado = value
        End Set
    End Property

    Public Property IdTipoServicio() As Integer
        Get
            Return _idTipoServicio
        End Get
        Set(ByVal value As Integer)
            _idTipoServicio = value
        End Set
    End Property

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property FechaAsignadoFinal As Date
        Get
            Return _fechaAsignadoFinal
        End Get
        Set(value As Date)
            _fechaAsignadoFinal = value
        End Set
    End Property

    Public Property FechaAsignadoInicial As Date
        Get
            Return _fechaAsignadoInicial
        End Get
        Set(value As Date)
            _fechaAsignadoInicial = value
        End Set
    End Property

    Public Property FechaTransitoInicial As Date
        Get
            Return _fechaTransitoInicial
        End Get
        Set(value As Date)
            _fechaTransitoInicial = value
        End Set
    End Property

    Public Property FechaTransitoFinal As Date
        Get
            Return _fechaTransitoFinal
        End Get
        Set(value As Date)
            _fechaTransitoFinal = value
        End Set
    End Property

    Public Property IdCiudad As Integer
        Get
            Return _idCiudad
        End Get
        Set(value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property IdBodega As Integer
        Get
            Return _idBodega
        End Get
        Set(value As Integer)
            _idBodega = value
        End Set
    End Property

    Public Property DocumentoRes As String
        Get
            Return _documentoRes
        End Get
        Set(value As String)
            _documentoRes = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _msisdn = ""
        _radicado = ""
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ObtenerReporte() As DataTable
        Dim dtReporte As DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _fechaInicialAgenda > Date.MinValue Then .SqlParametros.Add("@fechaInicialAgenda", SqlDbType.SmallDateTime).Value = _fechaInicialAgenda
                If _fechaFinalAgenda > Date.MinValue Then .SqlParametros.Add("@fechaFinalAgenda", SqlDbType.SmallDateTime).Value = _fechaFinalAgenda
                If _fechaCreacionInicial > Date.MinValue Then .SqlParametros.Add("@fechaCreacionInicial", SqlDbType.SmallDateTime).Value = _fechaCreacionInicial
                If _fechaCreacionFinal > Date.MinValue Then .SqlParametros.Add("@fechaCreacionFinal", SqlDbType.SmallDateTime).Value = _fechaCreacionFinal
                If Not String.IsNullOrEmpty(_msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 20).Value = _msisdn
                If Not String.IsNullOrEmpty(_radicado) Then .SqlParametros.Add("@radicado", SqlDbType.VarChar, 20).Value = _radicado
                If _idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                .TiempoEsperaComando = 0
                dtReporte = .EjecutarDataTable("ReporteIndicadoresCEM", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtReporte
    End Function
    Public Function ObtenerReporteIndicadoresCEM(ByVal nombreArchivo As String, ByVal nombrePlantilla As String) As InfoResultado
        Dim resul As New InfoResultado

        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _fechaInicialAgenda > Date.MinValue Then .SqlParametros.Add("@fechaInicialAgenda", SqlDbType.SmallDateTime).Value = _fechaInicialAgenda
                If _fechaFinalAgenda > Date.MinValue Then .SqlParametros.Add("@fechaFinalAgenda", SqlDbType.SmallDateTime).Value = _fechaFinalAgenda
                If _fechaCreacionInicial > Date.MinValue Then .SqlParametros.Add("@fechaCreacionInicial", SqlDbType.SmallDateTime).Value = _fechaCreacionInicial
                If _fechaCreacionFinal > Date.MinValue Then .SqlParametros.Add("@fechaCreacionFinal", SqlDbType.SmallDateTime).Value = _fechaCreacionFinal
                If Not String.IsNullOrEmpty(_msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 20).Value = _msisdn
                If Not String.IsNullOrEmpty(_radicado) Then .SqlParametros.Add("@radicado", SqlDbType.VarChar, 20).Value = _radicado
                If _idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                .TiempoEsperaComando = 0
                resul = .GenerarArchivoExcel("ReporteIndicadoresCEM", nombreArchivo, CommandType.StoredProcedure, nombrePlantilla, "Reporte Indicadores CEM", 4)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resul
    End Function

    Public Function ObtenerReporteSalidaMotorizado(ByVal nombreArchivo As String, ByVal nombrePlantilla As String) As InfoResultado
        Dim resul As New InfoResultado

        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _fechaInicialAgenda > Date.MinValue Then .SqlParametros.Add("@fechaInicialAgenda", SqlDbType.SmallDateTime).Value = _fechaInicialAgenda
                If _fechaFinalAgenda > Date.MinValue Then .SqlParametros.Add("@fechaFinalAgenda", SqlDbType.SmallDateTime).Value = _fechaFinalAgenda
                If FechaAsignadoInicial > Date.MinValue Then .SqlParametros.Add("@fechaAsignadoInicial", SqlDbType.SmallDateTime).Value = FechaAsignadoInicial
                If FechaAsignadoFinal > Date.MinValue Then .SqlParametros.Add("@fechaAsignadoFinal", SqlDbType.SmallDateTime).Value = FechaAsignadoFinal
                If FechaTransitoInicial > Date.MinValue Then .SqlParametros.Add("@fechaTransitoInicial", SqlDbType.SmallDateTime).Value = FechaTransitoInicial
                If FechaTransitoFinal > Date.MinValue Then .SqlParametros.Add("@fechaTransitoFinal", SqlDbType.SmallDateTime).Value = FechaTransitoFinal

                If Not String.IsNullOrEmpty(DocumentoRes) Then .SqlParametros.Add("@documentoResponsable", SqlDbType.VarChar, 20).Value = DocumentoRes
                If Not String.IsNullOrEmpty(_radicado) Then .SqlParametros.Add("@radicado", SqlDbType.VarChar, 20).Value = _radicado
                If IdCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = IdCiudad
                If IdBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = IdBodega
                .TiempoEsperaComando = 0

                resul = .GenerarArchivoExcel("ReporteSalidaDeMotorizados", nombreArchivo, CommandType.StoredProcedure, nombrePlantilla, "Reporte Salida Motorizados", 3)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resul
    End Function

#End Region

End Class
