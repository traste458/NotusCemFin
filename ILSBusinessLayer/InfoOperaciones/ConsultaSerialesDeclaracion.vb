Imports System.Web
Imports GemBox.Spreadsheet
Imports LMDataAccessLayer
Imports System.IO

Public Class ConsultaSerialesDeclaracion

#Region "Atributos."

    Private _estructuraTablaBase As DataTable
    Private _estructuraTablaErrores As DataTable
    Private _estructuraTablaConsulta As DataTable
    Private _estructuraTablaSerial As DataTable
    Private _Ruta As String
    Private _extension As String
    Private _idUsuario As Integer
    Private _declaracion As String
    Private _facturas As ArrayList

#End Region

#Region "Propiedades"

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

    Public Property EstructuraTablaConsulta() As DataTable
        Get
            Return _estructuraTablaConsulta
        End Get
        Set(value As DataTable)
            _estructuraTablaConsulta = value
        End Set
    End Property

    Public Property EstructuraTablaSerial() As DataTable
        Get
            Return _estructuraTablaSerial
        End Get
        Set(value As DataTable)
            _estructuraTablaSerial = value
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

    Public Property Extencion() As String
        Get
            Return _extension
        End Get
        Set(value As String)
            _extension = value
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

    Public Property Facturas() As ArrayList
        Get
            Return _facturas
        End Get
        Set(value As ArrayList)
            _facturas = value
        End Set
    End Property

#End Region

#Region "Constructores"

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

    Private Sub AgregarSeriales(ByVal serial As String)
        If _estructuraTablaBase Is Nothing Then
            EstructuraDatosBase()
        End If
        Dim dr As DataRow = _estructuraTablaBase.NewRow
        dr("serial") = serial
        _estructuraTablaBase.Rows.Add(dr)
    End Sub

#End Region

#Region "Métodos Públicos"

    Function LeerPlano() As Boolean
        Dim _esValido As Boolean
        Try
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
                        If arregloDatos.Length = 1 Then
                            dato = arregloDatos(0)
                            If dato.ToString.Length <> 15 And dato.ToString.Length <> 17 Then
                                AdicionarError(numLinea, "Fila inválida", "El serial " & dato & " no cumple con la cantidad de caracteres validos (15 Serial ó 17 Sim.)")
                            End If
                            If Not _error Then Me.AgregarSeriales(arregloDatos(0))
                        Else
                            AdicionarError(numLinea, "Fila inválida", "El Número de columnas de la fila es inválido.")
                        End If
                    Else
                        AdicionarError(numLinea, "Fila inválida", "El número de linea se encuentra vacia, por favor verificar")
                    End If
                    numLinea += 1
                Loop

                If _estructuraTablaBase Is Nothing And EstructuraTablaErrores.Rows.Count = 0 Then
                    AdicionarError(0, "Datos Invalidos", "El archivo no contiene registros. Por favor verifique")
                ElseIf Not (EstructuraTablaErrores.Rows.Count > 0) Then
                    If _estructuraTablaBase.Rows.Count = 0 Then
                        AdicionarError(0, "Datos Invalidos", "El archivo no contiene registros válidos. Por favor verifique")
                    End If
                End If
                _esValido = Not (EstructuraTablaErrores.Rows.Count > 0)
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return _esValido
    End Function

    Public Sub ConsultarInformacionSerial()
        Dim dsResultado As New DataSet
        Try

            If EstructuraTablaBase.Columns.Contains("idUsuario") Then EstructuraTablaBase.Columns.Remove("idUsuario")
            EstructuraTablaBase.Columns.Add(New DataColumn("idUsuario", GetType(Integer), _idUsuario))

            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 0
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaConsultaSerialDeclaracion"
                        .ColumnMappings.Add("serial", "serial")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(EstructuraTablaBase)
                    End With
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    dsResultado = .EjecutarDataSet("ConsultarInformacionSerialDeclaracion", CommandType.StoredProcedure)
                    If dsResultado.Tables.Count > 0 Then
                        _estructuraTablaConsulta = dsResultado.Tables(0)
                        _estructuraTablaSerial = dsResultado.Tables(1)
                    End If
                    dsResultado.Dispose()
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Function ConsultarSerialesPorDeclaracion() As DataTable
        Dim dtResultado As New DataTable
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    If _facturas IsNot Nothing AndAlso _facturas.Count > 0 Then .SqlParametros.Add("@facturas", SqlDbType.VarChar).Value = Join(_facturas.ToArray, ",")
                    dtResultado = .ejecutarDataTable("ConsultarSerialPorDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dtResultado
    End Function

    Function ConsultarSoportePorDeclaracion() As DataTable
        Dim dtSoporte As New DataTable
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
                    dtSoporte = .ejecutarDataTable("ConsultarSoportePorDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dtSoporte
    End Function

    Function ConsultarDetalleSerialDeclaracion() As DataTable
        Dim dtResultado As New DataTable
        Try

            If EstructuraTablaBase.Columns.Contains("idUsuario") Then EstructuraTablaBase.Columns.Remove("idUsuario")
            EstructuraTablaBase.Columns.Add(New DataColumn("idUsuario", GetType(Integer), _idUsuario))

            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    .inicilizarBulkCopy(SqlClient.SqlBulkCopyOptions.UseInternalTransaction)
                    .TiempoEsperaComando = 0
                    With .BulkCopy
                        .DestinationTableName = "TransitoriaConsultaSerialDeclaracion"
                        .ColumnMappings.Add("serial", "serial")
                        .ColumnMappings.Add("idUsuario", "idUsuario")
                        .WriteToServer(EstructuraTablaBase)
                    End With
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    dtResultado = .ejecutarDataTable("ConsultarDetalleSerialDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dtResultado
    End Function

#End Region

End Class
