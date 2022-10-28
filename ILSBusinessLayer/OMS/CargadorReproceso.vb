Imports LMDataAccessLayer
Imports ILSBusinessLayer

Public Class CargadorReproceso

#Region "Atributos (Campos)"

    Private _idFactura As Integer
    Private _idGuia As Integer
    Private _codigoOrden As String
    Private _idOrden As Integer
    Private _material As String
    Private _idRegion As Short
    Private _idTipoInstruccion As Short
    Private _idTipoReproceso As Byte
    Private _dtOrden As DataTable
    Private _consultado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _codigoOrden = ""
        _material = ""
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdFactura() As Integer
        Get
            Return _idFactura
        End Get
        Set(ByVal value As Integer)
            _idFactura = value
        End Set
    End Property

    Public Property IdGuia() As Integer
        Get
            Return _idGuia
        End Get
        Set(ByVal value As Integer)
            _idGuia = value
        End Set
    End Property

    Public Property CodigoOrden() As String
        Get
            Return _codigoOrden
        End Get
        Set(ByVal value As String)
            _codigoOrden = value
        End Set
    End Property

    Public Property IdOrden() As Integer
        Get
            Return _idOrden
        End Get
        Set(ByVal value As Integer)
            _idOrden = value
        End Set
    End Property

    Public Property Material() As String
        Get
            Return _material
        End Get
        Set(ByVal value As String)
            _material = value
        End Set
    End Property

    Public Property IdRegion() As Short
        Get
            Return _idRegion
        End Get
        Set(ByVal value As Short)
            _idRegion = value
        End Set
    End Property

    Public Property IdTipoInstruccion() As Short
        Get
            Return _idTipoInstruccion
        End Get
        Set(ByVal value As Short)
            _idTipoInstruccion = value
        End Set
    End Property

    Public Property IdTipoReproceso() As Byte
        Get
            Return _idTipoReproceso
        End Get
        Set(ByVal value As Byte)
            _idTipoReproceso = value
        End Set
    End Property

    Public ReadOnly Property ListaOrdenReproceso() As DataTable
        Get
            If Not _consultado Then CargarListaOrdenReproceso()
            If _dtOrden Is Nothing Then Throw New Exception("No se ha cargardo el listado de órdenes de reproceso")
            Return _dtOrden
        End Get
    End Property

#End Region

#Region "Métodos Públicos"

    Public Sub CargarListaOrdenReproceso()
        Dim dbManager As New LMDataAccess
        _dtOrden = New DataTable
        Try
            With dbManager
                If _idOrden > 0 Then .SqlParametros.Add("@idOrden", SqlDbType.Int).Value = _idOrden
                If _codigoOrden IsNot Nothing AndAlso _codigoOrden.Trim.Length > 0 Then _
                    .SqlParametros.Add("@codigoOrden", SqlDbType.VarChar, 15).Value = _codigoOrden
                If _idFactura > 0 Then .SqlParametros.Add("@idFactura", SqlDbType.Int).Value = _idFactura
                If _idGuia > 0 Then .SqlParametros.Add("@idGuia", SqlDbType.Int).Value = _idGuia
                If _material IsNot Nothing AndAlso _material.Trim.Length > 0 Then _
                    .SqlParametros.Add("@material", SqlDbType.VarChar, 7).Value = _material
                If _idRegion > 0 Then .SqlParametros.Add("@idRegion", SqlDbType.Int).Value = _idRegion
                If _idTipoInstruccion > 0 Then .SqlParametros.Add("@idTipoInstruccion", SqlDbType.Int).Value = _idTipoInstruccion
                _dtOrden = .ejecutarDataTable("ObtenerListadoOrdenReproceso", CommandType.StoredProcedure)
                _consultado = True
            End With

        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
