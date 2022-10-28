Imports ILSBusinessLayer.Estructuras
Public Class Solicitante

    Private _idSolicitante As Integer
    Private _nombre As String
    Private _apellido As String
    Private _identificacion As String
    Private _idCiudad As Integer
    Private _ciudad As String
    Private _telefono As String
    Private _direccion As String
    Private _email As String
    Private _idEstado As Short
    Private _estado As String

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal idSolicitante As Integer)
        MyBase.New()
        ObtenerPorId(idSolicitante)
    End Sub

#End Region
    
#Region "Propiedades"


    Public Property IdSolicitante() As Integer
        Get
            Return _idSolicitante
        End Get
        Set(ByVal value As Integer)
            _idSolicitante = value
        End Set
    End Property

    Public Property Nombre() As String
        Get
            Return _nombre
        End Get
        Set(ByVal value As String)
            _nombre = value
        End Set
    End Property

    Public Property Apellido() As String
        Get
            Return _apellido
        End Get
        Set(ByVal value As String)
            _apellido = value
        End Set
    End Property

    Public Property Identificacion() As String
        Get
            Return _identificacion
        End Get
        Set(ByVal value As String)
            _identificacion = value
        End Set
    End Property

    Public Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property Telefono() As String
        Get
            Return _telefono
        End Get
        Set(ByVal value As String)
            _telefono = value
        End Set
    End Property

    Public Property Direccion() As String
        Get
            Return _direccion
        End Get
        Set(ByVal value As String)
            _direccion = value
        End Set
    End Property

    Public Property Email() As String
        Get
            Return _email
        End Get
        Set(ByVal value As String)
            _email = value
        End Set
    End Property

    Public Property IdEstado() As Short
        Get
            Return _idEstado
        End Get
        Set(ByVal value As Short)
            _idEstado = value
        End Set
    End Property

    Public ReadOnly Property Estado() As String
        Get
            Return _estado
        End Get
    End Property

#End Region

#Region "Metodos Privados"

    Public Overloads Function ObtenerListado() As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            Return dbManager.ejecutarDataTable("ObtenerInformacionSolicitante", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dbManager.Dispose()
        End Try

    End Function
    Public Overloads Function ObtenerListado(ByVal filtro As FiltroSolicitante) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            With filtro
                If .Idsolicitante <> 0 Then dbManager.agregarParametroSQL("@idSolicitante", .Idsolicitante, SqlDbType.Int)
                If .IdEstado = Enumerados.EstadoBinario.NoEstablecido Then
                    .IdEstado = Enumerados.EstadoBinario.Activo
                End If
                dbManager.agregarParametroSQL("@idEstado", .IdEstado, SqlDbType.Int)
            End With
            Return dbManager.ejecutarDataTable("ObtenerInformacionSolicitante", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dbManager.Dispose()
        End Try

    End Function
    Public Function ObtenerPorId(ByVal idSolicitante As Integer) As DataTable
        Dim filtro As New FiltroSolicitante
        filtro.Idsolicitante = idSolicitante
        Return ObtenerListado(filtro)
    End Function

    Public Function ObtenerPorEstado(ByVal idEstado As Short) As DataTable
        Dim filtro As New FiltroSolicitante
        filtro.IdEstado = idEstado
        Return ObtenerListado(filtro)
    End Function

#End Region

#Region "Metodos Publicos"

#End Region

#Region "Metodos Compartidos"

#End Region


    
End Class
