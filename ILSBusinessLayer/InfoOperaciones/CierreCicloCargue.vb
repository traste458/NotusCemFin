Imports LMDataAccessLayer
Imports GemBox
Imports GemBox.Spreadsheet

Public Class CierreCicloCargue

#Region "Variables"

    Private _estructuraTabla As DataTable
    Private _estructuraTablaErrores As DataTable

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
                TablaErroresEstructura()
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

    Public Sub New(ByVal tabla As DataTable)
        MyBase.New()
        _estructuraTabla = tabla
    End Sub

#End Region

#Region "Metodos Privados"

    Private Sub EstructuraDatos()        
        Try
            Dim dt As New DataTable
            If _estructuraTabla Is Nothing Then                
                Dim factura As New DataColumn("factura", GetType(String))
                Dim serial As New DataColumn("serial", GetType(String))
                serial.Unique = True
                Dim material As New DataColumn("material", GetType(String))
                Dim region As New DataColumn("region", GetType(String))
                Dim idOrdenEnvioLectura As New DataColumn("idOrdenEnvioLectura", GetType(Long))
                Dim declaracion As New DataColumn("declaracion", GetType(String))
                Dim idOrdenNacionalizacion As New DataColumn("idOrdenNacionalizacion", GetType(Long))
                Dim pedido As New DataColumn("pedido", GetType(Long))
                Dim entrega As New DataColumn("entrega", GetType(Long))
                Dim contabilizacion As New DataColumn("contabilizacion", GetType(Long))
                Dim fechaCargue As New DataColumn("fechaCargue", GetType(DateTime))
                Dim cambioMaterial As New DataColumn("cambioMaterial", GetType(String))
                Dim programado As New DataColumn("programado", GetType(Integer))
                programado.AllowDBNull = True

                With dt.Columns
                    .Add(factura)
                    .Add(serial)
                    .Add(material)
                    .Add(region)
                    .Add(idOrdenEnvioLectura)
                    .Add(declaracion)
                    .Add(idOrdenNacionalizacion)
                    .Add(pedido)
                    .Add(entrega)
                    .Add(contabilizacion)
                    .Add(fechaCargue)
                    .Add(cambioMaterial)
                    .Add(programado)
                End With
                _estructuraTabla = dt
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try        
    End Sub

    Private Sub TablaErroresEstructura()
        Try
            Dim dt As New DataTable
            Dim id As New DataColumn("id")
            id.DataType = GetType(Integer)
            id.AllowDBNull = False
            id.AutoIncrement = True
            id.AutoIncrementSeed = 1
            id.AutoIncrementStep = 1
            Dim nombre As New DataColumn("nombre")
            nombre.DataType = GetType(String)
            nombre.AllowDBNull = False
            Dim descripcion As New DataColumn("descripcion")
            descripcion.DataType = GetType(String)
            descripcion.AllowDBNull = False
            dt.Columns.Add(id)
            dt.Columns.Add(nombre)
            dt.Columns.Add(descripcion)
            _estructuraTablaErrores = dt
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

#End Region

#Region "Metodos Publicos"

    Public Sub CargarDatos()
        Try
            CierreCicloCargue.EstablecerGemBoxLicense()
            Dim oExcel As New ExcelFile
        Catch ex As Exception
            Throw New Exception("Error al cargar la hoja de excel. " & ex.Message)
        End Try
    End Sub

    Public Sub CargarDatos(ByVal dt As DataTable)
        Dim db As New LMDataAccess
        Try
            With db
                .iniciarTransaccion()
                .inicilizarBulkCopy()
                .TiempoEsperaComando = 900
                .BulkCopy.DestinationTableName = "CierreCicloCargue"
                .BulkCopy.WriteToServer(dt)
                .confirmarTransaccion()
            End With
            
        Catch ex As Exception
            If db IsNot Nothing AndAlso db.estadoTransaccional Then db.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
    End Sub

    Public Function ValidarSeriales(ByVal idUsuario As Integer) As DataTable
        Dim db As New LMDataAccess        
        Try
            db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
            db.TiempoEsperaComando = 900
            TablaErroresEstructura()
            _estructuraTablaErrores = db.ejecutarDataTable("ValidarSerialesCierreCicloCargue", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
        Return _estructuraTablaErrores
    End Function



#End Region

#Region "Metodos Compartidos"

    Public Shared Sub EstablecerGemBoxLicense()
        SpreadsheetInfo.SetLicense("EVIF-6YOV-FYFL-M3H6")
    End Sub

    Public Shared Sub LimpiarDatosDeUsuario(ByVal idUsuario As Integer)
        Dim db As New LMDataAccess
        Try
            If idUsuario > 0 Then
                db.SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuario
                db.TiempoEsperaComando = 900
                db.ejecutarNonQuery("EliminarSerialesPrecargueCicloDeCargue", CommandType.StoredProcedure)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            If Not db Is Nothing Then db.Dispose()
        End Try
    End Sub

    Public Shared Function ObtenerValorMaxCargue() As Integer
        Dim retorno As Integer
        Try
            Dim con As New Comunes.ConfigValues("MAX_CIERRE_CICLO")
            Integer.TryParse(con.ConfigKeyValue, retorno)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retorno
    End Function

    Public Shared Function ObtenerValorMaxCargueProgramado() As Integer
        Dim retorno As Integer
        Try
            Dim con As New Comunes.ConfigValues("MAX_CIERRE_CICLO_PROGRAMADO")
            Integer.TryParse(con.ConfigKeyValue, retorno)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
        Return retorno
    End Function

#End Region

#Region "Estructuras"

#End Region


End Class
