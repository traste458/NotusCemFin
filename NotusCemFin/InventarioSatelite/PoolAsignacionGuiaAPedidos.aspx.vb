Imports System.Collections.Generic
Imports System.IO
Imports DevExpress.Web
Imports GemBox.Spreadsheet
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Estructuras

Public Class PoolAsignacionGuiaAPedidos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
#If DEBUG Then
        Session("usxp001") = 64045

#End If
        Seguridad.verificarSession(Me)

        Try
            If Not IsPostBack Then
                With epPrincipal
                    .showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                    .setTitle("Pool de Asignación de Guías a Pedidos")
                End With

                deFechaInicial.Date = DateSerial(Year(DateTime.Now), Month(DateTime.Now), 1)
                deFechaFinal.Date = DateTime.Now
                chkEsAsignacion.Checked = True

                gvErrorAsignacion.ClientVisible = False

                CargarComboTiposBodega()
                CargarDatos()
            End If

        Catch ex As Exception
            epPrincipal.showError("Error al cargar la página: " & ex.Message)
        End Try
    End Sub

    Private Sub CargarComboTiposBodega()
        Dim dt As New DataTable
        Try

            dt = BodegaSatelite.ObtieneTiposDeBodega()

            MetodosComunes.CargarComboDX(cmbTipoBodega, dt, "idTipo", "nombre")
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los tipos de bodega. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarDatos()
        Try
            Dim dtDespacho As New DataTable
            Dim filtros As New FiltroDespachoSinPedidoSatelite
            Dim aux As Integer = 0
            If deFechaInicial.Value <> Date.MinValue Then
                filtros.FechaIncio = CDate(deFechaInicial.Value)
                deFechaInicial.Date = CDate(deFechaInicial.Value)
                aux = 1
            End If
            If deFechaFinal.Value <> Date.MinValue Then
                filtros.FechaFin = CDate(deFechaFinal.Value)
                deFechaFinal.Date = CDate(deFechaFinal.Value)
                aux = 1
            End If
            With filtros
                .IdUsuario = Session("usxp001")
                .EsAsignacionGuia = True
                dtDespacho = BodegaSatelite.ObtenerDespachosAsignacionGuia(filtros)
                Session("dtDespachos") = dtDespacho

                If dtDespacho.Rows.Count > 0 Then
                    btnExportSinAsigna.ClientVisible = True
                End If

                With gridInfoDespacho
                    .DataSource = dtDespacho
                    .DataBind()
                End With
            End With
        Catch ex As Exception
            epPrincipal.showError("No fué posible establecer cargar datos de despacho en proceso: " & ex.Message)
        End Try
    End Sub


    Protected Sub cmbBodegasDisp_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cmbBodegasDisp.Callback
        epPrincipal.clear()
        Try
            CargarComboBodegasDisponibles(e.Parameter)
        Catch ex As Exception
            Throw New Exception("Error al tratar de consultar las bodegas. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarComboBodegasDisponibles(ByVal tipoBodega As Integer)
        Dim dt As New DataTable
        Try
            Dim idUsuario As Integer = Session("usxp001")
            dt = ILSBusinessLayer.BodegaSatelite.ObtenerBodegasUsuarioPedido(idUsuario, tipoBodega)
            MetodosComunes.CargarComboDX(cmbBodegasDisp, dt, "idbodega", "bodega")
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener las bodegas disponibles. " & ex.Message)
        End Try
    End Sub

    Protected Sub cpPrincipal_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpPrincipal.Callback
        Dim resultado As New ILSBusinessLayer.ResultadoProceso
        epPrincipal.clear()

        Try
            Dim arrayParameters As String()
            arrayParameters = Split(e.Parameter.ToString, ":")
            Select Case arrayParameters(0)
                Case "200"
                    Dim dtDespacho As New DataTable
                    Dim filtros As New FiltroDespachoSinPedidoSatelite

                    If deFechaInicial.Value <> Date.MinValue Then
                        filtros.FechaIncio = CDate(deFechaInicial.Value)
                        deFechaInicial.Date = CDate(deFechaInicial.Value)
                    End If

                    If deFechaFinal.Value <> Date.MinValue Then
                        filtros.FechaFin = CDate(deFechaFinal.Value)
                        deFechaFinal.Date = CDate(deFechaFinal.Value)
                    End If

                    If cmbBodegasDisp.Value > 0 Then
                        filtros.IdBodegaOrigen = CInt(cmbBodegasDisp.Value)
                    End If

                    If txtPedido.Text <> "" Then
                        filtros.NumeroPedido = txtPedido.Text.Trim()
                    End If

                    With filtros
                        .IdUsuario = Session("usxp001")
                        .EsAsignacionGuia = chkEsAsignacion.Checked
                        dtDespacho = BodegaSatelite.ObtenerDespachosAsignacionGuia(filtros)
                        Session("dtDespachos") = dtDespacho

                        If dtDespacho.Rows.Count > 0 Then
                            btnExportSinAsigna.ClientVisible = True
                        End If

                        With gridInfoDespacho
                            .DataSource = dtDespacho
                            .DataBind()
                        End With
                    End With

                Case "ingresaGuia"

                    Dim recepcion As New RecepcionSatelite

                    With recepcion

                        If cmbTransportadora.Value > 0 Then
                            .IdTransportadora = cmbTransportadora.Value
                        End If

                        If Not String.IsNullOrEmpty(txtGuia.Text) Then
                            .NumeroGuia = txtGuia.Text.Trim
                        End If

                        If Not String.IsNullOrEmpty(txtCuenta.Text) Then
                            .NumeroCuenta = txtCuenta.Text.Trim
                        End If

                        .IdPedido = CDec(hfIdPedido("idPedido"))

                        resultado = .IngresarGuiaTransportadoraDespacho()

                        If resultado.Valor = 0 Then
                            epPrincipal.showSuccess(resultado.Mensaje)
                            CargarDatos()
                        Else
                            epPrincipal.showError(resultado.Mensaje)
                        End If

                    End With

            End Select


        Catch ex As Exception
            epPrincipal.showError("Error al tratar de ejecutar operación : " & ex.Message)
        End Try
    End Sub

    Protected Sub gridDetail_BeforePerformDataSelect(sender As Object, e As EventArgs)
        Try
            Session("idPedido") = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
            CargarDetallePedido(TryCast(sender, ASPxGridView))
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos de órdenes de recepción " & ex.Message)
        End Try
    End Sub

    Private Sub CargarDetallePedido(gv As ASPxGridView)

        If Session("idPedido") IsNot Nothing Then

            Dim idPedido As Decimal = CDec(Session("idPedido"))
            Dim dtDetalle As New DataTable
            dtDetalle = ObtenerDetalle(idPedido)
            Session("dtDetalle") = dtDetalle
            With gv
                .DataSource = Session("dtDetalle")
            End With
        Else
            Throw New Exception("No se pudo establecer el identificador del despacho, por favor intente nuevamente.")
        End If
    End Sub

    Private Function ObtenerDetalle(ByVal idPedido As Decimal) As DataTable
        Dim dtResultado As New DataTable
        Try
            Dim objRecepcion As New RecepcionSatelite
            With objRecepcion
                .IdPedido = idPedido
                dtResultado = .ObtenerPedidoDespachoDetalle()
            End With
        Catch ex As Exception
            Throw New Exception("Se presento un error al cargar el detalle del despacho:." & ex.Message)
        End Try
        Return dtResultado
    End Function

    Protected Sub gridInfoDespacho_DataBinding(sender As Object, e As EventArgs) Handles gridInfoDespacho.DataBinding
        gridInfoDespacho.DataSource = Session("dtDespachos")
    End Sub

    Private Sub Exportar()
        Dim cantidadRegistros As Integer = 0
        Dim aux As Integer = 0
        Try
            Dim dtDespachos As New DataTable

            If Session("dtDespachos") IsNot Nothing AndAlso CType(Session("dtDespachos"), DataTable).Rows.Count > 0 Then

                Dim filtros As New FiltroDespachoSinPedidoSatelite

                If deFechaInicial.Value <> Date.MinValue Then
                    filtros.FechaIncio = CDate(deFechaInicial.Value)
                    deFechaInicial.Date = CDate(deFechaInicial.Value)
                End If

                If deFechaFinal.Value <> Date.MinValue Then
                    filtros.FechaFin = CDate(deFechaFinal.Value)
                    deFechaFinal.Date = CDate(deFechaFinal.Value)
                End If

                If cmbBodegasDisp.Value > 0 Then
                    filtros.IdBodegaOrigen = CInt(cmbBodegasDisp.Value)
                End If

                If txtPedido.Text <> "" Then
                    filtros.NumeroPedido = txtPedido.Text.Trim()
                End If

                filtros.IdUsuario = Session("usxp001")
                filtros.EsAsignacionGuia = chkEsAsignacion.Checked
                filtros.opcion = 1

                dtDespachos = BodegaSatelite.ObtenerDespachosAsignacionGuiaReporte(filtros)
                Session("dtDespachos") = dtDespachos

                Dim arrayNombre As New ArrayList
                arrayNombre = Nothing

                MetodosComunes.exportarDatosAExcelGemBox(HttpContext.Current, dtDespachos, "Pool de Despachos para Asignacion de Guías", "PoolPedidoDespachoAsignacionGuias.xls", Server.MapPath("../archivos_planos/PoolPedidoDespachoAsignacionGuias.xls"), arrayNombre, True)
                epPrincipal.showSuccess("informe Generado Correctamente.")
            Else
                epPrincipal.showWarning("No se encontraron datos para exportar, por favor intente nuevamente.")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ExportarIdPedidos()
        Dim cantidadRegistros As Integer = 0
        Dim aux As Integer = 0
        Try
            Dim dtDespachos As New DataTable
            Dim dtTransportadoras As New DataTable
            Dim objDataSet As New DataSet

            If Session("dtDespachos") IsNot Nothing AndAlso CType(Session("dtDespachos"), DataTable).Rows.Count > 0 Then

                Dim filtros As New FiltroDespachoSinPedidoSatelite

                If deFechaInicial.Value <> Date.MinValue Then
                    filtros.FechaIncio = CDate(deFechaInicial.Value)
                    deFechaInicial.Date = CDate(deFechaInicial.Value)
                End If

                If deFechaFinal.Value <> Date.MinValue Then
                    filtros.FechaFin = CDate(deFechaFinal.Value)
                    deFechaFinal.Date = CDate(deFechaFinal.Value)
                End If

                If cmbBodegasDisp.Value > 0 Then
                    filtros.IdBodegaOrigen = CInt(cmbBodegasDisp.Value)
                End If

                If txtPedido.Text <> "" Then
                    filtros.NumeroPedido = txtPedido.Text.Trim()
                End If

                filtros.IdUsuario = Session("usxp001")
                filtros.EsAsignacionGuia = chkEsAsignacion.Checked
                filtros.opcion = 2

                dtTransportadoras = BodegaSatelite.ObtenerTransportadorasActivas()
                dtDespachos = BodegaSatelite.ObtenerDespachosAsignacionGuiaReporte(filtros)

                dtDespachos.TableName = "Pedidos"
                dtTransportadoras.TableName = "Transportadoras"

                objDataSet.Tables.Add(dtDespachos)
                objDataSet.Tables.Add(dtTransportadoras)

                Dim arrayNombre As New ArrayList
                arrayNombre = Nothing

                MetodosComunes.exportarDatosSinTitulo(HttpContext.Current, objDataSet, Nothing, "ReportePedidosYTransportadoras.xls", Server.MapPath("../archivos_planos/ReportePedidosYTransportadoras.xls"), arrayNombre, False)
                epPrincipal.showSuccess("informe Generado Correctamente.")
            Else
                epPrincipal.showWarning("No se encontraron datos para exportar, por favor intente nuevamente.")
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ProcesarArchivo(upLoadedFile As UploadedFile)
        Try
            MetodosComunes.setGemBoxLicense()
            Dim dtMaterial As New DataTable()
            Dim dtAsignacion As New DataTable
            Dim resultado As New ResultadoProceso
            epPrincipal.clear()
            Dim fec As String = DateTime.Now.ToString("HH:mm:ss:fff").Replace(":", "_")
            'Dim objEntregaCliente As New EntregaDespachoCliente
            Dim validacionArchivo As New ResultadoProceso

            If upLoadedFile.ContentLength <= 10485760 Then
                If upLoadedFile.FileName <> "" Then
                    'Dim ruta As String = "C:\Users\user\Documents\Mis archivos recibidos\"
                    Dim ruta As String = HerramientasFuncionales.RUTAALMACENAMIENTOARCHIVOS & "ArchivosTemporales\"
                    Dim nombreArchivo As String = "CargueAsignacionGuia_" & Session("usxp001") & fec & Path.GetExtension(upLoadedFile.FileName)
                    ruta += nombreArchivo
                    upLoadedFile.SaveAs(ruta)
                    Dim miExcel As New ExcelFile
                    Dim fileExtension As String = Path.GetExtension(upLoadedFile.FileName)
                    If (fileExtension <> "") Then
                        fileExtension = fileExtension.ToUpper()
                    End If

                    Try
                        Select Case fileExtension
                            Case ".XLS"
                                miExcel.LoadXls(ruta)
                            Case ".XLSX"
                                miExcel.LoadXlsx(ruta, XlsxOptions.None)
                                Exit Select
                        End Select
                    Catch ex As Exception
                        Throw New Exception("El archivo esta incorrecto o no tiene el formato esperado. Por favor verifique: " & ex.Message)
                    End Try

                    If miExcel.Worksheets.Count <= 2 Then
                        Dim oWsInfogenera As ExcelWorksheet = miExcel.Worksheets.Item(0)
                        Dim extencion As String = Path.GetExtension(upLoadedFile.FileName).ToLower
                        If extencion = ".xls" Or extencion = ".xlsx" Then
                            If oWsInfogenera.CalculateMaxUsedColumns() <> 6 Then
                                If oWsInfogenera.CalculateMaxUsedColumns() > 6 Then
                                    epPrincipal.showError("El archivo tiene mas columnas de las requeridas: " & oWsInfogenera.CalculateMaxUsedColumns().ToString())
                                Else
                                    epPrincipal.showError("El archivo tiene menos columnas de las requeridas: " & oWsInfogenera.CalculateMaxUsedColumns().ToString())
                                End If
                                Exit Sub
                            End If
                        End If


                        Dim filaInicial As Integer = oWsInfogenera.Cells.FirstRowIndex
                        Dim columnaInicial As Integer = oWsInfogenera.Cells.FirstColumnIndex
                        dtMaterial = CrearEstructuraInfo()

                        AddHandler oWsInfogenera.ExtractDataEvent, AddressOf ExtractDataErrorHandler
                        oWsInfogenera.ExtractToDataTable(dtMaterial, oWsInfogenera.Rows.Count, ExtractDataOptions.SkipEmptyRows,
                                    oWsInfogenera.Rows(filaInicial + 1), oWsInfogenera.Columns(columnaInicial))

                        dtMaterial.Columns.Add(New DataColumn("fila"))
                        Dim fil As Integer = 1
                        For Each row As DataRow In dtMaterial.Rows
                            row("fila") = fil
                            fil = fil + 1
                        Next

                        'objEntregaCliente.IdUsuario = Session("usxp001")

                        dtMaterial.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), Session("usxp001")))
                        dtMaterial.AcceptChanges()

                        'dtAsignacion = objEntregaCliente.CargarMasivoAsignacionGuiaDespacho(dtMaterial)

                        Session("tableAsignacionGuia") = dtAsignacion

                        'resultado = objEntregaCliente.resultado


                        If (resultado.Valor = 0) Then
                            epPrincipal.showSuccess(resultado.Mensaje)
                            gvErrorAsignacion.ClientVisible = False
                            CargarDatos()
                        Else
                            epPrincipal.showWarning(resultado.Mensaje)
                            gvErrorAsignacion.ClientVisible = True
                            gvErrorAsignacion.DataSource = CType(Session("tableAsignacionGuia"), DataTable)
                            gvErrorAsignacion.DataBind()
                        End If

                    End If

                End If
            End If
        Catch ex As Exception
            epPrincipal.showError("Se generó un error al intentar procesar el archivo: " & ex.Message)
        End Try
    End Sub

    Private Function CrearEstructuraInfo() As DataTable
        Dim dtAux As New DataTable

        With dtAux.Columns
            dtAux.Columns.Add("IdPedido", GetType(Decimal))
            dtAux.Columns.Add("NumeroPedido", GetType(String))
            dtAux.Columns.Add("BodegaOrigen", GetType(String))
            dtAux.Columns.Add("IdTransportadora", GetType(Decimal))
            dtAux.Columns.Add("Guia", GetType(String))
            dtAux.Columns.Add("CuentaTransportadora", GetType(String))
        End With
        Return dtAux
    End Function

    Private Sub ExtractDataErrorHandler(ByVal sender As Object, ByVal e As ExtractDataDelegateEventArgs)
        If e.ErrorID = ExtractDataError.WrongType Then
            If e.ExcelValue Is Nothing Then
                e.DataTableValue = DBNull.Value
            Else
                e.DataTableValue = e.ExcelValue.ToString()
            End If
            e.Action = ExtractDataEventAction.Continue
        End If
    End Sub

    Protected Sub lbExportar_Click(sender As Object, e As EventArgs) Handles lbExportar.Click
        Exportar()
    End Sub

    Protected Sub btnArchivo_Click(sender As Object, e As EventArgs) Handles btnArchivo.Click

    End Sub

    Protected Sub btnExportSinAsigna_Click(sender As Object, e As EventArgs) Handles btnExportSinAsigna.Click
        ExportarIdPedidos()
    End Sub

    Protected Sub upArchivoAsignacionGuia_FileUploadComplete(sender As Object, e As FileUploadCompleteEventArgs) Handles upArchivoAsignacionGuia.FileUploadComplete
        If upArchivoAsignacionGuia.HasFile Then
            ProcesarArchivo(e.UploadedFile)
            aspLabel.Text = e.UploadedFile.FileName.ToString
        Else
            epPrincipal.showWarning("Debe seleccionar el archivo a cargar")
        End If
    End Sub

    Protected Sub Link_Init_LeerPedido(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim lnkPedido As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim container As GridViewDataItemTemplateContainer = CType(lnkPedido.NamingContainer, GridViewDataItemTemplateContainer)
            lnkPedido.ClientSideEvents.Click = lnkPedido.ClientSideEvents.Click.Replace("{0}", container.KeyValue)
            Dim idEstado As Integer = CInt(container.Grid.GetRowValues(container.VisibleIndex, "idEstado").ToString())
            'lnkPedido.ClientVisible = False
            'gridInfoDespacho.Columns("Asignar Guía").Visible = False

            For Each c As GridViewDataColumn In gridInfoDespacho.Columns
                If (c.FieldName.ToString()).StartsWith("Pict") Then
                    c.Visible = False
                End If
            Next c

            lnkPedido.ClientVisible = False
            If chkEsAsignacion.Checked = True And idEstado = 313 Then
                lnkPedido.ClientVisible = True
                'gridInfoDespacho.Columns("Asignar Guía").Visible = True
            End If

        Catch ex As Exception
            epPrincipal.showError("No fué posible establecer el identificador de la orden inventario: " & ex.Message)
        End Try
    End Sub

    Private Sub CargarTransportadoras()
        Dim dt As New DataTable
        Try
            Dim objRecepcionSatelite As New RecepcionSatelite
            With objRecepcionSatelite
                dt = .ObtenerTransportadoras()
            End With

            MetodosComunes.CargarComboDX(cmbTransportadora, dt, "idTransportadora", "transportadora")
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos de tipo transportadoras. " & ex.Message)
        End Try
    End Sub

    Protected Sub popupGuia_WindowCallback(source As Object, e As PopupWindowCallbackArgs) Handles popupGuia.WindowCallback
        CargarTransportadoras()
    End Sub

    Protected Sub gvErrorAsignacion_DataBinding(sender As Object, e As EventArgs) Handles gvErrorAsignacion.DataBinding
        gvErrorAsignacion.DataSource = CType(Session("tableAsignacionGuia"), DataTable)
    End Sub
End Class