Imports LMDataAccessLayer
Namespace Pedidos

    Public Class DetallePicking

        Private _idDetalle As Integer
        Private _idPicking As Integer
        Private _detalle As DataTable
        Private _dbManager As LMDataAccess
        Private _dtErrores As New DataTable
        Private _ResultadoTransaccion As New ResultadoProceso

        Sub New()
            MyBase.New()
        End Sub

        Sub New(ByVal idPicking As Integer)
            MyBase.New()
            CargarInformacion(idPicking)
        End Sub

        Public ReadOnly Property Detalle() As DataTable
            Get
                If _detalle Is Nothing Then _detalle = CrearEstructuraDetalle()
                Return _detalle
            End Get
        End Property

        Public ReadOnly Property IdDetalle() As Integer
            Get
                Return _idDetalle
            End Get

        End Property

        Public ReadOnly Property IdPicking() As Integer
            Get
                Return _idPicking
            End Get
        End Property

        Public Property LogErrores() As DataTable
            Get
                Return _dtErrores
            End Get
            Set(ByVal value As DataTable)
                _dtErrores = value
            End Set
        End Property
        Public ReadOnly Property ResultadoTransaccuion() As ResultadoProceso
            Get
                Return _ResultadoTransaccion
            End Get
        End Property


        Public Function Crear(ByVal idPedido As Integer, ByVal idPicking As Integer, ByVal idTipoPedido As Integer, _
                              ByVal dbManager As LMDataAccess) As ResultadoProceso
            Dim dtDetallePedido As DataTable
            Dim tipoPedido As Pedidos.TipoPedido.Tipo

            Try

                If idTipoPedido = tipoPedido.SalidaDeProductoPruebas Then
                    With dbManager
                        .agregarParametroSQL("@idPedido", idPedido, SqlDbType.Int)
                        .agregarParametroSQL("@idPicking", idPicking, SqlDbType.Int)
                        .SqlParametros.Add("@idError", SqlDbType.SmallInt).Direction = ParameterDirection.Output
                        .ejecutarNonQuery("CrearDetallePickingPruebas", CommandType.StoredProcedure)
                        _ResultadoTransaccion.Valor = .SqlParametros("@idError").Value
                    End With

                ElseIf idTipoPedido = tipoPedido.DespachoCuarentena Then

                    Dim dtDetalle As New DataTable
                    dtDetalle = CrearEstructuraOTBs()

                    ' Se obtiene el detalle del pedido para obtener las otbs necesarias 
                    ' para cumplir con la cantidad pedido por cada detalle del pedido
                    With dbManager
                        .agregarParametroSQL("@idPedido", idPedido, SqlDbType.Int)
                        dtDetallePedido = .ejecutarDataTable("ObtenerDetalleCuarentenaDespacho", CommandType.StoredProcedure)
                    End With

                    If dtDetallePedido IsNot Nothing AndAlso dtDetallePedido.Rows.Count > 0 Then

                        Dim cantidadPedida, cantidadDisponible As Integer

                        For Each filaDetalle As DataRow In dtDetallePedido.Rows

                            Integer.TryParse(filaDetalle("cantidad").ToString, cantidadPedida)
                            With dbManager
                                While cantidadPedida > 0
                                    .SqlParametros.Clear()
                                    .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = idPedido
                                    .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Value = filaDetalle("material")

                                    ''para pedidos de consumo y de liberació de cuarentena no se valida la OTB por región
                                    If idTipoPedido <> tipoPedido.SalidaDeConsumos And idTipoPedido <> tipoPedido.LiberacionCuarentena Then
                                        .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = filaDetalle("idRegion")
                                    End If

                                    ''Consulta la primera OTB con cantidad disponible para cubrir la cantidad solicitada
                                    ''si la cantidad de la otb no es suficiente se consuslta la siguiente
                                    Dim arrParametrosAuxOtb As New ArrayList
                                    .ejecutarReader("ObtenerOtbPickingCuarentena", CommandType.StoredProcedure)

                                    If .Reader IsNot Nothing And .Reader.HasRows Then
                                        If .Reader.Read Then
                                            Integer.TryParse(.Reader("cantidad").ToString, cantidadDisponible)
                                            If cantidadPedida <= cantidadDisponible Then
                                                arrParametrosAuxOtb.Add(cantidadPedida)
                                                cantidadPedida = 0
                                            Else
                                                arrParametrosAuxOtb.Add(cantidadDisponible)
                                                cantidadPedida = cantidadPedida - cantidadDisponible
                                            End If
                                            arrParametrosAuxOtb.Add(.Reader("idOrdenBodegaje"))
                                            arrParametrosAuxOtb.Add(.Reader("idposicion"))
                                            arrParametrosAuxOtb.Add(.Reader("Material"))
                                            arrParametrosAuxOtb.Add(.Reader("idRegion"))
                                            arrParametrosAuxOtb.Add(.Reader("fechaRecepcion"))
                                            arrParametrosAuxOtb.Add(.Reader("lote"))
                                            arrParametrosAuxOtb.Add(idPicking)
                                        End If

                                    Else
                                        _ResultadoTransaccion.Valor = 1
                                    End If
                                    If .Reader IsNot Nothing Then .Reader.Close()

                                    .SqlParametros.Clear()
                                    If arrParametrosAuxOtb.Count > 0 Then
                                        .SqlParametros.Add("@cantidad", SqlDbType.Int).Value = arrParametrosAuxOtb.Item(0)
                                        .SqlParametros.Add("@idOrdenBodegaje", SqlDbType.Int).Value = arrParametrosAuxOtb.Item(1)
                                        .SqlParametros.Add("@idposicion", SqlDbType.Int).Value = arrParametrosAuxOtb.Item(2)
                                        .SqlParametros.Add("@Material", SqlDbType.VarChar, 20).Value = arrParametrosAuxOtb.Item(3)
                                        .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = arrParametrosAuxOtb.Item(4)
                                        .SqlParametros.Add("@fechaRecepcion", SqlDbType.SmallDateTime).Value = arrParametrosAuxOtb.Item(5)
                                        .SqlParametros.Add("@lote", SqlDbType.VarChar, 20).Value = arrParametrosAuxOtb.Item(6)
                                        .SqlParametros.Add("@idPicking", SqlDbType.Int).Value = arrParametrosAuxOtb.Item(7)
                                        .ejecutarNonQuery("CrearDetalleAuxiliarOTBPicking", CommandType.StoredProcedure)
                                    Else
                                        _ResultadoTransaccion.Valor = 3
                                        _ResultadoTransaccion.Mensaje = "No se encontraron cantidades suficientes para el material " & filaDetalle("material")
                                        Exit While
                                    End If
                                End While
                            End With
                            If _ResultadoTransaccion.Mensaje.Trim.Length > 0 Then RegistrarLogErrores(idPedido, _ResultadoTransaccion.Mensaje, filaDetalle("material"))
                        Next
                    Else
                        RegistrarLogErrores(idPedido, "No se encontro detalle del pedido")
                    End If


                    ''Despues de recorrer el detalle del pedido y crear el detalle auxiliar del picking se procede a 
                    ''registralo en la tabla principal
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idPicking", SqlDbType.Int).Value = idPicking
                        .SqlParametros.Add("@idError", SqlDbType.SmallInt).Direction = ParameterDirection.Output
                        .ejecutarNonQuery("CrearDetallePicking", CommandType.StoredProcedure)
                        _ResultadoTransaccion.Valor = .SqlParametros("@idError").Value

                    End With

                End If

                If _ResultadoTransaccion.Valor = 1 Then
                    _ResultadoTransaccion.Mensaje = "No se encontró detalle OTBs para crear detalle del pickinglist."
                ElseIf _ResultadoTransaccion.Valor = 2 Then
                    _ResultadoTransaccion.Mensaje = "No se registro detalle del pickinglist para pedido " & idPedido
                ElseIf _ResultadoTransaccion.Valor = 3 Then
                    _ResultadoTransaccion.Mensaje = "Los productos del pedido no se encuentran disponibles y/o nacionalizados."
                ElseIf _ResultadoTransaccion.Valor = 4 Then
                    _ResultadoTransaccion.Mensaje = "No se actualizo el pedido a su estado correspondiente."
                ElseIf _ResultadoTransaccion.Valor = -1 Then
                    _ResultadoTransaccion.Mensaje = "Error no identificado al generar detalle del pickinglist."
                End If

                If _ResultadoTransaccion.Mensaje.Trim.Length > 0 And _ResultadoTransaccion.Valor <> 0 Then
                    RegistrarLogErrores(idPedido, _ResultadoTransaccion.Mensaje)
                End If

            Catch ex As Exception
                _ResultadoTransaccion.Valor = -1
                _ResultadoTransaccion.Mensaje = ex.Message
            Finally
                If dbManager.Reader IsNot Nothing Then dbManager.Reader.Close()
            End Try
            Return _ResultadoTransaccion
        End Function

        Private Sub CargarInformacion(ByVal IdPicking As Integer)
            Dim dbManager As New LMDataAccessLayer.LMDataAccess
            Try
                _idPicking = IdPicking
                dbManager.agregarParametroSQL("@idPicking", IdPicking, SqlDbType.Int)
                _detalle = dbManager.ejecutarDataTable("ObtenerDetallePicking", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Private Function CrearEstructuraDetalle() As DataTable
            Dim dtAux As New DataTable
            With dtAux.Columns
                .Add("idPicking", GetType(Integer))
                .Add("idPedidoDetalle", GetType(Integer))
                .Add("idOrdenBodegaje", GetType(Integer))
                .Add("idPosicionBodega", GetType(Short))
                .Add("cantidad", GetType(Integer))
                .Add("fechaInicial", GetType(String))
                .Add("fechaFinal", GetType(String))
                .Add("lote", GetType(String))
            End With
            Return dtAux
        End Function

        Private Function CrearEstructuraOTBs() As DataTable
            Dim dtAux As New DataTable
            With dtAux.Columns
                .Add("idOrdenBodegaje", GetType(Integer))
                .Add("idPosicion", GetType(Short))
                .Add("Material", GetType(String))
                .Add("idRegion", GetType(Integer))
                .Add("fechaRecepcion", GetType(String))
                .Add("cantidad", GetType(Integer))
                .Add("lote", GetType(String))
            End With
            Return dtAux
        End Function

        Public Sub RegistrarLogErrores(ByVal idPedido As Integer, ByVal descripcion As String, Optional ByVal material As String = "")
            If _dtErrores Is Nothing Or _dtErrores.Rows.Count = 0 Then _dtErrores = EstablecerEstructuraLogErrores()
            Dim drRegistro As DataRow
            Try
                drRegistro = _dtErrores.NewRow
                With _dtErrores
                    drRegistro.Item("idPedido") = idPedido
                    drRegistro.Item("descripcion") = descripcion
                    drRegistro.Item("material") = material
                    .Rows.Add(drRegistro)
                    .AcceptChanges()
                End With
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub

        Private Function EstablecerEstructuraLogErrores() As DataTable
            Dim dt As New DataTable
            With dt
                .Columns.Add("idPedido")
                .Columns.Add("material")
                .Columns.Add("descripcion")
            End With
            Return dt
        End Function

    End Class

End Namespace