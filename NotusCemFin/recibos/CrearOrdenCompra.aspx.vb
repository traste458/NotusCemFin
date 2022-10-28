Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer.Estructuras

Partial Public Class CrearOrdenCompra
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            epNotificador.clear()
            If Not IsPostBack Then
                epNotificador.setTitle("Crear Orden de Compra")
                epNotificador.showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                ObtenerTipoProducto()
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar la página. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerTipoProducto()
        Dim filtroTipoProducto As New FiltroTipoProducto
        filtroTipoProducto.Activo = 1
        filtroTipoProducto.ExisteModulo = 1
        filtroTipoProducto.IdModulo = 1
        Try
            With ddlTipoProducto
                .DataSource = ILSBusinessLayer.Productos.TipoProducto.ObtenerListado(filtroTipoProducto)
                .DataTextField = "descripcion"
                .DataValueField = "idTipoProducto"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Tipo de Producto", 0))
            End With
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el tipo de producto " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlTipoProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoProducto.SelectedIndexChanged
        Try
            Dim ar As New ArrayList
            ar = ModuloTipoProducto.Obtener(1, ddlTipoProducto.SelectedValue)
            If ar.Count > 0 AndAlso ddlTipoProducto.SelectedValue <> 0 Then
                'frModulo.Attributes("src") = ar(2).ToString & "?tp=" & ddlTipoProducto.SelectedValue
                'frModulo.Attributes("style") = "display:block"
            Else
                'frModulo.Attributes("src") = ""
                'frModulo.Attributes("style") = "display:none"
                'epNotificador.showWarning("Opcion no permitida")
            End If
        Catch ex As Exception
            epNotificador.showError("Error al tratar de cargar el formulario asociado al Tipo de Producto seleccionado")
        End Try
    End Sub

    Private Function ExistenDatos() As Boolean
        Try
            Dim retorno As Boolean = False
            'Dim numeroOrden As String = frModulo.InnerHtml
            Return retorno
        Catch ex As Exception
            epNotificador.showError("Error al consultar la existencia de datos en el frame. " & ex.Message)
        End Try
    End Function
End Class