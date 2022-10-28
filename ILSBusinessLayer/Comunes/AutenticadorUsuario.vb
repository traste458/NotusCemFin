Imports LMDataAccessLayer
Public Class AutenticadorUsuario

    Public Shared Function AutenticarCredenciales(usuario As String, pwd As String, Optional ipAcceso As String = Nothing,
                                                  Optional sessionId As String = Nothing) As Usuario
        Dim infoUsuario As Usuario = Nothing

        Using dbManager As New LMDataAccess
            With dbManager
                .SqlParametros.AddWithValue("@usuario", usuario)
                .SqlParametros.AddWithValue("@password", pwd)
                If Not String.IsNullOrEmpty(ipAcceso) Then .SqlParametros.AddWithValue("@ipAcceso", ipAcceso)
                .SqlParametros.AddWithValue("@tipoAplicativo", "NotusIlsWeb")
                .SqlParametros.AddWithValue("@sessionId", sessionId)
                .ejecutarReader("ValidarCredencialesDeUsuario", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        infoUsuario = New Usuario With {
                            .Registrado = True
                        }

                        infoUsuario.IdUsuario = Integer.Parse(.Reader("idTercero").ToString)
                        infoUsuario.Nombre = .Reader("tercero").ToString
                        infoUsuario.Cliente = .Reader("cliente").ToString
                        infoUsuario.Cargo = .Reader("cargo").ToString
                        infoUsuario.Ciudad = .Reader("ciudad").ToString
                        If Not IsDBNull(.Reader("idCargo")) Then infoUsuario.IdCargo = Integer.Parse(.Reader("idCargo").ToString)
                        If Not IsDBNull(.Reader("idCiudad")) Then infoUsuario.IdCiudad = Integer.Parse(.Reader("idCiudad").ToString)
                        If Not IsDBNull(.Reader("idCliente")) Then infoUsuario.IdCliente = Integer.Parse(.Reader("idCliente").ToString)
                        If Not IsDBNull(.Reader("idPerfil")) Then infoUsuario.IdPerfil = Integer.Parse(.Reader("idPerfil").ToString)
                        If Not IsDBNull(.Reader("linea")) Then infoUsuario.Linea = Integer.Parse(.Reader("linea").ToString)
                        If Not IsDBNull(.Reader("idBodega")) Then infoUsuario.IdBodega = Integer.Parse(.Reader("idBodega").ToString)
                        infoUsuario.PoolAplicacion = .Reader("poolAplicacion").ToString
                    End If
                End If
            End With
        End Using

        Return infoUsuario
    End Function

    Public Shared Sub RegistrarLogOutDeUsuario(idUsuario As Integer, sessionId As String)

        Using dbManager As New LMDataAccess
            With dbManager
                .SqlParametros.AddWithValue("@idUsuario", idUsuario)
                .SqlParametros.AddWithValue("@sessionId", sessionId)
                .EjecutarNonQuery("RegistrarLogDeSalidaDeUsuarioDeSistema", CommandType.StoredProcedure)
            End With
        End Using
    End Sub

End Class
