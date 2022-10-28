Imports LMDataAccessLayer
Imports System.Web

Public Class NovedadesCEM

#Region "Atributos (Filtros de Busqueda)"

    Dim _fechaInicialReporte As DateTime
    Dim _fechaFinalReporte As DateTime
    Dim _fechaInicialSolucion As DateTime
    Dim _fechaFinalSolucion As DateTime
    Dim _idCiudad As Integer
    Dim _idBodega As Integer
    Dim _msisdn As String
    Dim _radicado As String
    Dim _tipoServicio As Integer
    Dim _subproceso As Integer

#End Region

#Region "Propiedades"

    Public Property FechaInicialReporte() As DateTime
        Get
            Return _fechaInicialReporte
        End Get
        Set(ByVal value As DateTime)
            _fechaInicialReporte = value
        End Set
    End Property

    Public Property FechaFinalReporte() As DateTime
        Get
            Return _fechaFinalReporte
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalReporte = value
        End Set
    End Property

    Public Property FechaInicialSolucion() As DateTime
        Get
            Return _fechaInicialSolucion
        End Get
        Set(ByVal value As DateTime)
            _fechaInicialSolucion = value
        End Set
    End Property

    Public Property FechaFinalSolucion() As DateTime
        Get
            Return _fechaFinalSolucion
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalSolucion = value
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

    Public Property TipoServicio() As Integer
        Get
            Return _tipoServicio
        End Get
        Set(ByVal value As Integer)
            _tipoServicio = value
        End Set
    End Property

    Public Property Subproceso() As Integer
        Get
            Return _subproceso
        End Get
        Set(ByVal value As Integer)
            _subproceso = value
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
                If _fechaInicialReporte > Date.MinValue Then .SqlParametros.Add("@fechaInicialReporte", SqlDbType.SmallDateTime).Value = _fechaInicialReporte
                If _fechaFinalReporte > Date.MinValue Then .SqlParametros.Add("@fechaFinalReporte", SqlDbType.SmallDateTime).Value = _fechaFinalReporte
                If _fechaInicialSolucion > Date.MinValue Then .SqlParametros.Add("@fechaInicialSolucion", SqlDbType.SmallDateTime).Value = _fechaInicialSolucion
                If _fechaFinalSolucion > Date.MinValue Then .SqlParametros.Add("@fechaFinalSolucion", SqlDbType.SmallDateTime).Value = _fechaFinalSolucion
                If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                If Not String.IsNullOrEmpty(_msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 20).Value = _msisdn
                If Not String.IsNullOrEmpty(_radicado) Then .SqlParametros.Add("@radicado", SqlDbType.VarChar, 20).Value = _radicado
                If _tipoServicio > 0 Then .SqlParametros.Add("@tipoServicio", SqlDbType.Int).Value = _tipoServicio
                If _subproceso > 0 Then .SqlParametros.Add("@subproceso", SqlDbType.Int).Value = _subproceso
                .TiempoEsperaComando = 0
                dtReporte = .ejecutarDataTable("ReporteNovedadesCEM", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtReporte
    End Function

#End Region

End Class
