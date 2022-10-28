Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Namespace Pedidos

    Public Class TipoPedido

#Region "Atributos"

        Private _idTipo As Short
        Private _nombre As String
        Private _codigo As String
        Private _contabilizarEnCliente As Boolean
        Private _usaMatizDeTransporte As Boolean
        Private _idEstado As Short
        Private _registrado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
            _codigo = ""
        End Sub

        Public Sub New(ByVal idTipo As Short)
            Me.New()
            CargarDatos(idTipo)
        End Sub
#End Region

#Region "Propiedades"


        ''' <summary>
        ''' Propiedad que recupera y almacena el valor del identificador del tipo de pedido.
        ''' </summary>
        ''' <value>Valor de tipo Integer, que contiene el valor  que almacena el atributo _idTipoPedido.</value>
        ''' <returns>Recupera el valor contenido en el atributo _idTipoPedido.</returns>
        ''' <remarks></remarks>

        Public Property IdTipo() As Short
            Get
                Return _idTipo
            End Get
            Set(ByVal value As Short)
                _idTipo = value
            End Set
        End Property

        ''' <summary>
        ''' Propiedad que recupera y almacena el valor del nombre del tipo de pedido.
        ''' </summary>
        ''' <value>Valor de tipo String, que contiene el valor  que almacena el atributo _nombre.</value>
        ''' <returns>Recupera el valor contenido en el atributo _nombre.</returns>
        ''' <remarks></remarks>

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
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

        Public Property ContabilizarEnCliente() As Boolean
            Get
                Return _contabilizarEnCliente
            End Get
            Set(ByVal value As Boolean)
                _contabilizarEnCliente = value
            End Set
        End Property

        Public Property UsaMatrizDeTransporte() As Boolean
            Get
                Return _usaMatizDeTransporte
            End Get
            Set(ByVal value As Boolean)
                _usaMatizDeTransporte = value
            End Set
        End Property
        ''' <summary>
        ''' Propiedad que recupera y almacena el valor del estado del tipo de pedido.
        ''' </summary>
        ''' <value>Valor de tipo Integer, que contiene el valor  que almacena el atributo _idEstado.</value>
        ''' <returns>Recupera el valor contenido en el atributo _idEstado.</returns>
        ''' <remarks></remarks>

        Public Property IdEstado() As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        ''' <summary>
        ''' Propiedad que permite establecer o consultar si un Tipo de Pedido está registrado en la Base de Datos, 
        ''' es decir, ha sido previamente creado
        ''' </summary>
        ''' <value>Valor de tipo Boolean que contiene el valor a establecer</value>
        ''' <returns>Retorna el estado del Tipo de Pedido, de acuerdo con la información registrada en la Base de Datos</returns>
        ''' <remarks></remarks>
        Public Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
            Protected Friend Set(ByVal value As Boolean)
                _registrado = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal idTipo As Short)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = idTipo
                    .ejecutarReader("ObtenerTipoPedido", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Short.TryParse(.Reader("idTipoPedido").ToString, _idTipo)
                            _nombre = .Reader("nombre").ToString
                            Short.TryParse(.Reader("estado").ToString, _idEstado)
                            _codigo = .Reader("codigo").ToString
                            Boolean.TryParse(.Reader("contabilizarEnCliente").ToString, _contabilizarEnCliente)
                            Boolean.TryParse(.Reader("usaMatrizDeTransporte").ToString, _usaMatizDeTransporte)
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


#Region "MetodosPublicos"

        ''' <summary>
        ''' Metodo que recupera la información de los tipos de pedido de a cuerdo a unas variables de filtrado.
        ''' </summary>
        ''' <returns>Retorna un objeto de tipo DataTable que contiene la información del tipo de pedido.</returns>
        ''' <remarks>Permite reallizar la consulta de los diferentes tipos de pedido.</remarks>

        Public Shared Function ObtenerListado(ByVal filtro As FiltroTipoPedido) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                If filtro.ListaTipoPedido IsNot Nothing AndAlso filtro.ListaTipoPedido.Trim.Length > 0 Then _
                .SqlParametros.Add("@listaTipoPedido", SqlDbType.VarChar, 1000).Value = filtro.ListaTipoPedido
                If filtro.IdTipoPedido > 0 Then .SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = filtro.IdTipoPedido
                Try
                    dtDatos = .ejecutarDataTable("ObtenerTipoPedido", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        ''' <summary>
        '''  Metodo que recupera la información un tipo de pedido específico.
        ''' </summary>
        ''' <param name="idTipoPedido"></param>
        ''' <returns>Retorna un objeto de tipo DataTable que contiene la información del tipo de pedido</returns>
        ''' <remarks>Permite reallizar la consulta por identificador del tipo</remarks>

        Public Shared Function ObtenerPorId(ByVal idTipoPedido As Integer) As DataTable
            Dim filtro As New FiltroTipoPedido
            Dim dtDatos As DataTable
            Try
                filtro.IdTipoPedido = idTipoPedido
                dtDatos = ObtenerListado(filtro)
                Return dtDatos
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Function


#End Region

#Region "Enums"

        Public Enum Tipo
            SalidaDeVentas = 1
            SalidaDeTraslados = 2
            SalidaDeConsumos = 3
            EntregaDeProductoACuarentena = 4
            SalidaDeProductoPruebas = 5
            DespachoCuarentena = 6
            LiberacionCuarentena = 7
            LiberacionParaDespachoCuarentena = 8
            SalidaPedidoEspecial = 9
            PedidoServicioTecnico = 10
        End Enum
#End Region

    End Class

End Namespace
