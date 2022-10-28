Imports LMDataAccessLayer
Public Class ReporteSerialSinDeclaracion

#Region "Atributos"
    Private _mensaje As String
    Private _resultado As Integer
    Private _factura As String
    Private _guia As String
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _serial As ArrayList
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

    Public Property Serial() As ArrayList
        Get
            Return _serial
        End Get
        Set(value As ArrayList)
            _serial = value
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

    Sub ConsultarInformacionSerialSinDeclaracion()
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _factura IsNot Nothing AndAlso _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                    If _guia IsNot Nothing AndAlso _guia <> "" Then .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                    If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("fechaInicial", SqlDbType.Date).Value = _fechaInicial
                    If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("fechaFinal", SqlDbType.Date).Value = _fechaFinal
                    If _serial IsNot Nothing AndAlso _serial.Count > 0 Then .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = Join(_serial.ToArray, ",")
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    _estructuraTablaConsulta = .ejecutarDataTable("ConsultarInformacionSerialSinDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub ConsultarSerialesLeidosSinDeclaracion()
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _factura IsNot Nothing AndAlso _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                    If _guia IsNot Nothing AndAlso _guia <> "" Then .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    _estructuraTablaSerial = .ejecutarDataTable("ConsultarSerialesFacturasSinDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Function ConsultarDetalleSerialSinDeclaracion() As DataTable
        Dim dtResultado As New DataTable
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _factura IsNot Nothing AndAlso _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                    If _guia IsNot Nothing AndAlso _guia <> "" Then .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                    If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("fechaInicial", SqlDbType.Date).Value = _fechaInicial
                    If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("fechaFinal", SqlDbType.Date).Value = _fechaFinal
                    If _serial IsNot Nothing AndAlso _serial.Count > 0 Then .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = Join(_serial.ToArray, ",")
                    dtResultado = .ejecutarDataTable("ConsultarDetalleSerialSinDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dtResultado
    End Function

#End Region

End Class
