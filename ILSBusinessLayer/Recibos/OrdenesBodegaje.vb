Namespace Recibos
    Public Class OrdenesBodegaje
#Region "variables"
        Private _idOrdenBodegaje As Long
        Private _idOrdenBodegaje2 As String
        Private _fecha As Date
        Private _idBodega As Long
        Private _idPosicion As Long
        Private _idAcomodador As Long
        Private _estado As Integer
        Private _estado_imp As Integer
        Private _fechaAcomodacion As Date
        Private _idUsuarioAcomodador As Long
        Private _idProducto As Long
        Private _material As String
        Private _cantidad As Integer
#End Region

#Region "propiedades"
        Public Property IdOrdenBodegaje() As Long
            Get
                Return _idOrdenBodegaje
            End Get
            Set(ByVal value As Long)
                _idOrdenBodegaje = value
            End Set
        End Property
        Public Property IdOrdenBodegaje2() As Long
            Get
                Return _idOrdenBodegaje2
            End Get
            Set(ByVal value As Long)
                _idOrdenBodegaje2 = value
            End Set
        End Property
        Public Property Fecha() As Date
            Get
                Return _fecha
            End Get
            Set(ByVal value As Date)
                _fecha = value
            End Set
        End Property
        Public Property IdBodega() As Long
            Get
                Return _idBodega
            End Get
            Set(ByVal value As Long)
                _idBodega = value
            End Set
        End Property
        Public Property IdPosicion() As Long
            Get
                Return _idPosicion
            End Get
            Set(ByVal value As Long)
                _idPosicion = value
            End Set
        End Property
        Public Property IdAcomodador() As Long
            Get
                Return _idAcomodador
            End Get
            Set(ByVal value As Long)
                _idAcomodador = value
            End Set
        End Property
        Public Property Estado() As Integer
            Get
                Return _estado
            End Get
            Set(ByVal value As Integer)
                _estado = value
            End Set
        End Property
        Public Property Estado_imp() As Integer
            Get
                Return _estado_imp
            End Get
            Set(ByVal value As Integer)
                _estado_imp = value
            End Set
        End Property
        Public Property FechaAcomodacion() As Date
            Get
                Return _fechaAcomodacion
            End Get
            Set(ByVal value As Date)
                _fechaAcomodacion = value
            End Set
        End Property
        Public Property IdUsuarioAcomodador() As Long
            Get
                Return _idUsuarioAcomodador
            End Get
            Set(ByVal value As Long)
                _idUsuarioAcomodador = value
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
#End Region

#Region "constructores"

#End Region

#Region "metodos"
        Public Sub Crear()

        End Sub
        Public Sub Obtener()

        End Sub
#End Region
    End Class
End Namespace
