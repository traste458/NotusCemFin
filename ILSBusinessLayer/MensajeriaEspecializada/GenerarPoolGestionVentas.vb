Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados
Imports System.Web

Public Class GenerarPoolGestionVentas

#Region "Atributos"

    Private _listIdServicio As List(Of String)
    Private _listMsisdn As List(Of String)
    Private _idServicioMensajeria As Integer
    Private _fechaRegistroInicio As Date
    Private _fechaRegistroFin As Date
    Private _fechaAprobacionInicio As DateTime
    Private _fechaAprobacionFin As DateTime
    Private _fechaAnulacionInicio As DateTime
    Private _fechaAnulacionFin As DateTime
    Private _idJornada As Integer
    Private _fechaAgenda As Date
    Private _idListaEstado As List(Of Integer)
    Private _idCiudad As Integer
    Private _identificacionCliente As String
    Private _nombreCliente As String

#End Region

#Region "Propiedades"

    Public Property ListIdServicio As List(Of String)
        Get
            If _listIdServicio Is Nothing Then _listIdServicio = New List(Of String)
            Return _listIdServicio
        End Get
        Set(value As List(Of String))
            _listIdServicio = value
        End Set
    End Property

    Public Property ListMsisdn As List(Of String)
        Get
            If _listMsisdn Is Nothing Then _listMsisdn = New List(Of String)
            Return _listMsisdn
        End Get
        Set(value As List(Of String))
            _listMsisdn = value
        End Set
    End Property

    Public Property IdServicioMensajeria As Integer
        Get
            Return _idServicioMensajeria
        End Get
        Set(value As Integer)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property FechaRegistroInicio As Date
        Get
            Return _fechaRegistroInicio
        End Get
        Set(value As Date)
            _fechaRegistroInicio = value
        End Set
    End Property

    Public Property FechaRegistroFin As Date
        Get
            Return _fechaRegistroFin
        End Get
        Set(value As Date)
            _fechaRegistroFin = value
        End Set
    End Property

    Public Property FechaAprobacionInicio As DateTime
        Get
            Return _fechaAprobacionInicio
        End Get
        Set(value As DateTime)
            _fechaAprobacionInicio = value
        End Set
    End Property

    Public Property FechaAprobacionFin As DateTime
        Get
            Return _fechaAprobacionFin
        End Get
        Set(value As DateTime)
            _fechaAprobacionFin = value
        End Set
    End Property

    Public Property FechaAnulacionInicio As DateTime
        Get
            Return _fechaAnulacionInicio
        End Get
        Set(value As DateTime)
            _fechaAnulacionInicio = value
        End Set
    End Property

    Public Property FechaAnulacionFin As DateTime
        Get
            Return _fechaAnulacionFin
        End Get
        Set(value As DateTime)
            _fechaAnulacionFin = value
        End Set
    End Property

    Public Property IdJornada As Integer
        Get
            Return _idJornada
        End Get
        Set(value As Integer)
            _idJornada = value
        End Set
    End Property

    Public Property FechaAgenda As Date
        Get
            Return _fechaAgenda
        End Get
        Set(value As Date)
            _fechaAgenda = value
        End Set
    End Property

    Public Property IdListaEstado As List(Of Integer)
        Get
            Return _idListaEstado
        End Get
        Set(value As List(Of Integer))
            _idListaEstado = value
        End Set
    End Property

    Public Property IdCiudad As Integer
        Get
            Return _idCiudad
        End Get
        Set(value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property IdentificacionCliente As String
        Get
            Return _identificacionCliente
        End Get
        Set(value As String)
            _identificacionCliente = value
        End Set
    End Property

    Public Property NombreCliente As String
        Get
            Return _nombreCliente
        End Get
        Set(value As String)
            _nombreCliente = value
        End Set
    End Property

#End Region

#Region "Contructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function GenerarPool() As DataTable
        Dim dtDatos As DataTable
        Dim idUsuario As Integer
        Using dbManager As New LMDataAccess
            Try
                Integer.TryParse(HttpContext.Current.Session("usxp001"), idUsuario)
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuario
                    If _listIdServicio IsNot Nothing AndAlso _listIdServicio.Count > 0 Then _
                        .SqlParametros.Add("@listIdServicio", SqlDbType.VarChar).Value = String.Join(",", _listIdServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _listMsisdn IsNot Nothing AndAlso _listMsisdn.Count > 0 Then _
                        .SqlParametros.Add("@listMsisdn", SqlDbType.VarChar).Value = String.Join(",", _listMsisdn.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                    If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                    If _fechaRegistroInicio <> Date.MinValue Then .SqlParametros.Add("@fechaRegistroInicio", SqlDbType.DateTime).Value = _fechaRegistroInicio
                    If _fechaRegistroFin <> Date.MinValue Then .SqlParametros.Add("@fechaRegistroFin", SqlDbType.DateTime).Value = _fechaRegistroFin
                    If _fechaAprobacionInicio > Date.MinValue Then .SqlParametros.Add("@fechaAprobacionInicio", SqlDbType.DateTime).Value = _fechaAprobacionInicio
                    If _fechaAprobacionFin > Date.MinValue Then .SqlParametros.Add("@fechaAprobacionFin", SqlDbType.DateTime).Value = _fechaAprobacionFin
                    If _fechaAnulacionInicio > Date.MinValue Then .SqlParametros.Add("@fechaAnulacionInicio", SqlDbType.DateTime).Value = _fechaAnulacionInicio
                    If _fechaAnulacionFin > Date.MinValue Then .SqlParametros.Add("@fechaAnulacionFin", SqlDbType.DateTime).Value = _fechaAnulacionFin
                    If _idJornada > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                    If _fechaAgenda <> Date.MinValue Then .SqlParametros.Add("@fechaAgenda", SqlDbType.DateTime).Value = _fechaAgenda
                    If _idListaEstado IsNot Nothing AndAlso _idListaEstado.Count > 0 Then _
                        .SqlParametros.Add("@listaIdEstado", SqlDbType.VarChar).Value = String.Join(",", _idListaEstado.ConvertAll(Of String)(Function(x) (x.ToString())).ToArray())

                    If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    If Not String.IsNullOrEmpty(_identificacionCliente) Then .SqlParametros.Add("@identificacionCliente", SqlDbType.VarChar).Value = _identificacionCliente
                    If Not String.IsNullOrEmpty(_nombreCliente) Then .SqlParametros.Add("@nombreCliente", SqlDbType.VarChar).Value = _nombreCliente

                    dtDatos = .EjecutarDataTable("ObtenerInformacionVentasCEM", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return dtDatos
    End Function

#End Region

End Class

