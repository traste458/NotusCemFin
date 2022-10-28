Imports GemBox.Spreadsheet
Imports System.IO
Imports LMDataAccessLayer
Imports System.Web
Imports ILSBusinessLayer
Imports ILSBusinessLayer.Comunes
Imports System.Text

Public Class CrearCargueRealceDavivienda
#Region "Atributos"

    Private _rutaArchivo As String
    Private _dtErrorArchivo As DataTable
    Private _dtArchivo As DataTable
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
        Dim idCargue As String

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
                        .SqlParametros.Add("@nombreEquipo", SqlDbType.VarChar).Value = nombreEquipo

                        .EjecutarNonQuery("SP_LiberarTransitoriaCargueRealceDavivienda", CommandType.StoredProcedure)

                        Dim rBCDatos As New ResultadoProceso

                        rBCDatos = BulkCopyDatos(_dtArchivo)

                        If rBCDatos.Valor = 1 Then
                            .SqlParametros.Clear()
                            .SqlParametros.Add("@idUsuario", SqlDbType.VarChar).Value = idUsuario
                            .SqlParametros.Add("@nombreEquipo", SqlDbType.VarChar).Value = nombreEquipo

                            _dtErrorArchivo = .EjecutarDataTable("SP_ValidarCargueRealceDavivienda", CommandType.StoredProcedure)

                            If _dtErrorArchivo.Rows.Count <= 0 Then
                                .SqlParametros.Clear()
                                .SqlParametros.Add("@idUsuario", SqlDbType.VarChar).Value = idUsuario
                                .SqlParametros.Add("@nombreEquipo", SqlDbType.VarChar).Value = nombreEquipo
                                .SqlParametros.Add("@result", SqlDbType.Int).Direction = ParameterDirection.ReturnValue

                                Dim result As Short = 0

                                .EjecutarNonQuery("SP_RegistrarCargueRealcesDavivienda", CommandType.StoredProcedure)

                                Short.TryParse(.SqlParametros("@result").Value.ToString, result)

                                If result <> 1 Then
                                    resultado.EstablecerMensajeYValor(2, "No se pudo establecer contacto con la base de datos")
                                End If
                            End If

                            If _dtErrorArchivo.Rows.Count > 0 Then
                                HttpContext.Current.Session("dtErrores") = _dtErrorArchivo
                                resultado.EstablecerMensajeYValor(2, "tablaErrores")
                            Else
                                resultado.EstablecerMensajeYValor(1, "El cargue se realizo satisfactoriamennte.")
                            End If
                        Else
                            resultado.EstablecerMensajeYValor(2, rBCDatos.Mensaje)
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

    Public Function ObtenerEntregaPoolDavivienda(ByVal fechaInicio As String,
                                                 ByVal FechaFin As String) As DataTable

        Dim dt As New DataTable
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@fechaInicio", SqlDbType.VarChar).Value = fechaInicio
                    .Add("@fechaFin", SqlDbType.VarChar).Value = FechaFin
                End With
                dt = .EjecutarDataTable("SP_ObtenerPoolEntregaDavivienda", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            If dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
        Return dt
    End Function

#End Region

#Region "Métodos privados"

    Private Function CrearEstructuraErroresArchivo() As DataTable
        Dim dtAux As New DataTable
        With dtAux.Columns
            .Add("Columna", GetType(String))
            .Add("Descripción", GetType(String))
            .Add("Fila", GetType(Integer))
            .Add("Hoja", GetType(String))
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
            .Add("Pseudocodigo", GetType(String))
            .Add("CompaniaDistribucion", GetType(String))
            .Add("FechaEnvioRealce", GetType(String))
            .Add("NumeroIdentificacion", GetType(String))
            .Add("NombreCliente", GetType(String))
            .Add("PlantaRealce", GetType(String))
            .Add("PuntoEntrega", GetType(String))
            .Add("LineaExcel", GetType(String))
            .Add("IdUsuario", GetType(String))
            .Add("NombreEquipo", GetType(String))

        End With
        Return dtAux
    End Function



    Private Function ObtenerFilaInicial(ByVal miWs As ExcelWorksheet) As Integer
        Dim filaInicial As Integer = -1
        For Each row As ExcelRow In miWs.Rows
            If row.AllocatedCells.Count = 7 Then
                If row.AllocatedCells(0).Value IsNot Nothing Then
                    If row.AllocatedCells(0).Value.ToString.Trim.ToUpper.IndexOf("PSEUDOCODIGO") >= 0 Then
                        filaInicial = row.Index + 1
                        Exit For
                    ElseIf IsDate(row.AllocatedCells(0).Value) Then
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
            drAux("IdUsuario") = idUsuario
            drAux("NombreEquipo") = System.Net.Dns.GetHostName
            drAux("LineaExcel") = .FirstRowIndex + 1


            If .Item(0).Value Is Nothing Then
                AdicionarErrorArchivo("Pseudocodigo", "Es un campo obligatorio.", .FirstRowIndex + 1, "Realces")
            Else
                Dim fila() As DataRow = dtDatos.Select("Pseudocodigo = '" & .Item(0).Value.ToString.Trim & "'")
                If fila.Length > 0 Then
                    AdicionarErrorArchivo("Pseudocodigo", "Ya existe el Número de Servicio en el archivo.", .FirstRowIndex + 1, "Realces")
                Else
                    Dim orden As String = .Item(0).Value
                    If orden.Length > 50 Then
                        AdicionarErrorArchivo("Pseudocodigo", "La longitud debe ser menor a 50.", .FirstRowIndex + 1, "Realces")
                    Else
                        If orden.IndexOf(",") > 0 Or orden.IndexOf(".") > 0 Then
                            AdicionarErrorArchivo("Pseudocodigo", "Contiene caracteres no relacionados al numero de orden.", .FirstRowIndex + 1, "Realces")
                        Else
                            If IsNumeric(orden) Then
                                drAux("Pseudocodigo") = .Item(0).Value
                            Else
                                AdicionarErrorArchivo("Pseudocodigo", "Debe ser numerico.", .FirstRowIndex + 1, "Realces")
                            End If
                        End If
                    End If
                End If
            End If

            If .Item(1).Value Is Nothing Then
                AdicionarErrorArchivo("CompaniaDistribucion", "Es un campo obligatorio.", .FirstRowIndex + 1, "Realces")
            Else
                Dim nombre As String = .Item(1).Value
                If nombre.Length > 100 Then
                    AdicionarErrorArchivo("CompaniaDistribucion", "La longitud debe ser menor a 100.", .FirstRowIndex + 1, "Realces")
                Else
                    drAux("CompaniaDistribucion") = .Item(1).Value
                End If
            End If

            If .Item(2).Value Is Nothing Then
                AdicionarErrorArchivo("Numero Identificacion", "Es un campo obligatorio.", .FirstRowIndex + 1, "Realces")
            Else
                Dim cedula As String = .Item(2).Value
                If cedula.Length > 50 Then
                    AdicionarErrorArchivo("Numero Identificacion", "La longitud debe ser menor a 50.", .FirstRowIndex + 1, "Realces")
                Else
                    drAux("NumeroIdentificacion") = .Item(2).Value
                End If
            End If

            If .Item(3).Value Is Nothing Then
                AdicionarErrorArchivo("Fecha Envio Realce", "Es un campo obligatorio.", .FirstRowIndex + 1, "Realces")
            Else
                Dim apellido As String = .Item(3).Value
                If apellido.Length > 10 Then
                    AdicionarErrorArchivo("Fecha Envio Realce", "La longitud debe ser menor a 10.", .FirstRowIndex + 1, "Realces")
                Else
                    drAux("FechaEnvioRealce") = .Item(3).Value
                End If
            End If


            If .Item(4).Value Is Nothing Then
                AdicionarErrorArchivo("Nombre Cliente", "Es un campo obligatorio.", .FirstRowIndex + 1, "Realces")
            Else
                Dim cedula As String = .Item(4).Value
                If cedula.Length > 100 Then
                    AdicionarErrorArchivo("Nombre Cliente", "La longitud debe ser menor a 100.", .FirstRowIndex + 1, "Realces")
                Else
                    drAux("NombreCliente") = .Item(4).Value
                End If
            End If

            If .Item(5).Value Is Nothing Then
                AdicionarErrorArchivo("Planta Realce", "Es un campo obligatorio.", .FirstRowIndex + 1, "Realces")
            Else
                Dim cedula As String = .Item(5).Value
                If cedula.Length > 50 Then
                    AdicionarErrorArchivo("Planta Realce", "La longitud debe ser menor a 50.", .FirstRowIndex + 1, "Realces")
                Else
                    drAux("PlantaRealce") = .Item(5).Value
                End If
            End If

            If .Item(6).Value Is Nothing Then
                AdicionarErrorArchivo("Punto Entrega", "Es un campo obligatorio.", .FirstRowIndex + 1, "Realces")
            Else
                Dim cedula As String = .Item(6).Value
                If cedula.Length > 50 Then
                    AdicionarErrorArchivo("Punto Entrega", "La longitud debe ser menor a 50.", .FirstRowIndex + 1, "Realces")
                Else
                    drAux("PuntoEntrega") = .Item(6).Value
                End If
            End If

            dtDatos.Rows.Add(drAux)
        End With
    End Sub


    Private Sub AdicionarErrorArchivo(ByVal documento As String, ByVal descripcion As String, ByVal fila As Integer, ByVal hoja As String)
        Dim drAux As DataRow
        drAux = _dtErrorArchivo.NewRow
        drAux("Columna") = documento
        drAux("Descripción") = descripcion
        drAux("Fila") = fila
        drAux("Hoja") = hoja
        _dtErrorArchivo.Rows.Add(drAux)
    End Sub

    Private Function BulkCopyDatos(ByVal dtDatos As DataTable) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                .InicilizarBulkCopy()
                .TiempoEsperaComando = 0
                With .BulkCopy
                    .DestinationTableName = "TransitoriaCargueRealceDavivienda"

                    .ColumnMappings.Add("Pseudocodigo", "pSeudocodigo")
                    .ColumnMappings.Add("CompaniaDistribucion", "companiaDistribucion")
                    .ColumnMappings.Add("FechaEnvioRealce", "fechaEnvioRealce")
                    .ColumnMappings.Add("NumeroIdentificacion", "numeroIdentificacionCliente")
                    .ColumnMappings.Add("NombreCliente", "nombreCliente")
                    .ColumnMappings.Add("PlantaRealce", "plantaRealce")
                    .ColumnMappings.Add("PuntoEntrega", "puntoEntrega")
                    .ColumnMappings.Add("LineaExcel", "lineaExcel")
                    .ColumnMappings.Add("IdUsuario", "idUsuarioCreacion")
                    .ColumnMappings.Add("NombreEquipo", "nombreEquipoCreacion")


                    '.ColumnMappings.Add("NumeroServicio", "numeroServicio")
                    '.ColumnMappings.Add("Nombres", "nombres")
                    '.ColumnMappings.Add("Apellidos", "apellidos")
                    '.ColumnMappings.Add("CC", "identificacionCliente")
                    '.ColumnMappings.Add("TelefonoFijo", "telefonoFijo")
                    '.ColumnMappings.Add("Celular", "celular")
                    '.ColumnMappings.Add("Ciudad", "ciudad")
                    '.ColumnMappings.Add("Direccion", "direccion")
                    '.ColumnMappings.Add("Barrio", "barrio")
                    ''.ColumnMappings.Add("CodigoCiudad", "codigoCiudad")
                    '.ColumnMappings.Add("CodigoCiudad", "idCiudad")
                    '.ColumnMappings.Add("Pool", "idpool")
                    '.ColumnMappings.Add("EquipoSKU", "equipoSKU")
                    '.ColumnMappings.Add("SimSKU", "simSKU")
                    '.ColumnMappings.Add("TipoSiniestro", "tipoSiniestro")
                    '.ColumnMappings.Add("IdUsuario", "idUsuario")
                    '.ColumnMappings.Add("NombreEquipo", "nombreEquipo")
                    '.ColumnMappings.Add("LineaExcel", "lineaExcel")
                    '.ColumnMappings.Add("OrdenVentaDax", "ordenVentaDax")
                    .WriteToServer(dtDatos)
                End With
            End With
            resultado.EstablecerMensajeYValor(1, "BulkCopy correcto")
        Catch ex As Exception
            resultado.EstablecerMensajeYValor(-511, ex.Message)
        End Try
        Return resultado
    End Function

    Private Function ReemplazarTildes(ByVal textoOriginal As String) As String
        Dim reg As RegularExpressions.Regex
        Dim textoNormalizado As String = textoOriginal.Normalize(NormalizationForm.FormD)
        reg = New RegularExpressions.Regex("[^a-zA-Z0-9 ]")
        Dim textoSinAcentos As String = reg.Replace(textoNormalizado, "")
        Return textoSinAcentos
    End Function



#End Region

End Class
