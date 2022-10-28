Public Class LecturaHandset
    Inherits FulfillmentBase

    Public Sub New()
        MyBase.new()
        CargarDatos()
    End Sub

#Region "Atributos (Campos)"

    Private _subProducto As String
    Private _material As String
    Private _serial As String
    Private _telefonoSecuencia As Integer
    Private _unidades_caja As Integer
    Private _caja As Integer
    Private _fechaProduccion As String
    Private _codigoEan As String
    Private _infoAdicional As String
    Private _estiba As Integer
    Private _cajasestiba As Integer
    Private _idfactura2 As String
    Private _guia As String
    Private _idorden As Integer
    Private _idfactura As Integer
    Private _region As String
    Private _orden As String
    Private _centro As String
    Private _sim As String
    Private _niu As String
    Private _noconformidad1 As String
    Private _noconformidad2 As String
    Private _noconformidad3 As String
    Private _noconformidad4 As String
    Private _tipoSoftware As String
    Private _otb As String
    Private _infoHomologacion As String
    Private _tipoProducto As String

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

    Public Property telefonoSecuencia() As Integer
        Get
            Return _telefonoSecuencia
        End Get
        Set(ByVal value As Integer)
            _telefonoSecuencia = value
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

    Public Property caja() As Integer
        Get
            Return _caja
        End Get
        Set(ByVal value As Integer)
            _caja = value
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

    Public Property codigoEan() As String
        Get
            Return _codigoEan
        End Get
        Set(ByVal value As String)
            _codigoEan = value
        End Set
    End Property

    Public Property infoAdicional() As String
        Get
            Return _infoAdicional
        End Get
        Set(ByVal value As String)
            _infoAdicional = value
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

    Public Property cajas_estiba() As Integer
        Get
            Return _cajasestiba
        End Get
        Set(ByVal value As Integer)
            _cajasestiba = value
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

    Public Property idorden() As Integer
        Get
            Return _idorden
        End Get
        Set(ByVal value As Integer)
            _idorden = value
        End Set
    End Property

    Public Property idfactura() As Integer
        Get
            Return _idfactura
        End Get
        Set(ByVal value As Integer)
            _idfactura = value
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

    Public Property centro() As String
        Get
            Return _centro
        End Get
        Set(ByVal value As String)
            _centro = value
        End Set
    End Property

    Public Property sim() As String
        Get
            Return _sim
        End Get
        Set(ByVal value As String)
            _sim = value

        End Set
    End Property

    Public Property niu() As String
        Get
            Return _niu
        End Get
        Set(ByVal value As String)
            _niu = value
        End Set
    End Property

    Public Property noconformidad1() As String
        Get
            Return _noconformidad1
        End Get
        Set(ByVal value As String)
            _noconformidad1 = value
        End Set
    End Property

    Public Property noconformidad2() As String
        Get
            Return _noconformidad2
        End Get
        Set(ByVal value As String)
            _noconformidad2 = value
        End Set
    End Property

    Public Property noconformidad3() As String
        Get
            Return _noconformidad3
        End Get
        Set(ByVal value As String)
            _noconformidad3 = value
        End Set
    End Property

    Public Property noconformidad4() As String
        Get
            Return _noconformidad4
        End Get
        Set(ByVal value As String)
            _noconformidad4 = value
        End Set
    End Property

    Public Property TipoSoftware As String
        Get
            Return _tipoSoftware
        End Get
        Set(value As String)
            _tipoSoftware = value
        End Set
    End Property

    Public Property Otb As String
        Get
            Return _otb
        End Get
        Set(value As String)
            _otb = value
        End Set
    End Property

    Public Property InfoHomologacion As String
        Get
            Return _infoHomologacion
        End Get
        Set(value As String)
            _infoHomologacion = value
        End Set
    End Property

    Public Property TipoProducto As String
        Get
            Return _tipoProducto
        End Get
        Set(value As String)
            _tipoProducto = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        RequiereSim = True
        RequierePin = True
        RequiereComprobarPallet = False
        ImprimeStickersCaja = True
    End Sub

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

    Public Sub obtenerDatosImpresion(ByVal idFactura As Integer, ByVal idOrden As String, ByVal serial As String, ByVal origen As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idFactura", SqlDbType.BigInt).Value = idFactura
                    .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    .Add("@serial", SqlDbType.VarChar).Value = serial
                End With
                .ejecutarReader("ObtenerDatosStickersTelefonosFulfillment", CommandType.StoredProcedure)
                If .Reader.Read Then
                    _subProducto = .Reader("subproducto").ToString
                    _material = .Reader("material").ToString
                    _serial = .Reader("serial").ToString
                    Long.TryParse(.Reader("facturaSecuencia").ToString, _telefonoSecuencia)
                    Long.TryParse(.Reader("unidadesCaja").ToString, _unidades_caja)
                    Long.TryParse(.Reader("caja").ToString, _caja)
                    _fechaProduccion = .Reader("fecha").ToString
                    _codigoEan = .Reader("codigoEan").ToString
                    _infoAdicional = "F.P" & FormatDateTime(Date.Now, DateFormat.ShortDate) & "    " & "TEL: " & .Reader("facturaSecuencia").ToString & "/" _
                                     & .Reader("unidadesCaja").ToString & " - " & .Reader("caja").ToString & .Reader("orden").ToString
                    Long.TryParse(.Reader("idorden").ToString, _idorden)
                    Long.TryParse(.Reader("estiba").ToString, _estiba)
                    _idfactura2 = .Reader("idfactura2").ToString
                    Long.TryParse(.Reader("idfactura").ToString, _idfactura)
                    _region = .Reader("region").ToString
                    _guia = .Reader("guia_aerea").ToString
                    _orden = .Reader("orden").ToString
                    Long.TryParse(.Reader("cajasestiba").ToString, _cajasestiba)
                    _centro = .Reader("centro").ToString
                    _sim = .Reader("sim").ToString
                    _niu = .Reader("niu").ToString
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Sub ObtenerDatosImpresionReprocesos(ByVal idOrden As String, ByVal serial As String, ByVal tipoProdcucto As Integer)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idTipoProducto", SqlDbType.BigInt).Value = tipoProdcucto
                    .Add("@idOrdenReproceso", SqlDbType.VarChar).Value = idOrden
                    .Add("@serial", SqlDbType.VarChar).Value = serial
                End With
                .ejecutarReader("ObtenerDatosStickersTelefonosFulfillmentReproceso", CommandType.StoredProcedure)
                If .Reader.Read Then
                    _subProducto = .Reader("descripcion").ToString
                    _material = .Reader("Material").ToString
                    _serial = .Reader("serial").ToString
                    Long.TryParse(.Reader("facturaSecuencia").ToString, _telefonoSecuencia)
                    Long.TryParse(.Reader("unidadesCaja").ToString, _unidades_caja)
                    Long.TryParse(.Reader("caja").ToString, _caja)
                    _fechaProduccion = .Reader("fecha").ToString
                    _codigoEan = .Reader("codigoEan").ToString
                    _infoAdicional = "F.P" & FormatDateTime(Date.Now, DateFormat.ShortDate) & "    " & "TEL: " & .Reader("facturaSecuencia").ToString & "/" _
                                     & .Reader("unidadesCaja").ToString & " - " & .Reader("caja").ToString & .Reader("orden").ToString
                    Long.TryParse(.Reader("idorden").ToString, _idorden)
                    Long.TryParse(.Reader("estiba").ToString, _estiba)
                    _idfactura2 = .Reader("idfactura2").ToString
                    Long.TryParse(.Reader("idfactura").ToString, _idfactura)
                    _region = .Reader("region").ToString
                    _guia = .Reader("guia_aerea").ToString
                    _orden = .Reader("orden").ToString
                    Long.TryParse(.Reader("cajaEstiba").ToString, _cajasestiba)
                    _centro = .Reader("centro").ToString
                    _sim = .Reader("sim").ToString
                    _niu = .Reader("niu").ToString
                    _tipoSoftware = .Reader("tipoSoftware").ToString
                    _infoHomologacion = .Reader("infoHomologacion").ToString
                    _tipoProducto = .Reader("tipoProdcucto").ToString
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Sub ObtenerDatosImpresionSimCaja(ByVal idOrden As String, ByVal tipoProdcucto As Integer, ByVal idUsuario As Integer, caja As Integer, Optional flag As Integer = 0)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idTipoProducto", SqlDbType.BigInt).Value = tipoProdcucto
                    .Add("@idOrdenReproceso", SqlDbType.VarChar).Value = idOrden
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@caja", SqlDbType.Int).Value = caja
                    .Add("@flag", SqlDbType.Int).Value = flag
                End With
                .ejecutarReader("ObtenerDatosStickersCajaSim", CommandType.StoredProcedure)
                If .Reader.Read Then
                    _subProducto = .Reader("descripcion").ToString
                    _material = .Reader("material").ToString
                    _region = .Reader("region").ToString
                    _otb = .Reader("otb").ToString
                    _caja = .Reader("caja").ToString
                    _fechaProduccion = .Reader("fecha").ToString
                    _infoAdicional = .Reader("infoAdicional").ToString
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Sub ObtenerDatosImpresionNoConformesReproceso(ByVal idOrdenReproceso As Integer, ByVal serial As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idOrdenReproceso", SqlDbType.Int).Value = idOrdenReproceso
                    .Add("@serial", SqlDbType.VarChar, 20).Value = serial
                End With
                .ejecutarReader("ObtenerDatosStickersTelefonosNCFulfillmentReprocesos", CommandType.StoredProcedure)
                If .Reader.Read Then
                    _subProducto = .Reader("descripcion").ToString
                    _material = .Reader("material").ToString
                    _serial = .Reader("serial").ToString
                    Long.TryParse(.Reader("facturaSecuencia").ToString, _telefonoSecuencia)
                    Long.TryParse(.Reader("unidadesCaja").ToString, _unidades_caja)
                    Long.TryParse(.Reader("caja").ToString, _caja)
                    _fechaProduccion = .Reader("fecha").ToString
                    _codigoEan = .Reader("codigoEan").ToString
                    _infoAdicional = "F.P" & FormatDateTime(Date.Now, DateFormat.ShortDate) & "    " & "TEL: " & .Reader("facturaSecuencia").ToString & "/" _
                                     & .Reader("unidadesCaja").ToString & " - " & .Reader("caja").ToString & .Reader("orden").ToString
                    Long.TryParse(.Reader("idOrden").ToString, _idorden)
                    Long.TryParse(.Reader("estiba").ToString, _estiba)
                    _idfactura2 = .Reader("idfactura2").ToString
                    Long.TryParse(.Reader("idFactura").ToString, _idfactura)
                    _region = .Reader("region").ToString
                    _guia = .Reader("guia_aerea").ToString
                    _orden = .Reader("orden").ToString
                    Long.TryParse(.Reader("cajaEstiba").ToString, _cajasestiba)
                    _centro = .Reader("centro").ToString
                    _sim = .Reader("sim").ToString
                    _niu = .Reader("niu").ToString
                    _noconformidad1 = .Reader("noconformidad1").ToString
                    _noconformidad2 = .Reader("noconformidad2").ToString
                    _noconformidad3 = .Reader("noconformidad3").ToString
                    _noconformidad4 = .Reader("noconformidad4").ToString
                    _tipoSoftware = .Reader("tipoSoftware").ToString
                    _infoHomologacion = .Reader("infoHomologacion").ToString
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

    Public Sub obtenerDatosImpresionNoConformes(ByVal idFactura As Integer, ByVal idOrden As String, ByVal serial As String, ByVal origen As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                With .SqlParametros
                    .Clear()
                    .Add("@idFactura", SqlDbType.BigInt).Value = idFactura
                    .Add("@idOrden", SqlDbType.VarChar).Value = idOrden
                    .Add("@serial", SqlDbType.VarChar).Value = serial
                End With
                .ejecutarReader("ObtenerDatosStickersTelefonosNCFulfillment", CommandType.StoredProcedure)
                If .Reader.Read Then
                    _subProducto = .Reader("subproducto").ToString
                    _material = .Reader("material").ToString
                    _serial = .Reader("serial").ToString
                    Long.TryParse(.Reader("facturaSecuencia").ToString, _telefonoSecuencia)
                    Long.TryParse(.Reader("unidadesCaja").ToString, _unidades_caja)
                    Long.TryParse(.Reader("caja").ToString, _caja)
                    _fechaProduccion = .Reader("fecha").ToString
                    _codigoEan = .Reader("codigoEan").ToString
                    _infoAdicional = "F.P" & FormatDateTime(Date.Now, DateFormat.ShortDate) & "    " & "TEL: " & .Reader("facturaSecuencia").ToString & "/" _
                                     & .Reader("unidadesCaja").ToString & " - " & .Reader("caja").ToString & .Reader("orden").ToString
                    Long.TryParse(.Reader("idorden").ToString, _idorden)
                    Long.TryParse(.Reader("estiba").ToString, _estiba)
                    _idfactura2 = .Reader("idfactura2").ToString
                    Long.TryParse(.Reader("idfactura").ToString, _idfactura)
                    _region = .Reader("region").ToString
                    _guia = .Reader("guia_aerea").ToString
                    _orden = .Reader("orden").ToString
                    Long.TryParse(.Reader("cajasestiba").ToString, _cajasestiba)
                    _centro = .Reader("centro").ToString
                    _sim = .Reader("sim").ToString
                    _niu = .Reader("niu").ToString
                    _noconformidad1 = .Reader("noconformidad1").ToString
                    _noconformidad2 = .Reader("noconformidad2").ToString
                    _noconformidad3 = .Reader("noconformidad3").ToString
                    _noconformidad4 = .Reader("noconformidad4").ToString
                End If
            End With
        Finally
            If db IsNot Nothing Then db.Dispose()
        End Try
    End Sub

#End Region

End Class











