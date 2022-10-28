Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Productos

    Public Class ProductoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idProducto As Integer
        Private _nombre As String
        Private _codigo As String
        Private _idTecnologia As Short
        Private _idTipoProducto As ArrayList
        Private _idFabricante As Short
        Private _idProveedor As ArrayList
        Private _activo As Enumerados.EstadoBinario
        Private _separadorProveedor As String
        Private _instruccionable As Enumerados.EstadoBinario
        Private _idClasificacionInterna As Short
        Private _idClasificacionExterna As Short
        Private _cargado As Boolean
        Private _filtroRapido As String

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
            _codigo = ""
            _activo = Enumerados.EstadoBinario.Activo
            _instruccionable = Enumerados.EstadoBinario.NoEstablecido
        End Sub

        Public Sub New(ByVal idProducto As Integer)
            Me.New()
            _idProducto = idProducto
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As Producto
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As Producto)
                If value IsNot Nothing OrElse (Not value.Registrado) Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdProducto() As Integer
            Get
                Return _idProducto
            End Get
            Set(ByVal value As Integer)
                _idProducto = value
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

        Public Property Codigo() As String
            Get
                Return _codigo
            End Get
            Set(ByVal value As String)
                _codigo = value
            End Set
        End Property

        Public Property IdTecnologia() As Short
            Get
                Return _idTecnologia
            End Get
            Set(ByVal value As Short)
                _idTecnologia = value
            End Set
        End Property

        Public ReadOnly Property IdTipoProducto() As ArrayList
            Get
                If _idTipoProducto Is Nothing Then _idTipoProducto = New ArrayList
                Return _idTipoProducto
            End Get
        End Property

        Public Property IdFabricante() As Short
            Get
                Return _idFabricante
            End Get
            Set(ByVal value As Short)
                _idFabricante = value
            End Set
        End Property

        Public ReadOnly Property IdProveedor() As ArrayList
            Get
                If _idProveedor Is Nothing Then _idProveedor = New ArrayList
                Return _idProveedor
            End Get
        End Property

        Public Property Activo() As Enumerados.EstadoBinario
            Get
                Return _activo
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _activo = value
            End Set
        End Property

        Public Property SeparadorProveedor() As String
            Get
                Return _separadorProveedor
            End Get
            Set(ByVal value As String)
                _separadorProveedor = value
            End Set
        End Property

        Public Property Instruccionable() As Enumerados.EstadoBinario
            Get
                Return _instruccionable
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _instruccionable = value
            End Set
        End Property

        Public Property IdClasificacionInterna() As Short
            Get
                Return _idClasificacionInterna
            End Get
            Set(ByVal value As Short)
                _idClasificacionInterna = value
            End Set
        End Property

        Public Property IdClasificacionExterna() As Short
            Get
                Return _idClasificacionExterna
            End Get
            Set(ByVal value As Short)
                _idClasificacionExterna = value
            End Set
        End Property

        Public Property FiltroRapido() As String
            Get
                Return _filtroRapido
            End Get
            Set(ByVal value As String)
                _filtroRapido = value
            End Set
        End Property

        Public Property Cargado() As Boolean
            Get
                Return _cargado
            End Get
            Set(value As Boolean)
                _cargado = value
            End Set
        End Property
#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miProducto As Type = GetType(Producto)
            Dim pInfo As PropertyInfo

            For Each pInfo In miProducto.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Producto)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Producto)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ProductoColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Producto)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idProducto As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), Producto)
                    If .IdProducto = idProducto Then
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
            Dim miProducto As Producto

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miProducto = CType(Me.InnerList(index), Producto)
                If miProducto IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Producto).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miProducto, Nothing)
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
                    With .SqlParametros
                        If Me._idProducto > 0 Then .Add("@idProducto", SqlDbType.Int).Value = Me._idProducto
                        If Me._nombre IsNot Nothing AndAlso Me._nombre.Trim.Length > 0 Then _
                            .Add("@nombre", SqlDbType.VarChar, 100).Value = Me._nombre
                        If Me._codigo IsNot Nothing AndAlso Me._codigo.Trim.Length > 0 Then _
                            .Add("@codigo", SqlDbType.VarChar, 10).Value = Me._codigo
                        If Me._idTecnologia > 0 Then .Add("@idTecnologia", SqlDbType.Int).Value = Me._idTecnologia
                        If Me._idTipoProducto IsNot Nothing AndAlso Me._idTipoProducto.Count > 0 Then _
                            .Add("@listaIdTipoProducto", SqlDbType.VarChar, 100).Value = Join(Me._idTipoProducto.ToArray, ",")
                        If Me._idFabricante > 0 Then .Add("@idFabricante", SqlDbType.SmallInt).Value = Me._idFabricante
                        If Me._idProveedor IsNot Nothing AndAlso Me._idProveedor.Count > 0 Then _
                            .Add("@listaIdProveedor", SqlDbType.VarChar, 100).Value = Join(Me._idProveedor.ToArray, ",")
                        If Me._activo <> Enumerados.EstadoBinario.NoEstablecido Then _
                            .Add("@estado", SqlDbType.Bit).Value = IIf(Me._activo = 1, 1, 0)
                        If Me._separadorProveedor IsNot Nothing AndAlso Me._separadorProveedor.Trim.Length > 0 Then _
                            .Add("@separadorProveedor", SqlDbType.VarChar, 4).Value = Me._separadorProveedor.Trim
                        If Me._instruccionable <> Enumerados.EstadoBinario.NoEstablecido Then _
                            .Add("@instruccionable", SqlDbType.Bit).Value = IIf(Me._instruccionable = 1, 1, 0)
                        If Me._idClasificacionInterna > 0 Then _
                            .Add("@idClasificacionInterna", SqlDbType.SmallInt).Value = Me._idClasificacionInterna
                        If Me._idClasificacionExterna > 0 Then _
                            .Add("@idClasificacionExterna", SqlDbType.SmallInt).Value = Me._idClasificacionExterna
                        If Not String.IsNullOrEmpty(Me._filtroRapido) Then .Add("@filtroRapido", SqlDbType.VarChar).Value = _filtroRapido
                    End With
                    .ejecutarReader("ObtenerInfoProducto", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim elProducto As Producto

                        While .Reader.Read
                            elProducto = New Producto
                            Integer.TryParse(.Reader("idProducto").ToString, elProducto.IdProducto)
                            elProducto.Nombre = .Reader("nombre").ToString
                            elProducto.Codigo = .Reader("codigo").ToString
                            Integer.TryParse(.Reader("idTecnologia").ToString, elProducto.IdTecnologia)
                            elProducto.Tecnologia = .Reader("tecnologia").ToString
                            Short.TryParse(.Reader("idFabricante").ToString, elProducto.IdFabricante)
                            elProducto.Fabricante = .Reader("fabricante").ToString
                            elProducto.Activo = CBool(.Reader("estado").ToString)
                            Short.TryParse(.Reader("idTipoProducto").ToString, elProducto.IdTipoProducto)
                            elProducto.TipoProducto = .Reader("tipoProducto").ToString
                            Integer.TryParse(.Reader("idTipoUnidad").ToString, elProducto.IdTipoUnidad)
                            elProducto.UnidadEmpaque = .Reader("unidadEmpaque").ToString
                            elProducto.EsSim = CBool(.Reader("esSim").ToString)
                            elProducto.AplicaTecnologia = CBool(.Reader("aplicaTecnologia").ToString)
                            elProducto.EsSerializado = CBool(.Reader("esSerializado").ToString)
                            Integer.TryParse(.Reader("idClasificacionInterna").ToString, elProducto.IdClasificacionInterna)
                            Integer.TryParse(.Reader("idClasificacionExterna").ToString, elProducto.IdClasificacionExterna)
                            elProducto.ClasificacionInterna = .Reader("clasificacionInterna").ToString
                            elProducto.ClasificacionExterna = .Reader("clasificacionExterna").ToString
							elProducto.TieneImagen = CBool(.Reader("tieneImagen").ToString)
                            elProducto.Registrado = True

                            Me.InnerList.Add(elProducto)
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
                dtAux = dbManager.ejecutarDataTable("ObtenerInfoProducto", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace
