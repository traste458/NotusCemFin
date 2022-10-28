Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class ReporteInstruccionamientoTiempos

#Region "Atributos (Filtros de búsqueda)"

        Private _idEstado As Integer
        Private _numRadicado As Long
        Private _fechaInstruccionInicial As DateTime
        Private _fechaInstruccionFinal As DateTime
        Private _fechaLlegadaRealInicial As DateTime
        Private _fechaLlegadaRealFinal As DateTime
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

        Public Property FechaInstruccionInicial As DateTime
            Get
                Return _fechaInstruccionInicial
            End Get
            Set(value As DateTime)
                _fechaInstruccionInicial = value
            End Set
        End Property

        Public Property FechaInstruccionFinal As DateTime
            Get
                Return _fechaInstruccionFinal
            End Get
            Set(value As DateTime)
                _fechaInstruccionFinal = value
            End Set
        End Property
        Public Property FechaLlegadaRealInicial As DateTime
            Get
                Return _fechaLlegadaRealInicial
            End Get
            Set(value As DateTime)
                _fechaLlegadaRealInicial = value
            End Set
        End Property

        Public Property FechaLlegadaRealFinal As DateTime
            Get
                Return _fechaLlegadaRealFinal
            End Get
            Set(value As DateTime)
                _fechaLlegadaRealFinal = value
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
                        If _fechaInstruccionInicial > Date.MinValue Then .SqlParametros.Add("@fechaInstruccionInicial", SqlDbType.DateTime).Value = _fechaInstruccionInicial
                        If _fechaInstruccionFinal > Date.MinValue Then .SqlParametros.Add("@fechaInstruccionFinal", SqlDbType.DateTime).Value = _fechaInstruccionFinal
                        If _fechaLlegadaRealInicial > Date.MinValue Then .SqlParametros.Add("@fechaLlegadaRealInicial", SqlDbType.DateTime).Value = _fechaLlegadaRealInicial
                        If _fechaLlegadaRealFinal > Date.MinValue Then .SqlParametros.Add("@fechaLlegadaRealFinal", SqlDbType.DateTime).Value = _fechaLlegadaRealFinal
                        If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .TiempoEsperaComando = 0
                        _resultado = .GenerarArchivoExcel("ObtenerInstruccionClienteExternoTiempoProceso", NombreArchivo, CommandType.StoredProcedure, NombrePlantilla, "Reporte Instruccionamiento", 3)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

        End Sub

#End Region



    End Class
End Namespace


