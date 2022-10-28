Imports LMDataAccessLayer
Public Class CrearOTBSimsEnGuacal

#Region "Atributos"
    Private _idOrden As Integer
#End Region

#Region "Propiedades"
    Public Property IdOrden As Integer
        Get
            Return _idOrden
        End Get
        Set(value As Integer)
            _idOrden = value
        End Set
    End Property
#End Region

#Region "Metodos Publicos"
    Public Function ObtenerGuacales() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                .TiempoEsperaComando = 0
                If _idOrden > 0 Then .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = _idOrden
                dtResultado = .ejecutarDataTable("ObtenerInformacionDeHuacalesEnOtb", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function
#End Region
End Class
