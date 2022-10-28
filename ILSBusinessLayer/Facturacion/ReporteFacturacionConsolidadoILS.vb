Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.Drawing

Public Class ReporteFacturacionConsolidadoILS
#Region "Atriburos"
    Private _idCentrocosto As Integer
    Private _idEventoFacturacion As Integer
    Private _idTipoProducto As Integer
    Private _anio As Integer
    Private _listaMeses As ArrayList
    Private _rutaExcel As String
    Private _dtDatos As DataTable
    Private _resultado As ResultadoProceso
    Private _auxEvento As Integer = 0
    Private _auxTipoProducto As Integer = 0
    Private _auxIdProductoFacturable As Integer
#End Region

#Region "Propiedades"
    Public Property IdCentroCosto() As Integer
        Get
            Return _idCentrocosto
        End Get
        Set(ByVal value As Integer)
            _idCentrocosto = value
        End Set
    End Property

    Public Property IdEventoFacturacion() As Integer
        Get
            Return _idEventoFacturacion
        End Get
        Set(ByVal value As Integer)
            _idEventoFacturacion = value
        End Set
    End Property

    Public Property IdTipoProducto() As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(ByVal value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property Anio() As Integer
        Get
            Return _anio
        End Get
        Set(ByVal value As Integer)
            _anio = value
        End Set
    End Property


    Public Property Listameses() As ArrayList
        Get
            Return _listaMeses
        End Get
        Set(ByVal value As ArrayList)
            _listaMeses = value
        End Set
    End Property

    Public ReadOnly Property RutaExcel() As String
        Get
            Return _rutaExcel
        End Get
    End Property

    Public ReadOnly Property DatosConsolidado() As DataTable
        Get
            If _dtDatos Is Nothing Then ObtenerDatosConsolidado()
            Return _dtDatos
        End Get
    End Property
#End Region

#Region "Metodos"

    Private Sub ObtenerDatosConsolidado()
        Dim db As New LMDataAccess

        If _idCentrocosto > 0 Then db.agregarParametroSQL("@idCentrocosto", _idCentrocosto, SqlDbType.Int)
        If _idEventoFacturacion > 0 Then db.agregarParametroSQL("@idEvento", _idEventoFacturacion, SqlDbType.Int)
        If _idTipoProducto > 0 Then db.agregarParametroSQL("@idTipoProducto", _idTipoProducto, SqlDbType.Int)
        db.agregarParametroSQL("@anio", _anio, SqlDbType.Int)
        If _listaMeses IsNot Nothing AndAlso _listaMeses.Count > 0 Then
            Dim miListaMeses As String = Join(_listaMeses.ToArray(), ",")
            db.agregarParametroSQL("@listaMeses", miListaMeses)
        End If
        _dtDatos = db.ejecutarDataTable("GenerarInformeConsolidadoFacturacionILS", CommandType.StoredProcedure)
        _resultado = New ResultadoProceso
        If _dtDatos.Rows.Count = 0 Then
            _resultado.Valor = 1
            _resultado.Mensaje = "No se encontraron registros con los filtros aplicados"
        End If
    End Sub

    Public Function GenerarInformeExcel(ByVal ruta As String) As ResultadoProceso
        If _dtDatos Is Nothing Then ObtenerDatosConsolidado()
        If _resultado.Valor > 0 Then
            Return _resultado
        End If
        HerramientasFuncionales.CargarLicenciaGembox()
        _auxIdProductoFacturable = 0
        Dim miWs As ExcelWorksheet
        Dim miExcel As New ExcelFile
        Dim dtRegiones As DataTable = Region.ObtenerTodas()
        Dim colInicial As Integer = 1
        Dim filaInicial As Integer = 2
        Dim colFinal As Integer = 0

        For Each mes As Integer In _listaMeses
            Dim dvDatos As New DataView(_dtDatos)
            dvDatos.RowFilter = "mesFactura=" & mes.ToString()
            'Encabezado 
            If dvDatos.Count > 0 Then
                miWs = miExcel.Worksheets.Add("Consolidado " & MonthName(mes, True).ToUpper() & " " & _anio.ToString())
                miWs.Cells("A1").Value = "Consolidado de " & MonthName(mes).ToUpper() & " - " & _anio
                miWs.Cells("A4").Value = "PRODUCTOS FACTURABLES"
                With miWs.Cells("A1")
                    With .Style
                        .Font.Weight = ExcelFont.BoldWeight
                        .Font.Size = 16 * 18
                        .HorizontalAlignment = HorizontalAlignmentStyle.Center
                    End With
                End With
                colInicial = 1
                filaInicial = 2

                For Each filaRegion As DataRow In dtRegiones.Rows
                    miWs.Cells(filaInicial, colInicial).Value = filaRegion("nombreRegion")
                    Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 2, Color.Gainsboro, miWs, True)
                    miWs.Cells(filaInicial + 1, colInicial).Value = "Unidades"
                    miWs.Cells(filaInicial + 1, colInicial + 1).Value = "VR. Unidad"
                    miWs.Cells(filaInicial + 1, colInicial + 2).Value = "VR. Total"
                    colInicial += 3
                Next
                miWs.Cells(filaInicial, colInicial).Value = "Total Factura"
                miWs.Cells(filaInicial + 1, colInicial).Value = "Unidades"
                miWs.Cells(filaInicial + 1, colInicial + 1).Value = "Pesos"
                Me.PintarTitulosCeldas(filaInicial, 0, filaInicial + 2, colInicial + 1, Color.Gainsboro, miWs)
                colFinal = colInicial + 1
                Me.PintarTitulosCeldas(0, 0, 0, colFinal, Color.White, miWs, True)
                miWs.Panes = New WorksheetPanes(PanesState.Frozen, 0, 4, "A5", PanePosition.BottomLeft)
                'Cuerpo del Reporte
                filaInicial = 4
                _auxEvento = 0
                _auxTipoProducto = 0
                For i As Integer = 0 To dvDatos.Count - 1
                    Dim registro As DataRowView = dvDatos(i)
                    colInicial = 1
                    If _auxEvento <> registro("idEvento") Then
                        _auxEvento = registro("idEvento")
                        miWs.Cells(filaInicial, 0).Value = registro("evento")
                        Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, colFinal, Color.Silver, miWs, True)
                        filaInicial += 1
                    End If
                    If _auxTipoProducto <> registro("idTipoProducto") Then
                        _auxTipoProducto = registro("idTipoProducto")
                        miWs.Cells(filaInicial, 0).Value = registro("nombreTipoProducto")
                        Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, 0, Color.Gainsboro, miWs)
                        miWs.Columns(0).AutoFit()
                        filaInicial += 1
                    End If
                    miWs.Cells(filaInicial, 0).Value = registro("nombreProductoFacturable")
                    Dim incrementador As Integer = 1
                    For Each filaRegion As DataRow In dtRegiones.Rows
                        Dim cantidad As Integer
                        Integer.TryParse(registro("CANTIDAD_" & filaRegion("nombreRegion")).ToString(), cantidad)
                        If cantidad > 0 Then
                            miWs.Rows(filaInicial).Style.NumberFormat = "#,##0"
                            miWs.Cells(filaInicial, colInicial).Value = registro("CANTIDAD_" & filaRegion("nombreRegion"))
                            miWs.Cells(filaInicial, colInicial + 1).Value = registro("tarifa")
                            miWs.Cells(filaInicial, colInicial + 2).Value = registro("VR_" & filaRegion("nombreRegion"))
                        End If
                        _auxIdProductoFacturable = registro("idProductoFacturable")
                        If i < dvDatos.Count - 1 Then
                            If dvDatos(i + 1)("idTipoProducto") <> _auxTipoProducto Then
                                Me.TotalizarSeccion(filaInicial + 1, colInicial, filaInicial + 1, colFinal, miWs, filaRegion("nombreRegion"), registro("nombreTipoProducto"), dvDatos.RowFilter)
                                incrementador = 2
                            Else
                                incrementador = 1
                            End If
                        Else
                            Me.TotalizarSeccion(filaInicial + 1, colInicial, filaInicial + 1, colFinal, miWs, filaRegion("nombreRegion"), registro("nombreTipoProducto"), dvDatos.RowFilter)
                            Me.TotalizarSeccion(filaInicial + 2, colInicial, filaInicial + 2, colFinal, miWs, filaRegion("nombreRegion"), registro("nombreTipoProducto"), dvDatos.RowFilter, esGranTotal:=True)
                        End If
                        colInicial += 3
                    Next
                    miWs.Cells(filaInicial, colInicial).Value = registro("undTotal")
                    miWs.Cells(filaInicial, colInicial + 1).Value = registro("pesosTotal")
                    filaInicial += incrementador
                Next
            End If
        Next
        Me.ObtenerSoportes(miExcel)
        _rutaExcel = ruta & "ReporteFacturacionConsolidadoILS.xls"
        miExcel.SaveXls(_rutaExcel)
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

#End Region

    Public Sub New()
        _listaMeses = New ArrayList
    End Sub

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

    Private Sub TotalizarSeccion(ByVal filaInicial As Integer, ByVal columnaInicial As Integer, ByVal filaFinal As Integer, ByVal columnaFinal As Integer, ByVal miWS As ExcelWorksheet, ByVal region As String, ByVal tipoProducto As String, ByVal filtroActual As String, Optional ByVal esGranTotal As Boolean = False)
        Dim filtro As String
        Dim colorFondo As Color
        If esGranTotal Then
            miWS.Cells(filaInicial, 0).Value = "TOTAL FACTURACIÓN"
            filtro = filtroActual
            colorFondo = Color.Gray
        Else
            miWS.Cells(filaInicial, 0).Value = "TOTAL " & tipoProducto
            filtro = filtroActual & " AND idTipoProducto=" & _auxTipoProducto.ToString() & _
                " AND idEvento=" & _auxEvento.ToString() & " AND idProductoFacturable=" & _auxIdProductoFacturable.ToString
            colorFondo = Color.WhiteSmoke
        End If
        miWS.Rows(filaInicial).Style.NumberFormat = "#,##0"
        miWS.Cells(filaInicial, columnaInicial).Value = _dtDatos.Compute("SUM(" & "CANTIDAD_" & region & ")", filtro)
        miWS.Cells(filaInicial, columnaInicial + 2).Value = _dtDatos.Compute("SUM(" & "VR_" & region & ")", filtro)

        miWS.Cells(filaInicial, columnaFinal - 1).Value = _dtDatos.Compute("SUM(undTotal)", filtro)
        miWS.Cells(filaInicial, columnaFinal).Value = _dtDatos.Compute("SUM(pesosTotal)", filtro)
        Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, columnaFinal, colorFondo, miWS, alineacion:=HorizontalAlignmentStyle.Right)
    End Sub

    Private Sub ObtenerSoportes(ByVal miExcel As ExcelFile)
        Dim soportes As New SoporteReporteFacturacionILS
        With soportes
            .IdTipoProducto = _idTipoProducto
            .IdEventoFacturacion = _idEventoFacturacion
            .IdCentroCosto = _idCentrocosto
            .Anio = _anio
            .Listameses = _listaMeses
        End With

        Try
            Dim excelSoportes As ExcelFile = soportes.ObtenerHojasExcel()
            For Each hoja As ExcelWorksheet In excelSoportes.Worksheets
                miExcel.Worksheets.AddCopy(hoja.Name, hoja)
            Next
        Catch ex As Exception
            Throw New Exception("Error al obtener la información de soportes. " & ex.Message, ex)
        End Try
    End Sub

End Class

