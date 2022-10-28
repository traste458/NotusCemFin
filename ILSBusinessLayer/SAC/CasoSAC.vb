Imports LMDataAccessLayer

Namespace SAC

    Public Class CasoSAC

#Region "Atributos"

        Private _idCaso As Integer
        Private _consecutivo As String
        Private _idCliente As Short
        Private _cliente As String
        Private _idTipoServicio As Integer
        Private _tipoDeServicio As String
        Private _idClaseServicio As Short
        Private _claseDeServicio As String
        Private _idRemitente As Integer
        Private _remitente As String
        Private _descripcion As String
        Private _fechaRecepcion As Date
        Private _idGeneradorInconformidad As Short
        Private _generadorInconformidad As String
        Private _respuesta As String
        Private _fechaRespuesta As Date
        Private _generoCobro As Boolean
        Private _valorCobro As Decimal
        Private _idResponsableCobro As Short
        Private _responsableCobro As String
        Private _idTramitador As Integer
        Private _tramitador As String
        Private _fechaRegistro As Date
        Private _idEstado As Short
        Private _estado As String
        Private _idUsuarioRegistra As Integer
        Private _usuarioRegistra As String
        Private _idUsuarioCierra As Integer
        Private _usuarioCierra As String
        Private _fechaCierre As Date
        Private _observacion As String
        Private _registrado As Boolean
        Private _idUnidadNegocio As Byte
        Private _consecutivoServicio As Integer
        Private _detalleSerial As SerialSACColeccion
        Private _detalleGestion As InfoGestionCasoSACColeccion

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdCaso() As Integer
            Get
                Return _idCaso
            End Get
        End Property

        Public ReadOnly Property Consecutivo() As String
            Get
                Return _consecutivo
            End Get
        End Property

        Public Property IdCliente() As Short
            Get
                Return _idCliente
            End Get
            Set(ByVal value As Short)
                _idCliente = value
            End Set
        End Property

        Public ReadOnly Property Cliente() As String
            Get
                Return _cliente
            End Get
        End Property

        Public Property IdTipoServicio() As Integer
            Get
                Return _idTipoServicio
            End Get
            Set(ByVal value As Integer)
                _idTipoServicio = value
            End Set
        End Property

        Public ReadOnly Property TipoDeServicio() As String
            Get
                Return _tipoDeServicio
            End Get
        End Property

        Public ReadOnly Property IdClaseServicio() As Short
            Get
                Return _idClaseServicio
            End Get
        End Property

        Public ReadOnly Property ClaseDeServicio() As String
            Get
                Return _claseDeServicio
            End Get
        End Property

        Public Property IdRemitente() As Integer
            Get
                Return _idRemitente
            End Get
            Set(ByVal value As Integer)
                _idRemitente = value
            End Set
        End Property

        Public ReadOnly Property Remitente() As String
            Get
                Return _remitente
            End Get
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property FechaDeRecepcion() As Date
            Get
                Return _fechaRecepcion
            End Get
            Set(ByVal value As Date)
                _fechaRecepcion = value
            End Set
        End Property

        Public Property IdGeneradorInconformidad() As Integer
            Get
                Return _idGeneradorInconformidad
            End Get
            Set(ByVal value As Integer)
                _idGeneradorInconformidad = value
            End Set
        End Property

        Public ReadOnly Property GeneradorInconformidad() As String
            Get
                Return _generadorInconformidad
            End Get
        End Property

        Public Property Respuesta() As String
            Get
                Return _respuesta
            End Get
            Set(ByVal value As String)
                _respuesta = value
            End Set
        End Property

        Public Property FechaRespuesta() As Date
            Get
                Return _fechaRespuesta
            End Get
            Set(ByVal value As Date)
                _fechaRespuesta = value
            End Set
        End Property

        Public Property GeneroCobro() As Boolean
            Get
                Return _generoCobro
            End Get
            Set(ByVal value As Boolean)
                _generoCobro = value
            End Set
        End Property

        Public Property ValorCobro() As Decimal
            Get
                Return _valorCobro
            End Get
            Set(ByVal value As Decimal)
                _valorCobro = value
            End Set
        End Property

        Public Property IdResponsableCobro() As Short
            Get
                Return _idResponsableCobro
            End Get
            Set(ByVal value As Short)
                _idResponsableCobro = value
            End Set
        End Property

        Public ReadOnly Property ResponsableCobro() As String
            Get
                Return _responsableCobro
            End Get
        End Property

        Public Property IdTramitador() As Integer
            Get
                Return _idTramitador
            End Get
            Set(ByVal value As Integer)
                _idTramitador = value
            End Set
        End Property

        Public ReadOnly Property Tramitador() As String
            Get
                Return _tramitador
            End Get
        End Property

        Public ReadOnly Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
        End Property

        Public Property IdEstado() As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        Public ReadOnly Property Estado() As String
            Get
                Return _estado
            End Get
        End Property

        Public Property IdUsuarioRegistra() As Integer
            Get
                Return _idUsuarioRegistra
            End Get
            Set(ByVal value As Integer)
                _idUsuarioRegistra = value
            End Set
        End Property

        Public ReadOnly Property UsuarioRegistra() As String
            Get
                Return _usuarioRegistra
            End Get
        End Property

        Public Property IdUsuarioCierra() As Integer
            Get
                Return _idUsuarioCierra
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCierra = value
            End Set
        End Property

        Public ReadOnly Property UsuarioCierra() As String
            Get
                Return _usuarioCierra
            End Get
        End Property

        Public ReadOnly Property FechaCierre() As Date
            Get
                Return _fechaCierre
            End Get
        End Property

        Public Property Observacion() As String
            Get
                Return _observacion
            End Get
            Set(ByVal value As String)
                _observacion = value
            End Set
        End Property

        Public ReadOnly Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
        End Property

        Public Property IdUnidadNegocio() As Byte
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Byte)
                _idUnidadNegocio = value
            End Set
        End Property

        Public Property ConsecutivoServicio() As Integer
            Get
                Return _consecutivoServicio
            End Get
            Set(ByVal value As Integer)
                _consecutivoServicio = value
            End Set
        End Property

        Public ReadOnly Property DetalleSerial() As SerialSACColeccion
            Get
                If _idCaso > 0 Then _detalleSerial = New SerialSACColeccion(_idCaso)
                If _detalleSerial Is Nothing Then _detalleSerial = New SerialSACColeccion
                Return _detalleSerial
            End Get
        End Property

        Public ReadOnly Property DetalleGestion() As InfoGestionCasoSACColeccion
            Get
                If _idCaso > 0 Then _detalleGestion = New InfoGestionCasoSACColeccion(_idCaso)
                If _detalleGestion Is Nothing Then _detalleGestion = New InfoGestionCasoSACColeccion
                Return _detalleGestion
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _consecutivo = ""
            _cliente = ""
            _tipoDeServicio = ""
            _claseDeServicio = ""
            _remitente = ""
            _descripcion = ""
            _generadorInconformidad = ""
            _respuesta = ""
            _responsableCobro = ""
            _tramitador = ""
            _estado = ""
            _usuarioRegistra = ""
            _usuarioCierra = ""            
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            CargarDatos(identificador)
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal identificador As Integer)
            Dim dbManager As New LMDataAccess
            Dim idPerfil As Integer
            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                With dbManager
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                    .SqlParametros.Add("@idCaso", SqlDbType.Int).Value = identificador
                    .ejecutarReader("ConsultarCasoSAC", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idCaso").ToString, _idCaso)
                            _consecutivo = .Reader("consecutivo").ToString                            
                            Short.TryParse(.Reader("idCliente").ToString, _idCliente)
                            _cliente = .Reader("cliente").ToString
                            Integer.TryParse(.Reader("idTipoServicio"), _idTipoServicio)
                            _tipoDeServicio = .Reader("tipoDeServicio").ToString
                            Short.TryParse(.Reader("idClaseServicio"), _idClaseServicio)
                            _claseDeServicio = .Reader("claseDeServicio").ToString
                            Integer.TryParse(.Reader("idRemitente").ToString, _idRemitente)
                            _remitente = .Reader("remitente").ToString
                            _descripcion = .Reader("descripcion").ToString
                            Date.TryParse(.Reader("fechaRecepcion").ToString, _fechaRecepcion)
                            Short.TryParse(.Reader("idGeneradorInconformidad").ToString, _idGeneradorInconformidad)
                            _generadorInconformidad = .Reader("generadorInconformidad").ToString
                            _respuesta = .Reader("respuesta").ToString
                            Date.TryParse(.Reader("fechaRespuesta").ToString, _fechaRespuesta)
                            Boolean.TryParse(.Reader("generoCobro").ToString, _generoCobro)
                            Decimal.TryParse(.Reader("valorCobro").ToString, _valorCobro)
                            Short.TryParse(.Reader("idResponsableCobro").ToString, _idResponsableCobro)
                            _responsableCobro = .Reader("responsableCobro").ToString
                            Integer.TryParse(.Reader("idTramitador").ToString, _idTramitador)
                            _tramitador = .Reader("tramitador").ToString
                            Date.TryParse(.Reader("fechaRegistro").ToString, _fechaRegistro)
                            Short.TryParse(.Reader("idEstado").ToString, _idEstado)
                            _estado = .Reader("estado").ToString
                            Integer.TryParse(.Reader("idUsuarioRegistra").ToString, _idUsuarioRegistra)
                            _usuarioRegistra = .Reader("usuarioRegistra").ToString
                            Integer.TryParse(.Reader("idUsuarioCierra").ToString, _idUsuarioCierra)
                            _usuarioCierra = .Reader("usuarioCierra").ToString
                            Date.TryParse(.Reader("fechaCierre").ToString, _fechaCierre)
                            _observacion = .Reader("observaciones").ToString
                            _registrado = True
                            Byte.TryParse(.Reader("idUnidadNegocio").ToString(), _idUnidadNegocio)
                            Integer.TryParse(.Reader("consecutivoServicio").ToString(), _consecutivoServicio)
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Function CrearEstructuraTablSerialPendienteRegistro() As DataTable
            Dim dtAux As New DataTable
            Dim pk(0) As DataColumn
            With dtAux
                .Columns.Add("serial", GetType(String))
                pk(0) = .Columns("serial")
                .PrimaryKey = pk
            End With

            Return dtAux
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub EstablecerIdentificador(ByVal identificador As Integer)
            _idCaso = identificador
        End Sub

        Protected Friend Sub EstablecerConsecutivo(ByVal valor As String)
            _consecutivo = valor
        End Sub

        Protected Friend Sub EstablecerCliente(ByVal valor As String)
            _cliente = valor
        End Sub

        Protected Friend Sub EstablecerTipoDeServicio(ByVal valor As String)
            _tipoDeServicio = valor
        End Sub

        Protected Friend Sub EstablecerIdClaseServicio(ByVal valor As String)
            _idClaseServicio = valor
        End Sub

        Protected Friend Sub EstablecerClaseDeServicio(ByVal valor As String)
            _claseDeServicio = valor
        End Sub

        Protected Friend Sub EstablecerRemitente(ByVal valor As String)
            _remitente = valor
        End Sub

        Protected Friend Sub EstablecerGeneradorInconformidad(ByVal valor As String)
            _generadorInconformidad = valor
        End Sub

        Protected Friend Sub EstablecerReponsableCobro(ByVal valor As String)
            _responsableCobro = valor
        End Sub

        Protected Friend Sub EstablecerTramitador(ByVal valor As String)
            _tramitador = valor
        End Sub

        Protected Friend Sub EstablecerFechaRegistro(ByVal valor As Date)
            _fechaRegistro = valor
        End Sub

        Protected Friend Sub EstablecerEstado(ByVal valor As String)
            _estado = valor
        End Sub

        Protected Friend Sub EstablecerUsuarioRegistra(ByVal valor As String)
            _usuarioRegistra = valor
        End Sub

        Protected Friend Sub EstablecerUsuarioCierra(ByVal valor As String)
            _usuarioCierra = valor
        End Sub

        Protected Friend Sub EstablecerFechaCierre(ByVal valor As Date)
            _fechaCierre = valor
        End Sub

        Protected Friend Sub MarcarComoRegistrado()
            _registrado = True
        End Sub

        Protected Friend Sub EstablecerUnidadNegocio(ByVal valor As Byte)
            _idUnidadNegocio = valor
        End Sub

        Protected Friend Sub EstablecerConsecutivoServicio(ByVal valor As Integer)
            _consecutivoServicio = valor
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Me._idCliente > 0 AndAlso Me._idTipoServicio > 0 AndAlso Me._idRemitente > 0 _
                AndAlso Me._descripcion IsNot Nothing AndAlso Me._descripcion.Trim.Length > 0 _
                AndAlso Me._fechaRecepcion > Date.MinValue AndAlso Me._idTramitador > 0 AndAlso Me._idUsuarioRegistra Then

                Dim idPerfil As Integer
                Dim dbManager As New LMDataAccess
                Try
                    If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                        Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                    Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                    _idUnidadNegocio = usuarioUnidad.IdUnidadNegocio
                    resultado.Valor = 1
                    With dbManager
                        With .SqlParametros
                            .Add("@idCliente", SqlDbType.SmallInt).Value = Me._idCliente
                            .Add("@idTipoServicio", SqlDbType.Int).Value = Me._idTipoServicio
                            .Add("@idRemitente", SqlDbType.Int).Value = Me._idRemitente
                            .Add("@descripcion", SqlDbType.VarChar, 2000).Value = Me._descripcion
                            .Add("@fechaRecepcion", SqlDbType.SmallDateTime).Value = Me._fechaRecepcion
                            .Add("@idTramitador", SqlDbType.Int).Value = Me._idTramitador
                            .Add("@idUsuarioRegistra", SqlDbType.Int).Value = Me._idUsuarioRegistra
                            If Me._observacion IsNot Nothing AndAlso Me._observacion.Trim.Length > 0 Then _
                                .Add("@observacion", SqlDbType.VarChar, 2000).Value = Me._observacion
                            .Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = Me._idUnidadNegocio
                            .Add("@consecutivoServicio", SqlDbType.Int).Value = Me._consecutivoServicio
                            .Add("@idCaso", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@return", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .IniciarTransaccion()
                        .EjecutarNonQuery("RegistrarCasoSAC", CommandType.StoredProcedure)
                        resultado.Valor = CShort(.SqlParametros("@return").Value)
                        If resultado.Valor = 0 Then
                            Me._idCaso = CInt(.SqlParametros("@idCaso").Value)
                            If Me._detalleSerial IsNot Nothing AndAlso Me._detalleSerial.Count > 0 Then
                                For index As Integer = 0 To Me._detalleSerial.Count - 1
                                    CType(_detalleSerial(index), SerialCasoSAC).IdCaso = Me._idCaso
                                Next
                                Dim dtSerial As DataTable = Me._detalleSerial.GenerarDataTable()
                                .InicilizarBulkCopy()
                                With .BulkCopy
                                    .DestinationTableName = "SerialCasoSAC"
                                    .ColumnMappings.Add("idCaso", "idCaso")
                                    .ColumnMappings.Add("serial", "serial")
                                    .ColumnMappings.Add("idTipoSerial", "idTipoSerial")
                                    .ColumnMappings.Add("idPos", "idPos")
                                    .ColumnMappings.Add("idCoordinador", "idCoordinador")
                                    .ColumnMappings.Add("idSupervisor", "idSupervisor")
                                    .WriteToServer(dtSerial)
                                End With
                            End If
                            If Me._detalleSerial IsNot Nothing AndAlso Me._detalleSerial.Count > 0 Then Me._detalleSerial.Clear()
                            Me.CargarDatos(Me._idCaso)
                            .ConfirmarTransaccion()
                        Else
                            resultado.Mensaje = "Imposible registrar el Caso. Ocurrió un error inesperado al tratar de realizar el registro. Por favor intente nuevamente"
                            If .estadoTransaccional Then .AbortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.AbortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.Valor = 2
                resultado.Mensaje = "No se han proporcionado todos los datos requeridos para procesar el registro de los datos. Por favor verifique"
            End If

            Return resultado
        End Function

        Public Function CerrarCaso() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Me._idCaso > 0 AndAlso Me._respuesta IsNot Nothing AndAlso Me._respuesta.Trim.Length > 0 _
                AndAlso Me._fechaRespuesta > Date.MinValue AndAlso Me._idGeneradorInconformidad > 0 _
                AndAlso Me._idUsuarioCierra Then

                Dim dbManager As New LMDataAccess
                Try
                    resultado.Valor = 2
                    With dbManager
                        .SqlParametros.Add("@idCaso", SqlDbType.Int).Value = Me._idCaso
                        .SqlParametros.Add("@respuesta", SqlDbType.VarChar, 2000).Value = Me._respuesta
                        .SqlParametros.Add("@fechaRespuesta", SqlDbType.SmallDateTime).Value = Me._fechaRespuesta
                        .SqlParametros.Add("@idGeneradorInconformidad", SqlDbType.SmallInt).Value = Me._idGeneradorInconformidad
                        .SqlParametros.Add("@generoCobro", SqlDbType.Bit).Value = Me._generoCobro
                        If Me._generoCobro Then
                            .SqlParametros.Add("@valorCobro", SqlDbType.Decimal).Value = Me._valorCobro
                            .SqlParametros.Add("@idResponsableCobro", SqlDbType.Int).Value = Me._idResponsableCobro
                        End If

                        .SqlParametros.Add("@idUsuarioCierra", SqlDbType.Int).Value = Me._idUsuarioCierra
                        .SqlParametros.Add("@return", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                        .IniciarTransaccion()
                        .EjecutarNonQuery("CerrarCasoSAC", CommandType.StoredProcedure)
                        resultado.Valor = CShort(.SqlParametros("@return").Value)
                        If resultado.Valor = 0 Then
                            Me.CargarDatos(Me._idCaso)
                            If .estadoTransaccional Then .ConfirmarTransaccion()
                        Else
                            Select Case resultado.Valor
                                Case 1
                                    resultado.Mensaje = "El caso que está tratando de cerrar no existe en la base de datos. Por favor verifique"
                                Case 2
                                    resultado.Mensaje = "Imposible registrar el Caso. Ocurrió un error inesperado al tratar de cerrar caso. Por favor intente nuevamente"
                            End Select

                            If .estadoTransaccional Then .AbortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.AbortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.Valor = 3
                resultado.Mensaje = "No se han proporcionado todos los datos requeridos para procesar el registro de los datos. Por favor verifique"
            End If

            Return resultado




        End Function

        Public Sub ActualizarInformacionBasica()
            Dim db As New LMDataAccess
            Try
                If _idCaso > 0 Then
                    With db.SqlParametros
                        .Add("@idCaso", SqlDbType.Int).Value = _idCaso
                        .Add("@idCliente", SqlDbType.SmallInt).Value = _idCliente
                        .Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                        .Add("@idRemitente", SqlDbType.Int).Value = _idRemitente
                        .Add("@descripcion", SqlDbType.VarChar).Value = _descripcion
                        .Add("@fechaRecepcion", SqlDbType.DateTime).Value = _fechaRecepcion
                        .Add("@idGeneradorInconformidad", SqlDbType.SmallInt).Value = _idGeneradorInconformidad
                        .Add("@respuesta", SqlDbType.VarChar).Value = _respuesta
                        If _fechaRespuesta <> Date.MinValue Then .Add("@fechaRespuesta", SqlDbType.DateTime).Value = _fechaRespuesta
                        .Add("@generoCobro", SqlDbType.Bit).Value = _generoCobro
                        .Add("@valorCobro", SqlDbType.Decimal).Value = _valorCobro
                        .Add("@idResponsableCobro", SqlDbType.SmallInt).Value = _idResponsableCobro
                        .Add("@idTramitador", SqlDbType.Int).Value = _idTramitador
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                        .Add("@idUsuarioCierra", SqlDbType.Int).Value = _idUsuarioCierra
                        .Add("@observaciones", SqlDbType.VarChar).Value = _observacion
                    End With
                    db.EjecutarNonQuery("ActualizarCasoSAC", CommandType.StoredProcedure)
                Else
                    Throw New Exception("No existe una instancia del caso.")
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

#End Region

    End Class

End Namespace

