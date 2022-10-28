Partial Class inicioVerArchivosRecepcion
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
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message
        End Try
    End Sub

    Private Sub btnContinuar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinuar.Click
        Dim htFiltros As New Hashtable
        Try
            With htFiltros
                .Add("factura", txtFactura.Text.Trim)
                .Add("guia", txtGuia.Text.Trim)
                .Add("fechaInicial", fechaInicial.Value)
                .Add("fechaFinal", fechaFinal.Value)
            End With
            Session("htFiltrosVerArchivosRecepcion") = htFiltros
            Response.Redirect("verArchivosRecepcion.aspx", True)
        Catch ex As Exception
            lblError.Text = "Error al tratar de redireccionar página. "
        End Try
    End Sub

End Class
