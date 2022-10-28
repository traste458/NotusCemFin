Imports System.IO
Imports System.Text
Imports System.Web.UI.WebControls
Imports LMDataAccessLayer
Imports Newtonsoft.Json
Imports NotusOmnicanalidadBL.Tools


Public Class CuentasTransportadoras
    Public Property IdCuenta As Integer
    Public Property descripcion As String
    Public Property CodigoCuenta As String
    Public Property cuentauser As String
    Public Property cuentapass As String
    Public Property idtransportadora As Integer
    Public Property CodFacturacion As String
    Public Property NombreCargue As String

    Public Property activo As Boolean
    Public Property idusuario As Integer
    Public Property strModo As String
    Public Property IdBodega As Integer

    Structure ResultadoRadicadosTemporal
        Dim Mensaje As String
        Dim diceContener As String
        Dim ValorDeclarado As Integer
        Dim dtTabla As DataTable
    End Structure
    Public Function ObtenerCuentasTransportadora(Optional ByVal idTransportadora As Integer = 0, Optional ByVal idBodega As Integer = 0) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtBase As New DataTable
        Try
            With dbManager
                If idTransportadora > 0 Then .SqlParametros.Add("@idTransportadora", SqlDbType.Decimal).Value = idTransportadora
                If idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = idBodega
                .ejecutarReader("ObtenerCuentasTransportadoras", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dtBase.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dtBase
    End Function

    Public Function ObtenerCuentasTransportadorasXidCuenta(ByVal idCuenta As Decimal, Optional ByVal idBodegaOrigen As Integer = 0) As GeneracionGuias.dtoConexion
        Dim dbManager As New LMDataAccess
        Dim dtBase As New GeneracionGuias.dtoConexion
        Try
            With dbManager
                If idCuenta > 0 Then .SqlParametros.Add("@idCuenta", SqlDbType.Decimal).Value = idCuenta
                If idBodegaOrigen > 0 Then .SqlParametros.Add("@idBodegaRemite", SqlDbType.Int).Value = idBodegaOrigen
                .ejecutarReader("ObtenerCuentasTransportadorasXidCuenta", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        dtBase.NombreTransportadora = .Reader("NombreTransportadora").ToString
                        dtBase.login = .Reader("CuentaUser").ToString
                        dtBase.pwd = .Reader("cuentapass").ToString
                        dtBase.NombreCuenta = .Reader("Descripcion").ToString
                        dtBase.CodigoCuenta = .Reader("CodigoCuenta").ToString
                        dtBase.idSistemaProceso = .Reader("ggtSistemaProceso")
                        dtBase.CodFacturacion = .Reader("CodFacturacion").ToString
                        dtBase.NombreCargue = .Reader("NombreCargue").ToString
                    End If
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dtBase
    End Function

    Public Function ObtenerDatosGenerarGuiaXidUsuario(ByVal Metodo As String, Optional ByVal idUsuario As Decimal = 0, Optional ByVal idBodegaRemite As Integer = 0, Optional ByVal IdBodegaDestino As Integer = 0) As GeneracionGuias.dtoGuias
        Dim dbManager As New LMDataAccess
        Dim dtBase As New GeneracionGuias.dtoGuias
        Try
            With dbManager
                If idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                If idBodegaRemite > 0 Then .SqlParametros.Add("@idBodegaRemite", SqlDbType.Int).Value = idBodegaRemite
                If IdBodegaDestino > 0 Then .SqlParametros.Add("@IdBodegaDestino", SqlDbType.Int).Value = IdBodegaDestino
                .SqlParametros.Add("@Metodo", SqlDbType.VarChar, 20).Value = Metodo

                .ejecutarReader("ObtenerDatosGenerarGuiaXidUsuario", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        dtBase.DestinatarioNombre = .Reader("DestinatarioNombre").ToString
                        dtBase.DestinatarioNombreAutorizado = .Reader("DestinatarioNombreAutorizado").ToString
                        dtBase.DestinatarioIdentificacion = .Reader("DestinatarioIdentificacion").ToString
                        dtBase.DestinatarioTelefono = .Reader("DestinatarioTelefono").ToString
                        dtBase.DestinatarioCiudad = .Reader("DestinatarioCiudad").ToString

                        dtBase.DestinatarioDireccion = .Reader("DestinatarioDireccion").ToString
                        dtBase.DestinatarioBarrio = .Reader("DestinatarioBarrio").ToString
                        dtBase.DestinatarioDireccionObservaciones = .Reader("observacionDireccion").ToString
                        dtBase.RemiteNombre = .Reader("RemiteNombre").ToString
                        dtBase.RemiteDireccion = .Reader("RemiteDireccion").ToString
                        dtBase.RemiteCiudad = .Reader("RemiteCiudad").ToString

                        dtBase.RemiteCiudadNombre = .Reader("RemiteCiudadNombre").ToString
                        dtBase.DestinatarioCiudadNombre = .Reader("DestinatarioCiudadNombre").ToString

                        'dtBase.DiceContener = .Reader("DiceContener").ToString
                        'Decimal.TryParse(.Reader("ValorDeclarado"), dtBase.Num_ValorDeclaradoTotal)
                    End If

                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dtBase
    End Function

    Public Function ObtenerDatosGenerarGuiaDetalleXidUsuario(ByVal idUsuario As Decimal) As GeneracionGuias.dtoDetalleMaterial()
        Dim dbManager As New LMDataAccess

        Dim dtBase As New DataTable

        Try
            With dbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                .ejecutarReader("ObtenerDatosGenerarGuiaDetalleXidUsuario", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dtBase.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Dim dtDetalle(dtBase.Rows.Count) As GeneracionGuias.dtoDetalleMaterial
        Dim xfila As Integer = 0
        For Each row As DataRow In dtBase.Rows
            Dim Xdetalle As New GeneracionGuias.dtoDetalleMaterial
            Xdetalle.numeroPedido = row.Item("numeroRadicado")
            Xdetalle.codMaterial = row.Item("Material")
            Xdetalle.descripcion = row.Item("producto")
            Xdetalle.cantidad = row.Item("cantidad")
            Xdetalle.valorUnitario = row.Item("ValorUnitario")
            Xdetalle.idGuia = 1
            dtDetalle(xfila) = Xdetalle
            xfila += 1
        Next

        Return dtDetalle
    End Function

    Public Function ObtenerCuentasTransportadoraLog(ByVal idCuentaX As Integer) As DataTable 'Optional ByVal IdCuenta As Integer = 0
        Dim dbManager As New LMDataAccess
        Dim dtBase As New DataTable
        Try
            With dbManager
                If idCuentaX > 0 Then .SqlParametros.Add("@idCuenta", SqlDbType.Decimal).Value = idCuentaX
                .ejecutarReader("ObtenerCuentasTransportadorasLog", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dtBase.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dtBase
    End Function

    Public Sub GuardarCuenta()
        Dim dbManager As New LMDataAccess
        Dim Mensaje As String = String.Empty
        Try
            With dbManager
                If strModo IsNot Nothing Then .SqlParametros.Add("@strModo", SqlDbType.VarChar, 20).Value = strModo
                If descripcion IsNot Nothing Then .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 200).Value = descripcion
                If CodigoCuenta IsNot Nothing Then .SqlParametros.Add("@CodigoCuenta", SqlDbType.VarChar, 80).Value = CodigoCuenta
                If cuentauser IsNot Nothing Then .SqlParametros.Add("@cuentauser", SqlDbType.VarChar, 150).Value = cuentauser
                If cuentapass IsNot Nothing Then .SqlParametros.Add("@cuentapass", SqlDbType.VarChar, 550).Value = cuentapass
                If idtransportadora > 0 Then .SqlParametros.Add("@idtransportadora", SqlDbType.Decimal).Value = idtransportadora
                If CodFacturacion IsNot Nothing Then .SqlParametros.Add("@CodFacturacion", SqlDbType.VarChar, 100).Value = CodFacturacion
                If NombreCargue IsNot Nothing Then .SqlParametros.Add("@NombreCargue", SqlDbType.VarChar, 80).Value = NombreCargue
                If IsDBNull(activo) = False Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = activo
                If idusuario > 0 Then .SqlParametros.Add("@idusuario", SqlDbType.Decimal).Value = idusuario
                If IdCuenta > 0 Then .SqlParametros.Add("@idCuenta", SqlDbType.Decimal).Value = IdCuenta
                If IdBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Decimal).Value = IdBodega

                .ejecutarReader("ggtCuentasTransportadoraCrud", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Mensaje = .Reader.ToString
                    .Reader.Close()
                End If
            End With
        Catch ex As Exception
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Function ListarTipoTransporte() As DataTable
        Dim respuesta As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Try
            respuesta = adminBD.EjecutarDataTable("ListarTipoTransporte", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar listado de tipos de transporte: " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try

        Return respuesta
    End Function

    Public Function numRadicadoTemporalGenerarGuiaXMetodo(ByVal NumRadicado As Decimal, ByVal idUsuario As Decimal, ByVal Metodo As String) As String
        Dim dbManager As New LMDataAccess
        Dim resultado As String = ""

        Try
            With dbManager
                If NumRadicado > 0 Then .SqlParametros.Add("@numRadicado", SqlDbType.Decimal).Value = NumRadicado
                If idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                .SqlParametros.Add("@Metodo", SqlDbType.VarChar, 25).Value = Metodo
                .SqlParametros.Add("@Mensaje", SqlDbType.VarChar, 50).Value = ""
                .SqlParametros.Add("@DiceContener", SqlDbType.VarChar, 50).Value = ""
                .SqlParametros.Add("@ValorDeclarado", SqlDbType.Int).Value = 0
                .SqlParametros("@Mensaje").Direction = ParameterDirection.Output
                .SqlParametros("@DiceContener").Direction = ParameterDirection.Output
                .SqlParametros("@ValorDeclarado").Direction = ParameterDirection.Output
                .ejecutarReader("numRadicadoTemporalGenerarGuiaXMetodo", CommandType.StoredProcedure)

                resultado = .SqlParametros("@Mensaje").Value

            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function



    Public Function ListarnumRadicadoTemporalGenerarGuiaXMetodo(ByVal idUsuario As Decimal) As ResultadoRadicadosTemporal
        Dim dbManager As New LMDataAccess
        Dim resultado As New ResultadoRadicadosTemporal
        Dim dtInfo As New DataTable
        Try
            With dbManager
                If idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                .SqlParametros.Add("@Metodo", SqlDbType.VarChar, 25).Value = "ListarRegistrosXUsuario"
                .SqlParametros.Add("@Mensaje", SqlDbType.VarChar, 50)
                .SqlParametros.Add("@DiceContener", SqlDbType.VarChar, 50)
                .SqlParametros.Add("@ValorDeclarado", SqlDbType.Int)
                .SqlParametros("@Mensaje").Direction = ParameterDirection.Output
                .SqlParametros("@DiceContener").Direction = ParameterDirection.Output
                .SqlParametros("@ValorDeclarado").Direction = ParameterDirection.Output
                .ejecutarReader("numRadicadoTemporalGenerarGuiaXMetodo", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    dtInfo.Load(.Reader)
                    .Reader.Close()
                End If
                resultado.Mensaje = ""
                resultado.diceContener = ""
                resultado.ValorDeclarado = 0

                If IsDBNull(.SqlParametros("@Mensaje").Value) = False Then
                    resultado.Mensaje = .SqlParametros("@Mensaje").Value
                End If
                If IsDBNull(.SqlParametros("@DiceContener").Value) = False Then
                    resultado.diceContener = .SqlParametros("@DiceContener").Value.ToString.Substring(1)
                End If
                If IsDBNull(.SqlParametros("@ValorDeclarado").Value) = False Then
                    resultado.ValorDeclarado = .SqlParametros("@ValorDeclarado").Value
                End If

                'resultado.Mensaje = IIf(IsDBNull(.SqlParametros("@Mensaje").Value), "", .SqlParametros("@Mensaje").Value)
                'resultado.diceContener = IIf(IsDBNull(.SqlParametros("@DiceContener").Value = True), "", .SqlParametros("@DiceContener").Value.ToString.Substring(1))
                'resultado.ValorDeclarado = IIf(IsDBNull(.SqlParametros("@ValorDeclarado").Value = True), "0", .SqlParametros("@ValorDeclarado").Value)
                resultado.dtTabla = dtInfo
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function

    Public Function ggtGuardarDatosGeneracionGuia(ByVal guia As String, ByVal dtoGuia As GeneracionGuias.dtoGeneracionGuias, ByVal idTipoEnvio As Integer) As Integer
        Dim dbManager As New LMDataAccess
        Dim resultado As Integer = 0

        Try
            With dbManager
                .SqlParametros.Add("@guia", SqlDbType.VarChar, 50).Value = guia
                .SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = dtoGuia.DatoConexion.idTransportadora
                .SqlParametros.Add("@Cuenta", SqlDbType.VarChar, 100).Value = dtoGuia.DatoConexion.NombreCuenta
                '.SqlParametros.Add("@NumPie", SqlDbType.VarChar, 50).Value = dtoGuia.DatoGuias.NumeroPiezas
                '.SqlParametros.Add("@CiuRem", SqlDbType.VarChar, 50).Value = dtoGuia.DatoGuias.RemiteCiudadNombre
                '.SqlParametros.Add("@DirRem", SqlDbType.VarChar, 200).Value = dtoGuia.DatoGuias.RemiteDireccion
                '.SqlParametros.Add("@CiuDes", SqlDbType.VarChar, 50).Value = dtoGuia.DatoGuias.DestinatarioCiudadNombre
                '.SqlParametros.Add("@NomDes", SqlDbType.VarChar, 50).Value = dtoGuia.DatoGuias.DestinatarioNombre
                '.SqlParametros.Add("@DirDes", SqlDbType.VarChar, 200).Value = dtoGuia.DatoGuias.DestinatarioDireccion
                '.SqlParametros.Add("@DiceContener", SqlDbType.VarChar, 100).Value = dtoGuia.DatoGuias.DiceContener
                '.SqlParametros.Add("@ValorDeclaradoTotal", SqlDbType.Decimal).Value = dtoGuia.DatoGuias.Num_ValorDeclaradoTotal
                '.SqlParametros.Add("@ValorFlete", SqlDbType.Decimal).Value = ResultadoGuia.ValorFlete
                '.SqlParametros.Add("@PesoTotal", SqlDbType.Decimal).Value = dtoGuia.DatoGuias.Num_PesoTotal
                '.SqlParametros.Add("@VolumenTotal", SqlDbType.Decimal).Value = dtoGuia.DatoGuias.Num_VolumenTotal
                '.SqlParametros.Add("@UnidadEnpaque", SqlDbType.VarChar, 50).Value = dtoGuia.DatoGuias.UnidadEmpaque
                .SqlParametros.Add("@idTipoEnvio", SqlDbType.Int).Value = idTipoEnvio
                .SqlParametros.Add("@idUsuario", SqlDbType.Decimal).Value = dtoGuia.DatoConexion.idUsuario
                If IsNothing(dtoGuia.DatoGuias(0).URLGuia) = False Then
                    If dtoGuia.DatoGuias(0).URLGuia.Trim <> "" Then
                        .SqlParametros.Add("@UrlGuia", SqlDbType.VarChar, 500).Value = dtoGuia.DatoGuias(0).URLGuia
                    End If
                End If

                .SqlParametros.Add("@idSerivicioMensajeria", SqlDbType.Int)
                .SqlParametros("@idSerivicioMensajeria").Direction = ParameterDirection.Output
                .ejecutarReader("ggtGuardarDatosGeneracionGuia", CommandType.StoredProcedure)
                'resultado = .SqlParametros("@Mensaje").Value
                If IsDBNull(.SqlParametros("@idSerivicioMensajeria").Value) = False Then
                    resultado = .SqlParametros("@idSerivicioMensajeria").Value
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function

    Public Function GenerarGuia(ByVal Dto As GeneracionGuias.dtoGeneracionGuias, Optional ByVal configAwait As Boolean = True) As GeneracionGuias.dtoGeneracionGuias

        Dim XDatos As New GeneracionGuias.dtoGeneracionGuias

        Try
            Dim Api As New Comunes.ConfigValues("GeneracionGuiasTransportadoras")
            Dim Rsta = ServicioRest.InvocarServicioRest("POST", Api.ConfigKeyValue, Api.ConfigKeyName, Dto, configAwait:=configAwait).Result
            If Rsta.Exitoso = False Then
                'XDatosEnvia.Codigo = 203
                'XDatosEnvia.EsExitoso = False
                'Dim xmensaje(2) As String
                'xmensaje(0) = Rsta.Mensaje
                'XDatosEnvia.Mensaje = xmensaje

                XDatos.EsExitoso = False
                XDatos.MensajeError = Rsta.Mensaje
            Else
                XDatos = JsonConvert.DeserializeObject(Of GeneracionGuias.dtoGeneracionGuias)(Rsta.Datos.ToString())
            End If
        Catch ex As Exception
            'Dim xmensaje(1) As String
            'XDatosEnvia.Codigo = 203
            'XDatosEnvia.EsExitoso = False
            'xmensaje(0) = "Error al llamar el servicio, " & ex.Message
            'XDatosEnvia.Mensaje = xmensaje
            XDatos.EsExitoso = False
            XDatos.MensajeError = "Error al llamar el servicio, " & ex.Message
        End Try
        Return XDatos
    End Function

    Public Function GenerarGuiaStickerServiEntrega(ByVal Dto As GeneracionGuias.dtoGenerarGuiaStickerServiEntrega, ByVal ruta As String) As Byte()
        Dim Sesultado As String = ""
        Dim bytesX() As Byte = {0}
        Try
            Dim Api As New Comunes.ConfigValues("GenerarGuiaStickerServiEntrega")
            Dim Rsta = ServicioRest.InvocarServicioRest("POST", Api.ConfigKeyValue, Api.ConfigKeyName, Dto).Result
            If Rsta.Exitoso = False Then
                Sesultado = ""
            Else
                If Rsta.Datos.ToString.Trim = """""" Then
                    Sesultado = ""
                    Return bytesX
                End If

                Sesultado = Replace(Rsta.Datos.ToString.Trim, """", "")

                bytesX = Convert.FromBase64String(Sesultado)
                'Dim obj As FileStream = File.Create(ruta & "\G" & Dto.guia & ".pdf")
                'obj.Write(bytesX, 0, bytesX.Length)
                'obj.Flush()
                'obj.Close()
            End If
            Sesultado = ruta & "\G" & Dto.guia & ".pdf"
        Catch ex As Exception

        End Try
        Return bytesX
    End Function

    Public Function GenerarGuiaStickerInterrapidisimo(ByVal Dto As GeneracionGuias.dtoGenerarGuiaStickerServiEntrega, ByVal ruta As String) As Byte()
        Dim Sesultado As String = ""
        Dim bytesX() As Byte = {0}
        Try
            Dim Api As New Comunes.ConfigValues("GenerarGuiaStickerInterRapidisimo")
            Dim Rsta = ServicioRest.InvocarServicioRest("POST", Api.ConfigKeyValue, Api.ConfigKeyName, Dto).Result
            If Rsta.Exitoso = False Then
                Sesultado = ""
            Else
                If Rsta.Datos.ToString.Trim = """""" Or Rsta.Datos.ToString.Trim = "" Then
                    Sesultado = ""
                    Return bytesX
                End If
                Sesultado = JsonConvert.DeserializeObject(Of String)(Rsta.Datos.ToString)
                bytesX = Convert.FromBase64String(Sesultado)
            End If
            Sesultado = ruta & "\G" & Dto.guia & ".pdf"
        Catch ex As Exception

        End Try
        Return bytesX
    End Function

    Public Function ggtRegistrarDocumentoServicioMensajeria(ByVal idUsuario As Integer, ByVal bRuta() As Byte, ByVal Guia As String) As ResultadoProceso
        Dim respuesta As New ResultadoProceso
        respuesta.Valor = -1
        Using dbManager As New LMDataAccess
            Try
                Dim msImagen As New MemoryStream(bRuta)
                Dim biteArray As Byte() = New Byte(msImagen.Length) {}
                msImagen.Position = 0
                msImagen.Read(biteArray, 0, msImagen.Length)
                Dim identificadorunico As String = Guid.NewGuid().ToString()
                With dbManager
                    .SqlParametros.Add("@nombreArchivo", SqlDbType.VarChar).Value = "G" & Guia & ".pdf"
                    .SqlParametros.Add("@tipoContenido", SqlDbType.VarChar).Value = "application/pdf"
                    .SqlParametros.Add("@identificadorUnico", SqlDbType.VarChar).Value = identificadorunico
                    .SqlParametros.Add("@tamanio", SqlDbType.Int).Value = msImagen.Length
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@rutaAlmacenamiento", SqlDbType.VarChar, 255)
                    .SqlParametros.Add("@idDocumento", SqlDbType.Int)
                    .SqlParametros("@rutaAlmacenamiento").Direction = ParameterDirection.Output
                    .SqlParametros("@idDocumento").Direction = ParameterDirection.Output
                    .IniciarTransaccion()
                    .ejecutarReader("ggtRegistrarDocumentoServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        .Reader.Close()
                    End If
                    Dim Rutaalmac As String
                    Rutaalmac = .SqlParametros("@rutaAlmacenamiento").Value
                    respuesta.Valor = .SqlParametros("@idDocumento").Value
                    If Rutaalmac.Length > 17 Then
                        'Dim raizRutaCarpeta As New Comunes.ConfigValues("RUTA_AZURE")
                        'Dim objAzure As New GestionArchivosAzureStorage()
                        Dim streamArchivo As New MemoryStream(biteArray)
                        'With objAzure
                        '    .TipoContenido = "application/pdf"
                        '    .ArchivoStream = streamArchivo
                        '    .RutaAzure = raizRutaCarpeta.ConfigKeyValue & "/" & Rutaalmac & "/" & identificadorunico
                        '    .AlmacenarArchivoAzureAsync().Wait()
                        'End With
                        Dim ruta As String = String.Empty
                        Dim rutaAlmacenaArchivo As Comunes.ConfigValues = New Comunes.ConfigValues("RUTACARGUEARCHIVOSTRANCITORIOS")
                        ruta = rutaAlmacenaArchivo.ConfigKeyValue
                        '"Archivos\Servicio" & idUsuario.ToString().PadLeft(8, "0")

                        If Not Directory.Exists(ruta & Rutaalmac) Then
                            Directory.CreateDirectory(ruta & Rutaalmac)
                        End If

                        If Not Directory.Exists(ruta & Rutaalmac) Then
                            Directory.CreateDirectory(ruta & Rutaalmac)
                        End If
                        Dim rutaGuardar As String = ruta & Rutaalmac & "\" & identificadorunico & ".pdf"
                        Using fsArchivo As FileStream = File.Create(rutaGuardar)
                            Dim arrContenido As Byte() = New Byte(streamArchivo.Length - 1) {}
                            streamArchivo.Read(arrContenido, 0, arrContenido.Length)
                            fsArchivo.Write(arrContenido, 0, arrContenido.Length)
                        End Using

                        .ConfirmarTransaccion()
                        respuesta.Mensaje = rutaGuardar
                    Else
                        .AbortarTransaccion()
                        respuesta.Mensaje = "Se generó un error al intentar registrar el documento [" & "G" & Guia & ".pdf" & "]"
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw ex
            End Try
        End Using
        Return respuesta
    End Function

End Class
