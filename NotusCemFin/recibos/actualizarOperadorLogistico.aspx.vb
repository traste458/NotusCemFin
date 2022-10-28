Imports System.Data.SqlClient

Partial Class acctualizarOperadorLogistico
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

    Dim idOperadorLogistico As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me)
            lblError.Text = ""
            lblRes.Text = ""
            idOperadorLogistico = Request.QueryString("idOL")
            If Not Me.IsPostBack Then
                getCiudades()
                getInformacionActual()
            End If
        Catch ex As Exception
            lblError.Text = "Error al trarar de cargar página. " & ex.Message
        End Try
    End Sub

    Private Sub getCiudades()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtCiudades As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idciudad, upper(rtrim(ciudad))+' ('+upper(rtrim(departamento))+')' as ciudad from ciudades"
        sqlSelect += " with(nolock) where estado=1 order by ciudad"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtCiudades)
            ddlCiudad.DataSource = dtCiudades
            ddlCiudad.DataTextField = "ciudad"
            ddlCiudad.DataValueField = "idciudad"
            ddlCiudad.DataBind()
            'ddlCiudad.Items.Insert(0, New ListItem("Escoja una Ciudad", 0))
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el listado de Ciudades. " & ex.Message)
        Finally
            sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try

    End Sub

    Private Sub getInformacionActual()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String

        sqlSelect = "select nombre,identificacion,direccion,telefonos,email,estado,idCiudad "
        sqlSelect += " from OperadorLogistico where idOperadorLogistico=@idOperador "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idOperador", SqlDbType.Int).Value = idOperadorLogistico
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                txtNombre.Text = sqlRead.Item("nombre").ToString
                txtIdentificacion.Text = sqlRead.Item("identificacion").ToString
                txtDireccion.Text = sqlRead.Item("direccion").ToString
                txtTelefonos.Text = sqlRead.Item("telefonos").ToString
                txtEmail.Text = sqlRead.Item("email").ToString
                With ddlEstado
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(sqlRead.Item("estado")))
                End With
                With ddlCiudad
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(sqlRead.Item("idCiudad")))
                End With
            Else
                lblError.Text = "Imposible determinar la Información Actual del Operador Logístico.<br><br>"
            End If
            sqlRead.Close()
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener la información actual. " & ex.Message)
        Finally
            sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub actualizarDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlUpdate, sqlSelect As String, existe As Boolean

        sqlUpdate = "update OperadorLogistico set nombre=@nombre,identificacion=@identificacion,direccion=@direccion,"
        sqlUpdate += " telefonos=@telefonos,email=@email,estado=@estado,idCiudad=@idCiudad where idOperadorLogistico=@idOperador"
        
        sqlSelect = "select count(idOperadorLogistico)as total from OperadorLogistico where nombre=@nombre and"
        sqlSelect += " idCiudad=@idCiudad and idOperadorLogistico<>@idOperador "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idOperador", SqlDbType.Int).Value = idOperadorLogistico
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
            sqlComando.Parameters.Add("@estado", SqlDbType.Int).Value = ddlEstado.SelectedValue
            sqlConexion.Open()
            existe = CBool(sqlComando.ExecuteScalar)
            If Not existe Then
                sqlComando.CommandText = sqlUpdate
                sqlComando.ExecuteNonQuery()
                lblRes.Text = "Los datos se actualizaron satisfactoriamente.<br><br> "
            Else
                lblError.Text = "Ya existe en la Base de Datos un Operador Logístico con el mismo nombre del que está tratando de registrar.<br><br>"
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de actualizar los datos. " & ex.Message & "<br><br>"
        Finally
            sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub btnActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActualizar.Click
        actualizarDatos()
    End Sub
End Class
