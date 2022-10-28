Imports System.Text.RegularExpressions

Public Class RestDelivery

    Public Function NotificacionCambioEstado(ByVal idDelivery As Integer, ByVal estadoDelivery As Integer, Optional ByVal NovedadCRM As String = "") As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim estado As String = ""
        Dim subEstado As String = ""

        Select Case estadoDelivery
            Case Enumerados.EstadoServicio.Confirmado
                estado = "01_RECIBIDO"
            Case Enumerados.EstadoServicio.AsignadoRuta
                estado = "02_ASIGNADO"
            Case Enumerados.EstadoServicio.Recogido
                estado = "03_RECOGIDO"
            Case Enumerados.EstadoServicio.Transito
                estado = "04_REPARTO"
            Case Enumerados.EstadoServicio.Entregado
                estado = "05_ENTREGADO"
            Case Enumerados.EstadoServicio.Devolucion
                estado = "07_NO_ENTREGADO"
                subEstado = NovedadCRM
            Case Enumerados.EstadoServicio.Reagendado
                estado = "06_REAGENDADO"
            Case Enumerados.EstadoServicio.EntregaParcial
                estado = "08_ENTREGA_PARCIAL"
            Case Enumerados.EstadoServicio.NoCobertura
                estado = "09_NO_COBERTURA"
        End Select

        Dim servicioRest As ServicioRest = New ServicioRest()
        Dim objDelivery As New Delivery

        objDelivery = HerramientasDelivery.ObtenerInformacionGeneralDelivery(idDelivery)

        If objDelivery IsNot Nothing Then

            Dim orden As OrdenDto = New OrdenDto()
            orden.numero_orden = objDelivery.numeroOrden

            Dim alistamiento As AlistamientoDto = New AlistamientoDto()
            alistamiento.id_alistamiento = objDelivery.idAlistamiento

            Dim estadoEntrega As EstadoEntregaDto = New EstadoEntregaDto()
            estadoEntrega.estado = estado
            estadoEntrega.subestado = subEstado
            estadoEntrega.fecha_actualizacion = Format(Now, "short Date")

            Dim materiales As List(Of MaterialDto) = New List(Of MaterialDto)()
            materiales = HerramientasDelivery.ObtenerInformacionDetalleDelivery(idDelivery)

            Dim transportador As TransportadorDto = New TransportadorDto()

            If objDelivery.nombreTransportador IsNot Nothing Then
                transportador.nombre = objDelivery.nombreTransportador
            Else
                transportador.nombre = ""
            End If

            transportador.cedula = objDelivery.cedulaTransportador

            If objDelivery.placaTransportador IsNot Nothing Then
                transportador.placa_vehiculo = Regex.Replace(objDelivery.placaTransportador, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled)
            Else
                transportador.placa_vehiculo = "0"
            End If

            Dim pedidos As List(Of PedidoDto) = New List(Of PedidoDto)()
            Dim pedido As PedidoDto = New PedidoDto()
            If objDelivery.numeroGuia IsNot Nothing Then
                pedido.numero_guia = objDelivery.numeroGuia
            Else
                pedido.numero_guia = "0"
            End If
            pedido.numero_pedido = objDelivery.numeroPedido
            pedido.estado_entrega = estadoEntrega
            pedido.materiales = materiales
            pedido.transportador = transportador
            pedido.fecha_entrega_cliente = Format(objDelivery.fecha, "yyyy-MM-dd")
            pedido.fecha_reprogramacion = ""
            pedido.fecha_solicitud = ""
            pedidos.Add(pedido)

            Dim entrega As EntregaDto = New EntregaDto()
            entrega.id_process = objDelivery.numeroOrden
            entrega.orden = orden
            entrega.alistamiento = alistamiento
            entrega.pedidos = pedidos
            entrega.observacion = objDelivery.observacion

            Dim rootObject As RootObject = New RootObject()
            rootObject.entrega = entrega

            Dim url As String = Comunes.ConfigValues.seleccionarConfigValue("URL_SERVICIO_REST")
            Dim metodo As String = "entrega"
            Dim accion As String = "orden"
            Dim id As String = objDelivery.numeroOrden

            resultado = servicioRest.actualizacionServicioRestPut(url, rootObject, metodo, accion, id)
        Else
            resultado.Valor = 0
            resultado.Mensaje = resultado.Mensaje & "Objeto delivery vacio"
        End If

        Return resultado

    End Function

    Public Function NotificacionCambioEstadoMotorizado(ByVal idDelivery As Integer, ByVal IdMotorizado As Decimal) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim estado As String = "02_ASIGNADO"
        Dim subEstado As String = ""

        Dim servicioRest As ServicioRest = New ServicioRest()
        Dim objDelivery As New Delivery

        objDelivery = HerramientasDelivery.ObtenerInformacionGeneralDelivery(idDelivery)

        If objDelivery IsNot Nothing Then

            Dim orden As OrdenDto = New OrdenDto()
            orden.numero_orden = objDelivery.numeroOrden

            Dim alistamiento As AlistamientoDto = New AlistamientoDto()
            alistamiento.id_alistamiento = objDelivery.idAlistamiento

            Dim estadoEntrega As EstadoEntregaDto = New EstadoEntregaDto()
            estadoEntrega.estado = estado
            estadoEntrega.subestado = subEstado
            estadoEntrega.fecha_actualizacion = Format(Now, "short Date")

            Dim materiales As List(Of MaterialDto) = New List(Of MaterialDto)()
            materiales = HerramientasDelivery.ObtenerInformacionDetalleDelivery(idDelivery)

            Dim infoMoto As New serviceClaroSamsung.InfoMotorizado
            Dim dtMotorizado As New DataTable

            With infoMoto
                .Id = IdMotorizado
                dtMotorizado = .ConsultarMotorizado
            End With

            Dim transportador As TransportadorDto = New TransportadorDto()

            If dtMotorizado.Rows(0)("Nombre") IsNot Nothing Then
                transportador.nombre = dtMotorizado.Rows(0)("Nombre")
            Else
                transportador.nombre = ""
            End If

            If dtMotorizado.Rows(0)("Cedula") IsNot Nothing Then
                transportador.cedula = dtMotorizado.Rows(0)("Cedula")
            Else
                transportador.cedula = ""
            End If

            If dtMotorizado.Rows(0)("Placa") IsNot Nothing Then
                transportador.placa_vehiculo = dtMotorizado.Rows(0)("Placa")
            Else
                transportador.placa_vehiculo = ""
            End If

            Dim pedidos As List(Of PedidoDto) = New List(Of PedidoDto)()
            Dim pedido As PedidoDto = New PedidoDto()
            If objDelivery.numeroGuia IsNot Nothing Then
                pedido.numero_guia = objDelivery.numeroGuia
            Else
                pedido.numero_guia = "0"
            End If
            pedido.numero_pedido = objDelivery.numeroPedido
            pedido.estado_entrega = estadoEntrega
            pedido.materiales = materiales
            pedido.transportador = transportador
            pedido.fecha_entrega_cliente = Format(objDelivery.fecha, "yyyy-MM-dd")
            pedido.fecha_reprogramacion = ""
            pedido.fecha_solicitud = ""
            pedidos.Add(pedido)

            Dim entrega As EntregaDto = New EntregaDto()
            entrega.id_process = objDelivery.numeroOrden
            entrega.orden = orden
            entrega.alistamiento = alistamiento
            entrega.pedidos = pedidos
            entrega.observacion = objDelivery.observacion

            Dim rootObject As RootObject = New RootObject()
            rootObject.entrega = entrega

            Dim url As String = Comunes.ConfigValues.seleccionarConfigValue("URL_SERVICIO_REST")
            Dim metodo As String = "entrega"
            Dim accion As String = "orden"
            Dim id As String = objDelivery.numeroOrden

            resultado = servicioRest.actualizacionServicioRestPut(url, rootObject, metodo, accion, id)
        Else
            resultado.Valor = 0
            resultado.Mensaje = resultado.Mensaje & "Objeto delivery vacio"
        End If

        Return resultado

    End Function

End Class
