Namespace Recibos
    Public Class DetallePallet
#Region "variables"
        Private _idDetallePallet As Long
        Private _idPallet As Long
        Private _idProducto As Long
        Private _cantidad As Integer
        Private _cantidadRecibida As Integer
        Private _idTipoUnidad As Integer
        Private _idOrdenBodega As Long
#End Region

#Region "propiedades"
        Public Property IdDetallePallet() As Long
            Get
                Return _idDetallePallet
            End Get
            Set(ByVal value As Long)
                _idDetallePallet = value
            End Set
        End Property
        Public Property IdPallet() As Long
            Get
                Return _idPallet
            End Get
            Set(ByVal value As Long)
                _idPallet = value
            End Set
        End Property
        Public Property IdProducto() As Long
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Long)
                _idProducto = value
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
        Public Property CantidadRecibida() As Integer
            Get
                Return _cantidadRecibida
            End Get
            Set(ByVal value As Integer)
                _cantidadRecibida = value
            End Set
        End Property
        Public Property IdTipoUnidad() As Integer
            Get
                Return _idTipoUnidad
            End Get
            Set(ByVal value As Integer)
                _idTipoUnidad = value
            End Set
        End Property
        Public Property IdOrdenBodega() As Long
            Get
                Return _idOrdenBodega
            End Get
            Set(ByVal value As Long)
                _idOrdenBodega = value
            End Set
        End Property
#End Region

#Region "constructores"
        Public Sub New(ByVal idDetallePallet As Long, ByVal idPallet As Long)

        End Sub
#End Region

#Region "metodos"
        Public Sub Crear()

        End Sub
        Public Sub Obtener()

        End Sub
        Public Sub Eliminar()

        End Sub
#End Region
    End Class
End Namespace

