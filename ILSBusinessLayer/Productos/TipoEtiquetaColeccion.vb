Imports ILSBusinessLayer
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Productos

    Public Class TipoEtiquetaColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idTipoEtiqueta As Short
        Private _descripcion As String
        Private _porDefecto As Enumerados.EstadoBinario
        Private _activo As Enumerados.EstadoBinario
        Private _cargado As Boolean
#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _descripcion = ""
            _activo = Enumerados.EstadoBinario.Activo
            _porDefecto = Enumerados.EstadoBinario.NoEstablecido
        End Sub

        Public Sub New(ByVal idTipoEtiqueta As Short)
            Me.New()
            _idTipoEtiqueta = idTipoEtiqueta
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As TipoEtiqueta
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As TipoEtiqueta)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdTipoEtiqueta() As Short
            Get
                Return _idTipoEtiqueta
            End Get
            Set(ByVal value As Short)
                _idTipoEtiqueta = value
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

        Public Property PorDefecto() As Enumerados.EstadoBinario
            Get
                Return _porDefecto
            End Get
            Set(ByVal value As Enumerados.EstadoBinario)
                _porDefecto = value
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
            Dim miTipoEtiqueta As Type = GetType(TipoEtiqueta)
            Dim pInfo As PropertyInfo

            For Each pInfo In miTipoEtiqueta.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As TipoEtiqueta)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As TipoEtiqueta)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As TipoEtiquetaColeccion)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As TipoEtiqueta)
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
                With CType(Me.InnerList(index), TipoEtiqueta)
                    If .IdTipoEtiqueta = idTipo Then
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
            Dim miTipoEtiqueta As TipoEtiqueta

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miTipoEtiqueta = CType(Me.InnerList(index), TipoEtiqueta)
                If miTipoEtiqueta IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(TipoEtiqueta).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miTipoEtiqueta, Nothing)
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
                    If Me._idTipoEtiqueta > 0 Then .SqlParametros.Add("@idTipoEtiqueta", SqlDbType.SmallInt).Value = Me._idTipoEtiqueta
                    If Not String.IsNullOrEmpty(Me._descripcion) Then _
                        .SqlParametros.Add("@descripcion", SqlDbType.VarChar, 7).Value = Me._descripcion.Trim
                    If Me._porDefecto <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@porDefecto", SqlDbType.Bit).Value = IIf(Me._porDefecto = Enumerados.EstadoBinario.Activo, 1, 0)
                    If Me._activo <> Enumerados.EstadoBinario.NoEstablecido Then _
                        .SqlParametros.Add("@activo", SqlDbType.Bit).Value = IIf(Me._activo = Enumerados.EstadoBinario.Activo, 1, 0)

                    .ejecutarReader("ObtenerListadoTiposEtiqueta", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim miTipoEtiqueta As TipoEtiqueta

                        While .Reader.Read
                            miTipoEtiqueta = New TipoEtiqueta
                            Short.TryParse(.Reader("idTipoEtiqueta").ToString, miTipoEtiqueta.IdTipoEtiqueta)
                            miTipoEtiqueta.Descripcion = .Reader("descripcion").ToString
                            miTipoEtiqueta.PorDefecto = CBool(.Reader("porDefecto"))
                            miTipoEtiqueta.Activo = CBool(.Reader("activo"))
                            miTipoEtiqueta.Registrado = True
                            Me.InnerList.Add(miTipoEtiqueta)
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
                dtAux = dbManager.ejecutarDataTable("ObtenerListadoTiposEtiqueta", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace
