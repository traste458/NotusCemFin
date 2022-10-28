Imports GemBox.Spreadsheet
Imports System.IO
Imports System.Drawing

Namespace Comunes

    Public Class ExcelManager

#Region "Atributos"

        Private _ef As ExcelFile
        Private _ews As ExcelWorksheet
        Private _strmArchivo As MemoryStream

        Private _nombreHoja As String
        Private _incluirEncabezado As Boolean
        Private _filaInicial As Integer
        Private _columnaInicial As Integer

        Private _autoAjustarColumna As Boolean
        Private _estiloEncabezado As CellStyle
        Private _estiloSubTitulo As CellStyle

#End Region

#Region "Propiedades"

        Public Property NombreHoja As String
            Get
                Return _nombreHoja
            End Get
            Set(value As String)
                _nombreHoja = value
            End Set
        End Property

        Public Property IncluirEncabezado As Boolean
            Get
                Return _incluirEncabezado
            End Get
            Set(value As Boolean)
                _incluirEncabezado = value
            End Set
        End Property

        Public Property FilaInicial As Integer
            Get
                Return _filaInicial
            End Get
            Set(value As Integer)
                _filaInicial = value
            End Set
        End Property

        Public Property ColumnaInicial As Integer
            Get
                Return _columnaInicial
            End Get
            Set(value As Integer)
                _columnaInicial = value
            End Set
        End Property

        Public Property AutoAjustarColumna As Boolean
            Get
                Return _autoAjustarColumna
            End Get
            Set(value As Boolean)
                _autoAjustarColumna = value
            End Set
        End Property

        Public Property EstiloEncabezado As CellStyle
            Get
                Return _estiloEncabezado
            End Get
            Set(value As CellStyle)
                _estiloEncabezado = value
            End Set
        End Property

        Public Property EstiloSubTitulo As CellStyle
            Get
                Return _estiloSubTitulo
            End Get
            Set(value As CellStyle)
                _estiloSubTitulo = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")

            _nombreHoja = "Datos"
            _incluirEncabezado = True
            _filaInicial = 0
            _columnaInicial = 0

            _autoAjustarColumna = True
            _estiloEncabezado = EstableceEstiloEncabezadoBase()
            _estiloSubTitulo = EstableceEstiloSubTituloBase()

        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function GenerarExcel(ByVal dtDatos As DataTable) As MemoryStream
            Try
                _ef = New ExcelFile()
                _strmArchivo = New MemoryStream
                _ews = _ef.Worksheets.Add(NombreHoja)
                _ews.InsertDataTable(dtDatos, _filaInicial, _columnaInicial, _incluirEncabezado)

                EstablecerFormatos()

                _ef.SaveXlsx(_strmArchivo)
                _strmArchivo.Position = 0

                Return _strmArchivo
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GenerarExcel(ByVal dtDatos As DataTable, ByVal pathPlantilla As String) As MemoryStream
            Try
                If File.Exists(pathPlantilla) Then
                    _ef = New ExcelFile()
                    _strmArchivo = New MemoryStream

                    _ef.LoadXlsx(pathPlantilla, XlsxOptions.PreserveKeepOpen)

                    _ews = _ef.Worksheets.ActiveWorksheet
                    _ews.InsertDataTable(dtDatos, _filaInicial, _columnaInicial, _incluirEncabezado)

                    _ef.SaveXlsx(_strmArchivo)
                    _strmArchivo.Position = 0

                    Return _strmArchivo
                Else
                    Throw New Exception("No se logro encontrar la plantilla de Excel proporcionada.")
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GenerarExcelAgrupado(ByVal dsDatos As DataSet, _
                                             ByVal nombreCampoMasterDetail As String, _
                                             Optional ByVal colapsado As Boolean = True) As MemoryStream
            Dim dtMaestro As DataTable
            Dim dtDetalle As DataTable
            Try
                _ef = New ExcelFile()
                _strmArchivo = New MemoryStream()
                _ews = _ef.Worksheets.Add(NombreHoja)

                dtMaestro = dsDatos.Tables(0)
                dtDetalle = dsDatos.Tables(1)

                'Titulo general
                For tituloMaestro As Integer = 0 To dtMaestro.Columns.Count - 1
                    _ews.Cells(_filaInicial, tituloMaestro).Value = dtMaestro.Columns(tituloMaestro).ColumnName
                    _ews.Cells(_filaInicial, tituloMaestro).Style = _estiloEncabezado
                Next
                _filaInicial += 1

                'Se recorren los datos maestros
                For filaMaestro As Integer = 0 To dtMaestro.Rows.Count - 1
                    Dim maestro As DataRow = dtMaestro.Rows(filaMaestro)

                    _ews.Rows(_filaInicial).OutlineLevel = 1

                    'Se crean los títulos del Maestro
                    For valorMaestro As Integer = 0 To dtMaestro.Columns.Count - 1
                        _ews.Cells(_filaInicial, valorMaestro).Value = dtMaestro.Rows(filaMaestro).Item(valorMaestro)
                        _ews.Cells(_filaInicial, valorMaestro).Style = _estiloEncabezado
                    Next
                    _filaInicial += 1


                    'Titulo Detalle
                    For tituloDetalle As Integer = 0 To dtDetalle.Columns.Count - 1
                        _ews.Cells(_filaInicial, tituloDetalle).Value = dtDetalle.Columns(tituloDetalle).ColumnName
                        _ews.Cells(_filaInicial, tituloDetalle).Style = _estiloSubTitulo
                    Next
                    _ews.Rows(_filaInicial).OutlineLevel = 2
                    _ews.Rows(_filaInicial).Collapsed = colapsado
                    _ews.Rows(_filaInicial).Hidden = colapsado
                    _filaInicial += 1

                    'Se adicionan los detalles
                    Dim dvDetalle As DataView = dtDetalle.DefaultView
                    dvDetalle.RowFilter = nombreCampoMasterDetail & "=" & maestro.Item(nombreCampoMasterDetail)

                    For Each itemDetalle As DataRow In dvDetalle.ToTable().Rows
                        For celda As Integer = 0 To itemDetalle.ItemArray.Length - 1
                            _ews.Cells(_filaInicial, celda).Value = itemDetalle(celda)
                            _ews.Rows(_filaInicial).OutlineLevel = 2
                            _ews.Rows(_filaInicial).Collapsed = colapsado
                            _ews.Rows(_filaInicial).Hidden = colapsado
                            _ews.Cells(_filaInicial, celda).SetBorders(MultipleBorders.Outside, Color.Gainsboro, LineStyle.Thick)
                        Next

                        _filaInicial += 1
                    Next
                Next
                _ews.ViewOptions.OutlineRowButtonsBelow = False
                EstablecerFormatos()

                _ef.SaveXlsx(_strmArchivo)
                _strmArchivo.Position = 0

                Return _strmArchivo
            Catch ex As Exception
                Throw ex
            End Try
        End Function

#End Region

#Region "Métodos Privados"

        Private Sub EstablecerFormatos()
            'Se adiciona formato al Encabezado
            If _incluirEncabezado Then FormatoEncabezado()

            'Se valida si debe ajustar tamaño de columnas
            If _autoAjustarColumna Then AjustarTamanioColumna()
        End Sub

        Private Sub AjustarTamanioColumna()
            Try
                Dim columnCount = _ews.CalculateMaxUsedColumns()
                For i As Integer = 0 To columnCount - 1
                    _ews.Columns(i).AutoFit()
                Next
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub FormatoEncabezado()
            Try
                For Each columnaEncabezado As ExcelCell In _ews.Rows(_filaInicial).Cells
                    columnaEncabezado.Style = _estiloEncabezado
                Next
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Function EstableceEstiloEncabezadoBase() As CellStyle
            Dim estiloEncabezado As New CellStyle
            With estiloEncabezado
                .Font.Size = 18 * 10
                .HorizontalAlignment = HorizontalAlignmentStyle.Center
                .Font.Weight = ExcelFont.BoldWeight
                .Borders.SetBorders(MultipleBorders.Outside, Color.Gray, LineStyle.Thick)
            End With
            Return estiloEncabezado
        End Function

        Private Function EstableceEstiloSubTituloBase() As CellStyle
            Dim estiloEncabezado As New CellStyle
            With estiloEncabezado
                .Font.Size = 18 * 10
                .HorizontalAlignment = HorizontalAlignmentStyle.Center
                .Font.Weight = ExcelFont.NormalWeight
                .Font.Italic = True
                .Font.UnderlineStyle = UnderlineStyle.Single
            End With
            Return estiloEncabezado
        End Function

#End Region

    End Class

End Namespace