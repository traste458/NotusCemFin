Public Class FulfillmentFactory

    Public Sub New()
        MyBase.New()
    End Sub

    Public Function Fabricar(ByVal tipoInstancia As String) As IFulfillment
        Select Case tipoInstancia
            Case "LECTIMEI"
                Return New LecturaHandset
            Case "PRODIMEI"
                Return New ProduccionHandset
            Case "REPRIMEI"
                Return New ReprocesosHandset
            Case "LECTREP"
                Return New ReprocesosHandset
            Case "LECTNC"
                Return Nothing
            Case "LECTSC"
                Return Nothing
            Case "LECTPP"
                Return Nothing
        End Select
        Return Nothing
    End Function

End Class

