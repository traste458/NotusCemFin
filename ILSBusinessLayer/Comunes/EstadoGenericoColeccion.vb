Imports LMDataAccessLayer
Imports System.Reflection

Namespace Comunes

    Public Class EstadoGenericoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idEntidad As Short
        Private _idEstado As Short
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal idEntidad As Short)
            Me.New()
            _idEntidad = idEntidad
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As EstadoGenerico
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As EstadoGenerico)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
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

        Public Property IdEntidad() As Short
            Get
                Return _idEntidad
            End Get
            Set(ByVal value As Short)
                _idEntidad = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miEstado As Type = GetType(EstadoGenerico)
            Dim pInfo As PropertyInfo

            For Each pInfo In miEstado.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As EstadoGenerico)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As EstadoGenerico)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As EstadoGenerico)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As EstadoGenerico)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idEstado As Short) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), EstadoGenerico)
                    If .IdEstado = idEstado Then
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
            Dim miEstado As EstadoGenerico

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miEstado = CType(Me.InnerList(index), EstadoGenerico)
                If miEstado IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(EstadoGenerico).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miEstado, Nothing)
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
                Me.InnerList.Clear()
                With dbManager
                    .SqlParametros.Add("@idEntidad", SqlDbType.SmallInt).Value = Me._idEntidad
                    If Me._idEstado > 0 Then _
                        .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = Me._idEstado
                    .ejecutarReader("ObtenerEstadosGenericos", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim oEstado As EstadoGenerico
                        While .Reader.Read
                            oEstado = New EstadoGenerico
                            Short.TryParse(.Reader("idEstado").ToString, oEstado.IdEstado)
                            oEstado.Descripcion = .Reader("descripcion").ToString
                            Short.TryParse(.Reader("idEntidad").ToString, oEstado.IdEntidad)
                            oEstado.Entidad = .Reader("entidad").ToString
                            oEstado.Registrado = True
                            Me.InnerList.Add(oEstado)
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
                dtAux = dbManager.ejecutarDataTable("ObtenerEstadosGenericos", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace