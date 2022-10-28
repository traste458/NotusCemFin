Public Class DevolucionSerialRechazado

#Region "Variables"
    Private _idRechazado As Integer
    Private _idDevolucion As Integer
    Private _serial As String
    Private _fecha As Date
    Private _idDetalle As Integer
    Private _idProducto As Integer
#End Region

#Region "Propiedades"
    Public Property IdProducto() As Integer
        Get
            Return _idProducto
        End Get
        Set(ByVal value As Integer)
            _idProducto = value
        End Set
    End Property
    Public Property IdDetalle() As Integer
        Get
            Return _idDetalle
        End Get
        Set(ByVal value As Integer)
            _idDetalle = value
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
    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
        End Set
    End Property
    Public Property IdDevolucion() As Integer
        Get
            Return _idDevolucion
        End Get
        Set(ByVal value As Integer)
            _idDevolucion = value
        End Set
    End Property
    Public Property IdRechazado() As Integer
        Get
            Return _idRechazado
        End Get
        Set(ByVal value As Integer)
            _idRechazado = value
        End Set
    End Property
#End Region

#Region "Metodos"
    Public Shared Function ObtenerListado(ByVal idDevolucion As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        db.agregarParametroSQL("@idDevolucion", idDevolucion, SqlDbType.Int)
        Dim dt As DataTable = db.ejecutarDataTable("ObtenerDevolucionSerialRechazado", CommandType.StoredProcedure)
        Return dt
    End Function

    Public Sub Registrar()
        Dim db As New LMDataAccessLayer.LMDataAccess
        With db
            .agregarParametroSQL("@serial", Serial)
            .agregarParametroSQL("@idDevolucion", _idDevolucion, SqlDbType.Int)
            .SqlParametros.Add("@codigoError", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            .ejecutarNonQuery("InsertarDevolucionSerialRechazado", CommandType.StoredProcedure)
            If .SqlParametros("@codigoError").Value <> 0 Then Throw New Exception(.SqlParametros("@codigoError").Value)
        End With
    End Sub

    Public Sub Eliminar()
        Dim db As New LMDataAccessLayer.LMDataAccess
        With db
            .agregarParametroSQL("@serial", _serial)
            .agregarParametroSQL("@idDevolucion", _idDevolucion, SqlDbType.Int)
            .ejecutarNonQuery("EliminarDevolucionSerialRechazado", CommandType.StoredProcedure)
        End With
    End Sub

#End Region

End Class
