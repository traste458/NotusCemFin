Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO

Namespace MensajeriaEspecializada

    Public Class ReporteNotificacionesCEMDetalle

#Region "Atributos"

        Private _numeroRadicado As Long
        Private _material As String
        Private _descripcion As String
        Private _cantidad As Integer
        Private _fechaReporteNoDisponibilidad As String

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property NumeroRadicado As Long
            Get
                Return _numeroRadicado
            End Get
            Set(value As Long)
                _numeroRadicado = value
            End Set
        End Property

        Public Property Material As String
            Get
                Return _material
            End Get
            Set(value As String)
                _material = value
            End Set
        End Property

        Public Property Descripcion As String
            Get
                Return _descripcion
            End Get
            Set(value As String)
                _descripcion = value
            End Set
        End Property

        Public Property Cantidad As Integer
            Get
                Return _cantidad
            End Get
            Set(value As Integer)
                _cantidad = value
            End Set
        End Property

        Public Property FechaReporteNoDisponibilidad As String
            Get
                Return _fechaReporteNoDisponibilidad
            End Get
            Set(value As String)
                _fechaReporteNoDisponibilidad = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal numeroRadicado As Long)
            MyBase.New()
            _numeroRadicado = numeroRadicado
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@numeroRadicado", SqlDbType.Int).Value = CStr(_numeroRadicado)
                    .ejecutarReader("ReporteNotificacionesCEMDetalle", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Privados"

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("numeroRadicado"), _numeroRadicado)
                    If Not IsDBNull(reader("material")) Then _material = (reader("material").ToString)
                    If Not IsDBNull(reader("descripcion")) Then _descripcion = (reader("descripcion").ToString)
                    Integer.TryParse(reader("cantidad"), _cantidad)
                    If Not IsDBNull(reader("fechaReporteSinDisponibilidad")) Then _fechaReporteNoDisponibilidad = CDate(reader("fechaReporteSinDisponibilidad").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace