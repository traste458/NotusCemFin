Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Public Class RecepcionSatelite

    Public Property resultado As New ResultadoProceso
    Property IdUsuario As Integer
    Property Serial As String
    Property IdBodega As Integer
    Property IdServicio As Integer
    Property IdDespacho As Decimal
    Property IdPedido As Decimal
    Property IdTransportadora As Decimal
    Property IdMotorizado As Decimal
    Property NumeroGuia As String
    Property NumeroCuenta As String
    Property MsjMaterialRecoleccion As String
    Property ResultMaterialTransaccion As Integer
    Property patronBusqueda As String
    Property IdServicioMensajeria As Integer
    Property IdSubProducto As Decimal
    Property RangoFinal As Integer
    Property IdRango As Integer
    Property RangoInicial As Integer
    Property IdProducto As Decimal
    Property IdTipoTransporte As Integer
    Property IdTipoMaterial As Integer
    Property Fecha As Date
    Property IdMaterial As String
    Property IdOrdenRecepcion As Integer



    Public Shared Function ObtenerRecepciones() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess

        Return db.EjecutarDataTable("ObtenerTipoRecepcionSatelite", CommandType.StoredProcedure)
    End Function

    Public Function CargarMasivoCantidadOrdenRecepcion(dtMaterial As DataTable)

        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .EjecutarNonQuery("EliminarTransitoriaMaterialCantidadOrden", CommandType.StoredProcedure)
                .TiempoEsperaComando = 0
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaMaterialCantidadOrden"
                    .ColumnMappings.Add("codigoRegion", "CodigoRegion")
                    .ColumnMappings.Add("Material", "Material")
                    .ColumnMappings.Add("Cantidad", "Cantidad")
                    .ColumnMappings.Add("IdBodega", "IdBodega")

                    .ColumnMappings.Add("fila", "Fila")
                    .ColumnMappings.Add("idUsuario", "IdUsuario")
                    .WriteToServer(dtMaterial)
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Clear()
                    .Add("@IdBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idOrdenRecepcion", SqlDbType.Int).Value = IdOrdenRecepcion
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .EjecutarDataTable("ValidarCargueCantidadOrdenRecepcion", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado.EstablecerMensajeYValor(0, "El archivo se proceso de forma correcta")

                    .ConfirmarTransaccion()
                    Return dt
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "Se presentaron errores en el cargue del archivo")
                    Return dt
                    Exit Function
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dt
    End Function

    Public Function CargarMasivoCantidadSerialRecepcion(dtMaterial As DataTable)

        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .EjecutarNonQuery("EliminarTransitoriaMaterialSerialOrden", CommandType.StoredProcedure)
                .TiempoEsperaComando = 0
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaMaterialSerialOrden"
                    .ColumnMappings.Add("CodigoRegion", "CodigoRegion")
                    .ColumnMappings.Add("Material", "Material")
                    .ColumnMappings.Add("Serial", "Serial")
                    .ColumnMappings.Add("idBodega", "IdBodega")

                    .ColumnMappings.Add("fila", "Fila")
                    .ColumnMappings.Add("idUsuario", "IdUsuario")
                    .WriteToServer(dtMaterial)
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Clear()
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .EjecutarDataTable("ValidarCargueSerialOrdenRecepcion", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado.EstablecerMensajeYValor(0, "El archivo se proceso de forma correcta")

                    .ConfirmarTransaccion()
                    Return dt
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "Se presentaron errores en el cargue del archivo")
                    Return dt
                    Exit Function
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dt
    End Function

    Public Function ConfirmarSerialMaterial() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar, 50).Value = Serial
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("ConfirmarSerialProducto", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function EliminarSerialMaterial() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar, 50).Value = Serial
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idUsuario", SqlDbType.Decimal).Value = IdUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("EliminarSerialMaterialTraslado", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function ActualizarEntregaPedidoBodegaSatelite() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idPedido", SqlDbType.VarChar, 50).Value = IdPedido
                    .Add("@idUsuario", SqlDbType.Decimal).Value = IdUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("ActualizarEntregaPedidoBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function ObtenerMotorizados() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        db.SqlParametros.Add("@patronBusqueda", SqlDbType.VarChar, 50).Value = patronBusqueda
        dtDatos = db.EjecutarDataTable("ObtenerMotorizados", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Function ObtenerTransportadoras() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        dtDatos = db.EjecutarDataTable("ObtenerTransportadoras", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Function ObtenerDespachoPorId() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
        Return db.EjecutarDataTable("ObtenerDespachosTraslados", CommandType.StoredProcedure)
    End Function

    Public Function CerrarDespachoTrasladoCEM() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idTipoTransporte", SqlDbType.Decimal).Value = IdTipoTransporte
                    If IdTransportadora > 0 Then dbManager.SqlParametros.Add("@idTransPortadora", SqlDbType.Decimal).Value = IdTransportadora
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    If IdMotorizado > 0 Then dbManager.SqlParametros.Add("@idMotorizado", SqlDbType.Decimal).Value = IdMotorizado
                    .Add("@numeroGuia", SqlDbType.VarChar, 100).Value = NumeroGuia
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CierraDespachoTrasladoCEM", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function CerrarDespachoEntregaClienteCEM() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idTipoTransporte", SqlDbType.Decimal).Value = IdTipoTransporte
                    If IdTransportadora > 0 Then dbManager.SqlParametros.Add("@idTransPortadora", SqlDbType.Decimal).Value = IdTransportadora
                    If IdMotorizado > 0 Then dbManager.SqlParametros.Add("@idMotorizado", SqlDbType.Decimal).Value = IdMotorizado
                    .Add("@numeroGuia", SqlDbType.VarChar, 100).Value = NumeroGuia
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CierraDespachoEntregaClienteCEM", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function CerrarDespachoEntregaCAV() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CierraDespachoEntregaCAV", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function RegistrarSerialDespachoTraslado() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@serial", SqlDbType.VarChar).Value = Serial
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                    .Add("@idServicioMensajeria", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                'dtMaterial = .EjecutarDataTable("AdicionarSerialLeidoDespachoTraslado", CommandType.StoredProcedure)
                'Short.TryParse(.SqlParametros("@result").Value.ToString, ResultMaterialRecoleccion)
                .EjecutarNonQuery("AdicionarSerialLeidoDespachoTraslado", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                'MsjMaterialRecoleccion = .SqlParametros("@mensaje").Value.ToString
                Dim resul As Integer = CType(.SqlParametros("@result").Value.ToString, Integer)
                If resul = 0 Then
                    .ConfirmarTransaccion()
                ElseIf resul = 2 Then
                    .ConfirmarTransaccion()
                ElseIf resul = 5 Then
                    IdServicio = CType(.SqlParametros("@idServicioMensajeria").Value.ToString, Integer)
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function ObtenerPedidoOrdenPorServicio() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@idServicioMensajeria", SqlDbType.Decimal).Value = IdServicioMensajeria
        Return db.EjecutarDataTable("ObtenerPedidoOrdenServicio", CommandType.StoredProcedure)
    End Function

    Public Function ObtenerPedidoDetallePorServicio() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
        Return db.EjecutarDataTable("ObtenerPedidoDetalleServicio", CommandType.StoredProcedure)
    End Function

    Public Function ObtenerPedidoDetalleRemision() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@idPedido", SqlDbType.Decimal).Value = IdDespacho
        Return db.EjecutarDataTable("ObtenerPedidoDetalleRemision", CommandType.StoredProcedure)
    End Function

    Public Function ObtenerSerialesPedidoOrdenServicio() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@idSubProducto", SqlDbType.Decimal).Value = IdSubProducto
        db.SqlParametros.Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
        Return db.EjecutarDataTable("ObtieneSerialesPedidoOrdenServicio", CommandType.StoredProcedure)
    End Function

    Public Function ObtenerPedidoDetalleNotificacion() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
        Return db.EjecutarDataTable("ObtenerPedidiYDetalleNotificacion", CommandType.StoredProcedure)
    End Function

    Public Function ObtenerMateriales(opcion As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        db.SqlParametros.Add("@opcion", SqlDbType.Int).Value = opcion
        If patronBusqueda <> "" Then db.SqlParametros.Add("@patronBusqueda", SqlDbType.VarChar, 50).Value = patronBusqueda
        If IdProducto <> 0 Then db.SqlParametros.Add("@idProducto", SqlDbType.VarChar, 50).Value = IdProducto
        dtDatos = db.EjecutarDataTable("ObtenerMaterialesPorPatron", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Function ObtenerRangosEdadMaterial() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        dtDatos = db.EjecutarDataTable("ObtenerRangosEdadMaterialSatelite", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Function IngresarRangoEdadMaterial() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()

                    .Add("@rFinal", SqlDbType.Int).Value = RangoFinal
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("IngresarRangosEdadMaterial", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function EliminarRangoEdadMaterial() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()

                    .Add("@idRango", SqlDbType.Int).Value = IdRango
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("EliminarRangoEdadMaterial", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function ModificarRangoEdadMaterial() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()

                    .Add("@idRango", SqlDbType.Int).Value = IdRango
                    .Add("@rInicial", SqlDbType.Int).Value = RangoInicial
                    .Add("@rFinal", SqlDbType.Int).Value = RangoFinal
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("ModificaRangoEdadMaterial", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function ObtenerProductos() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        db.SqlParametros.Add("@patronBusqueda", SqlDbType.VarChar, 50).Value = patronBusqueda
        dtDatos = db.EjecutarDataTable("ObtieneProductoPorPatron", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Function ObtenerHistorialCardex() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        If IdBodega > 0 Then db.SqlParametros.Add("@idBodega", SqlDbType.Int).Value = IdBodega
        db.SqlParametros.Add("@fecha", SqlDbType.Date).Value = Fecha
        db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
        If IdTipoMaterial > 0 Then db.SqlParametros.Add("@tipoMaterial", SqlDbType.Int).Value = IdTipoMaterial
        If IdMaterial IsNot Nothing Then db.SqlParametros.Add("@idMaterial", SqlDbType.VarChar, 50).Value = IdMaterial
        dtDatos = db.EjecutarDataTable("ObtenerHistorialCardexBodegaSatelite", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Function CerrarDespachoEntregaClienteDomicilio() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idTipoTransporte", SqlDbType.Decimal).Value = IdTipoTransporte
                    If IdTransportadora > 0 Then dbManager.SqlParametros.Add("@idTransPortadora", SqlDbType.Decimal).Value = IdTransportadora
                    If IdMotorizado > 0 Then dbManager.SqlParametros.Add("@idMotorizado", SqlDbType.Decimal).Value = IdMotorizado
                    .Add("@numeroGuia", SqlDbType.VarChar, 100).Value = NumeroGuia
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CierraDespachoEntregaClienteDomicilio", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function CargarRecepcionPorSeriales(dtMaterial As DataTable)

        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .EjecutarNonQuery("EliminarTransitoriaMaterialSerialOrden", CommandType.StoredProcedure)
                .TiempoEsperaComando = 0
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaMaterialSerialOrden"
                    .ColumnMappings.Add("CodigoRegion", "CodigoRegion")
                    .ColumnMappings.Add("Material", "Material")
                    .ColumnMappings.Add("Serial", "Serial")
                    .ColumnMappings.Add("idBodega", "IdBodega")

                    .ColumnMappings.Add("fila", "Fila")
                    .ColumnMappings.Add("idUsuario", "IdUsuario")
                    .WriteToServer(dtMaterial)
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Clear()
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idOrdenRecepcion", SqlDbType.Int).Value = IdOrdenRecepcion
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .EjecutarDataTable("ValidarCargueSerialOrdenRecepcion", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado.EstablecerMensajeYValor(0, "El archivo se proceso de forma correcta")

                    .ConfirmarTransaccion()
                    Return dt
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "Se presentaron errores en el cargue del archivo")
                    Return dt
                    Exit Function
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dt
    End Function

    Public Function CargarRecepcionPorSerialesFabricantes(dtMaterial As DataTable)

        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .EjecutarNonQuery("EliminarTransitoriaMaterialSerialOrden", CommandType.StoredProcedure)
                .TiempoEsperaComando = 0
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaMaterialSerialOrden"
                    .ColumnMappings.Add("CodigoRegion", "CodigoRegion")
                    .ColumnMappings.Add("Material", "Material")
                    .ColumnMappings.Add("Serial", "Serial")
                    .ColumnMappings.Add("idBodega", "IdBodega")

                    .ColumnMappings.Add("fila", "Fila")
                    .ColumnMappings.Add("idUsuario", "IdUsuario")
                    .WriteToServer(dtMaterial)
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Clear()
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idOrdenRecepcion", SqlDbType.Int).Value = IdOrdenRecepcion
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .EjecutarDataTable("ValidarCargueSerialOrdenRecepcionFabricante", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado.EstablecerMensajeYValor(0, "El archivo se proceso de forma correcta")

                    .ConfirmarTransaccion()
                    Return dt
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "Se presentaron errores en el cargue del archivo")
                    Return dt
                    Exit Function
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dt
    End Function

    Public Function CargarMasivoCantidadOrdenRecepcionFabricante(dtMaterial As DataTable)

        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .EjecutarNonQuery("EliminarTransitoriaMaterialCantidadOrden", CommandType.StoredProcedure)
                .TiempoEsperaComando = 0
                .InicilizarBulkCopy()
                With .BulkCopy
                    .DestinationTableName = "TransitoriaMaterialCantidadOrden"
                    .ColumnMappings.Add("codigoRegion", "CodigoRegion")
                    .ColumnMappings.Add("Material", "Material")
                    .ColumnMappings.Add("Cantidad", "Cantidad")
                    .ColumnMappings.Add("IdBodega", "IdBodega")

                    .ColumnMappings.Add("fila", "Fila")
                    .ColumnMappings.Add("idUsuario", "IdUsuario")
                    .WriteToServer(dtMaterial)
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                With .SqlParametros
                    .Clear()
                    .Add("@IdBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idOrdenRecepcion", SqlDbType.Int).Value = IdOrdenRecepcion
                    .Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dt = .EjecutarDataTable("ValidarCargueCantidadOrdenRecepcionFabricante", CommandType.StoredProcedure)
                Dim resul As Integer = CType(.SqlParametros("@resultado").Value.ToString, Integer)
                If resul = 0 Then
                    resultado.EstablecerMensajeYValor(0, "El archivo se proceso de forma correcta")

                    .ConfirmarTransaccion()
                    Return dt
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "Se presentaron errores en el cargue del archivo")
                    Return dt
                    Exit Function
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dt
    End Function

    Public Function PosicionarOrdenRecepcion(ByVal idOrdenRecepcion As Integer, ByVal IdPosicion As Integer, ByVal idUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idOrdenRecepcion", SqlDbType.Decimal).Value = idOrdenRecepcion
                    .Add("@idPosicion", SqlDbType.Decimal).Value = IdPosicion
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("AsignarOrdenRecepcionAPosicionDeBodega", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    idOrdenRecepcion = .SqlParametros("@IdOrdenRecepcion").Value
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(33, ex.Message)
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Shared Function ObtenerOTBenOrdenRecepcion(filtro As FiltrosOTB) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.IdOrdenRecepcion > 0 Then db.SqlParametros.Add("@idOrdenReepcion", SqlDbType.Int).Value = filtro.IdOrdenRecepcion
        If filtro.IdBodega > 0 Then db.SqlParametros.Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodega
        If filtro.IdOTB > 0 Then db.SqlParametros.Add("@idOTB", SqlDbType.Int).Value = filtro.IdOTB

        Return db.EjecutarDataTable("ObtenerOtbsPendientesAcomodarFabricante", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerOrdenRecepcionPosicion(filtro As FiltrosOrdenRecepcionSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.IdOrdenRecepcion > 0 Then db.SqlParametros.Add("@idOrdenReepcion", SqlDbType.Int).Value = filtro.IdOrdenRecepcion
        If filtro.IdBodega > 0 Then db.SqlParametros.Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodega
        If filtro.FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaInicial
        If filtro.FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFinal

        Return db.EjecutarDataTable("ObtenerOrdenesRecepcionPendientesAcomodarFabricante", CommandType.StoredProcedure)
    End Function

    Public Overloads Shared Function BuscarDetalleOrdenRecepcionPosicion(ByVal idOrden As Integer) As DataTable
        Dim db As New LMDataAccess
        Dim dtDatos As New DataTable
        With idOrden
            db.AgregarParametroSQL("@idOrdenReepcion", idOrden)
            dtDatos = db.EjecutarDataTable("ObtenerOrdenesRecepcionDetallePendientesAcomodarFabricante", CommandType.StoredProcedure)
            Return dtDatos
        End With
        Return dtDatos

    End Function

    Public Function ObtenerPedidoDespachoDetalle() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
        Return db.EjecutarDataTable("ObtenerPedidoDespachoDetalle", CommandType.StoredProcedure)
    End Function

    Public Function IngresarGuiaTransportadoraDespacho() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()

                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@idTransportadora", SqlDbType.Decimal).Value = IdTransportadora
                    .Add("@guia", SqlDbType.VarChar, 50).Value = NumeroGuia
                    .Add("@cuenta", SqlDbType.VarChar, 100).Value = NumeroCuenta
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("IngresarGuiaTransportadoraDespacho", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@resultado").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@resultado").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function
End Class
