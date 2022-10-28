Imports System.Data.SqlClient
Imports System.IO

Partial Class crearNuevaFactura
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

    Dim idTipoProducto, idTipoRecepcion As Integer, factura As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me, Anthem.Manager.IsCallBack)
            lblError.Text = ""
            lblRes.Text = ""
            factura = Request.QueryString("factura")
            idTipoProducto = Request.QueryString("idTp")
            idTipoRecepcion = Request.QueryString("idTr")
            If Not Me.IsPostBack And Not Anthem.Manager.IsCallBack Then
                Dim sqlConexion As SqlConnection
                Try
                    getProveedor()
                    getProductos(0)
                    getBodegas(sqlConexion)
                    getEstadosRecepcionFactura()
                    If idTipoRecepcion = 3 Then
                        getOperadoresLogisticos()
                        pnlTraslado.Visible = True
                    End If
                Catch ex As Exception
                    lblError.Text = ex.Message & "<br><br>"
                Finally
                    MetodosComunes.liberarConexion(sqlConexion)
                End Try
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message
        End Try
    End Sub

    Private Sub getProveedor()
        Try
            With ddlProveedor
                .DataSource = MetodosComunes.getAllProveedores
                .DataTextField = "proveedor"
                .DataValueField = "idproveedor"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Proveedor", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener Proveedores. " & ex.Message)
        End Try
    End Sub

    Private Sub getProductos(ByVal idProveedor As Integer)
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtProducto As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idproducto,producto from productos where estado in (1,2) and idproducto in "
        sqlSelect += " (select idProducto from DetalleProductoTipoProducto with(nolock) where idTipoProducto=@idTipoProducto) "
        If idProveedor <> 0 Then sqlSelect += " and idproveedor=@idProveedor "
        sqlSelect += " order by producto "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlComando.Parameters.Add("@idTipoProducto", SqlDbType.Int).Value = idTipoProducto
            sqlComando.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor
            sqlAdaptador.Fill(dtProducto)
            With ddlProducto
                .DataSource = dtProducto
                .DataTextField = "producto"
                .DataValueField = "idproducto"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Producto", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el listado de Productos. " & ex.Message)
        End Try
    End Sub

    Private Function getTipoRecepcion(ByVal sqlConexion As SqlConnection) As String
        Dim sqlComando As SqlCommand
        Dim sqlSelect, tipoRecepcion As String

        sqlSelect = "select tipoRecepcion from TipoRecepcion where idTipoRecepcion=@idTipoRecepcion"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idTipoRecepcion", SqlDbType.Int).Value = idTipoRecepcion
            sqlConexion.Open()
            tipoRecepcion = sqlComando.ExecuteScalar
            Return tipoRecepcion
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el Tipo de Recepción. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Function

    Private Sub getBodegas(ByVal sqlConexion As SqlConnection)
        Dim sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtBodegas As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idbodega,bodega from bodegas where idestado=1 order by bodega"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtBodegas)
            With ddlBodega
                .DataSource = dtBodegas
                .DataTextField = "bodega"
                .DataValueField = "idbodega"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja una Bodega", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de ontener el listado de Bodegas. " & ex.Message)
        End Try
    End Sub

    Private Sub getEstadosRecepcionFactura()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtEstados As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idEstadoRecepcion,estadoRecepcion from EstadoRecepcionFactura order by idEstadoRecepcion"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtEstados)
            With ddlEstadoRecepcion
                .DataSource = dtEstados
                .DataTextField = "estadoRecepcion"
                .DataValueField = "idEstadoRecepcion"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Estado", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el listado de Estados de Recepción. " & ex.Message)
        Finally
            If Not dtEstados Is Nothing Then dtEstados.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub registrarDatosFactura()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlInsert, sqlQuery As String, tecnologia, idFactura As Integer
        Dim sqlSelect, sqlInsertFile As String, existeArchivo As Boolean
        Dim sqlTrans As SqlTransaction

        sqlInsert = "insert into Factura (factura,ordenCompra,idTipoProducto,idProducto,idProveedor,guia,idTipoRecepcion,"
        sqlInsert += "cantidadEsperada,numeroPalets,cantidadAprox,idBodega,peso,fechaLlegada,estado,idEstadoRecepcion,"
        sqlInsert += "observacion,idUsuarioModifica) values (@factura,@ordenCompra,@idTipoProducto,@idProducto,@idProveedor,"
        sqlInsert += "@guia,@idTipoRecepcion,@cantidadEsperada,@numeroPalets,@cantidadAprox,@idBodega,@peso,getdate(),"
        sqlInsert += "1,@idEstadoRecepcion,@observacion,@idUsuario);select max(idFactura)as idFactura from Factura  "

        sqlQuery = "insert into OrigenTrasladoFactura values(@idFactura,@idOperadorLogistico,'f',getdate(),@idUsuario)"

        sqlSelect = "select count(idArchivo) from ArchivoRecepcionFactura where "
        sqlSelect += "idFactura=@idFactura and nombreArchivo=@nombreArchivo and tipoFactura='f'"

        sqlInsertFile = "insert into ArchivoRecepcionFactura values('f',@idFactura,@nombreArchivo,getdate(),@idUsuario)"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlInsert)
            With sqlComando.Parameters
                .Add("@factura", SqlDbType.VarChar).Value = txtFactura.Text
                .Add("@ordenCompra", SqlDbType.VarChar).IsNullable = True
                .Item("@ordenCompra").Value = IIf(txtOrdenCompra.Text <> "", txtOrdenCompra.Text, DBNull.Value)
                .Add("@idTipoProducto", SqlDbType.Int).Value = idTipoProducto
                .Add("@idProducto", SqlDbType.Int).Value = ddlProducto.SelectedValue
                .Add("@idProveedor", SqlDbType.Int).Value = ddlProveedor.SelectedValue
                .Add("@guia", SqlDbType.VarChar).Value = txtGuia.Text
                .Add("@idTipoRecepcion", SqlDbType.Int).Value = idTipoRecepcion
                .Add("@cantidadEsperada", SqlDbType.Int).Value = txtCantidad.Text
                .Add("@numeroPalets", SqlDbType.Int).Value = txtPalets.Text
                .Add("@cantidadAprox", SqlDbType.Int).Value = txtCantidad.Text
                .Add("@idBodega", SqlDbType.Int).IsNullable = True
                .Item("@idBodega").Value = IIf(ddlBodega.SelectedValue <> 0, ddlBodega.SelectedValue, DBNull.Value)
                .Add("@peso", SqlDbType.Int).Value = txtPeso.Text
                .Add("@idEstadoRecepcion", SqlDbType.Int).Value = ddlEstadoRecepcion.SelectedValue
                .Add("@observacion", SqlDbType.VarChar).IsNullable = True
                .Item("@observacion").Value = IIf(txtObservacion.Text <> "", txtObservacion.Text, DBNull.Value)
                .Add("@idUsuario", Session("usxp001"))
            End With
            sqlConexion.Open()
            sqlTrans = sqlConexion.BeginTransaction
            sqlComando.Transaction = sqlTrans
            idFactura = sqlComando.ExecuteScalar()
            sqlComando.Parameters.Add("@idFactura", SqlDbType.Int).Value = idFactura
            If flArchivo.Value <> "" Then
                subirArchivoAlServidor()
                sqlComando.Parameters.Add("@nombreArchivo", SqlDbType.VarChar).Value = Path.GetFileName(flArchivo.PostedFile.FileName)
                sqlComando.CommandText = sqlSelect
                existeArchivo = CBool(sqlComando.ExecuteScalar)
                If existeArchivo = False Then
                    sqlComando.CommandText = sqlInsertFile
                    sqlComando.ExecuteNonQuery()
                End If
            End If
            If idTipoRecepcion = 3 Then
                sqlComando.Parameters.Add("@idOperadorLogistico", SqlDbType.Int).Value = ddlOrigenTraslado.SelectedValue
                sqlComando.CommandText = sqlQuery
                sqlComando.ExecuteNonQuery()
            End If
            sqlTrans.Commit()
            Response.Redirect("verResumenFactura.aspx?idFactura=" & idFactura, True)
        Catch ex As Exception
            If Not sqlTrans Is Nothing Then sqlTrans.Rollback()
            Throw New Exception("Error al tratar de registrar datos. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Function existeFactura() As Boolean
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlQuery As String, resultado As Boolean
        Try
            sqlQuery = "select count(0) as numFact from Factura with(nolock) where factura=@factura and idProveedor=@idProveedor"
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlQuery)
            With sqlComando.Parameters
                .Add("@factura", SqlDbType.VarChar, 52).Value = txtFactura.Text.Trim
                .Add("@idProveedor", SqlDbType.Int).Value = ddlProveedor.SelectedValue
            End With
            sqlConexion.Open()
            resultado = CBool(sqlComando.ExecuteScalar)
            Return resultado
        Catch ex As Exception
            Throw New Exception("Imposible validar si la Factura ya existe en el sistema. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Function

    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            If Not existeFactura() Then
                registrarDatosFactura()
            Else
                lblError.Text = "Ya existe en el sistema una Factura con la misma nomenclatura, asociada al Proveedor escogido.<br><br>"
            End If
        Catch ex As Exception
            lblError.Text = ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getOperadoresLogisticos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtOperadores As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idOperadorLogistico,nombre from OperadorLogistico where estado=1"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtOperadores)
            With ddlOrigenTraslado
                .DataSource = dtOperadores
                .DataTextField = "nombre"
                .DataValueField = "idOperadorLogistico"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Origen", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al trartar de obtener el listado de Orígenes de Traslado. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub subirArchivoAlServidor()
        Dim nombreArchivo As String = Server.MapPath("ArchivosRecepcion/") & Path.GetFileName(flArchivo.PostedFile.FileName)
        Try
            flArchivo.PostedFile.SaveAs(nombreArchivo)
        Catch ex As Exception
            Throw New Exception("Imposible subir el archivo al servidor. " & ex.Message)
        End Try
    End Sub

    Private Sub ddlProveedor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddlProveedor.SelectedIndexChanged
        If Session.Count > 0 Then
            Try
                getProductos(ddlProveedor.SelectedValue)
            Catch ex As Exception
                lblError.Text = ex.Message & "<br><br>"
            End Try
        End If
    End Sub
End Class

