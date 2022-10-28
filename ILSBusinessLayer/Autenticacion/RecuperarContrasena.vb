
Imports System.Net.Mail
Imports System.Security.Claims
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Public Class RecuperarContrasena
    Public Function recuperacionContrasena(ByVal identificacion As String) As Integer
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

        Dim esEmailValido As Boolean = validar_Mail(resultadoConsulta.Item(1))

        If esEmailValido = False Then
            Return 6
        End If

        If resultadoConsulta.Item(0) <> "0" Then
            With recuperarContrasenaBD
                EnviarCorreoRecuperacionContrasena(resultadoConsulta.Item(1), resultadoConsulta.Item(0), resultadoConsulta.Item(2))
            End With
            Return 1
        Else
            Return 0
        End If

    End Function

    Private Function EnviarCorreoRecuperacionContrasena(ByVal destinatario As String, ByVal usuario As String, ByVal token As String)
        Dim direccionPara As New MailAddressCollection
        Dim resultadoEnviado As Boolean = False
        Dim sb As New StringBuilder
        Dim correo As New AdministradorCorreo
        correo.Receptor.Add(destinatario)
        Dim urlRecuperacion As Comunes.ConfigValues = New Comunes.ConfigValues("URL_RECUPERACION_CONTRASENA")
        Try
            With correo
                .Titulo = "Recuperación contraseña Logytech Mobile"
                .Asunto = "Recuperación contraseña Logytech Mobile"
                .Receptor = .Receptor
                .TextoMensaje = String.Concat("Hola: ", usuario, vbCrLf, ", Por favor ingresar al link de abajo para recuperar la contraseña: ", vbCrLf, "</br><a style='margin:10px 0 10px 0;color:#ffffff;font-weight:bold;display:inline-block;padding:6px 10px;font-size:16px;text-align:center;background-image:none;border:1px solid transparent;border-radius:10px;-moz-border-radius:10px;-webkit-border-radius:10px;-khtml-border-radius:10px; background-color:#836493;' href='" & urlRecuperacion.ConfigKeyValue.ToString & token & "'> RecuperarContraseña </a></br>", vbCrLf, "Si no solicitaste recuperar la contraseña, por favor ignora este email y tu contraseña seguirá siendo la misma.")
                .FirmaMensaje = "Logytech Mobile S.A.S <br />PBX. 57(1) 4395237 Ext 174 - 135"
                resultadoEnviado = .EnviarMail()
            End With
        Finally
        End Try
        Return resultadoEnviado
    End Function


    Private Function validar_Mail(ByVal sMail As String) As Boolean
        ' retorna true o false   
        Return Regex.IsMatch(sMail,
                  "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
    End Function
End Class
