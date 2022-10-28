Imports LMDataAccessLayer
Namespace Fulfillment
    Public Class CargarDatosNoConformes

#Region "Atributos"

        Private _ordentrabajo As String
        Private _idsubproducto As String
        Private _subproducto As String
        Private _cajasPorHuacal As Integer
        Private _unidadesPorCaja As Integer
        Private _idTipoProducto As Integer
        Private _idTecnologia As Integer
        Private _estadoOrden As String
        Private _idTipoproceso As Integer
        Private _cantidadPedida As Integer
        Private _cantidadLeida As Integer
        Private _unidadesCaja As Integer
        Private _cajasEstiba As Integer
        Private _estiba As Integer
        Private _caja As Integer
        Private _totalCaja As Integer
        Private _totalEstiba As Integer
        Private _RequierePin As Boolean
        Private _ordenSecuencia As Integer
        Private _cantidadProducto As Integer
        Private _cantidadEstiba As Integer
        Private _idFactura As String
        Private _cntDevolucion As Integer
        Private _secuencia As Integer
        Private _cntSeriales As Integer
        Private _existePin As Boolean
        Private _REGEX_PIN As String
        Private _caracteresPermitidos As String
        Private _strLongitud As String
        Private _strLongMenor As Integer
        Private _strLongMayor As Integer
        Private _idProductoEnDev As Integer
        Private _idProducto As String
        Private _region As String

#End Region

#Region "Propiedades"

        Public Property ordenTrabajo() As String
            Get
                Return _ordentrabajo
            End Get
            Set(ByVal value As String)
                _ordentrabajo = value
            End Set
        End Property

        Public Property idSubproducto() As String
            Get
                Return _idsubproducto
            End Get
            Set(ByVal value As String)
                _idsubproducto = value
            End Set
        End Property

        Public Property subproducto() As String
            Get
                Return _subproducto
            End Get
            Set(ByVal value As String)
                _subproducto = value
            End Set
        End Property

        Public Property cajasporhuacal() As Integer
            Get
                Return _cajasPorHuacal
            End Get
            Set(ByVal value As Integer)
                _cajasPorHuacal = value
            End Set
        End Property

        Public Property idTipoProducto() As Integer
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Integer)
                _idTipoProducto = value
            End Set
        End Property

        Public Property idTecnologia() As Integer
            Get
                Return _idTecnologia
            End Get
            Set(ByVal value As Integer)
                _idTecnologia = value
            End Set
        End Property

        Public Property estadoOrden() As String
            Get
                Return _estadoOrden
            End Get
            Set(ByVal value As String)
                _estadoOrden = value
            End Set
        End Property

        Public Property idTipoproceso() As Integer
            Get
                Return _idTipoproceso
            End Get
            Set(ByVal value As Integer)
                _idTipoproceso = value
            End Set
        End Property

        Public Property cantidadPedida() As Integer
            Get
                Return _cantidadPedida
            End Get
            Set(ByVal value As Integer)
                _cantidadPedida = value
            End Set
        End Property

        Public Property cantidadLeida() As Integer
            Get
                Return _cantidadLeida
            End Get
            Set(ByVal value As Integer)
                _cantidadLeida = value
            End Set
        End Property

        Public Property unidadesPorCaja() As Integer
            Get
                Return _unidadesPorCaja
            End Get
            Set(ByVal value As Integer)
                _unidadesPorCaja = value
            End Set
        End Property

        Public Property cajasEstiba() As Integer
            Get
                Return _cajasEstiba
            End Get
            Set(ByVal value As Integer)
                _cajasEstiba = value
            End Set
        End Property

        Public Property estiba() As Long
            Get
                Return _estiba
            End Get
            Set(ByVal value As Long)
                _estiba = value
            End Set
        End Property

        Public Property caja() As Long
            Get
                Return _caja
            End Get
            Set(ByVal value As Long)
                _caja = value
            End Set
        End Property

        Public Property totalCaja() As Long
            Get
                Return _totalCaja
            End Get
            Set(ByVal value As Long)
                _totalCaja = value
            End Set
        End Property

        Public Property totalEstiba() As Long
            Get
                Return _totalEstiba
            End Get
            Set(ByVal value As Long)
                _totalEstiba = value
            End Set
        End Property

        Public Property requierePin() As Boolean
            Get
                Return _RequierePin
            End Get
            Set(ByVal value As Boolean)
                _RequierePin = value
            End Set
        End Property

        Public Property ordenSecuencia() As Integer
            Get
                Return _ordenSecuencia
            End Get
            Set(ByVal value As Integer)
                _ordenSecuencia = value
            End Set
        End Property

        Public Property cantidadProducto() As Integer
            Get
                Return _cantidadProducto
            End Get
            Set(ByVal value As Integer)
                _cantidadProducto = value
            End Set
        End Property

        Public Property cantidadEstiba() As Integer
            Get
                Return _cantidadEstiba
            End Get
            Set(ByVal value As Integer)
                _cantidadEstiba = value
            End Set
        End Property

        Public Property idFactura() As String
            Get
                Return _idFactura
            End Get
            Set(ByVal value As String)
                _idFactura = value
            End Set
        End Property

        Public Property cntDevolucion() As Integer
            Get
                Return _cntDevolucion
            End Get
            Set(ByVal value As Integer)
                _cntDevolucion = value
            End Set
        End Property

        Public Property secuencia() As Integer
            Get
                Return _secuencia
            End Get
            Set(ByVal value As Integer)
                _secuencia = value
            End Set
        End Property

        Public Property cntSeriales() As Integer
            Get
                Return _cntSeriales
            End Get
            Set(ByVal value As Integer)
                _cntSeriales = value
            End Set
        End Property

        Public Property existePin() As Boolean
            Get
                Return _existePin
            End Get
            Set(ByVal value As Boolean)
                _existePin = value
            End Set
        End Property

        Public Property REGEX_PIN() As String
            Get
                Return _REGEX_PIN
            End Get
            Set(ByVal value As String)
                _REGEX_PIN = value
            End Set
        End Property

        Public Property caracteresPermitidos() As String
            Get
                Return _caracteresPermitidos
            End Get
            Set(ByVal value As String)
                _caracteresPermitidos = value
            End Set
        End Property

        Public Property strLongitud() As String
            Get
                Return _strLongitud
            End Get
            Set(ByVal value As String)
                _strLongitud = value
            End Set
        End Property

        Public Property LongitudMenor() As Integer
            Get
                Return _strLongMenor
            End Get
            Set(ByVal value As Integer)
                _strLongMenor = value
            End Set
        End Property

        Public Property LongitudMayor() As Integer
            Get
                Return _strLongMayor
            End Get
            Set(ByVal value As Integer)
                _strLongMayor = value
            End Set
        End Property

        Public Property idProductoEnDev() As Integer
            Get
                Return _idProductoEnDev
            End Get
            Set(ByVal value As Integer)
                _idProductoEnDev = value
            End Set
        End Property

        Public Property idProducto() As String
            Get
                Return _idProducto
            End Get
            Set(ByVal value As String)
                _idProducto = value
            End Set
        End Property

        Public Property region() As String
            Get
                Return _region
            End Get
            Set(ByVal value As String)
                _region = value
            End Set
        End Property

#End Region

#Region "Metodos Públicos"
        Public Function CargarFacturasFulfillment(ByVal _idLinea As Integer) As DataTable

            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idLinea", SqlDbType.VarChar).Value = _idLinea
                    End With
                    dt = .ejecutarDataTable("ObtenerFacturaNoConformesFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Function ObtenerRegion() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                Return db.ejecutarDataTable("ObtenerRegiones")
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

        Public Function CargarNoConformides(Optional ByVal noConformidad As String = "")
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                If Not String.IsNullOrEmpty(noConformidad) Then db.SqlParametros.Add("@noConformidad", SqlDbType.VarChar, 20).Value = noConformidad
                Return db.ejecutarDataTable("ObtenerListaNoConformidad", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

        Public Function ObtenerOrdenes(ByVal idFactura As Integer, ByVal sRegion As Integer, ByVal idLinea As Integer, ByVal idOrden As String, ByVal idSubproducto As Integer, ByVal filtro As String) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.VarChar).Value = idFactura
                        .Add("@region", SqlDbType.VarChar).Value = sRegion
                        .Add("@idLinea", SqlDbType.VarChar).Value = idLinea
                    End With
                    dt = .ejecutarDataTable("ObtenerOrdenesNoConformesFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Sub ObtenerDatosOrden(ByVal idFactura As Integer, ByVal sRegion As Integer, ByVal idLinea As Integer, ByVal material As String, ByVal idOrden As String, ByVal filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    End With
                    .ejecutarReader("ObtenerOrdenTrabajoNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _ordentrabajo = .Reader("codigo").ToString
                        _idsubproducto = .Reader("material").ToString
                        _subproducto = .Reader("subproducto").ToString
                        Long.TryParse(.Reader("cantidadLeida").ToString, _cantidadLeida)
                        Long.TryParse(.Reader("cantidadPedida").ToString, _cantidadPedida)
                        Long.TryParse(.Reader("cajasPorHuacal").ToString, _cajasPorHuacal)
                        Long.TryParse(.Reader("unidadesCaja").ToString, _unidadesPorCaja)
                        Long.TryParse(.Reader("idTipoProducto").ToString, _idTipoProducto)
                        Long.TryParse(.Reader("idTecnologia").ToString, _idTecnologia)
                        _estadoOrden = .Reader("idestado").ToString
                        Long.TryParse(.Reader("idmodificador").ToString, _idTipoproceso)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub cargarCantidades(ByVal sIdOrden As String, ByVal sFiltro As String, ByVal sFiltroProceso As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@filtroProceso", SqlDbType.VarChar).Value = sFiltroProceso
                        .Add("@idOrden", SqlDbType.VarChar).Value = sIdOrden
                    End With
                    .ejecutarReader("ObtenerCantidadesOrdenNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cajasEstiba").ToString, _cajasEstiba)
                        Long.TryParse(.Reader("cantidadPedida").ToString, _cantidadPedida)
                        Long.TryParse(.Reader("cantidadLeida").ToString, _cantidadLeida)
                        Boolean.TryParse(.Reader("requierePin").ToString, _RequierePin)
                    End If
                    .Reader.Close()
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub cargarCajaEstiba(ByVal sIdOrden As String, ByVal sidfactura As String, ByVal sFiltro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.VarChar).Value = sidfactura
                        .Add("@idOrden", SqlDbType.VarChar).Value = sIdOrden
                    End With
                    .ejecutarReader("ObtenerEstibaCajaOrdenNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("estiba").ToString, _estiba)
                        Long.TryParse(.Reader("ordensecuencia").ToString, _ordenSecuencia)
                    Else
                        _estiba = 1
                        _ordenSecuencia = 1
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub cantidadProductoSecuencia(ByVal sidfactura As String, ByVal sRegion As String, ByVal _estiba As String, ByVal _caja As String, ByVal sFiltro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.VarChar).Value = sidfactura
                        .Add("@region", SqlDbType.VarChar).Value = sRegion
                        .Add("@estiba", SqlDbType.VarChar).Value = _estiba
                        .Add("@caja", SqlDbType.VarChar).Value = _caja
                    End With
                    .ejecutarReader("ObtenerCantidadProductoSecuenciaOrdenNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cantidad").ToString, _cantidadProducto)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub cantidadProductoEstiba(ByVal sidfactura As String, ByVal sRegion As String, ByVal _estiba As String, ByVal sFiltro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.VarChar).Value = sidfactura
                        .Add("@region", SqlDbType.VarChar).Value = sRegion
                        .Add("@estiba", SqlDbType.VarChar).Value = _estiba
                    End With
                    .ejecutarReader("ObtenerCantidadProductoEstibaOrdenNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cantidadEstiba").ToString, _cantidadEstiba)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionFacturaNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                    End With
                    .ejecutarReader("validacionFacturaNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _idFactura = .Reader("idFactura").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionVirgenesNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                    End With
                    .ejecutarReader("validacionVirgenesNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cntDevolucion").ToString, _cntDevolucion)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionDevolucionesNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                    End With
                    .ejecutarReader("validacionDevolucionesNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cntDevolucion").ToString, _cntDevolucion)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionProduccionNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                    End With
                    .ejecutarReader("validacionProduccionNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cntDevolucion").ToString, _cntDevolucion)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionCantidadNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = _idorden
                        .Add("@caja", SqlDbType.VarChar).Value = _caja
                    End With
                    .ejecutarReader("validacionCantidadNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("secuencia").ToString, _secuencia)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionExisteSerialNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                        .Add("@idOrden", SqlDbType.VarChar).Value = _idorden
                    End With
                    .ejecutarReader("validacionExisteSerialNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cntSeriales").ToString, _cntSeriales)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionCantidadRegionProductoNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@region", SqlDbType.VarChar).Value = _region
                        .Add("@idFactura", SqlDbType.VarChar).Value = _factura
                        .Add("@estiba", SqlDbType.VarChar).Value = _estiba
                    End With
                    .ejecutarReader("validacionCantidadRegionProductoNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cntSeriales").ToString, _cntSeriales)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionCantidadOrdenFacturaNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = _idorden
                        .Add("@idFactura", SqlDbType.VarChar).Value = _factura
                    End With
                    .ejecutarReader("validacionCantidadOrdenFacturaNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cntSeriales").ToString, _cntSeriales)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionDevolucionesDetalleNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                    End With
                    .ejecutarReader("validacionDevolucionesDetalleNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("_idProductoEnDev").ToString, _idProductoEnDev)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ValidacionProduccionSerialNoConformes(ByVal _serial As String, ByVal _idorden As String, ByVal _caja As String, ByVal _region As String, ByVal _factura As String, ByVal _estiba As String, ByVal _filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                    End With
                    .ejecutarReader("validacionProduccionSerialNoConformesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _idsubproducto = .Reader("idSubproducto").ToString
                        _ordentrabajo = .Reader("idOrden").ToString
                        _region = .Reader("region").ToString
                        _idProducto = .Reader("idproducto").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Function CargaValidacionVirgenes(ByVal idTipoProducto As Integer, ByVal idTecnologia As Integer, ByVal filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idTipoProducto", SqlDbType.BigInt).Value = idTipoProducto
                        .Add("@idTecnologia", SqlDbType.BigInt).Value = idTecnologia
                    End With
                    .ejecutarReader("ObtenerInfoConfiguracionVirgenesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _caracteresPermitidos = .Reader("caracterPermitido").ToString
                        Long.TryParse(.Reader("longitudPermitida").ToString, _strLongitud)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

        Public Function CargaValidacionDevoluciones(ByVal idTipoProducto As Integer, ByVal idTecnologia As Integer, ByVal filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idTecnologia", SqlDbType.BigInt).Value = idTecnologia
                    End With
                    .ejecutarReader("ObtenerInfoConfiguracionDevolucionesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _caracteresPermitidos = .Reader("car_permitidos").ToString
                        Long.TryParse(.Reader("long_menor").ToString, _strLongMenor)
                        Long.TryParse(.Reader("long_mayor").ToString, _strLongMayor)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

        Public Sub ValidaPin(ByVal _pin As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@pin", SqlDbType.VarChar).Value = _pin
                    End With
                    .ejecutarReader("ValidarExistePin", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Boolean.TryParse(.Reader("existePin").ToString, _existePin)
                        _REGEX_PIN = .Reader("REGEX_PIN").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Function validaDevolucion(ByVal pSerial As String) As Integer
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim resul As Integer
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = pSerial
                    End With
                    resul = .ejecutarScalar("esSerialVirgenRecibidoEnDevolucion", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return resul
        End Function

        Public Sub RegistrarTelefonoNoConforme(ByVal _serial As String, ByVal _idProducto As String, ByVal _idFactura As String, ByVal _region As String, _
                                               ByVal _caja As String, ByVal _estiba As String, ByVal _facturaSecuencia As String, ByVal _idSubproducto As String, _
                                               ByVal _idOrden As String, ByVal _ordenSecuencia As String, ByVal _idLinea As String, ByVal _pin As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                        .Add("@idProducto", SqlDbType.VarChar).Value = _idProducto
                        .Add("@idFactura", SqlDbType.VarChar).Value = _idFactura
                        .Add("@region", SqlDbType.VarChar).Value = _region
                        .Add("@caja", SqlDbType.VarChar).Value = _caja
                        .Add("@estiba", SqlDbType.VarChar).Value = _estiba
                        .Add("@facturaSecuencia", SqlDbType.VarChar).Value = _facturaSecuencia
                        .Add("@idSubproducto", SqlDbType.VarChar).Value = _idSubproducto
                        .Add("@idOrden", SqlDbType.VarChar).Value = _idOrden
                        .Add("@ordenSecuencia", SqlDbType.VarChar).Value = _ordenSecuencia
                        .Add("@idLinea", SqlDbType.VarChar).Value = _idLinea
                        .Add("@pin", SqlDbType.VarChar).Value = _pin
                    End With
                    .ejecutarNonQuery("RegistrarTelefonosNoConformeFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub RegistrarTelefonoNoConformeDevolucion(ByVal _serial As String, ByVal _idProducto As String, ByVal idFactura As String, ByVal _region As String, _
                                       ByVal _caja As String, ByVal _estiba As String, ByVal _facturaSecuencia As String, ByVal _idSubproducto As String, _
                                       ByVal _idOrden As String, ByVal _ordenSecuencia As String, ByVal _fechaEtiquetado As String, _
                                       ByVal _idLinea As String, ByVal _pin As String, ByVal pOrigenBP As Boolean, ByVal pOrigenDevolucion As Boolean)

            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                        .Add("@idProducto", SqlDbType.VarChar).Value = _idProducto
                        .Add("@idFactura", SqlDbType.VarChar).Value = _idFactura
                        .Add("@region", SqlDbType.VarChar).Value = _region
                        .Add("@caja", SqlDbType.VarChar).Value = _caja
                        .Add("@estiba", SqlDbType.VarChar).Value = _estiba
                        .Add("@facturaSecuencia", SqlDbType.VarChar).Value = _facturaSecuencia
                        .Add("@idSubproducto", SqlDbType.VarChar).Value = _idSubproducto
                        .Add("@idOrden", SqlDbType.VarChar).Value = _idOrden
                        .Add("@ordenSecuencia", SqlDbType.VarChar).Value = _ordenSecuencia
                        .Add("@fechaEtiquetado", SqlDbType.VarChar).Value = _fechaEtiquetado
                        .Add("@idLinea", SqlDbType.VarChar).Value = _idLinea
                        .Add("@pin", SqlDbType.VarChar).Value = _pin
                        .Add("@filtroBP", SqlDbType.Bit).Value = pOrigenBP
                        .Add("@filtroDEV", SqlDbType.Bit).Value = pOrigenDevolucion
                    End With
                    .ejecutarNonQuery("RegistrarTelefonosNoConformeDevolucionFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub RegistrarHistorialNoConformes(ByVal pSerial As String, ByVal pIdusuario As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Add("@serial", SqlDbType.VarChar).Value = pSerial
                        .Add("@idUsuario", SqlDbType.VarChar).Value = pIdusuario
                    End With
                    .ejecutarNonQuery("RegistrarHistorialNoConformidadesFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub RegistrarNoConformidades(ByVal serial As String, ByVal arreglo As Integer, ByVal idOrden As String, ByVal idtercero As String, ByVal idtipoProceso As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Add("@serial", SqlDbType.VarChar).Value = serial
                        .Add("@idNoConforme", SqlDbType.Int).Value = arreglo
                        .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                        .Add("@idUsuario", SqlDbType.VarChar).Value = idtercero
                    End With
                    .ejecutarNonQuery("RegistrarNoConformidadesSerialFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ActualizarOrden(ByVal _idOrden As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = _idOrden
                    End With
                    .ejecutarNonQuery("ActualizaOrdenTrabajoFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub RegistrarSerialenProductoSerial(ByVal idfactura As Integer, ByVal idLinea As Integer, ByVal idOrden As String, ByVal origen As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.BigInt).Value = idfactura
                        .Add("@idLinea", SqlDbType.BigInt).Value = idLinea
                        .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    End With
                    .ejecutarNonQuery("InsertarDatosNoConformesFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

#End Region
    End Class
End Namespace
