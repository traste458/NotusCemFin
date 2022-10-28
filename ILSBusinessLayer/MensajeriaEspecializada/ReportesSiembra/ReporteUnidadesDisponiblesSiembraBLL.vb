Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class ReporteUnidadesDisponiblesSiembraBLL

#Region "Atributos (Filtros de Busqueda)"

        Dim _idCiudad As Integer

#End Region

#Region "Propiedades"

        Public Property IdCiudad() As Integer
            Get
                Return _idCiudad
            End Get
            Set(ByVal value As Integer)
                _idCiudad = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function ObtenerReporte() As DataTable
            Dim dtReporte As New DataTable

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
                        dtReporte = .EjecutarDataTable("ReporteUnidadesDisponiblesSiembra", CommandType.StoredProcedure)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dtReporte
        End Function

#End Region

    End Class

End Namespace
