Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO
Imports ILSBusinessLayer.Enumerados

Public Class OrdenTrabajoReproceso

#Region "Atributos (campos)"

    Dim _idOrdenReproceso As Integer
    Dim _codigo As String
    Dim _idInstruccionReprocesoDetalle As Integer
    Dim _idInstruccionReproceso As Integer
    Dim _linea As Integer
    Dim _idOperador As Integer
    Dim _operador As String
    Dim _cantidadPedida As Integer
    Dim _cantidadLeida As Integer
    Dim _idCreador As Integer
    Dim _creador As String
    Dim _fechaCreacion As DateTime
    Dim _fechaCreacionInicial As DateTime
    Dim _fechaCreacionFinal As DateTime
    Dim _fechaInicio As DateTime
    Dim _fechaFinalizacion As DateTime
    Dim _idUsuarioCierre As Integer
    Dim _usuarioCierre As String
    Dim _observacion As String
    Dim _idEstado As Integer
    Dim _estado As String
    Dim _idModificador As Integer
    Dim _unidadesCaja As Integer
    Dim _cajaEstiba As Integer
    Dim _termosellado As Boolean
    Dim _label As Boolean
    Dim _idTipoOrden As Integer
    Dim _tipoOrden As String
    Dim _justificacion As String
    Dim _requierePin As Boolean
    Dim _leerDupla As Boolean
    Dim _materialSim As String
    Dim _idTipoSoftware As Integer
    Dim _tipoSoftware As String
    Dim _idTipoProducto As Integer
    Dim _idTipoProductoTexto As String
    Dim _idTipoClasificacionInstruccion As Integer
    Dim _idTipoClasificacionInstruccionTexto As String
    Dim _insertoHomologacion As Boolean
    Dim _revisado As Boolean

    Dim _registrado As Boolean


#End Region

#Region "Propiedades"

    Public Property IdOrdenReproceso() As Integer
        Get
            Return _idOrdenReproceso
        End Get
        Set(ByVal value As Integer)
            _idOrdenReproceso = value
        End Set
    End Property

    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property

    Public Property IdInstruccionReprocesoDetalle() As Integer
        Get
            Return _idInstruccionReprocesoDetalle
        End Get
        Set(ByVal value As Integer)
            _idInstruccionReprocesoDetalle = value
        End Set
    End Property

    Public Property IdInstruccionReproceso() As Integer
        Get
            Return _idInstruccionReproceso
        End Get
        Set(ByVal value As Integer)
            _idInstruccionReproceso = value
        End Set
    End Property

    Public Property Linea() As Integer
        Get
            Return _linea
        End Get
        Set(ByVal value As Integer)
            _linea = value
        End Set
    End Property

    Public Property IdOperador() As Integer
        Get
            Return _idOperador
        End Get
        Set(ByVal value As Integer)
            _idOperador = value
        End Set
    End Property

    Public Property Operador() As String
        Get
            Return _operador
        End Get
        Set(ByVal value As String)
            _operador = value
        End Set
    End Property

    Public Property CantidadPedida() As Integer
        Get
            Return _cantidadPedida
        End Get
        Set(ByVal value As Integer)
            _cantidadPedida = value
        End Set
    End Property

    Public Property CantidadLeida() As Integer
        Get
            Return _cantidadLeida
        End Get
        Set(ByVal value As Integer)
            _cantidadLeida = value
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

    Public Property FechaCreacion() As DateTime
        Get
            Return _fechaCreacion
        End Get
        Set(ByVal value As DateTime)
            _fechaCreacion = value
        End Set
    End Property

    Public Property FechaCreacionInicial() As DateTime
        Get
            Return _fechaCreacionInicial
        End Get
        Set(ByVal value As DateTime)
            _fechaCreacionInicial = value
        End Set
    End Property

    Public Property FechaCreacionFinal() As DateTime
        Get
            Return _fechaCreacionFinal
        End Get
        Set(ByVal value As DateTime)
            _fechaCreacionFinal = value
        End Set
    End Property

    Public Property FechaInicio() As DateTime
        Get
            Return _fechaInicio
        End Get
        Set(ByVal value As DateTime)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFinalizacion() As DateTime
        Get
            Return _fechaFinalizacion
        End Get
        Set(ByVal value As DateTime)
            _fechaFinalizacion = value
        End Set
    End Property

    Public Property IdUsuarioCierre() As Integer
        Get
            Return _idUsuarioCierre
        End Get
        Set(ByVal value As Integer)
            _idUsuarioCierre = value
        End Set
    End Property

    Public Property UsuarioCierre() As String
        Get
            Return _usuarioCierre
        End Get
        Set(ByVal value As String)
            _usuarioCierre = value
        End Set
    End Property

    Public Property Observacion() As String
        Get
            Return _observacion
        End Get
        Set(ByVal value As String)
            _observacion = value
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

    Public Property IdModificador() As Integer
        Get
            Return _idModificador
        End Get
        Set(ByVal value As Integer)
            _idModificador = value
        End Set
    End Property

    Public Property UnidadesCaja() As Integer
        Get
            Return _unidadesCaja
        End Get
        Set(ByVal value As Integer)
            _unidadesCaja = value
        End Set
    End Property

    Public Property CajaEstiba() As Integer
        Get
            Return _cajaEstiba
        End Get
        Set(ByVal value As Integer)
            _cajaEstiba = value
        End Set
    End Property

    Public Property Termosellado() As Boolean
        Get
            Return _termosellado
        End Get
        Set(ByVal value As Boolean)
            _termosellado = value
        End Set
    End Property

    Public Property Label() As Boolean
        Get
            Return _label
        End Get
        Set(ByVal value As Boolean)
            _label = value
        End Set
    End Property

    Public Property IdTipoOrden() As Integer
        Get
            Return _idTipoOrden
        End Get
        Set(ByVal value As Integer)
            _idTipoOrden = value
        End Set
    End Property

    Public Property TipoOrden() As String
        Get
            Return _tipoOrden
        End Get
        Set(ByVal value As String)
            _tipoOrden = value
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

    Public Property Justificacion As String
        Get
            Return _justificacion
        End Get
        Set(value As String)
            _justificacion = value
        End Set
    End Property

    Public Property RequierePin As Boolean
        Get
            Return _requierePin
        End Get
        Set(value As Boolean)
            _requierePin = value
        End Set
    End Property

    Public Property LeerDupla As Boolean
        Get
            Return _leerDupla
        End Get
        Set(value As Boolean)
            _leerDupla = value
        End Set
    End Property

    Public Property MaterialSim As String
        Get
            Return _materialSim
        End Get
        Set(value As String)
            _materialSim = value
        End Set
    End Property

    Public Property IdTipoSoftware As Integer
        Get
            Return _idTipoSoftware
        End Get
        Set(value As Integer)
            _idTipoSoftware = value
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

    Public Property IdTipoProducto As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property IdTipoProductoTexto As String
        Get
            Return _idTipoProductoTexto
        End Get
        Set(value As String)
            _idTipoProductoTexto = value
        End Set
    End Property

    Public Property IdTipoClasificacionInstruccion As Integer
        Get
            Return _idTipoClasificacionInstruccion
        End Get
        Set(value As Integer)
            _idTipoClasificacionInstruccion = value
        End Set
    End Property

    Public Property IdTipoClasificacionInstruccionTexto As String
        Get
            Return _idTipoClasificacionInstruccionTexto
        End Get
        Set(value As String)
            _idTipoClasificacionInstruccionTexto = value
        End Set
    End Property

    Public Property InsertoHomologacion As Boolean
        Get
            Return _insertoHomologacion
        End Get
        Set(value As Boolean)
            _insertoHomologacion = value
        End Set
    End Property

    Public Property Revisado As Boolean
        Get
            Return _revisado
        End Get
        Set(value As Boolean)
            _revisado = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _codigo = ""
        _operador = ""
        _creador = ""
        _observacion = ""
        _estado = ""
        _usuarioCierre = ""
        _tipoOrden = ""
    End Sub

    Public Sub New(ByVal idOrdenReproceso As Integer)
        MyBase.New()
        _idOrdenReproceso = idOrdenReproceso
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idOrdenReproceso", SqlDbType.Int).Value = _idOrdenReproceso

                .ejecutarReader("ConsultarOrdenTrabajoReproceso", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    If .Reader.Read Then

                        Integer.TryParse(.Reader("idOrdenReproceso"), _idOrdenReproceso)
                        _codigo = .Reader("codigo").ToString
                        Integer.TryParse(.Reader("idInstruccionReprocesoDetalle"), _idInstruccionReprocesoDetalle)
                        Integer.TryParse(.Reader("linea"), _linea)
                        Integer.TryParse(.Reader("idOperador"), _idOperador)
                        Integer.TryParse(.Reader("cantidadPedida"), _cantidadPedida)
                        Integer.TryParse(.Reader("cantidadLeida"), _cantidadLeida)
                        Integer.TryParse(.Reader("idCreador"), _idCreador)
                        _creador = .Reader("creador").ToString
                        If Not IsDBNull(.Reader("fechaCreacion")) Then _fechaCreacion = CDate(.Reader("fechaCreacion"))
                        If Not IsDBNull(.Reader("fechaInicio")) Then _fechaInicio = CDate(.Reader("fechaInicio"))
                        If Not IsDBNull(.Reader("fechaFinalizacion")) Then _fechaFinalizacion = CDate(.Reader("fechaFinalizacion"))
                        If Not IsDBNull(.Reader("idUsuarioCierre")) Then Integer.TryParse(.Reader("idUsuarioCierre"), _idUsuarioCierre)
                        If Not IsDBNull(.Reader("usuarioCierre")) Then _usuarioCierre = .Reader("usuarioCierre").ToString
                        If Not IsDBNull(.Reader("observacion")) Then _observacion = .Reader("observacion").ToString
                        Integer.TryParse(.Reader("idEstado"), _idEstado)
                        _estado = .Reader("estado").ToString
                        Integer.TryParse(.Reader("unidadesCaja"), _unidadesCaja)
                        Integer.TryParse(.Reader("cajaEstiba"), _cajaEstiba)
                        Boolean.TryParse(.Reader("termosellado"), _termosellado)
                        Boolean.TryParse(.Reader("label"), _label)
                        Integer.TryParse(.Reader("idTipoOrden"), _idTipoOrden)
                        If Not IsDBNull(.Reader("tipoOrden")) Then _tipoOrden = .Reader("tipoOrden").ToString
                        If Not IsDBNull(.Reader("justificacion")) Then _justificacion = .Reader("justificacion").ToString
                        Boolean.TryParse(.Reader("requierePin"), _requierePin)
                        Boolean.TryParse(.Reader("leerDupla"), _leerDupla)
                        If Not IsDBNull(.Reader("materialSim")) Then _materialSim = .Reader("materialSim").ToString
                        If Not IsDBNull(.Reader("idTipoSoftware")) Then Integer.TryParse(.Reader("idTipoSoftware"), _idTipoSoftware)
                        If Not IsDBNull(.Reader("tipoSoftware")) Then _tipoSoftware = .Reader("tipoSoftware").ToString
                        If Not IsDBNull(.Reader("idTipoProducto")) Then Integer.TryParse(.Reader("idTipoProducto"), _idTipoProducto)
                        If Not IsDBNull(.Reader("idTipoClasificacionInstruccion")) Then Integer.TryParse(.Reader("idTipoClasificacionInstruccion"), _idTipoClasificacionInstruccion)
                        Boolean.TryParse(.Reader("insertoHomologacion"), _insertoHomologacion)
                        Boolean.TryParse(.Reader("revisado"), _revisado)

                        .Reader.Close()
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
        Dim noResultado As Integer = -1
        If _idCreador > 0 Then
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        .Add("@idInstruccionReprocesoDetalle", SqlDbType.Int).Value = _idInstruccionReprocesoDetalle
                        .Add("@linea", SqlDbType.Int).Value = _linea
                        .Add("@idOperador", SqlDbType.Int).Value = _idOperador
                        .Add("@cantidadPedida", SqlDbType.Int).Value = _cantidadPedida
                        .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                        If Not String.IsNullOrEmpty(_observacion) Then .Add("@observacion", SqlDbType.VarChar, 450).Value = _observacion
                        '.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        .Add("@unidadesCaja", SqlDbType.Int).Value = _unidadesCaja
                        .Add("@cajaEstiba", SqlDbType.Int).Value = _cajaEstiba
                        .Add("@termosellado", SqlDbType.Bit).Value = _termosellado
                        .Add("@label", SqlDbType.Bit).Value = _label
                        .Add("@idTipoOrden", SqlDbType.Int).Value = _idTipoOrden
                        .Add("@requierePin", SqlDbType.Bit).Value = _requierePin
                        .Add("@leerDupla", SqlDbType.Bit).Value = _leerDupla
                        If _idTipoSoftware > 0 Then .Add("@idTipoSoftware", SqlDbType.Int).Value = _idTipoSoftware
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .Add("@idOrdenReproceso", SqlDbType.Int).Direction = ParameterDirection.Output
                        If Not String.IsNullOrEmpty(_materialSim) Then .Add("@materialSim", SqlDbType.VarChar, 450).Value = _materialSim
                        .Add("@revisado", SqlDbType.Bit).Value = _revisado
                        .Add("@insertoHomologacion", SqlDbType.Bit).Value = _insertoHomologacion
                    End With

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarOrdenTrabajoReproceso", CommandType.StoredProcedure)

                    Integer.TryParse(.SqlParametros("@idOrdenReproceso").Value.ToString(), _idOrdenReproceso)
                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)

                    If noResultado = 0 Then
                        .confirmarTransaccion()
                        resultado.EstablecerMensajeYValor(0, "Se realizo el registro correctamente.")
                    ElseIf noResultado = 1 Then
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(8, "La cantidad Asignada supera la cantidad de la instrucción, por favor verifique.")
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(9, "Se generó un error al realizar el registro, por favor intente nuevamente.")
                    End If

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

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim noResultado As Integer = -1
        If _idModificador > 0 Then
            Dim dbManager As New LMDataAccess
            With dbManager
                Try
                    With .SqlParametros
                        .Add("@idOrdenReproceso", SqlDbType.Int).Value = _idOrdenReproceso
                        .Add("@idModificador", SqlDbType.Int).Value = _idModificador
                        If _idInstruccionReprocesoDetalle > 0 Then .Add("@idInstruccionReprocesoDetalle", SqlDbType.Int).Value = _idInstruccionReprocesoDetalle
                        If _linea > 0 Then .Add("@linea", SqlDbType.Int).Value = _linea
                        If _idOperador > 0 Then .Add("@idOperador", SqlDbType.Int).Value = _idOperador
                        If _cantidadPedida > 0 Then .Add("@cantidadPedida", SqlDbType.Int).Value = _cantidadPedida
                        If _cantidadLeida > 0 Then .Add("@cantidadLeida", SqlDbType.Int).Value = _cantidadLeida
                        If _fechaInicio > Date.MinValue Then .Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
                        If _fechaFinalizacion > Date.MinValue Then .Add("@fechaFinalizacion", SqlDbType.DateTime).Value = _fechaFinalizacion
                        If _idUsuarioCierre > 0 Then .Add("@idUsuarioCierre", SqlDbType.Int).Value = _idUsuarioCierre
                        If Not String.IsNullOrEmpty(_observacion) Then .Add("@observacion", SqlDbType.VarChar, 450).Value = _observacion
                        If _idEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                        If _unidadesCaja > 0 Then .Add("@unidadesCaja", SqlDbType.Int).Value = _unidadesCaja
                        If _cajaEstiba > 0 Then .Add("@cajaEstiba", SqlDbType.Int).Value = _cajaEstiba
                        .Add("@termosellado", SqlDbType.Bit).Value = _termosellado
                        .Add("@label", SqlDbType.Bit).Value = _label
                        If _idTipoOrden > 0 Then .Add("@idTipoOrden", SqlDbType.Int).Value = _idTipoOrden
                        .Add("@requierePin", SqlDbType.Bit).Value = _requierePin
                        .Add("@leerDupla", SqlDbType.Bit).Value = _leerDupla
                        If _idTipoSoftware > 0 Then .Add("@idTipoSoftware", SqlDbType.Int).Value = _idTipoSoftware
                        If Not String.IsNullOrEmpty(_justificacion) Then .Add("@justificacion", SqlDbType.VarChar, 450).Value = _justificacion
                        .Add("@revisado", SqlDbType.Bit).Value = _revisado
                        .Add("@insertoHomologacion", SqlDbType.Bit).Value = _insertoHomologacion
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    End With

                    .iniciarTransaccion()
                    .ejecutarNonQuery("ActualizarOrdenTrabajoReproceso", CommandType.StoredProcedure)

                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), noResultado)

                    If noResultado = 0 Then
                        .confirmarTransaccion()
                        resultado.EstablecerMensajeYValor(0, "Se realizo la actualización correctamente.")
                    ElseIf noResultado = 1 Then
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(1, "No se encontro el identificador de la orden consultada, por favor intente nuevamente.")
                    ElseIf noResultado = 5 Then
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(5, "La cantidad solicitada supera la cantidad de la instrucción, por favor verificar.")
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
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para realizar el registro. ")
        End If
        Return resultado
    End Function

#End Region

#Region "Métodos Compartidos"

    Public Shared Function ObtenerOrdenesReprocesoPorInstruccion(Optional ByVal idInstruccionReprocesoDetalle As Integer = 0) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            With _dbManager

                If idInstruccionReprocesoDetalle > 0 Then .SqlParametros.Add("@idInstruccionReprocesoDetalle", SqlDbType.Int).Value = idInstruccionReprocesoDetalle
                dtDatos = .ejecutarDataTable("ConsultarOrdenTrabajoReproceso", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Shared Function ObtenerTipoOrden() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            With _dbManager
                dtDatos = .ejecutarDataTable("ConsultarTipoOrdenReproceso", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function CargarPoolOrdenReproceso() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            With _dbManager
                If _idInstruccionReproceso > 0 Then .SqlParametros.Add("@idInstruccionReproceso", SqlDbType.Int).Value = _idInstruccionReproceso
                If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                If _idOrdenReproceso > 0 Then .SqlParametros.Add("@idOrdenReproceso", SqlDbType.Int).Value = _idOrdenReproceso
                If _fechaCreacionInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaCreacionInicial
                If _fechaCreacionFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaCreacionFinal
                If Not String.IsNullOrEmpty(_idTipoClasificacionInstruccionTexto) Then .SqlParametros.Add("@listaClasificacionInstruccion", SqlDbType.VarChar, 450).Value = _idTipoClasificacionInstruccionTexto
                If Not String.IsNullOrEmpty(_idTipoProductoTexto) Then .SqlParametros.Add("@listaTipoProducto", SqlDbType.VarChar, 450).Value = _idTipoProductoTexto
                dtDatos = .ejecutarDataTable("ConsultarOrdenTrabajoReproceso", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try

        Return dtDatos
    End Function

#End Region

#Region "Enumerados"

    Public Enum Estados
        Creada = 152
        Proceso = 153
        Cerrado = 154
        CerradoManual = 155
        Cancelada = 156
    End Enum

    Public Enum TipoOrdenReproceso
        ReprocesoNC = 1
        ReprocesoRec = 2
        Reproceso = 3
    End Enum


#End Region

End Class
