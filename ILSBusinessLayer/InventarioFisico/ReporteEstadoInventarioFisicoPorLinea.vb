Imports LMDataAccessLayer

Namespace InventarioFisico
    Public Class ReporteEstadoInventarioFisicoPorLinea

#Region "Atributos/Filtros"

        Private _idBodega As Integer
        Private _datos As DataTable
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New(ByVal idBodega As Integer)
            _idBodega = idBodega
        End Sub

#End Region

#Region "Propiedades/Filtros"

        Public Property IdBodega As Integer
            Get
                Return _idBodega
            End Get
            Set(value As Integer)
                _idBodega = value
            End Set
        End Property

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
                    .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = Me._idBodega
                    _datos = .EjecutarDataTable("GenerarInformeEstadoInventarioFisicoPorLinea", CommandType.StoredProcedure)
                    _cargado = True
                End With
            End Using

        End Sub

#End Region

    End Class
End Namespace