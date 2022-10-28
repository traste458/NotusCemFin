Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class MaterialSiembraColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _material As String
    Private _referencia As String
    Private _filtroRapido As String

    Private _cargado As Boolean

#End Region

#Region "Cosntructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal filtroRapido As String)
        MyBase.New()
        _filtroRapido = filtroRapido
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Public Property Material As String
        Get
            Return _material
        End Get
        Set(value As String)
            _material = value
        End Set
    End Property

    Public Property Referencia As String
        Get
            Return _referencia
        End Get
        Set(value As String)
            _referencia = value
        End Set
    End Property

    Public Property FiltroRapido As String
        Get
            Return _filtroRapido
        End Get
        Set(value As String)
            _filtroRapido = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objBase As Type = GetType(MaterialSiembra)
        Dim pInfo As PropertyInfo

        For Each pInfo In objBase.GetProperties
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

    Default Public Property Item(ByVal index As Integer) As MaterialSiembra
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As MaterialSiembra)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As MaterialSiembra)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As MaterialSiembra)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As MaterialSiembra)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As MaterialSiembra

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), MaterialSiembra)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(MaterialSiembra).GetProperties
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
            If Not String.IsNullOrEmpty(_material) Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = _material
            If Not String.IsNullOrEmpty(_referencia) Then .SqlParametros.Add("@referencia", SqlDbType.VarChar).Value = _referencia
            If Not String.IsNullOrEmpty(_filtroRapido) Then .SqlParametros.Add("@filtroRapido", SqlDbType.VarChar).Value = _filtroRapido

            .ejecutarReader("ObtenerMaterialSiembra", CommandType.StoredProcedure)

            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objDetalle As MaterialSiembra
                While .Reader.Read
                    objDetalle = New MaterialSiembra()
                    objDetalle.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objDetalle)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

    Public Function CargarMaterialesCombo(Material As String, startIndex As Integer, endIndex As Integer) As DataTable
        Dim dbManager As New LMDataAccess
        Dim Dtmateriales As New DataTable
        If _cargado Then Me.InnerList.Clear()
        With dbManager
            If Not String.IsNullOrEmpty(Material) Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = String.Format("%{0}%", Material)
            If (startIndex > 0) Then .SqlParametros.Add("@startIndex", SqlDbType.Int).Value = startIndex
            If (endIndex > 0) Then .SqlParametros.Add("@endIndex", SqlDbType.Int).Value = endIndex
            .TiempoEsperaComando = 0
            Dtmateriales = .ejecutarDataTable("ObtenerMaterialSiembraCombo", CommandType.StoredProcedure)
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
        Return Dtmateriales
    End Function
#End Region

End Class
