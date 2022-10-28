
Public Class UsuarioCargue

    Private Shared _instancia As UsuarioCargue
    Private _idUsuario As Integer

    Public Sub New()
        MyBase.New()
    End Sub

    Public Shared Function Instancia() As UsuarioCargue
        If (_instancia Is Nothing) Then
            _instancia = New UsuarioCargue()
        End If
        Return _instancia
    End Function

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Shared Function VerificarCarguePendiente() As Integer
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim idUsuario As Integer = 0
        Try
            With db
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Direction = ParameterDirection.Output
                .ejecutarNonQuery("ObtenerCarguesEnProceso", CommandType.StoredProcedure)
                Integer.TryParse(.SqlParametros("@idUsuario").Value.ToString(), idUsuario)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
        Return idUsuario
    End Function


End Class
