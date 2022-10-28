Imports ILSBusinessLayer
Partial Public Class AdmTipoProductoPrincipalSecundario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epPrincipal.clear()
        If Not IsPostBack Then
            Try
                epPrincipal.setTitle("Adm. Tipos de Producto(s) Principal y Secundario")
                CargarProductoPrincipal()
                lnkAgregarSecundario.Visible = False
            Catch ex As Exception
                epPrincipal.showError("Error al cargar la pagina. " & ex.Message)
            End Try
        End If
    End Sub

    Protected Sub CargarProductoPrincipal()
        Try
            Dim filtroTipoProducto As New Estructuras.FiltroTipoProducto
            Dim dtTipoProducto As New DataTable
            filtroTipoProducto.Activo = Enumerados.EstadoBinario.Activo
            dtTipoProducto = Productos.TipoProducto.ObtenerListado(filtroTipoProducto)            
            gvTipoPrincipal.DataSource = dtTipoProducto
            gvTipoPrincipal.DataBind()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub CargarTipoProductoSecundario(ByVal idTipoProductoPrincipal As Integer)
        Try
            Dim filtroProductoAdicional As New Estructuras.FiltroCombinacionTipoProducto
            Dim dtTipoSecundario As New DataTable
            Dim tipoPrincipal As New Productos.TipoProducto(idTipoProductoPrincipal)
            Dim tipoProductoLista As New ArrayList()
            lblInfoTipoPrincipal.Text = "del tipo de producto " & tipoPrincipal.Descripcion
            filtroProductoAdicional.IdTipoPrimario = CShort(idTipoProductoPrincipal)
            dtTipoSecundario = Recibos.CombinacionTipoProducto.ObtenerListado(filtroProductoAdicional)
            If Not dtTipoSecundario.Rows.Count > 0 Then
                gvTipoSecundario.EmptyDataText = "<p class='vacio'>No existen tipo de producto secundario para el producto " & tipoPrincipal.Descripcion & "</p>"
            Else                
                For i As Integer = 0 To dtTipoSecundario.Rows.Count - 1
                    tipoProductoLista.Add(dtTipoSecundario.Rows(i)("idTipoProductoSecundario"))
                Next
            End If
            Session("idTipoProductoSecundario") = tipoProductoLista
            gvTipoSecundario.DataSource = dtTipoSecundario
            gvTipoSecundario.DataBind()            
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Protected Sub gvTipoPrincipal_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTipoPrincipal.RowCommand
        Try
            If e.CommandName = "verSecundario" Then            
                'e.Row.BackColor = Drawing.Color.Green
                'Dim botonImagen As ImageButton = CType(e.CommandSource, ImageButton)
                'Dim fila As GridViewRow = CType(botonImagen.Parent.Parent, GridViewRow)
                'If hfFilaActual.Value <> String.Empty Then
                '    CType(gvTipoSecundario.FindControl(hfFilaActual.Value), GridViewRow).BackColor = Color.White
                'End If                
                'fila.BackColor = Color.Blue
                'hfFilaActual.Value = fila.ID
                Dim idTipoPrincipal As Integer = CInt(e.CommandArgument)
                CargarTipoProductoSecundario(idTipoPrincipal)
                hfTipoSeleccionado.Value = idTipoPrincipal
                lnkAgregarSecundario.Visible = True
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al cargar el tipo de producto secundario. " & ex.Message)
        End Try
    End Sub

    Protected Sub lnkAgregarSecundario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAgregarSecundario.Click
        CargarTipoProductoSeleccionar()
        mpeTipoProductoSecundario.Show()
    End Sub

    Protected Sub CargarTipoProductoSeleccionar()
        Try            
            Dim dtTipoProducto As New DataTable
            Dim listaTipoSecundario As New ArrayList()
            Dim filtro As New Estructuras.FiltroTipoProducto
            listaTipoSecundario = CType(Session("idTipoProductoSecundario"), ArrayList)
            If listaTipoSecundario.Count > 0 Then filtro.ListaNoCargar = listaTipoSecundario
            dtTipoProducto = Productos.TipoProducto.ObtenerListado(filtro)
            cblTipoProductos.DataSource = dtTipoProducto
            cblTipoProductos.DataTextField = "descripcion"
            cblTipoProductos.DataValueField = "idTipoProducto"
            cblTipoProductos.DataBind()
        Catch ex As Exception
            epPrincipal.showError("Error al cargar el tipo de producto adicional. " & ex.Message)
        End Try
    End Sub

    Protected Sub gvTipoSecundario_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTipoSecundario.RowCommand
        Try
            If e.CommandName = "eliminar" Then
                Dim idTipoSecundario As Short = CShort(e.CommandArgument)
                Dim idTipoPrimario As Short = CShort(hfTipoSeleccionado.Value)
                Dim Combinacion As New Recibos.CombinacionTipoProducto(idTipoPrimario, idTipoSecundario)
                Combinacion.Eliminar()
                CargarTipoProductoSecundario(CInt(hfTipoSeleccionado.Value))
                epPrincipal.showSuccess("Eliminación correcta")                
            End If
        Catch ex As Exception
            epPrincipal.showError("Error al eliminar el tipo de producto secundario. " & ex.Message)
        End Try
    End Sub

    Protected Sub btnAgregar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAgregar.Click
        Try
            Dim listaSeleccionado As New ArrayList()
            For i As Integer = 0 To cblTipoProductos.Items.Count - 1                
                If cblTipoProductos.Items(i).Selected Then
                    listaSeleccionado.Add(cblTipoProductos.Items(i).Value)
                End If
            Next
            Dim combinacion As New Recibos.CombinacionTipoProducto()
            With combinacion
                .IdCreador = CLng(Session("usxp001"))
                .Descripcion = String.Empty
                .Crear(CInt(hfTipoSeleccionado.Value), listaSeleccionado)
                epPrincipal.showSuccess("Tipo de producto agregado correctamente")
            End With
            CargarTipoProductoSecundario(CInt(hfTipoSeleccionado.Value))
        Catch ex As Exception
            epPrincipal.showError("Error al agregar el producto adicional. " & ex.Message)
        End Try
    End Sub
End Class