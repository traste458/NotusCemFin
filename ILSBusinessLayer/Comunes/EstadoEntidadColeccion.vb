Imports LMDataAccessLayer
Imports System.Reflection

Public Class EstadoEntidadColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idEstado As Short
    Private _idEntidad As Short
    Private _dt As DataTable
    Private _listaEstado As String
#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
        _dt = CrearEstructuraDeTabla()
    End Sub

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As EstadoEntidad
        Get
            Return Me.Item(index)
        End Get
        Set(ByVal value As EstadoEntidad)
            If value IsNot Nothing Then
                Me.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o sin datos a la colección.")
            End If
        End Set
    End Property

    Public ReadOnly Property IdEstado() As Short
        Get
            Return _idEstado
        End Get
    End Property

    Public Property IdEntidad() As Short
        Get
            Return _idEntidad
        End Get
        Set(ByVal value As Short)
            _idEntidad = value
        End Set
    End Property

    Public Property ListaEstado As String
        Get
            Return _listaEstado
        End Get
        Set(value As String)
            _listaEstado = value
        End Set
    End Property

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim tipo As Type = GetType(EstadoEntidad)
        Dim pInfo As PropertyInfo

        For Each pInfo In tipo.GetProperties
            With dtAux
                .Columns.Add(pInfo.Name, pInfo.PropertyType)
            End With
        Next

        Return dtAux
    End Function

#End Region

#Region "Métodos Públicos"

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As EstadoEntidad)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As EstadoEntidad)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub Remover(ByVal valor As EstadoEntidad)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function GenerarDataTable() As DataTable
        Dim drAux As DataRow
        Dim elEstado As EstadoEntidad

        For index As Integer = 0 To Me.InnerList.Count - 1
            drAux = _dt.NewRow
            elEstado = CType(Me.InnerList(index), EstadoEntidad)
            If elEstado IsNot Nothing Then
                For Each pInfo As PropertyInfo In GetType(EstadoEntidad).GetProperties
                    drAux(pInfo.Name) = pInfo.GetValue(elEstado, Nothing)
                Next
                _dt.Rows.Add(drAux)
            End If
        Next

        Return _dt
    End Function

    Public Sub CargarDatos()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Me._idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.SmallInt).Value = Me._idEstado
                If Me._idEntidad > 0 Then .SqlParametros.Add("@idEntidad", SqlDbType.SmallInt).Value = Me._idEntidad
                .ejecutarReader("ConsultarEstadoEntidad", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim elEstado As EstadoEntidad
                    Dim idEstado As Short
                    While .Reader.Read
                        elEstado = New EstadoEntidad
                        Short.TryParse(.Reader("idEstado").ToString, idEstado)
                        elEstado.EstablecerIdentificador(idEstado)
                        elEstado.Nombre = .Reader("nombre").ToString
                        Short.TryParse(.Reader("idEntidad").ToString, elEstado.IdEntidad)
                        elEstado.EstablecerEntidad(.Reader("entidad").ToString)
                        Me.InnerList.Add(elEstado)
                    End While
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

    Public Sub CargarDatosListaEstado()
        Dim dbManager As New LMDataAccess
        Try
            Me.Clear()
            With dbManager
                If Not String.IsNullOrEmpty(Me._listaEstado) Then .SqlParametros.Add("@listaEstado", SqlDbType.VarChar).Value = Me._listaEstado
                .ejecutarReader("ObtenerEstadoEntidadPorListaEstados", CommandType.StoredProcedure)
                If .Reader IsNot Nothing Then
                    Dim elEstado As EstadoEntidad
                    Dim idEstado As Short
                    While .Reader.Read
                        elEstado = New EstadoEntidad
                        Short.TryParse(.Reader("idEstado").ToString, idEstado)
                        elEstado.EstablecerIdentificador(idEstado)
                        elEstado.Nombre = .Reader("nombre").ToString
                        Short.TryParse(.Reader("idEntidad").ToString, elEstado.IdEntidad)
                        elEstado.EstablecerEntidad(.Reader("entidad").ToString)
                        Me.InnerList.Add(elEstado)
                    End While
                    .Reader.Close()
                End If
            End With
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try
    End Sub

#End Region

#Region "Métodos Compartidos"

    Public Shared Function ObtenerTodosEnDataTable() As DataTable
        Dim dtAux As New DataTable
        Dim dbManager As New LMDataAccess

        Try
            dtAux = dbManager.EjecutarDataTable("ConsultarEstadoEntidad", CommandType.StoredProcedure)
        Finally
            If dbManager IsNot Nothing Then dbManager.Dispose()
        End Try

        Return dtAux
    End Function

#End Region

End Class

