Imports LMDataAccessLayer
Imports System.Web

Public Class EstadoDespachos

#Region "Atributos (Filtros de Busqueda)"

    Dim _fechaInicialEntrega As DateTime
    Dim _fechaFinalEntrega As DateTime
    Dim _fechaInicialDespacho As DateTime
    Dim _fechaFinalDespacho As DateTime
    Dim _fechaInicialAsignacion As DateTime
    Dim _fechaFinalAsignacion As DateTime
    Dim _idCiudad As Integer
    Dim _idBodega As Integer
    Dim _msisdn As String
    Dim _serial As String
    Dim _radicado As String
    Dim _tipoServicio As Integer

#End Region

#Region "Propiedades"

    Public Property FechaInicialEntrega() As DateTime
        Get
            Return _fechaInicialEntrega
        End Get
        Set(ByVal value As DateTime)
            _fechaInicialEntrega = value
        End Set
    End Property

    Public Property FechaFinalEntrega() As DateTime
        Get
            Return _fechaFinalEntrega
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalEntrega = value
        End Set
    End Property

    Public Property FechaInicialDespacho() As DateTime
        Get
            Return _fechaInicialDespacho
        End Get
        Set(ByVal value As DateTime)
            _fechaInicialDespacho = value
        End Set
    End Property

    Public Property FechaFinalDespacho() As DateTime
        Get
            Return _fechaFinalDespacho
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalDespacho = value
        End Set
    End Property

    Public Property FechaInicialAsignacion() As DateTime
        Get
            Return _fechaInicialAsignacion
        End Get
        Set(ByVal value As DateTime)
            _fechaInicialAsignacion = value
        End Set
    End Property

    Public Property FechaFinalAsignacion() As DateTime
        Get
            Return _fechaFinalAsignacion
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalAsignacion = value
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

    Public Property Serrial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
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

    Public Function ObtenerReporte() As DataTable
        Dim dtReporte As DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _fechaInicialEntrega > Date.MinValue Then .SqlParametros.Add("@fechaInicialEntrega", SqlDbType.SmallDateTime).Value = _fechaInicialEntrega
                If _fechaFinalEntrega > Date.MinValue Then .SqlParametros.Add("@fechaFinalEntrega", SqlDbType.SmallDateTime).Value = _fechaFinalEntrega
                If _fechaInicialDespacho > Date.MinValue Then .SqlParametros.Add("@fechaInicialDespacho", SqlDbType.SmallDateTime).Value = _fechaInicialDespacho
                If _fechaFinalDespacho > Date.MinValue Then .SqlParametros.Add("@fechaFinalDespacho", SqlDbType.SmallDateTime).Value = _fechaFinalDespacho
                If _fechaInicialAsignacion > Date.MinValue Then .SqlParametros.Add("@fechaInicialAsignacion", SqlDbType.SmallDateTime).Value = _fechaInicialAsignacion
                If _fechaFinalAsignacion > Date.MinValue Then .SqlParametros.Add("@fechaFinalAsignacion", SqlDbType.SmallDateTime).Value = _fechaFinalAsignacion
                If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                If Not String.IsNullOrEmpty(_msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 20).Value = _msisdn
                If Not String.IsNullOrEmpty(_radicado) Then .SqlParametros.Add("@radicado", SqlDbType.VarChar, 20).Value = _radicado
                If Not String.IsNullOrEmpty(_serial) Then .SqlParametros.Add("@serial", SqlDbType.VarChar, 20).Value = _serial
                If _tipoServicio > 0 Then .SqlParametros.Add("@tipoServicio", SqlDbType.Int).Value = _tipoServicio
                dtReporte = .ejecutarDataTable("ReporteEstadoDespacho", CommandType.StoredProcedure)
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
