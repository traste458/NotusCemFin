Public Interface IFulfillment

    Property RequiereComprobarPallet() As Boolean
    Property RequiereSim() As Boolean
    Property RequierePin() As Boolean
    Property ImprimeStickersCaja() As Boolean

    Function RequierePallet() As Boolean


End Interface