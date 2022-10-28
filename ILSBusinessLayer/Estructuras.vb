Imports ILSBusinessLayer.Enumerados

Namespace Estructuras

    Public Structure FiltroGeneral
        Dim Identificador As Integer
        Dim Nombre As String
        Dim IdCiudad As Integer
        Dim Activo As EstadoBinario
    End Structure

    Public Structure FiltroFabricante
        Dim IdFabricante As Integer
        Dim Nombre As String
        Dim IdCiudad As Integer
        Dim Activo As EstadoBinario
        Dim IdTipoProducto As Short
    End Structure

    Public Structure FiltrosOTB
        Dim IdUsuario As Integer
        Dim IdOrdenRecepcion As Integer
        Dim IdOTB As Integer
        Dim IdOTBOrigen As Integer
        Dim IdOTBDestino As Integer
        Dim IdBodega As Integer
        Dim Serial As String
    End Structure

    Public Structure FiltrosOrdenRecepcionSatelite
        Dim IdUsuario As Integer
        Dim IdOrdenRecepcion As Integer
        Dim IdTipoRecepcion As Integer
        Dim NumeroOrden As String
        Dim IdBodega As Integer
        Dim NumeroGuia As String
        Dim link As String
        Dim IdTransportadora As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim valida As Integer
        Dim idDocumento As Integer
        Dim IdEstado As Integer
        Dim TipoDeclaracion As Integer
    End Structure


    Public Structure FiltroDespachoSinPedidoSatelite
        Dim IdUsuario As Integer
        Dim IdPedido As Decimal
        Dim IdDespacho As Decimal
        Dim NumeroPedido As String
        Dim IdBodegaOrigen As Integer
        Dim IdTipoServicio As Integer
        Dim IdTipoPedido As Integer
        Dim IdBodegaDestino As Integer
        Dim Observaciones As String
        Dim IdSubProducto As Decimal
        Dim FechaIncio As Date
        Dim FechaFin As Date
        Dim IdEstado As Integer
        Dim ListRango As String
        Dim IdProducto As Decimal
        Dim IdSubProducto2 As String
        Dim NumeroGuia As String
        Dim IdTransportadora As Integer
        Dim IdMotorizado As Integer
        Dim IdTipoTransporte As Integer
        Dim NumeroDocumentoCliente As String
        Dim EsAsignacionGuia As Boolean
        Dim opcion As Integer
        Dim ListaBodegas As String
    End Structure


    Public Structure FiltroProducto
        Dim IdProducto As Integer
        Dim Nombre As String
        Dim Codigo As String
        Dim IdTecnologia As Integer
        Dim IdFabricante As Integer
        Dim IdProveedor As Integer
        Dim IdTipoProducto As Short
        Dim Activo As EstadoBinario
        Dim SeparadorProveedor As String
    End Structure

    Public Structure FiltroListaProducto
        Dim IdProducto As Integer
        Dim IdTecnologia As Integer
        Dim IdFabricante As Integer
        Dim IdProveedor As Integer
        Dim IdTipoProducto As Short
        Dim Activo As EstadoBinario
        Dim ListaIdTipoProducto As ArrayList
    End Structure

    Public Structure FiltroTipoProducto
        Dim IdTipoProducto As Short
        Dim Descripcion As String
        Dim Instruccionable As EstadoBinario
        Dim Activo As EstadoBinario
        Dim ExisteModulo As EstadoBinario
        Dim IdModulo As Integer
        Dim tipoAplicativo As Short
        Dim listaNoCargar As ArrayList
        Dim Pesado As EstadoBinario
        Dim EsSerialziado As EstadoBinario
    End Structure

    Public Structure FiltroUnidadEmpaque
        Dim IdTipoUnidad As Short
        Dim Descripcion As String
        Dim Material As String
        Dim Activo As EstadoBinario
    End Structure

    Public Structure FiltroTecnologia
        Dim IdTecnologia As Integer
        Dim Descripcion As String
        Dim Codigo As String
        Dim Activo As EstadoBinario
    End Structure

    Public Structure FiltroPais
        Dim IdPais As Short
        Dim Nombre As String
        Dim CodigoAlpha As String
    End Structure

    Public Structure FiltroCiudad
        Dim IdCiudad As Integer
        Dim Nombre As String
        Dim Departamento As String
        Dim IdPais As Short
        Dim IdRegion As Short
        Dim Activo As EstadoBinario
    End Structure

    Public Structure FiltroOrdenCompra
        Dim IdOrden As Long
        Dim NumeroOrden As String
        Dim IdTipoProducto As Integer
        Dim IdProveedor As Integer
        Dim IdMoneda As Integer
        Dim IdIncoterm As Integer
        Dim IdEstado As Integer
        Dim IdCreador As Long
        Dim FechaCreacion As Date
        Dim Observacion As String
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim ListaEstado As ArrayList
        Dim IdNumeroOrden As String
        Dim ProductoRecibido As EstadoBinario
        Dim FechaPrevista As Date
        Dim CantidadPendiente As EstadoBinario
    End Structure

    Public Structure FiltroConsignatario
        Dim IdConsignatario As Integer
        Dim Nombre As String
        Dim Propio As EstadoBinario
        Dim Predeterminado As EstadoBinario
        Dim Activo As EstadoBinario
    End Structure

    Public Structure FiltroClienteExterno
        Dim IdClienteExterno As Integer
        Dim Nombre As String
        Dim Activo As EstadoBinario
        Dim EsFinanciero As Boolean
    End Structure

    Public Structure FiltroInstruccion
        Dim idInstruccion As Long
        Dim idFacturaGuia As Long
        Dim idRegion As Integer
        Dim idTipoInstruccion As Integer
        Dim material As String
        Dim prioridad As Integer
        Dim cantidad As Integer
        Dim idEstado As Integer
        Dim idCreador As Integer
        Dim idClasificacion As Integer
        Dim mostrarOcultos As EstadoBinario
        Dim obtenerActivas As Boolean
    End Structure

    Public Structure FiltroOrdenTrabajo
        Dim IdOrden As Long
        Dim Codigo As String
        Dim IdInstruccion As Long
        Dim Linea As Integer
        Dim idCreador As Long
        Dim factura As String
        Dim guia As String
        '***fechas
        'dim fechaCreacion As Date
        'dim fechaFinalizacion As String
        Dim IdEstado As Integer
        Dim LeerSimSuelta As Boolean
        Dim Revisada As Boolean
        Dim cargarActivas As Boolean
        Dim IdOperador As Integer
        Dim fechaFinalizacionInicial As Date
        Dim fechaFinalizacionFinal As Date
        Dim fechaCreacionInicial As Date
        Dim fechaCreacionFinal As Date
        Dim fechaCierreInicial As Date
        Dim fechaCierreFinal As Date
        Dim idModificador As Integer
        Dim idRegion As Integer
        Dim idFactura As Integer
    End Structure

    Public Structure FiltroPedido
        Dim IdPedido As Integer
        Dim idPedidoClienteExterno As Long
        Dim IdCiudadDestino As Integer
        Dim IdTipoPedido As Short
        Dim IdEstado As Short
        Dim idEntregaClienteExterno As Long
        Dim IdTipoTransporte As Integer
        Dim IdTransportadora As Integer
        Dim IdTipoAlistamiento As Integer
        Dim IdCliente As Integer
        Dim IdUsuario As Integer
        Dim IdPickingList As Integer
        Dim Material As String
        Dim FechaInicial As String
        Dim FechaFinal As String
        Dim ListaPedido As String
        Dim ListaNumeroPedido As String
        Dim ListaEstado As String
        Dim ListaTipoPedido As String
        Dim IdPedidoDespacho As Integer
        Dim EsEdicionLiberacionCuarentena As EstadoBinario
        Dim idPedidoDespachoEdicion As Integer
        Dim IdSolicitante As Integer

    End Structure

    Public Structure FiltroPickig
        Dim IdPicking As Integer
        Dim Idpedido As Integer
        Dim IdEstado As Short
        Dim Material As String
        Dim FechaInicial As String
        Dim FechaFinal As String
        Dim FechaCreado As String
        Dim FechaAtendio As String

    End Structure

    Public Structure FiltroTipoPedido
        Dim IdTipoPedido As Short
        Dim Nombre As String
        Dim Activo As EstadoBinario
        Dim ListaTipoPedido As String
    End Structure

    Public Structure FiltroEnvioSerial
        Dim NumeroNacionalizacion As String
        Dim idRegion As Long
    End Structure

    Public Structure FiltroEnvioNacionalizacionSerial
        Dim Entrega As Long
        Dim Pedido As Long
        Dim IdEnvio As Long
        Dim Material As String
        Dim Centro As String
        Dim Cantidad As Integer
        Dim IdOrden As Long
    End Structure

    Public Structure FiltroEnvio
        Dim IdEnvio As Long
        Dim IdFacturaGuia As Long
        Dim IdFactura As Long
        Dim IdGuia As Long
        Dim IdCreador As Long
        Dim IdEstado As Integer
        Dim FechaCreacion As Date
        Dim FechaCierre As Date
        Dim IdUsuarioCierre As Long
        Dim NombreDocumentoImportacion As String
        Dim Observacion As String
        Dim IdOrden As Long
        Dim Serial As String
        Dim CodigoOrden As String
        Dim IncluirNoConformes As EstadoBinario
    End Structure

    Public Structure FiltroDetalleEnvio
        Dim IdEnvio As Long
        Dim IdDetalleEnvio As Long
        Dim Material As String
        Dim Region As Integer
    End Structure

    Public Structure FiltroOrdenNacionalizacion
        Dim IdOrden As Long
        Dim IdCreador As Long
        Dim IdEstado As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim IdFactura As Integer
        Dim Serial As String
    End Structure

    Public Structure FiltroTermosellado
        Dim IdOrdenTermosellado As Long
        Dim IdCreador As Long
        Dim FechaCreacion As Date
        Dim IdEstado As Integer
        Dim FechaCierre As Date
        Dim IdUsuarioCierre As Long
        Dim Serial As String
        Dim IdFactura As Long
        Dim Region As String
        Dim Estiba As Integer
        Dim Caja As Integer
        Dim OTB As Long
        Dim factura As String
        Dim guia As String
        Dim ordenCompra As String
        Dim pendienteTermosellado As Boolean
        Dim idFacturaGuia As Integer
    End Structure

    Public Structure FiltroTipoUnidad
        Dim IdTipoUnidad As Integer
        Dim Descripcion As String
        Dim Activo As EstadoBinario
    End Structure

    Public Structure FiltroTipoDistribucion
        Dim IdTipoDistribucion As Integer
        Dim Descripcion As String
        Dim OrdenOrdinal As Integer
    End Structure

    Public Structure FiltroInstruccionOrdenCompra
        Dim IdInstruccion As Integer
        Dim IdDetalleOrdenCompra As Integer
        Dim IdSubproducto As Integer
        Dim IdTipoDistribucion As Integer
        Dim Porcentaje As Decimal
        Dim IdEstado As Integer
    End Structure

    Public Structure FiltroSubdistribucionInstruccion
        Dim IdSubdistribucion As Short
        Dim IdRegionEquivalente As Integer
        Dim Codigo As String
        Dim Nombre As String
        Dim Activo As EstadoBinario
        Dim IdRegionPadre As Integer
        Dim IdTipoInstruccionPadre As Integer
    End Structure

    Public Structure FiltroPreInsPorcentajeSubdistribucion
        Dim IdCantidad As Integer
        Dim IdPreinstruccion As Integer
        Dim IdSubdistribucion As Short
        Dim Porcentaje As Decimal
        Dim Cantidad As Integer
        Dim IdUsuario As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
    End Structure

    Public Structure FiltroPreInsPorcentajeRegion
        Dim IdPorcentaje As Integer
        Dim IdPreinstruccion As Integer
        Dim IdRegion As Integer
        Dim Porcentaje As Decimal
        Dim IdUsuario As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
    End Structure

    Public Structure FiltroPreInsCantidadDistribucion
        Dim IdCantidad As Integer
        Dim IdPreinstruccion As Integer
        Dim IdRegion As Integer
        Dim IdTipoInstruccion As Short
        Dim Cantidad As Integer
        Dim IdUsuario As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
    End Structure

    Public Structure FiltroPreinstruccionCliente
        Dim IdPreinstruccion As Integer
        Dim IdOrdenCompra As Integer
        Dim IdDetalleOrdenCompra As Integer
        Dim IdFactura As Integer
        Dim Prioridad As Short
        Dim IdEstado As Short
        Dim CantidadInstruccionada As Integer
        Dim IdUsuario As Integer
        Dim NoAnulada As Integer
    End Structure

    Public Structure FiltroDestinatario
        Dim IdCliente As Integer
        Dim Cliente As String
        Dim IdCiudad As Integer
        Dim NombreRegion As String
        Dim Estado As EstadoBinario
    End Structure

    Public Structure FiltroTransportadora
        Dim IdTransportadora As Integer
        Dim UsaGuia As Boolean
        Dim UsaPrecinto As Boolean
        Dim AplicaLogisticaInversa As Short
        Dim CargaPorImportacion As Short
        Dim Activo As EstadoBinario
        Dim ManejaPos As String
        Dim IdTipoTransporte As Short
    End Structure

    Public Structure FiltroSubproducto
        Dim IdSubproducto As Integer
        Dim Material As String
        Dim idRegion As Integer
        Dim Subproducto As String
        Dim Estado As EstadoBinario
        Dim IdProducto As Integer
        Dim IdtipoOrden As Integer
        Dim IdTipoInstruccion As Integer
        Dim IdTipoProducto As Integer
        Dim EsSerializado As EstadoBinario
    End Structure

    Public Structure FiltroSubproductoInstruccionamiento
        Dim IdSubproducto As Integer
        Dim Material As String
        Dim Subproducto As String
        Dim Estado As EstadoBinario
        Dim IdProducto As Integer
        Dim Referencia As String
        Dim IdTipoOrden As Integer
        Dim IdTipoInstruccion As Integer
        Dim IdClasificacionInstruccion As Integer
    End Structure

    Public Structure FiltroInfoGuia
        Dim IdGuia As Integer
        Dim IdOrdenCompra As Integer
        Dim Guia As String
        Dim IdTransportador As Integer
        Dim IdCiudadOrigen As Integer
        Dim IdFactura As Integer
        Dim Activo As EstadoBinario
        Dim EstadoOrdenCompra As Integer
        Dim ListaEstado As ArrayList
    End Structure

    Public Structure FiltroInfoFactura
        Dim IdFactura As Integer
        Dim IdDetalleOrdenCompra As Integer
        Dim Factura As String
        Dim IdCiudadCompra As Integer
        Dim IdGuia As Integer
        Dim IdEstado As Integer
        Dim IdOrdenCompra As Integer
        Dim IdProveedor As Integer
        Dim ListaEstado As ArrayList

    End Structure

    Public Structure FiltroPosicionBodega
        Dim IdPosicion As Integer
        Dim IdBodega As Integer
        Dim Codigo As String
        Dim IdProducto As Integer
        Dim Material As String
        Dim IdClasificacion As Short
        Dim IdRegion As Short
        Dim FechaVencimientoInicial As Date
        Dim FechaVencimientoFinal As Date
        Dim FechaRecepcionInicial As Date
        Dim FechaRecepcionFinal As Date
        Dim BodegaActiva As Enumerados.EstadoBinario
        Dim CodigoRegion As String
    End Structure

    Public Structure FiltroDetalleOrdenCompra
        Dim IdDetalle As Integer
        Dim IdOrden As Integer
        Dim IdFabricante As Integer
        Dim IdProducto As Integer
        Dim IdTipoDetalle As Short
    End Structure

    Public Structure FiltroOrdenRecepcion
        Dim NumeroOrden As String
        Dim IdOrdenRecepcion As Long
        Dim IdTipoProducto As Integer
        Dim IdTipoRecepcion As Integer
        Dim IdOrdenCompra As Long
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim Remision As String
        Dim IdCreador As Integer
        Dim IdEstado As Integer
        Dim IdFacturaGuia As Integer
        Dim IdProveedor As Integer
        Dim ListaEstado As ArrayList
        Dim ListaIdOrdenesRecepcion As ArrayList
        Dim ListaIdTipoRecepcion As ArrayList
        Dim ListaIdTipoProducto As ArrayList
        Dim Factura As String
        Dim Guia As String
        Dim IdConsignatario As Integer
        Dim idClienteExterno As Integer
        Dim idDistribuidor As Long
        Dim idTrasportadora As Integer
    End Structure

    Public Structure FiltroReporteRecepcion
        Dim IdOrdenRecepcion As Integer
        Dim IdOrdenCompra As Integer
        Dim NumeroOrdenCompra As String
        Dim IdTipoProducto As Integer
        Dim IdProducto As Integer
        Dim IdEstado As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim EstadoNotificacion As String
    End Structure

    Public Structure FiltroOrdenCombo
        Dim IdOrdenCombo As Long
        Dim IdMaterial1 As String
        Dim IdMaterial2 As String
        Dim IdLinea As Integer
        Dim IdUsuario As Integer
        Dim Cantidad As Integer
        Dim CantidadLeida As Integer
        Dim IdEstado As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim ListaEstado As ArrayList
    End Structure

    Public Structure FiltroPalletRecepcion
        Dim IdPallet As Integer
        Dim IdOrdenRecepcion As Integer
        Dim IdCreador As Integer
        Dim IdFacturaGuia As Long
        Dim IdTipoDetalleProducto As Short
        Dim IdEstado As Integer
    End Structure

    Public Structure FiltroBodega
        Dim IdBodega As Integer
        Dim Nombre As String
        Dim Codigo As String
        Dim IdCiudad As Integer
        Dim Activa As EstadoBinario
        Dim IdTipo As Integer
    End Structure

    Public Structure FiltroFacturaGuia
        Dim IdFacturaGuia As Long
        Dim IdFactura As Long
        Dim IdGuia As Long
        Dim Cantidad As Integer
        Dim Muestreo As Integer
        Dim idDetalleOrdenCompra As Integer
    End Structure

    Public Structure FiltroSerialMuestra
        Dim IdSerialMuestra As Long
        Dim IdOrden As Long
        Dim Serial As String
        Dim IdCreador As Long
        Dim FechaMuestra As Date
        Dim IdFactura As Long
        Dim IdGuia As Long
    End Structure

    Public Structure FiltroOrdenBodegaje
        Dim IdOrden As Integer
        Dim Codigo As String
        Dim IdProducto As Integer
        Dim Material As String
        Dim IdPosicion As Integer
        Dim IdInventario As Integer
        Dim IdEstado As Integer
        Dim SinAcomodar As EstadoBinario
        Dim IdOrigen As Short
        Dim FechaCreacionInicial As String
        Dim FechaCreacionFinal As String
    End Structure

    Public Structure FiltroNovedadILS
        Dim IdNovedad As Integer
        Dim IdTipoNovedad As Integer
        Dim Descripcion As String
        Dim Estado As Boolean
    End Structure

    Public Structure FiltroTipoNovedadILS
        Dim IdTipoNovedad As Integer
        Dim Descripcion As String
    End Structure

    Public Structure FiltroPalletNovedad
        Dim IdNovedad As Integer
        Dim IdPallet As Long
    End Structure

    Public Structure FiltroCajaEmpaque
        Dim IdCaja As Long
        Dim IdOrdenRecepcion As Long
        Dim IdDetallePallet As Long
        Dim IdPallet As Long
        Dim IdProducto As Integer
        Dim Material As String
        Dim IdRegion As Short
        Dim IdEstado As Short
        Dim IdTipoDetalleProducto As Short
    End Structure

    Public Structure FiltroEnvioLectura
        Dim IdOrdenEnvioLectura As Long
        Dim IdInstruccion As Long
        Dim IdEstado As Integer
        Dim IdCreador As Long
        Dim FechaCreacion As Date
        Dim IdUsuarioEnvio As Short
        Dim FechaEnvio As Date
        Dim Observaciones As String
        Dim IdOrdenTrabajo As Long
        Dim Idfactura As Long
        Dim IdGuia As Long
        Dim IdRegion As Integer
        Dim IdProducto As Integer
        Dim Material As String
        Dim ListaEstados As String
    End Structure

    Public Structure FiltroDetalleEnvioLectura
        Dim IdDetalleEnvioLectura As Long
        Dim IdOrdenEnvioLectura As Long
        Dim IdOrdenTrabajo As Long
    End Structure

    Public Structure FiltroDescargaEnvioLectura
        Dim IdHistorialDescargaEnvioLectura As Long
        Dim IdOrdenEnvioLectura As Long
        Dim IdEstado As Integer
        Dim IdUsuario As Integer
        Dim FechaDescarga As Date
    End Structure

    Public Structure FiltroEnvioPrueba
        Dim idOrdenEnvioPrueba As Integer
        Dim idFactura As Integer
        Dim idOrdenTrabajo As Integer
        Dim serial As String
    End Structure

    Public Structure FiltroEnvioPruebaSerial
        Dim idEvioPruebaSerial As Integer
        Dim idOrdenEnvioPrueba As Integer
        Dim idEstado As Short
        Dim serial As String

    End Structure

    Public Structure FiltroUsuarioNotificacion
        Dim IdUsuarioNotificacion As Integer
        Dim Nombres As String
        Dim Apellidos As String
        Dim Email As String
        Dim IdUsuarioCreacion As Integer
        Dim IdPerfil As Integer
        Dim IdAsuntoNotificacion As Integer
        Dim IdBodega As Integer
        Dim Separador As String
    End Structure

    Public Structure FiltroAsuntoNotificacion
        Dim IdAsuntoNotificacion As Integer
        Dim Nombre As String
        Dim Estado As Short
        Dim IdUsuarioCreacion As Integer
        Dim FechaCreacion As Date
        Dim IdPerfil As Integer
    End Structure

    Public Structure FiltroOrdenRecoleccion
        Dim IdOrden As Integer
        Dim IdOrigen As Integer
        Dim IdDestino As Integer
        Dim IdTransportadora As Integer
        Dim Guia As String
        Dim OrdenServicio As String
        Dim IdEstado As Integer
        Dim IdCreador As Integer
        Dim FechaIncio As Date
        Dim FechaFin As Date
        Dim IdConfirmadorRecoleccion As Integer
        Dim TipoFecha As fechaFiltro
        Public Enum fechaFiltro
            NoEstablecido = 0
            fechaCreacion = 1
            fechaRecoleccionTrans = 2
            fechaRecoleccionPunto = 3
        End Enum
        Dim idUsuarioPool As Integer
    End Structure

    Public Structure FiltroOrdenCuarentena
        Dim IdOrdenCuarentena As Long
        Dim IdCreador As Integer
        Dim Solicitante As String
        Dim Asunto As String
        Dim FechaInicial As String
        Dim FechaFinal As String
        Dim Activo As EstadoBinario
        Dim Observacion As String
    End Structure

    Public Structure FiltroCategoriaInventario
        Dim IdCategoriaInventario As Long
        Dim Nombre As String
        Dim PrecioMinimo As Decimal
        Dim PrecioMaximo As Decimal
        Dim Periodo As Integer
        Dim IdCreador As Integer
        Dim FechaCreacion As Date
        Dim Observaciones As String
        Dim filtrar As Boolean
    End Structure

    Public Structure FiltroDevolucion
        Dim idDevolucion As Integer
        Dim idOrdenRecoleccion As Integer
    End Structure

    Public Structure FiltroModificacionCategoria
        Dim IdModificacionCategoria As Integer
        Dim material As String
        Dim IdCategoriaInventario As Short
        Dim Activo As Boolean
        Dim IdTipoModificacion As Short
        Dim IdCreador As Integer
        Dim FechaCreacion As Date
        Dim Observaciones As String
    End Structure

    Public Structure FiltroCicloInventario
        Dim IdCicloInventario As Integer
        Dim IdEstado As Integer
        Dim Consecutivo As Short
        Dim FechaCreacion As Date
        Dim FechaInicio As Date
        Dim FechaEsperadaFin As Date
        Dim IdCategoriaInventario As Short
        Dim FechaCierre As Date
        Dim IdUsuarioCierre As Integer
    End Structure

    Public Structure FiltroOrdenInventario
        Dim IdOrdenInventario As Long
        Dim IdCicloInventario As Integer
        Dim IdEstado As Integer
        Dim FechaCreacion As Date
        Dim IdBodega As Integer
        Dim IdRegion As Integer
        Dim IdUsuarioConteo1 As Integer
        Dim IdUsuarioConteo2 As Integer
        Dim IdUsuarioConteo3 As Integer
        Dim IdUsuarioCierre As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim TipoFecha As Short
        Dim IdCategoriaInventario As Short
    End Structure

    Public Structure FiltroDetalleOrdenInventario
        Dim IdDetalleOrdenInventario As Long
        Dim IdOrdenInventario As Long
        Dim IdSubProducto As Integer
        Dim IdTipoStock As Short
        Dim IdUsuarioRegistra As Integer
        Dim FechaRegistro As Date
        Dim validarCierre As Boolean
    End Structure

    Public Structure FiltroDetalleSerializacionOrden
        Dim IdDetalleSerializacionOrden As Long
        Dim IdOrdenInventario As Long
        Dim IdDetalleOrdenInventario As Long
        Dim FechaLectura As Date
        Dim IdUsuarioLector As Integer
        Dim Serial As String
        Dim OTB As Long
    End Structure

    Public Structure FiltroInventarioSegunCliente
        Dim IdInventarioSegunCliente As Long
        Dim IdSubProducto As Integer
        Dim IdRegion As Integer
        Dim IdTipoStock As Short
        Dim IdBodega As Integer
        Dim Cantidad As Integer
        Dim IdUsuarioModificador As Integer
        Dim FechaUltimaModificacion As Date
    End Structure

    Public Structure FiltroGrupoDevolucion
        Dim IdGrupoDevolucion As Long
        Dim IdGrupo As Long
        Dim IdGrupoDevolucion2 As String
        Dim IdCreador As Long
        Dim Estado As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
    End Structure

    Public Structure FiltroCombinacionTipoProducto
        Dim IdTipoPrimario As Short
        Dim IdTipoSecundario As Short
        Dim IdCreador As Long
        Dim Observacion As String
        Dim FechaInicial As Date
        Dim FechaFinal As Date
    End Structure

    Public Structure InfoEnvioPrueba
        Dim Guia As String
        Dim factura As String
        Dim fechaRecepcion As String
        Dim Producto As String
        Dim CantidadSeriales As Integer
    End Structure

    Public Structure InfoBodegas
        Dim idBodega As Integer
        Dim centro As Integer
        Dim almacen As Integer
    End Structure

    Public Structure FiltroSolicitante
        Dim Idsolicitante As Integer
        Dim IdEstado As Short
    End Structure

    Public Structure FiltroDetalleCuarentena
        Dim IdDetalleSerial As Long
        Dim Serial As String
        Dim IdDetallePedido As Integer
        Dim IdPedido As Integer
        Dim sinOTB As EstadoBinario
        Dim liberado As EstadoBinario
        Dim IdDetallePedidoLiberacion As Integer
        Dim IdPedidoLiberacion As Integer
    End Structure

    Public Structure FiltroRegion
        Dim idRegion As Integer
        Dim codigo As String
        Dim centro As String
        Dim almacen As String
        Dim esRegion As Nullable(Of Boolean)
    End Structure

    Public Structure FiltroTipoDespacho
        Dim idTipoDespacho As Integer
        Dim estado As EstadoBinario
        Dim idEntidad As Integer
    End Structure

    Public Structure FiltroPreinstruccion
        Dim IdOrdenCompra As Integer
        Dim IdDetalleOrdenCompra As Integer
        Dim NumeroOrdenCompra As String
        Dim IdFactura As Integer
        Dim Factura As String
        Dim IdProducto As String
        Dim IdFabricante As Integer
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim IdEstado As Integer
        Dim MostrarPool As EstadoBinario
    End Structure

    Public Structure FiltroInfoTarjetaPrepago
        Dim IdRegistro As Integer
        Dim Serial As String
        Dim IdRegion As Integer
        Dim Centro As String
        Dim IdProducto As Integer
        Dim Material As String
        Dim Lote As String
        Dim FechaVencimientoInicial As DateTime
        Dim FechaVencimientoFinal As DateTime
        Dim FechaRegistroInicial As DateTime
        Dim FechaRegistroFinal As DateTime
        Dim IdOrdenRecepcion As Integer
        Dim FechaCargueInicial As DateTime
        Dim FechaCargueFinal As DateTime
        Dim Cargado As EstadoBinario
    End Structure

    Public Structure FiltroInfoCargueProductoSAP
        Dim IdInfoCargue As Integer
        Dim IdCargue As Integer
        Dim Serial As String
        Dim IdRegion As Integer
        Dim IdProducto As Integer
        Dim Material As String
        Dim Lote As String
        Dim FechaRegistroInicial As DateTime
        Dim FechaRegistroFinal As DateTime
        Dim IdOrdenRecepcion As Integer
        Dim FechaCargueInicial As DateTime
        Dim FechaCargueFinal As DateTime
        Dim Cargado As EstadoBinario
    End Structure

    Public Structure FiltroInfoCargueSAPToken
        Dim IdInfoCargue As Integer
        Dim IdCargue As Integer
        Dim Serial As String
        Dim Centro As String
        Dim IdRegion As Integer
        Dim Region As String
        Dim IdProducto As Integer
        Dim Material As String
        Dim FechaRegistroInicial As DateTime
        Dim FechaRegistroFinal As DateTime
        Dim IdOrdenRecepcion As Integer
        Dim FechaCargueInicial As DateTime
        Dim FechaCargueFinal As DateTime
        Dim Cargado As EstadoBinario
    End Structure

    Public Structure FiltroSerialesNCRecuperados
        Dim Serial As String
        Dim IdFabricante As Integer
        Dim IdProveedor As Integer
        Dim IdFactura As Integer
        Dim IdOrden As Long
        Dim TipoFecha As Short
        Dim FechaInicial As Date
        Dim FechaFinal As Date
    End Structure

    Public Structure FiltroEdadInventario
        Dim IdProducto As Integer
        Dim Material As String
        Dim EdadInventario As Integer
    End Structure

    Public Structure FiltroConfigValues
        Dim IdConfig As Integer
        Dim ConfigKeyName As String
    End Structure

    Public Structure FiltroPedidoCuarentena
        Dim IdPedido As Integer
        Dim Serial As String
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim IdCreador As Integer
        Dim IdSolicitante As Integer
    End Structure

    Public Structure FiltroTipoAlistamiento
        Dim idTipoAlistamiento As Integer
        Dim activo As EstadoBinario
        Dim codigo As String
    End Structure

    Public Structure FiltroLiberacionCuarentena
        Dim IdLogLiberacion As Long
        Dim Serial As String
        Dim IdPedido As Integer
        Dim IdDetallePedido As Integer
    End Structure

    Public Structure FiltroExistenciasMaterial
        Dim material As String
        Dim idRegion As Integer
        Dim idCliente As Integer
        Dim idBodega As Integer
        Dim idPedido As Integer
        Dim numeroPedido As Long
        Dim idTipoPedido As Integer
        Dim cantidadSolicitada As Integer
    End Structure

    Public Structure FiltroDetalleCargaPedidoSAP
        Dim IdDetalle As Long
        Dim Pedido As Long
        Dim IdOrden As Long
        Dim Posicion As Integer
        Dim Material As String
        Dim Centro As String
        Dim Cantidad As Integer
        Dim Entrega As Long
        Dim Contabilizado As Boolean
        Dim CambioMaterial As Boolean
    End Structure

    Public Structure FiltroClaseMovimientoStockSAP
        Dim IdClaseMovimiento As Long
        Dim Movimiento As Integer
        Dim StockOrigen As Integer
        Dim StockDestino As Integer
    End Structure

    Public Structure FiltroReporteFacturacionTransportador
        Dim idTransportador As Integer
        Dim idCanalDistribucion As Integer
        Dim fechaDespachoInicial As Date
        Dim fechaDespachoFinal As Date
    End Structure

    Public Structure FiltroDespacho
        Dim IdDespacho As Integer
        Dim IdPedido As Integer
        Dim NumeroEntrega As Long
        Dim NumeroPedido As Long
        Dim IdAuxiliarAtiende As Integer
        Dim FechaCreacionInicial As String
        Dim FechaCreacionFinal As String
        Dim FechaCierreInicial As String
        Dim FechaCierreFinal As String
        Dim IdEstado As Short
    End Structure
    Public Structure FiltroConsultaVisitasSimpliRoute
        Dim FechaInicial As Date
        Dim FechaFinal As Date
        Dim bodega As List(Of Object)
        Dim ciudad As Integer
        Dim tipoServicio As List(Of Object)
        Dim jornada As Integer

        Dim tbRadicados As DataTable

    End Structure
End Namespace

Namespace Enumerados

    Public Enum PerfilesDelivery
        Motorizado_Delivery = 113
        Transportadora_Delivery = 194
    End Enum
    Public Enum OrigenCausal
        MesaControl = 1
        DocumentoBanco = 2
    End Enum
    Public Enum EstadoBinario
        NoEstablecido = 0
        Activo = 1
        Inactivo = 2
    End Enum

    Public Enum SiNo
        NoEstablecido = -1
        Si = 1
        No = 0
    End Enum

    Public Enum Sistema
        BPColsys = 0
        SID = 1
    End Enum

    Public Enum AccionItem
        Ninguna = 0
        Adicionar = 1
        Actualizar = 2
        Eliminar = 3
    End Enum

    Public Enum TipoServicio
        Reposicion = 1
        Venta = 2
        CesionContrato = 3
        Migracion = 4
        ServicioTecnico = 5
        Portacion = 6
        OrdenCompra = 7
        Siembra = 8
        VentaWeb = 9
        ServiciosFinancieros = 10
        VentaEDUCLIC = 11
        VentaCorporativa = 12
        CampañaClaroFijo = 13
        TiendaVirtual = 14
        VentaCorporativaPrestamo = 15
        EquiposReparadosST = 16
        ServiciosFinancierosBancolombia = 17
        ServiciosFinancierosDavivienda = 18
        DaviviendaSamsung = 19
        Servicios_Financieros_Davivienda_AM_PM = 20
        MercadoNaturalFeria = 21
    End Enum

    Public Enum SistemaOrigen
        NotusIls = 1
        NotusExpress = 2
    End Enum

    Public Enum EstadoServicio As Integer
        EnCuarentena = 99
        Creado = 100
        Confirmado = 101
        Despachado = 102
        Entregado = 103
        Cerrado = 104
        DespachadoCambio = 106
        Legalizado = 107
        Transito = 111
        Devolucion = 112
        AsignadoRuta = 113
        RecibidoCliente = 119
        RecibidoST = 122
        RevisionServicioTecnico = 120
        ServicioTecnico = 121
        Preventa = 162
        Anulado = 163
        Radicado = 164
        DevueltoCallCenter = 165
        PendienteRecoleccion = 208
        PendienteCierre = 228
        Entregadoalegalizacion = 231
        AnuladoPorSistemaDevCallCenter = 235
        AnuladoPorSistemaPreventa = 236
        AnuladoDevCallCenter = 237
        AnuladoPreventa = 238
        GestionadoConNovedadEnContacto = 239
        SerialesAsignados = 240
        Facturado = 241
        Asignadoaresponsable = 244
        RecuperacionMesaControl = 249
        PendienteAprobacionCalidad = 271
        RechazadoCalidadContactCenter = 274
        VerificacionMesaControl = 275
        RadicadoBanco = 278
        CampaniaFinalizada = 279
        RechazadoBanco = 281
        RechazadoMesaControl = 283
        DestruccionDocumentosMC = 284

        Recogido = 294 '03_RECOGIDO
        Reagendado = 298 '06_REAGENDADO
        EntregaParcial = 299 '08_ENTREGA_PARCIAL
        NoCobertura = 300 '09_NO_COBERTURA

    End Enum

    Public Enum ProcesoMensajeria As Integer
        Registro = 1
        Confirmacion = 2
        Alistamiento = 3
        Despacho = 4
        Entrega = 5
        Legalización = 6
        Devolución = 7
        RecepciónServicioTécnico = 8
        GestiónServicioTécnico = 9
        RecoleccionEntregaSTecnico = 10
        Preventa = 11
        Enrutamiento = 12
        Entrega_Recolección_Siembra = 13
        Cierre = 14
        Reagenda = 15
        LiberacionServicio = 16
    End Enum

    Public Enum RutaMensajeria As Integer
        Creada = 108
        Reparto = 109
        Cerrado = 110
    End Enum

    Public Enum MovimientoSecuencia As Integer
        Subir = 1
        Bajar = 2
    End Enum

    Public Enum EstadoNovedadMensajeria As Integer
        Registrado = 105
        Solucionada = 114
    End Enum

    Public Enum Entidad As Integer
        InventarioBodega = 29
        InventarioBloqueo = 30
        ServicioMensajeria = 31
        NovedadMensajeria = 32
        RutaMensajeria = 33
        PedidoProducto = 43
        EstadoDevoluciónSiembra = 50
    End Enum

    Public Enum InventarioBloqueo As Integer
        Temporal = 96
        Confirmado = 97
        Anulado = 98
    End Enum

    Public Enum TipoBloqueo As Integer
        Cuarentena = 1
        Preactivado = 2
        ControldeCalidad = 3
        ReservadeInventario = 4
        Campaña = 5
    End Enum

    Public Enum UnidadNegocio As Integer
        Telefonia = 1
        Hardware = 2
        MensajeriaEspecializada = 3
        MensajeriaEDUCLIC = 5
    End Enum

    Public Enum FuncionalidadMensajeria As Integer
        ConsultaGeneralRutas = 2
        EdicionInformacionServicio = 4
        PoolGeneralServicios = 1
        PoolGeneralServiciosUrgentes = 3
        PoolRutasActivas = 5
    End Enum

    Public Enum EstadoInventario As Integer
        Bloqueado = 94
        LibreUtilizacion = 95
        PendienteReitegroBodega = 228
    End Enum

    Public Enum EstadoBloqueos As Integer
        Temporal = 96
        Confirmado = 97
        Anulado = 98
    End Enum

    Public Enum TipoRutaServicioMensajeria As Integer
        EntregaCliente = 1
        RecoleccionCliente = 2
        EntregaProveedorServicioTecnico = 3
        RecoleccionProveedorServicioTecnico = 4
        EntregaClienteServicioTecnico = 5
        RecoleccionClienteSiembra = 6
    End Enum

    Public Enum TipoNovedadMensajeria As Integer
        RecepcionIncompletaST = 48
    End Enum

    Public Enum EstadoSerialCEM As Short
        RecibidoCliente = 1
        ServicioTécnico = 2
        Reparado = 3
        EnRuta = 4
        RecibidoST = 5
        Entregado = 6
        SerialPrestamoLiberado = 7
        EntregadoACliente = 8
        RecibidoDeCliente = 9
        EnRutaRecolecciónSiembra = 10
        EnTransitoABodega = 11
        Pendiente_Reintegro_a_Bodega = 12
        Pendiente_por_ingresar_NC = 13
        Devueltos_a_Bodega = 14
    End Enum

    Public Enum EstadoDevolucionSerial
        Conforme = 206
        No_Conforme = 207
        Siniestro = 209
    End Enum

    Public Enum TipoValidacionBloqueo
        Producto = 1
        Serial = 2
    End Enum

    Public Enum TipoProductoMaterial
        HANDSETS = 1
        SIM_CARDS = 2
        TARJETAS_PREPAGO = 3
        INSUMOS = 4
        MERCHANDISING = 5
        MATERIA_POP_PUBLICIDAD = 6
        ACCESORIOS = 7
        BONOS = 8
        TOKEN = 9
        PAPELERIA = 10
        DUMMIES = 11
    End Enum

    Public Enum EstadoCajaEmpaque
        Temporal = 39
        Confirmada = 40
        Anulada = 41
    End Enum

    Public Enum EstadoPalletRecepcion
        En_Cesion = 56
        Recepcionado = 57
        Anulado = 58
        Inactivo = 59
    End Enum

    Public Enum EstadoPedidoProducto
        Creado = 169
        EnProcesoBodega = 170
        AtendidoBodega = 171
        EnProcesoDespacho = 172
        EnDespacho = 173
        Despachado = 174
        Anulado = 175
        BloqueoFaltante = 176
        SinDisponibilidadSAP = 224
    End Enum

    Public Enum TipoPicking
        PickingBodega = 1
        PickingDespacho = 2
    End Enum

    Public Enum EstadoPicking
        Creado = 12
        Anulado = 13
        Atendido = 14
    End Enum

    Public Enum EstadoDespacho
        Creado = 3
        En_Proceso = 4
        Cerrado = 5
        Leído_en_Entregas = 6
        Despachado = 33
        Anulado = 35
        Sincronizado = 180
        Pendiente_Asignacion_Guia = 185
    End Enum

    Public Enum TipoDespacho
        Serializado = 1
        Merchandising = 2
        Seriales_de_Prueba = 4
        Mixto = 5
    End Enum

    Public Enum SolucionNovedadClienteExterno
        Redireccionar = 1
        Ofrecer_Nuevamente = 2
        Corregir_Información = 3
    End Enum

    Public Enum InstruccionPOP
        Creada = 193
        Proceso = 194
        Cerrada = 195
        Anulada = 196
    End Enum

    Public Enum DetalleInstruccionPOP
        Creada = 197
        PedidosParciales = 198
        Cerrada = 199
        Anulada = 200
    End Enum

    Public Enum DetalleInstruccionPOPMaterial
        Creada = 201
        EnPedido = 202
        Anulada = 203
    End Enum

    Public Enum ResponsableGestionNovedad
        ClienteExterno = 1
        LogytechMobile = 2
    End Enum

    Public Enum TipoPersonaSiembra
        Gerente = 1
        Coordinador = 2
        Consultor = 3
    End Enum

    Public Enum TipoFechaSerialPapeleria
        fechaRegistro = 1
        fechaVersion = 2
    End Enum

    Public Enum TipoRecepcion
        IMPORTACION = 1
        NACIONAL = 2
        TRASLADO = 3
        DEVOLUCION = 4
    End Enum

    Public Enum TipoDestinatario
        CAD_LM = 1
        CAD_BRIGHTSTAR = 2
        CVC = 3
        CAC = 4
        ZF_CRISMA = 5
        CADENAS = 6
        ZONA_FRANCA = 7
        DISTRIBUIDORES = 8
        CACs_BRIGHTSTAR = 9
        CENTROS_VIRTUALES = 10
        SINIESTROS = 11
        CAV = 12
        COORD_REGIONAL = 13
    End Enum

    Public Enum ClienteExterno
        COMCEL = 1
        DAVIVIENDA = 3
        DAVIVIENDAEXTERNO = 7
        BANCOLOMBIA = 4
    End Enum

    Public Enum TipoPedidoProducto
        SalidadeVentas = 1
        SalidadeTraslado = 2
        SalidadeConsumos = 3
        EntregadeProductoaCuarentena = 4
        SalidadeProductoparaPruebas = 5
        DespachodeCuarentena = 6
        LiberaciondeCuarentena = 7
        SalidadePublicidad = 8
    End Enum

    Public Enum ProcesoJustificacionInventario
        Bloqueodeseriales = 1
        Desbloqueodeseriales = 2
        Descarguedeseriales = 3
    End Enum

    Public Enum TipoMovimientoSAP
        SalidaCentroCosto = 201
        CambioRegion = 301
        CambioMaterial = 309
        CambioStockControlCalidadLibreUtilizacion = 321
        CambioStockLibreUtilizacionControlCalidad = 322
    End Enum

    Public Enum PerfilesMensajeria
        Motorizado_Mensajeria_Especializada = 113
        Enrutador_Mensajeria_Especializada = 120
        SoloConsulta_Mensajeria_Especializada = 164
    End Enum

    Public Enum EstadoAlertaInventario
        Registrada = 233
        Finalizada = 234
    End Enum

    Public Enum TipoDocumento
        SoportesdePago = 1
        SoportesdePreciosEspeciales = 2
        Soportesdefactura = 3
    End Enum

    Public Enum TipoUnidadNegocio As Integer
        Interna = 1
        Externa = 2
    End Enum

    Public Enum ExtensionArchivo
        XLSX = 1
        XLS = 2
        CSV = 3
    End Enum
    Public Enum TipoBodega
        Principal = 1
        Satelite = 2
        SateliteSecundaria = 3
    End Enum

End Namespace