Partial Public Class CrearOrdenCompraDetallePopInsumo
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epEncabezado.clear()
        If Not Me.IsPostBack Then
            hfIdTipoProducto.Value = Request.QueryString("tp").ToString
        End If
    End Sub

    Protected Sub btnEnviar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEnviar.Click
        Try
            Dim opcion As Integer
            Dim redireccion As String = Request.Url.AbsoluteUri.Substring(0, (Request.Url.AbsoluteUri.LastIndexOf("/")))
            opcion = CInt(ddlTipoOrden.SelectedValue)
            If opcion = 1 Then
                redireccion += "CrearOrdenCompraGeneral.aspx"
            ElseIf opcion = 2 Then                
                redireccion += "CrearOrdenCompraMerchanPopInsumoP1.aspx?tp=" & hfIdTipoProducto.Value.ToString            
            End If
            ClientScript.RegisterClientScriptBlock(Me.GetType, "Redireccionar", "window.parent.location='" & redireccion & "';", True)
        Catch ex As Exception
            epEncabezado.showError("Error al enviar. " & ex.Message)
        End Try
    End Sub

    Private Sub CrearOrdenCompraMerchanPopInsumo_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        ClientScript.RegisterClientScriptBlock(Me.GetType, "codModificarFrameAlto", "modificarAltoFramePadre();", True)
    End Sub
End Class