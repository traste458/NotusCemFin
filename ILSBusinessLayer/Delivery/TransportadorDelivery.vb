Imports LMDataAccessLayer

Public Class TransportadorDelivery

#Region "Atributos (Propiedades)"
    Private dbManager As New LMDataAccess
#End Region

#Region "Propiedades"
    Public Property idTercero As String
    Public Property idDelivery As Integer
    Public Property nombreTransportador As String
    Public Property cedulaTransportador As String
    Public Property placaTransportador As String
    Public Property numeroGuia As String
    Public Property idUsuario As String
#End Region

    Public Function AsignarTransportadorDelivery() As ResultadoProceso

        Dim resultado As New ResultadoProceso
        Dim idRuta As Integer = -1

        If dbManager IsNot Nothing Then dbManager = New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    If idTercero <> 0 Then .Add("@id_tercero", SqlDbType.VarChar).Value = idTercero
                    If nombreTransportador <> "" And nombreTransportador IsNot Nothing Then .Add("@nombre_transportador", SqlDbType.VarChar).Value = nombreTransportador
                    If cedulaTransportador <> "" And cedulaTransportador IsNot Nothing Then .Add("@cedula_transportador", SqlDbType.VarChar).Value = cedulaTransportador
                    If placaTransportador <> "" And placaTransportador IsNot Nothing Then .Add("@placa_transportador", SqlDbType.VarChar).Value = placaTransportador
                    If numeroGuia <> "" And numeroGuia IsNot Nothing Then .Add("@numero_guia", SqlDbType.VarChar).Value = numeroGuia
                    If idDelivery <> 0 Then .Add("@id_delivery", SqlDbType.VarChar).Value = idDelivery
                    If idUsuario <> "" And idUsuario IsNot Nothing Then .Add("@usuario_registro", SqlDbType.VarChar).Value = idUsuario
                    .Add("@idRutas", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .EjecutarScalar("AsignarTransportadorDelivery", CommandType.StoredProcedure)
                Integer.TryParse(.SqlParametros("@idRutas").Value.ToString(), idRuta)

                If idRuta > 0 Then
                    .ConfirmarTransaccion()
                    resultado.EstablecerMensajeYValor(idRuta, "Transacción exitosa.")
                Else
                    resultado.EstablecerMensajeYValor(idRuta, "Se generó un error al tratar de actualizar.")
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            End Try
        End With
        dbManager.Dispose()

        Return resultado
    End Function

    Public Function ObtenerInformacionTransportadoras() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            With _dbManager
                .SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = Enumerados.PerfilesDelivery.Transportadora_Delivery
                dtDatos = .EjecutarDataTable("ObtenerInformacionTerceroPerfil", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ObtenerDatosTransportador() As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable
        Try
            With _dbManager
                .SqlParametros.Add("@idTercero", SqlDbType.Int).Value = idTercero
                dtDatos = .EjecutarDataTable("ObtenerDatosTransportadorDelivery", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

End Class
