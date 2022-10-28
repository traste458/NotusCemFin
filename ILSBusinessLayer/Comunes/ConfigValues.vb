Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Comunes

    Public Class ConfigValues

#Region "Atributos (Campos)"

        Private _idConfig As Integer
        Private _configKeyName As String
        Private _configKeyValue As String

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal configKeyName As String)
            MyBase.New()
            _configKeyName = configKeyName
            CargarInformacion()
        End Sub

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdConfig() As Integer
            Get
                Return _idConfig
            End Get
        End Property

        Public Property ConfigKeyName() As String
            Get
                Return _configKeyName
            End Get
            Set(ByVal value As String)
                _configKeyName = value
            End Set
        End Property

        Public Property ConfigKeyValue() As String
            Get
                Return _configKeyValue
            End Get
            Set(ByVal value As String)
                _configKeyValue = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _configKeyName IsNot Nothing AndAlso _configKeyName.Trim.Length > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@configKeyName", SqlDbType.VarChar, 50).Value = _configKeyName
                        .ejecutarReader("ObtenerInfoConfigValues", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                _configKeyValue = .Reader("configKeyValue").ToString
                                Integer.TryParse(.Reader("idConfig").ToString, _idConfig)
                            End If
                            .Reader.Close()
                        End If

                        If Not .Reader.IsClosed Then .Reader.Close()
                    End With
                Catch ex As Exception
                    Throw New Exception(ex.Message)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroConfigValues
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroConfigValues) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.ConfigKeyName > 0 Then .Add("@configKeyName", SqlDbType.Int).Value = filtro.ConfigKeyName
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoConfigValues", CommandType.StoredProcedure)

                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return dtDatos
        End Function

        Public Shared Function seleccionarConfigValue(ByVal configKeyName As String) As String
            Dim db As New LMDataAccessLayer.LMDataAccess
            Dim resultado As String = ""
            Try
                With db
                    .SqlParametros.Add("@configKeyName", SqlDbType.VarChar, 200).Value = configKeyName
                    .ejecutarReader("SELECT configKeyValue FROM ConfigValues WHERE configKeyName=@configKeyName AND status=1")
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            resultado = .Reader("configKeyValue").ToString
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If db IsNot Nothing Then db.Dispose()
            End Try

            Return resultado
        End Function
#End Region

    End Class

End Namespace
