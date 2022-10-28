Imports LMDataAccessLayer
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Comunes
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Pedidos
Imports ILSBusinessLayer.Productos
Imports LMWebServiceSyncMonitorBusinessLayer

Namespace Despachos

    Public Class DespachoGeneral

#Region "Atributos"

        Protected _idDespacho As Long
        Protected _idPedido As Long
        Protected _numeroPedido As Long
        Protected _numeroEntrega As Long
        Protected _idAuxiliarAtiende As Integer
        Protected _idAuxiliarCierra As Integer
        Protected _fechaCreacion As Date
        Protected _fechaCierre As Date
        Protected _idTransportadora As Integer
        Protected _transportadora As String
        Protected _guia As String
        Protected _idEstado As Integer
        Protected _estado As String
        Protected _peso As Double
        Protected _idTipoDespacho As Short
        Protected _tipoDespacho As String
        Protected _idTipoTransporte As Integer
        Protected _tipoTransporte As String
        Protected _valorDeclarado As Double
        Protected _cantidadDeEmpaques As Integer
        Protected _idUnidadEmpaque As Integer
        Protected _unidadDeEmpaque As String
        Protected _volumen As Double
        Protected _idTipoPedido As Integer
        Protected _tipoPedido As String
        Protected _ciudadDestino As String
        Protected _regionDestino As String
        Protected _codigoCliente As String
        Protected _infoCliente As String
        Protected _requierePrecintos As Boolean
        Protected _requiereGuia As Boolean
        Protected _diceContener As String
        Protected _contabilizarEnCliente As Boolean
        Protected _listaPrecintos As String
        Protected _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdDespacho() As Long
            Get
                Return _idDespacho
            End Get
            Protected Friend Set(ByVal value As Long)
                _idDespacho = value
            End Set
        End Property

        Public Property IdPedido() As Long
            Get
                Return _idPedido
            End Get
            Set(ByVal value As Long)
                _idPedido = value
            End Set
        End Property

        Public Property NumeroPedido() As Long
            Get
                Return _numeroPedido
            End Get
            Protected Friend Set(ByVal value As Long)
                _numeroPedido = value
            End Set
        End Property

        Public Property NumeroEntrega() As Long
            Get
                Return _numeroEntrega
            End Get
            Set(ByVal value As Long)
                _numeroEntrega = value
            End Set
        End Property

        Public Property IdAuxiliarAtiende() As Integer
            Get
                Return _idAuxiliarAtiende
            End Get
            Set(ByVal value As Integer)
                _idAuxiliarAtiende = value
            End Set
        End Property

        Public Property IdAuxiliarCierra() As Integer
            Get
                Return _idAuxiliarCierra
            End Get
            Set(ByVal value As Integer)
                _idAuxiliarCierra = value
            End Set
        End Property

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaCreacion = value
            End Set
        End Property

        Public Property FechaCierre() As Date
            Get
                Return _fechaCierre
            End Get
            Protected Friend Set(ByVal value As Date)
                _fechaCierre = value
            End Set
        End Property

        Public Property IdTransportadora() As Integer
            Get
                Return _idTransportadora
            End Get
            Set(ByVal value As Integer)
                _idTransportadora = value
            End Set
        End Property

        Public Property Transportadora() As String
            Get
                Return _transportadora
            End Get
            Protected Friend Set(ByVal value As String)
                _idTransportadora = value
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
            Protected Friend Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property Peso() As Double
            Get
                Return _peso
            End Get
            Set(ByVal value As Double)
                _peso = value
            End Set
        End Property

        Public Property IdTipoDespacho() As Integer
            Get
                Return _idTipoDespacho
            End Get
            Set(ByVal value As Integer)
                _idTipoDespacho = value
            End Set
        End Property

        Public Property TipoDespacho() As String
            Get
                Return _tipoDespacho
            End Get
            Set(ByVal value As String)
                _tipoDespacho = value
            End Set
        End Property

        Public Property IdTipoTransporte() As Integer
            Get
                Return _idTipoTransporte
            End Get
            Set(ByVal value As Integer)
                _idTipoTransporte = value
            End Set
        End Property

        Public Property TipoTransporte() As String
            Get
                Return _tipoTransporte
            End Get
            Set(ByVal value As String)
                _tipoTransporte = value
            End Set
        End Property

        Public Property ValorDeclarado() As Double
            Get
                Return _valorDeclarado
            End Get
            Protected Friend Set(ByVal value As Double)
                _valorDeclarado = value
            End Set
        End Property

        Public Property CantidadDeEmpaques() As Integer
            Get
                Return _cantidadDeEmpaques
            End Get
            Set(ByVal value As Integer)
                _cantidadDeEmpaques = value
            End Set
        End Property

        Public Property IdUnidadEmpaque() As Integer
            Get
                Return _idUnidadEmpaque
            End Get
            Set(ByVal value As Integer)
                _idUnidadEmpaque = value
            End Set
        End Property

        Public Property Volumen() As Double
            Get
                Return _volumen
            End Get
            Set(ByVal value As Double)
                _volumen = value
            End Set
        End Property

        Public Property IdTipoPedido() As Integer
            Get
                Return _idTipoPedido
            End Get
            Protected Friend Set(ByVal value As Integer)
                _idTipoPedido = value
            End Set
        End Property

        Public Property TipoPedido() As String
            Get
                Return _tipoPedido
            End Get
            Set(ByVal value As String)
                _tipoPedido = value
            End Set
        End Property

        Public Property CiudadDestino() As String
            Get
                Return _ciudadDestino
            End Get
            Set(ByVal value As String)
                _ciudadDestino = value
            End Set
        End Property

        Public Property RegionDestino() As String
            Get
                Return _regionDestino
            End Get
            Set(ByVal value As String)
                _regionDestino = value
            End Set
        End Property

        Public Property CodigoCliente() As String
            Get
                Return _codigoCliente
            End Get
            Set(ByVal value As String)
                _codigoCliente = value
            End Set
        End Property

        Public Property InfoCliente() As String
            Get
                Return _infoCliente
            End Get
            Set(ByVal value As String)
                _infoCliente = value
            End Set
        End Property

        Public Property RequierePrecintos() As Boolean
            Get
                Return _requierePrecintos
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _requierePrecintos = value
            End Set
        End Property

        Public Property RequiereGuia() As Boolean
            Get
                Return _requiereGuia
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _requiereGuia = value
            End Set
        End Property

        Public Property ContabilizarEnCliente() As Boolean
            Get
                Return _contabilizarEnCliente
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _contabilizarEnCliente = value
            End Set
        End Property

        Public Property DiceContener() As String
            Get
                Return _diceContener
            End Get
            Set(ByVal value As String)
                _diceContener = value
            End Set
        End Property

        Public Property ListaPrecintos() As String
            Get
                Return _listaPrecintos
            End Get
            Set(ByVal value As String)
                _listaPrecintos = value
            End Set
        End Property

        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Constructores"
        Public Sub New()
            _transportadora = ""
            _guia = ""
            _estado = ""
            _tipoDespacho = ""
            _tipoTransporte = ""
            _unidadDeEmpaque = ""
            _tipoPedido = ""
            _ciudadDestino = ""
            _regionDestino = ""
            _codigoCliente = ""
            _infoCliente = ""
            _listaPrecintos = ""
            _diceContener = ""
        End Sub

        Public Sub New(ByVal idDespacho As Integer)
            Me.New()
            _idDespacho = idDespacho
            Me.CargarDatos()
        End Sub
#End Region

#Region "Métodos Públicos"

        Public Sub CargarDatos()
            If _idDespacho > 0 Or _idPedido > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idDespacho > 0 Then .SqlParametros.Add("@idDespacho", SqlDbType.BigInt).Value = _idDespacho
                        If _idPedido > 0 Then .SqlParametros.Add("@idPedido", SqlDbType.BigInt).Value = _idPedido

                        .ejecutarReader("ObtenerInfoDespachoGeneral", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then CargarResultadoConsulta(.Reader)
                            If Not .Reader.IsClosed Then .Reader.Close()
                        End If
                    End With

                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idDespacho").ToString, _idDespacho)
                    Integer.TryParse(reader("idPedido").ToString, _idPedido)
                    Long.TryParse(reader("numeroPedido").ToString, _numeroPedido)
                    Long.TryParse(reader("numeroEntrega").ToString, _numeroEntrega)
                    Integer.TryParse(reader("idAuxiliarAtiende").ToString, _idAuxiliarAtiende)
                    Integer.TryParse(reader("idAuxiliarCierra").ToString, _idAuxiliarCierra)
                    Date.TryParse(reader("fechaCreacion").ToString, _fechaCreacion)
                    Date.TryParse(reader("fechaCierre").ToString, _fechaCierre)
                    Integer.TryParse(reader("idTransportadora").ToString, _idTransportadora)
                    _transportadora = reader("transportadora").ToString
                    _guia = reader("guia").ToString
                    Integer.TryParse(reader("idEstado").ToString, _idEstado)
                    _estado = reader("estado").ToString
                    Double.TryParse(reader("peso").ToString, _peso)
                    Short.TryParse(reader("idTipoDespacho").ToString, IdTipoDespacho)
                    _tipoDespacho = reader("tipoDespacho").ToString
                    Integer.TryParse(reader("idTipoTransporte").ToString, _idTipoTransporte)
                    _tipoTransporte = reader("tipoTransporte").ToString
                    Double.TryParse(reader("valorDeclarado").ToString, _valorDeclarado)
                    Integer.TryParse(reader("cantidadDeEmpaques").ToString, _cantidadDeEmpaques)
                    Integer.TryParse(reader("idUnidadEmpaque").ToString, _idUnidadEmpaque)
                    _unidadDeEmpaque = reader("unidadDeEmpaque").ToString
                    Double.TryParse(reader("volumen").ToString, _volumen)
                    Integer.TryParse(reader("idTipoPedido").ToString, _idTipoPedido)
                    _tipoPedido = reader("tipoPedido").ToString
                    _ciudadDestino = reader("ciudadDestino").ToString
                    _regionDestino = reader("regionDestino").ToString
                    _codigoCliente = reader("codigoCliente").ToString
                    _infoCliente = reader("infoCliente").ToString
                    _requierePrecintos = CBool(reader("requierePrecinto").ToString)
                    _requiereGuia = CBool(reader("usaGuia").ToString)
                    _diceContener = reader("diceContener").ToString
                    _contabilizarEnCliente = CBool(reader("contabilizarEnCliente").ToString)
                    _registrado = True
                End If
            End If

        End Sub

#End Region

    End Class

End Namespace