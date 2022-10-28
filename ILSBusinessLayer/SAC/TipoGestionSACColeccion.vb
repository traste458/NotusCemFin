Imports LMDataAccessLayer
Imports ILSBusinessLayer.Enumerados

Namespace SAC

    Public Class TipoGestionSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idTipo As ArrayList
        Private _idUnidadNegocio As Byte
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

        Default Public Property Item(ByVal index As Integer) As TipoGestionSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As TipoGestionSAC)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o sin datos a la colección.")
                End If
            End Set
        End Property

        Public ReadOnly Property IdTipo() As ArrayList
            Get
                If _idTipo Is Nothing Then _idTipo = New ArrayList
                Return _idTipo
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoGestionSAC)
            If valor.Registrado Then
                Me.InnerList.Insert(posicion, valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Adicionar(ByVal valor As TipoGestionSAC)
            If valor.Registrado Then
                Me.InnerList.Add(valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Remover(ByVal valor As TipoGestionSAC)
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
            Dim elTipo As TipoGestionSAC

            With dtAux
                .Columns.Add("idTipoGestion", GetType(Short))
                .Columns.Add("idUnidadNegocio", GetType(Byte))
                .Columns.Add("descripcion", GetType(String))
                .Columns.Add("activo", GetType(SByte))
            End With

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                elTipo = CType(Me.InnerList(index), TipoGestionSAC)
                If elTipo IsNot Nothing Then
                    drAux("idTipoGestion") = elTipo.IdTipo
                    drAux("idUnidadNegocio") = elTipo.IdUnidadNegocio
                    drAux("descripcion") = elTipo.Descripcion
                    drAux("activo") = IIf(elTipo.Activo, 1, 0)
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Dim idPerfil As Integer
            Try
                Me.Clear()
                With dbManager
                    If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                        Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                    Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                    Me._idUnidadNegocio = usuarioUnidad.IdUnidadNegocio
                    If Me._idTipo IsNot Nothing AndAlso Me._idTipo.Count > 0 Then _
                        .SqlParametros.Add("@listaIdTipo", SqlDbType.VarChar, 1000).Value = Join(Me._idTipo.ToArray, ",")
                    If Me._idUnidadNegocio > 0 Then _
                        .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = Me._idUnidadNegocio
                    If Me._descripcion IsNot Nothing AndAlso Me._descripcion.Trim.Length > 0 Then _
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = Me._descripcion
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ConsultarTipoDeGestion", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elTipo As TipoGestionSAC
                        Dim idTipo As Short
                        While .Reader.Read
                            elTipo = New TipoGestionSAC
                            Short.TryParse(.Reader("idTipoGestion").ToString, idTipo)
                            Byte.TryParse(.Reader("idUnidadNegocio").ToString(), elTipo.IdUnidadNegocio)
                            elTipo.EstablecerIdentificador(idTipo)
                            elTipo.Descripcion = .Reader("descripcion").ToString
                            Boolean.TryParse(.Reader("activo").ToString, elTipo.Activo)
                            elTipo.MarcarComoRegistrado()
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
            Dim idPerfil As Integer
            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                dbManager.SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                dtAux = dbManager.EjecutarDataTable("ConsultarTipoDeGestion", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace
