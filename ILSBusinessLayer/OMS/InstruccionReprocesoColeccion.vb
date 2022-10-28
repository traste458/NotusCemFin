Imports LMDataAccessLayer
Imports System.Reflection

Public Class InstruccionReprocesoColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _idInstruccionReproceso As ArrayList
    Private _idTipoInstruccion As ArrayList
    Private _idEstado As ArrayList
    Private _idTipoClasificacionInstruccion As ArrayList
    Private _idCreador As ArrayList
    Private _fechaInicio As Date
    Private _fechaFinal As Date
    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As InstruccionReproceso
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As InstruccionReproceso)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
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

    Public Property IdTipoInstruccion() As ArrayList
        Get
            If _idTipoInstruccion Is Nothing Then _idTipoInstruccion = New ArrayList
            Return _idTipoInstruccion
        End Get
        Set(ByVal value As ArrayList)
            _idTipoInstruccion = value
        End Set
    End Property

    Public Property IdEstado() As ArrayList
        Get
            If _idEstado Is Nothing Then _idEstado = New ArrayList
            Return _idEstado
        End Get
        Set(ByVal value As ArrayList)
            _idEstado = value
        End Set
    End Property

    Public Property IdTipoClasificacionInstruccion As ArrayList
        Get
            If _idTipoClasificacionInstruccion Is Nothing Then _idTipoClasificacionInstruccion = New ArrayList
            Return _idTipoClasificacionInstruccion
        End Get
        Set(value As ArrayList)
            _idTipoClasificacionInstruccion = value
        End Set
    End Property

    Public Property IdCreador As ArrayList
        Get
            If _idCreador Is Nothing Then _idCreador = New ArrayList
            Return _idCreador
        End Get
        Set(value As ArrayList)
            _idCreador = value
        End Set
    End Property

    Public Property FechaInicio() As Date
        Get
            Return _fechaInicio
        End Get
        Set(ByVal value As Date)
            _fechaInicio = value
        End Set
    End Property

    Public Property FechaFinal() As Date
        Get
            Return _fechaFinal
        End Get
        Set(ByVal value As Date)
            _fechaFinal = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objInstruccionReproceso As Type = GetType(InstruccionReproceso)
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As InstruccionReproceso)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As InstruccionReproceso)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As InstruccionReprocesoColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As InstruccionReproceso

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), InstruccionReproceso)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(InstruccionReproceso).GetProperties
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
            If _idInstruccionReproceso IsNot Nothing AndAlso _idInstruccionReproceso.Count > 0 Then _
                        .SqlParametros.Add("@listaIdInstruccionReproceso", SqlDbType.VarChar).Value = Join(_idInstruccionReproceso.ToArray(), ",")
            If _idTipoInstruccion IsNot Nothing AndAlso _idTipoInstruccion.Count > 0 Then _
                        .SqlParametros.Add("@listaIdTipoInstruccion", SqlDbType.VarChar).Value = Join(_idTipoInstruccion.ToArray(), ",")
            If _idEstado IsNot Nothing AndAlso _idEstado.Count > 0 Then _
                        .SqlParametros.Add("@listaIdEstado", SqlDbType.VarChar).Value = Join(_idEstado.ToArray(), ",")
            If _idTipoClasificacionInstruccion IsNot Nothing AndAlso _idTipoClasificacionInstruccion.Count > 0 Then _
                        .SqlParametros.Add("@listaTipoClasificacion", SqlDbType.VarChar).Value = Join(_idTipoClasificacionInstruccion.ToArray(), ",")
            If _idCreador IsNot Nothing AndAlso _idCreador.Count > 0 Then _
                        .SqlParametros.Add("@listaCreador", SqlDbType.VarChar).Value = Join(_idCreador.ToArray(), ",")
            If Not _fechaInicio.Equals(Date.MinValue) Then _
                    .SqlParametros.Add("@fechaInicio", SqlDbType.DateTime).Value = _fechaInicio
            If Not _fechaFinal.Equals(Date.MinValue) Then _
                    .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal

            .ejecutarReader("ConsultaItemInstruccionReproceso", CommandType.StoredProcedure)
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objListaPrecios As InstruccionReproceso
                While .Reader.Read
                    objListaPrecios = New InstruccionReproceso()
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
