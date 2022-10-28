Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.IO
Imports System.Web

Namespace InventarioFisico

    Public Class ReporteMaestroDeInventarioFisico

#Region "Atributos"

        Private _datosGenerales As DataTable
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            CargarLicenciaGembox()
        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property DatosGenerales As DataTable
            Get
                If Not _cargado OrElse _datosGenerales Is Nothing Then CargarDatosGenerales()
                Return _datosGenerales
            End Get
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub CargarDatosGenerales()
            Dim dbManager As LMDataAccess = Nothing
            Try
                dbManager = New LMDataAccess

                With dbManager
                    _datosGenerales = .EjecutarDataTable("ReporteGeneralDeInventarioFisico", CommandType.StoredProcedure)
                    _cargado = True
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub GenerarYDescargarArchivo(Optional ByVal ext As Extension = Extension.XLSX)
            CargarLicenciaGembox()
            Dim contexto As HttpContext = HttpContext.Current
            Dim oExcel As New ExcelFile()
            Dim rutaPlantilla As String = contexto.Server.MapPath("~/PlantillasYEjemplos/PlantillaReporteAvanceGeneralInventarioFisico.xlsx")
            If File.Exists(rutaPlantilla) Then
                Dim oWs As ExcelWorksheet

                oExcel.LoadXlsx(rutaPlantilla, XlsxOptions.PreserveMakeCopy)
                oWs = oExcel.Worksheets("ReporteAvanceInventarioFisico")

                If _datosGenerales Is Nothing OrElse Not _cargado Then CargarDatosGenerales()
                Dim dtAux As DataTable = _datosGenerales.Copy
                If dtAux.Columns.Contains("CantidadLeida") Then dtAux.Columns.Remove("CantidadLeida")
                oWs.InsertDataTable(dtAux, 3, 0, False)

                'For Each eCol As ExcelColumn In oWs.Columns
                '    eCol.AutoFitAdvanced(1.1000000000000001)
                'Next

                Dim nombreExtension As String = [Enum].GetName(GetType(Extension), ext)
                Dim rutaArchivo As String = contexto.Server.MapPath("~/RepositorioArchivos/ReporteAvanceGeneralInventarioFisico_" & _
                                                                    contexto.Session("userId") & "." & nombreExtension.ToLower)

                Select Case ext
                    Case Extension.XLSX
                        oExcel.SaveXlsx(rutaArchivo)
                    Case Extension.XLS
                        oExcel.SaveXls(rutaArchivo)
                    Case Extension.CSV
                        oExcel.SaveCsv(rutaArchivo, CChar(vbTab))
                End Select

                ForzarDescargaDeArchivo(rutaArchivo, Path.GetFileName(rutaArchivo))
            Else
                Throw New Exception("No se encontró la plantilla del reporte de avance general de inventario físico. Por favor contracte a IT")
            End If

        End Sub


        Public Enum Extension
            XLSX = 1
            XLS = 2
            CSV = 3
        End Enum

#End Region

    End Class

End Namespace