Imports LMDataAccessLayer

Namespace MensajeriaEspecializada

    Public Class ReporteIndicadoresSiembraBLL

#Region "Atributos (Filtros de Busqueda)"

        Dim _fechaInicial As DateTime
        Dim _fechaFinal As DateTime
        Dim _noInventario As Long

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

        Public Property NoInventario As Long
            Get
                Return _noInventario
            End Get
            Set(value As Long)
                _noInventario = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function ObtenerReporte() As DataSet
            Dim dsReporte As DataSet

            Using dbManager As New LMDataAccess
                Try
                    With dbManager
                        .TiempoEsperaComando = 600
                        If _fechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.SmallDateTime).Value = _fechaInicial
                        If _fechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.SmallDateTime).Value = _fechaFinal
                        .SqlParametros.Add("@nInventarioSiembra", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        dsReporte = .EjecutarDataSet("ReporteIndicadoresGestionSiembra", CommandType.StoredProcedure)

                        Long.TryParse(.SqlParametros("@nInventarioSiembra").Value, _noInventario)
                    End With
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return dsReporte
        End Function

#End Region

    End Class

End Namespace
