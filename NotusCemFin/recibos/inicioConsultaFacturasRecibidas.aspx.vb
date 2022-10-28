Imports System.Data.SqlClient

Partial Class inicioConsultaFacturasRecibidas
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
            If Not IsPostBack And Not Anthem.Manager.IsCallBack Then
                hlRegresar.NavigateUrl = MetodosComunes.getUrlFrameBack(Me)
                getTiposProducto()
                getProveedor()
                getProductos(0, 0)
                getTipoRecepcion()
                getEstadosRecepcionFactura()
                getEstadosFactura()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getTiposProducto()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtTipos As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idTipoProducto,tipoProducto from TipoProducto with(nolock) where estado=1"

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

    Private Sub getProductos(ByVal idTipoProducto As Integer, ByVal idProveedor As Integer)
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtProductos As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idproducto,producto from productos with(nolock) where estado in (1,2) "
        If idTipoProducto <> 0 Then
            sqlSelect += " and idproducto in (select idProducto from DetalleProductoTipoProducto "
            sqlSelect += " with(nolock) where idTipoProducto=@idTipoProducto) "
        End If
        If idProveedor <> 0 Then sqlSelect += " and idproveedor=@idProveedor "
        sqlSelect += " order by producto "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            With sqlComando.Parameters
                If idTipoProducto <> 0 Then .Add("@idTipoProducto", SqlDbType.Int).Value = idTipoProducto
                If idProveedor <> 0 Then .Add("@idProveedor", SqlDbType.Int).Value = idProveedor
            End With
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
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub getTipoRecepcion()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtTipoRecepcion As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idTipoRecepcion,tipoRecepcion from TipoRecepcion with(nolock) where estado=1"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtTipoRecepcion)
            With ddlTipoRecepcion
                .DataSource = dtTipoRecepcion
                .DataTextField = "tipoRecepcion"
                .DataValueField = "idTipoRecepcion"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Tipo Recepción", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener Tipos de Recepción. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub getEstadosRecepcionFactura()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtEstados As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idEstadoRecepcion,estadoRecepcion from EstadoRecepcionFactura with(nolock) order by idEstadoRecepcion"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtEstados)
            With ddlEstadoRecepcion
                .DataSource = dtEstados
                .DataTextField = "estadoRecepcion"
                .DataValueField = "idEstadoRecepcion"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Estado", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el listado de Estados de Recepción. " & ex.Message)
        Finally
            If Not dtEstados Is Nothing Then dtEstados.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub getEstadosFactura()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtEstados As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idEstado,estado from EstadoFactura with(nolock) where idEstado>0 order by idEstado"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtEstados)
            With ddlEstado
                .DataSource = dtEstados
                .DataTextField = "estado"
                .DataValueField = "idEstado"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Estado", "-2"))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el listado de Estados. " & ex.Message)
        End Try
    End Sub

    Private Sub ddlTipoProducto_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlTipoProducto.SelectedIndexChanged
        If Session.Count > 0 Then
            Try
                getProductos(ddlTipoProducto.SelectedValue, ddlProveedor.SelectedValue)
            Catch ex As Exception
                lblError.Text = ex.Message & "<br><br>"
            End Try
        End If
    End Sub

    Private Sub ddlProveedor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlProveedor.SelectedIndexChanged
        If Session.Count > 0 Then
            Try
                getProductos(ddlTipoProducto.SelectedValue, ddlProveedor.SelectedValue)
            Catch ex As Exception
                lblError.Text = ex.Message & "<br><br>"
            End Try
        End If
    End Sub

    Private Sub btnContinuar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinuar.Click
        Dim url As String, filtros As New filtroBusquedaFacturasOP

        Try
            With filtros
                .factura = txtFactura.Text
                .ordenCompra = txtOrdenCompra.Text
                .guia = txtGuia.Text
                .idTipoProducto = ddlTipoProducto.SelectedValue
                .idProveedor = ddlProveedor.SelectedValue
                .idProducto = ddlProducto.SelectedValue
                .idTipoRecepcion = ddlTipoRecepcion.SelectedValue
                .idEstadoRecepcion = ddlEstadoRecepcion.SelectedValue
                .idEstadoFactura = ddlEstado.SelectedValue
                .fechaInicial = fechaInicial.Value
                .fechaFinal = fechaFinal.Value
            End With
            Session.Remove("filtrosReporteFacturasRecibidas")
            Session("filtrosReporteBuscarFacturasRecibidas") = filtros
            Response.Redirect("reporteFacturasRecibidas.aspx", True)
        Catch ex As Exception
            lblError.Text = "Error al tratar de redireccionar página. " & ex.Message & "<br><br>"
        End Try
    End Sub

End Class
