Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class ProductoDocumentoFinanciero

#Region "Atributos"

    Private _idProducto As Long
    Private _nombre As String
    Private _codigo As String
    Private _estado As Boolean
    Private _fechaRegistro As DateTime
    Private _idCreador As Integer
    Private _creador As String
    Private _esSerializado As Boolean

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdProducto As Long
        Get
            Return _idProducto
        End Get
        Set(value As Long)
            _idProducto = value
        End Set
    End Property

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Codigo As String
        Get
            Return _codigo
        End Get
        Set(value As String)
            _codigo = value
        End Set
    End Property

    Public Property Estado As Boolean
        Get
            Return _estado
        End Get
        Set(value As Boolean)
            _estado = value
        End Set
    End Property

    Public Property FechaRegistro As DateTime
        Get
            Return _fechaRegistro
        End Get
        Set(value As DateTime)
            _fechaRegistro = value
        End Set
    End Property

    Public Property IdCreador As Integer
        Get
            Return _idCreador
        End Get
        Set(value As Integer)
            _idCreador = value
        End Set
    End Property

    Public Property Creador As String
        Get
            Return _creador
        End Get
        Set(value As String)
            _creador = value
        End Set
    End Property

    Public Property EsSerializado As Boolean
        Get
            Return _esSerializado
        End Get
        Set(value As Boolean)
            _esSerializado = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idProducto As Long)
        MyBase.New()
        _idProducto = idProducto
        CargarDatos()
    End Sub

    Public Sub New(ByVal codigo As String)
        MyBase.New()
        _codigo = codigo
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idProducto > 0 Then .SqlParametros.Add("@listIdProducto", SqlDbType.VarChar, 2000).Value = CStr(_idProducto)
                If _codigo > 0 Then .SqlParametros.Add("@listCodigo", SqlDbType.VarChar, 2000).Value = CStr(_codigo)
                .ejecutarReader("ObtenerProductoDocumentoFinanciero", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    If .Reader.Read Then
                        CargarResultadoConsulta(.Reader)
                        _registrado = True
                    End If
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

    End Sub

#End Region

#Region "Métodos Protegidos"

    Protected Friend Sub CargarResultadoConsulta(ByVal reader As Data.Common.DbDataReader)
        If reader IsNot Nothing Then
            If reader.HasRows Then
                Long.TryParse(reader("idProducto"), _idProducto)
                If Not IsDBNull(reader("nombre")) Then _nombre = (reader("nombre").ToString)
                If Not IsDBNull(reader("codigo")) Then _codigo = (reader("codigo").ToString)
                If Not IsDBNull(reader("estado")) Then _estado = (reader("estado").ToString)
                If Not IsDBNull(reader("fechaRegistro")) Then _fechaRegistro = CDate(reader("fechaRegistro").ToString)
                Long.TryParse(reader("idCreador"), _idCreador)
                If Not IsDBNull(reader("creador")) Then _creador = (reader("creador").ToString)
                If Not IsDBNull(reader("esSerializado")) Then _esSerializado = (reader("esSerializado").ToString)
            End If
        End If
    End Sub

#End Region


End Class
