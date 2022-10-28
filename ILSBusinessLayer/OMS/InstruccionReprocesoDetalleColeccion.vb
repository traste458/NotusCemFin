Imports LMDataAccessLayer
Imports System.Reflection

Public Class InstruccionReprocesoDetalleColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _idInstruccionReproceso As ArrayList
    Private _idInstruccionReprocesoDetalle As ArrayList
    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As InstruccionReprocesoDetalle
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(value As InstruccionReprocesoDetalle)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("El objeto que intenta adicionar es Nulo")
            End If
        End Set
    End Property

    Public Property IdInstruccionReproceso() As ArrayList
        Get
            If _idInstruccionReproceso Is Nothing Then _idInstruccionReproceso = New ArrayList
            Return _idInstruccionReproceso
        End Get
        Set(ByVal value As ArrayList)
            _idInstruccionReproceso = value
        End Set
    End Property

    Public Property IdInstruccionReprocesoDetalle() As ArrayList
        Get
            If _idInstruccionReprocesoDetalle Is Nothing Then _idInstruccionReprocesoDetalle = New ArrayList
            Return _idInstruccionReprocesoDetalle
        End Get
        Set(ByVal value As ArrayList)
            _idInstruccionReprocesoDetalle = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idInstruccionReprocesoDetalle As Integer)
        MyBase.New()
        _idInstruccionReprocesoDetalle = New ArrayList()
        _idInstruccionReprocesoDetalle.Add(idInstruccionReprocesoDetalle)
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objDetalleListaPrecios As Type = GetType(InstruccionReprocesoDetalle)
        Dim pInfo As PropertyInfo

        For Each pInfo In objDetalleListaPrecios.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As InstruccionReprocesoDetalle)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As InstruccionReprocesoDetalle)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As InstruccionReprocesoDetalle)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As InstruccionReprocesoDetalle

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), InstruccionReprocesoDetalle)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(InstruccionReprocesoDetalle).GetProperties
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
        Try
            With dbManager
                If _idInstruccionReproceso IsNot Nothing AndAlso _idInstruccionReproceso.Count > 0 Then _
                        .SqlParametros.Add("@listaIdInstruccionReproceso", SqlDbType.VarChar).Value = Join(_idInstruccionReproceso.ToArray(), ",")
                If _idInstruccionReprocesoDetalle IsNot Nothing AndAlso _idInstruccionReprocesoDetalle.Count > 0 Then _
                        .SqlParametros.Add("@listaIdInstruccionReprocesoDetalle", SqlDbType.VarChar).Value = Join(_idInstruccionReprocesoDetalle.ToArray, ",")
                .ejecutarReader("ConsultaItemDetalleInstruccionReproceso", CommandType.StoredProcedure)
                If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                    Dim objDetalleInstruccionReproceso As InstruccionReprocesoDetalle
                    While .Reader.Read
                        objDetalleInstruccionReproceso = New InstruccionReprocesoDetalle()
                        objDetalleInstruccionReproceso.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(objDetalleInstruccionReproceso)
                    End While
                    .Reader.Close()
                    _cargado = True
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
