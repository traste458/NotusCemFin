Public Class RespuestaRootObject
    Public Property success As Boolean
    Public Property origen As String
    Public Property message As String
    Public Property documents As DocumentsDto

End Class

Public Class ApiRestRespuesta
    Public Property Exitoso As Boolean
    Public Property Mensaje As String
    Public Property Datos As Object
    Public Property ModeloNoValido As Boolean
End Class

Public Class ApiHeaders
    Public Property HeaderName As String
    Public Property HeaderValue As String
End Class