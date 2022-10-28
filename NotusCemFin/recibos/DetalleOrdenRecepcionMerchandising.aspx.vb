Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer

Partial Public Class DetalleOrdenRecepcionMerchandising
    Inherits System.Web.UI.Page

    Private idOrdenRecepcion As Integer
    Private _ordenRecepcionObj As OrdenRecepcion

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epNotificador.clear()
        epAuxNotificacion.clear()
        With Request.QueryString
            If .Item("ord") IsNot Nothing Then Integer.TryParse(.Item("ord"), idOrdenRecepcion)
        End With
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Detalle de Orden de Recepción")
            epNotificador.showReturnLink("~/recibos/BuscarOrdenRecepcion.aspx")
            Session.Remove("dtDetalleOrdenCompra")
            Session.Remove("dtCajasTemporales")
            If idOrdenRecepcion > 0 Then
                With Request.QueryString
                    If .Item("showSuccess") IsNot Nothing Then _
                        epNotificador.showSuccess("La Orden de Recepción No. " & .Item("ord") & " fue creada satisfactoriamente.")
                End With
                hfIdOrdenRecepcion.Value = idOrdenRecepcion
                CargarInfoGeneralOrdenRecepcion(idOrdenRecepcion)
                CargarMateriales()
                CargarRegiones()
                CargarNovedades()
                CargarPallets()
                CalcularCantidadDisponibleTotal()
                CargarInfoProductoAdicional()
                If (hfIdEstadoOrdenRecepcion.Value = "16" Or hfIdEstadoOrdenRecepcion.Value = "17") Then
                    CargarCajasTemporales()
                Else
                    lbCerrarOrden.Visible = False
                    pnlCreacionPallet.Visible = False
                    epNotificador.showWarning("La Orden de Recepción No. " & idOrdenRecepcion.ToString & " ya se encuentra cerrada.")
                End If
                CalcularCantidadDisponiblePorProducto()
                lbCrearPallet.Focus()
            Else
                epNotificador.showError("Imposible recuperar el identificador de la Orden de Recepción. Por favor recargue la página.")
                pnlGeneral.Visible = False
            End If
        End If
    End Sub

    Protected Sub CargarInfoProductoAdicional()
        Dim dtProductoAdicional As New DataTable
        Try
            If OrdenRecepcionObj.IdOrdenCompra > 0 Then
                Dim filtroDetalleProducto As New Estructuras.FiltroDetalleOrdenCompra
                filtroDetalleProducto.IdOrden = CInt(OrdenRecepcionObj.IdOrdenCompra)
                filtroDetalleProducto.IdTipoDetalle = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                dtProductoAdicional = Recibos.DetalleOrdenCompra.ObtenerListado(filtroDetalleProducto)
            Else
                If ProductoAdicional Then
                    Dim filtro As New Estructuras.FiltroListaProducto
                    Dim ListaIdTipoProducto As New ArrayList
                    Dim dtTipoProducto As New DataTable
                    Dim filtroComboProducto As New Estructuras.FiltroCombinacionTipoProducto
                    filtroComboProducto.IdTipoPrimario = CShort(hfIdTipoProducto.Value)
                    dtTipoProducto = CombinacionTipoProducto.ObtenerListado(filtroComboProducto)
                    For Each fila As DataRow In dtTipoProducto.Rows
                        ListaIdTipoProducto.Add(fila("idTipoProductoSecundario"))
                    Next
                    filtro.ListaIdTipoProducto = ListaIdTipoProducto
                    dtProductoAdicional = Productos.Producto.ObtenerLista(filtro)

                Else
                    tblCrearPalletAdicional.Visible = False
                    pnlProductoAdicional.Visible = False
                End If
            End If
            If dtProductoAdicional.Rows.Count > 0 Then
                With ddlProductoAdicional
                    .DataSource = dtProductoAdicional
                    .DataTextField = "producto"
                    .DataValueField = "idProducto"
                    .DataBind()
                    If dtProductoAdicional.Rows.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el producto adicional", 0))
                End With
                CargarCajasTemporalesProAdicional()
                CargarPalletsProAdicional()
                '**** Ocultar palet adicional por solicitud de recibos solo para productos diferentes a telefonos y sim cards
                'pnlProductoAdicional.Visible = True
                'tblCrearPalletAdicional.Visible = True
                'tblPalletsProductoAdicional.Visible = True
                pnlProductoAdicional.Visible = False
                tblCrearPalletAdicional.Visible = False
                tblPalletsProductoAdicional.Visible = False
            Else
                pnlProductoAdicional.Visible = False
                tblCrearPalletAdicional.Visible = False
                tblPalletsProductoAdicional.Visible = False
            End If

        Catch ex As Exception
            EncabezadoPagina1.showError("Error al cargar los datos del producto adicional. " & ex.Message)
        End Try
    End Sub

    Private Sub LimpiarFormularioCrearPalletProductoAdicional()
        ddlProductoAdicional.ClearSelection()
        txtCantidadAdicional.Text = String.Empty
        txtPesoPalletAdicionl.Text = String.Empty
        CargarCajasTemporalesProAdicional()
        CargarPalletsProAdicional()
    End Sub

    Private Sub CargarPalletsProAdicional()
        Dim dtPallet As New DataTable
        Dim filtro As Estructuras.FiltroPalletRecepcion
        Try
            filtro.IdOrdenRecepcion = CInt(OrdenRecepcionObj.IdOrdenRecepcion)
            filtro.IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Secundario
            filtro.IdEstado = 57
            Dim dcAux As New DataColumn("numPallet", GetType(Short))
            dcAux.AutoIncrement = True
            dcAux.AutoIncrementSeed = 1
            dcAux.AutoIncrementStep = 1
            dtPallet.Columns.Add(dcAux)
            PalletRecepcion.LlenarListado(filtro, dtPallet)
            With gvPalletProductoAdicional
                .DataSource = dtPallet
                If dtPallet.Rows.Count > 0 Then .Columns(0).FooterText = "<div class='thGris'>" & dtPallet.Rows.Count.ToString & " Pallet(s) Registrado(s)</div>"
                .DataBind()
            End With
            MetodosComunes.mergeGridViewFooter(gvPalletProductoAdicional)
        Catch ex As Exception
            EncabezadoPagina1.showError("Error al tratar de cargar Pallets registrados. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarCajasTemporalesProAdicional()
        Dim dtCaja As New DataTable
        Dim filtro As Estructuras.FiltroCajaEmpaque
        Try
            filtro.IdOrdenRecepcion = CLng(OrdenRecepcionObj.IdOrdenRecepcion)
            filtro.IdEstado = 39
            filtro.IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Secundario
            Dim dcAux As New DataColumn("numCaja", GetType(Short))
            dcAux.AutoIncrement = True
            dcAux.AutoIncrementSeed = 1
            dcAux.AutoIncrementStep = 1
            dtCaja.Columns.Add(dcAux)
            CajaEmpaque.LlenarListado(filtro, dtCaja)
            Dim dvCaja As DataView = dtCaja.DefaultView
            'dvCaja.Sort = "numCaja desc"
            With gvProductoAdicional
                .DataSource = dvCaja
                If dvCaja.Count > 0 Then .Columns(0).FooterText = "<div class='thGris'>" & _
                    dvCaja.Count.ToString & " Cajas(s) Temporalmente Registrada(s)</div>"
                .DataBind()
            End With
            Session("dtCajasTemporalesProAdicional") = dtCaja
            MetodosComunes.mergeGridViewFooter(gvProductoAdicional)
            tblCrearPalletAdicional.Visible = CBool(dtCaja.Rows.Count)
        Catch ex As Exception
            EncabezadoPagina1.showError("Error al tratar de cargar Cajas temporalmente para producto adicional. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarInfoGeneralOrdenRecepcion(ByVal idOrden As Integer)
        Try
            Dim miOrden As New OrdenRecepcion(idOrden)
            If miOrden.IdOrdenRecepcion > 0 Then
                Dim dtInfoDistribucion As DataTable
                Dim cantidadTotal As Integer = 0
                With miOrden
                    lblOrdenRecepcion.Text = .IdOrdenRecepcion
                    lblFechaRecepcion.Text = .FechaRecepcion.ToShortDateString()
                    If .IdOrdenCompra > 0 Then
                        fOrdenCompra.Visible = True
                        lblOrdenCompra.Text = .NumeroOrdenCompra
                        dtInfoDistribucion = OrdenCompra.ObtenerDistribucionRegional(.IdOrdenCompra)
                        hfIdOrdenCompra.Value = .IdOrdenCompra
                        CargarDistribucionRegional(dtInfoDistribucion)
                        trDistribucionPorRegiones.Visible = True
                    Else
                        trDistribucionPorRegiones.Visible = False
                        fOrdenCompra.Visible = False
                    End If

                    lblRemision.Text = .Remision
                    lblTipoProducto.Text = .TipoProducto
                    lblTipoRecepcion.Text = .TipoRecepcion
                    lblConsignado.Text = .Consignatario.Nombre
                    lblDestinatario.Text = .ClienteExterno.Nombre
                    lblEstadoOrden.Text = .Estado.Descripcion

                    hfIdEstadoOrdenRecepcion.Value = .IdEstado
                End With
                '**** Define si el tipo de producto actual debe ser pesado o no
                Dim tipoProductoObj As New Productos.TipoProducto(miOrden.IdTipoProducto)
                trPeso.Visible = tipoProductoObj.Pesado

            Else
                epNotificador.showWarning("Imposible recuperar la información de la Orden de Recepción desde la BD. Por favor recargue la página.")
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener la información general de la Orden de Recepción. " & ex.Message)
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

        With gvDistribucion
            .DataSource = dtAux
            .DataBind()
        End With
    End Sub

    Private Sub CargarMateriales()
        Dim dtMateriales As New DataTable
        Try
            If OrdenRecepcionObj.IdOrdenCompra > 0 Then
                dtMateriales = OrdenCompra.ObtenerMateriales(OrdenRecepcionObj.IdOrdenCompra)
            Else
                Dim filtro As New Estructuras.FiltroSubproducto
                filtro.IdTipoProducto = OrdenRecepcionObj.IdTipoProducto
                dtMateriales = Productos.Subproducto.ObtenerListado(filtro)                
            End If
            With ddlMaterial
                .DataSource = dtMateriales
                .DataTextField = "subproducto"
                .DataValueField = "idSubProducto2"
                .DataBind()
            End With
            Session("dtDetalleOrdenCompra") = OrdenCompra.ObtenerDetalle(OrdenRecepcionObj.IdOrdenCompra)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Materiales.")
        End Try
        If ddlMaterial.Items.Count <> 1 Then
            ddlMaterial.Items.Insert(0, New ListItem("Escoja un Material", "0"))
        Else
            txtFiltroMaterial.Enabled = False
        End If
    End Sub

    Private Sub CargarRegiones()
        Dim idOrdenCompra As Integer = 0
        Dim dtRegiones As New DataTable
        Dim campoRegion As String
        Integer.TryParse(hfIdOrdenCompra.Value, idOrdenCompra)
        Try
            If idOrdenCompra > 0 Then
                dtRegiones = OrdenCompra.ObtenerDistribucionRegional(idOrdenCompra)
                campoRegion = "region"
            Else
                dtRegiones = Region.ObtenerTodas()
                campoRegion = "nombreRegion"
            End If
            With ddlRegion
                .DataSource = dtRegiones
                .DataTextField = campoRegion
                .DataValueField = "idRegion"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Regiones asociadas a la Orden de Compra")
        End Try
        If ddlRegion.Items.Count <> 1 Then ddlRegion.Items.Insert(0, New ListItem("Escoja una Región", "0"))
    End Sub

    Private Sub CargarNovedades()
        Dim dtNovedad As DataTable
        Dim filtro As New Estructuras.FiltroNovedadILS
        Try
            filtro.Estado = True
            filtro.IdTipoNovedad = 1
            dtNovedad = Novedad.Novedad.ObtenerListado(filtro)
            With cblNovedad
                .DataSource = dtNovedad
                .DataTextField = "descripcion"
                .DataValueField = "idNovedad"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de novedades. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarPallets()
        Dim dtPallet As New DataTable
        Dim filtro As Estructuras.FiltroPalletRecepcion
        Try
            filtro.IdOrdenRecepcion = idOrdenRecepcion
            filtro.IdEstado = 57
            Dim dcAux As New DataColumn("numPallet", GetType(Short))
            dcAux.AutoIncrement = True
            dcAux.AutoIncrementSeed = 1
            dcAux.AutoIncrementStep = 1
            dtPallet.Columns.Add(dcAux)
            PalletRecepcion.LlenarListado(filtro, dtPallet)
            With gvPallets
                .DataSource = dtPallet
                If dtPallet.Rows.Count > 0 Then .Columns(0).FooterText = "<div class='thGris'>" & dtPallet.Rows.Count.ToString & " Pallet(s) Registrado(s)</div>"
                .DataBind()
            End With
            MetodosComunes.mergeGridViewFooter(gvPallets)
            lbCerrarOrden.Enabled = CBool(dtPallet.Rows.Count)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar Pallets registrados. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarCajasTemporales()
        Dim dtCaja As New DataTable
        Dim filtro As Estructuras.FiltroCajaEmpaque
        Try
            filtro.IdOrdenRecepcion = idOrdenRecepcion
            filtro.IdEstado = 39
            Dim dcAux As New DataColumn("numCaja", GetType(Short))
            dcAux.AutoIncrement = True
            dcAux.AutoIncrementSeed = 1
            dcAux.AutoIncrementStep = 1
            dtCaja.Columns.Add(dcAux)
            CajaEmpaque.LlenarListado(filtro, dtCaja)
            Dim dvCaja As DataView = dtCaja.DefaultView
            'dvCaja.Sort = "numCaja desc"
            With gvCajas
                .DataSource = dvCaja
                If dvCaja.Count > 0 Then .Columns(0).FooterText = "<div class='thGris'>" & _
                    dvCaja.Count.ToString & " Cajas(s) Temporalmente Registrada(s)</div>"
                .DataBind()
            End With
            Session("dtCajasTemporales") = dtCaja
            MetodosComunes.mergeGridViewFooter(gvCajas)
            'lbCrearPallet.Enabled = CBool(dtCaja.Rows.Count)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar Cajas temporalmente registradas. " & ex.Message)
        End Try
    End Sub

    Private Sub gvPallets_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPallets.RowCommand
        If e.CommandName = "Imprimir" Then
            Dim idPallet As Long
            Long.TryParse(e.CommandArgument, idPallet)
            ImprimirHojaViajera(idPallet, True)
        ElseIf e.CommandName = "Eliminar" Then
            EliminarPallet(CLng(e.CommandArgument))
        End If
    End Sub

    Private Sub EliminarPallet(ByVal idPallet As Long)
        Try
            If Recibos.PalletRecepcion.Eiliminar(idPallet) Then
                CargarPallets()
                'CargarPalletsProAdicional()
                EncabezadoPagina1.showSuccess("Pallet eliminado correctamente.")
            End If
        Catch ex As Exception
            EncabezadoPagina1.showError("Error al eliminar el palllet. " & ex.Message)
        End Try
    End Sub

    Private Sub gvPallets_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPallets.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(4).Text = e.Row.Cells(4).Text.Replace(",", "<br/>")
            Dim idPallet As Integer
            Integer.TryParse(e.Row.Cells(1).Text, idPallet)
            Try
                Dim idPerfil As Integer = CInt(Session("usxp009"))
                Dim btnViajera As ImageButton = CType(e.Row.FindControl("ibImprimir"), ImageButton)
                If idPerfil > 0 Then
                    btnViajera.Visible = OrdenRecepcion.PerfilesReimprimirViajera.Contains(idPerfil)
                End If

                Dim dtDetalle As DataTable = PalletRecepcion.ObtenerDetallePorPallet(idPallet)
                With CType(e.Row.FindControl("gvDetalle"), GridView)
                    .DataSource = dtDetalle
                    .DataBind()
                End With
                dtDetalle.Dispose()
            Catch ex As Exception
                epNotificador.showError("Ocurrión un error al tratar de obtener el detalle de uno más pallets. " & ex.Message)
            End Try
        End If
    End Sub

    Protected Sub lbAdicionarCaja_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbAdicionarCaja.Click
        Dim miCaja As New CajaEmpaque()
        Dim resultado As Short
        Dim materialObj As Productos.Material
        Try
            With miCaja
                .IdOrdenRecepcion = idOrdenRecepcion
                materialObj = New Productos.Material(ddlMaterial.SelectedValue.ToString())
                .Material = materialObj.Material
                .IdProducto = materialObj.IdProductoPadre
                Integer.TryParse(ddlRegion.SelectedValue, .IdRegion)
                Integer.TryParse(txtCantidad.Text, .Cantidad)
                .IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Principal
                If Session("usxp001") IsNot Nothing Then Integer.TryParse(Session("usxp001"), .IdCreador)
                resultado = .Registrar()
                If resultado = 0 Then
                    epAuxNotificacion.showSuccess("La Caja fue adicionada satisfactoriamente.")
                    LimpiarFormularioAdicionCaja()
                    CargarCajasTemporales()
                    CalcularCantidadDisponiblePorProducto()
                Else
                    If resultado = 2 Then
                        epAuxNotificacion.showWarning("No se puede registrar la información, porque no se han proporcionado todos los datos requeridos. Por favor verifique")
                    Else
                        epAuxNotificacion.showError("Ocurrió un error inesperado al registrar la información. Por favor intente nuevamente")
                    End If
                End If
            End With
        Catch ex As Exception
            epAuxNotificacion.showError("Error al tratar de adicionar caja. " & ex.Message)
        End Try
        ddlMaterial.Focus()
    End Sub

    Protected Sub lbCrearPallet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCrearPallet.Click
        Try
            Dim miPallet As New PalletRecepcion
            Dim ordenRecepcionObj As New OrdenRecepcion(idOrdenRecepcion)
            With miPallet
                .IdOrdenRecepcion = idOrdenRecepcion
                If Session("usxp001") IsNot Nothing Then Integer.TryParse(Session("usxp001"), .IdCreador)
                Decimal.TryParse(txtPeso.Text, .Peso)
                .Observacion = txtObservacion.Text.Trim
                For Each liNovedad As ListItem In cblNovedad.Items
                    If liNovedad.Selected Then .AdicionarNovedad(CInt(liNovedad.Value))
                Next
                Dim dtCajas As DataTable = CType(Session("dtCajasTemporales"), DataTable)
                .IdFacturaGuia = ordenRecepcionObj.IdFacturaGuia
                If .CrearConCajas(dtCajas) Then
                    ImprimirHojaViajera(.IdPallet)
                    epAuxNotificacion.showSuccess("La información del Pallet No. " & .IdPallet.ToString & " fue registrada satisfactoriamente. ")
                    LimpiarFormularioCrearPallet()
                Else
                    epAuxNotificacion.showError("Ocurrió un error inesperado al crear el Pallet. Por vafor intente nuevamente")
                End If
            End With
            Session.Remove("dtCajasTemporales")
        Catch ex As Exception
            epAuxNotificacion.showError("Error al tratar de crear Pallet. " & ex.Message)
        End Try

        lbCrearPallet.Focus()
    End Sub

    Protected Sub lbCerrarOrden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCerrarOrden.Click
        Try
            Dim miOrden As New OrdenRecepcion(idOrdenRecepcion)
            With miOrden
                .IdOrdenRecepcion = idOrdenRecepcion
                .IdEstado = 18
                .Actualizar()
            End With
            pnlCreacionPallet.Visible = False
            epNotificador.showSuccess("La Orden de Recepción fue cerrar satisfactoriamente.")
            lbCerrarOrden.Visible = False
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cerrar la orden de recepción. " & ex.Message)
        End Try
        lbCerrarOrden.Focus()
    End Sub

    Private Sub LimpiarFormularioAdicionCaja()
        If txtFiltroMaterial.Text.Trim.Length > 0 Then CargarMateriales()
        txtFiltroMaterial.Text = ""
        ddlMaterial.ClearSelection()
        ddlRegion.ClearSelection()
        txtCantidad.Text = ""
    End Sub

    Private Sub LimpiarFormularioCrearPallet()
        If txtFiltroMaterial.Text.Trim.Length > 0 Then CargarMateriales()
        txtFiltroMaterial.Text = ""
        ddlMaterial.ClearSelection()
        ddlRegion.ClearSelection()
        txtCantidad.Text = ""
        txtPeso.Text = ""
        txtObservacion.Text = ""
        cblNovedad.ClearSelection()
        CargarCajasTemporales()
        CargarPallets()
        CalcularCantidadDisponiblePorProducto()
        CalcularCantidadDisponibleTotal()
    End Sub

    Private Sub CalcularCantidadDisponiblePorProducto()
        Dim idProducto As Integer
        Dim cantidadDisponible As Integer
        Dim materialObj As New Productos.Material(ddlMaterial.SelectedValue.ToString())
        idProducto = materialObj.IdProductoPadre
        If idProducto > 0 And OrdenRecepcionObj.IdOrdenCompra > 0 Then
            Try
                Dim cantidadObjetivo As Integer
                Dim cantidadTemporal As Integer
                Dim cantidadRecibida As Integer
                Dim idOrdenCompra As Integer
                Dim filtro As String = "idProducto=" & idProducto.ToString
                Integer.TryParse(hfIdOrdenCompra.Value, idOrdenCompra)
                If Session("dtDetalleOrdenCompra") IsNot Nothing Then
                    Dim dtDetalleOrdenCompra As DataTable = CType(Session("dtDetalleOrdenCompra"), DataTable)
                    Integer.TryParse(dtDetalleOrdenCompra.Compute("SUM(cantidad)", filtro).ToString, cantidadObjetivo)
                End If
                If Session("dtCajasTemporales") IsNot Nothing Then
                    Dim dtCajasTemporales As DataTable = CType(Session("dtCajasTemporales"), DataTable)
                    Integer.TryParse(dtCajasTemporales.Compute("SUM(cantidad)", filtro).ToString, cantidadTemporal)
                End If
                Dim dtDetalleRecepcionOrdenCompra As DataTable = OrdenCompra.ObtenerDetalleRecepcion(idOrdenCompra)
                Integer.TryParse(dtDetalleRecepcionOrdenCompra.Compute("SUM(cantidad)", filtro).ToString, cantidadRecibida)
                cantidadDisponible = Math.Max((cantidadObjetivo - (cantidadTemporal + cantidadRecibida)), 0)
                hfCantidadDisponible.Value = cantidadDisponible.ToString
                'lblCantidadDisponible.Text = "Cantidad No Recibida: " & cantidadDisponible.ToString
            Catch ex As Exception
                epAuxNotificacion.showError("Error al tratar de obtener la cantidad disponible del Producto. " & ex.Message)
            End Try
        Else
            hfCantidadDisponible.Value = ""
            'lblCantidadDisponible.Text = ""
        End If
    End Sub

    Private Sub CalcularCantidadDisponibleTotal()
        Try
            Dim idOrdenCompra As Integer
            Integer.TryParse(hfIdOrdenCompra.Value, idOrdenCompra)

            If idOrdenCompra > 0 Then

                Dim dtDetalleOrdenCompra As DataTable = CType(Session("dtDetalleOrdenCompra"), DataTable)
                'Dim dtCajasTemporales As DataTable = CType(Session("dtCajasTemporales"), DataTable)
                Dim dtDetalleRecepcionOrdenCompra As DataTable = OrdenCompra.ObtenerDetalleRecepcion(idOrdenCompra)
                Dim cantidadObjetivo As Integer
                'Dim cantidadTemporal As Integer
                Dim cantidadRecibida As Integer
                Integer.TryParse(dtDetalleOrdenCompra.Compute("SUM(cantidad)", "").ToString, cantidadObjetivo)
                'Integer.TryParse(dtCajasTemporales.Compute("SUM(cantidad)", "").ToString, cantidadTemporal)
                Integer.TryParse(dtDetalleRecepcionOrdenCompra.Compute("SUM(cantidad)", "").ToString, cantidadRecibida)
                'Dim cantidadDisponible As Integer = (cantidadObjetivo - (cantidadTemporal + cantidadRecibida))
                Dim cantidadDisponible As Integer = (cantidadObjetivo - cantidadRecibida)
                If cantidadDisponible = 0 Then
                    pnlCreacionPallet.Enabled = False
                Else
                    pnlCreacionPallet.Enabled = True
                End If
            End If
        Catch ex As Exception
            epAuxNotificacion.showError("Error al tratar de obtener la cantidad disponible total. " & ex.Message)
        End Try
    End Sub

    Private Sub GenerarDetalleDePallet(ByVal dtCajas As DataTable, ByVal miPallet As PalletRecepcion)
        Try
            Dim arrCampos As New ArrayList(("idProducto,idRegion").Split(","))
            Dim dtAux As DataTable = MetodosComunes.getDistinctsFromDataTable(dtCajas, arrCampos)
            Dim idProducto As Integer
            Dim idRegion As Integer
            Dim cantidad As Integer
            Dim filtro As String

            For Each drAux As DataRow In dtAux.Rows
                Integer.TryParse(drAux("idProducto").ToString, idProducto)
                Integer.TryParse(drAux("idRegion").ToString, idRegion)
                filtro = "idProducto = " & idProducto.ToString & " AND idRegion = " & idRegion.ToString
                Integer.TryParse(dtCajas.Compute("SUM(cantidad)", filtro).ToString, cantidad)
                miPallet.AdicionarDetalle(idProducto, cantidad, 0, idRegion)
            Next
        Catch ex As Exception
            epAuxNotificacion.showError("Error al tratar de generar el detalle del pallet. " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlMaterial.SelectedIndexChanged
        CalcularCantidadDisponiblePorProducto()
    End Sub

    Private Sub gvCajas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvCajas.RowCommand
        If e.CommandName = "Anular" Then
            Dim idCaja As Long = CLng(e.CommandArgument)
            Try
                Dim miCaja As New CajaEmpaque(idCaja)
                Dim resultado As Short = 0
                If miCaja.IdCaja > 0 Then
                    resultado = miCaja.Anular()
                    If resultado = 0 Then
                        epAuxNotificacion.showSuccess("La Caja fue removida satisfactoriamente. ")
                        CargarCajasTemporales()
                        CalcularCantidadDisponiblePorProducto()
                    Else
                        If resultado = 1 Then
                            epAuxNotificacion.showWarning("La Caja seleccionada ya no existe, por favor recargue la página. ")
                        ElseIf resultado = 3 Then
                            epAuxNotificacion.showWarning("No se puede registrar la información, porque no se han proporcionado todos los datos requeridos. Por favor verifique")
                        Else
                            epAuxNotificacion.showError("Ocurrió un error inesperado al registrar la información. Por favor intente nuevamente")
                        End If
                    End If
                Else
                    epAuxNotificacion.showWarning("Imposible remover la Caja. Por favor intente nuevamente.")
                End If
            Catch ex As Exception
                epAuxNotificacion.showError("Error al tratar de remover caja. " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub ImprimirHojaViajera(ByVal idPallet As Integer, Optional ByVal reImpresion As Boolean = False)
        Try
            Dim rpt As New ReporteCrystal("resumenPalletRecepcion", Server.MapPath("~/Reports"))
            rpt.agregarParametroDiscreto("@idPallet", idPallet)
            rpt.agregarParametroDiscreto("reimpresion", reImpresion)
            Dim ruta As String = rpt.exportar(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
            ruta = ruta.Substring(ruta.LastIndexOf("\") + 1)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "newWindow", "window.open('../Reports/rptTemp/" + ruta + "','Impresion_Viajera', 'status=1, toolbar=0, location=0,menubar=1,directories=0,resizable=1,scrollbars=1'); ", True)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de generar el documento. " & ex.Message)
        End Try
        Dim bl As New BulletedList

    End Sub


    Protected Sub lnkAgregarProductoAdicional_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAgregarProductoAdicional.Click
        Dim miCaja As New CajaEmpaque()
        Dim resultado As Short
        Dim cajaCargada As CajaEmpaque
        Try
            With miCaja
                .IdOrdenRecepcion = CLng(OrdenRecepcionObj.IdOrdenRecepcion)
                Integer.TryParse(ddlProductoAdicional.SelectedValue, .IdProducto)
                Integer.TryParse(txtCantidadAdicional.Text, .Cantidad)
                If Session("usxp001") IsNot Nothing Then Integer.TryParse(Session("usxp001"), .IdCreador)
                .IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Secundario

                resultado = .Registrar()
                If resultado = 0 Then
                    cajaCargada = New CajaEmpaque(CInt(.IdCaja))
                    EncabezadoPagina1.showSuccess("El producto fue adicionado satisfactoriamente.")
                    LimpiarFormularioAdicionCaja()
                    CargarCajasTemporalesProAdicional()
                Else
                    If resultado = 2 Then
                        EncabezadoPagina1.showWarning("No se puede registrar la información, porque no se han proporcionado todos los datos requeridos. Por favor verifique")
                    Else
                        EncabezadoPagina1.showError("Ocurrió un error inesperado al registrar la información. Por favor intente nuevamente")
                    End If
                End If
            End With
        Catch ex As Exception
            EncabezadoPagina1.showError("Error al agregar el producto adicional. " & ex.Message)
        End Try
        ddlProductoAdicional.Focus()
    End Sub

    Protected Sub gvProductoAdicional_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvProductoAdicional.RowCommand
        If e.CommandName = "Anular" Then
            Dim idCaja As Long = CLng(e.CommandArgument)
            Try
                Dim miCaja As New CajaEmpaque(idCaja)
                Dim resultado As Short = 0
                If miCaja.IdCaja > 0 Then
                    resultado = miCaja.Anular()
                    If resultado = 0 Then
                        CargarCajasTemporalesProAdicional()
                        EncabezadoPagina1.showSuccess("El producto adicional fue removido satisfactoriamente. ")
                    Else
                        If resultado = 1 Then
                            EncabezadoPagina1.showWarning("El producto adicional ya no existe, por favor recargue la página. ")
                        ElseIf resultado = 3 Then
                            EncabezadoPagina1.showWarning("No se puede registrar la información, porque no se han proporcionado todos los datos requeridos. Por favor verifique")
                        Else
                            EncabezadoPagina1.showError("Ocurrió un error inesperado al registrar la información. Por favor intente nuevamente")
                        End If
                    End If
                Else
                    EncabezadoPagina1.showWarning("Imposible remover la Caja. Por favor intente nuevamente.")
                End If
            Catch ex As Exception
                EncabezadoPagina1.showError("Error al tratar de remover caja. " & ex.Message)
            End Try
        End If
    End Sub

    Protected Sub lnkCrearPalletProAdicional_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCrearPalletProAdicional.Click
        Try
            Dim miPallet As New PalletRecepcion
            With miPallet
                .IdOrdenRecepcion = CLng(OrdenRecepcionObj.IdOrdenRecepcion)
                If Session("usxp001") IsNot Nothing Then Integer.TryParse(Session("usxp001"), .IdCreador)
                .Peso = CDec(txtPesoPalletAdicionl.Text)
                .Observacion = txtObservacion.Text.Trim
                Dim dtCajas As DataTable = CType(Session("dtCajasTemporalesProAdicional"), DataTable)
                .IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                If .CrearConCajasSinRegion(dtCajas) Then
                    imprimirViajera(.IdPallet)
                    EncabezadoPagina1.showSuccess("La información del Pallet No. " & .IdPallet.ToString & " fue registrada satisfactoriamente. ")
                    LimpiarFormularioCrearPalletProductoAdicional()
                Else
                    EncabezadoPagina1.showError("Ocurrió un error inesperado al crear el Pallet. Por vafor intente nuevamente")
                End If
            End With
            Session.Remove("dtCajasTemporalesProAdicional")
        Catch ex As Exception
            EncabezadoPagina1.showError("Error al tratar de crear Pallet. " & ex.Message)
        End Try
    End Sub

    Private Sub imprimirViajera(ByVal idDetallePallet As Integer, Optional ByVal reImpresion As Boolean = False)
        Try
            Dim rpt As New ReporteCrystal("resumenPalletRecepcion", Server.MapPath("../Reports"))
            rpt.agregarParametroDiscreto("@idPallet", idDetallePallet)
            rpt.agregarParametroDiscreto("reimpresion", reImpresion)
            Dim ruta As String = rpt.exportar(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
            ruta = ruta.Substring(ruta.LastIndexOf("\") + 1)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "newWindow", "window.open('../Reports/rptTemp/" + ruta + "','Impresion_Viajera', 'status=1, toolbar=0, location=0,menubar=1,directories=0,resizable=1,scrollbars=1'); ", True)
        Catch ex As Exception
            EncabezadoPagina1.showError("Error al generar el documento. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvPalletProductoAdicional_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPalletProductoAdicional.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim idPallet As Integer
            Integer.TryParse(e.Row.Cells(1).Text, idPallet)
            Try
                Dim dtDetalle As DataTable = PalletRecepcion.ObtenerDetallePorPallet(idPallet, TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                With CType(e.Row.FindControl("gvDetalle"), GridView)
                    .DataSource = dtDetalle
                    .DataBind()
                End With
                dtDetalle.Dispose()
                CType(e.Row.FindControl("imgBtnEliminarPallet"), ImageButton).Visible = MostrarOcultar

                Dim idPerfil As Integer = CInt(Session("usxp009"))
                Dim btnViajera As ImageButton = CType(e.Row.FindControl("ibImprimir"), ImageButton)
                If idPerfil > 0 Then
                    btnViajera.Visible = OrdenRecepcion.PerfilesReimprimirViajera.Contains(idPerfil)
                End If
            Catch ex As Exception
                EncabezadoPagina1.showError("Ocurrio un error al tratar de obtener el detalle de uno más pallets. " & ex.Message)
            End Try
        End If
    End Sub


    Protected Sub gvPalletProductoAdicional_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPalletProductoAdicional.RowCommand
        If e.CommandName = "Imprimir" Then
            imprimirViajera(CInt(e.CommandArgument), True)
        ElseIf e.CommandName = "Eliminar" Then
            EliminarPallet(CLng(e.CommandArgument))
        End If
    End Sub


    Public ReadOnly Property MostrarOcultar() As Boolean
        Get
            If Me._ordenRecepcionObj Is Nothing Then _
                Me._ordenRecepcionObj = New Recibos.OrdenRecepcion(CLng(hfIdOrdenRecepcion.Value))
            If Me._ordenRecepcionObj.IdEstado = OrdenRecepcion.EstadoOrden.Abierta Or Me._ordenRecepcionObj.IdEstado = OrdenRecepcion.EstadoOrden.Parcial Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property ProductoAdicional() As Boolean
        Get
            Dim filtroComboProducto As New Estructuras.FiltroCombinacionTipoProducto
            Dim dtTipoProducto As New DataTable
            filtroComboProducto.IdTipoPrimario = CShort(OrdenRecepcionObj.IdTipoProducto)
            dtTipoProducto = CombinacionTipoProducto.ObtenerListado(filtroComboProducto)
            Return IIf(dtTipoProducto.Rows.Count > 0, True, False)
        End Get
    End Property
    Public ReadOnly Property OrdenRecepcionObj() As OrdenRecepcion
        Get
            If Me._ordenRecepcionObj Is Nothing Then
                Me._ordenRecepcionObj = New OrdenRecepcion(CLng(hfIdOrdenRecepcion.Value))
            End If
            Return Me._ordenRecepcionObj
        End Get
    End Property

    Protected Sub FiltroProducto(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroMaterial.TextChanged
        Try
            Dim dt As New DataTable
            Dim dvProducto As DataView
            Dim textoFiltro As String
            If txtFiltroMaterial.Text.Length >= 2 Then
                If OrdenRecepcionObj.IdOrdenCompra > 0 Then
                    dt = OrdenCompra.ObtenerDetalle(OrdenRecepcionObj.IdOrdenCompra)
                    textoFiltro = "producto"
                Else
                    Dim filtro As New Estructuras.FiltroProducto
                    filtro.IdTipoProducto = OrdenRecepcionObj.IdTipoProducto
                    dt = Productos.Producto.ObtenerListado(filtro)
                    textoFiltro = "nombre"
                End If

                dvProducto = New DataView(dt)
                dvProducto.RowFilter = textoFiltro & " LIKE '% " & txtFiltroMaterial.Text & " %'"
                dvProducto.Sort = textoFiltro & " asc"

                If dvProducto.Count > 0 Then

                    With ddlMaterial
                        .DataSource = dvProducto
                        .DataTextField = textoFiltro
                        .DataValueField = "idProducto"
                        .DataBind()
                        If dvProducto.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el producto", 0))
                    End With
                Else
                    inicializaDropDownList(ddlMaterial)
                End If
            Else
                inicializaDropDownList(ddlMaterial)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub inicializaDropDownList(ByRef control As DropDownList)
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem("Seleccione...", 0))
    End Sub
End Class