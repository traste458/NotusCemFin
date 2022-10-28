Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class TipoNovedad

#Region "Propiedades"

        Public Property IdTipoNovedad As Integer
        Public Property IdProceso As Integer
        Public Property Descripcion As String
        Public Property Proceso As String
        Public Property Activo As Boolean
        Public Property Gestionable As Boolean
        Public Property IdTipoServicio As Integer

        Private Property Registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(idTipoNovedad As Integer)
            Me.New()
            Me.IdTipoNovedad = idTipoNovedad
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me.IdTipoNovedad > 0 Then .SqlParametros.Add("@idTipoNovedad", SqlDbType.Int).Value = Me.IdTipoNovedad

                        .ejecutarReader("ObtenerTiposNovedadMensajeriaEspecializada", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                CargarResultadoConsulta(.Reader)
                                Me.Registrado = True
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

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idTipoNovedad"), Me.IdTipoNovedad)
                    Integer.TryParse(reader("idProceso"), Me.IdProceso)
                    Me.Descripcion = reader("descripcion")
                    Me.Proceso = reader("proceso")
                    Me.IdProceso = CBool(reader("idProceso"))
                    Boolean.TryParse(reader("gestionable"), Me.Gestionable)
                    Boolean.TryParse(reader("activo"), Me.Activo)
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
