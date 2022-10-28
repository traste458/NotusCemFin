Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados

Public Class GenerarPoolAsignacionSerialesPrestamo

#Region "Atributos (Campos)"

    Private _idServicioMensajeria As Integer
    Private _numeroRadicado As Long
    Private _idCiudad As Integer
    Private _idBodega As Integer
    Private _fechaCreacionInicial As Date
    Private _fechaCreacionFinal As Date
    Private _clienteVIP As Enumerados.EstadoBinario
    Private _idPrioridad As Integer
    Private _idEstado As Integer
    Private _listaEstado As ArrayList

#End Region

#Region "Propiedades"

    Public Property IdServicioMensajeria() As String
        Get
            Return _idServicioMensajeria
        End Get
        Set(ByVal value As String)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property NumeroRadicado() As Integer
        Get
            Return _numeroRadicado
        End Get
        Set(ByVal value As Integer)
            _numeroRadicado = value
        End Set
    End Property

    Public Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property IdBodega() As Integer
        Get
            Return _idBodega
        End Get
        Set(ByVal value As Integer)
            _idBodega = value
        End Set
    End Property

    Public Property FechaCreacionInicial() As Date
        Get
            Return _fechaCreacionInicial
        End Get
        Set(ByVal value As Date)
            _fechaCreacionInicial = value
        End Set
    End Property

    Public Property FechaCreacionFinal() As Date
        Get
            Return _fechaCreacionFinal
        End Get
        Set(ByVal value As Date)
            _fechaCreacionFinal = value
        End Set
    End Property

    Public Property ClienteVIP() As Enumerados.EstadoBinario
        Get
            Return _clienteVIP
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _clienteVIP = value
        End Set
    End Property

    Public Property IdPrioridad() As Integer
        Get
            Return _idPrioridad
        End Get
        Set(ByVal value As Integer)
            _idPrioridad = value
        End Set
    End Property

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property ListaEstado() As ArrayList
        Get
            If _listaEstado Is Nothing Then _listaEstado = New ArrayList
            Return _listaEstado
        End Get
        Set(ByVal value As ArrayList)
            _listaEstado = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function GenerarPool() As DataTable
        Dim dtDatos As New DataTable
        Dim _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                    If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.Int).Value = _numeroRadicado
                    If _fechaCreacionInicial > Date.MinValue AndAlso _fechaCreacionFinal > Date.MinValue Then
                        .Add("@fechaCreacionInicial", SqlDbType.DateTime).Value = _fechaCreacionInicial
                        .Add("@fechaCreacionFinal", SqlDbType.DateTime).Value = _fechaCreacionFinal
                    End If
                    If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                    .Add("@idTipoServicio", SqlDbType.Int).Value = Enumerados.TipoServicio.ServicioTecnico
                    If _listaEstado IsNot Nothing AndAlso _listaEstado.Count > 0 Then .Add("@listaEstado", SqlDbType.VarChar).Value = Join(_listaEstado.ToArray, ",")
                    If _clienteVIP <> EstadoBinario.NoEstablecido Then .Add("@clienteVIP", SqlDbType.Bit).Value = IIf(_clienteVIP = EstadoBinario.Activo, 1, 0)
                    If _idPrioridad > 0 Then .Add("@idPrioridad", SqlDbType.Int).Value = _idPrioridad
                End With
                dtDatos = .ejecutarDataTable("ObtieneInfoServicioRequierePrestamo", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw ex
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class
