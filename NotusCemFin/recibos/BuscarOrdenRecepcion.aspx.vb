Imports ILSBusinessLayer
Imports ILSBusinessLayer.Recibos
Imports System.Linq
Imports System.IO
Imports ILSBusinessLayer.Comunes

Partial Public Class BuscarOrdenRecepcion
    Inherits System.Web.UI.Page

#Region "Atributos"

    Private _folderTempImage As String

#End Region

#Region "Propiedades"

    Public Property FolderTempImage As String
        Get
            If Session("_folderTempImage") IsNot Nothing Then _folderTempImage = Session("_folderTempImage")
            Return _folderTempImage
        End Get
        Set(value As String)
            _folderTempImage = value
            Session("_folderTempImage") = _folderTempImage
        End Set
    End Property

#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            EncabezadoPagina.clear()
            If Not IsPostBack Then
                EncabezadoPagina.setTitle("Búsqueda de Orden de Recepción")
                If Request.UrlReferrer IsNot Nothing Then
                    EncabezadoPagina.showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                End If
                ObtenerTipoProducto()
                ObtenerTipoRecepcion()
                ObtenerEstado()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            Dim dtRespuesta As New DataTable

            Dim filtro As Estructuras.FiltroOrdenRecepcion
            'Dim filtroTP As Estructuras.FiltroTipoProducto
            'Dim filtroTipoProducto As New ArrayList
            'Dim dtTPAplicativoWeb As DataTable
            With filtro
                If txtIdOrdenRecepcion.Text.Trim() <> String.Empty Then .IdOrdenRecepcion = txtIdOrdenRecepcion.Text.Trim()
                .NumeroOrden = txtNoOrden.Text.Trim()
                If txtIdOrden.Text.Trim() <> String.Empty Then .IdOrdenCompra = txtIdOrden.Text.Trim()
                .IdTipoProducto = ddlTipoProducto.SelectedValue
                .IdTipoRecepcion = ddlTipoRecepcion.SelectedValue
                .IdEstado = CInt(ddlEstado.SelectedValue)
                If txtFechaInicial.Text <> String.Empty Then .FechaInicial = CDate(txtFechaInicial.Text)
                If txtFechaFinal.Text <> String.Empty Then .FechaFinal = CDate(txtFechaFinal.Text)
            End With

            'filtroTP.tipoAplicativo = 1
            'filtroTP.Activo = Enumerados.EstadoBinario.Activo
            'dtTPAplicativoWeb = Productos.TipoProducto.ObtenerListado(filtroTP)
            'For Each fila As DataRow In dtTPAplicativoWeb.Rows
            '    filtroTipoProducto.Add(CInt(fila("idTipoProducto")))
            'Next
            'filtro.ListaIdTipoProducto = filtroTipoProducto

            Dim exporDetalleSerialOrdenRecep As String = MetodosComunes.seleccionarConfigValue("EXPORTAR_SERIALESORDEN_RECEPCION")
            Dim vTipoProducto As DataTable
            If (exporDetalleSerialOrdenRecep IsNot Nothing) Then
                Session("ExporDetalleSerialOrdenRecep") = exporDetalleSerialOrdenRecep
            Else
                Session.Remove("ExporDetalleSerialOrdenRecep")
            End If



            dtRespuesta = Recibos.OrdenRecepcion.ObtenerListado(filtro)
            dtRespuesta.DefaultView.Sort = "idOrdenRecepcion DESC"
            Session("dtRespuesta") = dtRespuesta
            gvDatosRecepcion.DataSource = dtRespuesta
            gvDatosRecepcion.DataBind()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de buscar las ordenes de recepcion. " & ex.Message)
        End Try
    End Sub

    Private Sub gvDatosRecepcion_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatosRecepcion.RowCommand
        Try
            hfIdOrden.Value = e.CommandArgument.ToString
            Dim idOrdenRecepcion As Long = CLng(e.CommandArgument.ToString)
            If e.CommandName.Equals("Detalle") Then
                CargarConsecutivos(idOrdenRecepcion)
                mdDetalle.Show()
                If Session("dtRespuesta") IsNot Nothing Then
                    gvDatosRecepcion.DataSource = Session("dtRespuesta")
                    gvDatosRecepcion.DataBind()
                End If
            End If
            If e.CommandName.Equals("VerImagen") Then
                Try
                    FolderTempImage = "\recibos\images\ImagenRecepcion\" & Guid.NewGuid().ToString()
                    If Not Directory.Exists(Server.MapPath("~") & FolderTempImage) Then
                        Directory.CreateDirectory(Server.MapPath("~") & FolderTempImage)
                    End If
                    Dim miRecepcion As New OrdenRecepcion(idOrdenRecepcion)
                    For Each imgProd As OrdenRecepcion.Imagen In miRecepcion.ListaImagenes
                        Dim objImagen As New Imagen()
                        With objImagen
                            .ArregloByte_Imagen(imgProd.imagen, Server.MapPath("~") & FolderTempImage & "\" & imgProd.nombreImagen, imgProd.contenType)
                        End With
                    Next
                    isImagenes.ImageSourceFolder = "~\" & FolderTempImage
                    mpeVisualizacionImagen.Show()
                Catch ex As Exception
                    EncabezadoPagina.showError("Error al trata de visualizar las imagenes: " & ex.Message)
                End Try
            End If
            If e.CommandName.Equals("ExportarSeriales") Then
                Dim objodetalleseriaorderecep As New DetalleSerialOrdenRecepcion()
                Dim dtDatos As DataTable
                Dim excelma As New ExcelManager()
                Try
                    With objodetalleseriaorderecep
                        dtDatos = .ObtenerSerialOrdenRecepcion(idOrdenRecepcion)
                        If dtDatos IsNot Nothing Then
                            Dim ruta As String = Server.MapPath("../archivos_planos/")
                            MetodosComunes.exportarDtAExcelGemBox(HttpContext.Current, dtDatos, "Reporte Detalle Seriales OrdenRecepcion", "DetalleSerialesOrdenRecepcion.xls", ruta)
                            'With excelma
                            '    .IncluirEncabezado = True
                            '    .FilaInicial = 1
                            '    .ColumnaInicial = 1
                            '    .NombreHoja = "Seriales Orden de Recepcion"
                            '    .ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                            '    .NombreArchivo = "DetalleSerialesOrdenRecepcion.xlsx"
                            '    .ForzarDescargaDeArchivo(HttpContext.Current, .GenerarExcel(dtDatos))
                            'End With
                        Else
                            EncabezadoPagina.showWarning("No se encontraron registros con el filtro utilizado .")
                        End If


                    End With

                Catch ex As Exception
                    Throw New Exception("Error al tratar de obtener los Seriales de la Orden de Recepcion. " & ex.Message)
                End Try

            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Se presento un error al ejecutar el comando: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCerrarVisualizar_Click(sender As Object, e As System.EventArgs) Handles btnCerrarVisualizar.Click
        Try
            isImagenes.ImageSourceFolder = Nothing
            'Se eliminan las imagenes temporales
            If Directory.Exists(Server.MapPath("~") & FolderTempImage) Then Directory.Delete(Server.MapPath("~") & FolderTempImage, True)
            mpeVisualizacionImagen.Hide()
        Catch : End Try
    End Sub

    Protected Sub gvDatosRecepcion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDatosRecepcion.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Try
                Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim idOrdenRecepcion As Integer = CInt(fila("idOrdenRecepcion"))
                Dim idEstado As Integer = CInt(fila("idEstado"))
                Dim idTipoProducto As Integer = CInt(fila("idTipoProducto"))
                Dim imgBtnAgregarDetalleOrden As ImageButton = e.Row.FindControl("imgAgregarDetalleOrdenRecepcion")
                Dim imgBtnVerDetalleOrden As ImageButton = e.Row.FindControl("ImbBtnVerDetalleOrden")
                Dim imgBtnExportarLotes As ImageButton = e.Row.FindControl("imgBtnExportarLotes")
                Dim imgBtnCargarSAP As ImageButton = CType(e.Row.FindControl("imgBtnCargarSAP"), ImageButton)
                Dim imgInfo As Control = CType(e.Row.FindControl("imgInfo"), Control)
                Dim imgBtnConsecutivos As ImageButton = e.Row.FindControl("ImgConsecutivos")
                imgInfo.Visible = False
                Dim verDetalle As String
                Dim agregarDetalle As String
                imgBtnAgregarDetalleOrden.Visible = False
                imgBtnVerDetalleOrden.Visible = False
                Dim flagDetalle As Boolean = True
                Dim filtroTP As New Estructuras.FiltroTipoProducto
                Dim dtTPAplicativoWeb As DataTable
                Dim filtroTPAplicativoWeb As New ArrayList
                filtroTP.tipoAplicativo = 1
                filtroTP.Activo = Enumerados.EstadoBinario.Activo
                dtTPAplicativoWeb = Productos.TipoProducto.ObtenerListado(filtroTP)
                For Each filaTP As DataRow In dtTPAplicativoWeb.Rows
                    filtroTPAplicativoWeb.Add(CInt(filaTP("idTipoProducto")))
                Next

                Dim tipoProductoObj As New Productos.TipoProducto(idTipoProducto)

                If idTipoProducto <> Enumerados.TipoProductoMaterial.PAPELERIA Then
                    imgBtnConsecutivos.Visible = False
                Else
                    imgBtnConsecutivos.Visible = True
                End If

                If tipoProductoObj.Instruccionable Then
                    agregarDetalle = "CrearDetalleRecepcion.aspx?orep=" & idOrdenRecepcion.ToString
                    verDetalle = "CrearDetalleRecepcion.aspx?orep=" & idOrdenRecepcion.ToString
                ElseIf idTipoProducto = Productos.TipoProducto.Tipo.MERCHANDISING Then
                    agregarDetalle = "DetalleOrdenRecepcionMerchandising.aspx?ord=" & idOrdenRecepcion.ToString
                    verDetalle = "DetalleOrdenRecepcionMerchandising.aspx?ord=" & idOrdenRecepcion.ToString
                ElseIf filtroTPAplicativoWeb.Contains(idTipoProducto) Then
                    agregarDetalle = "DetalleOrdenRecepcionGeneral.aspx?ord=" & idOrdenRecepcion.ToString
                    verDetalle = "DetalleOrdenRecepcionGeneral.aspx?ord=" & idOrdenRecepcion.ToString
                Else
                    imgInfo.Visible = True
                    imgBtnVerDetalleOrden.Visible = False
                    flagDetalle = False
                End If

                imgBtnAgregarDetalleOrden.PostBackUrl = agregarDetalle
                imgBtnVerDetalleOrden.PostBackUrl = verDetalle
                If Not imgInfo.Visible Then _
                    If idEstado = Recibos.OrdenRecepcion.EstadoOrden.Abierta Then imgBtnAgregarDetalleOrden.Visible = True
                If idEstado = Recibos.OrdenRecepcion.EstadoOrden.Finalizada And flagDetalle Then imgBtnVerDetalleOrden.Visible = True

                Dim ordenRecepcionObj As New Recibos.OrdenRecepcion(idOrdenRecepcion)
                If ordenRecepcionObj.IdTipoRecepcion = 2 Then
                    imgBtnCargarSAP.PostBackUrl = "CargaSAPOrdenCompraProductoNacional.aspx?idrep=" & idOrdenRecepcion.ToString
                    ordenRecepcionObj.CumpleCondicionesCargueProductoNacionalSAP()
                Else
                    imgBtnCargarSAP.Visible = False
                End If
                Dim permisoTipoPro As String = Session("ExporDetalleSerialOrdenRecep")
                Dim exporDetalleSerialOrdenRecep As Boolean = False

                If Not EsNuloOVacio(Session("ExporDetalleSerialOrdenRecep")) Then
                    exporDetalleSerialOrdenRecep = (From tipoProducto As String In permisoTipoPro.Split(",")
                                                    Where (tipoProducto.Equals(idTipoProducto.ToString())) Select tipoProducto).Any()
                End If

                If (exporDetalleSerialOrdenRecep) Then
                    imgBtnExportarLotes.Visible = True
                Else
                    imgBtnExportarLotes.Visible = False
                End If

            Catch ex As Exception
                EncabezadoPagina.showError("Error enlazado datos. " & ex.Message)
            End Try

        End If
    End Sub

    Protected Sub btnBorrarFiltros_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBorrarFiltros.Click
        LimpiarFiltros()
    End Sub

#End Region

#Region "Métodos Privados"

    Protected Sub ObtenerTipoProducto()
        Dim dtTipoProducto As New DataTable
        Dim filtro As New Estructuras.FiltroTipoProducto
        filtro.ExisteModulo = Enumerados.EstadoBinario.Activo
        filtro.IdModulo = 3
        dtTipoProducto = Productos.TipoProducto.ObtenerListado(filtro)
        Try
            With ddlTipoProducto
                .DataSource = dtTipoProducto
                .DataTextField = "descripcion"
                .DataValueField = "idTipoProducto"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja el tipo de producto", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los tipos de producto. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerTipoRecepcion()
        Try
            With ddlTipoRecepcion
                .DataSource = ILSBusinessLayer.Recibos.TipoRecepcion.ObtenerListado
                .DataTextField = "descripcion"
                .DataValueField = "idTipoRecepcion"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja el tipo de recepcion", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los tipos de recepcion. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerEstado()
        Dim dt As New DataTable
        Try
            dt = ILSBusinessLayer.Estado.Obtener(5)
            With ddlEstado
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idEstado"
                .DataBind()
                If dt.Rows.Count > 1 Then
                    .Items.Insert(0, New ListItem("Escoja el Estado", 0))
                End If
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos para Moneda. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarConsecutivos(ByVal OrdenRecepcion As Long)
        Dim dtDatos As New DataTable
        Dim objReporte As New OrdenRecepcion
        With objReporte
            .IdOrdenRecepcion = OrdenRecepcion
            dtDatos = .ConsultarConsecutivoOrdenRecepcion
        End With
        With gvDetalle
            .DataSource = dtDatos
            DataBind()
        End With
    End Sub

    Private Sub LimpiarFiltros()
        Try
            txtIdOrdenRecepcion.Text = ""
            txtIdOrden.Text = ""
            txtNoOrden.Text = ""
            ddlTipoProducto.ClearSelection()
            ddlTipoRecepcion.ClearSelection()
            ddlEstado.ClearSelection()
            txtFechaInicial.Text = ""
            txtFechaFinal.Text = ""
            Session.Remove("dtRespuesta")
            gvDatosRecepcion.DataSource = Session("dtRespuesta")
            gvDatosRecepcion.DataBind()
        Catch ex As Exception
            EncabezadoPagina.showError("Error a limpiar los filtros " & ex.Message)
        End Try
    End Sub

#End Region

    'Protected Sub ObtenerOrdenCompra()
    '    Try
    '        With ddlOrdenCompra
    '            .DataSource = ILSBusinessLayer.Recibos.OrdenCompra.ObtenerListado
    '            .DataTextField = "numeroOrden"
    '            .DataValueField = "idOrden"
    '            .DataBind()
    '            .Items.Insert(0, New ListItem("Escoja la orden de compra", 0))
    '        End With
    '    Catch ex As Exception
    '        Throw New Exception("Error al tratar de obtener las ordenes de recepcion. " & ex.Message)
    '    End Try
    'End Sub

End Class