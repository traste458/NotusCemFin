Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class InfoCapacidadEntregaServicioMensajeriaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idRegistro As Integer
    Private _idBodega As Integer
    Private _idCiudad As Integer
    Private _idUsuario As Integer
    Private _fechaInicial As Date
    Private _fechaFinal As Date
    Private _idJornada As Integer
    Private _idAgrupacion As Integer
    Private _cargado As Boolean
    Private _idEmpresa As Integer
    Private _resultado As New InfoResultado
    Private _idCampania As Integer

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idRegistro As Integer)
        Me.New()
        _idRegistro = idRegistro
        CargarDatos()
    End Sub

    Public Sub New(ByVal fecha As Date)
        Me.New()
        _fechaInicial = fecha
        CargarDatos()
    End Sub

    Public Sub New(ByVal fechaInicial As Date, ByVal fechaFinal As Date)
        Me.New()
        _fechaInicial = fechaInicial
        _fechaFinal = fechaFinal
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As InfoCapacidadEntregaServicioMensajeria
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As InfoCapacidadEntregaServicioMensajeria)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdEmpresa() As Integer
        Get
            Return _idEmpresa
        End Get
        Set(ByVal value As Integer)
            _idEmpresa = value
        End Set
    End Property

    Public Property IdRegistro() As Integer
        Get
            Return _idRegistro
        End Get
        Set(ByVal value As Integer)
            _idRegistro = value
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

    Public Property idUsuario() As Integer
        Get
            Return _idUsuario
        End Get
        Set(value As Integer)
            _idUsuario = value
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

    Public Property FechaInicial() As Date
        Get
            Return _fechaInicial
        End Get
        Set(ByVal value As Date)
            _fechaInicial = value
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

    Public Property IdJornada() As Integer
        Get
            Return _idJornada
        End Get
        Set(ByVal value As Integer)
            _idJornada = value
        End Set
    End Property

    Public Property IdAgrupacion() As Integer
        Get
            Return _idAgrupacion
        End Get
        Set(ByVal value As Integer)
            _idAgrupacion = value
        End Set
    End Property

    Public Property Resultado() As InfoResultado
        Get
            Return _resultado
        End Get
        Set(ByVal value As InfoResultado)
            _resultado = value
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
        Dim miInfoCapacidadEntregaServicioMensajeria As Type = GetType(InfoCapacidadEntregaServicioMensajeria)
        Dim pInfo As PropertyInfo

        For Each pInfo In miInfoCapacidadEntregaServicioMensajeria.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As InfoCapacidadEntregaServicioMensajeria)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As InfoCapacidadEntregaServicioMensajeria)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As InfoCapacidadEntregaServicioMensajeriaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As InfoCapacidadEntregaServicioMensajeria)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal fecha As Date) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), InfoCapacidadEntregaServicioMensajeria)
                If .Fecha = fecha Then
                    indice = index
                    Exit For
                End If
            End With
        Next
        Return indice
    End Function

    Public Function ItemPorIdentificador(ByVal identificador As Integer) As InfoCapacidadEntregaServicioMensajeria
        Dim resultado As InfoCapacidadEntregaServicioMensajeria
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), InfoCapacidadEntregaServicioMensajeria)
                If .IdRegistro = identificador Then
                    resultado = CType(Me.InnerList(index), InfoCapacidadEntregaServicioMensajeria)
                    Exit For
                End If
            End With
        Next
        Return resultado
    End Function

    Public Function GenerarDataTable() As DataTable
        If Not _cargado Then CargarDatos()
        Dim dtAux As DataTable = CrearEstructuraDeTabla()
        Dim drAux As DataRow
        Dim info As InfoCapacidadEntregaServicioMensajeria

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            info = CType(Me.InnerList(index), InfoCapacidadEntregaServicioMensajeria)
            If info IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(InfoCapacidadEntregaServicioMensajeria).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(info, Nothing)
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
                If Me._idRegistro > 0 Then .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = Me._idRegistro
                If Me._idEmpresa > 0 Then .SqlParametros.Add("@idEmpresa", SqlDbType.Int).Value = Me._idEmpresa
                If Me._idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = Me._idBodega
                If Me._idCiudad > 0 Then .SqlParametros.Add("@idCiudad", SqlDbType.Int).Value = Me._idCiudad
                If Me._idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                If Me._fechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = _fechaInicial
                If Me._fechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = _fechaFinal
                If Me._idJornada > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = Me._idJornada
                If Me._idAgrupacion > 0 Then .SqlParametros.Add("@idAgrupacion", SqlDbType.Int).Value = Me._idAgrupacion
                If Me._idCampania > 0 Then .SqlParametros.Add("@idCampania", SqlDbType.Int).Value = Me._idCampania

                .ejecutarReader("ObtenerInfoCapacidadEntregaCEM", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim info As InfoCapacidadEntregaServicioMensajeria

                    While .Reader.Read
                        info = New InfoCapacidadEntregaServicioMensajeria
                        info.CargarResultadoConsulta(.Reader)
                        _cargado = True
                        Me.InnerList.Add(info)
                    End While
                    .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub
    Public Function ReporteCapacidadesEntrega(ByVal nombreArchivo As String, ByVal rutaPlantilla As String) As InfoResultado
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._fechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = _fechaInicial
                If Me._fechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = _fechaFinal
                If Me._idJornada > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = Me._idJornada
                If Me._idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = Me._idBodega
                If Me._idAgrupacion > 0 Then .SqlParametros.Add("@idAgrupacion", SqlDbType.Int).Value = Me._idAgrupacion
                _resultado = .GenerarArchivoExcel("ReporteCapacidadEntregaCEM", nombreArchivo, CommandType.StoredProcedure, rutaPlantilla, "ReporteCapacidadDeEntrega", 3)
            End With
            Return _resultado
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Function
    Public Function ReporteCapacidadesEntrega() As DataTable
        Dim dbManager As New LMDataAccess
        Dim dt As DataTable
        Try
            Me.Clear()
            With dbManager
                If Me._fechaInicial > Date.MinValue Then .SqlParametros.Add("@fechaInicial", SqlDbType.Date).Value = _fechaInicial
                If Me._fechaFinal > Date.MinValue Then .SqlParametros.Add("@fechaFinal", SqlDbType.Date).Value = _fechaFinal
                If Me._idJornada > 0 Then .SqlParametros.Add("@idJornada", SqlDbType.Int).Value = Me._idJornada
                If Me._idBodega > 0 Then .SqlParametros.Add("@idBodega", SqlDbType.Int).Value = Me._idBodega
                If Me._idAgrupacion > 0 Then .SqlParametros.Add("@idAgrupacion", SqlDbType.Int).Value = Me._idAgrupacion
                If Me._idEmpresa > 0 Then .SqlParametros.Add("@idEmpresa", SqlDbType.Int).Value = Me._idEmpresa

                dt = .EjecutarDataTable("ReporteCapacidadEntregaCEM", CommandType.StoredProcedure)
            End With
            Return dt
        Catch ex As Exception
            If dbManager IsNot Nothing AndAlso dbManager.EstadoTransaccional Then dbManager.AbortarTransaccion()
            Throw New Exception(ex.Message, ex)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Function
#End Region

End Class
