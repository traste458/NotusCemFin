Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Comunes

    Public Class UsuarioNotificacion

#Region "Atributos"

        Private _idUsuarioNotificacion As Integer
        Private _nombres As String
        Private _apellidos As String
        Private _email As String
        Private _idUsuarioCreacion As Integer
        Private _fechaCreacion As Date
        Private _usuarioCreacion As String
        Private _idAsuntoNotificacion As Integer
        Private _infoAsuntoNotificacion As DataTable
        Private _idPerfil As Integer
        Private _copia As Short
        Private _tipoDestino As Short
        Private _tipo As String

#End Region

#Region "Propiedades"

        Public Property IdUsuarioNotificacion() As Integer
            Get
                Return _idUsuarioNotificacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioNotificacion = value
            End Set
        End Property

        Public Property IdAsuntoNotificacion() As Integer
            Get
                Return _idAsuntoNotificacion
            End Get
            Set(ByVal value As Integer)
                _idAsuntoNotificacion = value
            End Set
        End Property

        Public Property Nombres() As String
            Get
                Return _nombres
            End Get
            Set(ByVal value As String)
                _nombres = value
            End Set
        End Property

        Public Property Apellidos() As String
            Get
                Return _apellidos
            End Get
            Set(ByVal value As String)
                _apellidos = value
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

        Public Property UsuarioCreacion() As String
            Get
                Return _usuarioCreacion
            End Get
            Set(ByVal value As String)
                _usuarioCreacion = value
            End Set
        End Property

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As Date)
                _fechaCreacion = value
            End Set
        End Property

        Public Property IdUsuarioCreacion() As Integer
            Get
                Return _idUsuarioCreacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCreacion = value
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

        Public Property Copia() As Short
            Get
                Return _copia
            End Get
            Set(ByVal value As Short)
                _copia = value
            End Set
        End Property

        Public Property TipoDestino() As Short
            Get
                Return _tipoDestino
            End Get
            Set(ByVal value As Short)
                _tipoDestino = value
            End Set
        End Property

        Public ReadOnly Property InfoAsuntoNotificacion() As DataTable
            Get
                If _infoAsuntoNotificacion Is Nothing Then CargarListadoAsuntoNotificacion()
                Return _infoAsuntoNotificacion
            End Get
        End Property

        Public Property Tipo() As String
            Get
                Return _tipo
            End Get
            Set(ByVal value As String)
                _tipo = value
            End Set
        End Property

#End Region

#Region "Contructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idUsuarioNotificacion = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idUsuarioNotificacion <> 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idUsuarioNotificacion", SqlDbType.Int).Value = _idUsuarioNotificacion
                        .ejecutarReader("ObtenerInfoUsuarioNotificacion", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                CargarResultadoConsulta(.Reader)
                            End If
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

        Private Sub CargarListadoAsuntoNotificacion()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idUsuarioNotificacion", SqlDbType.Int).Value = _idUsuarioNotificacion
                    _infoAsuntoNotificacion = .ejecutarDataTable("ObtenerDetalleUsuarioNotificacion", CommandType.StoredProcedure)
                End With
                If _infoAsuntoNotificacion.PrimaryKey.Count = 0 Then
                    Dim pkColumn(0) As DataColumn
                    pkColumn(0) = _infoAsuntoNotificacion.Columns("idAsuntoNotifiacionDetalle")
                    _infoAsuntoNotificacion.PrimaryKey = pkColumn
                End If
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Crear() As Short
            Dim resultado As Short = 0
            If _nombres.Trim.Length > 0 AndAlso _apellidos.Trim.Length > 0 AndAlso _email.Trim.Length > 0 AndAlso _idUsuarioCreacion > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@nombres", SqlDbType.VarChar, 50).Value = _nombres
                            .Add("@apellidos", SqlDbType.VarChar, 50).Value = _apellidos
                            .Add("@email", SqlDbType.VarChar, 50).Value = _email
                            .Add("@idUsuarioCreacion", SqlDbType.Int).Value = _idUsuarioCreacion
                            .Add("@idUsuarioNotificacion", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearUsuarioNotificacion", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)

                        If resultado = 0 Then
                            Integer.TryParse(.SqlParametros("@idUsuarioNotificacion").Value, _idUsuarioNotificacion)
                            With .SqlParametros
                                .Clear()
                                .Add("@idUsuarioNotificacion", SqlDbType.Int).Value = _idUsuarioNotificacion
                                .Add("@idAsuntoNotificacion", SqlDbType.Int).Value = _idAsuntoNotificacion
                                .Add("@tipoDestino", SqlDbType.TinyInt).Value = _tipoDestino
                                .Add("@returnValue", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                            End With

                            ' Relaciona el asunto de notificacion con un destinatario
                            .ejecutarNonQuery("CrearAsuntoNotificacionDetalleUsuario", CommandType.StoredProcedure)
                            resultado = .SqlParametros("@returnValue").Value

                            If resultado <> 0 Then
                                If .estadoTransaccional Then .abortarTransaccion()
                            End If
                            .confirmarTransaccion()
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                        End If

                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

        Public Function Actualizar() As Short
            Dim resultado As Short = 0
            If _idUsuarioNotificacion <> 0 AndAlso _nombres.Trim.Length > 0 AndAlso _apellidos.Trim.Length > 0 _
            AndAlso _email.Trim.Length > 0 AndAlso _tipoDestino > 0 Then

                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idUsuarioNotificacion", SqlDbType.Int).Value = _idUsuarioNotificacion
                            .Add("@nombres", SqlDbType.VarChar, 50).Value = _nombres
                            .Add("@apellidos", SqlDbType.VarChar, 50).Value = _apellidos
                            .Add("@email", SqlDbType.VarChar, 50).Value = _email
                            .Add("@tipoDestino", SqlDbType.TinyInt).Value = _tipoDestino
                            .Add("@idAsuntoNotificacion", SqlDbType.Int).Value = _idAsuntoNotificacion
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarUsuarioNotificacion", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado)

                        If resultado = 0 Then .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If
            Return resultado
        End Function

        Public Function Eliminar() As Short
            Dim resultado As Short = 0
            If _idUsuarioNotificacion > 0 Then

                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idUsuarioNotificacion", SqlDbType.Int).Value = _idUsuarioNotificacion
                            .Add("@idPerfil", SqlDbType.Int).Value = _idPerfil
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("EliminarUsuarioNotificacion", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado)

                        If resultado = 0 Or resultado = 2 Then
                            .confirmarTransaccion()
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If

            Return resultado
        End Function

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroUsuarioNotificacion
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroUsuarioNotificacion) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdUsuarioNotificacion > 0 Then .Add("@idUsuarioNotificacion", SqlDbType.Int).Value = filtro.IdUsuarioNotificacion
                        If filtro.Nombres IsNot Nothing AndAlso filtro.Nombres.Trim.Length > 0 Then _
                            .Add("@nombres", SqlDbType.VarChar, 50).Value = filtro.Nombres
                        If filtro.Apellidos IsNot Nothing AndAlso filtro.Apellidos.Trim.Length > 0 Then _
                            .Add("@apellidos", SqlDbType.VarChar, 50).Value = filtro.Apellidos
                        If filtro.Email IsNot Nothing AndAlso filtro.Email.Trim.Length > 0 Then _
                            .Add("@email", SqlDbType.VarChar, 50).Value = filtro.Email
                        'If filtro.IdPerfil > 0 Then
                        .Add("@idPerfil", SqlDbType.Int).Value = filtro.IdPerfil
                        If filtro.IdAsuntoNotificacion <> 0 Then .Add("@idAsuntoNotificacion", SqlDbType.Int).Value = filtro.IdAsuntoNotificacion
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoUsuarioNotificacion", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerDestinatarioNotificacion(ByVal filtro As FiltroUsuarioNotificacion) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdAsuntoNotificacion <> 0 Then .Add("@idAsuntoNotificacion", SqlDbType.Int).Value = filtro.IdAsuntoNotificacion
                        If filtro.IdBodega <> 0 Then .Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodega
                        If filtro.Separador IsNot Nothing AndAlso filtro.Separador.Trim.Length > 0 Then _
                            .Add("@separador", SqlDbType.VarChar, 4).Value = filtro.Separador
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerDestinatarioNotificacion", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerDestinatarioNotificacionPOP(ByVal filtro As FiltroUsuarioNotificacion) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdAsuntoNotificacion <> 0 Then .Add("@idAsuntoNotificacion", SqlDbType.Int).Value = filtro.IdAsuntoNotificacion
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerDestinatarioNotificacionPOP", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    _nombres = reader("nombres").ToString
                    _apellidos = reader("apellidos").ToString
                    _email = reader("email").ToString
                    _usuarioCreacion = reader("usuarioCreacion").ToString()
                    Date.TryParse(reader("fechaCreacion").ToString, _fechaCreacion)
                    Integer.TryParse(reader("idAsuntoNotificacion").ToString, _idAsuntoNotificacion)
                    _tipo = reader("tipo").ToString
                    Short.TryParse(reader("tipoDestino").ToString, _tipoDestino)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace