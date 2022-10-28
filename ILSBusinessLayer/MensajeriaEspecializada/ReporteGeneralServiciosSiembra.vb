Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class ReporteGeneralServiciosSiembra

#Region "Atributos (Filtros de búsqueda)"

        Private _idEstado As Integer
        Private _numRadicado As Long
        Private _fechaRegistroInicio As DateTime
        Private _fechaRegistroFin As DateTime
        Private _idUsuario As Integer
        Private _resultado As New InfoResultado
        Private _nombreArchivo As String
        Private _nombrePlantilla As String


#End Region

#Region "Propiedades"
        Public Property IdEstado As Integer
            Get
                Return _idEstado
            End Get
            Set(value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property NumeroRadicado As Long
            Get
                Return _numRadicado
            End Get
            Set(value As Long)
                _numRadicado = value
            End Set
        End Property

        Public Property FechaRegistroInicio As DateTime
            Get
                Return _fechaRegistroInicio
            End Get
            Set(value As DateTime)
                _fechaRegistroInicio = value
            End Set
        End Property

        Public Property FechaRegistroFin As DateTime
            Get
                Return _fechaRegistroFin
            End Get
            Set(value As DateTime)
                _fechaRegistroFin = value
            End Set
        End Property
        Public Property IdUsuario As Integer
            Get
                Return _idUsuario
            End Get
            Set(value As Integer)
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
                        If _fechaRegistroInicio > Date.MinValue Then .SqlParametros.Add("@fechaRegistroInicio", SqlDbType.DateTime).Value = _fechaRegistroInicio
                        If _fechaRegistroFin > Date.MinValue Then .SqlParametros.Add("@fechaRegistroFin", SqlDbType.DateTime).Value = _fechaRegistroFin
                        If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .TiempoEsperaComando = 0
                        _resultado = .GenerarArchivoExcel("ReporteGeneralServiciosSiembra", NombreArchivo, CommandType.StoredProcedure, NombrePlantilla, "Reporte General Servicios Siembra", 4)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

        End Sub

#End Region



    End Class
End Namespace


