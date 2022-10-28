Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes
Imports ILSBusinessLayer.MensajeriaEspecializada.OfficeTrack

Namespace MensajeriaEspecializada

    Public Class RutaServicioMensajeria

#Region "Atributos"

        Protected _idRuta As Integer
        Protected _idResponsableEntrega As Integer
        Protected _idEstado As Enumerados.RutaMensajeria
        Protected _nombreEstado As String
        Protected _fechaCreacion As Date
        Protected _fechaSalida As Date
        Protected _fechaCierre As Date
        Protected _idUsuarioLog As Integer
        Protected _fechaModificacion As Date
        Protected _identificacionResponsable As String
        Protected _nombreResponsable As String
        Protected _tipoRuta As Enumerados.TipoRutaServicioMensajeria
        Protected _idProveedorServicioTecnico As Integer

        Protected _serviciosDatatable As DataTable
        Private _registrado As Boolean

        Protected Shared infoEstados As InfoEstadoRestriccionCEM

#End Region

#Region "Propiedades"

        Shared ReadOnly synLock As Object = New Object

        Private Shared ReadOnly Property LogFilePath() As String
            Get
                Return AppDomain.CurrentDomain.BaseDirectory + "\LogOfficeTrack.txt"
            End Get
        End Property

        Public Shared Sub logServicio(mensaje As String)

            SyncLock synLock

                Dim texto As String = Convert.ToString(DateTime.Now.ToString() + ": ") & mensaje
                Dim sw As New System.IO.StreamWriter(LogFilePath, True)
                sw.WriteLine(texto)
                sw.Close()

            End SyncLock

        End Sub

        Public Property IdRuta() As Integer
            Get
                Return _idRuta
            End Get
            Set(ByVal value As Integer)
                _idRuta = value
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

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As Date)
                _fechaCreacion = value
            End Set
        End Property

        Public Property FechaSalida() As Date
            Get
                Return _fechaSalida
            End Get
            Set(ByVal value As Date)
                _fechaSalida = value
            End Set
        End Property

        Public Property FechaCierre()
            Get
                Return _fechaCierre
            End Get
            Set(ByVal value)
                _fechaCierre = value
            End Set
        End Property

        Public Property IdUsuarioLog() As Integer
            Get
                Return _idUsuarioLog
            End Get
            Set(ByVal value As Integer)
                _idUsuarioLog = value
            End Set
        End Property

        Public Property FechaModificacion() As Date
            Get
                Return _fechaModificacion
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaModificacion = value
            End Set
        End Property

        Public Property IdentificacionResponsable() As String
            Get
                Return _identificacionResponsable
            End Get
            Protected Friend Set(ByVal value As String)
                _identificacionResponsable = value
            End Set
        End Property

        Public Property NombreResponsable() As String
            Get
                Return _nombreResponsable
            End Get
            Protected Friend Set(ByVal value As String)
                _nombreResponsable = value
            End Set
        End Property

        Public Property NombreEstado() As String
            Get
                Return _nombreEstado
            End Get
            Set(ByVal value As String)
                _nombreEstado = value
            End Set
        End Property

        Public Property TipoRuta() As Enumerados.TipoRutaServicioMensajeria
            Get
                Return _tipoRuta
            End Get
            Set(ByVal value As Enumerados.TipoRutaServicioMensajeria)
                _tipoRuta = value
            End Set
        End Property

        Public Property IdProveedorServicioTecnico() As Integer
            Get
                Return _idProveedorServicioTecnico
            End Get
            Set(ByVal value As Integer)
                _idProveedorServicioTecnico = value
            End Set
        End Property


        Public Property ServiciosDatatable() As DataTable
            Get
                Return _serviciosDatatable
            End Get
            Set(ByVal value As DataTable)
                _serviciosDatatable = value
            End Set
        End Property

        Public ReadOnly Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idRuta As Integer)
            MyBase.New()
            _idRuta = idRuta
            CargarDatos()

        End Sub

#End Region

#Region "Métodos"

        Private Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = _idRuta
                        .ejecutarReader("ObtenerRutaServicioMensajeria", CommandType.StoredProcedure)

                        If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                            If .Reader.Read Then
                                If Not IsDBNull(.Reader("idResponsableEntrega")) Then _idResponsableEntrega = CInt(.Reader("idResponsableEntrega").ToString)
                                _idEstado = CInt(.Reader("idEstado"))
                                _fechaCreacion = CDate(.Reader("fechaCreacion"))
                                If Not IsDBNull(.Reader("fechaSalida")) Then Date.TryParse(.Reader("fechaSalida"), _fechaSalida)
                                If Not IsDBNull(.Reader("fechaCierre")) Then Date.TryParse(.Reader("fechaCierre"), _fechaCierre)
                                Integer.TryParse(.Reader("idUsuarioLog").ToString, _idUsuarioLog)
                                Date.TryParse(.Reader("fechaModificacion"), _fechaModificacion)
                                _identificacionResponsable = (.Reader("identificacionResponsable").ToString().Trim())
                                _nombreResponsable = (.Reader("nombreResponsable").ToString())
                                _nombreEstado = .Reader("nombreEstado").ToString()
                                _tipoRuta = .Reader("idTipoRuta")

                                _registrado = True
                            End If
                            .Reader.Close()
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

#End Region

#Region "Funciones"
        Public Function RegitrarOfficeTrackIdRuta(ByVal IdRuta As Long) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                With dbManager
                    '-------------OfficeTrack-------------
                    Dim EnviadoOfficeTrack As Boolean = False
                    Dim MensajeErrorOfficeTrack As String = ""
                    Dim idDetalle As Integer
                    Dim ConfigurationID As Integer = ConfigurationManager.AppSettings.Item("idDatabase")
                    Dim userName As String = ConfigurationManager.AppSettings.Item("userName")
                    Dim password As String = ConfigurationManager.AppSettings.Item("password")
                    Try

                        .SqlParametros.Clear()
                        Dim objOfficeTrack As New ConfiguracionOfficeTrack

                        Dim dtresul As DataTable = objOfficeTrack.CargarIdDetalleRutaRetrasmicion(IdRuta, dbManager)
                        If dtresul.Rows.Count = 0 Then
                            resultado.EstablecerMensajeYValor(10, "No se encontraron registros para sincronizar")
                            Return resultado
                            Exit Function
                        End If
                        For Each Detalle As DataRow In dtresul.Rows

                            idDetalle = Integer.Parse(Detalle("idDetalle"))
                            Dim pidDetalle As String = idDetalle
                            .SqlParametros.Clear()
                            With objOfficeTrack
                                objOfficeTrack.CargarConfigOfficeTrack(pidDetalle, userName, password, ConfigurationID, dbManager)
                            End With
                        Next
                        resultado.EstablecerMensajeYValor(0, "Se realizo la transmicion de forma correcta")
                        EnviadoOfficeTrack = True
                    Catch ex As Exception
                        resultado.EstablecerMensajeYValor(100, ex.Message)
                        .SqlParametros.Clear()
                        Dim TaskNumber As String = (idDetalle.ToString() & ConfigurationID.ToString())
                        .SqlParametros.Add("@TaskNumber", SqlDbType.BigInt).Value = Int64.Parse(TaskNumber)
                        .EjecutarNonQuery("ActualizarOfficeTrackTaskNoEnviado", CommandType.StoredProcedure)

                        logServicio("   IdDetalle:" + idDetalle.ToString())
                        logServicio("   ConfigurationID:" + ConfigurationID.ToString())
                        logServicio("   Message: " + ex.Message)
                        logServicio("   StackTrace: " + ex.StackTrace)

                        EnviadoOfficeTrack = False
                        MensajeErrorOfficeTrack = ex.Message
                    End Try
                    '-------------Fin OfficeTrack-------------
                End With
            End Using
            Return resultado
        End Function

        Public Overridable Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim noResultado As Integer = -1
            Dim idRutaServicio As Integer

            Using dbManager As New LMDataAccess
                With dbManager
                    Try
                        With .SqlParametros
                            .Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega
                            .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            If _fechaCreacion <> Date.MinValue Then .Add("@fechaCreacion", SqlDbType.DateTime).Value = _fechaCreacion
                            If _fechaSalida <> Date.MinValue Then .Add("@fechaSalida", SqlDbType.DateTime).Value = _fechaSalida
                            If _fechaCierre <> Date.MinValue Then .Add("@fechaCierre", SqlDbType.DateTime).Value = _fechaCierre
                            .Add("@idUsuarioLog", SqlDbType.Int).Value = _idUsuarioLog
                            .Add("@idTipoRuta", SqlDbType.Int).Value = _tipoRuta
                            If _idProveedorServicioTecnico > 0 Then .Add("@idProveedorServicioTecnico", SqlDbType.Int).Value = _idProveedorServicioTecnico

                            .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@idRutaServicio", SqlDbType.Int).Direction = ParameterDirection.Output
                        End With

                        .IniciarTransaccion()

                        'Registro en [RutaServicio]
                        .EjecutarNonQuery("RegistrarRutaServicioMensajeria", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)
                        Integer.TryParse(.SqlParametros("@idRutaServicio").Value.ToString(), idRutaServicio)

                        If idRutaServicio > 0 Then
                            If _serviciosDatatable IsNot Nothing AndAlso _serviciosDatatable.Rows.Count > 0 Then
                                Dim columnaIdRuta As New DataColumn("idRuta", GetType(Integer))
                                columnaIdRuta.DefaultValue = idRutaServicio
                                _serviciosDatatable.Columns.Add(columnaIdRuta)

                                Dim columnaidUsuario As New DataColumn("idUsuarioLog", GetType(Integer))
                                columnaidUsuario.DefaultValue = _idUsuarioLog
                                _serviciosDatatable.Columns.Add(columnaidUsuario)

                                .InicilizarBulkCopy()

                                If _tipoRuta = Enumerados.TipoRutaServicioMensajeria.EntregaCliente Or _
                                    _tipoRuta = Enumerados.TipoRutaServicioMensajeria.RecoleccionCliente Or _
                                    _tipoRuta = Enumerados.TipoRutaServicioMensajeria.EntregaProveedorServicioTecnico Or _
                                    _tipoRuta = Enumerados.TipoRutaServicioMensajeria.EntregaClienteServicioTecnico Then
                                    With .BulkCopy
                                        .DestinationTableName = "DetalleRutaServicioMensajeria"
                                        .ColumnMappings.Add("idRuta", "idRuta")
                                        .ColumnMappings.Add("idServicio", "idServicio")
                                        .ColumnMappings.Add("idUsuarioLog", "idUsuarioLog")
                                        .ColumnMappings.Add("secuencia", "secuencia")
                                        .WriteToServer(_serviciosDatatable)
                                    End With

                                ElseIf _tipoRuta = Enumerados.TipoRutaServicioMensajeria.RecoleccionProveedorServicioTecnico Or _
                                    _tipoRuta = Enumerados.TipoRutaServicioMensajeria.RecoleccionClienteSiembra Then
                                    With .BulkCopy
                                        .DestinationTableName = "DetalleDespachoServicioMensajeria"
                                        .ColumnMappings.Add("idRuta", "idRuta")
                                        .ColumnMappings.Add("idDetalleSerial", "idDetalleSerial")
                                        .ColumnMappings.Add("idUsuarioLog", "idUsuarioLog")
                                        .WriteToServer(_serviciosDatatable)
                                    End With
                                End If

                                'Se realiza el cambio de estado de los Servicios
                                If _serviciosDatatable.Columns.Contains("idServicio") Then
                                    Dim idServicioTemp As Integer = CInt(_serviciosDatatable(0).Item("idServicio"))
                                    infoEstados = New InfoEstadoRestriccionCEM(New ServicioMensajeria(idServicio:=idServicioTemp).IdTipoServicio, _
                                                               Enumerados.ProcesoMensajeria.Despacho, _
                                                               Enumerados.ProcesoMensajeria.Enrutamiento, 0)
                                End If

                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = idRutaServicio
                                If infoEstados IsNot Nothing Then .SqlParametros.Add("@idEstadoServicio", SqlDbType.Int).Value = infoEstados.IdEstadoSiguiente 'Enumerados.EstadoServicio.AsignadoRuta
                                .SqlParametros.Add("@idEstadoRuta", SqlDbType.Int).Value = Enumerados.RutaMensajeria.Reparto
                                If (_idUsuarioLog > 0) Then
                                    .SqlParametros.Add("@idUsuarioLog", SqlDbType.Int).Value = _idUsuarioLog
                                End If

                                .EjecutarNonQuery("ActualizaEstadoServiciosRuta", CommandType.StoredProcedure)

                                .ConfirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, idRutaServicio.ToString() & "-Transacción exitosa.")
                            Else
                                resultado.EstablecerMensajeYValor(2, "Imposible registrar ruta sin servicios asociados.")
                                .AbortarTransaccion()
                            End If
                        Else
                            resultado.EstablecerMensajeYValor(1, "Imposible crear el registro en la tabla Ruta Servicio.")
                            .AbortarTransaccion()
                        End If
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw ex
                    End Try
                End With
            End Using

            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim noResultado As Integer = -1

            Using dbManager As New LMDataAccess
                With dbManager
                    Try
                        dbManager.TiempoEsperaComando = 0
                        With .SqlParametros
                            .Add("@idRuta", SqlDbType.Int).Value = _idRuta
                            If _idResponsableEntrega > 0 Then .Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega
                            If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            If _fechaCreacion <> Date.MinValue Then .Add("@fechaCreacion", SqlDbType.DateTime).Value = _fechaCreacion
                            If _fechaSalida <> Date.MinValue Then .Add("@fechaSalida", SqlDbType.DateTime).Value = _fechaSalida
                            If _fechaCierre <> Date.MinValue Then .Add("@fechaCierre", SqlDbType.DateTime).Value = _fechaCierre
                            .Add("@idUsuarioLog", SqlDbType.Int).Value = _idUsuarioLog

                            .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                        End With

                        .IniciarTransaccion()

                        'Actualiza en [RutaServicio]
                        .EjecutarNonQuery("ActualizarRutaServicioMensajeria", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)

                        If noResultado = 0 Then
                            If _idEstado = Enumerados.RutaMensajeria.Reparto Then
                                'Se realiza el cambio de estado de los Servicios
                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = _idRuta
                                If _tipoRuta <> Enumerados.TipoRutaServicioMensajeria.RecoleccionClienteSiembra Then .SqlParametros.Add("@idEstadoServicio", SqlDbType.Int).Value = Enumerados.EstadoServicio.Transito
                                .SqlParametros.Add("@idEstadoRuta", SqlDbType.Int).Value = _idEstado
                                If (_idUsuarioLog > 0) Then
                                    .SqlParametros.Add("@idUsuarioLog", SqlDbType.Int).Value = _idUsuarioLog
                                End If
                                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output

                                .EjecutarNonQuery("ActualizaEstadoServiciosRuta", CommandType.StoredProcedure)
                                Integer.TryParse(.SqlParametros("@resultado").Value, noResultado)
                                If noResultado <> 0 Then
                                    .AbortarTransaccion()
                                    resultado.EstablecerMensajeYValor(1, "No se logro realizar la actualización del estado de los Servicios.")
                                Else
                                    .ConfirmarTransaccion()
                                    Dim DetalleRuta As New DetalleRutaServicioMensajeria
                                    DetalleRuta.IdRuta = _idRuta
                                    Dim dt As DataTable = DetalleRuta.ObtenerDatosEstado(Enumerados.RutaMensajeria.Reparto)
                                    'If dt.Rows.Count > 0 Then
                                    '    Dim miServicio As New ServicioMensajeria(idServicio:=CInt(CInt(dt.Rows(0)("idServicio").ToString)))
                                    '    If miServicio.IdTipoServicio = Enumerados.TipoServicio.ServiciosFinancierosDavivienda Then
                                    '        resultado = ActualizarGestionVenta(New NotusExpressDaviviendaService.WsGestionVenta, miServicio.IdServicioMensajeria, Enumerados.EstadoServicio.Transito, "Servicio modificado desde CEM")
                                    '    ElseIf miServicio.IdTipoServicio = Enumerados.TipoServicio.DaviviendaSamsung Then
                                    '        resultado = ActualizarGestionVenta(New ServicioNotusExpressDaviviendaSamsung, miServicio.IdServicioMensajeria, Enumerados.EstadoServicio.Transito, "Servicio modificado desde CEM")
                                    '    End If
                                    'End If
                                    resultado.EstablecerMensajeYValor(0, "Actualización exitosa.")
                                End If

                            ElseIf _idEstado = Enumerados.RutaMensajeria.Cerrado Then
                                'Se realiza el cambio de estado de los Servicios
                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idRuta", SqlDbType.Int).Value = _idRuta
                                .SqlParametros.Add("@idEstadoServicio", SqlDbType.Int).Value = Enumerados.EstadoServicio.Transito
                                .SqlParametros.Add("@idEstadoRuta", SqlDbType.Int).Value = _idEstado
                                If (_idUsuarioLog > 0) Then
                                    .SqlParametros.Add("@idUsuarioLog", SqlDbType.Int).Value = _idUsuarioLog
                                End If
                                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output

                                .EjecutarNonQuery("ActualizaEstadoServiciosRuta", CommandType.StoredProcedure)
                                Integer.TryParse(.SqlParametros("@resultado").Value, noResultado)
                                If noResultado <> 0 Then
                                    .AbortarTransaccion()
                                    Select Case noResultado
                                        Case 1
                                            resultado.EstablecerMensajeYValor(1, "Existen radicados con estado Devolución, y no tienen novedades asociadas.")
                                        Case 2
                                            resultado.EstablecerMensajeYValor(2, "Existen radicados con estado Tránsito.")
                                    End Select
                                Else
                                    .ConfirmarTransaccion()
                                    resultado.EstablecerMensajeYValor(0, "Actualización exitosa.")
                                End If
                            Else
                                .ConfirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, "Actualización exitosa.")
                            End If
                        Else
                            .AbortarTransaccion()
                            Select Case noResultado
                                Case 1
                                    resultado.EstablecerMensajeYValor(1, "El número de ruta no se encuentra registrado en el sistema.")
                                Case 2
                                    resultado.EstablecerMensajeYValor(2, "El cambio de estado no es válido. Para poder registrar salida debe estar creada la ruta. Para poder registrar llegada debe estar en Reparto.")
                            End Select
                        End If
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw ex
                    End Try
                End With
            End Using

            Return resultado
        End Function

        Public Shared Function ActualizarGestionVenta(ByVal servicioNotusExpress As IServicioNotusExpress,
                                           ByVal idServicio As Integer,
                                           ByVal idEstado As Integer,
                                           Optional ByVal justificacion As String = "Servicio modificado desde CEM, por el usuario: Admin") As ResultadoProceso
            Return servicioNotusExpress.ActualizarGestionVenta(idServicio, idEstado, justificacion)
        End Function


#End Region

#Region "Funciones Compartidas"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroRutaMensajeria
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroRutaMensajeria) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdRuta > 0 Then db.SqlParametros.Add("@idRuta", SqlDbType.BigInt).Value = .IdRuta
                If .IdResponsableEntrega > 0 Then db.SqlParametros.Add("@idResponsable", SqlDbType.Int).Value = .IdResponsableEntrega
                If .FechaCreacionInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaCreacionInicial", SqlDbType.SmallDateTime).Value = .FechaCreacionInicial
                If .FechaCreacionFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaCreacionFinal", SqlDbType.SmallDateTime).Value = .FechaCreacionFinal
                If .IdJornada > 0 Then db.SqlParametros.Add("@idJornada", SqlDbType.SmallInt).Value = .IdJornada
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                If .ListaEstado IsNot Nothing AndAlso .ListaEstado.Count > 0 Then db.SqlParametros.Add("@listaEstado", SqlDbType.VarChar).Value = Join(.ListaEstado.ToArray, ",")
                If .IdCiudad > 0 Then db.SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = .IdCiudad
                dtDatos = db.EjecutarDataTable("ObtenerListadoRutaMensajeria", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

        Public Overloads Shared Function ObtenerListadoDetalle(ByVal filtro As FiltroRutaMensajeria) As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                Try
                    With filtro
                        If .IdRuta > 0 Then dbManager.SqlParametros.Add("@idRuta", SqlDbType.BigInt).Value = .IdRuta
                        If .IdResponsableEntrega > 0 Then dbManager.SqlParametros.Add("@idResponsable", SqlDbType.Int).Value = .IdResponsableEntrega
                        If .FechaCreacionInicial <> Date.MinValue Then dbManager.SqlParametros.Add("@fechaCreacionInicial", SqlDbType.SmallDateTime).Value = .FechaCreacionInicial
                        If .FechaCreacionFinal <> Date.MinValue Then dbManager.SqlParametros.Add("@fechaCreacionFinal", SqlDbType.SmallDateTime).Value = .FechaCreacionFinal
                        If .FechaAgendaInicial <> Date.MinValue Then dbManager.SqlParametros.Add("@fechaAgendaInicial", SqlDbType.SmallDateTime).Value = .FechaAgendaInicial
                        If .FechaAgendaFinal <> Date.MinValue Then dbManager.SqlParametros.Add("@fechaAgendaFinal", SqlDbType.SmallDateTime).Value = .FechaAgendaFinal
                        If .IdJornada > 0 Then dbManager.SqlParametros.Add("@idJornada", SqlDbType.SmallInt).Value = .IdJornada
                        If .IdEstado > 0 Then dbManager.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                        If .ListaEstado IsNot Nothing AndAlso .ListaEstado.Count > 0 Then dbManager.SqlParametros.Add("@listaEstado", SqlDbType.VarChar).Value = Join(.ListaEstado.ToArray, ",")
                        If .IdCiudad > 0 Then dbManager.SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = .IdCiudad

                        dtDatos = dbManager.EjecutarDataTable("ObtenerDetalleListadoRutaMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using

            Return dtDatos
        End Function

        Public Shared Function ObtenerRadicadosPorId(idRuta As Long) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            db.SqlParametros.Add("@idRuta", SqlDbType.BigInt).Value = idRuta
            dtDatos = db.EjecutarDataTable("ObtenerInfoHojaRutaMensajeria", CommandType.StoredProcedure)
            Return dtDatos

        End Function

        Public Shared Function ObtenerTiposServicioEnRuta(ByVal idRuta As Long) As List(Of Integer)
            Dim listDatos As New List(Of Integer)
            Dim dtDatos As DataTable
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idRuta", SqlDbType.BigInt).Value = idRuta
                        dtDatos = .EjecutarDataTable("ObtenerDetalleListadoRutaMensajeria", CommandType.StoredProcedure)
                    End With
                    If dtDatos.Rows.Count > 0 Then
                        For Each fila As DataRow In dtDatos.Rows
                            If Not listDatos.Contains(CInt(fila("idTipoServicio"))) Then
                                listDatos.Add(CInt(fila("idTipoServicio")))
                            End If
                        Next
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return listDatos
        End Function

#End Region

#Region "Estructuras"

        Public Structure FiltroRutaMensajeria
            Dim IdRuta As Integer
            Dim IdResponsableEntrega As Integer
            Dim IdEstado As Integer
            Dim ListaEstado As ArrayList
            Dim FechaCreacionInicial As Date
            Dim FechaCreacionFinal As Date
            Dim FechaAgendaInicial As Date
            Dim FechaAgendaFinal As Date
            Dim IdUsuarioLog As Integer
            Dim IdJornada As Integer
            Dim IdCiudad As Integer
            Dim idTipoRuta As Short
        End Structure

#End Region

    End Class

End Namespace

