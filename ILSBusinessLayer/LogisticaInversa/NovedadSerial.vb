Namespace LogisticaInversa
    Public Class NovedadSerial
        Public Enum ProcesoNovedad
            NoEstablecido = 0
            Recoleccion = 3
            Entrega = 4
            Devolucion = 5
        End Enum
#Region "variables"
        Private _listaCodigosNovedad As List(Of Integer)
        Private _idRegistroSerial As Integer
        Private _codigoNovedad As Integer
        Private _novedad As String
#End Region

#Region "Propiedades"

        Public Property Novedad() As String
            Get
                Return _codigoNovedad
            End Get
            Set(ByVal value As String)
                _novedad = value
            End Set
        End Property
        Public Property CodigoNovedad() As Integer
            Get
                Return _codigoNovedad
            End Get
            Set(ByVal value As Integer)
                _codigoNovedad = value
            End Set
        End Property

        Public Property IdRegistroSerial() As Integer
            Get
                Return _idRegistroSerial
            End Get
            Set(ByVal value As Integer)
                _idRegistroSerial = value
            End Set
        End Property

        Public Property ListaCodigosNovedad() As List(Of Integer)
            Get
                Return _listaCodigosNovedad
            End Get
            Set(ByVal value As List(Of Integer))
                _listaCodigosNovedad = value
            End Set
        End Property

#End Region

#Region "Metodos"
        Friend Sub Registrar(ByVal proceso As ProcesoNovedad, ByVal db As LMDataAccessLayer.LMDataAccess)
            db.SqlParametros.Clear()
            db.agregarParametroSQL("@idregistroSerial", _idRegistroSerial, SqlDbType.Int)
            db.agregarParametroSQL("@idProceso", proceso, SqlDbType.Int)
            db.SqlParametros.Add("@codigoNovedad", SqlDbType.Int)
            If _listaCodigosNovedad IsNot Nothing Then
                For Each codigoNovedad As Integer In _listaCodigosNovedad
                    db.SqlParametros("@CodigoNovedad").Value = codigoNovedad
                    db.ejecutarNonQuery("RegistrarNovedadSerial", CommandType.StoredProcedure)
                Next
            End If
        End Sub

        Public Shared Function ObtenerNovedadesSerial(ByVal proceso As ProcesoNovedad, ByVal idRegistroSerial As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idRegistroSerial", idRegistroSerial, SqlDbType.Int)
            db.agregarParametroSQL("@idProceso", proceso, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerNovedadesSerial", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ObtenerClasificacionNovedades() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarClasificacionNovedadRecoleccionLI", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ObtenerNovedadesTransporte(ByVal idProcesonovedad As ProcesoNovedad, ByVal idClasificacion As Integer) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idProcesonovedad", idProcesonovedad, SqlDbType.Int)
            db.agregarParametroSQL("@idClasificacion", idClasificacion, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarNovedadTransporte", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ConsultarPorRecoleccion(ByVal idOrdenRecoleccion As Integer)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idRecoleccion", idOrdenRecoleccion, SqlDbType.Int)
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerNovedadesSerial", CommandType.StoredProcedure)
            Return dt
        End Function
#End Region
    End Class
End Namespace
