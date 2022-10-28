Imports LMDataAccessLayer
Imports System.Web
Imports GemBox.Spreadsheet
Imports System.IO

Namespace Despachos

    Public Class ResumenInfoAsignacionTransportadoraADespachoMasivo

#Region "Atributos"

        Private _idDespacho As Long
        Private _dtResumen As DataTable
        Private _nombreArchivo As String

#End Region

#Region "Cosntructores"

        Public Sub New(ByVal idDespacho As Long)
            _idDespacho = idDespacho
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdDespacho As Long
            Get
                Return _idDespacho
            End Get
            Set(value As Long)
                _idDespacho = value
            End Set
        End Property

        Public ReadOnly Property DatosResumen As DataTable
            Get
                If _dtResumen Is Nothing Then CargarDatos()
                Return _dtResumen
            End Get
        End Property

        Public Property NombreArchivo As String
            Get
                Return _nombreArchivo
            End Get
            Set(value As String)
                _nombreArchivo = value
            End Set
        End Property

#End Region

#Region "Métodos Públicos"

        Public Sub CargarDatos()
            If _idDespacho > 0 Then
                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Add("@idDespacho", SqlDbType.BigInt).Value = _idDespacho
                        _dtResumen = .EjecutarDataTable("ObtenerResumenInfoTransportadoraDespachoMasivo", CommandType.StoredProcedure)
                        GenerarArchivoExcel()
                    End With
                End Using
            Else
                Throw New Exception("No se ha proporcionado el ID de despacho masivo para el cual se desea consultar el resumen de asignación de transportadora.")
            End If
        End Sub

        Public Sub GenerarArchivoExcel()
            If _dtResumen IsNot Nothing AndAlso _dtResumen.Rows.Count > 0 AndAlso HttpContext.Current IsNot Nothing Then
                HerramientasFuncionales.CargarLicenciaGembox()
                Dim plantilla As String = HttpContext.Current.Server.MapPath("~/POP/Archivos/PlantillaInfoAsignacionTransportadora.xlsx")
                If File.Exists(plantilla) Then
                    Dim oExcel As New ExcelFile
                    Dim oWs As ExcelWorksheet = Nothing

                    oExcel.LoadXlsx(plantilla, XlsxOptions.PreserveMakeCopy)
                    oWs = oExcel.Worksheets.ActiveWorksheet

                    oWs.InsertDataTable(_dtResumen, 1, 0, False)

                    With HttpContext.Current
                        Dim idUsuario As Integer
                        If .Session("usxp001") IsNot Nothing Then Integer.TryParse(.Session("usxp001").ToString, idUsuario)
                        _nombreArchivo = .Server.MapPath("~/archivos_planos/InfoAsignacionTransportadora_" & idUsuario.ToString & ".xlsx")
                    End With

                    oExcel.SaveXlsx(_nombreArchivo)

                End If
            End If
        End Sub


#End Region

    End Class

End Namespace
