Imports LMDataAccessLayer
Imports ILSBusinessLayer.Recibos
Imports ILSBusinessLayer.Estructuras
Imports ILSBusinessLayer.Region
Imports System.Drawing
Imports GemBox.Spreadsheet
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Localizacion
Imports ILSBusinessLayer.OMS
Imports ILSBusinessLayer.Productos
Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Namespace OMS

    Public Class OrdenEnvioLectura

#Region "Campos"

        Private _idOrdenEnvioLectura As Long
        Private _idInstruccion As Long
        Private _idEstado As Integer
        Private _idCreador As Long
        Private _fechaCreacion As Date
        Private _idUsuarioEnvio As Long
        Private _fechaEnvio As Date
        Private _observaciones As String
        Private _cantidadEnvio As Integer
        Private _idFactura As Long
        Private _idGuia As Long
        Private _estado As String
        Private _region As String
        Private _creador As String
        Private _infoDatosEnvio As InfoEnvioCorreo
        Private _arrIdsOrdenes As ArrayList
        Private _idFacturaGuia As Integer
        Private _infoDetalleEnvio As DetalleEnvioLectura

#End Region

#Region "Propiedades"

        Public Property IdOrdenEnvioLectura() As Long
            Get
                Return _idOrdenEnvioLectura
            End Get
            Set(ByVal value As Long)
                _idOrdenEnvioLectura = value
            End Set
        End Property

        Public Property IdInstruccion() As Long
            Get
                Return _idInstruccion
            End Get
            Set(ByVal value As Long)
                _idInstruccion = value
            End Set
        End Property

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property IdCreador() As Long
            Get
                Return _idCreador
            End Get
            Set(ByVal value As Long)
                _idCreador = value
            End Set
        End Property

        Public Property FechaCreacion() As Date
            Get
                Return _fechaCreacion
            End Get
            Set(ByVal value As Date)
                _fechaCreacion = value
            End Set
        End Property

        Public Property IdUsuarioEnvio() As Long
            Get
                Return _idUsuarioEnvio
            End Get
            Set(ByVal value As Long)
                _idUsuarioEnvio = value
            End Set
        End Property

        Public Property FechaEnvio() As Date
            Get
                Return _fechaEnvio
            End Get
            Set(ByVal value As Date)
                _fechaEnvio = value
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

        Public ReadOnly Property EstadoOrden() As String
            Get
                Return _estado
            End Get

        End Property

        Public Property Creador() As String
            Get
                Return _creador
            End Get
            Set(ByVal value As String)
                _creador = value
            End Set
        End Property

        Public ReadOnly Property Region() As String
            Get
                Return _region
            End Get
        End Property

        Public Property IdFacturaGuia() As Long
            Get
                Return _idFacturaGuia
            End Get
            Set(ByVal value As Long)
                _idFacturaGuia = value
            End Set
        End Property

        Public Property IdFactura() As Long
            Get
                Return _idFactura
            End Get
            Set(ByVal value As Long)
                _idFactura = value
            End Set
        End Property

        Public Property IdGuia() As Long
            Get
                Return _idGuia
            End Get
            Set(ByVal value As Long)
                _idGuia = value
            End Set
        End Property

        Public Property CantidadEnvio() As Integer
            Get
                Return _cantidadEnvio
            End Get
            Set(ByVal value As Integer)
                _cantidadEnvio = value
            End Set
        End Property

        Public ReadOnly Property InfoDatosEnvio() As InfoEnvioCorreo
            Get
                Return _infoDatosEnvio
            End Get
        End Property

        Public Property ArrIdsOrdenes() As ArrayList
            Get
                Return _arrIdsOrdenes
            End Get
            Set(ByVal value As ArrayList)
                _arrIdsOrdenes = value
            End Set
        End Property

        Public Property InfoDetalleEnvio() As DetalleEnvioLectura
            Get
                If _infoDetalleEnvio Is Nothing Then
                    InfoDetalleEnvio = New DetalleEnvioLectura()
                    InfoDetalleEnvio.IdOrdenEnvioLectura = _idOrdenEnvioLectura
                End If

                Return InfoDetalleEnvio
            End Get
            Set(ByVal value As DetalleEnvioLectura)
                InfoDetalleEnvio = value
            End Set
        End Property

#End Region

#Region "Contructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal identificador As Integer)
            MyBase.New()
            _idOrdenEnvioLectura = identificador
            CargarInformacion()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarInformacion()
            If _idOrdenEnvioLectura <> 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        .SqlParametros.Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = _idOrdenEnvioLectura
                        .ejecutarReader("ObtenerInfoEnvioLectura", CommandType.StoredProcedure)
                        If .Reader IsNot Nothing Then
                            If .Reader.Read Then
                                Long.TryParse(.Reader("idInstruccion").ToString, _idInstruccion)
                                _estado = .Reader("Estado").ToString
                                _idEstado = .Reader("idEstado").ToString
                                Creador = .Reader("Creador").ToString()
                                Date.TryParse(.Reader("fechaCreacion").ToString, _fechaCreacion)
                                Long.TryParse(.Reader("idUsuarioEnvio").ToString, _idUsuarioEnvio)
                                Date.TryParse(.Reader("fechaEnvio").ToString, _fechaEnvio)
                                _observaciones = .Reader("observaciones").ToString()
                                Integer.TryParse(.Reader("idFacturaGuia").ToString, _idFacturaGuia)
                                Long.TryParse(.Reader("idFactura").ToString(), _idFactura)
                                Long.TryParse(.Reader("idGuia").ToString(), _idGuia)
                                '_region = .Reader("region").ToString()
                                Integer.TryParse(.Reader("cantidad").ToString, _cantidadEnvio)
                            End If
                            .Reader.Close()
                        End If
                    End With
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            End If
        End Sub

        ''' <summary>
        ''' Carga los datos de un datatable en la tabla SerialesCargaEnvioLectura.
        ''' </summary>
        ''' <param name="dtdatos">datatable con la informacion a cargar</param>
        ''' <remarks></remarks>
        Private Function CargarDatos(ByVal dtdatos As DataTable, ByVal idUsuario As Long) As Boolean
            Dim dbManager As New LMDataAccess

            With dbManager
                dtdatos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
                dbManager.agregarParametroSQL("@idUsuario", idUsuario)

                Try
                    dbManager.ejecutarNonQuery("BorrarSerialesCargaEnvioLectura", CommandType.StoredProcedure)

                    .inicilizarBulkCopy()
                    With .BulkCopy
                        .DestinationTableName = "SerialesCargaEnvioLectura"
                        .ColumnMappings.Add("serial", "serial")
                        .ColumnMappings.Add("fechaCarga", "fechaCarga")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(dtdatos)
                    End With
                    Return True
                Catch ex As Exception

                End Try
            End With

            Return False

        End Function

#End Region

#Region "Métodos Publicos"

        Public Function Registrar() As Short
            Dim resultado As Short = 0

            If _idInstruccion > 0 And _idEstado > 0 And _idCreador > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idInstruccion", SqlDbType.BigInt).Value = _idInstruccion
                            .Add("@idEstado", SqlDbType.Int).Value = IdEstado
                            .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador
                            .Add("@identity", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CrearOrdenEnvioLectura", CommandType.StoredProcedure)
                        resultado = CShort(.SqlParametros("@returnValue").Value)
                        If resultado = 0 Then
                            _idOrdenEnvioLectura = CLng(.SqlParametros("@identity").Value)

                            With .SqlParametros
                                .Clear()
                                .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = IdOrdenEnvioLectura
                                .Add("@idInstruccion", SqlDbType.BigInt).Value = _idInstruccion
                                .Add("@returnValue", SqlDbType.BigInt).Direction = ParameterDirection.ReturnValue
                            End With

                            ' Agrega las ordenes de trabajo de produccion que no han sido adicionadas a un envío 
                            ' y las relaciona con el envio
                            .ejecutarNonQuery("CrearDetalleOrdenEnvioLectura", CommandType.StoredProcedure)
                            resultado = .SqlParametros("@returnValue").Value

                            If resultado <> 0 Then
                                If .estadoTransaccional Then .abortarTransaccion()
                                'Throw New Exception("Imposible registrar la información de la Orden de Envio en la Base de Datos.")
                            End If

                            .confirmarTransaccion()
                        Else
                            If .estadoTransaccional Then .abortarTransaccion()
                            'Throw New Exception("Imposible registrar la información de la Orden de Envio en la Base de Datos.")
                        End If
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 3
            End If

            Return resultado

        End Function

        Public Function Actualizar() As Short
            Dim resultado As Short = 0
            If _idOrdenEnvioLectura > 0 AndAlso _idUsuarioEnvio > 0 AndAlso _idEstado > 0 Then

                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = _idOrdenEnvioLectura
                            .Add("@idUsuarioEnvio", SqlDbType.BigInt).Value = _idUsuarioEnvio
                            .Add("@idEstado", SqlDbType.Int).Value = _idEstado
                            .Add("@observaciones", SqlDbType.VarChar, 500).Value = _observaciones
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("ActualizarOrdenEnvioLectura", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado)
                        If resultado <> 0 Then
                            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        End If
                        If resultado = 0 Then .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 2
            End If

            Return resultado
        End Function

        Public Function CargarSeriales(ByVal dtDatos As DataTable) As Short
            Dim resultado As Short = 0

            If _idOrdenEnvioLectura > 0 AndAlso _idUsuarioEnvio > 0 _
            AndAlso dtDatos IsNot Nothing AndAlso dtDatos.Rows.Count > 0 Then

                If Not CargarDatos(dtDatos, _idUsuarioEnvio) Then Return 1

                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        With .SqlParametros
                            .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = _idOrdenEnvioLectura
                            .Add("@idUsuario", SqlDbType.BigInt).Value = _idUsuarioEnvio
                            .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("CargarSerialesSapAEnvioLectura", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@returnValue").Value.ToString, resultado)
                        If resultado <> 0 Then
                            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                        End If
                        If resultado = 0 Then .confirmarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 2
            End If

            Return resultado
        End Function

        Public Function AgruparEnvioLectura() As Short
            Dim resultado As Short = 0
            Dim idsOrdenes As String
            If _idCreador > 0 AndAlso _arrIdsOrdenes.Count > 0 Then
                Dim dbManager As New LMDataAccess
                Try
                    With dbManager
                        idsOrdenes = Join(_arrIdsOrdenes.ToArray, ",")
                        With .SqlParametros
                            .Add("@listaOrdenes", SqlDbType.VarChar, 8000).Value = idsOrdenes
                            .Add("@idCreador", SqlDbType.BigInt).Value = _idCreador
                            .Add("@idOrdenEnvioLectura", SqlDbType.Int).Direction = ParameterDirection.Output
                            .Add("@returnValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        End With
                        .iniciarTransaccion()
                        .ejecutarNonQuery("AgruparEnviosLectura", CommandType.StoredProcedure)

                        Long.TryParse(.SqlParametros("@idOrdenEnvioLectura").Value, _idOrdenEnvioLectura)
                        Short.TryParse(.SqlParametros("@returnValue").Value, resultado)
                        If resultado = 0 Then .confirmarTransaccion()

                        If .estadoTransaccional Then .abortarTransaccion()
                    End With
                Catch ex As Exception
                    If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                    Throw New Exception(ex.Message, ex)
                Finally
                    If dbManager IsNot Nothing Then dbManager.Dispose()
                End Try
            Else
                resultado = 10 ' No fue posible obtener los datos necesarios para realizar la agrupación
            End If

            Return resultado

        End Function
#End Region

#Region "Métodos Compartidos"

        Public Overloads Shared Function ObtenerListado() As DataTable
            Dim filtro As New FiltroEnvioLectura
            Dim dtDatos As DataTable = ObtenerListado(filtro)
            Return dtDatos
        End Function

        Public Overloads Shared Function ObtenerListado(ByVal filtro As FiltroEnvioLectura) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As New DataTable()
            Try
                With dbManager
                    With .SqlParametros
                        If filtro.IdOrdenEnvioLectura > 0 Then .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = filtro.IdOrdenEnvioLectura
                        If filtro.IdInstruccion > 0 Then .Add("@idInstruccion", SqlDbType.BigInt).Value = filtro.IdInstruccion
                        If filtro.Idfactura > 0 Then .Add("@idfactura", SqlDbType.BigInt).Value = filtro.Idfactura
                        If filtro.IdGuia > 0 Then .Add("@idGuia", SqlDbType.BigInt).Value = filtro.IdGuia
                        If filtro.IdRegion > 0 Then .Add("@idRegion", SqlDbType.Int).Value = filtro.IdRegion
                        If filtro.IdProducto > 0 Then .Add("@idProducto", SqlDbType.Int).Value = filtro.IdProducto
                        If filtro.IdEstado > 0 Then .Add("@idEstado", SqlDbType.Int).Value = filtro.IdEstado
                        If filtro.Material IsNot Nothing AndAlso Not String.IsNullOrEmpty(filtro.Material.ToString()) Then .Add("@material", SqlDbType.VarChar, 20).Value = filtro.Material.ToString()
                        If filtro.ListaEstados IsNot Nothing AndAlso filtro.ListaEstados.Trim.Length > 0 Then _
                        dbManager.agregarParametroSQL("@listaEstados", filtro.ListaEstados, SqlDbType.VarChar, 200)
                    End With
                    dtDatos = .ejecutarDataTable("ObtenerInfoEnvioLectura", CommandType.StoredProcedure)
                End With
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function

        Public Shared Function ObtenerListadoEnvioLecturaConsultaTemporal(ByVal idOrdenEnvioLectura As Integer) As DataTable
            Dim dbManager As New LMDataAccess
            Dim dtDatos As New DataTable()
            Try
                dbManager.SqlParametros.Add("@idOrdenEnvioLectura", SqlDbType.Int).Value = idOrdenEnvioLectura
                dtDatos = dbManager.ejecutarDataTable("ObtenerEnvioLConsulta", CommandType.StoredProcedure)
                Return dtDatos
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtDatos
        End Function

        Public Shared Function ValidarExistenciaOrdenEnEnvio(ByVal filtro As FiltroEnvioLectura) As Short

            Dim db As New LMDataAccess
            Dim result As Short
            With db
                With .SqlParametros
                    If filtro.IdOrdenTrabajo <> 0 Then .Add("@idOrdenTrabajo", SqlDbType.BigInt).Value = filtro.IdOrdenTrabajo
                    .Add("@returnValue", SqlDbType.SmallInt).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    .ejecutarNonQuery("ValidarExistenciaOrdenEnEnvio", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@returnValue").Value.ToString, result)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return result
        End Function

        Public Shared Function SerialesSinCargarEnEnvio(ByVal filtro As FiltroEnvioLectura) As Short
            Dim db As New LMDataAccess
            Dim result As Short
            If filtro.IdOrdenEnvioLectura > 0 Then
                With db
                    With .SqlParametros
                        If filtro.IdOrdenEnvioLectura <> 0 Then .Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = filtro.IdOrdenEnvioLectura
                        .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                    End With

                    Try
                        .ejecutarNonQuery("SerialesSinCargarEnEnvio", CommandType.StoredProcedure)
                        Short.TryParse(.SqlParametros("@result").Value.ToString, result)
                    Finally
                        If db IsNot Nothing Then db.Dispose()
                    End Try
                End With

                Return result
            Else
                Return 3 ' No se suministro id envio lectura
            End If

        End Function

        Public Shared Function ObtenerSerialesEnvio(ByVal identificador As Long) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                If identificador <> 0 Then .SqlParametros.Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = identificador
                Try
                    dtDatos = .ejecutarDataTable("ObtenerSerialesEnvioLectura", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerListadoSeriales(ByVal identificador As Long, Optional ByVal _facturas As ArrayList = Nothing) As DataSet
            Dim dsDatos As New DataSet
            Dim db As New LMDataAccess
            With db
                If _facturas IsNot Nothing AndAlso _facturas.Count > 0 Then .SqlParametros.Add("@facturas", SqlDbType.VarChar).Value = Join(_facturas.ToArray, ",")
                If identificador <> 0 Then .SqlParametros.Add("@idOrdenEnvioLectura", SqlDbType.BigInt).Value = identificador
                Try
                    dsDatos = .EjecutarDataSet("ObtenerSerialesEnvioLectura", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dsDatos
        End Function

        Public Shared Function ObtenerCantidadesPorInstruccion(ByVal filtro As FiltroEnvioLectura) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                If filtro.IdInstruccion <> 0 Then .SqlParametros.Add("@idInstruccion", SqlDbType.BigInt).Value = filtro.IdInstruccion
                Try
                    dtDatos = .ejecutarDataTable("ObtenerCantidadesEnvioLectura", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerCantidadPendienteEnvioLectura(ByVal filtro As FiltroEnvioLectura) As DataTable
            Dim dtDatos As New DataTable
            Dim db As New LMDataAccess
            With db
                If filtro.IdOrdenTrabajo <> 0 Then .SqlParametros.Add("@idOrden", SqlDbType.BigInt).Value = filtro.IdOrdenTrabajo
                Try
                    dtDatos = .ejecutarDataTable("ObtenerCantidadPendienteEnvioLectura", CommandType.StoredProcedure)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return dtDatos
        End Function

        Public Shared Function ObtenerMaximoEnvioLectura() As Long
            Dim maxId As Long
            Dim db As New LMDataAccess
            With db
                Try
                    maxId = CLng(.ejecutarScalar("ObtenerMaximoEnvioLectura", CommandType.StoredProcedure))
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With
            Return maxId
        End Function

        ''' <summary>
        ''' Generar el archivo con base a la información suministrada
        ''' </summary>
        ''' <param name="dtDatos">datos para generar el archivo</param>
        ''' <param name="nombreArchivo">nombre con el cual se genera el archivo</param>
        ''' <returns>Retorna el nombre con el que se guarda el archivo</returns>
        ''' <remarks></remarks>
        Public Shared Function GenerarArchivo(ByVal dsDatos As DataSet, ByVal nombreArchivo As String, ByVal rutaLocal As String, Optional ByVal lstInfoEnvioLectura As List(Of OrdenEnvioLectura.InfoEnvioLectura) = Nothing) As String
            SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
            Dim ruta As String = rutaLocal & nombreArchivo
            Dim oExcel As New ExcelFile
            Dim dtRegion As New DataTable()
            Dim dtDatoRegion As New DataTable()
            Dim dtProductoNormal As DataTable = dsDatos.Tables("PRODUCTO_NORMAL")
            Dim dtNoConforme As DataTable = dsDatos.Tables("PRODUCTO_NO_CONFORME")

            'dtProductoNormal.Clear()
            'dtTotal = dtDatos.Copy
            'If dtDatos.Columns.Contains("NO_CONFORMIDAD") Then
            '    dtProductoNormal = AplicarFiltro(dtProductoNormal, "NO_CONFORMIDAD = ''")
            '    dtProductoNormal.Columns.Remove("NO_CONFORMIDAD")
            'End If

            'If dtTotal.Columns.Contains("IMEI") Then
            '    dtTotal = dtTotal.DefaultView.ToTable(True, "FACTURA", "IMEI", "PRODUCTO", "REGION")
            'Else
            '    dtTotal = dtTotal.DefaultView.ToTable(True, "FACTURA", "ICCID", "PRODUCTO", "REGION")
            'End If

            dtRegion = ObtenerTodas()

            If dtRegion IsNot Nothing AndAlso dtRegion.Rows.Count > 0 Then
                For index As Integer = 0 To dtRegion.Rows.Count - 1
                    dtDatoRegion.Clear()
                    'dtDatoRegion = dtProductoNormal.Clone
                    dtDatoRegion = AplicarFiltro(dtProductoNormal, "region = '" & dtRegion.Rows(index)("codigo").ToString() & "'")
                    If dtDatoRegion.Rows.Count > 0 OrElse dtProductoNormal.Columns.Contains("IMEI") Then
                        ObtenerDatosHojas(oExcel, dtDatoRegion, dtRegion.Rows(index)("nombreRegion").ToString())
                    End If
                    'If index = 0 AndAlso mostrarEncabezado Then
                    '    ObtenerDatosHojas(oExcel, dtDatoRegion, dtRegion.Rows(index)("nombreRegion").ToString(), lstInfoEnvioLectura)
                    'Else
                    'End If
                Next
            End If

            If dtProductoNormal.Columns.Contains("IMEI") Then ObtenerDatosHojas(oExcel, dtProductoNormal, "TOTAL")
            If dtNoConforme IsNot Nothing AndAlso dtNoConforme.Rows.Count > 0 Then ObtenerDatosHojas(oExcel, dtNoConforme, "NO CONFORME")

            'If dtDatos.Columns.Contains("NO_CONFORMIDAD") Then
            '    dtNoConforme = AplicarFiltro(dtDatos, "NO_CONFORMIDAD <> ''")
            '    ObtenerDatosHojas(oExcel, dtNoConforme, "NO CONFORME")
            'End If

            oExcel.SaveXls(ruta)
            Return nombreArchivo
        End Function

        ''' <summary>
        ''' Obtiene la información que va a visualizarse separada por hojas según corresponda
        ''' </summary>
        ''' <param name="oExcel">objeto excel contenedor</param>
        ''' <param name="dtDatos">datos con los que se alimentan las hojas</param>
        ''' <param name="nombreHoja">nombre asignado para las hojas creadas en el excel</param>
        ''' <remarks></remarks>
        Private Shared Sub ObtenerDatosHojas(ByRef oExcel As ExcelFile, ByVal dtDatos As DataTable, ByVal nombreHoja As String, Optional ByVal lstInfoEnvioLectura As List(Of OrdenEnvioLectura.InfoEnvioLectura) = Nothing)
            Dim oWs As ExcelWorksheet
            Dim numRows As Integer = 60000
            Dim maxIndex As Integer = dtDatos.Rows.Count - 1
            Dim numHojas As Integer = Math.Ceiling((dtDatos.Rows.Count / numRows))
            Dim fila As Integer = 0
            Dim ind As Integer
            Dim maxRow As Integer
            Dim dtAux As DataTable = dtDatos.Clone
            Dim nombre As String = String.Empty

            If nombreHoja.Equals("RETAIL") Then nombreHoja = "CADENAS"

            If dtDatos IsNot Nothing AndAlso numHojas = 0 Then
                oWs = oExcel.Worksheets.Add(nombreHoja)
                AdicionarDatosAHoja(oWs, dtDatos, lstInfoEnvioLectura)
            End If

            For index As Integer = 1 To numHojas
                dtAux.Rows.Clear()

                If numHojas > 1 Then
                    nombre = nombreHoja & "_" & index.ToString
                Else
                    nombre = nombreHoja
                End If

                oWs = oExcel.Worksheets.Add(nombre)

                maxRow = Math.Min((ind + (numRows - 1)), maxIndex)
                For ind = fila To maxRow
                    dtAux.ImportRow(dtDatos.Rows(ind))
                Next
                fila = ind
                AdicionarDatosAHoja(oWs, dtAux, lstInfoEnvioLectura)
            Next
        End Sub

        ''' <summary>
        ''' Adiciona los datos de las hojas en el objeto excel
        ''' </summary>
        ''' <param name="OWs">Objeto Hoja de excel a la que se adiciona la información</param>
        ''' <param name="dtDatos">datos con los que se llena la hoja de excel</param>
        ''' <param name="arrNombreColumna">arreglo con el nombre de las columnas para la hoja</param>
        ''' <remarks></remarks>
        Private Shared Sub AdicionarDatosAHoja(ByVal OWs As ExcelWorksheet, ByVal dtDatos As DataTable, Optional ByVal lstInfoEnvioLectura As List(Of OrdenEnvioLectura.InfoEnvioLectura) = Nothing, Optional ByVal arrNombreColumna As ArrayList = Nothing)
            Dim fila As Integer = 1
            'If lstInfoEnvioLectura IsNot Nothing AndAlso lstInfoEnvioLectura.Count > 0 Then
            '    fila = lstInfoEnvioLectura.Count

            '    For index As Integer = 0 To lstInfoEnvioLectura.Count - 1
            '        Dim infoEnvio As New InfoEnvioLectura
            '        infoEnvio = CType(lstInfoEnvioLectura.Item(index), InfoEnvioLectura)

            '        OWs.Cells.GetSubrangeAbsolute(index, 0, index, dtDatos.Columns.Count - 1).Merged = True

            '        With OWs.Cells("A" & (index + 1).ToString())
            '            .Value = infoEnvio.campo & " " & infoEnvio.valor
            '            With .Style
            '                .Font.Color = Color.Black
            '                .Font.Weight = ExcelFont.BoldWeight
            '                .Font.Size = 16 * 16
            '            End With
            '        End With
            '    Next
            'End If

            Dim filaCelda As Integer

            If fila = 1 Then
                filaCelda = fila - 1
                OWs.InsertDataTable(dtDatos, "A" & (fila).ToString(), True)
            Else
                filaCelda = fila
                OWs.InsertDataTable(dtDatos, "A" & (fila + 1).ToString(), True)
            End If

            For i As Integer = 0 To dtDatos.Columns.Count - 1
                If arrNombreColumna IsNot Nothing Then OWs.Cells(filaCelda, i).Value = arrNombreColumna(i) ' valor inicial fila en 0
                With OWs.Cells(filaCelda, i).Style ' valor inicial fila en 0
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

            OWs.Cells.GetSubrangeAbsolute(dtDatos.Rows.Count + (filaCelda + 1), 0, (dtDatos.Rows.Count + (filaCelda + 1)), dtDatos.Columns.Count - 1).Merged = True ' valor inicial en 1

            With OWs.Cells("A" & (dtDatos.Rows.Count + (filaCelda + 2)).ToString).Style
                .FillPattern.SetPattern(FillPatternStyle.Solid, Color.LightGray, Color.LightGray)
                .Font.Color = Color.DarkBlue
                .Font.Weight = ExcelFont.BoldWeight
                .Borders.SetBorders(MultipleBorders.Top, Color.FromName("black"), LineStyle.Thin)
                .Borders.SetBorders(MultipleBorders.Right, Color.FromName("black"), LineStyle.Thin)
                .Borders.SetBorders(MultipleBorders.Left, Color.FromName("black"), LineStyle.Thin)
                .Borders.SetBorders(MultipleBorders.Bottom, Color.FromName("black"), LineStyle.Thin)
                .HorizontalAlignment = HorizontalAlignmentStyle.Center
            End With
            OWs.Cells("A" & (dtDatos.Rows.Count + (filaCelda + 2)).ToString).Value = dtDatos.Rows.Count & " Registro(s) Generado(s)" ' valor inicial fila en 2

            For index As Integer = 0 To dtDatos.Columns.Count - 1
                OWs.Columns(index).AutoFitAdvanced(1)
            Next
        End Sub

        ''' <summary>
        ''' Realiza el filtro de datos en un data table
        ''' </summary>
        ''' <param name="dt">datos a filtrar</param>
        ''' <param name="filtro">filtro realizado</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function AplicarFiltro(ByVal dt As DataTable, ByVal filtro As String) As DataTable
            dt.DefaultView.RowFilter = filtro
            Return dt.DefaultView.ToTable()
        End Function

        Public Function ObtenerDatosEnvio(ByVal idOrdenEnvioLectura As Integer) As InfoEnvioCorreo
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .TiempoEsperaComando = 600
                    .SqlParametros.Add("@idOrdenEnvioLectura", SqlDbType.Int).Value = idOrdenEnvioLectura
                    .ejecutarReader("ObtenerDatosEnvioLectura", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing And .Reader.HasRows Then
                        If .Reader.Read Then
                            _infoDatosEnvio.Guia = .Reader("guia").ToString
                            _infoDatosEnvio.Factura = .Reader("factura").ToString
                            Date.TryParse(.Reader("fechaLlegada").ToString, _infoDatosEnvio.FechaLlegada)
                            _infoDatosEnvio.Producto = .Reader("producto").ToString
                            _infoDatosEnvio.Subproducto = .Reader("subProducto").ToString
                            Integer.TryParse(.Reader("cantidad").ToString, _infoDatosEnvio.Cantidad)
                            _infoDatosEnvio.Material = .Reader("material").ToString
                            'Integer.TryParse(.Reader("idInstruccion").ToString, _infoDatosEnvio.IdInstruccion)
                            Short.TryParse(.Reader("tipoEnvio").ToString, _infoDatosEnvio.TipoEnvio)
                            Short.TryParse(.Reader("es_Serial").ToString, _infoDatosEnvio.Es_Serial)
                            Short.TryParse(.Reader("cantidadFactura").ToString, _infoDatosEnvio.CantidadFactura)
                            Integer.TryParse(.Reader("ordenEnInstruccion").ToString, _infoDatosEnvio.OrdenEnInstruccion)
                            Long.TryParse(.Reader("secuenciaGeneral").ToString, _infoDatosEnvio.SecuenciaGeneral)
                            Long.TryParse(.Reader("idOrdenEnvioLectura").ToString, _infoDatosEnvio.IdOrdenEnvioLectura)
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

        Public Shared Function ValidarParcialCompletaFactura(ByVal filtro As FiltroEnvioLectura) As Short
            Dim db As New LMDataAccess
            Dim result As Short
            With db
                .TiempoEsperaComando = 600
                With .SqlParametros
                    .Add("@idOrdenEnvioLectura", SqlDbType.Int).Value = filtro.IdOrdenEnvioLectura
                    .Add("@result", SqlDbType.Bit).Direction = ParameterDirection.ReturnValue
                End With

                Try
                    .ejecutarNonQuery("ValidarParcialCompletaFactura", CommandType.StoredProcedure)
                    Short.TryParse(.SqlParametros("@result").Value.ToString, result)
                Catch ex As Exception
                    Throw New Exception(ex.Message, ex)
                Finally
                    If db IsNot Nothing Then db.Dispose()
                End Try
            End With

            Return result
        End Function

#End Region

#Region "Enums"

        Public Enum Estado
            PendienteEnvio = 42
            Enviado = 43
            NacionalizadoParcialmente = 77
            Nacionalizado = 78
            Agrupado = 81
        End Enum

        Public Structure InfoEnvioLectura
            Dim campo As String
            Dim valor As String
        End Structure

        Public Structure InfoEnvioCorreo
            Dim Guia As String
            Dim Factura As String
            Dim FechaLlegada As Date
            Dim Producto As String
            Dim Subproducto As String
            Dim Material As String
            Dim IdInstruccion As Integer
            Dim TipoEnvio As Short
            Dim Cantidad As Integer
            Dim Es_Serial As Short
            Dim CantidadFactura As Integer
            Dim OrdenEnInstruccion As Integer
            Dim SecuenciaGeneral As Long
            Dim IdOrdenEnvioLectura As Long
        End Structure
#End Region

    End Class

End Namespace