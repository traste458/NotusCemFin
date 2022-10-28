Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class DetalleMsisdnEnServicioMensajeriaCargue

#Region "Atributos (Campos)"

    Private _idRegistro As Integer
    Private _idServicioMensajeria As Integer
    Private _idTipoServicio As Integer
    Private _msisdn As Long
    Private _activaEquipoAnterior As String
    Private _comseguro As String
    Private _precioConIva As Double
    Private _precioSinIva As Double
    Private _idClausula As Integer
    Private _clausula As String
    Private _registrado As Boolean
    Private _bloquear As Boolean
    Private _tieneCambioServicio As String
    Private _cargadoMSISDN As Integer

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idRegistro As Integer)
        MyBase.New()
        _idRegistro = idRegistro
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdMsisdn() As Integer
        Get
            Return _idRegistro
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idRegistro = value
        End Set
    End Property

    Public Property IdServicioMensajeria() As Integer
        Get
            Return _idServicioMensajeria
        End Get
        Set(ByVal value As Integer)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property IdTipoServicio() As Integer
        Get
            Return _idTipoServicio
        End Get
        Set(ByVal value As Integer)
            _idTipoServicio = value
        End Set
    End Property

    Public Property MSISDN() As String
        Get
            Return _msisdn
        End Get
        Set(ByVal value As String)
            _msisdn = value
        End Set
    End Property

    Public Property ActivaEquipoAnterior() As String
        Get
            Return _activaEquipoAnterior
        End Get
        Set(ByVal value As String)
            _activaEquipoAnterior = value
        End Set
    End Property

    Public Property Comseguro() As String
        Get
            Return _comseguro
        End Get
        Set(ByVal value As String)
            _comseguro = value
        End Set
    End Property

    Public Property PrecioConIva() As Double
        Get
            Return _precioConIva
        End Get
        Set(ByVal value As Double)
            _precioConIva = value
        End Set
    End Property

    Public Property PrecioSinIva() As Double
        Get
            Return _precioSinIva
        End Get
        Set(ByVal value As Double)
            _precioSinIva = value
        End Set
    End Property

    Public Property IdClausula() As Integer
        Get
            Return _idClausula
        End Get
        Set(ByVal value As Integer)
            _idClausula = value
        End Set
    End Property

    Public Property Clausula() As String
        Get
            Return _clausula
        End Get
        Set(ByVal value As String)
            _clausula = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

    Public Property Bloquear() As Boolean
        Get
            Return _bloquear
        End Get
        Set(ByVal value As Boolean)
            _bloquear = value
        End Set
    End Property

    Public Property cargadoMSISDN() As Integer
        Get
            Return _cargadoMSISDN
        End Get
        Set(ByVal value As Integer)
            _cargadoMSISDN = value
        End Set
    End Property

    Public Property TieneCambioServicio() As String
        Get
            Return _tieneCambioServicio
        End Get
        Set(ByVal value As String)
            _tieneCambioServicio = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idMaterialServicio", SqlDbType.Int).Value = _idRegistro
                .ejecutarReader("ObtenerDetalleMsisdnEnCargueServicioMensajeria", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then CargarResultadoConsulta(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Sub Adicionar()
        'Using dbManager As New LMDataAccess
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                .SqlParametros.Add("@msisdn", SqlDbType.BigInt).Value = _msisdn
                .SqlParametros.Add("@activaEquipoAnterior", SqlDbType.Bit).Value = _activaEquipoAnterior
                .SqlParametros.Add("@comseguro", SqlDbType.Bit).Value = _comseguro
                .SqlParametros.Add("@precioConIva", SqlDbType.Money).Value = _precioConIva
                .SqlParametros.Add("@precioSinIva", SqlDbType.Money).Value = _precioSinIva
                .SqlParametros.Add("@idClausula", SqlDbType.Int).Value = _idClausula

                .iniciarTransaccion()
                .ejecutarNonQuery("AdicionarMsisdnServicioMensajeria", CommandType.StoredProcedure)
                .confirmarTransaccion()
            Catch ex As Exception
                .abortarTransaccion()
                Throw ex
            End Try
        End With
        dbManager.Dispose()
        'End Using
    End Sub

    Public Sub Modificar()
        'Using dbManager As New LMDataAccess
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                .SqlParametros.Add("@msisdn", SqlDbType.BigInt).Value = _msisdn
                .SqlParametros.Add("@activaEquipoAnterior", SqlDbType.Bit).Value = _activaEquipoAnterior
                .SqlParametros.Add("@comSeguro", SqlDbType.Bit).Value = _comseguro
                .SqlParametros.Add("@precioConIVA", SqlDbType.Money).Value = _precioConIva
                .SqlParametros.Add("@precioSinIVA", SqlDbType.Money).Value = _precioSinIva
                .SqlParametros.Add("@idClausula", SqlDbType.Money).Value = _idClausula

                .iniciarTransaccion()
                .ejecutarNonQuery("ModificarMsisdnServicioMensajeria", CommandType.StoredProcedure)
                .confirmarTransaccion()
            Catch ex As Exception
                .abortarTransaccion()
                Throw ex
            End Try
        End With
        dbManager.Dispose()
        'End Using
    End Sub

    Public Sub Eliminar()
        'Using dbManager As New LMDataAccess
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro

                .iniciarTransaccion()
                .ejecutarNonQuery("EliminarMsisdnServicioMensajeria", CommandType.StoredProcedure)
                .confirmarTransaccion()
            Catch ex As Exception
                .abortarTransaccion()
                Throw ex
            End Try
        End With
        dbManager.Dispose()
        'End Using
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idMsisdn").ToString, _idRegistro)
                Integer.TryParse(reader("idTipoServicio").ToString, _idTipoServicio)
                _msisdn = reader("msisdn").ToString
                _activaEquipoAnterior = reader("activaEquipoAnterior").ToString
                _comseguro = reader("comseguro").ToString
                Double.TryParse(reader("precioConIva"), _precioConIva)
                Double.TryParse(reader("precioSinIva"), _precioSinIva)
                Integer.TryParse(reader("idClausula"), _idClausula)
                _clausula = reader("clausula").ToString
                _tieneCambioServicio = reader("tieneCambioServicio").ToString
                _registrado = True
                _bloquear = CBool(reader("bloquear").ToString())
                _cargadoMSISDN = reader("cargado")
            End If
        End If

    End Sub

#End Region
End Class
