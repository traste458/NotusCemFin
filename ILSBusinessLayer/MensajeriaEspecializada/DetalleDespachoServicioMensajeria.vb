Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class DetalleDespachoServicioMensajeria

#Region "Propiedades"

        Public Property IdDetalle As Long
        Public Property IdRuta As Long
        Public Property IdDetalleSerial As Long
        Public Property IdServicio As Long
        Public Property FechaModificacion As Date
        Public Property IdUsuarioLog As Integer

        Private Property Registrado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(idDetalle As Long)
            Me.New()
            Me.IdDetalle = idDetalle
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            If Me.IdDetalle > 0 Then
                Using dbManager As New LMDataAccess
                    Try
                        With dbManager
                            If Me.IdDetalle > 0 Then .SqlParametros.Add("@idDetalle", SqlDbType.Int).Value = Me.IdDetalle
                            .ejecutarReader("ObtenerInfoDetalleDespachoServicio", CommandType.StoredProcedure)
                            If .Reader IsNot Nothing Then
                                If .Reader.Read Then CargarResultadoConsulta(.Reader)
                                If Not dbManager.Reader.IsClosed Then dbManager.Reader.Close()
                            End If
                        End With
                    Catch ex As Exception
                        Throw ex
                    End Try
                End Using
            End If
        End Sub

#End Region

#Region "Métodos Protegidos"

        Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
            If reader IsNot Nothing Then
                If reader.HasRows Then
                    Long.TryParse(reader("idDetalle").ToString, Me.IdDetalle)
                    Long.TryParse(reader("idRuta").ToString, Me.IdRuta)
                    Long.TryParse(reader("idDetalleSerial").ToString, Me.IdDetalleSerial)
                    Long.TryParse(reader("idServicio").ToString, Me.IdServicio)
                    
                    Me.Registrado = True
                End If
            End If

        End Sub

#End Region

    End Class

End Namespace
