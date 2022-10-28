Imports System.Data.SqlClient

Partial Class reporteFacturasPendientesRecibir
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me)
            lblError.Text = ""
            If Not Me.IsPostBack Then
                getDatos()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
        End Try
    End Sub
    
    Private Sub getDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand, sqlAdaptador As SqlDataAdapter
        Dim dtDatos As New DataTable, sqlSelect As String, htFiltros As Hashtable

        Try
            htFiltros = getFiltrosAplicados()

            sqlSelect = "select idfactura2 as factura,(select proveedor from proveedores with(nolock) where idproveedor="
            sqlSelect += " fe.idProveedor) as proveedor,(select producto from productos with(nolock) where idproducto="
            sqlSelect += " fe.idproducto) as producto,(select tipoProducto from TipoProducto with(nolock) where idTipoProducto="
            sqlSelect += " fe.idTipoProducto) as tipoProducto,(select tipoRecepcion from TipoRecepcion with(nolock) where"
            sqlSelect += " idTipoRecepcion=fe.idTipoRecepcion) as tipoRecepcion,cantidadEsperada,fecha from facturas_externas"
            sqlSelect += " fe with(nolock) where estado=0"
            If htFiltros("factura").ToString <> "" Then sqlSelect += " and idfactura2 like '%'+@factura+'%' "
            If htFiltros("idProveedor").ToString <> "0" Then sqlSelect += " and ia.idProveedor=@idProveedor "
            If htFiltros("idProducto").ToString <> "0" Then sqlSelect += " and fe.idProducto=@idProducto "
            If htFiltros("idTipoProducto").ToString <> "0" Then sqlSelect += " and ia.idTipoProducto=@idTipoProducto "
            If htFiltros("fechaEsperada").ToString <> "" Then
                sqlSelect += " and convert(varchar,fecha,112) between @fechaInicial and @fechaFinal "
            End If
            sqlSelect += " order by proveedor,producto,factura"

            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            With sqlComando.Parameters
                If htFiltros("factura").ToString <> "" Then .Add("@factura", SqlDbType.VarChar, 52).Value = htFiltros("factura").ToString
                If htFiltros("idProveedor").ToString <> "0" Then .Add("@idProveedor", SqlDbType.Int).Value = htFiltros("idProveedor").ToString
                If htFiltros("idProducto").ToString <> "0" Then .Add("@idProducto", SqlDbType.Int).Value = htFiltros("idProducto").ToString
                If htFiltros("idTipoProducto").ToString <> "0" Then .Add("@idTipoProducto", SqlDbType.Int).Value = htFiltros("idTipoPoducto").ToString
                If htFiltros("fechaEsperada").ToString <> "" Then
                    .Add("@fechaInicial", SqlDbType.VarChar, 10).Value = String.Format("{0:yyyyMMdd}", CDate(htFiltros("fechaEsperada")).AddDays(-2))
                    .Add("@fechaFinal", SqlDbType.VarChar, 10).Value = String.Format("{0:yyyyMMdd}", CDate(htFiltros("fechaEsperada")).AddDays(2))
                End If
            End With
            sqlAdaptador.Fill(dtDatos)
            If dtDatos.Rows.Count > 0 Then
                With dgDatos
                    .DataSource = dtDatos
                    .Columns(5).FooterText = dtDatos.Compute("sum(cantidadEsperada)", "")
                    .DataBind()
                End With
                Session("dtFactPendientesRecibir") = dtDatos
                lbExportar.Visible = True
            Else
                lblError.Text = "<i>No se encontraron datos con las características solicitadas</i>.<br><br>"
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener Datos. " & ex.Message)
        End Try
    End Sub

    Private Sub dgDatos_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs)
        Try
            dgDatos.CurrentPageIndex = e.NewPageIndex
            Dim dtDatos As New DataTable
            dtDatos = CType(Session("dtFactPendientesRecibir"), DataTable)
            With dgDatos
                .DataSource = dtDatos
                .Columns(5).FooterText = dtDatos.Compute("sum(cantidadEsperada)", "")
                .DataBind()
            End With
        Catch ex As Exception
            lblError.Text = "Imposible camiar de Página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Function getFiltrosAplicados() As Hashtable
        Dim htFiltros As Hashtable
        Try
            If Not Session("htFiltrosBuscarFacPendientes") Is Nothing Then
                htFiltros = CType(Session("htFiltrosBuscarFacPendientes"), Hashtable)
            Else
                htFiltros = New Hashtable
                With htFiltros
                    .Add("factura", "")
                    .Add("idTipoProducto", 0)
                    .Add("idProveedor", 0)
                    .Add("idProducto", 0)
                    .Add("fechaEsperada", "")
                End With
            End If
            Return htFiltros
        Catch ex As Exception
            Throw New Exception("Error al tratar de recuperar filtros aplicados. " & ex.Message)
        End Try
    End Function

    Private Sub exportarReporteAExcel()
        Try
            Dim sw As New System.IO.StringWriter, htw As New System.Web.UI.HtmlTextWriter(sw)
            Dim dgAux As DataGrid = MetodosComunes.clonarDataGrid(dgDatos)
            Dim dtDatos As DataTable = CType(Session("dtFactPendientesRecibir"), DataTable)
            AddHandler dgAux.ItemDataBound, AddressOf dgAux_ItemDataBound
            With dgAux
                .AllowPaging = False
                .AlternatingItemStyle.BackColor = Nothing
                .DataSource = dtDatos
                .Columns(5).FooterText = dtDatos.Compute("sum(cantidadEsperada)", "")
                .DataBind()
                .RenderControl(htw)
            End With
            MetodosComunes.exportarDatosAExcel(HttpContext.Current, sw.ToString, "Reporte de Facturas Pendientes Por Recibir", "ReporteFacturaPendientesRecibir.xls")
        Catch ex As Exception
            lblError.Text = "Error al tratar de exportar reporte a Excel. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub lbExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbExportar.Click
        exportarReporteAExcel()
    End Sub

    Private Sub dgAux_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Cells(0).CssClass = "text"
        End If
    End Sub
End Class





