Imports System.IO
Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class ReporteDetalladoNotificacionesCEM

#Region "Filtros de Búsqueda"

        Private _numeroRadicado As ArrayList
        Private _ciudad As ArrayList
        Private _bodega As ArrayList
        Private _tipoNotificacion As ArrayList
        Private _estado As ArrayList
        Private _fechaInicio As Date
        Private _fechaFin As Date

#End Region

#Region "Propiedades"

        Public Property NumeroRadicado As ArrayList
            Get
                If _numeroRadicado Is Nothing Then _numeroRadicado = New ArrayList
                Return _numeroRadicado
            End Get
            Set(value As ArrayList)
                _numeroRadicado = value
            End Set
        End Property

        Public Property Ciudad As ArrayList
            Get
                If _ciudad Is Nothing Then _ciudad = New ArrayList
                Return _ciudad
            End Get
            Set(value As ArrayList)
                _ciudad = value
            End Set
        End Property

        Public Property Bodega As ArrayList
            Get
                If _bodega Is Nothing Then _bodega = New ArrayList
                Return _bodega
            End Get
            Set(value As ArrayList)
                _bodega = value
            End Set
        End Property

        Public Property TipoNotificacion As ArrayList
            Get
                If _tipoNotificacion Is Nothing Then _tipoNotificacion = New ArrayList
                Return _tipoNotificacion
            End Get
            Set(value As ArrayList)
                _tipoNotificacion = value
            End Set
        End Property

        Public Property Estado As ArrayList
            Get
                If _estado Is Nothing Then _estado = New ArrayList
                Return _estado
            End Get
            Set(value As ArrayList)
                _estado = value
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

        Public Property FechaFin As Date
            Get
                Return _fechaFin
            End Get
            Set(value As Date)
                _fechaFin = value
            End Set
        End Property

#End Region

        Public Function ConsultarReporteDetallado() As DataSet
            Dim dsDatos As New DataSet
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        If _numeroRadicado IsNot Nothing AndAlso _numeroRadicado.Count > 0 Then _
                            .Add("@numeroRadicado", SqlDbType.VarChar).Value = Join(_numeroRadicado.ToArray(), ",")
                        If _ciudad IsNot Nothing AndAlso _ciudad.Count > 0 Then _
                            .Add("@ciudad", SqlDbType.VarChar).Value = Join(_ciudad.ToArray(), ",")
                        If _bodega IsNot Nothing AndAlso _bodega.Count > 0 Then _
                            .Add("@bodega", SqlDbType.VarChar).Value = Join(_bodega.ToArray(), ",")
                        If _estado IsNot Nothing AndAlso _estado.Count > 0 Then _
                            .Add("@estado", SqlDbType.VarChar).Value = Join(_estado.ToArray(), ",")
                        If _fechaInicio > Date.MinValue Then .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                        If _fechaFin > Date.MinValue Then .Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFin
                    End With
                    dsDatos = .EjecutarDataSet("ReporteDetalladoNotificacionesCEM", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dsDatos
        End Function

    End Class

End Namespace