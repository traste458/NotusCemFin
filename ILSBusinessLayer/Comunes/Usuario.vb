Imports LMDataAccessLayer
Imports EncryptionClassLibrary.LMEncryption

Public Class Usuario

#Region "Atributos"

    Private _idUsuario As Integer
    Private _idPerfil As Integer
	Private _perfil As String
    Private _nombre As String
    Private _usuario As String
    Private _clave As String
    Private _linea As Integer
    Private _idCargo As Integer
	Private _cargo As String
    Private _email As String
    Private _identificacion As String
    Private _idCiudad As Integer
    Private _ciudad As String
    Private _registrado As Boolean
#End Region

#Region "Propiedades"

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property IdPerfil() As Integer
        Get
            Return _idPerfil
        End Get
        Set(ByVal value As Integer)
            _idPerfil = value
        End Set
    End Property

    Public Property NombrePerfil As String
        Get
            Return _perfil
        End Get
        Set(value As String)
            _perfil = value
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

    Public Property Usuario As String
        Get
            Return _usuario
        End Get
        Set(value As String)
            _usuario = value
        End Set
    End Property

    Public Property Clave As String
        Get
            Return _clave
        End Get
        Set(value As String)
            _clave = value
        End Set
    End Property

    Public Property Linea As Integer
        Get
            Return _linea
        End Get
        Set(value As Integer)
            _linea = value
        End Set
    End Property

    Public Property IdCargo As Integer
        Get
            Return _idCargo
        End Get
        Set(value As Integer)
            _idCargo = value
        End Set
    End Property

    Public Property Cargo As String
        Get
            Return _cargo
        End Get
        Set(value As String)
            _cargo = value
        End Set
    End Property
    Public Property Email As String
        Get
            Return _email
        End Get
        Set(value As String)
            _email = value
        End Set
    End Property

    Public Property Identificacion As String
        Get
            Return _identificacion
        End Get
        Set(value As String)
            _identificacion = value
        End Set
    End Property

    Public Property IdCiudad As Integer
        Get
            Return _idCiudad
        End Get
        Set(value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property Ciudad As String
        Get
            Return _ciudad
        End Get
        Set(value As String)
            _ciudad = value
        End Set
    End Property

    Public Property Registrado As Boolean
        Get
            Return _registrado
        End Get
        Set(value As Boolean)
            _registrado = value
        End Set
    End Property

    Public Property Mip As String
    Public Property IdCliente As Integer
    Public Property Cliente As String
    Public Property IdBodega As Integer
    Public Property PoolAplicacion As String
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idUsuario As Integer)
        MyBase.New()
        _idUsuario = idUsuario
        CargarDatos()
    End Sub

    Public Sub New(ByVal identificacion As String)
        MyBase.New()
        _identificacion = identificacion
        CargarDatos()
    End Sub

    Public Sub New(ByVal usuario As String, clave As String)
        MyBase.New()
        _usuario = usuario
        _clave = EncryptionData.getMD5Hash(clave)
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess()
        Try
            With dbManager
                If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                If _usuario <> String.Empty Then .SqlParametros.Add("@usuario", SqlDbType.VarChar).Value = _usuario
                If _clave <> String.Empty Then .SqlParametros.Add("@clave", SqlDbType.VarChar).Value = _clave
                If Not String.IsNullOrEmpty(_identificacion) Then .SqlParametros.Add("@identificacion", SqlDbType.VarChar).Value = _identificacion

                .ejecutarReader("ObtenerInfoUsuarioGeneral", CommandType.StoredProcedure)
                If .Reader.Read Then
                    Integer.TryParse(.Reader("idUsuario"), _idUsuario)
                    Integer.TryParse(.Reader("idPerfil").ToString(), _idPerfil)
					_perfil = .Reader("perfil").ToString
                    _nombre = .Reader("nombre").ToString()
                    Integer.TryParse(.Reader("linea"), _linea)
                    Integer.TryParse(.Reader("idCargo"), _idCargo)
					_cargo = .Reader("cargo").ToString
                    If Not IsDBNull(.Reader("email")) Then _email = .Reader("email")
                    Integer.TryParse(.Reader("idCiudad").ToString(), _idCiudad)
					_ciudad = .Reader("ciudad").ToString
                    _registrado = True
                End If
                If Not .Reader.IsClosed Then .Reader.Close()
            End With
        Catch ex As Exception
            Throw New Exception("Error al obtener la información del usuario." & ex.Message)
        Finally
            If Not dbManager Is Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Compartidos"

    Public Shared Function SeleccionarUsuarios(ByVal idPerfil As Integer) As DataTable
        Dim resultado As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        Try
            adminBD.agregarParametroSQL("@idPerfil", idPerfil)
            resultado = adminBD.ejecutarDataTable("ConsultarUsuariosPerfil", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Ocurrió un error cargando listado de usuarios: " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try
        Return resultado
    End Function

    Public Shared Function SeleccionarUsuariosDespacho() As DataTable
        Dim resultado As New DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        Try
            resultado = adminBD.ejecutarDataTable("ConsultarUsuariosDespachos", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception("Ocurrió un error cargando listado de usuarios: " & ex.Message)
        Finally
            adminBD.Dispose()
        End Try
        Return resultado
    End Function

    Public Shared Function ObtenerListadoPorPerfiles(ByVal idPerfiles As String) As DataTable
        Dim db As New LMDataAccess
        Dim dt As New DataTable

        If Not String.IsNullOrEmpty(idPerfiles) Then
            db.agregarParametroSQL("idPerfiles", idPerfiles, SqlDbType.VarChar, 200)
            dt = db.ejecutarDataTable("ObtenerListadoPorPerfiles", CommandType.StoredProcedure)
        End If
        Return dt
    End Function

    Public Shared Function ObtenerListadoPorPerfilesIdOrden(ByVal idPerfiles As String, ByVal idorden As Integer) As DataTable
        Dim db As New LMDataAccess
        Dim dt As New DataTable

        If Not String.IsNullOrEmpty(idPerfiles) Then
            db.agregarParametroSQL("idPerfiles", idPerfiles, SqlDbType.VarChar, 200)
            db.agregarParametroSQL("@idOrden", idorden, SqlDbType.Int)
            dt = db.ejecutarDataTable("ObtenerListadoPorPerfilesidOrden", CommandType.StoredProcedure)
        End If
        Return dt
    End Function

    Public Function ObtenerPerfilUsuarios() As DataTable
        Dim dtDatos As DataTable
        Using dbManager As New LMDataAccess
            With dbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                dtDatos = .ejecutarDataTable("ObtenerInfoPerfilUsuarios", CommandType.StoredProcedure)
            End With
        End Using
        Return dtDatos
    End Function

#End Region

#Region "Enumeradores"

    Public Enum Perfil
        AcomodadorBodega = 35
        AdministradorBodega = 38
        Solicitante = 98
    End Enum

#End Region

End Class
