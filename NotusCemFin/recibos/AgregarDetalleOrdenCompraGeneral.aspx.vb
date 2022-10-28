Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer

Partial Public Class AgregarDetalleOrdenCompraGeneral
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)

            EncabezadoPagina.clear()
            EncabezadoPaginaAgregarDetalle.clear()
            If Not IsPostBack Then
                hfIdOrdenCompra.Value = Request.QueryString("ido").ToString
                Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
                Dim tipoProducto As Integer = CInt(ordenCompra.IdTipoProducto)
                Session.Remove("dtDatosDetalleOrdenCompra")
                Session.Remove("dtDatosOrdenCompraProductoAdicional")
                EncabezadoPagina.setTitle("Administrar Detalle de Orden de Compra")
                If ordenCompra.IdEstado <> 16 Then
                    pnlAgregarDetalleOrdenCompra.Visible = False
                    gvDetalleOrdenCompra.Columns(6).Visible = False
                End If
                EncabezadoPagina.showReturnLink("~/recibos/BuscarOrdenCompra.aspx")

                'Carga inicial para combos de detalle de la orden
                CargarInfoOrden()
                ObtenerFabricante(tipoProducto)
                inicializaDropDownList(ddlProducto)
                'ObtenerProducto(tipoProducto)
                ObtenerTipoUnidad()
                CargarDetallesOrdenCompra()
                ObtenerTipoProductoAdicional()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerTipoProductoAdicional()
        Try
            Dim ordenCompraObj As New Recibos.OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim idEstadoOrden As Integer = ordenCompraObj.IdEstado
            Dim filtroComboProducto As New Estructuras.FiltroCombinacionTipoProducto
            Dim dtTipoProducto As New DataTable
            filtroComboProducto.IdTipoPrimario = CShort(ordenCompraObj.IdTipoProducto)
            dtTipoProducto = CombinacionTipoProducto.ObtenerListado(filtroComboProducto)
            With ddlTipoProductoAdicional
                .DataSource = dtTipoProducto
                .DataTextField = "TipoProductoAdicional"
                .DataValueField = "idTipoProductoSecundario"
                .DataBind()
            End With
            trProductoAdicional.Visible = IIf(dtTipoProducto.Rows.Count > 0 AndAlso _
                                              (idEstadoOrden = Recibos.OrdenCompra.EstadoOrden.Abierta Or idEstadoOrden = Recibos.OrdenCompra.EstadoOrden.Parcial) _
                                               , True, False)
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el tipo de producto adicional. " & ex.Message)
        End Try
        ddlTipoProductoAdicional.Items.Insert(0, New ListItem("Escoja Tipo Producto", 0))
    End Sub

    Private Sub CargarDetallesOrdenCompra(Optional ByVal TipoDetalle As Integer = TipoDetalleOrdenCompra.TipoDetalle.Principal)
        Try
            Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim filtro As Estructuras.FiltroDetalleOrdenCompra
            Dim dt As New DataTable
            filtro.IdOrden = CInt(ordenCompra.IdOrden)
            filtro.IdTipoDetalle = TipoDetalle            

            dt = Recibos.DetalleOrdenCompra.ObtenerListado(filtro)

            If TipoDetalle = TipoDetalleOrdenCompra.TipoDetalle.Principal Then
                dt.Columns("idDetalle").AutoIncrement = True
                dt.Columns("idDetalle").AutoIncrementStep = 1
                Dim idMax As Integer
                Integer.TryParse(dt.Compute("MAX(idDetalle)", "").ToString, idMax)
                dt.Columns("idDetalle").AutoIncrementSeed = idMax + 1
                dt.Columns("idOrden").DefaultValue = ordenCompra.IdOrden
                dt.Columns("numeroOrden").DefaultValue = ordenCompra.NumeroOrden.ToString
                Dim llavePrimaria(1) As DataColumn
                llavePrimaria(0) = dt.Columns("idDetalle")
                dt.PrimaryKey = llavePrimaria
                dt.AcceptChanges()
                Session("dtDatosDetalleOrdenCompra") = dt
                gvDetalleOrdenCompra.DataSource = dt
                gvDetalleOrdenCompra.DataBind()
                Dim totalOrden As Integer
                Integer.TryParse(dt.Compute("SUM(cantidad)", "").ToString, totalOrden)
                hfTotalOrdenCompra.Value = totalOrden.ToString
                'hfCantidadDistribucion.Value = totalOrden.ToString
            Else
                Session("dtDatosOrdenCompraProductoAdicional") = dt
                CargaGrillaProductoAdicional(dt)
                Dim totalDetalleAdicional As Integer
                Integer.TryParse(dt.Compute("SUM(cantidad)", "").ToString, totalDetalleAdicional)
                hfTotalProductoAdicional.Value = totalDetalleAdicional.ToString
            End If
            
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los detalle agregados a la orden de compra. " & ex.Message)
        End Try
    End Sub

    Protected Sub CargaGrillaProductoAdicional(ByVal dtDatos As DataTable)
        With gvProductoAdicional
            .DataSource = dtDatos
            .DataBind()
        End With
    End Sub

    Private Sub CargarInfoOrden()
        Try
            Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            CargarDatosEditarOrden(ordenCompra)
            With ordenCompra
                lblNumeroOrden.Text = .NumeroOrden
                lblProveedor.Text = .Proveedor
                lblMoneda.Text = .Moneda
                lblIncoterm.Text = .Incoterm
                lblObservacion.Text = .Observacion
                lblEstado.Text = .Estado
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar la informacion de la orden. " & ex.Message)
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
                If trDistribucionRegional.Visible Then CargarDistribucionRegional()
            End With
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar los datos para editar el detalle. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarDistribucionRegional()
        Try
            Dim objOrdenCompra As New Recibos.OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim dtDistribucionPorRegion As New DataTable
            dtDistribucionPorRegion = objOrdenCompra.DistribucionRegional()
            With gvRegion
                .DataSource = dtDistribucionPorRegion
                .Columns(2).Visible = True
                .DataBind()
                .Columns(2).Visible = False
            End With
            Dim totalDistribucion As Integer
            Integer.TryParse(dtDistribucionPorRegion.Compute("SUM(cantidad)", "").ToString, totalDistribucion)
            hfCantidadDistribucion.Value = totalDistribucion.ToString
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar la distribución por región. " & ex.Message)
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


    '************************************* Carga Combos detalle de la orden ********************
    Protected Sub ObtenerFabricante(ByVal tipoProducto As Integer)
        Try
            Dim filtro As Estructuras.FiltroFabricante
            filtro.IdTipoProducto = tipoProducto
            With ddlFabricante
                .DataSource = Fabricante.ObtenerListado(filtro)
                .DataTextField = "nombre"
                .DataValueField = "idFabricante"
                .DataBind()
                If .Items.Count > 0 Then .Items.Insert(0, New ListItem("Escoja el Fabricante", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar el tipo de producto " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerProducto(ByVal tipoProducto As Integer)
        Try
            Dim dt As New DataTable
            Dim filtro As Estructuras.FiltroProducto
            filtro.IdTipoProducto = tipoProducto
            If ddlFabricante.SelectedValue <> "" Then filtro.IdFabricante = CInt(ddlFabricante.SelectedValue)
            dt = Productos.Producto.ObtenerListado(filtro)
            With ddlProducto
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idProducto"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el Producto", 0))
            End With
            If ddlProducto.Items.Count > 0 Then
                lblCantidadProducto.Visible = True
                lblCantidadProducto.Text = "Total productos encontrados para este fabricante " & dt.Rows.Count
            Else
                lblCantidadProducto.Visible = False
                lblCantidadProducto.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar el producto " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerTipoUnidad(Optional ByVal idTipoUnidad As Integer = 0)
        Try
            Dim tipProducto As New Productos.TipoProducto(1)
            Dim dt As New DataTable

            ddlTipoUnidad.Enabled = True
            dt = Recibos.TipoUnidad.ObtenerListado
            With ddlTipoUnidad
                .DataSource = dt
                .DataTextField = "descripcion"
                .DataValueField = "idTipoUnidad"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el Tipo Unidad", 0))
                If idTipoUnidad <> 0 Then ddlTipoUnidad.SelectedValue = idTipoUnidad
                filaTipoUnidad.Visible = False
            End With
            If tipProducto.IdTipoUnidad <> 0 Then
                ddlTipoUnidad.SelectedValue = tipProducto.IdTipoUnidad.ToString
                ddlTipoUnidad.Enabled = False
                filaTipoUnidad.Visible = False
            End If

        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar el tipo de unidad " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlFabricante_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFabricante.SelectedIndexChanged
        Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
        ObtenerProducto(CInt(ordenCompra.IdTipoProducto))
        pnlAdicionarDetalleOrdenCompra.Attributes("style") = "display:block"
    End Sub

    Protected Sub CargarDtDetalleOrdenCompra()
        Dim dtDetalleOrden As New DataTable
        Dim drAux() As DataRow
        dtDetalleOrden = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
        drAux = dtDetalleOrden.Select("idProducto=" & ddlProducto.SelectedValue)
        If drAux.Length = 0 Then
            Dim dt As New DataTable
            Dim dr As DataRow
            dt = dtDetalleOrden
            dr = dt.NewRow
            dr("fabricante") = ddlFabricante.SelectedItem.ToString
            dr("idfabricante") = ddlFabricante.SelectedValue
            dr("producto") = ddlProducto.SelectedItem.ToString
            dr("idproducto") = ddlProducto.SelectedValue
            dr("TipoUnidad") = ddlTipoUnidad.SelectedItem.ToString
            dr("idtipounidad") = ddlTipoUnidad.SelectedValue
            dr("cantidad") = txtCantidad.Text
            dr("valorUnitario") = txtValorUnitario.Text
            dr("observacion") = txtObservacionDetalleOrdenCompra.Text
            dt.Rows.InsertAt(dr, 0)
            dt.AcceptChanges()
            dr.SetAdded()
            gvDetalleOrdenCompra.DataSource = dt
            gvDetalleOrdenCompra.DataBind()
            Dim totalOrden As Integer
            Integer.TryParse(dt.Compute("SUM(cantidad)", "").ToString, totalOrden)
            hfTotalOrdenCompra.Value = totalOrden.ToString            
            'dt.AcceptChanges()
            Session("dtDatosDetalleOrdenCompra") = dt
            LimpiarDatosDetalleOrden()
        Else
            EncabezadoPaginaAgregarDetalle.showWarning("El producto seleccionado ya hace parte del detalle de la orden. Por favor verifique")
            mpeAgregarDetalle.Show()
        End If

    End Sub

    Protected Function EstructuraDtDetalleOrdenCompra() As DataTable
        Dim dtDatos As DataTable
        If Session("dtDatosDetalleOrdenCompra") Is Nothing Then
            dtDatos = New DataTable
            Dim dcIdOrden As New DataColumn("idOrden", GetType(Integer))
            Dim dcNumeroOrden As New DataColumn("numeroOrden", GetType(String))
            dcNumeroOrden.DefaultValue = "pepe"
            dcIdOrden.DefaultValue = hfIdOrdenCompra.Value
            'Dim dc As New DataColumn("idDetalle")
            'dc.AutoIncrement = True
            'dc.AutoIncrementStep = 1
            'dtDatos.Columns.Add(dc)
            dtDatos.Columns.Add(dcIdOrden)
            dtDatos.Columns.Add(dcNumeroOrden)
            dtDatos.Columns.Add("fabricante")
            dtDatos.Columns.Add("idfabricante")
            dtDatos.Columns.Add("producto")
            dtDatos.Columns.Add("idproducto")
            dtDatos.Columns.Add("TipoUnidad")
            dtDatos.Columns.Add("idtipounidad")
            dtDatos.Columns.Add("cantidad")
            dtDatos.Columns.Add("valorUnitario")
            dtDatos.Columns.Add("observacion")
            'Dim keyColumn(1) As DataColumn
            'keyColumn(0) = dc
            'dtDatos.PrimaryKey = keyColumn



        Else
            dtDatos = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
        End If
        Return dtDatos
    End Function


    Private Sub LimpiarDatosDetalleOrden()
        ddlFabricante.SelectedIndex = 0
        inicializaDropDownList(ddlProducto)
        ddlTipoUnidad.SelectedIndex = 0
        txtCantidad.Text = String.Empty
        txtValorUnitario.Text = String.Empty
        txtObservacionDetalleOrdenCompra.Text = String.Empty
        hfIdDetalle.Value = String.Empty
    End Sub

    Protected Sub btnCrearDetalleOrden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrearDetalleOrden.Click
        Try
            'AgregarDetalle()
            CargarDtDetalleOrdenCompra()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al crear el detalle. " & ex.Message)
        End Try
    End Sub

    Private Sub AgregarDetalle()
        Try
            Dim dtDetalleOrden As New DataTable
            Dim drAux() As DataRow
            dtDetalleOrden = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
            drAux = dtDetalleOrden.Select("idProducto=" & ddlProducto.SelectedValue)
            If drAux.Length = 0 Then
                Dim DetOrdenCompra As New Recibos.DetalleOrdenCompra()
                With DetOrdenCompra
                    .IdOrden = CLng(hfIdOrdenCompra.Value)
                    .IdFabricante = CInt(ddlFabricante.SelectedValue)
                    .IdProducto = CLng(ddlProducto.SelectedValue)
                    .IdTipoUnidad = CInt(ddlTipoUnidad.SelectedValue)
                    .Cantidad = CInt(txtCantidad.Text)
                    .ValorUnitario = CLng(txtValorUnitario.Text)
                    .IdUsuario = CLng(Session("usxp001"))
                    If txtObservacionDetalleOrdenCompra.Text <> "" Then
                        .Observacion = txtObservacionDetalleOrdenCompra.Text
                    End If
                    If .Crear() Then
                        CargarDetallesOrdenCompra()
                        mpeAgregarDetalle.Hide()
                        EncabezadoPagina.showSuccess("Detalle Agregado con exito.")
                        LimpiarDatosDetalleOrden()
                    End If
                End With
            Else
                EncabezadoPaginaAgregarDetalle.showWarning("El producto seleccionado ya hace parte del detalle de la orden. Por favor verifique")
                mpeAgregarDetalle.Show()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al agregar el detalle. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvDetalleOrdenCompra_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetalleOrdenCompra.RowCommand
        If e.CommandName = "Editar" Then
            lblTituloAccion.Text = "Editar detalle de orden"
            CargarDatosEdicionDetalle(e.CommandArgument)
            mpeAgregarDetalle.Show()
        ElseIf e.CommandName = "Eliminar" Then
            Dim dt As New DataTable
            Dim dr As DataRow
            dt = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
            dt.Select("idDetalle = " & e.CommandArgument.ToString)(0).Delete()

            Dim filtroDetOrdenCompra As New Estructuras.FiltroDetalleOrdenCompra
            Dim dtBuscarDetalle As New DataTable
            filtroDetOrdenCompra.IdDetalle = CInt(e.CommandArgument)
            dtBuscarDetalle = Recibos.DetalleOrdenCompra.ObtenerListado(filtroDetOrdenCompra)
            dt.AcceptChanges()            

            Dim totalOrden As Integer
            Integer.TryParse(dt.Compute("SUM(cantidad)", "").ToString, totalOrden)
            hfTotalOrdenCompra.Value = totalOrden.ToString

            gvDetalleOrdenCompra.DataSource = dt
            gvDetalleOrdenCompra.DataBind()
            EncabezadoPagina.showSuccess("Detalle de la orden eliminado")
            'dt.AcceptChanges()
            Session("dtDatosDetalleOrdenCompra") = dt
        End If
    End Sub

    Private Sub CargarDatosEdicionDetalle(ByVal idDetalle As Integer)
        Try
            Dim datos() As DataRow
            Dim dtDetalle As New DataTable
            dtDetalle = CType(Session("dtDatosDetalleOrdenCompra"),DataTable)
            datos = dtDetalle.Select("idDetalle = " & idDetalle)
            hfIdDetalle.Value = idDetalle.ToString
            ddlFabricante.SelectedValue = datos(0)("idFabricante").ToString
            Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            ObtenerProducto(CInt(ordenCompra.IdTipoProducto))
            ddlProducto.SelectedValue = datos(0)("idProducto").ToString
            ddlTipoUnidad.SelectedValue = datos(0)("idTipoUnidad").ToString
            txtCantidad.Text = datos(0)("cantidad").ToString
            txtValorUnitario.Text = datos(0)("valorUnitario").ToString
            txtObservacionDetalleOrdenCompra.Text = datos(0)("observacion").ToString
            btnCrearDetalleOrden.Visible = False
            btnEditarDetalleOrden.Visible = True           
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el detalle de la orden. " & ex.Message)
        End Try

    End Sub

    Protected Sub btnEditarDetalleOrden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarDetalleOrden.Click
        Try
            Dim dtDetalleOrden As New DataTable
            'Dim detOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(hfIdDetalle.Value))
            Dim existeProducto As Boolean = False
            Dim drAux() As DataRow
            
            dtDetalleOrden = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)            
            drAux = dtDetalleOrden.Select("idProducto=" & ddlProducto.SelectedValue & " AND idDetalle<>" & CLng(hfIdDetalle.Value))
            If Not drAux.Length = 0 Then _
                existeProducto = True            
            If Not existeProducto Then                                                

                dtDetalleOrden.Rows.Find(CInt(hfIdDetalle.Value))("idFabricante") = ddlFabricante.SelectedValue
                dtDetalleOrden.Rows.Find(CInt(hfIdDetalle.Value))("idProducto") = ddlProducto.SelectedValue
                dtDetalleOrden.Rows.Find(CInt(hfIdDetalle.Value))("idTipoUnidad") = ddlTipoUnidad.SelectedValue
                dtDetalleOrden.Rows.Find(CInt(hfIdDetalle.Value))("cantidad") = txtCantidad.Text
                dtDetalleOrden.Rows.Find(CInt(hfIdDetalle.Value))("valorUnitario") = txtValorUnitario.Text
                dtDetalleOrden.Rows.Find(CInt(hfIdDetalle.Value))("observacion") = txtObservacionDetalleOrdenCompra.Text
                Dim filtroDetOrdenCompra As New Estructuras.FiltroDetalleOrdenCompra
                Dim dtBuscarDetalle As New DataTable
                filtroDetOrdenCompra.IdDetalle = CInt(hfIdDetalle.Value)                
                'CargarDetallesOrdenCompra()
                Session("dtDatosDetalleOrdenCompra") = dtDetalleOrden
                gvDetalleOrdenCompra.DataSource = dtDetalleOrden
                gvDetalleOrdenCompra.DataBind()
                Dim totalOrden As Integer
                Integer.TryParse(dtDetalleOrden.Compute("SUM(cantidad)", "").ToString, totalOrden)
                hfTotalOrdenCompra.Value = totalOrden.ToString
                EncabezadoPagina.showSuccess("Detalle de Orden de compra actualizado.")
                mpeAgregarDetalle.Hide()
                LimpiarDatosDetalleOrden()
            Else
                EncabezadoPaginaAgregarDetalle.showWarning("El producto seleccionado ya hace parte del detalle de la orden. Por favor verifique")
                mpeAgregarDetalle.Show()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al editar el detalle de la orden de compra. " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlProducto.SelectedIndexChanged
        Try
            Dim producto As New Productos.Producto(CInt(ddlProducto.SelectedValue))
            ObtenerTipoUnidad(CInt(producto.IdTipoUnidad))
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el tipo de unidad para el producto seleccionado. " & ex.Message)
        End Try
    End Sub

    Protected Sub imgBtnAgregarDetalle_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnAgregarDetalle.Click
        Try
            lblTituloAccion.Text = "Agregar detalle a la orden"
            btnCrearDetalleOrden.Visible = true
            btnEditarDetalleOrden.Visible = false
            mpeAgregarDetalle.Show()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el panel para agregar detalle. " & ex.Message)
        End Try
    End Sub

    Protected Sub imgBtnCerrarPopUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnCerrarPopUp.Click
        Try
            LimpiarDatosDetalleOrden()
            mpeAgregarDetalle.Hide()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cerrar el popup" & ex.Message)
        End Try

    End Sub

    Private Sub inicializaDropDownList(ByRef control As DropDownList, Optional ByVal mensaje As String = "Seleccione...")
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem(mensaje, 0))
    End Sub

    Protected Sub btnEditarOrdenCompra_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarOrdenCompra.Click
        Dim ordenObj As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
        Dim dtDetalle As New DataTable
        dtDetalle = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
        With ordenObj
            .IdProveedor = ddlEditarProveedorOrden.SelectedValue
            .IdMoneda = ddlEditarMonedaOrden.SelectedValue
            .IdIncoterm = ddlEditarIncotermOrden.SelectedValue
            .Observacion = txtEditarObservacionOrden.Text
        End With
        If trDistribucionRegional.Visible Then
            Dim dtDistribucion As DataTable = ObtenerDistribucionPorRegion()
            ordenObj.ModificarDistribucionRegional(dtDistribucion)
        End If
        If dtDetalle.Rows.Count > 0 Then ordenObj.AjustarADetalle(dtDetalle)

        'ordenObj.ActualizarDetalleOrdenCompra(dtDetalle)
        ordenObj.Actualizar()
        CargarDetallesOrdenCompra()
        CargarInfoOrden()


        EncabezadoPagina.showSuccess("Orden actualizada y confirmada")
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
                dtDistribucion.Rows.Add(drDistribucion)
            End If
        Next
        Return dtDistribucion
    End Function

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Try
            CargarDetallesOrdenCompra()
            CargarInfoOrden()
            EncabezadoPagina.showSuccess("Cambios cancelados")
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cancelar la edicion de la orden. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvProductoAdicional_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvProductoAdicional.RowCommand
        If e.CommandName = "Editar" Then
            LimpiarFormsProductoAdicional()
            CargarDatosEdicionDetalleAdicional(e.CommandArgument)
            mpeAgregarProductoAdcional.Show()
        ElseIf e.CommandName = "Eliminar" Then
            Try
                EncabezadoPagina.showSuccess("Producto Eliminado")
                Recibos.DetalleOrdenCompra.EliminarDetalle(CLng(e.CommandArgument))
                CargarDetallesOrdenCompra(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
            Catch ex As Exception
                EncabezadoPagina.showError("Error al eliminar el producto adicional inidicado. " & ex.Message)
            End Try
        End If
    End Sub

    Protected Sub LimpiarFormsProductoAdicional()
        ddlTipoProductoAdicional.SelectedIndex = -1
        inicializaDropDownList(ddlProductoAdicional, "Escoja el producto")
        txtCantidadAcional.Text = String.Empty
        MensajeCantidadAdicional(0)
    End Sub

    Private Sub CargarDatosEdicionDetalleAdicional(ByVal idDetalle As Integer)
        Try
            EncabezadoProductoAdicional.clear()
            Dim dtDetalle As DataTable = EstructuraProductoAdicional()
            Dim drDetalle As DataRow
            Dim DetalleOrdenObj As New Recibos.DetalleOrdenCompra(CLng(idDetalle))

            If DetalleOrdenObj IsNot Nothing Then
                Dim productoObj As New Productos.Producto(DetalleOrdenObj.IdProducto)
                ddlTipoProductoAdicional.SelectedValue = productoObj.IdTipoProducto
                CargarComboProductoAdicional(CShort(productoObj.IdTipoProducto))
                With ddlProductoAdicional
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(DetalleOrdenObj.IdProducto))
                End With
                txtCantidadAcional.Text = DetalleOrdenObj.Cantidad
                hfIdDetalleAdicional.Value = idDetalle
                btnAgregarAdicionales.Visible = False
                btnEditarAdicionles.Visible = True
            Else
                EncabezadoProductoAdicional.showError("Imposible recuperar la información de los productos adicionales desde la memoria. Por favor intente nuevamente.")
            End If
        Catch ex As Exception
            EncabezadoProductoAdicional.showError("Error al tratar de cargar la información de los productos adicionales. " & ex.Message)
        End Try
    End Sub

    Protected Sub MensajeCantidadAdicional(ByVal cantidad As Integer)
        If cantidad > 0 Then
            lblCantidadProductoAdicional.Text = "Cantidad de productos " & cantidad.ToString
            lblCantidadProductoAdicional.Visible = True
        Else
            lblCantidadProductoAdicional.Visible = False
        End If
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
            EncabezadoPagina.showError("Error la obtener el producto adicional. " & ex.Message)
        End Try
    End Function

    Protected Sub CargarComboProductoAdicional(ByVal idTipoProducto As Short)
        Dim filtro As New Estructuras.FiltroProducto
        Dim dt As New DataTable
        If idTipoProducto > 0 Then
            filtro.IdTipoProducto = idTipoProducto
            dt = Productos.Producto.ObtenerListado(filtro)
            With ddlProductoAdicional
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idProducto"
                .DataBind()
                ddlProductoAdicional.Items.Insert(0, New ListItem("Escoja el producto", 0))
            End With
            MensajeCantidadAdicional(dt.Rows.Count)
        Else
            inicializaDropDownList(ddlProductoAdicional, "Escoja un Producto")
            MensajeCantidadAdicional(0)
        End If
    End Sub

    Protected Sub btnAgregarAdicionales_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregarAdicionales.Click
        Dim dtDetalle As DataTable = CType(Session("dtDatosOrdenCompraProductoAdicional"), DataTable)
        Dim drDetalle As DataRow
        Dim drAux() As DataRow
        Try
            EncabezadoProductoAdicional.clear()
            drAux = dtDetalle.Select("idProducto=" & ddlProductoAdicional.SelectedValue)
            If drAux.Length = 0 Then
                drDetalle = dtDetalle.NewRow
                Dim productoObj As New Productos.Producto(CInt(ddlProductoAdicional.SelectedValue))
                Dim DetOrdenCompra As New Recibos.DetalleOrdenCompra()
                With DetOrdenCompra
                    .IdOrden = CLng(hfIdOrdenCompra.Value)
                    .IdFabricante = productoObj.IdFabricante
                    .IdProducto = productoObj.IdProducto
                    .IdTipoUnidad = productoObj.IdTipoUnidad
                    .Cantidad = CInt(txtCantidadAcional.Text)
                    .ValorUnitario = 0
                    .IdUsuario = CLng(Session("usxp001"))
                    .IdTipoDetalleOrdenCompra = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                    If .Crear() Then
                        CargarDetallesOrdenCompra(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                    End If
                End With
                LimpiarFormsProductoAdicional()
                EncabezadoProductoAdicional.showSuccess("Producto Agregado")
            Else
                EncabezadoProductoAdicional.showWarning("El producto seleccionado ya hace parte de los productos. Por favor verifique")
            End If
        Catch ex As Exception
            EncabezadoProductoAdicional.showError("Error al tratar de adicionar el producto indicado a la orden de compra. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnEditarAdicionles_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarAdicionles.Click    
        Try
            EncabezadoProductoAdicional.clear()
            Dim dtDetalleOrden As New DataTable
            Dim detOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(hfIdDetalleAdicional.Value))
            Dim existeProducto As Boolean = False
            Dim drAux() As DataRow
            dtDetalleOrden = CType(Session("dtDatosOrdenCompraProductoAdicional"), DataTable)
            If detOrdenCompra.IdProducto <> ddlProductoAdicional.SelectedValue Then
                drAux = dtDetalleOrden.Select("idProducto=" & ddlProductoAdicional.SelectedValue)
                If Not drAux.Length = 0 Then _
                    existeProducto = True
            End If
            If Not existeProducto Then
                Dim productoObj As New Productos.Producto(ddlProductoAdicional.SelectedValue)
                With detOrdenCompra
                    .IdFabricante = productoObj.IdFabricante
                    .IdProducto = productoObj.IdProducto
                    .IdTipoUnidad = productoObj.IdTipoUnidad
                    .Cantidad = CInt(txtCantidadAcional.Text)
                    .ValorUnitario = 0
                    .IdUsuario = CLng(Session("usxp001"))
                    .Observacion = String.Empty
                    .IdTipoDetalleOrdenCompra = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                    .Actualizar()
                    CargarDetallesOrdenCompra(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                    EncabezadoProductoAdicional.showSuccess("Producto adicional actualizado.")
                End With
            Else
                EncabezadoProductoAdicional.showWarning("El producto seleccionado ya hace parte del detalle de la orden. Por favor verifique")
            End If
        Catch ex As Exception
            EncabezadoProductoAdicional.showError("Error al editar el producto adicional. " & ex.Message)
        End Try

    End Sub

    Protected Sub imgBtnAgregarProductoAdicional_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnAgregarProductoAdicional.Click
        Try
            lblTituloAccionProductoAdicional.Text = "Agregar Producto Adicional"
            LimpiarFormsProductoAdicional()
            btnAgregarAdicionales.Visible = True
            btnEditarAdicionles.Visible = False
            EncabezadoProductoAdicional.clear()
            mpeAgregarProductoAdcional.Show()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el panel para agregar detalle. " & ex.Message)
        End Try
    End Sub
End Class