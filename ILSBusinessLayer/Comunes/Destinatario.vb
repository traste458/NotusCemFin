Imports ILSBusinessLayer.Estructuras
Public Class Destinatario

    Private _idCliente As Integer
    Private _cliente As String
    Private _estado As Short
    Private _region As Integer
    Private _idCiudad As Integer


    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal idCliente As Integer)
        MyBase.New()
        ObtenerPorId(idCliente)
    End Sub

    Public Property IdCliente() As Integer
        Get
            Return _idCliente
        End Get
        Set(ByVal value As Integer)
            _idCliente = IdCliente
        End Set
    End Property

    Public Property Cliente() As String
        Get
            Return _cliente
        End Get
        Set(ByVal value As String)
            _cliente = value
        End Set
    End Property

    Public Property Estado() As Short
        Get
            Return _estado
        End Get
        Set(ByVal value As Short)
            _estado = value
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

    Public Property Region() As String
        Get
            Return _region
        End Get
        Set(ByVal value As String)
            _region = value
        End Set
    End Property

    Public Overloads Function ObtenerListado() As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            Return dbManager.ejecutarDataTable("ObtenerInfoClientes", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dbManager.Dispose()
        End Try

    End Function
    Public Overloads Function ObtenerListado(ByVal filtro As FiltroDestinatario) As DataTable
        Dim dbManager As New LMDataAccessLayer.LMDataAccess
        Try
            With filtro
                If .IdCliente <> 0 Then dbManager.agregarParametroSQL("@idCliente", .IdCliente, SqlDbType.Int)
                If Not IsNothing(.Cliente) Then dbManager.agregarParametroSQL("@cliente", .Cliente, SqlDbType.VarChar, 70)
                If .IdCiudad <> 0 Then dbManager.agregarParametroSQL("@idCiudad", .IdCiudad, SqlDbType.Int)
                If Not IsNothing(.NombreRegion) And .NombreRegion <> "" Then dbManager.agregarParametroSQL("@nombreRegion", .NombreRegion, SqlDbType.VarChar, 10)
                If .Estado = Enumerados.EstadoBinario.NoEstablecido Then
                    .Estado = Enumerados.EstadoBinario.Activo
                End If
                dbManager.agregarParametroSQL("@estado", .Estado, SqlDbType.Int)
            End With
            Return dbManager.ejecutarDataTable("ObtenerInfoClientes", CommandType.StoredProcedure)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            dbManager.Dispose()
        End Try

    End Function
    Public Function ObtenerPorId(ByVal IdDliente As Integer) As DataTable
        Dim filtro As New FiltroDestinatario
        filtro.IdCliente = IdDliente
        Return ObtenerListado(filtro)
    End Function

    Public Function ObtenerPorEstado(ByVal estado As Short) As DataTable
        Dim filtro As New FiltroDestinatario
        filtro.Estado = estado
        Return ObtenerListado(filtro)
    End Function

End Class
