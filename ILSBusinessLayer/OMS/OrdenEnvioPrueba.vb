Imports ILSBusinessLayer.Estructuras
Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.IO
Imports System.Drawing

Namespace OMS

    Public Class OrdenEnvioPrueba

#Region "atributos"
        Private _idOrdenEnvioPrueba As Integer
        Private _idEstado As Short
        Private _observaciones As String
        Private _fechaCreacion As String
        Private _fechaNotificacion As String
        Private _fechaUltimaRecepcion As String
        Private _idUsuarioCreacion As Integer
        Private _idUsuarioNotificacion As Integer
        Private _DetalleEnvio As DataTable
        Private _DetalleEnvioSerial As DataTable
        Private _estado As String
        Private _usuarioNotificacion As String
        Private _usuarioCreacion As String
        Private _infoDatosEnvio As InfoEnvioPrueba
        Private _idFactura As Integer
        Private _idPedido As Integer
#End Region

#Region "constructores"
        Sub New()
            MyBase.new()
        End Sub
        Sub New(ByVal idOrdenEnvioPrueba As Integer)
            MyBase.new()
            _idOrdenEnvioPrueba = idOrdenEnvioPrueba
            CargarInformacion()
        End Sub
#End Region

#Region "propiedades"

        Public Property IdEstado() As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        Public Property IdOrdenEnvioPrueba() As Integer
            Get
                Return _idOrdenEnvioPrueba
            End Get
            Set(ByVal value As Integer)
                _idOrdenEnvioPrueba = value
            End Set
        End Property

        Public Property Observaciones() As String
            Get
                Return _observaciones
            End Get
            Set(ByVal value As String)
                _observaciones = value
            End Set
        End Property

        Public ReadOnly Property FechaCreacion() As String
            Get
                Return _fechaCreacion
            End Get
        End Property

        Public ReadOnly Property FechaNotificacion() As String
            Get
                Return _fechaNotificacion
            End Get
        End Property

        Public ReadOnly Property FechaUltimaRecepcion() As String
            Get
                Return _fechaUltimaRecepcion
            End Get
        End Property

        Public Property IdUsuarioCreacion() As Integer
            Get
                Return _idUsuarioCreacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioCreacion = value
            End Set
        End Property

        Public Property IdUsuarioNotificacion() As Integer
            Get
                Return _idUsuarioNotificacion
            End Get
            Set(ByVal value As Integer)
                _idUsuarioNotificacion = value
            End Set
        End Property

        Public ReadOnly Property Estado() As String
            Get
                Return _estado
            End Get
        End Property

        Public ReadOnly Property UsuarioNotificacion() As Integer
            Get
                Return UsuarioNotificacion
            End Get
        End Property

        Public ReadOnly Property UsuarioCreacion() As Integer
            Get
                Return UsuarioCreacion
            End Get
        End Property

        Public ReadOnly Property DetalleEnvio() As DataTable
            Get
                If _DetalleEnvio Is Nothing Then _DetalleEnvio = EstructuraDetalleEnvio()
                Return _DetalleEnvio
            End Get
        End Property

        Public ReadOnly Property DetalleEnvioSerial() As DataTable
            Get
                Return _DetalleEnvioSerial
            End Get
        End Property

        Public ReadOnly Property InfoDatosEnvio() As InfoEnvioPrueba
            Get
                Return _infoDatosEnvio
            End Get
        End Property

        Public Property IdFactura() As Integer
            Get
                Return _idFactura
            End Get
            Set(ByVal value As Integer)
                _idFactura = value
            End Set
        End Property

        Public Property IdPedido() As Integer
            Get
                Return _idPedido
            End Get
            Set(ByVal value As Integer)
                _idPedido = value
            End Set
        End Property

#End Region

#Region "Metodos privados"

        Private Sub CargarInformacion()
            Dim dbManager As New LMDataAccess
            Dim objDetalleSerial As New EnvioPruebaSerial
            With dbManager
                .agregarParametroSQL("@idOrdenEnvioPrueba", _idOrdenEnvioPrueba, SqlDbType.Int)
                Try
                    .ejecutarReader("ObtenerEnvioPrueba", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                        If .Reader.Read() Then
                            _idOrdenEnvioPrueba = .Reader("OrdenEnvio")
                            _idEstado = .Reader("idEstado")
                            _observaciones = .Reader("observaciones")
                            _fechaCreacion = .Reader("Fecha Creación")
                            _fechaNotificacion = .Reader("fechaNotificacion")
                            _fechaUltimaRecepcion = .Reader("fechaUltimaRecepcion")
                            _idUsuarioCreacion = .Reader("idUsuarioCreacion")
                            _idUsuarioNotificacion = .Reader("IdUsuarioNotificacion")
                            _estado = .Reader("Estado")
                            _usuarioCreacion = .Reader("UsuarioCreacion")
                            _usuarioNotificacion = .Reader("UsuarioNotificacion")
                            _idPedido = .Reader("idPedido")
                        End If
                    End If

                    If Not .Reader.IsClosed Then .Reader.Close()

                    _DetalleEnvio = .ejecutarDataTable("ObtenerDetalleEnvioPrueba", CommandType.StoredProcedure)
                    _DetalleEnvioSerial = objDetalleSerial.ObtenerSerialesPorEnvio(_idOrdenEnvioPrueba)

                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End With
        End Sub

        Private Function EstructuraDetalleEnvio() As DataTable
            Dim dtEstructura As New DataTable
            Dim pk(0) As DataColumn
            With dtEstructura
                .Columns.Add("idOrdenEnvioPrueba", GetType(Integer))
                .Columns.Add("idOrdenTrabajo", GetType(Integer))
                pk(0) = .Columns("idOrdenTrabajo")
                .PrimaryKey = pk
            End With
            Return dtEstructura
        End Function

#End Region

#Region "Metodos publicos"

        Public Function Crear() As Integer
            Dim dbManager As New LMDataAccess
            Dim objDetalleEnvioSerial As New EnvioPruebaSerial

            Try
                dbManager.iniciarTransaccion()
                With dbManager
                    .agregarParametroSQL("@idEstado", _idEstado, SqlDbType.SmallInt)
                    .agregarParametroSQL("@0bservaciones", _observaciones, SqlDbType.VarChar, 500)
                    .agregarParametroSQL("@idUsuario", _idUsuarioCreacion, SqlDbType.Int)
                    _idOrdenEnvioPrueba = .ejecutarScalar("CrearEnvioPrueba", CommandType.StoredProcedure)
                    'Crear detalle
                    If _idOrdenEnvioPrueba <> 0 AndAlso _DetalleEnvio.Rows.Count > 0 Then
                        Using dtAux As DataTable = _DetalleEnvio.Copy
                            Dim dcAux As New DataColumn("idOrdenEnvioPruebaAux")
                            dcAux.DefaultValue = _idOrdenEnvioPrueba
                            dtAux.Columns.Add(dcAux)
                            .inicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "EnvioPruebaDetalle"
                                .ColumnMappings.Add("idOrdenEnvioPruebaAux", "idOrdenEnvioPrueba")
                                .ColumnMappings.Add("idOrdenTrabajo", "idOrdenTrabajo")
                                .WriteToServer(dtAux)
                            End With
                        End Using
                        'crea el detalle de los seriales de las ordenes agregadas a la orden de envío
                        objDetalleEnvioSerial.IdOrdenEnvioPrueba = _idOrdenEnvioPrueba
                        objDetalleEnvioSerial.Crear(dbManager)
                    Else
                        Throw New Exception("No existe ordenes de trabajo asociadas al envío.")
                    End If
                End With



                dbManager.confirmarTransaccion()
            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return _idOrdenEnvioPrueba
        End Function

        Public Function Actualizar(Optional ByVal idOrdenTrabajo As Integer = 0) As Boolean
            Dim dbManager As New LMDataAccess
            Dim vista As New DataView(_DetalleEnvio)
            Dim arrOTDelete As New ArrayList
            Dim arrOTAdd As New ArrayList
            Dim resultado As Boolean

            Try
                dbManager.iniciarTransaccion()

                With dbManager
                    .SqlParametros.Add("@idOrdenEnvioPrueba", SqlDbType.Int).Value = _idOrdenEnvioPrueba
                    .SqlParametros.Add("@0bservaciones", SqlDbType.VarChar, 200).Value = _observaciones
                    .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = _idEstado
                    If _idPedido <> 0 Then .SqlParametros.Add("@idPedido", SqlDbType.Int).Value = _idPedido
                    If _idEstado = 45 Or _idEstado = 85 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuarioNotificacion
                    resultado = .ejecutarNonQuery("ActualizarEnvioPrueba", CommandType.StoredProcedure)

                End With
                dbManager.confirmarTransaccion()
            Catch ex As Exception
                Throw New Exception(ex.Message)
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            Return resultado
        End Function

        Public Sub AdicionarOTDetalle(ByVal idOrdenTrabajo As Integer, Optional ByVal idOrdenEnvio As Integer = 0)
            If _DetalleEnvio Is Nothing Then _DetalleEnvio = EstructuraDetalleEnvio()
            If _DetalleEnvio.Rows.Find(idOrdenTrabajo) Is Nothing Then
                Dim drDetalle As DataRow = _DetalleEnvio.NewRow
                drDetalle("idOrdenEnvioPrueba") = idOrdenEnvio
                drDetalle("idOrdenTrabajo") = idOrdenTrabajo
                _DetalleEnvio.Rows.Add(drDetalle)
            Else
                Throw New Exception("La orden de trabajo ya existe para este envío.")
            End If

        End Sub

        Public Sub CrearDetalle(ByVal idOrdenTrabajo As Integer)
            Dim dbManager As New LMDataAccess
            Dim objDetalleEnvioSerial As New EnvioPruebaSerial

            Try
                dbManager.iniciarTransaccion()
                With dbManager
                    .agregarParametroSQL("@idOrdenEnvioPrueba", _idOrdenEnvioPrueba, SqlDbType.Int)
                    .agregarParametroSQL("@idOrdenTrabajo", idOrdenTrabajo, SqlDbType.Int)
                    'Adiciona detalle
                    If (.ejecutarNonQuery("CrearDetalleEnvioPrueba", CommandType.StoredProcedure)) Then
                        objDetalleEnvioSerial.IdOrdenEnvioPrueba = _idOrdenEnvioPrueba
                        objDetalleEnvioSerial.Crear(dbManager)
                    End If
                    _DetalleEnvio = .ejecutarDataTable("ObtenerDetalleEnvioPrueba", CommandType.StoredProcedure)
                    _DetalleEnvioSerial = objDetalleEnvioSerial.ObtenerSerialesPorEnvio(_idOrdenEnvioPrueba)
                End With

                dbManager.confirmarTransaccion()

            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Sub


        Public Sub EliminarDetalle(ByVal idOrdenTrabajo As Integer)
            Dim dbManager As New LMDataAccess
            Dim objDetalleEnvioSerial As New EnvioPruebaSerial
            Try
                dbManager.iniciarTransaccion()
                With dbManager
                    .agregarParametroSQL("@idOrdenEnvioPrueba", _idOrdenEnvioPrueba, SqlDbType.Int)
                    .agregarParametroSQL("@idOrdenTrabajo", idOrdenTrabajo, SqlDbType.Int)
                    'Adiciona detalle
                    If (.ejecutarNonQuery("EliminarDetalleEnvioPrueba", CommandType.StoredProcedure)) Then
                        objDetalleEnvioSerial.IdOrdenEnvioPrueba = _idOrdenEnvioPrueba
                        objDetalleEnvioSerial.Crear(dbManager)
                    End If
                    _DetalleEnvio = .ejecutarDataTable("ObtenerDetalleEnvioPrueba", CommandType.StoredProcedure)
                    _DetalleEnvioSerial = objDetalleEnvioSerial.ObtenerSerialesPorEnvio(_idOrdenEnvioPrueba)
                End With

                dbManager.confirmarTransaccion()

            Catch ex As Exception
                If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
            
        End Sub

        Public Function GenerarEnvioLectura() As ResultadoProceso
            Dim dbManager As New LMDataAccess
            Dim rp As New ResultadoProceso
            Dim retornoResultado As Short = 0

            If _idOrdenEnvioPrueba > 0 Then
                Try
                    With dbManager
                        .SqlParametros.Add("@idOrdenEnvioPrueba", SqlDbType.Int).Value = _idOrdenEnvioPrueba
                        If _observaciones IsNot Nothing And _observaciones.Trim.Length > 0 Then .SqlParametros.Add("@observaciones", SqlDbType.VarChar, 500).Value = _observaciones
                        .SqlParametros.Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue

                        .iniciarTransaccion()
                        .ejecutarNonQuery("GenerarEnvioLecturaParaEnvioDePrueba", CommandType.StoredProcedure)
                        retornoResultado = CShort(.SqlParametros("@returnValue").Value)

                        If retornoResultado = 0 Then
                            .confirmarTransaccion()
                            rp.EstablecerMensajeYValor(retornoResultado, "Ejecución Satisfactoria")
                        Else
                            rp.EstablecerMensajeYValor(retornoResultado, "No fue posible establecer la información para generar el envío de lectura.")

                            If .estadoTransaccional Then .abortarTransaccion()
                        End If
                    End With
                Catch ex As Exception
                    If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                rp.EstablecerMensajeYValor(10, "No se suministraron los datos necesarios para generar el envío de lectura.")
            End If

            Return rp
        End Function

#End Region

#Region "Metodos compartidos"

        Public Shared Function ListarOrdenEnvioPorId(ByVal idOrdenEnvio As Integer) As DataTable
            Dim filtro As New FiltroEnvioPrueba
            Dim dtDatos As DataTable
            filtro.idOrdenEnvioPrueba = idOrdenEnvio
            dtDatos = ObtenerListadoOrdenEnvio(filtro)
            Return dtDatos
        End Function

        Public Shared Function ListarOrdenEnvio() As DataTable
            Dim dtDatos As DataTable
            dtDatos = ObtenerListadoOrdenEnvio()
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListadoOrdenEnvio(ByVal filtro As FiltroEnvioPrueba) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            With dbManager
                .SqlParametros.Add("@idOrdenEnvioPrueba", SqlDbType.Int).Value = filtro.idOrdenEnvioPrueba
                dtDatos = .ejecutarDataTable("ObtenerEnvioPrueba", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListadoOrdenEnvio() As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As DataTable
            With dbManager
                dtDatos = .ejecutarDataTable("ObtenerEnvioPrueba", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function


        Public Shared Function ObtenerOrdeneTrabajoPrueba(ByVal filtros As FiltroEnvioPrueba) As DataTable
            Dim dtDatos As DataTable
            Dim dbManager As New LMDataAccess
            With dbManager
                'If filtros.idFactura <> 0 Then .agregarParametroSQL("@idFactura", filtros.idFactura, SqlDbType.VarChar, 15)
                If filtros.idOrdenEnvioPrueba <> 0 Then .agregarParametroSQL("@IdOrdenEnvioPrueba", filtros.idOrdenEnvioPrueba, SqlDbType.VarChar, 250)
                dtDatos = .ejecutarDataTable("ObtenerOrdenTrabajoPruebas", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerOrdeneTrabajoPruebaSerial(ByVal idOrdenTrabajo As Integer) As DataTable
            Dim dtDatos As DataTable
            Dim dbManager As New LMDataAccess
            With dbManager
                .agregarParametroSQL("@idOrdenTrabajo", idOrdenTrabajo, SqlDbType.Int)
                dtDatos = .ejecutarDataTable("ObtenerSerialesOrdenTrabajoPruebas", CommandType.StoredProcedure)
            End With
            Return dtDatos
        End Function

        Public Function ObtenerDatosEnvio(ByVal idOrdenEnvioPrueba As Integer) As InfoEnvioPrueba
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Add("@idOrdenEnvioPrueba", SqlDbType.Int).Value = idOrdenEnvioPrueba
                    .ejecutarReader("ObtenerInfoEnvioPrueba", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing And .Reader.HasRows Then
                        If .Reader.Read Then
                            _infoDatosEnvio.Guia = .Reader("Guia").ToString
                            _infoDatosEnvio.factura = .Reader("factura").ToString
                            _infoDatosEnvio.fechaRecepcion = .Reader("fechaRecepcion")
                            _infoDatosEnvio.Producto = .Reader("Producto").ToString
                            _infoDatosEnvio.CantidadSeriales = .Reader("cantidadSerial")
                            _idPedido = .Reader("idPedido")
                        End If
                    End If
                    If .Reader IsNot Nothing Then .Reader.Close()
                End With
                Return _infoDatosEnvio
            Catch ex As Exception
                Throw New Exception(ex.Message)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

        End Function

        Public Shared Function CrearArchivoExcel(ByVal dtDatos As DataTable, ByVal titulo As String, ByVal nombreArchivo As String, ByVal ruta As String, _
                                                Optional ByVal nombreColumnas As ArrayList = Nothing, Optional ByVal showFooter As Boolean = True)
            SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
            Dim ef As New ExcelFile
            Dim ws As ExcelWorksheet

            Try
                ws = ef.Worksheets.Add("Hoja 1")
                ws.ExtractToDataTable(dtDatos, dtDatos.Rows.Count, ExtractDataOptions.StopAtFirstEmptyRow, ws.Rows(3), ws.Columns(0))
                ws.InsertDataTable(dtDatos, "A3", True)
                ws.Cells.GetSubrangeAbsolute(0, 0, 0, dtDatos.Columns.Count).Merged = True
                With ws.Cells("A1")
                    .Value = titulo
                    With .Style
                        .Font.Color = Color.Black
                        .Font.Weight = ExcelFont.BoldWeight
                        .Font.Size = 16 * 18
                    End With
                End With
                For i As Integer = 0 To dtDatos.Columns.Count - 1
                    If Not nombreColumnas Is Nothing Then
                        ws.Cells(2, i).Value = nombreColumnas(i)
                    Else
                        ws.Cells(2, i).Value = dtDatos.Columns(i).ColumnName
                    End If
                    With ws.Cells(2, i).Style
                        .FillPattern.SetPattern(FillPatternStyle.Solid, Color.DarkBlue, Color.DarkBlue)
                        .Font.Color = Color.White
                        .Font.Weight = ExcelFont.BoldWeight
                        .Borders.SetBorders(MultipleBorders.Top, Color.FromName("black"), LineStyle.Thin)
                        .Borders.SetBorders(MultipleBorders.Right, Color.FromName("black"), LineStyle.Thin)
                        .Borders.SetBorders(MultipleBorders.Left, Color.FromName("black"), LineStyle.Thin)
                        .Borders.SetBorders(MultipleBorders.Bottom, Color.FromName("black"), LineStyle.Thin)
                        .HorizontalAlignment = HorizontalAlignmentStyle.Center
                    End With

                Next
                If showFooter Then
                    ws.Cells.GetSubrangeAbsolute(dtDatos.Rows.Count + 3, 0, (dtDatos.Rows.Count + 3), dtDatos.Columns.Count - 1).Merged = True
                    With ws.Cells("A" & (dtDatos.Rows.Count + 4).ToString).Style
                        .FillPattern.SetPattern(FillPatternStyle.Solid, Color.LightGray, Color.LightGray)
                        .Font.Color = Color.DarkBlue
                        .Font.Weight = ExcelFont.BoldWeight
                        .Borders.SetBorders(MultipleBorders.Top, Color.FromName("black"), LineStyle.Thin)
                        .Borders.SetBorders(MultipleBorders.Right, Color.FromName("black"), LineStyle.Thin)
                        .Borders.SetBorders(MultipleBorders.Left, Color.FromName("black"), LineStyle.Thin)
                        .Borders.SetBorders(MultipleBorders.Bottom, Color.FromName("black"), LineStyle.Thin)
                        .HorizontalAlignment = HorizontalAlignmentStyle.Center
                    End With
                    ws.Cells("A" & (dtDatos.Rows.Count + 4).ToString).Value = dtDatos.Rows.Count & " Registro(s) Encontrado(s)"
                End If

                For index As Integer = 0 To dtDatos.Columns.Count - 1
                    ws.Columns(index).AutoFit()
                Next
                ef.SaveXls(ruta)

                Return ruta
            Catch ex As Exception
                Throw New Exception("Al crear archivo de excel: " & ex.Message & ex.StackTrace)
            End Try
        End Function

#End Region

    End Class

End Namespace
