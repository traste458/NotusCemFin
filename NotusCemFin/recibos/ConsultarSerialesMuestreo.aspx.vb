Imports ILSBusinessLayer.OMS
Imports ILSBusinessLayer.Estructuras
Imports GemBox.Spreadsheet
Imports System.IO

Partial Public Class ConsultarSerialesMuestreo
    Inherits System.Web.UI.Page

#Region "Variables"

    Private Shared RutaArchivo As String = "~\archivos_planos\"
    Private Shared RutaLocal As String = HttpContext.Current.Server.MapPath(RutaArchivo)
    Private Shared RutaRelativa As String = "~/../../archivos_planos/"

#End Region

#Region "Eventos"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Seguridad.verificarSession(Me)
        epMuestreo.clear()
        If Not IsPostBack Then
            epMuestreo.setTitle("Consultar Seriales de Muestreo")
            epMuestreo.showReturnLink("ConsultarFacturaGuia.aspx")
            MetodosComunes.setGemBoxLicense()
            CargarDatos()
        End If
    End Sub

    Protected Sub lnkDescargarExcel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkDescargarExcel.Click
        If Session("datosExcel") IsNot Nothing Then
            Dim nombreArchivo As String = RutaLocal & GenerarArchivo(Session("datosExcel"), "ReporteSerialesMuestreo.xls")
            If File.Exists(nombreArchivo) Then
                MetodosComunes.forzarDescargaDeArchivo(HttpContext.Current, nombreArchivo)
            Else
                epMuestreo.showWarning("Imposible recuperar el archivo desde su ruta de almacenamiento. Por favor intente nuevamente.")
            End If
        Else
            epMuestreo.showWarning("No se pudo recuperar la información del reporte desde la memoria. Por favor intente nuevamente")
        End If
    End Sub

    Private Sub grvMuestreo_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grvMuestreo.PageIndexChanging
        grvMuestreo.PageIndex = e.NewPageIndex
        CargarDatos()
    End Sub

#End Region

#Region "Metodos"

    Private Sub CargarDatos()
        Dim dtDatos As New DataTable()
        Dim filtro As New FiltroSerialMuestra

        If Session("Factura") IsNot Nothing And Session("Guia") IsNot Nothing Then
            filtro.IdFactura = CLng(Session("Factura").ToString)
            filtro.IdGuia = CLng(Session("guia").ToString)
        Else
            filtro.IdFactura = 0
            filtro.IdGuia = 0
        End If

        Try
            dtDatos = SerialMuestra.ObtenerListado(filtro)

            If dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then
                EnlazarDatos(grvMuestreo, dtDatos)
                CambiarNombreColumnas(dtDatos)
                Session("datosExcel") = dtDatos
            Else
                epMuestreo.showWarning("No se encontraron registros que concuerden con los filtros.")
            End If
        Catch ex As Exception
            epMuestreo.showError("Error al tratar de generar reporte. " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Carga una grilla a partir de un datatable
    ''' </summary>
    ''' <param name="grv">grilla donde se carga la informacion</param>
    ''' <param name="dtDatos">datos que deben va a cargar</param>
    ''' <remarks></remarks>
    Private Sub EnlazarDatos(ByRef grv As GridView, ByVal dtDatos As DataTable)
        With grv
            .DataSource = dtDatos
            If dtDatos.Rows.Count > 0 Then
                .Columns(0).FooterText = dtDatos.Rows.Count.ToString & " Registro(s) Encontrado(s)"
            End If
            .DataBind()
        End With
        MetodosComunes.mergeGridViewFooter(grv)
    End Sub

    ''' <summary>
    ''' Cambia el nombre de las columnas del datatable
    ''' </summary>
    ''' <param name="dt">datatable origen</param>
    ''' <remarks></remarks>
    Private Sub CambiarNombreColumnas(ByRef dt As DataTable)
        If (dt IsNot Nothing AndAlso dt.Rows.Count > 0) Then
            For index As Integer = 0 To dt.Rows.Count - 1
                If Not IsDBNull(dt.Rows(index)("fecha")) And IsDate(dt.Rows(index)("fecha")) Then dt.Rows(index)("fecha") = CDate(dt.Rows(index)("fecha")).ToString("dd/MM/yyyy hh:mm tt")
            Next

            With dt.Columns
                .Item("factura").ColumnName = "FACTURA"
                .Item("guia").ColumnName = "GUÍA"
                .Item("serial").ColumnName = "SERIAL MUESTREADO"
                .Item("orden").ColumnName = "ORDEN"
                .Item("fecha").ColumnName = "FECHA"
            End With
        End If
    End Sub

    ''' <summary>
    ''' Genera un archivo excel a partir de los datos suministrados, y el nombre del archivo.
    ''' </summary>
    ''' <param name="dtDatos">Información que se carga en el excel</param>
    ''' <param name="nombreArchivo">Nombre asignado al archivo</param>
    ''' <returns>Nombre del archivo generado</returns>
    ''' <remarks></remarks>
    Public Function GenerarArchivo(ByVal dtDatos As DataTable, ByVal nombreArchivo As String) As String
        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
        Dim ruta As String = RutaLocal & nombreArchivo
        Dim oExcel As New ExcelFile

        ObtenerDatosHojas(oExcel, dtDatos, "Seriales")

        oExcel.SaveXls(ruta)
        Return nombreArchivo
    End Function

    ''' <summary>
    ''' Obtiene las cantidad de hojas y la información que se adiciona por hoja.
    ''' </summary>
    ''' <param name="oExcel">Objeto excel donde se adicionan las hojas</param>
    ''' <param name="dtDatos">información que se adiciona a las hojas</param>
    ''' <param name="nombreHoja">Nombre que se le da a la hoja</param>
    ''' <remarks></remarks>
    Private Sub ObtenerDatosHojas(ByRef oExcel As ExcelFile, ByVal dtDatos As DataTable, ByVal nombreHoja As String)
        Dim oWs As ExcelWorksheet
        Dim numRows As Integer = 60000
        Dim maxIndex As Integer = dtDatos.Rows.Count - 1
        Dim numHojas As Integer = Math.Ceiling((dtDatos.Rows.Count / numRows))
        Dim fila As Integer = 0
        Dim ind As Integer
        Dim maxRow As Integer
        Dim dtAux As DataTable = dtDatos.Clone
        Dim nombre As String = String.Empty

        For index As Integer = 1 To numHojas
            dtAux.Rows.Clear()

            If numHojas > 1 Then
                nombre = nombreHoja & "_" & index.ToString
            Else
                nombre = nombreHoja
            End If

            oWs = oExcel.Worksheets.Add(nombre)

            maxRow = Math.Min((ind + (numRows - 1)), maxIndex)
            For ind = fila To maxRow
                dtAux.ImportRow(dtDatos.Rows(ind))
            Next
            fila = ind
            AdicionarDatosAHoja(oWs, dtAux)
        Next
    End Sub

    ''' <summary>
    ''' Adiciona los datos a la hoja referenciada 
    ''' </summary>
    ''' <param name="OWs">Objeto hoja excel a la cual se le agrega la información</param>
    ''' <param name="dtDatos">Información que contendra la hoja</param>
    ''' <param name="arrNombreColumna">Arreglo con los nombres de columnas que se adicionara a la hoja. Es opcional</param>
    ''' <remarks></remarks>
    Private Sub AdicionarDatosAHoja(ByVal OWs As ExcelWorksheet, ByVal dtDatos As DataTable, Optional ByVal arrNombreColumna As ArrayList = Nothing)
        OWs.InsertDataTable(dtDatos, "A1", True)
        For i As Integer = 0 To dtDatos.Columns.Count - 1
            If arrNombreColumna IsNot Nothing Then OWs.Cells(0, i).Value = arrNombreColumna(i)
            With OWs.Cells(0, i).Style
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
        OWs.Cells.GetSubrangeAbsolute(dtDatos.Rows.Count + 1, 0, (dtDatos.Rows.Count + 1), dtDatos.Columns.Count - 1).Merged = True
        With OWs.Cells("A" & (dtDatos.Rows.Count + 2).ToString).Style
            .FillPattern.SetPattern(FillPatternStyle.Solid, Color.LightGray, Color.LightGray)
            .Font.Color = Color.DarkBlue
            .Font.Weight = ExcelFont.BoldWeight
            .Borders.SetBorders(MultipleBorders.Top, Color.FromName("black"), LineStyle.Thin)
            .Borders.SetBorders(MultipleBorders.Right, Color.FromName("black"), LineStyle.Thin)
            .Borders.SetBorders(MultipleBorders.Left, Color.FromName("black"), LineStyle.Thin)
            .Borders.SetBorders(MultipleBorders.Bottom, Color.FromName("black"), LineStyle.Thin)
            .HorizontalAlignment = HorizontalAlignmentStyle.Center
        End With
        OWs.Cells("A" & (dtDatos.Rows.Count + 2).ToString).Value = dtDatos.Rows.Count & " Registro(s)"

        For index As Integer = 0 To dtDatos.Columns.Count - 1
            OWs.Columns(index).AutoFitAdvanced(1)
        Next
    End Sub

#End Region

End Class