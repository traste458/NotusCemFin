Imports ILSBusinessLayer
Imports ILSBusinessLayer.Recibos

Partial Public Class CrearOrdenCompraMerchanPopInsumoP1
    Inherits System.Web.UI.Page

    Private TipoProductoObj As Productos.TipoProducto

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epEncabezado.clear()
        If Not Me.IsPostBack Then
            Try                
                If Request.QueryString.Item("tp") IsNot Nothing Then
                    hfIdTipoProducto.Value = Request.QueryString("tp").ToString
                    epEncabezado.setTitle("Crear Orden Compra")
                    epEncabezado.showReturnLink("CrearOrdenCompra.aspx")
                    TipoProductoObj = New Productos.TipoProducto(CInt(hfIdTipoProducto.Value))
                    lblTipoProducto.Text = "Orden de compra para el tipo de producto " & TipoProductoObj.Descripcion
                    CargarProveedores()
                    'CargarRemisiones()
                Else
                    contenedor.Visible = False
                    epEncabezado.showError("Error al cargar la pagina, por favor actualizar")
                End If

                'If Request.UrlReferrer IsNot Nothing Then
                '    epEncabezado.showReturnLink(Request.UrlReferrer.ToString)
                'Else
                '    epEncabezado.showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                'End If
            Catch ex As Exception
                epEncabezado.showError("Error al cargar la pagina " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub CargarRemisiones(ByVal idProveedor As Integer)
        Try
            Dim filtroRemision As New Estructuras.FiltroOrdenRecepcion
            Dim dt As New DataTable
            Dim estados As New ArrayList
            'estados.Add(Recibos.OrdenRecepcion.EstadoOrden.Abierta)
            'estados.Add(Recibos.OrdenRecepcion.EstadoOrden.Parcial)
            estados.Add(Recibos.OrdenRecepcion.EstadoOrden.Finalizada)
            With filtroRemision
                .ListaEstado = estados
                .IdProveedor = idProveedor
                .IdTipoProducto = CInt(hfIdTipoProducto.Value)
                .IdOrdenCompra = -1
            End With

            dt = Recibos.OrdenRecepcion.ObtenerListado(filtroRemision)
            Session("listadoRemisiones") = dt
            gvRemisiones.DataSource = dt
            gvRemisiones.DataBind()
            filtroRemision.IdTipoProducto = CInt(hfIdTipoProducto.Value)
            If dt.Rows.Count > 0 Then
                btnEnviar.Visible = True
                lblMensaje.Visible = True
            Else
                btnEnviar.Visible = False
                lblMensaje.Visible = False
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al cargar las remisiones. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarProveedores()
        Dim ddl As ListControl = ddlProveedor
        Dim dtDatos As DataTable
        Dim filtro As New Estructuras.FiltroGeneral
        Dim numProveedores As Integer = 0
        Try
            filtro.Activo = Enumerados.EstadoBinario.Activo

            dtDatos = Proveedor.ObtenerListado(filtro, CInt(hfIdTipoProducto.Value))
            numProveedores = dtDatos.Rows.Count
            With ddlProveedor
                .DataSource = dtDatos
                .DataTextField = "nombre"
                .DataValueField = "idProveedor"
                .DataBind()
            End With
        Catch ex As Exception
            epEncabezado.showError("Error al tratar de cargar el listado de Proveedores. " & ex.Message)
        End Try
        lblNumProveedores.Text = numProveedores.ToString & " Registro(s) Cargado(s)"
        ddlProveedor.Items.Insert(0, New ListItem("Escoja un Proveedor", "0"))
        ddlProveedor.Enabled = True
    End Sub

    Protected Sub gvRemisiones_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRemisiones.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
                Dim idOrdenRecepcion As Integer = CInt(fila("idOrdenRecepcion"))
                Dim ProductosAgregados As BulletedList = CType(e.Row.FindControl("bltProductosAgregados"), BulletedList)
                Dim Pallets As BulletedList = CType(e.Row.FindControl("bltPalletsAgregados"), BulletedList)
                Dim camposGrupo As New ArrayList(1)
                camposGrupo.Add("nombreProducto")
                Dim filtroPallet As New Estructuras.FiltroPalletRecepcion
                filtroPallet.IdOrdenRecepcion = idOrdenRecepcion
                filtroPallet.IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Principal
                filtroPallet.IdEstado = 57
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
        Dim remisionArray As New ArrayList
        Dim chk As CheckBox
        Dim idOrdenRecepcion As Long
        Try
            For Each row As GridViewRow In gvRemisiones.Rows
                chk = CType(row.FindControl("chkRemision"), CheckBox)
                idOrdenRecepcion = CLng(row.Cells(2).Text)
                If chk.Checked Then
                    remisionArray.Add(idOrdenRecepcion)
                End If
            Next
            If remisionArray.Count > 0 Then
                Session("remisionSeleccionada") = remisionArray
            End If
            Response.Redirect("~/recibos/CrearOrdenCompraMerchanPopInsumoP2.aspx?tp=" & hfIdTipoProducto.Value.ToString(), False)
        Catch ex As Exception
            epEncabezado.showError("Error  al enviar las ordenes seleccionadas " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlProveedor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlProveedor.SelectedIndexChanged
        Try
            Dim idProveedor As Integer = CInt(ddlProveedor.SelectedValue)
            If idProveedor > 0 Then
                CargarRemisiones(idProveedor)
            End If
        Catch ex As Exception
            epEncabezado.showError("Error al realizar el filtro de proveedor. " & ex.Message)
        End Try
    End Sub
End Class