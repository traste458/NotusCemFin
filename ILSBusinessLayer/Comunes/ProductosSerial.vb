Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer
Imports LMDataAccessLayer

Namespace Comunes
    Public Class ProductosSerial

#Region "Variables privadas"

#End Region

#Region "Propiedades Publicas"

#End Region

#Region "Estructuras"

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Metodos Privados"

#End Region

#Region "Metodos Publicos"

#End Region

#Region "Metodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New Estructuras.FiltroInfoCargueSAPToken
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As Estructuras.FiltroInfoCargueSAPToken) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable
            With filtro
                If .IdCargue > 0 Then db.SqlParametros.Add("@idCargue", SqlDbType.Int).Value = .IdCargue
                If .Serial <> "" Then db.SqlParametros.Add("@serial", SqlDbType.VarChar).Value = .Serial
                If .IdOrdenRecepcion > 0 Then db.SqlParametros.Add("@idOrdenRecepcion", SqlDbType.Int).Value = .IdOrdenRecepcion
                If .Region <> "" Then db.SqlParametros.Add("@region", SqlDbType.VarChar).Value = .Region
                If .Centro <> "" Then db.SqlParametros.Add("@centro", SqlDbType.VarChar).Value = .Centro
                If .Cargado <> Enumerados.EstadoBinario.NoEstablecido Then db.SqlParametros.Add("@cargado", SqlDbType.SmallInt).Value = .Cargado
                dtDatos = db.ejecutarDataTable("ObtenerInfoProductosSerial", CommandType.StoredProcedure)
                Return dtDatos
            End With
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListadoPorOrdenes(ByVal alIdOrden As ArrayList) As DataTable
            Dim db As New LMDataAccess
            Dim dtDatos As New DataTable

            If alIdOrden.Count > 0 Then
                Dim idsOrdenes As String = String.Empty
                idsOrdenes = Join(alIdOrden.ToArray, ",")

                db.SqlParametros.Add("@listaOrden", SqlDbType.VarChar, 500).Value = idsOrdenes
                dtDatos = db.ejecutarDataTable("ObtenerInfoSerialesOrdenes", CommandType.StoredProcedure)
            End If

            Return dtDatos
        End Function

#End Region

    End Class
End Namespace
