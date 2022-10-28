Imports System.Data.SqlClient
Imports LMDataAccessLayer
Imports NotusExpressBusinessLayer.General

Public Class CargadorArchivosGestionBaseClientesPrencece

#Region "Atributos"

    Private _dtDatosArchivoPresence As DataTable
    Private _dtDatosArchivo As DataTable
    Private _idRegistroBase As Integer
    Private _nombreCargaPresence As String
    Private _idUsusrio As Integer
    Private _idservicio As Integer
    Private _loadId As Integer
    Private _datosCargaPresence As DataTable
    Private _datosInfoServicioLoadOutbound As DataTable
    Private _datosCargadosPendientesPorIntegrar As DataTable
    Private _datosIntegradosPendientesPorNotificar As DataTable
    Private _fechaInicio As Date
    Private _fechaFin As Date

#End Region


#Region "Propiedades"

    Public Property DtDatosArchivoPresence() As DataTable
        Get
            Return _dtDatosArchivoPresence
        End Get
        Set(ByVal value As DataTable)
            _dtDatosArchivoPresence = value
        End Set
    End Property

    Public Property loadId() As Integer
        Get
            Return _loadId
        End Get
        Set(ByVal value As Integer)
            _loadId = value
        End Set
    End Property
    Public Property IdRegistroBase() As Integer
        Get
            Return _idRegistroBase
        End Get
        Set(ByVal value As Integer)
            _idRegistroBase = value
        End Set
    End Property


    Public Property idservicio() As Integer
        Get
            Return _idservicio
        End Get
        Set(ByVal value As Integer)
            _idservicio = value
        End Set
    End Property

    Public Property FechaInicio As Date
        Get
            Return _fechaInicio
        End Get
        Set(value As Date)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFin As Date
        Get
            Return _fechaFin
        End Get
        Set(value As Date)
            _fechaFin = value
        End Set
    End Property

    Public Property idUsuario() As Integer
        Get
            Return _idUsusrio
        End Get
        Set(ByVal value As Integer)
            _idUsusrio = value
        End Set
    End Property

    Public Property NombreCargaPresence() As String
        Get
            Return _nombreCargaPresence
        End Get
        Set(ByVal value As String)
            _nombreCargaPresence = value
        End Set
    End Property
#End Region


#Region "Métodos Privados"

    Private Sub InicializarTablaDatosArchivo()
        _dtDatosArchivo = New DataTable

        With _dtDatosArchivo
            .Columns.Add("lineaArchivo", GetType(Integer))
            .Columns.Add("estrategia", GetType(String))
            .Columns.Add("cedulaAsesor", GetType(String))
            .Columns.Add("nombreAsesor", GetType(String))
            .Columns.Add("tipoProducto", GetType(String))
            .Columns.Add("anio", GetType(Short))
            .Columns.Add("mes", GetType(Short))
            .Columns.Add("meta", GetType(Integer))
        End With
    End Sub

    Public ReadOnly Property ConsultarServiciosSalientes() As DataTable
        Get
            If _datosCargaPresence Is Nothing Then CargarDatosPresence()
            Return _datosCargaPresence
        End Get
    End Property
    Public ReadOnly Property ConsultarInfoVolverLlamarServicioPresence() As DataTable
        Get
            If _datosCargaPresence Is Nothing Then CargarInfoValverLlamarServicioPresence()
            Return _datosCargaPresence
        End Get
    End Property
    Public ReadOnly Property ConsultarInfoServicioLoadsOutbound() As DataTable
        Get
            If _datosInfoServicioLoadOutbound Is Nothing Then ObtenerInfoServicioLoadOutboundPresence()
            Return _datosInfoServicioLoadOutbound
        End Get
    End Property
    Public ReadOnly Property consultarSiHayCargaPrendiente() As DataTable
        Get
            If _datosCargadosPendientesPorIntegrar Is Nothing Then ObtenerCargasPendiensXIntegrar()
            Return _datosCargadosPendientesPorIntegrar
        End Get
    End Property
    Public ReadOnly Property consultarIntegracionesPrendientePorNotificar() As DataTable
        Get
            If _datosIntegradosPendientesPorNotificar Is Nothing Then ObtenerDatosPendienteXNotificar()
            Return _datosIntegradosPendientesPorNotificar
        End Get
    End Property
#End Region
#Region "Métodos Públicos"

    Public Function SaveDataInTables(ByVal dataTable As DataTable, ByVal tablename As String) As Boolean

        Dim send As New ConectorPresenceDB
        Dim stringConection As String = send.CadenaConexion

        If dataTable.Rows.Count > 0 Then

            Using con As SqlConnection = New SqlConnection(stringConection)

                Using sqlBulkCopy As SqlBulkCopy = New SqlBulkCopy(con)
                    sqlBulkCopy.DestinationTableName = tablename
                    con.Open()
                    sqlBulkCopy.WriteToServer(dataTable)
                    con.Close()
                    Return True
                End Using
            End Using
        Else
            Return False
        End If

    End Function

    Public Function BorrarCargaTransitoriaIntegrada(ByVal LoadId As Integer) As Boolean

        Dim send As New ConectorPresenceDB
        Dim stringConection As String = send.CadenaConexion

        Using con As SqlConnection = New SqlConnection(stringConection)
            con.Open()
            Dim cadena As String = " DELETE TransitoriaCargasBD 	WHERE LoadId =" & LoadId
            Dim comando As SqlCommand = New SqlCommand(cadena, con)
            Dim cant As Integer
            cant = comando.ExecuteNonQuery()
            con.Close()
            If cant = 1 Then
                Return True
            Else
                Return False
            End If

        End Using


    End Function


    Public Sub CargarDatosPresence()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager

                If _idUsusrio >= 0 Then .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = _idUsusrio
                If _idservicio >= 0 Then .SqlParametros.Add("@IdServicio", SqlDbType.Int).Value = _idservicio
                If _nombreCargaPresence IsNot Nothing Then .SqlParametros.Add("@NombreCargaPresence", SqlDbType.VarChar).Value = _nombreCargaPresence
                _datosCargaPresence = .EjecutarDataTable("ConsultarGestionBaseclientePresence", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub
    Public Sub CargarInfoValverLlamarServicioPresence()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager

                If _idRegistroBase >= 0 Then .SqlParametros.Add("@IdRegistroBase", SqlDbType.Int).Value = _idRegistroBase
                If _idUsusrio >= 0 Then .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = _idUsusrio
                _datosCargaPresence = .EjecutarDataTable("ObtenerInfoCargaParaVolverAllamar", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Sub ObtenerCargasPendiensXIntegrar()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager

                If _idUsusrio >= 0 Then .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = _idUsusrio
                If _idservicio > 0 Then .SqlParametros.Add("@IdServicio", SqlDbType.Int).Value = _idservicio
                _datosCargadosPendientesPorIntegrar = .EjecutarDataTable("ObtenerCargasPendientesXIntegrarXidUsuario", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Sub ObtenerDatosPendienteXNotificar()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager

                If _idUsusrio >= 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsusrio
                If _idUsusrio >= 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idservicio
                If FechaInicio <> Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.Date).Value = FechaInicio
                If FechaFin <> Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.Date).Value = FechaFin
                _datosIntegradosPendientesPorNotificar = .EjecutarDataTable("ObtenerDatosPendientePorNotificarPresence", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub



    Public Sub ObtenerInfoServicioLoadOutboundPresence()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager

                If _idUsusrio >= 0 Then .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = _idUsusrio
                If _idservicio >= 0 Then .SqlParametros.Add("@IdServicio", SqlDbType.Int).Value = _idservicio
                _datosInfoServicioLoadOutbound = .EjecutarDataTable("ObtenerInfoCosumoServicioLoadsOutBound", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Function ConsultarUltimaCargaIntegrada() As String
        Dim dbManager As New LMDataAccess
        Dim IdCargaIntegrada As String
        Try
            With dbManager
                If _idUsusrio >= 0 Then .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = _idUsusrio
                .SqlParametros.Add("@idLoad", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .EjecutarDataSet("ObtenerUltimaCargaIntegradaPresence", CommandType.StoredProcedure)
                IdCargaIntegrada = .SqlParametros("@idLoad").Value.ToString
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return IdCargaIntegrada
    End Function

    Public Function ConsultarServicioXCarga() As String
        Dim dbManager As New LMDataAccess
        Dim IdServicio As String
        Try
            With dbManager
                If _idUsusrio >= 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsusrio
                If _loadId >= 0 Then .SqlParametros.Add("@idLoad", SqlDbType.Int).Value = _loadId
                .SqlParametros.Add("@idServicio", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .EjecutarDataSet("ObtenerServicioPresenceCarga", CommandType.StoredProcedure)
                IdServicio = .SqlParametros("@idServicio").Value.ToString
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return IdServicio
    End Function

    Public Function ConsultarEstadoCargaPresence() As String
        Dim dbManager As New LMDataAccess
        Dim EstadoCarga As String
        Try
            With dbManager
                If _idUsusrio >= 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsusrio
                If _loadId >= 0 Then .SqlParametros.Add("@idLoad", SqlDbType.Int).Value = _loadId
                .SqlParametros.Add("@EstadoCarga", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .EjecutarDataSet("ObtenerEstadoCargaPresence", CommandType.StoredProcedure)
                EstadoCarga = .SqlParametros("@EstadoCarga").Value.ToString
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return EstadoCarga
    End Function

    Public Function EliminarCargasNoIntegradas() As String
        Dim dbManager As New LMDataAccess
        Dim IdCargaNoIntegrada As String
        Try
            With dbManager
                If _idUsusrio >= 0 Then .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = _idUsusrio
                If _idservicio >= 0 Then .SqlParametros.Add("@IdServicio", SqlDbType.Int).Value = _idservicio
                .SqlParametros.Add("@NombreIntegracion", SqlDbType.VarChar).Value = _nombreCargaPresence
                .SqlParametros.Add("@idLoad", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .EjecutarDataSet("BorrarCargaNoIntegradaPresence", CommandType.StoredProcedure)
                IdCargaNoIntegrada = .SqlParametros("@idLoad").Value.ToString
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return IdCargaNoIntegrada
    End Function

    Public Function GenerarTemporalesRemarcado(ByVal datosBase As DataTable, ByVal idTipo As Integer, ByVal nombreCarga As String, ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.AddWithValue("@tbBaseServicio", datosBase)
                If idTipo >= 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = idTipo
                .SqlParametros.Add("@nombreCarga", SqlDbType.VarChar).Value = nombreCarga
                If idUsuario >= 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .IniciarTransaccion()
                .EjecutarNonQuery("GenerarTemporalesRemarcado", CommandType.StoredProcedure)


                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor > 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(500, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(400, "Se presento un error al realizar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function


    Public Function ConsultaDetalleDatosRemarcado(ByVal idBase) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                If idBase <> 0 Then .SqlParametros.Add("@idBase", SqlDbType.Int).Value = idBase
                dtDatos = .EjecutarDataTable("ConsultaDetalleDatosRemarcado", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region


End Class
