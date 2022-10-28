Imports LMDataAccessLayer
Imports System.Web

Public Class SalidaRutaMotorizados

#Region "Atributos (Filtros de Busqueda)"

    Dim _fechaInicialAgenda As DateTime
    Dim _fechaFinalAgenda As DateTime
    Dim _idCiudad As Integer
    Dim _idBodega As Integer
    Dim _msisdn As String
    Dim _radicado As String
    Dim _idJornada As Integer

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

    Public Property IdJornada() As Integer
        Get
            Return _idJornada
        End Get
        Set(ByVal value As Integer)
            _idJornada = value
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
                If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                If Not String.IsNullOrEmpty(_msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 20).Value = _msisdn
                If Not String.IsNullOrEmpty(_radicado) Then .SqlParametros.Add("@radicado", SqlDbType.VarChar, 20).Value = _radicado
                If _idJornada > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                dtReporte = .ejecutarDataTable("ReporteSalidaRutaMotorizado", CommandType.StoredProcedure)
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
