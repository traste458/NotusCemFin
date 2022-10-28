Imports System.Data.SqlClient

Partial Class recibosActualizarProducto
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

    Dim idProducto As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me)
            lblError.Text = ""
            lblRes.Text = ""
            idProducto = Request.QueryString("idP")
            If Not Me.IsPostBack Then
                hlRegresar.NavigateUrl = MetodosComunes.getUrlFrameBack(Me)
                getProveedores()
                getTiposProducto()
                getDatosProductoRegistrados()
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
                .Items.Insert(0, New ListItem("Escoja un Tipo de Producto", 0))
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

    Private Sub getDatosProductoRegistrados()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String
        Dim idProveedor, estado As Integer

        sqlSelect = "select producto,idproveedor,(select top 1 idTipoProducto from "
        sqlSelect += " DetalleProductoTipoProducto where idProducto=p.idproducto) as idTipoProducto, "
        sqlSelect += " case when estado=1 or estado=2 then 1 else 0 end "
        sqlSelect += " as estado,idproducto2 as material from productos p where idproducto=@idProducto "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                txtProducto.Text = sqlRead.GetValue(0).ToString
                idProveedor = IIf(IsDBNull(sqlRead.GetValue(1)), 0, sqlRead.GetValue(1))
                hIdTipoProducto.Value = sqlRead.GetValue(2).ToString
                estado = sqlRead.GetValue(3).ToString
                With ddlProveedor
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(idProveedor.ToString))
                End With
                With ddlTipoProducto
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(hIdTipoProducto.Value))
                End With
                txtMaterial.Text = sqlRead.GetValue(4).ToString
                With ddlEstado
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(estado.ToString))
                End With
            Else
                lblError.Text = "Imposible obtener la información registrada del producto. "
            End If
            sqlRead.Close()
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener la información registrada del Productos previamente registrados." & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Function esNombreValido(ByVal sqlConexion As SqlConnection) As Boolean
        Dim sqlComando As SqlCommand, sqlSelect As String, resultado As Boolean
        Dim sqlRead As SqlDataReader

        sqlSelect = "select count(idproducto) from productos where producto=@producto and idproducto<>@idProducto"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@producto", SqlDbType.VarChar).Value = txtProducto.Text.Trim
            sqlComando.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto
            resultado = Not CBool(sqlComando.ExecuteScalar)
            Return resultado
        Catch ex As Exception
            Throw New Exception("Error al tratar de validar el nombre del Producto. " & ex.Message)
        Finally
            sqlComando.Dispose()
        End Try
    End Function

    Private Sub actualizarDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand, sqlTrans As SqlTransaction
        Dim sqlUpdate, sqlQuery As String, idTecnologia, estado As Integer

        sqlUpdate = "update productos set producto=@producto,idproveedor=@idProveedor,"
        sqlUpdate += "estado=@estado,idtipo=@idTecnologia,idproducto2=@material where idproducto=@idProducto"

        sqlQuery = "update DetalleProductoTipoProducto set idTipoProducto=@idTipoProducto "
        sqlQuery += " where idProducto=@idProducto and idTipoProducto=@idTipoActual "

        Try
            With ddlTipoProducto.SelectedItem.Text.ToUpper
                If .IndexOf("TARJETAS PREPAGO") <> -1 Or .IndexOf("TARJETA PREPAGO") <> -1 Then
                    idTecnologia = 903
                    estado = ddlEstado.SelectedValue
                Else
                    idTecnologia = 0
                    estado = IIf(ddlEstado.SelectedValue = 1, 2, 0)
                End If
            End With
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlUpdate)
            sqlConexion.Open()
            If esNombreValido(sqlConexion) Then
                With sqlComando.Parameters
                    .Add("@idProducto", SqlDbType.Int).Value = idProducto
                    .Add("@producto", SqlDbType.VarChar).Value = txtProducto.Text.Trim.ToUpper
                    .Add("@idProveedor", SqlDbType.Int).Value = ddlProveedor.SelectedValue
                    .Add("@idTipoProducto", SqlDbType.Int).Value = ddlTipoProducto.SelectedValue
                    .Add("@idTipoActual", SqlDbType.Int).Value = hIdTipoProducto.Value
                    .Add("@material", SqlDbType.VarChar, 10).Value = txtMaterial.Text.Trim
                    .Add("@estado", SqlDbType.Int).Value = estado
                    .Add("@idTecnologia", SqlDbType.Int).IsNullable = True
                    .Item("@idTecnologia").Value = IIf(idTecnologia <> 0, idTecnologia, DBNull.Value)
                End With
                sqlTrans = sqlConexion.BeginTransaction
                sqlComando.Transaction = sqlTrans
                sqlComando.ExecuteNonQuery()
                sqlComando.CommandText = sqlQuery
                sqlComando.ExecuteNonQuery()
                sqlTrans.Commit()
                lblRes.Text = "El Producto se actualizó satisfactoriamente.<br><br>"
            Else
                If Not sqlTrans Is Nothing Then sqlTrans.Rollback()
                lblError.Text = "Ya existe otro registro con el mismo nombre que está tratando de registrar para el producto actual. Por favor verifique.<br><br>"
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de Actualizar datos. " & ex.Message & "<br><br>"
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub btnActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizar.Click
        actualizarDatos()
    End Sub
End Class
