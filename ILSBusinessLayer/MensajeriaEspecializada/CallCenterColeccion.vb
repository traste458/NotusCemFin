Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.MensajeriaEspecializada
Imports System.Web

Public Class CallCenterColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idCallCenter As Integer
    Private _nombreCallCenter As String
    Private _activo As Nullable(Of Boolean)

    Private _cargado As Boolean

    'Filtros externos
    Private _idCampania As Integer

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

    Public Sub New(ByVal idCampania As Integer)
        MyBase.New()
        _idCampania = idCampania
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As CallCenter
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(value As CallCenter)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdCallCenter As Integer
        Get
            Return _idCallCenter
        End Get
        Set(value As Integer)
            _idCallCenter = value
        End Set
    End Property

    Public Property NombreCallCenter As String
        Get
            Return _nombreCallCenter
        End Get
        Set(value As String)
            _nombreCallCenter = value
        End Set
    End Property

    Public Property Activo As Nullable(Of Boolean)
        Get
            Return _activo
        End Get
        Set(value As Nullable(Of Boolean))
            _activo = value
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

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miObj As Type = GetType(CallCenter)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As CallCenter)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As CallCenter)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As CallCenterColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As CallCenter)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idPlan As Short) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), CallCenter)
                If .IdCallCenter = idPlan Then
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
            Dim miDetalle As CallCenter

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miDetalle = CType(Me.InnerList(index), CallCenter)
                If miDetalle IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(CallCenter).GetProperties
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
                    Dim idUsuarioConsulta As Integer = 0

                    If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                        If Not EsNuloOVacio(HttpContext.Current.Session("usxp001")) Then Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuarioConsulta)
                    End If
                    If _idCallCenter > 0 Then .SqlParametros.Add("@idCallCenter", SqlDbType.Int).Value = _idCallCenter
                    If Not String.IsNullOrEmpty(_nombreCallCenter) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombreCallCenter
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                    If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuarioConsulta

                    .ejecutarReader("ObtenerCallCenters", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As CallCenter

                        While .Reader.Read
                            If .Reader.HasRows Then
                                elDetalle = New CallCenter
                                Integer.TryParse(.Reader("idCallCenter"), elDetalle.IdCallCenter)
                                elDetalle.NombreCallCenter = .Reader("nombreCallCenter")
                                elDetalle.NombreContacto = .Reader("nombreContacto")
                                elDetalle.TelefonoContacto = .Reader("telefonoContacto")
                                elDetalle.Activo = .Reader("activo")
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
