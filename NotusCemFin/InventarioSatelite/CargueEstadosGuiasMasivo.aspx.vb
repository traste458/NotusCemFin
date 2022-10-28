Imports System.IO
Imports GemBox.Spreadsheet
Imports ILSBusinessLayer
Imports DevExpress.Web
Imports LumenWorks.Framework.IO.Csv
Public Class CargueEstadosGuiasMasivo
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Seguridad.verificarSession(Me)
#If DEBUG Then
        Session("usxp001") = 2009  '2009
        Session("usxp009") = 180  '185
        Session("usxp007") = 150
        Session("usxp014") = "192.127.62.1"
#End If
        'Seguridad.verificarSession(Me)
        Try
            If Not Me.IsPostBack Then

                With epPrincipal
                    .showReturnLink(MetodosComunes.getUrlFrameBack(Me))
                    .setTitle("Cargue estado de Guias - Masivo")
                End With
                gvError.ClientVisible = False

            End If
        Catch ex As Exception
            epPrincipal.showError("Error al cargar los datos: " & ex.Message)
        End Try

    End Sub

    Private Sub ProcesarArchivoGuia(ByVal dtGuias As DataTable)
        Try
            Dim resultado As New ResultadoProceso
            Dim objGuias As New TransportadoraSatelite

            dtGuias.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), Session("usxp001")))
            dtGuias.AcceptChanges()

            objGuias.IdUsuario = Session("usxp001")
            Session("dtError") = objGuias.CargarAsignacionEstadoGuias(dtGuias)

            resultado = objGuias.resultado

            If (resultado.Valor = 0) Then
                epPrincipal.showSuccess(resultado.Mensaje)
                gvError.ClientVisible = False
            Else
                epPrincipal.showWarning(resultado.Mensaje)
                gvError.ClientVisible = True
                gvError.DataSource = CType(Session("dtError"), DataTable)
                gvError.DataBind()
            End If
        Catch ex As Exception
            epPrincipal.showError("Se generó un error al intentar procesar el archivo: " & ex.Message)
        End Try
    End Sub

    Private Function CrearEstructuraInfoGuia(upLoadedFile As UploadedFile) As DataTable
        MetodosComunes.setGemBoxLicense()
        Dim dtGuias As New DataTable()
        epPrincipal.clear()
        Dim fec As String = DateTime.Now.ToString("HH:mm:ss:fff").Replace(":", "_")
        Dim validacionArchivo As New ResultadoProceso

        If upLoadedFile.ContentLength <= 10485760 Then
            If upLoadedFile.FileName <> "" Then


                Dim ruta As String = HerramientasFuncionales.RUTAALMACENAMIENTOARCHIVOS & "ArchivosTemporales\"
                'Dim ruta As String = "C:\Users\user\Documents\Mis archivos recibidos\"
                Dim nombreArchivo As String = "CambioMaterialGuia_" & Session("usxp001") & fec & Path.GetExtension(upLoadedFile.FileName)
                ruta += nombreArchivo
                upLoadedFile.SaveAs(ruta)
                Dim miExcel As New ExcelFile
                Dim fileExtension As String = Path.GetExtension(upLoadedFile.FileName)
                If (fileExtension <> "") Then
                    fileExtension = fileExtension.ToUpper()
                End If

                Try
                    Select Case fileExtension
                        Case ".XLS"
                            miExcel.LoadXls(ruta)
                        Case ".XLSX"
                            miExcel.LoadXlsx(ruta, XlsxOptions.None)
                        Case ".TXT"
                            dtGuias = FetchFromCSVFileLong(ruta)
                            miExcel.LoadCsv(ruta, CsvType.TabDelimited)
                        Case ".CSV"
                            dtGuias = FetchFromCSVFileLong(ruta)
                            miExcel.LoadCsv(ruta, CsvType.TabDelimited)
                            Exit Select
                    End Select
                Catch ex As Exception
                    Throw New Exception("El archivo esta incorrecto o no tiene el formato esperado. Por favor verifique: " & ex.Message)
                End Try

                If miExcel.Worksheets.Count > 0 Then
                    Dim oWsInfogenera As ExcelWorksheet = miExcel.Worksheets.Item(0)
                    Dim extencion As String = Path.GetExtension(upLoadedFile.FileName).ToLower
                    If extencion = ".xls" Or extencion = ".xlsx" Or extencion = ".csv" Or extencion = ".txt" Then
                        If oWsInfogenera.CalculateMaxUsedColumns() > 5 Then
                            epPrincipal.showError("El archivo tiene mas columnas de las requeridas: " & oWsInfogenera.CalculateMaxUsedColumns().ToString())
                            Return dtGuias
                        ElseIf oWsInfogenera.CalculateMaxUsedColumns() < 5 Then
                            epPrincipal.showError("El archivo tiene menos columnas de las requeridas: " & oWsInfogenera.CalculateMaxUsedColumns().ToString())
                            Return dtGuias
                        End If
                    End If


                    Dim filaInicial As Integer = oWsInfogenera.Cells.FirstRowIndex
                    Dim columnaInicial As Integer = oWsInfogenera.Cells.FirstColumnIndex
                    If extencion = ".xls" Or extencion = ".xlsx" Then
                        dtGuias = CrearEstructuraInfoGuia()
                        AddHandler oWsInfogenera.ExtractDataEvent, AddressOf ExtractDataErrorHandler
                        oWsInfogenera.ExtractToDataTable(dtGuias, oWsInfogenera.Rows.Count, ExtractDataOptions.SkipEmptyRows,
                                    oWsInfogenera.Rows(filaInicial + 1), oWsInfogenera.Columns(columnaInicial))
                    End If
                End If
            End If
        End If

        Return dtGuias
    End Function

    Private Function CrearEstructuraInfoGuia() As DataTable
        Dim dtAux As New DataTable

        With dtAux.Columns
            dtAux.Columns.Add("GUIA", GetType(String))
            dtAux.Columns.Add("ESTADO", GetType(String))
            dtAux.Columns.Add("FECHA", GetType(String))
            dtAux.Columns.Add("NOVEDAD", GetType(String))
            dtAux.Columns.Add("ACLARACION", GetType(String))
        End With
        Return dtAux
    End Function

    Private Sub ExtractDataErrorHandler(ByVal sender As Object, ByVal e As ExtractDataDelegateEventArgs)
        If e.ErrorID = ExtractDataError.WrongType Then
            If e.ExcelValue Is Nothing Then
                e.DataTableValue = DBNull.Value
            Else
                e.DataTableValue = e.ExcelValue.ToString()
            End If
            e.Action = ExtractDataEventAction.Continue
        End If
    End Sub

    Protected Sub btnCargarGuias_Click(sender As Object, e As EventArgs) Handles btnCargarGuias.Click

    End Sub

    Protected Sub upArchivoGuia_FileUploadComplete(sender As Object, e As DevExpress.Web.FileUploadCompleteEventArgs) Handles upArchivoGuia.FileUploadComplete

        If upArchivoGuia.HasFile Then
            Dim dtGuias As DataTable = CrearEstructuraInfoGuia(e.UploadedFile)

            If dtGuias.Rows.Count < 1 Then
                epPrincipal.showError("No se pudieron leer los registros del archivo o esta vacío: ")
            Else
                dtGuias.Columns.Add(New DataColumn("fila"))
                Dim fil As Integer = 1
                For Each row As DataRow In dtGuias.Rows
                    row("fila") = fil
                    fil = fil + 1
                Next
                ProcesarArchivoGuia(dtGuias)
            End If


            aspLabelGuia.Text = e.UploadedFile.FileName.ToString
        Else
            epPrincipal.showWarning("Debe seleccionar el archivo a cargar")
        End If
    End Sub
    Public Function FetchFromCSVFileLong(ByVal filePath As String) As DataTable

        Dim hasHeader As Boolean = True
        Dim csvTable As DataTable = New DataTable()

        Using csvReader As CsvReader = New CsvReader(New StreamReader(filePath), hasHeader, vbTab, 1)
            Dim fieldCount As Integer = csvReader.FieldCount
            Dim headers As String() = csvReader.GetFieldHeaders()

            For Each headerLabel As String In headers
                csvTable.Columns.Add(headerLabel, GetType(String))
            Next

            While csvReader.ReadNextRecord()
                Dim newRow As DataRow = csvTable.NewRow()

                For i As Integer = 0 To fieldCount - 1
                    newRow(i) = csvReader(i)
                Next

                csvTable.Rows.Add(newRow)
            End While
        End Using

        Return csvTable
    End Function

    Protected Sub gvError_DataBinding(sender As Object, e As EventArgs) Handles gvError.DataBinding
        gvError.DataSource = CType(Session("dtError"), DataTable)
    End Sub
End Class