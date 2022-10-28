Imports System.Data.SqlClient
Imports System.IO

Partial Class recibirFacturaPendiente
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

    Dim idFactura, idTipoProducto, idTipoRecepcion As Integer, laFactura As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Seguridad.verificarSession(Me)
            lblError.Text = ""
            idFactura = Request.QueryString("idF")
            laFactura = Request.QueryString("factura")
            idTipoProducto = Request.QueryString("idTp")
            idTipoRecepcion = Request.QueryString("idTr")
            If Not Me.IsPostBack Then
                getInformacionGeneral(idTipoProducto)
                With hlRegresar
                    .NavigateUrl = .NavigateUrl.Replace("@f", laFactura).Replace("@tp", idTipoProducto.ToString)
                    .NavigateUrl = .NavigateUrl.Replace("@tr", idTipoRecepcion.ToString)
                End With
                getBodegas()
                getEstadosRecepcionFactura()
            End If
        Catch ex As Exception
            lblError.Text = "Error al tratar de cargar página. " & ex.Message & "<br><br>"
        End Try
    End Sub

    Private Sub getInformacionGeneral(ByRef idTipoProducto As Integer)
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlRead As SqlDataReader, sqlSelect As String

        sqlSelect = "select idfactura2 as factura,(select proveedor from proveedores with(nolock) where idproveedor="
        sqlSelect += " fe.idProveedor) as proveedor,(select producto from productos with(nolock) where idproducto="
        sqlSelect += " fe.idproducto) as producto,cantidadEsperada,(select tipoRecepcion from TipoRecepcion with(nolock)"
        sqlSelect += " where idTipoRecepcion=fe.idTipoRecepcion)as tipoRecepcion,fecha,idTipoProducto from facturas_externas"
        sqlSelect += " fe with(nolock) where idfactura=@idFactura "

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlSelect)
            sqlComando.Parameters.Add("@idFactura", idFactura)
            sqlConexion.Open()
            sqlRead = sqlComando.ExecuteReader
            With sqlRead
                If .Read Then
                    lblFactura.Text = .Item("factura").ToString
                    txtFactura.Text = .Item("factura").ToString
                    lblProveedor.Text = .Item("proveedor").ToString
                    lblProducto.Text = .Item("producto").ToString
                    lblCantidad.Text = .Item("cantidadEsperada").ToString
                    lblTipoRecepcion.Text = .Item("tipoRecepcion").ToString
                    lblFecha.Text = String.Format("{0:dd-MMM-yyyy}", .Item("fecha"))
                    idTipoProducto = .Item("idTipoProducto")
                End If
                .Close()
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener Información Registrada de la Factura. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try

    End Sub

    Private Sub getBodegas()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlAdaptador As SqlDataAdapter, dtBodegas As New DataTable
        Dim sqlSelect As String

        sqlSelect = "select idbodega,upper(bodega)+'('+idbodega2+')' as bodega from bodegas "
        sqlSelect += " with(nolock) where idestado=1"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlAdaptador, sqlSelect, True)
            sqlAdaptador.Fill(dtBodegas)
            With ddlBodega
                .DataSource = dtBodegas
                .DataTextField = "bodega"
                .DataValueField = "idbodega"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja una Bodega", "0"))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener listado de Bodegas. " & ex.Message)
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
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

    Private Sub registrarDatos()
        Dim sqlConexion As SqlConnection, sqlComando As SqlCommand
        Dim sqlUpdate, sqlUpAdicional, nombreArchivo As String, existeArchivo As Boolean
        Dim sqlSelect, sqlQuery As String, sqlTrans As SqlTransaction

        sqlUpdate = "update facturas_externas set idfactura2=@factura,guia_aerea=@guia,fecha=getdate(),unidades_caja=@unidadesCaja,"
        sqlUpdate += " cajas_estiva=@cajasEstiba,estado=1,ordenCompra=@ordenCompra,numeroPalets=@numPalets,cantidadAprox="
        sqlUpdate += "@cantidadAprox,peso=@peso,idEstadoRecepcion=@idEstadoRecepcion,idBodega=@idBodega,observacion=@observacion,"
        sqlUpdate += " idUsuarioModifica=@idUsuario where idfactura=@idFactura"

        sqlSelect = "select count(idArchivo) from ArchivoRecepcionFactura where "
        sqlSelect += "idFactura=@idFactura and nombreArchivo=@nombreArchivo and tipoFactura='fe'"

        sqlQuery = "insert into ArchivoRecepcionFactura values('fe',@idFactura,@nombreArchivo,getdate(),@idUsuario)"

        Try
            MetodosComunes.inicializarObjetos(sqlConexion, sqlComando, sqlUpdate)
            With sqlComando.Parameters
                .Add("@idFactura", SqlDbType.Int).Value = idFactura
                .Add("@factura", SqlDbType.VarChar).IsNullable = True
                .Item("@factura").Value = IIf(txtFactura.Text <> "", txtFactura.Text, DBNull.Value)
                .Add("@unidadesCaja", SqlDbType.Int).IsNullable = True
                .Item("@unidadesCaja").Value = IIf(txtUnidadesCaja.Text.Trim <> "", txtUnidadesCaja.Text, DBNull.Value)
                .Add("@cajasEstiba", SqlDbType.Int).IsNullable = True
                .Item("@cajasEstiba").Value = IIf(txtCajasPalet.Text.Trim <> "", txtCajasPalet.Text, DBNull.Value)
                .Add("@guia", txtGuia.Text)
                .Add("@ordenCompra", txtOrdenCompra.Text)
                .Add("@numPalets", txtPalets.Text)
                .Add("@cantidadAprox", txtCantidad.Text)
                .Add("@peso", txtPeso.Text)
                .Add("@idEstadoRecepcion", SqlDbType.Int).Value = ddlEstadoRecepcion.SelectedValue
                .Add("@idBodega", SqlDbType.Int).IsNullable = True
                If ddlBodega.SelectedValue <> "0" And ddlBodega.SelectedValue <> "" Then
                    .Item("@idBodega").Value = ddlBodega.SelectedValue
                Else
                    .Item("@idBodega").Value = DBNull.Value
                End If
                .Add("@observacion", SqlDbType.VarChar).IsNullable = True
                If txtObservacion.Text <> "" Then
                    .Item("@observacion").Value = txtObservacion.Text
                Else
                    .Item("@observacion").Value = DBNull.Value
                End If
                .Add("@idUsuario", SqlDbType.Int).Value = Session("usxp001")
            End With
            sqlConexion.Open()
            sqlTrans = sqlConexion.BeginTransaction
            sqlComando.Transaction = sqlTrans
            If flArchivo.Value <> "" Then
                subirArchivoAlServidor()
                sqlComando.Parameters.Add("@nombreArchivo", SqlDbType.VarChar).Value = Path.GetFileName(flArchivo.PostedFile.FileName)
                sqlComando.CommandText = sqlSelect
                existeArchivo = CBool(sqlComando.ExecuteScalar)
                If existeArchivo = False Then
                    sqlComando.CommandText = sqlQuery
                    sqlComando.ExecuteNonQuery()
                    sqlComando.CommandText = sqlUpdate
                End If
            End If
            sqlComando.ExecuteNonQuery()
            MetodosComunes.actualizarModificadorEnLogFE(sqlConexion, sqlTrans, idFactura, Me)
            sqlTrans.Commit()
            Response.Redirect("verResumenFacturaRecibida.aspx?idFactura=" & idFactura, True)
        Catch ex As Exception
            If Not sqlTrans Is Nothing Then sqlTrans.Rollback()
            lblError.Text = "Error al tratar de registrar datos. " & ex.Message & "<br><br>"
        Finally
            MetodosComunes.liberarConexion(sqlConexion)
        End Try
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        registrarDatos()
    End Sub

    Private Sub subirArchivoAlServidor()
        Dim nombreArchivo As String = Server.MapPath("ArchivosRecepcion/") & Path.GetFileName(flArchivo.PostedFile.FileName)
        Try
            flArchivo.PostedFile.SaveAs(nombreArchivo)
        Catch ex As Exception
            Throw New Exception("Imposible subir el archivo al servidor. " & ex.Message)
        End Try
    End Sub
End Class

