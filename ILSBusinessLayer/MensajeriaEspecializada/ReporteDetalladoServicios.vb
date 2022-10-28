Imports LMDataAccessLayer

Public Class ReporteDetalladoServicios

#Region "Atributos"

    Private _idTipo As Integer
    Private _idUsuario As Long
    Private _resultado As New InfoResultado
    Private _nombreArchivo As String
    Private _nombrePlantilla As String

#End Region

#Region "Propiedades"
    Public Property IdTipo As Integer
        Get
            Return _idTipo
        End Get
        Set(value As Integer)
            _idTipo = value
        End Set
    End Property

    Public Property IdUsuario As Long
        Get
            Return _idUsuario
        End Get
        Set(value As Long)
            _idUsuario = value
        End Set
    End Property
    Public Property Resultado() As InfoResultado
        Get
            Return _resultado
        End Get
        Set(ByVal value As InfoResultado)
            _resultado = value
        End Set
    End Property
    Public Property NombreArchivo() As String
        Get
            Return _nombreArchivo
        End Get
        Set(ByVal value As String)
            _nombreArchivo = value
        End Set
    End Property
    Public Property NombrePlantilla() As String
        Get
            Return _nombrePlantilla
        End Get
        Set(ByVal value As String)
            _nombrePlantilla = value
        End Set
    End Property
#End Region


#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Púbicos"

    Public Sub ObtenerReporte()
        Dim dtReporte As New ResultadoProceso
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If IdTipo <> 0 Then .SqlParametros.Add("@opcion", SqlDbType.Int).Value = IdTipo
                    If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .TiempoEsperaComando = 0
                    _resultado = .GenerarArchivoExcel("ObtenerReporteConsultaSeriales", NombreArchivo, CommandType.StoredProcedure, NombrePlantilla, "Reporte detallado de servicios", 3)
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using

    End Sub

#End Region

End Class
