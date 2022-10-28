Namespace LogisticaInversa
    Public Class Sucursal

#Region "Campos"

        Private _idSucursal As Integer
        Private _nombre As String
        Private _idCiudad As Integer
        Private _centro As Integer
        Private _almacen As Integer
        Private _codigo As String
        Private _nombreContacto As String
        Private _direccion As String
        Private _email As String
        Private _telefono As String
        Private _ciudad As String
        Private _idPadre As Integer

#End Region

#Region "Propiedades"

        Public Property IdPadre() As String
            Get
                Return _idPadre
            End Get
            Set(ByVal value As String)
                _idPadre = value
            End Set
        End Property

        Public Property Telefono() As String
            Get
                Return _telefono
            End Get
            Set(ByVal value As String)
                _telefono = value
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

        Public Property Direccion() As String
            Get
                Return _direccion
            End Get
            Set(ByVal value As String)
                _direccion = value
            End Set
        End Property

        Public Property NombreContacto() As String
            Get
                Return _nombreContacto
            End Get
            Set(ByVal value As String)
                _nombreContacto = value
            End Set
        End Property

        Public Property Almacen() As Integer
            Get
                Return _almacen
            End Get
            Set(ByVal value As Integer)
                _almacen = value
            End Set
        End Property

        Public Property Centro() As Integer
            Get
                Return _centro
            End Get
            Set(ByVal value As Integer)
                _centro = value
            End Set
        End Property

        Public Property Codigo() As String
            Get
                Return _codigo
            End Get
            Set(ByVal value As String)
                _codigo = value
            End Set
        End Property

        Public Property IdCiudad() As Integer
            Get
                Return _idCiudad
            End Get
            Set(ByVal value As Integer)
                _idCiudad = value
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

        Public Property IdSucursal() As Integer
            Get
                Return _idSucursal
            End Get
            Set(ByVal value As Integer)
                _idSucursal = value
            End Set
        End Property

#End Region

#Region "Constructores"
        Public Sub New()

        End Sub

        Public Sub New(ByVal idSucursal As Long)
            Me.New()
            Me.Seleccionar(idSucursal)
        End Sub

        Public Sub New(ByVal codigoSucursal As String)
            Me.New()
            Me.Seleccionar(codigoSucursal)
        End Sub

        Public Sub New(ByVal centro As String, ByVal almacen As String)
            Me.New()
            Me.Seleccionar(centro, almacen)
        End Sub

#End Region

#Region "Metodos"

        Public Sub Actualizar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            Me.EstablecerParametros(db)
            db.SqlParametros.Add("@idSucursal", SqlDbType.BigInt).Value = _idSucursal
            Try
                db.iniciarTransaccion()
                db.ejecutarNonQuery("ActualizarSucursal", CommandType.StoredProcedure)
                db.confirmarTransaccion()
            Catch ex As Exception
                db.abortarTransaccion()
                Throw New Exception("No se ha podido Modificar la sucursal")
            End Try
        End Sub

        Public Function ObtenerSucursalesDeDestinarario(ByVal idDestinatario As Integer, Optional ByVal idCiudad As Integer = 0) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dtSucursal As New DataTable
            Try
                db.SqlParametros.Add("@idDestinatario", SqlDbType.Int).Value = idDestinatario
                If idCiudad <> 0 Then db.agregarParametroSQL("@idCiudad", idCiudad, SqlDbType.Int)
                dtSucursal = db.ejecutarDataTable("SeleccionarSucursales", CommandType.StoredProcedure)
            Finally
                db.Dispose()
            End Try
            Return dtSucursal
        End Function

        Public Shared Function Seleccionar() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Return db.ejecutarDataTable("SeleccionarSucursales", CommandType.StoredProcedure)
        End Function

        Public Sub Seleccionar(ByVal idSucursal As Long)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@idSucursal", idSucursal, SqlDbType.BigInt)
            Me.CargarDatos(db)
        End Sub

        Public Sub Seleccionar(ByVal codigoSucursal As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@codigoSucursal", codigoSucursal, SqlDbType.VarChar, 15)
            Me.CargarDatos(db)
        End Sub

        Public Sub Seleccionar(ByVal centro As String, ByVal almacen As String)
            Dim db As New LMDataAccessLayer.LMDataAccess
            db.agregarParametroSQL("@centro", centro)
            db.agregarParametroSQL("@almacen", almacen)
            Me.CargarDatos(db)
        End Sub

        'Public Shared Function SeleccionarSegunUsuarioPadre(ByVal idUsuarioDestinatario As Long, Optional ByVal idCiudad As Integer = 0) As DataTable
        '    Dim db As New LMDataAccessLayer.LMDataAccess
        '    Dim perfilesAdmin As String = HerramientasDeNegocio.ObtenerValordeConfiguracion("PERFILES_ADMIN")
        '    Dim arrPerfilesAdmin As String() = perfilesAdmin.Split(",")
        '    Dim arrPerfilesActuales As ArrayList = ControlAcceso.ColeccionPerfiles.ObtenerPerfilesUsuario(idUsuarioDestinatario)
        '    Dim flagAplicarFiltro As Boolean = True
        '    For Each perfil As String In arrPerfilesActuales 'determina si tiene perfiles de administrador para mostrar todas las sucursales
        '        If arrPerfilesAdmin.Contains(perfil) Then
        '            flagAplicarFiltro = False
        '            Exit For
        '        End If
        '    Next
        '    If flagAplicarFiltro Then
        '        Dim idPadre As Long = ControlAcceso.Usuario.ObtenerIdDestinatario(idUsuarioDestinatario)
        '        db.agregarParametroSQL("@idDestinatario", idPadre, SqlDbType.BigInt)
        '    End If
        '    If idCiudad <> 0 Then db.agregarParametroSQL("@idCiudad", idCiudad, SqlDbType.Int)
        '    Return db.ejecutarDataTable("SeleccionarSucursales", CommandType.StoredProcedure)
        'End Function

        Private Sub CargarDatos(ByVal db As LMDataAccessLayer.LMDataAccess)
            Dim myReader As SqlClient.SqlDataReader = db.ejecutarReader("SeleccionarSucursales", CommandType.StoredProcedure)
            Try
                If myReader.Read Then
                    _idSucursal = myReader("idSucursal")
                    Integer.TryParse(myReader("idPadre").ToString(), _idPadre)
                    _telefono = myReader("telefono").ToString
                    _nombreContacto = myReader("nombreContacto").ToString
                    _nombre = myReader("nombre").ToString
                    _email = myReader("email").ToString
                    _direccion = myReader("direccion").ToString
                    _idCiudad = myReader("idciudad")
                    _ciudad = myReader("ciudad").ToString
                    Integer.TryParse(myReader("centro").ToString, _centro)
                    Integer.TryParse(myReader("almacen").ToString, _almacen)
                    _codigo = myReader("codigo").ToString

                End If
            Finally
                myReader.Close()
                db.cerrarConexion()
            End Try
        End Sub

        Private Sub EstablecerParametros(ByVal db As LMDataAccessLayer.LMDataAccess)
            With db.SqlParametros
                If _telefono <> "" Then .Add("@telefono", SqlDbType.VarChar).Value = _telefono
                If _nombreContacto <> "" Then .Add("@nombreContacto", SqlDbType.VarChar).Value = _nombreContacto
                If _direccion <> "" Then .Add("@direccion", SqlDbType.VarChar).Value = _direccion
                If _email <> "" Then .Add("@email", SqlDbType.VarChar).Value = _email
                .Add("@nombre", SqlDbType.VarChar).Value = _nombre
                .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                .Add("@idPadre", SqlDbType.Int).Value = _idPadre
            End With
        End Sub

        Protected Friend Sub Registrar(ByVal db As LMDataAccessLayer.LMDataAccess)
            db.SqlParametros.Clear()
            Me.EstablecerParametros(db)
            db.SqlParametros.Add("@idSucursal", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
            Try
                db.iniciarTransaccion()
                db.ejecutarNonQuery("RegistrarSucursal", CommandType.StoredProcedure)
                _idSucursal = db.SqlParametros("@idSucursal").Value
                db.confirmarTransaccion()
            Catch ex As Exception
                db.abortarTransaccion()
                Throw New Exception("No se ha podido registrar la sucursal")
            End Try
        End Sub

        Public Sub Registrar()
            Dim db As New LMDataAccessLayer.LMDataAccess
            Me.Registrar(db)
        End Sub

        Public Function SubirDatos(ByVal dt As DataTable, ByVal idUsuario As Long) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dtErrores As New DataTable
            Try
                dt.Columns.Add(New DataColumn("idUsuario", GetType(Long), idUsuario))
                db.agregarParametroSQL("@idUsuario", idUsuario, SqlDbType.BigInt)
                db.ejecutarNonQuery("LimpiarInfoCargaSucursal", CommandType.StoredProcedure)
                db.inicilizarBulkCopy()
                db.BulkCopy.DestinationTableName = "InfoCargaSucursal"
                db.BulkCopy.WriteToServer(dt)
                dtErrores = db.ejecutarDataTable("ValidarCargaSucursales", CommandType.StoredProcedure)
                If dtErrores.Rows.Count = 0 Then
                    db.iniciarTransaccion()
                    db.ejecutarNonQuery("CrearSucursalesEnBatch", CommandType.StoredProcedure)
                    db.confirmarTransaccion()
                End If
                Return dtErrores
            Catch ex As Exception
                If db.estadoTransaccional Then db.abortarTransaccion()
                Throw New Exception("Error al tratar de cargar los datos de Sucursales")
            End Try
        End Function

        Public Shared Function SeleccionarNodosPadre() As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dt As DataTable = db.ejecutarDataTable("ObtenerSucursalesPadre", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Shared Function ConsultarSucursales(ByVal filtros As FiltrosSucursal) As DataTable
            Dim db As New LMDataAccessLayer.LMDataAccess
            With filtros
                If .Nombre <> "" Then db.agregarParametroSQL("@nombre", .Nombre)
                If .IdCiudad > 0 Then db.agregarParametroSQL("@idCiudad", .IdCiudad, SqlDbType.Int)
                If .Centro > 0 Then db.agregarParametroSQL("@centro", .Centro, SqlDbType.Int)
                If .Almacen > 0 Then db.agregarParametroSQL("@almacen", .Almacen, SqlDbType.Int)
                If .Codigo <> "" Then db.agregarParametroSQL("@codigo", .Codigo, SqlDbType.Int)
                If .IdPadre > 0 Then db.agregarParametroSQL("@idPadre", .IdPadre, SqlDbType.Int)
            End With
            Dim dt As DataTable = db.ejecutarDataTable("ConsultarSucursales", CommandType.StoredProcedure)
            Return dt
        End Function

        Public Structure FiltrosSucursal
            Dim Nombre As String
            Dim IdCiudad As Integer
            Dim Centro As Integer
            Dim Almacen As Integer
            Dim Codigo As String
            Dim IdPadre As Integer
        End Structure
#End Region

    End Class
End Namespace

