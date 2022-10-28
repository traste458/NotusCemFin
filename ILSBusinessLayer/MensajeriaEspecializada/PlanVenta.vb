Imports LMDataAccessLayer
Imports System.String
Imports GemBox
Imports GemBox.Spreadsheet
Imports System.Web

Namespace MensajeriaEspecializada

    Public Class PlanVenta

#Region "Atributos (Campos)"

        Private _idPlan As Integer
        Private _nombrePlan As String
        Private _descripcion As String
        Private _cargoFijoMensual As Double
        Private _cargoFijoMensualConImpuesto As Double
        Private _impuesto As Double

        Private _activo As Nullable(Of Boolean)
        Private _idTipoPlan As Short
        Private _nombreTipoPlan As String
        Private _idUsuarioConsulta As Integer
        Private _idUsuario As Integer
        Private _listTipoServicio As List(Of Integer)

        Private _oExcel As ExcelFile
        Private _oEquipos As DataTable

        Private _estructuraTabla As DataTable
        Private _estructuraTablaErrores As DataTable

#End Region

#Region "Propiedades"

        Public Property Impuesto As Double
            Get
                Return _impuesto
            End Get
            Set(value As Double)
                _impuesto = value
            End Set
        End Property
        Public Property CargoFijoMensualConImpuesto As Double
            Get
                Return _cargoFijoMensualConImpuesto
            End Get
            Set(value As Double)
                _cargoFijoMensualConImpuesto = value
            End Set
        End Property

        Public Property IdPlan As Integer
            Get
                Return _idPlan
            End Get
            Set(value As Integer)
                _idPlan = value
            End Set
        End Property

        Public Property NombrePlan As String
            Get
                Return _nombrePlan
            End Get
            Set(value As String)
                _nombrePlan = value
            End Set
        End Property

        Public Property Descripcion As String
            Get
                Return _descripcion
            End Get
            Set(value As String)
                _descripcion = value
            End Set
        End Property

        Public Property CargoFijoMensual As Double
            Get
                Return _cargoFijoMensual
            End Get
            Set(value As Double)
                _cargoFijoMensual = value
            End Set
        End Property

        Public Property Activo As Boolean
            Get
                Return _activo
            End Get
            Set(value As Boolean)
                _activo = value
            End Set
        End Property

        Public Property IdTipoPlan As Short
            Get
                Return _idTipoPlan
            End Get
            Set(value As Short)
                _idTipoPlan = value
            End Set
        End Property

        Public Property NombreTipoPlan As String
            Get
                Return _nombreTipoPlan
            End Get
            Protected Friend Set(value As String)
                _nombreTipoPlan = value
            End Set
        End Property

        Public Property IdUsuarioConsulta As Integer
            Get
                Return _idUsuarioConsulta
            End Get
            Set(value As Integer)
                _idUsuarioConsulta = value
            End Set
        End Property

        Public Property IdUsuario As Integer
            Get
                Return _idUsuario
            End Get
            Set(value As Integer)
                _idUsuario = value
            End Set
        End Property
        Public Property ArchivoExcel As ExcelFile
            Get
                Return _oExcel
            End Get
            Set(value As ExcelFile)
                _oExcel = value
            End Set
        End Property

        Public Property EquiposManual As DataTable
            Get
                Return _oEquipos
            End Get
            Set(value As DataTable)
                _oEquipos = value
            End Set
        End Property

        Public Property EstructuraTabla() As DataTable
            Get
                If _estructuraTabla Is Nothing Then
                    EstructuraDatos()
                End If
                Return _estructuraTabla
            End Get
            Set(ByVal value As DataTable)
                _estructuraTabla = value
            End Set
        End Property

        Public Property EstructuraTablaErrores() As DataTable
            Get
                If _estructuraTablaErrores Is Nothing Then
                    EstructuraDatosErrores()
                End If
                Return _estructuraTablaErrores
            End Get
            Set(ByVal value As DataTable)
                _estructuraTablaErrores = value
            End Set
        End Property

        Public Property ListTipoServicio As List(Of Integer)
            Get
                If _listTipoServicio Is Nothing Then _listTipoServicio = New List(Of Integer)
                Return _listTipoServicio
            End Get
            Set(value As List(Of Integer))
                _listTipoServicio = value
            End Set
        End Property

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()

            If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                If Not EsNuloOVacio(HttpContext.Current.Session("usxp001")) Then Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, _idUsuarioConsulta)
            End If
        End Sub

        Public Sub New(ByVal idPlan As Integer)
            Me.New()
            _idPlan = idPlan
            CargarDatos()
        End Sub

#End Region

#Region "Métodos Privados"

        Private Sub CargarDatos()
            Using dbManager As New LMDataAccess
                With dbManager
                    If _idPlan > 0 Then .SqlParametros.Add("@idPlan", SqlDbType.VarChar).Value = _idPlan
                    If Not String.IsNullOrEmpty(_nombrePlan) Then .SqlParametros.Add("@nombrePlan", SqlDbType.VarChar).Value = _nombrePlan
                    If _activo IsNot Nothing Then
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                    Else
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = DBNull.Value
                    End If
                    If _idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = _idUsuarioConsulta

                    .ejecutarReader("ObtienePlanesDeVenta", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        If .Reader.Read Then
                            Integer.TryParse(.Reader("idPlan").ToString, _idPlan)
                            If Not IsDBNull(.Reader("nombrePlan")) Then _nombrePlan = .Reader("nombrePlan").ToString
                            If Not IsDBNull(.Reader("descripcion")) Then _descripcion = .Reader("descripcion").ToString
                            If Not IsDBNull(.Reader("cargoFijoMensual")) Then _cargoFijoMensual = .Reader("cargoFijoMensual").ToString
                            _activo = .Reader("activo")
                            Integer.TryParse(.Reader("idTipoPlan"), _idTipoPlan)
                            If Not IsDBNull(.Reader("nombreTipoPlan")) Then _nombreTipoPlan = .Reader("nombreTipoPlan")
                            If Not IsDBNull(.Reader("iva")) Then _impuesto = .Reader("iva")
                            'If Not IsDBNull(.Reader("cargoFijoMensualImpuesto")) Then _cargoFijoMensualConImpuesto = .Reader("cargoFijoMensualImpuesto")
                        End If
                        .Reader.Close()
                    End If
                End With
            End Using
        End Sub

        Private Sub EstructuraDatos()
            Try
                Dim dtDatos As New DataTable
                If _estructuraTabla Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("material", GetType(String)))
                        .Add(New DataColumn("precioEquipo", GetType(Double)))
                        .Add(New DataColumn("precioIvaEquipo", GetType(Double)))
                    End With
                    dtDatos.AcceptChanges()
                    _estructuraTabla = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub ValidarArchivo(ByVal ws As ExcelWorksheet)
            Dim index As Integer = 1
            Try
                For Each fila As ExcelRow In ws.Rows
                    If fila.AllocatedCells.Count <> Me.EstructuraTabla.Columns.Count Then
                        AdicionarError(index, "Fila inválida", "El Número de columnas de la fila es inválido.")
                    ElseIf index > 1 Then
                        If Not IsNumeric(fila.Cells(1).Value) Then
                            AdicionarError(index, "Dato inválido", "El valor del equipo debe ser numérico.")
                        End If
                        If Not IsNumeric(fila.Cells(2).Value) Then
                            AdicionarError(index, "Dato inválido", "El valor del iva debe ser numérico.")
                        End If
                    End If
                    index += 1
                Next
            Catch ex As Exception
                Throw New Exception("Se generó un error en la validación del archivo, por favor elimine las filas y columnas vacías e intente nuevamente.")
            End Try
        End Sub

        Private Sub AdicionarError(ByVal id As Integer, ByVal nombre As String, ByVal descripcion As String)
            Try
                With EstructuraTablaErrores
                    Dim drError As DataRow = .NewRow()
                    With drError
                        .Item("id") = id
                        .Item("nombre") = nombre
                        .Item("descripcion") = descripcion
                    End With
                    .Rows.Add(drError)
                    .AcceptChanges()
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub EstructuraDatosErrores()
            Try
                Dim dtDatos As New DataTable
                If _estructuraTablaErrores Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("id", GetType(Integer)))
                        .Add(New DataColumn("nombre", GetType(String)))
                        .Add(New DataColumn("descripcion", GetType(String)))
                    End With
                    dtDatos.AcceptChanges()
                    _estructuraTablaErrores = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub ExtractDataErrorHandler(ByVal sender As Object, ByVal e As ExtractDataDelegateEventArgs)
            If e.ErrorID = ExtractDataError.WrongType Then
                If Not IsNumeric(e.ExcelValue) And e.ExcelValue = Nothing Then
                    e.DataTableValue = Nothing
                Else
                    e.DataTableValue = e.ExcelValue.ToString()
                End If

                If e.DataTableValue = Nothing Then
                    e.Action = ExtractDataEventAction.SkipRow
                Else
                    e.Action = ExtractDataEventAction.Continue
                End If
            End If
        End Sub

#End Region

#Region "Métodos Públicos"

        Public Function Registrar() As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim wsData As ExcelWorksheet = Nothing
            Using dbManager As New LMDataAccess
                Try
                    Dim dtDatos As DataTable = EstructuraTabla()
                    Dim idUsuario As Integer = CInt(HttpContext.Current.Session("usxp001"))

                    If _oExcel IsNot Nothing Then
                        wsData = _oExcel.Worksheets(0)
                        ValidarArchivo(wsData)
                    End If

                    If EstructuraTablaErrores.Rows.Count = 0 Then
                        If _oExcel IsNot Nothing Then
                            AddHandler wsData.ExtractDataEvent, AddressOf ExtractDataErrorHandler
                            wsData.ExtractToDataTable(dtDatos, wsData.Rows.Count, ExtractDataOptions.SkipEmptyRows, wsData.Rows(1), wsData.Columns(0))

                            For Each fila As DataRow In dtDatos.Select("material is NULL")
                                fila.Delete()
                            Next
                        End If
                        If _oEquipos.Rows.Count > 0 Then
                            dtDatos.Merge((_oEquipos.DefaultView).ToTable(True, "material", "precioEquipo", "precioIvaEquipo"))
                        End If

                        If dtDatos.Columns.Contains("idUsuario") Then dtDatos.Columns.Remove("idUsuario")
                        dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idUsuario))

                        With dbManager
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                            .ejecutarNonQuery("LiberarDatosTransitoriosMaterialPlanVenta", CommandType.StoredProcedure)

                            .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                            .TiempoEsperaComando = 600000
                            With .BulkCopy
                                .DestinationTableName = "TransitoriaMaterialPlanVenta"
                                .ColumnMappings.Add("material", "material")
                                .ColumnMappings.Add("precioEquipo", "precioVentaEquipo")
                                .ColumnMappings.Add("precioIvaEquipo", "ivaEquipo")
                                .ColumnMappings.Add("idUsuario", "idUsuario")
                                .WriteToServer(dtDatos)
                            End With

                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                            _estructuraTablaErrores = .ejecutarDataTable("ValidarMaterialesPlanVenta", CommandType.StoredProcedure)

                            If _estructuraTablaErrores.Rows.Count = 0 Then
                                .SqlParametros.Clear()
                                .SqlParametros.Add("@nombrePlan", SqlDbType.VarChar).Value = _nombrePlan
                                .SqlParametros.Add("@desripcionPlan", SqlDbType.VarChar).Value = _descripcion
                                .SqlParametros.Add("@cargoFijoMensual", SqlDbType.Money).Value = _cargoFijoMensual
                                .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                                .SqlParametros.Add("@idTipoPlan", SqlDbType.SmallInt).Value = _idTipoPlan
                                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                                .SqlParametros.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                                .iniciarTransaccion()
                                .ejecutarNonQuery("RegistrarPlanVentaTelefonica", CommandType.StoredProcedure)

                                Dim respuesta As Integer = .SqlParametros("@result").Value
                                If respuesta = 0 Then
                                    resultado.EstablecerMensajeYValor(respuesta, "El plan fue registrado exitosamente.")
                                    .confirmarTransaccion()
                                Else
                                    Select Case respuesta
                                        Case 1 : resultado.EstablecerMensajeYValor(respuesta, "El nombre del plan ya se encuentra registrado en el sistema.")
                                        Case Else : resultado.EstablecerMensajeYValor(respuesta, "Error inesperado [" & respuesta & "]")
                                    End Select
                                    .abortarTransaccion()
                                End If
                            Else
                                resultado.EstablecerMensajeYValor(1, "No se logro realizar el cargue de las ventas, por favor verifique el log de errores.")
                            End If
                        End With
                    Else
                        resultado.EstablecerMensajeYValor(1, "Se encontraron errores en el archivo, por favor verifique el log de resultados.")
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Function Actualizar() As ResultadoProceso
            Dim resultado As New ResultadoProceso()
            Using dbManager As New LMDataAccess
                Try
                    If _idPlan > 0 Then
                        With dbManager
                            .SqlParametros.Add("@idPlan", SqlDbType.Int).Value = _idPlan
                            If Not String.IsNullOrEmpty(_nombrePlan) Then .SqlParametros.Add("@nombrePlan", SqlDbType.VarChar).Value = _nombrePlan
                            If Not String.IsNullOrEmpty(_descripcion) Then .SqlParametros.Add("@desripcionPlan", SqlDbType.VarChar).Value = _descripcion
                            If _cargoFijoMensual > 0 Then .SqlParametros.Add("@cargoFijoMensual", SqlDbType.Money).Value = _cargoFijoMensual
                            If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Money).Value = _idUsuario
                            If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                            If _idTipoPlan > 0 Then .SqlParametros.Add("@idTipoPlan", SqlDbType.SmallInt).Value = _idTipoPlan
                            .SqlParametros.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                            .iniciarTransaccion()
                            .ejecutarNonQuery("ActualizarPlanVentaTelefonica", CommandType.StoredProcedure)

                            Dim respuesta As Integer = .SqlParametros("@result").Value
                            If respuesta = 0 Then
                                resultado.EstablecerMensajeYValor(respuesta, "El plan fue actualizado exitosamente.")
                                .confirmarTransaccion()
                            Else
                                .abortarTransaccion()
                                resultado.EstablecerMensajeYValor(respuesta, "Error inesperado [" & respuesta & "]")
                            End If
                        End With
                    Else
                        resultado.EstablecerMensajeYValor(100, "No se propoercionaron los datos necesarios para realizar la actualización")
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
            Return resultado
        End Function

        Public Function RegistrarSiembra(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .Add("@nombre", SqlDbType.VarChar, 2000).Value = _nombrePlan
                        .Add("@descripcion", SqlDbType.VarChar, 2000).Value = _descripcion
                        .Add("@cargoFijo", SqlDbType.Money).Value = _cargoFijoMensual
                        .Add("@idTipoPlan", SqlDbType.Int).Value = _idTipoPlan
                        If _listTipoServicio IsNot Nothing AndAlso _listTipoServicio.Count > 0 Then _
                        .Add("@listTipoServicio", SqlDbType.VarChar).Value = String.Join(",", _listTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("RegistrarPlanVenta", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente. ")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional() Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(500, "Se generó un error al realizar el registro: " & ex.Message)
            End Try
            Return resultado
        End Function

        Public Function ActualizarSiembra(ByVal idUsuario As Integer) As ResultadoProceso
            Dim resultado As New ResultadoProceso
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    With .SqlParametros
                        .Add("@idPlan", SqlDbType.Int).Value = _idPlan
                        .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        If Not String.IsNullOrEmpty(_nombrePlan) Then .Add("@nombre", SqlDbType.VarChar, 2000).Value = _nombrePlan
                        If Not String.IsNullOrEmpty(_descripcion) Then .Add("@descripcion", SqlDbType.VarChar, 2000).Value = _descripcion
                        If _cargoFijoMensual > 0 Then .Add("@cargoFijoMensual", SqlDbType.Money).Value = _cargoFijoMensual
                        If _activo IsNot Nothing Then .Add("@activo", SqlDbType.Bit).Value = _activo
                        If _idTipoPlan > 0 Then .Add("@idTipoPlan", SqlDbType.Int).Value = _idTipoPlan
                        If _listTipoServicio IsNot Nothing AndAlso _listTipoServicio.Count > 0 Then _
                        .Add("@listTipoServicio", SqlDbType.VarChar).Value = String.Join(",", _listTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
                        .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                        .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    End With
                    .IniciarTransaccion()
                    .EjecutarNonQuery("ActualizarPlanVenta", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .ConfirmarTransaccion()
                        Else
                            .AbortarTransaccion()
                        End If
                    Else
                        .AbortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer la respuesta del servidor, por favor intentelo nuevamente. ")
                    End If
                End With
            Catch ex As Exception
                If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                resultado.EstablecerMensajeYValor(500, "Se generó un error al realizar la actualización del registro: " & ex.Message)
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace

