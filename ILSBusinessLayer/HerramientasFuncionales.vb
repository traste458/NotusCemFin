Imports System.Web.UI.WebControls
Imports GemBox.Spreadsheet
Imports System.Drawing
Imports System.IO
Imports System.Web
Imports System.Text

Public Module HerramientasFuncionales

    Public Const RUTAALMACENAMIENTOARCHIVOS As String = "\\Colbogsacde001\Portales\ArchivosTemporale\MensajeriaEspecializada\Tem\"

    Public Function GetDistinctsFromDataTable(ByVal dtOrigen As DataTable, ByVal arrCampos As ArrayList, Optional ByVal filtro As String = "", Optional ByVal sort As String = "") As DataTable
        Dim dtDatos, dtDictionary As DataTable, dcDato, dcsPK(arrCampos.Count - 1) As DataColumn
        Dim drDato, drAuxOrigen(), drEntry As DataRow, index As Integer

        Try
            dtDictionary = New DataTable
            dtDictionary.Columns.Add("key", GetType(Object))
            dtDictionary.Columns.Add("value", GetType(Object))
            Dim dictionaryKeys As DataColumn() = {dtDictionary.Columns("key")}
            dtDictionary.PrimaryKey = dictionaryKeys

            '****Se recorre el array de campos proporcionado, con el fin de crear una ****'
            '****columna y una entrada de diccionario (en hashtable) por cada campo****' 
            For index = 0 To arrCampos.Count - 1
                dcDato = New DataColumn(arrCampos(index).ToString, GetType(String))
                dcsPK(index) = dcDato
                drEntry = dtDictionary.NewRow
                drEntry("key") = arrCampos(index).ToString
                drEntry("value") = ""
                dtDictionary.Rows.Add(drEntry)
            Next
            dtDatos = New DataTable
            '***Se Agregan las Columnas a la Tabla****'
            dtDatos.Columns.AddRange(dcsPK)
            '****Se establecen todos los campos como llave primaria, con el fin de ****'
            '****optimizar la búsqueda, necesaria para que el procedimiento sea eficaz,****'
            '****aún cuando el conjunto de datos proporcionado, no esté ordenado****'
            dtDatos.PrimaryKey = dcsPK

            Dim hayDiferencia As Boolean, pkKeys As New ArrayList
            '****Se recorre el cunjunto de datos base, obteniendo los valores distintos****'
            drAuxOrigen = dtOrigen.Select(filtro, sort)
            For Each drAux As DataRow In drAuxOrigen
                hayDiferencia = False
                drDato = dtDatos.NewRow
                pkKeys.Clear()
                For Each drEntry In dtDictionary.Rows
                    drDato(drEntry("key")) = drAux(drEntry("key")).ToString
                    If drAux(drEntry("key")).ToString.ToLower <> drEntry("value").ToString.ToLower Then
                        drEntry("value") = drAux(drEntry("key")).ToString
                        hayDiferencia = True
                    End If
                    Dim clave As String
                    clave = drAux(drEntry("key"))
                    pkKeys.Add(drAux(drEntry("key")))
                Next
                If hayDiferencia Then
                    If dtDatos.Rows.Find(pkKeys.ToArray) Is Nothing Then dtDatos.Rows.Add(drDato)
                End If
            Next
            Return dtDatos
        Catch ex As Exception
            Throw New Exception("Error al tratar de obtener datos distintos de la Tabla. " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Función para obtener el índice de una columna dentro de un GridView
    ''' a partir del valor del HeaderText.
    ''' </summary>
    ''' <param name="nombreColumna"></param>
    ''' <param name="gvDatos"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function ObtenerIdColumna(ByVal nombreColumna As String, ByRef gvDatos As GridView) As Integer
        For Each columna As DataControlField In gvDatos.Columns
            If columna.HeaderText = nombreColumna Then
                Dim idColumna As Integer = gvDatos.Columns.IndexOf(columna)
                Return idColumna
            End If
        Next
    End Function

    Public Sub CargarLicenciaGembox()
        GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
    End Sub

    Public Function IsNullableType(ByVal myType As Type) As Boolean
        Return (myType.IsGenericType) AndAlso (myType.GetGenericTypeDefinition() Is GetType(Nullable(Of )))
    End Function

    Public Function EsNuloOVacio(ByVal cadena As Object) As Boolean
        If cadena IsNot Nothing AndAlso cadena.ToString.Trim.Length > 0 Then
            Return False
        Else
            Return True
        End If
    End Function

    Public Sub ForzarDescargaDeArchivo(ByVal rutaArchivo As String, ByVal nombreMostrarArchivo As String)
        Dim infoArchivo As FileInfo
        Dim contextoHttp As HttpContext = HttpContext.Current

        Try

            infoArchivo = New FileInfo(rutaArchivo)
            If infoArchivo.Exists Then
                Dim myFile As New FileStream(rutaArchivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                Dim binaryReader As New BinaryReader(myFile)
                Dim startBytes As Long = 0
                Dim lastUpdateTiemStamp As String = File.GetLastWriteTimeUtc(rutaArchivo).ToString("r")
                Dim encodedData As String = HttpUtility.UrlEncode(nombreMostrarArchivo, Encoding.UTF8) + lastUpdateTiemStamp

                With contextoHttp.Response
                    .Clear()
                    .Buffer = False
                    .ContentType = "application/octet-stream"
                    .AddHeader("Content-Disposition", "attachment; filename=" & nombreMostrarArchivo)
                    .AddHeader("Content-Length", (infoArchivo.Length - startBytes).ToString())
                    If infoArchivo.Length > 10485760 Then
                        .AddHeader("Accept-Ranges", "bytes")
                        .AppendHeader("Last-Modified", lastUpdateTiemStamp)
                        .AppendHeader("ETag", Chr(34) & encodedData & Chr(34))
                        .AddHeader("Connection", "Keep-Alive")
                        '.ContentEncoding = Encoding.UTF8
                        binaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin)
                        Dim maxCount As Integer = CInt(Math.Ceiling((infoArchivo.Length - startBytes + 0.0) / 1024))
                        Dim index As Integer
                        While index < maxCount And .IsClientConnected
                            .BinaryWrite(binaryReader.ReadBytes(1024))
                            .Flush()
                            index += 1
                        End While
                    Else
                        If .IsClientConnected Then .WriteFile(rutaArchivo)
                        If .IsClientConnected Then .Flush()
                    End If
                    .End()
                End With
            End If
        Catch abEx As System.Threading.ThreadAbortException
        End Try
    End Sub


#Region "GemBox"

    Public Sub exportarDatosAExcelGemBox(ByVal dtDatos As DataTable, ByVal nombreArchivo As String, ByVal ruta As String, _
                                                Optional ByVal nombreColumnas As ArrayList = Nothing)
        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
        Dim ef As New ExcelFile
        Dim ws As ExcelWorksheet

        Try
            ws = ef.Worksheets.Add("Datos")
            ws.ExtractToDataTable(dtDatos, dtDatos.Rows.Count, ExtractDataOptions.StopAtFirstEmptyRow, ws.Rows(0), ws.Columns(0))
            ws.InsertDataTable(dtDatos, "A1", True)

            For i As Integer = 0 To dtDatos.Columns.Count - 1
                If Not nombreColumnas Is Nothing Then
                    ws.Cells(0, i).Value = nombreColumnas(i)
                Else
                    ws.Cells(0, i).Value = dtDatos.Columns(i).ColumnName
                End If
                With ws.Cells(0, i).Style
                    .FillPattern.SetPattern(FillPatternStyle.Solid, Color.DarkBlue, Color.DarkBlue)
                    .Font.Color = Color.White
                    .Font.Weight = ExcelFont.BoldWeight
                    .Borders.SetBorders(MultipleBorders.Top, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Right, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Left, Color.FromName("black"), LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Bottom, Color.FromName("black"), LineStyle.Thin)
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            Next

            For index As Integer = 0 To dtDatos.Columns.Count - 1
                ws.Columns(index).AutoFit()
            Next

            ef.SaveXls(ruta)
        Catch ex As Exception
            Throw New Exception("Al tratar de exportar a Excel: " & ex.Message & ex.StackTrace)
        End Try
    End Sub

#End Region

End Module
