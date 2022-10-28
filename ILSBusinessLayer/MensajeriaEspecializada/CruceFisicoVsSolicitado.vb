Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports ILSBusinessLayer.Comunes
Imports System.Web
Public Class CruceFisicoVsSolicitado

#Region "Atributos"
    Private oExcel As ExcelFile
    Private _estructuraTablaBase As DataTable
    Private _estructuraTablaErrores As DataTable
    Private _dsDatos As DataSet

    Private _serial As String
    Private _idEstado As Integer = -1
    Private _fechaIni As Date
    Private _fechaFin As Date

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
    Public Property DsDatos() As DataSet
        Get
            Return _dsDatos
        End Get
        Set(value As DataSet)
            _dsDatos = value
        End Set
    End Property
    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(value As String)
            _serial = value
        End Set
    End Property
    Public Property IdEstado() As Integer
        Get
            Return _idEstado
        End Get
        Set(value As Integer)
            _idEstado = value
        End Set
    End Property
    Public Property FechaIni() As Date
        Get
            Return _fechaIni
        End Get
        Set(value As Date)
            _fechaIni = value
        End Set
    End Property
    Public Property FechaFin() As Date
        Get
            Return _fechaFin
        End Get
        Set(value As Date)
            _fechaFin = value
        End Set
    End Property
#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByRef ArchivoExcel As ExcelFile)
        MyBase.New()
        oExcel = ArchivoExcel
    End Sub
#End Region

#Region "Metodos Privados"
    Private Sub EstructuraDatosBase()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTablaBase Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("serial", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTablaBase = dtDatos
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

            Dim dtDatos As DataTable = EstructuraTablaBase()
            AddHandler oExcel.Worksheets(0).ExtractDataEvent, AddressOf ExtractDataErrorHandler
            oExcel.Worksheets(0).ExtractToDataTable(dtDatos, oExcel.Worksheets(0).Rows.Count, ExtractDataOptions.SkipEmptyRows, oExcel.Worksheets(0).Rows(1), oExcel.Worksheets(0).Columns(0))

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

#End Region

#Region "Métodos Públicos"

    Public Function ValidarEstructura() As Boolean
        Dim esValido As Boolean = True
        Dim index As Integer = 1
        Dim numInte As Double
        Dim dato As String
        Dim hayDatos As Boolean
        Dim expresion As New ConfigValues("EXPREG_GENERAL")
        Dim oExpReg As New System.Text.RegularExpressions.Regex(expresion.ConfigKeyValue)
        Try
            For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                hayDatos = HayDatosEnFila(oExcel.Worksheets(0).Rows.Item(index - 1))
                If fila.AllocatedCells.Count <> Me.EstructuraTablaBase.Columns.Count Then
                    AdicionarError(index, "Fila inválida", "El Número de columnas de la fila es inválido.")
                ElseIf index > 1 Then
                    dato = fila.Cells(0).Value
                    If dato IsNot Nothing Then
                        If Not IsNumeric(fila.Cells(0).Value) OrElse Not Double.TryParse(fila.Cells(0).Value, numInte) Then
                            AdicionarError(index, "Dato inválido", "El serial debe ser numérico.")
                        End If
                    Else
                        AdicionarError(index, "Dato inválido", "El campo serial no puede estar vacio.")
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
    Public Function ValidarInformacion() As ResultadoProceso
        Dim _result As New ResultadoProceso
        Dim _valor As Integer

        AdicionarColumnas()
        Dim idUsuario As Integer = CInt(HttpContext.Current.Session("userId"))

        If EstructuraTablaBase.Columns.Contains("idUsuario") Then EstructuraTablaBase.Columns.Remove("idUsuario")
        EstructuraTablaBase.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idUsuario))

        Using dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .EjecutarNonQuery("LiberarDatosTransitoriosCruceFisicoVsSolicitado", CommandType.StoredProcedure)

                    .InicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 600000
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaCruceFisicoVsSolicitado"
                        .ColumnMappings.Add("serial", "serial")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(EstructuraTablaBase)
                    End With

                    .IniciarTransaccion()

                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.Output
                    _dsDatos = .EjecutarDataSet("ValidarDatosCruceFisicoVsSolicitado", CommandType.StoredProcedure)
                    Integer.TryParse(.SqlParametros("@resultado").Value.ToString(), _valor)
                    _result.EstablecerMensajeYValor(_valor, "")

                    If _result.Valor = 0 Then
                        .ConfirmarTransaccion()
                    Else
                        .AbortarTransaccion()
                    End If

                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
                Throw ex
            End Try
        End Using
        Return _result
    End Function

    Function ObtenerDatosConsultaCruceFisicoVsSolicitado() As DataTable
        Dim dt As New DataTable
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _serial <> "" Then .SqlParametros.Add("@serial", SqlDbType.VarChar).Value = _serial
                    If _idEstado <> -1 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = _idEstado
                    If _fechaIni <> Date.MinValue Then .SqlParametros.Add("@fechaIni", SqlDbType.Date).Value = _fechaIni
                    If _fechaFin <> Date.MinValue Then .SqlParametros.Add("@fechaFin", SqlDbType.Date).Value = _fechaFin

                    dt = .EjecutarDataTable("ObtieneCruceSerialesFisicoVsSolicitado", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

#End Region



End Class
