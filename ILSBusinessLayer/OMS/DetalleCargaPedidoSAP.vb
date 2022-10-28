Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace OMS

    Public Class DetalleCargaPedidoSAP

#Region "Atributos (Campos)"

        Private _idDetalle As Long
        Private _pedido As Long
        Private _idOrden As Long
        Private _posicion As Integer
        Private _material As String
        Private _centro As String
        Private _cantidad As Integer
        Private _entrega As Long
        Private _contabilizado As Boolean
        Private _cambioMaterial As Boolean
        Private _idCreador As Integer
        Private _fechaCreacion As Date
        Private _creador As String
        Private _datosDetalle As DataTable

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal pedido As Long)
            MyBase.New()
            _pedido = pedido
            'CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdDetalle() As Long
            Get
                Return _idDetalle
            End Get
        End Property

        Public Property Pedido() As Long
            Get
                Return _pedido
            End Get
            Set(ByVal value As Long)
                _pedido = value
            End Set
        End Property

        Public Property IdOrden() As Long
            Get
                Return _idOrden
            End Get
            Set(ByVal value As Long)
                _idOrden = value
            End Set
        End Property

        Public Property Posicion() As Integer
            Get
                Return _posicion
            End Get
            Set(ByVal value As Integer)
                _posicion = value
            End Set
        End Property

        Public Property Material() As String
            Get
                Return _material
            End Get
            Set(ByVal value As String)
                _material = value
            End Set
        End Property

        Public Property Centro() As String
            Get
                Return _centro
            End Get
            Set(ByVal value As String)
                _centro = value
            End Set
        End Property

        Public Property Cantidad() As Integer
            Get
                Return _cantidad
            End Get
            Set(ByVal value As Integer)
                _cantidad = value
            End Set
        End Property

        Public Property Entrega() As Long
            Get
                Return _entrega
            End Get
            Set(ByVal value As Long)
                _entrega = value
            End Set
        End Property

        Public Property Contabilizado() As Boolean
            Get
                Return _contabilizado
            End Get
            Set(ByVal value As Boolean)
                _contabilizado = value
            End Set
        End Property

        Public Property CambioMaterial() As Boolean
            Get
                Return _cambioMaterial
            End Get
            Set(ByVal value As Boolean)
                _cambioMaterial = value
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

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As Date)
                _fechaCreacion = value
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

        Public ReadOnly Property DatosDetalle() As DataTable
            Get
                If _datosDetalle Is Nothing Then CargarDatos()
                Return _datosDetalle
            End Get
        End Property

#End Region

#Region "Métodos Privados"

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If _pedido > 0 Then .SqlParametros.Add("@pedido", SqlDbType.BigInt).Value = _pedido
                    _datosDetalle = .ejecutarDataTable("ObtenerInfoDetalleCargaPedidoSAP", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Sub CargarInformacion()
            If _pedido > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@pedido", SqlDbType.Int).Value = _pedido
                        .ejecutarReader("ObtenerDetalleCargaPedidoSAP", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                Long.TryParse(.Reader("pedido").ToString, _pedido)
                                Long.TryParse(.Reader("idOrden").ToString, _idOrden)
                                Integer.TryParse(.Reader("posicion").ToString, _posicion)
                                _material = .Reader("material").ToString
                                _centro = .Reader("centro").ToString
                                Integer.TryParse(.Reader("cantidad").ToString, _cantidad)
                                Long.TryParse(.Reader("entrega").ToString, _entrega)
                                _contabilizado = CBool(.Reader("contabilizado").ToString)
                                _cambioMaterial = CBool(.Reader("cambioMaterial").ToString)
                                Integer.TryParse(.Reader("idCreador").ToString, _idCreador)
                                Date.TryParse(.Reader("fechaCreacion").ToString, _fechaCreacion)
                                _creador = .Reader("creador").ToString
                            End If
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Crear() As Short
            Dim resultado As Short = 0
            If _pedido > 0 AndAlso _entrega AndAlso _idOrden AndAlso _posicion > 0 AndAlso _idCreador > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@pedido", SqlDbType.BigInt).Value = _pedido
                            .Add("@posicion", SqlDbType.Int).Value = _posicion
                            .Add("@entrega", SqlDbType.BigInt).Value = _entrega
                            .Add("@idOrden", SqlDbType.BigInt).Value = _idOrden
                            .Add("@material", SqlDbType.VarChar, 20).Value = _material
                            .Add("@centro", SqlDbType.VarChar, 10).Value = _centro
                            .Add("@cantidad", SqlDbType.Int).Value = _cantidad
                            .Add("@idCreador", SqlDbType.Int).Value = _idCreador
                            .Add("@idDetalle", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .TiempoEsperaComando = 600
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearDetalleCargaPedidoSAP", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                        If resultado = 0 Then
                            _idDetalle = CInt(.SqlParametros("@idDetalle").Value)
                            .confirmarTransaccion()
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                        End If

                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

        Public Function Actualizar() As Short
            Dim resultado As Short = 0
            If _entrega > 0 Then

                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@entrega", SqlDbType.BigInt).Value = _entrega
                            If _contabilizado Then .Add("@contabilizacion", SqlDbType.Bit).Value = _contabilizado
                            If _cambioMaterial Then .Add("@cambioMaterial", SqlDbType.Bit).Value = _cambioMaterial
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .TiempoEsperaComando = 600
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarDetalleCargaPedidoSAP", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado)

                        If resultado = 0 Then
                            .confirmarTransaccion()
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

#End Region

#Region "Metodos Compartidos"

        Public Shared Function ObtenerListado(ByVal filtro As FiltroDetalleCargaPedidoSAP) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                With .SqlParametros
                    If filtro.Pedido > 0 Then .Add("@pedido", SqlDbType.BigInt).Value = filtro.Pedido
                    If filtro.Entrega > 0 Then .Add("@entrega", SqlDbType.BigInt).Value = filtro.Entrega
                    If filtro.Material IsNot Nothing AndAlso filtro.Material.Trim.Length > 0 Then _
                    .Add("@material", SqlDbType.VarChar, 20).Value = filtro.Material
                    If filtro.Centro IsNot Nothing AndAlso filtro.Centro.Trim.Length > 0 Then _
                    .Add("@centro", SqlDbType.VarChar, 10).Value = filtro.Centro
                    If filtro.IdOrden > 0 Then .Add("@idOrden", SqlDbType.BigInt).Value = filtro.IdOrden
                End With

                Try
                    dtDatos = .ejecutarDataTable("ObtenerInfoDetalleCargaPedidoSAP", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

#End Region

    End Class

End Namespace