

Imports LMDataAccessLayer

Public Class RecuperacionContrasena


    Public Property ContrasenaActual As String
    Public Property IdUsuario As Decimal


    Public Function AlmacenarRamdomContrasena(ByVal identificacion As String, ByVal contrasena As String) As List(Of String)
        Dim dbManager As New LMDataAccess
        Dim resultado As New List(Of String)
        Try
            With dbManager
                .SqlParametros.Add("@identificacion", SqlDbType.VarChar, 50).Value = identificacion
                .SqlParametros.Add("@pwd", SqlDbType.VarChar, 50).Value = contrasena
                .SqlParametros.Add("@usuario", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                .SqlParametros.Add("@email", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                .SqlParametros.Add("@claveEncriptada", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .EjecutarNonQuery("SP_CambioContrasenaRandom", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultado.Add(.SqlParametros("@usuario").Value.ToString())
                    resultado.Add(.SqlParametros("@email").Value.ToString())
                    resultado.Add(.SqlParametros("@claveEncriptada").Value.ToString())
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function ValidarContrasena(token As String) As Integer
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim res As Integer
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@token", SqlDbType.VarChar, 100).Value = token
                    .Add("@contrasenaActual", SqlDbType.VarChar, 40).Direction = ParameterDirection.Output
                    .Add("@idUsuario", SqlDbType.Decimal).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("ValidarTokenUsuario", CommandType.StoredProcedure)

                res = CInt(.SqlParametros("@result").Value.ToString)

                If res = 1 Then
                    ContrasenaActual = .SqlParametros("@contrasenaActual").Value.ToString
                    IdUsuario = CDec(.SqlParametros("@idUsuario").Value.ToString)
                End If

            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return res

    End Function



End Class
