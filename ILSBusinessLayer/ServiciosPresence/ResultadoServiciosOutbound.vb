Public Class ResultadoServiciosOutbound

    Public Property Code As Integer
    Public Property ErrorMessage As String
    Public Property Data As List(Of EstructuraGeneralServicioOutbound)

End Class

Public Class EstructuraGeneralServicioOutbound
    Public Property General As EstructuraInfoServicioOutbound

End Class

Public Class EstructuraInfoServicioOutbound
    Public Property Id As Integer
    Public Property Name As String
    Public Property OutboundType As Integer
    Public Property Status As String
    Public Property NoScheduleGap As String
    Public Property EnableSchedLimitDate As Boolean
    Public Property SchedulingLimitDated As DateTime
    Public Property ServiceHours As String
    Public Property StopReasonGroupId As Integer
    Public Property ResourceProfileId As Integer
    Public Property ServerId As Integer

End Class
