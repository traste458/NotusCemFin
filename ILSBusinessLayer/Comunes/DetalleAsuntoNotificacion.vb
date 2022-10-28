Imports LMDataAccessLayer

Namespace Comunes

    Public Class DetalleAsuntoNotificacion

#Region "Propiedades"

        Public Property IdDetalle As Integer
        Public Property IdAsuntoNotificacion As Integer
        Public Property Seccion As String
        Public Property Mensaje As String

        Private Property Registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idDetalle As Integer)
            Me.New()
            Me.IdDetalle = idDetalle
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If Me.IdDetalle > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = Me.IdDetalle
                        .ejecutarReader("ObtenerDetalleAsuntoNotificacion", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing And .Reader.Read Then
                            CargarResultadoConsulta(.Reader)
                            Me.Registrado = True
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Integer.TryParse(reader("idDetalle"), Me.IdDetalle)
                    Integer.TryParse(reader("idAsuntoNotificacion"), Me.IdAsuntoNotificacion)
                    Me.Seccion = reader("seccion").ToString
                    Me.Mensaje = reader("mensaje").ToString
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
