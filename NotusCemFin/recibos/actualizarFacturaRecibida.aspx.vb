Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Web.Mail



Partial Class actualizarFacturaRecibida
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

    Dim idFactura As String, isFrom As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me, Anthem.Manager.IsCallBack)
            Server.ScriptTimeout = 600
            idFactura = Request.QueryString("idFactura")
            isFrom = Request.QueryString("isFrom")
            If Not Me.IsPostBack And Not Anthem.Manager.IsCallBack Then
                If isFrom = "fe" Then
                    pnlDatosCajas.Visible = True
                Else
                    pnlDatosCajas.Visible = False
                End If
                If Not Me.IsPostBack Then
                    getIdTipoProducto()
                    getProveedor()
                    getProductos(0)
                    getTipoRecepcion()
                    getEstadosRecepcionFactura()
                    getBodegas()
                    getDestinosTraslado()
                    getDatosActuales()
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message
        End Try
    End Sub

    Private Sub getIdTipoProducto()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlSelect As String
        If isFrom = "fe" Then
            sqlSelect = "select idTipoProducto from facturas_externas with(nolock) where idfactura=@idFactura "
        Else
            sqlSelect = "select idTipoProducto from Factura f where idFactura=@idFactura "
        End If

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idFactura", SqlDbType.Int).Value = idFactura
            sqlConexion.Open()
            hIdTipoProducto.Value = sqlComando.ExecuteScalar
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el Tipo de Producto de la Factura. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
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

        sqlSelect = "select idproducto,producto from productos with(nolock) where estado in (1,2) and idproducto in (select "
        sqlSelect += " idProducto from DetalleProductoTipoProducto with(nolock) where idTipoProducto=@idTipoProducto)"
        If idProveedor <> 0 Then sqlSelect += " and idproveedor=@idProveedor "
        sqlSelect += " order by producto "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlComando.Parameters.Add("@idTipoProducto", SqlDbType.Int).Value = hIdTipoProducto.Value
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

    Private Sub getTipoRecepcion()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtTipoRecepcion As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idTipoRecepcion,tipoRecepcion from TipoRecepcion with(nolock) where estado=1"

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

    Private Sub getEstadosRecepcionFactura()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtEstados As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idEstadoRecepcion,estadoRecepcion from EstadoRecepcionFactura with(nolock) order by idEstadoRecepcion"

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

    Private Sub getBodegas()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
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

    Private Sub getDestinosTraslado()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtOperadores As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idOperadorLogistico,nombre from OperadorLogistico with(nolock) where estado=1"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtOperadores)
            With ddlDestinoTraslado
                .DataSource = dtOperadores
                .DataTextField = "nombre"
                .DataValueField = "idOperadorLogistico"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja un Destino", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al trartar de obtener el listado de Orígenes de Traslado. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub getDatosActuales()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String

        If isFrom = "fe" Then
            sqlSelect = "select idfactura2 as factura,isnull(ordenCompra,'') as ordenCompra,isnull(guia_aerea,'')as guia,(select"
            sqlSelect += " tipoProducto from TipoProducto with(nolock) where idTipoProducto=@idTipoProducto) as tipoProducto,"
            sqlSelect += " idProveedor,idProducto,idTipoRecepcion,numeroPalets,cantidadAprox,peso,idEstadoRecepcion,isnull(idBodega,0)"
            sqlSelect += " as idBodega,isnull(observacion,'') as observacion,fecha,isnull(convert(varchar,fechaSalida,103),'') as"
            sqlSelect += " fechaSalida,isnull((select max(idOperadorLogistico) from SalidaDeFactura with(nolock) where idFactura="
            sqlSelect += " fe.idfactura and fechaSalida=fe.fechaSalida),0) as idDestinoTraslado,unidades_caja,cajas_estiva from "
            sqlSelect += " facturas_externas fe with(nolock) where fe.idfactura=@idFactura"
        Else
            sqlSelect = "select factura,isnull(ordenCompra,'') as ordenCompra,isnull(guia,'') as guia,(select tipoProducto from"
            sqlSelect += " TipoProducto with(nolock) where idTipoProducto=@idTipoProducto)as tipoProducto,idProveedor,idProducto,"
            sqlSelect += " idTipoRecepcion,numeroPalets,cantidadAprox,peso,idEstadoRecepcion,isnull(idBodega,0) as idBodega,"
            sqlSelect += " isnull(observacion,'') as observacion,fechaLlegada,fechaSalida,isnull((select max(idOperadorLogistico)"
            sqlSelect += " from SalidaDeFactura with(nolock) where idFactura=f.idFactura and fechaSalida=f.fechaSalida),0) as"
            sqlSelect += " idDestinoTraslado from Factura f with(nolock) where idFactura=@idFactura "
        End If
        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idTipoProducto", SqlDbType.Int).Value = hIdTipoProducto.Value
            sqlComando.Parameters.Add("@idFactura", SqlDbType.Int).Value = idFactura
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                txtFactura.Text = sqlRead("factura").ToString
                txtOrdenCompra.Text = sqlRead("ordenCompra").ToString
                txtGuia.Text = sqlRead("guia").ToString
                lblTipoProducto.Text = sqlRead("tipoProducto").ToString
                With ddlProveedor
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(sqlRead("idProveedor").ToString))
                End With
                With ddlProducto
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(sqlRead("idProducto").ToString))
                End With
                With ddlTipoRecepcion
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(sqlRead("idTipoRecepcion").ToString))
                End With
                txtPalets.Text = sqlRead("numeroPalets").ToString
                txtCantidad.Text = sqlRead("cantidadAprox").ToString
                txtPeso.Text = CDbl(sqlRead("peso")).ToString.Replace(",", ".")
                With ddlEstadoRecepcion
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(sqlRead("idEstadoRecepcion").ToString))
                End With
                With ddlBodega
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(sqlRead("idBodega").ToString))
                End With
                txtObservacion.Text = sqlRead("observacion").ToString
                lblFechaRecepcion.Text = String.Format("{0:dd-MMM-yyyy hh:mm tt}", sqlRead("fecha"))
                fechaSalida.Value = sqlRead("fechaSalida").ToString
                hFechaSalidaActual.Value = sqlRead("fechaSalida").ToString
                With ddlDestinoTraslado
                    .SelectedIndex = .Items.IndexOf(.Items.FindByValue(sqlRead("idDestinoTraslado").ToString))
                End With
                If isFrom = "fe" Then
                    txtUnidadesCaja.Text = sqlRead("unidades_caja").ToString
                    txtCajasPalet.Text = sqlRead("cajas_estiva").ToString
                End If
            End If
            sqlRead.Close()
            If fechaSalida.Value.Trim = "" Then trDestinoTraslado.Style.Add("display", "none")
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos actuales de la Factura. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub actualizarDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlUpdate, sqlInsert As String, tecnologia As Integer
        Dim sqlTrans As SqlTransaction, sqlSelect, sqlInsertFile As String, existeArchivo As Boolean

        sqlSelect = "select count(idArchivo) from ArchivoRecepcionFactura with(nolock) where "
        sqlSelect += "idFactura=@idFactura and nombreArchivo=@nombreArchivo and tipoFactura=@tipoFactura"

        sqlInsertFile = "insert into ArchivoRecepcionFactura values(@tipoFactura,@idFactura,@nombreArchivo,getdate(),@idUsuario)"

        If isFrom = "fe" Then
            sqlUpdate = "update facturas_externas set idfactura2=@factura,guia_aerea=@guia,idproducto=@idproducto,"
            sqlUpdate += "unidades_caja=@unidades,cajas_estiva=@cajas,idtipo=@tecnologia where idfactura=@idFactura,"
            If fechaSalida.Value.Trim <> "" And fechaSalida.Value <> hFechaSalidaActual.Value Then sqlUpdate += " estado=6,"
            sqlUpdate += "ordenCompra=@ordenCompra,idProveedor=@idProveedor,idTipoRecepcion=@idTipoRecepcion,"
            sqlUpdate += " numeroPalets=@numeroPalets,cantidadAprox=@cantidadAprox,peso=@peso,idEstadoRecepcion="
            sqlUpdate += " @idEstadoRecepcion,idBodega=@idBodega,fechaSalida=@fechaSalida,observacion=@observacion,"
            sqlUpdate += " idUsuarioModifica=@idUsuario where idFactura=@idFactura"
        Else
            sqlUpdate = "update Factura set factura=@factura,ordenCompra=@ordenCompra,guia=@guia,idProducto=@idproducto,"
            sqlUpdate += " idProveedor=@idProveedor,idTipoRecepcion=@idTipoRecepcion,numeroPalets=@numeroPalets,"
            sqlUpdate += " cantidadAprox=@cantidadAprox,peso=@peso,idEstadoRecepcion=@idEstadoRecepcion,idBodega=@idBodega,"
            sqlUpdate += " fechaSalida=@fechaSalida,observacion=@observacion,"
            If fechaSalida.Value.Trim <> "" And fechaSalida.Value <> hFechaSalidaActual.Value Then sqlUpdate += " estado=6,"
            sqlUpdate += " idUsuarioModifica=@idUsuario where idFactura=@idFactura"
        End If

        sqlInsert = "insert into SalidaDeFactura values(@idFactura,@idOperador,@fechaSalida,'fe',getdate(),@idUsuario)"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlUpdate)
            tecnologia = getTecnologia(ddlProducto.SelectedValue)
            With sqlComando.Parameters
                .Add("@idFactura", SqlDbType.Int).Value = idFactura
                .Add("@factura", SqlDbType.VarChar).Value = txtFactura.Text
                .Add("@ordenCompra", SqlDbType.VarChar).Value = txtOrdenCompra.Text
                .Add("@guia", SqlDbType.VarChar).Value = txtGuia.Text
                .Add("@idProducto", SqlDbType.Int).Value = ddlProducto.SelectedValue
                .Add("@idProveedor", SqlDbType.Int).Value = ddlProveedor.SelectedValue
                .Add("@idTipoRecepcion", SqlDbType.Int).Value = ddlTipoRecepcion.SelectedValue
                .Add("@numeroPalets", SqlDbType.Int).Value = txtPalets.Text
                .Add("@cantidadAprox", SqlDbType.Int).Value = txtCantidad.Text
                .Add("@peso", SqlDbType.Real).Value = txtPeso.Text
                .Add("@idEstadoRecepcion", SqlDbType.Int).Value = ddlEstadoRecepcion.SelectedValue
                .Add("@idBodega", SqlDbType.Int).Value = ddlBodega.SelectedValue
                .Add("@observacion", SqlDbType.VarChar).Value = txtObservacion.Text
                .Add("@fechaSalida", SqlDbType.SmallDateTime).IsNullable = True
                If fechaSalida.Value <> "" Then
                    .Item("@fechaSalida").Value = CDate(fechaSalida.Value)
                Else
                    .Item("@fechaSalida").Value = DBNull.Value
                End If
                .Add("@idUsuario", SqlDbType.Int).Value = Session("usxp001")
                If isFrom = "fe" Then
                    .Add("@tecnologia", SqlDbType.Int).Value = tecnologia
                    .Add("@unidades", SqlDbType.Int).Value = txtUnidadesCaja.Text
                    .Add("@cajas", SqlDbType.Int).Value = txtCajasPalet.Text
                End If
            End With
            sqlConexion.Open()
            sqlTrans = sqlConexion.BeginTransaction
            sqlComando.Transaction = sqlTrans
            sqlComando.CommandTimeout = 300
            sqlComando.ExecuteNonQuery()
            If flArchivo.Value <> "" Then
                subirArchivoAlServidor()
                sqlComando.Parameters.Add("@nombreArchivo", SqlDbType.VarChar).Value = Path.GetFileName(flArchivo.PostedFile.FileName)
                sqlComando.Parameters.Add("@tipoFactura", SqlDbType.VarChar).Value = isFrom
                sqlComando.CommandText = sqlSelect
                existeArchivo = CBool(sqlComando.ExecuteScalar)
                If existeArchivo = False Then
                    sqlComando.CommandText = sqlInsertFile
                    sqlComando.ExecuteNonQuery()
                End If
            End If
            If fechaSalida.Value.Trim <> "" And fechaSalida.Value <> hFechaSalidaActual.Value Then
                sqlComando.CommandText = sqlInsert
                sqlComando.Parameters.Add("@idOperador", SqlDbType.Int).Value = ddlDestinoTraslado.SelectedValue
                sqlComando.ExecuteNonQuery()
                cancelarInstruccionesPorTraslado(sqlConexion, sqlTrans)
            End If
            If isFrom = "fe" Then
                MetodosComunes.actualizarModificadorEnLogFE(sqlConexion, sqlTrans, idFactura, Me)
            End If
            sqlTrans.Commit()
            If fechaSalida.Value.Trim <> "" And fechaSalida.Value <> hFechaSalidaActual.Value Then enviarMail()
            lblRes.Text = "Los Datos de la Factura se Actualizaron Satisfactoriamente<br><br>"
        Catch ex As Exception
            If Not sqlTrans Is Nothing Then sqlTrans.Rollback()
            lblError.Text = "Error al tratar de actualizar Datos. " & ex.Message & "<br><br>"
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Function getTecnologia(ByVal idProducto As Integer) As Integer
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String, resultado As Integer

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
        actualizarDatos()
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

    Private Sub subirArchivoAlServidor()
        Dim nombreArchivo As String = Server.MapPath("ArchivosRecepcion/") & Path.GetFileName(flArchivo.PostedFile.FileName)
        Try
            flArchivo.PostedFile.SaveAs(nombreArchivo)
        Catch ex As Exception
            Throw New Exception("Imposible subir el archivo al servidor. " & ex.Message)
        End Try
    End Sub

    Private Sub cancelarInstruccionesPorTraslado(ByVal sqlConexion As SqlConnection, ByVal sqlTransaccion As SqlTransaction)
        Dim sqlComando As SqlCommand, sqlRead As SqlDataReader
        Dim sqlSelect, sqlUpdInstuccion, sqlUpdFactura As String
        Dim cantidadOR, cantidadOC, cantidadNO, cantidadPedida As Integer

        sqlSelect = "select isnull(sum(cantidadOR),0) as cantidadOR,isnull(sum(cantidadOC),0) as cantidadOC, "
        sqlSelect += " isnull(sum(cantidadNO),0) as cantidadNO from DistribucionInstruccionFactura with(nolock) "
        sqlSelect += " where idInstruccion=3 and idFactura=@idFactura"

        sqlUpdInstuccion = "update DistribucionInstruccionFactura set fechaSalidaFactura=@fechaSalida "
        sqlUpdInstuccion += " where idFactura=@idFactura and idInstruccion=3"

        sqlUpdFactura = "update facturas_externas set cant_region1=cant_region1-@cantidadOR, "
        sqlUpdFactura += " cant_region2=cant_region2-@cantidadOC,cant_region3=cant_region3-@cantidadNO,"
        sqlUpdFactura += " cantidad_pedida=cantidad_pedida-@cantidadPedida where idfactura=@idFactura "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idFactura", SqlDbType.Int).Value = idFactura
            sqlComando.Transaction = sqlTransaccion
            sqlRead = sqlComando.ExecuteReader
            If sqlRead.Read Then
                cantidadOR = sqlRead.GetValue(0)
                cantidadOC = sqlRead.GetValue(1)
                cantidadNO = sqlRead.GetValue(2)
                cantidadPedida = cantidadOR + cantidadOC + cantidadNO
                sqlRead.Close()
                With sqlComando
                    .CommandText = sqlUpdInstuccion
                    .Parameters.Add("@fechaSalida", SqlDbType.SmallDateTime).Value = CDate(fechaSalida.Value)
                    .ExecuteNonQuery()
                    .CommandText = sqlUpdFactura
                    .Parameters.Add("@cantidadOR", SqlDbType.Int).Value = cantidadOR
                    .Parameters.Add("@cantidadOC", SqlDbType.Int).Value = cantidadOC
                    .Parameters.Add("@cantidadNO", SqlDbType.Int).Value = cantidadNO
                    .Parameters.Add("@cantidadPedida", SqlDbType.Int).Value = cantidadPedida
                    .ExecuteNonQuery()
                End With
            End If
            If Not sqlRead.IsClosed Then sqlRead.Close()
        Catch ex As Exception
            Throw New Exception("Error al tratar de actualizar la Fecha de Salida de la Instrucciones de Producto Virgen asociadas a la Factura. " & ex.Message)
        End Try
    End Sub

#Region "Procedimientos para envío de Mail"

    Private Function armarCuerpoMail() As String
        Dim cuerpoMail As New StringBuilder

        Try
            Dim nombreSitio, logo As String
            With cuerpoMail
                .Append("<HTML>")
                .Append(" <HEAD>")
                .Append("  <LINK href='" & nombreSitio & "/include/styleBACK.css' type='text/css' rel='stylesheet'>")
                .Append(" </HEAD>")
                .Append("<body class='cuerpo2'>")
                .Append("<table width='100%' border='0' align='center' cellpadding='5' cellspacing='0' class='tabla'>")
                .Append(" <tr clas='encabezadoMail'>")
                .Append("  <td width='220' bgcolor='#e0e0e0'><img src='" & logo & "'></td>")
                .Append("  <td><b>NOTIFICACION SALIDA DE FACTURA</b></td>")
                .Append(" </tr>")
                .Append("</table>")
                .Append("<br>")
                If Now.Hour < 12 Then
                    .Append("<font class='fuente'>Buenos Días")
                Else
                    .Append("<font class='fuente'>Buenas Tardes")
                End If
                .Append("<br><br>Se acaba de registrar en el sistema, la Salida de una Factura con la siguiente información:")
                .Append("</font><br><br>")
                .Append("<TABLE class='tabla' id='Table2' borderColor='#006699' cellSpacing='1' cellPadding='1' border='1' >")
                .Append(" <TR>")
                .Append("  <TD class='tdTituloRec' colSpan='2'>INFORMACIÓN REGISTRADA</TD> ")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>FACTURA:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & txtFactura.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>ORDEN DE COMPRA:</b>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & txtOrdenCompra.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>GUÍA AEREA:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & txtGuia.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>TIPO DE PRODUCTO:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblTipoProducto.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>PROVEEDOR:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & ddlProveedor.SelectedItem.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>PRODUCTO:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & ddlProducto.SelectedItem.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>TIPO DE RECEPCIÓN:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & ddlTipoRecepcion.SelectedItem.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec' width='120'><b>NÚMERO DE PALETS:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & txtPalets.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>CANTIDAD APROX.:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & txtCantidad.Text & "</b></font></TD>")
                .Append(" </TR>")
                If isFrom = "fe" Then
                    .Append(" <TR>")
                    .Append("  <TD class='tdPrinRec'><b>UNIDADES POR CAJA:</b></TD>")
                    .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & txtUnidadesCaja.Text & "</b></font></TD>")
                    .Append(" </TR>")
                    .Append(" <TR>")
                    .Append("  <TD class='tdPrinRec'><b>CAJAS POR PALET:</b></TD>")
                    .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & txtCajasPalet.Text & "</b></font></TD>")
                    .Append(" </TR>")
                End If
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>PESO:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & txtPeso.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>FECHA RECEPCIÓN:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & lblFechaRecepcion.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>ESTADO RECEPCION:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & ddlEstadoRecepcion.SelectedItem.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>BODEGA:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & ddlBodega.SelectedItem.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>DESTINO TRASLADO:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & ddlDestinoTraslado.SelectedItem.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append(" <TR>")
                .Append("  <TD class='tdPrinRec'><b>OBSERVACIÓN:</b></TD>")
                .Append("  <TD class='tdCampoRec'><font color='MediumBlue'><b>&nbsp;" & txtObservacion.Text & "</b></font></TD>")
                .Append(" </TR>")
                .Append("</TABLE>")
                .Append("<br><font class='fuente'>Cordial Saludo,<br>")
                .Append("<br><b>RECEPCION DE PRODUCTO</b><br><br>")
                .Append("</font><font class='fuente' size='1'><i>Nota: Este correo es generado automaticamente, ")
                .Append("si tiene alguna duda, inquietud o comentario, por favor comuníquese con el responsable del proceso.</i></font></font>")
                .Append("</body>")
                .Append("</HTML>")
                Return .ToString
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de armar cuerpo del Mail de notificación de Salida de Factura. " & ex.Message)
        End Try
    End Function

    Private Sub enviarMail()
        Dim eMail As New MailMessage, cuerpoMail, destinatarios As String

        Try
            destinatarios = getDestinatarios()
            If destinatarios <> "" Then
                cuerpoMail = armarCuerpoMail()
                If cuerpoMail <> "" Then
                    SmtpMail.SmtpServer = ConfigurationManager.AppSettings("mailServer")
                    With eMail
                        .From = "Sistema de Recepción de Facturas <" & ConfigurationManager.AppSettings("mailSender") & ">"
                        .Subject = "Notificación de Salida de Factura - " & lblTipoProducto.Text
                        .To = destinatarios
                        .BodyFormat = MailFormat.Html
                        .Body = cuerpoMail
                    End With
                    SmtpMail.Send(eMail)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de enviar Mail de notificación de Salida de Factura. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Function getDestinatarios() As String
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String
        Dim destinatarios As New StringBuilder

        sqlSelect += "select email from UsuarioAvisarRecepcionFactura ua with(nolock) "
        sqlSelect += "  where enviarSiempre=1 or (enviarSiempre=0 and idUsuario in "
        sqlSelect += " (select idUsuario from UsuarioAvisarTipoProducto with(nolock) where "
        sqlSelect += " idTipoProducto=@idTipoProducto and idUsuario=ua.idUsuario))"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idTipoProducto", SqlDbType.Int).Value = hIdTipoProducto.Value
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            While sqlRead.Read
                destinatarios.Append(sqlRead.GetValue(0) & ";")
            End While
            sqlRead.Close()
            If destinatarios.ToString <> "" Then destinatarios.Length = destinatarios.Length - 1
            Return destinatarios.ToString
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener el listado de destinatarios. ")
        Finally
            If Not sqlComando Is Nothing Then sqlComando.Dispose()
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Function
#End Region

End Class
