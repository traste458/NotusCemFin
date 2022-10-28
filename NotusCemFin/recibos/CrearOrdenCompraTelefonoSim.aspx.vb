Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports ILSBusinessLayer

Partial Public Class CrearOrdenCompraTelefonoSim
    Inherits System.Web.UI.Page

    Private idTipoProducto As Long
    Private focoArriba As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            epNotificador.clear()
            lblMensajeDetalleOrden.Text = String.Empty
            lblDetalleOrdenAdicionado.Text = String.Empty
            If Request.QueryString IsNot Nothing Then Long.TryParse(Request.QueryString("tp"), idTipoProducto)
            If Not IsPostBack Then                
                hfPerfilUsuario.Value = Session("usxp009")
                btnEditarDetalleOrden.Visible = False
                Session.Remove("dtDatosDetalleOrdenCompra")
                Session.Remove("dtDatosDetalleOrdenCompraAdicional")
                ObtenerProveedor()
                ObtenerMoneda()
                ObtenerInconterm()
                lblCantidadProducto.Visible = False
                'Carga inicial para combos de detalle de la orden
                ObtenerFabricante(idTipoProducto)
                InicializaDropDownList(ddlProducto, "Escoja un Producto")
                InicializaDropDownList(ddlProductoAdicional, "Escoja un Producto")
                InicializaDropDownList(ddlTipoUnidad, "Escoja Unidad de Empaque")
                ObtenerTipoProductoAdicional()
                btnEditarAdicionles.Visible = False                
                EnlazarDetalle(Nothing)
                EnlazarDetalleAdicional(Nothing)
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar datos del formulario. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerTipoProductoAdicional()
        Try
            Dim filtroComboProducto As New Estructuras.FiltroCombinacionTipoProducto
            Dim dtTipoProducto As New DataTable
            filtroComboProducto.IdTipoPrimario = Me.idTipoProducto
            dtTipoProducto = CombinacionTipoProducto.ObtenerListado(filtroComboProducto)
            With ddlTipoProductoAdicional
                .DataSource = dtTipoProducto
                .DataTextField = "TipoProductoAdicional"
                .DataValueField = "idTipoProductoSecundario"
                .DataBind()
            End With
            'trAccesorios.Visible = IIf(dtTipoProducto.Rows.Count > 0, True, False)
            trAccesorios.Visible = False
        Catch ex As Exception
            epNotificador.showError("Error al cargar el tipo de producto adicional. " & ex.Message)
        End Try
        ddlTipoProductoAdicional.Items.Insert(0, New ListItem("Escoja Tipo Producto", 0))
    End Sub

    Protected Sub ObtenerProveedor()
        Dim filtro As New FiltroGeneral
        Dim dtProveedor As DataTable
        Try
            filtro.Activo = EstadoBinario.Activo
            dtProveedor = Proveedor.ObtenerListado(filtro, CInt(Me.idTipoProducto))
            With ddlProveedor
                .DataSource = dtProveedor
                .DataTextField = "nombre"
                .DataValueField = "idProveedor"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de proveedores. " & ex.Message)
        End Try
        ddlProveedor.Items.Insert(0, New ListItem("Escoja un Proveedor", 0))
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
                If dt.Rows.Count <> 1 Then .Items.Insert(0, New ListItem("Escoja una Moneda", 0))
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Monedas. " & ex.Message)
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
                If dt.Rows.Count <> 1 Then .Items.Insert(0, New ListItem("Escoja un Incoterm", 0))
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Incoterms. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnCrear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrearOrden.Click
        'If Not Recibos.OrdenCompra.ExisteNumeroOrden(txtNumeroOrden.Text) Then
        If gvDetalleOrdenCompra.Rows.Count > 0 Then
            Dim miOrden As New OrdenCompra
            Try
                With miOrden
                    .NumeroOrden = txtNumeroOrden.Text.Trim()
                    .IdTipoProducto = idTipoProducto
                    .IdProveedor = ddlProveedor.SelectedValue
                    .IdMoneda = CInt(ddlMoneda.SelectedValue)
                    .IdIncoterm = ddlIncoterm.SelectedValue
                    .IdEstado = OrdenCompra.EstadoOrden.Abierta
                    .IdCreador = CInt(Session("usxp001"))
                    .Observacion = txtObservacion.Text.Trim()
                    .FechaPrevista = CDate(txtFechaPrevista.Text)
                    Dim dtDetalle As DataTable = GenerarTablaDetalle()
                    Dim dtProductoAdicional As DataTable = GenerarTablaDetalleAdicional()
                    .AdicionarDetalle(dtDetalle)
                    .AdicionarDetalle(dtProductoAdicional)
                    If .Crear() Then
                        epNotificador.showSuccess("La Orden de Compra No. <span style='color:red;'>" & txtNumeroOrden.Text & "</span> con el identificador <span style='color:red;'>" & .IdOrden.ToString() & "</span> fue creada satisfactoriamente.")
                        LimpiarTodo()
                        With cpeDetailCollapser
                            .ClientState = "false"
                            .Collapsed = False
                        End With
                    End If
                End With
            Catch ex As Exception
                epNotificador.showError("Error al tratar de crear Orden de Compra. " & ex.Message)
            End Try
        Else
            epNotificador.showWarning("Debe ingresar un detalle de orden de compra.")
        End If
        'Else
        '    epNotificador.showWarning("Ya existe una orden de compra con el número especificado. Por favor verifique")
        'End If
        ScriptManager.RegisterStartupScript(Me.Page, upGeneral.GetType(), "codModificarAltoFrame", "modificarAltoFramePadre();", True)
        ClientScript.RegisterClientScriptBlock(Me.GetType, "Subir", "subir();", True)
    End Sub

    Private Sub LimpiarTodo()
        Try
            txtNumeroOrden.Text = ""
            ddlProveedor.ClearSelection()
            ddlMoneda.ClearSelection()
            ddlIncoterm.ClearSelection()
            txtObservacion.Text = ""
            ddlFabricante.ClearSelection()
            txtCantidad.Text = ""
            txtValorUnitario.Text = ""
            txtObservacion.Text = ""
            txtFechaPrevista.Text = ""
            Session.Remove("dtDatosDetalleOrdenCompra")
            With gvDetalleOrdenCompra
                .DataSource = Nothing
                .DataBind()
            End With
            With gvProductoAdicionales
                .DataSource = Nothing
                .DataBind()
            End With
            ddlTipoProductoAdicional.ClearSelection()
            ddlProductoAdicional.ClearSelection()
            hfTotalOrdenCompra.Value = "0"
            InicializaDropDownList(ddlProducto, "Escoja un Producto")
            InicializaDropDownList(ddlTipoUnidad, "Escoja Unidad de Empaque")
        Catch ex As Exception
            epNotificador.showError("Error al tratar de limpiar los controles. " & ex.Message)
        End Try
    End Sub

    '************************************* Carga Combos detalle de la orden ********************
    Protected Sub ObtenerFabricante(ByVal idTipoProducto As Integer)
        Try
            Dim filtro As Estructuras.FiltroFabricante
            filtro.IdTipoProducto = idTipoProducto
            With ddlFabricante
                .DataSource = Fabricante.ObtenerListado(filtro)
                .DataTextField = "nombre"
                .DataValueField = "idFabricante"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el Fabricante", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar el fabricante. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerProducto(ByVal idTipoProducto As Integer)
        Try
            Dim dt As New DataTable
            Dim filtro As New FiltroProducto
            filtro.IdTipoProducto = idTipoProducto
            filtro.Activo = EstadoBinario.Activo
            If ddlFabricante.SelectedValue <> "" Then filtro.IdFabricante = CInt(ddlFabricante.SelectedValue)
            dt = Productos.Producto.ObtenerListado(filtro)
            With ddlProducto
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idProducto"
                .DataBind()
            End With
            If ddlProducto.Items.Count > 1 Then
                lblCantidadProducto.Visible = True
                lblCantidadProducto.Text = "Total productos encontrados:  " & dt.Rows.Count
            Else
                lblCantidadProducto.Visible = False
                lblCantidadProducto.Text = ""
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar el producto " & ex.Message)
        End Try
        ddlProducto.Items.Insert(0, New ListItem("Escoja el Producto", 0))
    End Sub

    Protected Sub ObtenerUnidadEmpaque(ByVal idProducto As Integer)
        Dim miProducto As Productos.Producto
        ddlTipoUnidad.Items.Clear()
        Try
            miProducto = New Productos.Producto(idProducto)
            If miProducto.IdTipoUnidad <> 0 Then
                ddlTipoUnidad.Items.Add(New ListItem(miProducto.UnidadEmpaque, miProducto.IdTipoUnidad))
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener Unidad de Empaque. " & ex.Message)
        End Try
        With ddlTipoUnidad
            If .Items.Count <> 1 Then .Items.Insert(0, New ListItem("Escoja Unidad de Empaque", "0"))
        End With
    End Sub

    Protected Sub ddlFabricante_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFabricante.SelectedIndexChanged
        Try
            InicializaDropDownList(ddlTipoUnidad, "Escoja Unidad de Empaque")
            If ddlFabricante.SelectedValue <> 0 Then
                ObtenerProducto(Me.idTipoProducto)
                With ddlProducto
                    If .Items.Count = 1 AndAlso CInt(.SelectedValue) > 0 Then ObtenerUnidadEmpaque(CInt(.SelectedValue))
                End With
            Else
                lblCantidadProducto.Visible = False
                InicializaDropDownList(ddlProducto, "Escoja un Producto")
            End If
        Catch ex As Exception
            epNotificador.showError("Error al cargar los producto para el fabricante seleccionado. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnCrearDetalleOrden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrearDetalleOrden.Click
        Dim dtDetalle As DataTable = GenerarTablaDetalle()
        Dim drDetalle As DataRow
        Dim drAux() As DataRow
        Try
            drAux = dtDetalle.Select("idProducto=" & ddlProducto.SelectedValue)
            If txtCantidad.Text > 0 Then
                If drAux.Length = 0 Then
                    drDetalle = dtDetalle.NewRow
                    drDetalle("fabricante") = ddlFabricante.SelectedItem.Text
                    drDetalle("idFabricante") = ddlFabricante.SelectedValue
                    drDetalle("producto") = ddlProducto.SelectedItem.Text
                    drDetalle("idProducto") = ddlProducto.SelectedValue
                    drDetalle("tipoUnidad") = ddlTipoUnidad.SelectedItem.Text
                    drDetalle("idTipoUnidad") = ddlTipoUnidad.SelectedValue
                    drDetalle("cantidad") = txtCantidad.Text.Trim()
                    drDetalle("valorUnitario") = txtValorUnitario.Text.Trim()
                    drDetalle("observacion") = txtObservacionDetalleOrdenCompra.Text.Trim()
                    drDetalle("idTipoDetalle") = TipoDetalleOrdenCompra.TipoDetalle.Principal
                    dtDetalle.Rows.Add(drDetalle)
                    EnlazarDetalle(dtDetalle)
                    Session("dtDatosDetalleOrdenCompra") = dtDetalle
                    LimpiarFormularioDetalle()
                    lblMensajeDetalleOrden.CssClass = "ok"
                    lblMensajeDetalleOrden.Text = "Detalle agregado"
                    ScriptManager.RegisterStartupScript(Me.Page, upGeneral.GetType(), "codAgregarDetalle", "ocultar();", True)
                    ScriptManager.RegisterStartupScript(Me.Page, upGeneral.GetType(), "codModificarAltoFrame", "modificarAltoFramePadre();", True)
                Else
                    epNotificador.showWarning("El producto seleccionado ya hace parte del detalle de la orden. Por favor verifique")
                End If
            Else
                epNotificador.showWarning("La cantidad debe ser mayor de 0. Por favor verifique")
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de adicionar Detalle a la Orden de Compra. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnAgregarAdicionales_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregarAdicionales.Click
        Dim dtDetalle As DataTable = GenerarTablaDetalleAdicional()
        Dim drDetalle As DataRow
        Dim drAux() As DataRow
        Try
            drAux = dtDetalle.Select("idProducto=" & ddlProductoAdicional.SelectedValue)
            If drAux.Length = 0 Then
                drDetalle = dtDetalle.NewRow
                Dim productoObj As New Productos.Producto(CInt(ddlProductoAdicional.SelectedValue))
                drDetalle("fabricante") = productoObj.Fabricante
                drDetalle("idFabricante") = productoObj.IdFabricante
                drDetalle("producto") = productoObj.Nombre
                drDetalle("idProducto") = productoObj.IdProducto
                drDetalle("tipoUnidad") = productoObj.UnidadEmpaque
                drDetalle("idTipoUnidad") = productoObj.IdTipoUnidad
                drDetalle("cantidad") = txtCantidadAcional.Text.Trim()
                drDetalle("valorUnitario") = 0
                drDetalle("observacion") = String.Empty
                drDetalle("idTipoDetalle") = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                dtDetalle.Rows.Add(drDetalle)
                EnlazarDetalleAdicional(dtDetalle)
                Session("dtDatosDetalleOrdenCompraAdicional") = dtDetalle
                LimpiarFormularioDetalleAdicional()
                With lblDetalleOrdenAdicionado
                    .CssClass = "ok"
                    .Text = "Producto agregado"
                End With
                ScriptManager.RegisterStartupScript(Me.Page, upProductoAdicional.GetType(), "codAgregarDetalleAdicional", "ocultarAdicional();", True)
                ScriptManager.RegisterStartupScript(Me.Page, upProductoAdicional.GetType(), "codModificarAltoFrame", "modificarAltoFramePadre();", True)
            Else
                epNotificador.showWarning("El producto seleccionado ya hace parte de los producto. Por favor verifique")
                Me.focoArriba = True
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de adicionar el producto indicado a la orden de compra. " & ex.Message)
        End Try
    End Sub


    Protected Function GenerarTablaDetalle() As DataTable
        Dim dtDatos As New DataTable
        If Session("dtDatosDetalleOrdenCompra") Is Nothing Then
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
                .Add("valorUnitario", GetType(Decimal))
                .Add("observacion", GetType(String))
                .Add("idTipoDetalle", GetType(Short))
            End With
            Dim pkKeys() As DataColumn = {dcAux}
            dtDatos.PrimaryKey = pkKeys
        Else
            dtDatos = CType(Session("dtDatosDetalleOrdenCompra"), DataTable)
        End If
        Return dtDatos
    End Function

    Protected Function GenerarTablaDetalleAdicional() As DataTable
        Dim dtDatos As New DataTable
        If Session("dtDatosDetalleOrdenCompraAdicional") Is Nothing Then
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
                .Add("valorUnitario", GetType(Decimal))
                .Add("observacion", GetType(String))
                .Add("idTipoDetalle", GetType(Short))
            End With
            Dim pkKeys() As DataColumn = {dcAux}
            dtDatos.PrimaryKey = pkKeys
        Else
            dtDatos = CType(Session("dtDatosDetalleOrdenCompraAdicional"), DataTable)
        End If
        Return dtDatos
    End Function


    Private Sub EnlazarDetalle(ByVal dtDetalle As DataTable)
        If dtDetalle IsNot Nothing Then
            Dim dvDetalle As DataView = dtDetalle.DefaultView
            dvDetalle.Sort = "producto"
            With gvDetalleOrdenCompra
                .DataSource = dvDetalle
                If dvDetalle.Count > 0 Then .Columns(0).FooterText = dvDetalle.Count.ToString & " Registro(s) Adicionado(s)"
                .DataBind()
            End With
            Dim totalOrden As Integer
            Integer.TryParse(dtDetalle.Compute("SUM(cantidad)", "").ToString, totalOrden)
            hfTotalOrdenCompra.Value = totalOrden.ToString
            MetodosComunes.mergeGridViewFooter(gvDetalleOrdenCompra)
        Else
            With gvDetalleOrdenCompra
                .DataSource = Nothing
                .DataBind()
            End With
            hfTotalOrdenCompra.Value = "0"
        End If
    End Sub

    Private Sub EnlazarDetalleAdicional(ByVal dtDetalle As DataTable)
        If dtDetalle IsNot Nothing Then
            Dim dvDetalleAdicional As DataView = dtDetalle.DefaultView
            dvDetalleAdicional.Sort = "producto"
            With gvProductoAdicionales
                .DataSource = dvDetalleAdicional
                If dvDetalleAdicional.Count > 0 Then .Columns(0).FooterText = dvDetalleAdicional.Count.ToString & " Registro(s) Adicionado(s)"
                .DataBind()
            End With
            Dim totalOrdenAdicional As Integer
            Integer.TryParse(dtDetalle.Compute("SUM(cantidad)", "").ToString, totalOrdenAdicional)

            MetodosComunes.mergeGridViewFooter(gvProductoAdicionales)
        Else
            With gvProductoAdicionales
                .DataSource = Nothing
                .DataBind()
            End With
            hfTotalOrdenCompra.Value = "0"
        End If
    End Sub

    Private Sub CargarDatosEdicionDetalle(ByVal idDetalle As Integer)
        Try
            Dim dtDetalle As DataTable = GenerarTablaDetalle()
            Dim drDetalle As DataRow
            drDetalle = dtDetalle.Rows.Find(idDetalle)
            If drDetalle IsNot Nothing Then
                With ddlFabricante
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(drDetalle("idFabricante")))
                End With
                ObtenerProducto(Me.idTipoProducto)
                Dim idProducto As Integer
                Integer.TryParse(drDetalle("idProducto").ToString, idProducto)
                With ddlProducto
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(idProducto))
                End With
                ObtenerUnidadEmpaque(idProducto)
                txtCantidad.Text = drDetalle("cantidad").ToString
                txtValorUnitario.Text = drDetalle("valorUnitario").ToString
                txtObservacionDetalleOrdenCompra.Text = drDetalle("observacion").ToString
                hfIdDetalle.Value = drDetalle("idDetalleOrden").ToString
                btnCrearDetalleOrden.Visible = False
                btnEditarDetalleOrden.Visible = True
            Else
                epNotificador.showError("Imposible recuperar la información del Detalle desde la memoria. Por favor intente nuevamente.")
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la información del detalle. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarDatosEdicionDetalleAdicional(ByVal idDetalle As Integer)
        Try
            Dim dtDetalle As DataTable = GenerarTablaDetalleAdicional()
            Dim drDetalle As DataRow

            drDetalle = dtDetalle.Rows.Find(idDetalle)
            If drDetalle IsNot Nothing Then
                Dim idProducto As Integer
                Integer.TryParse(drDetalle("idProducto").ToString, idProducto)
                Dim productoObj As New Productos.Producto(idProducto)
                ddlTipoProductoAdicional.SelectedValue = productoObj.IdTipoProducto
                CargarComboProductoAdicional(CShort(productoObj.IdTipoProducto))
                With ddlProductoAdicional
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(idProducto))
                End With
                txtCantidadAcional.Text = drDetalle("cantidad").ToString
                hfIdDetalleAdicional.Value = drDetalle("idDetalleOrden").ToString
                btnAgregarAdicionales.Visible = False
                btnEditarAdicionles.Visible = True
            Else
                epNotificador.showError("Imposible recuperar la información de los productos adicionales desde la memoria. Por favor intente nuevamente.")
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la información de los productos adicionales. " & ex.Message)
        End Try
    End Sub

    Private Sub LimpiarFormularioDetalle()
        ddlFabricante.ClearSelection()
        InicializaDropDownList(ddlProducto, "Escoja un Producto")
        InicializaDropDownList(ddlTipoUnidad, "Escoja Unidad de Empaque")
        txtCantidad.Text = ""
        txtValorUnitario.Text = ""
        txtObservacionDetalleOrdenCompra.Text = ""
        hfIdDetalle.Value = ""
        lblCantidadProducto.Visible = False
    End Sub

    Private Sub LimpiarFormularioDetalleAdicional()
        ddlTipoProductoAdicional.SelectedIndex = -1
        InicializaDropDownList(ddlProductoAdicional, "Escoja un Producto")
        txtCantidadAcional.Text = ""
        hfIdDetalleAdicional.Value = ""
        MensajeCantidadAdicional(0)
    End Sub

    Protected Sub btnEditarDetalleOrden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarDetalleOrden.Click
        EditarDetalle()
    End Sub

    Private Sub EditarDetalle()
        Dim dtDetalle As DataTable = GenerarTablaDetalle()
        Dim drDetalle As DataRow
        Dim drAux() As DataRow
        Try
            drAux = dtDetalle.Select("idProducto=" & ddlProducto.SelectedValue)
            'If drAux.Length = 0 Then
            drDetalle = dtDetalle.Rows.Find(hfIdDetalle.Value)
            If drDetalle IsNot Nothing Then
                drDetalle("fabricante") = ddlFabricante.SelectedItem.Text
                drDetalle("idFabricante") = ddlFabricante.SelectedValue
                drDetalle("producto") = ddlProducto.SelectedItem.Text
                drDetalle("idProducto") = ddlProducto.SelectedValue
                drDetalle("tipoUnidad") = ddlTipoUnidad.SelectedItem.Text
                drDetalle("idTipoUnidad") = ddlTipoUnidad.SelectedValue
                drDetalle("cantidad") = txtCantidad.Text.Trim()
                drDetalle("valorUnitario") = txtValorUnitario.Text.Trim()
                drDetalle("observacion") = txtObservacionDetalleOrdenCompra.Text.Trim()
                drDetalle("idTipoDetalle") = TipoDetalleOrdenCompra.TipoDetalle.Principal
                dtDetalle.AcceptChanges()
                EnlazarDetalle(dtDetalle)
                Session("dtDatosDetalleOrdenCompra") = dtDetalle
                LimpiarFormularioDetalle()
                btnCrearDetalleOrden.Visible = True
                btnEditarDetalleOrden.Visible = False
                lblMensajeDetalleOrden.CssClass = "ok"
                lblMensajeDetalleOrden.Text = "Detalle modificado"
                ScriptManager.RegisterStartupScript(Me.Page, upGeneral.GetType(), "codModificarDetalle", "ocultar();", True)
                'Else
                '    epNotificador.showError("Imposible recuperar la información del Detalle desde la memoria. Por favor intente nuevamente.")
            End If
            'Else
            'epNotificador.showWarning("El producto seleccionado ya hace parte del detalle de la orden. Por favor verifique")
            'End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de editar detalle. " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlProducto.SelectedIndexChanged
        Try
            Dim producto As New Productos.Producto(CInt(ddlProducto.SelectedValue))
            ObtenerUnidadEmpaque(CInt(producto.IdTipoUnidad))
        Catch ex As Exception
            epNotificador.showError("Error al cargar el tipo de unidad para el producto seleccionado. " & ex.Message)
        End Try
    End Sub

    Private Sub InicializaDropDownList(ByRef control As DropDownList, ByVal opcionInicial As String)
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem(opcionInicial, "0"))
    End Sub

    Private Sub CrearOrdenCompraGeneral_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete, upEncabezado.DataBinding
        ScriptManager.RegisterStartupScript(Me.Page, upGeneral.GetType(), "codModificarAltoFrame", "modificarAltoFramePadre();", True)
        If Me.focoArriba Then
            ScriptManager.RegisterStartupScript(Me.Page, upProductoAdicional.GetType(), "codFocoArriba", "focoArriba();", True)
        End If
    End Sub


    Protected Sub gvDetalleOrdenCompra_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetalleOrdenCompra.RowCommand
        If e.CommandName = "Editar" Then
            EditarDetalle()
            CargarDatosEdicionDetalle(e.CommandArgument)
            With cpeDetailCollapser
                .ClientState = "False"
                .Collapsed = False
            End With
        ElseIf e.CommandName = "Eliminar" Then
            Dim dtAux As DataTable = GenerarTablaDetalle()
            Dim drAux As DataRow
            Dim idDetalle As Integer
            Integer.TryParse(e.CommandArgument.ToString, idDetalle)
            drAux = dtAux.Rows.Find(idDetalle)
            If drAux IsNot Nothing Then
                dtAux.Rows.Remove(drAux)
                EnlazarDetalle(dtAux)
                Session("dtDatosDetalleOrdenCompra") = dtAux
            End If
            lblMensajeDetalleOrden.CssClass = "warning"
            lblMensajeDetalleOrden.Text = "Detalle eliminado"
            ScriptManager.RegisterStartupScript(Me.Page, upGeneral.GetType(), "codEliminarDetalle", "ocultar();", True)
            ScriptManager.RegisterStartupScript(Me.Page, upGeneral.GetType(), "codModificarAltoFrame", "modificarAltoFramePadre();", True)
        End If
    End Sub

    Protected Sub gvProductoAdicionales_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvProductoAdicionales.RowCommand
        If e.CommandName = "Editar" Then
            CargarDatosEdicionDetalleAdicional(e.CommandArgument)
        ElseIf e.CommandName = "Eliminar" Then
            Dim dtAux As DataTable = GenerarTablaDetalleAdicional()
            Dim drAux As DataRow
            Dim idDetalle As Integer
            Integer.TryParse(e.CommandArgument.ToString, idDetalle)
            drAux = dtAux.Rows.Find(idDetalle)
            If drAux IsNot Nothing Then
                dtAux.Rows.Remove(drAux)
                EnlazarDetalleAdicional(dtAux)
                Session("dtDatosDetalleOrdenCompraAdicional") = dtAux
            End If
            lblDetalleOrdenAdicionado.CssClass = "warning"
            lblDetalleOrdenAdicionado.Text = "Detalle eliminado"
            ScriptManager.RegisterStartupScript(Me.Page, upProductoAdicional.GetType(), "codEliminarDetalleAdicional", "ocultarAdicional();", True)
            ScriptManager.RegisterStartupScript(Me.Page, upProductoAdicional.GetType(), "codModificarAltoFrame", "modificarAltoFramePadre();", True)
        End If
    End Sub

    Protected Sub ddlTipoProductoAdicional_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoProductoAdicional.SelectedIndexChanged
        Try
            CargarComboProductoAdicional(CShort(ddlTipoProductoAdicional.SelectedValue))
        Catch ex As Exception
            epNotificador.showError("Error al cargar el producto adicional. " & ex.Message)
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
            InicializaDropDownList(ddlProductoAdicional, "Escoja un Producto")
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



    Protected Sub btnEditarAdicionles_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarAdicionles.Click
        Dim dtDetalle As DataTable = GenerarTablaDetalleAdicional()
        Dim drDetalle As DataRow
        Dim drAux() As DataRow
        Dim productoObj As New Productos.Producto(CInt(ddlProductoAdicional.SelectedValue))
        Try
            drAux = dtDetalle.Select("idProducto=" & productoObj.IdProducto)
            drDetalle = dtDetalle.Rows.Find(hfIdDetalleAdicional.Value)
            If drDetalle IsNot Nothing Then
                If CInt(drDetalle("idProducto")) <> productoObj.IdProducto Then
                    If drAux.Length = 0 Then
                        EditarDetalleAdicional(productoObj)
                    Else
                        epNotificador.showWarning("El producto seleccionado ya hace parte de los productos. Por favor verifique")
                    End If
                Else
                    EditarDetalleAdicional(productoObj)
                End If
            Else
                epNotificador.showError("Imposible recuperar la información del Detalle desde la memoria. Por favor intente nuevamente.")
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de editar el producto adicionaldtDatosDetalleOrdenCompraAdicional. " & ex.Message)
        End Try
    End Sub

    Protected Sub EditarDetalleAdicional(ByVal productoObj As Productos.Producto)
        Dim dtDetalle As DataTable = GenerarTablaDetalleAdicional()
        Dim drDetalle As DataRow
        drDetalle = dtDetalle.Rows.Find(hfIdDetalleAdicional.Value)
        drDetalle("fabricante") = productoObj.Fabricante
        drDetalle("idFabricante") = productoObj.IdFabricante
        drDetalle("producto") = productoObj.Nombre
        drDetalle("idProducto") = productoObj.IdProducto
        drDetalle("tipoUnidad") = productoObj.UnidadEmpaque
        drDetalle("idTipoUnidad") = productoObj.IdTipoUnidad
        drDetalle("cantidad") = txtCantidadAcional.Text.Trim()
        drDetalle("valorUnitario") = 0
        drDetalle("observacion") = String.Empty
        drDetalle("idTipoDetalle") = TipoDetalleOrdenCompra.TipoDetalle.Secundario
        dtDetalle.AcceptChanges()
        EnlazarDetalleAdicional(dtDetalle)
        Session("dtDatosDetalleOrdenCompraAdicional") = dtDetalle
        LimpiarFormularioDetalleAdicional()
        btnAgregarAdicionales.Visible = True
        btnEditarAdicionles.Visible = False
        With lblDetalleOrdenAdicionado
            .CssClass = "ok"
            .Text = "Producto modificado"
        End With
        ScriptManager.RegisterStartupScript(Me.Page, upProductoAdicional.GetType(), "codModificarDetalleAdicional", "ocultarAdicional();", True)
    End Sub



End Class