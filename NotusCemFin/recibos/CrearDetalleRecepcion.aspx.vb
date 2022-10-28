Imports ILSBusinessLayer
Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer.Comunes
Imports LMDataAccessLayer
Imports System.IO
Imports DevExpress.Web


Partial Public Class CrearDetalleRecepcion
    Inherits System.Web.UI.Page
    Protected consignatarioObj As Recibos.Consignatario
    Protected ordenCompraObj As Recibos.OrdenCompra
    Protected ordenRecepcionObj As Recibos.OrdenRecepcion

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

    Public ReadOnly Property MostrarOcultar() As Boolean
        Get
            If Me.ordenRecepcionObj Is Nothing Then _
                Me.ordenRecepcionObj = New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
            If Me.ordenRecepcionObj.IdEstado = OrdenRecepcion.EstadoOrden.Abierta Or Me.ordenRecepcionObj.IdEstado = OrdenRecepcion.EstadoOrden.Parcial Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            EncabezadoPagina.clear()
            If Not IsPostBack Then
                Session("dtImagenes") = Nothing
                hfOrdenRecepcion.Value = CInt(Request.QueryString("orep"))
                hfFacturaGuia.Value = CInt(Request.QueryString("facGui"))
                EncabezadoPagina.setTitle("Administrador de Recibos")
                EncabezadoPagina.showReturnLink("~/recibos/BuscarOrdenRecepcion.aspx")
                CrearTablaSoporte()
                If CInt(hfOrdenRecepcion.Value) > 0 Then
                    CargarDatosFacturaGuia()
                    Me.ordenRecepcionObj = New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
                    Session("ordenRecepcionObj") = Me.ordenRecepcionObj
                    Me.ordenCompraObj = New Recibos.OrdenCompra(ordenRecepcionObj.IdOrdenCompra)
                    Me.consignatarioObj = New Recibos.Consignatario(ordenRecepcionObj.IdConsignatario)

                    CargarDatosOrdenRecepcion(ordenRecepcionObj)
                    CargarPalletsActuales()
                    CargarInfoProductoAdicional()
                    CargarProducto()
                    ucImagen.Enabled = MostrarOcultar
                    btnCrear.Visible = MostrarOcultar
                    pnlCrearPallet.Visible = MostrarOcultar
                    MostrarBotonCerrarOrden()
                End If
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Protected Sub CargarInfoProductoAdicional()
        Try
            Dim filtroDetalleProducto As New Estructuras.FiltroDetalleOrdenCompra
            Dim dtProductoAdicional As New DataTable
            filtroDetalleProducto.IdOrden = CInt(ordenCompraObj.IdOrden)
            filtroDetalleProducto.IdTipoDetalle = TipoDetalleOrdenCompra.TipoDetalle.Secundario
            dtProductoAdicional = Recibos.DetalleOrdenCompra.ObtenerListado(filtroDetalleProducto)
            If dtProductoAdicional.Rows.Count > 0 Then
                With ddlProductoAdicional
                    .DataSource = dtProductoAdicional
                    .DataTextField = "producto"
                    .DataValueField = "idProducto"
                    .DataBind()
                    If dtProductoAdicional.Rows.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el producto adicional", 0))
                End With
                lblCantidadProductoAdicional.Text = CantidadProductoAdicionalEsperado(ordenCompraObj.IdOrden)
                'tblPalletsProductoAdicional.Visible = True
                'pnlProductoAdicional.Visible = True
                trCantidadProductoAdicional.Visible = True
                hfBoolProductoAdicional.Value = 1
            Else
                tblPalletsProductoAdicional.Visible = False
                pnlProductoAdicional.Visible = False
                trCantidadProductoAdicional.Visible = False
                hfBoolProductoAdicional.Value = 0
            End If


            CargarCajasTemporalesProAdicional()
            CargarPalletsProAdicional()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar los datos del producto adicional. " & ex.Message)
        End Try
    End Sub

    Private Function CantidadProductoAdicionalEsperado(ByVal idOrden As Long) As Integer
        Try
            Dim cantidadEsperada As Integer
            Dim dtResultado As New DataTable
            Dim filtroOrden As New Estructuras.FiltroDetalleOrdenCompra
            Dim detalleOrdenCompraObj As New Recibos.DetalleOrdenCompra()
            filtroOrden.IdOrden = idOrden
            dtResultado = detalleOrdenCompraObj.ObtenerListado(filtroOrden)
            Integer.TryParse(dtResultado.Compute("SUM(cantidad)", "idTipoDetalle = 2").ToString, cantidadEsperada)
            Return cantidadEsperada
        Catch ex As Exception
            EncabezadoPagina.showError("Error al obtener las cantidades de producto adicional. " & ex.Message)
        End Try
    End Function

    Private Function CantidadProductoAdicionalCargado() As Integer
        Try
            Dim filtroPallet As New Estructuras.FiltroPalletRecepcion
            Dim dtPalletRegistrados As New DataTable
            Dim dtDetallePallet As New DataTable
            Dim cantidadTemporal As Integer
            Dim cantidadRecibida As Integer
            Dim palletObj As PalletRecepcion
            Dim idPallet As Long
            filtroPallet.IdOrdenRecepcion = CInt(hfOrdenRecepcion.Value)
            filtroPallet.IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Secundario
            dtPalletRegistrados = PalletRecepcion.ObtenerListado(filtroPallet)
            For Each fila As DataRow In dtPalletRegistrados.Rows
                Long.TryParse(fila("idPallet"), idPallet)
                palletObj = New PalletRecepcion(idPallet)
                dtDetallePallet = palletObj.ObtenerDetallePorPallet(idPallet, TipoDetalleOrdenCompra.TipoDetalle.Secundario)
                Integer.TryParse(dtDetallePallet.Compute("SUM(cantidad)", "").ToString, cantidadTemporal)
                cantidadRecibida += cantidadTemporal
            Next
            Return cantidadRecibida
        Catch ex As Exception
            EncabezadoPagina.showError("Error al obtener las cantidades registradas de producto adicional. " & ex.Message)
        End Try
    End Function

    Private Sub CargarProducto()
        Dim dt As New DataTable
        Dim infoRecepcion As New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
        Dim filtro As Estructuras.FiltroProducto
        Dim infoFacturaGuia As New Recibos.FacturaGuia(infoRecepcion.IdFacturaGuia)
        Dim Factura As New Recibos.InfoFactura(CLng(infoFacturaGuia.IdFactura))
        Dim detalleOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(Factura.IdDetalleOrdenCompra))
        Dim infoOrdenCompra As New Recibos.OrdenCompra(CLng(infoRecepcion.IdOrdenCompra))
        filtro.IdTipoProducto = CShort(infoOrdenCompra.IdTipoProducto)
        filtro.IdFabricante = detalleOrdenCompra.IdFabricante
        filtro.IdProducto = CInt(Factura.IdProducto)
        dt = Productos.Producto.ObtenerListado(filtro)
        ObtenerProducto(dt)
    End Sub

    Private Sub CargarDatosFacturaGuia()
        Try
            Dim infoRecepcion As New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
            Dim infoFacturaGuia As New Recibos.FacturaGuia(infoRecepcion.IdFacturaGuia)
            Dim Factura As New Recibos.InfoFactura(CLng(infoFacturaGuia.IdFactura))
            Dim Guia As New Recibos.InfoGuia(CLng(infoFacturaGuia.IdGuia))
            Dim detalleOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(Factura.IdDetalleOrdenCompra))
            Dim infoOrdenCompra As New Recibos.OrdenCompra(CLng(infoRecepcion.IdOrdenCompra))
            Dim dtNovedades As New DataTable
            Dim filtroNovedad As New Estructuras.FiltroNovedadILS
            filtroNovedad.IdTipoNovedad = 1
            dtNovedades = Novedad.Novedad.ObtenerListado(filtroNovedad)
            gvNovedades.DataSource = dtNovedades
            gvNovedades.DataBind()

            With infoRecepcion
                lblNumeroRecepcion.Text = .IdOrdenRecepcion
                lblFechaRecepcion.Text = .FechaRecepcion.ToString
                lblTipoRecepcion.Text = .TipoRecepcion
                lblRemision.Text = .Remision
            End With
            With infoOrdenCompra
                lblNumeroOrdenCompra.Text = .NumeroOrden.ToString
                lblTipoProducto.Text = .TipoProducto.Descripcion
            End With

            With infoFacturaGuia
                Session("idFacturaGuia") = .IdFacturaGuia
            End With

            With Factura
                lblFactura.Text = .Factura
                lblCantidad.Text = .Cantidad
                hfCantidadPermitida.Value = .Cantidad
            End With

            With Guia
                lblGuia.Text = .Guia
            End With

            lblConsignado.Text = infoRecepcion.Consignatario.Nombre
            lblDestinatario.Text = infoRecepcion.ClienteExterno.Nombre
            Dim estadoRecepcion As New Estado(infoRecepcion.IdEstado)
            lblEstadoOrden.Text = estadoRecepcion.Descripcion
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar los datos de factura, guia. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarPalletsActuales()
        Dim dtResultado As New DataTable
        Try
            dtResultado = PalletRecepcion.ObtenerInfoDetalle(CLng(hfOrdenRecepcion.Value), _
                                                                            TipoDetalleOrdenCompra.TipoDetalle.Principal)
            ObtenerCantidadPallets(dtResultado)
            ObtenerPesoPallets(dtResultado)
            gvDetallePallet.DataSource = dtResultado
            If dtResultado.Rows.Count > 0 Then
                ddlProducto.Enabled = False
                cmbColor.Enabled = False
                cmbColor.Text = dtResultado.Rows(0).Item("nombreColor")
            Else
                ddlProducto.Enabled = True
                cmbColor.Enabled = True
            End If
            If dtResultado.Rows.Count > 0 Then gvDetallePallet.Columns(0).FooterText = "<div class='thGris'>" & dtResultado.Rows.Count.ToString & " Pallet(s) Registrado(s)</div>"
            gvDetallePallet.DataBind()
            MetodosComunes.mergeGridViewFooter(gvDetallePallet)

            hfCantidadPalletRegistrada.Value = CantidadRegistradaPallet(dtResultado).ToString()
            MostrarBotonCerrarOrden()
            '---------------------------------------------------------------------------------------------------
            Dim dtDetallePallets As New DataTable
            Dim dtNovedadesPallets As New DataTable
            Dim _detallePallets As String
            Dim _NovedadesPallets As String
            'Carga el detalle del pallet
            dtDetallePallets = PalletRecepcion.ObtenerDetallePalletGeneral(CLng(hfOrdenRecepcion.Value))
            Dim i As Integer
            If dtDetallePallets IsNot Nothing Then
                _detallePallets = ""
                For i = 0 To dtDetallePallets.Rows.Count - 1
                    If _detallePallets.Trim.Length = 0 Then
                        _detallePallets = dtDetallePallets.Rows(i).Item("pallets").ToString.Trim
                    Else
                        _detallePallets = _detallePallets & ", " & dtDetallePallets.Rows(i).Item("pallets").ToString.Trim
                    End If
                Next
            End If
            'Cargar Novedades Pallets
            dtNovedadesPallets = PalletRecepcion.ObtenerNovedadesPallet(CLng(hfOrdenRecepcion.Value))
            i = 0
            _NovedadesPallets = ""
            If dtNovedadesPallets IsNot Nothing Then
                For i = 0 To dtNovedadesPallets.Rows.Count - 1
                    If _NovedadesPallets.Trim.Length = 0 Then
                        _NovedadesPallets = dtNovedadesPallets.Rows(i).Item("novedad").ToString.Trim
                    Else
                        _NovedadesPallets = _NovedadesPallets & ", " & dtNovedadesPallets.Rows(i).Item("novedad").ToString.Trim
                    End If
                Next
            End If
            If _NovedadesPallets.Trim.Length = 0 Then
                _NovedadesPallets = "Sin Novedad"
            End If
            Session("detallePallets") = _detallePallets
            Session("NovedadesPallets") = _NovedadesPallets
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los pallets actuales a esta orden " & ex.Message)
        End Try
    End Sub

    Private Function CantidadRegistradaPallet(ByVal dtPallet As DataTable) As Integer
        Try
            Dim totalCantidad As Integer
            For Each Fila As DataRow In dtPallet.Rows
                totalCantidad = totalCantidad + CInt(Fila("cantidadRecibida"))
            Next
            Return totalCantidad
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Sub CargarDatosOrdenRecepcion(ByVal ordenRecepcion As OrdenRecepcion)
        'With ordenRecepcion
        '    lblTipoProducto.Text = .TipoProducto
        '    lblTipoRecepcion.Text = .TipoRecepcion
        '    lblOrdenCompra.Text = .OrdenCompra.NumeroOrden
        '    lblRemision.Text = .Remision.ToString
        '    lblFechaRecepcion.Text = .FechaRecepcion.ToString
        'End With

    End Sub

    Protected Sub ObtenerProducto(ByVal dt As DataTable)
        Try
            If dt.Rows.Count < 4 Then
                txtFiltroProducto.Visible = False
            End If
            With ddlProducto
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idProducto"
                .DataBind()
                If .Items.Count > 1 Then : .Items.Insert(0, New ListItem("Escoja el Producto", 0))
                ElseIf .Items.Count = 0 Then : inicializaDropDownList(ddlProducto)
                ElseIf .Items.Count = 1 Then : ObtenerColor(dt.Rows(0).Item("idProducto").ToString.Trim)
                End If
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar el tipo de producto " & ex.Message)
        End Try
    End Sub

    Protected Sub CargarDtDetallePallet()
        Dim dt As New DataTable
        Dim dr As DataRow
        dt = EstructuraDtDetallePallet()
        dr = dt.NewRow
        dr("producto") = ddlProducto.SelectedItem.ToString
        dr("idProducto") = ddlProducto.SelectedValue
        dr("cantidad") = txtCantidad.Text
        dt.Rows.InsertAt(dr, 0)
        ObtenerCantidadPallets(dt)
        ObtenerPesoPallets(dt)
        gvDetallePallet.DataSource = dt
        gvDetallePallet.DataBind()
        dt.AcceptChanges()
        Session("dtDatosDetallePalletRecepcion") = dt
        LimpiarDatosDetallePallet()

    End Sub

    Private Function ObtenerCantidadPallets(ByVal dt As DataTable) As Integer
        Try
            Dim retorno As Integer
            Integer.TryParse(dt.Compute("SUM(cantidadRecibida)", "").ToString(), retorno)
            lblCantidadRecibida.Text = retorno
            Return retorno
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Function ObtenerPesoPallets(ByVal dt As DataTable) As Decimal
        Try
            Dim retorno As Decimal
            Decimal.TryParse(dt.Compute("SUM(peso)", "").ToString(), retorno)
            lblPesoPallet.Text = retorno & " (Kgs)"
            Return retorno
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Protected Function EstructuraDtDetallePallet() As DataTable
        Dim dtDatos As DataTable
        If Session("dtDatosDetallePalletRecepcion") Is Nothing Then
            dtDatos = New DataTable
            Dim dc As New DataColumn("idDetallePallet", GetType(Integer))
            dc.AutoIncrement = True
            dc.AutoIncrementSeed = 1
            dtDatos.Columns.Add(dc)
            dtDatos.Columns.Add("producto")
            dtDatos.Columns.Add("idProducto")
            dtDatos.Columns.Add("cantidad")
            dtDatos.Columns.Add("cantidadRecibida")
            dtDatos.Columns.Add("tipoUnidad")
            dtDatos.Columns.Add("idTipoUnidad")
            dtDatos.Columns.Add("idOrdenBodegaje2")
            dtDatos.Columns.Add("idOrdenBodegaje")
        Else
            dtDatos = CType(Session("dtDatosDetallePalletRecepcion"), DataTable)
        End If
        Return dtDatos
    End Function

    Private Sub LimpiarDatosDetallePallet()
        ddlProducto.SelectedIndex = 0
        txtCantidad.Text = String.Empty
    End Sub

    Protected Sub gvDetallePallet_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetallePallet.RowCommand
        If e.CommandName = "Editar" Then
            CargarDatosEdicionDetalle(e.CommandArgument)
        ElseIf e.CommandName = "Eliminar" Then
            EliminarPallet(CLng(e.CommandArgument))
        ElseIf e.CommandName = "imprimirViajera" Then
            imprimirViajera(CInt(e.CommandArgument), True)
        ElseIf e.CommandName = "verNovedades" Then
            MostrarNovedadesPallet(CInt(e.CommandArgument))
        End If
    End Sub

    Private Sub EliminarPallet(ByVal idPallet As Long)
        Try
            If Recibos.PalletRecepcion.Eiliminar(idPallet) Then
                CargarPalletsActuales()
                CargarPalletsProAdicional()
                EncabezadoPagina.showSuccess("Pallet eliminado correctamente.")
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al eliminar el palllet. " & ex.Message)
        End Try
    End Sub

    Private Sub imprimirViajera(ByVal idDetallePallet As Integer, Optional ByVal reImpresion As Boolean = False)
        Try
            'Dim rpt As New ReporteCrystal("resumenPalletRecepcion", Server.MapPath("../Reports"))
            Dim rpt As New ReporteCrystal("HojaViajera", Server.MapPath("../Reports"))
            rpt.agregarParametroDiscreto("@idPallet", idDetallePallet)
            rpt.agregarParametroDiscreto("reimpresion", reImpresion)
            Dim ruta As String = rpt.exportar(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
            ruta = ruta.Substring(ruta.LastIndexOf("\") + 1)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "newWindow", "window.open('../Reports/rptTemp/" + ruta + "','Impresion_Viajera', 'status=1, toolbar=0, location=0,menubar=1,directories=0,resizable=1,scrollbars=1'); ", True)

        Catch ex As Exception
            EncabezadoPagina.showError("Error al generar el documento. " & ex.Message)
        End Try
    End Sub

    Private Sub MostrarNovedadesPallet(ByVal idPallet As Integer)
        Try
            Dim filtro As New Estructuras.FiltroPalletNovedad
            filtro.IdPallet = idPallet
            bltNovedades.DataSource = Recibos.PalletNovedad.ObtenerListado(filtro)
            bltNovedades.DataBind()
            mpeMostrarNovedades.Show()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar las novedades. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarDatosEdicionDetalle(ByVal idDetallePallet As Integer)
        Dim dt As New DataTable
        Dim dr As DataRow
        dt = CType(Session("dtDatosDetallePalletRecepcion"), DataTable)
        dr = dt.Select("idDetallePallet = " & idDetallePallet.ToString)(0)
        ddlProducto.SelectedValue = dr("idProducto")
        txtCantidad.Text = dr("cantidad")
    End Sub

    Private Sub RegistrarNovedad(ByVal idPallet As Long)
        Try
            Dim idNovedad As Integer
            Dim hfIdNovedad As HiddenField
            For i As Integer = 0 To gvNovedades.Rows.Count - 1
                Dim row As GridViewRow = gvNovedades.Rows(i)
                Dim isChecked As Boolean = DirectCast(row.FindControl("chkNovedad"), CheckBox).Checked

                If isChecked Then
                    hfIdNovedad = CType(row.FindControl("hfIdNovedad"), HiddenField)
                    idNovedad = CInt(hfIdNovedad.Value)
                    Dim infoPalletNovedad As New Recibos.PalletNovedad()
                    With infoPalletNovedad
                        .IdNovedad = idNovedad
                        .IdPallet = idPallet
                        .Crear()
                    End With
                End If
            Next
        Catch ex As Exception
            EncabezadoPagina.showError("Error al registrar las novedades")
        End Try
    End Sub

    Private Sub LimpiarNovedad()
        Try
            Dim chkNovedad As CheckBox
            For i As Integer = 0 To gvNovedades.Rows.Count - 1
                Dim row As GridViewRow = gvNovedades.Rows(i)
                chkNovedad = CType(row.FindControl("chkNovedad"), CheckBox)

                If chkNovedad.Checked Then
                    chkNovedad.Checked = False
                End If
            Next
        Catch ex As Exception
            EncabezadoPagina.showError("Error al limpiar las novedades")
        End Try
    End Sub

    Private Function CantidadEnFactura() As Integer
        Try
            Dim dt As New DataTable
            Dim infoRecepcion As New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
            Dim infoFacturaGuia As New Recibos.FacturaGuia(infoRecepcion.IdFacturaGuia)
            Dim factura As New Recibos.InfoFactura(infoFacturaGuia.IdFactura)
            Return infoFacturaGuia.Cantidad
        Catch ex As Exception
            EncabezadoPagina.showError("Error al obtener la cantidad en factura. " & ex.Message)
        End Try
    End Function

    Private Function CantidadDetalleRecepcion() As Integer
        Try
            Dim dt As New DataTable
            dt = PalletRecepcion.ObtenerInfoDetalle(CLng(hfOrdenRecepcion.Value))
            Dim totalCantidad As Integer = 0
            Dim i As Integer = 0
            If dt.Rows.Count > 0 Then
                For i = 0 To dt.Rows.Count - 1
                    totalCantidad += CInt(dt.Rows(i)("cantidad"))
                Next
            End If
            Return totalCantidad
        Catch ex As Exception
            EncabezadoPagina.showError("Error al obtener la cantidad en detalle de pallet. " & ex.Message)
        End Try
    End Function

    Private Sub limpiarFormulario()
        Try
            ddlProducto.SelectedIndex = 0
            txtCantidad.Text = String.Empty
            txtPeso.Text = String.Empty
            txtObservacion.Text = String.Empty
            LimpiarNovedad()
        Catch ex As Exception
            EncabezadoPagina.showError("Imposible limpiar el formulario de registro de pallet. " & ex.Message)
        End Try
    End Sub

    Protected Sub FiltrarProducto(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroProducto.TextChanged
        Try
            Dim dt As New DataTable
            Dim filtro As Estructuras.FiltroProducto
            Dim infoRecepcion As New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
            Dim infoFacturaGuia As New Recibos.FacturaGuia(infoRecepcion.IdFacturaGuia)
            Dim Factura As New Recibos.InfoFactura(CLng(infoFacturaGuia.IdFactura))
            Dim Guia As New Recibos.InfoGuia(CLng(infoFacturaGuia.IdGuia))
            Dim detalleOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(Factura.IdDetalleOrdenCompra))
            Dim infoOrdenCompra As New Recibos.OrdenCompra(CLng(detalleOrdenCompra.IdOrden))
            filtro.IdTipoProducto = CShort(infoOrdenCompra.IdTipoProducto)
            filtro.IdFabricante = detalleOrdenCompra.IdFabricante
            filtro.IdProducto = CInt(detalleOrdenCompra.IdProducto)
            If Session("dtProductos") Is Nothing Then
                dt = Productos.Producto.ObtenerListado(filtro)
            Else
                dt = CType(HttpContext.Current.Session("dtProductos"), DataTable)
            End If
            If txtFiltroProducto.Text.Length > 3 Then
                dt.DefaultView.RowFilter = "nombre like '%" + txtFiltroProducto.Text + "%'  "
                dt.DefaultView.Sort = "nombre asc"
                Session("dtProductos") = dt
                ObtenerProducto(dt)
                'Else : dt.DefaultView.RowFilter = " idProducto=idProducto"
            Else
                inicializaDropDownList(ddlProducto)
            End If

        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar los producto. " & ex.Message)
        End Try
    End Sub

    Private Sub inicializaDropDownList(ByRef control As DropDownList)
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem("Seleccione...", 0))
    End Sub

    Private Sub ObtenerColor(_idProducto)
        Try
            Dim dtColor As DataTable
            Dim objColor As New Productos.Producto
            With objColor
                .IdProducto = _idProducto
                dtColor = .ObtenerColoresProducto()
            End With
            If dtColor.Rows.Count > 0 Then
                MetodosComunes.CargarComboDX(cmbColor, dtColor, "id", "color")
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al obtener colores: " & ex.ToString)
        End Try
    End Sub

    Protected Sub btnAceptar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAceptar.Click
        Dim palletRecepcion As New PalletRecepcion
        Try
            With palletRecepcion
                .IdOrdenRecepcion = CLng(hfOrdenRecepcion.Value)
                .Peso = CDbl(txtPeso.Text)
                .IdCreador = CLng(Session("usxp001"))
                .Observacion = txtObservacion.Text
                palletRecepcion.AdicionarDetalle(CInt(ddlProducto.SelectedValue), CInt(txtCantidad.Text), "", "")
                If .Crear(CantidadDetalleRecepcion()) Then
                    'lblCantidad.Text = CantidadDetalleRecepcion().ToString
                    RegistrarNovedad(.IdPallet)
                    CargarPalletsActuales()
                    EncabezadoPagina.showSuccess("Pallet de recepción creado con exito.")
                    mpeConfirmarRecepcion.Hide()
                    imprimirViajera(.IdPallet)
                End If
            End With
            limpiarFormulario()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de crear el pallet indicado. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        mpeConfirmarRecepcion.Hide()
    End Sub

    Protected Sub BtnCerrarRecepcion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnCerrarRecepcion.Click
        Try
            Dim respuestaEnvioNotificacion As New ResultadoProceso
            Dim dtImagen As DataTable = Session("dtImagenes")
            If dtImagen Is Nothing Then
                EncabezadoPagina.showWarning("Debe cargar minimo 5 imagenes de la recepción para continuar con el cierre de la misma.")
                Exit Sub
            End If
            If dtImagen.Rows.Count < 5 Then
                EncabezadoPagina.showWarning("Debe cargar minimo 5 imagenes de la recepción para continuar con el cierre de la misma.")
                lblCantidadArchivos.Text = dtImagen.Rows.Count
                Exit Sub
            End If
            If Session("usxp009") = 55 Then
                If gvDetallePallet.Rows.Count > 0 Then
                    Dim infoRecepcion As New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
                    Dim infoFacturaGuia As New Recibos.FacturaGuia(CLng(infoRecepcion.IdFacturaGuia))
                    'Dim factura As New Recibos.InfoFactura(CLng(infoFacturaGuia.IdFactura))
                    Dim guia As New Recibos.InfoGuia(CLng(infoFacturaGuia.IdGuia))
                    Dim dtFacturasNoCerradas As New DataTable
                    infoRecepcion.IdEstado = Recibos.OrdenRecepcion.EstadoOrden.Finalizada
                    infoRecepcion.dtImagenes = Session("dtImagenes")
                    infoRecepcion.Actualizar()
                    pnlCrearPallet.Visible = False
                    dtFacturasNoCerradas = Recibos.OrdenCompra.ObtenerFacturasNoCerradas(CInt(infoRecepcion.IdOrdenCompra))
                    lblEstadoOrden.Text = infoRecepcion.Estado.Descripcion
                    EncabezadoPagina.showSuccess("Orden de recepción cerrada correctamente.")
                    BtnCerrarRecepcion.Visible = False
                    BtnCerrarRecepcion.Text = "Orden Cerrada"
                    CargarPalletsActuales()
                    CargarPalletsProAdicional()
                    Dim _dtDatos As New DataTable
                    _dtDatos.Columns.Add("guia", GetType(String))
                    _dtDatos.Columns.Add("factura", GetType(String))
                    _dtDatos.Columns.Add("producto", GetType(String))
                    _dtDatos.Columns.Add("cantidadAprox", GetType(String))
                    _dtDatos.Columns.Add("piezas", GetType(String))
                    _dtDatos.Columns.Add("pesoGuia", GetType(String))
                    _dtDatos.Columns.Add("pesoRecibido", GetType(String))
                    _dtDatos.Columns.Add("diferencia", GetType(String))
                    _dtDatos.Columns.Add("bodega", GetType(String))
                    _dtDatos.Columns.Add("estado", GetType(String))
                    _dtDatos.Columns.Add("ordenRecepcion", GetType(String))
                    _dtDatos.Columns.Add("color", GetType(String))
                    Dim drRow As DataRow
                    drRow = _dtDatos.NewRow()
                    drRow("guia") = infoRecepcion.Guia
                    drRow("factura") = infoRecepcion.Factura
                    drRow("producto") = ddlProducto.SelectedItem
                    drRow("cantidadAprox") = lblCantidadRecibida.Text
                    drRow("piezas") = Session("detallePallets")
                    drRow("pesoGuia") = guia.PesoBruto
                    Dim _pesoRecibido As String = PalletRecepcion.ObtenerPesoPalletGeneral(infoRecepcion.IdOrdenRecepcion)
                    drRow("pesoRecibido") = _pesoRecibido
                    drRow("diferencia") = CInt(guia.PesoBruto) - CInt(_pesoRecibido.Split(",").GetValue(0))
                    drRow("bodega") = "Sin Definir"
                    drRow("estado") = Session("NovedadesPallets")
                    drRow("ordenRecepcion") = infoRecepcion.IdOrdenRecepcion
                    drRow("color") = cmbColor.Text.Trim
                    _dtDatos.Rows.Add(drRow)
                    ucImagen.Enabled = False
                    lbImagen.Enabled = False
                    lblCantidadArchivos.Text = Session("dtImagenes").rows.count
                    If FolderTempImage IsNot Nothing Then
                        If Directory.Exists(Server.MapPath("~/recibos/images/ImagenRecepcion/") & FolderTempImage) Then
                            Directory.Delete(Server.MapPath("~/recibos/images/ImagenRecepcion/") & FolderTempImage, True)
                        End If
                    End If
                    respuestaEnvioNotificacion = EnviarNotificacion(_dtDatos, infoRecepcion.IdTipoProducto)
                    If (respuestaEnvioNotificacion.Valor = 9) Then
                        EncabezadoPagina.showWarning("No existe destinatario de correo configurado para enviar la notificación.")
                    End If
                Else
                    EncabezadoPagina.showWarning("No existe ningún pallet para esta orden.")
                End If
            Else
                EncabezadoPagina.showWarning("Solo un usuario con perfil de Supervisor de Recibos puede realizar el cierre de la recepción.")
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al intentar cerrar la recepción. " & ex.Message)
        End Try
    End Sub

    Public Sub CerrarOrden()
        Try
            BtnCerrarRecepcion.Enabled = False
            BtnCerrarRecepcion.Text = "Orden Cerrada"
        Catch ex As Exception
            EncabezadoPagina.showError("Error ala cerrar la orden. " & ex.Message)
        End Try
    End Sub

    Protected Sub imgBtnCerrarPopUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnCerrarPopUp.Click
        mpeMostrarNovedades.Hide()
    End Sub

    Protected Sub gvDetallePallet_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetallePallet.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim idPallet = CInt(fila("idPallet"))
            Dim dtResultado As New DataTable
            Dim filtro As New Estructuras.FiltroPalletNovedad
            Dim btnViajera As ImageButton = CType(e.Row.FindControl("imgBtnGenerar"), ImageButton)
            Dim btnVerNovedad As ImageButton = e.Row.FindControl("imgBtnVerNovedades")
            filtro.IdPallet = idPallet
            dtResultado = Recibos.PalletNovedad.ObtenerListado(filtro)
            If dtResultado.Rows.Count > 0 Then
                btnVerNovedad.Visible = True
            Else
                btnVerNovedad.Visible = False
            End If
            CType(e.Row.FindControl("imgBtnEliminarPallet"), ImageButton).Visible = MostrarOcultar

            Dim idPerfil As Integer = CInt(Session("usxp009"))
            If idPerfil > 0 Then
                btnViajera.Visible = OrdenRecepcion.PerfilesReimprimirViajera.Contains(idPerfil)
            End If

        End If
    End Sub

    Protected Sub lnkAgregarProductoAdicional_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAgregarProductoAdicional.Click
        Dim miCaja As New CajaEmpaque()
        Dim resultado As Short
        Dim cajaCargada As CajaEmpaque
        Try
            With miCaja
                .IdOrdenRecepcion = CLng(hfOrdenRecepcion.Value)
                Integer.TryParse(ddlProductoAdicional.SelectedValue, .IdProducto)
                Integer.TryParse(txtCantidadAdicional.Text.Trim(), .Cantidad)
                If Session("usxp001") IsNot Nothing Then Integer.TryParse(Session("usxp001"), .IdCreador)
                .IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Secundario

                resultado = .Registrar()
                If resultado = 0 Then
                    cajaCargada = New CajaEmpaque(CInt(.IdCaja))
                    EncabezadoPagina.showSuccess("El producto fue adicionado satisfactoriamente.")
                    LimpiarFormularioAdicionCaja()
                    CargarCajasTemporalesProAdicional()
                Else
                    If resultado = 2 Then
                        EncabezadoPagina.showWarning("No se puede registrar la información, porque no se han proporcionado todos los datos requeridos. Por favor verifique")
                    Else
                        EncabezadoPagina.showError("Ocurrió un error inesperado al registrar la información. Por favor intente nuevamente")
                    End If
                End If
            End With
        Catch ex As Exception
            EncabezadoPagina.showError("Error al agregar el producto adicional. " & ex.Message)
        End Try
        ddlProductoAdicional.Focus()
    End Sub

    Private Sub LimpiarFormularioAdicionCaja()
        ddlProductoAdicional.ClearSelection()
        txtCantidadAdicional.Text = ""
    End Sub

    Private Sub CargarCajasTemporalesProAdicional()
        Dim dtCaja As New DataTable
        Dim filtro As Estructuras.FiltroCajaEmpaque
        Try
            filtro.IdOrdenRecepcion = CLng(hfOrdenRecepcion.Value)
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
            EncabezadoPagina.showError("Error al tratar de cargar Cajas temporalmente para producto adicional. " & ex.Message)
        End Try
    End Sub

    Protected Sub lnkCrearPalletProAdicional_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkCrearPalletProAdicional.Click
        Try
            Dim miPallet As New PalletRecepcion
            Dim ordenRecepcionObj As New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
            With miPallet
                .IdOrdenRecepcion = CLng(hfOrdenRecepcion.Value)
                If Session("usxp001") IsNot Nothing Then Integer.TryParse(Session("usxp001"), .IdCreador)
                .Peso = CDec(txtPesoPalletAdicionl.Text)
                .Observacion = txtObservacion.Text.Trim
                Dim dtCajas As DataTable = CType(Session("dtCajasTemporalesProAdicional"), DataTable)
                .IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Secundario
                .IdFacturaGuia = ordenRecepcionObj.IdFacturaGuia
                If .CrearConCajasSinRegion(dtCajas) Then
                    imprimirViajera(.IdPallet)
                    EncabezadoPagina.showSuccess("La información del Pallet No. " & .IdPallet.ToString & " fue registrada satisfactoriamente. ")
                    LimpiarFormularioCrearPalletProductoAdicional()
                Else
                    EncabezadoPagina.showError("Ocurrió un error inesperado al crear el Pallet. Por vafor intente nuevamente")
                End If
            End With
            Session.Remove("dtCajasTemporalesProAdicional")
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de crear Pallet. " & ex.Message)
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
            filtro.IdOrdenRecepcion = CInt(hfOrdenRecepcion.Value)
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
            hfCantidadPalletAdicionalRegistrada.Value = CantidadProductoAdicionalCargado().ToString()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de cargar Pallets registrados. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnCrear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrear.Click
        Dim palletRecepcion As New PalletRecepcion
        Dim cantidadTotalConIngreso As Integer
        Dim infoRecepcion As New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
        cantidadTotalConIngreso = CantidadDetalleRecepcion()
        cantidadTotalConIngreso += CInt(txtCantidad.Text.Trim())
        ddlProducto.Enabled = False
        cmbColor.Enabled = False
        Try
            Dim _existePrincipal
            Dim objPrincipal As New Productos.MaterialColeccion
            With objPrincipal
                .IdProductoPadre = ddlProducto.SelectedValue
                .ValidarPrincipal()
                _existePrincipal = .ExistePrincipal
            End With
            If _existePrincipal = 1 Then
                Dim filtro As Estructuras.FiltroProducto
                filtro.IdProducto = CInt(ddlProducto.SelectedValue)
                Dim dt As New Productos.Producto(ddlProducto.SelectedValue)
                With palletRecepcion
                    .IdOrdenRecepcion = CLng(hfOrdenRecepcion.Value)
                    .Peso = CDec(txtPeso.Text.Trim())
                    .IdCreador = CLng(Session("usxp001"))
                    .Observacion = txtObservacion.Text.Trim()
                    .IdFacturaGuia = infoRecepcion.IdFacturaGuia
                    .Color = cmbColor.Text.Trim
                    Session("color") = cmbColor.Text.Trim
                    palletRecepcion.AdicionarDetalle(CInt(ddlProducto.SelectedValue), CInt(txtCantidad.Text), cmbColor.Text.Trim, dt.ProductoPrincipal)
                    If .Crear() Then
                        RegistrarNovedad(.IdPallet)
                        CargarPalletsActuales()
                        imprimirViajera(.IdPallet)
                        EncabezadoPagina.showSuccess("Pallet de recepción creado con exito.")
                    End If
                End With
                limpiarFormulario()
            Else
                EncabezadoPagina.showWarning("No existe material principal para el producto seleccionado, por esta razón no se puede realizar la recepción del producto.")
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de crear el pallet indicado. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvPalletProductoAdicional_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPalletProductoAdicional.RowCommand
        If e.CommandName = "Imprimir" Then
            imprimirViajera(CInt(e.CommandArgument), True)
        ElseIf e.CommandName = "Eliminar" Then
            EliminarPallet(CLng(e.CommandArgument))
        End If
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
                        EncabezadoPagina.showSuccess("El producto adicional fue removido satisfactoriamente. ")
                    Else
                        If resultado = 1 Then
                            EncabezadoPagina.showWarning("El producto adicional ya no existe, por favor recargue la página. ")
                        ElseIf resultado = 3 Then
                            EncabezadoPagina.showWarning("No se puede registrar la información, porque no se han proporcionado todos los datos requeridos. Por favor verifique")
                        Else
                            EncabezadoPagina.showError("Ocurrió un error inesperado al registrar la información. Por favor intente nuevamente")
                        End If
                    End If
                Else
                    EncabezadoPagina.showWarning("Imposible remover la Caja. Por favor intente nuevamente.")
                End If
            Catch ex As Exception
                EncabezadoPagina.showError("Error al tratar de remover caja. " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub MostrarBotonCerrarOrden()
        If gvDetallePallet.Rows.Count > 0 AndAlso MostrarOcultar Then
            BtnCerrarRecepcion.Visible = True
        Else
            BtnCerrarRecepcion.Visible = False
        End If
    End Sub

    Protected Sub gvPalletProductoAdicional_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPalletProductoAdicional.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim idPallet As Integer
            Integer.TryParse(e.Row.Cells(0).Text, idPallet)
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
                EncabezadoPagina.showError("Ocurrio un error al tratar de obtener el detalle de uno más pallets. " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub ddlProducto_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlProducto.SelectedIndexChanged
        Try
            ObtenerColor(ddlProducto.SelectedValue)
        Catch ex As Exception
            EncabezadoPagina.showError("Imposible los colores del material: " & ex.Message)
        End Try
    End Sub

    Private Sub ucArchivo_FilesUploadComplete(sender As Object, e As DevExpress.Web.FilesUploadCompleteEventArgs) Handles ucArchivo.FilesUploadComplete
        Try
            If ucArchivo.HasFile Then
                Dim dtSoporte As DataTable = Session("dtSoporte")
                Dim peso As Double
                Dim pesoMaximo As Double = "5242880"
                For i As Integer = 0 To ucArchivo.UploadedFiles.Length - 1
                    If dtSoporte.Rows.Count > 0 Then
                        peso = dtSoporte.Compute("sum(Peso)", "")
                    Else
                        peso = 0
                    End If
                    If peso <= pesoMaximo Then
                        If dtSoporte.Rows.Count > 0 Then
                            Dim dr As DataRow
                            dr = dtSoporte.NewRow
                            If Path.GetExtension(ucArchivo.UploadedFiles(i).FileName).ToLower.Trim = ".docx" Or _
                                Path.GetExtension(ucArchivo.UploadedFiles(i).FileName).ToLower.Trim = ".doc" Or _
                                Path.GetExtension(ucArchivo.UploadedFiles(i).FileName).ToLower.Trim = ".pdf" Then
                                dr("idTipoSoporte") = 1
                                dr("IdUsuarioRegistra") = Session("usxp001")
                                dr("RutaCompleta") = Server.MapPath("~/infoOperaciones/SoportesOperacion/Novedades/") & ucArchivo.UploadedFiles(i).FileName
                                dr("NombreOriginal") = ucArchivo.UploadedFiles(i).FileName
                                dr("DatosBinarios") = ucArchivo.UploadedFiles(i).FileBytes
                                dr("contenType") = ucArchivo.UploadedFiles(i).ContentType
                                dr("Peso") = 0
                                dtSoporte.Rows.Add(dr)
                                Session("dtSoporte") = dtSoporte
                            Else
                                dr("idTipoSoporte") = 2
                                dr("IdUsuarioRegistra") = Session("usxp001")
                                dr("RutaCompleta") = Server.MapPath("~/infoOperaciones/SoportesOperacion/Novedades/") & ucArchivo.UploadedFiles(i).FileName
                                dr("NombreOriginal") = ucArchivo.UploadedFiles(i).FileName
                                Dim imageResize As ImageResizer.ImageJob
                                Dim msImagenOrigen As New MemoryStream(ucArchivo.UploadedFiles(i).FileBytes)
                                Dim msImagenDestino As New MemoryStream()
                                Dim settings As New ImageResizer.ResizeSettings()
                                With settings
                                    .Width = 450
                                    .Height = 450
                                    .Mode = ImageResizer.FitMode.Max
                                End With
                                imageResize = New ImageResizer.ImageJob(msImagenOrigen, msImagenDestino, settings).Build()
                                Dim biteArray As Byte() = New Byte(msImagenDestino.Length) {}
                                msImagenDestino.Position = 0
                                msImagenDestino.Read(biteArray, 0, msImagenDestino.Length)
                                dr("DatosBinarios") = biteArray
                                dr("contenType") = ucArchivo.UploadedFiles(i).ContentType
                                dr("Peso") = ucArchivo.UploadedFiles(i).ContentLength
                                dtSoporte.Rows.Add(dr)
                                Session("dtSoporte") = dtSoporte
                            End If
                            CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = "Archivos Cargados: " & dtSoporte.Rows.Count
                        Else
                            Dim dr As DataRow
                            dr = dtSoporte.NewRow
                            If Path.GetExtension(ucArchivo.UploadedFiles(i).FileName).ToLower.Trim = ".docx" Or _
                                Path.GetExtension(ucArchivo.UploadedFiles(i).FileName).ToLower.Trim = ".doc" Or _
                                Path.GetExtension(ucArchivo.UploadedFiles(i).FileName).ToLower.Trim = ".pdf" Then
                                dr("idTipoSoporte") = 1
                                dr("IdUsuarioRegistra") = Session("usxp001")
                                dr("RutaCompleta") = Server.MapPath("~/infoOperaciones/SoportesOperacion/Novedades/") & ucArchivo.UploadedFiles(i).FileName
                                dr("NombreOriginal") = ucArchivo.UploadedFiles(i).FileName
                                dr("DatosBinarios") = ucArchivo.UploadedFiles(i).FileBytes
                                dr("contenType") = ucArchivo.UploadedFiles(i).ContentType
                                dr("Peso") = 0
                                dtSoporte.Rows.Add(dr)
                                Session("dtSoporte") = dtSoporte
                            Else
                                dr("idTipoSoporte") = 2
                                dr("IdUsuarioRegistra") = Session("usxp001")
                                dr("RutaCompleta") = Server.MapPath("~/infoOperaciones/SoportesOperacion/Novedades/") & ucArchivo.UploadedFiles(i).FileName
                                dr("NombreOriginal") = ucArchivo.UploadedFiles(i).FileName
                                Dim imageResize As ImageResizer.ImageJob
                                Dim msImagenOrigen As New MemoryStream(ucArchivo.UploadedFiles(i).FileBytes)
                                Dim msImagenDestino As New MemoryStream()
                                Dim settings As New ImageResizer.ResizeSettings()
                                With settings
                                    .Width = 450
                                    .Height = 450
                                    .Mode = ImageResizer.FitMode.Max
                                End With
                                imageResize = New ImageResizer.ImageJob(msImagenOrigen, msImagenDestino, settings).Build()
                                Dim biteArray As Byte() = New Byte(msImagenDestino.Length) {}
                                msImagenDestino.Position = 0
                                msImagenDestino.Read(biteArray, 0, msImagenDestino.Length)
                                dr("DatosBinarios") = biteArray
                                dr("contenType") = ucArchivo.UploadedFiles(i).ContentType
                                dr("Peso") = ucArchivo.UploadedFiles(i).ContentLength
                                dtSoporte.Rows.Add(dr)
                                peso = dtSoporte.Compute("sum(Peso)", "")
                                If peso > pesoMaximo Then
                                    CType(sender, ASPxUploadControl).JSProperties("cpPeso") = peso
                                    CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = "Archivos Cargados: 0"
                                    dtSoporte.Clear()
                                Else
                                    CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = "Archivos Cargados: " & dtSoporte.Rows.Count
                                    Session("dtSoporte") = dtSoporte
                                End If
                            End If
                        End If
                    Else
                        CType(sender, ASPxUploadControl).JSProperties("cpPeso") = peso
                        CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = "Archivos Cargados: 0"
                        dtSoporte.Clear()
                        Session("dtSoporte") = dtSoporte
                        Exit For
                    End If
                Next
                If dtSoporte.Rows.Count > 0 Then
                    peso = dtSoporte.Compute("sum(Peso)", "")
                    If peso > pesoMaximo Then
                        CType(sender, ASPxUploadControl).JSProperties("cpPeso") = peso
                        CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = "Archivos Cargados: 0"
                        CType(sender, ASPxUploadControl).JSProperties("cpCantidad") = "-1"
                        dtSoporte.Clear()
                    Else
                        CType(sender, ASPxUploadControl).JSProperties("cpPeso") = 0
                        CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = "Archivos Cargados: " & dtSoporte.Rows.Count
                        Session("dtSoporte") = dtSoporte
                        CType(sender, ASPxUploadControl).JSProperties("cpCantidad") = dtSoporte.Rows.Count
                    End If
                Else
                    CType(sender, ASPxUploadControl).JSProperties("cpCantidad") = "-1"
                End If
                CType(sender, ASPxUploadControl).JSProperties("cpResultado") = ""
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Imposible subir el archivo al Servidor: " & ex.Message)
            CType(sender, ASPxUploadControl).JSProperties("cpResultado") = "Imposible subir el archivo al Servidor: " & ex.ToString
        End Try
    End Sub

    Private Sub pcRegistro_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles pcRegistro.WindowCallback
        Try
            Dim arrAccion As String()
            arrAccion = e.Parameter.Split(":")
            Select Case arrAccion(1)
                Case "grabar"
                    Dim r As New ILSBusinessLayer.NovedadProduccion
                    Dim dt As DataTable = Session("dtSoporte")
                    With r
                        For i As Integer = 0 To dt.Rows.Count - 1
                            Dim sop As New ILSBusinessLayer.SoporteNovedadProduccion
                            With sop
                                .IdTipoSoporte = dt.Rows(i).Item("idTipoSoporte")
                                .IdUsuarioRegistra = dt.Rows(i).Item("IdUsuarioRegistra")
                                .RutaCompleta = dt.Rows(i).Item("RutaCompleta")
                                .NombreOriginal = dt.Rows(i).Item("NombreOriginal")
                                If Path.GetExtension(dt.Rows(i).Item("NombreOriginal").ToLower.Trim) = ".docx" Or _
                                Path.GetExtension(dt.Rows(i).Item("NombreOriginal").ToLower.Trim) = ".doc" Or _
                                Path.GetExtension(dt.Rows(i).Item("NombreOriginal").ToLower.Trim) = ".pdf" Then
                                    Dim nombre As String = dt.Rows(i).Item("NombreOriginal").ToString.Split(".").GetValue(0).ToString.ToLower.Trim & "_" & _
                                        System.Guid.NewGuid().ToString & Path.GetExtension(dt.Rows(i).Item("NombreOriginal"))
                                    Dim rutaCompleta As String = Server.MapPath("~/infoOperaciones/SoportesOperacion/Novedades/") & nombre
                                    Using objFS As New FileStream(rutaCompleta, FileMode.Create, FileAccess.ReadWrite)
                                        objFS.Write(dt.Rows(i).Item("DatosBinarios"), 0, dt.Rows(i).Item("DatosBinarios").Length)
                                        objFS.Flush()
                                        objFS.Close()
                                    End Using
                                    .RutaCompleta = rutaCompleta
                                    .NombreOriginal = nombre
                                    .DatosBinarios = dt.Rows(i).Item("DatosBinarios")
                                    Array.Clear(sop.DatosBinarios, 0, sop.DatosBinarios.Length)
                                Else
                                    .NombreOriginal = dt.Rows(i).Item("NombreOriginal")
                                    .DatosBinarios = dt.Rows(i).Item("DatosBinarios")
                                End If
                                .ContentType = dt.Rows(i).Item("contenType")
                            End With
                            .Soportes.Add(sop)
                        Next
                        .IdUsuarioRegistra = Session("usxp001")
                        .IdFacturaGuia = Session("idFacturaGuia")
                        If lblNumeroRecepcion.Text.Trim = "" Then
                            .IdOrdenRecepcion = 0
                        Else
                            .IdOrdenRecepcion = CInt(lblNumeroRecepcion.Text.Trim)
                        End If

                        .Descripcion = mmObservacion.Text.Trim
                        Dim resultado As ILSBusinessLayer.ResultadoProceso = r.Registrar()
                        If resultado.Valor = 0 Then
                            CType(source, ASPxPopupControl).JSProperties("cpResultado") = "999"
                        Else
                            lblMensaje.ForeColor = Color.Red
                            lblMensaje.Font.Bold = True
                            CType(source, ASPxPopupControl).JSProperties("cpResultado") = "-999"
                            CType(source, ASPxPopupControl).JSProperties("cpMensaje") = resultado.Mensaje
                            For i As Integer = 0 To .Soportes.Count - 1
                                If .Soportes(i).IdTipoSoporte = 1 Then
                                    File.Delete(.Soportes(i).RutaCompleta)
                                End If
                            Next
                        End If
                    End With
                Case "cargar"
                    lblRespuesta.Text = "-1"
                    CType(source, ASPxPopupControl).JSProperties("cpResultado") = "-1"
                    CType(source, ASPxPopupControl).JSProperties("cpMensaje") = ""
                    lblMensaje.Text = ""
                    mmObservacion.Text = ""
            End Select
        Catch ex As Exception
            EncabezadoPagina.showError("Imposible subir el archivo al Servidor: " & ex.Message)
            CType(source, ASPxPopupControl).JSProperties("cpResultado") = "-1"
            CType(source, ASPxPopupControl).JSProperties("cpMensaje") = "Imposible subir el archivo al Servidor: " & ex.Message
        End Try
    End Sub

    Private Sub ucImagen_FilesUploadComplete(sender As Object, e As DevExpress.Web.FilesUploadCompleteEventArgs) Handles ucImagen.FilesUploadComplete
        Try
            Dim dtImagenes As New DataTable
            Dim i As Integer
            If ucImagen.HasFile Then
                Dim imagenes As UploadedFile() = ucImagen.UploadedFiles
                If Session("dtImagenes") Is Nothing Then
                    With dtImagenes
                        .Columns.Add(New DataColumn("idImagen", GetType(Integer)))
                        .Columns.Add(New DataColumn("imagen", GetType(Byte())))
                        .Columns.Add(New DataColumn("contenType", GetType(String)))
                        .Columns.Add(New DataColumn("nombre", GetType(String)))
                        .Columns.Add(New DataColumn("peso", GetType(String)))
                    End With
                Else
                    dtImagenes = Session("dtImagenes")
                End If
                If dtImagenes.Rows.Count < 15 Then
                    For indexImg As Integer = 0 To imagenes.Length - 1
                        Dim imageResize As ImageResizer.ImageJob
                        Dim msImagenOrigen As New MemoryStream(imagenes(indexImg).FileBytes)
                        Dim msImagenDestino As New MemoryStream()
                        Dim settings As New ImageResizer.ResizeSettings()
                        With settings
                            .Width = 450
                            .Height = 450
                            .Mode = ImageResizer.FitMode.Max
                        End With
                        imageResize = New ImageResizer.ImageJob(msImagenOrigen, msImagenDestino, settings).Build()
                        Dim biteArray As Byte() = New Byte(msImagenDestino.Length) {}
                        msImagenDestino.Position = 0
                        msImagenDestino.Read(biteArray, 0, msImagenDestino.Length)
                        Dim dr As DataRow
                        dr = dtImagenes.NewRow
                        dr("idImagen") = dtImagenes.Rows.Count + 1
                        dr("imagen") = biteArray 'imagenes(indexImg).FileBytes
                        dr("contenType") = imagenes(indexImg).ContentType
                        Dim nombreImagen As String = validaNombre(imagenes(indexImg).FileName)
                        dr("nombre") = nombreImagen 'imagenes(indexImg).FileName
                        dr("peso") = imagenes(indexImg).ContentLength
                        dtImagenes.Rows.Add(dr)
                        If dtImagenes.Rows.Count = 15 Then
                            Exit For
                        End If
                        Session("dtImagenes") = dtImagenes
                    Next
                    CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = dtImagenes.Rows.Count
                    lblMensajeCantidad.Text = dtImagenes.Rows.Count
                End If
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Imposible subir los archivos al servidor: " & ex.Message)
        End Try
    End Sub

    Private Sub pcImagen_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles pcImagen.WindowCallback
        Try
            Dim arrAccion As String()
            arrAccion = e.Parameter.Split(":")
            Select Case arrAccion(1)
                Case "cargar"
                    Dim dtdatos As DataTable = Session("dtImagenes")
                    EnlazarImagenes(dtdatos)
            End Select
        Catch ex As Exception
            EncabezadoPagina.showError("Imposible obtener las imagenes: " & ex.Message)
        End Try
    End Sub

    Private Sub EnlazarImagenes(ByVal dtDatos As DataTable)
        With gvImagenes
            .PageIndex = 0
            .DataSource = dtDatos
            .DataBind()
        End With
    End Sub

    Protected Sub Link_Init(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim lnkEliminar As ASPxHyperLink = CType(sender, ASPxHyperLink)
            Dim templateContainer As GridViewDataItemTemplateContainer = CType(lnkEliminar.NamingContainer, GridViewDataItemTemplateContainer)
            lnkEliminar.ClientSideEvents.Click = lnkEliminar.ClientSideEvents.Click.Replace("{0}", templateContainer.KeyValue)

        Catch ex As Exception
            EncabezadoPagina.showError("No fué posible establecer los parametros: " & ex.Message)
        End Try
    End Sub

    Private Sub gvImagenes_CustomCallback(sender As Object, e As DevExpress.Web.ASPxGridViewCustomCallbackEventArgs) Handles gvImagenes.CustomCallback
        Try
            Dim arrAccion As String()
            arrAccion = e.Parameters.Split(":")
            Select Case arrAccion(1)
                Case "eliminar"
                    Dim dtdatos As DataTable = Session("dtImagenes")
                    Dim i As Integer
                    For i = 0 To dtdatos.Rows.Count - 1
                        If dtdatos.Rows(i).Item("idImagen") = arrAccion(0) Then
                            If FolderTempImage IsNot Nothing Then
                                Dim j As Integer
                                For j = 0 To Directory.GetFiles(Server.MapPath("~/recibos/images/ImagenRecepcion/") & FolderTempImage).Length - 1
                                    Dim nombreArchivo As String = Directory.GetFiles(Server.MapPath("~/recibos/images/ImagenRecepcion/") & FolderTempImage)(j).Split("\").GetValue(Directory.GetFiles(Server.MapPath("~/recibos/images/ImagenRecepcion/") & FolderTempImage)(j).Split("\").Length - 1)
                                    If nombreArchivo = dtdatos.Rows(i).Item("nombre") Then
                                        File.Delete(Server.MapPath("~/recibos/images/ImagenRecepcion/") & FolderTempImage & "\" & nombreArchivo)
                                        Exit For
                                    End If
                                Next
                            End If
                            dtdatos.Rows(i).Delete()
                            Exit For
                        End If
                    Next
                    CType(sender, ASPxGridView).JSProperties("cpCantidad") = dtdatos.Rows.Count
                    Session("dtImagenes") = dtdatos
                    EnlazarImagenes(dtdatos)
            End Select
        Catch ex As Exception
            EncabezadoPagina.showError("Imposible obtener las imagenes: " & ex.Message)
        End Try
    End Sub

    Private Sub gvImagenes_DataBinding(sender As Object, e As System.EventArgs) Handles gvImagenes.DataBinding
        If Session("dtImagenes") IsNot Nothing Then gvImagenes.DataSource = Session("dtImagenes")
    End Sub

    Private Sub pcVisor_WindowCallback(source As Object, e As DevExpress.Web.PopupWindowCallbackArgs) Handles pcVisor.WindowCallback
        Try
            Dim arrAccion As String()
            arrAccion = e.Parameter.Split(":")
            Select Case arrAccion(1)
                Case "visualizar"
                    If FolderTempImage Is Nothing Then
                        FolderTempImage = Guid.NewGuid().ToString()
                        Session("FolderTempImage") = FolderTempImage
                        Directory.CreateDirectory(Server.MapPath("~/recibos/images/ImagenRecepcion/") & FolderTempImage)
                    End If

                    Dim dtImagenes As DataTable = Session("dtImagenes")
                    Dim objImagen As New Imagen()
                    With objImagen
                        Dim i As Integer
                        For i = 0 To dtImagenes.Rows.Count - 1
                            .ArregloByte_Imagen(dtImagenes.Rows(i).Item("imagen"), Server.MapPath("~/recibos/images/ImagenRecepcion/") & FolderTempImage & "\" & dtImagenes.Rows(i).Item("nombre"), dtImagenes.Rows(i).Item("ContenType"))
                        Next
                    End With
                    isImagenes.ImageSourceFolder = "~/recibos/images/ImagenRecepcion/" & FolderTempImage
            End Select
        Catch ex As Exception
            EncabezadoPagina.showError("Imposible obtener las imagenes: " & ex.Message)
        End Try
    End Sub

    Private Function validaNombre(_nombre As String) As String
        Dim nombreAsignado As String
        Try
            If Session("dtImagenes") IsNot Nothing Then
                Dim dtImagenes As DataTable = Session("dtImagenes")
                Dim resultado() As DataRow = dtImagenes.Select("nombre like'%" & _nombre.Split(".").GetValue(0) & "%'")
                If resultado.Length > 0 Then
                    nombreAsignado = _nombre.Split(".").GetValue(0) & "_" & (resultado.Length) + 1 & "." & _nombre.Split(".").GetValue(1)
                Else
                    nombreAsignado = _nombre
                End If
            Else
                nombreAsignado = _nombre
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Imposible validar nombre: " & ex.Message)
        End Try
        Return nombreAsignado
    End Function

    Private Function EnviarNotificacion(ByVal _dtDatos As DataTable, ByVal IdTipoproducto As Integer) As ResultadoProceso
        Dim notificador As New NotificadorGeneralEventos
        Dim resultado As New ResultadoProceso
        With notificador
            .Titulo = "Recepción de Producto " & _dtDatos.Rows(0).Item("ordenRecepcion")
            .Asunto = "Notificación de Recepción de Producto"
            .Mensaje = "Relacionamos la guía recibida, a continuación se muestran los datos y las imagenes de la misma:"
            .dtImagenes = Session("dtImagenes")
            .dtDatos = _dtDatos

            If (IdTipoproducto = Enumerados.TipoProductoMaterial.PAPELERIA Or IdTipoproducto = Enumerados.TipoProductoMaterial.MATERIA_POP_PUBLICIDAD Or IdTipoproducto = Enumerados.TipoProductoMaterial.INSUMOS Or IdTipoproducto = Enumerados.TipoProductoMaterial.MERCHANDISING Or IdTipoproducto = Enumerados.TipoProductoMaterial.ACCESORIOS Or IdTipoproducto = Enumerados.TipoProductoMaterial.BONOS Or IdTipoproducto = Enumerados.TipoProductoMaterial.DUMMIES) Then
                .TipoNotificacion = AsuntoNotificacion.Tipo.RecepcionProductoPapeleria
            Else
                .TipoNotificacion = AsuntoNotificacion.Tipo.NotificacionRecepcionProducto
            End If

            resultado = .NotificacionEventoImagen()
        End With
        Return resultado
    End Function

    Private Sub CrearTablaSoporte()
        Dim dtSoporte As New DataTable
        With dtSoporte
            .Columns.Add(New DataColumn("IdTipoSoporte", GetType(Integer)))
            .Columns.Add(New DataColumn("IdUsuarioRegistra", GetType(Integer)))
            .Columns.Add(New DataColumn("RutaCompleta", GetType(String)))
            .Columns.Add(New DataColumn("NombreOriginal", GetType(String)))
            .Columns.Add(New DataColumn("DatosBinarios", GetType(Byte())))
            .Columns.Add(New DataColumn("contenType", GetType(String)))
            .Columns.Add(New DataColumn("Peso", GetType(Double)))
        End With
        Session("dtSoporte") = dtSoporte
    End Sub

End Class