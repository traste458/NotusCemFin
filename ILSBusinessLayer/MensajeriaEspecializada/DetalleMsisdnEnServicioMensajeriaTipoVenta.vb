Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class DetalleMsisdnEnServicioMensajeriaTipoVenta
    Inherits DetalleMsisdnEnServicioMensajeria

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idRegistro As Integer)
        MyBase.New(idRegistro)
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Overloads Function Modificar() As ResultadoProceso
        Dim resultado As New ResultadoProceso()
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = _idRegistro
                If _msisdn > 0 Then .SqlParametros.Add("@msisdn", SqlDbType.BigInt).Value = _msisdn
                If Not String.IsNullOrEmpty(_numeroReserva) Then .SqlParametros.Add("@numeroReserva", SqlDbType.VarChar).Value = _numeroReserva
                .SqlParametros.Add("@activaEquipoAnterior", SqlDbType.Bit).Value = _activaEquipoAnterior
                .SqlParametros.Add("@comSeguro", SqlDbType.Bit).Value = _comseguro
                If _precioConIva > 0 Then .SqlParametros.Add("@precioConIVA", SqlDbType.Money).Value = _precioConIva
                If _precioSinIva > 0 Then .SqlParametros.Add("@precioSinIVA", SqlDbType.Money).Value = _precioSinIva
                If _idClausula > 0 Then .SqlParametros.Add("@idClausula", SqlDbType.Int).Value = _idClausula
                If _idRegion > 0 Then .SqlParametros.Add("@idRegion", SqlDbType.SmallInt).Value = _idRegion

                .iniciarTransaccion()
                .ejecutarNonQuery("ModificarMsisdnServicioMensajeria", CommandType.StoredProcedure)

                'Se realiza la actualización del precio 
                Dim respuestaPrecio As Integer = -1
                .SqlParametros.Clear()
                .SqlParametros.Add("@idServicioMensajeria", SqlDbType.Int).Value = IdServicioMensajeria
                .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                .ejecutarScalar("RegistraPrecioServicioVenta", CommandType.StoredProcedure)
                Integer.TryParse(.SqlParametros("@respuesta").Value.ToString(), respuestaPrecio)
                If respuestaPrecio <> 0 Then
                    .abortarTransaccion()
                    resultado.EstablecerMensajeYValor(respuestaPrecio, "No se logro actualizar el precio del servicio")
                End If

                resultado.EstablecerMensajeYValor(0, "Modificación exitosa.")
                .confirmarTransaccion()
            Catch ex As Exception
                .abortarTransaccion()
                Throw ex
            End Try
        End With
        dbManager.Dispose()
        Return resultado
    End Function

#End Region

End Class
