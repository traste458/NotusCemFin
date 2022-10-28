Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class ProductoComercial

#Region "Atributos"

    Private _idProductoComercial As Integer
    Private _idClienteExterno As Integer
    Private _clienteExterno As String
    Private _idProductoExterno As Integer
    Private _productoExterno As String
    Private _codigo As String

    Private _registrado As Boolean

#End Region

#Region "Propiedades"

    Public Property IdProductoComercial As Integer
        Get
            Return _idProductoComercial
        End Get
        Set(value As Integer)
            _idProductoComercial = value
        End Set
    End Property

    Public Property IdClienteExterno As Integer
        Get
            Return _idClienteExterno
        End Get
        Set(value As Integer)
            _idClienteExterno = value
        End Set
    End Property

    Public Property ClienteExterno As String
        Get
            Return _clienteExterno
        End Get
        Set(value As String)
            _clienteExterno = value
        End Set
    End Property

    Public Property IdProductoExterno As Integer
        Get
            Return _idProductoExterno
        End Get
        Set(value As Integer)
            _idProductoExterno = value
        End Set
    End Property

    Public Property ProductoExterno As String
        Get
            Return _productoExterno
        End Get
        Set(value As String)
            _productoExterno = value
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

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idProductoComercial As Integer)
        MyBase.New()
        _idProductoComercial = idProductoComercial
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If _idProductoComercial > 0 Then .SqlParametros.Add("@listIdProductoComercial", SqlDbType.VarChar, 2000).Value = CStr(_idProductoComercial)
                .ejecutarReader("ObtenerProductoComercial", CommandType.StoredProcedure)
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
                Integer.TryParse(reader("idProductoComercial"), _idProductoComercial)
                Integer.TryParse(reader("idClienteExterno"), _idClienteExterno)
                Integer.TryParse(reader("idProductoExterno"), _idProductoExterno)
                If Not IsDBNull(reader("clienteExterno")) Then _clienteExterno = (reader("clienteExterno").ToString)
                If Not IsDBNull(reader("productoExterno")) Then _productoExterno = (reader("productoExterno").ToString)
                If Not IsDBNull(reader("codigo")) Then _codigo = (reader("codigo").ToString)
            End If
        End If
    End Sub

#End Region

End Class


