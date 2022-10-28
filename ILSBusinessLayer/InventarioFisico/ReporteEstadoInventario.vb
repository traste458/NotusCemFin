Imports LMDataAccessLayer

Namespace InventarioFisico

    Public Class ReporteEstadoInventario

#Region "Atributos/Filtros"
        Private _datos As DataTable
        Private _cargado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            _cargado = False
        End Sub

#End Region

#Region "Propiedades/Filtros"

        Public Property Estado As String
        Public Property TipoProducto As String
        Public Property Fabricante As String

        Public ReadOnly Property Datos As DataTable
            Get
                If _datos Is Nothing OrElse Not _cargado Then ConsultarInformacion()
                Return _datos
            End Get
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub ConsultarInformacion()
            Using dbManager As New LMDataAccess
                With dbManager

                    If Not EsNuloOVacio(Me._Estado) Then .SqlParametros.Add("@estado", SqlDbType.VarChar, 20).Value = Me._Estado.Trim
                    If Not EsNuloOVacio(Me._TipoProducto) Then .SqlParametros.Add("@tipoProducto", SqlDbType.VarChar, 50).Value = Me._TipoProducto.Trim
                    If Not EsNuloOVacio(Me._Fabricante) Then .SqlParametros.Add("@fabricante", SqlDbType.VarChar, 100).Value = Me._Fabricante.Trim
                    _datos = .EjecutarDataTable("ObtenerAvanceInventarioFisico")
                    _cargado = True
                End With
            End Using
        End Sub

#End Region

    End Class

End Namespace