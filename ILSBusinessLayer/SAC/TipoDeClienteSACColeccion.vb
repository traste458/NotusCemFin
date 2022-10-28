Imports LMDataAccessLayer
Imports System.Web
Imports ILSBusinessLayer.Enumerados

Namespace SAC

    Public Class TipoDeClienteSACColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idTipo As ArrayList
        Private _idTipoGestion As ArrayList
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

        Default Public Property Item(ByVal index As Integer) As TipoDeClienteSAC
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As TipoDeClienteSAC)
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

        Public ReadOnly Property IdTipoGestion() As ArrayList
            Get
                If _idTipoGestion Is Nothing Then _idTipoGestion = New ArrayList
                Return _idTipoGestion
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoDeClienteSAC)
            If valor.Registrado Then
                Me.InnerList.Insert(posicion, valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Adicionar(ByVal valor As TipoDeClienteSAC)
            If valor.Registrado Then
                Me.InnerList.Add(valor)
            Else
                Throw New Exception("No se puede adicionar un objeto nulo o sin datos a la colección.")
            End If
        End Sub

        Public Sub Remover(ByVal valor As TipoDeClienteSAC)
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
            Dim elTipo As TipoDeClienteSAC

            With dtAux
                .Columns.Add("idTipoCliente", GetType(Short))
                .Columns.Add("descripcion", GetType(String))
                .Columns.Add("activo", GetType(SByte))
            End With

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                elTipo = CType(Me.InnerList(index), TipoDeClienteSAC)
                If elTipo IsNot Nothing Then
                    drAux("idTipoCliente") = elTipo.IdTipoCliente
                    drAux("descripcion") = elTipo.Descripcion
                    drAux("activo") = IIf(elTipo.Activo, 1, 0)
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Dim dbManager As New LMDataAccess
            Dim idUsuarioConsulta As Integer = 0
            Try
                If HttpContext.Current IsNot Nothing AndAlso HttpContext.Current.Session IsNot Nothing _
                    AndAlso HttpContext.Current.Session("usxp001") IsNot Nothing Then
                    Integer.TryParse(HttpContext.Current.Session("usxp001").ToString, idUsuarioConsulta)
                End If
                Me.Clear()
                With dbManager
                    If Me._idTipo IsNot Nothing AndAlso Me._idTipo.Count > 0 Then _
                        .SqlParametros.Add("@listaIdTipo", SqlDbType.VarChar, 1000).Value = Join(Me._idTipo.ToArray, ",")
                    If Me._idTipoGestion IsNot Nothing AndAlso Me._idTipoGestion.Count > 0 Then _
                        .SqlParametros.Add("@listaIdTipoGestion", SqlDbType.VarChar, 1000).Value = Join(Me._idTipoGestion.ToArray, ",")
                    If Me._descripcion IsNot Nothing AndAlso Me._descripcion.Trim.Length > 0 Then _
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 50).Value = Me._descripcion
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = EstadoBinario.Activo, 1, 0)
                    If idUsuarioConsulta > 0 Then .SqlParametros.Add("@idUsuario", SqlDbType.Int).Value = idUsuarioConsulta
                    .ejecutarReader("ConsultarTipoDeClienteSAC", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elTipo As TipoDeClienteSAC
                        While .Reader.Read
                            elTipo = New TipoDeClienteSAC
                            Short.TryParse(.Reader("idTipoCliente").ToString, elTipo.IdTipoCliente)
                            elTipo.Descripcion = .Reader("descripcion").ToString
                            Boolean.TryParse(.Reader("activo").ToString, elTipo.Activo)
                            elTipo.Registrado = True
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
                dtAux = dbManager.EjecutarDataTable("ConsultarTipoDeClienteSAC", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace