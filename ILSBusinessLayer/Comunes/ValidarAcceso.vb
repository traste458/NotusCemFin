Imports LMDataAccessLayer
Imports EncryptionClassLibrary

Public Class ValidarAcceso

#Region "Atributos"

    Private _idUsuario As Integer
    Private _login As String
    Private _password As String

#End Region

#Region "Propiedades"

    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property Login As String
        Get
            Return _login
        End Get
        Set(value As String)
            _login = value
        End Set
    End Property

    Public Property Password As String
        Get
            Return _password
        End Get
        Set(value As String)
            _password = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal login As String, clave As String)
        MyBase.New()
        _login = login
        _password = clave
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function EsUsuarioValido() As Boolean
        Dim resultado As Boolean
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@usuario", SqlDbType.VarChar, 100).Value = _login
                    .SqlParametros.Add("@password", SqlDbType.VarChar, 100).Value = LMEncryption.EncryptionData.getMD5Hash(_password)
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@idLinea", SqlDbType.Int).Direction = ParameterDirection.Output
                    .SqlParametros.Add("@idPerfil", SqlDbType.Int).Direction = ParameterDirection.Output

                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .ejecutarNonQuery("AutenticarUsuarioLinea", CommandType.StoredProcedure)

                    If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                        resultado = CByte(.SqlParametros("@resultado").Value)
                        If resultado Then
                            Integer.TryParse(.SqlParametros("@idUsuario").Value, _idUsuario)
                        End If
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return resultado
    End Function

#End Region

End Class
