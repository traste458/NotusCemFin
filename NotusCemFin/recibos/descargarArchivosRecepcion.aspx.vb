Imports System.IO

Partial Class descargarArchivosRecepcion
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
        Dim nombreArchivo, ruta As String
        Try
            Seguridad.verificarSession(Me)
            nombreArchivo = Server.UrlDecode(Request.QueryString("file"))
            ruta = Server.MapPath("ArchivosRecepcion/") & nombreArchivo
            If File.Exists(ruta) Then
                Response.Clear()
                Response.ContentType = "application/octet-stream"
                Response.AddHeader("Content-Disposition", _
                  "attachment; filename=" & nombreArchivo)
                Response.Flush()
                Response.WriteFile(ruta)
                Response.End()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de Descargar Archivo. "
        End Try
    End Sub
End Class
