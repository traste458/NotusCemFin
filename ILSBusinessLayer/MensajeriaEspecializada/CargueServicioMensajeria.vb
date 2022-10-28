Imports ILSBusinessLayer
Imports LMDataAccessLayer


Namespace MensajeriaEspecializada

    Public Class CargueServicioMensajeria

#Region "Atributos"

        Private _idServicioMensajeria As Integer
        Private _idAgendamiento As Integer
        Private _idBodega As Integer
        Private _bodega As String
        Private _idUsuario As Integer
        Private _usuarioRegistra As String
        Private _fechaRegistro As Date
        Private _fechaAgenda As Date
        Private _fechaRegistroAgenda As Date
        Private _fechaAsignacion As Date
        Private _usuarioEjecutor As String
        Private _idJornada As Short
        Private _jornada As String
        Private _idEstado As Integer
        Private _estado As String
        Private _idReserva As Integer
        Private _idUsuarioConfirmacion As Integer
        Private _usuarioConfirmacion As String
        Private _fechaConfirmacion As Date
        Private _idResponsableEntrega As Integer
        Private _responsableEntrega As String
        Private _fechaCierre As Date
        Private _idUsuarioCierre As Integer
        Private _usuarioCierre As String
        Private _codigoActivacion As String
        Private _observacion As String
        Private _nombreCliente As String
        Private _personaContacto As String
        Private _identicacionCliente As String
        Private _idCiudad As Integer
        Private _idCiudadBodega As Integer
        Private _nombreCiudad As String
        Private _barrio As String
        Private _direccion As String
        Private _telefonoContacto As String
        Private _extensionContacto As String
        Private _tipoTelefono As String
        Private _numeroRadicado As Integer
        Private _clienteVip As Boolean
        Private _planActual As String
        Private _idTipoServicio As Integer
        Private _tipoServicio As String
        Private _registrado As Boolean
        Private _idZona As Integer
        Private _nombreZona As String
        Private _facturaCambioServicio As String
        Private _remisionCambioServicio As String
        Private _novedadEnCambioServicio As Boolean
        Private _observacionCambioServicio As String
        Private _fechaDespacho As Date
        Private _idUsuarioDespacho As Integer
        Private _usuarioDespacho As String
        Private _idPrioridad As Integer
        Private _prioridad As String
        Private _fechaVencimientoReserva As Date
        Private _disponibilidadAgenda As Integer
        Private _fechaCambioServicio As Date
        Private _idUsuarioCambioServicio As Integer
        Private _usuarioCambioServicio As String
        Private _adendo As Boolean

        Private _urgente As Boolean

        'TODO: Reemplazar los datatables por colecciones de clases.
        Private _referenciasDataTable As DataTable
        Private _minsDataTable As DataTable
        Private _dtServicio As DataTable

        Private _referenciasColeccion As DetalleMaterialServicioMensajeriaColeccion
        Private _minsColeccion As DetalleMsisdnEnServicioMensajeriaColeccionCargue


#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idServicio As Integer)
            MyBase.New()
            _idServicioMensajeria = idServicio
            CargarDatosServicio()
        End Sub

#End Region

#Region "Propiedades"
        Public Property IdServicioMensajeria() As Integer
            Get
                Return _idServicioMensajeria
            End Get
            Set(ByVal value As Integer)
                _idServicioMensajeria = value
            End Set
        End Property

        Public Property IdAgendamiento() As Integer
            Get
                Return _idAgendamiento
            End Get
            Set(ByVal value As Integer)
                _idAgendamiento = value
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

        Public Property Bodega() As String
            Get
                Return _bodega
            End Get
            Protected Friend Set(ByVal value As String)
                _bodega = value
            End Set
        End Property

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Property UsuarioRegistra() As String
            Get
                Return _usuarioRegistra
            End Get
            Protected Friend Set(ByVal value As String)
                _usuarioRegistra = value
            End Set
        End Property

        Public Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaRegistro = value
            End Set
        End Property

        Public Property FechaAgenda() As Date
            Get
                Return _fechaAgenda
            End Get
            Set(ByVal value As Date)
                _fechaAgenda = value
            End Set
        End Property

        Public Property FechaRegistroAgenda() As Date
            Get
                Return _fechaRegistroAgenda
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaRegistroAgenda = value
            End Set
        End Property

        Public Property FechaAsignacion() As Date
            Get
                Return _fechaAsignacion
            End Get
            Set(ByVal value As Date)
                _fechaAsignacion = value
            End Set
        End Property

        Public Property UsuarioEjecutor() As String
            Get
                Return _usuarioEjecutor
            End Get
            Set(ByVal value As String)
                _usuarioEjecutor = value
            End Set
        End Property

        Public Property IdJornada() As Short
            Get
                Return _idJornada
            End Get
            Set(ByVal value As Short)
                _idJornada = value
            End Set
        End Property

        Public Property Jornada() As String
            Get
                Return _jornada
            End Get
            Protected Friend Set(ByVal value As String)
                _jornada = value
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
            Protected Friend Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property IdReserva() As Integer
            Get
                Return _idReserva
            End Get
            Set(ByVal value As Integer)
                _idReserva = value
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

        Public Property UsuarioConfirmacion() As String
            Get
                Return _usuarioConfirmacion
            End Get
            Protected Friend Set(ByVal value As String)
                _usuarioConfirmacion = value
            End Set
        End Property

        Public Property FechaConfirmacion() As Date
            Get
                Return _fechaConfirmacion
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaConfirmacion = value
            End Set
        End Property

        Public Property IdResponsableEntrega() As Integer
            Get
                Return _idResponsableEntrega
            End Get
            Set(ByVal value As Integer)
                _idResponsableEntrega = value
            End Set
        End Property

        Public Property ResponsableEntrega() As String
            Get
                Return _responsableEntrega
            End Get
            Protected Friend Set(ByVal value As String)
                _responsableEntrega = value
            End Set
        End Property

        Public Property FechaCierre() As Date
            Get
                Return _fechaCierre
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaCierre = value
            End Set
        End Property

        Public Property IdUsuarioCierre() As Integer
            Get
                Return _idUsuarioCierre
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCierre = value
            End Set
        End Property

        Public Property UsuarioCierre() As String
            Get
                Return _usuarioCierre
            End Get
            Protected Friend Set(ByVal value As String)
                _usuarioCierre = value
            End Set
        End Property

        Public Property CodigoActivacion() As String
            Get
                Return _codigoActivacion
            End Get
            Set(ByVal value As String)
                _codigoActivacion = value
            End Set
        End Property

        Public Property Observacion() As String
            Get
                Return _observacion
            End Get
            Set(ByVal value As String)
                _observacion = value
            End Set
        End Property

        Public Property NombreCliente() As String
            Get
                Return _nombreCliente
            End Get
            Set(ByVal value As String)
                _nombreCliente = value
            End Set
        End Property

        Public Property PersonaContacto() As String
            Get
                Return _personaContacto
            End Get
            Set(ByVal value As String)
                _personaContacto = value
            End Set
        End Property

        Public Property IdenticacionCliente() As String
            Get
                Return _identicacionCliente
            End Get
            Set(ByVal value As String)
                _identicacionCliente = value
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

        Public Property IdCiudadBodega() As Integer
            Get
                Return _idCiudadBodega
            End Get
            Set(ByVal value As Integer)
                _idCiudadBodega = value
            End Set
        End Property

        Public Property Ciudad() As String
            Get
                Return _nombreCiudad
            End Get
            Protected Friend Set(ByVal value As String)
                _nombreCiudad = value
            End Set
        End Property

        Public Property Barrio() As String
            Get
                Return _barrio
            End Get
            Set(ByVal value As String)
                _barrio = value
            End Set
        End Property

        Public Property Direccion() As String
            Get
                Return _direccion
            End Get
            Set(ByVal value As String)
                _direccion = value
            End Set
        End Property

        Public Property TelefonoContacto() As String
            Get
                Return _telefonoContacto
            End Get
            Set(ByVal value As String)
                _telefonoContacto = value
            End Set
        End Property

        Public Property ExtensionContacto() As String
            Get
                Return _extensionContacto
            End Get
            Set(ByVal value As String)
                _extensionContacto = value
            End Set
        End Property

        Public Property TipoTelefono() As String
            Get
                Return _tipoTelefono
            End Get
            Set(ByVal value As String)
                _tipoTelefono = value
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

        Public Property ClienteVIP() As Boolean
            Get
                Return _clienteVip
            End Get
            Set(ByVal value As Boolean)
                _clienteVip = value
            End Set
        End Property

        Public Property PlanActual() As String
            Get
                Return _planActual
            End Get
            Set(ByVal value As String)
                _planActual = value
            End Set
        End Property

        Public Property TipoServicio() As String
            Get
                Return _tipoServicio
            End Get
            Protected Friend Set(ByVal value As String)
                _tipoServicio = value
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

        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

        Public Property IdZona() As Integer
            Get
                Return _idZona
            End Get
            Set(ByVal value As Integer)
                _idZona = value
            End Set
        End Property

        Public Property NombreZona() As String
            Get
                Return _nombreZona
            End Get
            Protected Friend Set(ByVal value As String)
                _nombreZona = value
            End Set
        End Property

        Public Property FacturaCambioServicio() As String
            Get
                Return _facturaCambioServicio
            End Get
            Set(ByVal value As String)
                _facturaCambioServicio = value
            End Set
        End Property

        Public Property RemisionCambioServicio() As String
            Get
                Return _remisionCambioServicio
            End Get
            Set(ByVal value As String)
                _remisionCambioServicio = value
            End Set
        End Property

        Public Property TieneNovedadCambioServicio() As Boolean
            Get
                Return _novedadEnCambioServicio
            End Get
            Set(ByVal value As Boolean)
                _novedadEnCambioServicio = value
            End Set
        End Property

        Public Property ObservacionCambioServicio() As String
            Get
                Return _observacionCambioServicio
            End Get
            Set(ByVal value As String)
                _observacionCambioServicio = value
            End Set
        End Property

        Public Property FechaDespacho() As Date
            Get
                Return _fechaDespacho
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaDespacho = value
            End Set
        End Property

        Public Property IdUsuarioDespacho() As Integer
            Get
                Return _idUsuarioDespacho
            End Get
            Set(ByVal value As Integer)
                _idUsuarioDespacho = value
            End Set
        End Property

        Public Property UsuarioDespacho() As String
            Get
                Return _usuarioDespacho
            End Get
            Protected Friend Set(ByVal value As String)
                _usuarioDespacho = value
            End Set
        End Property

        Public Property FechaCambioServicio() As Date
            Get
                Return _fechaCambioServicio
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaCambioServicio = value
            End Set
        End Property

        Public Property IdUsuarioCambioServicio() As Integer
            Get
                Return _idUsuarioCambioServicio
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCambioServicio = value
            End Set
        End Property

        Public Property UsuarioCambioServicio() As String
            Get
                Return _usuarioCambioServicio
            End Get
            Set(ByVal value As String)
                _usuarioCambioServicio = value
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

        Public Property Prioridad() As String
            Get
                Return _prioridad
            End Get
            Set(ByVal value As String)
                _prioridad = value
            End Set
        End Property

        Public Property FechaVencimientoReserva() As Date
            Get
                Return _fechaVencimientoReserva
            End Get
            Set(ByVal value As Date)
                _fechaVencimientoReserva = value
            End Set
        End Property

        Public Property Urgente() As Boolean
            Get
                Return _urgente
            End Get
            Set(ByVal value As Boolean)
                _urgente = value
            End Set
        End Property


        Public Property ReferenciasDataTable() As DataTable
            Get
                Return _referenciasDataTable
            End Get
            Set(ByVal value As DataTable)
                _referenciasDataTable = value
            End Set
        End Property

        Public Property MinsDataTable() As DataTable
            Get
                Return _minsDataTable
            End Get
            Set(ByVal value As DataTable)
                _minsDataTable = value
            End Set
        End Property

        Public Property ReferenciasColeccion() As DetalleMaterialServicioMensajeriaColeccion
            Get
                Return _referenciasColeccion
            End Get
            Set(ByVal value As DetalleMaterialServicioMensajeriaColeccion)
                _referenciasColeccion = value
            End Set
        End Property

        Public Property MinsColeccion() As DetalleMsisdnEnServicioMensajeriaColeccionCargue
            Get
                Return _minsColeccion
            End Get
            Set(ByVal value As DetalleMsisdnEnServicioMensajeriaColeccionCargue)
                _minsColeccion = value
            End Set
        End Property

        Public Property TablaServicio() As DataTable
            Get
                If _dtServicio Is Nothing Then _dtServicio = New DataTable
                Return _dtServicio
            End Get
            Set(ByVal value As DataTable)
                _dtServicio = value
            End Set
        End Property

        Public Property DisponibilidadAgenda() As Integer
            Get
                Return _disponibilidadAgenda
            End Get
            Set(ByVal value As Integer)
                _disponibilidadAgenda = value
            End Set
        End Property

        Public Property Adendo() As Boolean
            Get
                Return _adendo
            End Get
            Set(ByVal value As Boolean)
                _adendo = value
            End Set
        End Property
#End Region

#Region "Métodos privados"

        Public Sub CargarDatosServicio()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria

                    .ejecutarReader("ObtenerServicioMensajeriaCargue", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idCargueServicioMensajeria").ToString(), _idServicioMensajeria)
                            Integer.TryParse(.Reader("idBodega").ToString(), _idBodega)
                            _bodega = .Reader("bodega").ToString()
                            If Not IsDBNull(.Reader("fechaasignacion")) Then _fechaAsignacion = CDate(.Reader("fechaAsignacion"))
                            _usuarioEjecutor = .Reader("usuarioejecutor").ToString()
                            _observacion = .Reader("observacion").ToString()
                            _nombreCliente = .Reader("NomCliente").ToString()
                            _personaContacto = .Reader("nombreautorizado").ToString()
                            _identicacionCliente = .Reader("identicacion").ToString()
                            _nombreCiudad = .Reader("ciudad").ToString()
                            _barrio = .Reader("barrio").ToString()
                            _direccion = .Reader("direccion").ToString()
                            _telefonoContacto = .Reader("telefono").ToString()
                            _extensionContacto = .Reader("extension").ToString()
                            _tipoTelefono = .Reader("tipotelefono").ToString()
                            _numeroRadicado = .Reader("numeroradicado").ToString()
                            If Not IsDBNull(.Reader("clienteVIP")) Then _clienteVip = CBool(.Reader("clienteVip"))
                            _planActual = .Reader("planActual").ToString()
                            Integer.TryParse(.Reader("idtiposervicio").ToString(), _idTipoServicio)
                            _tipoServicio = .Reader("tiposervicio").ToString()
                            Integer.TryParse(.Reader("idPrioridad").ToString(), _idPrioridad)
                            _prioridad = .Reader("prioridad").ToString()
                            If Not IsDBNull(.Reader("fechavencimientoreserva")) Then _fechaVencimientoReserva = CDate(.Reader("fechavencimientoreserva"))
                            _adendo = CBool(.Reader("adendo"))
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If

                    _referenciasColeccion = New DetalleMaterialServicioMensajeriaColeccion(_idServicioMensajeria)
                    _minsColeccion = New DetalleMsisdnEnServicioMensajeriaColeccionCargue(_idServicioMensajeria)

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub ActualizaCargueArchivo()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                    .ejecutarNonQuery("ActualizaEstadoServicioCargue", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim noResultadoServicio As Integer = -1
            Dim idServicioMensajeria As Integer
            Dim idServicioTipo As Integer

            'Using dbManager As New LMDataAccess
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        If _idAgendamiento > 0 Then .Add("@idAgendamiento", SqlDbType.Int).Value = _idAgendamiento
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        If Not _fechaRegistro.Equals(Date.MinValue) Then .Add("@fecha", SqlDbType.DateTime).Value = _fechaRegistro
                        If Not _fechaAgenda.Equals(Date.MinValue) Then .Add("@fechaAgenda", SqlDbType.DateTime).Value = _fechaAgenda
                        If Not _fechaRegistroAgenda.Equals(Date.MinValue) Then .Add("@fechaRegistroAgenda", SqlDbType.DateTime).Value = _fechaRegistroAgenda
                        If Not _fechaAsignacion.Equals(Date.MinValue) Then .Add("@fechaAsignacion", SqlDbType.DateTime).Value = _fechaAsignacion
                        If _usuarioEjecutor <> String.Empty Then .Add("@usuarioEjecutor", SqlDbType.VarChar).Value = _usuarioEjecutor
                        If _idJornada > 0 Then .Add("@idJornada", SqlDbType.SmallInt).Value = _idJornada
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If _idReserva > 0 Then .Add("@idReserva", SqlDbType.Int).Value = _idReserva
                        If _idUsuarioConfirmacion > 0 Then .Add("@idUsuarioConfirmacion", SqlDbType.Int).Value = _idUsuarioConfirmacion
                        If Not _fechaConfirmacion.Equals(Date.MinValue) Then .Add("@fechaConfirmacion", SqlDbType.DateTime).Value = _fechaConfirmacion
                        If _idResponsableEntrega > 0 Then .Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega
                        If Not _fechaCierre.Equals(Date.MinValue) Then .Add("@fechaCierre", SqlDbType.DateTime).Value = _fechaCierre
                        If _idUsuarioCierre > 0 Then .Add("@idUsuarioCierre", SqlDbType.Int).Value = IdUsuarioCierre
                        If CodigoActivacion <> String.Empty Then .Add("@codigoActivacion", SqlDbType.VarChar).Value = _codigoActivacion
                        If Observacion <> String.Empty Then .Add("@observacion", SqlDbType.VarChar).Value = _observacion
                        If _nombreCliente <> String.Empty Then .Add("@nombre", SqlDbType.VarChar).Value = _nombreCliente
                        If _personaContacto <> String.Empty Then .Add("@nombreAutorizado", SqlDbType.VarChar).Value = _personaContacto
                        If _identicacionCliente <> String.Empty Then .Add("@identicacion", SqlDbType.VarChar).Value = _identicacionCliente
                        If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        If _barrio <> String.Empty Then .Add("@barrio", SqlDbType.VarChar).Value = _barrio
                        If _direccion <> String.Empty Then .Add("@direccion", SqlDbType.VarChar).Value = _direccion
                        If _telefonoContacto <> String.Empty Then .Add("@telefono", SqlDbType.VarChar).Value = _telefonoContacto
                        If _extensionContacto <> String.Empty Then .Add("@extension", SqlDbType.VarChar).Value = _extensionContacto
                        If _tipoTelefono <> String.Empty Then .Add("tipoTelefono", SqlDbType.VarChar).Value = _tipoTelefono
                        If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                        If _fechaVencimientoReserva > Date.MinValue Then .Add("@fechaVencimientoReserva", SqlDbType.SmallDateTime).Value = _fechaVencimientoReserva
                        If _idPrioridad > 0 Then .Add("@idPrioridad", SqlDbType.Int).Value = _idPrioridad

                        .Add("@ClienteVIP", SqlDbType.Bit).Value = _clienteVip
                        If _planActual <> String.Empty Then .Add("@planActual", SqlDbType.VarChar).Value = _planActual

                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                        .Add("@idServicioMensajeria", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With

                    .iniciarTransaccion()

                    'Registro en [ServicioMensajeria]
                    .ejecutarScalar("RegistraServicioMensajeria", CommandType.StoredProcedure)
                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultadoServicio)
                    Integer.TryParse(.SqlParametros("@idServicioMensajeria").Value.ToString(), idServicioMensajeria)

                    If noResultadoServicio = 0 Then

                        If _idTipoServicio <> 0 Then 'TODO: Se debe implementar la opción de crear varios tipos de servicio.
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = idServicioMensajeria
                            .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario

                            .SqlParametros.Add("@idServicioTipo", SqlDbType.Int).Direction = ParameterDirection.Output

                            'Registro en [ServicioMensajeriaTipoServicio]
                            .ejecutarScalar("RegistraTipoServicioMensajeria", CommandType.StoredProcedure)
                            Integer.TryParse(.SqlParametros("@idServicioTipo").Value.ToString(), idServicioTipo)

                            If idServicioTipo <> 0 Then
                                'Registro en [MaterialServicioTipoServicio]
                                If _referenciasDataTable IsNot Nothing AndAlso _referenciasDataTable.Rows.Count > 0 Then
                                    Dim columnaidServicioTipo As New DataColumn("idServicioTipo", GetType(Integer))
                                    columnaidServicioTipo.DefaultValue = idServicioTipo
                                    _referenciasDataTable.Columns.Add(columnaidServicioTipo)

                                    Dim columnaidUsuario As New DataColumn("idUsuario", GetType(Integer))
                                    columnaidUsuario.DefaultValue = _idUsuario
                                    _referenciasDataTable.Columns.Add(columnaidUsuario)

                                    .inicilizarBulkCopy()
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
                                    Dim columnaidServicioTipoMin As New DataColumn("idServicioTipo", GetType(Integer))
                                    columnaidServicioTipoMin.DefaultValue = idServicioTipo
                                    _minsDataTable.Columns.Add(columnaidServicioTipoMin)

                                    .inicilizarBulkCopy()
                                    With .BulkCopy
                                        .DestinationTableName = "MsisdnEnServicioMensajeria"
                                        .ColumnMappings.Add("idServicioTipo", "idServicioTipo")
                                        .ColumnMappings.Add("msisdn", "msisdn")
                                        .ColumnMappings.Add("activaEquipoAnterior", "activaEquipoAnterior")
                                        .ColumnMappings.Add("comSeguro", "comSeguro")
                                        .ColumnMappings.Add("precioConIVA", "precioConIVA")
                                        .ColumnMappings.Add("precioSinIVA", "precioSinIVA")
                                        .ColumnMappings.Add("idClausula", "idClausula")
                                        .WriteToServer(_minsDataTable)
                                    End With
                                End If                                

                                .confirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, "Transacción exitosa.")
                            Else
                                resultado.EstablecerMensajeYValor(7, "Se generó un error al tratar de generar tipo de Servicio de Mensajeria.")
                                .abortarTransaccion()
                            End If
                        Else
                            resultado.EstablecerMensajeYValor(8, "No se selecciono el tipo de servicio.")
                            .abortarTransaccion()
                        End If
                    Else
                        If noResultadoServicio = 1 Then resultado.EstablecerMensajeYValor(1, "El Número de Radicado ingresado ya se encuentra registrado en el sistema. Por favor verifique e intente nuevamente.")
                        If noResultadoServicio = -1 Then resultado.EstablecerMensajeYValor(9, "Se generó un error al tratar de generar Servicio de Mensajeria.")
                        .abortarTransaccion()
                    End If

                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End With
            dbManager.Dispose()
            'End Using
            Return resultado
        End Function

        Public Shared Function Registrar(ByVal servicios As List(Of MensajeriaEspecializada.CargueServicioMensajeria), _
                                         ByRef errores As Dictionary(Of Integer, String), _
                                         ByRef correctos As Dictionary(Of Long, String)) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim noResultadoServicio As Integer = -1
            Dim idServicioMensajeria As Integer
            Dim idServicioTipo As Integer

            If Not servicios Is Nothing AndAlso servicios.Count > 0 Then
                Dim dbManager As New LMDataAccess
                With dbManager
                    Try
                        .iniciarTransaccion()
                        For Each servicioObj As MensajeriaEspecializada.CargueServicioMensajeria In servicios
                            With .SqlParametros
                                .Clear()
                                If servicioObj.IdAgendamiento > 0 Then .Add("@idAgendamiento", SqlDbType.Int).Value = servicioObj.IdAgendamiento
                                .Add("@idUsuario", SqlDbType.Int).Value = servicioObj.IdUsuario
                                If Not servicioObj.FechaRegistro.Equals(Date.MinValue) Then .Add("@fecha", SqlDbType.DateTime).Value = servicioObj.FechaRegistro
                                If Not servicioObj.FechaAgenda.Equals(Date.MinValue) Then .Add("@fechaAgenda", SqlDbType.DateTime).Value = servicioObj.FechaAgenda
                                If Not servicioObj.FechaRegistroAgenda.Equals(Date.MinValue) Then .Add("@fechaRegistroAgenda", SqlDbType.DateTime).Value = servicioObj.FechaRegistroAgenda
                                If Not servicioObj.FechaAsignacion.Equals(Date.MinValue) Then .Add("@fechaAsignacion", SqlDbType.DateTime).Value = servicioObj.FechaAsignacion
                                If servicioObj.UsuarioEjecutor <> String.Empty Then .Add("@usuarioEjecutor", SqlDbType.VarChar).Value = servicioObj.UsuarioEjecutor
                                If servicioObj.Jornada > 0 Then .Add("@idJornada", SqlDbType.SmallInt).Value = servicioObj.Jornada
                                .Add("@idEstado", SqlDbType.Int).Value = servicioObj.IdEstado
                                If servicioObj.IdReserva > 0 Then .Add("@idReserva", SqlDbType.Int).Value = servicioObj.IdReserva
                                If servicioObj.IdUsuarioConfirmacion > 0 Then .Add("@idUsuarioConfirmacion", SqlDbType.Int).Value = servicioObj.IdUsuarioConfirmacion
                                If Not servicioObj.FechaConfirmacion.Equals(Date.MinValue) Then .Add("@fechaConfirmacion", SqlDbType.DateTime).Value = servicioObj.FechaConfirmacion
                                If servicioObj.IdResponsableEntrega > 0 Then .Add("@idResponsableEntrega", SqlDbType.Int).Value = servicioObj.IdResponsableEntrega
                                If Not servicioObj.FechaCierre.Equals(Date.MinValue) Then .Add("@fechaCierre", SqlDbType.DateTime).Value = servicioObj.FechaCierre
                                If servicioObj.IdUsuarioCierre > 0 Then .Add("@idUsuarioCierre", SqlDbType.Int).Value = servicioObj.IdUsuarioCierre
                                If servicioObj.CodigoActivacion <> String.Empty Then .Add("@codigoActivacion", SqlDbType.VarChar).Value = servicioObj.CodigoActivacion
                                If servicioObj.Observacion <> String.Empty Then .Add("@observacion", SqlDbType.VarChar).Value = servicioObj.Observacion
                                If servicioObj.NombreCliente <> String.Empty Then .Add("@nombre", SqlDbType.VarChar).Value = servicioObj.NombreCliente
                                If servicioObj.PersonaContacto <> String.Empty Then .Add("@nombreAutorizado", SqlDbType.VarChar).Value = servicioObj.PersonaContacto
                                If servicioObj.IdenticacionCliente <> String.Empty Then .Add("@identicacion", SqlDbType.VarChar).Value = servicioObj.IdenticacionCliente
                                If servicioObj.IdCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = servicioObj.IdCiudad
                                If servicioObj.IdBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = servicioObj.IdBodega
                                If servicioObj.Barrio <> String.Empty Then .Add("@barrio", SqlDbType.VarChar).Value = servicioObj.Barrio
                                If servicioObj.Direccion <> String.Empty Then .Add("@direccion", SqlDbType.VarChar).Value = servicioObj.Direccion
                                If servicioObj.TelefonoContacto <> String.Empty Then .Add("@telefono", SqlDbType.VarChar).Value = servicioObj.TelefonoContacto
                                If servicioObj.ExtensionContacto <> String.Empty Then .Add("@extension", SqlDbType.VarChar).Value = servicioObj.ExtensionContacto
                                If servicioObj.TipoTelefono <> String.Empty Then .Add("tipoTelefono", SqlDbType.VarChar).Value = servicioObj.TipoTelefono
                                If servicioObj.NumeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = servicioObj.NumeroRadicado
                                If servicioObj.FechaVencimientoReserva > Date.MinValue Then .Add("@fechaVencimientoReserva", SqlDbType.SmallDateTime).Value = servicioObj.FechaVencimientoReserva
                                If servicioObj.IdPrioridad > 0 Then .Add("@idPrioridad", SqlDbType.Int).Value = servicioObj.IdPrioridad

                                .Add("@ClienteVIP", SqlDbType.Bit).Value = servicioObj.ClienteVIP
                                .Add("@adendo", SqlDbType.Bit).Value = servicioObj.Adendo
                                If servicioObj.PlanActual <> String.Empty Then .Add("@planActual", SqlDbType.VarChar).Value = servicioObj.PlanActual

                                .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                                .Add("@idServicioMensajeria", SqlDbType.Int).Direction = ParameterDirection.Output
                            End With

                            'Registro en [ServicioMensajeria]
                            .ejecutarScalar("RegistraCargueServicioMensajeria", CommandType.StoredProcedure)
                            Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultadoServicio)
                            Integer.TryParse(.SqlParametros("@idServicioMensajeria").Value.ToString(), idServicioMensajeria)

                            If noResultadoServicio = 0 Then
                                If servicioObj.IdTipoServicio <> 0 Then 'TODO: Se debe implementar la opción de crear varios tipos de servicio.
                                    .SqlParametros.Clear()
                                    .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = idServicioMensajeria
                                    .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = servicioObj.IdTipoServicio
                                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = servicioObj.IdUsuario

                                    .SqlParametros.Add("@idServicioTipo", SqlDbType.Int).Direction = ParameterDirection.Output

                                    'Registro en [ServicioMensajeriaTipoServicio]
                                    .ejecutarScalar("RegistraCargueTipoServicioMensajeria", CommandType.StoredProcedure)
                                    Integer.TryParse(.SqlParametros("@idServicioTipo").Value.ToString(), idServicioTipo)

                                    If idServicioTipo <> 0 Then
                                        'Registro en [MsisdnEnServicioMensajeria]
                                        If servicioObj.MinsDataTable IsNot Nothing AndAlso servicioObj.MinsDataTable.Rows.Count > 0 Then
                                            Dim columnaidServicioTipoMin As New DataColumn("idServicioTipo", GetType(Integer))
                                            columnaidServicioTipoMin.DefaultValue = idServicioTipo
                                            servicioObj.MinsDataTable.Columns.Add(columnaidServicioTipoMin)

                                            .inicilizarBulkCopy()
                                            With .BulkCopy
                                                .DestinationTableName = "CargueMsisdnEnServicioMensajeria"
                                                .ColumnMappings.Add("idServicioTipo", "idServicioTipo")
                                                .ColumnMappings.Add("msisdn", "msisdn")
                                                .ColumnMappings.Add("activaEquipoAnterior", "activaEquipoAnterior")
                                                .ColumnMappings.Add("comSeguro", "comSeguro")
                                                .ColumnMappings.Add("precioConIVA", "precioConIVA")
                                                .ColumnMappings.Add("precioSinIVA", "precioSinIVA")
                                                .ColumnMappings.Add("idClausula", "idClausula")
                                                .WriteToServer(servicioObj.MinsDataTable)
                                            End With
                                        End If
                                        correctos.Add(servicioObj.NumeroRadicado, "El Número de servicio " + servicioObj.NumeroRadicado.ToString() + " se registró existosamente.")
                                    Else
                                        resultado.EstablecerMensajeYValor(7, "Se generó un error al tratar de generar tipo de Servicio de Mensajeria.")
                                    End If
                                Else
                                    errores.Add(servicioObj.NumeroRadicado, "No se selecciono el tipo de servicio. En el número de radicado: " + servicioObj.NumeroRadicado.ToString())
                                End If
                            Else
                                If noResultadoServicio = 1 Then errores.Add(servicioObj.NumeroRadicado, "El Número de servicio " + servicioObj.NumeroRadicado.ToString() + " ya se encuentra registrado en el sistema. Por favor verifique e intente nuevamente.")
                                If noResultadoServicio = -1 Then errores.Add(servicioObj.NumeroRadicado, "Se generó un error al tratar de generar el cargue Servicio de Mensajeria.")
                            End If
                        Next
                        .confirmarTransaccion()
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    Finally
                        If dbManager IsNot Nothing Then dbManager.Dispose()
                    End Try

                End With
                dbManager.Dispose()
            Else
                errores.Add(1, "Por favor verifique que el archivo tenga servicios para cargar.")
            End If
            Return resultado
        End Function

        Public Function Actualizar(ByVal idUsuarioLog As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim idResultado As Integer = -1
            Dim idServicioTipo As Integer = -1

            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                        .Add("@idUsuarioLog", SqlDbType.Int).Value = idUsuarioLog

                        If _idAgendamiento > 0 Then .Add("@idAgendamiento", SqlDbType.Int).Value = _idAgendamiento
                        If _idUsuario > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        If Not _fechaAgenda.Equals(Date.MinValue) Then .Add("@fechaAgenda", SqlDbType.DateTime).Value = _fechaAgenda
                        If Not _fechaRegistroAgenda.Equals(Date.MinValue) Then .Add("@fechaRegistroAgenda", SqlDbType.DateTime).Value = _fechaRegistroAgenda
                        If Not _fechaAsignacion.Equals(Date.MinValue) Then .Add("@fechaAsignacion", SqlDbType.DateTime).Value = _fechaAsignacion
                        If _usuarioEjecutor <> String.Empty Then .Add("@usuarioEjecutor", SqlDbType.VarChar).Value = _usuarioEjecutor
                        If _idJornada > 0 Then .Add("@idJornada", SqlDbType.SmallInt).Value = _idJornada
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If _idReserva > 0 Then .Add("@idReserva", SqlDbType.Int).Value = _idReserva
                        If _idUsuarioConfirmacion > 0 Then .Add("@idUsuarioConfirmacion", SqlDbType.Int).Value = _idUsuarioConfirmacion
                        If Not _fechaConfirmacion.Equals(Date.MinValue) Then .Add("@fechaConfirmacion", SqlDbType.DateTime).Value = _fechaConfirmacion
                        If _idResponsableEntrega > 0 Then .Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega
                        If Not _fechaCierre.Equals(Date.MinValue) Then .Add("@fechaCierre", SqlDbType.DateTime).Value = _fechaCierre
                        If _idUsuarioCierre > 0 Then .Add("@idUsuarioCierre", SqlDbType.Int).Value = IdUsuarioCierre
                        If CodigoActivacion <> String.Empty Then .Add("@codigoActivacion", SqlDbType.VarChar).Value = _codigoActivacion
                        If Observacion <> String.Empty Then .Add("@observacion", SqlDbType.VarChar).Value = _observacion
                        If _nombreCliente <> String.Empty Then .Add("@nombre", SqlDbType.VarChar).Value = _nombreCliente
                        If _personaContacto <> String.Empty Then .Add("@nombreAutorizado", SqlDbType.VarChar).Value = _personaContacto
                        If _identicacionCliente <> String.Empty Then .Add("@identicacion", SqlDbType.VarChar).Value = _identicacionCliente
                        If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        If _barrio <> String.Empty Then .Add("@barrio", SqlDbType.VarChar).Value = _barrio
                        If _direccion <> String.Empty Then .Add("@direccion", SqlDbType.VarChar).Value = _direccion
                        If _telefonoContacto <> String.Empty Then .Add("@telefono", SqlDbType.VarChar).Value = _telefonoContacto
                        If _extensionContacto <> String.Empty Then .Add("@extension", SqlDbType.VarChar).Value = _extensionContacto
                        If _tipoTelefono <> String.Empty Then .Add("@tipoTelefono", SqlDbType.VarChar, 1).Value = _tipoTelefono
                        If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                        .Add("@ClienteVIP", SqlDbType.Bit).Value = _clienteVip
                        If _planActual <> String.Empty Then .Add("@planActual", SqlDbType.VarChar).Value = _planActual
                        If Not _fechaVencimientoReserva.Equals(Date.MinValue) Then .Add("@fechaVencimientoReserva", SqlDbType.DateTime).Value = _fechaVencimientoReserva
                        .Add("@urgente", SqlDbType.Bit).Value = _urgente

                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With

                    .iniciarTransaccion()
                    .ejecutarScalar("ActualizaServicioMensajeria", CommandType.StoredProcedure)
                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), idResultado)

                    If idResultado = 0 Then

                        .confirmarTransaccion()
                        resultado.EstablecerMensajeYValor(0, "Transacción exitosa.")
                    Else
                        resultado.EstablecerMensajeYValor(7, "Se generó un error al tratar de actualizar.")
                        .abortarTransaccion()
                    End If
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                End Try
            End With
            dbManager.Dispose()

            Return resultado
        End Function

        Public Function Confirmar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Not (_idServicioMensajeria = 0 OrElse String.IsNullOrEmpty(_direccion) OrElse String.IsNullOrEmpty(_barrio) _
                OrElse _fechaAgenda = Date.MinValue OrElse _idJornada = 0 OrElse _idUsuarioConfirmacion = 0) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@direccion", SqlDbType.VarChar).Value = _direccion
                        .SqlParametros.Add("@barrio", SqlDbType.VarChar, 70).Value = _barrio
                        .SqlParametros.Add("@fechaAgenda", SqlDbType.SmallDateTime).Value = _fechaAgenda
                        .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                        .SqlParametros.Add("@idUsuarioConfirma", SqlDbType.Int).Value = _idUsuarioConfirmacion
                        .SqlParametros.Add("@telefonoContacto", SqlDbType.VarChar, 30).Value = _telefonoContacto
                        .SqlParametros.Add("@extensionContacto", SqlDbType.VarChar, 30).Value = _extensionContacto
                        .SqlParametros.Add("@tipoTelefono", SqlDbType.VarChar, 1).Value = _tipoTelefono
                        .SqlParametros.Add("@personaContacto", SqlDbType.VarChar, 155).Value = _personaContacto
                        .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                        .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ConfirmarServicioMensajeria", CommandType.StoredProcedure)
                        If Not IsDBNull(.SqlParametros("@result").Value) Then
                            resultado.Valor = CShort(.SqlParametros("@result").Value)
                            If resultado.Valor = 0 Then
                                resultado.Mensaje = "El servicio fue confirmado de manera exitosa."
                                .confirmarTransaccion()
                            Else
                                If resultado.Valor = 1 Then
                                    resultado.Mensaje = "Ocurrió un error inesperado al confirmar el servicio. Por favor intente nuevamente"
                                Else
                                    resultado.Mensaje = "No se pudo realizar la reserva de inventario para atender el servicio. Por favor intente nuevamente"
                                End If

                                .abortarTransaccion()
                            End If
                        Else
                            Throw New Exception("Ocurrió un error interno al confirmar el servicio. Por favor intente nuevamente")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                End Try
            Else
                resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para realizar la confirmación. ")
            End If

            Return resultado
        End Function

        Public Function Anular(ByVal idUsuario As Integer)
            Dim resultado As New ResultadoProceso

            Using dbManager As New LMDataAccess
                With dbManager
                    Try
                        .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@idUsuarioLog", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output

                        .iniciarTransaccion()
                        .ejecutarNonQuery("AnularServicioMensajeria", CommandType.StoredProcedure)

                        Dim idResultado As Integer
                        Integer.TryParse(.SqlParametros("@resultado").Value, idResultado)

                        If idResultado = 0 Then
                            resultado.EstablecerMensajeYValor(idResultado, "Anulación exitosa.")
                            .confirmarTransaccion()
                        Else
                            resultado.EstablecerMensajeYValor(idResultado, "Imposible realizar la anulación.")
                            .abortarTransaccion()
                        End If
                    Catch ex As Exception
                        .abortarTransaccion()
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try
                End With
            End Using
            Return resultado
        End Function

        Public Function LeerSerial(ByVal serial As String, ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idServicioMensajeria > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = serial
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarSerialEnServicioMensajeria", CommandType.StoredProcedure)

                        If Not IsDBNull(.SqlParametros("@result").Value) Then
                            resultado.Valor = CShort(.SqlParametros("@result").Value)
                            If resultado.Valor = 0 Then
                                resultado.Mensaje = "El serial fue registrado satisfactoriamente."
                                .confirmarTransaccion()
                            Else
                                Select Case resultado.Valor
                                    Case 1
                                        resultado.Mensaje = "El seriale no existe en el inventario de bodegas satélites."
                                    Case 2
                                        resultado.Mensaje = "El serial no está asignado a la bodega que prestará el servicio."
                                    Case 3
                                        resultado.Mensaje = "El serial está bloqueado para despacho."
                                    Case 4
                                        resultado.Mensaje = "El material asociado al serial no existe en el detalle del servicio."
                                    Case 5
                                        resultado.Mensaje = "El material asociado al serial ya fue leido en su totalidad en el despacho actual."
                                    Case 6
                                        resultado.Mensaje = "El serial no corresponde a la región del Servicio."
                                    Case 7
                                        resultado.Mensaje = "El material asociado al serial no pertence al MSISDN proporcionado"
                                    Case 8
                                        resultado.Mensaje = "El servicio ya se encuentra despachado no se pueden adicionar seriales por esta opción"
                                    Case Else
                                        resultado.Mensaje = "Ocurrió un error inesperado al confirmar el servicio. Por favor intente nuevamente."

                                End Select
                                .abortarTransaccion()
                            End If
                        Else
                            Throw New Exception("Ocurrió un error interno al registrar serial. Por favor intente nuevamente")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para realizar la confirmación. ")
            End If

            Return resultado
        End Function

        Public Function CerrarDespacho() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Not (_idServicioMensajeria = 0 OrElse _idUsuarioCierre = 0) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioCierre
                        .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CerrarDespachoServicioMensajeria", CommandType.StoredProcedure)
                        If Not IsDBNull(.SqlParametros("@result").Value) Then
                            resultado.Valor = CShort(.SqlParametros("@result").Value)
                            If resultado.Valor = 0 Then
                                resultado.Mensaje = "El despacho fue cerrado de manera exitosa."
                                .confirmarTransaccion()
                            Else
                                resultado.Mensaje = "Ocurrió un error inesperado al cerrar despacho. Por favor intente nuevamente"
                                .abortarTransaccion()
                            End If
                        Else
                            Throw New Exception("Ocurrió un error interno al cerrar despacho. Por favor intente nuevamente")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                End Try
            Else
                resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para cerrar el despacho. ")
            End If

            Return resultado
        End Function

        Public Function RegistrarCambioDeServicio(ByVal imei As String, ByVal iccid As String, ByVal msisdn As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idServicioMensajeria > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        If Not String.IsNullOrEmpty(imei) Then .SqlParametros.Add("@imei", SqlDbType.VarChar, 50).Value = imei
                        If Not String.IsNullOrEmpty(iccid) Then .SqlParametros.Add("@iccid", SqlDbType.VarChar, 50).Value = iccid
                        If Not String.IsNullOrEmpty(msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar, 50).Value = msisdn
                        .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarCambioDeServicio", CommandType.StoredProcedure)

                        If Not IsDBNull(.SqlParametros("@result").Value) Then
                            resultado.Valor = CShort(.SqlParametros("@result").Value)
                            If resultado.Valor = 0 Then
                                If String.IsNullOrEmpty(imei) Or String.IsNullOrEmpty(iccid) Then
                                    resultado.Mensaje = "El cambio de servicio fue registrado satisfactoriamente."
                                Else
                                    resultado.Mensaje = "Los cambios de servicio fueron registrados satisfactoriamente."
                                End If
                                .confirmarTransaccion()
                            Else
                                Select Case resultado.Valor
                                    Case 1
                                        resultado.Mensaje = "El Imei proporcionado no figura como despachado en el servicio actual"
                                    Case 2
                                        resultado.Mensaje = "El Iccid proporcionado no figura como despachado en el servicio actual"
                                    Case Else
                                        resultado.Mensaje = "Ocurrió un error inesperado al registrar cambio de servicio. Por favor intente nuevamente."

                                End Select
                                .abortarTransaccion()
                            End If
                        Else
                            Throw New Exception("Ocurrió un error interno al registrar serial. Por favor intente nuevamente")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para realizar la confirmación. ")
            End If

            Return resultado
        End Function

        Public Function FinalizarCambioServicio(ByVal observacion As String, ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                    .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = observacion
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                    .iniciarTransaccion()
                    .ejecutarNonQuery("FinalizarCambioDeServicio", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@result").Value) Then
                        resultado.Valor = CShort(.SqlParametros("@result").Value)
                        If resultado.Valor = 0 Then
                            resultado.Mensaje = "El proceso fue finalizado de manera exitosa."
                            .confirmarTransaccion()
                        Else
                            resultado.Mensaje = "Ocurrió un error inesperado al finalizar cambios de servicio. Por favor intente nuevamente"
                            .abortarTransaccion()
                        End If
                    Else
                        Throw New Exception("Ocurrió un error interno al finalizar cambios. Por favor intente nuevamente")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            End Try

            Return resultado
        End Function

        Public Function ConfirmarEntrega()
            Dim resultado As New ResultadoProceso

            'Using dbManager As New LMDataAccess
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@idZona", SqlDbType.Int).Value = _idZona
                    .SqlParametros.Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega

                    .iniciarTransaccion()

                    .ejecutarReader("ConfirmarEntregaServicioMensajeria", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing And .Reader.HasRows Then
                        If .Reader.Read() Then
                            If CInt(.Reader.Item(0).ToString()) = 0 Then
                                resultado.EstablecerMensajeYValor(CInt(.Reader.Item(0).ToString()), .Reader.Item(1).ToString())
                                .Reader.Close()
                                .confirmarTransaccion()
                            Else
                                resultado.EstablecerMensajeYValor(CInt(.Reader.Item(0).ToString()), .Reader.Item(1).ToString())
                                .abortarTransaccion()
                            End If
                        Else
                            .abortarTransaccion()
                        End If
                    Else
                        .abortarTransaccion()
                        Throw New Exception("Ocurrió un error interno al finalizar cambios. Por favor intente nuevamente")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            End Try
            dbManager.Dispose()
            'End Using

            Return resultado
        End Function

        Public Function Reabrir(ByVal idUsuario As Integer, Optional ByVal observacion As String = "", _
                                Optional ByVal idNuevoEstado As Integer = 0) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idServicioMensajeria > 0 AndAlso idUsuario > 0 Then
                Using dbManager As New LMDataAccess
                    With dbManager
                        Try
                            .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                            If Not String.IsNullOrEmpty(observacion) Then .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = observacion
                            If idNuevoEstado > 0 Then .SqlParametros.Add("@idNuevoEstado", SqlDbType.Int).Value = idNuevoEstado
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            .iniciarTransaccion()
                            .ejecutarNonQuery("ReabrirServicioMensajeriaDespachado", CommandType.StoredProcedure)
                            If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                                resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                                If resultado.Valor = 0 Then
                                    .confirmarTransaccion()
                                Else
                                    If resultado.Valor = 1 Then
                                        resultado.Mensaje = "El servicio proporcionado no existe. Por favor verifique"
                                    Else
                                        resultado.Mensaje = "Ocurrió un error inesperado al tratar de reabrir servicio. Por favor intente nuevamente"
                                    End If
                                    .abortarTransaccion()
                                End If
                            Else
                                Throw New Exception("Imposible evaluar la respuesta del servidor. Por favor intente nuevamente")
                            End If
                        Catch ex As Exception
                            If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                            Throw New Exception(ex.Message, ex)
                        End Try
                    End With
                End Using
            Else
                resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para reabrir el servicio. ")
            End If
            Return resultado
        End Function

        Public Function Cancelar(ByVal idUsuario As Integer, Optional ByVal observacion As String = "", _
                                Optional ByVal idNuevoEstado As Integer = 0) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idServicioMensajeria > 0 AndAlso idUsuario > 0 Then
                Using dbManager As New LMDataAccess
                    With dbManager
                        Try
                            .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                            If Not String.IsNullOrEmpty(observacion) Then .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = observacion
                            If idNuevoEstado > 0 Then .SqlParametros.Add("@idNuevoEstado", SqlDbType.Int).Value = idNuevoEstado
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            .iniciarTransaccion()
                            .ejecutarNonQuery("CancelarServicioMensajeriaDespachado", CommandType.StoredProcedure)
                            If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                                resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                                If resultado.Valor = 0 Then
                                    .confirmarTransaccion()
                                Else
                                    If resultado.Valor = 1 Then
                                        resultado.Mensaje = "El servicio proporcionado no existe. Por favor verifique"
                                    Else
                                        resultado.Mensaje = "Ocurrió un error inesperado al tratar de cancelar servicio. Por favor intente nuevamente"
                                    End If
                                    .abortarTransaccion()
                                End If
                            Else
                                Throw New Exception("Imposible evaluar la respuesta del servidor. Por favor intente nuevamente")
                            End If
                        Catch ex As Exception
                            If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                            Throw New Exception(ex.Message, ex)
                        End Try
                    End With
                End Using
            Else
                resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para realizar la cancelacion. ")
            End If
            Return resultado
        End Function

        Public Function Reactivar(ByVal idUsuario As Integer, ByVal observacion As String, _
                                  Optional ByVal idNuevoEstado As Enumerados.EstadoServicio = Enumerados.EstadoServicio.Creado, _
                                  Optional ByVal numeroRadicadoNuevo As Long = 0) As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Using dbManager As New LMDataAccess
                With dbManager
                    Try
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = observacion
                        .SqlParametros.Add("@idNuevoEstado", SqlDbType.Int).Value = idNuevoEstado
                        If numeroRadicadoNuevo > 0 Then .SqlParametros.Add("@numeroRadicadoNuevo", SqlDbType.BigInt).Value = numeroRadicadoNuevo

                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("ReactivarServicioMensajeria", CommandType.StoredProcedure)
                        If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                            resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                            If resultado.Valor = 0 Then
                                .confirmarTransaccion()
                            Else
                                If resultado.Valor = 1 Then
                                    resultado.Mensaje = "El servicio proporcionado no existe. Por favor verifique."
                                Else
                                    resultado.Mensaje = "Ocurrió un error inesperado al tratar de reabrir servicio. Por favor intente nuevamente."
                                End If
                                .abortarTransaccion()
                            End If
                        Else
                            Throw New Exception("Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                        End If
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso .estadoTransaccional Then .abortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try
                End With
            End Using

            Return resultado
        End Function

        Public Function ConsultarCapacidad() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If IdUsuario >= 0 Then
                Dim dbManager As New LMDataAccess

                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@fecha", SqlDbType.DateTime).Value = _fechaAgenda
                        .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .SqlParametros.Add("@disponibilidad", SqlDbType.BigInt).Direction = ParameterDirection.Output

                        .ejecutarNonQuery("ConsultarCapacidadCEM", CommandType.StoredProcedure)
                        If Integer.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado.Valor) Then
                            If resultado.Valor = 0 Then
                                resultado.Mensaje = "Cupos de entrega disponibles: " & CInt(.SqlParametros("@disponibilidad").Value)
                            Else
                                Select Case resultado.Valor
                                    Case 1
                                        resultado.Mensaje = "No se pudo evaluar la capacidad de entrega, puesto que no existen registros para la fecha y jornada especificados. Por favor contacte al personal administrativo del CEM"
                                    Case 2
                                        resultado.Mensaje = "No se puede confirmar el servicio, pues no existe disponibilidad de entrega en la fecha y jornada seleccionadas."
                                    Case Else
                                        resultado.Mensaje = "Ocurrió un error inesperado al tratar de validar campacidad de entrega. Por favor intente nuevamente."
                                End Select
                            End If
                        Else
                            resultado.EstablecerMensajeYValor("9", "Imposible evaluar la respuesta del servidos. Por favor intente nuevamente")
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para la consulta.")
            End If
            Return resultado
        End Function

#End Region

#Region "Metodos Compartidos"

        Public Function GenerarPool() As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Clear()
                        If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                        If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.Int).Value = _numeroRadicado
                        If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _idCiudadBodega > 0 Then .Add("@idCiudadBodega", SqlDbType.Int).Value = _idCiudadBodega
                        If _idTipoServicio > 0 Then .Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio                        
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerServicioMensajeriaCargue", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw ex
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

#End Region
    End Class

End Namespace