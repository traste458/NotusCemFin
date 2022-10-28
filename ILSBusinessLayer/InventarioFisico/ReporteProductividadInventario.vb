Imports LMDataAccessLayer

Namespace InventarioFisico

    Public Class ReporteProductividadInventario

#Region "Atributos/Filtros"

        Private _idRango As Short
        Private _datosProductividad As DataTable
        Private _datosGrafico As DataTable
        Private _datosGraficoCargados As Boolean
        Private _datosProductividadCargados As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            _datosGraficoCargados = False
            _idRango = 0
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdRango As Short
            Get
                Return _idRango
            End Get
            Set(value As Short)
                _idRango = value
            End Set
        End Property

        Public ReadOnly Property DatosProductividad() As DataTable
            Get
                If _datosProductividad Is Nothing OrElse Not _datosProductividadCargados Then CargarDatosProductividad()
                Return _datosProductividad
            End Get
        End Property

        Public ReadOnly Property DatosGrafico As DataTable
            Get
                If _datosGrafico Is Nothing OrElse Not _datosGraficoCargados Then CargarDatosGrafico()
                Return _datosGrafico
            End Get
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub CargarDatosGrafico()
            Using dbManager As New LMDataAccess
                With dbManager
                    _datosGrafico = .EjecutarDataTable("ObtenerGraficaProductividadInventarioFisico", CommandType.StoredProcedure)
                    _datosGraficoCargados = True
                End With
            End Using
        End Sub

        Public Sub CargarDatosProductividad()
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Add("@idRango", SqlDbType.SmallInt).Value = Me._idRango
                    _datosProductividad = .EjecutarDataTable("GenerarInformeProductividadInventarioFisico", CommandType.StoredProcedure)
                    _datosProductividadCargados = True
                End With
            End Using
        End Sub

#End Region

    End Class

End Namespace
