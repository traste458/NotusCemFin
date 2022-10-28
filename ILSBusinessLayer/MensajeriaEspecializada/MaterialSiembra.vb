Imports GemBox.Spreadsheet
Imports System.Web
Imports LMDataAccessLayer

Public Class MaterialSiembra

#Region "Atributos"

    Private _oExcel As ExcelFile
    Private _idMaterial As Integer
    Private _material As String
    Private _referencia As String

    Private _estructuraTabla As DataTable
    Private _estructuraTablaErrores As DataTable

#End Region

#Region "Propiedades"

    Public Property Excel As ExcelFile
        Get
            Return _oExcel
        End Get
        Set(value As ExcelFile)
            _oExcel = value
        End Set
    End Property

    Public Property IdMaterial As Integer
        Get
            Return _idMaterial
        End Get
        Set(value As Integer)
            _idMaterial = value
        End Set
    End Property

    Public Property Material As String
        Get
            Return _material
        End Get
        Set(value As String)
            _material = value
        End Set
    End Property

    Public Property Referencia As String
        Get
            Return _referencia
        End Get
        Protected Friend Set(value As String)
            _referencia = value
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

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idMaterial As Integer)
        MyBase.New()
        _idMaterial = idMaterial
    End Sub

    Public Sub New(ByVal material As String)
        MyBase.New()
        _material = material
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function Registrar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Dim wsData As ExcelWorksheet = Nothing
        Try
            Dim dtDatos As DataTable = EstructuraTabla()
            Dim idUsuario As Integer = CInt(HttpContext.Current.Session("usxp001"))

            If _oExcel IsNot Nothing Then
                wsData = _oExcel.Worksheets(0)
                ValidarArchivo(wsData)
            ElseIf Not String.IsNullOrEmpty(_material) Then
                AdicionarDato(_material)
            End If

            If EstructuraTablaErrores.Rows.Count = 0 Then
                If _oExcel IsNot Nothing Then
                    AddHandler wsData.ExtractDataEvent, AddressOf ExtractDataErrorHandler
                    wsData.ExtractToDataTable(dtDatos, wsData.Rows.Count, ExtractDataOptions.SkipEmptyRows, wsData.Rows(1), wsData.Columns(0))

                    For Each fila As DataRow In dtDatos.Select("material is NULL")
                        fila.Delete()
                    Next
                End If

                If dtDatos.Columns.Contains("idUsuario") Then dtDatos.Columns.Remove("idUsuario")
                dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(Integer), idUsuario))

                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .ejecutarNonQuery("LiberarDatosTransitoriosMaterialSiembra", CommandType.StoredProcedure)

                        .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                        .TiempoEsperaComando = 600000
                        With .BulkCopy
                            .DestinationTableName = "TransitoriaMaterialServicioSiembra"
                            .ColumnMappings.Add("material", "material")
                            .ColumnMappings.Add("idUsuario", "idUsuario")
                            .WriteToServer(dtDatos)
                        End With

                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                        .SqlParametros.Add("@retorno", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarReader("RegistrarMaterialesSiembra", CommandType.StoredProcedure)

                        If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                            While .Reader.Read()
                                AdicionarError(.Reader("id"), .Reader("nombre"), .Reader("descripcion"))
                            End While
                            If Not .Reader.IsClosed Then .Reader.Close()
                        End If

                        resultado.Valor = .SqlParametros("@retorno").Value
                        If resultado.Valor = 0 Then
                            resultado.EstablecerMensajeYValor(0, "Se realizó el registro de los materiales correctamente.")
                        Else
                            resultado.EstablecerMensajeYValor(2, "Se encontraron errores en los datos del archivo, por favor verifique el log de resultados.")
                        End If
                    End With
                End Using
            Else
                resultado.EstablecerMensajeYValor(1, "Se encontraron errores en la estructura del archivo, por favor verifique el log de resultados.")
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function

    Public Function Eliminar() As ResultadoProceso
        Dim resultado As New ResultadoProceso
        Try
            If _idMaterial > 0 Then
                Using dbManager As New LMDataAccess
                    With dbManager
                        .SqlParametros.Clear()
                        .SqlParametros.Add("@idMaterial", SqlDbType.Int).Value = _idMaterial
                        .SqlParametros.Add("@retorno", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                        .ejecutarNonQuery("EliminarMaterialSiembra", CommandType.StoredProcedure)

                        resultado.Valor = .SqlParametros("@retorno").Value
                        If resultado.Valor = 0 Then
                            resultado.EstablecerMensajeYValor(0, "Se realizó la eliminación del registro correctamente.")
                        Else
                            resultado.EstablecerMensajeYValor(resultado.Valor, "Se generó un error inesperado al eliminar el registro [" + resultado.Valor + "].")
                        End If
                    End With
                End Using
            Else
                resultado.EstablecerMensajeYValor(1, "No se proporcionaron los datos suficientes para realizar la eliminación.")
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return resultado
    End Function

#End Region

#Region "Métodos Privados"

    Private Sub EstructuraDatos()
        Try
            Dim dtDatos As New DataTable
            If _estructuraTabla Is Nothing Then
                With dtDatos.Columns
                    .Add(New DataColumn("material", GetType(String)))
                End With
                dtDatos.AcceptChanges()
                _estructuraTabla = dtDatos
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub ValidarArchivo(ByVal ws As ExcelWorksheet)
        Dim index As Integer = 1
        Try
            For Each fila As ExcelRow In ws.Rows
                If fila.AllocatedCells.Count <> Me.EstructuraTabla.Columns.Count Then
                    AdicionarError(index, "Fila inválida", "El Número de columnas de la fila es inválido.")
                ElseIf index > 1 Then
                    If Not IsNumeric(fila.Cells(0).Value) Then
                        AdicionarError(index, "Dato inválido", "El material del equipo debe ser numérico.")
                    End If
                End If
                index += 1
            Next
        Catch ex As Exception
            Throw New Exception("Se generó un error en la validación del archivo, por favor elimine las filas y columnas vacías e intente nuevamente.")
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

    Private Sub AdicionarDato(ByVal material As String)
        Try
            With EstructuraTabla
                Dim drError As DataRow = .NewRow()
                With drError
                    .Item("material") = material
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

    Private Sub ExtractDataErrorHandler(ByVal sender As Object, ByVal e As ExtractDataDelegateEventArgs)
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

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Integer.TryParse(reader("idMaterial"), _idMaterial)
                _material = reader("material").ToString()
                _referencia = reader("referencia").ToString()
            End If
        End If
    End Sub

#End Region

End Class
