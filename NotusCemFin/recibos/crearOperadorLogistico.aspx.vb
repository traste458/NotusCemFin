Imports System.Data.SqlClient

Partial Class crearOperadorLogistico
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
            lblRes.Text = ""
            If Not Me.IsPostBack Then
                hlRegresar.NavigateUrl = MetodosComunes.getUrlFrameBack(Me)
                getCiudades()
                getOperadoresRegistrados()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
        End Try

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
            Throw New Exception("Error al tratar de obtener el listado de Ciudades. " & ex.Message)
        Finally
            sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try

    End Sub

    Private Sub getOperadoresRegistrados()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtDatos As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select nombre,identificacion,direccion,telefonos,email,(select upper(rtrim(ciudad)) "
        sqlSelect += " from ciudades where idciudad=ol.idCiudad) as ciudad,fechaCreacion "
        sqlSelect += "from OperadorLogistico ol where estado=1 order by nombre"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtDatos)
            dgDatos.DataSource = dtDatos
            dgDatos.DataBind()
        Catch ex As Exception
            lblError.Text = "Error al tratar de obtener el listado de Operadores Lógisticos registrados. " & ex.Message & "<br><br>"
        Finally
            sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub registraDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlInsert, sqlSelect As String, existe As Boolean

        sqlInsert = "insert into OperadorLogistico values(@nombre,@identificacion,@direccion,"
        sqlInsert += "@telefonos,@email,1,@idCiudad,getdate(),@idUsuario)"

        sqlSelect = "select count(idOperadorLogistico)as total from "
        sqlSelect += " OperadorLogistico where nombre=@nombre and idCiudad=@idCiudad "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = txtNombre.Text.ToUpper
            sqlComando.Parameters.Add("@identificacion", SqlDbType.VarChar).IsNullable = True
            sqlComando.Parameters("@identificacion").Value = IIf(txtIdentificacion.Text <> "", txtIdentificacion.Text.ToUpper, DBNull.Value)
            sqlComando.Parameters.Add("@direccion", SqlDbType.VarChar).IsNullable = True
            sqlComando.Parameters("@direccion").Value = IIf(txtDireccion.Text <> "", txtDireccion.Text.ToUpper, DBNull.Value)
            sqlComando.Parameters.Add("@telefonos", SqlDbType.VarChar).IsNullable = True
            sqlComando.Parameters("@telefonos").Value = IIf(txtTelefonos.Text <> "", txtTelefonos.Text, DBNull.Value)
            sqlComando.Parameters.Add("@email", SqlDbType.VarChar).IsNullable = True
            sqlComando.Parameters("@email").Value = IIf(txtEmail.Text <> "", txtEmail.Text, DBNull.Value)
            sqlComando.Parameters.Add("@idCiudad", SqlDbType.Int).Value = ddlCiudad.SelectedValue
            sqlComando.Parameters.Add("@idUsuario", SqlDbType.Int).Value = Session("usxp001")
            sqlConexion.Open()
            existe = CBool(sqlComando.ExecuteScalar)
            If Not existe Then
                sqlComando.CommandText = sqlInsert
                sqlComando.ExecuteNonQuery()
                limpiarControles()
                lblRes.Text = "Los datos se registraron satisfactoriamente.<br><br> "
                getOperadoresRegistrados()
            Else
                lblError.Text = "Ya existe en la Base de Datos un Operador Logístico con el mismo nombre del que está tratando de registrar.<br><br>"
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de registrar los datos. " & ex.Message & "<br><br>"
        Finally
            sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        registraDatos()
    End Sub


    Private Sub limpiarControles()
        txtNombre.Text = ""
        txtIdentificacion.Text = ""
        txtDireccion.Text = ""
        txtEmail.Text = ""
        txtTelefonos.Text = ""
        ddlCiudad.ClearSelection()
    End Sub

    Private Sub dgDatos_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgDatos.PageIndexChanged
        getOperadoresRegistrados()
        dgDatos.CurrentPageIndex = e.NewPageIndex
    End Sub
End Class
