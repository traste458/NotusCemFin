Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Comunes
Imports LMDataAccessLayer
Imports System.Net.Mail
Imports System.Text
Imports System.IO

Public Class CambioDeSoftware

#Region "Atributos (Campos)"

    Private _dbManager As New LMDataAccess
    Private dtError As New DataTable
    Private _idUsuario As Integer
    Private _idAutorizacion As Long
    Private _idEstado As Integer
    Private _fechaInicial As DateTime
    Private _fechaFinal As DateTime
    Private _cargaInicial As Integer
    Private _observacion As String
    Private _ruta As String

#End Region

#Region "Propiedades"

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property IdAutorizacion() As Long
        Get
            Return _idAutorizacion
        End Get
        Set(ByVal value As Long)
            _idAutorizacion = value
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

    Public Property FechaInicial() As DateTime
        Get
            Return _fechaInicial
        End Get
        Set(ByVal value As DateTime)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal() As DateTime
        Get
            Return _fechaFinal
        End Get
        Set(ByVal value As DateTime)
            _fechaFinal = value
        End Set
    End Property

    Public Property CargaInicial() As Integer
        Get
            Return _cargaInicial
        End Get
        Set(ByVal value As Integer)
            _cargaInicial = value
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

    Public Property Ruta() As String
        Get
            Return _ruta
        End Get
        Set(ByVal value As String)
            _ruta = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()

        dtError.Columns.Add(New DataColumn("Linea"))
        dtError.Columns.Add(New DataColumn("Descripción"))
        dtError.Columns.Add(New DataColumn("Serial", GetType(String)))
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Shared Function CargarPlano(ByVal dtDatos As DataTable, ByVal idUsuario As Integer) As Boolean
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As DataTable
        dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        Try
            db.agregarParametroSQL("@idUsuario", idUsuario, SqlDbType.BigInt)
            db.ejecutarNonQuery("BorrarTablaTemporalAutorizaciones", CommandType.StoredProcedure)
            db.inicilizarBulkCopy()
            db.BulkCopy.DestinationTableName = "SerialesTransitoriosTipoSoftware"
            db.BulkCopy.WriteToServer(dtDatos)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try

    End Function

    Public Function CargarInventarioArchivo(ByRef dtError As DataTable, Optional ByVal Flag As Integer = 0) As ResultadoProceso
        Dim resultado As New ResultadoProceso

        If IdUsuario > 0 Then
            If _dbManager Is Nothing Then _dbManager = New LMDataAccess
            With _dbManager
                With .SqlParametros
                    .Clear()
                    If Flag > 0 Then
                        .Add("@flag", SqlDbType.Int).Value = Flag
                    End If
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .Add("@idAutorizacion", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .iniciarTransaccion()
                Dim returnValue As Integer
                dtError = .ejecutarDataTable("IngresarSerialesCambioSoftware", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@returnValue").Value) AndAlso Integer.TryParse(.SqlParametros("@returnValue").Value.ToString, returnValue) Then
                    If returnValue = 0 Then
                        If dtError Is Nothing OrElse dtError.Rows.Count = 0 Then
                            If Integer.TryParse(.SqlParametros("@idAutorizacion").Value.ToString, IdAutorizacion) Then
                                resultado.EstablecerMensajeYValor(0, "Información cargada correctamente")
                                .confirmarTransaccion()
                            End If
                        Else
                            resultado.EstablecerMensajeYValor(2, "No se pudo realizar el cargue de la información. El archivo contenía registros erroneos")
                            .confirmarTransaccion()
                        End If
                    ElseIf returnValue = 2 Then
                        resultado.EstablecerMensajeYValor(2, "No se pudo realizar el cargue de la información. Todos los seriales del archivo se encuentran con diferencias.")
                        .confirmarTransaccion()
                    Else
                        If _dbManager IsNot Nothing AndAlso _dbManager.estadoTransaccional Then _dbManager.abortarTransaccion()
                        resultado.EstablecerMensajeYValor(1, "Ocurrió un error inesperado al realizar el registro. Por favor intente nuevamente")
                    End If
                Else
                    If _dbManager IsNot Nothing AndAlso _dbManager.estadoTransaccional Then _dbManager.abortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "Imposible determinar si el registro fue satisfactorio. Por favor intente registrar nuevamente")
                End If
            End With
        Else
            resultado.EstablecerMensajeYValor(1, "No se han proporcionado los datos necesarios para cargar los seriales.")
        End If
        Return resultado
    End Function

    Public Function NotificarSolicitud(ByVal idAutorizacion As Integer) As ResultadoProceso
        Dim Notificacion As New AdministradorCorreo
        Dim DestinosPP As New MailAddressCollection
        Dim DestinosCC As New MailAddressCollection
        Dim respuestaEnvio As ResultadoProceso
        Dim sbContenido As New StringBuilder
        Dim _adjuntos As New ArrayList
        Dim dtAdjunto As New DataTable
        Try

            With sbContenido
                .Append("Se notifica la creación de solicitud de autorización para cambio de tipo de Software número: " & idAutorizacion)
                .Append("<br/>Por favor ingrese al sistema para visualizar las autorizaciones pendientes. ")
            End With

            With Notificacion

                CargarDestinatarios(AsuntoNotificacion.Tipo.AutorizacionCambioSoftware, DestinosPP, DestinosCC)

                dtAdjunto = ObtenerRuta(idAutorizacion)
                Dim dr As DataRow() = dtAdjunto.Select("idAutorizacion=" & idAutorizacion.ToString())
                Dim ruta As String = dr(0).Item("rutaArchivo").ToString
                _adjuntos.Add(ruta)

                .Titulo = "Solicitud de Cambio de Software"
                .Asunto = "Notificación de solicitud de cambio de Software"
                .TextoMensaje = sbContenido.ToString
                .FirmaMensaje = "Logytech Mobile S.A.S <br />"
                .Receptor = DestinosPP
                .Copia = DestinosCC
                .RutaAttachment = _adjuntos
                If Not .EnviarMail() Then
                    respuestaEnvio.Valor = 1
                    respuestaEnvio.Mensaje = "Ocurrió un error inesperado y no fué posible enviar la notificación"
                End If
            End With

        Catch ex As Exception

        End Try
    End Function

    Private Function CargarDestinatarios(ByVal tipo As Comunes.AsuntoNotificacion.Tipo, ByVal destinoPP As MailAddressCollection, ByVal destinoCC As MailAddressCollection) As MailAddressCollection
        Dim ConfiguracionUsuario As New UsuarioNotificacion
        Dim filtro As New FiltroUsuarioNotificacion
        Dim dtDestinos As New DataTable
        Dim strDestinoPP, strDestinoCC As String

        filtro.IdAsuntoNotificacion = tipo
        filtro.Separador = ","
        Try
            dtDestinos = ConfiguracionUsuario.ObtenerDestinatarioNotificacion(filtro)
            For Each fila As DataRow In dtDestinos.Rows
                strDestinoPP += fila.Item("destinoPara")
                strDestinoCC += fila.Item("destinoCopia")
            Next

            destinoPP.Add(strDestinoPP)
            destinoCC.Add(strDestinoCC)

        Catch ex As Exception
        Finally
            If dtDestinos IsNot Nothing Then dtDestinos.Rows.Clear()
        End Try

    End Function

    Public Function ObtenerPool() As DataTable
        Dim dtPool As DataTable
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess

        Try
            With _dbManager
                If _fechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = _fechaInicial
                If _fechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = _fechaFinal
                If _idAutorizacion > 0 Then .SqlParametros.Add("@idAutorizacion", SqlDbType.Int).Value = _idAutorizacion
                If _idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                If _cargaInicial > 0 Then .SqlParametros.Add("@flag", SqlDbType.Int).Value = _cargaInicial
                dtPool = .ejecutarDataTable("ConsultarAutorizacionesCambioSoftware", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtPool
    End Function

    Public Shared Function ObtenerSeriales(ByVal idAutorizacion As Integer) As DataTable
        Dim dtSerial As DataTable
        Dim _dbManager As New LMDataAccess

        Try
            With _dbManager
                .SqlParametros.Add("@idAutorizacion", SqlDbType.Int).Value = idAutorizacion
                dtSerial = .ejecutarDataTable("ConsultarAutorizacionesCambioSoftwareSerial", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtSerial
    End Function

    Public Function Actualizar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            With .SqlParametros
                .Clear()
                If IdUsuario > 0 Then .Add("@idUsuarioAutoriza", SqlDbType.Int).Value = IdUsuario
                .Add("@idAutorizacion", SqlDbType.Int).Value = IdAutorizacion
                If Not String.IsNullOrEmpty(Observacion) Then .Add("@observacion", SqlDbType.VarChar, 450).Value = Observacion
                If IdEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = IdEstado
                If Not String.IsNullOrEmpty(Ruta) Then .Add("@ruta", SqlDbType.VarChar, 450).Value = Ruta
                .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            End With
            Dim returnValue As Integer
            .ejecutarNonQuery("ActualizaAutorizacionCambioSW", CommandType.StoredProcedure)
            If Not IsDBNull(.SqlParametros("@returnValue").Value) AndAlso Integer.TryParse(.SqlParametros("@returnValue").Value.ToString, returnValue) Then
                If returnValue = 0 Then
                    resultado.EstablecerMensajeYValor(0, "Se realizo el cambio de la autorización satisfactoriamente. ")
                End If
            Else
                If _dbManager IsNot Nothing AndAlso _dbManager.estadoTransaccional Then _dbManager.Dispose()
                resultado.EstablecerMensajeYValor(1, "Imposible determinar si el registro fue satisfactorio. Por favor intente registrar nuevamente")
            End If
        End With
        Return resultado
    End Function

    Private Function ObtenerRuta(ByVal idAutorizacion As Integer) As DataTable
        Dim _dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            With _dbManager
                If idAutorizacion > 0 Then _
                    .SqlParametros.Add("@idAutorizacion", SqlDbType.Int).Value = idAutorizacion
            End With
            Return _dbManager.ejecutarDataTable("ConsultarAutorizacionesCambioSoftware", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try

    End Function

#End Region

#Region "Metodos Compartidos"

    Public Shared Function ConsultarEstado(ByVal idEntidad As Integer) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                .SqlParametros.Add("@idEntidad", SqlDbType.Int).Value = idEntidad
                dtDatos = .ejecutarDataTable("ConsultarEstadoEntidad", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Shared Function ConsultarAutorizaciones() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                dtDatos = .ejecutarDataTable("ConsultarAutorizacionesCambioSoftware", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class
