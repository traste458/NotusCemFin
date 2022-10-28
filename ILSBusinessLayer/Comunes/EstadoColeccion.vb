Imports ILSBusinessLayer
Imports ILSBusinessLayer.Enumerados
Imports LMDataAccessLayer
Imports System.Reflection

Namespace Comunes

    Public Class EstadoColeccion
        Inherits CollectionBase

#Region "Atributos (Filtros de Búsqueda)"

        Private _idEstado As Integer
        Private _nombre As String
        Private _idEntidad As Integer
        Private _arrListaEstado As ArrayList
        Private _idUsuarioConsulta As Integer
        Private _idDenegacionListaOpcion As Integer
        Private _cargado As Boolean

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _nombre = ""
            _arrListaEstado = New ArrayList
        End Sub

        Public Sub New(ByVal idEstado As Integer)
            Me.New()
            _idEstado = idEstado
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As Estado
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As Estado)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdEstado() As Integer
            Get
                Return _idEstado
            End Get
            Set(ByVal value As Integer)
                _idEstado = value
            End Set
        End Property

        Public Property IdEntidad() As Integer
            Get
                Return _idEntidad
            End Get
            Set(ByVal value As Integer)
                _idEntidad = value
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

        Public Property ListadoEstado() As ArrayList
            Get
                If _arrListaEstado Is Nothing Then _arrListaEstado = New ArrayList
                Return _arrListaEstado
            End Get
            Set(ByVal value As ArrayList)
                _arrListaEstado = value
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
            Dim miEstado As Type = GetType(Estado)
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As Estado)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As Estado)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As Estado)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As Estado)
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
                With CType(Me.InnerList(index), Estado)
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
            Dim miEstado As Estado

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miEstado = CType(Me.InnerList(index), Estado)
                If miEstado IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(Estado).GetProperties
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
                Me.Clear()
                With dbManager
                    If Me._idEstado > 0 Then _
                        .SqlParametros.Add("@idEstado", SqlDbType.Int).Value = Me._idEstado
                    If Me._idEntidad > 0 Then _
                        .SqlParametros.Add("@idEntidad", SqlDbType.Int).Value = Me._idEntidad
                    If Me._arrListaEstado IsNot Nothing AndAlso Me._arrListaEstado.Count > 0 Then _
                        .SqlParametros.Add("@listaEstados", SqlDbType.VarChar, 1000).Value = Join(Me._arrListaEstado.ToArray, ",")
                    If Me._idUsuarioConsulta > 0 Then _
                        .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = Me._idUsuarioConsulta
                    If Me._idDenegacionListaOpcion > 0 Then _
                        .SqlParametros.Add("@idListaDenegada", SqlDbType.Int).Value = Me._idDenegacionListaOpcion

                    .ejecutarReader("SeleccionarEstados", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        Dim oEstado As Estado

                        While .Reader.Read
                            oEstado = New Estado
                            Short.TryParse(.Reader("idEstado").ToString, oEstado.IdEstado)
                            oEstado.Descripcion = .Reader("nombre").ToString
                            Short.TryParse(.Reader("idEntidad").ToString, oEstado.IdEntidad)
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
                dtAux = dbManager.ejecutarDataTable("SeleccionarEstados", CommandType.StoredProcedure)
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try

            Return dtAux
        End Function

#End Region

    End Class

End Namespace
