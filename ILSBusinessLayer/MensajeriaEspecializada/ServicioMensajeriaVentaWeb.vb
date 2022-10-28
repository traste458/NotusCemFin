Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class ServicioMensajeriaVentaWeb
        Inherits ServicioMensajeria

#Region "Constructores"

        ''' <summary>
        ''' Sobrecarga del constructor con el número de radicado
        ''' </summary>
        ''' <param name="numeroRadicado"></param>
        ''' <remarks>
        ''' Instanciar este constructor de la siguiente forma:
        ''' Dim XXX As Long = 12345
        ''' Dim x = new ServicioMensajeria(numeroRadicado:=XXX)
        ''' 
        ''' </remarks>
        Public Sub New(ByVal numeroRadicado As Long)
            MyBase.New()
            _numeroRadicado = numeroRadicado
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Legalizar(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Using dbManager As New LMDataAccess
                If _idServicioMensajeria > 0 Then
                    With dbManager
                        .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = _idServicioMensajeria
                        .SqlParametros.Add("@idUsuarioLegaliza", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("RegistrarLegalizacionVentaWeb", CommandType.StoredProcedure)
                        Integer.TryParse(.SqlParametros("@respuesta").Value, resultado.Valor)

                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                            resultado.Mensaje = "Se realizó la legalización del servicio exitosamente."
                        Else
                            .abortarTransaccion()
                            resultado.Mensaje = "No se logró realizar la lagalización del servicio: [" & resultado.Valor & "]"
                        End If
                    End With
                End If
            End Using
            Return resultado
        End Function

#End Region

    End Class

End Namespace
