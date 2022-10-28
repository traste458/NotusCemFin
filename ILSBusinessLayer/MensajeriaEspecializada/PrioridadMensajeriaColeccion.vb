Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class PrioridadMensajeriaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idPrioridad As Integer
    Private _cargado As Boolean
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idPrioridad As Integer)
        Me.New()
        _idPrioridad = idPrioridad
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As PrioridadMensajeria
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As PrioridadMensajeria)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miPrioridadMensajeria As Type = GetType(PrioridadMensajeria)
        Dim pInfo As PropertyInfo

        For Each pInfo In miPrioridadMensajeria.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As PrioridadMensajeria)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As PrioridadMensajeria)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As PrioridadMensajeriaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As PrioridadMensajeria)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idPrioridad As String) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), PrioridadMensajeria)
                If .IdPrioridad = idPrioridad Then
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
        Dim miDetalle As PrioridadMensajeria

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), PrioridadMensajeria)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(PrioridadMensajeria).GetProperties
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
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idPrioridad > 0 Then .SqlParametros.Add("@idPrioridad", SqlDbType.Int).Value = Me._idPrioridad
                .ejecutarReader("ObtenerPrioridadesDeMensajeria", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim infoPrioridad As PrioridadMensajeria

                    While .Reader.Read
                        infoPrioridad = New PrioridadMensajeria
                        Integer.TryParse(.Reader("idPrioridad").ToString, infoPrioridad.IdPrioridad)
                        infoPrioridad.Prioridad = .Reader("prioridad").ToString
                        infoPrioridad.Activo = CBool(.reader("activo"))
                        infoPrioridad.Registrado = True
                        _cargado = True
                        Me.InnerList.Add(infoPrioridad)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
