Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class ReporteMaterialesSiembraBLL

#Region "Atributos (Filtros de Busqueda)"

        Dim _fechaInicial As DateTime
        Dim _fechaFinal As DateTime

#End Region

#Region "Propiedades"

        Public Property FechaInicial() As DateTime
            Get
                Return _fechaInicial
            End Get
            Set(ByVal value As DateTime)
                _fechaInicial = value
            End Set
        End Property

        Public Property FechaFinal() As DateTime
            Get
                Return _fechaFinal
            End Get
            Set(ByVal value As DateTime)
                _fechaFinal = value
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
            Dim dtReporte As DataTable

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        If _fechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.SmallDateTime).Value = _fechaInicial
                        If _fechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.SmallDateTime).Value = _fechaFinal

                        dtReporte = .ejecutarDataTable("ReporteMaterialesServicioSiembra", CommandType.StoredProcedure)
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

