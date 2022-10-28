Imports LMDataAccessLayer
Imports ILSBusinessLayer.Enumerados

Namespace SAC

    Public Class ClaseDeServicioSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idClase As ArrayList
        Private _idUnidadNegocio As Byte
        Private _codigo As String
        Private _descripcion As String
        Private _activo As EstadoBinario

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _codigo = ""
            _descripcion = ""
            _activo = EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As ClaseDeServicioSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As ClaseDeServicioSAC)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public ReadOnly Property IdClase() As ArrayList
            Get
                If _idClase Is Nothing Then _idClase = New ArrayList
                Return _idClase
            End Get
        End Property

        Public Property IdUnidadNegocio() As Byte
            Get
                Return _idUnidadNegocio
            End Get
            Set(ByVal value As Byte)
                _idUnidadNegocio = value
            End Set
        End Property

        Public ReadOnly Property Codigo() As String
            Get
                Return _codigo
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

#Region "Métodos Públicos"

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ClaseDeServicioSAC)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As ClaseDeServicioSAC)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub Remover(ByVal valor As ClaseDeServicioSAC)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function GenerarDataTable() As DataTable
            Dim dtAux As New DataTable
            Dim drAux As DataRow
            Dim miClase As ClaseDeServicioSAC

            With dtAux
                .Columns.Add("idClaseServicio", GetType(Short))
                .Columns.Add("idUnidadNegocio", GetType(Byte))
                .Columns.Add("codigo", GetType(String))
                .Columns.Add("descripcion", GetType(String))
                .Columns.Add("tiempoRespuesta", GetType(Short))
                .Columns.Add("activo", GetType(SByte))
            End With

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miClase = CType(Me.InnerList(index), ClaseDeServicioSAC)
                If miClase IsNot Nothing Then
                    drAux("idClaseServicio") = miClase.IdClase
                    drAux("idUnidadNegocio") = miClase.IdUnidadNegocio
                    drAux("codigo") = miClase.Codigo
                    drAux("descripcion") = miClase.Descripcion
                    drAux("tiempoRespuesta") = miClase.TiempoMaximoParaRespuesta
                    drAux("activo") = IIf(miClase.Activo, 1, 0)
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
                    If Me._idClase IsNot Nothing AndAlso Me._idClase.Count > 0 Then _
                        .SqlParametros.Add("@listaIdClase", SqlDbType.VarChar, 1000).Value = Join(Me._idClase.ToArray, ",")
                    If Me._codigo IsNot Nothing AndAlso Me._codigo.Trim.Length > 0 Then _
                        .SqlParametros.Add("@codigo", SqlDbType.VarChar, 8).Value = Me._codigo
                    If Me._descripcion IsNot Nothing AndAlso Me._descripcion.Trim.Length > 0 Then _
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = Me._descripcion
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ConsultarClaseDeServicioSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim miClase As ClaseDeServicioSAC
                        Dim idClase As Short
                        While .Reader.Read
                            miClase = New ClaseDeServicioSAC
                            Short.TryParse(.Reader("idClaseServicio").ToString, idClase)
                            miClase.EstablecerIdentificador(idClase)
                            miClase.Codigo = .Reader("codigo").ToString
                            miClase.Descripcion = .Reader("descripcion").ToString
                            Integer.TryParse(.Reader("tiempoRespuesta").ToString, miClase.TiempoMaximoParaRespuesta)
                            Boolean.TryParse(.Reader("activo").ToString, miClase.Activo)
                            miClase.MarcarComoRegistrado()
                            Me.InnerList.Add(miClase)
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
            Dim idPerfil As Integer
            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                dbManager.SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                dtAux = dbManager.EjecutarDataTable("ConsultarClaseDeServicioSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace