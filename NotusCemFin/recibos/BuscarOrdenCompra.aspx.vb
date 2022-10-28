Imports ILSBusinessLayer
Imports ILSBusinessLayer.Recibos
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls


Partial Public Class BuscarOrdenCompra
    Inherits System.Web.UI.Page

    Protected WithEvents grillaDatos As Global.System.Web.UI.WebControls.GridView

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            EncabezadoPagina.clear()
            If Not IsPostBack Then
                EncabezadoPagina.setTitle("Buscar Orden de Compra")
                EncabezadoPagina.showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                Dim dtTipoProducto As DataTable = ObtenerTipoProducto()
                EnlazarTipoProducto(dtTipoProducto)
                Session.Remove("dtConsultaOrdenCompra")
                ObtenerProveedor()
                ObtenerMoneda()
                ObtenerInconterm()
                ObtenerEstado()
                VerificarBusqueda()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Private Sub VerificarBusqueda()
        Try
            Dim dtConsultaPrevia As New DataTable
            If Not Session("dtConsultaOrdenCompra") Is Nothing Then
                dtConsultaPrevia = CType(Session("dtConsultaOrdenCompra"), DataTable)
            End If


            If Not dtConsultaPrevia Is Nothing And dtConsultaPrevia.Rows.Count > 0 Then
                grdOrden.DataSource = dtConsultaPrevia
                grdOrden.DataBind()
                If dtConsultaPrevia.Select("distribucionPorRegion <> ''").Length > 0 Then
                    grdOrden.Columns(7).Visible = True
                Else
                    grdOrden.Columns(7).Visible = False
                End If
            End If

        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar una busqueda previa. " & ex.Message)
        End Try
    End Sub

    Private Sub ObtenerOrdenCompra()       
        Try
            Dim dtRespuesta As New DataTable
            Dim filtro As Estructuras.FiltroOrdenCompra
            If txtIdOrden.Text <> "" Then filtro.IdOrden = txtIdOrden.Text.Trim()
            If txtNumeroOrden.Text <> "" Then filtro.NumeroOrden = txtNumeroOrden.Text.Trim()
            If ddlTipoProducto.SelectedValue > 0 Then filtro.IdTipoProducto = CInt(ddlTipoProducto.SelectedValue)
            If ddlProveedor.SelectedValue > 0 Then filtro.IdProveedor = CInt(ddlProveedor.SelectedValue)
            If ddlMoneda.SelectedValue > 0 Then filtro.IdMoneda = CInt(ddlMoneda.SelectedValue)
            If ddlIncoterm.SelectedValue > 0 Then filtro.IdIncoterm = CInt(ddlIncoterm.SelectedValue)
            If ddlEstado.SelectedValue > 0 Then filtro.IdEstado = CInt(ddlEstado.SelectedValue)
            If txtFechaInicial.Text <> String.Empty Then filtro.FechaInicial = CDate(txtFechaInicial.Text)
            If txtFechaFinal.Text <> String.Empty Then filtro.FechaFinal = CDate(txtFechaFinal.Text)

            dtRespuesta = Recibos.OrdenCompra.ObtenerListado(filtro)
            With grdOrden
                .DataSource = dtRespuesta

                If dtRespuesta IsNot Nothing AndAlso dtRespuesta.Rows.Count > 0 Then
                    .Columns(0).FooterText = dtRespuesta.Rows.Count.ToString & " Registro(s) Encontrado(s)"
                    Dim pk() As DataColumn = {dtRespuesta.Columns("idOrden")}
                    dtRespuesta.PrimaryKey = pk
                End If
                Session("dtConsultaOrdenCompra") = dtRespuesta
                .DataBind()
                If dtRespuesta IsNot Nothing AndAlso dtRespuesta.Select("distribucionPorRegion <> ''").Length > 0 Then
                    .Columns(7).Visible = True
                Else
                    .Columns(7).Visible = False
                End If
            End With

            MetodosComunes.mergeGridViewFooter(grdOrden)
        Catch ex As Exception
            Throw New Exception("Error al cargar las ordenes de compra ." + ex.Message)
        End Try
    End Sub

    Protected Sub grdOrden_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdOrden.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim idOrdenCompra = CInt(fila("idOrden"))
            Dim distribucionPorRegion As String = CStr(fila("distribucionPorRegion"))
            'Dim ordenCompra As New Recibos.OrdenCompra(CLng(idOrdenCompra))
            Dim editarDetalleOrdenCompra As ImageButton = e.Row.FindControl("imgEditarOrdenCompra")
            Dim anularOrdenCompra As ImageButton = e.Row.FindControl("imgAnularOrden")
            Dim activarOrdenCompra As ImageButton = e.Row.FindControl("imgActivarOrden")
            Dim hfAnularOrdenMsn As HiddenField = CType(e.Row.FindControl("hfAnularOrdenMsn"), HiddenField)
            Dim idEstadoOc As Integer
            Dim idTipoProductoOc As Integer
            Dim productoRecibidoOc As Boolean

            '***Se carga la información de la Orden de Compra que se está Tratando***'
            If Session("dtConsultaOrdenCompra") IsNot Nothing Then
                Dim dtInfoOrdenCompra As DataTable = CType(Session("dtConsultaOrdenCompra"), DataTable)
                Dim drAux As DataRow = dtInfoOrdenCompra.Rows.Find(idOrdenCompra)
                Integer.TryParse(drAux("idEstado").ToString, idEstadoOc)
                Integer.TryParse(drAux("idTipoProducto").ToString, idTipoProductoOc)
                Boolean.TryParse(drAux("productoRecibido").ToString, productoRecibidoOc)
            Else
                Dim ocAux As New Recibos.OrdenCompra(CLng(idOrdenCompra))
                idEstadoOc = ocAux.IdEstado
                idTipoProductoOc = ocAux.IdTipoProducto
                productoRecibidoOc = ocAux.ProductoRecibido
            End If

            Dim bltDisPorRegion As BulletedList = e.Row.FindControl("bltDisPorRegion")
            If distribucionPorRegion <> "" Then
                bltDisPorRegion.DataSource = distribucionPorRegion.Split(",")
                bltDisPorRegion.DataBind()
            End If
            Dim filtroDetalleOrdenCompra As Estructuras.FiltroDetalleOrdenCompra
            Dim dtDetalle As New DataTable

            With filtroDetalleOrdenCompra
                .IdOrden = idOrdenCompra
                .IdTipoDetalle = TipoDetalleOrdenCompra.TipoDetalle.Principal
            End With
            dtDetalle = Recibos.DetalleOrdenCompra.ObtenerListado(filtroDetalleOrdenCompra)

            editarDetalleOrdenCompra.Visible = True

            If idEstadoOc = OrdenCompra.EstadoOrden.Cancelada Then
                activarOrdenCompra.Visible = True
                anularOrdenCompra.Visible = False
            Else
                anularOrdenCompra.Visible = True
                activarOrdenCompra.Visible = False
            End If

            Dim ordenCompraObj As OrdenCompra = New OrdenCompra(idOrdenCompra)
            If Not ordenCompraObj.PosibleAnular Then
                anularOrdenCompra.ImageUrl = "~/images/Info-32.png"
                anularOrdenCompra.ToolTip = "Información de la orden de compra."
                hfAnularOrdenMsn.Value = ordenCompraObj.MensajeInfo
            Else
                hfAnularOrdenMsn.Value = String.Empty
            End If            

            

            Dim imgShow As System.Web.UI.WebControls.Image = e.Row.FindControl("imgShow")

            '***Se carga la información del Tipo de Producto asociado a la Orden de Compra***'
            Dim dtTipoProducto As DataTable = Session("dtTipoProducto")
            If dtTipoProducto Is Nothing Then dtTipoProducto = ObtenerTipoProducto()
            Dim drTipoProducto As DataRow = dtTipoProducto.Rows.Find(idTipoProductoOc)
            Dim esInstruccionable As Boolean
            If drTipoProducto IsNot Nothing Then Boolean.TryParse(drTipoProducto("instruccionable").ToString, esInstruccionable)

            'If OrdenCompra.IdTipoProducto = 1 Or OrdenCompra.IdTipoProducto = 2 Or OrdenCompra.IdTipoProducto = 7 Then
            If esInstruccionable Then
                editarDetalleOrdenCompra.PostBackUrl = "AgregarDetalleOrdenCompra.aspx?ido=" & idOrdenCompra.ToString
                If dtDetalle.Rows.Count > 0 Then
                    Dim gvr As New GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
                    Dim tabla As Table = CType(e.Row.Parent, Table)
                    imgShow.Visible = True
                    gvr.Cells.Add(CrearCeldasProductoInstruccionable(dtDetalle, idEstadoOc))
                    tabla.Rows.Add(gvr)
                Else
                    imgShow.Visible = False
                End If
            ElseIf idTipoProductoOc = Productos.TipoProducto.Tipo.TARJETAS_PREPAGO Then
                editarDetalleOrdenCompra.PostBackUrl = "AgregarDetalleOrdenCompra.aspx?ido=" & idOrdenCompra.ToString
                If dtDetalle.Rows.Count > 0 Then
                    Dim gvr As New GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
                    Dim tabla As Table = CType(e.Row.Parent, Table)
                    imgShow.Visible = True
                    gvr.Cells.Add(CrearCeldaTarjetasPre(dtDetalle))
                    tabla.Rows.Add(gvr)
                Else
                    imgShow.Visible = False
                End If
            Else
                If productoRecibidoOc Then
                    editarDetalleOrdenCompra.PostBackUrl = "EditarOrdenCompraMerchanPopInsumo.aspx?ido=" & idOrdenCompra.ToString
                Else
                    editarDetalleOrdenCompra.PostBackUrl = "AgregarDetalleOrdenCompra.aspx?ido=" & idOrdenCompra.ToString
                End If

                If dtDetalle.Rows.Count > 0 Then
                    Dim gvr As New GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Normal)
                    Dim tabla As Table = CType(e.Row.Parent, Table)
                    imgShow.Visible = True
                    gvr.Cells.Add(CrearCeldaTarjetasPre(dtDetalle))
                    tabla.Rows.Add(gvr)
                Else
                    imgShow.Visible = False
                End If
            End If
        End If
    End Sub

    Private Function CrearCeldasProductoInstruccionable(ByVal dtDetalle As DataTable, ByVal iEstadoOC As Integer) As TableCell
        Try
            Dim celda As New TableCell()
            Dim grillaDatos As New System.Web.UI.WebControls.GridView()
            Dim container As New System.Web.UI.Control

            AddHandler grillaDatos.RowDataBound, AddressOf grillaDatos_RowDataBound
            grillaDatos.AutoGenerateColumns = False
            grillaDatos.ID = "ControlGrillaDatos"
            Dim columna1 As New BoundField
            columna1.HeaderText = "Fabricante"
            columna1.DataField = "fabricante"
            Dim columna2 As New BoundField
            columna2.HeaderText = "Producto"
            columna2.DataField = "producto"
            Dim columna3 As New BoundField
            columna3.HeaderText = "Cantidad"
            columna3.DataField = "cantidad"
            columna3.ItemStyle.HorizontalAlign = HorizontalAlign.Center
            Dim columna4 As New BoundField
            columna4.HeaderText = "Valor Unitario"
            columna4.DataField = "valorUnitario"
            columna4.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            columna4.DataFormatString = "{0:C}"

            Dim columna5 As New TemplateField()
            columna5.HeaderText = "Factura"

            columna5.ItemTemplate = New PlantillaDinamica2(PlantillaDinamica2.TipoDato.BulletList, "bltFacturas")

            Dim columna6 As New BoundField
            columna6.HeaderText = "Fecha de Registro"
            columna6.DataField = "fechaRegistro"
            columna6.ItemStyle.Width = System.Web.UI.WebControls.Unit.Pixel(150)
            Dim columna7 As New BoundField
            columna7.HeaderText = "Observación"
            columna7.DataField = "observacion"
            columna7.ItemStyle.Width = System.Web.UI.WebControls.Unit.Pixel(200)

            grillaDatos.Columns.Add(columna1)
            grillaDatos.Columns.Add(columna2)
            grillaDatos.Columns.Add(columna3)
            grillaDatos.Columns.Add(columna4)
            grillaDatos.Columns.Add(columna5)
            grillaDatos.Columns.Add(columna6)
            grillaDatos.Columns.Add(columna7)
            Dim columna8 As New HyperLinkField
            With columna8
                .HeaderText = "Facturas"
                .Text = "Adm. Facturas"
                .NavigateUrl = "DetalleOrdenCompra.aspx"
                .DataNavigateUrlFields = New String() {"idDetalle"}
                .DataNavigateUrlFormatString = "DetalleOrdenCompra.aspx?doc={0}"
            End With

            grillaDatos.Columns.Add(columna8)

            grillaDatos.DataSource = dtDetalle
            grillaDatos.DataBind()
            'Ocultar opcion de agregar detalle a la orden de compra
            If CInt(Session("usxp009")) = 98 Then
                grillaDatos.Columns(7).Visible = False
            ElseIf iEstadoOC = 15 Then
                grillaDatos.Columns(7).Visible = False
            End If
            If dtDetalle.Rows.Count > 0 Then celda.CssClass = "DetalleOrden"
            celda.ColumnSpan = grdOrden.Columns.Count
            celda.Controls.Add(grillaDatos)
            Return celda
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar las ordenes. " & ex.Message)
        End Try

    End Function

    Protected Sub grillaDatos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim listaFacturas As New BulletedList
            listaFacturas = CType(e.Row.FindControl("bltFacturas"), BulletedList)

            If Not listaFacturas Is Nothing Then
                Dim idDetalle = CInt(fila("idDetalle"))
                Dim filtro As New Estructuras.FiltroInfoFactura
                filtro.IdDetalleOrdenCompra = idDetalle
                listaFacturas.DataSource = Recibos.InfoFactura.ObtenerListado(filtro)
                listaFacturas.DataTextField = "factura"
                listaFacturas.DataBind()
            End If
        End If
    End Sub

    Private Function CrearCeldaTarjetasPre(ByVal dtDetalle As DataTable) As TableCell
        Try
            Dim celda As New TableCell()
            Dim grillaDatosDistribucionPorRegion As New System.Web.UI.WebControls.GridView()

            grillaDatosDistribucionPorRegion.AutoGenerateColumns = False
            grillaDatosDistribucionPorRegion.ID = "ControlgrillaDatos"
            Dim columna1 As New BoundField
            columna1.HeaderText = "Fabricante"
            columna1.DataField = "fabricante"
            Dim columna2 As New BoundField
            columna2.HeaderText = "Producto"
            columna2.DataField = "producto"
            Dim columna3 As New BoundField
            columna3.HeaderText = "Cantidad"
            columna3.DataField = "cantidad"
            columna3.ItemStyle.HorizontalAlign = HorizontalAlign.Center
            Dim columna4 As New BoundField
            columna4.HeaderText = "Valor Unitario"
            columna4.DataField = "valorUnitario"
            columna4.ItemStyle.HorizontalAlign = HorizontalAlign.Right
            columna4.DataFormatString = "{0:C}"

            Dim columna5 As New BoundField
            columna5.HeaderText = "Fecha de Registro"
            columna5.DataField = "fechaRegistro"
            Dim columna6 As New BoundField
            columna6.HeaderText = "Observación"
            columna6.DataField = "observacion"

            grillaDatosDistribucionPorRegion.Columns.Add(columna1)
            grillaDatosDistribucionPorRegion.Columns.Add(columna2)
            grillaDatosDistribucionPorRegion.Columns.Add(columna3)
            grillaDatosDistribucionPorRegion.Columns.Add(columna4)
            grillaDatosDistribucionPorRegion.Columns.Add(columna5)
            grillaDatosDistribucionPorRegion.Columns.Add(columna6)


            grillaDatosDistribucionPorRegion.DataSource = dtDetalle
            grillaDatosDistribucionPorRegion.DataBind()

            If dtDetalle.Rows.Count > 0 Then celda.CssClass = "DetalleOrden"
            celda.ColumnSpan = grdOrden.Columns.Count
            celda.Controls.Add(grillaDatosDistribucionPorRegion)
            Return celda
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar las ordenes. " & ex.Message)
        End Try

    End Function

    Protected Function ObtenerTipoProducto() As DataTable
        Dim filtroTipoProducto As New Estructuras.FiltroTipoProducto
        Dim dtTipoProducto As DataTable
        Try
            filtroTipoProducto.Activo = 1
            filtroTipoProducto.ExisteModulo = 1
            filtroTipoProducto.IdModulo = 1
            dtTipoProducto = ILSBusinessLayer.Productos.TipoProducto.ObtenerListado(filtroTipoProducto)
            If dtTipoProducto IsNot Nothing Then
                Dim pk() As DataColumn = {dtTipoProducto.Columns("idTipoProducto")}
                dtTipoProducto.PrimaryKey = pk
            End If
            Return dtTipoProducto
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los tipos de producto. " & ex.Message)
        End Try
    End Function

    Private Sub EnlazarTipoProducto(ByVal dtTipoProducto As DataTable)
        Try
            With ddlTipoProducto
                .DataSource = dtTipoProducto
                .DataTextField = "descripcion"
                .DataValueField = "idTipoProducto"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja el tipo de producto", 0))
            End With
            Session("dtTipoProducto") = dtTipoProducto
        Catch ex As Exception
            Throw New Exception("Error al tratar de enlazar tipos de producto. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerProveedor()
        Try
            With ddlProveedor
                .DataSource = MetodosComunes.getAllProveedores
                .DataTextField = "proveedor"
                .DataValueField = "idproveedor"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Proveedor", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener Proveedores. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerMoneda()
        Dim dt As New DataTable
        Try
            dt = Moneda.Obtener(1)
            With ddlMoneda
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idMoneda"
                .DataBind()
                If dt.Rows.Count > 1 Then
                    .Items.Insert(0, New ListItem("Escoja la Moneda", 0))
                End If

            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos para Moneda. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerInconterm()
        Dim dt As New DataTable
        Try
            dt = Incoterm.Obtener(1)
            With ddlIncoterm
                .DataSource = dt
                .DataTextField = "termino"
                .DataValueField = "idIncoterm"
                .DataBind()
                If dt.Rows.Count > 1 Then
                    .Items.Insert(0, New ListItem("Escoja el Incoterm", 0))
                End If
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos para Moneda. " & ex.Message)
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

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            ObtenerOrdenCompra()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar la consulta. " & ex.Message)
        End Try

    End Sub

    Protected Sub grdOrden_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdOrden.RowCommand
        Try
            Dim idOrdenCompra As Long
            If e.CommandName = "editarOrdenCompra" Then
                ObtenerOrdenCompra()
                hfIdOrdenEditar.Value = e.CommandArgument.ToString
                Dim ordenObj As New OrdenCompra(CLng(hfIdOrdenEditar.Value))
                CargarDatosEditarOrden(ordenObj)
                imgEditarOrdenCompra_ModalPopupExtender.Show()
            ElseIf e.CommandName = "AnularOrdenCompra" Then
                Long.TryParse(e.CommandArgument.ToString, idOrdenCompra)                                
                AnularOrden(idOrdenCompra)
                ObtenerOrdenCompra()
                EncabezadoPagina.showSuccess("Orden Anulada")            
            ElseIf e.CommandName = "ActivarOrdenCompra" Then
                ActivarOrden(CLng(e.CommandArgument))
                ObtenerOrdenCompra()
                EncabezadoPagina.showSuccess("Orden Activada")
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar la opcion indicada. " & ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Anular la orden de compra indicada
    ''' </summary>
    ''' <param name="idOrden">Id de la orden de compra a anular</param>
    ''' <remarks></remarks>
    Protected Sub AnularOrden(ByVal idOrden As Long)
        Try
            Dim ordenObj As New Recibos.OrdenCompra(idOrden)
            ordenObj.IdEstado = Recibos.OrdenCompra.EstadoOrden.Cancelada
            ordenObj.Actualizar()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al anular la orden indicada. " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Activa la orden de compra indicada
    ''' </summary>
    ''' <param name="idOrden">Id de la orden de compra a activar</param>
    ''' <remarks></remarks>
    Protected Sub ActivarOrden(ByVal idOrden As Long)
        Try
            Dim ordenObj As New Recibos.OrdenCompra(idOrden)
            ordenObj.IdEstado = Recibos.OrdenCompra.EstadoOrden.Abierta
            ordenObj.Actualizar()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al anular la orden indicada. " & ex.Message)
        End Try
    End Sub


    '*****************************************Carga componentes edicion de la orden**********************
    Private Sub CargarDatosEditarOrden(ByVal ordenObj As Recibos.OrdenCompra)
        Try
            ObtenerProveedorEditar()
            ObtenerMonedaEditar()
            ObtenerIncontermEditar()
            With ordenObj
                lblEditarOrdenNo.Text = .NumeroOrden.ToString
                ddlEditarProveedorOrden.SelectedValue = .IdProveedor
                ddlEditarMonedaOrden.SelectedValue = .IdMoneda
                ddlEditarIncotermOrden.SelectedValue = .IdIncoterm
                txtEditarObservacionOrden.Text = .Observacion.ToString
                trDistribucionRegional.Visible = IIf(.IdTipoProducto = 3, True, False)
                If trDistribucionRegional.Visible Then CargarRegiones()
            End With
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar los datos para editar el detalle. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerProveedorEditar()
        Try
            With ddlEditarProveedorOrden
                .DataSource = MetodosComunes.getAllProveedores
                .DataTextField = "proveedor"
                .DataValueField = "idproveedor"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Proveedor", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener Proveedores para editar. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerMonedaEditar()
        Dim dt As New DataTable
        Try
            dt = Moneda.Obtener(1)
            With ddlEditarMonedaOrden
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idMoneda"
                .DataBind()
                If dt.Rows.Count > 1 Then
                    .Items.Insert(0, New ListItem("Escoja la Moneda", 0))
                End If

            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos para editar Moneda. " & ex.Message)
        End Try
    End Sub
    Protected Sub ObtenerIncontermEditar()
        Dim dt As New DataTable
        Try
            dt = Incoterm.Obtener(1)
            With ddlEditarIncotermOrden
                .DataSource = dt
                .DataTextField = "termino"
                .DataValueField = "idIncoterm"
                .DataBind()
                If dt.Rows.Count > 1 Then
                    .Items.Insert(0, New ListItem("Escoja el Incoterm", 0))
                End If
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos para editar Moneda. " & ex.Message)
        End Try
    End Sub


    '*****************************************Fin Carga componentes edicion de la orden**********************

    Protected Sub btnEditarOrdenCompra_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarOrdenCompra.Click
        Dim ordenObj As New OrdenCompra(CLng(hfIdOrdenEditar.Value))
        With ordenObj
            .IdProveedor = ddlEditarProveedorOrden.SelectedValue
            .IdMoneda = ddlEditarMonedaOrden.SelectedValue
            .IdIncoterm = ddlEditarIncotermOrden.SelectedValue
            .Observacion = txtEditarObservacionOrden.Text
            If trDistribucionRegional.Visible Then
                Dim dtDistribucion As DataTable = ObtenerDistribucionPorRegion()
                .AdicionarDistribucionRegional(dtDistribucion)
            End If
            .Actualizar()
            EncabezadoPagina.showSuccess("Orden actualizada")
        End With
        ObtenerOrdenCompra()
        imgEditarOrdenCompra_ModalPopupExtender.Hide()
    End Sub

    Protected Sub imgBtnCerrarPopUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnCerrarPopUp.Click
        Try
            LimpiarDatosDetalleOrden()
            ObtenerOrdenCompra()
            imgEditarOrdenCompra_ModalPopupExtender.Hide()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cerrar el popup" & ex.Message)
        End Try
    End Sub

    Private Sub LimpiarDatosDetalleOrden()
        Try

        Catch ex As Exception
            EncabezadoPagina.showError("Error al limpiar el formulario" & ex.Message)
        End Try
    End Sub

    Private Sub CargarRegiones()
        Dim dtDatos As DataTable
        Try
            dtDatos = Region.ObtenerTodas()
            With gvRegion
                .DataSource = dtDatos
                .Columns(2).Visible = True
                .DataBind()
                .Columns(2).Visible = False
            End With
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de cargar el listado de Regiones. " & ex.Message)
        End Try
    End Sub

    Private Function ObtenerDistribucionPorRegion() As DataTable
        Dim dtDistribucion As New DataTable
        With dtDistribucion.Columns
            .Add("idRegion", GetType(Short))
            .Add("cantidad", GetType(Integer))
        End With
        Dim drDistribucion As DataRow
        Dim txt As TextBox
        For Each fila As GridViewRow In gvRegion.Rows
            txt = CType(fila.FindControl("txtCantidadRegion"), TextBox)
            If txt.Text.Trim.Length > 0 Then
                drDistribucion = dtDistribucion.NewRow
                drDistribucion("idRegion") = fila.Cells(2).Text
                drDistribucion("cantidad") = txt.Text.Trim
                txt.Text = String.Empty
                dtDistribucion.Rows.Add(drDistribucion)
            End If
        Next
        Return dtDistribucion
    End Function

    Protected Sub btnBorrarFiltros_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBorrarFiltros.Click
        LimpiarFiltros()
    End Sub

    Private Sub LimpiarFiltros()
        Try
            txtIdOrden.Text = ""
            txtNumeroOrden.Text = ""
            ddlTipoProducto.ClearSelection()
            ddlProveedor.ClearSelection()
            ddlMoneda.ClearSelection()
            ddlIncoterm.ClearSelection()
            ddlEstado.ClearSelection()
            txtFechaInicial.Text = ""
            txtFechaFinal.Text = ""
        Catch ex As Exception
            EncabezadoPagina.showError("Error al limpiar los filtros. " & ex.Message)
        End Try
    End Sub

    Protected Sub grdOrden_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles grdOrden.SelectedIndexChanged

    End Sub
End Class