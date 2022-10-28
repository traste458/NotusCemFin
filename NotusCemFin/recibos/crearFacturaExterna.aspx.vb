Imports System.Data.SqlClient
Imports System.IO

Partial Class crearFacturaExterna
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
                    'txtFactura.Text = factura
                    getProveedor()
                    getProductos(0)
                    getBodegas(sqlConexion)
                    getEstadosRecepcionFactura()
                    hTipoRecepcion.Value = getTipoRecepcion()
                    If hTipoRecepcion.Value.ToUpper.IndexOf("TRASLADO") > -1 Then
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

        sqlSelect = "select idproducto,producto from productos with(nolock) where estado in (1,2) and idproducto in (select"
        sqlSelect += " idProducto from DetalleProductoTipoProducto where idTipoProducto=@idTipoProducto) "
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

    Private Sub getBodegas(ByVal sqlConexion As SqlConnection)
        Dim sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtBodegas As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idbodega,bodega from bodegas with(nolock) where idestado=1 order by bodega"

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
        Dim sqlInsert, sqlInsertIA, sqlQuery As String, tecnologia, idFactura As Integer
        Dim sqlTrans As SqlTransaction, sqlInsertFile, sqlSelect As String, existeArchivo As Boolean

        sqlInsert = "insert into facturas_externas(idfactura2,fecha,idproducto,unidades_caja,cajas_estiva,estado,"
        sqlInsert += "cant_region1,cant_region2,cant_region3,guia_aerea,idtipo,idTipoProducto,ordenCompra,numeroPalets,"
        sqlInsert += "cantidadEsperada,cantidadAprox,peso,idTipoRecepcion,idProveedor,idEstadoRecepcion,idBodega,observacion,"
        sqlInsert += "idUsuarioModifica) values(@factura,getdate(),@idProducto,@unidadesCaja,@cajasEstiva,0,0,0,0,"
        sqlInsert += "@guia,@tecnologia,@idTipoProducto,@ordenCompra,@palets,@cantidad,@cantidad,@peso,@idTipoRecepcion,"
        sqlInsert += "@idProveedor,@idEstadoRecepcion,@idBodega,@observacion,@idUsuario);"
        sqlInsert += "select max(idfactura) from facturas_externas where idfactura2=@factura"

        'sqlInsertIA = "insert into InfoAdicionalFactura(idFactura,idTipoProducto,ordenCompra,numeroPalets,"
        'sqlInsertIA += "cantidadEsperada,cantidadAprox,peso,idTipoRecepcion,idProveedor,"
        'sqlInsertIA += "idEstadoRecepcion,idBodega,observacion,idUsuarioModifica) "
        'sqlInsertIA += "values (@idFactura,@idTipoProducto,@ordenCompra,@palets,@cantidad,@cantidad,@peso,"
        'sqlInsertIA += "@idTipoRecepcion,@idProveedor,@idEstadoRecepcion,@idBodega,@observacion,@idUsuario)"

        sqlQuery = "insert into OrigenTrasladoFactura values(@idFactura,@idOperadorLogistico,'fe',getdate(),@idUsuario)"

        sqlSelect = "select count(idArchivo) from ArchivoRecepcionFactura where "
        sqlSelect += "idFactura=@idFactura and nombreArchivo=@nombreArchivo and tipoFactura='fe'"

        sqlInsertFile = "insert into ArchivoRecepcionFactura values('fe',@idFactura,@nombreArchivo,getdate(),@idUsuario)"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlInsert)
            tecnologia = getTecnologia(sqlConexion, ddlProducto.SelectedValue)
            With sqlComando.Parameters
                '*****Parámetros Factura Externa*****'
                .Add("@factura", SqlDbType.VarChar).Value = txtFactura.Text
                .Add("@idProducto", SqlDbType.Int).Value = ddlProducto.SelectedValue
                .Add("@unidadesCaja", SqlDbType.Int).Value = txtUnidadesCaja.Text
                .Add("@cajasEstiva", SqlDbType.Int).Value = txtCajasPalet.Text
                .Add("@guia", SqlDbType.VarChar).Value = txtGuia.Text
                .Add("@tecnologia", SqlDbType.Int).Value = tecnologia

                .Add("@idTipoProducto", SqlDbType.Int).Value = idTipoProducto
                .Add("@ordenCompra", SqlDbType.VarChar).Value = txtOrdenCompra.Text
                .Add("@palets", SqlDbType.Int).Value = txtPalets.Text
                .Add("@cantidad", SqlDbType.Int).Value = txtCantidad.Text
                .Add("@peso", SqlDbType.Int).Value = txtPeso.Text
                .Add("@idTipoRecepcion", SqlDbType.Int).Value = idTipoRecepcion
                .Add("@idProveedor", SqlDbType.Int).Value = ddlProveedor.SelectedValue
                .Add("@idEstadoRecepcion", SqlDbType.Int).Value = ddlEstadoRecepcion.SelectedValue
                .Add("@idBodega", SqlDbType.Int).IsNullable = True
                .Item("@idBodega").Value = _
                  IIf(ddlBodega.SelectedValue <> 0, ddlBodega.SelectedValue, DBNull.Value)
                .Add("@observacion", SqlDbType.VarChar).IsNullable = True
                .Item("@observacion").Value = _
                  IIf(txtObservacion.Text <> "", txtObservacion.Text.Trim, DBNull.Value)
                .Add("@idUsuario", Session("usxp001"))
                .Add("@idFactura", SqlDbType.Int).Value = 0
            End With
            sqlTrans = sqlConexion.BeginTransaction
            sqlComando.Transaction = sqlTrans
            idFactura = sqlComando.ExecuteScalar()
            sqlComando.Parameters("@idFactura").Value = idFactura
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
            'sqlComando.CommandText = sqlInsertIA
            'sqlComando.ExecuteNonQuery()
            If hTipoRecepcion.Value.ToUpper.IndexOf("TRASLADO") > -1 Then
                sqlComando.Parameters.Add("@idOperadorLogistico", SqlDbType.Int).Value = ddlOrigenTraslado.SelectedValue
                sqlComando.CommandText = sqlQuery
                sqlComando.ExecuteNonQuery()
            End If
            sqlTrans.Commit()
            Response.Redirect("verResumenFacturaRecibida.aspx?idFactura=" & idFactura, True)
        Catch ex As Exception
            If Not sqlTrans Is Nothing Then sqlTrans.Rollback()
            Throw New Exception("Error al tratar de registrar datos. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Function getTecnologia(ByVal sqlConexion As SqlConnection, ByVal idProducto As Integer) As Integer
        Dim sqlComando As SqlCommand, sqlRead As SqlDataReader
        Dim sqlSelect As String, resultado As Integer

        sqlSelect = "select idtipo from productos with(nolock) where idproducto=@idProducto"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idProducto", idProducto)
            If sqlConexion.State <> ConnectionState.Open Then sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                resultado = sqlRead.GetValue(0)
            End If
            sqlRead.Close()
            Return resultado
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener tecnología. " & ex.Message)
        End Try
    End Function

    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            If esFacturaValida() = True Then
                registrarDatosFactura()
            End If
        Catch ex As Exception
            lblError.Text = ex.Message & "<br><br>"
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

    Private Sub getOperadoresLogisticos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtOperadores As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idOperadorLogistico,nombre from OperadorLogistico where estado=1"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtOperadores)
            ddlOrigenTraslado.DataSource = dtOperadores
            ddlOrigenTraslado.DataTextField = "nombre"
            ddlOrigenTraslado.DataValueField = "idOperadorLogistico"
            ddlOrigenTraslado.DataBind()
            ddlOrigenTraslado.Items.Insert(0, New ListItem("Escoja un Origen", 0))
        Catch ex As Exception
            Throw New Exception("Error al trartar de obtener el listado de Orígenes de Traslado. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Function getTipoRecepcion() As String
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
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

    Private Sub subirArchivoAlServidor()
        Dim nombreArchivo As String = Server.MapPath("ArchivosRecepcion/") & Path.GetFileName(flArchivo.PostedFile.FileName)
        Try
            flArchivo.PostedFile.SaveAs(nombreArchivo)
        Catch ex As Exception
            Throw New Exception("Imposible subir el archivo al servidor. " & ex.Message)
        End Try
    End Sub

    Private Function esFacturaValida() As Boolean
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String
        Dim resultado As Boolean

        sqlSelect = "select idfactura2,(select estado from EstadoFactura where idEstado=fe.estado) as estado "
        sqlSelect += " from facturas_externas fe with(nolock) where idfactura2=@factura"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@factura", SqlDbType.VarChar).Value = txtFactura.Text
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                lblError.Text = "La Factura " & sqlRead.GetValue(0) & " ya se encuentra registrada en el sistema con estado: " & sqlRead.GetValue(1) & ".<br><br>"
                resultado = False
            Else
                resultado = True
            End If
            sqlRead.Close()
            Return resultado
        Catch ex As Exception
            lblError.Text = "Error al tratar de validar la existencia de la Factura a registrar. " & ex.Message & "<br><br>"
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Function

End Class
