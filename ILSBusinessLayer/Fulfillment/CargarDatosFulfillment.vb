Imports LMDataAccessLayer
Namespace Fulfillment
    Public Class CargarDatosFulfillment

#Region "Atributos (Campos)"
        Private _dtFacturas As New DataTable
        Private _dtRegiones As New DataTable
        Private _idProceso As String
        Private _PorcentajeMuestreo As Integer
        Private _CantidadMuestra As Integer
        Private _existenOTL As Boolean = False
        Private _caracteresPermitidos As String
        Private _strLongitud As Integer
        Private _idEstadoFactura As Integer
        Private _estadofacturas As String
        Private _estadoOrden As String
        Private _materialSim As String
        Private _estadoSim As Integer
        Private _regionSim As String
        Private _idSubproductoSerial As String
        Private _idordenSerial As String
        Private _regionSerial As String
        Private _idProductoserial As String
        Private _noConformidadSerial As String
        Private _fechaProduccionSerial As String
        Private _enOrdenAbiertaSerial As String
        Private _idOrdenAnteriorSerial As String
        Private _idFacturaSerial As String
        Private _tipoOrdenSerial As String
        Private _existePIN As Boolean
        Private _regexPIN As String
        Private _Niu As String
        Private _estibaReproceso As String
        Private _idRegionReproceso As Integer
        Private _idProductoDevolucion As Integer
        Private _facturaSecuencia As Integer = 0
        Private _nombreRegion As String
        Private _centroRegion As String
        Private _idEstadoOrden As Integer

#End Region

#Region "Propiedades"

        Public Property PorcentajeMuestreo() As Integer
            Get
                Return _PorcentajeMuestreo
            End Get
            Set(ByVal value As Integer)
                _PorcentajeMuestreo = value
            End Set
        End Property

        Public Property CantidadMuestra() As Integer
            Get
                Return _CantidadMuestra
            End Get
            Set(ByVal value As Integer)
                _CantidadMuestra = value
            End Set
        End Property

        Public Property ExisteOTL() As Boolean
            Get
                Return _existenOTL
            End Get
            Set(ByVal value As Boolean)
                _existenOTL = value
            End Set
        End Property

        Public Property CaracteresPermitidos() As String
            Get
                Return _caracteresPermitidos
            End Get
            Set(ByVal value As String)
                _caracteresPermitidos = value
            End Set
        End Property

        Public Property strLongitud() As Integer
            Get
                Return _strLongitud
            End Get
            Set(ByVal value As Integer)
                _strLongitud = value
            End Set
        End Property

        Public Property idestadofactura() As Integer
            Get
                Return _idEstadoFactura
            End Get
            Set(ByVal value As Integer)
                _idEstadoFactura = value
            End Set
        End Property

        Public Property estadofactura() As String
            Get
                Return _estadofacturas
            End Get
            Set(ByVal value As String)
                _estadofacturas = value
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

        Public Property materialSim() As String
            Get
                Return _materialSim
            End Get
            Set(ByVal value As String)
                _materialSim = value
            End Set
        End Property

        Public Property estadoSim() As Integer
            Get
                Return _estadoSim
            End Get
            Set(ByVal value As Integer)
                _estadoSim = value
            End Set
        End Property

        Public Property regionSim() As String
            Get
                Return _regionSim
            End Get
            Set(ByVal value As String)
                _regionSim = value
            End Set
        End Property

        Public Property idSubproductoSerial() As String
            Get
                Return _idSubproductoSerial
            End Get
            Set(ByVal value As String)
                _idSubproductoSerial = value
            End Set
        End Property

        Public Property idOrdenSerial() As String
            Get
                Return _idordenSerial
            End Get
            Set(ByVal value As String)
                _idordenSerial = value
            End Set
        End Property

        Public Property idProductoSerial() As String
            Get
                Return _idProductoserial
            End Get
            Set(ByVal value As String)
                _idProductoserial = value
            End Set
        End Property

        Public Property regionSerial() As String
            Get
                Return _regionSerial
            End Get
            Set(ByVal value As String)
                _regionSerial = value
            End Set
        End Property

        Public Property noConformidadSerial() As String
            Get
                Return _noConformidadSerial
            End Get
            Set(ByVal value As String)
                _noConformidadSerial = value
            End Set
        End Property

        Public Property fechaproduccionSerial() As String
            Get
                Return _fechaProduccionSerial
            End Get
            Set(ByVal value As String)
                _fechaProduccionSerial = value
            End Set
        End Property

        Public Property enOrdenAbiertaSerial() As String
            Get
                Return _enOrdenAbiertaSerial
            End Get
            Set(ByVal value As String)
                _enOrdenAbiertaSerial = value
            End Set
        End Property

        Public Property idOrdenAnteriorSerial() As String
            Get
                Return _idOrdenAnteriorSerial
            End Get
            Set(ByVal value As String)
                _idOrdenAnteriorSerial = value
            End Set
        End Property

        Public Property idFacturaSerial() As String
            Get
                Return _idFacturaSerial
            End Get
            Set(ByVal value As String)
                _idFacturaSerial = value
            End Set
        End Property

        Public Property tipoOrdenSerial() As String
            Get
                Return _tipoOrdenSerial
            End Get
            Set(ByVal value As String)
                _tipoOrdenSerial = value
            End Set
        End Property

        Public Property existePIN() As Boolean
            Get
                Return _existePIN
            End Get
            Set(ByVal value As Boolean)
                _existePIN = value
            End Set
        End Property

        Public Property regexPIN() As String
            Get
                Return _regexPIN
            End Get
            Set(ByVal value As String)
                _regexPIN = value
            End Set
        End Property

        Public Property Niu() As String
            Get
                Return _Niu
            End Get
            Set(ByVal value As String)
                _Niu = value
            End Set
        End Property

        Public Property estibaReproceso() As String
            Get
                Return _estibaReproceso
            End Get
            Set(ByVal value As String)
                _estibaReproceso = value
            End Set
        End Property

        Public Property idRegionReproceso() As Integer
            Get
                Return _idRegionReproceso
            End Get
            Set(ByVal value As Integer)
                _idRegionReproceso = value
            End Set
        End Property

        Public Property idProductoDevolucion() As Integer
            Get
                Return _idProductoDevolucion
            End Get
            Set(ByVal value As Integer)
                _idProductoDevolucion = value
            End Set
        End Property

        Public Property facturaSecuencia() As Integer
            Get
                Return _facturaSecuencia
            End Get
            Set(ByVal value As Integer)
                _facturaSecuencia = value
            End Set
        End Property

        Public Property nombreRegion() As String
            Get
                Return _nombreRegion
            End Get
            Set(ByVal value As String)
                _nombreRegion = value
            End Set
        End Property

        Public Property centroRegion() As String
            Get
                Return _centroRegion
            End Get
            Set(ByVal value As String)
                _centroRegion = value
            End Set
        End Property

        Public Property idEstadoOrden() As Integer
            Get
                Return _idEstadoOrden
            End Get
            Set(ByVal value As Integer)
                _idEstadoOrden = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Sub New(ByVal sIdOrden As Integer)
            Me.New()
            ObtenerDatosMuestreo(sIdOrden)
        End Sub

        Sub New(ByVal sIdTipoproducto As Integer, ByVal sIdTecnologia As Integer)
            Me.New()
            CargaValidacion(sIdTipoproducto, sIdTecnologia)
        End Sub

#End Region

#Region "Metodos Públicos"
        Public Function CargarFacturasFulfillment(ByVal _idLinea As Integer, ByVal filtroOrden As String, ByVal filtroOrden1 As String, ByVal filtroFacturas As Integer) As DataTable

            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idLinea", SqlDbType.VarChar).Value = _idLinea
                        .Add("@filtroOrden", SqlDbType.VarChar).Value = filtroOrden
                        .Add("@filtroOrden1", SqlDbType.VarChar).Value = filtroOrden1
                    End With
                    dt = .ejecutarDataTable("ObtenerFacturaFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Function CargarFacturasReprocesosFulfillment(ByVal _idLinea As Integer, ByVal filtroOrden As String, ByVal filtroFacturas As Integer) As DataTable

            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idLinea", SqlDbType.VarChar).Value = _idLinea
                        .Add("@filtroOrden", SqlDbType.VarChar).Value = filtroOrden
                    End With
                    dt = .ejecutarDataTable("ObtenerFacturaReprocesoFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Function ObtenerRegion() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                Return db.ejecutarDataTable("ObtenerRegiones", CommandType.StoredProcedure)
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

        Public Function cantidadOTL(ByVal idFactura As String, ByVal filtroOrden As String) As String
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Integer
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.VarChar).Value = idFactura
                        .Add("@filtroOrden", SqlDbType.VarChar).Value = filtroOrden
                    End With
                    resultado = .ejecutarScalar("ObtenerCantidadOtlFulfillment", CommandType.StoredProcedure)
                    If resultado > 0 Then
                        _existenOTL = True
                    Else
                        _existenOTL = False
                    End If

                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return resultado
        End Function

        Public Function ConfirmacionDePalletRequerida(ByVal idordent As String) As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrdenTrabajo", SqlDbType.BigInt).Value = idordent
                    End With
                    Return .ejecutarScalar("HayConfirmacionPalletEnOrdenTrabajo", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

        Public Function ObtenerOrdenes(ByVal idFactura As Integer, ByVal sfiltroOrden As String, ByVal sfiltroOrden1 As String, ByVal sRegion As Integer, ByVal idLinea As Integer, ByVal idOrden As String, ByVal idSubproducto As Integer, ByVal filtro As String) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.VarChar).Value = idFactura
                        .Add("@region", SqlDbType.VarChar).Value = sRegion
                        .Add("@idLinea", SqlDbType.VarChar).Value = idLinea
                        .Add("@filtroOrden", SqlDbType.VarChar).Value = sfiltroOrden
                        .Add("@filtroOrden1", SqlDbType.VarChar).Value = sfiltroOrden1
                        .Add("@filtro", SqlDbType.VarChar).Value = filtro
                    End With
                    dt = .ejecutarDataTable("ObtenerOrdenesFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Function ObtenerOrdenesReprocesos(ByVal idFactura As Integer, ByVal sfiltroOrden As String, ByVal sRegion As Integer, ByVal idLinea As Integer, ByVal idOrden As String, ByVal idSubproducto As Integer, ByVal filtro As String) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.VarChar).Value = idFactura
                        .Add("@region", SqlDbType.VarChar).Value = sRegion
                        .Add("@idLinea", SqlDbType.VarChar).Value = idLinea
                        .Add("@filtroOrden", SqlDbType.VarChar).Value = sfiltroOrden
                    End With
                    dt = .ejecutarDataTable("ObtenerOrdenesReprocesoFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Function ConfirmarPallet(ByVal ordenTrabajo As Long, ByVal idPallet As Long, ByVal idUsuario As Long) As Integer
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrdenTrabajo", SqlDbType.BigInt).Value = ordenTrabajo
                        .Add("@idPallet", SqlDbType.BigInt).Value = idPallet
                        .Add("@idConfirmador", SqlDbType.BigInt).Value = idUsuario
                    End With
                    Return .ejecutarScalar("ConfirmarPalletEnOrdenDeTrabajo", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

        Public Function ObtenerDatosMuestreo(ByVal sidOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = sidOrden
                    End With
                    .ejecutarReader("ObtenerPorcentajeMuestreo", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("porcentajeMuestreo").ToString, _PorcentajeMuestreo)
                        Long.TryParse(.Reader("cantidadMuestra").ToString, _CantidadMuestra)
                    End If
                    .Reader.Close()
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

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
                        _estibaReproceso = .Reader("estiba").ToString
                        Long.TryParse(.Reader("idregion").ToString, _idRegionReproceso)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Function

        Public Function ExistenOTLs(ByVal sidOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = sidOrden
                    End With
                    _existenOTL = .ejecutarScalar("ValidarSiExistenOrdenesLectura", CommandType.StoredProcedure)
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

        Public Function validaSerial(ByVal serial As String) As Boolean
            Dim resultado As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = serial
                    End With

                    resultado = .ejecutarScalar("ExisteSerialFulfillment", CommandType.StoredProcedure)

                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return resultado
        End Function

        Public Sub validaSim(ByVal sim As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@sim", SqlDbType.VarChar).Value = sim
                    End With
                    .ejecutarReader("DatosSimsFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("idestado").ToString, _estadoSim)
                        _materialSim = .Reader("material").ToString
                        _regionSim = .Reader("region").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub validaPin(ByVal _Pin As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@pin", SqlDbType.VarChar).Value = _Pin
                    End With
                    .ejecutarReader("ValidarExistePin", CommandType.StoredProcedure)

                    If .Reader.Read Then
                        Boolean.TryParse(.Reader("existePIN").ToString, _existePIN)
                        _regexPIN = .Reader("REGEX_PIN").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ObtenerEstadofactura(ByVal idFactura As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.VarChar).Value = idFactura
                    End With
                    .ejecutarReader("ObtenerEstadoFacturaFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("idEstado").ToString, _idEstadoFactura)
                        _estadofacturas = .Reader("estado").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ObtenerEstadoOrden(ByVal idOrden As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    End With
                    .ejecutarReader("ObtenerEstadoOrdenesFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("idEstado").ToString, _idEstadoOrden)
                        _estadoOrden = .Reader("estado").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub obtenerInfoSerial(ByVal _serial As String, ByVal _filtro As String, ByVal _idOrden As String, ByVal _idcaja As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                    End With
                    .ejecutarReader("ObtenerInfoSerialFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _idSubproductoSerial = .Reader("idsubproducto").ToString
                        _idOrdenAnteriorSerial = .Reader("idOrden").ToString
                        _idFacturaSerial = .Reader("idFactura").ToString
                        _regionSerial = .Reader("region").ToString
                        _tipoOrdenSerial = .Reader("tipoOrden").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub obtenerInfoProduccionSerial(ByVal _serial As String, ByVal _filtro As String, ByVal _idOrden As String, ByVal _idcaja As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                    End With
                    .ejecutarReader("ObtenerInfoProduccionSerialFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _idSubproductoSerial = .Reader("idSubproducto").ToString
                        _idordenSerial = .Reader("idOrden").ToString
                        _regionSerial = .Reader("region").ToString
                        _idProductoserial = .Reader("idProducto").ToString
                        _noConformidadSerial = .Reader("no_Conformidad").ToString
                        _fechaProduccionSerial = .Reader("fechaProduccion").ToString
                        _enOrdenAbiertaSerial = .Reader("enOrdenAbierta").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub obtenerInfoCantidadSerial(ByVal _serial As String, ByVal _filtro As String, ByVal _idOrden As String, ByVal _idcaja As String)
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

        Public Sub registrarSerial(ByVal TipoAccion As Integer, ByVal serial As String, ByVal idProducto As Integer, ByVal idFactura As Integer, _
                                   ByVal region As String, ByVal caja As Integer, ByVal estiba As Integer, ByVal facturaSecuencia As Integer, _
                                   ByVal idSubproducto As Integer, ByVal idOrden As Integer, ByVal ordenSecuencia As Integer, ByVal linea As Integer, _
                                   ByVal Sim As String, ByVal pin As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@tipoAccion", SqlDbType.BigInt).Value = TipoAccion
                        .Add("@serial", SqlDbType.VarChar).Value = serial
                        .Add("@idProducto", SqlDbType.BigInt).Value = idProducto
                        .Add("@idFactura", SqlDbType.BigInt).Value = idFactura
                        .Add("@region", SqlDbType.VarChar).Value = region
                        .Add("@caja", SqlDbType.BigInt).Value = caja
                        .Add("@estiba", SqlDbType.BigInt).Value = estiba
                        .Add("@secuenciaEnFactura", SqlDbType.BigInt).Value = facturaSecuencia
                        .Add("@idSubproducto", SqlDbType.BigInt).Value = idSubproducto
                        .Add("@idOrden", SqlDbType.BigInt).Value = idOrden
                        .Add("@secuenciaEnOrden", SqlDbType.BigInt).Value = ordenSecuencia
                        .Add("@linea", SqlDbType.BigInt).Value = linea
                        .Add("@sim", SqlDbType.VarChar).Value = Sim
                        .Add("@pin", SqlDbType.VarChar).Value = pin
                    End With

                    .ejecutarNonQuery("RegistrarSerialFulfillment", CommandType.StoredProcedure)

                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub RegistrarSerialReproceso(ByVal _IngresaPorDevolucion As Boolean, ByVal _serial As String, ByVal _idProducto As String, ByVal _idFactura As String, _
                                            ByVal _region As String, ByVal _caja As Integer, ByVal _estiba As Integer, ByVal _facturaSecuencia As Integer, _
                                            ByVal _idSubproducto As Integer, ByVal _idOrden As Integer, ByVal _ordenSecuencia As Integer, ByVal _linea As Integer, _
                                            ByVal _Sim As String)

            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@ingresaPorDevolucion", SqlDbType.BigInt).Value = _IngresaPorDevolucion
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                        .Add("@idProducto", SqlDbType.BigInt).Value = _idProducto
                        .Add("@idFactura", SqlDbType.BigInt).Value = _idFactura
                        .Add("@region", SqlDbType.VarChar).Value = _region
                        .Add("@caja", SqlDbType.BigInt).Value = _caja
                        .Add("@estiba", SqlDbType.BigInt).Value = _estiba
                        .Add("@secuenciaEnFactura", SqlDbType.BigInt).Value = _facturaSecuencia
                        .Add("@idSubproducto", SqlDbType.BigInt).Value = _idSubproducto
                        .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                        .Add("@secuenciaEnOrden", SqlDbType.BigInt).Value = _ordenSecuencia
                        .Add("@linea", SqlDbType.BigInt).Value = _linea
                        .Add("@sim", SqlDbType.VarChar).Value = _Sim
                    End With
                    .ejecutarNonQuery("RegistrarReprocesoDeSerialFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub registrarSerialMuestra(ByVal _serial As String, ByVal _idorden As String, ByVal _iduser As String)

            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim resultado As Integer
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@serial", SqlDbType.VarChar).Value = _serial
                        .Add("@idOrden", SqlDbType.VarChar).Value = _idorden
                        .Add("@idCreador", SqlDbType.Int).Value = _iduser
                    End With
                    resultado = .ejecutarScalar("CrearSerialMuestra", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub RegistrarSerialenProductoSerial(ByVal idfactura As Integer, ByVal sRegion As String, ByVal idLinea As Integer, ByVal idOrden As String, ByVal origen As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.BigInt).Value = idfactura
                        .Add("@region", SqlDbType.VarChar).Value = sRegion
                        .Add("@idLinea", SqlDbType.BigInt).Value = idLinea
                        .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    End With
                    If origen <> "REPRIMEI" Then
                        .ejecutarNonQuery("InsertarDatosFulfillment", CommandType.StoredProcedure)
                    Else
                        .ejecutarNonQuery("InsertarDatosReprocesosFulfillment", CommandType.StoredProcedure)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

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
                        _Niu = .Reader("niu").ToString
                    End If
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

        Public Sub DatosRegion(ByVal _idRegion As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idregion", SqlDbType.BigInt).Value = _idRegion
                    End With
                    .ejecutarReader("ObtenerDatosRegionFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _nombreRegion = .Reader("nombreRegion").ToString
                        _centroRegion = .Reader("centro").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub
#End Region

    End Class
End Namespace