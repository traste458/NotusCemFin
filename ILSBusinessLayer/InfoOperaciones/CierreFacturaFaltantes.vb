Imports LMDataAccessLayer
Public Class CierreFacturaFaltantes

#Region "Variables"
    Private _idOrdenEnvioLectura As UInteger
    Private _estadoInstruccion As string
    Private _idOrden As UInteger
    Private _idInstruccion As UInteger
    Private _idPreinstruccion As UInteger
    Private _factura As String
    Private _idFacturaGuia As UInteger
    Private _producto As String
    Private _cantidadInstruccionada As UInteger
    Private _ordenCompra As String
    Private _cantidadLeida As UInteger
    Private _cantidadFaltane As UInteger
    Private _idUsuario As Integer
    Private _contieneOrdenesActivas As UInteger
    Dim Errorgenerado As String
    Private _esImagen As Integer
#End Region
#Region "Propiedades"


    Public Property cantidadInstruccionada() As UInteger
        Get
            Return _cantidadInstruccionada
        End Get
        Set(ByVal Value As UInteger)
            _cantidadInstruccionada = Value
        End Set
    End Property

    Public Property Factura() As String
        Get
            Return _factura
        End Get
        Set(ByVal Value As String)
            _factura = Value
        End Set
    End Property

    Public Property producto() As String
        Get
            Return _producto
        End Get
        Set(ByVal Value As String)
            _producto = Value
        End Set
    End Property

    Public Property idInstruccion() As UInteger
        Get
            Return _idInstruccion
        End Get
        Set(ByVal Value As UInteger)
            _idInstruccion = Value
        End Set
    End Property

    Public Property idPreinstruccion() As UInteger
        Get
            Return _idPreinstruccion
        End Get
        Set(ByVal Value As UInteger)
            _idPreinstruccion = Value
        End Set
    End Property

    Public Property idFacturaGuia() As UInteger
        Get
            Return _idFacturaGuia
        End Get
        Set(ByVal Value As UInteger)
            _idFacturaGuia = Value
        End Set
    End Property

    Public Property idOrden() As UInteger
        Get
            Return _idOrden
        End Get
        Set(ByVal Value As UInteger)
            _idOrden = Value
        End Set
    End Property

    Public Property idOrdenEnvioLectura() As UInteger
        Get
            Return _idOrdenEnvioLectura
        End Get
        Set(ByVal Value As UInteger)
            _idOrdenEnvioLectura = Value
        End Set
    End Property

    Public Property ordenCompra() As String
        Get
            Return _ordenCompra
        End Get
        Set(ByVal value As String)
            _ordenCompra = value
        End Set
    End Property

    Property CantidaLeida() As UInteger
        Get
            Return _cantidadLeida
        End Get
        Set(ByVal value As UInteger)
            _cantidadLeida = value
        End Set
    End Property

    Property CantidadFaltane() As UInteger
        Get
            Return _cantidadFaltane
        End Get
        Set(ByVal value As UInteger)
            _cantidadFaltane = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal Value As Integer)
            _idUsuario = Value
        End Set
    End Property

    Public Property ContieneOrdenesActivas() As UInteger
        Get
            Return _contieneOrdenesActivas
        End Get
        Set(ByVal value As UInteger)
            _contieneOrdenesActivas = value
        End Set
    End Property

    Public Property EsImagen() As Integer
        Get
            Return _esImagen
        End Get
        Set(value As Integer)
            _esImagen = value
        End Set
    End Property

#End Region
#Region "Metodos Publicos"
    Private Function fillData(ByVal reader As IDataReader) As CierreFacturaFaltantes
        Try

            Dim _data As New CierreFacturaFaltantes()
            _data.idPreinstruccion = CInt(If(reader("idPreinstruccion") Is DBNull.Value, 0, CType(reader("idPreinstruccion"), Integer)))
            _data.idOrden = CInt(If(reader("idOrdenCompra") Is DBNull.Value, 0, CType(reader("idOrdenCompra"), Integer)))
            _data.Factura = CStr(If(reader("factura") Is DBNull.Value, Nothing, CType(reader("factura"), String)))
            _data.ordenCompra = CStr(If(reader("ordenCompra") Is DBNull.Value, Nothing, CType(reader("ordenCompra"), String)))
            _data.idOrdenEnvioLectura = CInt(If(reader("idOrdenEnvioLectura") Is DBNull.Value, 0, CType(reader("idOrdenEnvioLectura"), Integer)))
            _data.producto = CStr(If(reader("producto") Is DBNull.Value, Nothing, CType(reader("producto"), String)))
            _data.cantidadInstruccionada = CInt(If(reader("cantidadInstruccionada") Is DBNull.Value, 0, CType(reader("cantidadInstruccionada"), Integer)))
            _data.CantidaLeida = CStr(If(reader("cantidadLeida") Is DBNull.Value, Nothing, CType(reader("cantidadLeida"), String)))
            _data.ContieneOrdenesActivas = CInt(If(reader("contieneOrdenesActivas") Is DBNull.Value, 0, CType(reader("contieneOrdenesActivas"), Integer)))
            Return _data

        Catch ex As Exception
            Errorgenerado = "Error al le er los datos proProvenientes de la consulta " + ex.Message
            Throw New Exception("Error al le er los datos proProvenientes de la consulta " + ex.Message)
        End Try
    End Function

    Public Function ConsultaSerial(ByVal idOrden As UInteger, ByVal idPreinstruccion As UInteger, ByVal Factura As String) As IList(Of CierreFacturaFaltantes)
        Dim reader As IDataReader = Nothing
        Dim db As New LMDataAccess
        Try
            If idOrden > 0 Then db.SqlParametros.Add("@idOrden", SqlDbType.Int).Value = idOrden
            If idPreinstruccion > 0 Then db.SqlParametros.Add("@idPreinstruccion", SqlDbType.Int).Value = idPreinstruccion
            If Not String.IsNullOrEmpty(Factura) Then db.SqlParametros.Add("@factura", SqlDbType.VarChar).Value = Factura
            db.TiempoEsperaComando = 900
            reader = db.ejecutarReader("ObtenerInstruccionesCierreFacturaFaltante", CommandType.StoredProcedure)
            'If reader.Read() Then
            Dim coll As IList(Of CierreFacturaFaltantes) = New List(Of CierreFacturaFaltantes)()
            While reader.Read()
                coll.Add(fillData(reader))

            End While

            Return coll

        Catch ex As Exception
            Throw New Exception(Errorgenerado + ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try

    End Function

    Public Function Cerrarinstruccion(ByVal datos As IList(Of CierreFacturaFaltantes), ByVal Justificacion As String, _
                                      ByVal IdUsuario As Integer) As Boolean
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim arrNuevosSeriales As New ArrayList
        Try
            db.iniciarTransaccion()
            For Each row As CierreFacturaFaltantes In datos
                db.SqlParametros.Add("@idPreinstruccion", SqlDbType.Int).Value = row.idPreinstruccion
                db.SqlParametros.Add("@idOrdenEnvioLectura", SqlDbType.Int).Value = row.idOrdenEnvioLectura
                db.SqlParametros.Add("@cantidadPedida", SqlDbType.Int).Value = row.cantidadInstruccionada
                db.SqlParametros.Add("@cantidadFaltante", SqlDbType.Int).Value = row.CantidadFaltane
                db.SqlParametros.Add("@idusuario", SqlDbType.Int).Value = IdUsuario
                db.SqlParametros.Add("@Justificacion", SqlDbType.VarChar).Value = Justificacion
                db.ejecutarNonQuery("CierreFacturaconFaltantes", CommandType.StoredProcedure)

                db.SqlParametros.Clear()
            Next

            db.confirmarTransaccion()
            Return True
        Catch ex As Exception
            db.abortarTransaccion()
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try

    End Function

    Public Function RegistrarImagenesCierre(ByVal datos As IList(Of CierreFacturaFaltantes), ByVal _nombreArchivo As String)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim arrNuevosSeriales As New ArrayList
        Try
            db.iniciarTransaccion()
            For Each row As CierreFacturaFaltantes In datos
                db.SqlParametros.Add("@idPreinstruccion", SqlDbType.Int).Value = row.idPreinstruccion
                db.SqlParametros.Add("@nombreArchivo", SqlDbType.VarChar).Value = _nombreArchivo
                db.SqlParametros.Add("@esImagen", SqlDbType.Bit).Value = _esImagen
                db.SqlParametros.Add("@idusuario", SqlDbType.Int).Value = IdUsuario
                db.ejecutarNonQuery("RegistrarImagenesFacturaconFaltantes", CommandType.StoredProcedure)
                db.SqlParametros.Clear()
            Next
            db.confirmarTransaccion()
            Return True
        Catch ex As Exception
            db.abortarTransaccion()
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
    End Function
#End Region
End Class
