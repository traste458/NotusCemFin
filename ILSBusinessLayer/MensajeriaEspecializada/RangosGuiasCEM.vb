Imports LMDataAccessLayer

Public Class RangosGuiasCEM
#Region "Atributos (Filtros de Búsqueda)"

    Public Property IdRango As Integer
    Public Property NombreRango As String
    Public Property IdTransportadora As Integer
    Public Property CodigoCuenta As String
    Public Property GuiaInicial As String
    Public Property GuiaFinal As String
    Public Property GuiaActual As String
    Public Property IdUsuario As Integer
#End Region

    Public Function CargarDatosRangos() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtBase As New DataTable
        Try
            With dbManager
                .ejecutarReader("ObtenerRangosGuiasCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dtBase.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dtBase
    End Function

    Public Function TraerDatosRango() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtBase As New DataTable
        Try
            With dbManager
                If IdRango <> 0 Then .SqlParametros.Add("@IdRango", SqlDbType.Int).Value = IdRango

                .ejecutarReader("ObtenerRangosGuiasCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dtBase.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dtBase
    End Function

    Public Function TraerDatosRangosPorTransportadora() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtBase As New DataTable
        Try
            With dbManager
                If IdTransportadora <> 0 Then .SqlParametros.Add("@IdTransportadora", SqlDbType.Int).Value = IdTransportadora

                .ejecutarReader("ObtenerRangosGuiasTransportadoraCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    dtBase.Load(.Reader)
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dtBase
    End Function

    Public Sub GuardarRango()
        Dim dbManager As New LMDataAccess
        Dim Mensaje As String = String.Empty
        Try
            With dbManager
                If IdRango > 0 Then .SqlParametros.Add("@IdRango", SqlDbType.Int).Value = IdRango
                If NombreRango IsNot Nothing Then .SqlParametros.Add("@NombreRango", SqlDbType.NVarChar).Value = NombreRango
                If IdTransportadora <> 0 Then .SqlParametros.Add("@IdTransportadora", SqlDbType.Int).Value = IdTransportadora
                If CodigoCuenta <> 0 Then .SqlParametros.Add("@CodigoCuenta", SqlDbType.Int).Value = CodigoCuenta
                If GuiaInicial IsNot Nothing Then .SqlParametros.Add("@GuiaInicial", SqlDbType.NVarChar).Value = GuiaInicial
                If GuiaFinal IsNot Nothing Then .SqlParametros.Add("@GuiaFinal", SqlDbType.NVarChar).Value = GuiaFinal
                If GuiaActual IsNot Nothing Then .SqlParametros.Add("@GuiaActual", SqlDbType.NVarChar).Value = GuiaActual
                .SqlParametros.Add("@IdUsuario", SqlDbType.Int).Value = IdUsuario

                .ejecutarReader("GuardarRangosGuiasCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Mensaje = .Reader.ToString
                    .Reader.Close()
                End If
            End With
        Catch ex As Exception
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Sub EliminarRango()
        Dim dbManager As New LMDataAccess
        Dim Mensaje As String = String.Empty
        Try
            With dbManager
                If IdRango > 0 Then .SqlParametros.Add("@IdRango", SqlDbType.Int).Value = IdRango
                .SqlParametros.Add("@Mensaje", SqlDbType.NVarChar).Value = ""

                .ejecutarReader("EliminarRangoGuia", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Mensaje = .Reader.ToString
                    .Reader.Close()
                End If
            End With
        Catch ex As Exception
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

End Class
