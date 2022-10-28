Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports GemBox.Spreadsheet
Imports System.Drawing
Imports System.Web

Public Class ReporteSerialesEnviadosPendienteNacionalizacion

#Region "Atributos (Campos)"

    Private _idOrdenEnvio As Integer
    Private _idFactura As Integer
    Private _idGuia As Integer
    Private _idFacturaGuia As Integer
    Private _factura As String
    Private _datosReporte As DataTable

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idOrdenEnvio As Integer)
        MyBase.New()
        _idOrdenEnvio = idOrdenEnvio
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdOrdenEnvio() As Integer
        Get
            Return _idOrdenEnvio
        End Get
        Set(ByVal value As Integer)
            _idOrdenEnvio = value
        End Set
    End Property

    Public Property IdFactura() As Integer
        Get
            Return _idFactura
        End Get
        Set(ByVal value As Integer)
            _idFactura = value
        End Set
    End Property

    Public Property IdGuia() As Integer
        Get
            Return _idGuia
        End Get
        Set(ByVal value As Integer)
            _idGuia = value
        End Set
    End Property

    Public Property IdFacturaGuia() As Integer
        Get
            Return _idFacturaGuia
        End Get
        Set(ByVal value As Integer)
            _idFacturaGuia = value
        End Set
    End Property

    Public Property Factura() As String
        Get
            Return _idFactura
        End Get
        Set(ByVal value As String)
            _factura = value
        End Set
    End Property

    Public ReadOnly Property DatosReporte() As DataTable
        Get
            If _datosReporte Is Nothing Then CargarDatos()
            Return _datosReporte
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idOrdenEnvio > 0 Then .SqlParametros.Add("@idOrdenEnvio", SqlDbType.Int).Value = _idOrdenEnvio
                If _idFactura > 0 Then .SqlParametros.Add("@idFactura", SqlDbType.Int).Value = _idFactura
                If _idGuia > 0 Then .SqlParametros.Add("@idGuia", SqlDbType.Int).Value = _idGuia
                If _idFacturaGuia > 0 Then .SqlParametros.Add("@idFacturaGuia", SqlDbType.Int).Value = _idFacturaGuia
                If Not String.IsNullOrEmpty(Factura) Then .SqlParametros.Add("@factura", SqlDbType.VarChar, 50).Value = _factura

                .TiempoEsperaComando = 600
                _datosReporte = .ejecutarDataTable("ReporteSerialesEnviadosPendienteNacionalizacion", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Function GenerarReporteEnExcel() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        CargarLicenciaGembox()
        Dim oExcel As New ExcelFile
        Dim oWs As ExcelWorksheet
        Dim idUsuario As Integer
        Dim rutaPlantilla As String = HttpContext.Current.Server.MapPath("~/Reports/Plantillas/ReporteSerialesEnviadosPendienteNacionalizacion.xlsx")
        Dim nombreArchivo As String = ""

        resultado.EstablecerMensajeYValor(1, "Imposible Generar el archivo. Por favor intente nuevamente.")
        If System.IO.File.Exists(rutaPlantilla) Then
            oExcel.LoadXlsx(rutaPlantilla, XlsxOptions.PreserveMakeCopy)
            oWs = oExcel.Worksheets.ActiveWorksheet
            oWs.InsertDataTable(_datosReporte, 3, 0, False)
        Else
            oWs = oExcel.Worksheets.Add("SerialesSinNacionalizar")
            With oWs.Cells("A1")
                .Value = "Reporte de Seriales Enviados Pendientes por Nacionalización"
                .Style.Font.Weight = ExcelFont.BoldWeight
                .Style.Font.Size = 20 * 16
            End With

            oWs.InsertDataTable(_datosReporte, 2, 0, True)
            Dim myStyle As New CellStyle
            With myStyle
                .Borders.SetBorders(MultipleBorders.Outside, Color.Black, LineStyle.Thin)
                .HorizontalAlignment = HorizontalAlignmentStyle.Center
                .Font.Weight = ExcelFont.BoldWeight
                .FillPattern.SetSolid(ColorTranslator.FromHtml("gray"))
                .Font.Color = Color.White
            End With
            oWs.Cells.GetSubrange("A3", CellRange.RowColumnToPosition(2, _datosReporte.Columns.Count - 1)).Style = myStyle
            oWs.Cells.GetSubrange("A1", CellRange.RowColumnToPosition(0, _datosReporte.Columns.Count - 1)).Merged = True
        End If
        For index As Integer = 0 To _datosReporte.Columns.Count - 1
            oWs.Columns(index).AutoFitAdvanced(1.1)
        Next

        With HttpContext.Current
            If .Session("usxp001") IsNot Nothing Then Integer.TryParse(.Session("usxp001").ToString, idUsuario)
            If _datosReporte.Rows.Count <= 65000 Then
                nombreArchivo = .Server.MapPath("~/archivos_planos/ReporteSerialesEnviadosPendienteNacionalizacion_" & idUsuario.ToString & ".xls")
                oExcel.SaveXls(nombreArchivo)
            Else
                nombreArchivo = .Server.MapPath("~/archivos_planos/ReporteSerialesEnviadosPendienteNacionalizacion_" & idUsuario.ToString & ".xlsx")
                oExcel.SaveXlsx(nombreArchivo)
            End If
        End With

        If System.IO.File.Exists(nombreArchivo) Then
            resultado.EstablecerMensajeYValor(0, nombreArchivo)
        Else
            resultado.EstablecerMensajeYValor(2, "No fue posible almacenar el archivo en el servidor. Por favor intente nuevamente.")
        End If
        Return resultado
    End Function

#End Region

End Class
