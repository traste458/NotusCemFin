Imports ILSBusinessLayer
'Version anterior estable filtro orden compra 1179 siguiente version estable 1181
Partial Public Class CrearOrdenRecepcionTelefonoSim
    Inherits System.Web.UI.Page
    Protected _idTipoProducto As Integer
    Protected ReadOnly Property IdTipoProducto() As Integer
        Get
            If _idTipoProducto < 1 Then Integer.TryParse(Request.QueryString("tp"), _idTipoProducto)
            Return _idTipoProducto
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            If Not IsPostBack Then
                If Request.QueryString IsNot Nothing Then Integer.TryParse(Request.QueryString("tp"), _idTipoProducto)
                ObtenerTipoRecepcion()
                ObtenerClienteExterno()
                ObtenerConsignatario()
                inicializaDropDownList(ddlOrdenCompra)
                inicializaDropDownList(ddlFactura)
                inicializaDropDownList(ddlGuia)
                trConsignatario.Visible = False
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerConsignatario()
        Dim filtro As New Estructuras.FiltroConsignatario
        Dim dtDatos As New DataTable()
        filtro.Activo = Enumerados.EstadoBinario.Activo
        dtDatos = Recibos.Consignatario.ObtenerListado(filtro)
        Try
            With ddlConsignado
                .DataSource = dtDatos
                .DataTextField = "nombre"
                .DataValueField = "idConsignatario"
                .DataBind()
                If dtDatos.Rows.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el consignatario", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos para el campo consignado. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerClienteExterno()
        Dim filtro As New Estructuras.FiltroClienteExterno
        Dim dtDatos As New DataTable()
        filtro.Activo = Enumerados.EstadoBinario.Activo
        dtDatos = ILSBusinessLayer.Comunes.ClienteExterno.ObtenerListado(filtro)
        Try
            With ddlClienteExterno
                .DataSource = dtDatos
                .DataTextField = "nombre"
                .DataValueField = "idClienteExterno"
                .DataBind()
                If dtDatos.Rows.Count > 1 Then .Items.Insert(0, New ListItem("Escoja el tipo el Destinatario", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos para el campo consignado. " & ex.Message)
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


    Protected Sub CargarOrdenCompra()
        Try
            Dim dt As New DataTable            
            If txtFiltroOrden.Text.Length >= 2 Then
                'If Session("dtOrdenes") Is Nothing Then
                Dim filtroOrden As New Estructuras.FiltroOrdenCompra
                Dim estados As New ArrayList
                estados.Add(Recibos.OrdenCompra.EstadoOrden.Abierta)
                estados.Add(Recibos.OrdenCompra.EstadoOrden.Parcial)
                filtroOrden.ListaEstado = estados
                filtroOrden.IdTipoProducto = Me.IdTipoProducto
                filtroOrden.IdNumeroOrden = txtFiltroOrden.Text
                filtroOrden.CantidadPendiente = Enumerados.EstadoBinario.Activo
                dt = Recibos.OrdenCompra.ObtenerListado(filtroOrden)
                'Else
                '   dt = CType(HttpContext.Current.Session("dtOrdenes"), DataTable)
                'End If
                'dt.DefaultView.RowFilter = "numeroOrden like '%" + txtFiltroOrden.Text + "%'  "
                dt.DefaultView.Sort = "idNumeroOrden asc"

                If dt.DefaultView.Count > 0 Then
                    With ddlOrdenCompra
                        .DataSource = dt
                        .DataTextField = "idNumeroOrden"
                        .DataValueField = "idOrden"
                        .DataBind()
                        If dt.DefaultView.Count > 1 Then .Items.Insert(0, New ListItem("Escoja la orden de compra", 0))
                    End With
                    CargarFacturas(CLng(ddlOrdenCompra.SelectedValue))
                Else
                    inicializaDropDownList(ddlOrdenCompra)
                End If
            Else
                inicializaDropDownList(ddlOrdenCompra)
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener las ordenes de compra. " & ex.Message)
        End Try
    End Sub

    Protected Sub FiltrarOrden(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroOrden.TextChanged
        Try            
            CargarOrdenCompra()           
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar la orden. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarFacturas(Optional ByVal idOrden As Long = 0, Optional ByVal idGuia As Long = 0)
        Try
            Dim dt As New DataTable
            'If Session("dtFacturas") Is Nothing Then
            '    Dim filtroFactuas As New Estructuras.FiltroInfoFactura
            '    Dim estados As New ArrayList
            '    estados.Add(Recibos.OrdenCompra.EstadoOrden.Abierta)
            '    estados.Add(Recibos.OrdenCompra.EstadoOrden.Parcial)
            '    filtroFactuas.ListaEstado = estados
            '    If idOrden > 0 Then filtroFactuas.IdOrdenCompra = CInt(idOrden)
            '    If idGuia > 0 Then filtroFactuas.IdGuia = CInt(idGuia)
            '    dt = Recibos.InfoFactura.ObtenerListado(filtroFactuas)
            'Else
            '    dt = CType(HttpContext.Current.Session("dtFacturas"), DataTable)
            'End If
            If txtFiltroFactura.Text.Length >= 2 Or idOrden > 0 Or idGuia > 0 Then
                Dim filtroFacturas As New Estructuras.FiltroInfoFactura
                Dim estados As New ArrayList
                estados.Add(Recibos.OrdenCompra.EstadoOrden.Abierta)
                estados.Add(Recibos.OrdenCompra.EstadoOrden.Parcial)
                filtroFacturas.ListaEstado = estados
                filtroFacturas.Factura = txtFiltroFactura.Text
                If idOrden > 0 Then filtroFacturas.IdOrdenCompra = CInt(idOrden)
                If idGuia > 0 And ddlFactura.SelectedValue = "0" Then filtroFacturas.IdGuia = CInt(idGuia)
                dt = Recibos.InfoFactura.ObtenerListado(filtroFacturas)
                'dt.DefaultView.RowFilter = "factura like '%" + txtFiltroFactura.Text + "%'  "
                dt.DefaultView.Sort = "factura asc"
                If dt.DefaultView.Count > 0 Then
                    'Session("dtOrdenes") = dt
                    'CargarOrdenCompra()
                    With ddlFactura
                        .DataSource = dt
                        .DataTextField = "factura"
                        .DataValueField = "idFactura"
                        .DataBind()
                        If dt.DefaultView.Count > 1 Then .Items.Insert(0, New ListItem("Escoja la factura", 0))
                    End With
                    CargarGuias(CLng(ddlFactura.SelectedValue))
                Else
                    'Session.Remove("dtOrdenes")
                    inicializaDropDownList(ddlFactura)
                End If
            Else
                inicializaDropDownList(ddlFactura)
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar las facturas. " & ex.Message)
        End Try
    End Sub

    Protected Sub FiltrarFactura(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroFactura.TextChanged
        Try
            CargarFacturas(ddlOrdenCompra.SelectedValue)        
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar la orden. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarGuias(Optional ByVal idFactura As Long = 0)
        Try
            Dim dt As New DataTable
            'If Session("dtGuias") Is Nothing Then
            '    Dim filtroGuia As New Estructuras.FiltroInfoGuia
            '    Dim estados As New ArrayList
            '    estados.Add(Recibos.OrdenCompra.EstadoOrden.Abierta)
            '    estados.Add(Recibos.OrdenCompra.EstadoOrden.Parcial)
            '    filtroGuia.ListaEstado = estados
            '    If idFactura > 0 Then filtroGuia.IdFactura = CInt(idFactura)
            '    dt = Recibos.InfoGuia.ObtenerListado(filtroGuia)
            'Else
            '    dt = CType(HttpContext.Current.Session("dtGuias"), DataTable)
            'End If

            If txtFiltroGuia.Text.Length >= 2 Or idFactura > 0 Then
                Dim filtroGuia As New Estructuras.FiltroInfoGuia
                Dim estados As New ArrayList
                estados.Add(Recibos.OrdenCompra.EstadoOrden.Abierta)
                estados.Add(Recibos.OrdenCompra.EstadoOrden.Parcial)
                filtroGuia.ListaEstado = estados
                filtroGuia.Guia = txtFiltroGuia.Text
                If idFactura > 0 Then filtroGuia.IdFactura = CInt(idFactura)
                dt = Recibos.InfoGuia.ObtenerListado(filtroGuia)
                'dt.DefaultView.RowFilter = "guia like '%" + txtFiltroGuia.Text + "%'  "
                dt.DefaultView.Sort = "guia asc"
                If dt.DefaultView.Count > 0 Then
                    With ddlGuia
                        .DataSource = dt
                        .DataTextField = "guiTransp"
                        .DataValueField = "idGuia"
                        .DataBind()
                        If dt.DefaultView.Count > 1 Then .Items.Insert(0, New ListItem("Escoja la guia", 0))
                    End With
                Else
                    inicializaDropDownList(ddlGuia)
                End If
            Else
                inicializaDropDownList(ddlGuia)
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar las guias. " & ex.Message)
        End Try
    End Sub

    Protected Sub FiltrarGuia(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroGuia.TextChanged, txtFiltroGuia.TextChanged
        Try
            CargarGuias(CLng(ddlFactura.SelectedValue))
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar la orden. " & ex.Message)
        End Try
    End Sub


    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            Dim ordenRecepcion As New Recibos.OrdenRecepcion
            With ordenRecepcion
                .IdTipoProducto = Me.IdTipoProducto
                .IdTipoRecepcion = ddlTipoRecepcion.SelectedValue
                .IdOrdenCompra = ddlOrdenCompra.SelectedValue
                Dim infoFacturaGuia As New Recibos.FacturaGuia(CLng(ddlFactura.SelectedValue), CLng(ddlGuia.SelectedValue))
                .IdFacturaGuia = infoFacturaGuia.IdFacturaGuia
                If txtRemision.Text.Trim() <> "" Then
                    .Remision = txtRemision.Text.Trim()
                End If
                If trConsignatario.Visible Then
                    .IdConsignatario = ddlConsignado.SelectedValue
                Else
                    Dim consignatarioObj As New Recibos.Consignatario(True)
                    .IdConsignatario = consignatarioObj.IdConsignatario
                End If
                .IdClienteExterno = ddlClienteExterno.SelectedValue
                .IdCreador = CLng(Session("usxp001"))
                .IdEstado = 16

                If .Crear Then
                    CambioEstadoFacturaGuia()
                    EncabezadoPagina.showSuccess("Orden Creada exitosamente.")
                    Dim rutaPagina As String = Request.Url.AbsoluteUri.Substring(0, (Request.Url.AbsoluteUri.LastIndexOf("/")))
                    rutaPagina += "/CrearDetalleRecepcion.aspx" & "?orep=" & .IdOrdenRecepcion & "&facGui=" & infoFacturaGuia.IdFacturaGuia.ToString

                    ClientScript.RegisterClientScriptBlock(Me.GetType, "Redireccionar", "window.parent.location='" & rutaPagina & "';", True)
                End If
            End With

        Catch ex As Exception
            EncabezadoPagina.showError("Error al crear la orden de recepcion. " & ex.Message)
        End Try
    End Sub

    Private Sub CambioEstadoFacturaGuia()
        Try
            Dim infoOrdenCompra As New Recibos.OrdenCompra(CLng(ddlOrdenCompra.SelectedValue))
            Dim factura As New Recibos.InfoFactura(CLng(ddlFactura.SelectedValue))
            Dim guia As New Recibos.InfoGuia(CLng(ddlGuia.SelectedValue))
            With infoOrdenCompra
                .IdEstado = 17
                .Actualizar()
            End With

            With factura
                .IdEstado = 17
                .Actualizar()
            End With

            With guia
                .IdEstado = 17
                .Actualizar()
            End With

        Catch ex As Exception
            EncabezadoPagina.showError("Error al cambiar el estado de la factura y la guia. " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlOrdenCompra_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrdenCompra.SelectedIndexChanged
        Try
            CargarFacturas(ddlOrdenCompra.SelectedValue, ddlGuia.SelectedValue)
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar por las ordenes. " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlGuia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlGuia.SelectedIndexChanged
        Try
            CargarFacturas(ddlOrdenCompra.SelectedValue, ddlGuia.SelectedValue) 
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar por las guias. " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlFactura_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFactura.SelectedIndexChanged
        Try
            CargarGuias(ddlFactura.SelectedValue)
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar por las facturas. " & ex.Message)
        End Try
    End Sub

    Private Sub inicializaDropDownList(ByRef control As DropDownList)
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem("Seleccione...", 0))
    End Sub

    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ClientScript.RegisterClientScriptBlock(Me.GetType, "codModificarFrameAlto", "modificarAltoFramePadre();", True)
    End Sub

    Protected Sub ddlTipoRecepcion_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoRecepcion.SelectedIndexChanged
        Try
            Dim idTipoRecepcion As Integer
            idTipoRecepcion = CInt(ddlTipoRecepcion.SelectedValue)
            Dim tipoRecepcionObj As New Recibos.TipoRecepcion(idTipoRecepcion)            
            trConsignatario.Visible = tipoRecepcionObj.RequiereConsignatario
            rfvConsignado.EnableClientScript = tipoRecepcionObj.RequiereConsignatario
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar el tipo de recepción. " & ex.Message)
        End Try
    End Sub
End Class