Imports ILSBusinessLayer
Imports ILSBusinessLayer.Inventario
Imports LMDataAccessLayer
Imports System.Web

Namespace MensajeriaEspecializada


    Public Class ServicioMensajeriaVentaEducativa
        Inherits ServicioMensajeriaVenta
        Implements IServicioMensajeria

#Region "Atributos"

        Private _numeroIdentificacionNino As String 'Niño
        Private _idTipoIdentificacionNino As Integer
        Private _idTipoCliente As Integer
        Private _idEstablecimiento As Integer
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _idTipoServicio = Enumerados.TipoServicio.VentaEDUCLIC
        End Sub

        Public Sub New(idServicio As Integer)
            Me._idServicioMensajeria = idServicio
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Public Property NumeroIdentificacionNino As String
            Get
                Return _numeroIdentificacionNino
            End Get
            Set(value As String)
                _numeroIdentificacionNino = value
            End Set
        End Property

        Public Property IdTipoIdentificacionNino As Integer
            Get
                Return _idTipoIdentificacionNino
            End Get
            Set(value As Integer)
                _idTipoIdentificacionNino = value
            End Set
        End Property

        Public Property IdTipoCliente As Integer
            Get
                Return _idTipoCliente
            End Get
            Set(value As Integer)
                _idTipoCliente = value
            End Set
        End Property

        Public Property IdEstablecimiento As Integer
            Get
                Return _idEstablecimiento
            End Get
            Set(value As Integer)
                _idEstablecimiento = value
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
                            Integer.TryParse(.Reader("idCampania"), IdCampania)
                            NombreCampania = .Reader("nombreCampania").ToString
                            If Not String.IsNullOrEmpty(.Reader("idPlanVenta").ToString) Then Integer.TryParse(.Reader("idPlanVenta"), IdPlanVenta)
                            If Not String.IsNullOrEmpty(.Reader("nombrePlanVenta").ToString) Then NombrePlanVenta = .Reader("nombrePlanVenta").ToString()
                            If Not String.IsNullOrEmpty(.Reader("cargoFijoMensual").ToString) Then Double.TryParse(.Reader("cargoFijoMensual"), CfmPlan)
                            DireccionEdicion = .Reader("direccionEdicion").ToString()
                            ObservacionDireccion = .Reader("observacionDireccion").ToString()
                            If Not IsDBNull(.Reader("telefonoFijo")) Then TelefonoFijo = .Reader("telefonoFijo")
                            If Not String.IsNullOrEmpty(.Reader("idMedioPago").ToString) Then Integer.TryParse(.Reader("idMedioPago"), IdMedioPago)
                            If Not String.IsNullOrEmpty(.Reader("nombreMedioPago").ToString) Then NombreMedioPago = .Reader("nombreMedioPago")
                            If Not String.IsNullOrEmpty(.Reader("idTipoMigracion").ToString) Then If Not IsDBNull(.Reader("idTipoMigracion")) Then Integer.TryParse(.Reader("idTipoMigracion"), IdTipoMigracion)
                            If Not String.IsNullOrEmpty(.Reader("nombreTipoMigracion").ToString) Then If Not IsDBNull(.Reader("nombreTipoMigracion")) Then NombreTipoMigracion = .Reader("nombreTipoMigracion")

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

                        If _valorRecaudado > 0 Then .SqlParametros.Add("@valorARecaudar", SqlDbType.Money).Value = _valorRecaudado
                        If Not String.IsNullOrEmpty(_material) Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
                        If _idTipoCliente > 0 Then .SqlParametros.Add("@idTipoCliente", SqlDbType.Int).Value = _idTipoCliente
                        If _idTipoIdentificacionNino > 0 Then .SqlParametros.Add("@idTipoIdentificacionNino", SqlDbType.Int).Value = _idTipoIdentificacionNino
                        If Not String.IsNullOrEmpty(_numeroIdentificacionNino) Then .SqlParametros.Add("@numeroIdentificacionNino", SqlDbType.VarChar).Value = _numeroIdentificacionNino
                        If _idEstablecimiento > 0 Then .SqlParametros.Add("@idEstablecimiento", SqlDbType.Int).Value = _idEstablecimiento

                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Direction = ParameterDirection.Output

                        .IniciarTransaccion()

                        'Registro en [ServicioMensajeria]
                        .EjecutarScalar("RegistraServicioMensajeriaVentaEducativa", CommandType.StoredProcedure)
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
                                    If _valorRecaudado > 0 Then .SqlParametros.Add("@valorARecaudar", SqlDbType.Money).Value = _valorRecaudado
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

#End Region

#Region "Médotos Compartidos"

        Public Overloads Shared Function ObtenerDocumentosAsociados(ByVal idServicio As Integer) As DataTable
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

        Public Overloads Shared Function RadicarServicios(ByVal numVolante As String, ByVal valorTotal As Double, fechaRadicacion As Date, ByVal dtDatos As DataTable) As ResultadoProceso
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

        Public Overloads Shared Function AgregarReferencia(ByVal material As String, ByVal referencia As String) As DataTable
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

        Public Overloads Shared Function CrearEstructuraTablaReferencia() As DataTable
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

        Public Overloads Shared Function AgregarMINs(ByVal material As String, ByVal idPlan As Integer, _
                                           ByVal msisdn As Long, ByVal idClausula As Integer, _
                                           ByVal clausula As String, ByVal region As String) As DataTable
            Dim dtMsisdn As DataTable
            Try
                dtMsisdn = CrearEstructuraTablaMsisdn()
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
            Catch ex As Exception
                Throw ex
            End Try
            Return dtMsisdn
        End Function

        Public Overloads Shared Function CrearEstructuraTablaMsisdn() As DataTable
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
                Dim pkMin() As DataColumn = {dtMsisdn.Columns("idMsisdn")}
                dtMsisdn.PrimaryKey = pkMin
            End If
            Return dtMsisdn
        End Function

#End Region

    End Class

End Namespace