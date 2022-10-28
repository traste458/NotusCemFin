Imports System
Imports System.Net
Imports System.Net.Http.Headers
Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Text

Public Class SoporteNotificacionTransportadora

    Private raizRutaAzure As New Comunes.ConfigValues("RUTA_RAIZ_AZURE_GUIA_TRANSPOTADORA")
    Private apiLogin As New Comunes.ConfigValues("API_LOGIN_GUIA_TRANSPOTADORA")
    Private apiSoporte As New Comunes.ConfigValues("API_SOPORTE_GUIA_TRANSPOTADORA")
    Private emailLogin As New Comunes.ConfigValues("EMAIL_GUIA_TRANSPOTADORA")
    Private passwordLogin As New Comunes.ConfigValues("PASSWORD_GUIA_TRANSPOTADORA")

    Public Function ObtenerSoporte(numeroGuia As String) As List(Of SoporteNotificacionEntidad)

        Dim listaSoporte As New List(Of SoporteNotificacionEntidad)

        Try

            If Not String.IsNullOrEmpty(numeroGuia) And numeroGuia IsNot Nothing Then
                Dim token As String = GenerarToken()

                Dim url As String = raizRutaAzure.ConfigKeyValue.ToString & apiSoporte.ConfigKeyValue.ToString & numeroGuia
                Dim client As WebClient = New WebClient()
                client.Headers.Add("Authorization", "Bearer " & token)
                Dim json = client.DownloadString(url)

                listaSoporte = JsonConvert.DeserializeObject(Of List(Of SoporteNotificacionEntidad))(json)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return listaSoporte
    End Function

    Private Function GenerarToken() As String

        Dim clientte = New HttpClient()

        Dim user As UserInfo = New UserInfo With {
                    .Email = emailLogin.ConfigKeyValue.ToString,
                    .Password = passwordLogin.ConfigKeyValue.ToString
                    }

        Dim strUser = JsonConvert.SerializeObject(user)
        Dim content = New StringContent(strUser.ToString(), Encoding.UTF8, "application/json")
        content.Headers.ContentType = New MediaTypeHeaderValue("application/json")

        Dim clienteHeader As HttpClient = New HttpClient()
        clienteHeader.BaseAddress = New Uri(raizRutaAzure.ConfigKeyValue.ToString)
        clienteHeader.DefaultRequestHeaders.Accept.Clear()
        clienteHeader.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

        Dim response As HttpResponseMessage = clienteHeader.PostAsync(apiLogin.ConfigKeyValue.ToString, content).Result
        Dim oToken As TokenInfo = Nothing

        If response.Content IsNot Nothing Then
            Dim responseContent As String = response.Content.ReadAsStringAsync().Result
            oToken = JsonConvert.DeserializeObject(Of TokenInfo)(responseContent)
        End If

        Return oToken.token.ToString
    End Function


End Class
