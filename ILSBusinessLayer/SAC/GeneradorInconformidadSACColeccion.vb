Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer

Namespace SAC

    Public Class GeneradorInconformidadSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idGenerador As ArrayList
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

        Default Public Property Item(ByVal index As Integer) As GeneradorInconformidadSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As GeneradorInconformidadSAC)
                If value IsNot Nothing AndAlso value.Registrado Then
                    Me.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o sin datos a la colección.")
                End If
            End Set
        End Property

        Public ReadOnly Property IdGenerador() As ArrayList
            Get
                If _idGenerador Is Nothing Then _idGenerador = New ArrayList
                Return _idGenerador
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As GeneradorInconformidadSAC)
            If valor.Registrado Then
                Me.InnerList.Insert(posicion, valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Adicionar(ByVal valor As GeneradorInconformidadSAC)
            If valor.Registrado Then
                Me.InnerList.Add(valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Remover(ByVal valor As GeneradorInconformidadSAC)
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
            Dim elGenerador As GeneradorInconformidadSAC

            With dtAux
                .Columns.Add("idGeneradorInconformidad", GetType(Short))
                .Columns.Add("idUnidadNegocio", GetType(Byte))
                .Columns.Add("descripcion", GetType(String))
                .Columns.Add("activo", GetType(SByte))
            End With

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                elGenerador = CType(Me.InnerList(index), GeneradorInconformidadSAC)
                If elGenerador IsNot Nothing Then
                    drAux("idGeneradorInconformidad") = elGenerador.IdGenerador
                    drAux("idUnidadNegocio") = elGenerador.IdUnidadNegocio
                    drAux("descripcion") = elGenerador.Descripcion
                    drAux("activo") = IIf(elGenerador.Activo, 1, 0)
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Dim idPerfil As Integer
            Try
                If System.Web.HttpContext.Current.Session("usxp009") IsNot Nothing Then _
                    Integer.TryParse(System.Web.HttpContext.Current.Session("usxp009").ToString(), idPerfil)
                Dim usuarioUnidad As New UsuarioPerfilUnidadNegocio(idPerfil)
                Me.Clear()
                With dbManager
                    Me._idUnidadNegocio = usuarioUnidad.IdUnidadNegocio
                    If Me._idGenerador IsNot Nothing AndAlso Me._idGenerador.Count > 0 Then _
                        .SqlParametros.Add("@listaIdGenerador", SqlDbType.VarChar, 1000).Value = Join(Me._idGenerador.ToArray, ",")
                    .SqlParametros.Add("@idUnidadNegocio", SqlDbType.TinyInt).Value = Me._idUnidadNegocio
                    If Me._descripcion IsNot Nothing AndAlso Me._descripcion.Trim.Length > 0 Then _
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = Me._descripcion
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ConsultarGeneradorInconformidadSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elGenerador As GeneradorInconformidadSAC
                        Dim idGenerador As Short
                        While .Reader.Read
                            elGenerador = New GeneradorInconformidadSAC
                            Short.TryParse(.Reader("idGeneradorInconformidad").ToString, idGenerador)
                            elGenerador.EstablecerIdentificador(idGenerador)
                            elGenerador.Descripcion = .Reader("descripcion").ToString
                            Boolean.TryParse(.Reader("activo").ToString, elGenerador.Activo)
                            elGenerador.MarcarComoRegistrado()
                            Me.InnerList.Add(elGenerador)
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
                dtAux = dbManager.EjecutarDataTable("ConsultarGeneradorInconformidadSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace

