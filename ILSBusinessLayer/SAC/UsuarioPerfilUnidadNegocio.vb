Imports LMDataAccessLayer

Namespace SAC
    Public Class UsuarioPerfilUnidadNegocio

#Region "Atributos"

        Private _idUsuario As Integer
        Private _idPerfilUnidad As Integer
        Private _idPerfil As Integer
        Private _idUnidadNegocio As Short

#End Region

#Region "Propiedades"

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Property IdPerfilUnidad() As Integer
            Get
                Return _idPerfilUnidad
            End Get
            Set(ByVal value As Integer)
                _idPerfilUnidad = value
            End Set
        End Property

        Public Property IdPerfil() As Integer
            Get
                Return _idPerfil
            End Get
            Set(ByVal value As Integer)
                _idPerfil = value
            End Set
        End Property

        Public Property IdUnidadNegocio() As Short
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Short)
                _idUnidadNegocio = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New(ByVal idPerfil As Integer)
            MyBase.New()
            CargarDatos(idPerfil)
        End Sub

#End Region

#Region "Metodos Privados"

        Private Sub CargarDatos(ByVal idPerfil As Integer)
            Dim db As New LMDataAccess
            Try
                With db
                    .SqlParametros.Add("@idPerfil", SqlDbType.Int).Value = idPerfil
                    .ejecutarReader("ConsultarUsuarioUnidadNegocio", CommandType.StoredProcedure)
                    If Not .Reader Is Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idUsuario").ToString(), _idUsuario)
                            Integer.TryParse(.Reader("idPerfilUnidad").ToString(), _idPerfilUnidad)
                            Integer.TryParse(.Reader("idPerfil"), _idPerfil)
                            Short.TryParse(.Reader("idUnidadNegocio").ToString(), _idUnidadNegocio)
                        End If
                        .Reader.Close()
                    End If
                End With
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

#End Region

#Region "Metodos Protegidos"

#End Region

#Region "Metodos Publicos"

#End Region

    End Class
End Namespace
