Imports LMDataAccessLayer
Imports ILSBusinessLayer.Estructuras

Public Class Region

#Region "Variables"

    Private _idRegion As Integer
    Private _codigo As String
    Private _nombreRegion As String
    Private _activo As Boolean
    Private _posicionOrdinal As Integer
    Private _centro As String
    Private _almacen As String
    Private _esRegion As Boolean
    Private _error As String

#End Region

#Region "Propiedades"
    Public Property Almacen() As String
        Get
            Return _almacen
        End Get
        Set(ByVal value As String)
            _almacen = value
        End Set
    End Property
    Public Property Centro() As String
        Get
            Return _centro
        End Get
        Set(ByVal value As String)
            _centro = value
        End Set
    End Property
    Public Property PosicionOrdinal() As Integer
        Get
            Return _posicionOrdinal
        End Get
        Set(ByVal value As Integer)
            _posicionOrdinal = value
        End Set
    End Property
    Public Property Activo() As Boolean
        Get
            Return _activo
        End Get
        Set(ByVal value As Boolean)
            _activo = value
        End Set
    End Property
    Public Property NombreRegion() As String
        Get
            Return _nombreRegion
        End Get
        Set(ByVal value As String)
            _nombreRegion = value
        End Set
    End Property
    Public Property Codigo() As String
        Get
            Return _codigo
        End Get
        Set(ByVal value As String)
            _codigo = value
        End Set
    End Property
    Public ReadOnly Property IdRegion() As Integer
        Get
            Return _idRegion
        End Get

    End Property

    Public Property EsRegion() As Boolean
        Get
            Return _esRegion
        End Get
        Set(ByVal value As Boolean)
            _esRegion = value
        End Set
    End Property

    Public ReadOnly Property InfoError() As String
        Get
            Return _error
        End Get
    End Property


#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idRegion As Integer)
        CargarDatos(idRegion)
    End Sub

    Public Sub New(ByVal codRegion As String)
        CargarDatos(codRegion)
    End Sub

#End Region

#Region "Metodos Privados"

    Private Overloads Sub CargarDatos(ByVal idRegion As Integer)
        Dim db As New LMDataAccess
        Try
            db.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = idRegion
            db.ejecutarReader("ObtenerRegiones", CommandType.StoredProcedure)
            If db.Reader.Read Then
                _idRegion = idRegion
                _nombreRegion = db.Reader("nombreRegion").ToString()
                _codigo = db.Reader("codigo").ToString()
                _activo = CBool(db.Reader("activo"))
                _posicionOrdinal = CInt(db.Reader("posicionOrdinal"))
                _centro = db.Reader("centro").ToString()
                _almacen = db.Reader("almacen").ToString()
                _esRegion = CBool(db.Reader("esRegion"))
            End If
            Me._error = String.Empty
        Catch ex As Exception
            _error = "Error al cargar los datos. " & ex.Message
        Finally
            db.Dispose()
        End Try
    End Sub

    Private Overloads Sub CargarDatos(ByVal codRegion As String)
        Dim db As New LMDataAccess
        Try
            db.SqlParametros.Add("@codigo", SqlDbType.VarChar, 5).Value = codRegion
            db.ejecutarReader("ObtenerRegiones", CommandType.StoredProcedure)
            If db.Reader.Read Then
                _idRegion = idRegion
                _nombreRegion = db.Reader("nombreRegion").ToString()
                _codigo = db.Reader("codigo").ToString()
                _activo = CBool(db.Reader("activo"))
                _posicionOrdinal = CInt(db.Reader("posicionOrdinal"))
                _centro = db.Reader("centro").ToString()
                _almacen = db.Reader("almacen").ToString()
                _esRegion = CBool(db.Reader("esRegion"))
            End If
            Me._error = String.Empty
        Catch ex As Exception
            _error = "Error al cargar los datos. " & ex.Message
        Finally
            If db.Reader IsNot Nothing Then db.Reader.Close()
            db.Dispose()
        End Try
    End Sub

#End Region

#Region "Metodos"
    Public Shared Function ObtenerTodas() As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Return db.ejecutarDataTable("ObtenerRegiones")
    End Function

    Public Shared Function ObtenerTodas(ByVal filtro As Estructuras.FiltroRegion) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtDatos As New DataTable
        With filtro
            If .idRegion > 0 Then db.SqlParametros.Add("@idRegion", SqlDbType.Int).Value = .idRegion
            If .codigo <> String.Empty Then db.SqlParametros.Add("@codigo", SqlDbType.VarChar).Value = .codigo
            If .centro <> String.Empty Then db.SqlParametros.Add("@centro", SqlDbType.VarChar).Value = .centro
            If .almacen <> String.Empty Then db.SqlParametros.Add("@almacen", SqlDbType.VarChar).Value = .almacen
        End With
        dtDatos = db.ejecutarDataTable("ObtenerRegiones", CommandType.StoredProcedure)
        Return dtDatos
    End Function

    Public Shared Function ObtenerRegionesMaterial(ByVal material As String) As List(Of Region)
        Dim db As New LMDataAccessLayer.LMDataAccess
        db.agregarParametroSQL("material", material, SqlDbType.VarChar, 20)
        Dim listaregiones As New List(Of Region)
        Dim obRegion As Region
        Try
            db.ejecutarReader("ObtenerRegionesMaterial", CommandType.StoredProcedure)
            While db.Reader.Read
                obRegion = New Region
                With obRegion
                    ._idRegion = db.Reader("idRegion")
                    ._codigo = db.Reader("codigo")
                    ._nombreRegion = db.Reader("nombreRegion")
                    ._esRegion = db.Reader("esRegion")
                End With
                listaregiones.Add(obRegion)
            End While
            Return listaregiones
        Finally
            If Not db.Reader.IsClosed Then db.Reader.Close()
            db.cerrarConexion()
            db.Dispose()
        End Try
    End Function

    Public Shared Function ValidarSerialesDesdeOTB(ByVal idRegion As Integer, ByVal idOTB As Long) As DataTable
        Dim db As New LMDataAccessLayer.LMDataAccess
        Dim dtResultado As New DataTable()
        Try
            With db
                .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = idRegion
                .SqlParametros.Add("@idOTB", SqlDbType.BigInt).Value = idOTB
                dtResultado = .ejecutarDataTable("ValidaSerialesCambioRegionDesdeOTB", CommandType.StoredProcedure)
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            db.Dispose()
        End Try
        Return dtResultado
    End Function

    Public Shared Sub CambiarSerialesRegionDesdeOTB(ByVal idRegion As Integer, ByVal idOTB As Long)
        Dim db As New LMDataAccessLayer.LMDataAccess
        Try
            With db
                .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = idRegion
                .SqlParametros.Add("@idOTB", SqlDbType.BigInt).Value = idOTB
                .iniciarTransaccion()
                .ejecutarDataTable("CambiarSerialesRegionDesdeOTB", CommandType.StoredProcedure)
                .abortarTransaccion()
            End With
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            db.Dispose()
        End Try
    End Sub


#End Region

End Class


