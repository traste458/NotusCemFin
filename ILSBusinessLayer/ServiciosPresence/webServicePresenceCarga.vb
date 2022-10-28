Public Class webServicePresenceCarga
    Private wsClient As referenciaws.PcowsServiceClient
    Public Property ServiceId As Int64
    Public Property LoadId As Int64
    Public Property SourceId As Int64
    Public Property Name As String
    Public Property TimeZone As String
    Public Property Status As Int64
    Public Property Phone As String
    Public Property PhoneTimeZone As String
    Public Property AlternativePhones As String
    Public Property AlternativePhoneDescriptions As String
    Public Property AlternativePhoneTimeZones As String
    Public Property ScheduleDate As DateTime
    Public Property CapturingAgent As Int64
    Public Property Priority As Int64
    Public Property Comments As String
    Public Property CustomData1 As String
    Public Property CustomData2 As String
    Public Property CustomData3 As String
    Public Property CallerId As String
    Public Property CallerName As String
    Public Property AutomaticTimeZoneDetection As Boolean


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

            Result = wsClient.InsertOutboundRecord4(ServiceId, LoadId, SourceId, Name, TimeZone, Status, Phone, PhoneTimeZone, AlternativePhones, AlternativePhoneDescriptions,
                                                    AlternativePhoneTimeZones, ScheduleDate, CapturingAgent, Priority, Comments, CustomData1, CustomData2, CustomData3, CallerId, CallerName, False)

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
