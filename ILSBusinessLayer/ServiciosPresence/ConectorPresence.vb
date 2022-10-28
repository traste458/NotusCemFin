Imports System.Configuration
Imports System.Net.Http.Headers
Imports System.Threading.Tasks
Imports System.Web
Imports LMDataAccessLayer
Imports Newtonsoft.Json
Imports ILSBusinessLayer

Public Class ConectorPresence

    Public Property Token As String
    Public Property UrlApi As String

    Public Property SesionIniciada As Boolean

    Public Sub New()

        ' UrlApi = ConfigurationManager.AppSettings("UrlApiPresence")
        SesionIniciada = False

        'If String.IsNullOrWhiteSpace(Me.UrlApi) Then Throw New Exception("No fue posible obtener la URL del API de Conexion de Presence. Por favor contacte al grupo de soporte de Desarrollo")
    End Sub

    Public Function GenerarTokenDeAcceso() As Boolean

        Dim resultado As Boolean = False

        Dim authInfo = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes("presenceapi:presenceapi"))
        Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Basic", authInfo)


        Dim r As ResultadoApiRest = Task.Run(Function() RestApiManager.GetServicioRest(UrlApi, "api/v1/token?expiration=0", auth)).Result

        If r IsNot Nothing AndAlso r.EsExitoso Then

            Dim resTk As ResultadoToken = JsonConvert.DeserializeObject(Of ResultadoToken)(r.JsonResp)

            Token = resTk.Data.Token
            If Not String.IsNullOrWhiteSpace(Token) Then resultado = True

        End If

        Return resultado

    End Function

    Public Function IniciarSesion() As Boolean

        Dim resultado As Boolean = False

        If String.IsNullOrWhiteSpace(Token) Then
            resultado = GenerarTokenDeAcceso()
        End If

        Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Bearer", Token)
        Dim info As InfoSesion = New InfoSesion()
        Dim I As String = info.Password
        Dim r As ResultadoApiRest = Task.Run(Function() RestApiManager.PostServicioRest(UrlApi, "api/v1/sessions/login", info, auth)).Result

        If r IsNot Nothing AndAlso r.EsExitoso Then
            resultado = True
            SesionIniciada = True
        Else
            resultado = False
        End If

        Return resultado

    End Function

    Public Function CerrarSesion() As Boolean

        Dim resultado As Boolean = False

        If String.IsNullOrWhiteSpace(Token) Then Throw New Exception("No se puede cerrar sesión. Debe contactar a Infraestructura IT")

        Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Bearer", Token)

        Dim r As ResultadoApiRest = Task.Run(Function() RestApiManager.PostServicioRest(UrlApi, "api/v1/sessions/logout", Nothing, auth)).Result

        If r IsNot Nothing AndAlso r.EsExitoso Then
            resultado = True
            SesionIniciada = False
        End If

        Return resultado

    End Function

    Public Function ObtenerServiciosOutbound(Optional ByVal id As Integer = 0) As ResultadoServiciosOutbound

        Dim resultado As ResultadoServiciosOutbound = Nothing
        Try
            If IniciarSesion() Then
                Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Bearer", Token)

                Dim api As String = "api/v2/services/outbound"
                If id > 0 Then api = "api/v5/services/outbound/" & id.ToString


                Dim r As ResultadoApiRest = Task.Run(Function() RestApiManager.GetServicioRest(UrlApi, api, auth)).Result

                If r IsNot Nothing AndAlso r.EsExitoso Then

                    resultado = JsonConvert.DeserializeObject(Of ResultadoServiciosOutbound)(r.JsonResp)

                End If
            Else
                Throw New Exception("No fue posible iniciar sesión. Por favor validar que cuenta con licencias de Administrador disponibles.")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If SesionIniciada AndAlso Not String.IsNullOrWhiteSpace(Token) Then
                CerrarSesion()
                'If Not CerrarSesion() Then
                '    Throw New Exception("No se puede cerrar sesión. Debe contactar a Infraestructura IT")
                'End If
            End If
        End Try

        Return resultado

    End Function

    Public Function EnviaGestionClientes(ByVal dataTable As DataTable, Optional ByVal id As Integer = 0) As String

        Dim resultadoExitoso As String = "Proceso no exitoso"

        Try
            If IniciarSesion() Then
                Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Bearer", Token)

                Dim api As String = ""
                If id >= 0 Then api = "api/v3/services/outbound/" + id.ToString + "/loads"

                If IniciarEnvioPrecence(api, dataTable) Then
                    Dim IdCarga As Integer = 0
                    For Each dr As DataRow In dataTable.Rows
                        IdCarga = Convert.ToInt32(dr("LoadId"))
                    Next

                    'Activar 
                    Dim resultado As String = HabilitarCargaPresence(IdCarga, id)

                    If resultado = "CARGA ACTIVADA" Then
                        'Actualizar estado a Activadp
                        Dim ActualizarEstadoIntegracion As String = ActualizarEstadotransacionEnBD("CARGADO", "ACTIVADO", id, IdCarga)
                    Else
                        'Actualizar estado a Desactivado
                        Dim ActualizarEstadoIntegracion As String = ActualizarEstadotransacionEnBD("CARGADO", "DESACTIVADO", id, IdCarga)
                    End If

                    resultadoExitoso = "OK"

                Else
                    resultadoExitoso = "Proceso No Exitoso"
                End If

                'Return resultadoExitoso
            Else
                resultadoExitoso = "No fue posible Iniciar Sesion: Valide que tenga Licencias disponibles y Vuelva a intentarlo"
            End If
            Return resultadoExitoso
        Catch e As Exception
            Return resultadoExitoso = e.Message
        Finally

            CerrarSesion()

        End Try


    End Function
    Public Function EnviaGestionClientes(ByVal id As Int64, ByVal Nombre As String, ByVal idCarga As Int64) As ResultadoProceso

        Dim resultadoExitoso As Boolean = False
        Dim Resul As New ResultadoProceso

        Try
            If IniciarSesion() Then
                Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Bearer", Token)

                Dim api As String = ""
                If id >= 0 Then api = "api/v3/services/outbound/" + id.ToString + "/loads"

                Dim RegistroPresence As New EncabezadoCampaniaPresence()
                Dim Detalle As MapFields = New MapFields()
                RegistroPresence.LoadId = idCarga 'id
                RegistroPresence.Description = Nombre
                RegistroPresence.PriorityType = 1
                RegistroPresence.PriorityValue = "100"
                RegistroPresence.PriorityField = ""
                RegistroPresence.DeleteDuplicatePhoneNumbers = False
                RegistroPresence.DataSourceType = 3

                Dim r As ResultadoApiRest = Task.Run(Function() RestApiManager.PostServicioRest(UrlApi, api, RegistroPresence, auth)).Result
                If r IsNot Nothing AndAlso r.EsExitoso Then
                    resultadoExitoso = True
                    Dim resultado As String = HabilitarCargaPresence(idCarga, id)

                    If resultado = "CARGA ACTIVADA" Then
                        'Actualizar estado a Activadp
                        Dim ActualizarEstadoIntegracion As String = ActivarCargaPresence(idCarga, 50)
                    Else
                        'Actualizar estado a Desactivado
                        Dim ActualizarEstadoIntegracion As String = ActivarCargaPresence(idCarga, 51)
                    End If
                    Resul.EstablecerMensajeYValor(0, "La carga fue registrada de forma correcta")
                Else
                    resultadoExitoso = False

                    Dim ActualizarEstadoIntegracion As String = EliminarCargaPresence(idCarga)
                    Resul.EstablecerMensajeYValor(20, "No se encontaron licencias de administracion")
                End If
                Return Resul
            Else
                Dim ActualizarEstadoIntegracion As String = EliminarCargaPresence(idCarga)
                Resul.EstablecerMensajeYValor(20, "No se encontaron licencias de administracion")
            End If
        Catch e As Exception
            Dim xx = e.Message
        Finally
            'If SesionIniciada AndAlso Not String.IsNullOrWhiteSpace(Token) Then
            CerrarSesion()
            ' AndAlso Not String.IsNullOrWhiteSpace(Token)
            ' End If
        End Try

        Return Resul
    End Function

    Protected Function ActualizarEstadotransacionEnBD(ByVal EstadoActual As String, ByVal NuevoEstado As String, ByVal idServicio As Integer, ByVal idLoad As Integer) As String
        Dim obOperacionBD As New OperacionesBD
        Dim IdUsuario As Integer = 0
        If CInt(HttpContext.Current.Session("userId")) > 0 Then IdUsuario = CInt(HttpContext.Current.Session("userId"))


        Dim NuevoestadoConsultado As String = obOperacionBD.ActualizarEstadoEnvioTransacionPresence(IdUsuario, EstadoActual, NuevoEstado, idServicio, idLoad)
        Return NuevoestadoConsultado

    End Function
    Protected Function ActivarCargaPresence(ByVal idLoad As Integer, ByVal idEstado As Int16) As String
        Dim obOperacionBD As New OperacionesBD
        Dim IdUsuario As Integer = 0
        If CInt(HttpContext.Current.Session("userId")) > 0 Then IdUsuario = CInt(HttpContext.Current.Session("userId"))

        Dim dbManager As New LMDataAccess
        Dim EstadoActualizado As Int64
        Try
            With dbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                .SqlParametros.Add("@idLoad", SqlDbType.Int).Value = idLoad
                .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = idEstado
                .SqlParametros.Add("@Resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .EjecutarDataSet("ActivarCargaPresence", CommandType.StoredProcedure)
                EstadoActualizado = .SqlParametros("@Resultado").Value.ToString
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return EstadoActualizado


    End Function

    Protected Function EliminarCargaPresence(ByVal idLoad As Integer) As String
        Dim obOperacionBD As New OperacionesBD
        Dim IdUsuario As Integer = 0
        If CInt(HttpContext.Current.Session("userId")) > 0 Then IdUsuario = CInt(HttpContext.Current.Session("userId"))

        Dim dbManager As New LMDataAccess
        Dim EstadoActualizado As Int64
        Try
            With dbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                .SqlParametros.Add("@idLoad", SqlDbType.Int).Value = idLoad
                .SqlParametros.Add("@Resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .EjecutarDataSet("EliminarCargaPresence", CommandType.StoredProcedure)
                EstadoActualizado = .SqlParametros("@Resultado").Value.ToString
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return EstadoActualizado


    End Function


    Public Function IniciarEnvioPrecence(ByVal Api As String, ByVal dataTable As DataTable) As Boolean
        Dim resultado As Boolean = False
        Try
            Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Bearer", Token)

            Dim RegistroPresence As EncabezadoServiciosSalientes = convertirTableinObjec(dataTable)

            Dim r As ResultadoApiRest = Task.Run(Function() RestApiManager.PostServicioRest(UrlApi, Api, RegistroPresence, auth)).Result
            If r IsNot Nothing AndAlso r.EsExitoso Then

                resultado = True
            Else
                resultado = False


            End If

            Return resultado
        Catch ex As Exception
            Dim xx = ex.Message
        End Try


    End Function
    Private Function convertirTableinObjec(ByVal dataTable As DataTable) As EncabezadoServiciosSalientes
        Dim RegistroPresence As EncabezadoServiciosSalientes = New EncabezadoServiciosSalientes()
        Dim Detalle As MapFields = New MapFields()
        For Each dr As DataRow In dataTable.Rows
            RegistroPresence.LoadId = Convert.ToInt32(dr("LoadId"))
            RegistroPresence.Description = dr("Descripcion").ToString()
            RegistroPresence.PriorityType = Convert.ToInt32(dr("PriorityType"))
            RegistroPresence.PriorityValue = dr("PriorityValue").ToString()
            RegistroPresence.PriorityField = dr("PriorityField").ToString()
            RegistroPresence.DeleteDuplicatePhoneNumbers = Convert.ToInt32(dr("DeleteDuplicatePhoneNumbers"))
            RegistroPresence.DataSourceType = Convert.ToInt32(dr("DataSourceType"))
            RegistroPresence.DataSourceConnection = dr("DataSourceConnection").ToString()
            RegistroPresence.DataSourceUser = dr("DataSourceUser").ToString()
            RegistroPresence.DataSourcePassword = dr("DataSourcePassword").ToString()

            Detalle.SourceIdField = dr("SourceIdField").ToString()
            Detalle.NameField = dr("NameField").ToString()

            Detalle.TimeZoneType = Convert.ToInt32(dr("TimeZoneType"))
            Detalle.TimeZone = dr("TimeZone").ToString()
            Detalle.DefaultTimeZone = dr("DefaultTimeZone").ToString()
            Detalle.PhoneField = dr("PhoneField").ToString()
            Detalle.PhoneDescription = Convert.ToInt32(dr("PhoneDescription"))
            Detalle.PhoneTimeZoneType = Convert.ToInt32(dr("PhoneTimeZoneType"))
            Detalle.PhoneTimeZone = dr("PhoneTimeZone").ToString()

            Detalle.PhoneField2 = dr("PhoneField2").ToString()
            Detalle.PhoneDescription2 = Convert.ToInt32(dr("PhoneDescription2"))
            Detalle.PhoneTimeZoneType2 = Convert.ToInt32(dr("PhoneTimeZoneType2"))
            Detalle.PhoneTimeZone2 = dr("PhoneTimeZone2").ToString()

            Detalle.PhoneField3 = dr("PhoneField3").ToString()
            Detalle.PhoneDescription3 = Convert.ToInt32(dr("PhoneDescription3"))
            Detalle.PhoneTimeZoneType3 = Convert.ToInt32(dr("PhoneTimeZoneType3"))
            Detalle.PhoneTimeZone3 = dr("PhoneTimeZone3").ToString()

            Detalle.PhoneField4 = dr("PhoneField4").ToString()
            Detalle.PhoneDescription4 = Convert.ToInt32(dr("PhoneDescription4"))
            Detalle.PhoneTimeZoneType4 = Convert.ToInt32(dr("PhoneTimeZoneType4"))
            Detalle.PhoneTimeZone4 = dr("PhoneTimeZone4").ToString()

            Detalle.PhoneField5 = dr("PhoneField5").ToString()
            Detalle.PhoneDescription5 = Convert.ToInt32(dr("PhoneDescription5"))
            Detalle.PhoneTimeZoneType5 = Convert.ToInt32(dr("PhoneTimeZoneType5"))
            Detalle.PhoneTimeZone5 = dr("PhoneTimeZone5").ToString()

            Detalle.PhoneField6 = dr("PhoneField6").ToString()
            Detalle.PhoneDescription6 = Convert.ToInt32(dr("PhoneDescription6"))
            Detalle.PhoneTimeZoneType6 = Convert.ToInt32(dr("PhoneTimeZoneType6"))
            Detalle.PhoneTimeZone6 = dr("PhoneTimeZone6").ToString()

            Detalle.PhoneField7 = dr("PhoneField7").ToString()
            Detalle.PhoneDescription7 = Convert.ToInt32(dr("PhoneDescription7"))
            Detalle.PhoneTimeZoneType7 = Convert.ToInt32(dr("PhoneTimeZoneType7"))
            Detalle.PhoneTimeZone7 = dr("PhoneTimeZone7").ToString()

            Detalle.PhoneField8 = dr("PhoneField8").ToString()
            Detalle.PhoneDescription8 = Convert.ToInt32(dr("PhoneDescription8"))
            Detalle.PhoneTimeZoneType8 = Convert.ToInt32(dr("PhoneTimeZoneType8"))
            Detalle.PhoneTimeZone8 = dr("PhoneTimeZone8").ToString()

            Detalle.PhoneField9 = dr("PhoneField9").ToString()
            Detalle.PhoneDescription9 = Convert.ToInt32(dr("PhoneDescription9"))
            Detalle.PhoneTimeZoneType9 = Convert.ToInt32(dr("PhoneTimeZoneType9"))
            Detalle.PhoneTimeZone9 = dr("PhoneTimeZone9").ToString()

            Detalle.PhoneField10 = dr("PhoneField10").ToString()
            Detalle.PhoneDescription10 = Convert.ToInt32(dr("PhoneDescription10"))
            Detalle.PhoneTimeZoneType10 = Convert.ToInt32(dr("PhoneTimeZoneType10"))
            Detalle.PhoneTimeZone10 = dr("PhoneTimeZone10").ToString()

            Detalle.CommentsField = dr("CommentsField").ToString()
            Detalle.CustomDataField1 = dr("CustomDataField1").ToString()
            Detalle.CustomDataField2 = dr("CustomDataField2").ToString()
            Detalle.CustomDataField3 = dr("CustomDataField3").ToString()
            Detalle.CallerIdField = dr("CallerIdField").ToString()
            Detalle.CallerNameField = dr("CallerNameField").ToString()
            Detalle.Filter = dr("Filter").ToString()

            RegistroPresence.MapFields = Detalle
        Next
        Return RegistroPresence
    End Function
    Public Function ValidarCargaHabilitada(ByVal Loadid As Integer, ByVal IdServicio As Integer) As String

        Dim RespuestaServicio As ResultadoCargaHabilitada = Nothing
        Dim EstaHabilitado As Boolean = False
        Dim resultado As String = ""
        Try
            If IniciarSesion() Then
                Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Bearer", Token)
                Dim api As String = "api/v1/services/outbound/"
                If IdServicio > 0 And Loadid > 0 Then api = "api/v1/services/outbound/" & IdServicio.ToString & "/loads/" & Loadid.ToString & "/enabled"


                Dim r As ResultadoApiRest = Task.Run(Function() RestApiManager.GetServicioRest(UrlApi, api, auth)).Result

                If r IsNot Nothing AndAlso r.EsExitoso Then

                    RespuestaServicio = JsonConvert.DeserializeObject(Of ResultadoCargaHabilitada)(r.JsonResp)
                    EstaHabilitado = RespuestaServicio.Data.Enabled

                    If EstaHabilitado Then

                        resultado = DeshabilitarCargaPresence(Loadid, IdServicio)
                        Return resultado

                    Else
                        resultado = HabilitarCargaPresence(Loadid, IdServicio)
                        Return resultado
                    End If
                Else
                    resultado = "No fue posible obtener el estado de la carga"
                    Return resultado
                End If

            Else
                resultado = "No fue posible iniciar sesión. Por favor validar que cuenta con licencias de Administrador disponibles."
                Return resultado
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally

            CerrarSesion()

        End Try


    End Function

    Public Function HabilitarCargaPresence(ByVal Loadid As Integer, ByVal IdServicio As Integer) As String
        Dim resultado As String = ""

        Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Bearer", Token)
        Dim api As String = "api/v1/services/outbound/"
        If IdServicio > 0 And Loadid > 0 Then api = "api/v1/services/outbound/" & IdServicio.ToString & "/loads/" & Loadid.ToString & "/enabled"

        Dim r As ResultadoApiRest = Task.Run(Function() RestApiManager.PutServicioRest(UrlApi, api, "", auth)).Result

        If r IsNot Nothing AndAlso r.EsExitoso Then

            resultado = "CARGA ACTIVADA"
        End If

        Return resultado
    End Function
    Public Function DeshabilitarCargaPresence(ByVal Loadid As Integer, ByVal IdServicio As Integer) As String
        Dim resultado As String = ""

        Dim auth As AuthenticationHeaderValue = New AuthenticationHeaderValue("Bearer", Token)
        Dim api As String = "api/v1/services/outbound/"
        If IdServicio > 0 And Loadid > 0 Then api = "api/v1/services/outbound/" & IdServicio.ToString & "/loads/" & Loadid.ToString & "/enabled"

        Dim r As ResultadoApiRest = Task.Run(Function() RestApiManager.DELETEServicioRest(UrlApi, api, auth)).Result

        If r IsNot Nothing AndAlso r.EsExitoso Then
            resultado = "CARGA DESACIVADA"

        End If

        Return resultado
    End Function
End Class

