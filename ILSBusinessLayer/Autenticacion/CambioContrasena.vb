

Imports LMDataAccessLayer

Public Class CambioContrasena

#Region "Atributos"
    Private _idUsuario As Integer
    Private _pwd As String
#End Region


#Region "Propiedades"
    Public Property idUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property Pwd() As String
        Get
            Return _pwd
        End Get
        Set(ByVal value As String)
            _pwd = value
        End Set
    End Property
#End Region


    Public Function ValidarUsuarioCambiar(idUsuario As Integer, pwd As String)
        Dim dbManager As New LMDataAccess
        Dim resultado As Byte = 1
        Try
            With dbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Int, 10).Value = idUsuario
                .SqlParametros.Add("@pwd", SqlDbType.VarChar, 50).Value = pwd
                .SqlParametros.Add("@resultado", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                .EjecutarNonQuery("SP_ValidarContrasenaACambiar", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultado = CByte(.SqlParametros("@resultado").Value.ToString)
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return resultado
    End Function

    Public Function CambioContrasena(idUsuario As Integer, pwd As String) As String
        Dim DbManager As New LMDataAccess
        Dim resultado As Byte = 1
        Dim identificacion As String = ""
        Try
            With DbManager
                .SqlParametros.Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                .SqlParametros.Add("@pwd", SqlDbType.VarChar, 50).Value = pwd
                .SqlParametros.Add("@resultado", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                .SqlParametros.Add("@identificacion", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                .EjecutarNonQuery("SP_CambioContrasena", CommandType.StoredProcedure)
                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultado = CByte(.SqlParametros("@resultado").Value.ToString)
                    identificacion = .SqlParametros("@identificacion").Value.ToString
                End If
            End With
        Finally
            If DbManager IsNot Nothing Then DbManager.Dispose()
        End Try
        Return identificacion
    End Function

End Class
