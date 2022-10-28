Imports LMDataAccessLayer
Imports System.Reflection

Namespace PermisoOpcion

    Public Class ListaOpcionFuncionalDenegada
        Inherits CollectionBase


#Region "Atributos (Filtros de Búsqueda)"

        Private _idOpcion As Integer
        Private _arrListaOpcionFuncional As ArrayList
        Private _idUsuario As Integer
        Private _cargado As Boolean
        Private _OpcionFuncional As OpcionFuncionalDenegada

#End Region

#Region "Constructores"

        Public Sub New()
            MyBase.New()
            _arrListaOpcionFuncional = New ArrayList
        End Sub

        Public Sub New(ByVal idOpcion As Integer)
            Me.New()
            _idOpcion = idOpcion
            CargarDatos()
        End Sub

#End Region

#Region "Propiedades"

        Default Public Property Item(ByVal index As Integer) As OpcionFuncionalDenegada
            Get
                Return Me.InnerList.Item(index)
            End Get
            Set(ByVal value As OpcionFuncionalDenegada)
                If value IsNot Nothing Then
                    Me.InnerList.Item(index) = value
                Else
                    Throw New Exception("No se puede asignar un objeto nulo o no registrado a la colección.")
                End If
            End Set
        End Property

        Public Property IdOpcion() As Integer
            Get
                Return _idOpcion
            End Get
            Set(ByVal value As Integer)
                _idOpcion = value
            End Set
        End Property


        Public Property ListadoOpcionFuncional() As ArrayList
            Get
                If _arrListaOpcionFuncional Is Nothing Then _arrListaOpcionFuncional = New ArrayList
                Return _arrListaOpcionFuncional
            End Get
            Set(ByVal value As ArrayList)
                _arrListaOpcionFuncional = value
            End Set
        End Property

        Public Property IdUsuario() As Integer
            Get
                Return _idUsuario
            End Get
            Set(ByVal value As Integer)
                _idUsuario = value
            End Set
        End Property

#End Region

#Region "Métodos Privados"

        Private Function CrearEstructuraDeTabla() As DataTable
            Dim dtAux As New DataTable
            Dim miOpcionFuncional As Type = GetType(OpcionFuncionalDenegada)
            Dim pInfo As PropertyInfo

            For Each pInfo In miOpcionFuncional.GetProperties
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

        Public Sub Insertar(ByVal posicion As Integer, ByVal valor As OpcionFuncionalDenegada)
            Me.InnerList.Insert(posicion, valor)
        End Sub

        Public Sub Adicionar(ByVal valor As OpcionFuncionalDenegada)
            Me.InnerList.Add(valor)
        End Sub

        Public Sub AdicionarRango(ByVal rango As ListaOpcionFuncionalDenegada)
            Me.InnerList.AddRange(rango)
        End Sub

        Public Sub Remover(ByVal valor As OpcionFuncionalDenegada)
            With Me.InnerList
                If .Contains(valor) Then .Remove(valor)
            End With
        End Sub

        Public Sub RemoverDe(ByVal index As Integer)
            Me.InnerList.RemoveAt(index)
        End Sub

        Public Function IndiceDe(ByVal idOpcionFuncional As Integer) As Integer
            Dim indice As Integer = -1
            For index As Integer = 0 To Me.InnerList.Count - 1
                With CType(Me.InnerList(index), OpcionFuncionalDenegada)
                    If .IdOpcion = idOpcionFuncional Then
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
            Dim miOpcionFuncional As OpcionFuncionalDenegada

            For index As Integer = 0 To Me.InnerList.Count - 1
                drAux = dtAux.NewRow
                miOpcionFuncional = CType(Me.InnerList(index), OpcionFuncionalDenegada)
                If miOpcionFuncional IsNot Nothing Then
                    For Each pInfo As PropertyInfo In GetType(OpcionFuncionalDenegada).GetProperties
                        If pInfo.PropertyType.Namespace = "System" Then
                            drAux(pInfo.Name) = pInfo.GetValue(miOpcionFuncional, Nothing)
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
                    EstablecerFiltros(dbManager)
                    .ejecutarReader("ObtenerOpcionFuncionalDenegada", CommandType.StoredProcedure)
                    If .Reader IsNot Nothing Then
                        While .Reader.Read
                            _OpcionFuncional = New OpcionFuncionalDenegada
                            _OpcionFuncional.IdOpcion = .Reader("idOpcionFuncional")
                            _OpcionFuncional.NombreOpcion = .Reader("nombre")
                            _OpcionFuncional.Activo = .Reader("activo")
                            _OpcionFuncional.IdDenegacion = .Reader("idDenegarOpcion")
                            _OpcionFuncional.IdPerfil = .Reader("idPerfil")
                            _OpcionFuncional.Perfil = .Reader("perfil")
                            _OpcionFuncional.Registrado = True
                            Me.InnerList.Add(_OpcionFuncional)
                        End While
                        .Reader.Close()
                    End If
                End With
                _cargado = True
            Finally
                If dbManager IsNot Nothing Then dbManager.Dispose()
            End Try
        End Sub

        Public Sub EstablecerFiltros(ByRef db As LMDataAccess)
            With db
                If Me._idUsuario > 0 Then _
                    .SqlParametros.Add("@idUsuarioConsulta", SqlDbType.Int).Value = Me._idUsuario
                If Me._idOpcion > 0 Then _
                          .SqlParametros.Add("@idOpcionFuncional", SqlDbType.Int).Value = Me._idOpcion
                If Me._arrListaOpcionFuncional IsNot Nothing AndAlso Me._arrListaOpcionFuncional.Count > 0 Then _
                    .SqlParametros.Add("@listaOpcionFuncional", SqlDbType.VarChar, 1000).Value = Join(Me._arrListaOpcionFuncional.ToArray, ",")
            End With
        End Sub

#End Region

#Region "Métodos Compartidos"

        Public Shared Function ObtenerTodosEnDataTable() As DataTable
            Dim dtAux As New DataTable
            Dim laOpcionFuncional As New OpcionFuncionalDenegada
            Try
                dtAux = laOpcionFuncional.ObtenerListado()
            Finally
                If laOpcionFuncional IsNot Nothing Then laOpcionFuncional = Nothing
            End Try
            Return dtAux
        End Function
#End Region


    End Class
End Namespace
