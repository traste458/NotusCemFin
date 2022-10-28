Imports LMDataAccessLayer
Imports System.IO
Imports System.Web

Public Class MsisdnTemporalServicioMensajeria

#Region "Atributos (Campos)"

    Private _msisdn As String
    Private _region As String
    Private _idRegion As Integer
    Private _precioUnitario As Integer
    Private _precioEspecial As String
    Private _tipoSim As String
    Private _idTipoSim As Integer
    Private _materialEquipo As String
    Private _requiereSim As String
    Private _idUsuario As Integer

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property msisdn As String
        Get
            Return _msisdn
        End Get
        Set(value As String)
            _msisdn = value
        End Set
    End Property

    Public Property Region As String
        Get
            Return _region
        End Get
        Set(value As String)
            _region = value
        End Set
    End Property

    Public Property IdRegion As Integer
        Get
            Return _idRegion
        End Get
        Set(value As Integer)
            _idRegion = value
        End Set
    End Property

    Public Property PrecioUnitario As Integer
        Get
            Return _precioUnitario
        End Get
        Set(value As Integer)
            _precioUnitario = value
        End Set
    End Property

    Public Property PrecioEspecial As String
        Get
            Return _precioEspecial
        End Get
        Set(value As String)
            _precioEspecial = value
        End Set
    End Property

    Public Property TipoSim As String
        Get
            Return _tipoSim
        End Get
        Set(value As String)
            _tipoSim = value
        End Set
    End Property

    Public Property IdTipoSim As Integer
        Get
            Return _idTipoSim
        End Get
        Set(value As Integer)
            _idTipoSim = value
        End Set
    End Property

    Public Property MaterialEquipo As String
        Get
            Return _materialEquipo
        End Get
        Set(value As String)
            _materialEquipo = value
        End Set
    End Property

    Public Property RequiereSim As String
        Get
            Return _requiereSim
        End Get
        Set(value As String)
            _requiereSim = value
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

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal msisdn As String, ByVal idUsuario As Integer)
        _msisdn = msisdn
        _idUsuario = idUsuario
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If Not String.IsNullOrEmpty(_msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 250).Value = _msisdn
                If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                .ejecutarReader("ConsultarMsisdnTemporalesServicioMensajeria", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        CargarResultadoConsulta(.Reader)
                        _registrado = True
                    End If
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Púbicos"

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@msisdn", SqlDbType.VarChar, 50).Value = _msisdn
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If _idRegion > 0 Then .Add("@idRegion", SqlDbType.Int).Value = _idRegion
                    If Not String.IsNullOrEmpty(_region) Then .Add("@region", SqlDbType.VarChar, 10).Value = _region
                    If _precioUnitario > 0 Then .Add("@precioUnitario", SqlDbType.Int).Value = _precioUnitario
                    If Not String.IsNullOrEmpty(_precioEspecial) Then .Add("@precioEspecial", SqlDbType.VarChar, 10).Value = _precioEspecial
                    If Not String.IsNullOrEmpty(_tipoSim) Then .Add("@tipoSim", SqlDbType.VarChar, 50).Value = _tipoSim
                    If _idTipoSim > 0 Then .Add("@idTipoSim", SqlDbType.Int).Value = _idTipoSim
                    If Not String.IsNullOrEmpty(_materialEquipo) Then .Add("@material", SqlDbType.VarChar, 50).Value = _materialEquipo
                    If Not String.IsNullOrEmpty(_requiereSim) Then .Add("@requiereSim", SqlDbType.VarChar, 50).Value = _requiereSim
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                .IniciarTransaccion()
                .EjecutarNonQuery("ActualizaMsisdnTransitorio", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(300, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al realizar la actualización: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                If Not IsDBNull(reader("msisdn")) Then _msisdn = reader("msisdn").ToString()
                If Not IsDBNull(reader("region")) Then _region = reader("region").ToString()
                If Not IsDBNull(reader("idRegion")) Then Integer.TryParse(reader("idRegion"), _idRegion)
                If Not IsDBNull(reader("precioUnitario")) Then _precioUnitario = reader("precioUnitario").ToString()
                If Not IsDBNull(reader("precioEspecial")) Then _precioEspecial = reader("precioEspecial").ToString()
                If Not IsDBNull(reader("tipoSim")) Then _tipoSim = reader("tipoSim").ToString()
                If Not IsDBNull(reader("idClase")) Then Integer.TryParse(reader("idClase"), _idTipoSim)
                If Not IsDBNull(reader("materialEquipo")) Then _materialEquipo = reader("materialEquipo").ToString()
                If Not IsDBNull(reader("requiereSim")) Then _requiereSim = reader("requiereSim")
                If Not IsDBNull(reader("idUsuario")) Then Integer.TryParse(reader("idUsuario"), _idUsuario)
            End If
        End If
    End Sub

#End Region

End Class
