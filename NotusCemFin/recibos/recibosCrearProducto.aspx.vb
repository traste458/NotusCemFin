Imports System.Data.SqlClient

Partial Class recibosCrearProducto
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
            lblRes.Text = ""
            If Not Me.IsPostBack And Not Anthem.Manager.IsCallBack Then
                hlRegresar.NavigateUrl = MetodosComunes.getUrlFrameBack(Me)
                getProveedores()
                getTiposProducto()
                getProductosRegistrados()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de Cargar Página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getProveedores()
        Try
            With ddlProveedor
                .DataSource = MetodosComunes.getAllProveedores
                .DataTextField = "proveedor"
                .DataValueField = "idproveedor"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Proveedor", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el listado de Proveedores. " & ex.Message)
        End Try
    End Sub

    Private Sub getTiposProducto()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtTiposProducto As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idTipoProducto,tipoProducto from TipoProducto where estado=1 and regionalizado=0"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtTiposProducto)
            With ddlTipoProducto
                .DataSource = dtTiposProducto
                .DataTextField = "tipoProducto"
                .DataValueField = "idTipoProducto"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Tipo de Producto", "0"))
            End With
        Catch ex As Exception
            If Me.IsPostBack Then
                Throw New Exception("Error al tratar de obtener el listado de Tipos de Producto. " & ex.Message)
            Else
                lblError.Text = "Error al tratar de obtener el listado de Tipos de Producto. " & ex.Message & "<br><br>"
            End If
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub getProductosRegistrados()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtDatos As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select distinct p.idproducto,(select upper(proveedor) from proveedores where idproveedor=p.idproveedor)"
        sqlSelect += " as proveedor,upper(producto) as producto,idproducto2 as material from productos p with(nolock) inner"
        sqlSelect += " join DetalleProductoTipoProducto dpt with(nolock) on p.idproducto=dpt.idProducto where dpt.idTipoProducto"
        sqlSelect += " in (select idTipoProducto from TipoProducto where regionalizado=0) and estado in (1,2) "
        sqlSelect += " order by proveedor,producto"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtDatos)
            dgProductos.DataSource = dtDatos
            '*****Obtener los Tipos de Producto*****'
            getTiposPorProducto(dtDatos)
            '***************************************'
            dgProductos.Columns(0).FooterText = dtDatos.Rows.Count.ToString & " Registros Encontrados"
            dgProductos.DataBind()
            MetodosComunes.mergeFooter(dgProductos)
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener listado de Productos previamente registrados." & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub getTiposPorProducto(ByRef dtDatos As DataTable)
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

    'Private Sub dgProductos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgProductos.ItemDataBound
    '    If e.Item.ItemType = ListItemType.Footer Then
    '        For index As Integer = 1 To 4
    '            e.Item.Cells.RemoveAt(1)
    '        Next
    '        e.Item.Cells(0).ColumnSpan = 5
    '    End If
    'End Sub

    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        registrarDatos()
        getProductosRegistrados()
    End Sub

    Private Sub registrarDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlTrans As SqlTransaction, sqlInsert, sqlQuery As String
        Dim idProducto, idTecnologia, estado As Integer

        sqlInsert = "insert into productos(idproducto,idproducto2,producto,idproveedor,idtipo,estado,sims_sn)"
        sqlInsert += " values(@idProducto,@material,@producto,@idProveedor,@idTecnologia,@estado,'N')"

        sqlQuery = "insert into DetalleProductoTipoProducto(idProducto,idTipoProducto,fechaRegistro,idUsuarioRegistra) "
        sqlQuery += " values (@idProducto,@idTipoProducto,getdate(),@idUsuario)"

        Try
            With ddlTipoProducto.SelectedItem.Text.ToUpper
                If .IndexOf("TARJETAS PREPAGO") <> -1 Or .IndexOf("TARJETA PREPAGO") <> -1 Then
                    idTecnologia = 9015
                    estado = 1
                Else
                    idTecnologia = 0
                    estado = 2
                End If
            End With
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlInsert)
            With sqlComando.Parameters
                .Add("@producto", SqlDbType.VarChar, 100).Value = txtProducto.Text.ToUpper
                .Add("@idProveedor", SqlDbType.Int).Value = ddlProveedor.SelectedValue
                .Add("@idTipoProducto", SqlDbType.Int).Value = ddlTipoProducto.SelectedValue
                .Add("@idTecnologia", SqlDbType.Int).IsNullable = True
                .Item("@idTecnologia").Value = IIf(idTecnologia <> 0, idTecnologia, DBNull.Value)
                .Add("@estado", SqlDbType.Int).Value = estado
                .Add("@material", SqlDbType.VarChar, 10).IsNullable = True
                .Item("@material").Value = IIf(Not txtMaterial.Text.Trim.Equals(""), txtMaterial.Text.Trim, DBNull.Value)
                .Add("@idUsuario", SqlDbType.Int).Value = Session("usxp001")
            End With
            sqlConexion.Open()
            If esNombreValido(sqlConexion) Then
                sqlTrans = sqlConexion.BeginTransaction
                idProducto = getIdProducto(sqlConexion, sqlTrans)
                If idProducto <> 0 Then
                    sqlComando.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto
                    sqlComando.Transaction = sqlTrans
                    sqlComando.ExecuteNonQuery()
                    sqlComando.CommandText = sqlQuery
                    sqlComando.ExecuteNonQuery()
                Else
                    Throw New Exception("Imposible obtener el ID que se asignará al Producto.")
                End If
                sqlTrans.Commit()
                lblRes.Text = "El Producto se registró satisfactoriamente.<br><br>"
                txtProducto.Text = ""
                ddlProveedor.SelectedIndex = 0
                ddlTipoProducto.SelectedIndex = 0
            Else
                lblError.Text = "El nombre del Producto que está tratando de registrar ya se encuentra registrado. Por favor verifique.<br><br>"
            End If
        Catch ex As Exception
            'If Not sqlTrans Is Nothing Then sqlTrans.Rollback()
            lblError.Text = "Error al tratar de registrar datos. " & ex.Message & "<br><br>"
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try

    End Sub

    Private Function esNombreValido(ByVal sqlConexion As SqlConnection) As Boolean
        Dim sqlComando As SqlCommand, sqlSelect As String, resultado As Boolean
        Dim sqlRead As SqlDataReader

        sqlSelect = "select count(idproducto) from productos where producto=@producto"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@producto", SqlDbType.VarChar).Value = txtProducto.Text.Trim
            resultado = Not CBool(sqlComando.ExecuteScalar)
            Return resultado
        Catch ex As Exception
            Throw New Exception("Error al tratar de validar el nombre del Producto. " & ex.Message)
        Finally
            sqlComando.Dispose()
        End Try
    End Function

    Private Function getIdProducto(ByVal sqlConexion As SqlConnection, ByVal sqlTrans As SqlTransaction) As Integer
        Dim sqlComando As SqlCommand, sqlSelect, sqlUpdate As String
        Dim sqlRead As SqlDataReader, idProducto As Integer

        sqlSelect = "select isnull(max(idproducto),0)+1 from productos"
        sqlUpdate = "update secuencias set numero=@idProducto where secuencia='productos'"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Transaction = sqlTrans
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                idProducto = sqlRead.GetValue(0)
            Else
                idProducto = 1
            End If
            sqlRead.Close()
            sqlComando.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto
            sqlComando.CommandText = sqlUpdate
            sqlComando.ExecuteNonQuery()
            Return idProducto
        Catch ex As Exception
            If Not sqlTrans Is Nothing Then sqlTrans.Rollback()
            Throw New Exception("Error al tratar de obtener ID de Producto. " & ex.Message)
        End Try
    End Function

    Private Sub dgProductos_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgProductos.PageIndexChanged
        If Session.Count > 0 Then
            Try
                dgProductos.CurrentPageIndex = e.NewPageIndex
                getProductosRegistrados()
            Catch ex As Exception
                lblError.Text = "Error al tratar de Pagina Tabla. " & ex.Message
            End Try
        End If
    End Sub
End Class
