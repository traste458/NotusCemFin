Imports LMDataAccessLayer
Imports System.IO
Imports System.Web

Public Class DocumentoTemporalServicioMensajeria

#Region "Atributos (Campos)"

    Private _idRegistro As Integer
    Private _idServicio As Integer
    Private _nombreDocumento As String
    Private _nombreArchivo As String
    Private _rutaAlmacenamiento As String
    Private _tipoContenido As String
    Private _tamanio As Long
    Private _identificadorUnico As Guid
    Private _fechaRecepcion As Date
    Private _idUsuarioRecepcion As Integer
    Private _strArchivo As Stream
    Private _pedidoSAP As String
    Private _documentoSAP As String


    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdRegistro As Integer
        Get
            Return _idRegistro
        End Get
        Set(value As Integer)
            _idRegistro = value
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

    Public Property IdentificadorUnico As Guid
        Get
            Return _identificadorUnico
        End Get
        Set(value As Guid)
            _identificadorUnico = value
        End Set
    End Property

    Public Property FechaRecepcion As Date
        Get
            Return _fechaRecepcion
        End Get
        Set(value As Date)
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

    Public Property PedidoSAP As String
        Get
            Return _pedidoSAP
        End Get
        Set(value As String)
            _pedidoSAP = value
        End Set
    End Property

    Public Property DocumentoSAP As String
        Get
            Return _documentoSAP
        End Get
        Set(value As String)
            _documentoSAP = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idRegistro As Integer)
        _idRegistro = idRegistro
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idRegistro > 0 Then .SqlParametros.Add("@idRegistro", SqlDbType.BigInt).Value = _idRegistro
                If _idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicio
                If _idUsuarioRecepcion > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioRecepcion
                .ejecutarReader("ConsultarDocumentosTemporales", CommandType.StoredProcedure)

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

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idRegistro"), _idRegistro)
                If Not IsDBNull(reader("idServicio")) Then Integer.TryParse(reader("idServicio"), _idServicio)
                If Not IsDBNull(reader("nombreDocumento")) Then _nombreDocumento = reader("nombreDocumento").ToString()
                If Not IsDBNull(reader("nombreArchivo")) Then _nombreArchivo = reader("nombreArchivo").ToString()
                If Not IsDBNull(reader("rutaAlmacenamiento")) Then _rutaAlmacenamiento = reader("rutaAlmacenamiento").ToString()
                If Not IsDBNull(reader("tipoContenido")) Then _tipoContenido = reader("tipoContenido").ToString()
                If Not IsDBNull(reader("tamanio")) Then Long.TryParse(reader("tamanio"), _tamanio)
                If Not IsDBNull(reader("identificadorUnico")) Then _identificadorUnico = reader("identificadorUnico")
                If Not IsDBNull(reader("fechaRecepcion")) Then Date.TryParse(reader("fechaRecepcion"), _fechaRecepcion)
                If Not IsDBNull(reader("idUsuarioRecepcion")) Then Integer.TryParse(reader("idUsuarioRecepcion"), _idUsuarioRecepcion)
            End If
        End If
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function RegistrarDocumentosTemporales(ByVal idUsuario As Integer, ByVal idTipoDocumento As Integer, Optional ByVal idServicio As Integer = 0) As ResultadoProceso
        Dim respuesta As New ResultadoProceso

        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@nombreDocumento", SqlDbType.VarChar).Value = _nombreDocumento
                    .SqlParametros.Add("@nombreArchivo", SqlDbType.VarChar).Value = _nombreArchivo
                    .SqlParametros.Add("@rutaAlmacenamiento", SqlDbType.VarChar).Value = _rutaAlmacenamiento
                    .SqlParametros.Add("@tipoContenido", SqlDbType.VarChar).Value = _tipoContenido
                    .SqlParametros.Add("@identificadorUnico", SqlDbType.UniqueIdentifier).Value = _identificadorUnico
                    .SqlParametros.Add("@tamanio", SqlDbType.Int).Value = _tamanio
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@idTipoDocumento", SqlDbType.Int).Value = idTipoDocumento
                    If idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                    If Not String.IsNullOrEmpty(_pedidoSAP) Then .SqlParametros.Add("@pedidoSAP", SqlDbType.VarChar, 50).Value = _pedidoSAP
                    If Not String.IsNullOrEmpty(_documentoSAP) Then .SqlParametros.Add("@documentoSAP", SqlDbType.VarChar, 50).Value = _documentoSAP

                    .IniciarTransaccion()
                    .EjecutarNonQuery("RegistrarDocumentoTemporalServicioMensajeria", CommandType.StoredProcedure)
                    Integer.TryParse(.SqlParametros("@resultado").Value, respuesta.Valor)

                    If respuesta.Valor = 0 Then

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

                        If Not Directory.Exists(ruta & _rutaAlmacenamiento) Then
                            Directory.CreateDirectory(ruta & _rutaAlmacenamiento)
                        End If

                        Dim rutaGuardar As String = ruta & _rutaAlmacenamiento.ToString() & "\" & _identificadorUnico.ToString()
                        Using fsArchivo As FileStream = File.Create(rutaGuardar)
                            Dim arrContenido As Byte() = New Byte(_strArchivo.Length - 1) {}
                            _strArchivo.Read(arrContenido, 0, arrContenido.Length)
                            fsArchivo.Write(arrContenido, 0, arrContenido.Length)
                        End Using

                        .ConfirmarTransaccion()
                        respuesta.Mensaje = "Se realizó el registro del documento exitosamente."
                    ElseIf respuesta.Valor = 1 Then
                        .AbortarTransaccion()
                        respuesta.Mensaje = "Ya existe Soportes de factura para el pedidoSAS: " & _pedidoSAP & " y documentoSAP: " & _documentoSAP & "."
                    Else
                        .AbortarTransaccion()
                        respuesta.Mensaje = "Se generó un error al intentar registrar el documento. [" & respuesta.Valor & "]"
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw ex
            End Try
        End Using
        Return respuesta
    End Function

    Public Function Actualizar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idDocumento", SqlDbType.Int).Value = _idRegistro
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
                .EjecutarNonQuery("ActualizarDocumentoTemporalServicioMensajeria", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If resultado.Valor = 0 Then
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
                        .ConfirmarTransaccion()
                        'Se guarda el archivo de acuerdo a la ruta de almacenamiento
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

    Public Function Eliminar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idRegistro", SqlDbType.BigInt).Value = _idRegistro
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .EjecutarNonQuery("EliminarDocumentosTemporales", CommandType.StoredProcedure)

                If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente. ")
                End If
            End With
        Catch ex As Exception
            dbManager.Dispose()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al realizar la eliminación del documento: " & ex.Message)
        End Try
        Return resultado
    End Function

    Public Function ConsultarDocumentosTemporales(ByVal idUsuario As Integer, ByVal idServicio As Integer) As DataTable
        Dim dtDocumentos As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    If idServicio > 0 Then .Add("@idServicio", SqlDbType.Int).Value = idServicio
                End With
                dtDocumentos = .EjecutarDataTable("ConsultarDocumentosTemporales", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            dbManager.Dispose()
        End Try
        Return dtDocumentos
    End Function

    Public Function EliminarFacturasTemporal(ByVal idRegistro As String) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idRegistro", SqlDbType.Int).Value = idRegistro
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                .EjecutarNonQuery("EliminarFacturasTemporal", CommandType.StoredProcedure)

                If (Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor)) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With

        Catch ex As Exception
            If dbManager IsNot Nothing Then dbManager.Dispose()
            resultado.EstablecerMensajeYValor(500, "Se generó un error al eliminar los mines: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

End Class
