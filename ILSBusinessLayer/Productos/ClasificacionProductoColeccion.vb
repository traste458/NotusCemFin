Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Productos

    Public Class ClasificacionProductoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idClasificacion As Short
        Private _visibilidadInterna As Enumerados.EstadoBinario
        Private _visibilidadExterna As Enumerados.EstadoBinario
        Private _activa As Enumerados.EstadoBinario
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _visibilidadInterna = Enumerados.EstadoBinario.NoEstablecido
            _visibilidadExterna = Enumerados.EstadoBinario.NoEstablecido
            _activa = Enumerados.EstadoBinario.Activo
        End Sub

        Public Sub New(ByVal idClasificacion As Short)
            Me.New()
            _idClasificacion = idClasificacion
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As ClasificacionProducto
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As ClasificacionProducto)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdClasificacion() As Short
            Get
                Return _idClasificacion
            End Get
            Set(ByVal value As Short)
                _idClasificacion = value
            End Set
        End Property

        Public Property VisibilidadInterna() As Enumerados.EstadoBinario
            Get
                Return _visibilidadInterna
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _visibilidadInterna = value
            End Set
        End Property

        Public Property VisibilidadExterna() As Enumerados.EstadoBinario
            Get
                Return _visibilidadExterna
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _visibilidadExterna = value
            End Set
        End Property

        Public Property Activa() As Enumerados.EstadoBinario
            Get
                Return _activa
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _activa = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miClasificacionProducto As Type = GetType(ClasificacionProducto)
            Dim pInfo As PropertyInfo

            For Each pInfo In miClasificacionProducto.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As ClasificacionProducto)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As ClasificacionProducto)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ClasificacionProductoColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As ClasificacionProducto)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idClasificacion As Short) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), ClasificacionProducto)
                    If .IdClasificacion = idClasificacion Then
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
            Dim miClasificacionProducto As ClasificacionProducto

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miClasificacionProducto = CType(Me.InnerList(index), ClasificacionProducto)
                If miClasificacionProducto IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(ClasificacionProducto).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miClasificacionProducto, Nothing)
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
                    If _idClasificacion > 0 Then _
                        .SqlParametros.Add("@idClasificacion", SqlDbType.SmallInt).Value = Me._idClasificacion
                    If Me._visibilidadInterna <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@visibilidadInterna", SqlDbType.Bit).Value = IIf(Me._visibilidadInterna = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Me._visibilidadExterna <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@visibilidadExterna", SqlDbType.Bit).Value = IIf(Me._visibilidadExterna = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Me._activa <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activa", SqlDbType.Bit).Value = IIf(Me._activa = Enumerados.EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ConsultarListadoClasificacionProducto", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim clasificacion As ClasificacionProducto
                        While .Reader.Read
                            clasificacion = New ClasificacionProducto
                            Short.TryParse(.Reader("idClasificacion").ToString, clasificacion.IdClasificacion)
                            clasificacion.Nombre = .Reader("nombre").ToString
                            clasificacion.VisibilidadInterna = CBool(.Reader("visibilidadInterna").ToString)
                            clasificacion.VisibilidadExterna = CBool(.Reader("visibilidadExterna").ToString)
                            clasificacion.Activa = CBool(.Reader("activa").ToString)
                            clasificacion.Registrado = True

                            Me.InnerList.Add(clasificacion)
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
                dtAux = dbManager.ejecutarDataTable("ConsultarListadoClasificacionProducto", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace