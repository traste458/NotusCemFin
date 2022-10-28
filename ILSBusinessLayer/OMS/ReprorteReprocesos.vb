Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO

Public Class ReprorteReprocesos

#Region "Filtros de Búsqueda"

    Private _idInstruccionReproceso As ArrayList
    Private _ordenReproceso As ArrayList
    Private _materiales As ArrayList
    Private _idCreador As Integer
    Private _fechaInicio As Date
    Private _fechaFinal As Date
    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdInstruccionReproceso() As ArrayList
        Get
            If _idInstruccionReproceso Is Nothing Then _idInstruccionReproceso = New ArrayList
            Return _idInstruccionReproceso
        End Get
        Set(ByVal value As ArrayList)
            _idInstruccionReproceso = value
        End Set
    End Property

    Public Property OrdenReproceso() As ArrayList
        Get
            If _ordenReproceso Is Nothing Then _ordenReproceso = New ArrayList
            Return _ordenReproceso
        End Get
        Set(ByVal value As ArrayList)
            _ordenReproceso = value
        End Set
    End Property

    Public Property Materiales() As ArrayList
        Get
            If _materiales Is Nothing Then _materiales = New ArrayList
            Return _materiales
        End Get
        Set(ByVal value As ArrayList)
            _materiales = value
        End Set
    End Property

    Public Property IdCreador As Integer
        Get
            Return _idCreador
        End Get
        Set(value As Integer)
            _idCreador = value
        End Set
    End Property

    Public Property FechaInicio() As Date
        Get
            Return _fechaInicio
        End Get
        Set(ByVal value As Date)
            _fechaInicio = value
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


#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function GenerarReporte() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtReporte As New DataTable

        With dbManager
            If _idInstruccionReproceso IsNot Nothing AndAlso _idInstruccionReproceso.Count > 0 Then _
                        .SqlParametros.Add("@listaIdInstruccionReproceso", SqlDbType.VarChar).Value = Join(_idInstruccionReproceso.ToArray(), ",")
            If _materiales IsNot Nothing AndAlso _materiales.Count > 0 Then _
                        .SqlParametros.Add("@listaMateriales", SqlDbType.VarChar).Value = Join(_materiales.ToArray(), ",")
            If _ordenReproceso IsNot Nothing AndAlso _ordenReproceso.Count > 0 Then _
                        .SqlParametros.Add("@listaOrdenes", SqlDbType.VarChar).Value = Join(_ordenReproceso.ToArray(), ",")
            If _idCreador > 0 Then .SqlParametros.Add("@idCreador", SqlDbType.Int).Value = _idCreador
            If Not _fechaInicio.Equals(Date.MinValue) Then _
                    .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
            If Not _fechaFinal.Equals(Date.MinValue) Then _
                    .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
            dtReporte = .ejecutarDataTable("ReporteDeReprocesos", CommandType.StoredProcedure)
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
        Return dtReporte
    End Function

#End Region

End Class
