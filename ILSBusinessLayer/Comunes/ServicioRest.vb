Imports System.Text
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json

Imports Newtonsoft.Json.Linq
Imports System.Security.Authentication
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports LMDataAccessLayer
Imports System.Threading.Tasks
Imports System.Threading

Public Class ServicioRest

#Region "Propiedades"
    Public Property url As String
    Public Property IdUsuario As Integer
    Public Property Token As String
    Public Property tbErrores As DataTable
#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos Publicos"
    Public Function ConsultarServicioRestGet(url As String, json As String, Optional tipo As String = "POST")

        Dim res As String = ""
        Dim resultado As ResultadoProceso = New ResultadoProceso
        Try
            Dim request As WebRequest = WebRequest.Create(url)
            request.ContentType = "application/json"
            request.Headers("Authorization") = Token
            request.Method = tipo

            Dim s As System.IO.Stream = request.GetResponse.GetResponseStream

            Using (s)
                Try
                    Dim sr As System.IO.StreamReader = New System.IO.StreamReader(s)
                    Using (sr)
                        Dim body As String = sr.ReadToEnd()
                        res = body
                        resultado.EstablecerMensajeYValor(1, body)
                    End Using
                Catch ex As Exception
                    resultado.EstablecerMensajeYValor(0, "Error al consultar el servicio: " & ex.Message)
                End Try
            End Using

        Catch e As WebException
            If (e.Status = WebExceptionStatus.ProtocolError) Then
                Dim response As WebResponse = e.Response
                Using (response)
                    Dim httpResponse As HttpWebResponse = CType(response, HttpWebResponse)
                    Try
                        Dim _Answer As StreamReader = New StreamReader(response.GetResponseStream())
                        Using (_Answer)
                            Dim body As String = _Answer.ReadToEnd()
                            resultado.EstablecerMensajeYValor(2, body)
                        End Using
                    Catch ex As Exception
                        resultado.EstablecerMensajeYValor(0, "Error al consultar el servicio: " & ex.Message)
                    End Try
                End Using
            End If

        End Try

        Return res
    End Function

    Public Function EliminarVisita(dsDatos As DataSet) As DataTable

        Try
            Dim resultado As New ResultadoEntidad
            Dim json As String
            Dim url As String = Comunes.ConfigValues.seleccionarConfigValue("URL_WSLOCAL_ELIMINACION_VISITA")


            'Validar si existen token vacios o invalidos
            Dim rows As DataRow()
            Dim dtResultadoSinToken As DataTable
            ' copia estructura de tabla 2 del dataset  a tabla resultado
            dtResultadoSinToken = dsDatos.Tables(0).Clone()
            ' filtra los datos de tabla 2 con el token recorrido en el ciclo
            rows = dsDatos.Tables(0).Select("tokenSimpliRoute='Token '")
            ' pasa las filas obtenidas a la tabla resultado
            If rows.Count > 0 Then
                CrearEstructuraTabla()
                For Each dr As DataRow In rows
                    dtResultadoSinToken.ImportRow(dr)
                    AgregarError("Pendiente configurar Token a Bodega: " & dr("bodega").ToString & "")
                Next
            End If


            If dtResultadoSinToken.Rows.Count = 0 Then
                json = JsonConvert.SerializeObject(dsDatos)
                resultado = consultarService(url, json)
                If resultado.Valor <> 1 Then
                    tbErrores = resultado.ResultadoNovedades
                Else
                    tbErrores = New DataTable
                End If
            End If

        Catch ex As Exception
            AgregarError("Error al consumir WS interno para notificar a SimpliRoute: " & ex.Message)
        End Try

        Return tbErrores
    End Function

    Public Function consultarService(url As String, json As String, Optional tipo As String = "POST") As ResultadoEntidad

        Dim resultadoEntidad As ResultadoEntidad = New ResultadoEntidad
        Try
            Dim request As WebRequest = WebRequest.Create(url)

            request.ContentType = "application/json"
            request.Method = tipo
            request.Headers("Authorization") = Token

            Dim encoding As New System.Text.UTF8Encoding()
            Dim bytes As Byte() = encoding.GetBytes(json)
            request.ContentLength = bytes.Length
            request.CachePolicy = New System.Net.Cache.RequestCachePolicy(Cache.RequestCacheLevel.NoCacheNoStore)


            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls


            Using requestStream As Stream = request.GetRequestStream()
                requestStream.Write(bytes, 0, bytes.Length)
                requestStream.Flush()
                requestStream.Close()
            End Using
            Dim myWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Dim _Answer As StreamReader = New StreamReader(myWebResponse.GetResponseStream())
            Using (_Answer)
                Dim body As String = _Answer.ReadToEnd()
                'resultado.EstablecerMensajeYValor(1, body)
                resultadoEntidad = JsonConvert.DeserializeObject(Of ResultadoEntidad)(body)
            End Using

        Catch e As WebException
            Dim errorResponse As HttpWebResponse = CType(e.Response, HttpWebResponse)

            If errorResponse.StatusCode = HttpStatusCode.NotFound Then
                CrearEstructuraTabla()
                AgregarError("Error al consumir WS: " & e.Message & "")
                resultadoEntidad.ResultadoNovedades = tbErrores
                resultadoEntidad.Valor = 0
                resultadoEntidad.Mensaje = "Error al consultar el servicio: " & e.Message
            Else

                If (e.Status = WebExceptionStatus.ProtocolError) Then
                    Dim response As WebResponse = e.Response
                    Using (response)
                        Dim httpResponse As HttpWebResponse = CType(response, HttpWebResponse)
                        Try
                            Dim _Answer As StreamReader = New StreamReader(response.GetResponseStream())
                            Using (_Answer)
                                Dim body As String = _Answer.ReadToEnd()
                                'resultado.EstablecerMensajeYValor(2, body)
                                resultadoEntidad = JsonConvert.DeserializeObject(Of ResultadoEntidad)(body)
                            End Using
                        Catch ex As Exception
                            resultadoEntidad.Valor = 0
                            resultadoEntidad.Mensaje = "Error al consultar el servicio: " & ex.Message
                            'resultado.EstablecerMensajeYValor(0, "Error al consultar el servicio: " & ex.Message)
                        End Try
                    End Using
                End If

            End If


        End Try

        Return resultadoEntidad
    End Function

    Public Function CreacionVisita(dsDatos As DataSet) As DataTable

        Try
            Dim resultado As New ResultadoEntidad
            Dim json As String
            Dim url As String = Comunes.ConfigValues.seleccionarConfigValue("URL_WSLOCAL_CREAR_VISITA")


            'Validar si existen token vacios o invalidos
            Dim rows As DataRow()
            Dim dtResultadoSinToken As DataTable
            ' copia estructura de tabla 2 del dataset  a tabla resultado
            dtResultadoSinToken = dsDatos.Tables(0).Clone()
            ' filtra los datos de tabla 2 con el token recorrido en el ciclo
            rows = dsDatos.Tables(0).Select("tokenSimpliRoute='Token '")
            ' pasa las filas obtenidas a la tabla resultado
            If rows.Count > 0 Then
                CrearEstructuraTabla()
                For Each dr As DataRow In rows
                    dtResultadoSinToken.ImportRow(dr)
                    AgregarError("Pendiente configurar Token a Bodega: " & dr("bodega").ToString & "")
                Next
            End If


            If dtResultadoSinToken.Rows.Count = 0 Then
                json = JsonConvert.SerializeObject(dsDatos)
                resultado = consultarService(url, json)
                If resultado.Valor <> 1 Then
                    tbErrores = resultado.ResultadoNovedades
                Else
                    tbErrores = New DataTable
                End If
            End If




        Catch ex As Exception
            AgregarError("Error al consumir WS interno para notificar a SimpliRoute: " & ex.Message)
        End Try

        Return tbErrores
    End Function

    Public Shared Async Function InvocarServicioRest(ByVal metodo As String, ByVal urlBase As String, ByVal api As String, ByVal Optional objeto As Object = Nothing, ByVal Optional autorizacion As AuthenticationHeaderValue = Nothing, ByVal Optional opcHeaders As HttpRequestHeaders = Nothing, ByVal Optional configAwait As Boolean = True) As Tasks.Task(Of ApiRestRespuesta)
        Dim res As ApiRestRespuesta
        Try
            If urlBase.Substring(urlBase.Length - 1) <> "/" Then
                urlBase += "/"
            End If
            Dim client As HttpClient = New HttpClient()
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 Or SecurityProtocolType.Tls11 Or SecurityProtocolType.Tls
            client.BaseAddress = New Uri(urlBase)
            client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite)
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

            If autorizacion IsNot Nothing Then
                client.DefaultRequestHeaders.Authorization = autorizacion
            End If

            If opcHeaders IsNot Nothing Then

                For Each hd In opcHeaders
                    client.DefaultRequestHeaders.TryAddWithoutValidation(hd.Key, hd.Value)
                Next
            End If

            Dim contenido As String = ""

            If objeto IsNot Nothing Then
                contenido = JsonConvert.SerializeObject(objeto)
            End If

            Dim buffer = System.Text.Encoding.UTF8.GetBytes(contenido)
            Dim byteContent = New ByteArrayContent(buffer)
            byteContent.Headers.ContentType = New MediaTypeHeaderValue("application/json")
            Dim response As HttpResponseMessage = Nothing

            Select Case metodo
                Case "POST"
                    response = Await client.PostAsync(api, byteContent).ConfigureAwait(configAwait)
                Case "PUT"
                    response = Await client.PutAsync(api, byteContent)
                Case "GET"
                    response = Await client.GetAsync(api)
            End Select

            If response.IsSuccessStatusCode Then
                res = New ApiRestRespuesta With {
                .Exitoso = True,
                .Mensaje = "Servicio invocado exitosamente"
            }
                res.Datos = Await response.Content.ReadAsStringAsync()
            Else
                res = New ApiRestRespuesta With {
                .Exitoso = False,
                .Mensaje = "Error al invocar servicio. Codigo Estado: " & response.StatusCode.ToString() & " Descripción: " + response.ReasonPhrase
            }
                res.Datos = Await response.Content.ReadAsStringAsync()
            End If

        Catch e As Exception
            res = New ApiRestRespuesta With {
            .Exitoso = False,
            .Mensaje = "Error al invocar servicio. " & e.Message
        }
        End Try

        Return res
    End Function


    Public Function AgregarRutas(dtDatos As DataTable) As ResultadoEntidad

        Dim resultado As New ResultadoEntidad
        Try

            Dim json As String
            Dim url As String = Comunes.ConfigValues.seleccionarConfigValue("URL_WSLOCAL_AGREGAR_RUTA")

            json = JsonConvert.SerializeObject(dtDatos)
            resultado = consultarService(url, json)

        Catch ex As Exception
            AgregarError("Error al consumir WS interno para notificar a SimpliRoute: " & ex.Message)
        End Try

        Return resultado
    End Function

    Public Sub WriteLog(ByVal TimeDate As Date, ByVal Message As String)

        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            db.SqlParametros.Clear()
            With db.SqlParametros

                .Add("@mensaje", SqlDbType.VarChar).Value = Message
            End With

            db.EjecutarNonQuery("RegistrarLogSimpliRoute", CommandType.StoredProcedure)
        Catch ex As Exception

        End Try

    End Sub

    Public Sub CrearEstructuraTabla()
        tbErrores = New DataTable()
        tbErrores.Columns.Add("Mensaje")
    End Sub

    Public Sub AgregarError(mensaje As String)
        Dim Renglon As DataRow = tbErrores.NewRow()
        Renglon("Mensaje") = mensaje
        tbErrores.Rows.Add(Renglon)


    End Sub

    Public Function actualizacionServicioRestPut(url As String, objectJson As Object, metodo As String, accion As String, id As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim objRespuestaRootObject As RespuestaRootObject = New RespuestaRootObject
        Dim res As String = ""

        'Se comvierte el objeto en formato json
        Dim json As String = JsonConvert.SerializeObject(objectJson)
        Const _Tls12 As SslProtocols = DirectCast(&HC00, SslProtocols)
        Const Tls12 As SecurityProtocolType = DirectCast(_Tls12, SecurityProtocolType)
        ServicePointManager.SecurityProtocol = Tls12

        Dim urlRequest As String = String.Format("{0}/{1}/{2}/{3}", url, metodo, accion, id)
        Dim request As HttpWebRequest = HttpWebRequest.Create(urlRequest)

        Try
            request.Method = "PUT"
            'se colocan los headers

            request.ContentType = "application/json"

            request.Headers("token") = ObtenerTokenServicioRest(url).GetAwaiter().GetResult()

            Dim encoding As System.Text.UTF8Encoding = New System.Text.UTF8Encoding()
            request.Timeout = 99999999
            Dim bytes As Byte() = encoding.GetBytes(json)
            request.ContentLength = bytes.Length
            request.CachePolicy = New System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
            Try
                'se envia el request 
                Using requestStream As Stream = request.GetRequestStream()
                    requestStream.Write(bytes, 0, bytes.Length)
                    requestStream.Flush()
                    requestStream.Close()
                End Using
            Catch e As Exception
                resultado.EstablecerMensajeYValor(0, "Error al consultar el servicio: " & e.Message)
            End Try
            'se obtiene la respuesta
            Dim myWebResponse As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Dim _Answer As StreamReader = New StreamReader(myWebResponse.GetResponseStream())

            'retorna resultado en formato json
            Using _Answer
                res = _Answer.ReadToEnd()
            End Using

            'se deserealiza la respuesta para convertirla en objetos visual basic
            objRespuestaRootObject = JsonConvert.DeserializeObject(Of RespuestaRootObject)(res)

            If objRespuestaRootObject.success Then
                resultado.Valor = 1
                resultado.Mensaje = objRespuestaRootObject.message & " _ " & objRespuestaRootObject.documents.errors

            End If

        Catch e As WebException
            Dim [error] As String
            [error] = e.Message
            'resultado.Mensaje = e.Message
            resultado.Valor = 0

            Dim _Answer As StreamReader = New StreamReader(e.Response.GetResponseStream())

            'retorna resultado en formato json
            Using _Answer
                res = _Answer.ReadToEnd()
            End Using
            RegistrarErrorLogIntegracionCRMServicioRest(request.Method, urlRequest, json, res)
            objRespuestaRootObject = JsonConvert.DeserializeObject(Of RespuestaRootObject)(res)
            If objRespuestaRootObject.message Is Nothing Then
                resultado.Mensaje = "Se generó un error al consumir el servicio del Orquestador: " + e.Message
            Else
                resultado.Mensaje = objRespuestaRootObject.message & " _ " & objRespuestaRootObject.documents.errors
            End If

        Finally

        End Try
        Return resultado

    End Function

    Public Shared Async Function ObtenerTokenServicioRest(ByVal urlBase As String, Optional ByVal metodo As String = "POST", ByVal Optional autorizacion As AuthenticationHeaderValue = Nothing) As Task(Of String)
        Dim res As RespuestaRootObject
        Try

            Dim email As String = Comunes.ConfigValues.seleccionarConfigValue("EMAIL_SERVICIO_REST_TOKEN")
            Dim password As String = Comunes.ConfigValues.seleccionarConfigValue("PASSWORD_SERVICIO_REST_TOKEN")

            If urlBase.Substring(urlBase.Length - 1) <> "/" Then
                urlBase += "/users/"
            End If

            Dim client As HttpClient = New HttpClient()
            client.BaseAddress = New Uri(urlBase)
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(New MediaTypeWithQualityHeaderValue("application/json"))

            If autorizacion IsNot Nothing Then
                client.DefaultRequestHeaders.Authorization = autorizacion
            End If

            Dim contenido As String = "{""email"":""" + email + """,""password"":""" + password + """}"

            Dim buffer = System.Text.Encoding.UTF8.GetBytes(contenido)
            Dim byteContent = New ByteArrayContent(buffer)
            byteContent.Headers.ContentType = New MediaTypeHeaderValue("application/json")
            Dim response As HttpResponseMessage = Nothing

            Select Case metodo
                Case "POST"
                    response = Await client.PostAsync("login", byteContent)
                Case "PUT"
                    response = Await client.PutAsync("login", byteContent)
                Case "GET"
                    response = Await client.GetAsync("login")
            End Select

            If response.IsSuccessStatusCode Then
                res = JsonConvert.DeserializeObject(Of RespuestaRootObject)(Await response.Content.ReadAsStringAsync())
            Else
                res = New RespuestaRootObject With {
                    .success = False,
                    .message = "Error al invocar servicio. Codigo Estado: " & response.StatusCode.ToString() & " Descripción: " + response.ReasonPhrase
                }
            End If

        Catch e As Exception
            res = New RespuestaRootObject With {
            .success = False,
            .message = "Error al invocar servicio. " & e.Message
        }
        End Try

        Return res.documents.token
    End Function

    Function RegistrarErrorLogIntegracionCRMServicioRest(ByVal requestType As String, ByVal urlServicio As String, ByVal bodyRequest As String, ByVal response As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim _dbManager As New LMDataAccess

        Try
            With _dbManager
                With .SqlParametros
                    .Add("@request_type", SqlDbType.VarChar).Value = requestType
                    .Add("@url_servicio", SqlDbType.VarChar).Value = urlServicio
                    .Add("@body_request", SqlDbType.VarChar).Value = bodyRequest
                    .Add("@response", SqlDbType.VarChar).Value = response
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .EjecutarNonQuery("RegistrarErrorLogIntegracionCRMServicioRest", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = resultado.Mensaje & .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(300, "No se logró establecer la respuesta del servidor.")
                End If
            End With
        Catch ex As Exception
            _dbManager.Dispose()
            resultado.EstablecerMensajeYValor(400, "Se presentó un error al generar el mensaje de confirmación: " & ex.Message)
        End Try
        Return resultado
    End Function


#End Region

End Class
