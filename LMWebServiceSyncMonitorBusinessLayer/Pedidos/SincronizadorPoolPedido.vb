Imports System.Configuration
Imports LMWebServiceSyncMonitorBusinessLayer.Puestos
Imports LMWebServiceSyncMonitorBusinessLayer.ClasesComunes
Imports LMMailSenderLibrary

Public Class SincronizadorPoolPedido

#Region "Atributos (Campos)"

    Private _dtPedido As DataTable
    Private _dtDetallePedido As DataTable
    Private _dtPedidoNoValido As DataTable
    Private _dtErrorSincronizacion As DataTable
    Private _resultado As New ResultadoProceso
    Private _listaPuestos As PuestosColeccion
    Private _fecha As Date
    Private _listaPedido As New ArrayList
    Private _rutaArchivoPedidosNoSincronizados As String

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _rutaArchivoPedidosNoSincronizados = ""
    End Sub

#End Region

#Region "Propiedades"

    Public Property Fecha() As Date
        Get
            Return _fecha
        End Get
        Set(ByVal value As Date)
            _fecha = value
        End Set
    End Property

    Public Property ListaPuestosExpedicion() As PuestosColeccion
        Get
            If _listaPuestos Is Nothing Then _listaPuestos = New PuestosColeccion
            Return _listaPuestos
        End Get
        Set(ByVal value As PuestosColeccion)
            _listaPuestos = value
        End Set
    End Property

    Public Property ListaPedido() As ArrayList
        Get
            Return _listaPedido
        End Get
        Set(ByVal value As ArrayList)
            _listaPedido = value
        End Set
    End Property

    Public ReadOnly Property HayPedidosNoSincronizados() As Boolean
        Get
            If _dtPedidoNoValido IsNot Nothing Then
                Return CBool(_dtPedidoNoValido.Rows.Count)
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property RutaArchivoDetalleErrorSincronizacion() As String
        Get
            Return _rutaArchivoPedidosNoSincronizados
        End Get
    End Property

    Public ReadOnly Property InfoPedidosNoValidos() As DataTable
        Get
            Return _dtPedidoNoValido
        End Get
    End Property

    Public ReadOnly Property InfoErroesSincronizacion() As DataTable
        Get
            Return _dtErrorSincronizacion
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub ObtenerPoolPedidoSAP()
        Dim wsPedidosSap As New SAPPoolPedidos.WS_PEDIDOS_LG
        Dim wsResultado As New SAPPoolPedidos.OutputLg
        Dim fechaConsulta As String = IIf(_fecha > Date.MinValue, _fecha.ToString("yyyyMMdd"), Now.ToString("yyyyMMdd"))
        Dim genCredenciales As New GeneradorCredencialesWebService

        Try
            Dim puestos(_listaPuestos.Count - 1) As SAPPoolPedidos.ZmmIntVstel
            Dim strValor As String = String.Empty
            For index As Integer = 0 To _listaPuestos.Count - 1
                puestos(index) = New SAPPoolPedidos.ZmmIntVstel
                puestos(index).vstel = _listaPuestos.Item(index).vstel
            Next

            Dim nombreClaseWS As String = wsPedidosSap.GetType().Name
            Dim infoWs As New InfoUrlWebService(wsPedidosSap, True)

            wsPedidosSap.Credentials = genCredenciales.Credenciales
            wsPedidosSap.Timeout = 600000
            wsResultado = wsPedidosSap.executeZmmLgPoolPedidosEntregas(Nothing, puestos, Nothing, Nothing, Nothing, Nothing)

            If wsResultado IsNot Nothing Then
                With wsResultado
                    If .oMensajes IsNot Nothing Then
                        If .oMensajes.Length > 0 Then
                            _resultado.Mensaje = ""
                            For index As Integer = 0 To wsResultado.oMensajes.Length - 1
                                If .oMensajes(index).type.ToUpper = "E" Or .oMensajes(0).type.ToUpper = "A" Then
                                    If .oMensajes(index).message <> "No existen datos" Then
                                        _resultado.Valor = 1
                                        _resultado.Mensaje += IIf(_resultado.Mensaje.Trim.Length > 0, vbNewLine, "") & _
                                            .oMensajes(index).type.ToUpper & ": " & .oMensajes(index).message.ToString
                                    End If
                                End If
                            Next
                        End If
                        If _resultado.Valor = 0 Then
                            If .rPedidosC.Length = 0 Then
                                _resultado.Valor = 1
                                _resultado.Mensaje = "No se obtuvo datos de pedido para sincronizar, con las condicion."
                            ElseIf .rPedidosD.Length = 0 Then
                                _resultado.Valor = 1
                                _resultado.Mensaje = "No se obtuvo detalle de pedido para sincronizar."
                            Else
                                LeerCabecerasDePedido(wsResultado.rPedidosC())
                                If _resultado.Valor = 0 AndAlso _dtPedido.Rows.Count > 0 Then LeerDetallesDePedido(wsResultado.rPedidosD())
                            End If

                        End If
                    Else
                        _resultado.EstablecerValorYMensaje(1, "No se obtuvo respuesta válida desde el WS. El Ws está retornando datos núlos")
                    End If
                End With
            Else
                _resultado.EstablecerValorYMensaje(1, "No se obtuvo respuesta válida desde el WS. La respuesta estaba vacía")
            End If
        Catch ex As Exception
            _resultado.EstablecerValorYMensaje(1, "Error al obtener datos del pool de pedidos: " & ex.Message)
        End Try
    End Sub

    Private Sub LeerCabecerasDePedido(ByVal cabeceras() As SAPPoolPedidos.ZmmLgPedidosC)
        If _dtPedido Is Nothing Then _dtPedido = GenerarEstructuraCabeceraPedido()
        Dim drPedido As DataRow
        Dim strEntrega As String
        Dim auxDatosDestino As New ArrayList
        Dim numPedido As Long = 0
        'Dim indice As Integer = 0
        Try
            For index As Integer = 0 To cabeceras.Length - 1
                Long.TryParse(cabeceras(index).pedido.Trim, numPedido)
                If (_listaPedido IsNot Nothing AndAlso _listaPedido.Count > 0 AndAlso _listaPedido.Contains(numPedido.ToString)) _
                    OrElse _listaPedido Is Nothing OrElse _listaPedido.Count = 0 Then
                    drPedido = _dtPedido.NewRow
                    If auxDatosDestino.Count > 0 Then auxDatosDestino.Clear()
                    auxDatosDestino.AddRange(Split(cabeceras(index).observaciones.Trim, "|"))
                    With drPedido
                        strEntrega = cabeceras(index).entrega.Trim.ToString
                        .Item("pedido") = numPedido
                        .Item("codCliente") = cabeceras(index).codCliente.Trim
                        If auxDatosDestino.Count > 0 Then .Item("direccion") = auxDatosDestino.Item(0)
                        If auxDatosDestino.Count >= 2 Then .Item("ciudad") = auxDatosDestino.Item(1)
                        If auxDatosDestino.Count >= 3 Then .Item("departamento") = auxDatosDestino.Item(2)
                        .Item("disponibilidad") = cabeceras(index).disponibilidad.Trim
                        .Item("entrega") = strEntrega
                        .Item("fechaHora") = ConvertirAFecha(cabeceras(index).fechaHora.Trim)
                        .Item("observaciones") = "" 'cabeceras(index).observaciones.Trim
                        .Item("ordenCompra") = cabeceras(index).ordenCompra.Trim
                        .Item("tipoPedido") = cabeceras(index).tipoPedido.Trim
                    End With
                    _dtPedido.Rows.Add(drPedido)
                    'indice = index
                End If
            Next

            If _dtPedido.Rows.Count > 0 AndAlso cabeceras.Length > _dtPedido.Rows.Count Then
                _resultado.EstablecerValorYMensaje(1, "Uno o más pedidos no existen en el pool para sincronizar. Por favor verifique.")
            End If

            _resultado.Valor = 0
        Catch ex As Exception
            _resultado.EstablecerValorYMensaje(1, "Error al tratar de leer cabeceras del pool de pedidos: " & ex.Message)
        End Try
    End Sub

    Private Sub LeerDetallesDePedido(ByVal detalles() As SAPPoolPedidos.ZmmLgPedidosD)
        If _dtDetallePedido Is Nothing Then _dtDetallePedido = GenerarEstructuraDetallePedido()
        Dim drDetallePedido As DataRow
        Dim cantidad, material As String
        Dim numPedido As Long = 0
        Try
            For index As Integer = 0 To detalles.Length - 1
                Long.TryParse(detalles(index).pedido.Trim.ToString, numPedido)
                If (_listaPedido IsNot Nothing AndAlso _listaPedido.Count > 0 AndAlso _
                    _listaPedido.Contains(numPedido.ToString)) OrElse _listaPedido Is Nothing _
                    OrElse _listaPedido.Count = 0 Then

                    material = detalles(index).material.TrimEnd.TrimStart("0")
                    cantidad = Replace(detalles(index).cantidad.Trim, ".", ",")

                    drDetallePedido = _dtDetallePedido.NewRow
                    With drDetallePedido
                        .Item("almacenDest") = detalles(index).almacenDest.Trim
                        .Item("cantidad") = CInt(cantidad)
                        .Item("centroDest") = detalles(index).centroDest.Trim
                        .Item("lote") = detalles(index).lote.Trim
                        .Item("material") = material
                        .Item("pedido") = CLng(detalles(index).pedido.Trim.ToString)
                        .Item("posicion") = detalles(index).posicion.Trim
                        .Item("puestoExp") = detalles(index).puestoExp.Trim
                        .Item("unidadVenta") = detalles(index).unidadVenta.Trim
                    End With
                    _dtDetallePedido.Rows.Add(drDetallePedido)
                End If
            Next
            _resultado.Valor = 0
        Catch ex As Exception
            _resultado.Valor = 1
            _resultado.Mensaje = "No se cargó estructura de detalles de los pedidos satisfactoriamente: " & ex.Message
        End Try
    End Sub

    Private Sub RegistrarTemporalmenteCabecerasDePedidos()
        Dim dm As New LMDataAccessLayer.LMDataAccess
        Try
            With dm
                .TiempoEsperaComando = 600
                .EjecutarNonQuery("TRUNCATE TABLE InfoSincronizacionPoolPedido ", CommandType.Text)
                Using dtAux As DataTable = _dtPedido.Copy
                    .InicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "InfoSincronizacionPoolPedido"
                        .ColumnMappings.Add("pedido", "pedido")
                        .ColumnMappings.Add("codCliente", "codCliente")
                        .ColumnMappings.Add("direccion", "direccion")
                        .ColumnMappings.Add("ciudad", "ciudad")
                        .ColumnMappings.Add("departamento", "departamento")
                        .ColumnMappings.Add("disponibilidad", "disponibilidad")
                        .ColumnMappings.Add("entrega", "entrega")
                        .ColumnMappings.Add("fechaHora", "fechaHora")
                        .ColumnMappings.Add("observaciones", "observaciones")
                        .ColumnMappings.Add("ordenCompra", "ordenCompra")
                        .ColumnMappings.Add("tipoPedido", "tipoPedido")
                        .WriteToServer(dtAux)
                    End With
                End Using
            End With
            If _dtPedido IsNot Nothing And _dtPedido.Rows.Count > 0 Then _dtPedido.Clear()
            _resultado.Valor = 0
        Catch ex As Exception
            _resultado.Valor = 1
            _resultado.Mensaje = " error al cargar pool de pedidos en tabla auxiliar: " & ex.Message
        Finally
            If dm IsNot Nothing Then dm.Dispose()
        End Try
    End Sub

    Private Sub RegistrarTemporalmenteDetallesDePedidos()
        Dim dm As New LMDataAccessLayer.LMDataAccess
        Try
            With dm
                .EjecutarNonQuery("TRUNCATE TABLE InfoSincronizacionDetallePoolPedido ", CommandType.Text)
                Using dtAux As DataTable = _dtDetallePedido.Copy
                    .InicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "InfoSincronizacionDetallePoolPedido"
                        .ColumnMappings.Add("pedido", "pedido")
                        .ColumnMappings.Add("material", "material")
                        .ColumnMappings.Add("cantidad", "cantidadSolicitada")
                        .ColumnMappings.Add("almacenDest", "almacenDest")
                        .ColumnMappings.Add("centroDest", "centroDest")
                        .ColumnMappings.Add("lote", "lote")
                        .ColumnMappings.Add("posicion", "posicion")
                        .ColumnMappings.Add("puestoExp", "puestoExp")
                        .ColumnMappings.Add("unidadVenta", "unidadVenta")
                        .WriteToServer(dtAux)
                    End With
                End Using
            End With
            If _dtDetallePedido IsNot Nothing And _dtDetallePedido.Rows.Count > 0 Then _dtDetallePedido.Clear()
            _resultado.Valor = 0
        Catch ex As Exception
            _resultado.EstablecerValorYMensaje(1, " error al cargar detalles del pool de pedidos en tabla auxiliar: " & ex.Message)
        Finally
            If dm IsNot Nothing Then dm.Dispose()
        End Try
    End Sub

    Private Function GenerarEstructuraCabeceraPedido() As DataTable
        Dim dtEstructuraDatos As New DataTable
        With dtEstructuraDatos
            .Columns.Add("pedido", GetType(String))
            .Columns.Add("codCliente", GetType(String))
            .Columns.Add("direccion", GetType(String))
            .Columns.Add("ciudad", GetType(String))
            .Columns.Add("departamento", GetType(String))
            .Columns.Add("disponibilidad", GetType(String))
            .Columns.Add("entrega", GetType(String))
            .Columns.Add("fechaHora", GetType(String))
            .Columns.Add("observaciones", GetType(String))
            .Columns.Add("ordenCompra", GetType(String))
            .Columns.Add("tipoPedido", GetType(String))
        End With
        Return dtEstructuraDatos
    End Function

    Private Function GenerarEstructuraDetallePedido() As DataTable
        Dim dtEstructuraDatos As New DataTable
        With dtEstructuraDatos
            .Columns.Add("almacenDest", GetType(String))
            .Columns.Add("cantidad", GetType(Integer))
            .Columns.Add("centroDest", GetType(String))
            .Columns.Add("lote", GetType(String))
            .Columns.Add("material", GetType(String))
            .Columns.Add("pedido", GetType(String))
            .Columns.Add("posicion", GetType(String))
            .Columns.Add("puestoExp", GetType(String))
            .Columns.Add("unidadVenta", GetType(String))
        End With
        Return dtEstructuraDatos
    End Function

    Private Function ConvertirAFecha(ByVal strCadena As String) As DateTime
        Dim cadenaFecha As DateTime

        Dim iAno As Integer = System.Convert.ToInt32(strCadena.Substring(0, 4))
        Dim iMes As Integer = System.Convert.ToInt32(strCadena.Substring(4, 2))
        Dim iDia As Integer = System.Convert.ToInt32(strCadena.Substring(6, 2))
        cadenaFecha = New DateTime(iAno, iMes, iDia)
        If strCadena.Length > 8 Then
            Dim iHora As Integer = System.Convert.ToInt32(strCadena.Substring(8, 2))
            Dim iMinutos As Integer = System.Convert.ToInt32(strCadena.Substring(10, 2))
            Dim iSegundos As Integer = System.Convert.ToInt32(strCadena.Substring(12, 2))
            cadenaFecha = New DateTime(iAno, iMes, iDia, iHora, iMinutos, iSegundos)
        End If
        Return cadenaFecha
    End Function

    Private Function CrearEstructuraPedidoErroresSincronizacion() As DataTable
        Dim dtAux As New DataTable("PedidoErroresSincronizacion")
        With dtAux.Columns
            .Add("pedido", GetType(String))
            .Add("entrega", GetType(String))
            .Add("descripcion", GetType(String))
        End With
        Return dtAux
    End Function

    Private Function ValidarPoolTemporalDePedidos() As DataSet
        Dim dsAux As New DataSet
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            With dbManager
                .TiempoEsperaComando = 900000
                dsAux = .EjecutarDataSet("ValidarPoolTemporalDePedidos", CommandType.StoredProcedure)
            End With
            dsAux.Tables(0).TableName = "PedidoNoValido"
            dsAux.Tables(1).TableName = "PedidoValido"
            dsAux.Tables(2).TableName = "PedidoValidoDetalle"

            ActualizarLogErrorSincronizacionesAnteriores()

        Catch ex As Exception
            _resultado.EstablecerValorYMensaje(1, " error al tratar de validar Pool Temporal de Pedidos. " & ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dsAux
    End Function

    Private Sub GenerarEntregaEnSAP(ByVal dtPedido As DataTable)
        If _dtPedidoNoValido Is Nothing Then _dtPedidoNoValido = CrearEstructuraPedidoErroresSincronizacion()
        Dim wsPedidoSap As New SAPPoolPedidos.WS_PEDIDOS_LG
        Dim wsResultado As New SAPPoolPedidos.OutputLg
        Dim wsInfoCabecera(0) As SAPPoolPedidos.ZmmLgPedidosC
        Dim wsInfoDetalle() As SAPPoolPedidos.ZmmLgPedidosD
        Dim dvDetallePedido As DataView = Me._dtDetallePedido.DefaultView
        Dim genCredenciales As New GeneradorCredencialesWebService
        Dim strMensajeError As String = String.Empty
        Dim objRegion As New Region
        Dim numeroEntrega As String = String.Empty
        Dim numeroPedido As String = ""
        Dim nombreClaseWS As String = wsPedidoSap.GetType().Name
        Dim infoWs As New InfoUrlWebService(wsPedidoSap, True)
        If infoWs.Registrado AndAlso infoWs.Url IsNot Nothing AndAlso infoWs.Url.Trim.Length > 0 Then
            wsPedidoSap.Url = infoWs.Url
        Else
            Throw New Exception("No se encontró registro de la dirección URL asociada al Web Service de nombre: " & nombreClaseWS)
        End If

        Try
            For Each drAux As DataRow In dtPedido.Rows
                numeroPedido = drAux("pedido").ToString
                strMensajeError = String.Empty
                If drAux("entrega").ToString.Trim.Length = 0 Then
                    wsInfoCabecera(0) = New SAPPoolPedidos.ZmmLgPedidosC
                    With wsInfoCabecera(0)
                        .disponibilidad = "X" 'Afecta Disponibilidad
                        .pedido = drAux("pedido").ToString
                        .tipoPedido = drAux("tipoPedido").ToString
                        .codCliente = drAux("codCliente").ToString
                    End With
                    dvDetallePedido.RowFilter = "pedido='" & drAux("pedido").ToString & "'"
                    ReDim wsInfoDetalle(dvDetallePedido.Count - 1)
                    For index As Integer = 0 To dvDetallePedido.Count - 1
                        wsInfoDetalle(index) = New SAPPoolPedidos.ZmmLgPedidosD
                        objRegion = New Region(dvDetallePedido(index).Item("puestoExp").ToString)
                        With wsInfoDetalle(index)
                            .pedido = dvDetallePedido(index).Item("pedido")
                            .material = dvDetallePedido(index).Item("material")
                            .posicion = dvDetallePedido(index).Item("posicion")
                            .puestoExp = dvDetallePedido(index).Item("puestoExp")
                            .centroDest = IIf(objRegion.Centro.Trim.Length > 0, objRegion.Centro, String.Empty)
                            .almacenDest = IIf(objRegion.Almacen.Trim.Length > 0, objRegion.Almacen, String.Empty)
                            .cantidad = dvDetallePedido(index).Item("cantidadSolicitada")
                            .unidadVenta = dvDetallePedido(index).Item("unidadVenta")
                        End With

                    Next

                    Dim puestoExp(0) As SAPPoolPedidos.ZmmIntVstel

                    puestoExp(0) = New SAPPoolPedidos.ZmmIntVstel
                    puestoExp(0).vstel = dvDetallePedido(0).Item("puestoExp").ToString

                    wsPedidoSap.Credentials = genCredenciales.Credenciales
                    wsResultado = wsPedidoSap.executeZmmLgPoolPedidosEntregas(Nothing, puestoExp, "X", wsInfoCabecera, wsInfoDetalle, Nothing)
                    If wsResultado IsNot Nothing Then
                        If wsResultado.oMensajes IsNot Nothing AndAlso wsResultado.oMensajes.Length > 0 Then
                            If wsResultado.oMensajes(0).type <> "E" And wsResultado.oMensajes(0).type <> "A" Then
                                numeroEntrega = wsResultado.oMensajes(0).messageV1.Trim
                                If Not String.IsNullOrEmpty(numeroEntrega) AndAlso CLng(numeroEntrega) > 0 Then
                                    drAux("entrega") = Long.Parse(numeroEntrega)
                                    ActualizarEntregaEnTablaTemporal(CLng(numeroPedido), CLng(numeroEntrega))
                                    If _resultado.Valor <> 0 Then
                                        RegistrarErrorSincronizacion(CLng(numeroPedido), IIf(numeroEntrega.Trim.Length > 0, numeroEntrega, 0), _resultado.Mensaje)
                                        _resultado.EstablecerValorYMensaje(0, "")
                                    End If
                                Else
                                    strMensajeError = " no se generó entrega. El WS está retornando datos núlos."
                                End If
                            Else
                                strMensajeError = wsResultado.oMensajes(0).message
                            End If
                        Else
                            strMensajeError = " no se obtuvo respuesta válida desde el WS. El WS está retornando datos núlos"
                        End If
                    Else
                        strMensajeError = " no se obtuvo respuesta válida desde el WS. La respuesta estaba vacía"
                    End If
                End If
                If strMensajeError.Trim.Length <> 0 Then RegistrarErrorSincronizacion(CLng(drAux("pedido").ToString), IIf(numeroEntrega.Trim.Length > 0, numeroEntrega, 0), strMensajeError)
            Next
            _dtPedido = dtPedido
        Catch ex As Exception
            RegistrarErrorSincronizacion(CLng(numeroPedido), IIf(numeroEntrega.Trim.Length > 0, numeroEntrega, 0), _
                                         " Error al tratar de crear Entregas en SAP. " & ex.Message)
        End Try
    End Sub

    Public Sub GenerarEnviarDatosDeNotificacion(ByVal dsInformacion As DataSet)
        Dim arrDestinatarios, urlAdjuntos As New ArrayList
        _dtPedidoNoValido = dsInformacion.Tables("PedidoNoValido")
        If _dtPedidoNoValido IsNot Nothing AndAlso _dtPedidoNoValido.Rows.Count > 0 Then
            Dim configuarcionNotificacion As New NotificacionConfiguracion
            With configuarcionNotificacion
                .TextoMensaje = "Adjunto se envía listado de pedidos no válidos para realizar sincronización del pool de pedidos desde SAP."
                .ObtenerPorId(6)
                GenerarAdjuntoExcel(_dtPedidoNoValido, "Reporte " & .Titulo, .Titulo, urlAdjuntos)
                EnviarNotificacion("Envío notificación " & .Titulo, .CuerpoMensaje, .DestinarariosPP, .DestinarariosCC, urlAdjuntos)
            End With
        End If
        If _dtErrorSincronizacion IsNot Nothing AndAlso _dtErrorSincronizacion.Rows.Count > 0 Then
            Dim configuarcionNotificacion As New NotificacionConfiguracion
            With configuarcionNotificacion
                .TextoMensaje = "Adjunto se envía pedidos con errores generados durante la sincronización."
                .ObtenerPorId(7)
                GenerarAdjuntoExcel(_dtErrorSincronizacion, "Reporte " & .Titulo, .Titulo, urlAdjuntos)
                EnviarNotificacion("Envío notificación " & .Titulo, .CuerpoMensaje, .DestinarariosPP, .DestinarariosCC, urlAdjuntos)
            End With
        End If
    End Sub

    Public Sub ExportarInfomeErrores(ByVal nombrearchivo As String, ByVal titulo As String)
        Dim ruta As String
        Dim configuarcionNotificacion As New NotificacionConfiguracion
        Try
            ruta = MetodosComunes.GetStartupPath()
            ruta = ruta & "\" & nombrearchivo & ".xls"
            MetodosComunes.GenerarReportesEnExcel(_dtPedidoNoValido, ruta, nombrearchivo, "Pedidos no válidos", Nothing, False, True)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub GenerarAdjuntoExcel(ByVal dtDatos As DataTable, ByVal NombreRutaArchivo As String, ByVal titulo As String, ByRef urlAdjunto As ArrayList)
        Dim ruta As String
        Try
            urlAdjunto.Clear()
            ruta = MetodosComunes.GetStartupPath()
            NombreRutaArchivo = ruta & "\" & NombreRutaArchivo & ".xls"

            MetodosComunes.GenerarReportesEnExcel(dtDatos, NombreRutaArchivo, titulo, titulo)
            urlAdjunto.Add(NombreRutaArchivo)

        Catch ex As Exception
            Throw New Exception(" error al generar archivo de Excel: " & ex.Message)
        End Try

    End Sub

    Public Sub EnviarNotificacion(ByVal asunto As String, ByVal cuerpo As String, ByVal destinatarios As ArrayList, ByVal destinatariosCopia As ArrayList, Optional ByVal adjunto As ArrayList = Nothing)
        Dim EnvioNotificacion As LMMailSender = InicializarMailManager()
        Dim datosMail As New ArrayList
        With EnvioNotificacion
            .AdjuntoUrl = adjunto
            .Asunto = asunto
            .Cuerpo = cuerpo
            For index As Integer = 0 To destinatarios.Count - 1
                datosMail.Clear()
                datosMail.AddRange(Split(destinatarios(index), ";"))
                .AdicionarDestinatario(datosMail(0), datosMail(1))
            Next

            For index As Integer = 0 To destinatariosCopia.Count - 1
                datosMail.Clear()
                datosMail.AddRange(Split(destinatariosCopia(index), ";"))
                .AdicionarDestinatarioCopia(datosMail(0), datosMail(1))
            Next
            .AdjuntarArchivos()
            .Enviar()
            .LimpiarTodosLosDestinatarios()
        End With

    End Sub

    Private Function InicializarMailManager() As LMMailSender
        Dim mailHandler As New LMMailSender

        If ConfigurationManager.AppSettings("mailServer") IsNot Nothing Then _
            mailHandler.ServidorCorreo = ConfigurationManager.AppSettings("mailServer")
        If ConfigurationManager.AppSettings("credenciales") IsNot Nothing Then
            Dim credenciales() As String
            credenciales = ConfigurationManager.AppSettings("credenciales").Split(";")
            mailHandler.EstablecerCredenciales(credenciales(0), credenciales(1), credenciales(2))
        End If
        If ConfigurationManager.AppSettings("mailSender") IsNot Nothing Then
            Dim cuentaOrigen As String = ConfigurationManager.AppSettings("mailSender")
            mailHandler.EstablecerCuentaOrigen("system.notifier <" & cuentaOrigen & ">")
        End If
        mailHandler.Prioridad = Net.Mail.MailPriority.High
        mailHandler.CuerpoEsHtml = True

        Return mailHandler
    End Function

    Private Sub CrearPedidosSincronizacion()
        Dim dm As New LMDataAccessLayer.LMDataAccess
        Try
            With dm
                .TiempoEsperaComando = 1200
                _dtPedido = .EjecutarDataTable("ObtenerInfoPedidoValidoSincronizacion", CommandType.StoredProcedure)
                If _dtPedido IsNot Nothing AndAlso _dtPedido.Rows.Count > 0 Then
                    .EjecutarNonQuery("RegistrarPedidosSincronizadosDesdeCliente", CommandType.StoredProcedure)
                End If
            End With
        Catch ex As Exception
            _resultado.EstablecerValorYMensaje(1, "Error al registrar pedidos " & ex.Message)
        Finally
            If dm IsNot Nothing Then dm.Dispose()
        End Try
    End Sub

    Private Function ObtenerDetallePorPedido(Optional ByVal lPedido As Long = 0) As DataTable
        Dim dtDetallePorPedido As New DataTable
        Dim xdm As New LMDataAccessLayer.LMDataAccess
        Try
            With xdm
                If lPedido <> 0 Then
                    .TiempoEsperaComando = 900
                    .AgregarParametroSQL("@pedido", lPedido, SqlDbType.BigInt)
                    dtDetallePorPedido = .EjecutarDataTable("ObtenerInfoDetallePedidoValidoSincronizacion", CommandType.StoredProcedure)
                Else
                    Throw New Exception(" error al obtener detalle del pedido " & lPedido)
                End If
            End With
            Return dtDetallePorPedido
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If xdm IsNot Nothing Then xdm.Dispose()
        End Try
    End Function

    Private Sub EstablecerParametrosCreacionPedido(ByVal dm As LMDataAccessLayer.LMDataAccess)
        With dm
            .SqlParametros.Add("@idTipoPedido", SqlDbType.SmallInt)
            .SqlParametros.Add("@idCliente", SqlDbType.Int)
            .SqlParametros.Add("@fechaHora", SqlDbType.VarChar, 50)
            .SqlParametros.Add("@ordenCompra", SqlDbType.VarChar, 50)
            .SqlParametros.Add("@observaciones", SqlDbType.VarChar, 250)
            .SqlParametros.Add("@idPedidoClienteExterno", SqlDbType.BigInt)
            .SqlParametros.Add("@direccionDestino", SqlDbType.VarChar, 150)
            .SqlParametros.Add("@idCiudadDestino", SqlDbType.Int)
            .SqlParametros.Add("@idEntregaClienteExterno", SqlDbType.BigInt)
            .SqlParametros.Add("@sincronizado", SqlDbType.Bit)
            .SqlParametros.Add("@idPedidoCreado", SqlDbType.Int)
            .SqlParametros.Add("@errorMensaje", SqlDbType.VarChar, 250)
        End With

    End Sub

    Private Sub ActualizarEntregasEnTablaTemporal(ByVal dtPedido As DataTable)
        Dim dm As New LMDataAccessLayer.LMDataAccess
        Try
            If _dtErrorSincronizacion Is Nothing Then _dtErrorSincronizacion = CrearEstructuraPedidoErroresSincronizacion()
            With dm
                .TiempoEsperaComando = 1200
                .SqlParametros.Add("@pedido", SqlDbType.BigInt)
                .SqlParametros.Add("@entrega", SqlDbType.BigInt)
                .SqlParametros.Add("@errorMensaje", SqlDbType.VarChar, 250)
                For Each drPedido As DataRow In dtPedido.Rows
                    .SqlParametros.Item("@Pedido").Value = CLng(drPedido.Item("pedido").ToString)
                    .SqlParametros.Item("@entrega").Value = CLng(IIf(drPedido.Item("entrega").ToString.Trim = "", 0, drPedido.Item("entrega").ToString))
                    .SqlParametros.Item("@errorMensaje").Direction = ParameterDirection.Output
                    .EjecutarNonQuery("ActualizaEntregaPedidoPool", CommandType.StoredProcedure)
                    If .SqlParametros.Item("@errorMensaje").Value.ToString.Trim.Length > 0 Then
                        RegistrarErrorSincronizacion(drPedido.Item("pedido"), CLng(drPedido.Item("entrega")), "No se actualizó correctamente el dato entrega: " & .SqlParametros.Item("@errorMensaje").Value)
                    End If
                Next
                .SqlParametros.Clear()
                _dtPedido = .EjecutarDataTable("ObtenerInfoPedidoValidoSincronizacion", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            _resultado.EstablecerValorYMensaje(1, " error al actualizar entregas de pedidos en tabla temporal: " & ex.Message)
        Finally
            If dm IsNot Nothing Then dm.Dispose()
        End Try

    End Sub

    Private Sub ActualizarEntregaEnTablaTemporal(ByVal numeroPedido As Long, ByVal numeroEntrega As Long)
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            If _dtErrorSincronizacion Is Nothing Then _dtErrorSincronizacion = CrearEstructuraPedidoErroresSincronizacion()
            With dbManager
                .TiempoEsperaComando = 1200
                .SqlParametros.Add("@pedido", SqlDbType.BigInt).Value = numeroPedido
                .SqlParametros.Add("@entrega", SqlDbType.BigInt).Value = numeroEntrega
                .SqlParametros.Add("@errorMensaje", SqlDbType.VarChar, 250).Direction = ParameterDirection.Output
                .EjecutarNonQuery("ActualizaEntregaPedidoPool", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros.Item("@errorMensaje").Value) AndAlso _
                    .SqlParametros.Item("@errorMensaje").Value.ToString.Trim.Length > 0 Then
                    RegistrarErrorSincronizacion(numeroPedido, numeroEntrega, "No se actualizó correctamente el dato entrega: " & .SqlParametros.Item("@errorMensaje").Value)
                End If
            End With
        Catch ex As Exception
            _resultado.EstablecerValorYMensaje(1, " Error al actualizar numero de entrega en tabla temporal: " & ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

    End Sub

    Private Sub RegistrarErrorSincronizacion(ByVal pedido As Long, ByVal entrega As Long, ByVal descripcion As String)
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            With dbManager
                .TiempoEsperaComando = 900
                .SqlParametros.Add("@pedido", SqlDbType.BigInt).Value = pedido
                .SqlParametros.Add("@entrega", SqlDbType.BigInt).Value = entrega
                .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 250).Value = descripcion
                .EjecutarNonQuery("RegistrarErrorSincronizacion", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            _resultado.EstablecerValorYMensaje(1, " error al registrar en log de errores." & ex.Message)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Private Sub ActualizarLogErrorSincronizacionesAnteriores(Optional ByVal iIdLog As Integer = 0, Optional ByVal lPedido As Long = 0)
        Dim dm As New LMDataAccessLayer.LMDataAccess
        Try
            With dm.SqlParametros
                If iIdLog <> 0 Then .Add("@idLogErrorCreacionPedidoSincronizacion", SqlDbType.Int).Value = iIdLog
                If lPedido <> 0 Then .Add("@pedido", SqlDbType.BigInt).Value = lPedido
            End With
            dm.TiempoEsperaComando = 900
            dm.EjecutarNonQuery("ActualizarLogErrorCreacionPedidoSincronizacion", CommandType.StoredProcedure)
        Catch ex As Exception
            _resultado.EstablecerValorYMensaje(1, " error al actualizar log de errores." & ex.Message)
        Finally
            If dm IsNot Nothing Then dm.Dispose()
        End Try
    End Sub

    'Private Sub DepurarListaPedidosASincronizar()
    '    Dim dm As New LMDataAccessLayer.LMDataAccess
    '    Try
    '        With dm
    '            .iniciarTransaccion()
    '            .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 1000).Value = Join(_listaPedido.ToArray, ",")
    '            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
    '            .ejecutarNonQuery("DepurarPoolTemporalDePedidos", CommandType.StoredProcedure)
    '            _resultado.Valor = .SqlParametros("@resultado").Value
    '        End With

    '        With _resultado
    '            If .Valor = 0 Then
    '                dm.confirmarTransaccion()
    '            ElseIf .Valor = 2 Then
    '                .EstablecerValorYMensaje(.Valor, " la lista de pedidos a sincronizar esta vacía.")
    '            Else
    '                .EstablecerValorYMensaje(.Valor, " error al depurar pool temporal de pedidos.")
    '            End If
    '        End With
    '    Catch ex As Exception
    '        _resultado.EstablecerValorYMensaje(1, ex.Message)
    '        If dm.estadoTransaccional Then dm.abortarTransaccion()
    '    Finally
    '        If dm IsNot Nothing Then dm.Dispose()
    '    End Try
    'End Sub

    Private Sub LimpiarEstructurasDeSincronizacion()
        If _dtPedido IsNot Nothing AndAlso _dtPedido.Rows.Count > 0 Then _dtPedido.Rows.Clear()
        If _dtDetallePedido IsNot Nothing AndAlso _dtDetallePedido.Rows.Count > 0 Then _dtDetallePedido.Rows.Clear()
        If _dtPedidoNoValido IsNot Nothing AndAlso _dtPedidoNoValido.Rows.Count > 0 Then _dtPedidoNoValido.Rows.Clear()
        If _dtErrorSincronizacion IsNot Nothing AndAlso _dtErrorSincronizacion.Rows.Count > 0 Then _dtErrorSincronizacion.Rows.Clear()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Sincronizar() As ResultadoProceso
        Dim esIntentoOK As Boolean = False

        LimpiarEstructurasDeSincronizacion()

        If _listaPedido Is Nothing OrElse _listaPuestos Is Nothing OrElse _listaPuestos.Count = 0 Then _
            _listaPuestos = New PuestosColeccion

        For index As Integer = 1 To 3
            _resultado.Valor = 0
            ObtenerPoolPedidoSAP()
            If _resultado.Valor = 0 Then Exit For
        Next

        If _resultado.Valor = 0 Then
            If _dtPedido IsNot Nothing AndAlso _dtPedido.Rows.Count > 0 Then
                RegistrarTemporalmenteCabecerasDePedidos()
                If _resultado.Valor = 0 Then
                    RegistrarTemporalmenteDetallesDePedidos()
                    'If _listaPedido IsNot Nothing AndAlso _listaPedido.Count > 0 Then DepurarListaPedidosASincronizar()
                    If _resultado.Valor = 0 Then
                        Dim dsAux As DataSet = ValidarPoolTemporalDePedidos()
                        If _resultado.Valor = 0 Then
                            If dsAux IsNot Nothing Then
                                _dtPedido = dsAux.Tables("PedidoValido")
                                If _dtPedido IsNot Nothing AndAlso _dtPedido.Rows.Count > 0 Then
                                    _dtDetallePedido = dsAux.Tables("PedidoValidoDetalle")
                                    GenerarEntregaEnSAP(_dtPedido)
                                    CrearPedidosSincronizacion()
                                    If _dtErrorSincronizacion IsNot Nothing AndAlso _dtErrorSincronizacion.Rows.Count > 0 Then _
                                        dsAux.Tables.Add(_dtErrorSincronizacion)

                                    'If _resultado.Valor = 0 Then
                                    '    ActualizarEntregasEnTablaTemporal(_dtPedido)
                                    '    If _resultado.Valor = 0 Then
                                    '        CrearPedidosSincronizacion()
                                    '    Else
                                    '        Throw New Exception(_resultado.Mensaje)
                                    '    End If
                                    '    dsAux.Tables.Add(_dtErrorSincronizacion)
                                    'Else
                                    '    Throw New Exception(_resultado.Mensaje)
                                    'End If
                                Else
                                    _resultado.EstablecerValorYMensaje(1, "No se obtuvo pedidos válidos para registrar.")
                                End If

                                If dsAux IsNot Nothing AndAlso dsAux.Tables(0).Rows.Count > 0 Then
                                    Try
                                        GenerarEnviarDatosDeNotificacion(dsAux)
                                    Catch ex As Exception
                                        _resultado.EstablecerValorYMensaje(2, "No fue posible enviar notificación de errores" & ex.Message & ", se activara el link para el detalle de la notificación.")
                                    End Try

                                End If

                            Else
                                _resultado.EstablecerValorYMensaje(1, "No se obtuvo información de la validación del Pool Pedidos.")
                            End If
                        Else
                            _resultado.EstablecerValorYMensaje(1, " No fue posible validar Pool de Pedidos, " & _resultado.Mensaje)
                        End If
                    Else
                        _resultado.EstablecerValorYMensaje(1, " No fue posible registrar infomración temporal, " & _resultado.Mensaje)
                    End If
                Else
                    _resultado.EstablecerValorYMensaje(1, "  No fue posible registrar infomración temporal, " & _resultado.Mensaje)
                End If
            Else
                _resultado.EstablecerValorYMensaje(200, "No se encontraron pedidos en el Pool para sincronizar con las condiciones dadas.")
            End If
        Else
            _resultado.EstablecerValorYMensaje(1, "Se agotó el numero de intentos, " & _resultado.Mensaje)
        End If
        Return _resultado
    End Function

#End Region


End Class

