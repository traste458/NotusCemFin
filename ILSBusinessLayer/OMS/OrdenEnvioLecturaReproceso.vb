Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO

Public Class OrdenEnvioLecturaReproceso

#Region "Atributos (campos)"

    Private _idOrdenEnvio As Integer
    Private _idInstruccionReproceso As Integer
    Private _idEstado As Integer
    Private _estado As String
    Private _idCreador As Integer
    Private _creador As String
    Private _fechaCreacion As DateTime
    Private _idUsuarioEnvio As Integer
    Private _usuarioEnvio As String
    Private _fechaEnvio As DateTime
    Private _observaciones As String
    Private _idFactura As Integer
    Private _idLecturaBodega As Integer
    Private _consecutivoEnvio As Integer

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdOrdenEnvio() As Integer
        Get
            Return _idOrdenEnvio
        End Get
        Set(ByVal value As Integer)
            _idOrdenEnvio = value
        End Set
    End Property

    Public Property IdInstruccionReproceso() As Integer
        Get
            Return _idInstruccionReproceso
        End Get
        Set(ByVal value As Integer)
            _idInstruccionReproceso = value
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

    Public Property Estado() As String
        Get
            Return _estado
        End Get
        Set(ByVal value As String)
            _estado = value
        End Set
    End Property

    Public Property IdCreador() As Integer
        Get
            Return _idCreador
        End Get
        Set(ByVal value As Integer)
            _idCreador = value
        End Set
    End Property

    Public Property Creador() As String
        Get
            Return _creador
        End Get
        Set(ByVal value As String)
            _creador = value
        End Set
    End Property

    Public Property FechaCreacion() As DateTime
        Get
            Return _fechaCreacion
        End Get
        Set(ByVal value As DateTime)
            _fechaCreacion = value
        End Set
    End Property

    Public Property IdUsuarioEnvio() As Integer
        Get
            Return _idUsuarioEnvio
        End Get
        Set(ByVal value As Integer)
            _idUsuarioEnvio = value
        End Set
    End Property

    Public Property UsuarioEnvio() As String
        Get
            Return _usuarioEnvio
        End Get
        Set(ByVal value As String)
            _usuarioEnvio = value
        End Set
    End Property

    Public Property FechaEvio() As DateTime
        Get
            Return _fechaEnvio
        End Get
        Set(ByVal value As DateTime)
            _fechaEnvio = value
        End Set
    End Property

    Public Property Observaciones() As String
        Get
            Return _observaciones
        End Get
        Set(ByVal value As String)
            _observaciones = value
        End Set
    End Property

    Public Property IdFactura As Integer
        Get
            Return _idFactura
        End Get
        Set(value As Integer)
            _idFactura = value
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

    Public Property IdLecturaBodega As Integer
        Get
            Return _idLecturaBodega
        End Get
        Set(value As Integer)
            _idLecturaBodega = value
        End Set
    End Property

    Public Property ConsecutivoEnvio As Integer
        Get
            Return _consecutivoEnvio
        End Get
        Set(value As Integer)
            _consecutivoEnvio = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _estado = ""
        _usuarioEnvio = ""
        _creador = ""
        _observaciones = ""
    End Sub

    Public Sub New(ByVal idOrdenEnvio As Integer)
        MyBase.New()
        _idOrdenEnvio = idOrdenEnvio
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idOrdenEnvio", SqlDbType.Int).Value = _idOrdenEnvio

                .ejecutarReader("ConsultarOrdenEnvioLecturaReproceso", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    If .Reader.Read Then

                        Integer.TryParse(.Reader("idOrdenEnvio"), _idOrdenEnvio)
                        Integer.TryParse(.Reader("idInstruccionReproceso"), _idInstruccionReproceso)
                        Integer.TryParse(.Reader("idEstado"), _idEstado)
                        If Not IsDBNull(.Reader("estado")) Then _estado = .Reader("estado").ToString
                        Integer.TryParse(.Reader("idCreador"), _idCreador)
                        If Not IsDBNull(.Reader("creador")) Then _creador = .Reader("creador")
                        If Not IsDBNull(.Reader("fechaCreacion")) Then _fechaCreacion = CDate(.Reader("fechaCreacion"))
                        Integer.TryParse(.Reader("idUsuarioEnvio"), _idUsuarioEnvio)
                        If Not IsDBNull(.Reader("usuarioEnvio")) Then _usuarioEnvio = .Reader("creador").ToString
                        If Not IsDBNull(.Reader("fechaEnvio")) Then _fechaEnvio = CDate(.Reader("fechaEnvio"))
                        If Not IsDBNull(.Reader("observaciones")) Then _observaciones = .Reader("observaciones").ToString
                        Integer.TryParse(.Reader("consecutivoEnvio"), _consecutivoEnvio)

                        .Reader.Close()
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

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim noResultado As Integer = -1
        If _idCreador > 0 Then
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        .Add("@idInstruccionReproceso", SqlDbType.Int).Value = _idInstruccionReproceso
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        If Not String.IsNullOrEmpty(_observaciones) Then .Add("@observaciones", SqlDbType.VarChar, 450).Value = _observaciones
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarOrdenEnvioLecturaReproceso", CommandType.StoredProcedure)
                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)

                    If noResultado = 0 Then
                        .confirmarTransaccion()
                        resultado.EstablecerMensajeYValor(0, "Se realizo el registro correctamente.")
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(9, "Se generó un error al realizar el registro, por favor intente nuevamente.")
                    End If
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            End With
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para realizar el registro. ")
        End If
        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim noResultado As Integer = -1
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Add("@idOrdenEnvio", SqlDbType.Int).Value = _idOrdenEnvio
                    If _idInstruccionReproceso > 0 Then .Add("@idInstruccionReproceso", SqlDbType.Int).Value = _idInstruccionReproceso
                    If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    If _idUsuarioEnvio > 0 Then .Add("@idUsuarioEnvio", SqlDbType.Int).Value = _idUsuarioEnvio
                    If _fechaEnvio > Date.MinValue Then .Add("@fechaEnvio", SqlDbType.DateTime).Value = _fechaEnvio
                    If Not String.IsNullOrEmpty(_observaciones) Then .Add("@observaciones", SqlDbType.VarChar, 450).Value = _observaciones
                    If _consecutivoEnvio > 0 Then .Add("@consecutivoEnvio", SqlDbType.Int).Value = _consecutivoEnvio
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With

                .iniciarTransaccion()
                .ejecutarNonQuery("ActualizarOrdenEnvioLecturaReproceso", CommandType.StoredProcedure)

                Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)

                If noResultado = 0 Then
                    .confirmarTransaccion()
                    resultado.EstablecerMensajeYValor(0, "Se realizo la actualización correctamente.")
                ElseIf noResultado = 1 Then
                    .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "No se encontro el identificador de la orden consultada, por favor intente nuevamente.")
                Else
                    .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(9, "Se generó un error inesperado al realizar la actualización, por favor intente el registro nuevamente.")
                End If

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
        End With
        Return resultado
    End Function

#End Region

#Region "Métodos Compartidos"

    Public Function ConsultarEnvioLecturaReproceso(Optional ByVal flag As Integer = 0) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                With .SqlParametros
                    If flag > 0 Then .Add("@flag", SqlDbType.Int).Value = flag
                    If _idOrdenEnvio > 0 Then .Add("@idOrdenEnvio", SqlDbType.Int).Value = _idOrdenEnvio
                    If _idInstruccionReproceso > 0 Then .Add("@idInstruccionReproceso", SqlDbType.Int).Value = _idInstruccionReproceso
                    If _idFactura > 0 Then .Add("@idFactura", SqlDbType.Int).Value = _idFactura
                    If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                End With
                dtDatos = .ejecutarDataTable("ConsultarOrdenEnvioLecturaReproceso", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Shared Function ObtenerSerialesReproceso(ByVal idOrdenEnvio As Integer) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            With _dbManager
                With .SqlParametros
                    .Add("@idOrdenEnvio", SqlDbType.Int).Value = idOrdenEnvio
                End With
                dtDatos = .ejecutarDataTable("LecturaSerialReproceso", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Shared Function ObtenerSerialesLecturaBodega(ByVal idOrdenLectura As Integer) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            With _dbManager
                With .SqlParametros
                    .Add("@idOrdenLecturaBodega", SqlDbType.Int).Value = idOrdenLectura
                End With
                dtDatos = .ejecutarDataTable("LecturaSerialBodega", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Shared Function ObtenerMensajeMailReprocesos(ByVal idInstruccionReproceso As Integer) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            With _dbManager
                With .SqlParametros
                    .Add("@idInstruccionReproceso", SqlDbType.Int).Value = idInstruccionReproceso
                End With
                dtDatos = .ejecutarDataTable("ObtenerMensajeMailReprocesos", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ConsultarLecturaBodegaje() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                With .SqlParametros
                    If _idLecturaBodega > 0 Then .Add("@idLectura", SqlDbType.Int).Value = _idLecturaBodega
                End With
                dtDatos = .ejecutarDataTable("ObtenerListadoLecturaBodegaje", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ActualizarSerialesLB() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim db As LMDataAccessLayer.LMDataAccess
        Try
            db = New LMDataAccess
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idLectura", SqlDbType.BigInt).Value = _idLecturaBodega
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output
                End With
                .ejecutarNonQuery("ActualizarSerialesLB", CommandType.StoredProcedure)
                If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
        Return resultado
    End Function

#End Region

#Region "Enumerados"

    Public Enum Estados
        Creado = 157
        PendienteEnvío = 158
        Enviado = 159
        Cancelado = 160
        CargaSapNoCulminada = 161
    End Enum

#End Region


End Class
