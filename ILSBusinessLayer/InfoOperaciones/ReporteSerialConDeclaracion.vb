Imports LMDataAccessLayer
Public Class ReporteSerialConDeclaracion

#Region "Atributos"
    Private _mensaje As String
    Private _resultado As Integer
    Private _declaracion As String
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _factura As String
    Private _guia As String
    Private _estructuraTablaConsulta As DataTable
    Private _estructuraTablaSerial As DataTable
#End Region

#Region "Propiedades"

    Public Property Resultado() As Integer
        Get
            Return _resultado
        End Get
        Set(value As Integer)
            _resultado = value
        End Set
    End Property

    Public Property Mensaje() As String
        Get
            Return _mensaje
        End Get
        Set(value As String)
            _mensaje = value
        End Set
    End Property

    Public Property Declaracion() As String
        Get
            Return _declaracion
        End Get
        Set(value As String)
            _declaracion = value
        End Set
    End Property

    Public Property FechaInicial() As Date
        Get
            Return _fechaInicial
        End Get
        Set(value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal() As Date
        Get
            Return _fechaFinal
        End Get
        Set(value As Date)
            _fechaFinal = value
        End Set
    End Property

    Public Property Factura() As String
        Get
            Return _factura
        End Get
        Set(value As String)
            _factura = value
        End Set
    End Property

    Public Property Guia() As String
        Get
            Return _guia
        End Get
        Set(value As String)
            _guia = value
        End Set
    End Property

    Public Property EstructuraTablaConsulta() As DataTable
        Get
            Return _estructuraTablaConsulta
        End Get
        Set(value As DataTable)
            _estructuraTablaConsulta = value
        End Set
    End Property

    Public Property EstructuraTablaSerial() As DataTable
        Get
            Return _estructuraTablaSerial
        End Get
        Set(value As DataTable)
            _estructuraTablaSerial = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Metodos Publicos"

    Sub ConsultarInformacionSerialConDeclaracion()
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _declaracion IsNot Nothing AndAlso _declaracion <> "" Then .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("fechaInicial", SqlDbType.Date).Value = _fechaInicial
                    If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("fechaFinal", SqlDbType.Date).Value = _fechaFinal
                    _estructuraTablaConsulta = .ejecutarDataTable("ConsultarInformacionSerialConDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub ConsultarSerialesLeidosConDeclaracion()
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _declaracion IsNot Nothing AndAlso _declaracion <> "" Then .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    If _factura IsNot Nothing AndAlso _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                    If _guia IsNot Nothing AndAlso _guia <> "" Then .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                    _estructuraTablaSerial = .ejecutarDataTable("ConsultarSerialesFacturasConDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Function ConsultarDetalleSerialConDeclaracion() As DataTable
        Dim dtResultado As New DataTable
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _declaracion IsNot Nothing AndAlso _declaracion <> "" Then .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("fechaInicial", SqlDbType.Date).Value = _fechaInicial
                    If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("fechaFinal", SqlDbType.Date).Value = _fechaFinal
                    dtResultado = .ejecutarDataTable("ConsultarDetalleSerialConDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dtResultado
    End Function

#End Region

End Class
