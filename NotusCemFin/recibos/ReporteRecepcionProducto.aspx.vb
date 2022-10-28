Imports ILSBusinessLayer
Imports ILSBusinessLayer.Productos
Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer.Comunes
Imports System.IO

Partial Public Class ReporteRecepcionProducto
    Inherits System.Web.UI.Page

#Region "Atributos"
    Private _origen As String = ""
    Private _folderTempImage As String
#End Region

#Region "Propiedades"
    Public Property FolderTempImage As String
        Get
            If Session("_folderTempImage") IsNot Nothing Then _folderTempImage = Session("_folderTempImage")
            Return _folderTempImage
        End Get
        Set(value As String)
            _folderTempImage = value
            Session("_folderTempImage") = _folderTempImage
        End Set
    End Property
#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epPrincipal.clear()
        If Not IsPostBack Then
            Try
                epPrincipal.setTitle("Reporte Recepción de Producto")
                CargaInicial()
                Session("notificacion") = Nothing
                'btnBuscar_Click(sender, e)
            Catch ex As Exception
                epPrincipal.showError("Error al cargar la pagina. " & ex.Message)
            End Try
        End If
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            Dim dtDatos As New DataTable
            dtDatos = ObtenerReporte()
            gvDatos.DataSource = dtDatos
            gvDatos.DataBind()
            lnkGenerarExcel.Visible = CBool(dtDatos.Rows.Count)
            Session("notificacion") = 1
        Catch ex As Exception
            epPrincipal.showError("Error al generar el reporte. " & ex.Message)
        End Try
    End Sub

    Protected Sub lnkGenerarExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkGenerarExcel.Click
        Try
            Dim reporte As New Reports.AdministradorInformesExcel("Reporte_Recepcion_Producto.xls", "~/Reports/Plantillas/ReporteRecepcionProducto.xls")
            reporte.DtDatos = EstructuraDescarga()
            reporte.ColumnaInicial = 0
            reporte.FilaInicial = 3
            If reporte.GenerarExcelPlanilla Then
                If _origen <> "EnvioCorreo" Then
                    epPrincipal.showSuccess("Reporte generado correctamente.")
                    Session("nombreArchivo") = reporte.RutaArchivoGenerado
                    MetodosComunes.ForzarDescargaDeArchivo(HttpContext.Current, reporte.RutaArchivoGenerado, True)
                Else
                    Session("nombreArchivo") = reporte.RutaArchivoGenerado
                    _origen = ""
                End If
            Else
                Throw New Exception(reporte.Mensaje)
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al generar el reporte" & ex.Message)
        End Try
    End Sub

    Protected Sub lnkEnviarInformacion_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkEnviarInformacion.Click
        'Envio Información
        EnviarNotificacion()
        '---------------------------------------------------------------------------------------
        'Actualiza campo Notificado de la orden de recepcion
        Dim respuesta As Integer
        Dim dt As DataTable
        dt = Session("Reporte")
        Dim objrecepcion As New Recibos.OrdenRecepcion
        With objrecepcion
            .dtRecepcion = dt
            respuesta = .ActualizarEstadoNotificacionRecepcion()
            If respuesta <> 0 Then
                epPrincipal.showWarning("Correo Enviado satisfatoriamente, pero se genero un error en la actualizacion del estado de envio de la recepcion.")
            Else
                btnBuscar_Click(sender, e)
                epPrincipal.showSuccess("Correo Enviado y estados de notificacion de las recepciones actualizadas exitosamente.")
            End If
        End With
    End Sub

    Protected Sub btnBorrarFiltros_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBorrarFiltros.Click
        Try
            txtIdOrdenRecepcion.Text = String.Empty
            txtIdOrdenCompra.Text = String.Empty
            txtNoOrdenCompra.Text = String.Empty
            ddlTipoProducto.ClearSelection()
            ddlEstado.ClearSelection()
            txtFechaInicial.Text = String.Empty
            txtFechaFinal.Text = String.Empty
        Catch ex As Exception
            epPrincipal.showError("Error al limpiar los filtros")
        End Try
    End Sub

    Private Sub gvDatos_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim fila As DataRowView = CType(e.Row.DataItem, DataRowView)

            Dim imgNotificacion As ImageButton = CType(e.Row.FindControl("imgNotificacion"), ImageButton)
            Dim idEstado As Integer = fila("notificada")
            Select Case idEstado
                Case 0
                    imgNotificacion.ImageUrl = "../images/BallRed.gif"
                Case 1
                    imgNotificacion.ImageUrl = "../images/BallGreen.gif"
            End Select
        End If
    End Sub

    Private Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            'hfIdOrden.Value = e.CommandArgument.ToString
            Dim idOrdenRecepcion As Long = CLng(e.CommandArgument.ToString)
            If e.CommandName.Equals("VerImagen") Then
                Try
                    FolderTempImage = "recibos\images\ImagenRecepcion\" & Guid.NewGuid().ToString()
                    If Not Directory.Exists(Server.MapPath("~") & FolderTempImage) Then
                        Directory.CreateDirectory(Server.MapPath("~") & FolderTempImage)
                    End If

                    Dim miRecepcion As New OrdenRecepcion(idOrdenRecepcion)
                    For Each imgProd As OrdenRecepcion.Imagen In miRecepcion.ListaImagenes
                        Dim objImagen As New Imagen()
                        With objImagen
                            .ArregloByte_Imagen(imgProd.imagen, Server.MapPath("~") & FolderTempImage & "\" & imgProd.nombreImagen, imgProd.contenType)
                        End With
                    Next
                    If (miRecepcion.ListaImagenes.Count > 0) Then
                        isImagenes.ImageSourceFolder = "~\" & FolderTempImage
                        mpeVisualizacionImagen.Show()
                    End If
                  
                Catch ex As Exception
                    epPrincipal.showError("Error al trata de visualizar las imagenes: " & ex.Message)
                End Try
            End If
        Catch ex As Exception
            epPrincipal.showError("Se presento un error al ejecutar el comando: " & ex.Message)
        End Try
    End Sub

    Private Sub btnCerrarVisualizar_Click(sender As Object, e As System.EventArgs) Handles btnCerrarVisualizar.Click
        Try
            isImagenes.ImageSourceFolder = Nothing
            'Se eliminan las imagenes temporales
            If Directory.Exists(Server.MapPath("~") & FolderTempImage) Then Directory.Delete(Server.MapPath("~") & FolderTempImage, True)
            mpeVisualizacionImagen.Hide()
        Catch : End Try
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub CargaInicial()
        Try
            Session.Remove("Reporte")
            lnkGenerarExcel.Visible = False
            ObtenerTipoProducto()
            ObtenerEstado()
        Catch ex As Exception
            epPrincipal.showError("Error al cargar los controles. " & ex.Message)
        End Try
    End Sub

    Private Sub ObtenerTipoProducto()
        Dim filtroTipoProducto As New Estructuras.FiltroTipoProducto
        filtroTipoProducto.Activo = 1
        filtroTipoProducto.ExisteModulo = 1
        filtroTipoProducto.IdModulo = 1
        Try
            With ddlTipoProducto
                .DataSource = ILSBusinessLayer.Productos.TipoProducto.ObtenerListado(filtroTipoProducto)
                .DataTextField = "descripcion"
                .DataValueField = "idTipoProducto"
                .DataBind()
                .Items.Insert(0, New ListItem("Escoja el tipo de producto", 0))
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los tipos de producto. " & ex.Message)
        End Try
    End Sub

    Protected Sub ObtenerEstado()
        Dim dt As New DataTable
        Try
            dt = ILSBusinessLayer.Estado.Obtener(5)
            With ddlEstado
                .DataSource = dt
                .DataTextField = "nombre"
                .DataValueField = "idEstado"
                .DataBind()
                If dt.Rows.Count > 1 Then
                    .Items.Insert(0, New ListItem("Escoja el Estado", 0))
                End If
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener los datos para Moneda. " & ex.Message)
        End Try
    End Sub

    Private Sub cpFiltroProducto_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cpFiltroProducto.Callback
        Dim filtroRapido As String = ""
        If e.Parameter.Length >= 4 Then
            filtroRapido = e.Parameter
            FiltrarProductos(filtroRapido)
        Else
            lblResultadoProducto.Text = "0 Registro(s) Cargado(s)"
        End If
    End Sub

    Public Sub FiltrarProductos(ByVal filtro As String)
        Dim dtProductos As DataTable, dwProductos As DataView
        Try
            dtProductos = ObtenerListadoDeProductos()
            filtro = IIf(filtro.Trim.Length > 0, "nombre like '%" & filtro & "%'", "")
            EnlazarProductos(dtProductos, filtro)
        Catch ex As Exception
            Throw New Exception("Error al tratar de filtrar el listado de Productos. " & ex.Message)
        End Try
    End Sub

    Private Sub EnlazarProductos(ByVal dtProducto As DataTable, Optional ByVal filtro As String = "")
        Try
            Dim dvProducto As DataView = dtProducto.DefaultView
            dvProducto.RowFilter = filtro
            dvProducto.Sort = "nombre asc"
            Dim dt As DataTable = dvProducto.Table
            MetodosComunes.CargarComboDX(cmbProducto, dt, "idProducto", "nombre")
            With cmbProducto
                lblResultadoProducto.Text = .Items.Count.ToString & " Registro(s) Cargado(s)"
                If .Items.Count = 1 Then
                    .SelectedIndex = 0
                End If
            End With
        Catch ex As Exception
            Throw New Exception("Error al tratar de enlazar el listado de Productos. " & ex.Message)
        End Try
    End Sub

    Private Function ObtenerListadoDeProductos() As DataTable
        Dim dtProductos As New DataTable

        Dim listaProducto As ProductoColeccion
        If Session("listaProductos") Is Nothing Then
            listaProducto = New ProductoColeccion
        Else
            listaProducto = CType(Session("listaProductos"), ProductoColeccion)
        End If
        With listaProducto
            .Activo = Enumerados.EstadoBinario.Activo
            dtProductos = .GenerarDataTable()
        End With
        Session("listaProductos") = listaProducto
        Return dtProductos
    End Function

    Private Function ObtenerReporte() As DataTable
        Try
            Dim dt As New DataTable
            Dim filtro As Estructuras.FiltroReporteRecepcion
            With filtro
                If txtIdOrdenRecepcion.Text <> "" Then Integer.TryParse(txtIdOrdenRecepcion.Text, .IdOrdenRecepcion)
                If txtIdOrdenCompra.Text <> "" Then Integer.TryParse(txtIdOrdenCompra.Text, .IdOrdenCompra)
                If txtNoOrdenCompra.Text <> "" Then .NumeroOrdenCompra = txtNoOrdenCompra.Text
                If ddlTipoProducto.SelectedValue > 0 Then .IdTipoProducto = ddlTipoProducto.SelectedValue
                If cmbProducto.Value > 0 Then .IdProducto = cmbProducto.Value
                If ddlEstado.SelectedValue > 0 Then .IdEstado = ddlEstado.SelectedValue
                If txtFechaInicial.Text <> String.Empty Then .FechaInicial = CDate(txtFechaInicial.Text)
                If txtFechaFinal.Text <> String.Empty Then .FechaFinal = CDate(txtFechaFinal.Text)
                If Session("notificacion") = Nothing Then
                    .EstadoNotificacion = 0
                Else
                    .EstadoNotificacion = Nothing
                End If

            End With
            dt = Recibos.OrdenRecepcion.ObtenerReporteRecepcion(filtro)
            Session("Reporte") = dt
            Return dt
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Function EstructuraDescarga() As DataTable
        Try
            Dim dt As New DataTable("ReporteDescarga")
            If Session("ReporteDescarga") Is Nothing Then
                dt.Columns.Add(New DataColumn("material", GetType(String)))
                dt.Columns.Add(New DataColumn("referencia", GetType(String)))
                dt.Columns.Add(New DataColumn("numeroPiezas", GetType(String)))
                dt.Columns.Add(New DataColumn("cantidadEstimada", GetType(Integer)))
                dt.Columns.Add(New DataColumn("guia", GetType(String)))
                dt.Columns.Add(New DataColumn("factura", GetType(String)))
                dt.Columns.Add(New DataColumn("fechaLlegada", GetType(Date)))
                dt.Columns.Add(New DataColumn("observacion", GetType(String)))
            Else
                dt = CType(Session("ReporteDescarga"), DataTable)
            End If
            Dim dtReporte As New DataTable
            If Session("Reporte") IsNot Nothing Then _
            dtReporte = CType(Session("Reporte"), DataTable)
            dt.Merge(dtReporte, False, MissingSchemaAction.Ignore)
            Return dt
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Sub EnviarNotificacion()
        Try
            Dim notificador As New NotificadorGeneralEventos
            Dim resultado As New ResultadoProceso
            With notificador
                .Titulo = "Reporte Recepción de Productos"
                .Asunto = "Reporte de Recepciónes de Producto"
                .Mensaje = "Adjuntamos archivo con las recepciones recibidas."
                .TipoNotificacion = AsuntoNotificacion.Tipo.ReporteRecepcionProducto
                _origen = "EnvioCorreo"
                Dim sender As Object
                Dim e As EventArgs
                lnkGenerarExcel_Click(sender, e)
                _origen = ""
                Dim nombrearchivo As String = Session("nombreArchivo")
                .AdjuntosURL.Add(nombrearchivo)
                .NotificacionEventoAdjunto()
            End With
        Catch ex As Exception
            epPrincipal.showError("Error al enviar la información: " & ex.Message)
        End Try
    End Sub

#End Region

End Class