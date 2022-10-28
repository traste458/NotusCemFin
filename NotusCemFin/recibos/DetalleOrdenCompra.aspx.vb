Imports ILSBusinessLayer

Partial Public Class DetalleOrdenCompra
    Inherits System.Web.UI.Page
    Public detalleOrdenFactura As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            EncabezadoPagina.clear()
            txtFiltroCiudad.Attributes.Add("onkeyup", "RefrescaUpdatePanel('" & txtFiltroCiudad.ClientID & "','" & hfFlagFiltrado.ClientID & "')")
            If Not IsPostBack Then
                'Session("usxp001") = 1
                hfIdDetalleOrdenCompra.Value = CInt(Request.QueryString("doc"))
                'hfIdDetalleOrdenCompra.Value = 448
                Dim detalleOC As New Recibos.DetalleOrdenCompra(CLng(hfIdDetalleOrdenCompra.Value))
                EncabezadoPagina.setTitle("Administración de Facturas")
                EncabezadoPagina.showReturnLink("~/recibos/BuscarOrdenCompra.aspx")
                hfIdOrdenCompra.Value = detalleOC.IdOrden
                CargarInfoOrden(detalleOC.IdOrden)
                ObtenerPais()
                InicializaDropDownList(ddlCiudadCompra)
                'ObtenerCiudadCompra()
                CargarDatosDetalleOrdenCompra()
                ObtenerInfoFactura()
                VerificarCantidadesFacturas()
                txtFiltroCiudad.Enabled = False
            End If

        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Private Sub CargarInfoOrden(ByVal idOrden As Long)
        Try
            Dim ordenCompra As New Recibos.OrdenCompra(idOrden)
            With ordenCompra
                lblIdOrden.Text = .IdOrden.ToString()
                lblNumeroOrden.Text = .NumeroOrden
                lblProveedor.Text = .Proveedor
                lblMoneda.Text = .Moneda
                lblIncoterm.Text = .Incoterm
                lblObservacionOrden.Text = .Observacion
                lblEstado.Text = .Estado
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar la informacion de la orden. " & ex.Message)
        End Try
    End Sub

    Private Function CargarDatosDetalleOrdenCompra() As Recibos.DetalleOrdenCompra
        Try
            Dim detalleOrCompra As New Recibos.DetalleOrdenCompra(CLng(hfIdDetalleOrdenCompra.Value))
            With detalleOrCompra
                lblFabricante.Text = .Fabricante
                lblProducto.Text = .Producto
                lblCantidad.Text = .Cantidad.ToString
                lblFechaRegistro.Text = .FechaRegistro.ToString
                lblObservacion.Text = .Observacion.ToString
                lblValorUnitario.Text = .ValorUnitario.ToString("C")
            End With
            Return detalleOrCompra
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los datos del detalle de la orden de compra. " & ex.Message)
        End Try
    End Function

    Private Sub ObtenerInfoFactura()
        Try
            Dim filtro As Estructuras.FiltroInfoFactura
            Dim dt As New DataTable
            dt = EstructuraDtFacturasAgregadas()
            filtro.IdDetalleOrdenCompra = CInt(hfIdDetalleOrdenCompra.Value)
            dt = Recibos.InfoFactura.ObtenerListado(filtro)
            gvFacturasAgregadas.DataSource = dt
            Session("dtDatosFacturaDetalleOC") = dt
            gvFacturasAgregadas.DataBind()
            gvFacturasAgregadas.Columns(0).Visible = False
            If gvFacturasAgregadas.Rows.Count > 0 Then
                pnlFacturasAgregadas.Visible = True
            Else
                pnlFacturasAgregadas.Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar las Ciudades. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerPais()
        Dim filro As Estructuras.FiltroPais
        Try
            With ddlPais
                .DataSource = Localizacion.Pais.ObtenerListado(filro)
                .DataTextField = "nombre"
                .DataValueField = "idPais"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el Pais", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los Paises. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerPaisGuia()
        Dim filro As Estructuras.FiltroPais
        Try
            With ddlPaisFacGuia
                .DataSource = Localizacion.Pais.ObtenerListado(filro)
                .DataTextField = "nombre"
                .DataValueField = "idPais"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el Pais", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los Paises. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerCiudadCompra(Optional ByVal filtroCarga As Integer = 0)
        Dim filro As Estructuras.FiltroCiudad
        Dim dtCiudad As New DataTable
        filro.Activo = 1
        filro.IdPais = CShort(ddlPais.SelectedValue)
        Try
            With ddlCiudadCompra
                If filtroCarga = 0 Then
                    dtCiudad = Localizacion.Ciudad.ObtenerListado(filro)
                    Session("dtCiudad") = dtCiudad
                Else : dtCiudad = CType(Session("dtCiudad"), DataTable)
                End If
                .DataSource = dtCiudad
                .DataTextField = "nombre"
                .DataValueField = "idCiudad"
                .DataBind()
                If dtCiudad.DefaultView.Count = 0 Then
                    .Items.Insert(0, New ListItem("No existen ciudades", 0))
                ElseIf dtCiudad.DefaultView.Count > 1 Then
                    .Items.Insert(0, New ListItem("Escoja la Ciudad", 0))
                End If

            End With
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de cargar las Ciudades. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnCrear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrear.Click
        Try
            EncabezadoAgregarFactura.clear()
            Dim resultado As Boolean = True
            Dim detalleOrCompra As New Recibos.DetalleOrdenCompra()
            detalleOrCompra = CargarDatosDetalleOrdenCompra()
            If ddlCiudadCompra.SelectedValue > 0 Then
                Dim infOrdenCompra As New Recibos.OrdenCompra(detalleOrCompra.IdOrden)
                If Not Recibos.InfoFactura.ExisteFactura(txtFactura.Text, infOrdenCompra.IdProveedor) Then
                    Dim cantidadFacturas As Integer = Recibos.InfoFactura.CantidadEnFactura(CInt(hfIdDetalleOrdenCompra.Value))
                    If CInt(txtCantidad.Text) <= CantidadPermitida() Then
                        Dim factura As New Recibos.InfoFactura
                        With factura
                            .IdDetalleOrdenCompra = CLng(hfIdDetalleOrdenCompra.Value)
                            .Factura = txtFactura.Text.Trim
                            .Cantidad = CInt(txtCantidad.Text)
                            .IdCiudadCompra = CInt(ddlCiudadCompra.SelectedValue)
                            .IdEstado = CLng(16)
                            Long.TryParse(Session("usxp001").ToString(), .IdUsuario)
                            If .Crear() Then
                                CargarDtFacturas(factura)
                            End If
                        End With                        
                    Else
                        resultado = False
                        EncabezadoAgregarFactura.showWarning("La cantidad es mayor a la cantidad del detalle de la orden. Cantidad max. permitida " & CantidadPermitida())
                        'mpeAgregarFactura.Show()
                        txtFactura.Focus()
                        dlgInfoFactura.Show()
                    End If
                    If resultado Then
                        ObtenerInfoFactura()
                        EncabezadoPagina.showSuccess("Factura creada con exito.")
                        LimpiarFormularioCrearFactura()
                        'mpeAgregarFactura.Hide()  
                        VerificarCantidadesFacturas()
                    End If

                Else
                    EncabezadoAgregarFactura.showWarning("La factura " & txtFactura.Text & " del proveedor: " & infOrdenCompra.Proveedor & " ya existe.")
                    'mpeAgregarFactura.Show()
                    dlgInfoFactura.Show()
                End If
            Else
                rfvCiudad.IsValid = False
                'mpeAgregarFactura.Show()
                dlgInfoFactura.Show()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al crear la factura. " & ex.Message)
        Finally
            ObtenerInfoFactura()
            MensajeCantidadDisponible()
            cpFacturasAgregadas.Update()
        End Try
    End Sub

    Protected Sub btnEditarFactura_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarFactura.Click
        Try
            EncabezadoAgregarFactura.clear()
            Dim detalleOrCompra As New Recibos.DetalleOrdenCompra()
            detalleOrCompra = CargarDatosDetalleOrdenCompra()
            Dim facturaEdicionActual As New Recibos.InfoFactura(CLng(hfIdFacturaEdicionActual.Value))
            Dim cantidadEnFactura As Integer = Recibos.InfoFactura.CantidadEnFactura(facturaEdicionActual.IdFactura)
            'cantidadFacturas -= facturaEdicionActual.Cantidad
            'If cantidadFacturas + CInt(txtCantidad.Text) <= detalleOrCompra.Cantidad Then


            Dim cantidadMinima As Integer = Recibos.FacturaGuia.CantidadPorFactura(CLng(hfIdFacturaEdicionActual.Value))            

            If CInt(txtCantidad.Text) <= CantidadPermitida() Then
                If cantidadMinima > 0 Then
                    If CInt(txtCantidad.Text) <= cantidadMinima Then
                        ActualizarFactura()
                    Else
                        EncabezadoAgregarFactura.showWarning("La cantidad es menor a la suma de cantidades de guias adicionadas. Cantidad min. permitida " & cantidadMinima.ToString())
                        'mpeAgregarFactura.Show()
                        dlgInfoFactura.Show()
                    End If
                Else
                    ActualizarFactura()
                End If
            Else
                EncabezadoAgregarFactura.showWarning("La cantidad es mayor a la cantidad del detalle de la orden. Cantidad max. permitida " & CantidadPermitida())
                'mpeAgregarFactura.Show()
                dlgInfoFactura.Show()
            End If
        Catch ex As Exception
            EncabezadoAgregarFactura.showError("Error al tratar de editar la factura. " & ex.Message)
        Finally
            MensajeCantidadDisponible()
            cpFacturasAgregadas.Update()
        End Try
    End Sub

    Public Function ActualizarFactura()
        Dim resultado As ResultadoProceso
        Try
            Dim facturaEdicionActual As New Recibos.InfoFactura(CLng(hfIdFacturaEdicionActual.Value))
            With facturaEdicionActual
                .IdDetalleOrdenCompra = CLng(hfIdDetalleOrdenCompra.Value)
                .Factura = txtFactura.Text.Trim
                .Cantidad = CInt(txtCantidad.Text)
                .IdCiudadCompra = CInt(ddlCiudadCompra.SelectedValue)
                resultado = .Actualizar()
                If resultado.Valor = 0 Then
                    ObtenerInfoFactura()
                    EncabezadoPagina.showSuccess("Factura Actualizada")
                Else
                    Select Case resultado.Valor
                        Case 1
                            EncabezadoPagina.showWarning(resultado.Mensaje)
                        Case Else
                            EncabezadoPagina.showError(resultado.Mensaje)
                    End Select
                End If
                'mpeAgregarFactura.Hide()
            End With
            VerificarCantidadesFacturas()
        Catch ex As Exception
            EncabezadoAgregarFactura.showWarning("Error al actualizar la factura indicada. " & ex.Message)
            'mpeAgregarFactura.Show()
        End Try
    End Function

    Private Sub VerificarCantidadesFacturas()
        Try
            'Dim detalleOrCompra As New Recibos.DetalleOrdenCompra()
            'detalleOrCompra = CargarDatosDetalleOrdenCompra()
            'Dim cantidadFacturas As Integer = Recibos.InfoFactura.CantidadEnFactura(CInt(hfIdDetalleOrdenCompra.Value))
            'If cantidadFacturas = detalleOrCompra.Cantidad Then
            '    pnlBotonAgregarFactura.Visible = False                                
            'ElseIf cantidadFacturas < detalleOrCompra.Cantidad Then
            '    pnlBotonAgregarFactura.Visible = True                
            'End If
            Dim detalleOrdenObj As New Recibos.DetalleOrdenCompra(CLng(hfIdDetalleOrdenCompra.Value))
            If detalleOrdenObj.PosibleAdicionarFactura Then
                pnlBotonAgregarFactura.Visible = True
                pnlInfoEstadoDetalleOrden.Visible = False
            Else
                pnlBotonAgregarFactura.Visible = False
                pnlInfoEstadoDetalleOrden.Visible = True
                hfInformacionEstadoDetalleOrden.Value = detalleOrdenObj.MensajeInfo
            End If

        Catch ex As Exception
            EncabezadoPagina.showError("Error al verificar las cantidades de facturas. " & ex.Message)
        End Try
    End Sub

    Private Function CantidadAgregadaEnFacturas() As Integer
        Return Recibos.InfoFactura.CantidadEnFactura(CInt(hfIdDetalleOrdenCompra.Value))
    End Function

    Private Function CantidadPermitida() As Integer
        Try
            Dim detalleOrCompra As New Recibos.DetalleOrdenCompra()
            Dim facturaEdicionActual As Recibos.InfoFactura
            Dim cantidaActual As Integer
            Dim idFactura As Integer
            detalleOrCompra = CargarDatosDetalleOrdenCompra()
            Dim cantidadFacturas As Integer = Recibos.InfoFactura.CantidadEnFactura(CInt(hfIdDetalleOrdenCompra.Value))
            Integer.TryParse(hfIdFacturaEdicionActual.Value.ToString(), idFactura)
            If idFactura > 0 Then
                facturaEdicionActual = New Recibos.InfoFactura(CLng(hfIdFacturaEdicionActual.Value))
                cantidaActual = facturaEdicionActual.Cantidad
            End If
            Return detalleOrCompra.Cantidad - cantidadFacturas + cantidaActual
        Catch ex As Exception
            EncabezadoPagina.showError("Error al obtener la cantidad permitida. " & ex.Message)
        End Try
    End Function

    Private Sub LimpiarDatosDetalleOrden()
        txtFactura.Text = String.Empty
        txtCantidad.Text = String.Empty
        InicializaDropDownList(ddlCiudadCompra)
    End Sub

    Protected Sub CargarDtFacturas(ByVal factura As Recibos.InfoFactura)
        Dim dt As New DataTable
        Dim dr As DataRow
        dt = EstructuraDtFacturasAgregadas()
        dr = dt.NewRow
        dr("idFactura") = factura.IdFactura
        dr("idDetalleOrdenCompra") = hfIdDetalleOrdenCompra.Value
        dr("factura") = txtFactura.Text
        dr("cantidad") = txtCantidad.Text
        dr("CiudadCompra") = ddlCiudadCompra.SelectedItem
        dr("idCiudadCompra") = ddlCiudadCompra.SelectedValue
        dr("idUsuario") = CInt(Session("usxp001"))
        dr("fechaRegistro") = factura.fechaRegistro.ToString
        dt.Rows.InsertAt(dr, 0)
        gvFacturasAgregadas.DataSource = dt
        gvFacturasAgregadas.DataBind()
        gvFacturasAgregadas.Columns(0).Visible = False
        dt.AcceptChanges()
        Session("dtDatosFacturaDetalleOC") = dt
        LimpiarDatosDetalleOrden()

    End Sub

    Protected Function EstructuraDtFacturasAgregadas() As DataTable
        Dim dtDatos As DataTable
        If Session("dtDatosFacturaDetalleOC") Is Nothing Then
            dtDatos = New DataTable
            'Dim dc As New DataColumn("idFactura", GetType(Integer))
            'dc.AutoIncrement = True
            'dc.AutoIncrementSeed = 1
            'dtDatos.Columns.Add(dc)
            dtDatos.Columns.Add("idFactura")
            dtDatos.Columns.Add("idDetalleOrdenCompra")
            dtDatos.Columns.Add("factura")
            dtDatos.Columns.Add("cantidad")
            dtDatos.Columns.Add("CiudadCompra")
            dtDatos.Columns.Add("idCiudadCompra")
            dtDatos.Columns.Add("idUsuario")
            dtDatos.Columns.Add("fechaRegistro")
        Else
            dtDatos = CType(Session("dtDatosFacturaDetalleOC"), DataTable)
        End If
        Return dtDatos
    End Function

    Protected Sub gvFacturasAgregadas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvFacturasAgregadas.RowCommand
        Try
            If e.CommandName = "agregarFactura" Then
                hfIdFacturaEdicionActual.Value = e.CommandArgument.ToString
                PrepararModalAgregarGuia()
                dlgInfoGuia.Show()
            ElseIf e.CommandName = "editar" Then
                hfIdFacturaEdicionActual.Value = e.CommandArgument.ToString
                LimpiarFormularioEditarFactura()
                'mpeAgregarFactura.Show()
                dlgInfoFactura.Show()
            ElseIf e.CommandName = "eliminar" Then
                Dim idFactura As Long = CLng(e.CommandArgument)
                Dim infoFacturaGuia As New Recibos.FacturaGuia()
                infoFacturaGuia.IdFactura = idFactura
                infoFacturaGuia.Eliminar()
                Dim factura As New Recibos.InfoFactura(idFactura)
                factura.Eliminar()
                EncabezadoPagina.showSuccess("Factura eliminada correctamente.")
                ObtenerInfoFactura()
                VerificarCantidadesFacturas()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al realizar la operación indicada. " & ex.Message)
        End Try
    End Sub

    Private Sub PrepararModalAgregarGuia()
        ddlTransportadora.Enabled = True
        txtNoGuia.Enabled = True
        txtCantidadFacGuia.Enabled = True
        txtNoGuia.Text = String.Empty
        txtCantidadFacGuia.Text = String.Empty
        ObtenerPaisGuia()
        CargarDataGuia()
        Dim facturaActual As New Recibos.InfoFactura(CLng(hfIdFacturaEdicionActual.Value))
        Dim dtGuias As New DataTable
        dtGuias = ObtenerGuiaDeFactura(CInt(facturaActual.IdFactura))
        If dtGuias.Rows.Count > 0 Then
            ddlTransportadora.SelectedValue = CInt(dtGuias.Rows(0)("idTransportador"))
        End If        
        dlgInfoGuia.HeaderHtml = "<b>Informacion de la Guia para la Factura: " + facturaActual.Factura + "</b>"
        EncabezadoFacGuia.clear()
        btnConsultar.Visible = True
        tblContenidoGuia.Visible = False
        btnCancelarAdicionGuia.Visible = False
        btnCrearGuia.Visible = False
        pnlAdicionarGuia.Visible = False
        LimpiarCajasGuia()
        'mpeAgregarGuia.Show()
        CantidadPermitidaPorFacturaMensaje(facturaActual.IdFactura)
    End Sub

    Private Sub CargarDataGuia()
        obtenerTransportadora()
        InicializaDropDownList(ddlCiudadOrigen)
        'obtenerCiudadOrigen()
    End Sub

    Protected Sub obtenerTransportadora()
        Dim filtroTransportadoras As Estructuras.FiltroTransportadora

        filtroTransportadoras.Activo = Enumerados.EstadoBinario.Activo

        Try
            With ddlTransportadora
                .DataSource = Transportadora.ListadoTransportadoras(filtroTransportadoras)
                .DataTextField = "transportadora"
                .DataValueField = "idTransportadora"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja la Transportadora", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar la transportadora" & ex.Message)
        End Try
    End Sub

    Protected Sub obtenerCiudadOrigen()
        Dim filtro As Estructuras.FiltroCiudad
        filtro.Activo = 1
        filtro.IdPais = CShort(ddlPaisFacGuia.SelectedValue)
        Try
            With ddlCiudadOrigen
                .DataSource = Localizacion.Ciudad.ObtenerListado(filtro)
                .DataTextField = "nombre"
                .DataValueField = "idCiudad"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja la Ciudad", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar las Ciudades" & ex.Message)
        End Try
    End Sub

    Protected Sub btnCrearGuia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrearGuia.Click
        Dim db As New LMDataAccessLayer.LMDataAccess

        Try
            Dim factura As New Recibos.InfoFactura(CLng(hfIdFacturaEdicionActual.Value))
            If CInt(txtCantidadFacGuia.Text) > CantidadPermitidaPorFactura(factura.IdFactura) Then
                EncabezadoFacGuia.showWarning("Esta cantidad supera la cantidad de la factura. Cantidad maxima permitida " + CantidadPermitidaPorFactura(factura.IdFactura).ToString())
                'mpeAgregarGuia.Show()
                dlgInfoGuia.Show()
            Else
                db.iniciarTransaccion()
                Dim facturaActual As New Recibos.InfoFactura(CLng(hfIdFacturaEdicionActual.Value))
                Dim DetalleOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(facturaActual.IdDetalleOrdenCompra))
                Dim guia As New Recibos.InfoGuia()
                With guia
                    .IdOrdenCompra = DetalleOrdenCompra.IdOrden
                    .Guia = txtNoGuia.Text.Trim()
                    .IdTransportador = ddlTransportadora.SelectedValue
                    .IdCiudadOrigen = ddlCiudadOrigen.SelectedValue
                    .FechaSalida = CDate(dpFechaSalida.SelectedDates.ToString())
                    .FechaEsperadaArribo = CDate(dpFechaEsperaArribo.SelectedDates.ToString())
                    .IdEstado = 16
                    .PesoNeto = txtPesoNeto.Text.Trim()
                    '.PesoBruto = txtPesoBruto.Text.Trim()
                    .IdUsuario = CLng(Session("usxp001"))
                    If .Crear() Then
                        Dim facturaGuia As New Recibos.FacturaGuia()
                        facturaGuia.IdFactura = facturaActual.IdFactura
                        facturaGuia.IdGuia = .IdGuia
                        facturaGuia.Cantidad = txtCantidadFacGuia.Text.Trim()
                        facturaGuia.Crear()
                        EncabezadoPagina.showSuccess("Guia Creada con exito.")
                        LimpiarCajasGuia()
                        'mpeAgregarGuia.Hide()
                    End If
                End With
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al crear la guia. " & ex.Message)
            db.abortarTransaccion()
        Finally
            'db.confirmarTransaccion()
            ObtenerInfoFactura()
            cpFacturasAgregadas.Update()
        End Try

    End Sub

    Private Sub LimpiarCajasGuia()
        txtNoGuia.Text = String.Empty
        ddlTransportadora.ClearSelection()
        ddlCiudadOrigen.ClearSelection()
        ddlPaisFacGuia.ClearSelection()
        txtCantidadFacGuia.Text = String.Empty
        dpFechaSalida.SelectedDates.Clear()
        dpFechaEsperaArribo.SelectedDates.Clear()
        'txtFechaSalida.Text = String.Empty

        'txtFechaEsperadaArribo.Text = String.Empty
        txtPesoNeto.Text = String.Empty
        'txtPesoBruto.Text = String.Empty
        'hfIdDetalleOrdenCompra.Value = String.Empty
    End Sub

    Protected Sub gvFacturasAgregadas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFacturasAgregadas.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ordenCompraObj As New Recibos.OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim detalleOrdenObj As New Recibos.DetalleOrdenCompra(CLng(hfIdDetalleOrdenCompra.Value))
            Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim idFactura = CInt(fila("idFactura"))
            Dim Factura As New Recibos.InfoFactura(CLng(idFactura))
            Dim dtDetalle As New DataTable
            Dim imgShow As System.Web.UI.WebControls.Image = e.Row.FindControl("imgShow")
            Dim btnEditarFactura As ImageButton = e.Row.FindControl("imgBtnEditarFatura")
            Dim btnAgregarFactura As ImageButton = e.Row.FindControl("imbAgregarFactura")
            Dim btnEliminarFactura As ImageButton = e.Row.FindControl("imgBtnEliminarFactura")
            Dim btnDetalleFactura As ImageButton = e.Row.FindControl("imgBtnDetalleFactura")
            Dim hfInfoEstadoFactura As HiddenField = CType(e.Row.FindControl("hfInfoEstadoFactura"), HiddenField)
            Dim listaGuias As BulletedList = e.Row.FindControl("bltGuias")
            dtDetalle = ObtenerGuiaDeFactura(idFactura)
            If dtDetalle.Rows.Count > 0 Then
                'Dim gvr As New GridViewRow(-1, e.Row.DataItemIndex, DataControlRowType.DataRow, DataControlRowState.Normal)
                'Dim imgEditarOrdenCompra As ImageButton = e.Row.FindControl("imgEditarOrdenCompra")
                'gvr.Cells.Add(crearCelda(dtDetalle))
                listaGuias.DataSource = dtDetalle
                listaGuias.DataBind()
                'Dim tabla As Table = CType(e.Row.Parent, Table)
                btnDetalleFactura.Visible = True
                btnDetalleFactura.PostBackUrl = "DetalleFacturaOrdenCompra.aspx?idfactura=" & idFactura.ToString
                'tabla.Rows.Add(gvr)
                'imgShow.Visible = True
            Else
                btnDetalleFactura.Visible = False
                imgShow.Visible = False
            End If
            If ordenCompraObj.IdEstado = Recibos.OrdenCompra.EstadoOrden.Cancelada Or ordenCompraObj.IdEstado = Recibos.OrdenCompra.EstadoOrden.Finalizada Then
                'btnEditarFactura.Visible = False
                btnAgregarFactura.Visible = False
                'btnEliminarFactura.Visible = False
            Else
                Dim dtPreinsCliente As New DataTable
                Dim filtroPreinstruccion As New Estructuras.FiltroPreinstruccionCliente
                filtroPreinstruccion.IdFactura = idFactura
                filtroPreinstruccion.noAnulada = 1
                'dtPreinsCliente = OMS.PreinstruccionCliente.ObtenerListado(filtroPreinstruccion)
                If Factura.IdEstado <> 16 Then
                    'btnEditarFactura.Visible = False
                    btnAgregarFactura.Visible = False
                    'btnEliminarFactura.Visible = False
                Else
                    'btnEditarFactura.Visible = True
                    btnAgregarFactura.Visible = True
                    'btnEliminarFactura.Visible = True
                End If
            End If


            If Not detalleOrdenObj.PosibleEliminarFactura(idFactura) Then
                btnEliminarFactura.ImageUrl = "~/images/Info-32.png"
                btnEliminarFactura.ToolTip = "Información de la factura"
                hfInfoEstadoFactura.Value = detalleOrdenObj.MensajeInfo
            End If


        End If
    End Sub

    Private Function ObtenerGuiaDeFactura(ByVal idFactura As Integer) As DataTable
        Try
            Dim filtro As Estructuras.FiltroInfoGuia
            Dim dtRetorno As New DataTable
            filtro.IdFactura = idFactura
            dtRetorno = Recibos.InfoGuia.ObtenerListado(filtro)
            Return dtRetorno
        Catch ex As Exception
            EncabezadoPagina.showError("Error al traer las guias para la factura indicada. " & ex.Message)
        End Try
    End Function

    Private Function crearCelda(ByVal dtDetalle As DataTable) As TableCell
        Dim celda As New TableCell()
        Dim grillaDatos As New System.Web.UI.WebControls.GridView
        grillaDatos.AutoGenerateColumns = False
        Dim columna1 As New BoundField
        columna1.HeaderText = "Guia"
        columna1.DataField = "guia"
        Dim columna2 As New BoundField
        columna2.HeaderText = "Fecha Salida"
        columna2.DataField = "fechaSalida"
        Dim columna3 As New BoundField
        columna3.HeaderText = "Fecha Esperada Arribo"
        columna3.DataField = "fechaEsperadaArribo"
        Dim columna4 As New BoundField
        columna4.HeaderText = "Peso Neto"
        columna4.DataField = "pesoNeto"
        Dim columna5 As New BoundField
        columna5.HeaderText = "Peso Bruto"
        columna5.DataField = "pesoBruto"
        grillaDatos.Columns.Add(columna1)
        grillaDatos.Columns.Add(columna2)
        grillaDatos.Columns.Add(columna3)
        grillaDatos.Columns.Add(columna4)
        grillaDatos.Columns.Add(columna5)
        'Dim columna6 As New HyperLinkField
        'Dim columna7 As New HyperLinkField
        'With columna6
        '    .HeaderText = "Facturas"
        '    .Text = "Facturas"
        '    .NavigateUrl = "DetalleOrdenCompra.aspx"
        '    .DataNavigateUrlFields = New String() {"idDetalle"}
        '    .DataNavigateUrlFormatString = "DetalleOrdenCompra.aspx?doc={0}"
        'End With

        'With columna7
        '    .HeaderText = "Editar"
        '    .Text = "Editar"
        '    .NavigateUrl = "EditarDetalleOrdenCompra.aspx"
        '    .DataNavigateUrlFields = New String() {"idDetalle"}
        '    .DataNavigateUrlFormatString = "EditarDetalleOrdenCompra.aspx?doc={0}"
        'End With

        'grillaDatos.Columns.Add(columna6)
        'grillaDatos.Columns.Add(columna7)


        grillaDatos.DataSource = dtDetalle
        grillaDatos.DataBind()
        If dtDetalle.Rows.Count > 0 Then celda.CssClass = "DetalleFactura"
        celda.ColumnSpan = gvFacturasAgregadas.Columns.Count
        celda.Controls.Add(grillaDatos)
        Return celda
    End Function

    Protected Sub ddlPais_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPais.SelectedIndexChanged
        Try
            dlgInfoFactura.Show()
            txtFiltroCiudad.Enabled = True
            lblIngreseCiudad.Visible = True
            ObtenerCiudadCompra()

        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de filtrar datos. " & ex.Message)
        End Try

    End Sub

    Protected Sub imgBtnCerrarPnlAgregarFactura_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            LimpiarDatosDetalleOrden()
            ObtenerInfoFactura()
            'mpeAgregarFactura.Hide()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar cerrar la ventana. " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlPaisFacGuia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPaisFacGuia.SelectedIndexChanged
        Try
            obtenerCiudadOrigen()
            'mpeAgregarGuia.Show()
            dlgInfoGuia.Show()
        Catch ex As Exception
            EncabezadoFacGuia.showError("Error al cargar los paises. " & ex.Message)
        End Try
    End Sub

    Private Sub InicializaDropDownList(ByRef control As DropDownList)
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem("Seleccione...", 0))
    End Sub


    Protected Sub BtnCancelarAdicion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCancelarAdicion.Click
        PrepararModalAgregarGuia()
        'mpeAgregarGuia.Hide()        
        dlgInfoGuia.Show()
    End Sub

    Protected Sub BtnAdicionarGuia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnAdicionarGuia.Click
        Try
            Dim guia As New Recibos.InfoGuia(CLng(ddlTransportadora.SelectedValue), txtNoGuia.Text)

            If guia.IdGuia > 0 Then
                Dim infoFacturaGuia As New Recibos.FacturaGuia
                Dim facturaAcutualObj As New Recibos.InfoFactura(CLng(hfIdFacturaEdicionActual.Value))
                infoFacturaGuia.IdFactura = CLng(hfIdFacturaEdicionActual.Value)
                infoFacturaGuia.IdGuia = CLng(guia.IdGuia)
                Integer.TryParse(txtCantidadGuiaExistente.Text, infoFacturaGuia.Cantidad)                
                infoFacturaGuia.Crear()
                'mpeAgregarGuia.Hide()
                EncabezadoPagina.showSuccess("Guia agregada correctamente.")
            Else
                EncabezadoFacGuia.showError("Error al agregar la guia. No existe. ")
                'mpeAgregarGuia.Show()
                dlgInfoGuia.Show()
            End If
        Catch ex As Exception
            EncabezadoFacGuia.showError("Error al agregar la guia. " & ex.Message)
            'mpeAgregarGuia.Show()
            dlgInfoGuia.Show()
        Finally
            ObtenerInfoFactura()            
            cpFacturasAgregadas.Update()
        End Try

    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultar.Click
        Try
            EncabezadoFacGuia.clear()
            Dim guia As New Recibos.InfoGuia(CInt(ddlTransportadora.SelectedValue), txtNoGuia.Text)
            Dim infoFacturaGuia As New Recibos.FacturaGuia
            Dim factura As New Recibos.InfoFactura(CLng(hfIdFacturaEdicionActual.Value))
            If CLng(guia.IdGuia) > 0 Then infoFacturaGuia = New Recibos.FacturaGuia(CLng(hfIdFacturaEdicionActual.Value), CLng(guia.IdGuia))

            If infoFacturaGuia.IdFacturaGuia > 0 Then
                EncabezadoFacGuia.showWarning("Esta guia ya se encuentra agregada en esta factura")
                'mpeAgregarGuia.Show()
                dlgInfoGuia.Show()
            Else
                btnConsultar.Visible = False
                If guia.IdGuia > 0 Then
                    pnlAdicionarGuia.Visible = True
                    ddlTransportadora.Enabled = False
                    txtNoGuia.Enabled = False
                    'mpeAgregarGuia.Show()
                    dlgInfoGuia.Show()
                Else
                    ddlTransportadora.Enabled = False
                    txtNoGuia.Enabled = False
                    txtCantidadFacGuia.Enabled = True
                    btnCrearGuia.Visible = True
                    btnCancelarAdicionGuia.Visible = True
                    tblContenidoGuia.Visible = True
                    'mpeAgregarGuia.Show()
                    dlgInfoGuia.Show()
                End If
            End If
        Catch ex As Exception
            EncabezadoFacGuia.showError("Error al realizar la consulta de esta guia. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnCancelarAdicionGuia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelarAdicionGuia.Click
        Try
            PrepararModalAgregarGuia()
            'mpeAgregarGuia.Show()
            dlgInfoGuia.Show()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cancelar la opcion. " & ex.Message)
        End Try
    End Sub

    Protected Sub lnkAgregarFactura_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAgregarFactura.Click
        Try
            LimpiarFormularioCrearFactura()            
            MensajeCantidadDisponible()
            dlgInfoFactura.Show()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar la opcion para agregar factura. " & ex.Message)
        End Try
    End Sub

    Private Sub LimpiarFormularioCrearFactura()
        dlgInfoFactura.HeaderHtml = "<b>Agregar Factura</b>"
        txtFactura.Enabled = True
        EncabezadoAgregarFactura.clear()
        txtFiltroCiudad.Enabled = False
        lblIngreseCiudad.Visible = False
        txtFactura.Text = String.Empty
        txtCantidad.Text = String.Empty
        ddlPais.ClearSelection()
        InicializaDropDownList(ddlCiudadCompra)
        MensajeCantidadDisponible()
        txtFiltroCiudad.Text = String.Empty
        btnCrear.Visible = True
        btnEditarFactura.Visible = False
        hfIdFacturaEdicionActual.Value = 0
    End Sub

    Private Sub LimpiarFormularioEditarFactura()
        dlgInfoFactura.HeaderHtml = "<b>Editar Factura</b>"
        'txtFactura.Enabled = False
        txtFiltroCiudad.Text = String.Empty
        Dim facturaActual As New Recibos.InfoFactura(CLng(hfIdFacturaEdicionActual.Value))
        btnCrear.Visible = False
        btnEditarFactura.Visible = True
        ObtenerPais()
        Dim ciudadActual As New Localizacion.Ciudad(facturaActual.IdCiudadCompra)
        Dim paisActual As New Localizacion.Pais(CInt(ciudadActual.IdPais))
        ddlPais.SelectedValue = paisActual.IdPais.ToString
        txtFactura.Text = facturaActual.Factura
        txtCantidad.Text = facturaActual.Cantidad
        MensajeCantidadDisponibleEdicionFactura(facturaActual.IdFactura)
        lblIngreseCiudad.Visible = False
        ObtenerCiudadCompra()
        ddlCiudadCompra.SelectedValue = facturaActual.IdCiudadCompra
        EncabezadoAgregarFactura.clear()

        Dim detalleOrdenObj As New Recibos.DetalleOrdenCompra(CLng(hfIdDetalleOrdenCompra.Value))

        If Not detalleOrdenObj.PosibleEditarTodaFactura(hfIdFacturaEdicionActual.Value) Then

        End If

    End Sub

    Private Sub MensajeCantidadDisponible()
        lblInfoCantMaxPermitida.Text = "Cantidad maxima permitida " & CantidadPermitida().ToString
    End Sub

    Private Sub MensajeCantidadDisponibleEdicionFactura(ByVal idFactura As Long)
        Dim mensaje As String
        Dim cantidadMinima As Integer = Recibos.FacturaGuia.CantidadPorFactura(idFactura)
        mensaje = "Cantidad maxima permitida " & CantidadPermitida().ToString
        If cantidadMinima > 0 Then
            mensaje += " y la cantidad minima permitida es " & cantidadMinima
        End If
        lblInfoCantMaxPermitida.Text = mensaje
    End Sub

    Private Function CantidadPermitidaPorFactura(ByVal idFactura As Long) As Integer
        Dim factura As New Recibos.InfoFactura(idFactura)
        Dim cantidadesEnFactura As Integer = Recibos.FacturaGuia.CantidadPorFactura(idFactura)
        Return (factura.Cantidad - cantidadesEnFactura)
    End Function

    Protected Sub CantidadPermitidaPorFacturaMensaje(ByVal idFactura As Long)
        lblCantidadPermitidaPorFactura.Text = "Cantidad maxima permitida " & CantidadPermitidaPorFactura(idFactura).ToString
        lblCantidadPermitidaGuiaExistente.Text = "Cantidad maxima permitida " & CantidadPermitidaPorFactura(idFactura).ToString
        cvCantidadPermitidaGuiaExistente.ValueToCompare = CantidadPermitidaPorFactura(idFactura).ToString
    End Sub

    Private Sub cpFiltro_Execute(ByVal sender As Object, ByVal e As EO.Web.CallbackEventArgs) Handles cpFiltroFactura.Execute
        If e.Parameter.ToLower = "filtrarciudad" Then
            FiltrarCiudad()
        End If
    End Sub

    Protected Sub FiltrarCiudad()
        Dim dt As DataTable = HttpContext.Current.Session("dtCiudad")
        If txtFiltroCiudad.Text.Length > 3 Then
            dt.DefaultView.RowFilter = "nombre like '%" + txtFiltroCiudad.Text + "%'  "
            dt.DefaultView.Sort = "nombre asc"
            Session("dtCiudad") = dt
            ObtenerCiudadCompra(1)
        Else
            InicializaDropDownList(ddlCiudadCompra)
        End If
        '        ScriptManager.RegisterStartupScript(Me.Page, upAgregarFactura.GetType(), "enfocaFiltroCiudad", "enfocar(""#txtFiltroCiudad"");", True)
    End Sub

End Class