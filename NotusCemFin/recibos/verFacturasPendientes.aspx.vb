Imports System.Data.SqlClient

Partial Class verFacturasPendientes
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

    Dim idTipoProducto, idTipoRecepcion As Integer, factura As String = ""

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me)
            lblError.Text = ""
            idTipoProducto = Request.QueryString("idTp")
            idTipoRecepcion = Request.QueryString("idTr")
            factura = Request.QueryString("factura")
            If Not Me.IsPostBack Then
                With hlCrearFactura
                    .NavigateUrl = .NavigateUrl.Replace("@tp", idTipoProducto).Replace("@f", factura)
                    .NavigateUrl = .NavigateUrl.Replace("@idTr", idTipoRecepcion)
                End With
                If Not Request.QueryString("esTras") Is Nothing Then
                    If Request.QueryString("idF") <> 0 Then
                        hlCrearFactura.Visible = False
                        hlRecibirTraslado.Visible = True
                        With hlRecibirTraslado
                            .NavigateUrl = .NavigateUrl.Replace("@f", factura).Replace("@tp", idTipoProducto)
                            .NavigateUrl = .NavigateUrl.Replace("@idF", Request.QueryString("idF"))
                            .NavigateUrl = .NavigateUrl.Replace("@idTr", idTipoRecepcion)
                        End With
                    End If
                End If
                getFacturasPendientes()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getFacturasPendientes()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtDatos As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select (select proveedor from proveedores with(nolock) where idproveedor=fe.idProveedor) as proveedor,"
        sqlSelect += " (select producto from productos with(nolock)  where idproducto=fe.idProducto)as producto,idfactura2"
        sqlSelect += " as factura,fecha,cantidadEsperada,'recibirFacturaPendiente.aspx?'+'idF='+convert(varchar,fe.idfactura)+"
        'sqlSelect += "  idTipoRecepcion=fe.idTipoRecepcion) like '%TRASLADO%' then "
        'sqlSelect += "  'recibirFacturaPendientePorTraslado.aspx?' else 'recibirFacturaPendiente.aspx?' end "
        'sqlSelect += " +'idF='+convert(varchar,fe.idfactura)+"
        sqlSelect += " '&factura='+@factura+'&idTp='+@idTp+'&idTr='+@idTr as url from facturas_externas fe with(nolock)  "
        sqlSelect += " where estado=0"

        If factura <> "" Then
            sqlSelect += " and idfactura2 like '%'+@factura+'%' "
        Else
            sqlSelect += " and fe.idTipoProducto=@idTipoProducto and fe.idTipoRecepcion=@idTipoRecepcion "
        End If
        sqlSelect += " order by proveedor,producto,fecha"
        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            With sqlComando.Parameters
                .Add("@idTipoProducto", SqlDbType.Int).Value = idTipoProducto
                .Add("@factura", SqlDbType.VarChar, 52).Value = factura
                .Add("@idTipoRecepcion", SqlDbType.Int).Value = idTipoRecepcion
                .Add("@idTp", SqlDbType.VarChar, 5).Value = idTipoProducto
                .Add("@idTr", SqlDbType.VarChar, 5).Value = idTipoRecepcion
            End With
            sqlAdaptador.Fill(dtDatos)
            dgFacturas.DataSource = dtDatos
            dgFacturas.Columns(0).FooterText = dtDatos.Rows.Count.ToString & " Registros Encontrados"
            dgFacturas.DataBind()
            MetodosComunes.mergeFooter(dgFacturas)
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener Facturas pendientes por recibir. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try

    End Sub
End Class
