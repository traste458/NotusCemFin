Imports System.Text
Imports LMDataAccessLayer

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 01/08/2014
''' Description: Clase diseñada para el manejo y administración de los datos almacenados en la tabla almacenBodega
''' </summary>
''' <remarks></remarks>
Public Class AlmacenBodega

#Region "Atributos"

    Private _idAlmacenBodega As Integer
    Private _idBodega As Integer
    Private _bodega As String
    Private _centro As String
    Private _almacen As String
    Private _descripcion As String
    Private _activo As Boolean
    Private _idClienteCem As Integer
    Private _cadenaWeb As String
    Private _idUsuario As Integer
    Private _idUnidadNegocio As Integer
    Private _unidadNegocio As String
    Private _idCiudad As Integer
    Private _ciudad As String
    Private _idTipoBodega As Integer
    Private _idClienteExterno As Integer
    Private _nombre As String
    Private _codigo As String
    Private _direccion As String
    Private _telefono As String
    Private _ciudades As String
    Private _estado As Boolean
    Private _aceptaProductoEnReconocimiento As Boolean
    Private _idBodega2 As String
    Private _ciuPrincipal As String
    Private _registrado As Boolean
    Private _clientExterno As Integer
    Private _Tipo As Integer
    Private _nomCiudad As String
    Private _estados As String
    Private _nomCli As String
    Private _nomTipoAs As String
    Private _unidNego As String
    Private _nomTipo As String
    Private _tokenSimpliRoute As String
    Private _HorarioAtencion As String
    Public Property IdSucursal As String
    Public Property codigoSucursalInterRapidisimo As Integer

#End Region

#Region "Propiedades"
    Public Property HorarioAtencion() As String
        Get
            Return _HorarioAtencion
        End Get
        Set(ByVal value As String)
            _HorarioAtencion = value
        End Set
    End Property

    Public Property nomTipo() As String
        Get
            Return _nomTipo
        End Get
        Set(ByVal value As String)
            _nomTipo = value
        End Set
    End Property

    Public Property unidNego() As String
        Get
            Return _unidNego
        End Get
        Set(ByVal value As String)
            _unidNego = value
        End Set
    End Property

    Public Property nomTipoAs() As String
        Get
            Return _nomTipoAs
        End Get
        Set(ByVal value As String)
            _nomTipoAs = value
        End Set
    End Property

    Public Property nomCli() As String
        Get
            Return _nomCli
        End Get
        Set(ByVal value As String)
            _nomCli = value
        End Set
    End Property

    Public Property estados() As String
        Get
            Return _estados
        End Get
        Set(ByVal value As String)
            _estados = value
        End Set
    End Property

    Public Property nomCiudad() As String
        Get
            Return _nomCiudad
        End Get
        Set(ByVal value As String)
            _nomCiudad = value
        End Set
    End Property

    Public Property Tipo() As Integer
        Get
            Return _Tipo
        End Get
        Set(ByVal value As Integer)
            _Tipo = value
        End Set
    End Property

    Public Property clientExterno As Integer
        Get
            Return _clientExterno
        End Get
        Set(value As Integer)
            _clientExterno = value
        End Set
    End Property

    Public Property ciuPrincipal As String
        Get
            Return _ciuPrincipal
        End Get
        Set(value As String)
            _ciuPrincipal = value
        End Set
    End Property

    Public Property idBodega2 As String
        Get
            Return _idBodega2
        End Get
        Set(value As String)
            _idBodega2 = value
        End Set
    End Property

    Public Property aceptaProductoEnReconocimiento As Boolean
        Get
            Return _aceptaProductoEnReconocimiento
        End Get
        Set(value As Boolean)
            _aceptaProductoEnReconocimiento = value
        End Set
    End Property

    Public Property estado As Boolean
        Get
            Return _estado
        End Get
        Set(value As Boolean)
            _estado = value
        End Set
    End Property

    Public Property ciudades As String
        Get
            Return _ciudades
        End Get
        Set(value As String)
            _ciudades = value
        End Set
    End Property

    Public Property telefono As String
        Get
            Return _telefono
        End Get
        Set(value As String)
            _telefono = value
        End Set
    End Property

    Public Property direccion As String
        Get
            Return _direccion
        End Get
        Set(value As String)
            _direccion = value
        End Set
    End Property

    Public Property codigo As String
        Get
            Return _codigo
        End Get
        Set(value As String)
            _codigo = value
        End Set
    End Property

    Public Property nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador del almacenBodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdAlmacenBodega As Integer
        Get
            Return _idAlmacenBodega
        End Get
        Set(value As Integer)
            _idAlmacenBodega = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador de bodega asociado al almacén
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdBodega As Integer
        Get
            Return _idBodega
        End Get
        Set(value As Integer)
            _idBodega = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece  el nombre de la bodega asociada al almacén
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Bodega As String
        Get
            Return _bodega
        End Get
        Set(value As String)
            _bodega = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el código del centro
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Centro As String
        Get
            Return _centro
        End Get
        Set(value As String)
            _centro = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el código del almacén
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Almacen As String
        Get
            Return _almacen
        End Get
        Set(value As String)
            _almacen = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el nombre ó descripción del almacén
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Descripcion As String
        Get
            Return _descripcion
        End Get
        Set(value As String)
            _descripcion = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el estado del almacén (Activo - Inactivo)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador del cliente CEM al que pertenece el almacén
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdClienteCem As Integer
        Get
            Return _idClienteCem
        End Get
        Set(value As Integer)
            _idClienteCem = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el nombre del  cliente cem al que pertenece el almacén
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ClienteCEM As String
        Get
            Return _cadenaWeb
        End Get
        Set(value As String)
            _cadenaWeb = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el usuario de registro o el usuario modificador
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador de la unidad de negocio a la que corresponde la bodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdUnidadNegocio As Integer
        Get
            Return _idUnidadNegocio
        End Get
        Set(value As Integer)
            _idUnidadNegocio = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el nombre de la unidad de negocio a la que corresponde la bodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UnidadNegocio As String
        Get
            Return _unidadNegocio
        End Get
        Set(value As String)
            _unidadNegocio = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador de la ciudad de la bodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdCiudad As Integer
        Get
            Return _idCiudad
        End Get
        Set(value As Integer)
            _idCiudad = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el nombre de la ciudad de la bodega
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Ciudad As String
        Get
            Return _ciudad
        End Get
        Set(value As String)
            _ciudad = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador del tipo de bodega (aplica para la bodega del almacén asociado a la tabla 'TipoBodega')
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdTipoBodega As Integer
        Get
            Return _idTipoBodega
        End Get
        Set(value As Integer)
            _idTipoBodega = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el identificador del cliente externo asociado a la bodega (en relación con la tabla 'ClienteExterno')
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdClienteExterno As Integer
        Get
            Return _idClienteExterno
        End Get
        Set(value As Integer)
            _idClienteExterno = value
        End Set
    End Property

    ''' <summary>
    ''' Define o establece si se encuentra registrada la unidad de negocio
    ''' </summary>
    ''' <remarks></remarks>
    ''' 
    Public Property Registrado As Boolean
        Get
            Return _registrado
        End Get
        Set(value As Boolean)
            _registrado = value
        End Set
    End Property

    Public Property TokenSimpliRoute As String
        Get
            Return _tokenSimpliRoute
        End Get
        Set(value As String)
            _tokenSimpliRoute = value
        End Set
    End Property

#End Region

#Region "Construtores"

    Public Sub New()
        MyBase.New()
    End Sub

    ''' <summary>
    ''' Constructor que sobrecarga la clase con los datos del idServicio proporcionado
    ''' </summary>
    ''' <param name="idAlmacenBodega"> de tipo <see langword="Integer"/> que contiene la información correspondiente al identificador del servicio. </param>
    ''' <remarks>
    ''' Su forma de instanciamiento se debe realizar de la siguiente manera:
    ''' Dim miClase As New  WMS.AlmacenBodega(idAlmacenBodega:= idAlmacenBodega)
    ''' </remarks>
    Public Sub New(ByVal idAlmacenBodega As Integer)
        MyBase.New()
        _idAlmacenBodega = idAlmacenBodega
        CargarDatos()
    End Sub

    Public Sub New(ByVal idbodega As Long)
        MyBase.New()
        idbodega = idbodega
        CargarInformacion(idbodega)
    End Sub

    Public Sub New(ByVal Bodegas As String)
        MyBase.New()
        CargarInformacionBodega(Bodegas)
    End Sub

#End Region

#Region "Métodos Privados"

    ''' <summary>
    ''' Función que realiza la inicialización de la carga de los atributos de la clase, según los parametros establecidos
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@listaIdAlmacenBodega", SqlDbType.VarChar, 30).Value = CStr(_idAlmacenBodega)
                .ejecutarReader("ObtenerInfoAlmacenBodega", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        CargarResultadoConsulta(.Reader)
                        _registrado = True
                    End If
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Sub CrearUsuarioBodega(usuario As Integer, idBodega As Integer)
        Dim db As New LMDataAccess

        Try

            With db
                .AgregarParametroSQL("@idUsuario", usuario, SqlDbType.Int)
                .AgregarParametroSQL("@idBodega", idBodega, SqlDbType.Int)
                .EjecutarNonQuery("CrearUsuarioBodega", CommandType.StoredProcedure)
            End With


        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        Finally
            If db IsNot Nothing Then db.Dispose()

        End Try



    End Sub

    Public Overloads Function ObtenerReporteCiudadCercana(idBodega As Integer, idTipo As Integer)

        Dim db As New LMDataAccess
        Dim dtCiudad As New DataTable
        With db
            If idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = idBodega
            If idTipo > 0 Then .SqlParametros.Add("@tipoBodega", SqlDbType.Int).Value = idTipo
            dtCiudad = .EjecutarDataTable("ReporteBodegaCiudadCercana", CommandType.StoredProcedure)
        End With
        Return dtCiudad
    End Function

    Public Overloads Function ObtenerCiudades()

        Dim db As New LMDataAccess
        Dim dtCiudad As New DataTable
        With db
            dtCiudad = .EjecutarDataTable("obtenerCiudades", CommandType.StoredProcedure)
        End With
        Return dtCiudad
    End Function

    Public Overloads Sub CargarInformacionBodega(Bodegas As Integer)
        If Bodegas > 0 Then
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = Bodegas
                    .ejecutarReader("ObtenerDatisBodega", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _idBodega2 = .Reader("idbodega2").ToString
                            _nombre = .Reader("bodega").ToString
                            _direccion = .Reader("direccion").ToString
                            _telefono = .Reader("telefonos").ToString
                            Integer.TryParse(.Reader("idCiudad").ToString, _idCiudad)
                            Integer.TryParse(.Reader("idClienteExterno").ToString, _clientExterno)
                            Integer.TryParse(.Reader("idUnidadNegocio").ToString, _unidadNegocio)
                            Integer.TryParse(.Reader("idTipo").ToString, _Tipo)
                            _codigo = .Reader("codigo").ToString
                            _nomCiudad = .Reader("ciudad").ToString
                            _estados = .Reader("estado").ToString
                            _aceptaProductoEnReconocimiento = CBool(.Reader("aceptaProdSinReconocimiento"))
                            _nomCli = .Reader("clienteExterno").ToString
                            _nomTipo = .Reader("tipoBod").ToString
                            _unidNego = .Reader("Negocio").ToString
                            _tokenSimpliRoute = .Reader("tokenSimpliRoute").ToString
                            _IdSucursal = .Reader("idSucursal").ToString
                            codigoSucursalInterRapidisimo = .Reader("codigoSucursalInterRapidisimo")
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If

    End Sub

    Public Sub Crear()
        Dim db As New LMDataAccess
        Try

            With db
                .AgregarParametroSQL("@idBodega2", _idBodega2, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@bodega", _nombre, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@direccion", _direccion, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@telefonos", _telefono, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@idCiudadPrinc", _ciuPrincipal, SqlDbType.Int)
                .AgregarParametroSQL("@aceptaProdSinReconocimiento", _aceptaProductoEnReconocimiento, SqlDbType.Bit)
                .AgregarParametroSQL("@idClienteExterno", _clientExterno, SqlDbType.Decimal)
                .AgregarParametroSQL("@idUnidadNegocio", _unidadNegocio, SqlDbType.SmallInt)
                .AgregarParametroSQL("@idTipo", _Tipo, SqlDbType.SmallInt)
                .AgregarParametroSQL("@codigo", _codigo, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@tokenSimpliRoute", _tokenSimpliRoute, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@codigoSucursalInterRapidisimo", codigoSucursalInterRapidisimo, SqlDbType.Int)
                .AgregarParametroSQL("@centro", _centro, SqlDbType.VarChar, (10))
                .AgregarParametroSQL("@almacen", _almacen, SqlDbType.VarChar, (10))
                .AgregarParametroSQL("@horarioAtencion", _HorarioAtencion, SqlDbType.VarChar, -1)
                .AgregarParametroSQL("@idSucursal", _IdSucursal, SqlDbType.VarChar, (5))

                .EjecutarNonQuery("CrearBodegaCiudad", CommandType.StoredProcedure)
            End With

        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        Finally
            If db IsNot Nothing Then db.Dispose()

        End Try


    End Sub

    Public Overloads Function borrarCiudadCercana(idBodegaCerca As Integer)
        Dim db As New LMDataAccess
        Dim dtCiudades As New DataTable
        With db
            .SqlParametros.Add("@idBodega", Data.SqlDbType.Int).Value = idBodegaCerca
            dtCiudades = .EjecutarDataTable("eliminarCiudadCercana", CommandType.StoredProcedure)
        End With
        Return dtCiudades
    End Function

    Private Sub CargarInformacion(idbodega As Integer)
        If idbodega > 0 Then
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = idbodega
                    .ejecutarReader("ObtenerDatisBodega", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            _idBodega2 = .Reader("idbodega2").ToString
                            _nombre = .Reader("bodega").ToString
                            _direccion = .Reader("direccion").ToString
                            _telefono = .Reader("telefonos").ToString
                            Integer.TryParse(.Reader("idCiudad").ToString, _idCiudad)
                            Integer.TryParse(.Reader("idClienteExterno").ToString, _clientExterno)
                            Integer.TryParse(.Reader("idUnidadNegocio").ToString, _unidadNegocio)
                            Integer.TryParse(.Reader("idTipo").ToString, _Tipo)
                            _codigo = .Reader("codigo").ToString
                            _nomCiudad = .Reader("ciudad").ToString
                            _estados = .Reader("estado").ToString
                            _aceptaProductoEnReconocimiento = CBool(.Reader("aceptaProdSinReconocimiento"))
                            _nomCli = .Reader("clienteExterno").ToString
                            _nomTipo = .Reader("tipoBod").ToString
                            _unidNego = .Reader("Negocio").ToString
                            _tokenSimpliRoute = .Reader("tokenSimpliRoute").ToString
                            codigoSucursalInterRapidisimo = .Reader("codigoSucursalInterRapidisimo")
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If
    End Sub

    Public Sub editar()
        Dim db As New LMDataAccess
        Try

            With db
                .AgregarParametroSQL("@bodega2", _idBodega2, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@idBodega", _idBodega, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@bodega", _nombre, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@direccion", _direccion, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@telefonos", _telefono, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@idciudad", _ciuPrincipal, SqlDbType.Int)
                .AgregarParametroSQL("@aceptaProdSinReconocimiento", _aceptaProductoEnReconocimiento, SqlDbType.Bit)
                .AgregarParametroSQL("@idClienteExterno", _clientExterno, SqlDbType.Decimal)
                .AgregarParametroSQL("@idUnidadNegocio", _unidadNegocio, SqlDbType.SmallInt)
                .AgregarParametroSQL("@idTipo", _Tipo, SqlDbType.SmallInt)
                .AgregarParametroSQL("@codigo", _codigo, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@tokenSimpliRoute", _tokenSimpliRoute, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@idSucursal", IdSucursal, SqlDbType.VarChar, (100))
                .AgregarParametroSQL("@codigoSucursalInterRapidisimo", codigoSucursalInterRapidisimo, SqlDbType.Int)
                .EjecutarNonQuery("ActualizarBodega", CommandType.StoredProcedure)
            End With

        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        Finally
            If db IsNot Nothing Then db.Dispose()

        End Try


    End Sub

    Public Sub CrearCiudadCercana(idBodega As Integer, idCiudad As Integer, idUsuario As Integer)
        Dim db As New LMDataAccess

        Try

            With db
                .AgregarParametroSQL("@idBodega", idBodega, SqlDbType.Int)
                .AgregarParametroSQL("@idCiudad", idCiudad, SqlDbType.Int)
                .AgregarParametroSQL("@idUsuario", idUsuario, SqlDbType.Int)
                .EjecutarNonQuery("crearBodegaCercana", CommandType.StoredProcedure)
            End With


        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        Finally
            If db IsNot Nothing Then db.Dispose()

        End Try

    End Sub

    Public Overloads Function ObtenerUnidadNegocio()

        Dim db As New LMDataAccess
        Dim dtNegocio As New DataTable
        With db
            dtNegocio = .EjecutarDataTable("verUnidadNegocio", CommandType.StoredProcedure)
        End With
        Return dtNegocio
    End Function

    Public Overloads Function ObtenerTipoBodega()

        Dim db As New LMDataAccess
        Dim dtBodega As New DataTable
        With db
            dtBodega = .EjecutarDataTable("verTipoBodega", CommandType.StoredProcedure)
        End With
        Return dtBodega
    End Function

    Public Overloads Function ObtenerClienteExterno()

        Dim db As New LMDataAccess
        Dim dtClientes As New DataTable
        With db
            dtClientes = .EjecutarDataTable("verClientesExternos", CommandType.StoredProcedure)
        End With
        Return dtClientes
    End Function

    Public Overloads Function ListarBodegas()
        Dim db As New LMDataAccess
        Dim dtBodegas As New DataTable
        With db
            If IdBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = IdBodega
            If IdTipoBodega > 0 Then .SqlParametros.Add("@idTipoBodega", SqlDbType.Int).Value = IdTipoBodega
            dtBodegas = .EjecutarDataTable("listarBodegas", CommandType.StoredProcedure)
        End With
        Return dtBodegas
    End Function

    Public Overloads Function ConsultarBodega()
        Dim db As New LMDataAccess
        Dim dtBodegas As New DataTable
        With db
            dtBodegas = .EjecutarDataTable("obtenerBodegas", CommandType.StoredProcedure)
        End With
        Return dtBodegas
    End Function

    Public Overloads Function ConsultarTipoBodega()
        Dim db As New LMDataAccess
        Dim dtBodegas As New DataTable
        With db
            dtBodegas = .EjecutarDataTable("obtenerTipoBodega", CommandType.StoredProcedure)
        End With
        Return dtBodegas
    End Function

    Public Overloads Function ObtenerCiudadCercanaSinBodega(idBodega As Integer)
        Dim db As New LMDataAccess
        Dim dtCiudades As New DataTable
        With db
            .SqlParametros.Add("@idBodega", Data.SqlDbType.Int).Value = idBodega
            dtCiudades = .EjecutarDataTable("verCiudadesCercanasSinBodega", CommandType.StoredProcedure)
        End With
        Return dtCiudades
    End Function

    Public Overloads Function ObtenerCiudadSinBodega()
        Dim db As New LMDataAccess
        Dim dtCiudades As New DataTable
        With db
            dtCiudades = .EjecutarDataTable("verCiudadesSinBodega", CommandType.StoredProcedure)
        End With
        Return dtCiudades
    End Function

    Public Overloads Function ObtenerCiudadAsignada(idBodega As Integer)
        Dim db As New LMDataAccess
        Dim dtBodegas As New DataTable
        With db
            .SqlParametros.Add("@idBodega", Data.SqlDbType.Int).Value = idBodega
            dtBodegas = .EjecutarDataTable("verCiudadCercaPorBodega", CommandType.StoredProcedure)
        End With
        Return dtBodegas
    End Function

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@centro", SqlDbType.VarChar, 50).Value = _centro
                    .Add("@almacen", SqlDbType.VarChar, 50).Value = _almacen
                    .Add("@descripcion", SqlDbType.VarChar, 450).Value = _descripcion
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                    If _idClienteCem > 0 Then .Add("@idClienteCem", SqlDbType.Int).Value = _idClienteCem
                    If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    If _idClienteExterno > 0 Then .Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                    If _idUnidadNegocio > 0 Then .Add("@idUnidadNegocio", SqlDbType.Int).Value = _idUnidadNegocio
                    If _idTipoBodega > 0 Then .Add("@idTipoBodega", SqlDbType.Int).Value = _idTipoBodega
                    .Add("@estado", SqlDbType.Int).Value = _activo
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarAlmacenBodega", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With

        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se generó un error al realizar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idAlmacenBodega", SqlDbType.Int).Value = _idAlmacenBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
                    If _idUnidadNegocio > 0 Then .Add("@idUnidadNegocio", SqlDbType.Int).Value = _idUnidadNegocio
                    If Not String.IsNullOrEmpty(_centro) Then .Add("@centro", SqlDbType.VarChar, 50).Value = _centro
                    If Not String.IsNullOrEmpty(_almacen) Then .Add("@almacen", SqlDbType.VarChar, 50).Value = _almacen
                    If Not String.IsNullOrEmpty(_descripcion) Then .Add("@descripcion", SqlDbType.VarChar, 450).Value = _descripcion
                    If _idClienteCem > 0 Then .Add("@idClienteCem", SqlDbType.Int).Value = _idClienteCem
                    If _idCiudad > 0 Then .Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                    .Add("@activo", SqlDbType.Bit).Value = _activo
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .EjecutarNonQuery("ActualizarAlmacenBodega", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With

        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se generó un error al realizar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

#Region "Métodos Protegidos"

    ''' <summary>
    ''' Método encargado de sobrecargar los atributos de la clase 
    ''' </summary>
    ''' <param name="reader"> de tipo <see langword="Data.Common.DbDataReader"/> que contiene un objeto de tipo reader, 
    ''' para realizar la lectura y asignación de valores a los atributos de la clase</param>
    ''' <remarks></remarks>
    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idAlmacenBodega"), _idAlmacenBodega)
                Integer.TryParse(reader("idBodega"), _idBodega)
                If Not IsDBNull(reader("bodega")) Then _bodega = CStr(reader("bodega"))
                If Not IsDBNull(reader("centro")) Then _centro = CStr(reader("centro"))
                If Not IsDBNull(reader("almacen")) Then _almacen = CStr(reader("almacen"))
                If Not IsDBNull(reader("descripcion")) Then _descripcion = CStr(reader("descripcion"))
                If Not IsDBNull(reader("activo")) Then Boolean.TryParse(reader("activo"), _activo)
                If Not IsDBNull(reader("idClienteCem")) Then Integer.TryParse(reader("idClienteCem"), _idClienteCem)
                If Not IsDBNull(reader("nombre")) Then _cadenaWeb = CStr(reader("nombre"))
                If Not IsDBNull(reader("idUnidadNegocio")) Then Integer.TryParse(reader("idUnidadNegocio"), _idUnidadNegocio)
                If Not IsDBNull(reader("unidadNegocio")) Then _unidadNegocio = CStr(reader("unidadNegocio"))
                If Not IsDBNull(reader("idCiudad")) Then Integer.TryParse(reader("idCiudad"), _idCiudad)
                If Not IsDBNull(reader("ciudad")) Then _ciudad = CStr(reader("ciudad"))
            End If
        End If
    End Sub

#End Region

End Class
