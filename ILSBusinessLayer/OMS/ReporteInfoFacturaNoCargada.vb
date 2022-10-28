Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS

    Public Class ReporteInfoFacturaNoCargada

#Region "Atributos (Campos)"

        Private _idProducto As Integer
        Private _datosReporte As DataTable

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub
#End Region

#Region "Propiedades"

        Public ReadOnly Property DatosReporte() As DataTable
            Get
                If _datosReporte Is Nothing Then CargarDatos()
                Return _datosReporte
            End Get
        End Property

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = Me._idProducto
                    .TiempoEsperaComando = 600
                    _datosReporte = .ejecutarDataTable("ReporteInfoFacturaNoCargada", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

    End Class

End Namespace