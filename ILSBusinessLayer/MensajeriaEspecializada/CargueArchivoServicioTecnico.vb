Imports System.Web
Imports GemBox.Spreadsheet
Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes
Imports System.IO

Public Class CargueArchivoServicioTecnico

#Region "Atributos"

    Private oExcel As ExcelFile
    Private _estructuraTablaBase As DataTable
    Private _estructuraTabla As DataTable
    Private _estructuraTablaErrores As DataTable

#End Region

#Region "Propiedades"

    Public Property ArchivoExcel As ExcelFile
        Get
            Return oExcel
        End Get
        Set(value As ExcelFile)
            oExcel = value
        End Set
    End Property

    Public Property EstructuraTablaBase() As DataTable
        Get
            If _estructuraTablaBase Is Nothing Then
                EstructuraDatosBase()
            End If
            Return _estructuraTablaBase
        End Get
        Set(ByVal value As DataTable)
            _estructuraTablaBase = value
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

#End Region

#Region "Constructores"

    Public Sub New(ByRef ArchivoExcel As ExcelFile)
        MyBase.New()
        oExcel = ArchivoExcel
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub EstructuraDatosBase()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTablaBase Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("sucursal", GetType(String)))
                    .Add(New DataColumn("fechaReparacion", GetType(DateTime)))
                    .Add(New DataColumn("imeiEquipoReparado", GetType(String)))
                    .Add(New DataColumn("ods", GetType(Long)))
                    .Add(New DataColumn("marca", GetType(String)))
                    .Add(New DataColumn("modelo", GetType(String)))
                    .Add(New DataColumn("msisdn", GetType(String)))
                    .Add(New DataColumn("identificaion", GetType(String)))
                    .Add(New DataColumn("nombre", GetType(String)))
                    .Add(New DataColumn("direccion", GetType(String)))
                    .Add(New DataColumn("telefono", GetType(String)))
                    .Add(New DataColumn("tipoTelefono", GetType(String)))
                    .Add(New DataColumn("nombrePlan", GetType(String)))
                    .Add(New DataColumn("prestamo", GetType(String)))
                    .Add(New DataColumn("imeiPrestamo", GetType(String)))
                    .Add(New DataColumn("marcaPrestamo", GetType(String)))
                    .Add(New DataColumn("modeloPrestamo", GetType(String)))
                    .Add(New DataColumn("observaciones", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTablaBase = dtDatos
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub EstructuraDatos()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTabla Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("sucursal", GetType(String)))
                    .Add(New DataColumn("fechaReparacion", GetType(DateTime)))
                    .Add(New DataColumn("imeiEquipoReparado", GetType(String)))
                    .Add(New DataColumn("ods", GetType(Long)))
                    .Add(New DataColumn("marca", GetType(String)))
                    .Add(New DataColumn("modelo", GetType(String)))
                    .Add(New DataColumn("msisdn", GetType(String)))
                    .Add(New DataColumn("identificaion", GetType(String)))
                    .Add(New DataColumn("nombre", GetType(String)))
                    .Add(New DataColumn("direccion", GetType(String)))
                    .Add(New DataColumn("telefono", GetType(String)))
                    .Add(New DataColumn("tipoTelefono", GetType(String)))
                    .Add(New DataColumn("nombrePlan", GetType(String)))
                    .Add(New DataColumn("prestamo", GetType(String)))
                    .Add(New DataColumn("imeiPrestamo", GetType(String)))
                    .Add(New DataColumn("marcaPrestamo", GetType(String)))
                    .Add(New DataColumn("modeloPrestamo", GetType(String)))
                    .Add(New DataColumn("observaciones", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTabla = dtDatos
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AdicionarError(ByVal id As Integer, ByVal nombre As String, ByVal descripcion As String)
        Try
            With EstructuraTablaErrores
                Dim drError As DataRow = .NewRow()
                With drError
                    .Item("id") = id
                    .Item("Novedad") = nombre
                    .Item("Mensaje") = descripcion
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
                    .Add(New DataColumn("Novedad", GetType(String)))
                    .Add(New DataColumn("Mensaje", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTablaErrores = dtDatos
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ExtractDataErrorHandler(sender As Object, e As ExtractDataDelegateEventArgs)
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

    Private Sub AdicionarColumnas()
        Try
            'Se crean los campos de los materiales en la estructura de tabla
            Dim index As Integer = 1
            Dim fila As ExcelRow = oExcel.Worksheets(0).Rows(0)

            Dim dtDatos As DataTable = EstructuraTabla()
            AddHandler oExcel.Worksheets(0).ExtractDataEvent, AddressOf ExtractDataErrorHandler
            oExcel.Worksheets(0).ExtractToDataTable(dtDatos, oExcel.Worksheets(0).Rows.Count, ExtractDataOptions.SkipEmptyRows, oExcel.Worksheets(0).Rows(1), oExcel.Worksheets(0).Columns(0))

            'Se crea la estructura por Filas
            For Each registro As DataRow In dtDatos.Rows
                Dim registroFinal As DataRow = EstructuraTablaBase.NewRow()
                With registroFinal
                    .Item("sucursal") = registro("sucursal").ToString.Trim
                    .Item("fechaReparacion") = registro("fechaReparacion").ToString.Trim
                    .Item("imeiEquipoReparado") = registro("imeiEquipoReparado").ToString.Trim
                    .Item("ods") = registro("ods").ToString.Trim
                    .Item("marca") = registro("marca").ToString.Trim
                    .Item("modelo") = registro("modelo").ToString.Trim
                    .Item("msisdn") = registro("msisdn").ToString.Trim
                    .Item("identificaion") = registro("identificaion").ToString.Trim
                    .Item("nombre") = registro("nombre").ToString.Trim
                    .Item("direccion") = registro("direccion").ToString.Trim
                    .Item("telefono") = registro("telefono").ToString.Trim
                    .Item("tipoTelefono") = registro("tipoTelefono").ToString.Trim
                    .Item("nombrePlan") = registro("nombrePlan").ToString.Trim
                    .Item("prestamo") = registro("prestamo").ToString.Trim
                    .Item("imeiPrestamo") = registro("imeiPrestamo").ToString.Trim
                    .Item("marcaPrestamo") = registro("marcaPrestamo").ToString.Trim
                    .Item("modeloPrestamo") = registro("modeloPrestamo").ToString.Trim
                    .Item("observaciones") = registro("observaciones").ToString.Trim
                End With
                EstructuraTablaBase.Rows.Add(registroFinal)
                index = index + 1
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function HayDatosEnFila(ByVal infoFila As ExcelRow)
        Dim resultado As Boolean = False
        For index As Integer = 0 To infoFila.AllocatedCells.Count
            If infoFila.AllocatedCells(index).Value IsNot Nothing AndAlso Not String.IsNullOrEmpty(infoFila.AllocatedCells(index).Value.ToString) Then
                resultado = True
                Exit For
            End If
        Next
        Return resultado
    End Function

    Private Function ObternerHoja(ByVal ruta As String) As ExcelWorksheet
        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
        Dim miExcel As New ExcelFile
        Dim miWs As ExcelWorksheet
        Try
            Try
                Dim extension As String = Path.GetExtension(ruta).ToString().ToLower()
                Select Case extension
                    Case ".xls"
                        miExcel.LoadXls(ruta)
                    Case ".xlsx"
                        miExcel.LoadXlsx(ruta, XlsxOptions.None)
                End Select

            Catch ex As Exception
                Throw New Exception("Se presento un error: " & ex.Message)
                Try
                    miExcel.LoadCsv(ruta, CsvType.TabDelimited)
                Catch es As Exception
                    Throw New Exception("El archivo esta incorrecto o no tiene el formato esperado. Por favor verifique")
                End Try
            End Try

            If miExcel.Worksheets.Count > 0 Then
                miWs = miExcel.Worksheets(0)
                If miWs.Rows.Count > 1 Then
                    Return miWs
                Else : Throw New Exception("El archivo especificado no contiende Datos. Por favor verifique")
                End If
            Else : Throw New Exception("El archivo especificado no contiende Hojas. Por favor verifique")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Private Sub AdicionarRegistro(ByVal sucursal As String, ByVal fechaReparacion As String, ByVal imeiEquipoReparado As String, ByVal ods As Long _
                                  , ByVal marca As String, ByVal modelo As String, ByVal msisdn As String, ByVal identificaion As String _
                                  , ByVal nombre As String, ByVal direccion As String, ByVal telefono As String, ByVal tipoTelefono As String _
                                  , ByVal nombrePlan As String, ByVal prestamo As String, ByVal imeiPrestamo As String, ByVal marcaPrestamo As String _
                                  , ByVal modeloPrestamo As String, ByVal observaciones As String)
        Try
            With EstructuraTablaBase
                Dim drRegistro As DataRow = .NewRow()
                With drRegistro
                    .Item("sucursal") = sucursal
                    .Item("fechaReparacion") = fechaReparacion
                    .Item("imeiEquipoReparado") = imeiEquipoReparado
                    .Item("ods") = ods
                    .Item("marca") = marca
                    .Item("modelo") = modelo
                    .Item("msisdn") = msisdn
                    .Item("identificaion") = identificaion
                    .Item("nombre") = nombre
                    .Item("direccion") = direccion
                    .Item("telefono") = telefono
                    .Item("tipoTelefono") = tipoTelefono
                    .Item("nombrePlan") = nombrePlan
                    .Item("prestamo") = prestamo
                    .Item("imeiPrestamo") = imeiPrestamo
                    .Item("marcaPrestamo") = marcaPrestamo
                    .Item("modeloPrestamo") = modeloPrestamo
                    .Item("observaciones") = observaciones
                End With
                .Rows.Add(drRegistro)
                .AcceptChanges()
            End With
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ValidarEstructura(ByVal ruta As String) As Boolean
        Dim esValido As Boolean = True
        Dim numInte As Integer
        Dim fecha As DateTime
        Dim dato As String
        Dim hayDatos As Boolean
        Dim expresion As New ConfigValues("EXP_REG_CONSULTA_MSISDN_AR")
        Dim oExpReg As New System.Text.RegularExpressions.Regex(expresion.ConfigKeyValue)
        Dim expresion1 As New ConfigValues("EXPREG_DESTINATARIO_POP_COD")
        Dim oExpReg1 As New System.Text.RegularExpressions.Regex(expresion1.ConfigKeyValue)
        Dim expresion2 As New ConfigValues("EXPREG_PEDIDOS_PAPELERIA")
        Dim oExpReg2 As New System.Text.RegularExpressions.Regex(expresion2.ConfigKeyValue)
        Dim miWs As ExcelWorksheet
        miWs = ObternerHoja(ruta)

        Try
            Dim numCeldasActivas As Integer

            For index As Integer = 1 To miWs.Rows.Count - 1
                With miWs.Rows
                    If Not String.IsNullOrEmpty(.Item(index).Cells.FirstRowIndex) Then
                        hayDatos = HayDatosEnFila(.Item(index))
                        If numCeldasActivas <> Me.EstructuraTabla.Columns.Count AndAlso hayDatos Then
                            If String.IsNullOrEmpty(.Item(index).Cells(0).Value) Then
                                AdicionarError(index, "Dato inválido", "La sucursal no puede estar vacia.")
                            Else
                                If Not oExpReg2.IsMatch(.Item(index).Cells(0).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para la sucursal, no son válidas.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(1).Value) Then
                                AdicionarError(index, "Dato inválido", "La fecha de reparación no puede estar vacia.")
                            Else
                                If Not IsDate(.Item(index).Cells(1).Value) OrElse Not Date.TryParse(.Item(index).Cells(1).Value, fecha) Then
                                    AdicionarError(index, "Dato inválido", "La fecha de reparación no tiene un formato válido.")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(2).Value) Then
                                AdicionarError(index, "Dato inválido", "El campor del IMEI Reparado no puede estar vacío.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(3).Value) Then
                                AdicionarError(index, "Dato inválido", "El número de la ODS no puede estar vacío.")
                            Else
                                If Not IsNumeric(.Item(index).Cells(3).Value) OrElse Not Integer.TryParse(.Item(index).Cells(3).Value, numInte) Then
                                    AdicionarError(index, "Dato inválido", "El número de la ODS debe ser numérico.")
                                Else
                                    If (.Item(index).Cells(3).Value) < 0 Then _
                                    AdicionarError(index, "Dato inválido", "El número de la ODS debe ser numérico y mayor que 0.")
                                End If

                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(4).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo de marca, no puede estar vacío.")
                            Else
                                If Not oExpReg2.IsMatch(.Item(index).Cells(4).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para la marca, no son válidas.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(5).Value) Then
                                AdicionarError(index, "Dato inválido", "El modelo no puede estar vacío.")
                            Else
                                If Not oExpReg2.IsMatch(.Item(index).Cells(4).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el modelo, no son válidas.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(6).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo MSISDN, no puede estar vacío.")
                            Else
                                If Not oExpReg.IsMatch(.Item(index).Cells(6).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el MSISND, no son válidas.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(7).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Identificación', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(7).Value
                                If dato.Length > 50 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'Identificación' no es válido.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(8).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Nombre Persona', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(8).Value
                                If dato.Length > 150 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'Nombre Persona' no es válido.")
                                If Not oExpReg2.IsMatch(.Item(index).Cells(8).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Nombre Persona', no es válida.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(9).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Dirección', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(9).Value
                                If dato.Length > 200 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'Dirección' no es válido.")
                                If Not oExpReg2.IsMatch(.Item(index).Cells(9).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Dirección', no es válida.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(10).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Teléfono', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(10).Value
                                If dato.Length > 50 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'Teléfono' no es válido.")
                                If Not oExpReg1.IsMatch(.Item(index).Cells(10).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Teléfono', no es válida.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(11).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Tipo Teléfono', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(11).Value
                                If dato.Length > 7 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'Tipo Teléfono' no es válido.")
                                If dato.ToString <> "CELULAR" Then
                                    If dato.ToString <> "FIJO" Then
                                        AdicionarError(index, "Dato inválido", "El texto permitido para el campo 'Tipo Teléfono', debe ser 'CELULAR o FIJO'.")
                                    End If
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(12).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'nombrePlan', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(12).Value
                                If dato.Length > 50 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'nombrePlan' no es válido.")
                                If Not oExpReg2.IsMatch(.Item(index).Cells(12).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'nombrePlan', no es válida.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(13).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Préstamo', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(13).Value
                                If dato.Length > 2 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'Péstamo' no es válido.")
                                If dato.ToString <> "SI" Then
                                    If dato.ToString <> "NO" Then
                                        AdicionarError(index, "Dato inválido", "El texto permitido para el campo 'Péstamo', debe ser 'SI o NO'.")
                                    End If
                                End If
                                If dato.ToString = "SI" Then
                                    If String.IsNullOrEmpty(.Item(index).Cells(14).Value) Then
                                        AdicionarError(index, "Dato inválido", "El campo 'Imei Préstamo', no puede estar vacío.")
                                    Else
                                        dato = .Item(index).Cells(14).Value
                                        If dato.Length > 20 Then _
                                            AdicionarError(index, "Dato inválido", "La longuitud campo 'Imei Péstamo' no es válido.")
                                        If Not oExpReg2.IsMatch(.Item(index).Cells(14).Value) Then _
                                        AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Imei Péstamo', no es válida.")
                                    End If

                                    If String.IsNullOrEmpty(.Item(index).Cells(15).Value) Then
                                        AdicionarError(index, "Dato inválido", "El campo 'Marca Préstamo', no puede estar vacío.")
                                    Else
                                        dato = .Item(index).Cells(15).Value
                                        If dato.Length > 150 Then _
                                            AdicionarError(index, "Dato inválido", "La longuitud campo 'Marca Péstamo' no es válido.")
                                        If Not oExpReg2.IsMatch(.Item(index).Cells(15).Value) Then _
                                        AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Marca Péstamo', no es válida.")
                                    End If

                                    If String.IsNullOrEmpty(.Item(index).Cells(16).Value) Then
                                        AdicionarError(index, "Dato inválido", "El campo 'Modelo Préstamo', no puede estar vacío.")
                                    Else
                                        dato = .Item(index).Cells(16).Value
                                        If dato.Length > 150 Then _
                                            AdicionarError(index, "Dato inválido", "La longuitud campo 'Modelo Péstamo' no es válido.")
                                        If Not oExpReg2.IsMatch(.Item(index).Cells(16).Value) Then _
                                        AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Modelo Péstamo', no es válida.")
                                    End If
                                End If
                            End If

                            If Not String.IsNullOrEmpty(.Item(index).Cells(17).Value) Then
                                dato = .Item(index).Cells(17).Value
                                If dato.Length > 150 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'Observación' no es válido.")
                                If Not oExpReg2.IsMatch(.Item(index).Cells(17).Value) Then _
                                AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el campo 'Observación', no es válida.")
                            End If

                            If EstructuraTablaErrores.Rows.Count = 0 Then
                                AdicionarRegistro(.Item(index).Cells(0).Value, .Item(index).Cells(1).Value, .Item(index).Cells(2).Value, .Item(index).Cells(3).Value _
                                                    , .Item(index).Cells(4).Value, .Item(index).Cells(5).Value, .Item(index).Cells(6).Value, .Item(index).Cells(7).Value _
                                                    , .Item(index).Cells(8).Value, .Item(index).Cells(9).Value, .Item(index).Cells(10).Value, .Item(index).Cells(11).Value _
                                                    , .Item(index).Cells(12).Value, .Item(index).Cells(13).Value, .Item(index).Cells(14).Value, .Item(index).Cells(15).Value _
                                                    , .Item(index).Cells(16).Value, .Item(index).Cells(17).Value)
                            End If
                        ElseIf hayDatos Then
                            AdicionarError(index, "Dato inválido", "El Número de columnas de la .Item(index) es inválido. ")
                        Else
                            AdicionarError(index, "Dato inválido", "El número de línea se encuentra vacia, por favor verificar.")
                        End If
                    Else
                        AdicionarError(index, "Dato inválido", "El número de línea se encuentra vacia, por favor verificar. ")
                    End If
                End With
            Next
            esValido = Not (EstructuraTablaErrores.Rows.Count > 0)
        Catch ex As Exception
            Throw ex
        End Try
        Return esValido
    End Function

    Public Function ValidarInformacion() As Boolean
        Dim esValido As Boolean = True
        Try
            'AdicionarColumnas()
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("usxp001"))

            If EstructuraTablaBase.Columns.Contains("idUsuario") Then EstructuraTablaBase.Columns.Remove("idUsuario")
            EstructuraTablaBase.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idUsuario))

            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .EjecutarNonQuery("LiberarDatosTransitorioST", CommandType.StoredProcedure)

                    .InicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 600000
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaServicioTecnico"
                        .ColumnMappings.Add("sucursal", "sucursal")
                        .ColumnMappings.Add("fechaReparacion", "fechaReparacion")
                        .ColumnMappings.Add("imeiEquipoReparado", "imeiEquipoReparado")
                        .ColumnMappings.Add("ods", "ods")
                        .ColumnMappings.Add("marca", "marca")
                        .ColumnMappings.Add("modelo", "modelo")
                        .ColumnMappings.Add("msisdn", "msisdn")
                        .ColumnMappings.Add("identificaion", "identificaion")
                        .ColumnMappings.Add("nombre", "nombre")
                        .ColumnMappings.Add("direccion", "direccion")
                        .ColumnMappings.Add("telefono", "telefono")
                        .ColumnMappings.Add("tipoTelefono", "tipoTelefono")
                        .ColumnMappings.Add("nombrePlan", "nombrePlan")
                        .ColumnMappings.Add("prestamo", "prestamo")
                        .ColumnMappings.Add("imeiPrestamo", "imeiPrestamo")
                        .ColumnMappings.Add("marcaPrestamo", "marcaPrestamo")
                        .ColumnMappings.Add("modeloPrestamo", "modeloPrestamo")
                        .ColumnMappings.Add("observaciones", "observaciones")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(EstructuraTablaBase)
                    End With

                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    _estructuraTablaErrores = .EjecutarDataTable("ValidarDatosTransitoriosST", CommandType.StoredProcedure)

                    esValido = (EstructuraTablaErrores.Rows.Count = 0)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return esValido
    End Function

    Public Function RegistrarServicioEquiposReparadosST() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("usxp001"))
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                .EjecutarNonQuery("RegistrarServicioEquiposReparadosST", CommandType.StoredProcedure)

                If (Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor)) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With

        Catch ex As Exception
            If dbManager IsNot Nothing Then dbManager.Dispose()
            resultado.EstablecerMensajeYValor(500, "Se generó un error al almacenar los mines: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

End Class
