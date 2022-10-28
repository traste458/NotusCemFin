Public Class FulfillmentBase
    Implements IFulfillment

#Region "Atributos (Campos)"

    Protected _requiereComprobarPallet As Boolean
    Protected _requierePin As Boolean
    Protected _requiereSim As Boolean
    Protected _ImprimeStickersCaja As Boolean
    Protected _imei As String
    Protected _iccid As String
    Protected _idProducto As Integer
    Protected _idSubproducto As Integer
    Protected _idfactura As Integer
    Protected _msisdn As String
    Protected _fechaRegistro As DateTime
    Protected _idOrden As Integer
    Protected _linea As Integer
    Protected _region As String
    Protected _estiba As Integer
    Protected _caja As Integer
    Protected _facturaSecuencia As Integer
    Protected _ordenSecuencia As Integer
    Protected _fechaEtiquetado As DateTime
    Protected _idEstado As Integer
    Protected _noConforme As Boolean
    Protected _idBodega As Integer
    Protected _idOrdenAnterior As Integer
    Protected _fulfillmentPropio As Boolean
    Protected _pin As String
    Protected _idOrdenRecepcion As Integer
    Protected _fechaRecepcion As DateTime
    Protected _idOrdenTermosellado As Integer
    Protected _fechaTermosellado As DateTime
    Protected _cargado As Boolean
    Protected _fechaCargue As DateTime
    Protected _pedidoCargue As String
    Protected _entregaCargue As String
    Protected _contabilizacionCargue As String
    Protected _nacionalizado As Boolean
    Protected _enInventario As Boolean
    Protected _totalCaja As Integer
    Protected _totalEstiba As Integer
    Protected _caracteresPermitidos As String
    Protected _strLongitud As String
    Protected _fechaProduccion As DateTime
    Protected _enOrdenAbierta As String
    Protected _idProductoDevolucion As Integer
    Protected _ingresaPorDevolucion As Boolean
    Protected _idUsuario As Integer
    Protected _serial As String
    Protected _idRegion As Integer
    Protected _registrado As Boolean
    Protected _niu As String
    Protected _materialSim As String
    Protected _regionSim As String
    Protected _noConformidad As ArrayList

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Public Property RequiereComprobarPallet() As Boolean Implements IFulfillment.RequiereComprobarPallet
        Get
            Return _requiereComprobarPallet
        End Get
        Set(ByVal value As Boolean)
            _requiereComprobarPallet = value
        End Set
    End Property

    Public Property RequiereSim() As Boolean Implements IFulfillment.RequiereSim
        Get
            Return _requiereSim
        End Get
        Set(ByVal value As Boolean)
            _requiereSim = value
        End Set
    End Property

    Public Property RequierePin() As Boolean Implements IFulfillment.RequierePin
        Get
            Return _requierePin
        End Get
        Set(ByVal value As Boolean)
            _requierePin = value
        End Set
    End Property

    Public Property ImprimeStickersCaja() As Boolean Implements IFulfillment.ImprimeStickersCaja
        Get
            Return _ImprimeStickersCaja
        End Get
        Set(ByVal value As Boolean)
            _ImprimeStickersCaja = value
        End Set
    End Property

    Public Property Imei As String
        Get
            Return _imei
        End Get
        Set(value As String)
            _imei = value
        End Set
    End Property

    Public Property Iccid As String
        Get
            Return _iccid
        End Get
        Set(value As String)
            _iccid = value
        End Set
    End Property

    Public Property IdProducto As Integer
        Get
            Return _idProducto
        End Get
        Set(value As Integer)
            _idProducto = value
        End Set
    End Property

    Public Property idSubProducto As Integer
        Get
            Return _idSubproducto
        End Get
        Set(value As Integer)
            _idSubproducto = value
        End Set
    End Property

    Public Property IdFactura As Integer
        Get
            Return _idfactura
        End Get
        Set(value As Integer)
            _idfactura = value
        End Set
    End Property

    Public Property Msisdn As String
        Get
            Return _msisdn
        End Get
        Set(value As String)
            _msisdn = value
        End Set
    End Property

    Public Property FechaRegistro As DateTime
        Get
            Return _fechaRegistro
        End Get
        Set(value As DateTime)
            _fechaRegistro = value
        End Set
    End Property

    Public Property IdOrden As Integer
        Get
            Return _idOrden
        End Get
        Set(value As Integer)
            _idOrden = value
        End Set
    End Property

    Public Property Linea As Integer
        Get
            Return _linea
        End Get
        Set(value As Integer)
            _linea = value
        End Set
    End Property

    Public Property Region As String
        Get
            Return _region
        End Get
        Set(value As String)
            _region = value
        End Set
    End Property

    Public Property Estiba As Integer
        Get
            Return _estiba
        End Get
        Set(value As Integer)
            _estiba = value
        End Set
    End Property

    Public Property Caja As Integer
        Get
            Return _caja
        End Get
        Set(value As Integer)
            _caja = value
        End Set
    End Property

    Public Property FacturaSecuencia As Integer
        Get
            Return _facturaSecuencia
        End Get
        Set(value As Integer)
            _facturaSecuencia = value
        End Set
    End Property

    Public Property OrdenSecuencia As Integer
        Get
            Return _ordenSecuencia
        End Get
        Set(value As Integer)
            _ordenSecuencia = value
        End Set
    End Property

    Public Property FechaEtiquetado As String
        Get
            Return _fechaEtiquetado
        End Get
        Set(value As String)
            _fechaEtiquetado = value
        End Set
    End Property

    Public Property IdEstado As Integer
        Get
            Return _idEstado
        End Get
        Set(value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property NoConforme As Boolean
        Get
            Return _noConforme
        End Get
        Set(value As Boolean)
            _noConforme = value
        End Set
    End Property

    Public Property IdBodega As Integer
        Get
            Return _idBodega
        End Get
        Set(value As Integer)
            _idBodega = value
        End Set
    End Property

    Public Property IdOrdenAnterior As Integer
        Get
            Return _idOrdenAnterior
        End Get
        Set(value As Integer)
            _idOrdenAnterior = value
        End Set
    End Property

    Public Property FulfillmentPropio As Boolean
        Get
            Return _fulfillmentPropio
        End Get
        Set(value As Boolean)
            _fulfillmentPropio = value
        End Set
    End Property

    Public Property Pin As String
        Get
            Return _pin
        End Get
        Set(value As String)
            _pin = value
        End Set
    End Property

    Public Property IdOrdenRecepcion As Integer
        Get
            Return _idOrdenRecepcion
        End Get
        Set(value As Integer)
            _idOrdenRecepcion = value
        End Set
    End Property

    Public Property FechaRecepcion As DateTime
        Get
            Return _fechaRecepcion
        End Get
        Set(value As DateTime)
            _fechaRecepcion = value
        End Set
    End Property

    Public Property IdOrdenTermosellado As Integer
        Get
            Return _idOrdenTermosellado
        End Get
        Set(value As Integer)
            _idOrdenTermosellado = value
        End Set
    End Property

    Public Property FechaTermosellado As DateTime
        Get
            Return _fechaTermosellado
        End Get
        Set(value As DateTime)
            _fechaTermosellado = value
        End Set
    End Property

    Public Property Cargado As Boolean
        Get
            Return _cargado
        End Get
        Set(value As Boolean)
            _cargado = value
        End Set
    End Property

    Public Property FechaCargue As DateTime
        Get
            Return _fechaCargue
        End Get
        Set(value As DateTime)
            _fechaCargue = value
        End Set
    End Property

    Public Property PedidoCargue As String
        Get
            Return _pedidoCargue
        End Get
        Set(value As String)
            _pedidoCargue = value
        End Set
    End Property

    Public Property EntregaCargue As String
        Get
            Return _entregaCargue
        End Get
        Set(value As String)
            _entregaCargue = value
        End Set
    End Property

    Public Property ContabilizacionCargue As String
        Get
            Return _contabilizacionCargue
        End Get
        Set(value As String)
            _contabilizacionCargue = value
        End Set
    End Property

    Public Property Nacionalizado As Boolean
        Get
            Return _nacionalizado
        End Get
        Set(value As Boolean)
            _nacionalizado = value
        End Set
    End Property

    Public Property EnInventario As Boolean
        Get
            Return _enInventario
        End Get
        Set(value As Boolean)
            _enInventario = value
        End Set
    End Property

    Public Property TotalCaja As Integer
        Get
            Return _totalCaja
        End Get
        Set(value As Integer)
            _totalCaja = value
        End Set
    End Property

    Public Property TotalEstiba As Integer
        Get
            Return _totalCaja
        End Get
        Set(value As Integer)
            _totalCaja = value
        End Set
    End Property

    Public Property CaracteresPermitidos As String
        Get
            Return _caracteresPermitidos
        End Get
        Set(value As String)
            _caracteresPermitidos = value
        End Set
    End Property

    Public Property StrLongitud As String
        Get
            Return _strLongitud
        End Get
        Set(value As String)
            _strLongitud = value
        End Set
    End Property

    Public Property FechaProduccion As DateTime
        Get
            Return _fechaProduccion
        End Get
        Set(value As DateTime)
            _fechaProduccion = value
        End Set
    End Property

    Public Property EnOrdenAbierta As String
        Get
            Return _enOrdenAbierta
        End Get
        Set(value As String)
            _enOrdenAbierta = value
        End Set
    End Property

    Public Property IdProductoDevolucion As Integer
        Get
            Return _idProductoDevolucion
        End Get
        Set(value As Integer)
            _idProductoDevolucion = value
        End Set
    End Property

    Public Property IngresaPorDevolucion As Boolean
        Get
            Return _ingresaPorDevolucion
        End Get
        Set(value As Boolean)
            _ingresaPorDevolucion = value
        End Set
    End Property

    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property Serial As String
        Get
            Return _serial
        End Get
        Set(value As String)
            _serial = value
        End Set
    End Property

    Public Property IdRegion As Integer
        Get
            Return _idRegion
        End Get
        Set(value As Integer)
            _idRegion = value
        End Set
    End Property

    Public Property Niu As String
        Get
            Return _niu
        End Get
        Set(value As String)
            _niu = value
        End Set
    End Property

    Public Property Registrado As Boolean
        Get
            Return _registrado
        End Get
        Set(value As Boolean)
            _registrado = value
        End Set
    End Property

    Public Property MaterialSim As String
        Get
            Return _materialSim
        End Get
        Set(value As String)
            _materialSim = value
        End Set
    End Property

    Public Property RegionSim As String
        Get
            Return _regionSim
        End Get
        Set(value As String)
            _regionSim = value
        End Set
    End Property

    Public Property NoConformidad As ArrayList
        Get
            If _noConformidad Is Nothing Then _noConformidad = New ArrayList
            Return _noConformidad
        End Get
        Set(value As ArrayList)
            _noConformidad = value
        End Set
    End Property


#End Region

#Region "Métodos Privados"



#End Region

#Region "Métodos Públicos"

    Public Overridable Function RegistrarSeriales() As ResultadoProceso

    End Function

    Public Overridable Function ValidarPallet() As Boolean Implements IFulfillment.RequierePallet
        _requiereComprobarPallet = False
    End Function

    Public Function CargarCajaEstiba(ByVal sIdOrden As String, Optional ByVal flag As Integer = 0) As String
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idOrden", SqlDbType.VarChar).Value = sIdOrden
                    If flag > 0 Then .Add("@flag", SqlDbType.Int).Value = flag
                End With
                .ejecutarReader("ObtenerEstibaCajaOrdenFulfillment", CommandType.StoredProcedure)
                If .Reader.Read Then
                    Long.TryParse(.Reader("estiba").ToString, _estiba)
                    Long.TryParse(.Reader("caja").ToString, _caja)
                    Long.TryParse(.Reader("totalcaja").ToString, _totalCaja)
                    Long.TryParse(.Reader("totalestiba").ToString, _totalEstiba)
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Function

    Public Function CargaValidacion(ByVal idTipoProducto As Integer, ByVal idTecnologia As Integer)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idTipoProducto", SqlDbType.BigInt).Value = idTipoProducto
                    .Add("@idTecnologia", SqlDbType.BigInt).Value = idTecnologia
                End With
                .ejecutarReader("ObtenerInfoConfiguracionLecturaSerial", CommandType.StoredProcedure)
                If .Reader.Read Then
                    _caracteresPermitidos = .Reader("caracterPermitido").ToString
                    Long.TryParse(.Reader("longitudPermitida").ToString, _strLongitud)
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Function

    Public Sub ObtenerInfoProduccionSerial(ByVal _serial As String, ByVal _filtro As String, Optional ByVal _idOrden As Integer = 0, Optional ByVal _idcaja As Integer = 0)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar).Value = _serial
                End With
                .ejecutarReader("ObtenerInfoProduccionSerialFulfillment", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        Integer.TryParse(.Reader("idSubproducto"), _idSubproducto)
                        Integer.TryParse(.Reader("idOrden"), _idOrden)
                        If Not IsDBNull(.Reader("region")) Then _region = .Reader("region").ToString
                        Integer.TryParse(.Reader("idProducto"), _idProducto)
                        Boolean.TryParse(.Reader("no_Conformidad"), _noConforme)
                        If Not IsDBNull(.Reader("fechaProduccion")) Then _fechaProduccion = CDate(.Reader("fechaProduccion"))
                        If Not IsDBNull(.Reader("enOrdenAbierta")) Then _enOrdenAbierta = .Reader("enOrdenAbierta").ToString

                        _registrado = True
                    Else
                        _registrado = False
                    End If
                    .Reader.Close()
                End If

            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Function SerialesVirgen(ByVal _serial As String) As String
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim result As Boolean
        Try
            With db
                With .SqlParametros
                    .Add("@serial", SqlDbType.VarChar).Value = _serial
                End With
                result = .ejecutarScalar("esSerialVirgenRecibidoEnDevolucion", CommandType.StoredProcedure)
            End With
            Return result
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
        Return result
    End Function

    Public Sub ObtenerInfoCantidadSerial(ByVal _serial As String, ByVal _filtro As String, ByVal _idOrden As String, ByVal _idcaja As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar).Value = _serial
                    .Add("@idOrden", SqlDbType.VarChar).Value = _idOrden
                    .Add("@idCaja", SqlDbType.VarChar).Value = _idcaja
                End With
                .ejecutarReader("ObtenerInfoCantidadSerialFulfillment", CommandType.StoredProcedure)
                If .Reader.Read Then
                    Long.TryParse(.Reader("cantidad").ToString, _facturaSecuencia)
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Sub VerificarTermosellado(ByVal _serial As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar).Value = _serial
                End With
                .ejecutarNonQuery("BorrarTermoSelladoSerialFulfillment", CommandType.StoredProcedure)
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Sub InfoSerialDevolucion(ByVal serial As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar).Value = serial
                End With
                .ejecutarReader("ObtenerInfoSerialDevolucionFulfillment", CommandType.StoredProcedure)
                If .Reader.Read Then
                    Long.TryParse(.Reader("idProducto").ToString, _idProductoDevolucion)
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Function ConfirmarDatosReproceso(ByVal serial As String, ByVal idfactura As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar).Value = serial
                    .Add("@idfactura", SqlDbType.VarChar).Value = idfactura
                End With
                .ejecutarReader("ObtenerDatosReprocesosFulfillment", CommandType.StoredProcedure)
                If .Reader.Read Then
                    _estiba = .Reader("estiba").ToString
                    Long.TryParse(.Reader("idregion").ToString, _idRegion)
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Function

    Public Sub ObtenerNiu(ByVal _serial As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar).Value = _serial
                End With

                .ejecutarReader("ObtenerNiuFulfillment", CommandType.StoredProcedure)

                If .Reader.Read Then
                    _niu = .Reader("niu").ToString
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Sub ValidaSim(ByVal sim As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@sim", SqlDbType.VarChar).Value = sim
                End With
                .ejecutarReader("DatosSimsFulfillment", CommandType.StoredProcedure)
                If .Reader.Read Then
                    Long.TryParse(.Reader("idestado").ToString, _idEstado)
                    _materialSim = .Reader("material").ToString
                    _regionSim = .Reader("region").ToString
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Overridable Function EsSerialValido() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        'resultado.EstablecerMensajeYValor(0, "")
        Return resultado
    End Function

#End Region

End Class

