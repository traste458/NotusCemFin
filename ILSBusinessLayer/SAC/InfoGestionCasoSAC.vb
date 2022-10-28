Imports LMDataAccessLayer

Namespace SAC

    Public Class InfoGestionCasoSAC

#Region "Atributos"

        Private _idGestion As Int64
        Private _idCaso As Integer
        Private _idTipoGestion As Short
        Private _tipoGestion As String
        Private _descripcion As String
        Private _idCliente As Short
        Private _cliente As String
        Private _idGestionador As Short
        Private _gestionador As String
        Private _fechaGestion As Date
        Private _fechaRegistro As Date
        Private _idUsuarioRegistra As Integer
        Private _usuarioRegistra As String
        Private _registrado As Boolean
        Private _respuesta As RespuestaGestionCasoSACColeccion

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdGestion() As Integer
            Get
                Return _idGestion
            End Get
        End Property
        Public Property IdCaso() As Integer
            Get
                Return _idCaso
            End Get
            Set(ByVal value As Integer)
                _idCaso = value
            End Set
        End Property

        Public Property IdTipoGestion() As Short
            Get
                Return _idTipoGestion
            End Get
            Set(ByVal value As Short)
                _idTipoGestion = value
            End Set
        End Property

        Public ReadOnly Property TipoGestion() As String
            Get
                Return _tipoGestion
            End Get
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property IdCliente() As Short
            Get
                Return _idCliente
            End Get
            Set(ByVal value As Short)
                _idCliente = value
            End Set
        End Property

        Public ReadOnly Property Cliente() As String
            Get
                Return _cliente
            End Get
        End Property

        Public Property IdGestionador() As Integer
            Get
                Return _idGestionador
            End Get
            Set(ByVal value As Integer)
                _idGestionador = value
            End Set
        End Property

        Public ReadOnly Property Gestionador() As String
            Get
                Return _gestionador
            End Get
        End Property

        Public Property FechaDeGestion() As Date
            Get
                Return _fechaGestion
            End Get
            Set(ByVal value As Date)
                _fechaGestion = value
            End Set
        End Property

        Public ReadOnly Property FechaRegistro() As Date
            Get
                Return _fechaRegistro
            End Get
        End Property

        Public Property IdUsuarioRegistra() As Integer
            Get
                Return _idUsuarioRegistra
            End Get
            Set(ByVal value As Integer)
                _idUsuarioRegistra = value
            End Set
        End Property

        Public ReadOnly Property UsuarioRegistra() As String
            Get
                Return _usuarioRegistra
            End Get
        End Property

        Public ReadOnly Property Registrado() As Boolean
            Get
                Return _registrado
            End Get
        End Property

        Public ReadOnly Property ListadoRespuesta() As RespuestaGestionCasoSACColeccion
            Get
                If _idGestion > 0 Then _respuesta = New RespuestaGestionCasoSACColeccion(_idGestion)
                If _respuesta Is Nothing Then _respuesta = New RespuestaGestionCasoSACColeccion
                Return _respuesta
            End Get
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _tipoGestion = ""
            _descripcion = ""
            _cliente = ""
            _gestionador = ""
            _usuarioRegistra = ""
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            CargarDatos(identificador)
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos(ByVal identificador As Integer)
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idGestion", SqlDbType.Int).Value = identificador
                    .ejecutarReader("ConsultarInfoGestionCasoSAC", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idGestion").ToString, _idGestion)
                            Integer.TryParse(.Reader("idCaso").ToString, _idCaso)
                            Short.TryParse(.Reader("idTipoGestion").ToString, _idTipoGestion)
                            _tipoGestion = .Reader("tipoGestion").ToString
                            _descripcion = .Reader("descripcion").ToString
                            Short.TryParse(.Reader("idCliente").ToString, _idCliente)
                            _cliente = .Reader("cliente").ToString
                            Short.TryParse(.Reader("idGestionador").ToString, _idGestionador)
                            _gestionador = .Reader("gestionador").ToString
                            Date.TryParse(.Reader("fechaGestion").ToString, _fechaGestion)
                            Date.TryParse(.Reader("fechaRegistro").ToString, _fechaRegistro)
                            Integer.TryParse(.Reader("idUsuarioRegistra").ToString, _idUsuarioRegistra)
                            _usuarioRegistra = .Reader("usuarioRegistra").ToString
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

        Protected Friend Sub EstablecerIdentificador(ByVal identificador As Int64)
            _idGestion = identificador
        End Sub

        Protected Friend Sub EstablecerTipoGestion(ByVal valor As String)
            _tipoGestion = valor
        End Sub

        Protected Friend Sub EstablecerCliente(ByVal valor As String)
            _cliente = valor
        End Sub

        Protected Friend Sub EstablecerGestionador(ByVal valor As String)
            _gestionador = valor
        End Sub

        Protected Friend Sub EstablecerUsuarioRegistra(ByVal valor As String)
            _usuarioRegistra = valor
        End Sub

        Protected Friend Sub EstablecerFechaRegistro(ByVal valor As Date)
            _fechaRegistro = valor
        End Sub

        Protected Friend Sub MarcarComoRegistrado()
            _registrado = True
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Me._idCaso > 0 AndAlso Me._idTipoGestion > 0 AndAlso Me._idCliente > 0 AndAlso Me._idGestionador > 0 _
                AndAlso Me._fechaGestion > Date.MinValue AndAlso Me._idUsuarioRegistra Then

                Dim dbManager As New LMDataAccess
                Try
                    resultado.Valor = 1
                    With dbManager
                        With .SqlParametros
                            .Add("@idCaso", SqlDbType.Int).Value = Me._idCaso
                            .Add("@idTipoGestion", SqlDbType.SmallInt).Value = Me._idTipoGestion
                            If Me._descripcion.Trim.Length > 0 Then _
                                .Add("@descripcion", SqlDbType.VarChar, 2000).Value = Me._descripcion
                            .Add("@idCliente", SqlDbType.SmallInt).Value = Me._idCliente
                            .Add("@idGestionador", SqlDbType.Int).Value = Me._idGestionador
                            .Add("@fechaGestion", SqlDbType.SmallDateTime).Value = Me._fechaGestion
                            .Add("@idUsuarioRegistra", SqlDbType.Int).Value = Me._idUsuarioRegistra
                            .Add("@idGestion", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@return", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .IniciarTransaccion()
                        .EjecutarNonQuery("RegistrarInfoGestionCasoSAC", CommandType.StoredProcedure)
                        resultado.Valor = CShort(.SqlParametros("@return").Value)
                        If resultado.Valor = 0 Then
                            Me._idGestion = CInt(.SqlParametros("@idGestion").Value)
                            If Me._respuesta IsNot Nothing AndAlso Me._respuesta.Count > 0 Then
                                For index As Integer = 0 To Me._respuesta.Count - 1
                                    CType(_respuesta(index), RespuestaGestionCasoSAC).IdGestion = Me._idGestion
                                Next
                                Dim dtRespuesta As DataTable = Me._respuesta.GenerarDataTable()
                                .InicilizarBulkCopy()
                                With .BulkCopy
                                    .DestinationTableName = "RespuestaGestionCasoSAC"
                                    .ColumnMappings.Add("IdGestion", "idGestion")
                                    .ColumnMappings.Add("IdOrigenRespuesta", "idOrigenRespuesta")
                                    .ColumnMappings.Add("Descripcion", "descripcion")
                                    .ColumnMappings.Add("NombreArchivo", "archivo")
                                    .ColumnMappings.Add("NombreArchivoConRuta", "archivoConRuta")
                                    .ColumnMappings.Add("NombreArchivoOriginal", "archivoOriginal")
                                    .ColumnMappings.Add("FechaRecepcion", "fechaRecepcion")
                                    .WriteToServer(dtRespuesta)
                                End With
                            End If
                            If Me._respuesta IsNot Nothing AndAlso Me._respuesta.Count > 0 Then Me._respuesta.Clear()
                            Me.CargarDatos(Me._idGestion)
                            .ConfirmarTransaccion()
                        Else
                            resultado.Mensaje = "Imposible registrar la información de Gestión del Caso. Ocurrió un error inesperado al tratar de realizar el registro. Por favor intente nuevamente"
                            If .estadoTransaccional Then .AbortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.AbortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.Valor = 2
                resultado.Mensaje = "No se han proporcionado todos los datos requeridos para procesar el registro. Por favor verifique"
            End If

            Return resultado
        End Function

        Public Function Editar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If Me._idGestion > 0 AndAlso Me._idCaso > 0 AndAlso Me._idTipoGestion > 0 AndAlso Me._idCliente > 0 AndAlso Me._idGestionador > 0 _
                AndAlso Me._fechaGestion > Date.MinValue AndAlso Me._idUsuarioRegistra Then

                Dim dbManager As New LMDataAccess
                Try
                    resultado.Valor = 1
                    With dbManager
                        With .SqlParametros
                            .Add("@idGestion", SqlDbType.Int).Value = Me._idGestion
                            .Add("@idCaso", SqlDbType.Int).Value = Me._idCaso
                            .Add("@idTipoGestion", SqlDbType.SmallInt).Value = Me._idTipoGestion
                            If Me._descripcion.Trim.Length > 0 Then _
                                .Add("@descripcion", SqlDbType.VarChar, 2000).Value = Me._descripcion
                            .Add("@idCliente", SqlDbType.SmallInt).Value = Me._idCliente
                            .Add("@idGestionador", SqlDbType.Int).Value = Me._idGestionador
                            .Add("@fechaGestion", SqlDbType.SmallDateTime).Value = Me._fechaGestion
                            .Add("@idUsuarioRegistra", SqlDbType.Int).Value = Me._idUsuarioRegistra
                            .Add("@return", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .IniciarTransaccion()
                        .EjecutarNonQuery("EditarInfoGestionCasoSAC", CommandType.StoredProcedure)
                        resultado.Valor = CShort(.SqlParametros("@return").Value)
                        If resultado.Valor = 0 Then
                            Me._idGestion = CInt(.SqlParametros("@idGestion").Value)
                            If Me._respuesta IsNot Nothing AndAlso Me._respuesta.Count > 0 Then
                                For index As Integer = 0 To Me._respuesta.Count - 1
                                    CType(_respuesta(index), RespuestaGestionCasoSAC).IdGestion = Me._idGestion
                                Next
                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idGestion", SqlDbType.Int).Value = Me._idGestion
                                .EjecutarNonQuery("EliminarRespuestaInfoGestionCasoSAC", CommandType.StoredProcedure)
                                Dim dtRespuesta As DataTable = Me._respuesta.GenerarDataTable()
                                .InicilizarBulkCopy()
                                With .BulkCopy
                                    .DestinationTableName = "RespuestaGestionCasoSAC"
                                    .ColumnMappings.Add("IdGestion", "idGestion")
                                    .ColumnMappings.Add("IdOrigenRespuesta", "idOrigenRespuesta")
                                    .ColumnMappings.Add("Descripcion", "descripcion")
                                    .ColumnMappings.Add("NombreArchivo", "archivo")
                                    .ColumnMappings.Add("NombreArchivoConRuta", "archivoConRuta")
                                    .ColumnMappings.Add("NombreArchivoOriginal", "archivoOriginal")
                                    .ColumnMappings.Add("FechaRecepcion", "fechaRecepcion")
                                    .WriteToServer(dtRespuesta)
                                End With
                            End If
                            If Me._respuesta IsNot Nothing AndAlso Me._respuesta.Count > 0 Then Me._respuesta.Clear()
                            Me.CargarDatos(Me._idGestion)
                            .ConfirmarTransaccion()
                        Else
                            resultado.Mensaje = "Imposible editar la información de Gestión del Caso. Ocurrió un error inesperado al tratar de realizar el registro. Por favor intente nuevamente"
                            If .estadoTransaccional Then .AbortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.AbortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado.Valor = 2
                resultado.Mensaje = "No se han proporcionado todos los datos requeridos para procesar la edición. Por favor verifique"
            End If

            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso


            Return resultado
        End Function

#End Region

    End Class

End Namespace

