Imports DevExpress.Web
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Estructuras

Public Class PoolTrazabilidadGuias
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

#If DEBUG Then
        Session("usxp001") = 64045

#End If
        Seguridad.verificarSession(Me)

        Try
            If Not Me.IsPostBack Then
                Session.Remove("dtTrazabilidadGuias")

                With epPrincipal
                    .showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                    .setTitle("Pool Trazabilidad Guias")
                End With

                CargarDatos()
            End If

        Catch ex As Exception
            epPrincipal.showError("Error al cargar los productos de pedido: " & ex.Message)
        End Try
    End Sub

    Private Sub CargarDatos()
        Try

        Catch ex As Exception
            epPrincipal.showError("No fué posible establecer cargar datos de despacho en proceso: " & ex.Message)
        End Try
    End Sub

    Protected Sub cpPrincipal_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpPrincipal.Callback
        Dim resultado As ILSBusinessLayer.ResultadoProceso
        epPrincipal.clear()

        Try
            Dim arrayParameters As String()
            arrayParameters = Split(e.Parameter.ToString, ":")
            Select Case arrayParameters(0)
                Case "200"
                    Dim dtTrazabilidadGuias As New DataTable

                    TrazabilidadGuias.numeroRadicado = txtRadicado.Value
                    TrazabilidadGuias.pedido = txtPedido.Value
                    TrazabilidadGuias.guia = txtGuia.Value

                    dtTrazabilidadGuias = TrazabilidadGuias.ObtenerInformacionTrazabilidadGuias()
                    Session("dtTrazabilidadGuias") = dtTrazabilidadGuias
                    With gvTrazabilidadGuia
                        .DataSource = dtTrazabilidadGuias
                        .DataBind()
                    End With
            End Select
        Catch ex As Exception
            epPrincipal.showError("Error al tratar de cosultar la guia: " & ex.Message)
        End Try
    End Sub

    Protected Sub gvTrazabilidadGuia_DataBinding(sender As Object, e As EventArgs) Handles gvTrazabilidadGuia.DataBinding
        gvTrazabilidadGuia.DataSource = Session("dtTrazabilidadGuias")
    End Sub

    Protected Sub gridDetail_BeforePerformDataSelect(sender As Object, e As EventArgs)
        Try
            Session("guia") = (TryCast(sender, ASPxGridView)).GetMasterRowKeyValue()
            CargarDetallePedido(TryCast(sender, ASPxGridView))
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos de órdenes de recepción " & ex.Message)
        End Try
    End Sub
    Private Sub CargarDetallePedido(gv As ASPxGridView)

        If Session("guia") IsNot Nothing Then

            Dim guia As String = CDec(Session("guia"))
            Dim dtDetalle As New DataTable
            dtDetalle = ObtenerDetalleMovimientosGuia(guia)
            Session("dtDetalle") = dtDetalle
            With gv
                .DataSource = Session("dtDetalle")
            End With
        Else
            Throw New Exception("No se pudo establecer el identificador del despacho, por favor intente nuevamente.")
        End If
    End Sub

    Private Function ObtenerDetalleMovimientosGuia(ByVal guia As Decimal) As DataTable
        Dim dtResultado As New DataTable
        Try

            TrazabilidadGuias.guia = guia
            dtResultado = TrazabilidadGuias.ObtenerMovimientosGuias()
        Catch ex As Exception
            Throw New Exception("Se presento un error al cargar el detalle del despacho:." & ex.Message)
        End Try
        Return dtResultado
    End Function

    Protected Sub lbExportar_Click(sender As Object, e As EventArgs) Handles lbExportar.Click
        Exportar()
    End Sub
    Private Sub Exportar()
        Dim cantidadRegistros As Integer = 0
        Dim aux As Integer = 0
        Try
            Dim dtDatos As New DataTable

            If Session("dtTrazabilidadGuias") IsNot Nothing AndAlso CType(Session("dtTrazabilidadGuias"), DataTable).Rows.Count > 0 Then
                Dim fecha As DateTime = DateTime.Now
                Dim fec As String = fecha.ToString("HH:mm:ss:fff").Replace(":", "_")
                Dim nombre As String = "PoolTrazabilidadGuias"
                nombre = nombre & "_" & Session("usxp001") & "_" & fec & ".xls"
                Dim filtros As New FiltroDespachoSinPedidoSatelite

                If txtPedido.Text <> "" Then
                    filtros.NumeroPedido = txtPedido.Text.Trim()
                End If

                filtros.IdUsuario = Session("usxp001")

                dtDatos = BodegaSatelite.ObtenerDespachosEnProcesoReporte(filtros)
                Session("dtTrazabilidadGuias") = dtDatos

                Dim arrayNombre As New ArrayList
                With arrayNombre
                    .Add("tipo")
                    .Add("numeroRadicado")
                    .Add("guia")
                    .Add("transportadora")
                End With
                MetodosComunes.exportarDatosAExcelGemBox(HttpContext.Current, dtDatos, "PoolTrazabilidadGuias", nombre, Server.MapPath("../archivos_planos/PoolTrazabilidadGuias.xls"), arrayNombre, True)
                epPrincipal.showSuccess("informe Generado Correctamente.")
            Else
                epPrincipal.showWarning("No se encontraron datos para exportar, por favor intente nuevamente.")
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class