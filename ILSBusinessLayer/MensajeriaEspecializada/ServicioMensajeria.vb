Imports ILSBusinessLayer
Imports ILSBusinessLayer.Inventario
Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class ServicioMensajeria
        Implements IServicioMensajeria


#Region "Atributos"

        Protected Friend _idServicioMensajeria As Long
        Protected Friend _idAgendamiento As Integer
        Protected Friend _idBase As Integer
        Protected Friend _idBodega As Integer
        Protected Friend _bodega As String
        Protected Friend _idUsuario As Integer
        Protected Friend _usuarioRegistra As String
        Protected Friend _fechaRegistro As Date
        Protected Friend _fechaAgenda As Date
        Protected Friend _fechaRegistroAgenda As Date
        Protected Friend _fechaAsignacion As Date
        Protected Friend _usuarioEjecutor As String
        Protected Friend _idJornada As Short
        Protected Friend _idEmpresa As Integer
        Protected Friend _jornada As String
        Protected Friend _idEstado As Integer
        Protected Friend _estado As String
        Protected Friend _idReserva As Integer
        Protected Friend _idUsuarioConfirmacion As Integer
        Protected Friend _usuarioConfirmacion As String
        Protected Friend _fechaConfirmacion As Date
        Protected Friend _idResponsableEntrega As Integer
        Protected Friend _responsableEntrega As String
        Protected Friend _fechaCierre As Date
        Protected Friend _idUsuarioCierre As Integer
        Protected Friend _usuarioCierre As String
        Protected Friend _codigoActivacion As String
        Protected Friend _observacion As String
        Protected Friend _nombreCliente As String
        Protected Friend _personaContacto As String
        Protected Friend _identificacionCliente As String
        Protected Friend _idCiudad As Integer
        Protected Friend _nombreCiudad As String
        Protected Friend _barrio As String
        Protected Friend _direccion As String
        Protected Friend _telefonoCelular As String
        Protected Friend _telefonoFijo As String
        Protected Friend _telefonoContacto As String
        Protected Friend _extensionContacto As String
        Protected Friend _tipoTelefono As String
        Protected Friend _numeroRadicado As Long
        Protected Friend _clienteVip As Boolean
        Protected Friend _planActual As String
        Protected Friend _idTipoServicio As Integer
        Protected Friend _actividadLaboral As String
        Protected Friend _idCampania As Integer
        Protected Friend _Campania As String
        Protected Friend _tipoServicio As String
        Protected Friend _registrado As Boolean
        Protected Friend _idZona As Integer
        Protected Friend _nombreZona As String
        Protected Friend _facturaCambioServicio As String
        Protected Friend _remisionCambioServicio As String
        Protected Friend _novedadEnCambioServicio As Boolean
        Protected Friend _observacionCambioServicio As String
        Protected Friend _fechaDespacho As Date
        Protected Friend _idUsuarioDespacho As Integer
        Protected Friend _usuarioDespacho As String
        Protected Friend _idPrioridad As Integer
        Protected Friend _prioridad As String
        Protected Friend _fechaDevolucion As Date
        Protected Friend _idUsuarioDevolucion As Integer
        Protected Friend _fechaVencimientoReserva As Date
        Protected Friend _disponibilidadAgenda As Integer
        Protected Friend _fechaCambioServicio As Date
        Protected Friend _idUsuarioCambioServicio As Integer
        Protected Friend _usuarioCambioServicio As String
        Protected Friend _disponibleAutomarcado As Boolean
        Protected Friend _fechaAgendaEntrega As Date
        Protected Friend _idServicioTipo As Long
        Protected Friend _reagenda As Boolean
        Protected Friend _fechaIni As Date
        Protected Friend _fechaFin As Date
        Protected Friend _urgente As Boolean
        Protected Friend _medioEnvioCH As String
        Protected Friend _correoEnvioCH As String
        Protected Friend _numeroGuia As String
        Protected Friend _idTransportadora As Integer
        Protected Friend _referenciasDataTable As DataTable
        Protected Friend _minsDataTable As DataTable
        Protected Friend _tablaNovedad As DataTable
        Protected Friend _imeisDataTable As DataTable
        Protected Friend _dtDetalleArchivo As DataTable
        Protected Friend _detalleBloqueoInventario As BloqueoInventario
        Protected Friend _referenciasColeccion As DetalleMaterialServicioMensajeriaColeccion
        Protected Friend _minsColeccion As DetalleMsisdnEnServicioMensajeriaColeccion
        Protected Friend _codOficinaCliene As String
        Protected Friend _productoNoSerializado As String
        Private _idCampaniaList As ArrayList
        Private _estadoBanco As String
        Private _estadoConfrimacion As Integer

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idServicio As Integer)
            MyBase.New()
            _idServicioMensajeria = idServicio
            CargarDatos()
        End Sub

        ''' <summary>
        ''' Sobrecarga del constructor con el número de radicado
        ''' </summary>
        ''' <param name="numeroRadicado"></param>
        ''' <remarks>
        ''' Instanciar este constructor de la siguiente forma:
        ''' Dim XXX As Long = 12345
        ''' Dim x = new ServicioMensajeria(numeroRadicado:=XXX)
        ''' 
        ''' </remarks>
        Public Sub New(ByVal numeroRadicado As Long)
            MyBase.New()
            _numeroRadicado = numeroRadicado
            CargarDatos()
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

        Public Property IdBase() As Integer
            Get
                Return _idBase
            End Get
            Set(ByVal value As Integer)
                _idBase = value
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
            Set(ByVal value As Date)
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

        Public Property IdEmpresa() As Short
            Get
                Return _idEmpresa
            End Get
            Set(ByVal value As Short)
                _idEmpresa = value
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
            Set(ByVal value As Date)
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

        Public Property IdentificacionCliente() As String
            Get
                Return _identificacionCliente
            End Get
            Set(ByVal value As String)
                _identificacionCliente = value
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

        Public Property TelefonoFijo() As String
            Get
                Return _telefonoFijo
            End Get
            Set(ByVal value As String)
                _telefonoFijo = value
            End Set
        End Property

        Public Property TelefonoCelular() As String
            Get
                Return _telefonoCelular
            End Get
            Set(ByVal value As String)
                _telefonoCelular = value
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

        Public Property NumeroRadicado() As Long
            Get
                Return _numeroRadicado
            End Get
            Set(ByVal value As Long)
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

        Public Property ActividadLaboral() As String
            Get
                Return _actividadLaboral
            End Get
            Set(value As String)
                _actividadLaboral = value
            End Set
        End Property

        Public Property Campania() As String
            Get
                Return _Campania
            End Get
            Set(value As String)
                _Campania = value
            End Set
        End Property

        Public Property IdCampania() As Integer
            Get
                Return _idCampania
            End Get
            Set(value As Integer)
                _idCampania = value
            End Set
        End Property

        Public Property IdServicioTipo As Long
            Get
                Return _idServicioTipo
            End Get
            Set(value As Long)
                _idServicioTipo = value
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

        Public Property FechaDevolucion As Date
            Get
                Return _fechaDevolucion
            End Get
            Set(value As Date)
                _fechaDevolucion = value
            End Set
        End Property

        Public Property IdUsuarioDevolucion As Integer
            Get
                Return _idUsuarioDevolucion
            End Get
            Set(value As Integer)
                _idUsuarioDevolucion = value
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

        Public Property DisponibleAutomarcado() As Boolean
            Get
                Return _disponibleAutomarcado
            End Get
            Set(ByVal value As Boolean)
                _disponibleAutomarcado = value
            End Set
        End Property

        Public Property MedioEnvioCH As String
            Get
                Return _medioEnvioCH
            End Get
            Set(value As String)
                _medioEnvioCH = value
            End Set
        End Property

        Public Property CorreoEnvioCH As String
            Get
                Return _correoEnvioCH
            End Get
            Set(value As String)
                _correoEnvioCH = value
            End Set
        End Property

        Public Property NumeroGuia As String
            Get
                Return _numeroGuia
            End Get
            Set(value As String)
                _numeroGuia = value
            End Set
        End Property

        Public Property IdTransportadora As Integer
            Get
                Return _idTransportadora
            End Get
            Set(value As Integer)
                _idTransportadora = value
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

        Public Property ImeisDataTable() As DataTable
            Get
                Return _imeisDataTable
            End Get
            Set(ByVal value As DataTable)
                _imeisDataTable = value
            End Set
        End Property

        Public Property tablaDetalleArchivo() As DataTable
            Get
                Return _dtDetalleArchivo
            End Get
            Set(value As DataTable)
                _dtDetalleArchivo = value
            End Set
        End Property

        Public Property Reagenda() As Boolean
            Get
                Return _reagenda
            End Get
            Set(value As Boolean)
                _reagenda = value
            End Set
        End Property


        Public Property FechaIni() As Date
            Get
                Return _fechaIni
            End Get
            Set(value As Date)
                _fechaIni = value
            End Set
        End Property

        Public Property fechaFin() As Date
            Get
                Return _fechaFin
            End Get
            Set(value As Date)
                _fechaFin = value
            End Set
        End Property

        Public Property DetalleBloqueoInventario() As BloqueoInventario
            Get
                Return _detalleBloqueoInventario
            End Get
            Set(ByVal value As BloqueoInventario)
                _detalleBloqueoInventario = value
            End Set
        End Property

        Public Property ReferenciasColeccion() As DetalleMaterialServicioMensajeriaColeccion
            Get
                If _referenciasColeccion Is Nothing Then _referenciasColeccion = New DetalleMaterialServicioMensajeriaColeccion(_idServicioMensajeria)
                Return _referenciasColeccion
            End Get
            Set(ByVal value As DetalleMaterialServicioMensajeriaColeccion)
                _referenciasColeccion = value
            End Set
        End Property

        Public Overridable Property MinsColeccion() As DetalleMsisdnEnServicioMensajeriaColeccion
            Get
                If _minsColeccion Is Nothing Then _minsColeccion = New DetalleMsisdnEnServicioMensajeriaColeccion(_idServicioMensajeria)
                Return _minsColeccion
            End Get
            Set(ByVal value As DetalleMsisdnEnServicioMensajeriaColeccion)
                _minsColeccion = value
            End Set
        End Property

        Public Property TablaNovedad() As DataTable
            Get
                If _tablaNovedad Is Nothing Then _tablaNovedad = New DataTable
                Return _tablaNovedad
            End Get
            Set(ByVal value As DataTable)
                _tablaNovedad = value
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

        Public Property FechaAgendaEntrega() As Date
            Get
                Return _fechaAgendaEntrega
            End Get
            Set(ByVal value As Date)
                _fechaAgendaEntrega = value
            End Set
        End Property

        Public Property CodOficinaCliene As String
            Get
                Return _codOficinaCliene
            End Get
            Set(ByVal value As String)
                _codOficinaCliene = value
            End Set
        End Property

        Public Property ProductoNoSerializado As String
            Get
                Return _productoNoSerializado
            End Get
            Set(ByVal value As String)
                _productoNoSerializado = value
            End Set
        End Property


        Public Property IdCampaniaList As ArrayList
            Get
                If _idCampaniaList Is Nothing Then _idCampaniaList = New ArrayList
                Return _idCampaniaList
            End Get
            Set(value As ArrayList)
                _idCampaniaList = value
            End Set
        End Property

        Public Property EstadoBanco As String
            Get
                Return _estadoBanco
            End Get
            Set(value As String)
                _estadoBanco = value
            End Set
        End Property

        Public Property EstadoConfrimacion As Integer
            Get
                Return _estadoConfrimacion
            End Get
            Set(value As Integer)
                _estadoConfrimacion = value
            End Set
        End Property


#End Region

#Region "Métodos Privados"

        Protected Overridable Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                    If _numeroRadicado > 0 Then .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = _numeroRadicado
                    .TiempoEsperaComando = 0
                    .ejecutarReader("ObtenerInformacionGeneralServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idServicioMensajeria").ToString(), _idServicioMensajeria)
                            Integer.TryParse(.Reader("idAgendamiento").ToString(), _idAgendamiento)
                            Integer.TryParse(.Reader("idBodega").ToString(), _idBodega)
                            Integer.TryParse(.Reader("idEmpresa").ToString(), _idEmpresa)
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
                            Long.TryParse(.Reader("numeroRadicado").ToString(), _numeroRadicado)
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
                            Integer.TryParse(.Reader("idServicioTipo"), _idServicioTipo)
                            Boolean.TryParse(.Reader("reagenda"), _reagenda)
                            _actividadLaboral = .Reader("actividadLaboral").ToString
                            _Campania = .Reader("nombreCampania").ToString()
                            Integer.TryParse(.Reader("idCampania").ToString, _idCampania)
                            _numeroGuia = .Reader("numeroGuia").ToString
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If

                    'José Vélez Correa
                    'Enero 03, 2014
                    'El instanciamiento de las colecciones _referenciasColeccion y _minsColeccion se elimina de esta sección para 
                    'optimizar la inicialización de la clase, ya que no es correcto inicializar objetos de detalle, durante la 
                    'inicialización de la información de cabecera

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub

#End Region

#Region "Métodos Públicos"

        Public Overridable Function Registrar() As ResultadoProceso Implements IServicioMensajeria.Registrar
            Dim resultado As New ResultadoProceso
            Dim noResultadoServicio As Integer = -1
            Dim idServicioTipo As Integer

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
                        If _identificacionCliente <> String.Empty Then .Add("@identicacion", SqlDbType.VarChar).Value = _identificacionCliente
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
                        If _medioEnvioCH <> String.Empty Then .Add("@medioEnvioCH", SqlDbType.VarChar).Value = _medioEnvioCH
                        If _correoEnvioCH <> String.Empty Then .Add("@correoEnvioCH", SqlDbType.VarChar).Value = _correoEnvioCH

                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                        .Add("@idServicioMensajeria", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With

                    .iniciarTransaccion()
                    .TiempoEsperaComando = 0
                    'Registro en [ServicioMensajeria]
                    .ejecutarScalar("RegistraServicioMensajeria", CommandType.StoredProcedure)
                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultadoServicio)
                    Integer.TryParse(.SqlParametros("@idServicioMensajeria").Value.ToString(), _idServicioMensajeria)

                    If noResultadoServicio = 0 Then

                        If _idTipoServicio <> 0 Then
                            .SqlParametros.Clear()
                            .TiempoEsperaComando = 0
                            .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = IdServicioMensajeria
                            .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario

                            .SqlParametros.Add("@idServicioTipo", SqlDbType.Int).Direction = ParameterDirection.Output

                            'Registro en [ServicioMensajeriaTipoServicio]
                            .ejecutarScalar("RegistraTipoServicioMensajeria", CommandType.StoredProcedure)
                            Integer.TryParse(.SqlParametros("@idServicioTipo").Value.ToString(), idServicioTipo)

                            If idServicioTipo <> 0 Then

                                'Registro en [MaterialServicioTipoServicio]
                                If _referenciasDataTable IsNot Nothing AndAlso _referenciasDataTable.Rows.Count > 0 Then
                                    If _referenciasDataTable.Columns.Contains("idServicioTipo") Then _referenciasDataTable.Columns.Remove("idServicioTipo")
                                    If _referenciasDataTable.Columns.Contains("idUsuario") Then _referenciasDataTable.Columns.Remove("idUsuario")

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
                                    If _minsDataTable.Columns.Contains("idServicioTipo") Then _minsDataTable.Columns.Remove("idServicioTipo")

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
                                        .ColumnMappings.Add("numeroReserva", "numeroReserva")
                                        .ColumnMappings.Add("lista28", "lista28")
                                        .WriteToServer(_minsDataTable)
                                    End With
                                End If

                                'Registro en [DetalleSerialServicioMensajeria]
                                If _imeisDataTable IsNot Nothing AndAlso _imeisDataTable.Rows.Count > 0 Then
                                    Dim dcIdServicio As New DataColumn("idServicioMensajeria", GetType(Long), IdServicioMensajeria)
                                    Dim dcIdUsuarioRegistra As New DataColumn("IdUsuarioRegistra", GetType(Integer), _idUsuario)

                                    With _imeisDataTable.Columns
                                        .Add(dcIdServicio)
                                        .Add(dcIdUsuarioRegistra)
                                    End With

                                    .inicilizarBulkCopy()
                                    With .BulkCopy
                                        .DestinationTableName = "DetalleSerialServicioMensajeria"
                                        .ColumnMappings.Add("idServicioMensajeria", "idServicio")
                                        .ColumnMappings.Add("imei", "serial")
                                        .ColumnMappings.Add("prestamo", "requierePrestamoEquipo")
                                        .ColumnMappings.Add("msisdn", "msisdn")
                                        .ColumnMappings.Add("IdUsuarioRegistra", "idUsuarioRegistra")
                                        .WriteToServer(_imeisDataTable)
                                    End With

                                    'Se realiza la reserva del Inventario de prestamo
                                    If Me._detalleBloqueoInventario IsNot Nothing Then
                                        Dim resultadoBloqueo As ResultadoProceso = Me._detalleBloqueoInventario.Registrar()
                                        If resultadoBloqueo.Valor <> 0 Then
                                            resultado.EstablecerMensajeYValor(6, resultadoBloqueo.Mensaje)
                                            .abortarTransaccion()
                                        End If
                                    End If
                                End If

                                If _dtDetalleArchivo IsNot Nothing AndAlso _dtDetalleArchivo.Rows.Count > 0 Then

                                    Dim columnaPrecioTotalSinIva As New DataColumn("precioTotalSinIva", GetType(Double))
                                    columnaPrecioTotalSinIva.DefaultValue = 0
                                    _dtDetalleArchivo.Columns.Add(columnaPrecioTotalSinIva)

                                    Dim columnaPrecioTotalConIva As New DataColumn("precioTotalConIva", GetType(Double))
                                    columnaPrecioTotalConIva.DefaultValue = 0
                                    _dtDetalleArchivo.Columns.Add(columnaPrecioTotalConIva)

                                    Dim i As Integer
                                    i = 0
                                    For i = 0 To _dtDetalleArchivo.Rows.Count - 1
                                        _dtDetalleArchivo.Rows(i).Item("precioTotalSinIva") = _dtDetalleArchivo.Rows(i).Item("precioEquipoSinIva") + _dtDetalleArchivo.Rows(i).Item("precioSimCardSinIva")
                                        _dtDetalleArchivo.Rows(i).Item("precioTotalConIva") = _dtDetalleArchivo.Rows(i).Item("precioSimCardConIva") + _dtDetalleArchivo.Rows(i).Item("precioSimCardConIva")
                                    Next

                                    .inicilizarBulkCopy()
                                    With .BulkCopy
                                        .DestinationTableName = "AuxDetalleCEM"
                                        .ColumnMappings.Add("min", "min")
                                        .ColumnMappings.Add("clausula", "clausula")
                                        .ColumnMappings.Add("material", "material")
                                        .ColumnMappings.Add("referencia", "referencia")
                                        .ColumnMappings.Add("envioSim", "envioSim")
                                        .ColumnMappings.Add("materialSim", "materialSim")
                                        .ColumnMappings.Add("zonaSim", "zonaSim")
                                        .ColumnMappings.Add("activaEquipo", "activaEquipo")
                                        .ColumnMappings.Add("comseguro", "comseguro")
                                        .ColumnMappings.Add("precioEquipoSinIva", "precioEquipoSinIva")
                                        .ColumnMappings.Add("precioEquipoConIva", "precioEquipoConIva")
                                        .ColumnMappings.Add("precioSimCardSinIva", "precioSimCardSinIva")
                                        .ColumnMappings.Add("precioSimCardConIva", "precioSimCardConIva")
                                        .ColumnMappings.Add("precioTotalSinIva", "precioTotalSinIva")
                                        .ColumnMappings.Add("precioTotalConIva", "precioTotalConIva")
                                        .WriteToServer(_dtDetalleArchivo)
                                    End With

                                    .SqlParametros.Clear()
                                    .SqlParametros.Add("@idServicioTipo", SqlDbType.Int).Value = idServicioTipo
                                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                                    .SqlParametros.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output

                                    .ejecutarScalar("RegistrarDetalleCem", CommandType.StoredProcedure)

                                    Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                                    If resul = 0 Then
                                        resultado.EstablecerMensajeYValor(10, "Se generó un error al tratar de registrar el detalle del Servicio de Mensajeria.")
                                        .abortarTransaccion()
                                        Return resultado
                                        Exit Function
                                    End If

                                End If

                                '[Registrar Novedades]
                                If _tablaNovedad IsNot Nothing AndAlso _tablaNovedad.Rows.Count > 0 Then
                                    Dim dcAux As New DataColumn("idServicioMensajeria", GetType(Integer))
                                    dcAux.DefaultValue = IdServicioMensajeria
                                    _tablaNovedad.Columns.Add(dcAux)

                                    dcAux = New DataColumn("idUsuario")
                                    dcAux.DefaultValue = _idUsuario
                                    _tablaNovedad.Columns.Add(dcAux)

                                    dcAux = New DataColumn("numeroRadicado")
                                    dcAux.DefaultValue = _numeroRadicado
                                    _tablaNovedad.Columns.Add(dcAux)

                                    .inicilizarBulkCopy()
                                    With .BulkCopy
                                        .DestinationTableName = "NovedadServicioMensajeria"
                                        .ColumnMappings.Add("idServicioMensajeria", "idServicioMensajeria")
                                        .ColumnMappings.Add("idTipoNovedad", "idTipoNovedad")
                                        .ColumnMappings.Add("idUsuario", "idUsuario")
                                        .ColumnMappings.Add("observacion", "observacion")
                                        .ColumnMappings.Add("numeroRadicado", "numeroRadicado")
                                        .WriteToServer(_tablaNovedad)
                                    End With
                                End If

                                ' Se registran los materiales que no cuentan con disponibilidad de inventario
                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idServicioTipo", SqlDbType.BigInt).Value = idServicioTipo
                                .SqlParametros.Add("@idServicioMensajeria", SqlDbType.BigInt).Value = _idServicioMensajeria
                                .ejecutarNonQuery("VerificarDisponibilidadMaterial", CommandType.StoredProcedure)

                                .confirmarTransaccion()

                                'Se asocia la reserva al servicio
                                If Me._detalleBloqueoInventario IsNot Nothing AndAlso Me._detalleBloqueoInventario.IdBloqueo <> 0 Then
                                    Me.IdReserva = Me._detalleBloqueoInventario.IdBloqueo
                                    Me.Actualizar(_idUsuario)
                                End If
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

            Return resultado
        End Function

        Public Overridable Function Actualizar(ByVal idUsuarioLog As Integer) As ResultadoProceso Implements IServicioMensajeria.Actualizar
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
                        If _identificacionCliente <> String.Empty Then .Add("@identicacion", SqlDbType.VarChar).Value = _identificacionCliente
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
                        .Add("@disponibleAutomarcado", SqlDbType.Bit).Value = _disponibleAutomarcado
                        If _idEstado = Enumerados.EstadoServicio.RecibidoST And Not _fechaAgenda.Equals(Date.MinValue) Then .Add("@fechaAgendaEntrega", SqlDbType.DateTime).Value = _fechaAgenda
                        If _medioEnvioCH <> String.Empty Then .Add("@medioEnvioCH", SqlDbType.VarChar).Value = _medioEnvioCH
                        If _correoEnvioCH <> String.Empty Then .Add("@correoEnvioCH", SqlDbType.VarChar).Value = _correoEnvioCH

                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With

                    .IniciarTransaccion()
                    .TiempoEsperaComando = 0
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
        Public Overridable Function AnularServicioVenta(ByVal idUsuarioLog As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim idResultado As Integer = -1
            Dim idServicioTipo As Integer = -1

            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                        .Add("@idUsuarioLog", SqlDbType.Int).Value = idUsuarioLog
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With
                    .ejecutarScalar("AnularServicioVenta", CommandType.StoredProcedure)
                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), idResultado)
                    If idResultado = 0 Then
                        resultado.EstablecerMensajeYValor(0, "Transacción exitosa.")
                    Else
                        resultado.EstablecerMensajeYValor(7, "Se generó un error al tratar de actualizar.")
                    End If
                Catch ex As Exception

                    Throw New Exception(ex.Message, ex)
                End Try
            End With
            dbManager.Dispose()

            Return resultado
        End Function
        Public Overridable Function Confirmar() As ResultadoProceso
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
                        If _medioEnvioCH <> String.Empty Then .SqlParametros.Add("@medioEnvioCH", SqlDbType.VarChar).Value = _medioEnvioCH
                        If _correoEnvioCH <> String.Empty Then .SqlParametros.Add("@correoEnvioCH", SqlDbType.VarChar).Value = _correoEnvioCH

                        .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ConfirmarServicioMensajeria", CommandType.StoredProcedure)
                        If Not IsDBNull(.SqlParametros("@result").Value) Then
                            resultado.Valor = CShort(.SqlParametros("@result").Value)
                            If resultado.Valor = 0 Then
                                resultado.Mensaje = "El servicio fue confirmado de manera exitosa."
                                .confirmarTransaccion()
                            Else
                                Select Case resultado.Valor
                                    Case 1
                                        resultado.Mensaje = "Ocurrió un error inesperado al confirmar el servicio. Por favor intente nuevamente"
                                    Case 2
                                        resultado.Mensaje = "No se pudo realizar la reserva de inventario para atender el servicio. Por favor intente nuevamente"
                                    Case 3
                                        resultado.Mensaje = "Uno o más materiales del servicio ya no tienen disponibilidad de inventario."
                                    Case 4
                                        resultado.Mensaje = "No hay disponibilidad de entregas para agendar este servicio"
                                End Select


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

        Public Function LeerSerial(ByVal serial As String, ByVal idUsuario As Integer, Optional ByVal validaRegion As Boolean = False) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idServicioMensajeria > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = serial
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        If validaRegion Then .SqlParametros.Add("@validaRegion", SqlDbType.Bit).Value = validaRegion
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
                                        resultado.Mensaje = "El serial no existe en el inventario de bodegas satélites."
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

        Public Function LeerSerialServicioFinanciero(ByVal serial As String, ByVal idUsuario As Integer, ByVal codigo As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .Add("@serial", SqlDbType.VarChar, 50).Value = serial
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@codigo", SqlDbType.VarChar, 50).Value = codigo
                        If Not String.IsNullOrEmpty(_productoNoSerializado) Then
                            .Add("@productoNoSerializado", SqlDbType.VarChar, 20).Value = _productoNoSerializado
                        End If
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    End With
                    .TiempoEsperaComando = 0
                    .IniciarTransaccion()
                    .EjecutarNonQuery("LeerSerialServicioFinanciero", CommandType.StoredProcedure)
                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        .ConfirmarTransaccion()
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(500, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente.")
                    End If

                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se presento un error al registrar el serial: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function RecibirSerialCierre(ByVal serial As String, Optional ByVal idEstado As Integer = 0) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idServicioMensajeria > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                            .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = serial
                            If idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.VarChar, 50).Value = idEstado
                            .SqlParametros.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .IniciarTransaccion()
                            .EjecutarNonQuery("RecibirSerialEnServicioMensajeriaCerrado", CommandType.StoredProcedure)

                            If Not IsDBNull(.SqlParametros("@result").Value) Then
                                resultado.Valor = CShort(.SqlParametros("@result").Value)
                                If resultado.Valor = 0 Then
                                    resultado.Mensaje = "El serial fue registrado satisfactoriamente."
                                    .ConfirmarTransaccion()
                                Else
                                    Select Case resultado.Valor
                                        Case 1
                                            resultado.Mensaje = "El serial no se encuentra asociado al servicio."
                                        Case 2
                                            resultado.Mensaje = "El serial no se encuentra en estado Pendiente Reintegro a Bodega."
                                        Case 3
                                            resultado.Mensaje = "Por favor seleccione un estado del serial."
                                        Case Else
                                            resultado.Mensaje = "Ocurrió un error inesperado al confirmar el servicio. Por favor intente nuevamente."
                                    End Select
                                    .AbortarTransaccion()
                                End If
                            Else
                                Throw New Exception("Ocurrió un error interno al registrar serial. Por favor intente nuevamente")
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para realizar la confirmación. ")
            End If

            Return resultado
        End Function

        Public Function ObtenerDatosCruceRealceVsFisico() As DataTable
            Dim dt As New DataTable
            Using dbManager As New LMDataAccess
                With dbManager

                    .SqlParametros.Clear()
                    If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                    If _idBase <> 0 Then .SqlParametros.Add("@idBase", SqlDbType.Int).Value = _idBase
                    If _fechaIni <> Date.MinValue Then .SqlParametros.Add("@fechaIni", SqlDbType.Date).Value = _fechaIni
                    If _fechaFin <> Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.Date).Value = _fechaFin

                    dt = .EjecutarDataTable("ObtenerCruceRealceVsFisico", CommandType.StoredProcedure)
                End With
            End Using
            Return dt
        End Function

        Public Function CerrarDespacho() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Not (_idServicioMensajeria = 0 OrElse _idUsuarioCierre = 0) Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioCierre
                        If Not String.IsNullOrEmpty(_numeroGuia) Then .SqlParametros.Add("@numeroGuia", SqlDbType.VarChar).Value = _numeroGuia
                        If _idTransportadora > 0 Then .SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = _idTransportadora

                        .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("CerrarDespachoServicioMensajeria", CommandType.StoredProcedure)
                        If Not IsDBNull(.SqlParametros("@result").Value) Then
                            resultado.Valor = CShort(.SqlParametros("@result").Value)
                            If resultado.Valor = 0 Then
                                resultado.Mensaje = "El despacho fue cerrado de manera exitosa."
                                .ConfirmarTransaccion()
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

        Public Function FinalizarCambioServicio(ByVal observacion As String, idUsuario As Integer, Optional ByVal numeroContrato As Long = 0) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                    .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = observacion
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    If numeroContrato > 0 Then .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = numeroContrato
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

        Public Overridable Function ConfirmarEntrega()
            Dim resultado As New ResultadoProceso

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
                            .SqlParametros.Add("@mensajeResp", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .IniciarTransaccion()
                            .ejecutarNonQuery("CancelarServicioMensajeriaDespachado", CommandType.StoredProcedure)
                            If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                                resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                                If resultado.Valor = 0 Then
                                    resultado.Mensaje = CStr(.SqlParametros("@mensajeResp").Value)
                                    .ConfirmarTransaccion()
                                Else
                                    If resultado.Valor = 1 Then
                                        resultado.Mensaje = "El servicio proporcionado no existe. Por favor verifique"
                                    Else
                                        resultado.Mensaje = "Ocurrió un error inesperado al tratar de cancelar servicio. Por favor intente nuevamente"
                                    End If
                                    .AbortarTransaccion()
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

        Public Function ConfirmarCierre() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idServicioMensajeria > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .iniciarTransaccion()
                            .ejecutarNonQuery("ConfirmarCierreServicioMensajeria", CommandType.StoredProcedure)
                            If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                                resultado.Valor = CInt(.SqlParametros("@resultado").Value)
                                If resultado.Valor = 0 Then
                                    resultado.Mensaje = "Se finalizó el cierre del servicio exitosamente."
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
                        End With
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(10, "No se han propocionado todos los datos requeridos para realizar el Cierre.")
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
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                    If _idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.SmallInt).Value = _idTipoServicio
                    If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                    If _idEmpresa > 0 Then .SqlParametros.Add("@idEmpresa", SqlDbType.Int).Value = _idEmpresa
                    If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
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
            Return resultado
        End Function

        Public Function RegistrarCupoEntrega() As ResultadoProceso
            Dim resultado As New ResultadoProceso()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                        .SqlParametros.Add("@fechaAgenda", SqlDbType.Date).Value = _fechaAgenda
                        If _idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.SmallInt).Value = _idTipoServicio
                        .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("ReservarCupoEntregaCEM", CommandType.StoredProcedure)
                        Dim respuesta As Integer = .SqlParametros("@return").Value
                        If respuesta = 0 Then
                            resultado.EstablecerMensajeYValor(respuesta, "Reserva generada de forma exitosa.")
                        Else
                            resultado.EstablecerMensajeYValor(respuesta, "Se generó un error inesperado al realizar la reserva: [" & respuesta & "]")
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Function LiberarCupoEntrega() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
                        .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                        .SqlParametros.Add("@fechaAgenda", SqlDbType.Date).Value = _fechaAgenda
                        .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("LiberarCupoEntregaCEM", CommandType.StoredProcedure)
                        Dim respuesta As Integer = .SqlParametros("@return").Value
                        If respuesta = 0 Then
                            resultado.EstablecerMensajeYValor(respuesta, "Liberación generada de forma exitosa. ")
                        Else
                            resultado.EstablecerMensajeYValor(respuesta, "Se generó un error inesperado al realizar la liberación: [" & respuesta & "]")
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Protected Overridable Sub ConsultarInfoDocumentosLegalizados()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria

                    .ejecutarReader("ObtenerInformacionGeneralServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idServicioMensajeria").ToString(), _idServicioMensajeria)
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
                            Long.TryParse(.Reader("numeroRadicado").ToString(), _numeroRadicado)
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
                            Integer.TryParse(.Reader("idServicioTipo"), _idServicioTipo)
                            Boolean.TryParse(.Reader("reagenda"), _reagenda)
                            _actividadLaboral = .Reader("actividadLaboral").ToString
                            _Campania = .Reader("nombreCampania").ToString()
                            Integer.TryParse(.Reader("idCampania").ToString, _idCampania)
                            _registrado = True
                        End If
                        .Reader.Close()
                    End If

                    'José Vélez Correa
                    'Enero 03, 2014
                    'El instanciamiento de las colecciones _referenciasColeccion y _minsColeccion se elimina de esta sección para 
                    'optimizar la inicialización de la clase, ya que no es correcto inicializar objetos de detalle, durante la 
                    'inicialización de la información de cabecera

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub

        Public Function DevolucionMTI() As ResultadoProceso
            Dim res As New ResultadoProceso()
            Dim dt As New DataTable
            Try
                Dim db As New LMDataAccess
                db.SqlParametros.Clear()
                db.SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                dt = db.EjecutarDataTable("DevolucionMTIServicioMensajeria", CommandType.StoredProcedure)
                If dt.Rows.Count < 1 Then
                    res.EstablecerMensajeYValor(0, "Estado cambiado satisfactoriamente")
                Else
                    res.EstablecerMensajeYValor(dt.Rows(0).Item("Error"), dt.Rows(0).Item("mensaje"))
                End If
            Catch ex As Exception

            End Try
            Return res
        End Function

        Public Function ConsultaRemarcadoPrecense() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager

                    If _numeroRadicado <> 0 Then .SqlParametros.Add("@radicado", SqlDbType.Decimal, 20).Value = _numeroRadicado
                    If _estadoBanco IsNot Nothing AndAlso _estadoBanco > 0 Then .SqlParametros.Add("@estadoBanco", SqlDbType.VarChar, 20).Value = _estadoBanco
                    If _estadoConfrimacion <> 0 Then .SqlParametros.Add("@EstadoConfrimacion", SqlDbType.Int).Value = _estadoConfrimacion
                    If _idEmpresa <> 0 Then .SqlParametros.Add("@empresa", SqlDbType.Int).Value = _idEmpresa
                    If _idCampaniaList IsNot Nothing AndAlso _idCampaniaList.Count > 0 Then .SqlParametros.Add("@listaCampanias", SqlDbType.VarChar).Value = Join(_idCampaniaList.ToArray, ",")
                    If _fechaIni.Date > Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.Date).Value = _fechaIni
                    If _fechaFin.Date > Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.Date).Value = _fechaFin
                    dtDatos = .EjecutarDataTable("ConsultaRemarcadoPrecense", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ExisteNumeroRadicado(ByVal numeroRadicado As Long, Optional ByVal validaServicio As Boolean = False) As Boolean
            Dim dbManager As New LMDataAccess
            Dim resultado As Boolean
            Try
                With dbManager
                    .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = numeroRadicado
                    If validaServicio Then .SqlParametros.Add("@validaServicio", SqlDbType.Bit).Value = validaServicio
                    .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("ExisteNumeroRadicado", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@result").Value) Then
                        resultado = CBool(.SqlParametros("@result").Value)
                    Else
                        Throw New Exception("Imposible validar el número de radicado proporcionado. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Shared Function ObtieneIdServicioTipo(ByVal idServicioMensajeria As Integer) As Integer
            Dim dbManager As New LMDataAccess
            Dim resultado As Integer

            With dbManager
                .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = idServicioMensajeria

                resultado = .ejecutarScalar("ObtieneIdServicio", CommandType.StoredProcedure)

                If resultado = -1 Then
                    Throw New Exception("Imposible obtener el idServicioTipo.")
                End If
            End With
            dbManager.Dispose()
            Return resultado
        End Function

        Public Shared Function AutorizaMisdnRepetido(ByVal idUsuario As Integer, ByVal nRadicado1 As Long, ByVal nRadicado2 As Long) As ResultadoProceso
            Dim resultado As New ResultadoProceso

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@radicado1", SqlDbType.BigInt).Value = nRadicado1
                        .SqlParametros.Add("@radicado2", SqlDbType.BigInt).Value = nRadicado2
                        .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .ejecutarNonQuery("AutorizarMSISDNRepetido", CommandType.StoredProcedure)
                        resultado.Valor = .SqlParametros("@return").Value
                        Select Case resultado.Valor
                            Case 0
                                resultado.Mensaje = "Autorización realizada exitosamente."
                            Case 1
                                resultado.Mensaje = "El radicado: " & nRadicado1 & " no se encontró registrado en el sistema."
                            Case 2
                                resultado.Mensaje = "El radicado: " & nRadicado2 & " no se encontró registrado en el sistema."
                            Case 3
                                resultado.Mensaje = "Se encontró que los radicados ingresados se encuentran en estado Cancelado."
                            Case 4
                                resultado.Mensaje = "Error inesperado al intentar autorizar."
                            Case 5
                                resultado.Mensaje = "La autorización ya se encuentra registrada."
                            Case 6
                                resultado.Mensaje = "Existe autorización activa relacionada al radicado: " & nRadicado1
                        End Select

                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Function ActualizarReagenda(ByVal idUsuarioLog As Integer, ByVal objServicio As ServicioMensajeria) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim idResultado As Integer = -1
            Dim idServicioTipo As Integer = -1

            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        .Add("@idServicioMensajeria", SqlDbType.Int).Value = objServicio.IdServicioMensajeria
                        .Add("@idUsuarioLog", SqlDbType.Int).Value = idUsuarioLog
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With

                    .iniciarTransaccion()
                    .ejecutarScalar("ActualizaServicioMensajeriaReagenda", CommandType.StoredProcedure)
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

        Public Overloads Shared Function ConfirmarRecoleccion(ByVal idUsuario As Integer, ByVal numRadicado As Long, ByVal cantidadRecibida As Integer) As ResultadoProceso
            Dim respuesta As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = numRadicado
                        .SqlParametros.Add("@cantidadRecibida", SqlDbType.Int).Value = cantidadRecibida
                        .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .EjecutarNonQuery("ConfirmarRecoleccionServicio", CommandType.StoredProcedure)
                        If Integer.TryParse(.SqlParametros("@return").Value, respuesta.Valor) Then
                            Select Case respuesta.Valor
                                Case 0
                                    respuesta.Mensaje = "[Confirmación Exitosa]"
                                Case -1
                                    respuesta.Mensaje = "[Error no controlado]"
                                Case 1
                                    respuesta.Mensaje = "[No existe el número de radicado ingresado]"
                                Case 2
                                    respuesta.Mensaje = "[El radicado ingresado no es de tipo Servicio Técnico]"
                                Case 3
                                    respuesta.Mensaje = "[La cantidad recibida es mayor a la cantidad de equipos registrados]"
                                Case 4
                                    respuesta.Mensaje = "[El radicado ingresado ya se encuentra confirmado como recogido]"
                            End Select
                        Else
                            respuesta.EstablecerMensajeYValor(-1, "No se logro obtener respuesta del servidor, por favor intente nuevamente.")
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return respuesta
        End Function

#End Region

    End Class

End Namespace