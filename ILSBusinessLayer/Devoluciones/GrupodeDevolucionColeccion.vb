Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.MensajeriaEspecializada

Public Class GrupodeDevolucionColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idGrupoDevolucion As Integer
    Private _idGrupo As Integer
    Private _idUsuario As Integer
    Private _nombre As String
    Private _idTipoDevolucion As Integer
    Private _activo As Integer
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal activo As Boolean)
        MyBase.New()
        _activo = activo
        CargarDatos()
    End Sub

    Public Sub New(ByVal idGrupoDevolucion As Integer)
        MyBase.New()
        _idGrupoDevolucion = idGrupoDevolucion
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As GrupodeDevolucion
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(value As GrupodeDevolucion)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property
    Public Property IdGrupoDevolucion As Integer
        Get
            Return _idGrupoDevolucion
        End Get
        Set(value As Integer)
            _idGrupoDevolucion = value
        End Set
    End Property
    Public Property IdGrupo As Integer
        Get
            Return _idGrupo
        End Get
        Set(value As Integer)
            _idGrupo = value
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

    Public Property IdTipoDevolucion As Integer
        Get
            Return _idTipoDevolucion
        End Get
        Set(value As Integer)
            _idTipoDevolucion = value
        End Set
    End Property

    Public Property NombreGupoDevolucion As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property Activo As Boolean
        Get
            Return _activo
        End Get
        Set(value As Boolean)
            _activo = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miObj As Type = GetType(GrupodeDevolucion)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GrupodeDevolucion)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As GrupodeDevolucion)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As GrupodeDevolucionColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As GrupodeDevolucion)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal IdGrupodeDevolucion As Short) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), GrupodeDevolucion)
                If .IdGrupoDevolucion = IdGrupodeDevolucion Then
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
            Dim miDetalle As GrupodeDevolucion

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miDetalle = CType(Me.InnerList(index), GrupodeDevolucion)
                If miDetalle IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(GrupodeDevolucion).GetProperties
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

                    If _idGrupoDevolucion > 0 Then .SqlParametros.Add("@idGrupoDevolucion", SqlDbType.Int).Value = _idGrupoDevolucion
                    If _activo > 0 Then .SqlParametros.Add("@estado", SqlDbType.Int).Value = _activo

                    .ejecutarReader("ObtenerGruposDevolucion", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As GrupodeDevolucion

                        While .Reader.Read
                            If .Reader.HasRows Then
                                elDetalle = New GrupodeDevolucion()
                                Integer.TryParse(.Reader("idgrupo_devolucion"), elDetalle.IdGrupoDevolucion)
                                elDetalle.NombreGupoDevolucion = .Reader("idgrupo_devolucion2")
                                Integer.TryParse(.Reader("idgrupo"), elDetalle.IdGrupo)
                                Integer.TryParse(.Reader("idTipoDevolucion"), elDetalle.IdTipoDevolucion)
                                Integer.TryParse(.Reader("estado"), elDetalle.Activo)
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
