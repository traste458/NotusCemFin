Imports System.Runtime.Serialization

Namespace CEMService

    Public Class DetallePedidoCEM

        <DataMember()> _
        Public Property CodigoMaterialSAPEquipo As Decimal
        <DataMember()> _
        Public Property CantidadEquipos As Short
        <DataMember()> _
        Public Property CodigoMaterialSAPSim As Decimal
        <DataMember()> _
        Public Property CantidadSims As Short

    End Class
End Namespace