Namespace Despachos
    Public Class Detalle

#Region "Atributos"

        Protected _idDespacho As Integer
        Protected _material As String
        Protected _descripcion As String
        Protected _cantidadPedida As Integer
        Protected _cantidadLeida As Integer

#End Region

#Region "Propiedades"

        Public Property IdDespacho() As Integer
            Get
                Return _idDespacho
            End Get
            Set(ByVal value As Integer)
                _idDespacho = value
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

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
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

#End Region

#Region "Constructores"

        Public Sub New()
            Me._idDespacho = 0
            Me._material = ""
            Me._cantidadPedida = 0
            Me._cantidadLeida = 0
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Shared Function Obtener(ByVal idDespacho As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idDespacho", idDespacho, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerMaterialesDespacho", CommandType.StoredProcedure)
            Return dt
        End Function

#End Region

#Region "Métodos Privados"

#End Region

    End Class
End Namespace