Imports System.Text.RegularExpressions
Imports EncryptionClassLibrary.LMEncryption

Public Class ValidacionContrasena
    Public Function validacionContrasena(ByVal pwd As String, ByVal idUsuario As Integer) As String
        Dim dtPoliticasContrasena As DataTable
        Dim politicasContrasena As New ObtenerPolicticasContrasena
        Dim mayuscula As New Regex("[A-Z]")
        Dim minuscula As New Regex("[a-z]")
        Dim numeros As New Regex("[0-9]")
        Dim especiales As New Regex("[^a-zA-Z0-9]")
        Dim encriptarContrasena As New EncryptionLibrary

        'dtPoliticasContrasena = politicasContrasena.ObtenerRestriccionesContrasena()

        Dim minimaLongitud As Comunes.ConfigValues = New Comunes.ConfigValues("minLongitud")
        Dim numeroMayusculas As Comunes.ConfigValues = New Comunes.ConfigValues("numMayusculas")
        Dim numeroMinusculas As Comunes.ConfigValues = New Comunes.ConfigValues("numMinusculas")
        Dim numNumeros As Comunes.ConfigValues = New Comunes.ConfigValues("numNumeros")
        Dim numerosEspeciales As Comunes.ConfigValues = New Comunes.ConfigValues("numEspeciales")
        Dim NumValidacionUltimasContrasenas As Comunes.ConfigValues = New Comunes.ConfigValues("NumValidacionUltimasContrasenas")

        With politicasContrasena

            .MinLongitud = minimaLongitud.ConfigKeyValue
            .NumMayusculas = numeroMayusculas.ConfigKeyValue
            .NumMinusculas = numeroMinusculas.ConfigKeyValue
            .NumNumeros = numNumeros.ConfigKeyValue
            .NumEspeciales = numerosEspeciales.ConfigKeyValue
            .ValidacionUltimasContrasena = NumValidacionUltimasContrasenas.ConfigKeyValue

            If politicasContrasena.ValidarCantrasenaUltimosIngresos(idUsuario, EncryptionData.getMD5Hash(pwd)) Then Return String.Concat("La contraseña no puede coincidir con las útimas ", .ValidacionUltimasContrasena.ToString(), " contraseñas")
            If Len(pwd) < .MinLongitud Then Return String.Concat("La longitud de la nueva contraseña debe contener como mínimo ", .MinLongitud.ToString(), " caracteres")
            If mayuscula.Matches(pwd).Count < .NumMayusculas Then Return String.Concat("La contraseña debe contener como mínimo ", .NumMayusculas.ToString(), " mayuscula")
            If minuscula.Matches(pwd).Count < .NumMinusculas Then Return String.Concat("La contraseña debe contener como mínimo ", .NumMinusculas.ToString(), " minuscula")
            If numeros.Matches(pwd).Count < .NumNumeros Then Return String.Concat("La contraseña debe contener como mínimo ", .NumNumeros.ToString(), " número")
            If especiales.Matches(pwd).Count < .NumEspeciales Then Return String.Concat("La contraseña debe contener como mínimo ", .NumEspeciales.ToString(), " caracteres especiales")
        End With

        Return ""
    End Function
End Class
