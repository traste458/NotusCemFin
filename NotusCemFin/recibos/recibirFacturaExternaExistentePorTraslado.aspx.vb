Imports System.Data.SqlClient

Partial Class recibirFacturaExternaExistentePorTraslado
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

    Dim idFactura, idTipoRecepcion As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me)
            lblError.Text = ""
            idFactura = Request.QueryString("idF")
            idTipoRecepcion = Request.QueryString("idTr")
            If Not Me.IsPostBack Then
                lblFactura.Text = Request.QueryString("f")
                With hlRegresar
                    .NavigateUrl = .NavigateUrl.Replace("@f", lblFactura.Text.Trim)
                    .NavigateUrl = .NavigateUrl.Replace("@tp", Request.QueryString("idTp"))
                    .NavigateUrl = .NavigateUrl.Replace("@idTr", idTipoRecepcion)
                    .NavigateUrl += "&esTras=true&idF=" & idFactura.ToString
                End With
                getInfoFromFacturaExterna()
                getOperadoresLogisticos()
                getBodegas()
                getEstadosRecepcion()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message
        End Try
    End Sub

    Private Sub getInfoFromFacturaExterna()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String

        sqlSelect = "select (select proveedor from proveedores with(nolock) where idproveedor=fe.idProveedor) as proveedor,"
        sqlSelect += " (select producto from productos with(nolock) where idproducto=fe.idproducto) as producto,guia_aerea"
        sqlSelect += " as guia, ordenCompra,fecha,fechaSalida,(select estadoRecepcion from EstadoRecepcionFactura with(nolock)"
        sqlSelect += " where idEstadoRecepcion=fe.idEstadoRecepcion) as estadoRecepcion,cantidadAprox,cantidad_pedida,cantidad"
        sqlSelect += " from facturas_externas fe with(nolock) where fe.idfactura==@idFactura"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idFactura", SqlDbType.Int).Value = idFactura
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                lblProveedor.Text = sqlRead("proveedor").ToString
                lblProducto.Text = sqlRead("producto").ToString
                lblGuia.Text = sqlRead("guia").ToString
                lblOrdenCompra.Text = sqlRead("ordenCompra").ToString
                lblFecha.Text = String.Format("{0:dd-MMM-yyyy hh:mm tt}", CDate(sqlRead("fecha")))
                If Not IsDBNull(sqlRead("fechaSalida")) Then
                    lblFechaSalida.Text = String.Format("{0:dd-MMM-yyyy hh:mm tt}", CDate(sqlRead("fechaSalida")))
                End If
                lblEstadoRecepcion.Text = sqlRead("estadoRecepcion").ToString
                lblCantidad.Text = sqlRead("cantidadAprox").ToString
                lblCantidadPedida.Text = sqlRead("cantidad_pedida").ToString
                lblCantidadProcesada.Text = sqlRead("cantidad").ToString
            Else
                lblError.Text = "Imposible obtener la información actual de la Factura.<br><br>"
            End If
            sqlRead.Close()
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener Información General de la Factura. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
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

    Private Sub getBodegas()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
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

    Private Sub getEstadosRecepcion()
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
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub actualizarInfoFacturaExterna()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlTransaccion As SqlTransaction
        Dim sqlUpdate, sqlQuery, sqlInsert, sqlAct As String

        sqlUpdate = "update facturas_externas set estado=0,fecha=getdate() where idfactura=@idFactura "

        sqlQuery = "update InfoAdicionalFactura set idTipoRecepcion=@idTipoRecepcion,"
        sqlQuery += "numeroPalets=@numeroPalets,cantidadAprox=cantidadAprox+@cantidadAprox,"
        sqlQuery += "peso=@peso,idBodega=@idBodega,observacion=@observacion,fechaSalida=null,"
        sqlQuery += " idEstadoRecepcion=@idEstadoRecepcion,idUsuarioModifica=@idUsuario where idFactura=@idFactura"

        sqlInsert = "insert into OrigenTrasladoFactura values(@idFactura,"
        sqlInsert += "@idOperadorLogistico,'fe',getdate(),@idUsuario)"


        sqlAct = "update LogFacturasExternas set idUsuarioCambia=@idUsuario "
        sqlAct += " where idLogFactura=(select max(idLogFactura) from LogFacturasExternas "
        sqlAct += " where idFactura=@idFactura)"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlUpdate)
            With sqlComando.Parameters
                .Add("@idFactura", SqlDbType.Int).Value = idFactura
                .Add("@idUsuario", SqlDbType.Int).Value = Session("usxp001")
                .Add("@idOperadorLogistico", SqlDbType.Int).Value = ddlOrigenTraslado.SelectedValue
                .Add("@idTipoRecepcion", SqlDbType.Int).Value = idTipoRecepcion
                .Add("@numeroPalets", SqlDbType.Int).Value = txtPalets.Text
                .Add("@cantidadAprox", SqlDbType.Int).Value = txtCantidad.Text
                .Add("@peso", SqlDbType.Int).Value = txtPeso.Text
                .Add("@idBodega", SqlDbType.Int).IsNullable = True
                If ddlBodega.SelectedValue <> "0" Then
                    .Item("@idBodega").Value = ddlBodega.SelectedValue
                Else
                    .Item("@idBodega").Value = DBNull.Value
                End If
                .Add("@idEstadoRecepcion", SqlDbType.Int).Value = ddlEstadoRecepcion.SelectedValue
                .Add("@observacion", SqlDbType.VarChar).IsNullable = True
                If txtObservacion.Text <> "" Then
                    .Item("@observacion").Value = txtObservacion.Text
                Else
                    .Item("@observacion").Value = DBNull.Value
                End If
            End With

            sqlConexion.Open()
            sqlTransaccion = sqlConexion.BeginTransaction
            sqlComando.Transaction = sqlTransaccion
            sqlComando.ExecuteNonQuery()
            sqlComando.CommandText = sqlAct
            sqlComando.ExecuteNonQuery()
            sqlComando.CommandText = sqlQuery
            sqlComando.ExecuteNonQuery()
            sqlComando.CommandText = sqlInsert
            sqlComando.ExecuteNonQuery()
            sqlTransaccion.Commit()
        Catch ex As Exception
            If Not sqlTransaccion Is Nothing Then sqlTransaccion.Rollback()
            Throw New Exception("Error al tratar de registrar la nueva información de la Factura. " & ex.Message)
        Finally
            sqlTransaccion.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        Try
            actualizarInfoFacturaExterna()
            Response.Redirect("verResumenFacturaRecibida.aspx?idFactura=" & idFactura, True)
        Catch ex As Exception
            lblError.Text = ex.Message
        End Try
    End Sub

End Class
