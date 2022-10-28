Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.MensajeriaEspecializada
Imports System.Web

Public Class PlanVentaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idPlan As Short
    Private _nombrePlan As String
    Private _idTipoPlan As Short
    Private _activo As Nullable(Of Boolean)
    Private _idUsuarioConsulta As Integer
    Private _listTipoServicio As List(Of Integer)

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

    Default Public Property Item(ByVal index As Integer) As PlanVenta
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(value As PlanVenta)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdPlan As Short
        Get
            Return _idPlan
        End Get
        Set(value As Short)
            _idPlan = value
        End Set
    End Property

    Public Property NombrePlan As String
        Get
            Return _nombrePlan
        End Get
        Set(value As String)
            _nombrePlan = value
        End Set
    End Property

    Public Property IdTipoPlan As Short
        Get
            Return _idTipoPlan
        End Get
        Set(value As Short)
            _idTipoPlan = value
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

    Public Property ListTipoServicio As List(Of Integer)
        Get
            If _listTipoServicio Is Nothing Then _listTipoServicio = New List(Of Integer)
            Return _listTipoServicio
        End Get
        Set(value As List(Of Integer))
            _listTipoServicio = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miObj As Type = GetType(PlanVenta)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As PlanVenta)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As PlanVenta)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As PlanVentaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As PlanVenta)
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
            With CType(Me.InnerList(index), PlanVenta)
                If .IdPlan = idPlan Then
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
            Dim miDetalle As PlanVenta

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miDetalle = CType(Me.InnerList(index), PlanVenta)
                If miDetalle IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(PlanVenta).GetProperties
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

    Public Sub CargarDatos()
        Using dbManager As New LMDataAccess
            With dbManager
                Try
                    
                    If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                        If Not EsNuloOVacio(HttpContext.Current.Session("usxp001")) Then Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, _idUsuarioConsulta)
                        If Not EsNuloOVacio(HttpContext.Current.Session("usxp001")) Then Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, _idUsuarioConsulta)
                    End If

                    .SqlParametros.Clear()

                    If _idPlan > 0 Then .SqlParametros.Add("@idPlan", SqlDbType.VarChar).Value = _idPlan
                    If Not String.IsNullOrEmpty(_nombrePlan) Then .SqlParametros.Add("@nombrePlan", SqlDbType.VarChar).Value = _nombrePlan
                    .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                    If _idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = Me._idUsuarioConsulta
                    If _idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = _idCampania
                    If _listTipoServicio IsNot Nothing AndAlso _listTipoServicio.Count > 0 Then _
                    .SqlParametros.Add("@listTipoServicio", SqlDbType.VarChar).Value = String.Join(",", _listTipoServicio.ConvertAll(Of String)(Function(x) x.ToString()).ToArray())

                    .ejecutarReader("ObtienePlanesDeVenta", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As PlanVenta

                        While .Reader.Read
                            If .Reader.HasRows Then
                                elDetalle = New PlanVenta
                                Integer.TryParse(.Reader("idPlan"), elDetalle.IdPlan)
                                elDetalle.NombrePlan = .Reader("nombrePlan")
                                elDetalle.Descripcion = .Reader("descripcion").ToString
                                elDetalle.CargoFijoMensual = .Reader("cargoFijoMensual").ToString
                                elDetalle.Activo = .Reader("activo")
                                Integer.TryParse(.Reader("idTipoPlan"), elDetalle.IdTipoPlan)
                                elDetalle.NombreTipoPlan = .Reader("nombreTipoPlan")
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
