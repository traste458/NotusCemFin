Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.Drawing

Public Class ReporteFaltantes

#Region "Atributos"
    Private _idOrdenRecepcion As Integer
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

    Public Property IdOrdenRecepcion As Integer
        Get
            Return _idOrdenRecepcion
        End Get
        Set(value As Integer)
            _idOrdenRecepcion = value
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

#Region "Metodos"

    Public Function ObtenerInformacionFaltante() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                .TiempoEsperaComando = 300
                If _idOrdenRecepcion > 0 Then .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                If _idOrdenCompra > 0 Then .SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = _idOrdenCompra
                If _numeroOrdenCompra IsNot Nothing Then .SqlParametros.Add("@ordenCompra", SqlDbType.VarChar).Value = _numeroOrdenCompra
                If _idTipoProducto > 0 Then .SqlParametros.Add("@tipoProducto", SqlDbType.Int).Value = _idTipoProducto
                If _estado > 0 Then .SqlParametros.Add("@estado", SqlDbType.Int).Value = _estado
                If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = _fechaInicial
                If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = _fechaFinal
                dtResultado = .ejecutarDataTable("ObtenerInformacionDeReporteDeFaltantes", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    'Private Sub ObtenerDatosFaltantes()
    '    'Dim db As New LMDataAccess

    '    'If _idCentrocosto > 0 Then db.agregarParametroSQL("@idCentrocosto", _idCentrocosto, SqlDbType.Int)
    '    'If _idEventoFacturacion > 0 Then db.agregarParametroSQL("@idEvento", _idEventoFacturacion, SqlDbType.Int)
    '    'If _idTipoProducto > 0 Then db.agregarParametroSQL("@idTipoProducto", _idTipoProducto, SqlDbType.Int)
    '    'db.agregarParametroSQL("@anio", _anio, SqlDbType.Int)
    '    'If _listaMeses IsNot Nothing AndAlso _listaMeses.Count > 0 Then
    '    '    Dim miListaMeses As String = Join(_listaMeses.ToArray(), ",")
    '    '    db.agregarParametroSQL("@listaMeses", miListaMeses)
    '    'End If
    '    'db.TiempoEsperaComando = 600
    '    '_dtDatos = db.ejecutarDataTable("GenerarInformeConsolidadoFacturacionILS", CommandType.StoredProcedure)
    '    '_resultado = New ResultadoProceso
    '    'If _dtDatos.Rows.Count = 0 Then
    '    '    _resultado.Valor = 1
    '    '    _resultado.Mensaje = "No se encontraron registros con los filtros aplicados"
    '    'End If
    'End Sub

    Public Function GenerarInformeExcel(ByVal ruta As String, ByVal _dtDatos As DataTable) As ResultadoProceso
        HerramientasFuncionales.CargarLicenciaGembox()
        Dim miWs As ExcelWorksheet
        Dim miExcel As New ExcelFile
        Dim colInicial As Integer = 1
        Dim filaInicial As Integer = 2
        Dim colFinal As Integer = 0
        Dim dvDatos As New DataView(_dtDatos)

        'Encabezado 
        miWs = miExcel.Worksheets.Add("Diario")
        miWs.Cells("A1").Value = "INFORMES FALTANTES DIARIOS"
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
        miWs.Cells(filaInicial, colInicial).Value = "Material"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Referencia"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "CantidadFaltante"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Guia"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Factura"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Fecha Llegada"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Observacion"
        Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, colInicial, Color.Gainsboro, miWs)
        colFinal = colInicial
        Me.PintarTitulosCeldas(0, 0, 0, colFinal, Color.White, miWs, True)
        miWs.Panes = New WorksheetPanes(PanesState.Frozen, 0, 3, "A4", PanePosition.BottomLeft)
        dvDatos.RowFilter = "fechaLlegada='" & CDate(Today) & "'"
        If dvDatos.Count > 0 Then
            'Cuerpo del Reporte
            filaInicial = 3
            For i As Integer = 0 To dvDatos.Count - 1
                Dim registro As DataRowView = dvDatos(i)
                colInicial = 0
                miWs.Cells(filaInicial, colInicial).Value = registro("material")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("referencia")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("cantidadFaltante")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("guia")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("factura")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("fechaLlegada")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("observacion")
                filaInicial += 1
            Next
        End If
        For i As Integer = 0 To colInicial
            miWs.Columns(i).AutoFit()
        Next
        miWs = miExcel.Worksheets.Add("Historico")
        miWs.Cells("A1").Value = "INFORME FALTANTES HISTORICO"
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
        miWs.Cells(filaInicial, colInicial).Value = "Material"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Referencia"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "CantidadFaltante"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Guia"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Factura"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Fecha Llegada"
        colInicial = colInicial + 1
        miWs.Cells(filaInicial, colInicial).Value = "Observacion"
        Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, colInicial, Color.Gainsboro, miWs)
        colFinal = colInicial
        Me.PintarTitulosCeldas(0, 0, 0, colFinal, Color.White, miWs, True)
        miWs.Panes = New WorksheetPanes(PanesState.Frozen, 0, 3, "A4", PanePosition.BottomLeft)
        dvDatos.RowFilter = "fechaLlegada<>'" & CDate(Today) & "'"
        If dvDatos.Count > 0 Then
            'Cuerpo del Reporte
            filaInicial = 3
            For i As Integer = 0 To dvDatos.Count - 1
                Dim registro As DataRowView = dvDatos(i)
                colInicial = 0
                miWs.Cells(filaInicial, colInicial).Value = registro("material")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("referencia")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("cantidadFaltante")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("guia")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("factura")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("fechaLlegada")
                colInicial += 1
                miWs.Cells(filaInicial, colInicial).Value = registro("observacion")
                filaInicial += 1
            Next
        End If
        For i As Integer = 0 To colInicial
            miWs.Columns(i).AutoFit()
        Next
        _rutaExcel = ruta & "InformeFaltanteDiario.xls"
        miExcel.SaveXls(_rutaExcel)
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