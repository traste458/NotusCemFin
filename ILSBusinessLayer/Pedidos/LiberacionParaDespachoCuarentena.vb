Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Comunes

Public Class LiberacionParaDespachoCuarentena

    Inherits Pedidos.Pedido

#Region "Atributos"
    Private _serial As String
    Private _idOtb As String
    Private _tipoLectura As Short
    Private _resultado As ResultadoProceso
    Private _idUsuario As String
    Private _estaReservado As Boolean
    Private _idDetalleCuarentena As Integer
    Private _idDetalleLiberacion As Integer
    Private _documentoCambioEstadoSAP As String

#End Region

#Region "Propiedades"

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
        End Set
    End Property

    Public Property IdOTB() As Integer
        Get
            Return _idOtb
        End Get
        Set(ByVal value As Integer)
            _idOtb = value
        End Set
    End Property

    Public Property TipoLectura() As Short
        Get
            Return _tipoLectura
        End Get
        Set(ByVal value As Short)
            _tipoLectura = value
        End Set
    End Property

    Public Property IdUsusario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public ReadOnly Property EstaReservado() As Boolean
        Get
            Return _estaReservado
        End Get
    End Property

    Public ReadOnly Property Resultado() As ResultadoProceso
        Get
            Return _resultado
        End Get
    End Property

    Public Property IdDetalleCuarentena() As Integer
        Get
            Return _idDetalleCuarentena
        End Get
        Set(ByVal value As Integer)
            _idDetalleCuarentena = value
        End Set
    End Property

    Public Property IdDetalleiberacion() As Integer
        Get
            Return _idDetalleLiberacion
        End Get
        Set(ByVal value As Integer)
            _idDetalleLiberacion = value
        End Set
    End Property

    Public Property DocumentoCambioEstadoSAP() As String
        Get
            Return _documentoCambioEstadoSAP
        End Get
        Set(ByVal value As String)
            _documentoCambioEstadoSAP = value
        End Set
    End Property

#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idPedidoLiberacion As Integer)
        MyBase.New()
        IdPedido = idPedidoLiberacion
    End Sub
#End Region

#Region "Metodos Privados"

#End Region

#Region "Metodos Publicos"

    Public Shared Function ObtenerDetalleCuarentenaDisponible(ByVal filtro As FiltroPedido) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtDetalle As DataTable
        Try
            With dbManager
                With .SqlParametros
                    If filtro.IdPedido > 0 Then dbManager.agregarParametroSQL("@idPedido", filtro.IdPedido, SqlDbType.Int)
                    If filtro.IdPedidoDespacho > 0 Then dbManager.agregarParametroSQL("@idPedidoDespacho", filtro.IdPedidoDespacho, SqlDbType.Int)
                    If filtro.EsEdicionLiberacionCuarentena = 1 Then dbManager.agregarParametroSQL("@esEdicionLiberacion", filtro.EsEdicionLiberacionCuarentena, SqlDbType.Bit)
                End With
                dtDetalle = .ejecutarDataTable("ObtenerDetalleCuarentenaDisponible", CommandType.StoredProcedure)

                Dim pk(1) As DataColumn
                pk(0) = dtDetalle.Columns("material")
                pk(1) = dtDetalle.Columns("idRegion")
                dtDetalle.PrimaryKey = pk
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDetalle
    End Function

    Public Sub AdicionarReservaSerialParaDespacho()
        Dim dbManager As New LMDataAccess
        Dim cantResultado As Integer
        Try
            _resultado = New ResultadoProceso
            _estaReservado = True
            With dbManager
                EstablecerParametrosReserva(dbManager)
                cantResultado = .ejecutarScalar("ActualizaReservaSerialEnCuarentenaParaDespacho", CommandType.StoredProcedure)
                _resultado.Valor = CInt(.SqlParametros.Item("@resultado").Value)
                If _resultado.Valor = 0 Then
                    If _tipoLectura = 1 Then
                        _resultado.EstablecerMensajeYValor(_resultado.Valor, "Se ha reservado correctamente el serial " & _serial)
                    ElseIf _tipoLectura = 2 Then
                        _resultado.EstablecerMensajeYValor(_resultado.Valor, "Se ha reservado correctamente " & cantResultado & " seriale(s) de la OTB " & _idOtb)
                    End If
                ElseIf _resultado.Valor = 1 Then
                    _resultado.EstablecerMensajeYValor(_resultado.Valor, "No se recibio ningun dato para la lectura.")
                Else
                    _resultado.EstablecerMensajeYValor(_resultado.Valor, "Ha ocurrido un error durante la lectura")
                End If
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Sub EliminarReservaSerialParaDespacho()
        Dim dbManager As New LMDataAccess
        Dim cantResultado As Integer
        Try
            _resultado = New ResultadoProceso
            _estaReservado = False
            With dbManager
                EstablecerParametrosReserva(dbManager)
                cantResultado = .ejecutarScalar("ActualizaReservaSerialEnCuarentenaParaDespacho", CommandType.StoredProcedure)
                _resultado.Valor = CInt(.SqlParametros.Item("@resultado").Value)
                If _resultado.Valor = 0 Then
                    If _tipoLectura = 1 Then
                        _resultado.EstablecerMensajeYValor(_resultado.Valor, "Se borrado correctamente el serial " & _serial)
                    ElseIf _tipoLectura = 2 Then
                        _resultado.EstablecerMensajeYValor(_resultado.Valor, "Se han borrado correctamente " & cantResultado & " seriales de la OTB " & _idOtb)
                    End If
                ElseIf _resultado.Valor = 1 Then
                    _resultado.EstablecerMensajeYValor(_resultado.Valor, "No se recibio ningun dato para la lectura.")
                Else
                    _resultado.EstablecerMensajeYValor(_resultado.Valor, "Ha ocurrido un error durante la lectura.")
                End If
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Public Function ValidarLecturaSerialParaReservar(ByVal idDetalle As Integer, ByVal codigo As String, ByVal tipoLectura As Integer) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtResultado As New DataTable
        Try
            _resultado = New ResultadoProceso
            With dbManager
                .SqlParametros.Add("@idDetalleLiberacion", SqlDbType.Int).Value = idDetalle
                .SqlParametros.Add("@tipoLectura", SqlDbType.SmallInt).Value = tipoLectura
                .SqlParametros.Add("@codigo", SqlDbType.VarChar, 20).Value = codigo

                dtResultado = .ejecutarDataTable("ValidarSerialEnCuarentenaParaReservaDeDespacho", CommandType.StoredProcedure)
                If dtResultado.Rows.Count = 0 Then
                    _resultado.EstablecerMensajeYValor(0, "")
                ElseIf dtResultado.Rows.Count >= 1 And tipoLectura = 1 Then
                    _resultado.EstablecerMensajeYValor(1, "El serial " & codigo & " pose una o más condiciones no validas para la lectura, Ver log de errores.")
                Else
                    _resultado.EstablecerMensajeYValor(1, "Uno o más seriales de la OTB " & codigo & " no cumplen con las condiciones necesarias. Ver log de errores.")
                End If
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return dtResultado
    End Function

    Public Function ValidarLecturaBorrarSerialReservado(ByVal idDetalle As Integer, ByVal codigo As String, ByVal tipoLectura As Integer) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtResultado As New DataTable
        Dim msjResutado As String = String.Empty
        Try
            _resultado = New ResultadoProceso
            With dbManager
                .SqlParametros.Add("@idDetalleLiberacion", SqlDbType.Int).Value = idDetalle
                .SqlParametros.Add("@tipoLectura", SqlDbType.SmallInt).Value = tipoLectura
                .SqlParametros.Add("@codigo", SqlDbType.VarChar, 20).Value = codigo

                dtResultado = .ejecutarDataTable("ValidarSerialEnCuarentenaParaQuitarReservaDeDespacho", CommandType.StoredProcedure)
                If dtResultado.Rows.Count = 0 Then
                    _resultado.EstablecerMensajeYValor(0, "")
                ElseIf dtResultado.Rows.Count = 1 Then
                    _resultado.EstablecerMensajeYValor(1, dtResultado.Rows(0).Item(1).ToString)
                Else
                    _resultado.EstablecerMensajeYValor(1, "Uno o más seriales de la OTB " & codigo & " no se pueden borrar. Ver log de errores.")
                End If

            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return dtResultado
    End Function

    Public Sub EstablecerParametrosReserva(ByRef dm As LMDataAccess)
        Try
            With dm
                .SqlParametros.Add("@esReservado", SqlDbType.Bit).Value = _estaReservado
                .SqlParametros.Add("@idDetalleLiberacion", SqlDbType.BigInt).Value = _idDetalleLiberacion
                .SqlParametros.Add("@tipoLectura", SqlDbType.Int).Value = _tipoLectura
                If _serial IsNot Nothing AndAlso _serial.Trim.Length > 0 Then
                    .SqlParametros.Add("@codigo", SqlDbType.VarChar, 20).Value = _serial
                ElseIf _idOtb <> 0 Then
                    .SqlParametros.Add("@codigo", SqlDbType.VarChar, 20).Value = _idOtb
                End If
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            End With
        Catch ex As Exception
            Throw New Exception("Error estableciendo parametros, " & ex.Message)
        End Try
    End Sub

    Public Function LiberacionCuarentenaSAP(ByVal dtInfoSerialesCuarentena As DataTable) As DataTable
        Dim cambioDeEstado As New CambioDeEstadoSAP
        Dim dtResultado As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess

        _resultado = New ResultadoProceso
        _resultado.EstablecerMensajeYValor(0, "Ejecución Satisfactoria")

        If IdPedido > 0 Then
            If dtInfoSerialesCuarentena IsNot Nothing AndAlso dtInfoSerialesCuarentena.Rows.Count > 0 Then
                With cambioDeEstado
                    .IdPedido = IdPedido
                    .TipoCambio = CambioDeEstadoSAP.Tipo.LiberacionCuarentena
                    .InfoSeriales = dtInfoSerialesCuarentena
                    .ValeMaterial = "CAMBIO STOCK"
                    .TextoCabecera = "Pedido " & IdPedido.ToString
                    .StockOrigen = CambioDeEstadoSAP.TipoStock.ControlCalidad
                    .StockDestino = CambioDeEstadoSAP.TipoStock.LibreUtilizacion
                    _resultado = .GenerarCambio()
                    dtResultado = .InfoErrores

                    If _resultado.Valor = 0 Then
                        _documentoCambioEstadoSAP = .DocumentoSAP
                        dtInfoSerialesCuarentena = .InfoSeriales
                        _resultado = RegistrarDocumentoSAPLiberacion(dbManager, .DocumentoSAP)
                    End If
                End With
            Else
                _resultado.Valor = 5
                _resultado.Mensaje = "No se pudieron obtener los seriales de la cuarentena para cambiar el estado en SAP como Liberado. "
            End If
        Else
            _resultado.Valor = 4
            _resultado.Mensaje = "No fue posible obtener el pedido para realizar el cambio de estado en SAP. "
        End If

        Return dtResultado
    End Function

    Private Function RegistrarDocumentoSAPLiberacion(ByVal dbManager As LMDataAccessLayer.LMDataAccess, ByVal documentoSAP As String) As ResultadoProceso
        Dim rp As New ResultadoProceso
        rp.EstablecerMensajeYValor(0, "Ejecución Satisfactoria")
        With dbManager
            With .SqlParametros
                .Clear()
                .Add("@documentoLiberacion", SqlDbType.VarChar, 20).Value = documentoSAP
                .Add("@idPedido", SqlDbType.BigInt).Value = IdPedido
                .Add("@idDetallePedido", SqlDbType.BigInt).Value = IdDetallePedido
                .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
            End With

            .ejecutarNonQuery("ActualizarDocumentoSAPLiberacion", CommandType.StoredProcedure)

            rp.Valor = CShort(dbManager.SqlParametros("@returnValue").Value)
            If rp.Valor <> 0 Then
                Select Case rp.Valor
                    Case 1 : rp.EstablecerMensajeYValor(rp.Valor, "No fue posible obtener los seriales liberados de cuarentena para actualizar el documento de cambio de estado. ")
                    Case 2 : rp.EstablecerMensajeYValor(rp.Valor, "Error al actualizar el documento cambio de estado SAP en el pedido de Liberación de cuarentena. ")
                End Select
            End If
        End With

        Return rp
    End Function


#End Region

#Region "Metodos Compartidos"

    Public Shared Function ObtenerListaSerial(ByVal filtro As FiltroDetalleCuarentena) As DataTable
        Dim dbManager As New LMDataAccess
        Dim dtDatos As DataTable
        Try
            With dbManager
                With .SqlParametros
                    If filtro.Serial IsNot Nothing AndAlso filtro.Serial.Trim.Length > 0 Then .Add("@Serial", SqlDbType.VarChar, 20).Value = filtro.Serial
                    If filtro.IdDetallePedido > 0 Then .Add("@idDetallePedido", SqlDbType.Int).Value = filtro.IdDetallePedido
                    If filtro.IdPedido > 0 Then .Add("@idPedido", SqlDbType.Int).Value = filtro.IdPedido
                End With
                dtDatos = .ejecutarDataTable("ObtenerInfoSerialesLiberadosParaDespacho", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class