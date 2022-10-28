Imports System.Data.SqlClient

Partial Class inicioBuscarOperadorLogistico
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
        Seguridad.verificarSession(Me)
        lblError.Text = ""
        If Not Me.IsPostBack Then
            hlRegresar.NavigateUrl = MetodosComunes.getUrlFrameBack(Me)
            getCiudades()
        End If
    End Sub

    Private Sub getCiudades()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtCiudades As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idciudad, upper(rtrim(ciudad))+' ('+upper(rtrim(departamento))+"
        sqlSelect += "')' as ciudad from ciudades where estado=1 order by ciudad"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtCiudades)
            ddlCiudad.DataSource = dtCiudades
            ddlCiudad.DataTextField = "ciudad"
            ddlCiudad.DataValueField = "idciudad"
            ddlCiudad.DataBind()
            ddlCiudad.Items.Insert(0, New ListItem("Escoja una Ciudad", 0))
        Catch ex As Exception
            lblError.Text = "Error al tratar de obtener el listado de Ciudades. " & ex.Message & "<br><br>"
        Finally
            sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub btnBuscar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBuscar.Click
        Dim filtros As String
        filtros = txtNombre.Text & ";" & txtIdentificacion.Text & ";" & ddlCiudad.SelectedValue
        Session.Remove("filtrosBuscarOL")
        Session("filtrosBuscarOL") = filtros
        Response.Redirect("buscarOperadorLogistico.aspx", True)
    End Sub


End Class
