Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports ILSBusinessLayer.Comunes
Imports System.Web

Public Class CargueInventarioCemMasivoSerializado
#Region "Atributos (Propiedades)"
    Property cedulaCliente As String
    Property idUsuario As Integer
    Property idBodega As Integer
    Property idTipoBodega As Integer
    Property idPosicion As Integer
    Property idServicio As String
    Property idTipoServicio As Integer
    Property serial As String
    Property material As String
    Property resultado As New ResultadoProceso
    Private dbManager As New LMDataAccess

    Private oExcel As ExcelFile
    Private _estructuraTablaBase As DataTable
    Private _estructuraTabla As DataTable
    Private _estructuraTablaErrores As DataTable
#End Region

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
#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByRef ArchivoExcel As ExcelFile)
        MyBase.New()
        oExcel = ArchivoExcel
    End Sub
    Private Sub EstructuraDatosBase()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTablaBase Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("Serial", GetType(String)))
                    .Add(New DataColumn("Posicion", GetType(String)))
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
                    .Add(New DataColumn("Serial", GetType(String)))
                    .Add(New DataColumn("Posicion", GetType(String)))
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
                    .Item("Serial") = registro("Serial").ToString.Trim
                    .Item("Posicion") = registro("Posicion").ToString.Trim
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
#End Region

#Region "Métodos Públicos"
    Public Function ConsultarTipoBodega() As DataTable
        Dim dtDatos As New DataTable
        If dbManager IsNot Nothing Then dbManager = New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                End With
                dtDatos = .EjecutarDataTable("obtenerTipoBodega", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function ConsultarBodegasPosicion() As DataTable
        Dim dtDatos As New DataTable
        If dbManager IsNot Nothing Then dbManager = New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idTipoBodega", SqlDbType.Int).Value = idTipoBodega
                End With
                dtDatos = .EjecutarDataTable("listarBodegas", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function


    Public Function ConsultarBodegas() As DataTable
        Dim dtDatos As New DataTable
        If dbManager IsNot Nothing Then dbManager = New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()

                End With
                dtDatos = .EjecutarDataTable("ConsultarBodegaServiciosFinancieros", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function


    Public Function ConsultarPosiconesBodegas() As DataTable
        Dim dtDatos As New DataTable
        If dbManager IsNot Nothing Then dbManager = New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idBodega", SqlDbType.Int).Value = idBodega
                End With
                dtDatos = .EjecutarDataTable("ObtenerInfoPosicionBodega", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function
    Public Function ConsultarMaterialesTipoServicio() As DataTable
        Dim dtDatos As New DataTable
        If dbManager IsNot Nothing Then dbManager = New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idTiposServicio", SqlDbType.Int).Value = idTipoServicio
                End With
                dtDatos = .EjecutarDataTable("ObtenerMaterialesTipoServicio", CommandType.StoredProcedure)
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dtDatos
    End Function


    Public Sub RegistrarSerialFinanciero()
        Try
            With dbManager

                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = idBodega
                .SqlParametros.Add("@idPosicion", SqlDbType.Int).Value = idPosicion
                .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = serial
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .IniciarTransaccion()
                .EjecutarNonQuery("posicionarSerialEnBodega", CommandType.StoredProcedure)

                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultado.Valor = CShort(.SqlParametros("@resultado").Value)
                    If resultado.Valor = 1 Then
                        resultado.Mensaje = "El serial fue registrado satisfactoriamente."
                        .ConfirmarTransaccion()
                    Else
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        .AbortarTransaccion()
                    End If
                Else
                    Throw New Exception("Ocurrió un error interno al registrar serial. Por favor intente nuevamente")
                    .AbortarTransaccion()
                End If
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

    End Sub

    Public Sub RegistrarSerialFinancieroSinServicio()
        Try
            With dbManager
                .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = idTipoServicio
                .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = idBodega
                .SqlParametros.Add("@material", SqlDbType.VarChar, 50).Value = material
                .SqlParametros.Add("@serial", SqlDbType.VarChar, 50).Value = serial
                .SqlParametros.Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                .IniciarTransaccion()
                .EjecutarNonQuery("RegistrarSerialServicioFinancieroSinServicio", CommandType.StoredProcedure)

                If Not IsDBNull(.SqlParametros("@resultado").Value) Then
                    resultado.Valor = CShort(.SqlParametros("@resultado").Value)
                    If resultado.Valor = 1 Then
                        resultado.Mensaje = "El serial fue registrado satisfactoriamente."
                        .ConfirmarTransaccion()
                    Else
                        resultado.Mensaje = .SqlParametros("@mensaje").Value
                        .AbortarTransaccion()
                    End If
                Else
                    Throw New Exception("Ocurrió un error interno al registrar serial. Por favor intente nuevamente")
                    .AbortarTransaccion()
                End If
            End With
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

    End Sub

    Function ObtenerInventarioProductoFinanciero() As ResultadoProceso
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@consecutivo", SqlDbType.VarChar).Value = serial
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .ejecutarReader("ObtenerProductosFinancierosTransitoriosSerial", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.HasRows Then
                        Dim listSeriales As New List(Of String)
                        While .Reader.Read
                            listSeriales.Add(.Reader("serial").ToString)
                        End While
                        .Reader.Close()
                        EnviarInventarioProductoFinanciero(listSeriales.ToArray)
                    End If
                Else
                    resultado.EstablecerMensajeYValor(500, "No se logró establecer respuesta del servidor, por favor intentelo nuevamente.")
                End If
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(400, "Se generó un error al cargar el inventario: " & ex.Message)
        End Try
        Return resultado
    End Function
    Public Function ValidarEstructura() As Boolean
        Dim esValido As Boolean = True
        Dim index As Integer = 1
        Dim numInte As Double
        Dim dato As String
        Dim hayDatos As Boolean
        'Dim expresion As New ConfigValues("EXPREG_GENERAL")
        'Dim oExpReg As New System.Text.RegularExpressions.Regex(expresion.ConfigKeyValue)
        Try
            For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                hayDatos = HayDatosEnFila(oExcel.Worksheets(0).Rows.Item(index - 1))
                If fila.AllocatedCells.Count <> Me.EstructuraTabla.Columns.Count Then
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

                    If String.IsNullOrEmpty(fila.Cells(1).Value) Then
                        AdicionarError(index, "Dato inválido", "El codigo de la posicion no puede estar vacío.")
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
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("userId"))

            If EstructuraTablaBase.Columns.Contains("idUsuario") Then EstructuraTablaBase.Columns.Remove("idUsuario")
            EstructuraTablaBase.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idUsuario))

            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .EjecutarNonQuery("LiberarDatosTransitoriosPosicionSerial", CommandType.StoredProcedure)

                    .InicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 0
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaPosicionamientoSerializado"
                        .ColumnMappings.Add("Serial", "Serial")
                        .ColumnMappings.Add("Posicion", "Posicion")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(EstructuraTablaBase)
                    End With

                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    _estructuraTablaErrores = .EjecutarDataTable("ValidarDatosPosiconSerial", CommandType.StoredProcedure)

                    esValido = (EstructuraTablaErrores.Rows.Count = 0)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return esValido
    End Function
    Function RegistrarInventario() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("userId"))
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                    .Add("@mensaje", SqlDbType.VarChar, 2000).Direction = ParameterDirection.Output
                    .Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                End With
                .IniciarTransaccion()
                .ejecutarReader("RegistrarPosicionSerialMasivo", CommandType.StoredProcedure)

                Dim respuesta As Integer = .SqlParametros("@resultado").Value
                If respuesta = 0 Then
                    resultado.Valor = respuesta
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    If Not .Reader.IsClosed Then .Reader.Close()
                    .ConfirmarTransaccion()
                Else
                    resultado.Valor = respuesta
                    resultado.Mensaje = .SqlParametros("@mensaje").Value
                    .AbortarTransaccion()
                End If
            End With

        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            resultado.EstablecerMensajeYValor(400, "Se generó un error al registrar el inventario: " & ex.Message)
        End Try
        Return resultado
    End Function
    Public Sub EnviarInventarioProductoFinanciero(ByVal listSeriales() As String)
        Dim servicioNotusExpressBancolombia As New NotusExpressBancolombiaService.NotusExpressBancolombiaService()
        servicioNotusExpressBancolombia.RegistraSerialInventario(listSeriales:=listSeriales)
    End Sub

#End Region

End Class
