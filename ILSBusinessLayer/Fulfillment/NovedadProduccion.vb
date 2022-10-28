Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Enumerados
Imports ILSBusinessLayer.Comunes
Imports System.IO

Public Class NovedadProduccion

#Region "Atributos"

    Private _idNovedad As Integer
    Private _idFacturaGuia As Long
    Private _factura As String
    Private _guia As String
    Private _idOrdenRecepcion As Long
    Private _ordenCompra As String
    Private _idProducto As Long
    Private _idSubproducto As Long
    Private _contentType As String
    Private _descripcion As String
    Private _fechaRegistro As Date
    Private _fechaRecepcion As Date
    Private _idUsuarioRegistra As Integer
    Private _usuarioRegistra As String
    Private _fechaSolucion As Date
    Private _idUsuarioSoluciona As Integer
    Private _usuarioSoluciona As String
    Private _soportes As SoporteNovedadProduccionColeccion
    Private _fechaFacturaInicial As Date
    Private _fechaFacturaFinal As Date
    Private _fechaNovedadInicial As Date
    Private _fechaNovedadFinal As Date
    Private _registrado As Boolean
    Private _gestionNovedad As String
    Private _fechaGestion As Date
    Private _usuarioGestion As String

#End Region

#Region "Constructores"

    Public Sub New()
    End Sub

    Public Sub New(ByVal identificador As Integer)
        _idNovedad = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdNovedad As Integer
        Get
            Return _idNovedad
        End Get
        Set(ByVal value As Integer)
            _idNovedad = value
        End Set
    End Property

    Public Property IdFacturaGuia As Long
        Get
            Return _idFacturaGuia
        End Get
        Set(ByVal value As Long)
            _idFacturaGuia = value
        End Set
    End Property

    Public Property Factura As String
        Get
            Return _factura
        End Get
        Set(value As String)
            _factura = value
        End Set
    End Property

    Public Property Guia As String
        Get
            Return _guia
        End Get
        Set(value As String)
            _guia = value
        End Set
    End Property

    Public Property IdOrdenRecepcion As Long
        Get
            Return _idOrdenRecepcion
        End Get
        Set(ByVal value As Long)
            _idOrdenRecepcion = value
        End Set
    End Property

    Public Property OrdenCompra As String
        Get
            Return _ordenCompra
        End Get
        Set(value As String)
            _ordenCompra = value
        End Set
    End Property

    Public Property IdProducto As Long
        Get
            Return _idProducto
        End Get
        Set(value As Long)
            _idProducto = value
        End Set
    End Property

    Public Property IdSubproducto As Long
        Get
            Return _idSubproducto
        End Get
        Set(value As Long)
            _idSubproducto = value
        End Set
    End Property

    Public Property ContentType As String
        Get
            Return _contentType
        End Get
        Set(value As String)
            _contentType = value
        End Set
    End Property

    Public Property Descripcion As String
        Get
            Return _Descripcion
        End Get
        Set(ByVal value As String)
            _Descripcion = value
        End Set
    End Property

    Public Property FechaRegistro As Date
        Get
            Return _FechaRegistro
        End Get
        Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property FechaRecepcion As Date
        Get
            Return _fechaRecepcion
        End Get
        Set(value As Date)
            _fechaRecepcion = value
        End Set
    End Property

    Public Property IdUsuarioRegistra As Integer
        Get
            Return _IdUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _IdUsuarioRegistra = value
        End Set
    End Property

    Public Property UsuarioRegistra As String
        Get
            Return _usuarioRegistra
        End Get
        Set(value As String)
            _usuarioRegistra = value
        End Set
    End Property

    Public Property FechaSolucion As Date
        Get
            Return _FechaSolucion
        End Get
        Set(ByVal value As Date)
            _FechaSolucion = value
        End Set
    End Property

    Public Property UsuarioSoluciona As String
        Get
            Return _usuarioSoluciona
        End Get
        Set(value As String)
            _usuarioSoluciona = value
        End Set
    End Property

    Public Property IdUsuarioSoluciona As Integer
        Get
            Return _IdUsuarioSoluciona
        End Get
        Set(ByVal value As Integer)
            _IdUsuarioSoluciona = value
        End Set
    End Property

    Public ReadOnly Property Soportes As SoporteNovedadProduccionColeccion
        Get
            If _soportes Is Nothing OrElse Not _soportes.Cargado Then
                If _idNovedad > 0 Then
                    _soportes = New SoporteNovedadProduccionColeccion(_idNovedad)
                Else
                    _soportes = New SoporteNovedadProduccionColeccion()
                End If
            End If
            Return _soportes
        End Get
    End Property

    Public Property FechaFacturaInicial As Date
        Get
            Return _fechaFacturaInicial
        End Get
        Set(value As Date)
            _fechaFacturaFinal = value
        End Set
    End Property

    Public Property FechaFacturaFinal As Date
        Get
            Return _fechaFacturaFinal
        End Get
        Set(value As Date)
            _fechaFacturaFinal = value
        End Set
    End Property

    Public Property FechaNovedadInicial As Date
        Get
            Return _fechaNovedadInicial
        End Get
        Set(value As Date)
            _fechaNovedadInicial = value
        End Set
    End Property

    Public Property FechaNovedadFinal As Date
        Get
            Return _fechaNovedadFinal
        End Get
        Set(value As Date)
            _fechaNovedadFinal = value
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

    Public Property GestionNovedad As String
        Get
            Return _gestionNovedad
        End Get
        Set(value As String)
            _gestionNovedad = value
        End Set
    End Property

    Public Property FechaGestion As Date
        Get
            Return _fechaGestion
        End Get
        Set(value As Date)
            _fechaGestion = value
        End Set
    End Property

    Public Property UsuarioGestion As String
        Get
            Return _usuarioGestion
        End Get
        Set(value As String)
            _usuarioGestion = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarInformacion()
        If _idNovedad > 0 Then
            Using dbManager As New LMDataAccess
                With dbManager
                    If _idNovedad > 0 Then .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                    If _idOrdenRecepcion > 0 Then .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = _idOrdenRecepcion
                    If _guia <> Nothing Then .SqlParametros.Add("@guia", SqlDbType.Int).Value = _guia
                    If _factura <> Nothing Then .SqlParametros.Add("@factura", SqlDbType.Int).Value = _factura
                    If _idProducto > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = _idProducto
                    If IdSubproducto > 0 Then .SqlParametros.Add("@idSubproducto", SqlDbType.Int).Value = _idSubproducto
                    If _fechaFacturaInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInicialFactura", SqlDbType.Int).Value = _fechaFacturaInicial
                    If _fechaFacturaFinal <> Date.MinValue Then .SqlParametros.Add("@fechaFinalFactura", SqlDbType.Int).Value = _fechaFacturaFinal
                    If _fechaNovedadInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInicialNovedad", SqlDbType.Int).Value = _fechaNovedadInicial
                    If _fechaNovedadFinal <> Date.MinValue Then .SqlParametros.Add("@fechaFinalNovedad", SqlDbType.Int).Value = _fechaNovedadFinal
                    .ejecutarReader("ObtenerInformacionDeNovedadDeProduccion", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarValorDePropiedades(.Reader)
                        End If
                        .Reader.Close()
                    End If
                End With
            End Using
        End If
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarValorDePropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing AndAlso reader.HasRows Then
            Integer.TryParse(reader("idNovedad").ToString, _idNovedad)
            Long.TryParse(reader("idFacturaGuia").ToString, _idFacturaGuia)
            Long.TryParse(reader("idOrdenRecepcion").ToString, _idOrdenRecepcion)
            _descripcion = reader("descripcion").ToString
            Date.TryParse(reader("fechaRegistro").ToString, _fechaRegistro)
            Integer.TryParse(reader("idUsuarioRegistra").ToString, _idUsuarioRegistra)
            _usuarioRegistra = reader("usuarioRegistra").ToString
            _factura = reader("factura").ToString
            _guia = reader("guia").ToString
            _ordenCompra = reader("ordenCompra").ToString
            Date.TryParse(reader("fechaSolucion").ToString, _fechaSolucion)
            Integer.TryParse(reader("idUsuarioSoluciona").ToString, _idUsuarioSoluciona)
            _usuarioSoluciona = reader("usuarioSoluciona").ToString
            _gestionNovedad = reader("gestionNovedad").ToString
            Date.TryParse(reader("fechaGestion").ToString, _fechaGestion)
            _usuarioGestion = reader("usuarioGestion").ToString
            _registrado = True
        End If
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso(-1, "Registro no creado")
        If _idNovedad = 0 AndAlso (_idFacturaGuia > 0 Or _idOrdenRecepcion > 0) AndAlso Not EsNuloOVacio(_descripcion) AndAlso _idUsuarioRegistra > 0 _
            AndAlso _soportes IsNot Nothing AndAlso _soportes.Count > 0 Then
            Dim dt As DataTable = _soportes.GenerarDataTable()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idFacturaGuia > 0 Then .SqlParametros.Add("@idFacturaGuia", SqlDbType.Int).Value = _idFacturaGuia
                        If _idOrdenRecepcion > 0 Then .SqlParametros.Add("@idOrdenRecepcion", SqlDbType.BigInt).Value = _idOrdenRecepcion
                        .SqlParametros.Add("descripcion", SqlDbType.VarChar, 2000).Value = _descripcion.Trim
                        .SqlParametros.Add("idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@idNovedad", SqlDbType.Int).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .TiempoEsperaComando = 200
                        .ejecutarNonQuery("RegistrarNovedadDeProduccion", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            If resultado.Valor = 0 Then
                                Integer.TryParse(.SqlParametros("@idNovedad").Value.ToString, _idNovedad)
                                For Each dr As DataRow In dt.Rows
                                    dr("idNovedad") = _idNovedad
                                    dr("idUsuarioRegistra") = _idUsuarioRegistra
                                Next
                                .SqlParametros.Clear()
                                .inicilizarBulkCopy()
                                With .BulkCopy
                                    .DestinationTableName = "SoporteNovedadProduccion"
                                    .ColumnMappings.Add("idNovedad", "idNovedad")
                                    .ColumnMappings.Add("nombreOriginal", "nombreOriginal")
                                    .ColumnMappings.Add("rutaCompleta", "rutaCompleta")
                                    .ColumnMappings.Add("datosBinarios", "datosBinarios")
                                    .ColumnMappings.Add("contentType", "contentType")
                                    .ColumnMappings.Add("idTipoSoporte", "idTipoSoporte")
                                    .ColumnMappings.Add("idUsuarioRegistra", "idUsuarioRegistra")
                                    .WriteToServer(dt)
                                End With
                                If .estadoTransaccional Then .confirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, "La novedad fue registrada satisfactoriamente.")
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                            End If
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            resultado.Mensaje = "No se pudo evaluar el resultado de registro arrojado por la base de  datos. Por favor intente nuevamente."
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                    resultado.EstablecerMensajeYValor(500, "Error al registrar la novedad: " & ex.Message)
                End Try
            End Using
        Else
            resultado.EstablecerMensajeYValor(300, "No se han proporcionado los valores de todos los parámetros obligatorios. Por favor verifique")
        End If

        Return resultado
    End Function

    Public Function ObtenerNovedadesExportar() As DataTable
        Dim dtResultado As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Clear()
                If _idOrdenRecepcion <> Nothing Then .SqlParametros.Add("@OrdenRecepcion", SqlDbType.VarChar).Value = _idOrdenRecepcion
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

#End Region

End Class