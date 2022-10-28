Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports GemBox.Spreadsheet
Imports System.Drawing

Module MetodosComunes

    Sub New()
    End Sub

    Public Sub GenerarReportesEnExcel(ByVal dsColeccionDatos As DataSet, ByVal NombreRutaArchivo As String, ByVal titulo As ArrayList, _
                                            Optional ByVal nombreHoja As ArrayList = Nothing, Optional ByVal columnas As ArrayList = Nothing, _
                                            Optional ByVal showFooter As Boolean = False)

        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
        Dim miExcel As New ExcelFile
        Dim i As Integer

        For Each dt As DataTable In dsColeccionDatos.Tables
            Dim strTitulo As String = titulo.Item(i)
            Dim strNombre As String = nombreHoja.Item(i)
            Dim arrColumnas As New ArrayList

            If columnas IsNot Nothing AndAlso columnas.Count > 0 Then
                AdicionarHojaDeExcel(miExcel, dt, strNombre, strTitulo, arrColumnas, showFooter)
            Else
                AdicionarHojaDeExcel(miExcel, dt, strNombre, strTitulo)
            End If
            i = i + 1
        Next
        '***Se crea la hoja del reporte ***'
        miExcel.SaveXls(NombreRutaArchivo)

    End Sub

    Public Sub GenerarReportesEnExcel(ByVal dtDatos As DataTable, ByVal NombreRutaArchivo As String, ByVal titulo As String, _
                                          Optional ByVal nombreHoja As String = "", Optional ByVal columnas As ArrayList = Nothing, _
                                          Optional ByVal showFooter As Boolean = False, Optional showOpen As Boolean=False)

        Dim df As IO.FileStream

        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
        Dim miExcel As New ExcelFile
        Dim i As Integer

        If columnas IsNot Nothing AndAlso columnas.Count > 0 Then
            AdicionarHojaDeExcel(miExcel, dtDatos, nombreHoja, titulo, columnas, showFooter)
        Else
            AdicionarHojaDeExcel(miExcel, dtDatos, nombreHoja, titulo)
        End If
        i = i + 1
        '***Se almacena en disco el archivo***'
        miExcel.SaveXls(NombreRutaArchivo)

        If showOpen Then
            Dim startInfo As New ProcessStartInfo()
            startInfo.FileName = NombreRutaArchivo
            startInfo.WindowStyle = ProcessWindowStyle.Maximized
            startInfo.Arguments = NombreRutaArchivo
            Process.Start(NombreRutaArchivo)
        End If
    End Sub

    Public Sub GenerarReportesEnExcel(ByVal oWs As ExcelWorksheetCollection, ByVal ruta As String, Optional ByVal nombreHojas As ArrayList = Nothing)
        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
        Dim miExcel As New ExcelFile
        Dim i As Integer
        '***Se crea la hoja del reporte consolidado***'
        For i = 0 To nombreHojas.Count Step 1
            miExcel.Worksheets.AddCopy(nombreHojas.Item(i), oWs.Item(i))
        Next
        miExcel.SaveXls(ruta)

    End Sub

    Public Sub AdicionarHojaDeExcel(ByVal oExcel As ExcelFile, ByVal dtDatos As DataTable, ByVal nombreHoja As String, ByVal titulo As String, _
                                            Optional ByVal nombreColumnas As ArrayList = Nothing, Optional ByVal showFooter As Boolean = True)

        Dim oWs As ExcelWorksheet = oExcel.Worksheets.Add(nombreHoja)

        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")

        Try
            oWs.ExtractToDataTable(dtDatos, dtDatos.Rows.Count, ExtractDataOptions.StopAtFirstEmptyRow, oWs.Rows(3), oWs.Columns(0))
            oWs.InsertDataTable(dtDatos, "A3", True)
            oWs.Cells.GetSubrangeAbsolute(0, 0, 0, dtDatos.Columns.Count).Merged = True
            With oWs.Cells("A1")
                .Value = titulo
                With .Style
                    .Font.Color = Color.Black
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 16 * 18
                End With
            End With
            For i As Integer = 0 To dtDatos.Columns.Count - 1
                If Not nombreColumnas Is Nothing Then
                    oWs.Cells(2, i).Value = nombreColumnas(i)
                Else
                    oWs.Cells(2, i).Value = dtDatos.Columns(i).ColumnName
                End If
                With oWs.Cells(2, i).Style
                    .FillPattern.SetPattern(FillPatternStyle.Solid, Color.DarkViolet, Color.DarkBlue)
                    .Font.Color = Color.White
                    .Font.Weight = ExcelFont.BoldWeight
                    .Borders.SetBorders(MultipleBorders.Top, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Right, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Left, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Bottom, Color.FromName("black"), LineStyle.Thin)
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With

            Next
            If showFooter Then
                oWs.Cells.GetSubrangeAbsolute(dtDatos.Rows.Count + 3, 0, (dtDatos.Rows.Count + 3), dtDatos.Columns.Count - 1).Merged = True
                With oWs.Cells("A" & (dtDatos.Rows.Count + 4).ToString).Style
                    .FillPattern.SetPattern(FillPatternStyle.Solid, Color.LightGray, Color.LightGray)
                    .Font.Color = Color.DarkBlue
                    .Font.Weight = ExcelFont.BoldWeight
                    .Borders.SetBorders(MultipleBorders.Top, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Right, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Left, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Bottom, Color.FromName("black"), LineStyle.Thin)
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
                oWs.Cells("A" & (dtDatos.Rows.Count + 4).ToString).Value = dtDatos.Rows.Count & " Registro(s) Encontrado(s)"
            End If

            For index As Integer = 0 To dtDatos.Columns.Count - 1
                oWs.Columns(index).AutoFit()
            Next

        Catch ex As Exception
            Throw New Exception("Al crear  hoja de archivo de excel: " & ex.Message & ex.StackTrace)
        End Try

    End Sub

    Public Function GetStartupPath() As String
        Dim fullApplicationPath As String = Process.GetCurrentProcess.MainModule.FileName
        Dim thePath As String = System.IO.Path.GetDirectoryName(fullApplicationPath)
        Return thePath
    End Function

    Public Structure filtrosOrdenesPendientes
        Dim idEstado As Integer
    End Structure

    Public Structure filtroOrdenesProductoProximosRecepcion
        Dim idEstado As Integer
    End Structure
End Module
