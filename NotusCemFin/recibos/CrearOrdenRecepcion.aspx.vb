Imports ILSBusinessLayer
Imports ILSBusinessLayer.Recibos

Partial Public Class CrearOrdenRecepcion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            EncabezadoPagina.clear()
            If Not IsPostBack Then
                EncabezadoPagina.setTitle("Crear Orden de Recepción")

                EncabezadoPagina.showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                ObtenerTipoProducto()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerTipoProducto()
        Dim filtroTipoProducto As Estructuras.FiltroTipoProducto
        filtroTipoProducto.tipoAplicativo = 1
        filtroTipoProducto.Activo = 1
        filtroTipoProducto.ExisteModulo = 1
        filtroTipoProducto.IdModulo = 2
        Try
            With ddlTipoProducto
                .DataSource = ILSBusinessLayer.Productos.TipoProducto.ObtenerListado(filtroTipoProducto)
                .DataTextField = "descripcion"
                .DataValueField = "idTipoProducto"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Tipo de Producto", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar el tipo de producto " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlTipoProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlTipoProducto.SelectedIndexChanged

        Dim ar As New ArrayList
        ar = ModuloTipoProducto.Obtener(2, ddlTipoProducto.SelectedValue)
        If ar.Count > 0 AndAlso CInt(ddlTipoProducto.SelectedValue) <> 0 Then
            'frModulo.Attributes("src") = ar(2).ToString & "?tp=" & ddlTipoProducto.SelectedValue
            'frModulo.Attributes("style") = "display:block"
            EncabezadoPagina.clear()
        Else
            'frModulo.Attributes("src") = ""
            'frModulo.Attributes("style") = "display:none"
            EncabezadoPagina.showWarning("Opcion no permitida")
        End If

    End Sub

End Class