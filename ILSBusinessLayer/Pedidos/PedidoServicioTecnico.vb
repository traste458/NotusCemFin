Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Productos
Imports ILSBusinessLayer.Comunes
Imports System.Data.SqlClient
Imports LMDataAccessLayer

Namespace Pedidos
    Public Class PedidoServicioTecnico
        Inherits Pedido

#Region "Atributos"
        Private _remision As String
        Private _usuario As String
#End Region

#Region "Propiedades"
        Public Property Remision() As String
            Get
                Return _remision
            End Get
            Set(value As String)
                _remision = value
            End Set
        End Property

        Public Property UsuarioServicioTecnico() As String
            Get
                Return _usuario
            End Get
            Set(value As String)
                _usuario = value
            End Set
        End Property
#End Region

#Region "Metodos Publicos"

        Public Function ConfirmarPedidoServicioTecnico() As ResultadoProceso
            Dim resultado As New ILSBusinessLayer.ResultadoProceso(-1, "Recepción no confirmada")
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idPedido", SqlDbType.Int).Value = _idPedido
                        .Add("@usuario", SqlDbType.VarChar).Value = _usuario
                        .Add("@remision", SqlDbType.VarChar).Value = _remision
                        .Add("@observacion", SqlDbType.VarChar).Value = _observaciones
                        .Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("ConfirmarRecepcionPedidoServicioTecnico", CommandType.StoredProcedure)
                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        If resultado.Valor = 0 Then
                            resultado.EstablecerMensajeYValor(resultado.Valor, resultado.Mensaje)
                            If .estadoTransaccional Then .confirmarTransaccion()
                            Return resultado
                        Else
                            resultado.EstablecerMensajeYValor(resultado.Valor, resultado.Mensaje)
                            If .estadoTransaccional Then .abortarTransaccion()
                            Return resultado
                        End If
                    Else
                        If .estadoTransaccional Then .abortarTransaccion()
                        resultado.Valor = -1
                        resultado.Mensaje = "No se pudo evaluar el resultado arrojado por la base de  datos. Por favor intente nuevamente."
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Function RechazarPedidoServicioTecnico() As ResultadoProceso
            Dim resultado As New ILSBusinessLayer.ResultadoProceso(-1, "Recepción rechazada")
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idPedido", SqlDbType.Int).Value = _idPedido
                        .Add("@usuario", SqlDbType.VarChar).Value = _usuario
                        .Add("@remision", SqlDbType.VarChar).Value = _remision
                        .Add("@observacion", SqlDbType.VarChar).Value = _observaciones
                        .Add("@mensaje", SqlDbType.VarChar, 400).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                    End With
                    .iniciarTransaccion()
                    .ejecutarNonQuery("RechazarRecepcionPedidoServicioTecnico", CommandType.StoredProcedure)
                    resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                    If Long.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.Valor) Then
                        If resultado.Valor = 0 Then
                            resultado.EstablecerMensajeYValor(resultado.Valor, resultado.Mensaje)
                            If .estadoTransaccional Then .confirmarTransaccion()
                            Return resultado
                        Else
                            resultado.EstablecerMensajeYValor(resultado.Valor, resultado.Mensaje)
                            If .estadoTransaccional Then .abortarTransaccion()
                            Return resultado
                        End If
                    Else
                        If .estadoTransaccional Then .abortarTransaccion()
                        resultado.Valor = -1
                        resultado.Mensaje = "No se pudo evaluar el resultado arrojado por la base de  datos. Por favor intente nuevamente."
                    End If
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

#End Region

    End Class
End Namespace