Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.Drawing

Public Class SoporteReporteFacturacionILS

#Region "Atriburos"

    Private _idCentroCosto As Integer
    Private _idEventoFacturacion As Integer
    Private _idTipoProducto As Integer
    Private _anio As Integer
    Private _listaMeses As ArrayList
    Private _dsDatos As DataSet
    Private _resultado As ResultadoProceso
    Private _miExcel As New ExcelFile
    Private _rutaExcel As String

#End Region

#Region "Propiedades"

    Public Property IdCentroCosto() As Integer
        Get
            Return _idCentroCosto
        End Get
        Set(ByVal value As Integer)
            _idCentroCosto = value
        End Set
    End Property

    Public Property IdEventoFacturacion() As Integer
        Get
            Return _idEventoFacturacion
        End Get
        Set(ByVal value As Integer)
            _idEventoFacturacion = value
        End Set
    End Property

    Public Property IdTipoProducto() As Integer
        Get
            Return _idTipoProducto
        End Get
        Set(ByVal value As Integer)
            _idTipoProducto = value
        End Set
    End Property

    Public Property Anio() As Integer
        Get
            Return _anio
        End Get
        Set(ByVal value As Integer)
            _anio = value
        End Set
    End Property

    Public Property Listameses() As ArrayList
        Get
            Return _listaMeses
        End Get
        Set(ByVal value As ArrayList)
            _listaMeses = value
        End Set
    End Property

    Public ReadOnly Property DatosSoporte() As DataSet
        Get
            If _dsDatos Is Nothing Then ObtenerDatosSoportes()
            Return _dsDatos
        End Get
    End Property

    Public ReadOnly Property ArchivoExcel() As ExcelFile
        Get
            Return _miExcel
        End Get
    End Property

#End Region

#Region "Metodos"

    Private Sub ObtenerDatosSoportes()
        Dim db As New LMDataAccess
        Dim miListameses As String = Join(_listaMeses.ToArray(), ",")
        With db
            .agregarParametroSQL("@anio", _anio, SqlDbType.Int)
            .agregarParametroSQL("@listameses", miListameses)
            .agregarParametroSQL("@consolidado", 0)
            If _idTipoProducto > 0 Then .SqlParametros.Add("@idTipoProducto", SqlDbType.Int).Value = _idTipoProducto
            .TiempoEsperaComando = 900
        End With

        Select Case _idEventoFacturacion
            Case 1 'Fullfilment
                If _idTipoProducto = 0 Or _idTipoProducto = 1 Then _
                    Me.GenerarSoporteLogisticaTelefonosFullfiliados(db)
                If _idTipoProducto = 0 Or _idTipoProducto = 9 Then _
                    Me.GenerarSoporteLogisticaTokensFullfiliados(db)
            Case 2 'Recepcion
                If _idTipoProducto = 0 Or _idTipoProducto = 2 Then
                    Me.GenerarSoporteLogisticaIngresosSimCards(db)
                    Me.GenerarSoporteLogisticaIngresosSimCardsCrisma(db)
                End If
                If _idTipoProducto = 0 Or _idTipoProducto = 3 Then
                    Me.GenerarSoporteLogisticaRecepcionTarjetasPrepago(db)
                    Me.GenerarSoporteLogisticaDevolucionesTarjetasPrepago(db)
                End If
            Case 3 'Reprocesos
                If _idTipoProducto = 0 Or _idTipoProducto = 1 Then _
                    Me.GenerarSoporteLogisticaReprocesosHandsets(db) 'OK
                If _idTipoProducto = 0 Or _idTipoProducto = 2 Then _
                    Me.GenerarSoporteLogisticaReprocesosSimCards(db) 'OK
            Case 4 ' Despachos RIM
                Me.GenerarSoporteLogisticaDespachosRim(db) 'OK
            Case 6 'EventosEspeciales Logistica
                Me.GenerarSoporteProductosEspeciales(db)
            Case 7 'Devoluciones
                Me.GenerarSoporteInventarioSerialesCobroDevoluciones(db) 'OK
            Case 9 'Despachos
                If _idTipoProducto = 0 Or _idTipoProducto = 5 Then
                    Me.GenerarSoporteInventariosMerchandising(db) 'OK
                End If
                If _idTipoProducto = 0 Or _idTipoProducto = 6 Then
                    Me.GenerarSoporteInventariosProductosConsumo(db) 'OK
                End If
            Case 10 'Blackberry Servicio Tecnico
                If _idTipoProducto = 0 Or _idTipoProducto = 1 Then
                    Me.GenerarSoporteInventariosServicioTecnico(db) 'OK
                End If
            Case 11
                If _idTipoProducto = 0 Or _idTipoProducto = 1 Then 'Telefonos otros operadores
                    Me.GenerarSoporteInventariosIngresosCrisma(db) 'OK
                End If
            Case 13
                If _idTipoProducto = 0 Then 'CACs
                    Me.GenerarSoporteInventariosCACs(db) 'OK
                End If
            Case 14 'EventosEspeciales Inventarios
                Me.GenerarSoporteProductosEspeciales(db)
            Case 0
                If IdCentroCosto = 1 Then
                    Me.GenerarSoporteLogisticaTelefonosFullfiliados(db) 'OK
                    Me.GenerarSoporteLogisticaIngresosSimCards(db) 'OK 1/2
                    Me.GenerarSoporteLogisticaLecturaSimCards(db) 'OK
                    Me.GenerarSoporteLogisticaTokensFullfiliados(db) 'OK
                    Me.GenerarSoporteLogisticaIngresosSimCardsCrisma(db) 'OK
                    Me.GenerarSoporteProductosEspeciales(db)
                    Me.GenerarSoporteLogisticaRecepcionTarjetasPrepago(db) 'OK
                    Me.GenerarSoporteLogisticaDevolucionesTarjetasPrepago(db) 'OK
                    Me.GenerarSoporteLogisticaReprocesosSimCards(db) 'OK
                    Me.GenerarSoporteLogisticaDespachosRim(db) 'OK
                ElseIf IdCentroCosto = 2 Then
                    Me.GenerarSoporteInventarioSerialesCobroDevoluciones(db) 'OK
                    Me.GenerarSoporteInventariosDevolucionesConsolidado(db) 'OK
                    Me.GenerarSoporteInventariosIngresosCrisma(db) 'OK
                    Me.GenerarSoporteInventariosMerchandising(db) 'OK
                    Me.GenerarSoporteInventariosProductosConsumo(db) 'OK
                    Me.GenerarSoporteProductosEspeciales(db)
                End If
        End Select
    End Sub

    Private Function GenerarSoporteLogisticaTelefonosFullfiliados(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteLogisticaTelefonosFullfiliados", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvTable2 As New DataView(_dsDatos2.Tables("table2"))
            '------------------------------------------------------------------------------------------------------------
            'RESUMEN FULFILLMENT CONSOLIDADO TELEFONOS
            'Encabezado
            miWs = _miExcel.Worksheets.Add("Telefonos Fullfiliados")
            miWs.Cells("A1").Value = "RESUMEN FULFILLMENT CONSOLIDADO TELEFONOS"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable2.Table.Columns.Count - 1, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0

            miWs.Cells("A2").Value = "Suma de CANTIDAD"
            With miWs.Cells("A2")
                With .Style
                    .Font.Weight = ExcelFont.NormalWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With

            miWs.Cells("B2").Value = "INSTRUCCION"
            With miWs.Cells("B2")
                With .Style
                    .Font.Weight = ExcelFont.NormalWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable2.Table.Columns.Count - 1, Color.Gainsboro, miWs)

            filaInicial += 1
            colInicial = 0

            For columnas As Integer = 0 To dvTable2.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable2.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable2.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            colInicial = 0
            'Cuerpo del Reporte
            Dim x As Integer = filaInicial
            For i As Integer = 0 To dvTable2.Count - 1
                Dim registro As DataRowView = dvTable2(i)
                miWs.Cells(x, 0).Value = registro("REGION")
                miWs.Cells(x, 1).Value = registro("PRE")
                miWs.Cells(x, 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                miWs.Cells(x, 2).Value = registro("POST")
                miWs.Cells(x, 2).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                miWs.Cells(x, 3).Value = registro("Total general")
                miWs.Cells(x, 3).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                x += 1
            Next
            For columnas As Integer = 0 To dvTable2.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            'Total General
            Dim dvTable3 As New DataView(_dsDatos2.Tables("table3"))
            filaInicial += 1
            colInicial = 0
            miWs.Cells(x, 0).Value = "Total general"
            With miWs.Cells(x, 0)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Left
                End With
            End With
            Me.PintarTitulosCeldas(x, colInicial, x, 0, Color.Gainsboro, miWs, , , HorizontalAlignmentStyle.Left)

            For i As Integer = 0 To dvTable3.Count - 1
                Dim registro As DataRowView = dvTable3(i)
                miWs.Cells(x, 1).Value = registro("totPrep")
                miWs.Cells(x, 1).Style.Font.Weight = ExcelFont.BoldWeight
                miWs.Cells(x, 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                miWs.Cells(x, 2).Value = registro("totPost")
                miWs.Cells(x, 2).Style.Font.Weight = ExcelFont.BoldWeight
                miWs.Cells(x, 2).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                miWs.Cells(x, 3).Value = registro("totGen")
                miWs.Cells(x, 3).Style.Font.Weight = ExcelFont.BoldWeight
                miWs.Cells(x, 3).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
            Next

            For columnas As Integer = 0 To dvTable3.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next

            Me.PintarTitulosCeldas(filaInicial, 0, x, colInicial + dvTable2.Table.Columns.Count - 1, Color.White, miWs, True)
            '------------------------------------------------------------------------------------------------------------
            'DETALLE FULFILLMENT TELEFONOS
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            'Encabezado
            x += 3
            filaInicial = x
            Dim fila_detalle As Integer = x
            colInicial = 0
            miWs.Cells(x, 0).Value = "DETALLE FULFILLMENT TELEFONOS"
            With miWs.Cells(x, 0)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0

            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            colInicial = 0

            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            'Cuerpo del Reporte
            x = filaInicial
            For i As Integer = 0 To dvTable.Count - 1
                Dim registro As DataRowView = dvTable(i)
                Dim fecha As Date
                If Date.TryParse(registro("FECHA").ToString(), fecha) Then
                    miWs.Cells(x, 0).Value = fecha.ToShortDateString()
                End If
                miWs.Cells(x, 1).Value = registro("FACTURA LM")
                miWs.Cells(x, 2).Value = registro("FACTURA")
                miWs.Cells(x, 3).Value = registro("GUIA")
                miWs.Cells(x, 4).Value = registro("REFERENCIA")
                miWs.Cells(x, 5).Value = registro("REGION")
                miWs.Cells(x, 6).Value = registro("INSTRUCCION")
                miWs.Cells(x, 7).Value = registro("CANTIDAD")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            For columnas As Integer = 0 To dvTable2.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado

    End Function

    Private Function GenerarSoporteLogisticaIngresosSimCards(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteLogisticaIngresoSimCards", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            Dim dvTable1 As New DataView(_dsDatos2.Tables("table1"))
            '------------------------------------------------------------------------------------------------------------
            'RESUMEN FULFILLMENT SIMS CONSOLIDADO
            'Encabezado
            miWs = _miExcel.Worksheets.Add("Ingresos Sim Card")
            miWs.Cells("A1").Value = "RESUMEN FULFILLMENT SIMS CONSOLIDADO"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, 5, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0

            miWs.Cells("A2").Value = "Suma de CANTIDAD"
            With miWs.Cells("A2")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, 1, Color.Gainsboro, miWs, True, False)

            colInicial = 2
            miWs.Cells("C2").Value = "INSTRUCCION"
            With miWs.Cells("C2")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, 4, Color.Gainsboro, miWs, True, True)
            Me.PintarTitulosCeldas(filaInicial, 5, filaInicial, 5, Color.Gainsboro, miWs, True, True)
            filaInicial += 1
            colInicial = 0
            miWs.Cells(filaInicial, colInicial).Value = "REFERENCIA"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs, True, False)
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "REGION"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs, True, False)
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "KIT"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs, True, False)
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "POST"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs, True, False)
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "POST PROPIO"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs, True, False)
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "Total general"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs, True, False)

            colInicial = 0
            For columnas As Integer = 0 To 5
                miWs.Columns(columnas).AutoFit()
            Next
            filaInicial += 1


            'Cuerpo del Reporte
            Dim x As Integer = filaInicial
            x = filaInicial
            For i As Integer = 0 To dvTable1.Count - 1
                Dim registro As DataRowView = dvTable1(i)
                miWs.Cells(x, 0).Value = registro("referencia")
                miWs.Cells(x, 0).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                miWs.Cells(x, 1).Value = registro("region")
                miWs.Cells(x, 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                miWs.Cells(x, 2).Value = registro("KIT")
                miWs.Cells(x, 2).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                miWs.Cells(x, 3).Value = registro("POST")
                miWs.Cells(x, 3).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                miWs.Cells(x, 5).Value = registro("totalGeneral")
                miWs.Cells(x, 5).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable.Table.Columns.Count - 4, Color.White, miWs, True)
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 3
                miWs.Columns(columnas).AutoFit()
            Next

            '------------------------------------------------------------------------------------------------------------
            'DETALLE FULFILLMENT SIMS
            'Encabezado
            filaInicial = x + 2
            miWs.Cells(filaInicial, 0).Value = "DETALLE FULFILLMENT SIMS"
            With miWs.Cells(filaInicial, 0)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, 8, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            colInicial = 0

            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            'Cuerpo del Reporte
            x = filaInicial
            For i As Integer = 0 To dvTable.Count - 1
                Dim registro As DataRowView = dvTable(i)
                Dim fecha As Date
                If Date.TryParse(registro("FECHA").ToString(), fecha) Then
                    miWs.Cells(x, 0).Value = fecha.ToShortDateString()
                End If
                miWs.Cells(x, 1).Value = registro("FACTURA LM")
                miWs.Cells(x, 2).Value = registro("FACTURA")
                miWs.Cells(x, 3).Value = registro("GUIA")
                miWs.Cells(x, 4).Value = registro("PRODUCTO")
                miWs.Cells(x, 5).Value = registro("REGION")
                miWs.Cells(x, 6).Value = registro("INSTRUCCION")
                miWs.Cells(x, 7).Value = registro("CANTIDAD")
                miWs.Cells(x, 8).Value = registro("REFERENCIA")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado


    End Function

    Private Function GenerarSoporteLogisticaTokensFullfiliados(ByVal db As LMDataAccess)

        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteLogisticaTokensFullfiliados", CommandType.StoredProcedure)

        HerramientasFuncionales.CargarLicenciaGembox()
        Dim miWs As ExcelWorksheet
        Dim colInicial As Integer = 0
        Dim filaInicial As Integer = 0
        Dim colFinal As Integer = 0
        If _dsDatos2.Tables("table2").Rows(0).Item("cantidad") > 0 Then
            '------------------------------------------------------------------------------------------------------------
            'CANTIDADES Y VALORES POR DÍA
            'Encabezado
            miWs = _miExcel.Worksheets.Add("Tokens Fullfiliados")
            '------------------------------------------------------------------------------------------------------------
            'CANTIDADES Y VALORES POR DÍA
            'encabezado
            Dim x As Integer
            x = filaInicial
            filaInicial = x
            miWs.Cells(x, colInicial).Value = "CANTIDADES Y VALORES POR DÍA"
            With miWs.Cells(x, 9)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 11, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs, True, True)
            x += 1
            colInicial += 1
            miWs.Cells(x, 1).Value = "ORIENTE"
            With miWs.Cells(x, 1)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 2, Color.Gainsboro, miWs, True, True)
            colInicial += 3
            miWs.Cells(x, colInicial).Value = "OCCIDENTE"
            With miWs.Cells(x, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 2, Color.Gainsboro, miWs, True, True)
            colInicial += 3
            miWs.Cells(x, colInicial).Value = "NORTE"
            With miWs.Cells(x, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 2, Color.Gainsboro, miWs, True, True)
            colInicial += 3
            miWs.Cells(x, colInicial).Value = "TOTALES"
            With miWs.Cells(x, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 1, Color.Gainsboro, miWs, True, True)
            filaInicial += 1
            colInicial = 0
            miWs.Cells(filaInicial, colInicial).Value = "Fecha"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            miWs.Columns(0).AutoFit()
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "Unidades"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "V/Unidad"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "V/Total"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "Unidades"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "V/Unidad"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "V/Total"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "Unidades"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "V/Unidad"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "V/Total"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "Unidades"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            colInicial += 1
            miWs.Cells(filaInicial, colInicial).Value = "Pesos"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, 0, filaInicial, colInicial, Color.Gainsboro, miWs, True, False)
            For columnas As Integer = 0 To 11
                miWs.Columns(columnas).AutoFit()
            Next
            filaInicial += 1
            colInicial = 0
            x = filaInicial
            Dim dvtable As New DataView(_dsDatos2.Tables("table"))

            For i As Integer = 0 To dvtable.Count - 1
                Dim registro As DataRowView = dvtable(i)
                Dim fecha As Date
                If Date.TryParse(registro("FECHA").ToString(), fecha) Then
                    miWs.Cells(x, 0).Value = fecha.ToShortDateString()
                End If

                _dsDatos2.Tables("table1").DefaultView.RowFilter = ""
                _dsDatos2.Tables("table1").DefaultView.RowFilter = "fecha = '" & registro("fecha") & "'"

                Dim totUnidades As Integer = 0
                Dim totValor As Integer = 0

                For y As Integer = 0 To _dsDatos2.Tables("table1").DefaultView.Count - 1
                    Dim registro1 As DataRowView = _dsDatos2.Tables("table1").DefaultView.Item(y)
                    Select Case registro1("region").ToString.Trim
                        Case "OR"
                            miWs.Cells(x, 1).Value = registro1("unidades")
                            miWs.Cells(x, 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                            miWs.Cells(x, 3).Value = registro1("unidades") * registro1("vUnidad")
                            miWs.Cells(x, 3).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center

                            totUnidades = totUnidades + registro1("unidades")
                            totValor = totValor + miWs.Cells(x, 3).Value

                        Case "OC"
                            miWs.Cells(x, 4).Value = registro1("unidades")
                            miWs.Cells(x, 4).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                            miWs.Cells(x, 6).Value = registro1("unidades") * registro1("vUnidad")
                            miWs.Cells(x, 6).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center

                            totUnidades = totUnidades + registro1("unidades")
                            totValor = totValor + miWs.Cells(x, 6).Value

                        Case "NO"
                            miWs.Cells(x, 7).Value = registro1("unidades")
                            miWs.Cells(x, 7).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                            miWs.Cells(x, 9).Value = registro1("unidades") * registro1("vUnidad")
                            miWs.Cells(x, 9).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center

                            totUnidades = totUnidades + registro1("unidades")
                            totValor = totValor + miWs.Cells(x, 9).Value

                    End Select
                    miWs.Cells(x, 0).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    miWs.Cells(x, 2).Value = registro1("vUnidad")
                    miWs.Cells(x, 2).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    miWs.Cells(x, 5).Value = registro1("vUnidad")
                    miWs.Cells(x, 5).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    miWs.Cells(x, 8).Value = registro1("vUnidad")
                    miWs.Cells(x, 8).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    miWs.Cells(x, 10).Value = totUnidades
                    miWs.Cells(x, 10).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                    miWs.Cells(x, 11).Value = totValor
                    miWs.Cells(x, 11).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
                Next
                miWs.Rows(x).Style.NumberFormat = "#,##0"
                x += 1
            Next
            miWs.Columns(0).AutoFit()
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, 11, Color.White, miWs, True)
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteLogisticaIngresosSimCardsCrisma(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteLogisticaIngresosSimCardsCrisma", CommandType.StoredProcedure)
        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvTable1 As New DataView(_dsDatos2.Tables("table1"))
            '------------------------------------------------------------------------------------------------------------
            'RESUMEN INGRESOS SIM CARD OTRO OPERADOR
            'Encabezado
            miWs = _miExcel.Worksheets.Add("Ingresos Sim Card Otro Operador")
            miWs.Cells("A1").Value = "RESUMEN INGRESOS SIM CARD OTRO OPERADOR"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, 5, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0

            miWs.Cells("A2").Value = "Suma de CANTIDAD"
            With miWs.Cells("A2")
                With .Style
                    .Font.Weight = ExcelFont.NormalWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable1.Table.Columns.Count - 1, Color.Gainsboro, miWs)

            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable1.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable1.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            colInicial = 0

            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            'Cuerpo del Reporte
            Dim x As Integer = filaInicial
            For i As Integer = 0 To dvTable1.Count - 1
                Dim registro As DataRowView = dvTable1(i)
                miWs.Cells(x, 0).Value = registro("TIPO")
                miWs.Cells(x, 1).Value = registro("Material")
                miWs.Cells(x, 2).Value = registro("Texto breve de material")
                miWs.Cells(x, 3).Value = registro("Total")
                miWs.Cells(x, 4).Value = registro("DESCRIPCIÓN")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable1.Table.Columns.Count - 1, Color.White, miWs, True)
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            'Totales
            Dim dvTable2 As New DataView(_dsDatos2.Tables("table2"))
            filaInicial = x
            miWs.Cells(x, 0).Value = "Total Unidades"
            With miWs.Cells(x, 0)
                With .Style
                    .Font.Weight = ExcelFont.NormalWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 1, Color.Gainsboro, miWs)
            miWs.Cells(x, colInicial + 2).Value = dvTable2(0).Item("totalUnidades")
            x += 1
            filaInicial = x
            miWs.Cells(x, 0).Value = "Total Prepago"
            With miWs.Cells(x, 0)
                With .Style
                    .Font.Weight = ExcelFont.NormalWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 1, Color.Gainsboro, miWs)
            miWs.Cells(x, colInicial + 2).Value = dvTable2(0).Item("totalPrepago")
            x += 1
            filaInicial = x
            miWs.Cells(x, 0).Value = "Total Postpago"
            With miWs.Cells(x, 0)
                With .Style
                    .Font.Weight = ExcelFont.NormalWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 1, Color.Gainsboro, miWs)
            miWs.Cells(x, colInicial + 2).Value = dvTable2(0).Item("totalPostpago")
            x += 3
            filaInicial = x
            colInicial = 0
            '------------------------------------------------------------------------------------------------------------
            'DETALLE INGRESO SIM CARD OTRO OPERADOR
            'Encabezado
            miWs.Cells(x, 1).Value = "DETALLE INGRESO SIM CARD OTRO OPERADOR"
            With miWs.Cells(x, 1)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, 13, Color.Gainsboro, miWs, False, True)
            x += 1
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            filaInicial += 1
            colInicial = 0

            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            colInicial = 0
            x = filaInicial

            'Cuerpo del Reporte
            For i As Integer = 0 To dvTable.Count - 1
                Dim registro As DataRowView = dvTable(i)
                miWs.Cells(x, 0).Value = registro("TIPO")
                miWs.Cells(x, 1).Value = registro("Material")
                miWs.Cells(x, 2).Value = registro("Texto breve de material")
                miWs.Cells(x, 3).Value = registro("Hora")
                miWs.Cells(x, 4).Value = registro("Usuario")
                miWs.Cells(x, 5).Value = registro("CMv")
                miWs.Cells(x, 6).Value = registro("Doc.mat.")
                miWs.Cells(x, 7).Value = registro("Centro")
                miWs.Cells(x, 8).Value = registro("Almacen")
                miWs.Cells(x, 9).Value = registro("Texto cab.documento")
                miWs.Cells(x, 10).Value = registro("Referencia")
                miWs.Cells(x, 11).Value = registro("Entrega")
                Dim fecha As Date
                If Date.TryParse(registro("Registrado").ToString(), fecha) Then
                    miWs.Cells(x, 12).Value = fecha.ToShortDateString()
                End If
                miWs.Cells(x, 13).Value = registro("Cantidad")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFitAdvanced(1)
            Next
            '------------------------------------------------------------------------------------------------------------
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteLogisticaRecepcionTarjetasPrepago(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteLogisticaRecepcionTarjetasprepago", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            '------------------------------------------------------------------------------------------------------------
            'REPORTE DE RECEPCIÓN TARJETAS PREPAGO
            'Encabezado
            miWs = _miExcel.Worksheets.Add("Recepción Tarjetas Prepago ")
            miWs.Cells("A1").Value = "REPORTE DE RECEPCIÓN TARJETAS PREPAGO"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, 11, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            colInicial = 0
            '------------------------------------------------------------------------------------------------------------
            'Cuerpo del Reporte
            Dim x As Integer = filaInicial
            For i As Integer = 0 To dvTable.Count - 1
                Dim registro As DataRowView = dvTable(i)
                Dim fecha As Date
                If Date.TryParse(registro("Fecha").ToString(), fecha) Then
                    miWs.Cells(x, 0).Value = fecha.ToShortDateString()
                End If
                miWs.Cells(x, 1).Value = registro("Material")
                miWs.Cells(x, 2).Value = registro("Descripción")
                miWs.Cells(x, 3).Value = registro("Cantidad paca")
                miWs.Cells(x, 4).Value = registro("Oriente")
                miWs.Cells(x, 5).Value = registro("Occidente")
                miWs.Cells(x, 6).Value = registro("Norte")
                miWs.Cells(x, 7).Value = registro("Total")
                miWs.Cells(x, 8).Value = registro("Remisión del Proveedor")
                miWs.Cells(x, 9).Value = registro("Orden de Compra")
                miWs.Cells(x, 10).Value = registro("Documento de Cargue SAP")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            '------------------------------------------------------------------------------------------------------------
            filaInicial = x
            'Totales
            Dim dvTable1 As New DataView(_dsDatos2.Tables("table1"))
            miWs.Cells(x, 3).Value = "Totales"
            With miWs.Cells(x, 3)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            miWs.Cells(x, 4).Value = dvTable1(0).Item("totOriente")
            miWs.Cells(x, 5).Value = dvTable1(0).Item("totOccidente")
            miWs.Cells(x, 6).Value = dvTable1(0).Item("totNorte")
            miWs.Cells(x, 7).Value = dvTable1(0).Item("total")
            Me.PintarTitulosCeldas(filaInicial, 3, filaInicial, 7, Color.Gainsboro, miWs, True)
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteLogisticaReprocesosHandsets(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteLogisticaReprocesosHandsets", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))

            'Encabezado
            miWs = _miExcel.Worksheets.Add("Reprocesos Cobro Comcel")
            miWs.Cells("A1").Value = "SERIALES REPROCESOS NOVIEMBRE"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, 3, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
                miWs.Cells(filaInicial, columnas).Style.Font.Size = 12 * 14
                miWs.Cells(filaInicial, columnas).Style.HorizontalAlignment = HorizontalAlignmentStyle.Center
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs, True, False)
            filaInicial += 1
            colInicial = 0
            'Detalle
            Dim x As Integer = filaInicial
            For i As Integer = 0 To dvTable.Count - 1
                Dim registro As DataRowView = dvTable(i)
                miWs.Cells(x, 0).Value = registro("SERIAL")
                miWs.Cells(x, 1).Value = registro("PRODUCTO")
                Dim fecha As Date
                If Date.TryParse(registro("FECHA").ToString(), fecha) Then
                    miWs.Cells(x, 2).Value = fecha.ToShortDateString()
                End If
                miWs.Cells(x, 3).Value = registro("CONSECUTIVO")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, 3, Color.White, miWs, True)
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteLogisticaReprocesosSimCards(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteLogisticaReprocesosSimCards", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            '------------------------------------------------------------------------------------------------------------
            'REPORTE DE RECEPCIÓN TARJETAS PREPAGO
            'Encabezado
            miWs = _miExcel.Worksheets.Add("Reprocesos Sim Card")
            miWs.Cells("A1").Value = "SIM CARD - REPROCESOS"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, 6, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 2
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 2, Color.Gainsboro, miWs)
            filaInicial += 1
            colInicial = 0
            '------------------------------------------------------------------------------------------------------------
            'Cuerpo del Reporte
            Dim x As Integer = filaInicial
            For i As Integer = 0 To dvTable.Count - 1
                Dim registro As DataRowView = dvTable(i)
                miWs.Cells(x, 0).Value = registro("MAT ORIGEN")
                miWs.Cells(x, 1).Value = registro("DESCRIPCIÓN ORIGEN")
                miWs.Cells(x, 2).Value = registro("MAT DESTINO")
                miWs.Cells(x, 3).Value = registro("DESCRIPCIÓN DESTINO")
                Dim fecha As Date
                If Date.TryParse(registro("FECHA").ToString(), fecha) Then
                    miWs.Cells(x, 4).Value = fecha.ToShortDateString()
                End If
                miWs.Cells(x, 5).Value = registro("CONSECUTIVO")
                miWs.Cells(x, 6).Value = registro("CANTIDAD")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable.Table.Columns.Count - 2, Color.White, miWs, True)
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteLogisticaDespachosRim(ByVal db As LMDataAccess)
        db.LlenarDataSet(_dsDatos, "dtSoporteLogisticaDespachosRim", "SoporteLogisticaDespachosRim", CommandType.StoredProcedure)

        If _dsDatos.Tables("dtSoporteLogisticaDespachosRim").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvDatos As New DataView(_dsDatos.Tables("dtSoporteLogisticaDespachosRim"))

            'Encabezado 
            miWs = _miExcel.Worksheets.Add("Despachos Rim")
            miWs.Cells("A1").Value = "DESPACHOS DE EQUIPOS RIM"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 16 * 18
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvDatos.Table.Columns.Count - 1, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0

            For columnas As Integer = 0 To dvDatos.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvDatos.Table.Columns(columnas).ColumnName.ToString.Trim
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvDatos.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            Dim x = 2
            'Cuerpo del Reporte
            For i As Integer = 0 To dvDatos.Count - 1
                Dim registro As DataRowView = dvDatos(i)
                miWs.Cells(x, 0).Value = registro("No.")
                miWs.Cells(x, 1).Value = registro("Ref")
                miWs.Cells(x, 2).Value = registro("Cliente").ToString.Trim
                miWs.Cells(x, 3).Value = registro("IMEI(EQUIPO)")
                miWs.Cells(x, 4).Value = registro("PIN")
                Dim fecha As Date
                If Date.TryParse(registro("FECHA DE ACTIVACION").ToString(), fecha) Then
                    miWs.Cells(x, 5).Value = fecha.ToShortDateString()
                End If
                If Date.TryParse(registro("FECHA DE DESACTIVACION").ToString(), fecha) Then
                    miWs.Cells(x, 6).Value = fecha.ToShortDateString()
                End If
                If Date.TryParse(registro("FECHA DE NACIONALIZACION").ToString(), fecha) Then
                    miWs.Cells(x, 7).Value = fecha.ToShortDateString()
                End If
                If Date.TryParse(registro("TERMINACION GARANTIA FABRICANTE").ToString(), fecha) Then
                    miWs.Cells(x, 8).Value = fecha.ToShortDateString()
                End If
                miWs.Cells(x, 9).Value = registro("OBSERVACION").ToString.Trim
                miWs.Cells(x, 10).Value = registro("CODIGO DE FALLA")
                miWs.Cells(x, 11).Value = registro("DESCRIPCION DAÑO").ToString.Trim
                miWs.Cells(x, 12).Value = registro("IMEI(CAJA)")
                miWs.Cells(x, 13).Value = registro("REF PRESTAMO")
                miWs.Cells(x, 14).Value = registro("CAC").ToString.Trim
                miWs.Cells(x, 15).Value = registro("RESPONSABLE").ToString.Trim
                miWs.Cells(x, 16).Value = registro("TIPO DE CLIENTE").ToString.Trim
                miWs.Cells(x, 17).Value = registro("IDENTIFICACIÓN")
                miWs.Cells(x, 18).Value = registro("NUMERO DE MIN")
                miWs.Cells(x, 19).Value = registro("TELÉFONO FIJO")
                miWs.Cells(x, 20).Value = registro("CORREO ELECTRÓNICO").ToString.Trim
                miWs.Cells(x, 21).Value = registro("GUIA DE INGRESO")
                miWs.Cells(x, 22).Value = registro("No. FACTURA")
                miWs.Cells(x, 23).Value = registro("VALOR")
                miWs.Cells(x, 24).Value = registro("NIVEL REPARACION")
                x = x + 1
            Next

            Me.PintarTitulosCeldas(2, 0, dvDatos.Count + 1, colInicial + dvDatos.Table.Columns.Count - 1, Color.White, miWs, True)

            For columnas As Integer = 0 To dvDatos.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteProductosEspeciales(ByVal db As LMDataAccess)
        db.LlenarDataSet(_dsDatos, "dtSoporteProductoEspecial", "SoporteLogisticaProductoEspecial", CommandType.StoredProcedure)
        If _dsDatos.Tables("dtSoporteProductoEspecial").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvDatos As New DataView(_dsDatos.Tables("dtSoporteProductoEspecial"))

            'Encabezado 
            miWs = _miExcel.Worksheets.Add("Producto Especial")
            miWs.Cells("A1").Value = "PRODUCTOS ESPECIALES"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvDatos.Table.Columns.Count - 1, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0

            For columnas As Integer = 0 To dvDatos.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvDatos.Table.Columns(columnas).ColumnName.ToString.Trim
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvDatos.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            Dim x = 2

            'Cuerpo del Reporte
            For i As Integer = 0 To dvDatos.Count - 1
                Dim registro As DataRowView = dvDatos(i)
                miWs.Cells(x, 0).Value = registro("Nombre Producto").ToString.Trim
                miWs.Cells(x, 1).Value = registro("Descripción").ToString.Trim
                miWs.Cells(x, 2).Value = registro("Unidad").ToString.Trim
                miWs.Cells(x, 3).Value = registro("Cantidad")
                miWs.Cells(x, 4).Value = registro("Region")
                miWs.Cells(x, 5).Value = registro("Tarifa")
                Dim fecha As Date
                If Date.TryParse(registro("Fecha Registro").ToString(), fecha) Then
                    miWs.Cells(x, 6).Value = fecha.ToShortDateString()
                End If
                x = x + 1
            Next
            Me.PintarTitulosCeldas(2, 0, dvDatos.Count + 1, colInicial + dvDatos.Table.Columns.Count - 1, Color.White, miWs, True)
            For columnas As Integer = 0 To dvDatos.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteLogisticaDevolucionesTarjetasPrepago(ByVal db As LMDataAccess)

        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteLogisticaDevolucionesTarjetasPrepago", CommandType.StoredProcedure)
        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))

            'Encabezado 
            miWs = _miExcel.Worksheets.Add("Devoluciones Tarjetas Prepago")
            miWs.Cells("A1").Value = "REPORTE DE RECEPCIÓN DEVOLUCIONES TARJETAS PREPAGO"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0

            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            'Cuerpo del Reporte
            filaInicial += 1
            colInicial = 0
            Dim x As Integer = filaInicial
            For i As Integer = 0 To dvTable.Count - 1
                Dim registro As DataRowView = dvTable(i)
                Dim fecha As Date
                If Date.TryParse(registro("Fecha").ToString(), fecha) Then
                    miWs.Cells(x, 0).Value = fecha.ToShortDateString()
                End If
                miWs.Cells(x, 1).Value = registro("Material")
                miWs.Cells(x, 2).Value = registro("Descripción")
                miWs.Cells(x, 3).Value = registro("Cantidad paca")
                miWs.Cells(x, 4).Value = registro("Oriente")
                miWs.Cells(x, 5).Value = registro("Total")
                miWs.Cells(x, 6).Value = registro("Entrega Sap")
                miWs.Cells(x, 7).Value = registro("Documento de cargue SAP")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            'Totales
            Dim dvTable1 As New DataView(_dsDatos2.Tables("table1"))
            filaInicial = x
            colInicial = 0
            miWs.Cells(x, 0).Value = "Totales"
            With miWs.Cells(x, 0)
                With .Style
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 3, Color.Gainsboro, miWs, False, True)

            miWs.Cells(x, 4).Value = dvTable1(0).Item("cntRegion")
            miWs.Cells(x, 5).Value = dvTable1(0).Item("cntRegion")
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteLogisticaLecturaSimCards(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteLogisticaLecturaSimCards", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0
            Dim dvTable1 As New DataView(_dsDatos2.Tables("table1"))

            'Encabezado 
            miWs = _miExcel.Worksheets.Add("Lectura Sim Card  3FF")
            miWs.Cells("A1").Value = "RESUMEN FULFILLMENT SIMS CONSOLIDADO"
            With miWs.Cells("A1")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable1.Table.Columns.Count - 1, Color.Gainsboro, miWs, False, True)
            filaInicial += 1
            colInicial = 0
            miWs.Cells("A2").Value = "Suma de CANTIDAD"
            With miWs.Cells("A2")
                With .Style
                    .HorizontalAlignment = HorizontalAlignmentStyle.Left
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs, True, False, HorizontalAlignmentStyle.Left)
            miWs.Cells("B2").Value = "INSTRUCCION"
            With miWs.Cells("B2")
                With .Style
                    .HorizontalAlignment = HorizontalAlignmentStyle.Left
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial + 1, filaInicial, colInicial + 2, Color.Gainsboro, miWs, True, True, HorizontalAlignmentStyle.Left)
            filaInicial += 1
            colInicial = 0
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable1.Table.Columns(columnas).ColumnName.ToString.Trim
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable1.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            'Cuerpo del Reporte
            filaInicial += 1
            colInicial = 0
            Dim x As Integer = filaInicial
            For i As Integer = 0 To dvTable1.Count - 1
                Dim registro As DataRowView = dvTable1(i)
                miWs.Cells(x, 0).Value = registro("REGION")
                miWs.Cells(x, 1).Value = registro("LECTURA")
                miWs.Cells(x, 2).Value = registro("Total general")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable1.Table.Columns.Count - 1, Color.White, miWs, True)
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial, Color.Gainsboro, miWs, True)

            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            'Totales
            Dim dvTable2 As New DataView(_dsDatos2.Tables("table2"))
            filaInicial = x
            colInicial = 0
            miWs.Cells(x, 0).Value = "Total general"
            With miWs.Cells(x, 0)
                With .Style
                    .HorizontalAlignment = HorizontalAlignmentStyle.Left
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs, True, False)
            miWs.Cells(x, 1).Value = dvTable2(0).Item("lectura")
            miWs.Cells(x, 2).Value = dvTable2(0).Item("totalGeneral")
            Me.PintarTitulosCeldas(filaInicial, 1, filaInicial, 2, Color.White, miWs, True, False)
            x += 2
            filaInicial = x
            colInicial = 0
            '---------------------------------------------------------------------------------------------
            'DETALLE FULFILLMENT SIMS
            'Encabezado
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            miWs.Cells(x, 0).Value = "DETALLE FULFILLMENT SIMS"
            With miWs.Cells(x, 0)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Left
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs, False, True)
            x += 1
            filaInicial = x
            colInicial = 0
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next

            'Cuerpo del Reporte
            x += 1
            filaInicial = x
            colInicial = 0
            For i As Integer = 0 To dvTable.Count - 1
                Dim registro As DataRowView = dvTable(i)
                Dim fecha As Date
                If Date.TryParse(registro("FECHA").ToString(), fecha) Then
                    miWs.Cells(x, 0).Value = fecha.ToShortDateString()
                End If
                miWs.Cells(x, 1).Value = registro("FACTURA LM")
                miWs.Cells(x, 2).Value = registro("FACTURA")
                miWs.Cells(x, 3).Value = registro("GUIA")
                miWs.Cells(x, 4).Value = registro("REFERENCIA")
                miWs.Cells(x, 5).Value = registro("REGION")
                miWs.Cells(x, 6).Value = registro("INSTRUCCION")
                miWs.Cells(x, 7).Value = registro("CANTIDAD")
                x += 1
            Next
            Me.PintarTitulosCeldas(filaInicial, 0, x - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)

            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteInventarioSerialesCobroDevoluciones(ByVal db As LMDataAccess)
        db.LlenarDataSet(_dsDatos, "dtSoporteInventarioSerialesCobroDevoluciones", "SoporteInventarioSerialesCobroDevoluciones", CommandType.StoredProcedure)

        If _dsDatos.Tables("dtSoporteInventarioSerialesCobroDevoluciones").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaActual As Integer = 0
            Dim dvDatos As New DataView(_dsDatos.Tables("dtSoporteInventarioSerialesCobroDevoluciones"))

            'Encabezado 
            miWs = _miExcel.Worksheets.Add("Seriales Cobro Devoluciones")
            For columnas As Integer = 0 To dvDatos.Table.Columns.Count - 1
                miWs.Cells(filaActual, columnas).Value = dvDatos.Table.Columns(columnas).ColumnName.ToString.Trim
            Next
            Me.PintarTitulosCeldas(filaActual, colInicial, filaActual, colInicial + dvDatos.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaActual += 1

            'Cuerpo del Reporte
            For Each registro As DataRowView In dvDatos
                miWs.Cells(filaActual, 0).Value = registro("SERIAL")
                miWs.Cells(filaActual, 1).Value = registro("MATERIAL")
                miWs.Cells(filaActual, 2).Value = registro("DESCRIPCION").ToString.Trim
                miWs.Cells(filaActual, 3).Value = registro("DOCUMENTO")
                miWs.Cells(filaActual, 4).Value = registro("ENTREGA")
                miWs.Cells(filaActual, 5).Value = registro("PEDIDO").ToString.Trim
                miWs.Cells(filaActual, 6).Value = registro("CLIENTE")
                miWs.Cells(filaActual, 7).Value = registro("TIPO DEVOLUCIÓN").ToString.Trim
                filaActual += 1
            Next
            Me.PintarTitulosCeldas(1, 0, dvDatos.Count - 1, colInicial + dvDatos.Table.Columns.Count - 1, Color.White, miWs, True)
            For columnas As Integer = 0 To dvDatos.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFitAdvanced(1)
            Next
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteInventariosDevolucionesConsolidado(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteInventariosDevolucionesConsolidado", CommandType.StoredProcedure)
        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 0
            Dim colFinal As Integer = 0

            '---------------------------------------------------------------------------------------------------------------
            Dim dvTable2 As New DataView(_dsDatos2.Tables("table2"))
            'Encabezado 
            miWs = _miExcel.Worksheets.Add("Consolidado Devoluciones")
            miWs.Cells("A2").Value = "SOPORTE DEVOLUCIONES ZONA FRANCA"
            With miWs.Cells("A2")
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 13 * 15
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            filaInicial += 1
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable2.Table.Columns.Count + 1, Color.White, miWs, False, True)
            filaInicial += 2
            colInicial = 0
            For columnas As Integer = 0 To dvTable2.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable2.Table.Columns(columnas).ColumnName.ToString.Trim
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable2.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            'Cuerpo Reporte
            miWs.Cells(filaInicial, 0).Value = dvTable2(0).Item("ENTREGA")
            miWs.Cells(filaInicial, 1).Value = dvTable2(0).Item("SERIALES")
            filaInicial += 2
            '---------------------------------------------------------------------------------------------------------------
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            'Encabezado 
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            'Cuerpo Reporte
            Dim i As Integer = filaInicial
            For x As Integer = 0 To dvTable.Table.Rows.Count - 1
                Dim registro As DataRowView = dvTable(x)
                miWs.Cells(i, 0).Value = registro("ENTREGA")
                miWs.Cells(i, 1).Value = registro("Total")
                i += 1
            Next
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, i, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            filaInicial = i

            miWs.Cells(filaInicial, 0).Value = "Total General"
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial, Color.Gainsboro, miWs)

            Dim dvTable1 As New DataView(_dsDatos2.Tables("table1"))
            miWs.Cells(i, 1).Value = dvTable1(0).Item("totGeneral")
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            '---------------------------------------------------------------------------------------------------------------
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteInventariosIngresosCrisma(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteInventariosIngresosCrisma", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 1
            miWs = _miExcel.Worksheets.Add("Ingresos Otro Operador")
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            miWs.Cells(filaInicial, colInicial).Value = "DETALLE INGRESO TELEFONOS OTRO OPERADOR"
            With miWs.Cells(filaInicial, colInicial)
                With .Style
                    .Font.Weight = ExcelFont.BoldWeight
                    .Font.Size = 14 * 16
                    .HorizontalAlignment = HorizontalAlignmentStyle.Center
                End With
            End With
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs, True, True)
            filaInicial += 1

            'Encabezado 
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            'Cuerpo Reporte
            Dim i As Integer = filaInicial
            For x As Integer = 0 To dvTable.Table.Rows.Count - 1
                Dim registro As DataRowView = dvTable(x)
                miWs.Cells(i, 0).Value = registro("TIPO")
                miWs.Cells(i, 1).Value = registro("Material")
                miWs.Cells(i, 2).Value = registro("Texto breve de material")
                miWs.Cells(i, 3).Value = registro("Hora")
                miWs.Cells(i, 4).Value = registro("Usuario")
                miWs.Cells(i, 5).Value = registro("CMv")
                miWs.Cells(i, 6).Value = registro("Doc.mat.")
                miWs.Cells(i, 7).Value = registro("Ce.")
                miWs.Cells(i, 8).Value = registro("Alm.")
                miWs.Cells(i, 9).Value = registro("Texto cab.documento")
                miWs.Cells(i, 10).Value = registro("Referencia")
                miWs.Cells(i, 11).Value = registro("Entrega")
                miWs.Cells(i, 12).Value = registro("Registrado")
                miWs.Cells(i, 13).Value = registro("Cantidad")
                i += 1
            Next
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, i, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteInventariosMerchandising(ByVal db As LMDataAccess)

        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteInventariosMerchandising", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 1
            '-----------------------------------------------------------------------------------------------------------
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            miWs = _miExcel.Worksheets.Add("Merchandising")
            'Encabezado 
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            'Cuerpo Reporte
            Dim i As Integer = filaInicial
            For x As Integer = 0 To dvTable.Table.Rows.Count - 1
                Dim registro As DataRowView = dvTable(x)
                miWs.Cells(i, 0).Value = registro("Documento")
                miWs.Cells(i, 1).Value = registro("Pedido")
                miWs.Cells(i, 2).Value = registro("Cod.Cli")
                miWs.Cells(i, 3).Value = registro("Descripcion")
                miWs.Cells(i, 4).Value = registro("Fec.Ord.")
                miWs.Cells(i, 5).Value = registro("Hrs.Ord.")
                miWs.Cells(i, 6).Value = registro("Fe.SM real")
                miWs.Cells(i, 7).Value = registro("Hrs.Ent.")
                miWs.Cells(i, 8).Value = registro("Fec.Cont")
                miWs.Cells(i, 9).Value = registro("Hrs.Cont.")
                miWs.Cells(i, 10).Value = registro("Usuario")
                miWs.Cells(i, 11).Value = registro("Num.Guia")
                miWs.Cells(i, 12).Value = registro("Material")
                miWs.Cells(i, 13).Value = registro("Denominación")
                miWs.Cells(i, 14).Value = registro("Cantidad ent")
                miWs.Cells(i, 15).Value = registro("Cod. Bodega")
                miWs.Cells(i, 16).Value = registro("Cod.Lin.")
                miWs.Cells(i, 17).Value = registro("Notas")
                miWs.Cells(i, 18).Value = registro("Carta de porte")
                miWs.Cells(i, 19).Value = registro("Ident.medio.transp")
                i += 1
            Next
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, i - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            filaInicial = i
            '-----------------------------------------------------------------------------------------------------------
            'Resumen
            Dim dvTable1 As New DataView(_dsDatos2.Tables("table1"))
            colInicial = 21
            filaInicial = 4
            'Encabezado 
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Cells(filaInicial, colInicial).Value = dvTable1.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, colInicial).Style.Font.Weight = ExcelFont.BoldWeight
                colInicial += 1
            Next
            colInicial = 21
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable1.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            'Cuerpo Reporte
            Dim r As Integer = filaInicial
            For x As Integer = 0 To dvTable1.Table.Rows.Count - 1
                Dim registro1 As DataRowView = dvTable1(x)
                miWs.Cells(r, 21).Value = registro1("Documento")
                Dim c22, c23, c24, c25 As Double
                If registro1("1007-1007") Is DBNull.Value Then c22 = 0 Else c22 = registro1("1007-1007")
                miWs.Cells(r, 22).Value = c22
                If registro1("1002-1003") Is DBNull.Value Then c23 = 0 Else c23 = registro1("1002-1003")
                miWs.Cells(r, 23).Value = c23
                If registro1("4105-4105") Is DBNull.Value Then c24 = 0 Else c24 = registro1("4105-4105")
                miWs.Cells(r, 24).Value = c24
                If registro1("7005-7005") Is DBNull.Value Then c25 = 0 Else c25 = registro1("7005-7005")
                miWs.Cells(r, 25).Value = c25
                miWs.Cells(r, 26).Value = c22 + c23 + c24 + c25
                r += 1
            Next
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, r, colInicial + dvTable1.Table.Columns.Count - 1, Color.White, miWs, True)
            filaInicial = r
            colInicial = 21
            '-----------------------------------------------------------------------------------------------------------
            'Total General
            miWs.Cells(filaInicial, colInicial).Value = "Total General"
            Dim dvTable2 As New DataView(_dsDatos2.Tables("table2"))
            Dim registro2 As DataRowView = dvTable2(0)
            Dim r22, r23, r24, r25 As Double
            If registro2("1007") Is DBNull.Value Then r22 = 0 Else r22 = registro2("1007")
            miWs.Cells(r, 22).Value = r22
            If registro2("1002") Is DBNull.Value Then r23 = 0 Else r23 = registro2("1002")
            miWs.Cells(r, 23).Value = r23
            If registro2("4105") Is DBNull.Value Then r24 = 0 Else r24 = registro2("4105")
            miWs.Cells(r, 24).Value = r24
            If registro2("7005") Is DBNull.Value Then r25 = 0 Else r25 = registro2("7005")
            miWs.Cells(r, 25).Value = r25
            miWs.Cells(r, 26).Value = r22 + r23 + r24 + r25
            r += 1

            miWs.Columns(21).AutoFit()
            miWs.Columns(26).AutoFit()

        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteInventariosProductosConsumo(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteInventariosProductosConsumo", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 1
            '-----------------------------------------------------------------------------------------------------------
            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            miWs = _miExcel.Worksheets.Add("Productos Consumo")
            'Encabezado 
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            'Cuerpo Reporte
            Dim i As Integer = filaInicial
            For x As Integer = 0 To dvTable.Table.Rows.Count - 1
                Dim registro As DataRowView = dvTable(x)
                miWs.Cells(i, 0).Value = registro("Documento")
                miWs.Cells(i, 1).Value = registro("Pedido")
                miWs.Cells(i, 2).Value = registro("Cod.Cli")
                miWs.Cells(i, 3).Value = registro("Descripcion")
                miWs.Cells(i, 4).Value = registro("Fec.Ord.")
                miWs.Cells(i, 5).Value = registro("Hrs.Ord.")
                miWs.Cells(i, 6).Value = registro("Fec. Ent")
                miWs.Cells(i, 7).Value = registro("Hrs.Ent.")
                miWs.Cells(i, 8).Value = registro("Fec.Cont")
                miWs.Cells(i, 9).Value = registro("Hrs.Cont.")
                miWs.Cells(i, 10).Value = registro("Usuario")
                miWs.Cells(i, 11).Value = registro("Numero de Guia")
                miWs.Cells(i, 12).Value = registro("Material")
                miWs.Cells(i, 13).Value = registro("Desc.Mat.")
                miWs.Cells(i, 14).Value = registro("Cantidad")
                miWs.Cells(i, 15).Value = registro("Cod. Bodega")
                miWs.Cells(i, 16).Value = registro("Cod.Lin.Prod.")
                miWs.Cells(i, 17).Value = registro("Notas")
                miWs.Cells(i, 18).Value = registro("Peso")
                miWs.Cells(i, 19).Value = registro("Recibe")
                miWs.Cells(i, 20).Value = registro("Desc. Bodega")
                miWs.Cells(i, 21).Value = registro("Desc. Lin. Prod")
                miWs.Cells(i, 22).Value = registro("Centro Costo")
                miWs.Cells(i, 23).Value = registro("Centro Costo 2")
                i += 1
            Next
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, i - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            filaInicial = i
            '-----------------------------------------------------------------------------------------------------------
            'Resumen
            Dim dvTable1 As New DataView(_dsDatos2.Tables("table1"))
            colInicial = 25
            filaInicial = 4
            'Encabezado 
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Cells(filaInicial, colInicial).Value = dvTable1.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, colInicial).Style.Font.Weight = ExcelFont.BoldWeight
                colInicial += 1
            Next
            colInicial = 25
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable1.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            'Cuerpo Reporte
            Dim r As Integer = filaInicial
            For x As Integer = 0 To dvTable1.Table.Rows.Count - 1
                Dim registro1 As DataRowView = dvTable1(x)
                miWs.Cells(r, 25).Value = registro1("Documento")
                Dim c26, c27, c28, c29 As Double
                If registro1("1007-1007") Is DBNull.Value Then c26 = 0 Else c26 = registro1("1007-1007")
                miWs.Cells(r, 26).Value = c26
                If registro1("1002-1003") Is DBNull.Value Then c27 = 0 Else c27 = registro1("1002-1003")
                miWs.Cells(r, 27).Value = c27
                If registro1("4105-4105") Is DBNull.Value Then c28 = 0 Else c28 = registro1("4105-4105")
                miWs.Cells(r, 28).Value = c28
                If registro1("7005-7005") Is DBNull.Value Then c29 = 0 Else c29 = registro1("7005-7005")
                miWs.Cells(r, 29).Value = c29
                miWs.Cells(r, 30).Value = c26 + c27 + c28 + c29
                r += 1
            Next
            For columnas As Integer = 0 To dvTable1.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, r, colInicial + dvTable1.Table.Columns.Count - 1, Color.White, miWs, True)
            filaInicial = r
            colInicial = 25
            '-----------------------------------------------------------------------------------------------------------
            'Total General
            miWs.Cells(filaInicial, colInicial).Value = "Total General"
            Dim dvTable2 As New DataView(_dsDatos2.Tables("table2"))
            Dim registro2 As DataRowView = dvTable2(0)
            Dim r26, r27, r28, r29 As Double
            If registro2("1007") Is DBNull.Value Then r26 = 0 Else r26 = registro2("1007")
            miWs.Cells(r, 26).Value = r26
            If registro2("1002") Is DBNull.Value Then r27 = 0 Else r27 = registro2("1002")
            miWs.Cells(r, 27).Value = r27
            If registro2("4105") Is DBNull.Value Then r28 = 0 Else r28 = registro2("4105")
            miWs.Cells(r, 28).Value = r28
            If registro2("7005") Is DBNull.Value Then r29 = 0 Else r29 = registro2("7005")
            miWs.Cells(r, 29).Value = r29
            miWs.Cells(r, 30).Value = r26 + r27 + r28 + r29
            r += 1

            miWs.Columns(25).AutoFit()
            miWs.Columns(30).AutoFit()

        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteInventariosServicioTecnico(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteInventariosServicioTecnico", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 1

            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            miWs = _miExcel.Worksheets.Add("Servicio Tecnico BlackBerry")
            '---------------------------------------------------------------------------------------------------------------
            'Encabezado 
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            '---------------------------------------------------------------------------------------------------------------
            'Cuerpo del Reporte
            Dim i As Integer = filaInicial
            For x As Integer = 0 To dvTable.Table.Rows.Count - 1
                Dim registro As DataRowView = dvTable(x)
                miWs.Cells(i, 0).Value = registro("GUIA").ToString.Trim
                miWs.Cells(i, 1).Value = registro("FECHA").ToString.Trim
                miWs.Cells(i, 2).Value = registro("ORIGEN").ToString.Trim
                miWs.Cells(i, 3).Value = registro("NOMBRE ORIGEN").ToString.Trim
                miWs.Cells(i, 4).Value = registro("DESTINO").ToString.Trim
                miWs.Cells(i, 5).Value = registro("NOMBRE DESTINATARIO").ToString.Trim
                miWs.Cells(i, 6).Value = registro("UNIDADES INTERNAS").ToString.Trim
                i += 1
            Next
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, i - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            '---------------------------------------------------------------------------------------------------------------
        End If
        _resultado = New ResultadoProceso
        _resultado.Valor = 0
        _resultado.Mensaje = "Se ha generado el archivo correctamente"
        Return _resultado
    End Function

    Private Function GenerarSoporteInventariosCACs(ByVal db As LMDataAccess)
        Dim _dsDatos2 As DataSet = db.EjecutarDataSet("SoporteInventariosCACs", CommandType.StoredProcedure)

        If _dsDatos2.Tables("table").Rows.Count > 0 Then
            HerramientasFuncionales.CargarLicenciaGembox()
            Dim miWs As ExcelWorksheet
            Dim colInicial As Integer = 0
            Dim filaInicial As Integer = 1

            Dim dvTable As New DataView(_dsDatos2.Tables("table"))
            miWs = _miExcel.Worksheets.Add("CAC")
            '---------------------------------------------------------------------------------------------------------------
            'Encabezado 
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Cells(filaInicial, columnas).Value = dvTable.Table.Columns(columnas).ColumnName.ToString.Trim
                miWs.Cells(filaInicial, columnas).Style.Font.Weight = ExcelFont.BoldWeight
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + dvTable.Table.Columns.Count - 1, Color.Gainsboro, miWs)
            filaInicial += 1
            '---------------------------------------------------------------------------------------------------------------
            'Cuerpo del Reporte
            Dim i As Integer = filaInicial
            Dim total As Double
            For x As Integer = 0 To dvTable.Table.Rows.Count - 1
                Dim registro As DataRowView = dvTable(x)
                miWs.Cells(i, 0).Value = registro("CAC").ToString.Trim
                miWs.Cells(i, 1).Value = Format(registro("Vlr Inventario Promedio"), "$ #,##0.00")
                miWs.Cells(i, 1).Style.HorizontalAlignment = HorizontalAlignmentStyle.Right
                miWs.Cells(i, 2).Value = Format(registro("Tarifa Plena"), "$ #,##0.00")
                miWs.Cells(i, 2).Style.HorizontalAlignment = HorizontalAlignmentStyle.Right
                total = total + registro("Tarifa Plena")
                i += 1
            Next
            For columnas As Integer = 0 To dvTable.Table.Columns.Count - 1
                miWs.Columns(columnas).AutoFit()
            Next
            Me.PintarTitulosCeldas(filaInicial, colInicial, i - 1, colInicial + dvTable.Table.Columns.Count - 1, Color.White, miWs, True)
            filaInicial = i
            colInicial = 0
            miWs.Cells(i, 0).Value = "Total General"
            Me.PintarTitulosCeldas(filaInicial, colInicial, filaInicial, colInicial + 1, Color.Gainsboro, miWs, False, True, HorizontalAlignmentStyle.Center)
            miWs.Cells(i, 2).Value = Format(total, "$ #,##0.00")
            Me.PintarTitulosCeldas(filaInicial, 2, filaInicial, 2, Color.Gainsboro, miWs, False, True, HorizontalAlignmentStyle.Right)
            '---------------------------------------------------------------------------------------------------------------
        End If
    End Function

#End Region

    Public Function ObtenerHojasExcel() As ExcelFile
        If _dsDatos Is Nothing Then Me.ObtenerDatosSoportes()
        Return _miExcel
    End Function

    Private Sub PintarTitulosCeldas(ByVal filaInicial As Integer, ByVal columnaInicial As Integer, ByVal filaFinal As Integer, ByVal columnaFinal As Integer, ByVal colorFondo As Color, ByVal miWS As ExcelWorksheet, Optional ByVal cuadricula As Boolean = False, Optional ByVal merge As Boolean = False, Optional ByVal alineacion As HorizontalAlignmentStyle = HorizontalAlignmentStyle.Center)
        Dim cr As CellRange = miWS.Cells.GetSubrangeAbsolute(filaInicial, columnaInicial, filaFinal, columnaFinal)
        cr.Merged = merge
        For Each cel As ExcelCell In cr
            With cel.Style
                If cuadricula = False Then
                    .Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Medium)
                    .Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Medium)
                    .Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Medium)
                    .Borders.SetBorders(MultipleBorders.Bottom, Color.Black, LineStyle.Medium)
                    .FillPattern.SetPattern(FillPatternStyle.Solid, colorFondo, colorFondo)
                    '.Font.Weight = ExcelFont.BoldWeight
                    .HorizontalAlignment = alineacion
                Else
                    .Borders.SetBorders(MultipleBorders.Top, Color.Black, LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Right, Color.Black, LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Left, Color.Black, LineStyle.Thin)
                    .Borders.SetBorders(MultipleBorders.Bottom, Color.Black, LineStyle.Thin)
                    .FillPattern.SetPattern(FillPatternStyle.Solid, colorFondo, colorFondo)
                End If

            End With
        Next
    End Sub

    Public Sub New()
        _miExcel = New ExcelFile
    End Sub
End Class
