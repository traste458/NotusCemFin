Imports ILSBusinessLayer
Imports ILSBusinessLayer.Recibos
Imports LMDataAccessLayer

Partial Public Class VerDetalleRecepcion
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            EncabezadoPagina.clear()


            If Not IsPostBack Then
                hfOrdenRecepcion.Value = CInt(Request.QueryString("orep"))

                EncabezadoPagina.setTitle("Pallet de Recepción Creados")

                EncabezadoPagina.showReturnLink("~/recibos/BuscarOrdenRecepcion.aspx")        
                If CInt(hfOrdenRecepcion.Value) > 0 Then
                    CargarDatosFacturaGuia()
                    CargarPalletsActuales()
                End If

            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Private Sub CargarPalletsActuales()
        Try
            gvDetallePallet.DataSource = PalletRecepcion.ObtenerInfoDetalle(CLng(hfOrdenRecepcion.Value))
            gvDetallePallet.DataBind()
            If gvDetallePallet.Rows.Count > 0 Then
                lblTotalPallet.Text = "Total Pallets " & gvDetallePallet.Rows.Count.ToString
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los pallets actuales a esta orden " & ex.Message)
        End Try
    End Sub


    Private Sub CargarDatosFacturaGuia()
        Try
            Dim infoRecepcion As New Recibos.OrdenRecepcion(CLng(hfOrdenRecepcion.Value))
            hfFacturaGuia.Value = infoRecepcion.IdFacturaGuia.ToString
            'Dim infoPalletRecepcion As New Recibos.PalletRecepcion(infoRecepcion.
            Dim infoFacturaGuia As New Recibos.FacturaGuia(CLng(hfFacturaGuia.Value))
            Dim Factura As New Recibos.InfoFactura(CLng(infoFacturaGuia.IdFactura))
            Dim Guia As New Recibos.InfoGuia(CLng(infoFacturaGuia.IdGuia))
            Dim detalleOrdenCompra As New Recibos.DetalleOrdenCompra(CLng(Factura.IdDetalleOrdenCompra))
            Dim infoOrdenCompra As New Recibos.OrdenCompra(CLng(detalleOrdenCompra.IdOrden))


            With infoRecepcion
                lblNumeroRecepcion.Text = .IdOrdenRecepcion
                lblFechaRecepcion.Text = .FechaRecepcion.ToString
                lblTipoRecepcion.Text = .TipoRecepcion
                lblRemision.Text = .Remision
                lblConsignado.Text = .Consignatario.Nombre
                lblDestinatario.Text = .ClienteExterno.Nombre
            End With
            With infoOrdenCompra
                lblNumeroOrdenCompra.Text = .NumeroOrden.ToString
                lblTipoProducto.Text = .TipoProducto.Descripcion
            End With

            With Factura
                lblFactura.Text = .Factura
            End With
            lblCantidad.Text = infoFacturaGuia.Cantidad
            With Guia
                lblGuia.Text = .Guia
            End With
        Catch ex As Exception
            EncabezadoPagina.showError("Error al cargar los datos de factura, guia. " & ex.Message)
        End Try
    End Sub
    

    Protected Sub gvDetallePallet_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDetallePallet.RowCommand        
        If e.CommandName = "imprimirViajera" Then
            imprimirViajera(CInt(e.CommandArgument))
        ElseIf e.CommandName = "verNovedades" Then
            MostrarNovedadesPallet(CInt(e.CommandArgument))
        End If
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

    Private Sub imprimirViajera(ByVal idDetallePallet As Integer)
        Try
            Dim rpt As New ReporteCrystal("resumenPalletRecepcion", Server.MapPath("../Reports"))
            rpt.agregarParametroDiscreto("@idPallet", idDetallePallet)
            Dim ruta As String = rpt.exportar(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat)
            ruta = ruta.Substring(ruta.LastIndexOf("\") + 1)
            'ScriptManager.RegisterClientScriptBlock(Me, Me.GetType, "newWindow", "window.open('../Reports/rptTemp/" + ruta + "','Impresion_Viajera', 'status=1, toolbar=0, location=0,menubar=1,directories=0,resizable=1,scrollbars=1'); ", True)
            Dim RutaRelativa As String = "../Reports/rptTemp/" & ruta & ""
            Dim js As String = "<script language={0}javascript{0} type={0}text/javascript{0}>window.open({0}" & RutaRelativa & "{0},{0}Descargar{0},{0}width=500,height=500,scrollbars=NO{0});</script>"


            js = String.Format(js, Chr(34))
            Me.RegisterStartupScript("Descarga", js)

        Catch ex As Exception
            EncabezadoPagina.showError("Error al generar el documento. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvDetallePallet_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetallePallet.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim idPallet = CInt(fila("idPallet"))
            Dim dtResultado As New DataTable
            Dim filtro As New Estructuras.FiltroPalletNovedad
            Dim btnVerNovedad As ImageButton = e.Row.FindControl("imgBtnVerNovedades")
            filtro.IdPallet = idPallet
            dtResultado = Recibos.PalletNovedad.ObtenerListado(filtro)
            If dtResultado.Rows.Count > 0 Then
                btnVerNovedad.Visible = True
            Else
                btnVerNovedad.Visible = False
            End If
        End If
    End Sub

    Protected Sub imgBtnCerrarPopUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBtnCerrarPopUp.Click
        mpeMostrarNovedades.Hide()
    End Sub
End Class