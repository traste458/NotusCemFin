Imports System.Data.SqlClient
Imports System.Configuration
Imports GemBox.Spreadsheet
Imports System.Threading.Tasks

Public Class LMDataAccess
    Implements IDisposable


#Region "variables"
    Private disposing As Boolean

    Private conexion As SqlConnection
    ''' <summary>
    ''' command encargado de ejecutar las sentencias SQL toda la clase
    ''' </summary>
    ''' <remarks></remarks>
    Private comando As SqlCommand
    Private transaccion As SqlTransaction
    Private myBulkCopy As SqlBulkCopy
    Private _infoConexion As InfoObjetoConexion
    Private _reader As SqlDataReader
    Public Shared _dbSeleccionada As String

    Private pSql As New SqlClientPermission(System.Security.Permissions.PermissionState.Unrestricted)
#End Region

#Region "Propiedades"

    ''' <summary>
    ''' indica el estado de la conexion
    ''' </summary>
    Public ReadOnly Property EstadoConexion() As System.Data.ConnectionState
        Get
            Return conexion.State
        End Get
    End Property

    ''' <summary>
    ''' flag que indica si el comando de la clase etá en modo transaccional
    ''' </summary>
    ''' <value></value>
    ''' <returns>true si se inicío la transacción, delo contrario false </returns>
    Public ReadOnly Property EstadoTransaccional() As Boolean
        Get
            If transaccion Is Nothing OrElse transaccion.Connection Is Nothing OrElse
                (transaccion.Connection.State <> ConnectionState.Open AndAlso transaccion.Connection.State <> ConnectionState.Executing _
                 AndAlso transaccion.Connection.State <> ConnectionState.Fetching) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Public ReadOnly Property BulkCopy() As SqlBulkCopy
        Get
            Return myBulkCopy
        End Get
    End Property

    Public ReadOnly Property TransaccionSQL() As SqlTransaction
        Get
            Return transaccion
        End Get
    End Property

    Public ReadOnly Property ConexionSQL() As SqlConnection
        Get
            Return conexion
        End Get
    End Property

    Public ReadOnly Property SqlParametros() As SqlParameterCollection
        Get
            Return comando.Parameters

        End Get
    End Property

    Public Shared Property DbSeleccionada As String
        Get
            Return _dbSeleccionada
        End Get
        Set(value As String)
            _dbSeleccionada = value
        End Set
    End Property

    ''' <summary>
    ''' obtiene o establece un parametro de la lista de parametos del comando
    ''' </summary>
    ''' <param name="index">indexación de base cero: indice a retornar de la lista de parametros del comando</param>
    ''' <value></value>
    ''' <returns>SqlClient.SqlParameter</returns>
    ''' <remarks>se puede utilizar para acceder directamente los paremetros establecidos al command de la clase</remarks>
    Property SqlParametros(ByVal index As Integer) As SqlParameter
        Get
            If comando.Parameters.Count > 0 Then
                Return comando.Parameters.Item(index)
            Else
                Throw New ArgumentOutOfRangeException
            End If
        End Get
        Set(ByVal Value As SqlParameter)
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
    Property SqlParametros(ByVal Nombre As String) As SqlParameter
        Get
            If comando.Parameters.Count > 0 Then
                Return comando.Parameters.Item(Nombre)
            Else
                Throw New ArgumentOutOfRangeException
            End If
        End Get
        Set(ByVal Value As SqlParameter)
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
            If myBulkCopy IsNot Nothing Then myBulkCopy.BulkCopyTimeout = value
        End Set
    End Property

    ''' <summary>
    ''' Obtiene la información asociada a la Conexión actual
    ''' </summary>
    Public ReadOnly Property InformacionDeConexion() As InfoObjetoConexion
        Get
            Dim builder As New SqlConnectionStringBuilder(conexion.ConnectionString)
            _infoConexion = New InfoObjetoConexion
            With _infoConexion
                .CadenaConexion = conexion.ConnectionString
                .NombreBaseDatos = conexion.Database
                .NombreServidor = conexion.DataSource
                .TiempoEspera = conexion.ConnectionTimeout
                .NombreUsuario = builder.UserID
                .Password = builder.Password
            End With
            builder.Clear()
            Return _infoConexion
        End Get
    End Property

    ''' <summary>
    ''' Permite manejar un objeto de tipo SqlDataReader creado mediante la ejecución previa
    ''' de un script utilizando el método EjecutarReader
    ''' </summary>
    Public ReadOnly Property Reader() As SqlDataReader
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
        pSql.Assert()
        conexion = New SqlConnection(cadenaConexion)
        comando = conexion.CreateCommand
        comando.Connection = conexion
    End Sub

    ''' <summary>
    '''   Inicializa el objeto con la cadena de coneccion que esta configurada en el webconfig
    ''' </summary>
    ''' <remarks>Buscar en el archivo de configuración (Web.config),la llave (CadenaConexion) la cadena de conexión a la base de datos
    ''' </remarks>
    Public Sub New()
        pSql.Assert()
        If DbSeleccionada IsNot Nothing Then
            conexion = New SqlConnection(ConfigurationManager.AppSettings(DbSeleccionada))
        Else
            conexion = New SqlConnection(ConfigurationManager.AppSettings("CadenaConexion"))
        End If
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
        If myBulkCopy IsNot Nothing Then myBulkCopy.Close()
        CcerrarConexion(True)
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
    Public Function AbrirConexion() As Boolean
        Try
            With conexion
                If .State = ConnectionState.Closed Then
                    .Open()
                ElseIf .State = ConnectionState.Broken Then
                    .Close()
                    .Open()
                End If
            End With
        Catch ex As SqlException
            Throw New Exception("Imposible abrir la conexión a la BD.")
        End Try
        Return CBool(conexion.State)
    End Function

    Public Async Function AbrirConexionAsync() As Task(Of Boolean)

        With conexion
            If .State = ConnectionState.Closed Then
                Await .OpenAsync()
            ElseIf .State = ConnectionState.Broken Then
                .Close()
                Await .OpenAsync()
            End If
        End With

        Return CBool(conexion.State)
    End Function

    ''' <summary>
    ''' Cierra la conexión a la base de datos solo si no hay una transaccion pendiente
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>Si hay una transacción por confirmar deja la conexoón en el estado que la encuentra</remarks>
    Public Function CerrarConexion() As Boolean

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
    Private Function CcerrarConexion(ByVal forzar As Boolean) As Boolean
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
    Public Function EjecutarNonQuery(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As Integer
        If AbrirConexion() Then
            comando.CommandType = tipoComando
            comando.CommandText = sentencia
            Try
                Return comando.ExecuteNonQuery()
            Finally
                Me.CerrarConexion()
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
    Public Sub IniciarTransaccion()
        If AbrirConexion() Then
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
    Public Function ConfirmarTransaccion() As Boolean
        Try
            transaccion.Commit()
            ConfirmarTransaccion = True
        Finally
            CcerrarConexion(True)
            transaccion.Dispose()
            transaccion = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Revierte las sentencias ejecutadas dentro de la transacción
    ''' </summary>
    ''' <remarks>termina el modo transaccional de la clase y cierra la conección</remarks>
    Public Sub AbortarTransaccion()
        If EstadoTransaccional Then
            If _reader IsNot Nothing AndAlso Not _reader.IsClosed Then _reader.Close()
            Try
                transaccion.Rollback()
                CcerrarConexion(True)
            Finally
                transaccion.Dispose()
                transaccion = Nothing
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Abre la conección e Inicializa el opjeto BulkCopy en la capa de datos.
    ''' Cuando la capa de datos no está en modo transaccional se debe cerrar la connección despues de implementar el BulkCopy
    ''' </summary>
    ''' <param name="CopyOptions"></param>
    ''' <remarks>Se de  cerrar la connección despues de implementar el BulkCopy</remarks>
    Public Sub InicilizarBulkCopy(Optional ByVal CopyOptions As SqlBulkCopyOptions = SqlBulkCopyOptions.Default)
        If transaccion Is Nothing Then
            If AbrirConexion() Then
                myBulkCopy = New SqlBulkCopy(conexion)
            End If
        Else
            myBulkCopy = New SqlBulkCopy(conexion, CopyOptions, transaccion)
        End If
    End Sub

    Public Sub InicilizarBulkCopy(ByVal cadenaConexion As String)
        myBulkCopy = New SqlBulkCopy(cadenaConexion)
    End Sub

    Public Sub InicilizarBulkCopy(ByVal cadenaConexion As String, ByVal CopyOptions As SqlBulkCopyOptions)
        myBulkCopy = New SqlBulkCopy(cadenaConexion, CopyOptions)
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
    Public Function ejecutarReader(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As SqlDataReader
        Dim requireCerrarConexion As Boolean = False
        If Not Me.conexion.State = ConnectionState.Closed Then requireCerrarConexion = True
        comando.CommandType = tipoComando
        comando.CommandText = sentencia
        AbrirConexion()
        _reader = comando.ExecuteReader()
        Return _reader
    End Function

    Public Async Function EjecutarReaderAsync(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As Task(Of SqlDataReader)
        Dim requireCerrarConexion As Boolean = False
        If Not Me.conexion.State = ConnectionState.Closed Then requireCerrarConexion = True
        comando.CommandType = tipoComando
        comando.CommandText = sentencia
        Await AbrirConexionAsync()
        _reader = Await comando.ExecuteReaderAsync()
        Return _reader
    End Function

    ''' <summary>
    ''' ejecuta una sentencia y Crea un archivo de Excel ;  este debe ser cerrado despues de su implementacion junto con la conexión
    ''' </summary>
    ''' <param name="sentencia">Consulta SQL</param>
    ''' <param name="pTitulo">Título que va a tener el reporte en el archivo Excel </param>
    ''' <param name="pFilaInicial">Es la fila en la que se va a iniciar a escribir el reporte en el archivo Excel</param>
    ''' <param name="pNombrePlantilla">Ruta y nombre donde esta almacenada la plantilla donde se va a escribir</param>
    ''' <param name="pNombreArchivo">Ruta y nombre del archivo que se va a generar </param>
    ''' <param name="tipoComando">Para metro opcional que especifica el commandtype; por defecto toma como valor text</param>
    ''' <remarks>Es responsabilidad de la persona que llama a cerrar la conexión y el lector cuando haya terminado.</remarks>
    ''' <returns>retorna la ruta y nombre del archivo almacenado </returns>
    ''' 
    Public Function GenerarArchivoExcel(ByVal sentencia As String, ByVal pNombreArchivo As String, Optional ByVal tipoComando As CommandType = CommandType.Text,
                                        Optional ByVal pNombrePlantilla As String = "", Optional ByVal pTitulo As String = "", Optional ByVal pFilaInicial As Integer = 3) As InfoResultado

        Dim resultado As New InfoResultado

        GemBox.Spreadsheet.SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
        Me.ejecutarReader(sentencia, tipoComando)
        If Me._reader IsNot Nothing AndAlso Me._reader.HasRows Then
            Dim oExcel As New ExcelFile
            Dim oWs As ExcelWorksheet
            If System.IO.File.Exists(pNombrePlantilla) Then
                Select Case System.IO.Path.GetExtension(pNombrePlantilla).ToUpper()
                    Case ".XLS"
                        oExcel.LoadXls(pNombrePlantilla, XlsxOptions.PreserveMakeCopy)
                    Case ".XLSX"
                        oExcel.LoadXlsx(pNombrePlantilla, XlsxOptions.PreserveMakeCopy)
                    Case ".CSV"
                        oExcel.LoadCsv(pNombrePlantilla, XlsxOptions.PreserveMakeCopy)
                    Case Else
                        oExcel.LoadCsv(pNombrePlantilla, GemBox.Spreadsheet.CsvType.TabDelimited)
                End Select
                oWs = oExcel.Worksheets.ActiveWorksheet
            Else
                oWs = oExcel.Worksheets.Add("Hoja 1")
                'Throw New Exception("No se encontró la Plantilla Por favor contacte el Grupo de IT " + pNombrePlantilla)
            End If
            While _reader IsNot Nothing AndAlso _reader.Read
                For ind As Integer = 0 To _reader.FieldCount - 1
                    If TypeOf _reader(ind) Is Date Then
                        Date.TryParse(_reader(ind).ToString, oWs.Cells(pFilaInicial, ind).Value)
                    Else
                        oWs.Cells(pFilaInicial, ind).Value = _reader(ind).ToString
                    End If
                Next
                pFilaInicial += 1
                resultado.Valor += 1
            End While

            If Not String.IsNullOrEmpty(pTitulo) Then oWs.Cells("A1").Value = pTitulo

            'oExcel.SaveXlsx(pNombreArchivo)
            Select Case System.IO.Path.GetExtension(pNombreArchivo).ToUpper()
                Case ".XLS"
                    oExcel.SaveXls(pNombreArchivo)
                Case ".XLSX"
                    oExcel.SaveXlsx(pNombreArchivo)
                Case ".CSV"
                    oExcel.SaveCsv(pNombreArchivo, CsvType.CommaDelimited)
                Case Else
                    oExcel.SaveCsv(pNombreArchivo, CsvType.CommaDelimited)
            End Select

            resultado.Mensaje = pNombreArchivo
        Else
            resultado.Mensaje = "La consulta no arrojó registros. Por favor valide los filtros de búsqueda aplicados."
        End If

        Return resultado

    End Function

    ''' <summary>
    ''' Ejecuta una consulta SQL y retorna un unico valor; Se omiten todas las demás columnas y filas 
    ''' de más generadas por la consulta
    ''' </summary>
    ''' <param name="sentencia">consulta SQL</param>
    ''' <param name="tipoComando">Para metro opcional que especifica el commandtype; por defecto toma como valor text </param>
    ''' <remarks>Utilice el método EjecutarScalar para recuperar un único valor</remarks>
    ''' <returns>devuelve la primera columna de la primera fila del conjunto de resultados de una sentencia sql</returns>
    Public Function EjecutarScalar(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As Object
        comando.CommandType = tipoComando
        comando.CommandText = sentencia
        If AbrirConexion() Then
            Try
                Return comando.ExecuteScalar()
            Finally
                CerrarConexion()
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
    Public Function EjecutarDataTable(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As DataTable
        Dim dt As New DataTable
        Dim adaptador As SqlDataAdapter
        comando.CommandType = tipoComando
        If AbrirConexion() Then
            Try
                comando.CommandText = sentencia
                adaptador = New SqlDataAdapter(comando)
                adaptador.Fill(dt)
            Finally
                Me.CerrarConexion()
            End Try
        End If
        Return dt
    End Function

    Public Async Function EjecutarDataTableAsync(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text) As Task(Of DataTable)
        Dim dt As New DataTable

        If Await AbrirConexionAsync() Then
            Try
                comando.CommandType = tipoComando
                comando.CommandText = sentencia
                Dim reader As IDataReader = Await comando.ExecuteReaderAsync()
                dt.Load(reader)
            Finally
                Me.CerrarConexion()
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
    Public Sub LlenarDataTable(ByRef dt As DataTable, ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text)
        If dt Is Nothing Then dt = New DataTable

        Dim adaptador As SqlDataAdapter
        comando.CommandType = tipoComando
        Try
            comando.CommandText = sentencia
            adaptador = New SqlDataAdapter(comando)
            adaptador.Fill(dt)
        Finally
            If Not Me.conexion Is Nothing Then
                If Me.conexion.State = ConnectionState.Open Then
                    Me.CerrarConexion()
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
    Public Function EjecutarDataSet(ByVal sentencia As String, Optional ByVal tipoComando As CommandType = CommandType.Text,
                                    Optional ByVal nombreTabla As String = "") As DataSet
        Dim ds As New DataSet
        Dim adaptador As SqlDataAdapter
        comando.CommandType = tipoComando
        If AbrirConexion() Then
            comando.CommandText = sentencia
            adaptador = New SqlDataAdapter(comando)
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
            Using adaptador As New SqlDataAdapter(comando)
                adaptador.Fill(ds)
            End Using
        Finally
            If Me.conexion IsNot Nothing Then
                If Me.conexion.State = ConnectionState.Open Then
                    Me.CerrarConexion()
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

    Public Sub LlenarDataSet(ByRef ds As DataSet, ByVal nombreTabla As String, ByVal sentencia As String,
                             Optional ByVal tipoComando As CommandType = CommandType.Text)
        If ds Is Nothing Then ds = New DataSet
        Try
            comando.CommandText = sentencia
            comando.CommandType = tipoComando
            Using adaptador As New SqlDataAdapter(comando)
                adaptador.Fill(ds, nombreTabla)
            End Using
        Finally
            If Me.conexion IsNot Nothing Then
                If Me.conexion.State = ConnectionState.Open Then
                    Me.CerrarConexion()
                End If
            End If
        End Try
    End Sub

#End Region

    ''' <summary>
    ''' carga un parametro SQL tipo al comand de la clase
    ''' </summary>
    ''' <param name="nombre">nombre del parametro SQL ej: @miparametro</param>
    ''' <param name="valor">valor que tomará em parametro</param>
    ''' <remarks>Si se agrega un parametro sin valor este tomará su valor por defecto NULL</remarks>
    Public Sub AgregarParametroObjetoSQL(ByVal nombre As String, ByVal valor As Object)
        comando.Parameters.AddWithValue(nombre, valor)
        comando.Parameters(nombre).IsNullable = True
    End Sub

    ''' <summary>
    ''' carga un parametro SQL al comand de la clase
    ''' </summary>
    ''' <param name="nombre">nombre del parametro SQL ej: @miparametro</param>
    ''' <param name="valor">valor que tomará em parametro</param>
    ''' <param name="tipo"> opcional es eltipo de dato que tomara el valor si no se establece quedará como string</param>
    ''' <remarks>Si se agrega un parametro sin valor este tomará su valor por defecto NULL</remarks>
    Public Sub AgregarParametroSQL(ByVal nombre As String, ByVal valor As Object, Optional ByVal tipo As SqlDbType = Nothing)
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
    Public Sub AgregarParametroSQL(ByVal nombre As String, ByVal valor As Object, ByVal tipo As SqlDbType, ByVal longitud As Integer)
        If tipo = Nothing Then
            comando.Parameters.Add(nombre, SqlDbType.VarChar).Value = valor
        Else
            comando.Parameters.Add(nombre, tipo, longitud).Value = valor
        End If
        comando.Parameters(nombre).IsNullable = True
    End Sub

    ''' <summary>
    ''' Obtiene la cadena de conexión
    ''' </summary>
    ''' <returns>cadena de conexión con credenciales</returns>
    ''' <remarks></remarks>
    Public Function GetConexion() As String
        Return conexion.ConnectionString
    End Function

    ''' <summary>
    ''' Obtiene la información asociada a la Conexión, a partir de la Cadena de Conexión configurada actualmente
    ''' </summary>
    Public Shared Function ObtenerInformacionDeConexion() As InfoObjetoConexion
        Dim infoConexion As New InfoObjetoConexion
        Dim builder As New SqlConnectionStringBuilder(ConfigurationManager.AppSettings("CadenaConexion"))
        With infoConexion
            .CadenaConexion = builder.ConnectionString
            .NombreBaseDatos = builder.InitialCatalog
            .NombreServidor = builder.DataSource
            .TiempoEspera = builder.ConnectTimeout
            .NombreUsuario = builder.UserID
            .Password = builder.Password
        End With
        builder.Clear()
        Return infoConexion
    End Function

#End Region

End Class

Public Structure InfoObjetoConexion
    Dim NombreServidor As String
    Dim NombreBaseDatos As String
    Dim CadenaConexion As String
    Dim TiempoEspera As Integer
    Dim NombreUsuario As String
    Dim Password As String
End Structure

Public Class InfoResultado

#Region "Atributos"

    Private _valor As Integer
    Private _mensaje As String

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _mensaje = ""
    End Sub

    Public Sub New(ByVal valor As Integer, ByVal mensaje As String)
        MyBase.New()
        _valor = valor
        _mensaje = mensaje
    End Sub

#End Region

#Region "Propiedades"

    Public Property Valor() As Integer
        Get
            Return _valor
        End Get
        Set(ByVal value As Integer)
            _valor = value
        End Set
    End Property

    Public Property Mensaje() As String
        Get
            Return _mensaje
        End Get
        Set(ByVal value As String)
            _mensaje = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Sub EstablecerMensajeYValor(ByVal valor As Integer, ByVal mensaje As String)
        _valor = valor
        _mensaje = mensaje
    End Sub

#End Region

End Class
