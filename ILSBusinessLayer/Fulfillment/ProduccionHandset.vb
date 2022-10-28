Public Class ProduccionHandset
    Inherits FulfillmentBase

    Public Sub New()
        MyBase.new()
        CargarDatos()
    End Sub


#Region "Métodos Privados"
    Private Sub CargarDatos()
        RequiereSim = True
        RequierePin = True
        RequiereComprobarPallet = True
        ImprimeStickersCaja = False
    End Sub
#End Region

End Class
