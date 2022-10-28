Imports GemBox.Spreadsheet
Imports System.Web
Imports LMDataAccessLayer

Namespace InventarioFisico

    Public Class CargueInventarioClienteBLL

#Region "Atributos"

        Private oExcel As ExcelFile
        Private _estructuraTabla As DataTable
        Private _estructuraTablaErrores As DataTable

#End Region

#Region "Constantes"

        Private Const CAMPOS_BASE As Short = 4

        Private Const C01_Serial As Short = 0
        Private Const C02_Material As Short = 1
        Private Const C03_Centro As Short = 2
        Private Const C04_Almacen As Short = 3
        Private Const C05_Stock As Short = 4

#End Region

#Region "Propiedades"

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

        Private Sub EstructuraDatos()
            Try
                Dim dtDatos As New DataTable
                If _estructuraTabla Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("serial", GetType(String)))
                        .Add(New DataColumn("material", GetType(String)))
                        .Add(New DataColumn("centro", GetType(String)))
                        .Add(New DataColumn("almacen", GetType(String)))
                        .Add(New DataColumn("stock", GetType(String)))
                    End With
                    dtDatos.AcceptChanges()
                    _estructuraTabla = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Sub EstructuraDatosErrores()
            Try
                Dim dtDatos As New DataTable
                If _estructuraTablaErrores Is Nothing Then
                    With dtDatos.Columns
                        .Add(New DataColumn("Id", GetType(Integer)))
                        .Add(New DataColumn("Nombre", GetType(String)))
                        .Add(New DataColumn("Descripcion", GetType(String)))
                    End With
                    dtDatos.AcceptChanges()
                    _estructuraTablaErrores = dtDatos
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

#End Region

#Region "Métodos Públicos"

        Private Sub AdicionarError(ByVal id As Integer, ByVal nombre As String, ByVal descripcion As String)
            Try
                With EstructuraTablaErrores
                    Dim drError As DataRow = .NewRow()
                    With drError
                        .Item("Id") = id
                        .Item("Nombre") = nombre
                        .Item("Descripcion") = descripcion
                    End With
                    .Rows.Add(drError)
                    .AcceptChanges()
                End With
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function ValidarEstructura() As Boolean
            Dim esValido As Boolean = True
            Dim index As Integer = 1

            For Each fila As ExcelRow In oExcel.Worksheets(0).Rows
                If fila.AllocatedCells.Count <> Me.EstructuraTabla.Columns.Count Then
                    AdicionarError(index, "Fila inválida", "El Número de columnas de la fila es inválido.")
                Else
                    If String.IsNullOrEmpty(fila.Cells(C01_Serial).Value) Then
                        AdicionarError(index, "Dato inválido", "El serial es requerido.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(C02_Material).Value) Then
                        AdicionarError(index, "Dato inválido", "El Material es requerido.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(C03_Centro).Value) Then
                        AdicionarError(index, "Dato inválido", "El Centro es requerido.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(C04_Almacen).Value) Then
                        AdicionarError(index, "Dato inválido", "El Almacén es requerido.")
                    End If

                    If String.IsNullOrEmpty(fila.Cells(C05_Stock).Value) Then
                        AdicionarError(index, "Dato inválido", "El Stock es requerido.")
                    End If
                End If
                index += 1
            Next
            esValido = Not (EstructuraTablaErrores.Rows.Count > 0)
            
            Return esValido
        End Function

        Public Function RegistrarInformacion() As ResultadoProceso
            Dim respuesta As New ResultadoProceso
            Dim idUsuario As Integer

            Try
                Integer.TryParse(HttpContext.Current.Session("usxp001"), idUsuario)

                AddHandler oExcel.Worksheets(0).ExtractDataEvent, AddressOf ExtractDataErrorHandler
                oExcel.Worksheets(0).ExtractToDataTable(EstructuraTabla, oExcel.Worksheets(0).Rows.Count, ExtractDataOptions.SkipEmptyRows, oExcel.Worksheets(0).Rows(1), oExcel.Worksheets(0).Columns(0))

                If EstructuraTabla.Columns.Contains("idUsuario") Then EstructuraTabla.Columns.Remove("idUsuario")
                EstructuraTabla.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idUsuario))

                AdicionarColumnaAutoIncremento(EstructuraTabla)

                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .EjecutarNonQuery("LiberarDatosTransitoriosCargueInventario", CommandType.StoredProcedure)

                        .InicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                        .TiempoEsperaComando = 600000
                        With .BulkCopy
                            .DestinationTableName = "TransitoriaInventarioCliente"
                            .ColumnMappings.Add("idUsuario", "idUsuario")
                            .ColumnMappings.Add("lineaArchivo", "lineaArchivo")
                            .ColumnMappings.Add("serial", "serial")
                            .ColumnMappings.Add("material", "material")
                            .ColumnMappings.Add("centro", "centro")
                            .ColumnMappings.Add("almacen", "almacen")
                            .ColumnMappings.Add("stock", "stock")
                            .WriteToServer(EstructuraTabla)
                        End With

                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        _estructuraTablaErrores = .EjecutarDataTable("ValidarDatosInventarioCliente", CommandType.StoredProcedure)

                        If (_estructuraTablaErrores.Rows.Count = 0) Then
                            .SqlParametros.Add("@respuesta", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                            .EjecutarNonQuery("RegistrarInventarioCliente", CommandType.StoredProcedure)

                            If Integer.TryParse(.SqlParametros("@respuesta").Value, respuesta.Valor) Then
                                If respuesta.Valor = 0 Then
                                    respuesta.EstablecerMensajeYValor(0, "Información Registrada satisfactoriamente.")
                                Else
                                    respuesta.EstablecerMensajeYValor(3, "No fué posible realizar el registro de los datos [" & respuesta.Valor.ToString() & "], por favor intente nuevamente.")
                                End If
                            Else
                                respuesta.EstablecerMensajeYValor(2, "No se pudo obtener respuesta desde el servidor de BD, por favor intente nuevamente.")
                            End If
                        Else
                            respuesta.EstablecerMensajeYValor(1, "Se encontraron registros que no cumplen las condiciones para ser registrados, por favor verifique el log.")
                        End If
                    End With
                End Using
            Catch ex As Exception
                Me.EstructuraTablaErrores = Nothing
                Throw ex
            End Try
            Return respuesta
        End Function

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

        Private Sub AdicionarColumnaAutoIncremento(dt As DataTable, Optional nombreCol As String = "lineaArchivo")
            Dim dc As DataColumn = New DataColumn(nombreCol, GetType(Long))
            With dc
                .AutoIncrement = True
                .AutoIncrementSeed = 1
                .AutoIncrementStep = 1
            End With
            dt.Columns.Add(dc)

            Dim index As Long = 1
            For Each row As DataRow In dt.Rows
                row.SetField(dc, index)
                index = index + 1
            Next
        End Sub

#End Region

    End Class

End Namespace
