Imports System
Imports LMDataAccessLayer
Public Class OperacionesBD

#Region "Propiedades"
    Property idOperacion As Integer
    Property Nombre As String
    Property userOperacion As Integer
    Property Descripcion As String
#End Region


#Region "Métodos Publicos"

    Public Function ActualizarEstadoEnvioTransacionPresence(ByVal idUsuario As Integer, ByVal EstadoActual As String, ByVal NuevoEstado As String, ByVal idServicio As Integer, Optional idLoad As Integer = 0) As String
        Dim dbManager As New LMDataAccess
        Dim EstadoActualizado As String
        Try
            With dbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
                .SqlParametros.Add("@idLoad", SqlDbType.Int).Value = idLoad
                .SqlParametros.Add("@EstadoActual", SqlDbType.VarChar).Value = EstadoActual
                .SqlParametros.Add("@NuevoEstado", SqlDbType.VarChar).Value = NuevoEstado
                .SqlParametros.Add("@Estado", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                .EjecutarDataSet("ActualizarEstadoTransacionPresenceLoadsOutbound", CommandType.StoredProcedure)
                EstadoActualizado = .SqlParametros("@Estado").Value.ToString
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return EstadoActualizado
    End Function


#End Region

End Class
