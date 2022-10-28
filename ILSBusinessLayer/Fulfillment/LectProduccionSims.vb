Public Class LectProduccionSims
    Inherits FulfillmentBase

    Public Sub New()
        MyBase.new()
    End Sub

#Region "Atributos (Campos)"

    Private _subProducto As String
    Private _material As String
    Private _serial As String
    Private _unidades_caja As Integer
    Private _caja As Integer
    Private _fechaProduccion As String
    Private _estiba As Integer
    Private _idfactura2 As String
    Private _guia As String
    Private _region As String
    Private _orden As String
    Private _iccd1 As String
    Private _iccd2 As String

#End Region

#Region "Propiedades"

    Public Property subproducto() As String
        Get
            Return _subProducto
        End Get
        Set(ByVal value As String)
            _subProducto = value
        End Set
    End Property

    Public Property material() As String
        Get
            Return _material
        End Get
        Set(ByVal value As String)
            _material = value
        End Set
    End Property

    Public Property serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
        End Set
    End Property

    Public Property unidades_caja() As Integer
        Get
            Return _unidades_caja
        End Get
        Set(ByVal value As Integer)
            _unidades_caja = value
        End Set
    End Property

    Public Property fechaProduccion() As String
        Get
            Return _fechaProduccion
        End Get
        Set(ByVal value As String)
            _fechaProduccion = value
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

    Public Property estiba() As Integer
        Get
            Return _estiba
        End Get
        Set(ByVal value As Integer)
            _estiba = value
        End Set
    End Property

    Public Property idfactura2() As String
        Get
            Return _idfactura2
        End Get
        Set(ByVal value As String)
            _idfactura2 = value
        End Set
    End Property

    Public Property guia() As String
        Get
            Return _guia
        End Get
        Set(ByVal value As String)
            _guia = value
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

    Public Property orden() As String
        Get
            Return _orden
        End Get
        Set(ByVal value As String)
            _orden = value
        End Set
    End Property

    Public Property iccd1() As String
        Get
            Return _iccd1
        End Get
        Set(ByVal value As String)
            _iccd1 = value
        End Set
    End Property

    Public Property iccd2() As String
        Get
            Return _iccd2
        End Get
        Set(ByVal value As String)
            _iccd2 = value
        End Set
    End Property

#End Region

#Region "Metodos Publicos"
    Public Function obtenerNombreArchivo(ByVal sProceso As String, ByVal procesoGenerico As String) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dt As DataTable
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@nombreProceso", SqlDbType.VarChar).Value = sProceso
                    .Add("@nombreprocesoGenerico", SqlDbType.VarChar).Value = procesoGenerico
                End With
                dt = .ejecutarDataTable("ObtenerNombreArchivoImpresionFulfillment", CommandType.StoredProcedure)
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
        Return dt
    End Function

    Public Function obtenerMapeoDatosImpresion(ByVal sProceso As String, ByVal sProcesoGenerico As String) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dt As New DataTable
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@nombreProceso", SqlDbType.VarChar).Value = sProceso
                    .Add("@nombreProcesoGenerico", SqlDbType.VarChar).Value = sProcesoGenerico
                End With
                dt = .ejecutarDataTable("ObtenerMapeoDatosImpresionFulfillment", CommandType.StoredProcedure)
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
        Return dt
    End Function

    Public Sub obtenerDatosImpresion(ByVal idFactura As Integer, ByVal idOrden As String, ByVal SimInicial As String, ByVal SimFinal As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idFactura", SqlDbType.BigInt).Value = idFactura
                    .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    .Add("@simInicial", SqlDbType.VarChar).Value = SimInicial
                    .Add("@simFinal", SqlDbType.VarChar).Value = SimFinal
                End With
                .ejecutarReader("ObtenerDatosStickersSimsFulfillment", CommandType.StoredProcedure)
                If .Reader.Read Then
                    _material = .Reader("material").ToString
                    _region = .Reader("region").ToString
                    _idfactura2 = .Reader("idfactura2").ToString
                    Long.TryParse(.Reader("estiba").ToString, _estiba)
                    _guia = .Reader("guia_aerea").ToString
                    Long.TryParse(.Reader("caja").ToString, _caja)
                    _subProducto = .Reader("subproducto").ToString
                    _iccd1 = SimInicial
                    _iccd2 = SimFinal
                    _orden = .Reader("orden").ToString
                    _fechaProduccion = .Reader("fecha").ToString
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub
#End Region

End Class
