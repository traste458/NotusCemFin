Imports LMDataAccessLayer
Namespace CEMService

    Public Class ServicioMensajeriaTiendaVirtual
#Region "Atributos"

        Private _cabeceraPedido As InfoPedidoCEM
        Private _resultadoPedido As ResultadoPedido
        Private _detallePedido As DataTable
#End Region

#Region "Propiedades"

        Public Property CabeceraPedido As InfoPedidoCEM
            Get
                If _cabeceraPedido Is Nothing Then _cabeceraPedido = New InfoPedidoCEM
                Return _cabeceraPedido
            End Get
            Set(value As InfoPedidoCEM)
                _cabeceraPedido = value
            End Set
        End Property

        Public Property ResultadoPedido As ResultadoPedido
            Get
                If _resultadoPedido Is Nothing Then _resultadoPedido = New ResultadoPedido
                Return _resultadoPedido
            End Get
            Set(value As ResultadoPedido)
                _resultadoPedido = value
            End Set
        End Property

        Public Property DetallePedido As DataTable
            Get
                If _detallePedido Is Nothing Then _detallePedido = ObtenerTablaDetallePedido()
                Return _detallePedido
            End Get
            Set(value As DataTable)
                _detallePedido = value
            End Set
        End Property
#End Region

#Region "Constructores"

        Public Sub New()

        End Sub

        Public Sub New(numeroPedidoSap As Long, NumeroEntregaSap As Long,
                       NombreApellido As String,
                       NombreApellidoAutorizadoRecibir As String,
                       IdentificacionCliente As String,
                       DireccionEntrega As String,
                       ObservacionDireccion As String,
                       Barrio As String,
                       CodigoCiudadEntrega As Integer,
                       TelefonoContacto1 As String,
                       TelefonoContacto2 As String,
                       FechaHoraAutorizacionCompra As DateTime, DetalleArticulos As List(Of DetallePedidoCEM))


            If _cabeceraPedido Is Nothing Then _cabeceraPedido = New CEMService.InfoPedidoCEM
            With _cabeceraPedido
                .NumeroPedidoSAP = numeroPedidoSap
                .NumeroEntregaSAP = NumeroEntregaSap
                .NombreApellidoCliente = NombreApellido
                .NombreApellidoAutorizadoRecibir = NombreApellidoAutorizadoRecibir
                .IdentificacionCliente = IdentificacionCliente
                .DireccionEntrega = DireccionEntrega
                .ObservacionesDireccion = ObservacionDireccion
                .Barrio = Barrio
                .CodigoCiudadEntrega = CodigoCiudadEntrega
                .TelefonoContacto1 = TelefonoContacto1
                .TelefonoContacto2 = TelefonoContacto2
                If (FechaHoraAutorizacionCompra = Date.MinValue) Then
                    .FechaHoraAutorizacionCompra = Date.Today
                Else
                    .FechaHoraAutorizacionCompra = FechaHoraAutorizacionCompra
                End If
                .DetalleArticulos = DetalleArticulos
            End With
            
            If _detallePedido Is Nothing Then _detallePedido = ObtenerTablaDetallePedido()

            For Each item As DetallePedidoCEM In DetalleArticulos
                _detallePedido.Rows.Add(item.CodigoMaterialSAPEquipo, item.CantidadEquipos, item.CodigoMaterialSAPSim, item.CantidadSims)
            Next
        End Sub

#End Region

#Region "Métodos"
        Private Sub Validar(pedido As CEMService.InfoPedidoCEM)
            If pedido.NumeroPedidoSAP = 0 Then
                Throw New Exception("El nùmero de Pedido SAP no puede estar vacio")
            End If
            'If pedido.NumeroEntregaSAP = 0 Then
            '    Throw New Exception("El nùmero de Entrega SAP no puede estar vacio")
            'End If
            If pedido.NombreApellidoCliente.Trim = String.Empty Then
                Throw New Exception("El nombre del cliente no puede estar vacio")
            End If
            If pedido.IdentificacionCliente.Trim = String.Empty Then
                Throw New Exception("La identificaciòn del cliente no puede estar vacia")
            End If
            If pedido.DireccionEntrega.Trim = String.Empty Then
                Throw New Exception("La Direcciòn de entrega no puede estar vacia")
            End If
            If pedido.Barrio.Trim = String.Empty Then
                Throw New Exception("El barrio no puede estar vacio")
            End If
            If pedido.CodigoCiudadEntrega = 0 Then
                Throw New Exception("La Ciudad de Entrega no puede estar vacia.")
            End If
            If pedido.TelefonoContacto1.Trim = String.Empty Then
                Throw New Exception("El teléfono de Contacto 1 no puede estar vacio.")
            End If

        End Sub

        Public Function Registrar() As CEMService.ResultadoPedido

            Dim resultado As New ResultadoPedido
            Dim dbManager As New LMDataAccess
            Dim idServicioMensajeria As Integer
            Validar(Me.CabeceraPedido)
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idCiudad", SqlDbType.Int).Value = Me.CabeceraPedido.CodigoCiudadEntrega
                        .Add("@numeroRadicado", SqlDbType.Decimal).Value = Me.CabeceraPedido.NumeroPedidoSAP
                        .Add("@nombre", SqlDbType.VarChar).Value = Me.CabeceraPedido.NombreApellidoCliente
                        .Add("@nombreAutorizado", SqlDbType.VarChar).Value = Me.CabeceraPedido.NombreApellidoAutorizadoRecibir
                        .Add("@identificacionAutorizado", SqlDbType.VarChar).Value = Me.CabeceraPedido.IdentificacionCliente
                        .Add("@direccion", SqlDbType.VarChar).Value = Me.CabeceraPedido.DireccionEntrega
                        .Add("@observacionDireccion", SqlDbType.VarChar).Value = Me.CabeceraPedido.ObservacionesDireccion
                        .Add("@barrio", SqlDbType.VarChar).Value = Me.CabeceraPedido.Barrio
                        .Add("@telefono", SqlDbType.VarChar).Value = Me.CabeceraPedido.TelefonoContacto1
                        .Add("@telefonoAutorizado", SqlDbType.VarChar).Value = Me.CabeceraPedido.TelefonoContacto2
                        .Add("@fecha", SqlDbType.DateTime).Value = Me.CabeceraPedido.FechaHoraAutorizacionCompra
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@idServicioMensajeria", SqlDbType.BigInt).Direction = ParameterDirection.Output
                        .Add("@numeroDiasEntrega", SqlDbType.SmallInt).Direction = ParameterDirection.Output
                        .Add("@fechaEstimadaEntrega", SqlDbType.Date).Direction = ParameterDirection.Output
                        .AddWithValue("@tbDetallePedido", Me.DetallePedido)
                    End With

                    .IniciarTransaccion()
                    .TiempoEsperaComando = 0
                    .EjecutarNonQuery("RegistrarServicioTipoTiendaVirtual", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value.ToString, resultado.CodigoResultado) Then
                        resultado.Mensaje = .SqlParametros("@mensaje").Value.ToString
                        Integer.TryParse(.SqlParametros("@numeroDiasEntrega").Value.ToString, resultado.NumeroDiasEntrega)
                        Date.TryParse(.SqlParametros("@fechaEstimadaEntrega").Value.ToString, resultado.FechaEstimadaDeEntrega)

                        If resultado.CodigoResultado = 20 Or resultado.CodigoResultado = 0 Then
                            .ConfirmarTransaccion()
                            idServicioMensajeria = .SqlParametros("@idServicioMensajeria").Value
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.CodigoResultado = 1
                        resultado.Mensaje = "No se logró establecer respuesta del servidor, por favor intentelo nuevamente."
                    End If

                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.CodigoResultado = 1
                resultado.Mensaje = "Se generó un error al realizar el registro: " & ex.Message
            End Try
            Return resultado
        End Function

#End Region

#Region "Estructuras de tablas de detalle"

        Public Function ObtenerTablaDetallePedido() As DataTable
            Dim dtDatos As New DataTable
            With dtDatos
                .TableName = "tbDetallePedido"
                .Columns.Add("codigoMaterialSAPEquipo", GetType(Decimal))
                .Columns.Add("cantidadEquipos", GetType(Integer))
                .Columns.Add("codigoMaterialSAPSim", GetType(Decimal))
                .Columns.Add("cantidadSims", GetType(Decimal))
            End With
            
            Return dtDatos
        End Function

#End Region

    End Class
End Namespace