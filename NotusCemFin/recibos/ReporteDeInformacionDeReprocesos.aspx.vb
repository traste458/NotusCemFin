Imports GemBox.Spreadsheet
Imports System.IO
Imports DevExpress.Web
Imports ILSBusinessLayer

Public Class Reporte_DeInformacionDeReprocesos
    Inherits System.Web.UI.Page


#Region "Atributos"

    Private oExcel As ExcelFile

#End Region

#Region "Propiedades"

    Public ReadOnly Property archivoResultado() As Boolean
        Get
            If Session("dtResultado") Is Nothing Then
                Return False
            Else
                Return (CType(Session("dtResultado"), DataTable).Rows.Count > 0)
            End If
        End Get
    End Property

#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Seguridad.verificarSession(Me)
            miEncabezado.clear()
            If Not IsPostBack Then
                With miEncabezado
                    .showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                    .setTitle("Reporte De Informacion De Reprocesos")
                End With
                limpiar()
            End If
            MetodosComunes.setGemBoxLicense()

        Catch ex As Exception
            miEncabezado.showError("Error al tratar de cargar página. " & ex.Message & "<br><br>")
        End Try
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        gveExportadorErrores.WriteXlsxToResponse("ErroresCargue")
    End Sub

    Private Sub cperrores_Callback(sender As Object, e As DevExpress.Web.CallbackEventArgsBase) Handles cperrores.Callback
        Try

            If Session("dtResultado") Is Nothing Then
            Else
                With gvErrores
                    .DataSource = CType(Session("dtResultado"), DataTable)
                    .DataBind()
                End With
                rpLogErrores.Visible = True
            End If

        Catch ex As Exception
            miEncabezado.showError("Se generó un error al intentar visualizar el log: " & ex.Message)
            CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
        End Try
    End Sub

    Protected Sub gvErrores_DataBinding(sender As Object, e As EventArgs) Handles gvErrores.DataBinding
        gvErrores.DataSource = CType(Session("dtResultado"), DataTable)
    End Sub
    Protected Sub ButtonVerEjemplo_Click(sender As Object, e As EventArgs) Handles ButtonVerEjemplo.Click
        Try
            miEncabezado.clear()
            Dim filename As String = Server.MapPath("~/Recibos/Plantillas/EjemploReporteDeInformacionDeReprocesos.xlsx")
            If filename <> "" Then
                Dim file As System.IO.FileInfo = New System.IO.FileInfo(filename)
                If file.Exists Then
                    Response.Clear()
                    Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
                    Response.AddHeader("Content-Length", file.Length.ToString())
                    Response.ContentType = "application/octet-stream"
                    Response.WriteFile(file.FullName)
                    Response.End()

                Else
                    Response.Write("Este archivo no existe.")

                End If

            End If
            Response.Redirect("~/Recibos/Plantillas/EjemploReporteDeInformacionDeReprocesos.xlsx", False)

        Catch ex As Exception
            miEncabezado.showError("Error al Abrir el archivo de Ejemplo. " & ex.Message & "<br><br>")
        End Try
    End Sub

#End Region
#Region "Métodos Privados"
    Private Sub upArchivo_FileUploadComplete(sender As Object, e As FileUploadCompleteEventArgs) Handles upArchivo.FileUploadComplete
        Try
            miEncabezado.clear()
            limpiar()
            If upArchivo.HasFile Then
                Dim fileExtension As String = Path.GetExtension(upArchivo.FileName)
                Dim numColumnas As Integer
                Dim resultado As Integer = -1
                Dim idUsuario As Integer = CInt(Session("usxp001"))
                Session.Remove("dtResultado")
                Dim dtInformacionGeneral As New DataTable()
                oExcel = New ExcelFile()
                Try
                    If fileExtension.ToUpper = ".XLS" Then
                        oExcel.LoadXls(New MemoryStream(upArchivo.UploadedFiles(0).FileBytes))
                    ElseIf fileExtension.ToUpper = ".XLSX" Then
                        oExcel.LoadXlsx(New MemoryStream(upArchivo.UploadedFiles(0).FileBytes), XlsxOptions.None)
                    End If
                Catch ex As Exception
                    miEncabezado.showError("El archivo que intenta abrir, tiene un formato diferente al especificado por la extensión del archivo, por favor ábralo y guárdelo en el formato correcto: ")
                    CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
                    CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 0
                    Exit Sub
                End Try
                Dim oWs As ExcelWorksheet = oExcel.Worksheets.ActiveWorksheet
                numColumnas = oWs.CalculateMaxUsedColumns()
                If numColumnas = 9 Then
                    dtInformacionGeneral = CrearEstructuraDeTabla()
                    Dim filaInicial As Integer = oWs.Cells.FirstRowIndex
                    Dim columnaInicial As Integer = oWs.Cells.FirstColumnIndex
                    AddHandler oWs.ExtractDataEvent, AddressOf ExtractDataErrorHandler
                    oWs.ExtractToDataTable(dtInformacionGeneral, oWs.Rows.Count, ExtractDataOptions.SkipEmptyRows, oWs.Rows(filaInicial), oWs.Columns(columnaInicial))
                    If Not dtInformacionGeneral.Rows(0).Item(0).ToString().ToLower() = "Serial".ToLower() Then
                        RegError("0", "Verifique que exista una columna llamada Serial  y sea la columna 1 en el archivo, la columna actual se llama: " & dtInformacionGeneral.Rows(0).Item(0).ToString())
                    End If
                    If Session("dtResultado") Is Nothing Then
                        dtInformacionGeneral.Rows(0).Delete()
                    Else
                        CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 0
                        Exit Sub
                    End If
                    dtInformacionGeneral.Columns.Add(New DataColumn("Fila"))
                    Dim fil As Integer = 1
                    For Each row As DataRow In dtInformacionGeneral.Rows
                        row("Fila") = fil
                        fil = fil + 1
                    Next
                    If Session("dtResultado") Is Nothing Then
                        'ValidarInformacionArchivo(dtInformacionGeneral)
                             Dim cargueSeriales As New ReporteDeInformacionDeReprocesos()
                            With cargueSeriales
                                .IdUsuario = idUsuario
                                .NombrePlantilla = Server.MapPath("~/recibos/Plantillas/PlantillaReporteDeInformacionDeReprocesos.xlsx")
                                .NombreArchivo = Server.MapPath("~/archivos_planos/" & "ReporteDeInformacionDeReprocesos_" & idUsuario & ".xlsx")
                                .GeneraReporte(dtInformacionGeneral)
                                If .Resultado.Valor <= 0 Then
                                    miEncabezado.showWarning("No se pudo generar la informacion solicitadas " & .Resultado.Mensaje)
                                    CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
                                    CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 0
                                Else
                                    Session("ArchivoAdescargar") = .Resultado.Mensaje
                                End If
                            End With
                            CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 1
                    Else
                        CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 0
                    End If
                ElseIf numColumnas > 9 Then
                    RegError(" ", "No se puede procesar el archivo, ya que contiene más columnas que las esperadas. Por favor verifique")
                    CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 0
                Else
                    RegError(" ", "No se puede procesar el archivo, ya que contiene menos columnas que las esperadas. Por favor verifique")
                    CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 0
                End If
            End If

            e.CallbackData = e.UploadedFile.PostedFile.FileName
        Catch ex As Exception
            miEncabezado.showError("Se generó un error al intentar procesar el archivo: " & ex.Message)
            CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
            CType(sender, ASPxUploadControl).JSProperties("cpResultadoProceso") = 0
        End Try
    End Sub

    Private Sub limpiar()
        Try
            Session.Remove("ArchivoAdescargar")
            Session.Remove("dtResultado")
            rpLogErrores.Visible = False
            btnDescargaArchivo.ClientVisible = False
        Catch ex As Exception
            miEncabezado.showError("Error Al limpiar los campos . " & ex.Message & "<br><br>")
        End Try
    End Sub
    Private Sub RegError(ByVal Fila As String, ByVal Mensaje As String)
        Try
            Dim dtError As New DataTable
            If Session("dtResultado") Is Nothing Then
                dtError.Columns.Add(New DataColumn("Fila", GetType(String)))
                dtError.Columns.Add(New DataColumn("Mensaje", GetType(String)))
                Session("dtResultado") = dtError
            Else
                dtError = Session("dtResultado")
            End If
            Dim dr As DataRow = dtError.NewRow()
            dr("Fila") = Fila
            dr("Mensaje") = Mensaje
            dtError.Rows.Add(dr)
            Session("dtResultado") = dtError
        Catch ex As Exception
            miEncabezado.showError("Error al registra errores . " & ex.Message & "<br><br>")
        End Try
    End Sub
    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        With dtAux.Columns
            dtAux.Columns.Add(New DataColumn("Serial", GetType(String)))
            dtAux.Columns.Add(New DataColumn("GrupoDevolucion", GetType(String)))
            dtAux.Columns.Add(New DataColumn("Revisado", GetType(String)))
            dtAux.Columns.Add(New DataColumn("Material", GetType(String)))
            dtAux.Columns.Add(New DataColumn("Region", GetType(String)))
            dtAux.Columns.Add(New DataColumn("Descripcion", GetType(String)))
            dtAux.Columns.Add(New DataColumn("TipoOrden", GetType(String)))
            dtAux.Columns.Add(New DataColumn("Fecha", GetType(String)))
            dtAux.Columns.Add(New DataColumn("observacion", GetType(String)))
        End With
        Return dtAux
    End Function

    Private Sub ExtractDataErrorHandler(sender As Object, e As ExtractDataDelegateEventArgs)
        If e.ErrorID = ExtractDataError.WrongType Then
            If Not IsNumeric(e.ExcelValue) And e.ExcelValue = Nothing Then
                e.DataTableValue = Nothing
            Else
                e.DataTableValue = e.ExcelValue.ToString()
            End If

            If e.DataTableValue = Nothing Then
                e.Action = ExtractDataEventAction.SkipRow
            Else
                e.Action = ExtractDataEventAction.Continue
            End If
        End If
    End Sub
    Private Sub ValidarInformacionArchivo(ByVal Dtdatos As DataTable)
        Try
            For Each row As DataRow In Dtdatos.Rows

                If Not (IsNumeric(row("Entrega"))) Then
                    RegError(row("Fila"), "El numero de (Entrega) tiene caracteres no validos por favor verificar")
                End If
                If Not (IsNumeric(row("Material"))) Then
                    RegError(row("Fila"), "El (material) tiene valores no validos por favor verificar")
                End If
                If Not (IsNumeric(row("Doccompra"))) Then
                    RegError(row("Fila"), "La (cantidad) no es valida por favor verificar")
                End If
            Next
        Catch ex As Exception
            miEncabezado.showError("Error al validar la información. " & ex.Message & "<br><br>")
        End Try
    End Sub
    Protected Sub btnDescargaArchivo_Click(sender As Object, e As EventArgs) Handles btnDescargaArchivo.Click

        Try
            miEncabezado.clear()
            DescargaArchivo()
        Catch ex As Exception
            miEncabezado.showError("Se genero el sigiente error al descargar el archivo. " & ex.Message & "<br><br>")
            CType(sender, ASPxUploadControl).JSProperties("cpMensaje") = miEncabezado.RenderHtml()
        End Try
    End Sub

    Protected Sub DescargaArchivo()
        If Session("ArchivoAdescargar") IsNot Nothing Then
            Dim archivoAdescargar As String
            archivoAdescargar = TryCast(Session("ArchivoAdescargar"), String)
            Dim nombreMostrar As String = System.IO.Path.GetFileName(archivoAdescargar)
            MetodosComunes.ForzarDescargaDeArchivo(HttpContext.Current, archivoAdescargar, nombreMostrar)
            'Dim file As System.IO.FileInfo = New System.IO.FileInfo(archivoAdescargar)
            'If file.Exists Then
            '    Response.Clear()
            '    Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
            '    Response.AddHeader("Content-Length", file.Length.ToString())
            '    Response.ContentType = "application/octet-stream"
            '    Response.WriteFile(file.FullName)
            '    Response.Redirect("~/archivos_planos/" & file.Name, False)
            'Else
            '    Response.Write("Este archivo no existe.")

            'End If
        End If
    End Sub
#End Region
End Class