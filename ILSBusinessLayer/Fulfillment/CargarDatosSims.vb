Imports LMDataAccessLayer
Namespace Fulfillment
    Public Class CargarDatosSims

#Region "Atributos (Campos)"

        Private _ordentrabajo As String
        Private _idsubproducto As String
        Private _subproducto As String
        Private _cntLeida As Integer
        Private _cntPedida As Integer
        Private _cajasPorHuacal As Integer
        Private _totalRegion As Integer
        Private _leerSimSuelta As Boolean
        Private _unidaddesPorCaja As Integer
        Private _idTipoProducto As Integer
        Private _idTecnologia As Integer
        Private _huacal As Integer
        Private _caja As Integer
        Private _caracterespermitidos As String
        Private _longitudminimasim As Integer
        Private _longitudmaximasim As Integer
        Private _estadoOrden As String
        Private _rangosValidos As String

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

        Public Property cantidadLeidad() As Integer
            Get
                Return _cntLeida
            End Get
            Set(ByVal value As Integer)
                _cntLeida = value
            End Set
        End Property

        Public Property cantidadPedida() As Integer
            Get
                Return _cntPedida
            End Get
            Set(ByVal value As Integer)
                _cntPedida = value
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

        Public Property totalRegion() As Integer
            Get
                Return _totalRegion
            End Get
            Set(ByVal value As Integer)
                _totalRegion = value
            End Set
        End Property

        Public Property leerSimSuelta() As Boolean
            Get
                Return _leerSimSuelta
            End Get
            Set(ByVal value As Boolean)
                _leerSimSuelta = value
            End Set
        End Property

        Public Property unidadesPorCaja() As Integer
            Get
                Return _unidaddesPorCaja
            End Get
            Set(ByVal value As Integer)
                _unidaddesPorCaja = value
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

        Public Property huacal() As Integer
            Get
                Return _huacal
            End Get
            Set(ByVal value As Integer)
                _huacal = value
            End Set
        End Property

        Public Property caja() As Integer
            Get
                Return _caja
            End Get
            Set(ByVal value As Integer)
                _caja = value
            End Set
        End Property

        Public Property caracterespermitidos() As String
            Get
                Return _caracterespermitidos
            End Get
            Set(ByVal value As String)
                _caracterespermitidos = value
            End Set
        End Property

        Public Property longitudMinima() As Integer
            Get
                Return _longitudminimasim
            End Get
            Set(ByVal value As Integer)
                _longitudminimasim = value
            End Set
        End Property

        Public Property longitudMaxima() As Integer
            Get
                Return _longitudmaximasim
            End Get
            Set(ByVal value As Integer)
                _longitudmaximasim = value
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

        Public Property rangosValidos() As String
            Get
                Return _rangosValidos
            End Get
            Set(ByVal value As String)
                _rangosValidos = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub
#End Region

#Region "Metodos Públicos"
        Public Function CargarFacturasSims(ByVal _idLinea As Integer) As DataTable

            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idLinea", SqlDbType.VarChar).Value = _idLinea
                    End With
                    dt = .ejecutarDataTable("ObtenerFacturaSimsFulfillment", CommandType.StoredProcedure)
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

        Public Function ObtenerOrdenes(ByVal idFactura As Integer, ByVal sRegion As Integer, ByVal idLinea As Integer, ByVal idOrden As String, ByVal filtro As String) As DataTable
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
                    dt = .ejecutarDataTable("ObtenerOrdenesSimsFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return dt
        End Function

        Public Sub ObtenerOrdenSim(ByVal idFactura As Integer, ByVal sRegion As Integer, ByVal idLinea As Integer, ByVal idOrden As String, ByVal filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                        .Add("@idfactura", SqlDbType.VarChar).Value = idFactura
                    End With
                    .ejecutarReader("ObtenerOrdenTrabajoSimsFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _ordentrabajo = .Reader("codigo").ToString
                        _idsubproducto = .Reader("idsubproducto").ToString
                        _subproducto = .Reader("subproducto").ToString
                        Long.TryParse(.Reader("cantidadLeida").ToString, _cntLeida)
                        Long.TryParse(.Reader("cantidadPedida").ToString, _cntPedida)
                        Long.TryParse(.Reader("cajasPorHuacal").ToString, _cajasPorHuacal)
                        Long.TryParse(.Reader("totalRegion").ToString, _totalRegion)
                        Boolean.TryParse(.Reader("leerSimSuelta").ToString, _leerSimSuelta)
                        Long.TryParse(.Reader("unidadesPorCaja").ToString, _unidaddesPorCaja)
                        Long.TryParse(.Reader("idTipoProducto").ToString, _idTipoProducto)
                        Long.TryParse(.Reader("idTecnologia").ToString, _idTecnologia)
                        _estadoOrden = .Reader("estado").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ObtenerHuacalSim(ByVal idFactura As Integer, ByVal sRegion As Integer, ByVal idLinea As Integer, ByVal idOrden As String, ByVal filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    End With
                    .ejecutarReader("ObtenerHuacalSimsFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("huacal").ToString, _huacal)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub ObtenerCajaSim(ByVal idFactura As Integer, ByVal sRegion As Integer, ByVal idLinea As Integer, ByVal idOrden As String, ByVal filtro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    End With
                    .ejecutarReader("ObtenerCajaSimsFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("caja").ToString, _caja)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Function validaSim(ByVal sim As String) As Boolean
            Dim resultado As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@sim", SqlDbType.VarChar).Value = sim
                    End With
                    resultado = .ejecutarScalar("ExisteSimsFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return resultado
        End Function

        Public Sub ObtenerInfoConfiguracionSims()
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                    End With
                    .ejecutarReader("ObtenerInfoConfiguracionSims", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        _caracterespermitidos = .Reader("caracteresPermitidos").ToString
                        Long.TryParse(.Reader("longitudMenor").ToString, _longitudminimasim)
                        Long.TryParse(.Reader("longitudMayor").ToString, _longitudmaximasim)
                        _rangosValidos = .Reader("rangosValidos").ToString
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Function validarRangodeSimEnProduccion(ByVal _simIni As String, ByVal _simFin As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim cntSims As Integer
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("simIni", SqlDbType.VarChar).Value = _simIni
                        .Add("simFin", SqlDbType.VarChar).Value = _simFin
                    End With
                    cntSims = .ejecutarScalar("ValidarCantidadSimsFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
            Return CntSims
        End Function

        Public Sub registrarSim(ByVal _sim As String, ByVal _idfactura As String, ByVal _idOrden As String, ByVal _idproducto As String, ByVal _idsubProducto As String, _
                                ByVal _region As String, ByVal _huacal As String, ByVal _caja As String)

            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@sim", SqlDbType.VarChar).Value = _sim
                        .Add("@idFactura", SqlDbType.VarChar).Value = _idfactura
                        .Add("@idOrden", SqlDbType.VarChar).Value = _idOrden
                        .Add("@idProducto", SqlDbType.VarChar).Value = _idproducto
                        .Add("@idSubProducto", SqlDbType.VarChar).Value = _idsubProducto
                        .Add("@region", SqlDbType.VarChar).Value = _region
                        .Add("@huacal", SqlDbType.VarChar).Value = _huacal
                        .Add("@caja", SqlDbType.VarChar).Value = _caja
                    End With
                    .ejecutarNonQuery("RegistrarSimFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub registrarSimCaja(ByVal _simIni As String, ByVal _simFin As String, ByVal _idfactura As String, ByVal _idOrden As String, ByVal _idproducto As String, ByVal _idsubProducto As String, _
                                ByVal _region As String, ByVal _huacal As String, ByVal _caja As String)

            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@simInicial", SqlDbType.VarChar).Value = _simIni
                        .Add("@simFinal", SqlDbType.VarChar).Value = _simFin
                        .Add("@idFactura", SqlDbType.VarChar).Value = _idfactura
                        .Add("@idOrden", SqlDbType.VarChar).Value = _idOrden
                        .Add("@idProducto", SqlDbType.VarChar).Value = _idproducto
                        .Add("@idSubProducto", SqlDbType.VarChar).Value = _idsubProducto
                        .Add("@region", SqlDbType.VarChar).Value = _region
                        .Add("@huacal", SqlDbType.VarChar).Value = _huacal
                        .Add("@caja", SqlDbType.VarChar).Value = _caja
                    End With
                    .ejecutarNonQuery("RegistrarSimsPorCajaFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub RegistrarSimsEnProduccion(ByVal idfactura As Integer, ByVal sRegion As String, ByVal idOrden As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idFactura", SqlDbType.BigInt).Value = idfactura
                        .Add("@region", SqlDbType.VarChar).Value = sRegion
                        .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    End With
                    .ejecutarNonQuery("InsertarDatosSimsFulfillment", CommandType.StoredProcedure)
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

#End Region

    End Class
End Namespace