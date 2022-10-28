Imports System.Web
Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.IO

Public Class RedistribucionSerialesCEArchivo

#Region "Atributos"

    Private _rutaArchivo As String
    Private _idDestino As Integer
    Private _idOrigen As String
    Private _dtErrorArchivo As DataTable
    Private _dtArchivo As DataTable
    Private _dsDatos As DataSet
    Private _tipoArchivo As String

#End Region

#Region "Propiedades"
    Public Property RutaArchivo As String
        Get
            Return _rutaArchivo
        End Get
        Set(value As String)
            _rutaArchivo = value
        End Set
    End Property

    Public Property IdDestino As Integer
        Get
            Return _idDestino
        End Get
        Set(value As Integer)
            _idDestino = value
        End Set
    End Property

    Public Property idOrigen As String
        Get
            Return _idOrigen
        End Get
        Set(value As String)
            _idOrigen = value
        End Set
    End Property

    Public Property DtErrorArchivo As DataTable
        Get
            Return _dtErrorArchivo
        End Get
        Set(value As DataTable)
            _dtErrorArchivo = value
        End Set
    End Property

    Public Property DtArchivo As DataTable
        Get
            Return _dtArchivo
        End Get
        Set(value As DataTable)
            _dtArchivo = value
        End Set
    End Property

    Public Property TipoArchivo As String
        Get
            Return _tipoArchivo
        End Get
        Set(value As String)
            _tipoArchivo = value
        End Set
    End Property
#End Region

#Region "Constructores"
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Métodos públicos"

    Public Function CargarArchivo() As ResultadoProceso

        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Dim resObtDatos As New ResultadoProceso
        Dim libRetorno As Integer

        Try
            If _rutaArchivo <> "" Then
                _dtErrorArchivo = CrearEstructuraErroresArchivo()
                _dtArchivo = ObtenerDatosArchivo(_rutaArchivo, resObtDatos)

                If _dtArchivo IsNot Nothing And _dtErrorArchivo.Rows.Count = 0 And _dtArchivo.Rows.Count > 0 Then

                    HttpContext.Current.Session("dtCargue") = _dtArchivo

                    Dim idUsuario As Integer = 0
                    If HttpContext.Current.Session("usxp001") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp001"), idUsuario)
                    Dim nombreEquipo As String
                    nombreEquipo = System.Net.Dns.GetHostName

                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.VarChar).Value = idUsuario
                        .SqlParametros.Add("@logonUser", SqlDbType.VarChar).Value = nombreEquipo
                        .SqlParametros.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                        .EjecutarNonQuery("LiberarRegistrosTransitoriaRedistribucionCE", CommandType.StoredProcedure)

                        Integer.TryParse(.SqlParametros("@result").Value.ToString, libRetorno)
                        If libRetorno = 1 Then
                            '***Se guarda temporalmente el Detalle de la Orden***'
                            .InicilizarBulkCopy()
                            With .BulkCopy
                                .DestinationTableName = "TransitoriaRedistribucionCE"
                                .ColumnMappings.Add("serial", "serial")
                                .ColumnMappings.Add("producto", "producto")
                                .ColumnMappings.Add("codigo", "codigo")
                                .ColumnMappings.Add("idUsuario", "idUsuario")
                                .ColumnMappings.Add("nombreEquipo", "nombreEquipo")
                                .ColumnMappings.Add("idOrigen", "idOrigen")
                                .ColumnMappings.Add("idDestino", "idDestino")
                                .WriteToServer(_dtArchivo)
                            End With

                            resultado.EstablecerMensajeYValor(1, "Se inserto correctamente el detalle en la tabla transitoria")
                        Else
                            resultado.EstablecerMensajeYValor(0, "Error al insertar el detalle en la tabla transitoria")
                        End If

                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idOrigen", SqlDbType.Int).Value = _idOrigen
                        .SqlParametros.Add("@idDestino", SqlDbType.Int).Value = _idDestino
                        .SqlParametros.Add("@idUsuario", SqlDbType.VarChar).Value = idUsuario
                        .SqlParametros.Add("@nombreEquipo", SqlDbType.VarChar).Value = nombreEquipo

                        _dsDatos = .EjecutarDataSet("ValidarDatosDetalleRedistribucionCE", CommandType.StoredProcedure)

                        _dsDatos.Tables(0).TableName = "dtDetalle"
                        _dsDatos.Tables(1).TableName = "dtErrorArchivo"

                        If _dsDatos.Tables("dtErrorArchivo").Rows.Count <= 0 Then
                            HttpContext.Current.Session("dtServiciosAdicionados") = _dsDatos.Tables("dtDetalle")
                        Else
                            HttpContext.Current.Session("dtErrores") = _dsDatos.Tables("dtErrorArchivo")
                            resultado.EstablecerMensajeYValor(2, "tablaErrores")
                        End If
                    End With
                Else
                    If _dtErrorArchivo IsNot Nothing And _dtErrorArchivo.Rows.Count > 0 Then
                        HttpContext.Current.Session("dtErrores") = _dtErrorArchivo
                        resultado.EstablecerMensajeYValor(2, "tablaErrores")
                    Else
                        If resObtDatos.Mensaje <> "" Then
                            resultado.EstablecerMensajeYValor(2, resObtDatos.Mensaje)
                        Else
                            resultado.EstablecerMensajeYValor(2, "Imposible cargar las recargas del sistema. Por favor intente nuevamente")
                        End If
                    End If
                End If
            Else
                resultado.EstablecerMensajeYValor(2, "Imposible Cargar los archivos al servidor, por favor intente nuevamente. ")
            End If
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(2, "Se presento un error al cargar los archivos: " & ex.Message)
        End Try

        Return resultado
    End Function
#End Region

#Region "Métodos privados"

    Private Function CrearEstructuraErroresArchivo() As DataTable
        Dim dtAux As New DataTable
        With dtAux.Columns
            .Add("columna", GetType(String))
            .Add("descripcion", GetType(String))
            .Add("fila", GetType(Integer))
            .Add("hoja", GetType(String))
        End With
        Return dtAux
    End Function

    Private Function ObtenerDatosArchivo(ByVal rutaArchivo As String, ByRef resultado As ResultadoProceso) As DataTable
        Dim miExcel As New ExcelFile
        Dim miWs As ExcelWorksheet
        Dim dtDatos As DataTable = Nothing
        Dim hayDatos As Boolean
        Dim dbManager As New LMDataAccess
        Try
            If Path.GetExtension(rutaArchivo) = ".xls" Then
                miExcel.LoadXls(rutaArchivo)
            ElseIf Path.GetExtension(rutaArchivo) = ".xlsx" Then
                miExcel.LoadXlsx(rutaArchivo, XlsxOptions.None)
            End If
            If miExcel.Worksheets.Count > 0 Then
                miWs = miExcel.Worksheets(0)
                Dim registros As Integer = miWs.Rows.Count
                If miWs.Rows.Count > 1 Then
                    dtDatos = CrearEstructuraArchivo()
                    Dim filaInicial As Integer = ObtenerFilaInicial(miWs)
                    If filaInicial <> -1 Then
                        For index As Integer = filaInicial To miWs.Rows.Count - 1
                            With miWs.Rows
                                hayDatos = HayDatosEnFila(.Item(index))
                                If hayDatos Then
                                    If .Item(index).AllocatedCells.Count > 0 Then
                                        AdicionarRegistro(dtDatos, .Item(index).Cells)
                                    End If
                                End If
                            End With
                        Next
                    Else
                        resultado.EstablecerMensajeYValor("-501", "El archivo no tiene el formato requerido. Por favor verifique")
                    End If
                Else
                    resultado.EstablecerMensajeYValor("-502", "El archivo no tiene el formato requerido. Por favor verifique")
                End If
                Dim x As Integer = 0
            Else
                resultado.EstablecerMensajeYValor("-503", "El archivo especificado no contiende Hojas. Por favor verifique")
            End If
        Catch ex As Exception
            resultado.EstablecerMensajeYValor("-504", "Imposible obtener datos del archivo. " & ex.Message)
        End Try

        If String.IsNullOrEmpty(resultado.Mensaje) And Not dtDatos Is Nothing Then
            resultado.EstablecerMensajeYValor("1", "Correcto")
        End If
        Return dtDatos
    End Function

    Private Function CrearEstructuraArchivo() As DataTable
        Dim dtAux As New DataTable
        With dtAux.Columns
            .Add("serial", GetType(String))
            .Add("producto", GetType(String))
            .Add("codigo", GetType(String))
            .Add("idUsuario", GetType(Integer))
            .Add("nombreEquipo", GetType(String))
            .Add("idOrigen", GetType(String))
            .Add("idDestino", GetType(Integer))
            '.Add("bodegaOrigen", GetType(String))
        End With
        Return dtAux
    End Function

    Private Function ObtenerFilaInicial(ByVal miWs As ExcelWorksheet) As Integer
        Dim filaInicial As Integer = -1
        For Each row As ExcelRow In miWs.Rows
            If row.AllocatedCells.Count = 3 Then
                If row.AllocatedCells(0).Value IsNot Nothing Then
                    If row.AllocatedCells(0).Value.ToString.Trim.ToUpper.IndexOf("SERIAL") >= 0 Then
                        filaInicial = row.Index + 1
                        Exit For
                    ElseIf IsNumeric(row.AllocatedCells(0).Value) Then
                        filaInicial = row.Index
                        Exit For
                    End If
                End If
            End If
        Next
        Return filaInicial
    End Function

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

    Private Sub AdicionarRegistro(ByRef dtDatos As DataTable, ByVal infoFila As CellRange)
        Dim drAux As DataRow
        drAux = dtDatos.NewRow

        With infoFila
            Dim idUsuario As Integer = 0
            If HttpContext.Current.Session("usxp001") IsNot Nothing Then Integer.TryParse(HttpContext.Current.Session("usxp001"), idUsuario)

            If String.IsNullOrEmpty(.Item(0).Value) Then
                AdicionarErrorArchivo("serial", "Es un campo obligatorio.", .FirstRowIndex + 1, "DATOS")
            ElseIf .Item(0).Value.ToString.Trim.Length < 15 Then
                AdicionarErrorArchivo("serial", "La longitud minima de caracteres es de 15", .FirstRowIndex + 1, "DATOS")
            ElseIf .Item(0).Value.ToString.Trim.Length > 22 Then
                AdicionarErrorArchivo("serial", "La longitud maxima de caracteres es de 22", .FirstRowIndex + 1, "DATOS")
            Else
                drAux("serial") = .Item(0).Value
            End If

            If String.IsNullOrEmpty(.Item(1).Value) Then
                AdicionarErrorArchivo("producto", "Es un campo obligatorio.", .FirstRowIndex + 1, "DATOS")
            End If
            drAux("producto") = .Item(1).Value

            If String.IsNullOrEmpty(.Item(2).Value) Then
                AdicionarErrorArchivo("codigo", "Es un campo obligatorio.", .FirstRowIndex + 1, "DATOS")
            End If
            drAux("codigo") = .Item(2).Value

            drAux("idUsuario") = idUsuario
            drAux("nombreEquipo") = System.Net.Dns.GetHostName
            drAux("idOrigen") = _idOrigen
            drAux("idDestino") = _idDestino

            dtDatos.Rows.Add(drAux)
            dtDatos.AcceptChanges()

        End With
    End Sub

    Private Sub AdicionarErrorArchivo(ByVal documento As String, ByVal descripcion As String, ByVal fila As Integer, ByVal hoja As String)
        Dim drAux As DataRow
        drAux = _dtErrorArchivo.NewRow
        drAux("columna") = documento
        drAux("descripcion") = descripcion
        drAux("fila") = fila
        drAux("hoja") = hoja
        _dtErrorArchivo.Rows.Add(drAux)
    End Sub

#End Region
End Class
