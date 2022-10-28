Imports LMDataAccessLayer
Imports System.Web

Public Class RadicadosDevoluciones

#Region "Atributos (Filtros de Busqueda)"

    Dim _fechaInicialDevolucion As DateTime
    Dim _fechaFinalDevolucion As DateTime
    Dim _fechaInicialReagendamiento As DateTime
    Dim _fechaFinalReagendamiento As DateTime
    Dim _fechaInicialCierre As DateTime
    Dim _fechaFinalCierre As DateTime
    Dim _fechaInicialAgenda As DateTime
    Dim _fechaFinalAgenda As DateTime
    Dim _idCiudad As Integer
    Dim _idBodega As Integer
    Dim _msisdn As String
    Dim _radicado As String
    Dim _serial As String
    Dim _tipoServicio As Integer
#End Region

#Region "Propiedades"

    Public Property FechaInicialDevolucion() As DateTime
        Get
            Return _fechaInicialDevolucion
        End Get
        Set(ByVal value As DateTime)
            _fechaInicialDevolucion = value
        End Set
    End Property

    Public Property FechaFinalDevolucion() As DateTime
        Get
            Return _fechaFinalDevolucion
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalDevolucion = value
        End Set
    End Property

    Public Property FechaInicialReagendamiento() As DateTime
        Get
            Return _fechaInicialReagendamiento
        End Get
        Set(ByVal value As DateTime)
            _fechaInicialReagendamiento = value
        End Set
    End Property

    Public Property FechaFinalReagendamiento() As DateTime
        Get
            Return _fechaFinalReagendamiento
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalReagendamiento = value
        End Set
    End Property

    Public Property FechaInicialCierre() As DateTime
        Get
            Return _fechaInicialCierre
        End Get
        Set(ByVal value As DateTime)
            _fechaInicialCierre = value
        End Set
    End Property

    Public Property FechaFinalCierre() As DateTime
        Get
            Return _fechaFinalCierre
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalCierre = value
        End Set
    End Property

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

    Public Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property IdBodega() As Integer
        Get
            Return _idBodega
        End Get
        Set(ByVal value As Integer)
            _idBodega = value
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

    Public Property Serrial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
        End Set
    End Property

    Public Property TipoServicio() As Integer
        Get
            Return _tipoServicio
        End Get
        Set(ByVal value As Integer)
            _tipoServicio = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _msisdn = ""
        _serial = ""
        _radicado = ""
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ObtenerReporte() As DataSet
        Dim dtReporte As DataTable
        Dim dtReporteExtendido As DataTable
        Dim dsReporte As New DataSet
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .TiempoEsperaComando = 0
                If _fechaInicialDevolucion > Date.MinValue Then .SqlParametros.Add("@fechaInicialDevolucion", SqlDbType.SmallDateTime).Value = _fechaInicialDevolucion
                If _fechaFinalDevolucion > Date.MinValue Then .SqlParametros.Add("@fechaFinalDevolucion", SqlDbType.SmallDateTime).Value = _fechaFinalDevolucion
                If _fechaInicialReagendamiento > Date.MinValue Then .SqlParametros.Add("@fechaInicialReagendamiento", SqlDbType.SmallDateTime).Value = _fechaInicialReagendamiento
                If _fechaFinalReagendamiento > Date.MinValue Then .SqlParametros.Add("@fechaFinalReagendamiento", SqlDbType.SmallDateTime).Value = _fechaFinalReagendamiento
                If _fechaInicialCierre > Date.MinValue Then .SqlParametros.Add("@fechaInicialCierre", SqlDbType.SmallDateTime).Value = _fechaInicialCierre
                If _fechaFinalCierre > Date.MinValue Then .SqlParametros.Add("@fechaFinalCierre", SqlDbType.SmallDateTime).Value = _fechaFinalCierre
                If _fechaInicialAgenda > Date.MinValue Then .SqlParametros.Add("@fechaInicialAgenda", SqlDbType.SmallDateTime).Value = _fechaInicialAgenda
                If _fechaFinalAgenda > Date.MinValue Then .SqlParametros.Add("@fechaFinalAgenda", SqlDbType.SmallDateTime).Value = _fechaFinalAgenda
                If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                If Not String.IsNullOrEmpty(_msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 20).Value = _msisdn
                If Not String.IsNullOrEmpty(_radicado) Then .SqlParametros.Add("@radicado", SqlDbType.VarChar, 20).Value = _radicado
                If Not String.IsNullOrEmpty(_serial) Then .SqlParametros.Add("@serial", SqlDbType.VarChar, 20).Value = _serial
                If _tipoServicio > 0 Then .SqlParametros.Add("@tipoServicio", SqlDbType.Int).Value = _tipoServicio
                dtReporte = .ejecutarDataTable("ReporteRadicadosDevoluciones", CommandType.StoredProcedure)
                dtReporteExtendido = .ejecutarDataTable("ReporteRadicadosDevolucionesExtendido", CommandType.StoredProcedure)
            End With
            dtReporte.TableName = "dtReporte"
            dtReporteExtendido.TableName = "dtReporteExtendido"

            dsReporte.Tables.Add(dtReporte)
            dsReporte.Tables.Add(dtReporteExtendido)

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dsReporte
    End Function

    Public Function ObtenerDetalle() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                .TiempoEsperaComando = 0
                dtDatos = .ejecutarDataTable("ReporteRadicadosDevolucionesDetalle", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class
