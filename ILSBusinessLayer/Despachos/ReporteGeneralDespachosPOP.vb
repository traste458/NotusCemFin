Imports LMDataAccessLayer

Namespace Despachos

    Public Class ReporteGeneralDespachosPOP

#Region "Atributos (Filtros de búsqueda)"

        Private _idEstado As Integer
        Private _FechaPedidoInicial As DateTime
        Private _FechaPedidoFinal As DateTime
        Private _FechaDespachoInicial As DateTime
        Private _FechaDespachoFinal As DateTime
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
        Public Property FechaPedidoInicial As DateTime
            Get
                Return _FechaPedidoInicial
            End Get
            Set(value As DateTime)
                _FechaPedidoInicial = value
            End Set
        End Property

        Public Property FechaPedidoFinal As DateTime
            Get
                Return _FechaPedidoFinal
            End Get
            Set(value As DateTime)
                _FechaPedidoFinal = value
            End Set
        End Property
        Public Property FechaDespachoInicial As DateTime
            Get
                Return _FechaDespachoInicial
            End Get
            Set(value As DateTime)
                _FechaDespachoInicial = value
            End Set
        End Property

        Public Property FechaDespachoFinal As DateTime
            Get
                Return _FechaDespachoFinal
            End Get
            Set(value As DateTime)
                _FechaDespachoFinal = value
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
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                      If _FechaDespachoInicial > Date.MinValue Then .SqlParametros.Add("@FechaDespachoInicial", SqlDbType.Date).Value = _FechaDespachoInicial
                    If _FechaDespachoFinal > Date.MinValue Then .SqlParametros.Add("@FechaDespachoFinal", SqlDbType.Date).Value = _FechaDespachoFinal
                    If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .TiempoEsperaComando = 0
                    _resultado = .GenerarArchivoExcel("ObtenerReporteGeneralDespachosPOP", NombreArchivo, CommandType.StoredProcedure, NombrePlantilla, "Reporte General de Despachos POP", 4)
                End With
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

#End Region



    End Class
End Namespace


