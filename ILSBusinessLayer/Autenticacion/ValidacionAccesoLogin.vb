Imports System.Net.Mail
Imports LMDataAccessLayer

Public Class ValidacionAccesoLogin
#Region "Atributos (Campos)"

    Private _usuario As String
    Private _password As String
    Private _latitud As String
    Private _longitud As String
    Private _idUsuario As Integer
    Private _esPresencial As Boolean
    Private _idPerfil As Integer
    Private _idCiudad As Integer
    Private _passwordAntiguo As String
    Private _passwordNuevo As String
    Private _confirmacionPasswordNuevo As String

#End Region

#Region "Propiedades"

    Public Property Usuario() As String
        Get
            Return _usuario
        End Get
        Set(ByVal value As String)
            _usuario = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return _password
        End Get
        Set(ByVal value As String)
            _password = value
        End Set
    End Property

    Public Property PasswordAntiguo() As String
        Get
            Return _passwordAntiguo
        End Get
        Set(ByVal value As String)
            _passwordAntiguo = value
        End Set
    End Property

    Public Property PasswordNuevo() As String
        Get
            Return _passwordNuevo
        End Get
        Set(ByVal value As String)
            _passwordNuevo = value
        End Set
    End Property

    Public Property ConfirmarPasswordNuevo() As String
        Get
            Return _confirmacionPasswordNuevo
        End Get
        Set(ByVal value As String)
            _confirmacionPasswordNuevo = value
        End Set

    End Property

    Public Property Latitud() As String
        Get
            Return _latitud
        End Get
        Set(value As String)
            _latitud = value
        End Set
    End Property

    Public Property Longitud() As String
        Get
            Return _longitud
        End Get
        Set(value As String)
            _longitud = value
        End Set
    End Property

    Public Property EsPresencial() As Boolean
        Get
            Return _esPresencial
        End Get
        Set(value As Boolean)
            _esPresencial = value
        End Set
    End Property

    Public ReadOnly Property IdPerfil() As Integer
        Get
            Return _idPerfil
        End Get
    End Property

    Public ReadOnly Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
    End Property

    Public ReadOnly Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
    End Property

#End Region

    Public Function EsUsuarioValido(ByVal pwdEncriptado As String, ByVal usuario As String) As String

        Dim dbManager As New LMDataAccess
        Dim resultado As Byte = 1
        Dim usuarioValido As Byte
        Dim identificacion As String = 0
        Try
            With dbManager
                .SqlParametros.Add("@usuario", SqlDbType.VarChar, 100).Value = usuario
                .SqlParametros.Add("@pwd", SqlDbType.VarChar, 100).Value = pwdEncriptado
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Direction = ParameterDirection.Output
                .SqlParametros.Add("@identificacion", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .EjecutarNonQuery("SP_ValidarCredencialesDeAcceso", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultado = CByte(.SqlParametros("@resultado").Value.ToString)
                    Integer.TryParse(.SqlParametros("@idUsuario").Value.ToString, _idUsuario)
                    If _idUsuario > 0 Then
                        usuarioValido = ValidarPrimerIngreso(_idUsuario)
                        If usuarioValido = 0 Then
                            identificacion = 1
                            Return identificacion
                        Else
                            identificacion = .SqlParametros("@identificacion").Value.ToString
                            Return identificacion
                        End If
                    Else
                        usuarioValido = 1
                        identificacion = .SqlParametros("@identificacion").Value.ToString
                        Return identificacion
                    End If
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return identificacion

    End Function

    Private Function ValidarPrimerIngreso(idUsuario As Integer)
        Dim dbManager As New LMDataAccess
        Dim resultado As Byte = 1
        Try
            With dbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Int, 10).Value = idUsuario
                .SqlParametros.Add("@resultado", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                .EjecutarNonQuery("SP_ValidarAccesoPrimeraVez", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultado = CByte(.SqlParametros("@resultado").Value.ToString)
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function ValidarcantidadIngresos(ByVal usuario As String) As Integer
        Dim dbManager As New LMDataAccess
        Dim resultado As Integer = 0
        Try
            With dbManager
                .SqlParametros.Add("@usuario", SqlDbType.VarChar, 50).Value = usuario
                '.SqlParametros.Add("@password", SqlDbType.VarChar, 50).Value = password
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                .EjecutarNonQuery("SP_ValidarNumeroIngresos", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultado = .SqlParametros("@resultado").Value
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()

        End Try
        Return resultado
    End Function

    Public Function BolqueoDeUsuarioPorIntentosFallidos(ByVal usuario As String) As Byte
        Dim dbManager As New LMDataAccess
        Dim resultados As Byte = 1
        Try
            With dbManager
                .SqlParametros.Add("@usuario", SqlDbType.VarChar, 50).Value = usuario
                .SqlParametros.Add("@resultado", SqlDbType.BigInt).Direction = ParameterDirection.Output
                .EjecutarNonQuery("SP_BloquearUsuario", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultados = .SqlParametros("@resultado").Value
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultados
    End Function

    Public Function NotificacionUsuarioBloqueoPorIntentosFallidos(ByVal usuario As String) As Byte
        Dim resultado As Boolean = False
        Dim direccionPara As New MailAddressCollection
        Dim correo As New AdministradorCorreo
        Dim destinatario As String

        Try
            destinatario = ObtenerUsuarioNotificacion(usuario)
            correo.Receptor.Add(destinatario)
            With correo
                .Titulo = "Bloqueo Usuario Notus"
                .Asunto = "Bloqueo Usuario Notus"
                .Receptor = .Receptor
                .TextoMensaje = "Informo que se han detectado unos movimientos extraños con su usuario por lo que se ha procedido ha realizar el bloqueo del mismo. Por favor utilizar la herramienta para recuperar contraseña ó contactese con el administrador del correo"
                .FirmaMensaje = "Logytech Mobile S.A.S <br />PBX. 57(1) 4395237 Ext 174 - 135"
                resultado = .EnviarMail()
            End With

        Catch ex As Exception

        End Try
        Return resultado
    End Function

    Public Function ObtenerUsuarioNotificacion(ByVal usuario As String) As String
        Dim dbManager As New LMDataAccess
        Dim resultado As Boolean
        Dim email As String = ""
        Try
            With dbManager
                .SqlParametros.Add("@usuario", SqlDbType.VarChar, 50).Value = usuario
                .SqlParametros.Add("@resultado", SqlDbType.BigInt).Direction = ParameterDirection.Output
                .SqlParametros.Add("@email", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                .EjecutarNonQuery("SP_ObtenerUsuarioNotificacion", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@resultado").Value.ToString) Then
                    resultado = .SqlParametros("@resultado").Value
                    email = .SqlParametros("@email").Value.ToString
                End If
            End With
        Finally

        End Try
        Return email
    End Function
End Class
