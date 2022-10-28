Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Namespace Comunes

    Public Class ClaseMovimientoStockSAP

#Region "Atributos"

        Private _idClaseMovimiento As Integer
        Private _movimiento As Integer
        Private _stockOrigen As Integer
        Private _stockDestino As Integer

#End Region

#Region "Propiedades"

        Public ReadOnly Property IdClaseMovimiento() As Integer
            Get
                Return _idClaseMovimiento
            End Get
        End Property

        Public Property Movimiento() As Integer
            Get
                Return _movimiento
            End Get
            Set(ByVal value As Integer)
                _movimiento = value
            End Set
        End Property

        Public Property StockOrigen() As Integer
            Get
                Return _stockOrigen
            End Get
            Set(ByVal value As Integer)
                _stockOrigen = value
            End Set
        End Property

        Public Property StockDestino() As Integer
            Get
                Return _stockDestino
            End Get
            Set(ByVal value As Integer)
                _stockDestino = value
            End Set
        End Property

#End Region

#Region "Contructores"

        Public Sub New()
            MyBase.New()
            ObtenerListado()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idClaseMovimiento = identificador
        End Sub

#End Region

#Region "Método Privado"

#End Region

#Region "Método Público"

        Private Function CargarInformacion() As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If _idClaseMovimiento > 0 Then .Add("@idClaseMovimiento", SqlDbType.Int).Value = _idClaseMovimiento
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerClaseMovimientoStockSAP", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function

#End Region

#Region "Método Compartido"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroClaseMovimientoStockSAP
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroClaseMovimientoStockSAP) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdClaseMovimiento > 0 Then .Add("@idClaseMovimiento", SqlDbType.BigInt).Value = filtro.IdClaseMovimiento
                        If filtro.Movimiento > 0 Then .Add("@movimiento", SqlDbType.Int).Value = filtro.Movimiento
                        If filtro.StockOrigen > 0 Then .Add("@stockOrigen", SqlDbType.Int).Value = filtro.StockOrigen
                        If filtro.StockDestino > 0 Then .Add("@stockDestino", SqlDbType.Int).Value = filtro.StockDestino
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerClaseMovimientoStockSAP", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function
#End Region

    End Class

End Namespace