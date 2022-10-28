Imports ILSBusinessLayer
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Recibos

Partial Public Class CrearOrdenRecepcionMerchanPopInsumo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            If Not IsPostBack Then
                hfIdTipoProducto.Value = CLng(Request.QueryString("tp"))
                ObtenerTipoRecepcion()
                CargarOrdenesDeCompra()
                CargarProveedores()
                ObtenerClienteExterno()
                ObtenerConsignatario()
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
        Try
            dtDatos = Recibos.Consignatario.ObtenerListado(filtro)
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
        Try
            dtDatos = ILSBusinessLayer.Comunes.ClienteExterno.ObtenerListado(filtro)
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

    Private Sub CargarOrdenesDeCompra()
        Dim ddl As ListControl = ddlOrdenCompra
        Dim dtDatos As DataTable
        Dim filtro As New FiltroOrdenCompra
        Dim numOrdenes As Integer = 0
        Try
            filtro.IdEstado = 16
            filtro.IdTipoProducto = IIf(CInt(hfIdTipoProducto.Value) > 0, CInt(hfIdTipoProducto.Value), 3)
            If txtFiltroOrden.Text.Trim.Length > 3 Then filtro.NumeroOrden = txtFiltroOrden.Text.Trim
            dtDatos = OrdenCompra.ObtenerListado(filtro)
            numOrdenes = dtDatos.Rows.Count
            With ddlOrdenCompra
                .DataSource = dtDatos
                .DataTextField = "numeroOrden"
                .DataValueField = "idOrden"
                .DataBind()
            End With
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de cargar el listado de Órdenes de Compra. " & ex.Message)
        End Try
        lblNumOrdenesCompra.Text = numOrdenes.ToString & " Registro(s) Cargado(s)"
        ddlOrdenCompra.Items.Insert(0, New ListItem("Escoja una Orden", "0"))
        ddlOrdenCompra.Enabled = True
    End Sub

    Private Sub CargarProveedores()
        Dim ddl As ListControl = ddlProveedor
        Dim dtDatos As DataTable
        Dim filtro As New FiltroGeneral
        Dim numProveedores As Integer = 0
        Try

            filtro.Activo = Enumerados.EstadoBinario.Activo

            If txtFiltroProveedor.Text.Trim.Length > 3 Then filtro.Nombre = txtFiltroProveedor.Text.Trim
            dtDatos = Proveedor.ObtenerListado(filtro, CInt(hfIdTipoProducto.Value))
            numProveedores = dtDatos.Rows.Count
            With ddlProveedor
                .DataSource = dtDatos
                .DataTextField = "nombre"
                .DataValueField = "idProveedor"
                .DataBind()
            End With
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar de cargar el listado de Proveedores. " & ex.Message)
        End Try
        lblNumProveedores.Text = numProveedores.ToString & " Registro(s) Cargado(s)"
        ddlProveedor.Items.Insert(0, New ListItem("Escoja un Proveedor", "0"))
        ddlProveedor.Enabled = True
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

    Protected Sub ObtenerOrdenCompra(ByVal dt As DataTable)
        Try
            With ddlOrdenCompra
                .DataSource = dt
                .DataTextField = "idNumeroOrden"
                .DataValueField = "idOrden"
                .DataBind()

                lblNumOrdenesCompra.Text = dt.Rows.Count.ToString() & " Registro(s) Cargado(s)"
                If dt.Rows.Count > 0 Then
                    .Items.Insert(0, New ListItem("Escoja la orden de compra", 0))
                Else
                    .Items.Insert(0, New ListItem("Escoja una Orden", 0))
                End If

            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener las ordenes de compra. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerProveedor(ByVal dt As DataTable)
        Try
            With ddlProveedor
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idProveedor"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Proveedor", 0))
                lblNumProveedores.Text = .Items.Count - 1.ToString & " Registro(s) Cargado(s)"
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los proveedores. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            Dim ordenRecepcion As New Recibos.OrdenRecepcion
            If Not ordenRecepcion.ExisteRemision(txtRemision.Text) Then
                With ordenRecepcion
                    .IdTipoProducto = CInt(hfIdTipoProducto.Value)
                    .IdTipoRecepcion = ddlTipoRecepcion.SelectedValue
                    .IdOrdenCompra = ddlOrdenCompra.SelectedValue
                    .IdProveedor = ddlProveedor.SelectedValue
                    .Remision = txtRemision.Text.Trim()
                    .IdCreador = CLng(Session("usxp001"))
                    .IdEstado = Recibos.OrdenRecepcion.EstadoOrden.Abierta
                    .IdClienteExterno = CInt(ddlClienteExterno.SelectedValue)
                    .Factura = txtFactura.Text.Trim()
                    .Guia = txtGuia.Text.Trim()
                    If trConsignatario.Visible Then
                        .IdConsignatario = ddlConsignado.SelectedValue
                    Else
                        Dim consignatarioObj As New Recibos.Consignatario(True)
                        .IdConsignatario = consignatarioObj.IdConsignatario
                    End If
                    If .Crear() Then
                        EncabezadoPagina.showSuccess("Orden Creada exitosamente.")
                        Dim rutaPagina As String = Request.Url.AbsoluteUri.Substring(0, (Request.Url.AbsoluteUri.LastIndexOf("/")))
                        'If .IdTipoProducto = Productos.TipoProducto.Tipo.INSUMOS Or .IdTipoProducto = Productos.TipoProducto.Tipo.MATERIAL_POP Then
                        '    rutaPagina += "/DetalleOrdenRecepcionPopInsumo.aspx" & "?ord=" & .IdOrdenRecepcion.ToString & "&showSuccess=true"
                        'ElseIf .IdTipoProducto = Productos.TipoProducto.Tipo.MERCHANDISING Then
                        rutaPagina += "/DetalleOrdenRecepcionGeneral.aspx" & "?ord=" & .IdOrdenRecepcion.ToString & "&showSuccess=true"
                        'End If
                        ClientScript.RegisterClientScriptBlock(Me.GetType, "Redireccionar", "window.parent.location='" & rutaPagina & "';", True)
                    End If
                End With
            Else
                EncabezadoPagina.showError("La remisión: " & txtRemision.Text & " ya existe.")
            End If

        Catch ex As Exception
            EncabezadoPagina.showError("Error al crear la orden de recepcion. " & ex.Message)
        End Try
    End Sub


    Protected Sub FiltrarOrden(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroOrden.TextChanged
        Try
            Dim dt As New DataTable
            'If Session("dtOrdenes") Is Nothing Then
            '    dt = Recibos.OrdenCompra.ObtenerListado
            'Else
            '    dt = CType(HttpContext.Current.Session("dtOrdenes"), DataTable)
            'End If
            If txtFiltroOrden.Text.Length > 3 Then
                Dim filtroOrden As New Estructuras.FiltroOrdenCompra
                Dim arrayEstado As New ArrayList
                arrayEstado.Add(OrdenCompra.EstadoOrden.Abierta)
                arrayEstado.Add(OrdenCompra.EstadoOrden.Parcial)
                With filtroOrden
                    .IdTipoProducto = CInt(hfIdTipoProducto.Value)
                    .ListaEstado = arrayEstado
                    .IdNumeroOrden = txtFiltroOrden.Text
                End With
                dt = Recibos.OrdenCompra.ObtenerListado(filtroOrden)
                'dt.DefaultView.RowFilter = "numeroOrden like '%" + txtFiltroOrden.Text + "%'  "
                dt.DefaultView.Sort = "numeroOrden asc"
                'Session("dtOrdenes") = dt
                ObtenerOrdenCompra(dt)
                Dim filtro As New Estructuras.FiltroInfoFactura
                filtro.IdOrdenCompra = ddlOrdenCompra.SelectedValue
            Else               
                ObtenerOrdenCompra(dt)
            End If
            txtFiltroOrden.Focus()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar la orden. " & ex.Message)
        End Try
    End Sub

    Protected Sub FiltrarProveedor(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroProveedor.TextChanged
        Try
            Dim dt As New DataTable
            Dim filtroProveedor As New FiltroGeneral
            filtroProveedor.Activo = Enumerados.EstadoBinario.Activo
            If Session("dtProveedores") Is Nothing Then
                dt = Proveedor.ObtenerListado(filtroProveedor)
            Else
                dt = CType(HttpContext.Current.Session("dtProveedores"), DataTable)
            End If
            If txtFiltroProveedor.Text.Length > 3 Then
                dt.DefaultView.RowFilter = "nombre like '%" + txtFiltroProveedor.Text + "%'  "
                dt.DefaultView.Sort = "nombre asc"
                Session("dtProveedores") = dt
                ObtenerProveedor(dt)
            Else
                dt = Proveedor.ObtenerListado(filtroProveedor)
                ObtenerProveedor(dt)
            End If
            txtFiltroProveedor.Focus()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar el proveedor. " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlOrdenCompra_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlOrdenCompra.SelectedIndexChanged
        Try
            If ddlOrdenCompra.SelectedValue > 0 Then
                Dim filtro As New Estructuras.FiltroInfoFactura
                filtro.IdOrdenCompra = ddlOrdenCompra.SelectedValue
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar por las ordenes. " & ex.Message)
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