Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer

Partial Public Class DetalleOrdenRecepcionPopInsumo
    Inherits System.Web.UI.Page

    Private idOrdenRecepcion As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epNotificador.clear()
        epAuxNotificacion.clear()
        With Request.QueryString
            If .Item("ord") IsNot Nothing Then Integer.TryParse(.Item("ord"), idOrdenRecepcion)
        End With
        If Not Me.IsPostBack Then
            epNotificador.setTitle("Detalle de Orden de Recepción")
            epNotificador.showReturnLink(MetodosComunes.getUrlFrameBack(Me))
            Session.Remove("dtDetalleOrdenCompra")
            Session.Remove("dtCajasTemporales")
            If idOrdenRecepcion > 0 Then
                With Request.QueryString
                    If .Item("showSuccess") IsNot Nothing Then _
                        epNotificador.showSuccess("La Orden de Recepción No. " & .Item("ord") & " fue creada satisfactoriamente.")
                End With
                CargarInfoGeneralOrdenRecepcion(idOrdenRecepcion)
                CargarProductos()
                CargarNovedades()
                CargarPallets()
                CalcularCantidadDisponibleTotal()
                If (hfIdEstadoOrdenRecepcion.Value = "16" Or hfIdEstadoOrdenRecepcion.Value = "17") Then
                    CargarCajasTemporales()
                Else
                    lbCerrarOrden.Visible = False
                    pnlCreacionPallet.Visible = False
                    epNotificador.showWarning("La Orden de Recepción No. " & idOrdenRecepcion.ToString & " ya se encuentra cerrada.")
                End If                
                lbCrearPallet.Focus()
            Else
                epNotificador.showError("Imposible recuperar el identificador de la Orden de Recepción. Por favor recargue la página.")
                pnlGeneral.Visible = False
            End If
        End If
    End Sub

    Private Sub CargarInfoGeneralOrdenRecepcion(ByVal idOrden As Integer)
        Try
            Dim miOrden As New OrdenRecepcion(idOrden)
            If miOrden.IdOrdenRecepcion > 0 Then                
                With miOrden
                    lblOrdenRecepcion.Text = .IdOrdenRecepcion
                    lblFechaRecepcion.Text = .FechaRecepcion.ToShortDateString()
                    lblProveedor.Text = .Proveedor
                    If .IdOrdenCompra > 0 Then
                        fOrdenCompra.Visible = True
                        lblOrdenCompra.Text = .NumeroOrdenCompra

                        hfIdOrdenCompra.Value = .IdOrdenCompra                        
                    Else
                        fOrdenCompra.Visible = False
                    End If

                    lblRemision.Text = .Remision
                    lblTipoProducto.Text = .TipoProducto
                    lblTipoRecepcion.Text = .TipoRecepcion

                    hfIdEstadoOrdenRecepcion.Value = .IdEstado
                    hfIdTipoProducto.Value = .IdTipoProducto
                End With

            Else
                epNotificador.showWarning("Imposible recuperar la información de la Orden de Recepción desde la BD. Por favor recargue la página.")
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener la información general de la Orden de Recepción. " & ex.Message)
        End Try
    End Sub

    Function FiltrarDataTable(ByVal poDataTable As DataTable, ByVal psFiltro As String, Optional ByVal psOrder As String = "") As DataTable
        Dim loRows As DataRow()
        Dim loNuevoDataTable As DataTable
        ' Copio la estructura del DataTable original
        loNuevoDataTable = poDataTable.Clone()
        ' Establezco el filtro y el orden
        If psOrder = "" Then
            loRows = poDataTable.Select(psFiltro)
        Else
            loRows = poDataTable.Select(psFiltro, psOrder)
        End If
        ' Cargo el nuevo DataTable con los datos filtrados
        For Each ldrRow As DataRow In loRows
            loNuevoDataTable.ImportRow(ldrRow)
        Next
        ' Retorno el nuevo DataTable
        Return loNuevoDataTable
    End Function

    Private Sub CargarProductos()
        Dim idOrdenCompra As Integer = 0
        Integer.TryParse(hfIdOrdenCompra.Value, idOrdenCompra)
        Dim dtProductos As New DataTable
        Dim campoProducto As String
        Try
            If idOrdenCompra > 0 Then
                dtProductos = OrdenCompra.ObtenerDetalle(idOrdenCompra)
                campoProducto = "producto"
            Else
                Dim filtroProducto As New Estructuras.FiltroProducto
                filtroProducto.Activo = Enumerados.EstadoBinario.Activo
                filtroProducto.IdTipoProducto = CShort(hfIdTipoProducto.Value)
                dtProductos = Productos.Producto.ObtenerListado(filtroProducto)
                campoProducto = "nombre"
            End If
            If txtFiltroProducto.Text.Trim.Length > 3 Then dtProductos = FiltrarDataTable(dtProductos, campoProducto & " LIKE '" & txtFiltroProducto.Text.Trim & "'")
            With ddlProducto
                .DataSource = dtProductos
                .DataTextField = campoProducto
                .DataValueField = "idProducto"
                .DataBind()
                If .Items.Count > 1 Then : .Items.Insert(0, New ListItem("Escoja el Producto", 0))
                ElseIf .Items.Count = 0 Then : inicializaDropDownList(ddlProducto)
                End If
            End With
            Session("dtDetalleOrdenCompra") = dtProductos
        Catch ex As Exception
            epNotificador.showError("Error al tratar de obtener el listado de Productos relacionados a la Orden de Compra")
        End Try
        If ddlProducto.Items.Count <> 1 Then
            ddlProducto.Items.Insert(0, New ListItem("Escoja un Producto", "0"))
            'Else
            '    txtFiltroProducto.Enabled = False
        End If
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
            lbCrearPallet.Enabled = CBool(dtCaja.Rows.Count)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar Cajas temporalmente registradas. " & ex.Message)
        End Try
    End Sub

    Private Sub gvPallets_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPallets.RowCommand
        If e.CommandName = "Imprimir" Then
            Dim idPallet As Long
            Long.TryParse(e.CommandArgument, idPallet)
            ImprimirHojaViajera(idPallet)
        End If
    End Sub

    Private Sub gvPallets_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPallets.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(4).Text = e.Row.Cells(4).Text.Replace(",", "<br/>")
            Dim idPallet As Integer
            Integer.TryParse(e.Row.Cells(1).Text, idPallet)
            Try
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
        Dim cajaCargada As CajaEmpaque
        Try
            With miCaja
                .IdOrdenRecepcion = idOrdenRecepcion
                Integer.TryParse(ddlProducto.SelectedValue, .IdProducto)                
                Integer.TryParse(txtCantidad.Text, .Cantidad)
                If Session("usxp001") IsNot Nothing Then Integer.TryParse(Session("usxp001"), .IdCreador)
                .IdTipoDetalleProducto = TipoDetalleOrdenCompra.TipoDetalle.Principal
                resultado = .Registrar()
                If resultado = 0 Then
                    cajaCargada = New CajaEmpaque(CInt(.IdCaja))                    
                    Dim codigoJavascript As String = "form1.myControl1.generar('" & cajaCargada.Producto & "', '" & cajaCargada.Producto & "', '" & cajaCargada.FechaRegistro.ToString & "', '" & cajaCargada.Cantidad & "');"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "generaStiker", codigoJavascript, True)
                    epAuxNotificacion.showSuccess("La Caja fue adicionada satisfactoriamente.")
                    LimpiarFormularioAdicionCaja()
                    CargarCajasTemporales()
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
        ddlProducto.Focus()
    End Sub

    Protected Sub lbCrearPallet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbCrearPallet.Click
        Try
            Dim miPallet As New PalletRecepcion
            With miPallet                                                
                .IdOrdenRecepcion = idOrdenRecepcion
                If Session("usxp001") IsNot Nothing Then Integer.TryParse(Session("usxp001"), .IdCreador)
                Decimal.TryParse(txtPeso.Text, .Peso)
                .Observacion = txtObservacion.Text.Trim
                For Each liNovedad As ListItem In cblNovedad.Items
                    If liNovedad.Selected Then .AdicionarNovedad(CInt(liNovedad.Value))
                Next
                Dim dtCajas As DataTable = CType(Session("dtCajasTemporales"), DataTable)
                If .CrearConCajasSinRegion(dtCajas) Then
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
        If txtFiltroProducto.Text.Trim.Length > 0 Then CargarProductos()
        txtFiltroProducto.Text = ""
        ddlProducto.ClearSelection()
        CargarProductos()
        txtCantidad.Text = ""
    End Sub

    Private Sub LimpiarFormularioCrearPallet()
        If txtFiltroProducto.Text.Trim.Length > 0 Then CargarProductos()
        txtFiltroProducto.Text = ""
        ddlProducto.ClearSelection()        
        txtCantidad.Text = ""
        txtPeso.Text = ""
        txtObservacion.Text = ""
        cblNovedad.ClearSelection()
        CargarCajasTemporales()
        CargarPallets()        
        CalcularCantidadDisponibleTotal()
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

    Private Sub ImprimirHojaViajera(ByVal idPallet As Integer)
        Try
            Dim rpt As New ReporteCrystal("resumenPalletRecepcion", Server.MapPath("~/Reports"))
            rpt.agregarParametroDiscreto("@idPallet", idPallet)
            Dim ruta As String = rpt.exportar(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
            ruta = ruta.Substring(ruta.LastIndexOf("\") + 1)
            ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "newWindow", "window.open('../Reports/rptTemp/" + ruta + "','Impresion_Viajera', 'status=1, toolbar=0, location=0,menubar=1,directories=0,resizable=1,scrollbars=1'); ", True)
        Catch ex As Exception
            epNotificador.showError("Error al tratar de generar el documento. " & ex.Message)
        End Try
        Dim bl As New BulletedList

    End Sub

    Protected Sub txtFiltroProducto_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroProducto.TextChanged
        CargarProductos()
    End Sub

    Private Sub inicializaDropDownList(ByRef control As DropDownList)
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem("Seleccione...", 0))
    End Sub
End Class