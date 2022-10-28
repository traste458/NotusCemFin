Imports System.Runtime.Serialization

Namespace CEMService

    Public Class InfoPedidoCEM
        <DataMember()> _
        Public Property NumeroPedidoSAP As Long
        <DataMember()> _
        Public Property NumeroEntregaSAP As Long
        <DataMember()> _
        Public Property NombreApellidoCliente As String
        <DataMember()> _
        Public Property NombreApellidoAutorizadoRecibir As String
        <DataMember()> _
        Public Property IdentificacionCliente As String
        <DataMember()> _
        Public Property DireccionEntrega As String
        <DataMember()> _
        Public Property ObservacionesDireccion As String
        <DataMember()> _
        Public Property Barrio As String
        <DataMember()> _
        Public Property CodigoCiudadEntrega As Integer
        <DataMember()> _
        Public Property TelefonoContacto1 As String
        <DataMember()> _
        Public Property TelefonoContacto2 As String
        <DataMember()> _
        Public Property FechaHoraAutorizacionCompra As DateTime
        <DataMember()> _
        Public Property DetalleArticulos As List(Of DetallePedidoCEM)

    End Class

End Namespace