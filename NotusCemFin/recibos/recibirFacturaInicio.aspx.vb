Imports System.Data.SqlClient

Partial Class recibirFacturaInicio
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
                getTiposProducto()
                getTipoRecepcion()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getTiposProducto()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtTipos As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idTipoProducto,tipoProducto from TipoProducto where estado=1"

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

    Private Sub getTipoRecepcion()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtTipoRecepcion As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idTipoRecepcion,tipoRecepcion from TipoRecepcion where estado=1"

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
        End Try
    End Sub

    Private Sub getInformacionDeDesicion(ByRef regionalizado As Boolean, ByRef esBP As Boolean)
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String

        sqlSelect = "select regionalizado,esBP from TipoProducto with(nolock) where idTipoProducto=@idTipoProducto"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idTipoProducto", ddlTipoProducto.SelectedValue)
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                regionalizado = CBool(sqlRead.GetValue(0))
                esBP = CBool(sqlRead.GetValue(1))
            End If
            sqlRead.Close()
        Catch ex As Exception
            Throw New Exception("Error al tratar de realizar validación para redirección. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Function getIdFactura(ByVal regionalizado As Boolean) As Integer
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String, idFactura As Integer

        If regionalizado = True Then
            sqlSelect = "select idfactura from facturas_externas where idfactura2=@factura"
        Else
            sqlSelect = "select idFactura from Factura where factura=@factura"
        End If
        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@factura", SqlDbType.VarChar).Value = txtFactura.Text
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                idFactura = sqlRead.GetValue(0)
            End If
            sqlRead.Close()
            Return idFactura
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el Identificador de la Factura. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Function

    Private Sub btnContinuar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnContinuar.Click
        Dim regionalizado, esBP, esTraslado As Boolean, url As String, idFactura As Integer

        Try
            getInformacionDeDesicion(regionalizado, esBP)
            If ddlTipoRecepcion.SelectedItem.Text.IndexOf("TRASLADO") <> -1 Then esTraslado = True
            idFactura = getIdFactura(regionalizado)
            If regionalizado = True Then
                url = "verFacturasPendientes.aspx?factura=" & txtFactura.Text & "&idTp=" & ddlTipoProducto.SelectedValue
            Else
                If esTraslado = False Or idFactura = 0 Then
                    url = "crearNuevaFactura.aspx?idTp=" & ddlTipoProducto.SelectedValue
                Else
                    url = "recibirFacturaExistentePorTraslado.aspx?idTp=" & ddlTipoProducto.SelectedValue
                End If
            End If
            url += "&idTr=" & ddlTipoRecepcion.SelectedValue
            If esTraslado = True Then
                url += "&esTras=true&idF=" & idFactura.ToString
            End If
            Response.Redirect(url, True)
        Catch ex As Exception
            lblError.Text = ex.Message & "<br><br>"
        End Try
    End Sub
End Class
