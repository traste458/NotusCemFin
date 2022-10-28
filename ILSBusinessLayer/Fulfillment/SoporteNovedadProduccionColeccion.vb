﻿Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports System.IO

Public Class SoporteNovedadProduccionColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idNovedad As Integer
    Private _idTipoSoporte As Byte
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idNovedad As Integer)
        Me.New()
        _idNovedad = idNovedad
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As SoporteNovedadProduccion
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As SoporteNovedadProduccion)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdNovedad() As Integer
        Get
            Return _idNovedad
        End Get
        Set(ByVal value As Integer)
            _idNovedad = value
        End Set
    End Property

    Public Property IdTipoSoporte() As Byte
        Get
            Return _idTipoSoporte
        End Get
        Set(ByVal value As Byte)
            _idTipoSoporte = value
        End Set
    End Property

    Public ReadOnly Property Cargado As Boolean
        Get
            Return _cargado
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim mSoporteNovedadProduccion As Type = GetType(SoporteNovedadProduccion)
        Dim pInfo As PropertyInfo

        For Each pInfo In mSoporteNovedadProduccion.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As SoporteNovedadProduccion)
        Me.InnerList.Insert(posicion, valor)
        If Not _cargado Then _cargado = True
    End Sub

    Public Sub Add(ByVal valor As SoporteNovedadProduccion)
        Me.InnerList.Add(valor)
        If Not _cargado Then _cargado = True
    End Sub

    Public Sub AdicionarRango(ByVal rango As SoporteNovedadProduccionColeccion)
        Me.InnerList.AddRange(rango)
        If Not _cargado Then _cargado = True
    End Sub

    Public Sub Remover(ByVal valor As SoporteNovedadProduccion)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal identificador As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), SoporteNovedadProduccion)
                If .IdSoporte = identificador Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Function GenerarDataTable() As DataTable
        If Not _cargado AndAlso Me.InnerList.Count = 0 Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miDetalle As SoporteNovedadProduccion

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), SoporteNovedadProduccion)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(SoporteNovedadProduccion).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess

            Me.Clear()
            With dbManager
                If Me._idNovedad > 0 Then .SqlParametros.Add("@idNovedad", SqlDbType.Int).Value = Me._idNovedad
                If Me._idTipoSoporte > 0 Then .SqlParametros.Add("@idTipoSoporte", SqlDbType.TinyInt).Value = Me._idTipoSoporte
                .ejecutarReader("ObtenerInformacionDeSoporteDeNovedadDeProduccion", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim obj As SoporteNovedadProduccion
                    While .Reader.Read
                        obj = New SoporteNovedadProduccion
                        obj.CargarValorDePropiedades(.Reader)
                        Me.InnerList.Add(obj)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        End Using
    End Sub

#End Region

#Region "Métodos Compartidos"

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As SoporteNovedadProduccion, ByVal ExtraTypes() As System.Type)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(SoporteNovedadProduccion), ExtraTypes)
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Sub SerializeObject(ByVal filename As String, ByVal col As SoporteNovedadProduccion)
        Dim s As New Xml.Serialization.XmlSerializer(GetType(SoporteNovedadProduccion))
        Dim writer As New StreamWriter(filename)

        s.Serialize(writer, col)
        writer.Close()
    End Sub

    Public Shared Function DeserializeObject(ByVal filename As String, ByVal ExtraTypes() As System.Type) As SoporteNovedadProduccion

        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(SoporteNovedadProduccion), ExtraTypes)
        Dim g As SoporteNovedadProduccion = CType(w.Deserialize(fs), SoporteNovedadProduccion)

        fs.Close()

        Return g
    End Function

    Public Shared Function DeserializeObject(ByVal filename As String) As SoporteNovedadProduccion
        Dim fs As New IO.FileStream(filename, FileMode.Open)
        Dim w As New Xml.Serialization.XmlSerializer(GetType(SoporteNovedadProduccion))
        Dim g As SoporteNovedadProduccion = CType(w.Deserialize(fs), SoporteNovedadProduccion)

        fs.Close()

        Return g
    End Function

#End Region

End Class
