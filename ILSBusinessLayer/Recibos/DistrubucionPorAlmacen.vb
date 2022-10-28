Namespace Recibos
    Public Class DistrubucionPorAlmacen
#Region "variables"
        Private _idDistribucion As Long
        Private _idDetalleOrdenCompra As Long
        Private _idRegion As Integer
        Private _cantidad As Integer
        Private _idUsuario As Long
        Private _fechaRegistro As Date
#End Region

#Region "propiedades"
        Public Property IdDistribucion() As Long
            Get
                Return _idDistribucion
            End Get
            Set(ByVal value As Long)
                _idDistribucion = value
            End Set
        End Property
        Public Property IdDetalleOrdenCompra() As Long
            Get
                Return _idDetalleOrdenCompra
            End Get
            Set(ByVal value As Long)
                _idDetalleOrdenCompra = value
            End Set
        End Property
        Public Property IdRegion() As Integer
            Get
                Return _idRegion
            End Get
            Set(ByVal value As Integer)
                _idRegion = value
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
        Public Property IdUsuario() As Long
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Long)
                _idUsuario = value
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
#End Region

#Region "constructores"
        Public Sub New(ByVal idDistribucion As Long)

        End Sub
        Public Sub New(ByVal idDistribucion As Long, ByVal idDetalleOrdenCompra As Long)

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

