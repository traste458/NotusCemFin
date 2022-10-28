Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes
Imports System.Web
Imports System.IO
Imports GemBox.Spreadsheet
Imports System.Drawing

Namespace MensajeriaEspecializada

    ''' <summary>
    ''' Author: Beltrán, Diego
    ''' Date:   15/12/2014
    ''' Description: Clase diseñada para generar el reporte de Excel de generación de adendos para el proceso de venta corporativa
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ReporteAdendoVentaCorporativa

#Region "Atributos"

        Private _idServicio As Integer
        Private _dtDatosReporte As DataTable
        Private _dtDatosReporteAux As DataTable
        Private _rutaArchivo As String
        Private _cliente As String
        Private _nit As String
        Private _direccion As String
        Private _ciudad As String
        Private _departamento As String
        Private _telefono As String
        Private _representanteLegal As String
        Private _identificacionRepresentanteLegal As String
        Private _fecha As String

#End Region

#Region "Propiedades"

        Public Property IdServicio As Integer
            Get
                Return _idServicio
            End Get
            Set(value As Integer)
                _idServicio = value
            End Set
        End Property

        Public Property DatosReporte As DataTable
            Get
                If IsNothing(_dtDatosReporte) Then _dtDatosReporte = New DataTable
                Return _dtDatosReporte
            End Get
            Set(value As DataTable)
                _dtDatosReporte = value
            End Set
        End Property

        Public Property DatosReporteAux As DataTable
            Get
                If IsNothing(_dtDatosReporteAux) Then _dtDatosReporteAux = New DataTable
                Return _dtDatosReporteAux
            End Get
            Set(value As DataTable)
                _dtDatosReporteAux = value
            End Set
        End Property

        Public Property RutaArchivo As String
            Get
                Return _rutaArchivo
            End Get
            Set(value As String)
                _rutaArchivo = value
            End Set
        End Property

        Public Property Cliente As String
            Get
                Return _cliente
            End Get
            Set(value As String)
                _cliente = value
            End Set
        End Property

        Public Property Nit As String
            Get
                Return _nit
            End Get
            Set(value As String)
                _nit = value
            End Set
        End Property

        Public Property Direccion As String
            Get
                Return _direccion
            End Get
            Set(value As String)
                _direccion = value
            End Set
        End Property

        Public Property Ciudad As String
            Get
                Return _ciudad
            End Get
            Set(value As String)
                _ciudad = value
            End Set
        End Property

        Public Property Departamento As String
            Get
                Return _departamento
            End Get
            Set(value As String)
                _departamento = value
            End Set
        End Property

        Public Property Telefono As String
            Get
                Return _telefono
            End Get
            Set(value As String)
                _telefono = value
            End Set
        End Property

        Public Property RepresentanteLegal As String
            Get
                Return _representanteLegal
            End Get
            Set(value As String)
                _representanteLegal = value
            End Set
        End Property

        Public Property IdentificacionRepresentanteLegal As String
            Get
                Return _identificacionRepresentanteLegal
            End Get
            Set(value As String)
                _identificacionRepresentanteLegal = value
            End Set
        End Property

        Public Property Fecha As String
            Get
                Return _fecha
            End Get
            Set(value As String)
                _fecha = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function GenerarReporteExcel() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim FolderTempImage As String
            Try
                If _dtDatosReporte.Rows.Count > 0 Then
                    Dim contexto As HttpContext = HttpContext.Current
                    Dim fullPath As String
                    FolderTempImage = Guid.NewGuid().ToString()
                    fullPath = contexto.Server.MapPath("~/Reportes/Archivos/")
                    Directory.CreateDirectory(contexto.Server.MapPath("~/Reportes/Archivos/") & FolderTempImage)
                    Dim ruta As String = fullPath & FolderTempImage
                    Dim resul As ResultadoProceso
                    resul = GenerarInformeExcel(fullPath, ruta & "\", fullPath)
                    Directory.Delete(ruta, True)
                    If resul.Valor <> 0 Then
                        resultado.EstablecerMensajeYValor(1, "No se pudo generar el reporte, por favor intentelo nuevamente.")
                    End If
                Else
                    resultado.EstablecerMensajeYValor(1, "No existen registros para exportar")
                End If

            Catch ex As Exception
                Throw New ApplicationException("Error al Generar El ReporteExcel:(", ex)
            End Try
            Return resultado
        End Function

#End Region

#Region "Métodos Privados"

        Private Function GenerarInformeExcel(ByVal ruta As String, ByVal rutaImagen As String, ByVal RutaArchivo As String) As ResultadoProceso
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim resultado As New ResultadoProceso
            Dim miWs As ExcelWorksheet
            Dim contexto As HttpContext = HttpContext.Current
            Dim fullPath As String
            fullPath = contexto.Server.MapPath("~/MensajeriaEspecializada/Plantillas/")
            Dim miExcel As New ExcelFile
            miExcel.LoadXlsx(fullPath & "PlantillaAdendo3.xlsx", XlsxOptions.None)
            Dim templateSheet As ExcelWorksheet = miExcel.Worksheets(0)
            Dim colInicial As Integer = 1
            Dim filaconteohoja As Integer = 0
            Dim filaInicial As Integer = 2
            Dim filaFinal As Integer = 0
            Dim colFinal As Integer = 0
            Dim PosicionX As Integer = 0
            Dim PosicionY As Integer = 0
            Dim AnchoImagen As Integer = 0
            Dim AltoImagen As Integer = 0
            Dim dvDatos As New DataView

            Dim dvAux As DataView = _dtDatosReporte.DefaultView
            For y As Integer = 0 To _dtDatosReporte.Rows.Count - 1
                dvAux.RowFilter = "hoja= " & y + 1
                If dvAux.Count > 0 Then
                    _dtDatosReporteAux = dvAux.ToTable()
                    'Encabezado
                    miExcel.Worksheets.AddCopy("Adendo" & (y + 2), miExcel.Worksheets(y))
                    miWs = miExcel.Worksheets(y)

                    miWs.Cells("I2").Value = y + 1

                    miWs.Cells("K2").Value = _dtDatosReporte.Compute("MAX(hoja)", "")

                    miWs.Cells("A13").Value = (y * 15) + 1

                    miWs.Cells("G2").Value = _dtDatosReporte.Rows(y).Item("nombreCanalVenta")

                    miWs.Cells("G3").Value = _dtDatosReporte.Rows(y).Item("codigoCanalVenta")

                    miWs.Cells("I67").Value = _cliente.ToUpper()

                    miWs.Cells("S67").Value = _nit

                    miWs.Cells("L64").Value = _representanteLegal.ToUpper()

                    miWs.Cells("S64").Value = _identificacionRepresentanteLegal

                    miWs.Cells("I70").Value = _direccion.ToUpper()

                    miWs.Cells("O70").Value = _ciudad.ToUpper()

                    miWs.Cells("S70").Value = _telefono

                    miWs.Cells("G4").Value = _dtDatosReporte.Rows(y).Item("solicitudServicioNumero")

                    miWs.Cells("T2").Value = _idServicio

                    miWs.Cells("G5").Value = _dtDatosReporte.Rows(y).Item("contratoCompraVentaEquipo")

                    Dim fechaArray() As String = _fecha.Split(New Char() {"/"c})

                    If fechaArray.Length = 3 Then
                        miWs.Cells("I3").Value = fechaArray(0)
                        miWs.Cells("J3").Value = fechaArray(1)
                        fechaArray = fechaArray(2).Split(New Char() {" "c})
                        miWs.Cells("K3").Value = fechaArray(0)
                    End If

                    miWs.Cells("A70").Value = _dtDatosReporte.Rows(y).Item("nombreEjecutivoVenta")
                    miWs.Cells("S29").Value = _dtDatosReporte.Rows(y).Item("VentaTotal")
                    PosicionX = 75
                    PosicionY = 30
                    AnchoImagen = 60
                    AltoImagen = 60

                    Dim _impuestosValorBasico As Decimal = 0
                    Dim _impuestosConsumo As Decimal = 0
                    If _dtDatosReporteAux.Rows.Count > 0 Then
                        'Cuerpo del Reporte
                        filaInicial = 12
                        For i As Integer = 0 To _dtDatosReporteAux.Rows.Count - 1
                            miWs.Cells("I5").Value = _dtDatosReporteAux.Rows(i).Item("codigoCuenta")
                            Filaconteohoja = Filaconteohoja + 1
                            colInicial = 0
                            miWs.Cells(filaInicial, colInicial).Value = Filaconteohoja
                            colInicial += 1
                            miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("msisdn")
                            colInicial += 2
                            miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("Imei")
                            colInicial += 1
                            miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("Iccid")
                            colInicial += 1
                            miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("descripcionImei")
                            colInicial += 1
                            miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("nombrePlan")
                            colInicial += 1
                            miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("paquete")
                            colInicial += 1
                            If _dtDatosReporteAux.Rows(i).Item("clausula").ToString.ToLower.Trim = "si" Then
                                miWs.Cells(filaInicial, colInicial).Value = "X"
                                colInicial += 2
                            Else
                                colInicial += 1
                                miWs.Cells(filaInicial, colInicial).Value = "X"
                                colInicial += 1
                            End If

                            If _dtDatosReporteAux.Rows(i).Item("clausula").ToString.ToLower.Trim = "si" Then
                                miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("valorClausula")
                            Else
                                miWs.Cells(filaInicial, colInicial).Value = ""
                            End If
                            colInicial += 1
                            If _dtDatosReporteAux.Rows(i).Item("ventaEquipoContado").ToString.ToLower.Trim = "si" Then
                                miWs.Cells(filaInicial, colInicial).Value = "X" 'contado
                            End If
                            colInicial += 1
                            If _dtDatosReporteAux.Rows(i).Item("ventaEquipoCuotas").ToString.ToLower.Trim = "si" Then
                                miWs.Cells(filaInicial, colInicial).Value = "X" 'contado
                            End If
                            'valor inicial 
                            colInicial += 2
                            miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("numeroCuotasVenta")
                            colInicial += 2
                            With miWs.Cells(filaInicial, colInicial)
                                '.Value = CInt(_dtDatosReporteAux.Rows(i).Item("valorSim")) + (CInt(_dtDatosReporteAux.Rows(i).Item("valorSim")) * 16 / 100)
                                .Value = CInt(_dtDatosReporteAux.Rows(i).Item("valorSim")) + (CInt(_dtDatosReporteAux.Rows(i).Item("IvaSim")))
                                .Style.NumberFormat = "$ #,##0"
                            End With
                            colInicial += 1
                            With miWs.Cells(filaInicial, colInicial)
                                '.Value = CInt(_dtDatosReporteAux.Rows(i).Item("valorEquipo")) + (CInt(_dtDatosReporteAux.Rows(i).Item("valorEquipo")) * 16 / 100)
                                .Value = CInt(_dtDatosReporteAux.Rows(i).Item("valorEquipo")) + (CInt(_dtDatosReporteAux.Rows(i).Item("IvaEquipo")))
                                .Style.NumberFormat = "$ #,##0"
                            End With
                            colInicial += 1
                            With miWs.Cells(filaInicial, colInicial)
                                .Value = CInt(_dtDatosReporteAux.Rows(i).Item("valorCargoBasicoPlanSinImpuesto"))
                                .Style.NumberFormat = "$ #,##0"
                            End With
                            colInicial += 1
                            If (_dtDatosReporteAux.Rows(i).Item("tipoPlanVozDatos").ToString.ToLower.Trim) = "voz" Then
                                _impuestosValorBasico = _impuestosValorBasico + ((_dtDatosReporteAux.Rows(i).Item("valorCargoBasicoPlanSinImpuesto") * 16) / 100)
                                _impuestosConsumo = ((_dtDatosReporteAux.Rows(i).Item("valorCargoBasicoPlanSinImpuesto") * 4) / 100)
                            ElseIf (_dtDatosReporteAux.Rows(i).Item("tipoPlanVozDatos").ToString.ToLower.Trim) = "datos" Then
                                _impuestosValorBasico = _impuestosValorBasico + ((_dtDatosReporteAux.Rows(i).Item("valorCargoBasicoPlanSinImpuesto") * 16) / 100)
                                _impuestosConsumo = 0
                            End If

                            With miWs.Cells(filaInicial, colInicial)
                                .Value = (CInt(_dtDatosReporteAux.Rows(i).Item("valorCargoBasicoPlanSinImpuesto")) * 16 / 100)
                                .Style.NumberFormat = "$ #,##0"
                            End With

                            colInicial += 1
                            With miWs.Cells(filaInicial, colInicial)
                                .Value = _impuestosConsumo
                                .Style.NumberFormat = "$ #,##0"
                            End With
                            colInicial += 1
                            With miWs.Cells(filaInicial, colInicial)
                                .Formula = " SUM(Q" & (filaInicial + 1) & ":U" & (filaInicial + 1) & ")"
                                .Style.NumberFormat = "$ #,##0"
                            End With

                            'miWs.Cells("M30").Value = _impuestosConsumo
                            'miWs.Cells("M30").Style.NumberFormat = "$ #,##0,00"
                            'miWs.Cells("M31").Value = _impuestosValorBasico
                            'miWs.Cells("M31").Style.NumberFormat = "$ #,##0,00"
                            'colInicial += 1

                            'miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("IvaSim") 'Iva Sim
                            'colInicial += 1
                            'miWs.Cells(filaInicial, colInicial).Value = _dtDatosReporteAux.Rows(i).Item("IvaEquipo") 'Iva Equipo
                            Dim fila As Integer = filaInicial + 1
                            filaInicial = fila
                        Next
                    Else
                        filaInicial = filaInicial + 1
                    End If
                Else
                    y = _dtDatosReporte.Rows.Count
                End If

            Next
            Dim nombre As String
            Dim _idUsuario As String
            _idUsuario = HttpContext.Current.Session("usxp001")
            Dim fecha As DateTime = DateTime.Now
            Dim fec As String = fecha.ToString("HH:mm:ss:fff").Replace(":", "_")

            _rutaArchivo = RutaArchivo & "Adendo Servicio" & "_" & _idUsuario & "_" & fec & ".xlsx"
            miExcel.SaveXlsx(_rutaArchivo)
            resultado = New ResultadoProceso
            resultado.Valor = 0
            resultado.Mensaje = "Se ha generado el archivo correctamente"
            Return resultado
        End Function

        Private Sub PintarTitulosCeldas(ByVal filaInicial As Integer, ByVal columnaInicial As Integer, ByVal filaFinal As Integer, ByVal columnaFinal As Integer, ByVal colorFondo As Color, ByVal miWS As ExcelWorksheet, Optional ByVal merge As Boolean = False, Optional ByVal alineacion As HorizontalAlignmentStyle = HorizontalAlignmentStyle.Center, Optional ByVal celdas As Boolean = True, Optional ByVal colorLetra As Boolean = False)
            Dim cr As CellRange = miWS.Cells.GetSubrangeAbsolute(filaInicial, columnaInicial, filaFinal, columnaFinal)
            cr.Merged = merge
            For Each cel As ExcelCell In cr
                With cel.Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                    If colorLetra Then
                        .Font.Color = Color.White
                    End If
                    If celdas Then
                        .FillPattern.SetPattern(FillPatternStyle.Solid, colorFondo, colorFondo)
                        .Borders.SetBorders(MultipleBorders.Top, Color.Gray, LineStyle.Thin)
                        .Borders.SetBorders(MultipleBorders.Right, Color.Gray, LineStyle.Thin)
                        .Borders.SetBorders(MultipleBorders.Left, Color.Gray, LineStyle.Thin)
                        .Borders.SetBorders(MultipleBorders.Bottom, Color.Gray, LineStyle.Thin)
                        .HorizontalAlignment = alineacion
                    End If
                End With
            Next
        End Sub

        Private Sub PintarTitulosCuerpo(ByVal filaInicial As Integer, ByVal columnaInicial As Integer, ByVal filaFinal As Integer, ByVal columnaFinal As Integer, ByVal colorFondo As Color, ByVal miWS As ExcelWorksheet, Optional ByVal merge As Boolean = False, Optional ByVal alineacion As HorizontalAlignmentStyle = HorizontalAlignmentStyle.Center)
            Dim cr As CellRange = miWS.Cells.GetSubrangeAbsolute(filaInicial, columnaInicial, filaFinal, columnaFinal)
            cr.Merged = merge
            For Each cel As ExcelCell In cr
                With cel.Style
                    .FillPattern.SetPattern(FillPatternStyle.Solid, colorFondo, colorFondo)
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                    .Borders.SetBorders(MultipleBorders.Top, Color.Gray, LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Right, Color.Gray, LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Left, Color.Gray, LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Bottom, Color.Gray, LineStyle.Thin)
                    .HorizontalAlignment = HorizontalAlignmentStyle.Left
                End With
            Next
        End Sub

#End Region

    End Class

End Namespace