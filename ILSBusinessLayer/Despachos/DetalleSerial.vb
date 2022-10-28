Namespace Despachos
    Public Class DetalleSerial
#Region "Atributos"
        Private _idDespacho As Integer
        Private _material As String
        Private _serial As String
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

        Public Property Serial() As String
            Get
                Return _serial
            End Get
            Set(ByVal value As String)
                _serial = value
            End Set
        End Property
#End Region

        Public Sub New()
            _idDespacho = 0
            _material = ""
            _serial = 0
        End Sub


        Public Shared Function Obtener(ByVal idDespacho As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idDespacho", idDespacho, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("SeleccionarDetalleDespacho", CommandType.StoredProcedure)
            Return dt
        End Function
    End Class
End Namespace

