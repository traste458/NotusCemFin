Imports LMDataAccessLayer
Imports System.IO

Public Class ValidarCargue

#Region "Atributos (Campos)"

    Private _nombreArchivo As String
    Private _ruta As String
    Private _idUsuario As Integer
    Private _dbManager As New LMDataAccess
    Private dtError As New DataTable

#End Region

#Region "Propiedades"

    Public Property NombreArchivo() As String
        Get
            Return _nombreArchivo
        End Get
        Set(ByVal value As String)
            _nombreArchivo = value
        End Set
    End Property

    Public Property Ruta() As String
        Get
            Return _ruta
        End Get
        Set(ByVal value As String)
            _ruta = value
        End Set
    End Property

    Public Property IdUsuario() As String
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As String)
            _idUsuario = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()

        dtError.Columns.Add(New DataColumn("Linea"))
        dtError.Columns.Add(New DataColumn("Descripción"))
        dtError.Columns.Add(New DataColumn("Serial", GetType(String)))

        _nombreArchivo = ""
        _ruta = ""
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function LeerPlano() As DataSet
        Dim dsReturn As New DataSet()
        Dim Caracterper As String = String.Empty
        Dim dtDatos As New DataTable
        Dim lectorArchivo As StreamReader = Nothing
        Dim linea As String
        Dim arregloDatos() As String
        Dim numLinea As Integer = 1

        dtDatos = ObtenerEstructuraDatos()
        If Ruta <> "" Then
            Try
                Dim flag As Boolean
                Dim mate As String = "99999999"
                lectorArchivo = File.OpenText(Ruta)
                Do While lectorArchivo.Peek >= 0
                    linea = lectorArchivo.ReadLine
                    If Not String.IsNullOrEmpty(linea) Then
                        flag = True
                        arregloDatos = linea.Split(vbTab)
                        If arregloDatos.Length = 5 Then
                            If (mate <> arregloDatos(1)) Then
                                Caracterper = CargaValidacion(arregloDatos(1))
                                mate = arregloDatos(1)
                            End If
                            If (Caracterper Is Nothing) Then
                                RegError(numLinea, "El Material: " & mate & "  no existe")
                            Else
                                Dim oExpReg As New System.Text.RegularExpressions.Regex(Caracterper)
                                If String.IsNullOrEmpty(arregloDatos(0)) Then RegError(numLinea, "Se esperaba un serial", arregloDatos(0)) : flag = False
                                If flag And Not oExpReg.IsMatch(arregloDatos(0)) Then RegError(numLinea, "El tipo de dato no es valido para el serial", arregloDatos(0)) : flag = False
                                If arregloDatos(1).Trim() = "" Then RegError(numLinea, "Se esperaba una material", arregloDatos(1)) : flag = False
                                If arregloDatos(2).Trim() = "" Then RegError(numLinea, "Se esperaba una centro", arregloDatos(2)) : flag = False
                                If arregloDatos(3).Trim() = "" Then RegError(numLinea, "Se esperaba un almacén", arregloDatos(3)) : flag = False
                                If arregloDatos(4).Trim() = "" Then RegError(numLinea, "Se esperaba un Region", arregloDatos(4)) : flag = False
                                If flag Then Me.AgregarSeriales(arregloDatos(0), arregloDatos(1), arregloDatos(2), arregloDatos(3), arregloDatos(4), numLinea, dtDatos)
                            End If
                        Else
                            RegError(numLinea, "Formato no esperado para determinar el serial a actualizar")
                        End If
                        Else
                            RegError(numLinea, "El número de linea se encuentra vacia, por favor verificar")
                        End If
                        numLinea += 1
                Loop
                If dtDatos.Rows.Count = 0 Then RegError(0, "El archivo no contiene registros válidos. Por favor verifique")
            Catch ex As Exception
                Throw New Exception("Error al tratar de leer los datos del archivo. " & ex.Message)
            Finally
                If Not lectorArchivo Is Nothing Then lectorArchivo.Close()
            End Try
        End If

        dsReturn.Tables.Add(dtDatos)
        dsReturn.Tables.Add(dtError)
        dsReturn.AcceptChanges()

        Return dsReturn
    End Function

    Public Shared Function CargarPlano(ByVal dtDatos As DataTable, ByVal idUsuario As Integer) As Boolean
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As DataTable
        dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        Try
            db.agregarParametroSQL("@tabla", 5, SqlDbType.BigInt)
            db.ejecutarNonQuery("BorrarTablasAuxiliares", CommandType.StoredProcedure)
            db.inicilizarBulkCopy()
            db.BulkCopy.DestinationTableName = "SerialesTransitoriosCEM"
            db.BulkCopy.WriteToServer(dtDatos)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try

    End Function
    Public Function CargaValidacion(ByVal Material As String) As String
        Dim _caracteresPermitidos As String = String.Empty
        Using db As New LMDataAccessLayer.LMDataAccess
            With db
                .SqlParametros.Clear()
                .SqlParametros.Add("@Material", SqlDbType.VarChar).Value = Material
                .ejecutarReader("ObtenerInfoConfiguracionLecturaSerialMaterial", CommandType.StoredProcedure)
                If .Reader.Read Then
                    _caracteresPermitidos = .Reader("caracterPermitido").ToString
                End If
            End With
        End Using
        Return _caracteresPermitidos
    End Function
    Public Function CargarInventarioArchivo(ByRef dtError As DataTable) As ResultadoProceso
        Dim resultado As New ResultadoProceso
        If IdUsuario > 0 Then
            If _dbManager Is Nothing Then _dbManager = New LMDataAccess
            With _dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                dtError = .ejecutarDataTable("IngresarInventarioCEMporArchivoSerial", CommandType.StoredProcedure)
                If dtError Is Nothing OrElse dtError.Rows.Count = 0 Then
                    resultado.EstablecerMensajeYValor(0, "Información cargada correctamente")
                Else
                    resultado.EstablecerMensajeYValor(2, "No se pudo realizar el cargue de la información. El archivo contenía registros erroneos")
                End If
            End With
        Else
            resultado.EstablecerMensajeYValor(1, "No se han proporcionado los datos necesarios para cargar el inventario.")
        End If
        Return resultado
    End Function

#End Region

#Region "Métodos Privados"

    Private Sub RegError(ByVal linea As Integer, ByVal descripcion As String, Optional ByVal serial As String = "")
        Dim dr As DataRow = dtError.NewRow()
        dr("Linea") = linea
        dr("Serial") = serial
        dr("Descripción") = descripcion
        dtError.Rows.Add(dr)
        dtError.AcceptChanges()
    End Sub

    Private Sub AgregarSeriales(ByVal serial As String, ByVal material As String, ByVal centro As String, _
                                ByVal almacen As String, ByVal region As String, ByVal lineaArchivo As Integer, ByVal dtDatos As DataTable)
        Dim dr As DataRow = dtDatos.NewRow
        dr("serial") = serial
        dr("material") = material
        dr("centro") = centro
        dr("almacen") = almacen
        dr("region") = region
        dr("lineaArchivo") = lineaArchivo
        dtDatos.Rows.Add(dr)
    End Sub

    Public Function ObtenerEstructuraDatos() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("serial", GetType(String)))
        dt.Columns.Add(New DataColumn("material", GetType(String)))
        dt.Columns.Add(New DataColumn("centro", GetType(String)))
        dt.Columns.Add(New DataColumn("almacen", GetType(String)))
        dt.Columns.Add(New DataColumn("region", GetType(String)))
        dt.Columns.Add(New DataColumn("lineaArchivo"))
        Return dt
    End Function

#End Region

End Class
