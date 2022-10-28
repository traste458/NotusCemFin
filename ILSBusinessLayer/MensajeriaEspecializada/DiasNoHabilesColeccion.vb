Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class DiasNoHabilesColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _estado As Nullable(Of Boolean)

    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal activo As Boolean)
        MyBase.New()
        _estado = activo
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As DiasNoHabiles
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As DiasNoHabiles)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property FechaInicial As Date
        Get
            Return _fechaInicial
        End Get
        Set(value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal As Date
        Get
            Return _fechaFinal
        End Get
        Set(value As Date)
            _fechaFinal = value
        End Set
    End Property

    Public Property Estado As Nullable(Of Boolean)
        Get
            Return _estado
        End Get
        Set(value As Nullable(Of Boolean))
            _estado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miObj As Type = GetType(DiasNoHabiles)
        Dim pInfo As PropertyInfo

        For Each pInfo In miObj.GetProperties
            If pInfo.PropertyType.Namespace = "System" Then
                With dtAux
                    .Columns.Add(pInfo.Name, pInfo.PropertyType)
                End With
            ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                With dtAux
                    .Columns.Add(pInfo.Name, GetType(Boolean))
                End With
            End If
        Next
        Return dtAux
    End Function

#End Region

#Region "Métodos Públicos"

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DiasNoHabiles)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As DiasNoHabiles)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As DiasNoHabilesColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As DiasNoHabiles)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal fecha As Date) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), DiasNoHabiles)
                If .Fecha = fecha Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miDetalle As DiasNoHabiles

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), DiasNoHabiles)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(DiasNoHabiles).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                    ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
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
            Try
                Me.Clear()
                With dbManager
                    If Me._fechaInicial <> Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.DateTime).Value = Me._fechaInicial
                    If Me._fechaFinal <> Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = Me._fechaFinal
                    If Me._estado IsNot Nothing Then .SqlParametros.Add("@estado", SqlDbType.Bit).Value = Me._estado

                    .ejecutarReader("ObtenerDiasNoHabiles", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As DiasNoHabiles

                        While .Reader.Read
                            If .Reader.HasRows Then
                                elDetalle = New DiasNoHabiles
                                Integer.TryParse(.Reader("idDia"), elDetalle.IdDia)
                                Date.TryParse(.Reader("fecha"), elDetalle.Fecha)
                                elDetalle.Estado = CBool(.Reader("estado"))
                                elDetalle.NombreDia = .Reader("nombreDia")

                                Me.InnerList.Add(elDetalle)
                            End If
                        End While
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                End With
                _cargado = True
            Catch ex As Exception
                Throw ex
            End Try
        End Using
    End Sub

#End Region

End Class
