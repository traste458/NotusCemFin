Imports System.Text

Public Class CreacionContrasenaRandom
    Public Function CreacionContrasenaRandom() As String
        Dim linea As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmneopqrs"
        Dim random As New Random
        Dim sb As New StringBuilder
        For variabel As Integer = 1 To 32
            Dim idx As Integer = random.Next(0, 42)
            sb.Append(linea.Substring(idx, 1))
        Next
        Return sb.ToString()
    End Function
End Class
