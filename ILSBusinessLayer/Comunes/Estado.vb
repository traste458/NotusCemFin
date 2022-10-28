Public Class Estado
#Region "Variables"
    Private _idEstado As String
    Private _descripcion As String
    Private _idEntidad As String
#End Region

#Region "Propiedades"
    Public Property IdEntidad() As String
        Get
            Return _idEntidad
        End Get
        Set(ByVal value As String)
            _idEntidad = value
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
    Public Property IdEstado() As String
        Get
            Return _idEstado
        End Get
        Set(ByVal value As String)
            _idEstado = value
        End Set
    End Property
#End Region

#Region "Metodos"
    Private Sub CargarDatos(ByVal idEstado As Integer)
        Dim db As New LMDataAccessLayer.LMDataAccess
        db.agregarParametroSQL("@idEstado", idEstado, SqlDbType.Int)
        Try
            db.ejecutarReader("SeleccionarEstados", CommandType.StoredProcedure)
            While db.Reader.Read()

                _idEstado = db.Reader("idEstado")
                _descripcion = db.Reader("nombre")
                _idEntidad = db.Reader("idEntidad")

            End While
        Catch ex As Exception
            Throw New Exception("Imposible obtener el Estado correspondiente al ID especificado")
        Finally
            If Not db.Reader.IsClosed Then db.Reader.Close()
            db.cerrarConexion()
        End Try
    End Sub
    Public Shared Function Obtener(ByVal idEntidad As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.agregarParametroSQL("@idEntidad", idEntidad, SqlDbType.Int)
        Return db.ejecutarDataTable("SeleccionarEstados", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerListadoPorIds(ByVal listaEstados As String) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        db.agregarParametroSQL("@listaEstados", listaEstados, SqlDbType.VarChar, 1000)
        Return db.ejecutarDataTable("SeleccionarEstados", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerListadoPorTipologia(ByVal idTipo As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        db.agregarParametroSQL("@idTipo", idTipo, SqlDbType.Int)
        Return db.ejecutarDataTable("SeleccionarEstadoPorTipologia", CommandType.StoredProcedure)
    End Function

#End Region

    Public Sub New()

    End Sub
    Public Sub New(ByVal idEstado As Integer)
        Me.New()
        Me.CargarDatos(idEstado)
    End Sub
End Class
