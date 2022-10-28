Public Class EntregaDto
    Public Property id_process As String
    Public Property orden As OrdenDto
    Public Property alistamiento As AlistamientoDto
    Public Property pedidos As List(Of PedidoDto)
    Public Property observacion As String
End Class
