Imports System.Data.SqlClient

Partial Class buscarOperadorLogistico
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
                getOperadoresRegistrados()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getOperadoresRegistrados()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtDatos As New DataTable
        Dim sqlSelect, nombre, identificacion As String, idCiudad As Integer

        Try
            sqlSelect = "select nombre,identificacion,direccion,telefonos,email,(select upper(rtrim(ciudad)) "
            sqlSelect += " from ciudades with(nolock) where idciudad=ol.idCiudad) as ciudad,fechaCreacion, "
            sqlSelect += " case when estado=1 then 'ACTIVO' else 'INACTIVO' end as estado, "
            sqlSelect += " 'actualizarOperadorLogistico.aspx?idOL='+convert(varchar,idOperadorLogistico) as url "
            sqlSelect += " from OperadorLogistico ol with(nolock) where idOperadorLogistico is not null "
            aplicarFiltros(nombre, identificacion, idCiudad)
            If nombre <> "" Then sqlSelect += " and nombre like '%'+@nombre+'%' "
            If identificacion <> "" Then sqlSelect += " and identificacion like '%'+@identificacion+'%' "
            If idCiudad <> 0 Then sqlSelect += " and idCiudad=@idCiudad "
            sqlSelect += " order by nombre "
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            If nombre <> "" Then sqlComando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombre
            If identificacion <> "" Then sqlComando.Parameters.Add("@identificacion", SqlDbType.VarChar).Value = identificacion
            If idCiudad <> 0 Then sqlComando.Parameters.Add("@idCiudad", SqlDbType.Int).Value = idCiudad
            sqlAdaptador.Fill(dtDatos)
            If dtDatos.Rows.Count > 0 Then
                dgDatos.DataSource = dtDatos
                dgDatos.DataBind()
            Else
                pnlDatos.Visible = False
                lblError.Text = "No se encontraron datos con las características solicitadas.<br><br>"
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de obtener el listado de Operadores Lógisticos registrados. " & ex.Message & "<br><br>"
        Finally
            If Not sqlComando Is Nothing Then sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub aplicarFiltros(ByRef nombre As String, ByRef identificacion As String, ByRef idCiudad As Integer)
        Dim filtros() As String
        Try
            filtros = CStr(Session("filtrosBuscarOL")).Split(";")
            If filtros.GetUpperBound(0) = 2 Then
                nombre = filtros(0).ToUpper
                identificacion = filtros(1).ToUpper
                idCiudad = CInt(filtros(2))
            Else
                Throw New Exception("Imposible recuperar filtros.")
            End If
        Catch ex As Exception
            Throw New Exception("Error al tratar de aplicar filtros. " & ex.Message)
        End Try
    End Sub

    Private Sub dgDatos_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgDatos.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or _
                   e.Item.ItemType = ListItemType.AlternatingItem Then

            If CType(e.Item.Cells(7).Controls(0), HyperLink).Text.StartsWith("A") Then
                CType(e.Item.Cells(7).Controls(0), HyperLink).ForeColor = Color.Blue
            Else
                CType(e.Item.Cells(7).Controls(0), HyperLink).ForeColor = Color.Red
            End If
        End If
    End Sub
End Class
