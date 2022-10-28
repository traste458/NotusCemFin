Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.Enumerados

Public Class ProductoDocumentoFinancieroColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _listIdProducto As List(Of Long)
    Private _listCodigo As List(Of String)
    Private _idCampania As Integer
    Private _esProductoExterno As List(Of Integer)

    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As ProductoDocumentoFinanciero
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As ProductoDocumentoFinanciero)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property ListIdProducto As List(Of Long)
        Get
            If _listIdProducto Is Nothing Then _listIdProducto = New List(Of Long)
            Return _listIdProducto
        End Get
        Set(value As List(Of Long))
            _listIdProducto = value
        End Set
    End Property

    Public Property ListCodigo As List(Of String)
        Get
            If _listCodigo Is Nothing Then _listCodigo = New List(Of String)
            Return _listCodigo
        End Get
        Set(value As List(Of String))
            _listCodigo = value
        End Set
    End Property

    Public Property IdCampania As Integer
        Get
            Return _idCampania
        End Get
        Set(value As Integer)
            _idCampania = value
        End Set
    End Property

    Public Property EsProductoExterno As List(Of Integer)
        Get
            If _esProductoExterno Is Nothing Then _esProductoExterno = New List(Of Integer)
            Return _esProductoExterno
        End Get
        Set(value As List(Of Integer))
            _esProductoExterno = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal idCampania As Integer)
        MyBase.new()
        _idCampania = idCampania
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objGestionDeVenta As Type = GetType(ProductoDocumentoFinanciero)
        Dim pInfo As PropertyInfo

        For Each pInfo In objGestionDeVenta.GetProperties
            If pInfo.PropertyType.Namespace = "System" Then
                With dtAux
                    .Columns.Add(pInfo.Name, pInfo.PropertyType)
                End With
            End If
        Next
        Return dtAux
    End Function

#End Region

#Region "Métodos Públicos"

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ProductoDocumentoFinanciero)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As ProductoDocumentoFinanciero)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As ProductoDocumentoFinanciero)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As ProductoDocumentoFinanciero

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), ProductoDocumentoFinanciero)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(ProductoDocumentoFinanciero).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        If _cargado Then Me.InnerList.Clear()
        With dbManager
            If _listIdProducto IsNot Nothing AndAlso _listIdProducto.Count > 0 Then _
                    .SqlParametros.Add("@listIdProducto", SqlDbType.VarChar).Value = String.Join(",", _listIdProducto.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _listCodigo IsNot Nothing AndAlso _listCodigo.Count > 0 Then _
                    .SqlParametros.Add("@listCodigo", SqlDbType.VarChar).Value = String.Join(",", _listCodigo.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _esProductoExterno IsNot Nothing AndAlso _esProductoExterno.Count > 0 Then _
                    .SqlParametros.Add("@esProductoExterno", SqlDbType.VarChar).Value = String.Join(",", _esProductoExterno.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())
            If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania

            .ejecutarReader("ObtenerProductoDocumentoFinanciero", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objProductoDocumentoFinanciero As ProductoDocumentoFinanciero
                While .Reader.Read
                    objProductoDocumentoFinanciero = New ProductoDocumentoFinanciero
                    objProductoDocumentoFinanciero.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objProductoDocumentoFinanciero)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
