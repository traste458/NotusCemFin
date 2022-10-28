Imports LMDataAccessLayer

Namespace InventarioFisico

    Public Class ReporteInactividadLineasDeInventario
#Region "Atributos/Filtros"

        Private _datos As DataTable
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
        End Sub

#End Region

#Region "Propiedades/Filtros"

        Public ReadOnly Property Datos As DataTable
            Get
                If _datos Is Nothing OrElse Not _cargado Then CargarInformacion()
                Return _datos
            End Get
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub CargarInformacion()

            Using dbManager As New LMDataAccess
                With dbManager
                    _datos = .EjecutarDataTable("GenerarInactividadDeLineasDeInventario", CommandType.StoredProcedure)
                    _cargado = True
                End With
            End Using

        End Sub

#End Region
    End Class

End Namespace