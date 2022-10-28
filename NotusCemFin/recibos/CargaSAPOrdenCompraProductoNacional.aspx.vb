Imports ILSBusinessLayer
Imports ILSBusinessLayer.Recibos
Imports BPColSysOP.SAPContabilizacionEntrada
Imports LMWebServiceSyncMonitorBusinessLayer
Imports System.Collections.Generic
Imports System.IO
Imports ILSBusinessLayer.SAPContabilizacionEntrada

Partial Public Class CargaSAPOrdenCompraProductoNacional
    Inherits System.Web.UI.Page
    Private Shared tipoCaso As Integer '1 -> Cargar no serializada, 2 -> Carga serializada por materiales (Tarjetas Prepagos --Tabla InfoTarjetaPrepago), 3 -> Cargar serializada por dSeriales (Token --Tabla productos_serial), 4-> Carga serializada con materiales (Bonos --Tabla InfoCargueProductoSAP)
    Private Shared idOrdenRecepcion As Long

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            Seguridad.verificarSession(Me)
            Page.Server.ScriptTimeout = 10800
            epPrincipal.clear()
            If Not IsPostBack Then
                epPrincipal.showReturnLink("~/recibos/BuscarOrdenRecepcion.aspx")
                pnlContenedorOpciones.Visible = False
                epPrincipal.setTitle("Cargue en SAP de Producto Recibido Por Compra Nacional")
                If Request.QueryString("idrep") IsNot Nothing Then
                    idOrdenRecepcion = CInt(Request.QueryString("idrep"))
                    If idOrdenRecepcion > 0 Then
                        hfIdOrdenRecepcion.Value = idOrdenRecepcion
                        CargarDatosOrdenCompra()
                        pnlContenedorOpciones.Visible = True
                    Else
                        epPrincipal.showWarning("No se encontraron datos para la Orden de Recepción proporcionada.")
                    End If
                Else
                    epPrincipal.showWarning("No se encontraron datos para la Orden de Recepción proporcionada.")
                End If
            End If
        Catch ex As Exception
            epPrincipal.showError("Error en la cargar. " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Carga los datos de presentacion de orden de compra de BpColSys
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarDatosOrdenCompra()
        Try
            If Not String.IsNullOrEmpty(hfIdOrdenRecepcion.Value) Then Integer.TryParse(hfIdOrdenRecepcion.Value, idOrdenRecepcion)
            Dim ordRecepcion As New Recibos.OrdenRecepcion(idOrdenRecepcion)
            Dim tipoProductoObj As New Productos.TipoProducto(ordRecepcion.IdTipoProducto)
            tipoCaso = tipoProductoObj.IdTipoCargue

            Session.Remove("dtInfoRecepcion")
            Session.Remove("dtInfoOcSAP")
            If ordRecepcion.IdOrdenRecepcion > 0 Then

                If ordRecepcion.IdOrdenCompra <> 0 Then
                    With ordRecepcion.OrdenCompra
                        lblNumeroOrden.Text = .NumeroOrden
                        lblObservacion.Text = .Observacion
                        lblIdOrdenRecepcion.Text = ordRecepcion.IdOrdenRecepcion
                    End With
                    '**** Numero para pruebas en desarrollo 4500023169

                    'Token (ZME5)	Por lote y seriales 	4500070984
                    'Pacas o tarjetas amigo o tarjetas prepago (ZME2)	Por lotes	4500070986
                    'Bonos (ZNVR)	Por seriales 	4500070982
                    'Merchandising (ZMER)	 	4500070990
                    'Insumos (ZNVA)	 	4500070988
                    'Papelería (ZNVA)	 	4500071014
                    'Publicidad (ZNVA)	 	4500071019

                    lblNumeroOrden.Text = ordRecepcion.OrdenCompra.NumeroOrden
                    lblRemision.Text = ordRecepcion.Remision
                    If tipoCaso = 3 Then
                        lblNotaEntrega.Text = ordRecepcion.Factura
                        lblTextoCabecera.Text = ordRecepcion.Guia
                    Else
                        lblNotaEntrega.Text = "REM " & ordRecepcion.Remision
                        lblTextoCabecera.Text = "PROV " & ordRecepcion.Proveedor
                    End If
                    ConsultarInfoOrdenCompraEnSAP()
                Else
                    pnlContenedorOpciones.Visible = False
                End If
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al tratar de cargar la información de la Orden de Recepción y/o de la Orden de Compra desde la BD. " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Carga la información asociada a la orden de recepción actual.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarDatosOrdenRecepcion()
        Try
            Dim dtDetalleRecepcion As New DataTable
            dtDetalleRecepcion = ObtenerInformacionRecepcion()
            If dtDetalleRecepcion.Rows.Count > 0 Then
                gvInfo.DataSource = dtDetalleRecepcion
                gvInfo.DataBind()
                gvInfo.Visible = True
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Permite obtener el listado de Regiones registradas en el sistema desde la BD o desde memoria
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ObtenerListadoRegion() As DataTable
        Dim dtRegion As DataTable
        Try
            If Session("dtRegion") IsNot Nothing Then
                dtRegion = CType(Session("dtRegion"), DataTable)
            Else
                dtRegion = Region.ObtenerTodas()
                Dim pkRegion() As DataColumn = {dtRegion.Columns("centro")}
                dtRegion.PrimaryKey = pkRegion
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al tratar de cargar el listado de Centros Propios. " & ex.Message)
        End Try
        Return dtRegion
    End Function

    ''' <summary>
    ''' Permite obtener el listado de la informacion para la recepción actual.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function ObtenerInformacionRecepcion() As DataTable
        Dim dtDetalleRecepcion As DataTable
        Try
            If Session("dtInfoRecepcion") IsNot Nothing Then
                dtDetalleRecepcion = CType(Session("dtInfoRecepcion"), DataTable)
                If (dtDetalleRecepcion.Rows.Count <= 0) Then
                    Dim ordenRecepcionObj As New Recibos.OrdenRecepcion(CLng(hfIdOrdenRecepcion.Value))
                    dtDetalleRecepcion = ordenRecepcionObj.Material
                    Session("dtInfoRecepcion") = dtDetalleRecepcion
                End If
            Else
                Dim ordenRecepcionObj As New Recibos.OrdenRecepcion(CLng(hfIdOrdenRecepcion.Value))
                dtDetalleRecepcion = ordenRecepcionObj.Material
                'dtDetalleRecepcion = Recibos.PalletRecepcion.ObtenerInfoDetallePorMaterial(CLng(hfIdOrdenRecepcion.Value))
                Session("dtInfoRecepcion") = dtDetalleRecepcion
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al tratar de cargar información de orden de recepción. " & ex.Message)
        End Try
        Return dtDetalleRecepcion
    End Function

    Private Sub EnlazarYMostrarErrores(ByVal dtError As DataTable, Optional ByVal info As Boolean = False)
        If info Then
            With gvInfoWS
                .DataSource = dtError
                .DataBind()
                .Visible = True
            End With
        Else
            With gvErrores
                .DataSource = dtError
                .DataBind()
                .Visible = True
            End With
        End If

    End Sub

    Private Sub EnlazarYMostrarDocumentosGenerados(ByVal dtDocumento As DataTable)
        With gvDocumentosCargados
            .DataSource = dtDocumento
            .DataBind()
            .Visible = True
        End With
    End Sub

    Private Sub EnlazarYMostrarInfoOrdenCompra(ByVal dtInfoOC As DataTable)
        With gvDatos
            .DataSource = dtInfoOC
            .DataBind()
            If Me.tipoCaso <> 1 Then
                gvDatos.Columns(4).Visible = False
                gvDatos.Columns(5).Visible = False
            End If

            'If Me.tipoCaso = 2 Or Me.tipoCaso = 3 Then
            '    gvDatos.Columns(11).Visible = True
            'Else
            '    gvDatos.Columns(11).Visible = False
            'End If

        End With
        pnlContenedorOpciones.Visible = True
    End Sub

    ''' <summary>
    ''' Cargue inicial y consulta de orden de compra en SAP
    ''' </summary>
    ''' <remarks>123</remarks>
    Private Sub ConsultarInfoOrdenCompraEnSAP()
        Try
            Dim wsContEntrada As New SAPContabilizacionEntrada.WS_ENTRADAS_LG
            Dim infoOC As New SAPContabilizacionEntrada.ZmmLgEntradasCab
            Dim respuesta As SAPContabilizacionEntrada.OutputContabLg
            Dim dtError As DataTable = CrearEstructuraError()
            Dim dtInfoWS As DataTable = CrearEstructuraError()
            Dim infoWs As New InfoUrlWebService(wsContEntrada, True)

            With infoOC
                .entregaFactura = lblNotaEntrega.Text
                .textoCab = lblTextoCabecera.Text
                .pedidoDoccomp = lblNumeroOrden.Text 'Número de orden de compra - No orden errado 4500023170 - No orden correcto 4500023168
            End With
            Dim credencia As GeneradorCredencialesWebService = New GeneradorCredencialesWebService()
            wsContEntrada.Credentials = credencia.Credenciales
            respuesta = wsContEntrada.executeZmmLgContabEntradas("O", "101", infoOC, "X", Nothing, Nothing)
            If Not HayErroresEnRespuestaDeConsulta(respuesta, dtError, dtInfoWS) Then
                Dim dtInfoOC As DataTable = CrearEstructuraOrdenesSAP()
                Dim drInfoOC As DataRow
                Dim almacen As Integer
                For index As Integer = 0 To respuesta.rMateriales.Length - 1
                    drInfoOC = dtInfoOC.NewRow
                    With respuesta.rMateriales(index)
                        drInfoOC("posicionContable") = .posContable
                        drInfoOC("posicionDocumento") = .posDocumento
                        drInfoOC("material") = .material
                        drInfoOC("centro") = .centro
                        Integer.TryParse(.almacen, almacen)
                        If almacen <> 0 Then drInfoOC("almacen") = .almacen
                        drInfoOC("cantidad") = .cantidad
                        drInfoOC("unidadMedida") = .unidad
                        dtInfoOC.Rows.Add(drInfoOC)
                    End With

                Next
                If tipoCaso <> 1 Then _
                    DeterminarRepetidos(dtInfoOC)
                Session("dtInfoOcSAP") = dtInfoOC
                If dtInfoOC.Rows.Count > 0 Then
                    CargarDatosOrdenRecepcion()
                    ValidacionesCargueInicialDtSAP(dtInfoOC, dtError)
                    EnlazarYMostrarInfoOrdenCompra(dtInfoOC)
                    If dtError.Rows.Count Then
                        epPrincipal.showWarning("Uno o más registros obtenidos desde SAP presentan problemas. Por favor verifique.")
                        EnlazarYMostrarErrores(dtError)
                    End If
                End If
            ElseIf dtError.Rows.Count > 0 Then
                epPrincipal.showError("Imposible obtener la información de la orden de compra desde SAP. Ver listado de errores.")
                EnlazarYMostrarErrores(dtError)
            End If
            If dtInfoWS.Rows.Count > 0 Then
                epPrincipal.showWarning("Imposible obtener la información de la orden de compra desde SAP. Ver listado.")
                EnlazarYMostrarErrores(dtInfoWS, True)
            End If
            If Not gvInfo.Rows.Count > 0 Then
                btnCargar.Visible = False
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al tratar de consultar la información de la Orden de Compra desde SAP. " & ex.Message)
        End Try
    End Sub

    Private Sub DeterminarRepetidos(ByRef dt As DataTable)
        Dim filas() As DataRow
        For i As Integer = 0 To dt.Rows.Count - 1
            filas = dt.Select("centro=" & dt.Rows(i)("centro").ToString() & " AND material=" & dt.Rows(i)("material").ToString())
            If filas.Length > 1 Then
                For Each reg As DataRow In filas
                    reg("cargueManual") = True
                Next
            End If
        Next
    End Sub

    ''Private Sub CargarOCEntradaMercancia()
    ''    Dim cont As SAPContabilizacionEntrada.WS_ENTRADAS_LG
    ''    Dim infoOC As New SAPContabilizacionEntrada.ZmmLgEntradasCab
    ''    Dim resultado As SAPContabilizacionEntrada.OutputContabLg

    ''    With infoOC
    ''        .entregaFactura = "REMISION"
    ''        .textoCab = "PROVEEDOR" 'Proveedor                
    ''        '.noOrden = "4500023170" 'Sin información           
    ''        .pedidoDoccomp = "4500023170" 'Número de orden de compra        
    ''    End With

    ''    Dim detMat() As SAPContabilizacionEntrada.ZmmLgMateriales 'Depende de la selección en pantalla

    ''    detMat(0) = New SAPContabilizacionEntrada.ZmmLgMateriales
    ''    With detMat(0)
    ''        .posContable = 10
    ''        .material = "1111"
    ''        .cantidad = 10
    ''        .centro = "1002"
    ''        .almacen = "1003"
    ''    End With

    ''    Dim credencia As GeneradorCredencialesWebService = New GeneradorCredencialesWebService()
    ''    cont.Credentials = credencia.Credenciales
    ''    resultado = cont.executeZmmLgContabEntradas("O", "101", infoOC, Nothing, detMat, Nothing)
    ''    Dim hayError As Boolean = False
    ''    For index As Integer = 0 To resultado.oMensajes.Length - 1
    ''        If (resultado.oMensajes(index).type = "E" Or resultado.oMensajes(index).type = "A") Then
    ''            'Hubo un error y debo parar el proceso y notificarle al usuariotur
    ''            'Conveniente: guardar los errores en un datatable para mostrarlos al usuario

    ''            hayError = True
    ''        End If
    ''    Next
    ''    If Not hayError Then
    ''        'Imprimir documento y volver a consultar (depende de si se acabó la Orden de Recepción)
    ''        'Dim doc As SAPImpresionDocumentos.WS_PDF_LG
    ''    End If

    ''End Sub

    ''' <summary>
    ''' Función que permite crear un DataTable con la estructura requerida para almacenar los datos
    ''' obtenidos a través del WS Service de Contabilización de Entradas de Mercancía, cuando es lanzado
    ''' en modo consulta
    ''' </summary>
    ''' <returns>Retorna un objeto de tipo DataTable con la estructura requerida</returns>
    ''' <remarks></remarks>
    Private Function CrearEstructuraOrdenesSAP() As DataTable
        Dim dtAux As New DataTable
        With dtAux
            With .Columns
                .Add("posicionContable", GetType(Integer))
                .Add("posicionDocumento", GetType(Integer))
                .Add("material", GetType(Integer))
                .Add("centro", GetType(Integer))
                .Add("almacen", GetType(Integer))
                .Add("cantidadReal", GetType(Integer))
                .Add("cantidad", GetType(Integer))
                Dim columnaCantidadPendiente As New DataColumn("cantidadPendiente", GetType(Integer))
                columnaCantidadPendiente.DefaultValue = 0
                .Add(columnaCantidadPendiente)
                .Add("unidadMedida", GetType(String))
                Dim columnaHabilitado As New DataColumn("habilitado", GetType(Boolean))
                columnaHabilitado.DefaultValue = True
                .Add(columnaHabilitado)
                Dim columnaCargueManual As New DataColumn("cargueManual", GetType(Boolean))
                columnaCargueManual.DefaultValue = False
                .Add(columnaCargueManual)
            End With
        End With
        Return dtAux
    End Function

    ''' <summary>
    ''' Función que permite crear un DataTable con la estructura requerida para almacenar la información
    ''' de las diferentes posiciones seleccionadas por el usuario para realizar contabilizacion
    ''' </summary>
    ''' <returns>Retorna un objeto de tipo DataTable con la estructura requerida</returns>
    ''' <remarks></remarks>
    Private Function CrearEstructuraPosicionSeleccionada() As DataTable
        Dim dtAux As New DataTable
        With dtAux
            With .Columns
                .Add("posicionContable", GetType(Integer))
                .Add("material", GetType(Integer))
                .Add("idProducto", GetType(Integer))
                .Add("centro", GetType(Integer))
                .Add("almacen", GetType(Integer))
                .Add("cantidadContabilizar", GetType(Integer))
                .Add("cantidadSAP", GetType(Integer))
                .Add("unidadMedida", GetType(String))
            End With
        End With
        Return dtAux
    End Function

    ''' <summary>
    ''' Función que permite crear un DataTable con las estructura requerida para almacenar errores
    ''' encontrados en los diferentes procesos
    ''' </summary>
    ''' <returns>Retorna un objeto de tipo DataTable con la estructura requerida</returns>
    ''' <remarks></remarks>
    Private Function CrearEstructuraError() As DataTable
        Dim dtAux As New DataTable
        With dtAux.Columns
            .Add("indice", GetType(Integer))
            .Add("descripcion", GetType(String))
        End With
        Return dtAux
    End Function

    ''' <summary>
    ''' Función que permite crear un DataTable con las estructura requerida para adicionar no de documentos generados
    ''' </summary>
    ''' <returns>Retorna un objeto de tipo DataTable con la estructura requerida</returns>
    ''' <remarks></remarks>
    Private Function CrearEstructuraDocumentoGenerado() As DataTable
        Dim dtAux As New DataTable
        With dtAux.Columns
            .Add("indice", GetType(Integer))
            .Add("lote", GetType(String))
            .Add("noDocumento", GetType(String))
        End With
        Return dtAux
    End Function

    ''' <summary>
    ''' Procedimiento que permite registrar errores encontrados en los diferentes procesos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AdicionarError(ByVal dtError As DataTable, ByVal indice As Integer, ByVal descripcion As String)
        If dtError Is Nothing Then dtError = CrearEstructuraError()
        Dim drAux As DataRow = dtError.NewRow
        drAux("indice") = indice
        drAux("descripcion") = descripcion
        dtError.Rows.Add(drAux)
    End Sub

    ''' <summary>
    ''' Procedimiento que permite registrar no de documento generados
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AdicionarDocumento(ByVal dtDocumento As DataTable, ByVal indice As Integer, ByVal lote As String, ByVal noDocumento As String)
        If dtDocumento Is Nothing Then dtDocumento = CrearEstructuraDocumentoGenerado()
        Dim drAux As DataRow = dtDocumento.NewRow
        drAux("indice") = indice
        drAux("lote") = lote
        drAux("noDocumento") = noDocumento
        dtDocumento.Rows.Add(drAux)
    End Sub

    ''' <summary>
    ''' Función que permite validar la existencia de errores en una respuesta obtenida desde el Web Service de Contabilización de Entradas.
    ''' </summary>
    ''' <returns>Retorna un valor de tipo Booleano que indica si se encontraron errores en el proceso</returns>
    ''' <remarks></remarks>
    Private Function HayErroresEnRespuestaDeConsulta(ByVal respuesta As SAPContabilizacionEntrada.OutputContabLg, ByRef dtError As DataTable, Optional ByRef dtInfoWS As DataTable = Nothing) As Boolean
        Dim resultado As Boolean = False
        If respuesta IsNot Nothing Then
            Dim mensajes() As SAPContabilizacionEntrada.Bapiret2
            mensajes = respuesta.oMensajes
            If mensajes IsNot Nothing Then
                For index As Integer = 0 To mensajes.Length - 1
                    If mensajes(0) IsNot Nothing Then
                        If mensajes(0).type = "E" Or mensajes(0).type = "A" Then
                            resultado = True
                            AdicionarError(dtError, mensajes(0).number, mensajes(0).message)
                        ElseIf mensajes(0).type = "I" AndAlso Not dtInfoWS Is Nothing Then
                            resultado = True
                            AdicionarError(dtInfoWS, mensajes(0).number, mensajes(0).message)
                        End If
                    Else
                        resultado = True
                        AdicionarError(dtError, index + 1, "El Web Service no arrojó una respuesta válida. La estructura Mensaje tiene valor nulo en la posición " & (index + 1).ToString)
                    End If
                Next
            Else
                resultado = True
                AdicionarError(dtError, 1, "El Web Service no arrojó una respuesta válida. La estructura de Mensajes tiene valor nulo")
            End If
            If dtError.Rows.Count > 0 Then
                Dim materiales() As SAPContabilizacionEntrada.ZmmLgMateriales
                materiales = respuesta.rMateriales
                If materiales IsNot Nothing Then
                    If materiales.Length = 0 Then
                        resultado = True
                        AdicionarError(dtError, 1, "El Web Service no arrojó una respuesta válida. La estructura de Materiales vino vacía")
                    End If
                Else
                    resultado = True
                    AdicionarError(dtError, 1, "El Web Service no arrojó una respuesta válida. La estructura de Materiales tiene valor nulo")
                End If
            End If
        Else
            resultado = True
            AdicionarError(dtError, 1, "El Web Service no arrojó una respuesta válida. La estructura de Respuesta tiene valor nulo")
        End If
        Return resultado
    End Function

    ''' <summary>
    ''' Estructura de datos de posiciones seleccionadas en la interfaz.
    ''' </summary>
    ''' <param name="dt">dt a asociar estructura.</param>
    ''' <remarks></remarks>
    Protected Sub EstructuraPosicionesChequeadas(ByVal dt As DataTable)
        Try
            With dt
                With .Columns
                    .Add("posicion", GetType(Integer))
                    .Add("material", GetType(String))
                    .Add("idProducto", GetType(Integer))
                    .Add("cantidadIngresada", GetType(Integer))
                    .Add("cantidadSAP", GetType(Integer))
                    .Add("unidad", GetType(String))
                    .Add("centro", GetType(String))
                    .Add("almacen", GetType(String))
                End With
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub gvDatos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Try
                Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim habilitado As Boolean
                Dim cargueManual As Boolean
                Dim dtSerialesCargados As New DataTable
                Dim material As String = fila("material").ToString
                Dim dllAlmacenCargue As DropDownList = CType(e.Row.FindControl("ddlAlmacen"), DropDownList)
                Dim dllUnidadMedida As DropDownList = CType(e.Row.FindControl("ddlUnidadMedida"), DropDownList)
                Dim lblCantidadACargar As Label = CType(e.Row.FindControl("lblCantidadACargar"), Label)
                Dim txtCantidadACargar As TextBox = CType(e.Row.FindControl("txtCantidadACargar"), TextBox)
                Dim hfMaterialCentroCantidad As HiddenField = CType(e.Row.FindControl("hfMaterialCentroCantidad"), HiddenField)

                cargueManual = CType(fila("cargueManual").ToString(), Boolean)
                txtCantidadACargar.Visible = cargueManual
                lblCantidadACargar.Visible = IIf(cargueManual, False, True)

                'dtSerialesCargados = ObtenerListadoSeriales(fila("material").ToString(), idOrdenRecepcion, fila("centro").ToString(), Enumerados.EstadoBinario.Activo)                
                'TryCast(e.Row.FindControl("imgVerSerialesCargados"), ImageButton).Visible = CBool(dtSerialesCargados.Rows.Count)


                Dim chkAgregar As CheckBox = CType(e.Row.FindControl("chkAgregar"), CheckBox)
                CargarListadoAlmacen(dllAlmacenCargue, fila("centro").ToString())
                CargarUnidadMedida(dllUnidadMedida, material, fila("unidadMedida").ToString())
                hfMaterialCentroCantidad.Value = fila("centro").ToString() & "_" & dllAlmacenCargue.SelectedValue.ToString() & "_0_" & fila("material").ToString()
                habilitado = CType(fila("habilitado"), Boolean)
                chkAgregar.Enabled = habilitado
                If Not habilitado Then
                    chkAgregar.CssClass = String.Empty
                Else
                    Dim txt As TextBox = e.Row.FindControl("txtCantidad")
                    txt.Attributes.Add("onblur", "javascript:__doPostBack('" & txt.UniqueID & "','0')")
                End If

                If tipoCaso = 1 Then
                    txtCantidadACargar.Visible = False
                    lblCantidadACargar.Text = "0"
                End If

            Catch ex As Exception
                epPrincipal.showError("Error al realizar enlace de datos. " & ex.Message)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Carga el control de almacenes de un centro especifico.
    ''' </summary>
    ''' <param name="ddlAlmacen">Control dropdownlist para cargar los almacenes</param>
    ''' <param name="centro">Centro suministrado por SAP</param>
    ''' <remarks></remarks>
    Private Sub CargarListadoAlmacen(ByVal ddlAlmacen As DropDownList, ByVal centro As String)
        Try
            Dim dtRegion As DataTable = ObtenerListadoRegion()
            Dim dvAlmacen As DataView = dtRegion.DefaultView
            dvAlmacen.RowFilter = "centro='" & centro & "'"
            With ddlAlmacen
                .DataSource = dvAlmacen
                .DataTextField = "almacen"
                .DataValueField = "almacen"
                .DataBind()
                If dvAlmacen.Count > 1 Then .Items.Insert(0, New ListItem("Seleccione...", "0"))
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub CargarUnidadMedida(ByVal ddlUnidadMedida As DropDownList, ByVal material As String, ByVal unidadMedidaSAP As String)
        Try
            Dim materialObjBP As New Productos.Material(material)
            Dim dtUnidadMedida As New DataTable("UnidadMedida")
            dtUnidadMedida.Columns.Add("unidad", GetType(String))
            dtUnidadMedida.Columns.Add("codigo", GetType(String))
            Dim filaUnidadMedida As DataRow = dtUnidadMedida.NewRow()
            filaUnidadMedida("unidad") = materialObjBP.UnidadEmpaque
            filaUnidadMedida("codigo") = materialObjBP.CodigoEmpaque
            dtUnidadMedida.Rows.Add(filaUnidadMedida)
            If materialObjBP.CodigoEmpaque <> unidadMedidaSAP Then
                Dim filaUnidadMedidaSAP As DataRow = dtUnidadMedida.NewRow()
                filaUnidadMedidaSAP("codigo") = unidadMedidaSAP
                filaUnidadMedidaSAP("unidad") = unidadMedidaSAP
                dtUnidadMedida.Rows.Add(filaUnidadMedidaSAP)
            End If

            With ddlUnidadMedida
                .DataSource = dtUnidadMedida
                .DataTextField = "unidad"
                .DataValueField = "codigo"
                .DataBind()
                If dtUnidadMedida.Rows.Count > 1 Then .Items.Insert(0, New ListItem("Seleccione...", "0"))
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Realiaza las validaciones correspondientes para realizar la presentacion de las posiciones en pantalla.
    ''' </summary>
    ''' <param name="dtSAP">Datos de consulta de SAP</param>
    ''' <remarks></remarks>
    Private Sub ValidacionesCargueInicialDtSAP(ByVal dtSAP As DataTable, ByRef dtError As DataTable)
        If Session("dtInfoRecepcion") IsNot Nothing Then
            Try
                dtSAP.Columns.Add("referencia", GetType(String))
                Dim dtListadoRegion As DataTable = ObtenerListadoRegion()
                Dim dtOrdenRecepcion As DataTable = ObtenerInformacionRecepcion()
                Dim materialbp As Productos.Material
                Dim pkMaterial() As DataColumn = {dtOrdenRecepcion.Columns("material"), dtOrdenRecepcion.Columns("centro")}
                Dim arrFilasEliminar As New ArrayList
                dtOrdenRecepcion.PrimaryKey = pkMaterial
                For Each drSAP As DataRow In dtSAP.Rows
                    If dtListadoRegion.Rows.Find(drSAP("centro").ToString) IsNot Nothing Then
                        materialbp = New Productos.Material(drSAP("material").ToString)
                        If materialbp.Referencia.Trim.Length > 0 Then
                            drSAP("referencia") = materialbp.Referencia.ToUpper
                            Dim filaCorrecta As DataRow
                            Dim filtroClave() As Object = {materialbp.Material, drSAP("centro")}
                            filaCorrecta = dtOrdenRecepcion.Rows.Find(filtroClave)
                            If Not filaCorrecta Is Nothing Then
                                drSAP("cantidadPendiente") = filaCorrecta("cantidadPendienteCargar")
                                drSAP("cantidadReal") = filaCorrecta("cantidadPendienteCargar")
                            ElseIf filaCorrecta Is Nothing Then
                                drSAP("habilitado") = False

                                AdicionarError(dtError, drSAP("posicionContable"), "El material " & drSAP("material").ToString & _
                                           " de la posición " & drSAP("posicionContable") & " no existe en la orden de recepción.")
                            End If


                            If Me.tipoCaso <> 1 And Not filaCorrecta Is Nothing Then
                                Dim cantidadReal As Integer
                                If drSAP("unidadMedida").ToString <> materialbp.CodigoEmpaque Then
                                    cantidadReal = CInt(filaCorrecta("cantidadPendienteCargar")) * materialbp.CantidadEmpaque
                                Else
                                    cantidadReal = CInt(filaCorrecta("cantidadPendienteCargar"))
                                End If
                                Integer.TryParse(cantidadReal.ToString(), drSAP("cantidadPendiente"))
                                Integer.TryParse(cantidadReal.ToString(), drSAP("cantidadReal"))
                                'If drSAP("centro").ToString = materialbp

                                If cantidadReal > drSAP("cantidad") Then
                                    drSAP("habilitado") = False
                                    AdicionarError(dtError, drSAP("posicionContable"), "La cantidad de la posición " & drSAP("posicionContable") & " es mayor de la permitida para cargar.")
                                End If

                                Dim dtSerial As DataTable = ObtenerListadoSeriales(drSAP("material").ToString, idOrdenRecepcion, drSAP("centro").ToString())
                                If Not (dtSerial IsNot Nothing AndAlso dtSerial.Rows.Count > 0) Then
                                    drSAP("habilitado") = False
                                    AdicionarError(dtError, drSAP("posicionContable"), "El material " & drSAP("material").ToString & _
                                           " de la posición " & drSAP("posicionContable") & " no tiene seriales para cargar.")
                                End If
                            ElseIf Me.tipoCaso = 1 And Not filaCorrecta Is Nothing Then
                                Dim cantidadParaCargar As Integer
                                Integer.TryParse(filaCorrecta("cantidadPendienteCargar").ToString(), cantidadParaCargar)
                                If drSAP("habilitado") Then _
                                    drSAP("habilitado") = CBool(cantidadParaCargar)
                            End If
                        Else
                            drSAP("habilitado") = False
                            AdicionarError(dtError, drSAP("posicionContable"), "El material " & drSAP("material").ToString & _
                                           " de la posición " & drSAP("posicionContable") & " no existe en la Base de Datos.")
                        End If
                    Else
                        arrFilasEliminar.Add(drSAP)
                    End If
                Next

                For Each drSAP As DataRow In arrFilasEliminar
                    drSAP.Delete()
                Next
            Catch ex As Exception
                Throw New Exception("Error al tratar de validar datos de la Orden obtenidos desde SAP. " & ex.Message)
            End Try
        Else
            Throw New Exception("Error al tratar de validar datos de la Orden obtenidos desde SAP. Imposible recuperar el detalle de la orden de recepción desde la memoria")
        End If
    End Sub

    Protected Sub btnCargar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCargar.Click
        Dim dtError As DataTable = CrearEstructuraError()
        Dim dtSeleccion As DataTable = ObtenerPosicionesSeleccionadas()
        Try
            gvErrores.Visible = False
            If SonPosicionesSeleccionadasValidas(dtSeleccion, dtError) Then
                If tipoCaso = 2 Then
                    CargarProductoEnSAPTarjetasPrepago(dtSeleccion, dtError)
                Else
                    CargarProductoEnSAP(dtSeleccion, dtError)
                End If

            Else
                EnlazarYMostrarErrores(dtError)
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al tratar de cargar producto en SAP. " & ex.Message)
        End Try
    End Sub

    Private Function CargarProductoEnSAP(ByVal dtSeleccion As DataTable, ByVal dtError As DataTable)
        Try
            Dim retorno As Boolean = True
            Dim wsContEntrada As New SAPContabilizacionEntrada.WS_ENTRADAS_LG
            Dim infoOC As New SAPContabilizacionEntrada.ZmmLgEntradasCab
            Dim respuesta As SAPContabilizacionEntrada.OutputContabLg
            Dim infoWs As New InfoUrlWebService(wsContEntrada, True)

            With infoOC
                '.entregaFactura = lblNotaEntrega.Text
                '.entregaFactura = "remision"
                .textoCab = lblTextoCabecera.Text
                .pedidoDoccomp = lblNumeroOrden.Text
                .nota = lblNotaEntrega.Text
            End With

            Dim dt As DataTable = CType(Session("dtInfoOcSAP"), DataTable)
            Dim detMat(dtSeleccion.Rows.Count - 1) As SAPContabilizacionEntrada.ZmmLgMateriales 'Depende de la selección en pantalla
            Dim detSerMat() As SAPContabilizacionEntrada.ZmmLgMateriales  'Cargos todos los lotes
            ''prueba  Dim detSerial() As ZmmLgSerialnumber
            Dim detSerial() As SAPContabilizacionEntrada.ZmmLgSerialnumber

            Dim infoMaterial As Productos.Material
            For i As Integer = 0 To dtSeleccion.Rows.Count - 1
                detMat(i) = New SAPContabilizacionEntrada.ZmmLgMateriales
                With detMat(i)
                    Integer.TryParse(dtSeleccion.Rows(i)("posicionContable").ToString, .posContable)
                    Integer.TryParse(dt.Compute("SUM(posicionDocumento)", "posicionContable=" & .posContable.ToString).ToString, .posDocumento)
                    .unidad = dtSeleccion.Rows(i)("unidadMedida").ToString
                    .centro = dtSeleccion.Rows(i)("centro").ToString()
                    .almacen = dtSeleccion.Rows(i)("almacen").ToString()
                    .material = dtSeleccion.Rows(i)("material").ToString()
                    Double.TryParse(dtSeleccion.Rows(i)("cantidadContabilizar").ToString, .cantidad)
                    infoMaterial = New Productos.Material(.material)
                    If infoMaterial.EsSerializado And Me.tipoCaso = 2 Then
                        Dim dtSerial As DataTable = ObtenerListadoSeriales(.material, idOrdenRecepcion)
                        '.lote = dtSerial.Rows(0)("lote").ToString()
                        ''.fechaVenc = dtSerial.Rows(0)("fechaVencimiento").ToString()
                        '.fechaVenc = "20121031"
                        '.cantidad = dtSerial.Rows.Count
                        CargarSerialesTarjetasPrepago(dtSerial, detMat(i), detSerMat, infoMaterial)
                    ElseIf infoMaterial.EsSerializado And Me.tipoCaso = 3 Then
                        .lote = Now.ToString("ddMMyyyy")
                        Dim dtSerial As DataTable = ObtenerListadoSeriales(.material, idOrdenRecepcion, .centro)
                        Dim cantidadSeriales As Integer = dtSerial.Rows.Count
                        'Dim dtCopia As DataTable = dtSerial.Copy
                        For index As Integer = dtSerial.Rows.Count - 1 To .cantidad Step -1
                            dtSerial.Rows(index).Delete()
                        Next

                        CargarSerialesToken(dtSerial, .posContable, detSerial)
                    ElseIf infoMaterial.EsSerializado And Me.tipoCaso = 4 Then
                        Dim dtSerial As DataTable = ObtenerListadoSeriales(.material, idOrdenRecepcion)
                        CargarSerialesBonos(dtSerial, .posContable, detSerial)
                    End If

                End With
            Next

            Dim credencia As GeneradorCredencialesWebService = New GeneradorCredencialesWebService()
            wsContEntrada.Credentials = credencia.Credenciales
            wsContEntrada.Timeout = 1200000

            If tipoCaso = 1 Or tipoCaso = 3 Or tipoCaso = 4 Then
                respuesta = wsContEntrada.executeZmmLgContabEntradas("O", "101", infoOC, Nothing, detMat, detSerial)
            ElseIf tipoCaso = 2 Then
                For Each lote As SAPContabilizacionEntrada.ZmmLgMateriales In detSerMat
                    Dim loteCargar(0) As SAPContabilizacionEntrada.ZmmLgMateriales
                    loteCargar(0) = lote
                    respuesta = wsContEntrada.executeZmmLgContabEntradas("O", "101", infoOC, Nothing, loteCargar, detSerial)
                Next
            End If

            Dim mensajes() As SAPContabilizacionEntrada.Bapiret2
            mensajes = respuesta.oMensajes
            If mensajes IsNot Nothing Then
                For index As Integer = 0 To mensajes.Length - 1
                    If mensajes(0) IsNot Nothing Then
                        If mensajes(0).type = "E" Or mensajes(0).type = "A" Then
                            retorno = False
                            AdicionarError(dtError, mensajes(0).number, mensajes(0).message)
                        End If
                    Else
                        retorno = False
                        AdicionarError(dtError, index + 1, "El Web Service no arrojó una respuesta válida. La estructura Mensaje tiene valor nulo en la posición " & (index + 1).ToString)
                    End If
                Next
            Else
                retorno = False
                AdicionarError(dtError, 1, "El Web Service no arrojó una respuesta válida. La estructura de Mensajes tiene valor nulo")
            End If

            If retorno Then

                Dim doc As New GeneradorDocumentosSAP
                doc.idDocumento = respuesta.oMensajes(0).messageV1
                doc.AnioEjercicio = Now.Year
                doc.ModoTratamiento = GeneradorDocumentosSAP.modoTratamientoDoc.primerProceso
                doc.TipoDocumento = GeneradorDocumentosSAP.tipoDoc.Material
                doc.NombreArchivo = "CargueRecepcion_"
                Dim ruta As String = Server.MapPath("archivos_SAP")
                Dim resul As ResultadoProceso = doc.GenerarDocumento(ruta)
                If resul.Valor = 0 Then
                    'Imprimir documento y volver a consultar (depende de si se acabó la Orden de Recepción)
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "newWindow", "window.open ('archivos_SAP/" & Path.GetFileName(doc.RutaDocumento) & "','CargueRecepcion', 'status=1, toolbar=0, location=0,menubar=1,directories=0,resizable=1,scrollbars=1'); ", True)
                End If

                Dim cargueSapObj As New CargueSAP(CLng(hfIdOrdenRecepcion.Value))
                If tipoCaso = 1 Then 'Carga no serializada
                    cargueSapObj.RegistrarCargueSAP(respuesta.rMateriales(), CInt(Session("usxp001")), doc.idDocumento.ToString())
                ElseIf tipoCaso = 3 Then 'Actualizacion de seriales token cargados
                    cargueSapObj.RegistrarCargueSerializadoSAPToken(respuesta.rMateriales(), detSerial, CInt(Session("usxp001")), doc.idDocumento.ToString())
                ElseIf tipoCaso = 4 Then
                    cargueSapObj.RegistrarCargueSerializadoSAPBonos(detSerial, CInt(Session("usxp001")), doc.idDocumento.ToString())
                Else 'Carga serializada
                    cargueSapObj.RegistrarCargueSerializadoSAP(respuesta.rMateriales(), CInt(Session("usxp001")), doc.idDocumento.ToString(), tipoCaso)
                End If


                CargarDatosOrdenCompra()
                epPrincipal.showSuccess("Carga realizada correctamente con el No. " & doc.idDocumento.ToString())
            Else
                EnlazarYMostrarErrores(dtError)
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al realizar la carga en SAP. " & ex.Message)
        End Try
    End Function

    Private Function CargarProductoEnSAPTarjetasPrepago(ByVal dtSeleccion As DataTable, ByVal dtError As DataTable)
        Dim dtDocumentoCargado As DataTable = CrearEstructuraDocumentoGenerado()
        Try
            Dim retorno As Boolean = True
            Dim wsContEntrada As New SAPContabilizacionEntrada.WS_ENTRADAS_LG
            Dim infoOC As New SAPContabilizacionEntrada.ZmmLgEntradasCab
            Dim respuesta As SAPContabilizacionEntrada.OutputContabLg
            Dim infoWs As New InfoUrlWebService(wsContEntrada, True)

            With infoOC
                '.entregaFactura = lblNotaEntrega.Text
                '.entregaFactura = "remision"
                .textoCab = lblTextoCabecera.Text
                .pedidoDoccomp = lblNumeroOrden.Text
                .nota = lblNotaEntrega.Text
            End With

            Dim dt As DataTable = CType(Session("dtInfoOcSAP"), DataTable)
            Dim detMat(dtSeleccion.Rows.Count - 1) As SAPContabilizacionEntrada.ZmmLgMateriales 'Depende de la selección en pantalla
            Dim detSerMat() As SAPContabilizacionEntrada.ZmmLgMateriales  'Cargos todos los lotes
            Dim detSerial() As SAPContabilizacionEntrada.ZmmLgSerialnumber

            Dim infoMaterial As Productos.Material
            For i As Integer = 0 To dtSeleccion.Rows.Count - 1
                detMat(i) = New SAPContabilizacionEntrada.ZmmLgMateriales
                With detMat(i)
                    Integer.TryParse(dtSeleccion.Rows(i)("posicionContable").ToString, .posContable)
                    Integer.TryParse(dt.Compute("SUM(posicionDocumento)", "posicionContable=" & .posContable.ToString).ToString, .posDocumento)
                    .unidad = dtSeleccion.Rows(i)("unidadMedida").ToString
                    .centro = dtSeleccion.Rows(i)("centro").ToString()
                    .almacen = dtSeleccion.Rows(i)("almacen").ToString()
                    .material = dtSeleccion.Rows(i)("material").ToString()
                    Double.TryParse(dtSeleccion.Rows(i)("cantidadContabilizar").ToString, .cantidad)
                    infoMaterial = New Productos.Material(.material)
                    If infoMaterial.EsSerializado Then
                        Dim dtSerial As DataTable = ObtenerListadoSeriales(.material, idOrdenRecepcion, .centro)
                        '.lote = dtSerial.Rows(0)("lote").ToString()
                        ''.fechaVenc = dtSerial.Rows(0)("fechaVencimiento").ToString()
                        '.fechaVenc = "20121031"
                        '.cantidad = dtSerial.Rows.Count
                        CargarSerialesTarjetasPrepago(dtSerial, detMat(i), detSerMat, infoMaterial)
                    End If

                End With
            Next

            Dim credencia As GeneradorCredencialesWebService = New GeneradorCredencialesWebService()
            wsContEntrada.Credentials = credencia.Credenciales
            wsContEntrada.Timeout = 1200000

            Dim indice As Integer = 0
            Dim mensajes() As SAPContabilizacionEntrada.Bapiret2
            Dim doc As GeneradorDocumentosSAP
            For Each lote As SAPContabilizacionEntrada.ZmmLgMateriales In detSerMat
                indice += 1
                Dim loteCargar(0) As SAPContabilizacionEntrada.ZmmLgMateriales
                loteCargar(0) = lote
                respuesta = wsContEntrada.executeZmmLgContabEntradas("O", "101", infoOC, Nothing, loteCargar, detSerial)


                mensajes = respuesta.oMensajes
                If mensajes IsNot Nothing Then
                    For index As Integer = 0 To mensajes.Length - 1
                        If mensajes(0) IsNot Nothing Then
                            If mensajes(0).type = "E" Or mensajes(0).type = "A" Then
                                retorno = False
                                AdicionarError(dtError, mensajes(0).number, mensajes(0).message)
                            End If
                        Else
                            retorno = False
                            AdicionarError(dtError, index + 1, "El Web Service no arrojó una respuesta válida. La estructura Mensaje tiene valor nulo en la posición " & (index + 1).ToString)
                        End If
                    Next
                Else
                    retorno = False
                    AdicionarError(dtError, 1, "El Web Service no arrojó una respuesta válida. La estructura de Mensajes tiene valor nulo")
                End If

                If retorno Then
                    doc = New GeneradorDocumentosSAP
                    doc.idDocumento = respuesta.oMensajes(0).messageV1

                    Dim cargueSapObj As New CargueSAP(CLng(hfIdOrdenRecepcion.Value))

                    cargueSapObj.RegistrarCargueSerializadoSAP(respuesta.rMateriales(), CInt(Session("usxp001")), doc.idDocumento.ToString(), tipoCaso)

                    AdicionarDocumento(dtDocumentoCargado, indice, lote.lote, "Carga realizada correctamente con el No. " & doc.idDocumento.ToString())

                End If
            Next

            doc.AnioEjercicio = Now.Year
            doc.ModoTratamiento = GeneradorDocumentosSAP.modoTratamientoDoc.primerProceso
            doc.TipoDocumento = GeneradorDocumentosSAP.tipoDoc.Material
            doc.NombreArchivo = "CargueRecepcion_"
            Dim ruta As String = Server.MapPath("archivos_SAP")
            Dim resul As ResultadoProceso = doc.GenerarDocumento(ruta)
            If resul.Valor = 0 Then
                'Imprimir documento y volver a consultar (depende de si se acabó la Orden de Recepción)
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "newWindow", "window.open ('archivos_SAP/" & Path.GetFileName(doc.RutaDocumento) & "','CargueRecepcion', 'status=1, toolbar=0, location=0,menubar=1,directories=0,resizable=1,scrollbars=1'); ", True)
            End If



            If dtError.Rows.Count > 0 Then
                EnlazarYMostrarErrores(dtError)
            End If


        Catch ex As Exception
            epPrincipal.showError("Error al realizar la carga en SAP. " & ex.Message)
        Finally
            CargarDatosOrdenCompra()
            If dtDocumentoCargado.Rows.Count > 0 Then
                epPrincipal.clear()
                epPrincipal.showSuccess("Cargue realizado.")
                EnlazarYMostrarDocumentosGenerados(dtDocumentoCargado)
            End If
        End Try
    End Function


    Private Function ObtenerListadoSeriales(ByVal material As String, ByVal idOrdenRecepcion As Integer, Optional ByVal centro As String = "", Optional ByVal cargado As Enumerados.EstadoBinario = Enumerados.EstadoBinario.Inactivo) As DataTable
        Try
            Dim dtSeriales As DataTable
            If tipoCaso = 2 Then '2 -> Carga serializada por materiales (Tarjetas Prepagos --Tabla InfoTarjetaPrepago)
                Dim filtro As New Estructuras.FiltroInfoTarjetaPrepago
                filtro.Material = material
                filtro.IdOrdenRecepcion = idOrdenRecepcion
                filtro.Centro = centro
                filtro.Cargado = cargado
                dtSeriales = OMS.InfoTarjetaPrepago.ObtenerListado(filtro)
                Dim dv As New DataView(dtSeriales)
                dv.Sort = "fechaRegistro"
                dtSeriales = dv.ToTable()
            ElseIf tipoCaso = 3 Then '3 -> Cargar serializada por dSeriales (Token --Tabla productos_serial)
                Dim filtro As New Estructuras.FiltroInfoCargueSAPToken
                filtro.Material = material
                filtro.Centro = centro
                filtro.IdOrdenRecepcion = idOrdenRecepcion
                filtro.Cargado = cargado
                dtSeriales = Comunes.ProductosSerial.ObtenerListado(filtro)
                Dim dv As New DataView(dtSeriales)
                dv.Sort = "etiquetado"
                dtSeriales = dv.ToTable()
            ElseIf tipoCaso = 4 Then '4-> Carga serializada por materiales (Bonos --Tabla InfoCargueProductoSAP)
                Dim filtro As New Estructuras.FiltroInfoCargueProductoSAP
                filtro.Material = material
                filtro.IdOrdenRecepcion = idOrdenRecepcion
                filtro.Cargado = cargado
                dtSeriales = Comunes.InfoCargueProductoSAP.ObtenerListado(filtro)
            End If
            Return dtSeriales
        Catch ex As Exception
            epPrincipal.showError("Error al obtener el listado de seriales." & ex.Message)
        End Try
    End Function

    Private Sub CargarSerialesToken(ByVal dtSerial As DataTable, ByVal posContable As Integer, ByRef detSerial() As SAPContabilizacionEntrada.ZmmLgSerialnumber)
        Try
            If dtSerial IsNot Nothing AndAlso dtSerial.Rows.Count > 0 Then
                Dim indiceMasAlto As Integer = 0
                If detSerial IsNot Nothing Then indiceMasAlto = detSerial.Length

                ReDim Preserve detSerial(indiceMasAlto + (dtSerial.Rows.Count - 1))
                For i As Integer = 0 To dtSerial.Rows.Count - 1
                    detSerial(indiceMasAlto + i) = New SAPContabilizacionEntrada.ZmmLgSerialnumber
                    With detSerial(indiceMasAlto + i)
                        .material = dtSerial.Rows(i)("material").ToString
                        .noSerie = dtSerial.Rows(i)("serial").ToString.Trim()
                        .posContable = posContable
                    End With
                Next
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al cargar los seriales de la recepción. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarSerialesTarjetasPrepago(ByVal dtSerial As DataTable, ByVal materialActual As SAPContabilizacionEntrada.ZmmLgMateriales, ByRef detSerMateriales() As SAPContabilizacionEntrada.ZmmLgMateriales, ByVal materialBP As Productos.Material)
        Try
            If dtSerial IsNot Nothing AndAlso dtSerial.Rows.Count > 0 Then
                Dim indice As Integer = 0
                Dim fechaVencimiento As Date
                If detSerMateriales IsNot Nothing Then indice = detSerMateriales.GetUpperBound(0) + 1

                ReDim Preserve detSerMateriales(indice + (dtSerial.Rows.Count - 1))
                For i As Integer = 0 To dtSerial.Rows.Count - 1
                    detSerMateriales(indice + i) = New SAPContabilizacionEntrada.ZmmLgMateriales
                    With detSerMateriales(indice + i)
                        .posContable = materialActual.posContable
                        .posDocumento = materialActual.posDocumento
                        .unidad = materialActual.unidad
                        .centro = materialActual.centro
                        .almacen = materialActual.almacen
                        .material = materialActual.material
                        If .unidad = "UND" Then
                            .cantidad = materialBP.CantidadEmpaque
                        ElseIf .unidad = "PAC" Then
                            .cantidad = 1
                        End If
                        Date.TryParse(dtSerial.Rows(i)("fechaVencimiento"), fechaVencimiento)
                        .fechaVenc = fechaVencimiento.ToString("yyyyMMdd")
                        '.fechaVenc = DirectCast(dtSerial.Rows(i)("fechaVencimiento"), System.DateTime).ToString("yyyyMMdd")
                        .lote = dtSerial.Rows(i)("serial").ToString
                    End With
                Next
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al cargar los seriales de la recepción. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarSerialesBonos(ByVal dtSerial As DataTable, ByVal posContable As Integer, ByRef detSerial() As SAPContabilizacionEntrada.ZmmLgSerialnumber)
        Try
            If dtSerial IsNot Nothing AndAlso dtSerial.Rows.Count > 0 Then
                Dim indiceMasAlto As Integer = 0
                If detSerial IsNot Nothing Then indiceMasAlto = detSerial.GetUpperBound(0)

                ReDim Preserve detSerial(indiceMasAlto + (dtSerial.Rows.Count - 1))
                For i As Integer = 0 To dtSerial.Rows.Count - 1
                    detSerial(indiceMasAlto + i) = New SAPContabilizacionEntrada.ZmmLgSerialnumber
                    With detSerial(indiceMasAlto + i)
                        .material = dtSerial.Rows(i)("material").ToString
                        .noSerie = dtSerial.Rows(i)("serial").ToString
                        .posContable = posContable
                    End With
                Next
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al cargar los seriales de la recepción. " & ex.Message)
        End Try
        'Try
        '    If dtSerial IsNot Nothing AndAlso dtSerial.Rows.Count > 0 Then
        '        Dim indice As Integer = 0
        '        If detSerMateriales IsNot Nothing Then indice = detSerMateriales.GetUpperBound(0) + 1

        '        ReDim Preserve detSerMateriales(indice + (dtSerial.Rows.Count - 1))
        '        For i As Integer = 0 To dtSerial.Rows.Count - 1
        '            detSerMateriales(indice + i) = New SAPContabilizacionEntrada.ZmmLgMateriales
        '            With detSerMateriales(indice + i)
        '                .posContable = materialActual.posContable
        '                .posDocumento = materialActual.posDocumento
        '                .unidad = materialActual.unidad
        '                .centro = materialActual.centro
        '                .almacen = materialActual.almacen
        '                .material = materialActual.material
        '                .cantidad = materialActual.cantidad
        '                .lote = Now.ToString("ddMMyyyy")
        '            End With
        '        Next
        '    End If
        'Catch ex As Exception
        '    epPrincipal.showError("Error al cargar los seriales de la recepción. " & ex.Message)
        'End Try
    End Sub

    Private Function SonPosicionesSeleccionadasValidas(ByVal dtSeleccion As DataTable, ByRef dtError As DataTable) As Boolean
        Dim resultado As Boolean = True
        If dtError Is Nothing Then dtError = CrearEstructuraError()
        ''Validar cantidades
        Dim cantidadCargar As Integer
        Dim cantidadDisponible As Integer
        Dim arrIdProductoEvaluado As New ArrayList
        Dim idProducto As Integer
        Dim material As Integer
        Dim dtInfoRecepcion As DataTable = ObtenerInformacionRecepcion()
        For Each drSeleccion As DataRow In dtSeleccion.Rows
            Integer.TryParse(drSeleccion("cantidadContabilizar").ToString, cantidadCargar)
            Integer.TryParse(drSeleccion("cantidadSAP").ToString, cantidadDisponible)
            If cantidadCargar <= cantidadDisponible Then
                'idProducto = CInt(drSeleccion("idProducto"))
                'material = CInt(drSeleccion("material")).ToString
                'If Not arrIdProductoEvaluado.Contains(material) Then
                '    'cantidadCargar = dtSeleccion.Compute("SUM(cantidadContabilizar)", "idProducto='" & idProducto.ToString & "'")
                '    cantidadCargar = dtSeleccion.Compute("SUM(cantidadContabilizar)", "material='" & material & "'")
                '    'cantidadDisponible = dtInfoRecepcion.Compute("SUM(cantidadRecibida)", "idProducto='" & idProducto.ToString & "'")
                '    cantidadDisponible = dtInfoRecepcion.Compute("SUM(cantidadRecibida)", "material='" & material & "'")
                '    If cantidadCargar > cantidadDisponible Then
                '        resultado = False
                '        'AdicionarError(dtError, drSeleccion("posicionContable"), "La cantidad proporcionada del Producto Padre, " & _
                '        '               "asociado al material de la posición " & drSeleccion("posicionContable").ToString & _
                '        '               ", es mayor que la cantidad de ese Producto Padre en la Orden de Recepción.")
                '        AdicionarError(dtError, drSeleccion("posicionContable"), "La cantidad " & _
                '                       "asociado al material de la posición " & drSeleccion("posicionContable").ToString & _
                '                       ", es mayor que la cantidad de ese material en la Orden de Recepción.")
                '    End If
                '    'arrIdProductoEvaluado.Add(idProducto)
                '    arrIdProductoEvaluado.Add(material)
                'End If
            Else
                resultado = False
                'AdicionarError(dtError, drSeleccion("posicionContable"), "La cantidad proporcionada en la posición " & _
                '               drSeleccion("posicionContable").ToString & " es mayor que la cantidad disponible: " & cantidadDisponible.ToString)
                AdicionarError(dtError, drSeleccion("posicionContable"), "La cantidad en la posición " & _
                               drSeleccion("posicionContable").ToString & " es mayor que la cantidad disponible: " & cantidadDisponible.ToString)
            End If
        Next
        Return resultado
    End Function

    Protected Function ValidacionPosicionesSeleccionadas(ByVal dtPosicionesSeleccionadas As DataTable) As Boolean
        Try
            Dim retorno As Boolean = True

            Dim dtErrores As New DataTable
            Dim filaNuevaError As DataRow
            Dim dtInfoRecepcion As New DataTable
            ''EstructuraErroresSAP(dtErrores)
            dtInfoRecepcion = CType(Session("dtInfoRecepcion"), DataTable)
            If dtPosicionesSeleccionadas.Rows.Count > 0 Then
                'Validaciones de cantidades de productos con cantidades de productos en cargue
                Dim cantidadProductoIngresada As Integer
                Dim cantidadProductoRecepcion As Integer
                Dim idProducto As Integer
                For Each fila As DataRow In dtInfoRecepcion.Rows
                    Integer.TryParse(fila("idProducto").ToString, idProducto)
                    Integer.TryParse(dtPosicionesSeleccionadas.Compute("SUM(cantidadIngresada)", "idProducto=" & idProducto.ToString()).ToString, cantidadProductoIngresada)
                    Integer.TryParse(dtInfoRecepcion.Compute("SUM(cantidadRecibida)", "idProducto=" & idProducto.ToString()), cantidadProductoRecepcion)
                    If cantidadProductoIngresada > cantidadProductoRecepcion Then
                        filaNuevaError = dtErrores.NewRow()
                        filaNuevaError("indice") = "Producto: " & fila("nombreProducto").ToString()
                        filaNuevaError("error") = "La sumatoria de cargue para este producto supera  la cantidad permitida."
                        dtErrores.Rows.Add(filaNuevaError)
                    End If
                Next
                Dim cantidadIngresada As Integer
                Dim cantidadSAP As Integer
                For Each fila As DataRow In dtPosicionesSeleccionadas.Rows
                    Integer.TryParse(fila("cantidadIngresada").ToString, cantidadIngresada)
                    Integer.TryParse(fila("cantidadSAP").ToString, cantidadSAP)
                    If cantidadIngresada > cantidadSAP Then
                        filaNuevaError = dtErrores.NewRow
                        filaNuevaError("indice") = "Posición SAP: " & fila("posicion").ToString
                        filaNuevaError("error") = "La cantidad ingresada es mayor a la cantidad permitida"
                        dtErrores.Rows.Add(filaNuevaError)
                    End If

                Next

                If dtErrores.Rows.Count > 0 Then
                    gvErrores.DataSource = dtErrores
                    gvErrores.DataBind()
                    epPrincipal.showError("Se presentaron errores al realizar el cargue.")
                    gvErrores.Visible = True
                    retorno = False
                Else
                    gvErrores.Visible = False
                End If

            Else
                retorno = False
            End If
            Return retorno
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Function ObtenerPosicionesSeleccionadas() As DataTable
        Dim dtSeleccion As DataTable = CrearEstructuraPosicionSeleccionada()
        Dim drSeleccion As DataRow
        Dim chkAux As CheckBox
        Dim subAux As Productos.Material
        For Each gvRow As GridViewRow In gvDatos.Rows
            chkAux = CType(gvRow.FindControl("chkAgregar"), CheckBox)
            If chkAux IsNot Nothing AndAlso chkAux.Checked Then
                drSeleccion = dtSeleccion.NewRow
                drSeleccion("posicionContable") = gvRow.Cells(1).Text
                drSeleccion("material") = gvRow.Cells(2).Text
                subAux = New Productos.Material(gvRow.Cells(2).Text)
                drSeleccion("idProducto") = subAux.IdProductoPadre
                drSeleccion("centro") = gvRow.Cells(9).Text
                drSeleccion("almacen") = CType(gvRow.FindControl("ddlAlmacen"), DropDownList).SelectedValue
                If Me.tipoCaso = 1 Then
                    drSeleccion("cantidadContabilizar") = CType(gvRow.FindControl("txtCantidad"), TextBox).Text
                ElseIf Me.tipoCaso = 3 Or Me.tipoCaso = 2 Then
                    Dim controlCantidadToken As Control = gvRow.FindControl("txtCantidadACargar")
                    If Not controlCantidadToken Is Nothing AndAlso controlCantidadToken.Visible Then
                        drSeleccion("cantidadContabilizar") = CType(controlCantidadToken, TextBox).Text
                    Else
                        drSeleccion("cantidadContabilizar") = CType(gvRow.FindControl("lblCantidadACargar"), Label).Text
                    End If
                End If
                drSeleccion("cantidadSAP") = gvRow.Cells(7).Text
                drSeleccion("unidadMedida") = gvRow.Cells(8).Text
                dtSeleccion.Rows.Add(drSeleccion)
            End If
        Next
        Return dtSeleccion
    End Function

    ''' <summary>
    ''' Estructura información retornada por SAP
    ''' </summary>
    ''' <remarks></remarks>
    Structure materialSAP
        Public idProducto As Integer
        Public posicion As Integer
        Public material As String
        Public cantidad As Integer
        Public centro As String
        Public almacen As String
    End Structure

    Protected Sub ActualizarCantidad(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim gvr As GridViewRow
            Dim txt As TextBox
            Dim ddl As DropDownList
            If sender.GetType().Name = "TextBox" Then
                txt = CType(sender, TextBox)
                gvr = CType(txt.NamingContainer, GridViewRow)
                ddl = CType(gvr.FindControl("ddlUnidadMedida"), DropDownList)
            ElseIf sender.GetType().Name = "DropDownList" Then
                ddl = CType(sender, DropDownList)
                gvr = CType(ddl.NamingContainer, GridViewRow)
                txt = CType(gvr.FindControl("txtCantidad"), TextBox)
            End If
            If ddl.SelectedValue <> "0" Then
                If ddl.SelectedValue <> gvr.Cells(8).Text Then
                    Dim auxMaterial As New Productos.Material(gvr.Cells(2).Text)
                    If auxMaterial.Registrado Then
                        Dim cantidadRegistrada As Integer
                        Integer.TryParse(txt.Text, cantidadRegistrada)
                        Dim cantidad As Integer = cantidadRegistrada * auxMaterial.CantidadEmpaque
                        CType(gvr.FindControl("lblCantidadACargar"), Label).Text = cantidad.ToString
                        'gvr.Cells(6).Text = cantidad.ToString
                    Else
                        epPrincipal.showWarning("No fue posible obtener la información del material en el sistema, " & _
                                                "para realizar el cálculo de la cantidad en la unidad de medida correcta. Por favor intente nuevamente")
                    End If
                Else
                    CType(gvr.FindControl("lblCantidadACargar"), Label).Text = txt.Text
                End If
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al tratar de evaluar Cantidad según Unidad de Medida. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnReimprimirDocumento_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReimprimirDocumento.Click
        lblRespuesta.Text = String.Empty
        mpeReimpresionDocumento.Show()
    End Sub

    Protected Sub btnReImprimir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReImprimir.Click
        Try
            Dim doc As New GeneradorDocumentosSAP
            doc.idDocumento = txtNumeroDocumento.Text
            doc.AnioEjercicio = txtYearEjercicion.Text
            doc.ModoTratamiento = GeneradorDocumentosSAP.modoTratamientoDoc.reimpresion
            doc.TipoDocumento = GeneradorDocumentosSAP.tipoDoc.Material
            doc.NombreArchivo = "CargueRecepcion_"
            Dim ruta As String = Server.MapPath("archivos_SAP")
            Dim resul As ResultadoProceso = doc.GenerarDocumento(ruta)
            If resul.Valor = 0 Then
                'Imprimir documento y volver a consultar (depende de si se acabó la Orden de Recepción)
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "newWindow", "window.open ('archivos_SAP/" & Path.GetFileName(doc.RutaDocumento) & "','CargueRecepcion', 'status=1, toolbar=0, location=0,menubar=1,directories=0,resizable=1,scrollbars=1'); ", True)
                lblRespuesta.CssClass = "ok"
                lblRespuesta.Text = "Documento generado"
            Else
                lblRespuesta.CssClass = "error"
                lblRespuesta.Text = resul.Mensaje
            End If
            mpeReimpresionDocumento.Show()
        Catch ex As Exception
            mpeReimpresionDocumento.Hide()
            epPrincipal.showError("Error al reimprimir el numero de documento " & txtNumeroDocumento.Text & ". " & ex.Message)
        End Try
    End Sub

    Protected Sub gvInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvInfo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Try
                Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim dtSeriales As New DataTable
                dtSeriales = ObtenerListadoSeriales(fila("material").ToString(), idOrdenRecepcion, fila("centro").ToString(), Enumerados.EstadoBinario.Activo)
                If Not dtSeriales Is Nothing AndAlso dtSeriales.Rows.Count > 0 Then
                    TryCast(e.Row.FindControl("imgVerSeriales"), ImageButton).Visible = True
                Else
                    TryCast(e.Row.FindControl("imgVerSeriales"), ImageButton).Visible = False
                End If
                Dim hfMaterialCentroCantidad As HiddenField = CType(e.Row.FindControl("hfMaterialCentroCantidad"), HiddenField)
                hfMaterialCentroCantidad.Value = fila("centro").ToString() & "_" & fila("almacen").ToString() & "_" & fila("cantidadPendienteCargar").ToString() & "_" & fila("material").ToString()
                Dim lnkCantidadPendiente As LinkButton = TryCast(e.Row.FindControl("lnkCantidadPendiente"), LinkButton)
                lnkCantidadPendiente.Enabled = IIf(CInt(lnkCantidadPendiente.Text) > 0, True, False)
            Catch ex As Exception
                epPrincipal.showError("Error al realizar enlace de datos. " & ex.Message)
            End Try
        End If
    End Sub

    Protected Sub gvInfo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvInfo.RowCommand
        Try
            Dim fila As GridViewRow

            If e.CommandName = "verSerialesCargados" Then
                fila = TryCast(DirectCast(e.CommandSource, ImageButton).NamingContainer, GridViewRow)
                If tipoCaso = 2 Then _
                    VerSerialesTarjetasPrepago(gvInfo.DataKeys(fila.RowIndex).Values(0).ToString(), gvInfo.DataKeys(fila.RowIndex).Values(1).ToString(), Enumerados.EstadoBinario.Activo)
                If tipoCaso = 3 Then _
                    VerSerialesToken(gvInfo.DataKeys(fila.RowIndex).Values(0).ToString(), gvInfo.DataKeys(fila.RowIndex).Values(1).ToString(), Enumerados.EstadoBinario.Activo)
            ElseIf e.CommandName = "verSerialesPendientes" Then
                fila = TryCast(DirectCast(e.CommandSource, LinkButton).NamingContainer, GridViewRow)
                If tipoCaso = 2 Then _
                    VerSerialesTarjetasPrepago(gvInfo.DataKeys(fila.RowIndex).Values(0).ToString(), gvInfo.DataKeys(fila.RowIndex).Values(1).ToString())
                If tipoCaso = 3 Then _
                    VerSerialesToken(gvInfo.DataKeys(fila.RowIndex).Values(0).ToString(), gvInfo.DataKeys(fila.RowIndex).Values(1).ToString())
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al cargar los datos " & ex.Message)
        End Try
    End Sub

    Private Sub VerSerialesTarjetasPrepago(ByVal material As String, ByVal centro As String, Optional ByVal cargado As Enumerados.EstadoBinario = Enumerados.EstadoBinario.Inactivo)
        Dim dt As DataTable = ObtenerListadoSeriales(material, idOrdenRecepcion, centro, cargado)
        Dim nombreColumnas As New ArrayList
        Dim dtDatos As DataTable = EstructuraDtSerialesPacas(nombreColumnas)
        dtDatos.Merge(dt, True, MissingSchemaAction.Ignore)
        MetodosComunes.exportarDatosAExcelGemBox(HttpContext.Current, dtDatos, "Reporte de Seriales de Pacas", "ReporteSerialesPacas.xls", Server.MapPath("../archivos_planos/ReporteSerialesPacas.xls"), nombreColumnas)
    End Sub

    Private Function EstructuraDtSerialesPacas(Optional ByRef nombreColumnas As ArrayList = Nothing) As DataTable
        Dim dt As New DataTable("SerialesPacas")
        Dim dcLote As New DataColumn("lote", GetType(String))
        Dim dcMaterial As New DataColumn("material", GetType(String))
        Dim dcReferenciaMaterial As New DataColumn("descripcionMaterial", GetType(String))
        Dim dcFechaVencimiento As New DataColumn("fechaVencimiento", GetType(String))
        'Dim dcContabilizacion As New DataColumn("contabilizacionCargue", GetType(String))
        Dim dcRegion As New DataColumn("nombreRegion", GetType(String))
        With dt.Columns
            .Add(dcLote)
            .Add(dcMaterial)
            .Add(dcReferenciaMaterial)
            .Add(dcFechaVencimiento)
            .Add(dcRegion)
        End With
        'dt.Columns.Add(dcContabilizacion)
        If Not nombreColumnas Is Nothing Then
            With nombreColumnas
                .Add("Lote")
                .Add("Material")
                .Add("Descripción material")
                .Add("Fecha de vencimiento")
                .Add("Region")
            End With
        End If
        Return dt
    End Function

    Private Sub VerSerialesToken(ByVal material As String, ByVal centro As String, Optional ByVal cargado As Enumerados.EstadoBinario = Enumerados.EstadoBinario.Inactivo)
        Dim dt As DataTable = ObtenerListadoSeriales(material, idOrdenRecepcion, centro, cargado)
        Dim nombreColumnas As New ArrayList
        Dim dtDatos As DataTable = EstructuraDtSerialesToken(nombreColumnas)
        dtDatos.Merge(dt, True, MissingSchemaAction.Ignore)
        MetodosComunes.exportarDatosAExcelGemBox(HttpContext.Current, dtDatos, "Reporte de Seriales Token", "ReporteSerialesToken.xls", Server.MapPath("../archivos_planos/ReporteSerialesToken.xls"), nombreColumnas)
    End Sub

    Private Function EstructuraDtSerialesToken(Optional ByRef nombreColumnas As ArrayList = Nothing) As DataTable
        Dim dt As New DataTable("SerialesToken")
        Dim dcSerial As New DataColumn("serial", GetType(String))
        Dim dcMaterial As New DataColumn("material", GetType(String))
        Dim dcReferenciaMaterial As New DataColumn("referenciaMaterial", GetType(String))
        Dim dcFechaVencimiento As New DataColumn("fechaVencimiento", GetType(String))
        Dim dcRegion As New DataColumn("nombreRegion", GetType(String))
        'Dim dcContabilizacion As New DataColumn("contabilizacionCargue", GetType(String))
        With dt.Columns
            .Add(dcSerial)
            .Add(dcMaterial)
            .Add(dcReferenciaMaterial)
            .Add(dcFechaVencimiento)
            .Add(dcRegion)
        End With
        If Not nombreColumnas Is Nothing Then
            With nombreColumnas
                .Add("Serial")
                .Add("Material")
                .Add("Descripción material")
                .Add("Fecha de vencimiento")
                .Add("Region")
            End With
        End If
        Return dt
    End Function

    <Services.WebMethod()> _
    Public Shared Function HoraSesion() As String
        Return Date.Now.ToString()
    End Function
End Class