Imports ILSBusinessLayer.MensajeriaEspecializada
Imports LMDataAccessLayer
Imports System.Text

Namespace MensajeriaEspecializada

    Public Class ServicioMensajeriaSiembra
        Inherits ServicioMensajeria
        Implements IServicioMensajeria

#Region "Atributos"

        Private _nombreRepresentanteLegal As String
        Private _identificacionRepresentanteLegal As String
        Private _telefonoRepresentanteLegal As String
        Private _identificacionAutorizado As String
        Private _cargoAutorizado As String
        Private _telefonoAutorizado As String
        Private _clienteClaro As Boolean
        Private _idGerencia As Integer
        Private _nombreGerencia As String
        Private _idCoordinador As Integer
        Private _nombreCoordinador As String
        Private _idConsultor As Integer
        Private _nombreConsultor As String
        Private _emailConsultor As String
        Private _emailCoordinador As String
        Private _direccionEdicion As String
        Private _observacionDireccion As String

#End Region

#Region "Propiedades"

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

#End Region

#Region "Construtores"

        Public Sub New()
            MyBase.New()
            _idTipoServicio = Enumerados.TipoServicio.Siembra
        End Sub

        Public Sub New(ByVal idServicio As Long)
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
                        If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.BigInt).Value = _idServicioMensajeria
                        .ejecutarReader("ObtenerInfoGeneralServicioSiembra", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                CargarResultadoConsulta(.Reader)
                            End If
                            .Reader.Close()
                        End If

                        _referenciasColeccion = New DetalleMaterialServicioMensajeriaColeccion(_idServicioMensajeria)
                        _minsColeccion = New DetalleMsisdnEnServicioMensajeriaColeccion(_idServicioMensajeria)

                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Sub

        Public Sub CargarDatosRadicado(ByVal NumeroRadicado As Long)
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If NumeroRadicado > 0 Then .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = NumeroRadicado
                        .ejecutarReader("ObtenerInfoGeneralServicio", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                CargarResultadoConsulta(.Reader)
                            End If
                            .Reader.Close()
                            _referenciasColeccion = New DetalleMaterialServicioMensajeriaColeccion(_idServicioMensajeria)
                            _minsColeccion = New DetalleMsisdnEnServicioMensajeriaColeccion(_idServicioMensajeria)
                        End If

                       

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
            Dim noResultadoServicio As Integer = -1
            Dim idServicioTipo As Integer

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                        If Not _fechaRegistro.Equals(Date.MinValue) Then .SqlParametros.Add("@fechaRegistro", SqlDbType.DateTime).Value = _fechaRegistro
                        If Not _fechaAsignacion.Equals(Date.MinValue) Then .SqlParametros.Add("@fechaAsignacion", SqlDbType.DateTime).Value = _fechaAsignacion
                        .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        If _nombreCliente <> String.Empty Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombreCliente
                        If _identificacionCliente <> String.Empty Then .SqlParametros.Add("@identicacion", SqlDbType.VarChar).Value = _identificacionCliente
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

                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Direction = ParameterDirection.Output

                        .iniciarTransaccion()

                        'Registro en [ServicioMensajeria]
                        .ejecutarScalar("RegistraServicioMensajeria", CommandType.StoredProcedure)
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
                                .ejecutarScalar("RegistraTipoServicioMensajeria", CommandType.StoredProcedure)
                                Integer.TryParse(.SqlParametros("@idServicioTipo").Value.ToString(), idServicioTipo)

                                If idServicioTipo <> 0 Then
                                    'Registro en MINs 
                                    If _minsDataTable IsNot Nothing AndAlso _minsDataTable.Rows.Count > 0 Then
                                        If _minsDataTable.Columns.Contains("idServicioTipo") Then _minsDataTable.Columns.Remove("idServicioTipo")
                                        _minsDataTable.Columns.Add(New DataColumn("idServicioTipo", GetType(Integer), idServicioTipo))

                                        For Each min As DataRow In _minsDataTable.Rows

                                            Dim objMsisdn As New DetalleMsisdnEnServicioMensajeriaTipoSiembra()
                                            With objMsisdn
                                                .IdTipoServicio = CInt(min("idServicioTipo"))
                                                .MSISDN = min("msisdn")
                                                If Not IsDBNull(min("idPlan")) Then .IdPlan = min("idPlan")
                                                If Not IsDBNull(min("idPaquete")) Then .IdPaquete = min("idPaquete")
                                                If Not IsDBNull(min("idRegion")) Then .IdRegion = min("idRegion")

                                                .FechaDevolucion = CDate(min("fechaDevolucion"))
                                                resultado = .Adicionar(dbManager)
                                            End With

                                            'Se realiza el registro de los Materiales asociados al MSISDN
                                            Dim dvMin As DataView = _minsDataTable.DefaultView
                                            dvMin.RowFilter = "msisdn=" + min("msisdn")

                                            Dim dtMinsTemp As DataTable = dvMin.ToTable(True, "idServicioTipo", "material", "materialSim")
                                            With dtMinsTemp
                                                If .Columns.Contains("idMsisdn") Then dtMinsTemp.Columns.Remove("idMsisdn")
                                                If .Columns.Contains("cantidad") Then dtMinsTemp.Columns.Remove("cantidad")
                                                If .Columns.Contains("idUsuario") Then dtMinsTemp.Columns.Remove("idUsuario")

                                                .Columns.Add(New DataColumn("idMsisdn", GetType(Integer), objMsisdn.IdRegistro))
                                                .Columns.Add(New DataColumn("cantidad", GetType(Integer), 1))
                                                .Columns.Add(New DataColumn("idUsuario", GetType(Integer), _idUsuario))

                                                If Not IsDBNull(dtMinsTemp.Rows(0).Item("material")) And Not IsDBNull(dtMinsTemp.Rows(0).Item("materialSim")) Then
                                                    'Llega Equipo y SIM
                                                    Dim dtNuevo As DataTable = dtMinsTemp.Copy()
                                                    dtNuevo.Rows(0).Item("material") = dtMinsTemp.Rows(0).Item("materialSim")
                                                    dtMinsTemp.Merge(dtNuevo)

                                                ElseIf IsDBNull(dtMinsTemp.Rows(0).Item("material")) And Not IsDBNull(dtMinsTemp.Rows(0).Item("materialSim")) Then
                                                    'Llega Sim Solamente
                                                    dtMinsTemp.Rows(0).Item("material") = dtMinsTemp.Rows(0).Item("materialSim")
                                                End If
                                            End With


                                            dbManager.inicilizarBulkCopy()
                                            With dbManager.BulkCopy
                                                .DestinationTableName = "MaterialServicioTipoServicio"
                                                .ColumnMappings.Add("idServicioTipo", "idServicioTipo")
                                                .ColumnMappings.Add("material", "material")
                                                .ColumnMappings.Add("cantidad", "cantidad")
                                                .ColumnMappings.Add("idUsuario", "idUsuario")
                                                .ColumnMappings.Add("idMsisdn", "idMsisdn")
                                                .WriteToServer(dtMinsTemp)
                                            End With
                                        Next
                                   

                                    'Se realiza la reserva del Inventario
                                    If Me._detalleBloqueoInventario IsNot Nothing AndAlso Me._detalleBloqueoInventario.ProductoBloqueoColeccion.Count > 0 Then
                                        Dim resultadoBloqueo As ResultadoProceso = Me._detalleBloqueoInventario.Registrar()
                                        If resultadoBloqueo.Valor <> 0 Then
                                            Dim sbMensaje As New StringBuilder

                                            For Each itemError As DataRow In Me.DetalleBloqueoInventario.Errores.Rows
                                                sbMensaje.AppendLine(itemError("mensaje").ToString())
                                            Next

                                            resultado.EstablecerMensajeYValor(6, sbMensaje.ToString())
                                            .abortarTransaccion()
                                        End If
                                    End If
                                    Else
                                        resultado.EstablecerMensajeYValor(9, "No se pudieron establecer materiales para el servicio")
                                        .abortarTransaccion()
                                    End If

                                    If .estadoTransaccional Then
                                        .confirmarTransaccion()
                                        resultado.EstablecerMensajeYValor(0, "Transacción exitosa.")

                                        'Se asocia la reserva al servicio
                                        If Me._detalleBloqueoInventario IsNot Nothing AndAlso Me._detalleBloqueoInventario.IdBloqueo <> 0 Then
                                            Me.IdReserva = Me._detalleBloqueoInventario.IdBloqueo
                                            Me.Actualizar(_idUsuario)
                                        End If
                                    End If
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
                    End With
                Catch ex As Exception
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
                            .Clear()
                            .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                            .Add("@idUsuarioLog", SqlDbType.Int).Value = idUsuarioLog

                            If _idUsuario > 0 Then .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                            If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                            If _nombreCliente <> String.Empty Then .Add("@nombre", SqlDbType.VarChar).Value = _nombreCliente
                            If _identificacionCliente <> String.Empty Then .Add("@identicacion", SqlDbType.VarChar).Value = _identificacionCliente
                            If _telefonoContacto <> String.Empty Then .Add("@telefono", SqlDbType.VarChar).Value = _telefonoContacto
                            If _extensionContacto <> String.Empty Then .Add("@extension", SqlDbType.VarChar).Value = _extensionContacto
                            If _idReserva > 0 Then .Add("@idReserva", SqlDbType.Int).Value = _idReserva
                            If Not String.IsNullOrEmpty(_nombreRepresentanteLegal) Then .Add("@nombreRepresentante", SqlDbType.VarChar).Value = _nombreRepresentanteLegal
                            If Not String.IsNullOrEmpty(_identificacionRepresentanteLegal) Then .Add("@identificacionRepresentante", SqlDbType.VarChar).Value = _identificacionRepresentanteLegal
                            If Not String.IsNullOrEmpty(_telefonoRepresentanteLegal) Then .Add("@telefonoRepresentante", SqlDbType.VarChar).Value = _telefonoRepresentanteLegal
                            If _personaContacto <> String.Empty Then .Add("@nombreAutorizado", SqlDbType.VarChar).Value = _personaContacto
                            If Not String.IsNullOrEmpty(_identificacionAutorizado) Then .Add("@identificacionAutorizado", SqlDbType.VarChar).Value = _identificacionAutorizado
                            If Not String.IsNullOrEmpty(_cargoAutorizado) Then .Add("@cargoAutorizado", SqlDbType.VarChar).Value = _cargoAutorizado
                            If Not String.IsNullOrEmpty(_telefonoAutorizado) Then .Add("@telefonoAutorizado", SqlDbType.VarChar).Value = _telefonoAutorizado
                            If _direccion <> String.Empty Then .Add("@direccion", SqlDbType.VarChar).Value = _direccion
                            If Not String.IsNullOrEmpty(_direccionEdicion) Then .Add("@direccionEdicion", SqlDbType.VarChar).Value = _direccionEdicion
                            If Not String.IsNullOrEmpty(_observacionDireccion) Then .Add("@observacionDireccion", SqlDbType.VarChar).Value = _observacionDireccion
                            If _barrio <> String.Empty Then .Add("@barrio", SqlDbType.VarChar).Value = _barrio
                            If _idGerencia > 0 Then .Add("@idGerenciaCliente", SqlDbType.Int).Value = _idGerencia
                            If _idCoordinador > 0 Then .Add("@idCoordinador", SqlDbType.Int).Value = _idCoordinador
                            If _idConsultor > 0 Then .Add("@idConsultor", SqlDbType.Int).Value = _idConsultor
                            .Add("@clienteClaro", SqlDbType.Bit).Value = _clienteClaro
                            If _observacion <> String.Empty Then .Add("@observacion", SqlDbType.VarChar).Value = _observacion
                            If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                            If Not _fechaCierre.Equals(Date.MinValue) Then .Add("@fechaCierre", SqlDbType.DateTime).Value = _fechaCierre
                            If _idUsuarioCierre > 0 Then .Add("@idUsuarioCierre", SqlDbType.Int).Value = IdUsuarioCierre

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
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Overrides Function Confirmar() As ResultadoProceso
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

                        .iniciarTransaccion()

                        .ejecutarNonQuery("ConfirmarServicioMensajeriaTipoSiembra", CommandType.StoredProcedure)
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

        Public Overloads Function LeerSerial(ByVal serial As String, ByVal idUsuario As Integer, Optional ByVal validaRegion As Boolean = False, _
                                            Optional ByVal msisdn As String = Nothing) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idServicioMensajeria > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = serial
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        If validaRegion Then .SqlParametros.Add("@validaRegion", SqlDbType.Bit).Value = validaRegion
                        If msisdn IsNot Nothing Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar).Value = msisdn
                        .SqlParametros.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarSerialEnServicioMensajeriaSiembra", CommandType.StoredProcedure)

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
                                    Case 7
                                        resultado.Mensaje = "El material asociado al serial no pertenece al MSISDN seleccionado."
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

        Public Overloads Function ConfirmarEntrega()
            Dim resultado As New ResultadoProceso

            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = _idServicioMensajeria
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@idZona", SqlDbType.Int).Value = _idZona
                    .SqlParametros.Add("@idResponsableEntrega", SqlDbType.Int).Value = _idResponsableEntrega
                    .SqlParametros.Add("@tipoServicio", SqlDbType.Int).Value = Enumerados.TipoServicio.Siembra

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

        Public Function MesaControl(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                If _idServicioMensajeria > 0 Then
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@idUsuarioMesaControl", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarCambioEstadoMesaControl", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@respuesta").Value, resultado.Valor)

                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                            resultado.Mensaje = "Se realizó el cambio de estado del servicio exitosamente."
                        Else
                            .abortarTransaccion()
                            resultado.Mensaje = "No se logró realizar el cambio de estado del servicio: [" & resultado.Valor & "]"
                        End If
                    End With
                End If
            End Using
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
                        .ejecutarNonQuery("RegistrarLegalizacionSiembra", CommandType.StoredProcedure)
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

        Public Function RecibirSerial(ByVal serial As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idServicioMensajeria > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                            .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = serial
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario

                            .SqlParametros.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .iniciarTransaccion()
                            .ejecutarNonQuery("RecibirSerialSiembra", CommandType.StoredProcedure)

                            If Not IsDBNull(.SqlParametros("@result").Value) Then
                                resultado.Valor = CShort(.SqlParametros("@result").Value)
                                If resultado.Valor = 0 Then
                                    resultado.Mensaje = "El serial fue recibido satisfactoriamente."
                                    .confirmarTransaccion()
                                ElseIf resultado.Valor = 10 Then
                                    resultado.Mensaje = "Se realizó la recepción total de los Seriales. El Servicio se cerró satisfactoriamente."
                                    .confirmarTransaccion()
                                Else
                                    Select Case resultado.Valor
                                        Case 1
                                            resultado.Mensaje = "El serial no existe en el inventario de bodegas satélites."
                                        Case 2
                                            resultado.Mensaje = "El serial no está asignado al servicio seleccionado."
                                        Case 3
                                            resultado.Mensaje = "El serial no se encuentra en estado disponible para recepción."
                                        Case Else
                                            resultado.Mensaje = "Ocurrió un error inesperado al confirmar el servicio. Por favor intente nuevamente."

                                    End Select
                                    .abortarTransaccion()
                                End If
                            Else
                                Throw New Exception("Ocurrió un error interno al recibir serial. Por favor intente nuevamente")
                            End If
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        Throw ex
                    End Try
                End Using
            Else
                resultado.EstablecerMensajeYValor(1, "No se han propocionado todos los datos requeridos para realizar la recepción del serial.")
            End If
            Return resultado
        End Function

        Public Function InformacionMSISDNMateriales() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        dtDatos = .ejecutarDataTable("InfoMaterialMSISDNSiembra", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

        Public Function InformacionRutas() As DataTable
            Dim dtDatos As New DataTable
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        dtDatos = .ejecutarDataTable("ObtieneRutasServicioMensajeria", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtDatos
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idServicioMensajeria").ToString(), _idServicioMensajeria)
                    Integer.TryParse(reader("idEstado").ToString(), _idEstado)
                    Integer.TryParse(reader("idAgendamiento").ToString(), _idAgendamiento)
                    Integer.TryParse(reader("idBodega").ToString(), _idBodega)
                    Integer.TryParse(reader("idCiudad").ToString(), _idCiudad)
                    _nombreCiudad = reader("nombreCiudad").ToString()
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
                    If reader("idGerenciaCliente").ToString() IsNot Nothing And reader("idGerenciaCliente").ToString() <> "0" Then
                        Integer.TryParse(reader("idGerenciaCliente").ToString, _idGerencia)
                    End If
                    _nombreGerencia = reader("nombreGerencia").ToString
                    Integer.TryParse(reader("idPersonaCoordinador").ToString, _idCoordinador)
                    _nombreCoordinador = reader("nombreCoordinador").ToString
                    _emailCoordinador = reader("emailCoordinador").ToString
                    Integer.TryParse(reader("idPersonaConsultor").ToString, _idConsultor)
                    _nombreConsultor = reader("nombreConsultor").ToString
                    If Not IsDBNull(reader("emailConsultor")) Then _emailConsultor = reader("emailConsultor").ToString
                    Date.TryParse(reader("fechaRegistro").ToString(), _fechaRegistro)
                    _observacion = reader("observacion").ToString()
                    If Not IsDBNull(reader("fechaAgenda")) Then _fechaAgenda = CDate(reader("fechaAgenda"))
                    Integer.TryParse(reader("idJornada").ToString(), _idJornada)
                    _jornada = reader("jornada").ToString()
                    Integer.TryParse(reader("idTipoServicio").ToString(), _idTipoServicio)
                    If Not IsDBNull(reader("clienteClaro")) Then _clienteClaro = CBool(reader("clienteClaro"))
                    _registrado = True
                End If
            End If

        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ConfirmarRecoleccion(ByVal idUsuario As Integer, ByVal numOrdenRecoleccion As Long, ByVal cantidadRecibida As Integer) As ResultadoProceso
            Dim respuesta As New ResultadoProceso
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@numOrdenRecoleccion", SqlDbType.BigInt).Value = numOrdenRecoleccion
                        .SqlParametros.Add("@cantidadRecibida", SqlDbType.Int).Value = cantidadRecibida
                        .SqlParametros.Add("@return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("ConfirmarRecoleccionServicioSiembra", CommandType.StoredProcedure)
                        If Integer.TryParse(.SqlParametros("@return").Value, respuesta.Valor) Then
                            Select Case respuesta.Valor
                                Case 0
                                    respuesta.Mensaje = "[Confirmación Exitosa]"
                                Case -1
                                    respuesta.Mensaje = "[Error no controlado]"
                                Case 1
                                    respuesta.Mensaje = "[No existe el número de la orden de recolección]"
                                Case 2
                                    respuesta.Mensaje = "[La Orden de Recolección no es de tipo Recolección Cliente Siembra]"
                                Case 3
                                    respuesta.Mensaje = "[Cantidad recibida mayor a la cantidad de equipos registrados]"
                                Case 4
                                    respuesta.Mensaje = "[La Orden de Recolección no se encuentra en estado de tránsito]"
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
