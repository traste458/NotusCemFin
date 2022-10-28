Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class NovedadServicioMensajeriaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idServicioMensajeria As Integer
    Private _idEstadoNovedad As Integer
    Private _numeroRadicado As Long
    Private _fechaAgendaInicio As Date
    Private _fechaAgendaFin As Date
    Private _idCiudad As Integer
    Private _idBodega As Integer
    Private _clienteVIP As Enumerados.EstadoBinario
    Private _urgente As Enumerados.EstadoBinario
    Private _idPrioridad As Integer
    Private _idTipoServicio As Integer
    Private _idProceso As Integer
    Private _listIdServicio As ArrayList
    Private _listNumeroRadicado As ArrayList
    Private _idUsuarioConsulta As Integer


    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idServicio As Integer)
        Me.New()
        _idServicioMensajeria = idServicio
        CargarDatos()
    End Sub

    Public Sub New(ByVal idServicio As Integer, ByVal idEstadoNovedad As Integer)
        Me.New()
        _idServicioMensajeria = idServicio
        _idEstadoNovedad = idEstadoNovedad
        CargarDatos()
    End Sub

    Public Sub New(ByVal idServicio As Integer, ByVal idEstadoNovedad As Integer, ByVal idProceso As Integer)
        Me.New()
        _idServicioMensajeria = idServicio
        _idEstadoNovedad = idEstadoNovedad
        _idProceso = idProceso
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As NovedadServicioMensajeria
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As NovedadServicioMensajeria)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdServicioMensajeria() As Integer
        Get
            Return _idServicioMensajeria
        End Get
        Set(ByVal value As Integer)
            _idServicioMensajeria = value
        End Set
    End Property

    Public Property IdEstadoNovedad() As Integer
        Get
            Return _idEstadoNovedad
        End Get
        Set(ByVal value As Integer)
            _idEstadoNovedad = value
        End Set
    End Property

    Public Property NumeroRadicado() As Long
        Get
            Return _numeroRadicado
        End Get
        Set(ByVal value As Long)
            _numeroRadicado = value
        End Set
    End Property

    Public Property FechaAgendaInicio() As Date
        Get
            Return _fechaAgendaInicio
        End Get
        Set(ByVal value As Date)
            _fechaAgendaInicio = value
        End Set
    End Property

    Public Property FechaAgendaFin() As Date
        Get
            Return _fechaAgendaFin
        End Get
        Set(ByVal value As Date)
            _fechaAgendaFin = value
        End Set
    End Property

    Public Property IdCiudad() As Integer
        Get
            Return _idCiudad
        End Get
        Set(ByVal value As Integer)
            _idCiudad = value
        End Set
    End Property

    Public Property IdBodega() As Integer
        Get
            Return _idBodega
        End Get
        Set(ByVal value As Integer)
            _idBodega = value
        End Set
    End Property

    Public Property ClienteVIP() As Enumerados.EstadoBinario
        Get
            Return _clienteVIP
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _clienteVIP = value
        End Set
    End Property

    Public Property Urgente() As Enumerados.EstadoBinario
        Get
            Return _urgente
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _urgente = value
        End Set
    End Property

    Public Property IdPrioridad() As Integer
        Get
            Return _idPrioridad
        End Get
        Set(ByVal value As Integer)
            _idPrioridad = value
        End Set
    End Property

    Property IdTipoServicio As String
        Get
            Return _idTipoServicio
        End Get
        Set(value As String)
            _idTipoServicio = value
        End Set
    End Property

    Public Property IdProceso As Integer
        Get
            Return _idProceso
        End Get
        Set(value As Integer)
            _idProceso = value
        End Set
    End Property

    Public Property ListaNumeroRadicado() As ArrayList
        Get
            If _listNumeroRadicado Is Nothing Then _listNumeroRadicado = New ArrayList
            Return _listNumeroRadicado
        End Get
        Set(ByVal value As ArrayList)
            _listNumeroRadicado = value
        End Set
    End Property

    Public Property ListaIdServicio As ArrayList
        Get
            Return _listIdServicio
        End Get
        Set(value As ArrayList)
            _listIdServicio = value
        End Set
    End Property

    Public Property IdUsuarioConsulta As Integer
        Get
            Return _idUsuarioConsulta
        End Get
        Set(value As Integer)
            _idUsuarioConsulta = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miNovedad As Type = GetType(NovedadServicioMensajeria)
        Dim pInfo As PropertyInfo

        For Each pInfo In miNovedad.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As NovedadServicioMensajeria)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As NovedadServicioMensajeria)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As NovedadServicioMensajeriaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As NovedadServicioMensajeria)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idNovedad As Integer) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), NovedadServicioMensajeria)
                If .IdNovedad = idNovedad Then
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
        Dim drAux As DataRow
        Dim miNovedad As NovedadServicioMensajeria

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miNovedad = CType(Me.InnerList(index), NovedadServicioMensajeria)
            If miNovedad IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(NovedadServicioMensajeria).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miNovedad, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = Me._idServicioMensajeria
                If Me._idEstadoNovedad > 0 Then .SqlParametros.Add("@idEstadoNovedad", SqlDbType.Int).Value = Me._idEstadoNovedad
                If Me._numeroRadicado > 0 Then .SqlParametros.Add("@numeroRadicado", SqlDbType.BigInt).Value = Me._numeroRadicado
                If Me._fechaAgendaInicio <> Date.MinValue And Me._fechaAgendaFin <> Date.MinValue Then
                    .SqlParametros.Add("@fechaAgendaInicio", SqlDbType.DateTime).Value = Me._fechaAgendaInicio
                    .SqlParametros.Add("@fechaAgendaFin", SqlDbType.DateTime).Value = Me._fechaAgendaFin
                End If
                If Me._idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = Me._idCiudad
                If Me._idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = Me._idBodega
                If Me._clienteVIP <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@clienteVIP", SqlDbType.Bit).Value = IIf(Me._clienteVIP = Enumerados.EstadoBinario.Activo, 1, 0)
                If Me._urgente <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@urgente", SqlDbType.Bit).Value = IIf(Me._urgente = Enumerados.EstadoBinario.Activo, 1, 0)
                If Me._idPrioridad > 0 Then .SqlParametros.Add("@idPrioridad", SqlDbType.Int).Value = Me._idPrioridad
                If Me._idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = Me._idTipoServicio
                If Me._idProceso > 0 Then .SqlParametros.Add("@idProceso", SqlDbType.Int).Value = Me._idProceso
                If _listNumeroRadicado IsNot Nothing AndAlso _listNumeroRadicado.Count > 0 Then .SqlParametros.Add("@listaNumeroRadicado", SqlDbType.VarChar).Value = Join(_listNumeroRadicado.ToArray, ",")
                If _listIdServicio IsNot Nothing AndAlso _listIdServicio.Count > 0 Then .SqlParametros.Add("@listaIdServicio", SqlDbType.VarChar).Value = Join(_listIdServicio.ToArray, ",")
                If Me._idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = Me._idUsuarioConsulta

                .ejecutarReader("ObtenerNovedadServicioMensajeria", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim laNovedad As NovedadServicioMensajeria

                    While .Reader.Read
                        laNovedad = New NovedadServicioMensajeria
                        Integer.TryParse(.Reader("idNovedad").ToString, laNovedad.IdNovedad)
                        Integer.TryParse(.Reader("idServicioMensajeria").ToString, laNovedad.IdServicioMensajeria)
                        Integer.TryParse(.Reader("idTipoNovedad").ToString, laNovedad.IdTipoNovedad)
                        laNovedad.TipoNovedad = .Reader("tipoNovedad").ToString
                        Integer.TryParse(.Reader("idUsuario").ToString, laNovedad.IdUsuario)
                        laNovedad.UsuarioRegistra = .Reader("usuarioRegistra").ToString
                        laNovedad.FechaRegistro = CDate(.Reader("fechaRegistro"))
                        laNovedad.FechaModificacion = CDate(.Reader("fechaModificacion"))
                        Integer.TryParse(.Reader("idEstado").ToString, laNovedad.IdEstado)
                        laNovedad.Estado = .Reader("estado").ToString
                        laNovedad.Observacion = .Reader("observacion")
                        Integer.TryParse(.Reader("numeroRadicado").ToString, laNovedad.NumeroRadicado)
                        laNovedad.ComentarioEspecifico = .Reader("comentarioEspecifico").ToString
                        laNovedad.Registrado = True
                        laNovedad.NombreCliente = .Reader("nombre").ToString
                        laNovedad.Identificacion = .Reader("identicacion").ToString
                        laNovedad.NombreContacto = .Reader("nombreAutorizado").ToString
                        laNovedad.Direccion = .Reader("direccion").ToString
                        laNovedad.Telefono = .Reader("telefono").ToString
                        If Not IsDBNull(.Reader("fechaAgenda")) Then laNovedad.FechaAgenda = CDate(.Reader("fechaAgenda"))
                        Integer.TryParse(.Reader("idTipoServicio").ToString, laNovedad.IdTipoServicio)
                        laNovedad.TipoServicio = .Reader("tipoServicio").ToString
                        laNovedad.Consultor = .Reader("consultor").ToString

                        _cargado = True
                        Me.InnerList.Add(laNovedad)
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
