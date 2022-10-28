Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.Drawing
Imports System.Web.UI.WebControls
Imports GemBox

Public Class ReportePruebasMuestreo

#Region "Atributos"
    Private _factura As String
    Private _idOrdenCompra As Integer
    Private _numeroOrdenCompra As String
    Private _idTipoProducto As Integer
    Private _estado As Integer
    Private _idProducto As Integer
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _resultado As ResultadoProceso
    Private _rutaExcel As String
#End Region

#Region "Propiedades"

    Public Property Factura As String
        Get
            Return _factura
        End Get
        Set(value As String)
            _factura = value
        End Set
    End Property

    Public Property IdOrdenCompra As Integer
        Get
            Return _idOrdenCompra
        End Get
        Set(value As Integer)
            _idOrdenCompra = value
        End Set
    End Property

    Public Property NumeroOrdenCompra As String
        Get
            Return _numeroOrdenCompra
        End Get
        Set(value As String)
            _numeroOrdenCompra = value
        End Set
    End Property

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property Estado As Integer
        Get
            Return _estado
        End Get
        Set(value As Integer)
            _estado = value
        End Set
    End Property

    Public Property IdProducto As Integer
        Get
            Return _idProducto
        End Get
        Set(value As Integer)
            _idProducto = value
        End Set
    End Property

    Public Property FechaInicial As Date
        Get
            Return _fechaInicial
        End Get
        Set(value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal As Date
        Get
            Return _fechaFinal
        End Get
        Set(value As Date)
            _fechaFinal = value
        End Set
    End Property

    Public Property RutaExcel As String
        Get
            Return _rutaExcel
        End Get
        Set(value As String)
            _rutaExcel = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos Publicos"

    Public Function ObtenerInformacionMuestreo() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                .TiempoEsperaComando = 300
                If _factura IsNot Nothing Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                If _idOrdenCompra > 0 Then .SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = _idOrdenCompra
                If _numeroOrdenCompra IsNot Nothing Then .SqlParametros.Add("@ordenCompra", SqlDbType.VarChar).Value = _numeroOrdenCompra
                If _idTipoProducto > 0 Then .SqlParametros.Add("@tipoProducto", SqlDbType.Int).Value = _idTipoProducto
                If _estado > 0 Then .SqlParametros.Add("@estado", SqlDbType.Int).Value = _estado
                If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = _fechaInicial
                If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = _fechaFinal
                dtResultado = .ejecutarDataTable("ObtenerInformacionDeReportePruebasMuestreo", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Function GenerarInformeExcel(ByVal ruta As String, ByVal _dtDatos As DataTable, ByVal rutaImagen As String) As ResultadoProceso
        HerramientasFuncionales.CargarLicenciaGembox()
        Dim miWs As ExcelWorksheet
        Dim miExcel As New ExcelFile
        Dim colInicial As Integer = 1
        Dim filaInicial As Integer = 2
        Dim filaFinal As Integer = 0
        Dim colFinal As Integer = 0
        Dim PosicionX As Integer = 0
        Dim PosicionY As Integer = 0
        Dim AnchoImagen As Integer = 0
        Dim AltoImagen As Integer = 0
        Dim dvDatos As New DataView(_dtDatos)
        'Encabezado 
        miWs = miExcel.Worksheets.Add("Muestreo")
        miWs.Cells("A1").Value = "FACTURAS CON MUESTREO"
        With miWs.Cells("A1")
            With .Style
                .Font.Weight = ExcelFont.BoldWeight
                .Font.Size = 14 * 16
                .HorizontalAlignment = HorizontalAlignmentStyle.Center
                .Font.Color = Color.Red
            End With
        End With
        colInicial = 0
        filaInicial = 2
        miWs.Cells(filaInicial, colInicial).Value = "Periodo"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Referencia"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Factura"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Producido"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Unidades Revisadas"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Observación"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Sim Lock"

        Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, colInicial, Color.Gainsboro, miWs)
        colFinal = colInicial
        Me.PintarTitulosCeldas(0, 0, 0, colFinal, Color.White, miWs, True)
        miWs.Panes = New WorksheetPanes(PanesState.Frozen, 0, 3, "A4", PanePosition.BottomLeft)
        If dvDatos.Count > 0 Then
            'Cuerpo del Reporte
            filaInicial = 3
            For i As Integer = 0 To dvDatos.Count - 1
                Dim registro As DataRowView = dvDatos(i)
                colInicial = 0
                miWs.Cells(filaInicial, colInicial).Value = registro("periodo")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("referencia")
                colInicial += 1
                With miWs.Cells.Item(filaInicial, colInicial)
                    .Value = registro("factura")
                    .Style.Font.UnderlineStyle = UnderlineStyle.Single
                    .Style.Font.Color = Color.Blue
                    .Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    .Hyperlink.Location = registro("factura") & "!A1"
                    .Hyperlink.IsExternal = False
                End With
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("cantidadLeida")
                miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("cantidadMuestreo")
                miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("observacion")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("simLock")
                miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                filaInicial += 1
            Next
        End If
        For i As Integer = 0 To colInicial
            miWs.Columns(i).AutoFit()
        Next
        For x As Integer = 0 To _dtDatos.Rows.Count - 1
            Dim _existeHoja As Boolean = False
            For i As Integer = 0 To miExcel.Worksheets.Count - 1
                If miExcel.Worksheets.Item(i).Name = _dtDatos.Rows(x).Item("factura") Then
                    _existeHoja = True
                    Exit For
                Else
                    _existeHoja = False
                End If
            Next
            If Not _existeHoja Then
                miWs = miExcel.Worksheets.Add(_dtDatos.Rows(x).Item("factura"))
                colInicial = 0
                filaInicial = 0
                miWs.Cells(filaInicial, colInicial).Value = "Factura"
                colInicial = colInicial + 1
                miWs.Cells(filaInicial, colInicial).Value = "Referencia"
                Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, colInicial, Color.Gainsboro, miWs)
                With miWs.Cells.Item(filaInicial, 4)
                    .Value = "Volver"
                    .Style.Font.UnderlineStyle = UnderlineStyle.Single
                    .Style.Font.Color = Color.Blue
                    .Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    .Hyperlink.Location = "Muestreo!A1"
                    .Hyperlink.IsExternal = False
                End With
                dvDatos.RowFilter = "factura='" & _dtDatos.Rows(x).Item("factura") & "' and id=" & _dtDatos.Rows(x).Item("Id")
                'Cuerpo del Reporte
                filaInicial = 1
                Dim registro As DataRowView = dvDatos(0)
                colInicial = 0
                miWs.Cells(filaInicial, colInicial).Value = registro("factura")
                miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("referencia")
                miWs.Cells(filaInicial, colInicial).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                colInicial += 1
                filaInicial = 3
                miWs.Cells("A4").Value = "Imagen"
                Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, 4, Color.Gainsboro, miWs, True)
                filaInicial = 4
                Me.PintarTitulosCeldas(4, 0, 15, 4, Color.White, miWs, True)
                filaFinal = 15
                If registro("contentType") <> "" Then
                    PosicionX = 30
                    PosicionY = 80
                    AnchoImagen = 275
                    AltoImagen = 175
                    Dim rc As Rectangle = New Rectangle(PosicionX, PosicionY, AnchoImagen, AltoImagen)
                    miWs.Pictures.Add(rutaImagen & registro("nombreImagen"), rc)
                End If
                For i As Integer = 0 To colInicial
                    miWs.Columns(i).AutoFit()
                Next
            Else
                dvDatos.RowFilter = "factura='" & _dtDatos.Rows(x).Item("factura") & "' and id=" & _dtDatos.Rows(x).Item("Id")
                'Cuerpo del Reporte
                filaInicial = 1
                Dim registro As DataRowView = dvDatos(0)
                filaInicial = filaFinal + 1
                filaFinal += 11
                Me.PintarTitulosCeldas(filaInicial, 0, filaFinal, 4, Color.White, miWs, True)
                filaInicial = filaFinal
                If registro("contentType") <> "" Then
                    PosicionY = PosicionY + 190
                    '325
                    Dim rc As Rectangle = New Rectangle(PosicionX, PosicionY, AnchoImagen, AltoImagen)
                    miWs.Pictures.Add(rutaImagen & registro("nombreImagen"), rc)
                End If
            End If
        Next
        _rutaExcel = ruta & "MuestreoDeSeriales.xlsx"
        miExcel.SaveXlsx(_rutaExcel)
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

#End Region

#Region "Metodos Privados"

    Private Sub PintarTitulosCeldas(ByVal filaInicial As Integer, ByVal columnaInicial As Integer, ByVal filaFinal As Integer, ByVal columnaFinal As Integer, ByVal colorFondo As Color, ByVal miWS As ExcelWorksheet, Optional ByVal merge As Boolean = False, Optional ByVal alineacion As HorizontalAlignmentStyle = HorizontalAlignmentStyle.Center)
        Dim cr As CellRange = miWS.Cells.GetSubrangeAbsolute(filaInicial, columnaInicial, filaFinal, columnaFinal)
        cr.Merged = merge
        For Each cel As ExcelCell In cr
            With cel.Style
                .FillPattern.SetPattern(FillPatternStyle.Solid, colorFondo, colorFondo)
                .Font.Weight = ExcelFont.BoldWeight
                .HorizontalAlignment = HorizontalAlignmentStyle.Center
                .Borders.SetBorders(MultipleBorders.Top, Color.Gray, LineStyle.Thin)
                .Borders.SetBorders(MultipleBorders.Right, Color.Gray, LineStyle.Thin)
                .Borders.SetBorders(MultipleBorders.Left, Color.Gray, LineStyle.Thin)
                .Borders.SetBorders(MultipleBorders.Bottom, Color.Gray, LineStyle.Thin)
                .HorizontalAlignment = alineacion
            End With
        Next
    End Sub

#End Region

End Class
