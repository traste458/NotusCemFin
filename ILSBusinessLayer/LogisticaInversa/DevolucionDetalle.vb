Namespace LogisticaInversa
    Public Class DevolucionDetalle
#Region "Variables"
        Private _iddetalle As Integer
        Private _iddevolucion As Integer
        Private _idproducto As Integer
        Private _cantidad As Integer
        Private _cantidad_leida As Integer
        Private _observaciones As String
        Private _seriales As DevolucionDetalleSerial
        Private _rechazado As DevolucionSerialRechazado
#End Region

#Region "Propiedades"

        Public Property Rechazado() As DevolucionSerialRechazado
            Get
                Return _rechazado
            End Get
            Set(ByVal value As DevolucionSerialRechazado)
                _rechazado = value
            End Set
        End Property

        Public Property Seriales() As DevolucionDetalleSerial
            Get
                Return _seriales
            End Get
            Set(ByVal value As DevolucionDetalleSerial)
                _seriales = value
            End Set
        End Property

        Public Property Observaciones() As String
            Get
                Return _observaciones
            End Get
            Set(ByVal value As String)
                _observaciones = value
            End Set
        End Property

        Public Property Cantidad_leida() As Integer
            Get
                Return _cantidad_leida
            End Get
            Set(ByVal value As Integer)
                _cantidad_leida = value
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

        Public Property IdProducto() As Integer
            Get
                Return _idproducto
            End Get
            Set(ByVal value As Integer)
                _idproducto = value
            End Set
        End Property

        Public Property IdDetalle() As Integer
            Get
                Return _iddetalle
            End Get
            Set(ByVal value As Integer)
                _iddetalle = value
            End Set
        End Property

        Public Property IdDevolucion() As Integer
            Get
                Return _iddevolucion
            End Get
            Set(ByVal value As Integer)
                _iddevolucion = value
            End Set
        End Property

#End Region

#Region "Metodos"
        Public Shared Function ObtenerDetalleDevolucion(ByVal idDevolucion As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idDevolucion", idDevolucion, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarDetalledevolucion", CommandType.StoredProcedure)
            Return dt
        End Function
#End Region

        Public Sub New()
            _seriales = New DevolucionDetalleSerial
            _rechazado = New DevolucionSerialRechazado
        End Sub
    End Class
End Namespace

