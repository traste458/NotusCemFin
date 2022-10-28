Imports NotusExpressBusinessLayer.General
Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes

Public Class ConectorPresenceDB

    Private _cadenaConexion As String

    Public Sub New()

        Dim cv As New ConfigValues("DB_PRESENCE")
        _cadenaConexion = cv.ConfigKeyValue
        If String.IsNullOrWhiteSpace(_cadenaConexion) Then Throw New Exception("No existe la cadena de Conexion a la BD de Presence. Por favor reportar a IT Development")

    End Sub

    Public Property CadenaConexion() As String
        Get
            Return _cadenaConexion
        End Get
        Set(ByVal value As String)
            _cadenaConexion = value
        End Set
    End Property
    Public Function ObtenerListaTipificacionesDesdePresence(Optional ByVal idServicio As Integer = 0) As DataTable
        Dim dt As DataTable

        Using dbManager As LMDataAccess = New LMDataAccess(_cadenaConexion)
            With dbManager

                If idServicio > 0 Then .SqlParametros.AddWithValue("@idServicio", idServicio)
                dt = .EjecutarDataTable("dbo.ObtenerListadoTipificaciones", CommandType.StoredProcedure)

            End With
        End Using

        Return dt
    End Function

    Public Function ObtenerResultadoGestionDeCargaDeServicio(dtCarga As DataTable) As DataTable
        Dim dt As DataTable

        Using dbManager As LMDataAccess = New LMDataAccess(_cadenaConexion)
            With dbManager

                .SqlParametros.AddWithValue("@TablaCargasDeServicio", dtCarga)
                dt = .EjecutarDataTable("dbo.ObtenerResultadoGestionDeCargaDeServicio", CommandType.StoredProcedure)

            End With
        End Using

        Return dt
    End Function



End Class
