Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.MensajeriaEspecializada

Public Class ServicioMensajeriaSiembraColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _listServicioMensajeria As List(Of Integer)
    Private _idEstado As Integer
    Private _idGerencia As Integer
    Private _idCoordinador As Integer
    Private _idConsultor As Integer
    Private _fechaRegistroInicio As Date
    Private _fechaRegistroFin As Date

    Private _cargado As Boolean

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As ServicioMensajeriaSiembra
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As ServicioMensajeriaSiembra)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property ListServicioMensajeria As List(Of Integer)
        Get
            Return _listServicioMensajeria
        End Get
        Set(value As List(Of Integer))
            _listServicioMensajeria = value
        End Set
    End Property

    Public Property IdEstado As Integer
        Get
            Return _idEstado
        End Get
        Set(value As Integer)
            _idEstado = value
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

    Public Property IdCoordiandor As Integer
        Get
            Return _idCoordinador
        End Get
        Set(value As Integer)
            _idCoordinador = value
        End Set
    End Property

    Public Property IdConsultor As Integer
        Get
            Return _idConsultor
        End Get
        Set(value As Integer)
            _idConsultor = value
        End Set
    End Property

    Public Property FechaRegistroInicio As Date
        Get
            Return _fechaRegistroInicio
        End Get
        Set(value As Date)
            _fechaRegistroInicio = value
        End Set
    End Property

    Public Property FechaRegistroFin As Date
        Get
            Return _fechaRegistroFin
        End Get
        Set(value As Date)
            _fechaRegistroFin = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miObjeto As Type = GetType(ServicioMensajeriaSiembra)
        Dim pInfo As PropertyInfo

        For Each pInfo In miObjeto.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ServicioMensajeriaSiembra)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As ServicioMensajeriaSiembra)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As ServicioMensajeriaSiembraColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As ServicioMensajeriaSiembra)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idServicio As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), ServicioMensajeriaSiembra)
                If .IdServicioMensajeria = idServicio Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Not _listServicioMensajeria Is Nothing AndAlso _listServicioMensajeria.Count > 0 Then _
                        .SqlParametros.Add("@listIdServicio", SqlDbType.VarChar).Value = String.Join(",", _listServicioMensajeria.ConvertAll(Of String)(Function(x) x.ToString()).ToArray)
                If Me._idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = Me._idEstado
                If Me._fechaRegistroInicio <> Date.MinValue Then .SqlParametros.Add("@fechaRegistroInicio", SqlDbType.DateTime).Value = _fechaRegistroInicio
                If Me._fechaRegistroFin <> Date.MinValue Then .SqlParametros.Add("@fechaRegistroFin", SqlDbType.DateTime).Value = _fechaRegistroFin
                If Me._idGerencia > 0 Then .SqlParametros.Add("@idGerencia", SqlDbType.Int).Value = Me._idGerencia
                If Me._idCoordinador > 0 Then .SqlParametros.Add("@idPersonaCoordinador", SqlDbType.Int).Value = _idCoordinador
                If Me._idConsultor > 0 Then .SqlParametros.Add("@idPersonaConsultor", SqlDbType.Int).Value = _idConsultor

                .ejecutarReader("ObtenerInfoGeneralServicioSiembra", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim elDetalle As ServicioMensajeriaSiembra

                    While .Reader.Read
                        elDetalle = New ServicioMensajeriaSiembra
                        elDetalle.CargarResultadoConsulta(.Reader)
                        _cargado = True
                        Me.InnerList.Add(elDetalle)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
