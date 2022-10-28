Imports System.Data.SqlClient

Partial Class recibosBuscarProducto
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
                hlRegresar.NavigateUrl = MetodosComunes.getUrlFrameBack(Me)
                getProveedores()
                getTiposProducto()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
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

    Private Sub btnbuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnbuscar.Click
        Dim filtros As String
        Try
            filtros = txtProducto.Text & ";" & ddlProveedor.SelectedValue & ";"
            filtros += ddlTipoProducto.SelectedValue
            Session.Remove("filtrosBuscarProductoRecibos")
            Session("filtrosBuscarProductoRecibos") = filtros
            Response.Redirect("recibosResultadoBuscarProducto.aspx", True)
        Catch ex As Exception
            lblError.Text = "Error al tratar de redireccionar página. " & ex.Message
        End Try
    End Sub
End Class
