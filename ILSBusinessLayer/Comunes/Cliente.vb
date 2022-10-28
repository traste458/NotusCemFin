Public Class Cliente

#Region "Variables"
    Private _idcliente As Integer
    Private _codigoCliente As String
    Private _centro As String
    Private _almacen As String
    Private _dealer As String
    Private _nombre As String
    Private _idciudad As Integer
    Private _ciudad As String
    Private _direccion As String
    Private _telefonos As String
    Private _email As String
    Private _gerente As String
    Private _gerente_cel As String
    Private _estado As Boolean
    Private _idRegion As Integer
    Private _region As String
    Private _idTipoDestinatario As Integer
    Private _tipoDestinatario As String
    Private _nit As String


#End Region

#Region "Propiedades"

    Public Property Almacen() As String
        Get
            Return _almacen
        End Get
        Set(ByVal value As String)
            _almacen = value
        End Set
    End Property

    Public Property Centro() As String
        Get
            Return _centro
        End Get
        Set(ByVal value As String)
            _centro = value
        End Set
    End Property

    Public Property CodigoCliente() As String
        Get
            Return _codigoCliente
        End Get
        Set(ByVal value As String)
            _codigoCliente = value
        End Set
    End Property

    Public Property Nit() As String
        Get
            Return _nit
        End Get
        Set(ByVal value As String)
            _nit = value
        End Set
    End Property

    Public Property IdTipoDestinatario() As Integer
        Get
            Return _idTipoDestinatario
        End Get
        Set(ByVal value As Integer)
            _idTipoDestinatario = value
        End Set
    End Property

    Public Property TipoDestinatario() As String
        Get
            Return _tipoDestinatario
        End Get
        Set(ByVal value As String)
            _tipoDestinatario = value
        End Set
    End Property

    Public Property Region() As String
        Get
            Return _region
        End Get
        Set(ByVal value As String)
            _region = value
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

    Public Property GerenteCelular() As String
        Get
            Return _gerente_cel
        End Get
        Set(ByVal value As String)
            _gerente_cel = value
        End Set
    End Property

    Public Property Gerente() As String
        Get
            Return _gerente
        End Get
        Set(ByVal value As String)
            _gerente = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    Public Property Telefonos() As String
        Get
            Return _telefonos
        End Get
        Set(ByVal value As String)
            _telefonos = value
        End Set
    End Property

    Public Property Direccion() As String
        Get
            Return _direccion
        End Get
        Set(ByVal value As String)
            _direccion = value
        End Set
    End Property

    Public Property IdCiudad() As Integer
        Get
            Return _idciudad
        End Get
        Set(ByVal value As Integer)
            _idciudad = value
        End Set
    End Property

    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property Dealer() As String
        Get
            Return _dealer
        End Get
        Set(ByVal value As String)
            _dealer = value
        End Set
    End Property

    Public Property IdCliente() As Integer
        Get
            Return _idcliente
        End Get
        Set(ByVal value As Integer)
            _idcliente = value
        End Set
    End Property

    Public Property IdRegion() As Integer
        Get
            Return _idRegion
        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Property Ciudad() As String
        Get
            Return _ciudad
        End Get
        Set(ByVal value As String)
            _ciudad = value
        End Set
    End Property
#End Region

#Region "Metodos"

    Private Sub CargarDatos(ByVal db As LMDataAccessLayer.LMDataAccess)
        Try
            With db
                .ejecutarReader("ConsultarCliente", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        _idcliente = .Reader("idcliente")
                        _codigoCliente = .Reader("idcliente2").ToString
                        _dealer = .Reader("dealer").ToString
                        _nombre = .Reader("cliente").ToString
                        _direccion = .Reader("direccion").ToString
                        _telefonos = .Reader("telefonos").ToString
                        _email = .Reader("email").ToString
                        _gerente = .Reader("gerente").ToString
                        _gerente_cel = .Reader("gerente_cel").ToString
                        _dealer = .Reader("dealer").ToString
                        _nit = .Reader("nit").ToString
                        _estado = .Reader("estado").ToString
                        _region = .Reader("region").ToString
                        _ciudad = .Reader("ciudad").ToString
                        _tipoDestinatario = .Reader("tipoDestinatario").ToString
                        Integer.TryParse(.Reader("idRegion").ToString(), _idRegion)
                        Integer.TryParse(.Reader("idCiudad").ToString(), _idciudad)
                        Integer.TryParse(.Reader("centro").ToString(), _centro)
                        Integer.TryParse(.Reader("almacen").ToString(), _almacen)
                        Integer.TryParse(.Reader("idTipoDestinatario").ToString(), _idTipoDestinatario)
                    End If
                End If
                .Reader.Close()
            End With
        Finally
            db.Dispose()
        End Try
    End Sub

    Public Sub Crear()
        Dim db As New LMDataAccessLayer.LMDataAccess
        With db
            .SqlParametros.Add("@idCliente", SqlDbType.Int).Direction = ParameterDirection.Output
            .agregarParametroSQL("@dealer", _dealer)
            .agregarParametroSQL("@cliente", _nombre)
            .agregarParametroSQL("@idCliente2", _codigoCliente)
            .agregarParametroSQL("@centro", _centro, SqlDbType.Int)
            .agregarParametroSQL("@almacen", _almacen, SqlDbType.Int)
            .agregarParametroSQL("@idciudad", _idciudad, SqlDbType.Int)
            .agregarParametroSQL("@direccion", _direccion)
            .agregarParametroSQL("@telefonos", _telefonos)
            .agregarParametroSQL("@email", _email)
            .agregarParametroSQL("@gerente", _gerente)
            .agregarParametroSQL("@gerente_cel", _gerente_cel)
            .agregarParametroSQL("@estado", True, SqlDbType.Bit)
            .agregarParametroSQL("@idTipoDestinatario", _idTipoDestinatario, SqlDbType.Int)
            .agregarParametroSQL("@nit", _nit)
            .SqlParametros.Add("@codigoError", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            .ejecutarNonQuery("CrearCliente", CommandType.StoredProcedure)
            If .SqlParametros("@codigoError").Value <> 0 Then Throw New Exception(.SqlParametros("@codigoError").Value)
        End With
    End Sub

    Public Sub Actualizar()
        Dim db As New LMDataAccessLayer.LMDataAccess
        With db
            With .SqlParametros
                .Clear()
                If _idcliente > 0 Then _
                    .Add("@idCliente", SqlDbType.BigInt).Value = _idcliente
                If Not String.IsNullOrEmpty(_dealer) Then _
                    .Add("@dealer", SqlDbType.VarChar, 20).Value = _dealer
                If Not String.IsNullOrEmpty(_nombre) Then _
                    .Add("@cliente", SqlDbType.VarChar, 70).Value = _nombre
                If _idciudad > 0 Then _
                    .Add("@idciudad", SqlDbType.BigInt).Value = _idciudad
                If Not String.IsNullOrEmpty(_direccion) Then _
                    .Add("@direccion", SqlDbType.VarChar, 100).Value = _direccion
                If Not String.IsNullOrEmpty(_telefonos) Then _
                    .Add("@telefonos", SqlDbType.VarChar, 20).Value = _telefonos
                If Not String.IsNullOrEmpty(_email) Then _
                    .Add("@email", SqlDbType.VarChar, 50).Value = _email
                If Not String.IsNullOrEmpty(_gerente) Then _
                    .Add("@gerente", SqlDbType.VarChar, 60).Value = _gerente
                If Not String.IsNullOrEmpty(_gerente_cel) Then _
                    .Add("@gerente_cel", SqlDbType.VarChar, 15).Value = _gerente_cel
                .Add("@estado", SqlDbType.Int).Value = _estado
                If Not String.IsNullOrEmpty(_region) Then _
                    .Add("@region", SqlDbType.VarChar, 10).Value = _region
                If _idTipoDestinatario > 0 Then _
                    .Add("@idTipoDestinatario", SqlDbType.BigInt).Value = _idTipoDestinatario
                If Not String.IsNullOrEmpty(_nit) Then _
                    .Add("@nit", SqlDbType.VarChar, 100).Value = _nit
                If Not String.IsNullOrEmpty(_codigoCliente) Then _
                     .Add("@idCliente2", SqlDbType.VarChar, 100).Value = _codigoCliente
                If Not String.IsNullOrEmpty(_centro) Then _
                    .Add("@centro", SqlDbType.VarChar, 10).Value = _centro
                If Not String.IsNullOrEmpty(_almacen) Then _
                    .Add("@almacen", SqlDbType.VarChar, 10).Value = _almacen

            End With
            .ejecutarNonQuery("ActualizarCliente", CommandType.StoredProcedure)
        End With
    End Sub

    Public Shared Function Consultar(ByVal filtro As FiltroCliente) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        With filtro
            If .idCliente > 0 Then db.agregarParametroSQL("@idCliente", .idCliente, SqlDbType.Int)
            If .centro > 0 Then db.agregarParametroSQL("@centro", .centro, SqlDbType.Int)
            If .almacen > 0 Then db.agregarParametroSQL("@almacen", .almacen, SqlDbType.Int)
            If .CodigoCliente <> "" Then db.agregarParametroSQL("@idCliente2", .CodigoCliente)
            If .cliente <> "" Then db.agregarParametroSQL("@cliente", .cliente)
            If .idCiudad > 0 Then db.agregarParametroSQL("@idCiudad", .idCiudad, SqlDbType.Int)
            If .estado > 0 Then db.agregarParametroSQL("@estado", (.estado = 1), SqlDbType.Bit)
            If .idRegion > 0 Then db.agregarParametroSQL("@idRegion", .idRegion)
            If .idTipoDestinatario > 0 Then db.agregarParametroSQL("@idTipoDestinatario", .idTipoDestinatario)
            If .nit <> "" Then db.agregarParametroSQL("@nit", .nit)
            If .conCentroAlmacen = 1 Then db.agregarParametroSQL("@conCentroAlmacen", .conCentroAlmacen, SqlDbType.SmallInt)
            If .filtroRapido IsNot Nothing AndAlso .filtroRapido.Trim.Length >= 3 Then _
                db.SqlParametros.Add("@filtroRapido", SqlDbType.VarChar, 50).Value = .filtroRapido
        End With
        Dim dt As DataTable = db.ejecutarDataTable("ConsultarCliente", CommandType.StoredProcedure)
        Return dt
    End Function

    Public Structure FiltroCliente
        Dim idCliente As Integer
        Dim cliente As String
        Dim idCiudad As Integer
        Dim estado As Enumerados.EstadoBinario
        Dim idRegion As Integer
        Dim idTipoDestinatario As String
        Dim nit As String
        Dim CodigoCliente As String
        Dim centro As Integer
        Dim almacen As Integer
        Dim filtroRapido As String
        Dim conCentroAlmacen As Short
    End Structure
#End Region

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idCliente As Integer)
        Dim db As New LMDataAccessLayer.LMDataAccess
        db.agregarParametroSQL("@idCliente", idCliente, SqlDbType.Int)
        Me.CargarDatos(db)
    End Sub

End Class
