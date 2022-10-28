Imports System.Web
Imports GemBox.Spreadsheet
Imports LMDataAccessLayer
Imports System.IO

Public Class CargueArchivoDeclaraciones

#Region "Atributos."

    Private oExcel As ExcelFile
    Private _estructuraTablaBase As DataTable
    Private _estructuraTabla As DataTable
    Private _estructuraTablaErrores As DataTable
    Private _estructuraTablaResumen As DataTable
    Private _idUsuario As Integer
    Private _declaracion As String
    Private _extencion As String
    Private _Ruta As String

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

    Public Property EstructuraTablaResumen() As DataTable
        Get
            Return _estructuraTablaResumen
        End Get
        Set(value As DataTable)
            _estructuraTablaResumen = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property Declaracion() As String
        Get
            Return _declaracion
        End Get
        Set(value As String)
            _declaracion = value
        End Set
    End Property

    Public Property Extension() As String
        Get
            Return _extencion
        End Get
        Set(value As String)
            _extencion = value
        End Set
    End Property

    Public Property Ruta() As String
        Get
            Return _Ruta
        End Get
        Set(value As String)
            _Ruta = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New(ByRef ArchivoExcel As ExcelFile)
        MyBase.New()
        oExcel = ArchivoExcel
    End Sub

    Public Sub New()
        MyBase.New()

    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub EstructuraDatosBase()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTablaBase Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("factura", GetType(String)))
                    .Add(New DataColumn("guia", GetType(String)))
                    .Add(New DataColumn("serial", GetType(String)))
                    .Add(New DataColumn("declaracion", GetType(String)))
                    .Add(New DataColumn("linea", GetType(Integer)))
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
                    .Add(New DataColumn("factura", GetType(String)))
                    .Add(New DataColumn("guia", GetType(String)))
                    .Add(New DataColumn("serial", GetType(String)))
                    .Add(New DataColumn("declaracion", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTabla = dtDatos
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub AdicionarError(ByVal id As Integer, ByVal nombre As String, ByVal descripcion As String, ByVal serial As String, ByVal linea As String)
        Try
            With EstructuraTablaErrores
                Dim drError As DataRow = .NewRow()
                With drError
                    .Item("id") = id
                    .Item("nombre") = nombre
                    .Item("descripcion") = descripcion
                    .Item("serial") = serial
                    .Item("linea") = Linea
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
                    .Add(New DataColumn("serial", GetType(String)))
                    .Add(New DataColumn("linea", GetType(String)))
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
                e.DataTableValue = Convert.ToInt64(e.ExcelValue).ToString()
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
            Dim index As Integer
            If _extencion.ToUpper = ".TXT" Then
                index = 0
            Else
                index = 1
            End If
            Dim fila As ExcelRow = oExcel.Worksheets(0).Rows(0)

            Dim dtDatos As DataTable = EstructuraTabla()
            AddHandler oExcel.Worksheets(0).ExtractDataEvent, AddressOf ExtractDataErrorHandler
            If _extencion.ToUpper = ".TXT" Then
                oExcel.Worksheets(0).ExtractToDataTable(dtDatos, oExcel.Worksheets(0).Rows.Count, ExtractDataOptions.SkipEmptyRows, oExcel.Worksheets(0).Rows(0), oExcel.Worksheets(0).Columns(0))
            Else
                oExcel.Worksheets(0).ExtractToDataTable(dtDatos, oExcel.Worksheets(0).Rows.Count, ExtractDataOptions.SkipEmptyRows, oExcel.Worksheets(0).Rows(1), oExcel.Worksheets(0).Columns(0))
            End If

            'Se crea la estructura por Filas
            For Each registro As DataRow In dtDatos.Rows
                Dim registroFinal As DataRow = EstructuraTablaBase.NewRow()
                With registroFinal
                    .Item("factura") = registro("factura").ToString.Trim
                    .Item("guia") = registro("guia").ToString.Trim
                    .Item("serial") = registro("serial").ToString.Trim
                    .Item("declaracion") = registro("declaracion").ToString.Trim
                    .Item("linea") = EstructuraTablaBase.Rows.Count + 1
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

    Private Sub AgregarSeriales(ByVal factura As String, ByVal guia As String, ByVal serial As String, ByVal declaracion As String)
        If _estructuraTablaBase Is Nothing Then
            EstructuraDatosBase()
        End If
        Dim dr As DataRow = _estructuraTablaBase.NewRow
        dr("factura") = factura
        dr("guia") = guia
        dr("serial") = serial
        dr("declaracion") = declaracion
        dr("linea") = _estructuraTablaBase.Rows.Count + 1
        _estructuraTablaBase.Rows.Add(dr)
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function ValidarInformacion() As Boolean
        Dim esValido As Boolean = True
        Dim index As Integer = 1
        Dim dato As String
        Dim hayDatos As Boolean
        Try
            For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                hayDatos = HayDatosEnFila(oExcel.Worksheets(0).Rows.Item(index - 1))
                If fila.AllocatedCells.Count <> Me.EstructuraTabla.Columns.Count Then
                    AdicionarError(index, "Fila inválida", "El Número de columnas de la fila es inválido.", "", "")
                ElseIf index = 1 Then
                    EstructuraDatosBase()
                    If _estructuraTabla.Columns(0).ToString <> fila.Cells(0).Value.ToString.ToLower Or _estructuraTabla.Columns(1).ToString <> fila.Cells(1).Value.ToString.ToLower.Replace("í", "i") Or _
                       _estructuraTabla.Columns(2).ToString <> fila.Cells(2).Value.ToString.ToLower Or _estructuraTabla.Columns(3).ToString <> fila.Cells(3).Value.ToString.ToLower Then
                        AdicionarError(index, "Estructura Errada", "El orden de las columnas no concuerda con el Orden esperado o los nombres no coinciden con la estructura definida (factura-guia-serial-declaracion).", "", "")
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

    Function LeerPlano() As Boolean
        Dim _esValido As Boolean
        Try
            Dim dtDatos As New DataTable
            Dim lectorArchivo As StreamReader = Nothing
            Dim linea As String
            Dim arregloDatos() As String
            Dim numLinea As Integer = 1

            If Ruta <> "" Then
                Dim dato As String
                lectorArchivo = File.OpenText(Ruta)
                Do While lectorArchivo.Peek >= 0
                    Dim _error As Boolean = False
                    linea = lectorArchivo.ReadLine
                    If Not String.IsNullOrEmpty(linea) Then
                        arregloDatos = linea.Split(vbTab)
                        If arregloDatos.Length = 4 Then
                            If Not _error Then Me.AgregarSeriales(arregloDatos(0), arregloDatos(1), arregloDatos(2), arregloDatos(3))
                        Else
                            AdicionarError(numLinea, "Fila inválida", "El Número de columnas de la fila es inválido.", "", "")
                        End If
                    Else
                        AdicionarError(numLinea, "Fila inválida", "El número de linea se encuentra vacia, por favor verificar", "", "")
                    End If
                    numLinea += 1
                Loop
                _esValido = Not (EstructuraTablaErrores.Rows.Count > 0)
                If _estructuraTablaBase.Rows.Count = 0 Then
                    AdicionarError(0, "Datos Invalidos", "El archivo no contiene registros válidos. Por favor verifique", "", "")
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return _esValido
    End Function

    Public Function ObtenerInformacion() As DataTable
        Try
            If _extencion.ToLower <> ".txt" Then
                AdicionarColumnas()
            End If
            If EstructuraTablaBase.Columns.Contains("idUsuario") Then EstructuraTablaBase.Columns.Remove("idUsuario")
            EstructuraTablaBase.Columns.Add(New DataColumn("idUsuario", GetType(Integer), _idUsuario))

            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .ejecutarNonQuery("LiberarDatosTransitoriosDeclaraciones", CommandType.StoredProcedure)
                    .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 600000
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaDeclaracionesSerial"
                        .ColumnMappings.Add("factura", "factura")
                        .ColumnMappings.Add("guia", "guia")
                        .ColumnMappings.Add("serial", "serial")
                        .ColumnMappings.Add("declaracion", "declaracion")
                        .ColumnMappings.Add("linea", "linea")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(EstructuraTablaBase)
                    End With
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    _estructuraTablaResumen = .ejecutarDataTable("ValidarDatosDeclaraciones", CommandType.StoredProcedure)
                    If .SqlParametros("@resultado").Value.ToString = 1 Then
                        If _estructuraTablaErrores Is Nothing Then
                            EstructuraDatosErrores()
                        End If
                        For i As Integer = 0 To _estructuraTablaResumen.Rows.Count - 1
                            AdicionarError(_estructuraTablaResumen.Rows(i).Item("id"), _estructuraTablaResumen.Rows(i).Item("nombre"), _estructuraTablaResumen.Rows(i).Item("descripcion"), _estructuraTablaResumen.Rows(i).Item("serial"), _estructuraTablaResumen.Rows(i).Item("linea"))
                        Next
                        _estructuraTablaResumen.Clear()
                    End If
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return _estructuraTablaResumen
    End Function

    Public Function ObtenerResumenInformacion() As DataTable
        Dim dt As New DataTable
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    dt = .ejecutarDataTable("ValidarDatosDeclaraciones", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Public Function RegistrarDeclaracionIndividual() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarDeclaracionIndividual", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                        Else
                            .abortarTransaccion()
                        End If
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                    End If

                End With
            End With
        Catch ex As Exception
            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

    Function RegistarDeclaracionTotal() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 300).Direction = ParameterDirection.Output

                    .iniciarTransaccion()
                    .ejecutarNonQuery("RegistrarDeclaraciones", CommandType.StoredProcedure)

                    If Integer.TryParse(.SqlParametros("@resultado").Value, resultado.Valor) Then
                        resultado.Valor = .SqlParametros("@resultado").Value
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        If resultado.Valor = 0 Then
                            .confirmarTransaccion()
                        Else
                            .abortarTransaccion()
                        End If
                    Else
                        .abortarTransaccion()
                        resultado.EstablecerMensajeYValor(400, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                    End If

                End With
            End With
        Catch ex As Exception
            If dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            resultado.EstablecerMensajeYValor(500, "Se presentó un error al generar el registro: " & ex.Message)
        End Try
        Return resultado
    End Function

#End Region





End Class
