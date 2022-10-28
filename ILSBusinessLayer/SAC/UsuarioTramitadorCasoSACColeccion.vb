Imports LMDataAccessLayer
Imports ILSBusinessLayer.Enumerados

Namespace SAC

    Public Class UsuarioTramitadorCasoSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idUsuario As Integer
        Private _nombre As String
        Private _email As String
        Private _activo As EstadoBinario

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
            _email = ""
            _idUsuario = 0
            _activo = EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As UsuarioTramitadorCasoSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As UsuarioTramitadorCasoSAC)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o sin datos a la colección.")
                End If
            End Set
        End Property

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property EMail() As String
            Get
                Return _email
            End Get
            Set(ByVal value As String)
                _email = value
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As UsuarioTramitadorCasoSAC)
            If valor.Registrado Then
                Me.InnerList.Insert(posicion, valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Adicionar(ByVal valor As UsuarioTramitadorCasoSAC)
            If valor.Registrado Then
                Me.InnerList.Add(valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Remover(ByVal valor As UsuarioTramitadorCasoSAC)
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
            Dim elUsuario As UsuarioTramitadorCasoSAC

            With dtAux
                .Columns.Add("idUsuario", GetType(Integer))
                .Columns.Add("nombre", GetType(String))
                .Columns.Add("email", GetType(String))
                .Columns.Add("idPerfil", GetType(Short))
                .Columns.Add("perfil", GetType(String))
                .Columns.Add("activo", GetType(SByte))
            End With

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                elUsuario = CType(Me.InnerList(index), UsuarioTramitadorCasoSAC)
                If elUsuario IsNot Nothing Then
                    drAux("idUsuario") = elUsuario.IdUsuario
                    drAux("nombre") = elUsuario.Nombre
                    drAux("email") = elUsuario.EMail
                    drAux("idPerfil") = elUsuario.IdPerfil
                    drAux("perfil") = elUsuario.Perfil
                    drAux("activo") = IIf(elUsuario.Activo, 1, 0)
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Dim idPerfilUnidad As Integer
            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfilUnidad)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfilUnidad)
                Me.Clear()
                With dbManager
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.VarChar, 100).Value = usuarioUnidad.IdUnidadNegocio
                    If Me._idUsuario > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = Me._idUsuario
                    If Me._nombre IsNot Nothing AndAlso Me._nombre.Trim.Length > 0 Then _
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar, 100).Value = Me._nombre
                    If Me._email IsNot Nothing AndAlso Me._email.Trim.Length > 0 Then _
                        .SqlParametros.Add("@email", SqlDbType.VarChar, 100).Value = Me._email
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ConsultarUsuarioTramitadorCasoSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elUsuario As UsuarioTramitadorCasoSAC
                        Dim idUsuario As Integer
                        Dim idPerfil As Short
                        While .Reader.Read
                            elUsuario = New UsuarioTramitadorCasoSAC
                            Integer.TryParse(.Reader("idUsuario").ToString, idUsuario)
                            elUsuario.EstablecerIdentificador(idUsuario)
                            elUsuario.EstablecerNombre(.Reader("nombre").ToString)
                            elUsuario.EstablecerEmail(.Reader("email").ToString)
                            Short.TryParse(.Reader("idPerfil").ToString, idPerfil)
                            elUsuario.EstablecerIdPerfil(idPerfil)
                            elUsuario.EstablecerPerfil(.Reader("perfil").ToString)
                            Boolean.TryParse(.Reader("activo").ToString, elUsuario.Activo)
                            elUsuario.MarcarComoRegistrado()
                            Me.InnerList.Add(elUsuario)
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
                dtAux = dbManager.EjecutarDataTable("ConsultarTipoDeClienteSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace

