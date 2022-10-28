Imports LMDataAccessLayer

Public Module HerramientasDelivery

    Public Function ObtenerInformacionGeneralDelivery(ByVal idDelivery As Integer) As Delivery
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                If (idDelivery > 0) Then
                    .SqlParametros.Add("@id_delivery", SqlDbType.Int).Value = idDelivery
                End If
                dtDatos = .EjecutarDataTable("ObtenerInformacionGeneralDelivery", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Dim delivery As New Delivery()

        If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
            Dim rowDelivery As DataRow = dtDatos.Rows(0)
            If Not IsDBNull(rowDelivery.Item("numero_orden")) Then delivery.numeroOrden = rowDelivery.Item("numero_orden")
            If Not IsDBNull(rowDelivery.Item("id_alistamiento")) Then delivery.idAlistamiento = rowDelivery.Item("id_alistamiento")
            If Not IsDBNull(rowDelivery.Item("nombre")) Then delivery.nombre = rowDelivery.Item("nombre")
            If Not IsDBNull(rowDelivery.Item("tipo_documento")) Then delivery.tipoDocumento = rowDelivery.Item("tipo_documento")
            If Not IsDBNull(rowDelivery.Item("numero_documento")) Then delivery.numeroDocumento = rowDelivery.Item("numero_documento")
            If Not IsDBNull(rowDelivery.Item("telefono")) Then delivery.telefono = rowDelivery.Item("telefono")
            If Not IsDBNull(rowDelivery.Item("numero_guia")) Then delivery.numeroGuia = rowDelivery.Item("numero_guia")
            If Not IsDBNull(rowDelivery.Item("numero_pedido")) Then delivery.numeroPedido = rowDelivery.Item("numero_pedido")
            If Not IsDBNull(rowDelivery.Item("valor_declarado")) Then delivery.valorDeclarado = rowDelivery.Item("valor_declarado")
            If Not IsDBNull(rowDelivery.Item("proceso_venta")) Then delivery.procesoVenta = rowDelivery.Item("proceso_venta")
            If Not IsDBNull(rowDelivery.Item("centro_origen")) Then delivery.centroOrigen = rowDelivery.Item("centro_origen")
            If Not IsDBNull(rowDelivery.Item("nombre_ubicacion")) Then delivery.nombreUbicacion = rowDelivery.Item("nombre_ubicacion")
            If Not IsDBNull(rowDelivery.Item("direccion_origen")) Then delivery.direccionOrigen = rowDelivery.Item("direccion_origen")
            If Not IsDBNull(rowDelivery.Item("codigo_municipio")) Then delivery.codigoMunicipio = rowDelivery.Item("codigo_municipio")
            If Not IsDBNull(rowDelivery.Item("departamento")) Then delivery.departamento = rowDelivery.Item("departamento")
            If Not IsDBNull(rowDelivery.Item("municipio")) Then delivery.municipio = rowDelivery.Item("municipio")
            If Not IsDBNull(rowDelivery.Item("barrio")) Then delivery.barrio = rowDelivery.Item("barrio")
            If Not IsDBNull(rowDelivery.Item("direccion_normalizada")) Then delivery.direccionNormalizada = rowDelivery.Item("direccion_normalizada")
            If Not IsDBNull(rowDelivery.Item("direccion_lenguaje_natural")) Then delivery.direccionLenguajeNatural = rowDelivery.Item("direccion_lenguaje_natural")
            If Not IsDBNull(rowDelivery.Item("complemento")) Then delivery.complemento = rowDelivery.Item("complemento")

            If Not IsDBNull(rowDelivery.Item("nombre_transportador")) Then delivery.nombreTransportador = rowDelivery.Item("nombre_transportador")
            If Not IsDBNull(rowDelivery.Item("cedula_transportador")) Then delivery.cedulaTransportador = rowDelivery.Item("cedula_transportador").ToString.Trim()
            If Not IsDBNull(rowDelivery.Item("placa_transportador")) Then delivery.placaTransportador = rowDelivery.Item("placa_transportador")

            If Not IsDBNull(rowDelivery.Item("estado_delivery")) Then delivery.estado = rowDelivery.Item("estado_delivery")
            If Not IsDBNull(rowDelivery.Item("fecha")) Then delivery.fecha = rowDelivery.Item("fecha")
            If Not IsDBNull(rowDelivery.Item("franja")) Then delivery.franja = rowDelivery.Item("franja")
            If Not IsDBNull(rowDelivery.Item("hora")) Then delivery.hora = rowDelivery.Item("hora")
            If Not IsDBNull(rowDelivery.Item("observacion")) Then delivery.observacion = rowDelivery.Item("observacion")

        End If

        Return delivery
    End Function

    Function EliminarCheckSerialesDelivery(ByVal idDelivery As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim _dbManager As New LMDataAccess

        Try
            With _dbManager
                If (idDelivery > 0) Then
                    With .SqlParametros
                        .Add("@idServicio", SqlDbType.Int).Value = idDelivery
                        .Add("@mensaje", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .EjecutarNonQuery("EliminarCheckSerialesDelivery", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.EstablecerMensajeYValor(.SqlParametros("@resultado").Value, resultado.Mensaje & .SqlParametros("@mensaje").Value)
                    Else
                        resultado.EstablecerMensajeYValor(300, "No se logró establecer la respuesta del servidor.")
                    End If
                End If
            End With
        Catch ex As Exception
            _dbManager.Dispose()
            resultado.EstablecerMensajeYValor(0, "No se logro la eliminacion de checks seriales: " & ex.Message)
        End Try
        Return resultado
    End Function

    Function ActualizarEstadoDelivery(ByVal idDelivery As Integer, ByVal estadoDelivery As Integer, ByVal idUsuario As Integer, Optional ByVal NovedadCRM As String = "") As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim _dbManager As New LMDataAccess

        Try
            With _dbManager
                If (idDelivery > 0) Then
                    With .SqlParametros
                        .Add("@id_delivery", SqlDbType.BigInt).Value = idDelivery
                        .Add("@estado", SqlDbType.Int).Value = estadoDelivery
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@mensaje", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With

                    Dim objRestDelivery As New RestDelivery
                    resultado = objRestDelivery.NotificacionCambioEstado(idDelivery, estadoDelivery, NovedadCRM)

                    If resultado.Valor = 1 Then
                        .EjecutarNonQuery("ActualizarEstadoDelivery", CommandType.StoredProcedure)

                        If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                            resultado.Valor = .SqlParametros("@resultado").Value
                            resultado.Mensaje = resultado.Mensaje & .SqlParametros("@mensaje").Value
                        Else
                            resultado.EstablecerMensajeYValor(300, "No se logró establecer la respuesta del servidor.")
                            resultado.Valor = 0
                            resultado.Mensaje = "No fue posible la actualizacion en base de datos"
                        End If
                    Else
                        resultado.Valor = 0

                    End If
                End If
            End With
        Catch ex As Exception
            _dbManager.Dispose()
            resultado.EstablecerMensajeYValor(0, "No se logro la actualizacion de estado delivery: " & ex.Message)
        End Try
        Return resultado
    End Function

    Function ObtenerCantidadCheckSerialesDelivery(ByVal idDelivery As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim _dbManager As New LMDataAccess

        Try
            With _dbManager
                If (idDelivery > 0) Then
                    With .SqlParametros
                        .Add("@id_delivery", SqlDbType.Int).Value = idDelivery
                        .Add("@valor", SqlDbType.VarChar, 5000).Direction = ParameterDirection.Output
                    End With
                    .EjecutarNonQuery("ObtenerCantidadCheckSerialesDelivery", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@valor").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@valor").Value
                    Else
                        resultado.EstablecerMensajeYValor(300, "No se logró establecer la respuesta del servidor.")
                    End If
                End If
            End With
        Catch ex As Exception
            _dbManager.Dispose()
            resultado.EstablecerMensajeYValor(400, "Se presentó un error al generar el mensaje de confirmación: " & ex.Message)
        End Try
        Return resultado
    End Function

    Function ActualizarCheckSerialesDelivery(ByVal idDetalle As Integer, ByVal checkValue As Integer, ByVal idUsuario As Integer) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                If (idDetalle > 0) Then
                    .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = idDetalle
                    .SqlParametros.Add("@checkValue", SqlDbType.Int).Value = checkValue
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End If
                dtDatos = .EjecutarDataTable("ActualizarCheckSerialesDelivery", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ObtenerInformacionMaterialSerialesDelivery(ByVal idDelivery As Integer) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                If (idDelivery > 0) Then
                    .SqlParametros.Add("@id_delivery", SqlDbType.Int).Value = idDelivery
                End If
                dtDatos = .EjecutarDataTable("ObtenerInformacionMaterialSerialesDelivery", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ObtenerInformacionDeliveryDetalleMateriales(ByVal idDelivery As Integer) As DataTable

        Dim _dbManager As New LMDataAccess
        Dim dtDatosMaterial As New DataTable

        Try
            With _dbManager
                If (idDelivery > 0) Then
                    .SqlParametros.Add("@id_delivery", SqlDbType.Int).Value = idDelivery
                End If
                dtDatosMaterial = .EjecutarDataTable("ObtenerInformacionDeliveryMaterialesJson", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try

        Return dtDatosMaterial
    End Function

    Public Function ObtenerInformacionDetalleDelivery(ByVal idDelivery As Integer) As List(Of MaterialDto)
        Dim _dbManager As New LMDataAccess
        Dim dtDatosMaterial As New DataTable
        Dim dtDatosSerial As New DataTable
        Dim materiales As New List(Of MaterialDto)

        Try
            With _dbManager
                If (idDelivery > 0) Then
                    .SqlParametros.Add("@id_delivery", SqlDbType.Int).Value = idDelivery
                End If
                dtDatosMaterial = .EjecutarDataTable("ObtenerInformacionMaterialesDelivery", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try

        For Each rowDatosMaterial As DataRow In dtDatosMaterial.Rows
            Dim _dbManagerDetalle As New LMDataAccess
            Dim material As New MaterialDto
            Dim seriales As New List(Of SerialDto)
            Try
                With _dbManagerDetalle
                    If (idDelivery > 0) Then
                        .SqlParametros.Add("@id_material", SqlDbType.Int).Value = rowDatosMaterial(0).ToString
                    End If
                    dtDatosSerial = .EjecutarDataTable("ObtenerInformacionSerialesDelivery", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManagerDetalle IsNot Nothing Then _dbManagerDetalle.Dispose()
            End Try

            For Each rowDatosSerial As DataRow In dtDatosSerial.Rows
                Dim serial As New SerialDto
                serial.serial = rowDatosSerial(0).ToString()
                seriales.Add(serial)
            Next
            material.sku = rowDatosMaterial(1).ToString
            material.cantidad = rowDatosMaterial(2).ToString
            material.seriales = seriales
            materiales.Add(material)
        Next

        Return materiales
    End Function

    Public Function ObtenerListadoComboTransportador(pTransportador As String, idDelivery As Integer) As DataTable
        Dim _dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As DataTable
        Try
            With _dbManager
                If (Not String.IsNullOrEmpty(pTransportador) AndAlso pTransportador.Length > 0) Then
                    .SqlParametros.Add("@transportador", SqlDbType.VarChar, 10).Value = pTransportador
                    .SqlParametros.Add("@id_delivery", SqlDbType.Int).Value = idDelivery
                End If
                dtDatos = .EjecutarDataTable("ObtenerListadoTransportadorfiltroCombo", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ConsultarEstado() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                dtDatos = .EjecutarDataTable("ConsultarEstadosDelivery", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ConsultarJornada() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                dtDatos = .EjecutarDataTable("ConsultarJornada", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ConsultarProcesoVenta() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                dtDatos = .EjecutarDataTable("ConsultarProcesoVenta", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

End Module
