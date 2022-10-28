Imports LMDataAccessLayer
Imports System.String

Public Class GrupodeDevolucion

#Region "Atributos"

    Private _idGrupoDevolucion As Integer
    Private _idGrupo As Integer
    Private _idUsuario As Integer
    Private _nombre As String
    Private _idTipoDevolucion As Integer
    Private _activo As Integer

#End Region

#Region "Propiedades"

    Public Property IdGrupoDevolucion As Integer
        Get
            Return _idGrupoDevolucion
        End Get
        Set(value As Integer)
            _idGrupoDevolucion = value
        End Set
    End Property
    Public Property IdGrupo As Integer
        Get
            Return _idGrupo
        End Get
        Set(value As Integer)
            _idGrupo = value
        End Set
    End Property

    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property IdTipoDevolucion As Integer
        Get
            Return _idTipoDevolucion
        End Get
        Set(value As Integer)
            _idTipoDevolucion = value
        End Set
    End Property

    Public Property NombreGupoDevolucion As String
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

    Public Sub New(ByVal idGrupoDevolucion As Integer)
        MyBase.New()
        _idGrupoDevolucion = idGrupoDevolucion
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    If _idGrupoDevolucion > 0 Then .SqlParametros.Add("@idGrupoDevolucion", SqlDbType.Int).Value = _idGrupoDevolucion
                    If _activo > 0 Then .SqlParametros.Add("@activo", SqlDbType.Int).Value = _activo

                    .ejecutarReader("ObtenerGruposDevolucion", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idgrupo_devolucion").ToString, _idGrupoDevolucion)
                            _nombre = .Reader("idgrupo_devolucion2").ToString
                            Integer.TryParse(.Reader("idgrupo").ToString, _idGrupo)
                            Integer.TryParse(.Reader("idTipoDevolucion").ToString, _idTipoDevolucion)
                            _activo = .Reader("estado")
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
