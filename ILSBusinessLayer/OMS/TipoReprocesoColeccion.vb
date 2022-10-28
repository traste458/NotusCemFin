Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class TipoReprocesoColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idTipo As ArrayList
    Private _cargado As Boolean
    
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _cargado = False
    End Sub

    Public Sub New(ByVal idTipo As Byte)
        MyBase.New()
        _idTipo = New ArrayList
        _idTipo.Add(idTipo)
        _cargado = False
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As TipoReproceso
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As TipoReproceso)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public ReadOnly Property IdTipo() As ArrayList
        Get
            If _idTipo Is Nothing Then _idTipo = New ArrayList
            Return _idTipo
        End Get
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim tipo As Type = GetType(TipoReproceso)
        Dim pInfo As PropertyInfo

        For Each pInfo In tipo.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoReproceso)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As TipoReproceso)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As TipoReprocesoColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As TipoReproceso)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idTipo As Byte) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), TipoReproceso)
                If .IdTipo = idTipo Then
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
        Dim tipo As TipoReproceso

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            tipo = CType(Me.InnerList(index), TipoReproceso)
            If tipo IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(TipoReproceso).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(tipo, Nothing)
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
                If Me._idTipo IsNot Nothing AndAlso Me._idTipo.Count > 0 Then _
                    .SqlParametros.Add("@listaIdTipo", SqlDbType.VarChar, 500).Value = Join(Me._idTipo.ToArray, ",")
                .ejecutarReader("ConsultarTipoReproceso", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim tipo As TipoReproceso

                    While .Reader.Read
                        tipo = New TipoReproceso
                        Byte.TryParse(.Reader("idTipo").ToString, tipo.IdTipo)
                        tipo.Descripcion = .Reader("descripcion").ToString
                        Me.InnerList.Add(tipo)
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

#Region "Métodos Compartidos"

    Public Shared Function ObtenerTodosEnDataTable() As DataTable
        Dim dtAux As New DataTable
        Dim dbManager As New LMDataAccess

        Try
            dtAux = dbManager.ejecutarDataTable("ConsultarTipoReproceso", CommandType.StoredProcedure)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dtAux
    End Function

#End Region

End Class
