Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer

Public Class BodegaSatelite

#Region "Propiedades"
    Property NumeroPedido As String
    Property IdSubProducto As Decimal
    Public Property IdDespacho As Decimal
    Public Property IdPedido As Decimal
    Property IdBodega As Integer
    Property CantidadIngresada As Integer
    Property IdUsuario As Integer
    Property IdUsuarioRegistra As Integer
    Property UnidadesRecogidas As Integer
    Property Serial As String
    Property SerialNuevo As String
    Property IdentificacionUsuario As String
    Property IdPerfil As String
    Property Nombre As String
    Property Usuario As String
    Property Identificacion As String
    Property ObservacionAnular As String
    Property MsjMaterialRecoleccion As String
    Property ResultMaterialRecoleccion As Integer
    Public Property tienePedidoSap As Boolean
    Public Property idOrdenRecepcion As Decimal
    Public Property opcion As Integer
    Public Property resultado As New ResultadoProceso
    Public Property esMismaBodega As Boolean

    Private _idOrdenDeRecepcion As Integer
    Private _idRegistro As Integer
    Private _nombreArchivo As String
    Private _rutaAlmacenamiento As String
    Private _tipoContenido As String

    Public Property IdOrdenDeRecepcion As Integer
        Get
            Return _idOrdenDeRecepcion
        End Get
        Set(value As Integer)
            _idOrdenDeRecepcion = value
        End Set
    End Property

    Public Property IdRegistro As Integer
        Get
            Return _idRegistro
        End Get
        Set(value As Integer)
            _idRegistro = value
        End Set
    End Property

    Public Property NombreArchivo As String
        Get
            Return _nombreArchivo
        End Get
        Set(value As String)
            _nombreArchivo = value
        End Set
    End Property

    Public Property RutaAlmacenamiento As String
        Get
            Return _rutaAlmacenamiento
        End Get
        Set(value As String)
            _rutaAlmacenamiento = value
        End Set
    End Property

    Public Property TipoContenido As String
        Get
            Return _tipoContenido
        End Get
        Set(value As String)
            _tipoContenido = value
        End Set
    End Property

#End Region


#Region "Metodos"


    Public Shared Function ConsultarBodega(ByVal idUsuarioConsulta As Integer, ByVal idTipo As Integer) As DataTable
        Dim _dbManager As New LMDataAccess
        Dim dtDatos As New DataTable

        Try
            With _dbManager
                If idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuarioConsulta
                If idTipo > 0 Then .SqlParametros.Add("@idTipo", SqlDbType.Int).Value = idTipo
                dtDatos = .EjecutarDataTable("ObtenerBodegasPorUsuarioCiudadTipoBodega", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function


    Public Shared Function ObtenerBodegasPorUsuario(idUsuario As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dt As New DataTable()
        db.AgregarParametroSQL("@UsuarioId", idUsuario, SqlDbType.Int)
        dt = db.EjecutarDataTable("ObtenerBodegaXUsuarioSatelite", CommandType.StoredProcedure)
        Return dt
    End Function

    Public Shared Function ObtenerTipoRecepcion() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Return db.EjecutarDataTable("ObtenerTipoRecepcionSatelite", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerBodegaOrigenDespachoSinPedido(idUsuario As Integer, Optional idTipoBodega As Integer = 0) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        If idUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.BigInt).Value = idUsuario
        If idTipoBodega > 0 Then db.SqlParametros.Add("@idTipoBodega", SqlDbType.Int).Value = idTipoBodega
        dtDatos = db.EjecutarDataTable("ObtenerBodegaOrigenDespachoSinPedido", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Shared Function ObtenerTiposPedidoBodega(opcion As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        db.SqlParametros.Add("@opcion", SqlDbType.BigInt).Value = opcion
        dtDatos = db.EjecutarDataTable("ObtenerTiposPedidoBodega", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Shared Function ObtenerBodegasDestino(idBodegaOrigen As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        db.AgregarParametroSQL("@idBodegaOrigen", idBodegaOrigen, SqlDbType.Int)
        dtDatos = db.EjecutarDataTable("ObtenerBodegasDestino", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Shared Function ObtenerMaterialesNoSerial(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        db.AgregarParametroSQL("@idBodega", filtro.IdBodegaOrigen, SqlDbType.Int)
        Return db.EjecutarDataTable("ObtenerMaterialesNoSerialDespachoSinPedido", CommandType.StoredProcedure)
    End Function

    Public Function VerificarOrdenRecepcion(datos As FiltrosOrdenRecepcionSatelite) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@numeroOrden", SqlDbType.VarChar).Value = datos.NumeroOrden
                    .Add("@guiaTransportadora", SqlDbType.VarChar).Value = datos.NumeroGuia
                    .Add("@idBodega", SqlDbType.Int).Value = datos.IdBodega
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("VerificarExistenciaOrdenRecepcion", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                .ConfirmarTransaccion()
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

    Public Function VerificarDetalleOrdenRecepcion(datos As FiltrosOrdenRecepcionSatelite) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idOrden", SqlDbType.Int).Value = datos.IdOrdenRecepcion
                    .Add("@valida", SqlDbType.Int).Value = datos.valida
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("VerificarExistenciaDetalleOrdenRecepcion", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                .ConfirmarTransaccion()
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

    Public Function VerificarImpresionOTB(idOrden As Decimal, idOtb As Decimal) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idOrden", SqlDbType.Decimal).Value = idOrden
                    .Add("@idOtb", SqlDbType.Decimal).Value = idOtb
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("VerificarExistenciaReimpresion", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                .ConfirmarTransaccion()
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

    Public Function AnularOrdenRecepcion(ByVal idOrcen As Integer, ByVal Observacion As String) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idOrden", SqlDbType.Int).Value = idOrcen
                    .Add("@Observacion", SqlDbType.VarChar, 500).Value = Observacion
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("AnularOrdenRecepcionFabricante", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                .ConfirmarTransaccion()
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

    Public Function CerrarOrdenRecepcionFabricante(datos As FiltrosOrdenRecepcionSatelite) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@IdUsuario", SqlDbType.Decimal).Value = datos.IdUsuario
                    .Add("@IdOrdenRecepcion", SqlDbType.Decimal).Value = datos.IdOrdenRecepcion
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CerrarRecepcionBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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
    Public Function CrearRecepcionSatlite(datos As FiltrosOrdenRecepcionSatelite) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@IdUsuario", SqlDbType.Decimal).Value = datos.IdUsuario
                    .Add("@idTipoRecepcion", SqlDbType.Int).Value = datos.IdTipoRecepcion
                    .Add("@idBodega", SqlDbType.Int).Value = datos.IdBodega
                    .Add("@numeroOrden", SqlDbType.VarChar).Value = datos.NumeroOrden
                    .Add("@guiaTransportadora", SqlDbType.VarChar).Value = datos.NumeroGuia
                    .Add("@link", SqlDbType.VarChar).Value = datos.link
                    .Add("@idTransportadora", SqlDbType.Int).Value = datos.IdTransportadora
                    .Add("@idDocumento", SqlDbType.Int).Value = datos.idDocumento
                    .Add("@tipoDeclaracion", SqlDbType.Int).Value = datos.TipoDeclaracion
                    .Add("@IdOrdenRecepcion", SqlDbType.Decimal).Direction = ParameterDirection.Output
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CrearRecepcionBodegaSatelite", CommandType.StoredProcedure)
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
    Public Function CrearPedidoDespacho(datos As FiltroDespachoSinPedidoSatelite) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@IdUsuario", SqlDbType.Decimal).Value = datos.IdUsuario
                    .Add("@tipoPedido", SqlDbType.Int).Value = datos.IdTipoPedido
                    .Add("@numeroPedido", SqlDbType.VarChar, 500).Value = datos.NumeroPedido
                    If datos.Observaciones IsNot Nothing Then .Add("@observaciones", SqlDbType.VarChar).Value = datos.Observaciones
                    .Add("@idBodegaOrigen", SqlDbType.Int).Value = datos.IdBodegaOrigen
                    .Add("@idBodegaDestino", SqlDbType.Int).Value = datos.IdBodegaDestino
                    .Add("@idPedido", SqlDbType.Decimal).Direction = ParameterDirection.Output
                    .Add("@idDespacho", SqlDbType.Decimal).Direction = ParameterDirection.Output
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CrearPedidoDespachoBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    IdPedido = .SqlParametros("@idPedido").Value
                    IdDespacho = .SqlParametros("@idDespacho").Value
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

    Public Shared Function ObtenerOrdenRecepcionEnProceso(filtro As FiltrosOrdenRecepcionSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.FechaInicial <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaInicial
        If filtro.FechaFinal <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFinal
        If filtro.IdOrdenRecepcion > 0 Then db.SqlParametros.Add("@idOrdenReepcion", SqlDbType.Int).Value = filtro.IdOrdenRecepcion
        If filtro.NumeroOrden IsNot Nothing Then db.SqlParametros.Add("@numeroOrden", SqlDbType.VarChar, 200).Value = filtro.NumeroOrden
        If filtro.NumeroGuia IsNot Nothing Then db.SqlParametros.Add("@numeroGuia", SqlDbType.VarChar, 200).Value = filtro.NumeroGuia
        If filtro.IdBodega > 0 Then db.SqlParametros.Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodega
        If filtro.IdTransportadora > 0 Then db.SqlParametros.Add("@idtransportadora", SqlDbType.Int).Value = filtro.IdTransportadora
        If filtro.IdTipoRecepcion > 0 Then db.SqlParametros.Add("@idTipoRecepcion", SqlDbType.Int).Value = filtro.IdTipoRecepcion
        If filtro.IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.BigInt).Value = filtro.IdEstado

        Return db.EjecutarDataTable("ObtenerOrdenRecepcionSateliteEnProceso", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerDespachoPoolPedidosBodegaSatelite(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodegaOrigen", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.IdTipoPedido > 0 Then db.SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = filtro.IdTipoPedido
        If filtro.IdPedido > 0 Then db.SqlParametros.Add("@idPedido", SqlDbType.Int).Value = filtro.IdPedido
        If filtro.NumeroPedido IsNot Nothing Then db.SqlParametros.Add("@pedido", SqlDbType.VarChar, 200).Value = filtro.NumeroPedido
        Return db.EjecutarDataTable("ObtenerDespachoPoolPedidosBodegaSatelite", CommandType.StoredProcedure)
    End Function

    Public Shared Function DespacharPedidoBodegaSatelite(idPedido As Integer, idUsuario As Integer, cantidadCajas As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@cantidadCajas", SqlDbType.Decimal).Value = cantidadCajas
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("DespacharPedidoBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Shared Function AdicionarSerialesOtbAOtbBodegaSatelite(filtro As FiltrosOTB) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = filtro.IdUsuario
                    .Add("@idOtbOrigen", SqlDbType.Decimal).Value = filtro.IdOTBOrigen
                    .Add("@idOtbDestino", SqlDbType.Decimal).Value = filtro.IdOTBDestino
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("AdicionarSerialesOtbAOtbBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Shared Function AdicionarSerialesCajaAOtbBodegaSatelite(filtro As FiltrosOTB) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = filtro.IdUsuario
                    .Add("@idCajaOrigen", SqlDbType.Decimal).Value = filtro.IdOTBOrigen
                    .Add("@idOtbDestino", SqlDbType.Decimal).Value = filtro.IdOTBDestino
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("AdicionarSerialesCajaAOtbBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Shared Function CheckDetallePedidoBodegaSatelite(idPedido As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idDetalle", SqlDbType.Decimal).Value = idPedido
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CheckDetallePedidoBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Shared Function ValidarCantidadesPedidoBodegaSatelite(idPedido As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("validarCantidadesPedidoBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Shared Function RegistrarLecturaMaterialDespachoBodegaSateliteCantidadNoSerializada(idPedido As Integer, idDetalle As Integer, cantidad As Integer, idUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@idDetallePedido", SqlDbType.Decimal).Value = idDetalle
                    If cantidad > 0 Then .Add("@cantidad", SqlDbType.Int).Value = cantidad
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("RegistrarLecturaMaterialDespachoBodegaSateliteCantidadNoSerializada", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Shared Function RegistrarLecturaMaterialDespachoBodegaSateliteSerial(idPedido As Integer, idDetalle As Integer, serialInicial As String, idUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@idDetallePedido", SqlDbType.Decimal).Value = idDetalle
                    If Not String.IsNullOrEmpty(serialInicial) Then .Add("@serialInicial", SqlDbType.VarChar).Value = serialInicial
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("RegistrarLecturaMaterialDespachoBodegaSateliteSerial", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Shared Function RegistrarLecturaMaterialDespachoBodegaSateliteRango(idPedido As Integer, idDetalle As Integer, serialInicial As String, serialFinal As String, idUsuario As Integer) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As New DataTable
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@idDetallePedido", SqlDbType.Decimal).Value = idDetalle
                    If Not String.IsNullOrEmpty(serialInicial) Then .Add("@serialInicial", SqlDbType.VarChar).Value = serialInicial
                    If Not String.IsNullOrEmpty(serialFinal) Then .Add("@serialFinal", SqlDbType.VarChar).Value = serialFinal
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtErrores = .EjecutarDataTable("RegistrarLecturaMaterialDespachoBodegaSateliteRango", CommandType.StoredProcedure)
                If dtErrores.Rows.Count > 0 Then
                    dbManager.AbortarTransaccion()
                Else
                    dbManager.ConfirmarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dtErrores
    End Function

    Public Shared Function RegistrarLecturaMaterialDespachoBodegaSateliteSerialMasivo(idPedido As Integer, idDetalle As Integer, dtSeriales As DataTable, idUsuario As Integer) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As New DataTable
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@idDetallePedido", SqlDbType.Decimal).Value = idDetalle
                    .Add("@tbSerial", SqlDbType.Structured).Value = dtSeriales
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtErrores = .EjecutarDataTable("RegistrarLecturaMaterialDespachoBodegaSateliteSerialMasivo", CommandType.StoredProcedure)
                If dtErrores.Rows.Count > 0 Then
                    dbManager.AbortarTransaccion()
                Else
                    dbManager.ConfirmarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dtErrores
    End Function

    Public Shared Function RegistrarLecturaMaterialDespachoBodegaSateliteCantidadMasivo(idPedido As Integer, dtMaterial As DataTable, idUsuario As Integer) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As New DataTable
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@tbMaterial", SqlDbType.Structured).Value = dtMaterial
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtErrores = .EjecutarDataTable("RegistrarLecturaMaterialDespachoBodegaSateliteCantidadMasivo", CommandType.StoredProcedure)
                If dtErrores.Rows.Count > 0 Then
                    dbManager.AbortarTransaccion()
                Else
                    dbManager.ConfirmarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dtErrores
    End Function

    Public Shared Function RegistrarLecturaMaterialDespachoBodegaSateliteCaja(idPedido As Integer, idDetalle As Integer, serialInicial As String, idUsuario As Integer) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As New DataTable
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@idDetallePedido", SqlDbType.Decimal).Value = idDetalle
                    .Add("@caja", SqlDbType.VarChar).Value = serialInicial
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtErrores = .EjecutarDataTable("RegistrarLecturaMaterialDespachoBodegaSateliteCaja", CommandType.StoredProcedure)
                If dtErrores.Rows.Count > 0 Then
                    dbManager.AbortarTransaccion()
                Else
                    dbManager.ConfirmarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dtErrores
    End Function

    Public Shared Function RegistrarLecturaMaterialDespachoBodegaSateliteOtb(idPedido As Integer, idDetalle As Integer, serialInicial As String, idUsuario As Integer) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As New DataTable
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@idDetallePedido", SqlDbType.Decimal).Value = idDetalle
                    .Add("@otb", SqlDbType.VarChar).Value = serialInicial
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtErrores = .EjecutarDataTable("RegistrarLecturaMaterialDespachoBodegaSateliteOtb", CommandType.StoredProcedure)
                If dtErrores.Rows.Count > 0 Then
                    dbManager.AbortarTransaccion()
                Else
                    dbManager.ConfirmarTransaccion()
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dtErrores
    End Function

    Public Shared Function RegistrarDespachoBodegaSatelite(idPedido As Integer, idUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = idUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = idPedido
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("RegistrarDespachoBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Shared Function ObtenerDespachoPoolPedidosDetalleBodegaSatelite(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdPedido > 0 Then db.SqlParametros.Add("@idPedido", SqlDbType.Int).Value = filtro.IdPedido
        Return db.EjecutarDataTable("ObtenerDespachoPoolPedidosDetalleBodegaSatelite", CommandType.StoredProcedure)
    End Function
    Public Function ObtenerOtbBodegaSatelite(filtro As FiltrosOTB) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        db.SqlParametros.Add("@idOtb", SqlDbType.Decimal).Value = filtro.IdOTB
        db.SqlParametros.Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodega
        db.SqlParametros.Add("@mismaBodega", SqlDbType.Bit).Direction = ParameterDirection.Output
        dtDatos = db.EjecutarDataTable("ObtenerOtbBodegaSatelite", CommandType.StoredProcedure)
        esMismaBodega = CBool(db.SqlParametros("@mismaBodega").Value)

        Return dtDatos
    End Function

    Public Shared Function ObtenerUrlNacionalizacionDespachoBodegaSatelite(ByVal idPedido As String) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If idPedido > 0 Then db.SqlParametros.Add("@idPedido", SqlDbType.Int).Value = idPedido
        Return db.EjecutarDataTable("ObtenerUrlNacionalizacionDespachoBodegaSatelite", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerInfoDocumentoNacionalizacion(ByVal idDocumento As String) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If idDocumento > 0 Then db.SqlParametros.Add("@idDocumento", SqlDbType.Int).Value = idDocumento
        Return db.EjecutarDataTable("ObtenerInfoDocumentoNacionalizacion", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerDespachosEnProceso(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodegaOrigen", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.IdTipoPedido > 0 Then db.SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = filtro.IdTipoPedido
        If filtro.NumeroPedido IsNot Nothing Then db.SqlParametros.Add("@pedido", SqlDbType.VarChar, 200).Value = filtro.NumeroPedido
        Return db.EjecutarDataTable("ObtenerDespachoSateliteEnProceso", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerDespachosFactuacionTransportadora(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodegaOrigen", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.IdTipoServicio > 0 Then db.SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = filtro.IdTipoServicio
        If filtro.IdTransportadora > 0 Then db.SqlParametros.Add("@idTransportadora", SqlDbType.Int).Value = filtro.IdTransportadora
        If filtro.NumeroPedido IsNot Nothing Then db.SqlParametros.Add("@pedido", SqlDbType.VarChar, 200).Value = filtro.NumeroPedido
        Return db.EjecutarDataTable("ObtenerDespachosFactuacionTransportadora", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerDespachoSateliteMotorizado(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.IdMotorizado > 0 Then db.AgregarParametroSQL("@idMotorizado", filtro.IdMotorizado, SqlDbType.Int)
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodegaOrigen", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.IdTipoPedido > 0 Then db.SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = filtro.IdTipoPedido
        If filtro.NumeroPedido IsNot Nothing Then db.SqlParametros.Add("@pedido", SqlDbType.VarChar, 200).Value = filtro.NumeroPedido
        Return db.EjecutarDataTable("ObtenerDespachoSateliteMotorizado", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerPedidoPorId(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdPedido > 0 Then db.AgregarParametroSQL("@idPedido", filtro.IdPedido, SqlDbType.Int)
        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        Return db.EjecutarDataTable("ObtenerDespachoSateliteEnProceso", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerPedidoPorIdServicio(idServicio As Integer) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess
        db.SqlParametros.Add("@idServicio", SqlDbType.Int).Value = idServicio
        Return db.EjecutarDataTable("ObtenerPedidosDespachoSatelite", CommandType.StoredProcedure)
    End Function

    Public Function AnularSerialPedidoBodegasSatelite(idServicio As Integer, idUsuario As Integer, serial As String) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idServicio", SqlDbType.Int).Value = idServicio
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@serial", SqlDbType.VarChar, 50).Value = serial
                    .Add("@idPedidoSerial", SqlDbType.Decimal).Direction = ParameterDirection.Output
                    .Add("@tienePedidoSap", SqlDbType.Bit).Direction = ParameterDirection.Output
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("AnularSerialPedidoBodegasSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)

                If Not IsDBNull(.SqlParametros("@tienePedidoSap").Value) Then
                    tienePedidoSap = CBool(.SqlParametros("@tienePedidoSap").Value)
                End If

                If Not IsDBNull(.SqlParametros("@idPedidoSerial").Value) Then
                    IdPedido = CInt(.SqlParametros("@idPedidoSerial").Value.ToString)
                End If

                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Function AnularPedidoBodegasSatelite(idUsuario As Integer, Optional idServicio As Integer = 0) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    If idServicio > 0 Then .Add("@idServicio", SqlDbType.Int).Value = idServicio
                    If IdPedido > 0 Then .Add("@idPedido", SqlDbType.Int).Value = IdPedido
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("AnularPedidoBodegasSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)

                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Shared Function RegistrarFacturacionPedido(idServicio As Integer, idUsuario As Integer) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idServicio", SqlDbType.Int).Value = idServicio
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("RegistrarFacturacionPedido", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)

                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Function IngresarCantidadMaterialesNoserial() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idSubproducto", SqlDbType.Decimal).Value = IdSubProducto
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@cantIngresada", SqlDbType.Int).Value = CantidadIngresada
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("IngresarCantidadMaterialesNoSerializados", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)

                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Function EliminarCantidadMaterialesNoserial() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idSubproducto", SqlDbType.Decimal).Value = IdSubProducto
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idUsuario", SqlDbType.Decimal).Value = IdUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("EliminarCantidadNoSerializado", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Sub VerTotalRecoleccionLeido(ByVal filtros As FiltroDespachoSinPedidoSatelite)
        Dim db As New LMDataAccess
        db.SqlParametros.Add("@idBodega", SqlDbType.Int).Value = filtros.IdBodegaOrigen
        db.SqlParametros.Add("@numeroPedido", SqlDbType.VarChar).Value = filtros.NumeroPedido
        Try
            db.ejecutarReader("ObtenertotalPedidoLeido", CommandType.StoredProcedure)
            If db.Reader.Read Then
                NumeroPedido = filtros.NumeroPedido
                UnidadesRecogidas = db.Reader("cantidad").ToString
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        Finally
            If Not db.Reader.IsClosed Then db.Reader.Close()
            db.Dispose()
        End Try
    End Sub

    Public Function RegistarSerialPedido() As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtMaterial As New DataTable
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@numeroPedido", SqlDbType.VarChar, 100).Value = NumeroPedido
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@serial", SqlDbType.VarChar, 50).Value = Serial
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtMaterial = .EjecutarDataTable("IngresrSerialLeidoProductoPedido", CommandType.StoredProcedure)
                Short.TryParse(.SqlParametros("@result").Value.ToString, ResultMaterialRecoleccion)
                MsjMaterialRecoleccion = .SqlParametros("@mensaje").Value.ToString
                Dim resul As Integer = CType(.SqlParametros("@result").Value.ToString, Integer)
                If resul = 0 Then
                    .ConfirmarTransaccion()
                ElseIf resul = 2 Then
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
        Return dtMaterial
    End Function

    Public Function RegistarSerialesPedido(dtSeriales) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As New DataTable

        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@numeroPedido", SqlDbType.VarChar).Value = NumeroPedido
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@tbSerial", SqlDbType.Structured).Value = dtSeriales
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtErrores = .EjecutarDataTable("IngresrSerialesLeidoProductoPedido", CommandType.StoredProcedure)
                If dtErrores.Rows.Count = 0 Then
                    resultado.EstablecerMensajeYValor(0, "El archivo se proceso de correcta")
                    .ConfirmarTransaccion()
                Else
                    .AbortarTransaccion()
                    resultado.EstablecerMensajeYValor(1, "Se presentaron errores en el cargue del archivo")
                End If
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return dtErrores
    End Function

    Public Function RegistarCantidadesPedidoTraslado(dtCantidad) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As New DataTable
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@numeroPedido", SqlDbType.VarChar).Value = NumeroPedido
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@tbMaterial", SqlDbType.Structured).Value = dtCantidad
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtErrores = .EjecutarDataTable("IngresarCantidadesPedidoTraslado", CommandType.StoredProcedure)
                If dtErrores.Rows.Count = 0 Then
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
        Return dtErrores
    End Function
    Public Function IngresarProductoPedidoMasivo(dtSeriales, IdTipoPedido) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As New DataTable
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@tbListaproductoPedido", SqlDbType.Structured).Value = dtSeriales
                    .Add("@idBodegaOrigen", SqlDbType.Int).Value = IdBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idTipoPedido", SqlDbType.Int).Value = IdTipoPedido
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtErrores = .EjecutarDataTable("IngresarProductoPedidoMasivo", CommandType.StoredProcedure)
                If dtErrores.Rows.Count = 0 Then
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
        Return dtErrores
    End Function

    Public Function EliminarSerialPedido() As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim dtMaterial As New DataTable
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@numeroPedido", SqlDbType.VarChar).Value = NumeroPedido
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@serial", SqlDbType.VarChar).Value = Serial
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                dtMaterial = .EjecutarDataTable("EliminarSerialLeidoProductoPedido", CommandType.StoredProcedure)
                Short.TryParse(.SqlParametros("@result").Value.ToString, ResultMaterialRecoleccion)
                MsjMaterialRecoleccion = .SqlParametros("@mensaje").Value.ToString
                Dim resul As Integer = CType(.SqlParametros("@result").Value.ToString, Integer)
                If resul = 0 Then
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
        Return dtMaterial
    End Function

    Public Shared Function ObtenerMaterialDespachoLeidos(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.AgregarParametroSQL("@idPedido", filtro.IdPedido, SqlDbType.Decimal)
        db.AgregarParametroSQL("@idDespacho", filtro.IdDespacho, SqlDbType.Decimal)
        'db.AgregarParametroSQL("@idBodega", filtro.IdBodegaOrigen, SqlDbType.Int)
        Return db.EjecutarDataTable("ObtenerMaterialesDespachoLeidos", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerSerialesPorProducto(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.AgregarParametroSQL("@idDespacho", filtro.IdDespacho, SqlDbType.Decimal)
        db.AgregarParametroSQL("@idSubProducto", filtro.IdSubProducto, SqlDbType.Decimal)
        Return db.EjecutarDataTable("ObtenerSerialesDespachoSinPedido", CommandType.StoredProcedure)
    End Function

    Public Function CerrarDespachoSinPedido(filtro As FiltroDespachoSinPedidoSatelite) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@IdBodega", SqlDbType.Int).Value = filtro.IdBodegaOrigen
                    .Add("@numeroPedido", SqlDbType.VarChar).Value = filtro.NumeroPedido
                    .Add("@idUsuario", SqlDbType.Decimal).Value = filtro.IdUsuario
                    .Add("@idPedido", SqlDbType.Decimal).Value = filtro.IdPedido
                    .Add("@idDespacho", SqlDbType.Decimal).Value = filtro.IdDespacho
                    .Add("@idTipoPedido", SqlDbType.Int).Value = filtro.IdTipoPedido
                    .Add("@idTipoTransporte", SqlDbType.Int).Value = filtro.IdTipoTransporte
                    If filtro.IdTransportadora > 0 Then dbManager.SqlParametros.Add("@idTransPortadora", SqlDbType.Int).Value = filtro.IdTransportadora
                    If filtro.IdMotorizado > 0 Then dbManager.SqlParametros.Add("@idMotorizado", SqlDbType.Int).Value = filtro.IdMotorizado
                    .Add("@numeroGuia", SqlDbType.VarChar, 100).Value = filtro.NumeroGuia
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("CerrarDespachoSinPedido", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Function AnularDespachoSinPedidoEnProceso() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
                    .Add("@idDespacho", SqlDbType.Decimal).Value = IdDespacho
                    .Add("@idUsuario", SqlDbType.Decimal).Value = IdUsuario
                    .Add("@observacionAnular", SqlDbType.VarChar, 500).Value = ObservacionAnular
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("AnularDespachoEnProceso", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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
#End Region

#Region "Administracion bodegas por usuario"
    Public Function ObtenerBodegasAsignadasUsuario() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        With db
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@identificacionUsuario", SqlDbType.VarChar).Value = IdentificacionUsuario
                    .Add("@idUsuario", SqlDbType.Decimal, 200).Direction = ParameterDirection.Output
                    .Add("@idPerfil", SqlDbType.Decimal).Direction = ParameterDirection.Output
                    .Add("@usuario", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                    .Add("@nombre", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                    .Add("@identificacion", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output
                End With
                dtDatos = db.EjecutarDataTable("ObtenerBodegasPorUsuario", CommandType.StoredProcedure)
                If .SqlParametros("@result").Value > 0 Then
                    IdPerfil = .SqlParametros("@idPerfil").Value
                    IdUsuario = .SqlParametros("@idUsuario").Value
                    Nombre = .SqlParametros("@nombre").Value
                    Identificacion = .SqlParametros("@identificacion").Value
                    Usuario = .SqlParametros("@usuario").Value

                Else
                    IdPerfil = Nothing
                    IdUsuario = Nothing
                    Nombre = Nothing
                    Identificacion = Nothing
                    Usuario = Nothing
                End If
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End With
        Return dtDatos
    End Function

    Public Shared Function ObtenerBodegasDisponibles(numeroIdentificacion As String, idtipoBodega As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        If numeroIdentificacion <> "" Then db.SqlParametros.Add("@identificacionUsuario", SqlDbType.VarChar).Value = numeroIdentificacion
        If idtipoBodega > 0 Then db.SqlParametros.Add("@idTipoBodega", SqlDbType.Int).Value = idtipoBodega
        dtDatos = db.EjecutarDataTable("ObtenerBodegasDisponibles", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Function AsignarBodegaUsuario() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idUsuarioRegistra", SqlDbType.Int).Value = IdUsuarioRegistra
                    .Add("@idPerfil", SqlDbType.Int).Value = IdPerfil
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("AsignarBodegaUsuario", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
            Catch ex As Exception
                resultado.EstablecerMensajeYValor(33, ex.Message)
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Function EliminarBodegaUsuario() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idUsuarioRegistra", SqlDbType.Int).Value = IdUsuarioRegistra
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .EjecutarNonQuery("EliminarAsignacionBodegaUsuario", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
            Catch ex As Exception
                resultado.EstablecerMensajeYValor(33, ex.Message)
                Throw New Exception(ex.Message, ex)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End With
        Return resultado
    End Function

    Public Shared Function ObtieneTiposDeBodega() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        dtDatos = db.EjecutarDataTable("ObtenerTiposDeBodega", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Function ObtieneTiposDeBodegaCiclico() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
        dtDatos = db.EjecutarDataTable("ObtenerTiposDeBodegaCiclico", CommandType.StoredProcedure)
        Return dtDatos
    End Function
#End Region

    Public Shared Function ObtenerMaterialesSerialesTraslado(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess

        'db.AgregarParametroSQL("@idPedido", filtro.IdPedido, SqlDbType.Decimal)
        db.AgregarParametroSQL("@idDespacho", filtro.IdDespacho, SqlDbType.Decimal)
        'db.AgregarParametroSQL("@idBodega", filtro.IdBodegaOrigen, SqlDbType.Int)
        Return db.EjecutarDataTable("ObtenerMaterialSerialesDespachoTraslado", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerDespachosSateliteRemision(filtro As FiltroDespachoSinPedidoSatelite, opcion As Integer) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.AgregarParametroSQL("@opcion", opcion, SqlDbType.Decimal)
        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodegaOrigen", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.IdTipoPedido > 0 Then db.SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = filtro.IdTipoPedido
        If filtro.IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = filtro.IdEstado
        If filtro.NumeroPedido <> "" Then db.SqlParametros.Add("@numeroPedido", SqlDbType.VarChar, 50).Value = filtro.NumeroPedido
        Return db.EjecutarDataTable("ObtenerDespachoSateliteRemision", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerDespachosSateliteRemisionDetalle(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodegaUsuario", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.IdTipoPedido > 0 Then db.SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = filtro.IdTipoPedido
        If filtro.IdEstado > 0 Then db.SqlParametros.Add("@idEstado", SqlDbType.Int).Value = filtro.IdEstado
        If filtro.NumeroPedido <> "" Then db.SqlParametros.Add("@numeroPedido", SqlDbType.VarChar, 50).Value = filtro.NumeroPedido
        Return db.EjecutarDataTable("ObtenerReportePedidoRemisionDetalle", CommandType.StoredProcedure)
    End Function

    Public Function ModificarSerialBodegaSatelite() As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@IdBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@serialActual", SqlDbType.VarChar, 50).Value = Serial
                    .Add("@serialNuevo", SqlDbType.VarChar, 50).Value = SerialNuevo
                    .Add("@usuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("ModificarSerialSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Function ObtenerHistorialSerial() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        With db
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar, 50).Value = Serial
                End With
                dtDatos = db.EjecutarDataTable("ObtenerHistoriaSerialSatelite", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End With
        Return dtDatos
    End Function

    Public Function ObtenerEstadoActualSerial() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        With db
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@serial", SqlDbType.VarChar, 50).Value = Serial
                End With
                dtDatos = db.EjecutarDataTable("ObtenerEstadoActualSerialSatelite", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception(ex.Message, ex)
            End Try
        End With
        Return dtDatos
    End Function

    Public Shared Function ObtenerSerialesIncumplimientoPeps(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdPedido > 0 Then db.SqlParametros.Add("@idPedido", SqlDbType.Decimal).Value = filtro.IdPedido
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        Return db.EjecutarDataTable("ObtenerSerialesIncumplimientoPeps", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerEdadMaterialesBodegaSatelite(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@listIntervalos", SqlDbType.VarChar).Value = filtro.ListRango
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodega", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.IdSubProducto2 > 0 Then db.SqlParametros.Add("@idSubproducto", SqlDbType.VarChar, 50).Value = filtro.IdSubProducto2
        If filtro.IdProducto > 0 Then db.SqlParametros.Add("@idProducto", SqlDbType.Decimal).Value = filtro.IdProducto
        Return db.EjecutarDataTable("ObtenerEdadMaterialesBodegaSatelite", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerBodegasUsuarioPedido(idUsuario As Integer, idtipoBodega As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        If idUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
        If idtipoBodega > 0 Then db.SqlParametros.Add("@idTipoBodega", SqlDbType.Int).Value = idtipoBodega
        dtDatos = db.EjecutarDataTable("ObtenerBodegasPorUsuarioPedido", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Shared Function ObtenerDespachosEnProcesoCliente(filtro As FiltroDespachoSinPedidoSatelite, opcion As Integer) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.NumeroDocumentoCliente IsNot Nothing Then db.SqlParametros.Add("@documentoCliente", SqlDbType.VarChar, 50).Value = filtro.NumeroDocumentoCliente
        If filtro.FechaIncio <> Nothing Then db.SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = filtro.FechaIncio
        If filtro.FechaFin <> Nothing Then db.SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = filtro.FechaFin
        If Not String.IsNullOrEmpty(filtro.ListaBodegas) Then db.SqlParametros.Add("@ListaBodegas", SqlDbType.VarChar).Value = filtro.ListaBodegas
        db.SqlParametros.Add("@opcion", SqlDbType.SmallInt).Value = opcion
        Return db.EjecutarDataTable("ObtenerDespachoSateliteEnProcesoPorCliente", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerMaterialesDespachoCliente(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.AgregarParametroSQL("@idPedido", filtro.IdPedido, SqlDbType.Decimal)

        Return db.EjecutarDataTable("ObtenerMaterialesDespachoLeidosCliente", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerDespachosEnProcesoReporte(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodegaOrigen", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.IdTipoPedido > 0 Then db.SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = filtro.IdTipoPedido
        If filtro.NumeroPedido IsNot Nothing Then db.SqlParametros.Add("@pedido", SqlDbType.VarChar, 200).Value = filtro.NumeroPedido
        Return db.EjecutarDataTable("ObtenerDespachoSateliteEnProcesoReporte", CommandType.StoredProcedure)
    End Function

    Public Sub ObtenerInfoDocumentoNacionalizacion(ByVal idDocumento As Integer)

        Dim db As New LMDataAccess
        db.AgregarParametroSQL("@idDocumento", idDocumento)
        Dim dReader As SqlClient.SqlDataReader = db.ejecutarReader("ObtenerInfoDocumentoNacionalizacion", CommandType.StoredProcedure)
        If dReader.Read() Then
            Try
                _idRegistro = dReader("idRegistro").ToString()
                _nombreArchivo = dReader("nombreArchivo").ToString()
                _rutaAlmacenamiento = dReader("rutaAlmacenamiento").ToString()
                _idOrdenDeRecepcion = dReader("idOrdenRecepcion").ToString()
                _tipoContenido = dReader("tipoContenido").ToString()

            Finally
                dReader.Close()
                db.CerrarConexion()
            End Try
        End If
    End Sub

    Public Function ConsultarBodegasPorUsuario(idUsuario As Integer) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable

        If idUsuario > 0 Then db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario

        dtDatos = db.EjecutarDataTable("ConsultarBodegasPorUsuario", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Shared Function ObtenerDespachosAsignacionGuia(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodegaOrigen", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.NumeroPedido IsNot Nothing Then db.SqlParametros.Add("@pedido", SqlDbType.VarChar, 200).Value = filtro.NumeroPedido
        db.SqlParametros.Add("@esSinAsignacionGuia", SqlDbType.Bit).Value = filtro.EsAsignacionGuia
        Return db.EjecutarDataTable("ObtenerDespachoSateliteAsignacionGuia", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerDespachosAsignacionGuiaReporte(filtro As FiltroDespachoSinPedidoSatelite) As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        If filtro.IdUsuario > 0 Then db.AgregarParametroSQL("@idUsuario", filtro.IdUsuario, SqlDbType.Int)
        If filtro.FechaIncio <> Date.MinValue Then db.SqlParametros.Add("@fechaInicial", SqlDbType.SmallDateTime).Value = filtro.FechaIncio
        If filtro.FechaFin <> Date.MinValue Then db.SqlParametros.Add("@fechaFinal", SqlDbType.SmallDateTime).Value = filtro.FechaFin
        If filtro.IdBodegaOrigen > 0 Then db.SqlParametros.Add("@idBodegaOrigen", SqlDbType.Int).Value = filtro.IdBodegaOrigen
        If filtro.NumeroPedido IsNot Nothing Then db.SqlParametros.Add("@pedido", SqlDbType.VarChar, 200).Value = filtro.NumeroPedido
        db.SqlParametros.Add("@esSinAsignacionGuia", SqlDbType.Bit).Value = filtro.EsAsignacionGuia
        db.SqlParametros.Add("@opcion", SqlDbType.SmallInt).Value = filtro.opcion
        Return db.EjecutarDataTable("ObtenerDespachoGuiasReporte", CommandType.StoredProcedure)
    End Function

    Public Shared Function ObtenerTransportadorasActivas() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess
        Return db.EjecutarDataTable("ObtenerTransportadoras", CommandType.StoredProcedure)
    End Function

    Public Shared Function AdicionarCantidadesOtbAOtbBodegaSatelite(filtro As FiltrosOTB) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = filtro.IdUsuario
                    .Add("@idOtbOrigen", SqlDbType.Decimal).Value = filtro.IdOTBOrigen
                    .Add("@idOtbDestino", SqlDbType.Decimal).Value = filtro.IdOTBDestino
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("AdicionarCantidadOtbAOtbBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
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

    Public Function AdicionarSerialesAOtbBodegaSatelite(filtro As FiltrosOTB) As ResultadoProceso
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New ResultadoProceso
        With dbManager
            Try
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Decimal).Value = filtro.IdUsuario
                    .Add("@serial", SqlDbType.Decimal).Value = filtro.Serial
                    .Add("@idOtbDestino", SqlDbType.Decimal).Value = filtro.IdOTBDestino
                    .Add("@cantidadLeida", SqlDbType.Int).Direction = ParameterDirection.Output
                    .Add("@mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output
                    .Add("@result", SqlDbType.BigInt).Direction = ParameterDirection.Output
                End With
                .IniciarTransaccion()
                .TiempoEsperaComando = 0
                .EjecutarNonQuery("AdicionarSerialesAOTBBodegaSatelite", CommandType.StoredProcedure)
                resultado.EstablecerMensajeYValor(.SqlParametros("@result").Value.ToString, .SqlParametros("@mensaje").Value.ToString)
                Dim res As Integer = CInt(.SqlParametros("@result").Value.ToString)
                If res = 0 Then
                    .ConfirmarTransaccion()
                    CantidadIngresada = .SqlParametros("@cantidadLeida").Value
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

    Public Function ObtenerSerialesPedidoBodegaSatelite() As DataTable
        Dim resultado As New ResultadoProceso
        Dim db As New LMDataAccessLayer.LMDataAccess

        db.SqlParametros.Add("@idPedido", SqlDbType.Decimal).Value = IdPedido
        db.SqlParametros.Add("@idSubproducto", SqlDbType.Decimal).Value = IdSubProducto

        Return db.EjecutarDataTable("ObtenerSerialesPedidoBodegaSatelite", CommandType.StoredProcedure)
    End Function

End Class
