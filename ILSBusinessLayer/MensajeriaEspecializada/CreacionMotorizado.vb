Imports System.Text
Imports LMDataAccessLayer
Imports System.Net.Mail
Imports System.Security.Claims
Imports System.Text.RegularExpressions
Imports System.Web

Public Class CreacionMotorizado

    Public Property resultado As New ResultadoProceso

    Private _Identificacion As String

    Private _idperfil As String

    Private _Usuario As String

    Private _NombreCompleto As String

    Private _placa As String

    Private _perfil As String

    Private _ciudad As String

    Private _centroCosto As String

    Private _correo As String

    Property idUsuario As Integer

    Property idTerceroCreado As Integer

    Property idEmpresaTemporal As Integer
    Property IdCiudad As Integer

    Property IdZona As Integer

    Property idCentroCostos As Integer

    Property idCall As Integer

    Property idSiteCall As Integer
    Property idTercero As Integer

    Property Observacion As String

    Property contrasena As String

    Property idDriver As Integer

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

    Public Property idperfil As String
        Get
            Return _idperfil
        End Get
        Set(value As String)
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

    Public Property Placa As String
        Get
            Return _placa
        End Get
        Set(value As String)
            _placa = value
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

    Public Overloads Function ConsultarPerfiles(idperfil As String)
        Dim db As New LMDataAccess
        Dim dtPerfiles As New DataTable
        With db
            db.SqlParametros.Add("@listPerfiles", SqlDbType.VarChar).Value = idperfil
            dtPerfiles = .EjecutarDataTable("ObtenerPerfilesMotorizado", CommandType.StoredProcedure)
        End With
        Return dtPerfiles
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

    Public Function CrearMotorizado(ByRef idTerceroUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@nombreCompleto", SqlDbType.VarChar).Value = NombreCompleto
                    .Add("@placa", SqlDbType.VarChar).Value = Placa
                    .Add("@identificacion", SqlDbType.VarChar).Value = Identificacion
                    .Add("@idPerfil", SqlDbType.Int).Value = idperfil
                    .Add("@idCiudad", SqlDbType.Int).Value = IdCiudad
                    .Add("@idDriver", SqlDbType.Int).Value = idDriver
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                    .Add("@idTerceroCreado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CrearMotorizado", CommandType.StoredProcedure)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                idTerceroUsuario = .SqlParametros("@idTerceroCreado").Value
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
                    .Add("@idPerfil", SqlDbType.Int).Value = idperfil
                    .Add("@idCiudad", SqlDbType.Int).Value = IdCiudad
                    .Add("@placa", SqlDbType.VarChar).Value = Placa
                    .Add("@idZona", SqlDbType.Int).Value = IdZona
                    .Add("@idDriver", SqlDbType.Int).Value = idDriver
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("ActualizarMotorizado", CommandType.StoredProcedure)
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

    Public Overloads Sub CargarInformacionMotorizado(idTercero As Integer)
        If idTercero > 0 Then
            Dim dbManager As New LMDataAccess

            Try
                With dbManager
                    .SqlParametros.Add("@idTercero", SqlDbType.Int).Value = idTercero
                    .ejecutarReader("ObtenerDatosMotorizado", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idtercero").ToString, idTercero)
                            _Identificacion = .Reader("Cedula").ToString
                            _NombreCompleto = .Reader("nombre").ToString
                            _perfil = .Reader("perfil").ToString
                            _placa = .Reader("placa").ToString
                            _ciudad = .Reader("ciudad").ToString
                            Integer.TryParse(.Reader("idperfil"), _idperfil)
                            Integer.TryParse(.Reader("idciudad"), IdCiudad)
                            Integer.TryParse(.Reader("idZona"), IdZona)
                            Integer.TryParse(.Reader("idDriver"), idDriver)
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End If

    End Sub

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
            dtUsuarios = .EjecutarDataTable("listarMotorizados", CommandType.StoredProcedure)
        End With
        Return dtUsuarios
    End Function

    Public Function CargarMasivoMotorizado(dtMotorizado As DataTable)

        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                End With
                .EjecutarNonQuery("EliminarTransitoriaMotorizados", CommandType.StoredProcedure)
                .TiempoEsperaComando = 0
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TrancitoriCreacionMotorizadosMasiva"
                    .ColumnMappings.Add("fila", "Fila")
                    .ColumnMappings.Add("idUsuario", "IdUsuario")
                    .ColumnMappings.Add("NombreCompleto", "NombreCompleto")
                    .ColumnMappings.Add("Identificacion", "Identificacion")
                    .ColumnMappings.Add("IdPerfil", "IdPerfil")
                    .ColumnMappings.Add("Ciudad", "Ciudad")
                    .ColumnMappings.Add("Placa", "Placa")
                    .ColumnMappings.Add("idDriver", "idDriver")
                    .WriteToServer(dtMotorizado)
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                End With
                dt = .EjecutarDataTable("ValidarCargueUsuariosMotorizado", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@result").Value.ToString, Integer)
                If resul = 0 Then
                    resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                    .ConfirmarTransaccion()
                    Return dt
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                    Return dt
                    Exit Function
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dt
    End Function

End Class
