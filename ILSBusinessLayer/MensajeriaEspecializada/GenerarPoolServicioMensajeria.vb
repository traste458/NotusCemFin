Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados

Public Class GenerarPoolServicioMensajeria

#Region "Atributos (Campos)"

    Private _idServicioMensajeria As String
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _fechaCreacionInicial As Date
    Private _fechaCreacionFinal As Date
    Private _idCiudad As Integer
    Private _nombreCiudad As String
    Private _idBodega As Integer
    Private _idTipoServicio As Integer
    Private _idEstado As Integer
    Private _listaEstado As ArrayList
    Private _numeroRadicado As Integer
    Private _listNumeroRadicado As ArrayList
    Private _idUsuarioGenerador As Integer
    Private _tieneNovedad As Enumerados.EstadoBinario
    Private _urgente As Enumerados.EstadoBinario
    Private _clienteVIP As Enumerados.EstadoBinario
    Private _disponibleAutomarcado As Enumerados.EstadoBinario
    Private _dbManager As New LMDataAccess
    Private _listIdServicio As ArrayList
    Private _idRadicado As Integer
    Private _nombreDocumento As String
    Private _byteDocumento As String
    Private _rutaDocumento As String
    Private _idCausalDevolucion As Integer
    Private _observacionesDevolucionDocs As String
    Private _docRecuperacion As Boolean
    Private _consultaInfRad As Boolean
    Private _dtInformacionRadicado As New DataTable()
    Private _idClienteExterno As Integer
    Private _precinto As String
    Private _planillaGenerada As Integer
    Private _pagare As Integer
    Private _campania As String
    Private _codEstrategia As String
    Private _oficina As Integer
    Private _validarPasoDestruccion As Boolean
    Private _encabezadoPlanilla As String
    Private _cuerpoPlanilla As String
    Private _numEvidencias As Integer
    Private _codOficinaCliente As String
    Private _splitServicios As String
    Private _splitCausales As String

#End Region

#Region "Propiedade"

    Public Property IdServicioMensajeria() As String
        Get
            Return _idServicioMensajeria
        End Get
        Set(ByVal value As String)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property NumeroRadicado() As Integer
        Get
            Return _numeroRadicado
        End Get
        Set(ByVal value As Integer)
            _numeroRadicado = value
        End Set
    End Property

    Public Property ListaNumeroRadicado() As ArrayList
        Get
            If _listNumeroRadicado Is Nothing Then _listNumeroRadicado = New ArrayList
            Return _listNumeroRadicado
        End Get
        Set(ByVal value As ArrayList)
            _listNumeroRadicado = value
        End Set
    End Property

    Public Property FechaInicial() As Date
        Get
            Return _fechaInicial
        End Get
        Set(ByVal value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal() As Date
        Get
            Return _fechaFinal
        End Get
        Set(ByVal value As Date)
            _fechaFinal = value
        End Set
    End Property

    Public Property FechaCreacionInicial() As Date
        Get
            Return _fechaCreacionInicial
        End Get
        Set(ByVal value As Date)
            _fechaCreacionInicial = value
        End Set
    End Property

    Public Property FechaCreacionFinal() As Date
        Get
            Return _fechaCreacionFinal
        End Get
        Set(ByVal value As Date)
            _fechaCreacionFinal = value
        End Set
    End Property

    Public Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property NombreCiudad As String
        Get
            Return _nombreCiudad
        End Get
        Set(ByVal value As String)
            _nombreCiudad = value
        End Set
    End Property

    Public Property IdBodega() As Integer
        Get
            Return _idBodega
        End Get
        Set(ByVal value As Integer)
            _idBodega = value
        End Set
    End Property

    Public Property IdTipoServicio() As Integer
        Get
            Return _idTipoServicio
        End Get
        Set(ByVal value As Integer)
            _idTipoServicio = value
        End Set
    End Property

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property ListaEstado() As ArrayList
        Get
            If _listaEstado Is Nothing Then _listaEstado = New ArrayList
            Return _listaEstado
        End Get
        Set(ByVal value As ArrayList)
            _listaEstado = value
        End Set
    End Property

    Public Property IdUsuarioGenerador() As Integer
        Get
            Return _idUsuarioGenerador
        End Get
        Set(ByVal value As Integer)
            _idUsuarioGenerador = value
        End Set
    End Property

    Public Property TieneNovedad() As Enumerados.EstadoBinario
        Get
            Return _tieneNovedad
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _tieneNovedad = value
        End Set
    End Property

    Public Property Urgente() As Enumerados.EstadoBinario
        Get
            Return _urgente
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _urgente = value
        End Set
    End Property

    Public Property ClienteVIP As Enumerados.EstadoBinario
        Get
            Return _clienteVIP
        End Get
        Set(value As Enumerados.EstadoBinario)
            _clienteVIP = value
        End Set
    End Property

    Public Property DisponibleAutomarcado() As Enumerados.EstadoBinario
        Get
            Return _disponibleAutomarcado
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _disponibleAutomarcado = value
        End Set
    End Property

    Public Property ListaIdServicio As ArrayList
        Get
            If _listIdServicio Is Nothing Then _listIdServicio = New ArrayList
            Return _listIdServicio
        End Get
        Set(value As ArrayList)
            _listIdServicio = value
        End Set
    End Property
    Public Property Identificaion As String
    Public Property SeudoCodigo As String
    Public Property Msisdn As String

    Public Property NombreDocumento As String
        Get
            Return _nombreDocumento
        End Get
        Set(value As String)
            _nombreDocumento = value
        End Set
    End Property

    Public Property ByteDocumento As String
        Get
            Return _byteDocumento
        End Get
        Set(value As String)
            _byteDocumento = value
        End Set
    End Property

    Public Property RutaDocumento As String
        Get
            Return _rutaDocumento
        End Get
        Set(value As String)
            _rutaDocumento = value
        End Set
    End Property

    Public Property IdRadicado As Integer
        Get
            Return _idRadicado
        End Get
        Set(value As Integer)
            _idRadicado = value
        End Set
    End Property

    Public Property IdCausalDevolucion As Integer
        Get
            Return _idCausalDevolucion
        End Get
        Set(value As Integer)
            _idCausalDevolucion = value
        End Set
    End Property

    Public Property ObservacionesDevolucionDocs As String
        Get
            Return _observacionesDevolucionDocs
        End Get
        Set(value As String)
            _observacionesDevolucionDocs = value
        End Set
    End Property

    Public Property DocRecuperacion As Boolean
        Get
            Return _docRecuperacion
        End Get
        Set(value As Boolean)
            _docRecuperacion = value
        End Set
    End Property

    Public Property ConsultaInfRad As Boolean
        Get
            Return _consultaInfRad
        End Get
        Set(value As Boolean)
            _consultaInfRad = value
        End Set
    End Property

    Public Property DtInformacionRadicado As DataTable
        Get
            Return _dtInformacionRadicado
        End Get
        Set(value As DataTable)
            _dtInformacionRadicado = value
        End Set
    End Property

    Public Property IdClienteExterno As Integer
        Get
            Return _idClienteExterno
        End Get
        Set(value As Integer)
            _idClienteExterno = value
        End Set
    End Property

    Public Property Precinto As String
        Get
            Return _precinto
        End Get
        Set(value As String)
            _precinto = value
        End Set
    End Property

    Public Property PlanillaGenerada As Integer
        Get
            Return _planillaGenerada
        End Get
        Set(value As Integer)
            _planillaGenerada = value
        End Set
    End Property

    Public Property Pagare As Integer
        Get
            Return _pagare
        End Get
        Set(value As Integer)
            _pagare = value
        End Set
    End Property

    Public Property Campania As String
        Get
            Return _campania
        End Get
        Set(value As String)
            _campania = value
        End Set
    End Property

    Public Property CodEstrategia As String
        Get
            Return _codEstrategia
        End Get
        Set(value As String)
            _codEstrategia = value
        End Set
    End Property

    Public Property Oficina As Integer
        Get
            Return _oficina
        End Get
        Set(value As Integer)
            _oficina = value
        End Set
    End Property

    Public Property ValidarPasoDestruccion As Boolean
        Get
            Return _validarPasoDestruccion
        End Get
        Set(value As Boolean)
            _validarPasoDestruccion = value
        End Set
    End Property

    Public Property EncabezadoPlanilla As String
        Get
            Return _encabezadoPlanilla
        End Get
        Set(value As String)
            _encabezadoPlanilla = value
        End Set
    End Property

    Public Property CuerpoPlanilla As String
        Get
            Return _cuerpoPlanilla
        End Get
        Set(value As String)
            _cuerpoPlanilla = value
        End Set
    End Property

    Public Property NumEvidencias As Integer
        Get
            Return _numEvidencias
        End Get
        Set(value As Integer)
            _numEvidencias = value
        End Set
    End Property

    Public Property CodOficinaCliente As String
        Get
            Return _codOficinaCliente
        End Get
        Set(value As String)
            _codOficinaCliente = value
        End Set
    End Property

    Public Property SplitServicios As String
        Get
            Return _splitServicios
        End Get
        Set(value As String)
            _splitServicios = value
        End Set
    End Property

    Public Property SplitCausales As String
        Get
            Return _splitCausales
        End Get
        Set(value As String)
            _splitCausales = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _tieneNovedad = Enumerados.EstadoBinario.NoEstablecido
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function GenerarPool() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                    If _msisdn > 0 Then .Add("@msisdn", SqlDbType.BigInt).Value = _msisdn
                    If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.Int).Value = _numeroRadicado
                    If _listNumeroRadicado IsNot Nothing AndAlso _listNumeroRadicado.Count > 0 Then .Add("@listaNumeroRadicado", SqlDbType.VarChar).Value = Join(_listNumeroRadicado.ToArray, ",")
                    If _listIdServicio IsNot Nothing AndAlso _listIdServicio.Count > 0 Then .Add("@listaIdServicio", SqlDbType.VarChar).Value = Join(_listIdServicio.ToArray, ",")
                    If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                        .Add("@fechaInicial", SqlDbType.DateTime).Value = _fechaInicial
                        .Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
                    End If
                    If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                    If _idTipoServicio > 0 Then .Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                    If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    If _listaEstado IsNot Nothing AndAlso _listaEstado.Count > 0 Then .Add("@listaEstado", SqlDbType.VarChar).Value = Join(_listaEstado.ToArray, ",")
                    If _idUsuarioGenerador > 0 Then .Add("@idUsuarioGenerador", SqlDbType.Int).Value = _idUsuarioGenerador
                    If _tieneNovedad <> EstadoBinario.NoEstablecido Then .Add("@tieneNovedad", SqlDbType.Bit).Value = IIf(_tieneNovedad = EstadoBinario.Activo, 1, 0)
                    If _urgente <> EstadoBinario.NoEstablecido Then .Add("@urgente", SqlDbType.Bit).Value = IIf(_urgente = EstadoBinario.Activo, 1, 0)
                    If _clienteVIP <> EstadoBinario.NoEstablecido Then .Add("@clienteVIP", SqlDbType.Bit).Value = IIf(_clienteVIP = EstadoBinario.Activo, 1, 0)
                    If _disponibleAutomarcado <> EstadoBinario.NoEstablecido Then .Add("@disponibleAutomarcado", SqlDbType.Bit).Value = IIf(_disponibleAutomarcado = EstadoBinario.Activo, 1, 0)
                End With
                dtDatos = .EjecutarDataTable("ObtenerInformacionGeneralServicioMensajeria", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw ex
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function
    Public Function GenerarPoolnew() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                    If Msisdn <> "" And Msisdn IsNot Nothing Then .Add("@msisdn", SqlDbType.VarChar).Value = Msisdn
                    If Identificaion <> "" And Identificaion IsNot Nothing Then .Add("@Identificaion", SqlDbType.VarChar).Value = Identificaion
                    If SeudoCodigo <> "" And SeudoCodigo IsNot Nothing Then .Add("@serial", SqlDbType.VarChar).Value = SeudoCodigo
                    If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.Int).Value = _numeroRadicado
                    If _listNumeroRadicado IsNot Nothing AndAlso _listNumeroRadicado.Count > 0 Then .Add("@listaNumeroRadicado", SqlDbType.VarChar).Value = Join(_listNumeroRadicado.ToArray, ",")
                    If _listIdServicio IsNot Nothing AndAlso _listIdServicio.Count > 0 Then .Add("@listaIdServicio", SqlDbType.VarChar).Value = Join(_listIdServicio.ToArray, ",")
                    If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                        .Add("@fechaInicial", SqlDbType.DateTime).Value = _fechaInicial
                        .Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
                    End If
                    If _fechaCreacionInicial > Date.MinValue AndAlso _fechaCreacionFinal > Date.MinValue Then
                        .Add("@fechaCreacionInicial", SqlDbType.DateTime).Value = _fechaCreacionInicial
                        .Add("@fechaCreacionFinal", SqlDbType.DateTime).Value = _fechaCreacionFinal
                    End If
                    If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                    If _idTipoServicio > 0 Then .Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                    If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    If _listaEstado IsNot Nothing AndAlso _listaEstado.Count > 0 Then .Add("@listaEstado", SqlDbType.VarChar).Value = Join(_listaEstado.ToArray, ",")
                    If _idUsuarioGenerador > 0 Then .Add("@idUsuarioGenerador", SqlDbType.Int).Value = _idUsuarioGenerador
                    If _tieneNovedad <> EstadoBinario.NoEstablecido Then .Add("@tieneNovedad", SqlDbType.Bit).Value = IIf(_tieneNovedad = EstadoBinario.Activo, 1, 0)
                    If _urgente <> EstadoBinario.NoEstablecido Then .Add("@urgente", SqlDbType.Bit).Value = IIf(_urgente = EstadoBinario.Activo, 1, 0)
                    If _clienteVIP <> EstadoBinario.NoEstablecido Then .Add("@clienteVIP", SqlDbType.Bit).Value = IIf(_clienteVIP = EstadoBinario.Activo, 1, 0)
                    If _disponibleAutomarcado <> EstadoBinario.NoEstablecido Then .Add("@disponibleAutomarcado", SqlDbType.Bit).Value = IIf(_disponibleAutomarcado = EstadoBinario.Activo, 1, 0)

                End With
                .TiempoEsperaComando = 0
                dtDatos = .EjecutarDataTable("ObtenerInformacionPoolServicioMensajeria", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw ex
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function GenerarPoolEliminarVisitasSimpliRoute(listaVisitas As DataTable) As DataSet
        Dim dsDatos As New DataSet
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If _idUsuarioGenerador > 0 Then .Add("@idUsuarioGenerador", SqlDbType.Int).Value = _idUsuarioGenerador
                    .AddWithValue("@tbIdVisita", listaVisitas)
                End With
                dsDatos = .EjecutarDataSet("ConsultarServiciosParaEliminarVisitaSimpliRoute", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw ex
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dsDatos
    End Function

    Public Function GenerarPoolneMesaControl() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                       If _listNumeroRadicado IsNot Nothing AndAlso _listNumeroRadicado.Count > 0 Then .Add("@listaNumeroRadicado", SqlDbType.VarChar).Value = Join(_listNumeroRadicado.ToArray, ",")
                    If _listIdServicio IsNot Nothing AndAlso _listIdServicio.Count > 0 Then .Add("@listaIdServicio", SqlDbType.VarChar).Value = Join(_listIdServicio.ToArray, ",")
                    If _fechaInicial > Date.MinValue AndAlso _fechaFinal > Date.MinValue Then
                        .Add("@fechaInicial", SqlDbType.DateTime).Value = _fechaInicial
                        .Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal
                    End If
                    If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    If _idUsuarioGenerador > 0 Then .Add("@idUsuarioGenerador", SqlDbType.Int).Value = _idUsuarioGenerador
                    If Identificaion <> "" And Identificaion IsNot Nothing Then .Add("@Identificaion", SqlDbType.VarChar).Value = Identificaion

                  
                End With
                .TiempoEsperaComando = 0
                dtDatos = .EjecutarDataTable("ObtenerInformacionPoolMesaControl", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw ex
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function
    Public Function GenerarPoolDisponibleAutomarcado() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If _idServicioMensajeria > 0 Then .Add("@idServicioMensajeria", SqlDbType.Int).Value = _idServicioMensajeria
                    If _numeroRadicado > 0 Then .Add("@numeroRadicado", SqlDbType.Int).Value = _numeroRadicado
                    If _fechaCreacionInicial > Date.MinValue AndAlso _fechaCreacionFinal > Date.MinValue Then
                        .Add("@fechaCreacionInicial", SqlDbType.DateTime).Value = _fechaCreacionInicial
                        .Add("@fechaCreacionFinal", SqlDbType.DateTime).Value = _fechaCreacionFinal
                    End If
                    If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                    If _tieneNovedad <> EstadoBinario.NoEstablecido Then .Add("@tieneNovedad", SqlDbType.Bit).Value = IIf(_tieneNovedad = EstadoBinario.Activo, 1, 0)
                    If _disponibleAutomarcado <> EstadoBinario.NoEstablecido Then .Add("@disponibleAutomarcado", SqlDbType.Bit).Value = IIf(_disponibleAutomarcado = EstadoBinario.Activo, 1, 0)
                End With
                dtDatos = .ejecutarDataTable("ObtenerInfoDisponibilidadAutomarcadoServicioMensajeria", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw ex
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ConsultaAutocomplete(Operacion As Integer, Filtro As String) As DataTable

        Dim dbManager As New LMDataAccess
        Dim dtDatosCodigoEstrategia As New DataTable
        With dbManager
            .SqlParametros.Add("@Operacion", SqlDbType.Int).Value = Operacion
            .SqlParametros.Add("@Filtro", SqlDbType.VarChar).Value = Filtro
            dtDatosCodigoEstrategia = .EjecutarDataTable("ConsultaAutoComplete", CommandType.StoredProcedure)
        End With
        Return dtDatosCodigoEstrategia
    End Function

    Public Function ActualizarEstadoServicioVisitaEliminadaSimpleRute(listaServicio As DataTable, listaVisitas As DataTable) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If _idUsuarioGenerador > 0 Then .Add("@idUsuarioGenerador", SqlDbType.Int).Value = _idUsuarioGenerador
                    .AddWithValue("@tbIdServicioMensajeria", listaServicio)
                    .AddWithValue("@tbIdVisita", listaVisitas)
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .EjecutarNonQuery("ActualizarEstadoServicioVisitaEliminadaSimpleRute", CommandType.StoredProcedure)
                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                Else
                    resultado.EstablecerMensajeYValor(300, "No se logró establecer la respuesta del servidor.")
                End If
            End With
        Catch ex As Exception
            Throw ex
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return resultado
    End Function


    Public Function GuardarDocumentoMC() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim mensaje As String
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                With .SqlParametros
                    .Add("@NumeroRadicado", SqlDbType.Int).Value = _idRadicado
                    If Not String.IsNullOrEmpty(_nombreDocumento) Then
                        .Add("@NombreDocumento", SqlDbType.VarChar).Value = _nombreDocumento
                    End If
                    If Not String.IsNullOrEmpty(_byteDocumento) Then
                        .Add("@ByteImagen", SqlDbType.VarChar).Value = _byteDocumento
                    End If
                    If Not String.IsNullOrEmpty(_rutaDocumento) Then
                        .Add("@RutaDocumento", SqlDbType.VarChar).Value = _rutaDocumento
                    End If
                    If Not String.IsNullOrEmpty(_observacionesDevolucionDocs) Then
                        .Add("@Observaciones", SqlDbType.VarChar).Value = _observacionesDevolucionDocs
                    End If
                    If _idUsuarioGenerador > 0 Then
                        .Add("@IdUsuarioMod", SqlDbType.Int).Value = _idUsuarioGenerador
                    End If
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("GuardarDocumentossMesaControl", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
        Catch ex As Exception
            mensaje = ex.ToString()
        End Try
        Return resultado
    End Function

    Public Function ConsultaDocumentosMC() As DataTable

        Dim dbManager As New LMDataAccess
        Dim dtDocMC As New DataTable
        With dbManager
            If Not String.IsNullOrEmpty(_nombreDocumento) Then
                .SqlParametros.Add("@NombreDocumento", SqlDbType.VarChar).Value = _nombreDocumento
            End If
            If _idRadicado > 0 Then
                .SqlParametros.Add("@NoRadicado", SqlDbType.Int).Value = _idRadicado
            End If
            dtDocMC = .EjecutarDataTable("ConsultarDocumentosMC", CommandType.StoredProcedure)
        End With
        Return dtDocMC
    End Function

    Public Function ConsultaCausalesRechazoMC() As DataSet

        Dim dbManager As New LMDataAccess
        Dim dsCausales As New DataSet
        With dbManager
            With .SqlParametros
                .Add("@IdServicio", SqlDbType.Int).Value = _idRadicado
            End With
            dsCausales = .EjecutarDataSet("VerCausalesRechazoDocumentosMC", CommandType.StoredProcedure)
        End With
        Return dsCausales
    End Function
    Public Function ConsultaCausalesRechazoMCdt() As DataTable

        Dim dbManager As New LMDataAccess
        Dim dsCausales As New DataTable
        With dbManager
            With .SqlParametros
                .Add("@IdServicio", SqlDbType.Int).Value = _idServicioMensajeria
            End With
            dsCausales = .EjecutarDataTable("VerCausalesRechazoDocumentosMC", CommandType.StoredProcedure)
        End With
        Return dsCausales
    End Function

    Public Function GuardarRechazoMC() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                With .SqlParametros
                    .Add("@IdServicio", SqlDbType.VarChar, 2000).Value = _splitServicios
                    .Add("@IdCausal", SqlDbType.VarChar, 2000).Value = _splitCausales
                    .Add("@Observaciones", SqlDbType.VarChar, 5000).Value = _observacionesDevolucionDocs
                    .Add("@IdUsuario", SqlDbType.Int).Value = _idUsuarioGenerador
                    .Add("@IdEstado", SqlDbType.Int).Value = _idEstado
                    If _docRecuperacion = True Then
                        .Add("@docRec", SqlDbType.Bit).Value = _docRecuperacion
                    End If
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("GuardarCausalRechazoDocServicio", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString())
        End Try
        Return resultado
    End Function

    Public Function GuardarRechazoBanco() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                With .SqlParametros

                    .Add("@IdServicio", SqlDbType.VarChar, 2000).Value = _splitServicios
                    .Add("@IdCausal", SqlDbType.VarChar, 2000).Value = _splitCausales
                    .Add("@Observaciones", SqlDbType.VarChar, 5000).Value = _observacionesDevolucionDocs
                    .Add("@IdUsuario", SqlDbType.Int).Value = _idUsuarioGenerador
                    .Add("@IdEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("GuardarCausalRechazoBanco", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString())
        End Try
        Return resultado
    End Function

    Public Function DocumentoRecuperacionMC() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                With .SqlParametros
                    .Add("@IdServicio", SqlDbType.Int).Value = _idRadicado
                    If _idCausalDevolucion > 0 Then
                        .Add("@IdCausal", SqlDbType.Int).Value = _idCausalDevolucion
                    End If
                    If Not String.IsNullOrEmpty(_observacionesDevolucionDocs) Then
                        .Add("@Observaciones", SqlDbType.VarChar).Value = _observacionesDevolucionDocs
                    End If
                    If _idUsuarioGenerador > 0 Then
                        .Add("@IdUsuario", SqlDbType.Int).Value = _idUsuarioGenerador
                    End If
                    If _docRecuperacion = True Then
                        .Add("@docRec", SqlDbType.Bit).Value = _docRecuperacion
                    End If
                    .Add("@IdEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("GuardarCausalRechazoDocServicio", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString())
        End Try
        Return resultado
    End Function
    Public Function RemoverCambiarEstadoRadicado() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim db As New LMDataAccess
            With db.SqlParametros
                .Clear()
                .Add("@radicado", SqlDbType.Int).Value = IdRadicado
                .Add("@idUsuario", SqlDbType.Int).Value = IdUsuarioGenerador
            End With
            db.EjecutarScalar("BorrarInfoPlantillaTransitoria", CommandType.StoredProcedure)
            resultado.EstablecerMensajeYValor(0, "Borrado exitosamente")
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(1, "Error al borrar radicado: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function ActualizarCambiarEstadoRadicado() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable

            With dbManager
                With .SqlParametros
                    .Add("@idRadicado", SqlDbType.Int).Value = _idRadicado
                    If _consultaInfRad Then
                        .Add("@consultarRadicado", SqlDbType.Bit).Value = _consultaInfRad
                    End If
                    If _idEstado > 0 Then
                        .Add("@IdEstado", SqlDbType.Int).Value = _idEstado
                    End If
                    If _idUsuarioGenerador > 0 Then
                        .Add("@IdUsuarioMod", SqlDbType.Int).Value = _idUsuarioGenerador
                    End If
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                If (_consultaInfRad = False) Then
                    .EjecutarNonQuery("VerificarCambiarEstadoRadicado", CommandType.StoredProcedure)
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    _dtInformacionRadicado = .EjecutarDataTable("VerificarCambiarEstadoRadicado", CommandType.StoredProcedure)
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                End If
            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al guardar el código: " & ex.ToString())
        End Try
        Return resultado
    End Function

    Public Function ConsultaCausalesDevolucion(ByVal Origen As Integer) As DataTable

        Dim dbManager As New LMDataAccess
        Dim dtCausales As New DataTable
        With dbManager
            .SqlParametros.Add("@Origen", SqlDbType.Int).Value = Origen
            dtCausales = .EjecutarDataTable("ConsultarCausalesRechazo", CommandType.StoredProcedure)
        End With
        Return dtCausales
    End Function

    Public Function ConsultaCausales(ByVal Origen As Integer) As DataTable

        Dim dbManager As New LMDataAccess
        Dim dtCausales As New DataTable
        With dbManager
            .SqlParametros.Add("@Origen", SqlDbType.Int).Value = Origen
            dtCausales = .EjecutarDataTable("ConsultarCausales", CommandType.StoredProcedure)
        End With
        Return dtCausales
    End Function

    Public Function GenerarPlanillaRadicacionBanco() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess
            Dim dsDatosCampania As New DataTable
            With dbManager
                With .SqlParametros
                    If _planillaGenerada <= 0 Then
                        .Add("@ClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                        .Add("@Precinto", SqlDbType.VarChar).Value = _precinto
                        .Add("@CiudadRadicacion", SqlDbType.Int).Value = _idCiudad
                        .Add("@IdUsuarioModif", SqlDbType.Int).Value = _idUsuarioGenerador
                    ElseIf _planillaGenerada > 0 Then
                        .Add("@idPlanilla", SqlDbType.Int).Value = _planillaGenerada
                        .Add("@Pagare", SqlDbType.Int).Value = _pagare
                        .Add("@Identificacion", SqlDbType.Int).Value = Identificaion
                        .Add("@Campania", SqlDbType.VarChar).Value = _campania
                        .Add("@CodEstrategia", SqlDbType.VarChar).Value = _codEstrategia
                        .Add("@Oficina", SqlDbType.Int).Value = _oficina
                        .Add("@IdUsuarioModif", SqlDbType.Int).Value = _idUsuarioGenerador
                        .Add("@IdEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@Observaciones", SqlDbType.VarChar).Value = _observacionesDevolucionDocs
                        .Add("@IdRadicado", SqlDbType.Int).Value = _idRadicado
                    End If
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@Evidencias", SqlDbType.Int).Direction = ParameterDirection.Output
                    .Add("@nombreCiudad", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@CodOficina", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("GuardarPlanillaRadicacionBanco", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
                _numEvidencias = .SqlParametros("@Evidencias").Value
                _nombreCiudad = .SqlParametros("@nombreCiudad").Value
                If Not IsDBNull(.SqlParametros("@CodOficina").Value) Then
                    _codOficinaCliente = .SqlParametros("@CodOficina").Value
                End If
            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al generar la planilla: " & ex.ToString())
        End Try
        Return resultado
    End Function

    Public Function PasoDestruccionDoc() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess

            With dbManager
                With .SqlParametros
                    .Add("@NumeroRadicado", SqlDbType.Int).Value = _idRadicado
                    If _idEstado > 0 Then
                        .Add("@IdEstado", SqlDbType.Int).Value = _idEstado
                    End If
                    .Add("@ValidarDestruccion", SqlDbType.Bit).Value = _validarPasoDestruccion
                    If _idUsuarioGenerador > 0 Then
                        .Add("@IdUsuarioMod", SqlDbType.Int).Value = _idUsuarioGenerador
                    End If
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("DestruccionDocumentosMC", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al pasar a destrucción: " & ex.ToString())
        End Try
        Return resultado
    End Function

    Public Function FinalizarCampania() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtFinalizar As New DataTable

        With dbManager
            With .SqlParametros
                .Add("@IdServicio", SqlDbType.Int).Value = _idRadicado
                .Add("@IdEstado", SqlDbType.Int).Value = _idEstado
                .Add("@IdUsuario", SqlDbType.Int).Value = _idUsuarioGenerador
            End With
            dtFinalizar = .EjecutarDataTable("FinalizarCampaniaMesaControl", CommandType.StoredProcedure)
        End With
        Return dtFinalizar
    End Function

    Public Function VerNovedadesRadicado() As DataSet
        Dim dbManager As New LMDataAccess
        Dim dsNovedades As New DataSet

        With dbManager
            With .SqlParametros
                .Add("@IdServicio", SqlDbType.Int).Value = _idRadicado
            End With
            dsNovedades = .EjecutarDataSet("ConsultarNovedadesMesaControl", CommandType.StoredProcedure)
        End With
        Return dsNovedades
    End Function

    Public Function DevolverEstadoRecepcionRadicado() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Dim dbManager As New LMDataAccess

            With dbManager
                With .SqlParametros
                    .Add("@NumeroRadicado", SqlDbType.Int).Value = _idRadicado
                    .Add("@IdEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@IdUsuarioMod", SqlDbType.Int).Value = _idUsuarioGenerador
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("DevolverEstadoRecepcionRadicado", CommandType.StoredProcedure)
                resultado.Valor = .SqlParametros("@resultado").Value
                resultado.Mensaje = .SqlParametros("@mensaje").Value
            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(500, "Ocurrió un error al pasar a destrucción: " & ex.ToString())
        End Try
        Return resultado
    End Function

    Public Function ObtenerCodOficina() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtCodOfc As New DataTable

        With dbManager
            With .SqlParametros
                .Add("@idServicio", SqlDbType.Int).Value = _idRadicado
            End With
            dtCodOfc = .EjecutarDataTable("ConsultarCodOficina", CommandType.StoredProcedure)
        End With
        Return dtCodOfc
    End Function
    Public Function GenerarPoolVisitasSimpliRoute(listaServicio As DataTable) As DataSet
        Dim dsDatos As New DataSet
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If _idUsuarioGenerador > 0 Then .Add("@idUsuarioGenerador", SqlDbType.Int).Value = _idUsuarioGenerador
                    'If _listIdServicio IsNot Nothing AndAlso _listIdServicio.Count > 0 Then .Add("@listaIdServicio", SqlDbType.VarChar).Value = Join(_listIdServicio.ToArray, ",")
                    '.SqlParametros.Add("@listaPedidos", SqlDbType.Structured).Value = listaPedidos
                    .AddWithValue("@tbIdServicioMensajeria", listaServicio)
                End With
                dsDatos = .EjecutarDataSet("ConsultarServiciosParaCrearVisitaSimpliRoute", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw ex
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dsDatos
    End Function
#End Region

End Class
