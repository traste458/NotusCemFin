Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class CiudadesCEMColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _idCiudad As Integer
    Private _idBodega As Integer
    Private _idCampania As Integer

    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As CiudadesCEM
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As CiudadesCEM)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdCiudad As Integer
        Get
            Return _idCiudad
        End Get
        Set(value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property IdBodega As Integer
        Get
            Return _idBodega
        End Get
        Set(value As Integer)
            _idBodega = value
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

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

    Public Sub New(ByVal idCampania As Integer)
        MyBase.New()
        _idCampania = idCampania
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objCiudadesCEM As Type = GetType(CiudadesCEM)
        Dim pInfo As PropertyInfo

        For Each pInfo In objCiudadesCEM.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ProductoComercial)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As ProductoComercial)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As ProductoComercial)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As ProductoComercial

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), ProductoComercial)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(ProductoComercial).GetProperties
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
            If _idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = _idCiudad
            If _idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = _idBodega
            If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania

            .ejecutarReader("ObtenerCiudadesBodegasCEM", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objCiudadesCEM As CiudadesCEM
                While .Reader.Read
                    objCiudadesCEM = New CiudadesCEM
                    objCiudadesCEM.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objCiudadesCEM)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
