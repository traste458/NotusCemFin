Imports LMDataAccessLayer

Public Class Delivery

#Region "Atributos (Propiedades)"
    Private dbManager As New LMDataAccess
#End Region

#Region "Propiedades"
    Public Property numeroOrden As String
    Public Property idAlistamiento As String
    Public Property nombre As String
    Public Property tipoDocumento As String
    Public Property numeroDocumento As String
    Public Property telefono As String
    Public Property numeroGuia As String
    Public Property numeroPedido As String
    Public Property valorDeclarado As String
    Public Property procesoVenta As String
    Public Property centroOrigen As String
    Public Property nombreUbicacion As String
    Public Property direccionOrigen As String
    Public Property codigoMunicipio As String
    Public Property departamento As String
    Public Property municipio As String
    Public Property barrio As String
    Public Property direccionNormalizada As String
    Public Property direccionLenguajeNatural As String
    Public Property complemento As String
    Public Property estado As String
    Public Property fecha As Date
    Public Property franja As String
    Public Property hora As String
    Public Property observacion As String

    Public Property nombreTransportador As String
    Public Property cedulaTransportador As String
    Public Property placaTransportador As String

    Public Property idEstado As Integer
    Public Property dateFechaInicio As Date
    Public Property dateFechaFin As Date
#End Region

    Public Function ConsultarDeliveryPool() As DataTable

        Dim dtDatos As New DataTable
        If dbManager IsNot Nothing Then dbManager = New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    If idAlistamiento <> "" And idAlistamiento IsNot Nothing Then .Add("@id_alistamiento", SqlDbType.VarChar).Value = idAlistamiento
                    If numeroOrden <> "" And numeroOrden IsNot Nothing Then .Add("@numero_orden", SqlDbType.VarChar).Value = numeroOrden
                    If numeroDocumento <> "" And numeroDocumento IsNot Nothing Then .Add("@numero_documento", SqlDbType.VarChar).Value = numeroDocumento
                    If numeroPedido <> "" And numeroPedido IsNot Nothing Then .Add("@numero_pedido", SqlDbType.VarChar).Value = numeroPedido
                    If nombreUbicacion <> "" And nombreUbicacion IsNot Nothing Then .Add("@nombre_ubicacion", SqlDbType.VarChar).Value = nombreUbicacion
                    If departamento <> "" And departamento IsNot Nothing Then .Add("@departamento", SqlDbType.VarChar).Value = departamento
                    If procesoVenta <> "" And procesoVenta IsNot Nothing Then .Add("@proceso_venta", SqlDbType.VarChar).Value = procesoVenta
                    If franja <> "" And franja IsNot Nothing Then .Add("@franja", SqlDbType.VarChar).Value = franja
                    If idEstado > 0 Then .Add("@IdEstado", SqlDbType.Int).Value = idEstado
                    If centroOrigen <> "" And centroOrigen IsNot Nothing Then .Add("@centro_origen", SqlDbType.VarChar).Value = centroOrigen
                    If dateFechaInicio > Date.MinValue AndAlso dateFechaFin > Date.MinValue Then
                        .Add("@fechaInicial", SqlDbType.DateTime).Value = dateFechaInicio
                        .Add("@fechaFinal", SqlDbType.DateTime).Value = dateFechaFin
                    End If

                End With
                .TiempoEsperaComando = 0
                dtDatos = .EjecutarDataTable("ObtenerInformacionPoolDelivery", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

End Class