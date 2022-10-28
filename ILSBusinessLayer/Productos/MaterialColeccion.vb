Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Productos

    Public Class MaterialColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _material As String
        Private _referencia As String
        Private _referenciaCliente As String
        Private _idProductoPadre As Integer
        Private _idTipoProducto As Short
        Private _idTecnologia As Short
        Private _idTipoOrden As Short
        Private _asignarMin As Enumerados.EstadoBinario
        Private _leerSim As Enumerados.EstadoBinario
        Private _codigoEan As String
        Private _esSim As Enumerados.EstadoBinario
        Private _idEstado As Short
        Private _tipoMaterial As String
        Private _esSerializado As Enumerados.EstadoBinario
        Private _cargado As Boolean
        Private _idOrdenCompra As Long
        Private _idProveedor As Integer
        Private _idFabricante As Integer
        Private _principal As Boolean
        Private _filtroRapido As String
        Private _color As String
        Private _existePrincipal As Integer = -1
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _material = ""
            _referencia = ""
            _codigoEan = ""
            _asignarMin = Enumerados.EstadoBinario.NoEstablecido
            _leerSim = Enumerados.EstadoBinario.NoEstablecido
            _esSim = Enumerados.EstadoBinario.NoEstablecido
            _tipoMaterial = ""
            _esSerializado = Enumerados.EstadoBinario.NoEstablecido
            _idEstado = 1
        End Sub

        Public Sub New(ByVal material As String)
            Me.New()
            _material = material
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As Material
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As Material)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property Material() As String
            Get
                Return _material
            End Get
            Set(ByVal value As String)
                _material = value
            End Set
        End Property

        Public Property Referencia() As String
            Get
                Return _referencia
            End Get
            Set(ByVal value As String)
                _referencia = value
            End Set
        End Property

        Public Property ReferenciaSegunCliente() As String
            Get
                Return _referenciaCliente
            End Get
            Set(ByVal value As String)
                _referenciaCliente = value
            End Set
        End Property

        Public Property IdProductoPadre() As Integer
            Get
                Return _idProductoPadre
            End Get
            Set(ByVal value As Integer)
                _idProductoPadre = value
            End Set
        End Property

        Public Property IdTipoProducto() As Short
            Get
                Return _idTipoProducto
            End Get
            Set(ByVal value As Short)
                _idTipoProducto = value
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

        Public Property IdTipoOrden() As Short
            Get
                Return _idTipoOrden
            End Get
            Set(ByVal value As Short)
                _idTipoOrden = value
            End Set
        End Property

        Public Property AsignarMin() As Enumerados.EstadoBinario
            Get
                Return _asignarMin
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _asignarMin = value
            End Set
        End Property

        Public Property LeerSim() As Enumerados.EstadoBinario
            Get
                Return _leerSim
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _leerSim = value
            End Set
        End Property

        Public Property CodigoEan() As String
            Get
                Return _codigoEan
            End Get
            Set(ByVal value As String)
                _codigoEan = value
            End Set
        End Property

        Public Property EsSim() As Enumerados.EstadoBinario
            Get
                Return _esSim
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _esSim = value
            End Set
        End Property

        Public Property IdEstado() As Short
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Short)
                _idEstado = value
            End Set
        End Property

        Public Property TipoMaterial() As String
            Get
                Return _tipoMaterial
            End Get
            Set(ByVal value As String)
                _tipoMaterial = value
            End Set
        End Property

        Public Property EsSerializado() As Enumerados.EstadoBinario
            Get
                Return _esSerializado
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _esSerializado = value
            End Set
        End Property

        Public Property IdOrdenCompra() As Long
            Get
                Return _idOrdenCompra
            End Get
            Set(ByVal value As Long)
                _idOrdenCompra = value
            End Set
        End Property

        Public Property IdProveedor() As Integer
            Get
                Return _idProveedor
            End Get
            Set(ByVal value As Integer)
                _idProveedor = value
            End Set
        End Property

        Public Property IdFabricante() As Integer
            Get
                Return _idFabricante
            End Get
            Set(ByVal value As Integer)
                _idFabricante = value
            End Set
        End Property

        Public Property Principal() As Boolean
            Get
                Return _principal
            End Get
            Set(value As Boolean)
                _principal = value
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

        Public Property Color() As String
            Get
                Return _color
            End Get
            Set(value As String)
                _color = value
            End Set
        End Property

        Public Property ExistePrincipal() As Integer
            Get
                Return _existePrincipal
            End Get
            Set(value As Integer)
                _existePrincipal = value
            End Set
        End Property
#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miMaterial As Type = GetType(Material)
            Dim pInfo As PropertyInfo

            For Each pInfo In miMaterial.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Material)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Material)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As MaterialColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Material)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal material As String) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), Material)
                    If .Material = material Then
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
            Dim miMaterial As Material

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miMaterial = CType(Me.InnerList(index), Material)
                If miMaterial IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Material).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miMaterial, Nothing)
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
                    If Not String.IsNullOrEmpty(Me._material) Then _
                        .SqlParametros.Add("@material", SqlDbType.VarChar, 20).Value = Me._material.Trim
                    If Not String.IsNullOrEmpty(Me._referencia) Then _
                        .SqlParametros.Add("@referencia", SqlDbType.VarChar, 250).Value = Me._referencia.Trim
                    If Not String.IsNullOrEmpty(_referenciaCliente) Then _
                        .SqlParametros.Add("@referenciaCliente", SqlDbType.VarChar, 250).Value = Me._referenciaCliente.Trim
                    If Me._idProductoPadre > 0 Then .SqlParametros.Add("@idProducto", SqlDbType.Int).Value = Me._idProductoPadre
                    If Me._idTipoProducto > 0 Then .SqlParametros.Add("@idTipoProducto", SqlDbType.SmallInt).Value = Me._idTipoProducto
                    If Me._idTecnologia > 0 Then .SqlParametros.Add("@idTecnologia", SqlDbType.SmallInt).Value = Me._idTecnologia
                    If Me._idTipoOrden > 0 Then .SqlParametros.Add("@idTipoOrden", SqlDbType.SmallInt).Value = Me._idTipoOrden
                    If Me._asignarMin <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@asignarMin", SqlDbType.Bit).Value = IIf(Me._asignarMin = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Me._leerSim <> Enumerados.EstadoBinario.NoEstablecido Then _
                            .SqlParametros.Add("@leerSim", SqlDbType.Bit).Value = IIf(Me._leerSim = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Me._esSim <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@esSim", SqlDbType.Bit).Value = IIf(Me._esSim = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Me._idEstado > 0 Then .SqlParametros.Add("@idEstado", SqlDbType.TinyInt).Value = Me._idEstado
                    If Me._tipoMaterial IsNot Nothing AndAlso Me._tipoMaterial.Trim.Length > 0 Then _
                        .SqlParametros.Add("@tipoMaterial", SqlDbType.VarChar, 10).Value = Me._tipoMaterial.Trim
                    If Me._esSerializado <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@esSerializado", SqlDbType.Bit).Value = IIf(Me._esSerializado = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Me._idOrdenCompra > 0 Then .SqlParametros.Add("@idOrdenCompra", SqlDbType.BigInt).Value = Me._idOrdenCompra
                    If Me._idProveedor > 0 Then .SqlParametros.Add("@idProveedor", SqlDbType.Int).Value = Me._idProveedor
                    If Me._idFabricante > 0 Then .SqlParametros.Add("@idFabricante", SqlDbType.Int).Value = Me._idFabricante
					If Not String.IsNullOrEmpty(Me._color) Then .SqlParametros.Add("@color", SqlDbType.VarChar, 50).Value = Me._color.Trim
                    If Not String.IsNullOrEmpty(Me._filtroRapido) Then .SqlParametros.Add("@filtroRapido", SqlDbType.VarChar).Value = _filtroRapido
                    If Me._principal <> Enumerados.EstadoBinario.NoEstablecido Then .SqlParametros.Add("@principal", SqlDbType.Bit).Value = Me._principal
                    .ejecutarReader("ConsultarListadoMateriales", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elMaterial As Material

                        While .Reader.Read
                            elMaterial = New Material
                            elMaterial.CodigoOriginal = .Reader("material").ToString
                            elMaterial.Material = .Reader("material").ToString
                            elMaterial.Referencia = .Reader("referencia").ToString
                            elMaterial.ReferenciaCliente = .Reader("referenciaCliente").ToString
                            Integer.TryParse(.Reader("cantidadEmpaque").ToString, elMaterial.CantidadEmpaque)
                            Integer.TryParse(.Reader("idProductoPadre").ToString, elMaterial.IdProductoPadre)
                            elMaterial.ProductoPadre = .Reader("productoPadre").ToString
                            Short.TryParse(.Reader("idTecnologia").ToString, elMaterial.IdTecnologia)
                            elMaterial.Tecnologia = .Reader("tecnologia").ToString
                            Short.TryParse(.Reader("idTipoOrden").ToString, elMaterial.IdTipoOrden)
                            elMaterial.TipoOrden = .Reader("tipoOrden").ToString
                            elMaterial.CodigoEan = .Reader("codigoEan").ToString
                            elMaterial.EsSim = CBool(.Reader("esSim").ToString)
                            elMaterial.LeerSim = CBool(.Reader("leerSim").ToString)
                            Short.TryParse(.Reader("idEstado").ToString, elMaterial.IdEstado)
                            elMaterial.Estado = .Reader("estado").ToString
                            elMaterial.TipoMaterial = .Reader("tipoMaterial").ToString
                            Short.TryParse(.Reader("idTipoProducto").ToString, elMaterial.IdTipoProducto)
                            elMaterial.TipoProducto = .Reader("tipoProducto").ToString
                            elMaterial.UnidadEmpaque = .Reader("unidadEmpaque").ToString
                            elMaterial.EsSerializado = CBool(.Reader("esSerializado").ToString)
                            elMaterial.ListaRegiones = .Reader("listadoRegiones").ToString
                            elMaterial.AsignarMin = CBool(.Reader("asignarMin").ToString)
                            Short.TryParse(.Reader("idTipoEtiqueta").ToString, elMaterial.IdTipoEtiqueta)
                            elMaterial.TipoEtiqueta = .Reader("tipoEtiqueta").ToString
							elMaterial.Color = .Reader("color").ToString
                            elMaterial.Registrado = True
                            Me.InnerList.Add(elMaterial)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub ValidarPrincipal()
            Dim dbManager As New LMDataAccess
            Try
                With dbManager
                    .SqlParametros.Clear()
                    .SqlParametros.Add("@idProductoPadre", SqlDbType.VarChar, 20).Value = _idProductoPadre
                    If _material <> "" Then .SqlParametros.Add("@idSubproducto2", SqlDbType.VarChar, 30).Value = _material
                    _existePrincipal = .ejecutarScalar("ValidarMaterialPrincipal", CommandType.StoredProcedure)
                End With
            Catch ex As Exception
                If dbManager IsNot Nothing AndAlso dbManager.estadoTransaccional Then dbManager.abortarTransaccion()
                Throw New Exception(ex.Message, ex)
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
                dtAux = dbManager.ejecutarDataTable("ConsultarListadoMateriales", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace