Imports System.Web
Imports GemBox.Spreadsheet
Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes
Imports System.IO

Public Class CargueArchivoMinesCorporativoSoloSim

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
                    .Add(New DataColumn("msisdn", GetType(String)))
                    .Add(New DataColumn("region", GetType(String)))
                    .Add(New DataColumn("precioUnitarioSinDescuento", GetType(Integer)))
                    .Add(New DataColumn("precioUnitario", GetType(Integer)))
                    .Add(New DataColumn("precioEspecial", GetType(String)))
                    .Add(New DataColumn("tipoSim", GetType(String)))
                    .Add(New DataColumn("codigoCuenta", GetType(String)))
                    .Add(New DataColumn("nombrePlan", GetType(String)))
                    .Add(New DataColumn("tipoPlanVozDatos", GetType(String)))
                    .Add(New DataColumn("valorCargoBasicoPlanSinImpuesto", GetType(String)))
                    .Add(New DataColumn("paquete", GetType(String)))
                    .Add(New DataColumn("clausula", GetType(String)))
                    .Add(New DataColumn("valorClausula", GetType(String)))
                    .Add(New DataColumn("ventaEquipoContado", GetType(String)))
                    .Add(New DataColumn("ventaEquipoCuotas", GetType(String)))
                    .Add(New DataColumn("numeroCuotasVenta", GetType(String)))
                    .Add(New DataColumn("nombreCanalVenta", GetType(String)))
                    .Add(New DataColumn("codigoCanalVenta", GetType(String)))
                    .Add(New DataColumn("solicitudServicioNumero", GetType(String)))
                    .Add(New DataColumn("contratoCompraVentaEquipo", GetType(String)))
                    .Add(New DataColumn("nombreEjecutivoVenta", GetType(String)))
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
                    .Add(New DataColumn("msisdn", GetType(String)))
                    .Add(New DataColumn("region", GetType(String)))
                    .Add(New DataColumn("precioUnitarioSinDescuento", GetType(Integer)))
                    .Add(New DataColumn("precioUnitario", GetType(Integer)))
                    .Add(New DataColumn("precioEspecial", GetType(String)))
                    .Add(New DataColumn("tipoSim", GetType(String)))
                    .Add(New DataColumn("codigoCuenta", GetType(String)))
                    .Add(New DataColumn("nombrePlan", GetType(String)))
                    .Add(New DataColumn("tipoPlanVozDatos", GetType(String)))
                    .Add(New DataColumn("valorCargoBasicoPlanSinImpuesto", GetType(String)))
                    .Add(New DataColumn("paquete", GetType(String)))
                    .Add(New DataColumn("clausula", GetType(String)))
                    .Add(New DataColumn("valorClausula", GetType(String)))
                    .Add(New DataColumn("ventaEquipoContado", GetType(String)))
                    .Add(New DataColumn("ventaEquipoCuotas", GetType(String)))
                    .Add(New DataColumn("numeroCuotasVenta", GetType(String)))
                    .Add(New DataColumn("nombreCanalVenta", GetType(String)))
                    .Add(New DataColumn("codigoCanalVenta", GetType(String)))
                    .Add(New DataColumn("solicitudServicioNumero", GetType(String)))
                    .Add(New DataColumn("contratoCompraVentaEquipo", GetType(String)))
                    .Add(New DataColumn("nombreEjecutivoVenta", GetType(String)))
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
                    .Item("msisdn") = registro("msisdn").ToString.Trim
                    .Item("region") = registro("region").ToString.Trim
                    .Item("precioUnitarioSinDescuento") = registro("precioUnitarioSinDescuento").ToString.Trim
                    .Item("precioUnitario") = registro("precioUnitario").ToString.Trim
                    .Item("precioEspecial") = registro("precioEspecial").ToString.Trim
                    .Item("tipoSim") = registro("tipoSim").ToString.Trim
                    .Item("codigoCuenta") = registro("codigoCuenta").ToString.Trim
                    .Item("nombrePlan") = registro("nombrePlan").ToString.Trim
                    .Item("tipoPlanVozDatos") = registro("tipoPlanVozDatos").ToString.Trim
                    .Item("valorCargoBasicoPlanSinImpuesto") = registro("valorCargoBasicoPlanSinImpuesto").ToString.Trim
                    .Item("paquete") = registro("paquete").ToString.Trim
                    .Item("clausula") = registro("clausula").ToString.Trim
                    .Item("valorClausula") = registro("valorClausula").ToString.Trim
                    .Item("ventaEquipoContado") = registro("ventaEquipoContado").ToString.Trim
                    .Item("ventaEquipoCuotas") = registro("ventaEquipoCuotas").ToString.Trim
                    .Item("numeroCuotasVenta") = registro("numeroCuotasVenta").ToString.Trim
                    .Item("nombreCanalVenta") = registro("nombreCanalVenta").ToString.Trim
                    .Item("codigoCanalVenta") = registro("codigoCanalVenta").ToString.Trim
                    .Item("solicitudServicioNumero") = registro("solicitudServicioNumero").ToString.Trim
                    .Item("contratoCompraVentaEquipo") = registro("contratoCompraVentaEquipo").ToString.Trim
                    .Item("nombreEjecutivoVenta") = registro("nombreEjecutivoVenta").ToString.Trim
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

    Private Sub AdicionarRegistro(ByVal msisdn As String, ByVal region As String, ByVal precioUnitarioSinDescuento As Long, ByVal precioUnitario As Long _
                                  , ByVal precioEspecial As String, ByVal tipoSim As String, ByVal codigoCuenta As String, ByVal nombrePlan As String _
                                  ,  ByVal tipoPlanVozDatos As String, ByVal valorCargoBasicoPlanSinImpuesto As String _
                                  , ByVal paquete As String, ByVal clausula As String, ByVal valorClausula As String, ByVal ventaEquipoContado As String _
                                  , ByVal ventaEquipoCuotas As String, ByVal numeroCuotasVenta As String, ByVal nombreCanalVenta As String, ByVal codigoCanalVenta As String _
                                  , ByVal solicitudServicioNumero As String, ByVal contratoCompraVentaEquipo As String, ByVal nombreEjecutivoVenta As String)
        Try
            With EstructuraTablaBase
                Dim drRegistro As DataRow = .NewRow()
                With drRegistro
                    .Item("msisdn") = msisdn
                    .Item("region") = region
                    .Item("precioUnitarioSinDescuento") = precioUnitarioSinDescuento
                    .Item("precioUnitario") = precioUnitario
                    .Item("precioEspecial") = precioEspecial
                    .Item("tipoSim") = tipoSim
                    .Item("codigoCuenta") = codigoCuenta
                    .Item("nombrePlan") = nombrePlan
                    .Item("tipoPlanVozDatos") = tipoPlanVozDatos
                    .Item("valorCargoBasicoPlanSinImpuesto") = valorCargoBasicoPlanSinImpuesto
                    .Item("paquete") = paquete
                    .Item("clausula") = clausula
                    .Item("valorClausula") = valorClausula
                    .Item("ventaEquipoContado") = ventaEquipoContado
                    .Item("ventaEquipoCuotas") = ventaEquipoCuotas
                    .Item("numeroCuotasVenta") = numeroCuotasVenta
                    .Item("nombreCanalVenta") = nombreCanalVenta
                    .Item("codigoCanalVenta") = codigoCanalVenta
                    .Item("solicitudServicioNumero") = solicitudServicioNumero
                    .Item("contratoCompraVentaEquipo") = contratoCompraVentaEquipo
                    .Item("nombreEjecutivoVenta") = nombreEjecutivoVenta
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
        Dim esvalido As Boolean = True
        Dim numinte As Integer
        Dim dato As String
        Dim haydatos As Boolean
        Dim expresion As New ConfigValues("EXP_REG_CONSULTA_MSISDN_AR")
        Dim oexpreg As New System.Text.RegularExpressions.Regex(expresion.ConfigKeyValue)
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
                        haydatos = HayDatosEnFila(.Item(index))
                        numCeldasActivas = .Item(index).AllocatedCells.Count
                        If numCeldasActivas = Me.EstructuraTabla.Columns.Count AndAlso haydatos Then
                            If String.IsNullOrEmpty(.Item(index).Cells(0).Value) Then
                                AdicionarError(index, "dato inválido", "el número del min no puede estar vacío.")
                            Else
                                If Not oexpreg.IsMatch(.Item(index).Cells(0).Value) Then _
                                    AdicionarError(index, "dato inválido", "la cadena de carateres ingresados para el min, no son válidas.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(1).Value) Then
                                AdicionarError(index, "dato inválido", "el nombre de la región no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(1).Value
                                If dato.Length > 2 Then _
                                    AdicionarError(index, "dato inválido", "la longuitud de la región no es válida.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(2).Value) Or .Item(index).Cells(2).Value Is Nothing Then
                                AdicionarError(index, "Dato inválido", "El precio unitario sin descuento no puede estar vacío.")
                            Else
                                If Not IsNumeric(.Item(index).Cells(2).Value) OrElse Not Integer.TryParse(.Item(index).Cells(2).Value, numinte) Then
                                    AdicionarError(index, "Dato inválido", "El precio unitario sin descuento debe ser numérico.")
                                Else
                                    If (.Item(index).Cells(2).Value) < 0 Then _
                                    AdicionarError(index, "Dato inválido", "El precio unitario sin descuento debe ser numérico y mayor que 0.")
                                End If

                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(3).Value) Or .Item(index).Cells(3).Value Is Nothing Then
                                AdicionarError(index, "dato inválido", "el precio unitario no puede estar vacío.")
                            Else
                                If Not IsNumeric(.Item(index).Cells(3).Value) OrElse Not Integer.TryParse(.Item(index).Cells(3).Value, numinte) Then
                                    AdicionarError(index, "dato inválido", "el precio unitario debe ser numérico.")
                                Else
                                    If (.Item(index).Cells(3).Value) < 0 Then
                                        AdicionarError(index, "dato inválido", "el precio unitario debe ser numérico y mayor que 0.")
                                        If String.IsNullOrEmpty(.Item(index).Cells(2).Value) Or .Item(index).Cells(2).Value Is Nothing Then
                                            AdicionarError(index, "Dato inválido", "El precio unitario sin descuento debe ser mayor que el precio unitario sin impuestos.")
                                        Else
                                            If (CDbl(.Item(index).Cells(2).Value)) > (.Item(index).Cells(3).Value) Then _
                                       AdicionarError(index, "Dato inválido", "El precio unitario sin descuento debe ser mayor que el precio unitario sin impuestos.")
                                        End If
                                    End If
                            End If
                        End If


                            If String.IsNullOrEmpty(.Item(index).Cells(4).Value) Or .Item(index).Cells(4).Value Is Nothing Then
                                AdicionarError(index, "dato inválido", "el campo de precio especial, no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(4).Value
                                If dato.Length > 2 Then _
                                    AdicionarError(index, "dato inválido", "la longuitud campo de precio especial no es válido.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(5).Value) Or .Item(index).Cells(5).Value Is Nothing Then
                                AdicionarError(index, "dato inválido", "el tipo de sim no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(5).Value
                                If dato.Length > 150 Then _
                                    AdicionarError(index, "dato inválido", "la longuitud del tipo de sim no es válida.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(6).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Cuenta', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(6).Value
                                If dato.Length > 50 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'Cuenta' no es válido.")
                                If Not oExpReg1.IsMatch(.Item(index).Cells(6).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para la Cuenta, no es válida.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(7).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Plan', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(7).Value
                                If dato.Length > 150 Then _
                                    AdicionarError(index, "Dato inválido", "La longuitud campo 'Plan' no es válido.")
                                If Not oExpReg2.IsMatch(.Item(index).Cells(7).Value) Then _
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el Plan, no es válida.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(8).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Tipo de Plan VOZ/DATOS', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(8).Value
                                If dato.ToString.ToLower.Trim <> "voz" And dato.ToString.ToLower.Trim <> "datos" Then
                                    AdicionarError(index, "Dato inválido", "La información contenida en el campo 'Tipo de Plan VOZ/DATOS' no es valida, debe ser Voz o Datos.")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(9).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Valor Cargo basico del plan sin impuesto', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(9).Value
                                If Not IsNumeric(dato) Then
                                    AdicionarError(index, "Dato inválido", "La cadena de carateres ingresados para el Valor Cargo basico del plan sin impuesto, no es válida, debe ser numerica.")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(11).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Clausula', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(11).Value
                                If dato.ToString.ToLower.Trim <> "si" And dato.ToString.ToLower.Trim <> "no" Then
                                    AdicionarError(index, "Dato inválido", "La información contenida en el campo 'Clausula' no es valida, debe ser Si ó No.")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(12).Value) And (.Item(index).Cells(11).Value).ToString.ToLower.Trim = "si" Then
                                AdicionarError(index, "Dato inválido", "El campo 'Valor Clausula', no puede estar vacío porque el campo 'clausula' es 'Si'.")
                            Else
                                Dim conClausula As Boolean = False
                                If (.Item(index).Cells(11).Value).ToString.ToLower.Trim = "si" Then
                                    conClausula = True
                                End If
                                dato = .Item(index).Cells(12).Value
                                If conClausula And Not IsNumeric(dato) Then
                                    AdicionarError(index, "Dato inválido", "La información contenida en el campo 'Valor Clausula' no es valida, debe ser Numerica")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(13).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Venta equipo de contado', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(13).Value
                                If dato.ToString.ToLower.Trim <> "si" And dato.ToString.ToLower.Trim <> "no" Then
                                    AdicionarError(index, "Dato inválido", "La información contenida en el campo 'Venta equipo de contado' no es valida, debe ser Si ó No.")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(14).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Venta equipo a cuotas', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(14).Value
                                If dato.ToString.ToLower.Trim <> "si" And dato.ToString.ToLower.Trim <> "no" Then
                                    AdicionarError(index, "Dato inválido", "La información contenida en el campo 'Venta equipo a cuotas' no es valida, debe ser Si ó No.")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(15).Value) And (.Item(index).Cells(14).Value).ToString.ToLower.Trim = "si" Then
                                AdicionarError(index, "Dato inválido", "El campo 'No. Cuotas venta', no puede estar vacío porque el campo 'Venta equipo a cuotas' es 'Si'.")
                            Else
                                Dim conCuotas As Boolean = False
                                If (.Item(index).Cells(14).Value).ToString.ToLower.Trim = "si" Then
                                    conCuotas = True
                                End If
                                dato = .Item(index).Cells(15).Value
                                If conCuotas And Not IsNumeric(dato) Then
                                    AdicionarError(index, "Dato inválido", "La información contenida en el campo 'No. Cuotas venta' no es valida, debe ser Numerica")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(16).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Nombre canal de venta', no puede estar vacío.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(17).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Codigo canal de venta', no puede estar vacío.")
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(18).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Solicitud de servicio No.', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(18).Value
                                If Not IsNumeric(dato) Then
                                    AdicionarError(index, "Dato inválido", "La información contenida en el campo 'Solicitud de servicio No.' no es valida, debe ser Numerica")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(19).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Contrato compra venta de equipo', no puede estar vacío.")
                            Else
                                dato = .Item(index).Cells(19).Value
                                If Not IsNumeric(dato) Then
                                    AdicionarError(index, "Dato inválido", "La información contenida en el campo 'Contrato compra venta de equipo' no es valida, debe ser Numerica")
                                End If
                            End If

                            If String.IsNullOrEmpty(.Item(index).Cells(20).Value) Then
                                AdicionarError(index, "Dato inválido", "El campo 'Nombre del ejecutivo de venta', no puede estar vacío.")
                            End If

                            If EstructuraTablaErrores.Rows.Count = 0 Then
                                AdicionarRegistro(.Item(index).Cells(0).Value, .Item(index).Cells(1).Value, .Item(index).Cells(2).Value, .Item(index).Cells(3).Value _
                                                  , .Item(index).Cells(4).Value, .Item(index).Cells(5).Value, .Item(index).Cells(6).Value, .Item(index).Cells(7).Value _
                                                  , .Item(index).Cells(8).Value, .Item(index).Cells(9).Value, .Item(index).Cells(10).Value, .Item(index).Cells(11).Value _
                                                  , .Item(index).Cells(12).Value, .Item(index).Cells(13).Value, .Item(index).Cells(14).Value, .Item(index).Cells(15).Value _
                                                  , .Item(index).Cells(16).Value, .Item(index).Cells(17).Value, .Item(index).Cells(18).Value, .Item(index).Cells(19).Value _
                                                  , .Item(index).Cells(20).Value)
                            End If
                        ElseIf haydatos Then
                            AdicionarError(index, "Dato inválido", "El Número de columnas de la .Item(index) es inválido. ")
                        Else
                            AdicionarError(index, "Dato inválido", "El número de línea se encuentra vacia, por favor verificar.")
                        End If
                    Else
                        AdicionarError(index, "Dato inválido", "El número de línea se encuentra vacia, por favor verificar. ")
                    End If
                End With
            Next
            esvalido = Not (EstructuraTablaErrores.Rows.Count > 0)
        Catch ex As Exception
            Throw ex
        End Try
        Return esvalido
    End Function

    Public Function ValidarInformacion(ByVal idBodega As Integer) As Boolean
        Dim esvalido As Boolean = True
        Try
            'AdicionarColumnas()
            Dim idusuario As Integer = CInt(HttpContext.Current.Session("usxp001"))

            If EstructuraTablaBase.Columns.Contains("idUsuario") Then EstructuraTablaBase.Columns.Remove("idUsuario")
            EstructuraTablaBase.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idusuario))

            Using dbmanager As New LMDataAccess
                With dbmanager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idusuario", SqlDbType.Int).Value = idusuario
                    .EjecutarNonQuery("liberardatosmsisdncorporativo", CommandType.StoredProcedure)

                    .InicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 600000
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaMsisdnCorporativo"
                        .ColumnMappings.Add("msisdn", "msisdn")
                        .ColumnMappings.Add("region", "region")
                        .ColumnMappings.Add("precioUnitarioSinDescuento", "precioUnitarioSinDescuento")
                        .ColumnMappings.Add("precioUnitario", "precioUnitario")
                        .ColumnMappings.Add("precioEspecial", "precioEspecial")
                        .ColumnMappings.Add("tipoSim", "tipoSim")
                        .ColumnMappings.Add("codigoCuenta", "codigoCuenta")
                        .ColumnMappings.Add("nombrePlan", "nombrePlan")
                        .ColumnMappings.Add("tipoPlanVozDatos", "tipoPlanVozDatos")
                        .ColumnMappings.Add("valorCargoBasicoPlanSinImpuesto", "valorCargoBasicoPlanSinImpuesto")
                        .ColumnMappings.Add("paquete", "paquete")
                        .ColumnMappings.Add("clausula", "clausula")
                        .ColumnMappings.Add("valorClausula", "valorClausula")
                        .ColumnMappings.Add("ventaEquipoContado", "ventaEquipoContado")
                        .ColumnMappings.Add("ventaEquipoCuotas", "ventaEquipoCuotas")
                        .ColumnMappings.Add("numeroCuotasVenta", "numeroCuotasVenta")
                        .ColumnMappings.Add("nombreCanalVenta", "nombreCanalVenta")
                        .ColumnMappings.Add("codigoCanalVenta", "codigoCanalVenta")
                        .ColumnMappings.Add("solicitudServicioNumero", "solicitudServicioNumero")
                        .ColumnMappings.Add("contratoCompraVentaEquipo", "contratoCompraVentaEquipo")
                        .ColumnMappings.Add("nombreEjecutivoVenta", "nombreEjecutivoVenta")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(EstructuraTablaBase)
                    End With

                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = idBodega
                    .SqlParametros.Add("@idusuario", SqlDbType.Int).Value = idusuario
                    _estructuraTablaErrores = .EjecutarDataTable("ValidarDatosMsisdnCorporativo", CommandType.StoredProcedure)

                    esvalido = (EstructuraTablaErrores.Rows.Count = 0)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return esvalido
    End Function

    Function RegistrarMsisdnTemporales() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbmanager As New LMDataAccess
        Try
            Dim idusuario As Integer = CInt(HttpContext.Current.Session("usxp001"))
            With dbmanager
                With .SqlParametros
                    .Clear()
                    .Add("@idusuario", SqlDbType.Int).Value = idusuario
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With

                .EjecutarNonQuery("RegistrarMsisdnTemporalesCorporativo", CommandType.StoredProcedure)

                If (Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor)) Then
                    resultado.Valor = .SqlParametros("@resultado").Value
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                Else
                    resultado.EstablecerMensajeYValor(400, "no se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If

            End With

        Catch ex As Exception
            If dbmanager IsNot Nothing Then dbmanager.Dispose()
            resultado.EstablecerMensajeYValor(500, "se generó un error al almacenar los mines: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

End Class
