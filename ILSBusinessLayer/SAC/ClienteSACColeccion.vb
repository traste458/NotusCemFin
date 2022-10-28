Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace SAC

    Public Class ClienteSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idCliente As ArrayList
        Private _idUnidadNegocio As Byte
        Private _idTipo As ArrayList
        Private _nombre As String
        Private _activo As EstadoBinario

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
            _activo = EstadoBinario.NoEstablecido
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As ClienteSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As ClienteSAC)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public ReadOnly Property IdCliente() As ArrayList
            Get
                If _idCliente Is Nothing Then _idCliente = New ArrayList
                Return _idCliente
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

        Public ReadOnly Property IdTipo() As ArrayList
            Get
                If _idTipo Is Nothing Then _idTipo = New ArrayList
                Return _idTipo
            End Get
        End Property

        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ClienteSAC)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As ClienteSAC)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub Remover(ByVal valor As ClienteSAC)
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
            Dim elCliente As ClienteSAC

            With dtAux
                .Columns.Add("idCliente", GetType(Short))
                .Columns.Add("nombre", GetType(String))
                .Columns.Add("idTipoCliente", GetType(Short))
                .Columns.Add("tipo", GetType(String))
                .Columns.Add("activo", GetType(SByte))
            End With

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                elCliente = CType(Me.InnerList(index), ClienteSAC)
                If elCliente IsNot Nothing Then
                    drAux("idCliente") = elCliente.IdCliente
                    drAux("nombre") = elCliente.Nombre
                    drAux("idTipoCliente") = elCliente.IdTipo
                    drAux("tipo") = elCliente.Tipo
                    drAux("activo") = IIf(elCliente.Activo, 1, 0)
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
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                    If Me._idCliente IsNot Nothing AndAlso Me._idCliente.Count > 0 Then _
                        .SqlParametros.Add("@listaIdCliente", SqlDbType.VarChar, 1000).Value = Join(Me._idCliente.ToArray, ",")
                    If Me._idTipo IsNot Nothing AndAlso Me._idTipo.Count > 0 Then _
                        .SqlParametros.Add("@listaIdTipo", SqlDbType.VarChar, 1000).Value = Join(Me._idTipo.ToArray, ",")
                    If Me._nombre IsNot Nothing AndAlso Me._nombre.Trim.Length > 0 Then _
                        .SqlParametros.Add("@nombre", SqlDbType.VarChar, 50).Value = Me._nombre
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ConsultarClienteSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elCliente As ClienteSAC
                        Dim idCliente As Short
                        While .Reader.Read
                            elCliente = New ClienteSAC
                            Short.TryParse(.Reader("idCliente").ToString, idCliente)
                            elCliente.EstablecerIdentificador(idCliente)
                            elCliente.Nombre = .Reader("nombre").ToString
                            Byte.TryParse(.Reader("idUnidadNegocio").ToString(), elCliente.IdUnidadNegocio)
                            Short.TryParse(.Reader("idTipoCliente").ToString, elCliente.IdTipo)
                            elCliente.EstablecerTipo(.Reader("tipo").ToString)
                            Boolean.TryParse(.Reader("activo").ToString, elCliente.Activo)
                            elCliente.MarcarComoRegistrado()
                            Me.InnerList.Add(elCliente)
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
                Dim idPerfil As Integer
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                dbManager.SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = usuarioUnidad.IdUnidadNegocio
                dtAux = dbManager.EjecutarDataTable("ConsultarClienteSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace