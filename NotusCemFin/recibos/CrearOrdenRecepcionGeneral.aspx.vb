Imports ILSBusinessLayer
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Recibos

Partial Public Class CrearOrdenRecepcionGeneral
    Inherits System.Web.UI.Page

    Dim idTipoProducto As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        Try
            epNotificador.clear()
            If Not Me.IsPostBack Then
                CargarTiposDeRecepcion()
                CargarOrdenesDeCompra()
                ObtenerClienteExterno()
                ObtenerConsignatario()
                trConsignatario.Visible = False
            End If
            If Request.QueryString("tp") IsNot Nothing Then Integer.TryParse(Request.QueryString("tp"), idTipoProducto)
            If idTipoProducto = 0 Then idTipoProducto = 3
        Catch ex As Exception
            epNotificador.showError(ex.Message)
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

    Private Sub CargarTiposDeRecepcion()
        Dim ddl As ListControl = ddlTipoRecepcion
        Dim dtDatos As DataTable
        Dim filtro As New FiltroGeneral
        Try
            filtro.Activo = Enumerados.EstadoBinario.Activo
            dtDatos = TipoRecepcion.ObtenerListado(filtro)
            MetodosComunes.CargarDropDown(dtDatos, ddl, "Escoja un Tipo de Recepción")
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar Tipos de Recepción. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarOrdenesDeCompra()
        Dim ddl As ListControl = ddlOrdenCompra
        Dim dtDatos As DataTable
        Dim filtro As New FiltroOrdenCompra
        Dim numOrdenes As Integer = 0
        Try
            Dim arrayEstado As New ArrayList
            arrayEstado.Add(OrdenCompra.EstadoOrden.Abierta)
            arrayEstado.Add(OrdenCompra.EstadoOrden.Parcial)
            filtro.ListaEstado = arrayEstado
            filtro.IdTipoProducto = IIf(idTipoProducto > 0, idTipoProducto, 3)
            If txtFiltroOrdenCompra.Text.Trim.Length >= 2 Then filtro.IdNumeroOrden = txtFiltroOrdenCompra.Text.Trim
            dtDatos = OrdenCompra.ObtenerListado(filtro)
            numOrdenes = dtDatos.Rows.Count
            dtDatos.DefaultView.Sort = "numeroOrden asc"
            With ddlOrdenCompra
                .DataSource = dtDatos
                .DataTextField = "idNumeroOrden"
                .DataValueField = "idOrden"
                .DataBind()
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el listado de Órdenes de Compra. " & ex.Message)
        End Try
        lblNumOrdenesCompra.Text = numOrdenes.ToString & " Registro(s) Cargado(s)"
        ddlOrdenCompra.Items.Insert(0, New ListItem("Escoja una Orden", "0"))
        ddlOrdenCompra.Enabled = True
        txtFiltroOrdenCompra.Focus()
    End Sub

    Protected Sub txtFiltroOrdenCompra_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroOrdenCompra.TextChanged
        CargarOrdenesDeCompra()
    End Sub

    Protected Sub btnCrear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrear.Click
        Try
            Dim miOrden As New Recibos.OrdenRecepcion
            With miOrden
                .IdTipoProducto = idTipoProducto
                .IdTipoRecepcion = ddlTipoRecepcion.SelectedValue
                .IdOrdenCompra = ddlOrdenCompra.SelectedValue
                If trConsignatario.Visible Then
                    .IdConsignatario = ddlConsignado.SelectedValue
                Else
                    Dim consignatarioObj As New Recibos.Consignatario(True)
                    .IdConsignatario = consignatarioObj.IdConsignatario
                End If

                .IdClienteExterno = ddlClienteExterno.SelectedValue

                If txtRemision.Text.Trim.Length > 0 Then .Remision = txtRemision.Text.Trim()
                .IdCreador = IIf(Session("usxp001") IsNot Nothing, CLng(Session("usxp001")), 1)
                .IdEstado = 16
                If .Crear Then
                    epNotificador.showSuccess("La Orden de Recepción No. " & .IdOrdenRecepcion.ToString & " fue creada exitosamente.")
                    Dim ruta As String = Page.ResolveUrl("~/recibos/DetalleOrdenRecepcionGeneral.aspx?ord=" & .IdOrdenRecepcion.ToString & _
                                                         "&showSuccess=true")
                    Dim script As String = "if(window.frameElement){window.parent.location='" & ruta & "';} else {window.location='" & ruta & "';}"
                    ClientScript.RegisterClientScriptBlock(Me.GetType, "redireccionOC", script, True)
                End If
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de crear la orden de recepcion. " & ex.Message)
        End Try
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
            epNotificador.showError("Error al filtrar el tipo de recepción. " & ex.Message)
        End Try
    End Sub
End Class