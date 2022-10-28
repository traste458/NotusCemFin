Imports ILSBusinessLayer
Imports ILSBusinessLayer.Recibos

Partial Public Class CrearOrdenCompraMerchanPopInsumoP2
    Inherits System.Web.UI.Page

    Private TipoProductoObj As Productos.TipoProducto
    Private ordenesRecepcionSeleccionadas As ArrayList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epEncabezado.clear()
        If Not Me.IsPostBack Then
            Try                
                epEncabezado.setTitle("Crear Orden Compra")
                If Request.QueryString.Item("tp") IsNot Nothing And Session("remisionSeleccionada") IsNot Nothing Then
                    Me.ordenesRecepcionSeleccionadas = New ArrayList
                    Me.ordenesRecepcionSeleccionadas = CType(Session("remisionSeleccionada"), ArrayList)
                    hfIdTipoProducto.Value = Request.QueryString("tp").ToString
                    epEncabezado.setTitle("Crear Orden Compra")
                    Session.Remove("dtDatosDetalleOrdenCompraAdicional")
                    TipoProductoObj = New Productos.TipoProducto(CInt(hfIdTipoProducto.Value))
                    lblTipoProducto.Text = "Orden de compra para el tipo de producto " & TipoProductoObj.Descripcion
                    CargarMoneda()
                    CargarInconterm()
                    CargarRemisiones()
                    CargarRegiones()
                    CargarRemisionesSeleccionadas()                                        
                Else
                    contenedor.Visible = False
                    epEncabezado.showError("Error al cargar la pagina, por favor actualizar")
                End If

                If Request.UrlReferrer IsNot Nothing Then
                    epEncabezado.showReturnLink(Request.UrlReferrer.ToString)
                Else
                    epEncabezado.showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                End If
            Catch ex As Exception
                epEncabezado.showError("Error al cargar la pagina " & ex.Message)
            End Try
        End If
    End Sub

    Protected Sub CargarMoneda()
        Dim dt As New DataTable
        Try
            dt = Moneda.Obtener(1)
            With ddlMoneda
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idMoneda"
                .DataBind()
                If dt.Rows.Count <> 1 Then .Items.Insert(0, New ListItem("Escoja una Moneda", 0))
            End With
        Catch ex As Exception
            epEncabezado.showError("Error al tratar de obtener el listado de Monedas. " & ex.Message)
        End Try
    End Sub

    Protected Sub CargarInconterm()
        Dim dt As New DataTable
        Try
            dt = Incoterm.Obtener(1)
            With ddlIncoterm
                .DataSource = dt
                .DataTextField = "termino"
                .DataValueField = "idIncoterm"
                .DataBind()
                If dt.Rows.Count <> 1 Then .Items.Insert(0, New ListItem("Escoja un Incoterm", 0))
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
            gvCargaRecepcion.DataSource = Recibos.OrdenRecepcion.ObtenerListadoProducto(Me.ordenesRecepcionSeleccionadas)
            gvCargaRecepcion.DataBind()
            Dim dtProductoAdicional As New DataTable
            dtProductoAdicional = Recibos.OrdenRecepcion.ObtenerListadoProducto(Me.ordenesRecepcionSeleccionadas, TipoDetalleOrdenCompra.TipoDetalle.Secundario)
            If dtProductoAdicional.Rows.Count > 0 Then
                gvProductoAdicional.DataSource = dtProductoAdicional
                gvProductoAdicional.DataBind()
                'pnlProductoAdicional.Visible = True
                pnlProductoAdicional.Visible = False
            Else
                pnlProductoAdicional.Visible = False
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar las remisiones. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarRemisiones()
        Try            
            Dim dt As New DataTable
            Dim filtro As New Estructuras.FiltroOrdenRecepcion
            filtro.ListaIdOrdenesRecepcion = Me.ordenesRecepcionSeleccionadas
            dt = Recibos.OrdenRecepcion.ObtenerListado(filtro)
            hfIdProveedor.Value = dt.Rows("0")("idProveedor").ToString
            gvRemisiones.DataSource = dt
            gvRemisiones.DataBind()            
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
                filtroPallet.IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Principal
                filtroPallet.IdEstado = 57
                Dim ProductosAgregados As BulletedList = CType(e.Row.FindControl("bltProductosAgregados"), BulletedList)
                Dim Pallets As BulletedList = CType(e.Row.FindControl("bltPallet"), BulletedList)
                Dim camposGrupo As New ArrayList(1)
                camposGrupo.Add("nombreProducto")
                Pallets.DataSource = Recibos.PalletRecepcion.ObtenerListado(filtroPallet)
                Pallets.DataBind()
                ProductosAgregados.DataSource = MetodosComunes.getDistinctsFromDataTable(Recibos.PalletRecepcion.ObtenerInfoDetalle(CLng(idOrdenRecepcion), 1), camposGrupo)
                ProductosAgregados.DataBind()
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar los productos y las cantidades de las recepciones. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnEnviar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnviar.Click

        Dim ordenCompraObj As New OrdenCompra()
        Dim dtDetalleOrden As New DataTable()

        Dim ddlFabricanteObj As New DropDownList
        Dim txtCantidadObj As New TextBox
        Dim txtValorUnitarioObj As New TextBox
        Dim txtObservacionObj As New TextBox        
        Dim hfIdProductoObj As New HiddenField
        Dim hfIdTipoUnidadObj As New HiddenField

        Try
            'If Not Recibos.OrdenCompra.ExisteNumeroOrden(txtNumeroOrden.Text) Then

            For Each row As GridViewRow In gvCargaRecepcion.Rows
                ddlFabricanteObj = CType(row.FindControl("ddlFabricante"), DropDownList)
                txtCantidadObj = CType(row.FindControl("txtCantidad"), TextBox)
                txtValorUnitarioObj = CType(row.FindControl("txtValorUnitario"), TextBox)
                txtObservacionObj = CType(row.FindControl("txtObservacion"), TextBox)
                hfIdProductoObj = CType(row.FindControl("hfIdProducto"), HiddenField)
                hfIdTipoUnidadObj = CType(row.FindControl("hfIdTipoUnidad"), HiddenField)

                With ordenCompraObj
                    .AdicionarDetalle(CInt(ddlFabricanteObj.SelectedValue), ddlFabricanteObj.SelectedItem.Text, hfIdProductoObj.Value, row.Cells(1).Text, hfIdTipoUnidadObj.Value, row.Cells(2).Text, CInt(txtCantidadObj.Text.Trim()), CType(txtValorUnitarioObj.Text.Trim(), Decimal), txtObservacionObj.Text.Trim())
                End With

            Next

            'Se modifica o agrega el producto adicional
            For Each row As GridViewRow In gvProductoAdicional.Rows
                Dim idProducto As Long
                idProducto = CType(row.FindControl("hfIdProductoAdicional"), HiddenField).Value
                Dim productoObj As New Productos.Producto(CInt(idProducto))

                txtCantidadObj = CType(row.FindControl("txtCantidadAdicional"), TextBox)
                With ordenCompraObj
                    .AdicionarDetalle(productoObj.IdFabricante, productoObj.Fabricante, idProducto, productoObj.Nombre, productoObj.IdTipoUnidad, productoObj.UnidadEmpaque, CInt(txtCantidadObj.Text.Trim()), 0, "")
                End With
            Next

            If gvRegion.Visible Then
                Dim idRegion As Integer
                Dim cantidad As Integer
                For Each row As GridViewRow In gvRegion.Rows
                    idRegion = CInt(row.Cells(2).Text)
                    cantidad = CInt(CType(row.FindControl("txtCantidad"), TextBox).Text.Trim())
                    ordenCompraObj.AdicionarDistribucionRegional(idRegion, cantidad)
                Next
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
                .ProductoRecibido = True
            End With
            If ordenCompraObj.Crear(CType(Session("remisionSeleccionada"), ArrayList)) Then
                epEncabezado.showSuccess("Orden de compra No. <span style='color:red;'>" & txtNumeroOrden.Text & "</span> con el identificador <span style='color:red;'>" & ordenCompraObj.IdOrden.ToString() & "</span> fue creada satisfactoriamente.")
                btnEnviar.Visible = False
                If Session("remisionSeleccionada") IsNot Nothing Then Session.Remove("remisionSeleccionada")
            Else
                epEncabezado.showError("Error al crear la orden de compra")
            End If
            'Else
            '    epEncabezado.showError("Ya existe una orden de compra con el número especificado. Por favor verifique.")
            'End If
        Catch ex As Exception
            epEncabezado.showError("Error al crear la orden de compra " & ex.Message)
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

                With ddlFabricante
                    .DataTextField = "nombre"
                    .DataValueField = "idFabricante"
                    .DataSource = dtFabricante
                    .DataBind()
                    If dtFabricante.Rows.Count > 1 Then .Items.Insert(0, New ListItem("Escoja un Fabricante", 0))
                End With
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar los productos de la recepción. " & ex.Message)
        End Try
    End Sub

    Private Sub InicializaDropDownList(ByRef control As DropDownList, ByVal opcionInicial As String)
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem(opcionInicial, "0"))
    End Sub

End Class