Namespace OMS
    Public Class ClasificacionInstruccion
#Region "variables"
        Private _idClasificacionidClasificacion As Integer
        Private _descripcion As String
        Private _fechaCreacion As Date
        Private _estado As Boolean
        Private _idUsuario As Long
        Private _visiblePorCliente As Boolean
        Private _validarCantidades As Boolean
#End Region

#Region "Propiedades"
        Public Property IdClasificacion() As Integer
            Get
                Return _idClasificacionidClasificacion
            End Get
            Set(ByVal value As Integer)
                _idClasificacionidClasificacion = value
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

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As Date)
                _fechaCreacion = value
            End Set
        End Property

        Public Property Estado() As Boolean
            Get
                Return _estado
            End Get
            Set(ByVal value As Boolean)
                _estado = value
            End Set
        End Property

        Public Property ValidarCantidades() As Boolean
            Get
                Return _validarCantidades
            End Get
            Set(ByVal value As Boolean)
                _validarCantidades = value
            End Set
        End Property

        Public Property VisiblePorCliente() As Boolean
            Get
                Return _visiblePorCliente
            End Get
            Set(ByVal value As Boolean)
                _visiblePorCliente = value
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
#End Region

#Region "Metodos"
        Public Shared Function Obtener(ByVal incluirSoloReinstruccion As Boolean, ByVal visibleCliente As Boolean) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            If Not visibleCliente Then db.agregarParametroSQL("@visiblePorCliente", True, SqlDbType.Bit)
            If Not incluirSoloReinstruccion Then db.agregarParametroSQL("@incluirSoloReinstruccion", incluirSoloReinstruccion, SqlDbType.Bit)
            Return db.ejecutarDataTable("ObtenerClasificacionInstruccion", CommandType.StoredProcedure)
        End Function
        Public Shared Function ObtenerCompatibles(ByVal visibleCliente As Boolean, ByVal idClasificacion As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            If Not visibleCliente Then db.agregarParametroSQL("@visiblePorCliente", True, SqlDbType.Bit)
            ' If Not incluirSoloReinstruccion Then db.agregarParametroSQL("@incluirSoloReinstruccion", incluirSoloReinstruccion, SqlDbType.Bit)
            db.agregarParametroSQL("@idClasificacion", idClasificacion, SqlDbType.Int)
            Return db.ejecutarDataTable("ObtenerClasificacionInstruccionCompatibles", CommandType.StoredProcedure)
        End Function
#End Region



    End Class
End Namespace

