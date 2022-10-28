Imports System.IO
Imports GemBox.Spreadsheet
Imports ILSBusinessLayer.Localizacion

Public Class AdministradorArchivos

#Region "Atributos"
    Private _TipoArchivoCarga As TipoArchivoCarga
    Private _extensionArchivo As Extension
    Private _rutaServidor As String
    Private _pesoArchivo As Long
    Private _filaInicial As Integer
    Private _columnaInicial As Integer
    Private _listaDestinos As Hashtable
    Private _arrColumnasArchivo As List(Of Columna)
    Private _idUsuario As Long
    Private _dtErrores As DataTable


    Public Enum TipoArchivoCarga
        MatrizTransporte = 1
        ValoresDeclarados = 2
        DespachosRIM = 4
    End Enum

    Protected Friend Structure Columna
        Dim nombre As String
        Dim posicion As Integer
    End Structure

    Public Structure Validacion
        Dim Nombre As String
        Dim ExpresionRegular As String
        Dim Extension As String
    End Structure

    Public Enum Formato
        texto
        Excel
        word
    End Enum

    Public Enum Extension
        Excel
        Texto
    End Enum
#End Region

#Region "Propiedades"
    Public ReadOnly Property PesoArchivo() As Long
        Get
            Return _pesoArchivo
        End Get
    End Property

    Public Property RutaServidor() As String
        Get
            Return _rutaServidor
        End Get
        Set(ByVal value As String)
            _rutaServidor = value
        End Set
    End Property

    Public Property IdUsuario() As Long
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Long)
            _idUsuario = value
        End Set

    End Property

    Public ReadOnly Property ExtensionArchivo() As Extension
        Get
            Return ExtensionArchivo
        End Get
    End Property

    Public ReadOnly Property ListaErrores() As DataTable
        Get
            Return _dtErrores
        End Get
    End Property

    Public ReadOnly Property ContieneErrores() As Boolean
        Get
            If _dtErrores.Rows.Count > 0 Then
                Return True
            Else : Return False
            End If
        End Get
    End Property
#End Region

#Region "Métodos compartidos"
    Public Shared Function ObtenerDatosFormato(ByVal miFormato As Formato) As Validacion
        Dim datosFormato As New Validacion
        Select Case miFormato
            Case Formato.Excel
                datosFormato.Extension = ".xls"
                datosFormato.Nombre = "Excel 97 - 2003"
                datosFormato.ExpresionRegular = ".+\.([xX][lL][sS])([xX]?)"
            Case Formato.texto
                datosFormato.Extension = ".txt"
                datosFormato.Nombre = "Documento de Texto"
                datosFormato.ExpresionRegular = ".+\.([tT][xX][tT])"
            Case Formato.word
                datosFormato.Extension = ".doc"
                datosFormato.Nombre = "Word 97 - 2003"
                datosFormato.ExpresionRegular = ".+\.([dD][oO][cC])"
        End Select
        Return datosFormato
    End Function

    Public Shared Function ObtenerTiposArchivoCarga() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dt As DataTable
        dt = db.ejecutarDataTable("SELECT idTipoarchivo, nombre FROM TipoArchivoCarga WHERE estado = 1")
        Return dt
    End Function

    Public Shared Function ObtenerListadoCentros() As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New DataTable

        resultado = adminBD.ejecutarDataTable("SELECT DISTINCT centro FROM AlmacenBodega WHERE activo = 1 order by centro")

        Return resultado
    End Function

    Public Shared Function ObtenerListadoMaterialesCentro(Optional ByVal centro As String = "") As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New DataTable

        If centro = "" Then
            adminBD.agregarParametroSQL("@centro", DBNull.Value)
            resultado = adminBD.ejecutarDataTable("ObtenerListadoMaterialesCentro", CommandType.StoredProcedure)
        Else
            adminBD.agregarParametroSQL("@centro", centro)
            resultado = adminBD.ejecutarDataTable("ObtenerListadoMaterialesCentro", CommandType.StoredProcedure)
        End If


        Return resultado
    End Function

    Public Shared Function ObtenerListadoMateriales() As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New DataTable

        adminBD.agregarParametroSQL("@idEstado", 1)
        resultado = adminBD.ejecutarDataTable("ObtenerListadoMateriales", CommandType.StoredProcedure)
        
        Return resultado
    End Function

    Public Shared Function ObtenerListadoConceptoAsignacionPrecio() As DataTable
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim resultado As New DataTable

        resultado = adminBD.ejecutarDataTable("ObtenerListadoConceptoAsignacionPrecio", CommandType.StoredProcedure)

        Return resultado
    End Function

    Public Shared Function ObtenerValorDeclaradoActual(ByVal centro As String, ByVal material As String) As Double
        Dim adminBD As New LMDataAccessLayer.LMDataAccess
        Dim resultado As Double = 0

        adminBD.agregarParametroSQL("@centro", centro)
        adminBD.agregarParametroSQL("@material", material, SqlDbType.VarChar)

        resultado = adminBD.ejecutarScalar("SELECT TOP 1 precio FROM PreciosMaterial WHERE centro = @centro AND material = @material")

        Return resultado
    End Function

    Public Shared Sub EditarValorDeclarado(ByVal centro As Integer, ByVal material As String, ByVal valorDeclarado As Double)
        Dim adminBD As New LMDataAccessLayer.LMDataAccess

        adminBD.agregarParametroSQL("@centro", centro)
        adminBD.agregarParametroSQL("@material", material, SqlDbType.VarChar)
        adminBD.agregarParametroSQL("@valorDeclarado", valorDeclarado, SqlDbType.Float)

        adminBD.ejecutarNonQuery("EditarValorDeclarado", CommandType.StoredProcedure)
    End Sub


#End Region

#Region "Constructor"
    Public Sub New()
        _listaDestinos = New Hashtable
        _arrColumnasArchivo = New List(Of Columna)
        EstablecerLicenciaGembox()
        _dtErrores = New DataTable
        _dtErrores.Columns.Add(New DataColumn("Fila"))
        _dtErrores.Columns.Add(New DataColumn("Mensaje"))
    End Sub
#End Region

#Region "Métodos privados"
    Private Sub EstablecerLicenciaGembox()
        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
    End Sub

    Public Sub RegistrarError(ByVal linea As String, ByVal mensaje As String)
        Dim fila As DataRow = _dtErrores.NewRow
        fila("Fila") = linea + 1
        fila("Mensaje") = mensaje
        _dtErrores.Rows.Add(fila)
    End Sub

    Private Function ObternerHoja() As ExcelWorksheet
        Dim miExcel As New ExcelFile
        Dim miWs As ExcelWorksheet
        Try
            Try
                miExcel.LoadXls(_rutaServidor)
            Catch ex As Exception
                Try
                    miExcel.LoadCsv(_rutaServidor, CsvType.TabDelimited)
                Catch es As Exception
                    Throw New Exception("El archivo esta corrupto o no tiene el formato esperado. Por favor verifique")
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
            Throw New Exception("El archivo esta corrupto o no tiene el formato esperado. Por favor verifique" & ex.Message)
        End Try
    End Function

    Protected Friend Sub CargarColumnas()
        Dim db As New LMDataAccessLayer.LMDataAccess
        db.agregarParametroSQL("@idTipoArchivo", _TipoArchivoCarga, SqlDbType.Int)
        Dim col As Columna
        Dim dReader As SqlClient.SqlDataReader = db.ejecutarReader("SeleccionarColumnasArchivo", CommandType.StoredProcedure)
        _arrColumnasArchivo.Clear()
        Try
            While dReader.Read()
                col = New Columna
                col.nombre = dReader("nombreColumna")
                col.posicion = dReader("posicionOrdinal")
                _arrColumnasArchivo.Add(col)
            End While
            _columnaInicial = _arrColumnasArchivo(0).posicion
        Finally
            dReader.Close()
            db.cerrarConexion()
        End Try
    End Sub

    Protected Friend Function CargarColumnas(ByVal tipoArchivo As TipoArchivoCarga) As List(Of Columna)
        _TipoArchivoCarga = tipoArchivo
        Me.CargarColumnas()
        Return _arrColumnasArchivo
    End Function

    Private Function ObtenerFilaInicial(ByRef miWs As ExcelWorksheet) As Integer
        _filaInicial = -1

        Dim primeraColumna As Integer = _arrColumnasArchivo(0).posicion - 1
        Dim textoColumna As String = _arrColumnasArchivo(0).nombre
        Dim auxTexto As String
        For index As Integer = 0 To miWs.Rows.Count - 1
            With miWs.Rows
                If miWs.Rows.Item(index).Cells(primeraColumna).Value IsNot Nothing Then
                    If miWs.Rows.Item(index).Cells(primeraColumna).Value.ToString.ToUpper() = textoColumna.ToUpper() Then
                        For Each col As Columna In _arrColumnasArchivo
                            auxTexto = miWs.Rows.Item(index).Cells(col.posicion - 1).Value
                            If auxTexto Is Nothing Then
                                Me.RegistrarError(index, "El archivo " & _TipoArchivoCarga.ToString & " No contiene el orden de columnas esperado")
                                Exit For
                            ElseIf auxTexto.Trim.ToLower <> col.nombre.Trim.ToLower() Then
                                Me.RegistrarError(index, "El archivo " & _TipoArchivoCarga.ToString & " No contiene el orden de columnas esperado")
                                Exit For
                            End If
                        Next
                        _filaInicial = index + 1
                        Exit For
                    End If
                End If
            End With
        Next
        If _filaInicial < 0 Then Me.RegistrarError(0, "El archivo " & _TipoArchivoCarga.ToString & " No contiene el orden de columnas esperado")
        Return _filaInicial
    End Function

    Private Sub SubirMatriz(ByVal dtDatos As DataTable, ByVal IdUsuario As Integer)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            db.iniciarTransaccion()
            db.inicilizarBulkCopy()

            db.agregarParametroSQL("idModificador", IdUsuario)
            db.ejecutarNonQuery("GuardarHistorialRutasTransportadoras", CommandType.StoredProcedure)

            db.SqlParametros.Clear()
            db.ejecutarNonQuery("EliminarInformacionActualTransportadoras", CommandType.StoredProcedure)
            
            db.BulkCopy.DestinationTableName = "dbo.InformacionRutasTransportadoras"
            db.BulkCopy.WriteToServer(dtDatos)
            db.confirmarTransaccion()
        Catch ex As Exception
            db.abortarTransaccion()
            Throw New Exception("Error al tratar de subir la matriz de transporte" & ex.Message)
        End Try
    End Sub


    Private Sub SubirValoresDeclarados(ByVal dtDatos As DataTable, ByVal idUsuario As Integer)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            db.iniciarTransaccion()
            db.agregarParametroSQL("@idModificador", idUsuario)
            db.agregarParametroSQL("@idFuenteSincronizacion", 1)
            db.ejecutarNonQuery("GuardarHistorialPreciosMaterial", CommandType.StoredProcedure)

            db.SqlParametros.Clear()
            db.ejecutarNonQuery("EliminarInformacionActualValoresDeclarados", CommandType.StoredProcedure)

            db.inicilizarBulkCopy()
            With db.BulkCopy
                .DestinationTableName = "dbo.PreciosMaterial"
                .ColumnMappings.Add("material", "material")
                .ColumnMappings.Add("centro", "centro")
                .ColumnMappings.Add("precio", "precio")
                .ColumnMappings.Add("idConceptoAsignacionPrecio", "idConceptoAsignacionPrecio")
                .WriteToServer(dtDatos)
            End With
            db.confirmarTransaccion()
        Catch ex As Exception
            db.abortarTransaccion()
            Throw New Exception("Error al tratar de subir los valores declarados" & ex.Message)
        End Try
    End Sub

    Private Function CrearEstructuraDeDatos()
        Dim dt As New DataTable
        Dim columnas As String = ""
        Select Case _TipoArchivoCarga
            Case TipoArchivoCarga.MatrizTransporte
                columnas = "idCiudadOrigen,idCiudadDestino,idTipoDestinatario,idTipoProducto,idTransportadora,idTipoTransporte,idTipoMovimientoTransporte,codigo,idTipoServicio"
            Case TipoArchivoCarga.ValoresDeclarados
                columnas = "material,centro,precio,idConceptoAsignacionPrecio"
            Case TipoArchivoCarga.DespachosRIM
                columnas = "idDespacho,codigo, cliente, serialEquipo, pin, fechaActivacion, fechaDesactivacion, fechaNacionalizacion, fechaTerminacionGarantiaProveedor, observacion, " & _
                      "codigoFalla, descripcionFalla, serialCaja, referenciaPrestamo, cac, responsable, tipoCliente, identificacion, codigoMIN, telefonoFijo, email, guia, factura, valor," & _
                      "nivelReparacion"
        End Select
        Dim arrcolumnas As String() = columnas.Split(",")
        For Each nombreColumna As String In arrcolumnas
            dt.Columns.Add(New DataColumn(nombreColumna))
        Next
        If dt.Columns.Count = 0 Then
            Throw New Exception("No se ha podido obtener la estructura de los datos")
        Else
            Return dt
        End If
    End Function

#End Region

#Region "Matriz transporte"
    Public Sub CargarMatrizTransporte(ByVal ruta As String, ByVal idUsuario As Integer)
        _TipoArchivoCarga = TipoArchivoCarga.MatrizTransporte
        _rutaServidor = ruta
        Dim miWs As ExcelWorksheet
        Dim dt As DataTable
        miWs = ObternerHoja()
        Me.CargarColumnas()
        _filaInicial = Me.ObtenerFilaInicial(miWs)
        If Not Me.ContieneErrores Then
            dt = Me.LeerMatriz(miWs)
            If Not Me.ContieneErrores Then SubirMatriz(dt, idUsuario)
        End If
    End Sub

    Private Function LeerMatriz(ByRef miWs As ExcelWorksheet) As DataTable
        Dim filtroTransportadoras As Estructuras.FiltroTransportadora
        filtroTransportadoras.Activo = Enumerados.EstadoBinario.Activo
        filtroTransportadoras.CargaPorImportacion = 2
        Dim dtDatos As DataTable = CrearEstructuraDeDatos()
        Dim dtCiudades As DataTable = Ciudad.ObtenerCiudadesPorPais(170)
        Dim dtTransportadoras As DataTable = Transportadora.ListadoTransportadoras(filtroTransportadoras)
        Dim dtTiposProducto As DataTable = Transportadora.ListadoTipoProductos
        Dim dtTiposDestinatario As DataTable = Transportadora.ListadoTipoDestinatarios
        Dim dtTiposTransporte As DataTable = Transportadora.ListadoTipoTransporte
        Dim dtTiposMovimientoTransporte As DataTable = Transportadora.ListadoMovimientosTransporte
        Dim dtTipoServicio As DataTable = Transportadora.ListadoTipoServicio
        Dim i As Integer = 0
        Try
            For index As Integer = _filaInicial To miWs.Rows.Count - 1
                i = index
                With miWs.Rows
                    If .Item(index).AllocatedCells.Count >= _arrColumnasArchivo.Count Then
                        AdicionarRegistroMatriz(dtDatos, .Item(index).Cells, dtCiudades, dtTransportadoras, dtTiposProducto, dtTiposDestinatario, _
                                                dtTiposTransporte, dtTiposMovimientoTransporte, dtTipoServicio)
                    End If
                End With
            Next
        Catch ex As Exception
            Throw New Exception("Imposible obtener datos del archivo. Iterador = " & i & ex.Message)
        End Try
        Return dtDatos
    End Function

    Private Sub AdicionarRegistroMatriz(ByRef dtDatos As DataTable, ByVal infoFila As CellRange, ByRef ciudades As DataTable, _
                                        ByRef transportadoras As DataTable, ByRef tiposProducto As DataTable, ByRef tiposDestinatario As DataTable, _
                                        ByRef tiposTransporte As DataTable, ByRef tiposMovimientoTransporte As DataTable, ByRef tipoServicio As DataTable)
        Dim drAux As DataRow

        With infoFila
            If .Item(_columnaInicial - 1).Value IsNot Nothing AndAlso .Item(_columnaInicial - 1).MergedRange Is Nothing Then
                Dim idCiudadOrigen As String = ""
                Dim idCiudadDestino As String = ""
                Dim idTransportadora As String = ""
                Dim idTipoProducto As String = ""
                Dim idTipoDestinatario As String = ""
                Dim idTipoTransporte As String = ""
                Dim codigo As String = ""
                Dim idTipoMovimientoTransporte As String = ""
                Dim idTipoServicio As String = ""
                Dim flagValidacion As Boolean = False
                drAux = dtDatos.NewRow

                If .Item(_arrColumnasArchivo(0).posicion - 1).Value IsNot Nothing Then idCiudadOrigen = .Item(_arrColumnasArchivo(0).posicion - 1).Value.ToString
                If .Item(_arrColumnasArchivo(1).posicion - 1).Value IsNot Nothing Then idCiudadDestino = .Item(_arrColumnasArchivo(1).posicion - 1).Value.ToString
                If .Item(_arrColumnasArchivo(2).posicion - 1).Value IsNot Nothing Then idTipoDestinatario = .Item(_arrColumnasArchivo(2).posicion - 1).Value.ToString
                If .Item(_arrColumnasArchivo(3).posicion - 1).Value IsNot Nothing Then idTipoProducto = .Item(_arrColumnasArchivo(3).posicion - 1).Value.ToString
                If .Item(_arrColumnasArchivo(4).posicion - 1).Value IsNot Nothing Then idTransportadora = .Item(_arrColumnasArchivo(4).posicion - 1).Value.ToString
                If .Item(_arrColumnasArchivo(5).posicion - 1).Value IsNot Nothing Then idTipoTransporte = .Item(_arrColumnasArchivo(5).posicion - 1).Value.ToString
                If .Item(_arrColumnasArchivo(6).posicion - 1).Value IsNot Nothing Then idTipoMovimientoTransporte = .Item(_arrColumnasArchivo(6).posicion - 1).Value.ToString
                If .Item(_arrColumnasArchivo(7).posicion - 1).Value IsNot Nothing Then codigo = .Item(_arrColumnasArchivo(7).posicion - 1).Value.ToString
                If .Item(_arrColumnasArchivo(8).posicion - 1).Value IsNot Nothing Then idTipoServicio = .Item(_arrColumnasArchivo(8).posicion - 1).Value.ToString

                If ciudades.Select("idciudad=" & CType(Val(idCiudadOrigen.Trim), Integer)).Length = 0 Then
                    RegistrarError(.FirstRowIndex, "El código de ciudad origen no existe o se encuentra inactivo")
                    flagValidacion = True
                Else
                    drAux("idCiudadOrigen") = idCiudadOrigen
                End If

                If ciudades.Select("idciudad=" & CType(Val(idCiudadDestino.Trim), Integer)).Length = 0 Then
                    RegistrarError(.FirstRowIndex, "El código de ciudad destino no existe o se encuentra inactivo")
                    flagValidacion = True
                Else
                    drAux("idCiudadDestino") = idCiudadDestino
                End If
                If tiposDestinatario.Select("idTipoDestinatario=" & CType(Val(idTipoDestinatario.Trim), Integer)).Length = 0 Then
                    RegistrarError(.FirstRowIndex, "El código de tipo de destinatario no existe o se encuentra inactivo")
                    flagValidacion = True
                Else
                    drAux("idTipoDestinatario") = idTipoDestinatario
                End If
                If tiposProducto.Select("idTipoProducto=" & CType(Val(idTipoProducto.Trim), Integer)).Length = 0 Then
                    RegistrarError(.FirstRowIndex, "El código de producto no existe o se encuentra inactivo")
                    flagValidacion = True
                Else
                    drAux("idTipoProducto") = idTipoProducto
                End If
                If transportadoras.Select("idTransportadora=" & CType(Val(idTransportadora.Trim), Integer)).Length = 0 Then
                    RegistrarError(.FirstRowIndex, "El código de transportadora no existe o se encuentra inactivo")
                    flagValidacion = True
                Else
                    drAux("idTransportadora") = idTransportadora
                End If
                If tiposTransporte.Select("idTipo=" & CType(Val(idTipoTransporte.Trim), Integer)).Length = 0 Then
                    RegistrarError(.FirstRowIndex, "El código de tipo de transporte no existe o se encuentra inactivo")
                    flagValidacion = True
                Else
                    drAux("idTipoTransporte") = idTipoTransporte
                End If
                If tiposMovimientoTransporte.Select("idTipoMovimientoTransporte=" & CType(Val(idTipoMovimientoTransporte.Trim), Integer)).Length = 0 Then
                    RegistrarError(.FirstRowIndex, "El código de tipo de alistamiento de transporte no existe o se encuentra inactivo")
                    flagValidacion = True
                Else
                    drAux("idTipoMovimientoTransporte") = idTipoMovimientoTransporte
                End If

                If tipoServicio.Select("idTipoServicio =" & CType(Val(idTipoServicio.Trim), Integer)).Length = 0 Then
                    RegistrarError(.FirstRowIndex, "El tipo de servicio de transportadora no existe.")
                    flagValidacion = True
                Else
                    drAux("idTipoServicio") = idTipoServicio
                End If

                drAux("codigo") = codigo

                If Not flagValidacion Then
                    If dtDatos.Select("idCiudadOrigen='" & idCiudadOrigen.ToString & "'" & " AND idCiudadDestino='" & idCiudadDestino.ToString & "'" & " AND idTipoProducto='" & idTipoProducto.ToString & "'" & " AND idTipoDestinatario='" & idTipoDestinatario.ToString & "'").Length = 1 Then
                        RegistrarError(.FirstRowIndex, "Los datos de esta fila se encuentran repetidos")
                    Else
                        dtDatos.Rows.Add(drAux)
                    End If
                End If
            End If
        End With
    End Sub

    Public Shared Function GenerarPlantillaMatriz() As DataTable
        Dim resultado As New DataTable
        Dim adminDB As New LMDataAccessLayer.LMDataAccess

        Try
            resultado = adminDB.ejecutarDataTable("ObtenerMatrizTransporte", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            adminDB.Dispose()
        End Try

        Return resultado
    End Function

    Public Shared Function GenerarPlantillaValores() As DataTable
        Dim resultado As New DataTable
        Dim adminDB As New LMDataAccessLayer.LMDataAccess

        Try
            resultado = adminDB.ejecutarDataTable("ObtenerPreciosMaterial", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            adminDB.Dispose()
        End Try

        Return resultado
    End Function
#End Region

#Region "Valores Declarados"
    Public Sub CargarValoresDeclarados(ByVal ruta As String, ByVal idUsuario As Integer)
        _TipoArchivoCarga = TipoArchivoCarga.ValoresDeclarados
        _rutaServidor = ruta
        Dim miWs As ExcelWorksheet
        Dim dt As DataTable
        miWs = ObternerHoja()
        Me.CargarColumnas()
        _filaInicial = Me.ObtenerFilaInicial(miWs)
        If Not Me.ContieneErrores Then
            dt = Me.LeerValores(miWs)
            If Not ContieneErrores Then SubirValoresDeclarados(dt, idUsuario)
        End If
    End Sub

    Private Function LeerValores(ByRef miWs As ExcelWorksheet) As DataTable
        Dim dtDatos As DataTable = CrearEstructuraDeDatos()
        Dim dtMateriales As DataTable = ObtenerListadoMateriales()
        Dim dtCentros As DataTable = ObtenerListadoCentros()
        Dim dtConceptos As DataTable = ObtenerListadoConceptoAsignacionPrecio()
        Try
            For index As Integer = _filaInicial To miWs.Rows.Count - 1
                With miWs.Rows
                    If .Item(index).AllocatedCells.Count >= _arrColumnasArchivo.Count Then
                        AdicionarRegistroValores(dtDatos, .Item(index).Cells, dtMateriales, dtCentros, dtConceptos)
                    End If
                End With
            Next

        Catch ex As Exception
            Throw New Exception("Imposible obtener datos del archivo. " & ex.Message)
        End Try
        Return dtDatos
    End Function

    Private Sub AdicionarRegistroValores(ByRef dtDatos As DataTable, ByVal infoFila As CellRange, ByRef materiales As DataTable, ByRef centros As DataTable, ByRef conceptos As DataTable)
        Dim drAux As DataRow

        For Each dr As DataRow In conceptos.Rows
            With infoFila
                If .Item(_columnaInicial).Value IsNot Nothing AndAlso .Item(_columnaInicial).MergedRange Is Nothing Then
                    Dim centro As String = ""
                    Dim material As String = ""
                    Dim precio As String = ""
                    drAux = dtDatos.NewRow

                    If .Item(_arrColumnasArchivo(0).posicion - 1).Value IsNot Nothing Then material = .Item(_arrColumnasArchivo(0).posicion - 1).Value.ToString.Trim
                    If .Item(_arrColumnasArchivo(1).posicion - 1).Value IsNot Nothing Then centro = .Item(_arrColumnasArchivo(1).posicion - 1).Value.ToString.Trim
                    If .Item(_arrColumnasArchivo(2).posicion - 1).Value IsNot Nothing Then precio = .Item(_arrColumnasArchivo(2).posicion - 1).Value.ToString.Trim

                    If materiales.Select("material='" & material.ToString & "'").Length = 0 Then
                        RegistrarError(.FirstRowIndex, "El material indicado no existe")
                        Exit For
                    Else
                        drAux("material") = material
                    End If

                    If centros.Select("centro='" & centro.ToString & "'").Length = 0 Then
                        RegistrarError(.FirstRowIndex, "El centro indicado no existe")
                        Exit For
                    Else
                        drAux("centro") = centro
                    End If

                    drAux("precio") = precio

                    drAux("idConceptoAsignacionPrecio") = dr("idConceptoAsignacionPrecio")

                    If dtDatos.Select("material='" & material.ToString & "'" & " AND centro = '" & centro.ToString & "'" & " AND idConceptoAsignacionPrecio = " & dr("idConceptoAsignacionPrecio").ToString).Length = 1 Then
                        RegistrarError(.FirstRowIndex, "La combinación de precio indicada ya existe")
                        Exit For
                    Else
                        dtDatos.Rows.Add(drAux)
                    End If

                End If
            End With
        Next
        
    End Sub
#End Region

#Region "DespachosRIM"
    Public Function CargarDespachosRIM(ByVal ruta As String) As DataTable
        _TipoArchivoCarga = TipoArchivoCarga.DespachosRIM
        _rutaServidor = ruta
        Dim miWs As ExcelWorksheet
        Dim dt As DataTable
        miWs = ObternerHoja()
        Me.CargarColumnas()
        _filaInicial = Me.ObtenerFilaInicial(miWs)
        If Not Me.ContieneErrores Then
            dt = Me.LeerDespachoRIM(miWs)
            If dt.Rows.Count = 0 Then Me.RegistrarError("1", "El archivo no contiene registros")
            If Not Me.ContieneErrores Then Return dt
        End If
    End Function

    Private Function LeerDespachoRIM(ByRef miWs As ExcelWorksheet) As DataTable
        Dim dtDatos As DataTable = CrearEstructuraDeDatos()
        Try
            ' miWs.ExtractToDataTable(dtDatos, miWs.Rows.Count, ExtractDataOptions.None, map, miWs.Rows(_filaInicial))
            For index As Integer = _filaInicial To miWs.Rows.Count - 1
                With miWs.Rows
                    If .Item(index).AllocatedCells.Count >= _arrColumnasArchivo.Count Then
                        AdicionarRegistroDespachoRIM(dtDatos, .Item(index).Cells)
                    End If
                End With
            Next
            Return dtDatos
        Catch ex As Exception
            Throw New Exception("Imposible obtener datos del archivo. " & ex.Message)
        End Try
        Return dtDatos
    End Function

    Private Sub AdicionarRegistroDespachoRIM(ByRef dtDatos As DataTable, ByVal infoFila As CellRange)
        Dim drAux As DataRow
        If infoFila.Item(_columnaInicial).Value IsNot Nothing AndAlso infoFila.Item(_columnaInicial).MergedRange Is Nothing Then
            drAux = dtDatos.NewRow
            For i As Integer = 0 To dtDatos.Columns.Count - 1
                If infoFila.Item(_arrColumnasArchivo(i).posicion - 1).Value IsNot Nothing Then
                    Select Case _arrColumnasArchivo(i).posicion
                        Case 6, 7, 8
                            Dim auxFecha As Date
                            Dim valorCelda As String = infoFila.Item(_arrColumnasArchivo(i).posicion - 1).Value
                            If Date.TryParse(valorCelda, auxFecha) Then
                                drAux(i) = infoFila.Item(_arrColumnasArchivo(i).posicion - 1).Value
                            ElseIf infoFila.Item(_arrColumnasArchivo(i).posicion - 1).Value.ToString.ToLower() <> "n/a" Then
                                Me.RegistrarError(infoFila.FirstRowIndex, "El Formato de fecha de la columna " & _arrColumnasArchivo(i).nombre & " no es válido")
                            End If
                        Case 23
                            Dim valor As Long
                            If Long.TryParse(infoFila.Item(_arrColumnasArchivo(i).posicion - 1).Value.ToString(), valor) Then
                                drAux(i) = infoFila.Item(_arrColumnasArchivo(i).posicion - 1).Value
                            End If
                        Case Is <= 16
                            Dim valorCelda As String = infoFila.Item(_arrColumnasArchivo(i).posicion - 1).Value
                            If Not String.IsNullOrEmpty(valorCelda) Then
                                drAux(i) = infoFila.Item(_arrColumnasArchivo(i).posicion - 1).Value
                            Else
                                Me.RegistrarError(infoFila.FirstRowIndex, "El campo " & _arrColumnasArchivo(i).nombre & " está vacío, debe ingresar esta información")
                            End If
                        Case Else
                            drAux(i) = infoFila.Item(_arrColumnasArchivo(i).posicion - 1).Value
                    End Select
                ElseIf _arrColumnasArchivo(i).posicion <= 16 Then
                    Me.RegistrarError(infoFila.FirstRowIndex, "El campo " & _arrColumnasArchivo(i).nombre & " está vacío, debe ingresar esta información")
                End If

            Next
            dtDatos.Rows.Add(drAux)
        End If
    End Sub
#End Region

End Class
