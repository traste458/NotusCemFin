Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.Enumerados

Public Class CampaniaPOPColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _idCampania As ArrayList
    Private _estado As Integer = -1
    Private _fechaInicio As Date
    Private _fechaFin As Date
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As CampaniaPOP
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As CampaniaPOP)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdCampania As ArrayList
        Get
            If _idCampania Is Nothing Then _idCampania = New ArrayList
            Return _idCampania
        End Get
        Set(value As ArrayList)
            _idCampania = value
        End Set
    End Property

    Public Property Estado As Integer
        Get
            Return _estado
        End Get
        Set(value As Integer)
            _estado = value
        End Set
    End Property

    Public Property FechaInicio As Date
        Get
            Return _fechaInicio
        End Get
        Set(value As Date)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFin As Date
        Get
            Return _fechaFin
        End Get
        Set(value As Date)
            _fechaFin = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objCampaniaPOP As Type = GetType(CampaniaPOP)
        Dim pInfo As PropertyInfo

        For Each pInfo In objCampaniaPOP.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As CampaniaPOP)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As CampaniaPOP)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As CampaniaPOP)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As CampaniaPOP

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), CampaniaPOP)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(CampaniaPOP).GetProperties
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
            If _idCampania IsNot Nothing AndAlso _idCampania.Count > 0 Then _
                        .SqlParametros.Add("@idListaCampania", SqlDbType.VarChar).Value = Join(_idCampania.ToArray(), ",")
            If _estado > -1 Then .SqlParametros.Add("@estado", SqlDbType.Int).Value = _estado
            If _fechaInicio > Date.MinValue Then .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
            If _fechaFin > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFin

            .ejecutarReader("ObtenerCampaniasPOP", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objCampaniaPOP As CampaniaPOP
                While .Reader.Read
                    objCampaniaPOP = New CampaniaPOP()
                    objCampaniaPOP.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objCampaniaPOP)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
