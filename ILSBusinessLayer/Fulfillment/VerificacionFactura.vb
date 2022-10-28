Imports LMDataAccessLayer

Public Class VerificacionFactura

#Region "Atributos"

    Private _factura As String
    Private _idVerificacion As Integer
    Private _idFacturaGuia As Integer
    Private _producto As String
    Private _verificadoFulfillment As Boolean
    Private _mensaje As String
    Private _resultado As Integer
    Private _verificado As Boolean
    Private _concuerdaVersion As Boolean
    Private _idTipoSoftware As Integer
    Private _simLock As Boolean
    Private _soportesSimLock As SoporteSimLockVerificacionFacturaColeccion
    Private _serial As String
    Private _idUsuarioRegistra As Integer
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
    End Sub

    Public Sub New(ByVal identificador As String)
        _factura = identificador
        CargarInformacion()
    End Sub

#End Region

#Region "Propiedades"

    Public Property Factura As String
        Get
            Return _factura
        End Get
        Set(value As String)
            _factura = value
        End Set
    End Property

    Public Property IdFacturaGuia As Integer
        Get
            Return _idFacturaGuia
        End Get
        Set(value As Integer)
            _idFacturaGuia = value
        End Set
    End Property

    Public Property Producto As String
        Get
            Return _producto
        End Get
        Set(value As String)
            _producto = value
        End Set
    End Property

    Public Property VerificadoFulfillment As Boolean
        Get
            Return _verificadoFulfillment
        End Get
        Set(value As Boolean)
            _verificadoFulfillment = value
        End Set
    End Property

    Public Property IdUsuarioRegistra() As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(value As Integer)
            _idUsuarioRegistra = value
        End Set
    End Property

    Public Property Mensaje() As String
        Get
            Return _mensaje
        End Get
        Set(value As String)
            _mensaje = value
        End Set
    End Property

    Public Property Verificado() As Boolean
        Get
            Return _verificado
        End Get
        Set(value As Boolean)
            _verificado = value
        End Set
    End Property

    Public Property ConcuerdaVersion() As Boolean
        Get
            Return _concuerdaVersion
        End Get
        Set(value As Boolean)
            _concuerdaVersion = value
        End Set
    End Property

    Public Property IdTipoSoftware() As Integer
        Get
            Return _idTipoSoftware
        End Get
        Set(value As Integer)
            _idTipoSoftware = value
        End Set
    End Property

    Public Property SimLock() As Boolean
        Get
            Return _simLock
        End Get
        Set(value As Boolean)
            _simLock = value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(value As String)
            _serial = value
        End Set
    End Property

    Public ReadOnly Property Soportes As SoporteSimLockVerificacionFacturaColeccion
        Get
            If _soportesSimLock Is Nothing OrElse Not _soportesSimLock.Cargado Then
                If _idVerificacion > 0 Then
                    _soportesSimLock = New SoporteSimLockVerificacionFacturaColeccion(_idVerificacion)
                Else
                    _soportesSimLock = New SoporteSimLockVerificacionFacturaColeccion()
                End If
            End If
            Return _soportesSimLock
        End Get
    End Property

    Public Property Registrado As Boolean
        Get
            Return _registrado
        End Get
        Set(value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarInformacion()
        If _factura <> "" Then
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
                    .ejecutarReader("ObtenerInformacionVerificacionDeFacturas", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            CargarValorDePropiedades(.Reader)
                        End If
                        .Reader.Close()
                    End If
                End With
            End Using
        End If
    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarValorDePropiedades(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing AndAlso reader.HasRows Then
            Long.TryParse(reader("idFacturaGuia").ToString, _idFacturaGuia)
            Long.TryParse(reader("idTipoSoftware").ToString, _idTipoSoftware)
            _factura = reader("factura").ToString
            _producto = reader("producto").ToString
            Boolean.TryParse(reader("verificadoFulfillment").ToString, _verificadoFulfillment)
            _registrado = True
        End If
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso(-1, "Registro no creado")
        If _idFacturaGuia > 0 AndAlso Not EsNuloOVacio(_verificado) AndAlso Not EsNuloOVacio(_concuerdaVersion) > 0 AndAlso Not EsNuloOVacio(_idTipoSoftware) _
            AndAlso _soportesSimLock.Count > 0 AndAlso Not EsNuloOVacio(_simLock) AndAlso Not EsNuloOVacio(_serial) Then
            Dim dt As DataTable = _soportesSimLock.GenerarDataTable()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idFacturaGuia", SqlDbType.Int).Value = _idFacturaGuia
                        .SqlParametros.Add("@verificado", SqlDbType.Bit).Value = _verificado
                        .SqlParametros.Add("@concuerdaVersion", SqlDbType.Bit).Value = _concuerdaVersion
                        .SqlParametros.Add("@idTipoSoftware", SqlDbType.Int).Value = _idTipoSoftware
                        .SqlParametros.Add("@simLock", SqlDbType.Bit).Value = _simLock
                        .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = _serial
                        .SqlParametros.Add("@idUsuarioRegistra", SqlDbType.Int).Value = _idUsuarioRegistra
                        .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@idVerificacion", SqlDbType.Int).Direction = ParameterDirection.Output
                        .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        .iniciarTransaccion()
                        .TiempoEsperaComando = 200
                        .ejecutarNonQuery("RegistrarVerificacionDeFactura", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            If resultado.Valor = 0 Then
                                Integer.TryParse(.SqlParametros("@idVerificacion").Value.ToString, _idVerificacion)
                                For Each dr As DataRow In dt.Rows
                                    dr("idVerificacion") = _idVerificacion
                                    dr("idUsuarioRegistro") = _idUsuarioRegistra
                                Next
                                .SqlParametros.Clear()
                                .inicilizarBulkCopy()
                                With .BulkCopy
                                    .DestinationTableName = "SoporteVerificacionSimLock"
                                    .ColumnMappings.Add("IdVerificacion", "idVerificacion")
                                    .ColumnMappings.Add("NombreOriginal", "nombreOriginal")
                                    .ColumnMappings.Add("RutaCompleta", "rutaCompleta")
                                    .ColumnMappings.Add("DatosBinarios", "datosBinarios")
                                    .ColumnMappings.Add("ContentType", "contentType")
                                    .ColumnMappings.Add("IdTipoSoporte", "idTipoSoporte")
                                    .ColumnMappings.Add("IdUsuarioRegistro", "idUsuarioRegistro")
                                    .WriteToServer(dt)
                                End With
                                If .estadoTransaccional Then .confirmarTransaccion()
                                resultado.EstablecerMensajeYValor(0, "La verificación de la factura fue registrada satisfactoriamente.")
                            Else
                                If .estadoTransaccional Then .abortarTransaccion()
                            End If
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            resultado.Mensaje = "No se pudo evaluar el resultado de registro arrojado por la base de  datos. Por favor intente nuevamente."
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                    resultado.EstablecerMensajeYValor(500, "Error al registrar la verificación de la factura.: " & ex.Message)
                End Try
            End Using
        Else
            resultado.EstablecerMensajeYValor(300, "No se han proporcionado los valores de todos los parámetros obligatorios. Por favor verifique")
        End If
        Return resultado
    End Function

#End Region

End Class

