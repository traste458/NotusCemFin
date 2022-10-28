Imports LMDataAccessLayer

Public Class CargarInventarioCEM

#Region "Atributos (Campos)"

    Private _numeroEntrega As Double
    Private _idUsuario As Integer
    Private _idBodega As Integer
    Private _idDespacho As String
    Private _cantidad As Integer
    Private _cantidadLeida As Integer
    Private _serial As String
    Private _dbManager As New LMDataAccess

#End Region

#Region "Propiedades"

    Public Property NumeroEntrega() As Double
        Get
            Return _numeroEntrega
        End Get
        Set(ByVal value As Double)
            _numeroEntrega = value
        End Set
    End Property

    Public Property IdUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property IdBodega() As Integer
        Get
            Return _idBodega
        End Get
        Set(ByVal value As Integer)
            _idBodega = value
        End Set
    End Property

    Public Property Cantidad() As Integer
        Get
            Return _cantidad
        End Get
        Set(ByVal value As Integer)
            _cantidad = value
        End Set
    End Property

    Public Property CantidadLeida() As Integer
        Get
            Return _cantidadLeida
        End Get
        Set(ByVal value As Integer)
            _cantidadLeida = value
        End Set
    End Property

    Public Property idDespacho() As Integer
        Get
            Return _idDespacho
        End Get
        Set(ByVal value As Integer)
            _idDespacho = value
        End Set
    End Property

    Public Property Serial() As String
        Get
            Return _serial
        End Get
        Set(ByVal value As String)
            _serial = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Públicos"

    Public Function CargarDatos() As DataTable
        Dim dtDatos As New DataTable
        If _dbManager IsNot Nothing Then _dbManager = New LMDataAccess
        Try
            With _dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@numeroEntrega", SqlDbType.BigInt).Value = NumeroEntrega
                End With
                dtDatos = .ejecutarDataTable("ObtenerSerialesEntregaCEM", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

    Public Function CargarInventario() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If IdBodega > 0 AndAlso idDespacho > 0 Then
            If _dbManager Is Nothing Then _dbManager = New LMDataAccess
            With _dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@entrega", SqlDbType.BigInt).Value = NumeroEntrega
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@idBodega", SqlDbType.Int).Value = IdBodega
                    .Add("@idDespacho", SqlDbType.Int).Value = idDespacho
                End With
                .ejecutarReader("IngresarInventarioCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    While .Reader.Read
                        lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                    End While
                    .Reader.Close()
                End If
            End With
        Else
            lstResultado.Add(New ResultadoProceso(1, "No se han proporcionado los datos necesarios para cargar el inventario."))
        End If
        Return lstResultado
    End Function

    Public Function CargarInventarioArchivo() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If IdUsuario > 0 Then
            If _dbManager Is Nothing Then _dbManager = New LMDataAccess
            With _dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                End With
                .ejecutarReader("IngresarInventarioCEMporArchivo", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    While .Reader.Read
                        lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                    End While
                    .Reader.Close()
                End If
            End With
        Else
            lstResultado.Add(New ResultadoProceso(1, "No se han proporcionado los datos necesarios para cargar el inventario."))
        End If
        Return lstResultado
    End Function

    Public Function CargarInventarioLectura() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If IdUsuario >= 0 Then
            If _dbManager Is Nothing Then _dbManager = New LMDataAccess
            With _dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                    .Add("@entrega", SqlDbType.Int).Value = NumeroEntrega
                End With
                .ejecutarReader("IngresarInventarioCEMporLectura", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    While .Reader.Read
                        lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                    End While
                    .Reader.Close()
                End If
            End With
        Else
            lstResultado.Add(New ResultadoProceso(1, "No se han proporcionado los datos necesarios para cargar el inventario."))
        End If
        Return lstResultado
    End Function

    Public Function RemisionCargada() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            With .SqlParametros
                .Clear()
                .Add("@entrega", SqlDbType.BigInt).Value = NumeroEntrega
            End With
            .ejecutarReader("ValidarRemision", CommandType.StoredProcedure)
            If .Reader IsNot Nothing Then
                While .Reader.Read
                    lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                End While
                .Reader.Close()
            End If
        End With
        Return lstResultado
    End Function

    Public Shared Function ObtenerEstructuraDatos() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("entrega", GetType(Long)))
        dt.Columns.Add(New DataColumn("serial", GetType(Long)))
        dt.Columns.Add(New DataColumn("material", GetType(Long)))
        dt.Columns.Add(New DataColumn("lineaArchivo"))
        dt.Columns.Add(New DataColumn("codigoCAC", GetType(String)))
        Return dt
    End Function

    Public Shared Function CargarZMMAK(ByVal dtDatos As DataTable, ByVal idUsuario As Integer) As Boolean
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As DataTable
        dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        Try
            db.agregarParametroSQL("@tabla", 1, SqlDbType.BigInt)
            db.ejecutarNonQuery("BorrarTablasAuxiliares", CommandType.StoredProcedure)
            db.inicilizarBulkCopy()
            db.BulkCopy.DestinationTableName = "ArchivoZmmak"
            db.BulkCopy.WriteToServer(dtDatos)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try

    End Function

    Public Shared Function ObtenerEstructuraDatosZmma1() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("entrega", GetType(Long)))
        dt.Columns.Add(New DataColumn("centro", GetType(String)))
        dt.Columns.Add(New DataColumn("almacen", GetType(String)))
        dt.Columns.Add(New DataColumn("material", GetType(String)))
        dt.Columns.Add(New DataColumn("cantidad", GetType(Long)))
        dt.Columns.Add(New DataColumn("lineaArchivo"))
        Return dt
    End Function

    Public Shared Function CargarZMMA1(ByVal dtDatos As DataTable, ByVal idUsuario As Integer) As Boolean
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As DataTable
        dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        Try
            db.agregarParametroSQL("@tabla", 2, SqlDbType.BigInt)
            db.ejecutarNonQuery("BorrarTablasAuxiliares", CommandType.StoredProcedure)
            db.inicilizarBulkCopy()
            db.BulkCopy.DestinationTableName = "ArchivoZmma1"
            db.BulkCopy.WriteToServer(dtDatos)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try

    End Function

    Public Shared Function ObtenerEstructuraDatosLectura() As DataTable
        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("entrega", GetType(Long)))
        dt.Columns.Add(New DataColumn("serial", GetType(String)))
        dt.Columns.Add(New DataColumn("material", GetType(String)))
        dt.Columns.Add(New DataColumn("lineaArchivo"))
        Return dt
    End Function

    Public Shared Function CargarLecturaCEM(ByVal dtDatos As DataTable, ByVal idUsuario As Integer) As Boolean
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtErrores As DataTable
        dtDatos.Columns.Add(New DataColumn("idUsuario", GetType(System.Int64), idUsuario))
        Try
            db.agregarParametroSQL("@tabla", 4, SqlDbType.BigInt)
            db.ejecutarNonQuery("BorrarTablasAuxiliares", CommandType.StoredProcedure)
            db.inicilizarBulkCopy()
            db.BulkCopy.DestinationTableName = "LecturaInventarioCEM"
            db.BulkCopy.WriteToServer(dtDatos)
        Catch ex As Exception
            Throw New Exception(ex.Message, ex)
        End Try

    End Function

    Public Function CrearOrdenRecepcion() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            With .SqlParametros
                .Clear()
                .Add("@entrega", SqlDbType.BigInt).Value = NumeroEntrega
                .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
                .Add("@cantidad", SqlDbType.Int).Value = Cantidad
            End With
            .ejecutarReader("CrearOrdenRecepcionCEM", CommandType.StoredProcedure)
            If .Reader IsNot Nothing Then
                While .Reader.Read
                    lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                End While
                .Reader.Close()
            End If
        End With
        Return lstResultado
    End Function

    Public Function CargarEntrega() As List(Of ResultadoProceso)
        Dim resultado As New List(Of ResultadoProceso)
        If NumeroEntrega > 0 Then
            If _dbManager Is Nothing Then _dbManager = New LMDataAccess
            With _dbManager
                With .SqlParametros
                    .Clear()
                    .Add("@entrega", SqlDbType.BigInt).Value = NumeroEntrega
                End With
                .ejecutarReader("ObtenerInformacionEntregaCEM", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        _cantidadLeida = .Reader("cantidadLeida").ToString()
                        _cantidad = .Reader("cantidad").ToString()
                    End If
                    .Reader.Close()
                End If
            End With
        Else
            resultado.Add(New ResultadoProceso(1, "No se ha establecido el identificador de la entrega a cargar"))
        End If
        Return resultado
    End Function

    Public Function RegistrarSerial() As List(Of ResultadoProceso)
        Dim lstResultado As New List(Of ResultadoProceso)
        If _dbManager Is Nothing Then _dbManager = New LMDataAccess
        With _dbManager
            With .SqlParametros
                .Clear()
                .Add("@entrega", SqlDbType.BigInt).Value = NumeroEntrega
                .Add("@serial", SqlDbType.VarChar, 20).Value = Serial
                .Add("@idUsuario", SqlDbType.Int).Value = IdUsuario
            End With
            .ejecutarReader("RegistrarSerialCEM", CommandType.StoredProcedure)
            If .Reader IsNot Nothing Then
                While .Reader.Read
                    lstResultado.Add(New ResultadoProceso(.Reader("valor").ToString, .Reader("mensaje").ToString))
                End While
                .Reader.Close()
            End If
        End With
        Return lstResultado
    End Function

    Public Shared Function DescargarSerialesLeidos(ByVal numeroEntrega As String) As DataTable
        Dim dtDatos As New DataTable
        Dim _dbManager As New LMDataAccess
        Try
            With _dbManager
                .SqlParametros.Add("@entrega", SqlDbType.BigInt).Value = NumeroEntrega
                dtDatos = .ejecutarDataTable("ObtenerSerialesLeidosParaCargueCEM", CommandType.StoredProcedure)
            End With
        Finally
            If _dbManager IsNot Nothing Then _dbManager.Dispose()
        End Try
        Return dtDatos
    End Function

#End Region

End Class
