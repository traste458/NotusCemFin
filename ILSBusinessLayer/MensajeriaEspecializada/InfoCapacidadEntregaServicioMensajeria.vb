Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class InfoCapacidadEntregaServicioMensajeria

#Region "Atributos (Campos)"

    Private _idRegistro As Integer
    Private _idBodega As Integer
    Private _bodega As String
    Private _fecha As Date
    Private _nit As String
    Private _idEmpresa As Integer
    Private _cliente As String
    Private _idJornada As Integer
    Private _jornada As String
    Private _cantidadServicios As Integer
    Private _cantidadDisponible As Integer
    Private _cantidadServiciosUtilizados As Integer
    Private _idUsuarioRegistra As Integer
    Private _usuarioRegistra As String
    Private _fechaRegistro As Date
    Private _idAgrupacion As Short
    Private _nombreAgrupacion As String
    Private _registrado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idRegistro As Integer)
        MyBase.New()
        _idRegistro = idRegistro
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdRegistro() As Integer
        Get
            Return _idRegistro
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idRegistro = value
        End Set
    End Property

    Public Property IdBodega() As Integer
        Get
            Return _idBodega
        End Get
        Set(ByVal value As Integer)
            _idBodega = value
        End Set
    End Property

    Public Property Bodega() As String
        Get
            Return _bodega
        End Get
        Set(ByVal value As String)
            _bodega = value
        End Set
    End Property

    Public Property Fecha() As Date
        Get
            Return _fecha
        End Get
        Set(ByVal value As Date)
            _fecha = value
        End Set
    End Property

    Public Property nit() As String
        Get
            Return _nit
        End Get
        Set(ByVal value As String)
            _nit = value
        End Set
    End Property

    Public Property cliente() As String
        Get
            Return _cliente
        End Get
        Set(ByVal value As String)
            _cliente = value
        End Set
    End Property

    Public Property IdEmpresa() As String
        Get
            Return _idEmpresa
        End Get
        Set(ByVal value As String)
            _idEmpresa = value
        End Set
    End Property

    Public Property IdJornada() As Integer
        Get
            Return _idJornada
        End Get
        Set(ByVal value As Integer)
            _idJornada = value
        End Set
    End Property

    Public Property Jornada() As String
        Get
            Return _jornada
        End Get
        Protected Friend Set(ByVal value As String)
            _jornada = value
        End Set
    End Property

    Public Property CantidadServicios() As Integer
        Get
            Return _cantidadServicios
        End Get
        Set(ByVal value As Integer)
            _cantidadServicios = value
        End Set
    End Property

    Public Property CantidadServiciosUtilizados() As Integer
        Get
            Return _cantidadServiciosUtilizados
        End Get
        Protected Friend Set(ByVal value As Integer)
            _cantidadServiciosUtilizados = value
        End Set
    End Property

    Public Property CantidadDisponible As Integer
        Get
            Return _cantidadDisponible
        End Get
        Set(value As Integer)
            _cantidadDisponible = value
        End Set
    End Property

    Public Property IdUsuarioRegistra() As Integer
        Get
            Return _idUsuarioRegistra
        End Get
        Set(ByVal value As Integer)
            _idUsuarioRegistra = value
        End Set
    End Property

    Public Property UsuarioRegistra() As String
        Get
            Return _usuarioRegistra
        End Get
        Protected Friend Set(ByVal value As String)
            _usuarioRegistra = value
        End Set
    End Property

    Public Property FechaRegistro() As Date
        Get
            Return _fechaRegistro
        End Get
        Protected Friend Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property IdAgrupacion As Short
        Get
            Return _idAgrupacion
        End Get
        Set(value As Short)
            _idAgrupacion = value
        End Set
    End Property

    Public Property NombreAgrupacion As String
        Get
            Return _nombreAgrupacion
        End Get
        Set(value As String)
            _nombreAgrupacion = value
        End Set
    End Property

    Public Property Registrado() As Boolean
        Get
            Return _registrado
        End Get
        Protected Friend Set(ByVal value As Boolean)
            _registrado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                .ejecutarReader("ObtenerInfoCapacidadEntregaCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then CargarResultadoConsulta(.Reader)
                    If Not .Reader.IsClosed Then .Reader.Close()
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
                Integer.TryParse(reader("idRegistro").ToString, _idRegistro)
                _cliente = reader("cliente").ToString
                _nit = reader("nit").ToString
                Integer.TryParse(reader("idBodega").ToString, _idBodega)
                _bodega = reader("bodega").ToString
                _fecha = CDate(reader("fecha").ToString)
                Integer.TryParse(reader("idJornada").ToString, _idJornada)
                _jornada = reader("jornada").ToString
                Integer.TryParse(reader("cantidadServicios"), _cantidadServicios)
                Integer.TryParse(reader("cantidadDisponible"), _cantidadDisponible)
                Integer.TryParse(reader("cantidadServiciosUtilizados"), _cantidadServiciosUtilizados)
                Integer.TryParse(reader("idUsuarioRegistra"), _idUsuarioRegistra)
                _usuarioRegistra = reader("usuarioRegistra").ToString
                _fechaRegistro = CDate(reader("fechaRegistro").ToString)
                Short.TryParse(reader("idAgrupacion"), _idAgrupacion)
                _nombreAgrupacion = reader("nombreAgrupacion")
                _registrado = True
            End If
        End If
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _fecha > Date.MinValue AndAlso _idJornada > 0 AndAlso _cantidadServicios > 0 AndAlso _idUsuarioRegistra > 0 Then
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@fecha", SqlDbType.Date).Value = _fecha
                    .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                    .SqlParametros.Add("@idEmpresa", SqlDbType.Int).Value = _idEmpresa
                    .SqlParametros.Add("@cantidadServicios", SqlDbType.Int).Value = _cantidadServicios
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioRegistra
                    If _idAgrupacion > 0 Then .SqlParametros.Add("@idAgrupacion", SqlDbType.Int).Value = _idAgrupacion
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("RegistrarInfoCapacidadEntregaCEM", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@returnValue").Value) _
                        AndAlso Integer.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado.Valor) Then

                        Select Case resultado.Valor
                            Case 0
                                resultado.Mensaje = "La Información fue registrada satisfactoriamente."
                            Case 1
                                resultado.Mensaje = "Imposible determinar la bodega CEM a la cual está asociado el usuario autenticado"
                            Case 2
                                resultado.Mensaje = "Existe un registro previo asociado a la Fecha y Jornada proporcionadas"
                            Case 3
                                resultado.Mensaje = "La cantidad de servicios debe ser mayor que cero (0)"
                            Case Else
                                resultado.Mensaje = "Ocurrió un error inesperado al registrar datos. Por favor intente nuevamente"
                        End Select
                    Else
                        resultado.EstablecerMensajeYValor(9, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente")
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado los parámetros mínimos para realizar el registro.")
        End If

        Return resultado
    End Function

    Public Function Actualizar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idRegistro > 0 AndAlso _fecha > Date.MinValue AndAlso _idJornada > 0 AndAlso _cantidadServicios > 0 _
            AndAlso idUsuario > 0 Then
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                    .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = _idJornada
                    .SqlParametros.Add("@cantidadServicios", SqlDbType.Int).Value = _cantidadServicios
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ModificarInfoCapacidadEntregaCEM", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@returnValue").Value) _
                        AndAlso Integer.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado.Valor) Then

                        If resultado.Valor = 0 Then
                            resultado.Mensaje = "La Información fue actualizada satisfactoriamente."
                            .confirmarTransaccion()
                        Else
                            Select Case resultado.Valor
                                Case 1
                                    resultado.Mensaje = "El registro que se está tratando de actualizar no existe"
                                Case 2
                                    resultado.Mensaje = "La cantidad de servicios debe ser mayor que cero (0)"
                                Case 3
                                    resultado.Mensaje = "La nueva cantidad de servicios de entrega programados, no puede ser menor que el número de servicios utilizados"
                                Case 4
                                    resultado.Mensaje = "Existe un registro previo para la combinación Fecha/Jornada proporcionada"
                                Case 5
                                    resultado.Mensaje = "Ocurrió un error inesperado al registrar datos. Por favor intente nuevamente"
                            End Select
                            .abortarTransaccion()
                        End If
                    Else
                        resultado.EstablecerMensajeYValor(9, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado los parámetros mínimos para realizar la actualización.")
        End If
        Return resultado
    End Function

    Public Function Eliminar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _idRegistro > 0 AndAlso idUsuario > 0 Then
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .iniciarTransaccion()
                    .ejecutarNonQuery("EliminarInfoCapacidadEntregaCEM", CommandType.StoredProcedure)
                    If Not IsDBNull(.SqlParametros("@returnValue").Value) _
                        AndAlso Integer.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado.Valor) Then

                        If resultado.Valor = 0 Then
                            resultado.Mensaje = "La Información fue eliminada satisfactoriamente."
                            .confirmarTransaccion()
                        Else
                            Select Case resultado.Valor
                                Case 1
                                    resultado.Mensaje = "El registro que se está tratando de actualizar no existe"
                                Case 2
                                    resultado.Mensaje = "La cantidad de servicios utilizados es mayor que cero (0)"
                                Case 3
                                    resultado.Mensaje = "Ocurrió un error inesperado al eliminar datos. Por favor intente nuevamente"
                            End Select
                            .abortarTransaccion()
                        End If
                    Else
                        resultado.EstablecerMensajeYValor(9, "Imposible evaluar la respuesta del servidor. Por favor intente nuevamente")
                    End If
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        Else
            resultado.EstablecerMensajeYValor(10, "No se han proporcionado todos los datos requeridos para realizar la eliminación.")
        End If
        Return resultado
    End Function

#End Region

End Class
