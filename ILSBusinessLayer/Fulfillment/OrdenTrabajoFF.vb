Imports ILSBusinessLayer
Imports LMDataAccessLayer
Namespace Fulfillment
    Public Class OrdenTrabajoFF

#Region "Atributos"
        Private _idOrden As Long
        Private _Orden As String
        Private _idSubproducto As Long
        Private _subproducto As String
        Private _material As Long
        Private _tipoOrden As String
        Private _leerSim As String
        Private _leerDupla As Boolean
        Private _generaNiu As Boolean
        Private _idEstadoOrden As Integer
        Private _estadoOrden As String
        Private _materialSim As String
        Private _codigoEan As String
        Private _idTipoProducto As Long
        Private _requierePin As Boolean
        Private _idTipoEtiqueta As Long
        Private _cantidadPedida As Long
        Private _cantidadLeida As Long
        Private _unidadesCaja As Long
        Private _cajasEstiba As Long
        Private _estiba As Long
        Private _caja As Long
        Private _totalCaja As Long
        Private _totalEstiba As Long
        Private _region As String
        Private _revisada As Boolean

#End Region

#Region "Propiedades"

        Public Property idOrden() As Long
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Long)
                _idOrden = value
            End Set
        End Property

        Public Property orden() As String
            Get
                Return _Orden
            End Get
            Set(ByVal value As String)
                _Orden = value
            End Set
        End Property

        Public Property idSubproducto() As Long
            Get
                Return _idSubproducto
            End Get
            Set(ByVal value As Long)
                _idSubproducto = value
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

        Public Property material() As Long
            Get
                Return _material
            End Get
            Set(ByVal value As Long)
                _material = value
            End Set
        End Property

        Public Property tipoOrden() As String
            Get
                Return _tipoOrden
            End Get
            Set(ByVal value As String)
                _tipoOrden = value
            End Set
        End Property

        Public Property leerSim() As String
            Get
                Return _leerSim
            End Get
            Set(ByVal value As String)
                _leerSim = value
            End Set
        End Property

        Public Property leerDupla() As Boolean
            Get
                Return _leerDupla
            End Get
            Set(ByVal value As Boolean)
                _leerDupla = value
            End Set
        End Property

        Public Property generaNiu() As Boolean
            Get
                Return _generaNiu
            End Get
            Set(ByVal value As Boolean)
                _generaNiu = value
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

        Public Property codigoEAN() As String
            Get
                Return _codigoEan
            End Get
            Set(ByVal value As String)
                _codigoEan = value
            End Set
        End Property

        Public Property idTipoProducto() As Long
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Long)
                _idTipoProducto = value
            End Set
        End Property

        Public Property requierePin() As Boolean
            Get
                Return _requierePin
            End Get
            Set(ByVal value As Boolean)
                _requierePin = value
            End Set
        End Property

        Public Property idTipoEtiqueta() As Long
            Get
                Return _idTipoEtiqueta
            End Get
            Set(ByVal value As Long)
                _idTipoEtiqueta = value
            End Set
        End Property

        Public Property cantidadPedida() As Long
            Get
                Return _cantidadPedida
            End Get
            Set(ByVal value As Long)
                _cantidadPedida = value
            End Set
        End Property

        Public Property cantidadLeida() As Long
            Get
                Return _cantidadLeida
            End Get
            Set(ByVal value As Long)
                _cantidadLeida = value
            End Set
        End Property

        Public Property unidadesCaja() As Long
            Get
                Return _unidadesCaja
            End Get
            Set(ByVal value As Long)
                _unidadesCaja = value
            End Set
        End Property

        Public Property cajasEstiba() As Long
            Get
                Return _cajasEstiba
            End Get
            Set(ByVal value As Long)
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

        Public Property regionOrden() As String
            Get
                Return _region
            End Get
            Set(ByVal value As String)
                _region = value
            End Set
        End Property

        Public Property revisada() As Boolean
            Get
                Return _revisada
            End Get
            Set(ByVal value As Boolean)
                _revisada = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Sub New()
            MyBase.New()
        End Sub

        Sub New(ByVal sIdOrden As String, ByVal sIdSubproducto As String, ByVal sFiltro As String)
            Me.New()
            cargarOrden(sIdOrden, sIdSubproducto, sFiltro)
        End Sub

        Sub New(ByVal sIdOrden As String, ByVal sFiltro As String)
            Me.New()
            If sFiltro = "cantidades" Then
                cargarCantidades(sIdOrden, sFiltro)
            Else
                cargarCajaEstiba(sIdOrden, sFiltro)
            End If
        End Sub

#End Region

#Region "metodos"

        Private Sub cargarOrden(ByVal sIdOrden As String, ByVal sIdSubproducto As String, ByVal sFiltro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess

            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = sIdOrden
                        .Add("@idSubproducto", SqlDbType.VarChar).Value = sIdSubproducto
                    End With
                    .ejecutarReader("ObtenerOrdenTrabajoFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(sIdOrden, _idOrden)
                        _Orden = .Reader("codigo").ToString.Trim
                        Long.TryParse(sIdSubproducto, _idSubproducto)
                        _subproducto = .Reader("subproducto").ToString.Trim
                        Long.TryParse(.Reader("material").ToString, _material)
                        _tipoOrden = .Reader("tipoOrden").ToString.Trim
                        _leerSim = .Reader("leerSim").ToString.Trim
                        Integer.TryParse(.Reader("idEstado").ToString, idEstadoOrden)
                        _estadoOrden = .Reader("estado").ToString.Trim
                        _codigoEan = .Reader("codigoEan").ToString.Trim
                        Long.TryParse(.Reader("idTipoProducto").ToString, _idTipoProducto)
                        _requierePin = CBool(.Reader("requierePin").ToString())
                        Long.TryParse(.Reader("idTipoEtiqueta").ToString, _idTipoEtiqueta)
                        _leerDupla = CBool(.Reader("leerDupla").ToString())
                        _materialSim = .Reader("materialSim").ToString.Trim
                        _region = .Reader("codigo").ToString.Trim
                        Boolean.TryParse(.Reader("revisada").ToString, _revisada)
                    End If
                    .Reader.Close()
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Private Sub cargarCantidades(ByVal sIdOrden As String, ByVal sFiltro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = sIdOrden
                    End With
                    .ejecutarReader("ObtenerCantidadesOrdenFulfillment", CommandType.StoredProcedure)
                    If .Reader.Read Then
                        Long.TryParse(.Reader("cantidadLeer").ToString, _cantidadPedida)
                        Long.TryParse(.Reader("cantidadLeida").ToString, _cantidadLeida)
                        Long.TryParse(.Reader("unidadesPorCaja").ToString, _unidadesCaja)
                        Long.TryParse(.Reader("cajasPorEstiba").ToString, _cajasEstiba)
                    End If
                    .Reader.Close()
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub

        Private Sub cargarCajaEstiba(ByVal sIdOrden As String, ByVal sFiltro As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrden", SqlDbType.VarChar).Value = sIdOrden
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
        End Sub

#End Region

#Region "Metodos Publicos"
        Public Sub cerrarOrden(ByVal idOrden As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idOrden", SqlDbType.BigInt).Value = idOrden
                    .Add("@idEstado", SqlDbType.BigInt).Value = 30
                End With
                .ejecutarNonQuery("CambiarEstadoOrdenesFulfillment", CommandType.StoredProcedure)
            End With
        End Sub
#End Region

    End Class
End Namespace