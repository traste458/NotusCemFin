Public Class WebServicesPresence
    Private wsClient As referenciaws.PcowsServiceClient


    Public Property ServiceId As Int64
    Public Property LoadId As Int64
    Public Property Status As Int64
    Public Property CapturingAgent As Int64 = 0
    Public Property Priority As Int64 = 10
    Public Property Name As String
    Public Property Phone As String
    Public Property Comments As String = ""
    Public Property TelefonoAlternativo As String = ""
    Public Property tbIdentificacion As String = ""
    Public Property tbEmail As String = ""


    Private Sub InicialiarServicio(ByVal urlServicio As String)
        wsClient = New referenciaws.PcowsServiceClient()
        wsClient.Endpoint.Address = New System.ServiceModel.EndpointAddress(urlServicio)

    End Sub
    Public Function InsertarRegistroOutbound(ByVal urlServicio As String) As String
        Dim mensaje As String = ""

        Try
            InicialiarServicio(urlServicio)
            Dim rnd As Random = New Random(Environment.TickCount)
            Dim SourceId As Integer = rnd.[Next](9999999)
            Dim ScheduleDate As DateTime = DateTime.Now
            Dim Result As Integer = 0

            Result = wsClient.InsertOutboundRecord4(ServiceId, LoadId, SourceId, Name, "", Status, Phone, "", TelefonoAlternativo, "1,2,3,4,5,1,2,3,4", "", ScheduleDate, CapturingAgent, Priority, Comments, tbIdentificacion, tbEmail, "", "", "", False)

            If Result = 0 Then
                mensaje = "OK"

            Else
                mensaje = "No se ha podido insertar el registro de emisión: " & wsClient.GetErrorMessage(Result)

            End If
            Return mensaje

        Catch e As Exception
            Return "Ocurrio un error con el llamado del servicio" & e.Message
        End Try
    End Function

End Class
