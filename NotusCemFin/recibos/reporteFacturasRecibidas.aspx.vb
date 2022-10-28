Imports System.Data.SqlClient

Partial Class reporteFacturasRecibidas
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

    Dim filtrosAplicados As New filtroBusquedaFacturasOP

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me, Anthem.Manager.IsCallBack)
            filtrosAplicados = CType(Session("filtrosReporteBuscarFacturasRecibidas"), filtroBusquedaFacturasOP)
            If Not Me.IsPostBack And Not Anthem.Manager.IsCallBack Then
                getDatos()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtDatos As New DataTable
        Dim sqlSelect, sqlQuery As String

        sqlSelect = "select idfactura2 as factura,ordenCompra,guia_aerea as guia,(select proveedor from proveedores"
        sqlSelect += " with(nolock) where idproveedor=fe.idProveedor)as proveedor,(select producto from productos with(nolock)"
        sqlSelect += " where idproducto=fe.idproducto) as producto,(select tipoProducto from TipoProducto with(nolock) where"
        sqlSelect += " idTipoProducto=fe.idTipoProducto)as tipoProducto,(select tipoRecepcion from TipoRecepcion with(nolock)"
        sqlSelect += " where idTipoRecepcion=fe.idTipoRecepcion)as tipoRecepcion,(select estadoRecepcion from EstadoRecepcionFactura"
        sqlSelect += " with(nolock) where idEstadoRecepcion=fe.idEstadoRecepcion) as estadoRecepcion,numeroPalets,cantidadAprox,"
        sqlSelect += " cantidadEsperada,(select bodega from bodegas with(nolock) where idbodega=fe.idBodega)as bodega,fecha,"
        sqlSelect += " (select estado from EstadoFactura with(nolock) where idEstado=fe.estado) as estado,observacion from "
        sqlSelect += " facturas_externas fe with(nolock) where estado>0 and fe.idClasificacion in (1)"

        sqlQuery = "select factura,ordenCompra,guia,(select proveedor from proveedores with(nolock) where idproveedor="
        sqlQuery += " f.idProveedor)as proveedor,(select producto from productos with(nolock) where idproducto=f.idProducto)"
        sqlQuery += " as producto,(select tipoProducto from TipoProducto with(nolock) where idTipoProducto=f.idTipoProducto)"
        sqlQuery += " as tipoProducto,(select tipoRecepcion from TipoRecepcion with(nolock) where idTipoRecepcion=f.idTipoRecepcion)"
        sqlQuery += " as tipoRecepcion,(select estadoRecepcion from EstadoRecepcionFactura with(nolock) where idEstadoRecepcion="
        sqlQuery += " f.idEstadoRecepcion) as estadoRecepcion,numeroPalets,cantidadAprox,cantidadEsperada,(select bodega from bodegas"
        sqlQuery += " with(nolock) where idbodega=f.idBodega)as bodega,fechaLlegada as fecha, (select estado from EstadoFactura"
        sqlQuery += " with(nolock) where idEstado=f.estado) as estado,observacion from Factura f with(nolock) where estado>0 "
        With filtrosAplicados
            If .factura <> "" Then
                sqlSelect += " and idfactura2 like '%'+@factura+'%' "
                sqlQuery += " and factura like '%'+@factura+'%' "
            End If
            If .ordenCompra <> "" Then
                sqlSelect += " and ordenCompra like '%'+@ordenCompra+'%' "
                sqlQuery += " and ordenCompra like '%'+@ordenCompra+'%' "
            End If
            If .guia <> "" Then
                sqlSelect += " and guia_aerea like '%'+@guia+'%' "
                sqlQuery += " and guia like '%'+@guia+'%' "
            End If
            If .idTipoProducto <> 0 Then
                sqlSelect += " and fe.idTipoProducto=@idTipoProducto "
                sqlQuery += " and f.idTipoProducto=@idTipoProducto "
            End If
            If .idProveedor <> 0 Then
                sqlSelect += " and idProveedor=@idProveedor "
                sqlQuery += " and idProveedor=@idProveedor "
            End If
            If .idProducto <> 0 Then
                sqlSelect += " and idproducto=@idProducto "
                sqlQuery += " and idproducto=@idProducto "
            End If
            If .idTipoRecepcion <> 0 Then
                sqlSelect += " and idTipoRecepcion=@idTipoRecepcion "
                sqlQuery += " and idTipoRecepcion=@idTipoRecepcion "
            End If
            If .idEstadoRecepcion <> 0 Then
                sqlSelect += " and idEstadoRecepcion=@idEstadoRecepcion "
                sqlQuery += " and idEstadoRecepcion=@idEstadoRecepcion "
            End If
            If .idEstadoFactura <> -2 Then
                sqlSelect += " and estado=@estado "
                sqlQuery += " and estado=@estado "
            End If
            If .fechaInicial <> "" Then
                sqlSelect += " and convert(varchar,fecha,112) between @fechaInicial and @fechaFinal "
                sqlQuery += " and convert(varchar,fechaLlegada,112) between @fechaInicial and @fechaFinal "
            End If
        End With

        Try
            With filtrosAplicados
                If .idTipoProducto <> 0 Then
                    If esRegionalizado() = False Then
                        sqlSelect = sqlQuery
                    End If
                Else
                    sqlSelect += " union " & sqlQuery
                End If
                sqlSelect += " order by proveedor,producto,tipoRecepcion,fecha,guia,ordenCompra,factura,estado"
                MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
                sqlComando.Parameters.Add("@factura", SqlDbType.VarChar).Value = .factura
                sqlComando.Parameters.Add("@ordenCompra", SqlDbType.VarChar).Value = .ordenCompra
                sqlComando.Parameters.Add("@guia", SqlDbType.VarChar).Value = .guia
                sqlComando.Parameters.Add("@idTipoProducto", SqlDbType.Int).Value = .idTipoProducto
                sqlComando.Parameters.Add("@idProveedor", SqlDbType.Int).Value = .idProveedor
                sqlComando.Parameters.Add("@idProducto", SqlDbType.Int).Value = .idProducto
                sqlComando.Parameters.Add("@idTipoRecepcion", SqlDbType.Int).Value = .idTipoRecepcion
                sqlComando.Parameters.Add("@idEstadoRecepcion", SqlDbType.Int).Value = .idEstadoRecepcion
                sqlComando.Parameters.Add("@estado", SqlDbType.Int).Value = .idEstadoFactura
                If .fechaInicial <> "" Then
                    sqlComando.Parameters.Add("@fechaInicial", SqlDbType.VarChar).Value = String.Format("{0:yyyyMMdd}", CDate(.fechaInicial))
                    sqlComando.Parameters.Add("@fechaFinal", SqlDbType.VarChar).Value = String.Format("{0:yyyyMMdd}", CDate(.fechaFinal))
                End If
            End With
            sqlAdaptador.Fill(dtDatos)
            If dtDatos.Rows.Count > 0 Then
                With dgDatos
                    .DataSource = dtDatos
                    .Columns(7).FooterText = dtDatos.Compute("sum(numeroPalets)", "")
                    .Columns(8).FooterText = dtDatos.Compute("sum(cantidadAprox)", "")
                    .Columns(9).FooterText = dtDatos.Compute("sum(cantidadEsperada)", "")
                    .DataBind()
                End With
                Session("dtFacturasRecibidas") = dtDatos
                lbExportar.Visible = True
            Else
                lblError.Text = "No se encontraron datos con las características solicitadas.<br><br>"
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener datos. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Function esRegionalizado() As Boolean
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlSelect As String, resultado As Boolean

        sqlSelect = "select regionalizado from TipoProducto with(nolock) where idTipoProducto=@idTipoProducto"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idTipoProducto", SqlDbType.Int).Value = filtrosAplicados.idTipoProducto
            sqlConexion.Open()
            resultado = CBool(sqlComando.ExecuteScalar)
            Return resultado
        Catch ex As Exception
            Throw New Exception("Error al tratar de validar el Tipo de Producto. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Function

    Private Sub dgDatos_PageIndexChanged(ByVal source As System.Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDatos.PageIndexChanged
        If Session.Count > 0 Then
            Try
                dgDatos.CurrentPageIndex = e.NewPageIndex
                Dim dtDatos As DataTable
                dtDatos = CType(Session("dtFacturasRecibidas"), DataTable)
                With dgDatos
                    .DataSource = dtDatos
                    .Columns(7).FooterText = dtDatos.Compute("sum(numeroPalets)", "")
                    .Columns(8).FooterText = dtDatos.Compute("sum(cantidadAprox)", "")
                    .Columns(9).FooterText = dtDatos.Compute("sum(cantidadEsperada)", "")
                    .DataBind()
                End With
            Catch ex As Exception
                lblError.Text = "Imposible Paginar Tabla. " & ex.Message & "<br><br>"
            End Try
        End If
    End Sub

    Private Sub exportarReporteAExcel()
        Try
            Dim dgAux As DataGrid, dtDatos As DataTable
            Dim sw As New System.IO.StringWriter, htw As New System.Web.UI.HtmlTextWriter(sw)
            dgAux = MetodosComunes.clonarDataGrid(dgDatos)
            dtDatos = CType(Session("dtFacturasRecibidas"), DataTable)
            AddHandler dgAux.ItemDataBound, AddressOf dgAux_ItemDataBound
            With dgAux
                .AllowPaging = False
                .AlternatingItemStyle.BackColor = Nothing
                .DataSource = dtDatos
                .Columns(7).FooterText = dtDatos.Compute("sum(numeroPalets)", "")
                .Columns(8).FooterText = dtDatos.Compute("sum(cantidadAprox)", "")
                .Columns(9).FooterText = dtDatos.Compute("sum(cantidadEsperada)", "")
                .DataBind()
                .RenderControl(htw)
            End With
            MetodosComunes.exportarDatosAExcel(HttpContext.Current, sw.ToString, "Reporte de Facturas Recibidas", "ReporteDeFacturasRecibidas.xls")
        Catch ex As Exception
            lblError.Text = "Error al tratar de exportar reporte a Excel. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub dgAux_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Cells(0).CssClass = "text"
            e.Item.Cells(1).CssClass = "text"
            e.Item.Cells(2).CssClass = "text"
        End If
    End Sub

    Private Sub lbExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbExportar.Click
        exportarReporteAExcel()
    End Sub

End Class
