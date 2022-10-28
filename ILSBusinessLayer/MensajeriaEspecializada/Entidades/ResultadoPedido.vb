Imports System.Runtime.Serialization

Namespace CEMService
    Public Class ResultadoPedido

        <DataMember()> _
        Public Property CodigoResultado As Integer
        <DataMember()> _
        Public Property Mensaje As String
        <DataMember()> _
        Public Property NumeroDiasEntrega As Integer
        <DataMember()> _
        Public Property FechaEstimadaDeEntrega As DateTime

    End Class
End Namespace