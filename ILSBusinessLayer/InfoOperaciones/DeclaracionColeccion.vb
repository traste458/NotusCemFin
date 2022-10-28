Imports LMDataAccessLayer
Imports System.Reflection

Public Class DeclaracionColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"
    Private _conSoporte As String
    Private _cargado As Boolean

    Private _declaracion As String
    Private _factura As String
    Private _fechaInicial As Date
    Private _fechaFinal As Date

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As Declaracion.Declaracion
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As Declaracion.Declaracion)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property ConSoporte As String
        Get
            Return _conSoporte
        End Get
        Set(value As String)
            _conSoporte = value
        End Set
    End Property

    Public Property Declaracion As String
        Get
            Return _declaracion
        End Get
        Set(value As String)
            _declaracion = value
        End Set
    End Property

    Public Property Factura As String
        Get
            Return _factura
        End Get
        Set(value As String)
            _factura = value
        End Set
    End Property

    Public Property FechaInicial As Date
        Get
            Return _fechaInicial
        End Get
        Set(value As Date)
            _fechaInicial = value
        End Set
    End Property

    Public Property FechaFinal As Date
        Get
            Return _fechaFinal
        End Get
        Set(value As Date)
            _fechaFinal = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objAbreviaturaDireccion As Type = GetType(Declaracion.Declaracion)
        Dim pInfo As PropertyInfo

        For Each pInfo In objAbreviaturaDireccion.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Declaracion.Declaracion)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As Declaracion.Declaracion)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As Declaracion.Declaracion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As Declaracion.Declaracion
        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), Declaracion.Declaracion)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(Declaracion.Declaracion).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miRegistro, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next
        _cargado = False
        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess

        If _cargado Then Me.InnerList.Clear()
        With dbManager
            If _conSoporte <> "" Then .SqlParametros.Add("@conSoporte", SqlDbType.Bit).Value = CBool(ConSoporte)
            If _declaracion <> "" Then .SqlParametros.Add("@declaracion", SqlDbType.VarChar).Value = _declaracion
            If _factura <> "" Then .SqlParametros.Add("@factura", SqlDbType.VarChar).Value = _factura
            If _fechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.DateTime).Value = _fechaInicial
            If _fechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.DateTime).Value = _fechaFinal

            .ejecutarReader("ObtenerInfoDeclaracionSoporte", CommandType.StoredProcedure)
            Me.InnerList.Clear()
            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objDeclaracion As New Declaracion.Declaracion
                While .Reader.Read
                    objDeclaracion = New Declaracion.Declaracion()
                    objDeclaracion.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objDeclaracion)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class

