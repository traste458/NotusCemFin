Imports System.Data.SqlClient

Partial Class verArchivosRecepcion
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
                getDatos()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtDatos As New DataTable
        Dim sqlSelect, sqlQuery As String, htFiltros As Hashtable

        sqlSelect = "select fe.idfactura2 as factura,fe.guia_aerea as guia,nombreArchivo,'descargarArchivosRecepcion.aspx?file='+"
        sqlSelect += " nombreArchivo as url,fechaRegistro from ArchivoRecepcionFactura ar with(nolock) left join"
        sqlSelect += "  facturas_externas fe with(nolock) on ar.idFactura=fe.idfactura where tipoFactura='fe' "
        sqlQuery = "union "
        sqlQuery += "select f.factura,f.guia,nombreArchivo,'descargarArchivosRecepcion.aspx?file='+nombreArchivo as url,"
        sqlQuery += " fechaRegistro from ArchivoRecepcionFactura ar with(nolock) left join Factura f on"
        sqlQuery += " ar.idFactura=f.idFactura where tipoFactura='f' "

        Try
            htFiltros = getFiltrosAplicados()
            If htFiltros("factura").ToString <> "" Then
                sqlSelect += " and fe.idfactura2 like '%'+@factura+'%' "
                sqlQuery += " and f.factura like '%'+@factura+'%' "
            End If
            If htFiltros("guia").ToString <> "" Then
                sqlSelect += " and fe.guia_aerea like '%'+@guia+'%' "
                sqlQuery += " and f.guia like '%'+@guia+'%' "
            End If
            If htFiltros("fechaInicial").ToString <> "" Then
                sqlSelect += " and convert(varchar,fe.fecha,112) between @fechaInicial and @fechaFinal "
                sqlQuery += " and convert(varchar,f.fechaLlegada,112) between @fechaInicial and @fechaFinal "
            End If
            sqlSelect += sqlQuery & " order by guia,factura "
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            With sqlComando.Parameters
                If htFiltros("factura").ToString <> "" Then .Add("@factura", SqlDbType.VarChar).Value = htFiltros("factura")
                If htFiltros("guia").ToString <> "" Then .Add("@guia", SqlDbType.VarChar).Value = htFiltros("guia")
                If htFiltros("fechaInicial").ToString <> "" Then
                    .Add("@fechaInicial", SqlDbType.VarChar).Value = String.Format("{0:yyyyMMdd}", CDate(htFiltros("fechaInicial")))
                    .Add("@fechaFinal", SqlDbType.VarChar).Value = String.Format("{0:yyyyMMdd}", CDate(htFiltros("fechaFinal")))
                End If
            End With
            sqlAdaptador.Fill(dtDatos)
            If dtDatos.Rows.Count > 0 Then
                dgDatos.DataSource = dtDatos
                dgDatos.DataBind()
            Else
                lblError.Text = "No se encontraron registros con las características solicitadas.<br><br>"
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de obtener los datos del Reporte. " & ex.Message & "<br><br>"
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Function getFiltrosAplicados() As Hashtable
        Dim htFiltros As Hashtable
        Try
            If Not Session("htFiltrosVerArchivosRecepcion") Is Nothing Then
                htFiltros = CType(Session("htFiltrosVerArchivosRecepcion"), Hashtable)
            Else
                htFiltros = New Hashtable
                With htFiltros
                    .Add("factura", "")
                    .Add("guia", "")
                    .Add("fechaInicial", "")
                    .Add("fechaFinal", "")
                End With
            End If
            Return htFiltros
        Catch ex As Exception
            Throw New Exception("Impsible recuperar filtros aplicados. " & ex.Message)
        End Try
    End Function

    Private Sub dgDatos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDatos.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            With CType(e.Item.Cells(3).Controls(0), HyperLink)
                .ForeColor = Color.Blue
            End With
        End If
    End Sub

End Class
