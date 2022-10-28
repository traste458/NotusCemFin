Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO
Imports System.Web

Public Class PreventasNoExitosasReporte

#Region "Filtros de Búsqueda"

    Private _idCallCenter As Integer
    Private _fechaInicio As Date
    Private _fechaFinal As Date
    Private _idEstados As String

#End Region

#Region "Propiedades"

    Public Property IdCallCenter As Integer
        Get
            Return _idCallCenter
        End Get
        Set(value As Integer)
            _idCallCenter = value
        End Set
    End Property

    Public Property FechaInicio As Date
        Get
            Return _fechaInicio
        End Get
        Set(value As Date)
            _fechaInicio = value
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

    Public Property IdEstados As String
        Get
            Return _idEstados
        End Get
        Set(value As String)
            _idEstados = value
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
        Dim idUsuarioConsulta As Integer = 0

        With dbManager

            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                If Not EsNuloOVacio(HttpContext.Current.Session("usxp001")) Then Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuarioConsulta)
            End If
            If _idCallCenter > 0 Then .SqlParametros.Add("@idCallCenter", SqlDbType.Int).Value = _idCallCenter
            If Not String.IsNullOrEmpty(_idEstados) Then .SqlParametros.Add("@idEstados", SqlDbType.VarChar, 200).Value = _idEstados
            If Not _fechaInicio.Equals(Date.MinValue) Then _
                    .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
            If Not _fechaFinal.Equals(Date.MinValue) Then _
                    .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
            If idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuarioConsulta

            dtReporte = .EjecutarDataTable("ReportePreventasNoExitosas", CommandType.StoredProcedure)
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
        Return dtReporte
    End Function

#End Region

End Class
