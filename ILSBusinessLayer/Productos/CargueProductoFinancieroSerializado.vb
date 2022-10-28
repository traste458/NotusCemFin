Imports System.Web
Imports GemBox.Spreadsheet
Imports LMDataAccessLayer
Imports ILSBusinessLayer.Comunes

Public Class CargueProductoFinancieroSerializado

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
                    .Add(New DataColumn("codigoProducto", GetType(String)))
                    .Add(New DataColumn("rangoInicial", GetType(Long)))
                    .Add(New DataColumn("rangoFinal", GetType(Long)))
                    .Add(New DataColumn("almacen", GetType(String)))
                    .Add(New DataColumn("centro", GetType(String)))
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
                    .Add(New DataColumn("codigoProducto", GetType(String)))
                    .Add(New DataColumn("rangoInicial", GetType(Long)))
                    .Add(New DataColumn("rangoFinal", GetType(Long)))
                    .Add(New DataColumn("almacen", GetType(String)))
                    .Add(New DataColumn("centro", GetType(String)))
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
                    .Item("codigoProducto") = registro("codigoProducto").ToString.Trim
                    .Item("rangoInicial") = registro("rangoInicial").ToString.Trim
                    .Item("rangoFinal") = registro("rangoFinal").ToString.Trim
                    .Item("almacen") = registro("almacen").ToString.Trim
                    .Item("centro") = registro("centro").ToString.Trim
                End With
                EstructuraTablaBase.Rows.Add(registroFinal)
                index = index + 1
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ValidarEstructura() As Boolean
        Dim esValido As Boolean = True
        Dim index As Integer = 1
        Dim numInte As Integer
        Dim inicial As Integer
        Dim final As Integer
        Try
            For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                If fila.AllocatedCells.Count <> Me.EstructuraTabla.Columns.Count Then
                    AdicionarError(index, "Fila inválida", "El Número de columnas de la fila es inválido.")
                ElseIf index > 1 Then

                    If String.IsNullOrEmpty(fila.Cells(0).Value) Then
                        AdicionarError(index, "Dato inválido", "El código del producto no puede estar vacío.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(1).Value) Then
                        AdicionarError(index, "Dato inválido", "El rango inicial no puede estar vacío.")
                    Else
                        If Not IsNumeric(fila.Cells(1).Value) OrElse Not Integer.TryParse(fila.Cells(1).Value, numInte) Then
                            AdicionarError(index, "Dato inválido", "El rango inicial debe ser numérico.")
                        Else
                            Integer.TryParse(fila.Cells(1).Value, inicial)
                            Integer.TryParse(fila.Cells(2).Value, final)
                            If inicial > final Then
                                AdicionarError(index, "Dato inválido", "El rango inicial debe ser menor que el rango final.")
                            End If
                        End If
                    End If

                    If String.IsNullOrEmpty(fila.Cells(2).Value) Then
                        AdicionarError(index, "Dato inválido", "El rango final no puede estar vacío.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(3).Value) Then
                        AdicionarError(index, "Dato inválido", "El centro no puede estar vacío.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(4).Value) Then
                        AdicionarError(index, "Dato inválido", "El almacén no puede estar vacío.")
                    End If
                End If
                index += 1
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
            AdicionarColumnas()
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("usxp001"))

            If EstructuraTablaBase.Columns.Contains("idUsuario") Then EstructuraTablaBase.Columns.Remove("idUsuario")
            EstructuraTablaBase.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idUsuario))

            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .EjecutarNonQuery("LiberarDatosTransitoriosInventarioFinanciero", CommandType.StoredProcedure)

                    .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 600000
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaInventarioFinancieroSerializado"
                        .ColumnMappings.Add("codigoProducto", "codigoProducto")
                        .ColumnMappings.Add("rangoInicial", "rangoInicial")
                        .ColumnMappings.Add("rangoFinal", "rangoFinal")
                        .ColumnMappings.Add("almacen", "almacen")
                        .ColumnMappings.Add("centro", "centro")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(EstructuraTablaBase)
                    End With

                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    _estructuraTablaErrores = .EjecutarDataTable("ValidarDatosInventarioFinancieroSerializado", CommandType.StoredProcedure)

                    esValido = (EstructuraTablaErrores.Rows.Count = 0)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return esValido
    End Function

    Function CargarInventario() As ResultadoProceso
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
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarInventarioFinancieroSerializado", CommandType.StoredProcedure)

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
                    resultado.EstablecerMensajeYValor(500, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(400, "Se generó un error al cargar el inventario: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region

End Class
