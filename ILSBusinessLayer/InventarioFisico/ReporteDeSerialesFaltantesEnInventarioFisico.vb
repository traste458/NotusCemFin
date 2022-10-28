Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.IO
Imports System.Web
Imports ILSBusinessLayer.Enumerados

Namespace InventarioFisico

    Public Class ReporteDeSerialesFaltantesEnInventarioFisico

#Region "Atributos"

        Private _datosReporte As DataTable
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            CargarLicenciaGembox()
        End Sub

#End Region

#Region "Propiedades"

        Public Property Material As String
        Public Property Centro As String
        Public Property Almacen As String
        Public Property Stock As String
        Public Property Justificado As Enumerados.EstadoBinario

        Public ReadOnly Property DatosGenerales As DataTable
            Get
                If Not _cargado OrElse _datosReporte Is Nothing Then CargarDatos()
                Return _datosReporte
            End Get
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub CargarDatos()
            Dim dbManager As LMDataAccess = Nothing
            Try
                dbManager = New LMDataAccess

                With dbManager
                    If Not EsNuloOVacio(Me._Material) Then .SqlParametros.AddWithValue("@material", Me._Material)
                    If Not EsNuloOVacio(Me._Centro) Then .SqlParametros.AddWithValue("@centro", Me._Centro)
                    If Not EsNuloOVacio(Me._Almacen) Then .SqlParametros.AddWithValue("@almacen", Me._Almacen)
                    If Not EsNuloOVacio(Me._Stock) Then .SqlParametros.AddWithValue("@stock", Me._Stock)
                    If Me._Justificado <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@justificado", SqlDbType.Bit).Value = IIf(Me._Justificado = EstadoBinario.Activo, 1, 0)

                    _datosReporte = .EjecutarDataTable("ReporteDeSerialesFaltantesDeInventarioFisico", CommandType.StoredProcedure)
                    _cargado = True
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Function GenerarArchivo(Optional ByVal ext As ExtensionArchivo = ExtensionArchivo.XLSX) As ResultadoProceso
            Dim resultado As New ResultadoProceso(-1, "Proceso no realizado")
            CargarLicenciaGembox()
            Dim contexto As HttpContext = HttpContext.Current
            Dim oExcel As New ExcelFile()
            Dim rutaPlantilla As String = contexto.Server.MapPath("~/Reports/PlantillasYEjemplos/PlantillaReporteFaltantesInventarioFisico.xlsx")
            If File.Exists(rutaPlantilla) Then
                Dim oWs As ExcelWorksheet

                oExcel.LoadXlsx(rutaPlantilla, XlsxOptions.PreserveMakeCopy)
                oWs = oExcel.Worksheets("FaltantesInventarioFisico")

                If _datosReporte Is Nothing OrElse Not _cargado Then CargarDatos()
                oWs.InsertDataTable(_datosReporte, 1, 0, False)

                'For Each eCol As ExcelColumn In oWs.Columns
                '    eCol.AutoFitAdvanced(1.1000000000000001)
                'Next

                Dim nombreExtension As String = [Enum].GetName(GetType(ExtensionArchivo), ext)
                Dim urlArchivo As String = "~/archivos_planos/ReporteFaltantesInventarioFisico_" & _
                                                                    contexto.Session("userId") & "." & nombreExtension.ToLower
                Dim rutaArchivo As String = contexto.Server.MapPath(urlArchivo)
                Select Case ext
                    Case ExtensionArchivo.XLSX
                        oExcel.SaveXlsx(rutaArchivo)
                    Case ExtensionArchivo.XLS
                        oExcel.SaveXls(rutaArchivo)
                    Case ExtensionArchivo.CSV
                        oExcel.SaveCsv(rutaArchivo, CChar(vbTab))
                End Select

                resultado.EstablecerMensajeYValor(0, rutaArchivo)
            Else
                resultado.EstablecerMensajeYValor(1, "No se encontró la plantilla del reporte de faltantes de inventario físico. Por favor contracte a IT")
            End If
            Return resultado
        End Function

#End Region

    End Class

End Namespace