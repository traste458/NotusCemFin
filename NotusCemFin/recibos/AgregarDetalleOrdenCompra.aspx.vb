Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer
Imports ILSBusinessLayer.OMS
Imports ILSBusinessLayer.Estructuras

Partial Public Class AgregarDetalleOrdenCompra
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            EncabezadoPagina.clear()
            EncabezadoPaginaAgregarDetalle.clear()
            If Not IsPostBack Then
                If Not Request.QueryString("ido") Is Nothing Then
                    hfIdOrdenCompra.Value = Request.QueryString("ido").ToString
                    Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
                    Dim tipoProducto As Integer = CInt(ordenCompra.IdTipoProducto)
                    hfIdTipoProducto.Value = ordenCompra.IdTipoProducto.ToString
                    Session.Remove("dtDatosDetalleOrdenCompra")
                    EncabezadoPagina.setTitle("Administrar Detalle de Orden de Compra")
                    If ordenCompra.IdEstado = Recibos.OrdenCompra.EstadoOrden.Cancelada Then
                        DeshabilitarOpciones()
                    End If
                    EncabezadoPagina.showReturnLink("~/recibos/BuscarOrdenCompra.aspx")

                    'Carga inicial para combos de detalle de la orden
                    CargarInfoOrden()
                    ObtenerFabricante(tipoProducto)
                    inicializaDropDownList(ddlProducto)
                    '******* Variable de sesion de datos adicionales Session("dtDatosDetalleOrdenCompra")
                    ObtenerTipoUnidad()
                    CargarDetallesOrdenCompra()
                    CargarDetallesOrdenCompra(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                    'CargaProductoAdicional()
                    ObtenerTipoProductoAdicional()


                    Dim ordenCompraObj As OrdenCompra = New OrdenCompra(CLng(hfIdOrdenCompra.Value))
                    If ordenCompraObj.PosibleAdicionarDetalle Then
                        pnlAgregarDetalleOrdenCompra.Visible = True
                        pnlInfoEstadoOrdenCompra.Visible = False
                    Else
                        pnlAgregarDetalleOrdenCompra.Visible = False
                        pnlInfoEstadoOrdenCompra.Visible = True
                        hfInformacionEstadoOrdenCompra.Value = ordenCompraObj.MensajeInfo
                    End If
                Else
                    EncabezadoPagina.showWarning("Por favor actualize la pagina.")
                End If
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Protected Sub DeshabilitarOpciones()
        Try
            txtNumeroOrden.Enabled = False
            ddlEditarProveedorOrden.Enabled = False
            ddlEditarMonedaOrden.Enabled = False
            ddlEditarIncotermOrden.Enabled = False
            txtFechaPrevista.Enabled = False
            txtEditarObservacionOrden.Enabled = False

            btnEditarOrdenCompra.Visible = False

            imgFechaPrevista.Visible = False

            pnlEditarAgregarProductoAdicional.Visible = False
            'gvDetalleOrdenCompra.Columns(6).Visible = False
            gvProductoAdicional.Columns(2).Visible = False
        Catch ex As Exception
            EncabezadoPagina.showError("Error al deshabilitar las opciones de la orden. " & ex.Message)
        End Try
    End Sub

    Protected Sub CargaProductoAdicional()
        Try
            Dim dtProductoAdicional As New DataTable
            dtProductoAdicional = ObtenerProductoAdicional()
            CargaGrillaProductoAdicional(dtProductoAdicional)
            inicializaDropDownList(ddlProductoAdicional, "Escoja un Producto")
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el producto adicional. " & ex.Message)
        End Try
    End Sub

    Protected Function ObtenerProductoAdicional() As DataTable
        Dim dtProductoAdicional As New DataTable
        Dim dtProductoAdicionalCargado As New DataTable
        Dim filtroProductoAdicional As New Estructuras.FiltroDetalleOrdenCompra
        Dim nuevaFila As DataRow
        Try
            filtroProductoAdicional.IdOrden = CInt(hfIdOrdenCompra.Value)
            filtroProductoAdicional.IdTipoDetalle = TipoDetalleOrdenCompra.TipoDetalle.Secundario
            dtProductoAdicionalCargado = Recibos.DetalleOrdenCompra.ObtenerListado(filtroProductoAdicional)
            dtProductoAdicional = EstructuraProductoAdicional()
            If dtProductoAdicionalCargado.Rows.Count > 0 Then
                For i As Integer = 0 To dtProductoAdicionalCargado.Rows.Count - 1
                    nuevaFila = dtProductoAdicional.NewRow()
                    nuevaFila("idDetalleOrden") = dtProductoAdicionalCargado.Rows(i)("idDetalle").ToString
                    nuevaFila("fabricante") = dtProductoAdicionalCargado.Rows(i)("fabricante").ToString
                    nuevaFila("idFabricante") = dtProductoAdicionalCargado.Rows(i)("idFabricante")
                    nuevaFila("producto") = dtProductoAdicionalCargado.Rows(i)("producto").ToString
                    nuevaFila("idProducto") = dtProductoAdicionalCargado.Rows(i)("idProducto")
                    nuevaFila("tipoUnidad") = dtProductoAdicionalCargado.Rows(i)("TipoUnidad").ToString
                    nuevaFila("cantidad") = dtProductoAdicionalCargado.Rows(i)("cantidad")
                    dtProductoAdicional.Rows.Add(nuevaFila)
                Next
            End If
            dtProductoAdicional.AcceptChanges()
            'Session("dtDatosOrdenCompraProductoAdicional") = dtProductoAdicional
            Return dtProductoAdicional
        Catch ex As Exception
            EncabezadoPagina.showError("Error la obtener el producto adicional. " & ex.Message)
        End Try
    End Function

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

    Protected Sub CargaGrillaProductoAdicional(ByVal dtDatos As DataTable)
        With gvProductoAdicional
            .DataSource = dtDatos
            .DataBind()
        End With
    End Sub

    Protected Sub ObtenerTipoProductoAdicional()
        Try
            Dim ordenCompraObj As New Recibos.OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim idEstadoOrden As Integer = ordenCompraObj.IdEstado
            Dim filtroComboProducto As New Estructuras.FiltroCombinacionTipoProducto
            Dim dtTipoProducto As New DataTable
            filtroComboProducto.IdTipoPrimario = CShort(hfIdTipoProducto.Value)
            dtTipoProducto = CombinacionTipoProducto.ObtenerListado(filtroComboProducto)
            With ddlTipoProductoAdicional
                .DataSource = dtTipoProducto
                .DataTextField = "TipoProductoAdicional"
                .DataValueField = "idTipoProductoSecundario"
                .DataBind()
            End With
            trProductoAdicional.Visible = False
            'trProductoAdicional.Visible = IIf(dtTipoProducto.Rows.Count > 0 AndAlso _
            '                                  (idEstadoOrden = Recibos.OrdenCompra.EstadoOrden.Abierta Or idEstadoOrden = Recibos.OrdenCompra.EstadoOrden.Parcial) _
            '                                   , True, False)
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el tipo de producto adicional. " & ex.Message)
        End Try
        ddlTipoProductoAdicional.Items.Insert(0, New ListItem("Escoja Tipo Producto", 0))
    End Sub
    ''' <summary>
    ''' Realiza la carga inicial del detalle de la orden de compra traida directamente de la base de datos
    ''' </summary>
    ''' <param name="TipoDetalle">Representa el tipo de detalle que se debe cargar</param>
    ''' <remarks></remarks>
    Private Sub CargarDetallesOrdenCompra(Optional ByVal TipoDetalle As Integer = TipoDetalleOrdenCompra.TipoDetalle.Principal)
        Try
            'Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            '-----------------Detalle en session--------------------
            Dim ordenCompraObj As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim dtDetalle As New DataTable            
            dtDetalle = ordenCompraObj.Detalle            
            Dim dvDetalle As New DataView(dtDetalle)
            dvDetalle.RowFilter = "idTipoDetalle = " & TipoDetalle
            Dim totalDetalleOrden As Integer
            Integer.TryParse(dtDetalle.Compute("SUM(cantidad)", "idTipoDetalle = " & TipoDetalle).ToString, totalDetalleOrden)
            Session("dtDatosDetalleOrdenCompra") = dtDetalle

            If TipoDetalle = TipoDetalleOrdenCompra.TipoDetalle.Principal Then

                gvDetalleOrdenCompra.DataSource = dvDetalle
                gvDetalleOrdenCompra.DataBind()
                Dim totalOrden As Integer
                hfTotalOrdenCompra.Value = totalDetalleOrden
            Else                
                gvProductoAdicional.DataSource = dvDetalle
                gvProductoAdicional.DataBind()
                Dim totalDetalleAdicional As Integer
                hfTotalProductoAdicional.Value = totalDetalleOrden
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los detalle agregados a la orden de compra. " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Cargar la grilla de detalle de la orden segun lo modificado por el usuario en session
    ''' </summary>
    ''' <param name="TipoDetalle">Representa el tipo de detalle que se debe cargar</param>
    ''' <remarks></remarks>
    Private Sub CargarDetallesOrdenCompraEnSession(Optional ByVal TipoDetalle As Integer = TipoDetalleOrdenCompra.TipoDetalle.Principal)
        Try            
            '-----------------Detalle en session--------------------            
            Dim dtDetalle As New DataTable            
            dtDetalle = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)                        
            Dim dvDetalle As New DataView(dtDetalle)
            dvDetalle.RowFilter = "idTipoDetalle = " & TipoDetalle
            Dim totalDetalleOrden As Integer
            Integer.TryParse(dtDetalle.Compute("SUM(cantidad)", "idTipoDetalle = " & TipoDetalle).ToString, totalDetalleOrden)


            If TipoDetalle = TipoDetalleOrdenCompra.TipoDetalle.Principal Then
                gvDetalleOrdenCompra.DataSource = dvDetalle
                gvDetalleOrdenCompra.DataBind()
                Dim totalOrden As Integer
                hfTotalOrdenCompra.Value = totalDetalleOrden
            Else
                gvProductoAdicional.DataSource = dvDetalle
                gvProductoAdicional.DataBind()
                Dim totalDetalleAdicional As Integer
                hfTotalProductoAdicional.Value = totalDetalleOrden
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los detalle agregados a la orden de compra. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarInfoOrden()
        Try
            Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            CargarDatosEditarOrden(ordenCompra)
            With ordenCompra
                lblIdOrden.Text = .IdOrden
                lblNumeroOrden.Text = .NumeroOrden
                lblProveedor.Text = .Proveedor
                lblMoneda.Text = .Moneda
                lblIncoterm.Text = .Incoterm
                lblObservacion.Text = .Observacion
                lblEstado.Text = .Estado
                lblTipoProducto.Text = .TipoProducto.Descripcion
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
                txtNumeroOrden.Text = lblEditarOrdenNo.Text
                ddlEditarProveedorOrden.SelectedValue = .IdProveedor
                ddlEditarMonedaOrden.SelectedValue = .IdMoneda
                ddlEditarIncotermOrden.SelectedValue = .IdIncoterm
                txtFechaPrevista.Text = .FechaPrevista.ToString("dd/MM/yyyy")
                txtEditarObservacionOrden.Text = .Observacion.ToString
                trDistribucionRegional.Visible = IIf(.IdTipoProducto = 3 Or .IdTipoProducto = 5, True, False)
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
            Dim totalOrden As Integer
            Integer.TryParse(dtDistribucionPorRegion.Compute("SUM(cantidad)", "").ToString, totalOrden)
            hfCantidadDistribucion.Value = totalOrden.ToString
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar la distribución por región. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerProveedorEditar()
        Dim filtro As New Estructuras.FiltroGeneral
        Dim dtProveedor As New DataTable
        filtro.Activo = Enumerados.EstadoBinario.Activo
        Try
            dtProveedor = Proveedor.ObtenerListado(filtro, CInt(hfIdTipoProducto.Value))
            With ddlEditarProveedorOrden
                .DataSource = dtProveedor
                .DataTextField = "nombre"
                .DataValueField = "idProveedor"
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
            If dt.Rows.Count > 0 Then
                lblCantidadProducto.Text = "Total productos encontrados para este fabricante " & dt.Rows.Count
                lblCantidadProducto.Visible = True
            Else
                lblCantidadProducto.Visible = False
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
        mpeAgregarDetalle.Show()
    End Sub

    Protected Sub CargarDtDetalleOrdenCompra()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt = EstructuraDtDetalleOrdenCompra()
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
        gvDetalleOrdenCompra.DataSource = dt
        gvDetalleOrdenCompra.DataBind()
        dt.AcceptChanges()
        Session("dtDatosDetalleOrdenCompra") = dt
        LimpiarDatosDetalleOrden()

    End Sub

    Protected Function EstructuraDtDetalleOrdenCompra() As DataTable
        Dim dtDatos As DataTable
        If Session("dtDatosDetalleOrdenCompra") Is Nothing Then
            dtDatos = New DataTable
            Dim dc As New DataColumn("idDetalleOrden", GetType(Integer))
            dc.AutoIncrement = True
            dc.AutoIncrementSeed = 1
            dtDatos.Columns.Add(dc)
            dtDatos.Columns.Add("fabricante")
            dtDatos.Columns.Add("idfabricante")
            dtDatos.Columns.Add("producto")
            dtDatos.Columns.Add("idproducto")
            dtDatos.Columns.Add("TipoUnidad")
            dtDatos.Columns.Add("idtipounidad")
            dtDatos.Columns.Add("cantidad")
            dtDatos.Columns.Add("valorUnitario")
            dtDatos.Columns.Add("observacion")
            dtDatos.Columns.Add("idTipoDetalle").DefaultValue = TipoDetalleOrdenCompra.TipoDetalle.Principal
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
            EncabezadoPaginaAgregarDetalle.clear()            
            AgregarDetalle()
            LimpiarDatosDetalleOrden()
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
                    '-----------------Adicion de detalle nueva------------------
                    Dim drDetalle As DataRow
                    drDetalle = dtDetalleOrden.NewRow()

                    Dim fabricanteObj As New Fabricante(CInt(ddlFabricante.SelectedValue))
                    Dim productoObj As New Productos.Producto(CInt(ddlProducto.SelectedValue))
                    Dim tipoUnidadObj As New TipoUnidad(CInt(ddlTipoUnidad.SelectedValue))

                'Integer.TryParse(dtDetalleOrden.Compute("MAX(idDetalleOrden)", "").ToString(), drDetalle("idDetalleOrden"))
                drDetalle("idDetalleOrden") = 0
                drDetalle("fabricante") = fabricanteObj.Nombre
                drDetalle("idFabricante") = fabricanteObj.IdFabricante
                drDetalle("producto") = productoObj.Nombre
                drDetalle("idProducto") = productoObj.IdProducto
                drDetalle("tipoUnidad") = tipoUnidadObj.Descripcion
                drDetalle("idTipoUnidad") = tipoUnidadObj.IdTipoUnidad
                drDetalle("cantidad") = CInt(txtCantidad.Text.Trim())
                drDetalle("valorUnitario") = txtValorUnitario.Text.Trim()
                drDetalle("observacion") = txtObservacionDetalleOrdenCompra.Text.Trim()
                drDetalle("idTipoDetalle") = TipoDetalleOrdenCompra.TipoDetalle.Principal
                dtDetalleOrden.Rows.Add(drDetalle)
                CargarDetallesOrdenCompraEnSession()
                mpeAgregarDetalle.Hide()
                EncabezadoPagina.showSuccess("Detalle Agregado con exito.")
                LimpiarDatosDetalleOrden()

                '-----------------Adicion de detalle antigua------------------
                'Dim DetOrdenCompra As New Recibos.DetalleOrdenCompra()
                'With DetOrdenCompra
                '    .IdOrden = CLng(hfIdOrdenCompra.Value)
                '    .IdFabricante = CInt(ddlFabricante.SelectedValue)
                '    .IdProducto = CLng(ddlProducto.SelectedValue)
                '    .IdTipoUnidad = CInt(ddlTipoUnidad.SelectedValue)
                '    .Cantidad = CInt(txtCantidad.Text)
                '    .ValorUnitario = CLng(txtValorUnitario.Text)
                '    .IdUsuario = CLng(Session("usxp001"))
                '    If txtObservacionDetalleOrdenCompra.Text <> "" Then
                '        .Observacion = txtObservacionDetalleOrdenCompra.Text
                '    End If
                '    .IdTipoDetalleOrdenCompra = TipoDetalleOrdenCompra.TipoDetalle.Principal
                '    If .Crear() Then
                '        CargarDetallesOrdenCompra()
                '        mpeAgregarDetalle.Hide()
                '        EncabezadoPagina.showSuccess("Detalle Agregado con exito.")
                '        LimpiarDatosDetalleOrden()
                '    End If
                'End With
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
            Try
                Dim drAux As DataRow
                Dim dtDetalleOrden As New DataTable                
                dtDetalleOrden = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
                drAux = dtDetalleOrden.Select("idDetalleOrden=" & CLng(e.CommandArgument))(0)
                drAux.Delete()
                CargarDetallesOrdenCompraEnSession()
                EncabezadoPagina.showSuccess("Producto Eliminado")
            Catch ex As Exception
                EncabezadoPagina.showError("Error al eliminar el producto inidicado. " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub CargarDatosEdicionDetalle(ByVal idDetalle As Integer)
        Try
            Dim drAux As DataRow
            Dim dtDetalleOrden As New DataTable
            dtDetalleOrden = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
            drAux = dtDetalleOrden.Select("idDetalleOrden=" & idDetalle)(0)
            hfIdDetalle.Value = idDetalle.ToString
            ddlFabricante.SelectedValue = drAux("idFabricante")
            Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            ObtenerProducto(CInt(ordenCompra.IdTipoProducto))
            ddlProducto.SelectedValue = drAux("idProducto")
            ddlTipoUnidad.SelectedValue = drAux("idTipoUnidad")
            txtCantidad.Text = drAux("cantidad")
            txtValorUnitario.Text = drAux("valorUnitario")
            txtObservacionDetalleOrdenCompra.Text = drAux("observacion")
            btnCrearDetalleOrden.Visible = False
            btnEditarDetalleOrden.Visible = True
            'Dim detOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(idDetalle))
            'With detOrdenCompra
            '    hfIdDetalle.Value = idDetalle.ToString
            '    ddlFabricante.SelectedValue = .IdFabricante.ToString
            '    Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            '    ObtenerProducto(CInt(ordenCompra.IdTipoProducto))
            '    ddlProducto.SelectedValue = .IdProducto.ToString
            '    ddlTipoUnidad.SelectedValue = .IdTipoUnidad.ToString
            '    txtCantidad.Text = .Cantidad
            '    txtValorUnitario.Text = .ValorUnitario.ToString
            '    txtObservacionDetalleOrdenCompra.Text = .Observacion
            '    btnCrearDetalleOrden.Visible = False
            '    btnEditarDetalleOrden.Visible = True
            'End With            
            If Not ordenCompra.PosibleEditarTodoDetalle(idDetalle) Then
                ddlFabricante.Enabled = False
                ddlProducto.Enabled = False
                ddlTipoUnidad.Enabled = False
                txtValorUnitario.Enabled = False
                txtObservacionDetalleOrdenCompra.Enabled = False

                '****Defino la cantidad maxima permitida ****
                hfCantidadMaxEdicionPermitida.Value = txtCantidad.Text
                '**** Defino la cantidad minima permitida ****

                Dim dtFacturas As New DataTable
                Dim filtroFacturas As New Estructuras.FiltroInfoFactura
                filtroFacturas.IdDetalleOrdenCompra = CInt(hfIdDetalle.Value)
                dtFacturas = InfoFactura.ObtenerListado(filtroFacturas)
                Dim dtInstruccionSinFactura As New DataTable
                Dim filtroPreinstruccion As New Estructuras.FiltroPreinstruccionCliente
                filtroPreinstruccion.IdDetalleOrdenCompra = CInt(hfIdDetalle.Value)
                filtroPreinstruccion.IdFactura = 0
                dtInstruccionSinFactura = PreinstruccionCliente.ObtenerListado(filtroPreinstruccion)
                Dim cantidadSinFactura As Integer
                Dim cantidadFactura As Integer
                Integer.TryParse(dtInstruccionSinFactura.Compute("SUM(cantidadInstruccionada)", "idFactura=0").ToString, cantidadSinFactura)
                Integer.TryParse(dtFacturas.Compute("SUM(cantidad)", "").ToString, cantidadFactura)
                hfCantidadMinEdicionPermitida.Value = cantidadSinFactura + cantidadFactura
            End If



        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el detalle de la orden. " & ex.Message)
        End Try

    End Sub

    Protected Sub btnEditarDetalleOrden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarDetalleOrden.Click
        Try
            EncabezadoPaginaAgregarDetalle.clear()
            Dim dtDetalleOrden As New DataTable
            'Dim detOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(hfIdDetalle.Value))
            Dim existeProducto As Boolean = False
            Dim drAux() As DataRow
            Dim drAuxProducto() As DataRow

            dtDetalleOrden = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
            drAux = dtDetalleOrden.Select("idDetalleOrden=" & CLng(hfIdDetalle.Value))
            If drAux(0)("idProducto") <> ddlProducto.SelectedValue Then
                drAuxProducto = dtDetalleOrden.Select("idProducto=" & ddlProducto.SelectedValue)
                If Not drAuxProducto.Length = 0 Then _
                    existeProducto = True
            End If

            If Not existeProducto Then

                Dim fabricanteObj As New Fabricante(CInt(ddlFabricante.SelectedValue))
                Dim productoObj As New Productos.Producto(CInt(ddlProducto.SelectedValue))
                Dim tipoUnidadObj As New TipoUnidad(CInt(ddlTipoUnidad.SelectedValue))

                drAux(0)("fabricante") = fabricanteObj.Nombre
                drAux(0)("idFabricante") = fabricanteObj.IdFabricante
                drAux(0)("producto") = productoObj.Nombre
                drAux(0)("idProducto") = productoObj.IdProducto
                drAux(0)("tipoUnidad") = tipoUnidadObj.Descripcion
                drAux(0)("idTipoUnidad") = tipoUnidadObj.IdTipoUnidad
                drAux(0)("cantidad") = CInt(txtCantidad.Text.Trim())
                drAux(0)("valorUnitario") = txtValorUnitario.Text.Trim()
                drAux(0)("observacion") = txtObservacionDetalleOrdenCompra.Text.Trim()
                drAux(0)("idTipoDetalle") = TipoDetalleOrdenCompra.TipoDetalle.Principal
                Session("dtDatosDetalleOrdenCompra") = dtDetalleOrden
                CargarDetallesOrdenCompraEnSession()
                EncabezadoPagina.showSuccess("Detalle de Orden de compra actualizado.")
                mpeAgregarDetalle.Hide()
                LimpiarDatosDetalleOrden()

                'With detOrdenCompra
                '    .IdFabricante = CInt(ddlFabricante.SelectedValue)
                '    .IdProducto = CLng(ddlProducto.SelectedValue)
                '    .IdTipoUnidad = CInt(ddlTipoUnidad.SelectedValue)
                '    .Cantidad = CInt(txtCantidad.Text)
                '    .ValorUnitario = CLng(txtValorUnitario.Text)
                '    .IdUsuario = CLng(Session("usxp001"))
                '    .Observacion = txtObservacionDetalleOrdenCompra.Text
                '    .IdTipoDetalleOrdenCompra = TipoDetalleOrdenCompra.TipoDetalle.Principal
                '    .Actualizar()
                '    
                'End With
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
            mpeAgregarDetalle.Show()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el tipo de unidad para el producto seleccionado. " & ex.Message)
        End Try
    End Sub

    Private Sub inicializaDropDownList(ByRef control As DropDownList, Optional ByVal mensaje As String = "Seleccione...")
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem(mensaje, 0))
    End Sub



    Protected Sub btnEditarOrdenCompra_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarOrdenCompra.Click
        Try
            Dim ordenObj As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim dtDetalle As New DataTable
            With ordenObj
                .NumeroOrden = txtNumeroOrden.Text
                .IdProveedor = ddlEditarProveedorOrden.SelectedValue
                .IdMoneda = ddlEditarMonedaOrden.SelectedValue
                .IdIncoterm = ddlEditarIncotermOrden.SelectedValue
                .FechaPrevista = txtFechaPrevista.Text
                .Observacion = txtEditarObservacionOrden.Text.Trim()

                If trDistribucionRegional.Visible Then
                    Dim dtDistribucion As DataTable = ObtenerDistribucionPorRegion()
                    .ModificarDistribucionRegional(dtDistribucion)
                End If
                '.AdicionarDetalle(EstructuraProductoAdicional())                
                dtDetalle = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
                .ActualizarDetalleOrdenCompra(dtDetalle)
                VerificarEstadoDetalleOrdenCompra(ordenObj)
                .Actualizar()
                CargarInfoOrden()
                CargarDetallesOrdenCompra()
                CargarDetallesOrdenCompra(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                EncabezadoPagina.showSuccess("Orden actualizada")
            End With
        Catch ex As Exception
            EncabezadoPagina.showError("Error al editar la orden. " & ex.Message)
        End Try
    End Sub

    Private Sub VerificarEstadoDetalleOrdenCompra(ByRef ordenCompraObj As Recibos.OrdenCompra)
        Try
            If Not ordenCompraObj.Detalle Is Nothing AndAlso ordenCompraObj.Detalle.Rows.Count > 0 Then
                Dim cantidadDetalle As Integer = 0
                Dim dtRecepcion As New DataTable
                Dim cantidadRecepcion As Integer = 0                
                dtRecepcion = OrdenRecepcion.ObtenerListadoDeOrdenCompra(ordenCompraObj.IdOrden)
                Integer.TryParse(dtRecepcion.Compute("SUM(cantidad)", "").ToString(), cantidadRecepcion)
                Integer.TryParse(ordenCompraObj.Detalle.Compute("SUM(cantidad)", "").ToString(), cantidadDetalle)
                If cantidadDetalle = cantidadRecepcion AndAlso ordenCompraObj.IdEstado <> 18 Then
                    ordenCompraObj.IdEstado = 18
                End If
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
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
                drDistribucion("cantidad") = txt.Text.Trim()
                dtDistribucion.Rows.Add(drDistribucion)
            End If
        Next
        Return dtDistribucion
    End Function



    Protected Sub LimpiarFormsProductoAdicional()
        ddlTipoProductoAdicional.SelectedIndex = -1
        inicializaDropDownList(ddlProductoAdicional, "Escoja el producto")
        txtCantidadAcional.Text = String.Empty
        MensajeCantidadAdicional(0)
    End Sub

    Protected Sub ddlTipoProductoAdicional_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoProductoAdicional.SelectedIndexChanged
        Try
            CargarComboProductoAdicional(CShort(ddlTipoProductoAdicional.SelectedValue))
            mpeAgregarProductoAdcional.Show()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el producto adicional. " & ex.Message)
        End Try
    End Sub

    Protected Sub CargarComboProductoAdicional(ByVal idTipoProducto As Short)
        Dim filtro As New FiltroProducto
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

    Protected Sub MensajeCantidadAdicional(ByVal cantidad As Integer)
        If cantidad > 0 Then
            lblCantidadProductoAdicional.Text = "Cantidad de productos " & cantidad.ToString
            lblCantidadProductoAdicional.Visible = True
        Else
            lblCantidadProductoAdicional.Visible = False
        End If
    End Sub

    Protected Sub btnAgregarAdicionales_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregarAdicionales.Click
        Dim dtDetalle As DataTable = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
        Dim drDetalle As DataRow
        Dim drAux() As DataRow
        Try
            EncabezadoProductoAdicional.clear()
            drAux = dtDetalle.Select("idProducto=" & ddlProductoAdicional.SelectedValue)
            If drAux.Length = 0 Then
                drDetalle = dtDetalle.NewRow

                Dim productoObj As New Productos.Producto(CInt(ddlProductoAdicional.SelectedValue))
                Dim fabricanteObj As New Fabricante(productoObj.IdFabricante)
                Dim tipoUnidadObj As New TipoUnidad(productoObj.IdTipoUnidad)

                drDetalle("idDetalleOrden") = CInt(dtDetalle.Compute("MAX(idDetalleOrden)", "").ToString()) + 1
                drDetalle("fabricante") = fabricanteObj.Nombre
                drDetalle("idFabricante") = fabricanteObj.IdFabricante
                drDetalle("producto") = productoObj.Nombre
                drDetalle("idProducto") = productoObj.IdProducto
                drDetalle("tipoUnidad") = tipoUnidadObj.Descripcion
                drDetalle("idTipoUnidad") = tipoUnidadObj.IdTipoUnidad
                drDetalle("cantidad") = CInt(txtCantidadAcional.Text.Trim())
                drDetalle("valorUnitario") = 0
                drDetalle("observacion") = String.Empty
                drDetalle("idTipoDetalle") = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                dtDetalle.Rows.Add(drDetalle)
                CargarDetallesOrdenCompraEnSession(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                LimpiarFormsProductoAdicional()
                EncabezadoPagina.showSuccess("Producto Agregado")


                'Dim DetOrdenCompra As New Recibos.DetalleOrdenCompra()
                'With DetOrdenCompra
                '    .IdOrden = CLng(hfIdOrdenCompra.Value)
                '    .IdFabricante = productoObj.IdFabricante
                '    .IdProducto = productoObj.IdProducto
                '    .IdTipoUnidad = productoObj.IdTipoUnidad
                '    .Cantidad = CInt(txtCantidadAcional.Text)
                '    .ValorUnitario = 0
                '    .IdUsuario = CLng(Session("usxp001"))
                '    .IdTipoDetalleOrdenCompra = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                '    If .Crear() Then
                '        CargarDetallesOrdenCompra(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                '    End If
                'End With
                'LimpiarFormsProductoAdicional()
                'EncabezadoPagina.showSuccess("Producto Agregado")
            Else
                EncabezadoPaginaAgregarDetalle.showWarning("El producto seleccionado ya hace parte de los productos. Por favor verifique")
                mpeAgregarProductoAdcional.Show()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de adicionar el producto indicado a la orden de compra. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvProductoAdicional_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvProductoAdicional.RowCommand
        If e.CommandName = "Editar" Then
            LimpiarFormsProductoAdicional()
            CargarDatosEdicionDetalleAdicional(e.CommandArgument)
            mpeAgregarProductoAdcional.Show()
        ElseIf e.CommandName = "Eliminar" Then
            Try            
                Dim drAux As DataRow
                Dim dtDetalleOrden As New DataTable
                dtDetalleOrden = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
                drAux = dtDetalleOrden.Select("idDetalleOrden=" & CLng(e.CommandArgument))(0)
                dtDetalleOrden.Rows.Remove(drAux)
                CargarDetallesOrdenCompraEnSession(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                EncabezadoPagina.showSuccess("Producto Eliminado")
            Catch ex As Exception
                EncabezadoPagina.showError("Error al eliminar el producto adicional inidicado. " & ex.Message)
            End Try
        End If

    End Sub

    Private Sub CargarDatosEdicionDetalleAdicional(ByVal idDetalle As Integer)
        Try
            EncabezadoProductoAdicional.clear()
            Dim dtDetalle As DataTable = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
            Dim drDetalle As DataRow
            drDetalle = dtDetalle.Select("idDetalleOrden = " & idDetalle)(0)
            Dim productoObj As New Productos.Producto(CInt(drDetalle("idProducto")))
            ddlTipoProductoAdicional.SelectedValue = productoObj.IdTipoProducto
            CargarComboProductoAdicional(CShort(productoObj.IdTipoProducto))
            With ddlProductoAdicional
                .SelectedIndex = .Items.IndexOf(.Items.FindByValue(CInt(drDetalle("idProducto"))))
            End With
            txtCantidadAcional.Text = drDetalle("cantidad")
            hfIdDetalleAdicional.Value = idDetalle
            btnAgregarAdicionales.Visible = False
            btnEditarAdicionles.Visible = True

            'Dim DetalleOrdenObj As New Recibos.DetalleOrdenCompra(CLng(idDetalle))

            'If DetalleOrdenObj IsNot Nothing Then
            '    Dim productoObj As New Productos.Producto(DetalleOrdenObj.IdProducto)
            '    ddlTipoProductoAdicional.SelectedValue = productoObj.IdTipoProducto
            '    CargarComboProductoAdicional(CShort(productoObj.IdTipoProducto))
            '    With ddlProductoAdicional
            '        .SelectedIndex = .Items.IndexOf(.Items.FindByValue(DetalleOrdenObj.IdProducto))
            '    End With
            '    txtCantidadAcional.Text = DetalleOrdenObj.Cantidad
            '    hfIdDetalleAdicional.Value = idDetalle
            '    btnAgregarAdicionales.Visible = False
            '    btnEditarAdicionles.Visible = True
            'Else
            '    EncabezadoProductoAdicional.showError("Imposible recuperar la información de los productos adicionales desde la memoria. Por favor intente nuevamente.")
            'End If
        Catch ex As Exception
            EncabezadoProductoAdicional.showError("Error al tratar de cargar la información de los productos adicionales. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnEditarAdicionles_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarAdicionles.Click
        Try
            EncabezadoProductoAdicional.clear()
            Dim dtDetalleOrden As New DataTable
            'Dim detOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(hfIdDetalleAdicional.Value))
            Dim existeProducto As Boolean = False
            Dim drAux() As DataRow
            Dim drAuxProducto() As DataRow

            dtDetalleOrden = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
            drAux = dtDetalleOrden.Select("idDetalleOrden=" & CLng(hfIdDetalleAdicional.Value))
            If drAux(0)("idProducto") <> ddlProductoAdicional.SelectedValue Then
                drAuxProducto = dtDetalleOrden.Select("idProducto=" & ddlProducto.SelectedValue)
                If Not drAuxProducto.Length = 0 Then _
                    existeProducto = True
            End If
            If Not existeProducto Then

                Dim productoObj As New Productos.Producto(CInt(ddlProductoAdicional.SelectedValue))
                Dim fabricanteObj As New Fabricante(productoObj.IdFabricante)
                Dim tipoUnidadObj As New TipoUnidad(productoObj.IdTipoUnidad)

                drAux(0)("fabricante") = fabricanteObj.Nombre
                drAux(0)("idFabricante") = fabricanteObj.IdFabricante
                drAux(0)("producto") = productoObj.Nombre
                drAux(0)("idProducto") = productoObj.IdProducto
                drAux(0)("tipoUnidad") = tipoUnidadObj.Descripcion
                drAux(0)("idTipoUnidad") = tipoUnidadObj.IdTipoUnidad
                drAux(0)("cantidad") = CInt(txtCantidadAcional.Text.Trim())
                drAux(0)("valorUnitario") = 0
                drAux(0)("observacion") = String.Empty
                drAux(0)("idTipoDetalle") = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                CargarDetallesOrdenCompraEnSession(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                EncabezadoPagina.showSuccess("Detalle de Orden de compra actualizado.")

                'Dim productoObj As New Productos.Producto(ddlProductoAdicional.SelectedValue)
                'With detOrdenCompra
                '    .IdFabricante = productoObj.IdFabricante
                '    .IdProducto = productoObj.IdProducto
                '    .IdTipoUnidad = productoObj.IdTipoUnidad
                '    .Cantidad = CInt(txtCantidadAcional.Text)
                '    .ValorUnitario = 0
                '    .IdUsuario = CLng(Session("usxp001"))
                '    .Observacion = String.Empty
                '    .IdTipoDetalleOrdenCompra = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                '    .Actualizar()
                '    CargarDetallesOrdenCompra(TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                '    EncabezadoPagina.showSuccess("Producto adicional actualizado.")
                'End With
            Else
                EncabezadoPaginaAgregarDetalle.showWarning("El producto seleccionado ya hace parte del detalle de la orden. Por favor verifique")
                mpeAgregarProductoAdcional.Show()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al editar el producto adicional. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvDetalleOrdenCompra_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetalleOrdenCompra.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim cantidadEnFactura As Integer = 0
                Dim idDetalleOrden As Integer = CInt(fila("idDetalleOrden"))
                Dim imgBtnEliminarDetalleOrdenCompra As ImageButton = CType(e.Row.FindControl("imgBtnEliminarDetalleOrdenCompra"), ImageButton)
                Dim imgBtnEditarDetalleOrdenCompra As ImageButton = CType(e.Row.FindControl("imgBtnEditarDetalleOrdenCompra"), ImageButton)
                Dim hfInfoEstadoDetalleOrden As HiddenField = CType(e.Row.FindControl("hfInfoEstadoDetalleOrden"), HiddenField)
                Dim filtroInstruccion As New Estructuras.FiltroPreinstruccionCliente
                Dim dtInstruccion As New DataTable()
                If idDetalleOrden <> 0 Then
                    cantidadEnFactura = Recibos.InfoFactura.CantidadEnFactura(idDetalleOrden)
                    filtroInstruccion.IdDetalleOrdenCompra = idDetalleOrden
                    dtInstruccion = PreinstruccionCliente.ObtenerListado(filtroInstruccion)
                End If
                'If dtInstruccion.Rows.Count = 0 Then
                '    CType(e.Row.FindControl("imgBtnEditarDetalleOrdenCompra"), ImageButton).Visible = IIf(cantidadEnFactura = 0, True, False)
                '    'CType(e.Row.FindControl("imgBtnEliminarDetalleOrdenCompra"), ImageButton).Visible = IIf(cantidadEnFactura = 0, True, False)                    
                'Else                    
                '    CType(e.Row.FindControl("imgBtnEditarDetalleOrdenCompra"), ImageButton).Visible = False
                '    'CType(e.Row.FindControl("imgBtnEliminarDetalleOrdenCompra"), ImageButton).Visible = False
                'End If
                imgBtnEditarDetalleOrdenCompra.Visible = True
                Dim ordenCompraObj As OrdenCompra = New OrdenCompra(CLng(hfIdOrdenCompra.Value))
                If Not ordenCompraObj.PosibleEliminarDetalle(idDetalleOrden) Then
                    imgBtnEliminarDetalleOrdenCompra.ImageUrl = "~/images/Info-32.png"
                    imgBtnEliminarDetalleOrdenCompra.ToolTip = "Información del detalle de orden de compra"
                    hfInfoEstadoDetalleOrden.Value = ordenCompraObj.MensajeInfo
                Else
                    hfInfoEstadoDetalleOrden.Value = ""
                End If
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar los datos del detalle de la orden de compra. " & ex.Message)
        End Try
    End Sub

    Protected Sub lnkAgregarDetalle_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAgregarDetalle.Click
        Try
            LimpiarDatosDetalleOrden()
            EncabezadoPaginaAgregarDetalle.clear()
            lblTituloAccion.Text = "Agregar detalle a la orden"
            lblCantidadProducto.Text = String.Empty
            btnEditarDetalleOrden.Visible = False
            btnCrearDetalleOrden.Visible = True
            mpeAgregarDetalle.Show()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el panel para agregar detalle. " & ex.Message)
        End Try
    End Sub

    Protected Sub lnkAgregarProductoAdicional_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAgregarProductoAdicional.Click
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

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class