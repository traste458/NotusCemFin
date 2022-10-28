Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer

Partial Public Class VerDetalleOrdenCompra
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)

            EncabezadoPagina.clear()
            If Not IsPostBack Then
                hfIdOrdenCompra.Value = Request.QueryString("ido").ToString
                Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
                Dim tipoProducto As Integer = CInt(ordenCompra.IdTipoProducto)
                EncabezadoPagina.setTitle("Detalle de la Orden de Compra")
                If Request.UrlReferrer IsNot Nothing Then
                    EncabezadoPagina.showReturnLink(Request.UrlReferrer.ToString)
                Else
                    EncabezadoPagina.showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                End If

                'Carga inicial para combos de detalle de la orden
                CargarInfoOrden()
                CargarDetallesOrdenCompra()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Private Sub CargarDetallesOrdenCompra()
        Try
            Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            Dim filtro As Estructuras.FiltroDetalleOrdenCompra
            Dim dt As New DataTable
            filtro.IdOrden = CInt(ordenCompra.IdOrden)
            dt = Recibos.DetalleOrdenCompra.ObtenerListado(filtro)
            gvDetalleOrdenCompra.DataSource = dt
            gvDetalleOrdenCompra.DataBind()
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar los detalle agregados a la orden de compra. " & ex.Message)
        End Try
    End Sub

    Private Sub CargarInfoOrden()
        Try
            Dim ordenCompra As New OrdenCompra(CLng(hfIdOrdenCompra.Value))
            With ordenCompra
                lblNumeroOrden.Text = .NumeroOrden
                lblProveedor.Text = .Proveedor
                lblMoneda.Text = .Moneda
                lblIncoterm.Text = .Incoterm
                lblObservacion.Text = .Observacion
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar la informacion de la orden. " & ex.Message)
        End Try
    End Sub



End Class