Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class MaterialEquipoClaseSIMColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _material As ArrayList
    Private _idClase As ArrayList
    Private _cargado As Boolean
    Private _Idciudad As ArrayList
    Private _IdCiuda As String

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.new()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As MaterialEquipoClaseSIM
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As MaterialEquipoClaseSIM)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property Material As ArrayList
        Get
            If _material Is Nothing Then _material = New ArrayList
            Return _material
        End Get
        Set(value As ArrayList)
            _material = value
        End Set
    End Property
    Public Property Ciudad As ArrayList
        Get
            If _Idciudad Is Nothing Then _Idciudad = New ArrayList
            Return _Idciudad
        End Get
        Set(value As ArrayList)
            _Idciudad = value
        End Set
    End Property

    Public Property IdClase As ArrayList
        Get
            If _idClase Is Nothing Then _idClase = New ArrayList
            Return _idClase
        End Get
        Set(value As ArrayList)
            _idClase = value
        End Set
    End Property

    Public Property IdCiuda As String
        Get
            Return _IdCiuda
        End Get
        Set(value As String)
            _IdCiuda = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objInstruccionReproceso As Type = GetType(MaterialEquipoClaseSIM)
        Dim pInfo As PropertyInfo

        For Each pInfo In objInstruccionReproceso.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As MaterialEquipoClaseSIM)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As MaterialEquipoClaseSIM)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As MaterialEquipoClaseSIM)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As MaterialEquipoClaseSIM

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), MaterialEquipoClaseSIM)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(MaterialEquipoClaseSIM).GetProperties
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
            If _material IsNot Nothing AndAlso _material.Count > 0 Then _
                        .SqlParametros.Add("@listaMaterial", SqlDbType.VarChar).Value = Join(_material.ToArray(), ",")
            If _idClase IsNot Nothing AndAlso _idClase.Count > 0 Then _
                        .SqlParametros.Add("@listaClases", SqlDbType.VarChar).Value = Join(_idClase.ToArray(), ",")
            .SqlParametros.Add("@ciudad", SqlDbType.VarChar).Value = _IdCiuda
            .ejecutarReader("ConsultaItemMaterialEquipoClaseSIM2", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objListaPrecios As MaterialEquipoClaseSIM
                While .Reader.Read
                    objListaPrecios = New MaterialEquipoClaseSIM()
                    objListaPrecios.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objListaPrecios)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
