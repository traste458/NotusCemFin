Imports System.Web
Imports GemBox.Spreadsheet
Imports LMDataAccessLayer
Imports System.IO

Public Class CruzeFacturaDeclaracion

#Region "Atributos"

    Private _estructuraTablaBase As DataTable
    Private _estructuraTablaConsulta As DataTable
    Private _estructuraTablaSerial As DataTable
    Private _facturas As ArrayList
    Private _idUsuario As Integer
    Private _declaracion As String

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

    Public Property Facturas() As ArrayList
        Get
            Return _facturas
        End Get
        Set(value As ArrayList)
            _facturas = value
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
                    dtResultado = .ejecutarDataTable("ConsultarSerialPorDeclaracion", CommandType.StoredProcedure)
                End With
            End Using
        Catch ex As Exception
            Throw ex
        End Try
        Return dtResultado
    End Function

    Sub ConsultarInformacionFacturaDeclaracion()
        Dim dsResultado As New DataSet
        Try
            Using dbManager As New LMDataAccess
                With dbManager
                    .SqlParametros.Clear()
                    If _facturas IsNot Nothing AndAlso _facturas.Count > 0 Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = Join(_facturas.ToArray, ",")
                    .SqlParametros.Add("@resultado", SqlDbType.Int).Direction = ParameterDirection.ReturnValue
                    dsResultado = .EjecutarDataSet("ConsultarInformacionFacturaDeclaracion", CommandType.StoredProcedure)
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

#End Region



End Class
