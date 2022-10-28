Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection
Imports ILSBusinessLayer.Enumerados

Namespace Productos

    Public Class TipoProductoColeccion
        Inherits CollectionBase


#Region "Atributos (Filtros de Búsqueda)"

        Private _idTipoProducto As Short
        Private _descripcion As String
        Private _instruccionable As EstadoBinario
        Private _activo As EstadoBinario
        Private _existeModulo As EstadoBinario
        Private _idModulo As Integer
        Private _tipoAplicativo As Short
        Private _listaNoCargar As ArrayList
        Private _pesado As EstadoBinario
        Private _esSerializado As EstadoBinario
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _instruccionable = EstadoBinario.NoEstablecido
            _activo = EstadoBinario.NoEstablecido
            _existeModulo = EstadoBinario.NoEstablecido
            _pesado = EstadoBinario.NoEstablecido
            _esSerializado = EstadoBinario.NoEstablecido
        End Sub

        Public Sub New(ByVal serializado As EstadoBinario)
            Me.New()
            _esSerializado = serializado
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As TipoProducto
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As TipoProducto)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdTipoProducto As Short
            Get
                Return _idTipoProducto
            End Get
            Set(value As Short)
                _idTipoProducto = value
            End Set
        End Property

        Public Property Descripcion As String
            Get
                Return _descripcion
            End Get
            Set(value As String)
                _descripcion = value
            End Set
        End Property

        Public Property Instruccionable As EstadoBinario
            Get
                Return _instruccionable
            End Get
            Set(value As EstadoBinario)
                _instruccionable = value
            End Set
        End Property

        Public Property Activo As EstadoBinario
            Get
                Return _activo
            End Get
            Set(value As EstadoBinario)
                _activo = value
            End Set
        End Property

        Public Property ExisteModulo As EstadoBinario
            Get
                Return _existeModulo
            End Get
            Set(value As EstadoBinario)
                _existeModulo = value
            End Set
        End Property

        Public Property IdModulo As Integer
            Get
                Return _idModulo
            End Get
            Set(value As Integer)
                _idModulo = value
            End Set
        End Property

        Public Property TipoAplicativo As Short
            Get
                Return _tipoAplicativo
            End Get
            Set(value As Short)
                _tipoAplicativo = value
            End Set
        End Property

        Public ReadOnly Property ListaNoCargar As ArrayList
            Get
                Return _listaNoCargar
            End Get
        End Property

        Public Property Pesado As EstadoBinario
            Get
                Return _pesado
            End Get
            Set(value As EstadoBinario)
                _pesado = value
            End Set
        End Property

        Public Property EsSerializado As EstadoBinario
            Get
                Return _esSerializado
            End Get
            Set(value As EstadoBinario)
                _esSerializado = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim obj As Type = GetType(TipoProducto)
            Dim pInfo As PropertyInfo

            For Each pInfo In obj.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoProducto)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As TipoProducto)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As DetalleMsisdnEnServicioMensajeriaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As TipoProducto)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal identificador As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), TipoProducto)
                    If .IdTipoProducto = identificador Then
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
            Dim obj As TipoProducto

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                obj = CType(Me.InnerList(index), TipoProducto)
                If obj IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(TipoProducto).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(obj, Nothing)
                        End If
                    Next
                    dtAux.Rows.Add(drAux)
                End If
            Next

            Return dtAux
        End Function

        Public Sub CargarDatos()
            Using dbManager As New LMDataAccess
                Me.Clear()
                With dbManager
                    If Me._idTipoProducto > 0 Then _
                        .SqlParametros.Add("@idTipoProducto", SqlDbType.BigInt).Value = Me._idTipoProducto
                    If Me._activo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@estado", SqlDbType.Int).Value = IIf(Me._activo = 1, 1, 0)
                    If Me._instruccionable <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@instruccionable", SqlDbType.Int).Value = IIf(Me._instruccionable = 1, 1, 0)
                    If Not EsNuloOVacio(Me._descripcion) Then _
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar).Value = Me._descripcion
                    If Me._existeModulo <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@existeModulo", SqlDbType.Int).Value = IIf(Me._existeModulo = 1, 1, 0)
                    If Me._idModulo > 0 Then .SqlParametros.Add("@idModulo", SqlDbType.Int).Value = Me._idModulo
                    If Me._tipoAplicativo > 0 Then .SqlParametros.Add("@tipoAplicativo", SqlDbType.SmallInt).Value = Me._tipoAplicativo
                    If Me._listaNoCargar IsNot Nothing AndAlso Me._listaNoCargar.Count Then _
                        .SqlParametros.Add("@listaNoCargar", SqlDbType.VarChar).Value = Join(Me._listaNoCargar.ToArray, ",")
                    If Me._pesado <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@pesado", SqlDbType.Bit).Value = IIf(Me._pesado = 1, 1, 0)
                    If Me._esSerializado <> EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@serializado", SqlDbType.Int).Value = IIf(Me._esSerializado = 1, 1, 0)

                    .ejecutarReader("ObtenerListadoTipoProducto", CommandType.StoredProcedure)

                    If .Reader IsNot Nothing Then
                        Dim obj As TipoProducto
                        While .Reader.Read
                            obj = New TipoProducto
                            obj.CargarResultadoConsulta(.Reader)
                            Me.InnerList.Add(obj)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            End Using
        End Sub
#End Region
    End Class

End Namespace