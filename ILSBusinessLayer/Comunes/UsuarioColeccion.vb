Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class UsuarioColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idUsuario As Integer
    Private _idPerfil As List(Of Integer)

    Private _cargado As Boolean

    'Filtros externos
    Private _idSite As Integer

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As Usuario
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(value As Usuario)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdUsuario As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
        End Set
    End Property

    Public Property IdPerfil As List(Of Integer)
        Get
            Return _idPerfil
        End Get
        Set(value As List(Of Integer))
            _idPerfil = value
        End Set
    End Property

    Public Property IdSite As Integer
        Get
            Return _idSite
        End Get
        Set(value As Integer)
            _idSite = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal listIdPerfil As List(Of Integer))
        MyBase.New()
        _idPerfil = listIdPerfil
        CargarDatos()
    End Sub

    Public Sub New(ByVal idSite As Integer)
        MyBase.New()
        _idSite = idSite
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miObj As Type = GetType(Usuario)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Usuario)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As Usuario)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As UsuarioColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As Usuario)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idUsuario As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), Usuario)
                If .IdUsuario = idUsuario Then
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
        Try
            Dim drAux As DataRow
            Dim miDetalle As Usuario

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miDetalle = CType(Me.InnerList(index), Usuario)
                If miDetalle IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Usuario).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                        ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
        Return dtAux
    End Function

    Private Sub CargarDatos()
        Using dbManager As New LMDataAccess
            With dbManager
                Try
                    .SqlParametros.Clear()

                    If _idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = _idUsuario
                    If Not _idPerfil Is Nothing AndAlso _idPerfil.Count > 0 Then _
                        .SqlParametros.Add("@listaIdPerfil", SqlDbType.VarChar).Value = String.Join(",", _idPerfil.ConvertAll(Of String)(Function(x) x.ToString()).ToArray)
                    If _idSite > 0 Then .SqlParametros.Add("@idSite", SqlDbType.Int).Value = _idSite

                    .ejecutarReader("ObtenerInfoUsuarioGeneral", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As Usuario

                        While .Reader.Read
                            If .Reader.HasRows Then
                                elDetalle = New Usuario
                                Integer.TryParse(.Reader("idUsuario"), elDetalle.IdUsuario)
                                Integer.TryParse(.Reader("idPerfil"), elDetalle.IdPerfil)
                                elDetalle.Nombre = .Reader("nombre")
                                elDetalle.NombrePerfil = .Reader("perfil")
                                _cargado = True
                                Me.InnerList.Add(elDetalle)
                            End If
                        End While
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End With
        End Using
    End Sub

#End Region

End Class
