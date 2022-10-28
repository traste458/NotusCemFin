Namespace OMS

    Public Class MaterialTermosellado

#Region "Variables"
        Private _idMaterial As Integer
        Private _Descripcion As String
#End Region

#Region "Propiedades"

        Public ReadOnly Property IdMaterial() As Integer
            Get
                Return _idMaterial
            End Get

        End Property

        Public Property Descricion() As String
            Get
                Return _Descripcion
            End Get
            Set(ByVal value As String)
                _Descripcion = value
            End Set

        End Property

#End Region

#Region "Metodos"
        Public Shared Function ObtenerListado() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerMaterialTermosellado", CommandType.StoredProcedure)
            Return dt
        End Function
#End Region
    End Class
End Namespace
