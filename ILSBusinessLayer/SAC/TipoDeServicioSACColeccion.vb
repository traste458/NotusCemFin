Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.Enumerados

Namespace SAC

    Public Class TipoDeServicioSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idTipoServicio As ArrayList
        Private _idClaseServicio As ArrayList
        Private _descripcion As String
        Private _activo As EstadoBinario

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _descripcion = ""
            _activo = EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As TipoDeServicioSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As TipoDeServicioSAC)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public ReadOnly Property IdTipoServicio() As ArrayList
            Get
                If _idTipoServicio Is Nothing Then _idTipoServicio = New ArrayList
                Return _idTipoServicio
            End Get
        End Property

        Public ReadOnly Property IdClaseServicio() As ArrayList
            Get
                If _idClaseServicio Is Nothing Then _idClaseServicio = New ArrayList
                Return _idClaseServicio
            End Get
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property Activo() As EstadoBinario
            Get
                Return _activo
            End Get
            Set(ByVal value As EstadoBinario)
                _activo = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miTipo As Type = GetType(TipoDeServicioSAC)
            Dim pInfo As PropertyInfo

            For Each pInfo In miTipo.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoDeServicioSAC)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As TipoDeServicioSAC)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub Remover(ByVal valor As TipoDeServicioSAC)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function GenerarDataTable() As DataTable
            Dim dtAux As DataTable = CrearEstructuraDeTabla()
            Dim drAux As DataRow
            Dim miTipo As TipoDeServicioSAC

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miTipo = CType(Me.InnerList(index), TipoDeServicioSAC)
                If miTipo IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(TipoDeServicioSAC).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miTipo, Nothing)
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
                Dim idPerfil As Integer
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                Me.Clear()
                With dbManager
                    If usuarioUnidad.IdUnidadNegocio > 0 Then _
                        .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                    If Me._idTipoServicio IsNot Nothing AndAlso Me._idTipoServicio.Count > 0 Then _
                        .SqlParametros.Add("@listaIdTipoServicio", SqlDbType.VarChar, 1000).Value = Join(Me._idTipoServicio.ToArray, ",")
                    If Me._idClaseServicio IsNot Nothing AndAlso Me._idClaseServicio.Count > 0 Then _
                        .SqlParametros.Add("@listaIdClaseServicio", SqlDbType.VarChar, 1000).Value = Join(Me._idClaseServicio.ToArray, ",")
                    If Me._descripcion IsNot Nothing AndAlso Me._descripcion.Trim.Length > 0 Then _
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = Me._descripcion
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ConsultarTipoDeServicioSAC", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim elTipo As TipoDeServicioSAC
                        While .Reader.Read
                            elTipo = New TipoDeServicioSAC
                            elTipo.CargarResultadoConsulta(.Reader)
                            Me.InnerList.Add(elTipo)
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
                dtAux = dbManager.EjecutarDataTable("ConsultarTipoDeServicioSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace