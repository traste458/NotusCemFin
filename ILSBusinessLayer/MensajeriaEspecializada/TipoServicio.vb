Imports LMDataAccessLayer
Imports System.String

Public Class TipoServicio

#Region "Atributos"

    Private _idTipoServicio As Integer
    Private _nombre As String
    Private _activo As Nullable(Of Boolean)

#End Region

#Region "Propiedades"

    Public Property IdTipoServicio As Short
        Get
            Return _idTipoServicio
        End Get
        Set(value As Short)
            _idTipoServicio = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idTipoServicio As Integer)
        MyBase.New()
        _idTipoServicio = IdTipoServicio
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = _idTipoServicio
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo

                    .ejecutarReader("ObtieneTipoServicio", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idTipoServicio").ToString, _idTipoServicio)
                            _nombre = .Reader("nombre")
                            _activo = .Reader("activo")
                        End If
                        .Reader.Close()
                    End If
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

#Region "Métodos Públicos"

#End Region

End Class
