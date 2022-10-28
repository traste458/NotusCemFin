Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports GemBox.Spreadsheet
Imports System.IO
Imports System.Drawing

Public Class DetalleMaterialesEntrega

#Region "Atributos (Campos)"

    Private _numeroEntrega As String
    Private _dtDetalle As DataTable
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        _numeroEntrega = Nothing
        _cargado = False
    End Sub

    Public Sub New(ByVal numeroEntrega As String)
        _numeroEntrega = numeroEntrega
        _cargado = False
    End Sub

#End Region

#Region "Propiedades"

    Public Property NumeroEntrega() As String
        Get
            Return _numeroEntrega
        End Get
        Set(ByVal value As String)
            _numeroEntrega = value
        End Set
    End Property

    Public ReadOnly Property Detalle() As DataTable
        Get
            If _dtDetalle Is Nothing Then CargarDatos()
            Return _dtDetalle
        End Get
    End Property

#End Region

#Region "Métodos Públicos"

    Public Sub CargarDatos()
        Dim dbManager As LMDataAccess = Nothing
        _dtDetalle = New DataTable

        Try
            dbManager = New LMDataAccess
            With dbManager
                If Not String.IsNullOrEmpty(_numeroEntrega) Then _
                    .SqlParametros.Add("@idServicio", SqlDbType.VarChar, 30).Value = _numeroEntrega.Trim
                _dtDetalle = .EjecutarDataTable("ObtenerDetalleMaterialesEntrega", CommandType.StoredProcedure)
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
