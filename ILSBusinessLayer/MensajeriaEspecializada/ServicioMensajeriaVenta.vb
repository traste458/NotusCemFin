Imports ILSBusinessLayer
Imports ILSBusinessLayer.Inventario
Imports LMDataAccessLayer
Imports System.Web

Namespace MensajeriaEspecializada

    Public Class ServicioMensajeriaVenta
        Inherits ServicioMensajeria
        Implements IServicioMensajeria

#Region "Atributos"

        Protected _idCampania As Integer
        Protected _nombreCampania As String
        Protected _idPlanVenta As Integer
        Protected _nombrePlanVenta As String
        Protected _cfmPlan As Double
        Protected _direccionEdicion As String
        Protected _observacionDireccion As String
        Protected _telefonoFijo As String
        Protected _telefonoMovil As String
        Protected _idMedioPago As Short
        Protected _nombreMedioPago As String
        Protected _idTipoMigracion As Short
        Protected _nombreTipoMigracion As String
        Protected _valorRecaudado As Double
        Protected _numeroContrato As Long
        Protected _material As String
        Protected Shadows _minsColeccion As DetalleMsisdnEnServicioMensajeriaTipoVentaColeccion
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _idTipoServicio = Enumerados.TipoServicio.Venta
        End Sub

        Public Sub New(idServicio As Integer)
            Me._idServicioMensajeria = idServicio
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property IdCampania As Integer
            Get
                Return _idCampania
            End Get
            Set(value As Integer)
                _idCampania = value
            End Set
        End Property

        Public Property NombreCampania As String
            Get
                Return _nombreCampania
            End Get
            Set(value As String)
                _nombreCampania = value
            End Set
        End Property

        Public Property IdPlanVenta As Integer
            Get
                Return _idPlanVenta
            End Get
            Set(value As Integer)
                _idPlanVenta = value
            End Set
        End Property

        Public Property NombrePlanVenta As String
            Get
                Return _nombrePlanVenta
            End Get
            Set(value As String)
                _nombrePlanVenta = value
            End Set
        End Property

        Public Property CfmPlan As Double
            Get
                Return _cfmPlan
            End Get
            Set(value As Double)
                _cfmPlan = value
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

        Public Property TelefonoFijo As String
            Get
                Return _telefonoFijo
            End Get
            Set(value As String)
                _telefonoFijo = value
            End Set
        End Property

        Public Property TelefonoMovil As String
            Get
                Return _telefonoMovil
            End Get
            Set(value As String)
                _telefonoMovil = value
            End Set
        End Property

        Public Property IdMedioPago As Short
            Get
                Return _idMedioPago
            End Get
            Set(value As Short)
                _idMedioPago = value
            End Set
        End Property

        Public Property NombreMedioPago As String
            Get
                Return _nombreMedioPago
            End Get
            Set(value As String)
                _nombreMedioPago = value
            End Set
        End Property

        Public Property IdTipoMigracion As Short
            Get
                Return _idTipoMigracion
            End Get
            Set(value As Short)
                _idTipoMigracion = value
            End Set
        End Property

        Public Property NombreTipoMigracion As String
            Get
                Return _nombreTipoMigracion
            End Get
            Set(value As String)
                _nombreTipoMigracion = value
            End Set
        End Property

        Public Property Material As String
            Get
                Return _material
            End Get
            Set(value As String)
                _material = value
            End Set
        End Property

        Public Property ValorRecaudado As Double
            Get
                Return _valorRecaudado
            End Get
            Set(value As Double)
                _valorRecaudado = value
            End Set
        End Property

        Public Property NumeroContrato As Long
            Get
                Return _numeroContrato
            End Get
            Set(value As Long)
                _numeroContrato = value
            End Set
        End Property

        Public Overloads Property MinsColeccion() As DetalleMsisdnEnServicioMensajeriaTipoVentaColeccion
            Get
                Return _minsColeccion
            End Get
            Set(ByVal value As DetalleMsisdnEnServicioMensajeriaTipoVentaColeccion)
                _minsColeccion = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Protected Overloads Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                    If _numeroRadicado > 0 Then .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado

                    .ejecutarReader("ObtenerInformacionGeneralServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idServicioMensajeria").ToString(), _idServicioMensajeria)
                            Integer.TryParse(.Reader("idAgendamiento").ToString(), _idAgendamiento)
                            Integer.TryParse(.Reader("idBodega").ToString(), _idBodega)
                            _bodega = .Reader("bodega").ToString()
                            Integer.TryParse(.Reader("idUsuario").ToString(), _idUsuario)
                            _usuarioRegistra = .Reader("usuarioRegistra").ToString()
                            _fechaRegistro = CDate(.Reader("fechaRegistro").ToString())
                            If Not IsDBNull(.Reader("fechaAgenda")) Then _fechaAgenda = CDate(.Reader("fechaAgenda"))
                            If Not IsDBNull(.Reader("fechaRegistroAgenda")) Then _fechaRegistroAgenda = CDate(.Reader("fechaRegistroAgenda"))
                            If Not IsDBNull(.Reader("fechaAsignacion")) Then _fechaAsignacion = CDate(.Reader("fechaAsignacion"))
                            _usuarioEjecutor = .Reader("usuarioEjecutor").ToString()
                            Integer.TryParse(.Reader("idJornada").ToString(), _idJornada)
                            _jornada = .Reader("jornada").ToString()
                            Integer.TryParse(.Reader("idEstado").ToString(), _idEstado)
                            _estado = .Reader("estado").ToString()
                            Integer.TryParse(.Reader("idReserva").ToString(), _idReserva)
                            Integer.TryParse(.Reader("idUsuarioConfirmacion").ToString(), _idUsuarioConfirmacion)
                            _usuarioConfirmacion = .Reader("usuarioConfirma").ToString()
                            If Not IsDBNull(.Reader("fechaConfirmacion")) Then _fechaConfirmacion = CDate(.Reader("fechaConfirmacion"))
                            Integer.TryParse(.Reader("idResponsableEntrega").ToString(), _idResponsableEntrega)
                            _responsableEntrega = .Reader("responsableEntrega").ToString()
                            If Not IsDBNull(.Reader("fechaCierre")) Then _fechaCierre = CDate(.Reader("fechaCierre"))
                            Integer.TryParse(.Reader("idUsuarioCierre").ToString(), _idUsuarioCierre)
                            _usuarioCierre = .Reader("usuarioCierre").ToString()
                            _codigoActivacion = .Reader("codigoActivacion").ToString()
                            _observacion = .Reader("observacion").ToString()
                            _nombreCliente = .Reader("nombreCliente").ToString()
                            _personaContacto = .Reader("personaContacto").ToString()
                            _identificacionCliente = .Reader("identificacionCliente").ToString()
                            Integer.TryParse(.Reader("idCiudad").ToString(), _idCiudad)
                            _nombreCiudad = .Reader("ciudadCliente").ToString()
                            _barrio = .Reader("barrio").ToString()
                            _direccion = .Reader("direccion").ToString()
                            _telefonoContacto = .Reader("telefonoContacto").ToString()
                            _extensionContacto = .Reader("extensionContacto").ToString()
                            _tipoTelefono = .Reader("tipoTelefono").ToString()
                            Integer.TryParse(.Reader("numeroRadicado").ToString(), _numeroRadicado)
                            If Not IsDBNull(.Reader("clienteVip")) Then _clienteVip = CBool(.Reader("clienteVip"))
                            _planActual = .Reader("planActual").ToString()
                            Integer.TryParse(.Reader("idTipoServicio").ToString(), _idTipoServicio)
                            _tipoServicio = .Reader("tipoServicio").ToString()
                            Integer.TryParse(.Reader("idZona").ToString(), _idZona)
                            _nombreZona = .Reader("nombreZona").ToString()
                            _facturaCambioServicio = .Reader("facturaCambioServicio").ToString()
                            _remisionCambioServicio = .Reader("remisionCambioServicio").ToString()
                            _novedadEnCambioServicio = CBool(.Reader("tieneNovedadCambioServicio"))
                            _observacionCambioServicio = .Reader("observacionCambioServicio").ToString()
                            If Not IsDBNull(.Reader("fechaDespacho")) Then _fechaDespacho = CDate(.Reader("fechaDespacho"))
                            Integer.TryParse(.Reader("idUsuarioDespacho").ToString(), _idUsuarioDespacho)
                            _usuarioDespacho = .Reader("usuarioDespacho").ToString()
                            If Not IsDBNull(.Reader("fechaCambioServicio")) Then _fechaCambioServicio = CDate(.Reader("fechaCambioServicio"))
                            Integer.TryParse(.Reader("idUsuarioCambioServicio").ToString(), _idUsuarioCambioServicio)
                            _usuarioCambioServicio = .Reader("usuarioCambioServicio").ToString()
                            Integer.TryParse(.Reader("idPrioridad").ToString(), _idPrioridad)
                            _prioridad = .Reader("prioridad").ToString()
                            If Not IsDBNull(.Reader("fechaVencimientoReserva")) Then _fechaVencimientoReserva = CDate(.Reader("fechaVencimientoReserva"))
                            _urgente = CBool(.Reader("urgente"))
                            _disponibleAutomarcado = CBool(.Reader("disponibleAutomarcado"))
                            _medioEnvioCH = .Reader("medioEnvioCH").ToString()
                            _correoEnvioCH = .Reader("correoEnvioCH").ToString()
                            If Not IsDBNull(.Reader("idCampania")) Then Integer.TryParse(.Reader("idCampania"), _idCampania)
                            If Not IsDBNull(.Reader("nombreCampania")) Then _nombreCampania = .Reader("nombreCampania").ToString
                            If Not String.IsNullOrEmpty(.Reader("idPlanVenta").ToString) Then Integer.TryParse(.Reader("idPlanVenta"), _idPlanVenta)
                            If Not String.IsNullOrEmpty(.Reader("nombrePlanVenta").ToString) Then _nombrePlanVenta = .Reader("nombrePlanVenta").ToString()
                            If Not String.IsNullOrEmpty(.Reader("cargoFijoMensual").ToString) Then Double.TryParse(.Reader("cargoFijoMensual"), _cfmPlan)
                            _direccionEdicion = .Reader("direccionEdicion").ToString()
                            _observacionDireccion = .Reader("observacionDireccion").ToString()
                            If Not IsDBNull(.Reader("telefonoFijo")) Then _telefonoFijo = .Reader("telefonoFijo")
                            If Not String.IsNullOrEmpty(.Reader("idMedioPago").ToString) Then Integer.TryParse(.Reader("idMedioPago"), _idMedioPago)
                            If Not String.IsNullOrEmpty(.Reader("nombreMedioPago").ToString) Then _nombreMedioPago = .Reader("nombreMedioPago")
                            If Not String.IsNullOrEmpty(.Reader("idTipoMigracion").ToString) Then If Not IsDBNull(.Reader("idTipoMigracion")) Then Integer.TryParse(.Reader("idTipoMigracion"), _idTipoMigracion)
                            If Not String.IsNullOrEmpty(.Reader("nombreTipoMigracion").ToString) Then If Not IsDBNull(.Reader("nombreTipoMigracion")) Then _nombreTipoMigracion = .Reader("nombreTipoMigracion")

                            _registrado = True
                        End If
                        .Reader.Close()
                    End If

                    _referenciasColeccion = New DetalleMaterialServicioMensajeriaColeccion(_idServicioMensajeria)
                    _minsColeccion = New DetalleMsisdnEnServicioMensajeriaTipoVentaColeccion(_idServicioMensajeria)

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub

#End Region

#Region "Métodos Públicos"

        Public Overrides Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim noResultadoServicio As Integer = -1
            Dim idServicioTipo As Integer

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        If Not _fechaRegistro.Equals(Date.MinValue) Then .SqlParametros.Add("@fecha", SqlDbType.DateTime).Value = _fechaRegistro
                        If Not _fechaAgenda.Equals(Date.MinValue) Then .SqlParametros.Add("@fechaAgenda", SqlDbType.DateTime).Value = _fechaAgenda
                        If Not _fechaRegistroAgenda.Equals(Date.MinValue) Then .SqlParametros.Add("@fechaRegistroAgenda", SqlDbType.DateTime).Value = _fechaRegistroAgenda
                        If _idJornada > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.SmallInt).Value = _idJornada
                        .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If _idReserva > 0 Then .SqlParametros.Add("@idReserva", SqlDbType.Int).Value = _idReserva
                        If _observacion <> String.Empty Then .SqlParametros.Add("@observacion", SqlDbType.VarChar).Value = _observacion
                        If _nombreCliente <> String.Empty Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombreCliente
                        If _identificacionCliente <> String.Empty Then .SqlParametros.Add("@identicacion", SqlDbType.VarChar).Value = _identificacionCliente
                        If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        If _barrio <> String.Empty Then .SqlParametros.Add("@barrio", SqlDbType.VarChar).Value = _barrio
                        If _direccion <> String.Empty Then .SqlParametros.Add("@direccion", SqlDbType.VarChar).Value = _direccion
                        If _telefonoContacto <> String.Empty Then .SqlParametros.Add("@telefono", SqlDbType.VarChar).Value = _telefonoContacto
                        If _tipoTelefono <> String.Empty Then .SqlParametros.Add("@tipoTelefono", SqlDbType.VarChar).Value = _tipoTelefono
                        If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                        If _idPlanVenta > 0 Then .SqlParametros.Add("@idPlanVenta", SqlDbType.Int).Value = _idPlanVenta
                        If Not String.IsNullOrEmpty(_direccionEdicion) Then .SqlParametros.Add("@direccionEdicion", SqlDbType.VarChar).Value = _direccionEdicion
                        If Not String.IsNullOrEmpty(_observacionDireccion) Then .SqlParametros.Add("@observacionDireccion", SqlDbType.VarChar).Value = _observacionDireccion
                        If Not String.IsNullOrEmpty(_telefonoFijo) Then .SqlParametros.Add("@telefonoFijo", SqlDbType.VarChar).Value = _telefonoFijo
                        If _idMedioPago > 0 Then .SqlParametros.Add("@idMedioPago", SqlDbType.Int).Value = _idMedioPago
                        If _idTipoMigracion > 0 Then .SqlParametros.Add("@idTipoMigracion", SqlDbType.Int).Value = _idTipoMigracion

                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Direction = ParameterDirection.Output

                        .IniciarTransaccion()

                        'Registro en [ServicioMensajeria]
                        .EjecutarScalar("RegistraServicioMensajeria", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultadoServicio)
                        Integer.TryParse(.SqlParametros("@idServicioMensajeria").Value.ToString(), _idServicioMensajeria)

                        If noResultadoServicio = 0 Then
                            If _idTipoServicio <> 0 Then
                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = IdServicioMensajeria
                                .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario

                                .SqlParametros.Add("@idServicioTipo", SqlDbType.Int).Direction = ParameterDirection.Output

                                'Registro en [ServicioMensajeriaTipoServicio]
                                .EjecutarScalar("RegistraTipoServicioMensajeria", CommandType.StoredProcedure)
                                Integer.TryParse(.SqlParametros("@idServicioTipo").Value.ToString(), idServicioTipo)

                                If idServicioTipo <> 0 Then

                                    'Registro en [MaterialServicioTipoServicio]
                                    If _referenciasDataTable IsNot Nothing AndAlso _referenciasDataTable.Rows.Count > 0 Then
                                        If _referenciasDataTable.Columns.Contains("idServicioTipo") Then _referenciasDataTable.Columns.Remove("idServicioTipo")
                                        Dim columnaidServicioTipo As New DataColumn("idServicioTipo", GetType(Integer))
                                        columnaidServicioTipo.DefaultValue = idServicioTipo
                                        _referenciasDataTable.Columns.Add(columnaidServicioTipo)

                                        If _referenciasDataTable.Columns.Contains("idUsuario") Then _referenciasDataTable.Columns.Remove("idUsuario")
                                        Dim columnaidUsuario As New DataColumn("idUsuario", GetType(Integer))
                                        columnaidUsuario.DefaultValue = _idUsuario
                                        _referenciasDataTable.Columns.Add(columnaidUsuario)

                                        .InicilizarBulkCopy()
                                        With .BulkCopy
                                            .DestinationTableName = "MaterialServicioTipoServicio"
                                            .ColumnMappings.Add("idServicioTipo", "idServicioTipo")
                                            .ColumnMappings.Add("material", "material")
                                            .ColumnMappings.Add("cantidad", "cantidad")
                                            .ColumnMappings.Add("idUsuario", "idUsuario")
                                            .WriteToServer(_referenciasDataTable)
                                        End With
                                    End If

                                    'Registro en [MsisdnEnServicioMensajeria]
                                    If _minsDataTable IsNot Nothing AndAlso _minsDataTable.Rows.Count > 0 Then
                                        If _minsDataTable.Columns.Contains("idServicioTipo") Then _minsDataTable.Columns.Remove("idServicioTipo")
                                        Dim columnaidServicioTipoMin As New DataColumn("idServicioTipo", GetType(Integer))
                                        columnaidServicioTipoMin.DefaultValue = idServicioTipo
                                        _minsDataTable.Columns.Add(columnaidServicioTipoMin)

                                        .InicilizarBulkCopy()
                                        With .BulkCopy
                                            .DestinationTableName = "MsisdnEnServicioMensajeria"
                                            .ColumnMappings.Add("idServicioTipo", "idServicioTipo")
                                            .ColumnMappings.Add("msisdn", "msisdn")
                                            .ColumnMappings.Add("activaEquipoAnterior", "activaEquipoAnterior")
                                            .ColumnMappings.Add("comSeguro", "comSeguro")
                                            .ColumnMappings.Add("precioConIVA", "precioConIVA")
                                            .ColumnMappings.Add("precioSinIVA", "precioSinIVA")
                                            .ColumnMappings.Add("idClausula", "idClausula")
                                            .ColumnMappings.Add("numeroReserva", "numeroReserva")
                                            .ColumnMappings.Add("idRegion", "idRegion")
                                            .WriteToServer(_minsDataTable)
                                        End With
                                    End If

                                    'Registra el precio total del servicio
                                    Dim respuestaPrecio As Integer = -1
                                    .SqlParametros.Clear()
                                    .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = IdServicioMensajeria
                                    .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                                    .EjecutarScalar("RegistraPrecioServicioVenta", CommandType.StoredProcedure)
                                    Integer.TryParse(.SqlParametros("@respuesta").Value.ToString(), respuestaPrecio)
                                    If respuestaPrecio <> 0 Then
                                        resultado.EstablecerMensajeYValor(9, "No se logro registrar el precio del servicio.")
                                        .AbortarTransaccion()
                                    End If

                                    'Se realiza la reserva del Inventario de Ventas
                                    If Me._detalleBloqueoInventario IsNot Nothing Then
                                        Dim resultadoBloqueo As ResultadoProceso = Me._detalleBloqueoInventario.Registrar()
                                        If resultadoBloqueo.Valor <> 0 Then
                                            resultado.EstablecerMensajeYValor(6, resultadoBloqueo.Mensaje)
                                            .AbortarTransaccion()
                                        End If
                                    End If

                                    If .EstadoTransaccional Then
                                        .ConfirmarTransaccion()
                                        resultado.EstablecerMensajeYValor(0, "Transacción exitosa.")

                                        'Se asocia la reserva al servicio
                                        If Me._detalleBloqueoInventario IsNot Nothing AndAlso Me._detalleBloqueoInventario.IdBloqueo <> 0 Then
                                            Me.IdReserva = Me._detalleBloqueoInventario.IdBloqueo
                                            Me.Actualizar(_idUsuario)
                                        End If
                                    End If
                                Else
                                    resultado.EstablecerMensajeYValor(7, "Se generó un error al tratar de generar tipo de Servicio de Mensajeria.")
                                    .AbortarTransaccion()
                                End If
                            Else
                                resultado.EstablecerMensajeYValor(8, "No se selecciono el tipo de servicio.")
                                .AbortarTransaccion()
                            End If
                        Else
                            If noResultadoServicio = 1 Then resultado.EstablecerMensajeYValor(1, "El Número de Radicado ingresado ya se encuentra registrado en el sistema. Por favor verifique e intente nuevamente.")
                            If noResultadoServicio = -1 Then resultado.EstablecerMensajeYValor(9, "Se generó un error al tratar de generar Servicio de Mensajeria.")
                            .AbortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Overrides Function Actualizar(idUsuarioLog As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim idResultado As Integer = -1
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                            .Add("@idUsuarioLog", SqlDbType.Int).Value = idUsuarioLog

                            If _idUsuario > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            If Not _fechaAgenda.Equals(Date.MinValue) Then .Add("@fechaAgenda", SqlDbType.DateTime).Value = _fechaAgenda
                            If Not _fechaRegistroAgenda.Equals(Date.MinValue) Then .Add("@fechaRegistroAgenda", SqlDbType.DateTime).Value = _fechaRegistroAgenda
                            If Not _fechaAsignacion.Equals(Date.MinValue) Then .Add("@fechaAsignacion", SqlDbType.DateTime).Value = _fechaAsignacion
                            If _idJornada > 0 Then .Add("@idJornada", SqlDbType.SmallInt).Value = _idJornada
                            If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            If _idReserva > 0 Then .Add("@idReserva", SqlDbType.Int).Value = _idReserva
                            If _idUsuarioConfirmacion > 0 Then .Add("@idUsuarioConfirmacion", SqlDbType.Int).Value = _idUsuarioConfirmacion
                            If Not _fechaConfirmacion.Equals(Date.MinValue) Then .Add("@fechaConfirmacion", SqlDbType.DateTime).Value = _fechaConfirmacion
                            If _idResponsableEntrega > 0 Then .Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega
                            If Not _fechaCierre.Equals(Date.MinValue) Then .Add("@fechaCierre", SqlDbType.DateTime).Value = _fechaCierre
                            If _idUsuarioCierre > 0 Then .Add("@idUsuarioCierre", SqlDbType.Int).Value = IdUsuarioCierre
                            If Observacion <> String.Empty Then .Add("@observacion", SqlDbType.VarChar).Value = _observacion
                            If _nombreCliente <> String.Empty Then .Add("@nombre", SqlDbType.VarChar).Value = _nombreCliente
                            If _personaContacto <> String.Empty Then .Add("@nombreAutorizado", SqlDbType.VarChar).Value = _personaContacto
                            If _identificacionCliente <> String.Empty Then .Add("@identicacion", SqlDbType.VarChar).Value = _identificacionCliente
                            If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                            If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                            If _barrio <> String.Empty Then .Add("@barrio", SqlDbType.VarChar).Value = _barrio
                            If _direccion <> String.Empty Then .Add("@direccion", SqlDbType.VarChar).Value = _direccion
                            If _telefonoContacto <> String.Empty Then .Add("@telefono", SqlDbType.VarChar).Value = _telefonoContacto
                            If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                            If Not _fechaVencimientoReserva.Equals(Date.MinValue) Then .Add("@fechaVencimientoReserva", SqlDbType.DateTime).Value = _fechaVencimientoReserva
                            .Add("@urgente", SqlDbType.Bit).Value = _urgente
                            .Add("@disponibleAutomarcado", SqlDbType.Bit).Value = _disponibleAutomarcado
                            If _idEstado = Enumerados.EstadoServicio.RecibidoST And Not _fechaAgenda.Equals(Date.MinValue) Then .Add("@fechaAgendaEntrega", SqlDbType.DateTime).Value = _fechaAgenda
                            If _medioEnvioCH <> String.Empty Then .Add("@medioEnvioCH", SqlDbType.VarChar).Value = _medioEnvioCH
                            If _correoEnvioCH <> String.Empty Then .Add("@correoEnvioCH", SqlDbType.VarChar).Value = _correoEnvioCH
                            If _idCampania > 0 Then .Add("@idCampania", SqlDbType.Int).Value = _idCampania
                            If _idPlanVenta > 0 Then .Add("@idPlanVenta", SqlDbType.Int).Value = _idPlanVenta
                            If Not String.IsNullOrEmpty(_direccionEdicion) Then .Add("@direccionEdicion", SqlDbType.VarChar).Value = _direccionEdicion
                            If Not String.IsNullOrEmpty(_observacionDireccion) Then .Add("@observacionDireccion", SqlDbType.VarChar).Value = _observacionDireccion
                            If Not String.IsNullOrEmpty(_telefonoFijo) Then .Add("@telefonoFijo", SqlDbType.VarChar).Value = _telefonoFijo
                            If _idMedioPago > 0 Then .Add("@idMedioPago", SqlDbType.Int).Value = _idMedioPago
                            If _idTipoMigracion > 0 Then .Add("@idTipoMigracion", SqlDbType.Int).Value = _idTipoMigracion

                            .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                        End With

                        .IniciarTransaccion()
                        .EjecutarScalar("ActualizaServicioMensajeria", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), idResultado)

                        If idResultado = 0 Then
                            .ConfirmarTransaccion()
                            resultado.EstablecerMensajeYValor(0, "Transacción exitosa.")
                        Else
                            resultado.EstablecerMensajeYValor(7, "Se generó un error al tratar de actualizar.")
                            .AbortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Overloads Function Confirmar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Not (_idServicioMensajeria = 0 OrElse String.IsNullOrEmpty(_direccion) OrElse String.IsNullOrEmpty(_barrio) _
                OrElse _fechaAgenda = Date.MinValue OrElse _idJornada = 0 OrElse _idUsuarioConfirmacion = 0) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@direccion", SqlDbType.VarChar).Value = _direccion
                        .SqlParametros.Add("@direccionEdicion", SqlDbType.VarChar).Value = _direccionEdicion
                        .SqlParametros.Add("@barrio", SqlDbType.VarChar, 70).Value = _barrio
                        .SqlParametros.Add("@fechaAgenda", SqlDbType.SmallDateTime).Value = _fechaAgenda
                        .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                        .SqlParametros.Add("@idUsuarioConfirma", SqlDbType.Int).Value = _idUsuarioConfirmacion
                        .SqlParametros.Add("@telefonoMovil", SqlDbType.VarChar, 30).Value = _telefonoContacto
                        .SqlParametros.Add("@telefonoFijo", SqlDbType.VarChar, 30).Value = _telefonoFijo
                        .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                        .SqlParametros.Add("@observacionDireccion", SqlDbType.VarChar, 2000).Value = _observacionDireccion
                        If _idMedioPago > 0 Then .SqlParametros.Add("@idMedioPago", SqlDbType.Int).Value = _idMedioPago

                        .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material

                        .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .IniciarTransaccion()
                        .EjecutarNonQuery("ConfirmarServicioMensajeriaTipoVenta", CommandType.StoredProcedure)
                        If Not IsDBNull(.SqlParametros("@result").Value) Then
                            resultado.Valor = CShort(.SqlParametros("@result").Value)
                            If resultado.Valor = 0 Then
                                resultado.Mensaje = "El servicio fue confirmado de manera exitosa."
                                .ConfirmarTransaccion()
                            Else
                                Select Case resultado.Valor
                                    Case 1
                                        resultado.Mensaje = "Ocurrió un error inesperado al confirmar el servicio. Por favor intente nuevamente."
                                    Case 2
                                        resultado.Mensaje = "No existe disponibilidad de cupos de entrega para la fecha y jornada seleccionada."
                                    Case 3
                                        resultado.Mensaje = "No existe disponibilidad de inventario para el Producto seleccionado."
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

        Public Overloads Function ConfirmarEntrega() As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = _idServicioMensajeria
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@idZona", SqlDbType.Int).Value = _idZona
                    .SqlParametros.Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega
                    .SqlParametros.Add("@medioPago", SqlDbType.SmallInt).Value = _idMedioPago
                    .SqlParametros.Add("@valorRecaudo", SqlDbType.Money).Value = _valorRecaudado
                    .SqlParametros.Add("@tipoServicio", SqlDbType.SmallInt).Value = Enumerados.TipoServicio.Venta
                    If _numeroContrato > 0 Then .SqlParametros.Add("@numContrato", SqlDbType.BigInt).Value = _numeroContrato

                    .IniciarTransaccion()

                    .ejecutarReader("ConfirmarEntregaServicioMensajeria", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing And .Reader.HasRows Then
                        If .Reader.Read() Then
                            If CInt(.Reader.Item(0).ToString()) = 0 Then
                                resultado.EstablecerMensajeYValor(CInt(.Reader.Item(0).ToString()), .Reader.Item(1).ToString())
                                .Reader.Close()
                                .ConfirmarTransaccion()
                            Else
                                resultado.EstablecerMensajeYValor(CInt(.Reader.Item(0).ToString()), .Reader.Item(1).ToString())
                                .AbortarTransaccion()
                            End If
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        Throw New Exception("Ocurrió un error interno al finalizar cambios. Por favor intente nuevamente")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            End Try
            dbManager.Dispose()

            Return resultado
        End Function

        Public Function VerificarMsisdn() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idCampania", SqlDbType.Int).Value = _idCampania
                        .Add("@telefonoMovil", SqlDbType.VarChar, 50).Value = _telefonoMovil
                        If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                        .Add("@mensaje", SqlDbType.VarChar, 8000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .EjecutarNonQuery("ValidarMsisdnEnVentaTelefonica", CommandType.StoredProcedure)
                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        resultado.EstablecerMensajeYValor(300, "No se logró obtener respuesta del servidor, por favor intentelo nuevamente.")
                    End If
                End With
            Catch ex As Exception
                dbManager.Dispose()
                resultado.EstablecerMensajeYValor(400, "Se presentó un error al validar el MSISDN: " & ex.Message)
            End Try
            Return resultado
        End Function

#End Region

#Region "Médotos Compartidos"

        Public Shared Function ObtenerDocumentosAsociados(ByVal idServicio As Integer) As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
                        dtDatos = .EjecutarDataTable("ObtenerDocumentosAsociadosServicioVenta", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerMaterialesSIM(ByVal materialEquipo As String) As List(Of String)
            Dim listMateriales As New List(Of String)
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@materialEquipo", SqlDbType.VarChar, 20).Value = materialEquipo
                        .ejecutarReader("ObtieneMaterialesSIM", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            While .Reader.Read()
                                listMateriales.Add(.Reader("material").ToString)
                            End While
                            .Reader.Close()
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return listMateriales
        End Function

        Public Overloads Shared Function ObtenerMaterialesSIM(ByVal idClaseSIM As Short) As List(Of String)
            Dim listMateriales As New List(Of String)
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idClaseSIM", SqlDbType.VarChar, 20).Value = idClaseSIM
                        .ejecutarReader("ObtieneMaterialesSIM", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            While .Reader.Read()
                                listMateriales.Add(.Reader("material").ToString)
                            End While
                            .Reader.Close()
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return listMateriales
        End Function

        Public Shared Function RadicarServicios(ByVal numVolante As String, ByVal valorTotal As Double, fechaRadicacion As Date, ByVal dtDatos As DataTable) As ResultadoProceso
            Dim respuesta As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = CInt(HttpContext.Current.Session("usxp001"))
                        .SqlParametros.Add("@numeroVolante", SqlDbType.VarChar).Value = numVolante
                        .SqlParametros.Add("@valorTotal", SqlDbType.Money).Value = valorTotal
                        .SqlParametros.Add("@fechaRadicacion", SqlDbType.DateTime).Value = fechaRadicacion
                        .SqlParametros.Add("@retorno", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@idRadicado", SqlDbType.Int).Direction = ParameterDirection.Output
                        .EjecutarNonQuery("RegistrarRadicadoServiciosVenta", CommandType.StoredProcedure)

                        .IniciarTransaccion()

                        If .SqlParametros("@retorno").Value = 0 Then
                            Dim idRadicado As Integer = .SqlParametros("@idRadicado").Value
                            If Not dtDatos.Columns.Contains("idRadicado") Then
                                dtDatos.Columns.Add(New DataColumn("idRadicado", GetType(Integer), idRadicado))
                                dtDatos.AcceptChanges()
                            End If
                            .InicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "DetalleRadicacionVenta"
                                .ColumnMappings.Add("idRadicado", "idRadicacion")
                                .ColumnMappings.Add("IdServicio", "idServicio")
                                .ColumnMappings.Add("NumeroRadicado", "numeroContrato")
                                .WriteToServer(dtDatos)
                            End With

                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idRadicado", SqlDbType.Int).Value = idRadicado
                            .SqlParametros.Add("@retorno", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            .EjecutarNonQuery("ActualizaEstadoServiciosRadicados", CommandType.StoredProcedure)
                            If .SqlParametros("@retorno").Value = 0 Then
                                .ConfirmarTransaccion()
                                respuesta.EstablecerMensajeYValor(0, "Se realizó exitosamete la radicación con el identificador: " & idRadicado)
                            Else
                                .AbortarTransaccion()
                                respuesta.EstablecerMensajeYValor(.SqlParametros("@retorno").Value, "No fue posible radicar los servicios: " & .SqlParametros("@retorno").Value)
                            End If
                        Else
                            .AbortarTransaccion()
                            respuesta.EstablecerMensajeYValor(.SqlParametros("@retorno").Value, "No fue posible registrar el Radicado del servicio [" & .SqlParametros("@retorno").Value & "]")
                        End If
                    End With
                Catch ex As Exception
                    dbManager.AbortarTransaccion()
                    Throw ex
                End Try
            End Using
            Return respuesta
        End Function

        Public Shared Function AgregarReferencia(ByVal material As String, ByVal referencia As String) As DataTable
            Dim dtReferencia As DataTable
            Try
                dtReferencia = CrearEstructuraTablaReferencia()
                If Not String.IsNullOrEmpty(material) Then
                    Dim filaDataRow As DataRow = dtReferencia.NewRow()
                    filaDataRow("material") = material
                    filaDataRow("referencia") = referencia
                    filaDataRow("cantidad") = 1

                    dtReferencia.Rows.Add(filaDataRow)
                    dtReferencia.AcceptChanges()
                    If HttpContext.Current.Session IsNot Nothing Then HttpContext.Current.Session("dtReferencia") = dtReferencia
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return dtReferencia
        End Function

        Public Shared Function CrearEstructuraTablaReferencia() As DataTable
            Dim dtReferencia As New DataTable
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("dtReferencia") IsNot Nothing Then
                dtReferencia = HttpContext.Current.Session("dtReferencia")
            Else
                With dtReferencia
                    .Columns.Add("material", GetType(String))
                    .Columns.Add("referencia", GetType(String))
                    .Columns.Add("cantidad", GetType(Integer))
                    .AcceptChanges()
                End With
                Dim pkMaterial() As DataColumn = {dtReferencia.Columns("material")}
                dtReferencia.PrimaryKey = pkMaterial
            End If
            Return dtReferencia
        End Function

        Public Shared Function AgregarMINs(ByVal material As String, ByVal idPlan As Integer, _
                                           ByVal msisdn As Long, ByVal idClausula As Integer, _
                                           ByVal clausula As String, ByVal region As String, ByVal ReuiereEquipo As Boolean) As DataTable
            Dim dtMsisdn As DataTable
            Try
                dtMsisdn = CrearEstructuraTablaMsisdn()
                If (ReuiereEquipo) Then
                    Dim objMaterialVenta As New MaterialEnPlanVentaColeccion(material, idPlan)

                    If objMaterialVenta.Count > 0 Then
                        Dim filaDataRow As DataRow = dtMsisdn.NewRow()
                        filaDataRow("msisdn") = msisdn
                        filaDataRow("activaEquipoAnterior") = False
                        filaDataRow("comSeguro") = False
                        filaDataRow("precioConIVA") = objMaterialVenta(0).PrecioVentaEquipo + objMaterialVenta(0).IvaEquipo
                        filaDataRow("precioSinIVA") = objMaterialVenta(0).PrecioVentaEquipo
                        filaDataRow("idClausula") = idClausula
                        filaDataRow("clausula") = clausula
                        filaDataRow("numeroReserva") = Nothing
                        filaDataRow("idregion") = region

                        dtMsisdn.Rows.Add(filaDataRow)
                        dtMsisdn.AcceptChanges()
                        If HttpContext.Current.Session IsNot Nothing Then HttpContext.Current.Session("dtMsisdn") = dtMsisdn
                    Else
                        Throw New Exception("No se encontraron materiales en el plan seleccionado.")
                    End If
                Else
                    Dim filaDataRow As DataRow = dtMsisdn.NewRow()
                    filaDataRow("msisdn") = msisdn
                    filaDataRow("activaEquipoAnterior") = False
                    filaDataRow("comSeguro") = False
                    filaDataRow("precioConIVA") = 0
                    filaDataRow("precioSinIVA") = 0
                    filaDataRow("idClausula") = idClausula
                    filaDataRow("clausula") = clausula
                    filaDataRow("numeroReserva") = Nothing
                    filaDataRow("idregion") = region
                    dtMsisdn.Rows.Add(filaDataRow)
                    dtMsisdn.AcceptChanges()
                    If HttpContext.Current.Session IsNot Nothing Then HttpContext.Current.Session("dtMsisdn") = dtMsisdn
                End If


            Catch ex As Exception
                Throw ex
            End Try
            Return dtMsisdn
        End Function

        Public Shared Function CrearEstructuraTablaMsisdn() As DataTable
            Dim dtMsisdn As New DataTable
            If HttpContext.Current.Session IsNot Nothing AndAlso HttpContext.Current.Session("dtMsisdn") IsNot Nothing Then
                dtMsisdn = HttpContext.Current.Session("dtMsisdn")
            Else
                With dtMsisdn
                    Dim dcAux As New DataColumn("idMsisdn")
                    With dcAux
                        .AutoIncrement = True
                        .AutoIncrementStep = 1
                    End With
                    .Columns.Add(dcAux)
                    .Columns.Add("msisdn", GetType(Long))
                    .Columns.Add("activaEquipoAnterior", GetType(Boolean))
                    .Columns.Add("comSeguro", GetType(Boolean))
                    .Columns.Add("precioConIVA", GetType(Double))
                    .Columns.Add("precioSinIVA", GetType(Double))
                    .Columns.Add("idClausula", GetType(Integer))
                    .Columns.Add("clausula", GetType(String))
                    .Columns.Add("numeroReserva", GetType(String))
                    .Columns.Add("idRegion", GetType(Integer))
                    .AcceptChanges()
                End With
                Dim pkMin() As DataColumn = {dtMsisdn.Columns("msisdn")}
                dtMsisdn.PrimaryKey = pkMin
            End If
            Return dtMsisdn
        End Function

#End Region

    End Class

End Namespace
