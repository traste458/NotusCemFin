Imports LMDataAccessLayer

Namespace Comunes

    Public Class ArchivoAyuda

#Region "Propiedades"

        Public Property IdArchivo As Integer
        Public Property Nombre As String
        Public Property Ruta As String

        Private Property Registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.new()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If Me.IdArchivo > 0 Then .SqlParametros.Add("@idArchivo", SqlDbType.Int).Value = Me.IdArchivo
                        .ejecutarReader("ObtenerArchivoAyuda", CommandType.StoredProcedure)
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
                    Integer.TryParse(reader("idArchivo").ToString, Me.IdArchivo)
                    Me.Nombre = reader("nombre").ToString
                    Me.Ruta = reader("ruta").ToString
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
