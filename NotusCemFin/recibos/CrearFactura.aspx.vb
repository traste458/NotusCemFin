Imports ILSBusinessLayer

Partial Public Class CrearFactura
    Inherits System.Web.UI.Page
    Public idDetalleOrdenCompra As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            idDetalleOrdenCompra = CInt(Request.QueryString("idoc"))
            If Not IsPostBack Then
                EncabezadoPagina.setTitle("Crear Factura de Orden de Compra")
                'obtenerTransportadora()
                obtenerCiudadOrigen()
                ObtenerEstado()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Protected Sub obtenerCiudadOrigen()
        Dim filro As Estructuras.FiltroCiudad
        filro.Activo = 1
        Try
            With ddlCiudadCompra
                .DataSource = Localizacion.Ciudad.ObtenerListado(filro)
                .DataTextField = "nombre"
                .DataValueField = "idCiudad"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja la Ciudad", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar las Ciudades" & ex.Message)
        End Try
    End Sub


    Protected Sub ObtenerEstado()
        Dim dt As New DataTable
        Try
            dt = Estado.Obtener(5)
            With ddlEstado
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idEstado"
                .DataBind()
                If dt.Rows.Count > 1 Then
                    .Items.Insert(0, New ListItem("Escoja el Estado", ""))
                End If
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos para Moneda. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnCrear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrear.Click
        Dim factura As New Recibos.InfoFactura
        With factura
            .IdDetalleOrdenCompra = CLng(idDetalleOrdenCompra)
            .Factura = txtFactura.Text
            .Cantidad = CInt(txtCantidad.Text)
            .IdCiudadCompra = CInt(ddlCiudadCompra.SelectedValue)
            .IdEstado = CLng(ddlEstado.SelectedValue)
            .Crear()
        End With
    End Sub
End Class