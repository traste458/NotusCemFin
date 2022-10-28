Namespace Inventario

    Public Class DetalleProductoBloqueo

#Region "Atributos"

        Private _idBloqueoDetalleProducto As Integer
        Private _idBloqueo As Integer
        Private _idProducto As Integer
        Private _material As String
        Private _cantidad As Integer
        Private _subproducto As String

        'Atributos para indicar el estado del item
        Private _registrado As Boolean
        Private _accion As Enumerados.AccionItem
#End Region

#Region "Propiedades"

        Public Property IdBloqueoDetalleProducto() As Integer
            Get
                Return _idBloqueoDetalleProducto
            End Get
            Set(ByVal value As Integer)
                _idBloqueoDetalleProducto = value
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

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
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

        Public Property Cantidad() As Integer
            Get
                Return _cantidad
            End Get
            Set(ByVal value As Integer)
                _cantidad = value
            End Set
        End Property

        Public Property Subproducto() As String
            Get
                Return _subproducto
            End Get
            Protected Friend Set(ByVal value As String)
                _subproducto = value
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

        Public Property Accion() As Enumerados.AccionItem
            Get
                Return _accion
            End Get
            Set(ByVal value As Enumerados.AccionItem)
                _accion = value
            End Set
        End Property

#End Region

    End Class

End Namespace


