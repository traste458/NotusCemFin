Imports System.Data.SqlClient
Imports System.IO


Partial Class resultadoBuscarFacturasRecibidas
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents fechaFin As System.Web.UI.HtmlControls.HtmlInputHidden

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

    Dim filtros As New filtroBusquedaFacturasOP

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me)
            lblError.Text = ""
            If Not Me.IsPostBack Then
                filtros = CType(Session("filtrosBuscarFacturasRecibidas"), filtroBusquedaFacturasOP)
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

        With filtros
            sqlSelect = "select idFactura,(select proveedor from proveedores with(nolock) where idproveedor=fe.idProveedor)"
            sqlSelect += " as proveedor,(select producto from productos with(nolock) where idproducto=fe.idproducto) as producto,"
            sqlSelect += " (select tipoProducto from TipoProducto with(nolock) where idTipoProducto=fe.idTipoProducto)as tipoProducto,"
            sqlSelect += " fe.guia_aerea as guia,fe.idfactura2 as factura,fe.ordenCompra,fe.numeroPalets,fe.cantidadEsperada,"
            sqlSelect += " fe.cantidadAprox,fe.peso,(select tipoRecepcion from TipoRecepcion with(nolock) where idTipoRecepcion="
            sqlSelect += " fe.idTipoRecepcion)as tipoRecepcion,(select estadoRecepcion from EstadoRecepcionFactura with(nolock)"
            sqlSelect += " where idEstadoRecepcion=fe.idEstadoRecepcion) as estadoRecepcion,fe.fecha,(select bodega from bodegas"
            sqlSelect += " with(nolock) where idbodega=fe.idBodega) as bodega,(select estado from EstadoFactura with(nolock)"
            sqlSelect += " where idEstado=fe.estado) as estadoFactura,fe.observacion,case when estado=2 and cantidad_pedida=0 then"
            sqlSelect += " 'actualizarFacturaRecibida.aspx?idFactura='+convert(varchar,fe.idfactura)+'&isFrom=fe' else"
            sqlSelect += " 'actualizarSalidaDeFactura.aspx?idF='+convert(varchar,fe.idfactura)+'&isfrom=fe'end as url from"
            sqlSelect += " facturas_externas fe with(nolock) where fe.idClasificacion in (1,2)"

            sqlQuery = " select idFactura,(select proveedor from proveedores with(nolock) where idproveedor=f.idProveedor)"
            sqlQuery += " as proveedor,(select producto from productos with(nolock) where idproducto=f.idproducto)as producto,"
            sqlQuery += " (select tipoProducto from TipoProducto with(nolock) where idTipoProducto=f.idTipoProducto) as"
            sqlQuery += " tipoProducto,guia,factura,ordenCompra,numeroPalets,cantidadAprox as cantidadEsperada,cantidadAprox,"
            sqlQuery += " peso,(select tipoRecepcion from TipoRecepcion with(nolock) where idTipoRecepcion=f.idTipoRecepcion)"
            sqlQuery += " as tipoRecepcion,(select estadoRecepcion from EstadoRecepcionFactura with(nolock) where idEstadoRecepcion="
            sqlQuery += " f.idEstadoRecepcion) as estadoRecepcion,fechaLlegada as fecha,(select rtrim(bodega) from bodegas"
            sqlQuery += " with(nolock) where idbodega=f.idBodega) as bodega, (select estado from EstadoFactura with(nolock)"
            sqlQuery += " where idEstado=f.estado) as estadoFactura,observacion,case when estado=2 then "
            sqlQuery += " 'actualizarFacturaRecibida.aspx?idFactura='+convert(varchar,idFactura)+'&isFrom=f' "
            sqlQuery += " else 'actualizarSalidaDeFactura.aspx?idF='+convert(varchar,idFactura)+'&isfrom=f' end as url "
            sqlQuery += " from Factura f with(nolock) where idFactura is not null "

            Dim hayFiltros As Boolean = False
            If .factura <> "" Then
                sqlSelect += " and fe.idfactura2=@factura "
                sqlQuery += " and factura=@factura "
                hayFiltros = True
            End If
            If .ordenCompra <> "" Then
                sqlSelect += " and fe.ordenCompra=@ordenCompra "
                sqlQuery += " and ordenCompra=@ordenCompra "
                hayFiltros = True
            End If
            If .guia <> "" Then
                sqlSelect += " and fe.guia_aerea=@guia "
                sqlQuery += " and guia=@guia "
                hayFiltros = True
            End If
            If .fechaInicial <> "" Then
                sqlSelect += " and convert(varchar,fecha,112) between @fechaInicial and @fechaFinal "
                sqlQuery += " and convert(varchar,fechaLlegada,112) between @fechaInicial and @fechaFinal "
                hayFiltros = True
            End If
            If .idProducto <> 0 Then
                sqlSelect += " and fe.idproducto=@idProducto "
                sqlQuery += " and idProducto=@idProducto "
                hayFiltros = True
            End If
            If .idTipoProducto <> 0 Then
                sqlSelect += " and fe.idTipoProducto=@idTipoProducto "
                sqlQuery += " and f.idTipoProducto=@idTipoProducto "
                hayFiltros = True
            End If
            If .idProveedor <> 0 Then
                sqlSelect += " and fe.idProveedor=@idProveedor "
                sqlQuery += " and idProveedor=@idProveedor "
                hayFiltros = True
            End If
            If .idTipoRecepcion <> 0 Then
                sqlSelect += " and fe.idTipoRecepcion=@idTipoRecepcion "
                sqlQuery += " and idTipoRecepcion=@idTipoRecepcion "
                hayFiltros = True
            End If
            If .idEstadoRecepcion <> 0 Then
                sqlSelect += " and fe.idEstadoRecepcion=@idEstadoRecepcion "
                sqlQuery += " and idEstadoRecepcion=@idEstadoRecepcion "
                hayFiltros = True
            End If
            If .idEstadoFactura <> -2 Then
                sqlSelect += " and estado=@idEstado"
                sqlQuery += " and estado=@idEstado"
            ElseIf Not hayFiltros Then
                sqlSelect += " and estado in (select idEstado from EstadoFactura with(nolock) where "
                sqlSelect += " idEstado>0 and tipoEstado='ACTIVO') "

                sqlQuery += " and estado in (select idEstado from EstadoFactura with(nolock) where "
                sqlQuery += " idEstado>0 and tipoEstado='ACTIVO') "
            End If

            Select Case .esRegionalizado.ToUpper
                Case ""
                    sqlSelect += " union " & sqlQuery
                Case "FALSE"
                    sqlSelect = sqlQuery
            End Select
        End With
        sqlSelect += " order by proveedor,producto,tipoRecepcion,fecha,guia,factura"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            With sqlComando.Parameters
                If filtros.factura <> "" Then .Add("@factura", SqlDbType.VarChar).Value = filtros.factura
                If filtros.ordenCompra <> "" Then .Add("@ordenCompra", SqlDbType.VarChar).Value = filtros.ordenCompra
                If filtros.guia <> "" Then .Add("@guia", SqlDbType.VarChar).Value = filtros.guia
                If filtros.fechaInicial <> "" Then
                    .Add("@fechaInicial", SqlDbType.VarChar).Value = String.Format("{0:yyyyMMdd}", CDate(filtros.fechaInicial))
                    .Add("@fechaFinal", SqlDbType.VarChar).Value = String.Format("{0:yyyyMMdd}", CDate(filtros.fechaFinal))
                End If
                If filtros.idTipoProducto <> 0 Then .Add("@idTipoProducto", SqlDbType.Int).Value = filtros.idTipoProducto
                If filtros.idProducto <> 0 Then .Add("@idProducto", SqlDbType.Int).Value = filtros.idProducto
                If filtros.idProveedor <> 0 Then .Add("@idProveedor", SqlDbType.Int).Value = filtros.idProveedor
                If filtros.idTipoRecepcion <> 0 Then .Add("@idTipoRecepcion", SqlDbType.Int).Value = filtros.idTipoRecepcion
                If filtros.idEstadoRecepcion <> 0 Then .Add("@idEstadoRecepcion", SqlDbType.Int).Value = filtros.idEstadoRecepcion
                If filtros.idEstadoFactura <> -2 Then .Add("@idEstado", SqlDbType.Int).Value = filtros.idEstadoFactura
            End With
            sqlAdaptador.Fill(dtDatos)
            If dtDatos.Rows.Count > 0 Then
                With dgFacturas
                    .DataSource = dtDatos
                    .Columns(0).FooterText = dtDatos.Rows.Count.ToString & " Registro(s) Entrado(s)"
                    .DataBind()
                End With
                MetodosComunes.mergeFooter(dgFacturas)
                Session("dtBuscarFacturasRecibidas") = dtDatos
                lbExportar.Visible = True
            Else
                lblError.Text = "No se encontraron Facturas con las Características Solicitadas.<br><br>"
                elTitulo.Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener datos. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub lbExportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbExportar.Click
        Try
            Dim dgAux As DataGrid, dtDatos As DataTable
            Dim sw As New StringWriter, htw As New System.Web.UI.HtmlTextWriter(sw)
            dtDatos = CType(Session("dtBuscarFacturasRecibidas"), DataTable)
            dgAux = MetodosComunes.clonarDataGrid(dgFacturas)
            AddHandler dgAux.ItemDataBound, AddressOf dgAux_ItemDataBound
            With dgAux
                .AllowPaging = False
                .AlternatingItemStyle.BorderColor = Nothing
                .DataSource = dtDatos
                .Columns(0).FooterText = dtDatos.Rows.Count.ToString & " Registro(s) Entrado(s)"
                .DataBind()
                For Each dgItem As DataGridItem In .Items
                    For index As Integer = 0 To dgItem.Cells.Count - 2
                        With CType(dgItem.Cells(index).Controls(0), HyperLink)
                            .NavigateUrl = Nothing
                        End With
                    Next
                Next
                .RenderControl(htw)
            End With

            MetodosComunes.exportarDatosAExcel(HttpContext.Current, sw.ToString, "Buscar Facturas Recibidas - Resultado", "ResultadoBuscarFacturasRecibidas.xls")
        Catch ex As Exception
            lblError.Text = "Error al tratar de exportar resultado a Excel. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub dgAux_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs)
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            e.Item.Cells(3).CssClass = "text"
            e.Item.Cells(4).CssClass = "text"
            e.Item.Cells(5).CssClass = "text"
        ElseIf e.Item.ItemType = ListItemType.Footer Then
            For index As Byte = 1 To e.Item.Cells.Count - 1
                e.Item.Cells(index).Visible = False
            Next
            e.Item.Cells(0).ColumnSpan = e.Item.Cells.Count
        End If
    End Sub

    Private Sub dgFacturas_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgFacturas.PageIndexChanged
        Try
            Dim dtDatos As New DataTable
            dtDatos = CType(Session("dtBuscarFacturasRecibidas"), DataTable)
            With dgFacturas
                .CurrentPageIndex = e.NewPageIndex
                .DataSource = dtDatos
                .Columns(0).FooterText = dtDatos.Rows.Count.ToString & " Registro(s) Entrado(s)"
                .DataBind()
            End With
        Catch ex As Exception
            lblError.Text = "Error al tratar de cambiar página. " & ex.Message & "<br><br>"
        End Try
    End Sub
End Class

