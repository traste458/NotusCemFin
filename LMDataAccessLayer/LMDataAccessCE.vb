Imports System.Configuration
Imports System.Data.SqlServerCe
Imports System.IO

''' <summary>
''' Provee funcionalidades de  acceso a datos 
''' </summary>

Public Class LMDataAccessCE
    Implements IDisposable

#Region "variables"
    Private disposing As Boolean

    Private conexion As SqlCeConnection
    ''' <summary>
    ''' command encargado de ejecutar las sentencias SQL toda la clase
    ''' </summary>
    ''' <remarks></remarks>
    Private comando As SqlCeCommand
    Private transaccion As SqlCeTransaction
    Private _infoConexion As InfoObjetoConexionCE
    Private _reader As SqlCeDataReader
    'Private pSql As New SqlceClientPermission(System.Security.Permissions.PermissionState.Unrestricted)
#End Region

#Region "Propiedades"

    ''' <summary>
    ''' indica el estado de la conexion
    ''' </summary>
    Public ReadOnly Property estadoConexion() As System.Data.ConnectionState
        Get
            Return conexion.State
        End Get
    End Property

    ''' <summary>
    ''' flag que indica si el comando de la clase etá en modo transaccional
    ''' </summary>
    ''' <value></value>
    ''' <returns>true si se inicío la transacción, delo contrario false </returns>
    Public ReadOnly Property estadoTransaccional() As Boolean
        Get
            If transaccion Is Nothing Then
                Return False
            ElseIf transaccion.Connection Is Nothing Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public ReadOnly Property transaccionSQL() As SqlCeTransaction
        Get
            Return transaccion
        End Get
    End Property

    Public ReadOnly Property ConexionSQL() As SqlCeConnection
        Get
            Return conexion
        End Get
    End Property

    Public ReadOnly Property SqlParametros() As SqlCeParameterCollection
        Get
            Return comando.Parameters

        End Get
    End Property

    ''' <summary>
    ''' obtiene o establece un parametro de la lista de parametos del comando
    ''' </summary>
    ''' <param name="index">indexación de base cero: indice a retornar de la lista de parametros del comando</param>
    ''' <value></value>
    ''' <returns>SqlClient.SqlParameter</returns>
    ''' <remarks>se puede utilizar para acceder directamente los paremetros establecidos al command de la clase</remarks>
    Property SqlParametros(ByVal index As Integer) As SqlCeParameter
        Get
            If comando.Parameters.Count > 0 Then
                Return comando.Parameters.Item(index)
            Else
                Throw New ArgumentOutOfRangeException
            End If
        End Get
        Set(ByVal Value As SqlCeParameter)
            If comando.Parameters.Count > 0 Then
                comando.Parameters.Item(index) = Value
            Else
                Throw New ArgumentOutOfRangeException
            End If
        End Set
    End Property

    ''' <summary>
    ''' obtiene o establece un parametro de la lista de parametos del comando
    ''' </summary>
    ''' <param name="Nombre">Nombre del parametro</param>
    ''' <value></value>
    ''' <returns>SqlClient.SqlParameter</returns>
    ''' <remarks>se puede utilizar para acceder directamente los paremetros establecidos al command de la clase</remarks>
    Property SqlParametros(ByVal Nombre As String) As SqlCeParameter
        Get
            If comando.Parameters.Count > 0 Then
                Return comando.Parameters.Item(Nombre)
            Else
                Throw New ArgumentOutOfRangeException
            End If
        End Get
        Set(ByVal Value As SqlCeParameter)
            If comando.Parameters.Count > 0 Then
                Try
                    comando.Parameters.Item(Nombre) = Value
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                End Try

            Else
                Throw New ArgumentOutOfRangeException
            End If
        End Set
    End Property

    ''' <summary>
    ''' Obtiene o etablece el tiempo de espera antes de finalizar el intento de ejecutar el comando y lanzar un error
    ''' </summary>
    Property TiempoEsperaComando() As Integer
        Get
            Return comando.CommandTimeout
        End Get
        Set(ByVal value As Integer)
            comando.CommandTimeout = value
            'If myBulkCopy IsNot Nothing Then myBulkCopy.BulkCopyTimeout = value
        End Set
    End Property


    'Public ReadOnly Property InformacionDeConexion() As InfoObjetoConexionCE
    '    Get
    '        Dim builder As New SqlConnectionStringBuilder(conexion.ConnectionString)
    '        _infoConexion = New InfoObjetoConexionCE
    '        With _infoConexion
    '            .CadenaConexion = conexion.ConnectionString
    '            .NombreBaseDatos = conexion.Database
    '            .NombreServidor = conexion.DataSource
    '            .TiempoEspera = conexion.ConnectionTimeout
    '            .NombreUsuario = builder.UserID
    '            .Password = builder.Password
    '        End With
    '        builder.Clear()
    '        Return _infoConexion
    '    End Get
    'End Property

    ''' <summary>
    ''' Permite manejar un objeto de tipo SqlDataReader creado mediante la ejecución previa
    ''' de un script utilizando el método EjecutarReader
    ''' </summary>
    Public ReadOnly Property Reader() As SqlCeDataReader
        Get
            Return _reader
        End Get
    End Property

#End Region

#Region "Contructores +/Destructor"
    ''' <summary>
    ''' inicializa una conexión a una base de datos externa a la aplicación
    ''' </summary>
    ''' <param name="cadenaConexion">Cadena de conexión a la base de datos con credenciales</param>
    Public Sub New(ByVal cadenaConexion As String)
        'pSql.Assert()
        conexion = New SqlCeConnection(cadenaConexion)
        comando = conexion.CreateCommand
        comando.Connection = conexion
    End Sub

    ''' <summary>
    '''   Inicializa el objeto con la cadena de coneccion que esta configurada en el webconfig
    ''' </summary>
    ''' <remarks>Buscar en el archivo de configuración (Web.config),la llave (CadenaConexion) la cadena de conexión a la base de datos
    ''' </remarks>
    Public Sub New(Optional ByVal esLocal As Integer = 0)
        'pSql.Assert()
        conexion = New SqlCeConnection(ConfigurationSettings.AppSettings("CadenaConexionLocal"))

        comando = conexion.CreateCommand
        comando.Connection = conexion

    End Sub

    'Protected Overrides Sub Finalize()
    '    cerrarConexion(True)
    '    MyBase.Finalize()
    'End Sub
    Public Sub Dispose() Implements System.IDisposable.Dispose
        If _reader IsNot Nothing Then If Not _reader.IsClosed Then _reader.Close()
        If comando IsNot Nothing Then comando.Dispose()
        If transaccion IsNot Nothing Then transaccion.Dispose()
        cerrarConexion(True)
        If conexion IsNot Nothing Then conexion.Dispose()
        _infoConexion = Nothing
        GC.Collect()
        Dispose(True)
    End Sub

    Protected Overridable Sub Dispose(ByVal b As Boolean)
        If Not disposing Then
            disposing = True
            GC.SuppressFinalize(Me)
        End If
    End Sub


#End Region

#Region "Metodos"

#Region "Abrir y Cerrar Conexion"

    ''' <summary>
    ''' Abre conexión a la base de datos 
    ''' </summary>
    ''' <returns>Retorna true solamente si la conexión está abierta</returns>
    ''' <remarks></remarks>
    Public Function abrirConexion() As Boolean
        Try
            With conexion
                If .State = ConnectionState.Closed Then
                    .Open()
                ElseIf .State = ConnectionState.Broken Then
                    .Close()
                    .Open()
                End If
            End With
        Catch ex As SqlCeException
            Throw New Exception("Imposible abrir la conexión a la BD.")
        End Try
        Return CBool(conexion.State)
    End Function

    ''' <summary>
    ''' Cierra la conexión a la base de datos solo si no hay una transaccion pendiente
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Si hay una transacción por confirmar deja la conexoón en el estado que la encuentra</remarks>
    Public Function cerrarConexion() As Boolean

        If comando.Transaction Is Nothing Then
            If conexion.State <> ConnectionState.Closed Then
                conexion.Close()
                Return True
            End If
        End If
    End Function

    ''' <summary>
    ''' realiza Cierre de la conexión a la base de datos de manera forzada
    ''' </summary>
    ''' <param name="forzar">si el parametro es true forza el cierre de la conexión 
    ''' sin importar las transacciones pendientes; si es false tiene en cuenta las transacciones
    ''' </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function cerrarConexion(ByVal forzar As Boolean) As Boolean
        If forzar Then
            If conexion.State <> ConnectionState.Closed Then
                If comando.Transaction Is Nothing Then
                    conexion.Close()
                    Return True
                End If
            End If
        Else
            If comando.Transaction Is Nothing Then
                If conexion.State <> ConnectionState.Closed Then
                    conexion.Close()
                    Return True
                End If
            End If
        End If
    End Function

#End Region

#Region "Sentencias y Trasacciones..."

    ''' <summary>
    ''' Ejecuta una  sentencia SQL la cual  no retorna filas
    ''' </summary>
    ''' <param name="sentencia">sentencia SQL insert,update,delete...</param>
    ''' <param name="tipoComando">Variable de tipo CommandType,Por defecto su valor es tipo Tex</param>
    ''' <returns>True cuando pudo ejecutar la sentenia</returns>
    ''' <remarks>si la clase está en modo transaccional solo se efectuaran los cambios al
    ''' confirmar la transacción.
    ''' si desea ejecutar procedimientos almacenados debe enviar en el parametro tipoComando = CommandType.StoredProcedure
    '''</remarks>
    Public Function ejecutarNonQuery(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As Integer
        If abrirConexion() Then
            comando.CommandType = tipoComando
            comando.CommandText = sentencia
            Try
                Return comando.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                Me.cerrarConexion()
            End Try
        End If
    End Function

    ''' <summary>
    ''' establece la clase en modo transaccional
    ''' </summary>
    ''' <remarks>La conección permanece abierta hasta terminar o abortar la transacción. 
    ''' su uso es el siguente:
    ''' Dim db as new LMDataAcces()
    ''' try
    ''' db.iniciarTransaccion()
    '''   ...Operaciones que conforman la transacción...
    ''' db.confirmarTransaccion()
    ''' cathc
    '''  abortarTransaccion()
    ''' end try
    ''' </remarks>
    Public Sub iniciarTransaccion()
        If abrirConexion() Then
            comando.Connection = conexion
            transaccion = conexion.BeginTransaction()
            comando.Transaction = transaccion
        End If
    End Sub

    ''' <summary>
    ''' Finaliza de manera satisfactoria la transacción
    ''' </summary>
    ''' <remarks>termina el modo transaccional de la clase y cierra la conección</remarks>
    ''' <returns>Retorna True si la transacción fué realizada con exito</returns>
    Public Function confirmarTransaccion() As Boolean
        Try
            transaccion.Commit()
            confirmarTransaccion = True
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            cerrarConexion(True)
            transaccion.Dispose()
        End Try
    End Function

    ''' <summary>
    ''' Revierte las sentencias ejecutadas dentro de la transacción
    ''' </summary>
    ''' <remarks>termina el modo transaccional de la clase y cierra la conección</remarks>
    Public Sub abortarTransaccion()
        If estadoTransaccional Then
            If _reader IsNot Nothing AndAlso Not _reader.IsClosed Then _reader.Close()
            transaccion.Rollback()
            cerrarConexion(True)
            transaccion.Dispose()
        End If
    End Sub

#End Region

#Region "Consultas"

    ''' <summary>
    ''' ejecuta una sentencia y retorna un datareader con los datos leidos;  este debe ser cerrado despues de su implementacion junto con la conexión
    ''' </summary>
    ''' <param name="sentencia">Consulta SQL</param>
    ''' <param name="tipoComando">Para metro opcional que especifica el commandtype; por defecto toma como valor text</param>
    ''' <remarks>Es responsabilidad de la persona que llama a cerrar la conexión y el lector cuando haya terminado.</remarks>
    ''' <returns>retorna un SqlDataReader listo para ser leido en la capa de negocio</returns>
    Public Function ejecutarReader(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As SqlCeDataReader
        Dim requireCerrarConexion As Boolean = False
        If Not Me.conexion.State = ConnectionState.Closed Then requireCerrarConexion = True
        comando.CommandType = tipoComando
        comando.CommandText = sentencia
        abrirConexion()
        _reader = comando.ExecuteReader()
        Return _reader
    End Function

    ''' <summary>
    ''' Ejecuta una consulta SQL y retorna un unico valor; Se omiten todas las demás columnas y filas 
    ''' de más generadas por la consulta
    ''' </summary>
    ''' <param name="sentencia">consulta SQL</param>
    ''' <param name="tipoComando">Para metro opcional que especifica el commandtype; por defecto toma como valor text </param>
    ''' <remarks>Utilice el método EjecutarScalar para recuperar un único valor</remarks>
    ''' <returns>devuelve la primera columna de la primera fila del conjunto de resultados de una sentencia sql</returns>
    Public Function ejecutarScalar(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As Object
        comando.CommandType = tipoComando
        comando.CommandText = sentencia
        If abrirConexion() Then
            Try
                Return comando.ExecuteScalar()
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                cerrarConexion()
            End Try
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' carga un Datable con una consulta SQL
    ''' </summary>
    ''' <param name="sentencia">sentencia SQL tipo texto o procedimiento almacenado</param>
    ''' <param name="tipoComando">Para metro opcional que especifica el commandtype; por defecto toma como valor text</param>
    ''' <returns>devuelve un DataTable con el resultado de la consulta</returns>
    ''' <remarks></remarks>
    Public Function ejecutarDataTable(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As DataTable
        Dim dt As New DataTable
        Dim adaptador As SqlCeDataAdapter
        comando.CommandType = tipoComando
        If abrirConexion() Then
            Try
                comando.CommandText = sentencia
                adaptador = New SqlCeDataAdapter(comando)
                adaptador.Fill(dt)
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                Me.cerrarConexion()
            End Try
        End If
        Return dt
    End Function

    ''' <summary>
    ''' carga un Datable con una consulta SQL, si el DataTable ya tiene datos, los nuevos serán adicionados al final del mismo
    ''' </summary>
    ''' <param name="dt" >DataTable que se llenará con los datos del resultado de la consulta</param>
    ''' <param name="sentencia">sentencia SQL tipo texto o procedimiento almacenado</param>
    ''' <param name="tipoComando">Para metro opcional que especifica el commandtype; por defecto toma como valor text</param>
    ''' <remarks></remarks>

    Public Sub llenarDataTable(ByRef dt As DataTable, ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text)
        If dt Is Nothing Then dt = New DataTable

        Dim adaptador As SqlCeDataAdapter
        comando.CommandType = tipoComando
        Try
            comando.CommandText = sentencia
            adaptador = New SqlCeDataAdapter(comando)
            adaptador.Fill(dt)
        Finally
            If Not Me.conexion Is Nothing Then
                If Me.conexion.State = ConnectionState.Open Then
                    Me.cerrarConexion()
                End If
            End If



        End Try
    End Sub

    ''' <summary>
    ''' Carga un DataSet a parti del resultado de ejecutar una consulta SQL o un procedimiento almacenado
    ''' </summary>
    ''' <param name="sentencia">sentencia SQL tipo texto o procedimiento almacenado</param>
    ''' <param name="tipoComando">Parámetro opcional que especifica el commandtype; por defecto toma como valor text</param>
    ''' <param name="nombreTabla">Parámetro opcional que especifica npmbre de la tabla que se va a llenar con el resultado de la consulta</param>
    ''' <returns>devuelve un DataTable con el resultado de la consulta</returns>
    ''' <remarks></remarks>
    Public Function EjecutarDataSet(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text, _
                                    Optional ByVal nombreTabla As String = "") As DataSet
        Dim ds As New DataSet
        Dim adaptador As SqlCeDataAdapter
        comando.CommandType = tipoComando
        If abrirConexion() Then
            comando.CommandText = sentencia
            adaptador = New SqlCeDataAdapter(comando)
            If nombreTabla.Trim.Length > 0 Then
                adaptador.Fill(ds.Tables(nombreTabla))
            Else
                adaptador.Fill(ds)
            End If
        End If
        Return ds
    End Function


    ''' <summary>
    ''' Carga un DataSet con el resultado de la ejecución de una consulta una consulta SQL
    ''' </summary>
    ''' <param name="ds" >DataSet que se llenará con los datos del resultado de la consulta</param>
    ''' <param name="sentencia">sentencia SQL tipo texto o procedimiento almacenado</param>
    ''' <param name="tipoComando">Para metro opcional que especifica el commandtype; por defecto toma como valor text</param>
    ''' <remarks></remarks>

    Public Sub LlenarDataSet(ByRef ds As DataSet, ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text)
        If ds Is Nothing Then ds = New DataSet
        Try
            comando.CommandText = sentencia
            comando.CommandType = tipoComando
            Using adaptador As New SqlCeDataAdapter(comando)
                adaptador.Fill(ds)
            End Using
        Finally
            If Me.conexion IsNot Nothing Then
                If Me.conexion.State = ConnectionState.Open Then
                    Me.cerrarConexion()
                End If
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Carga una tabla específica de un DataSet con el resultado de la ejecución de una consulta una consulta SQL
    ''' </summary>
    ''' <param name="ds" >DataSet que se llenará con los datos del resultado de la consulta</param>
    ''' <param name="nombreTabla" >Nombre de la tabla contenida en el DataSet que se llenará con los datos del resultado de la consulta</param>
    ''' <param name="sentencia">sentencia SQL tipo texto o procedimiento almacenado</param>
    ''' <param name="tipoComando">Para metro opcional que especifica el commandtype; por defecto toma como valor text</param>
    ''' <remarks></remarks>

    Public Sub LlenarDataSet(ByRef ds As DataSet, ByVal nombreTabla As String, ByVal sentencia As String, _
                             Optional ByVal tipoComando As CommandType = CommandType.Text)
        If ds Is Nothing Then ds = New DataSet
        Try
            comando.CommandText = sentencia
            comando.CommandType = tipoComando
            Using adaptador As New SqlCeDataAdapter(comando)
                adaptador.Fill(ds, nombreTabla)
            End Using
        Finally
            If Me.conexion IsNot Nothing Then
                If Me.conexion.State = ConnectionState.Open Then
                    Me.cerrarConexion()
                End If
            End If
        End Try
    End Sub

#End Region

    ''' <summary>
    ''' carga un parametro SQL al comand de la clase
    ''' </summary>
    ''' <param name="nombre">nombre del parametro SQL ej: @miparametro</param>
    ''' <param name="valor">valor que tomará em parametro</param>
    ''' <param name="tipo"> opcional es eltipo de dato que tomara el valor si no se establece quedará como string</param>
    ''' <remarks>Si se agrega un parametro sin valor este tomará su valor por defecto NULL</remarks>
    Public Sub agregarParametroSQL(ByVal nombre As String, ByVal valor As Object, Optional ByVal tipo As SqlDbType = Nothing)
        If tipo = Nothing Then
            comando.Parameters.Add(nombre, SqlDbType.VarChar).Value = valor
        Else
            comando.Parameters.Add(nombre, tipo).Value = valor
        End If
        comando.Parameters(nombre).IsNullable = True
    End Sub

    ''' <summary>
    ''' Carga un parametro SQL al comand de la clase, especificando obligatoriamente el tipo de dato y la longitud del mismo
    ''' </summary>
    ''' <param name="nombre">nombre del parametro SQL ej: @miparametro</param>
    ''' <param name="valor">valor que tomará em parametro</param>
    ''' <param name="tipo"> opcional es eltipo de dato que tomara el valor si no se establece quedará como string</param>
    ''' <remarks>Si se agrega un parametro sin valor este tomará su valor por defecto NULL</remarks>
    Public Sub agregarParametroSQL(ByVal nombre As String, ByVal valor As Object, ByVal tipo As SqlDbType, ByVal longitud As Integer)
        If tipo = Nothing Then
            comando.Parameters.Add(nombre, SqlDbType.VarChar).Value = valor
        Else
            comando.Parameters.Add(nombre, tipo).Value = valor
        End If
        comando.Parameters(nombre).IsNullable = True
    End Sub

    ''' <summary>
    ''' Obtiene la cadena de conexión
    ''' </summary>
    ''' <returns>cadena de conexión con credenciales</returns>
    ''' <remarks></remarks>
    Public Function getConexion() As String
        Return conexion.ConnectionString
    End Function

    ''' <summary>
    ''' Obtiene la información asociada a la Conexión, a partir de la Cadena de Conexión configurada actualmente
    ''' </summary>
    'Public Shared Function ObtenerInformacionDeConexion() As InfoObjetoConexionCE
    '    Dim infoConexion As New InfoObjetoConexionCE
    '    Dim builder As New SqlConnectionStringBuilder(ConfigurationManager.AppSettings("CadenaConexion"))
    '    With infoConexion
    '        .CadenaConexion = builder.ConnectionString
    '        .NombreBaseDatos = builder.InitialCatalog
    '        .NombreServidor = builder.DataSource
    '        .TiempoEspera = builder.ConnectTimeout
    '        .NombreUsuario = builder.UserID
    '        .Password = builder.Password
    '    End With
    '    builder.Clear()
    '    Return infoConexion
    'End Function

#End Region

End Class

Public Structure InfoObjetoConexionCE
    Dim NombreServidor As String
    Dim NombreBaseDatos As String
    Dim CadenaConexion As String
    Dim TiempoEspera As Integer
    Dim NombreUsuario As String
    Dim Password As String
End Structure
