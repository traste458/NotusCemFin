Imports ILSBusinessLayer
Imports LMDataAccessLayer

Public Class IdentificadorListaDenegadaPorSeleccion

#Region "Atributos (Campos o Filtros)"

    Private _idOpcionFuncional As Integer
    Private _idListadoSeleccionado As Integer
    Private _idListadoAfectado As Integer
    Private _identificadorSeleccionado As Integer
    Private _dtDenegaciones As DataTable
    Private _cargado As Boolean

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal idOpcionFuncional As Integer)
        Me.New()
        _idOpcionFuncional = idOpcionFuncional
        CargarDenegaciones()
    End Sub

#End Region

#Region "Propiedades"

    Public Property IdOpcionFuncional() As Integer
        Get
            Return _idOpcionFuncional
        End Get
        Set(ByVal value As Integer)
            _idOpcionFuncional = value
        End Set
    End Property

    Public Property IdListadoSeleccionado() As Integer
        Get
            Return _idListadoSeleccionado
        End Get
        Set(ByVal value As Integer)
            _idListadoSeleccionado = value
        End Set
    End Property

    Public Property IdListadoAfectado() As Integer
        Get
            Return _idListadoAfectado
        End Get
        Set(ByVal value As Integer)
            _idListadoAfectado = value
        End Set
    End Property

    Public Property IdentificadorSeleccionado() As Integer
        Get
            Return _identificadorSeleccionado
        End Get
        Set(ByVal value As Integer)
            _identificadorSeleccionado = value
        End Set
    End Property

    Public ReadOnly Property ListadoDenegaciones() As DataTable
        Get
            If _dtDenegaciones Is Nothing OrElse (Not _cargado) Then CargarDenegaciones()
            Return _dtDenegaciones
        End Get
    End Property

#End Region

#Region "Métodos Públicos"
    Public Sub CargarDenegaciones()
        Dim dbManager As New LMDataAccess
        Try
            With dbManager
                If Me._idOpcionFuncional > 0 Then .SqlParametros.Add("@idOpcionFuncional", SqlDbType.Int).Value = _idOpcionFuncional
                If Me._idListadoSeleccionado > 0 Then _
                    .SqlParametros.Add("@idListadoSeleccionado", SqlDbType.Int).Value = Me._idListadoSeleccionado
                If Me._idListadoAfectado > 0 Then _
                    .SqlParametros.Add("@idListadoAfectado", SqlDbType.Int).Value = Me._idListadoAfectado
                If Me._identificadorSeleccionado > 0 Then _
                     .SqlParametros.Add("@identificadorSeleccionado", SqlDbType.Int).Value = Me._identificadorSeleccionado

                _dtDenegaciones = .ejecutarDataTable("ObtenerIdentificadorListaDenegadoPorSeleccion", CommandType.StoredProcedure)
                _cargado = True
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

    End Sub

    Public Function ObtenerDenegacionesEnArray(ByVal idSeleccionado As Integer, _
        Optional ByVal idListadoSeleccionado As Integer = 0, Optional ByVal idListadoAfectado As Integer = 0) As ArrayList

        Dim arrAux As New ArrayList

        If idListadoSeleccionado = 0 Then idListadoSeleccionado = _idListadoSeleccionado
        If idListadoAfectado = 0 Then idListadoAfectado = _idListadoAfectado

        Dim filtro As String = "identificadorSeleccionado=" & idSeleccionado.ToString & " and idListadoSeleccion=" & _
            idListadoSeleccionado.ToString & " and idListadoAfectado=" & idListadoAfectado.ToString

        If _dtDenegaciones Is Nothing OrElse (Not _cargado) Then CargarDenegaciones()
        If _dtDenegaciones IsNot Nothing Then
            Dim drsAux() As DataRow = _dtDenegaciones.Select(filtro)
            For Each drAux As DataRow In drsAux
                arrAux.Add(drAux("identificadorDenegado"))
            Next
        End If
        Return arrAux
    End Function
#End Region

End Class
