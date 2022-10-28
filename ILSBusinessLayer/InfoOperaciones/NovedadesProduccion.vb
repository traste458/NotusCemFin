Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports ILSBusinessLayer.Comunes
Imports LMDataAccessLayer
Imports System.Net.Mail
Imports System.Text
Imports System.IO

Public Class NovedadesProduccion

#Region "Atributos"
    Private _idUsuario As Integer
    Private _mensaje As String
    Private _resultado As Integer
    Private _ordenRecepcion As String
    Private _ordenCompra As String
    Private _guia As String
    Private _idOrdenCompra As Integer
    Private _factura As String
    Private _idProducto As Integer
    Private _idMaterial As String
    Private _idSubproducto As Integer
    Private _fechaFacturaInicial As Date
    Private _fechaFacturaFinal As Date
    Private _fechaNovedadInicial As Date
    Private _fechaNovedadFinal As Date
    Private _listImagenes As List(Of ImagenProducto)
    Private _listArchivo As List(Of Archivo)
    Private _novedad As String
    Private _idNovedad As Integer
    Private _idDetalleNovedad As Integer
    Private _gestion As String
    Private _rutaArchivo As String
    Private _nombreArchivo As String
    Private _fechaFactura As Date
    Private _dtArchivo As DataTable
    Private _extencion As String
    Private _origen As String
#End Region

#Region "Propiedades"

    Public Property Resultado() As Integer
        Get
            Return _resultado
        End Get
        Set(value As Integer)
            _resultado = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property Mensaje() As String
        Get
            Return _mensaje
        End Get
        Set(value As String)
            _mensaje = value
        End Set
    End Property

    Public Property OrdenRecepcion() As String
        Get
            Return _ordenRecepcion
        End Get
        Set(value As String)
            _ordenRecepcion = value
        End Set
    End Property

    Public Property OrdenCompra() As String
        Get
            Return _ordenCompra
        End Get
        Set(value As String)
            _ordenCompra = value
        End Set
    End Property

    Public Property Guia() As String
        Get
            Return _guia
        End Get
        Set(value As String)
            _guia = value
        End Set
    End Property

    Public Property IdOrdenCompra() As Integer
        Get
            Return _idOrdenCompra
        End Get
        Set(value As Integer)
            _idOrdenCompra = value
        End Set
    End Property

    Public Property Factura() As String
        Get
            Return _factura
        End Get
        Set(value As String)
            _factura = value
        End Set
    End Property

    Public Property IdProducto() As Integer
        Get
            Return _idProducto
        End Get
        Set(value As Integer)
            _idProducto = value
        End Set
    End Property

    Public Property IdSubproducto() As Integer
        Get
            Return _idSubproducto
        End Get
        Set(value As Integer)
            _idSubproducto = value
        End Set
    End Property

    Public Property FechaFacturaInicial() As Date
        Get
            Return _fechaFacturaInicial
        End Get
        Set(value As Date)
            _fechaFacturaInicial = value
        End Set
    End Property

    Public Property FechaFacturaFinal() As Date
        Get
            Return _fechaFacturaFinal
        End Get
        Set(value As Date)
            _fechaFacturaFinal = value
        End Set
    End Property

    Public Property FechaNovedadInicial() As Date
        Get
            Return _fechaNovedadInicial
        End Get
        Set(value As Date)
            _fechaNovedadFinal = value
        End Set
    End Property

    Public Property FechaNovedadFinal() As Date
        Get
            Return _fechaNovedadFinal
        End Get
        Set(value As Date)
            _fechaNovedadFinal = value
        End Set
    End Property

    Public Property ListaImagenes As List(Of ImagenProducto)
        Get
            Return _listImagenes
        End Get
        Set(value As List(Of ImagenProducto))
            _listImagenes = value
        End Set
    End Property

    Public Property ListaArchivos As List(Of Archivo)
        Get
            Return _listArchivo
        End Get
        Set(value As List(Of Archivo))
            _listArchivo = value
        End Set
    End Property

    Public Property Novedad() As String
        Get
            Return _novedad
        End Get
        Set(value As String)
            _novedad = value
        End Set
    End Property

    Public Property IdNovedad() As Integer
        Get
            Return _idNovedad
        End Get
        Set(value As Integer)
            _idNovedad = value
        End Set
    End Property

    Public Property IdDetalleNovedad() As Integer
        Get
            Return _idDetalleNovedad
        End Get
        Set(value As Integer)
            _idDetalleNovedad = value
        End Set
    End Property

    Public Property Gestion() As String
        Get
            Return _gestion
        End Get
        Set(value As String)
            _gestion = value
        End Set
    End Property

    Public Property RutaArchivo() As String
        Get
            Return _rutaArchivo
        End Get
        Set(value As String)
            _rutaArchivo = value
        End Set
    End Property

    Public Property NombreArchivo() As String
        Get
            Return _nombreArchivo
        End Get
        Set(value As String)
            _nombreArchivo = value
        End Set
    End Property

    Public Property FechaFactura() As Date
        Get
            Return _fechaFactura
        End Get
        Set(value As Date)
            _fechaFactura = value
        End Set
    End Property

    Public Property IdMaterial() As String
        Get
            Return _idMaterial
        End Get
        Set(value As String)
            _idMaterial = value
        End Set
    End Property

    Public Property DtArchivo() As DataTable
        Get
            Return _dtArchivo
        End Get
        Set(value As DataTable)
            _dtArchivo = value
        End Set
    End Property

    Public Property Extencion() As String
        Get
            Return _extencion
        End Get
        Set(value As String)
            _extencion = value
        End Set
    End Property

    Public Property Origen() As String
        Get
            Return _origen
        End Get
        Set(value As String)
            _origen = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Metodos Privados"

#End Region

#Region "Metodos Publicos"

    Public Function ObtenerNovedades() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                If _ordenRecepcion <> Nothing Then .SqlParametros.Add("@OrdenRecepcion", SqlDbType.VarChar).Value = _ordenRecepcion
                If _guia <> Nothing Then .SqlParametros.Add("@Guia", SqlDbType.VarChar).Value = _guia
                If _factura <> Nothing Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If _idSubproducto > 0 Then .SqlParametros.Add("@idSubProducto", SqlDbType.Int).Value = _idSubproducto
                If _fechaFacturaInicial <> Date.MinValue Then .SqlParametros.Add("@FechaFacturaInicial", SqlDbType.DateTime).Value = _fechaFacturaInicial
                If _fechaFacturaFinal <> Date.MinValue Then .SqlParametros.Add("@FechaFacturaFinal", SqlDbType.DateTime).Value = _fechaFacturaFinal
                If _fechaNovedadInicial <> Date.MinValue Then .SqlParametros.Add("@FechaNovedadInicial", SqlDbType.DateTime).Value = _fechaNovedadInicial
                If _fechaNovedadFinal <> Date.MinValue Then .SqlParametros.Add("@FechaNovedadFinal", SqlDbType.DateTime).Value = _fechaNovedadFinal
                dtResultado = .ejecutarDataTable("ObtenerNovedadesProduccion", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Function RegistrarDetalleNovedad() As Short
        Dim respuesta As Integer = -1
        Dim dbManager As New LMDataAccess
        Try
            If dbManager IsNot Nothing Then
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                    .SqlParametros.Add("@novedad", SqlDbType.VarChar).Value = _novedad
                    .SqlParametros.Add("@nombreArchivo", SqlDbType.VarChar).Value = _nombreArchivo
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("RegistrarDetalleNovedadProduccion", CommandType.StoredProcedure)
                    respuesta = CShort(.SqlParametros("@returnValue").Value)
                End With
            End If
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return respuesta
    End Function

    Public Function ObtenerHistoricoNovedades() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                If _idOrdenCompra > 0 Then .SqlParametros.Add("@idOrdenCompra", SqlDbType.Int).Value = _idOrdenCompra
                If _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If _idNovedad > 0 Then .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                dtResultado = .ejecutarDataTable("ObtenerHistoricoNovedadesProduccion", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Function ObtenerArchivoNovedades() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                If _idNovedad > 0 Then .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                If _idDetalleNovedad > 0 Then .SqlParametros.Add("@idDetalleNovedad", SqlDbType.Int).Value = _idDetalleNovedad
                dtResultado = .ejecutarDataTable("ObtenerArchivoNovedadesProduccion", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Function RegistrarGestionNovedad() As Short
        Dim respuesta As Integer = -1
        Dim dbManager As New LMDataAccess
        Try
            If dbManager IsNot Nothing Then
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idDetalleNovedad", SqlDbType.Int).Value = _idDetalleNovedad
                    .SqlParametros.Add("@gestion", SqlDbType.VarChar).Value = _gestion
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("RegistrarGestionNovedadesProduccion", CommandType.StoredProcedure)
                    respuesta = CShort(.SqlParametros("@returnValue").Value)
                End With
            End If
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return respuesta
    End Function

    Public Function CargarImagenes()
        Dim dt As DataTable
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .TiempoEsperaComando = 1200
                    .SqlParametros.Add("@idDetalleNovedad", SqlDbType.Int).Value = _idDetalleNovedad
                    dt = .ejecutarDataTable("ObtenerImagenesNovedad", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return dt
    End Function

    Public Function CargarArchivos()
        Using dbManager As New LMDataAccess
            _listArchivo = New List(Of Archivo)
            Try
                With dbManager
                    .TiempoEsperaComando = 1200
                    .SqlParametros.Add("@idDetalleNovedad", SqlDbType.Int).Value = _idDetalleNovedad
                    .ejecutarReader("ObtenerArchivosNovedad", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        While .Reader.Read
                            Dim objArchivo As Archivo
                            objArchivo.nombreArchivo = .Reader("nombreArchivo")
                            objArchivo.idDetalleNovedad = .Reader("idDetalleNovedad")
                            _listArchivo.Add(objArchivo)
                        End While
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return _listArchivo
    End Function

    Public Function ObtenerNovedadesExportar() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                If _ordenRecepcion <> Nothing Then .SqlParametros.Add("@OrdenRecepcion", SqlDbType.VarChar).Value = _ordenRecepcion
                If _guia <> Nothing Then .SqlParametros.Add("@Guia", SqlDbType.VarChar).Value = _guia
                If _factura <> Nothing Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                If _idSubproducto > 0 Then .SqlParametros.Add("@idSubProducto", SqlDbType.Int).Value = _idSubproducto
                If _fechaFacturaInicial <> Date.MinValue Then .SqlParametros.Add("@FechaFacturaInicial", SqlDbType.DateTime).Value = _fechaFacturaInicial
                If _fechaFacturaFinal <> Date.MinValue Then .SqlParametros.Add("@FechaFacturaFinal", SqlDbType.DateTime).Value = _fechaFacturaFinal
                If _fechaNovedadInicial <> Date.MinValue Then .SqlParametros.Add("@FechaNovedadInicial", SqlDbType.DateTime).Value = _fechaNovedadInicial
                If _fechaNovedadFinal <> Date.MinValue Then .SqlParametros.Add("@FechaNovedadFinal", SqlDbType.DateTime).Value = _fechaNovedadFinal
                dtResultado = .ejecutarDataTable("ObtenerNovedadesProduccionExportables", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Function RegistrarNovedadTotal() As Short
        Dim respuesta As Integer = -2
        Dim dbManager As New LMDataAccess
        Try
            If dbManager IsNot Nothing Then
                With dbManager
                    .iniciarTransaccion()
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@ordenRecepcion", SqlDbType.Decimal).Value = _ordenRecepcion
                    .SqlParametros.Add("@ordenCompra", SqlDbType.Decimal).Value = _ordenCompra
                    .SqlParametros.Add("@guia", SqlDbType.VarChar).Value = _guia
                    If _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                    If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Decimal).Value = _idProducto
                    If _idSubproducto <> 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Decimal).Value = _idSubproducto
                    If _fechaFactura <> Date.MinValue Then .SqlParametros.Add("@fechaFactura", SqlDbType.SmallDateTime).Value = _fechaFactura
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@novedad", SqlDbType.VarChar).Value = _novedad
                    .SqlParametros.Add("@nombreArchivo", SqlDbType.VarChar).Value = _nombreArchivo
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("RegistrarNovedadProduccion", CommandType.StoredProcedure)
                    respuesta = CShort(.SqlParametros("@returnValue").Value)
                    If respuesta = 0 Then
                        .confirmarTransaccion()
                        If _origen = "escritorio" Then
                            Dim dtNotusWs As DataTable
                            dtNotusWs = ObtenerConfigWSNotus("UrlWebServiceNotus")
                            Dim dtUrl As DataTable
                            dtUrl = ObtenerConfigWSNotus("RutaNovedadesProduccion")
                            'Dim _objService As New NotusService.NotusService
                            '_objService.Url = dtNotusWs.Rows(0).Item("configKeyValue")
                            'Dim urlNotus As String = dtUrl.Rows(0).Item("configKeyValue")
                            'Dim objResp = _objService.RegistrarArchivoNovedad(File.ReadAllBytes(_dtArchivo.Rows(0).Item("url")), _dtArchivo.Rows(0).Item("nombre"), urlNotus)
                        End If
                    Else
                        .abortarTransaccion()
                    End If
                End With
            End If
        Catch ex As Exception
            Throw ex
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return respuesta
    End Function

    Public Function ObtenerConfigWSNotus(ByVal valor As String) As DataTable
        Dim dt As DataTable
        Dim dbmanager As New LMDataAccess
        Try
            With dbmanager
                .SqlParametros.Clear()
                .SqlParametros.Add("@configKeyName", SqlDbType.VarChar).Value = valor
                dt = .ejecutarDataTable("ObtenerInfoConfigValues", CommandType.StoredProcedure)
            End With
        Finally
            If dbmanager IsNot Nothing Then dbmanager.Dispose()
        End Try
        Return dt
    End Function

    Public Function ObtenerContentTypeTTT() As DataTable
        Dim dtResultado As DataTable
        Dim dbmanager As New LMDataAccess
        Try
            With dbmanager
                .SqlParametros.Clear()
                If _extencion <> "" Then .SqlParametros.Add("@extencion", SqlDbType.VarChar).Value = _extencion
                dtResultado = .ejecutarDataTable("ObtenerContentType", CommandType.StoredProcedure)
            End With
        Finally
            If dbmanager IsNot Nothing Then dbmanager.Dispose()
        End Try
        Return dtResultado
    End Function

#End Region

#Region "Estructuras"

    Public Structure ImagenProducto
        Dim imagen As Byte()
        Dim contenType As String
        Dim nombreArchivo As String
        Dim tamanio As Integer
    End Structure

    Public Structure Archivo
        Dim idDetalleNovedad As Integer
        Dim nombreArchivo As String
    End Structure

    Public Structure InformacionArchivo
        Dim ruta As String
        Dim nombre As String
    End Structure

#End Region

End Class
