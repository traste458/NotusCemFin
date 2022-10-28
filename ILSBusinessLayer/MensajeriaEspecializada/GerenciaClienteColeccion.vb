Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class GerenciaClienteColeccion
    Inherits CollectionBase

#Region "Filtros de Búsqueda"

    Private _idGerencia As Integer
    Private _activo As Boolean
    Private _nombre As String
    Private _idTercero As Integer

    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As GerenciaCliente
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As GerenciaCliente)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdGerencia As Integer
        Get
            Return _idGerencia
        End Get
        Set(value As Integer)
            _idGerencia = value
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

    Public Property Nombre As String
        Get
            Return _nombre
        End Get
        Set(value As String)
            _nombre = value
        End Set
    End Property

    Public Property IdTercero As Integer
        Get
            Return _idTercero
        End Get
        Set(value As Integer)
            _idTercero = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim objBase As Type = GetType(GerenciaCliente)
        Dim pInfo As PropertyInfo

        For Each pInfo In objBase.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GerenciaCliente)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As GerenciaCliente)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As GerenciaClienteColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim miRegistro As GerenciaCliente

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miRegistro = CType(Me.InnerList(index), GerenciaCliente)
            If miRegistro IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(GerenciaCliente).GetProperties
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
            If _idGerencia > 0 Then .SqlParametros.Add("@idGerencia", SqlDbType.Int).Value = _idGerencia
            .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
            If Not String.IsNullOrEmpty(_nombre) Then .SqlParametros.Add("@nombre", SqlDbType.VarChar).Value = _nombre
            If _idTercero > 0 Then .SqlParametros.Add("@idTercero", SqlDbType.Int).Value = _idTercero

            .ejecutarReader("ObtenerGerenciaCliente", CommandType.StoredProcedure)

            If .Reader IsNot Nothing AndAlso .Reader.HasRows Then
                Dim objDetalle As GerenciaCliente
                While .Reader.Read
                    objDetalle = New GerenciaCliente()
                    objDetalle.CargarResultadoConsulta(.Reader)
                    Me.InnerList.Add(objDetalle)
                End While
                _cargado = True
            End If
        End With
        If dbManager IsNot Nothing Then dbManager.Dispose()
    End Sub

#End Region

End Class
