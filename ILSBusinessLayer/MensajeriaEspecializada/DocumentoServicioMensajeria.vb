Imports LMDataAccessLayer
Imports System.IO
Imports System.Web
Imports ILSBusinessLayer

Namespace MensajeriaEspecializada

    Public Class DocumentoServicioMensajeria

#Region "Atributos (Campos)"

        Private _tablaArchivos As DataTable

        Private _idDocumento As Integer
        Private _idServicio As Integer
        Private _idProducto As Integer
        Private _tipoProducto As String
        Private _nombreDocumento As String
        Private _nombreArchivo As String
        Private _rutaAlmacenamiento As String
        Private _tipoContenido As String
        Private _tamanio As Long
        Private _identificadorUnico As String
        Private _archivo1 As Byte()
        Private _fechaRecepcion As Date
        Private _idUsuarioRecepcion As Integer
        Private _strArchivo As Stream
        Private _esEditable As Boolean
        Private _extension As String
        Private _existeArchivo As Integer

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property TablaArchivos As DataTable
            Get
                Return _tablaArchivos
            End Get
            Set(value As DataTable)
                _tablaArchivos = value
            End Set
        End Property

        Public Property IdDocumento As Integer
            Get
                Return _idDocumento
            End Get
            Set(value As Integer)
                _idDocumento = value
            End Set
        End Property

        Public Property IdServicio As Integer
            Get
                Return _idServicio
            End Get
            Set(value As Integer)
                _idServicio = value
            End Set
        End Property

        Public Property IdProducto As Integer
            Get
                Return _idProducto
            End Get
            Set(value As Integer)
                _idProducto = value
            End Set
        End Property

        Public Property TipoProducto As String
            Get
                Return _tipoProducto
            End Get
            Set(value As String)
                _tipoProducto = value
            End Set
        End Property

        Public Property NombreDocumento As String
            Get
                Return _nombreDocumento
            End Get
            Set(value As String)
                _nombreDocumento = value
            End Set
        End Property

        Public Property NombreArchivo As String
            Get
                Return _nombreArchivo
            End Get
            Set(value As String)
                _nombreArchivo = value
            End Set
        End Property

        Public Property RutaAlmacenamiento As String
            Get
                Return _rutaAlmacenamiento
            End Get
            Set(value As String)
                _rutaAlmacenamiento = value
            End Set
        End Property

        Public Property TipoContenido As String
            Get
                Return _tipoContenido
            End Get
            Set(value As String)
                _tipoContenido = value
            End Set
        End Property

        Public Property Tamanio As Long
            Get
                Return _tamanio
            End Get
            Set(value As Long)
                _tamanio = value
            End Set
        End Property

        Public Property IdentificadorUnico As String
            Get
                Return _identificadorUnico
            End Get
            Set(value As String)
                _identificadorUnico = value
            End Set
        End Property

        Public Property Archivo1 As Byte()
            Get
                Return _archivo1
            End Get
            Set(value As Byte())
                _archivo1 = value
            End Set
        End Property

        Public Property FechaRecepcion As String
            Get
                Return _fechaRecepcion
            End Get
            Set(value As String)
                _fechaRecepcion = value
            End Set
        End Property

        Public Property IdUsuarioRecepcion As Integer
            Get
                Return _idUsuarioRecepcion
            End Get
            Set(value As Integer)
                _idUsuarioRecepcion = value
            End Set
        End Property

        Public Property Archivo As Stream
            Get
                Return _strArchivo
            End Get
            Set(value As Stream)
                _strArchivo = value
            End Set
        End Property

        Public Property Extension As String
            Get
                Return _extension
            End Get
            Set(value As String)
                _extension = value
            End Set
        End Property

        Public Property EsEditable As Boolean
            Get
                Return _esEditable
            End Get
            Set(value As Boolean)
                _esEditable = value
            End Set
        End Property

        Public Property ExisteArchivo As Integer
            Get
                Return _existeArchivo
            End Get
            Set(value As Integer)
                _existeArchivo = value
            End Set
        End Property
        Public Property RutaAlmacenamientoRelativa As String
        Public Property IdTipoDocumento As Integer

        Public Property ImagenBytes As Byte()

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idDocumento As Integer)
            _idDocumento = idDocumento
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If IdDocumento > 0 Then .SqlParametros.Add("@idDocumento", SqlDbType.Int).Value = _idDocumento
                    If IdServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                    .ejecutarReader("ObtenerInfoDocumentoServicioMensajeria", CommandType.StoredProcedure)

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

        Private Sub EstructuraDatosArchivos()
            Try
                Dim dtDatos As New DataTable
                If _tablaArchivos Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("IdDocumento", GetType(Integer)))
                        .Add(New DataColumn("NombreArchivo", GetType(String)))
                        .Add(New DataColumn("NombreDocumento", GetType(String)))
                        .Add(New DataColumn("idProducto", GetType(Integer)))
                        .Add(New DataColumn("TipoContenido", GetType(String)))
                        .Add(New DataColumn("FechaRecepcion", GetType(String)))
                        .Add(New DataColumn("Archivo1", GetType(Byte())))
                        .Add(New DataColumn("Tamanio", GetType(Integer)))
                        .Add(New DataColumn("IdentificadorUnico", GetType(String)))
                        .Add(New DataColumn("RutaAlmacenamiento", GetType(String)))
                        .Add(New DataColumn("Extension", GetType(String)))
                    End With
                    dtDatos.AcceptChanges()
                    _tablaArchivos = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    ''Integer.TryParse(reader("idDocumento"), _idDocumento)
                    ''Integer.TryParse(reader("idServicio"), _idServicio)
                    ''_nombreDocumento = reader("nombreDocumento").ToString()
                    ''_nombreArchivo = reader("nombreArchivo").ToString()
                    ''Integer.TryParse(reader("idProducto"), _idProducto)
                    '''If Not IsDBNull(reader("tipoProducto")) Then
                    '''    _tipoProducto = reader("tipoProducto").ToString()
                    '''End If

                    ''_rutaAlmacenamiento = reader("rutaAlmacenamiento").ToString()

                    ''If IsDBNull(reader("tipoContenido")) = False Then _tipoContenido = reader("tipoContenido").ToString()

                    ''If IsDBNull(reader("tamanio")) = False Then Long.TryParse(reader("tamanio"), _tamanio)
                    ''If IsDBNull(reader("identificadorUnico")) = False Then _identificadorUnico = reader("identificadorUnico")

                    ''_extension = reader("nombreArchivo").ToString.Trim.Split(".").GetValue(1)
                    ''If Not IsDBNull(reader("archivo")) Then
                    ''    _archivo1 = reader("archivo")
                    ''End If
                    '''Dim index As Integer = reader.GetOrdinal("archivo")
                    '''If Not reader.IsDBNull(index) Then
                    '''    _archivo1 = reader("archivo")
                    '''End If
                    ''_fechaRecepcion = reader("fechaRecepcion")
                    ''Integer.TryParse(reader("idUsuarioRecepcion"), _idUsuarioRecepcion)
                    ''If Not String.IsNullOrEmpty(reader("esEditable")) Then Boolean.TryParse(reader("esEditable"), _esEditable)
                    '''If Not IsDBNull(reader("existeArchivo")) Then
                    '''    Integer.TryParse(reader("existeArchivo"), _existeArchivo)
                    '''End If
                    Integer.TryParse(reader("idDocumento"), _idDocumento)
                    Integer.TryParse(reader("idServicio"), _idServicio)
                    _nombreDocumento = reader("nombreDocumento").ToString()
                    _nombreArchivo = reader("nombreArchivo").ToString()
                    _rutaAlmacenamiento = reader("rutaAlmacenamiento").ToString()

                    If IsDBNull(reader("tipoContenido")) = False Then _tipoContenido = reader("tipoContenido").ToString()


                    _tamanio = 0
                    If IsDBNull(reader("tamanio")) = False Then Long.TryParse(reader("tamanio"), _tamanio)
                    If IsDBNull(reader("identificadorUnico")) = False Then _identificadorUnico = reader("identificadorUnico")

                    Date.TryParse(reader("fechaRecepcion"), _fechaRecepcion)
                    Integer.TryParse(reader("idUsuarioRecepcion"), _idUsuarioRecepcion)
                    If Not String.IsNullOrEmpty(reader("esEditable")) Then Boolean.TryParse(reader("esEditable"), _esEditable)


                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar(ByVal idUsuario As Integer) As ResultadoProceso
            Dim respuesta As New ResultadoProceso

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Directory.Exists(_tablaArchivos.Rows(0).Item("RutaAlmacenamiento")) Then
                            Directory.Delete(_tablaArchivos.Rows(0).Item("RutaAlmacenamiento"), True)
                        End If
                        .IniciarTransaccion()
                        For i As Integer = 0 To _tablaArchivos.Rows.Count - 1
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                            .SqlParametros.Add("@nombreDocumento", SqlDbType.VarChar).Value = _tablaArchivos.Rows(i).Item("NombreDocumento")
                            .SqlParametros.Add("@nombreArchivo", SqlDbType.VarChar).Value = _tablaArchivos.Rows(i).Item("NombreArchivo")
                            .SqlParametros.Add("@rutaAlmacenamiento", SqlDbType.VarChar).Value = _tablaArchivos.Rows(i).Item("RutaAlmacenamiento")
                            .SqlParametros.Add("@tipoContenido", SqlDbType.VarChar).Value = _tablaArchivos.Rows(i).Item("TipoContenido")
                            .SqlParametros.Add("@tamanio", SqlDbType.Int).Value = _tablaArchivos.Rows(i).Item("Tamanio")
                            .SqlParametros.Add("@identificadorUnico", SqlDbType.VarChar).Value = _tablaArchivos.Rows(i).Item("IdentificadorUnico")
                            .SqlParametros.Add("@archivo", SqlDbType.VarBinary).Value = _tablaArchivos.Rows(i).Item("Archivo")
                            .SqlParametros.Add("@fechaRecepcion", SqlDbType.Date).Value = _tablaArchivos.Rows(i).Item("FechaRecepcion")
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                            .SqlParametros.Add("@idTipoArchivo", SqlDbType.Int).Value = _tablaArchivos.Rows(i).Item("idProducto")
                            .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .EjecutarNonQuery("RegistrarDocumentoServicioMensajeria", CommandType.StoredProcedure)
                            Integer.TryParse(.SqlParametros("@resultado").Value, respuesta.Valor)

                            If respuesta.Valor = 0 Then
                                'Se guarda el archivo de acuerdo a la ruta de almacenamiento
                                If Not Directory.Exists(_tablaArchivos.Rows(i).Item("RutaAlmacenamiento")) Then
                                    Directory.CreateDirectory(_tablaArchivos.Rows(i).Item("RutaAlmacenamiento"))
                                End If

                                Dim rutaGuardar As String = _tablaArchivos.Rows(i).Item("RutaAlmacenamiento").ToString() & "\" & _tablaArchivos.Rows(i).Item("IdentificadorUnico").ToString() & "." & _tablaArchivos.Rows(i).Item("Extension")
                                Using fsArchivo As FileStream = File.Create(rutaGuardar)
                                    _strArchivo = _tablaArchivos.Rows(i).Item("Archivo")
                                    Dim arrContenido As Byte() = New Byte(_strArchivo.Length - 1) {}
                                    _strArchivo.Read(arrContenido, 0, arrContenido.Length)
                                    fsArchivo.Write(arrContenido, 0, arrContenido.Length)
                                End Using
                            Else
                                Exit For
                            End If
                        Next

                        If respuesta.Valor = 0 Then
                            .ConfirmarTransaccion()
                            respuesta.Mensaje = "Se realizó el registro del documento exitosamente."
                        Else
                            .AbortarTransaccion()
                            respuesta.Mensaje = "Se generó un error al intentar registrar el documento [" & respuesta.Valor & "]"
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                    Throw ex
                End Try
            End Using
            Return respuesta
        End Function
        Public Function ConsultarArchivosMesaControl() As DataTable
            Dim dt As DataTable
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    If IdDocumento > 0 Then .SqlParametros.Add("@idDocumento", SqlDbType.Int).Value = _idDocumento
                    If IdServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                    If IdTipoDocumento > 0 Then .SqlParametros.Add("@idTipoDocumento", SqlDbType.Int).Value = IdTipoDocumento
                    dt = .EjecutarDataTable("ObtenerInfoDocumentoMesaControlServicioMensajeria", CommandType.StoredProcedure)

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dt
        End Function

        Public Function RegistrarMesaControl(ByVal idUsuario As Integer) As ResultadoProceso
            Dim respuesta As New ResultadoProceso

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                        .SqlParametros.Add("@idTipoArchivo", SqlDbType.Int).Value = 4
                        .SqlParametros.Add("@nombreDocumento", SqlDbType.VarChar).Value = _nombreDocumento
                        .SqlParametros.Add("@nombreArchivo", SqlDbType.VarChar).Value = _nombreArchivo
                        .SqlParametros.Add("@rutaAlmacenamiento", SqlDbType.VarChar).Value = _rutaAlmacenamiento
                        .SqlParametros.Add("@tipoContenido", SqlDbType.VarChar).Value = _tipoContenido
                        .SqlParametros.Add("@identificadorUnico", SqlDbType.VarChar).Value = _identificadorUnico
                        .SqlParametros.Add("@tamanio", SqlDbType.Int).Value = _tamanio
                        If _fechaRecepcion <> Date.MinValue Then .SqlParametros.Add("@fechaRecepcion", SqlDbType.Date).Value = _fechaRecepcion
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@archivo", SqlDbType.VarBinary).Value = ImagenBytes
                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .TiempoEsperaComando = 0
                        .IniciarTransaccion()
                        .EjecutarNonQuery("RegistrarDocumentoMesaControlServicioMensajeria", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value, respuesta.Valor)

                        If respuesta.Valor = 0 Then
                            If Not Directory.Exists(RutaAlmacenamientoRelativa) Then
                                Directory.CreateDirectory(RutaAlmacenamientoRelativa)
                            End If
                            Using fsArchivo As FileStream = File.Create(RutaAlmacenamientoRelativa & _nombreArchivo)
                                Dim arrContenido As Byte() = New Byte(_strArchivo.Length - 1) {}
                                _strArchivo.Read(arrContenido, 0, arrContenido.Length)
                                fsArchivo.Write(arrContenido, 0, arrContenido.Length)
                            End Using
                            .ConfirmarTransaccion()
                            respuesta.Mensaje = "Se realizó el registro del documento exitosamente."
                        Else
                            .AbortarTransaccion()
                            respuesta.Mensaje = "Se generó un error al intentar registrar el documento [" & respuesta.Valor & "]"
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                    respuesta.Valor = 1000
                    respuesta.Mensaje = "Se generó un error al intentar registrar el documento " & ex.Message
                End Try
            End Using
            Return respuesta
        End Function

        Public Sub RegistrarDocumentos()
            Dim respuesta As New ResultadoProceso
            Using dbManager As New LMDataAccess
                _tablaArchivos.Columns.Add(New DataColumn("idServicio", GetType(System.Int64), _idServicio))
                _tablaArchivos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), _idUsuarioRecepcion))
                _tablaArchivos.AcceptChanges()
                With dbManager
                    Try
                        .InicilizarBulkCopy()
                        .TiempoEsperaComando = 0
                        With .BulkCopy
                            .DestinationTableName = "DocumentoServicioMensajeria"
                            .ColumnMappings.Add("idServicio", "idServicio")
                            .ColumnMappings.Add("NombreDocumento", "nombreDocumento")
                            .ColumnMappings.Add("NombreArchivo", "nombreArchivo")
                            .ColumnMappings.Add("RutaAlmacenamiento", "rutaAlmacenamiento")
                            .ColumnMappings.Add("TipoContenido", "tipoContenido")
                            .ColumnMappings.Add("Tamanio", "tamanio")
                            .ColumnMappings.Add("IdentificadorUnico", "identificadorUnico")
                            .ColumnMappings.Add("Archivo1", "archivo")
                            .ColumnMappings.Add("FechaRecepcion", "fechaRecepcion")
                            .ColumnMappings.Add("idUsuario", "idUsuarioRecepcion")
                            .ColumnMappings.Add("IdProducto", "idTipoDocumento")
                            .WriteToServer(_tablaArchivos)
                        End With
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try
                End With
            End Using
        End Sub

        Public Function Actualizar(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idDocumento", SqlDbType.Int).Value = _idDocumento
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@identificadorUnico", SqlDbType.UniqueIdentifier).Value = _identificadorUnico
                        If _idServicio > 0 Then .Add("@idServicio", SqlDbType.Int).Value = _idServicio
                        If Not String.IsNullOrEmpty(_nombreDocumento) Then .Add("@nombreDocumento", SqlDbType.VarChar).Value = _nombreDocumento
                        If Not String.IsNullOrEmpty(_nombreArchivo) Then .Add("@nombreArchivo", SqlDbType.VarChar).Value = _nombreArchivo
                        If Not String.IsNullOrEmpty(_rutaAlmacenamiento) Then .Add("@rutaAlmacenamiento", SqlDbType.VarChar).Value = _rutaAlmacenamiento
                        If Not String.IsNullOrEmpty(_tipoContenido) Then .Add("@tipoContenido", SqlDbType.VarChar).Value = _tipoContenido
                        If _tamanio > 0 Then .Add("@tamanio", SqlDbType.Int).Value = _tamanio
                        If _fechaRecepcion <> Date.MinValue Then .Add("@fechaRecepcion", SqlDbType.Date).Value = _fechaRecepcion
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ActualizarDocumentoServicioMensajeria", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                            'Se guarda el archivo de acuerdo a la ruta de almacenamiento
                            Dim ruta As String = String.Empty
                            Dim rutaAlmacenaArchivo As Comunes.ConfigValues = New Comunes.ConfigValues("RUTACARGUEARCHIVOSTRANCITORIOS")
                            If (rutaAlmacenaArchivo.ConfigKeyValue IsNot Nothing) Then
                                ruta = rutaAlmacenaArchivo.ConfigKeyValue
                            Else
                                Throw New Exception("No fue posible establecer la ruta de almacenamiento de los archivos por favor contacte a IT para configurar en ConfigValues RUTACARGUEARCHIVOSTRANCITORIOS ")
                            End If

                            If Not Directory.Exists(ruta & _rutaAlmacenamiento) Then
                                Directory.CreateDirectory(ruta & _rutaAlmacenamiento)
                            End If

                            Dim rutaGuardar As String = ruta & _rutaAlmacenamiento & "\" & _identificadorUnico.ToString()
                            Using fsArchivo As FileStream = File.Create(rutaGuardar)
                                Dim arrContenido As Byte() = New Byte(_strArchivo.Length - 1) {}
                                _strArchivo.Read(arrContenido, 0, arrContenido.Length)
                                fsArchivo.Write(arrContenido, 0, arrContenido.Length)
                            End Using
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(500, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente. ")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(400, "Se generó un error al intentar actualizar el documento: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function InsertarArchivo() As DataTable
            Try
                If TablaArchivos Is Nothing Then
                    EstructuraDatosArchivos()
                End If
                With TablaArchivos
                    Dim drArchivo As DataRow = .NewRow()
                    With drArchivo
                        .Item("IdDocumento") = _tablaArchivos.Rows.Count + 1
                        .Item("NombreArchivo") = _nombreArchivo
                        .Item("TipoContenido") = _tipoContenido
                        .Item("FechaRecepcion") = _fechaRecepcion
                        '.Item("Archivo") = _strArchivo
                        'Dim arrContenido As Byte() = New Byte(_strArchivo.Length - 1) {}
                        .Item("Archivo1") = _archivo1
                        .Item("IdentificadorUnico") = _identificadorUnico
                        .Item("Tamanio") = _tamanio
                        .Item("RutaAlmacenamiento") = _rutaAlmacenamiento
                        .Item("Extension") = _extension
                    End With
                    .Rows.Add(drArchivo)
                    .AcceptChanges()
                End With
            Catch ex As Exception
                Throw ex
            End Try
            Return _tablaArchivos
        End Function

        Public Sub ActualizarDocumentos()
            Using dbManager As New LMDataAccess
                _tablaArchivos.Columns.Add(New DataColumn("idServicio", GetType(System.Int64), _idServicio))
                _tablaArchivos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), _idUsuarioRecepcion))
                _tablaArchivos.AcceptChanges()
                With dbManager
                    Try
                        With .SqlParametros
                            .Add("@idServicio", SqlDbType.Int).Value = _idServicio
                            .Add("@idUsuario", SqlDbType.Int).Value = IdUsuarioRecepcion
                        End With
                        .IniciarTransaccion()
                        .EjecutarNonQuery("ActualizaDocumentosFinancieros", CommandType.StoredProcedure)
                        .InicilizarBulkCopy()
                        .TiempoEsperaComando = 10000
                        With .BulkCopy
                            .DestinationTableName = "DocumentoServicioMensajeria"
                            .ColumnMappings.Add("idServicio", "idServicio")
                            .ColumnMappings.Add("NombreDocumento", "nombreDocumento")
                            .ColumnMappings.Add("NombreArchivo", "nombreArchivo")
                            .ColumnMappings.Add("RutaAlmacenamiento", "rutaAlmacenamiento")
                            .ColumnMappings.Add("TipoContenido", "tipoContenido")
                            .ColumnMappings.Add("Tamanio", "tamanio")
                            .ColumnMappings.Add("IdentificadorUnico", "identificadorUnico")
                            .ColumnMappings.Add("Archivo1", "archivo")
                            .ColumnMappings.Add("FechaRecepcion", "fechaRecepcion")
                            .ColumnMappings.Add("idUsuario", "idUsuarioRecepcion")
                            .ColumnMappings.Add("IdProducto", "idTipoDocumento")
                            .WriteToServer(_tablaArchivos)
                        End With
                        .ConfirmarTransaccion()
                    Catch ex As Exception
                        If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                        Throw New Exception(ex.Message, ex)
                    End Try
                End With
            End Using
        End Sub
        Public Function RegistrarDestruccionDocumento(ByVal idUsuario As Integer) As ResultadoProceso
            Dim respuesta As New ResultadoProceso

            Using dbManager As New LMDataAccess
                Try
                    With dbManager

                        .SqlParametros.Add("@idTipoArchivo", SqlDbType.Int).Value = 5
                        .SqlParametros.Add("@nombreDocumento", SqlDbType.VarChar).Value = _nombreDocumento
                        .SqlParametros.Add("@nombreArchivo", SqlDbType.VarChar).Value = _nombreArchivo
                        .SqlParametros.Add("@rutaAlmacenamiento", SqlDbType.VarChar).Value = _rutaAlmacenamiento
                        .SqlParametros.Add("@tipoContenido", SqlDbType.VarChar).Value = _tipoContenido
                        .SqlParametros.Add("@identificadorUnico", SqlDbType.VarChar).Value = _identificadorUnico
                        .SqlParametros.Add("@tamanio", SqlDbType.Int).Value = _tamanio
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@archivo", SqlDbType.VarBinary).Value = ImagenBytes
                        .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .TiempoEsperaComando = 0
                        .IniciarTransaccion()
                        .EjecutarNonQuery("DestruccionDocumentos", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@resultado").Value, respuesta.Valor)

                        If respuesta.Valor = 0 Then
                            .ConfirmarTransaccion()
                            respuesta.Mensaje = "Se realizó el registro de destruccion exitosamente."
                        Else
                            .AbortarTransaccion()
                            respuesta.Mensaje = "Se generó un error al intentar registrar la destruccion [" & respuesta.Valor & "]"
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                    respuesta.Valor = 1000
                    respuesta.Mensaje = "Se generó un error al intentar registrar destruccion" & ex.Message
                End Try
            End Using
            Return respuesta
        End Function
        Public Function EliminarDocumento(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idDocumento", SqlDbType.Int).Value = _idDocumento
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .TiempoEsperaComando = 0
                    .EjecutarNonQuery("EliminarDocumento", CommandType.StoredProcedure)
                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                    Else
                        resultado.EstablecerMensajeYValor(500, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente. ")
                    End If
                End With
            Catch ex As Exception
                resultado.EstablecerMensajeYValor(400, "Se generó un error al intentar actualizar el documento: " & ex.Message)
            End Try
            Return resultado
        End Function
#End Region

    End Class

End Namespace

