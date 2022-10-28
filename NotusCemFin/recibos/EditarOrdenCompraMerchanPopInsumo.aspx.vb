Imports ILSBusinessLayer
Imports ILSBusinessLayer.Recibos

Partial Public Class EditarOrdenCompraMerchanPopInsumo
    Inherits System.Web.UI.Page

    Private TipoProductoObj As Productos.TipoProducto
    Private ordenesRecepcionSeleccionadas As ArrayList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epEncabezado.clear()

        Try
            If Not Me.IsPostBack Then
                If Request.QueryString.Item("ido") IsNot Nothing Then
                    hfIdOrdenCompra.Value = Request.QueryString("ido").ToString
                    Dim OrdenCompraObj As New Recibos.OrdenCompra(CLng(hfIdOrdenCompra.Value))
                    hfIdProveedor.Value = OrdenCompraObj.IdProveedor.ToString
                    epEncabezado.setTitle("Editar Orden Compra No. " & OrdenCompraObj.NumeroOrden)
                    Session.Remove("dtDatosOrdenCompraProductoAdicional")
                    hfIdTipoProducto.Value = OrdenCompraObj.IdTipoProducto.ToString
                    TipoProductoObj = New Productos.TipoProducto(CInt(hfIdTipoProducto.Value))
                    lblTipoProducto.Text = "Orden de compra para el tipo de producto " & TipoProductoObj.Descripcion
                    CargarDatos()
                Else
                    contenedor.Visible = False
                    epEncabezado.showError("Error al cargar la pagina, por favor actualizar")
                End If
                epEncabezado.showReturnLink("~/recibos/BuscarOrdenCompra.aspx")
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar la pagina " & ex.Message)
        End Try

    End Sub

    Protected Sub CargarDatos()
        Try
            Dim OrdenCompraObj As New Recibos.OrdenCompra(CLng(hfIdOrdenCompra.Value))
            txtNumeroOrden.Text = OrdenCompraObj.NumeroOrden
            txtObservacion.Text = OrdenCompraObj.Observacion
            CargarMoneda(OrdenCompraObj.IdMoneda)
            CargarInconterm(OrdenCompraObj.IdIncoterm)
            CargarRemisiones()
            CargarRegiones()
            CargarRemisionesSeleccionadas()
            VerificarRemisionesPendientes()
        Catch ex As Exception
            epEncabezado.showError("Error al cargar los datos de la orden. " & ex.Message)
        End Try

    End Sub

    Private Sub VerificarRemisionesPendientes()
        Try
            Dim filtroRecepcion As Estructuras.FiltroOrdenRecepcion
            Dim dtRecepcion As New DataTable
            Dim estados As New ArrayList
            estados.Add(Recibos.OrdenRecepcion.EstadoOrden.Finalizada)
            filtroRecepcion.ListaEstado = estados
            filtroRecepcion.IdTipoProducto = CInt(hfIdTipoProducto.Value)
            If hfIdProveedor.Value <> "" Then
                filtroRecepcion.IdProveedor = CInt(hfIdProveedor.Value)
            End If

            filtroRecepcion.IdOrdenCompra = -1
            dtRecepcion = Recibos.OrdenRecepcion.ObtenerListado(filtroRecepcion)
            IbtnAsociarRecepcion.Visible = CBool(dtRecepcion.Rows.Count)
        Catch ex As Exception
            Throw New Exception("Error al verificar las remisiones pendientess." & ex.Message)
        End Try
    End Sub

    Protected Sub CargarMoneda(ByVal idMoneda As Integer)
        Dim dt As New DataTable
        Try
            dt = Moneda.Obtener(1)
            With ddlMoneda
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idMoneda"
                .DataBind()
                If dt.Rows.Count <> 1 Then .Items.Insert(0, New ListItem("Escoja una Moneda", 0))
                If .Items.IndexOf(.Items.FindByValue(idMoneda)) <> -1 Then
                    .SelectedValue = idMoneda
                End If
                
            End With
        Catch ex As Exception
            epEncabezado.showError("Error al tratar de obtener el listado de Monedas. " & ex.Message)
        End Try
    End Sub

    Protected Sub CargarInconterm(ByVal idIncoterm As Integer)
        Dim dt As New DataTable
        Try
            dt = Incoterm.Obtener(1)
            With ddlIncoterm
                .DataSource = dt
                .DataTextField = "termino"
                .DataValueField = "idIncoterm"
                .DataBind()
                If dt.Rows.Count <> 1 Then .Items.Insert(0, New ListItem("Escoja un Incoterm", 0))
                If .Items.IndexOf(.Items.FindByValue(idIncoterm)) <> -1 Then
                    .SelectedValue = idIncoterm
                End If
            End With
        Catch ex As Exception
            epEncabezado.showError("Error al tratar de obtener el listado de Incoterms. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarRegiones()
        If CInt(hfIdTipoProducto.Value) = Productos.TipoProducto.Tipo.MERCHANDISING Then
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
                epEncabezado.showError("Error al tratar de cargar el listado de Regiones. " & ex.Message)
            End Try
            gvRegion.Visible = True
        Else
            gvRegion.Visible = False
        End If
    End Sub

    Private Sub CargarRemisionesSeleccionadas()
        Try
            gvCargaRecepcion.DataSource = Recibos.OrdenRecepcion.ObtenerListadoDeOrdenCompra(CLng(hfIdOrdenCompra.Value))
            gvCargaRecepcion.DataBind()
            Dim dtProductoAdicional As New DataTable
            dtProductoAdicional = Recibos.OrdenRecepcion.ObtenerListadoDeOrdenCompra(CLng(hfIdOrdenCompra.Value), TipoDetalleOrdenCompra.TipoDetalle.Secundario)
            If dtProductoAdicional.Rows.Count > 0 Then
                gvCargaRecepcionAdicional.DataSource = dtProductoAdicional
                gvCargaRecepcionAdicional.DataBind()
                pnlProductoAdicionalContenedor.Visible = True
            Else
                pnlProductoAdicionalContenedor.Visible = False
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar las remisiones. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarRemisiones()
        Try
            Dim dt As New DataTable
            Dim filtro As New Estructuras.FiltroOrdenRecepcion
            filtro.IdOrdenCompra = CInt(hfIdOrdenCompra.Value)            
            dt = Recibos.OrdenRecepcion.ObtenerListado(filtro)
            If dt.Rows.Count > 0 Then
                hfIdProveedor.Value = dt.Rows("0")("idProveedor").ToString
                gvRemisiones.DataSource = dt
                gvRemisiones.DataBind()
            End If
            
        Catch ex As Exception
            epEncabezado.showError("Error al cargar las remisiones. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvRemisiones_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRemisiones.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim idOrdenRecepcion As Integer = CInt(fila("idOrdenRecepcion"))
                Dim filtroPallet As New Estructuras.FiltroPalletRecepcion
                filtroPallet.IdOrdenRecepcion = idOrdenRecepcion
                filtroPallet.IdEstado = 57
                Dim ProductosAgregados As BulletedList = CType(e.Row.FindControl("bltProductosAgregados"), BulletedList)
                Dim Pallets As BulletedList = CType(e.Row.FindControl("bltPallet"), BulletedList)
                Dim camposGrupo As New ArrayList(1)
                camposGrupo.Add("nombreProducto")
                Pallets.DataSource = Recibos.PalletRecepcion.ObtenerListado(filtroPallet)
                Pallets.DataBind()
                ProductosAgregados.DataSource = MetodosComunes.getDistinctsFromDataTable(Recibos.PalletRecepcion.ObtenerInfoDetalle(CLng(idOrdenRecepcion)), camposGrupo)
                ProductosAgregados.DataBind()
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar los productos y las cantidades de las recepciones. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnEnviar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnviar.Click

        Dim ordenCompraObj As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
        Dim dtDetalleOrden As New DataTable()

        Dim ddlFabricanteObj As New DropDownList
        Dim txtCantidadObj As New TextBox
        Dim txtValorUnitarioObj As New TextBox
        Dim txtObservacionObj As New TextBox
        Dim hfIdProductoObj As New HiddenField
        Dim hfIdTipoUnidadObj As New HiddenField

        Try            

            For Each row As GridViewRow In gvCargaRecepcion.Rows
                ddlFabricanteObj = CType(row.FindControl("ddlFabricante"), DropDownList)
                txtCantidadObj = CType(row.FindControl("txtCantidad"), TextBox)
                txtValorUnitarioObj = CType(row.FindControl("txtValorUnitario"), TextBox)
                txtObservacionObj = CType(row.FindControl("txtObservacion"), TextBox)
                hfIdProductoObj = CType(row.FindControl("hfIdProducto"), HiddenField)
                hfIdTipoUnidadObj = CType(row.FindControl("hfIdTipoUnidad"), HiddenField)
                Dim filtroDetalleOrdenCompraObj As New Estructuras.FiltroDetalleOrdenCompra
                filtroDetalleOrdenCompraObj.IdProducto = CInt(hfIdProductoObj.Value)
                filtroDetalleOrdenCompraObj.IdOrden = CInt(hfIdOrdenCompra.Value)
                dtDetalleOrden = Recibos.DetalleOrdenCompra.ObtenerListado(filtroDetalleOrdenCompraObj)
                With ordenCompraObj
                    If dtDetalleOrden.Rows.Count > 0 Then
                        .ModificarDetalle(CInt(ddlFabricanteObj.SelectedValue), ddlFabricanteObj.SelectedItem.Text, hfIdProductoObj.Value, row.Cells(1).Text, hfIdTipoUnidadObj.Value, row.Cells(2).Text, CInt(txtCantidadObj.Text.Trim()), CType(txtValorUnitarioObj.Text.Trim(), Decimal), txtObservacionObj.Text.Trim())
                    Else
                        .AdicionarDetalle(CInt(ddlFabricanteObj.SelectedValue), ddlFabricanteObj.SelectedItem.Text, hfIdProductoObj.Value, row.Cells(1).Text, hfIdTipoUnidadObj.Value, row.Cells(2).Text, CInt(txtCantidadObj.Text.Trim()), CType(txtValorUnitarioObj.Text.Trim(), Decimal), txtObservacionObj.Text.Trim())
                    End If
                End With
            Next

            'Se modifica o agrega el producto adicional
            For Each row As GridViewRow In gvCargaRecepcionAdicional.Rows
                Dim idProducto As Long                
                idProducto = CType(row.FindControl("hfIdProductoAdicional"), HiddenField).Value
                Dim productoObj As New Productos.Producto(CInt(idProducto))

                txtCantidadObj = CType(row.FindControl("txtCantidadAdicional"), TextBox)
                Dim filtroDetalleOrdenCompraObj As New Estructuras.FiltroDetalleOrdenCompra
                filtroDetalleOrdenCompraObj.IdProducto = CInt(hfIdProductoObj.Value)
                filtroDetalleOrdenCompraObj.IdOrden = CInt(hfIdOrdenCompra.Value)
                filtroDetalleOrdenCompraObj.IdTipoDetalle = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                dtDetalleOrden = Recibos.DetalleOrdenCompra.ObtenerListado(filtroDetalleOrdenCompraObj)
                With ordenCompraObj
                    If dtDetalleOrden.Rows.Count > 0 Then
                        .ModificarDetalle(productoObj.IdFabricante, productoObj.Fabricante, idProducto, productoObj.Nombre, productoObj.IdTipoUnidad, productoObj.UnidadEmpaque, CInt(txtCantidadObj.Text.Trim()), 0, "")
                    Else
                        .AdicionarDetalle(productoObj.IdFabricante, productoObj.Fabricante, idProducto, productoObj.Nombre, productoObj.IdTipoUnidad, productoObj.UnidadEmpaque, CInt(txtCantidadObj.Text.Trim()), 0, "")
                    End If
                End With
            Next

            If gvRegion.Visible Then                
                Dim dtRegion As New DataTable
                Dim filaRegion As DataRow
                dtRegion = EstructuraDistribucionRegional()

                For Each row As GridViewRow In gvRegion.Rows
                    filaRegion = dtRegion.NewRow
                    filaRegion("idRegion") = CInt(row.Cells(2).Text)
                    filaRegion("cantidad") = CInt(CType(row.FindControl("txtCantidad"), TextBox).Text.Trim())
                    dtRegion.Rows.Add(filaRegion)
                Next
                ordenCompraObj.ModificarDistribucionRegional(dtRegion)
            End If

            With ordenCompraObj
                .NumeroOrden = txtNumeroOrden.Text.Trim()
                .IdTipoProducto = CInt(hfIdTipoProducto.Value)
                .IdProveedor = CInt(hfIdProveedor.Value)
                .IdMoneda = ddlMoneda.SelectedValue
                .IdIncoterm = ddlIncoterm.SelectedValue
                .IdEstado = OrdenCompra.EstadoOrden.Abierta
                .IdCreador = CLng(Session("usxp001"))
                .Observacion = txtObservacion.Text.Trim()
            End With

            ordenCompraObj.Actualizar()
            CargarDatos()
            epEncabezado.showSuccess("Orden de compra No. " & txtNumeroOrden.Text & " actualizada correctamente.")            

        Catch ex As Exception
            epEncabezado.showError("Error al modificar la orden de compra " & ex.Message)
        End Try
    End Sub

    Private Sub CargarDistribucionRegional(ByVal dtDistribucion As DataTable)
        Dim dtAux As New DataTable
        Dim pkKeys() As DataColumn = {dtDistribucion.Columns("region")}
        Dim drDistribucion As DataRow
        dtDistribucion.PrimaryKey = pkKeys
        For Each drDistribucion In dtDistribucion.Rows
            dtAux.Columns.Add(drDistribucion("region").ToString, GetType(Integer))
        Next
        Dim drAux As DataRow = dtAux.NewRow
        For Each dcAux As DataColumn In dtAux.Columns
            drDistribucion = dtDistribucion.Rows.Find(dcAux.ColumnName)
            drAux(dcAux.ColumnName) = drDistribucion("cantidad")
        Next
        dtAux.Rows.Add(drAux)

        'With gvDistribucionRegion
        '    .DataSource = dtAux
        '    .DataBind()
        'End With
    End Sub


    Protected Sub gvCargaRecepcion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCargaRecepcion.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim idProducto As Integer = CInt(fila("idProducto"))
                Dim idTipoUnidad As Integer = CInt(fila("idTipoUnidad"))
                Dim productoOjb As New Productos.Producto(idProducto)
                Dim ddlFabricante As DropDownList = CType(e.Row.FindControl("ddlFabricante"), DropDownList)


                CType(e.Row.FindControl("lblCantidadRegistrada"), Label).Text = "Cantidad registrada " & fila("cantidad").ToString
                CType(e.Row.FindControl("hfIdProducto"), HiddenField).Value = idProducto.ToString
                CType(e.Row.FindControl("hfIdTipoUnidad"), HiddenField).Value = idTipoUnidad.ToString




                Dim filtroFabricante As New Estructuras.FiltroFabricante
                Dim dtFabricante As New DataTable
                filtroFabricante.IdTipoProducto = productoOjb.IdTipoProducto
                dtFabricante = Fabricante.ObtenerListado(filtroFabricante)
                'Carga de datos del detalle de la orden de compra
                Dim detalleOrdenCompraObj As New Recibos.DetalleOrdenCompra(CLng(hfIdOrdenCompra.Value), idProducto)
                CType(e.Row.FindControl("txtCantidad"), TextBox).Text = IIf(detalleOrdenCompraObj.Cantidad > 0, detalleOrdenCompraObj.Cantidad.ToString, String.Empty)
                CType(e.Row.FindControl("txtValorUnitario"), TextBox).Text = IIf(detalleOrdenCompraObj.ValorUnitario > 0, detalleOrdenCompraObj.ValorUnitario.ToString, String.Empty)
                If detalleOrdenCompraObj.Observacion is Nothing then
                    CType(e.Row.FindControl("txtObservacion"), TextBox).Text = String.Empty
                Else
                    CType(e.Row.FindControl("txtObservacion"), TextBox).Text = detalleOrdenCompraObj.Observacion.ToString
                End if
                With ddlFabricante
                    .DataTextField = "nombre"
                    .DataValueField = "idFabricante"
                    .DataSource = dtFabricante
                    .DataBind()
                    If dtFabricante.Rows.Count > 1 Then .Items.Insert(0, New ListItem("Escoja un Fabricante", 0))
                    If .Items.IndexOf(.Items.FindByValue(detalleOrdenCompraObj.IdFabricante)) <> -1 Then
                        .SelectedValue = detalleOrdenCompraObj.IdFabricante
                    End If
                End With
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar los productos de la recepción. " & ex.Message)
        End Try
    End Sub


    Protected Sub gvCargaRecepcionAdicional_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCargaRecepcionAdicional.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
                CType(e.Row.FindControl("lblCantidadRegistradaAdicional"), Label).Text = "Cantidad registrada " & fila("cantidad").ToString
                Dim detalleOrdenCompraObj As New Recibos.DetalleOrdenCompra(CLng(hfIdOrdenCompra.Value), fila("idProducto"))
                CType(e.Row.FindControl("txtCantidadAdicional"), TextBox).Text = IIf(detalleOrdenCompraObj.Cantidad > 0, detalleOrdenCompraObj.Cantidad.ToString, String.Empty)
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar el producto adicional de la recepción. " & ex.Message)
        End Try
    End Sub

    Protected Sub IbtnAsociarRecepcion_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles IbtnAsociarRecepcion.Click
        Try
            Dim filtroRecepcion As Estructuras.FiltroOrdenRecepcion
            Dim dtRecepcion As New DataTable
            Dim estados As New ArrayList
            estados.Add(Recibos.OrdenRecepcion.EstadoOrden.Finalizada)            
            filtroRecepcion.ListaEstado = estados
            filtroRecepcion.IdTipoProducto = CInt(hfIdTipoProducto.Value)
            If hfIdProveedor.Value <> "" Then
                filtroRecepcion.IdProveedor = CInt(hfIdProveedor.Value)
            End If

            filtroRecepcion.IdOrdenCompra = -1
            dtRecepcion = Recibos.OrdenRecepcion.ObtenerListado(filtroRecepcion)

            gvAdicionarRecepcion.DataSource = dtRecepcion
            gvAdicionarRecepcion.DataBind()
            btnAgregarRecepcion.Visible = CType(IIf(dtRecepcion.Rows.Count > 0, True, False), Boolean)
            pnlMensajeRecibos.Visible = True
            mpeAgregarRecepcion.Show()
        Catch ex As Exception
            epEncabezado.showError("Error al mostrar las ordenes de recepción. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvAdicionarRecepcion_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAdicionarRecepcion.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim idOrdenRecepcion As Integer = CInt(fila("idOrdenRecepcion"))
                Dim ProductosAgregados As BulletedList = CType(e.Row.FindControl("bltProductosAgregados"), BulletedList)
                Dim camposGrupo As New ArrayList(1)
                camposGrupo.Add("nombreProducto")
                ProductosAgregados.DataSource = MetodosComunes.getDistinctsFromDataTable(Recibos.PalletRecepcion.ObtenerInfoDetalle(CLng(idOrdenRecepcion)), camposGrupo)
                ProductosAgregados.DataBind()
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar las ordenes de recepcion. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnAgregarRecepcion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregarRecepcion.Click
        Dim remisionArray As New ArrayList
        Dim chk As CheckBox
        Dim idOrdenRecepcion As Long
        Try
            For Each row As GridViewRow In gvAdicionarRecepcion.Rows
                chk = CType(row.FindControl("chkRemision"), CheckBox)
                idOrdenRecepcion = CLng(row.Cells(2).Text)
                If chk.Checked Then
                    remisionArray.Add(idOrdenRecepcion)
                    chk.Checked = False
                End If
            Next

            If remisionArray.Count > 0 Then
                AsociarRecepcionAOrdenCompra(remisionArray)
            End If
            CargarRemisionesSeleccionadas()
            CargarRemisiones()
            epEncabezado.showSuccess("Remisiones agregadas a la orden de compra.")
            mpeAgregarRecepcion.Hide()
        Catch ex As Exception
            epEncabezado.showError("Error  al enviar las ordenes seleccionadas " & ex.Message)
        End Try
    End Sub

    Protected Sub LimpiarOrdenesRecepcionSeleccionadas()
        Try
            Dim chk As New CheckBox
            For Each row As GridViewRow In gvAdicionarRecepcion.Rows
                chk = CType(row.FindControl("chkRemision"), CheckBox)
                If chk.Checked Then
                    chk.Checked = False
                End If
            Next
        Catch ex As Exception
            epEncabezado.showError("Error al limpiar las ordenes de recepción. " & ex.Message)
        End Try
    End Sub




    Protected Sub imgBtnCerrarPopUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnCerrarPopUp.Click
        Try            
            mpeAgregarRecepcion.Hide()
            LimpiarOrdenesRecepcionSeleccionadas()
        Catch ex As Exception
            epEncabezado.showError("Error la cerrar el popup. " & ex.Message)
        End Try
    End Sub

    
    Protected Sub gvRemisiones_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRemisiones.RowCommand
        Try
            If e.CommandName = "DesvincularRemision" Then
                DesvincularRemision(CLng(e.CommandArgument))
                CargarDatos()
                epEncabezado.showSuccess("Desvinculación de ordenes de recepción correcta.")
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al desvincular la recepcion. " & ex.Message)
        End Try
    End Sub

    Protected Sub AsociarRecepcionAOrdenCompra(ByVal Remisiones As ArrayList)
        Dim ordenCompraObj As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
        Try
            ordenCompraObj.AsociarOrdenesRecepcion(Remisiones)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)            
        End Try
    End Sub

    Protected Sub DesvincularRemision(ByVal idOrdenRecepcion As Long)
        Dim ordenCompraObj As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
        Try
            ordenCompraObj.DesAsociarOrdenesRecepcion(idOrdenRecepcion)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)            
        End Try
    End Sub

    Protected Function EstructuraDistribucionRegional() As DataTable
        Try
            Dim dtDistribucionRegion As New DataTable("DistribucionRegional")
            Dim idRegion As New DataColumn
            With idRegion
                .DataType = System.Type.GetType("System.Int32")
                .ColumnName = "idRegion"
                .Unique = True
            End With
            Dim cantidad As New DataColumn
            With cantidad
                .DataType = System.Type.GetType("System.Int32")
                .ColumnName = "region"
            End With
            dtDistribucionRegion.Columns.Add(idRegion)
            dtDistribucionRegion.Columns.Add(cantidad)
            Return dtDistribucionRegion
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Sub inicializaDropDownList(ByRef control As DropDownList, Optional ByVal mensaje As String = "Seleccione...")
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem(mensaje, 0))
    End Sub


    Private Sub CargarDetallesOrdenCompra(Optional ByVal TipoDetalle As Integer = TipoDetalleOrdenCompra.TipoDetalle.Principal)
        Try
            Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim filtro As Estructuras.FiltroDetalleOrdenCompra
            Dim dt As New DataTable
            filtro.IdOrden = CInt(ordenCompra.IdOrden)
            filtro.IdTipoDetalle = TipoDetalle
            dt = Recibos.DetalleOrdenCompra.ObtenerListado(filtro)

            If TipoDetalle = TipoDetalleOrdenCompra.TipoDetalle.Secundario Then
                Session("dtDatosOrdenCompraProductoAdicional") = dt
                Dim totalDetalleAdicional As Integer
                Integer.TryParse(dt.Compute("SUM(cantidad)", "").ToString, totalDetalleAdicional)
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los detalle agregados a la orden de compra. " & ex.Message)
        End Try
    End Sub

    Protected Function EstructuraProductoAdicional() As DataTable
        Try
            Dim dtDatos As New DataTable
            If Session("dtDatosOrdenCompraProductoAdicional") Is Nothing Then
                Dim dcAux As New DataColumn("idDetalleOrden", GetType(Integer))
                dcAux.AutoIncrement = True
                dcAux.AutoIncrementSeed = 1
                With dtDatos.Columns
                    .Add(dcAux)
                    .Add("fabricante", GetType(String))
                    .Add("idFabricante", GetType(String))
                    .Add("producto", GetType(String))
                    .Add("idProducto", GetType(String))
                    .Add("tipoUnidad", GetType(String))
                    .Add("idTipoUnidad", GetType(Short))
                    .Add("cantidad", GetType(Integer))
                    .Add("valorUnitario", GetType(Decimal)).DefaultValue = 0
                    .Add("observacion", GetType(String)).DefaultValue = String.Empty
                    .Add("idTipoDetalle", GetType(Short)).DefaultValue = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                End With
                Dim pkKeys() As DataColumn = {dcAux}
                dtDatos.PrimaryKey = pkKeys

            Else
                dtDatos = CType(Session("dtDatosOrdenCompraProductoAdicional"), DataTable)
            End If
            Return dtDatos
        Catch ex As Exception
            epEncabezado.showError("Error la obtener el producto adicional. " & ex.Message)
        End Try
    End Function


End Class

