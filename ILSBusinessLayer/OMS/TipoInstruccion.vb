Namespace OMS
    Public Class TipoInstruccion

#Region "variables"
        Private _idTipoInstruccion As Integer
        Private _idClasificacion As Integer
        Private _descripcion As String
        Private _idTipoOrden As Integer
        Private _reinstruccionable As Boolean
        Private _posicionOrdinal As Integer
        Private _prefijo As String
#End Region

#Region "Propiedades"

        Public Property Prefijo() As String
            Get
                Return _prefijo
            End Get
            Set(ByVal value As String)
                _prefijo = value
            End Set
        End Property

        Public Property PosicionOrdinal() As Integer
            Get
                Return _posicionOrdinal
            End Get
            Set(ByVal value As Integer)
                _posicionOrdinal = value
            End Set
        End Property

        Public Property EsReinstruccionable() As Boolean
            Get
                Return _reinstruccionable
            End Get
            Set(ByVal value As Boolean)
                _reinstruccionable = value
            End Set
        End Property

        Public Property IdTipoOrden() As Integer
            Get
                Return _idTipoOrden
            End Get
            Set(ByVal value As Integer)
                _idTipoOrden = value
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

        Public Property IdClasificacion() As Integer
            Get
                Return _idClasificacion
            End Get
            Set(ByVal value As Integer)
                _idClasificacion = value
            End Set
        End Property

        Public ReadOnly Property IdTipoInstruccion() As Integer
            Get
                Return _idTipoInstruccion
            End Get
        End Property

#End Region

#Region "Constructores"
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idTipoInstruccion As Integer)
            Me.CargarDatos(idTipoInstruccion)
        End Sub
#End Region

#Region "Metodos"
        Public Shared Function Obtener(ByVal idClasificacion As Integer, ByVal mostrarOcultos As Boolean) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            If Not mostrarOcultos Then db.agregarParametroSQL("@visiblePorCliente", True, SqlDbType.Bit)
            If idClasificacion > 0 Then db.agregarParametroSQL("@idClasificacion", idClasificacion, SqlDbType.Int)
            Return db.ejecutarDataTable("ObtenerTipoInstruccion", CommandType.StoredProcedure)
        End Function

        Private Sub CargarDatos(ByVal idTipoInstruccion As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idTipoInstruccion", idTipoInstruccion, SqlDbType.Int)
            Try
                db.ejecutarReader("ObtenerTipoInstruccion", CommandType.StoredProcedure)
                If db.Reader IsNot Nothing AndAlso db.Reader.Read Then
                    _idTipoInstruccion = db.Reader("idTipoInstruccion")
                    _idClasificacion = db.Reader("idClasificacion")
                    _descripcion = db.Reader("descripcion").ToString()
                    _idTipoOrden = db.Reader("idTipoOrden")
                    _reinstruccionable = db.Reader("reinstruccionable")
                    _posicionOrdinal = db.Reader("posicionOrdinal")
                    _prefijo = db.Reader("prefijo")
                End If
            Finally
                If Not db.Reader.IsClosed Then db.Reader.Close()
                db.Dispose()
            End Try
        End Sub
#End Region

    End Class
End Namespace

