Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Net.Security
Imports System.Security.Authentication
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Public Module RestApiManager

    Public Async Function GetServicioRest(ByVal urlBase As String, ByVal api As String,
                                          Optional autorizacion As AuthenticationHeaderValue = Nothing,
                                          Optional objeto As Object = Nothing) As Task(Of ResultadoApiRest)

        Dim resultado As New ResultadoApiRest

        Dim cliente As HttpClient = New HttpClient()

        'Const _Tls12 As SslProtocols = DirectCast(&HC00, SslProtocols)
        'Const Tls12 As SecurityProtocolType = DirectCast(_Tls12, SecurityProtocolType)
        'ServicePointManager.SecurityProtocol = Tls12

        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        cliente.BaseAddress = New Uri(urlBase)
        cliente.DefaultRequestHeaders.Accept.Clear()
        cliente.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        If autorizacion IsNot Nothing Then
            cliente.DefaultRequestHeaders.Authorization = autorizacion
        End If

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

        Dim respuesta As HttpResponseMessage = Await cliente.GetAsync(api)
        If respuesta IsNot Nothing Then
            With resultado
                .StatusCode = respuesta.StatusCode
                .ReasonPhrase = respuesta.ReasonPhrase
            End With
        End If

        If respuesta.IsSuccessStatusCode Then
            With resultado
                .EsExitoso = True
                .JsonResp = Await respuesta.Content.ReadAsStringAsync()
            End With
        Else
            resultado.Mensaje = "Error al invocar servicio. Codigo Estado: " & respuesta.StatusCode.ToString() &
                " Descripción: " & respuesta.ReasonPhrase
        End If

        Return resultado

    End Function

    Public Async Function PostServicioRest(ByVal urlBase As String, ByVal api As String,
                                           Optional ByVal objeto As Object = Nothing,
                                          Optional autorizacion As AuthenticationHeaderValue = Nothing
                                          ) As Task(Of ResultadoApiRest)

        Dim resultado As New ResultadoApiRest

        Dim cliente As HttpClient = New HttpClient()

        'Const _Tls12 As SslProtocols = DirectCast(&HC00, SslProtocols)
        'Const Tls12 As SecurityProtocolType = DirectCast(_Tls12, SecurityProtocolType)
        'ServicePointManager.SecurityProtocol = Tls12

        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        cliente.BaseAddress = New Uri(urlBase)
        cliente.DefaultRequestHeaders.Accept.Clear()
        cliente.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        If autorizacion IsNot Nothing Then
            cliente.DefaultRequestHeaders.Authorization = autorizacion
        End If

        Dim contenido As String = ""
        If objeto IsNot Nothing Then contenido = JsonConvert.SerializeObject(objeto)

        Dim buffer = System.Text.Encoding.UTF8.GetBytes(contenido)
        Dim byteContent = New ByteArrayContent(buffer)
        byteContent.Headers.ContentType = New MediaTypeHeaderValue("application/json")

        Dim respuesta As HttpResponseMessage = Await cliente.PostAsync(api, byteContent)

        If respuesta IsNot Nothing Then
            With resultado
                .StatusCode = respuesta.StatusCode
                .ReasonPhrase = respuesta.ReasonPhrase
            End With
        End If

        If respuesta.IsSuccessStatusCode Then
            With resultado
                .EsExitoso = True
                .JsonResp = Await respuesta.Content.ReadAsStringAsync()
            End With
        Else
            resultado.Mensaje = "Error al invocar servicio. Codigo Estado: " & respuesta.StatusCode.ToString() &
                " Descripción: " & respuesta.ReasonPhrase
        End If

        Return resultado

    End Function

    Public Async Function PutServicioRest(ByVal urlBase As String, ByVal api As String,
                                           Optional ByVal objeto As Object = Nothing,
                                          Optional autorizacion As AuthenticationHeaderValue = Nothing
                                          ) As Task(Of ResultadoApiRest)

        Dim resultado As New ResultadoApiRest

        Dim cliente As HttpClient = New HttpClient()

        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        cliente.BaseAddress = New Uri(urlBase)
        cliente.DefaultRequestHeaders.Accept.Clear()
        cliente.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        If autorizacion IsNot Nothing Then
            cliente.DefaultRequestHeaders.Authorization = autorizacion
        End If

        Dim contenido As String = ""
        If objeto IsNot Nothing Then contenido = JsonConvert.SerializeObject(objeto)

        Dim buffer = System.Text.Encoding.UTF8.GetBytes(contenido)
        Dim byteContent = New ByteArrayContent(buffer)
        byteContent.Headers.ContentType = New MediaTypeHeaderValue("application/json")

        Dim respuesta As HttpResponseMessage = Await cliente.PutAsync(api, byteContent)

        If respuesta IsNot Nothing Then
            With resultado
                .StatusCode = respuesta.StatusCode
                .ReasonPhrase = respuesta.ReasonPhrase
            End With
        End If

        If respuesta.IsSuccessStatusCode Then
            With resultado
                .EsExitoso = True
                .JsonResp = Await respuesta.Content.ReadAsStringAsync()
            End With
        Else
            resultado.Mensaje = "Error al invocar servicio. Codigo Estado: " & respuesta.StatusCode.ToString() &
                " Descripción: " & respuesta.ReasonPhrase
        End If

        Return resultado

    End Function

    Public Async Function DELETEServicioRest(ByVal urlBase As String, ByVal api As String,
                                          Optional autorizacion As AuthenticationHeaderValue = Nothing,
                                          Optional objeto As Object = Nothing) As Task(Of ResultadoApiRest)

        Dim resultado As New ResultadoApiRest

        Dim cliente As HttpClient = New HttpClient()

        ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        cliente.BaseAddress = New Uri(urlBase)
        cliente.DefaultRequestHeaders.Accept.Clear()
        cliente.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))
        If autorizacion IsNot Nothing Then
            cliente.DefaultRequestHeaders.Authorization = autorizacion
        End If

        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls

        Dim respuesta As HttpResponseMessage = Await cliente.DeleteAsync(api)
        If respuesta IsNot Nothing Then
            With resultado
                .StatusCode = respuesta.StatusCode
                .ReasonPhrase = respuesta.ReasonPhrase
            End With
        End If

        If respuesta.IsSuccessStatusCode Then
            With resultado
                .EsExitoso = True
                .JsonResp = Await respuesta.Content.ReadAsStringAsync()
            End With
        Else
            resultado.Mensaje = "Error al invocar servicio. Codigo Estado: " & respuesta.StatusCode.ToString() &
                " Descripción: " & respuesta.ReasonPhrase
        End If

        Return resultado

    End Function

    Public Function ValidateRemoteCertificate(sender As Object, certificate As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

End Module
