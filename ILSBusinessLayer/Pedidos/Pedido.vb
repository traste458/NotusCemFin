Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Productos
Imports ILSBusinessLayer.Comunes
Imports System.Data.SqlClient
Imports LMDataAccessLayer
Imports System.Text
Imports GemBox.Spreadsheet

Namespace Pedidos

    Public Class Pedido

#Region "Atributos"
        Protected Friend _idPedido As Integer
        Private _numeroPedido As Long
        Private _tipoPedido As TipoPedido
        Private _idCliente As Integer
        Private _cliente As String
        Private _idCiudad As Integer
        Private _ciudad As String
        Private _idEstado As EstadoPedido
        Private _estado As String
        Protected Friend _idUsuario As Integer
        Private _usuario As String
        Private _detalle As DataTable
        Private _seriales As DataTable
        Private _detalleAuxiliar As DataTable
        Private _idPicking As Integer
        Private _idDespacho As Integer
        Private _ordenCompra As String
        Private _fechaCreacion As String
        Private _infoBodega As Estructuras.InfoBodegas
        Private _idSolicitante As Integer
        Private _solicitante As String
        Private _centroCosto As String
        Protected Friend _observaciones As String
        Private _serial As String
        Private _tipoLectura As Integer
        Private _idDetallePedido As Integer
        Private _cuarentenasDespacho As DataTable
        Private _detalleLiberacionDespacho As DataTable
        Private _numeroEntrega As Long
        Private _tipoDespacho As Despachos.TipoDespacho
        Private _sincronizado As Boolean
        Private _infoDespacho As Despachos.Despacho
        Private _idCiudadDestino As Integer
        Private _direccionDestino As String
        Private _ciudadDestino As String
        Private _departamentoDestino As String
        Private _idTipoTransporte As Integer
        Private _idTransportadora As Integer
        Private _transportadora As String
        Private _tipoTransporte As String
        Private _tipoAlistamiento As String
        Private _tipoDestinatario As String
        Private _contabilizarSAP As Boolean
        Private _infoErrores As DataTable ' Errores seriales de cuarentena
        Private _infoSerialesCuarentena As DataTable 'Seriales de cuarentena
        Private _infoSerialEspecial As DataTable 'Seriales de pedidos especiales
        Private _infoMaterialEspecial As DataTable 'Detalle de pedidos especiales por cantidad
        Private _idTipoProductoPrioridadDeDespacho As Integer
        Private _resultado As ResultadoProceso
        Private _documentoCambioEstadoSAP As String
        Private _soloBloqueo As Boolean
        Private oExcel As ExcelFile
        Private _tablaDatosMaterialSerial As DataTable
        Private _tablaDatosMaterialCantidad As DataTable
        Private _tablaErrores As DataTable
        Private _tablaDatosPedidoServicioTecnico As DataTable
        Private _infoPedidoServicioTecnico As DataTable 'Seriales de pedidos Servicio Tecnico
        Private _dsDatos As DataSet
        Private _dtDatos As DataTable
        Private _dtDatosGeneral As DataTable
        Private _dtDetalle As DataTable
        Private _referencia As String
        Private _tipoReferencia As Integer

#End Region

#Region "Propiedades"

        Public ReadOnly Property ContabilizarSAP() As Boolean
            Get
                Return _contabilizarSAP
            End Get

        End Property

        Public Property IdPedido() As Integer
            Get
                Return _idPedido
            End Get
            Set(ByVal value As Integer)
                _idPedido = value
            End Set
        End Property

        Public Property NumeroPedido() As Long
            Get
                Return _numeroPedido
            End Get
            Set(ByVal value As Long)
                _numeroPedido = value
            End Set
        End Property

        Public Property NumeroEntrega() As Long
            Get
                Return _numeroEntrega
            End Get
            Set(ByVal value As Long)
                _numeroEntrega = value
            End Set
        End Property

        Public Property Tipo() As TipoPedido
            Get
                Return _tipoPedido
            End Get
            Set(ByVal value As TipoPedido)
                _tipoPedido = value
            End Set
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

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public ReadOnly Property Usuario() As String
            Get
                Return _usuario
            End Get
        End Property

        Public Property IdCliente() As Integer
            Get
                Return _idCliente
            End Get
            Set(ByVal value As Integer)
                _idCliente = value
            End Set
        End Property

        Public ReadOnly Property Cliente() As String
            Get
                Return _cliente
            End Get
        End Property

        Public ReadOnly Property IdCiudad() As Integer
            Get
                Return _idCiudad
            End Get
        End Property

        Public ReadOnly Property Ciudad() As String
            Get
                Return _ciudad
            End Get
        End Property

        Public ReadOnly Property Detalle() As DataTable
            Get
                If _detalle Is Nothing Then CargarDetalle()
                Return _detalle
            End Get

        End Property

        Public ReadOnly Property Seriales() As DataTable
            Get
                If _seriales Is Nothing Then CargarSeriales()
                Return _seriales
            End Get
        End Property

        Public ReadOnly Property SerialesServicioTecnico() As DataTable
            Get
                If _seriales Is Nothing Then CargarSerialesServicioTecnico()
                Return _seriales
            End Get
        End Property

        Public Property DetalleAuxiliar() As DataTable
            Get
                Return _detalleAuxiliar
            End Get
            Set(ByVal value As DataTable)
                _detalleAuxiliar = value
            End Set
        End Property

        Public ReadOnly Property FechaCreacion() As String
            Get
                Return _fechaCreacion
            End Get
        End Property

        Public ReadOnly Property IdPicking() As Integer
            Get
                Return _idPicking
            End Get
        End Property

        Public Property IdDespacho() As Integer
            Get
                Return _idDespacho
            End Get
            Set(ByVal value As Integer)
                _idDespacho = value
            End Set
        End Property

        Public Property OrdenCompra() As String
            Get
                Return _ordenCompra
            End Get
            Set(ByVal value As String)
                _ordenCompra = value
            End Set
        End Property

        Public Property IdSolicitante() As Integer
            Get
                Return _idSolicitante
            End Get
            Set(ByVal value As Integer)
                _idSolicitante = value
            End Set
        End Property

        Public ReadOnly Property Solicitante() As String
            Get
                Return _solicitante
            End Get
        End Property

        Public Property CentroCosto() As String
            Get
                Return _centroCosto
            End Get
            Set(ByVal value As String)
                _centroCosto = value
            End Set
        End Property

        Public Property Observaciones() As String
            Get
                Return _observaciones
            End Get
            Set(ByVal value As String)
                _observaciones = value
            End Set
        End Property

        Public Property Serial() As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property

        Public Property TipoLectura() As Integer
            Get
                Return _tipoLectura
            End Get
            Set(ByVal value As Integer)
                _tipoLectura = value
            End Set
        End Property

        Public Property IdDetallePedido() As Integer
            Get
                Return _idDetallePedido
            End Get
            Set(ByVal value As Integer)
                _idDetallePedido = value
            End Set
        End Property

        Public Property CuarentenasDespacho() As DataTable
            Get
                If _cuarentenasDespacho Is Nothing AndAlso _tipoPedido.IdTipo = 6 Then DetalleCuarentenasDespacho()
                Return _cuarentenasDespacho
            End Get
            Set(ByVal value As DataTable)
                _cuarentenasDespacho = value
            End Set
        End Property

        Public Property DetalleLiberacionDespacho() As DataTable
            Get
                If _detalleLiberacionDespacho Is Nothing Then CargarDetalleCuarentenaParaLiberacion()
                Return _detalleLiberacionDespacho
            End Get
            Set(ByVal value As DataTable)
                _detalleLiberacionDespacho = value
            End Set
        End Property

        Public Property TipoDespacho() As Despachos.TipoDespacho
            Get
                Return _tipoDespacho
            End Get
            Set(ByVal value As Despachos.TipoDespacho)
                _tipoDespacho = value
            End Set
        End Property

        Public Property Sincronizado() As Boolean
            Get
                Return _sincronizado
            End Get
            Set(ByVal value As Boolean)
                _sincronizado = value
            End Set
        End Property

        Public ReadOnly Property InfoDespacho() As Despachos.Despacho
            Get
                If _idDespacho <> 0 Then CargarInformacionDespacho()
                Return _infoDespacho
            End Get
        End Property

        Public Property DireccionDestino() As String
            Get
                Return _direccionDestino
            End Get
            Set(ByVal value As String)
                _direccionDestino = value
            End Set
        End Property

        Public Property IdCiudadDestino() As Integer
            Get
                Return _idCiudadDestino
            End Get
            Set(ByVal value As Integer)
                _idCiudadDestino = value
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

        Public Property DepartamentoDestino() As String
            Get
                Return _departamentoDestino
            End Get
            Set(ByVal value As String)
                _departamentoDestino = value
            End Set
        End Property

        Public ReadOnly Property IdTransportadora() As Integer
            Get
                Return _idTransportadora
            End Get
        End Property

        Public ReadOnly Property idTipoTransporte() As Integer
            Get
                Return _idTipoTransporte
            End Get
        End Property

        Public ReadOnly Property TipoAlistamiento() As String
            Get
                Return _tipoAlistamiento
            End Get
        End Property

        Public ReadOnly Property TipoDestinatario() As String
            Get
                Return _tipoDestinatario
            End Get
        End Property

        Public ReadOnly Property Transportadora() As String
            Get
                Return _transportadora
            End Get
        End Property

        Public ReadOnly Property TipoTransporte() As String
            Get
                Return _tipoTransporte
            End Get
        End Property

        Public ReadOnly Property InfoErrores() As DataTable
            Get
                Return _infoErrores
            End Get
        End Property

        Public Property InfoSerialesCuarentena() As DataTable
            Get
                Return _infoSerialesCuarentena
            End Get
            Set(ByVal value As DataTable)
                _infoSerialesCuarentena = value
            End Set
        End Property

        Public Property InfoSerialEspecial() As DataTable
            Get
                Return _infoSerialEspecial
            End Get
            Set(ByVal value As DataTable)
                _infoSerialEspecial = value
            End Set
        End Property

        Public Property InfoMaterialEspecial() As DataTable
            Get
                Return _infoMaterialEspecial
            End Get
            Set(ByVal value As DataTable)
                _infoMaterialEspecial = value
            End Set
        End Property

        Public ReadOnly Property IdTipoProductoPrioridadDeDespacho() As Integer
            Get
                Return _idTipoProductoPrioridadDeDespacho
            End Get
        End Property

        Public ReadOnly Property MensajeTransaccion() As ResultadoProceso
            Get
                Return _resultado
            End Get
        End Property

        Public Property DocumentoCambioEstadoSAP() As String
            Get
                Return _documentoCambioEstadoSAP
            End Get
            Set(ByVal value As String)
                _documentoCambioEstadoSAP = value
            End Set
        End Property

        Public Property SoloBloqueo() As Boolean
            Get
                Return _soloBloqueo
            End Get
            Set(ByVal value As Boolean)
                _soloBloqueo = value
            End Set
        End Property

        Public Property ArchivoExcel As ExcelFile
            Get
                Return oExcel
            End Get
            Set(value As ExcelFile)
                oExcel = value
            End Set
        End Property

        Public Property TablaDatosMaterialSerial() As DataTable
            Get
                If _tablaDatosMaterialSerial Is Nothing Then
                    EstructuraDatosMaterialSerial()
                End If
                Return _tablaDatosMaterialSerial
            End Get
            Set(ByVal value As DataTable)
                _tablaDatosMaterialSerial = value
            End Set
        End Property

        Public Property TablaDatosMaterialCantidad() As DataTable
            Get
                If _tablaDatosMaterialCantidad Is Nothing Then
                    EstructuraDatosMaterialCantidad()
                End If
                Return _tablaDatosMaterialCantidad
            End Get
            Set(ByVal value As DataTable)
                _tablaDatosMaterialCantidad = value
            End Set
        End Property

        Public Property TablaErrores() As DataTable
            Get
                If _tablaErrores Is Nothing Then
                    EstructuraDatosErrores()
                End If
                Return _tablaErrores
            End Get
            Set(ByVal value As DataTable)
                _tablaErrores = value
            End Set
        End Property

        Public Property TablaDatosPedidoServicioTecnico() As DataTable
            Get
                Return _tablaDatosPedidoServicioTecnico
            End Get
            Set(value As DataTable)
                _tablaDatosPedidoServicioTecnico = value
            End Set
        End Property

        Public Property InfoPedidoServicioTecnico() As DataTable
            Get
                Return _infoPedidoServicioTecnico
            End Get
            Set(ByVal value As DataTable)
                _infoPedidoServicioTecnico = value
            End Set
        End Property

        Public Property DtDatos As DataTable
            Get
                Return _dtDatos
            End Get
            Set(value As DataTable)
                _dtDatos = value
            End Set
        End Property

        Public Property DtDetalle As DataTable
            Get
                Return _dtDetalle
            End Get
            Set(value As DataTable)
                _dtDetalle = value
            End Set
        End Property

        Public Property DtGeneral As DataTable
            Get
                Return _dtDatosGeneral
            End Get
            Set(value As DataTable)
                _dtDatosGeneral = value
            End Set
        End Property

        Public Property DsDatos As DataSet
            Get
                Return _dsDatos
            End Get
            Set(value As DataSet)
                _dsDatos = value
            End Set
        End Property

        Public Property Referencia() As String
            Get
                Return _referencia
            End Get
            Set(ByVal value As String)
                _referencia = value
            End Set
        End Property

        Public Property TipoReferencia() As String
            Get
                Return _tipoReferencia
            End Get
            Set(ByVal value As String)
                _tipoReferencia = value
            End Set
        End Property
#End Region

#Region "Constructores"
        Public Sub New()
            MyBase.New()
            Inicializar()
        End Sub

        Public Sub New(ByVal idPedido As Integer)
            MyBase.New()
            Inicializar()
            _idPedido = idPedido
            CargarInformacion(_idPedido)
        End Sub

        Public Sub New(ByRef ArchivoExcel As ExcelFile, ByVal NombreArchivo As String)
            MyBase.New()
            oExcel = ArchivoExcel
        End Sub
#End Region

#Region "Metodos Privados"

        Private Sub CargarInformacion(ByVal idPedido As Integer)
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = idPedido
                    .ejecutarReader("ObtenerInformacionPedido", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing And .Reader.HasRows Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idPedido").ToString, _idPedido)
                            Long.TryParse(.Reader("idPedidoClienteExterno").ToString, _numeroPedido)
                            _numeroEntrega = .Reader("idEntregaClienteExterno").ToString
                            Integer.TryParse(.Reader("idTipoPedido").ToString, _tipoPedido.IdTipo)
                            _tipoPedido.Nombre = .Reader("tipoPedido").ToString
                            Integer.TryParse(.Reader("idCliente").ToString, _idCliente)
                            _cliente = .Reader("cliente").ToString
                            Integer.TryParse(.Reader("idCiudad").ToString, _idCiudad)
                            _ciudad = .Reader("ciudad").ToString
                            Integer.TryParse(.Reader("idEstado").ToString, _idEstado)
                            _estado = .Reader("estado").ToString
                            _fechaCreacion = .Reader("fechaCreacion").ToString
                            Integer.TryParse(.Reader("idUsuario").ToString, _idUsuario)
                            _usuario = .Reader("usuario").ToString
                            Integer.TryParse(.Reader("idPickingList"), _idPicking)
                            Integer.TryParse(.Reader("idDespacho"), _idDespacho)
                            _ordenCompra = .Reader("ordenCompra").ToString
                            Integer.TryParse(.Reader("idSolicitante"), _idSolicitante)
                            _solicitante = .Reader("solicitante").ToString
                            _observaciones = .Reader("observaciones").ToString
                            _direccionDestino = .Reader("direccionDestino").ToString
                            Integer.TryParse(.Reader("idCiudadDestino"), _idCiudadDestino)
                            _ciudadDestino = .Reader("ciudadDestino").ToString
                            _departamentoDestino = .Reader("departamentoDestino").ToString
                            _sincronizado = .Reader("sincronizado").ToString
                            Integer.TryParse(.Reader("idTipoTransporte"), _idTipoTransporte)
                            Integer.TryParse(.Reader("idTransportadora"), _idTransportadora)
                            _tipoTransporte = .Reader("tipoTransporte").ToString
                            _transportadora = .Reader("transportadora").ToString
                            _contabilizarSAP = .Reader("contabilizarSAP").ToString
                            _centroCosto = .Reader("centroCosto").ToString
                            _tipoAlistamiento = .Reader("tipoMovimientoTransporte").ToString
                            _tipoDestinatario = .Reader("tipoDestinatario").ToString
                        End If
                    End If

                    If .Reader IsNot Nothing Then .Reader.Close()
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub CargarDetalle()
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With dbManager
                    .TiempoEsperaComando = 60000
                    .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido

                    _detalle = .ejecutarDataTable("ObtenerDetallePedido", CommandType.StoredProcedure)
                    Dim pk() As DataColumn = {_detalle.Columns("material"), _detalle.Columns("idRegion")}
                    _detalle.PrimaryKey = pk
                End With
                If _detalle Is Nothing Then _detalle = CrearEstructuraDetalle()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Sub DetalleCuarentenasDespacho()
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido
                    _cuarentenasDespacho = .ejecutarDataTable("ObtenerCuarentenasDeDespacho", CommandType.StoredProcedure)
                    Dim pkey(0) As DataColumn
                    pkey(0) = _cuarentenasDespacho.Columns("idDetallePedido")
                    _cuarentenasDespacho.PrimaryKey = pkey
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub CargarSeriales()
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With dbManager
                    .TiempoEsperaComando = 60000
                    .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido
                    If _idDetallePedido > 0 Then .SqlParametros.Add("@idDetallePedido", SqlDbType.Int).Value = _idDetallePedido
                    _seriales = .ejecutarDataTable("ObtenerSerialesPedidoEspecial", CommandType.StoredProcedure)
                End With
                If _seriales Is Nothing Then _seriales = CrearEstructuraSerial()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Sub CargarDetalleCuarentenaParaLiberacion()
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With dbManager
                    If _idPedido <> 0 Then .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido
                    If _idDetallePedido <> 0 Then .SqlParametros.Add("@idDetallePedido", SqlDbType.Int).Value = _idDetallePedido
                    _detalleLiberacionDespacho = .ejecutarDataTable("ObtenerDetalleLiberacionDespacho", CommandType.StoredProcedure)
                    Dim pkey(1) As DataColumn
                    pkey(0) = _detalleLiberacionDespacho.Columns("idDetalleCuarentena")
                    _detalleLiberacionDespacho.PrimaryKey = pkey
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Sub CargarInformacionDespacho()
            Try
                If _idPedido <> 0 Then
                    _infoDespacho = New Despachos.Despacho(_idDespacho)
                Else
                    Throw New Exception("No se puede obtener infomracion del despacho, el identificador del pedido no es válido.")
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Sub

        Private Sub EstablecerParametros(ByVal dbManager As LMDataAccessLayer.LMDataAccess)
            Try
                With dbManager
                    If _tipoPedido.IdTipo <> 0 Then .agregarParametroSQL("@idTipoPedido", _tipoPedido.IdTipo, SqlDbType.SmallInt)
                    If _idEstado <> 0 Then .agregarParametroSQL("@idEstado", _idEstado, SqlDbType.SmallInt)
                    If _idCliente > 0 Then .agregarParametroSQL("@idCliente", _idCliente, SqlDbType.Int)
                    If _ordenCompra IsNot Nothing Then .agregarParametroSQL("@ordenCompra", _ordenCompra.Trim, SqlDbType.VarChar, 15)
                    If _observaciones IsNot Nothing Then .agregarParametroSQL("@observaciones", _observaciones.Trim, SqlDbType.VarChar, 15)
                    If _numeroPedido > 0 Then .agregarParametroSQL("@idPedidoClienteExterno", _numeroPedido, SqlDbType.BigInt)
                    If _direccionDestino IsNot Nothing Then .agregarParametroSQL("@direccionDestino", _direccionDestino.Trim, SqlDbType.VarChar, 150)
                    If _idCiudadDestino > 0 Then .agregarParametroSQL("@idCiudadDestino", _idCiudadDestino, SqlDbType.Int)
                    If _numeroEntrega > 0 Then .agregarParametroSQL("@idEntregaClienteExterno", _numeroEntrega, SqlDbType.BigInt)
                    If _centroCosto IsNot Nothing Then .agregarParametroSQL("@centroCosto", _centroCosto, SqlDbType.VarChar, 100)
                    .agregarParametroSQL("@sincronizado", _sincronizado, SqlDbType.Bit)
                    .agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                    If _idSolicitante > 0 Then .agregarParametroSQL("@idSolicitante", _idSolicitante, SqlDbType.VarChar, 15)
                End With
            Finally
            End Try

        End Sub

        Private Sub EstablecerParametrosLectura(ByVal dbmanager As LMDataAccessLayer.LMDataAccess)
            Try
                With dbmanager.SqlParametros
                    If _serial IsNot Nothing AndAlso _serial.Trim.Length > 0 Then .Add("@serial", SqlDbType.VarChar, 20).Value = _serial
                    .Add("@tipoLectura", SqlDbType.Int).Value = _tipoLectura
                    .Add("@idDetallePedido", SqlDbType.Int).Value = _idDetallePedido
                    .Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                End With
            Catch ex As Exception

            End Try
        End Sub

        Private Sub EstablecerParametrosLiberacion(ByVal dbManager As LMDataAccessLayer.LMDataAccess)
            Try
                With dbManager
                    .SqlParametros.Clear()
                    .agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                    .agregarParametroSQL("@idTipoPedido", _tipoPedido.IdTipo, SqlDbType.SmallInt)
                    If _idSolicitante > 0 Then .agregarParametroSQL("@idSolicitante", _idSolicitante, SqlDbType.VarChar, 15)
                    If _observaciones IsNot Nothing Then .agregarParametroSQL("@observaciones", _observaciones.Trim, SqlDbType.VarChar, 15)
                    .SqlParametros.Add("@idPedido", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
            Finally
            End Try

        End Sub

        Public Function CrearEstructuraDetalle() As DataTable
            Dim dtDetalle As New DataTable
            Dim pk(1) As DataColumn

            With dtDetalle
                .Columns.Add("Material", GetType(String))
                .Columns.Add("Cantidad", GetType(Integer))
                .Columns.Add("idRegion", GetType(Integer))
                .Columns.Add("IdTipoUnidad", GetType(Short))
                .Columns.Add("UnidadEmpaque", GetType(String))
                pk(0) = .Columns("Material")
                .PrimaryKey = pk
            End With

            Return dtDetalle
        End Function

        Public Function CrearEstructuraSerial() As DataTable
            Dim dtSerial As New DataTable
            With dtSerial
                .Columns.Add("idSerial", GetType(Integer))
                .Columns.Add("serial", GetType(String))
                .Columns.Add("especial", GetType(String))
            End With

            Return dtSerial
        End Function

        Public Function CrearEstructuraSerialServicioTecnico() As DataTable
            Dim dtSerial As New DataTable
            With dtSerial
                .Columns.Add("idSerial", GetType(Integer))
                .Columns.Add("serial", GetType(String))
            End With
            Return dtSerial
        End Function

        Private Function CrearEstructuraRelacionCuarentena() As DataTable
            Dim dtAux As New DataTable
            With dtAux.Columns
                .Add("idDetallePedido", GetType(Integer))
                .Add("material", GetType(String))
                .Add("idRegion", GetType(Integer))
                .Add("cantidad", GetType(Integer))
            End With
            Dim pk(0) As DataColumn
            pk(0) = dtAux.Columns("idDetallePedido")
            dtAux.PrimaryKey = pk
            Return dtAux
        End Function

        Private Sub Inicializar()
            _infoErrores = ObtenerEstructuraErrores()
            _tipoPedido = New TipoPedido
            _resultado = New ResultadoProceso
        End Sub

        Private Function ObtenerEstructuraErrores() As DataTable
            Dim dtAux As New DataTable
            With dtAux.Columns
                .Add("tipo", GetType(String))
                .Add("descripcion", GetType(String))
            End With
            Return dtAux
        End Function

        Private Sub EstructuraDatosMaterialSerial()
            Try
                Dim dtDatos As New DataTable
                If _tablaDatosMaterialSerial Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("material", GetType(String)))
                        .Add(New DataColumn("region", GetType(String)))
                        .Add(New DataColumn("serial", GetType(String)))
                    End With
                    dtDatos.AcceptChanges()
                    _tablaDatosMaterialSerial = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub EstructuraDatosMaterialCantidad()
            Try
                Dim dtDatos As New DataTable
                If _tablaDatosMaterialCantidad Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("material", GetType(String)))
                        .Add(New DataColumn("region", GetType(String)))
                        .Add(New DataColumn("cantidad", GetType(String)))
                    End With
                    dtDatos.AcceptChanges()
                    _tablaDatosMaterialCantidad = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub EstructuraDatosErrores()
            Try
                Dim dtDatos As New DataTable
                If _tablaErrores Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("id", GetType(Integer)))
                        .Add(New DataColumn("nombre", GetType(String)))
                        .Add(New DataColumn("descripcion", GetType(String)))
                        .Add(New DataColumn("serial", GetType(String)))
                    End With
                    dtDatos.AcceptChanges()
                    _tablaErrores = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub AdicionarError(ByVal id As Integer, ByVal nombre As String, ByVal descripcion As String, ByVal serial As String)
            Try
                With TablaErrores
                    Dim drError As DataRow = .NewRow()
                    With drError
                        .Item("id") = id
                        .Item("nombre") = nombre
                        .Item("descripcion") = descripcion
                        .Item("serial") = serial
                    End With
                    .Rows.Add(drError)
                    .AcceptChanges()
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Function ConsultarSeriales(ByVal _material As String, ByVal _region As String, ByVal _serial As String) As Integer
            Dim _existe As Integer = -1
            Try
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
                        .SqlParametros.Add("@region", SqlDbType.VarChar).Value = _region
                        .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = _serial
                        _existe = .ejecutarScalar("ConsultarSerialPedidoEspecial", CommandType.StoredProcedure)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Catch ex As Exception
                Throw ex
            End Try
            Return _existe
        End Function

        Private Function CargarMaterialesCantidad() As DataTable
            Dim dtResultado As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                Try
                    With dbManager
                        .SqlParametros.Clear()
                        dtResultado = .ejecutarDataTable("ConsultarCantidadSerialesPedidoEspecial", CommandType.StoredProcedure)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Catch ex As Exception
                Throw ex
            End Try
            Return dtResultado
        End Function

        Private Function CargarRegion() As DataTable
            Dim dtResultado As New DataTable
            Dim dbManager As New LMDataAccess
            Try
                Try
                    With dbManager
                        .SqlParametros.Clear()
                        dtResultado = .ejecutarDataTable("ObtenerRegiones", CommandType.StoredProcedure)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Catch ex As Exception
                Throw ex
            End Try
            Return dtResultado
        End Function

        Private Sub EstructuraDatosPedidoServicioTecnico()
            Try
                Dim dtDatos As New DataTable
                If _tablaDatosPedidoServicioTecnico Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("serial", GetType(String)))
                    End With
                    dtDatos.AcceptChanges()
                    _tablaDatosPedidoServicioTecnico = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Function ConsultarSerialesServicioTecnico(ByVal _serial As String) As DataTable
            Dim _dtDatos As DataTable
            Try
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = _serial
                        _dtDatos = .ejecutarDataTable("ConsultarSerialServicioTecnico", CommandType.StoredProcedure)
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Catch ex As Exception
                Throw ex
            End Try
            Return _dtDatos
        End Function

        Public Sub CargarSerialesServicioTecnico()
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With dbManager
                    .TiempoEsperaComando = 1200
                    .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido
                    _seriales = .ejecutarDataTable("ObtenerSerialesPedidoServicioTecnico", CommandType.StoredProcedure)
                End With
                If _seriales Is Nothing Then _seriales = CrearEstructuraSerialServicioTecnico()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Publicos"

        Public Function Crear() As Boolean

            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim registrado As Boolean = False
            Try
                _resultado = New ResultadoProceso
                If _detalle.Rows.Count > 0 Then
                    With dbManager
                        _idEstado = EstadoPedido.Pendiente
                        EstablecerTipoProductoConPrioridadDeDesapcho()
                        EstablecerParametros(dbManager)
                        If _idTipoProductoPrioridadDeDespacho > 0 Then _
                        .SqlParametros.Add("@idTipoProductoPrioridadDeDespacho", SqlDbType.Int).Value = _idTipoProductoPrioridadDeDespacho
                        .SqlParametros.Add("@soloBloqueo", SqlDbType.Bit).Value = _soloBloqueo

                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        dbManager.iniciarTransaccion()
                        _idPedido = .ejecutarScalar("CrearPedido", CommandType.StoredProcedure)

                        If .SqlParametros("@returnValue").Value = 0 And _idPedido <> 0 Then
                            If _tipoPedido.IdTipo = 6 Then
                                CargarCuarentenasDeDespacho(dbManager)
                            Else
                                CrearDetalle(dbManager)
                            End If


                            Select Case _tipoPedido.IdTipo
                                Case CInt(TipoPedido.Tipo.DespachoCuarentena)
                                    CargarCuarentenasDeDespacho(dbManager)
                                Case CInt(TipoPedido.Tipo.LiberacionParaDespachoCuarentena)
                                    CargarCuarentenasParaLiberacionDespacho(dbManager)
                            End Select

                            If .confirmarTransaccion() Then
                                _resultado.Valor = 0
                            Else
                                _resultado.Valor = -1
                                _resultado.Mensaje = "Ocurrio un error no identificado al momento de crear el pedido."
                            End If
                        Else
                            _resultado.Valor = CInt(.SqlParametros("@returnValue").Value)
                            Select Case _resultado.Valor
                                Case 1
                                    _resultado.Mensaje = "No se recibió información suficiente para asignar información de transporte."
                                Case 2
                                    _resultado.Mensaje = "Ya existe un pedido con la el número de pedido " & .SqlParametros("@idPedidoClienteExterno").Value
                                Case 3
                                    _resultado.Mensaje = "Ya existe un pedido con la el número de entrega " & .SqlParametros("@idEntregaClienteExterno").Value
                                Case 4
                                    _resultado.Mensaje = "No fue posible registrar el pedido."
                                Case 5
                                    _resultado.Mensaje = "No se asignó compltamente la información de transporte."
                                Case Else
                                    _resultado.Mensaje = "Ocurrio un error no identificado al momento de crear el pedido."
                            End Select
                            .abortarTransaccion()
                        End If
                    End With
                Else
                    Throw New Exception("Imposible obtener detalle del pedido a crear.")
                End If

                If _resultado.Valor = 0 Then registrado = True

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                _resultado.Valor = -1
                _resultado.Mensaje = ex.Message
                Throw New Exception(_resultado.Mensaje)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return registrado
        End Function

        Private Sub CrearDetalle(ByRef db As LMDataAccess)
            With db
                Using dtAux As DataTable = _detalle.Copy
                    Dim dcAux As New DataColumn("idPedido")
                    dcAux.DefaultValue = _idPedido
                    dtAux.Columns.Add(dcAux)
                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "DetallePedido"
                        .ColumnMappings.Add("idPedido", "idPedido")
                        .ColumnMappings.Add("material", "material")
                        .ColumnMappings.Add("cantidad", "cantidad")
                        .ColumnMappings.Add("idTipoUnidad", "idTipoUnidad")
                        .ColumnMappings.Add("cantidad", "cantidadSolicitada")
                        .ColumnMappings.Add("idTipoUnidad", "idTipoUnidadSolicitada")
                        .ColumnMappings.Add("idRegion", "idRegion")
                        .WriteToServer(dtAux)
                    End With
                End Using
            End With

        End Sub

        Public Sub CargarCuarentenasParaLiberacionDespacho(ByRef dbManager As LMDataAccess)
            Dim resultado As Short
            If _detalleLiberacionDespacho IsNot Nothing AndAlso _detalleLiberacionDespacho.Rows.Count > 0 AndAlso _idUsuario > 0 Then
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .ejecutarNonQuery("BorrarAuxDetalleLiberacionDetalleCuarentena", CommandType.StoredProcedure)
                    If resultado <> 0 Then
                        Throw New Exception("No se pudo borrar la información temporal de relacion de pedido para liberación de despachos con pedidos de cuarentena.")
                        Return
                    End If

                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "AuxDetalleLiberacionDetalleCuarentena"
                        .ColumnMappings.Add("idPedido", "idPedido")
                        .ColumnMappings.Add("idDetalleCuarentena", "idDetalleCuarentena")
                        .ColumnMappings.Add("material", "material")
                        .ColumnMappings.Add("idRegion", "idRegion")
                        .ColumnMappings.Add("cantidad", "cantidad")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(_detalleLiberacionDespacho)
                    End With

                    .SqlParametros.Clear()
                    .agregarParametroSQL("@idPedido", _idPedido, SqlDbType.Int)
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("CrearDetalleLiberacionDetalleCuarentena", CommandType.StoredProcedure)
                    resultado = CShort(.SqlParametros("@returnValue").Value)
                    Select Case resultado
                        Case 1 : Throw New Exception("No se pudo obtener información para relacionar los pedidos de cuarentena con el pedido de liberación para despacho de cuarentenas.")
                        Case 2 : Throw New Exception("No se pudo actualizar la información del pedido de liberación para despacho de cuarentena para relacionarlo con los pedidos de cuarentena.")
                        Case 3 : Throw New Exception("No fue posible adicionar la relación del pedido de liberación para despacho de cuarentena con los pedidos de cuarentena.")
                    End Select
                End With
            Else
                Throw New Exception("No fue suministrada la información necesaria para establecer la relacion entre el pedido de liberación para despacho y las cuarentenas.")
            End If
        End Sub

        Public Sub CargarCuarentenasDeDespacho(ByRef dbManager As LMDataAccess)
            Dim resultado As Short
            If _detalleLiberacionDespacho IsNot Nothing AndAlso _detalleLiberacionDespacho.Rows.Count > 0 AndAlso _idUsuario > 0 Then
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .ejecutarNonQuery("BorrarRelacionDespachoCuarentenas", CommandType.StoredProcedure)
                    If resultado <> 0 Then
                        Throw New Exception("No se pudo borrar la información temporal de relacion de despachos con pedidos de cuarentena.")
                        Return
                    End If

                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "AuxRelacionDespachoCuarentena"
                        .ColumnMappings.Add("idPedido", "idPedido")
                        .ColumnMappings.Add("idDetallePedido", "idDetalleCuarentena")
                        .ColumnMappings.Add("material", "material")
                        .ColumnMappings.Add("idRegion", "idRegion")
                        .ColumnMappings.Add("cantidad", "cantidad")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(_detalleLiberacionDespacho)
                    End With

                    .SqlParametros.Clear()
                    .agregarParametroSQL("@idPedido", _idPedido, SqlDbType.Int)
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("CrearRelacionDespachoCuarentenas", CommandType.StoredProcedure)
                    resultado = CShort(.SqlParametros("@returnValue").Value)
                    Select Case resultado
                        Case 1 : Throw New Exception("No se pudo obtener información para relacionar los pedidos de cuarentena con el pedido de despacho .")
                        Case 2 : Throw New Exception("No se pudo actualizar la información del despacho para relacionarlo con los pedidos de cuarentena.")
                        Case 3 : Throw New Exception("No adicionar la relacion del pedido de despacho con los pedidos de cuarentena.")
                    End Select
                End With
            Else
                Throw New Exception("No fue suministrada la información necesaria para establecer la relacion entre el despacho y la cuarentena .")
            End If
        End Sub

        Public Function CrearPedidoArchivoSeriales() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultadoEjecucion As New ResultadoProceso

            If _idUsuario > 0 AndAlso _infoSerialesCuarentena IsNot Nothing AndAlso _infoSerialesCuarentena.Rows.Count > 0 Then
                Try
                    With dbManager
                        .TiempoEsperaComando = 1200

                        dbManager.iniciarTransaccion()
                        .agregarParametroSQL("@idTipoPedido", _tipoPedido.IdTipo, SqlDbType.SmallInt)
                        If _idCliente > 0 Then .agregarParametroSQL("@idCliente", _idCliente, SqlDbType.Int)
                        .agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                        If _ordenCompra IsNot Nothing Then .agregarParametroSQL("@ordenCompra", _ordenCompra.Trim, SqlDbType.VarChar, 15)
                        If _idSolicitante > 0 Then .agregarParametroSQL("@idSolicitante", _idSolicitante, SqlDbType.VarChar, 15)
                        If _observaciones IsNot Nothing Then .agregarParametroSQL("@observaciones", _observaciones.Trim, SqlDbType.VarChar, 15)
                        If .SqlParametros.IndexOf("@soloBloqueo") < 0 Then .SqlParametros.Add("@soloBloqueo", SqlDbType.Bit).Value = _soloBloqueo
                        .SqlParametros.Add("@idPedido", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("CrearPedidoArchivoSeriales", CommandType.StoredProcedure)

                        resultadoEjecucion.EstablecerMensajeYValor(CShort(.SqlParametros("@returnValue").Value), "Ejecución Satisfactoria")
                        If resultadoEjecucion.Valor = 0 Then
                            _idPedido = CLng(.SqlParametros("@idPedido").Value)

                            If Not _soloBloqueo Then resultadoEjecucion = CargaCuarentenaSAP(dbManager)


                            If resultadoEjecucion.Valor = 0 Then
                                If Not .confirmarTransaccion() Then
                                    If .estadoTransaccional Then .abortarTransaccion()
                                    resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible confirmar la transacción para la creación de la cuarentena.")
                                End If
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                            End If
                        Else
                            Select Case resultadoEjecucion.Valor
                                Case 1 : resultadoEjecucion.EstablecerMensajeYValor(1, "No se encontrar seriales para crear el pedido de cuarentena.")
                                Case 2 : resultadoEjecucion.EstablecerMensajeYValor(2, "Error al registrar el pedido de cuarentena.")
                                Case 3 : resultadoEjecucion.EstablecerMensajeYValor(3, "Error al registra el detalle de pedido para la cuarentena.")
                                Case 4 : resultadoEjecucion.EstablecerMensajeYValor(4, "Error al marcar los seriales como cuarentenas.")
                                Case 5 : resultadoEjecucion.EstablecerMensajeYValor(5, "No existe el material perteneciente a la region de bloqueo para realizar la actualizacion de region de los seriales de la cuarentena.")
                                Case 6 : resultadoEjecucion.EstablecerMensajeYValor(6, "Error al actualizar la region de bloqueo para los seriales.")
                            End Select

                            If .estadoTransaccional Then .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible obtener los datos necesario para crear la cuarentena.")
            End If

            Return resultadoEjecucion
        End Function

        Public Function CargaCuarentenaSAP(ByRef dbManager As LMDataAccessLayer.LMDataAccess) As ResultadoProceso
            Dim cambioDeEstado As New CambioDeEstadoSAP
            Dim resultadoEjecucion As New ResultadoProceso

            resultadoEjecucion.EstablecerMensajeYValor(0, "Ejecución Satisfactoria")

            If _idPedido > 0 Then
                If _infoSerialesCuarentena IsNot Nothing AndAlso _infoSerialesCuarentena.Rows.Count > 0 Then
                    With cambioDeEstado
                        .IdPedido = _idPedido
                        .TipoCambio = CambioDeEstadoSAP.Tipo.Cuarentena
                        .InfoSeriales = _infoSerialesCuarentena
                        .CentroCambio = "4105"
                        .AlmacenCambio = "4105"
                        .ValeMaterial = "CAMBIO STOCK"
                        .TextoCabecera = "Pedido " & _idPedido.ToString
                        .StockOrigen = CambioDeEstadoSAP.TipoStock.LibreUtilizacion
                        .StockDestino = CambioDeEstadoSAP.TipoStock.ControlCalidad
                        resultadoEjecucion = .GenerarCambio()
                        _infoErrores = .InfoErrores

                        If resultadoEjecucion.Valor = 0 Then
                            _documentoCambioEstadoSAP = .DocumentoSAP
                            _infoSerialesCuarentena = .InfoSeriales
                            resultadoEjecucion = RegistrarDocumentoSAPCuarentena(dbManager, .DocumentoSAP)
                        End If
                    End With
                Else
                    resultadoEjecucion.EstablecerMensajeYValor(2, "No se pudieron obtener los seriales de la cuarentena para realizar el cambio de estado en SAP. ")
                End If
            Else
                resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible obtener el pedido para realizar el cambio de estado en SAP. ")
            End If

            Return resultadoEjecucion
        End Function

        Private Function RegistrarDocumentoSAPCuarentena(ByVal dbManager As LMDataAccessLayer.LMDataAccess, ByVal documentoSAP As String) As ResultadoProceso
            Dim rp As New ResultadoProceso

            rp.EstablecerMensajeYValor(0, "Ejecución Satisfactoria")
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@documentoCuarentena", SqlDbType.VarChar, 20).Value = documentoSAP
                    .Add("@idPedido", SqlDbType.BigInt).Value = _idPedido
                    .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .ejecutarNonQuery("ActualizarDocumentoSAPCuarentena", CommandType.StoredProcedure)

                rp.Valor = CShort(dbManager.SqlParametros("@returnValue").Value)
                If rp.Valor = 0 Then
                    If _infoSerialesCuarentena IsNot Nothing AndAlso _infoSerialesCuarentena.Rows.Count > 0 Then
                        Dim dtCambioRegion As DataTable
                        Dim dvAlmacen As DataView = _infoSerialesCuarentena.DefaultView
                        dvAlmacen.RowFilter = "LEN(documentoCambioRegion)>0 OR documentoCambioRegion IS NOT NULL"

                        With .SqlParametros
                            .Clear()
                            .Add("@documentoCambioRegion", SqlDbType.VarChar, 20)
                            .Add("@serial", SqlDbType.VarChar, 30)
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With

                        dtCambioRegion = dvAlmacen.ToTable
                        For i As Integer = 0 To dtCambioRegion.Rows.Count - 1
                            .SqlParametros("@documentoCambioRegion").Value = dtCambioRegion.Rows(i)("documentoCambioRegion").ToString()
                            .SqlParametros("@serial").Value = dtCambioRegion.Rows(i)("serial").ToString()
                            .ejecutarNonQuery("ActualizarCambioRegionSAPCuarentena", CommandType.StoredProcedure)

                            rp.Valor = CShort(dbManager.SqlParametros("@returnValue").Value)
                            If rp.Valor <> 0 Then
                                Select Case rp.Valor
                                    Case 1 : rp.EstablecerMensajeYValor(rp.Valor, "No fue posible encontrar el serial en la cuarentena para actualizar el documento de cambio de region. ")
                                    Case 2 : rp.EstablecerMensajeYValor(rp.Valor, "Error al actualizar el documento cambio de region SAP para el serial de cuarentena. ")
                                End Select
                                Exit For
                            End If
                        Next
                    Else
                        rp.EstablecerMensajeYValor(rp.Valor, "No fue posible obtener los seriales de la cuarentena que tuvieron cambio de region en SAP. ")
                    End If
                Else
                    Select Case rp.Valor
                        Case 1 : rp.EstablecerMensajeYValor(rp.Valor, "No se pudieron obtener los seriales de cuarentena para actualizar el documento de cambio de estado. ")
                        Case 2 : rp.EstablecerMensajeYValor(rp.Valor, "Error al actualizar el documento cambio de estado SAP en el pedido de cuarentena. ")
                    End Select
                End If
            End With

            Return rp
        End Function

        Public Function LiberarCuarentena() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short = 0
            Dim resultadoEjecucion As New ResultadoProceso

            If _idUsuario > 0 AndAlso _infoSerialesCuarentena IsNot Nothing AndAlso _infoSerialesCuarentena.Rows.Count > 0 Then
                Try
                    With dbManager
                        dbManager.iniciarTransaccion()
                        EstablecerParametrosLiberacion(dbManager)
                        .ejecutarNonQuery("LiberarCuarentena", CommandType.StoredProcedure)

                        resultadoEjecucion.EstablecerMensajeYValor(CShort(.SqlParametros("@returnValue").Value), "Ejecución Satisfactoria")
                        If resultadoEjecucion.Valor = 0 Then
                            _idPedido = CLng(.SqlParametros("@idPedido").Value)

                            resultadoEjecucion = LiberacionCuarentenaSAP(dbManager)

                            If resultadoEjecucion.Valor = 0 Then
                                If Not .confirmarTransaccion() Then
                                    If .estadoTransaccional Then .abortarTransaccion()
                                    resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible confirmar la transacción para la liberación de cuarentenas.")
                                End If
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                            End If
                        Else
                            Select Case resultadoEjecucion.Valor
                                Case 1 : resultadoEjecucion.EstablecerMensajeYValor(1, "No se pudo generar el pedido de liberación.")
                                Case 2 : resultadoEjecucion.EstablecerMensajeYValor(2, "No se pudo registrar el detalle del pedido.")
                                Case 3 : resultadoEjecucion.EstablecerMensajeYValor(3, "No se pudo registrar el historico del los seriales en cuarentena.")
                                Case 4 : resultadoEjecucion.EstablecerMensajeYValor(4, "No se pudo eliminar los seriales de la cuarentena.")
                                Case 5 : resultadoEjecucion.EstablecerMensajeYValor(5, "No se puedo actualizar el estado de los pedidos de cuarentena.")
                                Case 6 : resultadoEjecucion.EstablecerMensajeYValor(6, "No se ha suministrado la información necesaria.")
                            End Select

                            If .estadoTransaccional Then .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible obtener los datos necesario para liberar las cuarentenas.")
            End If

            Return resultadoEjecucion
        End Function

        Public Function LiberacionCuarentenaSAP(ByVal dbManager As LMDataAccessLayer.LMDataAccess) As ResultadoProceso
            Dim cambioDeEstado As New CambioDeEstadoSAP
            Dim resultadoEjecucion As New ResultadoProceso

            resultadoEjecucion.Valor = 0
            resultadoEjecucion.Mensaje = "Ejecución Satisfactoria"

            If _idPedido > 0 Then
                If _infoSerialesCuarentena IsNot Nothing AndAlso _infoSerialesCuarentena.Rows.Count > 0 Then
                    With cambioDeEstado
                        .IdPedido = _idPedido
                        .TipoCambio = CambioDeEstadoSAP.Tipo.LiberacionCuarentena
                        .InfoSeriales = _infoSerialesCuarentena
                        .ValeMaterial = "CAMBIO STOCK"
                        .TextoCabecera = "Pedido " & _idPedido.ToString
                        .StockOrigen = CambioDeEstadoSAP.TipoStock.ControlCalidad
                        .StockDestino = CambioDeEstadoSAP.TipoStock.LibreUtilizacion
                        resultadoEjecucion = .GenerarCambio()
                        _infoErrores = .InfoErrores

                        If resultadoEjecucion.Valor = 0 Then
                            _documentoCambioEstadoSAP = .DocumentoSAP
                            _infoSerialesCuarentena = .InfoSeriales
                            resultadoEjecucion = RegistrarDocumentoSAPLiberacion(dbManager, .DocumentoSAP)
                        End If
                    End With
                Else
                    resultadoEjecucion.Valor = 5
                    resultadoEjecucion.Mensaje = "No se pudieron obtener los seriales de la cuarentena para cambiar el estado en SAP como Liberado. "
                End If
            Else
                resultadoEjecucion.Valor = 4
                resultadoEjecucion.Mensaje = "No fue posible obtener el pedido para realizar el cambio de estado en SAP. "
            End If

            Return resultadoEjecucion
        End Function

        Private Function RegistrarDocumentoSAPLiberacion(ByVal dbManager As LMDataAccessLayer.LMDataAccess, ByVal documentoSAP As String) As ResultadoProceso
            Dim rp As New ResultadoProceso

            rp.EstablecerMensajeYValor(0, "Ejecución Satisfactoria")
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@documentoLiberacion", SqlDbType.VarChar, 20).Value = documentoSAP
                    .Add("@idPedido", SqlDbType.BigInt).Value = _idPedido
                    .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .ejecutarNonQuery("ActualizarDocumentoSAPLiberacion", CommandType.StoredProcedure)

                rp.Valor = CShort(dbManager.SqlParametros("@returnValue").Value)
                If rp.Valor <> 0 Then
                    Select Case rp.Valor
                        Case 1 : rp.EstablecerMensajeYValor(rp.Valor, "No fue posible obtener los seriales liberados de cuarentena para actualizar el documento de cambio de estado. ")
                        Case 2 : rp.EstablecerMensajeYValor(rp.Valor, "Error al actualizar el documento cambio de estado SAP en el pedido de Liberación de cuarentena. ")
                    End Select
                End If
            End With

            Return rp
        End Function

        Public Function Actualizar(ByVal idPedido As Integer, Optional ByVal ActualizaInfoTransporte As Boolean = True) As Boolean

            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim actualizado As Boolean = False

            Try
                _resultado = New ResultadoProceso
                _idPedido = idPedido

                EstablecerParametros(dbManager)
                dbManager.SqlParametros.Add("@idPedido", SqlDbType.Int).Value = idPedido
                dbManager.SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output

                With dbManager
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarPedido", CommandType.StoredProcedure)
                    _resultado.Valor = .SqlParametros("@resultado").Value

                    If _resultado.Valor = 0 Then
                        .SqlParametros.Clear()
                        If _tipoPedido.IdTipo = Tipo.Tipo.DespachoCuarentena Then
                            ActualizarRelacionCuarentenaDespacho(dbManager)
                        Else
                            ActualizarDetalle(dbManager)
                        End If

                        Select Case _tipoPedido.IdTipo
                            Case CInt(TipoPedido.Tipo.DespachoCuarentena)
                                ActualizarRelacionCuarentenaDespacho(dbManager)
                            Case CInt(TipoPedido.Tipo.LiberacionParaDespachoCuarentena)
                                ActualizarDetalleLiberacionDetalleCuarentena(dbManager)
                        End Select

                    ElseIf _resultado.Valor = 1 Then
                        _resultado.Mensaje = "Ya existe un pedido con el número " & .SqlParametros("@idPedidoClienteExterno").Value
                    ElseIf _resultado.Valor = 2 Then
                        _resultado.Mensaje = "Ya existe un pedido con la entrega número " & .SqlParametros("@idEntregaClienteExterno").Value
                    ElseIf _resultado.Valor = 3 Then
                        _resultado.Mensaje = "El pedido " & idPedido & " se encuentra en estado anulado, no se puede actualizar"
                    ElseIf _resultado.Valor = 4 Then
                        _resultado.Mensaje = "No es posible actualizar el pedido " & idPedido & " a un estado anterior, ya tiene un despacho asociado."
                    ElseIf _resultado.Valor = 5 Then
                        _resultado.Mensaje = "No es posible actualizar el pedido " & idPedido & " a un estado anterior, ya tiene seriales leidos."
                    ElseIf _resultado.Valor = -1 Then
                        _resultado.Mensaje = "Ocurrió un error inesperado al actualizar el pedido " & idPedido & "."
                    End If

                    '--------------------------------------------------------------------------
                    ' Asigna Información de Transporte
                    '--------------------------------------------------------------------------

                    If _resultado.Valor = 0 And ActualizaInfoTransporte Then
                        .SqlParametros.Clear()
                        AsignarInformacionDeTransporte(_idPedido, _tipoPedido.IdTipo, dbManager)
                    End If

                    If _resultado.Valor = 0 Then
                        actualizado = .confirmarTransaccion()
                    Else
                        .abortarTransaccion()
                    End If
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)

                dbManager.abortarTransaccion()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return actualizado
        End Function

        Public Function AdicionarDetalle(ByVal iMaterial As String, ByVal iCantidad As Integer, ByVal iIdTipoUnidad As Short, ByVal iIdRegion As Integer) As String
            Dim resultado As String = String.Empty
            Dim infoMaterial As New Subproducto()
            Dim filtro As FiltroExistenciasMaterial
            If _detalle Is Nothing Then _detalle = CrearEstructuraDetalle()
            Dim disponible As Integer
            Dim pk(1) As DataColumn
            pk(0) = _detalle.Columns("material")
            pk(1) = _detalle.Columns("idRegion")
            _detalle.PrimaryKey = pk

            filtro.material = iMaterial
            filtro.cantidadSolicitada = iCantidad
            If iIdRegion <> 0 Then filtro.idRegion = iIdRegion
            filtro.idCliente = _idCliente
            filtro.idPedido = _idPedido
            filtro.idTipoPedido = _tipoPedido.IdTipo


            Dim draux() As DataRow
            Dim filtroConsulta As String = "material = " & iMaterial

            disponible = DetallePedidoTieneDisponibilidad(filtro)

            If _tipoPedido.IdTipo = 4 OrElse _tipoPedido.IdTipo = 6 Then
                If _tipoPedido.IdTipo = 6 Then disponible = 1 ' Para pedidos de despacho de cuarentena, la validacion se realiza cuando se valida la disponibilidad de producto en cuarentena
                filtroConsulta = "material = " & iMaterial & " AND idRegion = " & iIdRegion.ToString
            End If


            If _detalle IsNot Nothing Then draux = _detalle.Select(filtroConsulta)

            If disponible >= 0 Then
                If draux Is Nothing OrElse draux.Length = 0 Then
                    Dim drDetalle As DataRow = _detalle.NewRow
                    drDetalle("material") = iMaterial
                    drDetalle("cantidad") = iCantidad
                    drDetalle("idTipoUnidad") = iIdTipoUnidad
                    drDetalle("idRegion") = iIdRegion
                    _detalle.Rows.Add(drDetalle)
                Else
                    resultado = "Ya se adicionó el material al detalle " & iMaterial
                End If
            Else
                resultado = "No hay existencias disponibles para el material " & iMaterial
            End If

            Return resultado
        End Function

        Public Function DetallePedidoTieneDisponibilidad(ByVal filtro As FiltroExistenciasMaterial) As Integer
            Dim dbManager As New LMDataAccess
            Dim iResultado As Integer = 0
            Try
                With dbManager
                    If filtro.material.ToString.Trim.Length > 0 Then .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Value = filtro.material.Trim
                    If filtro.idRegion <> 0 Then .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = filtro.idRegion
                    If filtro.idCliente <> 0 Then .SqlParametros.Add("@idCliente", SqlDbType.Int).Value = filtro.idCliente
                    If filtro.idBodega <> 0 Then .SqlParametros.Add("@idbodega", SqlDbType.Int).Value = filtro.idBodega
                    If filtro.idPedido <> 0 Then .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = filtro.idPedido
                    If filtro.numeroPedido <> 0 Then .SqlParametros.Add("@idPedido", SqlDbType.BigInt).Value = filtro.numeroPedido
                    If filtro.idTipoPedido <> 0 Then .SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = filtro.idTipoPedido
                    If filtro.cantidadSolicitada <> 0 Then .SqlParametros.Add("@cantidadSolicitada", SqlDbType.Int).Value = filtro.cantidadSolicitada
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    .ejecutarNonQuery("ObtenerExistenciaDisponibleMaterial", CommandType.StoredProcedure)
                    If Not Integer.TryParse(.SqlParametros("@resultado").Value.ToString, iResultado) Then
                        Throw New Exception("Imposible determinar la cantidad disponible para el pedido. Por favor intente nuevamente.")
                    End If
                End With

                Return iResultado
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return iResultado
        End Function

        Public Function AdicionarRelacionCuarentena(ByVal idDetalleCuarentena As Integer, ByVal material As String, ByVal idRegion As Integer, ByVal cantidad As Integer) As Boolean
            Dim resultado As Boolean = False
            If _detalleLiberacionDespacho Is Nothing Then _detalleLiberacionDespacho = CrearEstructuraRelacionCuarentena()

            Dim pk(0) As DataColumn
            pk(0) = _detalleLiberacionDespacho.Columns("idDetalleCuarentena")
            _detalleLiberacionDespacho.PrimaryKey = pk

            If _detalleLiberacionDespacho.Rows.Find(idDetalleCuarentena) Is Nothing Then
                Dim drDetalle As DataRow = _detalleLiberacionDespacho.NewRow
                drDetalle("idDetalleCuarentena") = idDetalleCuarentena
                drDetalle("material") = material
                drDetalle("idRegion") = idRegion
                drDetalle("cantidad") = cantidad
                _detalleLiberacionDespacho.Rows.Add(drDetalle)
                resultado = True
            Else
                _detalleLiberacionDespacho.Rows.Find(idDetalleCuarentena).Item("cantidad") = cantidad
            End If
            Return resultado
        End Function

        Public Function RemoverDetalle(ByVal material As String, Optional ByVal idRegion As Integer = 0) As Boolean
            Dim resultado As Boolean = False
            _detalle.DefaultView.AllowDelete = True

            If _detalle IsNot Nothing Then
                Dim pk(1) As DataColumn
                pk(0) = _detalle.Columns("material")
                _detalle.PrimaryKey = pk
                If _detalle.Rows.Find(material) IsNot Nothing Then
                    _detalle.Rows.Find(material).Delete()
                    resultado = True
                End If
            End If
            Return resultado
        End Function

        Public Function RemoverRelacionCuarentena(ByVal idDetalleCuarentena As Integer) As Boolean
            Dim resultado As Boolean = False
            _detalleLiberacionDespacho.DefaultView.AllowDelete = True

            If _detalleLiberacionDespacho IsNot Nothing Then
                Dim pk(0) As DataColumn
                pk(0) = _detalleLiberacionDespacho.Columns("idDetalleCuarentena")

                If _detalleLiberacionDespacho.Rows.Find(idDetalleCuarentena) IsNot Nothing Then
                    _detalleLiberacionDespacho.Rows.Find(idDetalleCuarentena).Delete()
                    resultado = True
                End If
            End If

            Return resultado
        End Function

        Public Function EditarDetalle(ByVal sMaterial As String, Optional ByVal iIdRegion As Integer = 0, Optional ByVal iCantidad As Integer = 0) As String
            Dim resultado As String = String.Empty
            Dim infoMaterial As New Subproducto()
            Dim filtro As FiltroExistenciasMaterial
            If _detalle Is Nothing Then _detalle = CrearEstructuraDetalle()
            Dim pk(1) As String
            filtro.material = sMaterial
            filtro.cantidadSolicitada = iCantidad
            If iIdRegion <> 0 Then filtro.idRegion = iIdRegion
            filtro.idCliente = _idCliente
            filtro.idPedido = _idPedido
            filtro.idTipoPedido = _tipoPedido.IdTipo

            If DetallePedidoTieneDisponibilidad(filtro) >= 0 Then

                If _detalle IsNot Nothing Then
                    pk = ObtenerValorLlavePrimaria(sMaterial, iIdRegion)
                    If _detalle.Rows.Find(pk) IsNot Nothing Then
                        _detalle.Rows.Find(pk).BeginEdit()
                        _detalle.Rows.Find(pk).Item("cantidad") = iCantidad
                        _detalle.Rows.Find(pk).Item("idRegion") = iIdRegion
                        _detalle.Rows.Find(pk).EndEdit()
                        resultado = True
                    Else
                        resultado = "No se encontró el detalle con el material " & sMaterial & "para realizar la edición"
                    End If
                Else
                    resultado = "No hay detalle para el pedido" & _idPedido
                End If
            Else
                resultado = "No hay existencias disponibles para el material " & sMaterial
            End If
            Return resultado
        End Function

        Public Function EditarRelacionCuarentena(ByVal idDetalleCuarentena As Integer, Optional ByVal cantidad As Integer = 0) As Boolean
            Dim resultado As Boolean = False
            If _detalleLiberacionDespacho IsNot Nothing Then
                Dim pk(0) As DataColumn

                pk(0) = _detalleLiberacionDespacho.Columns("idDetalleCuarentena")
                _detalleLiberacionDespacho.PrimaryKey = pk

                If _detalleLiberacionDespacho.Rows.Find(idDetalleCuarentena) IsNot Nothing Then
                    _detalleLiberacionDespacho.Rows.Find(idDetalleCuarentena).BeginEdit()
                    _detalleLiberacionDespacho.Rows.Find(idDetalleCuarentena).Item("cantidad") = cantidad
                    _detalleLiberacionDespacho.Rows.Find(idDetalleCuarentena).EndEdit()
                    resultado = True
                End If
            End If
            Return resultado
        End Function

        Private Overloads Function ObtenerValorLlavePrimaria(ByVal material As String, ByVal idRegion As Integer) As String()
            Dim arrKey(1) As String
            arrKey(0) = material
            arrKey(1) = idRegion
            Return arrKey
        End Function
        Private Overloads Function ObtenerValorLlavePrimaria(ByVal material As String, ByVal idRegion As Integer, ByVal idDetalleCuarentena As Integer) As String()
            Dim arrKey(2) As String
            arrKey(0) = idDetalleCuarentena
            arrKey(1) = material
            arrKey(2) = idRegion

            Return arrKey
        End Function
        Public Function RegistrarSerialesCuarentena() As Short
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short

            If _tipoLectura > 0 Then
                Try
                    With dbManager
                        dbManager.iniciarTransaccion()
                        EstablecerParametrosLectura(dbManager)
                        .ejecutarNonQuery("RegistrarSerialCuarentena", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@resultado").Value)

                        If resultado = 1 Then
                            If .estadoTransaccional Then .abortarTransaccion()
                            Return resultado
                        End If
                        .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                Return 1
            End If

            Return resultado
        End Function

        Public Function BorrarSerialesDeCuarentena() As Short
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short

            If TipoLectura > 0 Then

                Try
                    With dbManager
                        dbManager.iniciarTransaccion()
                        EstablecerParametrosLectura(dbManager)
                        .ejecutarNonQuery("BorrarSerialDetalleCuarentena", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@resultado").Value)

                        If resultado = 1 Then
                            If .estadoTransaccional Then .abortarTransaccion()
                            Return resultado
                        End If

                        .confirmarTransaccion()
                        CargarDetalle()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                Return 5
            End If

            Return resultado
        End Function

        Public Function ActualizarDetalle(ByVal dbManager As LMDataAccessLayer.LMDataAccess) As Boolean

            Dim vista As New DataView(_detalle)

            With dbManager
                .SqlParametros.Add("@idPedido", SqlDbType.BigInt).Value = _idPedido

                .SqlParametros.Add("@material", SqlDbType.VarChar, 20)
                .SqlParametros.Add("@idRegion", SqlDbType.Int)

                vista.RowStateFilter = DataViewRowState.Deleted
                For Each fila As DataRowView In vista
                    .SqlParametros("@material").Value = fila("material")
                    .SqlParametros("@idRegion").Value = fila("idRegion")
                    .ejecutarNonQuery("EliminarDetallePedido", CommandType.StoredProcedure)
                Next

                'Se aggrega los parametros para actualizacion o adicion de detalles

                .SqlParametros.Add("@cantidad", SqlDbType.SmallInt)
                .SqlParametros.Add("@idTipoUnidad", SqlDbType.SmallInt)

                vista.RowStateFilter = DataViewRowState.ModifiedCurrent
                For Each fila As DataRowView In vista
                    .SqlParametros("@material").Value = fila("material")

                    .SqlParametros("@cantidad").Value = fila("cantidad")
                    .SqlParametros("@idTipoUnidad").Value = fila("idTipoUnidad")
                    .SqlParametros("@idRegion").Value = fila("idRegion")
                    .ejecutarNonQuery("ActualizarDetallePedido", CommandType.StoredProcedure)
                Next

                vista.RowStateFilter = DataViewRowState.Added
                For Each fila As DataRowView In vista
                    .SqlParametros("@material").Value = fila("material")
                    .SqlParametros("@cantidad").Value = fila("cantidad")
                    .SqlParametros("@idTipoUnidad").Value = fila("idTipoUnidad")
                    .SqlParametros("@idRegion").Value = fila("idRegion")
                    .ejecutarNonQuery("CrearDetallePedido", CommandType.StoredProcedure)
                Next
            End With


        End Function

        Public Function ActualizarDetalleLiberacionDetalleCuarentena(ByVal dbManager As LMDataAccessLayer.LMDataAccess) As Boolean
            dbManager.SqlParametros.Clear()

            Dim vista As New DataView(_detalleLiberacionDespacho)

            dbManager.SqlParametros.Add("@idPedido", SqlDbType.SmallInt).Value = _idPedido
            dbManager.SqlParametros.Add("@idDetalleCuarentena", SqlDbType.Int)

            vista.RowStateFilter = DataViewRowState.Deleted
            For Each fila As DataRowView In vista
                dbManager.SqlParametros("@idDetalleCuarentena").Value = fila("idDetalleCuarentena")
                dbManager.ejecutarNonQuery("EliminarDetalleLiberacionDetalleCuarentena", CommandType.StoredProcedure)
            Next

            'Se agrega los parametros para actualizacion de relacion y adicion
            dbManager.SqlParametros.Add("@cantidad", SqlDbType.SmallInt)

            vista.RowStateFilter = DataViewRowState.ModifiedCurrent
            For Each fila As DataRowView In vista
                dbManager.SqlParametros("@idDetalleCuarentena").Value = fila("idDetalleCuarentena")
                dbManager.SqlParametros("@cantidad").Value = fila("cantidad")
                dbManager.ejecutarNonQuery("ActualizarDetalleLiberacionDetalleCuarentena", CommandType.StoredProcedure)
            Next

            vista.RowStateFilter = DataViewRowState.Added
            For Each fila As DataRowView In vista
                dbManager.SqlParametros("@idDetalleCuarentena").Value = fila("idDetalleCuarentena")
                dbManager.SqlParametros("@cantidad").Value = fila("cantidad")
                dbManager.ejecutarNonQuery("CrearDetalleLiberacionDetalleCuarentena", CommandType.StoredProcedure)
            Next
        End Function

        Public Function ActualizarRelacionCuarentenaDespacho(ByVal dbManager As LMDataAccessLayer.LMDataAccess) As Boolean
            dbManager.SqlParametros.Clear()

            Dim vista As New DataView(_detalleLiberacionDespacho)

            dbManager.SqlParametros.Add("@idPedido", SqlDbType.BigInt).Value = _idPedido
            dbManager.SqlParametros.Add("@idDetalleCuarentena", SqlDbType.Int)
            dbManager.SqlParametros.Add("@material", SqlDbType.VarChar, 20)
            dbManager.SqlParametros.Add("@idRegion", SqlDbType.Int)

            vista.RowStateFilter = DataViewRowState.Deleted
            For Each fila As DataRowView In vista
                dbManager.SqlParametros("@idDetalleCuarentena").Value = fila("idDetallePedido")
                dbManager.SqlParametros("@material").Value = fila("material")
                dbManager.SqlParametros("@idRegion").Value = fila("idRegion")
                dbManager.ejecutarNonQuery("EliminarRelacionCuarentenaPedido", CommandType.StoredProcedure)
            Next

            'Se agrega los parametros para actualizacion de relacion y adicion
            dbManager.SqlParametros.Add("@cantidad", SqlDbType.SmallInt)

            vista.RowStateFilter = DataViewRowState.ModifiedCurrent
            For Each fila As DataRowView In vista
                dbManager.SqlParametros("@idDetalleCuarentena").Value = fila("idDetallePedido")
                dbManager.SqlParametros("@material").Value = fila("material")
                dbManager.SqlParametros("@idRegion").Value = fila("idRegion")
                dbManager.SqlParametros("@cantidad").Value = fila("cantidad")
                dbManager.ejecutarNonQuery("ActualizarRelacionCuarentenaPedido", CommandType.StoredProcedure)
            Next

            vista.RowStateFilter = DataViewRowState.Added
            For Each fila As DataRowView In vista
                dbManager.SqlParametros("@idDetalleCuarentena").Value = fila("idDetallePedido")
                dbManager.SqlParametros("@material").Value = fila("material")
                dbManager.SqlParametros("@idRegion").Value = fila("idRegion")
                dbManager.SqlParametros("@cantidad").Value = fila("cantidad")
                dbManager.ejecutarNonQuery("CrearRelacionCuarentenaPedido", CommandType.StoredProcedure)
            Next
        End Function

        Public Sub AdicionarDetalleDespachoTemporal(ByVal idSession As String, Optional ByVal drDetalle As DataRow = Nothing)
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            If drDetalle IsNot Nothing Then
                With dbManager
                    .SqlParametros.Add("@idSession", SqlDbType.VarChar, 40).Value = idSession
                    .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido
                    .SqlParametros.Add("@idDetalleCuarentena", SqlDbType.Int).Value = drDetalle.Item("idDetallePedido")
                    .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Value = drDetalle.Item("material")
                    .SqlParametros.Add("@cantidad", SqlDbType.Int).Value = CInt(drDetalle.Item("cantidad"))
                    .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = drDetalle.Item("idRegion")
                    .ejecutarNonQuery("CrearAuxDetalleCuarentenaDespacho", CommandType.StoredProcedure)
                End With
            End If
        End Sub

        Public Sub EliminarAuxDetalleCuarentenaDespacho(ByVal idSession As String, ByVal intTodo As Integer, Optional ByVal idDetalleCuarentena As Integer = 0)
            Dim dbManager As New LMDataAccessLayer.LMDataAccess

            With dbManager
                .SqlParametros.Add("@idSession", SqlDbType.VarChar, 24).Value = idSession
                .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido
                .SqlParametros.Add("@idDetalleCuarentena", SqlDbType.Int).Value = idDetalleCuarentena
                .SqlParametros.Add("@todo", SqlDbType.Int).Value = intTodo
                .ejecutarNonQuery("EliminarAuxDetalleCuarentenaDespacho", CommandType.StoredProcedure)
            End With
        End Sub

        ''' <summary>
        ''' Asigna una transportadora , tipo de transporte y tipo de movimiento (alistamiento) transporte
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AsignarInformacionDeTransporte(ByVal idPedido As Integer, ByVal idTipoPedido As Integer, Optional ByRef dm As LMDataAccess = Nothing)

            Dim dtInfoTransporte As New DataTable
            Dim MiTipoPedido As New TipoPedido(idTipoPedido)
            Dim vlTipoPedido As New TipoPedido.Tipo

            Dim sbRetorno As New StringBuilder
            Try
                MiTipoPedido = New TipoPedido(idTipoPedido)
                If dm Is Nothing Then dm = New LMDataAccess



                If idTipoPedido = TipoPedido.Tipo.SalidaDeVentas Or idTipoPedido = TipoPedido.Tipo.SalidaDeTraslados _
                Or idTipoPedido = TipoPedido.Tipo.SalidaDeProductoPruebas Or idTipoPedido = TipoPedido.Tipo.DespachoCuarentena Then


                    EstablecerTipoProductoConPrioridadDeDesapcho()
                    With dm
                        .agregarParametroSQL("@idPedido", idPedido)

                        If _idTipoProductoPrioridadDeDespacho > 0 Then _
                        .SqlParametros.Add("@idTipoProductoPrioridadDeDespacho", SqlDbType.Int).Value = _idTipoProductoPrioridadDeDespacho

                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("AsignarInformacionDeTransporteAPedido", CommandType.StoredProcedure)
                        _resultado.Valor = .SqlParametros("@returnValue").Value
                    End With

                    Select Case _resultado.Valor
                        Case 1
                            _resultado.Mensaje = "No Se encontro ciudad destino "
                        Case 2
                            _resultado.Mensaje = "El pedido no tiene ciudad de destino asignada."
                        Case 3
                            _resultado.Mensaje = "El destinarario no esta activo o no tiene tipo de destinatario asignado."
                        Case 4
                            _resultado.Mensaje = "No se encontro información de transporte de acuerdo a los datos del pedido."
                        Case 5
                            _resultado.Mensaje = "Error no especificado."
                        Case 6
                            _resultado.Mensaje = "No se estableciÓ tipo de producto con prioridad de despacho."
                    End Select
                ElseIf _tipoPedido.IdTipo = 0 Then
                    Throw New Exception("Imposible determinar el tipo de pedido.")
                Else
                    _resultado.Valor = 0
                    _resultado.Mensaje = String.Empty
                End If
            Catch ex As Exception
                _resultado.Valor = -1
                _resultado.Mensaje = ex.Message
            End Try
        End Sub

        Public Function ObtenerDisponibilidadDetallePedido(ByVal filtro As FiltroPedido) As DataTable
            Dim dm As New LMDataAccess
            Dim dtDisponibilidadPedido As New DataTable
            Try
                If filtro.IdPedido = 0 And filtro.ListaPedido Is Nothing Then
                    Throw New Exception("La lista de pedidos esta vacia")
                Else
                    With dm
                        If filtro.IdPedido <> 0 Then .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = filtro.IdPedido
                        If filtro.ListaPedido IsNot Nothing AndAlso filtro.ListaPedido.Count > 0 Then _
                            .SqlParametros.Add("@listaPedido", SqlDbType.VarChar, 800).Value = filtro.ListaPedido
                        .TiempoEsperaComando = 120
                        dtDisponibilidadPedido = .ejecutarDataTable("ObtenerDisponibilidadPoolPedido", CommandType.StoredProcedure)
                    End With

                End If

            Finally
                If dm IsNot Nothing Then dm.Dispose()
            End Try
            Return dtDisponibilidadPedido
        End Function
        Public Function ObtenerTrazabilidadPedido() As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim resultado As ResultadoProceso
            Try
                resultado = ConsultarTrazabilidadPedidosFecha(dbManager)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Function ConsultarTrazabilidadPedidosFecha(dbManager As LMDataAccess) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Try
                With dbManager
                    If IdPedido <> 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = IdPedido
                    .TiempoEsperaComando = 190000
                    _dsDatos = .EjecutarDataSet("ConsultarTrazabilidadServicio", CommandType.StoredProcedure)
                    If DsDatos.Tables.Count > 0 Then
                        _dtDatos = _dsDatos.Tables(0)
                        _dtDetalle = _dsDatos.Tables(1)
                        _dtDatosGeneral = _dsDatos.Tables(2)
                    End If
                    .Dispose()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Function ConsultarTrazabilidadPedidos(ByVal numeroEntrega As Long, ByVal fecha As Date) As DataTable
            Dim dm As New LMDataAccess
            Dim dtDatos As New DataTable
            Try
                ''por publicar
                With dm
                    If numeroEntrega <> 0 Then .SqlParametros.Add("@numeroEntrega", SqlDbType.Int).Value = numeroEntrega
                    If fecha <> Date.MinValue Then .SqlParametros.Add("@fecha", SqlDbType.Date).Value = fecha
                    .TiempoEsperaComando = 0
                    dtDatos = .EjecutarDataTable("ConsultarTrazabilidadPedidos", CommandType.StoredProcedure)
                End With

            Finally
                If dm IsNot Nothing Then dm.Dispose()
            End Try
            Return dtDatos
        End Function

        Function ComprobarReferencia(idReferencia As String, idTipo As Integer) As ResultadoProceso
            Dim resultado As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                _referencia = idReferencia
                _tipoReferencia = idTipo
                resultado = ComprobarEntrega(dbManager)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Private Function ComprobarEntrega(dbManager As LMDataAccess) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Try
                With dbManager
                    .SqlParametros.Add("@referencia", SqlDbType.VarChar, 100).Value = _referencia
                    .SqlParametros.Add("@tipoReferencia", SqlDbType.Int).Value = _tipoReferencia
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ComprobarReferencia", CommandType.StoredProcedure)
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    Else
                        resultado.EstablecerMensajeYValor(200, "Imposible evaluar la respuesta del servidor de BD. Por favor intente nuevamente")
                    End If
                    .Dispose()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Sub EstablecerTipoProductoConPrioridadDeDesapcho()
            Dim dm As New LMDataAccess
            Dim arrListaMateriales As New ArrayList
            Try
                If _detalle.Rows.Count > 0 Then
                    For Each dr As DataRow In _detalle.Rows
                        arrListaMateriales.Add(dr("material"))
                    Next

                    dm.SqlParametros.Add("@listaMaterial", SqlDbType.VarChar, 1000).Value = Join(arrListaMateriales.ToArray, ",")
                    dm.ejecutarReader("ObtenerTipoProductoPrioridadDespacho", CommandType.StoredProcedure)
                    If dm.Reader IsNot Nothing And dm.Reader.HasRows Then
                        While dm.Reader.Read
                            _idTipoProductoPrioridadDeDespacho = dm.Reader("idTipoProducto")
                        End While
                    End If
                    If dm.Reader IsNot Nothing Then dm.Reader.Close()
                Else
                    Throw New Exception("No se estableció lista de materiales requerida para establecer informaciond de transporte.")
                End If
            Catch ex As Exception
                If dm.Reader IsNot Nothing Then dm.Reader.Close()
                Throw New Exception(ex.Message)
            Finally
                If dm IsNot Nothing Then dm.Dispose()
            End Try
        End Sub
        Public Sub AnularPedidos(ByRef dtDatos As DataTable, ByVal dtidPedidos As DataTable, ByVal vObservacion As String, ByVal vIdusuario As Integer)
            Try
                dtidPedidos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), vIdusuario))
                Using _dbManager As New LMDataAccess
                    With _dbManager
                        With .SqlParametros
                            .Clear()
                            .Add("@idUsuario", SqlDbType.Int).Value = vIdusuario
                        End With
                        .ejecutarNonQuery("EliminaTempAnularPedido", CommandType.StoredProcedure)
                        .inicilizarBulkCopy()
                        With .BulkCopy
                            .DestinationTableName = "TempAnularPedido"
                            .ColumnMappings.Add("idPedido", "idPedido")
                            .ColumnMappings.Add("idUsuario", "idUsuario")
                            .WriteToServer(dtidPedidos)
                        End With
                        With .SqlParametros
                            .Clear()
                            .Add("@Observacion", SqlDbType.VarChar).Value = vObservacion
                            .Add("@idUsuario", SqlDbType.Int).Value = vIdusuario
                        End With
                        .TiempoEsperaComando = 120
                        dtDatos = .ejecutarDataTable("AnularPedidos", CommandType.StoredProcedure)
                    End With
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function ValidarArchivoMaterialSerial() As Boolean
            Dim errorColumnas As Boolean = False
            Dim indexFila As Integer = 0
            Dim esValido As Boolean = True
            Dim index As Integer = 1
            Try
                If _tablaDatosMaterialSerial Is Nothing Then
                    EstructuraDatosMaterialSerial()
                End If

                If oExcel.Worksheets(0).Rows(0).AllocatedCells.Count <> Me._tablaDatosMaterialSerial.Columns.Count Then
                    AdicionarError(index, "Columnas inválidas", "El Número de columnas es inválido.", "")
                    errorColumnas = True
                End If
                Dim entroEstructura As Boolean = False
                If Not errorColumnas Then
                    For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                        Dim indexColumna As Integer = 0
                        Dim drAux As DataRow
                        Dim entroDatos As Boolean = False
                        drAux = _tablaDatosMaterialSerial.NewRow
                        If entroEstructura = False Then
                            Dim h As Integer
                            For h = 0 To _tablaDatosMaterialSerial.Columns.Count - 1
                                If fila.AllocatedCells(h).Value.ToString.Trim.ToLower <> _tablaDatosMaterialSerial.Columns(h).ColumnName.Trim.ToLower Then
                                    AdicionarError(index, "Archivo Con estructura errada", "El archivo no contiene el orden de columnas esperado.", "")
                                    errorColumnas = True
                                    Exit For
                                End If
                            Next
                        End If
                        entroEstructura = True
                        If errorColumnas Then
                            Exit For
                        End If
                        For Each columna As ExcelCell In fila.AllocatedCells
                            If indexFila > 0 Then
                                If indexColumna <= Me._tablaDatosMaterialSerial.Columns.Count - 1 Then
                                    If fila.AllocatedCells(indexColumna).Value IsNot Nothing Then
                                        entroDatos = True
                                        drAux(indexColumna) = fila.AllocatedCells(indexColumna).Value.ToString.Trim
                                    End If
                                    indexColumna += 1
                                    If indexColumna > _tablaDatosMaterialSerial.Columns.Count - 1 Then
                                        Exit For
                                    End If
                                Else
                                    indexColumna += 1
                                End If
                            Else
                                Exit For
                            End If
                        Next
                        If entroDatos Then
                            _tablaDatosMaterialSerial.Rows.Add(drAux)
                        End If
                        indexFila = indexFila + 1
                    Next
                End If
                If Not errorColumnas Then
                    If _tablaDatosMaterialSerial.Rows.Count = 0 Then
                        AdicionarError(_tablaErrores.Rows.Count + 1, "Archivo Sin Datos", "El archivo no tiene datos para procesar.", "")
                        errorColumnas = True
                    End If
                End If
                If _tablaErrores IsNot Nothing AndAlso _tablaErrores.Rows.Count > 0 Then
                    esValido = Not (_tablaErrores.Rows.Count > 0)
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return esValido
        End Function

        Public Function ValidarArchivoMaterialCantidad() As Boolean
            Dim errorColumnas As Boolean = False
            Dim indexFila As Integer = 0
            Dim esValido As Boolean = True
            Dim index As Integer = 1
            Try
                If _tablaDatosMaterialCantidad Is Nothing Then
                    EstructuraDatosMaterialCantidad()
                End If

                If oExcel.Worksheets(0).Rows(0).AllocatedCells.Count <> Me._tablaDatosMaterialCantidad.Columns.Count Then
                    AdicionarError(index, "Columnas inválidas", "El Número de columnas es inválido.", "")
                    errorColumnas = True
                End If
                Dim entroEstructura As Boolean = False
                If Not errorColumnas Then
                    For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                        Dim indexColumna As Integer = 0
                        Dim drAux As DataRow
                        Dim entroDatos As Boolean = False
                        drAux = _tablaDatosMaterialCantidad.NewRow
                        If entroEstructura = False Then
                            Dim h As Integer
                            For h = 0 To _tablaDatosMaterialCantidad.Columns.Count - 1
                                If fila.AllocatedCells(h).Value.ToString.Trim.ToLower <> _tablaDatosMaterialCantidad.Columns(h).ColumnName.Trim.ToLower Then
                                    AdicionarError(index, "Archivo Con estructura errada", "El archivo no contiene el orden de columnas esperado.", "")
                                    errorColumnas = True
                                    Exit For
                                End If
                            Next
                        End If
                        entroEstructura = True
                        If errorColumnas Then
                            Exit For
                        End If
                        For Each columna As ExcelCell In fila.AllocatedCells
                            If indexFila > 0 Then
                                If indexColumna <= Me._tablaDatosMaterialCantidad.Columns.Count - 1 Then
                                    If fila.AllocatedCells(indexColumna).Value IsNot Nothing Then
                                        entroDatos = True
                                        drAux(indexColumna) = fila.AllocatedCells(indexColumna).Value.ToString.Trim
                                    End If
                                    indexColumna += 1
                                    If indexColumna > _tablaDatosMaterialCantidad.Columns.Count - 1 Then
                                        Exit For
                                    End If
                                Else
                                    indexColumna += 1
                                End If
                            Else
                                Exit For
                            End If
                        Next
                        If entroDatos Then
                            _tablaDatosMaterialCantidad.Rows.Add(drAux)
                        End If
                        indexFila = indexFila + 1
                    Next
                End If
                If Not errorColumnas Then
                    If _tablaDatosMaterialCantidad.Rows.Count = 0 Then
                        AdicionarError(_tablaErrores.Rows.Count + 1, "Archivo Sin Datos", "El archivo no tiene datos para procesar.", "")
                        errorColumnas = True
                    End If
                End If
                If _tablaErrores IsNot Nothing AndAlso _tablaErrores.Rows.Count > 0 Then
                    esValido = Not (_tablaErrores.Rows.Count > 0)
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return esValido
        End Function

        Public Sub ValidarInformacionMaterialSerial(ByVal dtDatos As DataTable)
            Dim i As Integer = 0
            Dim conError As Boolean = False
            Try
                With dtDatos
                    .Columns.Add("especial", GetType(Integer))
                End With
                For i = 0 To dtDatos.Rows.Count - 1
                    If dtDatos.Rows(i).Item("material").ToString.Trim = "" Then
                        conError = True
                        AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "El campo 'Material' no puede ser vacio en el archivo.", "")
                    End If

                    If dtDatos.Rows(i).Item("region").ToString.Trim = "" Then
                        conError = True
                        AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "El campo 'Region' no puede ser vacio en el archivo.", "")
                    End If

                    If dtDatos.Rows(i).Item("serial").ToString.Trim = "" Then
                        conError = True
                        AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "El campo 'Serial' no puede ser vacio en el archivo.", "")
                    End If

                    Dim _existeSerial As Integer = -1
                    _existeSerial = ConsultarSeriales(dtDatos.Rows(i).Item("material").ToString.Trim, dtDatos.Rows(i).Item("region").ToString.Trim, dtDatos.Rows(i).Item("serial").ToString.Trim)
                    If _existeSerial = 0 Then
                        dtDatos.Rows(i).Item("especial") = 1
                    Else
                        dtDatos.Rows(i).Item("especial") = 0
                    End If
                Next
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub ValidarInformacionMaterialCantidad(ByVal dtDatos As DataTable)
            Dim i As Integer = 0
            Dim conError As Boolean = False
            Dim dtCantidad As DataTable
            Dim dtRegion As DataTable
            Try
                With dtDatos
                    .Columns.Add("idRegion", GetType(Integer))
                End With
                dtCantidad = CargarMaterialesCantidad()
                dtRegion = CargarRegion()
                For i = 0 To dtDatos.Rows.Count - 1
                    If dtDatos.Rows(i).Item("material").ToString.Trim = "" Then
                        conError = True
                        AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "El campo 'Material' no puede ser vacio en el archivo.", "")
                    End If

                    If dtDatos.Rows(i).Item("region").ToString.Trim = "" Then
                        conError = True
                        AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "El campo 'Region' no puede ser vacio en el archivo.", "")
                    Else
                        Dim dvRegion() As DataRow
                        dvRegion = dtRegion.Select("codigo='" & dtDatos.Rows(i).Item("region").ToString.Trim & "'")
                        If dvRegion.Length > 0 Then
                            dtDatos.Rows(i).Item("idRegion") = dvRegion(0).Item("idRegion")
                        End If
                    End If

                    If dtDatos.Rows(i).Item("cantidad").ToString.Trim = "" Then
                        conError = True
                        AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "El campo 'cantidad' no puede ser vacio en el archivo.", "")
                    End If

                    Dim dvdatos() As DataRow
                    dvdatos = dtCantidad.Select("material='" & dtDatos.Rows(i).Item("material").ToString.Trim & "' and region='" & dtDatos.Rows(i).Item("region").ToString.Trim & "'")

                    If dvdatos.Length > 0 Then
                        If dvdatos(0).Item("cantidad") < dtDatos.Rows(i).Item("cantidad") Then
                            conError = True
                            AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "La cantidad de seriales para la combinacion material-region es menor a la cantidad solicitada.", "")
                        End If
                    Else
                        conError = True
                        AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "La combinacion material-region no cuenta con registros en la base de datos.", "")
                    End If
                Next
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function CrearPedidoEspecialConSeriales() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultadoEjecucion As New ResultadoProceso

            If _idUsuario > 0 AndAlso _infoSerialEspecial IsNot Nothing AndAlso _infoSerialEspecial.Rows.Count > 0 Then
                Try
                    With dbManager
                        .TiempoEsperaComando = 1200
                        dbManager.iniciarTransaccion()
                        .SqlParametros.Clear()
                        .inicilizarBulkCopy()
                        With .BulkCopy
                            .DestinationTableName = "CargaSerialesPedidosEspecialTemporal"
                            .ColumnMappings.Add("serial", "serial")
                            .ColumnMappings.Add("material", "material")
                            .ColumnMappings.Add("region", "region")
                            .ColumnMappings.Add("especial", "esEspecial")
                            .WriteToServer(_infoSerialEspecial)
                        End With
                        .agregarParametroSQL("@idTipoPedido", _tipoPedido.IdTipo, SqlDbType.SmallInt)
                        .agregarParametroSQL("@idSolicitante", _idSolicitante, SqlDbType.SmallInt)
                        If _observaciones IsNot Nothing Then .agregarParametroSQL("@observaciones", _observaciones.Trim, SqlDbType.VarChar)
                        .agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                        .SqlParametros.Add("@idPedido", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("RegistrarPedidoEspecial", CommandType.StoredProcedure)

                        resultadoEjecucion.EstablecerMensajeYValor(CShort(.SqlParametros("@returnValue").Value), "Ejecución Satisfactoria")
                        If resultadoEjecucion.Valor = 0 Then
                            _idPedido = CLng(.SqlParametros("@idPedido").Value)
                            If .estadoTransaccional Then
                                .confirmarTransaccion()
                                resultadoEjecucion.EstablecerMensajeYValor(0, "El pedido especial " & _idPedido & " fue creado satisfactoriamente.")
                            End If
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            resultadoEjecucion.EstablecerMensajeYValor(1, "Error al crear el pedido, intentelo nuevamente.")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible obtener los datos necesario para crear el pedido.")
            End If

            Return resultadoEjecucion
        End Function

        Public Function CrearPedidoEspecialSinSeriales() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultadoEjecucion As New ResultadoProceso
            If _idUsuario > 0 AndAlso _infoSerialEspecial IsNot Nothing AndAlso _infoSerialEspecial.Rows.Count > 0 Then
                Try
                    With dbManager
                        .TiempoEsperaComando = 1200
                        dbManager.iniciarTransaccion()
                        .SqlParametros.Clear()
                        .agregarParametroSQL("@idTipoPedido", _tipoPedido.IdTipo, SqlDbType.SmallInt)
                        .agregarParametroSQL("@idSolicitante", _idSolicitante, SqlDbType.SmallInt)
                        If _observaciones IsNot Nothing Then .agregarParametroSQL("@observaciones", _observaciones.Trim, SqlDbType.VarChar)
                        .agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                        .SqlParametros.Add("@idPedido", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("RegistrarPedidoEspecial", CommandType.StoredProcedure)
                        resultadoEjecucion.EstablecerMensajeYValor(CShort(.SqlParametros("@returnValue").Value), "Ejecución Satisfactoria")
                        If resultadoEjecucion.Valor = 0 Then
                            _idPedido = CLng(.SqlParametros("@idPedido").Value)
                            With _infoSerialEspecial
                                .Columns.Add("idPedido", GetType(Integer))
                                .Columns.Add("idTipoUnidad", GetType(Integer))
                            End With
                            For i As Integer = 0 To _infoSerialEspecial.Rows.Count - 1
                                _infoSerialEspecial.Rows(i).Item("idPedido") = _idPedido
                                _infoSerialEspecial.Rows(i).Item("idTipoUnidad") = 2
                            Next
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "DetallePedido"
                                .ColumnMappings.Add("idPedido", "idPedido")
                                .ColumnMappings.Add("idRegion", "idRegion")
                                .ColumnMappings.Add("material", "material")
                                .ColumnMappings.Add("cantidad", "cantidad")
                                .ColumnMappings.Add("idTipoUnidad", "idTipoUnidad")
                                .WriteToServer(_infoSerialEspecial)
                            End With
                            If .estadoTransaccional Then
                                .confirmarTransaccion()
                                resultadoEjecucion.EstablecerMensajeYValor(0, "El pedido especial " & _idPedido & " fue creado satisfactoriamente.")
                            End If
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            resultadoEjecucion.EstablecerMensajeYValor(1, "Error al crear el pedido, intentelo nuevamente.")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible obtener los datos necesario para crear el pedido.")
            End If

            Return resultadoEjecucion
        End Function

        Public Function ActualizarPedidosEspecial() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultadoEjecucion As New ResultadoProceso
            If _idUsuario > 0 AndAlso (_infoSerialEspecial IsNot Nothing AndAlso _infoSerialEspecial.Rows.Count > 0 OrElse _
               _infoMaterialEspecial IsNot Nothing AndAlso _infoMaterialEspecial.Rows.Count > 0) Then
                Try
                    With dbManager
                        .TiempoEsperaComando = 1200
                        dbManager.iniciarTransaccion()
                        .SqlParametros.Clear()
                        If _infoSerialEspecial.Rows.Count > 0 Then
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "CargaSerialesPedidosEspecialTemporal"
                                .ColumnMappings.Add("serial", "serial")
                                .ColumnMappings.Add("material", "material")
                                .ColumnMappings.Add("region", "region")
                                .ColumnMappings.Add("especial", "esEspecial")
                                .WriteToServer(_infoSerialEspecial)
                            End With
                            .agregarParametroSQL("@idPedido", _idPedido, SqlDbType.SmallInt)
                            .agregarParametroSQL("@idSolicitante", _idSolicitante, SqlDbType.SmallInt)
                            If _observaciones IsNot Nothing Then .agregarParametroSQL("@observaciones", _observaciones.Trim, SqlDbType.VarChar)
                            .agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                            .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("ActualizarPedidoEspecial", CommandType.StoredProcedure)
                            resultadoEjecucion.EstablecerMensajeYValor(CShort(.SqlParametros("@returnValue").Value), "Ejecución Satisfactoria")
                            If resultadoEjecucion.Valor = 0 Then
                                If .estadoTransaccional Then
                                    .confirmarTransaccion()
                                    resultadoEjecucion.EstablecerMensajeYValor(0, "El pedido especial " & _idPedido & " fue actualizado satisfactoriamente.")
                                End If
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                                resultadoEjecucion.EstablecerMensajeYValor(1, "Error al actualizar el pedido, intentelo nuevamente.")
                            End If
                        Else
                            .agregarParametroSQL("@idPedido", _idPedido, SqlDbType.SmallInt)
                            .agregarParametroSQL("@idSolicitante", _idSolicitante, SqlDbType.SmallInt)
                            If _observaciones IsNot Nothing Then .agregarParametroSQL("@observaciones", _observaciones.Trim, SqlDbType.VarChar)
                            .agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                            .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("ActualizarPedidoEspecial", CommandType.StoredProcedure)
                            resultadoEjecucion.EstablecerMensajeYValor(CShort(.SqlParametros("@returnValue").Value), "Ejecución Satisfactoria")
                            If resultadoEjecucion.Valor = 0 Then
                                _idPedido = CLng(.SqlParametros("@idPedido").Value)
                                With _infoMaterialEspecial
                                    .Columns.Add("idPedido", GetType(Integer))
                                End With
                                For i As Integer = 0 To _infoMaterialEspecial.Rows.Count - 1
                                    _infoMaterialEspecial.Rows(i).Item("idPedido") = _idPedido
                                Next
                                .inicilizarBulkCopy()
                                With .BulkCopy
                                    .DestinationTableName = "DetallePedido"
                                    .ColumnMappings.Add("idPedido", "idPedido")
                                    .ColumnMappings.Add("idRegion", "idRegion")
                                    .ColumnMappings.Add("material", "material")
                                    .ColumnMappings.Add("cantidad", "cantidad")
                                    .ColumnMappings.Add("idTipoUnidad", "idTipoUnidad")
                                    .WriteToServer(_infoMaterialEspecial)
                                End With
                                If .estadoTransaccional Then
                                    .confirmarTransaccion()
                                    resultadoEjecucion.EstablecerMensajeYValor(0, "El pedido especial " & _idPedido & " fue actualizado satisfactoriamente.")
                                End If
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                                resultadoEjecucion.EstablecerMensajeYValor(1, "Error al actualizar el pedido, intentelo nuevamente.")
                            End If
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible obtener los datos necesario para actualizar el pedido especial.")
            End If
            Return resultadoEjecucion
        End Function

        Public Function RegistrarSerialesEspeciales() As Short
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short
            Try
                With dbManager
                    dbManager.iniciarTransaccion()
                    EstablecerParametrosLectura(dbManager)
                    .ejecutarNonQuery("RegistrarSerialEspecial", CommandType.StoredProcedure)
                    resultado = CShort(.SqlParametros("@resultado").Value)

                    If resultado = 1 Then
                        If .estadoTransaccional Then .abortarTransaccion()
                        Return resultado
                    End If
                    .confirmarTransaccion()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Function BorrarSerialesEspeciales() As Short
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short
            Try
                With dbManager
                    dbManager.iniciarTransaccion()
                    EstablecerParametrosLectura(dbManager)
                    .ejecutarNonQuery("BorrarSerialDetalleEspecial", CommandType.StoredProcedure)
                    resultado = CShort(.SqlParametros("@resultado").Value)

                    If resultado = 1 Then
                        If .estadoTransaccional Then .abortarTransaccion()
                        Return resultado
                    End If

                    .confirmarTransaccion()
                    CargarDetalle()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return resultado
        End Function

        Public Function ValidarArchivoPedidoServicioTecnico() As Boolean
            Dim errorColumnas As Boolean = False
            Dim indexFila As Integer = 0
            Dim esValido As Boolean = True
            Dim index As Integer = 1
            Try
                If _tablaDatosPedidoServicioTecnico Is Nothing Then
                    EstructuraDatosPedidoServicioTecnico()
                End If

                If oExcel.Worksheets(0).Rows(0).AllocatedCells.Count <> Me._tablaDatosPedidoServicioTecnico.Columns.Count Then
                    AdicionarError(index, "Columnas inválidas", "El Número de columnas es inválido.", "")
                    errorColumnas = True
                End If
                Dim entroEstructura As Boolean = False
                If Not errorColumnas Then
                    For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                        Dim indexColumna As Integer = 0
                        Dim drAux As DataRow
                        Dim entroDatos As Boolean = False
                        drAux = _tablaDatosPedidoServicioTecnico.NewRow
                        If entroEstructura = False Then
                            If fila.AllocatedCells(0).Value.ToString.Trim.ToLower <> _tablaDatosPedidoServicioTecnico.Columns(0).ColumnName.Trim.ToLower Then
                                AdicionarError(index, "Archivo Con estructura errada", "El archivo no contiene el orden de columnas esperado.", "")
                                errorColumnas = True
                                Exit For
                            End If
                        End If
                        entroEstructura = True
                        If errorColumnas Then
                            Exit For
                        End If
                        For Each columna As ExcelCell In fila.AllocatedCells
                            If indexFila > 0 Then
                                If indexColumna <= Me._tablaDatosPedidoServicioTecnico.Columns.Count - 1 Then
                                    If fila.AllocatedCells(indexColumna).Value IsNot Nothing Then
                                        entroDatos = True
                                        drAux(indexColumna) = fila.AllocatedCells(indexColumna).Value.ToString.Trim
                                    End If
                                    indexColumna += 1
                                    If indexColumna > _tablaDatosPedidoServicioTecnico.Columns.Count - 1 Then
                                        Exit For
                                    End If
                                Else
                                    indexColumna += 1
                                End If
                            Else
                                Exit For
                            End If
                        Next
                        If entroDatos Then
                            _tablaDatosPedidoServicioTecnico.Rows.Add(drAux)
                        End If
                        indexFila = indexFila + 1
                    Next
                End If
                If Not errorColumnas Then
                    If _tablaDatosPedidoServicioTecnico.Rows.Count = 0 Then
                        AdicionarError(_tablaErrores.Rows.Count + 1, "Archivo Sin Datos", "El archivo no tiene datos para procesar.", "")
                        errorColumnas = True
                    Else
                        With _tablaDatosPedidoServicioTecnico
                            .Columns.Add(New DataColumn("material", GetType(String)))
                            .Columns.Add(New DataColumn("cantidad", GetType(String)))
                            .Columns.Add(New DataColumn("idTipoUnidad", GetType(Integer)))
                            .Columns.Add(New DataColumn("idRegion", GetType(Integer)))
                        End With
                    End If
                End If
                If _tablaErrores IsNot Nothing AndAlso _tablaErrores.Rows.Count > 0 Then
                    esValido = Not (_tablaErrores.Rows.Count > 0)
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return esValido
        End Function

        Public Sub ValidarInformacionPedidoServicioTecnico(ByVal dtDatos As DataTable)
            Dim i As Integer = 0
            Dim conError As Boolean = False
            Try
                For i = 0 To dtDatos.Rows.Count - 1
                    If dtDatos.Rows(i).Item("serial").ToString.Trim = "" Then
                        conError = True
                        AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "El campo 'Serial' no puede ser vacio en el archivo.", "")
                    End If
                    Dim _dtDetalle As DataTable
                    _dtDetalle = ConsultarSerialesServicioTecnico(dtDatos.Rows(i).Item("serial").ToString.Trim)
                    If _dtDetalle.Rows.Count = 0 Then
                        conError = True
                        AdicionarError(TablaErrores.Rows.Count + 1, "Datos inválidos", "El serial no se encuentra registrado en la base de datos ó no pertenece al inventario activo.", dtDatos.Rows(i).Item("serial").ToString.Trim)
                    Else
                        dtDatos.Rows(i).Item("material") = _dtDetalle.Rows(0).Item("material")
                        dtDatos.Rows(i).Item("cantidad") = _dtDetalle.Rows(0).Item("cantidad")
                        dtDatos.Rows(i).Item("idTipoUnidad") = _dtDetalle.Rows(0).Item("idTipoUnidad")
                        dtDatos.Rows(i).Item("idRegion") = _dtDetalle.Rows(0).Item("idRegion")
                    End If
                Next
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function CrearPedidoServicioTecnico() As ResultadoProceso
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultadoEjecucion As New ResultadoProceso
            If _idUsuario > 0 AndAlso _infoPedidoServicioTecnico IsNot Nothing AndAlso _infoPedidoServicioTecnico.Rows.Count > 0 Then
                Try
                    With dbManager
                        .TiempoEsperaComando = 1200
                        .iniciarTransaccion()
                        .SqlParametros.Clear()
                        .agregarParametroSQL("@idTipoPedido", _tipoPedido.IdTipo, SqlDbType.SmallInt)
                        .agregarParametroSQL("@idSolicitante", _idSolicitante, SqlDbType.SmallInt)
                        If _observaciones IsNot Nothing Then .agregarParametroSQL("@observaciones", _observaciones.Trim, SqlDbType.VarChar)
                        .agregarParametroSQL("@idUsuario", _idUsuario, SqlDbType.Int)
                        .SqlParametros.Add("@idPedido", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("RegistrarPedidoServicioTecnico", CommandType.StoredProcedure)
                        resultadoEjecucion.EstablecerMensajeYValor(CShort(.SqlParametros("@returnValue").Value), "Ejecución Satisfactoria")
                        If resultadoEjecucion.Valor = 0 Then
                            _idPedido = CLng(.SqlParametros("@idPedido").Value)
                            .inicilizarBulkCopy()
                            With _infoPedidoServicioTecnico
                                .Columns.Add("idPedido", GetType(Integer), IdPedido)
                            End With
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "AuxDetallePedidoServicioTecnico"
                                .ColumnMappings.Add("idPedido", "idPedido")
                                .ColumnMappings.Add("serial", "serial")
                                .ColumnMappings.Add("material", "material")
                                .ColumnMappings.Add("idRegion", "idRegion")
                                .ColumnMappings.Add("cantidad", "cantidad")
                                .WriteToServer(_infoPedidoServicioTecnico)
                            End With
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            .ejecutarNonQuery("RegistrarDetallePedidoServicioTecnico", CommandType.StoredProcedure)
                            resultadoEjecucion.EstablecerMensajeYValor(CShort(.SqlParametros("@returnValue").Value), "Ejecución Satisfactoria")
                            If resultadoEjecucion.Valor = 0 Then
                                If .estadoTransaccional Then
                                    .confirmarTransaccion()
                                    resultadoEjecucion.EstablecerMensajeYValor(0, "El pedido de Servicio Tecnico " & _idPedido & " fue creado satisfactoriamente.")
                                End If
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                                resultadoEjecucion.EstablecerMensajeYValor(1, "Error al crear el pedido de Servicio Tecnico, intentelo nuevamente.")
                            End If
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            resultadoEjecucion.EstablecerMensajeYValor(1, "Error al crear el pedido de Servicio Tecnico, intentelo nuevamente.")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultadoEjecucion.EstablecerMensajeYValor(1, "No fue posible obtener los datos necesario para crear el pedido.")
            End If

            Return resultadoEjecucion
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListadoPedidos() As DataTable
            Dim filtro As New FiltroPedido
            Dim dtPedido As DataTable = ObtenerListadoPedidos(filtro)
            Return dtPedido
        End Function

        Public Overloads Shared Function ObtenerListadoPedidos(ByVal filtro As FiltroPedido) As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                Dim IdSolicitante As Integer
                With filtro
                    If .IdPedido <> 0 Then dbManager.agregarParametroSQL("@idPedido", .IdPedido, SqlDbType.Int)
                    If .idPedidoClienteExterno <> 0 Then dbManager.agregarParametroSQL("@idPedidoClienteExterno", .idPedidoClienteExterno, SqlDbType.BigInt)
                    If .IdCiudadDestino <> 0 Then dbManager.agregarParametroSQL("@idCiudad", .IdCiudadDestino, SqlDbType.Int)
                    If .IdTipoPedido <> 0 Then dbManager.agregarParametroSQL("@idTipoPedido", .IdTipoPedido, SqlDbType.SmallInt)
                    If .IdEstado <> 0 Then dbManager.agregarParametroSQL("@idEstado", .IdEstado, SqlDbType.Int)
                    If .idEntregaClienteExterno <> 0 Then dbManager.agregarParametroSQL("@idEntregaClienteExterno", .idEntregaClienteExterno, SqlDbType.BigInt)
                    If .IdTipoTransporte <> 0 Then dbManager.agregarParametroSQL("@idTipoTransporte", .IdTipoTransporte, SqlDbType.Int)
                    If .IdTransportadora <> 0 Then dbManager.agregarParametroSQL("@idTransportadora", .IdTransportadora, SqlDbType.Int)
                    If .IdTipoAlistamiento <> 0 Then dbManager.agregarParametroSQL("@IdTipoAlistamiento", .IdTipoAlistamiento, SqlDbType.Int)
                    If .IdCliente <> 0 Then dbManager.agregarParametroSQL("@idCliente", .IdCliente, SqlDbType.Int)
                    If .IdSolicitante <> 0 Then dbManager.agregarParametroSQL("@IdSolicitante", .IdSolicitante, SqlDbType.Int)
                    If .IdPickingList <> 0 Then dbManager.agregarParametroSQL("@idPickingList", .IdPickingList, SqlDbType.Int)

                    If .FechaInicial IsNot Nothing AndAlso .FechaInicial IsNot Nothing Then
                        dbManager.agregarParametroSQL("@fechaInicial", CDate(.FechaInicial), SqlDbType.SmallDateTime)
                        dbManager.agregarParametroSQL("@fechaFinal", CDate(.FechaFinal), SqlDbType.SmallDateTime)
                    End If

                    If .ListaPedido IsNot Nothing AndAlso .ListaPedido.Trim.Length > 0 Then _
                    dbManager.agregarParametroSQL("@ListaPedido", .ListaPedido, SqlDbType.VarChar, 400)

                    If .ListaNumeroPedido IsNot Nothing AndAlso .ListaNumeroPedido.Trim.Length > 0 Then _
                    dbManager.agregarParametroSQL("@listaNumeroPedido", .ListaNumeroPedido, SqlDbType.VarChar, 400)

                    If .ListaEstado IsNot Nothing AndAlso .ListaEstado.Trim.Length > 0 Then _
                    dbManager.agregarParametroSQL("@listaEstado", .ListaEstado, SqlDbType.VarChar, 200)

                    If .ListaTipoPedido IsNot Nothing AndAlso .ListaTipoPedido.Trim.Length > 0 Then _
                    dbManager.agregarParametroSQL("@listaTipoPedido", .ListaTipoPedido, SqlDbType.VarChar, 200)

                End With

                Return dbManager.ejecutarDataTable("ObtenerInformacionPedido", CommandType.StoredProcedure)
            Finally
                dbManager.Dispose()
            End Try

        End Function

        Public Shared Function ObtenerPorId(ByVal idPedido As Integer) As DataTable
            Dim filtro As New FiltroPedido
            filtro.IdPedido = idPedido
            Return ObtenerListadoPedidos(filtro)
        End Function

        Public Shared Function ObtenerListadoPedidosCuarentena(ByVal filtro As FiltroPedido) As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With filtro
                    If .IdEstado <> 0 Then dbManager.agregarParametroSQL("@idEstado", .IdEstado, SqlDbType.Int)
                    If .IdPedido <> 0 Then dbManager.agregarParametroSQL("@idPedido", .IdPedido, SqlDbType.Int)
                    If .IdTipoPedido <> 0 Then dbManager.agregarParametroSQL("@idTipoPedido", .IdTipoPedido, SqlDbType.SmallInt)
                    If .IdUsuario <> 0 Then dbManager.agregarParametroSQL("@idUsuario", .IdUsuario, SqlDbType.Int)
                    If .IdSolicitante <> 0 Then dbManager.agregarParametroSQL("@idSolicitante", .IdSolicitante, SqlDbType.Int)
                    If .Material IsNot Nothing AndAlso .Material.Trim.Length > 0 Then dbManager.agregarParametroSQL("@material", .Material, SqlDbType.VarChar, 20)
                    If .ListaPedido IsNot Nothing AndAlso .ListaPedido.Trim.Length > 0 Then dbManager.agregarParametroSQL("@arrPedidos", .ListaPedido, SqlDbType.VarChar, 8000)
                    If .idPedidoDespachoEdicion > 0 Then dbManager.agregarParametroSQL("@idPedidoDesapachoEdicion", .idPedidoDespachoEdicion, SqlDbType.Int)
                End With
                Return dbManager.ejecutarDataTable("ObtenerInfoPedidoCuarentena", CommandType.StoredProcedure)
            Finally
                dbManager.Dispose()
            End Try
        End Function

        Public Shared Function ObtenerListadoCuarentenas(ByVal filtro As FiltroPedidoCuarentena) As DataTable
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                With filtro
                    If .IdPedido > 0 Then dbManager.agregarParametroSQL("@idPedido", .IdPedido, SqlDbType.Int)
                    If .IdCreador > 0 Then dbManager.agregarParametroSQL("@idUsuario", .IdCreador, SqlDbType.Int)
                    If .IdSolicitante > 0 Then dbManager.agregarParametroSQL("@idSolicitante", .IdSolicitante, SqlDbType.Int)
                    If .FechaInicial <> Date.MinValue Then dbManager.agregarParametroSQL("@fechaInicial", .FechaInicial, SqlDbType.SmallDateTime)
                    If .FechaFinal <> Date.MinValue Then dbManager.agregarParametroSQL("@fechaFinal", .FechaFinal, SqlDbType.SmallDateTime)
                    If .Serial IsNot Nothing AndAlso .Serial.Trim.Length > 0 Then dbManager.agregarParametroSQL("@Serial", .Serial, SqlDbType.VarChar, 20)
                End With
                Return dbManager.ejecutarDataTable("ObtenerListadoCuarentenas", CommandType.StoredProcedure)
            Finally
                dbManager.Dispose()
            End Try
        End Function

        Public Shared Function ObtenerUsuariosPool() As DataTable
            Dim dmManager As New LMDataAccessLayer.LMDataAccess
            Dim sqlQuery As String
            Try
                sqlQuery = "SELECT idtercero, tercero  FROM terceros WHERE idperfil in (105,106,4,) AND estado=1"
                Return dmManager.ejecutarDataTable(sqlQuery, CommandType.Text)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function

        Public Shared Function ObtenerDatosBodega(ByVal idUsuario As Integer) As InfoBodegas
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim infoBodega As Estructuras.InfoBodegas
            Try
                With dbManager
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@estado", SqlDbType.Int).Value = 1
                    .ejecutarReader("ObtenerInfoBodegaRegion", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing And .Reader.HasRows Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idBodega").ToString, infoBodega.idBodega)
                            infoBodega.centro = .Reader("centro")
                            infoBodega.almacen = .Reader("almacen")
                        End If
                    End If
                    If .Reader IsNot Nothing Then .Reader.Close()
                End With

                Return infoBodega

            Catch ex As Exception
                Throw New Exception("Error al consultar información de bodega " & ex.Message)
            End Try
        End Function

        Public Shared Function ExisteMaterialCuarentena(ByVal material As String, ByVal cantidad As Integer) As Short
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim result As Short
            With db
                With .SqlParametros
                    If material.Trim.Length > 0 Then .Add("@material", SqlDbType.BigInt).Value = material
                    If cantidad > 0 Then .Add("@cantidad", SqlDbType.Int).Value = cantidad
                    .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    .ejecutarNonQuery("ExisteMaterialCuarentena", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, result)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return result
        End Function

        Public Shared Function ValidarSerialesDeCuarentena(ByVal serial As String, ByVal idDetallePedido As Integer, _
            ByVal tipoLectura As Integer) As Short

            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short

            Try
                With dbManager
                    With .SqlParametros
                        .Add("@serial", SqlDbType.VarChar, 20).Value = serial
                        .Add("@idDetallePedido", SqlDbType.Int).Value = idDetallePedido
                        .Add("@tipoLectura", SqlDbType.Int).Value = tipoLectura
                        .Add("@resultado", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ValidarSerialesDeCuarentena", CommandType.StoredProcedure)
                    resultado = CShort(.SqlParametros("@resultado").Value)

                    .confirmarTransaccion()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return resultado
        End Function

        Public Shared Function ValidarSerialesEspeciales(ByVal serial As String, ByVal idDetallePedido As Integer, _
            ByVal tipoLectura As Integer) As Short

            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Short

            Try
                With dbManager
                    With .SqlParametros
                        .Add("@serial", SqlDbType.VarChar, 20).Value = serial
                        .Add("@idDetallePedido", SqlDbType.Int).Value = idDetallePedido
                        .Add("@resultado", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ValidarSerialesEspeciales", CommandType.StoredProcedure)
                    resultado = CShort(.SqlParametros("@resultado").Value)

                    .confirmarTransaccion()
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return resultado
        End Function

        Public Shared Function ObtenerDetallePedido(ByVal filtro As FiltroPedido) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDetalle As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdPedido > 0 Then dbManager.agregarParametroSQL("@idPedido", filtro.IdPedido, SqlDbType.Int)
                    End With
                    dtDetalle = .ejecutarDataTable("ObtenerDetallePedido", CommandType.StoredProcedure)

                    Dim pk(1) As DataColumn
                    pk(0) = dtDetalle.Columns("material")
                    pk(1) = dtDetalle.Columns("idRegion")
                    dtDetalle.PrimaryKey = pk
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDetalle
        End Function

        Public Shared Function ObtenerCuarentenasDisponibles(ByVal filtro As FiltroPedido) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDetalle As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdPedido > 0 Then dbManager.agregarParametroSQL("@idPedido", filtro.IdPedido, SqlDbType.Int)
                        If filtro.IdPedidoDespacho > 0 Then dbManager.agregarParametroSQL("@idPedidoDespacho", filtro.IdPedidoDespacho, SqlDbType.Int)
                        If filtro.idPedidoDespachoEdicion > 0 Then dbManager.agregarParametroSQL("@idPedidoDespachoEdicion", filtro.idPedidoDespachoEdicion, SqlDbType.Int)
                    End With
                    dtDetalle = .ejecutarDataTable("ObtenerCuarentenasDisponibles", CommandType.StoredProcedure)

                    Dim pk(1) As DataColumn
                    pk(0) = dtDetalle.Columns("material")
                    pk(1) = dtDetalle.Columns("idRegion")
                    dtDetalle.PrimaryKey = pk
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDetalle
        End Function

        Public Shared Function ObtenerCuarentenasDeDespacho(ByVal filtro As FiltroPedido) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDetalle As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdPedido > 0 Then dbManager.agregarParametroSQL("@idPedido", filtro.IdPedido, SqlDbType.Int)
                    End With
                    dtDetalle = .ejecutarDataTable("ObtenerCuarentenasDeDespacho", CommandType.StoredProcedure)

                    Dim pk(1) As DataColumn
                    pk(0) = dtDetalle.Columns("material")
                    pk(1) = dtDetalle.Columns("idRegion")
                    dtDetalle.PrimaryKey = pk
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDetalle
        End Function

        Public Shared Function ObtenerDetalleCuarentenaDeLiberacion(ByVal filtro As FiltroPedido) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDetalle As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdPedido > 0 Then dbManager.agregarParametroSQL("@idPedido", filtro.IdPedido, SqlDbType.Int)
                    End With

                    dtDetalle = .ejecutarDataTable("ObtenerDetalleCuarentenaDeLiberacion", CommandType.StoredProcedure)

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDetalle
        End Function

#End Region

#Region "ENUMS"

        Public Enum EstadoPedido
            Pendiente = 7
            EnProceso = 8
            EnDespacho = 9
            Despachado = 10
            Anulado = 11
            AtendidoBodega = 34
            DespachadoParcialmente = 67
            Liberado = 68
            LiberadoParcialmente = 69
        End Enum

        Public Enum PerfilesPoolPedido
            AdminDespachos = 4
            AsistDespachos = 5
            AdminClienteExterno = 105
            AsistClienteExterno = 106
            AdminBodega = 38
            AsistBodega = 35
            AdminInfoOpera = 108
            AsistInfoOpera = 109
        End Enum

#End Region

    End Class
End Namespace
