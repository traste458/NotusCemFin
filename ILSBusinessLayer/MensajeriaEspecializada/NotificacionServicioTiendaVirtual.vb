Imports LMDataAccessLayer

Namespace CEMService
    Public Class NotificacionServicioTiendaVirtual

#Region "Métodos"
        ''' <summary>
        ''' Obtiene los pedidos con Cambio de Estado con fecha Superior a la ultima Ejecucion del Monitor Cambio Estado Tabla  = logReporteCambioEstado
        ''' </summary>
        ''' <returns></returns>
        Public Function ActualizarEstado(dtListaPedidoSincronizada As DataTable) As CEMService.ResultadoPedido
            Dim resultado As New ResultadoPedido
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .AddWithValue("@ListaReporteCambioEstado", dtListaPedidoSincronizada)
                    End With
                    .EjecutarNonQuery("ActualizarCambioEstadoPedido", CommandType.StoredProcedure)
                    resultado.CodigoResultado = 0
                    resultado.Mensaje = "Proceso Satisfactorio"
                End With
                Return resultado
            Catch ex As Exception
                Throw New Exception("Se generó un error al realizar el registro: " & ex.Message)
            End Try
        End Function
        ''' <summary>
        ''' Obtener aquellos pedidos que no han sido enviados a wcf Claro Tienda virtual
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ObtenerPediodosPorEnviarTiendaVirtual() As DataTable
            Dim dbManager As New LMDataAccess
            Dim resultado As New ResultadoProceso
            Try
                Return dbManager.EjecutarDataTable("ObtenerPedidosCambioEstadoTiendaVirtual", CommandType.StoredProcedure)
            Catch ex As Exception
                Throw New Exception("Error consultando los pedidos por sincronizar Tienda Virtual : " & ex.Message)
            End Try
        End Function
#End Region

#Region "Estructuras de tablas de detalle."
        Public Shared Function ObtenerTablaDetallePedido() As DataTable
            Dim dtDatos As New DataTable
            dtDatos.TableName = "tbDetallePedido"
            dtDatos.Columns.Add("codigoMaterialSAPEquipo", GetType(Decimal))
            dtDatos.Columns.Add("cantidadEquipos", GetType(Integer))
            dtDatos.Columns.Add("codigoMaterialSAPSim", GetType(Decimal))
            dtDatos.Columns.Add("cantidadSims", GetType(Decimal))
            Return dtDatos
        End Function
        Public Shared Function ObtenerTablaDetallePedidoParaActualizar() As DataTable
            Dim dtDatos As New DataTable
            dtDatos.TableName = "ListaPedidoTiendaVirtual"
            dtDatos.Columns.Add("numeroRadicado", GetType(Decimal))
            dtDatos.Columns.Add("idServicioMensajeria", GetType(Int32))
            dtDatos.Columns.Add("idEstado", GetType(Int32))
            dtDatos.Columns.Add("nombreEstado", GetType(String))
            dtDatos.Columns.Add("fechaEstado", GetType(DateTime))
            dtDatos.Columns.Add("novedadEstado", GetType(String))
            dtDatos.Columns.Add("fechaNovedadActual", GetType(DateTime))
            dtDatos.Columns.Add("codigoResultado", GetType(Integer))
            dtDatos.Columns.Add("mensaje", GetType(String))
            Return dtDatos
        End Function
#End Region

    End Class
End Namespace

