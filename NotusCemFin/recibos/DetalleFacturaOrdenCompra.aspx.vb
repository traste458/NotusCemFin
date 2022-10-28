Imports ILSBusinessLayer
Partial Public Class DetalleFacturaOrdenCompra
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            EncabezadoPagina.clear()
            If Not IsPostBack Then
                If Request.QueryString("idfactura") IsNot Nothing Then
                    hfIdFactura.Value = CInt(Request.QueryString("idfactura"))
                    Dim factura As New Recibos.InfoFactura(CLng(hfIdFactura.Value))
                    Dim detalleOrdenCompraObj As New Recibos.DetalleOrdenCompra(factura.IdDetalleOrdenCompra)
                    hfIdOrdenCompra.Value = detalleOrdenCompraObj.IdOrden
                    EncabezadoPagina.setTitle("Detalle de la Factura (" & factura.Factura & ") de Orden de Compra")
                    CargarDetalleFactura()
                    EncabezadoPagina.showReturnLink("~/recibos/DetalleOrdenCompra.aspx?doc=" & factura.IdDetalleOrdenCompra.ToString)
                Else
                    EncabezadoPagina.showWarning("No se especifico ninguna factura, por favor verificar.")
                End If
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Private Sub CargarDetalleFactura()
        Dim filtro As New Estructuras.FiltroInfoGuia
        filtro.IdFactura = CInt(hfIdFactura.Value)
        Try
            gvGuiasAgregadas.DataSource = Recibos.InfoGuia.ObtenerListado(filtro)
            gvGuiasAgregadas.DataBind()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar el detalle de la factura")
        End Try
    End Sub

    Protected Sub gvGuiasAgregadas_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvGuiasAgregadas.RowCommand
        Try
            If e.CommandName = "editarGuia" Then
                obtenerTransportadora()
                ObtenerPaisGuia()                
                hfIdGuiaEdicionActual.Value = e.CommandArgument.ToString
                Dim infoFacturaGuia As New Recibos.FacturaGuia(CLng(hfIdFactura.Value), CLng(hfIdGuiaEdicionActual.Value))
                CargarEdicionGuia(infoFacturaGuia)
                Dim facturaActual As New Recibos.InfoFactura(CLng(hfIdFactura.Value))
                EncabezadoFacGuia.clear()
                CantidadPermitidaPorFacturaMensaje(facturaActual.IdFactura, infoFacturaGuia)
                mpeEditarGuia.Show()
            ElseIf e.CommandName = "eliminarGuia" Then

                Dim idGuia As Long = CLng(e.CommandArgument)
                Dim idFactura As Long = CLng(hfIdFactura.Value)
                Dim dtGuiasFacturas As New DataTable
                Dim filtroFacturaGuia As New Estructuras.FiltroFacturaGuia
                filtroFacturaGuia.IdGuia = idGuia
                dtGuiasFacturas = Recibos.FacturaGuia.ObtenerListado(filtroFacturaGuia)
                If dtGuiasFacturas.Rows.Count = 1 Then
                    Dim guia As New Recibos.InfoGuia(idGuia)
                    guia.IdUsuario = CLng(Session("usxp001"))
                    guia.Eliminar()
                End If

                Dim infoFacturaGuia As New Recibos.FacturaGuia()
                infoFacturaGuia.IdFactura = idFactura
                infoFacturaGuia.IdGuia = idGuia
                infoFacturaGuia.Eliminar()
                LimpiarCajasGuia()
                mpeEditarGuia.Hide()
                EncabezadoPagina.showSuccess("Guia eliminada correctamente.")
            ElseIf e.CommandName = "eliminarRelacionFacGuia" Then
                Dim idGuia As Long = CLng(e.CommandArgument)
                Dim idFactura As Long = CLng(hfIdFactura.Value)
                Dim infoFacturaGuia As New Recibos.FacturaGuia()
                infoFacturaGuia.IdFactura = idFactura
                infoFacturaGuia.IdGuia = idGuia
                infoFacturaGuia.Eliminar()
                LimpiarCajasGuia()
                mpeEditarGuia.Hide()
                EncabezadoPagina.showSuccess("Guia desvinculada correctamente.")
            End If

        Catch ex As Exception
            EncabezadoPagina.showError("Error al realizar la operación indicada. " & ex.Message)
        Finally
            CargarDetalleFactura()
        End Try
    End Sub

    Private Sub CargarEdicionGuia(ByVal infoFacturaGuia As Recibos.FacturaGuia)
        Try
            Dim guia As New Recibos.InfoGuia(CLng(hfIdGuiaEdicionActual.Value))

            txtCantidad.Text = infoFacturaGuia.Cantidad.ToString
            lblGuia.Text = guia.Guia
            ddlTransportadora.SelectedValue = guia.IdTransportador.ToString
            ddlTransportadora.Enabled = False
            Dim ciudad As New Localizacion.Ciudad(CInt(guia.IdCiudadOrigen))
            ddlPaisFacGuia.SelectedValue = ciudad.IdPais.ToString
            obtenerCiudadOrigen()
            ddlCiudadOrigen.SelectedValue = guia.IdCiudadOrigen.ToString
            txtFechaSalida.Text = guia.FechaSalida.ToString("dd/MM/yyyy")
            txtFechaEsperadaArribo.Text = guia.FechaEsperadaArribo.ToString("dd/MM/yyyy")
            txtPesoNeto.Text = guia.PesoNeto.ToString
            txtPesoBruto.Text = guia.PesoBruto.ToString
            If guia.IdEstado <> 16 Then
                ddlTransportadora.Enabled = False
                ddlPaisFacGuia.Enabled = False
                ddlCiudadOrigen.Enabled = False
                txtFechaSalida.Enabled = False
                txtFechaEsperadaArribo.Enabled = False
                txtPesoNeto.Enabled = False
                txtPesoBruto.Enabled = False
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al intentar cargar los datos de edición. " & ex.Message)
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


    Protected Sub ddlPaisFacGuia_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlPaisFacGuia.SelectedIndexChanged
        Try
            obtenerCiudadOrigen()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al filtrar los paises. " & ex.Message)
        End Try
    End Sub


    Private Sub LimpiarCajasGuia()
        ddlTransportadora.ClearSelection()
        ddlCiudadOrigen.ClearSelection()
        ddlPaisFacGuia.ClearSelection()
        txtFechaSalida.Text = String.Empty
        txtFechaEsperadaArribo.Text = String.Empty
        txtPesoNeto.Text = String.Empty
        txtPesoBruto.Text = String.Empty
    End Sub


    Protected Sub imgBtnCerrarPopUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnCerrarPopUp.Click
        Try
            LimpiarCajasGuia()
            CargarDetalleFactura()
            mpeEditarGuia.Hide()
        Catch ex As Exception
            EncabezadoPagina.showError("Error al tratar cerrar la ventana. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnEditarGuia_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditarGuia.Click
        Try
            Dim guia As New Recibos.InfoGuia(CLng(hfIdGuiaEdicionActual.Value))
            Dim infoFacturaGuia As New Recibos.FacturaGuia(CLng(hfIdFactura.Value), CLng(hfIdGuiaEdicionActual.Value))
            Dim cantidadMinimaPermitida As Integer = CantidadMinPermitida()
            Dim factura As New Recibos.InfoFactura(CLng(hfIdFactura.Value))
            'Dim cantidadesEnFactura As Integer = Recibos.FacturaGuia.CantidadPorFactura(CLng(hfIdFactura.Value))
            If CInt(txtCantidad.Text) > CantidadPermitidaPorFactura(factura.IdFactura, infoFacturaGuia) Then
                EncabezadoFacGuia.showWarning("Esta cantidad supera la cantidad de la factura. Cantidad maxima permitida " + CantidadPermitidaPorFactura(factura.IdFactura, infoFacturaGuia).ToString())
                mpeEditarGuia.Show()
            ElseIf CInt(txtCantidad.Text) < cantidadMinimaPermitida Then
                EncabezadoFacGuia.showWarning("Esta cantidad es menor a la cantidad recepcionada. Cantidad minima permitida " + cantidadMinimaPermitida.ToString())
                mpeEditarGuia.Show()
            Else
                With guia
                    .IdTransportador = CInt(ddlTransportadora.SelectedValue)
                    .IdCiudadOrigen = CInt(ddlCiudadOrigen.SelectedValue)
                    .FechaSalida = txtFechaSalida.Text
                    .FechaEsperadaArribo = txtFechaEsperadaArribo.Text
                    .PesoNeto = txtPesoNeto.Text
                    .PesoBruto = txtPesoBruto.Text
                    .Actualizar()
                    infoFacturaGuia.Cantidad = CLng(txtCantidad.Text)
                    infoFacturaGuia.Actualizar()
                    EncabezadoPagina.showSuccess("Guia actualiada.")
                    mpeEditarGuia.Hide()
                End With
            End If
        Catch ex As Exception
            EncabezadoPagina.showError("Error al editar la guia. " & ex.Message)
        Finally
            CargarDetalleFactura()
        End Try
    End Sub

    Private Function CantidadMinPermitida() As Integer
        Dim retorno As Integer
        Dim cantidad As Integer
        Dim dtOrdenesRecepcion As New DataTable
        Dim facturaGuiaObj As New Recibos.FacturaGuia(CLng(hfIdFactura.Value), CLng(hfIdGuiaEdicionActual.Value))
        Dim filtroOrdenRecepcion As New Estructuras.FiltroOrdenRecepcion
        filtroOrdenRecepcion.IdFacturaGuia = facturaGuiaObj.IdFacturaGuia
        dtOrdenesRecepcion = Recibos.OrdenRecepcion.ObtenerListado(filtroOrdenRecepcion)
        Dim dtResultado As New DataTable
        For Each fila As DataRow In dtOrdenesRecepcion.Rows
            dtResultado = Recibos.PalletRecepcion.ObtenerInfoDetalle(CLng(fila("idOrdenRecepcion")), _
                                                                        Recibos.TipoDetalleOrdenCompra.TipoDetalle.Principal)
            Integer.TryParse(dtResultado.Compute("SUM(cantidadRecibida)", "").ToString(), cantidad)
            retorno += cantidad
        Next

        Return retorno
    End Function

    Protected Sub gvGuiasAgregadas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGuiasAgregadas.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim ordenCompraObj As New Recibos.OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim btnEditarGuia As ImageButton = e.Row.FindControl("imgBtnEditarGuia")
            Dim btnEliminarGuia As ImageButton = e.Row.FindControl("imgBtnEliminarGuia")
            Dim idEstadoGuia As Integer
            idEstadoGuia = CInt(fila("idEstado"))
            btnEditarGuia.Visible = True
            If ordenCompraObj.IdEstado = Recibos.OrdenCompra.EstadoOrden.Cancelada Or ordenCompraObj.IdEstado = Recibos.OrdenCompra.EstadoOrden.Finalizada Then                
                btnEliminarGuia.Visible = False
            Else
                If idEstadoGuia = 16 Then
                    btnEliminarGuia.Visible = True
                Else
                    btnEliminarGuia.Visible = False
                End If
            End If
            
        End If
    End Sub

    Private Function CantidadPermitidaPorFactura(ByVal idFactura As Long, Optional ByVal facturaGuia As Recibos.FacturaGuia = Nothing) As Integer
        Dim factura As New Recibos.InfoFactura(idFactura)
        Dim cantidadesEnFactura As Integer = Recibos.FacturaGuia.CantidadPorFactura(idFactura)
        If facturaGuia Is Nothing Then
            Return (factura.Cantidad - cantidadesEnFactura)
        Else
            Return (factura.Cantidad - cantidadesEnFactura) + facturaGuia.Cantidad
        End If

    End Function

    Protected Sub CantidadPermitidaPorFacturaMensaje(ByVal idFactura As Long, ByVal facturaGuia As Recibos.FacturaGuia)
        lblCantidadPermitidaPorFactura.Text = "Cantidad maxima permitida " & CantidadPermitidaPorFactura(idFactura, facturaGuia).ToString
    End Sub

End Class