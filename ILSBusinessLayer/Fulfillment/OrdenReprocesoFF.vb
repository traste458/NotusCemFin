Imports LMDataAccessLayer

Namespace Fulfillment

    Public Class OrdenReprocesoFF

#Region "Atributos (campos)"

        Private _idOrdenReproceso As Integer
        Private _codigo As String
        Private _idInstruccionReprocesoDetalle As Integer
        Private _idInstruccionReproceso As Integer
        Private _linea As Integer
        Private _idOperador As Integer
        Private _operador As String
        Private _cantidadPedida As Integer
        Private _cantidadLeida As Integer
        Private _idCreador As Integer
        Private _creador As String
        Private _fechaCreacion As DateTime
        Private _fechaCreacionInicial As DateTime
        Private _fechaCreacionFinal As DateTime
        Private _fechaInicio As DateTime
        Private _fechaFinalizacion As DateTime
        Private _idUsuarioCierre As Integer
        Private _usuarioCierre As String
        Private _observacion As String
        Private _idEstado As Integer
        Private _estado As String
        Private _idModificador As Integer
        Private _unidadesCaja As Integer
        Private _cajaEstiba As Integer
        Private _termosellado As Boolean
        Private _label As Boolean
        Private _idTipoOrden As Integer
        Private _tipoOrden As String
        Private _justificacion As String
        Private _ordenCompuesta As String
        Private _idTipoProducto As Integer
        Private _idTecnologia As Integer
        Private _idProdcuto As Integer
        Private _idTipoEtiqueta As Integer
        Private _codigoEan As String
        Private _requierePin As Boolean
        Private _leerDupla As Boolean
        Private _materialSim As String
        Private _regionOrden As String
        Private _revisado As Boolean
        Private _idOrdenBodegaje As Integer
        Private _tipoOtb As Integer
        Private _lecturaCaja As Boolean

        Private _registrado As Boolean


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

        Public Property OrdenCompuesta As String
            Get
                Return _ordenCompuesta
            End Get
            Set(value As String)
                _ordenCompuesta = value
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

        Public Property IdTecnologia As Integer
            Get
                Return _idTecnologia
            End Get
            Set(value As Integer)
                _idTecnologia = value
            End Set
        End Property

        Public Property IdProducto As Integer
            Get
                Return _idProdcuto
            End Get
            Set(value As Integer)
                _idProdcuto = value
            End Set
        End Property

        Public Property IdTipoEtiqueta As Integer
            Get
                Return _idTipoEtiqueta
            End Get
            Set(value As Integer)
                _idTipoEtiqueta = value
            End Set
        End Property

        Public Property CodigoEan As String
            Get
                Return _codigoEan
            End Get
            Set(value As String)
                _codigoEan = value
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

        Public Property RegionOrden As String
            Get
                Return _regionOrden
            End Get
            Set(value As String)
                _regionOrden = value
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

        Public Property IdOrdenBodegaje As Integer
            Get
                Return _idOrdenBodegaje
            End Get
            Set(value As Integer)
                _idOrdenBodegaje = value
            End Set
        End Property

        Public Property TipoOtb As Integer
            Get
                Return _tipoOtb
            End Get
            Set(value As Integer)
                _tipoOtb = value
            End Set
        End Property

        Public Property LecturaCaja As Boolean
            Get
                Return _lecturaCaja
            End Get
            Set(value As Boolean)
                _lecturaCaja = value
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
                            Integer.TryParse(.Reader("idInstruccionReproceso"), _idInstruccionReproceso)
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
                            If Not IsDBNull(.Reader("ordenCompuesta")) Then _ordenCompuesta = .Reader("ordenCompuesta").ToString
                            Integer.TryParse(.Reader("idTipoProducto"), _idTipoProducto)
                            Integer.TryParse(.Reader("idTecnologia"), _idTecnologia)
                            Integer.TryParse(.Reader("idProdcuto"), _idProdcuto)
                            Integer.TryParse(.Reader("idTipoEtiqueta"), _idTipoEtiqueta)
                            If Not IsDBNull(.Reader("codigoEan")) Then _codigoEan = .Reader("codigoEan").ToString
                            Boolean.TryParse(.Reader("leerDupla"), _leerDupla)
                            Boolean.TryParse(.Reader("requierePin"), _requierePin)
                            If Not IsDBNull(.Reader("regionOrden")) Then _regionOrden = .Reader("regionOrden").ToString
                            Boolean.TryParse(.Reader("revisado"), _revisado)
                            Integer.TryParse(.Reader("tipoOtb"), _tipoOtb)
                            Boolean.TryParse(.Reader("lecturaCaja"), _lecturaCaja)

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
                            If _termosellado Then .Add("@termosellado", SqlDbType.Bit).Value = _termosellado
                            If _label Then .Add("@label", SqlDbType.Bit).Value = _label
                            If _idTipoOrden > 0 Then .Add("@idTipoOrden", SqlDbType.Int).Value = _idTipoOrden
                            If _requierePin Then .Add("@requierePin", SqlDbType.Bit).Value = _requierePin
                            If _leerDupla Then .Add("@leerDupla", SqlDbType.Bit).Value = _leerDupla
                            If _idTipoProducto > 0 Then .Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
                            If _idOrdenBodegaje > 0 Then .Add("@idOrdenBodegaje", SqlDbType.Int).Value = _idOrdenBodegaje
                            If Not String.IsNullOrEmpty(_justificacion) Then .Add("@justificacion", SqlDbType.VarChar, 450).Value = _justificacion
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

        Public Function GenerarOTB() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess

            With dbManager
                Try
                    With .SqlParametros
                        .Clear()
                        .Add("@idOrdenReproceso", SqlDbType.Int).Value = _idOrdenReproceso
                        .Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioCierre
                        .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .Add("@idOrdenBodegaje", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    End With

                    Dim returnValue As Integer
                    .ejecutarNonQuery("RegistrarOtbReproceso", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@returnValue").Value) AndAlso Integer.TryParse(.SqlParametros("@returnValue").Value.ToString, returnValue) Then
                        If returnValue = 0 Then
                            Integer.TryParse(.SqlParametros("@idOrdenBodegaje").Value.ToString, _idOrdenBodegaje)
                            resultado.EstablecerMensajeYValor(0, "La Otb fue creada satisfactoriamente.")
                        End If
                    Else
                        If dbManager IsNot Nothing Then dbManager.Dispose()
                        resultado.EstablecerMensajeYValor(1, "Imposible Determinar si la OTB fue generada automaticamente.")
                    End If
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            End With

            Return resultado
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ConsultarOrdenes(ByVal idInstruccionReproceso As Integer, ByVal idLinea As Integer) As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    .SqlParametros.Add("@idLinea", SqlDbType.Int).Value = idLinea
                    .SqlParametros.Add("@idInstruccionReproceso", SqlDbType.Int).Value = idInstruccionReproceso
                    dtDatos = .ejecutarDataTable("ConsultarOrdenTrabajoReproceso", CommandType.StoredProcedure)
                End With
            Finally
                If _dbManager IsNot Nothing Then _dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function ConsultarInstrucciones() As DataTable
            Dim _dbManager As New LMDataAccess
            Dim dtDatos As New DataTable

            Try
                With _dbManager
                    dtDatos = .ejecutarDataTable("ObtenerInformacionInstruccionReproceso", CommandType.StoredProcedure)
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

End Namespace


