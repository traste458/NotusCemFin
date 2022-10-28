Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports System.Web

Public Class UnidadNegocioColeccion
    Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

    Private _idUnidadNegocio As Short
    Private _idClienteExterno As Short
    Private _activo As Nullable(Of Boolean)
    Private _idTipoUnidadNegocio As Integer

    Private _cargado As Boolean

    'Filtros externos
    Private _idDocumento As Short

#End Region

#Region "Propiedades"

    Default Public Property Item(ByVal index As Integer) As UnidadNegocio
        Get
            Return Me.InnerList.Item(index)
        End Get
        Set(value As UnidadNegocio)
            If value IsNot Nothing Then
                Me.InnerList.Item(index) = value
            Else
                Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
            End If
        End Set
    End Property

    Public Property IdUnidadNegocio As Short
        Get
            Return _idUnidadNegocio
        End Get
        Set(value As Short)
            _idUnidadNegocio = value
        End Set
    End Property

    Public Property IdClienteExterno As Short
        Get
            Return _idClienteExterno
        End Get
        Set(value As Short)
            _idClienteExterno = value
        End Set
    End Property

    Public Property Activo As Nullable(Of Boolean)
        Get
            Return _activo
        End Get
        Set(value As Nullable(Of Boolean))
            _activo = value
        End Set
    End Property

    Public Property IdDocumento As Short
        Get
            Return _idDocumento
        End Get
        Set(value As Short)
            _idDocumento = value
        End Set
    End Property

    Public Property IdTipoUnidadNegocio As Integer
        Get
            Return _idTipoUnidadNegocio
        End Get
        Set(value As Integer)
            _idTipoUnidadNegocio = value
        End Set
    End Property

#End Region

#Region "Constructores"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal activo As Boolean)
        MyBase.New()
        _activo = activo
        CargarDatos()
    End Sub

    Public Sub New(ByVal idDocumento As Short)
        MyBase.New()
        _idDocumento = idDocumento
        CargarDatos()
    End Sub

#End Region

#Region "Métodos Privados"

    Private Function CrearEstructuraDeTabla() As DataTable
        Dim dtAux As New DataTable
        Dim miObj As Type = GetType(UnidadNegocio)
        Dim pInfo As PropertyInfo

        For Each pInfo In miObj.GetProperties
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

    Public Sub Insertar(ByVal posicion As Integer, ByVal valor As UnidadNegocio)
        Me.InnerList.Insert(posicion, valor)
    End Sub

    Public Sub Adicionar(ByVal valor As UnidadNegocio)
        Me.InnerList.Add(valor)
    End Sub

    Public Sub AdicionarRango(ByVal rango As UnidadNegocioColeccion)
        Me.InnerList.AddRange(rango)
    End Sub

    Public Sub Remover(ByVal valor As UnidadNegocio)
        With Me.InnerList
            If .Contains(valor) Then .Remove(valor)
        End With
    End Sub

    Public Sub RemoverDe(ByVal index As Integer)
        Me.InnerList.RemoveAt(index)
    End Sub

    Public Function IndiceDe(ByVal idUnidadNegocio As Short) As Integer
        Dim indice As Integer = -1
        For index As Integer = 0 To Me.InnerList.Count - 1
            With CType(Me.InnerList(index), UnidadNegocio)
                If .IdUnidadNegocio = idUnidadNegocio Then
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
        Try
            Dim drAux As DataRow
            Dim miDetalle As UnidadNegocio

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miDetalle = CType(Me.InnerList(index), UnidadNegocio)
                If miDetalle IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(UnidadNegocio).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                        ElseIf pInfo.PropertyType.Namespace = "ILSBusinessLayer.Enumerados" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miDetalle, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
        Return dtAux
    End Function

    Private Sub CargarDatos()
        Using dbManager As New LMDataAccess
            With dbManager
                Try
                    .SqlParametros.Clear()
                    Dim idUsuarioConsulta As Integer = 0
                    Dim idPerfilUsuario As Integer = 0

                    If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing Then
                        If Not EsNuloOVacio(HttpContext.Current.Session("usxp001")) Then Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuarioConsulta)
                    End If

                    If _idUnidadNegocio > 0 Then .SqlParametros.Add("@idUnidadNegocio", SqlDbType.SmallInt).Value = _idUnidadNegocio
                    If _idClienteExterno > 0 Then .SqlParametros.Add("@idClienteExterno", SqlDbType.Int).Value = _idClienteExterno
                    If _activo IsNot Nothing Then .SqlParametros.Add("@activo", SqlDbType.Bit).Value = _activo
                    If _idTipoUnidadNegocio > 0 Then .SqlParametros.Add("@idTipoUnidadNegocio", SqlDbType.Int).Value = _idTipoUnidadNegocio

                    If _idDocumento > 0 Then .SqlParametros.Add("@idDOcumento", SqlDbType.SmallInt).Value = _idDocumento
                    If idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = idUsuarioConsulta

                    .ejecutarReader("ObtenerUnidadNegocio", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elDetalle As UnidadNegocio

                        While .Reader.Read
                            If .Reader.HasRows Then
                                elDetalle = New UnidadNegocio
                                Integer.TryParse(.Reader("idUnidadNegocio"), elDetalle.IdUnidadNegocio)
                                Integer.TryParse(.Reader("idClienteExterno"), elDetalle.IdClienteExterno)
                                elDetalle.Nombre = .Reader("nombre")
                                elDetalle.Activo = .Reader("activo")
                                elDetalle.Codigo = IIf(IsDBNull(.Reader("codigo")), String.Empty, .Reader("codigo"))
                                If Not IsDBNull(.Reader("idTipoUnidadNegocio")) Then Integer.TryParse(.Reader("idTipoUnidadNegocio"), elDetalle.IdTipoUnidadNegocio)
                                If Not IsDBNull(.Reader("tipoUnidadNegocio")) Then elDetalle.TipoUnidadNegocio = .Reader("tipoUnidadNegocio")
                                _cargado = True
                                Me.InnerList.Add(elDetalle)
                            End If
                        End While
                        If Not .Reader.IsClosed Then .Reader.Close()
                    End If
                Catch ex As Exception
                    Throw ex
                End Try
            End With
        End Using
    End Sub

#End Region

End Class
