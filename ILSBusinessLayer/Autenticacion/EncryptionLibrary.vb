

Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class EncryptionLibrary
    Public Function Encriptar(textoConversion As String)

        Dim llaveEncripcion As String = Convert.ToString(ConfigurationSettings.AppSettings("llaveEncripcion"))
        Dim LimpiarBit As Byte() = Encoding.Unicode.GetBytes(textoConversion)
        Using encriptor As Aes = Aes.Create()
            Dim pwd As New Rfc2898DeriveBytes(llaveEncripcion, New Byte() {&H49, &H76, &H61, &H6E, &H20, &H4D, &H65, &H64, &H76, &H65, &H64, &H65, &H76})
            encriptor.Key = pwd.GetBytes(32)
            encriptor.IV = pwd.GetBytes(16)
            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encriptor.CreateEncryptor(), CryptoStreamMode.Write)
                    cs.Write(LimpiarBit, 0, LimpiarBit.Length)
                    cs.Close()
                End Using
                textoConversion = Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
        Return textoConversion
    End Function

    Public Function CrearHash(ByVal textoHash As String) As String
        Dim UE As New UnicodeEncoding
        Dim Hash As Byte()
        Dim bCadena() As Byte = UE.GetBytes(textoHash)
        Dim hashServicio As New SHA1CryptoServiceProvider
        Hash = hashServicio.ComputeHash(bCadena)
        Dim cadenaFinal As String
        cadenaFinal = Convert.ToBase64String(Hash)
        Return cadenaFinal
    End Function
End Class
