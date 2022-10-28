Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports ILSBusinessLayer.MensajeriaEspecializada
Imports ILSBusinessLayer.Comunes
Imports System.Web

Public Class NovedadServicioMensajeria

#Region "Atributos (Campos)"

    Private _idNovedad As Integer
    Private _idServicioMensajeria As Integer
    Private _idTipoNovedad As Integer
    Private _tipoNovedad As String
    Private _idUsuario As Integer
    Private _usuarioRegistra As String
    Private _fechaRegistro As Date
    Private _fechaModificacion As Date
    Private _idEstado As Integer
    Private _estado As String
    Private _observacion As String
    Private _numeroRadicado As Integer
    Private _comentarioEspecifico As String
    Private _nombreCliente As String
    Private _identificacion As String
    Private _nombreContacto As String
    Private _fechaAgenda As DateTime
    Private _direccion As String
    Private _telefono As String
    Private _idTipoServicio As Integer
    Private _tipoServicio As String
    Private _consultor As String

    Private _registrado As Boolean
    Private _origenCausal As Integer
    Private _causales As String

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idNovedad As Integer)
        MyBase.New()
        _idNovedad = idNovedad
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property OrigenCausal As Integer
        Get
            Return _origenCausal
        End Get
        Set(value As Integer)
            _origenCausal = value
        End Set
    End Property
    Public Property Causales As String
        Get
            Return _causales
        End Get
        Set(value As String)
            _causales = value
        End Set
    End Property

    Public Property IdNovedad() As Integer
        Get
            Return _idNovedad
        End Get
        Protected Friend Set(ByVal value As Integer)
            _idNovedad = value
        End Set
    End Property

    Public Property IdServicioMensajeria() As Integer
        Get
            Return _idServicioMensajeria
        End Get
        Set(ByVal value As Integer)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property IdTipoNovedad() As Integer
        Get
            Return _idTipoNovedad
        End Get
        Set(ByVal value As Integer)
            _idTipoNovedad = value
        End Set
    End Property

    Public Property TipoNovedad() As String
        Get
            Return _tipoNovedad
        End Get
        Set(ByVal value As String)
            _tipoNovedad = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
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
        Set(ByVal value As Date)
            _fechaRegistro = value
        End Set
    End Property

    Public Property FechaModificacion() As Date
        Get
            Return _fechaModificacion
        End Get
        Set(ByVal value As Date)
            _fechaModificacion = value
        End Set
    End Property

    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Integer)
            _idEstado = value
        End Set
    End Property

    Public Property Estado() As String
        Get
            Return _estado
        End Get
        Protected Friend Set(ByVal value As String)
            _estado = value
        End Set
    End Property

    Public Property Observacion() As String
        Get
            Return _observacion
        End Get
        Set(ByVal value As String)
            _observacion = value
        End Set
    End Property

    Public Property NumeroRadicado() As Integer
        Get
            Return _numeroRadicado
        End Get
        Protected Friend Set(ByVal value As Integer)
            _numeroRadicado = value
        End Set
    End Property

    Public Property ComentarioEspecifico() As String
        Get
            Return _comentarioEspecifico
        End Get
        Set(ByVal value As String)
            _comentarioEspecifico = value
        End Set
    End Property

    Public Property NombreCliente() As String
        Get
            Return _nombreCliente
        End Get
        Protected Friend Set(ByVal value As String)
            _nombreCliente = value
        End Set
    End Property

    Public Property FechaAgenda() As String
        Get
            Return _fechaAgenda
        End Get
        Protected Friend Set(ByVal value As String)
            _fechaAgenda = value
        End Set
    End Property

    Public Property Identificacion() As String
        Get
            Return _identificacion
        End Get
        Protected Friend Set(ByVal value As String)
            _identificacion = value
        End Set
    End Property

    Public Property NombreContacto() As String
        Get
            Return _nombreContacto
        End Get
        Protected Friend Set(ByVal value As String)
            _nombreContacto = value
        End Set
    End Property

    Public Property Direccion() As String
        Get
            Return _direccion
        End Get
        Protected Friend Set(ByVal value As String)
            _direccion = value
        End Set
    End Property

    Public Property Telefono() As String
        Get
            Return _telefono
        End Get
        Set(ByVal value As String)
            _telefono = value
        End Set
    End Property

    Public Property IdTipoServicio As Integer
        Get
            Return _idTipoServicio
        End Get
        Set(value As Integer)
            _idTipoServicio = value
        End Set
    End Property

    Public Property TipoServicio As String
        Get
            Return _tipoServicio
        End Get
        Set(value As String)
            _tipoServicio = value
        End Set
    End Property

    Public Property Consultor As String
        Get
            Return _consultor
        End Get
        Set(value As String)
            _consultor = value
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

    Property IdSucursalFinanciera As Integer

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        Try
            With dbManager
                .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                .ejecutarReader("ObtenerNovedadServicioMensajeria", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        Integer.TryParse(.Reader("idServicioMensajeria").ToString, _idServicioMensajeria)
                        Integer.TryParse(.Reader("idTipoNovedad").ToString, _idTipoNovedad)
                        _tipoNovedad = .Reader("tipoNovedad").ToString
                        Integer.TryParse(.Reader("idUsuario").ToString, _idUsuario)
                        _usuarioRegistra = .Reader("usuarioRegistra").ToString
                        _fechaRegistro = CDate(.Reader("fechaRegistro"))
                        _fechaModificacion = CDate(.Reader("fechaModificacion"))
                        Integer.TryParse(.Reader("idEstado").ToString, _idEstado)
                        _estado = .Reader("estado").ToString
                        _observacion = .Reader("observacion").ToString
                        Integer.TryParse(.Reader("numeroRadicado").ToString, _numeroRadicado)
                        _comentarioEspecifico = .Reader("comentarioEspecifico").ToString
                        _nombreCliente = .Reader("nombre").ToString
                        _identificacion = .Reader("identicacion").ToString
                        _nombreContacto = .Reader("nombreAutorizado").ToString
                        Date.TryParse(.Reader("fechaAgenda").ToString, _fechaAgenda)
                        _direccion = .Reader("direccion").ToString
                        _telefono = .Reader("telefono").ToString
                        Integer.TryParse(.Reader("idTipoServicio").ToString, _idTipoServicio)
                        _tipoServicio = .Reader("tipoServicio").ToString

                        _registrado = True
                    End If
                    .Reader.Close()
                End If

            End With
        Finally

        End Try
    End Sub

    Public Shared Sub EnviarNotificacion(ByVal idServicio As Long, ByVal idTipoNovedad As Integer)
        Try
            Dim objServicio As New ServicioMensajeriaSiembra(idServicio)
            Dim objTipoNovedad As New TipoNovedad(idTipoNovedad)

            If objServicio.Registrado AndAlso objServicio.IdTipoServicio = Enumerados.TipoServicio.Siembra Then
                Dim objMail As New EMailManager(AsuntoNotificacion.Tipo.Notificación_Novedad_Servicio_Siembra, objServicio, objTipoNovedad)
                With objMail
                    If Not String.IsNullOrEmpty(objServicio.EmailConsultor) Then .AdicionarDestinatario(objServicio.EmailConsultor)
                    If Not String.IsNullOrEmpty(objServicio.EmailCoordinador) Then .AdicionarDestinatarioCopia(objServicio.EmailCoordinador)
                    .EnviarMail()
                End With
            ElseIf objServicio.IdTipoServicio = Enumerados.TipoServicio.ServiciosFinancierosBancolombia Then
                Dim resultado As ResultadoProceso
                resultado = ActualizarGestionVenta(New ServicioNotusExpressBancolombia, objServicio.IdServicioMensajeria, idTipoNovedad, "Servicio modificado desde CEM, por el usuario: ")
            ElseIf objServicio.IdTipoServicio = Enumerados.TipoServicio.DaviviendaSamsung Then
                Dim resultado As ResultadoProceso
                resultado = ActualizarGestionVenta(New ServicioNotusExpressDaviviendaSamsung, objServicio.IdServicioMensajeria, idTipoNovedad, "Servicio modificado desde CEM, por el usuario: ")
            End If
        Catch ex As Exception
            Throw New Exception("No se logró enviar notificación al Consultor: " & ex.Message, ex.InnerException)
        End Try
    End Sub


    Public Shared Sub EnviarNotificacionNovedad(ByVal NumeroRadicado As Int64, ByVal idTipoNovedad As Integer)
        Try
            Dim objServicio As New ServicioMensajeriaSiembra()
            objServicio.CargarDatosRadicado(NumeroRadicado)
            Dim objTipoNovedad As New TipoNovedad(idTipoNovedad)

            If objServicio.Registrado AndAlso objServicio.IdTipoServicio = Enumerados.TipoServicio.Siembra Then
                Dim objMail As New EMailManager(AsuntoNotificacion.Tipo.Notificación_Novedad_Servicio_Siembra, objServicio, objTipoNovedad)
                With objMail
                    If Not String.IsNullOrEmpty(objServicio.EmailConsultor) Then .AdicionarDestinatario(objServicio.EmailConsultor)
                    If Not String.IsNullOrEmpty(objServicio.EmailCoordinador) Then .AdicionarDestinatarioCopia(objServicio.EmailCoordinador)
                    .EnviarMail()
                End With
            ElseIf objServicio.IdTipoServicio = Enumerados.TipoServicio.ServiciosFinancierosBancolombia Then
                Dim resultado As NotusExpressBancolombiaService.ResultadoProceso

                Dim servicioNEBS As New NotusExpressBancolombiaService.NotusExpressBancolombiaService
                Dim idUsuario As Integer = HttpContext.Current.Session("usxp001")
                resultado = servicioNEBS.CrearNovedadCem(idTipoNovedad, objServicio.IdServicioMensajeria, idUsuario)
            End If
        Catch ex As Exception
            Throw New Exception("No se logró enviar notificación al Consultor: " & ex.Message, ex.InnerException)
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resultado As New ResultadoProceso
        Try
            With dbManager
                .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario

                .SqlParametros.Add("@observacion", SqlDbType.VarChar, 2000).Value = _observacion
                If Not String.IsNullOrEmpty(_comentarioEspecifico) Then _
                    .SqlParametros.Add("@comentarioEspecifico", SqlDbType.VarChar, 2000).Value = _comentarioEspecifico
                If _idTipoNovedad > 0 Then .SqlParametros.Add("@idTipoNovedad", SqlDbType.Int).Value = _idTipoNovedad
                If _IdSucursalFinanciera > 0 Then .SqlParametros.Add("@idSucursalFinanciera", SqlDbType.Int).Value = _IdSucursalFinanciera
                .SqlParametros.Add("@result", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                If _idTipoNovedad > 0 Then
                    .ejecutarReader("RegistrarNovedadServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.Read Then
                        resultado.Valor = .Reader("valor")
                        If resultado.Valor = 0 Then
                            resultado.Mensaje = "La novedad fue registrada de manera exitosa."

                            EnviarNotificacion(_idServicioMensajeria, _idTipoNovedad)
                        Else
                            resultado.Mensaje = .Reader("mensaje").ToString
                        End If
                        .Reader.Close()
                    Else
                        Throw New Exception("Ocurrió un error interno al registrar la novedad. Por favor intente nuevamente")
                    End If
                Else
                    Throw New Exception("Ocurrió un error interno al registrar la novedad, El idTipoNovedad no puede ser validado. Por favor intente nuevamente")
                End If

            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function

    Public Function Actualizar(ByVal idUsuario As Integer) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = _idNovedad
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    If _idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                    If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    If _observacion <> "" Then .SqlParametros.Add("@observacion", SqlDbType.VarChar).Value = _observacion
                    If _comentarioEspecifico <> "" Then .SqlParametros.Add("@comentarioEspecifico", SqlDbType.VarChar).Value = _comentarioEspecifico
                    If _idTipoNovedad > 0 Then .SqlParametros.Add("@idTipoNovedad", SqlDbType.Int).Value = _idTipoNovedad

                    .IniciarTransaccion()
                    .ejecutarReader("ModificarNovedadServicioMensajeria", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.Read Then
                        resultado.Valor = .Reader("valor")
                        If resultado.Valor = 0 Then
                            .Reader.Close()
                            .ConfirmarTransaccion()
                            resultado.Mensaje = "La novedad fue actualizad de manera exitosa."
                        Else
                            .AbortarTransaccion()
                            resultado.Mensaje = .Reader("mensaje").ToString
                        End If
                    Else
                        .AbortarTransaccion()
                        Throw New Exception("Ocurrió un error interno al registrar la novedad. Por favor intente nuevamente")
                    End If
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function

    Public Function RegistrarRechazo(ByVal idUsuario As Integer) As ResultadoProceso
        Dim db As New LMDataAccess
        Dim dt As New DataTable
        Dim res As New ResultadoProceso
        db.IniciarTransaccion()
        Try
            With db.SqlParametros
                .Clear()
                .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                .Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                .Add("@origenCausal", SqlDbType.Int).Value = _origenCausal
                .Add("@causales", SqlDbType.VarChar).Value = _causales
                .Add("@observacion", SqlDbType.VarChar).Value = _observacion
            End With
            dt = db.EjecutarDataTable("GuardarRechazoMesaControl", CommandType.StoredProcedure)
            If dt.Rows.Count > 0 Then
                db.AbortarTransaccion()
                res.EstablecerMensajeYValor(dt.Rows(0).Item("errorNo"), dt.Rows(0).Item("mensaje"))
            Else
                db.ConfirmarTransaccion()
                res.EstablecerMensajeYValor(0, "Novedad guardada Exitosamente")
            End If
        Catch ex As Exception
            If db.EstadoTransaccional Then db.AbortarTransaccion()
            res.EstablecerMensajeYValor(-1, "Error al guardar novedad: " & ex.Message)
        End Try
        Return res
    End Function
#End Region

#Region "Métodos Compartidos"

    Public Shared Function ActualizarGestionVenta(ByVal servicioNotusExpress As IServicioNotusExpress,
                                           ByVal idServicio As Integer,
                                           ByVal idEstado As Integer,
                                           Optional ByVal justificacion As String = "Servicio modificado desde CEM, por el usuario: Admin") As ResultadoProceso
        Return servicioNotusExpress.ActualizarGestionVenta(idServicio, idEstado, justificacion)
    End Function

    Public Shared Function RegistrarNovedadEntregaServicio(ByVal numeroRadicado As Int64, ByVal idTipoNovedad As Short, ByVal idUsuario As Int32) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim idResultado As Integer = -1

        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .SqlParametros.Add("@numeroRadicado", SqlDbType.Decimal).Value = numeroRadicado
                .SqlParametros.Add("@idTipoNovedad", SqlDbType.SmallInt).Value = idTipoNovedad
                .SqlParametros.Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario

                idResultado = CInt(.EjecutarScalar("RegistrarNovedadEntregaServicioMensajeria", CommandType.StoredProcedure))

                If idResultado = 0 Then
                    resultado.EstablecerMensajeYValor(0, "Novedad registrada satisfactoriamente.")
                    EnviarNotificacionNovedad(numeroRadicado, idTipoNovedad)
                ElseIf idResultado = 1 Then
                    resultado.EstablecerMensajeYValor(1, "Ocurrio un error inesperado al registrar novedad.")
                ElseIf idResultado = 2 Then
                    resultado.EstablecerMensajeYValor(2, "No se puede registrar la novedad, puesto que el servicio ya fue confirmado como entregado.")
                ElseIf idResultado = 3 Then
                    resultado.EstablecerMensajeYValor(3, "No se puede registrar la novedad, puesto que el numero de radicado no existe.")
                End If

            End With
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(3, ex.Message)
        End Try
        dbManager.Dispose()

        Return resultado
    End Function

#End Region

End Class
