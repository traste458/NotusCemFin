Namespace LogisticaInversa
    Public Class Devolucion
#Region "Variables"
        Private _iddevolucion As Integer
        Private _iddevolucion2 As String
        Private _fecha As Date
        Private _idCliente As Integer
        Private _idTercero As Integer
        Private _idEstado As Integer
        Private _cargado As Date
        Private _observacion As String
        Private _idGrupoDevolucion As Integer
        Private _idPosicion As Integer
        Private _guia As String
        Private _leido As Date
        Private _consultado As Date
        Private _entregado As Date
        Private _enviado As Date
        Private _reprocesado As Date
        Private _idTransportadora As Integer
        Private _idOrdenRecoleccion As Integer
        Private _posicion As String
        Private _detalle As DevolucionDetalle
        Private _grupoDevolucion As String
#End Region

#Region "Propiedades"

        Public Property GrupoDevolucion() As String
            Get
                Return _grupoDevolucion
            End Get
            Set(ByVal value As String)
                GrupoDevolucion = _grupoDevolucion
            End Set
        End Property

        Public Property Detalle() As DevolucionDetalle
            Get
                Return _detalle
            End Get
            Set(ByVal value As DevolucionDetalle)
                _detalle = value
            End Set
        End Property

        Public Property Posicion() As String
            Get
                Return _posicion
            End Get
            Set(ByVal value As String)
                _posicion = value
            End Set
        End Property

        Public Property IdOrdenRecoleccion() As Integer
            Get
                Return _idOrdenRecoleccion
            End Get
            Set(ByVal value As Integer)
                _idOrdenRecoleccion = value
            End Set
        End Property

        Public Property Reprocesado() As Date
            Get
                Return _reprocesado
            End Get
            Set(ByVal value As Date)
                _reprocesado = value
            End Set
        End Property

        Public Property Enviado() As Date
            Get
                Return _enviado
            End Get
            Set(ByVal value As Date)
                _enviado = value
            End Set
        End Property

        Public Property Entregado() As Date
            Get
                Return _entregado
            End Get
            Set(ByVal value As Date)
                _entregado = value
            End Set
        End Property

        Public Property Consultado() As Date
            Get
                Return _consultado
            End Get
            Set(ByVal value As Date)
                _consultado = value
            End Set
        End Property

        Public Property Leido() As Date
            Get
                Return _leido
            End Get
            Set(ByVal value As Date)
                _leido = value
            End Set
        End Property

        Public Property Guia() As String
            Get
                Return _guia
            End Get
            Set(ByVal value As String)
                _guia = value
            End Set
        End Property

        Public Property IdPosicion() As Integer
            Get
                Return _idPosicion
            End Get
            Set(ByVal value As Integer)
                _idPosicion = value
            End Set
        End Property

        Public Property IdGrupoDevolucion() As Integer
            Get
                Return _idGrupoDevolucion
            End Get
            Set(ByVal value As Integer)
                _idGrupoDevolucion = value
            End Set
        End Property

        Public Property Cargado() As Date
            Get
                Return _cargado
            End Get
            Set(ByVal value As Date)
                _cargado = value
            End Set
        End Property

        Public Property IdTercero() As Integer
            Get
                Return _idTercero
            End Get
            Set(ByVal value As Integer)
                _idTercero = value
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

        Public Property idCliente() As Integer
            Get
                Return _idCliente
            End Get
            Set(ByVal value As Integer)
                _idCliente = value
            End Set
        End Property

        Public ReadOnly Property Fecha() As Date
            Get
                Return _fecha
            End Get

        End Property

        Public Property IdDevolucion() As Integer
            Get
                Return _iddevolucion
            End Get
            Set(ByVal value As Integer)
                _iddevolucion = value
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
#End Region



#Region "Metodos"

        Public Function Crear() As Boolean
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Dim retorno As Boolean
            With dbManager
                With .SqlParametros
                    .Add("@idRecoleccion", SqlDbType.BigInt).Value = _idOrdenRecoleccion
                    .Add("@idTercero", SqlDbType.BigInt).Value = _idTercero
                    .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    .Add("@cargado", SqlDbType.SmallDateTime).Value = IIf(_cargado > Date.MinValue, _cargado, DBNull.Value)
                    .Add("@observacion", SqlDbType.VarChar).Value = IIf(_observacion <> String.Empty, _observacion, DBNull.Value)
                    .Add("@idgrupo_devolucion", SqlDbType.BigInt).Value = IIf(_idGrupoDevolucion > 0, _idGrupoDevolucion, DBNull.Value)
                    .Add("@idposicion", SqlDbType.Int).Value = IIf(_idPosicion > 0, _idPosicion, DBNull.Value)
                    .Add("@leido", SqlDbType.SmallDateTime).Value = IIf(_leido > Date.MinValue, _leido, DBNull.Value)
                    .Add("@consultado", SqlDbType.SmallDateTime).Value = IIf(_consultado > Date.MinValue, _consultado, DBNull.Value)
                    .Add("@entregado", SqlDbType.SmallDateTime).Value = IIf(_entregado > Date.MinValue, _entregado, DBNull.Value)
                    .Add("@enviado", SqlDbType.SmallDateTime).Value = IIf(_enviado > Date.MinValue, _enviado, DBNull.Value)
                    .Add("@reprocesado", SqlDbType.SmallDateTime).Value = IIf(_reprocesado > Date.MinValue, _reprocesado, DBNull.Value)
                    .Add("@idDevolucion", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    Dim result As Integer = 0
                    .iniciarTransaccion()
                    .ejecutarNonQuery("CrearOrdenDevolucionLogisticaInversa", CommandType.StoredProcedure)
                    result = .SqlParametros("@result").Value
                    If result = 0 Then
                        _iddevolucion = CLng(.SqlParametros("@idDevolucion").Value)
                        retorno = True
                        dbManager.confirmarTransaccion()
                    Else
                        dbManager.abortarTransaccion()
                        Throw New Exception(result)
                        retorno = False
                    End If
                Catch ex As Exception
                    dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                Finally
                    dbManager.Dispose()
                End Try
            End With
            Return retorno
        End Function

        Private Sub CargarDatos(ByVal filtro As Estructuras.FiltroDevolucion)
            Dim db As New LMDataAccessLayer.LMDataAccess

            If filtro.idDevolucion > 0 Then db.agregarParametroSQL("@idDevolucion", filtro.idDevolucion, SqlDbType.Int)
            If filtro.idOrdenRecoleccion > 0 Then db.agregarParametroSQL("@idRecoleccion", filtro.idOrdenRecoleccion, SqlDbType.Int)
            If filtro.idDevolucion > 0 Or filtro.idOrdenRecoleccion > 0 Then
                With db
                    Try
                        .ejecutarReader("ConsultarDevoluvionesLogisticaInversa", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read() Then
                                _iddevolucion = .Reader("iddevolucion")
                                _iddevolucion2 = .Reader("iddevolucion2").ToString()
                                _fecha = .Reader("fecha")
                                _idCliente = .Reader("idCliente")
                                _idTercero = .Reader("idTercero")
                                _idEstado = .Reader("idEstado")
                                Date.TryParse(.Reader("cargado").ToString(), _cargado)
                                _observacion = .Reader("observacion").ToString()
                                Integer.TryParse(.Reader("idGrupoDevolucion").ToString(), _idGrupoDevolucion)
                                Integer.TryParse(.Reader("idPosicion").ToString(), _idPosicion)
                                _guia = .Reader("guia").ToString()
                                Date.TryParse(.Reader("leido").ToString(), _leido)
                                Date.TryParse(.Reader("consultado").ToString(), _consultado)
                                Date.TryParse(.Reader("entregado").ToString(), _entregado)
                                Date.TryParse(.Reader("enviado").ToString(), _enviado)
                                Date.TryParse(.Reader("reprocesado").ToString(), _reprocesado)
                                _idTransportadora = .Reader("idTransportadora")
                                _idOrdenRecoleccion = .Reader("idRecoleccion")
                                _grupoDevolucion = .Reader("grupoDevolucion").ToString()
                            End If
                        End If
                    Finally
                        .Dispose()
                    End Try
                End With
            End If

        End Sub

        Public Shared Function ObtenerPorOrdenRecoleccion(ByVal idOrden As Integer) As Devolucion
            Dim filtro As New Estructuras.FiltroDevolucion
            filtro.idOrdenRecoleccion = idOrden
            Dim dev As New Devolucion
            dev.CargarDatos(filtro)
            Return dev
        End Function

        Public Sub Actualizar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            Try
                With db
                    .agregarParametroSQL("@idDevolucion", _iddevolucion)
                    If _observacion <> "" Then .agregarParametroSQL("@observacion", _observacion)
                    If _idGrupoDevolucion > 0 Then .agregarParametroSQL("@idGrupoDevolucion", _idGrupoDevolucion, SqlDbType.Int)
                    If _idEstado <> 0 Then .agregarParametroSQL("@idEstado", _idEstado, SqlDbType.Int)
                    .ejecutarNonQuery("ActualizarDevolucionLogisticaInversa", CommandType.StoredProcedure)
                    If _idEstado = 2 Then
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idDevolucion", SqlDbType.BigInt).Value = _iddevolucion
                        .ejecutarNonQuery("ActualizaInfoComplementariaSerialesAlCerrarOrdenDevolucion", CommandType.StoredProcedure)
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try
        End Sub
#End Region
        Public Sub New(ByVal idDevolucion As Integer)
            Me.New()
            Dim filtro As New Estructuras.FiltroDevolucion
            filtro.idDevolucion = idDevolucion
            Me.CargarDatos(filtro)
        End Sub

        Public Sub New()
            _detalle = New DevolucionDetalle
        End Sub

    End Class

End Namespace
