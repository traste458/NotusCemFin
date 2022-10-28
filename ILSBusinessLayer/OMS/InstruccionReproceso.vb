Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO

Public Class InstruccionReproceso

#Region "Atributos (campos)"

    Dim _idInstruccionReproceso As Integer
    Dim _idTipoInstruccion As Integer
    Dim _tipoInstruccion As String
    Dim _idFactura As Integer
    Dim _factura As String
    Dim _idEstado As Integer
    Dim _estado As String
    Dim _idCreador As Integer
    Dim _creador As String
    Dim _fechaRegistro As Date
    Dim _observacion As String
    Dim _idBloqueo As Integer
    Dim _idTipoClasificacionInstruccion As Integer
    Dim _tipoClasificacionInstruccion As String
    Dim _idModificador As Integer
    Dim _idPickingInstruccionReproceso As Integer
    Dim _flagEliminacion As Integer

    Dim _permiso As Integer

    Dim _registrado As Boolean
    Dim _instruccionTrabajo As DataTable

#End Region

#Region "Propiedades"

    Public Property IdInstruccionReproceso() As Integer
        Get
            Return _idInstruccionReproceso
        End Get
        Set(ByVal value As Integer)
            _idInstruccionReproceso = value
        End Set
    End Property

    Public Property IdTipoInstruccion() As Integer
        Get
            Return _idTipoInstruccion
        End Get
        Set(ByVal value As Integer)
            _idTipoInstruccion = value
        End Set
    End Property

    Public Property TipoInstruccion() As String
        Get
            Return _tipoInstruccion
        End Get
        Set(ByVal value As String)
            _tipoInstruccion = value
        End Set
    End Property

    Public Property IdFactura() As Integer
        Get
            Return _idFactura
        End Get
        Set(ByVal value As Integer)
            _idFactura = value
        End Set
    End Property

    Public Property Factura() As String
        Get
            Return _factura
        End Get
        Set(ByVal value As String)
            _factura = value
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

    Public Property Estado() As String
        Get
            Return _estado
        End Get
        Set(ByVal value As String)
            _estado = value
        End Set
    End Property

    Public Property IdCreador() As Integer
        Get
            Return _idCreador
        End Get
        Set(ByVal value As Integer)
            _idCreador = value
        End Set
    End Property

    Public Property Creador() As String
        Get
            Return _creador
        End Get
        Set(ByVal value As String)
            _creador = value
        End Set
    End Property

    Public Property FechaRegistro() As Date
        Get
            Return _fechaRegistro
        End Get
        Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property IdBloqueo() As Integer
        Get
            Return _idBloqueo
        End Get
        Set(ByVal value As Integer)
            _idBloqueo = value
        End Set
    End Property

    Public Property IdTipoClasificacacionInstruccion() As Integer
        Get
            Return _idTipoClasificacionInstruccion
        End Get
        Set(ByVal value As Integer)
            _idTipoClasificacionInstruccion = value
        End Set
    End Property

    Public Property TipoClasificacacionInstruccion() As String
        Get
            Return _tipoClasificacionInstruccion
        End Get
        Set(ByVal value As String)
            _tipoClasificacionInstruccion = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

    Public Property InstruccionTrabajo() As DataTable
        Get
            Return _instruccionTrabajo
        End Get
        Set(ByVal value As DataTable)
            _instruccionTrabajo = value
        End Set
    End Property

    Public Property IdModificador() As Integer
        Get
            Return _idModificador
        End Get
        Set(ByVal value As Integer)
            _idModificador = value
        End Set
    End Property

    Public Property Permiso() As Integer
        Get
            Return _permiso
        End Get
        Set(ByVal value As Integer)
            _permiso = value
        End Set
    End Property

    Public Property IdPickingInstruccionReproceso As Integer
        Get
            Return _idPickingInstruccionReproceso
        End Get
        Set(ByVal value As Integer)
            _idPickingInstruccionReproceso = value
        End Set
    End Property

    Public Property FlagEliminacion As Integer
        Get
            Return _flagEliminacion
        End Get
        Set(value As Integer)
            _flagEliminacion = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _tipoInstruccion = ""
        _factura = ""
        _creador = ""
        _estado = ""
        _observacion = ""
    End Sub

    Public Sub New(ByVal idInstruccionReproceso As Integer)
        MyBase.New()
        _idInstruccionReproceso = idInstruccionReproceso
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idInstruccionReproceso", SqlDbType.Int).Value = _idInstruccionReproceso

                .ejecutarReader("ObtenerInformacionInstruccionReproceso", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        'Integer.TryParse(reader("idInstruccionReproceso"), _idInstruccionReproceso)
                        'Integer.TryParse(reader("idTipoInstruccion"), _idTipoInstruccion)
                        'If Not IsDBNull(reader("tipoInstruccion")) Then _tipoInstruccion = reader("tipoInstruccion").ToString
                        'Integer.TryParse(reader("idFactura"), _idFactura)
                        'If Not IsDBNull(reader("factura")) Then _factura = reader("factura").ToString
                        'Integer.TryParse(reader("idEstado"), _idEstado)
                        'If Not IsDBNull(reader("estado")) Then _estado = reader("estado").ToString
                        'Integer.TryParse(reader("idCreador"), _idCreador)
                        'If Not IsDBNull(reader("creador")) Then _creador = reader("creador").ToString
                        'If Not IsDBNull(reader("fechaRegistro")) Then _fechaRegistro = CDate(reader("fechaRegistro"))
                        'If Not IsDBNull(reader("observaciones")) Then _observacion = reader("observaciones").ToString
                        'Integer.TryParse(reader("idBloqueo"), _idBloqueo)
                        'Integer.TryParse(reader("idTipoClasificacionInstruccion"), _idTipoClasificacionInstruccion)
                        'If Not IsDBNull(reader("tipoClasificacionInstruccion")) Then _tipoClasificacionInstruccion = reader("_tipoClasificacionInstruccion").ToString

                        'reader.Close()

                        '_instruccionTrabajo = .ejecutarDataTable("ConsultarInstruccionDetalle", CommandType.StoredProcedure)
                        CargarResultadoConsulta(.Reader)
                        _registrado = True

                    End If
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim returnValue As Integer
        If _idCreador > 0 Then
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        .Add("@idTipoInstruccion", SqlDbType.Int).Value = _idTipoInstruccion
                        If _idFactura > 0 Then .Add("@idFactura", SqlDbType.Int).Value = _idFactura
                        .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        If Not String.IsNullOrEmpty(_observacion) Then .Add("@observacion", SqlDbType.VarChar, 450).Value = _observacion
                        .Add("@idTipoClasificacacionInstruccion", SqlDbType.Int).Value = _idTipoClasificacionInstruccion
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .Add("@idInstruccionReproceso", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarInstruccionReproceso", CommandType.StoredProcedure)

                    Integer.TryParse(.SqlParametros("@idInstruccionReproceso").Value.ToString(), IdInstruccionReproceso)

                    If Not IsDBNull(.SqlParametros("@resultado").Value) AndAlso Integer.TryParse(.SqlParametros("@resultado").Value.ToString, returnValue) Then
                        If returnValue = 0 Then
                            .confirmarTransaccion()
                            resultado.EstablecerMensajeYValor(0, "La instrucción de reproceso se creo satisfactoriamente.")
                        Else
                            .abortarTransaccion()
                            resultado.EstablecerMensajeYValor(9, "Se generó un error al realizar la instrucción, por favor intente el registro nuevamente.")
                        End If
                    End If

                    'If noResultado = 0 Then
                    '    If IdInstruccionReproceso > 0 Then
                    '        If _instruccionTrabajo IsNot Nothing AndAlso _instruccionTrabajo.Rows.Count > 0 Then
                    '            Dim columnaidInstruccionReproceso As New DataColumn("idInstruccionReproceso", GetType(Integer))
                    '            columnaidInstruccionReproceso.DefaultValue = IdInstruccionReproceso
                    '            _instruccionTrabajo.Columns.Add(columnaidInstruccionReproceso)

                    '            .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.FireTriggers)
                    '            With .BulkCopy
                    '                .DestinationTableName = "InstruccionReprocesoDetalle"
                    '                .ColumnMappings.Add("idInstruccionReproceso", "idInstruccionReproceso")
                    '                .ColumnMappings.Add("idRegionOrigen", "idRegionOrigen")
                    '                .ColumnMappings.Add("idRegionDestino", "idRegionDestino")
                    '                .ColumnMappings.Add("materialOrigen", "materialOrigen")
                    '                .ColumnMappings.Add("materialDestino", "materialDestino")
                    '                .ColumnMappings.Add("cantidad", "cantidad")
                    '                .WriteToServer(_instruccionTrabajo)
                    '            End With
                    '        End If
                    '        .confirmarTransaccion()
                    '    Else
                    '        resultado.EstablecerMensajeYValor(8, "No se establecio el identificador de la instrucción, por favor intente nuevamente.")
                    '        .abortarTransaccion()
                    '    End If
                    'Else
                    '    If noResultado = -1 Then resultado.EstablecerMensajeYValor(9, "Se generó un error al realizar la instrucción, por favor intente el registro nuevamente.")
                    '    .abortarTransaccion()
                    'End If
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            End With
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para realizar el registro. ")
        End If
        Return resultado
    End Function

    Public Function Actualizar(Optional justificacion As String = "") As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim noResultado As Integer = -1
        If _idModificador > 0 Then
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        .Add("@idInstruccionReproceso", SqlDbType.Int).Value = _idInstruccionReproceso
                        .Add("@idModificador", SqlDbType.Int).Value = _idModificador
                        If _idTipoInstruccion > 0 Then .Add("@idTipoInstruccion", SqlDbType.Int).Value = _idTipoInstruccion
                        If _idFactura > 0 Then .Add("@idFactura", SqlDbType.Int).Value = _idFactura
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If Not String.IsNullOrEmpty(_observacion) Then .Add("@observacion", SqlDbType.VarChar, 450).Value = _observacion
                        If Not String.IsNullOrEmpty(justificacion) Then .Add("@justificacion", SqlDbType.VarChar, 450).Value = justificacion
                        If _idBloqueo > 0 Then .Add("@idBloqueo", SqlDbType.Int).Value = _idBloqueo
                        If _idTipoClasificacionInstruccion > 0 Then .Add("@idTipoClasificacionInstruccion", SqlDbType.Int).Value = _idTipoClasificacionInstruccion
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarInstruccionReproceso", CommandType.StoredProcedure)

                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)

                    If noResultado = 0 Then
                        .confirmarTransaccion()
                        resultado.EstablecerMensajeYValor(0, "Se realizo la actualización satisfactoriamente.")
                    ElseIf noResultado = 1 Then
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(1, "No se encontro el identificador de la instrucción consultada, por favor intente nuevamente.")
                    ElseIf noResultado = 2 Then
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(2, "La instrucción tiene picking asociado, por lo cual no se puede anular.")
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(9, "Se generó un error inesperado al realizar la actualización, por favor intente el registro nuevamente.")
                    End If
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            End With
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para actualizar el registro. ")
        End If
        Return resultado

    End Function

#End Region

#Region "Métodos Compartidos"

    Public Shared Function ObtenerTipoInstruccion() As DataTable
        Dim db As New LMDataAccess
        Return db.ejecutarDataTable("SeleccionarTipoInstruccion", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerCentro(Optional ByVal idRegionReproceso As Integer = 0) As DataTable
        Dim db As New LMDataAccess
        If idRegionReproceso > 0 Then db.SqlParametros.Add("@idRegionReproceso", SqlDbType.Int).Value = idRegionReproceso
        Return db.ejecutarDataTable("ObtenerRegiones", CommandType.StoredProcedure)
    End Function

    Public Shared Function Cargar(ByVal dtDatos As DataTable, ByVal idUsuario As Integer, Optional ByVal idFactura As Integer = 0) As Boolean
        Dim db As New LMDataAccessLayer.LMDataAccess
        dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))

        If idFactura > 0 Then
            dtDatos.Columns.Add(New DataColumn("idFactura", GetType(System.Int64), idFactura))
        Else
            dtDatos.Columns.Add(New DataColumn("idFactura", GetType(System.Int64)))
        End If

        Try
            db.agregarParametroSQL("@idUsuario", idUsuario, SqlDbType.BigInt)
            db.ejecutarNonQuery("BorrarTablasAuxiliaresReprocesos", CommandType.StoredProcedure)
            db.inicilizarBulkCopy()
            db.BulkCopy.DestinationTableName = "InstruccionReprocesoDetalleCargue"
            db.BulkCopy.ColumnMappings.Add("centroOrigen", "centroOrigen")
            db.BulkCopy.ColumnMappings.Add("idRegionOrigen", "idRegionOrigen")
            db.BulkCopy.ColumnMappings.Add("materialOrigen", "materialOrigen")
            db.BulkCopy.ColumnMappings.Add("materialDestino", "materialDestino")
            db.BulkCopy.ColumnMappings.Add("cantidad", "cantidad")
            db.BulkCopy.ColumnMappings.Add("centroDestino", "centroDestino")
            db.BulkCopy.ColumnMappings.Add("idRegionDestino", "idRegionDestino")
            db.BulkCopy.ColumnMappings.Add("lineaArchivo", "lineaArchivo")
            db.BulkCopy.ColumnMappings.Add("idUsuario", "idUsuario")
            db.BulkCopy.ColumnMappings.Add("idFactura", "idFactura")
            db.BulkCopy.ColumnMappings.Add("idTipoInstruccion", "idTipoInstruccion")
            db.BulkCopy.WriteToServer(dtDatos)
            db.ejecutarNonQuery("ActualizaInstruccionReprocesoDetalleCargue", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try

    End Function

    Public Shared Function ValidarDisponibilidad(ByVal idUsuario As Integer, ByVal flag As Integer) As DataSet
        Dim dsResultado As New DataSet()
        Dim dtLog As New DataTable ' En este dt se relizan las validaciones de disponibilidad y existencias de región y materiales.
        Dim dtDetalle As New DataTable ' Cuando la validación es exitosa se almacena el detalle de la instrucción.
        Dim db As New LMDataAccess

        With db
            With .SqlParametros
                .Clear()
                .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                If flag > 0 Then .Add("@flag", SqlDbType.Int).Value = flag
            End With
            dtLog = .ejecutarDataTable("ValidacionInventarioReprocesos", CommandType.StoredProcedure)
            If dtLog.Rows.Count = 0 Then
                dtDetalle = .ejecutarDataTable("CargarDetalleInstruccionReproceso", CommandType.StoredProcedure)
            End If
        End With

        dsResultado.Tables.Add(dtLog)
        dsResultado.Tables.Add(dtDetalle)
        dsResultado.AcceptChanges()
        Return dsResultado
    End Function

    Public Function ObtenerPermisos(ByVal idPerfil As Integer, ByVal idFuncionalidad As Integer, Optional ByVal tipoProducto As Integer = 0) As DataTable
        Dim db As New LMDataAccess
        Dim dtPermisos As New DataTable
        With db
            .SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = idPerfil
            .SqlParametros.Add("@idFuncionalidad", SqlDbType.Int).Value = idFuncionalidad
            If tipoProducto > 0 Then .SqlParametros.Add("@tipoProducto", SqlDbType.Int).Value = tipoProducto
            dtPermisos = .ejecutarDataTable("PermisosCreacionInstruccionReproceso", CommandType.StoredProcedure)
        End With
        Return dtPermisos
    End Function

    Public Shared Function ObtenerTipoClasificacion(ByVal tipoOrigen As String, Optional ByVal idClasificaion As String = "") As DataTable
        Dim db As New LMDataAccess
        db.SqlParametros.Add("@tipoOrigen", SqlDbType.VarChar, 250).Value = tipoOrigen
        If Not String.IsNullOrEmpty(idClasificaion) Then db.SqlParametros.Add("@idClasificacion", SqlDbType.VarChar, 250).Value = idClasificaion
        Return db.ejecutarDataTable("SeleccionarTipoClasificacion", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerDetalle(ByVal idUsuario As Integer) As DataTable
        Dim db As New LMDataAccess
        db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
        Return db.ejecutarDataTable("CargarDetalleInstruccionReproceso", CommandType.StoredProcedure)
    End Function

    Public Shared Function ConsultarTipoClasificacionPerfil(ByVal idPerfil As Integer) As DataTable
        Dim db As New LMDataAccess
        db.SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = idPerfil
        Return db.ejecutarDataTable("ConsultaTipoClasificacionInstruccionPerfil", CommandType.StoredProcedure)
    End Function

    Public Shared Function ConsultarFacturas(ByVal filtroRapido As String) As DataTable
        Dim db As New LMDataAccess
        db.SqlParametros.Add("@factura", SqlDbType.VarChar, 150).Value = filtroRapido
        Return db.ejecutarDataTable("ObtenerInfoFactura", CommandType.StoredProcedure)
    End Function

    Public Shared Function VerDisponibilidad(ByVal idUsuario As Integer) As DataTable
        Dim db As New LMDataAccess
        db.SqlParametros.Add("@idUsuario", SqlDbType.VarChar, 150).Value = idUsuario
        Return db.ejecutarDataTable("ObtenerDisponibilidadReprocesos", CommandType.StoredProcedure)
    End Function

    Public Shared Function ConsultarEstado(ByVal idEntidad As Integer) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                .SqlParametros.Add("@idEntidad", SqlDbType.Int).Value = idEntidad
                dtDatos = .ejecutarDataTable("ConsultarEstadoEntidad", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Shared Function ConsultarInstrucciones(Optional ByVal idPerfil As Integer = 0, Optional ByVal idEstado As Integer = 0) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                If idPerfil > 0 Then .SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = idPerfil
                If idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = idEstado
                dtDatos = .ejecutarDataTable("ObtenerInformacionInstruccionReproceso", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ObtenerIdPickingReproceso(ByVal idInstruccionReproceso As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idInstruccionReproceso", SqlDbType.Int).Value = idInstruccionReproceso
                .ejecutarReader("ObtenerInformacionPickingReproceso", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        Integer.TryParse(.Reader("idPickingInstruccionReproceso"), _idPickingInstruccionReproceso)
                    End If
                    .Reader.Close()
                    resultado.EstablecerMensajeYValor(0, "Se establecio correctamente el Picking")
                Else
                    _idPickingInstruccionReproceso = 0
                    resultado.EstablecerMensajeYValor(1, "No se logro establecer el identificador del Picking asociado a la isntrucción de reproceso, por favor intente nuevamente.")
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idInstruccionReproceso"), _idInstruccionReproceso)
                Integer.TryParse(reader("idTipoInstruccion"), _idTipoInstruccion)
                If Not IsDBNull(reader("tipoInstruccion")) Then _tipoInstruccion = reader("tipoInstruccion").ToString
                Integer.TryParse(reader("idFactura"), _idFactura)
                If Not IsDBNull(reader("factura")) Then _factura = reader("factura").ToString
                Integer.TryParse(reader("idEstado"), _idEstado)
                If Not IsDBNull(reader("estado")) Then _estado = reader("estado").ToString
                Integer.TryParse(reader("idCreador"), _idCreador)
                If Not IsDBNull(reader("creador")) Then _creador = reader("creador").ToString
                If Not IsDBNull(reader("fechaRegistro")) Then _fechaRegistro = CDate(reader("fechaRegistro"))
                If Not IsDBNull(reader("observaciones")) Then _observacion = reader("observaciones").ToString
                Integer.TryParse(reader("idTipoClasificacionInstruccion"), _idTipoClasificacionInstruccion)
                If Not IsDBNull(reader("tipoClasificacionInstruccion")) Then _tipoClasificacionInstruccion = reader("tipoClasificacionInstruccion").ToString
                Integer.TryParse(reader("flagEliminacion"), _flagEliminacion)
            End If
        End If

    End Sub

#End Region

#Region "Enumerados"

    Public Enum Estados
        Pendiente_Autorizar = 140
        Creada = 141
        Liberada = 142
        Picking_Generado = 143
        No_procesada = 144
        Proceso = 145
        Cerrada = 146
        Anulada = 151
    End Enum

    Public Enum Funcionalidad
        CreacionInstruccionReproceso = 1
        EditarOrdenReproceso = 2
        PoolInstruccionesReprocesos = 3
        ModificacionInstrucciones = 4
    End Enum


#End Region

End Class
