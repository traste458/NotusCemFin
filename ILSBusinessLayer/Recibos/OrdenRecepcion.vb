Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Recibos
    Public Class OrdenRecepcion

#Region "variables"
        Private _idOrdenRecepcion As Long
        Private _idTipoProducto As Integer
        Private _idTipoRecepcion As Integer
        Private _idOrdenCompra As Long
        Private _remision As String
        Private _fechaRecepcion As Date
        Private _idCreador As Long
        Private _idEstado As Integer
        Private _estado As Estado
        Private _tipoProducto As String
        Private _tipoRecepcion As String
        Private _numeroOrdenCompra As String
        Private _idFacturaGuia As Long
        Private _idProveedor As Integer
        Private _proveedor As String
        Private _factura As String
        Private _guia As String
        Private _idConsignatario As Integer
        Private _idDistribuidor As Long
        Private _distribuidor As String
        Private _idTransportadora As Integer
        Private _transportadora As String
        Private _numeroCargue As Long
        Private _consignatario As Consignatario
        Private _idClienteExterno As Integer
        Private _clienteExterno As Comunes.ClienteExterno
        Private _ordenCompra As OrdenCompra
        Private _material As DataTable
        Private _error As String
        Private _materialCargado As Boolean
        Private Shared _perfilesReimprimirViajera As New ArrayList
        Private _dtImagenes As DataTable
        Private _nombreProductos As String
        Private _cantidad As Integer
        Private _piezas As String
        Private _pesoGuia As String
        Private _pesoRecibido As String
        Private _diferenciaPeso As String
        Private _listImagenes As List(Of Imagen)
        Private _dtrecepcion As DataTable
        Public Enum EstadoOrden
            Cancelada = 15
            Abierta = 16
            Parcial = 17
            Finalizada = 18
        End Enum

#End Region

#Region "propiedades"

        Public Property IdOrdenRecepcion() As Long
            Get
                Return _idOrdenRecepcion
            End Get
            Set(ByVal value As Long)
                _idOrdenRecepcion = value
            End Set
        End Property

        Public Property IdTipoProducto() As Integer
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Integer)
                _idTipoProducto = value
            End Set
        End Property

        Public Property IdTipoRecepcion() As Integer
            Get
                Return _idTipoRecepcion
            End Get
            Set(ByVal value As Integer)
                _idTipoRecepcion = value
            End Set
        End Property

        Public Property IdOrdenCompra() As Long
            Get
                Return _idOrdenCompra
            End Get
            Set(ByVal value As Long)
                _idOrdenCompra = value
            End Set
        End Property

        Public Property Remision() As String
            Get
                Return _remision
            End Get
            Set(ByVal value As String)
                _remision = value
            End Set
        End Property

        Public Property FechaRecepcion() As Date
            Get
                Return _fechaRecepcion
            End Get
            Set(ByVal value As Date)
                _fechaRecepcion = value
            End Set
        End Property

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
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

        Public ReadOnly Property Estado() As Estado
            Get
                If _estado Is Nothing Then _estado = New Estado(_idEstado)
                Return _estado
            End Get
        End Property

        Public Property IdProveedor() As Integer
            Get
                Return _idProveedor
            End Get
            Set(ByVal value As Integer)
                _idProveedor = value
            End Set
        End Property

        Public ReadOnly Property Proveedor() As String
            Get
                Return _proveedor
            End Get
        End Property

        Public ReadOnly Property TipoProducto() As String
            Get
                Return _tipoProducto
            End Get
        End Property

        Public ReadOnly Property TipoRecepcion() As String
            Get
                Return _tipoRecepcion
            End Get
        End Property

        Public ReadOnly Property NumeroOrdenCompra() As String
            Get
                Return _numeroOrdenCompra
            End Get
        End Property

        Public ReadOnly Property OrdenCompra() As OrdenCompra
            Get
                If _ordenCompra Is Nothing Then _ordenCompra = New OrdenCompra(_idOrdenCompra)
                Return _ordenCompra
            End Get
        End Property

        Public Property IdFacturaGuia() As Long
            Get
                Return _idFacturaGuia
            End Get
            Set(ByVal value As Long)
                _idFacturaGuia = value
            End Set
        End Property

        Public Property Factura() As String
            Get
                Return _factura
            End Get
            Set(ByVal value As String)
                _factura = value
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

        Public Property IdConsignatario() As Integer
            Get
                Return _idConsignatario
            End Get
            Set(ByVal value As Integer)
                _idConsignatario = value
            End Set
        End Property

        Public ReadOnly Property Consignatario() As Consignatario
            Get
                If Me._consignatario Is Nothing Then Me._consignatario = New Consignatario(_idConsignatario)
                Return Me._consignatario
            End Get
        End Property

        Public Property IdDistribuidor As Long
            Get
                Return _idDistribuidor
            End Get
            Set(value As Long)
                _idDistribuidor = value
            End Set
        End Property

        Public Property Distribuidor As String
            Get
                Return _distribuidor
            End Get
            Set(value As String)
                _distribuidor = value
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

        Public Property Transportadora As String
            Get
                Return _transportadora
            End Get
            Set(value As String)
                _transportadora = value
            End Set
        End Property

        Public Property NumeroCargue As Long
            Get
                Return _numeroCargue
            End Get
            Set(value As Long)
                _numeroCargue = value
            End Set
        End Property

        Public Property IdClienteExterno() As Integer
            Get
                Return _idClienteExterno
            End Get
            Set(ByVal value As Integer)
                _idClienteExterno = value
            End Set
        End Property

        Public ReadOnly Property ClienteExterno() As Comunes.ClienteExterno
            Get
                If Me._clienteExterno Is Nothing Then Me._clienteExterno = New Comunes.ClienteExterno(_idClienteExterno)
                Return _clienteExterno
            End Get
        End Property

        Public ReadOnly Property Material() As DataTable
            Get
                If _idOrdenRecepcion > 0 AndAlso (Not _materialCargado) Then CargarMateriales()
                Return _material
            End Get
        End Property

        Public ReadOnly Property InfoError() As String
            Get
                Return _error
            End Get
        End Property

        ''' <summary>
        ''' Validación de reimpresión solo para perfiles de Supervisor, Jefe y Gerente de Operaciones
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared ReadOnly Property PerfilesReimprimirViajera() As ArrayList
            Get
                With _perfilesReimprimirViajera
                    .Add(92)
                    .Add(6)
                    .Add(55)
                End With
                Return _perfilesReimprimirViajera
            End Get
        End Property

		Public Property dtImagenes() As DataTable
            Get
                Return _dtImagenes
            End Get
            Set(value As DataTable)
                _dtImagenes = value
            End Set
        End Property

        Public Property NombreProductos() As String
            Get
                Return _nombreProductos
            End Get
            Set(value As String)
                _nombreProductos = value
            End Set
        End Property

        Public Property Cantidad() As Integer
            Get
                Return _cantidad
            End Get
            Set(value As Integer)
                _cantidad = value
            End Set
        End Property

        Public Property Piezas() As String
            Get
                Return _piezas
            End Get
            Set(value As String)
                _piezas = value
            End Set
        End Property

        Public Property PesoGuia() As String
            Get
                Return _pesoGuia
            End Get
            Set(value As String)
                _pesoGuia = value
            End Set
        End Property

        Public Property PesoRecibido() As String
            Get
                Return _pesoRecibido
            End Get
            Set(value As String)
                _pesoRecibido = value
            End Set
        End Property

        Public Property DiferenciaPeso() As String
            Get
                Return _diferenciaPeso
            End Get
            Set(value As String)
                _diferenciaPeso = value
            End Set
        End Property

        Public Property ListaImagenes As List(Of Imagen)
            Get
                If IsNothing(_listImagenes) Then CargarImagenes()
                Return _listImagenes
            End Get
            Set(value As List(Of Imagen))
                _listImagenes = value
            End Set
        End Property

        Public Property dtRecepcion() As DataTable
            Get
                Return _dtrecepcion
            End Get
            Set(value As DataTable)
                _dtrecepcion = value
            End Set
        End Property
#End Region

#Region "constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idOrdenRecepcion As Long)
            Me.New()
            Me.CargarDatos(idOrdenRecepcion)
            _idOrdenRecepcion = idOrdenRecepcion
        End Sub

#End Region

#Region "metodos privados"

        Private Sub CargarDatos(ByVal idOrdenRecepcion As Long)
            Dim db As New LMDataAccess
            db.SqlParametros.Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = idOrdenRecepcion
            Try
                db.ejecutarReader("ObtenerOrdenRecepcion", CommandType.StoredProcedure)
                If db.Reader.Read Then
                    _idOrdenRecepcion = db.Reader("idOrdenRecepcion")
                    _idTipoProducto = db.Reader("idTipoProducto")
                    _idTipoRecepcion = db.Reader("idTipoRecepcion")
                    _remision = db.Reader("remision").ToString()
                    _fechaRecepcion = db.Reader("fechaRecepcion")
                    _idCreador = db.Reader("idCreador")
                    _tipoProducto = db.Reader("tipoProducto")
                    _tipoRecepcion = db.Reader("tipoRecepcion")
                    _numeroOrdenCompra = db.Reader("numeroOrden").ToString
                    _idOrdenCompra = IIf(db.Reader("idOrdenCompra").ToString = String.Empty, 0, db.Reader("idOrdenCompra"))
                    _idEstado = db.Reader("idEstado")
                    _idFacturaGuia = IIf(db.Reader("idFacturaGuia").ToString = String.Empty, 0, db.Reader("idFacturaGuia"))
                    _idProveedor = IIf(db.Reader("idProveedor").ToString = String.Empty, 0, db.Reader("idProveedor"))
                    _proveedor = db.Reader("proveedor").ToString
                    _factura = db.Reader("factura").ToString
                    _guia = db.Reader("guia").ToString
                    Integer.TryParse(db.Reader("idConsignatario").ToString, _idConsignatario)
                    Integer.TryParse(db.Reader("idClienteExterno").ToString, _idClienteExterno)
                    Long.TryParse(db.Reader("idDistribuidor").ToString, _idDistribuidor)
                    _distribuidor = (db.Reader("distribuidor")).ToString
                    Integer.TryParse(db.Reader("idTransportadora"), _idTransportadora)
                    _transportadora = (db.Reader("transporta")).ToString
                End If
            Catch ex As Exception
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub

        ''' <summary>
        ''' Carga los materiales de recepción actual.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CargarMateriales()
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                    _material = .ejecutarDataTable("ObtenerInfoMaterialOrdenRecepcion", CommandType.StoredProcedure)
                    _materialCargado = True
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Function RegistrarImagenes(ByVal dbManager As LMDataAccess) As Short
            Dim respuesta As Short
            Dim i As Integer
            If dbManager IsNot Nothing Then
                For i = 0 To _dtImagenes.Rows.Count - 1
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@ordenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .SqlParametros.Add("@imagen", SqlDbType.VarBinary).Value = _dtImagenes.Rows(i).Item("imagen")
                        .SqlParametros.Add("@contentType", SqlDbType.VarChar).Value = _dtImagenes.Rows(i).Item("contenType")
                        .SqlParametros.Add("@nombreImagen", SqlDbType.VarChar).Value = _dtImagenes.Rows(i).Item("nombre")
                        .SqlParametros.Add("@tamanio", SqlDbType.Int).Value = CInt(_dtImagenes.Rows(0).Item("peso"))
                        .ejecutarNonQuery("RegistrarImagenRecepcion", CommandType.StoredProcedure)
                    End With
                Next
            End If
            Return respuesta
        End Function
#End Region

#Region "metodos publicos"

        Public Function Crear() As Boolean
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean
            With dbManager
                With .SqlParametros
                    .Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
                    .Add("@idTipoRecepcion", SqlDbType.Int).Value = _idTipoRecepcion
                    .Add("@idOrdenCompra", SqlDbType.Int).IsNullable = True
                    .Item("@idOrdenCompra").Value = IIf(_idOrdenCompra > 0, _idOrdenCompra, DBNull.Value)
                    .Add("@remision", SqlDbType.VarChar).IsNullable = True
                    .Item("@remision").Value = IIf(_remision <> String.Empty, _remision, DBNull.Value)
                    .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                    .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@idFacturaGuia", SqlDbType.Int).IsNullable = True
                    .Item("@idFacturaGuia").Value = IIf(_idFacturaGuia > 0, _idFacturaGuia, DBNull.Value)
                    .Add("@idProveedor", SqlDbType.Int).IsNullable = True
                    .Item("@idProveedor").Value = IIf(_idProveedor > 0, IdProveedor, DBNull.Value)
                    .Add("@factura", SqlDbType.VarChar).IsNullable = True
                    .Item("@factura").Value = IIf(_factura <> "", _factura, DBNull.Value)
                    .Add("@guia", SqlDbType.VarChar).IsNullable = True
                    .Item("@guia").Value = IIf(_guia <> "", _guia, DBNull.Value)
                    .Add("@idConsignatario", SqlDbType.Int).Value = _idConsignatario
                    .Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                    If _idDistribuidor > 0 Then .Add("@idDistribuidor", SqlDbType.BigInt).Value = _idDistribuidor
                    If _idTransportadora > 0 Then .Add("@idTransportadora", SqlDbType.Int).Value = _idTransportadora
                    .Add("@identity", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .ejecutarNonQuery("CrearOrdenRecepcion", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _idOrdenRecepcion = CLng(.SqlParametros("@identity").Value)
                        retorno = True
                    Else
                        Throw New Exception("Imposible registrar la información de la Orden en la Base de Datos.")
                    End If
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End With
            Return retorno
        End Function

        Public Sub Actualizar()
            If _idOrdenRecepcion > 0 Then
                Dim db As New LMDataAccessLayer.LMDataAccess
                Actualizar(db)
            Else
                Throw New DuplicateNameException("La Orden de recepcion aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub

        Public Sub Actualizar(ByVal db As LMDataAccess)
            If _idOrdenRecepcion > 0 AndAlso db IsNot Nothing Then
                Try
                    With db.SqlParametros
                        .Clear()
                        .Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
                        .Add("@idTipoRecepcion", SqlDbType.Int).Value = _idTipoRecepcion
                        .Add("@idOrdenCompra", SqlDbType.Int).IsNullable = True
                        .Item("@idOrdenCompra").Value = IIf(_idOrdenCompra > 0, _idOrdenCompra, DBNull.Value)
                        .Add("@remision", SqlDbType.VarChar).IsNullable = True
                        .Item("@remision").Value = IIf(_remision <> String.Empty, _remision, DBNull.Value)
                        .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        .Add("@idFacturaGuia", SqlDbType.Int).IsNullable = True
                        .Item("@idFacturaGuia").Value = IIf(_idFacturaGuia > 0, _idFacturaGuia, DBNull.Value)
                        .Add("@idProveedor", SqlDbType.Int).IsNullable = True
                        .Item("@idProveedor").Value = IIf(_idProveedor > 0, _idProveedor, DBNull.Value)
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@factura", SqlDbType.VarChar).IsNullable = True
                        .Item("@factura").Value = IIf(_factura <> "", _factura, DBNull.Value)
                        .Add("@guia", SqlDbType.VarChar).IsNullable = True
                        .Item("@guia").Value = IIf(_guia <> "", _guia, DBNull.Value)
                        .Add("@idConsignatario", SqlDbType.Int).Value = _idConsignatario
                        .Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                        If _idDistribuidor > 0 Then .Add("@idDistribuidor", SqlDbType.BigInt).Value = _idDistribuidor
                        If _idTransportadora > 0 Then .Add("@idTransportadora", SqlDbType.Int).Value = _idTransportadora
                        If _numeroCargue > 0 Then .Add("@numeroCargue", SqlDbType.BigInt).Value = _numeroCargue
                    End With
                    db.ejecutarNonQuery("ActualizarOrdenRecepcion", CommandType.StoredProcedure)
                    RegistrarImagenes(db)
                    Me.CargarDatos(_idOrdenRecepcion)
                Catch ex As Exception
                    If db.estadoTransaccional Then db.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    db.cerrarConexion()
                End Try
            Else
                Throw New DuplicateNameException("La Orden de recepcion aún no ha sido registrada en la Base de Datos.")
            End If
        End Sub

        Public Function ExisteRemision(ByVal numeroRemision As String) As Boolean
            Dim retorno As Boolean = False
            Dim filtroOrdenRecepcion As New Estructuras.FiltroOrdenRecepcion
            Dim dtRespuesta As New DataTable
            filtroOrdenRecepcion.Remision = numeroRemision
            dtRespuesta = ObtenerListado(filtroOrdenRecepcion)
            If dtRespuesta.Rows.Count > 0 Then
                retorno = True
            End If
            Return retorno
        End Function

        ''' <summary>
        ''' Verifica que la orden de recepción cumpla con las condiciones de cargue en SAP para producto nacional
        ''' </summary>
        ''' <returns>Verdadero en caso de cumplir condiciones de cargue Falso de lo contrario.</returns>
        ''' <remarks></remarks>
        Public Function CumpleCondicionesCargueProductoNacionalSAP() As Boolean
            Dim retorno = True
            Try
                If Not _idOrdenCompra > 0 Then
                    _error = "La orden de recepción no tiene una orden de compra asociada. "
                    retorno = False                
                ElseIf _idTipoProducto = Productos.TipoProducto.Tipo.HANDSETS Or _idTipoProducto = Productos.TipoProducto.Tipo.SIM_CARDS Then
                    _error &= "El tipo de producto de la orden de recepción no esta permitido para este cargue. "
                    retorno = False
                End If
                Return retorno
            Catch ex As Exception
                _error = ex.Message
                Throw New Exception(ex.Message)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las regiones asociadas a una orden de recepción
        ''' </summary>
        ''' <returns>DataTable de regiones para una orden de recepción</returns>
        ''' <remarks></remarks>
        Public Function ObtenerRegiones() As DataTable
            Dim dt As New DataTable
            Dim db As New LMDataAccess
            Try
                If _idOrdenRecepcion > 0 Then
                    With db
                        .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        dt = .ejecutarDataTable("ObtenerRegionesOrdenRecepcion", CommandType.StoredProcedure)
                    End With
                End If
                Return dt
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

        Public Function ObtenerCantidadRecibida() As Integer
            Try
                Dim cantidadRecibida As Integer                
                Dim dtDetalleRecepcionOrdenCompra As New DataTable
                Dim filtroDetalleOrden As New Estructuras.FiltroDetalleOrdenCompra
                If _idOrdenCompra > 0 Then
                    filtroDetalleOrden.IdOrden = _idOrdenCompra                    
                    dtDetalleRecepcionOrdenCompra = OrdenCompra.ObtenerDetalleRecepcion(_idOrdenCompra)
                    Integer.TryParse(dtDetalleRecepcionOrdenCompra.Compute("SUM(cantidadRecibida)", "").ToString, cantidadRecibida)
                Else
                    Throw New Exception("Falta el identificador de la orden de compra")
                End If
                Return cantidadRecibida
            Catch ex As Exception
                Me._error = ex.Message
                Throw New Exception(ex.Message)
            End Try
        End Function

        Public Function ObtenerCantidadRecibidaSinOrden() As Integer
            Try
                Dim dbManager As New LMDataAccess
                Dim cantidadRecibida As Integer
                Dim dtDetalleRecepcion As New DataTable
                If _idOrdenRecepcion > 0 Then
                    With dbManager
                        .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        dtDetalleRecepcion = .ejecutarDataTable("ObtenerDetalleRecepcionOrdenCompra", CommandType.StoredProcedure)
                    End With                    
                    Integer.TryParse(dtDetalleRecepcion.Compute("SUM(cantidadRecibida)", "").ToString, cantidadRecibida)
                Else
                    Throw New Exception("Falta el identificador de la orden de recepción")
                End If
                Return cantidadRecibida
            Catch ex As Exception
                Me._error = ex.Message
                Throw New Exception(ex.Message)
            Finally
            End Try
        End Function

        Public Function ObtenerCantidadObjetivo() As Integer
            Try
                Dim cantidadObjetivo As Integer
                Dim dtDetalleOrdenCompra As New DataTable                
                Dim filtroDetalleOrden As New Estructuras.FiltroDetalleOrdenCompra
                If _idOrdenCompra > 0 Then
                    filtroDetalleOrden.IdOrden = _idOrdenCompra
                    dtDetalleOrdenCompra = DetalleOrdenCompra.ObtenerListado(filtroDetalleOrden)                    
                    Integer.TryParse(dtDetalleOrdenCompra.Compute("SUM(cantidad)", "").ToString(), cantidadObjetivo)                    
                Else
                    Throw New Exception("Falta el identificador de la orden de compra")
                End If
                Return cantidadObjetivo
            Catch ex As Exception
                Me._error = ex.Message
                Throw New Exception(ex.Message)
            End Try
        End Function

        Public Function ValidarConsecutivos(ByVal consecutivoInicial As String, ByVal consecutivoFinal As String, ByVal material As String, _
                                            ByVal idVersion As Integer, ByVal idRegion As Integer, idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                        .Add("@idProveedor", SqlDbType.Int).Value = _idProveedor
                        .Add("@consecutivoInicial", SqlDbType.VarChar, 50).Value = consecutivoInicial
                        .Add("@consecutivoFinal", SqlDbType.VarChar, 50).Value = consecutivoFinal
                        .Add("@material", SqlDbType.VarChar, 20).Value = material
                        .Add("@idVersion", SqlDbType.Int).Value = idVersion
                        .Add("@idRegion", SqlDbType.Int).Value = idRegion
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ValidarConsecutivosRecepcion", CommandType.StoredProcedure)
                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        .confirmarTransaccion()
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        resultado.Valor = .SqlParametros("@resultado").Value
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try

            Return resultado
        End Function

        Public Function ConsultarConsecutivosTemporales(Optional ByVal idCaja As Long = 0, Optional ByVal flag As Integer = 0) As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        If _idOrdenRecepcion > 0 Then .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                        If idCaja > 0 Then .Add("@idCaja", SqlDbType.BigInt).Value = idCaja
                        If flag > 0 Then .Add("@flag", SqlDbType.Int).Value = flag
                    End With
                    dtDatos = .ejecutarDataTable("ConsultarConsecutivosTemporales", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
                Throw New Exception(ex.Message)
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarConsecutivoPallet(Optional ByVal idPallet As Long = 0) As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        If idPallet > 0 Then .Add("@idPallet", SqlDbType.BigInt).Value = idPallet
                    End With
                    dtDatos = .ejecutarDataTable("ConsultarConsecutivosRegistrados", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
                Throw New Exception(ex.Message)
            End Try
            Return dtDatos
        End Function

        Public Function ConsultarConsecutivoOrdenRecepcion() As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                        If _idOrdenCompra > 0 Then .Add("@idOrdenCompra", SqlDbType.BigInt).Value = _idOrdenCompra
                    End With
                    dtDatos = .ejecutarDataTable("ConsultarRangosConsecutivosRegistrados", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
                Throw New Exception(ex.Message)
            End Try
            Return dtDatos
        End Function

        Public Function EliminarSerialesTemporales(ByVal consecutivoInicial As String, ByVal consecutivoFinal As String, ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                        .Add("@consecutivoInicial", SqlDbType.VarChar, 50).Value = consecutivoInicial
                        .Add("@consecutivoFinal", SqlDbType.VarChar, 50).Value = consecutivoFinal
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("EliminarSerialesTemporales", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        .confirmarTransaccion()
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        resultado.Valor = .SqlParametros("@resultado").Value
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ValidarCantidades() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("ValidarCantidadesConsecutivos", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        resultado.Valor = .SqlParametros("@resultado").Value
                    Else
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
                Throw New Exception(ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ValidarVersionMaterial() As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            With dbManager
                With .SqlParametros
                    .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                End With
                dtDatos = .ejecutarDataTable("ValidarVersionMaterial", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Function ValidarConsecutivosDevolucion(ByVal consecutivoInicial As String, ByVal consecutivoFinal As String, ByVal material As String) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                        .Add("@consecutivoInicial", SqlDbType.VarChar, 50).Value = consecutivoInicial
                        .Add("@consecutivoFinal", SqlDbType.VarChar, 50).Value = consecutivoFinal
                        .Add("@material", SqlDbType.VarChar, 20).Value = material
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .ejecutarNonQuery("ValidarConsecutivosDevolucion", CommandType.StoredProcedure)
                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        resultado.Valor = .SqlParametros("@resultado").Value
                    Else
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
                Throw New Exception(ex.Message)
            End Try

            Return resultado
        End Function

        Public Function RegistrarConsecutivosDevolucion(ByVal consecutivoInicial As String, ByVal consecutivoFinal As String, ByVal material As String, _
                                            ByVal fechaVersion As Integer, ByVal idRegion As Integer, idUsuario As Integer, ByVal idCaja As Long) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                        .Add("@consecutivoInicial", SqlDbType.VarChar, 50).Value = consecutivoInicial
                        .Add("@consecutivoFinal", SqlDbType.VarChar, 50).Value = consecutivoFinal
                        .Add("@fechaVersion", SqlDbType.Int).Value = fechaVersion
                        .Add("@idRegion", SqlDbType.Int).Value = idRegion
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@idCaja", SqlDbType.BigInt).Value = idCaja
                        .Add("@material", SqlDbType.VarChar, 20).Value = material
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarConsecutivosDevolucion", CommandType.StoredProcedure)
                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        .confirmarTransaccion()
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        resultado.Valor = .SqlParametros("@resultado").Value
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try

            Return resultado
        End Function

        Public Function ObtenerInformacionNotificacionPapeleria() As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInformacionNotificacionPapeleria", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function ObtenerFechaVersion() As DataTable
            Dim dtDatos As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    dtDatos = .ejecutarDataTable("ObtenerFechaVersion", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

       Public Function CargarImagenes()
            Using dbManager As New LMDataAccess
                _listImagenes = New List(Of Imagen)
                Try
                    With dbManager
                        .TiempoEsperaComando = 1200
                        .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                        .ejecutarReader("ObtenerImagenDeRecepcion", CommandType.StoredProcedure)

                        If .Reader IsNot Nothing Then
                            While .Reader.Read
                                Dim objImg As Imagen
                                objImg.imagen = .Reader("imagen")
                                objImg.contenType = .Reader("contentType")
                                objImg.nombreImagen = .Reader("nombreImagen")
                                objImg.tamanio = .Reader("tamanio")
                                _listImagenes.Add(objImg)
                            End While
                        End If
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return _listImagenes
        End Function

        Public Function ActualizarEstadoNotificacionRecepcion() As Short
            Dim respuesta As Short
            Dim i As Integer
            Dim dbManager As New LMDataAccess
            Try
                For i = 0 To _dtrecepcion.Rows.Count - 1
                    With dbManager
                        .SqlParametros.Clear()
                        If Not dtRecepcion.Rows(i).Item("notificada") Then
                            .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _dtrecepcion.Rows(i).Item("idOrdenRecepcion")
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            .iniciarTransaccion()
                            .ejecutarNonQuery("ActualizarEstadoNotificacionRecepcion", CommandType.StoredProcedure)
                            Integer.TryParse(.SqlParametros("@resultado").Value.ToString, respuesta)
                            If respuesta <> 0 Then
                                .abortarTransaccion()
                                Exit For
                            Else
                                .confirmarTransaccion()
                            End If
                        End If
                    End With
                Next
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            End Try
            Return respuesta
        End Function
#End Region

#Region "métodos compartidos"

        ''' <summary>
        ''' Obtiene los datos para el reporte de recepcion de producto utiliza el SP ObtenerReporteRecepcion
        ''' </summary>
        ''' <param name="filtro">Filtros que se aplican a la consulta</param>
        ''' <returns>Datos retornados por el filtro</returns>
        ''' <remarks></remarks>
        Public Shared Function ObtenerReporteRecepcion(ByVal filtro As FiltroReporteRecepcion) As DataTable
            Try
                Dim db As New LMDataAccess
                Dim dt As New DataTable
                With filtro
                    If .IdOrdenRecepcion > 0 Then db.SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = .IdOrdenRecepcion
                    If .IdOrdenCompra > 0 Then db.SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = .IdOrdenCompra
                    If .NumeroOrdenCompra <> "" Then db.SqlParametros.Add("@numeroOrdenCompra", SqlDbType.VarChar).Value = .NumeroOrdenCompra
                    If .IdTipoProducto > 0 Then db.SqlParametros.Add("@idTipoProducto", SqlDbType.Int).Value = .IdTipoProducto
                    If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                    If .FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                    If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal
					If .EstadoNotificacion <> Nothing Then
                        If .EstadoNotificacion = "0" Then
                            db.SqlParametros.Add("@estadoNotificacion", SqlDbType.Bit).Value = 0
                        Else
                            db.SqlParametros.Add("@estadoNotificacion", SqlDbType.Bit).Value = 1
                        End If
                    End If
                    dt = db.ejecutarDataTable("ObtenerReporteRecepcion", CommandType.StoredProcedure)
                End With
                Return dt
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroOrdenRecepcion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroOrdenRecepcion) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdOrdenRecepcion > 0 Then db.SqlParametros.Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = .IdOrdenRecepcion
                If .IdTipoProducto > 0 Then db.SqlParametros.Add("@idTipoProducto", SqlDbType.Int).Value = .IdTipoProducto
                If .IdTipoRecepcion > 0 Then db.SqlParametros.Add("@idTipoRecepcion", SqlDbType.Int).Value = .IdTipoRecepcion
                If .IdOrdenCompra > 0 Or .IdOrdenCompra = -1 Then db.SqlParametros.Add("@idOrdenCompra", SqlDbType.BigInt).Value = .IdOrdenCompra
                If .Remision <> String.Empty Then db.SqlParametros.Add("@remision", SqlDbType.VarChar).Value = .Remision
                If .IdCreador > 0 Then db.SqlParametros.Add("@idCreador", SqlDbType.Int).Value = .IdCreador
                If .IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = .IdEstado
                If .NumeroOrden <> "" Then db.SqlParametros.Add("@numeroOrden", SqlDbType.VarChar).Value = .NumeroOrden.ToString
                If .FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = .FechaInicial
                If .FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = .FechaFinal
                If .IdFacturaGuia > 0 Then db.SqlParametros.Add("@idFacturaGuia", SqlDbType.Int).Value = .IdFacturaGuia
                If .IdProveedor > 0 Then db.SqlParametros.Add("@idProveedor", SqlDbType.Int).Value = .IdProveedor
                If .ListaEstado IsNot Nothing AndAlso .ListaEstado.Count Then db.SqlParametros.Add("@listaEstado", SqlDbType.VarChar).Value = Join(.ListaEstado.ToArray, ",")
                If .ListaIdOrdenesRecepcion IsNot Nothing AndAlso .ListaIdOrdenesRecepcion.Count > 0 Then db.SqlParametros.Add("@listaIdOrdenesRecepcion", SqlDbType.VarChar).Value = Join(.ListaIdOrdenesRecepcion.ToArray, ",")
                If .ListaIdTipoRecepcion IsNot Nothing AndAlso .ListaIdTipoRecepcion.Count > 0 Then db.SqlParametros.Add("@listaIdTipoRecepcion", SqlDbType.VarChar).Value = Join(.ListaIdTipoRecepcion.ToArray, ",")
                If .ListaIdTipoProducto IsNot Nothing AndAlso .ListaIdTipoProducto.Count > 0 Then db.SqlParametros.Add("@listaIdTipoProducto", SqlDbType.VarChar).Value = Join(.ListaIdTipoProducto.ToArray, ",")
                If .Factura <> "" Then db.SqlParametros.Add("@factura", SqlDbType.VarChar).Value = .Factura.ToString
                If .Guia <> "" Then db.SqlParametros.Add("@guia", SqlDbType.VarChar).Value = .Guia.ToString
                If .IdConsignatario > 0 Then db.SqlParametros.Add("@idConsignatario", SqlDbType.Int).Value = .IdConsignatario
                If .idClienteExterno > 0 Then db.SqlParametros.Add("@idClienteExterno", SqlDbType.Int).Value = .idClienteExterno
                If .idDistribuidor > 0 Then db.SqlParametros.Add("@idDistribuidor", SqlDbType.BigInt).Value = .idDistribuidor
                If .idTrasportadora > 0 Then db.SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = .idTrasportadora
                dtDatos = db.ejecutarDataTable("ObtenerOrdenRecepcion", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos

        End Function

        Public Overloads Shared Function ObtenerListadoProducto(ByVal idOrdenRecepcion As Long, Optional ByVal idTipoDetalleProducto As Short = 1) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            db.SqlParametros.Add("@idTipoDetalleProducto", SqlDbType.Int).Value = idTipoDetalleProducto
            db.SqlParametros.Add("@idORdenRecepcion", SqlDbType.BigInt).Value = idOrdenRecepcion
            dtDatos = db.ejecutarDataTable("ObtenerProductoDesdeOrdenRecepcion", CommandType.StoredProcedure)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListadoProducto(ByVal listaOrdenRecepcion As ArrayList, Optional ByVal idTipoDetalleProducto As Short = 1) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            db.SqlParametros.Add("@idTipoDetalleProducto", SqlDbType.Int).Value = idTipoDetalleProducto
            db.SqlParametros.Add("@listaOrdenRecepcion", SqlDbType.VarChar).Value = Join(listaOrdenRecepcion.ToArray, ",")
            dtDatos = db.ejecutarDataTable("ObtenerProductoDesdeOrdenRecepcion", CommandType.StoredProcedure)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListadoDeOrdenCompra(ByVal idOrdenCompra As Long, Optional ByVal idTipoDetalleProducto As Short = 1) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            db.SqlParametros.Add("@idTipoDetalleProducto", SqlDbType.Int).Value = idTipoDetalleProducto
            db.SqlParametros.Add("@idOrdenCompra", SqlDbType.BigInt).Value = idOrdenCompra
            dtDatos = db.ejecutarDataTable("ObtenerProductoDesdeOrdenRecepcion", CommandType.StoredProcedure)
            Return dtDatos
        End Function
#End Region

#Region "Estructuras"

        Public Structure Imagen
            Dim imagen As Byte()
            Dim contenType As String
            Dim nombreImagen As String
            Dim tamanio As Integer
        End Structure

#End Region
    End Class
End Namespace