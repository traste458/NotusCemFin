Imports Newtonsoft.Json

Public Class PedidoDto
    Public Property numero_guia As String
    Public Property numero_pedido As Long
    Public Property estado_entrega As EstadoEntregaDto
    Public Property materiales As List(Of MaterialDto)
    Public Property transportador As TransportadorDto
    Public Property fecha_entrega_cliente As String
    Public Property fecha_reprogramacion As String
    Public Property fecha_solicitud As String
End Class
