Imports LMDataAccessLayer

Public Class NovedadDeclaracion

#Region "Atributos."

    Private _novedad As String
    Private _declaracion As String
    Private _idUsuario As Integer

#End Region

#Region "Propiedades"

    Public Property Novedad() As String
        Get
            Return _novedad
        End Get
        Set(value As String)
            _novedad = value
        End Set
    End Property

    Public Property Declaracion() As String
        Get
            Return _declaracion
        End Get
        Set(value As String)
            _declaracion = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

#End Region

#Region "Métodos Públicos"

    Public Function RegistrarNovedad() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    .SqlParametros.Add("@novedad", SqlDbType.VarChar).Value = _novedad
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarNovedadDeclaraciones", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                        Else
                            .abortarTransaccion()
                        End If
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                    End If

                End With
            End With
        Catch ex As Exception
            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

End Class
