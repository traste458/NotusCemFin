Imports ILSBusinessLayer

Partial Public Class CrearGuia
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            If Not IsPostBack Then
                EncabezadoPagina.setTitle("Crear Guia de Orden de Compra")
                obtenerOrdenCompra()
                obtenerTransportadora()
                obtenerCiudadOrigen()
                ObtenerEstado()
            End If
        Catch ex As Exception
            EncabezadoPagina.showError(ex.Message)
        End Try
    End Sub

    Protected Sub obtenerOrdenCompra()
        Dim filtro As Estructuras.FiltroOrdenCompra        
        Try
            filtro.IdEstado = 16
            With ddlOrdenCompra
                .DataSource = Recibos.OrdenCompra.ObtenerListado(filtro)
                .DataTextField = "numeroOrden"
                .DataValueField = "idOrden"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja la Orden de Compra", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar el tipo de producto " & ex.Message)
        End Try
    End Sub

    Protected Sub obtenerTransportadora()
        Dim filtroTransportadoras As Estructuras.FiltroTransportadora

        filtroTransportadoras.Activo = True

        Try
            With ddlTransportadora
                .DataSource = Transportadora.ListadoTransportadoras(filtroTransportadoras)
                .DataTextField = "transportadora"
                .DataValueField = "idTransportadora"
                .DataBind()
                If .Items.Count > 1 Then .Items.Insert(0, New ListItem("Escoja la Transportadora", ""))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar la transportadora" & ex.Message)
        End Try
    End Sub

    Protected Sub obtenerCiudadOrigen()
        Dim filro As Estructuras.FiltroCiudad
        filro.Activo = 1        
        Try
            With ddlCiudadOrigen
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
        Dim guia As New Recibos.InfoGuia
        With guia
            .IdOrdenCompra = CLng(ddlOrdenCompra.SelectedValue)
            .Guia = txtNoGuia.Text
            .IdTransportador = CInt(ddlTransportadora.SelectedValue)
            .IdCiudadOrigen = CInt(ddlCiudadOrigen.SelectedValue)
            .FechaSalida = Date.Parse(txtFechaSalida.Text) 'CType(txtFechaSalida.Text, Date) 'CDate(txtFechaSalida.Text).ToString("dd-MM-YYYY")
            .FechaEsperadaArribo = CDate(txtFechaEsperadaArribo.Text)
            .IdEstado = CInt(ddlEstado.SelectedValue)
            .PesoNeto = CLng(txtPesoNeto.Text)
            .PesoBruto = CLng(txtPesoBruto.Text)
            .IdUsuario = CLng(Session("Idusuario"))
            .Crear()
        End With
    End Sub
End Class