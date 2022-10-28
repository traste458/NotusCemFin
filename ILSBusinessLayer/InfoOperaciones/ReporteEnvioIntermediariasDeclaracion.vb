Imports LMDataAccessLayer
Public Class ReporteEnvioIntermediariasDeclaracion

#Region "Atributos"
    Private _intermediaria As String
    Private _declaracion As String
    Private _factura As String
    Private _guia As String
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _estructuraTablaConsulta As DataTable
    Private _estructuraTablaSerial As DataTable
    Private _estructuraTablaIntermediariAduanera As DataTable
    Private _idAduanera As Integer
#End Region

#Region "Propiedades"

    Public Property Intermediaria() As String
        Get
            Return _intermediaria
        End Get
        Set(value As String)
            _intermediaria = value
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

    Public Property EstructuraTablaIntermediariAduanera() As DataTable
        Get
            Return _estructuraTablaIntermediariAduanera
        End Get
        Set(value As DataTable)
            _estructuraTablaIntermediariAduanera = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Metodos Publicos"

    Sub ConsultarInformacionIntermediariasAduaneras()
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _idAduanera > 0 AndAlso _idAduanera <> "" Then .SqlParametros.Add("@idAduanera", SqlDbType.VarChar).Value = _idAduanera
                    _estructuraTablaIntermediariAduanera = .ejecutarDataTable("ObtenerInfointermediariasAduaneras", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub ConsultarInformacionIntermediariaDeclaracion()
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _intermediaria IsNot Nothing AndAlso _intermediaria <> "" Then .SqlParametros.Add("@intermediaria", SqlDbType.Int).Value = _intermediaria
                    If _factura IsNot Nothing AndAlso _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                    If _guia IsNot Nothing AndAlso _guia <> "" Then .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                    If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("fechaInicial", SqlDbType.Date).Value = _fechaInicial
                    If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("fechaFinal", SqlDbType.Date).Value = _fechaFinal
                    _estructuraTablaConsulta = .ejecutarDataTable("ConsultarInformacionIntermediariaDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub ConsultarSerialesIntermediariaDeclaracion()
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _intermediaria IsNot Nothing AndAlso _intermediaria <> "" Then .SqlParametros.Add("@intermediaria", SqlDbType.VarChar).Value = _intermediaria
                    If _declaracion IsNot Nothing AndAlso _declaracion <> "" Then .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    If _factura IsNot Nothing AndAlso _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                    If _guia IsNot Nothing AndAlso _guia <> "" Then .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                    _estructuraTablaSerial = .ejecutarDataTable("ConsultarSerialesIntermediariaDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Function ConsultarDetalleIntermediariaDeclaracion() As DataTable
        Dim dtResultado As New DataTable
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _intermediaria IsNot Nothing AndAlso _intermediaria <> "" Then .SqlParametros.Add("@intermediaria", SqlDbType.VarChar).Value = _intermediaria
                    If _factura IsNot Nothing AndAlso _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                    If _guia IsNot Nothing AndAlso _guia <> "" Then .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                    If _fechaInicial <> Date.MinValue Then .SqlParametros.Add("fechaInicial", SqlDbType.Date).Value = _fechaInicial
                    If _fechaFinal <> Date.MinValue Then .SqlParametros.Add("fechaFinal", SqlDbType.Date).Value = _fechaFinal
                    dtResultado = .ejecutarDataTable("ConsultarDetalleIntermediariaDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dtResultado
    End Function

#End Region


End Class
