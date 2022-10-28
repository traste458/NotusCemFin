Imports System.Data.SqlClient
Partial Class consultaFactura
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblCadena As System.Web.UI.WebControls.Label
    Protected WithEvents sim_incluida As System.Web.UI.WebControls.DropDownList
    Protected WithEvents btnDescartar As System.Web.UI.WebControls.Button
    Protected WithEvents pnlControl As Anthem.Panel

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
                hlRegresar.NavigateUrl = MetodosComunes.getUrlFrameBack(Me)
                getTiposProducto()
                getProveedor()
                cargarProductos(0, 0)
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar Páginas. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub ddlTipoProducto_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoProducto.SelectedIndexChanged
        If Session.Count > 0 Then
            Try
                cargarProductos(ddlTipoProducto.SelectedValue, ddlProveedor.SelectedValue)
            Catch ex As Exception
                lblError.Text = ex.Message & "<br><br>"
            End Try
        End If
    End Sub

    Private Sub getTiposProducto()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtTipos As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idTipoProducto,tipoProducto from TipoProducto with(nolock) where estado=1 and regionalizado=1 "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtTipos)
            With ddlTipoProducto
                .DataSource = dtTipos
                .DataTextField = "tipoProducto"
                .DataValueField = "idTipoProducto"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Tipo de Producto", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar Tipos de Producto. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub getProveedor()
        Try
            With ddlProveedor
                .DataSource = MetodosComunes.getAllProveedores
                .DataTextField = "proveedor"
                .DataValueField = "idproveedor"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Proveedor", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener Proveedores. " & ex.Message)
        End Try
    End Sub

    Private Sub cargarProductos(ByVal idTipoProducto As Integer, ByVal idProveedor As Integer)
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtProductos As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idproducto,producto from productos with(nolock) where estado in (1) "
        If idTipoProducto <> 0 Then
            sqlSelect += " and idproducto in (select idProducto from DetalleProductoTipoProducto "
            sqlSelect += " with(nolock) where idTipoProducto=@idTipoProducto) "
        End If
        If idProveedor <> 0 Then sqlSelect += " and idproveedor=@idProveedor "
        sqlSelect += " order by producto "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlComando.Parameters.Add("@idTipoProducto", SqlDbType.Int).Value = idTipoProducto
            sqlComando.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor
            sqlAdaptador.Fill(dtProductos)
            With ddlProducto
                .DataSource = dtProductos
                .DataTextField = "producto"
                .DataValueField = "idproducto"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Producto", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el listado de Productos. " & ex.Message)
        End Try
    End Sub

    Private Sub ddlProveedor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlProveedor.SelectedIndexChanged
        If Session.Count > 0 Then
            Try
                cargarProductos(ddlTipoProducto.SelectedValue, ddlProveedor.SelectedValue)
            Catch ex As Exception
                lblError.Text = ex.Message & "<br><br>"
            End Try
        End If
    End Sub

    Private Sub btnContinuar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinuar.Click
        Dim htFiltros As New Hashtable, url As String

        Try
            With htFiltros
                .Add("factura", txtFactura.Text.Trim)
                .Add("idTipoProducto", ddlTipoProducto.SelectedValue)
                .Add("idProveedor", ddlProveedor.SelectedValue)
                .Add("idProducto", ddlProducto.SelectedValue)
                .Add("fechaEsperada", fechaInicial.Value)
            End With
            Session("htFiltrosBuscarFacPendientes") = htFiltros
            Response.Redirect("reporteFacturasPendientesRecibir.aspx", True)
        Catch ex As Exception
            lblError.Text = "Error al tratar de redireccionar página. " & ex.Message & "<br><br>"
        End Try
    End Sub
End Class
