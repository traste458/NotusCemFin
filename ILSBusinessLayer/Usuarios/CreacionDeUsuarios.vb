Imports System.Text
Imports LMDataAccessLayer
Imports System.Net.Mail
Imports System.Security.Claims
Imports System.Text.RegularExpressions
Imports System.Web

Public Class CreacionDeUsuarios



    Private _Identificacion As String

    Private _idperfil As Integer

    Private _Usuario As String

    Private _NombreCompleto As String

    Private _perfil As String

    Private _ciudad As String

    Private _centroCosto As String

    Private _correo As String

    Property idUsuario As Integer

    Property idTerceroCreado As Integer

    Property idEmpresaTemporal As Integer
    Property IdCiudad As Integer

    Property idCentroCostos As Integer

    Property idCall As Integer

    Property idSiteCall As Integer
    Property idTercero As Integer

    Property Observacion As String

    Property contrasena As String


    Public Property perfil As String
        Get
            Return _perfil
        End Get
        Set(value As String)
            _perfil = value
        End Set
    End Property

    Public Property centroCosto As String
        Get
            Return _centroCosto
        End Get
        Set(value As String)
            _centroCosto = value
        End Set
    End Property

    Public Property correo As String
        Get
            Return _correo
        End Get
        Set(value As String)
            _correo = value
        End Set
    End Property

    Public Property idperfil As Integer
        Get
            Return _idperfil
        End Get
        Set(value As Integer)
            _idperfil = value
        End Set
    End Property

    Public Property ciudad As String
        Get
            Return _ciudad
        End Get
        Set(value As String)
            _ciudad = value
        End Set
    End Property

    Public Property NombreCompleto As String
        Get
            Return _NombreCompleto
        End Get
        Set(value As String)
            _NombreCompleto = value
        End Set
    End Property

    Public Property Usuario As String
        Get
            Return _Usuario
        End Get
        Set(value As String)
            _Usuario = value
        End Set
    End Property

    Public Property Identificacion As String
        Get
            Return _Identificacion
        End Get
        Set(value As String)
            _Identificacion = value
        End Set
    End Property

    Public Overloads Function ConsultarCentrosDeCosto()
        Dim db As New LMDataAccess
        Dim dtCentrosDeCosto As New DataTable
        With db
            dtCentrosDeCosto = .EjecutarDataTable("ObtenerCentroDeCostos", CommandType.StoredProcedure)
        End With
        Return dtCentrosDeCosto
    End Function

    Public Overloads Function ConsultarCallCenter()
        Dim db As New LMDataAccess
        Dim dtCallCenterAs As New DataTable
        With db
            dtCallCenterAs = .EjecutarDataTable("ObtenerCallCenter", CommandType.StoredProcedure)
        End With
        Return dtCallCenterAs
    End Function

    Public Overloads Function ConsultarSitesCallCenter(ByVal idCall As Integer)
        Dim db As New LMDataAccess
        Dim dtSitesCallCenter As New DataTable
        With db
            .SqlParametros.Add("@idCall", SqlDbType.Int).Value = idCall
            dtSitesCallCenter = .EjecutarDataTable("ObtenerSitesCallCenter", CommandType.StoredProcedure)
        End With
        Return dtSitesCallCenter
    End Function

    Public Overloads Function ConsultardtEmpresaTemporal()
        Dim db As New LMDataAccess
        Dim dtEmpresaTemporal As New DataTable
        With db
            dtEmpresaTemporal = .EjecutarDataTable("ObtenerEmpresaTemporar", CommandType.StoredProcedure)
        End With
        Return dtEmpresaTemporal
    End Function

    Public Overloads Function ConsultarPerfiles()
        Dim db As New LMDataAccess
        Dim dtPerfiles As New DataTable
        With db
            dtPerfiles = .EjecutarDataTable("ObtenerPerfilesSistema", CommandType.StoredProcedure)
        End With
        Return dtPerfiles
    End Function

    Public Overloads Function listarUsuarios()
        Dim db As New LMDataAccess
        Dim dtUsuarios As New DataTable
        With db
            If idperfil > 0 Then .SqlParametros.Add("@idperfil", SqlDbType.Int).Value = idperfil
            If idTerceroCreado > 0 Then .SqlParametros.Add("@idTerceroCreado", SqlDbType.Int).Value = idTerceroCreado
            If idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
            If IdCiudad > 0 Then .SqlParametros.Add("@IdCiudad", SqlDbType.Int).Value = IdCiudad
            If Identificacion <> "" Then .SqlParametros.Add("@Cedula", SqlDbType.VarChar).Value = Identificacion
            If Usuario <> "" Then .SqlParametros.Add("@Usuario", SqlDbType.VarChar).Value = Usuario
            If NombreCompleto <> "" Then .SqlParametros.Add("@NombreCompleto", SqlDbType.VarChar).Value = NombreCompleto
            dtUsuarios = .EjecutarDataTable("listarUsuarios", CommandType.StoredProcedure)
        End With
        Return dtUsuarios
    End Function

    Public Function DesactivarTercero(ByVal IdUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idTercero", SqlDbType.Int).Value = idTercero
                    .Add("@Observacion", SqlDbType.VarChar).Value = Observacion
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("DesabilitarUsuario", CommandType.StoredProcedure)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Catch ex As Exception
                resultado.EstablecerMensajeYValor(33, ex.Message)
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function HabilitarTercero(ByVal IdUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idTercero", SqlDbType.Int).Value = idTercero
                    .Add("@Observacion", SqlDbType.VarChar).Value = Observacion
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("habilitarTercero", CommandType.StoredProcedure)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Catch ex As Exception
                resultado.EstablecerMensajeYValor(33, ex.Message)
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function


    Public Function ModificarTercero(ByVal IdUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idTercero", SqlDbType.Int).Value = idTercero
                    .Add("@nombreCompleto", SqlDbType.VarChar).Value = NombreCompleto
                    .Add("@identificacion", SqlDbType.VarChar).Value = Identificacion
                    .Add("@usuario", SqlDbType.VarChar).Value = Usuario
                    .Add("@correo", SqlDbType.VarChar).Value = correo
                    .Add("@idPerfil", SqlDbType.Int).Value = idperfil
                    .Add("@idCiudad", SqlDbType.Int).Value = IdCiudad
                    .Add("@idCentroCostos", SqlDbType.Int).Value = idCentroCostos
                    .Add("@idEmpresaTemporal", SqlDbType.Int).Value = idEmpresaTemporal
                    .Add("@idsite", SqlDbType.Int).Value = idSiteCall
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("ActualizarTercero", CommandType.StoredProcedure)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Catch ex As Exception
                resultado.EstablecerMensajeYValor(33, ex.Message)
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function


    Public Function CrearTercero(ByRef idTerceroUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@nombreCompleto", SqlDbType.VarChar).Value = NombreCompleto
                    .Add("@identificacion", SqlDbType.VarChar).Value = Identificacion
                    .Add("@usuario", SqlDbType.VarChar).Value = Usuario
                    .Add("@correo", SqlDbType.VarChar).Value = correo
                    .Add("@idPerfil", SqlDbType.Int).Value = idperfil
                    .Add("@idCiudad", SqlDbType.Int).Value = IdCiudad
                    If idCentroCostos <> 0 Then .Add("@idCentroCostos", SqlDbType.Int).Value = idCentroCostos
                    If idEmpresaTemporal <> 0 Then .Add("@idEmpresaTemporal", SqlDbType.Int).Value = idEmpresaTemporal
                    .Add("@idsite", SqlDbType.Int).Value = idSiteCall
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@idTerceroCreado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CrearTercero", CommandType.StoredProcedure)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                idTerceroUsuario  = .SqlParametros("@idTerceroCreado").Value
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Catch ex As Exception
                resultado.EstablecerMensajeYValor(33, ex.Message)
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function


    Public Overloads Sub CargarInformacionTercero(idTercero As Integer)
        If idTercero > 0 Then
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idTercero", SqlDbType.Int).Value = idTercero
                    .ejecutarReader("ObtenerDatosTercero", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idtercero").ToString, idTercero)
                            _Identificacion = .Reader("Cedula").ToString
                            _NombreCompleto = .Reader("nombre").ToString
                            contrasena = .Reader("clave").ToString
                            _Usuario = .Reader("usuario").ToString
                            _correo = .Reader("correo").ToString
                            _centroCosto = .Reader("centroCosto").ToString
                            _perfil = .Reader("perfil").ToString
                            _ciudad = .Reader("ciudad").ToString
                            Integer.TryParse(.Reader("idperfil"), _idperfil)
                            Integer.TryParse(.Reader("idcentro_costo"), idCentroCostos)
                            Integer.TryParse(.Reader("idciudad"), IdCiudad)
                            Integer.TryParse(.Reader("idempresa_temporal"), idEmpresaTemporal)
                            Integer.TryParse(.Reader("idCallCenter"), idCall)
                            Integer.TryParse(.Reader("idSite"), idSiteCall)
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If

    End Sub

    Public Function notificarContrasena(ByVal identificacion As String) As Integer
        Dim contrasenaRandom As New CreacionContrasenaRandom
        Dim encriptarContrasena As New EncryptionLibrary
        Dim resultadoConsulta As New List(Of String)
        Dim contrRandom As String
        Dim recuperarContrasenaBD As New RecuperacionContrasena

        contrRandom = contrasenaRandom.CreacionContrasenaRandom()
        resultadoConsulta = recuperarContrasenaBD.AlmacenarRamdomContrasena(identificacion, contrRandom)


        If resultadoConsulta.Item(0) = "0" Then
            Return 2
        End If

        If resultadoConsulta.Item(0) <> "0" Then
            With recuperarContrasenaBD
                EnviarCorreoCreacionUsuarioContrasena(resultadoConsulta.Item(1), resultadoConsulta.Item(0), resultadoConsulta.Item(2))
            End With
            Return 1
        Else
            Return 0
        End If

    End Function


    Public Function EnviarCorreoCreacionUsuarioContrasena(ByVal destinatario As String, ByVal usuario As String, ByVal token As String)
        Dim direccionPara As New MailAddressCollection
        Dim resultadoEnviado As Boolean = False
        Dim sb As New StringBuilder
        Dim correo As New AdministradorCorreo
        correo.Receptor.Add(destinatario)
        Dim urlRecuperacion As Comunes.ConfigValues = New Comunes.ConfigValues("URL_RECUPERACION_CONTRASENA")
        Try
            With correo
                .Titulo = "Asignacion Contraseña Creacion De Usuario Nuevo"
                .Asunto = "Asignacion Contraseña Creacion De Usuario Nuevo"
                .Receptor = .Receptor
                .TextoMensaje = String.Concat("Hola: ", usuario, vbCrLf, ", Por favor ingresar al link de abajo para asignar la contraseña a su usuario: ", vbCrLf, "</br><a style='margin:10px 0 10px 0;color:#ffffff;font-weight:bold;display:inline-block;padding:6px 10px;font-size:16px;text-align:center;background-image:none;border:1px solid transparent;border-radius:10px;-moz-border-radius:10px;-webkit-border-radius:10px;-khtml-border-radius:10px; background-color:#836493;' href='" & urlRecuperacion.ConfigKeyValue.ToString & token & "'> Asignacion contraseña </a></br>", vbCrLf, "si no asigna contraseña no podra ingresar con el usuario creado al sistema")
                .FirmaMensaje = "Logytech Mobile S.A.S <br />PBX. 57(1) 4395237 Ext 174 - 135"
                resultadoEnviado = .EnviarMail()
            End With
        Finally
        End Try
        Return resultadoEnviado
    End Function


End Class