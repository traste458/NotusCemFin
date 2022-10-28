Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class DetalleSerialServicioMensajeriaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idDetalle As Long
    Private _idServicio As Integer
    Private _numeroRadicado As Long
    Private _idEstadoServicio As Integer
    Private _idEstadoSerial As Integer
    Private _serialPrestamo As String
    Private _planillaLegalizacion As String
    Private _material As String
    Private _msisdn As String

    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idServicio As Integer)
        Me.New()
        _idServicio = idServicio
        CargarDatos()
    End Sub

    Public Sub New(byval idDetalle As Long)
        Me.New()
        _idDetalle = idDetalle
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As DetalleSerialServicioMensajeria
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As DetalleSerialServicioMensajeria)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdDetalle() As Long
        Get
            Return _idDetalle
        End Get
        Set(ByVal value As Long)
            _idDetalle = value
        End Set
    End Property

    Public Property IdServicio() As Integer
        Get
            Return _idServicio
        End Get
        Set(ByVal value As Integer)
            _idServicio = value
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

    Public Property idEstadoServicio() As Integer
        Get
            Return _idEstadoServicio
        End Get
        Set(ByVal value As Integer)
            _idEstadoServicio = value
        End Set
    End Property

    Public Property IdEstadoSerial As Integer
        Get
            Return _idEstadoSerial
        End Get
        Set(value As Integer)
            _idEstadoSerial = value
        End Set
    End Property

    Public Property SerialPrestamo() As String
        Get
            Return _serialPrestamo
        End Get
        Set(ByVal value As String)
            _serialPrestamo = value
        End Set
    End Property

    Public Property PlanillaLegalizacion() As String
        Get
            Return _planillaLegalizacion
        End Get
        Set(value As String)
            _planillaLegalizacion = value
        End Set
    End Property

    Public Property Material As String
        Get
            Return _material
        End Get
        Set(value As String)
            _material = value
        End Set
    End Property

    Public Property Msisdn As String
        Get
            Return _msisdn
        End Get
        Set(value As String)
            _msisdn = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miDetalleSerialServicioMensajeria As Type = GetType(DetalleSerialServicioMensajeria)
        Dim pInfo As PropertyInfo

        For Each pInfo In miDetalleSerialServicioMensajeria.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DetalleSerialServicioMensajeria)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As DetalleSerialServicioMensajeria)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As DetalleSerialServicioMensajeriaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As DetalleSerialServicioMensajeria)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal serial As String) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), DetalleSerialServicioMensajeria)
                If .Serial = serial Then
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
        Dim miDetalle As DetalleSerialServicioMensajeria

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), DetalleSerialServicioMensajeria)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(DetalleSerialServicioMensajeria).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                    ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
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
                If Me._idDetalle > 0 Then .SqlParametros.Add("@idDetalle", SqlDbType.BigInt).Value = Me._idDetalle
                If Me._idServicio > 0 Then .SqlParametros.Add("@idServicio", SqlDbType.Int).Value = Me._idServicio
                If Me._numeroRadicado > 0 Then .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = Me._numeroRadicado
                If Me._idEstadoServicio > 0 Then .SqlParametros.Add("@idEstadoServicio", SqlDbType.Int).Value = Me._idEstadoServicio
                If Me._idEstadoSerial > 0 Then .SqlParametros.Add("@idEstadoSerial", SqlDbType.Int).Value = Me._idEstadoSerial
                If Not String.IsNullOrEmpty(Me._serialPrestamo) Then .SqlParametros.Add("@serialPrestamo", SqlDbType.VarChar).Value = Me._serialPrestamo
                If Not String.IsNullOrEmpty(Me._planillaLegalizacion) Then .SqlParametros.Add("@planillaLegalizacion", SqlDbType.VarChar).Value = Me._planillaLegalizacion
                If Not String.IsNullOrEmpty(Me._material) Then .SqlParametros.Add("@material", SqlDbType.VarChar).Value = Me._material
                If Not String.IsNullOrEmpty(Me._msisdn) Then .SqlParametros.Add("@msisdn", SqlDbType.VarChar).Value = Me._msisdn
                .TiempoEsperaComando = 0
                .ejecutarReader("ObtenerDetalleSerialServicioMensajeria", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim elDetalle As DetalleSerialServicioMensajeria

                    While .Reader.Read
                        elDetalle = New DetalleSerialServicioMensajeria
                        elDetalle.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(elDetalle)
                    End While
                    If Not .Reader.IsClosed Then .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub
    Public Sub CargarDatosRadicadoEstado()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._numeroRadicado > 0 Then .SqlParametros.Add("@numRadicado", SqlDbType.BigInt).Value = Me._numeroRadicado
                If Me._idEstadoServicio > 0 Then .SqlParametros.Add("@idEstadoServicio", SqlDbType.Int).Value = Me._idEstadoServicio
                .TiempoEsperaComando = 0
                .ejecutarReader("ObtenerDetalleSerialServicioMensajeriaNumRadicadoEstados", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim elDetalle As DetalleSerialServicioMensajeria

                    While .Reader.Read
                        elDetalle = New DetalleSerialServicioMensajeria
                        elDetalle.CargarResultadoConsulta(.Reader)
                        Me.InnerList.Add(elDetalle)
                    End While
                    If Not .Reader.IsClosed Then .Reader.Close()
                End If
            End With
            _cargado = True
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

End Class
