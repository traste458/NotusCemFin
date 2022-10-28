Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Pedidos

    Public Class TipoPedidoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idTipoPedido As Short
        Private _nombre As String
        Private _activo As EstadoBinario
        Private _arrListaTipoPedido As ArrayList
        Private _idUsuarioConsulta As Integer
        Private _idDenegacionListaOpcion As Integer
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
            _activo = EstadoBinario.Activo
            _arrListaTipoPedido = New ArrayList
        End Sub

        Public Sub New(ByVal idTipo As Short)
            Me.New()
            _idTipoPedido = idTipo
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As TipoPedido
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As TipoPedido)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdTipoPedido() As Short
            Get
                Return _idTipoPedido
            End Get
            Set(ByVal value As Short)
                _idTipoPedido = value
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

        Public Property Activo() As EstadoBinario
            Get
                Return _activo
            End Get
            Set(ByVal value As EstadoBinario)
                _activo = value
            End Set
        End Property

        Public Property ListadoTipoPedido() As ArrayList
            Get
                If _arrListaTipoPedido Is Nothing Then _arrListaTipoPedido = New ArrayList
                Return _arrListaTipoPedido
            End Get
            Set(ByVal value As ArrayList)
                _arrListaTipoPedido = value
            End Set
        End Property

        Public Property IdUsuarioConsulta() As Integer
            Get
                Return _idUsuarioConsulta
            End Get
            Set(ByVal value As Integer)
                _idUsuarioConsulta = value
            End Set
        End Property

        Public Property IdDenegacionListadoOpcion() As Integer
            Get
                Return _idDenegacionListaOpcion
            End Get
            Set(ByVal value As Integer)
                _idDenegacionListaOpcion = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miTipoPedido As Type = GetType(TipoPedido)
            Dim pInfo As PropertyInfo

            For Each pInfo In miTipoPedido.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoPedido)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As TipoPedido)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As TipoPedidoColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As TipoPedido)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idTipoPedido As Short) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), TipoPedido)
                    If .IdTipo = idTipoPedido Then
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
            Dim drAux As DataRow
            Dim miTipoPedido As TipoPedido

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miTipoPedido = CType(Me.InnerList(index), TipoPedido)
                If miTipoPedido IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(TipoPedido).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miTipoPedido, Nothing)
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
                Me.Clear()
                With dbManager
                    If Me._idTipoPedido > 0 Then _
                        .SqlParametros.Add("@idTipoPedido", SqlDbType.Int).Value = Me._idTipoPedido
                    If Me._arrListaTipoPedido IsNot Nothing AndAlso Me._arrListaTipoPedido.Count > 0 Then _
                        .SqlParametros.Add("@listaTipoPedido", SqlDbType.VarChar, 1000).Value = Join(Me._arrListaTipoPedido.ToArray, ",")
                    If Me._idUsuarioConsulta > 0 Then _
                        .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = Me._idUsuarioConsulta
                    If Me._idDenegacionListaOpcion > 0 Then _
                        .SqlParametros.Add("@idListado", SqlDbType.Int).Value = Me._idDenegacionListaOpcion

                    .ejecutarReader("ObtenerTipoPedido", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elTipoPedido As TipoPedido

                        While .Reader.Read
                            elTipoPedido = New TipoPedido

                            Short.TryParse(.Reader("idTipoPedido").ToString, elTipoPedido.IdTipo)
                            elTipoPedido.Nombre = .Reader("nombre").ToString
                            elTipoPedido.Codigo = .Reader("codigo").ToString
                            Short.TryParse(.Reader("estado").ToString, elTipoPedido.IdEstado)
                            Boolean.TryParse(.Reader("contabilizarEnCliente").ToString, elTipoPedido.ContabilizarEnCliente)
                            elTipoPedido.Registrado = True

                            Me.InnerList.Add(elTipoPedido)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
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
                dtAux = dbManager.ejecutarDataTable("ObtenerTipoPedido", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace