Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Localizacion
Imports ILSBusinessLayer.OMS

Partial Public Class ConsultarFacturaGuia
    Inherits System.Web.UI.Page

    Private Shared idUsuario As Integer

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epMuestreo.clear()
        If Not IsPostBack Then
            epMuestreo.setTitle("Consultar Porcentaje de Muestreo")
            epMuestreo.showReturnLink(MetodosComunes.getUrlFrameBack(Me))
            MetodosComunes.setGemBoxLicense()
            pnlFactura.Visible = False
            CargaInicial()
        End If

    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultar.Click
        ConsultarDatos()
    End Sub

    Protected Sub btnActualizar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnActualizar.Click
        Dim fg As New FacturaGuia(CLng(hfIdFacturaGuia.Value))
        Dim resultado As Short

        Me.EstablecerValoresFacturaGuia(fg)
        Try
            resultado = fg.Actualizar()
            If resultado = 0 Then
                epMuestreo.showSuccess("El porcentaje de muestreo fue actualizado satisfactoriamente.")
            Else
                If resultado = 1 Then
                    epMuestreo.showError("Ocurrió un error inesperado al registrar la información. Por favor intente nuevamente")
                Else
                    epMuestreo.showWarning("No se puede registrar la información, por que no se han proporcionado todos los datos requeridos. Por favor verifique")
                End If
            End If
        Catch ex As Exception
            epMuestreo.showError("Error al tratar de actualizar información. " & ex.Message)
        End Try
    End Sub

    Protected Sub ddlFactura_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlFactura.SelectedIndexChanged
        If ddlFactura.SelectedIndex > 0 Then
            Session("factura") = ddlFactura.Items(ddlFactura.SelectedIndex).Value
            CargarGuias(ddlFactura.SelectedValue)
        Else
            If String.IsNullOrEmpty(txtFiltroGuia.Text.Trim()) Then
                Session.Contents.Remove("factura")
                Session.Contents.Remove("guia")
                'FiltrarFactura(sender, e)
                ddlGuia.ClearSelection()
            Else
                Session.Contents.Remove("factura")
                FiltrarGuia(sender, e)
            End If
        End If
    End Sub

    Protected Sub ddlGuia_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGuia.SelectedIndexChanged
        If ddlGuia.SelectedIndex > 0 Then
            Session("guia") = ddlGuia.Items(ddlGuia.SelectedIndex).Value
            CargarFacturas(ddlGuia.SelectedValue)
        Else
            If String.IsNullOrEmpty(txtFiltroFactura.Text.Trim()) Then
                Session.Contents.Remove("factura")
                Session.Contents.Remove("guia")
                'FiltrarGuia(sender, e)
                ddlFactura.ClearSelection()
            Else
                Session.Contents.Remove("guia")
                FiltrarFactura(sender, e)
            End If
        End If
    End Sub

#End Region

#Region "Metodos"

    ''' <summary>
    ''' Carga los elementos iniciales del formulario
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargaInicial()
        inicializaDropDownList(ddlFactura)
        inicializaDropDownList(ddlGuia)

        If Request.QueryString("back") IsNot Nothing And Request.QueryString("back") = "1" Then
            If Session("factura") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Session("factura")) _
            And Session("guia") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Session("guia")) Then

            End If
        End If

    End Sub

    ''' <summary>
    ''' Filtra las facturas según el nombre escrito en la caja de filtro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Filtra las guias según la factura</remarks>
    Protected Sub FiltrarFactura(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroFactura.TextChanged
        Dim dtInfoFacturaGuia As New DataTable
        Dim dtFacturas As New DataTable
        If txtFiltroFactura.Text.Length > 3 Then
            If Session("dtFacturas") Is Nothing Then
                dtInfoFacturaGuia = FacturaGuia.ObtenerListado()
                HttpContext.Current.Session("dtInfoFacturaGuia") = dtInfoFacturaGuia
            Else
                dtFacturas = HttpContext.Current.Session("dtInfoFacturaGuia")
            End If
            Dim arrColumnas As New ArrayList("idFactura,facturaOrden".Split(","))
            Dim filtro As String = "facturaOrden like '%" + txtFiltroFactura.Text + "%'  "
            dtFacturas = MetodosComunes.getDistinctsFromDataTable(dtInfoFacturaGuia, arrColumnas, filtro, "facturaOrden asc")
            MetodosComunes.CargarDropDown(dtFacturas, CType(ddlFactura, ListControl))
            EliminarItemsDuplicados(ddlFactura)

            'If dtFacturas.Rows.Count = 1 Then
            If ddlFactura.Items.Count = 2 Then
                ddlFactura.SelectedIndex = 1
                Session("factura") = ddlFactura.Items(ddlFactura.SelectedIndex).Value
                CargarGuias(ddlFactura.SelectedValue)
            Else
                If ddlFactura.Items.Count > 2 AndAlso Session("factura") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Session("factura")) Then
                    If ddlFactura.Items.FindByValue(Session("factura").ToString()) IsNot Nothing Then
                        ddlFactura.SelectedValue = Session("factura")
                        'CargarGuias(ddlFactura.SelectedValue)
                        Exit Sub
                    End If
                End If

                inicializaDropDownList(ddlGuia)
            End If
        Else
            Session.Contents.Remove("factura")
            Session.Contents.Remove("guia")
            inicializaDropDownList(ddlFactura)
            inicializaDropDownList(ddlGuia)
        End If
    End Sub

    ''' <summary>
    ''' Filtra las guia según el nombre escrito en la caja de filtro en los controles dropdownlist
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks>Filtra las guias según la factura</remarks>
    Protected Sub FiltrarGuia(ByVal sender As Object, ByVal e As EventArgs) Handles txtFiltroGuia.TextChanged
        Dim dtInfoFacturaGuia As New DataTable
        Dim dtGuias As New DataTable
        If txtFiltroGuia.Text.Length > 3 Then
            If Session("dtGuias") Is Nothing Then
                dtInfoFacturaGuia = FacturaGuia.ObtenerListado()
                HttpContext.Current.Session("dtInfoFacturaGuia") = dtInfoFacturaGuia
            Else
                dtGuias = HttpContext.Current.Session("dtInfoFacturaGuia")
            End If
            Dim arrColumnas As New ArrayList("idguia,guiaOrden".Split(","))
            Dim filtro As String = "guiaOrden like '%" + txtFiltroGuia.Text + "%'  "
            dtGuias = MetodosComunes.getDistinctsFromDataTable(dtInfoFacturaGuia, arrColumnas, filtro, "guiaOrden asc")
            MetodosComunes.CargarDropDown(dtGuias, CType(ddlGuia, ListControl))
            EliminarItemsDuplicados(ddlGuia)

            'If dtGuias.Rows.Count = 1 Then
            If ddlGuia.Items.Count = 2 Then
                ddlGuia.SelectedIndex = 1
                Session("factura") = ddlGuia.Items(ddlGuia.SelectedIndex).Value
                CargarFacturas(ddlGuia.SelectedValue)
            Else
                If ddlGuia.Items.Count > 2 AndAlso Session("guia") IsNot Nothing AndAlso Not String.IsNullOrEmpty(Session("guia")) Then
                    If ddlGuia.Items.FindByValue(Session("guia").ToString()) IsNot Nothing Then
                        ddlGuia.SelectedValue = Session("guia")
                        'CargarFacturas(ddlGuia.SelectedValue)
                        Exit Sub
                    End If
                End If

                inicializaDropDownList(ddlFactura)
            End If
        Else
            Session.Contents.Remove("factura")
            Session.Contents.Remove("guia")

            inicializaDropDownList(ddlGuia)
            inicializaDropDownList(ddlFactura)
        End If
    End Sub

    Private Sub EliminarItemsDuplicados(ByRef ddlControl As DropDownList)
        ' Recorre los items ( compara empezando desde el primero, de abajo hacia arriba)   
        For i As Integer = 0 To ddlControl.Items.Count - 2
            For j As Integer = ddlControl.Items.Count - 1 To i + 1 Step -1
                ' ... si es el mismo   
                If ddlControl.Items(i).Value = ddlControl.Items(j).Value Then
                    ' elimina el elemento indicando el índice   
                    ddlControl.Items.RemoveAt(j)
                End If
            Next
        Next
    End Sub

    Private Sub inicializaDropDownList(ByRef control As DropDownList)
        If control.Items.Count > 0 Then control.Items.Clear()
        control.Items.Add(New ListItem("Seleccione...", 0))
    End Sub

    Private Sub CargarGuias(ByVal idFactura As Integer)
        Dim dtInfoFacturaGuia As DataTable = Session("dtInfoFacturaGuia")
        Dim dtGuias As New DataTable
        Dim arrColumnas As New ArrayList("idguia,guiaOrden".Split(","))
        Dim filtro As String = "idFactura=" + idFactura.ToString() + ""
        dtGuias = MetodosComunes.getDistinctsFromDataTable(dtInfoFacturaGuia, arrColumnas, filtro, "guiaOrden asc")
        ddlGuia.DataSource = dtGuias
        ddlGuia.DataTextField = "guiaOrden"
        ddlGuia.DataValueField = "idguia"
        ddlGuia.DataBind()
        ddlGuia.Items.Insert(0, New ListItem("Seleccione", "0"))

        EliminarItemsDuplicados(ddlGuia)

        If dtGuias.Rows.Count = 1 Then
            ddlGuia.SelectedIndex = 1
            Session("guia") = ddlGuia.Items(ddlGuia.SelectedIndex).Value
        End If
    End Sub

    Private Sub CargarFacturas(ByVal idGuia As Integer)
        Dim dtInfoFacturaGuia As DataTable = Session("dtInfoFacturaGuia")
        Dim dtFacturas As New DataTable
        Dim arrColumnas As New ArrayList("idfactura,facturaOrden".Split(","))
        Dim filtro As String = "idGuia=" + idGuia.ToString() + ""

        If ddlFactura.Items.Count > 0 Then ddlFactura.Items.Clear()

        dtFacturas = MetodosComunes.getDistinctsFromDataTable(dtInfoFacturaGuia, arrColumnas, filtro, "facturaOrden asc")
        ddlFactura.DataSource = dtFacturas
        ddlFactura.DataTextField = "facturaOrden"
        ddlFactura.DataValueField = "idfactura"
        ddlFactura.DataBind()
        ddlFactura.Items.Insert(0, New ListItem("Seleccione", "0"))

        EliminarItemsDuplicados(ddlFactura)

        If dtFacturas.Rows.Count = 1 Then
            ddlFactura.SelectedIndex = 1
            Session("factura") = ddlFactura.Items(ddlFactura.SelectedIndex).Value
        End If
    End Sub

    Private Sub ConsultarDatos()
        Dim infoFactGuia As FacturaGuia
        Dim idFactura As Long
        Dim idGuia As Long
        Dim filtro As New FiltroOrdenCompra
        Dim dtOrden As New DataTable()

        pnlFactura.Visible = False

        idFactura = ddlFactura.SelectedValue
        idGuia = ddlGuia.SelectedValue

        Session("idFactura") = idFactura
        Session("idGuia") = idGuia

        Try
            infoFactGuia = New FacturaGuia(idFactura, idGuia)

            With infoFactGuia
                Dim detorden As New ILSBusinessLayer.Recibos.DetalleOrdenCompra(.InformacionFactura.IdDetalleOrdenCompra)
                lblFactura.Text = .InformacionFactura.Factura.ToString()
                lblGuia.Text = .InformacionGuia.Guia.ToString()
                txtMuestreo.Text = .Muestreo.ToString()
                lblMuestreo.Text = .Muestreo.ToString()
                lblCantidad.Text = .Cantidad.ToString()
                filtro.IdOrden = .InformacionFactura.IdOrden
                dtOrden = OrdenCompra.ObtenerListado(filtro)
                If dtOrden IsNot Nothing AndAlso dtOrden.Rows.Count > 0 Then lblCompra.Text = dtOrden.Rows(0)("numeroOrden")
                lblProducto.Text = detorden.Producto.ToString()

                hfIdFacturaGuia.Value = .IdFacturaGuia.ToString()
                Me.ValidarEdicionMuestreo(.IdFacturaGuia)

                lkbCerrar.Visible = MostrarEnlaceMuestreo(idFactura, idGuia)

            End With

            pnlFactura.Visible = True

        Catch ex As Exception
            epMuestreo.showError("Error al tratar de obtener los datos. " & ex.Message)
        End Try
    End Sub

    Private Function MostrarEnlaceMuestreo(ByVal idFactura As Long, ByVal idGuia As Long) As Boolean
        Dim filtro As New FiltroSerialMuestra
        Dim sm As New SerialMuestra
        Dim result As Short

        filtro.IdFactura = idFactura
        filtro.IdGuia = idGuia

        result = sm.ValidarExistenSerialesMuestreo(filtro)

        If result = 1 Then
            Return True
        ElseIf result = 0 Then
            Return False
        Else
            epMuestreo.showError("Error al tratar de validar si existen seriales de muestreo.")
            Return False
        End If

    End Function

    Private Sub ObtenerCiudadCompra(ByVal idCiudad As Integer)
        Dim filtro As New FiltroCiudad
        Dim dt As New DataTable()
        filtro.IdCiudad = idCiudad
        Try
            dt = Ciudad.ObtenerListado(filtro)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                'lblCiudad.Text = dt.Rows(0)("nombre").ToString()
            End If

        Catch ex As Exception
            Throw New Exception("Error al tratar de cargar las Ciudades. " & ex.Message)
        End Try
    End Sub

    Private Sub ValidarEdicionMuestreo(ByVal idFacturaGuia As Long)
        Dim filtro As New FiltroFacturaGuia

        filtro.IdFacturaGuia = idFacturaGuia
        If FacturaGuia.ValidarFacturaGuiaTieneOrdenes(filtro) Then
            txtMuestreo.Visible = False
            lblMuestreo.Visible = True
            btnActualizar.Visible = False
        Else
            txtMuestreo.Visible = True
            lblMuestreo.Visible = False
            btnActualizar.Visible = True
        End If

    End Sub

    Private Sub EstablecerValoresFacturaGuia(ByVal fg As FacturaGuia)
        With fg
            .Muestreo = CShort(txtMuestreo.Text.Trim)
            .Cantidad = CInt(lblCantidad.Text.Trim)
        End With
    End Sub

#End Region

End Class