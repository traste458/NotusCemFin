Imports ILSBusinessLayer.Pedidos
Imports ILSBusinessLayer.Productos
Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes
Imports ILSBusinessLayer.Estructuras
Imports System.Net.Mail
Imports ILSBusinessLayer
Imports LMWebServiceSyncMonitorBusinessLayer

Namespace Despachos
    Public Class Despacho

        Public Structure TipoMaterial
            Dim material As String
            Dim idTipoProducto As Integer
            Dim nombreTablaLectura As String
        End Structure

#Region "Atributos"

        Protected _idDespacho As Integer
        Protected _pedido As Pedido
        Protected _idAuxiliarAtiende As Integer
        Protected _idAuxiliarCierra As Integer
        Protected _fechaCreacion As Date
        Protected _fechaCierre As Date
        Protected _transportadora As Transportadora
        Protected _guia As String
        Protected _estado As Estado
        Protected _peso As Double
        Protected _tipoDespacho As TipoDespacho
        Protected _tipoTransporte As TipoTransporte
        Protected _valorDeclarado As Double
        Protected _detalle As ArrayList
        Protected _detalleSerial As ArrayList
        Protected _cantidadPedidaTotal As Integer
        Protected _cantidadCajas As Integer
        Protected _tipoUnidad As UnidadEmpaque
        Protected _volumen As Double
        Protected _idTipoPedido As Integer
        Protected _contadorIntentoConteoSAP As Integer
        Protected _ciudadDestino As String
        Protected _regionDestino As String
        Protected _codigoCliente As String
        Protected _numDocumentoConteoSAP As String

        'Atributos de contabilización sap
        Protected _resultadoConteoSAP As SAPZmmCapser.OutputLgCapSer
        Protected _infoEntregaSAP() As SAPZmmCapser.ZmmLmEntregas
        Protected _infoSerialesSAP() As SAPZmmCapser.ZmmLmSeriales
        Protected _dtMensajesContabilizacion As DataTable
        Protected _listaPrecintos As String
        Protected _infoErroresSAP As DataTable ' Errores de SAP
        Protected _infoSerialesDespachoSAP As DataTable 'Seriales de despacho
        Protected _contenido As String
        Protected _resultado As New ResultadoProceso

#End Region

#Region "Propiedades"

        Public Property ListaPrecintos() As String
            Get
                Return _listaPrecintos
            End Get
            Set(ByVal value As String)
                _listaPrecintos = value
            End Set
        End Property

        Public ReadOnly Property ListaMensajesContabilizacion() As DataTable
            Get
                Return _dtMensajesContabilizacion
            End Get
        End Property

        Public ReadOnly Property NumDocumentoConteoSAP() As Integer
            Get
                Return _numDocumentoConteoSAP
            End Get
        End Property

        Public ReadOnly Property IdDespacho() As Integer
            Get
                Return _idDespacho
            End Get
        End Property

        Public Property Pedido() As Pedido
            Get
                Return _pedido
            End Get
            Set(ByVal value As Pedido)
                _pedido = value
            End Set
        End Property

        Public Property IdAuxiliarAtiende() As Integer
            Get
                Return _idAuxiliarAtiende
            End Get
            Set(ByVal value As Integer)
                _idAuxiliarAtiende = value
            End Set
        End Property

        Public Property IdAuxiliarCierra() As Integer
            Get
                Return _idAuxiliarCierra
            End Get
            Set(ByVal value As Integer)
                _idAuxiliarCierra = value
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

        Public Property FechaCierre() As Date
            Get
                Return _fechaCierre
            End Get
            Set(ByVal value As Date)
                _fechaCierre = value
            End Set
        End Property

        Public Property Transportadora() As Transportadora
            Get
                Return _transportadora
            End Get
            Set(ByVal value As Transportadora)
                _transportadora = value
            End Set
        End Property

        Public Property Guia() As String
            Get
                Return _guia
            End Get
            Set(ByVal value As String)
                _guia = value
            End Set
        End Property

        Public Property Estado() As Estado
            Get
                Return _estado
            End Get
            Set(ByVal value As Estado)
                _estado = value
            End Set
        End Property

        Public Property Peso() As Double
            Get
                Return _peso
            End Get
            Set(ByVal value As Double)
                _peso = value
            End Set
        End Property

        Public Property TipoDespacho() As TipoDespacho
            Get
                Return _tipoDespacho
            End Get
            Set(ByVal value As TipoDespacho)
                _tipoDespacho = value
            End Set
        End Property

        Public Property TipoTransporte() As TipoTransporte
            Get
                Return _tipoTransporte
            End Get
            Set(ByVal value As TipoTransporte)
                _tipoTransporte = value
            End Set
        End Property

        Public Property ValorDeclarado() As Double
            Get
                Return _valorDeclarado
            End Get
            Set(ByVal value As Double)
                _valorDeclarado = value
            End Set
        End Property

        Public Property Detalle() As ArrayList
            Get
                Return _detalle
            End Get
            Set(ByVal value As ArrayList)
                _detalle = value
            End Set
        End Property

        Public Property CantidadPedidaTotal() As Integer
            Get
                Return _cantidadPedidaTotal
            End Get
            Set(ByVal value As Integer)
                _cantidadPedidaTotal = value
            End Set
        End Property

        Public Property CantidadCajas() As Integer
            Get
                Return _cantidadCajas
            End Get
            Set(ByVal value As Integer)
                _cantidadCajas = value
            End Set
        End Property

        Public Property Volumen() As Double
            Get
                Return _volumen
            End Get
            Set(ByVal value As Double)
                _volumen = value
            End Set
        End Property

        Public Property IdTipoPedido() As Integer
            Get
                Return _idTipoPedido
            End Get
            Set(ByVal value As Integer)
                _idTipoPedido = value
            End Set
        End Property

        Public Property TipoUnidad() As UnidadEmpaque
            Get
                Return _tipoUnidad
            End Get
            Set(ByVal value As UnidadEmpaque)
                _tipoUnidad = value
            End Set
        End Property


        Public Property CiudadDestino() As String
            Get
                Return _ciudadDestino
            End Get
            Set(ByVal value As String)
                _ciudadDestino = value
            End Set
        End Property

        Public Property RegionDestino() As String
            Get
                Return _regionDestino
            End Get
            Set(ByVal value As String)
                _regionDestino = value
            End Set
        End Property

        Public Property CodigoCliente() As String
            Get
                Return _codigoCliente
            End Get
            Set(ByVal value As String)
                _codigoCliente = value
            End Set
        End Property

        Public ReadOnly Property InfoErrores() As DataTable
            Get
                Return _infoErroresSAP
            End Get
        End Property

        Public Property InfoSerialesDespachoSAP() As DataTable
            Get
                Return _infoSerialesDespachoSAP
            End Get
            Set(ByVal value As DataTable)
                _infoSerialesDespachoSAP = value
            End Set
        End Property

        Public Property Contenido() As String
            Get
                Return _contenido
            End Get
            Set(ByVal value As String)
                _contenido = value
            End Set
        End Property

        Public ReadOnly Property Resultado() As ResultadoProceso
            Get
                Return _resultado
            End Get
        End Property
#End Region

#Region "Constructores"
        Public Sub New()
            _idDespacho = 0
            _pedido = New Pedido
            _idAuxiliarAtiende = 0
            _idAuxiliarCierra = 0
            _fechaCreacion = Date.Now
            _fechaCierre = Nothing
            _transportadora = New Transportadora
            _guia = ""
            _estado = New Estado
            _peso = 0
            _tipoDespacho = New TipoDespacho
            _tipoTransporte = New TipoTransporte
            _valorDeclarado = 0
            _detalle = New ArrayList
            _detalleSerial = New ArrayList
            _tipoUnidad = New UnidadEmpaque
            _cantidadPedidaTotal = 0
            _cantidadCajas = 0
            _volumen = 0
        End Sub

        Public Sub New(ByVal idDespacho As Integer)
            Me.New()
            Me.SeleccionarPorID(idDespacho)
        End Sub
#End Region

#Region "Métodos Públicos"

        ''' <summary>
        ''' Registra los datos del objeto instanciado en la base de datos
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RegistrarDespacho()
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Me.EstablecerParametrosCreacion(adminBD)

            Try
                adminBD.iniciarTransaccion()
                _idDespacho = adminBD.ejecutarScalar("CrearNuevoDespacho", CommandType.StoredProcedure)
                RegistrarDetalleDespacho(_pedido.IdPedido, adminBD)
                If _idTipoPedido <> 9 Then
                    CalcularValorDeclarado(adminBD)
                End If
                adminBD.confirmarTransaccion()
            Catch ex As Exception
                adminBD.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try

        End Sub

        ''' <summary>
        ''' Actualiza el estado del despacho específico en la base de datos y al objeto instanciado
        ''' </summary>
        ''' <param name="nuevoEstado">Nuevo identificador de estado que se desea asignar al despacho</param>
        ''' <remarks></remarks>
        Public Sub ActualizarEstado(ByVal nuevoEstado As Estado)
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
            adminBD.agregarParametroSQL("@idEstado", nuevoEstado.IdEstado)

            Try
                adminBD.ejecutarNonQuery("ActualizarEstadoDespacho", CommandType.StoredProcedure)
                Me._estado = nuevoEstado
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' Realiza todas las operaciones y actualizaciones necesarias para que el despacho quede cerrado 
        ''' </summary>
        ''' <remarks></remarks>
        Public Function CerrarDespacho(Optional ByVal adminBD As LMDataAccess = Nothing) As ResultadoProceso
            Dim asignarGuias As Boolean = False
            If adminBD Is Nothing Then
                adminBD = New LMDataAccessLayer.LMDataAccess
                asignarGuias = True
                adminBD.iniciarTransaccion()
            End If
            adminBD.TiempoEsperaComando = 1200
            Dim resultado As New ResultadoProceso
            Try
                If Me.Pedido.Tipo.IdTipo <> TipoPedido.Tipo.SalidaPedidoEspecial And Me.Pedido.Tipo.IdTipo <> TipoPedido.Tipo.PedidoServicioTecnico Then
                    '__ Esta opción se pasó hacia la contabilización del despacho SAP
                    If Me._transportadora.IdTransportadora = 0 Or Me._tipoTransporte.IdTipoTransporte = 0 Then
                        Throw New Exception("El despacho no puede cerrarse, pues no existe información de transporte registrada")
                    End If
                End If

                If (Me.Pedido.Tipo.IdTipo = TipoPedido.Tipo.SalidaDeVentas Or Me.Pedido.Tipo.IdTipo = TipoPedido.Tipo.SalidaDeTraslados) _
                And (Me.Pedido.NumeroEntrega = 0 Or Me.Pedido.NumeroPedido = 0) Then
                    'adminBD.abortarTransaccion()
                    resultado.Valor = 1
                    resultado.Mensaje = "El pedido a despachar no tiene número de entrega o número de pedido asignado."
                    Return resultado
                Else
                    If Not adminBD.estadoTransaccional Then adminBD.iniciarTransaccion()
                    If asignarGuias Then
                        If Me.Transportadora.UsaPrecintos Then Me.AsignarPrecintos(adminBD, _listaPrecintos)

                        If Me.Transportadora.UsaGuia Then Me.AsignarGuia(adminBD)
                    End If
                    adminBD.SqlParametros.Clear()
                    adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                    adminBD.agregarParametroSQL("@idAuxiliarCierra", Me._idAuxiliarCierra)
                    adminBD.agregarParametroSQL("@peso", Me._peso, SqlDbType.Float)
                    adminBD.agregarParametroSQL("@cantidadCajas", Me._cantidadCajas)
                    adminBD.agregarParametroSQL("@volumen", Me._volumen)
                    adminBD.agregarParametroSQL("@idTipoUnidad", Me._tipoUnidad.IdTipoUnidad)
                    adminBD.agregarParametroSQL("@numDocumentoConteoSAP", Me._numDocumentoConteoSAP)
                    adminBD.agregarParametroSQL("@contenido", Me._contenido)

                    adminBD.ejecutarNonQuery("CerrarDespacho", CommandType.StoredProcedure)

                    ' Si es despacho de pedido de cuarentena, marca los seriales como disponibles en SAP
                    If Me.Pedido.Tipo.IdTipo = TipoPedido.Tipo.DespachoCuarentena Then
                        resultado = LiberarSerialesEnSAP()
                        If resultado.Valor = 0 Then
                            adminBD.confirmarTransaccion()
                            resultado.EstablecerMensajeYValor(0, "Ejecución Satisfactoria")
                        Else
                            If adminBD.estadoTransaccional Then adminBD.abortarTransaccion()
                        End If
                    Else
                        If adminBD.estadoTransaccional Then adminBD.confirmarTransaccion()
                        resultado.Valor = 0
                    End If
                End If

                Return resultado

            Catch ex As Exception
                If adminBD.estadoTransaccional Then adminBD.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
        End Function

        Public Function LiberarSerialesEnSAP() As ResultadoProceso
            Dim cambioDeEstado As New CambioDeEstadoSAP
            Dim resultadoEjecucion As New ResultadoProceso

            resultadoEjecucion.Valor = 0
            resultadoEjecucion.Mensaje = "Ejecución Satisfactoria"

            If _idDespacho > 0 Then
                'Dim dtInfoSeriales As DataTable
                'dtInfoSeriales = DetalleSerial.Obtener(_idDespacho)

                If _infoSerialesDespachoSAP IsNot Nothing AndAlso _infoSerialesDespachoSAP.Rows.Count > 0 Then
                    With cambioDeEstado
                        .IdPedido = _idDespacho
                        .TipoCambio = CambioDeEstadoSAP.Tipo.Cuarentena
                        .InfoSeriales = _infoSerialesDespachoSAP
                        .ValeMaterial = "CAMBIO STOCK"
                        .TextoCabecera = "Despacho " & _idDespacho.ToString
                        .StockOrigen = CambioDeEstadoSAP.TipoStock.ControlCalidad
                        .StockDestino = CambioDeEstadoSAP.TipoStock.LibreUtilizacion
                        resultadoEjecucion = .GenerarCambio()
                        _infoErroresSAP = .InfoErrores
                    End With
                Else
                    resultadoEjecucion.Valor = 5
                    resultadoEjecucion.Mensaje = "No se pudieron obtener los seriales del despacho para cambiar el estado en SAP como liberados. "
                End If
            Else
                resultadoEjecucion.Valor = 4
                resultadoEjecucion.Mensaje = "No fue posible obtener el despacho para realizar el cambio de estado de los seriales en SAP. "
            End If

            Return resultadoEjecucion
        End Function

        ''' <summary>
        ''' Registra en la base de datos el detalle de lectura del despacho específico
        ''' </summary>
        ''' <param name="idPedido">Identificador del pedido asociado al despacho</param>
        ''' <param name="adminBD">Administrador de instancia actual de capa de datos</param>
        ''' <remarks></remarks>
        Public Sub RegistrarDetalleDespacho(ByVal idPedido As Integer, ByRef adminBD As LMDataAccessLayer.LMDataAccess)
            Dim datosDetallePedido As New DataTable
            Dim detalleTemporal As Despachos.Detalle

            adminBD.SqlParametros.Clear()
            datosDetallePedido = Me._pedido.Detalle

            For Each fila As DataRow In datosDetallePedido.Rows
                detalleTemporal = New Despachos.Detalle
                detalleTemporal.IdDespacho = Me._idDespacho
                detalleTemporal.Material = fila("Material")
                detalleTemporal.CantidadPedida = fila("Cantidad")
                Me._detalle.Add(detalleTemporal)

                'Registrar en la base de datos
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.agregarParametroSQL("@material", detalleTemporal.Material, SqlDbType.VarChar)
                adminBD.agregarParametroSQL("@cantidadPedida", detalleTemporal.CantidadPedida)
                adminBD.ejecutarNonQuery("CrearDetalleDespacho", CommandType.StoredProcedure)
                adminBD.SqlParametros.Clear()
            Next
        End Sub

        ''' <summary>
        ''' Asigna el tipo de despacho que se va a trabajar de acuerdo al tipo de mercancía que tenga el pedido
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EstablecerTipoDespacho(Optional ByVal idTipo As Integer = 1)
            ' UNDONE: Este método debe actualizarse tan pronto como se definan los parámetros de despachos de material no serializado
            Me.TipoDespacho = New TipoDespacho(idTipo)
        End Sub


        ''' <summary>
        ''' Selecciona un listado de todos los despachos que cumplen con las condiciones especificadas
        ''' </summary>
        ''' <param name="parametros">Arreglo de parámetros de filtrado: 
        ''' 0 - IdDespacho
        ''' 1 - IdPedido
        ''' 2 - IdAuxiliarAtiende
        ''' 3 - FechaCreacionInicial
        ''' 4 - FechaCreacionFinal
        ''' 5 - FechaCierreInicial
        ''' 6 - FechaCierreFinal
        ''' 7 - IdEstado</param>
        ''' <returns>Tabla de datos que contiene el listado de despachos que cumplen con los filtros</returns>
        ''' <remarks></remarks>
        Public Shared Function SeleccionarDespachos(ByVal parametros As FiltroDespacho) As DataTable
            Dim resultado As New DataTable
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try

                With parametros
                    If .NumeroEntrega <> 0 Then adminBD.agregarParametroSQL("@idDespacho", .NumeroEntrega, SqlDbType.BigInt)
                    If .NumeroPedido <> 0 Then adminBD.agregarParametroSQL("@idPedido", .NumeroPedido, SqlDbType.BigInt)
                    If .IdPedido <> 0 Then adminBD.agregarParametroSQL("@idPedidoLM", .IdPedido, SqlDbType.Int)
                    If .IdDespacho <> 0 Then adminBD.agregarParametroSQL("@idDespachoLM", .IdDespacho, SqlDbType.Int)
                    If .IdAuxiliarAtiende <> 0 Then adminBD.agregarParametroSQL("@idAuxiliarAtiende", .IdAuxiliarAtiende, SqlDbType.Int)
                    If .FechaCreacionInicial IsNot Nothing AndAlso .FechaCreacionInicial.Trim.Length > 0 Then _
                        adminBD.agregarParametroSQL("@fechaCreacionInicial", .FechaCreacionInicial.Trim, SqlDbType.DateTime)
                    If .FechaCreacionFinal IsNot Nothing AndAlso .FechaCreacionFinal.Trim.Length > 0 Then _
                        adminBD.agregarParametroSQL("@fechaCreacionFinal", .FechaCreacionFinal.Trim, SqlDbType.DateTime)

                    If .FechaCierreInicial IsNot Nothing AndAlso .FechaCierreInicial.Trim.Length > 0 Then _
                        adminBD.agregarParametroSQL("@fechaCierraInicial", .FechaCierreInicial.Trim, SqlDbType.DateTime)
                    If .FechaCierreFinal IsNot Nothing AndAlso .FechaCierreFinal.Trim.Length > 0 Then _
                        adminBD.agregarParametroSQL("@fechaCierraFinal", .FechaCierreFinal.Trim, SqlDbType.DateTime)

                    If .IdEstado <> 0 Then adminBD.agregarParametroSQL("@idEstado", .IdEstado, SqlDbType.SmallInt)

                End With

                resultado = adminBD.ejecutarDataTable("SeleccionarDespacho", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return resultado
        End Function

        ''' <summary>
        ''' Asigna una transportadora al despacho de acuerdo a la ciudad origen y destino que tenga la orden
        ''' </summary>
        ''' <remarks></remarks>
        Public Function AsignarTransportadora() As ResultadoProceso
            Dim respuesta As New ResultadoProceso
            Dim adminBD As New LMDataAccess
            Dim datosFaltantes As String
            Dim dtInfoTransportadora As New DataTable
            Dim msjNotifiacion As String = String.Empty
            Try
                adminBD.iniciarTransaccion()
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.agregarParametroSQL("@idTipoUnidad", Me._tipoUnidad.IdTipoUnidad)
                adminBD.SqlParametros.Add("@datosFaltantes", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                dtInfoTransportadora = adminBD.ejecutarDataTable("AsignarTransportadoraDespacho", CommandType.StoredProcedure)

                respuesta.Valor = adminBD.SqlParametros("@return_value").Value
                datosFaltantes = adminBD.SqlParametros("@datosFaltantes").Value.ToString

                If respuesta.Valor = 0 Then
                    adminBD.confirmarTransaccion()
                    For Each dr As DataRow In dtInfoTransportadora.Rows
                        Me._transportadora = New Transportadora(CInt(dr("idTransportadora")))
                        Me._tipoTransporte = New TipoTransporte(CInt(dr("idTipoTransporte")))
                    Next
                Else
                    If respuesta.Valor = 1 Then
                        adminBD.abortarTransaccion()
                        respuesta.Mensaje = "Ocurrió un error inesperado al asignar información de transporte al despacho. " & msjNotifiacion
                    ElseIf respuesta.Valor = 2 Then
                        adminBD.abortarTransaccion()
                        msjNotifiacion = EnviarCorreoNotificacion(AsuntoNotificacion.Tipo.TransportadoraDespacho, datosFaltantes)
                        respuesta.Mensaje = "No se encontró transportadora para la combinación Origen - Destino - Tipo de Producto. " & msjNotifiacion
                    ElseIf respuesta.Valor = 3 Then
                        adminBD.abortarTransaccion()
                        msjNotifiacion = EnviarCorreoNotificacion(AsuntoNotificacion.Tipo.TransportadoraDespacho, datosFaltantes)
                        respuesta.Mensaje = "No se encontró información de tipo de producto para realizar la asignación. " & msjNotifiacion
                    End If
                End If
            Catch ex As Exception
                If adminBD.Reader IsNot Nothing Then adminBD.Reader.Close()
                adminBD.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try

            Return respuesta
        End Function

        ''' <summary>
        ''' Consulta la transportadora asignada al pedido para mostrarla en la información inicial del despacho
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ConsultarTransportadoraPedido()
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.agregarParametroSQL("@idPedido", Me._pedido.IdPedido)
                adminBD.ejecutarReader("ConsultarTransportadoraPedido", CommandType.StoredProcedure)

                While adminBD.Reader.Read
                    Me._transportadora = New Transportadora(CInt(adminBD.Reader("idTransportadora")))
                    Me._tipoTransporte = New TipoTransporte(CInt(adminBD.Reader("idTipoTransporte")))
                End While

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function BuscarDetallePedido() As DataTable
            Dim resultado As New DataTable
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim detalleTemporal As Despachos.Detalle

            Try
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                resultado = adminBD.ejecutarDataTable("SeleccionarDetallePedido", CommandType.StoredProcedure)

                _detalle.Clear()

                For Each fila As DataRow In resultado.Rows
                    detalleTemporal = New Detalle
                    detalleTemporal.IdDespacho = Me.IdDespacho
                    detalleTemporal.Material = fila("material")
                    detalleTemporal.Descripcion = fila("descripcion").ToString()
                    detalleTemporal.CantidadPedida = CInt(fila("cantidadPedida"))
                    detalleTemporal.CantidadLeida = CInt(fila("cantidadLeida"))
                    Me._cantidadPedidaTotal += CInt(fila("cantidadPedida"))
                    Me._detalle.Add(detalleTemporal)
                Next
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return resultado
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Function BuscarDetalleDespacho() As DataTable
            Dim resultado As New DataTable
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim detalleTemporal As New Despachos.DetalleSerial

            Try
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                resultado = adminBD.ejecutarDataTable("SeleccionarDetalleDespacho", CommandType.StoredProcedure)

                _detalleSerial.Clear()

                For Each fila As DataRow In resultado.Rows
                    detalleTemporal = New DetalleSerial
                    detalleTemporal.IdDespacho = Me._idDespacho
                    detalleTemporal.Material = fila("material").ToString
                    detalleTemporal.Serial = fila("serial").ToString
                    Me._detalleSerial.Add(detalleTemporal)
                Next
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return resultado
        End Function

        Public Function ObtenerInfoCuarentenaDespacho() As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                db.agregarParametroSQL("@idDespacho", Me._idDespacho)
                Try
                    dtDatos = .ejecutarDataTable("ObtenerInfoCuarentenaDespacho", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarMaterialesDespacho() As DataTable
            Dim resultado As New DataTable
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.agregarParametroSQL("@idDespacho", Me.IdDespacho)
                resultado = adminBD.ejecutarDataTable("SeleccionarMaterialesDespacho", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return resultado
        End Function

        ''' <summary>
        ''' Registra todos los seriales pertenecientes a una caja en el detalle de un despacho
        ''' </summary>
        ''' <param name="idCaja">Identificador de la caja que contiene los seriales a leer en el despacho</param>
        ''' <remarks></remarks>
        Public Sub RegistrarCaja(ByVal idCaja As String, Optional ByVal material As String = "")
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim datosCaja As Array
            Dim idFactura As Integer = 0
            Dim region As String = ""
            Dim estiba As Integer = 0
            Dim caja As Integer = 0
            Dim respuesta As Integer = -1

            Try
                datosCaja = Split(idCaja, "-")
                idFactura = CInt(datosCaja(0))
                region = datosCaja(1)
                estiba = CInt(datosCaja(2))
                caja = CInt(datosCaja(3))
                adminBD.iniciarTransaccion()
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.agregarParametroSQL("@idFactura", idFactura)
                adminBD.agregarParametroSQL("@region", region, SqlDbType.VarChar)
                adminBD.agregarParametroSQL("@estiba", estiba)
                adminBD.agregarParametroSQL("@caja", caja)
                If material = "" Then
                    adminBD.agregarParametroSQL("@material", DBNull.Value)
                Else
                    adminBD.agregarParametroSQL("@material", material, SqlDbType.VarChar)
                End If
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                If _idTipoPedido = 6 Then
                    adminBD.ejecutarNonQuery("RegistrarCajaDespachoCuarentena", CommandType.StoredProcedure)
                Else
                    adminBD.ejecutarNonQuery("RegistrarCaja", CommandType.StoredProcedure)
                End If

                respuesta = adminBD.SqlParametros("@return_value").Value

                If respuesta = -1 Then
                    Throw New Exception("Ocurrió un error recuperando los datos de retorno de la base de datos")
                Else
                    If respuesta <> 0 Then
                        If _idTipoPedido = 6 Then
                            If respuesta = 1 Then
                                Throw New Exception("No fue posible obtener el tipo de producto para el material de los seriales")
                            ElseIf respuesta = 4 Then
                                Throw New Exception("No se encontró material para los seriales indicados")
                            ElseIf respuesta = 5 Then
                                Throw New Exception("Seriales no cumplen con las condiciones necesarias para despacho")
                            ElseIf respuesta = 7 Then
                                Throw New Exception("Ocurrió un error a nivel de base de datos durante el registro de información")
                            ElseIf respuesta = 8 Then
                                Throw New Exception("Seriales ya se encuentran leídos en un despacho")
                            ElseIf respuesta = 9 Then
                                Throw New Exception("Seriales no se encuentran dentro de una OTB")
                            ElseIf respuesta = 10 Then
                                Throw New Exception("No fue posible determinar el producto de los seriales")
                            ElseIf respuesta = 11 Then
                                Throw New Exception("Seriales ya se encuentran leídos en un despacho")
                            ElseIf respuesta = 12 Then
                                Throw New Exception("Seriales no se encuentran en cuarentena")
                            ElseIf respuesta = 13 Then
                                Throw New Exception("Los seriales están fuera del rango de las fechas de alistamiento")
                            ElseIf respuesta = 14 Then
                                Throw New Exception("La cantidad para el material indicado ya se encuentran completas")
                            End If
                        Else
                            If respuesta = 1 Then
                                Throw New Exception("Seriales no han completado el proceso de producción para ser despachados")
                            ElseIf respuesta = 2 Then
                                Throw New Exception("Uno o más de los seriales de la caja ya pertenecen a un despacho")
                            ElseIf respuesta = 3 Then
                                Throw New Exception("La caja no existe o no contiene seriales")
                            ElseIf respuesta = 4 Then
                                Throw New Exception("No se encontraron seriales en la caja para el material leído")
                            ElseIf respuesta = 5 Then
                                Throw New Exception("Uno o más de los seriales de la caja tiene un material no válido o una fecha de alistamiento no apropiada para ser despachado")
                            ElseIf respuesta = 6 Then
                                Throw New Exception("Uno o más de los seriales de la caja pertenecen a una cuarentena")
                            ElseIf respuesta = 7 Then
                                Throw New Exception("Ocurrió un error a nivel de base de datos durante el registro de información")
                            ElseIf respuesta = 8 Then
                                Throw New Exception("La cantidad para el material indicado ya se encuentran completas")
                            ElseIf respuesta = 10 Then
                                Throw New Exception("Los seriales de la caja no tienen un material asignado")
                            End If
                        End If
                    Else
                        adminBD.confirmarTransaccion()
                    End If
                End If

            Catch ex As Exception
                adminBD.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' Registra todos los seriales pertenecientes a una OTB en el detalle del despacho
        ''' </summary>
        ''' <param name="idOTB">Identificador de la OTB</param>
        ''' <remarks></remarks>
        Public Sub RegistrarOTB(ByVal idOTB As String, Optional ByVal material As String = "")
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim respuesta As Integer = -1

            Try
                adminBD.iniciarTransaccion()
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.agregarParametroSQL("@idOTB", idOTB, SqlDbType.VarChar)
                If material = "" Then
                    adminBD.agregarParametroSQL("@material", DBNull.Value)
                Else
                    adminBD.agregarParametroSQL("@material", material, SqlDbType.VarChar)
                End If
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                If _idTipoPedido = 6 Then
                    adminBD.ejecutarNonQuery("RegistrarOTBDespachoCuarentena", CommandType.StoredProcedure)
                Else
                    adminBD.ejecutarNonQuery("RegistrarOTB", CommandType.StoredProcedure)
                End If
                respuesta = adminBD.SqlParametros("@return_value").Value

                If respuesta = -1 Then
                    Throw New Exception("Ocurrió un error recuperando los valores de retorno de la base de datos")
                Else
                    If respuesta <> 0 Then
                        If _idTipoPedido = 6 Then
                            If respuesta = 1 Then
                                Throw New Exception("No fue posible obtener el tipo de producto para el material de los seriales")
                            ElseIf respuesta = 4 Then
                                Throw New Exception("No se encontró material para los seriales indicados")
                            ElseIf respuesta = 5 Then
                                Throw New Exception("Seriales no cumplen con las condiciones necesarias para despacho")
                            ElseIf respuesta = 7 Then
                                Throw New Exception("Ocurrió un error a nivel de base de datos durante el registro de información")
                            ElseIf respuesta = 8 Then
                                Throw New Exception("Seriales ya se encuentran leídos en un despacho")
                            ElseIf respuesta = 9 Then
                                Throw New Exception("Seriales no se encuentran dentro de una OTB")
                            ElseIf respuesta = 10 Then
                                Throw New Exception("No fue posible determinar el producto de los seriales")
                            ElseIf respuesta = 11 Then
                                Throw New Exception("Seriales ya se encuentran leídos en un despacho")
                            ElseIf respuesta = 12 Then
                                Throw New Exception("Seriales no se encuentran en cuarentena")
                            ElseIf respuesta = 13 Then
                                Throw New Exception("Los seriales están fuera del rango de las fechas de alistamiento")
                            ElseIf respuesta = 14 Then
                                Throw New Exception("La cantidad para el material indicado ya se encuentran completas")
                            End If
                        Else
                            If respuesta = 1 Then
                                Throw New Exception("Seriales no han completado el proceso de producción para ser despachados")
                            ElseIf respuesta = 2 Then
                                Throw New Exception("Uno o más de los seriales de la OTB ya pertenece a un despacho")
                            ElseIf respuesta = 3 Then
                                Throw New Exception("La OTB no contiene seriales")
                            ElseIf respuesta = 4 Then
                                Throw New Exception("La OTB no tiene un material asignado")
                            ElseIf respuesta = 5 Then
                                Throw New Exception("Uno o más de los seriales de la OTB tiene un material no válido o una fecha no apropiada para ser despachado")
                            ElseIf respuesta = 6 Then
                                Throw New Exception("Uno o más de los seriales de la OTB se encuentran en cuarentena")
                            ElseIf respuesta = 7 Then
                                Throw New Exception("Ocurrió un error a nivel de base de datos durante el registro de información")
                            ElseIf respuesta = 9 Then
                                Throw New Exception("La OTB indicada no existe")
                            ElseIf respuesta = 10 Then
                                Throw New Exception("Los seriales de la OTB no tienen un material asignado")
                            End If
                        End If
                    Else
                        adminBD.confirmarTransaccion()
                    End If
                End If

            Catch ex As Exception
                adminBD.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' Registra un serial en el detalle de un despacho de pedido de pruebas
        ''' </summary>
        ''' <param name="serial">Serial que se registrará en el despacho</param>
        ''' <remarks></remarks>
        Public Function RegistrarSerialEnvioPrueba(ByVal serial As String, ByVal usuarioRegistro As Integer) As ResultadoProceso
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim respuesta As New ResultadoProceso

            Try
                adminBD.iniciarTransaccion()
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.agregarParametroSQL("@serial", serial, SqlDbType.VarChar)
                If usuarioRegistro <> 0 Then adminBD.agregarParametroSQL("@usuarioRegistro", usuarioRegistro)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.ejecutarNonQuery("RegistrarSerialEnvioPrueba", CommandType.StoredProcedure)

                respuesta.Valor = adminBD.SqlParametros("@return_value").Value

                If respuesta.Valor <> 0 Then
                    adminBD.abortarTransaccion()
                    If respuesta.Valor = 1 Then
                        respuesta.Mensaje = "El serial indicado no ha se encuentra termosellado"
                    ElseIf respuesta.Valor = 2 Then
                        respuesta.Mensaje = "El serial leído ya pertenece al despacho actual"
                    ElseIf respuesta.Valor = 3 Then
                        respuesta.Mensaje = "El serial leído se encuentra asignado a una cuarentena"
                    ElseIf respuesta.Valor = 4 Then
                        respuesta.Mensaje = "No se encontró un material para el serial indicado"
                    ElseIf respuesta.Valor = 5 Then
                        respuesta.Mensaje = "El serial indicado no tiene fecha de recepción asignada"
                    ElseIf respuesta.Valor = 6 Then
                        respuesta.Mensaje = "El material del serial y el que fue leído en el despacho no coinciden"
                    ElseIf respuesta.Valor = 7 Then
                        respuesta.Mensaje = "Ocurrió un error a nivel de base de datos durante el registro de información"
                    ElseIf respuesta.Valor = 8 Then
                        respuesta.Mensaje = "La cantidad para el material indicado ya se encuentran completas"
                    ElseIf respuesta.Valor = 9 Then
                        respuesta.Mensaje = "El serial leído no se encuentra dentro de una OTB"
                    ElseIf respuesta.Valor = 10 Then
                        respuesta.Mensaje = "El serial leído no tiene un producto padre asignado"
                    ElseIf respuesta.Valor = 11 Then
                        respuesta.Mensaje = "El material del serial leído no corresponde a ninguno de los materiales del despacho"
                    ElseIf respuesta.Valor = 12 Then
                        respuesta.Mensaje = "La región del serial leído no corresponde a la región del despacho"
                    ElseIf respuesta.Valor = 13 Then
                        respuesta.Mensaje = "Serial leído se encuentra preactivado"
                    ElseIf respuesta.Valor = 14 Then
                        respuesta.Mensaje = "Serial se encuentra no conforme"
                    ElseIf respuesta.Valor = 15 Then
                        respuesta.Mensaje = "El serial indicado no se encuentra Nacionalizado."
                    ElseIf respuesta.Valor = 16 Then
                        respuesta.Mensaje = "El serial no pertenece a la orden de envío."
                    ElseIf respuesta.Valor = 17 Then
                        respuesta.Mensaje = "El serial no se encuentra acomodado en bodega."
                    Else
                        respuesta.Mensaje = "Ocurrió un error recuperando los valores de retorno de la base de datos"
                    End If
                Else
                    adminBD.confirmarTransaccion()
                End If

            Catch ex As Exception
                adminBD.abortarTransaccion()
                respuesta.Valor = -1
                respuesta.Mensaje = ex.Message
            Finally
                If adminBD IsNot Nothing Then adminBD.Dispose()
            End Try
            Return respuesta
        End Function

        ''' <summary>
        ''' Resgistra un serial específico en el detalle del despacho
        ''' </summary>
        ''' <param name="serial">Serial que va a ser registrado</param>
        ''' <remarks></remarks>
        Public Function RegistrarSerialSuelto(ByVal serial As String, ByVal usuarioRegistro As Integer, ByVal tipo As TipoMaterial, Optional ByVal material As String = "") As ResultadoProceso
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim respuesta As New ResultadoProceso

            respuesta.Valor = -1

            Try
                adminBD.iniciarTransaccion()
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.agregarParametroSQL("@serial", serial, SqlDbType.VarChar)
                If material = "" Then
                    adminBD.agregarParametroSQL("@material", DBNull.Value)
                Else
                    adminBD.agregarParametroSQL("@material", material, SqlDbType.VarChar)
                End If
                If usuarioRegistro <> 0 Then adminBD.agregarParametroSQL("@usuarioRegistro", usuarioRegistro)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                If _idTipoPedido = 6 Then
                    adminBD.ejecutarNonQuery("RegistrarSerialDespachoCuarentena", CommandType.StoredProcedure)
                ElseIf _idTipoPedido = 9 Then
                    adminBD.ejecutarNonQuery("RegistrarSerialDespachoEspecial", CommandType.StoredProcedure)
                ElseIf _idTipoPedido = 10 Then
                    adminBD.ejecutarNonQuery("RegistrarSerialDespachoServicioTecnico", CommandType.StoredProcedure)
                Else
                    If tipo.nombreTablaLectura = "productos_serial" Then
                        adminBD.ejecutarNonQuery("RegistrarSerial", CommandType.StoredProcedure)
                    ElseIf tipo.nombreTablaLectura = "sims" Then
                        adminBD.ejecutarNonQuery("RegistrarSim", CommandType.StoredProcedure)
                    ElseIf tipo.nombreTablaLectura = "infoTarjetaPrepago" Then
                        adminBD.ejecutarNonQuery("RegistrarTarjetaPrepago", CommandType.StoredProcedure)
                    End If
                End If

                respuesta.Valor = adminBD.SqlParametros("@return_value").Value

                If respuesta.Valor = -1 Then
                    adminBD.abortarTransaccion()
                    respuesta.Mensaje = "Ocurrió un error recuperando los valores de retorno de la base de datos"
                Else
                    If respuesta.Valor <> 0 Then
                        adminBD.abortarTransaccion()
                        If _idTipoPedido = 6 Then
                            If respuesta.Valor = 1 Then
                                respuesta.Mensaje = "No fue posible obtener el tipo de producto para el material del serial"
                            ElseIf respuesta.Valor = 4 Then
                                respuesta.Mensaje = "No se encontró material para el serial indicado"
                            ElseIf respuesta.Valor = 5 Then
                                respuesta.Mensaje = "Serial no cumple con las condiciones necesarias para despacho"
                            ElseIf respuesta.Valor = 7 Then
                                respuesta.Mensaje = "Ocurrió un error a nivel de base de datos durante el registro de información"
                            ElseIf respuesta.Valor = 8 Then
                                respuesta.Mensaje = "Serial ya se encuentra leído en un despacho"
                            ElseIf respuesta.Valor = 9 Then
                                respuesta.Mensaje = "Serial no se encuentra dentro de una OTB"
                            ElseIf respuesta.Valor = 10 Then
                                respuesta.Mensaje = "No fue posible determinar el producto del serial"
                            ElseIf respuesta.Valor = 11 Then
                                respuesta.Mensaje = "Serial ya se encuentra leído en un despacho"
                            ElseIf respuesta.Valor = 12 Then
                                respuesta.Mensaje = "Serial no se encuentra en cuarentena"
                            ElseIf respuesta.Valor = 13 Then
                                respuesta.Mensaje = "El serial está fuera del rango de las fechas de alistamiento"
                            ElseIf respuesta.Valor = 14 Then
                                respuesta.Mensaje = "La cantidad para el material ya está completa"
                            ElseIf respuesta.Valor = 15 Then
                                respuesta.Mensaje = "El serial indicado no se encuentra cargado."
                            End If
                        ElseIf _idTipoPedido = 9 Or _idTipoPedido = 10 Then
                            If respuesta.Valor = 1 Then
                                respuesta.Mensaje = "No fue posible obtener el tipo de producto para el material del serial"
                            ElseIf respuesta.Valor = 4 Then
                                respuesta.Mensaje = "No se encontró material para el serial indicado"
                            ElseIf respuesta.Valor = 5 Then
                                respuesta.Mensaje = "Serial no cumple con las condiciones necesarias para despacho"
                            ElseIf respuesta.Valor = 7 Then
                                respuesta.Mensaje = "Ocurrió un error a nivel de base de datos durante el registro de información"
                            ElseIf respuesta.Valor = 8 Then
                                respuesta.Mensaje = "Serial ya se encuentra leído en un despacho"
                            ElseIf respuesta.Valor = 9 Then
                                respuesta.Mensaje = "Serial no se encuentra dentro de una OTB"
                            ElseIf respuesta.Valor = 10 Then
                                respuesta.Mensaje = "No fue posible determinar el producto del serial"
                            ElseIf respuesta.Valor = 11 Then
                                respuesta.Mensaje = "Serial ya se encuentra leído en un despacho"
                            ElseIf respuesta.Valor = 14 Then
                                respuesta.Mensaje = "La cantidad para el material ya está completa"
                            End If
                        Else
                            If respuesta.Valor = 1 Then
                                respuesta.Mensaje = "El serial indicado no ha se encuentra termosellado"
                            ElseIf respuesta.Valor = 2 Then
                                respuesta.Mensaje = "El serial leído ya pertenece al despacho actual"
                            ElseIf respuesta.Valor = 3 Then
                                respuesta.Mensaje = "El serial leído se encuentra asignado a una cuarentena"
                            ElseIf respuesta.Valor = 4 Then
                                respuesta.Mensaje = "No se encontró un material para el serial indicado"
                            ElseIf respuesta.Valor = 5 Then
                                respuesta.Mensaje = "El serial indicado no tiene fecha de recepción asignada"
                            ElseIf respuesta.Valor = 6 Then
                                respuesta.Mensaje = "El material del serial y el que fue leído en el despacho no coinciden"
                            ElseIf respuesta.Valor = 7 Then
                                respuesta.Mensaje = "Ocurrió un error a nivel de base de datos durante el registro de información"
                            ElseIf respuesta.Valor = 8 Then
                                respuesta.Mensaje = "La cantidad para el material indicado ya se encuentran completas"
                            ElseIf respuesta.Valor = 9 Then
                                respuesta.Mensaje = "El serial leído no se encuentra dentro de una OTB"
                            ElseIf respuesta.Valor = 10 Then
                                respuesta.Mensaje = "El serial leído no tiene un producto padre asignado"
                            ElseIf respuesta.Valor = 11 Then
                                respuesta.Mensaje = "El material del serial leído no corresponde a ninguno de los materiales del despacho"
                            ElseIf respuesta.Valor = 12 Then
                                respuesta.Mensaje = "La región del serial leído no corresponde a la región del despacho"
                            ElseIf respuesta.Valor = 13 Then
                                respuesta.Mensaje = "Serial leído se encuentra preactivado"
                            ElseIf respuesta.Valor = 14 Then
                                respuesta.Mensaje = "Serial se encuentra no conforme"
                            ElseIf respuesta.Valor = 15 Then
                                respuesta.Mensaje = "El serial indicado no se encuentra cargado."
                            ElseIf respuesta.Valor = 16 Then
                                respuesta.Mensaje = "El serial no cumple con las condiciones de FIFO o FEFO para ser despachado"
                            ElseIf respuesta.Valor = 17 Then
                                respuesta.Mensaje = "El serial no se encuentra acomodado en bodega."
                            End If
                        End If
                    Else
                        adminBD.confirmarTransaccion()
                    End If
                End If

            Catch ex As Exception
                adminBD.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return respuesta
        End Function

        ''' <summary>
        ''' Registra todos los seriales comprendidos en un rango de sim cards en el detalle del despacho
        ''' </summary>
        ''' <param name="simInicial">Sim card inicial del rango a registrar</param>
        ''' <param name="simFinal">Sim card final del rango a registrar</param>
        ''' <remarks></remarks>
        Public Function RegistrarRangoSims(ByVal simInicial As String, ByVal simFinal As String, Optional ByVal material As String = "") As ResultadoProceso
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim respuesta As New ResultadoProceso
            Dim rango As Double = 0

            rango = CDbl(simFinal) - CDbl(simInicial)

            If CInt(rango) <> 5 And CInt(rango) <> 400 And CInt(rango) <> 800 Then
                Throw New Exception("Rango no válido para lectura")
            Else
                Try
                    adminBD.iniciarTransaccion()
                    adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                    adminBD.agregarParametroSQL("@simInicial", simInicial.Trim, SqlDbType.VarChar)
                    adminBD.agregarParametroSQL("@simFinal", simFinal.Trim, SqlDbType.VarChar)
                    If material = "" Then
                        adminBD.agregarParametroSQL("@material", DBNull.Value)
                    Else
                        adminBD.agregarParametroSQL("@material", material, SqlDbType.VarChar)
                    End If
                    adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    If _idTipoPedido = 6 Then
                        adminBD.ejecutarNonQuery("RegistrarRangoSimsDespachoCuarentena", CommandType.StoredProcedure)
                    Else
                        adminBD.ejecutarNonQuery("RegistrarRangoSims", CommandType.StoredProcedure)
                    End If

                    respuesta.Valor = adminBD.SqlParametros("@return_value").Value

                    If respuesta.Valor = -1 Then
                        adminBD.abortarTransaccion()
                        respuesta.Mensaje = "Ocurrió un error obteniendo información de retorno de la base de datos"
                    Else
                        If respuesta.Valor <> 0 Then
                            adminBD.abortarTransaccion()
                            If _idTipoPedido = 6 Then
                                If respuesta.Valor = 1 Then
                                    respuesta.Mensaje = "No fue posible obtener el tipo de producto para el material de los seriales"
                                ElseIf respuesta.Valor = 4 Then
                                    respuesta.Mensaje = "No se encontró material para los seriales indicados"
                                ElseIf respuesta.Valor = 5 Then
                                    respuesta.Mensaje = "Seriales no cumplen con las condiciones necesarias para despacho"
                                ElseIf respuesta.Valor = 7 Then
                                    respuesta.Mensaje = "Ocurrió un error a nivel de base de datos durante el registro de información"
                                ElseIf respuesta.Valor = 8 Then
                                    respuesta.Mensaje = "Seriales ya se encuentran leídos en un despacho"
                                ElseIf respuesta.Valor = 9 Then
                                    respuesta.Mensaje = "Seriales no se encuentran dentro de una OTB"
                                ElseIf respuesta.Valor = 10 Then
                                    respuesta.Mensaje = "No fue posible determinar el producto de los seriales"
                                ElseIf respuesta.Valor = 11 Then
                                    respuesta.Mensaje = "Seriales ya se encuentran leídos en un despacho"
                                ElseIf respuesta.Valor = 12 Then
                                    respuesta.Mensaje = "Seriales no se encuentran en cuarentena"
                                ElseIf respuesta.Valor = 13 Then
                                    respuesta.Mensaje = "Los seriales están fuera del rango de las fechas de alistamiento"
                                ElseIf respuesta.Valor = 14 Then
                                    respuesta.Mensaje = "La cantidad para el material indicado ya se encuentran completas"
                                ElseIf respuesta.Valor = 15 Then
                                    respuesta.Mensaje = "El rango especificado no contiene sims habilitadas para ser despachadas"
                                End If
                            Else
                                If respuesta.Valor = 1 Then
                                    respuesta.Mensaje = "Uno o más de los seriales del rango no se encuentra nacionalizado"
                                ElseIf respuesta.Valor = 2 Then
                                    respuesta.Mensaje = "Uno o más de los seriales del rango de sim cards ya pertenecen a un despacho"
                                ElseIf respuesta.Valor = 3 Then
                                    respuesta.Mensaje = "Uno o más de los seriales del rango se encuentran marcados como preactivados"
                                ElseIf respuesta.Valor = 4 Then
                                    respuesta.Mensaje = "Uno o más de los seriales del rango de sim cards no tiene asignado un material"
                                ElseIf respuesta.Valor = 5 Then
                                    respuesta.Mensaje = "Uno o más de los seriales del rango de sim cards no tiene fecha de recepción asociada"
                                ElseIf respuesta.Valor = 6 Then
                                    respuesta.Mensaje = "Uno o más de los seriales del rango de sim cards ya pertenecen a un despacho"
                                ElseIf respuesta.Valor = 7 Then
                                    respuesta.Mensaje = "Ocurrió un error a nivel de base de datos durante el registro de información"
                                ElseIf respuesta.Valor = 8 Then
                                    respuesta.Mensaje = "Uno o más de los seriales del rango no cumplen con FIFO"
                                ElseIf respuesta.Valor = 9 Then
                                    respuesta.Mensaje = "Uno o más seriales del rango de sim cards leído se encuentra en cuarentena"
                                ElseIf respuesta.Valor = 10 Then
                                    respuesta.Mensaje = "Los seriales del rango de sim cards no tienen un material asignado"
                                ElseIf respuesta.Valor = 11 Then
                                    respuesta.Mensaje = "Uno o más de los seriales del rango no se encuentran dentro de una OTB"
                                End If
                            End If
                        Else
                            adminBD.confirmarTransaccion()
                        End If
                    End If

                Catch ex As Exception
                    adminBD.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    adminBD.Dispose()
                End Try
            End If

            Return respuesta

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="estadoValido"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidarEstadoDespacho(ByVal estadoValido As Integer) As Boolean
            Dim respuesta As Boolean = False
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.agregarParametroSQL("@idEstado", estadoValido)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.ejecutarNonQuery("VerificarEstadoDespacho", CommandType.StoredProcedure)

                respuesta = CBool(adminBD.SqlParametros("@return_value").Value)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            Return respuesta
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AnularDespacho()
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.ejecutarNonQuery("AnularDespacho", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ActualizarInformacionDespacho(ByVal idUsuario As Integer)
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            With adminBD
                .agregarParametroSQL("@idDespacho", Me._idDespacho)
                .agregarParametroSQL("@idTipoTransporte", Me._tipoTransporte.IdTipoTransporte)
                .agregarParametroSQL("@idTransportadora", Me._transportadora.IdTransportadora)
                .agregarParametroSQL("@idUsuario", idUsuario)

                adminBD.ejecutarNonQuery("ActualizarInformacionDespacho", CommandType.StoredProcedure)
            End With

        End Sub

        Public Function EjecutarConteoSAP() As List(Of ResultadoProceso)
            Dim dtMaterial As DataTable = Despachos.Detalle.Obtener(_idDespacho) 'Consultar materiales del Pedido
            Dim dtSerial As DataTable = Despachos.DetalleSerial.Obtener(_idDespacho) 'Consultar seriales del Despacho
            Dim numMateriales As Integer = dtMaterial.Rows.Count
            Dim numSeriales As Integer = dtSerial.Rows.Count
            Dim miRetorno As New List(Of ResultadoProceso)
            ReDim _infoEntregaSAP(numMateriales - 1)
            ReDim _infoSerialesSAP(numSeriales - 1)
            'genera la guia para el despacho
            If Me._transportadora.IdTransportadora = 0 Or Me._tipoTransporte.IdTipoTransporte = 0 Then
                Throw New Exception("El despacho no puede cerrarse, pues no existe información de transporte registrada")
            End If
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Try
                If Me.Transportadora.UsaPrecintos Then
                    Me.AsignarPrecintos(adminBD, _listaPrecintos)
                    If _listaPrecintos.Trim.Length = 0 Then Throw New Exception("No se asignaron completamente los precintos, el despacho no puede cerrarse.")
                End If
                If Me.Transportadora.UsaGuia Then
                    Me.AsignarGuia(adminBD)
                    If _guia.Trim.Length = 0 Then Throw New Exception("No fue asignada la guía al despacho, por tanto no puede cerrarse.")
                End If

                If Me.Pedido.ContabilizarSAP Then
                    Dim dtTipoProducto As DataTable = GetDistinctsFromDataTable(dtMaterial, New ArrayList("esSerializado".Split(",")))
                    If dtTipoProducto.Rows.Count > 0 Then
                        If CBool(dtTipoProducto.Rows(0)("esSerializado")) Then
                            miRetorno = Me.ContabilizarSerializadoSAP(dtMaterial, dtSerial)
                        Else
                            miRetorno = Me.ContabilizarNoSerializadoSAP(dtMaterial, dtSerial)
                        End If
                    Else
                        Throw New Exception("El pedido contiene producto serializado y no serializado, por favor verificar")
                    End If
                End If

                If miRetorno.Count <> 0 Then
                    Me.CerrarDespacho(adminBD)
                Else
                    Throw New Exception("Error al contabilizar el despacho")
                End If
            Finally
                adminBD.Dispose()
            End Try
            Return miRetorno
        End Function

        Public Shared Function ObtenerReporteDeFacturacionTransportadora(ByVal filtro As FiltroReporteFacturacionTransportador, Optional ByVal esExtendido As Boolean = False)
            Dim db As New LMDataAccess

            db.TiempoEsperaComando = 1200
            If filtro.idTransportador > 0 Then db.agregarParametroSQL("@idtransportadora", filtro.idTransportador, SqlDbType.Int)
            If filtro.idCanalDistribucion > 0 Then db.agregarParametroSQL("@idTipoDestinatario", filtro.idCanalDistribucion, SqlDbType.Int)
            If filtro.fechaDespachoInicial > Date.MinValue Then
                db.agregarParametroSQL("@fechaDespachoInicial", filtro.fechaDespachoInicial, SqlDbType.Date)
                db.agregarParametroSQL("@fechaDespachoFinal", filtro.fechaDespachoFinal, SqlDbType.Date)
            End If
            Dim query As String
            If esExtendido Then
                query = "ReporteFacturacionTransportadoraExtendido"
            Else
                query = "ReporteFacturacionTransportadora"
            End If
            Dim dt As DataTable = db.ejecutarDataTable(query, CommandType.StoredProcedure)
            Return dt
        End Function
#End Region

#Region "Métodos Privados"
        Private Function EjecutarServicioWEBConteoSAP() As List(Of ResultadoProceso)

            Dim _resultadoConteoSAP = New SAPZmmCapser.OutputLgCapSer
            Dim hayError As Boolean = False
            Dim zmmCapser As New SAPZmmCapser.WS_CAPSER_LG
            Dim infoWs As New InfoUrlWebService(zmmCapser, True)
            Dim genResult As New List(Of ResultadoProceso)
            Dim genCredencialesWS As New GeneradorCredencialesWebService()
            zmmCapser.Credentials = genCredencialesWS.Credenciales
            zmmCapser.Timeout = 1200000
            Dim resultado As ResultadoProceso
            Do
                hayError = False
                Try
                    _contadorIntentoConteoSAP += 1
                    _resultadoConteoSAP = zmmCapser.executeZmmLgCapser(_pedido.NumeroEntrega, _peso, IIf(String.IsNullOrEmpty(_listaPrecintos), _guia, _listaPrecintos), "X", _infoEntregaSAP, _infoSerialesSAP)
                Catch ex As Exception
                    hayError = True
                End Try

                If _resultadoConteoSAP.oReturn IsNot Nothing AndAlso _resultadoConteoSAP.oReturn.Length > 0 Then
                    For indx As Integer = 0 To _resultadoConteoSAP.oReturn.Length - 1
                        If _resultadoConteoSAP.oReturn(indx).type = "E" Or _resultadoConteoSAP.oReturn(indx).type = "A" Then
                            hayError = True
                            Me.RegistrarMensaje(_resultadoConteoSAP.oReturn(indx).type, _resultadoConteoSAP.oReturn(indx).message)
                        End If
                    Next

                    If Not hayError Then
                        genResult.Clear()
                        For indx As Integer = 0 To _resultadoConteoSAP.oReturn.Length - 1
                            If _resultadoConteoSAP.oReturn(indx).type = "S" Then
                                Me.RegistrarMensaje(_resultadoConteoSAP.oReturn(indx).type, _resultadoConteoSAP.oReturn(indx).message)
                            End If
                        Next
                        Try
                            _numDocumentoConteoSAP = _resultadoConteoSAP.oReturn(0).messageV1
                            resultado = New ResultadoProceso
                            resultado.Valor = Pedido.NumeroEntrega
                            resultado.Mensaje = GeneradorDocumentosSAP.tipoDoc.Remision
                            genResult.Add(resultado)

                            If Me._pedido.Tipo.IdTipo = 1 Then
                                For indx As Integer = 1 To _resultadoConteoSAP.oReturn.Length - 1
                                    If _resultadoConteoSAP.oReturn(indx).type = "S" Then
                                        resultado = New ResultadoProceso
                                        resultado.Valor = _resultadoConteoSAP.oReturn(indx).messageV3
                                        resultado.Mensaje = GeneradorDocumentosSAP.tipoDoc.Factura
                                        genResult.Add(resultado)
                                    End If
                                Next
                            End If
                        Catch ex As Exception
                            Throw New Exception(ex.Message)
                        End Try

                    End If
                Else
                    hayError = True
                    Me.RegistrarMensaje(1, "No se obtuvo respuesta por parte del Web Service. Por favor contacte a IT Development")
                End If

            Loop While (hayError And _contadorIntentoConteoSAP < 3)

            Return genResult
        End Function

        Private Sub RegistrarMensaje(ByVal tipo As String, ByVal mensaje As String)
            If _dtMensajesContabilizacion Is Nothing Then
                _dtMensajesContabilizacion = New DataTable
                _dtMensajesContabilizacion.Columns.Add(New DataColumn("tipo"))
                _dtMensajesContabilizacion.Columns.Add(New DataColumn("mensaje"))
            End If
            Dim dr As DataRow = _dtMensajesContabilizacion.NewRow()
            dr("tipo") = tipo
            dr("mensaje") = mensaje
            _dtMensajesContabilizacion.Rows.Add(dr)
        End Sub

        Private Function ContabilizarSerializadoSAP(ByVal dtMaterial As DataTable, ByVal dtSerial As DataTable) As List(Of ResultadoProceso) 'Contabilización de Despachos

            Dim numMateriales As Integer = dtMaterial.Rows.Count
            Dim numSeriales As Integer = dtSerial.Rows.Count
            Dim posicionCliente As Integer = 0

            Dim index As Integer = 0
            Dim indexSerial As Integer = 0
            Dim iNumeroEntrega As Long = _pedido.NumeroEntrega
            Dim objCliente As New Cliente(_pedido.IdCliente)

            For Each drMaterial As DataRow In dtMaterial.Rows
                _infoEntregaSAP(index) = New SAPZmmCapser.ZmmLmEntregas
                With _infoEntregaSAP(index)
                    .vbeln = iNumeroEntrega
                    .matnr = drMaterial("idSubproducto2")
                    Integer.TryParse(drMaterial("idPosicionCliente").ToString(), posicionCliente)
                    .posnr = posicionCliente
                    .werks = drMaterial("centro")
                    .lgort = drMaterial("almacen")
                    .lfimg = drMaterial("cantidadLeida")
                    .werksR = objCliente.Centro
                    .lgortR = objCliente.Almacen
                End With
                Dim drSerial() As DataRow = dtSerial.Select("material='" & drMaterial("idSubproducto2") & "'")
                For Each fila As DataRow In drSerial
                    _infoSerialesSAP(indexSerial) = New SAPZmmCapser.ZmmLmSeriales
                    With _infoSerialesSAP(indexSerial)
                        .sernr = fila("serial").ToString.Trim()
                        Integer.TryParse(drMaterial("idPosicionCliente").ToString(), posicionCliente)
                        .posnr = posicionCliente
                        .vbeln = iNumeroEntrega
                        Dim lote As String
                        If fila("lote").ToString() <> "" Then
                            lote = CDate(fila("lote")).ToString("ddMMyyyy")
                            .typbz = lote
                            .herst = 1
                        End If
                    End With
                    indexSerial += 1
                Next
                index += 1
            Next
            Dim miRetorno As List(Of ResultadoProceso) = Me.EjecutarServicioWEBConteoSAP()
            '' guardar lista de documentos generados por SAP

            Return miRetorno
        End Function

        Private Function ContabilizarNoSerializadoSAP(ByVal dtMaterial As DataTable, ByVal dtSerial As DataTable) As List(Of ResultadoProceso)
            Dim numMateriales As Integer = dtMaterial.Rows.Count
            Dim numSeriales As Integer = dtSerial.Rows.Count

            Dim index As Integer = 0
            Dim indexSerial As Integer = 0
            Dim iNumeroEntrega As Long = _pedido.NumeroEntrega

            For Each drMaterial As DataRow In dtMaterial.Rows
                _infoEntregaSAP(index) = New SAPZmmCapser.ZmmLmEntregas
                With _infoEntregaSAP(index)
                    .vbeln = iNumeroEntrega
                    .matnr = drMaterial("idSubproducto2")
                    .posnr = drMaterial("idPosicionCliente")
                    .lfimg = drMaterial("cantidadLeida")
                End With
                Dim drSerial() As DataRow = dtSerial.Select("idSubproducto2='" & drMaterial("idSubproducto2") & "'")
                For indice As Integer = 0 To drSerial.Length - 1
                    _infoSerialesSAP(indexSerial) = New SAPZmmCapser.ZmmLmSeriales
                    With _infoSerialesSAP(indexSerial)
                        .sernr = drSerial(indice).Item("serial")
                        .posnr = drMaterial("idPosicionCliente")
                        .herst = drSerial(indice).Item("fechaVencimiento")
                        .typbz = drMaterial("cantidad_empaque")
                    End With
                    indexSerial += 1
                Next
                index += 1
            Next
            Dim miResultado As List(Of ResultadoProceso) = Me.EjecutarServicioWEBConteoSAP()
            Return miResultado
        End Function
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idDespacho"></param>
        ''' <remarks></remarks>
        Private Sub SeleccionarPorID(ByVal idDespacho As Integer)
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim idTrans As Integer
            Dim idTipoDespacho As Integer
            Dim idTipoTransporte As Integer
            Dim idTipoUnidad As Integer

            adminBD.agregarParametroSQL("@idDespachoLM", idDespacho, SqlDbType.Int)
            Try
                adminBD.ejecutarReader("SeleccionarDespacho", CommandType.StoredProcedure)
                While adminBD.Reader.Read()
                    Me._idDespacho = adminBD.Reader("idDespachoLM")
                    Me._pedido = New Pedido(CInt(adminBD.Reader("idPedidoLM")))
                    Me._idAuxiliarAtiende = adminBD.Reader("idAuxiliarAtiende")
                    Integer.TryParse(adminBD.Reader("idAuxiliarCierra").ToString, _idAuxiliarCierra)
                    Date.TryParse(adminBD.Reader("fechaCreacion").ToString, _fechaCreacion)
                    Date.TryParse(adminBD.Reader("fechaCierre").ToString, _fechaCierre)
                    Integer.TryParse(adminBD.Reader("idTransportadora").ToString, idTrans)
                    If idTrans = 0 Then
                        Me._transportadora = New Transportadora
                    Else
                        Me._transportadora = New Transportadora(idTrans)
                    End If
                    Me._guia = adminBD.Reader("guia").ToString
                    Me._estado = New Estado(CInt(adminBD.Reader("idEstado")))
                    Integer.TryParse(adminBD.Reader("idTipoDespacho").ToString, idTipoDespacho)
                    If idTipoDespacho = 0 Then
                        Me._tipoDespacho = New TipoDespacho
                    Else
                        Me._tipoDespacho = New TipoDespacho(CInt(adminBD.Reader("idTipoDespacho")))
                    End If
                    Integer.TryParse(adminBD.Reader("idTipoTransporte").ToString, idTipoTransporte)
                    If idTipoTransporte = 0 Then
                        Me._tipoTransporte = New TipoTransporte
                    Else
                        Me._tipoTransporte = New TipoTransporte(CInt(adminBD.Reader("idTipoTransporte")))
                    End If
                    Integer.TryParse(adminBD.Reader("idTipoUnidad").ToString, idTipoUnidad)
                    If idTipoUnidad = 0 Then
                        Me._tipoUnidad = New UnidadEmpaque
                    Else
                        Me._tipoUnidad = New UnidadEmpaque(CInt(adminBD.Reader("idTipoUnidad")))
                    End If
                    Double.TryParse(adminBD.Reader("peso").ToString, _peso)
                    Double.TryParse(adminBD.Reader("valorDeclarado").ToString, _valorDeclarado)
                    Integer.TryParse(adminBD.Reader("cantidadCajas").ToString, _cantidadCajas)
                    Double.TryParse(adminBD.Reader("volumen").ToString, _volumen)
                    Integer.TryParse(adminBD.Reader("numDocumentoConteoSAP").ToString(), _numDocumentoConteoSAP)
                    Me._ciudadDestino = adminBD.Reader("ciudadDestino").ToString
                    Me._regionDestino = adminBD.Reader("regionDestino").ToString
                    Me._codigoCliente = adminBD.Reader("codigoCliente").ToString
                    Me._contenido = adminBD.Reader("contenido").ToString
                End While
            Catch ex As Exception
                Throw New Exception("Imposible obtener tipo de despacho con ID especificado")
            Finally
                If Not adminBD.Reader.IsClosed Then adminBD.Reader.Close()
                adminBD.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="adminBD"></param>
        ''' <remarks></remarks>
        Private Sub EstablecerParametrosCreacion(ByRef adminBD As LMDataAccessLayer.LMDataAccess)
            With adminBD
                .agregarParametroSQL("@idPedido", Me.Pedido.IdPedido)
                .agregarParametroSQL("@idAuxiliar", Me.IdAuxiliarAtiende)
                If Me.Transportadora.IdTransportadora <> 0 Then
                    .agregarParametroSQL("@idTransportadora", Me.Transportadora.IdTransportadora)
                Else
                    .agregarParametroSQL("@idTransportadora", DBNull.Value)
                End If
                If Me.Guia <> "" Then
                    .agregarParametroSQL("@guia", Me.Guia, SqlDbType.VarChar)
                Else
                    .agregarParametroSQL("@guia", DBNull.Value)
                End If
                .agregarParametroSQL("@idTipoDespacho", Me.TipoDespacho.IdTipoDespacho)
                .agregarParametroSQL("@valorDeclarado", Me.ValorDeclarado, SqlDbType.Float)
                .agregarParametroSQL("@peso", Me.Peso, SqlDbType.Float)
                .agregarParametroSQL("@cantidadCajas", Me.CantidadCajas)
                .agregarParametroSQL("@volumen", Me.Volumen)
            End With
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="adminBD"></param>
        ''' <remarks></remarks>
        Private Sub EstablecerParametrosBusqueda(ByRef adminBD As LMDataAccessLayer.LMDataAccess, ByVal parametros As ArrayList)
            With parametros
                If .Item(0) <> 0 Then
                    adminBD.agregarParametroSQL("@idDespacho", .Item("idDespacho"))
                Else
                    adminBD.agregarParametroSQL("@idDespacho", DBNull.Value)
                End If
                If .Item(1) <> 0 Then
                    adminBD.agregarParametroSQL("@idPedido", .Item("idPedido"))
                Else
                    adminBD.agregarParametroSQL("@idPedido", DBNull.Value)
                End If
                If .Item(2) <> 0 Then
                    adminBD.agregarParametroSQL("@idAuxiliarAtiende", .Item("idAuxiliarAtiende"))
                Else
                    adminBD.agregarParametroSQL("@idAuxiliarAtiende", DBNull.Value)
                End If
                If .Item(3) <> 0 Then
                    adminBD.agregarParametroSQL("@fechaCreacionInicial", .Item("fechaCreacionInicial"))
                Else
                    adminBD.agregarParametroSQL("@fechaCreacionInicial", DBNull.Value)
                End If
                If .Item(4) <> 0 Then
                    adminBD.agregarParametroSQL("@fechaCreacionFinal", .Item("fechaCreacionFinal"))
                Else
                    adminBD.agregarParametroSQL("@fechaCreacionFinal", DBNull.Value)
                End If
                If .Item(5) <> 0 Then
                    adminBD.agregarParametroSQL("@fechaCierraInicial", .Item("fechaCierraInicial"))
                Else
                    adminBD.agregarParametroSQL("@fechaCierraInicial", DBNull.Value)
                End If
                If .Item(6) <> 0 Then
                    adminBD.agregarParametroSQL("@fechaCierraFinal", .Item("fechaCierraFinal"))
                Else
                    adminBD.agregarParametroSQL("@fechaCierraFinal", DBNull.Value)
                End If

                If .Item(7) <> 0 Then
                    adminBD.agregarParametroSQL("@idEstado", .Item("idEstado"))
                Else
                    adminBD.agregarParametroSQL("@idEstado", DBNull.Value)
                End If
            End With
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="adminBD"></param>
        ''' <param name="listadoPrecintos"></param>
        ''' <remarks></remarks>
        Private Sub AsignarPrecintos(ByRef adminBD As LMDataAccessLayer.LMDataAccess, ByVal listadoPrecintos As String)
            Dim respuesta As Integer = 0
            Try
                adminBD.SqlParametros.Clear()
                adminBD.agregarParametroSQL("@idDespacho", Me.IdDespacho)
                adminBD.agregarParametroSQL("@precintos", listadoPrecintos, SqlDbType.VarChar)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.ejecutarNonQuery("AsignarPrecintosDespacho", CommandType.StoredProcedure)

                respuesta = adminBD.SqlParametros("@return_value").Value

                If respuesta = -1 Then
                    Throw New Exception("Uno o más de los precintos indicados ya han sido utilizados")
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub AsignarGuia(ByRef adminBD As LMDataAccessLayer.LMDataAccess)
            Dim respuesta As Integer
            Dim notificacion As New AdministradorCorreo
            Dim textoMensaje As String = ""

            With adminBD
                .SqlParametros.Clear()
                .agregarParametroSQL("@idDespacho", Me.IdDespacho)
                .SqlParametros.Add("@guiaAsignada", SqlDbType.VarChar, 40).Direction = ParameterDirection.Output
                .SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .ejecutarNonQuery("AsignarGuiaDespacho", CommandType.StoredProcedure)
                respuesta = .SqlParametros("@return_value").Value
            End With

            If respuesta = 0 Then
                _guia = adminBD.SqlParametros("@guiaAsignada").Value
            ElseIf respuesta = 2 Then
                Throw New Exception("El despacho no tiene una transportadora asignada.")
            ElseIf respuesta = 3 Or respuesta = 1 Then
                Dim destinatarios As New MailAddressCollection
                Dim mensajeResultado As String = String.Empty
                destinatarios = CargarDestinatarios(8)
                If respuesta = 1 Then
                    textoMensaje = "Se ha agotado el número de guías para la transportadora: " & Me.Transportadora.Nombre
                Else
                    textoMensaje = "Actualmente el número disponible de guías para la transportadora: " & Me.Transportadora.Nombre & " es menor o igual a 1000."
                End If
                textoMensaje += "<br />Para realizar la asignación de rango de guías por favor dirigirse al sistema y realizar la asignación en la opción correspondiente."
                notificacion.Receptor = destinatarios
                notificacion.Asunto = "Asignación de rango de guías" & Me.Transportadora.Nombre
                notificacion.Titulo = notificacion.Asunto
                notificacion.TextoMensaje = textoMensaje
                notificacion.EnviarMail()
                If respuesta = 1 Then Throw New Exception("No existen guías diponibles para la transportadora " & Me.Transportadora.Nombre)
            End If
        End Sub

        ''' <summary>
        ''' Comprueba si las cantidades de un despacho están en ceros en su totalidad o para un material específico
        ''' </summary>
        ''' <param name="material">Material para el cual va a ser evaluada la cantidad restante por leer</param>
        ''' <returns>
        ''' TRUE: La cantidad restante por leer es cero (no hay más seriales por leer) para el material especificado o para el despacho en su totalidad       
        ''' FALSE: Aún hay diferencias en las cantidades del material especificado o de alguno de los materiales del despacho          
        ''' </returns>
        ''' <remarks></remarks>
        Public Function ComprobarCantidades(Optional ByVal material As String = "") As Boolean
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim respuesta As Boolean = False

            Try
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                If material <> "" Then
                    adminBD.agregarParametroSQL("@material", material)
                End If
                adminBD.SqlParametros.Add("@Return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                adminBD.ejecutarNonQuery("ComprobarCantidadesDespacho", CommandType.StoredProcedure)
                _resultado.Valor = adminBD.SqlParametros("@Return").Value
                If _resultado.Valor = 0 Then
                    respuesta = True
                ElseIf _resultado.Valor = 1 Then
                    respuesta = False
                    _resultado.Mensaje = "No se ha completado la lectura del despacho, por favor verificar "
                ElseIf Resultado.Valor = 2 Then
                    respuesta = False
                    _resultado.Mensaje = "La cantidad leida supera la cantidad pedida, por favor verificar "
                ElseIf _resultado.Valor = 3 Then
                    respuesta = False
                    _resultado.Mensaje = "No se pudo comprobar la cantidad leida del despacho "

                End If

                'For Each fila As DataRow In resultado.Rows
                '    If CInt(fila("diferencia")) <> 0 Then
                '        respuesta = False
                '    End If
                'Next
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return respuesta
        End Function

        Public Function ComprobarTipoTarifa(ByVal pIdDespacho As Integer) As Boolean
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim respuesta As Boolean = False

            Try
                adminBD.agregarParametroSQL("@idDespacho", pIdDespacho)
                adminBD.SqlParametros.Add("@Return", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                adminBD.ejecutarNonQuery("ComprobarTipoTarifa", CommandType.StoredProcedure)

                _resultado.Valor = adminBD.SqlParametros("@Return").Value
                If _resultado.Valor = 1 Then
                    respuesta = True
                ElseIf _resultado.Valor = 0 Then
                    respuesta = False
                    _resultado.Mensaje = "No existen tarifas registradas para el calculo del flete del despacho, por favor verificar."
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return respuesta
        End Function

        ''' <summary>
        ''' Envía un correo notificando el inconveniente presentado en el despacho
        ''' </summary>
        ''' <param name="tipo">tipo de notificación que será enviada</param>
        ''' <remarks></remarks>
        Private Function EnviarCorreoNotificacion(ByVal tipo As Comunes.AsuntoNotificacion.Tipo, ByVal datosFaltantes As String) As String
            Dim notificacion As New AdministradorCorreo
            Dim textoMensaje As String = ""
            Dim destinatarios As New MailAddressCollection
            Dim mensajeResultado As String = String.Empty

            destinatarios = CargarDestinatarios(tipo)

            Try
                Dim firma As String = "Logytech Mobile S.A.S <br />PBX. 57(1) 4395237 Ext 174 - 135"
                Dim observacion As String = String.Empty

                With notificacion
                    If tipo = AsuntoNotificacion.Tipo.TransportadoraDespacho Then
                        .Titulo = "Información de transportadora no encontrada"
                        .TextoMensaje = "No se encontró información suficiente para asignación de transportadora para despacho del pedido No. " & Me.Pedido.IdPedido & "." & _
                                        "<br /> Los datos correspondientes al despacho son: <br /><br />" & datosFaltantes & _
                                        "<br /><br /> Es necesario cargar la información en la matriz o editar la información correspondiente al despacho."

                    ElseIf tipo = AsuntoNotificacion.Tipo.ValorMaterialDespacho Then
                        .Titulo = "Valor de material no encontrado"
                        .TextoMensaje = "No se encontró información suficiente para calcular el valor declarado para despacho del pedido No. " & Me.Pedido.IdPedido & "." & _
                                        "<br /> Los datos faltantes son: <br /><br />" & datosFaltantes & _
                                        "<br /><br /> Es necesario cargar la información en la matriz."
                    End If

                    .DisplayName = "Información Faltante Despachos"
                    .Asunto = "Información Faltante Despachos"

                    ' destinatarios
                    .Receptor = destinatarios
                    .FirmaMensaje = firma
                    If Not .EnviarMail() Then
                        mensajeResultado = "Ocurrió un error inesperado y no fué posible enviar la notificación"
                    End If
                End With
            Catch ex As Exception
                mensajeResultado = ex.Message
            End Try
            Return mensajeResultado
        End Function

        Private Function CargarDestinatarios(ByVal tipo As Comunes.AsuntoNotificacion.Tipo) As MailAddressCollection
            Dim filtro As New FiltroUsuarioNotificacion
            Dim dtDestinos As New DataTable
            Dim destinosPara As String
            Dim destinosCopia As String
            Dim destinos As New MailAddressCollection

            filtro.IdAsuntoNotificacion = tipo
            filtro.Separador = ", "

            Try
                dtDestinos = UsuarioNotificacion.ObtenerDestinatarioNotificacion(filtro)
                destinosPara = dtDestinos.Rows(0)("destinoPara").ToString
                destinosCopia = dtDestinos.Rows(0)("destinoCopia").ToString

                destinos.Add(destinosPara)
                destinos.Add(destinosCopia)

            Catch ex As Exception
            End Try

            Return destinos
        End Function

        ''' <summary>
        ''' Calcula el valor declarado que debe tener el despacho de acuerdo a los seriales de la orden
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CalcularValorDeclarado(ByRef adminBD As LMDataAccessLayer.LMDataAccess)
            Dim respuesta As Integer
            Dim valorAsignado As Double = 0
            Dim datosFaltantes As String
            Dim msjNotifiacion As String = String.Empty
            Try
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.agregarParametroSQL("@idpedido", Me.Pedido.IdPedido)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                adminBD.SqlParametros.Add("@valorAsignado", SqlDbType.Int).Direction = ParameterDirection.Output
                adminBD.SqlParametros.Add("@datosFaltantes", SqlDbType.VarChar, 1200).Direction = ParameterDirection.Output

                adminBD.ejecutarNonQuery("CalcularValorDeclarado", CommandType.StoredProcedure)

                respuesta = adminBD.SqlParametros("@return_value").Value
                datosFaltantes = adminBD.SqlParametros("@datosFaltantes").Value.ToString

                If respuesta = 1 Then
                    msjNotifiacion = EnviarCorreoNotificacion(AsuntoNotificacion.Tipo.ValorMaterialDespacho, datosFaltantes)
                    Throw New Exception("No se encontró información en la Lista de Precios para una o más combinaciones Material - Región. " & msjNotifiacion)
                ElseIf respuesta = 2 Or respuesta = 3 Then
                    msjNotifiacion = EnviarCorreoNotificacion(AsuntoNotificacion.Tipo.ValorMaterialDespacho, datosFaltantes)
                    Throw New Exception("No se encontró información suficiente de costo o material para el centro indicado" & msjNotifiacion)
                ElseIf respuesta = 7 Then
                    Throw New Exception("Ocurrió un error inesperado durante el registro de información")
                Else
                    datosFaltantes = adminBD.SqlParametros("@datosFaltantes").Value.ToString
                    Me._valorDeclarado = adminBD.SqlParametros("@valorAsignado").Value
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cantidadLectura"></param>
        ''' <param name="material"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ValidarInsercionCantidad(ByVal cantidadLectura As Integer, Optional ByVal material As String = "") As Boolean
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim respuesta As Boolean = False

            Try
                adminBD.SqlParametros.Clear()
                adminBD.agregarParametroSQL("@idDespacho", Me.IdDespacho)
                If material = "" Then
                    adminBD.agregarParametroSQL("@material", DBNull.Value)
                Else
                    adminBD.agregarParametroSQL("@material", material, SqlDbType.VarChar)
                End If
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                respuesta = CBool(adminBD.ejecutarScalar("ValidarInsercionCantidad", CommandType.StoredProcedure))

            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try

            Return respuesta
        End Function

        Public Sub EliminarCaja(ByVal idCaja As String)
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim datosCaja As Array
            Dim idFactura As Integer = 0
            Dim region As String = ""
            Dim estiba As Integer = 0
            Dim caja As Integer = 0
            Dim respuesta As Integer = 0

            Try
                datosCaja = Split(idCaja, "-")
                idFactura = CInt(datosCaja(0))
                region = datosCaja(1)
                estiba = CInt(datosCaja(2))
                caja = CInt(datosCaja(3))
                adminBD.iniciarTransaccion()
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.agregarParametroSQL("@idFactura", idFactura)
                adminBD.agregarParametroSQL("@region", region, SqlDbType.VarChar)
                adminBD.agregarParametroSQL("@estiba", estiba)
                adminBD.agregarParametroSQL("@caja", caja)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.ejecutarNonQuery("EliminarCajaDespacho", CommandType.StoredProcedure)

                respuesta = adminBD.SqlParametros("@return_value").Value

                If respuesta = 1 Then
                    Throw New Exception("Uno o más seriales de la caja especificada no pertenecen al despacho")
                ElseIf respuesta = -1 Then
                    Throw New Exception("Ocurrió un error durante la eliminación, por favor intente nuevamente")
                Else
                    adminBD.confirmarTransaccion()
                End If
            Catch ex As Exception
                adminBD.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try
        End Sub

        Public Sub EliminarOTB(ByVal idOTB As String)
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim respuesta As Integer = 0

            Try
                adminBD.agregarParametroSQL("@idOTB", idOTB, SqlDbType.VarChar)
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.ejecutarNonQuery("EliminarOTBDespacho", CommandType.StoredProcedure)

                respuesta = adminBD.SqlParametros("@return_value").Value

                If respuesta = 1 Then
                    Throw New Exception("Uno o más seriales de la OTB especificada no pertenecen al despacho")
                ElseIf respuesta = -1 Then
                    Throw New Exception("Ocurrió un error durante la eliminación, por favor intente nuevamente")
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try
        End Sub

        Public Function EliminarRangoSims(ByVal simInicial As String, ByVal simFinal As String) As ResultadoProceso
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim resultado As New ResultadoProceso

            Try
                adminBD.agregarParametroSQL("@simInicial", simInicial, SqlDbType.VarChar)
                adminBD.agregarParametroSQL("@simFinal", simFinal, SqlDbType.VarChar)
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.iniciarTransaccion()
                adminBD.ejecutarNonQuery("EliminarRangoSims", CommandType.StoredProcedure)

                resultado.Valor = adminBD.SqlParametros("@return_value").Value


                If resultado.Valor = 0 Then
                    adminBD.confirmarTransaccion()
                Else
                    adminBD.abortarTransaccion()
                    If resultado.Valor = 1 Then
                        resultado.Mensaje = "No existen en el Despacho seriales dentro del Rango proporcionado, por favor verifique"
                    Else
                        resultado.Mensaje = "Ocurrió un Error inesperado al intentar eliminar los datos, por favor intente nuevamente"
                    End If
                End If

            Catch ex As Exception
                adminBD.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try
            Return resultado
        End Function

        Public Sub EliminarSerialSuelto(ByVal serial As String)
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Dim respuesta As Integer = 0

            Try
                adminBD.agregarParametroSQL("@serial", serial, SqlDbType.VarChar)
                adminBD.agregarParametroSQL("@idDespacho", Me._idDespacho)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.iniciarTransaccion()
                adminBD.ejecutarNonQuery("EliminarSerialSueltoDespacho", CommandType.StoredProcedure)
                respuesta = adminBD.SqlParametros("@return_value").Value

                If respuesta = 1 Then
                    Throw New Exception("El serial especificado no pertenece al despacho")
                ElseIf respuesta = -1 Then
                    Throw New Exception("Ocurrió un error durante la eliminación, por favor intente nuevamente")
                End If

                adminBD.confirmarTransaccion()
            Catch ex As Exception
                adminBD.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try
        End Sub

        Public Sub ActualizarInformacionDespachoCerrados(ByVal idUsuario As Integer)
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Try
                With adminBD
                    .agregarParametroSQL("@idDespacho", Me._idDespacho, SqlDbType.Int)
                    .agregarParametroSQL("@peso", Me._peso, SqlDbType.Float)
                    .agregarParametroSQL("@volumen", Me._volumen, SqlDbType.Float)
                    .agregarParametroSQL("@cantidadCajas", Me._cantidadCajas, SqlDbType.Int)
                    .agregarParametroSQL("@idTipoUnidad", Me._tipoUnidad.IdTipoUnidad, SqlDbType.Int)
                    .agregarParametroSQL("@contenido", Me._contenido, SqlDbType.VarChar)
                    .agregarParametroSQL("@idUsuario", idUsuario, SqlDbType.Int)

                    adminBD.ejecutarNonQuery("ActualizarInformacionDespachoCerrados", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
               Throw New Exception(ex.Message)
            Finally
                adminBD.Dispose()
            End Try
        End Sub
#End Region

#Region "Métodos Compartidos"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="material"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ComprobarTipoMaterial(ByVal material As String) As TipoMaterial
            Dim respuesta As Integer
            Dim oTipoMaterial As New TipoMaterial
            Dim adminBD As New LMDataAccessLayer.LMDataAccess
            Try
                With adminBD
                    .agregarParametroSQL("@material", material)
                    .SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarReader("ComprobarTipoMaterial", CommandType.StoredProcedure)
                    respuesta = adminBD.SqlParametros("@return_value").Value
                    If respuesta = 0 Then
                        If .Reader IsNot Nothing AndAlso .Reader.HasRows AndAlso .Reader.Read Then
                            oTipoMaterial.idTipoProducto = .Reader("idTipoProducto")
                            oTipoMaterial.nombreTablaLectura = .Reader("tablaLectura")
                        Else
                            Throw New Exception("No se encontró información de tipo de producto para el material " & material)
                        End If
                    Else
                        Throw New Exception("No fue posible consultar tipo de producto del material " & material)
                    End If
                End With
            Catch ex As Exception
                Throw New Exception("Se generó un error consultar tipo de producto del material " & material & "." & ex.Message)
            End Try

            Return oTipoMaterial
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPedido"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExisteDespacho(ByVal idPedido As Integer) As Integer
            Dim respuesta As Integer = 0
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                adminBD.agregarParametroSQL("@idPedido", idPedido)
                adminBD.SqlParametros.Add("@return_value", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                adminBD.ejecutarNonQuery("VerificarExisteDespacho", CommandType.StoredProcedure)

                respuesta = adminBD.SqlParametros("@return_value").Value
            Catch ex As Exception
                Throw New Exception("Ocurrió un error al tratar de comprobar los datos de despacho")
            End Try

            Return respuesta
        End Function

        ''' <summary>
        ''' Verifica el tipo de un material para saber qué tipo de inserción realizar
        ''' </summary>
        ''' <param name="serial">Serial al cual se le va a verificar el material</param>
        ''' <returns>Tipo de material específico que tiene el serial</returns>
        ''' <remarks>Los tipos de material que se retornan son; Serial = 1, Sim = 2, Tarjeta Prepago = 3</remarks>
        Public Shared Function VerificarTipoSerial(ByVal serial As String, Optional ByVal idPedido As Integer = 0) As TipoMaterial
            Dim respuesta As New TipoMaterial
            Dim adminBD As New LMDataAccessLayer.LMDataAccess

            Try
                With adminBD
                    .agregarParametroSQL("@serial", serial, SqlDbType.VarChar, 20)
                    .agregarParametroSQL("@idPedido", idPedido, SqlDbType.Int)
                    .ejecutarReader("VerificarTipoSerial", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.HasRows AndAlso .Reader.Read Then
                        respuesta.idTipoProducto = .Reader("idTipo")
                        respuesta.nombreTablaLectura = .Reader("tabla")
                    Else
                        Throw New Exception("No se encontro tipo de producto del serial.")
                    End If
                End With

            Catch ex As Exception
                Throw New Exception("Ocurrió un error al tratar de comprobar el tipo de producto del serial " & serial & " " & ex.Message)
            End Try
            Return respuesta
        End Function

        Public Shared Function SeleccionarPrecinto(ByVal idDespacho As Integer, Optional ByVal numeroEntrega As Long = 0) As DataTable
            Dim dm As New LMDataAccess
            Dim dtDatos As New DataTable
            Try
                With dm
                    .SqlParametros.Add("@idDespacho", SqlDbType.Int).Value = idDespacho
                    If numeroEntrega <> 0 Then .SqlParametros.Add("@numeroEntrega", SqlDbType.BigInt).Value = numeroEntrega
                    .SqlParametros.Add("@returnResultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    dtDatos = .ejecutarDataTable("ObtenerListaPrecinto", CommandType.StoredProcedure)

                    If CType(Val(.SqlParametros("@returnResultado").Value), Integer) = 1 Then
                        Throw New Exception("La transportadora asociada al despacho o entrega no usa precinto.")
                    End If
                End With
                Return dtDatos
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If dm IsNot Nothing Then dm.Dispose()
            End Try

        End Function
#End Region

    End Class
End Namespace

