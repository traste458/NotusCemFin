Imports System.Data.SqlClient

Partial Class recibosResultadoBuscarProducto
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
            Seguridad.verificarSession(Me, Anthem.Manager.IsCallBack)
            lblError.Text = ""
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
        Dim sqlSelect, producto As String, idProveedor, idTipoProducto As Integer

        sqlSelect = "select p.idproducto,(select upper(proveedor) from proveedores where idproveedor=p.idproveedor) as"
        sqlSelect += " proveedor,upper(producto) as producto,'recibosActualizarProducto.aspx?idP='+convert(varchar,p.idproducto)"
        sqlSelect += " as url,idproducto2 as material from productos p with(nolock) inner join DetalleProductoTipoProducto dpt"
        sqlSelect += " with(nolock) on p.idproducto=dpt.idProducto where dpt.idTipoProducto in (select idTipoProducto from "
        sqlSelect += " TipoProducto where regionalizado=0) and estado in (1,2)"

        Try
            aplicarFiltros(producto, idProveedor, idTipoProducto)
            If producto <> "" Then sqlSelect += " and producto like '%'+@producto+'%'"
            If idProveedor <> 0 Then sqlSelect += " and idproveedor=@idProveedor "
            If idTipoProducto <> 0 Then sqlSelect += " and idTipoProducto=@idTipoProducto "
            sqlSelect += " order by proveedor,producto,idTipoProducto "
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            With sqlComando.Parameters
                .Add("@producto", SqlDbType.VarChar).Value = producto
                .Add("@idProveedor", SqlDbType.Int).Value = idProveedor
                .Add("@idTipoProducto", SqlDbType.Int).Value = idTipoProducto
            End With
            sqlAdaptador.Fill(dtDatos)
            '*****Obtener Tipos de Producto*****'
            getTiposDeProducto(dtDatos)
            '***********************************'
            If dtDatos.Rows.Count > 0 Then
                dgProductos.DataSource = dtDatos
                dgProductos.Columns(0).FooterText = dtDatos.Rows.Count.ToString & " Registros Encontrados"
                dgProductos.DataBind()
                MetodosComunes.mergeFooter(dgProductos)
            Else
                lblError.Text = "No se encontraron datos con las características solicitadas.<br><br>"
                lblTitulo.Visible = False
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener listado de Porductos previamente registrados." & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub aplicarFiltros(ByRef producto As String, ByRef idProveedor As Integer, _
        ByRef idTipoProducto As Integer)
        Dim filtros() As String
        Try
            filtros = CStr(Session("filtrosBuscarProductoRecibos")).Split(";")
            If filtros.GetUpperBound(0) = 2 Then
                producto = filtros(0)
                idProveedor = filtros(1)
                idTipoProducto = filtros(2)
            Else
                Throw New Exception("Imposible recuperar los filtros aplicados.")
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de aplicar filtros. " & ex.Message)
        End Try
    End Sub

     Private Sub dgProductos_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgProductos.PageIndexChanged
        If Session.Count > 0 Then
            Try
                dgProductos.CurrentPageIndex = e.NewPageIndex
                getDatos()
            Catch ex As Exception
                lblError.Text = "Error al tratar de Pagina Tabla. " & ex.Message
            End Try
        End If
    End Sub

    Private Sub getTiposDeProducto(ByRef dtDatos As DataTable)
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String
        Dim tipos As New System.Text.StringBuilder

        sqlSelect = "select tipoProducto from DetalleProductoTipoProducto dpt inner join TipoProducto tp "
        sqlSelect += " on dpt.idTipoProducto=tp.idTipoProducto where idProducto=@idProducto order by tipoProducto"

        Try
            dtDatos.Columns.Add("tipoProducto", GetType(System.String))
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idProducto", SqlDbType.Int).Value = 0
            sqlConexion.Open()
            For Each drDato As DataRow In dtDatos.Rows
                sqlComando.Parameters("@idProducto").Value = drDato.Item("idproducto")
                sqlRead = sqlComando.ExecuteReader
                While sqlRead.Read
                    tipos.Append(sqlRead.GetValue(0).ToString & "<br>")
                End While
                drDato.Item("tipoProducto") = tipos.ToString
                tipos.Length = 0
                sqlRead.Close()
            Next
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los Tipos de Producto a los cuales están asociados cada uno de los Productos. " & ex.Message)
        Finally
            If Not sqlComando Is Nothing Then sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try

    End Sub

End Class
