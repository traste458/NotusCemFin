Imports LMDataAccessLayer
Namespace Facturacion
    Public Class CentroCostoFacturacion

#Region "Variables"
        Private _idCentroCosto As Integer
        Private _nombre As String
#End Region

#Region "Propiedades"

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property IdCentroCosto() As Integer
            Get
                Return _idCentroCosto
            End Get
            Set(ByVal value As Integer)
                _idCentroCosto = value
            End Set
        End Property

#End Region


#Region "Metodos"

        Public Shared Function ObtenerListado() As DataTable
            Dim db As New LMDataAccess
            Dim dt As DataTable
            dt = db.ejecutarDataTable("ObtenerCentroCostoFacturacion", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Structure FiltroReporteFacturacion
            Dim idCentroCosto As Integer
            Dim idEvento As Integer
            Dim idTipoProducto As Integer
            Dim año As Integer
            Dim meses As ArrayList
        End Structure

#End Region

    End Class
End Namespace

