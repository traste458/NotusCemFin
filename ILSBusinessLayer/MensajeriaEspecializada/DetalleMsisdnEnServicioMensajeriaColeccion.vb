Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Public Class DetalleMsisdnEnServicioMensajeriaColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Protected Friend _idServicioMensajeria As Long
    Protected Friend _idRegistro As Integer
    Protected Friend _idTipoServicio As Integer
    Protected Friend _msisdn As Long
    Protected Friend _cargado As Boolean
    Protected Friend _hayCambioServicio As Enumerados.EstadoBinario

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idServicio As Long)
        Me.New()
        _idServicioMensajeria = idServicio
        _hayCambioServicio = Enumerados.EstadoBinario.NoEstablecido
        CargarDatos()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As DetalleMsisdnEnServicioMensajeria
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(ByVal value As DetalleMsisdnEnServicioMensajeria)
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

    Public Property IdRegistro() As Integer
        Get
            Return _idRegistro
        End Get
        Set(ByVal value As Integer)
            _idRegistro = value
        End Set
    End Property

    Public Property IdTipoServicio() As Integer
        Get
            Return _idTipoServicio
        End Get
        Set(ByVal value As Integer)
            _idTipoServicio = value
        End Set
    End Property

    Public Property Msisdn() As Long
        Get
            Return _msisdn
        End Get
        Set(ByVal value As Long)
            _msisdn = value
        End Set
    End Property

    Public Property HayCambioServicio() As Enumerados.EstadoBinario
        Get
            Return IIf(_hayCambioServicio, Enumerados.EstadoBinario.Activo, Enumerados.EstadoBinario.Inactivo)
        End Get
        Set(ByVal value As Enumerados.EstadoBinario)
            _hayCambioServicio = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miDetalleMsisdnEnServicioMensajeria As Type = GetType(DetalleMsisdnEnServicioMensajeria)
        Dim pInfo As PropertyInfo

        For Each pInfo In miDetalleMsisdnEnServicioMensajeria.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As DetalleMsisdnEnServicioMensajeria)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As DetalleMsisdnEnServicioMensajeria)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As DetalleMsisdnEnServicioMensajeriaColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As DetalleMsisdnEnServicioMensajeria)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal msisdn As String) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), DetalleMsisdnEnServicioMensajeria)
                If .MSISDN = msisdn Then
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
        Dim miDetalle As DetalleMsisdnEnServicioMensajeria

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = dtAux.NewRow
            miDetalle = CType(Me.InnerList(index), DetalleMsisdnEnServicioMensajeria)
            If miDetalle IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(DetalleMsisdnEnServicioMensajeria).GetProperties
                    If pInfo.PropertyType.Namespace = "System" Then
                        drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                    End If
                Next
                dtAux.Rows.Add(drAux)
            End If
        Next

        Return dtAux
    End Function

    Public Function GenerarDataTableDesdeLista() As DataTable        
        Dim dtAux As DataTable
        _cargado = True
        dtAux = GenerarDataTable()
        Return dtAux
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idServicioMensajeria > 0 Then .SqlParametros.Add("@idServicioMensajeria", SqlDbType.BigInt).Value = Me._idServicioMensajeria
                If Me._idRegistro > 0 Then .SqlParametros.Add("@idRegistro", SqlDbType.Int).Value = Me._idRegistro
                If Me._idTipoServicio > 0 Then .SqlParametros.Add("@idTipoServicio", SqlDbType.Int).Value = Me._idTipoServicio
                If Me._msisdn > 0 Then .SqlParametros.Add("@msisdn", SqlDbType.BigInt).Value = Me._msisdn
                If Me._hayCambioServicio <> Enumerados.EstadoBinario.NoEstablecido Then _
                    .SqlParametros.Add("@tieneCambioServicio", SqlDbType.Bit).Value = IIf(_hayCambioServicio = Enumerados.EstadoBinario.Activo, 1, 0)
                .TiempoEsperaComando = 0
                .ejecutarReader("ObtenerDetalleMsisdnEnServicioMensajeria", CommandType.StoredProcedure)

                If .Reader IsNot Nothing Then
                    Dim elDetalle As DetalleMsisdnEnServicioMensajeria
                    While .Reader.Read
                        elDetalle = New DetalleMsisdnEnServicioMensajeria
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
