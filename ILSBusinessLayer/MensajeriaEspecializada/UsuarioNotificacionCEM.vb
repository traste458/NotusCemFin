Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.IO
Imports ILSBusinessLayer.Comunes

Namespace MensajeriaEspecializada

    Public Class UsuarioNotificacionCEM

#Region "Atributos (campos)"

        Private _idUsuarioNotificacion As Long
        Private _nombres As String
        Private _apellidos As String
        Private _email As String
        Private _idUsuarioCreacion As Long
        Private _usuarioCreacion As String
        Private _fechaCreacion As DateTime
        Private _idTipoNotificacion As Integer
        Private _tipoNotificacion As String
        Private _tipoDestino As Integer
        Private _estado As Boolean
        Private _nombreCompuesto As String

        Private _registrado As Boolean

#End Region

#Region "Propiedades"

        Public Property IdUsuarioNotificacion As Long
            Get
                Return _idUsuarioNotificacion
            End Get
            Set(value As Long)
                _idUsuarioNotificacion = value
            End Set
        End Property

        Public Property Nombres As String
            Get
                Return _nombres
            End Get
            Set(value As String)
                _nombres = value
            End Set
        End Property

        Public Property Apellidos As String
            Get
                Return _apellidos
            End Get
            Set(value As String)
                _apellidos = value
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

        Public Property IdUsuarioCreacion As Long
            Get
                Return _idUsuarioCreacion
            End Get
            Set(value As Long)
                _idUsuarioCreacion = value
            End Set
        End Property

        Public Property UsuarioCreacion As String
            Get
                Return _usuarioCreacion
            End Get
            Set(value As String)
                _usuarioCreacion = value
            End Set
        End Property

        Public Property FechaCreacion As DateTime
            Get
                Return _fechaCreacion
            End Get
            Set(value As DateTime)
                _fechaCreacion = value
            End Set
        End Property

        Public Property IdTipoNotificacion As Integer
            Get
                Return _idTipoNotificacion
            End Get
            Set(value As Integer)
                _idTipoNotificacion = value
            End Set
        End Property

        Public Property TipoNotificacion As String
            Get
                Return _tipoNotificacion
            End Get
            Set(value As String)
                _tipoNotificacion = value
            End Set
        End Property

        Public Property TipoDestino As Integer
            Get
                Return _tipoDestino
            End Get
            Set(value As Integer)
                _tipoDestino = value
            End Set
        End Property

        Public Property Estado As Boolean
            Get
                Return _estado
            End Get
            Set(value As Boolean)
                _estado = value
            End Set
        End Property

        Public Property NombreCompuesto As String
            Get
                Return _nombreCompuesto
            End Get
            Set(value As String)
                _nombreCompuesto = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idUsuarioNotificacion As Long)
            MyBase.New()
            _idUsuarioNotificacion = idUsuarioNotificacion
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idUsuarioNotificacion", SqlDbType.Int).Value = CStr(_idUsuarioNotificacion)
                    .ejecutarReader("ObtenerUsuarioNotificacion", CommandType.StoredProcedure)

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

#Region "Métodos Públicos"

        Public Function Registrar(ByVal listaNotificacion As ArrayList, Optional ByVal listaBodega As ArrayList = Nothing) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idUsuarioCreacion > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@nombres", SqlDbType.VarChar, 250).Value = _nombres
                            .Add("@apellidos", SqlDbType.VarChar, 250).Value = _apellidos
                            .Add("@email", SqlDbType.VarChar, 450).Value = _email
                            .Add("@idUsuarioCreacion", SqlDbType.BigInt).Value = _idUsuarioCreacion
                            .Add("@tipoDestino", SqlDbType.Int).Value = _tipoDestino
                            .Add("@listaNotificacion", SqlDbType.VarChar).Value = Join(listaNotificacion.ToArray(), ",")
                            If listaBodega IsNot Nothing AndAlso listaBodega.Count > 0 Then _
                            .Add("@listaBodega", SqlDbType.VarChar).Value = Join(listaBodega.ToArray(), ",")
                            .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                            .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarUsuarioNotificacion", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            .confirmarTransaccion()
                            resultado.Mensaje = .SqlParametros("@mensaje").Value
                            resultado.Valor = .SqlParametros("@resultado").Value
                        Else
                            .abortarTransaccion()
                            resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            Else
                resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para realizar el registro. ")
            End If
            Return resultado
        End Function

        Public Function Actualizar(Optional ByVal listaNotificacion As ArrayList = Nothing, Optional ByVal listaBodega As ArrayList = Nothing) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idUsuarioCreacion > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idUsuarioNotificacion", SqlDbType.Int).Value = _idUsuarioNotificacion
                            .Add("@estado", SqlDbType.Bit).Value = _estado
                            If Not String.IsNullOrEmpty(_nombres) Then .Add("@nombres", SqlDbType.VarChar, 250).Value = _nombres
                            If Not String.IsNullOrEmpty(_apellidos) Then .Add("@apellidos", SqlDbType.VarChar, 250).Value = _apellidos
                            If Not String.IsNullOrEmpty(_email) Then .Add("@email", SqlDbType.VarChar, 450).Value = _email
                            If _tipoDestino > 0 Then .Add("@tipoDestino", SqlDbType.Int).Value = _tipoDestino
                            If _idUsuarioCreacion > 0 Then .Add("@idUsuarioCreacion", SqlDbType.BigInt).Value = _idUsuarioCreacion
                            If listaNotificacion IsNot Nothing AndAlso listaNotificacion.Count > 0 Then _
                            .Add("@listaNotificacion", SqlDbType.VarChar).Value = Join(listaNotificacion.ToArray(), ",")
                            If listaBodega IsNot Nothing AndAlso listaBodega.Count > 0 Then _
                            .Add("@listaBodega", SqlDbType.VarChar).Value = Join(listaBodega.ToArray(), ",")
                            .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                            .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarUsuarioNotificacionCEM", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            .confirmarTransaccion()
                            resultado.Mensaje = .SqlParametros("@mensaje").Value
                            resultado.Valor = .SqlParametros("@resultado").Value
                        Else
                            .abortarTransaccion()
                            resultado.EstablecerMensajeYValor(500, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente.")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            Else
                resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para realizar el registro. ")
            End If
            Return resultado
        End Function

        Public Function Eliminar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            If _idUsuarioNotificacion > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idUsuarioNotificacion", SqlDbType.Int).Value = _idUsuarioNotificacion
                            .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                            .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarDataTable("EliminarUsuarioNotificacionCEM", CommandType.StoredProcedure)
                        If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                            .confirmarTransaccion()
                            resultado.Mensaje = .SqlParametros("@mensaje").Value
                            resultado.Valor = .SqlParametros("@resultado").Value
                        Else
                            .abortarTransaccion()
                            resultado.Mensaje = .SqlParametros("@mensaje").Value
                            resultado.Valor = .SqlParametros("@resultado").Value
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            End If
            Return resultado
        End Function

        Public Function ValidarDominio(ByVal email As String) As Boolean
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim dominios As New ConfigValues("DOMINIOS_VALIDOS_NOTIFICACION_CEM")
            Dim arrDominiosValidos As String() = dominios.ConfigKeyValue.Split(",")
            Dim dominioActual As String() = email.Split("@")
            Dim flag As Boolean
            If dominioActual.Length > 1 Then
                flag = arrDominiosValidos.Contains(dominioActual(1))
            Else
                flag = False
            End If
            Return flag
        End Function

        Public Function ObtenerTipoNotificacion() As DataTable
            Dim dtDatos As New DataTable
            If _idUsuarioNotificacion Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idUsuarioNotificacion", SqlDbType.BigInt).Value = _idUsuarioNotificacion
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerTipoNotificacionCEM", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            End If
            Return dtDatos
        End Function

        Public Function ObtenerBodegasNotificacion() As DataTable
            Dim dtDatos As New DataTable
            If _idUsuarioNotificacion Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idUsuarioNotificacion", SqlDbType.BigInt).Value = _idUsuarioNotificacion
                        End With
                        dtDatos = .ejecutarDataTable("ObtenerBodegasNotificacion", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message)
                End Try
            End If
            Return dtDatos
        End Function

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idUsuarioNotificacion"), _idUsuarioNotificacion)
                    If Not IsDBNull(reader("nombres")) Then _nombres = (reader("nombres").ToString)
                    If Not IsDBNull(reader("apellidos")) Then _apellidos = (reader("apellidos"))
                    If Not IsDBNull(reader("email")) Then _email = (reader("email").ToString)
                    If Not IsDBNull(reader("nombreCompuesto")) Then _nombreCompuesto = (reader("nombreCompuesto").ToString)
                    Integer.TryParse(reader("idUsuarioCreacion"), _idUsuarioCreacion)
                    Integer.TryParse(reader("tipoDestino"), _tipoDestino)
                    If Not IsDBNull(reader("usuarioCreacion")) Then _usuarioCreacion = (reader("usuarioCreacion").ToString)
                    If Not IsDBNull(reader("fechaCreacion")) Then _fechaCreacion = CDate(reader("fechaCreacion").ToString)
                    If Not IsDBNull(reader("tipoNotificacion")) Then _tipoNotificacion = (reader("tipoNotificacion").ToString)
                    Integer.TryParse(reader("idAsuntoNotificacion"), _idTipoNotificacion)
                    If Not IsDBNull(reader("estado")) Then _estado = CBool(reader("estado").ToString)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace