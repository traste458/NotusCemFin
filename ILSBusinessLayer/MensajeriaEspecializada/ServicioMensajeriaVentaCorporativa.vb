Imports ILSBusinessLayer.MensajeriaEspecializada
Imports LMDataAccessLayer
Imports System.Text

Namespace MensajeriaEspecializada

    Public Class ServicioMensajeriaVentaCorporativa

#Region "Atributos"

        Private _idUsuario As Integer
        Private _fechaAsignacion As DateTime
        Private _idEstado As Integer
        Private _estado As String
        Private _fechaRegistro As Date
        Private _idCiudad As Integer
        Private _nombreCiudad As String
        Private _nombreDepartamento As String
        Private _nombreCliente As String
        Private _identificacionCliente As String
        Private _telefonoContacto As String
        Private _extensionContacto As String
        Private _nombreRepresentanteLegal As String
        Private _identificacionRepresentanteLegal As String
        Private _telefonoRepresentanteLegal As String
        Private _personaContacto As String
        Private _identificacionAutorizado As String
        Private _cargoAutorizado As String
        Private _telefonoAutorizado As String
        Private _barrio As String
        Private _direccion As String
        Private _observacionDireccion As String
        Private _idGerencia As Integer
        Private _nombreGerencia As String
        Private _idCoordinador As Integer
        Private _nombreCoordinador As String
        Private _idConsultor As Integer
        Private _nombreConsultor As String
        Private _clienteClaro As Boolean
        Private _observacion As String
        Private _idJornada As Integer
        Private _jornada As String
        Private _fechaAgenda As Date
        Private _fechaAgendaString As String
        Private _fechaEntrega As Date
        Private _idFormaPago As Integer
        Private _formaPago As String
        Private _tipoServicio As String
        Private _fechaConfirmacion As Date
        Protected Friend _fechaConfirmacionString As String
        Private _confirmadoPor As String
        Private _fechaDespacho As Date
        Private _despachoPor As String
        Private _responsableEntrega As String
        Private _zona As String
        Private _bodega As String
        Private _portacion As Boolean

        Protected Friend _detalleServicio As DetalleServicioMensajeriaTipoVentaCorporativaColeccion
        Protected Friend _detalleMaterialServicio As DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion
        Protected Friend _idUsuarioConfirmacion As Integer

        Private _emailConsultor As String
        Private _emailCoordinador As String
        Private _direccionEdicion As String
        Private _idTipoServicio As String
        Private _idServicioMensajeria As Integer
        Private _numeroRadicado As Long
        Private _fechaAgendaEntrega As Date
        Private _idAgendamiento As integer
        Private _idBodega As Integer
        Private _fechaDevolucion As Date
        Private _idUsuarioDevolucion As String
        Private _tieneNovedad As String
        Private _idPersonaBackOficce As Integer
        Private _personaBackOficce As String
        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdServicioMensajeria As Integer
            Get
                Return _idServicioMensajeria
            End Get
            Set(value As Integer)
                _idServicioMensajeria = value
            End Set
        End Property

        Public Property NumeroRadicado As Long
            Get
                Return _numeroRadicado
            End Get
            Set(value As Long)
                _numeroRadicado = value
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

        Public Property FechaAsignacion As DateTime
            Get
                Return _fechaAsignacion
            End Get
            Set(value As DateTime)
                _fechaAsignacion = value
            End Set
        End Property

        Public Property NombreRepresentanteLegal As String
            Get
                Return _nombreRepresentanteLegal
            End Get
            Set(value As String)
                _nombreRepresentanteLegal = value
            End Set
        End Property

        Public Property IdentificacionRepresentanteLegal As String
            Get
                Return _identificacionRepresentanteLegal
            End Get
            Set(value As String)
                _identificacionRepresentanteLegal = value
            End Set
        End Property

        Public Property TelefonoRepresentanteLegal As String
            Get
                Return _telefonoRepresentanteLegal
            End Get
            Set(value As String)
                _telefonoRepresentanteLegal = value
            End Set
        End Property

        Public Property IdentificacionAutorizado As String
            Get
                Return _identificacionAutorizado
            End Get
            Set(value As String)
                _identificacionAutorizado = value
            End Set
        End Property

        Public Property CargoAutorizado As String
            Get
                Return _cargoAutorizado
            End Get
            Set(value As String)
                _cargoAutorizado = value
            End Set
        End Property

        Public Property TelefonoAutorizado As String
            Get
                Return _telefonoAutorizado
            End Get
            Set(value As String)
                _telefonoAutorizado = value
            End Set
        End Property

        Public Property ClienteClaro As Boolean
            Get
                Return _clienteClaro
            End Get
            Set(value As Boolean)
                _clienteClaro = value
            End Set
        End Property

        Public Property IdGerencia As Integer
            Get
                Return _idGerencia
            End Get
            Set(value As Integer)
                _idGerencia = value
            End Set
        End Property

        Public Property NombreGerencia As String
            Get
                Return _nombreGerencia
            End Get
            Protected Friend Set(value As String)
                _nombreGerencia = value
            End Set
        End Property

        Public Property IdCoordinador As Integer
            Get
                Return _idCoordinador
            End Get
            Set(value As Integer)
                _idCoordinador = value
            End Set
        End Property

        Public Property NombreCoordinador As String
            Get
                Return _nombreCoordinador
            End Get
            Set(value As String)
                _nombreCoordinador = value
            End Set
        End Property

        Public Property IdConsultor As Integer
            Get
                Return _idConsultor
            End Get
            Set(value As Integer)
                _idConsultor = value
            End Set
        End Property

        Public Property NombreConsultor As String
            Get
                Return _nombreConsultor
            End Get
            Protected Friend Set(value As String)
                _nombreConsultor = value
            End Set
        End Property

        Public Property EmailConsultor As String
            Get
                Return _emailConsultor
            End Get
            Protected Friend Set(value As String)
                _emailConsultor = value
            End Set
        End Property

        Public Property EmailCoordinador As String
            Get
                Return _emailCoordinador
            End Get
            Set(value As String)
                _emailCoordinador = value
            End Set
        End Property

        Public Property DireccionEdicion As String
            Get
                Return _direccionEdicion
            End Get
            Set(value As String)
                _direccionEdicion = value
            End Set
        End Property

        Public Property ObservacionDireccion As String
            Get
                Return _observacionDireccion
            End Get
            Set(value As String)
                _observacionDireccion = value
            End Set
        End Property

        Public Property FechaEntrega As Date
            Get
                Return _fechaEntrega
            End Get
            Set(value As Date)
                _fechaEntrega = value
            End Set
        End Property

        Public Property IdFormaPago As Integer
            Get
                Return _idFormaPago
            End Get
            Set(value As Integer)
                _idFormaPago = value
            End Set
        End Property

        Public Property FormaPago As String
            Get
                Return _formaPago
            End Get
            Set(value As String)
                _formaPago = value
            End Set
        End Property

        Public Property Registrado As Boolean
            Get
                Return _registrado
            End Get
            Set(value As Boolean)
                _registrado = value
            End Set
        End Property

        Public Property FechaRegistro As Date
            Get
                Return _fechaRegistro
            End Get
            Set(value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property IdEstado As Integer
            Get
                Return _idEstado
            End Get
            Set(value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property Estado As String
            Get
                Return _estado
            End Get
            Set(value As String)
                _estado = value
            End Set
        End Property

        Public Property IdBodega As Integer
            Get
                Return _idBodega
            End Get
            Set(value As Integer)
                _idBodega = value
            End Set
        End Property

        Public Property Portacion As Boolean
            Get
                Return _portacion
            End Get
            Set(value As Boolean)
                _Portacion = value
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

        Public Property Ciudad As String
            Get
                Return _nombreCiudad
            End Get
            Set(value As String)
                _nombreCiudad = value
            End Set
        End Property

        Public Property NombreDepartamento As String
            Get
                Return _nombreDepartamento
            End Get
            Set(value As String)
                _nombreDepartamento = value
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

        Public Property IdentificacionCliente As String
            Get
                Return _identificacionCliente
            End Get
            Set(value As String)
                _identificacionCliente = value
            End Set
        End Property

        Public Property TelefonoContacto As String
            Get
                Return _telefonoContacto
            End Get
            Set(value As String)
                _telefonoContacto = value
            End Set
        End Property

        Public Property PersonaContacto As String
            Get
                Return _personaContacto
            End Get
            Set(value As String)
                _personaContacto = value
            End Set
        End Property

        Public Property Barrio As String
            Get
                Return _barrio
            End Get
            Set(value As String)
                _barrio = value
            End Set
        End Property

        Public Property Direccion As String
            Get
                Return _direccion
            End Get
            Set(value As String)
                _direccion = value
            End Set
        End Property

        Public Property Observacion As String
            Get
                Return _observacion
            End Get
            Set(value As String)
                _observacion = value
            End Set
        End Property

        Public Property Jornada As String
            Get
                Return _jornada
            End Get
            Set(value As String)
                _jornada = value
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

        Public Property IdTipoServicio As Integer
            Get
                Return _idTipoServicio
            End Get
            Set(value As Integer)
                _idTipoServicio = value
            End Set
        End Property

        Public Property TipoServicio As String
            Get
                Return _tipoServicio
            End Get
            Set(value As String)
                _tipoServicio = value
            End Set
        End Property

        Public Property FechaConfirmacion As Date
            Get
                Return _fechaConfirmacion
            End Get
            Set(value As Date)
                _fechaConfirmacion = value
            End Set
        End Property

        Public Property ConfirmadoPor As String
            Get
                Return _confirmadoPor
            End Get
            Set(value As String)
                _confirmadoPor = value
            End Set
        End Property

        Public Property DespachoPor As String
            Get
                Return _despachoPor
            End Get
            Set(value As String)
                _despachoPor = value
            End Set
        End Property

        Public Property FechaDespacho As Date
            Get
                Return _fechaDespacho
            End Get
            Set(value As Date)
                _fechaDespacho = value
            End Set
        End Property

        Public Property ResponsableEntrega As String
            Get
                Return _responsableEntrega
            End Get
            Set(value As String)
                _responsableEntrega = value
            End Set
        End Property

        Public Property Zona As String
            Get
                Return _zona
            End Get
            Set(value As String)
                _zona = value
            End Set
        End Property

        Public Property Bodega As String
            Get
                Return _bodega
            End Get
            Set(value As String)
                _bodega = value
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

        Public Property DetalleServicio() As DetalleServicioMensajeriaTipoVentaCorporativaColeccion
            Get
                If _detalleServicio Is Nothing Then _detalleServicio = New DetalleServicioMensajeriaTipoVentaCorporativaColeccion(_idServicioMensajeria)
                Return _detalleServicio
            End Get
            Set(ByVal value As DetalleServicioMensajeriaTipoVentaCorporativaColeccion)
                _detalleServicio = value
            End Set
        End Property

        Public Property DetalleMaterialServicio() As DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion
            Get
                If _detalleMaterialServicio Is Nothing Then _detalleMaterialServicio = New DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion(_idServicioMensajeria)
                Return _detalleMaterialServicio
            End Get
            Set(ByVal value As DetalleMaterialServicioMensajeriaTipoVentaCorporativaColeccion)
                _detalleMaterialServicio = value
            End Set
        End Property

        Public Property IdUsuarioConfirmacion() As Integer
            Get
                Return _idUsuarioConfirmacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioConfirmacion = value
            End Set
        End Property

        Public Property FechaAgendaString As String
            Get
                Return _fechaAgendaString
            End Get
            Set(value As String)
                _fechaAgendaString = value
            End Set
        End Property

        Public Property FechaConfirmacionString As String
            Get
                Return _fechaConfirmacionString
            End Get
            Set(value As String)
                _fechaConfirmacionString = value
            End Set
        End Property

        Public Property TieneNovedad As String
            Get
                Return _tieneNovedad
            End Get
            Set(value As String)
                _tieneNovedad = value
            End Set
        End Property

        Public Property IdPersonaBackOficce As Integer
            Get
                Return _idPersonaBackOficce
            End Get
            Set(value As Integer)
                _idPersonaBackOficce = value
            End Set
        End Property

        Public Property PersonaBackOficce As String
            Get
                Return _personaBackOficce
            End Get
            Set(value As String)
                _personaBackOficce = value
            End Set
        End Property

#End Region

#Region "Construtores"

        Public Sub New()
            MyBase.New()
            _idTipoServicio = Enumerados.TipoServicio.VentaCorporativa
        End Sub

        Public Sub New(ByVal idServicio As Integer)
            MyBase.New()
            _idServicioMensajeria = idServicio
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Protected Overloads Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idServicioMensajeria > 0 Then .SqlParametros.Add("@listaIdServicio", SqlDbType.Int).Value = CStr(_idServicioMensajeria)
                        .ejecutarReader("ObtenerInfoGeneralServicioVentaCorporativa", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                CargarResultadoConsulta(.Reader)
                            End If
                            .Reader.Close()
                        End If

                        _detalleServicio = New DetalleServicioMensajeriaTipoVentaCorporativaColeccion(_idServicioMensajeria)

                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Overloads Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        .Add("@clienteClaro", SqlDbType.Bit).Value = _clienteClaro
                        If Not _fechaAsignacion.Equals(Date.MinValue) Then .Add("@fechaAsignacion", SqlDbType.DateTime).Value = _fechaAsignacion
                        If _nombreCliente <> String.Empty Then .Add("@nombre", SqlDbType.VarChar, 2000).Value = _nombreCliente
                        If _identificacionCliente <> String.Empty Then .Add("@identicacion", SqlDbType.VarChar, 150).Value = _identificacionCliente
                        If _telefonoContacto <> String.Empty Then .Add("@telefono", SqlDbType.VarChar, 150).Value = _telefonoContacto
                        If _extensionContacto <> String.Empty Then .Add("@extension", SqlDbType.VarChar, 20).Value = _extensionContacto
                        If Not String.IsNullOrEmpty(_nombreRepresentanteLegal) Then .Add("@nombreRepresentante", SqlDbType.VarChar, 2000).Value = _nombreRepresentanteLegal
                        If Not String.IsNullOrEmpty(_identificacionRepresentanteLegal) Then .Add("@identificacionRepresentante", SqlDbType.VarChar, 150).Value = _identificacionRepresentanteLegal
                        If Not String.IsNullOrEmpty(_telefonoRepresentanteLegal) Then .Add("@telefonoRepresentante", SqlDbType.VarChar, 150).Value = _telefonoRepresentanteLegal
                        If _personaContacto <> String.Empty Then .Add("@nombreAutorizado", SqlDbType.VarChar, 2000).Value = _personaContacto
                        If Not String.IsNullOrEmpty(_identificacionAutorizado) Then .Add("@identificacionAutorizado", SqlDbType.VarChar, 150).Value = _identificacionAutorizado
                        If Not String.IsNullOrEmpty(_cargoAutorizado) Then .Add("@cargoAutorizado", SqlDbType.VarChar, 2000).Value = _cargoAutorizado
                        If Not String.IsNullOrEmpty(_telefonoAutorizado) Then .Add("@telefonoAutorizado", SqlDbType.VarChar, 150).Value = _telefonoAutorizado
                        If _direccion <> String.Empty Then .Add("@direccion", SqlDbType.VarChar, 2000).Value = _direccion
                        If Not String.IsNullOrEmpty(_direccionEdicion) Then .Add("@direccionEdicion", SqlDbType.VarChar, 2000).Value = _direccionEdicion
                        If Not String.IsNullOrEmpty(_observacionDireccion) Then .Add("@observacionDireccion", SqlDbType.VarChar, 2000).Value = _observacionDireccion
                        If _barrio <> String.Empty Then .Add("@barrio", SqlDbType.VarChar, 2000).Value = _barrio
                        If _idGerencia > 0 Then .Add("@idGerenciaCliente", SqlDbType.Int).Value = _idGerencia
                        If _idCoordinador > 0 Then .Add("@idCoordinador", SqlDbType.Int).Value = _idCoordinador
                        If _idConsultor > 0 Then .Add("@idConsultor", SqlDbType.Int).Value = _idConsultor
                        If _observacion <> String.Empty Then .Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                        If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        .Add("@portacion", SqlDbType.Bit).Value = _portacion
                        If _idFormaPago > 0 Then .Add("@idFormaPago", SqlDbType.Int).Value = _idFormaPago
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@idServicioMensajeria", SqlDbType.Int).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarServicioTipoVentaCorporativo", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 20 Or resultado.Valor = 0 Then
                            .confirmarTransaccion()
                            _idServicioMensajeria = .SqlParametros("@idServicioMensajeria").Value
                        Else
                            .abortarTransaccion()
                        End If
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(300, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                    End If

                End With
            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                resultado.EstablecerMensajeYValor(500, "Se generó un error al realizar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function Editar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idUsuarioLog", SqlDbType.Int).Value = _idUsuario
                        .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _clienteClaro > 0 Then .Add("@clienteClaro", SqlDbType.Bit).Value = _clienteClaro
                        If Not _fechaAsignacion.Equals(Date.MinValue) Then .Add("@fechaAsignacion", SqlDbType.DateTime).Value = _fechaAsignacion
                        If _nombreCliente <> String.Empty Then .Add("@nombre", SqlDbType.VarChar, 2000).Value = _nombreCliente
                        If _identificacionCliente <> String.Empty Then .Add("@identicacion", SqlDbType.VarChar, 150).Value = _identificacionCliente
                        If _telefonoContacto <> String.Empty Then .Add("@telefono", SqlDbType.VarChar, 150).Value = _telefonoContacto
                        If _extensionContacto <> String.Empty Then .Add("@extension", SqlDbType.VarChar, 20).Value = _extensionContacto
                        If Not String.IsNullOrEmpty(_nombreRepresentanteLegal) Then .Add("@nombreRepresentante", SqlDbType.VarChar, 2000).Value = _nombreRepresentanteLegal
                        If Not String.IsNullOrEmpty(_identificacionRepresentanteLegal) Then .Add("@identificacionRepresentante", SqlDbType.VarChar, 150).Value = _identificacionRepresentanteLegal
                        If Not String.IsNullOrEmpty(_telefonoRepresentanteLegal) Then .Add("@telefonoRepresentante", SqlDbType.VarChar, 150).Value = _telefonoRepresentanteLegal
                        If _personaContacto <> String.Empty Then .Add("@nombreAutorizado", SqlDbType.VarChar, 2000).Value = _personaContacto
                        If Not String.IsNullOrEmpty(_identificacionAutorizado) Then .Add("@identificacionAutorizado", SqlDbType.VarChar, 150).Value = _identificacionAutorizado
                        If Not String.IsNullOrEmpty(_cargoAutorizado) Then .Add("@cargoAutorizado", SqlDbType.VarChar, 2000).Value = _cargoAutorizado
                        If Not String.IsNullOrEmpty(_telefonoAutorizado) Then .Add("@telefonoAutorizado", SqlDbType.VarChar, 150).Value = _telefonoAutorizado
                        If _direccion <> String.Empty Then .Add("@direccion", SqlDbType.VarChar, 2000).Value = _direccion
                        If Not String.IsNullOrEmpty(_direccionEdicion) Then .Add("@direccionEdicion", SqlDbType.VarChar, 2000).Value = _direccionEdicion
                        If Not String.IsNullOrEmpty(_observacionDireccion) Then .Add("@observacionDireccion", SqlDbType.VarChar, 2000).Value = _observacionDireccion
                        If _barrio <> String.Empty Then .Add("@barrio", SqlDbType.VarChar, 2000).Value = _barrio
                        If _idGerencia > 0 Then .Add("@idGerenciaCliente", SqlDbType.Int).Value = _idGerencia
                        If _idCoordinador > 0 Then .Add("@idCoordinador", SqlDbType.Int).Value = _idCoordinador
                        If _idConsultor > 0 Then .Add("@idConsultor", SqlDbType.Int).Value = _idConsultor
                        If _observacion <> String.Empty Then .Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                        If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        If _idFormaPago > 0 Then .Add("@idFormaPago", SqlDbType.Int).Value = _idFormaPago
                        If _idPersonaBackOficce > 0 Then .Add("@idPersonaBackOficce", SqlDbType.Int).Value = _idPersonaBackOficce
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ActualizaServicioMensajeria", CommandType.StoredProcedure)

                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), resultado.Valor)

                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                        resultado.EstablecerMensajeYValor(0, "Transacción exitosa.")
                    Else
                        resultado.EstablecerMensajeYValor(7, "Se generó un error al tratar de actualizar el servicio.")
                        .AbortarTransaccion()
                    End If

                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(500, "Se generó un error al realizar la actualización del registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ValidarRegistroServicio(ByRef dtErrores As DataTable, idFormaPago As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@idFormaPago", SqlDbType.Int).Value = idFormaPago
                        .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    End With
                    .TiempoEsperaComando = 0
                    dtErrores = .ejecutarDataTable("ValidarRegstroVentaCorporativa", CommandType.StoredProcedure)
                End With
                If dtErrores.Rows.Count = 0 Then
                    resultado.EstablecerMensajeYValor(0, "No se encontraron errores en la validación.")
                Else
                    resultado.EstablecerMensajeYValor(10, "Se encontraron errores en la validación.")
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
                resultado.EstablecerMensajeYValor(400, "Se presentó un error al validar los registros: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function CancelarRegistro() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        If _idServicioMensajeria > 0 Then .Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .EjecutarNonQuery("CancelarRegistroCorporativo", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        resultado.EstablecerMensajeYValor(300, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente. ")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
                resultado.EstablecerMensajeYValor(400, "Se generó un error al cancelar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function InformacionRutas() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        dtDatos = .EjecutarDataTable("ObtieneRutasServicioMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

        Public Function LeerSerial(ByVal serial As String, ByVal idUsuario As Integer, Optional ByVal material As String = "") As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            If _idServicioMensajeria > 0 Then
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                            .Add("@serial", SqlDbType.VarChar, 50).Value = serial
                            .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                            If Not String.IsNullOrEmpty(material) Then .Add("@material", SqlDbType.VarChar, 50).Value = material
                            .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                            .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .IniciarTransaccion()
                        .EjecutarNonQuery("RegistrarSerialEnServicioMensajeriaCorporativo", CommandType.StoredProcedure)

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
                    If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "Ocurrió un error al realizar el registro del serial: " & ex.Message)
                End Try
            Else
                resultado.EstablecerMensajeYValor(10, "No se logró establecer el identificador del servicio, para realizar la lectura. ")
            End If
            Return resultado
        End Function

        Public Function ObtenerSerialesReporteAlistamiento() As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            With dbManager
                .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                dtDatos = .EjecutarDataTable("ConsultarSerialesServicio", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Function FacturarServicio() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("RegistrarCambioServicioVentaCorporativa", CommandType.StoredProcedure)

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
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Ocurrió un error al realizar el registro del serial: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function Confirmar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Not (_idServicioMensajeria = 0 OrElse String.IsNullOrEmpty(_direccion) OrElse String.IsNullOrEmpty(_barrio) _
                OrElse _fechaAgenda = Date.MinValue OrElse _idJornada = 0 OrElse _idUsuarioConfirmacion = 0) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                        If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario

                        If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _nombreCliente <> String.Empty Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombreCliente
                        If _identificacionCliente <> String.Empty Then .SqlParametros.Add("@identificacion", SqlDbType.VarChar).Value = _identificacionCliente
                        If _telefonoContacto <> String.Empty Then .SqlParametros.Add("@telefono", SqlDbType.VarChar).Value = _telefonoContacto
                        If _extensionContacto <> String.Empty Then .SqlParametros.Add("@extension", SqlDbType.VarChar).Value = _extensionContacto
                        If Not String.IsNullOrEmpty(_nombreRepresentanteLegal) Then .SqlParametros.Add("@nombreRepresentante", SqlDbType.VarChar).Value = _nombreRepresentanteLegal
                        If Not String.IsNullOrEmpty(_identificacionRepresentanteLegal) Then .SqlParametros.Add("@identificacionRepresentante", SqlDbType.VarChar).Value = _identificacionRepresentanteLegal
                        If Not String.IsNullOrEmpty(_telefonoRepresentanteLegal) Then .SqlParametros.Add("@telefonoRepresentante", SqlDbType.VarChar).Value = _telefonoRepresentanteLegal
                        If _personaContacto <> String.Empty Then .SqlParametros.Add("@nombreAutorizado", SqlDbType.VarChar).Value = _personaContacto
                        If Not String.IsNullOrEmpty(_identificacionAutorizado) Then .SqlParametros.Add("@identificacionAutorizado", SqlDbType.VarChar).Value = _identificacionAutorizado
                        If Not String.IsNullOrEmpty(_cargoAutorizado) Then .SqlParametros.Add("@cargoAutorizado", SqlDbType.VarChar).Value = _cargoAutorizado
                        If Not String.IsNullOrEmpty(_telefonoAutorizado) Then .SqlParametros.Add("@telefonoAutorizado", SqlDbType.VarChar).Value = _telefonoAutorizado
                        If _direccion <> String.Empty Then .SqlParametros.Add("@direccion", SqlDbType.VarChar).Value = _direccion
                        If Not String.IsNullOrEmpty(_direccionEdicion) Then .SqlParametros.Add("@direccionEdicion", SqlDbType.VarChar).Value = _direccionEdicion
                        If Not String.IsNullOrEmpty(_observacionDireccion) Then .SqlParametros.Add("@observacionDireccion", SqlDbType.VarChar).Value = _observacionDireccion
                        If _barrio <> String.Empty Then .SqlParametros.Add("@barrio", SqlDbType.VarChar).Value = _barrio
                        If _idGerencia > 0 Then .SqlParametros.Add("@idGerenciaCliente", SqlDbType.Int).Value = _idGerencia
                        If _idCoordinador > 0 Then .SqlParametros.Add("@idCoordinador", SqlDbType.Int).Value = _idCoordinador
                        If _idConsultor > 0 Then .SqlParametros.Add("@idConsultor", SqlDbType.Int).Value = _idConsultor
                        .SqlParametros.Add("@clienteClaro", SqlDbType.Bit).Value = _clienteClaro
                        If _observacion <> String.Empty Then .SqlParametros.Add("@observacion", SqlDbType.VarChar).Value = _observacion
                        If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        .SqlParametros.Add("@fechaAgenda", SqlDbType.SmallDateTime).Value = _fechaAgenda
                        .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                        If _idUsuarioConfirmacion > 0 Then .SqlParametros.Add("@idUsuarioConfirmacion", SqlDbType.Int).Value = _idUsuarioConfirmacion

                        .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                        .IniciarTransaccion()

                        .EjecutarNonQuery("ConfirmarServicioMensajeriaTipoVentaCorporativa", CommandType.StoredProcedure)
                        If Not IsDBNull(.SqlParametros("@result").Value) Then
                            resultado.Valor = CShort(.SqlParametros("@result").Value)
                            If resultado.Valor = 0 Then
                                resultado.Mensaje = "El servicio fue confirmado de manera exitosa."
                                .ConfirmarTransaccion()
                            Else
                                Select Case resultado.Valor
                                    Case 1
                                        resultado.Mensaje = "Ocurrió un error inesperado al confirmar el servicio. Por favor intente nuevamente"
                                    Case 2
                                        resultado.Mensaje = "No se pudo realizar la reserva de inventario para atender el servicio. Por favor intente nuevamente"
                                    Case 3
                                        resultado.Mensaje = "Uno o más materiales del servicio ya no tienen disponibilidad de inventario."
                                    Case 4
                                        resultado.Mensaje = "No se encuentra capacidad de entrega para la fecha: " + _fechaAgenda.ToString("dd/MM/yyyy") + " y jornada: " + _jornada

                                End Select

                                .AbortarTransaccion()
                            End If
                        Else
                            Throw New Exception("Ocurrió un error interno al confirmar el servicio. Por favor intente nuevamente")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                End Try
            Else
                resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para realizar la confirmación. ")
            End If

            Return resultado
        End Function

        Public Function Legalizar(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                If _idServicioMensajeria > 0 Then
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@idUsuarioLegaliza", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .EjecutarNonQuery("RegistrarLegalizacionVentaCorporativa", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@respuesta").Value, resultado.Valor)

                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                            resultado.Mensaje = "Se realizó la legalización del servicio exitosamente."
                        Else
                            .abortarTransaccion()
                            resultado.Mensaje = "No se logró realizar la lagalización del servicio: [" & resultado.Valor & "]"
                        End If
                    End With
                End If
            End Using
            Return resultado
        End Function

        Public Function EditarMsisdn(ByVal msisdn As String, ByVal msisdnAnt As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    If _idServicioMensajeria > 0 Then
                        With dbManager
                            .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                            .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 150).Value = msisdn
                            .SqlParametros.Add("@msisdnAnt", SqlDbType.VarChar, 150).Value = msisdnAnt
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .IniciarTransaccion()
                            .EjecutarNonQuery("EditarMsisdnVentaCorporativa", CommandType.StoredProcedure)

                            If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                                resultado.Valor = .SqlParametros("@resultado").Value
                                resultado.Mensaje = .SqlParametros("@mensaje").Value
                                If resultado.Valor = 0 Then
                                    .ConfirmarTransaccion()
                                Else
                                    .AbortarTransaccion()
                                End If
                            Else
                                resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente. ")
                            End If
                        End With
                    End If
                Catch ex As Exception
                    If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(500, "Se generó un error al realizar la actualización: " & ex.Message)
                End Try

            End Using
            Return resultado
        End Function

        Public Function LiberarServicio() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    If _idServicioMensajeria > 0 Then
                        With dbManager
                            .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .IniciarTransaccion()
                            .EjecutarNonQuery("LiberarServicioVentaCorporativa", CommandType.StoredProcedure)

                            If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                                resultado.Valor = .SqlParametros("@resultado").Value
                                resultado.Mensaje = .SqlParametros("@mensaje").Value
                                If resultado.Valor = 0 Then
                                    .ConfirmarTransaccion()
                                Else
                                    .AbortarTransaccion()
                                End If
                            Else
                                resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente. ")
                            End If
                        End With
                    End If
                Catch ex As Exception
                    If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(500, "Se generó un error al realizar la actualización: " & ex.Message)
                End Try

            End Using
            Return resultado
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idServicioMensajeria").ToString(), _idServicioMensajeria)
                    Long.TryParse(reader("numeroRadicado").ToString(), _numeroRadicado)
                    Integer.TryParse(reader("idEstado").ToString(), _idEstado)
                    Integer.TryParse(reader("idAgendamiento").ToString(), _idAgendamiento)
                    Integer.TryParse(reader("idBodega").ToString(), _idBodega)
                    Integer.TryParse(reader("idCiudad").ToString(), _idCiudad)
                    _nombreCiudad = reader("nombreCiudad").ToString()
                    _nombreDepartamento = reader("nombreDepartamento").ToString()
                    _nombreCliente = reader("nombreCliente").ToString()
                    _identificacionCliente = reader("identificacionCliente").ToString()
                    _nombreRepresentanteLegal = reader("nombreRepresentanteLegal").ToString()
                    _identificacionRepresentanteLegal = reader("identificacionRepresentanteLegal").ToString()
                    _telefonoRepresentanteLegal = reader("telefonoRepresentanteLegal").ToString()
                    _personaContacto = reader("nombreAutorizado").ToString()
                    _identificacionAutorizado = reader("identificacionAutorizado").ToString()
                    _cargoAutorizado = reader("cargoAutorizado").ToString()
                    _telefonoAutorizado = reader("telefonoAutorizado").ToString()
                    _barrio = reader("barrio").ToString()
                    _direccion = reader("direccion").ToString()
                    _observacionDireccion = reader("observacionDireccion").ToString()
                    _telefonoContacto = reader("telefono").ToString()
                    _extensionContacto = reader("extension").ToString()
                    _estado = reader("nombreEstado").ToString()
                    _idGerencia = reader("idGerenciaCliente")
                    _nombreGerencia = reader("nombreGerencia").ToString
                    _idCoordinador = reader("idPersonaCoordinador")
                    _nombreCoordinador = reader("nombreCoordinador")
                    _emailCoordinador = reader("emailCoordinador")
                    _idConsultor = reader("idPersonaConsultor")
                    _nombreConsultor = reader("nombreConsultor")
                    If Not IsDBNull(reader("emailConsultor")) Then _emailConsultor = reader("emailConsultor").ToString
                    _fechaRegistro = CDate(reader("fechaRegistro").ToString())
                    _observacion = reader("observacion").ToString()
                    If Not IsDBNull(reader("fechaAgenda")) Then _fechaAgenda = CDate(reader("fechaAgenda"))
                    If Not IsDBNull(reader("fechaAgenda")) Then _fechaAgendaString = CDate(reader("fechaAgenda"))
                    Integer.TryParse(reader("idJornada").ToString(), _idJornada)
                    _jornada = reader("jornada").ToString()
                    Integer.TryParse(reader("idTipoServicio").ToString(), _idTipoServicio)
                    If Not IsDBNull(reader("fechaEntrega")) Then _fechaAgendaEntrega = CDate(reader("fechaEntrega"))
                    If Not IsDBNull(reader("fechaDevolucion")) Then _fechaDevolucion = CDate(reader("fechaDevolucion"))
                    Integer.TryParse(reader("idUsuarioDevolucion").ToString(), _idUsuarioDevolucion)
                    If Not IsDBNull(reader("fechaentrega")) Then _fechaEntrega = CDate(reader("fechaentrega"))
                    _formaPago = reader("formaPago").ToString()
                    _clienteClaro = reader("clienteClaro")
                    _portacion = reader("portacion")
                    _tipoServicio = reader("tipoServicio")
                    If Not IsDBNull(reader("fechaConfirmacion")) Then _fechaConfirmacion = CDate(reader("fechaConfirmacion"))
                    If Not IsDBNull(reader("fechaConfirmacion")) Then _fechaConfirmacionString = CDate(reader("fechaConfirmacion"))
                    If Not IsDBNull(reader("confirmadoPor")) Then _confirmadoPor = reader("confirmadoPor").ToString()
                    If Not IsDBNull(reader("fechaDespacho")) Then _fechaDespacho = CDate(reader("fechaDespacho"))
                    If Not IsDBNull(reader("despachoPor")) Then _despachoPor = reader("despachoPor").ToString()
                    If Not IsDBNull(reader("responsableEntrega")) Then _responsableEntrega = reader("responsableEntrega").ToString()
                    If Not IsDBNull(reader("zona")) Then _zona = reader("zona").ToString()
                    If Not IsDBNull(reader("bodega")) Then _bodega = reader("bodega").ToString()
                    If Not IsDBNull(reader("tieneNovedad")) Then _tieneNovedad = reader("tieneNovedad").ToString()
                    If Not IsDBNull(reader("idPersonaBackOficce")) Then Integer.TryParse(reader("idPersonaBackOficce"), IdPersonaBackOficce)
                    If Not IsDBNull(reader("personaBackOficce")) Then _personaBackOficce = reader("personaBackOficce").ToString()
                    _registrado = True
                End If
            End If

        End Sub

#End Region

    End Class

End Namespace
