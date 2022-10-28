﻿Imports LMDataAccessLayer
Imports System.Reflection

''' <summary>
''' Author: Beltrán, Diego
''' Create date: 02/02/2015
''' Description: Colección diseñada para el manejo y administración de los perfiles asociados a una bodega en dependencia de su unidad de negocio
''' </summary>
''' <remarks></remarks>
Public Class PerfilTipoServicioBodegaColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _idBodega As Integer

    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Propiedad por defecto que instancia la clase a la cual se asigna la colección
    ''' </summary>
    ''' <param name="index"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal index As Integer) As PerfilTipoServicioBodega
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As PerfilTipoServicioBodega)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    ''' <summary>
    ''' Define o establece el listado de identificadores de la tabla, que se desean consultar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IdBodega As Integer
        Get
            Return _idBodega
        End Get
        Set(value As Integer)
            _idBodega = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idBodega As Integer)
        MyBase.New()
        _idBodega = idBodega
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    ''' <summary>
    ''' Función que permite crear la estructura de la tabla, basada en las propiedades de la clase instanciada
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objPerfilTipoServicioBodega As Type = GetType(PerfilTipoServicioBodega)
        Dim pInfo As PropertyInfo

        For Each pInfo In objPerfilTipoServicioBodega.GetProperties
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

    ''' <summary>
    ''' Método que permite Insertar elementos a la colección
    ''' </summary>
    ''' <param name="posicion"></param>
    ''' <param name="valor"></param>
    ''' <remarks></remarks>
    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As PerfilTipoServicioBodega)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    ''' <summary>
    ''' Método que permite Adicionar elementos a la colección
    ''' </summary>
    ''' <param name="valor"></param>
    ''' <remarks></remarks>
    Public Sub Adicionar(ByVal valor As PerfilTipoServicioBodega)
        Me.InnerList.Add(valor)
    End Sub

    ''' <summary>
    ''' Método que permite adicionar un rango de elementos a la colección
    ''' </summary>
    ''' <param name="rango"></param>
    ''' <remarks></remarks>
    Public Sub AdicionarRango(ByVal rango As PerfilTipoServicioBodega)
        Me.InnerList.AddRange(rango)
    End Sub

    ''' <summary>
    ''' Método que permite generar un elemento de tipo datatable
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As PerfilTipoServicioBodega

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), PerfilTipoServicioBodega)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(PerfilTipoServicioBodega).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    ''' <summary>
    ''' Método que permite cargar la colección con los datos obtenidos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        If _cargado Then Me.InnerList.Clear()
        With dbManager
            With .SqlParametros
                If _idBodega > 0 Then .Add("@idBodega", SqlDbType.Int).Value = _idBodega
            End With

            .ejecutarReader("ObtenerInfoPerfilBodega", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objPerfilTipoServicioBodega As PerfilTipoServicioBodega
                While .Reader.Read
                    objPerfilTipoServicioBodega = New PerfilTipoServicioBodega()
                    objPerfilTipoServicioBodega.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objPerfilTipoServicioBodega)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
