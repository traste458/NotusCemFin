Partial Class estadoVentaSinHistorial
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
        lblRes.Text = ""
    End Sub

    Private Sub btnContinuar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinuar.Click
        Dim direcc As String
        direcc = "serialSinHistorico.aspx?serial=" + Trim(txtSerial.Text)
        direcc += "&tipoSerial=" + rblTipoSerial.SelectedValue
        Response.Redirect(direcc, True)
    End Sub

End Class
