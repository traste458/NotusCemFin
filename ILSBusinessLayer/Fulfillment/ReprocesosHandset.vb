Public Class ReprocesosHandset
    Inherits FulfillmentBase

#Region "Constructores"

    Public Sub New()
        MyBase.new()
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        RequiereSim = True
        RequierePin = False
        RequiereComprobarPallet = False
        ImprimeStickersCaja = True
    End Sub

#End Region

End Class
