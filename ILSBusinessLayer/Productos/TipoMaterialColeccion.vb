Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Productos

    Public Class TipoMaterialColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idTipoMaterial As Short
        Private _prefijo As String
        Private _activo As Enumerados.EstadoBinario
        Private _cargado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _prefijo = ""
            _activo = Enumerados.EstadoBinario.Activo
        End Sub

        Public Sub New(ByVal TipoMaterial As String)
            Me.New()
            _prefijo = TipoMaterial
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As TipoMaterial
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As TipoMaterial)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdTipoMaterial() As Short
            Get
                Return _idTipoMaterial
            End Get
            Set(ByVal value As Short)
                _idTipoMaterial = value
            End Set
        End Property

        Public Property Prefijo() As String
            Get
                Return _prefijo
            End Get
            Set(ByVal value As String)
                _prefijo = value
            End Set
        End Property

        Public Property Activo() As Enumerados.EstadoBinario
            Get
                Return _activo
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _activo = value
            End Set
        End Property
#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miTipoMaterial As Type = GetType(TipoMaterial)
            Dim pInfo As PropertyInfo

            For Each pInfo In miTipoMaterial.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoMaterial)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As TipoMaterial)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As TipoMaterialColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As TipoMaterial)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idTipo As Short) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), TipoMaterial)
                    If .IdTipoMaterial = idTipo Then
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
            Dim miTipoMaterial As TipoMaterial

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miTipoMaterial = CType(Me.InnerList(index), TipoMaterial)
                If miTipoMaterial IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(TipoMaterial).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miTipoMaterial, Nothing)
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
                    If Me._idTipoMaterial > 0 Then .SqlParametros.Add("@idTipoMaterial", SqlDbType.SmallInt).Value = Me._idTipoMaterial
                    If Me._prefijo IsNot Nothing AndAlso Me._prefijo.Trim.Length > 0 Then _
                        .SqlParametros.Add("@prefijo", SqlDbType.VarChar, 7).Value = Me._prefijo.Trim
                    If Me._activo <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = Enumerados.EstadoBinario.Activo, 1, 0)
                    .ejecutarReader("ObtenerListadoTiposMaterial", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim elTipoMaterial As TipoMaterial

                        While .Reader.Read
                            elTipoMaterial = New TipoMaterial
                            Short.TryParse(.Reader("idTipoMaterial").ToString, elTipoMaterial.IdTipoMaterial)
                            elTipoMaterial.Prefijo = .Reader("prefijo").ToString
                            Boolean.TryParse(.Reader("estado").ToString, elTipoMaterial.Activo)
                            elTipoMaterial.Registrado = True
                            Me.InnerList.Add(elTipoMaterial)
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
                dtAux = dbManager.ejecutarDataTable("ObtenerListadoTiposMaterial", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace
