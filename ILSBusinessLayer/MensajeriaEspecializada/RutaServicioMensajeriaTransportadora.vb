Imports LMDataAccessLayer

Public Class RutaServicioMensajeriaTransportadora

    Public Property IdDespacho As Long
    Public Property Radicado As Long
    Public Property IdTransportadora As Integer
    Public Property IdRangoGuia As Integer
    Public Property IdBodegaOrigen As Integer
    Public Property IdBodegaDestino As Integer
    Public Property Peso As Double
    Public Property Volumen As Double
    Public Property idTipoUnidad As Integer
    Public Property cantidad As String
    Public Property idTipoEnvio As Integer
    Public Property IdUsuario As Integer

    Public Property Guia As String
    Public Property FechaInicio As Date
    Public Property FechaFin As Date

    Public listIdDetalle As List(Of Integer)

    Public Sub New()
        If listIdDetalle Is Nothing Then listIdDetalle = New List(Of Integer)
    End Sub

    Public Function ValidarRadicado() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As New DataTable
        Try
            With dbManager
                If Radicado > 0 Then .SqlParametros.Add("@Radicado", SqlDbType.BigInt).Value = Radicado
                If IdBodegaDestino > 0 Then .SqlParametros.Add("@IdBodega", SqlDbType.Int).Value = IdBodegaDestino

                .ejecutarReader("ValidarBodegaPorRadicado", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dt.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dt
    End Function

    Public Function BuscarDespachos() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As New DataTable
        Try
            With dbManager
                If IdTransportadora > 0 Then .SqlParametros.Add("@IdTransportadora", SqlDbType.Int).Value = IdTransportadora
                If Guia.Length > 0 Then .SqlParametros.Add("@Guia", SqlDbType.VarChar).Value = Guia
                If Radicado > 0 Then .SqlParametros.Add("@Radicado", SqlDbType.BigInt).Value = Radicado
                .SqlParametros.Add("@FechaInicio", SqlDbType.Date).Value = FechaInicio
                .SqlParametros.Add("@FechaFin", SqlDbType.Date).Value = FechaFin

                .ejecutarReader("ObtenerDespachosGuiasCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dt.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dt
    End Function


    Public Function ObtenerDetalleDespacho() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As New DataTable
        Try
            With dbManager

                If IdDespacho > 0 Then .SqlParametros.Add("@IdDespacho", SqlDbType.Int).Value = IdDespacho

                .ejecutarReader("ObtenerDespachosGuiasCEMDetalle", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    dt.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dt
    End Function

    Public Function GuardarDespacho() As Integer
        Dim dbManager As New LMDataAccess
        Dim resultado As Integer = 0
        Try
            With dbManager
                If IdTransportadora > 0 Then .SqlParametros.Add("@IdTransportadora", SqlDbType.Int).Value = IdTransportadora
                If IdRangoGuia > 0 Then .SqlParametros.Add("@IdRangoGuia", SqlDbType.Int).Value = IdRangoGuia
                If IdBodegaOrigen > 0 Then .SqlParametros.Add("@IdBodegaOrigen", SqlDbType.Int).Value = IdBodegaOrigen
                .SqlParametros.Add("@IdBodegaDestino", SqlDbType.Int).Value = IdBodegaDestino
                If Peso > 0 Then .SqlParametros.Add("@Peso", SqlDbType.Int).Value = Peso
                If Volumen > 0 Then .SqlParametros.Add("@Volumen", SqlDbType.Int).Value = Volumen
                If cantidad > 0 Then .SqlParametros.Add("@cantidad", SqlDbType.Int).Value = cantidad
                If idTipoUnidad > 0 Then .SqlParametros.Add("@idTipoUnidad", SqlDbType.Int).Value = idTipoUnidad
                If idTipoEnvio > 0 Then .SqlParametros.Add("@idTipoEnvio", SqlDbType.Int).Value = idTipoEnvio
                .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario
                .SqlParametros.Add("@NuevoDespacho", SqlDbType.Int)
                .SqlParametros("@NuevoDespacho").Direction = ParameterDirection.Output

                .EjecutarNonQuery("GuardarDespachoGuiaTransportadora", CommandType.StoredProcedure)

                resultado = .SqlParametros("@NuevoDespacho").Value
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function

    Public Function ObtenerSiguienteDespacho() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As New DataTable
        Try
            With dbManager

                .ejecutarReader("ObtenerSiguienteDespachoGuiaTransportadora", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dt.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dt
    End Function

    Public Function TraerDespachosTemporales() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As New DataTable
        Try
            With dbManager
                .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario
                .ejecutarReader("ObtenerDespachosGuiasCEMTemporales", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dt.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dt
    End Function

    Public Function AgregarDespachoTemporal() As String
        Dim dbManager As New LMDataAccess
        Dim resultado As String = ""

        Try
            With dbManager
                .SqlParametros.Add("@Radicado", SqlDbType.BigInt).Value = Radicado
                .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario
                .SqlParametros.Add("@Mensaje", SqlDbType.BigInt)
                .SqlParametros("@Mensaje").Direction = ParameterDirection.Output
                .ejecutarReader("AgregarDespachoGuiaCEMTemporal", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return resultado
    End Function

    Public Sub EliminarDespachoTemporal()
        Dim dbManager As New LMDataAccess
        Dim resultado As Integer = 1

        Try
            With dbManager
                .SqlParametros.Add("@Radicado", SqlDbType.BigInt).Value = Radicado

                .EjecutarNonQuery("EliminarDespachoGuiaCEMTemporal", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Sub LimpiarDespachosTemporal()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .EjecutarNonQuery("LimpiarDespachosGuiasCEMTemporal", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

End Class
